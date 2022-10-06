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
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Text;
using System.Globalization;
using Telerik.Web.UI;
using BaseC;

public partial class ATD_Booking : System.Web.UI.Page
{

    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DAL.DAL dl = new DAL.DAL();
    SqlDataReader dr;
    String sqlstr = "";
    BaseC.ParseData bc = new BaseC.ParseData();
    private const string DefaultCountry = "DefaultCountry";
    private const string DefaultState = "DefaultState";
    private const string DefaultCity = "DefaultCity";
    private const string DefaultZip = "DefaultZip";
    BaseC.ParseData Parse = new BaseC.ParseData();
    BaseC.ATD objatd;
    BaseC.Hospital objh;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
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
        try
        {
            if (Session["UserID"] == null || Session["HospitalLocationID"] == null || Session["FacilityId"] == null)
            {
                Response.Redirect("~/Login.aspx?Logout=1", false);
                return;
            }
            if (!IsPostBack)
            {
                //done by rakesh for user authorisation start
                SetPermission();
                //done by rakesh for user authorisation end
                dtpbookingdate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpbookingdate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpExpecAdmtDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpExpecAdmtDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpbookingdate.SelectedDate = DateTime.Now;
                dtpExpecAdmtDate.SelectedDate = DateTime.Now;



                dtpExpDischDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpExpDischDate.DateInput.DisplayDateFormat = common.myStr(Application["OutputDateFormat"]);
                dtpExpDischDate.SelectedDate = DateTime.Now;



                if (Session["RegistrationId"] != null)
                {
                    BindPatientHiddenDetails(common.myInt(Session["RegistrationNo"]));
                    txtRegNo.Text = common.myStr(Session["RegistrationNo"]);
                }

                BindAllCombo();

                Session["AdmissionRequestBookingId"] = string.Empty;
                Session["AdmissionRequestBookingNo"] = string.Empty;
                Session["AdmissionRequestRegNo"] = string.Empty;
                Session["AdmissionRequestDocName"] = string.Empty;
                Session["AdmissionRequestBookingType"] = string.Empty;
                Session["AdmissionRequestFromDate"] = string.Empty;
                Session["AdmissionRequestToDate"] = string.Empty;

                Session["AdmissionRequestRT"] = string.Empty;
                Session["AdmissionRequestReportType"] = string.Empty;                
            }
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
            {
                btnsave.Visible = false;
                ibtnNew.Visible = false;
            }

            if (common.myStr(Session["OPIP"]).Equals("I"))
            {
                btnsave.Visible = false;
            }
            else
            {
                btnsave.Visible = true;
            }

            if (Session["OPIP"] == null)
            {
                FirstBedCategoryStar.Visible = true;
            }
            else
            {
                FirstBedCategoryStar.Visible = false;
            }
            setMandatoryAndEnableFields();
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnfind_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(txtRegNo.Text) != "")
            {
                BindPatientHiddenDetails(common.myInt(txtRegNo.Text));
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void checkPatientBooking()
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataTable dt = new DataTable();
        try
        {

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindBedNo()
    {
        DataSet ds = new DataSet();
        BaseC.EMRBilling bill = new BaseC.EMRBilling(sConString);
        try
        {
            ddlBedNo.Items.Clear();
            ddlBedNo.Text = "";

            ds = bill.GetWardBedCategoryWiseBedNo(Convert.ToInt16(Session["HospitalLocationId"]), Convert.ToInt16(Session["FacilityId"]), 0, common.myInt(ddlFirstBedCategory.SelectedValue));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlBedNo.DataSource = ds.Tables[0];
                    ddlBedNo.DataTextField = "BedNo";
                    ddlBedNo.DataValueField = "BedId";
                    ddlBedNo.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            bill = null;
        }
    }

    void BindPatientHiddenDetails(int RegistrationNo)
    {
        BaseC.ParseData bParse = new BaseC.ParseData();
        BaseC.Patient bC = new BaseC.Patient(sConString);
        BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);
        DataTable dtbooking = new DataTable();

        try
        {
            if (Session["PatientDetailString"] != null)
            {
                //  lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
                trInfo1.Visible = false;
                trInfo2.Visible = false;
                trInfo3.Visible = true;
            }
            else
            {
                trInfo1.Visible = true;
                trInfo2.Visible = true;
                trInfo3.Visible = false;
                if (RegistrationNo > 0)
                {
                    int HospId = common.myInt(Session["HospitalLocationID"]);
                    int FacilityId = common.myInt(Session["FacilityId"]);
                    int RegId = 0;
                    int EncounterId = 0;
                    int EncodedBy = common.myInt(Session["UserId"]);
                    DataSet ds = new DataSet();
                    ds = bC.getPatientDetails(HospId, FacilityId, RegId, RegistrationNo, EncounterId, EncodedBy);
                    if (ds.Tables.Count > 0)
                    {
                        DataView dvIP = new DataView(ds.Tables[0]);
                        //  dvIP.RowFilter = "OPIP = 'O'";
                        DataTable dt = new DataTable();
                        dt = dvIP.ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            hdnPatientAgeInYrs.Value = common.myStr(dr["PatientAgeInYrs"]);
                            hdnRegistrationId.Value = common.myStr(dr["RegistrationId"]);
                            hdnTaggedEmpNo.Value = common.myStr(dr["TaggedEmpNo"]);
                            if (common.myStr(dr["Dischargedate"]) == "" && common.myStr(dr["OPIP"]) != "O")
                            {
                                lblPatientName1.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                                lblDob.Text = common.myStr(dr["DOB"]);
                                lblMobile.Text = common.myStr(dr["MobileNo"]);
                                //lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                                //lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                                hdnEncounterId.Value = common.myStr(dr["EncounterId"]); ;
                            }
                            else
                            {
                                lblPatientName1.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                                lblDob.Text = common.myStr(dr["DOB"]);
                                lblMobile.Text = common.myStr(dr["MobileNo"]);
                                //lblEncounterNo.Text = "";
                                //lblAdmissionDate.Text = "";
                            }
                            lblAddress.Text = common.myStr(dr["Address"]);
                            lblCity.Text = common.myStr(dr["CityName"]);
                            lblState.Text = common.myStr(dr["StateName"]);
                            lblCountry.Text = common.myStr(dr["CountryName"]);
                            lblmsg.Text = "";

                        }


                    }
                    else
                    {
                        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblmsg.Text = "Patient not found !";
                        return;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            bParse = null;
            bC = null;
            objLISMaster = null;
        }
    }
    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "/Pharmacy/SaleIssue/PatientDetails.aspx?OPIP=I&RegEnc=0";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void lbtnSearchBookings_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadWindowForNew.NavigateUrl = "BookingList.aspx";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.OnClientClose = "SearchBookingOnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ibtnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), false);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (validation())
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                if (dtpbookingdate.SelectedDate == null)
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "Please Select Booking Date...";
                    Alert.ShowAjaxMsg("Please Select Booking Date...!", Page);
                    return;
                }
                if (dtpExpecAdmtDate.SelectedDate == null)
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "Please Select Expected Admission Date...";
                    Alert.ShowAjaxMsg("Please Select Expected Admission Date...!", Page);
                    return;
                }
                if (dtpExpecAdmtDate.SelectedDate != null)
                {
                    int DateCheck = DateTime.Compare(Convert.ToDateTime(dtpExpecAdmtDate.SelectedDate), DateTime.Now.Date);
                    if (DateCheck < 0)
                    {
                        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblmsg.Text = "Back date expected admission booking not allowed !";
                        Alert.ShowAjaxMsg("Back date expected admission booking not allowed !", Page);
                        return;
                    }
                }
                if (txtRegNo.Text.Trim() == "")
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "Please select patient for booking...!";
                    Alert.ShowAjaxMsg("Please select patient for booking...!", Page);
                    return;
                }
                if (common.myStr(ddlBookingType.SelectedValue).Equals(""))
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "Please select booking type...!";
                    ddlBookingType.Focus();
                    Alert.ShowAjaxMsg("Please select booking type...!", Page);
                    return;
                }
                if (common.myStr(ddlBookingSource.SelectedValue).Equals("0"))
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "Please select booking source...!";
                    Alert.ShowAjaxMsg("Please select booking source...!", Page);
                    return;
                }
                if (common.myStr(ddlBookingDoctor.SelectedValue).Equals("0"))
                {
                    lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblmsg.Text = "Please select booking doctor...!";
                    ddlBookingDoctor.Focus();
                    Alert.ShowAjaxMsg("Please select booking doctor...!", Page);
                    return;
                }
                if (Session["OPIP"] == null)
                {
                    if (common.myStr(ddlFirstBedCategory.SelectedValue).Equals(""))
                    {
                        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblmsg.Text = "Please select first bed category...!";
                        Alert.ShowAjaxMsg("Please select first bed category...!", Page);
                        return;
                    }
                }                

                //if (common.myInt(txtProbableStayInDays.Text) == 0)
                //{
                //    Alert.ShowAjaxMsg("Please enter Probable Stay in Days", Page);
                //    return;
                //}


                //if (common.myStr(txtReasonForAdmission.Text) == "")
                //{
                //    Alert.ShowAjaxMsg("Please enter Reason For Admission", Page);
                //    return;
                //}


                BaseC.ATD objBed = new BaseC.ATD(sConString);
                int HospId = common.myInt(Session["HospitalLocationId"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int RegId = common.myInt(hdnRegistrationId.Value) == 0 ? common.myInt(Session["RegistrationId"]) : common.myInt(hdnRegistrationId.Value);
                int EncId = common.myInt(hdnEncounterId.Value) == 0 ? common.myInt(Session["EncounterId"]) : common.myInt(hdnEncounterId.Value);
                int BookingId = common.myInt(hdnBookingId.Value);
                string BookingDate = dtpbookingdate.SelectedDate.Value.ToString("yyyy/MM/dd");
                string ExpAdmDate = dtpExpecAdmtDate.SelectedDate.Value.ToString("yyyy/MM/dd");

                string ExpDischDate = dtpExpDischDate.SelectedDate.Value.ToString("yyyy/MM/dd");

                int SourceId = common.myInt(ddlBookingSource.SelectedValue);
                string BookingType = common.myStr(ddlBookingType.SelectedValue);
                string BookingStatus = common.myStr(ddlBookingStatus.SelectedValue);
                int BedCatId1 = common.myInt(ddlFirstBedCategory.SelectedValue);
                int BedCatId2 = common.myInt(ddlSecondBedCategory.SelectedValue);
                int BedCatId3 = common.myInt(ddlThirdBedCategory.SelectedValue);
                int DoctorId = common.myInt(ddlBookingDoctor.SelectedValue);
                string Remarks = common.myStr(txtRemarks.Text);

                string ProcedureName = common.myStr(txtProcedureName.Text);
                string ProcedureDuration = common.myStr(txtProcedureDuration.Text);
                string ImplantDetails = common.myStr(txtImplantDetails.Text);
                string AnaesthesiaType = common.myStr(txtAnaesthesiaType.Text);
                string HeadCode = "";
                int UserId = common.myInt(Session["UserId"]);

                string BedNo = common.myStr(ddlBedNo.SelectedValue);


                Hashtable htOut = new Hashtable();

                htOut = objBed.SaveBedBooking(HospId, FacilityId, RegId, common.myInt(EncId), BookingId, BookingDate, SourceId,
                    BookingType, BookingStatus, BedCatId1, BedCatId2, BedCatId3, DoctorId, ExpAdmDate, Remarks, HeadCode, UserId, common.myInt(txtProbableStayInDays.Text), common.myStr(txtReasonForAdmission.Text),
                    ProcedureName, ProcedureDuration, ImplantDetails, AnaesthesiaType, ExpDischDate, BedNo);

                if (common.myStr(htOut["@chvErrorStatus"]).ToUpper().Contains("SAVED") || common.myStr(htOut["@chvErrorStatus"]).ToUpper().Contains("UPDATED"))
                {
                    if (common.myInt(htOut["@intId"]) > 0)
                    {
                        hdnBookingId.Value = common.myStr(htOut["@intId"]);
                        txtBookingNo.Text = common.myStr(htOut["@chvBookingNo"]);
                        lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    }
                }
                lblmsg.Text = common.myStr(htOut["@chvErrorStatus"]);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSearchByBookingNo_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myStr(txtBookingNo.Text) != "")
            {
                ShowBookingDetails(common.myStr(txtBookingNo.Text));
            }
            else
            {
                lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblmsg.Text = "Please Enter Booking No !";
                return;
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void ShowBookingDetails(string BookingNo)
    {
        //done by rakesh for user authorisation start
        SetPermission(btnsave, "E", true);
        if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true)
        {
            btnsave.Visible = false;
            ibtnNew.Visible = false;
        }

        //done by rakesh for user authorisation end
        BaseC.ATD objBed = new BaseC.ATD(sConString);
        int HospId = common.myInt(Session["HospitalLocationID"]);
        int FacilityId = common.myInt(Session["FacilityId"]);
        int BookingId = 0;
        string RegNo = "";
        string DocName = "";
        string BookingType = "";
        string FromDate = "1900/01/01";
        string ToDate = "2079/01/01";
        DataSet ds = new DataSet();
        try
        {
            ds = objBed.GetPatientBedBookingDetails(HospId, FacilityId, FromDate, ToDate, BookingId, BookingNo, RegNo, DocName,
                BookingType, "B");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    dtpbookingdate.SelectedDate = Convert.ToDateTime(dr["BookingDate"]);
                    ddlBookingType.SelectedValue = common.myStr(dr["Bookingtype"]);
                    ddlBookingSource.SelectedValue = common.myStr(dr["SourceId"]);
                    ddlBookingStatus.SelectedValue = common.myStr(dr["Bookingstatus"]);
                    ddlFirstBedCategory.SelectedValue = common.myStr(dr["Bedcategory1"]);
                    ddlSecondBedCategory.SelectedValue = common.myStr(dr["Bedcategory2"]);
                    ddlThirdBedCategory.SelectedValue = common.myStr(dr["Bedcategory3"]);
                    ddlBookingDoctor.SelectedValue = common.myStr(dr["DoctorId"]);
                    dtpExpecAdmtDate.SelectedDate = Convert.ToDateTime(dr["ExpectedAdmissiondate"]);
                    txtRemarks.Text = common.myStr(dr["Remarks"]);

                    txtProcedureName.Text = common.myStr(dr["ProcedureName"]);
                    txtProcedureDuration.Text = common.myStr(dr["ProcedureDuration"]);
                    txtImplantDetails.Text = common.myStr(dr["ImplantDetails"]);
                    txtAnaesthesiaType.Text = common.myStr(dr["AnaesthesiaType"]);

                    txtRegNo.Text = common.myStr(dr["RegistrationNo"]);
                    hdnBookingId.Value = common.myStr(dr["BookingId"]);
                    txtProbableStayInDays.Text = common.myStr(dr["ProbableStayInDays"]);
                    txtReasonForAdmission.Text = common.myStr(dr["ReasonForAdmission"]);

                    btnfind_Click(this, null);
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objBed = null;
        }
    }
    private void BindAllCombo()
    {
        DataSet ds = new DataSet();

        StringBuilder sbSQL = new StringBuilder();
        DataView dv = new DataView();
        DataSet dsDoctor = new DataSet();
        Hospital baseHosp = new Hospital(sConString);
        BaseC.EMRBilling objBed = new BaseC.EMRBilling(sConString);
        try
        {

            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            dsDoctor = baseHosp.fillDoctorCombo(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), 0, Convert.ToInt16(common.myInt(Session["FacilityId"])));
            if (dsDoctor.Tables.Count > 0 && dsDoctor.Tables[0].Rows.Count > 0)
            {
                ddlBookingDoctor.DataSource = dsDoctor;
                ddlBookingDoctor.DataTextField = "DoctorName";
                ddlBookingDoctor.DataValueField = "DoctorId";
                ddlBookingDoctor.DataBind();

                if (common.myBool(Session["IsLoginDoctor"]))
                {
                    ddlBookingDoctor.SelectedIndex = ddlBookingDoctor.Items.IndexOf(ddlBookingDoctor.Items.FindItemByValue(common.myInt(Session["EmployeeId"]).ToString()));
                }
            }
            if ((!common.myStr(Session["OPIP"]).Equals("") && common.myStr(Session["OPIP"]).Trim().Equals("E")) && !common.myStr(Session["DoctorId"]).Equals(""))
            {
                ddlBookingDoctor.SelectedValue = common.myStr(Session["DoctorId"]);
                ddlBookingType.SelectedValue = "G";
            }
            else
            {
                ddlBookingDoctor.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                ddlBookingDoctor.Items[0].Value = "0";
            }

            //populate Source drop down control 
            sbSQL.Append(" SELECT Name, id,Type FROM SourceMaster WHERE Active = 1");
            ds = dl.FillDataSet(CommandType.Text, common.myStr(sbSQL));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlBookingSource.DataSource = ds.Tables[0];
                ddlBookingSource.DataTextField = "Name";
                ddlBookingSource.DataValueField = "id";
                ddlBookingSource.DataBind();

                ddlBookingSource.SelectedIndex = ddlBookingSource.Items.IndexOf(ddlBookingSource.Items.FindItemByText("OPD"));
            }
            if (Session["OPIP"] != null && common.myStr(Session["OPIP"]).Trim().Equals("E"))
            {
                dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "Type='E'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    ddlBookingSource.SelectedValue = common.myStr(dv.ToTable().Rows[0]["id"]);
                }
            }
            else
            {
                ddlBookingSource.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                ddlBookingSource.Items[0].Value = "0";
            }
            ds = new DataSet();
            ds = objBed.GetBedCategory(common.myInt(Session["FacilityId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlFirstBedCategory.DataSource = ds.Tables[0];
                ddlFirstBedCategory.DataTextField = "BedCategoryName";
                ddlFirstBedCategory.DataValueField = "BedCategoryId";
                ddlFirstBedCategory.DataBind();

                ddlSecondBedCategory.DataSource = ds;
                ddlSecondBedCategory.DataTextField = "BedCategoryName";
                ddlSecondBedCategory.DataValueField = "BedCategoryId";
                ddlSecondBedCategory.DataBind();

                ddlThirdBedCategory.DataSource = ds;
                ddlThirdBedCategory.DataTextField = "BedCategoryName";
                ddlThirdBedCategory.DataValueField = "BedCategoryId";
                ddlThirdBedCategory.DataBind();

                ddlFirstBedCategory.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                ddlSecondBedCategory.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
                ddlThirdBedCategory.Items.Insert(0, new RadComboBoxItem("[Select]", ""));
            }

        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            sbSQL = null;
            dv = null;
            dsDoctor.Dispose();
            baseHosp = null;
            objBed = null;
        }
    }
    protected void ddlFirstBedCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindBedNo();
    }
    //done by rakesh for user authorisation start
    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        try
        {
            ua1.DisableEnableControl(btnsave, false);
            ua1.DisableEnableControl(ibtnNew, false);

            if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
            {
                ua1.DisableEnableControl(btnsave, true);
                ua1.DisableEnableControl(ibtnNew, true);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }
    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        try
        {
            ua1.DisableEnableControl(btnID, false);

            if (ua1.CheckPermissions(mode, Request.Url.AbsolutePath))
            {
                ua1.DisableEnableControl(btnID, action);
            }
            else
            {
                ua1.DisableEnableControl(btnID, !action);
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }
    private void SetPermission(Button btnID, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, action);
        ua1.Dispose();
    }
    //done by rakesh for user authorisation end

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //if (!common.myStr(Session["AdmissionRequestBookingId"]).Equals(string.Empty) &&
        //        !common.myStr(Session["AdmissionRequestRegNo"]).Equals(string.Empty))
        //{
        //    lblmsg.Text = string.Empty;
        //    RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + common.myStr(Session["AdmissionRequestFromDate"])
        //         + "&Todate=" + common.myStr(Session["AdmissionRequestToDate"])
        //         + "&BI=" + common.myStr(Session["AdmissionRequestBookingId"]) + "& BN=" + common.myStr(Session["AdmissionRequestBookingNo"])
        //         + "&RN=" + common.myStr(Session["AdmissionRequestRegNo"])
        //          //+ "&DN=" + DocName
        //          + "&DN=" + ""
        //         + "&ReportType=" + common.myStr(Session["AdmissionRequestReportType"])
        //         + "&BT="
        //         + common.myStr(Session["AdmissionRequestBookingType"]) + "&ReportName=BookingList"
        //         + "&RT=" + common.myStr(Session["AdmissionRequestRT"]);
        //    RadWindowForNew.Height = 480;
        //    RadWindowForNew.Width = 850;
        //    RadWindowForNew.Top = 40;
        //    RadWindowForNew.Left = 100;
        //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //    RadWindowForNew.Modal = true;
        //    //  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        //    RadWindowForNew.VisibleStatusbar = false;

        //}
        //else
        //{
        if (!common.myStr(txtBookingNo.Text).Equals(string.Empty))
        {
            GetPrintDetails();
        }

        //lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //lblmsg.Text = "Please Select Admission Request before printing...";
        //Alert.ShowAjaxMsg("Please Select Admission Request before printing...", Page);
        //return;
        //}
    }
    protected void GetPrintDetails()
    {
        BaseC.ATD objBed = new BaseC.ATD(sConString);
        int HospId = common.myInt(Session["HospitalLocationID"]);
        int FacilityId = common.myInt(Session["FacilityId"]);
        int BookingId = 0;
        string RegNo = "";
        string DocName = "";
        string BookingType = "";
        string FromDate = "1900/01/01";
        string ToDate = "2079/01/01";
        DataSet ds = new DataSet();
        try
        {
            ds = objBed.GetPatientBedBookingDetails(HospId, FacilityId, FromDate, ToDate, BookingId, common.myStr(txtBookingNo.Text), RegNo, DocName,
                BookingType, "B");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    //dtpbookingdate.SelectedDate = Convert.ToDateTime(dr["BookingDate"]);
                    //ddlBookingType.SelectedValue = common.myStr(dr["Bookingtype"]);
                    //ddlBookingSource.SelectedValue = common.myStr(dr["SourceId"]);
                    //ddlBookingStatus.SelectedValue = common.myStr(dr["Bookingstatus"]);
                    //ddlFirstBedCategory.SelectedValue = common.myStr(dr["Bedcategory1"]);
                    //ddlSecondBedCategory.SelectedValue = common.myStr(dr["Bedcategory2"]);
                    //ddlThirdBedCategory.SelectedValue = common.myStr(dr["Bedcategory3"]);
                    //ddlBookingDoctor.SelectedValue = common.myStr(dr["DoctorId"]);
                    //dtpExpecAdmtDate.SelectedDate = Convert.ToDateTime(dr["ExpectedAdmissiondate"]);
                    //txtRemarks.Text = common.myStr(dr["Remarks"]);
                    //txtRegNo.Text = common.myStr(dr["RegistrationNo"]);
                    //hdnBookingId.Value = common.myStr(dr["BookingId"]);
                    //txtProbableStayInDays.Text = common.myStr(dr["ProbableStayInDays"]);
                    //txtReasonForAdmission.Text = common.myStr(dr["ReasonForAdmission"]);


                    RadWindowForNew.NavigateUrl = "/EMRReports/PrintReport.aspx?Fromdate=" + Convert.ToDateTime(dr["BookingDate"]).ToString("yyyy/MM/dd")
                + "&Todate=" + Convert.ToDateTime(dr["BookingDate"]).ToString("yyyy/MM/dd")
                + "&BI=" + common.myStr(dr["BookingId"]) + "& BN=" + common.myStr(txtBookingNo.Text)
                + "&RN=" + common.myStr(dr["RegistrationNo"])
                 //+ "&DN=" + DocName
                 + "&DN=" + ""
                + "&ReportType=" + ""
                + "&BT=" + common.myStr(dr["Bookingtype"]) + "&ReportName=BookingList"
                + "&RT=" + "";
                    RadWindowForNew.Height = 480;
                    RadWindowForNew.Width = 850;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    //  RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
                    RadWindowForNew.VisibleStatusbar = false;
                }
            }
        }
        catch (Exception Ex)
        {
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objBed = null;
        }
    }

    private void setMandatoryAndEnableFields()
    {
        BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
        DataSet dsPage = new DataSet();
        try
        {
            dsPage = hsSetup.GetPageMandatoryAndEnableFields(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "EMR", "AdmissionAdvice");

            if (dsPage.Tables[0].Rows.Count > 0)
            {
                spnBookingStatus.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "BookingStatus");
                ddlBookingStatus.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "BookingStatus");

                FirstBedCategoryStar.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "FirstBedCat");
                ddlFirstBedCategory.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "FirstBedCat");

                spnSecondBedCategory.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "SecondBedCat");
                ddlSecondBedCategory.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "SecondBedCat");

                spnThirdBedCategory.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ThirdBedCat");
                ddlThirdBedCategory.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ThirdBedCat");

                spnExpecAdmtDate.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ExpAdmtDate");
                dtpExpecAdmtDate.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ExpAdmtDate");

                spnProbableStayInDays.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ProbableStay");
                txtProbableStayInDays.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ProbableStay");

                spnReasonForAdmission.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ReasonForAdmission");
                txtReasonForAdmission.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ReasonForAdmission");

                spnRemarks.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "Remarks");
                txtRemarks.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "Remarks");

                spnProcedureName.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ProcedureName");
                txtProcedureName.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ProcedureName");

                spnProcedureDuration.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ProcedureDuration");
                txtProcedureDuration.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ProcedureDuration");

                spnImplantDetails.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "ImplantDetails");
                txtImplantDetails.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "ImplantDetails");

                spnAnaesthesiaType.Visible = IsMandatory(dsPage.Tables[0].Copy().DefaultView, "AnaesthesiaType");
                txtAnaesthesiaType.Enabled = IsEnable(dsPage.Tables[0].Copy().DefaultView, "AnaesthesiaType");
            }
            else
            {
                spnBookingStatus.Visible = false;
                if (Session["OPIP"] == null)
                {
                    FirstBedCategoryStar.Visible = true;
                }
                else
                {
                    FirstBedCategoryStar.Visible = false;
                }
                spnSecondBedCategory.Visible = false;
                spnThirdBedCategory.Visible = false;
                spnExpecAdmtDate.Visible = false;
                spnProbableStayInDays.Visible = false;
                spnReasonForAdmission.Visible = false;
                spnRemarks.Visible = true;
                spnProcedureName.Visible = false;
                spnProcedureDuration.Visible = false;
                spnImplantDetails.Visible = false;
                spnAnaesthesiaType.Visible = false;
            }
        }

        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally
        {
            hsSetup = null;
            dsPage.Dispose();
        }
    }

    private bool IsMandatory(DataView dv, string FieldName)
    {
        bool isM = false;
        try
        {
            if (dv != null)
            {
                dv.RowFilter = "FieldName='" + FieldName + "'";
                if (dv.Count > 0)
                {
                    isM = common.myBool(dv.ToTable().Rows[0]["IsMandatoryField"]);
                }
                dv.RowFilter = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return isM;
    }

    private bool IsEnable(DataView dv, string FieldName)
    {
        bool isE = false;
        try
        {
            if (dv != null)
            {
                dv.RowFilter = "FieldName='" + FieldName + "'";
                if (dv.Count > 0)
                {
                    isE = common.myBool(dv.ToTable().Rows[0]["IsEnableField"]);
                }
                dv.RowFilter = string.Empty;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblmsg.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblmsg.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return isE;
    }

    private bool validation()
    {

        if (spnBookingStatus.Visible)
        {
            //if (common.myInt(ddlBookingStatus.SelectedValue).Equals(0))
            //{
            //    lblmsg.Text = "Please enter Batch No.";
            //    lblmsg.ForeColor = System.Drawing.Color.Red;
            //    Alert.ShowAjaxMsg("Please enter Batch No...", Page.Page);
            //    return;
            //}
        }
        if (FirstBedCategoryStar.Visible)
        {
            if (common.myInt(ddlFirstBedCategory.SelectedValue).Equals(0))
            {
                lblmsg.Text = "Please enter First BedCategory.";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                ddlFirstBedCategory.Focus();
                Alert.ShowAjaxMsg("Please enter First BedC ategory...", Page.Page);
                return false;
            }
        }
        if (spnSecondBedCategory.Visible)
        {
            if (common.myInt(ddlSecondBedCategory.SelectedValue).Equals(0))
            {
                lblmsg.Text = "Please enter Second BedCategory";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                ddlSecondBedCategory.Focus();
                Alert.ShowAjaxMsg("Please enter Second Bed Category...", Page.Page);
                return false;
            }
        }
        if (spnThirdBedCategory.Visible)
        {
            if (common.myInt(ddlThirdBedCategory.SelectedValue).Equals(0))
            {
                lblmsg.Text = "Please enter Third Bed Category";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                ddlThirdBedCategory.Focus();
                Alert.ShowAjaxMsg("Please enter Third Bed Category...", Page.Page);
                return false;
            }
        }
        if (spnExpecAdmtDate.Visible)
        {
            if (common.myLen(dtpExpecAdmtDate.SelectedDate).Equals(0))
            {
                lblmsg.Text = "Please enter Expected Addmission date";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                dtpExpecAdmtDate.Focus();
                Alert.ShowAjaxMsg("Please Expected Addmission date...", Page.Page);
                return false;
            }
        }
        if (spnProbableStayInDays.Visible)
        {
            if (common.myLen(txtProbableStayInDays.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Probable Stay (Days)";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtProbableStayInDays.Focus();
                Alert.ShowAjaxMsg("Please Probable Stay (Days)...", Page.Page);
                return false;
            }
        }
        if (spnReasonForAdmission.Visible)
        {
            if (common.myLen(txtReasonForAdmission.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Reason For Admission";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtReasonForAdmission.Focus();
                Alert.ShowAjaxMsg("Please enter Reason For Admission...", Page.Page);
                return false;
            }
        }
        if (spnRemarks.Visible)
        {
            if (common.myLen(txtRemarks.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Remarks";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtRemarks.Focus();
                Alert.ShowAjaxMsg("Please enter Remarks...", Page.Page);
                return false;
            }
        }
        if (spnProcedureName.Visible)
        {
            if (common.myLen(txtProcedureName.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Procedure Name";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtProcedureName.Focus();
                Alert.ShowAjaxMsg("Please enter Procedure Name...", Page.Page);
                return false;
            }
        }
        if (spnProcedureDuration.Visible)
        {
            if (common.myLen(txtProcedureDuration.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Procedure Duration";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtProcedureDuration.Focus();
                Alert.ShowAjaxMsg("Please enter Procedure Duration...", Page.Page);
                return false;
            }
        }


        if (spnImplantDetails.Visible)
        {
            if (common.myLen(txtImplantDetails.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Implant Details";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtImplantDetails.Focus();
                Alert.ShowAjaxMsg("Please enter Implant Details...", Page.Page);
                return false;
            }
        }
        if (spnAnaesthesiaType.Visible)
        {
            if (common.myLen(txtAnaesthesiaType.Text).Equals(0))
            {
                lblmsg.Text = "Please enter Anaesthesia Type";
                lblmsg.ForeColor = System.Drawing.Color.Red;
                txtAnaesthesiaType.Focus();
                Alert.ShowAjaxMsg("Please enter Anaesthesia Type...", Page.Page);
                return false;
            }
        }
        return true;

    }


}
