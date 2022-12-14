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
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using Microsoft.Reporting.WebForms;

public partial class EMR_Templates_Default : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;

    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    clsExceptionLog objException = new clsExceptionLog();
    //BaseC.RestFulAPI objwcf = new BaseC.RestFulAPI(sConString);
    //clsIVF objivf;
    //BaseC.clsEMR objEMR;
    private int minTemplateId = int.MaxValue;
    private string cCtlType = string.Empty;
    //DL_Funs fun = new DL_Funs();
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();
    static DataTable dtFieldMaxLength;
    static DataTable dtNoOfColumn;
    static DataView DropDownItems;


    Boolean AddRow = false;
    bool isFirstTime = false;

    string Saved_RTF_Content;
    StringBuilder sb = new StringBuilder();
    string Fonts = string.Empty;
    static string gBegin = "<u>";
    static string gEnd = "</u>";
    StringBuilder objStrTmp = new StringBuilder();
    private int iPrevId = 0;

    //private string sEncounterId = "";
    //private string sRegistrationId = "";
    string sFontSize = "";

    private enum enumT : byte
    {
        Col0 = 2,
        Col1 = 3,
        Col2 = 4,
        Col3 = 5,
        Col4 = 6,
        Col5 = 7,
        Col6 = 8,
        Col7 = 9,
        Col8 = 10,
        Col9 = 11,
        Col10 = 12,
        Col11 = 13,
        Col12 = 14,
        Col13 = 15,
        Col14 = 16,
        Col15 = 17,
        Col16 = 18,
        Col17 = 19,
        Col18 = 20,
        Col19 = 21,
        Col20 = 22,
        Col21 = 23,
        Col22 = 24,
        Col23 = 25,
        Col24 = 26,
        Col25 = 27,
        Col26 = 28,
        Col27 = 29,
        Col28 = 30,
        Col29 = 31,
        Col30 = 32,
        SectionId = 33,
        RowNum = 34,
        IsData = 35,
        Id = 36,
        RowCaptionId = 37,
        RowCaptionName = 38
    }

    private enum enumNonT : byte
    {
        FieldId = 0,
        FieldName = 1,
        FieldType = 2,
        Values = 3,
        Remarks = 4,
        ParentId = 5,
        ParentValue = 6,
        Hierarchy = 7,
        SectionId = 8,
        DataObjectId = 9,
        IsMandatory = 10,
        MandatoryType = 11,
        EmployeeTypeId = 12
        //    ,
        //DateType=13
    }

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myInt(Request.QueryString["TmpId"]) > 0
            || common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO")
            || common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP")
            || common.myStr(Request.QueryString["Source"]).ToUpper().Equals("PROCEDUREORDER"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            // Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMaster.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (common.myStr(Request.QueryString["EncounterId"]).Trim() != "")
        //    sEncounterId = common.myStr(Request.QueryString["EncounterId"]);
        //else if (common.myStr(Session["EncounterId"]) != "")
        //    sEncounterId = common.myStr(Session["EncounterId"]);

        //if (common.myStr(Request.QueryString["RegistrationId"]) != "")
        //    sRegistrationId = common.myStr(Request.QueryString["RegistrationId"]);
        //else if (common.myStr(Session["RegistrationId"]) != "")
        //    sRegistrationId = common.myStr(Session["RegistrationId"]);

        lblMessage.Text = string.Empty;

        if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
        {
            Session["CurrentNode"] = "P" + common.myStr(ddlTemplateMain.SelectedValue);
        }

        if (!IsPostBack)
        {
            Session["TemplatePreviousPageUrl"] = Request.Url.PathAndQuery;
            dtpfromDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpToDate.DateInput.DateFormat = common.myStr(Application["OutputDateformat"]);
            dtpfromDate.SelectedDate = DateTime.Now.AddMonths(-3);
            dtpToDate.SelectedDate = DateTime.Now;
            #region set view state values

            ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegistrationId"]);
            ViewState["EncounterId"] = common.myInt(Request.QueryString["EncounterId"]);
            ViewState["Source"] = common.myStr(Request.QueryString["Source"]).Trim();
            ViewState["ServiceId"] = common.myInt(Request.QueryString["ServId"]);
            ViewState["TagType"] = common.myStr(Request.QueryString["TagType"]).Trim();
            ViewState["CF"] = common.myStr(Request.QueryString["CF"]).Trim();
            ViewState["TypeName"] = common.myStr(Request.QueryString["Type"]).Trim();
            ViewState["SingleScreenTemplateCode"] = common.myStr(Request.QueryString["SingleScreenTemplateCode"]);


            if (common.myInt(ViewState["RegistrationId"]).Equals(0))
            {
                ViewState["RegistrationId"] = common.myInt(Session["RegistrationId"]);
            }
            if (common.myInt(ViewState["EncounterId"]).Equals(0))
            {
                ViewState["EncounterId"] = common.myInt(Session["EncounterId"]);
            }

            #endregion
            BindMotherChildRelationship();
            if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP") || common.myStr(Request.QueryString["IsEMRPopUp"]).ToUpper().Equals("1"))
            {
                btnClose.Visible = true;
                chkApprovalRequired.Visible = true;
                BaseC.clsEMRBilling objBill = new BaseC.clsEMRBilling(sConString);
                int iAdviserDoctor = 0;
                iAdviserDoctor = objBill.GetPatientConsultingDoctor(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
                Session["AdviserDoctorId"] = iAdviserDoctor;
                BindAdviserDoctor();
                ddlAdvisingDoctor.SelectedIndex = ddlAdvisingDoctor.Items.IndexOf(ddlAdvisingDoctor.Items.FindItemByValue(common.myStr(iAdviserDoctor)));

            }
            if (!common.myStr(ViewState["Source"]).Equals("DeptRegister"))
            {
                if (common.myInt(ViewState["EncounterId"]).Equals(0) && common.myStr(ViewState["CF"]).Equals(string.Empty))
                {
                    Response.Redirect("/default.aspx?RegNo=0", false);
                    return;
                }
            }
            if (common.myStr(ViewState["TagType"]).Equals("D"))
            {
                lblServices.Text = "Sub Department";
            }
            else
            {
                lblServices.Text = "Services";
            }

            dvMandatory.Visible = false;


            tblReport.Visible = false;
            ViewState["CalCulate"] = null;
            //btnSetDefault.Visible = false;
            btnAddRow.Visible = false;
            btnFormulaCalculate.Visible = false;
            btnSave.Visible = false;
            hdnIsUnSavedData.Value = "0";
            visiblilityResultSet(false);

            lblTemplateName.Visible = false;

            ddlRecord.Visible = false;
            Label1.Visible = false;
            btnNewRecord.Visible = false;
            BindGroupTaggingMenu();
            ViewState["ResultSetId"] = "0";

            BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
            ViewState["DoctorId"] = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));

            BaseC.User objU = new BaseC.User(sConString);
            ViewState["EmployeeType"] = common.myStr(objU.getEmployeeType(common.myInt(ViewState["DoctorId"]))).Trim();

            clsIVF objivf = new clsIVF(sConString);

            DataSet dsEncSpec = objivf.getDoctorSpecialisation(common.myInt(ViewState["DoctorId"]));

            if (dsEncSpec.Tables[0].Rows.Count > 0)
            {
                ViewState["DoctorSpecialisationId"] = common.myInt(dsEncSpec.Tables[0].Rows[0]["SpecialisationId"]);
            }

            ViewState["IsEncounterClose"] = objivf.IsEncounterClose(common.myInt(ViewState["EncounterId"]));

            ViewState["AllowNewRow"] = false;

            chkPrintHospitalHeader.Checked = objivf.getPrintTemplateReportHeaderCheck();

            bindMainTemplateList();

            if (common.myInt(ViewState["ServiceId"]) > 0 && common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO"))
            {
                btnNew.Visible = false;
                //btnCaseSheet.Visible = false;
                btnClose.Visible = true;

                if (common.myStr(ViewState["CF"]).Equals("LAB"))
                {
                    btnSave.Visible = false;
                }
            }

            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
            {
                btnLastVisitDetails.Visible = false;

                BindTemplateRequiredService(common.myStr(Request.QueryString["TemplateRequiredServices"]).Trim());

                if (!common.myStr(ViewState["TagType"]).Equals("D"))
                {
                    TemplateRequiredServices.Visible = true;

                    if (ddlTemplateMain.Items.Count > 0)
                    {
                        ddlTemplateMain.SelectedIndex = 0;
                    }

                    ddlTemplateMainSelectedIndexChanged(false);
                    setServiceChkVisiblity();
                }
                else
                {
                    trGridTemplateRequiredServices.Visible = true;
                }

                settingControls(common.myStr(ViewState["Source"]));
                dvlegend.Style["display"] = "block";
                lblColorCodeForTemplateRequired.BackColor = System.Drawing.Color.Red;
                lblColorCodeForMandatoryTemplate.BackColor = System.Drawing.Color.Blue;
            }
            BindPatientHiddenDetails();
            if (!common.myStr(Request.QueryString["SourceForLIS"]).Trim().Equals(string.Empty))
            {
                if (common.myStr(Request.QueryString["SourceForLIS"]).Trim().Equals("LIS"))
                {
                    bindMainTemplateListOfSelectedService(common.myInt(ViewState["ServiceId"]).ToString());
                }
                if (ddlTemplateMain.Items.Count > 0)
                {
                    if (common.myInt(ddlTemplateMain.SelectedIndex).Equals(-1))
                    {
                        ddlTemplateMain.SelectedIndex = 0;
                        ddlTemplateMainSelectedIndexChanged(false);
                    }
                }
                btnSave.Visible = true;
            }

            if (common.myStr(ViewState["CF"]).Equals("LAB"))
            {
                tblManualLabNo.Visible = true;
                lblManualLabNo.Text = common.myStr(Request.QueryString["ManualLabNo"]).Trim();
            }
            else
            {
                tblManualLabNo.Visible = false;
            }

            if (common.myInt(Request.QueryString["FreeTextTemplateId"]) > 0)
            {
                ddlTemplateMain_SelectedIndexChanged(null, null);
            }

            if ((common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true || (common.myStr(Request.QueryString["WardStatus"]).ToUpper().Equals("CLOSE"))) && (!common.myBool(Session["isEMRSuperUser"])))
            {
                ddlTemplateMain.SelectedIndex = -1;
                ddlTemplateMain_SelectedIndexChanged(null, null);
                btnSave.Visible = false;
                btnNew.Visible = false;
                ddlTemplateMain.Enabled = false;
                ddlTemplatePatient.Enabled = true;
            }
            else if (common.myBool(Session["isEMRSuperUser"]) == true
                && common.myStr(ViewState["SingleScreenTemplateCode"]) != "OTH"
                && common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                btnSave.Visible = true;
                btnSave.Enabled = true;
            }
            else if (common.myBool(Session["isEMRSuperUser"]) == true
                && common.myStr(ViewState["SingleScreenTemplateCode"]) == "OTH"
                && common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE"))
            {
                btnSave.Visible = true;
                btnSave.Enabled = true;
            }
            EnableSaving();
            ddlTemplateMain.Focus();
        }

        try
        {
            setRequestSelectedServiceColor();
            if (common.myInt(ddlTemplateMain.SelectedIndex) > 0)
            {
                bool a = common.myBool(ddlTemplateMain.SelectedItem.Attributes["IsCopyPreviousEpisod"]);
                btnsetPreviousData.Visible = common.myBool(ddlTemplateMain.SelectedItem.Attributes["IsCopyPreviousEpisod"]);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.ToString();
        }
        //   bindReportList();

        try
        {
            if (common.myInt(hdnSelCell.Text) > 0 && common.myLen(hdnSentanceGalleryControlId.Value) > 0)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "ctrlVisible();", true);

                //hdnSentanceGalleryControlId.Value = string.Empty;
            }
        }
        catch
        {
        }
        IsCopyCaseSheetAuthorized();
    }

    private void SetPermission()
    {
        if (common.myInt(Session["ModuleId"]).Equals(3) || common.myInt(Session["ModuleId"]).Equals(30))
        {
            UserAuthorisations ua1 = new UserAuthorisations(sConString);
            try
            {
                if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP")) //pagename
                {
                    if (common.myBool(ViewState["IsDataSaved"]))
                    {
                        btnSave.Enabled = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
                    }
                    else
                    {
                        btnSave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
                    }
                }
                else //pageid
                {
                    string pid = common.myStr(Request.QueryString["Mpg"]);

                    if (common.myLen(pid) > 0)
                    {
                        int len = pid.Length;
                        int pageid = common.myInt(pid.ToString().Substring(1, len - 1));

                        if (common.myBool(ViewState["IsDataSaved"]))
                        {
                            btnSave.Enabled = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]), pageid);
                        }
                        else
                        {
                            btnSave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]), pageid);
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
                ua1.Dispose();
            }
        }
    }
    protected void EnableSaving()
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataTable dt = new DataTable();
        BaseC.Dashboard objDash = new BaseC.Dashboard();
        BaseC.RestFulAPI objCM = new BaseC.RestFulAPI(sConString);
        try
        {
            if ((common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE").Equals(true) && common.myBool(Session["isEMRSuperUser"]).Equals(true)))
            {
                tblProviderDetails.Visible = true;
                ds = objCM.GetDoctorList(Convert.ToInt16(common.myInt(Session["HospitalLocationID"])), 0, Convert.ToInt16(common.myInt(Session["FacilityId"])));
                ddlProvider.DataSource = ds;
                ddlProvider.DataValueField = "DoctorID";
                ddlProvider.DataTextField = "DoctorName";
                ddlProvider.DataBind();

                RadComboBoxItem rcbDoctorId = (RadComboBoxItem)ddlProvider.Items.FindItemByValue(common.myInt(Session["DoctorID"]).ToString());
                if (rcbDoctorId != null)
                    rcbDoctorId.Selected = true;

                ds = objDash.getPatientEncounterDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), common.myInt(Session["RegistrationId"]));
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
            objDash = null;
        }
    }
    private void BindPermissionConfidentialUsers()
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        ViewState["AllowConfidentialTemplate"] = false;
        ViewState["DisplayIsConfidentialLink"] = false;
        try
        {
            if (ddlTemplateMain.SelectedItem != null && common.myBool(ddlTemplateMain.SelectedItem.Attributes["IsConfidential"]))
            {
                ds = emr.getPermissionConfidentialUsers(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                    common.myInt(ddlTemplateMain.SelectedValue), common.myInt(ViewState["EncounterId"]));
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "DoctorId=" + common.myStr(Session["EmployeeId"]);
                if (dv.ToTable().Rows.Count > 0 || common.myInt(Session["EmployeeId"]) == common.myInt(Session["DoctorId"]))
                {
                    ViewState["AllowConfidentialTemplate"] = true;
                }

                if (common.myInt(Session["EmployeeId"]) == common.myInt(Session["DoctorId"]))
                {
                    ViewState["DisplayIsConfidentialLink"] = true;
                }

                if (ds.Tables[1].Rows.Count == 0)
                {
                    ViewState["AllowConfidentialTemplate"] = true;
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        if (common.myInt(Session["EmployeeId"]) == common.myInt(ds.Tables[1].Rows[i]["Encodedby"]))
                        {
                            ViewState["AllowConfidentialTemplate"] = true;
                        }
                    }
                }
                dv.Dispose();
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = ("Error: " + ex.Message);
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            emr = null;
            ds.Dispose();
        }
    }
    private void BindGroupTaggingMenu()
    {
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = user.GetGroupWiseMenuTagging(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Session["GroupId"]),
                common.myInt(Session["ModuleId"]), "EMRTEMPLATE");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["AllowAddFieldValue"] = common.myStr(ds.Tables[0].Rows[0]["OptionCode"]);
            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            user = null;
            ds.Dispose();
        }
    }
    private void BindMotherChildRelationship()
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dt = new DataTable();
        try
        {
            dt = emr.GetMotherNewBornBabyRelation(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ViewState["RelationRegistrationId"] = dt.Rows[0]["RegistrationId"].ToString();
                    ViewState["RelationEncounterId"] = dt.Rows[0]["EncounterId"].ToString();

                    ViewState["RelationRegistrationNo"] = dt.Rows[0]["RegistrationNo"].ToString();
                    ViewState["RelationEncounterNo"] = dt.Rows[0]["EncounterNo"].ToString();

                    ViewState["RelationEncounterDate"] = dt.Rows[0]["EncounterDate"].ToString();
                    ViewState["RelationDoctorId"] = dt.Rows[0]["DoctorId"].ToString();
                    ViewState["RelationOPIP"] = dt.Rows[0]["OPIP"].ToString();
                    //  lnkClinicalDetail.Text =common.myStr(dt.Rows[0]["TitleName"]);
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            dt.Dispose();
            emr = null;
        }
    }
    //protected void lnkClinicalDetail_OnClick(object sender, EventArgs e)
    //{
    //    BaseC.Patient bC = new BaseC.Patient(sConString);
    //    DataSet dsPatientDetail = new DataSet();
    //    Session["RelationPatientDetailString"] = null;
    //    try
    //    {
    //        if (common.myStr(ViewState["RelationEncounterId"]) != "")
    //        {
    //            dsPatientDetail = bC.getEMRPatientDetails(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myStr(ViewState["RelationRegistrationNo"]),
    //                         common.myStr(ViewState["RelationEncounterNo"]), common.myInt(Session["UserId"]));

    //            if (dsPatientDetail.Tables.Count > 0)
    //            {
    //                if (dsPatientDetail.Tables[0].Rows.Count > 0)
    //                {
    //                    string sRegNoTitle = Resources.PRegistration.regno;
    //                    string sDoctorTitle = Resources.PRegistration.Doctor;
    //                    string DateTitle = common.myStr(dsPatientDetail.Tables[0].Rows[0]["OPIP"]) == "I" ? "Admission Date : " : "Encounter Date : ";
    //                    Session["RelationPatientDetailString"] = "<b><span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["PatientName"]) + ", " + common.myStr(dsPatientDetail.Tables[0].Rows[0]["GenderAge"]) + "</span>"
    //                     + "&nbsp;" + sRegNoTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["RegistrationNo"]) + "</span>"
    //                     + "&nbsp;Enc #.:&nbsp;" + "<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterNo"]) + "</span>"
    //                     + "&nbsp;" + sDoctorTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["DoctorName"]) + "</span>&nbsp;"
    //                     + DateTitle + "&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["EncounterDate"]) + "</span>"
    //                     + "&nbsp;Bed No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["BedNo"]) + "</span>"
    //                     + "&nbsp;Ward:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["WardName"]) + "</span>"
    //                     + "&nbsp;Mobile No.:&nbsp;<span style='color: #990066;font-weight: bold;'>" + common.myStr(dsPatientDetail.Tables[0].Rows[0]["MobileNo"]) + "</span>"
    //                     + "</b>";
    //                }


    //            }
    //            RadWindowPrint.NavigateUrl = "/Editor/WordProcessor.aspx?From=POPUP&RegId=" + common.myStr(ViewState["RelationRegistrationId"]) + "&EncId=" + common.myStr(ViewState["RelationEncounterId"]) + "&DoctorId=" + common.myStr(ViewState["RelationDoctorId"]) + "&OPIP=" + common.myStr(ViewState["OPIP"])
    //                           + "&EncounterDate=" + common.myDate(ViewState["RelationEncounterDate"]).ToString("yyyy/MM/dd") + "&RPD=Y" ;
    //            RadWindowPrint.Height = 600;
    //            RadWindowPrint.Width = 900;
    //            RadWindowPrint.Top = 20;
    //            RadWindowPrint.Left = 20;
    //            RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //            RadWindowPrint.Modal = true;
    //            RadWindowPrint.InitialBehavior = WindowBehaviors.Maximize;
    //            RadWindowPrint.VisibleStatusbar = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //    finally
    //    {
    //         bC = null;
    //        dsPatientDetail.Dispose();
    //    }
    //}
    private void setRequestSelectedServiceColor()
    {
        try
        {
            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
            {
                if (!common.myStr(ViewState["TagType"]).Equals("D"))
                {
                    if (common.myInt(ViewState["TemplateRequiredServiceId"]) > 0)
                    {
                        foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                        {
                            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");

                            if (common.myInt(hdnServiceId.Value) > 0)
                            {
                                if (common.myInt(hdnServiceId.Value).Equals(common.myInt(ViewState["TemplateRequiredServiceId"])))
                                {
                                    dataItem.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Pink);
                                }
                                else
                                {
                                    dataItem.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.White);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    void BindPatientHiddenDetails()
    {
        try
        {
            //if (Session["PatientDetailString"] != null)
            //{
            //    lblPatientDetail.Text = common.myStr(Session["PatientDetailString"]);
            //}

            if (common.myStr(ViewState["CF"]).Equals("LAB"))
            {
                settingControls(common.myStr(ViewState["CF"]));
            }
            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
            {
                settingControls(common.myStr(ViewState["Source"]));
            }
            if (common.myStr(ViewState["Source"]).Equals("DeptRegister"))
            {
                settingControls(common.myStr(ViewState["Source"]));
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnCheck_Onclick(object sender, EventArgs e)
    {
        int row = common.myInt(hdnSelCell.Text);
        RadEditor rad1 = (RadEditor)gvSelectedServices.Rows[row].FindControl("txtW");
        rad1.Content += common.myStr(Session["rtfText"]);
    }
    private void addRowTabular()
    {
        try
        {
            if (ViewState["TabularRowData"] == null)
            {
                return;
            }

            DataTable tbl = (DataTable)ViewState["TabularRowData"];

            if (common.myInt(tbl.Rows.Count).Equals(0))
            {
                return;
            }

            int SelectedRowId = common.myInt(hdnSelectedRow.Value);
            if (SelectedRowId.Equals(0))
            {
                return;
            }

            #region data retain

            DataTable dt = tbl.Copy();

            int flag = 0;

            for (int rIdx = 0; rIdx < gvTabularFormat.Items.Count; rIdx++)
            {
                if (common.myStr(gvTabularFormat.Items[rIdx].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                {
                    flag = 0;
                }
                else if (common.myStr(gvTabularFormat.Items[rIdx].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                {
                    if (flag.Equals(0))
                    {
                        ViewState["ValueOfControlType"] = rIdx - 1;
                        flag = 1;
                    }

                    string valText = string.Empty;

                    for (int cIdx = 1; cIdx < tbl.Columns.Count - 5; cIdx++)
                    {
                        Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[cIdx].FindControl("lblFieldId" + cIdx.ToString());

                        dt.DefaultView.RowFilter = string.Empty;
                        dt.DefaultView.RowFilter = "Id=" + (rIdx + 1);

                        if (dt.DefaultView.Count.Equals(0))
                        {
                            continue;
                        }

                        lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[cIdx].FindControl("lblFieldId" + cIdx.ToString());

                        if (common.myStr(lblControlType.Text).Equals("T"))
                        {
                            TextBox txtVisData = (TextBox)gvTabularFormat.Items[rIdx].Cells[cIdx].FindControl("txtT" + cIdx.ToString());
                            valText = common.myStr(txtVisData.Text);
                            txtVisData.ToolTip = valText;
                            Label fieldid = new Label();
                            if (dtFieldMaxLength != null)
                            {
                                if (dtFieldMaxLength.Rows.Count > 0)
                                {
                                    fieldid = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[cIdx].FindControl("lblFieldId" + cIdx.ToString());

                                    dtFieldMaxLength.DefaultView.RowFilter = "FieldId=" + common.myInt(fieldid.Text);
                                    if (dtFieldMaxLength.DefaultView.Count > 0)
                                    {
                                        txtVisData.Columns = (common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]) > 30) ? 30 : common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]);
                                        txtVisData.MaxLength = common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]);
                                    }
                                    dtFieldMaxLength.DefaultView.RowFilter = string.Empty;
                                }
                            }
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                txtVisData.Enabled = false;
                                btnAddRow.Enabled = false;
                                btnFormulaCalculate.Enabled = false;
                            }
                            else
                            {
                                txtVisData.Enabled = true;
                                btnAddRow.Enabled = true;
                                btnFormulaCalculate.Enabled = true;
                            }

                            if (btnFormulaCalculate.Visible)
                            {
                                IsFormulaField(common.myInt(fieldid.Text), false, ref txtVisData);
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("M"))
                        {
                            TextBox txtVisData = (TextBox)gvTabularFormat.Items[rIdx].Cells[cIdx].FindControl("txtM" + cIdx.ToString());
                            valText = common.myStr(txtVisData.Text);

                            txtVisData.ToolTip = valText;
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                txtVisData.Enabled = false;
                            }
                            else
                            {
                                txtVisData.Enabled = true;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("D"))
                        {
                            DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[rIdx].Cells[cIdx].FindControl("D" + cIdx.ToString());
                            valText = common.myStr(ddlVisData.SelectedValue);
                            if (common.myStr(ViewState["EntryType"]).Equals("E") || !common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                ddlVisData.Enabled = false;
                            }
                            else
                            {
                                ddlVisData.Enabled = true;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("IM"))
                        {
                            RadComboBox ddlVisData = (RadComboBox)gvTabularFormat.Items[rIdx].Cells[cIdx].FindControl("IM" + cIdx.ToString());
                            valText = common.myStr(ddlVisData.SelectedValue);
                            if (common.myStr(ViewState["EntryType"]).Equals("E") || !common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                ddlVisData.Enabled = false;
                            }
                            else
                            {
                                ddlVisData.Enabled = true;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("B"))
                        {
                            DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[rIdx].Cells[cIdx].FindControl("B" + cIdx.ToString());
                            valText = common.myStr(ddlVisData.SelectedValue);
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                ddlVisData.Enabled = false;
                            }
                            else
                            {
                                ddlVisData.Enabled = true;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("S"))
                        {
                            TextBox txtDate = (TextBox)gvTabularFormat.Items[rIdx].Cells[cIdx].FindControl("txtDate" + cIdx.ToString());

                            if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                            {
                                if (!txtDate.Text.Trim().Equals(string.Empty))
                                {
                                    //valText = Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy");
                                    valText = txtDate.Text.Trim();
                                }
                            }
                            else
                            {
                                valText = string.Empty;
                            }
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                txtDate.Enabled = false;
                            }
                            else
                            {
                                txtDate.Enabled = true;
                            }
                        }
                        else
                        {
                            valText = string.Empty;
                        }

                        if (dt.DefaultView.Count > 0)
                        {
                            dt.DefaultView[0]["Col" + (cIdx + 1)] = valText.Trim();
                            dt.AcceptChanges();
                            dt.DefaultView.RowFilter = string.Empty;
                            dt.AcceptChanges();
                        }
                    }
                }
            }
            #endregion

            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.RowFilter = string.Empty;
                dt.AcceptChanges();

                tbl = new DataTable();
                tbl = dt;
            }

            DataTable secTbl = tbl;
            DataView secDV = secTbl.Copy().DefaultView;

            secDV.RowFilter = "Id=" + SelectedRowId;
            secTbl = secDV.ToTable();

            tbl.AcceptChanges();
            DataRow DR = tbl.NewRow();

            if (secTbl.Rows.Count > 0)
            {
                DataRow tdr = secTbl.Rows[0];

                DR["Id"] = common.myInt(tdr["Id"]) + 1;
                DR["SectionId"] = tdr["SectionId"];
                DR["RowNum"] = common.myInt(tdr["RowNum"]) + 1;
                DR["IsData"] = tdr["IsData"];
                DR["RowCaptionId"] = tdr["RowCaptionId"];
                DR["Col1"] = tdr["Col1"];
            }
            tbl.AcceptChanges();
            tbl.Rows.InsertAt(DR, SelectedRowId);

            for (int idxR = 0; idxR < tbl.Rows.Count; idxR++)
            {
                tbl.Rows[idxR]["Id"] = idxR + 1;
                tbl.Rows[idxR]["RowNum"] = idxR;
            }
            tbl.AcceptChanges();

            ViewState["TabularRowData"] = tbl;
            gvTabularFormat.DataSource = tbl;
            gvTabularFormat.DataBind();

            if (dtNoOfColumn.Rows.Count > 0)
            {
                dtNoOfColumn.AcceptChanges();
                dtNoOfColumn.Rows[0]["RowsCount"] = tbl.Rows.Count - 3;
            }

            showgridcontrols();

            int RowStartIndex = 0;
            flag = 0;

            for (int i = 0; i < gvTabularFormat.Items.Count; i++)
            {
                if (common.myInt(common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text).Trim()).Equals(common.myInt(ViewState["SelectedNode"])))
                {
                    if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                    {
                        if (flag.Equals(0))
                        {
                            RowStartIndex = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.Id].Text);
                        }

                        if (flag < 3)
                        {
                            gvTabularFormat.Items[i].Visible = false;
                        }
                    }
                    else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                    {
                        gvTabularFormat.Items[i].Visible = true;
                    }
                    flag++;
                }
                else
                {
                    gvTabularFormat.Items[i].Visible = false;
                }
            }

            DataRow[] drTT = dtNoOfColumn.Select("SectionId=" + ViewState["SelectedNode"]);

            for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++) //change 5 to 6
            {
                Label lblHeading = (Label)gvTabularFormat.Items[RowStartIndex + 1].Cells[j].FindControl("lblFieldId" + j.ToString());

                //Label lblHeading = (Label)gvTabularFormat.Items[RowStartIndex].Cells[j].FindControl("lblFieldId" + j.ToString());

                GridHeaderItem headerItem = gvTabularFormat.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                headerItem.Cells[j + 2].Text = lblHeading.Text;
                headerItem.Cells[j + 2].Font.Size = 10;

                if (drTT.Length > 0)
                {
                    if (j >= common.myInt(drTT[0][1]))
                    {
                        gvTabularFormat.Columns[j + 2].Visible = false;
                    }
                    else
                    {
                        gvTabularFormat.Columns[j + 2].Visible = true;
                    }
                }
                else
                {
                    gvTabularFormat.Columns[j + 2].Visible = false;
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

    //private void bindReportList()
    //{
    //    clsIVF objivf = new clsIVF(sConString);


    //    try
    //    {
    //        ddlReport.Items.Clear();
    //        ddlReport.Text = "";

    //        DataSet ds = objivf.getEMRTemplateReportSetup(0, common.myInt(ViewState["PageId"]),
    //                            common.myInt(ViewState["DoctorId"]), "T", 1, common.myInt(Session["HospitalLocationID"]),
    //                            common.myInt(Session["UserID"]));

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlReport.Visible = true ;
    //            btnPrintTemplate.Visible = true ;

    //            ddlReport.DataSource = ds.Tables[0];
    //            ddlReport.DataTextField = "ReportName";
    //            ddlReport.DataValueField = "ReportId";
    //            ddlReport.DataBind();
    //            ddlReport.SelectedIndex = 0;
    //        }
    //        else
    //        {
    //            ddlReport.Visible = false ;
    //            btnPrintTemplate.Visible = false;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    private void bindReportList()
    {
        clsIVF objivf = new clsIVF(sConString);
        try
        {
            ddlReport.Items.Clear();
            ddlReport.Text = "";

            DataSet ds = objivf.getEMRTemplateReportSetup(0, common.myInt(ViewState["PageId"]),
                                common.myInt(ViewState["DoctorId"]), "T", 1, common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["UserID"]));

            ddlReport.Items.Clear();
            RadComboBoxItem item;
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlReport.Visible = true;
                btnPrintTemplate.Visible = true;

                //ddlReport.DataSource = ds.Tables[0];
                //ddlReport.DataTextField = "ReportName";
                //ddlReport.DataValueField = "ReportId";
                //ddlReport.DataBind();
                //ddlReport.SelectedIndex = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    item = new RadComboBoxItem();
                    item.Text = common.myStr(dr["ReportName"]);
                    item.Value = common.myInt(dr["ReportId"]).ToString();
                    item.Attributes.Add("SSRSReportName", common.myStr(dr["SSRSReportName"]));
                    item.Attributes.Add("IsSSRS", common.myStr(dr["IsSSRS"]));

                    ddlReport.Items.Add(item);

                }
                ddlReport.SelectedIndex = 0;
            }
            else
            {
                ddlReport.Visible = false;
                btnPrintTemplate.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    /// <summary>
    /// will Enable or Disable Tabuler Control based on Employee Type ID
    /// </summary>
    /// <param name="EmployeeTypeID"></param>
    /// <returns></returns>
    public bool IsEdit_NoTab(bool IsEmployeeTypeTagged, int EmployeeTypeID)
    {
        if (!IsEmployeeTypeTagged)
        {
            return false;
        }
        else
        {
            if (EmployeeTypeID.Equals(0) || EmployeeTypeID.Equals(common.myInt(Session["EmployeeTypeID"])))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //if (EmployeeTypeID == common.myInt(Session["EmployeeTypeID"]))
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}

        //int IsEditable_NoTab = 0;

        //if (EmployeeTypeID != "")
        //{
        //    if (common.myInt(EmployeeTypeID) == common.myInt(Session["EmployeeTypeID"]))
        //    {
        //        IsEditable_NoTab = 1;
        //    }
        //    else
        //    {
        //        if (common.myInt(EmployeeTypeID) == 0)
        //        {
        //            IsEditable_NoTab = 1;
        //        }
        //        else
        //        {
        //            IsEditable_NoTab = 0;
        //        }
        //    }
        //}
        //else
        //{
        //    if (EmployeeTypeID == "0" || EmployeeTypeID == "")
        //    {
        //        IsEditable_NoTab = 1;
        //    }
        //}
        //if (IsEditable_NoTab > 0)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    public bool IsEdit(int FieldID)
    {
        if (ViewState["TabularMandatoryField"] != null)
        {
            DataTable dt = (DataTable)ViewState["TabularMandatoryField"];
            DataView dv = new DataView(dt);
            dv.RowFilter = "FieldId=" + FieldID;

            if (dv.ToTable().Rows.Count > 0)
            {
                if (common.myBool(common.myInt(dv.ToTable().Rows[0]["IsEmployeeTypeTagged"])))
                {
                    if (common.myInt(dv.ToTable().Rows[0]["EmployeeTypeId"]).Equals(0) || common.myInt(dv.ToTable().Rows[0]["EmployeeTypeId"]).Equals(common.myInt(Session["EmployeeTypeID"])))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }


        //int IsEditable = 0;
        //if (ViewState["TabularMandatoryField"] != null)
        //{
        //    DataTable dt = (DataTable)ViewState["TabularMandatoryField"];
        //    DataView dv = new DataView(dt);
        //    dv.RowFilter = "FieldId=" + FieldID;
        //    if (dv.ToTable().Rows.Count > 0)
        //    {
        //        if (dv.ToTable().Rows[0]["EmployeeTypeId"].ToString() != "")
        //        {
        //            if (common.myInt(dv.ToTable().Rows[0]["EmployeeTypeId"].ToString()) == common.myInt(Session["EmployeeTypeID"]))
        //            {
        //                IsEditable = 1;
        //            }
        //            else
        //            {
        //                IsEditable = 0;
        //            }
        //        }
        //        else
        //        {
        //            if (dv.ToTable().Rows[0]["EmployeeTypeId"].ToString() == "0" || dv.ToTable().Rows[0]["EmployeeTypeId"].ToString() == "")
        //            {
        //                IsEditable = 1;
        //            }
        //        }
        //    }
        //}
        //if (IsEditable > 0)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    //Added on 16-04-2014 End  Naushad  Ali    

    protected void btnAddRowInTabular_Onclick(object sender, EventArgs e)
    {
        addRowTabular();
    }

    void showgridcontrols()
    {
        try
        {
            int flag = 0;

            for (int i = 0, k = 0; i < gvTabularFormat.Items.Count; i++, k++)
            {
                if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                {
                    flag = 0;
                }
                else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                {
                    if (flag.Equals(0))
                    {
                        ViewState["ValueOfControlType"] = i - 1;
                        flag = 1;
                    }

                    for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++) //change 5 to 6
                    {
                        Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j].FindControl("lblFieldId" + j.ToString());

                        if (common.myStr(lblControlType.Text).Equals("L"))
                        {
                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());

                            TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());
                            txtVisData.Visible = true;
                            txtVisData.Text = lblControlType.Text;

                            txtVisData.BorderColor = System.Drawing.Color.Transparent;
                            txtVisData.BorderStyle = BorderStyle.None;
                            txtVisData.Font.Bold = true;
                            txtVisData.ForeColor = System.Drawing.Color.FromArgb(40, 80, 230);
                            txtVisData.ToolTip = "Add New Record - " + txtVisData.Text;
                            txtVisData.Style["ReadOnly"] = "true";
                            txtVisData.Style["disabled"] = "true";

                            //txtVisData.Attributes.Add("onclick", "return addRowInTabular('" + common.myInt(gvTabularFormat.Rows[i].Cells[(byte)enumT.Id].Text).ToString() + "');");

                            txtVisData.Columns = 18;

                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                txtVisData.Enabled = false;
                                btnAddRow.Enabled = false;
                                btnFormulaCalculate.Enabled = false;
                            }
                            else
                            {
                                txtVisData.Enabled = true;
                                btnAddRow.Enabled = true;
                                btnFormulaCalculate.Enabled = true;
                            }
                        }
                        //Code Added on-12-Feb-2014
                        //if (lblControlType.Text == "Y")
                        //{

                        //    lblControlType = (Label)gvTabularFormat.Rows[i].Cells[j].FindControl("lblFieldId" + j.ToString());


                        //    HyperLink HyperLinkData = (HyperLink)gvTabularFormat.Rows[i].Cells[j].FindControl("Y");

                        //    HyperLinkData.Text  = "C";

                        //    HyperLinkData.Visible = true;
                        //    HyperLinkData.Enabled = true;   
                        //    //txtVisData.Visible = true;
                        //    //txtVisData.Text = lblControlType.Text;
                        //    //txtVisData.ToolTip = lblControlType.Text;
                        //    //txtVisData.Enabled = true;

                        //}
                        if (common.myStr(lblControlType.Text).Equals("T"))
                        {
                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());

                            TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());
                            txtVisData.Visible = true;
                            txtVisData.Text = lblControlType.Text;
                            txtVisData.ToolTip = lblControlType.Text;
                            txtVisData.Enabled = true;
                            // Button btnHelp = (Button)gvTabularFormat.Rows[i].Cells[j].FindControl("btnHelp" + j.ToString());
                            // btnHelp.Attributes.Add("onclick", "openRadWindow('" + txtVisData.ClientID + "','T')");

                            if (dtFieldMaxLength != null)
                            {
                                if (dtFieldMaxLength.Rows.Count > 0)
                                {
                                    Label fieldid = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                                    dtFieldMaxLength.DefaultView.RowFilter = "FieldId=" + common.myInt(fieldid.Text);
                                    if (dtFieldMaxLength.DefaultView.Count > 0)
                                    {
                                        txtVisData.Columns = (common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]) > 30) ? 30 : common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]);
                                        txtVisData.MaxLength = common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]);
                                    }
                                    dtFieldMaxLength.DefaultView.RowFilter = string.Empty;
                                }
                            }
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                txtVisData.Enabled = false;
                            }
                            else
                            {
                                txtVisData.Enabled = true;
                            }


                            Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            if (IsEdit(common.myInt(lblFieldId.Text)))
                            {
                                txtVisData.Enabled = true;
                            }
                            else
                            {
                                txtVisData.Enabled = false;
                            }

                            if (btnFormulaCalculate.Visible)
                            {
                                IsFormulaField(common.myInt(lblFieldId.Text), ((i >= (gvTabularFormat.Items.Count - 2)) ? true : false), ref txtVisData);
                            }
                            if (common.myLen(txtVisData.Text).Equals(0) && !btnFormulaCalculate.Visible)
                            {
                                if (ViewState["TabularMandatoryField"] != null)
                                {
                                    DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];
                                    try
                                    {
                                        if (tblRefField != null)
                                        {
                                            if (tblRefField.Rows.Count > 0)
                                            {
                                                DataView dvRefField = new DataView(tblRefField);
                                                dvRefField.RowFilter = "FieldId='" + common.myInt(lblFieldId.Text) + "' AND FieldType='T' AND IsDataUpdated=0";
                                                if (dvRefField.ToTable().Rows.Count > 0)
                                                {
                                                    string strVal = string.Empty;
                                                    if (getGenderValue(common.myInt(Session["Gender"])).Equals("M"))
                                                    {
                                                        strVal = common.myStr(dvRefField.ToTable().Rows[0]["DefaultTextMale"]);
                                                    }
                                                    else
                                                    {
                                                        strVal = common.myStr(dvRefField.ToTable().Rows[0]["DefaultTextFemale"]);
                                                    }
                                                    if (!strVal.Equals(string.Empty))
                                                    {
                                                        txtVisData.Text = strVal;
                                                    }
                                                }
                                                dvRefField.Dispose();
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    finally
                                    {
                                        tblRefField.Dispose();
                                    }
                                }
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("M"))
                        {
                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());
                            TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtM" + j.ToString());

                            txtVisData.Visible = true;
                            txtVisData.Text = lblControlType.Text;
                            txtVisData.ToolTip = lblControlType.Text;

                            ////Reference Link

                            Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                            HyperLink hypLink = (HyperLink)gvTabularFormat.Items[i].Cells[j].FindControl("hypLink" + j.ToString());
                            bool bLinkTrue = false;
                            hypLink.Visible = false;
                            if (ViewState["TabularMandatoryField"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["TabularMandatoryField"];
                                if (dt != null)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        DataView dv = new DataView(dt);
                                        dv.RowFilter = "FieldId='" + common.myStr(lblFieldId.Text) + "'";
                                        if (dv.ToTable().Rows.Count > 0)
                                        {
                                            if (common.myBool(dv.ToTable().Rows[0]["IsLinkRequire"]))
                                            {
                                                bLinkTrue = true;
                                                hypLink.Text = "Ref.";
                                                hypLink.ToolTip = "Click for View the Page";
                                                if (IsEdit(common.myInt(lblFieldId.Text)))
                                                {
                                                    hypLink.Attributes.Add("onclick", "openNewWindowExternlink('" + dv.ToTable().Rows[0]["LinkUrl"] + "')");
                                                }
                                            }
                                        }
                                        dv.Dispose();
                                    }
                                }
                                dt.Dispose();
                            }

                            if (bLinkTrue)
                            {
                                hypLink.Visible = true;
                            }
                            ////End
                            // Button btnHelp = (Button)gvTabularFormat.Rows[i].Cells[j].FindControl("btnHelp" + j.ToString());

                            // btnHelp.Attributes.Add("onclick", "openRadWindow('" + txtVisData.ClientID + "','M')");
                            //btnHelp.Visible = true;
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                txtVisData.Enabled = false;
                                //btnHelp.Enabled = false;
                            }
                            else
                            {
                                //btnHelp.Enabled = false;
                                txtVisData.Enabled = true;
                            }
                            //Label lblFieldId = (Label)gvTabularFormat.Rows[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                            if (IsEdit(common.myInt(lblFieldId.Text)))
                            {
                                txtVisData.Enabled = true;
                            }
                            else
                            {
                                txtVisData.Enabled = false;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("D"))
                        {
                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());

                            Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("D" + j.ToString());

                            ddlVisData.Visible = true;

                            ddlVisData.DataSource = null;
                            ddlVisData.DataBind();
                            DropDownItems.RowFilter = "FieldId='" + common.myStr(lblFieldId.Text) + "'";
                            if (DropDownItems != null)
                            {
                                if (DropDownItems.ToTable().Rows.Count > 0)
                                {
                                    ddlVisData.DataSource = DropDownItems;
                                    ddlVisData.DataTextField = "ValueName";
                                    ddlVisData.DataValueField = "ValueId";

                                    ddlVisData.DataBind();
                                }
                            }

                            ListItem l1 = new ListItem(string.Empty, string.Empty);
                            ddlVisData.Items.Insert(0, l1);

                            ddlVisData.SelectedIndex = ddlVisData.Items.IndexOf(ddlVisData.Items.FindByValue(common.myStr(lblControlType.Text)));

                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                ddlVisData.Enabled = false;
                            }
                            else
                            {
                                ddlVisData.Enabled = true;
                            }
                            //Label lblFieldId = (Label)gvTabularFormat.Rows[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                            if (IsEdit(common.myInt(lblFieldId.Text)))
                            {
                                ddlVisData.Enabled = true;
                            }
                            else
                            {
                                ddlVisData.Enabled = false;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("IM"))
                        {
                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());

                            Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            RadComboBox ddlVisData = (RadComboBox)gvTabularFormat.Items[i].Cells[j].FindControl("IM" + j.ToString());

                            ddlVisData.Visible = true;

                            DropDownItems.RowFilter = "FieldId='" + common.myStr(lblFieldId.Text) + "'";

                            if (DropDownItems.ToTable().Rows.Count > 0)
                            {
                                foreach (DataRow drImage in DropDownItems.ToTable().Rows)
                                {
                                    RadComboBoxItem item = new RadComboBoxItem();
                                    item.Text = (string)drImage["ValueName"];
                                    item.Value = drImage["ValueId"].ToString();
                                    item.ImageUrl = drImage["ImagePath"].ToString();
                                    ddlVisData.Items.Add(item);
                                    item.DataBind();
                                }
                            }

                            ddlVisData.SelectedIndex = ddlVisData.Items.IndexOf(ddlVisData.Items.FindItemByValue(common.myStr(lblControlType.Text)));

                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                ddlVisData.Enabled = false;
                            }
                            else
                            {
                                ddlVisData.Enabled = true;
                            }
                            //Label lblFieldId = (Label)gvTabularFormat.Rows[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                            if (IsEdit(common.myInt(lblFieldId.Text)))
                            {
                                ddlVisData.Enabled = true;
                            }
                            else
                            {
                                ddlVisData.Enabled = false;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("B"))
                        {
                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());
                            Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("B" + j.ToString());
                            ddlVisData.Visible = true;

                            ddlVisData.SelectedIndex = ddlVisData.Items.IndexOf(ddlVisData.Items.FindByValue(common.myStr(lblControlType.Text)));
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                ddlVisData.Enabled = false;
                            }
                            else
                            {
                                ddlVisData.Enabled = true;
                            }

                            //Label lblFieldId = (Label)gvTabularFormat.Rows[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                            if (IsEdit(common.myInt(lblFieldId.Text)))
                            {
                                ddlVisData.Enabled = true;
                            }
                            else
                            {
                                ddlVisData.Enabled = false;
                            }
                        }
                        else if (common.myStr(lblControlType.Text).Equals("S"))
                        {
                            Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());

                            HtmlTable hhtVisData = (HtmlTable)gvTabularFormat.Items[i].Cells[j].FindControl("tblDate" + j.ToString());
                            hhtVisData.Visible = true;

                            AjaxControlToolkit.CalendarExtender CEDate = (AjaxControlToolkit.CalendarExtender)gvTabularFormat.Items[i].Cells[j].FindControl("CalendarExtender" + j.ToString());

                            TextBox txtDate = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtDate" + j.ToString());
                            //txtDate.Text = lblControlType.Text;

                            txtDate.Text = string.Empty;
                            if (common.myStr(lblControlType.Text).Trim().Length > 0)
                            {
                                //txtDate.Text = common.myDate(lblControlType.Text).ToString("MM/dd/yyyy");
                                txtDate.Text = lblControlType.Text.Trim();
                            }
                            if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                            {
                                CEDate.Enabled = false;
                                txtDate.Enabled = false;
                            }
                            else
                            {
                                CEDate.Enabled = true;
                                txtDate.Enabled = true;
                            }
                            //Label lblFieldId = (Label)gvTabularFormat.Rows[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                            if (IsEdit(common.myInt(lblFieldId.Text)))
                            {
                                CEDate.Enabled = true;
                                txtDate.Enabled = true;
                            }
                            else
                            {
                                CEDate.Enabled = false;
                                txtDate.Enabled = false;
                            }
                            //Added  on 17-04-2014 End Naushad Ali
                        }
                    }
                }
            }
            if (btnFormulaCalculate.Visible)
            {
                gvTabularFormat.Visible = true;
                btnFormulaCalculate_OnClick(null, null);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnLock_OnClick(object sender, EventArgs e)
    {
        DL_Funs fun = new DL_Funs();
        try
        {
            if (!common.myInt(ViewState["EncounterId"]).Equals(0)
                && !common.myInt(Session["formId"]).Equals(0)
                && !common.myInt(Session["HospitalLocationId"]).Equals(0)
                && !common.myStr(Session["CurrentNode"]).Equals(string.Empty))
            {
                //int lck = fun.DoLockUnLock("Upd", common.myStr(Session["encounterid"]), common.myStr(Session["HospitalLocationId"]), common.myStr(Session["formId"]), common.myStr(Session["CurrentNode"]).Substring(1, common.myLen(Session["CurrentNode"]) - 1), btnLock.Text);
                int lck = fun.DoLockUnLock("Upd", common.myInt(ViewState["EncounterId"]).ToString(), common.myInt(Session["HospitalLocationId"]).ToString(),
                                    common.myInt(Session["formId"]).ToString(), common.myStr(ddlTemplateMain.SelectedValue), btnLock.Text);

                if (btnLock.Text.Equals("Lock"))
                {
                    lblMessage.Text = "Locked successfully";
                    btnLock.Text = "Unlock";
                    tblMainTable.Disabled = true;
                    gvSelectedServices.Enabled = false;
                    gvTabularFormat.Enabled = false;
                    //btnSetDefault.Enabled = false;
                    btnSave.Visible = false;
                }
                else
                {
                    lblMessage.Text = "Unlocked successfully";
                    tblMainTable.Disabled = false;
                    gvSelectedServices.Enabled = true;
                    gvTabularFormat.Enabled = true;
                    //btnSetDefault.Enabled = true;
                    btnSave.Visible = true;
                    btnLock.Text = "Lock";
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

    protected void Select_Lock_Unlock()
    {
        DL_Funs fun = new DL_Funs();
        try
        {
            if (!common.myInt(ViewState["EncounterId"]).Equals(0)
                && !common.myInt(Session["formId"]).Equals(0)
                && !common.myInt(Session["HospitalLocationId"]).Equals(0)
                && !common.myStr(Session["CurrentNode"]).Equals(string.Empty))
            {
                //int lck = fun.DoLockUnLock("Sel", common.myStr(Session["encounterid"]), common.myStr(Session["HospitalLocationId"]), common.myStr(Session["formId"]), common.myStr(Session["CurrentNode"]).Substring(1, common.myLen(Session["CurrentNode"]) - 1), btnLock.Text);
                int lck = fun.DoLockUnLock("Sel", common.myInt(ViewState["EncounterId"]).ToString(), common.myInt(Session["HospitalLocationId"]).ToString(),
                                    common.myInt(Session["formId"]).ToString(), common.myStr(ddlTemplateMain.SelectedValue), btnLock.Text);

                if (lck.Equals(0))
                {
                    btnLock.Text = "Lock";
                    tblMainTable.Disabled = false;
                    tblMainTable.Style.Add("disabled", "false");
                    gvSelectedServices.Enabled = true;
                    gvTabularFormat.Enabled = true;
                    //btnSetDefault.Enabled = true;
                    btnSave.Visible = true;
                }
                else
                {
                    btnLock.Text = "Unlock";
                    tblMainTable.Disabled = true;
                    gvSelectedServices.Enabled = false;
                    gvTabularFormat.Enabled = false;
                    //btnSetDefault.Enabled = false;
                    btnSave.Visible = false;
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

    protected void tvCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        //try
        //{
        lblMessage.Text = string.Empty;
        int RowStartIndex = 1;
        ArrayList arr;
        int flag = 0;

        if (common.myStr(ViewState["PreviousSelectedNode"]).Trim().Equals(string.Empty))
        {
            ViewState["PreviousSelectedNode"] = common.myStr(tvCategory.SelectedNode.Value).Trim();

            arr = GetSelectedNode(tvCategory.SelectedNode.Value);
        }
        else
        {
            ViewState["PreviousSelectedNode"] = ((common.myStr(ViewState["SelectedNodeIsTabular"]).Equals("F")) ? "P" : common.myStr(ViewState["SelectedNodeIsTabular"])) + common.myStr(ViewState["SelectedNode"]);

            //Added on 22-04-2014  By naushad
            //if (!isSaved(false))
            //{
            //    //tvCategory.SelectedNodeStyle.BackColor = System.Drawing.Color.White;
            //    //tvCategory.SelectedNodeStyle.ForeColor = System.Drawing.Color.Gray;

            //    //arr = GetSelectedNode(tvCategory.SelectedNode.Value);

            //    //tvCategory.SelectedNode.Value = common.myStr(ViewState["PreviousSelectedNode"]);

            //    //ViewState["PreviousSelectedNode"] = common.myStr(tvCategory.SelectedNode.Value);

            //    //tvCategory.SelectedNodeStyle.BackColor = System.Drawing.Color.Gray;
            //    //tvCategory.SelectedNodeStyle.ForeColor = System.Drawing.Color.White;

            //    return;
            //}
            //Added on 22-04-2014  By naushad

            if (tvCategory.SelectedNode != null)
                arr = GetSelectedNode(tvCategory.SelectedNode.Value);
            else
            {
                arr = GetSelectedNode(ViewState["PreviousSelectedNode"].ToString());
            }
        }
        gvSelectedServices.Enabled = false;
        gvTabularFormat.Enabled = false;

        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        bool IsAddendum = objEMR.IsSectionAddendum(common.myInt(ViewState["SelectedNode"]));

        if (IsAddendum)
        {
            if (common.myBool(Request.QueryString["IsAddendum"]))
            {
                gvTabularFormat.Enabled = true;
                gvSelectedServices.Enabled = true;
                btnSave.Visible = true;
            }
        }
        else
        {
            if (!common.myBool(Request.QueryString["IsAddendum"]))
            {
                gvTabularFormat.Enabled = true;
                gvSelectedServices.Enabled = true;
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") == true && common.myBool(Session["isEMRSuperUser"]) == false)
                {
                    btnSave.Visible = false;
                }

            }
            else
            {
                btnSave.Visible = false;
            }

        }

        DataRow[] dr = null;

        if (common.myStr(ViewState["SelectedNodeIsTabular"]).Trim().Equals("T"))
        {
            if (!isFirstTime)
            {
                if (!common.myStr(ddlTemplateMain.SelectedItem.Attributes["EntryType"]).Equals("E"))
                {
                    saveData(false);
                }
                else if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["EntryType"]).Equals("E"))
                {
                    if (!common.myInt(ddlEpisode.SelectedValue).Equals(0))
                    {
                        saveData(false);
                    }
                }
            }

            if (!AddRow)
            {
                BindBothGrids(true, false);
                //AddRow = false;
                // btnAddRow_Click(null, null);
            }
            showgridcontrols();

            if (dtNoOfColumn == null)
            {
                return;
            }
            else
            {
                if (dtNoOfColumn.Rows.Count.Equals(0))
                {
                    return;
                }
            }

            dr = dtNoOfColumn.Select("SectionId=" + ViewState["SelectedNode"]);

            btnAddRow.Visible = false;
            btnFormulaCalculate.Visible = false;

            if (gvTabularFormat.Items.Count > 0)
            {
                if (dr.Length > 0)
                {
                    btnAddRow.Visible = common.myBool(ViewState["AllowNewRow"]);

                    btnFormulaCalculate.Visible = false;
                    if (ViewState["TabularMandatoryField"] != null)
                    {
                        DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];

                        if (tblRefField != null)
                        {
                            if (tblRefField.Rows.Count > 0)
                            {
                                tblRefField.DefaultView.RowFilter = "LEN(ISNULL(ReferenceName,''))>0 OR ISNULL(TotalCalc,0)=1";
                                if (tblRefField.DefaultView.Count > 0)
                                {
                                    btnFormulaCalculate.Visible = true;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < gvTabularFormat.Items.Count; i++)
                {
                    if (common.myInt(common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text).Trim()).Equals(common.myInt(ViewState["SelectedNode"])))
                    {
                        if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                        {
                            //gvTabularFormat.Rows[i].Visible = true;
                            if (flag.Equals(0))
                            {
                                RowStartIndex = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.Id].Text);
                            }

                            if (flag < 3)
                            {
                                gvTabularFormat.Items[i].Visible = false;
                            }
                        }
                        else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                        {
                            gvTabularFormat.Items[i].Visible = true;
                        }
                        flag++;
                    }
                    else
                    {
                        gvTabularFormat.Items[i].Visible = false;
                    }
                }

                for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++) //change 5 to 6
                {
                    Label lblHeading = (Label)gvTabularFormat.Items[RowStartIndex].Cells[j].FindControl("lblFieldId" + j.ToString());

                    GridHeaderItem headerItem = gvTabularFormat.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                    headerItem.Cells[j + 2].Text = common.myStr(lblHeading.Text);

                    DataTable tbl = new DataTable();
                    if (ViewState["TabularMandatoryField"] != null)
                    {
                        tbl = (DataTable)ViewState["TabularMandatoryField"];
                        if (tbl.Rows.Count > 0)
                        {
                            Label lblFieldId = (Label)gvTabularFormat.Items[RowStartIndex - 1].Cells[j].FindControl("lblFieldId" + j.ToString());

                            DataView DV = tbl.DefaultView;
                            DV.RowFilter = "FieldId=" + common.myInt(lblFieldId.Text) + " AND IsMandatory=1";

                            if (DV.ToTable().Rows.Count > 0)
                            {
                                headerItem.Cells[j + 2].Text = common.myStr(lblHeading.Text) + hdnMandatoryStar.Value;
                            }
                            DV.RowFilter = string.Empty;
                        }
                    }

                    headerItem.Cells[j + 2].Font.Size = 10;


                    if (!lblHeading.Text.Trim().Equals("+"))
                    {
                        if (dr.Length > 0)
                        {
                            if (j >= common.myInt(dr[0][1]))
                            {
                                gvTabularFormat.Columns[j + 2].Visible = false;
                            }
                            else
                            {
                                Label lblControlType = (Label)gvTabularFormat.Items[2].Cells[j].FindControl("lblFieldId" + j.ToString());
                                if (lblControlType != null)
                                {
                                    if (common.myStr(lblControlType.Text).Equals("T"))
                                    {
                                        if (dtFieldMaxLength != null)
                                        {
                                            if (dtFieldMaxLength.Rows.Count > 0)
                                            {
                                                int FieldId = common.myInt(((Label)gvTabularFormat.Items[RowStartIndex - 1].Cells[j].FindControl("lblFieldId" + j.ToString())).Text);
                                                dtFieldMaxLength.DefaultView.RowFilter = "FieldId=" + common.myInt(FieldId);
                                                if (dtFieldMaxLength.DefaultView.Count > 0)
                                                {
                                                    int maxVal = getPixelValue(common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]));

                                                    gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(maxVal);
                                                    gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(maxVal);
                                                }

                                                dtFieldMaxLength.DefaultView.RowFilter = "";
                                            }
                                        }
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("M"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(260);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(260);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("S"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(140);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(140);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("D"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(120);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(120);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("IM"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(120);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(120);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("C"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(155);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(155);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("W"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(500);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(500);
                                    }
                                }

                                gvTabularFormat.Columns[j + 2].Visible = true;
                                gvTabularFormat.Columns[j + 2].HeaderText = lblHeading.Text;
                                gvTabularFormat.Columns[j].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                                gvTabularFormat.Columns[j].ItemStyle.VerticalAlign = VerticalAlign.Top;
                            }
                        }
                        else
                        {
                            gvTabularFormat.Columns[j + 2].Visible = false;
                        }
                    }
                    else
                    {
                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(150);
                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(150);
                    }

                }
                //divTabularFormat.Visible = true;
                gvTabularFormat.Visible = true;

                gvSelectedServices.Visible = false;
            }
            // btnAddRow_Click(null, null);
        }
        else
        {
            if (!isFirstTime)
            {
                if (!common.myStr(ddlTemplateMain.SelectedItem.Attributes["EntryType"]).Equals("E"))
                {
                    saveData(false);
                }
                else if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["EntryType"]).Equals("E"))
                {
                    if (!common.myInt(ddlEpisode.SelectedValue).Equals(0))
                    {
                        saveData(false);
                    }
                }
            }
            BindBothGrids(false, false);
            btnAddRow.Visible = false;
            btnFormulaCalculate.Visible = false;

            //divTabularFormat.Visible = false;
            gvTabularFormat.Visible = false;

            gvSelectedServices.Visible = true;

            for (int i = 0; i < gvSelectedServices.Rows.Count; i++)
            {
                if (common.myInt(gvSelectedServices.Rows[i].Cells[(byte)enumNonT.SectionId].Text).Equals(common.myInt(ViewState["SelectedNode"])))
                {
                    if (common.myInt(gvSelectedServices.Rows[i].Cells[(byte)enumNonT.ParentId].Text) > 0)
                    {
                        DataView DV = new DataView();
                        if (objDs != null)
                        {
                            if (objDs.Tables.Count > 0)
                            {
                                DV = objDs.Tables[2].DefaultView;

                                DV.RowFilter = "FieldId=" + common.myInt(gvSelectedServices.Rows[i].Cells[(byte)enumNonT.ParentId].Text).ToString() + " and FieldValue=" + common.myInt(common.myStr(gvSelectedServices.Rows[i].Cells[(byte)enumNonT.ParentValue].Text).Trim());

                                if (DV.ToTable().Rows.Count > 0)
                                {
                                    gvSelectedServices.Rows[i].Visible = true;
                                    gvSelectedServices.Rows[i].Style["display"] = "";
                                }
                                else
                                {
                                    //gvSelectedServices.Rows[i].Visible = false;
                                    gvSelectedServices.Rows[i].Style["display"] = "none";
                                }
                            }
                        }
                    }
                    else
                    {
                        gvSelectedServices.Rows[i].Visible = true;
                        gvSelectedServices.Rows[i].Style["display"] = "";
                    }
                }
                else
                {
                    //gvSelectedServices.Rows[i].Visible = false;
                    gvSelectedServices.Rows[i].Style["display"] = "none";
                }
            }
        }
        //if (tvCategory.SelectedNode == null)
        //{
        //TreeNode node2 = SelectNode(ViewState["SelectedCategory"].ToString(), tvCategory.Nodes);
        //if (node2 != null)
        //{
        //    node2.Selected = true;
        //}
        //}
        //else

        if (common.myStr(ViewState["SelectedNodeIsTabular"]).Trim().Equals("T"))
        {
            ViewState["SelectedCategory"] = tvCategory.SelectedNode.Text;
        }
        else
        {
            //if (common.myInt(ddlResultSet.SelectedValue) > 0)
            //{
            //    if (ViewState["SelectedCategory"] != null)
            //        tvCategory.SelectedNode.Text = common.myStr(ViewState["SelectedCategory"]);
            //}
        }
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }

    private int getPixelValue(int maxLenth)
    {
        int pixVal = 110;

        if (!btnFormulaCalculate.Visible)
        {
            if (maxLenth < 10)
            {
                pixVal = 50 + ((maxLenth - 1) * 6);
            }
            else if (maxLenth > 30)
            {
                pixVal = 225;
            }
            else
            {
                pixVal = 105 + ((maxLenth - 10) * 6);
            }
        }

        return pixVal;
    }

    TreeNode node { get; set; }
    private TreeNode SelectNode(string nodetext, TreeNodeCollection parentCollection)
    {
        foreach (TreeNode childnode in parentCollection)
        {
            if (common.myStr(childnode.Text).Equals(common.myStr(nodetext)))
            {
                node = childnode;
            }
            if ((node != null)) break;
        }
        return node;
    }
    private void BindCategoryTree()
    {
        Hashtable hstInput = new Hashtable();
        try
        {
            tvCategory.Nodes.Clear();

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));

            if (common.myInt(Request.QueryString["TmpId"]) > 0)
            {
                hstInput.Add("@intTemplateId", common.myInt(Request.QueryString["TmpId"]));
                hstInput.Add("chrGenderType", "B");
            }
            else
            {
                hstInput.Add("@intTemplateId", ddlTemplateMain.SelectedValue);
                hstInput.Add("chrGenderType", getGenderValue(common.myInt(Session["Gender"])));
            }

            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTree", hstInput);

            if (ds.Tables.Count > 0)
            {
                DataView dvFreeze = ds.Tables[0].Copy().DefaultView;
                dvFreeze.RowFilter = "Tabular=1 AND IsFreezeFirstColumn=1";

                ViewState["FreezeSectionTree"] = dvFreeze.ToTable().Copy();
            }

            string Tabular = string.Empty;
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];

                    Tabular = ((bool)dr["Tabular"]) ? "T" : string.Empty;

                    AddNodes(tvCategory, common.myStr(dr[0]) + Tabular, common.myInt(dr[2]), common.myStr(dr[1]));
                }
                if (tvCategory.Nodes.Count > 0)
                {
                    if (ViewState["SelectedCategory"] != null)
                    {
                        TreeNode node1 = SelectNode(common.myStr(ViewState["SelectedCategory"]), tvCategory.Nodes);
                        if (node1 != null)
                        {
                            node1.Selected = true;
                        }
                    }
                    else
                    {
                        tvCategory.Nodes[0].Selected = true;
                        tvCategory.CollapseAll();
                        tvCategory.PopulateNodesFromClient = true;
                        tvCategory.ShowLines = true;
                    }
                }
            }
            else
            {
                DataTable dt2 = new DataTable();

                DataColumn dcId = new DataColumn("Id");
                DataColumn dcPropertyId = new DataColumn("FieldId");
                DataColumn dcPropertyValue = new DataColumn("FieldName");
                DataColumn dcPropertyType = new DataColumn("FieldType");
                DataColumn dcMainText = new DataColumn("MainText");
                DataColumn dcRemarks = new DataColumn("Remarks");
                DataColumn dcParentIdId = new DataColumn("ParentId");
                DataColumn dcParentValue = new DataColumn("ParentValue");
                DataColumn dcHierarchy = new DataColumn("Hierarchy");
                DataColumn MaxLength = new DataColumn("MaxLength");
                DataColumn ValueId = new DataColumn("ValueId");
                DataColumn ValueName = new DataColumn("ValueName");
                DataColumn IsMandatory = new DataColumn("IsMandatory");
                DataColumn MandatoryType = new DataColumn("MandatoryType");

                dt2.Columns.Add(dcId);
                dt2.Columns.Add(dcPropertyId);
                dt2.Columns.Add(dcPropertyValue);
                dt2.Columns.Add(dcPropertyType);
                dt2.Columns.Add(dcMainText);
                dt2.Columns.Add(dcRemarks);
                dt2.Columns.Add(dcParentIdId);
                dt2.Columns.Add(dcParentValue);
                dt2.Columns.Add(dcHierarchy);
                dt2.Columns.Add(MaxLength);
                dt2.Columns.Add(ValueId);
                dt2.Columns.Add(ValueName);
                dt2.Columns.Add("SectionID");
                dt2.Columns.Add(IsMandatory);
                dt2.Columns.Add(MandatoryType);

                DataRow dr = dt2.NewRow();
                dr[0] = "0";
                dr[1] = "0";
                dr[2] = "0";
                dr[3] = "0";
                dr[4] = "0";
                dr[5] = "0";
                dr[6] = "0";
                dr[7] = "0";
                dr[8] = "0";
                dr[9] = "0";
                dr[10] = "0";
                dr[11] = "0";
                dr[12] = string.Empty;
                dr[13] = "False";
                dr[14] = string.Empty;

                dt2.Rows.Add(dr);

                ViewState["SelectedNode"] = 0;

                ViewState["NonTabularData"] = null;

                gvSelectedServices.DataSource = dt2;
                gvSelectedServices.DataBind();

                gvSelectedServices.Rows[0].Visible = false;
                //btnSetDefault.Enabled = false;
                //btnUndoChanges.Enabled = false;
                btnSave.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //DropDownList ddl = sender as DropDownList;
            //GridViewRow row = ddl.NamingContainer as GridViewRow;
            //RadEditor txtW = (RadEditor)row.FindControl("txtW");
            //DataSet ds = new DataSet();
            //Hashtable hs = new Hashtable();
            //dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //hs.Add("@intFormatId", common.myInt(ddl.SelectedValue));

            //ds = dl.FillDataSet(CommandType.Text, "SELECT FormatText FROM EMRTemplateFieldFormats WITH (NOLOCK) WHERE FormatId = @intFormatId AND Active = 1", hs);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    txtW.Content = common.myStr(ds.Tables[0].Rows[0]["FormatText"]);
            //}
            //else
            //{
            //    txtW.Content = string.Empty;
            //}
            //ddl.Focus();

            RadComboBox ddl = sender as RadComboBox;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadEditor txtW = (RadEditor)row.FindControl("txtW");

            txtW.Content = common.myStr(ddl.SelectedItem.Attributes["FormatText"]);
            ddl.Focus();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }


    protected void B_OnClick(object sender, EventArgs e)
    {
        try
        {
            RadioButtonList ddlB = sender as RadioButtonList;
            if (ddlB == null)
            {
                return;
            }
            bool IsFocusSet = false;

            GridViewRow row = ddlB.NamingContainer as GridViewRow;
            foreach (GridViewRow r in gvSelectedServices.Rows)
            {
                string ddlBClientID = common.myStr(ddlB.ClientID);

                string i = common.myStr(ddlB.Items.Count - 1);
                if (common.myInt(r.Cells[(byte)enumNonT.ParentId].Text).Equals(common.myInt(row.Cells[(byte)enumNonT.FieldId].Text)))
                {
                    if (common.myStr(row.Cells[(byte)enumNonT.FieldType].Text).Equals("B") || common.myStr(row.Cells[(byte)enumNonT.FieldType].Text).Equals("R"))
                    {
                        if (common.myInt(ddlB.SelectedValue).Equals(common.myInt(r.Cells[(byte)enumNonT.ParentValue].Text)))
                        {
                            r.Visible = true;
                            ddlB.Focus();

                            //ddlB.Items.FindByValue(i).Selected = true;
                            //ListItem selectedItem = ddlB.SelectedItem;
                            //selectedItem.Attributes.Add("onload", "this.focus();");

                            //SetFocus(r);

                            SetFocus(ddlBClientID + "_" + ddlB.SelectedIndex);
                            //ctl00_ContentPlaceHolder1_gvSelectedServices_ctl05_R_1
                            //ctl00_ContentPlaceHolder1_gvSelectedServices_ctl06_R_2

                            IsFocusSet = true;
                        }
                        else
                        {
                            r.Visible = false;
                            // SetFocus(r);
                            SetFocus(ddlBClientID + "_" + ddlB.SelectedIndex);
                            IsFocusSet = true;
                        }
                    }
                    else
                    {
                        ddlB.Focus();
                        // SetFocus("ctl00_ContentPlaceHolder1_gvSelectedServices_ctl21_R_" + ddlB.SelectedIndex);
                        SetFocus(ddlBClientID + "_" + ddlB.SelectedIndex);
                        IsFocusSet = true;
                    }
                }
                else
                {
                    ddlB.Focus();

                    SetFocus(ddlBClientID + "_" + ddlB.SelectedIndex);
                    IsFocusSet = true;
                }
            }
            //ListItem selectedItem = ddlB.SelectedItem;
            //selectedItem.Attributes.Add("onload", "this.focus();");

            if (!IsFocusSet)
            {
                ddlB.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
    }
    protected void D_OnClick(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = sender as DropDownList;
            bool IsFocusSet = false;
            if (ddl == null)
            {
                return;
            }
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            foreach (GridViewRow r in gvSelectedServices.Rows)
            {
                if (common.myInt(r.Cells[(byte)enumNonT.ParentId].Text).Equals(common.myInt(row.Cells[(byte)enumNonT.FieldId].Text)))
                {
                    if (common.myStr(row.Cells[(byte)enumNonT.FieldType].Text).Equals("D"))
                    {
                        DropDownList ddlD = row.FindControl("D") as DropDownList;
                        if (common.myInt(ddlD.SelectedValue).Equals(common.myInt(r.Cells[(byte)enumNonT.ParentValue].Text)))
                        {
                            //row.Cells[9].Text = ddlD.SelectedValue;
                            r.Visible = true;
                            ddlD.Focus();
                            SetFocus(r);
                            IsFocusSet = true;
                        }
                        else
                        {
                            r.Visible = false;
                        }
                    }
                }
            }

            if (!IsFocusSet)
            {
                ddl.Focus();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void BindBothGrids(bool bTabular, bool IsLastVisit)
    {
        DataSet ds;
        ArrayList ar;
        btnScoreCalc.Visible = false;
        try
        {
            if (tvCategory.SelectedNode != null)
                lblGrid.Text = tvCategory.SelectedNode.Text;
            else
            {
                ar = GetSelectedNode(common.myStr(ViewState["PreviousSelectedNode"]));
                lblGrid.Text = common.myStr(ar[0]);
            }

            BaseC.Security AuditCA = new BaseC.Security(sConString);
            string TemplateId = string.Empty;
            string GenderType = string.Empty;

            //if (Request.QueryString["TmpId"] != null)
            //{
            TemplateId = ddlTemplateMain.SelectedValue;

            GenderType = "B";
            //goto Lab;
            //}

            if (common.myInt(Session["HospitalLocationId"]).Equals(0)
                && common.myInt(Session["Gender"]).Equals(0)
                && common.myInt(ViewState["EncounterId"]).Equals(0))
            {
                return;
            }
            else
            {
                //TemplateId = common.myStr(Session["CurrentNode"]).Substring(1, common.myStr(Session["CurrentNode"]).Length - 1);
                TemplateId = getSubString(common.myStr(Session["CurrentNode"]), 1, common.myStr(Session["CurrentNode"]).Length - 1);

                GenderType = getGenderValue(common.myInt(Session["Gender"]));
            }

            //Lab:

            if (tvCategory.SelectedNode != null)
                ar = GetSelectedNode(tvCategory.SelectedNode.Value);
            else
            {
                ar = GetSelectedNode(common.myStr(ViewState["PreviousSelectedNode"]));
            }
            // = GetSelectedNode(common.myStr(tvCategory.SelectedNode.Value));

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (!bTabular)
            {
                ds = new DataSet();
                Hashtable hstInputStatic = new Hashtable();
                hstInputStatic.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                hstInputStatic.Add("@chrGenderType", GenderType);
                hstInputStatic.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
                hstInputStatic.Add("@intTemplateId", ddlTemplateMain.SelectedValue);
                hstInputStatic.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
                //hstInputStatic.Add("@intSectionID", common.myInt(ar[0]));
                if (!common.myStr(Request.QueryString["Data"]).Trim().Equals(string.Empty))
                {
                    hstInputStatic.Add("@bitShowPreviousData", "1");
                }
                if (common.myInt(Request.QueryString["tmpOrder"]) > 0)
                {
                    hstInputStatic.Add("@intOrderId", common.myInt(Request.QueryString["tmpOrder"]));
                }
                hstInputStatic.Add("@intRecordId", common.myInt(ddlRecord.SelectedValue));

                if (IsLastVisit)
                {
                    hstInputStatic.Add("@intResultSetId", 2000000000);
                }
                else
                {
                    hstInputStatic.Add("@intResultSetId", common.myInt(ddlResultSet.SelectedValue));
                }

                if (common.myStr(ViewState["EntryType"]).Equals("E"))
                {
                    if (common.myInt(ddlEpisode.SelectedValue).Equals(0))
                    {
                        hstInputStatic.Add("@intEpisodeId", 1000000000);
                    }
                    else
                    {
                        hstInputStatic.Add("@intEpisodeId", common.myInt(ddlEpisode.SelectedValue));
                    }

                }
                hstInputStatic.Add("@intSectionID", common.myInt(ar[0]));
                //hstInputStatic.Add("@intTemplateId", ddlTemplateMain.SelectedValue);
                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                    && !common.myStr(ViewState["TagType"]).Equals("D"))
                {
                    DataSet dsTemplateDataDetails = new DataSet();
                    StringBuilder strTemplateDataDetailsXML = new StringBuilder();
                    if (Session["TemplateDataDetails"] != null)
                    {
                        dsTemplateDataDetails = (DataSet)Session["TemplateDataDetails"];
                    }

                    if (dsTemplateDataDetails.Tables.Count > 0)
                    {
                        foreach (DataRow DR in dsTemplateDataDetails.Tables[0].Rows)
                        {
                            strTemplateDataDetailsXML.Append(DR["xmlTemplateDetails"]);
                        }
                    }

                    if (!strTemplateDataDetailsXML.ToString().Equals(string.Empty))
                    {
                        hstInputStatic.Add("@xmlTemplateDetails", strTemplateDataDetailsXML.ToString());
                    }

                    if (!common.myStr(ViewState["TagType"]).Equals("D"))
                    {
                        hstInputStatic.Add("@intServiceId", common.myInt(ViewState["TemplateRequiredServiceId"]));
                    }
                    else
                    {
                        hstInputStatic.Add("@intServiceId", common.myInt(ddlTemplateRequiredServices.SelectedValue));
                    }
                    //int serviceOrderDetailId = common.myInt(ddlTemplateRequiredServices.SelectedItem.Attributes["ServiceOrderDetailId"]);
                    //hstInputStatic.Add("@OrderDetailId", serviceOrderDetailId);
                }

                hstInputStatic.Add("@intRequestId", common.myInt(Request.QueryString["RequestId"]));
                hstInputStatic.Add("@intLoginEmployeeId", common.myStr(Session["EmployeeId"]));
                if (!common.myStr(Request.QueryString["SourceForLIS"]).Trim().Equals(string.Empty))
                {
                    if (common.myStr(Request.QueryString["SourceForLIS"]).Trim().Equals("LIS"))
                    {
                        if (common.myInt(Request.QueryString["sOrdDtlId"]).Equals(0)
                            && !common.myStr(ViewState["TagType"]).Equals("L"))
                        {
                            hstInputStatic.Add("@intServiceId", common.myInt(ViewState["ServiceId"]));
                        }
                        //else if (common.myStr(Request.QueryString["SourceForLIS"]).Trim().Equals("LIS")
                        //        && common.myStr(ViewState["TagType"]).Equals("L"))
                        //{
                        //    hstInputStatic.Add("@intServiceId", common.myInt(ViewState["ServiceId"]));
                        //}

                        hstInputStatic.Add("@OrderDetailId", common.myInt(Request.QueryString["sOrdDtlId"]));
                    }
                }
                hstInputStatic.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                hstInputStatic.Add("@chvOPIP", common.myStr(Session["OPIP"]));

                objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInputStatic);

                ViewState["NonTabularData"] = objDs;



                if (objDs != null)
                {
                    if (objDs.Tables.Count > 0)
                    {
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            DataView dvNonTabularData = objDs.Tables[0].Copy().DefaultView;
                            dvNonTabularData.RowFilter = "ISNULL(IsScoreCalc,0)=1";
                            if (dvNonTabularData.ToTable().Rows.Count > 0)
                            {
                                btnScoreCalc.Visible = true;
                            }
                            dvNonTabularData.Dispose();
                        }
                    }
                }

                gvSelectedServices.DataSource = objDs;
                gvSelectedServices.DataBind();

                //ViewState["DisplayIsConfidentialLink"] = null;
                //if (objDs.Tables[2].Rows.Count > 0)
                //{
                //    ViewState["DisplayIsConfidentialLink"] = true;
                //}
            }
            else
            {
                ds = new DataSet();
                DataSet objDsTabular = new DataSet();
                Hashtable hstInputTabular = new Hashtable();
                hstInputTabular.Add("@chrGenderType", GenderType);
                hstInputTabular.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
                hstInputTabular.Add("@intTemplateId", TemplateId);
                if (common.myInt(Request.QueryString["tmpOrder"]) > 0)
                {
                    hstInputTabular.Add("@intOrderId", common.myInt(Request.QueryString["tmpOrder"]));
                }
                hstInputTabular.Add("@intRecordId", common.myInt(ddlRecord.SelectedValue));
                hstInputTabular.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
                hstInputTabular.Add("@intResultSetId", common.myInt(ViewState["ResultSetId"]));

                if (common.myStr(ViewState["EntryType"]).Equals("E"))
                {
                    if (common.myInt(ddlEpisode.SelectedValue).Equals(0))
                    {
                        hstInputTabular.Add("@intEpisodeId", 1000000000);
                    }
                    else
                    {
                        hstInputTabular.Add("@intEpisodeId", common.myInt(ddlEpisode.SelectedValue));
                    }
                }
                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                {
                    //int serviceOrderDetailId = common.myInt(ddlTemplateRequiredServices.SelectedItem.Attributes["ServiceOrderDetailId"]);
                    //hstInputTabular.Add("@OrderDetailId", serviceOrderDetailId);
                }
                hstInputTabular.Add("@intSecId", common.myInt(ar[0]));
                hstInputTabular.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                hstInputTabular.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                hstInputTabular.Add("@intLoginEmployeeId", common.myStr(Session["EmployeeId"]));
                hstInputTabular.Add("@chvOPIP", common.myStr(Session["OPIP"]));

                objDsTabular = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInputTabular);

                ViewState["AllowNewRow"] = false;
                ViewState["TabularRowData"] = null;
                ViewState["TabularMandatoryField"] = null;
                //ViewState["DisplayIsConfidentialLink"] = null;
                //if (objDsTabular.Tables[0].Rows.Count > 3)
                //{
                //    ViewState["DisplayIsConfidentialLink"] = true;
                //}
                dtFieldMaxLength = new DataTable();
                if (objDsTabular.Tables[0].Rows.Count > 0)
                {
                    if (objDsTabular.Tables.Count > 4)
                    {
                        ViewState["TabularMandatoryField"] = objDsTabular.Tables[4];

                        btnFormulaCalculate.Visible = false;
                        if (ViewState["TabularMandatoryField"] != null)
                        {
                            DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];

                            if (tblRefField != null)
                            {
                                if (tblRefField.Rows.Count > 0)
                                {
                                    tblRefField.DefaultView.RowFilter = "LEN(ISNULL(ReferenceName,''))>0 OR ISNULL(TotalCalc,0)=1 ";
                                    if (tblRefField.DefaultView.Count > 0)
                                    {
                                        btnFormulaCalculate.Visible = true;
                                    }
                                    //DataView dvFilter=new DataView();
                                    //try
                                    //{
                                    //    dvFilter = new DataView(tblRefField);
                                    //    dvFilter.RowFilter = "ISNULL(TotalCalc,0)=1";
                                    //    if (dvFilter.ToTable().Rows.Count > 0)
                                    //    {
                                    //        bShowFooter = true;
                                    //    }
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //}
                                    //finally
                                    //{
                                    //    dvFilter.Dispose();
                                    //}
                                }
                            }
                        }
                    }

                    ViewState["TotalColCount"] = objDsTabular.Tables[0].Columns.Count; //t-change

                    int TRows = 0;
                    DataSet objDs = new DataSet();
                    if (ViewState["NonTabularData"] != null)
                    {
                        objDs = (DataSet)ViewState["NonTabularData"];
                        if (objDs != null)
                        {
                            if (objDs.Tables[0].Rows.Count > 0)
                            {
                                TRows = common.myInt(objDs.Tables[0].Rows[0]["TRows"]);
                            }
                        }
                    }

                    //if (TRows == 0)
                    //{
                    //    ViewState["AllowNewRow"] = true;
                    //    gvTabularFormat.DataSource = objDsTabular.Tables[0];
                    //    gvTabularFormat.DataBind();
                    //}
                    //else
                    //{
                    DataSet dsSectionRows = new DataSet();
                    Hashtable hsSR = new Hashtable();

                    hsSR.Add("@intSectionId", common.myInt(ViewState["SelectedNode"]));
                    objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                    dsSectionRows = objDl.FillDataSet(CommandType.Text, "SELECT ValueId, ValueName FROM EMRTemplateRows WITH (NOLOCK) WHERE SectionId = @intSectionId AND Active = 1 ORDER BY SequenceNo", hsSR);

                    if (dsSectionRows.Tables[0].Rows.Count > 0)
                    {
                        TRows = dsSectionRows.Tables[0].Rows.Count;

                        int currentDataRows = objDsTabular.Tables[0].Rows.Count - 3;

                        int counter = 0;
                        bool isFound = false;

                        foreach (DataRow drRow in dsSectionRows.Tables[0].Rows)
                        {
                            objDsTabular.Tables[0].AcceptChanges();

                            isFound = false;
                            for (int idxDR = 3; idxDR < objDsTabular.Tables[0].Rows.Count; idxDR++)
                            {
                                if (common.myInt(drRow["ValueId"]).Equals(common.myInt(objDsTabular.Tables[0].Rows[idxDR]["RowCaptionId"])))
                                {
                                    objDsTabular.Tables[0].Rows[idxDR]["Col1"] = common.myStr(drRow["ValueName"]);
                                    ++counter;
                                    isFound = true;
                                }
                            }

                            if (!isFound)
                            {
                                DataRow DR = objDsTabular.Tables[0].NewRow();

                                DR["SectionId"] = common.myInt(ViewState["SelectedNode"]);
                                DR["IsData"] = "D";
                                DR["RowCaptionId"] = common.myInt(drRow["ValueId"]);
                                DR["Col1"] = common.myStr(drRow["ValueName"]);

                                objDsTabular.Tables[0].Rows.InsertAt(DR, counter + 3);
                                ++counter;
                            }
                        }

                        objDsTabular.Tables[0].AcceptChanges();
                        for (int idxR = 3; idxR < objDsTabular.Tables[0].Rows.Count; idxR++)
                        {
                            objDsTabular.Tables[0].Rows[idxR]["Id"] = idxR + 1;
                            objDsTabular.Tables[0].Rows[idxR]["RowNum"] = idxR;
                        }

                        DataView DVF = objDsTabular.Tables[0].DefaultView;
                        if (objDsTabular.Tables[0].Rows.Count > (counter + 3))
                        {
                            DVF.RowFilter = "Id<=" + (counter + 3);
                        }

                        ViewState["AllowNewRow"] = false;

                        ViewState["TabularRowData"] = DVF.ToTable();
                        gvTabularFormat.DataSource = DVF.ToTable();
                        gvTabularFormat.DataBind();

                    }
                    else
                    {
                        ViewState["AllowNewRow"] = true;
                        gvTabularFormat.DataSource = objDsTabular.Tables[0];
                        gvTabularFormat.DataBind();

                    }
                    //if (bShowFooter == true)
                    //{
                    //    gvTabularFormat.ShowFooter = true;
                    //}
                    //else
                    //{
                    //    gvTabularFormat.ShowFooter = false;
                    //}
                    dtNoOfColumn = new DataTable();
                    dtNoOfColumn = objDsTabular.Tables[2];

                    if (ViewState["TabularRowData"] != null)
                    {
                        dtNoOfColumn.AcceptChanges();
                        dtNoOfColumn.Rows[0]["RowsCount"] = ((DataTable)ViewState["TabularRowData"]).Rows.Count - 3;
                    }

                    DropDownItems = objDsTabular.Tables[1].DefaultView;

                    if (objDsTabular.Tables.Count > 3)
                    {
                        dtFieldMaxLength = objDsTabular.Tables[3];
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

    protected void btnAddRow_Click(object sender, EventArgs e)
    {
        DataTable dtPreserve = new DataTable();
        try
        {
            if (common.myStr(ViewState["EntryType"]).Trim().Equals("E")
                && (common.myInt(ddlEpisode.SelectedValue).Equals(0)))
            {
                Alert.ShowAjaxMsg("Please select Episode", Page);
                return;
            }
            else
            {
                int flag = 0;

                //start tbl create
                dtPreserve.Columns.Add("Id");
                dtPreserve.Columns.Add("SectionId");
                dtPreserve.Columns.Add("RowNum");
                dtPreserve.Columns.Add("IsData");
                dtPreserve.Columns.Add("RowCaptionId");
                dtPreserve.Columns.Add("RowCaptionName");

                dtPreserve.Columns["Id"].AutoIncrement = true;
                dtPreserve.Columns["Id"].AutoIncrementSeed = 1;

                for (int idx = 0; idx < gvTabularFormat.Columns.Count - 6; idx++)
                {
                    dtPreserve.Columns.Add("Col" + (idx + 1).ToString());
                }
                //end tbl create

                int min = 6;

                for (int i = 0; i < gvTabularFormat.Items.Count; i++)
                {
                    DataRow dr = dtPreserve.NewRow();

                    dr[1] = gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text.Trim();
                    dr[2] = gvTabularFormat.Items[i].Cells[(byte)enumT.RowNum].Text.Trim();
                    dr[3] = gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text.Trim();

                    if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                    {
                        for (int j = 0; j < (common.myInt(ViewState["TotalColCount"]) - min); j++)
                        {
                            Label lblControlType = (Label)gvTabularFormat.Items[i].Cells[j].FindControl("lblFieldId" + j.ToString());
                            dr[j + min] = lblControlType.Text;

                        }
                        //ViewState["ValueOfControlType"] = i ;
                        flag = 0;
                    }

                    else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                    {
                        if (flag.Equals(0))
                        {
                            ViewState["ValueOfControlType"] = i - 1;
                        }

                        flag++;


                        for (int j = min; j < common.myInt(ViewState["TotalColCount"]); j++)
                        {
                            Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j - min].FindControl("lblFieldId" + (j - min).ToString());
                            if (common.myStr(lblControlType.Text).Equals("T"))
                            {
                                TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j - min].FindControl("txtT" + (j - min).ToString());
                                dr[j] = txtVisData.Text;
                            }
                            else if (common.myStr(lblControlType.Text).Equals("M"))
                            {
                                TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j - min].FindControl("txtM" + (j - min).ToString());

                                dr[j] = txtVisData.Text;
                            }
                            else if (common.myStr(lblControlType.Text).Equals("D"))
                            {
                                Label lblFieldId = (Label)gvTabularFormat.Items[i].Cells[j - min].FindControl("lblFieldId" + (j - min).ToString());

                                DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[i].Cells[j - min].FindControl("D" + (j - min).ToString());

                                dr[j] = ddlVisData.SelectedValue;
                            }
                            else if (common.myStr(lblControlType.Text).Equals("IM"))
                            {
                                Label lblFieldId = (Label)gvTabularFormat.Items[i].Cells[j - min].FindControl("lblFieldId" + (j - min).ToString());

                                RadComboBox ddlVisData = (RadComboBox)gvTabularFormat.Items[i].Cells[j - min].FindControl("IM" + (j - min).ToString());

                                dr[j] = ddlVisData.SelectedValue;
                            }
                            else if (common.myStr(lblControlType.Text).Equals("B"))
                            {
                                DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[i].Cells[j - min].FindControl("B" + (j - min).ToString());

                                dr[j] = ddlVisData.SelectedValue;
                            }
                            else if (common.myStr(lblControlType.Text).Equals("S"))
                            {
                                TextBox txtDate = (TextBox)gvTabularFormat.Items[i].Cells[j - min].FindControl("txtDate" + (j - min).ToString());
                                if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                                {
                                    if (!txtDate.Text.Trim().Equals(string.Empty))
                                    {
                                        //strTabular = strTabular + "<c3>" + bC.FormatDate(txtDate.Text, Session["OutputDateFormat"].ToString(), "dd/MM/yyyy") + "</c3>";
                                        //dr[j] = bC.FormatDate(txtDate.Text, Session["OutputDateFormat"].ToString(), "dd/MM/yyyy");
                                        //dr[j] = Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy");
                                        dr[j] = txtDate.Text.Trim();
                                    }
                                }
                                else
                                {
                                    dr[j] = null;
                                }
                            }
                        }
                    }

                    dtPreserve.Rows.Add(dr);

                    //if ((flag == common.myInt(drnoofrows[0][2]))
                    //    && (common.myStr(ViewState["SelectedNode"]) == common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text).Trim()))

                    if (i.Equals(gvTabularFormat.Items.Count - 1)
                        && (common.myInt(ViewState["SelectedNode"]).Equals(common.myInt(common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text).Trim()))))
                    {
                        for (int newrow = 0; newrow < 5; newrow++)
                        {
                            DataRow drnew = dtPreserve.NewRow();

                            drnew[1] = common.myStr(ViewState["SelectedNode"]);
                            drnew[2] = gvTabularFormat.Items[i].Cells[(byte)enumT.RowNum].Text.Trim();
                            drnew[3] = "D";

                            dtPreserve.Rows.Add(drnew);
                        }
                    }
                }

                gvTabularFormat.DataSource = dtPreserve;
                gvTabularFormat.DataBind();

                AddRow = true;

                isFirstTime = true;
                tvCategory_SelectedNodeChanged(sender, e);
                isFirstTime = false;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    public ArrayList GetSelectedNode(string Node)
    {
        ArrayList ar = new ArrayList();
        try
        {
            int length = Node.Length;
            //string Tabular = Node.Substring(length - 1, 1);
            string Tabular = getSubString(common.myStr(Node), length - 1, 1);
            if (Tabular.Equals("T"))
            {
                //ar.Add(Node.Substring(1, length - 2));
                ar.Add(getSubString(common.myStr(Node), 1, length - 2));
                ar.Add("T");//True for Tabular Data
            }
            else
            {
                //ar.Add(Node.Substring(1, length - 1));
                ar.Add(getSubString(common.myStr(Node), 1, length - 1));
                ar.Add("F");// False For Column wise Data
            }

            ViewState["SelectedNode"] = ar[0]; //tvCategory.SelectedValue.Substring(1, tvCategory.SelectedValue.Length - 1).ToString();
            ViewState["SelectedNodeIsTabular"] = ar[1];
            if (tvCategory.SelectedNode != null)
                lblTemplateName.Text = "<b>Template</b> : " + tvCategory.SelectedNode.Text;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return ar;
    }

    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType.Equals(DataControlRowType.DataRow) || e.Row.RowType.Equals(DataControlRowType.Header))
        {
            //e.Row.Cells[(byte)enumNonT.FieldId].Visible = false;
            //e.Row.Cells[(byte)enumNonT.FieldType].Visible = false;
            //e.Row.Cells[(byte)enumNonT.Remarks].Visible = false;
            //e.Row.Cells[(byte)enumNonT.ParentId].Visible = false;
            //e.Row.Cells[(byte)enumNonT.ParentValue].Visible = false;
            //e.Row.Cells[(byte)enumNonT.Hierarchy].Visible = false;
            //e.Row.Cells[(byte)enumNonT.SectionId].Visible = false;
            //e.Row.Cells[(byte)enumNonT.DataObjectId].Visible = false;
            //e.Row.Cells[(byte)enumNonT.IsMandatory].Visible = false;
            //e.Row.Cells[(byte)enumNonT.MandatoryType].Visible = false;
            //e.Row.Cells[(byte)enumNonT.EmployeeTypeId].Visible = false;
        }

        if (e.Row.RowType.Equals(DataControlRowType.DataRow))
        {
            if (!common.myInt(Session["HospitalLocationId"]).Equals(0) && !common.myInt(ViewState["EncounterId"]).Equals(0))
            {
                DataView objDv = null;
                DataView objDvValue;
                DataTable objDt = null;

                if (objDs.Tables[1].Rows.Count > 0)
                {
                    objDv = objDs.Tables[1].DefaultView;
                    objDv.RowFilter = "FieldId=" + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim());
                }
                DataView ddv = new DataView(dt); // To Be Check

                if (ddv.Count > 0)
                {
                    ddv.RowFilter = "FieldId=" + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim());
                    objDt = ddv.ToTable();
                    if (objDt.Rows.Count > 0)
                    {
                        e.Row.Visible = true;
                    }
                }
                else
                {
                    if (objDs.Tables.Count > 2)
                    {
                        if (objDs.Tables[2].Rows.Count > 0)
                        {
                            objDvValue = objDs.Tables[2].DefaultView;
                            if (objDvValue.Table.Columns["FieldId"] != null)
                            {
                                objDvValue.RowFilter = "FieldId=" + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim()) + " AND RowNo=0";
                            }
                            objDt = objDvValue.ToTable();
                            if (objDt.Rows.Count > 0)
                            {
                                e.Row.Visible = true;
                            }
                        }
                    }
                }

                HtmlTextArea txtMulti = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                if (!common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals(string.Empty))
                {
                    if (!common.myStr(e.Row.Cells[(byte)enumNonT.IsMandatory].Text).Equals("&nbsp;"))
                    {
                        if (common.myBool(e.Row.Cells[(byte)enumNonT.IsMandatory].Text))
                        {
                            e.Row.Cells[(byte)enumNonT.FieldName].Text = common.myStr(e.Row.Cells[(byte)enumNonT.FieldName].Text) + hdnMandatoryStar.Value;
                        }
                    }

                    //New Code added for showing hyper link Start-13-feb-2014
                    if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("Y"))
                    {
                        //cCtlType = "Y";
                        //HyperLink HYL = (HyperLink)e.Row.FindControl("HYL");
                        //HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");  
                        //HiddenField hdnlink = (HiddenField)e.Row.FindControl("hdnlink");
                        //HiddenField hdnisLinkRequire = (HiddenField)e.Row.FindControl("hdnisLinkRequire");
                        //HiddenField hdnLinkUrl = (HiddenField)e.Row.FindControl("hdnLinkUrl");  
                        ////Label lbl_HYLvalue = (Label)e.Row.FindControl("lbl_HYLvalue");  
                        //Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        //btnHelp.Visible = true;  
                        //if (common.myBool(hdnisLinkRequire.Value))
                        //{
                        //    Hy_LinkUrl.Visible = true;
                        //    Hy_LinkUrl.Text = hdnLinkUrl.Value;     

                        //}


                        //RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;
                        //HYL.Visible = true;
                        //txtW.Visible = false;
                        ////HYL.NavigateUrl="https://www.google.co.in";
                        //HYL.Text = hdnlink.Value;
                        //HYL.ToolTip = "Click to open the Page";
                        //HYL.Attributes.Add("onclick", "openNewWindowExternlink('" + HYL.Text + "')");

                    }
                    #region Single TextBox Type
                    if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("T"))
                    {
                        cCtlType = "T";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

                        btnAdd.Visible = false;
                        txtT.Enabled = true;
                        txtW.Visible = false;

                        string maxLength = common.myStr(txtT.MaxLength);
                        btnHelp.Attributes.Add("onclick", "openSentenceWindow('" + txtT.ClientID + "','T','" + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" +
                                                            common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) + "','" + common.myStr(e.Row.RowIndex) + "')");
                        //btnHelp.Attributes.Add("onclick", "openRadWindow('" + txtT.ClientID + "','T')");//"win=window.open('SentanceGallery.aspx?ctrlId=" + txtT.ClientID + "&Mx=" + maxLength + "&typ=1','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                        btnHelp.Visible = true;
                        txtT.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtT.ToolTip = txtT.Text;
                            }

                        }
                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            txtT.Enabled = false;
                            btnHelp.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            if (common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).Equals(64330))
                            {
                                txtT.Enabled = false;
                            }
                            else
                            {
                                txtT.Enabled = true;
                            }
                            btnHelp.Enabled = true;
                            txtW.Enabled = true;
                        }
                        if (common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).Equals(64330))
                        {
                            btnHelp.Enabled = false;
                        }

                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        //New Code Added-14-02-2014
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        HiddenField hdnisLinkRequire = (HiddenField)e.Row.FindControl("hdnisLinkRequire");
                        HiddenField hdnLinkUrl = (HiddenField)e.Row.FindControl("hdnLinkUrl");
                        //Label lbl_HYLvalue = (Label)e.Row.FindControl("lbl_HYLvalue");  
                        if (common.myBool(hdnisLinkRequire.Value))
                        {
                            Hy_LinkUrl.Visible = true;
                            Hy_LinkUrl.Text = "Ref.";
                            //Hy_LinkUrl.Text = hdnLinkUrl.Value;
                            Hy_LinkUrl.ToolTip = "Click for View the Page";

                            if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            {
                                Hy_LinkUrl.Attributes.Add("onclick", "openNewWindowExternlink('" + hdnLinkUrl.Value + "')");
                            }

                            //Hy_LinkUrl.Attributes.Add("onclick", "openNewWindowExternlink('" + "file://192.168.1.68/share/1.xlsx" + "')");

                            btnHelp.Visible = false;
                        }
                        else
                        {
                            Hy_LinkUrl.Visible = false;
                            btnHelp.Visible = true;
                        }
                        //New Code End
                        //Added on 16-04-2014 Start  Naushad

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            txtT.Enabled = true;
                            btnHelp.Enabled = true;
                        }
                        else
                        {
                            txtT.Enabled = false;
                            btnHelp.Enabled = false;
                        }
                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false
                            && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            txtT.Enabled = false;
                            btnHelp.Enabled = false;
                            Hy_LinkUrl.Enabled = false;
                            txtW.Enabled = false;
                            btnAdd.Enabled = false;
                        }

                        //Disable the text control for Score Calc and hide the help button
                        DataView dvFields = new DataView();
                        try
                        {
                            if (objDs != null)
                            {
                                if (objDs.Tables.Count > 0)
                                {
                                    if (objDs.Tables[0].Rows.Count > 0)
                                    {
                                        dvFields = objDs.Tables[0].Copy().DefaultView;
                                        dvFields.RowFilter = "FieldId=" + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text)) +
                                                            " AND SectionId=" + common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) +
                                                            " AND ISNULL(IsScoreCalc,0)=1";

                                        if (dvFields.ToTable().Rows.Count > 0)
                                        {
                                            btnHelp.Visible = false;

                                            txtT.Enabled = false;
                                            txtT.Style["text-align"] = "right";
                                            txtT.Width = Unit.Pixel(70);
                                            txtT.BorderWidth = Unit.Pixel(2);
                                            txtT.Font.Bold = true;
                                            txtT.ForeColor = System.Drawing.Color.Navy;
                                            txtT.Visible = true;
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
                            dvFields.Dispose();
                        }
                    }
                    #endregion
                    #region start for Field type I
                    if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("I"))
                    {
                        cCtlType = "I";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        // Label lblLabNo = (Label)e.Row.FindControl("lblLabNo");
                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;

                        btnAdd.Visible = false;
                        txtT.Enabled = true;
                        txtW.Visible = false;
                        btnHelp.Visible = false;
                        txtT.Visible = true;
                        Hy_LinkUrl.Visible = false;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (common.myInt(objDt.Rows[0]["StaticTemplateId"]) > 0)
                                {
                                    txtT.Text = string.Empty;
                                    txtT.ToolTip = string.Empty;
                                    lnkStaticTemplate.Attributes.Add("onclick", "openResultStaticTemplateWindow('/LIS/Phlebotomy/previewResult.aspx'" + ",'" +
                                      common.myStr(objDt.Rows[0]["TextValue"]) + "','" + common.myStr(objDt.Rows[0]["StaticTemplateId"]) + "','" +
                                      common.myStr(objDt.Rows[0]["MainText"]) + "','','RF','')");

                                    //source,diagsampleid,serviceid,agegende,statuscode,servicename
                                    lnkStaticTemplate.Text = common.myStr(objDt.Rows[0]["TextValue"]) + " Lab No- " + common.myStr(objDt.Rows[0]["EnterBy"]) + ", " + common.myStr(objDt.Rows[0]["VisitDateTime"]);// common.myStr(objDt.Rows[0]["FieldValue"]);    
                                }
                                if (common.myStr(objDt.Rows[0]["RowCaption"]).ToUpper().Equals("SPECIAL"))
                                {
                                    txtT.Text = string.Empty;
                                    txtT.ToolTip = string.Empty;
                                }
                                else
                                {
                                    txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    txtT.ToolTip = txtT.Text;
                                }
                                // lblLabNo.Text = "Lab No- " + common.myStr(objDt.Rows[0]["EnterBy"]); 
                            }
                            if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                            {
                                txtT.Enabled = false;
                                btnHelp.Enabled = false;
                                Hy_LinkUrl.Enabled = false;
                                txtW.Enabled = false;
                                btnAdd.Enabled = false;

                            }
                        }
                    }
                    #endregion
                    #region start for Field type IS
                    if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("IS"))
                    {
                        cCtlType = "IS";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        // Label lblLabNo = (Label)e.Row.FindControl("lblLabNo");
                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;

                        btnAdd.Visible = false;
                        txtT.Enabled = false;
                        txtW.Visible = false;
                        btnHelp.Visible = false;
                        txtT.Visible = false;
                        Hy_LinkUrl.Visible = false;
                        txtM.Visible = true;

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                //if (common.myStr(objDt.Rows[0]["RowCaption"]).ToUpper() == "SPECIAL")
                                if (common.myInt(objDt.Rows[0]["StaticTemplateId"]) > 0)
                                {
                                    txtM.Text = string.Empty;
                                    txtM.ToolTip = string.Empty;
                                    lnkStaticTemplate.Attributes.Add("onclick", "openResultStaticTemplateWindow('/LIS/Phlebotomy/previewResult.aspx'" + ",'" +
                                      common.myStr(objDt.Rows[0]["TextValue"]) + "','" + common.myStr(objDt.Rows[0]["StaticTemplateId"]) + "','" +
                                      common.myStr(objDt.Rows[0]["MainText"]) + "','','RF','')");

                                    //source,diagsampleid,serviceid,agegende,statuscode,servicename
                                    lnkStaticTemplate.Text = common.myStr(objDt.Rows[0]["TextValue"]) + " Lab No- " + common.myStr(objDt.Rows[0]["EnterBy"]) + ", " + common.myStr(objDt.Rows[0]["VisitDateTime"]);// common.myStr(objDt.Rows[0]["FieldValue"]);    
                                }
                                if (common.myStr(objDt.Rows[0]["RowCaption"]).ToUpper().Equals("SPECIAL"))
                                {
                                    txtT.Text = string.Empty;
                                    txtT.ToolTip = string.Empty;
                                }
                                else
                                {
                                    string txtMText = string.Empty;
                                    txtMText = common.myStr(objDt.Rows[0]["FieldValue"]);
                                    if (common.myStr(objDt.Rows[0]["FieldValue"]).Contains("&lt;"))
                                    {
                                        txtMText = common.myStr(objDt.Rows[0]["FieldValue"]).Replace("&lt;", "<");
                                    }
                                    if (common.myStr(objDt.Rows[0]["FieldValue"]).Contains("&gt;"))
                                    {
                                        if (txtMText.Equals(string.Empty))
                                        {
                                            txtMText = common.myStr(objDt.Rows[0]["FieldValue"]).Replace("&gt;", ">");
                                        }
                                        else
                                        {
                                            txtMText = txtMText.Replace("&gt;", ">");
                                        }
                                    }
                                    txtM.Text = txtMText;
                                    txtM.ToolTip = txtM.Text;
                                }
                                // lblLabNo.Text = "Lab No- " + common.myStr(objDt.Rows[0]["EnterBy"]);
                            }
                            if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                            {
                                txtT.Enabled = false;
                                btnHelp.Enabled = false;
                                Hy_LinkUrl.Enabled = false;
                                txtW.Enabled = false;
                                btnAdd.Enabled = false;
                                txtM.Enabled = false;
                            }
                        }
                    }
                    #endregion
                    #region Mutiple Text Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("M"))
                    {
                        cCtlType = "M";
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = false;
                        txtW.Visible = false;
                        btnHelp.Attributes.Add("onclick", "openSentenceWindow('" + txtM.ClientID + "','M','" + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" +
                                                            common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) + "','" + common.myStr(e.Row.RowIndex) + "')");

                        // btnHelp.Attributes.Add("onclick", "openRadWindow('" + txtM.ClientID + "','M')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                        btnHelp.Visible = true;
                        txtM.Visible = true;

                        txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                string txtMText = string.Empty;
                                txtMText = common.myStr(objDt.Rows[0]["FieldValue"]);
                                if (common.myStr(objDt.Rows[0]["FieldValue"]).Contains("&lt;"))
                                {
                                    txtMText = common.myStr(objDt.Rows[0]["FieldValue"]).Replace("&lt;", "<");
                                }
                                if (common.myStr(objDt.Rows[0]["FieldValue"]).Contains("&gt;"))
                                {
                                    if (txtMText.Equals(string.Empty))
                                    {
                                        txtMText = common.myStr(objDt.Rows[0]["FieldValue"]).Replace("&gt;", ">");
                                    }
                                    else
                                    {
                                        txtMText = txtMText.Replace("&gt;", ">");
                                    }
                                }
                                txtM.Text = txtMText;
                                txtM.ToolTip = txtM.Text;
                            }
                        }
                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            txtM.Enabled = false;
                            btnHelp.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            txtM.Enabled = true;
                            btnHelp.Enabled = true;
                            txtW.Enabled = true;
                        }

                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        //New Code Added-14-02-2014
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        HiddenField hdnisLinkRequire = (HiddenField)e.Row.FindControl("hdnisLinkRequire");
                        HiddenField hdnLinkUrl = (HiddenField)e.Row.FindControl("hdnLinkUrl");
                        //Label lbl_HYLvalue = (Label)e.Row.FindControl("lbl_HYLvalue");  
                        if (common.myBool(hdnisLinkRequire.Value))
                        {
                            Hy_LinkUrl.Visible = true;
                            Hy_LinkUrl.Text = "Ref.";
                            //Hy_LinkUrl.Text = hdnLinkUrl.Value;
                            Hy_LinkUrl.ToolTip = "Click for View the Page";

                            if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            {
                                Hy_LinkUrl.Attributes.Add("onclick", "openNewWindowExternlink('" + hdnLinkUrl.Value + "')");
                            }

                            btnHelp.Visible = false;
                        }
                        else
                        {
                            Hy_LinkUrl.Visible = false;
                            btnHelp.Visible = true;
                        }
                        //New Code End

                        //Added on 16-04-2014 Start  Naushad

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            txtM.Enabled = true;
                            btnHelp.Enabled = true;
                        }
                        else
                        {
                            txtM.Enabled = false;
                            btnHelp.Enabled = false;
                        }
                        //Added on 16-04-2014 End  Naushad

                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            btnHelp.Enabled = false;
                            Hy_LinkUrl.Enabled = false;
                            txtW.Enabled = false;
                            btnAdd.Enabled = false;
                            txtM.Enabled = false;
                        }
                    }
                    #endregion
                    #region WordProcessor Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("W")) // For WordProcessor
                    {
                        //Int32 rowindex = e.Row.RowIndex;
                        cCtlType = "W";
                        RadComboBox ddl = (RadComboBox)e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTemplateFieldFormats");
                        ddl.Visible = true;

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = false;


                        //ddl.DataSource = BindFieldFormats(common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).ToString());
                        //ddl.DataTextField = "FormatName";
                        //ddl.DataValueField = "FormatId";
                        //ddl.DataBind();

                        DataTable tblFieldFormats = new DataTable();
                        RadComboBoxItem itemTF;
                        try
                        {
                            ddl.Items.Clear();
                            tblFieldFormats = BindFieldFormats(common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).ToString());

                            foreach (DataRow DR in tblFieldFormats.Rows)
                            {
                                itemTF = new RadComboBoxItem();
                                itemTF.Text = common.myStr(DR["FormatName"]);
                                itemTF.Value = common.myInt(DR["FormatId"]).ToString();
                                itemTF.Attributes.Add("FormatText", common.myStr(DR["FormatText"]));

                                ddl.Items.Add(itemTF);
                            }
                        }
                        catch
                        {
                        }
                        finally
                        {
                            tblFieldFormats.Dispose();
                            itemTF = null;
                        }

                        ddl.Attributes.Add("onchange", "setWordProcessor('" + txtW.ClientID + "')");

                        btnHelp.Attributes.Add("onclick", "openSentenceWindow('" + txtW.ClientID + "','W','" + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" +
                                                            common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) + "','" + common.myStr(e.Row.RowIndex) + "')");


                        //btnHelp.Attributes.Add("onclick", "openRadWindowW('" + txtW.ClientID + "','W','" + common.myStr(e.Row.RowIndex) + "')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                        //txtW.Attributes.Add("onclick", "javascript: document.getElementById('" + hdnSelCell.ClientID + "').value = '" + e.Row.RowIndex.ToString() + "';");
                        btnHelp.Visible = true;
                        txtW.Visible = true;

                        //txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtW.Content = common.myStr(objDt.Rows[0]["FieldValue"]);
                            }
                        }
                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            ddl.Enabled = false;
                            btnHelp.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            ddl.Enabled = true;
                            btnHelp.Enabled = true;
                            txtW.Enabled = true;
                        }

                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        //New Code Added-14-02-2014
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        HiddenField hdnisLinkRequire = (HiddenField)e.Row.FindControl("hdnisLinkRequire");
                        HiddenField hdnLinkUrl = (HiddenField)e.Row.FindControl("hdnLinkUrl");
                        //Label lbl_HYLvalue = (Label)e.Row.FindControl("lbl_HYLvalue");  
                        if (common.myBool(hdnisLinkRequire.Value))
                        {
                            Hy_LinkUrl.Visible = true;
                            Hy_LinkUrl.Text = "Ref.";
                            //Hy_LinkUrl.Text = hdnLinkUrl.Value;
                            Hy_LinkUrl.ToolTip = "Click for View the Page";

                            if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            {
                                Hy_LinkUrl.Attributes.Add("onclick", "openNewWindowExternlink('" + hdnLinkUrl.Value + "')");
                            }
                            //Hy_LinkUrl.Attributes.Add("onclick", "openNewWindowExternlink('" + "http://www.orthopaedicscore.com/scorepages/oxford_shoulder_score.html" + "')");

                            btnHelp.Visible = false;
                        }
                        else
                        {
                            Hy_LinkUrl.Visible = false;
                            btnHelp.Visible = true;
                        }

                        //Added on 16-04-2014 Start Naushad

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            ddl.Enabled = true;
                            txtW.Enabled = true;
                            btnHelp.Enabled = true;
                        }
                        else
                        {
                            ddl.Enabled = false;
                            txtW.Enabled = false;
                            btnHelp.Enabled = false;
                        }

                        //Added on 16-04-2014 End  Naushad

                        txtW.Enabled = gvSelectedServices.Enabled;

                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]))
                        {
                            btnHelp.Enabled = false;
                            Hy_LinkUrl.Enabled = false;
                            txtW.Enabled = false;
                            btnAdd.Enabled = false;
                            ddl.Enabled = false;
                        }
                    }
                    #endregion
                    #region CheckBox Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("C"))
                    {
                        cCtlType = "C";
                        DataList list = (DataList)e.Row.FindControl("C");
                        HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = true;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        list.Visible = true;
                        if (hdnColumnNosToDisplay.Value != null)
                        {
                            list.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                        }

                        list.DataSource = objDv;
                        list.DataBind();

                        HtmlTextArea txtRemarks = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                        txtRemarks.Visible = false;
                        //e.Row.Cells.Remove(e.Row.Cells[4]);
                        //e.Row.Cells[3].ColumnSpan = 2;


                        HiddenField hdnCV = (HiddenField)list.FindControl("hdnCV");
                        foreach (DataListItem item in list.Items)
                        {
                            HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                            CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                            CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                            HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                            CheckBox chk = (CheckBox)item.FindControl("C");
                            if (objDt != null)
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    foreach (DataRow drow in objDt.Rows)
                                    {
                                        if (common.myStr(drow["FieldValue"]).Trim().Equals(common.myStr(hdn.Value).Trim()))
                                        {
                                            chk.Checked = true;
                                        }
                                    }
                                    if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                                    {
                                        chk.Checked = false;

                                        txtW.Enabled = false;
                                    }
                                    else
                                    {
                                        chk.Enabled = true;

                                        txtW.Enabled = true;
                                    }

                                    //btnAdd.Attributes.Add("onclick", "openRadWindowForFieldValue('" + txtW.ClientID + "','C','" + common.myStr(e.Row.RowIndex) + "')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                                }
                                if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                                {
                                    txtW.Enabled = false;
                                    btnAdd.Enabled = false;
                                    chk.Enabled = false;
                                }
                            }
                            //Added on 17-04-2014 Start  Naushad
                            HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                            HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                            if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            {
                                chk.Enabled = true;
                                btnAdd.Enabled = true;
                                //rpt.Enabled = true;
                            }
                            else
                            {
                                chk.Enabled = false;
                                //rpt.Visible = false;
                                btnAdd.Enabled = false;
                            }
                            //Added on 17-04-2014  End  Naushad
                        }
                        if (common.myStr(ViewState["AllowAddFieldValue"]) != "AFV")
                        {
                            btnAdd.Visible = false;
                        }
                        else
                        {
                            btnAdd.Visible = true;
                            btnAdd.Attributes.Add("onclick", "openRadWindowForFieldValue('" + list.ClientID + "','C','" + common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) + "',' " + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" + common.myStr(e.Row.RowIndex) + "')");
                        }
                    }
                    #endregion
                    #region Boolean Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("B"))
                    {
                        cCtlType = "B";
                        RadioButtonList B = (RadioButtonList)e.Row.FindControl("B");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = false;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        B.Visible = true;

                        B.Attributes.Add("onclick", "radioMe(event,'" + B.ClientID + "');");
                        B.Attributes.Add("onchange", "ctrlOnSelectedIndexChanged('" + B.ClientID + "','" + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" + common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text) + "')");

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                objDvValue = objDt.DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId=" + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim());
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(1))//Yes
                                    {
                                        B.SelectedValue = "1";

                                        //if (common.myInt(ddlB.SelectedValue) == common.myInt(r.Cells[6].Text))
                                        //{
                                        //    r.Visible = true;
                                        //}
                                        //else
                                        //{
                                        //    r.Visible = false;
                                        //}
                                    }
                                    else if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(0))//No
                                    {
                                        B.SelectedValue = "0";
                                    }
                                    else
                                    {
                                        B.SelectedValue = "-1";  //Select
                                    }
                                }
                            }
                        }
                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            B.Enabled = false;
                            //btnHelp.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            B.Enabled = true;
                            //btnHelp.Enabled = true;
                            txtW.Enabled = true;
                        }

                        //Added on 17-04-2014 Naushad Ali start
                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            B.Enabled = true;
                        }
                        else
                        {
                            B.Enabled = false;
                        }

                        // //Added on 17-04-2014 Naushad Ali End

                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            txtW.Enabled = false;
                            btnAdd.Enabled = false;
                            B.Enabled = false;
                        }
                    }
                    #endregion
                    #region DropDown Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("D"))
                    {
                        cCtlType = "D";
                        DropDownList ddl = (DropDownList)e.Row.Cells[(byte)enumNonT.Values].FindControl("D");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

                        btnAdd.Visible = true;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        ddl.Attributes.Add("onchange", "ctrlOnSelectedIndexChanged('" + ddl.ClientID + "','" + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" + common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text) + "')");

                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();

                        //btnHelp.Attributes.Add("onclick", "openRadWindowW('" + txtW.ClientID + "','W','" + common.myStr(e.Row.RowIndex) + "')"); 

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }
                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            ddl.Enabled = false;
                            //btnHelp.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            ddl.Enabled = true;
                            //btnHelp.Enabled = true;
                            txtW.Enabled = true;
                        }

                        //Added on 16-04-2014 Start  Naushad
                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            ddl.Enabled = true;
                            btnAdd.Enabled = true;
                        }
                        else
                        {
                            ddl.Enabled = false;
                            btnAdd.Enabled = false;
                        }
                        if (common.myStr(ViewState["AllowAddFieldValue"]) != "AFV")
                        {
                            btnAdd.Visible = false;
                        }
                        else
                        {
                            btnAdd.Visible = true;
                            btnAdd.Attributes.Add("onclick", "openRadWindowForFieldValue('" + ddl.ClientID + "','D','" + common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) + "',' " + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" + common.myStr(e.Row.RowIndex) + "')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                        }
                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            btnAdd.Enabled = false;
                            txtW.Enabled = false;
                            ddl.Enabled = false;
                        }
                    }
                    #endregion
                    #region DropDown Image Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("IM"))
                    {
                        cCtlType = "IM";
                        RadComboBox ddl = (RadComboBox)e.Row.Cells[(byte)enumNonT.Values].FindControl("IM");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = false;
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;


                        foreach (DataRow drImage in objDv.ToTable().Rows)
                        {
                            RadComboBoxItem item = new RadComboBoxItem();
                            item.Text = (string)drImage["ValueName"];
                            item.Value = drImage["ValueId"].ToString();
                            item.ImageUrl = drImage["ImagePath"].ToString();
                            ddl.Items.Add(item);
                            item.DataBind();
                        }
                        btnAdd.Attributes.Add("onclick", "openRadWindowForFieldValue('" + ddl.ClientID + "','D','" + common.myInt(e.Row.Cells[(byte)enumNonT.SectionId].Text) + "',' " + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" + common.myStr(e.Row.RowIndex) + "')"); //"win=window.open('SentanceGallery.aspx?ctrlId=" + txtM.ClientID + "&typ=0','SentanceGallery','resizable=0,scrollbars=yes,width=600,height=520'); win.moveTo(40,150); return false;");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (common.myStr(objDt.Rows[0]["FieldValue"]) != null)
                                {
                                    ddl.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]);
                                }
                            }
                        }
                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            ddl.Enabled = false;
                            //btnHelp.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            ddl.Enabled = true;
                            //btnHelp.Enabled = true;
                            txtW.Enabled = true;
                        }


                        //Added on 16-04-2014 Start  Naushad
                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            ddl.Enabled = true;
                            //btnAdd.Enabled = true;
                        }
                        else
                        {
                            ddl.Enabled = false;
                            //btnAdd.Enabled = false;
                        }
                        //Addedon 16-04-2014 End Naushad

                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            btnAdd.Enabled = false;
                            txtW.Enabled = false;
                            ddl.Enabled = false;
                        }
                    }
                    #endregion
                    #region RadioButton Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("R"))
                    {
                        cCtlType = "R";
                        RadioButtonList ddl = (RadioButtonList)e.Row.Cells[(byte)enumNonT.Values].FindControl("R");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        ddl.Attributes.Add("onchange", "ctrlOnSelectedIndexChanged('" + ddl.ClientID + "','" + common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text) + "','" + common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text) + "')");

                        if (hdnColumnNosToDisplay.Value != null)
                        {
                            ddl.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                        }
                        ddl.DataSource = objDv;
                        ddl.DataTextField = "ValueName";
                        ddl.DataValueField = "ValueId";
                        ddl.DataBind();


                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                if (ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])) != null)
                                {
                                    ddl.Items.FindByValue(common.myStr(objDt.Rows[0]["FieldValue"])).Selected = true;
                                }
                            }
                        }
                        //if (!objDv.Equals(DBNull.Value))
                        //{
                        //    if (objDv.ToTable().Rows.Count <= 2)
                        //    {
                        //        ddl.Width = Unit.Percentage(50);
                        //    }
                        //    else
                        //    {
                        //        ddl.Width = Unit.Percentage(100);
                        //    }
                        //}

                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            ddl.Enabled = false;
                            txtW.Enabled = false;
                        }
                        else
                        {
                            ddl.Enabled = true;
                            txtW.Enabled = true;
                        }
                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            ddl.Enabled = true;
                        }
                        else
                        {
                            ddl.Enabled = false;
                        }

                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            txtW.Enabled = false;
                            ddl.Enabled = false;
                        }
                    }
                    #endregion
                    #region Date Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("S") || common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("ST") || common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("SB"))//For Date
                    {
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadTimePicker tpTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("tpTime") as RadTimePicker;
                        RadComboBox ddlTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTime") as RadComboBox;
                        Image imgFromDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("imgFromDate") as Image;

                        //txtDate.Text = "";
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = false;
                        AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender3");
                        txtW.Visible = false;

                        //if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("S"))
                        //{

                        //    //containerPatientVisits.Attributes.Add("style", "display:none");
                        //    //containerAppointment.Attributes.Add("style", "display:inline-block");


                        //    txtDate.Attributes.Add("style", "display:inline-block");
                        //    imgFromDate.Attributes.Add("style", "display:inline-block");
                        //    tpTime.Enabled = false;
                        //    ddlTime.Enabled = false;

                        //    //txtDate.Visible = true;
                        //    //imgFromDate.Visible = true;
                        //    //tpTime.Enabled = false;
                        //    //ddlTime.Enabled = false;


                        //}
                        //else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("ST"))
                        //{

                        //    txtDate.Visible = false;
                        //    imgFromDate.Visible = false;
                        //    tpTime.Enabled = true;
                        //    ddlTime.Enabled = true;

                        //    //txtDate.Visible = false;
                        //    //imgFromDate.Visible = false;
                        //    //tpTime.Enabled = true;
                        //    //ddlTime.Enabled = true;
                        //}
                        //else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("SB"))
                        //{


                        //    txtDate.Visible = true;
                        //    imgFromDate.Visible = true;
                        //    tpTime.Enabled = true;
                        //    ddlTime.Enabled = true;


                        //    //txtDate.Visible = true;
                        //    //imgFromDate.Visible = true;
                        //    //tpTime.Enabled = true;
                        //    //ddlTime.Enabled = true;
                        //}



                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                RadComboBox ddlTime1 = (RadComboBox)e.Row.FindControl("ddlTime");
                                ddlTime1.Items.Clear();
                                for (int i = 0; i < 60; i++)
                                {
                                    if (i.ToString().Length == 1)
                                    {

                                        RadComboBoxItem item = new RadComboBoxItem();
                                        item.Text = common.myStr("0" + i.ToString());
                                        item.Value = common.myStr("0" + i.ToString());

                                        item.DataBind();

                                        ddlTime1.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));

                                    }
                                    else
                                    {
                                        RadComboBoxItem item = new RadComboBoxItem();
                                        item.Text = common.myStr(i.ToString());
                                        item.Value = common.myStr(i.ToString());

                                        item.DataBind();

                                        ddlTime1.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                                    }
                                }


                                //txtDate.Text = common.myDate(objDt.Rows[0]["FieldValue"]).ToString("MM/dd/yyyy");
                                string FieldType = common.myStr(objDt.Rows[0]["FieldType"]).Trim();

                                if (FieldType.Equals("S"))
                                {
                                    txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();

                                }
                                else if (FieldType.Equals("ST"))
                                {
                                    tpTime.SelectedDate = DateTime.Parse(common.myStr(objDt.Rows[0]["FieldValue"]).Trim());
                                    //  ddlTime.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Substring(2,2);;
                                    if (common.myStr(objDt.Rows[0]["FieldValue"]).Length.Equals(6))
                                    {
                                        ddlTime.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Substring(2, 2);
                                    }
                                    else if (common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Length.Equals(7))
                                    {
                                        ddlTime.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Substring(3, 2);
                                    }
                                }

                                else if (FieldType.Equals("SB"))
                                {
                                    txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                                    // tpTime.SelectedDate = DateTime.Parse(common.myStr(objDt.Rows[0]["FieldValue"]).Trim());

                                    tpTime.SelectedDate = DateTime.Parse(common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Substring(11, 7));
                                    //   DateTime.Parse("12:45");
                                    //  tpTime.SelectedDate = DateTime.Parse(common.myStr(objDt.Rows[0]["FieldValue"]).Trim());
                                    //   tpTime.SelectedDate = DateTime.Parse("12/06/2016  1:12PM");

                                    //  ddlTime.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Substring(14,2);;

                                    ddlTime.SelectedValue = common.myStr(objDt.Rows[0]["FieldValue"]).Trim().Substring(14, 2);
                                }






                                //tpTime.SelectedDate =;
                                //ddlTime.Text =;

                            }
                        }
                        txtDate.Attributes.Add("onChange", "javascript:OnClientCalenderClick();");
                        tblDate.Visible = true;
                        //txtDate.Text = "";

                        if (common.myStr(ViewState["EntryType"]).Equals("E") && common.myInt(ddlEpisode.SelectedValue).Equals(0))
                        {
                            cal.Enabled = false;


                            txtDate.Enabled = false;
                            tpTime.Enabled = false;
                            ddlTime.Enabled = false;

                            txtW.Enabled = false;
                        }
                        else
                        {
                            cal.Enabled = true;
                            txtDate.Enabled = true;
                            tpTime.Enabled = true;
                            ddlTime.Enabled = true;


                            txtW.Enabled = true;
                        }
                        if (common.myInt(ddlTemplateMain.SelectedValue).Equals(765))
                        {
                            if (common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).Equals(64329))
                            {
                                // TextBox txtDate = (TextBox)e.Row.FindControl("txtDate");
                                txtDate.Enabled = false;
                                tpTime.Enabled = false;
                                ddlTime.Enabled = false;

                                cal.Enabled = false;
                            }
                        }


                        //Added on 17-04-2014 Naushad Ali start
                        HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                        if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                        {
                            txtDate.Enabled = true;
                            tpTime.Enabled = true;
                            ddlTime.Enabled = true;

                            cal.Enabled = true;
                        }
                        else
                        {
                            txtDate.Enabled = false;
                            tpTime.Enabled = false;
                            ddlTime.Enabled = false;

                            cal.Enabled = false;
                        }

                        // //Added on 17-04-2014 Naushad Ali End

                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            txtW.Enabled = false;

                            txtDate.Enabled = false;
                            tpTime.Enabled = false;
                            ddlTime.Enabled = false;

                            btnAdd.Enabled = false;
                        }
                        if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("S"))
                        {
                            //containerPatientVisits.Attributes.Add("style", "display:none");
                            //containerAppointment.Attributes.Add("style", "display:inline-block");

                            //txtDate.Visible = true;
                            //tpTime.Visible = false;
                            //ddlTime.Visible = false;
                            // imgFromDate.Visible = true;

                            txtDate.Attributes.Add("style", "display:inline-block");
                            tpTime.Attributes.Add("style", "display:none");
                            ddlTime.Attributes.Add("style", "display:none");
                            imgFromDate.Attributes.Add("style", "display:inline-block");
                        }
                        else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("ST"))
                        {
                            txtDate.Attributes.Add("style", "display:none");
                            tpTime.Attributes.Add("style", "display:inline-block");
                            ddlTime.Attributes.Add("style", "display:inline-block");
                            imgFromDate.Attributes.Add("style", "display:none");

                            //txtDate.Visible = false;
                            //tpTime.Visible = true;
                            //ddlTime.Visible = true;
                            // imgFromDate.Visible = false;
                        }
                        else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("SB"))
                        {
                            //txtDate.Visible = true;
                            //tpTime.Visible = true;
                            //ddlTime.Visible = true;
                            //  imgFromDate.Visible = true;

                            txtDate.Attributes.Add("style", "display:inline-block");
                            tpTime.Attributes.Add("style", "display:inline-block");
                            ddlTime.Attributes.Add("style", "display:inline-block");
                            imgFromDate.Attributes.Add("style", "display:inline-block");
                        }
                    }
                    #endregion
                    #region Header Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("H"))//For Heading
                    {
                        cCtlType = "H";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        RadTimePicker tpTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("tpTime") as RadTimePicker;
                        RadComboBox ddlTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTime") as RadComboBox;

                        tpTime.Enabled = false;
                        ddlTime.Enabled = false;

                        btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;
                    }
                    #endregion
                    #region Static Template Type
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("L"))
                    {
                        cCtlType = "L";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");

                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");

                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        RadTimePicker tpTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("tpTime") as RadTimePicker;
                        RadComboBox ddlTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTime") as RadComboBox;

                        tpTime.Enabled = false;
                        ddlTime.Enabled = false;

                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;


                        BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                        DataSet dsStatic = master.GetAllTypeTemplates(common.myInt(Session["HospitalLocationId"]), "S");
                        DataView dvStatic = new DataView(dsStatic.Tables[0]);
                        dvStatic.RowFilter = "DisplayInTemplate=1 AND SectionId=" + sStaticTemplateId;
                        DataTable dtStatic = dvStatic.ToTable();
                        if (dtStatic.Rows.Count > 0)
                        {
                            HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                            HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                            if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            {
                                lnkStaticTemplate.Attributes.Add("onclick", "openStaticTemplateWindow('" + common.myStr(dtStatic.Rows[0]["PageUrl"]) + "',' " + sStaticTemplateId + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim()) + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.SectionId].Text).Trim()) + "')");
                                lnkStaticTemplate.Enabled = true;
                            }
                            else
                            {
                                lnkStaticTemplate.Enabled = false;
                            }

                            lnkStaticTemplate.Text = common.myStr(dtStatic.Rows[0]["SectionName"]);
                        }
                        btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;


                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            btnAdd.Enabled = false;

                            txtT.Enabled = false;
                            txtM.Enabled = false;
                            btnHelp.Enabled = false;
                            ddlB.Enabled = false;
                            ddlD.Enabled = false;
                            txtDate.Enabled = false;
                            txtW.Enabled = false;
                        }
                    }
                    #endregion
                    #region Patient Data Object
                    else if (common.myStr(e.Row.Cells[(byte)enumNonT.FieldType].Text).Equals("O"))//Patient Data Object
                    {
                        cCtlType = "O";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        RadTimePicker tpTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("tpTime") as RadTimePicker;
                        RadComboBox ddlTime = e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTime") as RadComboBox;

                        tpTime.Enabled = false;
                        ddlTime.Enabled = false;

                        ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        btnAdd.Visible = false;
                        txtT.Visible = false;
                        txtM.Visible = false;
                        btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;

                        int DataObjectId = common.myInt(e.Row.Cells[(byte)enumNonT.DataObjectId].Text);

                        clsIVF objivf = new clsIVF(sConString);

                        string strOutput = objivf.getDataObjectValue(DataObjectId);

                        if (strOutput.Length > 50)
                        {
                            txtM.Visible = true;
                            txtM.Text = strOutput;
                            txtM.ToolTip = strOutput;
                            txtM.Enabled = false;
                        }
                        else
                        {
                            txtT.Visible = true;
                            txtT.Text = strOutput;
                            txtT.ToolTip = strOutput;
                            txtT.Enabled = false;
                            txtT.Columns = strOutput.Length + 2;

                        }
                        if (common.myBool(ViewState["AllowConfidentialTemplate"]) == false && common.myBool(ViewState["ConfidentialTemplate"]) == true)
                        {
                            btnAdd.Enabled = false;

                            btnAdd.Enabled = false;
                            txtT.Enabled = false;
                            txtM.Enabled = false;
                            btnHelp.Enabled = false;
                            ddlB.Enabled = false;
                            ddlD.Enabled = false;
                            txtDate.Enabled = false;
                            txtW.Enabled = false;
                        }
                    }
                    #endregion
                }
                else
                {
                    e.Row.Cells[(byte)enumNonT.Values].Text = "No Record Found!";
                }
                if (cCtlType.Equals("T") || cCtlType.Equals("M"))
                {
                    HtmlTextArea txtMulti1 = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                    RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                    ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                    btnAdd.Visible = false;
                    txtW.Visible = false;
                    txtMulti1.Visible = false;
                }

                HtmlTextArea txt = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                txt.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txt.ClientID + "');");
                txt.Attributes.Add("onkeydown", "javascript:return AutoChange('" + txt.ClientID + "');");
            }

            //RadTimePicker tpTime1 = sender as RadTimePicker;
            //GridViewRow row = tpTime1.NamingContainer as GridViewRow;




            //tpTime1.TimeView.Interval = new TimeSpan(0, 60, 0);
            //tpTime1.TimeView.StartTime = new TimeSpan(0, 0, 0);
            //tpTime1.TimeView.EndTime = new TimeSpan(24, 0, 0);

            //  DropDownList ddlSubCategories = (DropDownList)e.Row.FindControl("ddlSubCategories");

            //ddlSubCategories.DataTextField = "SubCategoryName";
            //ddlSubCategories.DataValueField = "ProductSubcategoryID";
            //ddlSubCategories.DataSource = RetrieveSubCategories();
            //ddlSubCategories.DataBind();
            //DataRowView dr = e.Row.DataItem as DataRowView;
            //ddlSubCategories.SelectedValue = dr["ProductSubCategoryID"].ToString();



            //if (tpTime1.SelectedDate != null)
            //{
            //    string min = tpTime1.SelectedDate.Value.Minute.ToString();
            //    min = min.Length == 1 ? "0" + min : min;
            //    ddlTime1.SelectedIndex = ddlTime1.Items.IndexOf(ddlTime1.Items.FindItemByValue(min));
            //}


        }
    }
    private void ControlEnabled()
    {
        //for (int i = 0; i < 23; i++)
        //{
        //    TextBox txtT + i = (TextBox)e.Row.FindControl("txtT");
        //    TextBox txtM0+ i = (TextBox)e.Row.FindControl("txtM0");
        //    DropDownList D0+ i = (DropDownList)e.Row.FindControl("D0");
        //    DropDownList B0+ i = (DropDownList)e.Row.FindControl("B0");
        //    AjaxControlToolkit.CalendarExtender CalendarExtender0 = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender0");
        //}
    }

    protected void gvTabularFormat_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem || e.Item is GridFooterItem || e.Item is GridHeaderItem)
        {
            if (e.Item is GridDataItem)
            {
                for (int i = 0; i < common.myInt(ViewState["TotalColCount"]) - 6; i++)
                {
                    Label lbl = (Label)e.Item.FindControl("lblFieldId" + i.ToString());
                    lbl.Text = common.myStr(DataBinder.Eval(e.Item.DataItem, "Col" + (i + 1).ToString()));
                    e.Item.ToolTip = lbl.Text;
                }
            }

            for (int stindex = common.myInt(ViewState["TotalColCount"]) - (6 - 2); stindex < e.Item.Cells.Count; stindex++)
            {
                e.Item.Cells[stindex].Visible = false;
            }
        }

        if (ViewState["FreezeSectionTree"] != null)
        {
            DataTable tbl = (DataTable)ViewState["FreezeSectionTree"];
            if (tbl.Rows.Count > 0)
            {
                gvTabularFormat.ClientSettings.Scrolling.FrozenColumnsCount = 1;
            }
            else
            {
                gvTabularFormat.ClientSettings.Scrolling.FrozenColumnsCount = 0;
            }
        }
        else
        {
            gvTabularFormat.ClientSettings.Scrolling.FrozenColumnsCount = 0;
        }
        try
        {
            if (e.Item is GridFooterItem)
            {
                if (!btnFormulaCalculate.Visible)
                {
                    e.Item.Visible = false;
                }
            }
        }
        catch
        {
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (!isSaved(false))
        {
            return;
        }

        saveData(true);
        //btnAddRow_Click(null, null); 
        BindBothGrids(false, false);
    }

    protected bool isSaved(bool isSaveManual)
    {
        ViewState["isSaveManual"] = isSaveManual;

        bool IsSave = true;
        StringBuilder sbInformativeMsg = new StringBuilder();
        StringBuilder sbRestrictiveMsg = new StringBuilder();

        try
        {
            int flag = 0;

            bool IsMandatory = false;
            bool IsEntered = true;
            string MandatoryType = string.Empty;
            string FielType = string.Empty;
            string FieldName = string.Empty;

            if (gvTabularFormat.Visible)
            {
                DataTable tbl = new DataTable();
                if (ViewState["TabularMandatoryField"] != null)
                {
                    tbl = (DataTable)ViewState["TabularMandatoryField"];

                    if (tbl.Rows.Count > 0)
                    {
                        DataView DV = tbl.DefaultView;
                        DV.RowFilter = "IsMandatory=1";

                        if (DV.ToTable().Rows.Count > 0)
                        {
                            IsMandatory = true;
                        }
                        DV.RowFilter = string.Empty;
                    }
                }

                if (IsMandatory)
                {
                    return true;

                    for (int i = 0, k = 1; i < gvTabularFormat.Items.Count; i++)
                    {
                        if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                        {
                            flag = 0;
                        }
                        else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                        {
                            k++;
                            if (flag.Equals(0))
                            {
                                ViewState["ValueOfControlType"] = i - 1;
                                flag = 1;
                            }

                            string FieldType;
                            IsEntered = false;

                            //for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++) //change 5 to 6
                            //{
                            //    Label lblControlFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            //    if (common.myInt(lblControlFieldId.Text) == 0)
                            //    {
                            //        continue;
                            //    }

                            //    Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j].FindControl("lblFieldId" + j.ToString());

                            //    FieldType = common.myStr(lblControlType.Text).Trim();

                            //    switch (FieldType)
                            //    {
                            //        case "T":
                            //            TextBox txtT = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());

                            //            if (common.myStr(txtT.Text).Trim().Length == 0)
                            //            {
                            //                IsEntered = IsEntered | false;
                            //            }
                            //            else
                            //            {
                            //                IsEntered = IsEntered | true;
                            //            }
                            //            break;


                            //        case "M":
                            //            TextBox txtM = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtM" + j.ToString());

                            //            if (common.myStr(txtM.Text).Trim().Length == 0)
                            //            {
                            //                IsEntered = IsEntered | false;
                            //            }
                            //            else
                            //            {
                            //                IsEntered = IsEntered | true;
                            //            }

                            //            break;

                            //        case "D":

                            //            DropDownList ddl = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("D" + j.ToString());

                            //            if (common.myInt(ddl.SelectedValue) == 0)
                            //            {
                            //                IsEntered = IsEntered | false;
                            //            }
                            //            else
                            //            {
                            //                IsEntered = IsEntered | true;
                            //            }

                            //            break;

                            //        case "B":
                            //            DropDownList ddlB = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("B" + j.ToString());

                            //            if (common.myStr(ddlB.SelectedItem.Text).Trim() == "Select"
                            //                || common.myStr(ddlB.SelectedItem.Text).Trim() == "")
                            //            {
                            //                IsEntered = IsEntered | false;
                            //            }
                            //            else
                            //            {
                            //                IsEntered = IsEntered | true;
                            //            }

                            //            break;

                            //        case "S":
                            //            TextBox txtDate = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtDate" + j.ToString());

                            //            if (common.myStr(txtDate.Text).Trim() == "__/__/____"
                            //                || common.myStr(txtDate.Text).Trim() == "")
                            //            {
                            //                IsEntered = IsEntered | false;
                            //            }
                            //            else
                            //            {
                            //                IsEntered = IsEntered | true;
                            //            }

                            //            break;
                            //    }
                            //}

                            for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++) //change 5 to 6
                            {
                                Label lblControlFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                                if (common.myInt(lblControlFieldId.Text).Equals(0))
                                {
                                    continue;
                                }

                                int FieldId = common.myInt(lblControlFieldId.Text);

                                Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j].FindControl("lblFieldId" + j.ToString());

                                Label lblHeading = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 1].Cells[j].FindControl("lblFieldId" + j.ToString());

                                FieldName = common.myStr(lblHeading.Text).Replace(hdnMandatoryStar.Value, string.Empty);
                                FieldType = common.myStr(lblControlType.Text).Trim();


                                IsMandatory = false;
                                //Added on 24-04-2014 for Naushad Start   Enterded refres
                                IsEntered = false;
                                //Added on 24-04-2014 for End
                                tbl = (DataTable)ViewState["TabularMandatoryField"];
                                if (tbl.Rows.Count > 0)
                                {
                                    DataView DV = tbl.DefaultView;
                                    DV.RowFilter = "FieldId=" + FieldId + " AND IsMandatory=1";

                                    if (DV.ToTable().Rows.Count > 0)
                                    {
                                        IsMandatory = true;
                                        MandatoryType = common.myStr(DV.ToTable().Rows[0]["MandatoryType"]);
                                    }
                                    DV.RowFilter = string.Empty;
                                }

                                string strMessageShow = "Field: " + FieldName + " at row no. " + (i - 2).ToString() + " !" + Environment.NewLine;

                                //Added on 17-04-2014 Start  Naushad Ali
                                Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                                //Added on 17-04-2014 End Naushad Ali
                                switch (FieldType)
                                {
                                    case "T":
                                        TextBox txtT = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());

                                        if (common.myStr(txtT.Text).Trim().Length.Equals(0))
                                        {
                                            IsEntered = IsEntered | false;
                                            //Added on 17-04-2014 Naushad
                                            IsSave = false;
                                            //Added on 17-04-2014 Naushad
                                        }
                                        else
                                        {
                                            IsEntered = IsEntered | true;
                                        }

                                        if (IsEntered && IsMandatory)
                                        {
                                            if (common.myStr(txtT.Text).Trim().Length.Equals(0))
                                            {
                                                //IsSave = false;
                                                //if (MandatoryType == "I")//Informative
                                                //{
                                                //    sbInformativeMsg.Append(strMessageShow);
                                                //}
                                                //else if (MandatoryType == "R")//Restrictive
                                                //{
                                                //    sbRestrictiveMsg.Append(strMessageShow);
                                                //}
                                                //Added on 17-04-2014 Start Naushad Ali
                                                if (!IsEdit(common.myInt(lblFieldId.Text)))
                                                {
                                                    IsSave = true;
                                                }
                                                else  //Condtidion  has been added for  by bass mantory for login users
                                                {
                                                    IsSave = false;
                                                    if (MandatoryType.Equals("I"))//Informative
                                                    {
                                                        sbInformativeMsg.Append(strMessageShow);
                                                    }
                                                    else if (MandatoryType.Equals("R"))//Restrictive
                                                    {
                                                        sbRestrictiveMsg.Append(strMessageShow);
                                                    }
                                                }
                                                //Added on 17-04-2014 End  Naushad Ali
                                            }
                                        }
                                        break;
                                    //case "I":
                                    //    TextBox txtT = (TextBox)gvTabularFormat.Rows[i].Cells[j].FindControl("txtT" + j.ToString());
                                    //    if (common.myStr(txtT.Text).Trim().Length == 0)
                                    //    {
                                    //        IsEntered = IsEntered | false;
                                    //        IsSave = false;
                                    //    }
                                    //    else
                                    //    {
                                    //        IsEntered = IsEntered | true;
                                    //    }

                                    //    if (IsEntered && IsMandatory)
                                    //    {
                                    //        if (common.myStr(txtT.Text).Trim().Length == 0)
                                    //        {                                               
                                    //            if (IsEdit(lblFieldId.Text) == 0)
                                    //            {
                                    //                IsSave = true;
                                    //            }
                                    //            else  //Condtidion  has been added for  by bass mantory for login users
                                    //            {
                                    //                IsSave = false;
                                    //                if (MandatoryType == "I")//Informative
                                    //                {
                                    //                    sbInformativeMsg.Append(strMessageShow);
                                    //                }
                                    //                else if (MandatoryType == "R")//Restrictive
                                    //                {
                                    //                    sbRestrictiveMsg.Append(strMessageShow);
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //    else
                                    //    {
                                    //        if (IsMandatory == false && IsEntered == false)
                                    //        {
                                    //            IsSave = true;
                                    //        }
                                    //        else
                                    //        {
                                    //            if ((IsEdit(lblFieldId.Text) == 0 && IsMandatory == true))
                                    //            {
                                    //                IsSave = true;
                                    //            }
                                    //            else if (IsEdit(lblFieldId.Text) > 0 && IsMandatory == true)
                                    //            {
                                    //                IsSave = false;
                                    //            }

                                    //            else if (IsEdit(lblFieldId.Text) > 0 && IsMandatory == false)
                                    //            {
                                    //                IsSave = true;
                                    //            }
                                    //            lblMandatoryMessage.Text = "Please Enter the Mandatory Field" + FieldName;
                                    //        }
                                    //    }
                                    //    break;

                                    case "M":
                                        TextBox txtM = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtM" + j.ToString());

                                        if (common.myStr(txtM.Text).Trim().Length.Equals(0))
                                        {
                                            IsEntered = IsEntered | false;
                                            //Added on 17-04-2014 Naushad
                                            IsSave = false;
                                            //Added on 17-04-2014 Naushad

                                        }
                                        else
                                        {
                                            IsEntered = IsEntered | true;
                                        }

                                        if (IsEntered && IsMandatory)
                                        {
                                            if (common.myStr(txtM.Text).Trim().Length.Equals(0))
                                            {
                                                //IsSave = false;
                                                //if (MandatoryType == "I")
                                                //{
                                                //    sbInformativeMsg.Append(strMessageShow);
                                                //}
                                                //else if (MandatoryType == "R")
                                                //{
                                                //    sbRestrictiveMsg.Append(strMessageShow);
                                                //}

                                                //Added on 17-04-2014 Start Naushad Ali
                                                if (!IsEdit(common.myInt(lblFieldId.Text)))
                                                {
                                                    IsSave = true;
                                                }
                                                else  //Condtidion  has been added for  by bass mantory for login users
                                                {
                                                    IsSave = false;
                                                    if (MandatoryType.Equals("I"))//Informative
                                                    {
                                                        sbInformativeMsg.Append(strMessageShow);
                                                    }
                                                    else if (MandatoryType.Equals("R"))//Restrictive
                                                    {
                                                        sbRestrictiveMsg.Append(strMessageShow);
                                                    }
                                                }
                                                //Added on 17-04-2014 End  Naushad Ali
                                            }
                                        }
                                        //Added on 23-04-2014 
                                        else
                                        {
                                            if (!IsMandatory && !IsEntered)
                                            {
                                                IsSave = true;
                                            }
                                            else
                                            {
                                                if (!IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = false;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && !IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                lblMandatoryMessage.Text = "Please Enter the Mandatory Field" + FieldName;
                                            }
                                        }
                                        //Added on 23-04-2014
                                        break;

                                    case "D":

                                        DropDownList ddl = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("D" + j.ToString());

                                        if (common.myInt(ddl.SelectedValue).Equals(0))
                                        {
                                            IsEntered = IsEntered | false;
                                            //Added on 17-04-2014 Naushad
                                            IsSave = false;
                                            //Added on 17-04-2014 Naushad
                                        }
                                        else
                                        {
                                            IsEntered = IsEntered | true;
                                        }

                                        if (IsEntered && IsMandatory)
                                        {
                                            if (common.myInt(ddl.SelectedValue).Equals(0))
                                            {
                                                //IsSave = false;
                                                //if (MandatoryType == "I")//Informative
                                                //{
                                                //    sbInformativeMsg.Append(strMessageShow);
                                                //}
                                                //else if (MandatoryType == "R")//Restrictive
                                                //{
                                                //    sbRestrictiveMsg.Append(strMessageShow);
                                                //}
                                                //Added on 17-04-2014 Start Naushad Ali
                                                if (!IsEdit(common.myInt(lblFieldId.Text)))
                                                {
                                                    IsSave = true;
                                                }
                                                else  //Condtidion  has been added for  by bass mantory for login users
                                                {
                                                    IsSave = false;
                                                    if (MandatoryType.Equals("I"))//Informative
                                                    {
                                                        sbInformativeMsg.Append(strMessageShow);
                                                    }
                                                    else if (MandatoryType.Equals("R"))//Restrictive
                                                    {
                                                        sbRestrictiveMsg.Append(strMessageShow);
                                                    }
                                                    return IsSave;
                                                }
                                                //Added on 17-04-2014 End  Naushad Ali
                                            }
                                        }
                                        //Added on 24-04-2014 
                                        else
                                        {
                                            if (!IsMandatory && !IsEntered)
                                            {
                                                IsSave = true;
                                            }
                                            else
                                            {
                                                if (!IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = false;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && !IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                //IsSave = false;
                                                lblMandatoryMessage.Text = "Please Enter the Mandatory Field" + FieldName;
                                            }
                                        }
                                        //Added on 24-04-2014
                                        break;

                                    case "B":
                                        DropDownList ddlB = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("B" + j.ToString());

                                        if (common.myStr(ddlB.SelectedItem.Text).Trim().ToUpper().Equals("SELECT")
                                            || common.myStr(ddlB.SelectedItem.Text).Trim().Equals(string.Empty))
                                        {
                                            IsEntered = IsEntered | false;
                                        }
                                        else
                                        {
                                            IsEntered = IsEntered | true;
                                        }

                                        if (IsEntered && IsMandatory)
                                        {
                                            if (common.myStr(ddlB.SelectedItem.Text).Trim().ToUpper().Equals("SELECT")
                                                || common.myStr(ddlB.SelectedItem.Text).Trim().Equals(string.Empty))
                                            {
                                                IsSave = false;

                                                if (MandatoryType.Equals("I"))//Informative
                                                {
                                                    sbInformativeMsg.Append(strMessageShow);
                                                }
                                                else if (MandatoryType.Equals("R"))//Restrictive
                                                {
                                                    sbRestrictiveMsg.Append(strMessageShow);
                                                }
                                                return IsSave;
                                            }
                                        }
                                        //Added on 23-04-2014 
                                        else
                                        {
                                            if (!IsMandatory && !IsEntered)
                                            {
                                                IsSave = true;
                                            }
                                            else
                                            {
                                                if (!IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = false;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && !IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                lblMandatoryMessage.Text = "Please Enter the Mandatory Field" + FieldName;
                                            }
                                        }
                                        //Added on 23-04-2014
                                        break;

                                    case "S":
                                        TextBox txtDate = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtDate" + j.ToString());

                                        if (common.myStr(txtDate.Text).Trim().Equals("__/__/____")
                                            || common.myStr(txtDate.Text).Trim().Equals(string.Empty))
                                        {
                                            IsEntered = IsEntered | false;

                                            //Added on 17-04-2014 Naushad
                                            IsSave = false;
                                            //Added on 17-04-2014 Naushad
                                        }
                                        else
                                        {
                                            IsEntered = IsEntered | true;
                                        }

                                        if (IsEntered && IsMandatory)
                                        {
                                            if (common.myStr(txtDate.Text).Trim().Equals("__/__/____")
                                                || common.myStr(txtDate.Text).Trim().Equals(string.Empty))
                                            {
                                                //IsSave = false;

                                                //if (MandatoryType == "I")//Informative
                                                //{
                                                //    sbInformativeMsg.Append(strMessageShow);
                                                //}
                                                //else if (MandatoryType == "R")//Restrictive
                                                //{
                                                //    sbRestrictiveMsg.Append(strMessageShow);
                                                //}

                                                //Added on 17-04-2014 Start Naushad Ali
                                                if (!IsEdit(common.myInt(lblFieldId.Text)))
                                                {
                                                    IsSave = true;
                                                }
                                                else  //Condtidion  has been added for  by bass mantory for login users
                                                {
                                                    IsSave = false;
                                                    if (MandatoryType.Equals("I"))//Informative
                                                    {
                                                        sbInformativeMsg.Append(strMessageShow);
                                                    }
                                                    else if (MandatoryType.Equals("R"))//Restrictive
                                                    {
                                                        sbRestrictiveMsg.Append(strMessageShow);
                                                    }
                                                }
                                                //Added on 17-04-2014 End  Naushad Ali
                                            }
                                        }
                                        //Added on 23-04-2014 
                                        else
                                        {
                                            if (!IsMandatory && !IsEntered)
                                            {
                                                IsSave = true;
                                            }
                                            else
                                            {
                                                if (!IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && IsMandatory)
                                                {
                                                    IsSave = false;
                                                }
                                                else if (IsEdit(common.myInt(lblFieldId.Text)) && !IsMandatory)
                                                {
                                                    IsSave = true;
                                                }
                                                lblMandatoryMessage.Text = "Please Enter the Mandatory Field" + FieldName;
                                            }
                                        }
                                        //Added on 23-04-2014
                                        break;
                                }
                                //Added on 23-04-2014 Start Naushad
                                if (!IsSave)
                                {
                                    break;
                                }
                                //Added on 23-04-2014 Start Naushad

                            }
                        }
                    }
                }
            }
            else if (gvSelectedServices.Visible)
            {
                foreach (GridViewRow item2 in gvSelectedServices.Rows)
                {
                    if (item2.RowType.Equals(DataControlRowType.DataRow))
                    {
                        FielType = common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim();
                        FieldName = common.myStr(item2.Cells[(byte)enumNonT.FieldName].Text).Replace(hdnMandatoryStar.Value, string.Empty);
                        IsMandatory = common.myBool(item2.Cells[(byte)enumNonT.IsMandatory].Text);

                        //Added on 17-04-2014 by Naushad Ali Start
                        HiddenField hdnEmployeeTypeID = (HiddenField)item2.FindControl("hdnEmployeeTypeID");
                        HiddenField hdnIsEmployeeTypeTagged = (HiddenField)item2.FindControl("hdnIsEmployeeTypeTagged");



                        if (common.myInt(item2.Cells[(byte)enumNonT.EmployeeTypeId].Text) > 0)
                        {
                            if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            {
                                IsMandatory = true;
                                lblMandatoryMessage.Text = "Please Enter the Detail of" + FieldName;
                            }
                            else
                            {
                                IsMandatory = false;
                                lblMandatoryMessage.Text = string.Empty;
                            }
                        }
                        //else
                        //{
                        //    lblMandatoryMessage.Text = "Please Enter the Detail of" + FieldName;
                        //}

                        //Added on 17-04-2014 by Naushad Ali End

                        MandatoryType = common.myStr(item2.Cells[(byte)enumNonT.MandatoryType].Text).Trim();

                        if (IsMandatory)
                        {
                            IsEntered = true;

                            switch (FielType)
                            {
                                case "T":
                                    TextBox txtT = (TextBox)item2.FindControl("txtT");

                                    if (common.myStr(txtT.Text).Trim().Length.Equals(0) && txtT.Visible == true)
                                    {
                                        IsEntered = false;
                                    }
                                    break;


                                case "M":
                                    TextBox txtM = (TextBox)item2.FindControl("txtM");
                                    if (common.myStr(txtM.Text).Trim().Length.Equals(0) && txtM.Visible == true)
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "W":
                                    RadEditor txtW = (RadEditor)item2.FindControl("txtW");

                                    if (common.myStr(txtW.Content).Trim().Length.Equals(0) && txtW.Visible == true)
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "D":
                                    DropDownList ddl = (DropDownList)item2.FindControl("D");

                                    if (common.myInt(ddl.SelectedValue).Equals(0))
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "B":
                                    RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                                    if (common.myStr(ddlB.SelectedItem.Text).Trim().ToUpper().Equals("SELECT")
                                        || common.myStr(ddlB.SelectedItem.Text).Trim().Equals(string.Empty))
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "C":
                                    DataList rptC = (DataList)item2.FindControl("C");
                                    bool isChk = false;
                                    foreach (DataListItem rptItem in rptC.Items)
                                    {
                                        CheckBox chk = (CheckBox)rptItem.FindControl("C");
                                        isChk = isChk | chk.Checked;
                                    }

                                    if (!isChk)
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "S":
                                    TextBox txtDate = (TextBox)item2.FindControl("txtDate");

                                    if (common.myStr(txtDate.Text).Trim().Equals("__/__/____")
                                        || common.myStr(txtDate.Text).Trim().Equals(string.Empty))
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "ST":

                                    RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                                    RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");
                                    //TextBox txtDate = (TextBox)item2.FindControl("txtDate");

                                    if (tpTime.SelectedDate != null && tpTime.SelectedDate.GetValueOrDefault() != DateTime.MinValue)

                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "SB":

                                    TextBox txtDate1 = (TextBox)item2.FindControl("txtDate");
                                    RadTimePicker tpTime1 = (RadTimePicker)item2.FindControl("tpTime");
                                    RadComboBox ddlTime1 = (RadComboBox)item2.FindControl("ddlTime");

                                    if ((common.myStr(txtDate1.Text).Trim().Equals("__/__/____")
                                        || common.myStr(txtDate1.Text).Trim().Equals(string.Empty))
                                        && (tpTime1.SelectedDate != null && tpTime1.SelectedDate.GetValueOrDefault() != DateTime.MinValue))
                                    {
                                        IsEntered = false;
                                    }
                                    break;

                                case "H":
                                    break;
                            }


                            IsSave = IsSave & IsEntered;

                            if (!IsEntered)
                            {
                                if (MandatoryType.Equals("I"))//Informative
                                {
                                    sbInformativeMsg.Append("Field: " + FieldName + " !" + Environment.NewLine);
                                }
                                else if (MandatoryType.Equals("R"))//Restrictive
                                {
                                    sbRestrictiveMsg.Append("Field: " + FieldName + " !" + Environment.NewLine);
                                }
                            }

                        }

                    }
                    //Added on 24-04-2014 for Mandary Filed
                    if (!IsSave)
                    {
                        lblMandatoryMessage.Text = "Please Enter the Detail of  " + FieldName;
                        break;
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

        if (!IsSave)
        {
            dvMandatory.Visible = true;

            //Added on 17-04-2014
            //btnMandatorySave.Visible = false;
            //btnMandatoryCancel.Visible = false;
            //btnMandatoryOk.Visible = false;

            btnMandatorySave.Visible = false;
            btnMandatoryCancel.Visible = true;
            btnMandatoryOk.Visible = true;

            if (!sbRestrictiveMsg.ToString().Equals(string.Empty))
            {
                lblMandatoryMessage.Text = "Following field(s) are mandatory !" + Environment.NewLine + sbRestrictiveMsg.ToString();

                btnMandatoryOk.Visible = true;
            }
            else if (!sbInformativeMsg.ToString().Equals(string.Empty))
            {
                lblMandatoryMessage.Text = "Do you want to save blank field ?" + Environment.NewLine + sbInformativeMsg.ToString();

                btnMandatorySave.Visible = true;
                btnMandatoryCancel.Visible = true;
            }
        }

        return IsSave;
    }

    private void saveData(bool isSaveManual)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        DataSet dsTemplateDataDetails = new DataSet();
        //Hashtable hstInput = new Hashtable();
        StringBuilder strTabular = new StringBuilder();
        StringBuilder strNonTabular = new StringBuilder();
        ArrayList coll = new ArrayList();
        int flag = 0;
        int RowCaptionId = 0;
        int SectionId = 0;
        int FieldId;
        string FieldType;
        string FieldValue;
        bool IsDataFound = false;

        int SubDeptId = 0;
        int OrderRequestId = 0;
        int OrderDetailId = 0;

        bool IsAddendum = false;
        try
        {


            //if (common.myStr(Request.QueryString["CF"]) == "LAB")
            //{
            //    return;
            //}           
            if (!common.myBool(Session["isEMRSuperUser"]))
            {
                if (common.myBool(ViewState["IsEncounterClose"]) || !common.myBool(btnSave.Visible))
                {
                    if (!common.myBool(Request.QueryString["IsAddendum"]))
                    {
                        return;
                    }
                }
            }

            //if (common.myInt(ddlEpisode.SelectedValue) > 0)
            //{
            //    if (common.myBool(ddlEpisode.SelectedItem.Attributes["EpisodeClosed"]))
            //    {
            //        return;
            //    }
            //}

            if (gvTabularFormat.Visible)
            {
                #region Tabular
                if (btnFormulaCalculate.Visible)
                {
                    btnFormulaCalculate_OnClick(null, null);
                }

                //it is for first time only
                for (int i = 0, k = 1; i < gvTabularFormat.Items.Count; i++)
                {
                    if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                    {
                        flag = 0;
                    }
                    else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                    {
                        if (flag.Equals(0))
                        {
                            ViewState["ValueOfControlType"] = i - 1;
                            flag = 1;
                        }

                        RowCaptionId = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.RowCaptionId].Text);
                        SectionId = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text);

                        IsDataFound = false;

                        for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++)
                        {
                            FieldId = 0;
                            FieldType = string.Empty;
                            FieldValue = string.Empty;

                            Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            if (common.myInt(lblControlType.Text).Equals(0))
                            {
                                continue;
                            }

                            FieldId = common.myInt(lblControlType.Text);
                            //coll.Add(lblControlType.Text);

                            lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j].FindControl("lblFieldId" + j.ToString());

                            FieldType = common.myStr(lblControlType.Text);
                            //coll.Add(lblControlType.Text);

                            if (lblControlType.Text.Equals("T"))
                            {
                                TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());

                                if (!txtVisData.Text.Trim().Equals(string.Empty))
                                {
                                    //coll.Add(txtVisData.Text);
                                    FieldValue = txtVisData.Text;
                                }
                            }
                            //if (lblControlType.Text == "I")
                            //{
                            //    TextBox txtVisData = (TextBox)gvTabularFormat.Rows[i].Cells[j].FindControl("txtT" + j.ToString());

                            //    if (txtVisData.Text.Trim() != "")
                            //    {
                            //        //coll.Add(txtVisData.Text);
                            //        FieldValue = txtVisData.Text;
                            //    }
                            //}
                            //if (lblControlType.Text == "IS")
                            //{
                            //    TextBox txtVisData = (TextBox)gvTabularFormat.Rows[i].Cells[j].FindControl("txtM" + j.ToString());

                            //    if (txtVisData.Text.Trim() != "")
                            //    {
                            //        //coll.Add(txtVisData.Text);
                            //        FieldValue = txtVisData.Text;
                            //    }
                            //}
                            else if (lblControlType.Text.Equals("M"))
                            {
                                TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtM" + j.ToString());

                                if (!txtVisData.Text.Trim().Equals(string.Empty))
                                {
                                    //coll.Add(txtVisData.Text);
                                    FieldValue = txtVisData.Text;
                                }
                            }
                            else if (lblControlType.Text.Equals("D"))
                            {
                                Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                                DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("D" + j.ToString());

                                if (!ddlVisData.SelectedItem.Text.Equals("Select"))
                                {
                                    //coll.Add(ddlVisData.SelectedValue);
                                    FieldValue = ddlVisData.SelectedValue;
                                }
                            }
                            else if (lblControlType.Text.Equals("IM"))
                            {
                                Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                                RadComboBox ddlVisData = (RadComboBox)gvTabularFormat.Items[i].Cells[j].FindControl("IM" + j.ToString());

                                if (!ddlVisData.SelectedItem.Text.Equals("Select"))
                                {
                                    //coll.Add(ddlVisData.SelectedValue);
                                    FieldValue = ddlVisData.SelectedValue;
                                }
                            }
                            else if (lblControlType.Text.Equals("R"))
                            {
                                Label lblFieldId = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());
                                RadioButtonList ddlVisData = (RadioButtonList)gvTabularFormat.Items[i].Cells[j].FindControl("R" + j.ToString());
                                if (ddlVisData != null)
                                {
                                    if (!ddlVisData.SelectedValue.Equals(string.Empty))
                                    {
                                        FieldValue = ddlVisData.SelectedValue;
                                    }
                                }
                            }
                            else if (lblControlType.Text.Equals("B"))
                            {
                                DropDownList ddlVisData = (DropDownList)gvTabularFormat.Items[i].Cells[j].FindControl("B" + j.ToString());
                                if (ddlVisData != null)
                                {
                                    if (!ddlVisData.SelectedItem.Text.Equals("Select"))
                                    {
                                        //coll.Add(ddlVisData.SelectedValue);
                                        FieldValue = ddlVisData.SelectedValue;
                                    }
                                }
                            }
                            else if (lblControlType.Text.Equals("S"))
                            {
                                TextBox txtDate = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtDate" + j.ToString());

                                if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                                {
                                    if (!txtDate.Text.Trim().Equals(string.Empty))
                                    {
                                        //coll.Add(common.myStr(bC.FormatDate(txtDate.Text, Session["OutputDateFormat"].ToString(), "dd/MM/yyyy")));

                                        //coll.Add(Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy"));
                                        FieldValue = txtDate.Text.Trim();
                                    }
                                }
                                else
                                {
                                    FieldValue = string.Empty;
                                }
                            }
                            else
                            {
                                FieldValue = string.Empty;
                            }

                            if (!FieldValue.Equals(string.Empty))
                            {
                                coll.Add(FieldId);//FieldId int,                                  
                                coll.Add(FieldType);//FieldType char(1),                              
                                coll.Add(FieldValue);//FieldValue varchar(Max),
                                coll.Add(k);//RowNo int,
                                coll.Add(common.myInt(RowCaptionId));//RowCaptionId int

                                strTabular.Append(common.setXmlTable(ref coll));

                                IsDataFound = true;
                            }

                            if (i.Equals(gvTabularFormat.Items.Count - 1))
                            {
                                if (btnFormulaCalculate.Visible)
                                {
                                    if (FieldType.Equals("T"))
                                    {
                                        if (ViewState["TabularMandatoryField"] != null)
                                        {
                                            DataTable tbl = (DataTable)ViewState["TabularMandatoryField"];

                                            if (tbl != null)
                                            {
                                                if (tbl.Rows.Count > 0)
                                                {
                                                    DataView DV = tbl.Copy().DefaultView;

                                                    DV.RowFilter = "FieldId=" + FieldId;

                                                    tbl = DV.ToTable();
                                                    if (tbl.Rows.Count > 0)
                                                    {
                                                        if (common.myBool(tbl.Rows[0]["TotalCalc"]))
                                                        {
                                                            GridFooterItem footerItem = gvTabularFormat.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

                                                            Label lblTfooterTotal = (Label)footerItem.Cells[j].FindControl("lblT" + j.ToString());
                                                            //Label lblTfooterTotal = (Label)gvTabularFormat.FooterRow.FindControl("lblT" + j.ToString());

                                                            if (!lblTfooterTotal.Text.Equals(string.Empty))
                                                            {
                                                                coll.Add(FieldId);//FieldId int,                                  
                                                                coll.Add(FieldType);//FieldType char(1),                              
                                                                coll.Add(lblTfooterTotal.Text);//FieldValue varchar(Max),
                                                                coll.Add(k);//RowNo int,
                                                                coll.Add(common.myInt(RowCaptionId));//RowCaptionId int

                                                                strTabular.Append(common.setXmlTable(ref coll));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        if (IsDataFound)
                        {
                            k++;
                        }

                        //if (strTabular.ToString() != "")
                        //{
                        //    SectionId = common.myInt(gvTabularFormat.Rows[i].Cells[(byte)enumT.SectionId].Text);
                        //}
                    }
                }

                if (strTabular.ToString().Equals(string.Empty))
                {
                    DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];

                    if (tblRefField != null)
                    {
                        if (tblRefField.Rows.Count > 0)
                        {
                            for (int rowIdx = 0; rowIdx < tblRefField.Rows.Count; rowIdx++)
                            {
                                coll = new ArrayList();

                                coll.Add(common.myInt(tblRefField.Rows[rowIdx]["FieldId"]));//FieldId int,                                  
                                coll.Add(common.myStr(tblRefField.Rows[rowIdx]["FieldType"]));//FieldType char(1),                              
                                coll.Add(string.Empty);//FieldValue varchar(Max),
                                coll.Add(1);//RowNo int,
                                coll.Add(common.myInt(RowCaptionId));//RowCaptionId int

                                strTabular.Append(common.setXmlTable(ref coll));
                            }
                        }

                    }
                }
                #endregion
            }
            else if (gvSelectedServices.Visible)
            {
                #region Non Tabular
                if (btnScoreCalc.Visible)
                {
                    btnScoreCalc_Onclick(null, null);
                }

                RowCaptionId = 0;
                coll = new ArrayList();

                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                    && !common.myStr(ViewState["TagType"]).Equals("D"))
                {
                    if (Session["TemplateDataDetails"] == null)
                    {
                        return;
                    }

                    dsTemplateDataDetails = (DataSet)Session["TemplateDataDetails"];

                    if (dsTemplateDataDetails.Tables.Count > 0)
                    {
                        DataTable tblItem = new DataTable();
                        tblItem = dsTemplateDataDetails.Tables[0];

                        DataView DVItem = tblItem.Copy().DefaultView;

                        bool IsAnyChk = false;
                        foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                        {
                            CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                            IsAnyChk = IsAnyChk | chkRow.Checked;
                        }

                        foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                        {
                            strNonTabular = new StringBuilder();

                            CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");

                            if ((common.myBool(chkRow.Checked) && common.myBool(chkRow.Visible))
                                || !IsAnyChk)
                            {
                                HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");

                                int SelectedServiceId = common.myInt(hdnServiceId.Value);

                                if (!IsAnyChk)
                                {
                                    SelectedServiceId = common.myInt(ViewState["TemplateRequiredServiceId"]);
                                }
                                foreach (GridViewRow item2 in gvSelectedServices.Rows)
                                {
                                    if (item2.RowType.Equals(DataControlRowType.DataRow))
                                    {
                                        SectionId = common.myInt(item2.Cells[(byte)enumNonT.SectionId].Text);

                                        if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("T"))
                                        {
                                            TextBox txtT = (TextBox)item2.FindControl("txtT");
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("T");
                                            coll.Add(txtT.Text);

                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");

                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        if (item2.Cells[(byte)enumNonT.FieldType].Text.Equals("I"))
                                        {
                                            TextBox txtT = (TextBox)item2.FindControl("txtT");
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("I");
                                            coll.Add(txtT.Text);
                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("IS"))
                                        {
                                            TextBox txtM = (TextBox)item2.FindControl("txtM");
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("IS");
                                            if (common.myStr(txtM.Text).Contains("<"))
                                            {
                                                txtM.Text = txtM.Text.Replace("<", "&lt;");
                                            }
                                            if (common.myStr(txtM.Text).Contains(">"))
                                            {
                                                txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
                                            }

                                            coll.Add(txtM.Text);
                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("M"))
                                        {
                                            TextBox txtM = (TextBox)item2.FindControl("txtM");
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("M");
                                            if (common.myStr(txtM.Text).Contains("<"))
                                            {
                                                txtM.Text = txtM.Text.Replace("<", "&lt;");
                                            }
                                            if (common.myStr(txtM.Text).Contains(">"))
                                            {
                                                txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
                                            }
                                            coll.Add(txtM.Text);
                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("W")) // For Word Processor
                                        {
                                            RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("W");
                                            coll.Add(txtW.Content);

                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("D"))
                                        {
                                            DropDownList ddl = (DropDownList)item2.FindControl("D");

                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("D");
                                            coll.Add(ddl.SelectedValue);

                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("IM"))
                                        {
                                            RadComboBox ddl = (RadComboBox)item2.FindControl("IM");

                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("IM");
                                            coll.Add(ddl.SelectedValue);

                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("R"))
                                        {
                                            RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("R");
                                            coll.Add(ddl.SelectedValue);

                                            HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));

                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("B"))
                                        {
                                            RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                                            if (ddlB.SelectedItem != null)
                                            {
                                                if (!ddlB.SelectedItem.Text.Equals("Select"))
                                                {
                                                    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                                    coll.Add("B");
                                                    coll.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);

                                                    HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                                    coll.Add("0");
                                                    coll.Add(RowCaptionId);

                                                    coll.Add(common.myInt(SelectedServiceId));
                                                    coll.Add(common.myInt(ViewState["PageId"]));

                                                    strNonTabular.Append(common.setXmlTable(ref coll));
                                                }
                                                else
                                                {
                                                    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                                    coll.Add("B");
                                                    coll.Add(null);

                                                    HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                                    coll.Add("0");
                                                    coll.Add(RowCaptionId);

                                                    coll.Add(common.myInt(SelectedServiceId));
                                                    coll.Add(common.myInt(ViewState["PageId"]));

                                                    strNonTabular.Append(common.setXmlTable(ref coll));
                                                }
                                            }
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("C"))
                                        {
                                            DataList rptC = (DataList)item2.FindControl("C");
                                            string sCheckedValues = string.Empty;
                                            foreach (DataListItem rptItem in rptC.Items)
                                            {
                                                CheckBox chk = (CheckBox)rptItem.FindControl("C");
                                                HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                                                HtmlTextArea CT = (HtmlTextArea)rptItem.FindControl("CT");

                                                sCheckedValues = chk.Checked == true ? hdn.Value : "0";
                                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                                coll.Add("C");
                                                coll.Add(sCheckedValues);
                                                coll.Add("0");
                                                coll.Add(RowCaptionId);
                                                coll.Add(common.myInt(SelectedServiceId));
                                                coll.Add(common.myInt(ViewState["PageId"]));

                                                strNonTabular.Append(common.setXmlTable(ref coll));
                                            }
                                            sCheckedValues = string.Empty;
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("S"))
                                        {
                                            TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                                            RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                                            RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                                            if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                                            {
                                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                                coll.Add("S");

                                                DateTime d = tpTime.SelectedDate.Value;
                                                string time = d.ToString("HH:mm:ss");
                                                coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));


                                                coll.Add("0");
                                                coll.Add(RowCaptionId);

                                                coll.Add(common.myInt(SelectedServiceId));
                                                coll.Add(common.myInt(ViewState["PageId"]));

                                                strNonTabular.Append(common.setXmlTable(ref coll));
                                            }
                                        }
                                        else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("H"))
                                        {
                                            coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                            coll.Add("H");
                                            coll.Add("-");
                                            coll.Add("0");
                                            coll.Add(RowCaptionId);

                                            coll.Add(common.myInt(SelectedServiceId));
                                            coll.Add(common.myInt(ViewState["PageId"]));

                                            strNonTabular.Append(common.setXmlTable(ref coll));
                                        }

                                    }
                                }
                                if (ViewState["ResultSet"] == null)
                                {
                                    ViewState["ResultSet"] = strNonTabular;
                                }
                                else
                                {
                                    ViewState["ResultSet"] = common.myStr(ViewState["ResultSet"]) + strNonTabular;
                                }

                                DVItem.RowFilter = "ISNULL(EncounterId,0) = " + common.myInt(ViewState["EncounterId"]) +
                                               " AND ServiceId = " + common.myInt(SelectedServiceId) +
                                               " AND TemplateId = " + common.myInt(ddlTemplateMain.SelectedValue) +
                                               " AND RegistrationId = " + common.myInt(ViewState["RegistrationId"]) +
                                               " AND SectionId = " + SectionId;

                                if (DVItem.ToTable().Rows.Count > 0)
                                {
                                    int rowIdx = 0;
                                    foreach (DataRow DRItem in tblItem.Rows)
                                    {
                                        if (common.myInt(DRItem["EncounterId"]).Equals(common.myInt(ViewState["EncounterId"]))
                                            && common.myInt(DRItem["ServiceId"]).Equals(common.myInt(SelectedServiceId))
                                            && common.myInt(DRItem["RegistrationId"]).Equals(common.myInt(ViewState["RegistrationId"]))
                                            && common.myInt(DRItem["SectionId"]).Equals(common.myInt(SectionId))
                                            && common.myInt(DRItem["TemplateId"]).Equals(common.myInt(ddlTemplateMain.SelectedValue)))
                                        {
                                            tblItem.Rows[rowIdx]["RegistrationId"] = common.myInt(ViewState["RegistrationId"]);
                                            tblItem.Rows[rowIdx]["EncounterId"] = common.myInt(ViewState["EncounterId"]);
                                            tblItem.Rows[rowIdx]["xmlTemplateDetails"] = strNonTabular.ToString();
                                            tblItem.Rows[rowIdx]["SectionId"] = SectionId;
                                            tblItem.Rows[rowIdx]["ServiceId"] = common.myInt(SelectedServiceId);
                                            tblItem.Rows[rowIdx]["TemplateId"] = common.myInt(ddlTemplateMain.SelectedValue);

                                            tblItem.AcceptChanges();
                                            break;
                                        }
                                        rowIdx++;
                                    }
                                }
                                else
                                {
                                    DataRow DR = tblItem.NewRow();

                                    DR["RegistrationId"] = common.myInt(ViewState["RegistrationId"]);
                                    DR["EncounterId"] = common.myInt(ViewState["EncounterId"]);
                                    DR["xmlTemplateDetails"] = strNonTabular.ToString();
                                    DR["SectionId"] = SectionId;
                                    DR["ServiceId"] = common.myInt(SelectedServiceId);
                                    DR["TemplateId"] = common.myInt(ddlTemplateMain.SelectedValue);

                                    tblItem.Rows.Add(DR);
                                    tblItem.AcceptChanges();
                                }

                                DataView DV = tblItem.Copy().DefaultView;
                                DV.RowFilter = "ISNULL(EncounterId,0)<>0 AND ISNULL(ServiceId,0)<>0 AND ISNULL(RegistrationId,0)<>0 " +
                                    " AND ISNULL(SectionId,0)<>0 AND ISNULL(TemplateId,0)<>0";

                                dsTemplateDataDetails = new DataSet();
                                dsTemplateDataDetails.Tables.Add(DV.ToTable());
                            }

                            if (!IsAnyChk)
                            {
                                break;
                            }
                        }

                        Session["TemplateDataDetails"] = dsTemplateDataDetails;

                        if (isSaveManual)
                        {
                            setServiceChkVisiblity();
                        }

                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = "Data saved successfully !";
                    }

                    return;
                }
                else
                {
                    foreach (GridViewRow item2 in gvSelectedServices.Rows)
                    {
                        if (item2.RowType.Equals(DataControlRowType.DataRow))
                        {
                            SectionId = common.myInt(item2.Cells[(byte)enumNonT.SectionId].Text);

                            if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("T"))
                            {
                                TextBox txtT = (TextBox)item2.FindControl("txtT");
                                //if (txtT.Text.Trim() != "")
                                //{
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("T");
                                coll.Add(txtT.Text);

                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");

                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                                //}
                            }
                            if (item2.Cells[(byte)enumNonT.FieldType].Text.Equals("I"))
                            {
                                TextBox txtT = (TextBox)item2.FindControl("txtT");
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("I");
                                coll.Add(txtT.Text);
                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                            if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("IS"))
                            {
                                TextBox txtM = (TextBox)item2.FindControl("txtM");
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("IS");
                                if (common.myStr(txtM.Text).Contains("<"))
                                {
                                    txtM.Text = txtM.Text.Replace("<", "&lt;");
                                }
                                if (common.myStr(txtM.Text).Contains(">"))
                                {
                                    txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
                                }
                                coll.Add(txtM.Text);
                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("M"))
                            {
                                TextBox txtM = (TextBox)item2.FindControl("txtM");
                                //if (txtM.Text.Trim() != "")
                                //{
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("M");
                                if (common.myStr(txtM.Text).Contains("<"))
                                {
                                    txtM.Text = txtM.Text.Replace("<", "&lt;");
                                }
                                if (common.myStr(txtM.Text).Contains(">"))
                                {
                                    txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
                                }

                                coll.Add(txtM.Text);

                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                                //}
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("W")) // For Word Processor
                            {
                                RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                                //if (txtW.Content != null)
                                //{
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("W");
                                coll.Add(txtW.Content);

                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                                //}
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("D"))
                            {
                                DropDownList ddl = (DropDownList)item2.FindControl("D");
                                // if (ddl.SelectedItem.Text != "Select")
                                // {
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("D");
                                coll.Add(ddl.SelectedValue);

                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                                //}
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("IM"))
                            {
                                RadComboBox ddl = (RadComboBox)item2.FindControl("IM");
                                // if (ddl.SelectedItem.Text != "Select")
                                // {
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("IM");
                                coll.Add(ddl.SelectedValue);

                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                                //}
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("R"))
                            {
                                RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
                                // if (ddl.SelectedItem.Text != "Select")
                                // {
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("R");
                                coll.Add(ddl.SelectedValue);

                                HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                                //}
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("B"))
                            {
                                RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                                if (ddlB.SelectedItem != null)
                                {
                                    if (!ddlB.SelectedItem.Text.Equals("Select"))
                                    {
                                        coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                        coll.Add("B");
                                        coll.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);

                                        HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                        coll.Add("0");
                                        coll.Add(RowCaptionId);

                                        if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                        {
                                            coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                            coll.Add(common.myInt(ViewState["PageId"]));
                                        }

                                        strNonTabular.Append(common.setXmlTable(ref coll));
                                    }
                                    else
                                    {
                                        coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                        coll.Add("B");
                                        coll.Add(null);

                                        HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
                                        coll.Add("0");
                                        coll.Add(RowCaptionId);

                                        if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                        {
                                            coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                            coll.Add(common.myInt(ViewState["PageId"]));
                                        }

                                        strNonTabular.Append(common.setXmlTable(ref coll));
                                    }
                                }
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("C"))
                            {
                                DataList rptC = (DataList)item2.FindControl("C");
                                string sCheckedValues = string.Empty;
                                foreach (DataListItem rptItem in rptC.Items)
                                {
                                    CheckBox chk = (CheckBox)rptItem.FindControl("C");
                                    HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                                    HtmlTextArea CT = (HtmlTextArea)rptItem.FindControl("CT");

                                    sCheckedValues = chk.Checked == true ? hdn.Value : "0";
                                    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                    coll.Add("C");
                                    coll.Add(sCheckedValues);
                                    coll.Add("0");
                                    coll.Add(RowCaptionId);
                                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                    {
                                        coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                        coll.Add(common.myInt(ViewState["PageId"]));
                                    }
                                    strNonTabular.Append(common.setXmlTable(ref coll));
                                }
                                sCheckedValues = string.Empty;
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("S"))
                            {
                                TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                                RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                                RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                                if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                                {
                                    //txtDate.Text = bC.FormatDate(txtDate.Text, Session["OutputDateFormat"].ToString(), "dd/MM/yyyy");

                                    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                    coll.Add("S");
                                    // coll.Add(Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy"));
                                    /*****************************/

                                    coll.Add(common.myStr(txtDate.Text).Trim());

                                    //DateTime d = tpTime.SelectedDate.Value;
                                    //string time = d.ToString("HH:mm:ss");


                                    //coll.Add(common.myStr(txtDate.Text).Trim() +" "+ common.myStr(time) );
                                    /*****************************/
                                    coll.Add("0");
                                    coll.Add(RowCaptionId);

                                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                    {
                                        coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                        coll.Add(common.myInt(ViewState["PageId"]));
                                    }

                                    strNonTabular.Append(common.setXmlTable(ref coll));
                                }
                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("ST"))
                            {
                                TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                                RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                                RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                                //if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                                //{
                                if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
                                {
                                    //txtDate.Text = bC.FormatDate(txtDate.Text, Session["OutputDateFormat"].ToString(), "dd/MM/yyyy");

                                    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                    coll.Add("S");
                                    // coll.Add(Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy"));
                                    /*****************************/

                                    //coll.Add(common.myStr(txtDate.Text).Trim());

                                    DateTime d = tpTime.SelectedDate.Value;
                                    string time = d.ToString("HH:mm:ss");


                                    coll.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));
                                    // coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));
                                    /*****************************/
                                    coll.Add("0");
                                    coll.Add(RowCaptionId);

                                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                    {
                                        coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                        coll.Add(common.myInt(ViewState["PageId"]));
                                    }

                                    strNonTabular.Append(common.setXmlTable(ref coll));
                                }
                                //else if (ViewState["tpTimeSelectedDate"] != null && !common.myStr(ViewState["tpTimeSelectedDate"]).Equals(string.Empty))
                                //{
                                //    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                //    coll.Add("S");

                                //    DateTime d = Convert.ToDateTime(ViewState["tpTimeSelectedDate"]);
                                //    string time = d.ToString("HH:mm:ss");

                                //    coll.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));
                                //    // coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));
                                //    /*****************************/
                                //    coll.Add("0");
                                //    coll.Add(RowCaptionId);

                                //    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                //    {
                                //        coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                //        coll.Add(common.myInt(ViewState["PageId"]));
                                //    }

                                //    strNonTabular.Append(common.setXmlTable(ref coll));
                                //  }

                            }
                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim().Equals("SB"))
                            {
                                TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                                RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                                RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                                //if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                                //{
                                if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
                                {
                                    //txtDate.Text = bC.FormatDate(txtDate.Text, Session["OutputDateFormat"].ToString(), "dd/MM/yyyy");

                                    coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                    coll.Add("S");
                                    // coll.Add(Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy"));
                                    /*****************************/

                                    //coll.Add(C);

                                    DateTime d = tpTime.SelectedDate.Value;
                                    string time = d.ToString("HH:mm:ss");


                                    coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));
                                    // coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));
                                    /*****************************/
                                    coll.Add("0");
                                    coll.Add(RowCaptionId);

                                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                    {
                                        coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                        coll.Add(common.myInt(ViewState["PageId"]));
                                    }

                                    strNonTabular.Append(common.setXmlTable(ref coll));
                                }
                            }

                            else if (common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Equals("H"))
                            {
                                coll.Add(item2.Cells[(byte)enumNonT.FieldId].Text);
                                coll.Add("H");
                                coll.Add("-");
                                coll.Add("0");
                                coll.Add(RowCaptionId);

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    coll.Add(common.myInt(ddlTemplateRequiredServices.SelectedValue));
                                    coll.Add(common.myInt(ViewState["PageId"]));
                                }

                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }

                        }
                    }
                    if (ViewState["ResultSet"] == null)
                    {
                        ViewState["ResultSet"] = strNonTabular;
                    }
                    else
                    {
                        ViewState["ResultSet"] = common.myStr(ViewState["ResultSet"]) + strNonTabular;
                    }
                }

                #endregion
            }

            hdnIsUnSavedData.Value = "0";

            if (common.myStr(ViewState["TagType"]).Equals("D"))
            {
                SubDeptId = common.myInt(ddlTemplateRequiredServices.SelectedValue);
                //hstInput.Add("@intSubDeptId", common.myInt(ddlTemplateRequiredServices.SelectedValue));
                if (common.myStr(Request.QueryString["RList"]).Equals("RL"))// when page comming from request list 
                {
                    OrderRequestId = common.myInt(Request.QueryString["RequestId"]);
                    //hstInput.Add("@intOrderRequestId", common.myStr(Request.QueryString["RequestId"]));// Sub- department Id                   
                }
            }
            else
            {
                SubDeptId = 0;
            }

            if (common.myStr(Request.QueryString["SourceForLIS"]).Trim().Equals("LIS")
                && common.myStr(ViewState["CF"]).Equals("LAB")
                && common.myInt(Request.QueryString["sOrdDtlId"]) > 0)
            {
                OrderDetailId = common.myInt(Request.QueryString["sOrdDtlId"]);
            }

            IsAddendum = objEMR.IsSectionAddendum(common.myInt(SectionId));

            if (IsAddendum)
            {
                if (!common.myBool(Request.QueryString["IsAddendum"]))
                {
                    return;
                }
            }
            else
            {
                if (common.myBool(Request.QueryString["IsAddendum"]))
                {
                    return;
                }
            }

            int intProviderId = common.myInt(ddlProvider.SelectedValue);
            string Hourtime = string.Empty;
            if (RadTimeFrom.SelectedDate != null)
                Hourtime = RadTimeFrom.SelectedDate.Value.TimeOfDay.ToString();
            DateTime? dtChangeDate = null;
            if (dtpChangeDate.SelectedDate != null)
            {
                dtChangeDate = Convert.ToDateTime(Convert.ToDateTime(dtpChangeDate.SelectedDate.ToString()).ToString("yyyy/MM/dd HH:mm"));
                if (!string.IsNullOrEmpty(common.myStr(dtChangeDate)) && !string.IsNullOrEmpty(Hourtime))
                    dtChangeDate = Convert.ToDateTime(common.myStr((common.myStr(dtChangeDate).Split(' '))[0]) + " " + Hourtime);
            }

            if (tblProviderDetails.Visible)
            {
                if (ddlProvider.SelectedIndex < 1)
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

            if (!strNonTabular.ToString().Equals(string.Empty) || !strTabular.ToString().Equals(string.Empty))
            {
                string strMsg = objEMR.SavePatientNotesData(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), 1,
                                            common.myBool(chkPullForward.Checked) ? 1 : 0, common.myInt(ViewState["PageId"]),
                                            common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myInt(Request.QueryString["tmpOrder"]),
                                            common.myInt(ViewState["TemplateTypeID"]), strNonTabular.ToString(), strTabular.ToString(), 1,
                                            SectionId, common.myInt(ddlRecord.SelectedValue), common.myInt(ddlEpisode.SelectedValue),
                                            common.myInt(Session["UserID"]), OrderDetailId, OrderRequestId, SubDeptId, intProviderId, dtChangeDate, common.myBool(chkApprovalRequired.Checked), common.myInt(ddlAdvisingDoctor.SelectedValue));

                //hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientTemplates", hstInput, hstOutput);
                if (strMsg.ToUpper().Contains("SAVED") && common.myInt(ViewState["ServiceId"]) > 0 && common.myInt(Request.QueryString["SOD"]) > 0)
                {
                    objEMR.updateFurtherAckServiceOrderDetail(common.myInt(Request.QueryString["SOD"]), common.myInt(ViewState["ServiceId"]));
                    dtpChangeDate.SelectedDate = null;
                    ddlProvider.SelectedValue = null;
                }
                if (isSaveManual)
                {
                    AddRow = false;
                    setTabularHeader();

                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                        && !common.myStr(ViewState["TagType"]).Equals("D"))
                    {
                        setServiceChkVisiblity();
                    }

                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strMsg;

                }

                if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
                {
                    if (isSaveManual)
                    {
                        if (common.myStr(Request.QueryString["MASTER"]).ToUpper().Equals("NO")
                            || common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP")
                            || common.myStr(Request.QueryString["Source"]).ToUpper().Equals("PROCEDUREORDER")
                            || common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                        {
                            string strPopup = "<script language='javascript' ID='script1'>" + "window.close();" + "</script>";

                            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
                            return;
                        }
                    }

                    //if (common.myStr(Request.QueryString["From"]).Equals("POPUP")
                    //        && !common.myStr(Request.QueryString["SingleScreenTemplateCode"]).Equals(string.Empty))
                    //{
                    //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "returnToParent();", true);
                    //    return;
                    //}
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
            dsTemplateDataDetails.Dispose();
        }

    }


    private void AddNodes(TreeView tvName, string iNodeID, int iParentID, string sNodeText)
    {
        try
        {
            if (iParentID.Equals(0))
            {
                TreeNode masternode = new TreeNode(common.myStr(sNodeText), "P" + common.myStr(iNodeID));
                tvName.Nodes.Add(masternode);
                tvName.CollapseAll();
            }
            else
            {
                TreeNode masternode = new TreeNode();
                masternode = tvName.FindNode("P" + iParentID.ToString());
                if (masternode != null)
                {
                    TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString());
                    masternode.ChildNodes.Add(childNode);
                }
                else
                {
                    CallRecursive(tvName, iNodeID, "C" + iParentID, sNodeText);
                }
                tvName.CollapseAll();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void CallRecursive(TreeView tvName, String iNodeID, String sParentID, String sNodeText)
    {
        TreeNodeCollection nodes = tvName.Nodes;
        foreach (TreeNode n in nodes)
        {
            ReCallRecursive(n, iNodeID, sParentID, sNodeText);
        }
    }

    private void ReCallRecursive(TreeNode treeNode, String iNodeID, String sParentID, String sNodeText)
    {
        foreach (TreeNode tn in treeNode.ChildNodes)
        {
            if (common.myStr(tn.Value).Equals(common.myStr(sParentID)))
            {
                TreeNode childNode = new TreeNode(sNodeText.ToString(), "C" + iNodeID.ToString());
                tn.ChildNodes.Add(childNode);
            }
            ReCallRecursive(tn, iNodeID, sParentID, sNodeText);
        }
    }

    public DataTable BindFieldFormats(string strFieldId)
    {
        DataTable dt = new DataTable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hs = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hs.Add("@intFieldId", common.myInt(strFieldId));

            //ds = objDl.FillDataSet(CommandType.Text, "SELECT FormatId, FormatName FROM EMRTemplateFieldFormats WITH (NOLOCK) WHERE FieldId=@intFieldId AND Active=1 ORDER BY FormatName", hs);
            ds = objDl.FillDataSet(CommandType.Text, "SELECT DISTINCT FormatId, FormatName, LTRIM(RTRIM(CONVERT(VARCHAR(MAX), FormatText))) AS FormatText FROM EMRTemplateFieldFormats WITH (NOLOCK) WHERE FieldId=@intFieldId AND Active=1 ORDER BY FormatName", hs);

            dt = ds.Tables[0];
            DataRow dr;
            dr = dt.NewRow();
            dr["FormatId"] = 0;
            dr["FormatName"] = "--Options--";
            dt.Rows.InsertAt(dr, 0);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dt;
    }
    private void BindPrintReport()
    {
        try
        {
            //btnPrintReport.Visible = false;
            hdnReportContent.Value = string.Empty;

            if (common.myInt(ddlReport.SelectedValue) > 0)
            {
                hdnReportContent.Value = BindEditor(true);
                StringBuilder sbD = new StringBuilder();
                sbD.Append(" <table border='0' width='100%' style='border-collapse:collapse;' cellpadding='5' cellspacing='5' >");
                sbD.Append("<tr><td>Follow Up : </td></tr>");

                string SignatureLabel = common.myStr(ddlReport.SelectedItem.Attributes["SignatureLabel"]).Trim();

                if (SignatureLabel.Equals(string.Empty))
                {
                    sbD.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                }
                else
                {
                    sbD.Append("<tr><td align='right'><b>" + SignatureLabel + "</b></td></tr>");
                }
                sbD.Append("</table>");

                hdnReportContent.Value = hdnReportContent.Value + sbD.ToString();
            }
            else
            {
                Alert.ShowAjaxMsg("please select report", Page);
            }
            // btnPrintReport.Visible = true;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnPrintReport_Click(object sender, EventArgs e)
    {
        Session["PrintTemplateReport"] = null;
        BindPrintReport();
        if (common.myStr(hdnReportContent.Value) != "")
        {
            Session["PrintTemplateReport"] = hdnReportContent.Value;
            RadWindowPrint.NavigateUrl = "/EMR/Templates/PrintTemplateReport.aspx";
            RadWindowPrint.Height = 410;
            RadWindowPrint.Width = 610;
            RadWindowPrint.Top = 10;
            RadWindowPrint.Left = 10;
            RadWindowPrint.OnClientClose = string.Empty;
            RadWindowPrint.VisibleOnPageLoad = true;
            RadWindowPrint.Modal = true;
            RadWindowPrint.VisibleStatusbar = false;
            RadWindowPrint.InitialBehavior = WindowBehaviors.Maximize;
        }
    }


    protected void btnPrintReport2_Click(object sender, EventArgs e)
    {
        if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
        {
            //hdnReportContent.Value = PrintReport(false);
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "PrintSetupContent();", true);
            //return;

            Session["EMRTemplatePrintData"] = PrintReport(false, 0);

            if (common.myLen(Session["EMRTemplatePrintData"]) > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                sb.Append("<td align=center><B>" + common.myStr(ddlTemplateMain.SelectedItem.Text) + "</B></td>");
                sb.Append("</tr></table>");

                sb.Append(Session["EMRTemplatePrintData"]);

                Session["EMRTemplatePrintData"] = sb.ToString();
            }

            //RadWindowPrint.NavigateUrl = "/EMR/Templates/PrintPdf1.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ddlReportFormat.SelectedValue) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value);
            RadWindowPrint.NavigateUrl = "/EMR/Templates/PrintPdf1.aspx?PrintHeader=" + common.myBool(chkPrintHospitalHeader.Checked);
            RadWindowPrint.Height = 598;
            RadWindowPrint.Width = 900;
            RadWindowPrint.Top = 10;
            RadWindowPrint.Left = 10;
            RadWindowPrint.Modal = true;
            RadWindowPrint.OnClientClose = string.Empty; //"OnClientClose";
            RadWindowPrint.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

            RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindowPrint.VisibleStatusbar = false;
        }
    }

    private StringBuilder getReportHeader(int ReportId)
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();

        bool IsPrintHospitalHeader = false;
        clsIVF objivf = new clsIVF(sConString);
        ds = objivf.EditReportName(ReportId);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
            }
        }

        ds = new DataSet();
        ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

        sb.Append("<div>");

        if (IsPrintHospitalHeader)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                sb.Append("<table border='0' width='100%' cellpadding='0' cellspacing='2' style='font-size:smaller'>");
                for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                {
                    DataRow DR = ds.Tables[0].Rows[idx];

                    sb.Append("<tr>");

                    sb.Append("<td align ='center'>");
                    sb.Append("<table border='0' cellpadding='0' cellspacing='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td align ='right' valign='middle'><img src='../../Icons/SmallLogo.jpg' border='0' width='100px' height='25px' align='middle' alt='Image'/></td>");
                    sb.Append("<td align ='left' valign='middle'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");

                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td align ='center'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                    sb.Append("</tr>");

                    //sb.Append("<tr>");
                    //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
                    //sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td align ='center'>Phone : " + common.myStr(DR["Phone"]) + " Fax : " + common.myStr(DR["Fax"]) + "</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
            }
        }
        else
        {
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<br />");
        }

        sb.Append("<br />");

        sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
        //sb.Append("<td align=center><U>" + common.myStr(ddlReport.SelectedItem.Text) + "</U></td>");
        sb.Append("<td align=center><U>" + common.myStr(ddlTemplateMain.SelectedItem.Text) + "</U></td>");
        sb.Append("</tr></table></div>");

        return sb;
    }

    private string PrintReport(bool sign, int SectionidFilter)
    {
        string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
        Session["DisplayEnteredByInCaseSheet"] = string.Empty;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();

        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        //sb.Append(getReportHeader(3028).ToString());//ReportId

        //clsIVF objIVF = new clsIVF(sConString);

        //string strPatientHeader = objIVF.getCustomizedPatientReportHeader(21); //common.myInt(ddlReport.SelectedItem.Attributes["HeaderId"])
        //if (common.myLen(strPatientHeader).Equals(0))
        //{
        //    sb.Append(getIVFPatient().ToString());
        //}
        //else
        //{
        //    sb.Append(strPatientHeader);
        //}

        //string sTemplateName = common.myStr(ddlTemplatePatient.SelectedItem.Text) == "ALL" ? "" : common.myStr(ddlTemplatePatient.SelectedItem.Text);

        DataSet dsTemplateData = new DataSet();
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
        try
        {
            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            if (ddlTemplateMain.SelectedValue != "")
            {
                string sTemplateName = common.myStr(ddlTemplateMain.SelectedItem.Text) == "ALL" ? "" : common.myStr(ddlTemplateMain.SelectedItem.Text);

                if (chkIncludePastHistory.Checked)
                {
                    dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                       common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myDate(dtpfromDate.SelectedDate).ToString("yyyy-MM-dd"),
                       common.myDate(dtpToDate.SelectedDate).ToString("yyyy-MM-dd"), "", common.myInt(ddlTemplateMain.SelectedValue),
                       "D", false, 0, SectionidFilter, common.myBool(chkIncludePastHistory.Checked));
                }
                else
                {

                    dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
                        common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), "1900-01-01",
                        "2059-01-01", "", common.myInt(ddlTemplateMain.SelectedValue),
                        "D", false, 0, SectionidFilter, common.myBool(chkIncludePastHistory.Checked));
                }

            }
            else
            {
                Alert.ShowAjaxMsg("Please select template !", this.Page);
                return string.Empty;
            }

            dvDataFilter = new DataView(dsTemplateData.Tables[21]);
            dtEncounter = dsTemplateData.Tables[22];
            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    #region Template Wise
                    {
                        dtTemplate = dvDataFilter.ToTable();
                        TemplateString = new StringBuilder();
                        for (int i = 0; i < dtTemplate.Rows.Count; i++)
                        {
                            #region Template History Type
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                               && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                               && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call History Type Dynamic Template
                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindDataNew(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                        // TemplateString.Append(sbTemp);
                                        TemplateString.Append(sbTemp + "<br/>");
                                }

                                sbTemp = null;
                                dsDymanicTemplateData.Dispose();
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion

                            #region All the Templates except Hitory and Plan of case
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindDataNew(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                    {
                                        TemplateString.Append(sbTemp);
                                        //TemplateString.Append(sbTemp + "<br/>");
                                    }
                                }
                                sbTemp = null;
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion

                            #region Template Plan of Care
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                 && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call Dynamic Template Plan of Care
                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }

                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindDataNew(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                        TemplateString.Append(sbTemp + "<br/>");
                                }
                                sbTemp = null;

                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                            #endregion

                        }
                        if (TemplateString.Length > 30)
                        {
                            //if (iEn == 0)
                            //{
                            //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
                            //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
                            //sb.Append("</span>");
                            //}
                            sb.Append("<span style='" + String.Empty + "'>");
                            sb.Append(TemplateString);
                            sb.Append("</span><br/>");
                            TemplateString = null;
                        }
                    }
                    #endregion
                }
            }
            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                sb.Append(hdnDoctorImage.Value);
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;

            sbTemplateStyle = null;
            TemplateString = null;
            ds.Dispose();
            dsTemplateStyle.Dispose();
            dvDataFilter.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            Templinespace = "";
            bnotes = null;
            fun = null;
            emr = null;
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
        }
        return sb.ToString();
    }

    protected void bindDataNew(DataSet dsDynamicTemplateData, string TemplateId, StringBuilder sb, string GroupingDate)
    {
        DataSet ds = new DataSet();
        DataSet dsAllNonTabularSectionDetails = new DataSet();
        DataSet dsAllTabularSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();

        DataTable dtSectionLists = new DataTable();
        DataTable dtFieldValue = new DataTable();
        DataTable dtEntry = new DataTable();
        DataTable dtFieldName = new DataTable();

        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dv2 = new DataView();

        DataRow dr3;

        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        StringBuilder str = new StringBuilder();
        string sEntryType = "V";
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string BeginList2 = string.Empty;
        string BeginList3 = string.Empty;
        string EndList3 = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;

        int t = 0;
        int t2 = 0;
        int t3 = 0;
        int iRecordId = 0;
        DataView dvDyTable1 = new DataView();
        try
        {
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;

            t = 0;
            t2 = 0;
            t3 = 0;

            dtSectionLists = dsDynamicTemplateData.Tables[0].Copy();

            dvDyTable1 = new DataView(dsDynamicTemplateData.Tables[0]);
            DataView dvDyTable2 = new DataView(dsDynamicTemplateData.Tables[1]);
            DataView dvDyTable3 = new DataView(dsDynamicTemplateData.Tables[2]);

            dvDyTable1.ToTable().TableName = "TemplateSectionName";
            dvDyTable2.ToTable().TableName = "FieldName";
            dvDyTable3.ToTable().TableName = "PatientValue";
            dsAllNonTabularSectionDetails = new DataSet();
            if (dvDyTable3.ToTable().Rows.Count > 0)
            {
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable2.ToTable());
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable3.ToTable());
            }
            dvDyTable2.Dispose();
            dvDyTable3.Dispose();

            dsDynamicTemplateData.Dispose();





            DataView dvDyTable4 = new DataView(dsDynamicTemplateData.Tables[3]);
            DataView dvDyTable5 = new DataView(dsDynamicTemplateData.Tables[4]);
            DataView dvDyTable6 = new DataView(dsDynamicTemplateData.Tables[5]);

            dvDyTable4.ToTable().TableName = "TabularData";
            dvDyTable5.ToTable().TableName = "TabularColumnCount";
            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";

            dsAllTabularSectionDetails = new DataSet();
            if (dvDyTable4.ToTable().Rows.Count > 0)
            {
                dsAllTabularSectionDetails.Tables.Add(dvDyTable4.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable5.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable6.ToTable());
            }

            dvDyTable4.Dispose();
            dvDyTable5.Dispose();


            if (dtSectionLists != null)
            {
                if (dtSectionLists.Rows.Count > 0)
                {
                    ViewState["iPrevId"] = 0;

                    foreach (DataRow itemSections in dtSectionLists.Rows)
                    {
                        #region Non Tabular
                        if (dsAllNonTabularSectionDetails.Tables.Count > 0 && dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
                        {
                            DataView dvNonTabular = new DataView(dvDyTable1.ToTable());
                            dvNonTabular.RowFilter = "ISNULL(Tabular,0)=0 AND SectionId=" + common.myInt(itemSections["SectionId"]);

                            if (dvNonTabular.ToTable().Rows.Count > 0)
                            {
                                ds = new DataSet();
                                ds.Tables.Add(dvNonTabular.ToTable());//Section Name Table

                                dv = new DataView(dsAllNonTabularSectionDetails.Tables[1]);

                                dv.Sort = "RecordId DESC";
                                dtEntry = dv.ToTable(true, "RecordId");
                                iRecordId = 0;
                                dv.Dispose();
                                dvNonTabular.Dispose();

                                for (int it = 0; it < dtEntry.Rows.Count; it++)
                                {
                                    if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                                    {
                                        foreach (DataRow item in ds.Tables[0].Rows)
                                        {
                                            dv1 = new DataView(dsAllNonTabularSectionDetails.Tables[0]);
                                            dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                            dtFieldName = dv1.ToTable();

                                            if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                            {
                                                dv2 = new DataView(dsAllNonTabularSectionDetails.Tables[1]);
                                                dv2.RowFilter = "RecordId=" + common.myInt(dtEntry.Rows[it]["RecordId"]) + " AND SectionId=" + common.myInt(item["SectionId"]);
                                                dtFieldValue = dv2.ToTable();
                                                dv2.Dispose();
                                            }

                                            dsAllFieldsDetails = new DataSet();
                                            dsAllFieldsDetails.Tables.Add(dtFieldName);
                                            dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                            dtFieldName.Dispose();
                                            dtFieldValue.Dispose();
                                            dv1.Dispose();

                                            if (dsAllNonTabularSectionDetails.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                                {
                                                    if (dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
                                                    {
                                                        sBegin = string.Empty;
                                                        sEnd = string.Empty;
                                                        dr3 = dsAllNonTabularSectionDetails.Tables[0].Rows[0];
                                                        getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                                        ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                                        str = new StringBuilder();

                                                        str.Append(CreateStringNew(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                                    common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                                    common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                                        //str.Append("<br/> ");

                                                        dr3 = null;
                                                        dsAllNonTabularSectionDetails.Dispose();
                                                        dsAllFieldsDetails.Dispose();
                                                        string sBreak = common.myBool(item["IsConfidential"]) ? "<br/>" : "";
                                                        if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                                        {
                                                            if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                            {
                                                                if (sEntryType.Equals("M"))
                                                                {
                                                                    objStrTmp.Append("<br/>");
                                                                }
                                                            }
                                                            if (t2.Equals(0))
                                                            {
                                                                if (t3.Equals(0))//Template
                                                                {
                                                                    t3 = 1;
                                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                                    {
                                                                        BeginList3 = "<ul>";
                                                                        EndList3 = "</ul>";
                                                                    }
                                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                                    {
                                                                        BeginList3 = "<ol>";
                                                                        EndList3 = "</ol>";
                                                                    }
                                                                }
                                                            }

                                                            if (common.myStr(item["SectionsBold"]) != string.Empty
                                                                || common.myStr(item["SectionsItalic"]) != string.Empty
                                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                            {
                                                                sBegin = string.Empty;
                                                                sEnd = string.Empty;
                                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty))
                                                                    {
                                                                        objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                                    }
                                                                }
                                                                BeginList3 = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty))
                                                                    {
                                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                                    }
                                                                }
                                                            }

                                                            if (!str.ToString().Trim().Equals(string.Empty))
                                                            {
                                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                                {
                                                                    ////// objStrTmp.Append("<br />"); //code commented  for Examination (SectonName and fieldname getting extra space)
                                                                }
                                                                objStrTmp.Append(str.ToString());
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (t.Equals(0))
                                                            {
                                                                t = 1;
                                                                if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                                {
                                                                    BeginList = "<ul>"; EndList = "</ul>";
                                                                }
                                                                else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                                {
                                                                    BeginList = "<ol>"; EndList = "</ol>";
                                                                }
                                                            }
                                                            if (common.myStr(item["TemplateBold"]) != string.Empty
                                                                || common.myStr(item["TemplateItalic"]) != string.Empty
                                                                || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                                || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                                || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                                || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                            {
                                                                sBegin = string.Empty;
                                                                sEnd = string.Empty;
                                                                MakeFont("Template", ref sBegin, ref sEnd, item);
                                                                if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                                {
                                                                    if (sBegin.Contains("<br/>"))
                                                                    {
                                                                        sBegin = sBegin.Remove(0, 5);
                                                                        objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    }
                                                                    else
                                                                    {
                                                                        objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    }
                                                                }
                                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                                {
                                                                    objStrTmp.Append("<br/>");
                                                                }
                                                                BeginList = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (common.myBool(item["TemplateDisplayTitle"]))
                                                                {
                                                                    objStrTmp.Append(sBreak + common.myStr(item["TemplateName"]));//Default Setting
                                                                }
                                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                                {
                                                                    objStrTmp.Append("<br/>");
                                                                }
                                                            }
                                                            if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                                || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                            {
                                                                //objStrTmp.Append("<br />");
                                                            }

                                                            objStrTmp.Append(EndList);
                                                            if (t2.Equals(0))
                                                            {
                                                                t2 = 1;
                                                                if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                                {
                                                                    BeginList2 = "<ul>";
                                                                    EndList3 = "</ul>";
                                                                }
                                                                else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                                {
                                                                    BeginList2 = "<ol>";
                                                                    EndList3 = "</ol>";
                                                                }
                                                            }
                                                            if (common.myStr(item["SectionsBold"]) != string.Empty
                                                                || common.myStr(item["SectionsItalic"]) != string.Empty
                                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                            {
                                                                sBegin = string.Empty;
                                                                sEnd = string.Empty;
                                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                                    {

                                                                        if (sBegin.StartsWith("<br/>"))
                                                                        {
                                                                            if (sBegin.Length > 5)
                                                                            {

                                                                                //sBegin = sBegin.Remove(0, 5);
                                                                                //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                                sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);

                                                                        }

                                                                        //if (sBegin.Contains("<br/>"))
                                                                        //{
                                                                        //    sBegin = sBegin.Remove(0, 5);
                                                                        //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                        //}
                                                                        //else
                                                                        //{

                                                                        //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                        //}

                                                                    }
                                                                }
                                                                BeginList2 = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                                    {
                                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                                    }
                                                                }
                                                            }
                                                            if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                                || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                            {
                                                                //objStrTmp.Append("<br />");
                                                            }

                                                            objStrTmp.Append(str.ToString());
                                                        }
                                                        //if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        //{
                                                        iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                                        ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                                        // }
                                                    }
                                                    str = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Tabular
                        if (dsAllTabularSectionDetails.Tables.Count > 0 && dsAllTabularSectionDetails.Tables[1].Rows.Count > 0)
                        {
                            DataView dvTabular = new DataView(dvDyTable1.ToTable());
                            dvTabular.RowFilter = "ISNULL(Tabular,0)=1 AND SectionId=" + common.myInt(itemSections["SectionId"]);
                            if (dvTabular.ToTable().Rows.Count > 0)
                            {
                                ds = new DataSet();
                                ds.Tables.Add(dvTabular.ToTable());//Section Name Table
                                dv = new DataView(dsAllTabularSectionDetails.Tables[0]);
                                dv.Sort = "RecordId DESC";
                                dtEntry = dv.ToTable(true, "RecordId");
                                iRecordId = 0;
                                dv.Dispose();
                                dvTabular.Dispose();
                                for (int it = 0; it < dtEntry.Rows.Count; it++)
                                {
                                    if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                                    {
                                        foreach (DataRow item in ds.Tables[0].Rows)
                                        {
                                            dv1 = new DataView(dsAllTabularSectionDetails.Tables[0]);
                                            dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                            DataView dvFieldStyle = new DataView(dsAllTabularSectionDetails.Tables[2]);
                                            dvFieldStyle.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                            dtFieldName = dv1.ToTable();

                                            if (dsAllTabularSectionDetails.Tables.Count > 1)
                                            {
                                                dv2 = new DataView(dsAllTabularSectionDetails.Tables[1]);
                                                dv2.RowFilter = " SectionId=" + common.myStr(item["SectionId"]);
                                                dtFieldValue = dv2.ToTable();
                                                dv2.Dispose();
                                            }

                                            dsAllFieldsDetails = new DataSet();
                                            dsAllFieldsDetails.Tables.Add(dtFieldName);
                                            dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                            dsAllFieldsDetails.Tables.Add(dvDyTable6.ToTable());
                                            dvDyTable6.Dispose();
                                            dtFieldName.Dispose();
                                            dtFieldValue.Dispose();
                                            dv1.Dispose();

                                            if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsAllTabularSectionDetails.Tables.Count > 1)
                                                {
                                                    if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                                    {
                                                        sBegin = string.Empty;
                                                        sEnd = string.Empty;
                                                        dr3 = dvFieldStyle.ToTable().Rows[0];
                                                        getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                                        ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                                        str = new StringBuilder();
                                                        str.Append(CreateStringNew(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                                    common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                                    common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                                        str.Append("<br/> ");

                                                        dr3 = null;
                                                        dsAllTabularSectionDetails.Dispose();
                                                        dsAllFieldsDetails.Dispose();

                                                        if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                                        {
                                                            if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                            {
                                                                if (sEntryType.Equals("M"))
                                                                {
                                                                    objStrTmp.Append("<br/>");
                                                                }
                                                            }
                                                            if (t2.Equals(0))
                                                            {
                                                                if (t3.Equals(0))//Template
                                                                {
                                                                    t3 = 1;
                                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                                    {
                                                                        BeginList3 = "<ul>";
                                                                        EndList3 = "</ul>";
                                                                    }
                                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                                    {
                                                                        BeginList3 = "<ol>";
                                                                        EndList3 = "</ol>";
                                                                    }
                                                                }
                                                            }

                                                            if (common.myStr(item["SectionsBold"]) != string.Empty
                                                                || common.myStr(item["SectionsItalic"]) != string.Empty
                                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                            {
                                                                sBegin = string.Empty;
                                                                sEnd = string.Empty;
                                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty))
                                                                    {
                                                                        objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                                    }
                                                                }
                                                                BeginList3 = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty))
                                                                    {
                                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                                    }
                                                                }
                                                            }

                                                            if (!str.ToString().Trim().Equals(string.Empty))
                                                            {
                                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                                {
                                                                    objStrTmp.Append("<br />");
                                                                }
                                                                objStrTmp.Append(str.ToString());
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (t.Equals(0))
                                                            {
                                                                t = 1;
                                                                if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                                {
                                                                    BeginList = "<ul>"; EndList = "</ul>";
                                                                }
                                                                else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                                {
                                                                    BeginList = "<ol>"; EndList = "</ol>";
                                                                }
                                                            }
                                                            if (common.myStr(item["TemplateBold"]) != string.Empty
                                                                || common.myStr(item["TemplateItalic"]) != string.Empty
                                                                || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                                || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                                || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                                || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                            {
                                                                sBegin = string.Empty;
                                                                sEnd = string.Empty;
                                                                MakeFont("Template", ref sBegin, ref sEnd, item);
                                                                if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty))
                                                                    {
                                                                        if (sBegin.Contains("<br/>"))
                                                                        {
                                                                            sBegin = sBegin.Remove(0, 5);
                                                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                        }
                                                                        else
                                                                        {
                                                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                        }
                                                                    }
                                                                }
                                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                                {
                                                                    objStrTmp.Append("<br/>");
                                                                }
                                                                BeginList = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (common.myBool(item["TemplateDisplayTitle"]))
                                                                {
                                                                    objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                                                }
                                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                                {
                                                                    objStrTmp.Append("<br/>");
                                                                }
                                                            }
                                                            if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                                || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                            {
                                                                //objStrTmp.Append("<br />");
                                                            }

                                                            objStrTmp.Append(EndList);
                                                            if (t2.Equals(0))
                                                            {
                                                                t2 = 1;
                                                                if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                                {
                                                                    BeginList2 = "<ul>";
                                                                    EndList3 = "</ul>";
                                                                }
                                                                else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                                {
                                                                    BeginList2 = "<ol>";
                                                                    EndList3 = "</ol>";
                                                                }
                                                            }
                                                            if (common.myStr(item["SectionsBold"]) != string.Empty
                                                                || common.myStr(item["SectionsItalic"]) != string.Empty
                                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                            {
                                                                sBegin = string.Empty;
                                                                sEnd = string.Empty;
                                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                                    {
                                                                        objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                    }
                                                                }
                                                                BeginList2 = string.Empty;
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                                {
                                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                                    {
                                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                                    }
                                                                }
                                                            }
                                                            if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                                || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                            {
                                                                //objStrTmp.Append("<br />");
                                                            }

                                                            objStrTmp.Append(str.ToString());
                                                        }
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                                            ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                                        }
                                                    }
                                                    str = null;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            if (t2.Equals(1) && t3.Equals(1))
            {
                objStrTmp.Append(EndList3);
            }
            else
            {
                objStrTmp.Append(EndList);
            }
            if (GetPageProperty("1") != null)
            {
                objStrSettings.Append(objStrTmp.ToString());
                sb.Append(objStrSettings.ToString());
            }
            else
            {
                sb.Append(objStrTmp.ToString());
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            dsAllNonTabularSectionDetails.Dispose();
            dsAllTabularSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();

            dtFieldValue.Dispose();
            dtEntry.Dispose();
            dtFieldName.Dispose();
            dvDyTable1.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dv2.Dispose();
            dtSectionLists.Dispose();

            dr3 = null;

            objStrTmp = null;
            objStrSettings = null;

            sEntryType = string.Empty;
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;
            sBegin = string.Empty;
            sEnd = string.Empty;
        }
    }


    //private string PrintReport(bool sign)
    //{
    //    string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
    //    Session["DisplayEnteredByInCaseSheet"] = string.Empty;

    //    StringBuilder sb = new StringBuilder();
    //    StringBuilder sbTemplateStyle = new StringBuilder();
    //    StringBuilder TemplateString;
    //    DataSet ds = new DataSet();
    //    DataSet dsTemplateStyle = new DataSet();
    //    DataRow drTemplateStyle = null;
    //    DataTable dtTemplate = new DataTable();
    //    DataView dvDataFilter = new DataView();
    //    DataTable dtEncounter = new DataTable();

    //    string Templinespace = "";
    //    BindNotes bnotes = new BindNotes(sConString);
    //    BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
    //    BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
    //    StringBuilder sbTemp = new StringBuilder();
    //    bool bAllergyDisplay = false;
    //    bool bPatientBookingDisplay = false;

    //    sb.Append(getReportHeader(3028).ToString());//ReportId

    //    clsIVF objIVF = new clsIVF(sConString);

    //    string strPatientHeader = objIVF.getCustomizedPatientReportHeader(21); //common.myInt(ddlReport.SelectedItem.Attributes["HeaderId"])
    //    if (common.myLen(strPatientHeader).Equals(0))
    //    {
    //        sb.Append(getIVFPatient().ToString());
    //    }
    //    else
    //    {
    //        sb.Append(strPatientHeader);
    //    }

    //    //string sTemplateName = common.myStr(ddlTemplatePatient.SelectedItem.Text) == "ALL" ? "" : common.myStr(ddlTemplatePatient.SelectedItem.Text);

    //    DataSet dsTemplateData = new DataSet();        
    //    BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);
    //    try
    //    {
    //        string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
    //        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

    //        if (ddlTemplateMain.SelectedValue != "")
    //        {
    //            string sTemplateName = common.myStr(ddlTemplateMain.SelectedItem.Text) == "ALL" ? "" : common.myStr(ddlTemplateMain.SelectedItem.Text);

    //            dsTemplateData = emr.getEMRPrintCaseSheetDate(common.myInt(Session["HospitalLocationId"]),
    //                common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), "1900-01-01",
    //                "2059-01-01", "", common.myInt(ddlTemplateMain.SelectedValue),
    //                "D", false, 0);
    //        }
    //        else
    //        {
    //            Alert.ShowAjaxMsg("Please select template !", this.Page);
    //            return string.Empty;
    //        }

    //        dvDataFilter = new DataView(dsTemplateData.Tables[21]);
    //        dtEncounter = dsTemplateData.Tables[22];
    //        for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
    //        {
    //            if (dvDataFilter.ToTable().Rows.Count > 0)
    //            {
    //                #region Template Wise
    //                {
    //                    dtTemplate = dvDataFilter.ToTable();
    //                    TemplateString = new StringBuilder();
    //                    for (int i = 0; i < dtTemplate.Rows.Count; i++)
    //                    {
    //                        #region Template History Type
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                           && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
    //                           && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
    //                        {
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            dv.Dispose();
    //                            sbTemp = new StringBuilder();
    //                            //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                            //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                            //{
    //                            #region Assign Data and call History Type Dynamic Template
    //                            DataSet dsDymanicTemplateData = new DataSet();

    //                            DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
    //                            DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
    //                            DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
    //                            DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
    //                            DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
    //                            DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
    //                            DataTable dtDyTempTable = new DataTable();

    //                            dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //                            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
    //                            if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            else
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            string sSectionId = "0";
    //                            for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
    //                            {
    //                                sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
    //                                    : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
    //                            }
    //                            dvDyTable2.ToTable().TableName = "FieldName";
    //                            dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
    //                            dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

    //                            dvDyTable3.ToTable().TableName = "PatientValue";
    //                            if (dvDyTable3.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }
    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }

    //                            dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dtDyTempTable);
    //                            }
    //                            else
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
    //                            }
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
    //                            if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
    //                            {
    //                                bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
    //                                if (sbTemp.Length > 20)
    //                                    // TemplateString.Append(sbTemp);
    //                                    TemplateString.Append(sbTemp + "<br/>");
    //                            }

    //                            sbTemp = null;
    //                            dsDymanicTemplateData.Dispose();
    //                            dvDyTable1.Dispose();
    //                            dvDyTable2.Dispose();
    //                            dvDyTable3.Dispose();
    //                            dvDyTable4.Dispose();
    //                            dvDyTable5.Dispose();
    //                            dvDyTable6.Dispose();
    //                            dtDyTempTable.Dispose();
    //                            sSectionId = "";
    //                            #endregion
    //                            //}

    //                            Templinespace = "";
    //                        }
    //                        #endregion

    //                        #region All the Templates except Hitory and Plan of case
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                            && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
    //                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
    //                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
    //                        {
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            dv.Dispose();
    //                            sbTemp = new StringBuilder();
    //                            //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                            //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                            //{
    //                            #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

    //                            DataSet dsDymanicTemplateData = new DataSet();

    //                            DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
    //                            DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
    //                            DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
    //                            DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
    //                            DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
    //                            DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
    //                            DataTable dtDyTempTable = new DataTable();

    //                            dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //                            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
    //                            if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            else
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            string sSectionId = "0";
    //                            for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
    //                            {
    //                                sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
    //                                    : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
    //                            }
    //                            dvDyTable2.ToTable().TableName = "FieldName";
    //                            dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
    //                            dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

    //                            dvDyTable3.ToTable().TableName = "PatientValue";
    //                            if (dvDyTable3.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }
    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }

    //                            dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dtDyTempTable);
    //                            }
    //                            else
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
    //                            }
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
    //                            if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
    //                            {
    //                                bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
    //                                if (sbTemp.Length > 20)
    //                                    TemplateString.Append(sbTemp + "<br/>");
    //                            }
    //                            sbTemp = null;
    //                            dvDyTable1.Dispose();
    //                            dvDyTable2.Dispose();
    //                            dvDyTable3.Dispose();
    //                            dvDyTable4.Dispose();
    //                            dvDyTable5.Dispose();
    //                            dvDyTable6.Dispose();
    //                            dtDyTempTable.Dispose();
    //                            dsDymanicTemplateData.Dispose();
    //                            sSectionId = "";
    //                            #endregion
    //                            //}

    //                            Templinespace = "";
    //                        }
    //                        #endregion

    //                        #region Template Plan of Care
    //                        if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                             && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
    //                            && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
    //                        {
    //                            DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                            dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
    //                            if (dv.Count > 0)
    //                            {
    //                                drTemplateStyle = dv[0].Row;
    //                                string sBegin = "", sEnd = "";
    //                                Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                            }
    //                            dv.Dispose();
    //                            sbTemp = new StringBuilder();
    //                            //if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                            //    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                            //{
    //                            #region Assign Data and call Dynamic Template Plan of Care
    //                            DataSet dsDymanicTemplateData = new DataSet();

    //                            DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
    //                            DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
    //                            DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
    //                            DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
    //                            DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
    //                            DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
    //                            DataTable dtDyTempTable = new DataTable();

    //                            dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //                            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
    //                            if (common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }
    //                            else
    //                            {
    //                                dvDyTable1.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dvDyTable4.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dvDyTable6.RowFilter = "TemplateId=" + common.myStr(ddlTemplatePatient.SelectedValue);
    //                                dtDyTempTable = dvDyTable4.ToTable();
    //                                dvDyTable4.Sort = "RecordId ASC";
    //                            }

    //                            string sSectionId = "0";
    //                            for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
    //                            {
    //                                sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
    //                                    : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
    //                            }
    //                            dvDyTable2.ToTable().TableName = "FieldName";
    //                            dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
    //                            dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

    //                            dvDyTable3.ToTable().TableName = "PatientValue";
    //                            if (dvDyTable3.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }
    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
    //                            }

    //                            dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());

    //                            if (dvDyTable4.ToTable().Rows.Count > 0)
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dtDyTempTable);
    //                            }
    //                            else
    //                            {
    //                                dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
    //                            }
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
    //                            dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
    //                            if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
    //                            {
    //                                bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
    //                                if (sbTemp.Length > 20)
    //                                    TemplateString.Append(sbTemp + "<br/>");
    //                            }
    //                            sbTemp = null;

    //                            dvDyTable1.Dispose();
    //                            dvDyTable2.Dispose();
    //                            dvDyTable3.Dispose();
    //                            dvDyTable4.Dispose();
    //                            dvDyTable5.Dispose();
    //                            dvDyTable6.Dispose();
    //                            dtDyTempTable.Dispose();
    //                            dsDymanicTemplateData.Dispose();
    //                            sSectionId = "";
    //                            #endregion
    //                            //}

    //                            Templinespace = "";
    //                        }
    //                        #endregion

    //                    }
    //                    if (TemplateString.Length > 30)
    //                    {
    //                        //if (iEn == 0)
    //                        //{
    //                        //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
    //                        //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
    //                        //sb.Append("</span>");
    //                        //}
    //                        sb.Append("<span style='" + String.Empty + "'>");
    //                        sb.Append(TemplateString);
    //                        sb.Append("</span><br/>");
    //                        TemplateString = null;
    //                    }
    //                }
    //                #endregion
    //            }
    //        }
    //        Session["NoAllergyDisplay"] = null;
    //        if (sign == true)
    //        {
    //            sb.Append(hdnDoctorImage.Value);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + ex.Message;
    //        objException.HandleException(ex);
    //    }
    //    finally
    //    {
    //        Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;

    //        sbTemplateStyle = null;
    //        TemplateString = null;
    //        ds.Dispose();
    //        dsTemplateStyle.Dispose();
    //        dvDataFilter.Dispose();
    //        drTemplateStyle = null;
    //        dtTemplate.Dispose();
    //        Templinespace = "";
    //        bnotes = null;
    //        fun = null;
    //        emr = null;
    //        sbTemp = null;
    //        BindCaseSheet = null;
    //        dsTemplateData.Dispose();
    //    }
    //    return sb.ToString();
    //}

    //protected void bindData(DataSet dsDynamicTemplateData, string TemplateId, StringBuilder sb, string GroupingDate)
    //{
    //    DataSet ds = new DataSet();
    //    DataSet dsAllNonTabularSectionDetails = new DataSet();
    //    DataSet dsAllTabularSectionDetails = new DataSet();
    //    DataSet dsAllFieldsDetails = new DataSet();

    //    DataTable dtFieldValue = new DataTable();
    //    DataTable dtEntry = new DataTable();
    //    DataTable dtFieldName = new DataTable();

    //    DataView dv = new DataView();
    //    DataView dv1 = new DataView();
    //    DataView dv2 = new DataView();

    //    DataRow dr3;

    //    StringBuilder objStrTmp = new StringBuilder();
    //    StringBuilder objStrSettings = new StringBuilder();
    //    StringBuilder str = new StringBuilder();
    //    string sEntryType = "V";
    //    string BeginList = string.Empty;
    //    string EndList = string.Empty;
    //    string BeginList2 = string.Empty;
    //    string BeginList3 = string.Empty;
    //    string EndList3 = string.Empty;
    //    string sBegin = string.Empty;
    //    string sEnd = string.Empty;

    //    int t = 0;
    //    int t2 = 0;
    //    int t3 = 0;
    //    int iRecordId = 0;
    //    DataView dvDyTable1 = new DataView();
    //    try
    //    {
    //        BeginList = string.Empty;
    //        EndList = string.Empty;
    //        BeginList2 = string.Empty;
    //        BeginList3 = string.Empty;
    //        EndList3 = string.Empty;

    //        t = 0;
    //        t2 = 0;
    //        t3 = 0;

    //        dvDyTable1 = new DataView(dsDynamicTemplateData.Tables[0]);
    //        DataView dvDyTable2 = new DataView(dsDynamicTemplateData.Tables[1]);
    //        DataView dvDyTable3 = new DataView(dsDynamicTemplateData.Tables[2]);

    //        dvDyTable1.ToTable().TableName = "TemplateSectionName";
    //        dvDyTable2.ToTable().TableName = "FieldName";
    //        dvDyTable3.ToTable().TableName = "PatientValue";
    //        dsAllNonTabularSectionDetails = new DataSet();
    //        if (dvDyTable3.ToTable().Rows.Count > 0)
    //        {
    //            dsAllNonTabularSectionDetails.Tables.Add(dvDyTable2.ToTable());
    //            dsAllNonTabularSectionDetails.Tables.Add(dvDyTable3.ToTable());
    //        }
    //        dvDyTable2.Dispose();
    //        dvDyTable3.Dispose();

    //        dsDynamicTemplateData.Dispose();

    //        #region Non Tabular
    //        if (dsAllNonTabularSectionDetails.Tables.Count > 0 && dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
    //        {
    //            DataView dvNonTabular = new DataView(dvDyTable1.ToTable());
    //            dvNonTabular.RowFilter = "Tabular=0";
    //            if (dvNonTabular.ToTable().Rows.Count > 0)
    //            {
    //                ds = new DataSet();
    //                ds.Tables.Add(dvNonTabular.ToTable());//Section Name Table

    //                dv = new DataView(dsAllNonTabularSectionDetails.Tables[1]);

    //                dv.Sort = "RecordId DESC";
    //                dtEntry = dv.ToTable(true, "RecordId");
    //                iRecordId = 0;
    //                dv.Dispose();
    //                dvNonTabular.Dispose();

    //                for (int it = 0; it < dtEntry.Rows.Count; it++)
    //                {
    //                    if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
    //                    {
    //                        foreach (DataRow item in ds.Tables[0].Rows)
    //                        {
    //                            dv1 = new DataView(dsAllNonTabularSectionDetails.Tables[0]);
    //                            dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
    //                            dtFieldName = dv1.ToTable();

    //                            if (dsAllNonTabularSectionDetails.Tables.Count > 1)
    //                            {
    //                                dv2 = new DataView(dsAllNonTabularSectionDetails.Tables[1]);
    //                                dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]) + " AND SectionId=" + common.myStr(item["SectionId"]);
    //                                dtFieldValue = dv2.ToTable();
    //                                dv2.Dispose();
    //                            }

    //                            dsAllFieldsDetails = new DataSet();
    //                            dsAllFieldsDetails.Tables.Add(dtFieldName);
    //                            dsAllFieldsDetails.Tables.Add(dtFieldValue);

    //                            dtFieldName.Dispose();
    //                            dtFieldValue.Dispose();
    //                            dv1.Dispose();

    //                            if (dsAllNonTabularSectionDetails.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (dsAllNonTabularSectionDetails.Tables.Count > 1)
    //                                {
    //                                    if (dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
    //                                    {
    //                                        sBegin = string.Empty;
    //                                        sEnd = string.Empty;
    //                                        dr3 = dsAllNonTabularSectionDetails.Tables[0].Rows[0];
    //                                        getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
    //                                        ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

    //                                        str = new StringBuilder();

    //                                        str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
    //                                                    common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
    //                                                    common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

    //                                        str.Append("<br/> ");


    //                                        dr3 = null;
    //                                        dsAllNonTabularSectionDetails.Dispose();
    //                                        dsAllFieldsDetails.Dispose();
    //                                        string sBreak = common.myBool(item["IsConfidential"]) == true ? "<br/>" : "";
    //                                        if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
    //                                        {
    //                                            if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
    //                                            {
    //                                                if (sEntryType.Equals("M"))
    //                                                {
    //                                                    objStrTmp.Append("<br/>");
    //                                                }
    //                                            }
    //                                            if (t2.Equals(0))
    //                                            {
    //                                                if (t3.Equals(0))//Template
    //                                                {
    //                                                    t3 = 1;
    //                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
    //                                                    {
    //                                                        BeginList3 = "<ul>";
    //                                                        EndList3 = "</ul>";
    //                                                    }
    //                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
    //                                                    {
    //                                                        BeginList3 = "<ol>";
    //                                                        EndList3 = "</ol>";
    //                                                    }
    //                                                }
    //                                            }

    //                                            if (common.myStr(item["SectionsBold"]) != string.Empty
    //                                                || common.myStr(item["SectionsItalic"]) != string.Empty
    //                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
    //                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
    //                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
    //                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
    //                                            {
    //                                                sBegin = string.Empty;
    //                                                sEnd = string.Empty;
    //                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty))
    //                                                    {
    //                                                        objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
    //                                                    }
    //                                                }
    //                                                BeginList3 = string.Empty;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty))
    //                                                    {
    //                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
    //                                                    }
    //                                                }
    //                                            }

    //                                            if (!str.ToString().Trim().Equals(string.Empty))
    //                                            {
    //                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
    //                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
    //                                                {
    //                                                    ////// objStrTmp.Append("<br />"); //code commented  for Examination (SectonName and fieldname getting extra space)
    //                                                }
    //                                                objStrTmp.Append(str.ToString());
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (t.Equals(0))
    //                                            {
    //                                                t = 1;
    //                                                if (common.myInt(item["TemplateListStyle"]).Equals(1))
    //                                                {
    //                                                    BeginList = "<ul>"; EndList = "</ul>";
    //                                                }
    //                                                else if (common.myInt(item["TemplateListStyle"]).Equals(2))
    //                                                {
    //                                                    BeginList = "<ol>"; EndList = "</ol>";
    //                                                }
    //                                            }
    //                                            if (common.myStr(item["TemplateBold"]) != string.Empty
    //                                                || common.myStr(item["TemplateItalic"]) != string.Empty
    //                                                || common.myStr(item["TemplateUnderline"]) != string.Empty
    //                                                || common.myStr(item["TemplateFontSize"]) != string.Empty
    //                                                || common.myStr(item["TemplateForecolor"]) != string.Empty
    //                                                || common.myStr(item["TemplateListStyle"]) != string.Empty)
    //                                            {
    //                                                sBegin = string.Empty;
    //                                                sEnd = string.Empty;
    //                                                MakeFont("Template", ref sBegin, ref sEnd, item);
    //                                                if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
    //                                                {
    //                                                    if (sBegin.Contains("<br/>"))
    //                                                    {
    //                                                        sBegin = sBegin.Remove(0, 5);
    //                                                        objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
    //                                                    }
    //                                                }
    //                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
    //                                                {
    //                                                    objStrTmp.Append("<br/>");
    //                                                }
    //                                                BeginList = string.Empty;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (common.myBool(item["TemplateDisplayTitle"]))
    //                                                {
    //                                                    objStrTmp.Append(sBreak + common.myStr(item["TemplateName"]));//Default Setting
    //                                                }
    //                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
    //                                                {
    //                                                    objStrTmp.Append("<br/>");
    //                                                }
    //                                            }
    //                                            if (common.myInt(item["TemplateListStyle"]).Equals(3)
    //                                                || common.myInt(item["TemplateListStyle"]).Equals(0))
    //                                            {
    //                                                //objStrTmp.Append("<br />");
    //                                            }

    //                                            objStrTmp.Append(EndList);
    //                                            if (t2.Equals(0))
    //                                            {
    //                                                t2 = 1;
    //                                                if (common.myInt(item["SectionsListStyle"]).Equals(1))
    //                                                {
    //                                                    BeginList2 = "<ul>";
    //                                                    EndList3 = "</ul>";
    //                                                }
    //                                                else if (common.myInt(item["SectionsListStyle"]).Equals(2))
    //                                                {
    //                                                    BeginList2 = "<ol>";
    //                                                    EndList3 = "</ol>";
    //                                                }
    //                                            }
    //                                            if (common.myStr(item["SectionsBold"]) != string.Empty
    //                                                || common.myStr(item["SectionsItalic"]) != string.Empty
    //                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
    //                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
    //                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
    //                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
    //                                            {
    //                                                sBegin = string.Empty;
    //                                                sEnd = string.Empty;
    //                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
    //                                                    {

    //                                                        if (sBegin.StartsWith("<br/>"))
    //                                                        {
    //                                                            if (sBegin.Length > 5)
    //                                                            {

    //                                                                //sBegin = sBegin.Remove(0, 5);
    //                                                                //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
    //                                                                sBegin = sBegin.Substring(5, sBegin.Length - 5);
    //                                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                                            }
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);

    //                                                        }

    //                                                        //if (sBegin.Contains("<br/>"))
    //                                                        //{
    //                                                        //    sBegin = sBegin.Remove(0, 5);
    //                                                        //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                                        //}
    //                                                        //else
    //                                                        //{

    //                                                        //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                                        //}

    //                                                    }
    //                                                }
    //                                                BeginList2 = string.Empty;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
    //                                                    {
    //                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
    //                                                    }
    //                                                }
    //                                            }
    //                                            if (common.myInt(item["SectionsListStyle"]).Equals(3)
    //                                                || common.myInt(item["SectionsListStyle"]).Equals(0))
    //                                            {
    //                                                //objStrTmp.Append("<br />");
    //                                            }

    //                                            objStrTmp.Append(str.ToString());
    //                                        }
    //                                        //if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
    //                                        //{
    //                                        iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
    //                                        ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
    //                                        // }
    //                                    }
    //                                    str = null;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        #endregion
    //        #region Tabular
    //        DataView dvDyTable4 = new DataView(dsDynamicTemplateData.Tables[3]);
    //        DataView dvDyTable5 = new DataView(dsDynamicTemplateData.Tables[4]);
    //        DataView dvDyTable6 = new DataView(dsDynamicTemplateData.Tables[5]);

    //        dvDyTable4.ToTable().TableName = "TabularData";
    //        dvDyTable5.ToTable().TableName = "TabularColumnCount";
    //        dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";

    //        dsAllTabularSectionDetails = new DataSet();
    //        if (dvDyTable4.ToTable().Rows.Count > 0)
    //        {
    //            dsAllTabularSectionDetails.Tables.Add(dvDyTable4.ToTable());
    //            dsAllTabularSectionDetails.Tables.Add(dvDyTable5.ToTable());
    //            dsAllTabularSectionDetails.Tables.Add(dvDyTable6.ToTable());
    //        }

    //        dvDyTable4.Dispose();
    //        dvDyTable5.Dispose();



    //        if (dsAllTabularSectionDetails.Tables.Count > 0 && dsAllTabularSectionDetails.Tables[1].Rows.Count > 0)
    //        {
    //            DataView dvTabular = new DataView(dvDyTable1.ToTable());
    //            dvTabular.RowFilter = "Tabular=1";
    //            if (dvTabular.ToTable().Rows.Count > 0)
    //            {
    //                ds = new DataSet();
    //                ds.Tables.Add(dvTabular.ToTable());//Section Name Table
    //                dv = new DataView(dsAllTabularSectionDetails.Tables[0]);
    //                dv.Sort = "RecordId DESC";
    //                dtEntry = dv.ToTable(true, "RecordId");
    //                iRecordId = 0;
    //                dv.Dispose();
    //                dvTabular.Dispose();
    //                for (int it = 0; it < dtEntry.Rows.Count; it++)
    //                {
    //                    if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
    //                    {
    //                        foreach (DataRow item in ds.Tables[0].Rows)
    //                        {
    //                            dv1 = new DataView(dsAllTabularSectionDetails.Tables[0]);
    //                            dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
    //                            DataView dvFieldStyle = new DataView(dsAllTabularSectionDetails.Tables[2]);
    //                            dvFieldStyle.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
    //                            dtFieldName = dv1.ToTable();

    //                            if (dsAllTabularSectionDetails.Tables.Count > 1)
    //                            {
    //                                dv2 = new DataView(dsAllTabularSectionDetails.Tables[1]);
    //                                dv2.RowFilter = " SectionId=" + common.myStr(item["SectionId"]);
    //                                dtFieldValue = dv2.ToTable();
    //                                dv2.Dispose();
    //                            }

    //                            dsAllFieldsDetails = new DataSet();
    //                            dsAllFieldsDetails.Tables.Add(dtFieldName);
    //                            dsAllFieldsDetails.Tables.Add(dtFieldValue);

    //                            dsAllFieldsDetails.Tables.Add(dvDyTable6.ToTable());
    //                            dvDyTable6.Dispose();
    //                            dtFieldName.Dispose();
    //                            dtFieldValue.Dispose();
    //                            dv1.Dispose();

    //                            if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
    //                            {
    //                                if (dsAllTabularSectionDetails.Tables.Count > 1)
    //                                {
    //                                    if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
    //                                    {
    //                                        sBegin = string.Empty;
    //                                        sEnd = string.Empty;
    //                                        dr3 = dvFieldStyle.ToTable().Rows[0];
    //                                        getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
    //                                        ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

    //                                        str = new StringBuilder();
    //                                        str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
    //                                                    common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
    //                                                    common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

    //                                        str.Append("<br/> ");

    //                                        dr3 = null;
    //                                        dsAllTabularSectionDetails.Dispose();
    //                                        dsAllFieldsDetails.Dispose();

    //                                        if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
    //                                        {
    //                                            if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
    //                                            {
    //                                                if (sEntryType.Equals("M"))
    //                                                {
    //                                                    objStrTmp.Append("<br/>");
    //                                                }
    //                                            }
    //                                            if (t2.Equals(0))
    //                                            {
    //                                                if (t3.Equals(0))//Template
    //                                                {
    //                                                    t3 = 1;
    //                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
    //                                                    {
    //                                                        BeginList3 = "<ul>";
    //                                                        EndList3 = "</ul>";
    //                                                    }
    //                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
    //                                                    {
    //                                                        BeginList3 = "<ol>";
    //                                                        EndList3 = "</ol>";
    //                                                    }
    //                                                }
    //                                            }

    //                                            if (common.myStr(item["SectionsBold"]) != string.Empty
    //                                                || common.myStr(item["SectionsItalic"]) != string.Empty
    //                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
    //                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
    //                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
    //                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
    //                                            {
    //                                                sBegin = string.Empty;
    //                                                sEnd = string.Empty;
    //                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty))
    //                                                    {
    //                                                        objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
    //                                                    }
    //                                                }
    //                                                BeginList3 = string.Empty;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty))
    //                                                    {
    //                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
    //                                                    }
    //                                                }
    //                                            }

    //                                            if (!str.ToString().Trim().Equals(string.Empty))
    //                                            {
    //                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
    //                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
    //                                                {
    //                                                    objStrTmp.Append("<br />");
    //                                                }
    //                                                objStrTmp.Append(str.ToString());
    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (t.Equals(0))
    //                                            {
    //                                                t = 1;
    //                                                if (common.myInt(item["TemplateListStyle"]).Equals(1))
    //                                                {
    //                                                    BeginList = "<ul>"; EndList = "</ul>";
    //                                                }
    //                                                else if (common.myInt(item["TemplateListStyle"]).Equals(2))
    //                                                {
    //                                                    BeginList = "<ol>"; EndList = "</ol>";
    //                                                }
    //                                            }
    //                                            if (common.myStr(item["TemplateBold"]) != string.Empty
    //                                                || common.myStr(item["TemplateItalic"]) != string.Empty
    //                                                || common.myStr(item["TemplateUnderline"]) != string.Empty
    //                                                || common.myStr(item["TemplateFontSize"]) != string.Empty
    //                                                || common.myStr(item["TemplateForecolor"]) != string.Empty
    //                                                || common.myStr(item["TemplateListStyle"]) != string.Empty)
    //                                            {
    //                                                sBegin = string.Empty;
    //                                                sEnd = string.Empty;
    //                                                MakeFont("Template", ref sBegin, ref sEnd, item);
    //                                                if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty))
    //                                                    {
    //                                                        if (sBegin.Contains("<br/>"))
    //                                                        {
    //                                                            sBegin = sBegin.Remove(0, 5);
    //                                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
    //                                                        }
    //                                                        else
    //                                                        {
    //                                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
    //                                                        }
    //                                                    }
    //                                                }
    //                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
    //                                                {
    //                                                    objStrTmp.Append("<br/>");
    //                                                }
    //                                                BeginList = string.Empty;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (common.myBool(item["TemplateDisplayTitle"]))
    //                                                {
    //                                                    objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
    //                                                }
    //                                                if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
    //                                                {
    //                                                    objStrTmp.Append("<br/>");
    //                                                }
    //                                            }
    //                                            if (common.myInt(item["TemplateListStyle"]).Equals(3)
    //                                                || common.myInt(item["TemplateListStyle"]).Equals(0))
    //                                            {
    //                                                //objStrTmp.Append("<br />");
    //                                            }

    //                                            objStrTmp.Append(EndList);
    //                                            if (t2.Equals(0))
    //                                            {
    //                                                t2 = 1;
    //                                                if (common.myInt(item["SectionsListStyle"]).Equals(1))
    //                                                {
    //                                                    BeginList2 = "<ul>";
    //                                                    EndList3 = "</ul>";
    //                                                }
    //                                                else if (common.myInt(item["SectionsListStyle"]).Equals(2))
    //                                                {
    //                                                    BeginList2 = "<ol>";
    //                                                    EndList3 = "</ol>";
    //                                                }
    //                                            }
    //                                            if (common.myStr(item["SectionsBold"]) != string.Empty
    //                                                || common.myStr(item["SectionsItalic"]) != string.Empty
    //                                                || common.myStr(item["SectionsUnderline"]) != string.Empty
    //                                                || common.myStr(item["SectionsFontSize"]) != string.Empty
    //                                                || common.myStr(item["SectionsForecolor"]) != string.Empty
    //                                                || common.myStr(item["SectionsListStyle"]) != string.Empty)
    //                                            {
    //                                                sBegin = string.Empty;
    //                                                sEnd = string.Empty;
    //                                                MakeFont("Sections", ref sBegin, ref sEnd, item);
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
    //                                                    {
    //                                                        objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                                    }
    //                                                }
    //                                                BeginList2 = string.Empty;
    //                                            }
    //                                            else
    //                                            {
    //                                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
    //                                                {
    //                                                    if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
    //                                                    {
    //                                                        objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
    //                                                    }
    //                                                }
    //                                            }
    //                                            if (common.myInt(item["SectionsListStyle"]).Equals(3)
    //                                                || common.myInt(item["SectionsListStyle"]).Equals(0))
    //                                            {
    //                                                //objStrTmp.Append("<br />");
    //                                            }

    //                                            objStrTmp.Append(str.ToString());
    //                                        }
    //                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
    //                                        {
    //                                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
    //                                            ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
    //                                        }
    //                                    }
    //                                    str = null;
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        #endregion
    //        if (t2.Equals(1) && t3.Equals(1))
    //        {
    //            objStrTmp.Append(EndList3);
    //        }
    //        else
    //        {
    //            objStrTmp.Append(EndList);
    //        }
    //        if (GetPageProperty("1") != null)
    //        {
    //            objStrSettings.Append(objStrTmp.ToString());
    //            sb.Append(objStrSettings.ToString());
    //        }
    //        else
    //        {
    //            sb.Append(objStrTmp.ToString());
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + ex.Message;
    //        objException.HandleException(ex);
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //        dsAllNonTabularSectionDetails.Dispose();
    //        dsAllTabularSectionDetails.Dispose();
    //        dsAllFieldsDetails.Dispose();

    //        dtFieldValue.Dispose();
    //        dtEntry.Dispose();
    //        dtFieldName.Dispose();
    //        dvDyTable1.Dispose();
    //        dv.Dispose();
    //        dv1.Dispose();
    //        dv2.Dispose();

    //        dr3 = null;

    //        objStrTmp = null;
    //        objStrSettings = null;

    //        sEntryType = string.Empty;
    //        BeginList = string.Empty;
    //        EndList = string.Empty;
    //        BeginList2 = string.Empty;
    //        BeginList3 = string.Empty;
    //        EndList3 = string.Empty;
    //        sBegin = string.Empty;
    //        sEnd = string.Empty;
    //    }
    //}

    protected string CreateStringNew(DataSet objDs, int iRootId, string iRootName, string TabularType,
                    string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        DataSet dsTabulerTemplate = new DataSet();
        try
        {
            if (objDs != null)
            {
                if (IsConfidential == false)
                {
                    #region Tabular
                    if (bool.Parse(TabularType) == true)
                    {
                        DataView dvFilter = new DataView(objDs.Tables[0]);
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;
                            dvFilter.Sort = "RowNum ASC";
                            if (GroupingDate != "")
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
                            }
                            DataTable dtNewTable = dvFilter.ToTable();
                            if (dtNewTable.Rows.Count > 0)
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                StringBuilder sbCation = new StringBuilder();
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br/><table border='1' style='border-color:#000000; border:solid; border-collapse:collapse; " + sFontSize + "' cellspacing='3'><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();
                                        for (int k = 0; k < column; k++)
                                        {
                                            sbCation.Append("<td>");
                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                            sbCation.Append("</td>");
                                            count++;
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId='" + RecordId + "' AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId='" + RecordId + "'";
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 1; i < ColumnCount + 1; i++)
                                            {
                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["Col" + i]).Replace("\n", "<br/>") + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        dt.Dispose();
                                        dvData.Dispose();
                                    }
                                    sbCation.Append("</table>");
                                }
                                objStr.Append(sbCation);
                                dsTabulerTemplate.Dispose();
                                sbCation = null;

                            }
                            else
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                if (GroupingDate != "")
                                {
                                    dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                                }
                                else
                                {
                                    dvRowCaption.RowFilter = "RecordId= " + RecordId;
                                }
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    StringBuilder sbCation = new StringBuilder();
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    // dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br/><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();

                                        for (int k = 0; k < column + 1; k++)
                                        {
                                            if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                                && ColumnCount == 0)
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(" + ");
                                                sbCation.Append("</td>");
                                            }
                                            else
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                            }
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 0; i < ColumnCount; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
                                                }
                                                else
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        sbCation.Append("</table>");
                                        dvData.Dispose();
                                    }
                                    objStr.Append(sbCation);
                                    dt.Dispose();
                                    sbCation = null;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Non Tabular
                    else // For Non Tabular Templates
                    {
                        if (chkIncludePastHistory.Checked)
                        {

                            string BeginList = "", EndList = "";
                            string sBegin = "", sEnd = "";
                            int t = 0;
                            string FieldId = "";
                            string sStaticTemplate = "";
                            string sEnterBy = "";
                            string sVisitDate = "";

                            StringBuilder sbAllData = new StringBuilder();
                            StringBuilder sbTdData = new StringBuilder();
                            StringBuilder sbMultipleValue = new StringBuilder();
                            StringBuilder sbVisitDate = new StringBuilder();

                            string trStart = "<tr valign='top'>";
                            string trEnd = "</tr>";
                            string tdFieldNameStart = "<td valign='top'>";
                            string tdFieldValueStart = "<td valign='top' colspan='4'>";
                            string tdEnd = "</td>";


                            var GroupDateCount = objDs.Tables[1].AsEnumerable()
                .Select(row => new
                {
                    GroupDate = row.Field<string>("GroupDate"),
                }).Distinct().ToArray();

                            ArrayList arrGroupDate = new ArrayList(GroupDateCount);
                            DataView dvGroupDate = objDs.Tables[1].DefaultView;
                            for (int k = 0; k < arrGroupDate.Count; k++)
                            {
                                sbVisitDate = new StringBuilder();
                                dvGroupDate.RowFilter = "GroupDate='" + common.myStr(arrGroupDate[k]).Substring(14, 10) + "'";
                                sbVisitDate.Append(trStart + "<td valign='top' colspan='5'>" + sBegin + " Visit Date : " + common.myStr(arrGroupDate[k]).Substring(14, 10) + sEnd + tdEnd + trEnd);// + " Encounter No : " + common.myStr(dvGroupDate.ToTable().Rows[0]["EncounterId"]) + sEnd + tdEnd);
                                int intFieldCnt = 0;
                                foreach (DataRow item in objDs.Tables[0].Rows)
                                {
                                    intFieldCnt++;

                                    sbTdData = new StringBuilder();

                                    sbMultipleValue = new StringBuilder();

                                    objDv = new DataView(dvGroupDate.ToTable());
                                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                    objDt = objDv.ToTable();
                                    if (t == 0)
                                    {
                                        t = 1;
                                        if (common.myStr(item["FieldsListStyle"]) == "1")
                                        {
                                            BeginList = "<ul>"; EndList = "</ul>";
                                        }
                                        else if (item["FieldsListStyle"].ToString() == "2")
                                        {
                                            BeginList = "<ol>"; EndList = "</ol>";
                                        }
                                    }
                                    if (common.myStr(item["FieldsBold"]) != ""
                                        || common.myStr(item["FieldsItalic"]) != ""
                                        || common.myStr(item["FieldsUnderline"]) != ""
                                        || common.myStr(item["FieldsFontSize"]) != ""
                                        || common.myStr(item["FieldsForecolor"]) != ""
                                        || common.myStr(item["FieldsListStyle"]) != "")
                                    {
                                        if (objDt.Rows.Count > 0)
                                        {
                                            sBegin = "";
                                            sEnd = "";

                                            MakeFont("Fields", ref sBegin, ref sEnd, item);

                                            if (sBegin.StartsWith("<br/>"))
                                            {
                                                if (sBegin.Length > 5)
                                                {
                                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                                }
                                            }
                                            if (Convert.ToBoolean(item["DisplayTitle"]))
                                            {
                                                //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));

                                                sbTdData.Append(tdFieldNameStart + sBegin + common.myStr(item["FieldName"]) + sEnd + tdEnd);
                                                // sbTdData.Append(tdFieldNameStart + sBegin + common.myStr(item["FieldName"]) + sEnd + tdEnd);




                                                //if (objStr.ToString() != "")
                                                //{
                                                //    objStr.Append(sEnd + "</li>");
                                                //}
                                                ViewState["sBegin"] = sBegin;
                                            }
                                            else
                                            {
                                                sbTdData.Append(tdFieldNameStart + tdEnd);
                                            }

                                            BeginList = "";
                                            sBegin = "";
                                            sEnd = "";

                                        }
                                    }
                                    else
                                    {
                                        if (objDt.Rows.Count > 0)
                                        {
                                            //if (sStaticTemplate != "<br/><br/>")
                                            //{
                                            //    objStr.Append(common.myStr(item["FieldName"]));
                                            //}

                                            sbTdData.Append(tdFieldNameStart + common.myStr(item["FieldName"]) + tdEnd);
                                        }
                                    }
                                    if (objDs.Tables.Count > 1)
                                    {
                                        objDv = new DataView(dvGroupDate.ToTable());
                                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                        objDt = objDv.ToTable();
                                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                                        sBegin = "";
                                        sEnd = "";

                                        string sbeginTemp = string.Empty;
                                        MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);

                                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                                        {
                                            if (objDt.Rows.Count > 0)
                                            {
                                                sbeginTemp = common.myStr(ViewState["sBegin"]);
                                                if (sbeginTemp.StartsWith("<br/>"))
                                                {
                                                    if (sbeginTemp.Length > 5)
                                                    {
                                                        sbeginTemp = sbeginTemp.Substring(0, 5);
                                                    }
                                                }

                                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                                if (FType == "C")
                                                {
                                                    FType = "C";
                                                }
                                                if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                                {
                                                    if (FType == "B")
                                                    {
                                                        //objStr.Append(" : " + objDt.Rows[i]["TextValue"]);
                                                        sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                    }
                                                    else
                                                    {
                                                        BindDataValueNew(objDs, objDt, sbMultipleValue, i, FType, sBegin.Replace("font-weight: bold;", string.Empty), sEnd);
                                                    }
                                                }
                                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "ST" || FType == "SB" || FType == "W"
                                                    || FType == "I" || FType == "IS")
                                                {
                                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                                    {
                                                        if (i == 0)
                                                        {
                                                            if (FType == "M" || FType == "W" || FType == "IS")
                                                            {
                                                                //objStr.Append(sBegin + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                                if (FType == "M" || FType == "IS")
                                                                {
                                                                    sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n", "<br/>") + tdEnd);
                                                                }
                                                                else if (FType == "W")
                                                                {
                                                                    if (common.myStr(objDt.Rows[i]["TextValue"]).Contains("<table"))
                                                                    {
                                                                        sbTdData.Append(tdFieldValueStart + tdEnd);

                                                                        sbAllData.Append(trStart + sbTdData.ToString() + trEnd);

                                                                        sbTdData = new StringBuilder();
                                                                        sbTdData.Append("<td valign='top' colspan='5'>" + common.myStr(objDt.Rows[i]["TextValue"]) + "</td>");
                                                                    }
                                                                    else
                                                                    {
                                                                        sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                                sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]).Replace("<", "&lt;").Replace(">", "&gt;") + tdEnd);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                            sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (i == 0)
                                                        {
                                                            //objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                            sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                        }
                                                        else
                                                        {
                                                            //objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                            sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                        }
                                                    }
                                                }
                                                else if (FType == "L")
                                                {
                                                    //objStr.Append(BindStaticTemplates(Convert.ToInt32(objDt.Rows[0]["StaticTemplateId"]), Convert.ToInt32(objDt.Rows[0]["FieldId"])));

                                                    sbTdData.Append(tdFieldValueStart + common.myStr(BindStaticTemplates(Convert.ToInt32(objDt.Rows[0]["StaticTemplateId"]), Convert.ToInt32(objDt.Rows[0]["FieldId"])) + tdEnd));
                                                }
                                                else if (FType == "IM")
                                                {
                                                    //objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
                                                    sbTdData.Append("<td valign='top' colspan='3'>" + common.myStr(BindNonTabularImageTypeFieldValueTemplatesNew(objDt)) + tdEnd);
                                                }
                                                else if (FType == "H")
                                                {
                                                    sbTdData.Append(tdFieldValueStart + "&nbsp;" + tdEnd);
                                                }
                                                if (common.myStr(item["FieldsListStyle"]) == "")
                                                {
                                                    if (ViewState["iTemplateId"].ToString() != "163")
                                                    {
                                                        if (FType != "C")
                                                        {
                                                            if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
                                                            {
                                                            }
                                                            else
                                                            {
                                                                //objStr.Append("<br />");
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (FType != "C" && FType != "T")
                                                        {
                                                            //objStr.Append("<br />");
                                                        }
                                                    }
                                                }
                                            }

                                            sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                                            sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                                        }

                                        if (!sbMultipleValue.ToString().Equals(string.Empty))
                                        {
                                            sbTdData.Append(tdFieldValueStart + common.myStr(sbMultipleValue.ToString()) + tdEnd);
                                        }

                                        sBegin = "";
                                        sEnd = "";
                                        dvFieldType.Dispose();
                                        dtFieldType.Dispose();
                                    }

                                    if (!sbTdData.ToString().Equals(string.Empty))
                                    {
                                        if (intFieldCnt.Equals(1))
                                        {
                                            sbAllData.Append(sbVisitDate.ToString());
                                            sbVisitDate = new StringBuilder();
                                        }

                                        sbAllData.Append(trStart + sbTdData.ToString() + trEnd);
                                    }
                                    // sbAllData= sbVisitDate.Append(sbAllData);
                                }

                            }

                            if (!sbAllData.ToString().Equals(string.Empty))
                            {
                                objStr.Append("<table border='0' style='width:99%;" + sFontSize.Replace("font-weight: bold;", string.Empty) + "' cellpadding='2' cellspacing='2'>" +
                                    "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
                                    sbAllData.ToString() +
                                    "</table>");
                            }

                            //sbCation.Append("");

                            //if (objStr.ToString() != "")
                            //{
                            //    objStr.Append(EndList);
                            //}
                        }
                        else // For Non Tabular Templates
                        {
                            string BeginList = "", EndList = "";
                            string sBegin = "", sEnd = "";
                            int t = 0;
                            string FieldId = "";
                            string sStaticTemplate = "";
                            string sEnterBy = "";
                            string sVisitDate = "";

                            StringBuilder sbAllData = new StringBuilder();
                            StringBuilder sbTdData = new StringBuilder();
                            StringBuilder sbMultipleValue = new StringBuilder();

                            string trStart = "<tr valign='top'>";
                            string trEnd = "</tr>";
                            string tdFieldNameStart = "<td valign='top'>";
                            string tdFieldValueStart = "<td valign='top' colspan='5'>";
                            string tdEnd = "</td>";

                            foreach (DataRow item in objDs.Tables[0].Rows)
                            {
                                sbTdData = new StringBuilder();
                                sbMultipleValue = new StringBuilder();

                                objDv = new DataView(objDs.Tables[1]);
                                objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                objDt = objDv.ToTable();
                                if (t == 0)
                                {
                                    t = 1;
                                    if (common.myStr(item["FieldsListStyle"]) == "1")
                                    {
                                        BeginList = "<ul>"; EndList = "</ul>";
                                    }
                                    else if (item["FieldsListStyle"].ToString() == "2")
                                    {
                                        BeginList = "<ol>"; EndList = "</ol>";
                                    }
                                }
                                if (common.myStr(item["FieldsBold"]) != ""
                                    || common.myStr(item["FieldsItalic"]) != ""
                                    || common.myStr(item["FieldsUnderline"]) != ""
                                    || common.myStr(item["FieldsFontSize"]) != ""
                                    || common.myStr(item["FieldsForecolor"]) != ""
                                    || common.myStr(item["FieldsListStyle"]) != "")
                                {
                                    if (objDt.Rows.Count > 0)
                                    {
                                        sBegin = "";
                                        sEnd = "";

                                        MakeFont("Fields", ref sBegin, ref sEnd, item);

                                        if (sBegin.StartsWith("<br/>"))
                                        {
                                            if (sBegin.Length > 5)
                                            {
                                                sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                            }
                                        }

                                        if (Convert.ToBoolean(item["DisplayTitle"]))
                                        {
                                            //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));

                                            sbTdData.Append(tdFieldNameStart + sBegin + common.myStr(item["FieldName"]) + sEnd + tdEnd);

                                            //if (objStr.ToString() != "")
                                            //{
                                            //    objStr.Append(sEnd + "</li>");
                                            //}
                                            ViewState["sBegin"] = sBegin;
                                        }
                                        else
                                        {
                                            sbTdData.Append(tdFieldNameStart + tdEnd);
                                        }

                                        BeginList = "";
                                        sBegin = "";
                                        sEnd = "";

                                    }
                                }
                                else
                                {
                                    if (objDt.Rows.Count > 0)
                                    {
                                        //if (sStaticTemplate != "<br/><br/>")
                                        //{
                                        //    objStr.Append(common.myStr(item["FieldName"]));
                                        //}

                                        sbTdData.Append(tdFieldNameStart + common.myStr(item["FieldName"]) + tdEnd);
                                    }
                                }
                                if (objDs.Tables.Count > 1)
                                {
                                    objDv = new DataView(objDs.Tables[1]);
                                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                    objDt = objDv.ToTable();
                                    DataView dvFieldType = new DataView(objDs.Tables[0]);
                                    dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                    DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                                    sBegin = "";
                                    sEnd = "";

                                    string sbeginTemp = string.Empty;
                                    MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);

                                    for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                                    {
                                        if (objDt.Rows.Count > 0)
                                        {
                                            sbeginTemp = common.myStr(ViewState["sBegin"]);
                                            if (sbeginTemp.StartsWith("<br/>"))
                                            {
                                                if (sbeginTemp.Length > 5)
                                                {
                                                    sbeginTemp = sbeginTemp.Substring(0, 5);
                                                }
                                            }

                                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                            if (FType == "C")
                                            {
                                                FType = "C";
                                            }
                                            if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                            {
                                                if (FType == "B")
                                                {
                                                    //objStr.Append(" : " + objDt.Rows[i]["TextValue"]);
                                                    sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                }
                                                else
                                                {
                                                    BindDataValueNew(objDs, objDt, sbMultipleValue, i, FType, sBegin.Replace("font-weight: bold;", string.Empty), sEnd);
                                                }
                                            }
                                            else if (FType == "T" || FType == "M" || FType == "S" || FType == "ST" || FType == "SB" || FType == "W"
                                                || FType == "I" || FType == "IS")
                                            {
                                                if (common.myStr(ViewState["iTemplateId"]) != "163")
                                                {
                                                    if (i == 0)
                                                    {
                                                        if (FType == "M" || FType == "W" || FType == "IS")
                                                        {
                                                            //objStr.Append(sBegin + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                            if (FType == "M" || FType == "IS")
                                                            {
                                                                sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]).Replace("\n", "<br/>") + tdEnd);
                                                            }
                                                            else if (FType == "W")
                                                            {
                                                                if (common.myStr(objDt.Rows[i]["TextValue"]).Contains("<table"))
                                                                {
                                                                    sbTdData.Append(tdFieldValueStart + tdEnd);

                                                                    sbAllData.Append(trStart + sbTdData.ToString() + trEnd);

                                                                    sbTdData = new StringBuilder();
                                                                    sbTdData.Append("<td valign='top' colspan='5'>" + common.myStr(objDt.Rows[i]["TextValue"]) + "</td>");
                                                                }
                                                                else
                                                                {
                                                                    sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                            sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]).Replace("<", "&lt;").Replace(">", "&gt;") + tdEnd);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                        sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        //objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                    }
                                                    else
                                                    {
                                                        //objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        sbTdData.Append(tdFieldValueStart + common.myStr(objDt.Rows[i]["TextValue"]) + tdEnd);
                                                    }
                                                }
                                            }
                                            else if (FType == "L")
                                            {
                                                //objStr.Append(BindStaticTemplates(Convert.ToInt32(objDt.Rows[0]["StaticTemplateId"]), Convert.ToInt32(objDt.Rows[0]["FieldId"])));

                                                sbTdData.Append(tdFieldValueStart + common.myStr(BindStaticTemplates(Convert.ToInt32(objDt.Rows[0]["StaticTemplateId"]), Convert.ToInt32(objDt.Rows[0]["FieldId"])) + tdEnd));
                                            }
                                            else if (FType == "IM")
                                            {
                                                //objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
                                                sbTdData.Append("<td valign='top' colspan='3'>" + common.myStr(BindNonTabularImageTypeFieldValueTemplatesNew(objDt)) + tdEnd);
                                            }
                                            else if (FType == "H")
                                            {
                                                sbTdData.Append(tdFieldValueStart + "&nbsp;" + tdEnd);
                                            }
                                            if (common.myStr(item["FieldsListStyle"]) == "")
                                            {
                                                if (ViewState["iTemplateId"].ToString() != "163")
                                                {
                                                    if (FType != "C")
                                                    {
                                                        if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
                                                        {
                                                        }
                                                        else
                                                        {
                                                            //objStr.Append("<br />");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (FType != "C" && FType != "T")
                                                    {
                                                        //objStr.Append("<br />");
                                                    }
                                                }
                                            }
                                        }

                                        sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                                        sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                                    }

                                    if (!sbMultipleValue.ToString().Equals(string.Empty))
                                    {
                                        sbTdData.Append(tdFieldValueStart + common.myStr(sbMultipleValue.ToString()) + tdEnd);
                                    }

                                    sBegin = "";
                                    sEnd = "";
                                    dvFieldType.Dispose();
                                    dtFieldType.Dispose();
                                }

                                if (!sbTdData.ToString().Equals(string.Empty))
                                {
                                    sbAllData.Append(trStart + sbTdData.ToString() + trEnd);
                                }
                            }

                            if (!sbAllData.ToString().Equals(string.Empty))
                            {
                                objStr.Append("<table border='0' style='width:99%;" + sFontSize.Replace("font-weight: bold;", string.Empty) + "' cellpadding='2' cellspacing='2'>" +
                                    "<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>" +
                                    sbAllData.ToString() +
                                    "</table>");
                            }

                            //sbCation.Append("");

                            //if (objStr.ToString() != "")
                            //{
                            //    objStr.Append(EndList);
                            //}
                        }
                    }
                    #endregion
                }
                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
                {
                    if ((objStr.ToString() != "" || IsConfidential) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                        }
                        dvValues.Dispose();
                    }
                    else
                    {
                        if ((objStr.ToString() != "" || IsConfidential) && bool.Parse(TabularType))
                        {
                            DataView dvValues = new DataView(objDs.Tables[0]);
                            dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
                            if (dvValues.ToTable().Rows.Count > 0)
                            {
                                objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
                            }
                            dvValues.Dispose();
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
        finally
        {
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            objDs.Dispose();
            dsTabulerTemplate.Dispose();
        }
        objStr = objStr;
        return objStr.ToString();
    }

    //protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
    //    string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    //{
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    StringBuilder objStr = new StringBuilder();
    //    DataView objDv = new DataView();
    //    DataTable objDt = new DataTable();
    //    DataSet dsMain = new DataSet();
    //    StringBuilder objStrTmp = new StringBuilder();
    //    DataSet dsTabulerTemplate = new DataSet();
    //    try
    //    {
    //        if (objDs != null)
    //        {
    //            if (IsConfidential == false)
    //            {
    //                #region Tabular
    //                if (bool.Parse(TabularType) == true)
    //                {
    //                    DataView dvFilter = new DataView(objDs.Tables[0]);
    //                    if (objDs.Tables[0].Rows.Count > 0)
    //                    {
    //                        string sBegin = string.Empty;
    //                        string sEnd = string.Empty;
    //                        dvFilter.Sort = "RowNum ASC";
    //                        if (GroupingDate != "")
    //                        {
    //                            dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
    //                        }
    //                        else
    //                        {
    //                            dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
    //                        }
    //                        DataTable dtNewTable = dvFilter.ToTable();
    //                        if (dtNewTable.Rows.Count > 0)
    //                        {
    //                            DataView dvRowCaption = new DataView(objDs.Tables[0]);
    //                            StringBuilder sbCation = new StringBuilder();
    //                            if (dvRowCaption.ToTable().Rows.Count > 0)
    //                            {
    //                                dvRowCaption.RowFilter = "RowNum>0";
    //                                DataTable dt = dvRowCaption.ToTable();
    //                                dvRowCaption.Dispose();
    //                                if (dt.Rows.Count > 0)
    //                                {
    //                                    sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellspacing='3' ><tr align='center'>");
    //                                    DataView dvColumnCount = new DataView(objDs.Tables[1]);
    //                                    dvColumnCount.RowFilter = "SectionId=" + sectionId;

    //                                    int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
    //                                    int ColumnCount = 0;
    //                                    int count = 1;
    //                                    dvColumnCount.Dispose();
    //                                    for (int k = 0; k < column; k++)
    //                                    {
    //                                        sbCation.Append("<td>");
    //                                        sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
    //                                        sbCation.Append("</td>");
    //                                        count++;
    //                                        ColumnCount++;
    //                                    }
    //                                    sbCation.Append("</tr>");

    //                                    DataView dvData = new DataView(dt);
    //                                    if (GroupingDate != "")
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";
    //                                    }
    //                                    else
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId;
    //                                    }

    //                                    for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
    //                                    {
    //                                        sbCation.Append("<tr>");
    //                                        for (int i = 1; i < ColumnCount + 1; i++)
    //                                        {
    //                                            if (dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                            {
    //                                                if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                {
    //                                                    sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
    //                                                }
    //                                                else
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                }
    //                                            }
    //                                            else
    //                                            {
    //                                                if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
    //                                                }
    //                                                else
    //                                                {
    //                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                }
    //                                            }
    //                                        }
    //                                        sbCation.Append("</tr>");
    //                                    }
    //                                    dt.Dispose();
    //                                    dvData.Dispose();
    //                                }
    //                                sbCation.Append("</table>");
    //                            }
    //                            objStr.Append(sbCation);
    //                            dsTabulerTemplate.Dispose();
    //                            sbCation = null;

    //                        }
    //                        else
    //                        {
    //                            DataView dvRowCaption = new DataView(objDs.Tables[0]);
    //                            if (GroupingDate != "")
    //                            {
    //                                dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
    //                            }
    //                            else
    //                            {
    //                                dvRowCaption.RowFilter = "RecordId= " + RecordId;
    //                            }
    //                            if (dvRowCaption.ToTable().Rows.Count > 0)
    //                            {
    //                                StringBuilder sbCation = new StringBuilder();
    //                                dvRowCaption.RowFilter = "RowNum>0";
    //                                DataTable dt = dvRowCaption.ToTable();
    //                                // dvRowCaption.Dispose();
    //                                if (dt.Rows.Count > 0)
    //                                {
    //                                    sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
    //                                    DataView dvColumnCount = new DataView(objDs.Tables[1]);
    //                                    dvColumnCount.RowFilter = "SectionId=" + sectionId;

    //                                    int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
    //                                    int ColumnCount = 0;
    //                                    int count = 1;
    //                                    dvColumnCount.Dispose();

    //                                    for (int k = 0; k < column + 1; k++)
    //                                    {
    //                                        if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
    //                                            && ColumnCount == 0)
    //                                        {
    //                                            sbCation.Append("<td>");
    //                                            sbCation.Append(" + ");
    //                                            sbCation.Append("</td>");
    //                                        }
    //                                        else
    //                                        {
    //                                            sbCation.Append("<td>");
    //                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
    //                                            sbCation.Append("</td>");
    //                                            count++;
    //                                        }
    //                                        ColumnCount++;
    //                                    }
    //                                    sbCation.Append("</tr>");

    //                                    DataView dvData = new DataView(dt);
    //                                    if (GroupingDate != "")
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
    //                                    }
    //                                    else
    //                                    {
    //                                        dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
    //                                    }

    //                                    for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
    //                                    {
    //                                        sbCation.Append("<tr>");
    //                                        for (int i = 0; i < ColumnCount; i++)
    //                                        {
    //                                            if (i == 0)
    //                                            {
    //                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
    //                                            }
    //                                            else
    //                                            {
    //                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
    //                                                {
    //                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                    {
    //                                                        sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                                else
    //                                                {
    //                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
    //                                                    }
    //                                                    else
    //                                                    {
    //                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                        sbCation.Append("</tr>");
    //                                    }
    //                                    sbCation.Append("</table>");
    //                                    dvData.Dispose();
    //                                }
    //                                objStr.Append(sbCation);
    //                                dt.Dispose();
    //                                sbCation = null;
    //                            }
    //                        }
    //                    }
    //                }
    //                #endregion
    //                #region Non Tabular
    //                else // For Non Tabular Templates
    //                {
    //                    string BeginList = "", EndList = "";
    //                    string sBegin = "", sEnd = "";
    //                    int t = 0;
    //                    string FieldId = "";
    //                    string sStaticTemplate = "";
    //                    string sEnterBy = "";
    //                    string sVisitDate = "";
    //                    foreach (DataRow item in objDs.Tables[0].Rows)
    //                    {
    //                        objDv = new DataView(objDs.Tables[1]);
    //                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
    //                        objDt = objDv.ToTable();
    //                        if (t == 0)
    //                        {
    //                            t = 1;
    //                            if (common.myStr(item["FieldsListStyle"]) == "1")
    //                            {
    //                                BeginList = "<ul>"; EndList = "</ul>";
    //                            }
    //                            else if (item["FieldsListStyle"].ToString() == "2")
    //                            {
    //                                BeginList = "<ol>"; EndList = "</ol>";
    //                            }
    //                        }
    //                        if (common.myStr(item["FieldsBold"]) != ""
    //                            || common.myStr(item["FieldsItalic"]) != ""
    //                            || common.myStr(item["FieldsUnderline"]) != ""
    //                            || common.myStr(item["FieldsFontSize"]) != ""
    //                            || common.myStr(item["FieldsForecolor"]) != ""
    //                            || common.myStr(item["FieldsListStyle"]) != "")
    //                        {
    //                            //rafat1
    //                            if (objDt.Rows.Count > 0)
    //                            {
    //                                sBegin = "";
    //                                sEnd = "";

    //                                MakeFont("Fields", ref sBegin, ref sEnd, item);
    //                                if (Convert.ToBoolean(item["DisplayTitle"]))
    //                                {
    //                                    // if (EntryType != "M")
    //                                    // {


    //                                    ////if (sBegin.StartsWith("<br/>"))
    //                                    ////{
    //                                    ////    if (sBegin.Length > 5)
    //                                    ////    {
    //                                    ////        sBegin = sBegin.Substring(5, sBegin.Length - 5);

    //                                    ////    }
    //                                    ////}

    //                                    objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
    //                                    //}
    //                                    //else
    //                                    //{
    //                                    //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
    //                                    //}
    //                                    // 28/08/2011
    //                                    //if (objDt.Rows.Count > 0)
    //                                    //{
    //                                    if (objStr.ToString() != "")
    //                                    {
    //                                        objStr.Append(sEnd + "</li>");
    //                                    }
    //                                    ViewState["sBegin"] = sBegin;
    //                                }

    //                                BeginList = "";
    //                                sBegin = "";
    //                                sEnd = "";

    //                            }

    //                        }
    //                        else
    //                        {
    //                            if (objDt.Rows.Count > 0)
    //                            {
    //                                if (sStaticTemplate != "<br/><br/>")
    //                                {
    //                                    objStr.Append(common.myStr(item["FieldName"]));
    //                                }
    //                            }
    //                        }
    //                        if (objDs.Tables.Count > 1)
    //                        {

    //                            objDv = new DataView(objDs.Tables[1]);
    //                            objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
    //                            objDt = objDv.ToTable();
    //                            DataView dvFieldType = new DataView(objDs.Tables[0]);
    //                            dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
    //                            DataTable dtFieldType = dvFieldType.ToTable("FieldType");
    //                            sBegin = "";
    //                            sEnd = "";

    //                            string sbeginTemp = string.Empty;
    //                            MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
    //                            // MakeFont("Fields", ref sBegin, ref sEnd, item);
    //                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
    //                            {
    //                                if (objDt.Rows.Count > 0)
    //                                {

    //                                    sbeginTemp = common.myStr(ViewState["sBegin"]);
    //                                    if (sbeginTemp.StartsWith("<br/>"))
    //                                    {
    //                                        if (sbeginTemp.Length > 5)
    //                                        {
    //                                            sbeginTemp = sbeginTemp.Substring(0, 5);

    //                                            //objStrTmp.Append(sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                        }
    //                                    }



    //                                    string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
    //                                    if (FType == "C")
    //                                    {
    //                                        FType = "C";
    //                                    }
    //                                    if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
    //                                    {
    //                                        if (FType == "B")
    //                                        {

    //                                            objStr.Append(" : " + objDt.Rows[i]["TextValue"]);
    //                                            //objStr.Append("  " + objDt.Rows[i]["TextValue"]);
    //                                        }
    //                                        else
    //                                        {
    //                                            //////BindDataValue(objDs, objDt, objStr, i, FType) //comeented by niraj , create and added below overloading methd
    //                                            BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
    //                                        }
    //                                    }
    //                                    else if (FType == "T" || FType == "M" || FType == "S" || FType == "ST" || FType == "SB" || FType == "W")
    //                                    {
    //                                        if (common.myStr(ViewState["iTemplateId"]) != "163")
    //                                        {
    //                                            if (i == 0)
    //                                            {
    //                                                if (FType == "M" || FType == "W")
    //                                                {

    //                                                    objStr.Append(sBegin + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                }
    //                                                else
    //                                                {
    //                                                    objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                }


    //                                            }
    //                                            else
    //                                            {
    //                                                objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                //if (FType == "M" || FType == "W")
    //                                                //{
    //                                                //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //                                                //}
    //                                                //else
    //                                                //{
    //                                                //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

    //                                                //}

    //                                            }
    //                                        }
    //                                        else
    //                                        {
    //                                            if (i == 0)
    //                                            {
    //                                                objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                            }
    //                                            else
    //                                            {
    //                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
    //                                            }
    //                                        }
    //                                    }
    //                                    else if (FType == "L")
    //                                    {
    //                                        objStr.Append(BindStaticTemplates(Convert.ToInt32(objDt.Rows[0]["StaticTemplateId"]), Convert.ToInt32(objDt.Rows[0]["FieldId"])));
    //                                    }
    //                                    else if (FType == "IM")
    //                                    {
    //                                        objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
    //                                    }
    //                                    if (common.myStr(item["FieldsListStyle"]) == "")
    //                                    {
    //                                        if (ViewState["iTemplateId"].ToString() != "163")
    //                                        {
    //                                            if (FType != "C")
    //                                            {

    //                                                if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
    //                                                {

    //                                                }
    //                                                else
    //                                                {
    //                                                    objStr.Append("<br />");

    //                                                }

    //                                            }

    //                                        }
    //                                        else
    //                                        {
    //                                            if (FType != "C" && FType != "T")
    //                                            {
    //                                                objStr.Append("<br />");
    //                                            }
    //                                        }
    //                                    }





    //                                }
    //                                sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
    //                                sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
    //                                //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
    //                                //{
    //                                //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
    //                                //}
    //                            }
    //                            sBegin = "";
    //                            sEnd = "";
    //                            dvFieldType.Dispose();
    //                            dtFieldType.Dispose();

    //                            // Cmt 25/08/2011
    //                            //if (objDt.Rows.Count > 0)
    //                            //{
    //                            //    if (objStr.ToString() != "")
    //                            //        objStr.Append(sEnd + "</li>");
    //                            //}
    //                        }

    //                        //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
    //                    }

    //                    if (objStr.ToString() != "")
    //                    {
    //                        objStr.Append(EndList);
    //                    }
    //                }
    //                #endregion
    //            }
    //            string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
    //            if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
    //            {
    //                if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
    //                {
    //                    DataView dvValues = new DataView(objDs.Tables[1]);
    //                    dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
    //                    if (dvValues.ToTable().Rows.Count > 0)
    //                    {
    //                        objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
    //                    }
    //                    dvValues.Dispose();
    //                }
    //                else
    //                {
    //                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == true)
    //                    {
    //                        DataView dvValues = new DataView(objDs.Tables[0]);
    //                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
    //                        if (dvValues.ToTable().Rows.Count > 0)
    //                        {
    //                            objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
    //                        }
    //                        dvValues.Dispose();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //    finally
    //    {
    //        objDv.Dispose();
    //        objDt.Dispose();
    //        dsMain.Dispose();
    //        objDs.Dispose();
    //        dsTabulerTemplate.Dispose();
    //    }
    //    return objStr.ToString();
    //}
    protected void MakeFontWithoutBR(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            //if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            //{
            //    sBegin += "<br/>";
            //}
            //else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            //{
            //    sBegin += "; ";
            //}
            //else
            //{
            //    sBegin += "<br/>";
            //}
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }


    private string BindEditor(bool sign)
    {
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet ds = new DataSet();
        DataSet dsTemplate = new DataSet();
        DataTable dtTemplate = new DataTable();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        Hashtable hst = new Hashtable();
        string Templinespace = string.Empty;
        //BaseC.DiagnosisDA fun;

        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        Int16 UserId = Convert.ToInt16(Session["UserID"]);
        DL_Funs ff = new DL_Funs();
        BindNotes bnotes = new BindNotes(sConString);

        //fun = new BaseC.DiagnosisDA(sConString);
        string DoctorId = common.myStr(ViewState["DoctorId"]);//fun.GetDoctorId(HospitalId, UserId);
        string FormId = "0";
        if (!common.myInt(Session["formId"]).Equals(0))
        {
            FormId = common.myInt(Session["formId"]).ToString();
            hst.Add("@intFormId", common.myStr(1));
        }
        if (!common.myInt(Session["HospitalLocationID"]).Equals(0))
        {
            hst.Add("@intEncounterId", EncounterId);
            hst.Add("@inyModuleID", 3);
            hst.Add("@intGroupId", common.myInt(Session["GroupId"]));
        }
        string sql = "SELECT PatientSummary, StatusId FROM EMRPatientForms WITH (NOLOCK) WHERE EncounterId = @intEncounterId AND Active = 1 ";

        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        ds = dl.FillDataSet(CommandType.Text, sql, hst);

        Saved_RTF_Content += "";

        string fieldT = common.myStr(ddlReport.SelectedItem.Attributes["FieldType"]).Trim();

        if (fieldT.Equals("W") && common.myInt(ddlReport.SelectedValue) > 0)
        {
            clsIVF objivf = new clsIVF(sConString);

            DataSet dsW = new DataSet();
            dsW = objivf.getEMRTemplateWordData(common.myInt(ddlReport.SelectedValue), common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["RegistrationId"]));

            if (dsW.Tables[0].Rows.Count > 0)
            {
                Saved_RTF_Content += common.myStr(dsW.Tables[0].Rows[0]["ValueWordProcessor"]);
            }

            return Saved_RTF_Content;
        }
        clsIVF note = new clsIVF(sConString);
        dsTemplate = note.getEMRTemplateReportSequence(common.myInt(ddlReport.SelectedValue));

        hst = new Hashtable();

        hst.Add("@inyHospitalLocationID", HospitalId);
        sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
        + "TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
        + "SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
        + "FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber "
        + "FROM EMRTemplateStatic WITH (NOLOCK) WHERE (HospitalLocationId = @inyHospitalLocationID or HospitalLocationId is null)";

        dsTemplateStyle = dl.FillDataSet(CommandType.Text, sql, hst);
        //Making Sequence as, Chief Complaint on Top and rest following as per sequence number.

        dtTemplate = dsTemplate.Tables[0];

        if (Saved_RTF_Content.Equals(string.Empty))
            sb.Append("<span style='" + Fonts + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            foreach (DataRow dr in dtTemplate.Rows)
            {
                string strTemplateType = common.myStr(dr["PageIdentification"]);
                //strTemplateType = strTemplateType.Substring(0, 1);
                strTemplateType = getSubString(common.myStr(strTemplateType), 0, 1);

                if (common.myStr(dr["PageName"]).Trim() == "Vitals")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);

                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = string.Empty, sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");
                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("VTL", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(), 0, "0");
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("VTL", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (common.myStr(dr["PageName"]).Trim() == "LAB History")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");

                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("LAB", Saved_RTF_Content, sbTemp, lck.ToString());

                    Saved_RTF_Content += "<div id=\"LAB\"></div>";
                    bnotes.BindLabTestResultReport(common.myInt(ViewState["RegistrationId"]), HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, false);

                    //comment for demo
                    //bnotes.BindInvestigationResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dr["PageId"]), common.myStr(Session["UserID"]));

                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("LAB", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, dr["ShowNote"].ToString());
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (dr["PageName"].ToString().Trim() == "Chief Complaints")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");

                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("Chf", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString());
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("Chf", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (common.myStr(dr["PageName"]).Trim() == "Diagnosis")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");

                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("Dia", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindAssessments(RegId, HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(), 0, "0");
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("Dia", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (common.myStr(dr["PageName"]).Trim() == "Allergies")
                {
                    int lck = 0;

                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");

                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("All", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(), common.myInt(Session["UserID"]).ToString(), common.myStr(dr["PageID"]), 0);
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("All", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (common.myStr(dr["PageName"]).Trim() == "Prescription")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    //dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");
                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("Med", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                        "", "", common.myStr(Session["OPIP"]), "");
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("Med", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (common.myStr(dr["PageName"]).Trim() == "Current Medication")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");
                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("Cur", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(),
                        "", "", common.myStr(Session["OPIP"]), "");
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("Cur", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }

                else if (common.myStr(dr["PageName"]).Trim() == "Orders And Procedures")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["encounterid"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");
                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("Ord", Saved_RTF_Content, sbTemp, lck.ToString());
                    bnotes.BindOrders(common.myInt(ViewState["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString(), "0");
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("Ord", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                else if (common.myStr(dr["PageName"]).Trim() == "Immunization Chart")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]) + sEnd);
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["PageName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["EncounterId"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");
                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("Imm", Saved_RTF_Content, sbTemp, lck.ToString());
                    // string sdob = common.myStr(Session["DOB"]);
                    bnotes.BindImmunization(HospitalId.ToString(), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(dr["PageId"]), common.myInt(Session["UserID"]).ToString());
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("Imm", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    dr["PageName"] = null;
                    Templinespace = "";
                }

                else if ((common.myStr(dr["PageName"]).Trim() == "ROS" || common.myStr(dr["TemplateType"]).Trim() == "ROS"))
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    //lck = ff.DoLockUnLock("Sel", Session["EncounterId"].ToString(), Session["HospitalLocationId"].ToString(), Session["formId"].ToString(), dr["PageId"].ToString(), "");
                    StringBuilder sbTemp = new StringBuilder();
                    //AddStr1("ROS", Saved_RTF_Content, sbTemp, lck.ToString());
                    AddStr1("D" + common.myStr(dr["PageId"]), Saved_RTF_Content, sbTemp, lck.ToString());
                    BindProblemsROS(HospitalId, EncounterId, sbTemp, common.myStr(dr["DisplayName"]).Trim(), common.myStr(dr["PageName"]).Trim(), common.myStr(dr["PageId"]));
                    //AddStr2("ROS", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace);
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("D" + common.myStr(dr["PageId"]), ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    // AddStr2("ROS", ref Saved_RTF_Content, sbTemp, lck.ToString(), "-1");// passing to Templinespace for tabular
                    Templinespace = "";
                }
                else if (common.myStr(dr["SectionName"]).Trim() != "")
                {
                    int lck = 0;
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dr["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                        if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
                        {
                            if (dr["DisplayName"] != null)
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
                            }
                            else
                            {
                                sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
                            }
                        }
                    }
                    else
                    {
                        Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
                        //sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
                    }
                    lck = common.myInt(dr["Lock"]);
                    StringBuilder sbTemp = new StringBuilder();
                    AddStr1("D" + common.myStr(dr["TemplateId"]), Saved_RTF_Content, sbTemp, lck.ToString());
                    bindData(FormId, common.myStr(dr["TemplateId"]), common.myStr(dr["SectionId"]), sbTemp);
                    if (common.myStr(sbTemp).Length > 46)
                    {
                        AddStr2("D" + common.myStr(dr["PageId"]), ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
                    }
                    Templinespace = "";
                }
            }

            sb.Append("</span>");

            if (sign == true)
            {
                if (Saved_RTF_Content != null)
                {
                    if (Saved_RTF_Content.Contains("dvDoctorImage") != true)
                    {
                        if (Saved_RTF_Content != "")
                        {
                            Saved_RTF_Content += hdnDoctorImage.Value;
                        }
                        else
                        {
                            sb.Append(hdnDoctorImage.Value);
                        }
                    }
                }
            }
            else if (sign == false)
            {
                if (Saved_RTF_Content != null)
                {
                    if (Saved_RTF_Content.Contains("dvDoctorImage") == true)
                    {
                        Saved_RTF_Content = Saved_RTF_Content.Replace('"', '$');
                        string st = "<div id=$dvDoctorImage$>";
                        int start = Saved_RTF_Content.IndexOf(@st);
                        //int end = Saved_RTF_Content.IndexOf("</div>", start);
                        //if (start != -1)
                        //{
                        //    Saved_RTF_Content = Saved_RTF_Content.Remove(start);
                        //    Saved_RTF_Content = Saved_RTF_Content + "</div>";
                        //}
                        if (start > 0)
                        {
                            int End = Saved_RTF_Content.IndexOf("</div>", start);
                            StringBuilder sbte = new StringBuilder();
                            //sbte.Append(Saved_RTF_Content.Substring(start, (End + 6) - start));

                            sbte.Append(getSubString(common.myStr(Saved_RTF_Content), start, (End + 6) - start));

                            StringBuilder ne = new StringBuilder();
                            ne.Append(Saved_RTF_Content.Replace(sbte.ToString(), ""));
                            Saved_RTF_Content = ne.Replace('$', '"').ToString();
                        }
                    }

                }
            }

            if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
                return sb.ToString();
            else
                return Saved_RTF_Content;
            //    }                

        }
        //return "";

        return "";
    }
    public StringBuilder BindProblemsROS(int HospitalId, int EncounterId, StringBuilder sb, String sDisplayName, String sTemplateName, string pageID)
    {
        DataSet ds;
        DataSet dsGender;
        string strGender = "He";
        Hashtable hstInput = new Hashtable();
        Hashtable hsGender = new Hashtable();
        Hashtable hsProblems = new Hashtable();
        DataTable dtPositiveRos = new DataTable();
        DataTable dtNegativeRos = new DataTable();
        objStrTmp = new StringBuilder();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //rafat
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        //hstInput.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hstInput.Add("@intTemplateId", common.myInt(ViewState["PageId"]));
        hstInput.Add("chrGenderType", getGenderValue(common.myInt(Session["Gender"])));
        hstInput.Add("@intFormId", common.myStr(1));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        //string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
        string SqlQry = " SELECT dbo.GetGender(GENDER) AS 'Gender' FROM registration WITH (NOLOCK) WHERE Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]).ToUpper().Equals("MALE"))
            {
                strGender = "He";
            }
            else
            {
                strGender = "She";
            }
        }
        //Review Of Systems

        hsProblems.Add("@intEncounterId", EncounterId);
        //hsProblems.Add("@intTemplateId", GetTemplateId("ROS", common.myInt(Session["HospitalLocationID"])));
        hsProblems.Add("@intTemplateId", common.myInt(ViewState["PageId"]));
        ds = new DataSet();
        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", hsProblems);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv1 = new DataView(ds.Tables[0]);
            dv1.RowFilter = "PositiveValue <> ''";
            dtPositiveRos = dv1.ToTable();

            DataView dv2 = new DataView(ds.Tables[0]);
            dv2.RowFilter = "NegativeValue <> ''";
            dtNegativeRos = dv2.ToTable();
            //Make font start

            if (common.myStr(drFont["TemplateBold"]) != "" || common.myStr(drFont["TemplateItalic"]) != "" || common.myStr(drFont["TemplateUnderline"]) != "" || common.myStr(drFont["TemplateFontSize"]) != "" || common.myStr(drFont["TemplateForecolor"]) != "" || common.myStr(drFont["TemplateListStyle"]) != "")
            {
                string sBegin = "", sEnd = "";
                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                if (common.myBool(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                    objStrTmp.Append(sBegin + common.myStr(sDisplayName) + sEnd);
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
            }
            else
            {
                if (common.myBool(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                    objStrTmp.Append(common.myStr(sDisplayName));//Default Setting
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
            }

            // Make Font End

            //sb.Append("<u><Strong>Review of systems</Strong></u>");

        }

        // For Positive Symptoms
        if (dtPositiveRos.Rows.Count > 0)
        {
            string strSectionId = ""; // dtPositiveRos.Rows[0]["SectionId"].ToString();
            DataTable dt = new DataTable();
            for (int i = 0; i < dtPositiveRos.Rows.Count; i++)
            {

                DataRow dr = dtPositiveRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != "" || common.myStr(drFont["SectionsItalic"]) != "" || common.myStr(drFont["SectionsUnderline"]) != "" || common.myStr(drFont["SectionsFontSize"]) != "" || common.myStr(drFont["SectionsForecolor"]) != "" || common.myStr(drFont["SectionsListStyle"]) != "")
                    {
                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (common.myBool(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Positive Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != "" || common.myStr(dr["FieldsItalic"]) != "" || common.myStr(dr["FieldsUnderline"]) != "" || common.myStr(dr["FieldsFontSize"]) != "" || common.myStr(dr["FieldsForecolor"]) != "" || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " has ");
                    }
                    else
                        objStrTmp.Append(strGender + " has ");

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " has ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" and " + common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }

        // For Negative Symptoms
        if (dtNegativeRos.Rows.Count > 0)
        {
            //if (drFont["TemplateBold"].ToString() != "" || drFont["TemplateItalic"].ToString() != "" || drFont["TemplateUnderline"].ToString() != "" || drFont["TemplateFontSize"].ToString() != "" || drFont["TemplateForecolor"].ToString() != "" || drFont["TemplateListStyle"].ToString() != "")
            //{
            //    string sBegin = "", sEnd = "";
            //    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
            //    //objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
            //    //objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}
            //else
            //{
            //    objStrTmp.Append("<br /><br /><Strong>Negative Symptoms:</Strong>");
            //}          
            string strSectionId = ""; // 
            DataTable dt = new DataTable();
            for (int i = 0; i < dtNegativeRos.Rows.Count; i++)
            {

                DataRow dr = dtNegativeRos.Rows[i] as DataRow;
                if (common.myStr(dr["SectionId"]) != strSectionId)
                {
                    string sBegin = "", sEnd = "";
                    if (common.myStr(drFont["SectionsBold"]) != "" || common.myStr(drFont["SectionsItalic"]) != "" || common.myStr(drFont["SectionsUnderline"]) != "" || common.myStr(drFont["SectionsFontSize"]) != "" || common.myStr(drFont["SectionsForecolor"]) != "" || common.myStr(drFont["SectionsListStyle"]) != "")
                    {

                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (common.myBool(drFont["SectionDisplayTitle"]))   //19June2010
                        {
                            if (i == 0)
                            {
                                objStrTmp.Append("<br /><br />" + sBegin + "Negative Symptoms:" + sEnd);
                            }
                            objStrTmp.Append("<br />" + sBegin + common.myStr(dr["SectionName"]).Trim() + ": " + sEnd);
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            objStrTmp.Append("<br />" + "Negative Symptoms:");
                        }
                        objStrTmp.Append("<br />" + common.myStr(dr["SectionName"]).Trim() + ": ");
                    }


                    if (common.myStr(dr["FieldsBold"]) != "" || common.myStr(dr["FieldsItalic"]) != "" || common.myStr(dr["FieldsUnderline"]) != "" || common.myStr(dr["FieldsFontSize"]) != "" || common.myStr(dr["FieldsForecolor"]) != "" || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " does not have ");
                    }
                    else
                        objStrTmp.Append(strGender + " does not have ");



                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " does not have ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtNegativeRos);
                    dv.RowFilter = "SectionId =" + common.myInt(dr["SectionId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {

                        if (j == dt.Rows.Count - 1)
                        {
                            if (dt.Rows.Count == 1)
                            {
                                objStrTmp.Append("" + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                objStrTmp.Remove(objStrTmp.Length - 2, 2);
                                objStrTmp.Append(" or " + common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ". ");
                            }
                        }
                        else
                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }
        sb.Append(objStrTmp);
        //sb.Append("<br/>");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Hashtable hshtable = new Hashtable();
            StringBuilder sbDisplayName = new StringBuilder();
            BaseC.Patient bc = new BaseC.Patient(sConString);
            hshtable.Add("@intTemplateId", pageID);
            hshtable.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
            string strDisplayUserName = "SELECT DisplayUserName FROM EMRTemplate WITH (NOLOCK) WHERE ID=@intTemplateId AND HospitalLocationID=@inyHospitalLocationID";
            DataSet dsDisplayName = DlObj.FillDataSet(CommandType.Text, strDisplayUserName, hshtable);
            if (dsDisplayName.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsDisplayName.Tables[0].Rows[0]["DisplayUserName"]))
                {
                    Hashtable hshUser = new Hashtable();
                    hshUser.Add("@UserID", common.myInt(ds.Tables[0].Rows[0]["EncodedBy"]));
                    hshUser.Add("@inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                    string strUser = "SELECT ISNULL(FirstName,'') +  ISNULL(' ' + MiddleName,'') +  ISNULL(' ' + LastName,'') AS EmployeeName FROM Employee em WITH (NOLOCK) INNER JOIN Users us WITH (NOLOCK) ON em.ID=us.EmpID WHERE us.ID=@UserID AND em.HospitalLocationId=@inyHospitalLocationID";

                    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    DataSet dsUser = dl.FillDataSet(CommandType.Text, strUser, hshUser);
                    DataTable dt = dsUser.Tables[0];
                    DataRow dr = dt.Rows[0];
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        sb.Append("<br/>");
                        string sUBegin = "", sUEnd = "";
                        MakeFontWithoutListStyle("Sections", ref sUBegin, ref sUEnd, drFont);
                        sbDisplayName.Append(sUBegin + "Entered and Verified by " + common.myStr(dsUser.Tables[0].Rows[0]["EmployeeName"]) + " " + common.myStr(Convert.ToDateTime(ds.Tables[0].Rows[0]["EncodedDate"]).Date.ToString("MMMM dd yyyy")));
                    }
                    sb.Append(sbDisplayName);
                }
            }
        }
        return sb;
    }
    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        int RegId = common.myInt(ViewState["RegistrationId"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(ViewState["EncounterId"]);
        int UserId = common.myInt(Session["UserID"]);

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, "0");
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + Fonts + "'>");

        if (dtTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                //strTemplateType = strTemplateType.Substring(0, 1);
                strTemplateType = getSubString(common.myStr(strTemplateType), 0, 1);

                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                drTemplateStyle = null;// = dv[0].Row;
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
                            common.myInt(Session["UserID"]).ToString(), common.myStr(dtTemplate.Rows[0]["PageID"]), "", "", TemplateFieldId, "");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                //strTemplateType = strTemplateType.Substring(0, 1);
                strTemplateType = getSubString(common.myStr(strTemplateType), 0, 1);

                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbStatic, sbTemplateStyle, drTemplateStyle,
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                                    "", "", TemplateFieldId, "0", "");

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                //strTemplateType = strTemplateType.Substring(0, 1);
                strTemplateType = getSubString(common.myStr(strTemplateType), 0, 1);

                sbTemplateStyle = new StringBuilder();
                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[0]["PageId"]);
                if (dv.Count > 0)
                {
                    drTemplateStyle = dv[0].Row;
                    string sBegin = "", sEnd = "";
                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                }
                StringBuilder sbTemp = new StringBuilder();


                bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                            DoctorId, sbStatic, sbTemplateStyle, drTemplateStyle, Page,
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myInt(Session["UserID"]).ToString(),
                            "",
                            "", TemplateFieldId, "0", "");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();

        clsIVF objivf = new clsIVF(sConString);

        ds = objivf.getIVFPatient(common.myInt(ViewState["RegistrationId"]), 0);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "RegistrationId=" + common.myInt(ViewState["RegistrationId"]);

            DataTable tbl = DV.ToTable();

            if (tbl.Rows.Count > 0)
            {
                DataRow DR = tbl.Rows[0];

                DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
                DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(ViewState["RegistrationId"]);
                DataTable tblSpouse = DVSpouse.ToTable();

                sb.Append("<div><table border='0' width='100%' style='font-size:smaller; border-collapse:collapse;' cellpadding='2' cellspacing='3' ><tr valign='top'>");
                sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td>: " + common.myStr(DR["RegistrationNo"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "PatientName")) + "</td><td>: " + common.myStr(DR["PatientName"]) + "</td>");
                sb.Append("<td style='width: 109px;'>Age/Gender</td><td>: " + common.myStr(DR["Age/Gender"]) + "</td>");
                sb.Append("</tr>");

                if (tblSpouse.Rows.Count > 0)
                {
                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>Spouse</td><td>: " + common.myStr(tblSpouse.Rows[0]["PatientName"]) + "</td>");
                    sb.Append("<td>Spouse Age/Gender</td><td>: " + common.myStr(tblSpouse.Rows[0]["Age/Gender"]) + "</td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr valign='top'>");
                sb.Append("<td>Visit Date</td><td>: " + common.myStr(DR["RegistrationDate"]) + "</td>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "mobile")) + "</td><td>: " + common.myStr(DR["MobileNo"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "Address")) + "</td><td>: " + common.myStr(DR["PatientAddress"]) + "</td>");

                sb.Append("</tr>");

                sb.Append("</table></div>");
            }

            sb.Append("<hr />");

        }
        return sb;
    }
    protected void BindImageWithControl(object ImageData)
    {
        try
        {
            Stream strm;
            Object img = ImageData;
            String FileName = "BioChem.bmp";
            strm = new MemoryStream((byte[])img);
            byte[] buffer = new byte[strm.Length];
            int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
            FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);
            fs.Write(buffer, 0, byteSeq);
            fs.Dispose();
        }
        catch (Exception Ex)
        {

            //lblmassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblmassage.Text = "Error: " + Ex.Message;
            Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
            objException.HandleException(Ex);
        }
    }
    protected string getFontName(string id, string FtSize)
    {
        string sBegin = "";
        string sFontSize = "";
        if (id == "1")
            sBegin += " font-family:Arial ";
        if (id == "2")
            sBegin += " font-family:Courier New ";
        if (id == "3")
            sBegin += " font-family:Garamond ";
        if (id == "4")
            sBegin += " font-family:Georgia ";
        if (id == "5")
            sBegin += " font-family:MS Sans Serif ";
        if (id == "6")
            sBegin += " font-family:Segoe UI";
        if (id == "7")
            sBegin += " font-family:Tahoma ";
        if (id == "8")
            sBegin += " font-family:Times New Roman ";
        if (id == "9")
            sBegin += " font-family:Verdana ";


        if (FtSize == "9")
            sFontSize += " ; font-size:9pt ";
        if (FtSize == "11")
            sFontSize += " ; font-size:11pt ";
        if (FtSize == "12")
            sFontSize += " ; font-size:12pt ";
        if (FtSize == "14")
            sFontSize += " ; font-size:14pt ";
        if (FtSize == "18")
            sFontSize += " ; font-size:18pt ";
        if (FtSize == "20")
            sFontSize += " ; font-size:20pt ";
        if (FtSize == "24")
            sFontSize += " ; font-size:24pt ";
        if (FtSize == "26")
            sFontSize += " ; font-size:26pt ";
        if (FtSize == "36")
            sFontSize += " ; font-size:36pt ";

        return sBegin + sFontSize;
    }
    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sBegin += GetFontFamily(typ, item); }
        }

        if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
        { sBegin += " font-weight: bold;"; }
        if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
        { sBegin += " font-style: italic;"; }
        if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
        { sBegin += " text-decoration: underline;"; }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
            sBegin += " '>";
    }

    protected void AddStr1(string type, string Saved_RTF_Content, StringBuilder sbTemp, string Lock)
    {
        //sbTemp.Append("<div id='" + type + "'><span style='color: Blue;'>");
        sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
        //if (Lock == "0")
        //    sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");            
        //else
        //    sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
    }

    protected void AddStr2(string type, ref string Saved_RTF_Content, StringBuilder sbTemp, string Lock, string Linespace, string ShowNote)
    {
        sbTemp.Append("</span></div>");
        if (common.myStr(sbTemp).Length > 49)
        {
            if (Linespace != "")
            {
                int ls = common.myInt(Linespace);
                for (int i = 1; i <= ls; i++)
                {
                    sbTemp.Append("<br/>");
                }
            }
            else
            {
                sbTemp.Append("<br />");
            }
        }
        if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
        {
            if (common.myStr(sbTemp).Length > 62)  //if (sbTemp.ToString().Length > 68)
                sb.Append(common.myStr(sbTemp));
        }
        else
        {
            //change
            Saved_RTF_Content += sbTemp.ToString();

            //if (sbTemp.ToString().Length > 62)//if (sbTemp.ToString().Length > 68)
            //{
            if (ShowNote == "True" && (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null))
            {
                Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
            }
            else if (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null)
            {
                Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
            }
            else if (common.myStr(ViewState["DefaultTemplate"]) != "")
            {
                if (common.myStr(ViewState["DefaultTemplate"]).ToUpper() == "TRUE")
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
            }
            else if (common.myStr(ViewState["PullForward"]) != "")
            {
                if (common.myStr(ViewState["PullForward"]).ToUpper() == "TRUE")
                {
                    Replace(type, ref Saved_RTF_Content, sbTemp.ToString(), Lock);
                }
            }
        }

    }
    protected void bindData(string iFormId, string TemplateId, string SectionId, StringBuilder sb)
    {
        string str = string.Empty;
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);
        hstInput.Add("chrGenderType", getGenderValue(common.myInt(Session["Gender"])));
        hstInput.Add("@intFormId", 1);
        if (common.myInt(ddlReport.SelectedValue) != 0)
        {
            hstInput.Add("@intReportId", common.myInt(ddlReport.SelectedValue));
        }

        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);

        DataView dvrd = ds.Tables[0].Copy().DefaultView;
        dvrd.RowFilter = "SectionId IN (" + SectionId + ")";

        dvrd.Sort = "Hierarchy,SequenceNo";

        ds.Tables.RemoveAt(0);
        ds.Tables.Add(dvrd.ToTable());


        DataSet dsAllSectionDetails = new DataSet();
        if (common.myInt(ddlReport.SelectedValue) != 0 && SectionId != "" && ds.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DataSet dsGetData = new DataSet();
                hstInput = new Hashtable();
                hstInput.Add("@intTemplateId", dr["TemplateId"]);
                hstInput.Add("@intSectionID", common.myInt(SectionId));
                hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
                hstInput.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
                hstInput.Add("chrGenderType", getGenderValue(common.myInt(Session["Gender"])));
                hstInput.Add("@intRecordId", common.myInt(ddlRecord.SelectedValue));
                hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));

                dsGetData = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

                if (dsGetData.Tables.Count > 0)
                {
                    if (dsGetData.Tables[0].Rows.Count > 0)
                    {
                        dsAllSectionDetails.Merge(dsGetData);
                    }
                }
            }
        }
        else
        {
            hstInput = new Hashtable();
            hstInput.Add("@intTemplateId", TemplateId);
            hstInput.Add("@intEncounterId", common.myInt(ViewState["EncounterId"]));
            hstInput.Add("@intRegistrationId", common.myInt(ViewState["RegistrationId"]));
            hstInput.Add("chrGenderType", getGenderValue(common.myInt(Session["Gender"])));
            hstInput.Add("@intRecordId", common.myInt(ddlRecord.SelectedValue));
            hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));

            dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);
        }
        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            DataTable dtFieldValue = new DataTable();
            DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
            //dv1.Sort = " FieldId  ";
            dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
            DataTable dtFieldName = dv1.ToTable();
            if (dsAllSectionDetails.Tables.Count > 2)
            {
                DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
                dtFieldValue = dv2.ToTable();
            }

            DataSet dsAllFieldsDetails = new DataSet();
            dsAllFieldsDetails.Tables.Add(dtFieldName);
            dsAllFieldsDetails.Tables.Add(dtFieldValue);

            if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
            {
                if (dsAllSectionDetails.Tables.Count > 2)
                {

                    if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
                    {
                        string sBegin = "", sEnd = "";

                        DataRow dr3;
                        dr3 = dsAllSectionDetails.Tables[0].Rows[0];

                        getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);

                        ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                        str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]), common.myInt(item["NoOfBlankRows"]));
                        str += " ";

                        if (iPrevId == common.myInt(item["TemplateId"]))
                        {
                            if (t2 == 0)
                            {
                                if (t3 == 0)//Template
                                {
                                    t3 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    { BeginList3 = "<ul>"; EndList3 = "</ul>"; }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    { BeginList3 = "<ol>"; EndList3 = "</ol>"; }
                                }
                            }

                            if (common.myStr(item["SectionsBold"]) != "" || common.myStr(item["SectionsItalic"]) != "" || common.myStr(item["SectionsUnderline"]) != "" || common.myStr(item["SectionsFontSize"]) != "" || common.myStr(item["SectionsForecolor"]) != "" || common.myStr(item["SectionsListStyle"]) != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (common.myBool(item["SectionDisplayTitle"]))   //19June2010
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        //objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                    }
                                }
                                BeginList3 = "";
                            }
                            else
                            {
                                if (common.myBool(item["SectionDisplayTitle"]))    //19June
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                    }
                                }
                            }

                            if (common.myStr(item["SectionsListStyle"]) == "3" || common.myStr(item["TemplateListStyle"]) == "0")
                                objStrTmp.Append("<br />");
                            else
                            {
                                if (str.Trim() != "")
                                    objStrTmp.Append(str);
                            }
                        }
                        else
                        {

                            if (t == 0)
                            {
                                t = 1;
                                if (common.myStr(item["TemplateListStyle"]) == "1")
                                { BeginList = "<ul>"; EndList = "</ul>"; }
                                else if (common.myStr(item["TemplateListStyle"]) == "2")
                                { BeginList = "<ol>"; EndList = "</ol>"; }
                            }
                            if (common.myStr(item["TemplateBold"]) != "" || common.myStr(item["TemplateItalic"]) != "" || common.myStr(item["TemplateUnderline"]) != "" || common.myStr(item["TemplateFontSize"]) != "" || common.myStr(item["TemplateForecolor"]) != "" || common.myStr(item["TemplateListStyle"]) != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Template", ref sBegin, ref sEnd, item);
                                if (common.myBool(item["TemplateDisplayTitle"]))
                                    if (sBegin.Contains("<br/>") == true)
                                    {
                                        sBegin = sBegin.Remove(0, 5);
                                        //objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
                                    }
                                    else
                                    {
                                        //objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
                                    }
                                BeginList = "";
                            }
                            else
                            {
                                if (common.myBool(item["TemplateDisplayTitle"]))
                                    objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                            }
                            if (common.myStr(item["TemplateListStyle"]) == "3" || common.myStr(item["TemplateListStyle"]) == "0")
                                objStrTmp.Append("<br />");
                            objStrTmp.Append(EndList);
                            if (t2 == 0)
                            {
                                t2 = 1;
                                if (common.myStr(item["SectionsListStyle"]) == "1")
                                { BeginList2 = "<ul>"; EndList3 = "</ul>"; }
                                else if (common.myStr(item["SectionsListStyle"]) == "2")
                                { BeginList2 = "<ol>"; EndList3 = "</ol>"; }
                            }
                            if (common.myStr(item["SectionsBold"]) != "" || common.myStr(item["SectionsItalic"]) != "" || common.myStr(item["SectionsUnderline"]) != "" || item["SectionsFontSize"].ToString() != "" || item["SectionsForecolor"].ToString() != "" || common.myStr(item["SectionsListStyle"]) != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (common.myBool(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    if (str.Trim() != "") //add 19June2010
                                    {
                                        objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                    }
                                BeginList2 = "";
                            }
                            else
                            {
                                if (common.myBool(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                {
                                    objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                }
                            }
                            if (common.myStr(item["SectionsListStyle"]) == "3" || common.myStr(item["SectionsListStyle"]) == "0")
                                objStrTmp.Append("<br />");

                            objStrTmp.Append(str);
                        }
                        iPrevId = common.myInt(item["TemplateId"]);


                    }

                }
            }
        }

        if (t2 == 1 && t3 == 1)
            objStrTmp.Append(EndList3);
        else
            objStrTmp.Append(EndList);
        if (GetPageProperty(iFormId) != null)
        {
            objStrSettings.Append(objStrTmp.ToString());
            sb.Append(objStrSettings.ToString());
        }
        else
            sb.Append(objStrTmp.ToString());
    }

    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myInt(Session["HospitalLocationId"]).ToString());
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
                sFontSize = " font-size: " + sFontSize + ";";
        }
        return sFontSize;
    }

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        if (FontName != "")
            sBegin += " font-family: " + FontName + ";";
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myInt(Session["HospitalLocationId"]).ToString());
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }

    protected DataSet GetPageProperty(string iFormId)
    {
        Hashtable hstInput = new Hashtable();
        if (!common.myInt(Session["HospitalLocationID"]).Equals(0) && iFormId != "")
        {
            if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", 1);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }

    protected void Replace(string Ttype, ref string t, string strNew, string Lock)
    {

        //if (t != null)
        //{
        //    t = t.Replace('"', '$');
        //    //if (Lock == "0")
        //    //{

        //    string st = "<div id=$" + Ttype + "$>";
        //    int RosSt = t.IndexOf(st);
        //    if (RosSt > 0 || RosSt == 0)
        //    {
        //        int RosEnd = t.IndexOf("</div>", RosSt);

        //        //// string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
        //        //string str = t.Substring(RosSt, (RosEnd) - RosSt);
        //        //string ne = t.Replace(str, strNew);
        //        //t = ne.Replace('$', '"');


        //        if ((RosEnd - RosSt) < (strNew.Length))
        //        {
        //            if ((RosEnd - RosSt) < (strNew.Length))
        //            {
        //                // string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
        //                string str = t.Substring(RosSt, (RosEnd) - RosSt);
        //                string ne = t.Replace(str, strNew);
        //                t = ne.Replace('$', '"');
        //            }
        //            //else
        //            //{
        //            //    StringBuilder  strOld = new StringBuilder();
        //            //    StringBuilder strNew1 = new StringBuilder();
        //            //    strOld.Append(t, RosSt, RosEnd);
        //            //    strOld.AppendLine(strNew);
        //            //}
        //        }
        //        else if ((RosEnd - RosSt) > (strNew.Length))
        //        {
        //            // No Action Performed (No Replacement)
        //            t = t.Replace('$', '"');
        //        }
        //    }
        //    else
        //    {
        //        //string st2 = "<div id='" + Ttype + "'>";
        //        //int RosSt2 = t.IndexOf(st2);
        //        //if (RosSt2 > 0)
        //        //{
        //        //    int RosEnd2 = t.IndexOf("</div>", RosSt2);
        //        //    string str2 = t.PadRight(20).Substring(RosSt2, (RosEnd2) - RosSt2);
        //        //    string ne2 = t.Replace(str2, strNew);
        //        //    //t = ne2.Replace('$', '"');
        //        //}
        //        //else
        //        t += strNew; // re-activated on 28 Feb 2011 by rafat
        //        t = t.Replace('$', '"');
        //    }

        //    //}
        //    //else
        //    //{
        //    //    string st = "<div id=$" + Ttype + "$><span style=$color: #000000;$>";
        //    //    int RosSt = t.IndexOf(st);

        //    //    string ne = t.Replace("<div id=$" + Ttype + "$><span style=$color: #000000;$>", "<div id=$" + Ttype + "$><span style=$color: #000000;$>");
        //    //    t = ne.Replace('$', '"');
        //}
        //// }

        if (t != null)
        {
            t = t.Replace('"', '$');
            //if (Lock == "0")
            //{
            string st = "<div id=$" + Ttype + "$>";
            int RosSt = t.IndexOf(st);
            if (RosSt > 0 || RosSt == 0)
            {
                int RosEnd = t.IndexOf("</div>", RosSt);
                //string str = t.Substring(RosSt, (RosEnd - RosSt));
                string str = getSubString(common.myStr(t), RosSt, (RosEnd - RosSt));
                string ne = t.Replace(str, strNew);
                t = ne.Replace('$', '"');
            }
            else
            {
                //Remarks - Case will not happen because all templates <div> tag is inserted at the time of creating encounter

            }
            //}
            //else
            //{
            //    string st = "<div id=$" + Ttype + "$><span style=$color: #000000;$>";
            //    int RosSt = t.IndexOf(st);

            //    string ne = t.Replace("<div id=$" + Ttype + "$><span style=$color: #000000;$>", "<div id=$" + Ttype + "$><span style=$color: #000000;$>");
            //    t = ne.Replace('$', '"');
            //}
        }

        t = t.Replace('$', '"');
    }

    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {

        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sFontSize += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sFontSize += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sFontSize += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sFontSize += GetFontFamily(typ, item); };

            if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
            { sFontSize += " font-weight: bold;"; }
            if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
            { sFontSize += " font-style: italic;"; }
            if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
            { sFontSize += " text-decoration: underline;"; }

        }
        return sFontSize;
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, int NoOfBlankRows)
    {
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (common.myBool(TabularType))
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    //changes start
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    DataRow dr2;
                    foreach (DataRow dr in objDs.Tables[0].Rows)
                    {
                        dvValues.RowFilter = "FieldId = " + common.myStr(dr["FieldId"]);

                        //MaxLength = dvValues.ToTable().Rows.Count;

                        MaxLength = common.myInt(dvValues.ToTable().Compute("MAX(RowNo)", string.Empty));

                        if (MaxLength > 0)
                        {
                            dr2 = dr;
                            break;
                        }
                    }

                    if (MaxLength != 0)
                    {
                        int tableBorder = 1;

                        int TRows = 0;
                        int SectionId = 0;
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            TRows = common.myInt(objDs.Tables[0].Rows[0]["TRows"]);
                            SectionId = common.myInt(objDs.Tables[0].Rows[0]["SectionId"]);
                        }

                        if (SectionId == 4608
                            || SectionId == 4610
                            || SectionId == 4611)
                        {
                            tableBorder = 0;
                        }

                        objStr.Append("<br /><table border='" + tableBorder + "' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' ><tr align='center'>");

                        FieldsLength = objDs.Tables[0].Rows.Count;

                        #region header row - tabular with rows defination

                        DataSet dsR = new DataSet();
                        if (TRows > 0)
                        {
                            //border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;
                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + "+" + "</th>");
                        }
                        #endregion

                        for (int i = 0; i < FieldsLength; i++)   // it makes table header
                        {
                            //border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px;

                            string strHeader = common.myStr(objDs.Tables[0].Rows[i]["FieldName"]).Replace(hdnMandatoryStar.Value, "");

                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + strHeader + "</th>");
                            dr2 = objDs.Tables[0].Rows[i];

                            dvValues.RowFilter = "";
                            dvValues = new DataView(objDs.Tables[1]);
                            dvValues.RowFilter = "FieldId='" + common.myStr(dr2["FieldId"]) + "'";
                            dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                            if (dvValues.ToTable().Rows.Count > MaxLength)
                                MaxLength = dvValues.ToTable().Rows.Count;
                        }

                        objStr.Append("</tr>");
                        if (MaxLength == 0)
                        {
                        }
                        else
                        {
                            for (int i = 0; i < MaxLength; i++)
                            {
                                StringBuilder sbTR = new StringBuilder();
                                bool isDataFound = false;

                                for (int j = 0; j < dsMain.Tables.Count; j++)
                                {
                                    DataView dvM = dsMain.Tables[j].DefaultView;
                                    dvM.RowFilter = "RowNo=" + (i + 1);
                                    dvM.Sort = "RowNo ASC";

                                    DataTable tbl = dvM.ToTable();

                                    if (TRows > 0 && j == 0)
                                    {
                                        if (tbl.Rows.Count > 0)
                                        {
                                            if (common.myLen(tbl.Rows[0]["RowCaption"]) > 0)
                                            {
                                                sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tbl.Rows[0]["RowCaption"]) + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (dsMain.Tables.Count > (j + 1))
                                            {
                                                DataView dvM2 = dsMain.Tables[j + 1].DefaultView;
                                                dvM2.RowFilter = "RowNo=" + (i + 1);
                                                dvM2.Sort = "RowNo ASC";

                                                DataTable tblH = dvM2.ToTable();
                                                if (tblH.Rows.Count > 0)
                                                {
                                                    if (common.myLen(tblH.Rows[0]["RowCaption"]) > 0)
                                                    {
                                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tblH.Rows[0]["RowCaption"]) + "</td>");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (tbl.Rows.Count > 0)
                                    {
                                        isDataFound = true;
                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>" + common.myStr(tbl.Rows[0]["TextValue"]) + "</td>");
                                    }
                                    else
                                    {
                                        sbTR.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "'>&nbsp;</td>");
                                    }
                                }

                                if (isDataFound)
                                {
                                    objStr.Append("<tr valign='top'>");
                                    objStr.Append(sbTR.ToString());
                                    objStr.Append("</tr>");
                                }
                            }

                            for (int rIdx = 0; rIdx < NoOfBlankRows; rIdx++)
                            {
                                objStr.Append("<tr valign='top'>");

                                for (int cIdx = 0; cIdx < dsMain.Tables.Count; cIdx++)
                                {
                                    objStr.Append("<td style=' " + sFontSize.Replace("bold", "normal") + "' align='right'>&nbsp;</td>");
                                }
                                objStr.Append("</tr>");
                            }

                            objStr.Append("</table>");
                            //}
                        }
                    }
                    //changes end



                    //DataRow dr = objDs.Tables[0].Rows[0];
                    //DataView dvValues = new DataView(objDs.Tables[1]);
                    //dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";

                    //MaxLength = dvValues.ToTable().Rows.Count;

                    //if (MaxLength != 0)
                    //{
                    //    //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                    //    objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");

                    //    FieldsLength = objDs.Tables[0].Rows.Count;

                    //    for (int i = 0; i < FieldsLength; i++)   // it makes table
                    //    {
                    //        objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");
                    //        dr = objDs.Tables[0].Rows[i];
                    //        dvValues = new DataView(objDs.Tables[1]);
                    //        dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    //        dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));


                    //        if (dvValues.ToTable().Rows.Count > MaxLength)
                    //            MaxLength = dvValues.ToTable().Rows.Count;
                    //    }

                    //    objStr.Append("</tr>");
                    //    if (MaxLength == 0)
                    //    {
                    //        //objStr.Append("<tr>");
                    //        //for (int i = 0; i < FieldsLength; i++)
                    //        //{
                    //        //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                    //        //}
                    //        //objStr.Append("</tr></table>");
                    //    }
                    //    else
                    //    {
                    //        if (dsMain.Tables[0].Rows.Count > 0)
                    //        {
                    //            for (int i = 0; i < MaxLength; i++)
                    //            {
                    //                objStr.Append("<tr>");
                    //                for (int j = 0; j < dsMain.Tables.Count; j++)
                    //                {
                    //                    if (dsMain.Tables[j].Rows.Count > i
                    //                        && dsMain.Tables[j].Rows.Count > 0)
                    //                    {
                    //                        objStr.Append("<td style=' " + sFontSize + "' align='right'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                    //                    }
                    //                    else
                    //                    {
                    //                        objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                    //                    }
                    //                }
                    //                objStr.Append("</tr>");
                    //            }
                    //            objStr.Append("</table>");
                    //        }
                    //    }
                    //}
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;

                objStr.Append("<br /><table border='0' style='border-color:#000000; border-collapse:collapse; " + sFontSize + "'  cellpadding='4' cellspacing='4' >");

                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objStr.Append("<tr valign='top'>");

                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (common.myStr(item["FieldsListStyle"]) == "1")
                        { BeginList = "<ul>"; EndList = "</ul>"; }
                        else if (common.myStr(item["FieldsListStyle"]) == "2")
                        { BeginList = "<ol>"; EndList = "</ol>"; }
                    }
                    if (common.myStr(item["FieldsBold"]) != "" || common.myStr(item["FieldsItalic"]) != "" || common.myStr(item["FieldsUnderline"]) != "" || common.myStr(item["FieldsFontSize"]) != "" || common.myStr(item["FieldsForecolor"]) != "" || common.myStr(item["FieldsListStyle"]) != "")
                    {
                        //rafat1
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);

                            if (sBegin.StartsWith("<br/>"))
                            {
                                if (sBegin.Length > 5)
                                {
                                    //sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                    sBegin = getSubString(common.myStr(sBegin), 5, common.myStr(sBegin).Length - 5);
                                }
                            }

                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");

                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]).Replace(hdnMandatoryStar.Value, ""));
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    //changes
                                    //objStr.Append(sEnd + "</li>");
                                    objStr.Append(sEnd);
                                }
                                //}

                                string FType = item["FieldType"].ToString();
                                if (FType != "W")
                                {
                                    objStr.Append("</td>");
                                }
                            }
                            BeginList = "";
                            sBegin = "";
                        }
                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");
                                objStr.Append(common.myStr(item["FieldName"]).Replace(hdnMandatoryStar.Value, ""));

                                string FType = item["FieldType"].ToString();
                                if (FType != "W")
                                {
                                    objStr.Append("</td>");
                                }
                            }
                        }
                    }

                    if (objDs.Tables.Count > 1)
                    {
                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        objDt = objDv.ToTable();

                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");

                        if (dtFieldType.Rows.Count > 0 && objDt.Rows.Count == 0)
                        {
                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                            if (FType == "O")
                            {
                                int DataObjectId = common.myInt(dtFieldType.Rows[0]["DataObjectId"]);

                                clsIVF objivf = new clsIVF(sConString);

                                string strOutput = objivf.getDataObjectValue(DataObjectId);

                                if (common.myLen(strOutput) > 0)
                                {
                                    objStr.Append("<td>" + common.myStr(dtFieldType.Rows[0]["FieldName"]).Replace(hdnMandatoryStar.Value, "") + "</td>");
                                    objStr.Append("<td>" + strOutput + "</td>");
                                }
                            }
                        }

                        if (objDt.Rows.Count > 0)
                        {
                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                            if (FType != "W")
                            {
                                objStr.Append("<td>");
                            }

                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());

                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1" || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")

                                                dv1.RowFilter = "TextValue='Yes'";
                                            else
                                                dv1.RowFilter = "TextValue='No'";
                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    else
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    else
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                else
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                        }
                                        else
                                        {
                                            BindDataValue(objDs, objDt, objStr, i, FType);//check1
                                        }
                                    }
                                    else
                                    {
                                        BindDataValue(objDs, objDt, objStr, i, FType);
                                    }
                                }
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "ST" || FType == "SB" || FType == "W")
                                {
                                    if (FType == "W")
                                    {
                                        if (common.myStr(ViewState["iTemplateId"]) != "163")
                                        {
                                            if (i == 0)
                                            {
                                                objStr.Append("<br/> " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else
                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            if (i == 0)
                                                objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            else
                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                    else
                                    {
                                        if (common.myStr(ViewState["iTemplateId"]) != "163")
                                        {
                                            if (i == 0)
                                            {
                                                objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            }
                                            else
                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            if (i == 0)
                                                objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            else
                                                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                else if (FType == "L")
                                {
                                    objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["TemplateFieldId"])));
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (FType != "C")
                                            objStr.Append("<br />");
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                            objStr.Append("<br />");
                                    }

                                }
                            }
                            // Cmt 25/08/2011
                            //if (objDt.Rows.Count > 0)
                            //{
                            //    if (objStr.ToString() != "")
                            //        objStr.Append(sEnd + "</li>");
                            //}

                            objStr.Append("</td>");
                        }
                    }
                    //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");

                    objStr.Append("</tr>");
                }

                if (objStr.ToString() != "")
                    objStr.Append(EndList);

                objStr.Append("</table>");
            }
        }

        return objStr.ToString();
    }

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                //sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                //sBegin += "<br/>";
            }
        }


        if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            { sBegin += GetFontFamily(typ, item); }
        }
        if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
        { sBegin += " font-weight: bold;"; }
        if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
        { sBegin += " font-style: italic;"; }
        if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
        { sBegin += " text-decoration: underline;"; }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
            sBegin += " '>";

    }

    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {
        //DataView dv1 = new DataView(objDs.Tables[1]);
        //dv1.RowFilter = "ValueId='" + objDt.Rows[i]["FieldValue"].ToString() + "'";
        //DataTable dt1 = dv1.ToTable();
        //if (dt1.Rows[0]["MainText"].ToString().Trim() != "")
        //{
        //    if (i == 0)
        //        objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
        //    else
        //    {
        //        if (FType != "C")
        //            objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
        //        else
        //        {
        //            if (i == 0)
        //                objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
        //            else if (i + 1 == objDs.Tables[2].Rows.Count)
        //                objStr.Append(" and " + dt1.Rows[i]["MainText"].ToString() + ".");
        //            else
        //                objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
        //        }
        //    }
        //}
        //else
        //{
        if (i == 0)
            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
        else
        {
            if (FType != "C")
                objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            else
            {
                if (i == 0)
                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                    objStr.Append(" and " + common.myStr(objDt.Rows[i]["TextValue"]) + ".");
                else
                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
            }
        }
        //}
    }

    protected void btnDiagnosis_OnClick(object Sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;

            RadWindowPrint.NavigateUrl = "/EMR/Templates/TemplateDiagnosis.aspx";

            RadWindowPrint.Height = 410;
            RadWindowPrint.Width = 610;
            RadWindowPrint.Top = 10;
            RadWindowPrint.Left = 10;
            RadWindowPrint.OnClientClose = string.Empty;
            RadWindowPrint.VisibleOnPageLoad = true;
            RadWindowPrint.Modal = true;
            RadWindowPrint.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindVisitRecord(bool IsAddVisit)
    {
        if (!common.myStr(Request.QueryString["Data"]).Trim().Equals(string.Empty))
        {
            IsAddVisit = true;
        }

        clsIVF objivf = new clsIVF(sConString);
        try
        {
            DataSet ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["PageId"]), common.myInt(Session["FacilityId"]));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count.Equals(0))
                    {
                        DataRow DR = ds.Tables[0].NewRow();

                        DR["RecordId"] = 1;
                        DR["RecordName"] = "Session 1 - " + DateTime.Now.ToString("dd/MM/yyyy  hh:mmtt");

                        ds.Tables[0].Rows.Add(DR);
                    }

                    if (IsAddVisit)
                    {
                        int rCount = ds.Tables[0].Rows.Count;
                        if (rCount > 0)
                        {
                            int recordid = common.myInt(ds.Tables[0].Rows[rCount - 1]["RecordId"]) + 1;
                            DataRow DR = ds.Tables[0].NewRow();

                            DR["RecordId"] = recordid;
                            DR["RecordName"] = "Session " + recordid.ToString() + " - " + DateTime.Now.ToString("dd/MM/yyyy  h:mmtt");

                            ds.Tables[0].Rows.Add(DR);
                        }
                    }

                    ddlRecord.DataSource = ds.Tables[0];
                    ddlRecord.DataValueField = "RecordId";
                    ddlRecord.DataTextField = "RecordName";
                    ddlRecord.DataBind();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlRecord.SelectedIndex = 0;
                    }

                    if (IsAddVisit && ds.Tables[0].Rows.Count > 0)
                    {
                        ddlRecord.SelectedIndex = ds.Tables[0].Rows.Count - 1;

                        isFirstTime = true;
                        tvCategory_SelectedNodeChanged(null, null);
                        isFirstTime = false;
                    }
                    else if (common.myInt(Request.QueryString["RecordId"]) > 0)
                    {
                        ddlRecord.SelectedIndex = ddlRecord.Items.IndexOf(ddlRecord.Items.FindItemByValue(common.myInt(Request.QueryString["RecordId"]).ToString()));
                        isFirstTime = true;
                        tvCategory_SelectedNodeChanged(null, null);
                        isFirstTime = false;
                    }

                    ViewState["IsDataSaved"] = false;
                    SetPermission();

                    //ddlReport.SelectedIndex = 0;
                    //btnPrintReport.Visible = true;
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

    protected void btnNewRecord_OnClick(object sender, EventArgs e)
    {
        bindVisitRecord(true);
    }

    protected void ddlRecord_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            isFirstTime = true;
            tvCategory_SelectedNodeChanged(sender, e);
            isFirstTime = false;

            bindPatientTemplateList(true);

            //ddlReport.SelectedIndex = 0;
            //btnPrintReport.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnLabResult_OnClick(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = string.Empty;

            RadWindowPrint.NavigateUrl = "/EMR/Templates/LabResult.aspx?PT=Y";

            RadWindowPrint.Height = 640;
            RadWindowPrint.Width = 1000;
            RadWindowPrint.Top = 10;
            RadWindowPrint.Left = 10;
            RadWindowPrint.OnClientClose = string.Empty;
            RadWindowPrint.VisibleOnPageLoad = true;
            RadWindowPrint.Modal = true;
            RadWindowPrint.VisibleStatusbar = false;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void btnBindFieldData_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvSelectedServices.Rows)
        {
            string FieldId = common.myInt(row.Cells[(byte)enumNonT.FieldId].Text).ToString();
            string FieldType = common.myStr(row.Cells[(byte)enumNonT.FieldType].Text).Trim();
            string SectionId = common.myInt(row.Cells[(byte)enumNonT.SectionId].Text).ToString();
            BaseC.EMR objEMR = new BaseC.EMR(sConString);
            if (common.myInt(hdnSelCell.Text).Equals(common.myInt(row.RowIndex)))
            {
                if (FieldType.Equals("D"))
                {
                    DataSet ds = objEMR.GetTemplateFieldValue(common.myInt(Session["HospitalLocationId"]), common.myInt(SectionId), common.myInt(FieldId));
                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "Active=1";

                    DropDownList ddl = (DropDownList)row.FindControl("D");
                    ddl.Items.Clear();
                    ddl.DataSource = null;
                    ddl.DataBind();
                    ddl.DataSource = dv.ToTable();
                    ddl.DataTextField = "ValueName";
                    ddl.DataValueField = "ValueId";
                    ddl.DataBind();

                    if (!common.myInt(Session["ValueId"]).Equals(0))
                    {
                        ddl.Items.FindByValue(common.myInt(Session["ValueId"]).ToString()).Selected = true;
                        ddl.SelectedValue = common.myInt(Session["ValueId"]).ToString();
                        Session["ValueId"] = null;
                    }
                }
                else if (FieldType.Equals("C"))
                {
                    DataSet ds = objEMR.GetTemplateFieldValue(common.myInt(Session["HospitalLocationId"]), common.myInt(SectionId), common.myInt(FieldId));

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "Active=1";

                    DataList rpt = (DataList)row.FindControl("C");
                    rpt.DataSource = dv.ToTable();
                    rpt.DataBind();

                    foreach (DataListItem item in rpt.Items)
                    {
                        HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                        CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                        CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                        HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                        CheckBox chk = (CheckBox)item.FindControl("C");
                        if (!common.myInt(Session["ValueId"]).Equals(0))
                        {
                            if (common.myInt(hdn.Value).Equals(common.myInt(Session["ValueId"])))
                            {
                                chk.Checked = true;
                            }
                        }
                    }
                }
            }
        }
    }
    protected void btnCalender_Click(object sender, EventArgs e)
    {
        ViewState["CalCulate"] = true;
        foreach (GridViewRow row in gvSelectedServices.Rows)
        {
            string FieldId = common.myInt(row.Cells[(byte)enumNonT.FieldId].Text).ToString();
            BaseC.Patient patient = new BaseC.Patient(sConString);

            if (common.myStr(row.Cells[(byte)enumNonT.FieldType].Text).Equals("S"))
            {
                AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)row.FindControl("CalendarExtender3");
                if (FieldId.Equals(64327))
                {
                    TextBox txtDate = (TextBox)row.FindControl("txtDate");
                    string dt1 = patient.FormatDMY(txtDate.Text);//.AddDays(294);
                    ViewState["BookingDate"] = null;
                    ViewState["BookingDate"] = dt1;
                }
                if (FieldId.Equals(64328))
                {
                    TextBox txtDate = (TextBox)row.FindControl("txtDate");
                    if (!txtDate.Text.Equals("__/__/____") && !txtDate.Text.Equals(string.Empty))
                    {
                        string dt1 = patient.FormatDateDateMonthYear(txtDate.Text);//.AddDays(294);
                        ViewState["Date"] = Convert.ToDateTime(dt1).AddDays(280).ToString("dd/MM/yyyy");
                        ViewState["LMPDate"] = txtDate.Text;
                    }

                }
                if (FieldId.Equals(64329))
                {
                    if (ViewState["Date"] != null)
                    {
                        TextBox txtDate = (TextBox)row.FindControl("txtDate");
                        txtDate.Text = ViewState["Date"].ToString();
                        txtDate.Enabled = false;
                        cal.Enabled = false;
                    }
                }

            }
            if (common.myStr(row.Cells[(byte)enumNonT.FieldType].Text).Equals("T"))
            {
                if (FieldId.Equals("64330"))
                {
                    if (ViewState["LMPDate"] != null && ViewState["BookingDate"] != null)
                    {
                        TextBox txtDate = (TextBox)row.FindControl("txtT");
                        Button btnHelp = (Button)row.FindControl("btnHelp");
                        btnHelp.Enabled = false;
                        TimeSpan tt = Convert.ToDateTime(ViewState["BookingDate"]) - Convert.ToDateTime(patient.FormatDMY(ViewState["LMPDate"].ToString()));
                        int TotalDays = tt.Days;
                        if (!TotalDays.ToString().Contains("-"))
                        {
                            if (TotalDays < 7)
                            {
                                txtDate.Text = TotalDays.Equals(1) ? TotalDays + " dy " : TotalDays + " dys";
                            }
                            else
                            {
                                int iWeeks = TotalDays / 7;
                                int iDays = TotalDays % 7;
                                if (!iDays.Equals(0))
                                {
                                    string sResult = iWeeks.Equals(1) ? iWeeks + " wk " : iWeeks + " wks ";
                                    txtDate.Text = sResult + iDays + " Days";
                                }
                                else
                                {
                                    txtDate.Text = iWeeks.Equals(1) ? iWeeks + " wk " : iWeeks + " wks ";
                                }
                            }
                            txtDate.Enabled = false;
                            ViewState["LMPDate"] = null;
                            ViewState["BookingDate"] = null;
                            break;
                        }
                        else
                        {
                            Alert.ShowAjaxMsg("Please select correct LMP date", Page);
                            return;
                        }
                    }
                    //else
                    //{
                    //    Alert.ShowAjaxMsg("Please select correct LMP date", Page);
                    //    return;
                    //}

                }
            }
        }
    }

    protected void ibtnReportSetup_Click(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "/EMR/Masters/EMRReportSetup.aspx?PT=M&TId=" + common.myInt(ViewState["PageId"]) + "&DoctorId=" + common.myInt(ViewState["DoctorId"]);
        RadWindowForNew.Height = 620;
        RadWindowForNew.Width = 980;
        RadWindowForNew.Top = 10;
        RadWindowForNew.Left = 10;
        RadWindowForNew.OnClientClose = "OnClientCloseReportSetup";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; //Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
    }

    protected void btnReportSetup_Click(object sender, EventArgs e)
    {
        try
        {
            //btnPrintReport.Visible = false;
            //bindReportList();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindMainTemplateList()
    {
        clsIVF objivf = new clsIVF(sConString);
        int FreeTextTemplateId = 0;
        try
        {
            DataSet ds = new DataSet();

            int TemplateId = 0;
            //Commented by rakesh start
            //string TemplateIdType = "ST";
            //Commented by rakesh end
            //Added by rakesh start
            string TemplateIdType = string.Empty;
            if (common.myStr(ViewState["Source"]).Equals("DeptRegister"))
            {
                TemplateIdType = "DR";
            }
            else
            {
                TemplateIdType = "ST";
            }
            //Added by rakesh end

            if (common.myInt(Request.QueryString["DisplayMenu"]).Equals(1))
            {
                if (common.myStr(Request.QueryString["TemplateId"]).Trim().Length > 0)
                {
                    if (common.myStr(Request.QueryString["TemplateId"]).StartsWith("ST")
                        || common.myStr(Request.QueryString["TemplateId"]).StartsWith("TT"))
                    {
                        //TemplateId = common.myInt(common.myStr(Request.QueryString["TemplateId"]).Substring(2, common.myStr(Request.QueryString["TemplateId"]).Length - 2));
                        TemplateId = common.myInt(getSubString(common.myStr(Request.QueryString["TemplateId"]), 2, common.myStr(Request.QueryString["TemplateId"]).Length - 2));
                    }
                    else
                    {
                        TemplateId = common.myInt(Request.QueryString["TemplateId"]);
                    }
                }
                if (common.myStr(Request.QueryString["TemplateId"]).Trim().Length > 0)
                {
                    if (common.myStr(Request.QueryString["TemplateId"]).StartsWith("ST")
                        || common.myStr(Request.QueryString["TemplateId"]).StartsWith("TT"))
                    {
                        //TemplateIdType = common.myStr(common.myStr(Request.QueryString["TemplateId"]).Substring(0, 2));
                        TemplateIdType = getSubString(common.myStr(Request.QueryString["TemplateId"]), 0, 2);
                    }
                }
            }
            if (!common.myStr(ViewState["TypeName"]).Equals(string.Empty))
            {
                TemplateIdType = "TT";
            }

            int iDoctorSpecialisation = common.myInt(ViewState["DoctorSpecialisationId"]);
            string EmpType = common.myStr(ViewState["EmployeeType"]);
            if (common.myStr(Request.QueryString["TemplateGroupId"]).Trim().Length > 0)
            {
                TemplateIdType = "TG";//Template Grouping
                // ds = objivf.getEMRTemplateGroup(common.myInt(Session["HospitalLocationId"]), common.myInt(Request.QueryString["TemplateGroupId"]), TemplateIdType);
            }
            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                            && !common.myStr(ViewState["TagType"]).Equals("D"))
            {
                ds = getEMRTemplateForSelectedService(common.myInt(Session["HospitalLocationId"]),
                                        common.myStr(Request.QueryString["TemplateRequiredServices"]).Trim(), 0);
            }
            else
            {
                bool IsSuperUserLogin = false;
                FreeTextTemplateId = 0;
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE").Equals(true) && common.myBool(Session["isEMRSuperUser"]).Equals(true))
                {
                    IsSuperUserLogin = true;
                }
                if (common.myInt(Request.QueryString["FreeTextTemplateId"]) > 0)
                {
                    FreeTextTemplateId = common.myInt(Request.QueryString["FreeTextTemplateId"]);
                }
                if (Request.QueryString["TemplateId"] != null && Request.QueryString["IsEMRPopUp"] == null && Request.QueryString["SingleScreenTemplateCode"] == null && Request.QueryString["TemplateGroupId"] == null)
                {
                    if (Session["InvoiceId"] != null)
                    {
                        ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), EmpType, iDoctorSpecialisation, common.myInt(ViewState["ServiceId"]),
                              common.myStr(Session["OPIP"]), common.myInt(Request.QueryString["TemplateId"]), (FreeTextTemplateId > 0 ? string.Empty : TemplateIdType),
                              common.myInt(Session["FacilityID"]), common.myBool(Request.QueryString["IsAddendum"]),
                              common.myInt(Session["InvoiceId"]), common.myInt(Session["RegistrationID"]),
                              common.myInt(Session["encounterid"]), IsSuperUserLogin, FreeTextTemplateId, common.myInt(Session["LoginDoctorId"]));

                        if (FreeTextTemplateId > 0 && common.myLen(TemplateIdType) > 0)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Equals(0))
                                {
                                    ds = new DataSet();
                                    ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), EmpType, iDoctorSpecialisation, common.myInt(ViewState["ServiceId"]),
                                         common.myStr(Session["OPIP"]), common.myInt(Request.QueryString["TemplateId"]), TemplateIdType,
                                         common.myInt(Session["FacilityID"]), common.myBool(Request.QueryString["IsAddendum"]),
                                         common.myInt(Session["InvoiceId"]), common.myInt(Session["RegistrationID"]),
                                         common.myInt(Session["encounterid"]), IsSuperUserLogin, 0, common.myInt(Session["LoginDoctorId"]));
                                }
                            }
                        }

                    }
                }
                else
                {
                    //if (Request.QueryString["IsEMRPopUp"] != null)
                    //{
                    //    if (common.myStr(Request.QueryString["IsEMRPopUp"]) != string.Empty)
                    //    {
                    //        if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                    //        {
                    if (Session["InvoiceId"] != null)
                    {
                        ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), EmpType, iDoctorSpecialisation, common.myInt(ViewState["ServiceId"]),
                                                 common.myStr(Session["OPIP"]), common.myInt(Request.QueryString["TemplateGroupId"]), (FreeTextTemplateId > 0 ? string.Empty : TemplateIdType),
                                                 common.myInt(Session["FacilityID"]), common.myBool(Request.QueryString["IsAddendum"]),
                                                 common.myInt(Session["InvoiceId"]), common.myInt(Session["RegistrationID"]),
                                                 common.myInt(Session["encounterid"]), IsSuperUserLogin, FreeTextTemplateId, common.myInt(Session["LoginDoctorId"]));

                        if (FreeTextTemplateId > 0 && common.myLen(TemplateIdType) > 0)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count.Equals(0))
                                {
                                    ds = new DataSet();
                                    ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), EmpType, iDoctorSpecialisation, common.myInt(ViewState["ServiceId"]),
                                                 common.myStr(Session["OPIP"]), common.myInt(Request.QueryString["TemplateGroupId"]), TemplateIdType,
                                                 common.myInt(Session["FacilityID"]), common.myBool(Request.QueryString["IsAddendum"]),
                                                 common.myInt(Session["InvoiceId"]), common.myInt(Session["RegistrationID"]),
                                                 common.myInt(Session["encounterid"]), IsSuperUserLogin, 0, common.myInt(Session["LoginDoctorId"]));
                                }
                            }
                        }

                    }
                    //        }
                    //    }
                    //}
                }
            }
            // change by balkishan start
            DataView view = new DataView();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {

                    if (!ds.Tables[0].Columns.Contains("IsMandatory"))
                    {
                        DataColumn colIsMandatory = new DataColumn("IsMandatory");
                        ds.Tables[0].Columns.Add(colIsMandatory);
                    }
                    if (!ds.Tables[0].Columns.Contains("IsCopyPreviousEpisod"))
                    {
                        DataColumn colIsCopyPreviousEpisod = new DataColumn("IsCopyPreviousEpisod");
                        ds.Tables[0].Columns.Add(colIsCopyPreviousEpisod);
                    }

                    view.Table = ds.Tables[0];

                    if (TemplateIdType.Equals("TT") && common.myStr(ViewState["TypeName"]).Equals(string.Empty))
                    {
                        view.RowFilter = "TemplateTypeId=" + common.myInt(TemplateId).ToString();
                    }

                    DataView dvFilterTemplateCode = view;
                    if (common.myLen(ViewState["SingleScreenTemplateCode"]) > 0)
                    {
                        if (common.myStr(ViewState["SingleScreenTemplateCode"]).ToUpper().Equals("OTH"))
                        {
                            dvFilterTemplateCode.RowFilter = "TemplateTypeCode<>'EXM' AND TemplateTypeCode<>'HIS' AND TemplateTypeCode<>'POC' AND TemplateTypeCode<>'CA' AND TemplateTypeCode<>'PT' AND TemplateTypeCode<>'NS' AND TemplateTypeCode<>'IN'";
                        }
                        else
                        {
                            dvFilterTemplateCode.RowFilter = "TemplateTypeCode='" + common.myStr(ViewState["SingleScreenTemplateCode"]) + "'";
                        }
                    }
                    ddlTemplateMain.Items.Clear();
                    ddlTemplateMain.Visible = true;

                    RadComboBoxItem item;
                    if (dvFilterTemplateCode.ToTable().Rows.Count > 0)
                    {
                        DataView dv = ((DataTable)dvFilterTemplateCode.ToTable()).DefaultView;
                        if (common.myLen(ViewState["TypeName"]) > 0)
                        {
                            dv.RowFilter = "TypeName='" + common.myStr(ViewState["TypeName"]) + "'";
                        }

                        if (dv.ToTable().Rows.Count > 0)
                        {
                            foreach (DataRow dr in dv.ToTable().Rows)
                            {
                                item = new RadComboBoxItem();
                                item.Text = common.myStr(dr["TemplateName"]);
                                item.Value = common.myInt(dr["TemplateId"]).ToString();
                                item.Attributes.Add("TemplateTypeID", common.myStr(dr["TemplateTypeID"]));
                                item.Attributes.Add("EntryType", common.myStr(dr["EntryType"]));
                                item.Attributes.Add("TemplateTypeCode", common.myStr(dr["TemplateTypeCode"]));

                                item.Attributes.Add("IsCopyPreviousEpisod", common.myBool(dr["IsCopyPreviousEpisod"]).ToString());
                                item.Attributes.Add("IsConfidential", common.myBool(dr["IsConfidential"]).ToString());

                                if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                                {
                                    try
                                    {
                                        if (common.myInt(dr["IsMandatory"]).Equals(1))
                                        {
                                            item.ForeColor = System.Drawing.Color.Red;
                                        }
                                        else
                                        {
                                            item.ForeColor = System.Drawing.Color.Blue;
                                        }
                                    }
                                    catch { }
                                }
                                ddlTemplateMain.Items.Add(item);
                                //ddlTemplateMain.DataBind();                    
                            }
                            if (Request.QueryString["IsEMRPopUp"] != null)
                            {
                                if (common.myStr(Request.QueryString["IsEMRPopUp"]) != string.Empty)
                                {
                                    if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                                    {
                                        ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(dv.ToTable().Rows[0]["TemplateId"]).ToString()));
                                        // ddlTemplateMain_SelectedIndexChanged(null, null);

                                        // ddlTemplateMainSelectedIndexChanged(true);
                                        //tvCategory_SelectedNodeChanged(null, null);
                                    }
                                    if ((common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1")) && (common.myStr(ViewState["SingleScreenTemplateCode"]).Equals("OTH")))
                                    {
                                        if (Request.QueryString["TemplateId"] != null)
                                        {
                                            if (common.myStr(Request.QueryString["TemplateId"]) != string.Empty)
                                            {
                                                ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myStr(Request.QueryString["TemplateId"])));
                                            }
                                        }
                                    }

                                    if (FreeTextTemplateId > 0)
                                    {
                                        ddlTemplateMain.SelectedIndex = 0;
                                    }
                                }
                            }
                        }

                        dv.Dispose();
                    }
                    if (TemplateIdType.Equals("ST") || TemplateIdType.Equals("TT"))
                    {
                        if (common.myStr(ViewState["TypeName"]).ToUpper().Equals("PATIENTALERT"))
                        {
                            ddlTemplateMain.SelectedIndex = 0;
                        }
                        //else if (TemplateId > 0 && common.myInt(ddlTemplateMain.SelectedValue).Equals(0))
                        else if (TemplateId > 0)
                        {
                            if (Request.QueryString["IsEMRPopUp"] != null)
                            {
                                if (common.myStr(Request.QueryString["IsEMRPopUp"]) != string.Empty)
                                {
                                    if (common.myStr(Request.QueryString["IsEMRPopUp"]).Equals("1"))
                                    {
                                        //ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(dv.ToTable().Rows[0]["TemplateId"]).ToString()));
                                    }
                                    else
                                    {
                                        ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(TemplateId).ToString()));
                                    }
                                }
                                else
                                {
                                    ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(TemplateId).ToString()));
                                }
                            }
                            else
                            {
                                ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(TemplateId).ToString()));
                            }
                        }

                        if (!common.myInt(ddlTemplateMain.SelectedIndex).Equals(-1))
                        {
                            ddlTemplateMainSelectedIndexChanged(false);
                        }
                    }
                    dvFilterTemplateCode.Dispose();
                    view.Dispose();
                    //if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
                    //{
                    //    lnkPatientImmunization.Visible = common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateTypeCode"]).Equals("PH");
                    //}

                    bindPatientTemplateList(true);
                    //if (common.myStr(ViewState["TypeName"]).ToUpper().Equals("PATIENTALERT"))
                    //    ddlTemplatePatient.Enabled = false;
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

    private void bindPatientTemplateList(bool IsNewLoad)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            spnTemplatePatient.Visible = false;
            // btnViewDymanicTemplate.Visible = false;
            DataSet ds = new DataSet();
            ds = objEMR.getTemplateEnteredList(common.myInt(Session["HospitalLocationId"]),
                            common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                spnTemplatePatient.Visible = true;
                // btnViewDymanicTemplate.Visible = true;
            }
            if (common.myBool(Request.QueryString["IsAddendum"]))
            {
                ddlTemplatePatient.Visible = false;
                spnTemplatePatient.Visible = false;
                Label2.Visible = false;
            }

            ddlTemplatePatient.Items.Clear();

            RadComboBoxItem item;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                item = new RadComboBoxItem();
                if (common.myBool(dr["IsConfidential"]))
                {
                    item.Text = common.myStr(dr["TemplateName"]) + " (Confidential)";
                }
                else
                {
                    item.Text = common.myStr(dr["TemplateName"]);
                }
                item.Value = common.myInt(dr["TemplateId"]).ToString();
                item.Attributes.Add("IsConfidential", common.myBool(dr["IsConfidential"]).ToString());
                ddlTemplatePatient.Items.Add(item);
                ddlTemplatePatient.DataBind();
            }
            //ddlTemplatePatient.DataSource = ds.Tables[0];
            //ddlTemplatePatient.DataTextField = "TemplateName";
            //ddlTemplatePatient.DataValueField = "TemplateId";
            //ddlTemplatePatient.DataBind();
            ddlTemplatePatient.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));

            ViewState["IsDataSaved"] = false;
            if (common.myInt(Request.QueryString["TemplateId"]) > 0)
            {
                DataView DV = ds.Tables[0].DefaultView;

                DV.RowFilter = "TemplateId=" + common.myInt(Request.QueryString["TemplateId"]);

                if (DV.ToTable().Rows.Count > 0)
                {
                    ViewState["IsDataSaved"] = true;
                }
            }

            SetPermission();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void lnkViewConfidential_OnClick(object sender, EventArgs e)
    {
        if (common.myBool(ddlTemplateMain.SelectedItem.Attributes["IsConfidential"]))
        {
            RadWindowForNew.NavigateUrl = "~/EMR/Templates/PermissionConfidentialUsers.aspx?TemplateId=" + common.myStr(ddlTemplateMain.SelectedValue)
                + "&EncId=" + common.myStr(ViewState["EncounterId"]) + "&TemplateName=" + common.myStr(ddlTemplateMain.SelectedItem.Text);
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 1200;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
    }


    protected void ddlTemplateMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlTemplateMainSelectedIndexChanged(true);
    }

    protected void ddlTemplateMainSelectedIndexChanged(bool IsManualChange)
    {
        BindPermissionConfidentialUsers();
        if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
        {
            if (IsManualChange)
            {
                if ((common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                    && !common.myStr(ViewState["TagType"]).Equals("D")))
                {
                    DataSet dsRS = getEMRTemplateForSelectedService(common.myInt(Session["HospitalLocationId"]),
                                                                 common.myStr(Request.QueryString["TemplateRequiredServices"]).Trim(),
                                                                 common.myInt(ddlTemplateMain.SelectedValue));

                    if (!dsRS.Tables[0].Columns.Contains("ServiceOrderDetailId"))
                    {
                        DataColumn colRS = new DataColumn("ServiceOrderDetailId");
                        dsRS.Tables[0].Columns.Add(colRS);
                    }

                    gvTemplateRequiredServices.DataSource = dsRS.Tables[0];
                    gvTemplateRequiredServices.DataBind();

                    bool isFirst = false;
                    foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                    {
                        CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                        chkRow.Checked = true;

                        if (!isFirst)
                        {
                            isFirst = true;

                            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");
                            ViewState["TemplateRequiredServiceId"] = common.myInt(hdnServiceId.Value).ToString();
                        }
                    }

                    setRequestSelectedServiceColor();
                }

                setServiceChkVisiblity();
            }

            ViewState["SelectedCategory"] = null;
            ViewState["ResultSetId"] = "0";
            //txtResultSet.Text = string.Empty;
            tvCategory.CollapseAll();
            tblReport.Visible = true;
            //btnSetDefault.Visible = true;
            btnAddRow.Visible = true;
            btnFormulaCalculate.Visible = true;

            // btnSave.Visible = !common.myBool(ViewState["IsEncounterClose"]);

            if ((common.myBool(ViewState["IsEncounterClose"])) && (!common.myBool(Session["isEMRSuperUser"])))
            {
                btnSave.Visible = false;
            }
            else
            {
                btnSave.Visible = true;
            }

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Session["CurrentNode"] = "P" + common.myInt(ddlTemplateMain.SelectedValue);
            ViewState["PageId"] = common.myStr(ddlTemplateMain.SelectedValue);

            ViewState["EntryType"] = common.myStr(ddlTemplateMain.SelectedItem.Attributes["EntryType"]).Trim();
            ViewState["EntryType"] = common.myStr(ViewState["EntryType"]).Equals(string.Empty) ? "V" : common.myStr(ViewState["EntryType"]);

            ViewState["TemplateTypeID"] = common.myInt(ddlTemplateMain.SelectedItem.Attributes["TemplateTypeID"]).ToString();

            switch (common.myInt(ViewState["TemplateTypeID"]))
            {
                case 1:
                    lblHeading.Text = "Patient Alert";
                    break;
                case 2:
                    lblHeading.Text = "OT";
                    break;
                case 3:
                    lblHeading.Text = "Patient History";
                    break;
                case 4:
                    lblHeading.Text = "Emergency";
                    break;
                case 5:
                    lblHeading.Text = "Department Registers";
                    break;
                default:
                    lblHeading.Text = "Request Form";
                    break;
            }

            if (common.myStr(ViewState["EntryType"]).Equals("M"))
            {
                ddlRecord.Visible = true;
                Label1.Visible = true;
                btnNewRecord.Visible = true;
                //btnViewHistroy.Visible = true;
            }
            else
            {
                ddlRecord.Visible = false;
                Label1.Visible = false;
                btnNewRecord.Visible = false;
                //btnViewHistroy.Visible = false;
            }

            if (common.myStr(ViewState["EntryType"]).Equals("E"))
            {
                //tblEpisode.Visible = true;
                if (ddlRecord.Visible)
                {
                    tblEpisode.Attributes.Add("style", "display:none");
                }
                else
                {
                    tblEpisode.Attributes.Add("style", "display:inline-block");
                }
                divEpisode.Attributes.Add("class", "col-md-12");
                divEpisodeControl.Attributes.Add("class", "PatientHistoryText01b");
            }
            else
            {
                if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE").Equals(true)
                    && common.myBool(Session["isEMRSuperUser"]).Equals(true))
                {
                    //tblEpisode.Visible = true;
                    if (ddlRecord.Visible)
                    {
                        tblEpisode.Attributes.Add("style", "display:none");
                    }
                    else
                    {
                        tblEpisode.Attributes.Add("style", "display:inline-block");
                    }
                    divEpisode.Attributes.Add("class", "col-md-12");
                }
                else
                {
                    // tblEpisode.Visible = false;
                    tblEpisode.Attributes.Add("style", "display:none");
                    //divEpisode.Attributes.Add("class", "hide");
                }

                divEpisodeControl.Attributes.Add("class", "hide");
            }

            bindVisitRecord(false);

            bindResultSet();

            bindEpisode();

            Select_Lock_Unlock();
            if (common.myInt(ViewState["ServiceId"]) > 0)
            {
                btnSave.Visible = true;
                btnClose.Visible = true;
                btnNew.Visible = false;
                //btnCaseSheet.Visible = false;
            }

            BindCategoryTree();

            if (tvCategory.Nodes.Count > 0)
            {
                isFirstTime = true;
                tvCategory_SelectedNodeChanged(null, null);
                isFirstTime = false;

                //showgridcontrols();

                ///tvCategory.ExpandAll();

                if (!common.myInt(ViewState["EncounterId"]).Equals(0)
                    && !common.myInt(ViewState["RegistrationId"]).Equals(0)
                    && !common.myInt(Session["FormID"]).Equals(0))
                {
                    string pullvalue = common.myStr(dl.ExecuteScalar(CommandType.Text, "SELECT PullForward FROM EMRPatientForms epf WITH (NOLOCK) INNER JOIN EMRPatientFormDetails epfd WITH (NOLOCK) ON epf.PatientFormId = epfd.PatientFormId AND epfd.TemplateId = " + common.myInt(ViewState["PageId"]) + " WHERE epf.EncounterId = " + common.myInt(ViewState["EncounterId"]) + " AND epf.RegistrationId = " + common.myInt(ViewState["RegistrationId"]) + " AND epf.Active = 1 "));
                    chkPullForward.Checked = pullvalue.Equals(string.Empty) ? false : common.myBool(pullvalue);
                }

                bindReportList();

                visiblilityResultSet(true);
            }
            else
            {
                visiblilityResultSet(false);
                lblTemplateName.Text = "No Data Found.";
            }
            //Added by rakesh start
            settingControls(common.myStr(ViewState["CF"]));
            settingControls(common.myStr(ViewState["Source"]));
            if (ddlTemplateMain.SelectedValue != "")
            {
                if (common.myBool(ddlTemplateMain.SelectedItem.Attributes["IsConfidential"]))
                {
                    ViewState["ConfidentialTemplate"] = true;
                }
            }
            if (ddlTemplatePatient.SelectedValue != "")
            {
                if (common.myBool(ddlTemplatePatient.SelectedItem.Attributes["IsConfidential"]))
                {
                    ViewState["ConfidentialTemplate"] = true;
                }
            }
            if (ddlTemplateMain.SelectedValue != "")
            {
                if (common.myBool(ViewState["DisplayIsConfidentialLink"]))
                {
                    lnkViewConfidential.Visible = true;
                }
                else
                {
                    lnkViewConfidential.Visible = false;
                }
                if (common.myBool(ddlTemplateMain.SelectedItem.Attributes["IsConfidential"]))
                {
                    lblConfidentail.Visible = true;
                }
                else
                {
                    lblConfidentail.Visible = false;
                }
            }
            if (ddlTemplatePatient.SelectedValue != "")
            {
                if (common.myBool(ViewState["DisplayIsConfidentialLink"]))
                {
                    lnkViewConfidential.Visible = true;
                }
                else
                {
                    lnkViewConfidential.Visible = false;

                }
                if (common.myBool(ddlTemplatePatient.SelectedItem.Attributes["IsConfidential"]))
                {
                    lblConfidentail.Visible = true;
                }
                else
                {
                    lblConfidentail.Visible = false;
                }
            }
        }
        //Added by rakesh start
        else
        {
            ViewState["NonTabularData"] = null;

            gvSelectedServices.DataSource = null;
            gvSelectedServices.DataBind();
            //gvTabularFormat.DataSource = null;
            //gvTabularFormat.DataBind();
            tvCategory.Nodes.Clear();
            visiblilityResultSet(false);
            lblTemplateName.Text = "No Data Found.";
            if (tvCategory.SelectedNode != null)
                lblGrid.Text = string.Empty;
        }
        //Added by rakesh end
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }





    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }

    //protected void btnRefreshPatientTemplate_OnClick(Object sender, EventArgs e)
    //{
    //    bindPatientTemplateList(true);
    //}






    protected void ddlEpisode_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        btnEpisodeStart.Visible = true;
        btnEpisodeClose.Visible = false;
        btnEpisodeCancel.Visible = false;

        btnSave.Visible = true;
        visiblilityResultSet(true);

        if (common.myInt(ddlEpisode.SelectedValue) > 0)
        {
            if (!common.myBool(ddlEpisode.SelectedItem.Attributes["EpisodeClosed"]))
            {
                btnEpisodeStart.Visible = false;
                btnEpisodeClose.Visible = true;
                btnEpisodeCancel.Visible = true;


                BindCategoryTree();

                if (tvCategory.Nodes.Count > 0)
                {
                    isFirstTime = true;
                    tvCategory_SelectedNodeChanged(sender, e);
                    isFirstTime = false;

                    tvCategory.ExpandAll();

                    //bindReportList();
                }
            }
            else
            {
                btnSave.Visible = false;
                visiblilityResultSet(false);
            }
        }
        //}
        //catch (Exception Ex)
        //{
        //    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    lblMessage.Text = "Error: " + Ex.Message;
        //    objException.HandleException(Ex);
        //}
    }

    protected void btnEpisodeStart_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            DataSet dsEpisode = new DataSet();
            dsEpisode = objEMR.getPatientClosedEpisode(Convert.ToInt16(ddlTemplateMain.SelectedValue), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
            if (dsEpisode.Tables[0] != null && dsEpisode.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(dsEpisode.Tables[0].Rows[0]["EpisodeClosed"]))
                {
                    string strMsg = objEMR.CreateTemplateEpisode(0,
                                            common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                                            common.myInt(ViewState["PageId"]), "N", 0, 1, common.myInt(Session["UserId"]));

                    if ((strMsg.ToUpper().Contains(" CREATED")) && !strMsg.ToUpper().Contains("USP"))
                    {
                        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                        lblMessage.Text = strMsg;

                        bindEpisode();
                    }
                }
                else
                {
                    Alert.ShowAjaxMsg("Episode already open for this patient.", Page);
                    return;
                }
            }
            else
            {
                string strMsg = objEMR.CreateTemplateEpisode(0,
                                            common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                                            common.myInt(ViewState["PageId"]), "N", 0, 1, common.myInt(Session["UserId"]));

                if ((strMsg.ToUpper().Contains(" CREATED")) && !strMsg.ToUpper().Contains("USP"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = strMsg;

                    bindEpisode();
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

    protected void btnEpisodeClose_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            if (common.myInt(ddlEpisode.SelectedValue).Equals(0))
            {
                lblMessage.Text = "Episode not selected !";
                return;
            }

            string strMsg = objEMR.CreateTemplateEpisode(common.myInt(ddlEpisode.SelectedValue),
                                    common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                                    common.myInt(ViewState["PageId"]), string.Empty, 1, 1, common.myInt(Session["UserId"]));

            if ((strMsg.ToUpper().Contains(" CLOSED")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;

                bindEpisode();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnEpisodeCancel_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            if (common.myInt(ddlEpisode.SelectedValue).Equals(0))
            {
                lblMessage.Text = "Episode not selected !";
                return;
            }

            string strMsg = objEMR.CreateTemplateEpisode(common.myInt(ddlEpisode.SelectedValue),
                                    common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]),
                                    common.myInt(ViewState["PageId"]), string.Empty, 0, 0, common.myInt(Session["UserId"]));

            if ((strMsg.ToUpper().Contains(" CANCELLED")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;

                bindEpisode();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    private void BindAdviserDoctor()
    {
        BaseC.clsLISMaster objlis = new BaseC.clsLISMaster(sConString);
        DataTable tbl = new DataTable();
        try
        {
            tbl = objlis.getDoctorList(0, "", common.myInt(Session["HospitalLocationID"]), 0, common.myInt(Session["FacilityId"]), 0);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                ddlAdvisingDoctor.DataSource = tbl;
                ddlAdvisingDoctor.DataTextField = "DoctorName";
                ddlAdvisingDoctor.DataValueField = "DoctorId";
                ddlAdvisingDoctor.DataBind();
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
        }
        finally { objlis = null; tbl = null; }
    }
    private void bindEpisode()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            DataSet ds = new DataSet();
            ds = objEMR.UspEMRGetTemplateDetailsisApprovel(common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), common.myInt(ddlTemplateMain.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkApprovalRequired.Checked = true;

                lblAdvisingDoctor.Visible = true;
                ddlAdvisingDoctor.Visible = true;
                bool Approved = common.myBool(ds.Tables[0].Rows[0]["IsApproved"]);
                if (Approved)
                {
                    chkApprovalRequired.Enabled = false;
                    lblAdvisingDoctor.Enabled = false;
                    ddlAdvisingDoctor.Enabled = false;
                    chkApprovalRequired.ToolTip = "Order Approved";
                }
            }
            else
            {
                chkApprovalRequired.Checked = false;
                lblAdvisingDoctor.Visible = false;
                ddlAdvisingDoctor.Visible = false;
            }
            ds.Dispose();

            ds = objEMR.getTemplateEpisode(common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["PageId"]));

            ddlEpisode.Items.Clear();

            RadComboBoxItem item;

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string sEpisode = common.myStr(dr["EpisodeClosed"]).ToUpper() == "TRUE" ? "( Closed )" : "";
                    item = new RadComboBoxItem();

                    item.Text = common.myStr(dr["EpisodeStartDate"]) + sEpisode;

                    item.Value = common.myInt(dr["EpisodeId"]).ToString();
                    item.Attributes.Add("EpisodeClosed", common.myStr(dr["EpisodeClosed"]));

                    item.DataBind();
                    ddlEpisode.Items.Add(item);
                }
                ddlEpisode.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
                ddlEpisode.SelectedIndex = 1;
                ddlEpisode_SelectedIndexChanged(this, null);
            }

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnOPVisit_Click(object sender, EventArgs e)
    {
        if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
        {
            Session["formId"] = "1";
            RadWindowForNew.NavigateUrl = "~/EMR/Masters/PatientHistory.aspx?From=POPUP&RegId=" + common.myInt(ViewState["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]);
            RadWindowForNew.Height = 550;
            RadWindowForNew.Width = 800;
            RadWindowForNew.Top = 10;
            RadWindowForNew.Left = 10;
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.VisibleStatusbar = false;
            RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
        }
    }
    //protected void btnCaseSheet_Click(object sender, EventArgs e)
    //{
    //    if (common.myStr(Request.QueryString["From"]).ToUpper().Equals("POPUP"))
    //    {
    //        Session["formId"] = "1";
    //        //RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&ER=" +Request.QueryString["ER"].ToString();
    //        RadWindowForNew.NavigateUrl = "~/Editor/WordProcessor.aspx?From=POPUP&ER=" + common.myStr(Request.QueryString["ER"]) + "&EREncounterId=" + common.myStr(Request.QueryString["EREncounterId"])
    //             + "&AdmissionDate=" + Convert.ToDateTime(Request.QueryString["AdmissionDate"]).ToString("MM/dd/yyyy");
    //        RadWindowForNew.Height = 550;
    //        RadWindowForNew.Width = 800;
    //        RadWindowForNew.Top = 10;
    //        RadWindowForNew.Left = 10;
    //        //RadWindowForNew.OnClientClose = "OnCloseChangeMenuItem";//
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.VisibleStatusbar = false;
    //        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        //Response.Redirect("", false);

    //        if (btnAddRow.Visible)
    //        {
    //            isFirstTime = true;
    //            tvCategory_SelectedNodeChanged(sender, e);
    //            isFirstTime = false;
    //        }
    //    }
    //    else
    //    {
    //        Response.Redirect("~/Editor/WordProcessor.aspx", false);
    //    }
    //}

    //protected void lnkAlerts_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblMessage.Text = string.Empty;

    //        RadWindowPrint.NavigateUrl = "~/MPages/CommonGridPage.aspx?MP=NO&CF=PTA&PId=" + common.myInt(ViewState["RegistrationId"]);
    //        RadWindowPrint.Height = 600;
    //        RadWindowPrint.Width = 600;
    //        RadWindowPrint.Top = 10;
    //        RadWindowPrint.Left = 10;
    //        RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowPrint.Modal = true;
    //        RadWindowPrint.VisibleStatusbar = false;
    //        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }
    //}

    //protected void lnkPatientImmunization_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblMessage.Text = string.Empty;
    //        RadWindowPrint.NavigateUrl = "~/EMR/Immunization/ImmunizationBabyDueDate.aspx?From=POPUP";
    //        RadWindowPrint.Height = 600;
    //        RadWindowPrint.Width = 600;
    //        RadWindowPrint.Top = 10;
    //        RadWindowPrint.Left = 10;
    //        RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowPrint.Modal = true;
    //        RadWindowPrint.VisibleStatusbar = false;
    //        RadWindowPrint.InitialBehavior = WindowBehaviors.Maximize;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;

    //        objException.HandleException(Ex);
    //    }

    //}



    private void bindPatientHistroy()
    {

        // ViewState["EncounterId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnEncounterId")).Value).Trim();
        //ViewState["RegistrationId"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
        //Session["RegistrationID"] = common.myStr(((HiddenField)e.Item.FindControl("hdnRegistrationId")).Value).Trim();
        //if (common.myStr(ViewState["EncounterId"]) != "" && common.myStr(ViewState["RegistrationId"]) != "")
        //common.myInt(Session["RegistrationId"]

        if (!common.myInt(ViewState["EncounterId"]).Equals(0))
        {

            //RadWindow3.NavigateUrl = "/EMR/Masters/ViewPatientHistorybySession.aspx?RegId=" + common.myStr(Session["RegistrationId"]) + "&EncId=" + common.myInt(Session["encounterid"]) + "";
            RadWindowForNew.NavigateUrl = "/Editor/WordProcessorSessionWise.aspx?MP=NO";
            RadWindowForNew.Height = 500;
            RadWindowForNew.Width = 600;
            RadWindowForNew.Top = 20;
            RadWindowForNew.Left = 20;
            // RadWindowForNew.Title = "Time Slot";
            // RadWindow3.OnClientClose = "OnClientClose";
            RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindowForNew.Modal = true;
            RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindowForNew.VisibleStatusbar = false;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please Select Patient!";
        }
    }

    protected void btnMandatorySave_OnClick(object sender, EventArgs e)
    {
        try
        {
            //tvCategory.SelectedNode.Value = common.myStr(ViewState["PreviousSelectedNode"]);

            //tvCategory.SelectedNodeStyle.BackColor = System.Drawing.Color.Gray;
            //tvCategory.SelectedNodeStyle.ForeColor = System.Drawing.Color.White;

            ArrayList arr = GetSelectedNode(common.myStr(ViewState["PreviousSelectedNode"]));

            saveData(common.myBool(ViewState["isSaveManual"]));

            ViewState["PreviousSelectedNode"] = string.Empty;

            dvMandatory.Visible = false;

            tvCategory_SelectedNodeChanged(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    protected void btnMandatoryOk_OnClick(object sender, EventArgs e)
    {
        dvMandatory.Visible = false;
    }

    protected void btnMandatoryCancel_OnClick(object sender, EventArgs e)
    {
        dvMandatory.Visible = false;
    }
    private void settingControls(string source)
    {
        if (source.Equals("LAB"))
        {
            //Label2.Visible = false;
            //ddlTemplatePatient.Visible = false;
            //spnTemplatePatient.Visible = false;
            //lnkAlerts.Visible = false;
            //btnRefreshPatientTemplate.Visible = false;
            lblTemplateName.Visible = false;
            ddlcase.Visible = false;
            //btnSaveAs.Visible = false;
            //txtResultSet.Visible = false;
            //lblResultSet.Visible = false;
            //ddlResultSet.Visible = false;
            //btnDeleteResultSet.Visible = false;
            btnSave.Visible = false;
            //label1.Visible = false;
            //ddlReport.Visible = false;
            //ibtnReportSetup.Visible = false;
            //btnPrintReport.Visible = false;

            if (common.myStr(ViewState["CF"]).Equals("LAB"))
            {
                btnSave.Visible = true;
            }

            if (ddlTemplateMain.Items.Count > 0)
            {
                if (common.myInt(ddlTemplateMain.SelectedIndex).Equals(-1))
                {
                    ddlTemplateMain.SelectedIndex = 0;
                    ddlTemplateMainSelectedIndexChanged(false);
                }
            }
        }
        else if (source.Equals("ProcedureOrder"))
        {
            //Label2.Visible = false;
            //ddlTemplatePatient.Visible = false;
            //spnTemplatePatient.Visible = false;
            //lnkAlerts.Visible = false;
            //btnRefreshPatientTemplate.Visible = false;
            lblTemplateName.Visible = false;
            ddlcase.Visible = false;
            //btnSaveAs.Visible = false;
            //txtResultSet.Visible = false;
            //lblResultSet.Visible = false;
            //ddlResultSet.Visible = false;
            //btnDeleteResultSet.Visible = false;
            //label1.Visible = false;
            //ddlReport.Visible = false;
            //ibtnReportSetup.Visible = false;
            //btnPrintReport.Visible = false;
            //btnCaseSheet.Visible = false;
            btnNew.Visible = false;
            btnClose.Visible = true;
            btnSave.Visible = true;

            if (!common.myStr(ViewState["TagType"]).Equals("D"))
            {
                if (common.myInt(ViewState["TemplateRequiredServiceId"]).Equals(0))
                {
                    bool isFirst = false;
                    foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                    {
                        CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                        chkRow.Checked = true;

                        if (!isFirst)
                        {
                            isFirst = true;

                            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");
                            ViewState["TemplateRequiredServiceId"] = common.myInt(hdnServiceId.Value).ToString();
                        }

                        break;
                    }

                    if (common.myStr(Request.QueryString["RList"]).Equals("RL"))// when page comming from request list 
                    {
                        ViewState["TemplateRequiredServiceId"] = common.myInt(Request.QueryString["SubDepId"]).ToString();

                        foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                        {
                            CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");

                            if (common.myInt(hdnServiceId.Value).Equals(common.myInt(Request.QueryString["SubDepId"])))
                            {
                                chkRow.Checked = true;
                                ViewState["TemplateRequiredServiceId"] = common.myInt(hdnServiceId.Value).ToString(); //Sub-department Id
                                break;
                            }
                        }
                        gvTemplateRequiredServices.Enabled = false;
                    }

                    setRequestSelectedServiceColor();

                    //ddlTemplateRequiredServices_SelectedIndexChanged(null, null);
                    selectTemplateRequiredSerices(common.myStr(ViewState["TemplateRequiredServiceId"]));
                }
            }
            else
            {
                if (ddlTemplateRequiredServices.Items.Count > 0)
                {
                    if (common.myInt(ddlTemplateRequiredServices.SelectedIndex).Equals(-1))
                    {
                        ddlTemplateRequiredServices.SelectedIndex = 0;
                        if (common.myStr(Request.QueryString["RList"]).Equals("RL"))// when page comming from request list 
                        {
                            ddlTemplateRequiredServices.SelectedIndex = ddlTemplateRequiredServices.Items.IndexOf(ddlTemplateRequiredServices.Items.FindItemByValue(common.myInt(Request.QueryString["SubDepId"]).ToString()));// Sub- department Id
                            ddlTemplateRequiredServices.Enabled = false;
                        }
                        ddlTemplateRequiredServices_SelectedIndexChanged(null, null);
                    }
                }
            }

            if (ddlTemplateMain.Items.Count > 0)
            {
                if (common.myInt(ddlTemplateMain.SelectedIndex).Equals(-1))
                {
                    ddlTemplateMain.SelectedIndex = 0;
                    ddlTemplateMainSelectedIndexChanged(false);
                }
            }

        }
        else if (source.Equals("DeptRegister"))
        {
            //Label2.Visible = false;
            //ddlTemplatePatient.Visible = false;
            //spnTemplatePatient.Visible = false;
            //lnkAlerts.Visible = false;
            //btnRefreshPatientTemplate.Visible = false;
            lblTemplateName.Visible = false;
            ddlcase.Visible = false;
            //btnSaveAs.Visible = false;
            //txtResultSet.Visible = false;
            //lblResultSet.Visible = false;
            //ddlResultSet.Visible = false;
            //btnDeleteResultSet.Visible = false;
            //label1.Visible = false;
            //ddlReport.Visible = false;
            //ibtnReportSetup.Visible = false;
            //btnPrintReport.Visible = false;
            //btnCaseSheet.Visible = false;
            btnNew.Visible = false;
            btnClose.Visible = false;
            btnSave.Visible = true;

            if (ddlTemplateRequiredServices.Items.Count > 0)
            {
                if (common.myInt(ddlTemplateRequiredServices.SelectedIndex).Equals(-1))
                {
                    ddlTemplateRequiredServices.SelectedIndex = 0;
                    if (common.myStr(Request.QueryString["RList"]).Equals("RL"))// when page is comming from order page on request list button 
                    {
                        ddlTemplateRequiredServices.SelectedIndex = ddlTemplateRequiredServices.SelectedIndex = ddlTemplateRequiredServices.Items.IndexOf(ddlTemplateRequiredServices.Items.FindItemByValue(common.myInt(Request.QueryString["SubDepId"]).ToString()));// Sub- department Id
                        ddlTemplateRequiredServices.Enabled = false;
                    }
                    ddlTemplateRequiredServices_SelectedIndexChanged(null, null);
                }
            }

            if (ddlTemplateMain.Items.Count > 0)
            {
                if (common.myInt(ddlTemplateMain.SelectedIndex).Equals(-1))
                {
                    ddlTemplateMain.SelectedIndex = 0;
                    ddlTemplateMain_SelectedIndexChanged(null, null);
                }
            }
        }

    }


    private void BindTemplateRequiredService(string ServiceIds)
    {
        DataSet ds = new DataSet();
        Hashtable hstInput = new Hashtable();

        try
        {
            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                && common.myStr(ViewState["TagType"]).Equals("D"))
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                hstInput.Add("@xmlServiceOrderDetailIds", ServiceIds);
                hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                hstInput.Add("@chkTagType", common.myStr(ViewState["TagType"]));

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspTemplateRequiredServices", hstInput);

                if (ds.Tables.Count > 0)
                {
                    DataView view = new DataView();
                    view.Table = ds.Tables[0];

                    ddlTemplateMain.Items.Clear();
                    ddlTemplateMain.Visible = true;

                    RadComboBoxItem item;
                    if (view.ToTable().Rows.Count > 0)
                    {
                        DataView dv = ((DataTable)view.ToTable()).DefaultView;

                        foreach (DataRow dr in dv.ToTable().Rows)
                        {
                            item = new RadComboBoxItem();
                            item.Text = common.myStr(dr["ServiceName"]);
                            item.Value = common.myInt(dr["ServiceId"]).ToString();
                            item.Attributes.Add("ServiceOrderDetailId", common.myStr(dr["ServiceOrderDetailId"]));

                            ddlTemplateRequiredServices.Items.Add(item);
                            ddlTemplateRequiredServices.DataBind();

                            ddlTemplateRequiredServices.Visible = true;
                            lblServices.Visible = true;
                            ddlTemplateMain.Items.Clear();
                        }
                    }
                }
            }
            else
            {
                if (common.myLen(ServiceIds).Equals(0))
                {
                    return;
                }

                //BaseC.EMROrders objO = new BaseC.EMROrders(sConString);
                //ds = objO.getItemOfService(ServiceIds, common.myInt(Session["HospitalLocationId"]));

                if (ddlTemplateMain.Items.Count > 0)
                {
                    ddlTemplateMain.Visible = true;
                    lblServices.Visible = false;
                    gvTemplateRequiredServices.Visible = true;

                    if (ddlTemplateMain.SelectedIndex < 0)
                    {
                        ddlTemplateMain.SelectedIndex = 0;
                    }
                    DataSet dsRS = getEMRTemplateForSelectedService(common.myInt(Session["HospitalLocationId"]),
                                                                 common.myStr(Request.QueryString["TemplateRequiredServices"]).Trim(),
                                                                 common.myInt(ddlTemplateMain.SelectedValue));

                    if (!dsRS.Tables[0].Columns.Contains("ServiceOrderDetailId"))
                    {
                        DataColumn colRS = new DataColumn("ServiceOrderDetailId");
                        dsRS.Tables[0].Columns.Add(colRS);
                    }

                    gvTemplateRequiredServices.DataSource = dsRS.Tables[0];
                    gvTemplateRequiredServices.DataBind();

                    bool isFirst = false;
                    foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                    {
                        CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                        chkRow.Checked = true;

                        if (!isFirst)
                        {
                            isFirst = true;

                            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");
                            ViewState["TemplateRequiredServiceId"] = common.myInt(hdnServiceId.Value).ToString();
                        }
                    }

                    setRequestSelectedServiceColor();
                }
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            ds.Dispose();
        }
    }

    private void bindMainTemplateListOfSelectedService(string ServiceIds)
    {
        try
        {
            DataSet ds = new DataSet();

            //ds = objivf.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), EmpType, iDoctorSpecialisation, iServiceId, common.myStr(Session["OPIP"]), TemplateIdType);
            ds = getEMRTemplateForSelectedService(common.myInt(Session["HospitalLocationId"]), ServiceIds, 0);
            // change by balkishan start
            DataView view = new DataView();
            view.Table = ds.Tables[0];

            ddlTemplateMain.SelectedIndex = -1;

            ddlTemplateMain.Items.Clear();
            ddlTemplateMain.Visible = true;

            RadComboBoxItem item;
            if (view.ToTable().Rows.Count > 0)
            {
                DataView dv = ((DataTable)view.ToTable()).DefaultView;
                if (common.myLen(ViewState["TypeName"]) > 0)
                    dv.RowFilter = "TypeName='" + common.myStr(ViewState["TypeName"]) + "'";

                foreach (DataRow dr in dv.ToTable().Rows)
                {
                    item = new RadComboBoxItem();
                    item.Text = common.myStr(dr["TemplateName"]);
                    item.Value = common.myInt(dr["TemplateId"]).ToString();
                    item.Attributes.Add("TemplateTypeID", common.myStr(dr["TemplateTypeID"]));
                    item.Attributes.Add("EntryType", common.myStr(dr["EntryType"]));
                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                    {
                        BaseC.EMROrders order = new BaseC.EMROrders(sConString);
                        int result = order.IsTemplateRequiredForService(2, common.myInt(ServiceIds), common.myInt(dr["TemplateId"]));
                        if (result.Equals(1))
                        {
                            item.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            item.ForeColor = System.Drawing.Color.Blue;
                        }
                    }

                    ddlTemplateMain.Items.Add(item);
                    //ddlTemplateMain.DataBind();
                }
                ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myInt(dv.ToTable().Rows[0]["TemplateId"]).ToString()));
            }
            bindPatientTemplateList(true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    public DataSet getEMRTemplateForSelectedService(int HospId, string ServiceIds, int TemplateId)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //string qry = "";
            string[] strService = ServiceIds.Split(',');
            ArrayList coll = new ArrayList();
            StringBuilder sbServiceIds = new StringBuilder();
            foreach (string item in strService)
            {
                coll.Add(item);
                sbServiceIds.Append(common.setXmlTable(ref coll));
            }
            HshIn.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
            if (TemplateId > 0)
            {
                HshIn.Add("@intServiceId", 0);
                HshIn.Add("@intSubDeptId", 0);
                HshIn.Add("@xmlServiceIds", sbServiceIds.ToString());
                HshIn.Add("@intTemplateId", common.myInt(TemplateId));
            }
            else
            {
                if (common.myStr(ViewState["TagType"]).Equals("D"))// Sub Department
                {
                    HshIn.Add("@intServiceId", 0);
                    HshIn.Add("@intSubDeptId", ServiceIds);
                }
                else // Service
                {
                    HshIn.Add("@intServiceId", 0);
                    HshIn.Add("@intSubDeptId", 0);
                    HshIn.Add("@xmlServiceIds", sbServiceIds.ToString());
                }
            }


            //if (iServiceId > 0)
            //{
            //    HshIn.Add("@iServiceId", iServiceId);

            //    qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName " +
            //          " FROM EMRTemplate et  left join EMRTemplateTypes t on t.ID = et.TemplateTypeID " +
            //          " INNER JOIN EMRTemplateServiceTagging es on es.TemplateId = et.id " +
            //          " WHERE et.Active = 1 " +
            //        //Added by rakesh start
            //          " AND es.Active = 1 " +
            //        //Added by rakesh end
            //          " AND es.Serviceid = @iServiceId " +
            //          " AND et.HospitalLocationID = @intHospId " +
            //          " ORDER BY et.TemplateName";
            //}

            //ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetServiceTemplates", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }


    protected void btnScoreCalc_Onclick(object sender, EventArgs e)
    {
        string strReferenceName = string.Empty;
        string strFormulaDefinition = string.Empty;
        bool boolTotalCalc = false;

        string strResult = string.Empty;

        int SectionId = 0;
        int FieldId = 0;
        string FielType = string.Empty;
        DataSet dsNonTabularData = new DataSet();
        DataTable tblNonTabularData = new DataTable();
        try
        {
            if (gvSelectedServices.Visible)
            {
                foreach (GridViewRow item2 in gvSelectedServices.Rows)
                {
                    if (item2.RowType == DataControlRowType.DataRow)
                    {
                        SectionId = common.myInt(item2.Cells[(byte)enumNonT.SectionId].Text);
                        FieldId = common.myInt(item2.Cells[(byte)enumNonT.FieldId].Text);
                        FielType = common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim();

                        if (FielType.Equals("T"))
                        {
                            TextBox txtT = (TextBox)item2.FindControl("txtT");

                            if (txtT.Visible)
                            {
                                if (ViewState["NonTabularData"] != null)
                                {
                                    dsNonTabularData = (DataSet)ViewState["NonTabularData"];

                                    tblNonTabularData = dsNonTabularData.Tables[0];

                                    if (tblNonTabularData != null)
                                    {
                                        if (tblNonTabularData.Rows.Count > 0)
                                        {
                                            DataView DV = tblNonTabularData.Copy().DefaultView;

                                            DV.RowFilter = "SectionId=" + SectionId + " AND FieldId=" + FieldId + " AND ISNULL(IsScoreCalc,0)=1";

                                            tblNonTabularData = DV.ToTable();
                                            if (tblNonTabularData.Rows.Count > 0)
                                            {
                                                DataRow DR = tblNonTabularData.Rows[0];

                                                strReferenceName = common.myStr(DR["ScoreReferenceName"]);
                                                strFormulaDefinition = common.myStr(DR["ScoreFormulaDefinition"]).Trim().ToUpper();
                                                boolTotalCalc = common.myBool(DR["IsScoreCalc"]);

                                                if (strFormulaDefinition.Length > 0)
                                                {
                                                    txtT.Text = string.Empty;

                                                    strResult = scoreCalc(strFormulaDefinition, SectionId);

                                                    if (!strResult.Equals(string.Empty) && !strResult.Equals("NaN"))
                                                    {
                                                        txtT.Text = common.myDec(Decimal.Round(common.myDec(strResult), 2)).ToString("F0");
                                                    }
                                                }

                                                if (boolTotalCalc)
                                                {
                                                    txtT.Style["text-align"] = "right";
                                                    txtT.Width = Unit.Pixel(70);
                                                    txtT.BorderWidth = Unit.Pixel(2);
                                                    txtT.Font.Bold = true;
                                                    txtT.ForeColor = System.Drawing.Color.Navy;
                                                    txtT.Visible = true;
                                                }
                                            }

                                            DV.RowFilter = string.Empty;
                                        }
                                    }
                                }
                            }
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
        finally
        {
            dsNonTabularData.Dispose();
            tblNonTabularData.Dispose();
        }
    }

    private string scoreCalc(string strFormulaDefinition, int intSectionId)
    {
        string strReferenceName = string.Empty;
        string strRefNames = string.Empty;
        string strResult = string.Empty;

        int SectionId = 0;
        bool IsValueEntered = false;

        //string input = "(A+B)/10";
        //string val = Regex.Replace(input, "A", "20", RegexOptions.IgnoreCase);
        //bool isv = Regex.IsMatch(input, @"[A-Z]");
        //string sss = getAllLetters("((A+B)/10)*C");

        DataSet dsNonTabularData = new DataSet();
        DataView dvFieldValue = new DataView();
        DataTable tblFieldValue = new DataTable();
        DataTable tblRefField = new DataTable();
        try
        {
            strRefNames = getAllLetters(strFormulaDefinition);

            if (common.myLen(strRefNames) > 0)
            {
                if (ViewState["NonTabularData"] != null)
                {
                    dsNonTabularData = (DataSet)ViewState["NonTabularData"];

                    if (dsNonTabularData.Tables.Count > 0)
                    {
                        tblRefField = dsNonTabularData.Tables[0];

                        if (tblRefField != null)
                        {
                            if (tblRefField.Rows.Count > 0)
                            {
                                DataView DV = tblRefField.Copy().DefaultView;

                                DV.RowFilter = "SectionId =" + intSectionId + " and ScoreReferenceName IN(" + strRefNames + ")";

                                tblRefField = DV.ToTable();
                                if (tblRefField.Rows.Count > 0)
                                {
                                    if (dsNonTabularData.Tables[1].Rows.Count > 0)
                                    {
                                        dvFieldValue = dsNonTabularData.Tables[1].Copy().DefaultView;

                                        foreach (DataRow drField in tblRefField.Rows)
                                        {
                                            if (tblRefField.DefaultView.Count > 0)
                                            {
                                                strReferenceName = common.myStr(drField["ScoreReferenceName"]).Trim();

                                                if (common.myLen(strReferenceName) > 0)
                                                {
                                                    int FieldId = 0;
                                                    string FieldType = string.Empty;
                                                    string FieldValue = string.Empty;

                                                    foreach (GridViewRow item2 in gvSelectedServices.Rows)
                                                    {
                                                        if (item2.RowType == DataControlRowType.DataRow)
                                                        {
                                                            SectionId = common.myInt(item2.Cells[(byte)enumNonT.SectionId].Text);
                                                            FieldId = common.myInt(item2.Cells[(byte)enumNonT.FieldId].Text);
                                                            FieldType = common.myStr(item2.Cells[(byte)enumNonT.FieldType].Text).Trim();

                                                            if (FieldType.Equals("R"))
                                                            {
                                                                if (intSectionId.Equals(SectionId) && common.myInt(drField["FieldId"]).Equals(FieldId))
                                                                {
                                                                    RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");

                                                                    dvFieldValue.RowFilter = "FieldId=" + common.myInt(drField["FieldId"]) + " AND ValueId=" + common.myInt(ddl.SelectedValue).ToString();

                                                                    tblFieldValue = dvFieldValue.ToTable();

                                                                    if (tblFieldValue.Rows.Count > 0)
                                                                    {
                                                                        FieldValue = common.myDec(tblFieldValue.Rows[0]["ScoreValue"]).ToString();
                                                                        break;
                                                                    }

                                                                }
                                                            }
                                                            else if (FieldType.Equals("T"))
                                                            {
                                                                if (intSectionId.Equals(SectionId) && common.myInt(drField["FieldId"]).Equals(FieldId))
                                                                {
                                                                    TextBox txtT = (TextBox)item2.FindControl("txtT");

                                                                    if (!txtT.Text.Trim().Equals(string.Empty))
                                                                    {
                                                                        FieldValue = common.myDec(txtT.Text).ToString();
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    if (!FieldValue.Trim().Equals(string.Empty))
                                                    {
                                                        IsValueEntered = true;
                                                        strFormulaDefinition = Regex.Replace(strFormulaDefinition, strReferenceName, common.myDec(FieldValue).ToString(), RegexOptions.IgnoreCase);
                                                    }
                                                    else
                                                    {
                                                        if (!Regex.IsMatch(strFormulaDefinition, @"[/]")) //Operator if have +, -, * not allow /
                                                        {
                                                            strFormulaDefinition = Regex.Replace(strFormulaDefinition, strReferenceName, 0.ToString(), RegexOptions.IgnoreCase);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!Regex.IsMatch(strFormulaDefinition, @"[A-Z]"))
            {
                strResult = common.myStr(Evaluate(strFormulaDefinition));

                if (common.myDbl(strResult).Equals(0))
                {
                    if (!IsValueEntered)
                    {
                        strResult = string.Empty;
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
            dsNonTabularData.Dispose();
            dvFieldValue.Dispose();
            tblFieldValue.Dispose();
            tblRefField.Dispose();
        }
        return strResult;
    }

    protected void btnFormulaCalculate_OnClick(object sender, EventArgs e)
    {
        int flag = 0;
        int RowCaptionId = 0;
        int SectionId = 0;

        string strReferenceName = string.Empty;
        string strFormulaDefinition = string.Empty;
        bool boolTotalCalc = false;

        string strResult = string.Empty;
        int RowStartIndex = 1;

        try
        {
            if (gvTabularFormat.Visible)
            {
                for (int i = 0, k = 1; i < gvTabularFormat.Items.Count; i++)
                {
                    if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                    {
                        flag = 0;
                    }
                    else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                    {
                        k++;
                        if (flag.Equals(0))
                        {
                            ViewState["ValueOfControlType"] = i - 1;
                            flag = 1;
                        }

                        RowCaptionId = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.RowCaptionId].Text);
                        SectionId = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text);

                        int FieldId;
                        string FieldType;
                        string FieldValue;

                        for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++)
                        {
                            FieldId = 0;
                            FieldType = string.Empty;
                            FieldValue = string.Empty;

                            Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                            if (common.myInt(lblControlType.Text).Equals(0))
                            {
                                continue;
                            }

                            FieldId = common.myInt(lblControlType.Text);

                            lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j].FindControl("lblFieldId" + j.ToString());

                            FieldType = common.myStr(lblControlType.Text);

                            if (common.myStr(lblControlType.Text).Equals("T"))
                            {
                                TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());

                                if (!txtVisData.Text.Trim().Equals(string.Empty))
                                {
                                    FieldValue = txtVisData.Text;
                                }

                                if (ViewState["TabularMandatoryField"] != null)
                                {
                                    DataTable tbl = (DataTable)ViewState["TabularMandatoryField"];

                                    if (tbl != null)
                                    {
                                        if (tbl.Rows.Count > 0)
                                        {
                                            DataView DV = tbl.Copy().DefaultView;

                                            DV.RowFilter = "FieldId=" + FieldId;

                                            tbl = DV.ToTable();
                                            if (tbl.Rows.Count > 0)
                                            {
                                                DataRow DR = tbl.Rows[0];

                                                strReferenceName = common.myStr(DR["ReferenceName"]);
                                                strFormulaDefinition = common.myStr(DR["FormulaDefinition"]).Trim().ToUpper();
                                                boolTotalCalc = common.myBool(DR["TotalCalc"]);

                                                if (strFormulaDefinition.Length > 0)
                                                {
                                                    txtVisData.Text = string.Empty;

                                                    strResult = formulaCalc(strFormulaDefinition, i);

                                                    if (!strResult.Equals(string.Empty) && !strResult.Equals("NaN"))
                                                    {
                                                        txtVisData.Text = common.myDec(Decimal.Round(common.myDec(strResult), 2)).ToString("F2");
                                                    }
                                                }

                                                if (boolTotalCalc)
                                                {
                                                    GridFooterItem footerItem = gvTabularFormat.MasterTableView.GetItems(GridItemType.Footer)[0] as GridFooterItem;

                                                    Label lblTfooterTotal = (Label)footerItem.Cells[j].FindControl("lblT" + j.ToString());

                                                    // Label lblTfooterTotal = (Label)gvTabularFormat.FooterRow.FindControl("lblT" + j.ToString());

                                                    lblTfooterTotal.BorderWidth = Unit.Pixel(2);
                                                    lblTfooterTotal.Font.Bold = true;
                                                    lblTfooterTotal.ForeColor = System.Drawing.Color.Navy;
                                                    lblTfooterTotal.Visible = true;
                                                    if ((k - 1).Equals(1))
                                                    {
                                                        lblTfooterTotal.Text = "&nbsp;";
                                                    }

                                                    lblTfooterTotal.Text = common.myDec(Decimal.Round(common.myDec(common.myDec(lblTfooterTotal.Text) + common.myDec(txtVisData.Text)), 2)).ToString("F2");
                                                    lblTfooterTotal.ToolTip = "Total : " + lblTfooterTotal.Text;

                                                }

                                                //coll.Add(FieldId);//FieldId int,                                  
                                                //coll.Add(FieldType);//FieldType char(1),                              
                                                //coll.Add(FieldValue);//FieldValue varchar(Max),
                                                //coll.Add(k - 1);//RowNo int,
                                                //coll.Add(common.myInt(RowCaptionId));//RowCaptionId int

                                                //strTabular.Append(common.setXmlTable(ref coll));

                                                DV.RowFilter = string.Empty;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #region Display Formula Columns
                flag = 0;
                DataRow[] dr = null;

                dr = dtNoOfColumn.Select("SectionId=" + ViewState["SelectedNode"]);

                for (int i = 0; i < gvTabularFormat.Items.Count; i++)
                {
                    if (common.myInt(common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text).Trim()).Equals(common.myInt(ViewState["SelectedNode"])))
                    {
                        if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                        {
                            //gvTabularFormat.Rows[i].Visible = true;
                            if (flag.Equals(0))
                            {
                                RowStartIndex = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.Id].Text);
                            }

                            if (flag < 3)
                            {
                                gvTabularFormat.Items[i].Visible = false;
                            }
                        }
                        else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                        {
                            gvTabularFormat.Items[i].Visible = true;
                        }
                        flag++;
                    }
                    else
                    {
                        gvTabularFormat.Items[i].Visible = false;
                    }
                }

                for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++)
                {
                    Label lblHeading = (Label)gvTabularFormat.Items[RowStartIndex].Cells[j].FindControl("lblFieldId" + j.ToString());

                    GridHeaderItem headerItem = gvTabularFormat.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                    headerItem.Cells[j + 2].Text = common.myStr(lblHeading.Text);

                    DataTable tbl = new DataTable();
                    if (ViewState["TabularMandatoryField"] != null)
                    {
                        tbl = (DataTable)ViewState["TabularMandatoryField"];
                        if (tbl.Rows.Count > 0)
                        {
                            Label lblFieldId = (Label)gvTabularFormat.Items[RowStartIndex - 1].Cells[j].FindControl("lblFieldId" + j.ToString());

                            DataView DV = tbl.DefaultView;
                            DV.RowFilter = "FieldId=" + common.myInt(lblFieldId.Text) + " AND IsMandatory=1";

                            if (DV.ToTable().Rows.Count > 0)
                            {
                                headerItem.Cells[j + 2].Text = common.myStr(lblHeading.Text) + hdnMandatoryStar.Value;
                            }
                            DV.RowFilter = string.Empty;
                        }
                    }

                    headerItem.Cells[j].Font.Size = 10;

                    if (!lblHeading.Text.Trim().Equals("+"))
                    {
                        if (dr.Length > 0)
                        {
                            if (j >= common.myInt(dr[0][1]))
                            {
                                gvTabularFormat.Columns[j + 2].Visible = false;
                            }
                            else
                            {
                                Label lblControlType = (Label)gvTabularFormat.Items[2].Cells[j].FindControl("lblFieldId" + j.ToString());
                                if (lblControlType != null)
                                {
                                    if (common.myStr(lblControlType.Text).Equals("T"))
                                    {
                                        if (dtFieldMaxLength != null)
                                        {
                                            if (dtFieldMaxLength.Rows.Count > 0)
                                            {
                                                int FieldId = common.myInt(((Label)gvTabularFormat.Items[0].Cells[j].FindControl("lblFieldId" + j.ToString())).Text);
                                                dtFieldMaxLength.DefaultView.RowFilter = "FieldId=" + common.myInt(FieldId);
                                                if (dtFieldMaxLength.DefaultView.Count > 0)
                                                {
                                                    int maxVal = getPixelValue(common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]));

                                                    gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(maxVal);
                                                    gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(maxVal);
                                                }

                                                dtFieldMaxLength.DefaultView.RowFilter = "";
                                            }
                                        }
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("M"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(260);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(260);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("S"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(130);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(130);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("D"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(120);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(120);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("C"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(155);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(155);
                                    }
                                    else if (common.myStr(lblControlType.Text).Equals("W"))
                                    {
                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(500);
                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(500);
                                    }
                                }

                                gvTabularFormat.Columns[j + 2].Visible = true;
                                gvTabularFormat.Columns[j + 2].HeaderText = lblHeading.Text;
                                gvTabularFormat.Columns[j].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                                gvTabularFormat.Columns[j].ItemStyle.VerticalAlign = VerticalAlign.Top;
                            }
                        }
                        else
                        {
                            gvTabularFormat.Columns[j + 2].Visible = false;
                        }
                    }
                    else
                    {
                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(150);
                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(150);
                    }
                }

                #endregion
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private string formulaCalc(string strFormulaDefinition, int intRowIdx)
    {
        string strReferenceName = string.Empty;
        string strRefNames = string.Empty;
        string strResult = string.Empty;

        int flag = 0;
        int RowCaptionId = 0;
        int SectionId = 0;
        bool IsValueEntered = false;

        //string input = "(A+B)/10";
        //string val = Regex.Replace(input, "A", "20", RegexOptions.IgnoreCase);
        //bool isv = Regex.IsMatch(input, @"[A-Z]");
        //string sss = getAllLetters("((A+B)/10)*C");

        try
        {
            strRefNames = getAllLetters(strFormulaDefinition);

            if (common.myLen(strRefNames) > 0)
            {
                if (ViewState["TabularMandatoryField"] != null)
                {
                    DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];

                    if (tblRefField != null)
                    {
                        if (tblRefField.Rows.Count > 0)
                        {
                            DataView DV = tblRefField.Copy().DefaultView;

                            DV.RowFilter = "ReferenceName IN(" + strRefNames + ")";

                            tblRefField = DV.ToTable();
                            if (tblRefField.Rows.Count > 0)
                            {
                                for (int i = 0, k = 1; i < (intRowIdx + 1); i++)
                                {
                                    if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                                    {
                                        flag = 0;
                                    }
                                    else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                                    {
                                        k++;
                                        if (flag.Equals(0))
                                        {
                                            ViewState["ValueOfControlType"] = i - 1;
                                            flag = 1;
                                        }

                                        if (i < intRowIdx)
                                        {
                                            continue;
                                        }

                                        RowCaptionId = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.RowCaptionId].Text);
                                        SectionId = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text);

                                        int FieldId;
                                        string FieldType;
                                        string FieldValue;

                                        for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++)
                                        {
                                            FieldId = 0;
                                            FieldType = string.Empty;
                                            FieldValue = string.Empty;

                                            Label lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"]) - 2].Cells[j].FindControl("lblFieldId" + j.ToString());

                                            if (common.myInt(lblControlType.Text).Equals(0))
                                            {
                                                continue;
                                            }

                                            FieldId = common.myInt(lblControlType.Text);

                                            lblControlType = (Label)gvTabularFormat.Items[common.myInt(ViewState["ValueOfControlType"])].Cells[j].FindControl("lblFieldId" + j.ToString());

                                            FieldType = common.myStr(lblControlType.Text);

                                            if (common.myStr(lblControlType.Text).Equals("T"))
                                            {
                                                TextBox txtVisData = (TextBox)gvTabularFormat.Items[i].Cells[j].FindControl("txtT" + j.ToString());

                                                if (!txtVisData.Text.Trim().Equals(string.Empty))
                                                {
                                                    FieldValue = txtVisData.Text;
                                                }

                                                tblRefField.DefaultView.RowFilter = "FieldId=" + FieldId;

                                                if (tblRefField.DefaultView.Count > 0)
                                                {
                                                    strReferenceName = common.myStr(tblRefField.DefaultView[0]["ReferenceName"]).Trim();

                                                    if (!FieldValue.Trim().Equals(string.Empty))
                                                    {
                                                        IsValueEntered = true;
                                                        strFormulaDefinition = Regex.Replace(strFormulaDefinition, strReferenceName, common.myDec(FieldValue).ToString(), RegexOptions.IgnoreCase);
                                                    }
                                                    else
                                                    {
                                                        if (!Regex.IsMatch(strFormulaDefinition, @"[/]")) //Operator if have +, -, * not allow /
                                                        {
                                                            strFormulaDefinition = Regex.Replace(strFormulaDefinition, strReferenceName, 0.ToString(), RegexOptions.IgnoreCase);
                                                        }
                                                    }
                                                }

                                                tblRefField.DefaultView.RowFilter = string.Empty;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }

            if (!Regex.IsMatch(strFormulaDefinition, @"[A-Z]"))
            {
                strResult = common.myStr(Evaluate(strFormulaDefinition));

                if (common.myDbl(strResult).Equals(0))
                {
                    if (!IsValueEntered)
                    {
                        strResult = string.Empty;
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
        return strResult;
    }

    static object Evaluate(string expression)
    {
        object dblResult = null;
        try
        {
            DataTable dt = new DataTable();
            dblResult = dt.Compute(expression, string.Empty);

            //var loDataTable = new DataTable();
            //var loDataColumn = new DataColumn("Eval", typeof(decimal), expression);
            //loDataTable.Columns.Add(loDataColumn);
            //loDataTable.Rows.Add(0);
            //dblResult = (decimal)(loDataTable.Rows[0]["Eval"]);
        }
        catch
        {
        }
        return dblResult;
    }

    private string getAllLetters(string strFormulaDefinition)
    {
        StringBuilder sbLetters = new StringBuilder();
        try
        {
            foreach (char chr in strFormulaDefinition)
            {
                if (Char.IsLetter(chr))
                {
                    if (!sbLetters.ToString().Equals(string.Empty))
                    {
                        sbLetters.Append(",");
                    }
                    sbLetters.Append("'" + chr + "'");
                }
            }
        }
        catch
        {
        }
        return sbLetters.ToString();
    }

    private void IsFormulaField(int FieldId, bool isLastRow, ref TextBox TXT)
    {
        try
        {
            if (ViewState["TabularMandatoryField"] != null)
            {
                DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];

                if (tblRefField != null)
                {
                    if (tblRefField.Rows.Count > 0)
                    {
                        DataView DV = tblRefField.Copy().DefaultView;

                        if (isLastRow)
                        {
                            DV.RowFilter = "FieldId=" + FieldId + " AND ISNULL(TotalCalc,0)=1";
                            if (DV.Count > 0)
                            {
                                TXT.Text = string.Empty;
                            }
                        }
                        TXT.Width = Unit.Pixel(80);
                        DV.RowFilter = string.Empty;
                        DV.RowFilter = "FieldId=" + FieldId + " AND (LEN(ISNULL(ReferenceName,''))>0 OR LEN(ISNULL(FormulaDefinition,''))>0)";

                        if (DV.Count > 0)
                        {
                            if (TXT.MaxLength > 9 || TXT.MaxLength.Equals(0))
                            {
                                TXT.MaxLength = 9;
                            }

                            TXT.BorderColor = System.Drawing.Color.LightCoral;
                            TXT.BorderWidth = Unit.Pixel(1);

                            TXT.Style["text-align"] = "right";
                            TXT.Attributes.Add("onkeypress", "return isNumber(event);");

                            DV.RowFilter = "FieldId=" + FieldId + " AND LEN(ISNULL(FormulaDefinition,''))>0";

                            if (DV.Count > 0)
                            {
                                TXT.Enabled = false;
                                TXT.Font.Bold = true;
                                //TXT.Width = Unit.Pixel(60);
                                TXT.BorderWidth = Unit.Pixel(2);
                                TXT.BorderColor = System.Drawing.Color.Maroon;
                                TXT.ForeColor = System.Drawing.Color.Navy;
                            }
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

    private string getSubString(string strMain, int startIdx, int len)
    {
        string val = string.Empty;
        try
        {
            if (startIdx > -1 && len > 0 && strMain.Length > startIdx)
            {
                val = strMain.Substring(startIdx, len);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

        return val;
    }

    private string getGenderValue(int Gender)
    {
        if (Gender.Equals(1))
        {
            return "F";
        }
        else if (Gender.Equals(2))
        {
            return "M";
        }
        else
        {
            return "M";
        }
    }

    protected void setTabularHeader()
    {
        lblMessage.Text = string.Empty;
        int RowStartIndex = 1;
        int flag = 0;

        try
        {
            DataRow[] dr = null;

            if (common.myStr(ViewState["SelectedNodeIsTabular"]).Trim().Equals("T"))
            {
                dr = dtNoOfColumn.Select("SectionId=" + ViewState["SelectedNode"]);

                btnAddRow.Visible = false;
                btnFormulaCalculate.Visible = false;

                if (gvTabularFormat.Items.Count > 0)
                {
                    if (dr.Length > 0)
                    {
                        btnAddRow.Visible = common.myBool(ViewState["AllowNewRow"]);

                        btnFormulaCalculate.Visible = false;
                        if (ViewState["TabularMandatoryField"] != null)
                        {
                            DataTable tblRefField = (DataTable)ViewState["TabularMandatoryField"];

                            if (tblRefField != null)
                            {
                                if (tblRefField.Rows.Count > 0)
                                {
                                    tblRefField.DefaultView.RowFilter = "LEN(ISNULL(ReferenceName,''))>0 OR ISNULL(TotalCalc,0)=1";
                                    if (tblRefField.DefaultView.Count > 0)
                                    {
                                        btnFormulaCalculate.Visible = true;
                                    }
                                }
                            }
                        }
                    }

                    for (int i = 0; i < gvTabularFormat.Items.Count; i++)
                    {
                        if (common.myInt(common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.SectionId].Text).Trim()).Equals(common.myInt(ViewState["SelectedNode"])))
                        {
                            if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("H"))
                            {
                                //gvTabularFormat.Rows[i].Visible = true;
                                if (flag.Equals(0))
                                {
                                    RowStartIndex = common.myInt(gvTabularFormat.Items[i].Cells[(byte)enumT.Id].Text);
                                }

                                if (flag < 3)
                                {
                                    gvTabularFormat.Items[i].Visible = false;
                                }
                            }
                            else if (common.myStr(gvTabularFormat.Items[i].Cells[(byte)enumT.IsData].Text).Trim().Equals("D"))
                            {
                                gvTabularFormat.Items[i].Visible = true;
                            }
                            flag++;
                        }
                        else
                        {
                            gvTabularFormat.Items[i].Visible = false;
                        }
                    }

                    for (int j = 0; j < common.myInt(ViewState["TotalColCount"]) - 6; j++) //change 5 to 6
                    {
                        Label lblHeading = (Label)gvTabularFormat.Items[RowStartIndex].Cells[j].FindControl("lblFieldId" + j.ToString());

                        GridHeaderItem headerItem = gvTabularFormat.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                        headerItem.Cells[j + 2].Text = common.myStr(lblHeading.Text);

                        DataTable tbl = new DataTable();
                        if (ViewState["TabularMandatoryField"] != null)
                        {
                            tbl = (DataTable)ViewState["TabularMandatoryField"];
                            if (tbl.Rows.Count > 0)
                            {
                                Label lblFieldId = (Label)gvTabularFormat.Items[RowStartIndex - 1].Cells[j].FindControl("lblFieldId" + j.ToString());

                                DataView DV = tbl.DefaultView;
                                DV.RowFilter = "FieldId=" + common.myInt(lblFieldId.Text) + " AND IsMandatory=1";

                                if (DV.ToTable().Rows.Count > 0)
                                {
                                    headerItem.Cells[j + 2].Text = common.myStr(lblHeading.Text) + hdnMandatoryStar.Value;
                                }
                                DV.RowFilter = string.Empty;
                            }
                        }

                        headerItem.Cells[j + 2].Font.Size = 10;


                        if (!lblHeading.Text.Trim().Equals("+"))
                        {
                            if (dr.Length > 0)
                            {
                                if (j >= common.myInt(dr[0][1]))
                                {
                                    gvTabularFormat.Columns[j + 2].Visible = false;
                                }
                                else
                                {
                                    Label lblControlType = (Label)gvTabularFormat.Items[2].Cells[j].FindControl("lblFieldId" + j.ToString());
                                    if (lblControlType != null)
                                    {
                                        if (common.myStr(lblControlType.Text).Equals("T"))
                                        {
                                            if (dtFieldMaxLength != null)
                                            {
                                                if (dtFieldMaxLength.Rows.Count > 0)
                                                {
                                                    int FieldId = common.myInt(((Label)gvTabularFormat.Items[RowStartIndex - 1].Cells[j].FindControl("lblFieldId" + j.ToString())).Text);
                                                    dtFieldMaxLength.DefaultView.RowFilter = "FieldId=" + common.myInt(FieldId);
                                                    if (dtFieldMaxLength.DefaultView.Count > 0)
                                                    {
                                                        int maxVal = getPixelValue(common.myInt(dtFieldMaxLength.DefaultView[0]["MaxLength"]));

                                                        gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(maxVal);
                                                        gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(maxVal);
                                                    }

                                                    dtFieldMaxLength.DefaultView.RowFilter = "";
                                                }
                                            }
                                        }
                                        else if (common.myStr(lblControlType.Text).Equals("M"))
                                        {
                                            gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(260);
                                            gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(260);
                                        }
                                        else if (common.myStr(lblControlType.Text).Equals("S"))
                                        {
                                            gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(130);
                                            gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(130);
                                        }
                                        else if (common.myStr(lblControlType.Text).Equals("D"))
                                        {
                                            gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(120);
                                            gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(120);
                                        }
                                        else if (common.myStr(lblControlType.Text).Equals("C"))
                                        {
                                            gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(155);
                                            gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(155);
                                        }
                                        else if (common.myStr(lblControlType.Text).Equals("W"))
                                        {
                                            gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(500);
                                            gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(500);
                                        }
                                    }

                                    gvTabularFormat.Columns[j + 2].Visible = true;
                                    gvTabularFormat.Columns[j + 2].HeaderText = lblHeading.Text;
                                    gvTabularFormat.Columns[j].ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                                    gvTabularFormat.Columns[j].ItemStyle.VerticalAlign = VerticalAlign.Top;
                                }
                            }
                            else
                            {
                                gvTabularFormat.Columns[j + 2].Visible = false;
                            }
                        }
                        else
                        {
                            gvTabularFormat.Columns[j].ItemStyle.Width = Unit.Pixel(150);
                            gvTabularFormat.Columns[j].HeaderStyle.Width = Unit.Pixel(150);
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

    protected void gvTemplateRequiredServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //Find the checkbox control in header and add an attribute
            ((CheckBox)e.Row.FindControl("chkAll")).Attributes.Add("onclick", "javascript:SelectAllTemplateRequiredServices('" +
                ((CheckBox)e.Row.FindControl("chkAll")).ClientID + "')");
        }
        //else if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    HiddenField hdnCalculationBase = (HiddenField)e.Row.FindControl("hdnCalculationBase");
        //}
    }

    protected void ddlTemplateRequiredServices_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlTemplateMain.Text = string.Empty;
        ddlTemplateMain.SelectedIndex = -1;
        if (common.myInt(ddlTemplateMain.SelectedIndex).Equals(-1))
        {
            ddlTemplateMain_SelectedIndexChanged(null, null);
        }

        bindMainTemplateListOfSelectedService(common.myStr(ddlTemplateRequiredServices.SelectedValue));
        if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
        {
            settingControls(common.myStr(ViewState["Source"]));
        }

    }

    private void selectTemplateRequiredSerices(string ServiceIds)
    {
        //ddlTemplateMain.Text = string.Empty;
        //ddlTemplateMain.SelectedIndex = -1;

        if (common.myStr(ServiceIds).Length > 0)
        {
            ddlTemplateMainSelectedIndexChanged(false);

            //bindMainTemplateListOfSelectedService(ServiceIds);
            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
            {
                settingControls(common.myStr(ViewState["Source"]));
            }
        }
    }

    protected void gvTemplateRequiredServices_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SERVICE")
            {
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");

                ViewState["TemplateRequiredServiceId"] = common.myInt(hdnServiceId.Value).ToString();

                selectTemplateRequiredSerices(common.myStr(hdnServiceId.Value));

                setRequestSelectedServiceColor();
            }
            else if (e.CommandName == "DEL")
            {
                try
                {
                    if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder"))
                    {
                        if (Session["TemplateDataDetails"] == null)
                        {
                            return;
                        }

                        GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                        HiddenField hdnServiceId = (HiddenField)row.FindControl("hdnServiceId");

                        if (common.myInt(hdnServiceId.Value) > 0)
                        {
                            DataSet dsTemplateDataDetails = (DataSet)Session["TemplateDataDetails"];

                            if (dsTemplateDataDetails != null)
                            {
                                if (dsTemplateDataDetails.Tables.Count > 0)
                                {
                                    if (dsTemplateDataDetails.Tables[0].Rows.Count > 0)
                                    {
                                        DataView DV = dsTemplateDataDetails.Tables[0].Copy().DefaultView;
                                        DV.RowFilter = "ISNULL(EncounterId,0)=" + common.myInt(ViewState["EncounterId"]) +
                                        " AND ServiceId<>" + common.myInt(hdnServiceId.Value) +
                                        " AND TemplateId=" + common.myInt(ddlTemplateMain.SelectedValue) +
                                        " AND RegistrationId=" + common.myInt(ViewState["RegistrationId"]);

                                        dsTemplateDataDetails = new DataSet();
                                        dsTemplateDataDetails.Tables.Add(DV.ToTable());
                                        Session["TemplateDataDetails"] = dsTemplateDataDetails;

                                        ddlTemplateMainSelectedIndexChanged(true);
                                    }
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
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
        }
    }

    private void setServiceChkVisiblity()
    {
        if (Session["TemplateDataDetails"] != null)
        {
            if (common.myStr(ViewState["Source"]).Equals("ProcedureOrder")
                && !common.myStr(ViewState["TagType"]).Equals("D"))
            {
                DataSet dsTemplateDataDetails = (DataSet)Session["TemplateDataDetails"];

                if (dsTemplateDataDetails.Tables.Count > 0)
                {
                    DataTable tblItem = new DataTable();
                    tblItem = dsTemplateDataDetails.Tables[0];

                    DataView DVItem = tblItem.Copy().DefaultView;

                    if (common.myLen(Request.QueryString["TemplateRequiredServices"]) > 0)
                    {
                        DVItem.RowFilter = "ISNULL(EncounterId,0) = " + common.myInt(ViewState["EncounterId"]) +
                                           " AND ServiceId IN (" + common.myStr(Request.QueryString["TemplateRequiredServices"]) + ") " +
                                           " AND TemplateId = " + common.myInt(ddlTemplateMain.SelectedValue) +
                                           " AND RegistrationId = " + common.myInt(ViewState["RegistrationId"]);

                        foreach (GridViewRow dataItem in gvTemplateRequiredServices.Rows)
                        {
                            CheckBox chkRow = (CheckBox)dataItem.FindControl("chkRow");
                            LinkButton lnkEdit = (LinkButton)dataItem.FindControl("lnkEdit");

                            HiddenField hdnServiceId = (HiddenField)dataItem.FindControl("hdnServiceId");

                            if (DVItem.ToTable().Rows.Count > 0)
                            {
                                chkRow.Checked = false;
                                chkRow.Visible = false;
                                lnkEdit.Visible = true;
                            }
                            else
                            {
                                chkRow.Visible = true;
                                lnkEdit.Visible = false;
                            }
                        }
                    }
                }
            }
        }
    }

    //-------------Change for Patient template and ---------------


    protected void ddlTemplatePatient_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myStr(ddlTemplatePatient.SelectedValue)));

            if (!btnSave.Visible && ddlTemplateMain.SelectedIndex < 1 && common.myInt(ddlTemplatePatient.SelectedValue) > 0)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = common.myStr(ddlTemplatePatient.SelectedItem.Text);
                item.Value = common.myInt(ddlTemplatePatient.SelectedValue).ToString();
                //item.Attributes.Add("TemplateTypeID", "");
                //item.Attributes.Add("EntryType", "");
                //item.Attributes.Add("TemplateTypeCode", "");

                //item.Attributes.Add("IsCopyPreviousEpisod", false);
                //item.Attributes.Add("IsConfidential", false);

                ddlTemplateMain.Items.Add(item);

                ddlTemplateMain.SelectedIndex = ddlTemplateMain.Items.IndexOf(ddlTemplateMain.Items.FindItemByValue(common.myStr(ddlTemplatePatient.SelectedValue)));
            }

            ddlTemplateMain_SelectedIndexChanged(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }
    protected void ddlResultSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ViewState["ResultSetId"] = "0";

            txtResultSet.Text = string.Empty;

            if (ddlResultSet.SelectedIndex > 0)
            {
                txtResultSet.Text = common.myStr(ddlResultSet.SelectedItem.Text);
                ViewState["ResultSetId"] = common.myInt(ddlResultSet.SelectedValue).ToString();

                BindCategoryTree();

                if (tvCategory.Nodes.Count > 0)
                {
                    isFirstTime = true;
                    tvCategory_SelectedNodeChanged(sender, e);
                    isFirstTime = false;

                    showgridcontrols();

                    tvCategory.ExpandAll();
                }
                dvResultSet.Visible = false;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Data not found !";
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    protected void btnSaveAs_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            if (!isSaved(false))
            {
                return;
            }

            saveData(true);

            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;

            if (common.myStr(txtResultSet.Text).Trim().Length.Equals(0))
            {
                lblMessage.Text = "Result Set can't be blank !";
                return;
            }

            if (common.myInt(ddlResultSet.SelectedValue).Equals(1000000000))
            {
                lblMessage.Text = "Default Values not allow to Save As !";
                return;
            }

            if (common.myInt(ViewState["PageId"]).Equals(0))
            {
                lblMessage.Text = "Master Template not selected !";
                return;
            }
            string strMsg = string.Empty;
            string strNonTabular = string.Empty;
            if (ViewState["ResultSet"] != null)
            {
                strNonTabular = ViewState["ResultSet"].ToString();
            }

            if (!strNonTabular.ToString().Equals(string.Empty))
            {
                strMsg = objEMR.SaveEMRTemplateResultSet(common.myInt(ddlResultSet.SelectedValue), common.myStr(txtResultSet.Text).Trim(),
                                     common.myInt(ViewState["PageId"]), strNonTabular, common.myInt(ViewState["DoctorId"]),
                                     common.myInt(Session["HospitalLocationId"]), common.myInt(Session["UserId"]));
            }

            if ((strMsg.ToUpper().Contains(" UPDATE") || strMsg.ToUpper().Contains(" SAVE")) && !strMsg.ToUpper().Contains("USP"))
            {
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                lblMessage.Text = strMsg;

                txtResultSet.Text = string.Empty;
                ddlResultSet.SelectedIndex = 0;

                bindResultSet();
                dvResultSet.Visible = false;
            }
            else
            {
                lblMessage.Text = strMsg;
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    private void bindResultSet()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            DataSet ds = new DataSet();

            ds = objEMR.getTemplateResultSet(common.myInt(ViewState["PageId"]), common.myInt(ViewState["DoctorId"]));

            ddlResultSet.Items.Clear();
            //ddlResultSet.Visible = true;

            ddlResultSet.DataSource = ds.Tables[0];
            ddlResultSet.DataTextField = "ResultSetName";
            ddlResultSet.DataValueField = "ResultSetId";
            ddlResultSet.DataBind();

            ddlResultSet.Items.Insert(0, new RadComboBoxItem(string.Empty, "0"));
            ddlResultSet.Items.Insert(1, new RadComboBoxItem("Default Values", "1000000000"));

            ddlResultSet.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnDeleteResultSet_Click(object sender, EventArgs e)
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = string.Empty;
            if (common.myInt(ddlResultSet.SelectedValue).Equals(1000000000))
            {
                lblMessage.Text = "Default Values not allow to Delete!";
                return;
            }
            else
            {
                Hashtable hsin = new Hashtable();
                hsin.Add("@ResultSetId", common.myInt(ddlResultSet.SelectedValue));

                int i = objEMR.DeleteEMRTemplateResultSet(common.myInt(ddlResultSet.SelectedValue), common.myInt(Session["UserId"]));
                if (i.Equals(0))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = "Deleted Successfully!";
                    ddlResultSet.SelectedIndex = 0;
                    txtResultSet.Text = string.Empty;
                    bindResultSet();

                    dvResultSet.Visible = false;
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

    //protected void btnViewHistroy_Click(object sender, EventArgs e)
    //{
    //    bindPatientHistroy();
    //    Session["Historyrecord"] = null;
    //    Session["NoofRecord"] = null;

    //    if (btnAddRow.Visible)
    //    {
    //        isFirstTime = true;
    //        tvCategory_SelectedNodeChanged(sender, e);
    //        isFirstTime = false;
    //    }
    //}
    //protected void btnViewDymanicTemplate_Click(Object sender, EventArgs e)
    //{
    //    if (!common.myInt(ViewState["EncounterId"]).Equals(0))
    //    {
    //        RadWindowForNew.NavigateUrl = "/Editor/ShowExaminationData.aspx?From=POPUP&RegId=" + common.myInt(ViewState["RegistrationId"]) + "&EncId=" + common.myInt(ViewState["EncounterId"]) + "&RequestTemplateId=" + ddlTemplateMain.SelectedValue;
    //        RadWindowForNew.Height = 500;
    //        RadWindowForNew.Width = 600;
    //        RadWindowForNew.Top = 20;
    //        RadWindowForNew.Left = 20;
    //        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
    //        RadWindowForNew.Modal = true;
    //        RadWindowForNew.InitialBehavior = WindowBehaviors.Maximize;
    //        RadWindowForNew.VisibleStatusbar = false;

    //        if (btnAddRow.Visible)
    //        {
    //            isFirstTime = true;
    //            tvCategory_SelectedNodeChanged(sender, e);
    //            isFirstTime = false;
    //        }
    //    }
    //    else
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Please Select Patient!";
    //    }
    //}

    protected void btnsetPreviousData_Click(object sender, EventArgs e)
    {
        try
        {
            //Response.Redirect("Default.aspx?Data=Previous", false);

            // /EMR/Templates/Default.aspx?DisplayMenu=1&TemplateId=5898&Data=Previous

            if (common.myLen(Session["TemplatePreviousPageUrl"]) > 0)
            {
                if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
                {
                    string strTemplatePreviousPageUrl = common.myStr(Session["TemplatePreviousPageUrl"]).Trim();

                    strTemplatePreviousPageUrl = strTemplatePreviousPageUrl.Replace("&TemplateId=" + common.myInt(Request.QueryString["TemplateId"]), "&TemplateId=" + common.myInt(ddlTemplateMain.SelectedValue));

                    Session["TemplatePreviousPageUrl"] = strTemplatePreviousPageUrl;
                }

                if (!common.myStr(Session["TemplatePreviousPageUrl"]).ToUpper().Contains("DATA=PREVIOUS"))
                {
                    Response.Redirect(common.myStr(Session["TemplatePreviousPageUrl"]) + "&Data=Previous", false);
                }
                else
                {
                    Response.Redirect(common.myStr(Session["TemplatePreviousPageUrl"]), false);
                }
            }
        }
        catch
        {
        }
    }

    //protected void btnSetDefault_Click(object sender, EventArgs e)
    //{
    //    Hashtable hstInput = new Hashtable();
    //    try
    //    {
    //        string sQ = "Select tf.FieldId, "
    //        + "Case FieldType "
    //        + "When 'T' Then Case When  Gender='B' or Gender='M' Then DefaultTextMale else DefaultTextFemale End "
    //        + "When 'M' Then Case When  Gender='B' or Gender='M' Then DefaultTextMale else DefaultTextFemale End "
    //        + "When 'W' Then tff.FormatText "
    //        + "Else Cast(DefaultValue as Varchar(20)) "
    //        + "End As DefaultValue, "
    //        + "FieldType, tf.DefaultValue as WDefaultValue from EMRTemplateFields tf "
    //        + "LEFT JOIN EMRTemplateFieldFormats tff ON tf.DefaultValue =tff.FormatId "
    //        + "where SectionId = @CategoryId And "
    //        + "(Gender = 'B' Or Gender = 'F') "
    //        + "AND tf.Active = 1 Order By SequenceNo ";

    //        ArrayList ar = GetSelectedNode(tvCategory.SelectedValue);
    //        hstInput.Add("CategoryId", common.myStr(ar[0]));

    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //        DataSet objDs = objDl.FillDataSet(CommandType.Text, sQ, hstInput);
    //        DataView objDv = new DataView(objDs.Tables[0]);

    //        DataTable objDt;
    //        string sDefaultValue = string.Empty;
    //        string sCType = string.Empty;

    //        if (gvSelectedServices.Visible)
    //        {
    //            foreach (GridViewRow item in gvSelectedServices.Rows)
    //            {
    //                //item.Visible = true;
    //                objDv.RowFilter = "FieldId=" + common.myInt(item.Cells[(byte)enumNonT.FieldId].Text);
    //                if (objDv.Count > 0)
    //                {
    //                    objDt = objDv.ToTable();
    //                    sDefaultValue = common.myStr(objDt.Rows[0]["DefaultValue"]).Trim();
    //                    sCType = common.myStr(objDt.Rows[0]["FieldType"]).Trim();

    //                    if (!sDefaultValue.Equals(string.Empty))
    //                    {
    //                        if (sCType.Equals("T"))
    //                        {
    //                            TextBox txtT = (TextBox)item.FindControl("txtT");
    //                            txtT.Text = sDefaultValue;
    //                            txtT.ToolTip = txtT.Text;
    //                        }
    //                        //else if (sCType == "I")
    //                        //{
    //                        //    TextBox txtT = (TextBox)item.FindControl("txtT");
    //                        //    txtT.Text = sDefaultValue;
    //                        //    txtT.ToolTip = txtT.Text;
    //                        //}
    //                        //else if (sCType == "IS")
    //                        //{
    //                        //    TextBox txtM = (TextBox)item.FindControl("txtM");
    //                        //    txtM.Text = sDefaultValue;
    //                        //    txtM.ToolTip = txtM.Text;
    //                        //}
    //                        else if (sCType.Equals("M"))
    //                        {
    //                            TextBox txtM = (TextBox)item.FindControl("txtM");
    //                            txtM.Text = sDefaultValue;
    //                            txtM.ToolTip = txtM.Text;
    //                        }
    //                        else if (sCType.Equals("W"))
    //                        {
    //                            DropDownList ddlFormat = (DropDownList)item.FindControl("ddlTemplateFieldFormats");
    //                            //ddlFormat.Items.FindByValue(common.myStr(objDt.Rows[0]["WDefaultValue"])).Selected = true;
    //                            ddlFormat.SelectedValue = common.myStr(objDt.Rows[0]["WDefaultValue"]);
    //                            //ddlTemplateFieldFormats_OnSelectedIndexChanged(this, null);
    //                            RadEditor txtM = (RadEditor)item.FindControl("txtW");
    //                            txtM.Content = sDefaultValue;
    //                        }
    //                        else if (sCType.Equals("C"))
    //                        {
    //                            Repeater rpt = (Repeater)item.FindControl("C");
    //                            if (rpt != null)
    //                            {
    //                                foreach (RepeaterItem item1 in rpt.Items)
    //                                {
    //                                    if (objDt != null)
    //                                    {
    //                                        if (objDt.Rows.Count > 0)
    //                                        {
    //                                            HiddenField hdn = (HiddenField)item1.FindControl("hdnCV");
    //                                            CheckBox chk = (CheckBox)item1.FindControl("C");
    //                                            foreach (DataRow drow in objDt.Rows)
    //                                            {
    //                                                if (common.myStr(drow["DefaultValue"]).Trim().Equals(common.myStr(hdn.Value).Trim()))
    //                                                {
    //                                                    chk.Checked = true;
    //                                                }
    //                                                else
    //                                                {
    //                                                    chk.Checked = false;
    //                                                }
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                        else if (sCType.Equals("B"))
    //                        {
    //                            RadioButtonList ddl = (RadioButtonList)item.FindControl("B");
    //                            ddl.SelectedValue = sDefaultValue;
    //                        }
    //                        else if (sCType.Equals("D"))
    //                        {
    //                            DropDownList ddl = (DropDownList)item.FindControl("D");
    //                            ddl.SelectedValue = sDefaultValue;
    //                            //ddl.Items.FindByValue(sDefaultValue).Selected = true;
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            for (int i = 0; i < gvTabularFormat.Items.Count; i++)
    //            {
    //                GridDataItem item = gvTabularFormat.MasterTableView.GetItems(GridItemType.Item)[i] as GridDataItem;
    //                for (int j = 0; j < gvTabularFormat.Items.Count; j++)
    //                {
    //                    if (item.Cells[j].Visible)
    //                    {
    //                        Label lblFieldId = item.FindControl("lblFieldId" + j.ToString()) as Label;
    //                        objDv.RowFilter = "FieldId = '" + lblFieldId.Text + "'";
    //                        if (objDv.Count > 0)
    //                        {
    //                            objDt = objDv.ToTable();
    //                            sDefaultValue = common.myStr(objDt.Rows[0]["DefaultValue"]).Trim();
    //                            sCType = common.myStr(objDt.Rows[0]["FieldType"]).Trim();

    //                            if (!sDefaultValue.Equals(string.Empty))
    //                            {
    //                                if (sCType.Equals("T"))
    //                                {
    //                                    TextBox txtT = (TextBox)item.FindControl("txtT" + j.ToString());
    //                                    txtT.Text = sDefaultValue;

    //                                    if (btnFormulaCalculate.Visible)
    //                                    {
    //                                        IsFormulaField(common.myInt(lblFieldId.Text), false, ref txtT);
    //                                    }
    //                                }
    //                                //else if (sCType == "I")
    //                                //{
    //                                //    TextBox txtT = (TextBox)item.FindControl("txtT" + j.ToString());
    //                                //    txtT.Text = sDefaultValue;
    //                                //}
    //                                //else if (sCType == "IS")
    //                                //{
    //                                //    TextBox txtM = (TextBox)item.FindControl("txtM" + j.ToString());
    //                                //    txtM.Text = sDefaultValue;
    //                                //}
    //                                else if (sCType.Equals("M"))
    //                                {
    //                                    TextBox txtM = (TextBox)item.FindControl("txtM" + j.ToString());
    //                                    txtM.Text = sDefaultValue;
    //                                }
    //                                else if (sCType.Equals("B"))
    //                                {
    //                                    DropDownList ddl = (DropDownList)item.FindControl("B" + j.ToString());
    //                                    ddl.SelectedValue = sDefaultValue;
    //                                }
    //                                else if (sCType.Equals("D"))
    //                                {
    //                                    DropDownList ddl = (DropDownList)item.FindControl("D" + j.ToString());
    //                                    ddl.SelectedValue = sDefaultValue;
    //                                    //ddl.Items.FindByValue(sDefaultValue).Selected = true;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else
    //                        break;
    //                }
    //            }
    //        }
    //        lblMessage.Text = string.Empty;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}
    //protected void btnSetPositive_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string strBtnText = "None";
    //        int toggleindex = common.myInt(txtToggleIndex.Text);
    //        strBtnText = btnSetPositive.ToggleStates[toggleindex].Text;
    //        foreach (GridViewRow r in gvSelectedServices.Rows)
    //        {
    //            if (common.myStr(r.Cells[(byte)enumNonT.FieldType].Text).Equals("B"))
    //            {
    //                DropDownList ddlB = r.FindControl("B") as DropDownList;
    //                if (strBtnText.Equals("Set All Positive"))
    //                {
    //                    ddlB.SelectedValue = "1";
    //                }
    //                else if (strBtnText.Equals("Set All Negative"))
    //                {
    //                    ddlB.SelectedValue = "0";
    //                }
    //                else
    //                {
    //                    ddlB.SelectedValue = "-1";
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}
    protected void btnUndoChanges_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx", false);
    }
    void visiblilityResultSet(bool isShow)
    {
        //btnSaveAs.Visible = isShow;
        ////btnDeleteResultSet.Visible = isShow;
        //txtResultSet.Visible = isShow;
        ////lblResultSet.Visible = isShow;
        ////ddlResultSet.Visible = isShow;
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

    protected void BindDataValueNew(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        if (i == 0)
        {
            objStr.Append(sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(sBegin + ", " + sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBegin + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
            }
        }
    }


    //protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    //{
    //    if (i == 0)
    //    {
    //        objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //    }
    //    else
    //    {
    //        if (FType != "C")
    //        {
    //            objStr.Append(sBegin + ", " + sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //        }
    //        else
    //        {
    //            if (i == 0)
    //            {
    //                objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //            }
    //            else if (i + 1 == objDs.Tables[1].Rows.Count)
    //            {
    //                objStr.Append(sBegin + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
    //            }
    //            else
    //            {
    //                objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
    //            }
    //        }
    //    }
    //    //}
    //}

    private string BindNonTabularImageTypeFieldValueTemplates(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' runat='server'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["TextValue"]) + "</td></tr><tr align='left'><td align='center'><img src='" + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }

    private string BindNonTabularImageTypeFieldValueTemplatesNew(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' width='100%'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["TextValue"]) + "</td></tr><tr align='left'><td><img src='..." + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }


    protected void btnResultSetOpen_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
        {
            dvResultSet.Visible = true;
        }
        else
        {
            Alert.ShowAjaxMsg("Please select template!", this.Page);
            ddlTemplateMain.Focus();
            return;
        }
    }
    protected void btnResultSetClose_OnClick(object sender, EventArgs e)
    {
        dvResultSet.Visible = false;
    }

    protected void tpTime_SelectedIndexChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        try
        {
            RadTimePicker tpTime = sender as RadTimePicker;
            GridViewRow row = tpTime.NamingContainer as GridViewRow;
            RadComboBox ddlTime = (RadComboBox)row.FindControl("ddlTime");
            TextBox txtDate = (TextBox)row.FindControl("txtDate");

            if (!common.myStr(ViewState["tpTimeClientID"]).Equals(common.myStr(tpTime.ClientID)))
            {
                tpTime.TimeView.Interval = new TimeSpan(0, 60, 0);
                tpTime.TimeView.StartTime = new TimeSpan(0, 0, 0);
                tpTime.TimeView.EndTime = new TimeSpan(24, 0, 0);

                ddlTime.Items.Clear();
                for (int i = 0; i < 60; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        ddlTime.Items.Add(new RadComboBoxItem("0" + i.ToString(), "0" + i.ToString()));
                    }
                    else
                    {
                        ddlTime.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                    }
                }

                if (tpTime.SelectedDate != null)
                {
                    string min = tpTime.SelectedDate.Value.Minute.ToString();
                    min = min.Length == 1 ? "0" + min : min;
                    ddlTime.SelectedIndex = ddlTime.Items.IndexOf(ddlTime.Items.FindItemByValue(min));
                }
                // txtDate.Text=common.myStr(txtDate.Text) + common.myStr(tpTime.SelectedDate)+ common.myStr(ddlTime.Text)
            }
            else
            {
                tpTime.SelectedDate = Convert.ToDateTime(ViewState["tpTimeSelectedDate"]);
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void ddlTime_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
    {

        //RadTimePicker tpTime = o as RadTimePicker;
        //GridViewRow row = tpTime.NamingContainer as GridViewRow;
        //RadComboBox ddlTime = (RadComboBox)row.FindControl("ddlTime");

        RadComboBox ddlTime = o as RadComboBox;
        GridViewRow row = ddlTime.NamingContainer as GridViewRow;
        RadTimePicker tpTime = (RadTimePicker)row.FindControl("tpTime");


        StringBuilder sb = new StringBuilder();
        try
        {
            if (tpTime.SelectedDate == null)
            {
                lblMessage.Text = "Please select time.";
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                return;
            }
            sb.Append(tpTime.SelectedDate.Value.ToString());
            sb.Remove(tpTime.SelectedDate.Value.ToString().IndexOf(":") + 1, 2);
            sb.Insert(tpTime.SelectedDate.Value.ToString().IndexOf(":") + 1, ddlTime.Text);
            tpTime.SelectedDate = Convert.ToDateTime(sb.ToString());
            ViewState["tpTimeSelectedDate"] = Convert.ToDateTime(sb.ToString());
            tpTime.Focus();
            ViewState["tpTimeClientID"] = tpTime.ClientID;

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

    protected void btnPrintTemplate_Click(object sender, EventArgs e)
    {
        ShowReport();
    }
    protected void ShowReport()
    {
        //string ReportServerPath = "http://" + reportServer + "/ReportServer";

        //ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
        //IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
        //ReportViewer1.ServerReport.ReportServerCredentials = irsc;
        //ReportViewer1.ProcessingMode = ProcessingMode.Remote;

        //ReportViewer1.ShowCredentialPrompts = false;
        //ReportViewer1.ShowFindControls = false;
        //ReportViewer1.ShowParameterPrompts = false;
        //ReportViewer1.ServerReport.ReportPath = "/EMRReports/SickLeave";

        //ReportParameter[] p = new ReportParameter[2];
        //p[0] = new ReportParameter("intRegistrationId", Session["RegistrationId"].ToString());
        //p[1] = new ReportParameter("intEncounterId", Session["EncounterId"].ToString());

        //ReportViewer1.ServerReport.SetParameters(p);
        //ReportViewer1.ServerReport.Refresh();

        RadWindow1.NavigateUrl = "/EMR/Masters/PrintSickLeave.aspx?TemplateId=" + common.myInt(ddlTemplateMain.SelectedValue) + "&SSRSReportName=" + common.myStr(ddlReport.SelectedItem.Attributes["SSRSReportName"]) + "&IsSSRS=" + common.myInt(ddlReport.SelectedItem.Attributes["IsSSRS"]);
        RadWindow1.Height = 600;
        RadWindow1.Width = 950;
        RadWindow1.Top = 40;
        RadWindow1.Left = 100;
        RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        //  RadWindow1.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow1.VisibleStatusbar = false;
    }
    protected void btnTemplateHistory_Click(object sender, EventArgs e)
    {
        //if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
        //{
        //RadWindowPrint.NavigateUrl = "/WardManagement/VisitHistory.aspx?Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp&CloseButtonShow=No&FromEMR=1";

        if (common.myInt(ddlTemplateMain.SelectedValue).Equals(0))
        {
            RadWindowPrint.NavigateUrl = "/WardManagement/VisitHistory.aspx?Regid=" + common.myStr(Session["RegistrationID"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myStr(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&FromWard=Y&OP_IP=I&Category=PopUp&CloseButtonShow=No&FromEMR=1";
        }
        else
        {
            RadWindowPrint.NavigateUrl = "/WardManagement/VisitHistory.aspx?Regid=" + common.myInt(Session["RegistrationId"]) + "&RegNo=" + common.myStr(Session["RegistrationNo"]) + "&EncId=" + common.myInt(Session["EncounterId"]) + "&EncNo=" + common.myStr(Session["EncounterNo"]) + "&TemplateidCE=" + common.myInt(ddlTemplateMain.SelectedValue) + "&EncId=" + common.myInt(Session["EncounterId"]) + "&From=CE&FromWard=Y&OP_IP=I&Category=PopUp&CloseButtonShow=No&FromEMR=1";
        }
        RadWindowPrint.Height = 600;
        RadWindowPrint.Width = 1300;
        RadWindowPrint.Top = 10;
        RadWindowPrint.Left = 10;
        RadWindowPrint.Modal = true;
        RadWindowPrint.OnClientClose = string.Empty; //"OnClientClose";
                                                     // RadWindowPrint.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

        RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowPrint.VisibleStatusbar = false;
        RadWindowPrint.InitialBehavior = WindowBehaviors.Maximize;
        //}
    }

    protected void btnLastVisitDetails_OnClick(object sender, EventArgs e)
    {
        BindBothGrids(false, true);
    }


    protected void btnPrintSection_Click(object sender, EventArgs e)
    {
        try
        {
            if (common.myInt(ddlTemplateMain.SelectedValue) > 0)
            {
                //hdnReportContent.Value = PrintReport(false);
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "None", "PrintSetupContent();", true);
                //return;

                Session["EMRTemplatePrintData"] = PrintReport(false, common.myInt(ViewState["SelectedNode"]));

                if (common.myLen(Session["EMRTemplatePrintData"]) > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
                    sb.Append("<td align=center><B>" + common.myStr(ddlTemplateMain.SelectedItem.Text) + "</B></td>");
                    sb.Append("</tr></table>");

                    sb.Append(Session["EMRTemplatePrintData"]);

                    Session["EMRTemplatePrintData"] = sb.ToString();
                }

                //RadWindowPrint.NavigateUrl = "/EMR/Templates/PrintPdf1.aspx?page=Ward&EncId=" + common.myStr(ViewState["EncounterId"]) + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&ReportId=" + common.myStr(ddlReportFormat.SelectedValue) + "&For=" + strDthSum + "&Finalize=" + common.myStr(hdnFinalizeDeFinalize.Value);
                RadWindowPrint.NavigateUrl = "/EMR/Templates/PrintPdf1.aspx";
                RadWindowPrint.Height = 598;
                RadWindowPrint.Width = 900;
                RadWindowPrint.Top = 10;
                RadWindowPrint.Left = 10;
                RadWindowPrint.Modal = true;
                RadWindowPrint.OnClientClose = string.Empty; //"OnClientClose";
                RadWindowPrint.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;

                RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
                RadWindowPrint.VisibleStatusbar = false;
            }
        }
        catch
        {
        }
        finally
        {
            dvConfirm.Visible = false;
            chkIncludePastHistory.Checked = false;
            ddlIPHDateRange.SelectedIndex = 0;
        }
    }

    protected void lnkConsolidateLabReport_Click(object sender, EventArgs e)
    {
        RadWindow2.NavigateUrl = "/LIS/Phlebotomy/ConsolidateLabReport.aspx?RT=CLR&Master=NO&EncNo=" + common.myStr(Session["Encno"])
            + "&RegNo=" + common.myLong(Session["RegistrationNo"]);// +"&EncounterDate=" + common.myDate(Session["EncounterDate"]).ToString("yyyy/MM/dd");


        RadWindow2.Height = 600;
        RadWindow2.Width = 900;
        RadWindow2.Top = 10;
        RadWindow2.Left = 10;
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.InitialBehavior = WindowBehaviors.Maximize;
        RadWindow2.VisibleStatusbar = false;

    }

    protected void chkApprovalRequired_CheckedChanged(object sender, EventArgs e)
    {
        if (chkApprovalRequired.Checked)
        {
            lblAdvisingDoctor.Visible = true;
            ddlAdvisingDoctor.Visible = true;
        }
        else
        {
            lblAdvisingDoctor.Visible = false;
            ddlAdvisingDoctor.Visible = false;
        }
    }
    public void IsCopyCaseSheetAuthorized()
    {
        BaseC.Security objSecurity = new BaseC.Security(sConString);
        bool IsAuthorized = objSecurity.IsCopyCaseSheetAuthorized(common.myInt(Session["UserID"]), common.myInt(Session["HospitalLocationID"]));
        hdnIsCopyCaseSheetAuthorized.Value = common.myStr(IsAuthorized);
    }
    protected void chkIncludePastHistory_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkIncludePastHistory.Checked)
        {
            if (ddlTemplateMain.SelectedValue != "")
            {
                dvConfirm.Visible = true;

            }
            else
            {

                chkIncludePastHistory.Checked = false;
                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
                lblMessage.Text = "Please Select Any Template";

            }
        }
        else
        {
            dvConfirm.Visible = false;

        }
    }

    protected void ddlIPHDateRange_OnTextChanged(object sender, EventArgs e)
    {
        if (ddlIPHDateRange.SelectedValue.Equals(""))
        {
            tblIPHDateRange.Visible = true;
        }
        else
        {
            tblIPHDateRange.Visible = false;
        }

        dtpToDate.SelectedDate = DateTime.Now;

        switch (common.myStr(ddlIPHDateRange.SelectedValue))
        {
            case "3M":
                dtpfromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                break;
            case "6M":
                dtpfromDate.SelectedDate = DateTime.Now.AddMonths(-6);
                break;
            case "1Y":
                dtpfromDate.SelectedDate = DateTime.Now.AddYears(-1);
                break;
            default:
                dtpfromDate.SelectedDate = DateTime.Now.AddMonths(-3);
                break;
        }
    }

    protected void btnIPHCancel_OnClick(object sender, EventArgs e)
    {
        dvConfirm.Visible = false;
        chkIncludePastHistory.Checked = false;
    }
}
