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
using System.Xml;
using System.Drawing;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

public partial class EMR_Assessment_ProvisionalDiagnosis : System.Web.UI.Page
{
    clsExceptionLog objException = new clsExceptionLog();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private bool RowSelStatus = false;


    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }

        if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            BindDiagnosisSearchCode();
            ViewState["_ID"] = 0;
            BindPatientProvisionalDiagnosis(0);
            BindFavoriteProvDiagnosis();
            dvConfirmCancelOptions.Visible = false;
            //BindPatientHiddenDetails();
            //if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
            //{
            //    btnSave.Visible = false;
            //}  comment by Himanshu  care issue 
            EnableSaving();
            SetPermission();
        }
    }
    protected void EnableSaving()
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        //BaseC.Dashboard objDash = new BaseC.Dashboard();
        //BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        try
        {


            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE").Equals(true) && common.myBool(Session["isEMRSuperUser"]).Equals(true))
            {
                tblProviderDetails.Visible = true;
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetDoctorList";
                APIRootClass.ProvisionalDiagnosis objRoot = new global::APIRootClass.ProvisionalDiagnosis();
                objRoot.HospitalId = common.myInt(Session["HospitalLocationId"]);
                objRoot.intSpecialisationId = 0;
                objRoot.FacilityId = Convert.ToInt16(Session["FacilityId"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                // ds = objCM.GetDoctorList(Convert.ToInt16(Session["HospitalLocationID"]), 0, Convert.ToInt16(Session["FacilityId"]));
                ddlRendringProvider.DataSource = ds;
                ddlRendringProvider.DataValueField = "DoctorID";
                ddlRendringProvider.DataTextField = "DoctorName";
                ddlRendringProvider.DataBind();
                ServiceURL = WebAPIAddress.ToString() + "api/Common/GetPatientEncounterDetails";
                APIRootClass.GetPatientEncounterDetails objRoot1 = new global::APIRootClass.GetPatientEncounterDetails();
                objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot1.FacilityId = Convert.ToInt16(Session["FacilityId"]);
                objRoot1.RegistrationId = common.myInt(Session["RegistrationId"]);
                client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                inputJson = (new JavaScriptSerializer()).Serialize(objRoot1);
                sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);
                //ds = objDash.getPatientEncounterDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(Session["RegistrationId"]));
                dv = ds.Tables[0].DefaultView;
                dv.RowFilter = " Id = " + common.myInt(Session["encounterid"]);
                dt = dv.ToTable();
                dtpChangeDate.DateInput.DateFormat = Application["OutputDateFormat"].ToString();
                dtpChangeDate.MinDate = common.myDate(dt.Rows[0]["EncounterDate"].ToString());
                if (common.myStr(dt.Rows[0]["OPIP"]).Equals("O"))
                {
                    dtpChangeDate.MaxDate = common.myDate(dt.Rows[0]["EncounterDate"].ToString()).AddDays(3);
                    ViewState["DischargeDate"] = common.myDate(dt.Rows[0]["DischargeDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                }
                else if (common.myStr(dt.Rows[0]["OPIP"]).Equals("E"))
                {
                    dtpChangeDate.MaxDate = common.myDate(dt.Rows[0]["DischDate"].ToString());
                    ViewState["DischargeDate"] = common.myDate(dt.Rows[0]["DischDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                }
                else
                {
                    dtpChangeDate.MaxDate = common.myDate(dt.Rows[0]["DischargeDate"].ToString());
                    ViewState["DischargeDate"] = common.myDate(dt.Rows[0]["DischargeDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                }
                ViewState["AdmissionDate"] = common.myDate(dt.Rows[0]["EncounterDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                lblRange.Text = "Date Range: Admission Date(" + common.myStr(ViewState["AdmissionDate"]) + ") and Discharge Date(" + common.myStr(ViewState["DischargeDate"]) + ")";
                RadTimeFrom.TimeView.Interval = new TimeSpan(0, 15, 0);
                RadTimeFrom.TimeView.StartTime = new TimeSpan(0, 0, 0);
                RadTimeFrom.TimeView.EndTime = new TimeSpan(23, 59, 59);
                ddlMinute.Items.Clear();
                for (int i = 0; i < 60; i++)
                {
                    if (i.ToString().Length == 1)
                        ddlMinute.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    else
                        ddlMinute.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
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
            dv.Dispose();
            dt.Dispose(); ;
            // objDash = null;
        }
    }
    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(string.Empty);
        try
        {
            btnSave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));

            ViewState["IsAllowEdit"] = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            ViewState["IsAllowCancel"] = ua1.CheckPermissionsForEMRModule("C", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            ua1.Dispose();
        }
    }

    //void BindPatientHiddenDetails()
    //{
    //    try
    //    {
    //        if (Session["PatientDetailString"] != null)
    //        {
    //            lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void BindPatientProvisionalDiagnosis(int Id)
    {

        //try
        //{
        //    DataSet ds;
        //    BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);

        //    ds = new DataSet();
        //    ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]));

        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        txtProvisionalDiagnosis.Text = ds.Tables[0].Rows[0]["ProvisionalDiagnosis"].ToString();
        //        ddlDiagnosisSearchCodes.SelectedValue = ds.Tables[0].Rows[0]["DiagnosisSearchId"].ToString();
        //        lblEncodedBy.Text = ds.Tables[0].Rows[0]["EncodedBy"].ToString();
        //        lblEncodedDate.Text = ds.Tables[0].Rows[0]["EncodedDate"].ToString();
        //        lblLastChangedBy.Text = ds.Tables[0].Rows[0]["LastChangedBy"].ToString();
        //        lblLastChangedDate.Text = ds.Tables[0].Rows[0]["LastChangedDate"].ToString();
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}

        DataSet ds = new DataSet();
        //BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {

            //api/EMRAPI/GetPatientProvisionalDiagnosis
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientProvisionalDiagnosis";
            APIRootClass.GetPatientProvisionalDiagnosis objRoot = new global::APIRootClass.GetPatientProvisionalDiagnosis();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.ProvisionalDiagnosisId = Id;
            objRoot.FromDate = "";
            objRoot.ToDate = "";
            objRoot.GroupingDate = "";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            // ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]), Id,"","","");

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvData.DataSource = ds;
                gvData.DataBind();
            }
            else
            {
                gvData.DataSource = string.Empty;
                gvData.DataBind();
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
            //objDiag = null;
        }
    }

    protected void BindDiagnosisSearchCode()
    {
        try
        {
            DataSet ds = new DataSet();

            // BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetProvisionalDiagnosisSearchCodes";
            APIRootClass.GetProvisionalDiagnosisSearchCodes objRoot = new global::APIRootClass.GetProvisionalDiagnosisSearchCodes();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.UserId = common.myInt(Session["UserID"]);
            objRoot.KeywordType = "DIAGNOSIS";
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            // ds = objDiag.GetProvisionalDiagnosisSearchCodes(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserID"]), "DIAGNOSIS");

            ddlDiagnosisSearchCodes.Items.Clear();
            ddlDiagnosisSearchCodes.DataSource = ds;
            ddlDiagnosisSearchCodes.DataValueField = "Id";
            ddlDiagnosisSearchCodes.DataTextField = "DiagnosisSearchCode";
            ddlDiagnosisSearchCodes.DataBind();
            ddlDiagnosisSearchCodes.Items.Insert(0, new RadComboBoxItem("Select", ""));
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);

            if (!isSaved())
            {
                return;
            }
            int intProviderId = common.myInt(ddlRendringProvider.SelectedValue);
            string Hourtime = string.Empty;
            if (RadTimeFrom.SelectedDate != null)
                Hourtime = RadTimeFrom.SelectedDate.Value.TimeOfDay.ToString();
            DateTime? dtChangeDate = null;
            if (dtpChangeDate.SelectedDate != null)
            {
                dtChangeDate = common.myDate(dtpChangeDate.SelectedDate);
                if (!string.IsNullOrEmpty(common.myStr(dtChangeDate)) && !string.IsNullOrEmpty(Hourtime))
                    dtChangeDate = Convert.ToDateTime(common.myStr((common.myStr(dtChangeDate).Split(' '))[0]) + " " + Hourtime);
            }

            if (tblProviderDetails.Visible == true)
            {
                if (ddlRendringProvider.SelectedIndex < 1)
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please fill Provider";
                    return;
                }
                if (string.IsNullOrEmpty(dtpChangeDate.SelectedDate.ToString()))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select change date";
                    return;
                }
                if (string.IsNullOrEmpty(RadTimeFrom.SelectedDate.ToString()))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Please select change Time";
                    return;
                }
                if (dtChangeDate > common.myDate(ViewState["DischargeDate"]) || dtChangeDate < common.myDate(ViewState["AdmissionDate"]))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Change Date should be between Admission date and Discharge date.";
                    return;
                }
            }
            string strsave = "";




            /*string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientProvisionalDiagnosis";
            APIRootClass.saveDiagnosisInputs objRoot = new global::APIRootClass.saveDiagnosisInputs();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.ProvisionalDiagnosis = common.myStr(txtProvisionalDiagnosis.Text).Trim();
            objRoot.DiagnosisSearchId = common.myInt(ddlDiagnosisSearchCodes.SelectedValue);
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.ProvisionalDiagnosisId = common.myInt(ViewState["_ID"]);
            objRoot.ProviderId = intProviderId;
            objRoot.ChangeDate = dtChangeDate;
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            strsave = client.UploadString(ServiceURL, inputJson);
            strsave = JsonConvert.DeserializeObject<string>(strsave);
            //objDs = JsonConvert.DeserializeObject<DataSet>(sValue);
            */

            BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
            //strsave = objDiag.EMRSavePatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
            //                            common.myStr(txtProvisionalDiagnosis.Text).Trim(), common.myInt(ddlDiagnosisSearchCodes.SelectedValue), common.myInt(Session["UserId"]), common.myInt(ViewState["_ID"]),
            //                            intProviderId, dtChangeDate, "", false, false,
            //                            false, false, false);

            strsave = objDiag.EMRSavePatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                       common.myStr(txtProvisionalDiagnosis.Text).Trim(), common.myInt(ddlDiagnosisSearchCodes.SelectedValue), common.myInt(Session["UserId"]), common.myInt(ViewState["_ID"]),
                                       intProviderId, dtChangeDate, "", false, chkQuery.Checked,
                                       chkIsFinalDiagnosis.Checked, false, false);


            if (strsave.ToUpper().Contains("UPDATED") || strsave.ToUpper().Contains("SAVED"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strsave;
                ViewState["_ID"] = 0;
                BindPatientProvisionalDiagnosis(0);

                txtProvisionalDiagnosis.Text = string.Empty;
                dtpChangeDate.SelectedDate = null;
                ddlRendringProvider.SelectedValue = null;
                ddlDiagnosisSearchCodes.SelectedIndex = 0;

                if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP")
                         || common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                }
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = strsave;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void btnGetDiagnosisSearchCodes_Click(object sender, EventArgs e)
    {
        BindDiagnosisSearchCode();
    }

    protected void ibtnDiagnosisSearchCode_Click(object sender, ImageClickEventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Assessment/Status.aspx?Source=pd";
        RadWindowForNew.Height = 450;
        RadWindowForNew.Width = 550;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;

        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleStatusbar = false;
    }

    private bool isSaved()
    {
        bool isSave = true;
        string strmsg = "";

        if (common.myInt(Session["RegistrationId"]) == 0 || common.myInt(Session["EncounterId"]) == 0)
        {
            strmsg = "Patient details not found !";
            isSave = false;
        }
        else if (common.myLen(txtProvisionalDiagnosis.Text).Equals(0))
        {
            strmsg = "Please enter provisional diagnosis !";
            isSave = false;
        }
        //else if (common.myInt(ddlDiagnosisSearchCodes.SelectedValue) == 0)
        //{
        //    strmsg = "Please select diagnosis search keyword !";
        //    isSave = false;
        //}

        lblMessage.Text = strmsg;
        return isSave;
    }

    protected void gvData_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        BindPatientProvisionalDiagnosis(0);
    }
    protected void gvData_OnItemCommand(object sender, GridCommandEventArgs e)
    {
        DataSet ds = new DataSet();
        // BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            lblMessage.Text = string.Empty;
            HiddenField hdnProvisionalDiagnosisID = (HiddenField)e.Item.FindControl("hdnProvisionalDiagnosisID");
            ViewState["_ID"] = common.myInt(hdnProvisionalDiagnosisID.Value);
            if (e.CommandName.Equals("Select"))
            {
                if (!common.myBool(ViewState["IsAllowEdit"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to edit !", this.Page);
                    return;
                }

                RowSelStatus = true;

                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientProvisionalDiagnosis";
                APIRootClass.GetPatientProvisionalDiagnosis objRoot = new global::APIRootClass.GetPatientProvisionalDiagnosis();
                objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
                objRoot.EncounterId = common.myInt(Session["EncounterId"]);
                objRoot.UserId = common.myInt(Session["UserID"]);
                objRoot.ProvisionalDiagnosisId = common.myInt(ViewState["_ID"]);
                objRoot.FromDate = "";
                objRoot.ToDate = "";
                objRoot.GroupingDate = "";
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);

                // ds = objDiag.GetPatientProvisionalDiagnosis(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(Session["UserID"]), common.myInt(ViewState["_ID"]),"","","");

                if (ds.Tables[0].Rows.Count > 0)
                {

                    Label lblDate = (Label)e.Item.FindControl("lblEncodedDate");
                    HiddenField hdnProviderId = (HiddenField)e.Item.FindControl("hdnProviderId");
                    RadTimeFrom.SelectedDate = Convert.ToDateTime(Convert.ToDateTime(common.myStr(lblDate.Text.Substring(common.myLen(lblDate.Text) - 7).Trim())).ToShortTimeString());
                    dtpChangeDate.SelectedDate = common.myDate(lblDate.Text);
                    if (common.myInt(hdnProviderId.Value) > 0)
                    {
                        ddlRendringProvider.SelectedIndex = ddlRendringProvider.Items.IndexOf(ddlRendringProvider.Items.FindItemByValue(common.myStr(hdnProviderId.Value)));
                    }

                    txtProvisionalDiagnosis.Text = common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]);
                    ddlDiagnosisSearchCodes.SelectedValue = ds.Tables[0].Rows[0]["DiagnosisSearchId"].ToString();
                    chkQuery.Checked = common.myBool(ds.Tables[0].Rows[0]["Provisional"]);
                    chkIsFinalDiagnosis.Checked = common.myBool(ds.Tables[0].Rows[0]["Final"]);
                    //lblEncodedBy.Text = ds.Tables[0].Rows[0]["EncodedBy"].ToString();
                    //lblEncodedDate.Text = ds.Tables[0].Rows[0]["EncodedDate"].ToString();
                    //lblLastChangedBy.Text = ds.Tables[0].Rows[0]["LastChangedBy"].ToString();
                    //lblLastChangedDate.Text = ds.Tables[0].Rows[0]["LastChangedDate"].ToString();
                }

                //ddlStatus.Enabled = true;

                //objBb = new BaseC.clsBb(sConString);
                //ds = objBb.GetBloodGroupMaster(common.myInt(ViewState["_ID"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 0);

                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    txtBloodGroup.Text = common.myStr(ds.Tables[0].Rows[0]["BloodGroup"]);
                //    txtBloodGroupDescription.Text = common.myStr(ds.Tables[0].Rows[0]["BloodGroupDescription"]);
                //    ddlRhType.SelectedIndex = ddlRhType.Items.IndexOf(ddlRhType.Items.FindItemByValue(common.myStr(ds.Tables[0].Rows[0]["RhType"])));
                //    ddlStatus.SelectedValue = common.myBool(ds.Tables[0].Rows[0]["Active"]) == true ? "1" : "0";
                //}
            }
            else if (e.CommandName == "Delete")
            {
                if (!common.myBool(ViewState["IsAllowCancel"]))
                {
                    Alert.ShowAjaxMsg("Not authorized to cancel !", this.Page);
                    return;
                }

                //GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                //string strId = row.Cells[1].Text;
                //ViewState["strId"] = strId;

                //if (rblShowNote.SelectedIndex == -1)
                //{
                //    Alert.ShowAjaxMsg("Would you like to show update data in Notes. Please Select Show in Note Option Yes or No ? ", this.Page);
                //    return;

                //}

                if (common.myInt(ViewState["_ID"]) > 0)
                {
                    dvConfirmCancelOptions.Visible = true;
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
            ///objDiag = null;
        }
    }
    protected void gvData_PreRender(object sender, EventArgs e)
    {
        if (RowSelStatus == false)
        {
            BindPatientProvisionalDiagnosis(0);
        }
    }
    protected void gvData_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridPagerItem)
            {
                GridPagerItem pager = (GridPagerItem)e.Item;
                Label lbl = (Label)pager.FindControl("ChangePageSizeLabel");
                lbl.Visible = false;

                RadComboBox combo = (RadComboBox)pager.FindControl("PageSizeComboBox");
                combo.Visible = false;

            }
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;

                if (item != null)
                {
                    ImageButton btnDelete = (ImageButton)item["BtnCol"].Controls[0];
                    if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
                    {
                        btnDelete.Visible = false;
                    }
                    HiddenField hdnEncodedById = (HiddenField)e.Item.FindControl("hdnEncodedById");
                    LinkButton lnkEdit = (LinkButton)item["EditData"].Controls[0];

                    if (common.myInt(hdnEncodedById.Value) > 0)
                    {
                        if (!common.myInt(hdnEncodedById.Value).Equals(common.myInt(Session["UserId"])))
                        {
                            lnkEdit.Visible = false;
                            btnDelete.Visible = false;
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
    protected void ButtonOk_OnClick(object sender, EventArgs e)
    {
        // BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        try
        {
            if (common.myInt(ViewState["_ID"]) > 0)
            {


                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeletePatientProvisionalDiagnosis";
                APIRootClass.DeletePatientProvisionalDiagnosis objRoot = new global::APIRootClass.DeletePatientProvisionalDiagnosis();
                objRoot.ProvisionalDiagnosisId = common.myInt(ViewState["_ID"]);
                objRoot.UserId = common.myInt(Session["UserID"]);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                int i = common.myInt(sValue);

                // int i = objDiag.DeletePatientProvisionalDiagnosis(common.myInt(ViewState["_ID"]), common.myInt(Session["UserID"]));
                if (i == 0)
                {

                    lblMessage.Text = "Provisional Diagnosis deleted.";
                }
            }
            else
            {
                Alert.ShowAjaxMsg("Select a Provisional Diagnosis", this);
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
            // objDiag = null;
        }
        //DataSet ds = new DataSet();

        ////        objbc2.Canceltodayproblem(common.myInt(ViewState["strId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), common.myInt(Session["UserID"]), common.myInt(rblShowNote.SelectedItem.Value));
        //objbc2.Canceltodayproblem(common.myInt(ViewState["strId"]), common.myInt(Session["RegistrationID"]), common.myInt(Session["encounterid"]), common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(ViewState["PageId"]), common.myInt(Session["UserID"]), common.myInt(0));
        //RetrievePatientProblemsDetail();
        dvConfirmCancelOptions.Visible = false;
        BindPatientProvisionalDiagnosis(0);
        ViewState["_ID"] = 0;
    }
    protected void ButtonCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirmCancelOptions.Visible = false;
        //ViewState["_ID"] = 0;
        //BindPatientProvisionalDiagnosis(0);
    }

    protected void gvFav_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "FAVLIST")
        {
            GridViewRow grd = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            LinkButton lnkbtn = (LinkButton)grd.FindControl("lnkFavName");
            txtProvisionalDiagnosis.Text = ((common.myLen(txtProvisionalDiagnosis.Text) > 0) ? txtProvisionalDiagnosis.Text + ", " : txtProvisionalDiagnosis.Text) + lnkbtn.Text;
        }
        else if (e.CommandName == "Del")
        {
            // BaseC.DiagnosisDA ObjDiagnosis = new BaseC.DiagnosisDA(sConString);
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            HiddenField hdnFavId = (HiddenField)row.FindControl("hdnFavId");

            if (common.myInt(hdnFavId.Value) > 0)
            {
                string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/DeleteFavouriteProvDiagnosis";
                APIRootClass.DeletePatientProvisionalDiagnosis objRoot = new global::APIRootClass.DeletePatientProvisionalDiagnosis();
                objRoot.idoctorId = common.myInt(Session["DoctorID"]);
                objRoot.id = common.myInt(hdnFavId.Value);
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                string sValue = client.UploadString(ServiceURL, inputJson);
                sValue = JsonConvert.DeserializeObject<string>(sValue);
                // ObjDiagnosis.DeleteFavouriteProvDiagnosis(common.myInt(Session["DoctorID"]), common.myInt(hdnFavId.Value));
                lblMessage.Text = "Diagnosis deleted from your favorite list";
            }
            else
            {
                Alert.ShowAjaxMsg("Select Diagnosis to delete from favorite list", Page);
            }
            BindProvFavouriteDiagnosis("");

        }

    }

    public void BindFavoriteProvDiagnosis()
    {
        DataTable objdt = new DataTable();
        objdt = BindProvFavouriteDiagnosis("");
        gvFav.DataSource = objdt;
        gvFav.DataBind();
        ViewState["FavList"] = objdt;
    }

    protected DataTable BindProvFavouriteDiagnosis(string txt)
    {
        DataTable DT = new DataTable();
        try
        {
            // BaseC.DiagnosisDA objbc2 = new BaseC.DiagnosisDA(sConString);
            string strSearchCriteria = string.Empty;

            strSearchCriteria = "%" + txt + "%";

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/BindFavouriteProvDiagnosis";
            APIRootClass.ProvisionalDiagnosis objRoot = new global::APIRootClass.ProvisionalDiagnosis();
            objRoot.strSearchCriteria = strSearchCriteria;
            objRoot.DoctorID = common.myInt(Session["DoctorID"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            //DataSet objDs = (DataSet)objbc2.BindFavouriteProblems(strSearchCriteria, common.myInt(Session["DoctorID"]));
            //DataSet objDs = (DataSet)objbc2.BindFavouriteProvDiagnosis(strSearchCriteria, common.myInt(Session["DoctorID"]));

            if (objDs.Tables[0].Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                //dt = objDs.Tables[0];

                //if (CallFrom == "")
                //{
                //    btnAddToFavourite.Visible = false;
                //    btnRemovefromFavorites.Visible = false;
                //}
                DT = objDs.Tables[0];
                //yogesh
                ViewState["favDiagnosis"] = DT;
                gvFav.DataSource = objDs.Tables[0];
                gvFav.DataBind();
            }
            else
            {
                DT = BindBlankGrid();
                gvFav.DataSource = DT;
                gvFav.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return DT;
    }

    private DataTable BindBlankGrid()
    {
        DataTable dT = new DataTable();
        try
        {
            dT.Columns.Add("ID");
            dT.Columns.Add("Description");
            for (int i = 0; i < 1; i++)
            {
                DataRow dr = dT.NewRow();
                dr["ID"] = 0;
                dr["Description"] = "";
                dT.Rows.Add(dr);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dT;
    }

    protected void btnAddToFavourite_Click(object sender, EventArgs e)
    {
        try
        {
            // BaseC.DiagnosisDA objbc2 = new BaseC.DiagnosisDA(sConString);

            if (ViewState["FavList"] != "")
            {
                DataTable dt = (DataTable)ViewState["FavList"];
                DataView dv = dt.DefaultView;
                if (dt.Rows.Count > 0)
                {
                    if (txtProvisionalDiagnosis.Text != "")
                    {
                        //dv.RowFilter = "Description=" + common.myStr(txtProvisionalDiagnosis.Text);
                        //if (dv.Count > 0)
                        //{
                        //    Alert.ShowAjaxMsg("Diagnosis already exists in Favorites", this);
                        //    return;
                        //}
                    }
                }
            }
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/SaveFavouriteProvDiagnosis";
            APIRootClass.ProvisionalDiagnosis objRoot = new global::APIRootClass.ProvisionalDiagnosis();
            objRoot.DoctorID = common.myInt(Session["DoctorID"]);
            objRoot.DiagnosisDescription = common.myStr(txtProvisionalDiagnosis.Text).Trim();
            objRoot.userid = common.myInt(Session["UserID"]);
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string strmsg = client.UploadString(ServiceURL, inputJson);
            strmsg = JsonConvert.DeserializeObject<string>(strmsg);
            //objDs = JsonConvert.DeserializeObject<DataSet>(strmsg);

            // string strmsg = objbc2.SaveFavouriteProvDiagnosis(common.myInt(Session["DoctorID"]), common.myStr(txtProvisionalDiagnosis.Text).Trim(), common.myInt(Session["UserID"]));

            lblMessage.Text = strmsg;
            txtProvisionalDiagnosis.Text = "";
            BindProvFavouriteDiagnosis("");

            //}
            //else
            //{
            //    Alert.ShowAjaxMsg("Select a Problem to Add in Favorites", this);
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void ddlMinute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        StringBuilder sb = new StringBuilder();

        try
        {
            if (RadTimeFrom.SelectedDate != null)
            {
                sb.Append(RadTimeFrom.SelectedDate.Value.ToString());
                sb.Remove(RadTimeFrom.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
                sb.Insert(RadTimeFrom.SelectedDate.Value.ToString().IndexOf(":") + 1, ddlMinute.Text);
                RadTimeFrom.SelectedDate = Convert.ToDateTime(sb.ToString());
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please select time.";
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
            sb = null;
        }
    }


    //yogesh 08/07/2022

    protected void txtSearchFavriouteDiag_TextChanged(object sender, EventArgs e)
    {
        SearchFavrioute(txtSearchFavriouteDiag.Text.Trim());
    }


    //yogesh
    protected void SearchFavrioute(string s)
    {
        DataTable dt = (DataTable)ViewState["favDiagnosis"];
        DataTable dtSearch = new DataTable();
        DataRow workRow;
        dtSearch.Columns.Add("Id");
        dtSearch.Columns.Add("Description");
        for (int i = 0; i < dt.Rows.Count; i++)
        {

            string check = dt.Rows[i]["Description"].ToString().ToUpper();
            if (check.Contains(s.ToUpper()))
            {
                workRow = dtSearch.NewRow();
                workRow[0] = dt.Rows[i]["Id"].ToString();
                workRow[1] = dt.Rows[i]["Description"].ToString();
                dtSearch.Rows.Add(workRow);

            }
        }


        if (dtSearch.Rows.Count > 0)
        {

            gvFav.DataSource = dtSearch;
            gvFav.DataBind();
        }
        else
        {
            DataTable DT = BindBlankGrid();
            gvFav.DataSource = DT;
            gvFav.DataBind();
        }


    }



    protected void btnProceedFavourite_Click(object sender, ImageClickEventArgs e)
    {

    }



    protected void btnSearchFavDiag_Click(object sender, EventArgs e)
    {
        SearchFavrioute(txtSearchFavriouteDiag.Text.Trim());
    }
}
