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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using Telerik.Web.UI;
using Telerik.Web.UI.Calendar;
using System.Net;
using System.IO;
using System.Drawing;

public partial class DoseDetails : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int indentid = common.myInt(Request.QueryString["indentid"]);
            int ItemId = common.myInt(Request.QueryString["itemid"]);
            string dateTime = common.myStr(Request.QueryString["dateTime"]);
            int sEncounterId = common.myInt(Request.QueryString["EncId"]);
            int sRegistrationNo = common.myInt(Request.QueryString["RegNo"]);
            DateTime dt = Convert.ToDateTime(dateTime);
            string time1 = dt.ToString("yyyy-MM-dd HH:mm:00:000");

            //string format = "HH:mm";    // Use this format
            BindPatientHiddenDetails(sRegistrationNo);
            lblPrescriptionLabel.Text = common.myStr(Session["PrescriptionDetail"]);
            lblTitle.Text = common.myStr(Request.QueryString["DTN"].Replace("and", "&")) + " : ";
            //dtpScheduleDate.DateInput.DateFormat = common.myStr(Application["OutputDateFormat"]);
            lblScheduleDateTime.Text = Convert.ToDateTime(dateTime).ToString("dd/MM/yyyy hh:mm tt");
            Dose();
            Session["PrescriptionDetail"] = null;
            BindControls();
            ViewState["ID"] = 0;
            hdnBeforeTime.Value = "0";

            int timeSlot = 1;// common.myInt(Request.QueryString["timeSlot"].ToString());
            DateTime dttimeSlotValidation;
            if (common.myStr(Request.QueryString["DT"]).ToUpper() == "STAT")// DT - Drug Type
            {
                dttimeSlotValidation = Convert.ToDateTime(dateTime);//administration time cannot take before schedule time in case of STAT
            }
            else
            {
                if (common.myStr(Request.QueryString["DT"]).ToUpper() == "SINFU")
                {
                    dttimeSlotValidation = Convert.ToDateTime(dateTime); // infusion
                }
                else
                {
                    dttimeSlotValidation = Convert.ToDateTime(dateTime).AddMinutes(-30); //AddHours(-1);//validation before one houre
                }
            }
            if (timeSlot == 1 || timeSlot == 2 || timeSlot == 3 || timeSlot == 4 || timeSlot == 5
                || timeSlot == 6 || timeSlot == 7 || timeSlot == 8 || timeSlot == 9 || timeSlot == 10 || timeSlot == 11)
            {
                dtpNormalTime.TimeView.Interval = new TimeSpan(0, 15, 0);
                dtpNormalTime.TimeView.StartTime = new TimeSpan(dttimeSlotValidation.Hour, dttimeSlotValidation.Minute, dttimeSlotValidation.Second);
                dtpIVINFUMedHangedTime.TimeView.Interval = new TimeSpan(0, 15, 0);
                dtpIVINFUCompletedTime.TimeView.Interval = new TimeSpan(0, 15, 0);
                dtpIVINFUMedHangedTime.TimeView.StartTime = new TimeSpan(dttimeSlotValidation.Hour, dttimeSlotValidation.Minute, dttimeSlotValidation.Second);
                dtpIVINFUCompletedTime.TimeView.StartTime = new TimeSpan(dttimeSlotValidation.Hour, dttimeSlotValidation.Minute, dttimeSlotValidation.Second);

                dtpNormalDate.MinDate = Convert.ToDateTime(common.myDate(Request.QueryString["SD"]));
                dtpNormalDate.MaxDate = (Convert.ToDateTime(common.myDate(Request.QueryString["ED"])) < DateTime.Now) ? DateTime.Now : Convert.ToDateTime(common.myDate(Request.QueryString["ED"]));

                dtpIVINFUCompletedDate.MinDate = Convert.ToDateTime(common.myDate(Request.QueryString["SD"]));
                //dtpIVINFUCompletedDate.MaxDate = Convert.ToDateTime(common.myDate(Request.QueryString["ED"]));
                dtpIVINFUMedHangedDate.MinDate = Convert.ToDateTime(common.myDate(Request.QueryString["SD"]));
                dtpIVINFUMedHangedDate.MaxDate = (Convert.ToDateTime(common.myDate(Request.QueryString["ED"])) < DateTime.Now) ? DateTime.Now : Convert.ToDateTime(common.myDate(Request.QueryString["ED"]));
                try
                {
                    dtpIVINFUMedHangedDate.SelectedDate = DateTime.Now;
                }
                catch
                {
                }

                try
                {
                    dtpIVINFUMedHangedTime.SelectedDate = DateTime.Now;//for time
                }
                catch
                {
                }
            }
            else
            {
                dtpNormalTime.TimeView.Interval = new TimeSpan(0, timeSlot, 0);
                dtpIVINFUMedHangedTime.TimeView.Interval = new TimeSpan(0, timeSlot, 0);
                dtpIVINFUCompletedTime.TimeView.Interval = new TimeSpan(0, timeSlot, 0);
                dtpNormalDate.MinDate = Convert.ToDateTime(common.myDate(Request.QueryString["SD"]));
                dtpNormalDate.MaxDate = (Convert.ToDateTime(common.myDate(Request.QueryString["ED"])) < DateTime.Now) ? DateTime.Now : Convert.ToDateTime(common.myDate(Request.QueryString["ED"]));

                dtpIVINFUCompletedDate.MinDate = Convert.ToDateTime(common.myDate(Request.QueryString["SD"]));
                // dtpIVINFUCompletedDate.MaxDate = Convert.ToDateTime(common.myDate(Request.QueryString["ED"]));

                dtpIVINFUMedHangedDate.MinDate = Convert.ToDateTime(common.myDate(Request.QueryString["SD"]));
                dtpIVINFUMedHangedDate.MaxDate = (Convert.ToDateTime(common.myDate(Request.QueryString["ED"])) < DateTime.Now) ? DateTime.Now : Convert.ToDateTime(common.myDate(Request.QueryString["ED"]));
                try
                {
                    dtpIVINFUMedHangedDate.SelectedDate = DateTime.Now;
                }
                catch
                {
                }

                try
                {
                    dtpIVINFUMedHangedTime.SelectedDate = DateTime.Now;//for time
                }
                catch
                {
                }
            }

            BindDoseDetails(indentid, ItemId, time1, dt);
        }
    }
    private void Dose()
    {
        BaseC.ICM objEMR = new BaseC.ICM(sConString);
        DataSet ds = objEMR.GetDoseOfSlotWise(common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["EncId"]),
                                common.myInt(Request.QueryString["itemid"]),
                                common.myInt(Request.QueryString["indentid"]), common.myInt(Request.QueryString["frequencyid"]),
                                0, common.myInt(Request.QueryString["FrequencyDetailId"]), Convert.ToDateTime(Request.QueryString["dateTime"]).ToString("yyyy/MM/dd"));

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblDose.Text = common.myStr(ds.Tables[0].Rows[0]["Dose"]);
        }
        ds.Dispose();

    }
    private void BindDoseDetails(int indentid, int ItemId, string time1, DateTime dt)
    {
        DataSet resultStatus = new DataSet();
        BaseC.ICM icm = new BaseC.ICM(sConString);
        resultStatus = icm.GetDrugAdministerDetails(indentid, ItemId, time1, Session["FacilityId"].ToString(), common.myInt(Request.QueryString["FrequencyDetailId"]), common.myInt(Request.QueryString["EncId"]));
        hdnBeforeTime.Value = "0";

        if (common.myStr(Request.QueryString["DT"]) == "SINFU")
        {
            tblIVInfusion.Visible = true;
            tblNormailYes.Visible = false;
            // if(resultStatus.Tables[0].Rows.Count>0)
            txtIVINFInstruction.Text = common.myStr(Request.QueryString["Inst"]);
            BindUnit();
        }
        else
        {
            tblNormailYes.Visible = true;
            tblIVInfusion.Visible = false;
            // if (resultStatus.Tables[0].Rows.Count > 0)
            txtInstructions.Text = common.myStr(Request.QueryString["Inst"]);
        }
        if (resultStatus.Tables.Count > 0)
        {
            if (resultStatus.Tables[1].Rows.Count > 0)
            {
                lblInfusionOrder.Text = common.myStr(resultStatus.Tables[1].Rows[0]["InfusionOrder"]);
            }
            if (resultStatus.Tables[0].Rows.Count > 0 && common.myStr(Request.QueryString["PrnNew"]) != "Y")
            {
                if (resultStatus.Tables[1].Rows.Count > 0)
                {
                    lblInfusionOrder.Text = common.myStr(resultStatus.Tables[1].Rows[0]["InfusionOrder"]);
                }

                trIVINFVolume.Disabled = false;
                ddlYes.Enabled = false;
                ddlYes.SelectedValue = resultStatus.Tables[0].Rows[0]["IsMedicationAdminister"].ToString();
                BindControls();
                txtVolumnHanged.Enabled = false;
                dtpIVINFUMedHangedDate.Enabled = false;
                dtpIVINFUMedHangedTime.Enabled = false;
                ViewState["ID"] = resultStatus.Tables[0].Rows[0]["ID"].ToString();

                ddlVolumeUnit.SelectedValue = resultStatus.Tables[0].Rows[0]["VolumeUnit"].ToString();
                ddlVolInfusionUnit.SelectedValue = resultStatus.Tables[0].Rows[0]["VolumeInfusionUnit"].ToString();
                ddlvolDiscardUnit.SelectedValue = resultStatus.Tables[0].Rows[0]["VolumeDiscardUnit"].ToString();

                if (common.myStr(resultStatus.Tables[0].Rows[0]["MedicationHangedDate"]) != "")
                {
                    dtpIVINFUMedHangedDate.SelectedDate = common.myDate(resultStatus.Tables[0].Rows[0]["MedicationHangedDate"]);
                }
                if (common.myStr(resultStatus.Tables[0].Rows[0]["MedicationHangedTime"]) != "")
                {
                    //  dtpIVINFUMedHangedTime.SelectedDate = common.myDate(resultStatus.Tables[0].Rows[0]["MedicationHangedTime"]);
                    dtpIVINFUMedHangedTime.SelectedDate = Convert.ToDateTime(resultStatus.Tables[0].Rows[0]["MedicationHangedTime"]);
                }

                if (common.myStr(resultStatus.Tables[0].Rows[0]["MedCompletedDiscontinuedDate"]) != "")
                {
                    dtpIVINFUCompletedDate.SelectedDate = common.myDate(resultStatus.Tables[0].Rows[0]["MedCompletedDiscontinuedDate"]);
                }
                if (common.myStr(resultStatus.Tables[0].Rows[0]["MedCompletedDiscontinuedTime"]) != "")
                {
                    dtpIVINFUCompletedTime.SelectedDate = common.myDate(resultStatus.Tables[0].Rows[0]["MedCompletedDiscontinuedTime"]);
                }
                txtVolumnHanged.Text = resultStatus.Tables[0].Rows[0]["VolumnHanged"].ToString();
                txtVolumnInfusion.Text = resultStatus.Tables[0].Rows[0]["VolumnInfusion"].ToString();
                txtVolumeDiscard.Text = resultStatus.Tables[0].Rows[0]["VolumeDiscard"].ToString();

                if (common.myBool(resultStatus.Tables[0].Rows[0]["IsInfusion"]) == true)
                {
                    txtIVINFUReasonForDelay.Text = resultStatus.Tables[0].Rows[0]["DelayReason"].ToString();
                    txtIVINFReasonForNotAdministered.Text = resultStatus.Tables[0].Rows[0]["NotAdministeredReason"].ToString();
                    txtIVINFRemarks.Text = common.myStr(resultStatus.Tables[0].Rows[0]["UserRemarks"]);
                }
                else
                {
                    txtReasonForDelay.Text = resultStatus.Tables[0].Rows[0]["DelayReason"].ToString();
                    txtNormalReasonForNotAdministered.Text = resultStatus.Tables[0].Rows[0]["NotAdministeredReason"].ToString();
                    //txtRemark.Text = resultStatus.Tables[0].Rows[0]["AdministrationRemarks"].ToString();
                    txtHR.Text = resultStatus.Tables[0].Rows[0]["HR"].ToString();
                    txtRR.Text = resultStatus.Tables[0].Rows[0]["RR"].ToString();
                    txtDBP.Text = resultStatus.Tables[0].Rows[0]["DBP"].ToString();
                    txtSBP.Text = resultStatus.Tables[0].Rows[0]["SBP"].ToString();
                    txtNormalRemarks.Text = common.myStr(resultStatus.Tables[0].Rows[0]["UserRemarks"]);
                }
                if (common.myStr(resultStatus.Tables[0].Rows[0]["DrugAdministerDate"]) != "")
                {
                    dtpNormalDate.Enabled = false;
                    dtpNormalDate.SelectedDate = common.myDate(resultStatus.Tables[0].Rows[0]["DrugAdministerDate"]);
                }
                if (common.myStr(resultStatus.Tables[0].Rows[0]["DrugAdministerTime"]) != "")
                {
                    dtpNormalTime.Enabled = false;
                    dtpNormalTime.SelectedDate = common.myDate(resultStatus.Tables[0].Rows[0]["DrugAdministerTime"]);
                }
                //  ddlYes.SelectedValue = resultStatus.Tables[0].Rows[0]["IsMedicationAdminister"].ToString();           

                // chkNotAdministered.Checked = common.myStr(resultStatus.Tables[0].Rows[0]["IsMedicationAdminister"])=="Y"?false:true;

            }
            else
            {
                BindControls();
                //if (dt > DateTime.Now)
                //{
                //    hdnBeforeTime.Value = "1";
                //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                //    lblMessage.Text = "You are administrating before schedule... Remark is mandotory";
                //    return;
                //}
            }
        }
        else
        {
            BindControls();
            //if (dt > DateTime.Now)
            //{
            //    hdnBeforeTime.Value = "1";
            //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //    lblMessage.Text = "You are administrating before schedule... Remark is mandotory";
            //    return;
            //}
        }
        resultStatus.Dispose();
        icm = null;
    }
    void BindPatientHiddenDetails(int RegistrationNo)
    {
        try
        {
            BaseC.Patient bC = new BaseC.Patient(sConString);
            BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);

            if (RegistrationNo > 0)
            {
                int HospId = common.myInt(Session["HospitalLocationID"]);
                int FacilityId = common.myInt(Session["FacilityId"]);
                int EncodedBy = common.myInt(Session["UserId"]);
                DataSet ds = new DataSet();

                BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
                ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, 0, RegistrationNo, EncodedBy, 0, "");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                        // lblDob.Text = common.myStr(dr["DOB"]);
                        lblMobile.Text = common.myStr(dr["MobileNo"]);
                        lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                        lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                        lblRegNo.Text = common.myStr(dr["RegistrationNo"]);
                    }
                    else
                    {
                        DataSet ds1 = new DataSet();
                        ds1 = bC.getPatientDetails(HospId, FacilityId, 0, RegistrationNo, common.myInt(Request.QueryString["EncId"]), EncodedBy);
                        if (ds1.Tables.Count > 0)
                        {
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                DataRow dr = ds1.Tables[0].Rows[0];
                                lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
                                lblMobile.Text = common.myStr(dr["MobileNo"]);
                                lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
                                lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
                                lblRegNo.Text = common.myStr(dr["RegistrationNo"]);
                            }
                        }
                        else
                        {
                            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                            lblMessage.Text = "Patient not found !";
                            return;
                        }
                    }
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    lblMessage.Text = "Patient not found !";
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
    private bool CheckPassword(string UserName, string sPassword)
    {
        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        try
        {
            BaseC.User objUser = new BaseC.User(sConString);
            hdnIsValidPassword.Value = "0";
            DataSet ds = new DataSet();
            ds = objUser.ValidateUserName(UserName, sPassword, 0);
            if (ds != null)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "UserId>0";
                if (common.myInt(dv.ToTable().Rows.Count) > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        hdnIsValidPassword.Value = "1";
                    }
                }
            }
            if (hdnIsValidPassword.Value == "0")
            {
                lblMessage.Text = "Invalid Password !";
                txtNormalUserPassword.Text = "";
                txtNormalUserPassword.Focus();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
        }
        return hdnIsValidPassword.Value == "1" ? true : false;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        BaseC.ICM icm = new BaseC.ICM(sConString);
        string Id = common.myStr(ViewState["ID"]);
        string EncounterId = Request.QueryString["EncId"].ToString();
        string IndentId = Request.QueryString["indentid"].ToString();
        string ItemId = Request.QueryString["itemid"].ToString();
        string FrequencyId = Request.QueryString["frequencyid"].ToString();
        string dateTime = Request.QueryString["dateTime"].ToString();
        string timeSlot = Request.QueryString["dateTime"].ToString();
        string DoseType = Request.QueryString["DT"].ToString();
        // DateTime dt = DateTime.Parse(Request.QueryString["dateTime"].ToString());
        DateTime dt = Convert.ToDateTime(Request.QueryString["dateTime"]);//.ToString("yyyy-MM-dd HH:mm:00:000"); //Convert.ToDateTime(Request.QueryString["dateTime"].ToString());

        string DrugScheduleDate = dt.ToString("MM/dd/yyyy");

        string NewDrugScheduleTime = dt.ToString("t");
        string DrugScheduleTime = dt.ToString("HH:mm");

        string IVINFUMedHangedDate = "";
        string IVINFUMedHangedTime = "";
        string IVINFUCompletedDate = "";
        string IVINFUCompletedTime = "";
        string NormalTime = "";
        string NormalDate = "";
        string NormalTimeNew = "";

        try
        {
            if (ddlYes.SelectedValue == "")
            {
                Alert.ShowAjaxMsg("Please select for Administration Status.", Page);
                return;
            }

            if (DoseType != "SINFU")//IV Infusion
            {
                NormalDate = Convert.ToDateTime(dtpNormalDate.SelectedDate).ToString("MM/dd/yyyy");
                NormalTime = Convert.ToDateTime(dtpNormalTime.SelectedDate).ToString("HH:mm:00"); //common.myStr(RadTimeFrom.SelectedDate);
                NormalTimeNew = Convert.ToDateTime(dtpNormalTime.SelectedDate).ToString("t");
            }
            else
            {
                NormalDate = Convert.ToDateTime(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy");
                NormalTime = Convert.ToDateTime(dtpIVINFUMedHangedTime.SelectedDate).ToString("HH:mm:00"); //common.myStr(RadTimeFrom.SelectedDate);
                NormalTimeNew = Convert.ToDateTime(dtpIVINFUMedHangedTime.SelectedDate).ToString("t");
            }

            if (DoseType == "SINFU")//IV Infusion
            {
                if (ddlYes.SelectedValue == "Y")
                {
                    if (common.myDbl(txtVolumnHanged.Text) == 0)
                    {
                        Alert.ShowAjaxMsg("Please enter Volume !!", Page);
                        return;
                    }
                    if (dtpIVINFUMedHangedDate.SelectedDate == null)
                    {
                        Alert.ShowAjaxMsg("Please select Medication Start Date&Time", Page);
                        return;
                    }
                    if (dtpIVINFUMedHangedTime.SelectedDate == null)
                    {
                        Alert.ShowAjaxMsg("Please select Medication Start Date&Time", Page);
                        return;
                    }
                    if (Id != "0")
                    {
                        if (common.myDbl(txtVolumnInfusion.Text) == 0)
                        {
                            Alert.ShowAjaxMsg("Please enter Volume Infused !!", Page);
                            return;
                        }
                        if (common.myDbl(txtVolumnInfusion.Text) > common.myDbl(txtVolumnHanged.Text))
                        {
                            Alert.ShowAjaxMsg("Volume infused should not be greater than volume ", Page);
                            return;
                        }

                        if ((common.myDbl(txtVolumnInfusion.Text) + common.myDbl(txtVolumeDiscard.Text)) > common.myDbl(txtVolumnHanged.Text))
                        {
                            Alert.ShowAjaxMsg("Sum of Volume Infused and Volume discarded should not be greater than volume ", Page);
                            return;
                        }


                        //if ((Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy") + " "
                        //    + (Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedTime.SelectedDate)).ToString("hh:mm tt"))) >
                        //    Convert.ToDateTime(common.myDate(lblScheduleDateTime.Text))
                        //    && Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy") + " " + (Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedTime.SelectedDate)).ToString("hh:mm tt"))) > Convert.ToDateTime(common.myDate(lblScheduleDateTime.Text)).AddHours(1))
                        //     )
                        //{
                        //    //Alert.ShowAjaxMsg("Medication Hooked Time between less than and more than one hour from Schedule Date&Time ", Page);
                        //    Alert.ShowAjaxMsg("Medication Start Date&Time between less than and more than one hour from Schedule Date&Time ", Page);
                        //    return;
                        //}
                        if ((Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy") + " " + (Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedTime.SelectedDate)).ToString("hh:mm tt"))) < Convert.ToDateTime(common.myDate(lblScheduleDateTime.Text))
                            && Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy") + " " + (Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedTime.SelectedDate)).ToString("hh:mm tt"))) < Convert.ToDateTime(common.myDate(lblScheduleDateTime.Text)).AddMinutes(-30)))
                        {
                            // Alert.ShowAjaxMsg("Medication Start Date&Time between less than and more than one hour from Schedule Date&Time ", Page);
                            Alert.ShowAjaxMsg("Medication Start Date&Time less than 30 minites from Schedule Date&Time ", Page);
                            return;
                        }

                        //if ((Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedTime.SelectedDate)).ToString("hh:mm tt")) < Convert.ToDateTime(common.myDate(lblScheduleDateTime.Text))
                        //   && Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy") + " " + Convert.ToDateTime(common.myDate(dtpIVINFUMedHangedTime.SelectedDate)).ToString("hh:mm tt")) < Convert.ToDateTime(common.myDate(lblScheduleDateTime.Text).ToString("MM/dd/yyyy hh:mm tt")).AddHours(-1))
                        //   )
                        //{
                        //    // Alert.ShowAjaxMsg("Medication Hooked Time between less than and more than one hour from Schedule Date&Time ", Page);
                        //    Alert.ShowAjaxMsg("Medication Start Date&Time between less than and more than one hour from Schedule Date&Time ", Page);
                        //    return;
                        //}
                        if (dtpIVINFUCompletedDate.SelectedDate == null)
                        {
                            Alert.ShowAjaxMsg("Please select Medication Completed/Discontinued Date", Page);
                            return;
                        }

                        if (dtpIVINFUCompletedTime.SelectedDate == null)
                        {
                            Alert.ShowAjaxMsg("Please select Medication Completed/Discontinued Time", Page);
                            return;
                        }
                        if (dtpIVINFUCompletedDate.SelectedDate < dtpIVINFUMedHangedDate.SelectedDate)
                        {
                            Alert.ShowAjaxMsg("Completed date should not less than Medication Start Date&Time", Page);
                            return;
                        }
                        if (dtpIVINFUCompletedTime.SelectedDate < dtpIVINFUMedHangedTime.SelectedDate)
                        {
                            Alert.ShowAjaxMsg("Completed Time should not less than Medication Start Date&Time", Page);
                            return;
                        }
                    }

                    //if (common.myStr(Request.QueryString["DT"]) != "PRN")
                    //{
                    //    if (!(DrugScheduleDate.Equals(NormalDate)))// || Convert.ToDateTime(NewDrugScheduleTime).AddHours(1).TimeOfDay.Ticks <= Convert.ToDateTime(NormalTimeNew).TimeOfDay.Ticks )
                    //    {
                    //        if (txtIVINFUReasonForDelay.Text.Trim().Equals(string.Empty))
                    //        {
                    //            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                    //            lblMessage.Text = "Please enter the delay reason.";
                    //            return;
                    //        }
                    //    }
                    //}

                    if (common.myStr(Request.QueryString["DT"]) != "PRN")
                    {
                        // if (!(DrugScheduleDate.Equals(NormalDate)))// || Convert.ToDateTime(NewDrugScheduleTime).AddHours(1).TimeOfDay.Ticks <= Convert.ToDateTime(NormalTimeNew).TimeOfDay.Ticks )
                        if (Convert.ToString(Request.QueryString["Delay"]) == "Y")
                        {
                            if (txtIVINFUReasonForDelay.Text.Trim().Equals(string.Empty))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "Please enter the delay reason.";
                                return;
                            }
                        }
                    }
                }
                else//if (ddlYes.SelectedValue == "N")
                {

                    if (txtIVINFReasonForNotAdministered.Text.Trim().Equals(string.Empty))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter reason for not administered.";
                        txtIVINFReasonForNotAdministered.Focus();
                        return;
                    }
                }

                //   if (CheckPassword(common.myStr(Session["UserId"]), txtIVINFPassword.Text) == true)
                if (CheckPassword(common.myStr(Session["UserName"]), txtIVINFPassword.Text) == true)
                {
                    if (common.myStr(ddlYes.SelectedValue).Equals("Y"))
                    {
                        //Hight Alert Normal
                        if (Request.QueryString["IsHighAlert"] != null && common.myBool(Request.QueryString["IsHighAlert"]) == true)
                        {
                            if (txtIVINFWitnessUseName.Text != "" && txtIVINFWitnessPasswod.Text != "")
                            {
                                if (Session["UserId"].ToString() == txtIVINFWitnessPasswod.Text)
                                {
                                    Alert.ShowAjaxMsg("Please enter another Witness Id", Page);
                                    return;
                                }
                                if (CheckPassword(common.myStr(txtIVINFWitnessUseName.Text), txtIVINFWitnessPasswod.Text) == false)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Alert.ShowAjaxMsg("Please enter Witness Username or password", Page); return;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }

                IVINFUMedHangedDate = Convert.ToDateTime(dtpIVINFUMedHangedDate.SelectedDate).ToString("MM/dd/yyyy");

                IVINFUMedHangedTime = Convert.ToDateTime(dtpIVINFUMedHangedTime.SelectedDate).ToString("HH:mm:00");

                IVINFUCompletedDate = Convert.ToDateTime(dtpIVINFUCompletedDate.SelectedDate).ToString("MM/dd/yyyy");

                IVINFUCompletedTime = Convert.ToDateTime(dtpIVINFUCompletedTime.SelectedDate).ToString("HH:mm:00");

            }
            else
            {
                //  if (CheckPassword(common.myStr(Session["UserId"]), txtNormalUserPassword.Text) == true)
                if (CheckPassword(common.myStr(Session["UserName"]), txtNormalUserPassword.Text) == true)
                {
                    ViewState["pwd"] = txtNormalUserPassword.Text;

                    if (common.myStr(ddlYes.SelectedValue).Equals("Y"))
                    {
                        //Hight Alert IV INFU
                        if (Request.QueryString["IsHighAlert"] != null && common.myBool(Request.QueryString["IsHighAlert"]) == true)
                        {
                            if (txtHightAlertWitnessUserName.Text != "" && txtHightAlertWitnessPassword.Text != "")
                            {
                                if (Session["UserName"].ToString().ToUpper() == txtHightAlertWitnessUserName.Text.ToUpper())
                                {
                                    Alert.ShowAjaxMsg("User name and witness name should not be same\\nPlease enter another witness detail.", Page);
                                    return;
                                }
                                if (CheckPassword(common.myStr(txtHightAlertWitnessUserName.Text), txtHightAlertWitnessPassword.Text) == false)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Alert.ShowAjaxMsg("Please enter Witness Username or password", Page); return;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
                if (hdnBeforeTime.Value == "1")
                {
                    if (txtReasonForDelay.Text.Trim().Equals(string.Empty))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter the before Schedule reason.";
                        return;
                    }
                }


                if (common.myStr(Request.QueryString["DT"]) != "PRN")
                {
                    if (ddlYes.SelectedValue == "Y")
                    {
                        if (common.myStr(Request.QueryString["Delay"]) == "Y")
                        {
                            if (txtReasonForDelay.Text.Trim().Equals(string.Empty))
                            {
                                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                                lblMessage.Text = "Please enter the delay reason.";
                                txtReasonForDelay.Focus();
                                return;
                            }
                        }
                    }
                }
                if (ddlYes.SelectedValue == "N")
                {
                    if (txtNormalReasonForNotAdministered.Text.Trim().Equals(string.Empty))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                        lblMessage.Text = "Please enter reason for not administered.";
                        txtNormalReasonForNotAdministered.Focus();
                        return;
                    }
                }
            }
            string WitnessUserId = "";

            if (common.myBool(Request.QueryString["IsHighAlert"]) || common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                        common.myInt(Session["FacilityId"]), "IsShowWitnessSignInDrugAdmin", sConString)).Equals("Y"))
            //if (Request.QueryString["IsHighAlert"] != null && common.myBool(Request.QueryString["IsHighAlert"]) == true)
            //{
            {
                BaseC.User objuser = new BaseC.User(sConString);
                WitnessUserId = objuser.GetUserID(txtHightAlertWitnessUserName.Text);
            }
            //}

            string strMsg = icm.SaveDoseDetail(common.myInt(Id), common.myInt(Session["HospitalLocationId"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(EncounterId),
                                DrugScheduleDate, DrugScheduleTime, NormalDate, NormalTime,
                                common.myInt(ItemId), common.myInt(IndentId),
                                DoseType == "SINFU" ? txtIVINFReasonForNotAdministered.Text : txtNormalReasonForNotAdministered.Text.Trim(),
                                DoseType == "SINFU" ? txtIVINFUReasonForDelay.Text.Trim() : txtReasonForDelay.Text, common.myInt(Session["UserId"]),
                                DoseType == "SINFU" ? true : false,
                                DoseType == "SINFU" ? (ddlYes.SelectedValue == "Y" ? IVINFUMedHangedDate : null) : null,
                                DoseType == "SINFU" ? (ddlYes.SelectedValue == "Y" ? IVINFUMedHangedTime : null) : null,
                                DoseType == "SINFU" ? txtVolumnHanged.Text : null,
                                DoseType == "SINFU" ? (ddlYes.SelectedValue == "Y" ? (common.myInt(Id) > 0 ? IVINFUCompletedDate : null) : null) : null,
                                DoseType == "SINFU" ? (ddlYes.SelectedValue == "Y" ? (common.myInt(Id) > 0 ? IVINFUCompletedTime : null) : null) : null,
                                DoseType == "SINFU" ? txtVolumnInfusion.Text : null,
                                txtHR.Text, txtRR.Text, txtSBP.Text, txtDBP.Text,
                                ddlYes.SelectedValue == "Y" ? true : false,
                                common.myInt(Request.QueryString["FrequencyDetailId"]), "",
                                DoseType == "SINFU" ? txtVolumeDiscard.Text : null,
                                DoseType == "SINFU" ? ddlVolumeUnit.SelectedValue : "0",
                                DoseType == "SINFU" ? ddlVolInfusionUnit.SelectedValue : "0",
                                DoseType == "SINFU" ? ddlvolDiscardUnit.SelectedValue : "0", WitnessUserId,
                                DoseType == "SINFU" ? common.myStr(txtIVINFRemarks.Text) : common.myStr(txtNormalRemarks.Text));

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                ViewState["pwd"] = "";

                hdnxmlString.Value = "1";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
            }

            lblMessage.Text = strMsg;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            EncounterId = "";
            IndentId = "";
            ItemId = "";
            FrequencyId = "";
            dateTime = "";
            timeSlot = "";
            DoseType = "";
            IVINFUMedHangedDate = "";
            IVINFUMedHangedTime = "";
            IVINFUCompletedDate = "";
            IVINFUCompletedTime = "";
            NormalTime = "";
            NormalDate = "";
            NormalTimeNew = "";
            icm = null;
            //if (common.myStr(ViewState["pwd"]) != "")
            //{
            //    txtNormalUserPassword.Text = common.myStr(ViewState["pwd"]);
            //}

        }
    }
    private void ShowPatientDetails()
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = emr.getScreeningParameters(common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i][0].ToString() == "R")// Respiratory
                    {
                        txtRR.Text = ds.Tables[0].Rows[i][1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i][0].ToString() == "BPS")//BP Systolic
                    {
                        txtDBP.Text = ds.Tables[0].Rows[i][1].ToString();
                    }
                    else if (ds.Tables[0].Rows[i][0].ToString() == "BPD")//BP Diastolic
                    {
                        txtSBP.Text = ds.Tables[0].Rows[i][1].ToString();
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
            ds.Dispose();
        }
    }

    protected void ddlYes_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindControls();
    }


    private void BindControls()
    {
        if (common.myBool(Request.QueryString["IsHighAlert"]) || common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                           common.myInt(Session["FacilityId"]), "IsShowWitnessSignInDrugAdmin", sConString)).Equals("Y"))
        {
            trIVINFWitness.Visible = true;
            trWitness.Visible = true;
        }
        else
        {
            trIVINFWitness.Visible = false;
            trWitness.Visible = false;
        }
        if ((Request.QueryString["DT"] != null && Request.QueryString["IsInfusion"] != null) && ((Request.QueryString["IsInfusion"].ToString() == "1")
            || (Request.QueryString["IsInfusion"].ToString() == "1" && Request.QueryString["DT"].ToString() == "STOP")))
        {
            // common.myBool(Request.QueryString["IsHighAlert"]) == true
            if (ddlYes.SelectedValue == "Y")
            //if(chkNotAdministered.Checked == false)
            {
                tblIVInfusion.Visible = true;
                tblNormailYes.Visible = false;

                dtpIVINFUMedHangedDate.Enabled = true;
                dtpIVINFUMedHangedTime.Enabled = true;
                txtVolumnHanged.Enabled = true;
                dtpIVINFUCompletedDate.Enabled = true;
                dtpIVINFUCompletedTime.Enabled = true;
                txtVolumnInfusion.Enabled = true;
                // txtIVINFUReasonForDelay.Enabled = true;
                txtIVINFPassword.Enabled = true;
                txtIVINFWitnessUseName.Enabled = true;
                txtIVINFWitnessPasswod.Enabled = true;
                txtIVINFReasonForNotAdministered.Enabled = false;
                divIVINFNotAdministered.Visible = false;


                if (common.myStr(Request.QueryString["Delay"]) == "Y")
                {
                    divIVINFReasionDelay.Visible = true;
                    txtIVINFUReasonForDelay.Enabled = true;
                }
                else
                {
                    divIVINFReasionDelay.Visible = false;
                    txtIVINFUReasonForDelay.Enabled = false;
                }

            }
            else if (ddlYes.SelectedValue == "N")
            {
                tblNormailYes.Visible = false;
                tblIVInfusion.Visible = true;

                dtpIVINFUMedHangedDate.Enabled = false;
                dtpIVINFUMedHangedTime.Enabled = false;
                txtVolumnHanged.Enabled = false;
                dtpIVINFUCompletedDate.Enabled = false;
                dtpIVINFUCompletedTime.Enabled = false;
                txtVolumnInfusion.Enabled = false;
                // txtIVINFUReasonForDelay.Enabled = false;
                txtIVINFPassword.Enabled = true;
                txtIVINFWitnessUseName.Enabled = false;
                txtIVINFWitnessPasswod.Enabled = false;
                txtIVINFReasonForNotAdministered.Enabled = true;

                divIVINFReasionDelay.Visible = false;
                txtIVINFUReasonForDelay.Enabled = false;
                divIVINFNotAdministered.Visible = true;
            }
            else
            {
                tblIVInfusion.Visible = false;
                tblNormailYes.Visible = false;
            }
        }
        else  // without infusion 
        {
            tblNormailYes.Visible = true;
            ShowPatientDetails();
            try
            {
                dtpNormalDate.SelectedDate = System.DateTime.Now;
            }
            catch
            {
            }

            try
            {
                dtpNormalTime.SelectedDate = System.DateTime.Now;
            }
            catch
            {
            }
            // if (chkNotAdministered.Checked == false )
            if (ddlYes.SelectedValue == "Y")
            {
                tblIVInfusion.Visible = false;
                // tblNormailYes.Visible = true;
                dtpNormalDate.Enabled = true;
                dtpNormalTime.Enabled = true;
                //  txtReasonForDelay.Enabled = true;
                txtNormalUserPassword.Enabled = true;
                txtHightAlertWitnessUserName.Enabled = true;
                txtHightAlertWitnessPassword.Enabled = true;
                txtHR.Enabled = true;
                txtRR.Enabled = true;
                txtDBP.Enabled = true;
                txtSBP.Enabled = true;
                //txtNormalReasonForNotAdministered.Enabled = false;
                //txtRemark.Enabled = true;
                txtNormalReasonForNotAdministered.Text = "";
                divReasionDelay.Visible = true;
                dvNOtAdministered.Visible = false;

                if (common.myStr(Request.QueryString["Delay"]) == "Y")
                {
                    divReasionDelay.Visible = true;
                    txtReasonForDelay.Enabled = true;
                }
                else
                {
                    divReasionDelay.Visible = false;
                    txtReasonForDelay.Enabled = false;
                }
                if (common.myStr(Request.QueryString["DT"]) == "PRN")
                {
                    //txtReasonForDelay.Enabled = false;
                    txtReasonForDelay.Text = "";
                    divReasionDelay.Visible = false;
                }

                spnHightAlertUserPassword.Visible = true;
            }
            else if (ddlYes.SelectedValue == "N")
            {
                // tblNormailYes.Visible = true;
                tblIVInfusion.Visible = false;
                dtpNormalDate.Enabled = false;
                dtpNormalTime.Enabled = false;
                txtReasonForDelay.Enabled = false;
                txtNormalUserPassword.Enabled = true;
                txtHightAlertWitnessUserName.Enabled = false;
                txtHightAlertWitnessPassword.Enabled = false;
                txtHR.Enabled = false;
                txtRR.Enabled = false;
                txtDBP.Enabled = false;
                txtSBP.Enabled = false;
                txtNormalReasonForNotAdministered.Enabled = true;
                // txtRemark.Text = "";
                txtReasonForDelay.Text = "";
                // txtRemark.Enabled = false;
                divReasionDelay.Visible = false;
                dvNOtAdministered.Visible = true;

                spnHightAlertUserPassword.Visible = false;
            }
            else// in blank case
            {
                tblIVInfusion.Visible = false;
                tblNormailYes.Visible = false;
            }
        }
        if (Request.QueryString["DT"] != null && Request.QueryString["DT"].ToString() == "STOP")
        {
            //Normal COntrol
            //  txtRemark.Enabled = false;
            dtpNormalDate.Enabled = false;
            dtpNormalTime.Enabled = false;
            txtReasonForDelay.Enabled = false;
            txtNormalUserPassword.Enabled = false;
            txtHightAlertWitnessUserName.Enabled = false;
            txtHightAlertWitnessPassword.Enabled = false;
            txtHR.Enabled = false;
            txtRR.Enabled = false;
            txtDBP.Enabled = false;
            txtSBP.Enabled = false;
            //txtNormalReasonForNotAdministered.Enabled = false;
            //IV Infusion
            dtpIVINFUMedHangedDate.Enabled = false;
            dtpIVINFUMedHangedTime.Enabled = false;
            txtVolumnHanged.Enabled = false;
            dtpIVINFUCompletedDate.Enabled = false;
            dtpIVINFUCompletedTime.Enabled = false;
            txtVolumnInfusion.Enabled = false;
            txtIVINFUReasonForDelay.Enabled = false;
            divIVINFReasionDelay.Visible = false;
            divReasionDelay.Visible = false;
            txtIVINFPassword.Enabled = false;
            txtIVINFWitnessUseName.Enabled = false;
            txtIVINFWitnessPasswod.Enabled = false;
            txtIVINFReasonForNotAdministered.Enabled = false;
            btnSubmit.Enabled = false;
            ddlYes.Enabled = false;
            // chkNotAdministered.Enabled = false;            
        }

        lblNormalUserName.Text = common.myStr(Session["UserName"]);
        lblIVINFUserName.Text = common.myStr(Session["UserName"]);
        BaseC.EMR objEMR = new BaseC.EMR(sConString);
        SqlDataReader objDr = objEMR.GetEmployeeId(common.myInt(Session["Userid"]), common.myInt(Session["HospitalLocationId"]));
        if (objDr.Read())
        {
            lblNormalUserName.Text = lblNormalUserName.Text + " (" + common.myStr(objDr[1]) + ")";// Session["UserId"].ToString();
            lblIVINFUserName.Text = lblNormalUserName.Text + " (" + common.myStr(objDr[1]) + ")";// Session["UserName"].ToString();
        }
        objDr.Close();

    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        hdnxmlString.Value = "1";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string id = ViewState["ID"].ToString();

        string strMsg = Delete(common.myInt(id), common.myInt(Session["Userid"]));

        if (strMsg.Contains(" Delete") && !strMsg.Contains("usp"))
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }

        lblMessage.Text = strMsg;
    }
    public string Delete(int id, int encodedBy)
    {
        Hashtable HashIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HashIn.Add("@id", id);
        HashIn.Add("@Encodedby", encodedBy);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DeleteDrugAdministration", HashIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }
    public void BindUnit()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        ds = objEMR.GetUnitMaster(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), 1);

        ddlVolumeUnit.DataSource = ds.Tables[0];
        ddlVolumeUnit.DataValueField = "Id";
        ddlVolumeUnit.DataTextField = "UnitName";
        ddlVolumeUnit.DataBind();
        ddlVolumeUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
        ddlVolumeUnit.SelectedIndex = 0;

        ddlVolInfusionUnit.DataSource = ds.Tables[0];
        ddlVolInfusionUnit.DataValueField = "Id";
        ddlVolInfusionUnit.DataTextField = "UnitName";
        ddlVolInfusionUnit.DataBind();
        ddlVolInfusionUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
        ddlVolInfusionUnit.SelectedIndex = 0;

        ddlvolDiscardUnit.DataSource = ds.Tables[0];
        ddlvolDiscardUnit.DataValueField = "Id";
        ddlvolDiscardUnit.DataTextField = "UnitName";
        ddlvolDiscardUnit.DataBind();
        ddlvolDiscardUnit.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(string.Empty, "0"));
        ddlvolDiscardUnit.SelectedIndex = 0;

        ds.Dispose();
    }
}
