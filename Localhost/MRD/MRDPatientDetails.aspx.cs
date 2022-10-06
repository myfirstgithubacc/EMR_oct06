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
using System.Data.SqlClient;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Web.SessionState;
using Telerik.Web.UI;


public partial class Pharmacy_PatientDetails : System.Web.UI.Page
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
        EncounterStatus = 12,
        RegistrationNoOld = 24,
        MotherName = 25,
        FatherName = 26
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Font.Bold = commonLabelSetting.cBold;
            if (commonLabelSetting.cFont != "")
            {
                lblMessage.Font.Name = commonLabelSetting.cFont;
            }
            dtpFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
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

            bindControl();

            gvEncounter.PageSize = 10;
            rdoRegEnc.SelectedValue = "3";
            //if (common.myInt(Request.QueryString["RegEnc"]) == 1)
            //{
            //    rdoRegEnc.SelectedValue = "1";
            //    rdoRegEnc.Items[1].Text = "Admission";
            //    gvEncounter.PageSize = 10;
            //}
            //else if (common.myInt(Request.QueryString["RegEnc"]) == 2)
            //{
            //    rdoRegEnc.SelectedValue = "2";
            //    rdoRegEnc.Items[2].Text = "Discharge";
            //    gvEncounter.PageSize = 10;
            //}
            //else if (common.myInt(Request.QueryString["RegEnc"]) == 0)
            //{
            //    rdoRegEnc.SelectedValue = "2";
            //    gvEncounter.PageSize = 10;
            //    txtBedNo.Enabled = false;
            //    txtEncounterNo.Enabled = false;
            //}
            //else if (common.myInt(Request.QueryString["RegEnc"]) == 4)
            //{
            //    rdoRegEnc.SelectedValue = "4";
            //    //rdoRegEnc.Items[2].Text = "Emergency";
            //    gvEncounter.PageSize = 10;
            //    rdoRegEnc.Enabled = false;
            //}
            //if (common.myStr(Request.QueryString["SearchOn"]) != "")
            //{
            //    if (common.myInt(Request.QueryString["SearchOn"]) == 0)
            //    {
            //        rdoRegEnc.SelectedValue = "0";

            //        rdoRegEnc.Items[1].Enabled = false;
            //        gvEncounter.PageSize = 10;
            //    }
            //    else if (common.myInt(Request.QueryString["SearchOn"]) == 1)
            //    {
            //        //rdoRegEnc.SelectedValue = "1";

            //        gvEncounter.PageSize = 10;
            //    }
            //}

            //if (common.myStr(Request.QueryString["OPIP"]) == "O")
            //{

            //    rdoRegEnc.SelectedValue = "0";
            //    rdoRegEnc.Enabled = false;

            //}
            if (Request.QueryString["Source"] != null)
            {
                if (common.myStr(Request.QueryString["Source"]).Equals("BloodBank"))
                {
                    rdoRegEnc.SelectedValue = "1";
                    rdoRegEnc_OnSelectedIndexChanged(sender, e);
                }
            }
            if (Convert.ToString(Request.QueryString["SalType"]) == "IP")
            {

                ViewState["OPIP"] = "I";
            }
            else if (Convert.ToString(Request.QueryString["SalType"]) == "OP")
            {

                ViewState["OPIP"] = "O";
            }
            //if (Convert.ToString(Request.QueryString["Gender"]) == "1")
            //{
            //    dropSex.SelectedValue = "1";
            //    dropSex.Enabled = false;

            //}
            //else
            //{
            //    dropSex.SelectedValue = "0";
            //    dropSex.Enabled = true;

            //}
            CreateTable();
            rdoSearch.SelectedValue = "1";
            rdoSearch_SelectedIndexChanged(null, null);
            bindData("F", 0);
            FillEntrySite();

            ViewState["isDiagnosticSeriesSame"] = "";
            ViewState["DiagnosticColor"] = "";

            string isDiagnosticSeriesSame = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
            common.myInt(Session["FacilityId"]), "isDiagnosticSeriesSame", sConString);

            ViewState["isDiagnosticSeriesSame"] = isDiagnosticSeriesSame;

            if (common.myStr(ViewState["isDiagnosticSeriesSame"]) == "N")
            {
                objVal = new BaseC.clsEMRBilling(sConString);
                DataSet ds = objVal.getExternalPatientStatus(common.myInt(Session["HospitalLocationID"]), "ExternalPatient", "Ext");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["DiagnosticColor"] = common.myStr(ds.Tables[0].Rows[0]["StatusColor"]);
                }
                else
                {
                    ViewState["DiagnosticColor"] = "";
                }
            }

        }
    }
    private void FillEntrySite()
    {
        try
        {
            BaseC.clsEMRBilling obj = new BaseC.clsEMRBilling(sConString);
            int FacilityId;
            FacilityId = common.myInt(Session["FacilityID"]);
            DataSet ds = obj.getEntrySite(Convert.ToInt16(Session["UserID"]), FacilityId);
            ddlEntrySite.DataSource = ds.Tables[0];
            ddlEntrySite.DataValueField = "ESId";
            ddlEntrySite.DataTextField = "ESName";
            ddlEntrySite.DataBind();
            ddlEntrySite.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }

    }
    private void bindControl()
    {
        try
        {
            BaseC.EMRMasters.EMRFacility objEMRFacility = new BaseC.EMRMasters.EMRFacility(sConString);
            DataSet ds = objEMRFacility.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
            DataView dv;
            dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "Active = 1 ";
            ddlLocation.DataSource = dv;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "FacilityID";
            ddlLocation.DataBind();
            ddlLocation.SelectedIndex = ddlLocation.Items.IndexOf(ddlLocation.Items.FindByValue(common.myStr(Session["FacilityId"])));
            ListItem lst = new ListItem();
            bool tf = true;
            bool tEncounter = true;
            if (common.myInt(Request.QueryString["RegEnc"]) == 1)
            {
                tf = false;
            }
            if (common.myInt(Request.QueryString["RegEnc"]) == 0)
            {
                tEncounter = false;
            }
            if (common.myStr(Request.QueryString["PageFrom"]) == "MRD")
            {
                tf = false;
            }

            //lst = new ListItem("Registration", "0", tf);
            //rdoRegEnc.Items.Add(lst);

            //if (common.myStr(Request.QueryString["PageFrom"]) == "MRD")
            //{
            //    lst = new ListItem("IP Encounter", "1", false);
            //    rdoRegEnc.Items.Add(lst);

            //}
            //else
            //{

            //}
            //lst = new ListItem("Encounter", "1", tEncounter);
            //rdoRegEnc.Items.Add(lst);

            lst = new ListItem("Encounter", "1", true);
            rdoRegEnc.Items.Add(lst);
            lst = new ListItem("IP Encounter", "2", true);
            rdoRegEnc.Items.Add(lst);
            lst = new ListItem("OP Encounter", "3", true);
            rdoRegEnc.Items.Add(lst);

            //if (common.myStr(Request.QueryString["PageFrom"]) == "MRD")
            //{
            //    lst = new ListItem("OP Encounter", "3", false);
            //    rdoRegEnc.Items.Add(lst);
            //    lst = new ListItem("Emergency", "4", true);
            //    rdoRegEnc.Items.Add(lst);
            //}
            //if (common.myStr(Request.QueryString["SalType"]) == "ER")
            //{
            //    lst = new ListItem("Emergency", "4", false);
            //    rdoRegEnc.Items.Add(lst);
            //    rdoSearch.Enabled = false;
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnNew_OnClick(object sender, EventArgs e)
    {

    }
    protected void rdoRegEnc_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        tblDate.Visible = false;

        if (common.myInt(rdoRegEnc.SelectedValue) == 0)
        {
            gvEncounter.PageSize = 10;
        }
        else if (common.myInt(rdoRegEnc.SelectedValue) == 1)
        {
            gvEncounter.PageSize = 10;
        }
        else if (common.myInt(rdoRegEnc.SelectedValue) == 2)
        {

            tblDate.Visible = true;
            gvEncounter.PageSize = 10;
        }
        else if (common.myInt(rdoRegEnc.SelectedValue) == 4)
        {
            rdoSearch.Enabled = false;
            gvEncounter.PageSize = 10;
        }
        bindData("F", 0);
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
        dt.Columns.Add("RegistrationNoOld");
        dt.Columns.Add("MotherName");
        dt.Columns.Add("FatherName");
        dt.Columns.Add("ExternalPatient");
        dt.Columns.Add("RackNumber");
        dt.Columns.Add("ShelfNo");
        dt.Columns.Add("EncounterRackNo");
        dt.Columns.Add("EncounterShelfNo");
        dt.Columns.Add("UpdateStatus");
        dt.Columns.Add("PatientVisit");

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        gvEncounter.DataSource = dt;
        gvEncounter.DataBind();
    }

    private void bindData(string RecordButton, int RowNo)
    {
        try
        {


            objVal = new BaseC.clsEMRBilling(sConString);

            string BedNo = "";
            string EncNo = "";
            string RegNo = "";
            string PatientName = "";
            string PhoneNo = "";
            string Mobile = "";
            string CompanyName = "";
            string PassportNo = "";
            string Identityno = "";
            DateTime? Dob = null;
            string PEmail = "";
            string RegistrationOld = "";
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            string MotherName = "";
            string FatherName = "";
            string PreviousName = string.Empty;
            string LocalAddress = string.Empty;

            if (common.myStr(dtpFromDate.SelectedDate) != "" && common.myStr(dtpToDate.SelectedDate) != "")
            {
                FromDate = common.myDate(dtpFromDate.SelectedDate);
                ToDate = common.myDate(dtpToDate.SelectedDate);
            }



            EncNo = common.myStr(txtEncounterNo.Text);
            RegNo = common.myStr(txtRegistrationNo.Text);
            BedNo = common.myStr(txtBedNo.Text);
            PatientName = common.myStr(txtPatientName.Text);
            PhoneNo = common.myStr(txtPhoneNo.Text);
            Mobile = common.myStr(txtMobileNo.Text);
            CompanyName = common.myStr(txtCompany.Text);
            PassportNo = common.myStr(txtPassportno.Text);
            Identityno = common.myStr(txtCprno.Text);
            MotherName = common.myStr(txtMotherName.Text);
            FatherName = common.myStr(txtFatherName.Text);
            PreviousName = common.myStr(txtParentof.Text);
            LocalAddress = common.myStr(txtAddress.Text);
            if (common.myStr(txtDob.Text) != "")
            {
                Dob = common.myDate(txtDob.Text);
            }
            string PDateofbirth = "";
            if (Dob.HasValue)
            {
                PDateofbirth = Dob.Value.ToString("yyyy-MM-dd");
            }

            PEmail = common.myStr(txtEmailId.Text);
            RegistrationOld = common.myStr(txtOldRegistrationno.Text);
            int gender = 0;
            DataSet dsSearch = new DataSet();
            //if (common.myStr(Request.QueryString["PageFrom"]).Equals("Demographics"))
            //{
            //    gender = common.myInt(Request.QueryString["Gender"]);
            //    dsSearch = objVal.getOPIPRegEncDetailsFemaleOnly(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlLocation.SelectedValue),
            //    common.myStr(ViewState["OPIP"]), 0, 0, common.myInt(rdoRegEnc.SelectedValue), RegNo,
            //    EncNo, common.escapeCharString(PatientName, false), FromDate, ToDate, RecordButton, RowNo, common.myStr(Request.QueryString["PageFrom"]) == "MRD" ? 10 : common.myInt(gvEncounter.PageSize),
            //    common.myInt(Session["UserId"]), gvEncounter.CurrentPageIndex + 1, common.myStr(BedNo), 0,
            //    CompanyName, "", PhoneNo, Mobile, Identityno, PassportNo, PDateofbirth, PEmail, RegistrationOld, gender, common.myInt(ddlEntrySite.SelectedValue),
            //    common.myStr(MotherName), common.myStr(FatherName), PreviousName);
            //}
            //else
            //{
            dsSearch = objVal.getMRDOPIPRegEncDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(ddlLocation.SelectedValue),
            common.myStr(ViewState["OPIP"]), 0, 0, common.myInt(rdoRegEnc.SelectedValue), RegNo,
            EncNo, common.escapeCharString(PatientName, false), FromDate, ToDate, RecordButton, RowNo, common.myStr(Request.QueryString["PageFrom"]) == "MRD" && common.myInt(rdoRegEnc.SelectedValue) == 4 ? 100 : common.myInt(gvEncounter.PageSize),
            common.myInt(Session["UserId"]), gvEncounter.CurrentPageIndex + 1, common.myStr(BedNo), 0,
            CompanyName, "", PhoneNo, Mobile, Identityno, PassportNo, PDateofbirth, PEmail, RegistrationOld, common.myInt(ddlEntrySite.SelectedValue),
            common.myStr(MotherName), common.myStr(FatherName), PreviousName, 0, 0, 0, false, LocalAddress);
            //   }
            DataView dv = new DataView(dsSearch.Tables[0]);
            if (common.myStr(Request.QueryString["PageFrom"]) == "MRD" && common.myInt(rdoRegEnc.SelectedValue) == 4)
            {
                dv.RowFilter = "DischargeStatus<>0";
            }
            if (dv.ToTable().Rows.Count > 0)
            {
                gvEncounter.VirtualItemCount = Convert.ToInt32(dv.ToTable().Rows[0]["TotalRecordsCount"]);
            }
            else
            {
                gvEncounter.VirtualItemCount = 0;
            }
            if (dv.ToTable().Rows.Count > 0)
            {
                gvEncounter.DataSource = dv.ToTable();
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dv.ToTable());
                DataRow DR = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(DR);

                gvEncounter.DataSource = ds.Tables[0];
                ds.Dispose();
            }
            ViewState["GridDataAdmission"] = dv.ToTable();

            gvEncounter.DataBind();
            dsSearch.Dispose();
            dv.Dispose();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
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
                if (rdoRegEnc.SelectedValue == "2")
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Text = "Discharge Date";
                }
                else
                {
                    if (common.myStr(Request.QueryString["OPIP"]) == "O")
                    {
                        e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Text = "Registration Date";
                    }
                    if (common.myStr(Request.QueryString["OPIP"]) == "I")
                    {
                        e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Text = "Admission Date";
                    }
                }



            }
            //e.Item.Cells[Convert.ToByte(GridEncounter.DischargeStatus)].Text = "rafat";
            if (rdoRegEnc.SelectedValue == "0")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.EncounterNo)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.DoctorName)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.CurrentBedNo)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Visible = false;
                if (common.myInt(Session["FacilityId"]) == 7)
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.RegistrationNoOld)].Visible = true;
                }
                else
                {
                    e.Item.Cells[Convert.ToByte(GridEncounter.RegistrationNoOld)].Visible = false;
                }
                //e.Item.Cells[5].Visible = false;
                //e.Item.Cells[8].Visible = false;
                //e.Item.Cells[9].Visible = false;
                //e.Item.Cells[10].Visible = false;
            }
            if (rdoRegEnc.SelectedValue == "4")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.DischargeStatus)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
            }
            if (rdoRegEnc.SelectedValue == "0" || rdoRegEnc.SelectedValue == "1")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.DischargeStatus)].Visible = false;

            }
            if (rdoRegEnc.SelectedValue == "2")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.EncounterStatus)].Visible = false;
            }
            if (common.myStr(Request.QueryString["OPIP"]) == "I")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.OPIP)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
            }
            if (common.myStr(Request.QueryString["OPIP"]) == "O")
            {
                e.Item.Cells[Convert.ToByte(GridEncounter.OPIP)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.PhoneHome)].Visible = false;
                e.Item.Cells[Convert.ToByte(GridEncounter.EncounterStatus)].Visible = false;

                e.Item.Cells[Convert.ToByte(GridEncounter.EncDate)].Visible = true;

            }
        }
        if (e.Item is GridDataItem)
        {
            Label lblName = (Label)e.Item.FindControl("lblName");
            Label lblPatientAddress = (Label)e.Item.FindControl("lblPatientAddress");
            HiddenField hdnKinName = (HiddenField)e.Item.FindControl("hdnKinName");
            HiddenField hfExternalPatient = (HiddenField)e.Item.FindControl("hfExternalPatient");
            HiddenField hdnUpdateStatus = (HiddenField)e.Item.FindControl("hdnUpdateStatus");

            if (common.myStr(ViewState["isDiagnosticSeriesSame"]) == "N")
            {
                if (common.myStr(hfExternalPatient.Value) == "True")
                {
                    if (common.myStr(ViewState["DiagnosticColor"]) != "")
                    {
                        e.Item.BackColor = Color.FromName(common.myStr(ViewState["DiagnosticColor"]));
                    }
                }
            }

            #region Set UpdateStatus Color
            //if (common.myBool(hdnUpdateStatus.Value))
            //{
            //    e.Item.Cells[1].BackColor = Color.Yellow;
            //}

            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                if (common.myBool(hdnUpdateStatus.Value))
                {
                    //dataItem.ForeColor  = System.Drawing.Color.Red;  
                    dataItem.BackColor = Color.Yellow;
                   // dataItem.Font.Bold = true;
                }
            }
            #endregion

            e.Item.Attributes.Add("onclick", "javascript:ShowPatientDetails('" + lblName.ClientID +
                            "','" + lblPatientAddress.ClientID + "','" + hdnKinName.ClientID + "');");
            }


        }
    protected void gvEncounter_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvEncounter.CurrentPageIndex = e.NewPageIndex;
        bindData("F", 0);
    }
    protected void gvEncounter_PreRender(object sender, EventArgs e)
    {
        gvEncounter.DataSource = (DataTable)ViewState["GridDataAdmission"];
        gvEncounter.DataBind();
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
                    hdnCompanyCode.Value = common.myStr(((Label)e.Item.FindControl("lblCompanyCode")).Text);
                    hdnInsuranceCode.Value = common.myStr(((Label)e.Item.FindControl("lblInsuranceCode")).Text);
                    hdnCardId.Value = common.myStr(((Label)e.Item.FindControl("lblCardId")).Text);
                    hdnEncounterDate.Value = common.myStr(((Label)e.Item.FindControl("lblEncDate")).Text);

                    hdnRackNumber.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnRackNumber")).Value);
                    hdnShelfNo.Value = common.myStr(((HiddenField)e.Item.FindControl("hdnShelfNo")).Value);

                    hdnAgeGender.Value = common.myStr(((Label)e.Item.FindControl("lblGenderAge")).Text);
                    hdnPhoneHome.Value = common.myStr(((Label)e.Item.FindControl("lblPhoneHome")).Text);
                    hdnMobileNo.Value = common.myStr(((Label)e.Item.FindControl("lblMobileNo")).Text);
                    hdnPatientName.Value = common.myStr(((Label)e.Item.FindControl("lblName")).Text);
                    hdnDOB.Value = common.myStr(((Label)e.Item.FindControl("lblDOB")).Text);
                    hdnAddress.Value = common.myStr(((Label)e.Item.FindControl("lblPatientAddress")).Text);
                    // hdnFacilityId.Value = common.myStr(ddlLocation.SelectedValue);


                    dvRackNo.Visible = true;
                    txtRackNo.Text = common.myStr(hdnRackNumber.Value);
                    txtShelfNo.Text = common.myStr(hdnShelfNo.Value);
                    txtUHID.Text = common.myStr(hdnRegistrationNo.Value);

                    BindPatientHiddenDetails(common.myInt(hdnRegistrationNo.Value));

                    //RadWindow1.NavigateUrl = "/MRD/MRDFillRackNo.aspx.aspx?MRDSearch=IP";
                    //RadWindow1.Height = 580;
                    //RadWindow1.Width = 900;
                    //RadWindow1.Top = 10;
                    //RadWindow1.Left = 10;
                    //RadWindow1.OnClientClose = "SearchPatientOnClientCloseIP";
                    //RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    //RadWindow1.Modal = true;
                    //RadWindow1.VisibleStatusbar = false;

                    //   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);



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
    }
    protected void rdoSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdoSearch.SelectedValue == "0")
        {
            tblsearch.Visible = true;
            tblDate.Visible = false;

            dtpFromDate.SelectedDate = null;
            dtpToDate.SelectedDate = null;
        }
        else
        {
            tblsearch.Visible = false;
            tblDate.Visible = true;

            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;


        }
    }
    protected void btnSearch_OnClick(Object sender, EventArgs e)
    {
        bindData("F", 0);
    }
    protected void btnClearSearch_OnClick(Object sender, EventArgs e)
    {
        // clearControl();
        txtRegistrationNo.Text = "";
        txtEncounterNo.Text = "";
        txtBedNo.Text = "";
        txtMobileNo.Text = "";
        txtPatientName.Text = "";
        txtPhoneNo.Text = "";
        txtCompany.Text = "";
        txtPassportno.Text = "";
        txtEmailId.Text = "";
        txtDob.Text = "";
        txtOldRegistrationno.Text = "";
        txtCprno.Text = "";

        lblMessage.Text = "";
        bindData("F", 0);
    }

    void BindPatientHiddenDetails(int RegistrationNo)
    {
        try
        {
            BaseC.ParseData bParse = new BaseC.ParseData();
            BaseC.Patient bC = new BaseC.Patient(sConString);

            if (RegistrationNo > 0)
            {
                DataSet ds = new DataSet();
                ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0, RegistrationNo, 0, common.myInt(Session["UserId"]));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                    hdnEncounterNo.Value = common.myStr(dr["EncounterNo"]); ;
                    //   hdnEncounterId.Value = common.myStr(dr["EncounterID"]);

                    lblRackNoPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                    lblDob.Text = common.myStr(dr["DOB"]);
                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);


                }
                else
                {
                    Alert.ShowAjaxMsg("Patient Not Found", Page);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        dvRackNo.Visible = false;
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        SaveData();
    }
    private void SaveData()
    {
        BaseC.RestFulAPI objMRD = new BaseC.RestFulAPI(sConString);
        try
        {
            string strMsg = string.Empty;
            if (common.myLen(txtRackNo.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter Rack number";
                return;
            }
            if (common.myLen(txtShelfNo.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter Shelf Number";
                return;
            }
            if (common.myLen(txtRemarks.Text).Equals(0))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please enter Remarks";
                return;
            }

            strMsg = objMRD.MRDSaveFileRackNo(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                        common.myInt(hdnEncounterId.Value), common.myInt(hdnRegistrationId.Value), common.myStr(txtRackNo.Text),
                        common.myStr(txtShelfNo.Text), common.myStr(txtRemarks.Text),
                          common.myInt(Session["UserId"]));


            if ((strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                clearControl();
                dvRackNo.Visible = false;
                bindData("F", 0);
                clearControl();
            }
            lblMessage.Text = strMsg;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            objMRD = null;
        }
    }

    void clearControl()
    {
        txtRackNo.Text = string.Empty;
        txtShelfNo.Text = string.Empty;
        txtRemarks.Text = string.Empty;
    }
}
