using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using System.Drawing;

public partial class OTScheduler_OTBookingDeatils : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    BaseC.clsOTBooking objbc;
    BaseC.RestFulAPI objwcf;//= new wcf_Service_OT.ServiceClient();
    DataSet ds;
    StringBuilder strXML;
    ArrayList coll;
    private static bool status = false;

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Legend1.loadLegend("OT", "");
        if (!IsPostBack)
        {
            //palendra
            BaseC.Security objSecurity = new BaseC.Security(sConString);  //
            if (common.myBool(objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "IsAllowPREandPOSTOTDashBoard")))
            {
                btnShowHideInDashBoard.Visible = true;
                rdOTStatus.Visible = true;
            }
            else
            {
                btnShowHideInDashBoard.Visible = false;
                rdOTStatus.Visible = false;
            }
            //palendra
            objwcf = new BaseC.RestFulAPI(sConString);
            ViewState["PT"] = common.myStr(Request.QueryString["PT"]);
            btnBillClearance.Visible = false;
            txtSearchN.Visible = false;
            txtSearch.Visible = false;
            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                txtSearchN.Visible = true;
            }
            else
            {
                txtSearch.Visible = true;
            }
            Session["RegistrationId"] = null;
            Session["EncounterId"] = null;
            Session["EncounterNo"] = null;
            Session["RegistrationNo"] = null;
            Session["EncounterDate"] = null;
            if (common.myStr(ViewState["PT"]) == "BC")
            {
                pnlAllButtons.Visible = false;
                btnBillClearance.Visible = true;
                btnBillClearance.CommandArgument = "Bill";
                btnPACClear.Visible = false;
                BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);
                ds = objphr.getStatus(common.myInt(Session["HospitalLocationID"]), "OT", "OT-CONF", 0);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["ConfStatusId"] = common.myInt(ds.Tables[0].Rows[0]["StatusId"]).ToString();
                }
            }
            else if (common.myStr(ViewState["PT"]) == "ICM")
            {
                pnlAllButtons.Visible = false;
            }
            else //if (common.myStr(ViewState["PT"]) == "PAC")
            {
                BaseC.HospitalSetup baseHs = new BaseC.HospitalSetup(sConString);
                string isPACReqInOTSchedule = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "IsPACRequiredOnOTAppointmentScreen", common.myInt(Session["FacilityId"]));
                if (common.myStr(isPACReqInOTSchedule) == "Y")
                {
                    btnPACClear.Visible = false;
                    txtClearanceRemarks.Visible = false;
                    Label3.Visible = false;
                }


                btnPACClear.Text = "PAC Clearance";
                pnlAllButtons.Visible = true;
                btnBillClearance.Visible = false;
                // btnBillClearance.CommandArgument = "PAC";


                string HideBloodRequisitionBtn = baseHs.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "HideBloodRequisitionBtn", common.myInt(Session["FacilityId"]));
                if (common.myStr(HideBloodRequisitionBtn) == "Y")
                {
                    btnBloodRequest.Visible = false;
                }

                BaseC.clsPharmacy objphr = new BaseC.clsPharmacy(sConString);
                ds = objphr.getStatus(common.myInt(Session["HospitalLocationID"]), "OT", "OT-CONF", 0);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["ConfStatusId"] = common.myInt(ds.Tables[0].Rows[0]["StatusId"]).ToString();
                }
            }
            dtpdate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            toDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);

            dtpdate.SelectedDate = System.DateTime.Now;
            toDate.SelectedDate = System.DateTime.Now;


            PopulateOTName();
            BindProvider();
            CompanyBind();
            BindGrid();
            if (common.myStr(Request.QueryString["For"]) == "T")
            {
                btnBillClearance.Visible = false;
                btnPACClear.Visible = false;
                btnChecklist.Visible = false;
                btnClinicaldetails.Visible = false;
                btnDetails.Visible = false;
                btnServiceRequisition.Visible = false;
                btnInvestigationchart.Visible = false;
            }
        }

    }
    void BindProvider()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.clsBb objEmr = new BaseC.clsBb(sConString);
            ds = objEmr.GetDoctorNames(common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlProvider.DataSource = ds.Tables[0];
                ddlProvider.DataTextField = "FullName";
                ddlProvider.DataValueField = "Id";
                ddlProvider.DataBind();
                ddlProvider.SelectedIndex = -1;
                ddlProvider.Items.Insert(0, new RadComboBoxItem("All", "0"));
                ddlProvider.SelectedValue = common.myStr(Session["EmployeeId"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void CompanyBind()
    {
        DataTable dtCompany = new DataTable();
        BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);
        dtCompany = baseEBill.getCompanyList(common.myInt(Session["HospitalLocationId"]), "A", 0,
            common.myInt(Session["FacilityId"])).Tables[0];
        if (dtCompany.Rows.Count > 0)
        {
            ddlcompany.DataSource = dtCompany;
            ddlcompany.DataTextField = "Name";
            ddlcompany.DataValueField = "CompanyId";
            ddlcompany.DataBind();
            ddlcompany.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
        }
    }

    protected void gvDetails_ItemCommand(object source, GridCommandEventArgs e)
    {
        int selectedIdx = e.Item.ItemIndex;

        if (e.CommandName == "ItemSelect")
        {

            gvDetails.Items[selectedIdx].Selected = true;
            //e.Item.Cells[0].BackColor= System.Drawing.Color.Yellow;
            //e.Item.Cells[1].BackColor = System.Drawing.Color.Yellow;
            //e.Item.Cells[2].BackColor = System.Drawing.Color.Yellow;



            HiddenField hdngvPayerType = (HiddenField)e.Item.FindControl("hdngvPayerType");
            HiddenField hdngvPayerId = (HiddenField)e.Item.FindControl("hdngvPayerId");
            HiddenField hdngvSponsorId = (HiddenField)e.Item.FindControl("hdngvSponsorId");
            HiddenField hdngvCardId = (HiddenField)e.Item.FindControl("hdngvCardId");
            HiddenField hdngvStatusCode = (HiddenField)e.Item.FindControl("hdngvStatusCode");

            //palendra
            HiddenField hdnOTDashBoardStatus = (HiddenField)e.Item.FindControl("hdnOTDashBoardStatus");
            //palendra
            Label txtAgeGender = (Label)e.Item.FindControl("txtAgeGender");
            Label txtBedNo = (Label)e.Item.FindControl("txtBedNo");
            Label lblOrderDate = (Label)e.Item.FindControl("lblOrderDate");
            Label txtStartTime = (Label)e.Item.FindControl("txtStartTime");
            Label txtSurgery = (Label)e.Item.FindControl("txtSurgery");

            // Label lblStatusColor = (Label)e.Item.FindControl("lblStatusColor");

            //e.Item.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));

            hdnpayerType.Value = hdngvPayerType.Value;
            hdnpayerId.Value = hdngvPayerId.Value;
            hdnSponsorId.Value = hdngvSponsorId.Value;
            hdnCardId.Value = hdngvCardId.Value;
            hdnCode.Value = hdngvStatusCode.Value;
            ViewState["BookingId"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtOTBookingID")).Text;
            Session["RegistrationId"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtRegistrationID")).Text;
            Session["EncounterId"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtEncounterID")).Text;
            Session["EncounterNo"] = ((Label)gvDetails.SelectedItems[0].FindControl("txIpNo")).Text;
            Session["RegistrationNo"] = ((Label)gvDetails.SelectedItems[0].FindControl("txRegistrationNo")).Text;
            Session["InvoiceId"] = 0;

            Session["OTPatientAgeGender"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtAgeGender")).Text;
            Session["OTPatientBedNo"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtBedNo")).Text;
            Session["OTPatientPostedDate"] = ((Label)gvDetails.SelectedItems[0].FindControl("lblOrderDate")).Text;
            Session["OTPatientStartTime"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtStartTime")).Text;
            Session["OTPatientSurgery"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtSurgery")).Text;

            BaseC.Patient patient = new BaseC.Patient(sConString);
            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myStr(Session["RegistrationNo"]), "", common.myInt(Session["UserId"]), 0);

            hdnIpno.Value = ((Label)gvDetails.SelectedItems[0].FindControl("txIpNo")).Text;
            hdnPatientname.Value = ((Label)gvDetails.SelectedItems[0].FindControl("txPatient")).Text;
            hdnWardno.Value = ((Label)gvDetails.SelectedItems[0].FindControl("txtWard")).Text;
            hdnSurgeryname.Value = ((Label)gvDetails.SelectedItems[0].FindControl("txtSurgery")).Text;
            hdnBedId.Value = ((Label)gvDetails.SelectedItems[0].FindControl("txtCurrentBedId")).Text;
            hdnBookinId.Value = ((Label)gvDetails.SelectedItems[0].FindControl("txtOTBookingID")).Text;
            hdnPACClearance.Value = ((Label)gvDetails.SelectedItems[0].FindControl("lblPACClearance")).Text;
            hdnBookingStatus.Value = ((Label)gvDetails.SelectedItems[0].FindControl("lblBookingStatus")).Text;
            Session["EncounterDate"] = ((HiddenField)gvDetails.SelectedItems[0].FindControl("hdnEncounterDate")).Value;
            SetGridColor();
            e.Item.Cells[3].BackColor = System.Drawing.Color.Yellow;
            e.Item.Cells[4].BackColor = System.Drawing.Color.Yellow;

            rdOTStatus.SelectedIndex = rdOTStatus.Items.IndexOf(rdOTStatus.Items.FindItemByValue(common.myStr(hdnOTDashBoardStatus.Value)));


            lblMessage.Text = "";
        }
        if (e.CommandName == "Estimate")
        {
            gvDetails.Items[selectedIdx].Selected = true;
            ViewState["RegistrationId"] = ((Label)gvDetails.SelectedItems[0].FindControl("txtRegistrationID")).Text;

            if (common.myInt(ViewState["RegistrationId"]) > 0)
            {
                RadWindowForNew.NavigateUrl = "/ATD/SearchCounselingDetails.aspx?RegId=" + common.myInt(ViewState["RegistrationId"]);
                RadWindowForNew.Height = 600;
                RadWindowForNew.Width = 1000;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Patient Record";
            }
        }

    }
    //protected void gvDetails_PreRender(object sender, EventArgs e)
    //{
    //    //if (status == false)
    //    BindGrid();
    //    //  SetGridColor();
    //}
    private void SetGridColor()
    {
        try
        {
            foreach (GridDataItem dataItem in gvDetails.MasterTableView.Items)
            {
                if (dataItem.ItemType == GridItemType.Item
                    || dataItem.ItemType == GridItemType.AlternatingItem)
                {
                    Label lblStatusColor = (Label)dataItem.FindControl("lblStatusColor");
                    dataItem.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
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
    private void PopulateOTName()
    {
        try
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();
            HashIn.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            HashIn.Add("@intFacilityID", common.myInt(Session["FacilityId"]));

            DataSet dt = dl.FillDataSet(CommandType.StoredProcedure, "uspGetOTs", HashIn);

            ddlotname.DataSource = dt.Tables[0];
            ddlotname.DataTextField = "TheatreName";
            ddlotname.DataValueField = "TheatreID";
            ddlotname.DataBind();
            ddlotname.Items.Insert(0, new RadComboBoxItem("All", "0"));
            ddlotname.Items[0].Value = "0";
            ddlotname.SelectedIndex = 0;


        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //protected void BindBlankgrid(int no)
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("OTBookingID");
    //    dt.Columns.Add("StartTime");
    //    dt.Columns.Add("EndTime");
    //    dt.Columns.Add("RegistrationNo");
    //    dt.Columns.Add("Patient");
    //    dt.Columns.Add("Surgery");
    //    dt.Columns.Add("Confirm");
    //    dt.Columns.Add("Remarks");
    //    dt.Columns.Add("BookingNo");
    //    dt.Columns.Add("IpNo");
    //    dt.Columns.Add("BillingStatus");
    //    dt.Columns.Add("Clearance");
    //    dt.Columns.Add("AgeGender");
    //    dt.Columns.Add("WardName");
    //    dt.Columns.Add("Company");
    //    dt.Columns.Add("RegistrationID");
    //    dt.Columns.Add("EncounterID");
    //    dt.Columns.Add("StatusColor");
    //    dt.Columns.Add("CurrentBedId");
    //    dt.Columns.Add("SurgeryId");


    //    DataRow dr = dt.NewRow();
    //    for (int i = 0; i < no; i++)
    //    {
    //        dr["OTBookingID"] = DBNull.Value;
    //        dr["StartTime"] = DBNull.Value;
    //        dr["EndTime"] = DBNull.Value;
    //        dr["RegistrationNo"] = DBNull.Value;
    //        dr["Patient"] = DBNull.Value;
    //        dr["Surgery"] = DBNull.Value;
    //        dr["Confirm"] = DBNull.Value;
    //        dr["Remarks"] = DBNull.Value;
    //        dr["BookingNo"] = DBNull.Value;
    //        dr["IpNo"] = DBNull.Value;
    //        dr["BillingStatus"] = DBNull.Value;
    //        dr["Clearance"] = DBNull.Value;
    //        dr["AgeGender"] = DBNull.Value;
    //        dr["WardName"] = DBNull.Value;
    //        dr["Company"] = DBNull.Value;
    //        dr["RegistrationID"] = DBNull.Value;
    //        dr["EncounterID"] = DBNull.Value;
    //        dr["StatusColor"] = DBNull.Value;
    //        dr["CurrentBedId"] = DBNull.Value;
    //        dr["SurgeryId"] = DBNull.Value;

    //    }
    //    dt.Rows.Add(dr);
    //    gvDetails.DataSource = dt;
    //    gvDetails.DataBind();
    //}

    protected void BindGrid()
    {
        try
        {
            objbc = new BaseC.clsOTBooking(sConString);
            DateTime dt = DateTime.Today;
            DataView dv = new DataView();
            ds = new DataSet();

            string chvRegistrationNo = "", chvEncounterNo = "", chvPatientName = "";
            bool chvVentilator = false;

            if (common.myStr(ddlName.SelectedValue) == "R")
            {
                chvRegistrationNo = txtSearchN.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "ENC")
            {
                chvEncounterNo = txtSearch.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "N")
            {
                chvPatientName = txtSearch.Text;
            }
            else if (common.myStr(ddlName.SelectedValue) == "V")
            {
                chvVentilator = common.myBool(rcbVentilator.SelectedValue);
            }

            ds = objbc.GetOtDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myInt(ddlotname.SelectedValue), common.myInt(ddlProvider.SelectedValue), dtpdate.SelectedDate.Value,
                toDate.SelectedDate.Value, chvRegistrationNo, chvEncounterNo, chvPatientName, chvVentilator, common.myBool(chkReExploration.Checked));

            if (ds.Tables[0].Rows.Count == 0)
            {
                ds.Tables[0].AcceptChanges();
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);
                dv = ds.Tables[0].DefaultView;
            }
            else if (common.myStr(ViewState["PT"]) == "BC")
            {
                dv = ds.Tables[0].DefaultView;
                if (common.myInt(ddlcompany.SelectedValue) > 0)
                {
                    dv.RowFilter = "(ISNULL(PayorId,0)=" + common.myInt(ddlcompany.SelectedValue) + " AND BookingStatus = 'Confir' And BillClearance ='') OR BillClearance ='Yes'";//BillClearance <> 'Yes'
                }
                else
                {
                    dv.RowFilter = "(BookingStatus = 'Confir' And BillClearance ='') OR BillClearance ='Yes'";//BillClearance <> 'Yes'
                }
            }
            else
            {
                dv = ds.Tables[0].DefaultView;

                if (common.myInt(ddlcompany.SelectedValue) > 0)
                {
                    dv.RowFilter = "ISNULL(PayorId,0)=" + common.myInt(ddlcompany.SelectedValue);
                }
            }
            gvDetails.DataSource = dv.ToTable();
            gvDetails.DataBind();
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
        string firstCurrentDate = "";
        string newCurrentDate = "";
        string currentdate = formatdate.FormatDateDateMonthYear(outputCurrentDate);
        string strformatCurrDate = formatdate.FormatDate(currentdate, "MM/dd/yyyy", "yyyy/MM/dd");
        firstCurrentDate = strformatCurrDate.Remove(4, 1);
        newCurrentDate = firstCurrentDate.Remove(6, 1);
        return newCurrentDate;
    }
    protected void toDate_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        if (Convert.ToInt32(FindCurrentDate(dtpdate.SelectedDate.Value.ToString("dd/MM/yyyy"))) >
            Convert.ToInt32(FindCurrentDate(toDate.SelectedDate.Value.ToString("dd/MM/yyyy"))))
        {
            toDate.SelectedDate = System.DateTime.Now;
            Alert.ShowAjaxMsg("Please select valid date", Page);
            return;
        }
        status = true;

    }

    protected void btnMedicalIllustration_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {
            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }

            lblMessage.Text = "";

            if (common.myStr(hdnIpno.Value).Length > 3)
            {
                Session["OPIP"] = "I";
            }
            else
            {
                Session["OPIP"] = "O";
            }

            RadWindowForNew.NavigateUrl = "/Editor/MedicalIllustration.aspx?From=POPUP";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1100;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";

        }
    }

    protected void btnClinicaldetails_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {
            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }

            lblMessage.Text = "";
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            int intFormId = objPatient.GetPatientDefaultFormId(common.myInt(Session["EncounterId"]), Convert.ToInt16(Session["HospitalLocationID"]));
            if (intFormId > 0)
            {
                Session["formId"] = Convert.ToString(intFormId);
            }
            if (common.myStr(hdnIpno.Value).Length > 3)
            {
                Session["OPIP"] = "I";
            }
            else
            {
                Session["OPIP"] = "O";
            }

            RadWindowForNew.NavigateUrl = "/EMR/Templates/Default.aspx?Type=OT&From=POPUP";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1100;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";

        }
    }

    protected void gvDetails_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = e.Item as GridDataItem;

            Label lblBookingStatus = (Label)item.FindControl("lblBookingStatus");
            Label lblStatusColor = (Label)item.FindControl("lblStatusColor");
            Label lblBillClearance = (Label)item.FindControl("lblBillClearance");
            Label lblPACClearance = (Label)item.FindControl("lblPACClearance");
            //Label lblClearance = (Label)item.FindControl("lblClearance");

            //if (common.myInt(htnBillClearance.Value) == 1)
            //{
            //    lblClearance.Text = "YES";
            //}
            //else
            //{
            //    lblClearance.Text = "NO";
            //}

            //item.BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));

            for (int index = 0; index < item.Cells.Count; index++)
            {
                item.Cells[index].BackColor = System.Drawing.Color.FromName(common.myStr(lblStatusColor.Text));
            }


            if (common.myStr(ViewState["PT"]) == "BC")
            {
                LinkButton lnkSelect = (LinkButton)item.FindControl("lnkSelect");
                lnkSelect.Visible = false;
                if (common.myStr(lblBookingStatus.Text) == "Confir") //UnConf, Confir, Chk-in, Chk-out
                {

                    lnkSelect.Visible = true;
                }
            }
            // //common.myStr(lblBookingStatus.Text) == "Confir" || 
            //else if ((common.myStr(lblBookingStatus.Text) == "Chk-in") && common.myStr(lblPACClearance.Text) != "Yes") //UnConf, Confir, Chk-in, Chk-out
            //{
            //    LinkButton lnkSelect = (LinkButton)item.FindControl("lnkSelect");
            //    CheckBox chkRow = (CheckBox)item.FindControl("chkRow");

            //    //lnkSelect.Visible = false;
            //    chkRow.Visible = true;
            //}

        }
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        //BindGrid();
        SetGridColor();
    }
    protected void btnChecklist_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }
            lblMessage.Text = "";
            // RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?IpNo=" + hdnIpno.Value + "&Pname=" + hdnPatientname.Value + "&Surgery=" + hdnSurgeryname.Value.Replace("&", "@") + "&ward=" + hdnWardno.Value + "&BookingId=" + hdnBookinId.Value + "&BedId=" + hdnBedId.Value + "&SurgeryId=" + hdnSurgeryId.Value + "  ";
            RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?Surgery=" + hdnSurgeryname.Value.Replace("&", "@") + "&ward=" + hdnWardno.Value + "&BookingId=" + hdnBookinId.Value + "&BedId=" + hdnBedId.Value + "&SurgeryId=" + hdnSurgeryId.Value + "  ";
            RadWindowForNew.Height = 650;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";

        }

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnDetails_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }
            lblMessage.Text = "";
            RadWindowForNew.NavigateUrl = "~/OTScheduler/ViewPtientdetails.aspx?BookingId=" + ViewState["BookingId"].ToString();
            RadWindowForNew.Height = 350;
            RadWindowForNew.Width = 500;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";

        }

    }
    protected void btnInvestigationchart_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }
            lblMessage.Text = "";

            RadWindowForNew.NavigateUrl = "~/LIS/Phlebotomy/InvestigationResultV1.aspx?OT=OT";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 1000;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";
        }
    }
    protected void btnTagPatient_OnClick(object sender, EventArgs e)
    {
        if (common.myStr(Session["RegistrationNo"]) == "" || common.myStr(hdnIpno.Value) == "")
        {
            if (common.myStr(ViewState["BookingId"]) != "")
            {
                lblMessage.Text = "";
                // RadWindowForNew.NavigateUrl = "~/OTScheduler/OTChecklist.aspx?IpNo=" + hdnIpno.Value + "&Pname=" + hdnPatientname.Value + "&Surgery=" + hdnSurgeryname.Value.Replace("&", "@") + "&ward=" + hdnWardno.Value + "&BookingId=" + hdnBookinId.Value + "&BedId=" + hdnBedId.Value + "&SurgeryId=" + hdnSurgeryId.Value + "  ";
                RadWindowForNew.NavigateUrl = "~/OTScheduler/TagPatient.aspx?FromPage=OT&BookingId=" + hdnBookinId.Value;
                RadWindowForNew.Height = 450;
                RadWindowForNew.Width = 900;
                RadWindowForNew.Top = 40;
                RadWindowForNew.Left = 100;
                RadWindowForNew.OnClientClose = "OnClientClose";
                RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                RadWindowForNew.Modal = true;
                RadWindowForNew.VisibleStatusbar = false;
            }
            else
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Patient !";

            }
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select un-registered or non admitted patient for tagging !";
        }

    }
    protected void btnCaseSheet_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }

            RadWindowForNew.NavigateUrl = "/Editor/VisitHistory.aspx?FromWard=Y&Category=PopUp&Type=OT";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 990;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";//; //"SearchPatientOnClientClose";//
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.Title = "Case sheet";
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";
        }

    }
    protected void btnConsentForm_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }


            RadWindowForNew.NavigateUrl = "/EMR/Templates/TemplateNotesPrint.aspx?From=POPUP&OT=2&RegistrationNo=" + common.myInt(Session["RegistrationNo"]);
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";//
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";
        }
    }
    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        //if (common.myInt(Session["FacilityID"]) == 0)
        //{
        //    strmsg += "Facility not selected !";
        //    isSave = false;
        //}

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void btnBillClearance_Click(object sender, EventArgs e)
    {
        objwcf = new BaseC.RestFulAPI(sConString);
        try
        {
            if (common.myStr(hdnBookinId.Value) != "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "";

                if (!isSaved())
                {
                    return;
                }

                strXML = new StringBuilder();
                coll = new ArrayList();
                if (common.myStr(txtClearanceRemarks.Text) == "")
                {
                    lblMessage.Text = "Enter remarks before clearance !";
                    return;
                }
                string sPACClearance = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                            common.myInt(Session["FacilityId"]), "PACClearance", sConString);
                if (sPACClearance == "Y")
                {
                    if ((common.myStr(hdnBookingStatus.Value) != "Confir" && common.myStr(hdnBookingStatus.Value) != "Chk-in"))
                    {
                        lblMessage.Text = "Please confirm booking first from OT scheduler.....";
                        return;
                    }
                    else if ((common.myStr(hdnPACClearance.Value) == ""))
                    {
                        lblMessage.Text = "PAC Clearance pending...";
                        return;
                    }
                    else if ((common.myStr(hdnBookingStatus.Value) == ""))
                    {
                        lblMessage.Text = "Bill Clearance already done...";
                        return;
                    }
                    else if ((common.myStr(hdnBookingStatus.Value) == "Confir" || common.myStr(hdnBookingStatus.Value) == "Chk-in") && (common.myStr(hdnPACClearance.Value) == "Yes"))
                    {

                    }
                }
                coll.Add(common.myInt(hdnBookinId.Value));
                coll.Add(1);
                strXML.Append(common.setXmlTable(ref coll));

                if (strXML.ToString() == "")
                {
                    lblMessage.Text = "Please select booking first ...";
                    return;
                }

                string strMsg = objwcf.SaveOTBookingBillClearance(common.myInt(Session["HospitalLocationID"]), strXML.ToString(), common.myInt(Session["UserID"]), "Bill",
                    common.myStr(txtClearanceRemarks.Text));
                lblMessage.Text = strMsg;
                if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
                {
                    BindGrid();
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                }
                else
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnPACClear_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(hdnBookinId.Value) != "")
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "";

                if (!isSaved())
                {
                    return;
                }

                strXML = new StringBuilder();
                coll = new ArrayList();
                if (common.myStr(txtClearanceRemarks.Text) == "")
                {
                    lblMessage.Text = "Enter remarks before clearance !";
                    return;
                }
                else if ((common.myStr(hdnBookingStatus.Value) != "Confir" && common.myStr(hdnBookingStatus.Value) != "Chk-in"))
                {
                    lblMessage.Text = "Please confirm booking first from OT scheduler...";
                    return;
                }
                else if ((common.myStr(hdnBookingStatus.Value) == "Confir" || common.myStr(hdnBookingStatus.Value) == "Chk-in") && (common.myStr(hdnPACClearance.Value) == "Yes"))
                {
                    lblMessage.Text = "PAC Clearance already done...";
                    return;
                }
                else if ((common.myStr(hdnBookingStatus.Value) == "Confir" || common.myStr(hdnBookingStatus.Value) == "Chk-in") && (common.myStr(hdnPACClearance.Value) == ""))
                {
                    coll.Add(common.myInt(hdnBookinId.Value));
                    coll.Add(1);
                    strXML.Append(common.setXmlTable(ref coll));

                    if (strXML.ToString() == "")
                    {
                        lblMessage.Text = "Please confirm booking first from OT scheduler...";
                        return;
                    }
                    objwcf = new BaseC.RestFulAPI(sConString);
                    string strMsg = objwcf.SaveOTBookingBillClearance(common.myInt(Session["HospitalLocationID"]), strXML.ToString(), common.myInt(Session["UserID"]), "PAC", common.myStr(txtClearanceRemarks.Text));

                    if ((strMsg.Contains(" Update") || strMsg.Contains(" Save")) && !strMsg.Contains("usp"))
                    {
                        BindGrid();
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = strMsg;
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
    protected void btnServiceRequisition_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }
            RadWindowForNew.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?Regid=" + common.myStr(Session["RegistrationId"])
             + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myStr(Session["EncounterId"])
             + "&EncNo=" + "" + "&OP_IP=I&CompanyId=" + common.myInt(hdnpayerId.Value)
             + "&InsuranceId=" + common.myInt(hdnSponsorId.Value) + "&CardId=" + common.myInt(hdnCardId.Value)
             + "&PayerType=" + hdnpayerType.Value + "&BType=" + "" + "&From=BILL";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";//
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";
        }
    }
    protected void btnBloodRequest_Click(object sender, EventArgs e)
    {
        if (common.myStr(ViewState["BookingId"]) != "")
        {

            if (common.myInt(Session["EncounterId"]) == 0)
            {
                SetGridColor();
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Encounter not generated for this patient !";
                return;
            }
            RadWindowForNew.NavigateUrl = "~/BloodBank/SetupMaster/ComponentRequisition.aspx?Regid=" + common.myStr(Session["RegistrationId"])
             + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myStr(Session["EncounterId"])
             + "&EncNo=" + hdnIpno.Value + "&MP=NO";


            // RadWindowForNew.NavigateUrl = "/BloodBank/SetupMaster/ComponentRequisition.aspx?Regid=" + common.myStr(Session["RegistrationId"])
            //+ "&RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&EncId=" + common.myStr(Session["EncounterId"])
            //+ "&EncNo=" + "" + "&OP_IP=I&CompanyId=" + common.myInt(hdnpayerId.Value)
            //+ "&InsuranceId=" + common.myInt(hdnSponsorId.Value) + "&CardId=" + common.myInt(hdnCardId.Value)
            //+ "&PayerType=" + hdnpayerType.Value + "&BType=" + "" + "&From=BILL";


            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.OnClientClose = "OnClientClose";//
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            SetGridColor();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient !";
        }
    }
    protected void ddlName_OnTextChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        txtSearchN.Text = "";

        txtSearchN.Visible = false;
        txtSearch.Visible = false;

        if (common.myStr(ddlName.SelectedValue) == "V")
        {
            txtSearchN.Visible = false;
            txtSearch.Visible = false;
            rcbVentilator.Visible = true;
        }
        else if (common.myStr(ddlName.SelectedValue) == "R")
        {
            txtSearchN.Visible = true;
            txtSearch.Visible = false;
            rcbVentilator.Visible = false;
        }
        else
        {
            txtSearchN.Visible = false;
            txtSearch.Visible = true;
            rcbVentilator.Visible = false;
        }
        // bindStatus(rblSearchCriteria.SelectedValue);
    }

    protected void btnBloodReturn_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/BloodBank/SetupMaster/ComponentReturn.aspx?MP=NO&Regid=" + common.myInt(Session["RegistrationId"]) +
                                            "&RegNo=" + common.myStr(Session["RegistrationNo"]) +
                                            "&EncId=" + common.myInt(Session["EncounterId"]) +
                                            "&EncNo=" + common.myStr(Session["EncounterNo"]) +
                                            "&IsPasswordRequired=N";
        RadWindowForNew.Height = 610;
        RadWindowForNew.Width = 960;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = string.Empty; //"SearchPatientOnClientClose";//
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindowForNew.Title = "Blood Component Return";
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnShowHideInDashBoard_Click(object sender, EventArgs e)
    {
        //int ISShowHideInOTDisplay = 0;
        //if (common.myInt(hdnISShowHideInOTDisplay.Value) == 1 || common.myStr(hdnISShowHideInOTDisplay.Value) == "True")
        //{
        //    ISShowHideInOTDisplay = 0;
        //}
        //else
        //{
        //    ISShowHideInOTDisplay = 1;
        //}
        objbc = new BaseC.clsOTBooking(sConString);
        string strMsg = objbc.ShowAndHideOTDisplay(common.myStr(hdnBookinId.Value), common.myInt(Session["UserId"]), common.myInt(rdOTStatus.SelectedValue));

        if (strMsg.Contains("Succeeded"))
        {
            lblMessage.Text = "Record Saved";
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            BindGrid();
            //btnShowHideInDashBoard.Visible = false;
            //rdOTStatus.Visible = false;

        }
    }
}
