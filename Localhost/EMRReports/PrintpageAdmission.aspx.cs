using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class EMRReports_PrintpageAdmission : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.ParseData bc = new BaseC.ParseData();
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.ATD objadt;
    //DAL.DAL dl;
    DataSet ds;
    private bool Stauts = false;
    //added by bhakti    
    private static string IsWardStation = "N";
    private static int ChargeableforAttendantPassServiceId = 0;
    private static string IsChargeableforAttendantPass = "N";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["MASTER"]) == "NO")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            // ClearApplicationCache();
            FillEntrySite();
            BindMenu();
            chkIsAdmitted.Visible = false;
            dtpfromdate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpfromdate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpfromdate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " 00:00");

            dtpTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString() + " HH:mm";
            dtpTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])) + " HH:mm");
            lbtnSearchPatient.Text = "IP No.";
            lbtnSearchPatientvoice.Text = "Invoice";
            //Add EMR Screen 
            txtIpno.Text = common.myStr(Request.QueryString["Encno"]);
            txtUHID.Text = common.myStr(Request.QueryString["Regno"]);

            //Add EMR Screen 

            //added by bhakti
            getHospitalFlagValue();
            if (IsWardStation.ToUpper().Equals("Y") && common.myStr(Request.QueryString["RN"]) == "Admis")
            {
                foreach (RadComboBoxItem items in ddlReportType.Items)
                {
                    if (items.Value == "WardStation")
                    {
                        RadComboBoxItem item = (RadComboBoxItem)ddlReportType.FindItemByValue("WardStation");
                        items.Visible = true;
                    }

                }
            }
            if (common.myStr(Request.QueryString["RN"]) == "Admform")
            {
                lblHeader.Text = "Admission Form";
                // Form 60 Report for Balaji-- by Tripti
                //string IsRequiredForm60Process = common.GetFlagValueHospitalSetup(Convert.ToInt32(Session["HospitalLocationID"].ToString()), Convert.ToInt32(Session["FacilityId"].ToString()), "IsRequireForm60Process", sConString);
                //if (IsRequiredForm60Process == "N")
                //{
                //   // rdolabelform.Items[8].Attributes.CssStyle.Add("visibility", "hidden");
                //}
                ////Tripti

                rdolabelform.Visible = true;
                tradmfrm.Visible = true;
                trdate.Visible = false;
                trddl.Visible = false;
                trgrid.Visible = false;
                trloction.Visible = false;

            }
            if (common.myStr(Request.QueryString["RN"]) == "ReAdmform" || common.myStr(Request.QueryString["RN"]) == "ReAdmformWithDay")
            {

                dtFromDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtFromDate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtFromDate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));

                dtTodate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtTodate.DateInput.DisplayDateFormat = Application["OutputDateFormat"].ToString();
                dtTodate.SelectedDate = common.myDate(DateTime.Now.ToString(common.myStr(Application["OutputDateFormat"])));


                lblHeader.Text = "Re-Admitted Patient List";
                lbtnSearchPatient.Visible = false;
                txtIpno.Visible = false;
                lblfrmdate.Visible = true;
                lblToDate.Visible = true;
                dtFromDate.Visible = true;
                dtTodate.Visible = true;
                dtFromDate.Visible = true;
                lbtnSearchPatient.Enabled = false;
                rdolabelform.Visible = false;
                tradmfrm.Visible = true;
                trdate.Visible = false;
                trddl.Visible = false;
                trgrid.Visible = false;
                trloction.Visible = false;
                if (common.myStr(Request.QueryString["RN"]) == "ReAdmformWithDay")
                {
                    txtIpno.Visible = true;
                    lbtnSearchPatient.Visible = true;
                    lbtnSearchPatient.Enabled = false;
                    lbtnSearchPatient.Text = "Days Limit : ";
                }
            }
            else if (common.myStr(Request.QueryString["RN"]) == "PTD")
            {
                rdoList.Visible = false;
                lbtnSearchPatient.Text = "UHID";
                trddl.Visible = false;
                trgrid.Visible = false;
                trl.Visible = false;
                btnPrintreport.Visible = false;
                trloction.Visible = false;
            }
            else if (common.myStr(Request.QueryString["RN"]) == "DischSum")
            {
                lblHeader.Text = "Discharge Summary";

                rdolabelform.Visible = false;

                tradmfrm.Visible = true;
                trdate.Visible = false;
                trddl.Visible = false;
                trgrid.Visible = false;
                trloction.Visible = false;
            }
            else if (common.myStr(Request.QueryString["RN"]) == "Admitted")
            {
                lblHeader.Text = "Admitted List";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;

                Lab1.Visible = true;
                ddlReportType.Visible = true;
                trloction.Visible = true;

            }
            else if (common.myStr(Request.QueryString["RN"]) == "Admis")
            {
                lblHeader.Text = "Admission List";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;

                Lab1.Visible = true;
                ddlReportType.Visible = true;
                ChkSummary.Visible = true;
                trloction.Visible = true;
                foreach (RadComboBoxItem items in ddlReportType.Items)
                {
                    if (items.Text == "Discharge Type Wise")
                    {
                        {
                            RadComboBoxItem item = (RadComboBoxItem)ddlReportType.FindItemByText("Discharge Type Wise");
                            items.Visible = false;
                        }
                    }
                }
            }
            else if (common.myStr(Request.QueryString["RN"]) == "RegArea")
            {
                lblHeader.Text = "Registration List Area Wise";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;
                ChkSummary.Visible = false;
                Lab1.Visible = false;
                lblReportType.Visible = false;
                ddlReportType.Visible = false;
                //ChkSummary.Visible = true;
                trloction.Visible = true;
            }
            else if (common.myStr(Request.QueryString["RN"]) == "AdmER")
            {
                lblHeader.Text = "ER Admission List";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;

                Lab1.Visible = false;
                ddlReportType.Visible = false;
                ChkSummary.Visible = false;
                chkExport.Visible = false;
                trloction.Visible = true;

            }
            else if (common.myStr(Request.QueryString["RN"]) == "Disc")
            {
                lblHeader.Text = "Discharge List";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;

                Lab1.Visible = true;
                ddlReportType.Visible = true;
                trloction.Visible = true;
            }
            else if (common.myStr(Request.QueryString["RN"]) == "DischWOBill")
            {
                lblHeader.Text = "Discharge List Without Bill";
                trdate.Visible = true;
                trddl.Visible = false;
                trgrid.Visible = false;
                tradmfrm.Visible = false;

                Lab1.Visible = true;
                ddlReportType.Visible = true;
                trloction.Visible = true;
            }
            else if (common.myStr(Request.QueryString["RN"]) == "Admitason")
            {
                lblHeader.Text = "Admitted as on";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;
                ChkSummary.Visible = true;
                dtpfromdate.Visible = false;
                Lab1.Visible = true;
                ddlReportType.Visible = true;
                lbltdfrmdate.Visible = false;
                dttdfrmdate.Visible = false;
                Label3.Text = "As on date";
                Label3.Width = 300;
                dtpTodate.Enabled = true;
                trloction.Visible = true;
                foreach (RadComboBoxItem items in ddlReportType.Items)
                {
                    if (items.Text == "Discharge Type Wise")
                    {
                        {
                            RadComboBoxItem item = (RadComboBoxItem)ddlReportType.FindItemByText("Discharge Type Wise");
                            items.Visible = false;
                        }
                    }
                }
            }
            else if (common.myStr(Request.QueryString["RN"]) == "Trans")
            {
                lblHeader.Text = "Patient Transfer";

                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;
                dtpfromdate.Visible = true;
                trl.Visible = true;
                tdlblrtype.Visible = false;
                tdddlrtype.Visible = false;
                Lab1.Visible = false;
                ddlReportType.Visible = false;
                trloction.Visible = true;

                //   BindProviderList();


            }
            else if (common.myStr(Request.QueryString["RN"]) == "updownbdCatg")
            {
                lblHeader.Text = "Patient Upgrade Downgrade";

                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;
                dtpfromdate.Visible = true;
                chkIsAdmitted.Visible = true;
                tdlblrtype.Visible = false;
                tdddlrtype.Visible = false;
                Lab1.Visible = false;
                ddlReportType.Visible = false;
                trloction.Visible = true;

                //   BindProviderList();


            }
            else if (common.myStr(Request.QueryString["RN"]) == "DiscER")
            {
                lblHeader.Text = "ER Discharge List";
                trdate.Visible = true;
                trddl.Visible = true;
                trgrid.Visible = false;
                tradmfrm.Visible = false;

                Lab1.Visible = false;
                ddlReportType.Visible = false;
                ChkSummary.Visible = false;
                chkExport.Visible = false;
                trloction.Visible = true;

            }
            else if (common.myStr(Request.QueryString["RN"]) == "BillPrint")
            {
                lblHeader.Text = "IP Bill Detail";
                rdolabelform.Visible = false;
                rdolabelformipbilldetail.Visible = true;
                tradmfrm.Visible = true;
                trdate.Visible = false;
                trddl.Visible = false;
                trgrid.Visible = false;
                trloction.Visible = false;
                rdoIpbillType.Visible = true;

            }
            else if (common.myStr(Request.QueryString["RN"]) == "AckReport")
            {
                lblHeader.Text = "Patient Acknowledged Admitted Patient";
                rdolabelform.Visible = false;
                rdolabelformipbilldetail.Visible = false;
                tradmfrm.Visible = false;
                trdate.Visible = true;
                trddl.Visible = false;
                trgrid.Visible = false;
                trloction.Visible = false;
                rdoIpbillType.Visible = false;
                chkExport.Visible = true;
            }

            else if (common.myStr(Request.QueryString["RN"]) == "IPPatientStatus")
            {
                lblHeader.Text = "Patient Acknowledged Admitted Patient";
                rdolabelform.Visible = false;
                rdolabelformipbilldetail.Visible = false;
                tradmfrm.Visible = false;
                trdate.Visible = true;
                trddl.Visible = false;
                trgrid.Visible = false;
                trloction.Visible = false;
                rdoIpbillType.Visible = false;
                chkExport.Visible = true;
            }



        }
        if (rdolabelformipbilldetail.SelectedIndex == 1)
        {

            Panel2.Visible = true;
            Panel1.Visible = false;
        }
        else if (rdolabelformipbilldetail.SelectedIndex == 0)
        {

            Panel2.Visible = false;
            Panel1.Visible = true;
        }
        else if (rdolabelformipbilldetail.SelectedIndex == 2)
        {
            Panel2.Visible = false;
            Panel1.Visible = true;
        }


    }

    private void getHospitalFlagValue()
    {
        // get all flag value 
        BaseC.HospitalSetup objHospitalSetup = new BaseC.HospitalSetup(sConString); DataSet dsFlags = new DataSet();
        try
        {
            string flags = "'IsWardStation', 'IsChargeableforAttendantPass', 'ChargeableforAttendantPassServiceId'";

            dsFlags = objHospitalSetup.getHospitalSetupValueMultiple(flags, common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]));
            if (dsFlags.Tables.Count > 0)
            {

                //added by bhakti
                if (dsFlags.Tables[0].Select("Flag = 'IsWardStation'").Length > 0)
                {
                    IsWardStation = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsWardStation'")[0].ItemArray[1]);
                }

                
                if (dsFlags.Tables[0].Select("Flag = 'IsChargeableforAttendantPass'").Length > 0)
                {
                    IsChargeableforAttendantPass = common.myStr(dsFlags.Tables[0].Select("Flag = 'IsChargeableforAttendantPass'")[0].ItemArray[1]);
                }

               
                if (dsFlags.Tables[0].Select("Flag = 'ChargeableforAttendantPassServiceId'").Length > 0)
                {
                    ChargeableforAttendantPassServiceId = common.myInt(dsFlags.Tables[0].Select("Flag = 'ChargeableforAttendantPassServiceId'")[0].ItemArray[1]);
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally { dsFlags.Dispose(); objHospitalSetup = null; }

    }

    private void FillEntrySite()
    {
        try
        {
            int FacilityId;
            FacilityId = common.myInt(Session["FacilityId"]);
            BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);
            DataSet ds = obj.getEntrySite(Convert.ToInt16(Session["UserID"]), FacilityId);
            int EntrySiteIdx = ddlEntrySite.SelectedIndex;
            ddlEntrySite.DataSource = ds.Tables[0];
            ddlEntrySite.DataValueField = "ESId";
            ddlEntrySite.DataTextField = "ESName";
            ddlEntrySite.DataBind();
            ddlEntrySite.SelectedIndex = EntrySiteIdx;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            //objException.HandleException(Ex);
        }

    }
    protected void chkAllDepartment_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Stauts = true;
            CheckBox lbtn = sender as CheckBox;
            GridTableRow row = lbtn.NamingContainer as GridTableRow;
            CheckBox chkAllDep = (CheckBox)row.FindControl("chkAllDepartment");
            ViewState["chkAllDep"] = ((CheckBox)row.FindControl("chkAllDepartment")).Checked;
            if (chkAllDep.Checked == true)
            {

                foreach (GridTableRow rw in gvReporttype.Items)
                {
                    ((CheckBox)rw.FindControl("chkDepartment")).Checked = true;
                }
            }
            else
            {
                foreach (GridTableRow rw in gvReporttype.Items)
                {
                    ((CheckBox)rw.FindControl("chkDepartment")).Checked = false;
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


    protected void gvReporttype_PreRender(object sender, EventArgs e)
    {
        if (Stauts == false)
        {
            if (ddlReportType.SelectedValue.Equals("ReferByInternal") || ddlReportType.SelectedValue.Equals("ReferByExternal") || ddlReportType.SelectedValue.Equals("Sponsor"))
            {
                BindGridWithDate();
            }
            else
            {
                BindGrid();
            }

        }
    }

    protected void BindGrid()
    {
        objadt = new BaseC.ATD(sConString);
        ds = new DataSet();

        try
        {

            ds = objadt.GetReportTypes(common.myInt(Session["HospitalLocationId"]), common.myStr(ddlReportType.SelectedValue), common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                trgrid.Visible = true;
                gvReporttype.DataSource = ds;
                gvReporttype.DataBind();
            }
            else
            {
                trgrid.Visible = false;

                gvReporttype.DataSource = Bindblanktable();
                gvReporttype.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objadt = null;
            ds.Dispose();
        }
    }
    protected void BindGridWithDate()
    {
        BaseC.ATD objadt = new BaseC.ATD(sConString);
        ds = new DataSet();

        try
        {
            string fromdate = string.Empty;
            if (Request.QueryString["RN"] == "Admitason")
            {
                fromdate = Convert.ToDateTime("1988-08-19").ToString("yyyy-MM-dd");
            }
            else
            {
                fromdate = Convert.ToDateTime(dtpfromdate.SelectedDate).ToString("yyyy-MM-dd");
            }
            ds = objadt.GetReportTypes(common.myInt(Session["HospitalLocationId"]), common.myStr(ddlReportType.SelectedValue), common.myInt(Session["FacilityId"]), fromdate, Convert.ToDateTime(dtpTodate.SelectedDate).ToString("yyyy-MM-dd"));
            if (ds.Tables.Count > 0)
            {
                trgrid.Visible = true;
                gvReporttype.DataSource = ds;
                gvReporttype.DataBind();
            }
            else
            {
                trgrid.Visible = false;

                gvReporttype.DataSource = Bindblanktable();
                gvReporttype.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objadt = null;
            ds.Dispose();
        }
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            trgrid.Visible = true;
            lnkeditAgeRange.Visible = false;
            BaseC.clsLISExternalCenter obj = new BaseC.clsLISExternalCenter(sConString);
            objadt = new BaseC.ATD(sConString);
            ds = new DataSet();

            if (ddlReportType.SelectedValue == "Date")
            {
                ViewState["GroupId"] = "Datewise";
                //chkSelect.Checked = false;
            }
            else if (ddlReportType.SelectedValue == "Doctor")
            {
                Stauts = true;
                ViewState["GroupId"] = "Doctor";
                BindGrid();
            }
            else if (ddlReportType.SelectedValue == "Doctorteam")
            {
                Stauts = true;
                ViewState["GroupId"] = "Doctorteam";
                BindGrid();

            }

            else if (ddlReportType.SelectedValue == "Speciality")
            {
                Stauts = true;
                ViewState["GroupId"] = "Speciality";

                BindGrid();
            }
            else if (ddlReportType.SelectedValue == "Sponsor")
            {
                Stauts = true;
                ViewState["GroupId"] = "Sponsor";
                BindGridWithDate();

            }
            else if (ddlReportType.SelectedValue == "Bed")
            {
                Stauts = true;
                ViewState["GroupId"] = "Bed"; ;
                BindGrid();

            }
            else if (ddlReportType.SelectedValue == "Ward")
            {
                Stauts = true;
                ViewState["GroupId"] = "ward";
                BindGrid();

            }
            else if (ddlReportType.SelectedValue == "Country")
            {
                Stauts = true;
                ViewState["GroupId"] = "Country";
                BindGrid();
            }
            else if (ddlReportType.SelectedValue == "State")
            {
                Stauts = true;
                ViewState["GroupId"] = "State";
                BindGrid();
            }
            else if (ddlReportType.SelectedValue == "City")
            {
                Stauts = true;
                ViewState["GroupId"] = "City";
                BindGrid();
            }
            else if (ddlReportType.SelectedValue == "Area")
            {
                Stauts = true;
                ViewState["GroupId"] = "Area";
                BindGrid();

            }
            else if (ddlReportType.SelectedValue == "Source")
            {
                Stauts = true;
                ViewState["GroupId"] = "Source";
                BindGrid();

            }
            else if (ddlReportType.SelectedValue.Equals("Age"))
            {
                Stauts = true;
                ViewState["GroupId"] = "Age";
                lnkeditAgeRange.Visible = true;
                trgrid.Visible = false;
            }
            else if (ddlReportType.SelectedValue.Equals("DischargeTypeWise"))
            {
                Stauts = true;
                ViewState["GroupId"] = "Discharge Type Wise";
                BindGrid();
            }
            else if (ddlReportType.SelectedValue.Equals("ReferByInternal"))
            {
                Stauts = true;
                ViewState["GroupId"] = "ReferByInternal";
                BindGridWithDate();
            }

            else if (ddlReportType.SelectedValue.Equals("ReferByExternal"))
            {
                Stauts = true;
                ViewState["GroupId"] = "ReferByExternal";
                BindGridWithDate();
            }
            else if (ddlReportType.SelectedValue.Equals("CompanyType"))
            {
                Stauts = true;
                ViewState["GroupId"] = "CompanyType";
                BindGrid();
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void lnkeditAgeRange_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/MRD/Master/MRDReportAgeRange.aspx";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void lbtnSearchPatient_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["RN"]) == "PTD")
            RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetailsV1.aspx?RegEnc=0&SearchOn=0";
        else
            RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetailsV1.aspx?OPIP=I&RegEnc=1&SearchOn=1";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;

    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        try
        {

            if (common.myStr(Session["Retain"]) == "Retain")
                txtIpno.Text = common.myStr(Session["Retainipno"]);

            if (common.myStr(Request.QueryString["RN"]) == "PTD")
                txtIpno.Text = hdnRegistrationNo.Value.ToString();


            //if(common.myStr(Request.QueryString["RN"])== "BillPrint")
            //   txtIpno.Text = hdnInvEncounterId.Value.ToString();




        }
        catch (Exception Ex)
        {


        }
    }

    protected void lbtnSearchPatientvoice_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMRBILLING/AdvanceList.aspx?AR=IPBill";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 990;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "OnClientCloseSearch";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowForNew.VisibleStatusbar = false;

    }
    protected void btnfindinvoice_Click(object sender, EventArgs e)
    {
        BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);//Added by Ankit ojha
        DataTable dt = new DataTable();
        Hashtable hshIn = new Hashtable();
        try
        {


            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hshIn.Add("@HospitalLocationId", common.myInt(Session["HospitalLocationId"]));
            hshIn.Add("@FacilityId", common.myInt(Session["FacilityId"]));
            hshIn.Add("@BillNo", common.myStr(txtInvoice.Text));

            //ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetBillDetails", hshIn);
            dt = obj.GetBillDetails(hshIn);

            //if (ds.Tables.Count > 0)
            //{
            if (dt.Rows.Count > 0)
            {
                int billid = common.myInt(dt.Rows[0]["InvoiceId"].ToString().Trim());
                hdnBillId.Value = Convert.ToString(billid);
                DataRow dr = dt.Rows[0];

                hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                hdnRegistrationNo.Value = common.myStr(dr["RegistrationNo"]);
                hdnInvEncounterId.Value = common.myStr(dr["EncounterId"]);

            }
            //}
            rdolabelformipbilldetail_SelectedIndexChanged(this, null);
        }
        catch (Exception Ex)
        {

            throw Ex;
        }
        finally
        {
            obj = null;
            hshIn = null;
            dt.Dispose();
        }
    }



    protected void btnShow_Click(object sender, EventArgs e)
    {
        BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);//Added by Ankit ojha
        Hashtable htRegistration = new Hashtable();

        try
        {
            if (common.myStr(txtIpno.Text) != "" || common.myStr(txtInvoice.Text) != "" || common.myStr(dtFromDate.SelectedDate) != "")
            {


                Session["IPNO"] = txtIpno.Text.Trim();
                //LinkButton lnkBtn = (LinkButton)sender;

                //string RegId = ((HiddenField)lnkBtn.FindControl("hdnInvEncounterId")).Value;
                if (common.myStr(Request.QueryString["RN"]) == "Admform")
                {

                    //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    //DataSet ds = dl.FillDataSet(CommandType.Text, "select Id,RegistrationId,RegistrationNo from Encounter where EncounterNo ='" + txtIpno.Text + "' and FacilityId=" + common.myInt(Session["FacilityId"]));

                    htRegistration.Add("@EncounterNo", txtIpno.Text);
                    htRegistration.Add("@FacilityId", common.myInt(Session["FacilityId"]));
                    DataSet ds = obj.GetRegistrationNoFromIPNo(htRegistration);

                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        hdnRegistrationNo.Value = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        hdnRegistrationId.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
                        hdnInvEncounterId.Value = ds.Tables[0].Rows[0]["Id"].ToString();
                    }
                    if (common.myStr(rdolabelform.SelectedValue) == "N")
                    {
                        Session["DisEncounterNO"] = txtIpno.Text;
                        RadWindowForNew.NavigateUrl = "/EMRREPORTS/PrintOPDReceipt.aspx?PrintType=DischargeNotification&IpNo=" + common.myStr(txtIpno.Text);
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "OT")
                    {
                        Session["DisEncounterNO"] = txtIpno.Text;
                        RadWindowForNew.NavigateUrl = "/EMRREPORTS/PrintOPDReceipt.aspx?PrintType=OPTagging&IpNo=" + common.myStr(txtIpno.Text);
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "OS")
                    {
                        Session["DisEncounterNO"] = txtIpno.Text;
                        RadWindowForNew.NavigateUrl = "/EMRREPORTS/PrintOPDReceipt.aspx?PrintType=OPTaggingSummry&IpNo=" + common.myStr(txtIpno.Text);
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "B")
                    {
                        hdnDocumentType.Value = "AdmissionBarCodeLabel";
                        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                        DataSet ds1 = new DataSet();
                        ds1 = objBill.GetDocumentPrintingLog("AdmissionBarCodeLabel", common.myInt(hdnRegistrationId.Value), common.myInt(hdnInvEncounterId.Value), common.myInt(Session["FacilityId"]));
                        if (ds1 != null && ds1.Tables.Count > 0)
                        {
                            if (common.myInt(ds1.Tables[0].Rows[0]["IsReasonRequiredForReprint"]).Equals(0) || common.myInt(ds1.Tables[0].Rows[0]["PrintCount"]).Equals(0))
                            {
                                hdnReasonForPrinting.Value = "FirstPrint";
                                PrintDocumnets("AdmissionBarCodeLabel");
                            }
                            else
                            {
                                txtReasonForRePrint.Text = string.Empty;
                                btnPrintDocument.Text = "Print Bar Code";
                                Div2.Visible = true;
                            }
                        }
                        ds1.Dispose();//added by ankit ojha
                        objBill = null;
                        return;
                        //RadWindowForNew.NavigateUrl = "/EMRReports/ReportBarCode.aspx?ReportName=AdmissionBarCodeLabel&EncNo=" + common.myStr(txtIpno.Text);
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "V")
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "";
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "R")
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "";
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "AR")
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "&RegNo=" + common.myInt(hdnRegistrationNo.Value) + "";
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "AD")
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "&RegNo=" + common.myInt(hdnRegistrationNo.Value) + "";
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "FS")
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "&RegNo=" + common.myInt(hdnRegistrationNo.Value) + "";
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "PD")
                    {
                        hdnDocumentType.Value = "AdmissionPatientDetails";
                        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                        DataSet ds1 = new DataSet();
                        ds1 = objBill.GetDocumentPrintingLog("AdmissionPatientDetails", common.myInt(hdnRegistrationId.Value), common.myInt(hdnInvEncounterId.Value), common.myInt(Session["FacilityId"]));
                        if (ds1 != null && ds1.Tables.Count > 0)
                        {
                            if (common.myInt(ds1.Tables[0].Rows[0]["IsReasonRequiredForReprint"]).Equals(0) || common.myInt(ds1.Tables[0].Rows[0]["PrintCount"]).Equals(0))
                            {
                                hdnReasonForPrinting.Value = "FirstPrint";
                                PrintDocumnets("AdmissionPatientDetails");
                            }
                            else
                            {
                                txtReasonForRePrint.Text = string.Empty;
                                btnPrintDocument.Text = "Patient Details";
                                Div2.Visible = true;
                            }
                        }
                        ds1.Dispose();
                        objBill = null;
                        return;
                        //RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "&RegNo=" + common.myInt(hdnRegistrationNo.Value) + "";
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "F")
                    {
                        hdnDocumentType.Value = "AdmissionForm";
                        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                        DataSet ds1 = new DataSet();
                        ds1 = objBill.GetDocumentPrintingLog("AdmissionForm", common.myInt(hdnRegistrationId.Value), common.myInt(hdnInvEncounterId.Value), common.myInt(Session["FacilityId"]));
                        if (ds1 != null && ds1.Tables.Count > 0)
                        {
                            if (common.myInt(ds1.Tables[0].Rows[0]["IsReasonRequiredForReprint"]).Equals(0) || common.myInt(ds1.Tables[0].Rows[0]["PrintCount"]).Equals(0))
                            {
                                hdnReasonForPrinting.Value = "FirstPrint";
                                PrintDocumnets("AdmissionForm");
                            }
                            else
                            {
                                txtReasonForRePrint.Text = string.Empty;
                                btnPrintDocument.Text = "Print Form";
                                Div2.Visible = true;
                            }
                        }
                        ds1.Dispose();
                        objBill = null;
                        return;
                        //RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "";
                    }

                    else if (common.myStr(rdolabelform.SelectedValue) == "PS")
                    {
                        hdnDocumentType.Value = "PatientSatisfication";

                        PrintDocumnets("PatientSatisfication");
                    }

                    else if (common.myStr(rdolabelform.SelectedValue) == "EF")
                    {
                        hdnDocumentType.Value = "ExtensionRequestForm";

                        PrintDocumnets("ExtensionRequestForm");
                    }

                    else if (common.myStr(rdolabelform.SelectedValue) == "SL")
                    {
                        hdnDocumentType.Value = "Single Admission Label";

                        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                        DataSet ds1 = new DataSet();
                        ds1 = objBill.GetDocumentPrintingLog("SingleAdmissionLabel", common.myInt(hdnRegistrationId.Value), common.myInt(hdnInvEncounterId.Value), common.myInt(Session["FacilityId"]));
                        if (ds1 != null && ds.Tables.Count > 0)
                        {
                            if (common.myInt(ds1.Tables[0].Rows[0]["IsReasonRequiredForReprint"]).Equals(0) || common.myInt(ds1.Tables[0].Rows[0]["PrintCount"]).Equals(0))
                            {
                                hdnReasonForPrinting.Value = "SingleAdmissionLabel";
                                PrintDocumnets("SingleAdmissionLabel");
                            }
                            else
                            {
                                txtReasonForRePrint.Text = string.Empty;
                                btnPrintDocument.Text = "Single Admission Label";
                                Div2.Visible = true;
                            }
                        }
                        ds1.Dispose();
                        objBill = null;
                        return;
                    }
                    else if (common.myStr(rdolabelform.SelectedValue) == "L")
                    {
                        hdnDocumentType.Value = "AdmissionLabel";
                        BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                        DataSet ds1 = new DataSet();
                        ds1 = objBill.GetDocumentPrintingLog("AdmissionLabel", common.myInt(hdnRegistrationId.Value), common.myInt(hdnInvEncounterId.Value), common.myInt(Session["FacilityId"]));
                        if (ds1 != null && ds.Tables.Count > 0)
                        {
                            if (common.myInt(ds1.Tables[0].Rows[0]["IsReasonRequiredForReprint"]).Equals(0) || common.myInt(ds1.Tables[0].Rows[0]["PrintCount"]).Equals(0))
                            {
                                hdnReasonForPrinting.Value = "FirstPrint";
                                PrintDocumnets("AdmissionLabel");
                            }
                            else
                            {
                                txtReasonForRePrint.Text = string.Empty;
                                btnPrintDocument.Text = "Print Label";
                                Div2.Visible = true;
                            }
                        }
                        ds1.Dispose();
                        objBill = null;
                        return;
                        //RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "";
                    }
                    else
                    {
                        RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?ReportName=Admfrom&ReportType=" + rdolabelform.SelectedValue + "";
                    }


                }
                else if (common.myStr(Request.QueryString["RN"]) == "ReAdmform")
                {
                    if (common.myStr(dtFromDate.SelectedDate) == "" && common.myStr(dtTodate.SelectedDate) == "")
                    {
                        Alert.ShowAjaxMsg("Re-admitted List From date or To date can not be 'Blank' ..", Page);
                        return;
                    }
                    string NoOfDays = ((Convert.ToDateTime(dtTodate.SelectedDate) - Convert.ToDateTime(dtFromDate.SelectedDate)).TotalDays).ToString();
                    RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=ReAdmForm&dtFromdate=" + dtFromDate.SelectedDate + "&dtTodate=" + dtTodate.SelectedDate + "&NoOfDays=" + NoOfDays;
                }
                else if (common.myStr(Request.QueryString["RN"]) == "ReAdmformWithDay")
                {
                    if (common.myStr(dtFromDate.SelectedDate) == "" && common.myStr(dtTodate.SelectedDate) == "")
                    {
                        Alert.ShowAjaxMsg("Re-admitted List From date or To date can not be 'Blank' ..", Page.Page);
                        return;
                    }
                    if (common.myInt(txtIpno.Text) <= 0)
                    {
                        Alert.ShowAjaxMsg("Re-admitted List Within Days limit can not be 'Blank' ..", Page.Page);
                        lblMessage.Text = "Re-admitted List Within Days limit can not be 'Blank";
                        return;
                    }
                    RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=ReAdmformWithDay&NoOfDays=" + txtIpno.Text + "&dtFromdate=" + dtFromDate.SelectedDate + "&dtTodate=" + dtTodate.SelectedDate;
                }
                else if (common.myStr(Request.QueryString["RN"]) == "PTD")
                {
                    RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=TREATMENTOPIPREPORT&RNO=" + txtIpno.Text + "&RID=" + common.myInt(hdnRegistrationId.Value) + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "";
                }
                else if (common.myStr(Request.QueryString["RN"]) == "BillPrint")
                {
                    string strShowAdvance = "Y", strShowDiscount = "Y";
                    if (rdolabelformipbilldetail.SelectedIndex == 0 && rdoIpbillType.SelectedValue != "A")
                    {
                        if (hdnInvEncounterId.Value == "")
                        {
                            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                            //DataSet ds = dl.FillDataSet(CommandType.Text, "select Id,RegistrationId,RegistrationNo from Encounter where EncounterNo ='" + txtIpno.Text + "' and FacilityId=" + common.myInt(Session["FacilityId"]));

                            htRegistration.Add("@EncounterNo", txtIpno.Text);
                            htRegistration.Add("@FacilityId", common.myInt(Session["FacilityId"]));
                            DataSet ds = obj.GetRegistrationNoFromIPNo(htRegistration);



                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                hdnRegistrationNo.Value = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                                hdnRegistrationId.Value = ds.Tables[0].Rows[0]["RegistrationId"].ToString();
                                hdnInvEncounterId.Value = ds.Tables[0].Rows[0]["Id"].ToString();
                            }
                        }
                        RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(hdnInvEncounterId.Value) + "&RptName=IPBill&RptType=" + rdoIpbillType.SelectedValue + "&AdmDt=" + common.myStr(0) + "&Adv=" + strShowAdvance + "&Disc=" + strShowDiscount + "&RegId=" + common.myInt(hdnRegistrationId.Value) + "&BillId=" + common.myInt(0) + "&ToDate=" + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm") + "&ReportType=D" + "&ShowOrginal=" + true + "&ShowDefaultCurrency=N";
                        //txtIpno.Text = string.Empty;
                    }
                    else if (rdolabelformipbilldetail.SelectedIndex == 1 || rdoIpbillType.SelectedValue == "A")
                    {
                        if (chkIsChargeable.Checked)
                        {

                            BaseC.clsEMRBilling BaseBill = new BaseC.clsEMRBilling(sConString);
                            ArrayList coll = new ArrayList();
                            StringBuilder strXML = new StringBuilder();
                            coll.Add(common.myInt(hdnserviceId.Value)); //ServiceId INT,
                            coll.Add(DBNull.Value); //VisitDate SMALLDATETIME,   common.myDate(DateTime.Now).ToString("yyyy-MM-dd HH:mm:00")
                            coll.Add(common.myInt(1)); //Units TINYINT,
                            coll.Add(DBNull.Value); //DoctorId INT, 
                            coll.Add(common.myDec(hdnServiceAmount.Value)); //ServiceAmount MONEY,
                            coll.Add(common.myDec(hdnDoctorAmount.Value)); //DoctorAmount MONEY,  
                            coll.Add(common.myDec(hdnServiceDiscountAmount.Value)); //ServiceDiscountAmount MONEY, 
                            coll.Add(common.myDec(hdnDoctorDiscountAmount.Value)); //DoctorDiscountAmount MONEY,
                            coll.Add(common.myDec(hdnAmountPayableByPatient.Value)); //AmountPayableByPatient MONEY,
                            coll.Add(common.myDec(hdnAmountPayableByPayer.Value)); //AmountPayableByPayer MONEY,
                            coll.Add(common.myDec(hdnServiceDiscountPercentage.Value)); //ServiceDiscountPer MONEY,
                            coll.Add(common.myDec(hdnDoctorDiscountPercentage.Value)); //DoctorDiscountPer MONEY,
                            coll.Add(common.myInt(0)); //PackageId INT,  
                            coll.Add(common.myInt(0)); //OrderId INT,
                            coll.Add(common.myInt(0)); //UnderPackage BIT,               
                            coll.Add(DBNull.Value); //ICDID VARCHAR(100), 
                            coll.Add(DBNull.Value); //ResourceID INT,
                            coll.Add(DBNull.Value); //SurgeryAmount MONEY, 
                            coll.Add(DBNull.Value); //ProviderPercent MONEY,
                            coll.Add(common.myInt(1)); //SeQNo INT, 
                            coll.Add(common.myStr("AttendantPass default Servicecahege")); //Serviceremarks Varchar(150)
                            coll.Add(0);// DetailId 0 in case of new order
                            coll.Add(common.myInt(0));//23//Er Order
                            coll.Add(common.myStr(0));//24//pharmacyOrder
                            coll.Add(common.myDec(0));//CopayPerc
                            coll.Add(common.myDec(0));//DeductableAmount
                            coll.Add(common.myStr(" "));//Approval code
                            coll.Add(common.myStr(common.myInt(Session["FacilityId"])));//Facility Id
                            coll.Add(DBNull.Value);//Stat or routine order
                            coll.Add(common.myBool(0));
                            coll.Add(DBNull.Value);
                            coll.Add(DBNull.Value);
                            coll.Add(DBNull.Value);// 35
                            coll.Add(DBNull.Value);
                            coll.Add(DBNull.Value);
                            coll.Add(DBNull.Value);
                            coll.Add(common.myBool(0));//37
                            coll.Add(DBNull.Value);
                            coll.Add(common.myBool(0));//39
                            coll.Add(DBNull.Value);
                            coll.Add(common.myDec(0));//CopayPerc//41
                            coll.Add(DBNull.Value);//42
                            coll.Add(DBNull.Value);//43
                            coll.Add(DBNull.Value);//44
                            coll.Add(common.myDec(hdnServiceAmount.Value));//ServiceActualAmount//45
                            coll.Add(DBNull.Value);//46 ChargeableSeviceId  
                            coll.Add(common.myDec(hdnServiceAmount.Value));//47
                            coll.Add(common.myBool(0));//48
                            strXML.Append(common.setXmlTable(ref coll));

                            Hashtable hshOut = BaseBill.saveOrders(
                          common.myInt(Session["HospitalLocationID"]),
                          common.myInt(Session["FacilityId"]),
                          common.myInt(hdnRegistrationId.Value),
                          common.myInt(hdnInvEncounterId.Value),
                          strXML.ToString(), "",
                          common.myInt(Session["UserID"]),
                          common.myInt(0),
                          common.myInt(0),
                          "",
                          common.myStr(""),
                          common.myStr("I"),
                          common.myInt(""),
                          common.myInt(""),
                          Convert.ToDateTime(System.DateTime.UtcNow), "Y",
                          common.myInt(Session["EntrySite"]), 0, false);
                        }
                        RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(hdnInvEncounterId.Value)
                       + "&RptName=IPBill&RptType=" + rdoIpbillType.SelectedValue
                       + "&BillId=" + common.myInt(hdnBillId.Value)
                       + "&AdmDt=" + common.myStr(0)
                       + "&Adv=" + strShowAdvance
                       + "&Disc=" + strShowDiscount
                       + "&RegId=" + common.myInt(hdnRegistrationId.Value)
                       + "&FromDate=" + Convert.ToDateTime(dtpfromdate.SelectedDate).ToString("dd/MM/yyyy HH:mm")
                       + "&ToDate=" + Convert.ToDateTime(dtpTodate.SelectedDate).ToString("dd/MM/yyyy HH:mm")
                       + "&IsFilterByDate=" + common.myInt(0)
                       + "&ShowOrginal=" + true
                       + "&ShowDefaultCurrency=N";
                        //+ "&preview=" + preview; ReportType
                        //txtInvoice.Text = string.Empty;
                    }
                    else if (rdolabelformipbilldetail.SelectedIndex == 2)
                    {
                        RadWindowForNew.NavigateUrl = "/EMRBILLING/Report/PreView.aspx?EncId=" + common.myInt(hdnInvEncounterId.Value)
                            + "&RptName=IPBill&RptType=" + rdolabelformipbilldetail.SelectedValue
                            + "&AdmDt=" + common.myStr(0)
                            + "&Adv=" + strShowAdvance
                            + "&Disc=" + strShowDiscount
                            + "&RegId=" + common.myInt(hdnRegistrationId.Value)
                            + "&BillId=" + common.myInt(0)
                            + "&ToDate=" + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm")
                            + "&ShowOrginal=" + true
                            + "&ShowDefaultCurrency=N";
                        //txtIpno.Text = string.Empty;
                    }
                }

                else
                {
                    //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    //DataSet ds = dl.FillDataSet(CommandType.Text, "select Enc.Id,Enc.RegistrationId,ESD.formatid from Encounter Enc inner join EMRPatientSummaryDetails ESD on Enc.id = ESD.Encounterid  where Enc.EncounterNo ='" + txtIpno.Text + "' and Enc.FacilityId=" + common.myInt(Session["FacilityId"]));
                    htRegistration.Add("@EncounterNo", txtIpno.Text);
                    htRegistration.Add("@FacilityId", common.myInt(Session["FacilityId"]));
                    DataSet ds = obj.GetRegistrationNoFromEMRPatientSummaryDetails(htRegistration);




                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //RadWindowForNew.NavigateUrl = "/EMRReports/DischargeSummary.aspx?IpNo=" + common.myStr(txtIpno.Text) + "";
                        RadWindowForNew.NavigateUrl = "/EMRReports/PrintPdf1.aspx?page=Report&EncId=" + common.myStr(ds.Tables[0].Rows[0]["Id"]) +
                            "&RegId=" + common.myStr(ds.Tables[0].Rows[0]["RegistrationId"]) + "&ReportId=" + common.myStr(ds.Tables[0].Rows[0]["formatid"]) + "&For=DISSUM";
                    }
                }

                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 950;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                                                          //RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
                                                          //RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;

                rdolabelformipbilldetail_SelectedIndexChanged(this, null);


            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            obj = null;
            htRegistration = null;
        }
    }


    protected void btnPrintreport_OnClick(object sender, EventArgs e)
    {
        lblMessage.Text = string.Empty;
        String sbIdList = string.Empty;
        if (common.myStr(Request.QueryString["RN"]) == "Admitted" || common.myStr(Request.QueryString["RN"]) == "Admis" || common.myStr(Request.QueryString["RN"]) == "Disc" || common.myStr(Request.QueryString["RN"]) == "Admitason" || common.myStr(Request.QueryString["RN"]) == "DischWOBill")
        {
            if (ddlReportType.SelectedValue != "Age")
            {
                if ((ddlReportType.SelectedValue != "Date"))
                {

                    foreach (GridDataItem item in gvReporttype.Items)
                    {
                        if (((CheckBox)item.FindControl("chkDepartment")).Checked == true)
                        {
                            if (sbIdList == "")
                                sbIdList = ((Label)item.FindControl("lblId")).Text;
                            else
                                sbIdList = sbIdList + "," + ((Label)item.FindControl("lblId")).Text;
                        }
                    }
                    ViewState["setformula"] = sbIdList;
                    Session["AdmittedasonsbIdList"] = sbIdList;

                    if (common.myStr(sbIdList) == "")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please Select " + common.myStr(ViewState["GroupId"]);
                        return;
                    }
                }
            }
        }

        // 
        if (common.myStr(Request.QueryString["RN"]) == "Trans")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate
              + "&Todate=" + dtpTodate.SelectedDate
              //+ "&Provider=" + sbIdList
              + "&Export=" + chkExport.Checked
              + "&ReportType=" + ddlTransferReport.SelectedValue
              + "&ReportName=Transfer";

        }
        if (common.myStr(Request.QueryString["RN"]) == "Admitted")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"]
                // + "&sValue=" + sbIdList 
                + "&Export=" + chkExport.Checked + "&Summary=" + ChkSummary.Checked + "&ReportName=Admitted";
        }
        if (common.myStr(Request.QueryString["RN"]) == "Admitason")
        {

            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"] + "&Export=" + chkExport.Checked + "&Summary=" + ChkSummary.Checked + "&ReportName=Admittedason";
        }
        if (common.myStr(Request.QueryString["RN"]).Equals("Disc"))
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate
                + "&Todate=" + dtpTodate.SelectedDate
                + "&GroupId=" + ViewState["GroupId"]
                //+ "&sValue=" + sbIdList
                + "&Export=" + chkExport.Checked
                + "&Summary=" + ChkSummary.Checked + "&ReportName=DischargeDetailList";
        }
        if (common.myStr(Request.QueryString["RN"]) == "DischWOBill")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"]
                //+ "&sValue=" + sbIdList 
                + "&Export=True&Summary=" + ChkSummary.Checked + "&ReportName=DischargeListWOBilling";
        }
        if (common.myStr(Request.QueryString["RN"]) == "Admis")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"]
                // + "&sValue=" + sbIdList 
                + "&Export=" + chkExport.Checked + "&Summary=" + ChkSummary.Checked + "&ReportName=Admission";
        }
        //Added on 24/06/2015
        if (common.myStr(Request.QueryString["RN"]) == "RegArea")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate.Value + "&Todate=" + dtpTodate.SelectedDate.Value + "&Export=" + chkExport.Checked + "&ReportName=Registration";
        }
        if (common.myStr(Request.QueryString["RN"]) == "updownbdCatg")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"]
                //+ "&sValue=" + sbIdList 
                + "&isAdm= " + chkIsAdmitted.Checked + "&Export=" + chkExport.Checked + "&Summary=" + ChkSummary.Checked + "&ReportName=updownbdCatg";
        }
        if (common.myStr(Request.QueryString["RN"]) == "AdmER")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"]
                // + "&sValue=" + sbIdList 
                + "&Export=" + chkExport.Checked + "&Summary=" + ChkSummary.Checked + "&ReportName=AdmER";
        }
        if (common.myStr(Request.QueryString["RN"]) == "DiscER")//Added 
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&GroupId=" + ViewState["GroupId"]
                //+ "&sValue=" + sbIdList 
                + "&Export=" + chkExport.Checked + "&Summary=" + ChkSummary.Checked + "&ReportName=DischargeDetailListEmergency";
        }
        if (common.myStr(Request.QueryString["RN"]) == "AckReport")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=PatientAck";
        }
        if (common.myStr(Request.QueryString["RN"]) == "IPPatientStatus")
        {
            RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?EntrySite=" + ddlEntrySite.SelectedValue + "&Fromdate=" + dtpfromdate.SelectedDate + "&Todate=" + dtpTodate.SelectedDate + "&Export=" + chkExport.Checked + "&ReportName=IPPatientStatus";
        }

        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 980;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        // RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin | WindowBehaviors.Resize;
        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    DataTable Bindblanktable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Id");
        dt.Columns.Add("Name");

        return dt;
    }
    protected void BindProviderList()
    {
        //DAL.DAL dl; dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        BaseC.Hospital baseHc = new BaseC.Hospital(sConString);
        DataSet ds = baseHc.fillDoctorCombo(Convert.ToInt16(Session["HospitalLocationID"].ToString()), 0, Convert.ToInt16(Session["FacilityId"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].Columns[0].ColumnName = "Id";
            ds.Tables[0].Columns[1].ColumnName = "Name";

            gvReporttype.DataSource = ds;
            gvReporttype.DataBind();
        }
        else
        {

            gvReporttype.DataSource = Bindblanktable();
            gvReporttype.DataBind();
        }
    }
    protected void dtpTodate_OnSelectedDateChanged(object sender, EventArgs e)
    {
        if (common.myDate(dtpTodate.SelectedDate) < common.myDate(System.DateTime.Now))
        {

            dtpTodate.SelectedDate = common.myDate(dtpTodate.SelectedDate.Value.ToString(common.myStr(Session["OutputDateFormat"])) + " 23:59");
            // dtpfromdate.SelectedDate = common.myDate(dtpTodate.SelectedDate.Value.ToString(common.myStr(Session["OutputDateFormat"])) + " 00:00");
            dtpfromdate.SelectedDate = common.myDate(dtpfromdate.SelectedDate.Value.ToString(common.myStr(Session["OutputDateFormat"])) + " 00:00");

        }
        BindGridWithDate();
    }

    protected void rdolabelformipbilldetail_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdolabelformipbilldetail.SelectedValue == "I")
        {
            rdoIpbillType.Items[2].Attributes.Add("style", "display:none");
        }

    }
    //Added by om

    protected void rdoIpbillType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdolabelformipbilldetail.SelectedValue == "I")
        {
            rdoIpbillType.Items[2].Attributes.Add("style", "display:none");
        }
    }

    protected void btnPrintDocument_Click(object sender, EventArgs e)
    {
        if (common.myLen(txtReasonForRePrint.Text).Equals(0))
        {
            Alert.ShowAjaxMsg("Reason is mandatory for re print", Page);
            return;
        }
        hdnReasonForPrinting.Value = txtReasonForRePrint.Text;
        PrintDocumnets(hdnDocumentType.Value);
    }

    protected void btnCloseDivForReason_Click(object sender, EventArgs e)
    {
        Div2.Visible = false;
    }
    public void PrintDocumnets(string DocumentType)
    {
        switch (DocumentType)
        {
            case "AdmissionLabel":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=L&Reason=" + common.myStr(hdnReasonForPrinting.Value);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;
            case "AdmissionBarCodeLabel":
                RadWindowForNew.NavigateUrl = "/EMRReports/ReportBarCode.aspx?MPG=P22248&ReportName=AdmissionBarCodeLabel&EncNo=" + common.myStr(txtIpno.Text) + "&Reason=" + common.myStr(hdnReasonForPrinting.Value);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;
            case "AdmissionForm":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=F&Reason=" + common.myStr(hdnReasonForPrinting.Value);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;
            case "WristBandAdult":
                Session["RegistrationId"] = hdnRegistrationId.Value;
                Session["IPNO"] = txtIpno.Text;
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=WB&Reason=" + common.myStr(hdnReasonForPrinting.Value) + "&DocType=" + hdnDocumentType.Value;
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;
            case "AdmissionPatientDetails":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=Adminfo&Reason=" + common.myStr(hdnReasonForPrinting.Value);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;
            case "WristBandChild":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=WBC&Reason=" + common.myStr(hdnReasonForPrinting.Value) + "&DocType=" + hdnDocumentType.Value;
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;

            case "PatientSatisfication":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                if (!string.IsNullOrEmpty(txtUHID.Text.Trim()))
                {
                    RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=PS&Reason=" + common.myStr(hdnReasonForPrinting.Value) + "&UHID=" + hdnRegistrationId.Value.Trim();
                }
                else
                { RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=PS&Reason=" + common.myStr(hdnReasonForPrinting.Value) + "&UHID=0"; }
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;


            case "SingleAdmissionLabel":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=SL&Reason=" + common.myStr(hdnReasonForPrinting.Value);
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;

            case "ExtensionRequestForm":
                Session["IPNO"] = common.myStr(txtIpno.Text);
                if (!string.IsNullOrEmpty(txtUHID.Text.Trim()))
                {
                    RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=EF&Reason=" + common.myStr(hdnReasonForPrinting.Value) + "&UHID=" + hdnRegistrationId.Value.Trim();
                }
                else
                { RadWindowForNew.NavigateUrl = "/EMRReports/CallAdmissionfrmrpt.aspx?MPG=P22247&ReportName=Admfrom&ReportType=EF&Reason=" + common.myStr(hdnReasonForPrinting.Value) + "&UHID=0"; }
                RadWindowForNew.Height = 550;
                RadWindowForNew.Width = 850;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
                break;



        }


    }

    protected void lnkSearchPatientbyUHID_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetailsV1.aspx?OPIP=I&RegEnc=0&SearchOn=0";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 1000;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }


    public void BindMenu()
    {
        #region GroupWiseMenu
        BaseC.User user = new BaseC.User(sConString);
        DataTable dtGroupWiseMenu = new DataTable();
        rdolabelform.Items.Clear();
        dtGroupWiseMenu = (user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]), common.myInt(Session["ModuleId"]), "AdmissionReportType")).Tables[0];

        if (dtGroupWiseMenu != null && dtGroupWiseMenu.Rows.Count > 0)
        {
            rdolabelform.DataSource = dtGroupWiseMenu;
            rdolabelform.DataTextField = "OptionName";
            rdolabelform.DataValueField = "OptionCode";
            rdolabelform.DataBind();
        }
        dtGroupWiseMenu.Dispose();
        #endregion
    }

    protected void rdoIpbillType_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (common.myStr(rdoIpbillType.SelectedValue).ToUpper().Equals("A") && IsChargeableforAttendantPass.Equals("Y"))
        {
            chkIsChargeable.Visible = true;
        }
        else
        {
            chkIsChargeable.Visible = false;
        }
    }

    protected void chkIsChargeable_CheckedChanged(object sender, EventArgs e)
    {
        if(chkIsChargeable.Checked)
        {
            Hashtable hshServiceDetail = new Hashtable();
            BaseC.clsEMRBilling BaseBill = new BaseC.clsEMRBilling(sConString);
            hshServiceDetail = BaseBill.getSingleServiceAmount(common.myInt(Session["HospitalLocationID"]),
                                 common.myInt(Session["FacilityId"]),
                                 common.myInt(0),
                                 common.myInt(0),
                                 common.myInt(0),
                                 common.myStr(ViewState["OP_IP"]),
                                 common.myInt(ChargeableforAttendantPassServiceId),
                                 common.myInt(hdnRegistrationId.Value),
                                 common.myInt(hdnInvEncounterId.Value), 0, 0, 0);

            hdnserviceId.Value =common.myStr(ChargeableforAttendantPassServiceId);
            hdnServiceAmount.Value = common.myStr(hshServiceDetail["NChr"]);
            hdnDoctorAmount.Value = common.myStr(hshServiceDetail["DNchr"]);
            hdnServiceDiscountAmount.Value = common.myStr(hshServiceDetail["DiscountNAmt"]);
            hdnDoctorDiscountAmount.Value = common.myStr(hshServiceDetail["DiscountDNAmt"]);
            hdnAmountPayableByPatient.Value = common.myStr(hshServiceDetail["PatientNPayable"]);
            hdnAmountPayableByPayer.Value = common.myStr(hshServiceDetail["PayorNPayable"]);
            hdnServiceDiscountPercentage.Value = common.myStr(hshServiceDetail["DiscountPerc"]);
            hdnDoctorDiscountPercentage.Value = common.myStr(hshServiceDetail["DiscountDNAmt"]);
        }
    }
}
