using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Net;
using Telerik.Web.UI;
using System.Collections;
using System.Text;
using System.Configuration;

public partial class EMR_ClinicalPathway_PatientTreatmentPlan : System.Web.UI.Page
{
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    DataSet objDs = new DataSet();
    DataTable dt = new DataTable();
    private string cCtlType = string.Empty;

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
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["EncounterId"] = common.myInt(Request.QueryString["EncId"]);
            hdnEncId.Value = common.myStr(Request.QueryString["EncId"]);
            ViewState["RegistrationId"] = common.myInt(Request.QueryString["RegId"]);
            BindTreatmentPatientList();
            GetServiceData(0, 0, 0, "");

        }
    }


    private void BindTreatmentPatientList()
    {
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        clsExceptionLog objException = new clsExceptionLog();
        DataView dv = new DataView();
        try
        {

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanPatientLists";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            dv = new DataView(ds.Tables[0]);
            if (common.myInt(ViewState["EncounterId"]) > 0)
            {
                dv.RowFilter = "EncounterId=" + common.myInt(ViewState["EncounterId"]);

                lblProvider.Text = common.myStr(dv.ToTable().Rows[0]["DoctorName"]);
                lblEncounterNo.Text = common.myStr(dv.ToTable().Rows[0]["EncounterNo"]);
                lblAdmissionDate.Text = common.myStr(dv.ToTable().Rows[0]["AdmissionDate"]);
                ViewState["EncounterId"] = common.myStr(dv.ToTable().Rows[0]["EncounterId"]);
                ViewState["RegistrationId"] = common.myStr(dv.ToTable().Rows[0]["RegistrationId"]);

                ViewState["RegistrationNo"] = common.myStr(dv.ToTable().Rows[0]["RegistrationNo"]);
                ViewState["EncounterNo"] = common.myStr(dv.ToTable().Rows[0]["EncounterNo"]);

                ViewState["SponsorId"] = common.myStr(dv.ToTable().Rows[0]["SponsorId"]);
                ViewState["PayorId"] = common.myStr(dv.ToTable().Rows[0]["PayorId"]);

                ViewState["ConsultingDoctorId"] = common.myStr(dv.ToTable().Rows[0]["ConsultingDoctorId"]);
                ViewState["PayerType"] = common.myStr(dv.ToTable().Rows[0]["PayerType"]);
                ViewState["InsuranceCardId"] = common.myStr(dv.ToTable().Rows[0]["InsuranceCardId"]);
                ViewState["CurrentBillCategory"] = common.myStr(dv.ToTable().Rows[0]["CurrentBillCategory"]);
                ViewState["OPIP"] = common.myStr(dv.ToTable().Rows[0]["OPIP"]);


                bindEMRGetPatientTreatmentPlanList();

            }
            gvTreatmentPatientList.DataSource = dv.ToTable();
            gvTreatmentPatientList.DataBind();
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            objRoot = null;
            ds.Dispose();
            client = null;
            objException = null;
            dv.Dispose();
        }
    }
    protected void lnkSelect_Click(object sender, EventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            LinkButton link = (LinkButton)sender;
            HiddenField hdnEncounterId = (HiddenField)link.FindControl("hdnEncounterId");
            HiddenField hdnRegistrationId = (HiddenField)link.FindControl("hdnRegistrationId");
            HiddenField hdnConsultingDoctorId = (HiddenField)link.FindControl("hdnConsultingDoctorId");
            HiddenField hdnPayerType = (HiddenField)link.FindControl("hdnPayerType");
            HiddenField hdnInsuranceCardId = (HiddenField)link.FindControl("hdnInsuranceCardId");
            HiddenField hdnCurrentBillCategory = (HiddenField)link.FindControl("hdnCurrentBillCategory");
            Label lblRegistrationNo = (Label)link.FindControl("lblRegistrationNo");
            //Label lblEncounterNo = (Label)link.FindControl("lblEncounterNo");

            HiddenField hdnSponsorId = (HiddenField)link.FindControl("hdnSponsorId");
            HiddenField hdnPayorId = (HiddenField)link.FindControl("hdnPayorId");
            HiddenField hdnOPIP = (HiddenField)link.FindControl("hdnOPIP");
            HiddenField hdnTemplateTypeID = (HiddenField)link.FindControl("hdnTemplateTypeID");
            HiddenField hdnTemplateId = (HiddenField)link.FindControl("hdnTemplateId");

            HiddenField hdnDoctorName = (HiddenField)link.FindControl("hdnDoctorName");
            HiddenField hdnEncounterNo = (HiddenField)link.FindControl("hdnEncounterNo");
            HiddenField hdnAdmissionDate = (HiddenField)link.FindControl("hdnAdmissionDate");
            HiddenField hdnGender = (HiddenField)link.FindControl("hdnGender");





            lblProvider.Text = hdnDoctorName.Value;
            lblEncounterNo.Text = hdnEncounterNo.Value;
            lblAdmissionDate.Text = hdnAdmissionDate.Value;


            ViewState["EncounterId"] = hdnEncounterId.Value;
            ViewState["RegistrationId"] = hdnRegistrationId.Value;

            ViewState["RegistrationNo"] = lblRegistrationNo.Text;
            ViewState["EncounterNo"] = hdnEncounterNo.Value;

            ViewState["SponsorId"] = hdnSponsorId.Value;
            ViewState["PayorId"] = hdnPayorId.Value;

            ViewState["ConsultingDoctorId"] = hdnConsultingDoctorId.Value;
            ViewState["PayerType"] = hdnPayerType.Value;
            ViewState["InsuranceCardId"] = hdnInsuranceCardId.Value;
            ViewState["CurrentBillCategory"] = hdnCurrentBillCategory.Value;
            ViewState["OPIP"] = common.myStr(hdnOPIP.Value);
            ViewState["TemplateId"] = common.myStr(hdnTemplateId.Value);


            ViewState["TemplateTypeID"] = common.myStr(hdnTemplateTypeID.Value);
            Session["Gender"] = common.myStr(hdnGender.Value);

            bindEMRGetPatientTreatmentPlanList();

            GridViewRow row = (GridViewRow)(((LinkButton)sender).NamingContainer);
            if (ViewState["rowIndex"] != null)
            {
                GridViewRow gvRow = gvTreatmentPatientList.Rows[common.myInt(ViewState["rowIndex"])];

                gvRow.BackColor = System.Drawing.Color.Empty;
            }

            row.BackColor = System.Drawing.Color.LightBlue;
            ViewState["rowIndex"] = row.RowIndex;

            GetServiceData(0, 0, 0, "");

        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            patient = null;
            ds.Dispose();
            objException = null;
        }
    }

    protected void bindEMRGetPatientTreatmentPlanList()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {


            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRPatientTreatmentPlanNameLists";


            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblPlanName.Text = common.myStr(ds.Tables[0].Rows[0]["PlanName"]);
                    hdnPlanId.Value = common.myStr(ds.Tables[0].Rows[0]["PlanId"]);
                    BindPlanDetails();
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
            client = null;
            objRoot = null;
            objException = null;
        }
    }
    protected void BindPlanDetails()
    {
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        string LastChangedDate = string.Empty;
        try
        {
            ViewState["EnteredData"] = null;
            ViewState["DayDetail"] = null;

            ViewState["DayId"] = null;
            ViewState["DayDetailId"] = null;
            ViewState["SelectedRow"] = null;
            ddlPlanDayDetail.Items.Clear();
            ddlPlanDayDetail.SelectedIndex = -1;
            if (common.myInt(hdnPlanId.Value) == 0)
            {
                Alert.ShowAjaxMsg("Please! Select Template Name.", Page.Page);
                return;
            }


            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetPatientPlanWiseDayEnteredData";

            objRoot.PlanId = common.myInt(hdnPlanId.Value);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);
            ddlPlanDayDetail.Items.Clear();
            ddlPlanDayDetail.Text = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ViewState["EnteredData"] = ds.Tables[0];

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Text = (string)ds.Tables[0].Rows[i]["DurationName"];
                    item.Value = ds.Tables[0].Rows[i]["RowNo"].ToString();
                    item.Attributes["DayId"] = ds.Tables[0].Rows[i]["DayId"].ToString();
                    item.Attributes["DayDetailId"] = ds.Tables[0].Rows[i]["DayDetailId"].ToString();
                    item.Attributes["Code"] = ds.Tables[0].Rows[i]["Code"].ToString();
                    this.ddlPlanDayDetail.Items.Add(item);
                    item.DataBind();
                }
                ddlPlanDayDetail.Items.Insert(0, new RadComboBoxItem("", ""));
                ddlPlanDayDetail.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            LastChangedDate = string.Empty;
            client = null;
            objRoot = null;

        }
    }


    protected void chkAllService_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox gv = (CheckBox)sender;
        CheckBox chkAllService = (CheckBox)gv.FindControl("chkAllService");

        bool bTrue = false;
        if (chkAllService.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }

        foreach (GridViewRow item in gvService.Rows)
        {
            CheckBox chkService = (CheckBox)item.FindControl("chkService");
            HiddenField hdnDiagSampleId = (HiddenField)item.FindControl("hdnDiagSampleId");
            if (common.myInt(hdnDiagSampleId.Value) == 0)
            {
                chkService.Checked = bTrue;
            }
            else
            {
                chkService.Checked = false;
            }
        }
    }

    protected void GetServiceData(int TemplatePlanId, int DayId, int DayDetailId, string TemplateType)
    {
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        DataSet ds = new DataSet();
        WebClient client = new WebClient();
        clsExceptionLog objException = new clsExceptionLog();
        DataView dv = new DataView();
        try
        {

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetPatientTreatmentPlanTaggedDetails";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.PlanId = TemplatePlanId;
            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;
            objRoot.TemplateType = TemplateType;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            objDs = ds;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["RecordId"] = common.myInt(objDs.Tables[1].Rows[0]["RecordId"]);
                gvSelectedServices.DataSource = ds.Tables[0];
                gvSelectedServices.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.InsertAt(dr, 0);

                gvSelectedServices.DataSource = ds.Tables[0];
                gvSelectedServices.DataBind();
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                gvSpecialsation.DataSource = ds.Tables[2];
                gvSpecialsation.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[2].NewRow();
                ds.Tables[2].Rows.InsertAt(dr, 0);
                gvSpecialsation.DataSource = ds.Tables[2];
                gvSpecialsation.DataBind();
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                gvService.DataSource = ds.Tables[3];
                gvService.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[3].NewRow();
                ds.Tables[3].Rows.InsertAt(dr, 0);
                gvService.DataSource = ds.Tables[3];
                gvService.DataBind();
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                //gvDrugClass.DataSource = ds.Tables[4];
                //gvDrugClass.DataBind();
            }
            else
            {
                //DataRow dr = ds.Tables[4].NewRow();
                //ds.Tables[4].Rows.InsertAt(dr, 0);

                //gvDrugClass.DataSource = ds.Tables[4];
                //gvDrugClass.DataBind();
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                gvPrescription.DataSource = ds.Tables[5];
                gvPrescription.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[5].NewRow();
                ds.Tables[5].Rows.InsertAt(dr, 0);

                gvPrescription.DataSource = ds.Tables[5];
                gvPrescription.DataBind();
            }
            if (ds.Tables[6].Rows.Count > 0)
            {
                gvTemplateList.DataSource = ds.Tables[6];
                gvTemplateList.DataBind();
            }
            else
            {
                DataRow dr = ds.Tables[6].NewRow();
                ds.Tables[6].Rows.InsertAt(dr, 0);

                gvTemplateList.DataSource = ds.Tables[6];
                gvTemplateList.DataBind();
            }
            if (ds.Tables[7].Rows.Count > 0)
            {
                txtChiefComplaints.Text = "";
                txtFreeInstruction.Text = "";
                txtPlanOfCare.Text = "";
                txtHistory.Text = "";
                txtExamination.Text = "";
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='CH'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtChiefComplaints.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='IN'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='POC'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='HIS'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[7]);
                dv.RowFilter = "TemplateCode='EXM'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
            }
            if (ds.Tables[8].Rows.Count > 0)
            {
                if (common.myStr(ds.Tables[8].Rows[0]["ProblemDescription"]) != "")
                {
                    txtChiefComplaints.Text = common.myStr(ds.Tables[8].Rows[0]["ProblemDescription"]);
                }
            }
            if (ds.Tables[9].Rows.Count > 0)
            {



                dv = new DataView(ds.Tables[9]);
                dv.RowFilter = "TemplateCode='IN'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtFreeInstruction.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[9]);
                dv.RowFilter = "TemplateCode='POC'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtPlanOfCare.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[9]);
                dv.RowFilter = "TemplateCode='HIS'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtHistory.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
                }
                dv = new DataView(ds.Tables[9]);
                dv.RowFilter = "TemplateCode='EXM'";
                if (dv.ToTable().Rows.Count > 0)
                {
                    txtExamination.Text = common.myStr(dv.ToTable().Rows[0]["TemplateText"]);
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
            objRoot = null;
            client = null;
            ds.Dispose();
            objException = null;
            dv.Dispose();
        }
    }

    protected void chkAllPrescription_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox gv = (CheckBox)sender;
        CheckBox chkAllPrescription = (CheckBox)gv.FindControl("chkAllPrescription");

        bool bTrue = false;
        if (chkAllPrescription.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }

        foreach (GridViewRow item in gvPrescription.Rows)
        {
            CheckBox chkPrescription = (CheckBox)item.FindControl("chkPrescription");
            HiddenField hdnIndentId = (HiddenField)item.FindControl("hdnIndentId");
            if (common.myInt(hdnIndentId.Value) == 0)
            {
                chkPrescription.Checked = bTrue;
            }
            else
            {
                chkPrescription.Checked = false;
            }
        }
    }


    protected void gvSelectedServices_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        DataView ddv = new DataView();
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {

                DataView objDv = null;
                DataView objDvValue;
                DataTable objDt = null;
                // HtmlTextArea txtMulti = (HtmlTextArea)e.Row.FindControl("txtRemarks");
                HiddenField hdnFieldType = (HiddenField)e.Row.FindControl("hdnFieldType");
                HiddenField hdnSectionId = (HiddenField)e.Row.FindControl("hdnSectionId");
                HiddenField hdnFieldId = (HiddenField)e.Row.FindControl("hdnFieldId");
                //  HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                if (objDs != null && objDs.Tables.Count > 1 && objDs.Tables[1].Rows.Count > 0)
                {
                    objDv = objDs.Tables[1].DefaultView;
                    objDv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                }
                ddv = new DataView(dt); // To Be Check

                if (ddv.Count > 0)
                {
                    ddv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                    objDt = ddv.ToTable();
                    if (objDt.Rows.Count > 0)
                    {
                        e.Row.Visible = true;
                    }
                }
                else
                {
                    if (objDs.Tables.Count > 1)
                    {
                        if (objDs.Tables[1].Rows.Count > 0)
                        {
                            objDvValue = objDs.Tables[1].DefaultView;
                            if (objDvValue.Table.Columns["FieldId"] != null)
                            {
                                objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            }
                            objDt = objDvValue.ToTable();
                            if (objDt.Rows.Count > 0)
                            {
                                e.Row.Visible = true;
                            }
                        }
                    }
                }


                if (!common.myStr(hdnFieldType.Value).Trim().Equals(string.Empty))
                {

                    #region Single TextBox Type
                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        cCtlType = "T";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtT.Enabled = true;
                        txtW.Visible = false;

                        string maxLength = common.myStr(txtT.MaxLength);

                        txtT.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtT.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtT.ToolTip = txtT.Text;
                            }

                        }


                    }
                    #endregion
                    #region start for Field type I
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("I"))
                    {
                        cCtlType = "I";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");


                        txtT.Enabled = true;
                        txtW.Visible = false;
                        txtT.Visible = true;
                    }
                    #endregion

                    #region start for Field type IS
                    if (common.myStr(hdnFieldType.Value).Trim().Equals("IS"))
                    {
                        cCtlType = "IS";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        HyperLink Hy_LinkUrl = (HyperLink)e.Row.FindControl("Hy_LinkUrl");
                        txtT.Enabled = false;
                        txtW.Visible = false;
                        txtT.Visible = false;
                        Hy_LinkUrl.Visible = false;
                        txtM.Visible = true;

                    }
                    #endregion
                    #region Mutiple Text Type
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        cCtlType = "M";
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        txtM.Visible = true;

                        txtM.Attributes.Add("onkeypress", "javascript:return AutoChange('" + txtM.ClientID + "');");
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtM.Text = common.myStr(objDt.Rows[0]["FieldValue"]);
                                txtM.ToolTip = txtM.Text;
                            }
                        }
                    }
                    #endregion
                    #region WordProcessor Type
                    else if (common.myStr(hdnFieldType.Value).Trim().Equals("W")) // For WordProcessor
                    {
                        cCtlType = "W";
                        DropDownList ddl = (DropDownList)e.Row.Cells[(byte)enumNonT.Values].FindControl("ddlTemplateFieldFormats");
                        ddl.Visible = true;
                        ddl.DataSource = BindFieldFormats(common.myInt(e.Row.Cells[(byte)enumNonT.FieldId].Text).ToString());
                        ddl.DataTextField = "FormatName";
                        ddl.DataValueField = "FormatId";
                        ddl.DataBind();
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = true;

                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtW.Content = common.myStr(objDt.Rows[0]["FieldValue"]);
                            }
                        }
                    }
                    #endregion

                    #region CheckBox Type
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        cCtlType = "C";
                        DataList list = (DataList)e.Row.FindControl("C");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        list.Visible = true;

                        list.DataSource = objDv;
                        list.DataBind();

                        //if (hdnColumnNosToDisplay.Value != null)
                        //{
                        //    list.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                        list.RepeatDirection = RepeatDirection.Horizontal;

                        //}

                        HiddenField hdnCV = (HiddenField)list.FindControl("hdnCV");
                        foreach (DataListItem item in list.Items)
                        {
                            HtmlTextArea CT = (HtmlTextArea)item.FindControl("CT");
                            CT.Attributes.Add("onkeypress", "javascript:return AutoChange('" + CT.ClientID + "');");
                            CT.Attributes.Add("onkeydown", "javascript:return AutoChange('" + CT.ClientID + "');");
                            HiddenField hdn = (HiddenField)item.FindControl("hdnCV");
                            CheckBox chk = (CheckBox)item.FindControl("C");
                            chk.Checked = false;
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
                                }
                            }
                        }
                    }
                    #endregion
                    #region Boolean Type
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        cCtlType = "B";
                        RadioButtonList B = (RadioButtonList)e.Row.FindControl("B");

                        B.Attributes.Add("onclick", "radioMe(event,'" + B.ClientID + "');");


                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        B.Visible = true;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                objDvValue = objDt.DefaultView;
                                if (objDvValue.Table.Columns["FieldId"] != null)
                                {
                                    objDvValue.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                                }

                                objDt = objDvValue.ToTable();

                                if (objDt.Rows.Count > 0)
                                {
                                    if (common.myInt(objDt.Rows[0]["FieldValue"]).Equals(1))//Yes
                                    {
                                        B.SelectedValue = "1";
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
                    }
                    #endregion
                    #region DropDown Type
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        cCtlType = "D";
                        DropDownList ddl = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

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

                    }
                    #endregion
                    #region DropDown Image Type
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        cCtlType = "IM";
                        RadComboBox ddl = (RadComboBox)e.Row.Cells[(byte)enumNonT.Values].FindControl("IM");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;
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

                    }
                    #endregion

                    #region RadioButton Type
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        cCtlType = "R";
                        RadioButtonList ddl = (RadioButtonList)e.Row.Cells[(byte)enumNonT.Values].FindControl("R");
                        HtmlTable tbl1 = (HtmlTable)e.Row.FindControl("tbl1");
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        //  HiddenField hdnColumnNosToDisplay = (HiddenField)e.Row.FindControl("hdnColumnNosToDisplay");

                        txtW.Visible = false;
                        tbl1.Visible = false;
                        ddl.Visible = true;

                        // if (hdnColumnNosToDisplay.Value != null)
                        // {
                        // ddl.RepeatColumns = common.myInt(hdnColumnNosToDisplay.Value);
                        ddl.RepeatDirection = RepeatDirection.Horizontal;

                        // }
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

                    }
                    #endregion
                    #region Date Type
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))//For Date
                    {
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;

                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        AjaxControlToolkit.CalendarExtender cal = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("CalendarExtender3");
                        txtW.Visible = false;
                        if (objDt != null)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                txtDate.Text = common.myStr(objDt.Rows[0]["FieldValue"]).Trim();
                            }
                        }
                        tblDate.Visible = true;
                    }
                    #endregion
                    #region Header Type
                    else if (common.myStr(hdnFieldType.Value).Equals("H"))//For Heading
                    {
                        cCtlType = "H";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        //btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        //btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;
                    }
                    #endregion
                    #region Static Template Type
                    else if (common.myStr(hdnFieldType.Value).Equals("L"))
                    {
                        cCtlType = "L";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");

                        LinkButton lnkStaticTemplate = (LinkButton)e.Row.FindControl("lnkStaticTemplate");

                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");

                        string sStaticTemplateId = common.myStr(lnkStaticTemplate.CommandArgument);
                        lnkStaticTemplate.Visible = true;


                        //BaseC.EMRMasters master = new BaseC.EMRMasters(sConString);
                        //DataSet dsStatic = master.GetAllTypeTemplates(common.myInt(Session["HospitalLocationId"]), "S");

                        WebClient client = new WebClient();
                        client.Headers["Content-type"] = "application/json";
                        client.Encoding = Encoding.UTF8;
                        string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetAllTypeTemplates";
                        APIRootClass.GetAllTypeTemplates objRoot = new global::APIRootClass.GetAllTypeTemplates();
                        objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
                        objRoot.sType = "S";

                        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
                        string sValue = client.UploadString(ServiceURL, inputJson);
                        sValue = JsonConvert.DeserializeObject<string>(sValue);
                        DataSet dsStatic = JsonConvert.DeserializeObject<DataSet>(sValue);

                        DataView dvStatic = new DataView(dsStatic.Tables[0]);
                        dvStatic.RowFilter = "DisplayInTemplate=1 AND SectionId=" + sStaticTemplateId;
                        DataTable dtStatic = dvStatic.ToTable();
                        if (dtStatic.Rows.Count > 0)
                        {
                            HiddenField hdnEmployeeTypeID = (HiddenField)e.Row.FindControl("hdnEmployeeTypeID");
                            HiddenField hdnIsEmployeeTypeTagged = (HiddenField)e.Row.FindControl("hdnIsEmployeeTypeTagged");

                            //if (IsEdit_NoTab(common.myBool(common.myInt(hdnIsEmployeeTypeTagged.Value)), common.myInt(hdnEmployeeTypeID.Value)))
                            //{
                            //    lnkStaticTemplate.Attributes.Add("onclick", "openStaticTemplateWindow('" + common.myStr(dtStatic.Rows[0]["PageUrl"]) + "',' " + sStaticTemplateId + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.FieldId].Text).Trim()) + "',' " + common.myInt(common.myStr(e.Row.Cells[(byte)enumNonT.SectionId].Text).Trim()) + "')");
                            lnkStaticTemplate.Enabled = true;
                            //}
                            //else
                            //{
                            //    lnkStaticTemplate.Enabled = false;
                            //}

                            lnkStaticTemplate.Text = common.myStr(dtStatic.Rows[0]["SectionName"]);
                        }
                        // btnAdd.Visible = false;

                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;
                        e.Row.Cells[(byte)enumNonT.FieldName].Font.Bold = true;



                    }
                    #endregion
                    #region Patient Data Object
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))//Patient Data Object
                    {
                        cCtlType = "O";
                        TextBox txtT = (TextBox)e.Row.FindControl("txtT");
                        TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                        // Button btnHelp = (Button)e.Row.FindControl("btnHelp");
                        RadioButtonList ddlB = (RadioButtonList)e.Row.FindControl("B");
                        DropDownList ddlD = (DropDownList)e.Row.FindControl("D");
                        HtmlTable tblDate = (HtmlTable)e.Row.Cells[(byte)enumNonT.Values].FindControl("tblDate");
                        TextBox txtDate = e.Row.Cells[(byte)enumNonT.Values].FindControl("txtDate") as TextBox;
                        RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");

                        // ImageButton btnAdd = (ImageButton)e.Row.FindControl("btnAdd");
                        // btnAdd.Visible = false;
                        txtT.Visible = false;
                        txtM.Visible = false;
                        // btnHelp.Visible = false;
                        ddlB.Visible = false;
                        ddlD.Visible = false;
                        tblDate.Visible = false;
                        txtDate.Visible = false;
                        txtW.Visible = false;

                        int DataObjectId = common.myInt(e.Row.Cells[(byte)enumNonT.DataObjectId].Text);

                        //clsIVF objivf = new clsIVF(sConString);
                        clsIVF objivf = new clsIVF(string.Empty);

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

                    }
                    #endregion
                }
                else
                {
                    //e.Row.Cells[(byte)enumNonT.Values].Text = "No Record Found!";
                }
                if (cCtlType.Equals("T") || cCtlType.Equals("M"))
                {
                    RadEditor txtW = (RadEditor)e.Row.FindControl("txtW");
                    txtW.Visible = false;
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
            ddv.Dispose();
            objException = null;
        }
    }
    public DataTable BindFieldFormats(String strFieldId)
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetTemplateFieldFormats";
            APIRootClass.GetTemplateFieldFormats objRoot = new global::APIRootClass.GetTemplateFieldFormats();
            objRoot.FieldId = common.myInt(strFieldId);
            objRoot.SpecialisationId = common.myInt(ViewState["DoctorSpecialisationId"]);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dt = ds.Tables[0];
                DataRow dr;
                dr = dt.NewRow();
                dr["FormatId"] = 0;
                dr["FormatName"] = "--Options--";
                dt.Rows.InsertAt(dr, 0);
            }
            ds.Dispose();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        return dt;
    }

    protected void ddlTemplateFieldFormats_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        DataSet ds = new DataSet();
        try
        {
            DropDownList ddl = sender as DropDownList;
            GridViewRow row = ddl.NamingContainer as GridViewRow;
            RadEditor txtW = (RadEditor)row.FindControl("txtW");


            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/GetFormatText";
            APIRootClass.GetFormatText objRoot = new global::APIRootClass.GetFormatText();
            objRoot.FormatId = common.myInt(ddl.SelectedValue);
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            ds = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtW.Content = common.myStr(ds.Tables[0].Rows[0]["FormatText"]);
            }
            else
            {
                txtW.Content = String.Empty;
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
            objException = null;
            ds.Dispose();
        }
    }
    protected void ddlPlanDayDetail_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["RecordId"] = null;
        SearchData();
    }

    protected void gvService_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnDiagSampleId = (HiddenField)e.Row.FindControl("hdnDiagSampleId");
            if (common.myInt(hdnDiagSampleId.Value) > 0)
            {
                e.Row.Enabled = false;
            }
        }
    }
    private void SearchData()
    {
        if (common.myInt(ddlPlanDayDetail.SelectedValue) > 0)
        {
            int DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
            int DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);

            GetServiceData(common.myInt(hdnPlanId.Value), DayId, DayDetailId, "");
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        SearchData();


    }



    protected void gvPrescription_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField hdnIndentId = (HiddenField)e.Row.FindControl("hdnIndentId");

            if (common.myInt(hdnIndentId.Value) > 0)
            {
                e.Row.Enabled = false;
            }
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
        }
    }



    protected void gvTreatmentPatientList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (common.myInt(hdnEncId.Value) > 0)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[4].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[4].Visible = false;
            }
        }
    }

    private int bindVisitRecord()
    {
        clsExceptionLog objException = new clsExceptionLog();
        int recordid = 1;
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();
        try
        {
            ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["TemplateId"]), common.myInt(Session["FacilityId"]));

            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                recordid = 1;
            }
            else
            {
                int rCount = ds.Tables[0].Rows.Count;
                if (rCount > 0)
                {
                    recordid = common.myInt(ds.Tables[0].Rows[rCount - 1]["RecordId"]) + 1;
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
            objException = null;
            objivf = null;
            ds.Dispose();
        }
        return recordid;
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        bool bTrue = false;
        if (chkSelectAll.Checked)
        {
            bTrue = true;
        }
        else
        {
            bTrue = false;
        }
        foreach (GridViewRow item2 in gvSelectedServices.Rows)
        {
            HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
            if (common.myStr(hdnFieldType.Value).Equals("C"))
            {
                DataList rptC = (DataList)item2.FindControl("C");
                foreach (DataListItem rptItem in rptC.Items)
                {
                    CheckBox chk = (CheckBox)rptItem.FindControl("C");
                    chk.Checked = bTrue;
                }
            }
        }

    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(hdnPlanId.Value) == 0)
            {
                Alert.ShowAjaxMsg("Please select Plan Name", Page);
                return;
            }
            RadWindow1.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=PatientPreviewReport&PlanId=" + common.myInt(hdnPlanId.Value)
                + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&EncId=" + common.myStr(ViewState["EncounterId"]) + "&Export=false";
            RadWindow1.Height = 620;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;

            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }


    protected void btnVariationReport_Click(object sender, EventArgs e)
    {
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            if (common.myInt(hdnPlanId.Value) == 0)
            {
                Alert.ShowAjaxMsg("Please select Plan Name", Page);
                return;
            }
            RadWindow1.NavigateUrl = "/EMRReports/PrintReport.aspx?ReportName=PatientVariationReport&PlanId=" + common.myInt(hdnPlanId.Value)
                + "&RegId=" + common.myStr(ViewState["RegistrationId"]) + "&EncId=" + common.myStr(ViewState["EncounterId"]) + "&Export=false";
            RadWindow1.Height = 620;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;

            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + ex.Message;
            objException.HandleException(ex);
        }
        finally
        {
            objException = null;
        }
    }

    protected void btnSaveService_Onclick(object sender, EventArgs e)
    {
        string xmlSchema = "";
        StringBuilder xml = new StringBuilder();
        clsExceptionLog objException = new clsExceptionLog();

        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();

        WebClient client = new WebClient();

        try
        {

            if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }


            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            if (sValue != "")
            {
                Alert.ShowAjaxMsg(sValue, Page);
                return;
            }


            xml.Append("<NewDataSet>");

            foreach (GridViewRow item in gvService.Rows)
            {
                CheckBox chkService = (CheckBox)item.FindControl("chkService");
                HiddenField hdnServiceId = (HiddenField)item.FindControl("hdnServiceId");
                Label lblServiceName = (Label)item.FindControl("lblServiceName");
                if (chkService.Checked)
                {
                    xml.Append("<Table1>");
                    xml.Append("<ServiceId>");
                    xml.Append(common.myInt(hdnServiceId.Value));
                    xml.Append("</ServiceId>");
                    xml.Append("<ServiceName>");
                    xml.Append(common.myStr(lblServiceName.Text));
                    xml.Append("</ServiceName>");
                    xml.Append("</Table1>");
                }
            }

            xml.Append("</NewDataSet>");
            if (xml.Length < 30)
            {
                Alert.ShowAjaxMsg("Please select Service", Page);
                return;
            }
            RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServicesV1.aspx?Regid=" + common.myInt(ViewState["RegistrationId"]) +
                                        "&RegNo=" + common.myStr(ViewState["RegistrationNo"]) + "&EncId=" + common.myInt(ViewState["EncounterId"]) + "&EncNo=" + common.myStr(ViewState["EncounterNo"]) +
                                        "&OP_IP=I&CompanyId=" + common.myInt(ViewState["PayorId"]) +
                                        "&InsuranceId=" + common.myInt(ViewState["SponsorId"]) +
                                        "&CardId=" + common.myInt(ViewState["InsuranceCardId"]) +
                                        "&PayerType=" + common.myInt(ViewState["PayerType"]) +
                                        "&BType=" + common.myInt(ViewState["CurrentBillCategory"]) +
                                        "&PlanId=" + common.myInt(hdnPlanId.Value) +
                                        "&DayId=" + common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]) +
                                        "&DayDetailId=" + common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]) +
                                        "&IsPasswordRequired=0&CarePlan=1&ServiceIds=" + xml.ToString();

            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
            RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }


        finally
        {
            xml = null;
            xmlSchema = string.Empty;
            objException = null;

            objRoot = null;

            client = null;
        }
    }

    protected void btnAddTemplateField_Click(object sender, EventArgs e)
    {

        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

        StringBuilder strTabular = new StringBuilder();
        StringBuilder strNonTabular = new StringBuilder();
        ArrayList coll = new ArrayList();
        clsExceptionLog objException = new clsExceptionLog();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        APIRootClass.ClinicalPath objRoot1 = new global::APIRootClass.ClinicalPath();

        WebClient client = new WebClient();
        WebClient client1 = new WebClient();

        StringBuilder xmlTemplateDetails = new StringBuilder();
        try
        {

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            if (sValue != "")
            {
                Alert.ShowAjaxMsg(sValue, Page);
                return;
            }

            #region Non Tabular
            //bindVisitRecord(common.myInt(ddlPlanTemplates.SelectedItem.Attributes["TemplateId"]));
            //ViewState["RecordId"] = 1;
            foreach (GridViewRow item2 in gvSelectedServices.Rows)
            {
                if (item2.RowType.Equals(DataControlRowType.DataRow))
                {
                    HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
                    HiddenField hdnSectionId = (HiddenField)item2.FindControl("hdnSectionId");
                    HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");
                    // SectionId = common.myInt(hdnSectionId.Value);

                    if (common.myStr(hdnFieldType.Value).Equals("T"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");

                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("T");
                        coll.Add(txtT.Text);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("I"))
                    {
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("I");
                        coll.Add(txtT.Text);

                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);


                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    if (common.myStr(hdnFieldType.Value).Equals("IS"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        coll.Add(common.myInt(hdnFieldId.Value));
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

                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("M"))
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        //if (txtM.Text.Trim() != "")
                        //{
                        coll.Add(common.myInt(hdnFieldId.Value));
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



                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("W")) // For Word Processor
                    {
                        RadEditor txtW = (RadEditor)item2.FindControl("txtW");
                        //if (txtW.Content != null)
                        //{
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("W");
                        coll.Add(txtW.Content);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("D"))
                    {
                        DropDownList ddl = (DropDownList)item2.FindControl("D");
                        // if (ddl.SelectedItem.Text != "Select")
                        // {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("D");
                        coll.Add(ddl.SelectedValue);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);



                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("IM"))
                    {
                        RadComboBox ddl = (RadComboBox)item2.FindControl("IM");
                        // if (ddl.SelectedItem.Text != "Select")
                        // {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("IM");
                        coll.Add(ddl.SelectedValue);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);

                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("R"))
                    {
                        RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
                        // if (ddl.SelectedItem.Text != "Select")
                        // {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("R");
                        coll.Add(ddl.SelectedValue);


                        coll.Add("0");
                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);


                        strNonTabular.Append(common.setXmlTable(ref coll));
                        //}
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("B"))
                    {
                        RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
                        if (ddlB.SelectedItem != null)
                        {
                            if (!ddlB.SelectedItem.Text.Equals("Select"))
                            {
                                coll.Add(common.myInt(hdnFieldId.Value));
                                coll.Add("B");
                                coll.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);


                                coll.Add("0");
                                coll.Add(0);
                                coll.Add(1);
                                coll.Add(null);



                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                            else
                            {
                                coll.Add(common.myInt(hdnFieldId.Value));
                                coll.Add("B");
                                coll.Add(null);


                                coll.Add("0");
                                coll.Add(0);
                                coll.Add(1);
                                coll.Add(null);



                                strNonTabular.Append(common.setXmlTable(ref coll));
                            }
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("C"))
                    {
                        DataList rptC = (DataList)item2.FindControl("C");
                        string sCheckedValues = string.Empty;
                        foreach (DataListItem rptItem in rptC.Items)
                        {
                            CheckBox chk = (CheckBox)rptItem.FindControl("C");
                            HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
                            HtmlTextArea CT = (HtmlTextArea)rptItem.FindControl("CT");

                            sCheckedValues = chk.Checked == true ? hdn.Value : "";
                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("C");
                            coll.Add(sCheckedValues);
                            coll.Add("0");

                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);

                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                        sCheckedValues = string.Empty;
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("S"))
                    {
                        TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                        if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
                        {

                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("S");

                            coll.Add(common.myStr(txtDate.Text).Trim());


                            coll.Add("0");
                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);



                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("ST"))
                    {
                        TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");


                        if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
                        {

                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("S");

                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");

                            coll.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));

                            coll.Add("0");
                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);


                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }

                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("SB"))
                    {
                        TextBox txtDate = (TextBox)item2.FindControl("txtDate");
                        RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
                        RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

                        if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
                        {

                            coll.Add(common.myInt(hdnFieldId.Value));
                            coll.Add("S");

                            DateTime d = tpTime.SelectedDate.Value;
                            string time = d.ToString("HH:mm:ss");

                            coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));


                            coll.Add("0");
                            coll.Add(0);
                            coll.Add(1);
                            coll.Add(null);



                            strNonTabular.Append(common.setXmlTable(ref coll));
                        }
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("H"))
                    {
                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("H");
                        coll.Add("-");
                        coll.Add("0");


                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);

                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }
                    else if (common.myStr(hdnFieldType.Value).Equals("O"))
                    {
                        DropDownList ddl = (DropDownList)item2.FindControl("D");
                        TextBox txtT = (TextBox)item2.FindControl("txtT");
                        int DataObjectId = common.myInt(item2.Cells[(byte)enumNonT.DataObjectId].Text);

                        clsIVF objivf = new clsIVF(sConString);
                        DataTable dtObject = objivf.getTemplateDataObjectQuery(DataObjectId);

                        coll.Add(common.myInt(hdnFieldId.Value));
                        coll.Add("O");
                        if (common.myStr(dtObject.Rows[0]["ObjectFieldType"]) == "D")
                        {
                            coll.Add(ddl.SelectedValue);
                        }
                        else
                        {
                            coll.Add(common.myStr(txtT.Text));
                        }
                        coll.Add("0");


                        coll.Add(0);
                        coll.Add(1);
                        coll.Add(null);

                        objivf = null;
                        dtObject.Dispose();

                        strNonTabular.Append(common.setXmlTable(ref coll));
                    }

                }
            }
            #endregion

            xmlTemplateDetails.Append(strNonTabular.ToString());
            //if (strNonTabular.ToString() == "")
            //{
            //    Alert.ShowAjaxMsg("Please select Template option", Page);
            //    return;
            //}


            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";


            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot1.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot1.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot1.xmlServiceDetails = "";

            objRoot1.xmlProblems = "";
            objRoot1.xmlItems = "";

            objRoot1.xmlItemDetail = "";
            objRoot1.xmlTemplateDetails = xmlTemplateDetails.ToString();

            objRoot1.EncodedBy = common.myInt(Session["UserId"]);

            objRoot1.DoctorId = common.myInt(ViewState["ConsultingDoctorId"]);



            objRoot1.PlanId = common.myInt(hdnPlanId.Value);

            objRoot1.DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
            objRoot1.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);
            lblMessage.Text = sValue1;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);

            #region comments
            //lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            //int SectionId = 0;
            //int i = 0;

            //if (common.myInt(ViewState["RecordId"]) == 0)
            //{
            //    i = bindVisitRecord();
            //}
            //else
            //{
            //    i = common.myInt(ViewState["RecordId"]);
            //}
            //int RowCaptionId = 0;



            //#region Non Tabular
            //foreach (GridViewRow item2 in gvSelectedServices.Rows)
            //{
            //    if (item2.RowType.Equals(DataControlRowType.DataRow))
            //    {
            //        HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
            //        HiddenField hdnSectionId = (HiddenField)item2.FindControl("hdnSectionId");
            //        HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");
            //        SectionId = common.myInt(hdnSectionId.Value);

            //        if (common.myStr(hdnFieldType.Value).Equals("T"))
            //        {
            //            TextBox txtT = (TextBox)item2.FindControl("txtT");

            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("T");
            //            coll.Add(txtT.Text);

            //            // HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");

            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //            //}
            //        }
            //        if (common.myStr(hdnFieldType.Value).Equals("I"))
            //        {
            //            TextBox txtT = (TextBox)item2.FindControl("txtT");
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("I");
            //            coll.Add(txtT.Text);
            //            // HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));

            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //        }
            //        if (common.myStr(hdnFieldType.Value).Equals("IS"))
            //        {
            //            TextBox txtM = (TextBox)item2.FindControl("txtM");
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("IS");
            //            if (common.myStr(txtM.Text).Contains("<"))
            //            {
            //                txtM.Text = txtM.Text.Replace("<", "&lt;");
            //            }
            //            if (common.myStr(txtM.Text).Contains(">"))
            //            {
            //                txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
            //            }
            //            coll.Add(txtM.Text);
            //            //HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("M"))
            //        {
            //            TextBox txtM = (TextBox)item2.FindControl("txtM");
            //            //if (txtM.Text.Trim() != "")
            //            //{
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("M");
            //            if (common.myStr(txtM.Text).Contains("<"))
            //            {
            //                txtM.Text = txtM.Text.Replace("<", "&lt;");
            //            }
            //            if (common.myStr(txtM.Text).Contains(">"))
            //            {
            //                txtM.Text = common.myStr(txtM.Text).Replace(">", "&gt;");
            //            }

            //            coll.Add(txtM.Text);


            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //            //}
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("W")) // For Word Processor
            //        {
            //            RadEditor txtW = (RadEditor)item2.FindControl("txtW");
            //            //if (txtW.Content != null)
            //            //{
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("W");
            //            coll.Add(txtW.Content);

            //            //HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //            //}
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("D"))
            //        {
            //            DropDownList ddl = (DropDownList)item2.FindControl("D");
            //            // if (ddl.SelectedItem.Text != "Select")
            //            // {
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("D");
            //            coll.Add(ddl.SelectedValue);

            //            //HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //            //}
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("IM"))
            //        {
            //            RadComboBox ddl = (RadComboBox)item2.FindControl("IM");
            //            // if (ddl.SelectedItem.Text != "Select")
            //            // {
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("IM");
            //            coll.Add(ddl.SelectedValue);

            //            //HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));
            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //            //}
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("R"))
            //        {
            //            RadioButtonList ddl = (RadioButtonList)item2.FindControl("R");
            //            // if (ddl.SelectedItem.Text != "Select")
            //            // {
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("R");
            //            coll.Add(ddl.SelectedValue);

            //            //HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);

            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));

            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //            //}
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("B"))
            //        {
            //            RadioButtonList ddlB = (RadioButtonList)item2.FindControl("B");
            //            if (ddlB.SelectedItem != null)
            //            {
            //                if (!ddlB.SelectedItem.Text.Equals("Select"))
            //                {
            //                    coll.Add(common.myInt(hdnFieldId.Value));
            //                    coll.Add("B");
            //                    coll.Add((common.myInt(ddlB.SelectedValue).Equals(1)) ? 1 : 0);

            //                    //  HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //                    coll.Add("0");
            //                    coll.Add(RowCaptionId);
            //                    coll.Add(null);
            //                    coll.Add(null);
            //                    coll.Add(common.myInt(hdnPlanId.Value));
            //                    coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //                    coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //                    strNonTabular.Append(common.setXmlTable(ref coll));
            //                }
            //                else
            //                {
            //                    coll.Add(common.myInt(hdnFieldId.Value));
            //                    coll.Add("B");
            //                    coll.Add(null);

            //                    // HtmlTextArea txtRemarks = (HtmlTextArea)item2.FindControl("txtRemarks");
            //                    coll.Add("0");
            //                    coll.Add(RowCaptionId);
            //                    coll.Add(null);
            //                    coll.Add(null);
            //                    coll.Add(common.myInt(hdnPlanId.Value));
            //                    coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //                    coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //                    strNonTabular.Append(common.setXmlTable(ref coll));
            //                }
            //            }
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("C"))
            //        {
            //            DataList rptC = (DataList)item2.FindControl("C");
            //            string sCheckedValues = string.Empty;
            //            foreach (DataListItem rptItem in rptC.Items)
            //            {
            //                CheckBox chk = (CheckBox)rptItem.FindControl("C");
            //                HiddenField hdn = (HiddenField)rptItem.FindControl("hdnCV");
            //                HtmlTextArea CT = (HtmlTextArea)rptItem.FindControl("CT");

            //                sCheckedValues = chk.Checked == true ? hdn.Value : "";
            //                coll.Add(common.myInt(hdnFieldId.Value));
            //                coll.Add("C");
            //                coll.Add(sCheckedValues);
            //                coll.Add("0");
            //                coll.Add(RowCaptionId);
            //                coll.Add(null);
            //                coll.Add(null);
            //                coll.Add(common.myInt(hdnPlanId.Value));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));

            //                strNonTabular.Append(common.setXmlTable(ref coll));
            //            }
            //            sCheckedValues = string.Empty;
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("S"))
            //        {
            //            TextBox txtDate = (TextBox)item2.FindControl("txtDate");
            //            RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
            //            RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

            //            if (!txtDate.Text.Trim().Equals("__/__/____") && !txtDate.Text.Trim().Equals(string.Empty))
            //            {

            //                coll.Add(common.myInt(hdnFieldId.Value));
            //                coll.Add("S");

            //                coll.Add(common.myStr(txtDate.Text).Trim());

            //                coll.Add("0");
            //                coll.Add(RowCaptionId);
            //                coll.Add(null);
            //                coll.Add(null);
            //                coll.Add(common.myInt(hdnPlanId.Value));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //                strNonTabular.Append(common.setXmlTable(ref coll));
            //            }
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("ST"))
            //        {
            //            TextBox txtDate = (TextBox)item2.FindControl("txtDate");
            //            RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
            //            RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");


            //            if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
            //            {

            //                coll.Add(common.myInt(hdnFieldId.Value));
            //                coll.Add("S");

            //                DateTime d = tpTime.SelectedDate.Value;
            //                string time = d.ToString("HH:mm:ss");

            //                coll.Add(common.myStr("01/01/1900").Trim() + " " + common.myStr(time));

            //                coll.Add("0");
            //                coll.Add(RowCaptionId);
            //                coll.Add(null);
            //                coll.Add(null);

            //                coll.Add(common.myInt(hdnPlanId.Value));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));

            //                strNonTabular.Append(common.setXmlTable(ref coll));
            //            }

            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("SB"))
            //        {
            //            TextBox txtDate = (TextBox)item2.FindControl("txtDate");
            //            RadTimePicker tpTime = (RadTimePicker)item2.FindControl("tpTime");
            //            RadComboBox ddlTime = (RadComboBox)item2.FindControl("ddlTime");

            //            if (tpTime.SelectedDate != null && common.myStr(tpTime.SelectedDate) != string.Empty)
            //            {

            //                coll.Add(common.myInt(hdnFieldId.Value));
            //                coll.Add("S");

            //                DateTime d = tpTime.SelectedDate.Value;
            //                string time = d.ToString("HH:mm:ss");

            //                coll.Add(common.myStr(txtDate.Text).Trim() + " " + common.myStr(time));

            //                coll.Add("0");
            //                coll.Add(RowCaptionId);
            //                coll.Add(null);
            //                coll.Add(null);
            //                coll.Add(common.myInt(hdnPlanId.Value));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //                coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));


            //                strNonTabular.Append(common.setXmlTable(ref coll));
            //            }
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("H"))
            //        {
            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("H");
            //            coll.Add("-");
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);
            //            coll.Add(null);
            //            coll.Add(null);

            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));
            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //        }
            //        else if (common.myStr(hdnFieldType.Value).Equals("O"))
            //        {
            //            DropDownList ddl = (DropDownList)item2.FindControl("D");
            //            TextBox txtT = (TextBox)item2.FindControl("txtT");
            //            int DataObjectId = common.myInt(item2.Cells[(byte)enumNonT.DataObjectId].Text);

            //            clsIVF objivf = new clsIVF(sConString);
            //            DataTable dtObject = objivf.getTemplateDataObjectQuery(DataObjectId);

            //            coll.Add(common.myInt(hdnFieldId.Value));
            //            coll.Add("O");
            //            if (common.myStr(dtObject.Rows[0]["ObjectFieldType"]) == "D")
            //            {
            //                coll.Add(ddl.SelectedValue);
            //            }
            //            else
            //            {
            //                coll.Add(common.myStr(txtT.Text));
            //            }
            //            coll.Add("0");
            //            coll.Add(RowCaptionId);

            //            if (common.myStr(dtObject.Rows[0]["ObjectFieldType"]) == "D")
            //            {
            //                coll.Add(ddl.SelectedValue);
            //                coll.Add(common.myStr(ddl.SelectedItem.Text));
            //            }
            //            else
            //            {
            //                coll.Add(null);
            //                coll.Add(txtT.Text);
            //            }
            //            coll.Add(common.myInt(hdnPlanId.Value));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]));
            //            coll.Add(common.myInt(ddlPlanDayDetail.SelectedValue));
            //            objivf = null;
            //            dtObject.Dispose();

            //            strNonTabular.Append(common.setXmlTable(ref coll));
            //        }

            //    }
            //}
            //#endregion
            //if (!strNonTabular.ToString().Equals(string.Empty))
            //{
            //    string strMsg = objEMR.SavePatientNotesData(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), 1,
            //                                 0, common.myInt(ViewState["TemplateId"]), common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]), 0,
            //                                common.myInt(ViewState["TemplateTypeID"]), strNonTabular.ToString(), "", 1, SectionId, i, 0,
            //                               common.myInt(Session["UserID"]), 0, 0, 0, common.myInt(ViewState["ConsultingDoctorId"]),
            //                               null, false, 0, 0);

            //    if (strMsg.ToUpper().Contains("SAVED"))
            //    {

            //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
            //        lblMessage.Text = strMsg;
            //    }
            #endregion
            GetServiceData(common.myInt(hdnPlanId.Value), common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]), common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]), "TF");
            //}
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objEMR = null;

            strTabular = null;
            strNonTabular = null;
            coll = null;
            objException = null;
            objRoot = null;

            client = null;

        }
    }
    protected void lnkTemplateName_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        BaseC.Patient patient = new BaseC.Patient(sConString);
        clsExceptionLog objException = new clsExceptionLog();

        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();

        WebClient client = new WebClient();
        try
        {

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            if (sValue != "")
            {
                Alert.ShowAjaxMsg(sValue, Page);
                return;
            }

            LinkButton lnk = (LinkButton)sender;
            HiddenField hdnTemplateId = (HiddenField)lnk.FindControl("hdnTemplateId");

            Session["OPIP"] = ViewState["OPIP"];


            ds = patient.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), common.myInt(ViewState["RegistrationId"]),
                                                common.myStr(ViewState["EncounterNo"]), common.myInt(ViewState["EncounterId"]));

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;
            Session["InvoiceId"] = "";
            RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?From=POPUP&IsEMRPopUp=1&SingleScreenTemplateCode=OTH&DisplayMenu=1&TemplateId=" + common.myInt(hdnTemplateId.Value)
                + "&RegistrationId=" + common.myStr(ViewState["RegistrationId"]) + "&EncounterId=" + common.myStr(ViewState["EncounterId"]);
            RadWindow1.Height = 620;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
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
            patient = null;
            objRoot = null;

            client = null;
        }
    }

    protected void btnAddPrescription_Onclick(object sender, EventArgs e)
    {
        string xmlSchema = "";
        StringBuilder xml = new StringBuilder();
        clsExceptionLog objException = new clsExceptionLog();
        DataSet ds = new DataSet();
        BaseC.Patient patient = new BaseC.Patient(sConString);
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();

        WebClient client = new WebClient();

        try
        {
            if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
            {
                Alert.ShowAjaxMsg("Please select day", Page);
                return;
            }

            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            if (sValue != "")
            {
                Alert.ShowAjaxMsg(sValue, Page);
                return;
            }

            xml.Append("<NewDataSet>");
            foreach (GridViewRow item in gvPrescription.Rows)
            {
                CheckBox chkPrescription = (CheckBox)item.FindControl("chkPrescription");
                HiddenField hdnItemId = (HiddenField)item.FindControl("hdnItemId");

                HiddenField hdnDoseUnitID = (HiddenField)item.FindControl("hdnDoseUnitID");
                HiddenField hdnFrequencyID = (HiddenField)item.FindControl("hdnFrequencyID");
                HiddenField hdnFoodRelationId = (HiddenField)item.FindControl("hdnFoodRelationId");
                HiddenField hdnGenericId = (HiddenField)item.FindControl("hdnGenericId");
                HiddenField hdnFrequencyValue = (HiddenField)item.FindControl("hdnFrequencyValue");

                HiddenField hdnQty = (HiddenField)item.FindControl("hdnQty");
                HiddenField hdnRouteId = (HiddenField)item.FindControl("hdnRouteId");

                Label lblItemName = (Label)item.FindControl("lblItemName");
                Label lblDays = (Label)item.FindControl("lblDays");
                Label lblDose = (Label)item.FindControl("lblDose");
                Label lblFrequencyName = (Label)item.FindControl("lblFrequency");
                Label lblFoodName = (Label)item.FindControl("lblFoodName");
                Label lblIntructions = (Label)item.FindControl("lblIntructions");
                Label lblDaysType = (Label)item.FindControl("lblDaysType");
                Label lblGenericName = (Label)item.FindControl("lblGenericName");
                Label lblDoseUnit = (Label)item.FindControl("lblDoseUnit");
                Label lblRouteName = (Label)item.FindControl("lblRouteName");


                #region table string
                if (chkPrescription.Checked)
                {
                    xml.Append("<Table1>");
                    xml.Append("<UnitId>");
                    xml.Append(common.myInt(hdnDoseUnitID.Value));
                    xml.Append("</UnitId>");

                    xml.Append("<RouteName>");
                    xml.Append(common.myStr(lblRouteName.Text));
                    xml.Append("</RouteName>");

                    xml.Append("<Duration>");
                    xml.Append(common.myStr(lblDays.Text));
                    xml.Append("</Duration>");

                    xml.Append("<Type>");
                    xml.Append(common.myStr(lblDaysType.Text));
                    xml.Append("</Type>");

                    xml.Append("<DurationText>");
                    xml.Append(common.myStr(lblDays.Text) + " " + common.myStr(lblDaysType.Text));
                    xml.Append("</DurationText>");

                    xml.Append("<FoodRelationshipID>");
                    xml.Append(common.myInt(hdnFoodRelationId.Value));
                    xml.Append("</FoodRelationshipID>");

                    xml.Append("<FoodRelationshipName>");
                    xml.Append(common.myStr(lblFoodName.Text));
                    xml.Append("</FoodRelationshipName>");

                    xml.Append("<Instructions>");
                    xml.Append(common.myStr(lblIntructions.Text));
                    xml.Append("</Instructions>");

                    xml.Append("<GenericId>");
                    xml.Append(common.myInt(hdnGenericId.Value));
                    xml.Append("</GenericId>");

                    xml.Append("<GenericName>");
                    xml.Append(common.myInt(lblGenericName.Text));
                    xml.Append("</GenericName>");

                    xml.Append("<ItemId>");
                    xml.Append(common.myInt(hdnItemId.Value));
                    xml.Append("</ItemId>");

                    xml.Append("<ItemName>");
                    xml.Append(common.myStr(lblItemName.Text));
                    xml.Append("</ItemName>");

                    xml.Append("<RouteId>");
                    xml.Append(common.myInt(hdnRouteId.Value));
                    xml.Append("</RouteId>");

                    xml.Append("<Dose>");
                    xml.Append(common.myDbl(lblDose.Text));
                    xml.Append("</Dose>");

                    xml.Append("<FrequencyId>");
                    xml.Append(common.myInt(hdnFrequencyID.Value));
                    xml.Append("</FrequencyId>");

                    xml.Append("<Frequency>");
                    xml.Append(common.myStr(hdnFrequencyValue.Value));
                    xml.Append("</Frequency>");

                    xml.Append("<FrequencyName>");
                    xml.Append(common.myStr(lblFrequencyName.Text));
                    xml.Append("</FrequencyName>");

                    xml.Append("<Days>");
                    xml.Append(common.myInt(lblDays.Text));
                    xml.Append("</Days>");

                    xml.Append("<StartDate>");
                    xml.Append(common.myDate(System.DateTime.Now).ToString("dd/MM/yyyy"));
                    xml.Append("</StartDate>");

                    xml.Append("<EndDate>");
                    xml.Append(common.myDate(System.DateTime.Now.AddDays(common.myInt(lblDays.Text))).ToString("dd/MM/yyyy"));
                    xml.Append("</EndDate>");

                    xml.Append("<Qty>");
                    xml.Append(common.myDec(hdnQty.Value));
                    xml.Append("</Qty>");

                    xml.Append("<UnitName>");
                    xml.Append(common.myStr(lblDoseUnit.Text));
                    xml.Append("</UnitName>");

                    xml.Append("</Table1>");
                }
            }
            xml.Append("</NewDataSet>");
            #endregion
            if (xml.Length < 30)
            {
                Alert.ShowAjaxMsg("Please select Drug", Page);
                return;
            }
            Session["CarePlanDrugLists"] = xml.ToString();


            ds = patient.getEMRPatientDetailsFilter(common.myInt(Session["HospitalLocationId"]),
                                               common.myInt(Session["FacilityId"]), common.myStr(ViewState["RegistrationNo"]), common.myInt(ViewState["RegistrationId"]),
                                               common.myStr(ViewState["EncounterNo"]), common.myInt(ViewState["EncounterId"]));

            Session.Remove("TopPanelPatientDetail" + common.myInt(Session["UserId"]));
            Session["PatientDetailString"] = null;
            Session["TopPanelPatientDetail" + common.myInt(Session["UserId"])] = ds;

            Session["OPIP"] = ViewState["OPIP"];
            RadWindow1.NavigateUrl = "/EMR/Medication/PrescribeMedicationNew.aspx?POPUP=POPUP&Regid=" + common.myInt(ViewState["RegistrationId"]) +
                                            "&RegNo=" + common.myStr(ViewState["RegistrationNo"]) +
                                            "&EncId=" + common.myInt(ViewState["EncounterId"]) +
                                            "&EncNo=" + common.myStr(ViewState["EncounterNo"]) +
                                            "&DoctorId=" + common.myStr(ViewState["ConsultingDoctorId"]) +
                                            "&IsPasswordRequired=0&CompanyId=" + common.myInt(ViewState["PayorId"]) +
                                            "&PlanId=" + common.myInt(hdnPlanId.Value) +
                                            "&DayId=" + common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]) +
                                            "&DayDetailId=" + common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);
            //"&ItemIds=" + xml.ToString();

            RadWindow1.Height = 620;
            RadWindow1.Width = 1000;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = "OnClientClose";
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            xml = null;
            xmlSchema = string.Empty;
            objException = null;
            ds.Dispose();
            patient = null;
            objRoot = null;

            client = null;
        }
    }

    private void bindVisitRecord(int TemplateId)
    {
        clsIVF objivf = new clsIVF(sConString);
        DataSet ds = new DataSet();
        clsExceptionLog objException = new clsExceptionLog();
        try
        {
            ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(ViewState["EncounterId"]), common.myInt(TemplateId), common.myInt(Session["FacilityId"]));
            if (ds.Tables[0].Rows.Count.Equals(0))
            {
                ViewState["RecordId"] = 1;
            }
            else
            {
                int rCount = ds.Tables[0].Rows.Count;
                if (rCount > 0)
                {
                    ViewState["RecordId"] = common.myInt(ds.Tables[0].Rows[rCount - 1]["RecordId"]) + 1;
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
            objivf = null;
            ds.Dispose();
            objException = null;
        }
    }
    protected void btnChiefComplaints_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtChiefComplaints.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Chief Complaints", Page);
            return;
        }
        if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please select day", Page);
            return;
        }
        ArrayList collChief = new ArrayList();
        StringBuilder objXMLProblem = new StringBuilder();
        BaseC.ParseData Parse = new BaseC.ParseData();
        clsExceptionLog objException = new clsExceptionLog();
        int DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
        int DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);
        WebClient client = new WebClient();
        WebClient client1 = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        APIRootClass.ClinicalPath objRoot1 = new global::APIRootClass.ClinicalPath();
        try
        {

            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot1.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot1.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot1.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);

            if (sValue1 != "")
            {
                Alert.ShowAjaxMsg(sValue1, Page);
                return;
            }

            #region  Problem
            if (!txtChiefComplaints.Text.Equals(string.Empty))
            {
                string TemplateId = Request.QueryString["POPUP"] != null && common.myStr(Request.QueryString["POPUP"]).Equals("StaticTemplate") ? common.myStr(Request.QueryString["TemplateFieldId"]) : "0";

                string strProblem = Parse.ParseQ(txtChiefComplaints.Text.Trim()).Replace("\n", "<br/>");

                if (common.myLen(strProblem) > 4000)
                {
                    Alert.ShowAjaxMsg("Chief complaints (free text) length must be less than 4000 character!", this.Page);
                    return;
                }
                collChief.Add(0);//Id
                collChief.Add(0);//ProblemId
                collChief.Add(strProblem);//Problem
                collChief.Add(0);//DurationID
                collChief.Add(string.Empty);//Duration
                collChief.Add(0);//ContextID
                collChief.Add(string.Empty);//Context
                collChief.Add(0);//SeverityId
                collChief.Add(string.Empty);//Severity
                collChief.Add(0);//IsPrimary
                collChief.Add(0);//IsChronic
                collChief.Add(common.myStr(Session["DoctorID"]));//DoctorId
                collChief.Add(common.myStr(Session["FacilityId"]));//FacilityId
                collChief.Add(0);//SCTId
                collChief.Add(string.Empty);//QualityIDs
                collChief.Add(0);//LocationID
                collChief.Add(string.Empty);//Location
                collChief.Add(0);//OnsetID
                collChief.Add(0);//AssociatedProblemId1
                collChief.Add(string.Empty);//AssociatedProblem1
                collChief.Add(0);//AssociatedProblemId2
                collChief.Add(string.Empty);//AssociatedProblem2
                collChief.Add(0);//AssociatedProblemId3
                collChief.Add(string.Empty);//AssociatedProblem3
                collChief.Add(0);//AssociatedProblemId4
                collChief.Add(string.Empty);//AssociatedProblem4
                collChief.Add(0);//AssociatedProblemId5
                collChief.Add(string.Empty);//AssociatedProblem5
                collChief.Add(string.Empty);//Side
                collChief.Add(0);//ConditionId
                collChief.Add(0);//Percentage
                collChief.Add(common.myInt(1));//Durations
                collChief.Add(common.myStr('D'));//DurationType
                collChief.Add(TemplateId);//TemplateFieldId
                collChief.Add(0);//ComplaintSearchId
                objXMLProblem.Append(common.setXmlTable(ref collChief));

            }
            #endregion


            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";


            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.xmlServiceDetails = "";

            objRoot.xmlProblems = objXMLProblem.ToString();
            objRoot.xmlItems = "";

            objRoot.xmlItemDetail = "";
            objRoot.xmlTemplateDetails = "";

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            objRoot.DoctorId = common.myInt(ViewState["ConsultingDoctorId"]);

            objRoot.PlanId = common.myInt(hdnPlanId.Value);




            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            collChief = null;
            objXMLProblem = null;
            Parse = null;
            client = null;
            client1 = null;
            objRoot = null;
            objRoot1 = null;
            objException = null;
        }
    }

    protected void btnHistory_Click(object sender, EventArgs e)
    {
        if (common.myStr(txtHistory.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type History", Page);
            return;
        }
        if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Day", Page.Page);
            return;
        }
        StringBuilder strNonTabularP = new StringBuilder();
        ArrayList collFre = new ArrayList();
        // StringBuilder xmlTemplateDetails = new StringBuilder();
        clsExceptionLog objException = new clsExceptionLog();

        int DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
        int DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


        WebClient client = new WebClient();
        WebClient client1 = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        APIRootClass.ClinicalPath objRoot1 = new global::APIRootClass.ClinicalPath();

        DataSet dsD = new DataSet();
        DataView dvD = new DataView();
        WebClient clientD = new WebClient();

        APIRootClass.ClinicalPath objRootD = new global::APIRootClass.ClinicalPath();
        try
        {

            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot1.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot1.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot1.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);

            if (sValue1 != "")
            {
                Alert.ShowAjaxMsg(sValue1, Page);
                return;
            }



            clientD.Headers["Content-type"] = "application/json";
            clientD.Encoding = Encoding.UTF8;
            string ServiceURLD = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetDefaultTemplateForCarePlan";


            objRootD.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRootD.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJsonD = (new JavaScriptSerializer()).Serialize(objRootD);
            string sValueD = clientD.UploadString(ServiceURLD, inputJsonD);
            sValueD = JsonConvert.DeserializeObject<string>(sValueD);
            dsD = JsonConvert.DeserializeObject<DataSet>(sValueD);

            #region History

            if (common.myLen(txtHistory.Text) > 0)
            {
                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='PHIS'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
                
                string strHistory = txtHistory.Text.Replace("\n", "<br/>");
                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"]));
                collFre.Add("W");
                collFre.Add(strHistory);
                collFre.Add("0");
                collFre.Add(0);
                collFre.Add(common.myInt(ViewState["RecordId"]));
                collFre.Add(4);
                strNonTabularP.Append(common.setXmlTable(ref collFre));
                strHistory = string.Empty;
            }
            #endregion

            // xmlTemplateDetails.Append(strNonTabularP.ToString());



            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";


            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.xmlServiceDetails = "";

            objRoot.xmlProblems = "";
            objRoot.xmlItems = "";

            objRoot.xmlItemDetail = "";
            objRoot.xmlTemplateDetails = strNonTabularP.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            objRoot.DoctorId = common.myInt(ViewState["ConsultingDoctorId"]);

            objRoot.PlanId = common.myInt(hdnPlanId.Value);

            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            strNonTabularP = null;
            collFre = null;
            //xmlTemplateDetails = null;
            objException = null;
            client = null;
            client1 = null;
            objRoot = null;
            objRoot1 = null;
            dsD.Dispose();
            dvD.Dispose();
            clientD = null;

            objRootD = null;
        }
    }

    protected void btnExamination_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtExamination.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Examination", Page);
            return;
        }
        if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Day", Page.Page);
            return;
        }

        StringBuilder strNonTabularP = new StringBuilder();
        ArrayList collFre = new ArrayList();
        //StringBuilder xmlTemplateDetails = new StringBuilder();
        clsExceptionLog objException = new clsExceptionLog();
        int DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
        int DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);
        WebClient client = new WebClient();
        WebClient client1 = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        APIRootClass.ClinicalPath objRoot1 = new global::APIRootClass.ClinicalPath();


        DataSet dsD = new DataSet();
        DataView dvD = new DataView();
        WebClient clientD = new WebClient();

        APIRootClass.ClinicalPath objRootD = new global::APIRootClass.ClinicalPath();
        try
        {

            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot1.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot1.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot1.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);

            if (sValue1 != "")
            {
                Alert.ShowAjaxMsg(sValue1, Page);
                return;
            }



            clientD.Headers["Content-type"] = "application/json";
            clientD.Encoding = Encoding.UTF8;
            string ServiceURLD = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetDefaultTemplateForCarePlan";


            objRootD.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRootD.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJsonD = (new JavaScriptSerializer()).Serialize(objRootD);
            string sValueD = clientD.UploadString(ServiceURLD, inputJsonD);
            sValueD = JsonConvert.DeserializeObject<string>(sValueD);
            dsD = JsonConvert.DeserializeObject<DataSet>(sValueD);

            #region Examination


            if (common.myLen(txtExamination.Text) > 0)
            {
                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='EXM'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
               
                string strExamination = txtExamination.Text.Replace("\n", "<br/>");

                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"]));
                collFre.Add("W");
                collFre.Add(strExamination);
                collFre.Add("0");
                collFre.Add(0);
                collFre.Add(common.myInt(ViewState["RecordId"]));
                collFre.Add(5);
                strNonTabularP.Append(common.setXmlTable(ref collFre));

            }


            #endregion

            // xmlTemplateDetails.Append(strNonTabularP.ToString());



            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";


            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.xmlServiceDetails = "";

            objRoot.xmlProblems = "";
            objRoot.xmlItems = "";

            objRoot.xmlItemDetail = "";
            objRoot.xmlTemplateDetails = strNonTabularP.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            objRoot.DoctorId = common.myInt(ViewState["ConsultingDoctorId"]);
            objRoot.PlanId = common.myInt(hdnPlanId.Value);

            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;


            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            strNonTabularP = null;
            collFre = null;
            //xmlTemplateDetails = null;
            objException = null;
            client = null;
            client1 = null;
            objRoot = null;
            objRoot1 = null;

            dsD.Dispose();
            dvD.Dispose();
            clientD = null;

            objRootD = null;
        }
    }

    protected void btnPlanOfCare_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtPlanOfCare.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Plan Of Care", Page);
            return;
        }
        if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Day", Page.Page);
            return;
        }

        StringBuilder strNonTabularP = new StringBuilder();
        ArrayList collFre = new ArrayList();
        //StringBuilder xmlTemplateDetails = new StringBuilder();
        clsExceptionLog objException = new clsExceptionLog();

        int DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
        int DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);
        WebClient client = new WebClient();
        WebClient client1 = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        APIRootClass.ClinicalPath objRoot1 = new global::APIRootClass.ClinicalPath();

        DataSet dsD = new DataSet();
        DataView dvD = new DataView();
        WebClient clientD = new WebClient();

        APIRootClass.ClinicalPath objRootD = new global::APIRootClass.ClinicalPath();
        try
        {

            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot1.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot1.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot1.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);

            if (sValue1 != "")
            {
                Alert.ShowAjaxMsg(sValue1, Page);
                return;
            }



            clientD.Headers["Content-type"] = "application/json";
            clientD.Encoding = Encoding.UTF8;
            string ServiceURLD = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetDefaultTemplateForCarePlan";


            objRootD.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRootD.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJsonD = (new JavaScriptSerializer()).Serialize(objRootD);
            string sValueD = clientD.UploadString(ServiceURLD, inputJsonD);
            sValueD = JsonConvert.DeserializeObject<string>(sValueD);
            dsD = JsonConvert.DeserializeObject<DataSet>(sValueD);


            #region PlanofCare

            if (!txtPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strPlanOfCare = txtPlanOfCare.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strPlanOfCare = strPlanOfCare.Replace("\n", "<br/>");

                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='POC'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
              

                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"])); //coll.Add(item2.Cells[0].Text);
                collFre.Add("W");
                collFre.Add(strPlanOfCare);
                collFre.Add("0");
                collFre.Add(0); //coll.Add(RowCaptionId);

                collFre.Add(common.myInt(ViewState["RecordId"]));

                collFre.Add(3);
                strNonTabularP.Append(common.setXmlTable(ref collFre));


            }

            #endregion

            // xmlTemplateDetails.Append(strNonTabularP.ToString());



            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";


            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.xmlServiceDetails = "";

            objRoot.xmlProblems = "";
            objRoot.xmlItems = "";

            objRoot.xmlItemDetail = "";
            objRoot.xmlTemplateDetails = strNonTabularP.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            objRoot.DoctorId = common.myInt(ViewState["ConsultingDoctorId"]);

            objRoot.PlanId = common.myInt(hdnPlanId.Value);

            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            strNonTabularP = null;
            collFre = null;
            //xmlTemplateDetails = null;
            objException = null;
            client = null;
            client1 = null;
            objRoot = null;
            objRoot1 = null;

            dsD.Dispose();
            dvD.Dispose();
            clientD = null;

            objRootD = null;
        }
    }

    protected void btnInstruction_Click(object sender, EventArgs e)
    {

        if (common.myStr(txtFreeInstruction.Text) == "")
        {
            Alert.ShowAjaxMsg("Please type Instruction", Page);
            return;
        }
        if (common.myInt(ddlPlanDayDetail.SelectedValue) == 0)
        {
            Alert.ShowAjaxMsg("Please! Select Day", Page.Page);
            return;
        }

        StringBuilder strNonTabularP = new StringBuilder();
        ArrayList collFre = new ArrayList();
        //StringBuilder xmlTemplateDetails = new StringBuilder();
        clsExceptionLog objException = new clsExceptionLog();

        int DayId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayId"]);
        int DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);
        WebClient client = new WebClient();
        WebClient client1 = new WebClient();
        APIRootClass.ClinicalPath objRoot = new global::APIRootClass.ClinicalPath();
        APIRootClass.ClinicalPath objRoot1 = new global::APIRootClass.ClinicalPath();

        DataSet dsD = new DataSet();
        DataView dvD = new DataView();
        WebClient clientD = new WebClient();

        APIRootClass.ClinicalPath objRootD = new global::APIRootClass.ClinicalPath();
        try
        {

            client1.Headers["Content-type"] = "application/json";
            client1.Encoding = Encoding.UTF8;
            string ServiceURL1 = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry";

            objRoot1.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot1.FacilityId = common.myInt(Session["FacilityId"]);

            objRoot1.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot1.DaysType = common.myStr(ddlPlanDayDetail.SelectedItem.Attributes["Code"]);
            objRoot1.DayDetailId = common.myInt(ddlPlanDayDetail.SelectedItem.Attributes["DayDetailId"]);


            string inputJson1 = (new JavaScriptSerializer()).Serialize(objRoot1);
            string sValue1 = client1.UploadString(ServiceURL1, inputJson1);
            sValue1 = JsonConvert.DeserializeObject<string>(sValue1);

            if (sValue1 != "")
            {
                Alert.ShowAjaxMsg(sValue1, Page);
                return;
            }



            clientD.Headers["Content-type"] = "application/json";
            clientD.Encoding = Encoding.UTF8;
            string ServiceURLD = WebAPIAddress.ToString() + "api/EMRAPI/EMRGetDefaultTemplateForCarePlan";


            objRootD.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRootD.FacilityId = common.myInt(Session["FacilityId"]);

            string inputJsonD = (new JavaScriptSerializer()).Serialize(objRootD);
            string sValueD = clientD.UploadString(ServiceURLD, inputJsonD);
            sValueD = JsonConvert.DeserializeObject<string>(sValueD);
            dsD = JsonConvert.DeserializeObject<DataSet>(sValueD);


            #region Instructions

            if (!txtFreeInstruction.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty).Equals(string.Empty))
            {
                string strInstructions = txtFreeInstruction.Text.Replace("<br />\n<div id=\"__zsc_once\"></div>", string.Empty);
                strInstructions = strInstructions.Replace("\n", "<br/>");


                dvD = new DataView(dsD.Tables[0]);
                dvD.RowFilter = "Code='IN'";
                if (dvD.ToTable().Rows.Count > 0)
                {
                    int iTemplateId = common.myInt(dvD.ToTable().Rows[0]["TemplateId"]);
                    bindVisitRecord(iTemplateId);
                }
              

                collFre = new ArrayList();
                collFre.Add(common.myInt(dvD.ToTable().Rows[0]["FieldId"])); //coll.Add(item2.Cells[0].Text);
                collFre.Add("W");
                collFre.Add(strInstructions);
                collFre.Add("0");
                collFre.Add(0); //coll.Add(RowCaptionId);

                collFre.Add(common.myInt(ViewState["RecordId"]));

                collFre.Add(2);
                strNonTabularP.Append(common.setXmlTable(ref collFre));

            }

            #endregion

            //xmlTemplateDetails.Append(strNonTabularP.ToString());



            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/EMRSavePatientTreatmentPlanTemplates";


            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(ViewState["RegistrationId"]);
            objRoot.EncounterId = common.myInt(ViewState["EncounterId"]);

            objRoot.xmlServiceDetails = "";

            objRoot.xmlProblems = "";
            objRoot.xmlItems = "";

            objRoot.xmlItemDetail = "";
            objRoot.xmlTemplateDetails = strNonTabularP.ToString();

            objRoot.EncodedBy = common.myInt(Session["UserId"]);

            objRoot.DoctorId = common.myInt(ViewState["ConsultingDoctorId"]);

            objRoot.PlanId = common.myInt(hdnPlanId.Value);

            objRoot.DayId = DayId;
            objRoot.DayDetailId = DayDetailId;

            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            lblMessage.Text = sValue;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        }
        finally
        {
            strNonTabularP = null;
            collFre = null;
            //xmlTemplateDetails = null;
            objException = null;
            client = null;
            client1 = null;
            objRoot = null;
            objRoot1 = null;

            dsD.Dispose();
            dvD.Dispose();
            clientD = null;

            objRootD = null;
        }
    }
}