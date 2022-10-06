using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Configuration;

public partial class WardManagement_DischargeSummary : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.clsEMRBilling objVal;
    clsExceptionLog objException = new clsExceptionLog();

    private enum GridEncounter : byte
    {
        Select = 2,
        OPIP = 3,
        RegistrationNo = 4,
        EncounterNo = 5,
        Name = 6,
        AgeGender = 7,
        DoctorName = 8,
        CurrentBedNo = 9,
        EncDate = 10,
        DischargeStatus = 11,
        CompanyName = 13,
        PhoneHome = 14,
        MobileNo = 15,
        DOB = 16,
        Address = 17,
        REGID = 18,
        ENCID = 19,
        CompanyCode = 20,
        InsuranceCode = 21,
        CardId = 22,
        RowNo = 23,
        EncounterStatus = 12
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["OPIP"] = "I";
            ViewState["RegEnc"] = "1";
            ViewState["SearchOn"] = "0";

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            txtFromDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            hdnRegistrationId.Value = "";
            hdnRegistrationNo.Value = "";
            hdnEncounterId.Value = "";
            hdnEncounterNo.Value = "";
            hdnCompanyCode.Value = "";
            hdnInsuranceCode.Value = "";
            hdnCardId.Value = "";
            hdnEncounterDate.Value = "";

            hdnAgeGender.Value = "";
            hdnPhoneHome.Value = "";
            hdnMobileNo.Value = "";
            hdnPatientName.Value = "";
            hdnDOB.Value = "";
            hdnAddress.Value = "";
            hdnFacilityId.Value = "";

            objVal = new BaseC.clsEMRBilling(sConString);
            bindControl();

            if (common.myStr(ViewState["SearchOn"]) != "")
            {
                if (common.myInt(ViewState["SearchOn"]) == 0)
                {
                    ddlSearchOn.SelectedValue = "0";
                }
                else if (common.myInt(ViewState["SearchOn"]) == 1)
                {
                    ddlSearchOn.SelectedValue = "1";
                }
            }

            if (common.myStr(ViewState["OPIP"]) == "O")
            {
                ddlSearchOn.SelectedValue = "2";
            }

            if (Convert.ToString(ViewState["SalType"]) == "IP")
            {
                ViewState["OPIP"] = "I";
            }
            else if (Convert.ToString(ViewState["SalType"]) == "OP")
            {
                ViewState["OPIP"] = "O";
            }
            CreateTable();
            bindData("F", 0);
        }
    }
    private void bindControl()
    {
        try
        {
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private void clearControl()
    {
        txtSearch.Text = "";
        lblMessage.Text = "&nbsp;";
        ddlTime.SelectedIndex = 0;
    }

    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData("F", 0);
    }

    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        txtSearch.Text = "";
        lblMessage.Text = "";
        ddlSearchOn.SelectedIndex = 0;
    }

    protected void gvEncounter_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridPagerItem)
        {
            GridPagerItem pager = (GridPagerItem)e.Item;
            Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
            lbl.Visible = false;

            RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
            combo.Visible = false;
        }

        if (e.Item is GridHeaderItem || e.Item is GridDataItem)
        {
            //e.Item.Cells[3].Visible = false;
            //e.Item.Cells[Convert.ToByte(GridEncounter.KinName)].Visible = false;
            if (e.Item is GridHeaderItem)
            {
                //if (rdoRegEnc.SelectedValue == "2")
                //{
                //e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Text = "Discharge Date";
                //}
            }
            //if (rdoRegEnc.SelectedValue == "2")
            //{
            e.Item.Cells[Convert.ToByte(GridEncounter.EncounterStatus)].Visible = false;
            //}
            if (common.myStr(ViewState["OPIP"]) == "I")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.OPIP)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
            }
            if (common.myStr(ViewState["OPIP"]) == "O")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.OPIP)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.EncounterStatus)].Visible = false;
            }
        }
        if (e.Item is GridDataItem)
        {
            Label lblName = (Label)e.Item.FindControl("lblName");
            //Label lblPatientAddress = (Label)e.Item.FindControl("lblPatientAddress");
            HiddenField hdnPatientAddress = (HiddenField)e.Item.FindControl("hdnPatientAddress");
            HiddenField hdnKinName = (HiddenField)e.Item.FindControl("hdnKinName");

            e.Item.Attributes.Add("onclick", "javascript:ShowPatientDetails('" + lblName.ClientID + "','" + hdnPatientAddress.ClientID + "','" + hdnKinName.ClientID + "');");
        }
    }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;
        bindData("F", 0);
    }

    protected void gvEncounter_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Select")
            {
                if (common.myInt(((Label)e.Item.FindControl("lblREGID")).Text) > 0)
                {
                    hdnRegistrationId.Value = common.myStr(((Label)e.Item.FindControl("lblREGID")).Text);
                    hdnRegistrationNo.Value = common.myStr(((Label)e.Item.FindControl("lblRegistrationNo")).Text);
                    hdnEncounterId.Value = common.myStr(((Label)e.Item.FindControl("lblENCID")).Text);
                    hdnEncounterNo.Value = common.myStr(((Label)e.Item.FindControl("lblEncounterNo")).Text).Replace("&nbsp;", "");
                    hdnEncounterDate.Value = common.myStr(((Label)e.Item.FindControl("lblEncDate")).Text);
                    hdnAgeGender.Value = common.myStr(((Label)e.Item.FindControl("lblEncDate")).Text);
                    hdnDoctorName.Value = common.myStr(((Label)e.Item.FindControl("lblDoctorName")).Text);
                    hdnCurrentBedNo.Value = common.myStr(((Label)e.Item.FindControl("lblCurrentBedNo")).Text);
                    hdnMobileNo.Value = common.myStr(((Label)e.Item.FindControl("lblMobileNo")).Text);
                    //hdnCompanyCode.Value = common.myStr(((Label)e.Item.FindControl("lblCompanyCode")).Text);
                    //hdnInsuranceCode.Value = common.myStr(((Label)e.Item.FindControl("lblInsuranceCode")).Text);
                    //hdnCardId.Value = common.myStr(((Label)e.Item.FindControl("lblCardId")).Text);
                    //hdnEncounterDate.Value = common.myStr(((Label)e.Item.FindControl("lblEncDate")).Text);

                    //hdnAgeGender.Value = common.myStr(((Label)e.Item.FindControl("lblGenderAge")).Text);
                    //hdnPhoneHome.Value = common.myStr(((Label)e.Item.FindControl("lblPhoneHome")).Text);
                    //hdnMobileNo.Value = common.myStr(((Label)e.Item.FindControl("lblMobileNo")).Text);
                    //hdnPatientName.Value = common.myStr(((Label)e.Item.FindControl("lblName")).Text);
                    //hdnDOB.Value = common.myStr(((Label)e.Item.FindControl("lblDOB")).Text);
                    //hdnAddress.Value = common.myStr(((Label)e.Item.FindControl("lblPatientAddress")).Text);
                    //hdnFacilityId.Value = common.myStr(Session["FacilityId"]);

                    Session["OPIP"] = "I";
                    Session["EncounterId"] = common.myInt(hdnEncounterId.Value);
                    Session["RegistrationId"] = common.myInt(hdnRegistrationId.Value);
                    Session["EncounterDate"] = common.myStr(hdnEncounterDate.Value);
                    Session["RegistrationNo"] = common.myInt(hdnRegistrationNo.Value);
                    Session["Regno"] = common.myStr(hdnRegistrationNo.Value);
                    Session["Encno"] = common.myStr(hdnEncounterNo.Value);

                    BaseC.Patient patient = new BaseC.Patient(sConString);
                    DataSet dsPatient = new DataSet();

                    dsPatient = patient.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                            common.myStr(hdnRegistrationNo.Value), common.myStr(hdnEncounterNo.Value), common.myInt(Session["UserId"]), 0);

                    Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
                    Session["PatientDetailString"] = null;
                    Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = dsPatient;

                    Session["PatientDetailString"] = "&nbsp;Patient Name:&nbsp;<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnAgeGender.Value) + "</span>"
            + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnRegistrationNo.Value) + "</span>"
            + "&nbsp;Enc #.:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnEncounterNo.Value) + "</span>"
            + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnDoctorName.Value) + "</span>&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnEncounterDate.Value) + "</span>"
            + "&nbsp;Bed No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnCurrentBedNo.Value) + "</span>"
            + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'></span>"
            + "&nbsp;Mobile No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(hdnMobileNo.Value) + "</span>"
            + "&nbsp;Company:<span style='color: #990066;font-weight: bold;'>"
            + "</b>";
                    BaseC.ATD Objstatus = new BaseC.ATD(sConString);
                    DataSet ds = new DataSet();
                    ds = Objstatus.GetRegistrationDS(common.myInt(hdnRegistrationNo.Value.Trim()));
                    DateTime adminsiondate = DateTime.Now;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        adminsiondate = common.myDate(ds.Tables[0].Rows[0]["AdmissionDate"]);
                        Session["FollowUpDoctorId"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
                        Session["FollowUpRegistrationId"] = hdnRegistrationId.Value;
                        Session["DoctorId"] = ds.Tables[0].Rows[0]["DoctorId"].ToString();
                    }

                    if (chkDeathSummary.Checked)
                    {
                        RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?For=DthSum&Master=NO&RegId=" + common.myInt(Session["RegistrationId"])
                      + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                      + "&EncId=" + common.myInt(hdnEncounterId.Value)
                      + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                       + "&AdmissionDate=" + (adminsiondate).ToString("MM/dd/yyyy")
                      + "&AdmDId=" + common.myStr(Session["DoctorId"]);
                    }
                    else
                    {
                        RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(Session["RegistrationId"])
                      + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                      + "&EncId=" + common.myInt(hdnEncounterId.Value)
                      + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                       + "&AdmissionDate=" + (adminsiondate).ToString("MM/dd/yyyy")
                      + "&AdmDId=" + common.myStr(Session["DoctorId"]);
                    }

                    //RadWindow1.NavigateUrl = "~/ICM/DischargeSummary.aspx?Master=NO&RegId=" + common.myInt(Session["RegistrationId"])
                    //    + "&RegNo=" + common.myStr(hdnRegistrationNo.Value)
                    //    + "&EncId=" + common.myInt(hdnEncounterId.Value)
                    //    + "&EncNo=" + common.myStr(hdnEncounterNo.Value)
                    //     + "&AdmissionDate=" + (adminsiondate).ToString("MM/dd/yyyy")
                    //    + "&AdmDId=" + common.myStr(Session["DoctorId"]);

                    RadWindow1.Height = 590;
                    RadWindow1.Width = 1200;
                    RadWindow1.Top = 10;
                    RadWindow1.Left = 10;
                    RadWindow1.OnClientClose = ""; //"SearchPatientOnClientClose";//
                    RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindow1.Modal = true;
                    RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
                    // RadWindow1.Behaviors = WindowBehaviors.Maximize ;
                    RadWindow1.VisibleStatusbar = false;
                    RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    //return;
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
    private void bindData(string RecordButton, int RowNo)
    {
        try
        {
            setDate();

            txtSearch.Visible = true;
            txtRegNo.Visible = false;

            objVal = new BaseC.clsEMRBilling(sConString);

            string BedNo = "";
            string EncNo = "";
            string RegNo = "";
            string PatientName = "";
            string PhoneNo = "";
            string Mobile = "";

            switch (common.myInt(ddlSearchOn.SelectedValue))
            {
                case 1: // EncNo
                    EncNo = common.myStr(txtSearch.Text);
                    break;
                case 2: // RegNo
                    txtSearch.Visible = false;
                    txtRegNo.Visible = true;
                    RegNo = common.myStr(txtRegNo.Text);
                    break;
                case 3: // BedNo
                    BedNo = common.myStr(txtSearch.Text);
                    break;
                case 4: // PatientName
                    PatientName = common.myStr(txtSearch.Text);
                    break;
                case 5: // Phone
                    PhoneNo = common.myStr(txtSearch.Text);
                    break;
                case 6: // Mobile
                    Mobile = common.myStr(txtSearch.Text);
                    break;
            }


            DataSet dsSearch = objVal.getOPIPRegEncDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                    common.myStr(ViewState["OPIP"]), 0, 0, 2, RegNo, EncNo,
                                    common.escapeCharString(PatientName, false), common.myDate(txtFromDate.SelectedDate), common.myDate(txtToDate.SelectedDate),
                                    RecordButton, RowNo, common.myInt(gvEncounter.PageSize), common.myInt(Session["UserId"]),
                                    gvEncounter.CurrentPageIndex + 1, common.myStr(BedNo), 0, "", "", PhoneNo, Mobile, "", "", "", "", "", 0, 
                                    string.Empty, string.Empty, string.Empty, 0, 0, 0, false, string.Empty,0);

            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                gvEncounter.VirtualItemCount = Convert.ToInt32(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]);

            }
            else
            {
                gvEncounter.VirtualItemCount = 0;
            }
            if (dsSearch.Tables[0].Rows.Count > 0)
            {
                gvEncounter.DataSource = dsSearch.Tables[0];
            }
            else
            {
                DataRow DR = dsSearch.Tables[0].NewRow();
                dsSearch.Tables[0].Rows.Add(DR);

                gvEncounter.DataSource = dsSearch.Tables[0];
            }
            lblTotRecord.Text = "Total Record(s) Found - " + common.myStr(common.myInt(dsSearch.Tables[0].Rows[0]["TotalRecordsCount"]));
            gvEncounter.DataBind();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlTime_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindData("F", 0);
    }

    void setDate()
    {
        try
        {
            tblDate.Visible = false;

            switch (common.myStr(ddlTime.SelectedValue))
            {
                case "All":
                    txtFromDate.SelectedDate = common.myDate("01-01-1980");
                    txtToDate.SelectedDate = common.myDate("2099-12-31");
                    break;
                case "Today":
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastWeek":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-7);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastTwoWeeks":
                    txtFromDate.SelectedDate = DateTime.Now.AddDays(-14);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastOneMonth":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastThreeMonths":
                    txtFromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "LastYear":
                    txtFromDate.SelectedDate = DateTime.Now.AddYears(-1);
                    txtToDate.SelectedDate = DateTime.Now;
                    break;
                case "DateRange":

                    tblDate.Visible = true;

                    break;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("RegistrationNo");
        dt.Columns.Add("Name");
        dt.Columns.Add("EncounterNo");
        dt.Columns.Add("EncDate");
        dt.Columns.Add("OPIP");
        dt.Columns.Add("REGID");
        dt.Columns.Add("ENCID");
        dt.Columns.Add("CompanyCode");
        dt.Columns.Add("InsuranceCode");
        dt.Columns.Add("CardId");
        dt.Columns.Add("RowNo");
        dt.Columns.Add("GenderAge");
        dt.Columns.Add("DoctorName");
        dt.Columns.Add("PhoneHome");
        dt.Columns.Add("MobileNo");
        dt.Columns.Add("DOB");
        dt.Columns.Add("PatientAddress");
        dt.Columns.Add("CompanyName");
        dt.Columns.Add("CurrentBedNo");
        dt.Columns.Add("KinName");
        dt.Columns.Add("DischargeStatus");
        dt.Columns.Add("EncounterStatus");
        dt.Columns.Add("Status");
        dt.Columns.Add("DischargeDate");
        dt.Columns.Add("FinalizeDate");



        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        gvEncounter.DataSource = dt;
        gvEncounter.DataBind();
    }

    protected void ddlSearchOn_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRegNo.Text = "";
        txtSearch.Text = "";
        bindData("F", 0);

    }


}
