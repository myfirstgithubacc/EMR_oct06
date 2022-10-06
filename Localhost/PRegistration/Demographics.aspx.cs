using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PRegistration_Demographics : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private Hashtable hshTable = new Hashtable();
    private int PID = 0;
    private const string DefaultCountry = "DefaultCountry";
    private const string DefaultState = "DefaultState";
    private const string DefaultCity = "DefaultCity";
    private const string DefaultZip = "DefaultZip";
    private const string DefaultReligion = "DefaultReligion";
    private const string DefaultNationality = "DefaultNationality";
    private const string DefaultLanguage = "DefaultLanguage";
    BaseC.ParseData Parse = new BaseC.ParseData();
    protected UserControl uc1;
    private string Mode
    {
        get
        {
            return Convert.ToString(ViewState["Mode"]);
        }
        set
        {
            ViewState["Mode"] = value;
        }
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Page.MasterPageFile = "/Include/Master/BlankMaster.master";
            }
        }
    }
    public void GetPatintDetailsFromgRID()
{
    DataTable dt = new DataTable();
    try
    {

        if (Session["gvEncounterDetails"] != null)
        {

             dt = (DataTable)Session["gvEncounterDetails"];
             txtAccountNo.Text = common.myStr(dt.Rows[0]["lblRegistrationNo"]);
             populateControlsForEditing(common.myInt(dt.Rows[0]["lblRegistrationNo"]));


        }
    }

    catch (Exception ex)
    {

    }
    finally
    {
        dt.Dispose();
        Session["gvEncounterDetails"] = null;

    }

}


    protected void Page_Load(object sender, EventArgs e)
    {
        BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
        ClinicDefaults cd = new ClinicDefaults(Page);

        try
        {
           

            lblMessage.Text = string.Empty;

            GetPatintDetailsFromgRID();

            lnkPatientRelation.Visible = false;
            lnkOtherDetails.Visible = false;
            lnkResponsibleParty.Visible = false;
            lnkPayment.Visible = false;
            lnkAttachDocument.Visible = false;
            lnkbtnBilling.Visible = false;



            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Login.aspx?Logout=1", false);
            }
            SetPermission();
            setControlHospitalbased();
           
            btnSave.Enabled = true;
           
            if (Request.QueryString["Mpg"] != null)
            {
                Session["RNo"] = txtRegNo.Text == "" ? Session["RNo"] : txtRegNo.Text;
            }
            if (!IsPostBack)
            {
                SetPermission(btnSave, "N", true);
                
                if (Request.QueryString["Mpg"] != null)
                {
                    Session["CurrentNode"] = Request.QueryString["Mpg"].ToString();
                    string pid = Session["CurrentNode"].ToString();
                    int len = pid.Length;
                    ViewState["PageId"] = Session["CurrentNode"].ToString().Substring(1, len - 1);
                }
                else
                {
                    ViewState["PageId"] = "0";
                }
                hdnCurrDate.Value = DateTime.Today.ToString("dd/MM/yyyy");
                PopulateControl();

               
                Page.RegisterStartupScript("myScript", "<script language=JavaScript>onCheckBoxload()</script>");
              
                imgCalYear.Attributes.Add("onload", " this.style.visibility = 'hidden';");
                txtYear.Attributes.Add("onblur", "javascript:CalCulateAge();");
                txtMonth.Attributes.Add("onblur", "javascript:CalCulateAge();");
                txtDays.Attributes.Add("onblur", "javascript:CalCulateAge();");
                btnCalAge.Attributes.Add("onload", " this.style.visibility = 'hidden';");
                txtLame.Attributes.Add("onblur", "nSat=1;");
                SetPermission(btnSave, "N", true);
                //CheckAllowAddCountryStateCityAreaMaster();
              
                lnkbtnAppointment.Visible = false;
               
                bindKinRelation();
                if (common.myStr(Session["ModuleFlag"]) == "ER")
                {
                    
                    txtFname.Text = (string)hsSetup.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), "ERPatientName", common.myInt(Session["FacilityId"]));
                }
                if (Convert.ToString(Request.QueryString["mode"]) == "E")
                {
                    Mode = "E";
                   
                    if (Request.QueryString["RNo"] != null || Session["RNo"] != null)
                    {
                        Session["RNo"] = Convert.ToString(Request.QueryString["RNo"]) == null ? Convert.ToString(Session["RNo"]) : Convert.ToString(Request.QueryString["RNo"]);
                        txtRegNo.Text = Convert.ToString(Request.QueryString["RNo"]) == null ? Convert.ToString(Session["RNo"]) : Convert.ToString(Request.QueryString["RNo"]);
                        txtAccountNo.Text = txtRegNo.Text;
                        btnGetInfo_Click(sender, e);
                    }
                }
                else if (Convert.ToString(Request.QueryString["mode"]) == "N")
                {
                    Mode = "N";
                    lblDemographics.Text = "Patient Demographics";
                    lblRegistration.Text = "New Patient";
                    if (common.myInt(Request.QueryString["AppID"]) > 0)
                    {
                        PopulateUnRegisterPatient(common.myInt(Request.QueryString["AppID"]));
                    }
                    if (common.myInt(Request.QueryString["ResAppID"]) > 0)
                    {
                        PopulateRESUnRegisterPatient(common.myInt(Request.QueryString["ResAppID"]));
                    }

                    populateControlsForEditingOTBooking();
                   

                }
                PatientInfo();
                
                rdoPayertype_OnSelectedIndexChanged(sender, e);
                if (Session["HospitalLocationId"] != null)
                {
                    
                    dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(cd.GetHospitalDefaults(DefaultCountry, Session["HospitalLocationId"].ToString())));
                    LocalCountry_SelectedIndexChanged(sender, e);
                    dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(cd.GetHospitalDefaults(DefaultState, Session["HospitalLocationId"].ToString())));
                    LocalState_SelectedIndexChanged(sender, e);
                    dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(cd.GetHospitalDefaults(DefaultCity, Session["HospitalLocationId"].ToString())));
                    LocalCity_OnSelectedIndexChanged(sender, e);
                   
                    dropNationality.SelectedIndex = dropNationality.Items.IndexOf(dropNationality.Items.FindByValue(cd.GetHospitalDefaults(DefaultNationality, Session["HospitalLocationId"].ToString())));
                }
                if (txtRegNo.Text != "")
                {
                    populateControlsForEditing(Convert.ToInt32(txtRegNo.Text));
                }
                if (txtSearchMobile.Text != "")
                {
                    btnSearchMob_Click(sender, e); // Mobile No search patient
                }
                HideField();
                setMandatoryFields();
                if (common.myStr(Session["ModuleFlag"]) == "ER")
                {
                    disableMandatoryFields();
                }
                //dropTitle.Focus();
                string AllowPatientAgeEntryinRegistration = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                common.myInt(Session["FacilityId"]), "AllowPatientAgeEntryinRegistration", sConString);

                if (AllowPatientAgeEntryinRegistration.ToUpper() == "Y")
                {
                    txtYear.ReadOnly = false;
                    txtMonth.ReadOnly = false;
                    txtDays.ReadOnly = false;
                }
                else
                {
                    txtYear.ReadOnly = true;
                    txtMonth.ReadOnly = true;
                    txtDays.ReadOnly = true;
                }
                txtRegistrationDate.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            objException=null;
            
        }
        finally
        {
            hsSetup = null;
            
        }

    }
  
   
    private void HideField()
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {
            
           
            ds = bC.GetHideFieldType(1);// 1 for registration module id
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToString(dr[0]) == "ExternalPatient" && Convert.ToString(dr[1]) == "False")
                    {
                        //ltrExternalPatient.Visible = false;
                        //chkExternalPatient.Visible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            bC = null;
            ds.Dispose();
        }
    }
    private void disableMandatoryFields()
    {
        try
        {
            lblMiddleNameStar.Visible = false;
            lblLastNameStar.Visible = false;
            lblGuardianNameStar.Visible = false;
            lblAddressStar.Visible = false;
            lblCountryStar.Visible = false;
            lblStateStar.Visible = false;
            lblCityStar.Visible = false;
            lblAreaStar.Visible = false;
            lblPhoneHomeStar.Visible = false;
            lblMobileNoStar.Visible = false;
            lblEmailStar.Visible = false;

            lblNationalityStar.Visible = false;
            lblZipStar.Visible = false;

            lblIdentityTypeStar.Visible = false;

            lblVIPNarrationStar.Visible = false;
            lblTitle.Visible = false;
            lblIdentityNoStar.Visible = false;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            objException = null;

        }
        
       
    }
    private void IsMandatory(DataView dv, Label LBL, string FieldName)
    {
        try
        {
            dv.RowFilter = "FieldName = '" + FieldName + "'";
            if (dv.Count > 0)
            {
                LBL.Visible = common.myBool(dv.ToTable().Rows[0]["Active"]);
            }
            dv.RowFilter = "";
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void setMandatoryFields()
    {
        BaseC.HospitalSetup hsSetup = new BaseC.HospitalSetup(sConString);
        try
        {
            disableMandatoryFields();
            
            DataSet dsPage = hsSetup.GetPageMandatoryFields(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "Registration", "Demographics");
            if (dsPage.Tables[0].Rows.Count > 0)
            {
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblMiddleNameStar, "MiddleName");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblLastNameStar, "LastName");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblGuardianNameStar, "GuardianName");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblAddressStar, "Address");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblCountryStar, "Country");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblStateStar, "State");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblCityStar, "City");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblAreaStar, "Area");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblPhoneHomeStar, "PhoneHome");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblMobileNoStar, "MobileNo");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblEmailStar, "Email");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblNationalityStar, "Nationality");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblZipStar, "Zip");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblIdentityTypeStar, "IdentityType");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblIdentityNoStar, "IdentityNo");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblVIPNarrationStar, "VIPNarration");
                IsMandatory(dsPage.Tables[0].Copy().DefaultView, lblTitle, "Title");
               
            }
            dsPage.Dispose ();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            hsSetup = null;
          

        }
    }
    private void PatientInfo()
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        try
        {
            
            if (Convert.ToString(txtAccountNo.Text.Trim()) != "")
            {
                
                string sXSL = "";
                
                ds = objCM.getPatientDetails(Convert.ToInt32(txtAccountNo.Text.Trim()), 1, Convert.ToInt32(Session["HospitalLocationID"]));
                sXSL = "/include/xsl/PatientInfo-US.xsl";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString() != "Patient Is Expired, No Transaction Allowed!")
                        {
                            xmlPatientInfo.DocumentContent = ds.Tables[0].Rows[0][0].ToString();
                            xmlPatientInfo.TransformSource = sXSL;
                            xmlPatientInfo.DataBind();
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = ds.Tables[0].Rows[0][0].ToString();
                            xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                            xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                            xmlPatientInfo.DataBind();
                        }
                    }
                    else
                    {
                        //Alert.ShowAjaxMsg("No patient record found ", Page);
                        xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                        xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                        xmlPatientInfo.DataBind();
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("No patient record found ", Page);
                    xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                    xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                    xmlPatientInfo.DataBind();
                }
            }
            else
            {
                xmlPatientInfo.DocumentSource = "/Include/XML/DefaultPatientInfoOP.xml";
                xmlPatientInfo.TransformSource = "/include/xsl/PatientInfo-US.xsl";
                xmlPatientInfo.DataBind();
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objCM = null;

        }
    }
    
    private void populateControlsForEditing(int PID)
    {
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
        try
        {

            SetPermission();

            DataSet objDs = (DataSet)objPatient.GetPatientRecord(common.myInt(txtAccountNo.Text), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
            if (objDs != null)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    //DeleteFiles();
                    hdnRegistrationId.Value = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);
                    txtAccountNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationNo"]);
                    txtRegNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);

                    AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(txtAccountNo.Text), 0, Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
                    txtRegistrationDate.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationDate"]);
                    txtFname.Text = objDs.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                    txtMName.Text = objDs.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
                    txtLame.Text = objDs.Tables[0].Rows[0]["LastName"].ToString().Trim();
                    ViewState["PName"] = txt_hdn_PName.Text;
                    string[] str = Convert.ToString(objDs.Tables[0].Rows[0]["PreviousName"].ToString()).Split('#');
                    if (str.Length == 2)
                    {
                        ddlParentof.SelectedValue = str[0].ToString().Trim();
                        txtParentof.Text = str[1].ToString().Trim();
                    }
                    //if (chkNewBorn.Checked == false)
                    //{
                    dtpDateOfBirth.Text = Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateofBirth"]).ToString("dd/MM/yyyy");
                    txtYear.Text = objDs.Tables[0].Rows[0]["AgeYear"].ToString().Trim();
                    txtMonth.Text = objDs.Tables[0].Rows[0]["AgeMonth"].ToString().Trim();
                    txtDays.Text = objDs.Tables[0].Rows[0]["AgeDays"].ToString().Trim();
                    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["Gender"]) != "")
                    {
                        dropSex.SelectedIndex = dropSex.Items.IndexOf(dropSex.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["Gender"])));
                    }
                    if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                    {
                        btnCalAge_Click(null, null);
                    }
                    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
                    //}
                    //else
                    //{
                    //    dropTitle.SelectedValue = common.myStr(ViewState["IsnewBornTitle"]);
                    //    dtpDateOfBirth.Text = Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateofBirth"]).ToString("dd/MM/yyyy");
                    //}
                    if ((common.myStr(objDs.Tables[0].Rows[0]["MotherRegistrationNo"].ToString().Trim()) != "0") && (common.myStr(objDs.Tables[0].Rows[0]["MotherRegistrationNo"].ToString().Trim()) != ""))
                    {
                        // chkNewBorn.Checked = common.myBool(objDs.Tables[0].Rows[0]["NewBorn"].ToString().Trim());
                        //if (chkNewBorn.Checked)
                        //{
                        //    txtMothergNo.Text = objDs.Tables[0].Rows[0]["MotherRegistrationNo"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    txtMothergNo.Text = "";
                        //}
                    }
                    //else
                    //{
                    //    chkNewBorn.Checked = common.myBool(objDs.Tables[0].Rows[0]["NewBorn"].ToString().Trim());
                    //    txtMothergNo.Text = "";
                    //}
                    // chkExternalPatient.Checked = Convert.ToBoolean(objDs.Tables[0].Rows[0]["ExternalPatient"]) ? true : false;
                    //if (Convert.ToString(objDs.Tables[0].Rows[0]["MaritalStatus"].ToString()) != "")
                    //{
                    //    dropMarital.SelectedIndex = dropMarital.Items.IndexOf(dropMarital.Items.FindByValue(objDs.Tables[0].Rows[0]["MaritalStatus"].ToString()));
                    //}
                    ////if (Convert.ToString(objDs.Tables[0].Rows[0]["HCFID"].ToString()) != "")
                    ////{
                    ////    ddlHealthCareFacilitator.SelectedIndex = ddlHealthCareFacilitator.Items.IndexOf(ddlHealthCareFacilitator.Items.FindByValue(objDs.Tables[0].Rows[0]["HCFID"].ToString()));
                    ////}
                    ////if (Convert.ToString(objDs.Tables[0].Rows[0]["FacilityID"].ToString()) != "")
                    ////{
                    ////    ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(objDs.Tables[0].Rows[0]["FacilityID"].ToString()));
                    ////}
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]) != "")
                        dropIdentityType.SelectedIndex = dropIdentityType.Items.IndexOf(dropIdentityType.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]).Trim()));

                    txtIdentityNumber.Text = objDs.Tables[0].Rows[0]["IdentityNumber"].ToString().Trim();
                    txtLAddress1.Text = objDs.Tables[0].Rows[0]["LocalAddress"].ToString().Trim();
                    txtLAddress2.Text = objDs.Tables[0].Rows[0]["LocalAddress2"].ToString().Trim();
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"]) != "")
                    {
                        dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"])));
                        LocalCountry_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLCountry.SelectedIndex = -1;
                    }
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]) != "")
                    {
                        dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]).Trim()));
                        LocalState_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLState.SelectedIndex = -1;
                    }
                    if (common.myStr(objDs.Tables[0].Rows[0]["LocalCity"]) != "")
                    {
                        dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCity"]).Trim()));
                        LocalCity_OnSelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLCity.SelectedIndex = -1;
                    }
                    if (common.myStr(objDs.Tables[0].Rows[0]["CityAreaId"]) != "")
                    {
                        ddlCityArea.SelectedIndex = ddlCityArea.Items.IndexOf(ddlCityArea.Items.FindByValue(objDs.Tables[0].Rows[0]["CityAreaId"].ToString().Trim()));
                    }
                    else
                    {
                        ddlCityArea.SelectedIndex = -1;
                    }
                    //if (common.myStr(objDs.Tables[0].Rows[0]["OccupationId"]) != "")
                    //{
                    //    //ddlOccupation.SelectedIndex = ddlCityArea.Items.IndexOf(ddlCityArea.Items.FindItemByValue(objDs.Tables[0].Rows[0]["OccupationId"].ToString().Trim()));
                    //    ddlOccupation.SelectedValue = objDs.Tables[0].Rows[0]["OccupationId"].ToString().Trim();
                    //}
                    //else
                    //{
                    //    ddlOccupation.SelectedIndex = -1;
                    //}
                    txtLPhoneRes.Text = objDs.Tables[0].Rows[0]["PhoneHome"].ToString();
                    txtPMobile.Text = objDs.Tables[0].Rows[0]["MobileNo"].ToString();
                    if (objDs.Tables[0].Rows[0]["Localpin"] != null)
                    {

                        txtLPin.Text = objDs.Tables[0].Rows[0]["Localpin"].ToString().Trim();
                    }
                    txtPEmailID.Text = objDs.Tables[0].Rows[0]["Email"].ToString();
                    if (objDs.Tables[0].Rows[0]["NationalityId"] != null)
                    {
                        dropNationality.SelectedIndex = dropNationality.Items.IndexOf(dropNationality.Items.FindByValue(objDs.Tables[0].Rows[0]["NationalityId"].ToString()));
                    }
                    //if (objDs.Tables[0].Rows[0]["ReligionId"] != null)
                    //{
                    //    dropReligion.SelectedIndex = dropReligion.Items.IndexOf(dropReligion.Items.FindByValue(objDs.Tables[0].Rows[0]["ReligionId"].ToString()));
                    //}
                    if (objDs.Tables[0].Rows[0]["ReferredTypeID"] != null && common.myStr(objDs.Tables[0].Rows[0]["ReferredTypeID"]) != "0")
                    {
                        ////for (int i = 0; i < dropReferredType.Items.Count; i++)
                        ////{
                        ////    if (common.myStr(objDs.Tables[0].Rows[0]["ReferredTypeID"]) == common.myStr(dropReferredType.Items[i].Attributes["Id"]))
                        ////    {
                        ////        dropReferredType.SelectedIndex = dropReferredType.Items.IndexOf(dropReferredType.Items.FindByText(dropReferredType.Items[i].Text));
                        ////        break;
                        ////    }
                        ////}
                        ////RefType_SelectedIndexChanging(this, null);
                        ////if (common.myStr(objDs.Tables[0].Rows[0]["ReferredByID"]) != "")
                        ////{
                        ////    dropRefBy.SelectedIndex = dropRefBy.Items.IndexOf(dropRefBy.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["ReferredByID"])));
                        ////}
                        ////else
                        ////{
                        ////    dropRefBy.Items.Insert(0, new ListItem("Select"));
                        ////    dropRefBy.Items[0].Value = "0";
                        ////    dropRefBy.SelectedValue = "0";
                        ////}
                    }
                    else
                    {
                        //dropReferredType.SelectedValue = "0";                       
                        ////dropRefBy.Items.Insert(0, new ListItem("Select"));
                        ////dropRefBy.Items[0].Value = "0";
                        ////dropRefBy.SelectedValue = "0";
                    }
                    ////if (objDs.Tables[0].Rows[0]["RenderingProvider"] != null)
                    ////{
                    ////    ddlRenderingProvider.SelectedValue = objDs.Tables[0].Rows[0]["RenderingProvider"].ToString();
                    ////}
                    // ddlStatus.SelectedValue = objDs.Tables[0].Rows[0]["Status"].ToString().TrimEnd();
                    txtSSN.Text = objDs.Tables[0].Rows[0]["SocialSecurityNo"].ToString();


                    //if (objDs.Tables[0].Rows[0]["LeadSourceId"] != null)
                    //{
                    //    ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(objDs.Tables[0].Rows[0]["LeadSourceId"].ToString()));
                    //}
                    if (common.myStr(Session["DefaultCompany"]) == common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString()) &&
                      common.myStr(Session["DefaultCompany"]) == common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString()))
                    {
                        ////rdoPayertype.SelectedValue = "0";
                        ////ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ////rdoPayertype_OnSelectedIndexChanged(this, null);
                        ////ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                        ////ddlCompany.Enabled = false;
                    }
                    else if (common.myBool(objDs.Tables[0].Rows[0]["IsInsuranceCompany"]) == false)
                    {
                        ////ddlCompany.Enabled = true;
                        ////rdoPayertype.SelectedValue = "1";
                        ////rdoPayertype_OnSelectedIndexChanged(this, null);
                        ////ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ////ddlCompany_OnSelectedIndexChanged(null, null);
                        ////ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                    }
                    else if (common.myBool(objDs.Tables[0].Rows[0]["IsInsuranceCompany"]) == true)
                    {
                        ////ddlCompany.Enabled = true;
                        ////rdoPayertype.SelectedValue = "2";
                        ////rdoPayertype_OnSelectedIndexChanged(this, null);
                        ////ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ////ddlCompany_OnSelectedIndexChanged(null, null);
                        ////ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                    }
                    ////if (common.myInt(ddlCompany.SelectedValue) > 0 && (common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["StaffCompanyId"]) || common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["StaffDependentCompanyId"])))
                    ////    tblEmpDetails.Visible = true;
                    ////txtEmpNo.Text = objDs.Tables[0].Rows[0]["TaggedEmpNo"].ToString();
                    ////txtEmpNo_OnTextChanged(this, null);
                    ////ddlKinRelation.SelectedIndex = ddlKinRelation.Items.IndexOf(ddlKinRelation.Items.FindByText(objDs.Tables[0].Rows[0]["TaggedEmpName"].ToString()));
                    //chkVIP.Checked = common.myBool(objDs.Tables[0].Rows[0]["VIP"].ToString());
                    txtVIPNarration.Text = common.myStr(objDs.Tables[0].Rows[0]["VIPNarration"].ToString());
                    // txtEmergencyName.Text = common.myStr(objDs.Tables[0].Rows[0]["NotificationName"].ToString());
                    //txtEmergencyContactNo.Text = common.myStr(objDs.Tables[0].Rows[0]["NotificationPhoneNo"].ToString());
                    // txtEmiratesID.Text = common.myStr(objDs.Tables[0].Rows[0]["EmiratesID"]);
                    // txtMothername.Text = common.myStr(objDs.Tables[0].Rows[0]["MotherName"]); //Added  for child Trust    
                    //txtFatherName.Text = common.myStr(objDs.Tables[0].Rows[0]["FatherName"]); //Added  for child Trust    

                    //if (common.myInt(objDs.Tables[0].Rows[0]["FamilyMonthlyIncome"]) > 0)
                    //{
                    //    txtFamilyMonthlyIncome.Text = common.myStr(common.myInt(objDs.Tables[0].Rows[0]["FamilyMonthlyIncome"])); //Added  for child Trust    
                    //}
                    //else
                    //{
                    //    txtFamilyMonthlyIncome.Text = String.Empty;
                    //}
                    ViewState["UHID"] = txtRegNo.Text;
                    String strFullName = objDs.Tables[0].Rows[0]["FirstName"].ToString() + " " + objDs.Tables[0].Rows[0]["MiddleName"].ToString() + " " + objDs.Tables[0].Rows[0]["LastName"].ToString();
                    lnkOtherDetails.Visible = false;
                    lnkPayment.Visible = false;
                    lnkResponsibleParty.Visible = false;
                    if (common.myStr(Session["ModuleFlag"]) == "ER")
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    else
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    if (Request.QueryString["Category"] != null)
                    {
                        if (Request.QueryString["Category"].ToString() == "PopUp")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            if (common.myStr(Session["ModuleFlag"]) == "ER")
                            {
                                lnkbtnAppointment.Visible = false;
                                lnkbtnBilling.Visible = false;
                            }
                            else
                            {
                                lnkbtnAppointment.Visible = true;
                                lnkbtnBilling.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (common.myStr(Session["ModuleFlag"]) == "ER")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            lnkbtnAppointment.Visible = true;
                            lnkbtnBilling.Visible = false;
                        }
                    }
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        ShowImage();
                    }
                    else
                        PatientImage.ImageUrl = "~/Images/patientLeft.jpg";

                    lblMessage.Text = "";
                    btnSave.Text = "Update";
                    SetPermission(btnSave, "E", true);
                }
            }
            objDs.Dispose();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPatient=null;
            AuditCA=null;
            objwcf = null;

        }
    }
    private void populateControlsForEditingOTBooking()
    {
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
        try
        {

            SetPermission();

            DataSet objDs = new DataSet();
            BaseC.RestFulAPI objwcfOt = new BaseC.RestFulAPI(sConString);
            objDs = objwcfOt.getOTBookingDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["OTBookingId"]), "");
            hdnOTBookingId.Value = common.myStr(Request.QueryString["OTBookingId"]);
            if (objDs != null)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    //DeleteFiles();
                    //hdnRegistrationId.Value = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);
                    //txtAccountNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationNo"]);
                    //txtRegNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);

                    //AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(txtAccountNo.Text), 0, Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
                    //txtRegistrationDate.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationDate"]);
                    txtFname.Text = objDs.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                    txtMName.Text = objDs.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
                    txtLame.Text = objDs.Tables[0].Rows[0]["LastName"].ToString().Trim();
                    ViewState["PName"] = txt_hdn_PName.Text;
                    dtpDateOfBirth.Text = Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateOfBirth"]).ToString("dd/MM/yyyy");
                    txtYear.Text = objDs.Tables[0].Rows[0]["AgeYears"].ToString().Trim();
                    txtMonth.Text = objDs.Tables[0].Rows[0]["AgeMonths"].ToString().Trim();
                    txtDays.Text = objDs.Tables[0].Rows[0]["AgeDays"].ToString().Trim();
                    //dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["GenderId"]) != "")
                    {
                        dropSex.SelectedIndex = dropSex.Items.IndexOf(dropSex.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["GenderId"])));
                    }
                    if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                    {
                        btnCalAge_Click(null, null);
                    }
                    
                   
                   
                    //if (Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]) != "")
                    //    dropIdentityType.SelectedIndex = dropIdentityType.Items.IndexOf(dropIdentityType.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]).Trim()));

                    //txtIdentityNumber.Text = objDs.Tables[0].Rows[0]["IdentityNumber"].ToString().Trim();
                    //txtLAddress1.Text = objDs.Tables[0].Rows[0]["LocalAddress"].ToString().Trim();
                    //txtLAddress2.Text = objDs.Tables[0].Rows[0]["LocalAddress2"].ToString().Trim();
                    //if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"]) != "")
                    //{
                    //    dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"])));
                    //    LocalCountry_SelectedIndexChanged(this, null);
                    //}
                    //else
                    //{
                    //    dropLCountry.SelectedIndex = -1;
                    //}
                    //if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]) != "")
                    //{
                    //    dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]).Trim()));
                    //    LocalState_SelectedIndexChanged(this, null);
                    //}
                    //else
                    //{
                    //    dropLState.SelectedIndex = -1;
                    //}
                    //if (common.myStr(objDs.Tables[0].Rows[0]["LocalCity"]) != "")
                    //{
                    //    dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCity"]).Trim()));
                    //    LocalCity_OnSelectedIndexChanged(this, null);
                    //}
                    //else
                    //{
                    //    dropLCity.SelectedIndex = -1;
                    //}
                    //if (common.myStr(objDs.Tables[0].Rows[0]["CityAreaId"]) != "")
                    //{
                    //    ddlCityArea.SelectedIndex = ddlCityArea.Items.IndexOf(ddlCityArea.Items.FindByValue(objDs.Tables[0].Rows[0]["CityAreaId"].ToString().Trim()));
                    //}
                    //else
                    //{
                    //    ddlCityArea.SelectedIndex = -1;
                    //}
                    
                    //txtLPhoneRes.Text = objDs.Tables[0].Rows[0]["PhoneHome"].ToString();
                    txtPMobile.Text = objDs.Tables[0].Rows[0]["MobileNo"].ToString();
                    //if (objDs.Tables[0].Rows[0]["Localpin"] != null)
                    //{

                    //    txtLPin.Text = objDs.Tables[0].Rows[0]["Localpin"].ToString().Trim();
                    //}
                    //txtPEmailID.Text = objDs.Tables[0].Rows[0]["Email"].ToString();
                    //if (objDs.Tables[0].Rows[0]["NationalityId"] != null)
                    //{
                    //    dropNationality.SelectedIndex = dropNationality.Items.IndexOf(dropNationality.Items.FindByValue(objDs.Tables[0].Rows[0]["NationalityId"].ToString()));
                    //}
                    
                   
                    //txtSSN.Text = objDs.Tables[0].Rows[0]["SocialSecurityNo"].ToString();


                   
                   
                    ViewState["UHID"] = txtRegNo.Text;
                    String strFullName = objDs.Tables[0].Rows[0]["FirstName"].ToString() + " " + objDs.Tables[0].Rows[0]["MiddleName"].ToString() + " " + objDs.Tables[0].Rows[0]["LastName"].ToString();
                    lnkOtherDetails.Visible = false;
                    lnkPayment.Visible = false;
                    lnkResponsibleParty.Visible = false;
                    if (common.myStr(Session["ModuleFlag"]) == "ER")
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    else
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    if (Request.QueryString["Category"] != null)
                    {
                        if (Request.QueryString["Category"].ToString() == "PopUp")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            if (common.myStr(Session["ModuleFlag"]) == "ER")
                            {
                                lnkbtnAppointment.Visible = false;
                                lnkbtnBilling.Visible = false;
                            }
                            else
                            {
                                lnkbtnAppointment.Visible = true;
                                lnkbtnBilling.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (common.myStr(Session["ModuleFlag"]) == "ER")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            lnkbtnAppointment.Visible = true;
                            lnkbtnBilling.Visible = false;
                        }
                    }
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        ShowImage();
                    }
                    else
                        PatientImage.ImageUrl = "~/Images/patientLeft.jpg";

                    lblMessage.Text = "";
                    //btnSave.Text = "Update";
                    SetPermission(btnSave, "E", true);
                }
            }
            objDs.Dispose();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objPatient = null;
            AuditCA = null;
            objwcf = null;

        }
    }
    protected void imgCalYear_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.Patient bC = new BaseC.Patient(sConString);
        try
        {

            DateTime parsed;
            bool valid = DateTime.TryParseExact(dtpDateOfBirth.Text, "dd/MM/yyyy",
                                                CultureInfo.InvariantCulture,
                                                DateTimeStyles.None,
                                                out parsed);
            string date = common.myStr(parsed);

     

            if (dtpDateOfBirth.Text != "__/__/____")
            {

                txtYear.Text = "";
                txtMonth.Text = "";
                txtDays.Text = "";
                DateTime datet = DateTime.ParseExact(dtpDateOfBirth.Text, "dd/MM/yyyy", null);
               
                string[] result = bC.CalculateAge(datet.ToString("yyyy/MM/dd"));
                if (result.Length == 2)
                {
                    if (result[1] == "Yr")
                    {
                        txtYear.Text = result[0];
                    }
                    else if (result[1] == "Mnth")
                    {
                        txtMonth.Text = result[0];
                    }
                    else
                    {
                        txtDays.Text = result[0];
                    }

                    if (txtDays.Text.Contains("-"))
                    {

                        Alert.ShowAjaxMsg("You cannot select future day!", this.Page);

                        txtYear.Text = "";
                        txtMonth.Text = "";
                        txtDays.Text = "";
                        dtpDateOfBirth.Text = string.Empty;
                        return;
                    }

                }
                if (result.Length == 4)
                {
                    txtMonth.Text = result[0];
                    txtDays.Text = result[2];
                }
                if (result.Length == 6)
                {
                    txtYear.Text = result[0];
                    txtMonth.Text = result[2];
                    txtDays.Text = result[4];
                }
                if (txtYear.Text == "")
                {
                    txtYear.Text = "0";
                }
                if (txtMonth.Text == "")
                {
                    txtMonth.Text = "0";
                }
                if (txtDays.Text == "")
                {
                    txtDays.Text = "0";
                }
                txtParentof.Focus();
            }
            else
            {
                txtYear.Focus();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error:  This Date  format is not supported in calendar ";
            dtpDateOfBirth.Text = string.Empty;

            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            bC = null;

        }
         

    }
    protected void Title_SelectedIndexChanged(object sender, EventArgs e)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            string strGender = common.myStr(dropTitle.SelectedItem.Attributes["Gender"].ToString());
            if (strGender == "M")
            {
                dropSex.SelectedIndex = 1;
            }
            else if (strGender == "F")
            {
                dropSex.SelectedIndex = 2;
            }
            else
            {
                dropSex.SelectedIndex = 0;
            }
            txtFname.Focus();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            dl = null;

        }

    }
    protected void btnGetInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myLen(txtAccountNo.Text) > 0 && Convert.ToInt64(txtAccountNo.Text) > 2147483647)
            {
                Alert.ShowAjaxMsg("Value should not be more than 2147483647.", this.Page);
                txtAccountNo.Text = txtAccountNo.Text.Substring(0, 9);
                return;
            }
            if (common.myStr(Session["ModuleFlag"]).Equals("ER"))
            {
                BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                if (objEMR.IsPatientExpire(common.myStr(txtAccountNo.Text))
                || objEMR.IsPatientExpire(common.myStr(hdnRegistrationNo.Value)))
                {
                    Alert.ShowAjaxMsg("You Cannot Create Visit For Death patient!!!!", Page.Page);
                    return;
                }
            }
            SetPermission();
            
            if (Convert.ToInt32(Request.QueryString["RNo"]) != 0)
            {
                hdnRegistrationNo.Value = Convert.ToString(Request.QueryString["RNo"]);
            }
            if (common.myStr(txtAccountNo.Text) != "")
            {
                hdnRegistrationNo.Value = txtAccountNo.Text;
            }
          
            if ((txtAccountNo.Text.Trim() != "0") && (txtAccountNo.Text.Trim() != "") || (hdnRegistrationNo.Value != "" && hdnRegistrationNo.Value != "0"))
            {
                txtAccountNo.Text = hdnRegistrationNo.Value;
                DeleteFiles();
                PatientInfo();
                rdoPayertype_OnSelectedIndexChanged(sender, e);
                Mode = "E";
                populateControlsForEditing(Convert.ToInt32(txtAccountNo.Text));
            }
            else
            {
                ClearFields();
                DeleteFiles();
                Mode = "N";
                if (common.myInt(Request.QueryString["AppID"]) > 0)
                {
                    PopulateUnRegisterPatient(common.myInt(Request.QueryString["AppID"]));
                }
                if (common.myInt(Request.QueryString["ResAppID"]) > 0)
                {
                    PopulateRESUnRegisterPatient(common.myInt(Request.QueryString["ResAppID"]));
                }
            }
            BindMessage();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
            objException = null;
        }
      
    }
    private void PopulateUnRegisterPatient(int iAppointmentID)
    {
        BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
        DataSet objDs = (DataSet)objwcf.GetUnRegistedPatientRecord(Convert.ToInt32(Session["HospitalLocationID"]), Convert.ToInt32(iAppointmentID));
        try
        {

            if (objDs.Tables[0].Rows.Count > 0)
            {
                txtAccountNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationNo"]);
                dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));

                txtFname.Text = objDs.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                txtMName.Text = objDs.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
                txtLame.Text = objDs.Tables[0].Rows[0]["LastName"].ToString().Trim();
                if (objDs.Tables[0].Rows[0]["DateofBirth"].ToString().Trim() != "01-01-1900")
                {
                    dtpDateOfBirth.Text = Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateofBirth"]).ToString("dd/MM/yyyy"); // Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateofBirth"]).ToString();
                }
                txtYear.Text = objDs.Tables[0].Rows[0]["AgeYear"].ToString().Trim();
                txtMonth.Text = objDs.Tables[0].Rows[0]["AgeMonth"].ToString().Trim();
                txtDays.Text = objDs.Tables[0].Rows[0]["AgeDays"].ToString().Trim();
                if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                {
                    btnCalAge_Click(null, null);
                }
                if (Convert.ToString(objDs.Tables[0].Rows[0]["Gender"]) != "")
                {
                    dropSex.SelectedIndex = dropSex.Items.IndexOf(dropSex.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["Gender"])));
                }
                
                txtPMobile.Text = objDs.Tables[0].Rows[0]["MobileNo"].ToString();
                txtPEmailID.Text = objDs.Tables[0].Rows[0]["Email"].ToString();
                if (objDs.Tables[0].Rows[0]["ReferredTypeID"] != null && Convert.ToString(objDs.Tables[0].Rows[0]["ReferredTypeID"]) != "0")
                {
                    
                    RefType_SelectedIndexChanging(this, null);
                    
                    
                }
               
               
                ViewState["UHID"] = txtRegNo.Text;
                String strFullName = objDs.Tables[0].Rows[0]["FirstName"].ToString() + " " + objDs.Tables[0].Rows[0]["MiddleName"].ToString() + " " + objDs.Tables[0].Rows[0]["LastName"].ToString();
                lnkOtherDetails.Visible = false;
                lnkPayment.Visible = false;
                lnkResponsibleParty.Visible = false;
                if (common.myStr(Session["ModuleFlag"]) == "ER")
                {
                    lnkPatientRelation.Visible = false;
                    lnkAttachDocument.Visible = false;
                    LblLabelname.Visible = false;
                    lnkbtnAppointment.Visible = false;
                    lnkbtnBilling.Visible = false;
                }
                else
                {
                    lnkPatientRelation.Visible = false;
                    lnkAttachDocument.Visible = false;
                    LblLabelname.Visible = false;
                    lnkbtnAppointment.Visible = true;
                    lnkbtnBilling.Visible = false;
                }
                PatientImage.ImageUrl = "~/Images/patientLeft.jpg";
                lblMessage.Text = "";
                btnSave.Text = "Save";
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objwcf = null;
            objDs.Dispose(); 
        }
    }
    private void PopulateRESUnRegisterPatient(int iAppointmentID)
    {
        BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
        DataSet objDs = (DataSet)objwcf.GetUnRegistedPatientRecord(common.myInt(Session["HospitalLocationID"]), common.myInt(iAppointmentID));
        try
        {

            if (objDs.Tables[1].Rows.Count > 0)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
                }
                txtAccountNo.Text = Convert.ToString(objDs.Tables[1].Rows[0]["RegistrationNo"]);

                txtFname.Text = objDs.Tables[1].Rows[0]["FirstName"].ToString().Trim();
                txtMName.Text = objDs.Tables[1].Rows[0]["MiddleName"].ToString().Trim();
                txtLame.Text = objDs.Tables[1].Rows[0]["LastName"].ToString().Trim();
                if (objDs.Tables[1].Rows[0]["DateofBirth"].ToString().Trim() != "01-01-1900")
                {
                    dtpDateOfBirth.Text = Convert.ToString(objDs.Tables[1].Rows[0]["DateofBirth"]);
                }
                txtYear.Text = objDs.Tables[1].Rows[0]["AgeYear"].ToString().Trim();
                txtMonth.Text = objDs.Tables[1].Rows[0]["AgeMonth"].ToString().Trim();
                txtDays.Text = objDs.Tables[1].Rows[0]["AgeDays"].ToString().Trim();
                if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                {
                    btnCalAge_Click(null, null);
                }
                if (Convert.ToString(objDs.Tables[1].Rows[0]["Gender"]) != "")
                {
                    dropSex.SelectedIndex = dropSex.Items.IndexOf(dropSex.Items.FindByValue(Convert.ToString(objDs.Tables[1].Rows[0]["Gender"])));
                }
                txtLAddress1.Text = objDs.Tables[1].Rows[0]["PatientAddress"].ToString();
                txtPMobile.Text = objDs.Tables[1].Rows[0]["MobileNo"].ToString();
               
                ViewState["UHID"] = txtRegNo.Text;
                String strFullName = objDs.Tables[1].Rows[0]["FirstName"].ToString() + " " + objDs.Tables[1].Rows[0]["MiddleName"].ToString() + " " + objDs.Tables[1].Rows[0]["LastName"].ToString();
                lnkOtherDetails.Visible = false;
                lnkPayment.Visible = false;
                lnkResponsibleParty.Visible = false;
                if (common.myStr(Session["ModuleFlag"]) == "ER")
                {
                    lnkPatientRelation.Visible = false;
                    lnkAttachDocument.Visible = false;
                    LblLabelname.Visible = false;
                    lnkbtnAppointment.Visible = false;
                    lnkbtnBilling.Visible = false;
                }
                else
                {
                    lnkPatientRelation.Visible = false;
                    lnkAttachDocument.Visible = false;
                    LblLabelname.Visible = false;
                    lnkbtnAppointment.Visible = true;
                    lnkbtnBilling.Visible = false;
                }
                PatientImage.ImageUrl = "~/Images/patientLeft.jpg";
                lblMessage.Text = "";
                btnSave.Text = "Save";
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objwcf = null;
            objDs.Dispose(); 

        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            if ((Convert.ToString(ViewState["SaveData"]) == "N"))// && (txtMothergNo.Text.Length > 0))
            {
                Alert.ShowAjaxMsg("Please enter correct mother registration no", Page);
                return;
            }
            int iArea = 0;
            BaseC.Patient bC = new BaseC.Patient(sConString);
            if (dropSex.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please Select Gender", Page);
                dropSex.Focus();
                return;
            }
            if (ddlCityArea.SelectedValue != "0" && ddlCityArea.SelectedValue != "" && ddlCityArea.SelectedValue != "Select")
            {
                iArea = Convert.ToInt32(ddlCityArea.SelectedValue);
            }
            if (Page.IsValid)
            {
                if (dropSex.SelectedValue == "0")
                {
                    Alert.ShowAjaxMsg("Please Select the Gender !", Page);
                    return;
                }
                DateTime datet = new DateTime();

                
                if (dtpDateOfBirth.Text != "")
                {
                    datet = DateTime.ParseExact(dtpDateOfBirth.Text, "dd/MM/yyyy", null);
                    bC.DOB = datet.ToString("dd/MM/yyyy");

                }
               
                if (common.myStr(Session["ModuleFlag"]) != "ER")
                {
                    if (lblMiddleNameStar.Visible)
                    {
                        if (common.myStr(txtMName.Text).Trim().Length == 0)
                        {
                            Alert.ShowAjaxMsg("Please! enter patient Middle Name..", Page);
                            return;
                        }
                    }
                    if (lblLastNameStar.Visible)
                    {
                        if (common.myStr(txtLame.Text).Trim().Length == 0)
                        {
                            Alert.ShowAjaxMsg("Please! enter patient Last Name..", Page);
                            return;
                        }
                    }
                    if (lblGuardianNameStar.Visible)
                    {
                        if (common.myStr(txtParentof.Text).Trim().Length == 0)
                        {
                            Alert.ShowAjaxMsg("Please! Enter Guardian Name ..", Page);
                            return;
                        }
                    }
                   
                    if (lblAddressStar.Visible)
                    {
                        if (common.myStr(txtLAddress1.Text).Trim().Length == 0)
                        {
                            Alert.ShowAjaxMsg("Please! enter address ..", Page);
                            return;
                        }
                    }
                    if (lblCountryStar.Visible)
                    {
                        if (common.myInt(dropLCountry.SelectedValue) == 0)
                        {
                            Alert.ShowAjaxMsg("Please! select country ..", Page);
                            return;
                        }
                    }
                    if (lblStateStar.Visible)
                    {
                        if (common.myInt(dropLState.SelectedValue) == 0)
                        {
                            Alert.ShowAjaxMsg("Please! select State ..", Page);
                            return;
                        }
                    }

                    if (lblCityStar.Visible)
                    {
                        if (common.myInt(dropLCity.SelectedValue) == 0)
                        {
                            Alert.ShowAjaxMsg("Please! select city ..", Page);
                            return;
                        }
                    }
                    if (lblAreaStar.Visible)
                    {
                        if (common.myInt(ddlCityArea.SelectedValue) == 0)
                        {
                            Alert.ShowAjaxMsg("Please! select area ..", Page);
                            return;
                        }
                    }

                    if (lblZipStar.Visible)
                    {
                        if (common.myStr(txtLPin.Text).Trim().Length == 0)
                        {
                            Alert.ShowAjaxMsg("Please enter Zip !", Page);
                            return;
                        }
                    }

                    if (lblPhoneHomeStar.Visible)
                    {
                        if (common.myStr(txtLPhoneRes.Text).Trim() == "")
                        {
                            Alert.ShowAjaxMsg("Please! Enter a patient Home Phone No...", Page);
                            return;
                        }
                    }
                    if (lblMobileNoStar.Visible)
                    {
                        if (common.myStr(txtPMobile.Text).Trim() == "")
                        {
                            Alert.ShowAjaxMsg("Please! Enter a patient mobile no...", Page);
                            return;
                        }
                    }
                    if (lblEmailStar.Visible)
                    {
                        if (common.myStr(txtPEmailID.Text).Trim() == "")
                        {
                            Alert.ShowAjaxMsg("Please! Enter a patient Email ...", Page);
                            return;
                        }
                    }
                   
                    if (lblNationalityStar.Visible)
                    {
                        if (common.myInt(dropNationality.SelectedValue) == 0)
                        {
                            Alert.ShowAjaxMsg("Please Select Nationality !", Page);
                            return;
                        }
                    }
                    
                   
                    if (lblIdentityTypeStar.Visible)
                    {
                        if (common.myInt(dropIdentityType.SelectedValue) == 0 )
                        {
                            Alert.ShowAjaxMsg("Please! select Identity Type ..", Page);
                            return;
                        }
                    }

                    if (lblIdentityNoStar.Visible)
                    {
                        if (common.myStr(txtIdentityNumber.Text).Trim().Length == 0 || common.myStr(txtIdentityNumber.Text).Trim() == "")
                        {
                            Alert.ShowAjaxMsg("Please! enter Identity No ..", Page);
                            return;
                        }
                    }
                    
                    if (lblVIPNarrationStar.Visible)
                    {
                        if (common.myStr(txtVIPNarration.Text).Trim().Length == 0)
                        {
                            Alert.ShowAjaxMsg("Please! enter  Narration ..", Page);
                            return;
                        }
                    }
                }
                if (lblTitle.Visible)
                {
                    if (common.myInt(dropTitle.SelectedValue) == 0)
                    {
                        Alert.ShowAjaxMsg("Please Select Title !", Page);
                        return;
                    }
                }
                if (common.myStr(txtSSN.Text) != "")
                {
                    bC.SocialSecurityNumber = Parse.ParseQ(txtSSN.Text.ToString().Trim());
                }
                if (common.myStr(txtLPhoneRes.Text) != "")
                {
                    bC.Phone = Parse.ParseQ(txtLPhoneRes.Text.Trim());
                }

               
                ClinicDefaults cd = new ClinicDefaults(Page);
                if (cd.GetHospitalDefaults(DefaultNationality, Session["HospitalLocationId"].ToString()) != dropNationality.SelectedValue)
                {
                    if (common.myStr(Session["ModuleFlag"]) != "ER")
                    {
                        if (txtIdentityNumber.Text.Trim() == "" || dropIdentityType.SelectedValue.ToString() == "0")
                        {
                            Alert.ShowAjaxMsg("Please! Enter international patient identity type & no...", Page);
                             dropIdentityType.Focus();
                            return;
                        }
                    }
                }
                if (common.myStr(txtPMobile.Text) != "")
                {
                    bC.Mobile = Parse.ParseQ(txtPMobile.Text);
                }
               
                string strsaveupdate = "";
                string Parent = ddlParentof.SelectedValue + "#" + txtParentof.Text.Trim();
                if (dtpDateOfBirth.Text != "")
                {
                    datet = DateTime.ParseExact(dtpDateOfBirth.Text, "dd/MM/yyyy", null);
                }
                else
                {
                    DOBCalc();
                    datet = DateTime.ParseExact(dtpAgeDateOfBirth.Text, "dd/MM/yyyy", null);
                }
                if (Mode == "E")
                {
                    lnkOtherDetails.Visible = false;
                    lnkPayment.Visible = false;
                    lnkResponsibleParty.Visible = false;
                    if (common.myStr(Session["ModuleFlag"]) == "ER")
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                        lnkbtnAppointment.Visible = false;
                        lnkbtnBilling.Visible = false;
                    }
                    else
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                        lnkbtnAppointment.Visible = true;
                        lnkbtnBilling.Visible = false;
                    }
                    if (txtRegNo.Text != "")
                    {
                        int isERRegistration = 0;
                        if (common.myStr(Session["ModuleFlag"]) == "ER")
                        {
                            isERRegistration = 1;
                        }
                        BaseC.Patient bPatient = new BaseC.Patient(sConString);
                        StringBuilder strPname = new StringBuilder();
                        strPname.Append(common.myStr(txtFname.Text));
                        String strReg = "";
                        if (strPname.ToString() != string.Empty)
                        {
                            DateTime dateOfBirth = new DateTime();
                            string strDateOfBirth = "";
                            if (dtpDateOfBirth.Text != "")
                            {
                                dateOfBirth = DateTime.ParseExact(dtpDateOfBirth.Text, "dd/MM/yyyy", null);
                                strDateOfBirth = common.myStr(dateOfBirth.ToString("yyyy/MM/dd"));
                            }

                            strReg = bPatient.ValidateRegistrationNO(common.myInt(txtRegNo.Text), common.myInt(Session["HospitalLocationId"]),
                             common.myInt(0), common.myInt(dropTitle.SelectedValue), common.myStr(txtFname.Text.Trim()),
                             common.myStr(txtMName.Text.Trim()), common.myStr(txtLame.Text.Trim()), common.myStr(txtPMobile.Text),
                             common.myStr(txtPEmailID.Text.Trim()), strDateOfBirth.ToString(),
                             common.myInt(dropSex.SelectedValue), common.myStr(ddlParentof.SelectedValue + "#" + txtParentof.Text.Trim()));

                        }
                        if (strReg != "")
                        {
                            Alert.ShowAjaxMsg("Patient is already registered with id " + strReg + "....", Page.Page);
                            return;
                        }
                        else
                        {
                            bC = new BaseC.Patient(sConString);
                            Hashtable hsOut = bC.SaveUpdateRegistration(common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(Session["FacilityId"]),
                                    common.myInt(ViewState["pid"]),
                                    Parent,
                                    common.myInt(dropTitle.SelectedValue),
                                    common.myStr(txtFname.Text),
                                    common.myStr(txtMName.Text),
                                    common.myStr(txtLame.Text),
                                    common.myStr(txtFname.Text),
                                    0,
                                    common.myStr(txtYear.Text),
                                    common.myStr(txtMonth.Text),
                                    common.myStr(txtDays.Text),
                                    common.myInt(dropSex.SelectedValue),
                                    common.myInt(0),  //dropMarital.SelectedValue),
                                    common.myInt(0),
                                    common.myStr(txtLAddress1.Text),
                                    common.myStr(txtLAddress2.Text),
                                    common.myStr(txtLPin.Text),
                                    common.myInt(dropLCity.SelectedValue),
                                    common.myInt(dropLState.SelectedValue),
                                    common.myInt(dropLCountry.SelectedValue),
                                    common.myStr(txtPEmailID.Text),
                                    common.myStr(txtLPhoneRes.Text),
                                    common.myStr(txtPMobile.Text),
                                    common.myInt(dropNationality.SelectedValue),
                                    common.myStr(0),
                                    common.myInt(dropIdentityType.SelectedValue),
                                    common.myStr(txtIdentityNumber.Text),
                                    common.myInt(1), //iSponsorId
                                    common.myInt(1), //iPayerId
                                    common.myInt(Session["FacilityId"]), 0,
                                    common.myInt(0),
                                    common.myInt(0),
                                    common.myInt(0),
                                    common.myInt(0),
                                    common.myInt(0),
                                    common.myStr(""),
                                    common.myInt(0),
                                    common.myInt(0),
                                    common.myStr(0),
                                    common.myStr(txtSSN.Text),
                                     false, //chkExternalPatient.Checked,
                                    common.myInt(0),
                                    common.myInt(Session["UserId"]),
                                    common.myInt(txtRegNo.Text),
                                    common.myInt(txtAccountNo.Text),
                                    datet.ToString("dd/MM/yyyy"),
                                    
                                    common.myInt(Request.QueryString["AppID"]).ToString(),
                                    iArea,
                                    common.myStr(""),
                                    common.myStr(""),
                                    common.myInt(0),
                                    txtVIPNarration.Text.Trim(),
                                   "",
                                     false, //chkVIP.Checked,
                                    "",
                                    isERRegistration,
                                    common.myStr(ViewState["CheckListId"]),
                                    common.myStr(""), dtpAgeDateOfBirth.Text == "" ? true : false, common.myInt(Session["EntrySite"]),
                                    common.myInt(0),
                                    common.myInt(0),
                                    common.myStr(""),
                                    common.myInt(Request.QueryString["ResAppID"]),
                                    common.myStr(""),
                                    common.myDec(0.0)
                                  
                                    );

                        }
                        if (txtPatientImageId.Text != "")
                        {
                            SaveImage(txtRegNo.Text.ToString());
                        }
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = "Record(s) has been Updated successfully.";
                        btnSave.Text = "Save";
                        btnSave.Enabled = false;
                    }
                }
                else if (Mode == "N" || Mode == "")
                {
                    if ((Convert.ToString(txtRegNo.Text) == "0") || (Convert.ToString(txtRegNo.Text) == ""))
                    {
                        int isERRegistration = 0;
                        if (common.myStr(Session["ModuleFlag"]) == "ER")
                        {
                            isERRegistration = 1;
                        }

                        BaseC.Patient bPatient = new BaseC.Patient(sConString);
                        StringBuilder strPname = new StringBuilder();
                        strPname.Append(common.myStr(txtFname.Text));
                        String strReg = "";
                        if (strPname.ToString() != string.Empty)
                        {
                            DateTime dateOfBirth = new DateTime();
                            string strDateOfBirth = "";

                            if (dtpDateOfBirth.Text != "")
                            {
                                dateOfBirth = DateTime.ParseExact(dtpDateOfBirth.Text, "dd/MM/yyyy", null);
                                strDateOfBirth = common.myStr(dateOfBirth.ToString("yyyy/MM/dd"));
                            }

                            strReg = bPatient.ValidateRegistrationNO(Convert.ToInt32(0), common.myInt(Session["HospitalLocationId"]),
                                common.myInt(0), common.myInt(dropTitle.SelectedValue), common.myStr(txtFname.Text.Trim()),
                                common.myStr(txtMName.Text.Trim()), common.myStr(txtLame.Text.Trim()), common.myStr(txtPMobile.Text),
                                common.myStr(txtPEmailID.Text.Trim()), strDateOfBirth.ToString(),
                                common.myInt(dropSex.SelectedValue), common.myStr(ddlParentof.SelectedValue + "#" + txtParentof.Text.Trim()));

                        }
                        if (strReg != "")
                        {
                            Alert.ShowAjaxMsg("Patient is already registered with id " + strReg + "....", Page.Page);
                            return;
                        }
                        else
                        {
                            Hashtable hsOut = bC.SaveUpdateRegistration(
                                     common.myInt(Session["HospitalLocationId"]),
                                     common.myInt(Session["FacilityId"]),
                                     common.myInt(ViewState["pid"]),
                                     Parent,
                                     common.myInt(dropTitle.SelectedValue),
                                     common.myStr(txtFname.Text),
                                     common.myStr(txtMName.Text),
                                     common.myStr(txtLame.Text),
                                     common.myStr(txtFname.Text), 0,
                                     common.myStr(txtYear.Text),
                                     common.myStr(txtMonth.Text),
                                     common.myStr(txtDays.Text),
                                     common.myInt(dropSex.SelectedValue),
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myStr(txtLAddress1.Text),
                                     common.myStr(txtLAddress2.Text),
                                     common.myStr(txtLPin.Text),
                                     common.myInt(dropLCity.SelectedValue),
                                     common.myInt(dropLState.SelectedValue),
                                     common.myInt(dropLCountry.SelectedValue),
                                     common.myStr(txtPEmailID.Text),
                                     common.myStr(txtLPhoneRes.Text),
                                     common.myStr(txtPMobile.Text),
                                     common.myInt(dropNationality.SelectedValue),
                                     common.myStr(""),
                                     common.myInt(dropIdentityType.SelectedValue),
                                     common.myStr(txtIdentityNumber.Text),
                                     common.myInt(1),//iSponsorId
                                     common.myInt(1), //
                                     common.myInt(Session["FacilityId"]), 0,
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myStr(""),
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myStr(""),
                                     common.myStr(txtSSN.Text),
                                     false,
                                     common.myInt(0),
                                     common.myInt(Session["UserId"]),
                                     common.myInt(txtRegNo.Text),
                                     common.myInt(txtAccountNo.Text),
                                     datet.ToString("dd/MM/yyyy"),
                                     common.myInt(Request.QueryString["AppID"]).ToString(), iArea,
                                     common.myStr(""),
                                     common.myStr(""),
                                     common.myInt(0),
                                     txtVIPNarration.Text.Trim(),
                                     "", false, //chkVIP.Checked,
                                     "",
                                     common.myInt(isERRegistration),
                                     common.myStr(ViewState["CheckListId"]),
                                     common.myStr(""), dtpAgeDateOfBirth.Text.Trim() == "" ? true : false, common.myInt(Session["EntrySite"]),
                                     common.myInt(0),
                                     common.myInt(0),
                                     common.myStr(""),
                                     common.myInt(Request.QueryString["ResAppID"]),
                                     common.myStr(""),
                                     common.myDec(0.0),
                                      common.myInt(hdnOTBookingId.Value)
                                     );// Done by ujjwal 08April2015 to register unregisteredresource appointment patients


                            txtRegNo.Text = hsOut["@intRegistrationId"].ToString();
                            txtAccountNo.Text = hsOut["@intRegistrationNo"].ToString();
                            PID = Convert.ToInt32(txtRegNo.Text == "" ? "0" : txtRegNo.Text);
                            txt_hdn_PName.Text = txtFname.Text.Trim() + " " + txtMName.Text.Trim() + " " + txtLame.Text.Trim();
                            ViewState["UHID"] = PID;
                            ViewState["PName"] = txt_hdn_PName.Text;
                            if (txtPatientImageId.Text != "")
                            {
                                SaveImage(strsaveupdate.ToString());
                            }
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                            lblMessage.Text = "Record(s) has been Saved successfully.";
                            btnSave.Text = "Save";
                            btnSave.Enabled = false;
                            lnkOtherDetails.Visible = false;
                            lnkPayment.Visible = false;
                            lnkResponsibleParty.Visible = false;
                            if (common.myStr(Session["ModuleFlag"]) == "ER")
                            {
                                lnkPatientRelation.Visible = false;
                                lnkAttachDocument.Visible = false;
                                LblLabelname.Visible = false;
                                lnkbtnAppointment.Visible = false;
                                lnkbtnBilling.Visible = false;
                            }
                            else
                            {
                                lnkPatientRelation.Visible = false;
                                lnkAttachDocument.Visible = false;
                                LblLabelname.Visible = false;
                                lnkbtnAppointment.Visible = true;
                                lnkbtnBilling.Visible = false;
                            }
                        }
                        if (common.myStr(Session["ModuleFlag"]) != "ER")
                        {
                            if (common.myStr(Session["Source"]) == "Appointment")
                            {
                                dvRedirect.Visible = false;
                                Session["Source"] = string.Empty;
                            }
                            else
                            {
                                dvRedirect.Visible = true;
                                Session["Source"] = string.Empty;
                            }
                        }
                        if (Convert.ToString(Request.QueryString["mode"]) == "N")
                        {
                            dvRedirect.Visible = false;
                            Session["Source"] = string.Empty;
                        }
                    }
                }
                HttpCookie htc = new HttpCookie("PatientImage");
                htc.Value = "";
                htc.Expires = DateTime.Now.AddHours(-1);
                Response.Cookies.Add(htc);
                
                PatientInfo();
                
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void lnkAppointment_Click(object sender, EventArgs e)
    {
        int i = 0;
        DataSet ds1 = (DataSet)Session["ModuleData"];
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (common.myStr(dr["ModuleName"]) == "Appointment")
            {
                Session["ModuleId"] = common.myInt(dr["ModuleId"]);
            }
            i++;
        }
        int ProviderId = common.myInt(0);
        Session["RedirectedPage"] = "Yes";
        Session["RegIdFromRegistration"] = txtAccountNo.Text;
        Session["SpecializationId"] = common.myStr(0);
        Session["ProviderId"] = ProviderId;
        Response.Redirect("/Appointment/AppScheduler_New.aspx", true);
    }
    protected void fromPopUp(object sender, EventArgs e)
    {
        lblMessage.Text = "From child";
    }
    protected void lnkOpBilling_Click(object sender, EventArgs e)
    {
        int i = 0;
        DataSet ds1 = (DataSet)Session["ModuleData"];
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (common.myStr(dr["ModuleName"]) == "Billing")
            {
                Session["ModuleId"] = i;
            }
            i++;
        }
        Session["RedirectedPage"] = "Yes";
        Response.Redirect("~/EMRBILLING/OPBill.aspx?VisitType=R&RegNo=" + common.myStr(txtAccountNo.Text) + "&IsAppoint=R&AppointmentId=0");
    }
    protected void lnkPatientRelation_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C91&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C91&");
            }
        }
        else
        {
            Response.Redirect("/PRegistration/PatientContacts.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C91&");
        }
    }
    protected void lnkbtnAppointment_OnClick(Object sender, EventArgs e)
    {
        int i = 0;
        DataSet ds1 = (DataSet)Session["ModuleData"];
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (common.myStr(dr["ModuleName"]) == "Appointment")
            {
                Session["ModuleId"] = i;
            }
            i++;
        }
        int ProviderId = common.myInt(0);
        Session["RedirectedPage"] = "Yes";
        Session["RegIdFromRegistration"] = txtAccountNo.Text;
        Session["SpecializationId"] = common.myStr(0);
        Session["ProviderId"] = ProviderId;
        Response.Redirect("/Appointment/AppScheduler.aspx", true);
    }
    protected void lnkbtnBilling_OnClick(Object sender, EventArgs e)
    {
        int i = 0;
        DataSet ds1 = (DataSet)Session["ModuleData"];
        foreach (DataRow dr in ds1.Tables[0].Rows)
        {
            if (common.myStr(dr["ModuleName"]) == "Billing")
            {
                Session["ModuleId"] = i;
            }
            i++;
        }
        Session["RedirectedPage"] = "Yes";
        Response.Redirect("/EMRBILLING/OPBill.aspx?VisitType=R&RegNo=" + common.myStr(txtAccountNo.Text) + "&IsAppoint=R&AppointmentId=0", true);
    }

    protected void lnkOtherDetails_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C89&Category=PopUp", false);
            }
            else
            {
                Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C89", false);
            }
        }
        else
        {
            Response.Redirect("/PRegistration/PatientOtherDetails.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C89", false);
        }
    }

    protected void lnkPayment_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C90&Category=PopUp");
            }
            else
            {
                Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C90");
            }
        }
        else
        {
            Response.Redirect("/PRegistration/PaymentDetails.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C90");
        }
    }

    protected void lnkResponsibleParty_OnClick(Object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C94&Category=PopUp");
            }
            else
            {
                Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C94");
            }
        }
        else
        {
            Response.Redirect("/PRegistration/ResponsibleParty.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C94");
        }
    }

    protected void lnkAttachDocument_OnClick(object sender, EventArgs e)
    {
        if (Request.QueryString["Category"] != null)
        {
            if (Request.QueryString["Category"].ToString() == "PopUp")
            {
                //Response.Redirect("/emr/AttachDocument.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119&Category=PopUp");
                Response.Redirect("/emr/AttachDocumentFTP.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119&Category=PopUp");
            }
            else
            {
                //Response.Redirect("/emr/AttachDocument.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119");
                Response.Redirect("/emr/AttachDocumentFTP.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119");
            }
        }
        else
        {
            //Response.Redirect("/emr/AttachDocument.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119");
            Response.Redirect("/emr/AttachDocumentFTP.aspx?RNo=" + txtAccountNo.Text.ToString().Trim() + "&Mpg=C119");
        }
    }

    protected void btnSetImage_Click(object sender, EventArgs e)
    {
    }
    protected void LocalCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dropLCountry.SelectedValue != "" || hdnCountry.Value != "")
            {
                BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
                DataSet ds = new DataSet();
                if (Cache["StateTable"] == null)
                    ds = objCM.GetPatientState(common.myInt(dropLCountry.SelectedValue));
                else
                    ds = (DataSet)Cache["StateTable"];

                DataView dv = (DataView)ds.Tables[0].DefaultView;
                dv.RowFilter = "CountryId = " + common.myInt(dropLCountry.SelectedValue) + "";
                dropLState.Items.Clear();
                dropLState.AppendDataBoundItems = true;
                ListItem rdc = new ListItem("Select", "0");
                dropLState.Items.Insert(0, rdc);
                dropLState.DataSource = dv;
                dropLState.DataTextField = "StateName";
                dropLState.DataValueField = "StateID";
                dropLState.DataBind();
                dropLState.SelectedIndex = 0;
                LocalState_SelectedIndexChanged(sender, e);
                dropLState.Focus();

                dropLCity.Items.Clear();
                dropLCity.Items.Insert(0, rdc);
                ddlCityArea.Items.Clear();
                ddlCityArea.Items.Insert(0, rdc);
                txtLPin.Text = "";
            }
            else
            {
                dropLState.Items.Clear();
                dropLCity.Items.Clear();
                ddlCityArea.Items.Clear();
                
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void LocalState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
            dropLCity.Items.Clear();
            DataSet ds = new DataSet();
            if (Cache["CityTable"] == null)
                ds = objCM.GetPatientCity(common.myInt(dropLState.SelectedValue));
            else
                ds = (DataSet)Cache["CityTable"];

            if (dropLState.SelectedValue != "" || hdnState.Value != "")
            {
                DataView dv = (DataView)ds.Tables[0].DefaultView;
                dv.RowFilter = "StateId = " + common.myInt(dropLState.SelectedValue) + "";
                dropLCity.Items.Clear();
                dropLCity.DataSource = dv.ToTable();
                dropLCity.AppendDataBoundItems = true;
                ListItem rci = new ListItem("Select");
                dropLCity.Items.Insert(0, rci);
                dropLCity.DataTextField = "CityName";
                dropLCity.DataValueField = "CityID";
                dropLCity.DataBind();
                LocalCity_OnSelectedIndexChanged(sender, e);
                dropLCity.Focus();


                ddlCityArea.Items.Clear();
                ddlCityArea.Items.Insert(0, rci);
                txtLPin.Text = "";
            }
            else
            {
                dropLCity.Items.Clear();
                ddlCityArea.Items.Clear();
                txtLPin.Text = "";
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void LocalCity_OnSelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {

            BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
            ddlCityArea.Items.Clear();
            DataSet ds = new DataSet();
            if (dropLCity.SelectedValue != "Select" || hdnCity.Value != "")
            {
                ds = objCM.GetPatientCityArea(common.myInt(dropLCity.SelectedValue));
                ddlCityArea.DataSource = ds;
                ddlCityArea.AppendDataBoundItems = true;
                ListItem rdc = new ListItem("Select");
                ddlCityArea.Items.Insert(0, rdc);
                ddlCityArea.DataSource = ds;
                ddlCityArea.DataTextField = "AreaName";
                ddlCityArea.DataValueField = "AreaId";
                ddlCityArea.DataBind();
                ddlCityArea.Focus();
            }
            else
            {
                ddlCityArea.Items.Clear();
                txtLPin.Text = "";
            }

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlCityArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        ds = objCM.GetPatientZipcode(common.myInt(ddlCityArea.SelectedValue));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtLPin.Text = ds.Tables[0].Rows[0]["ZipCode"].ToString();
        }
    }
    protected void RefType_SelectedIndexChanging(object sender, EventArgs e)
    {
        try
        {
            //if (dropReferredType.SelectedValue.ToString() == "S")
            //{
            //    lblReferredByStar.Visible = false;
            //}
            //else
            //{
            //    lblReferredByStar.Visible = true;
            //}
            //if (common.myStr(dropReferredType.SelectedValue) == "D" || common.myStr(dropReferredType.SelectedValue) == "C")
            //{
            //    lnkAddName.Visible = true;
            //}
            //else
            //{
            //    lnkAddName.Visible = false;
            //}
            //  fillReferalBy();

        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public void ClearApplicationCache()
    {
        List<string> keys = new List<string>();
        // retrieve application Cache enumerator
        IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
        // copy all keys that currently exist in Cache
        while (enumerator.MoveNext())
        {
            keys.Add(enumerator.Key.ToString());
        }
        // delete every key from cache
        for (int i = 0; i < keys.Count; i++)
        {
            HttpRuntime.Cache.Remove(keys[i]);
        }
    }
    private void PopulateControl()
    {
        try
        {
            BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
            //populate title drop down control
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            BaseC.Security objSecurity = new BaseC.Security(sConString);
            if (Cache["TitleTable"] == null)
            {
                DataSet objDs = (DataSet)objBc.GetPatientTitle();
                Cache.Insert("TitleTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                foreach (DataRow dsTitle in objDs.Tables[0].Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)dsTitle["TextField"];
                    item.Value = dsTitle["Id"].ToString();
                    item.Attributes.Add("Gender", common.myStr(dsTitle["Gender"]));
                    dropTitle.Items.Add(item);
                }
                objDs.Tables[0].DefaultView.RowFilter = "IsNewBornTitle=1";
                if (objDs.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                {
                    ViewState["IsnewBornTitle"] = common.myStr(objDs.Tables[0].DefaultView.ToTable().Rows[0]["id"]);
                }
            }
            else
            {
                DataSet objDs = (DataSet)Cache["TitleTable"];
                foreach (DataRow dsTitle in objDs.Tables[0].Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)dsTitle["TextField"];
                    item.Value = dsTitle["Id"].ToString();
                    item.Attributes.Add("Gender", common.myStr(dsTitle["Gender"]));
                    dropTitle.Items.Add(item);
                }
                objDs.Tables[0].DefaultView.RowFilter = "IsNewBornTitle=1";
                if (objDs.Tables[0].DefaultView.ToTable().Rows.Count > 0)
                {
                    ViewState["IsnewBornTitle"] = common.myStr(objDs.Tables[0].DefaultView.ToTable().Rows[0]["id"]);
                }
            }
            //populate Local City drop down control
            if (Cache["CityTable"] != null)
            {
                DataSet objDs = (DataSet)objBc.GetPatientCity();
                Cache["CityTable"] = objDs;
                dropLCity.DataSource = (DataSet)Cache["CityTable"];
                dropLCity.DataTextField = "CityName";
                dropLCity.DataValueField = "CityId";
                dropLCity.DataBind();
                ListItem rci = new ListItem("[Select]");
                dropLCity.Items.Insert(0, rci);
            }
            else
            {
                DataSet objDs = (DataSet)Cache["CityTable"];
                dropLCity.DataSource = objDs;
                dropLCity.DataTextField = "CityName";
                dropLCity.DataValueField = "CityId";
                dropLCity.DataBind();
                ListItem rci = new ListItem("[Select]");
                dropLCity.Items.Insert(0, rci);
            }
            //populate Local State drop down control
            if (Cache["StateTable"] == null)
            {
                //BaseC.Patient objBc = new BaseC.Patient(sConString);
                DataSet objDs = (DataSet)objBc.GetPatientState();
                Cache["StateTable"] = objDs;
                dropLState.DataSource = (DataSet)Cache["StateTable"];
                dropLState.DataTextField = "StateName";
                dropLState.DataValueField = "StateID";
                dropLState.DataBind();
                ListItem rci = new ListItem("[Select]");
                dropLState.Items.Insert(0, rci);
            }
            else
            {
                DataSet objDs = (DataSet)Cache["StateTable"];
                dropLState.DataSource = objDs;
                dropLState.DataTextField = "StateName";
                dropLState.DataValueField = "StateID";
                dropLState.DataBind();
                ListItem rci = new ListItem("[Select]");
                dropLState.Items.Insert(0, rci);
            }
            //populate Local Country drop down control
            if (Cache["CountryTable"] == null)
            {
                DataSet objDs = (DataSet)objBc.GetPatientCountry();
                Cache["CountryTable"] = objDs;
                dropLCountry.DataSource = (DataSet)Cache["CountryTable"];
                dropLCountry.DataTextField = "CountryName";
                dropLCountry.DataValueField = "CountryId";
                dropLCountry.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["CountryTable"];
                dropLCountry.DataSource = objDs;
                dropLCountry.DataTextField = "CountryName";
                dropLCountry.DataValueField = "CountryId";
                dropLCountry.DataBind();
            }
            LocalCountry_SelectedIndexChanged(this, null);
            //populate Nationality drop down control
            if (Cache["NationalityTable"] == null)
            {
                //BaseC.Patient objBc = new BaseC.Patient(sConString);
                DataSet objDs = (DataSet)objBc.GetPatientNationality();
                Cache.Insert("NationalityTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                dropNationality.DataSource = (DataSet)Cache["NationalityTable"];
                dropNationality.DataTextField = "name";
                dropNationality.DataValueField = "ID";
                dropNationality.DataBind();
                ListItem rci = new ListItem("Select");
                dropNationality.Items.Insert(0, rci);
            }
            else
            {
                DataSet objDs = (DataSet)Cache["NationalityTable"];
                dropNationality.DataSource = objDs;
                dropNationality.DataTextField = "name";
                dropNationality.DataValueField = "ID";
                dropNationality.DataBind();
                ListItem rci = new ListItem("Select");
                dropNationality.Items.Insert(0, rci);
            }
            if (Convert.ToInt16(Session["FacilityId"]) != null)
            {
                //DataTable dt = (DataTable)objBc.GetHealthCareMaster(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt16(Session["FacilityId"]), 1, 0);
                //ddlHealthCareFacilitator.DataSource = dt;
                //ddlHealthCareFacilitator.DataTextField = "FacilatorName";
                //ddlHealthCareFacilitator.DataValueField = "HCFID";
                //ddlHealthCareFacilitator.DataBind();
                //ListItem rci = new ListItem("Select");
                //ddlHealthCareFacilitator.Items.Insert(0, rci);
            }
            //populate religion drop down control
            if (Cache["ReligionTable"] == null)
            {
                //BaseC.Patient objBc = new BaseC.Patient(sConString);
                DataSet objDs = (DataSet)objBc.GetPatientReligion();
                Cache.Insert("ReligionTable", objDs, null, DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration);
                //dropReligion.DataSource = (DataSet)Cache["ReligionTable"];
                //dropReligion.DataTextField = "Name";
                //dropReligion.DataValueField = "ID";
                //dropReligion.DataBind();
            }
            else
            {
                DataSet objDs = (DataSet)Cache["ReligionTable"];
                //dropReligion.DataSource = objDs;
                //dropReligion.DataTextField = "Name";
                //dropReligion.DataValueField = "ID";
                //dropReligion.DataBind();
            }
            if (Cache["ReferredTypeTable"] == null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                DataSet dsReferal = dl.FillDataSet(CommandType.StoredProcedure, "uspGetReferby");
                foreach (DataRowView drReferal in dsReferal.Tables[0].DefaultView)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)drReferal["ReferralTypeName"];
                    item.Value = drReferal["ReferralType"].ToString();
                    item.Attributes.Add("Id", common.myStr(drReferal["Id"]));
                    // dropReferredType.Items.Add(item);
                }
            }
            else
            {
                DataSet dsReferal = (DataSet)Cache["ReferredTypeTable"];
                foreach (DataRowView drReferal in dsReferal.Tables[0].DefaultView)
                {
                    ListItem item = new ListItem();
                    item.Text = (string)drReferal["ReferralTypeName"];
                    item.Value = drReferal["ReferralType"].ToString();
                    item.Attributes.Add("Id", common.myStr(drReferal["Id"]));
                    //dropReferredType.Items.Add(item);
                }
            }
            RefType_SelectedIndexChanging(this, null);
            DataSet ds;
            ds = null;
            BaseC.EMRVitals objEMRVitals = new BaseC.EMRVitals(sConString);
            BaseC.Appointment objAppointment = new BaseC.Appointment(sConString);
            ds = objAppointment.GetDoctorListForAppointment(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]), DateTime.Now);
            foreach (DataRowView drProvider in ds.Tables[0].DefaultView)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = (string)drProvider["DoctorName"];
                item.Value = drProvider["DoctorID"].ToString();
                item.Attributes.Add("Id", common.myStr(drProvider["SpecialisationId"]));
                // ddlRenderingProvider.Items.Add(item);
                item.DataBind();
            }
            // ddlRenderingProvider.Items.Insert(0, new RadComboBoxItem("Select", "0"));
            ds = null;
            ds = (DataSet)objCM.GetPatientIdentityType();
            dropIdentityType.DataSource = ds;
            dropIdentityType.DataValueField = "ID";
            dropIdentityType.DataTextField = "Description";
            dropIdentityType.DataBind();
            dropIdentityType.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    Double val(String value)
    {
        Double intData = 0;
        Boolean blnTemp = Double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture.NumberFormat, out intData);
        return intData;
    }
    //void BindLeadSource()
    //{
    //    BaseC.Patient objbc = new BaseC.Patient(sConString);
    //    DataSet ds = new DataSet();
    //    ddlLeadSource.Items.Clear();
    //    ds = objbc.GetLeadSource(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0);
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        ddlLeadSource.DataSource = ds;
    //        ddlLeadSource.DataTextField = "Name";
    //        ddlLeadSource.DataValueField = "Id";
    //        ddlLeadSource.DataBind();
    //    }
    //    ddlLeadSource.Items.Insert(0, new ListItem("Select", "0"));
    //    ddlLeadSource.Items[0].Value = "0";
    //}
    protected void btnCalAge_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            BaseC.ParseData bc = new BaseC.ParseData();
            Hashtable hshIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            String SqlStr = "select dbo.DOBFromAge(" + val(bc.ParseQ(txtYear.Text)) + "," + val(bc.ParseQ(txtMonth.Text)) + "," + val(bc.ParseQ(txtDays.Text)) + ")";
            SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, SqlStr, hshIn);
            dr.Read();
            string a = dr[0].ToString();
            dtpDateOfBirth.Text = Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy");
            txtLAddress1.Focus();
            dr.Close();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void ClearFields()
    {
        dropTitle.SelectedIndex = 0;
        txtFname.Text = "";
        txtMName.Text = "";
        txtLame.Text = "";
        txtPreviousName.Text = "";
       // dropMarital.SelectedIndex = 0;
        dropSex.SelectedIndex = 0;
        dtpDateOfBirth.Text = "";
        txtYear.Text = "";
        txtMonth.Text = "";
        txtDays.Text = "";
        // ddlFacility.SelectedIndex = 0;
        dropIdentityType.SelectedIndex = 0;
        txtIdentityNumber.Text = "";
        txtLAddress1.Text = "";
        dropLCountry.SelectedIndex = 0;
        //txtEmpName.Text = "";
        // ddlKinRelation.SelectedIndex = 0;
        // txtEmpNo.Text = "";
        txtLPin.Text = "";
        txtLPhoneRes.Text = "";
        txtPMobile.Text = "";
        txtPEmailID.Text = "";
        dropNationality.SelectedIndex = 0;
        //dropReligion.SelectedIndex = 0;
        // dropReferredType.SelectedIndex = 0;
        // dropRefBy.Items.Clear();
        // ddlRenderingProvider.SelectedIndex = 0;
        hdnRegistrationNo.Value = "";
        txtSSN.Text = "";
        txtAccountNo.Text = "";
        PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
        isreadonly.SetValue(this.Request.QueryString, false, null);
        this.Request.QueryString.Clear();
    }
    protected void Upload_OnClick(Object sender, EventArgs e)
    {
        try
        {
            string sFileName = "";
            string sFileExtension = "";
            string sSavePath = "/PatientDocuments/PatientImages/";
            StringBuilder objStr = new StringBuilder();
            if (FileUploader1.FileName != "" || FileUploader1.PostedFile != null)
            {
                DeleteFiles();
                HttpPostedFile myFile = FileUploader1.PostedFile;
                int nFileLen = myFile.ContentLength;
                if (nFileLen == 0)
                {
                    txtPatientImageId.Text = "";
                    lblMessage.Text = "Error: The file size is zero.";
                    return;
                }
                byte[] myData = new Byte[nFileLen];
                myFile.InputStream.Read(myData, 0, nFileLen);
                sFileExtension = "";
                sFileExtension = System.IO.Path.GetExtension(myFile.FileName).ToLower();
                sFileName = txtFname.Text.ToString() + txtLame.Text.ToString() + myFile.FileName.ToLower();
                System.IO.FileStream newFile = new System.IO.FileStream(Server.MapPath(sSavePath + sFileName), System.IO.FileMode.Create);
                newFile.Write(myData, 0, myData.Length);
                newFile.Close();
                txtPatientImageId.Text = sFileName;
                PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + sFileName;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void ShowImage()
    {
        try
        {
            DeleteFiles();
            StringBuilder strSQL = new StringBuilder();
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmdTemp;
            SqlParameter prm2;
            strSQL.Append("SELECT PatientImage, ImageType from RegistrationImage WITH (NOLOCK) where RegistrationId=@RegistrationNo and Active=1");
            cmdTemp = new SqlCommand(strSQL.ToString(), con);
            cmdTemp.CommandType = CommandType.Text;
            prm2 = new SqlParameter();
            prm2.ParameterName = "@RegistrationNo";
            prm2.Value = Parse.ParseQ(txtRegNo.Text.ToString().Trim());
            prm2.SqlDbType = SqlDbType.Int;
            prm2.Direction = ParameterDirection.Input;
            cmdTemp.Parameters.Add(prm2);
            con.Open();
            SqlDataReader dr = cmdTemp.ExecuteReader();
            if (dr.HasRows == true)
            {
                dr.Read();
                Stream strm;
                Object img = dr["PatientImage"];
                String FileName = dr["ImageType"].ToString();
                if (FileName != "")
                {
                    strm = new MemoryStream((byte[])img);
                    byte[] buffer = new byte[strm.Length];
                    int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
                    FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/PatientImages/" + FileName), FileMode.Create, FileAccess.Write);
                    fs.Write(buffer, 0, byteSeq);
                    fs.Dispose();
                    PatientImage.ImageUrl = "/PatientDocuments/PatientImages/" + FileName;
                }
            }
            else
            {
                PatientImage.ImageUrl = "~/Images/patientLeft.jpg";
            }
            dr.Close();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void DeleteFiles()
    {
        try
        {
            string strPatientImageid = "";
            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/PatientImages"));
            if (objDir.Exists == true)
            {
                FileInfo[] fi_array = objDir.GetFiles();
                foreach (FileInfo files in fi_array)
                {
                    if (files.Exists)
                    {
                        strPatientImageid = Path.GetFileNameWithoutExtension(Server.MapPath("/PatientDocuments/PatientImages/") + files);
                        if (Convert.ToString(Session["RegistrationId"]) == strPatientImageid)
                        {
                            File.Delete(Server.MapPath("/PatientDocuments/PatientImages/") + files);
                        }
                    }
                }
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
            }
            else
            {
                objDir.Create();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void SaveImage(string strRegistrationId)
    {
        try
        {
            StringBuilder strSQL = new StringBuilder();
            byte[] byteImageData;
            String FileName = "";
            string sFileExtension = "";
            string sNewFileName = "";
            if (txtPatientImageId.Text != "")
            {
                DirectoryInfo dir = new DirectoryInfo(Server.MapPath("/PatientDocuments/PatientImages/"));
                FileInfo[] fi_array = dir.GetFiles();
                foreach (FileInfo file in fi_array)
                {
                    FileName = file.ToString();
                    sFileExtension = System.IO.Path.GetExtension(FileName).ToLower();
                    if (FileName.ToString() == txtPatientImageId.Text.ToString())
                    {
                        File.Delete(Server.MapPath("/PatientDocuments/PatientImages/") + strRegistrationId == "0" || strRegistrationId == "" ? common.myInt(txtRegNo.Text.ToString().Trim()) + sFileExtension.ToLower() : strRegistrationId + sFileExtension.ToLower());
                        //File.Move(Server.MapPath("/PatientDocuments/PatientImages/" + txtPatientImageId.Text), Server.MapPath("/PatientDocuments/PatientImages/" + strRegistrationId + sFileExtension.ToLower()));
                        sNewFileName = strRegistrationId == "0" || strRegistrationId == "" ? common.myInt(txtRegNo.Text.ToString().Trim()) + sFileExtension.ToLower() : strRegistrationId + sFileExtension.ToLower();
                        break;
                    }
                }
                String FilePath = Server.MapPath("/PatientDocuments/PatientImages/") + txtPatientImageId.Text;
                FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] image = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
                byteImageData = image;
                SqlConnection con = new SqlConnection(sConString);
                SqlCommand cmdTemp;
                SqlParameter prm1, prm2, prm3, prm4, prm5;

                strSQL.Append("Exec UspSavePatientImage @intRegistrationId, @intRegistrationNo,@inyHospitalLocationId,@Image,@chvImageType");
                cmdTemp = new SqlCommand(strSQL.ToString(), con);
                cmdTemp.CommandType = CommandType.Text;

                prm1 = new SqlParameter();
                prm1.ParameterName = "@intRegistrationNo";
                prm1.Value = common.myInt(txtAccountNo.Text.ToString().Trim());
                prm1.SqlDbType = SqlDbType.Int;
                prm1.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm1);

                prm2 = new SqlParameter();
                prm2.ParameterName = "@intRegistrationId";
                prm2.Value = common.myInt(txtRegNo.Text.ToString().Trim());
                prm2.SqlDbType = SqlDbType.Int;
                prm2.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm2);

                prm3 = new SqlParameter();
                prm3.ParameterName = "@inyHospitalLocationId";
                prm3.Value = Convert.ToInt16(Session["HospitalLocationID"].ToString());
                prm3.SqlDbType = SqlDbType.TinyInt;
                prm3.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm3);

                prm4 = new SqlParameter();
                prm4.ParameterName = "@Image";
                prm4.Value = byteImageData;
                prm4.SqlDbType = SqlDbType.Image;
                prm4.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm4);

                prm5 = new SqlParameter();
                prm5.ParameterName = "@chvImageType";
                prm5.Value = sNewFileName;
                prm5.SqlDbType = SqlDbType.VarChar;
                prm5.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm5);
                con.Open();
                cmdTemp.ExecuteNonQuery();

                //PatientImage.ImageUrl = "/Images/patientLeft.jpg";
                txtPatientImageId.Text = "";
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void RemoveImage_OnClick(Object sender, EventArgs e)
    {
        try
        {
            if ((txtRegNo.Text == "0") || (txtRegNo.Text == ""))
            {
                DeleteFiles();
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
            }
            else
            {
                hshTable = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hshTable.Add("@RegNo", Parse.ParseQ(txtRegNo.Text.ToString().Trim()));
                hshTable.Add("@HospID", Session["HospitalLocationID"].ToString());
                String strSQL = "Update registrationImage set Active=0 where RegistrationId=@RegNo and Active=1";
                objDl.ExecuteNonQuery(CommandType.Text, strSQL, hshTable);
                //Alert.ShowAjaxMsg("Image Removed", Page);
                PatientImage.ImageUrl = "/Images/patientLeft.jpg";
                txtPatientImageId.Text = "";
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void populateDetailsFromDocAppointment()
    {
        try
        {
            BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
            DataSet objDs = (DataSet)objwcf.populateDetailsFromDocAppointment(common.myInt(Parse.ParseQ(common.myInt(Request.QueryString["AppID"]).ToString())), common.myInt(Session["HospitalLocationId"]));

            if (objDs.Tables[0].Rows.Count > 0)
            {
                String StrFullName = "";
                StrFullName = objDs.Tables[0].Rows[0]["PatientName"].ToString().Trim();
                if (StrFullName.Trim().Length > 0)
                {
                    if (StrFullName.IndexOf(" ") > 0)
                    {
                        txtFname.Text = StrFullName.Substring(1, StrFullName.IndexOf(" "));
                        StrFullName = StrFullName.Substring(txtFname.Text.Trim().Length, (StrFullName.Length - txtFname.Text.Trim().Length));
                    }
                    else
                    {
                        txtFname.Text = StrFullName.Trim();
                        StrFullName = "";
                    }
                }
                if (StrFullName.Trim().Length > 0)
                {
                    if (StrFullName.IndexOf(" ") > 0)
                    {
                        txtMName.Text = StrFullName.Substring(1, StrFullName.IndexOf(" "));
                        StrFullName = StrFullName.Substring(txtMName.Text.Trim().Length, (StrFullName.Length - txtMName.Text.Trim().Length));
                    }
                    else
                    {
                        txtFname.Text = StrFullName.Trim();
                        StrFullName = "";
                    }
                }
                if (StrFullName.Trim().Length > 0)
                {
                    if (StrFullName.IndexOf(" ") > 0)
                    {
                        txtLame.Text = StrFullName.Substring(1, StrFullName.IndexOf(" "));
                        StrFullName = StrFullName.Substring(txtLame.Text.Trim().Length, (StrFullName.Length - txtLame.Text.Trim().Length));
                    }
                    else
                    {
                        txtFname.Text = StrFullName.Trim();
                        StrFullName = "";
                    }
                }
                if (objDs.Tables[0].Rows[0]["DateofBirth"].ToString().Trim() == "01-01-1900")
                {
                    dtpDateOfBirth.Text = Convert.ToString(objDs.Tables[0].Rows[0]["DateofBirth"]);
                }
                txtYear.Text = objDs.Tables[0].Rows[0]["AgeYears"].ToString();
                txtMonth.Text = objDs.Tables[0].Rows[0]["AgeMonths"].ToString();
                txtDays.Text = objDs.Tables[0].Rows[0]["AgeDays"].ToString();
                dropSex.SelectedValue = Convert.ToString(objDs.Tables[0].Rows[0]["Gender"]);
                // ddlFacility.SelectedValue = objDs.Tables[0].Rows[0]["FacilityID"].ToString();
                dropIdentityType.SelectedValue = Convert.ToString(objDs.Tables[0].Rows[0]["OtherIdentityType"]);
                txtIdentityNumber.Text = objDs.Tables[0].Rows[0]["OtherIdentityNo"].ToString();
                txtLPhoneRes.Text = objDs.Tables[0].Rows[0]["ResidencePhone"].ToString();
                txtPMobile.Text = objDs.Tables[0].Rows[0]["Mobile"].ToString();
                txtPEmailID.Text = objDs.Tables[0].Rows[0]["Email"].ToString();
                txtSSN.Text = objDs.Tables[0].Rows[0]["SocialSecurityNo"].ToString();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ibtnUpload_Click(object sender, EventArgs e)
    {
    }
    protected void btnGetRegNoInfo_Click(object sender, EventArgs e)
    {
    }
    protected void ibtnPharmacy_Click(object sender, ImageClickEventArgs e)
    {
        //RadWindowForNew.NavigateUrl = "../../MPages/OrgContacts.aspx?Category=Pharmacy";
        //RadWindowForNew.Height = 500;
        //RadWindowForNew.Width = 550;
        //RadWindowForNew.Top = 40;
        //RadWindowForNew.Left = 100;
        //RadWindowForNew.OnClientClose = "BindPharmacy";
        //RadWindowForNew.Modal = true;
        //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        //RadWindowForNew.VisibleStatusbar = false;
        //return;
    }
    //protected void ibtnRImagecenter_Click(object sender, ImageClickEventArgs e)
    //{
    //    RadWindowForNew.NavigateUrl = "../../MPages/OrgContacts.aspx?Category=Imagecenter";
    //    RadWindowForNew.Height = 500;
    //    RadWindowForNew.Width = 550;
    //    RadWindowForNew.Top = 40;
    //    RadWindowForNew.Left = 100;
    //    RadWindowForNew.OnClientClose = "BindPharmacy";
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //    RadWindowForNew.VisibleStatusbar = false;
    //    return;
    //}

    //protected void ibtnDefaultLab_Click(object sender, ImageClickEventArgs e)
    //{
    //    RadWindowForNew.NavigateUrl = "../../MPages/OrgContacts.aspx?Category=Lab";
    //    RadWindowForNew.Height = 500;
    //    RadWindowForNew.Width = 550;
    //    RadWindowForNew.Top = 40;
    //    RadWindowForNew.Left = 100;
    //    RadWindowForNew.OnClientClose = "BindPharmacy";
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //    RadWindowForNew.VisibleStatusbar = false;
    //    return;
    //}

    //protected void ibtnreftype_Click(object sender, ImageClickEventArgs e)
    //{
    //    RadWindowForNew.NavigateUrl = "../../MPages/OrgContacts.aspx?Category=Organization";
    //    RadWindowForNew.Height = 500;
    //    RadWindowForNew.Width = 550;
    //    RadWindowForNew.Top = 40;
    //    RadWindowForNew.Left = 100;
    //    RadWindowForNew.OnClientClose = "BindPharmacy";
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //    RadWindowForNew.VisibleStatusbar = false;
    //    return;
    //}

    //protected void ibtnrefby_Click(object sender, ImageClickEventArgs e)
    //{
    //    RadWindowForNew.NavigateUrl = "../../MPages/OrgContacts.aspx?Category=Provider";
    //    RadWindowForNew.Height = 500;
    //    RadWindowForNew.Width = 550;
    //    RadWindowForNew.Top = 40;
    //    RadWindowForNew.Left = 100;
    //    RadWindowForNew.OnClientClose = "BindPharmacy";
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //    RadWindowForNew.VisibleStatusbar = false;
    //    return;
    //}

    //protected void ibtnpcp_Click(object sender, ImageClickEventArgs e)
    //{
    //    RadWindowForNew.NavigateUrl = "../../MPages/OrgContacts.aspx?Category=Provider";
    //    RadWindowForNew.Height = 500;
    //    RadWindowForNew.Width = 700;
    //    RadWindowForNew.Top = 40;
    //    RadWindowForNew.Left = 100;
    //    RadWindowForNew.OnClientClose = "BindPharmacy";
    //    RadWindowForNew.Modal = true;
    //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
    //    RadWindowForNew.VisibleStatusbar = false;
    //    return;
    //}
    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (txtRegNo.Text != "")
        {
            RadWindowForNew.NavigateUrl = "~/EMRReports/RegistrationPatientDetails.aspx?RegistrationId=" + txtRegNo.Text.ToString().Trim() + "&Pid=Demographic";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select Patient !", Page);
        }
    }
    protected void btnprintLabel_Click(object sender, EventArgs e)
    {
        if (txtRegNo.Text != "")
        {
            //RadWindowForNew.NavigateUrl = "~/EMRReports/RegistrationPatientDetails.aspx?RegistrationId=" + txtRegNo.Text.ToString().Trim() + "&Pid=DemographicLabel";
            RadWindowForNew.NavigateUrl = "~/EMRReports/PrintReport.aspx?ReportName=RegCard&Vlue=" + txtRegNo.Text.ToString().Trim() + "&Pid=DemographicLabel";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select Patient !", Page);
        }
    }

    #region Popup Windows

    protected void ibtnIdentityType_Click(object sender, ImageClickEventArgs e)
    {
        //RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=Iden";
        //RadWindowForNew.Height = 550;
        //RadWindowForNew.Width = 900;
        //RadWindowForNew.Top = 10;
        //RadWindowForNew.Left = 10;
        //ViewState["Tag"] = "Iden";

        //RadWindowForNew.OnClientClose = "BindCombo";
        //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindowForNew.Modal = true;
        //RadWindowForNew.VisibleStatusbar = false;
    }

    protected void ibtnCountry_Click(object sender, ImageClickEventArgs e)
    {
        //RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=Count";
        //RadWindowForNew.Height = 550;
        //RadWindowForNew.Width = 900;
        //RadWindowForNew.Top = 10;
        //RadWindowForNew.Left = 10;
        //ViewState["Tag"] = "Count";

        //RadWindowForNew.OnClientClose = "BindCombo";
        //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //RadWindowForNew.Modal = true;
        //RadWindowForNew.VisibleStatusbar = false;
    }
    protected void ibtnState_Click(object sender, ImageClickEventArgs e)
    {
        if (dropLCountry.SelectedValue.ToString() != "0")
        {
            //RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=State&CountId=" + dropLCountry.SelectedValue + "";
            //RadWindowForNew.Height = 550;
            //RadWindowForNew.Width = 900;
            //RadWindowForNew.Top = 10;
            //RadWindowForNew.Left = 10;
            //ViewState["Tag"] = "State";
            //hdnCountry.Value = dropLCountry.SelectedValue.ToString();
            //RadWindowForNew.OnClientClose = "BindCombo";
            //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //RadWindowForNew.Modal = true;
            //RadWindowForNew.VisibleStatusbar = false;
        }
    }

    protected void ibtnCity_Click(object sender, ImageClickEventArgs e)
    {
        if (dropLCountry.SelectedValue.ToString() != "0" && dropLState.SelectedValue.ToString() != "0")
        {
            //RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=City&StateId=" + dropLState.SelectedValue + "";
            //RadWindowForNew.Height = 550;
            //RadWindowForNew.Width = 900;
            //RadWindowForNew.Top = 10;
            //RadWindowForNew.Left = 10;
            //ViewState["Tag"] = "City";
            //hdnCountry.Value = dropLCountry.SelectedValue.ToString();
            //hdnState.Value = dropLState.SelectedValue.ToString();
            //RadWindowForNew.OnClientClose = "BindCombo";
            //RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            //RadWindowForNew.Modal = true;
            //RadWindowForNew.VisibleStatusbar = false;
        }
    }
    protected void ibtnCityArea_Click(object sender, ImageClickEventArgs e)
    {
        //if (dropLCountry.SelectedValue.ToString() != "0" && dropLState.SelectedValue.ToString() != "0")
        //{
        //    RadWindowForNew.NavigateUrl = "~/PRegistration/Status.aspx?CtrlDesp=CityArea&CityId=" + dropLCity.SelectedValue + "";
        //    RadWindowForNew.Height = 550;
        //    RadWindowForNew.Width = 900;
        //    RadWindowForNew.Top = 10;
        //    RadWindowForNew.Left = 10;
        //    ViewState["Tag"] = "CityArea";
        //    hdnCountry.Value = dropLCountry.SelectedValue.ToString();
        //    hdnState.Value = dropLState.SelectedValue.ToString();
        //    hdnCity.Value = dropLCity.SelectedValue.ToString();
        //    RadWindowForNew.OnClientClose = "BindCombo";
        //    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        //    RadWindowForNew.Modal = true;
        //    RadWindowForNew.VisibleStatusbar = false;
        //}
    }
    protected void btnFillCombo_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
            BaseC.Patient objBc = new BaseC.Patient(sConString);
            DataSet ds = new DataSet();
            ds = null;
            if (ViewState["Tag"].ToString() == "Iden")
            {
                ds = (DataSet)objBc.GetPatientIdentityType();
                dropIdentityType.Items.Clear();
                dropIdentityType.DataSource = ds;
                dropIdentityType.DataValueField = "ID";
                dropIdentityType.DataTextField = "Description";
                dropIdentityType.DataBind();
            }
            else if (ViewState["Tag"].ToString() == "Count")
            {
                DataSet objDs = (DataSet)objCM.GetPatientCountry();
                dropLCountry.Items.Clear();
                Cache["CountryTable"] = objDs;
                dropLCountry.DataSource = (DataSet)Cache["CountryTable"];
                dropLCountry.DataTextField = "CountryName";
                dropLCountry.DataValueField = "CountryId";
                dropLCountry.DataBind();
                if (hdnCountry.Value != "")
                    dropLCountry.SelectedValue = hdnCountry.Value;
                LocalCountry_SelectedIndexChanged(null, null);
            }
            else if (ViewState["Tag"].ToString() == "State")
            {
                dropLState.Items.Clear();
                DataSet objds = (DataSet)objCM.GetPatientState(0);
                Cache["StateTable"] = objds;
                dropLState.DataSource = (DataSet)Cache["StateTable"];
                dropLState.DataTextField = "StateName";
                dropLState.DataValueField = "StateID";
                dropLState.DataBind();
                dropLState.SelectedIndex = 0;
                if (hdnState.Value != "")
                    dropLState.SelectedValue = hdnState.Value;
                LocalState_SelectedIndexChanged(null, null);

            }
            else if (ViewState["Tag"].ToString() == "City")
            {
                dropLCity.Items.Clear();
                DataSet objds = (DataSet)objCM.GetPatientCity(0);
                Cache["CityTable"] = objds;
                dropLCity.DataSource = (DataSet)Cache["CityTable"];
                dropLCity.DataTextField = "CityName";
                dropLCity.DataValueField = "CityID";
                dropLCity.DataBind();
                if (hdnCity.Value != "")
                    dropLCity.SelectedValue = hdnCity.Value;

                LocalCity_OnSelectedIndexChanged(sender, e);

            }
            else if (ViewState["Tag"].ToString() == "CityArea")
            {
                ddlCityArea.Items.Clear();
                DataSet objds = (DataSet)objCM.GetPatientCityArea(common.myInt(hdnCity.Value));
                ddlCityArea.DataSource = objds;
                ddlCityArea.DataTextField = "AreaName";
                ddlCityArea.DataValueField = "AreaID";
                ddlCityArea.DataBind();
                ddlCityArea_SelectedIndexChanged(sender, e);
                ddlCityArea.Focus();
            }
            else
            {
                // fillReferalBy();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    #endregion
    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }
    protected void btnGotoMenu_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect("~/PRegistration/Demographics.aspx", false);
    }
    protected void txtYear_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        if (txtYear.Text != "")
        {
            txtMonth.Focus();
        }
        else
        {
            dtpDateOfBirth.Text = "";
            txtYear.Text = "";
            txtYear.Focus();
        }
    }
    protected void txtMonth_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        txtDays.Focus();
    }
    protected void txtDays_TextChanged(Object sender, EventArgs e)
    {
        DOBCalc();
        ddlParentof.Focus();
    }
    void DOBCalc()
    {
        try
        {
            int year = common.myInt(txtYear.Text);
            int month = common.myInt(txtMonth.Text);
            int day = common.myInt(txtDays.Text);

            DateTime DOB = DateTime.Now.AddDays(-day);
            DOB = DOB.AddMonths(-month);
            DOB = DOB.AddYears(-year);
            if (Convert.ToDateTime("1900/01/01") < common.myDate(DOB))
            {
                dtpAgeDateOfBirth.Text = common.myDate(DOB).ToString("dd/MM/yyyy");
                txtYear.Text = common.myStr(year);
                txtMonth.Text = common.myStr(month);
                txtDays.Text = common.myStr(day);
            }
            else
            {
                txtYear.Text = "";
                txtYear.Focus();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void rdoPayertype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // lnkInsuranceDetails.Visible = false;
            //lnkInsuranceDetailsPopup.Visible = false;
            //if (rdoPayertype.SelectedValue.ToString() == "0")
            //{
            //    InsurancecompanyBind("C", 0);
            //    ddlFacility.Focus();
            //}
            //else if (rdoPayertype.SelectedValue.ToString() == "1")
            //{
            //    InsurancecompanyBind("C", 0);
            //    ddlCompany.SelectedIndex = 0;
            //    ddlCompany_OnSelectedIndexChanged(sender, e);
            //    rdoPayertype.SelectedIndex = 1;
            //    ddlCompany.Focus();
            //}
            //else if (rdoPayertype.SelectedValue.ToString() == "2")
            //{
            //    InsurancecompanyBind("I", 0);
            //    ddlCompany.SelectedIndex = 0;
            //    ddlCompany_OnSelectedIndexChanged(sender, e);
            //    rdoPayertype.SelectedIndex = 2;
            //    ddlCompany.Focus();
            //    lnkInsuranceDetails.Visible = true;
            //    lnkInsuranceDetailsPopup.Visible = true;
            //}
            //else
            //{
            //    ddlSponsor.Enabled = false;
            //    ddlCompany.Enabled = false;

            //}
            PatientInfo();
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void bindKinRelation()
    {
        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //populate Admiting Doctor drop down control
        Hashtable htIn = new Hashtable();
        htIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
        StringBuilder sbSQL = new StringBuilder();
        sbSQL.Append(" SELECT KinId, KinName from KinRelation WITH (NOLOCK) ORDER BY KinName ");
        DataSet dsRelation = new DataSet();
        dsRelation = dl.FillDataSet(CommandType.Text, sbSQL.ToString(), htIn);
        if (dsRelation.Tables.Count > 0 && dsRelation.Tables[0].Rows.Count > 0)
        {
            //ddlKinRelation.DataSource = dsRelation.Tables[0];
            //ddlKinRelation.DataTextField = "KinName";
            //ddlKinRelation.DataValueField = "KinId";
            //ddlKinRelation.DataBind();
            //ddlKinRelation.Items.Insert(0, "SELECT");
        }
    }
    protected void ddlCompany_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        try
        {
            //if (common.myInt(ddlCompany.SelectedValue) > 0
            //    && common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["SeniorCitizenCompany"]))//mmb
            //{
            //    if (common.myInt(txtYear.Text) < common.myInt(Session["SeniorCitizenAge"]))
            //    {
            //        Alert.ShowAjaxMsg("Company cannot be tagged with patient as patient is not a senior citizen..", Page.Page);
            //        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(ddlSponsor.SelectedValue).Trim()));
            //        ddlSponsor.Enabled = false;
            //        return;
            //    }
            //}
            //txtEmpNo.Text = "";
            //txtEmpName.Text = "";
            //ddlKinRelation.SelectedIndex = 0;
            Hashtable hshIn = new Hashtable();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //populate Admiting Doctor drop down control
            Hashtable htIn = new Hashtable();
            htIn.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
            StringBuilder sbSQL = new StringBuilder();
            sbSQL.Append(" SELECT c.CompanyId FROM Company c WITH (NOLOCK) WHERE c.CompanyTypeId IN(SELECT ct.Id FROM CompanyType ct WITH (NOLOCK) WHERE ISNULL(ct.Name,'') = 'STAFF') OR ISNULL(c.ShortName,'') = 'STAFF' ");
            sbSQL.Append(" SELECT KinId, KinName, Gender FROM KinRelation WITH (NOLOCK) ORDER BY KinName ");

            DataSet dsStaffCompany = new DataSet();
            dsStaffCompany = dl.FillDataSet(CommandType.Text, sbSQL.ToString(), htIn);
            if (common.myInt(0) > 0 && dsStaffCompany.Tables.Count > 0 && dsStaffCompany.Tables[0].Rows.Count > 0)
            {
                DataView DV = dsStaffCompany.Tables[0].DefaultView;

                DV.RowFilter = "CompanyId=" + common.myInt(0);

                //if (DV.ToTable().Rows.Count > 0)
                //{
                //    //tblEmpDetails.Visible = true;
                //}
                //else
                //{
                //    tblEmpDetails.Visible = false;
                //}
            }
            // if (common.myStr(rdoPayertype.SelectedValue) == "2" || common.myStr(rdoPayertype.SelectedValue) == "1") //Condition added on 19-08-2014
            {
                // ddlSponsor.Items.Clear();
                string compType = "";
                //if (common.myStr(rdoPayertype.SelectedValue) == "1")
                //{
                //    compType = "C";
                //}
                //else
                //{
                //    compType = "IC";
                //}
                BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                ////  ds = objBill.getCompanyList(common.myInt(Session["HospitalLocationID"]), compType, common.myInt(ddlCompany.SelectedValue), common.myInt(Session["FacilityId"]));

                //if (ds.Tables.Count > 0)
                //{
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        ddlSponsor.DataSource = ds;
                //        ddlSponsor.DataTextField = "Name";
                //        ddlSponsor.DataValueField = "CompanyId";
                //        ddlSponsor.DataBind();
                //        ddlSponsor.Items.Insert(0, new ListItem("", "0"));
                //        ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(ddlCompany.SelectedValue).Trim()));
                //        ddlSponsor.Enabled = true;
                //    }
                //}
            }
            // else
            {
                //ddlSponsor.DataSource = (DataSet)ddlCompany.DataSource;
                //ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(ddlCompany.SelectedValue).Trim()));
                //ddlSponsor.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
        finally
        {
            objDl = null;
            ds.Dispose();
        }
    }
    void InsurancecompanyBind(string sType, Int32 iRegID)
    {
        try
        {
            BaseC.clsEMRBilling baseEBill = new BaseC.clsEMRBilling(sConString);
            //ddlCompany.Items.Clear();
            //ddlSponsor.Items.Clear();
            //ddlCompany.Enabled = true;
            //ddlSponsor.Enabled = true;
            DataSet ds = new DataSet();
            ds = baseEBill.getCompanyList(Convert.ToInt32(Session["HospitalLocationId"]), sType, 0, common.myInt(Session["FacilityId"]));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ////ddlCompany.DataSource = ds;
                    ////ddlCompany.DataTextField = "Name";
                    ////ddlCompany.DataValueField = "CompanyId";
                    ////ddlCompany.DataBind();
                    ////if (rdoPayertype.SelectedValue.ToString() != "0")
                    ////{
                    ////    ddlCompany.Items.Insert(0, new ListItem("", "0"));
                    ////}
                    ////ddlCompany.SelectedIndex = -1;
                    ////if (rdoPayertype.SelectedValue.ToString() != "2")
                    ////{
                    ////    ddlSponsor.DataSource = ds;
                    ////    ddlSponsor.DataTextField = "Name";
                    ////    ddlSponsor.DataValueField = "CompanyId";
                    ////    ddlSponsor.DataBind();
                    ////    if (rdoPayertype.SelectedValue.ToString() != "0")
                    ////    {
                    ////        ddlSponsor.Items.Insert(0, new ListItem("", "0"));
                    ////    }
                    ////    if (rdoPayertype.SelectedValue.ToString() == "0")
                    ////    {
                    ////        if (common.myInt(Session["DefaultHospitalCompanyId"]) > 0)
                    ////        {
                    ////            ddlCompany.SelectedValue = common.myStr(Session["DefaultHospitalCompanyId"]);
                    ////            ddlSponsor.SelectedValue = common.myStr(Session["DefaultHospitalCompanyId"]);
                    ////        }

                    ////        ddlCompany.Enabled = false;
                    ////        ddlSponsor.Enabled = false;
                    ////    }
                    ////}

                }

            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = ("Error: " + Ex.Message);
            objException.HandleException(Ex);
        }
    }
    protected void btnClose_OnClick(Object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true); // Call client method in radwindow page 
    }
    ////protected void txtEmpNo_OnTextChanged(object sender, EventArgs e)
    ////{
    ////    if (txtEmpNo.Text.ToString() != "")
    ////    {
    ////        BaseC.Patient objPat = new BaseC.Patient(sConString);
    ////        string EName = objPat.GetValidPatientEmployeeTaggingNew(common.myStr(txtEmpNo.Text.Trim()), common.myInt(Session["FacilityId"]));
    ////        if (common.myStr(EName) != "")
    ////        {
    ////           // txtEmpName.Text = EName.ToString().Trim();
    ////        }
    ////        else
    ////        {
    ////            Alert.ShowAjaxMsg("Employee No. is not valid. Please!! Enter valid code..", Page.Page);
    ////            return;
    ////        }
    ////    }
    ////}
    protected void btnNewBornBaby_Click(object sender, EventArgs e)
    {
        try
        {
            BaseC.Patient objPatient = new BaseC.Patient(sConString);
            BaseC.Security AuditCA = new BaseC.Security(sConString);
            BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
            DataSet objDs = (DataSet)objwcf.GetPatientRecord(common.myInt(0), common.myInt(Session["HospitalLocationID"]));
            if (objDs != null)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["Gender"]) != "1")
                    {
                        Alert.ShowAjaxMsg("Please enter correct mother registration no", Page);
                        ViewState["SaveData"] = "N";
                        return;
                    }
                    else
                    {
                        ViewState["SaveData"] = "Y";
                    }
                    dropTitle.SelectedValue = common.myStr(ViewState["IsnewBornTitle"]);

                    txtFname.Text = objDs.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                    txtMName.Text = objDs.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
                    txtLame.Text = objDs.Tables[0].Rows[0]["LastName"].ToString().Trim();
                    ViewState["PName"] = txt_hdn_PName.Text;
                    txtPreviousName.Text = objDs.Tables[0].Rows[0]["PreviousName"].ToString().Trim();
                    Mode = "N";
                    if (common.myStr(txtAccountNo.Text) != "")
                    {
                        Mode = "E";
                    }
                    //chkExternalPatient.Checked = Convert.ToBoolean(objDs.Tables[0].Rows[0]["ExternalPatient"]) ? true : false;
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["FacilityID"].ToString()) != "")
                    {
                        // ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(objDs.Tables[0].Rows[0]["FacilityID"].ToString()));
                    }
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]) != "")
                        dropIdentityType.SelectedIndex = dropIdentityType.Items.IndexOf(dropIdentityType.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]).Trim()));

                    txtIdentityNumber.Text = objDs.Tables[0].Rows[0]["IdentityNumber"].ToString().Trim();
                    txtLAddress1.Text = objDs.Tables[0].Rows[0]["LocalAddress"].ToString().Trim();
                    txtLAddress2.Text = objDs.Tables[0].Rows[0]["LocalAddress2"].ToString().Trim();
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"]) != "")
                    {
                        dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"])));
                        LocalCountry_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLCountry.SelectedIndex = -1;
                    }
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]) != "")
                    {
                        dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]).Trim()));
                        LocalState_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLState.SelectedIndex = -1;
                    }
                    if (common.myStr(objDs.Tables[0].Rows[0]["LocalCity"]) != "")
                    {
                        dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCity"]).Trim()));
                        LocalCity_OnSelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLCity.SelectedIndex = -1;
                    }
                    if (common.myStr(objDs.Tables[0].Rows[0]["CityAreaId"]) != "")
                    {
                        ddlCityArea.SelectedIndex = ddlCityArea.Items.IndexOf(ddlCityArea.Items.FindByValue(objDs.Tables[0].Rows[0]["CityAreaId"].ToString().Trim()));
                    }
                    else
                    {
                        ddlCityArea.SelectedIndex = -1;
                    }
                    txtLPhoneRes.Text = objDs.Tables[0].Rows[0]["PhoneHome"].ToString();
                    txtPMobile.Text = objDs.Tables[0].Rows[0]["MobileNo"].ToString();
                    //if (objDs.Tables[0].Rows[0]["Localpin"] != null)
                    //{
                    //    ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(objDs.Tables[0].Rows[0]["Localpin"].ToString().Trim()));
                    //    txtLPin.Text = objDs.Tables[0].Rows[0]["Localpin"].ToString().Trim();
                    //}
                    txtPEmailID.Text = objDs.Tables[0].Rows[0]["Email"].ToString();
                    if (objDs.Tables[0].Rows[0]["NationalityId"] != null)
                    {
                        dropNationality.SelectedIndex = dropNationality.Items.IndexOf(dropNationality.Items.FindByValue(objDs.Tables[0].Rows[0]["NationalityId"].ToString()));
                    }
                    //if (objDs.Tables[0].Rows[0]["ReligionId"] != null)
                    //{
                    //    dropReligion.SelectedIndex = dropReligion.Items.IndexOf(dropReligion.Items.FindByValue(objDs.Tables[0].Rows[0]["ReligionId"].ToString()));
                    //}
                    if (objDs.Tables[0].Rows[0]["ReferredTypeID"] != null && common.myStr(objDs.Tables[0].Rows[0]["ReferredTypeID"]) != "0")
                    {
                        // dropReferredType.SelectedIndex = dropReferredType.Items.IndexOf(dropReferredType.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["ReferredTypeID"])));

                        RefType_SelectedIndexChanging(this, null);
                        if (common.myStr(objDs.Tables[0].Rows[0]["ReferredByID"]) != "")
                        {
                            // dropRefBy.SelectedIndex = dropRefBy.Items.IndexOf(dropRefBy.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["ReferredByID"])));
                        }
                        else
                        {
                            //dropRefBy.Items.Insert(0, new ListItem("Select"));
                            //dropRefBy.Items[0].Value = "0";
                            //dropRefBy.SelectedValue = "0";
                        }
                    }
                    else
                    {
                        //dropReferredType.SelectedValue = "0";
                        //dropRefBy.Items.Insert(0, new ListItem("Select"));
                        //dropRefBy.Items[0].Value = "0";
                        //dropRefBy.SelectedValue = "0";
                    }
                    if (objDs.Tables[0].Rows[0]["RenderingProvider"] != null)
                    {
                        //ddlRenderingProvider.SelectedValue = objDs.Tables[0].Rows[0]["RenderingProvider"].ToString();
                    }
                    //ddlStatus.SelectedValue = objDs.Tables[0].Rows[0]["Status"].ToString().TrimEnd();
                    txtSSN.Text = objDs.Tables[0].Rows[0]["SocialSecurityNo"].ToString();
                    if (objDs.Tables[0].Rows[0]["LeadSourceId"] != null)
                    {
                        // ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(objDs.Tables[0].Rows[0]["LeadSourceId"].ToString()));
                    }
                    if (common.myStr(Session["DefaultCompany"]) == common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString()) &&
                      common.myStr(Session["DefaultCompany"]) == common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString()))
                    {
                        // rdoPayertype.SelectedValue = "0";
                        rdoPayertype_OnSelectedIndexChanged(this, null);
                        //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        //ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                        //ddlCompany.Enabled = false;
                    }
                    else if (common.myBool(objDs.Tables[0].Rows[0]["IsInsuranceCompany"]) == false)
                    {
                        //ddlCompany.Enabled = true;
                        //rdoPayertype.SelectedValue = "1";
                        rdoPayertype_OnSelectedIndexChanged(this, null);
                        //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ddlCompany_OnSelectedIndexChanged(null, null);
                        // ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                    }
                    else if (common.myBool(objDs.Tables[0].Rows[0]["IsInsuranceCompany"]) == true)
                    {
                        // ddlCompany.Enabled = true;
                        // rdoPayertype.SelectedValue = "2";
                        rdoPayertype_OnSelectedIndexChanged(this, null);
                        // ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ddlCompany_OnSelectedIndexChanged(null, null);
                        // ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                    }
                    //if (common.myInt(ddlCompany.SelectedValue) > 0 && (common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["StaffCompanyId"]) || common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["StaffDependentCompanyId"])))
                    //    tblEmpDetails.Visible = true;
                    //txtEmpNo.Text = objDs.Tables[0].Rows[0]["TaggedEmpNo"].ToString(); ;
                    //txtEmpName.Text = objDs.Tables[0].Rows[0]["TaggedEmpName"].ToString();
                    //ddlKinRelation.SelectedIndex = ddlKinRelation.Items.IndexOf(ddlKinRelation.Items.FindByText(common.myStr(objDs.Tables[0].Rows[0]["TaggedEmpName"].ToString())));
                    // chkVIP.Checked = common.myBool(objDs.Tables[0].Rows[0]["VIP"].ToString());
                    txtVIPNarration.Text = common.myStr(objDs.Tables[0].Rows[0]["VIPNarration"].ToString());
                    //  txtEmergencyName.Text = common.myStr(objDs.Tables[0].Rows[0]["NotificationName"].ToString());
                    //  txtEmergencyContactNo.Text = common.myStr(objDs.Tables[0].Rows[0]["NotificationPhoneNo"].ToString());
                    // txtEmiratesID.Text = common.myStr(objDs.Tables[0].Rows[0]["EmiratesID"]);
                    ViewState["UHID"] = txtRegNo.Text;
                    String strFullName = objDs.Tables[0].Rows[0]["FirstName"].ToString() + " " + objDs.Tables[0].Rows[0]["MiddleName"].ToString() + " " + objDs.Tables[0].Rows[0]["LastName"].ToString();
                    lnkOtherDetails.Visible = false;
                    lnkPayment.Visible = false;
                    lnkResponsibleParty.Visible = false;
                    if (common.myStr(Session["ModuleFlag"]) == "ER")
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    else
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    if (Request.QueryString["Category"] != null)
                    {
                        if (Request.QueryString["Category"].ToString() == "PopUp")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            if (common.myStr(Session["ModuleFlag"]) == "ER")
                            {
                                lnkbtnAppointment.Visible = false;
                                lnkbtnBilling.Visible = false;
                            }
                            else
                            {
                                lnkbtnAppointment.Visible = true;
                                lnkbtnBilling.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (common.myStr(Session["ModuleFlag"]) == "ER")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            lnkbtnAppointment.Visible = true;
                            lnkbtnBilling.Visible = false;
                        }
                    }
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        ShowImage();
                    }
                    else
                        PatientImage.ImageUrl = "~/Images/patientLeft.jpg";

                    lblMessage.Text = "";
                   
                    SetPermission(btnSave, "E", true);
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void txtMothergNo_OnTextChanged(object sender, EventArgs e)
    {
        btnNewBornBaby_Click(null, null);
    }
    public void fillReferalBy()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hsInput = new Hashtable();
        hsInput.Add("intHospitalId", Session["HospitalLocationID"]);
        hsInput.Add("intFacilityId", common.myInt(Session["FacilityId"]));
        // hsInput.Add("chvRefType", dropReferredType.SelectedValue);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "uspGetReferByDoctor", hsInput);
        // dropRefBy.Items.Clear();
        ListItem item = new ListItem();
        item.Value = "0";
        item.Text = "Select";
        //dropRefBy.Items.Add(item);
        //dropRefBy.DataSource = ds;
        //dropRefBy.DataTextField = "Name";
        //dropRefBy.DataValueField = "Id";
        //dropRefBy.DataBind();

        //   lnkAddName.Text = "Add " + common.myStr(dropReferredType.SelectedItem.Text) + " Name";

    }
    public void setControlHospitalbased()
    {
        string sAllow = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "DateofBirthManadatory", sConString);

        if (sAllow == "Y")
        {
            txtYear.Enabled = false;
            txtMonth.Enabled = false;
            txtDays.Enabled = false;
        }
        string IsFatherMotherRequired = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
             common.myInt(Session["FacilityId"]), "IsFatherandMotherName", sConString);

        if (IsFatherMotherRequired == "Y")
        {
            lblGardianname.Text = "Father Name";
            ddlParentof.SelectedValue = "S/O";
            ddlParentof.Enabled = false;
            //trMother.Visible = true;
            txtParentof.TabIndex = 5;
            // txtMothername.TabIndex = 5;
        }
    }
    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnNew, false);
        ua1.DisableEnableControl(btnSave, false);
        ua1.DisableEnableControl(btnprint, false);

        if ((hdnRegistrationId.Value.Equals("0")))
        {
            if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
            {
                ua1.DisableEnableControl(btnNew, true);
                ua1.DisableEnableControl(btnSave, true);
            }
        }
        else
        {
            ua1.DisableEnableControl(btnNew, false);
            ua1.DisableEnableControl(btnSave, false);
        }
        if (!(hdnRegistrationId.Value.Equals("0")))
        {
            if (ua1.CheckPermissions("E", Request.Url.AbsolutePath))
            {
                ua1.DisableEnableControl(btnSave, true);
                if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
                {
                    ua1.DisableEnableControl(btnNew, true);
                }
            }
            else
            {
                ua1.DisableEnableControl(btnSave, false);
                if (ua1.CheckPermissions("N", Request.Url.AbsolutePath))
                {
                    ua1.DisableEnableControl(btnNew, true);
                }
            }
        }
        if (ua1.CheckPermissions("P", Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnprint, true);
        }
        ua1.Dispose();
    }
    private void SetPermission(Button btnID, string mode, bool action)
    {
        UserAuthorisations ua1 = new UserAuthorisations();
        ua1.DisableEnableControl(btnID, false);

        if (ua1.CheckPermissions(mode, Request.Url.AbsolutePath))
        {
            ua1.DisableEnableControl(btnID, action);
        }
        else
        {
            ua1.DisableEnableControl(btnID, !action);
        }
        ua1.Dispose();
    }
    protected void btnFillInsurance_Click(object sender, EventArgs e)
    {
        if (common.myInt(hdnPayer.Value) != 0)
        {
            //rdoPayertype.SelectedValue = "2";
            //ddlCompany.SelectedValue = hdnPayer.Value;
            ddlCompany_OnSelectedIndexChanged(null, null);
            // ddlSponsor.SelectedValue = hdnSponsor.Value;
        }
    }
    protected void btnFillInsurancePop_Click(object sender, EventArgs e)
    {
        if (common.myInt(hdnPayer.Value) != 0)
        {
            //rdoPayertype.SelectedValue = "2";
            //ddlCompany.SelectedValue = hdnPayer.Value;
            ddlCompany_OnSelectedIndexChanged(null, null);
            //ddlSponsor.SelectedValue = hdnSponsor.Value;
        }
    }
    protected void btnFindchecklis_Click(object sender, EventArgs e)
    {
        if (hdnchecklistid.Value != "")
        {
            ViewState["CheckListId"] = hdnchecklistid.Value;
            PatientInfo();
        }
    }
    protected void btnCancel_onClick(object sender, EventArgs e)
    {
      dvRedirect.Visible = false;
      Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
      dropTitle.Focus();
        
    }
    protected void btnOk_OnClick(object sender, EventArgs e)
    {
      //  dvMessage.Visible = false;
    }
    void BindMessage()
    {
        BaseC.Patient objbc = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        ds = objbc.GetAlertBlockMessage(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
            common.myInt(hdnRegistrationId.Value));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ds.Tables[0].DefaultView.RowFilter = "NoteType=4";
            if (ds.Tables[0].DefaultView.Count > 0)
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }
            ds.Tables[0].DefaultView.RowFilter = "";
            //dlMissingDocument.DataSource = ds.Tables[0];
            //dlMissingDocument.DataBind();
            //dvMessage.Visible = true;
        }
        else
        {
            //dvMessage.Visible = false;
        }
    }
    protected void btnGetMotherInfo_Click(object sender, EventArgs e)
    {
        txtMothergNo_OnTextChanged(null, null);
    }
    protected void lbtnSearchPatient_Click(object sender, EventArgs e)
    {
        ClearFields();
        RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0";
      //  RadWindowForNew.NavigateUrl = "/Pharmacy//Saleissue/PatientDetails.aspx?OPIP=O&RegEnc=0";
      //  RadWindowForNew.NavigateUrl = "/EMR/PatientDetailsNew.aspx?OPIP=O&RegEnc=0&CloseTo=Reg";
        RadWindowForNew.Height = 600;
        RadWindowForNew.Width = 970;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }
    protected void btnSearchMob_Click(object sender, EventArgs e)
    {
        txtAccountNo.Text = string.Empty;
        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        if (txtSearchMobile.Text != "")
        {
            int mob = common.myInt(txtSearchMobile.Text);
            int hos = common.myInt(Session["HospitalLocationId"]);


           DataSet objDs = (DataSet)objPatient.getPatientDetailsByMobileNo(common.myStr(txtSearchMobile.Text), common.myInt(Session["HospitalLocationId"]));

     
            if (objDs.Tables[0].Rows.Count > 0)
            {
                if (objDs.Tables[0].Rows.Count >= 2)
                {
                    //Session["mobNo"] = txtSearchMobile.Text;
                    //////string url = "/Pharmacy/Saleissue/PatientDetailsNewMob.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();
                    //////string fullURL = "window.open('" + url + "', '_blank', 'height=300,width=500,status=yes,toolbar=no,menubar=no,location=no,scrollbars=yes,resizable=no,titlebar=no' );";
                    //////ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", fullURL, true);
                    //string s = "window.open('" + url + "', 'popup_window', 'width=300,height=100,left=100,top=100,resizable=yes');";                   
                    // ScriptManager.RegisterStartupScript(this.GetType(), "script", "OpenPatienDetailsOnMobile();", true);
                    //  ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('/PRegistration/Default.aspx','_new');", true);
                    //ScriptManager.RegisterStartupScript(update1, update1.GetType(), "OpenWindow", "window.open('/Pharmacy/Saleissue/PatientDetailsNewMob.aspx?OPIP=O&RegEnc=0&CloseTo=Reg','width=300,height=100,left=100,top=100,resizable=yes');", true);
                   //ScriptManager.RegisterStartupScript(update1, update1.GetType(), "OpenWindow", "window.open('" + url + "');", true);                   







                    ClearFields();
                    RadWindowForNew.NavigateUrl = "/EMR/PatientDetails.aspx?OPIP=O&RegEnc=0&Mob=" + txtSearchMobile.Text.Trim();
                    //RadWindowForNew.NavigateUrl = "/Pharmacy//Saleissue/PatientDetails.aspx?OPIP=O&RegEnc=0";
                    //RadWindowForNew.NavigateUrl = "/Pharmacy//Saleissue/PatientDetails.aspx?OPIP=O&RegEnc=0&Mob=" + txtSearchMobile.Text.Trim();
                   // RadWindowForNew.NavigateUrl = "/EMR/PatientDetailsNew.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();
                    RadWindowForNew.Height = 600;
                    RadWindowForNew.Width = 970;
                    RadWindowForNew.Top = 40;
                    RadWindowForNew.Left = 100;
                    RadWindowForNew.OnClientClose = "SearchPatientOnClientClose";
                    RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
                    RadWindowForNew.Modal = true;
                    RadWindowForNew.VisibleStatusbar = false;



                    //////RadWindowForNew.NavigateUrl = "~/Pharmacy/Saleissue/PatientDetailsNewMob.aspx?OPIP=O&RegEnc=0&CloseTo=Reg&Mob=" + txtSearchMobile.Text.Trim();
                    //////RadWindowForNew.Width = 1200;
                    //////RadWindowForNew.Height = 630;
                    //////RadWindowForNew.Top = 10;
                    //////RadWindowForNew.Left = 10;
                    //////RadWindowForNew.OnClientClose = "BindPatientOnClientClose";
                    //////RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
                    //////RadWindowForNew.Modal = true;
                    //////RadWindowForNew.VisibleStatusbar = false;
                    //////RadWindowForNew.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;
                                   
                }
                else
                {
                    txtAccountNo.Text = common.myStr(objDs.Tables[0].Rows[0][1]);
                    populateControlsForEditing(Convert.ToInt32(objDs.Tables[0].Rows[0][1]));
                }
            }
        }
        else
        { 
               
                Alert.ShowAjaxMsg("Please! Enter Mobile No..", Page);
                 return;        
            

        }
    }



    protected void btnprintlabel_Click(object sender, EventArgs e)
    {
        if (txtRegNo.Text != "")
        {
            //RadWindowForNew.NavigateUrl = "~/EMRReports/RegistrationPatientDetails.aspx?RegistrationId=" + txtRegNo.Text.ToString().Trim() + "&Pid=DemographicLabel";
            RadWindowForNew.NavigateUrl = "~/EMRReports/PrintReport.aspx?ReportName=RegCard&Vlue=" + txtRegNo.Text.ToString().Trim() + "&Pid=DemographicLabel";
            RadWindowForNew.Height = 600;
            RadWindowForNew.Width = 900;
            RadWindowForNew.Top = 40;
            RadWindowForNew.Left = 100;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            Alert.ShowAjaxMsg("Please Select Patient !", Page);
        }
    }
    protected void lnkUHID_Click(object sender, EventArgs e)
    {
        try
        {

            if (common.myStr(txtAccountNo.Text) != string.Empty)
            {
                populateControlsForEditing(common.myInt(txtAccountNo.Text));
            }
            else
            {
                Alert.ShowAjaxMsg("Please! Enter UHID.", Page);
                return;   

            }


            
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }






    protected void btnGetSinglePatientInfo_Click(object sender, EventArgs e)
    {

        try
        {
           int RegistrationId=common.myInt(hdnRegistrationId.Value);
           int RegistrationNo = common.myInt(hdnRegistrationNo.Value);

           if (common.myStr(RegistrationNo) != string.Empty)
            {
                populateControlsForBinding(common.myInt(RegistrationId));
            }
            //else
            //{
            //    Alert.ShowAjaxMsg("Please! Enter UHID.", Page);
            //    return;   

            //}


            
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
     }



    protected void populateControlsForBinding(int RegistrationId)
    {

        BaseC.Patient objPatient = new BaseC.Patient(sConString);
        BaseC.Security AuditCA = new BaseC.Security(sConString);
        BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);


        DataSet objDs = (DataSet)objPatient.GetPatientRecord(common.myInt(RegistrationId), common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));
        try
        {

            if (objDs != null)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    //DeleteFiles();
                    hdnRegistrationId.Value = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);
                    txtAccountNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationNo"]);
                    txtRegNo.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationId"]);

                    AuditCA.AuditCommonAccess(Convert.ToInt16(Session["HospitalLocationID"]), Convert.ToInt32(Session["FacilityID"]), Convert.ToInt32(txtAccountNo.Text), 0, Convert.ToInt32(ViewState["PageId"]), 0, Convert.ToInt32(Session["UserID"]), 0, "ACCESSED", Convert.ToString(Session["IPAddress"]));
                    txtRegistrationDate.Text = Convert.ToString(objDs.Tables[0].Rows[0]["RegistrationDate"]);
                    txtFname.Text = objDs.Tables[0].Rows[0]["FirstName"].ToString().Trim();
                    txtMName.Text = objDs.Tables[0].Rows[0]["MiddleName"].ToString().Trim();
                    txtLame.Text = objDs.Tables[0].Rows[0]["LastName"].ToString().Trim();
                    ViewState["PName"] = txt_hdn_PName.Text;
                    string[] str = Convert.ToString(objDs.Tables[0].Rows[0]["PreviousName"].ToString()).Split('#');
                    if (str.Length == 2)
                    {
                        ddlParentof.SelectedValue = str[0].ToString().Trim();
                        txtParentof.Text = str[1].ToString().Trim();
                    }
                    //if (chkNewBorn.Checked == false)
                    //{
                    dtpDateOfBirth.Text = Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateofBirth"]).ToString("dd/MM/yyyy");
                    txtYear.Text = objDs.Tables[0].Rows[0]["AgeYear"].ToString().Trim();
                    txtMonth.Text = objDs.Tables[0].Rows[0]["AgeMonth"].ToString().Trim();
                    txtDays.Text = objDs.Tables[0].Rows[0]["AgeDays"].ToString().Trim();
                    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["Gender"]) != "")
                    {
                        dropSex.SelectedIndex = dropSex.Items.IndexOf(dropSex.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["Gender"])));
                    }
                    if (common.myInt(txtYear.Text) == 0 && common.myInt(txtMonth.Text) == 0 && common.myInt(txtDays.Text) == 0)
                    {
                        btnCalAge_Click(null, null);
                    }
                    dropTitle.SelectedIndex = dropTitle.Items.IndexOf(dropTitle.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["TitleId"])));
                    //}
                    //else
                    //{
                    //    dropTitle.SelectedValue = common.myStr(ViewState["IsnewBornTitle"]);
                    //    dtpDateOfBirth.Text = Convert.ToDateTime(objDs.Tables[0].Rows[0]["DateofBirth"]).ToString("dd/MM/yyyy");
                    //}
                    if ((common.myStr(objDs.Tables[0].Rows[0]["MotherRegistrationNo"].ToString().Trim()) != "0") && (common.myStr(objDs.Tables[0].Rows[0]["MotherRegistrationNo"].ToString().Trim()) != ""))
                    {
                        // chkNewBorn.Checked = common.myBool(objDs.Tables[0].Rows[0]["NewBorn"].ToString().Trim());
                        //if (chkNewBorn.Checked)
                        //{
                        //    txtMothergNo.Text = objDs.Tables[0].Rows[0]["MotherRegistrationNo"].ToString().Trim();
                        //}
                        //else
                        //{
                        //    txtMothergNo.Text = "";
                        //}
                    }
                    //else
                    //{
                    //    chkNewBorn.Checked = common.myBool(objDs.Tables[0].Rows[0]["NewBorn"].ToString().Trim());
                    //    txtMothergNo.Text = "";
                    //}
                    // chkExternalPatient.Checked = Convert.ToBoolean(objDs.Tables[0].Rows[0]["ExternalPatient"]) ? true : false;
                    //if (Convert.ToString(objDs.Tables[0].Rows[0]["MaritalStatus"].ToString()) != "")
                    //{
                    //    dropMarital.SelectedIndex = dropMarital.Items.IndexOf(dropMarital.Items.FindByValue(objDs.Tables[0].Rows[0]["MaritalStatus"].ToString()));
                    //}
                    ////if (Convert.ToString(objDs.Tables[0].Rows[0]["HCFID"].ToString()) != "")
                    ////{
                    ////    ddlHealthCareFacilitator.SelectedIndex = ddlHealthCareFacilitator.Items.IndexOf(ddlHealthCareFacilitator.Items.FindByValue(objDs.Tables[0].Rows[0]["HCFID"].ToString()));
                    ////}
                    ////if (Convert.ToString(objDs.Tables[0].Rows[0]["FacilityID"].ToString()) != "")
                    ////{
                    ////    ddlFacility.SelectedIndex = ddlFacility.Items.IndexOf(ddlFacility.Items.FindByValue(objDs.Tables[0].Rows[0]["FacilityID"].ToString()));
                    ////}
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]) != "")
                        dropIdentityType.SelectedIndex = dropIdentityType.Items.IndexOf(dropIdentityType.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["IdentityTypeID"]).Trim()));

                    txtIdentityNumber.Text = objDs.Tables[0].Rows[0]["IdentityNumber"].ToString().Trim();
                    txtLAddress1.Text = objDs.Tables[0].Rows[0]["LocalAddress"].ToString().Trim();
                    txtLAddress2.Text = objDs.Tables[0].Rows[0]["LocalAddress2"].ToString().Trim();
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"]) != "")
                    {
                        dropLCountry.SelectedIndex = dropLCountry.Items.IndexOf(dropLCountry.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCountry"])));
                        LocalCountry_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLCountry.SelectedIndex = -1;
                    }
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]) != "")
                    {
                        dropLState.SelectedIndex = dropLState.Items.IndexOf(dropLState.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalState"]).Trim()));
                        LocalState_SelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLState.SelectedIndex = -1;
                    }
                    if (common.myStr(objDs.Tables[0].Rows[0]["LocalCity"]) != "")
                    {
                        dropLCity.SelectedIndex = dropLCity.Items.IndexOf(dropLCity.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["LocalCity"]).Trim()));
                        LocalCity_OnSelectedIndexChanged(this, null);
                    }
                    else
                    {
                        dropLCity.SelectedIndex = -1;
                    }
                    if (common.myStr(objDs.Tables[0].Rows[0]["CityAreaId"]) != "")
                    {
                        ddlCityArea.SelectedIndex = ddlCityArea.Items.IndexOf(ddlCityArea.Items.FindByValue(objDs.Tables[0].Rows[0]["CityAreaId"].ToString().Trim()));
                    }
                    else
                    {
                        ddlCityArea.SelectedIndex = -1;
                    }
                    //if (common.myStr(objDs.Tables[0].Rows[0]["OccupationId"]) != "")
                    //{
                    //    //ddlOccupation.SelectedIndex = ddlCityArea.Items.IndexOf(ddlCityArea.Items.FindItemByValue(objDs.Tables[0].Rows[0]["OccupationId"].ToString().Trim()));
                    //    ddlOccupation.SelectedValue = objDs.Tables[0].Rows[0]["OccupationId"].ToString().Trim();
                    //}
                    //else
                    //{
                    //    ddlOccupation.SelectedIndex = -1;
                    //}
                    txtLPhoneRes.Text = objDs.Tables[0].Rows[0]["PhoneHome"].ToString();
                    txtPMobile.Text = objDs.Tables[0].Rows[0]["MobileNo"].ToString();
                    //if (objDs.Tables[0].Rows[0]["Localpin"] != null)
                    //{
                    //    ddlZip.SelectedIndex = ddlZip.Items.IndexOf(ddlZip.Items.FindByValue(objDs.Tables[0].Rows[0]["Localpin"].ToString().Trim()));
                    //    txtLPin.Text = objDs.Tables[0].Rows[0]["Localpin"].ToString().Trim();
                    //}
                    txtPEmailID.Text = objDs.Tables[0].Rows[0]["Email"].ToString();
                    if (objDs.Tables[0].Rows[0]["NationalityId"] != null)
                    {
                        dropNationality.SelectedIndex = dropNationality.Items.IndexOf(dropNationality.Items.FindByValue(objDs.Tables[0].Rows[0]["NationalityId"].ToString()));
                    }
                    //if (objDs.Tables[0].Rows[0]["ReligionId"] != null)
                    //{
                    //    dropReligion.SelectedIndex = dropReligion.Items.IndexOf(dropReligion.Items.FindByValue(objDs.Tables[0].Rows[0]["ReligionId"].ToString()));
                    //}
                    if (objDs.Tables[0].Rows[0]["ReferredTypeID"] != null && common.myStr(objDs.Tables[0].Rows[0]["ReferredTypeID"]) != "0")
                    {
                        ////for (int i = 0; i < dropReferredType.Items.Count; i++)
                        ////{
                        ////    if (common.myStr(objDs.Tables[0].Rows[0]["ReferredTypeID"]) == common.myStr(dropReferredType.Items[i].Attributes["Id"]))
                        ////    {
                        ////        dropReferredType.SelectedIndex = dropReferredType.Items.IndexOf(dropReferredType.Items.FindByText(dropReferredType.Items[i].Text));
                        ////        break;
                        ////    }
                        ////}
                        ////RefType_SelectedIndexChanging(this, null);
                        ////if (common.myStr(objDs.Tables[0].Rows[0]["ReferredByID"]) != "")
                        ////{
                        ////    dropRefBy.SelectedIndex = dropRefBy.Items.IndexOf(dropRefBy.Items.FindByValue(Convert.ToString(objDs.Tables[0].Rows[0]["ReferredByID"])));
                        ////}
                        ////else
                        ////{
                        ////    dropRefBy.Items.Insert(0, new ListItem("Select"));
                        ////    dropRefBy.Items[0].Value = "0";
                        ////    dropRefBy.SelectedValue = "0";
                        ////}
                    }
                    else
                    {
                        //dropReferredType.SelectedValue = "0";                       
                        ////dropRefBy.Items.Insert(0, new ListItem("Select"));
                        ////dropRefBy.Items[0].Value = "0";
                        ////dropRefBy.SelectedValue = "0";
                    }
                    ////if (objDs.Tables[0].Rows[0]["RenderingProvider"] != null)
                    ////{
                    ////    ddlRenderingProvider.SelectedValue = objDs.Tables[0].Rows[0]["RenderingProvider"].ToString();
                    ////}
                    // ddlStatus.SelectedValue = objDs.Tables[0].Rows[0]["Status"].ToString().TrimEnd();
                    txtSSN.Text = objDs.Tables[0].Rows[0]["SocialSecurityNo"].ToString();


                    //if (objDs.Tables[0].Rows[0]["LeadSourceId"] != null)
                    //{
                    //    ddlLeadSource.SelectedIndex = ddlLeadSource.Items.IndexOf(ddlLeadSource.Items.FindByValue(objDs.Tables[0].Rows[0]["LeadSourceId"].ToString()));
                    //}
                    if (common.myStr(Session["DefaultCompany"]) == common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString()) &&
                      common.myStr(Session["DefaultCompany"]) == common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString()))
                    {
                        ////rdoPayertype.SelectedValue = "0";
                        ////ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ////rdoPayertype_OnSelectedIndexChanged(this, null);
                        ////ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                        ////ddlCompany.Enabled = false;
                    }
                    else if (common.myBool(objDs.Tables[0].Rows[0]["IsInsuranceCompany"]) == false)
                    {
                        ////ddlCompany.Enabled = true;
                        ////rdoPayertype.SelectedValue = "1";
                        ////rdoPayertype_OnSelectedIndexChanged(this, null);
                        ////ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ////ddlCompany_OnSelectedIndexChanged(null, null);
                        ////ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                    }
                    else if (common.myBool(objDs.Tables[0].Rows[0]["IsInsuranceCompany"]) == true)
                    {
                        ////ddlCompany.Enabled = true;
                        ////rdoPayertype.SelectedValue = "2";
                        ////rdoPayertype_OnSelectedIndexChanged(this, null);
                        ////ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["PayorId"].ToString())));
                        ////ddlCompany_OnSelectedIndexChanged(null, null);
                        ////ddlSponsor.SelectedIndex = ddlSponsor.Items.IndexOf(ddlSponsor.Items.FindByValue(common.myStr(objDs.Tables[0].Rows[0]["SponsorId"].ToString())));
                    }
                    ////if (common.myInt(ddlCompany.SelectedValue) > 0 && (common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["StaffCompanyId"]) || common.myInt(ddlCompany.SelectedValue) == common.myInt(Session["StaffDependentCompanyId"])))
                    ////    tblEmpDetails.Visible = true;
                    ////txtEmpNo.Text = objDs.Tables[0].Rows[0]["TaggedEmpNo"].ToString();
                    ////txtEmpNo_OnTextChanged(this, null);
                    ////ddlKinRelation.SelectedIndex = ddlKinRelation.Items.IndexOf(ddlKinRelation.Items.FindByText(objDs.Tables[0].Rows[0]["TaggedEmpName"].ToString()));
                    //chkVIP.Checked = common.myBool(objDs.Tables[0].Rows[0]["VIP"].ToString());
                    txtVIPNarration.Text = common.myStr(objDs.Tables[0].Rows[0]["VIPNarration"].ToString());
                    // txtEmergencyName.Text = common.myStr(objDs.Tables[0].Rows[0]["NotificationName"].ToString());
                    //txtEmergencyContactNo.Text = common.myStr(objDs.Tables[0].Rows[0]["NotificationPhoneNo"].ToString());
                    // txtEmiratesID.Text = common.myStr(objDs.Tables[0].Rows[0]["EmiratesID"]);
                    // txtMothername.Text = common.myStr(objDs.Tables[0].Rows[0]["MotherName"]); //Added  for child Trust    
                    //txtFatherName.Text = common.myStr(objDs.Tables[0].Rows[0]["FatherName"]); //Added  for child Trust    

                    //if (common.myInt(objDs.Tables[0].Rows[0]["FamilyMonthlyIncome"]) > 0)
                    //{
                    //    txtFamilyMonthlyIncome.Text = common.myStr(common.myInt(objDs.Tables[0].Rows[0]["FamilyMonthlyIncome"])); //Added  for child Trust    
                    //}
                    //else
                    //{
                    //    txtFamilyMonthlyIncome.Text = String.Empty;
                    //}
                    ViewState["UHID"] = txtRegNo.Text;
                    String strFullName = objDs.Tables[0].Rows[0]["FirstName"].ToString() + " " + objDs.Tables[0].Rows[0]["MiddleName"].ToString() + " " + objDs.Tables[0].Rows[0]["LastName"].ToString();
                    lnkOtherDetails.Visible = false;
                    lnkPayment.Visible = false;
                    lnkResponsibleParty.Visible = false;
                    if (common.myStr(Session["ModuleFlag"]) == "ER")
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    else
                    {
                        lnkPatientRelation.Visible = false;
                        lnkAttachDocument.Visible = false;
                        LblLabelname.Visible = false;
                    }
                    if (Request.QueryString["Category"] != null)
                    {
                        if (Request.QueryString["Category"].ToString() == "PopUp")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            if (common.myStr(Session["ModuleFlag"]) == "ER")
                            {
                                lnkbtnAppointment.Visible = false;
                                lnkbtnBilling.Visible = false;
                            }
                            else
                            {
                                lnkbtnAppointment.Visible = true;
                                lnkbtnBilling.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (common.myStr(Session["ModuleFlag"]) == "ER")
                        {
                            lnkbtnAppointment.Visible = false;
                            lnkbtnBilling.Visible = false;
                        }
                        else
                        {
                            lnkbtnAppointment.Visible = true;
                            lnkbtnBilling.Visible = false;
                        }
                    }
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        ShowImage();
                    }
                    else
                        PatientImage.ImageUrl = "~/Images/patientLeft.jpg";

                    lblMessage.Text = "";
                    btnSave.Text = "Update";
                    SetPermission(btnSave, "E", true);
                }
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
}