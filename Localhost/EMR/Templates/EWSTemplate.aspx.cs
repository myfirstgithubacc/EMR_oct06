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
using System.Drawing;
using System.Text;
using Telerik.Web.UI;

public partial class EMR_Templates_EWSTemplate : System.Web.UI.Page
{
    String sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else if (common.myStr(Request.QueryString["POPUP"]) == "StaticTemplate")
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
        if (Convert.ToString(Session["encounterid"]) == "")
            Response.Redirect("/default.aspx?RegNo=0", false);

        if (!IsPostBack)
        {
            BindTemplate();
            if (common.myStr(Session["EncounterStatus"]).ToUpper().Contains("CLOSE") && !common.myBool(Session["isEMRSuperUser"]))
            {
                btnsave.Visible = false;
                btnNew.Visible = false;
            }

            bindCurrentVitalValue();
        }
    }
    private void BindTemplate()
    {
        clsIVF emr = new clsIVF(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        ViewState["DoctorId"] = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), common.myInt(common.myInt(Session["UserID"])));

        BaseC.User objU = new BaseC.User(sConString);
        ViewState["EmployeeType"] = common.myStr(objU.getEmployeeType(common.myInt(ViewState["DoctorId"]))).Trim();
        DataSet ds = emr.getEMRTemplate(common.myInt(Session["HospitalLocationId"]), common.myStr(ViewState["EmployeeType"]), 0, 0, "B", "EW");

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView dv = new DataView(ds.Tables[0]);
            dv.RowFilter = "TypeName='EWS'";
            foreach (DataRow dr in dv.ToTable().Rows)
            {
                RadComboBoxItem item; item = new RadComboBoxItem();
                item.Text = common.myStr(dr["TemplateName"]);
                item.Value = common.myInt(dr["TemplateId"]).ToString();
                item.Attributes.Add("TemplateTypeID", common.myStr(dr["TemplateTypeID"]));
                item.Attributes.Add("EntryType", common.myStr(dr["EntryType"]));
                item.Attributes.Add("TemplateTypeCode", common.myStr(dr["TemplateTypeCode"]));
                item.Attributes.Add("TemplateCode", common.myStr(dr["TemplateCode"]));
                ddlTemplateMain.Items.Add(item);
            }
            ddlTemplateMain.SelectedIndex = 0;
            if (dv.ToTable().Rows.Count > 0)
            {
                ddlTemplateMain_SelectedIndexChanged(this, null);
            }
            ds.Dispose();
            dv.Dispose();
            bindVisitRecord(false);
        }
    }

    private void bindCurrentVitalValue()
    {
        BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
        DataSet ds = new DataSet();

        string vitalSPO2 = string.Empty;
        string vitalR = string.Empty;
        string vitalBPS = string.Empty;
        string vitalBPD = string.Empty;
        string vitalT = string.Empty;
        string vitalP = string.Empty;
        try
        {
            if (ddlTemplateMain.SelectedItem.Attributes["TemplateCode"] != null)
            {
                if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]).ToUpper().Equals("EWS"))
                {
                    ds = (DataSet)objv.GetVitalTemplateDetails(1, common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), 0);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int rowIdx = 0; rowIdx < ds.Tables[0].Rows.Count; rowIdx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[rowIdx];

                            switch (common.myStr(DR["DisplayName"]))
                            {
                                case "SPO2":
                                    vitalSPO2 = common.myStr(DR["LastValues"]);
                                    break;
                                case "R":
                                    vitalR = common.myStr(DR["LastValues"]);
                                    break;
                                case "BPS":
                                    vitalBPS = common.myStr(DR["LastValues"]);
                                    break;
                                case "BPD":
                                    vitalBPD = common.myStr(DR["LastValues"]);
                                    break;
                                case "T":
                                    vitalT = common.myStr(DR["LastValues"]);
                                    break;
                                case "P":
                                    vitalP = common.myStr(DR["LastValues"]);
                                    break;
                            }
                        }

                        if (common.myLen(vitalSPO2) > 0 || common.myLen(vitalR) > 0 || common.myLen(vitalBPS) > 0
                            || common.myLen(vitalBPD) > 0 || common.myLen(vitalT) > 0 || common.myLen(vitalP) > 0)
                        {
                            foreach (GridViewRow item2 in gvScoreCard.Rows)
                            {
                                HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
                                HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");

                                TextBox txtFieldValue = (TextBox)item2.FindControl("txtFieldValue");
                                TextBox txtM = (TextBox)item2.FindControl("txtM");
                                DropDownList ddlDropDown = (DropDownList)item2.FindControl("ddlDropDown");
                                Label lblLastValue = (Label)item2.FindControl("lblLastValue");
                                HiddenField hdnFieldCode = (HiddenField)item2.FindControl("hdnFieldCode");

                                if (common.myStr(hdnFieldType.Value).ToUpper().Equals("T"))
                                {
                                    switch (common.myStr(hdnFieldCode.Value))
                                    {
                                        case "SPO2C":
                                            txtFieldValue.Text = common.myStr(vitalSPO2);
                                            break;
                                        case "RRMC":
                                            txtFieldValue.Text = common.myStr(vitalR);
                                            break;
                                        case "SBPC":
                                            txtFieldValue.Text = common.myStr(vitalBPS);
                                            break;
                                        case "DBPC":
                                            txtFieldValue.Text = common.myStr(vitalBPD);
                                            break;
                                        case "TC":
                                            txtFieldValue.Text = common.myStr(vitalT);
                                            break;
                                        case "PUL":
                                            txtFieldValue.Text = common.myStr(vitalP);
                                            break;
                                    }
                                }
                                //if (common.myStr(hdnFieldType.Value).ToUpper().Equals("M"))
                                //{                                
                                //}
                                //if (common.myStr(hdnFieldType.Value).ToUpper().Equals("D"))
                                //{                                
                                //}
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
            objv = null;
            ds.Dispose();
        }
    }


    protected void ddlTemplateMain_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindScoreCardTemplate();
            bindVisitRecord(false);
            if (ddlTemplateMain.SelectedItem.Attributes["EntryType"].Equals("M"))
                btnNewRecord.Visible = true;
            else
                btnNewRecord.Visible = false;

            if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]) == "EWS")
            {
                this.ifrmpdf.Attributes.Add("src", "/DocumentShare/PHYSIOLOGICALOBSERVATIONCHART.pdf");
            }
            else if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]) == "EWSP")
            {
                this.ifrmpdf.Attributes.Add("src", "/DocumentShare/PAEDIATRICPHYSIOLOGICALOBSERVATIONCHART.pdf");
            }

            bindCurrentVitalValue();
        }
        catch (Exception)
        { }
    }
    protected void btnScoreCardDetail_Click(object sender, EventArgs e)
    {
        RadWindow1.NavigateUrl = "~/Editor/WordProcessorTemplateBased.aspx?From=POPUP&DisplayMenu=1&TemplateId=" + common.myInt(ddlTemplateMain.SelectedValue);
        RadWindow1.Height = 560;
        RadWindow1.Width = 930;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }
    private void SetPermission()
    {
        UserAuthorisations ua1 = new UserAuthorisations(sConString);
        try
        {
            btnsave.Enabled = ua1.CheckPermissionsForEMRModule("N", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
            btnsave.Enabled = ua1.CheckPermissionsForEMRModule("E", Request.Url.AbsolutePath, common.myInt(Session["EncounterId"]));
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
    private void BindScoreCardTemplate()
    {
        DataSet ds = new DataSet();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        try
        {
            //int RecordId = bindVisitRecord(true);

            ds = emr.GetEMRGetTemplateDetails(common.myInt(Session["FacilityId"]), common.myInt(Session["EncounterId"]),
                common.myInt(Session["RegistrationId"]), common.myInt(ddlTemplateMain.SelectedValue), "", common.myInt(ddlRecord.SelectedValue), common.myInt(Session["UserId"]));
            ViewState["TemplateData"] = ds;
            gvScoreCard.DataSource = ds.Tables[0];
            gvScoreCard.DataBind();


            DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];

            object LastEntryDateTime = dt.Compute("MAX(VisitDateTime)", "");

            object LastEnteredBy = dt.Compute("MAX(EnterBy)", "");
            //Label1.Text = "Last Entry Date time";
            lblEntryDateTime.Text = LastEntryDateTime.ToString();
            //Label2.Text = "Last Entered By";
            lblLastEnteredBy.Text = LastEnteredBy.ToString();
            dt.Dispose();

        }
        catch (Exception ex)
        {
        }
        finally
        {
            ds.Dispose();
            emr = null;
        }
    }
    protected void btnCopyLastEWS_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow item2 in gvScoreCard.Rows)
        {
            HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");
            HiddenField hdnFieldId = (HiddenField)item2.FindControl("hdnFieldId");

            TextBox txtFieldValue = (TextBox)item2.FindControl("txtFieldValue");
            TextBox txtM = (TextBox)item2.FindControl("txtM");
            DropDownList ddlDropDown = (DropDownList)item2.FindControl("ddlDropDown");
            Label lblLastValue = (Label)item2.FindControl("lblLastValue");
            HiddenField hdnFieldCode = (HiddenField)item2.FindControl("hdnFieldCode");
            if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]) == "EWS")
            {
                if (common.myStr(hdnFieldType.Value) == "T")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                txtFieldValue.Text = common.myStr(dv.ToTable().Rows[0]["TextValue"]);
                            }
                            dv.Dispose();
                            dt.Dispose();
                        }
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "M")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                txtM.Text = common.myStr(dv.ToTable().Rows[0]["TextValue"]);
                            }
                            dv.Dispose();
                            dt.Dispose();
                        }
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "D")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        DataTable dt1 = ((DataSet)ViewState["TemplateData"]).Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);

                            DataView dv1 = new DataView(dt1);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                dv1.RowFilter = "ValueId=" + common.myStr(dv.ToTable().Rows[0]["FieldValue"]);
                                if (dv1.ToTable().Rows.Count > 0)
                                {
                                    ddlDropDown.SelectedValue = common.myStr(dv1.ToTable().Rows[0]["ValueId"]);
                                }
                            }
                            dv.Dispose();
                            dt.Dispose();
                            dt1.Dispose();
                            dv1.Dispose();
                        }
                    }
                }
            }
            else if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]) == "EWSP")
            {
                if (common.myStr(hdnFieldType.Value) == "T")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                txtFieldValue.Text = common.myStr(dv.ToTable().Rows[0]["TextValue"]);
                            }
                            dv.Dispose();
                            dt.Dispose();
                        }
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "M")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                txtM.Text = common.myStr(dv.ToTable().Rows[0]["TextValue"]);
                            }
                            dv.Dispose();
                            dt.Dispose();
                        }
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "D")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        DataTable dt1 = ((DataSet)ViewState["TemplateData"]).Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            DataView dv1 = new DataView(dt1);
                            dv1.RowFilter = "ValueId=" + common.myStr(dv.ToTable().Rows[0]["FieldValue"]);
                            if (dv1.ToTable().Rows.Count > 0)
                            {
                                ddlDropDown.SelectedValue = common.myStr(dv1.ToTable().Rows[0]["ValueId"]);
                            }
                            dv.Dispose();
                            dt.Dispose();
                            dt1.Dispose();
                            dv1.Dispose();
                        }
                    }
                }
            }
        }
    }

    protected void btnScoreCalculate_OnClick(object sender, EventArgs e)
    {
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataSet ds = new DataSet();
        DataSet dsScoreValue = new DataSet();
        StringBuilder sb = new StringBuilder();
        ArrayList col = new ArrayList();
        DataView dv = new DataView();
        try
        {
            ds = (DataSet)ViewState["TemplateData"];
            if (ds.Tables.Count > 3)
            {
                if (ds.Tables[4].Rows.Count > 0)
                {
                    double SPO2CValues = 0.0;
                    double RespiratoryRateValues = 0;
                    double HeartRateValues = 0;
                    double SystolicBPValues = 0;
                    double TemperatureValues = 0;
                    double CNSValues = 0;


                    double BHRValues = 0;
                    double CVRValues = 0;
                    double RPYalues = 0;
                    double TMNValues = 0;
                    double PVFSValues = 0;

                    foreach (GridViewRow row in gvScoreCard.Rows)
                    {
                        HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
                        HiddenField hdnSectionId = (HiddenField)row.FindControl("hdnSectionId");
                        HiddenField hdnFieldCode = (HiddenField)row.FindControl("hdnFieldCode");
                        HiddenField hdnFieldType = (HiddenField)row.FindControl("hdnFieldType");
                        HiddenField hdnIsMandatory = (HiddenField)row.FindControl("hdnIsMandatory");
                        Label lblFieldName = (Label)row.FindControl("lblFieldName");
                        DropDownList ddl = (DropDownList)row.FindControl("ddlDropDown");
                        TextBox txtFieldValue = (TextBox)row.FindControl("txtFieldValue");

                        col.Add(common.myInt(hdnSectionId.Value));
                        col.Add(common.myInt(hdnFieldId.Value));
                        col.Add(common.myStr(txtFieldValue.Text));
                        col.Add(common.myInt(ddl.SelectedValue));
                        sb.Append(common.setXmlTable(ref col));
                    }
                    dsScoreValue = emr.GetEWSScoreValues(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), sb.ToString());
                    dv = new DataView(dsScoreValue.Tables[0]);
                    ViewState["ScoreValue"] = dsScoreValue.Tables[0];


                    foreach (GridViewRow row in gvScoreCard.Rows)
                    {
                        HiddenField hdnFieldId = (HiddenField)row.FindControl("hdnFieldId");
                        HiddenField hdnSectionId = (HiddenField)row.FindControl("hdnSectionId");
                        HiddenField hdnFieldCode = (HiddenField)row.FindControl("hdnFieldCode");
                        HiddenField hdnFieldType = (HiddenField)row.FindControl("hdnFieldType");
                        HiddenField hdnIsMandatory = (HiddenField)row.FindControl("hdnIsMandatory");
                        Label lblFieldName = (Label)row.FindControl("lblFieldName");
                        DropDownList ddl = (DropDownList)row.FindControl("ddlDropDown");

                        TextBox txtFieldValue = (TextBox)row.FindControl("txtFieldValue");

                        if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]) == "EWS")
                        {

                            //SOP2
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "SPO2C")
                            {
                                TextBox txtSPO2C = (TextBox)row.FindControl("txtFieldValue");
                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(txtSPO2C.Text) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv.RowFilter = "FieldCode='SPO2C'";
                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    SPO2CValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    SPO2CValues = 0;
                                }
                                dv.RowFilter = "";
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "SPO2")
                            {
                                TextBox txtSPO2 = (TextBox)row.FindControl("txtFieldValue");
                                txtSPO2.Text = common.myDec(SPO2CValues).ToString("F0");
                            }
                            //Respiratory rate (min)
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "RRMC")
                            {
                                TextBox txtRRMC = (TextBox)row.FindControl("txtFieldValue");
                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(txtRRMC.Text) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }

                                dv.RowFilter = "FieldCode='RRMC'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    RespiratoryRateValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                { RespiratoryRateValues = 0; }

                                dv.RowFilter = "";
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "RRS")
                            {
                                TextBox txtRRM = (TextBox)row.FindControl("txtFieldValue");
                                txtRRM.Text = common.myDec(RespiratoryRateValues).ToString("F0");
                            }
                            //Heart rate (min)
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "HRMC")
                            {
                                TextBox txtHRMC = (TextBox)row.FindControl("txtFieldValue");
                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(txtHRMC.Text) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv.RowFilter = "FieldCode='HRMC'";
                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    HeartRateValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    HeartRateValues = 0;
                                }
                                dv.RowFilter = "";
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "HRS")
                            {
                                TextBox txtHRS = (TextBox)row.FindControl("txtFieldValue");
                                txtHRS.Text = common.myDec(HeartRateValues).ToString("F0");
                            }
                            ////Systolic BP
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "SBPC")
                            {
                                TextBox txtSBPC = (TextBox)row.FindControl("txtFieldValue");
                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(txtSBPC.Text) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv.RowFilter = "FieldCode='SBPC'";
                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    SystolicBPValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    SystolicBPValues = 0;
                                }
                                dv.RowFilter = "";
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "SBPS")
                            {
                                TextBox txtSBPS = (TextBox)row.FindControl("txtFieldValue");
                                txtSBPS.Text = common.myDec(SystolicBPValues).ToString("F0");
                            }
                            ////Temperature
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TC")
                            {
                                TextBox txtTC = (TextBox)row.FindControl("txtFieldValue");
                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(txtTC.Text) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv.RowFilter = "FieldCode='TC'";
                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    TemperatureValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    TemperatureValues = 0;
                                }
                                dv.RowFilter = "";
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TS")
                            {
                                TextBox txtTS = (TextBox)row.FindControl("txtFieldValue");
                                txtTS.Text = common.myDec(TemperatureValues).ToString("F0");
                            }
                            ////CNS assessment
                            if (common.myStr(hdnFieldType.Value) == "D" && common.myStr(hdnFieldCode.Value) == "CNSAC")
                            {

                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(ddl.SelectedValue) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv.RowFilter = "ValueId=" + common.myStr(ddl.SelectedValue) + " AND FieldCode ='CNSAC'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    CNSValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    CNSValues = 0;
                                }
                                dv.RowFilter = "";
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "CNSS")
                            {
                                TextBox txtCNSS = (TextBox)row.FindControl("txtFieldValue");
                                txtCNSS.Text = common.myDec(CNSValues).ToString("F0");
                            }

                            ////Total
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TOT")
                            {
                                ViewState["Calculate"] = true;
                                TextBox txtTOT = (TextBox)row.FindControl("txtFieldValue");
                                txtTOT.Text = common.myDec(SPO2CValues + RespiratoryRateValues + HeartRateValues + SystolicBPValues + TemperatureValues + CNSValues).ToString("F0");
                                if (txtTOT.Text == "")
                                {
                                    ViewState["Calculate"] = false;
                                }
                            }
                        }
                        else if (common.myStr(ddlTemplateMain.SelectedItem.Attributes["TemplateCode"]) == "EWSP")
                        {


                            if (common.myStr(hdnFieldType.Value) == "D" && common.myStr(hdnFieldCode.Value) == "BHR")
                            {
                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(ddl.SelectedValue) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv = new DataView(ds.Tables[4]);
                                dv.RowFilter = "ValueId=" + common.myStr(ddl.SelectedValue) + " AND FieldCode ='BHR'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    BHRValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    BHRValues = 0;
                                }
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "BHR")
                            {
                                TextBox txtBHR = (TextBox)row.FindControl("txtFieldValue");
                                txtBHR.Text = common.myDec(BHRValues).ToString("F0");
                            }
                            //CVR 

                            if (common.myStr(hdnFieldType.Value) == "D" && common.myStr(hdnFieldCode.Value) == "CVR")
                            {

                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(ddl.SelectedValue) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv = new DataView(ds.Tables[4]);
                                dv.RowFilter = "ValueId=" + common.myStr(ddl.SelectedValue) + " AND FieldCode ='CVR'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    CVRValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    CVRValues = 0;
                                }
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "CVR")
                            {
                                TextBox txtCVR = (TextBox)row.FindControl("txtFieldValue");
                                txtCVR.Text = common.myDec(CVRValues).ToString("F0");
                            }

                            //RPY

                            if (common.myStr(hdnFieldType.Value) == "D" && common.myStr(hdnFieldCode.Value) == "RPY")
                            {

                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(ddl.SelectedValue) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv = new DataView(ds.Tables[4]);
                                dv.RowFilter = "ValueId=" + common.myStr(ddl.SelectedValue) + " AND FieldCode ='RPY'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    RPYalues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    RPYalues = 0;
                                }
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "RPY")
                            {
                                TextBox txtRPY = (TextBox)row.FindControl("txtFieldValue");
                                txtRPY.Text = common.myDec(RPYalues).ToString("F0");
                            }
                            // TMN
                            if (common.myStr(hdnFieldType.Value) == "D" && common.myStr(hdnFieldCode.Value) == "TMN")
                            {

                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(ddl.SelectedValue) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv = new DataView(ds.Tables[4]);
                                dv.RowFilter = "ValueId=" + common.myStr(ddl.SelectedValue) + " AND FieldCode ='TMN'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    TMNValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    TMNValues = 0;
                                }
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TMN")
                            {
                                TextBox txtTMN = (TextBox)row.FindControl("txtFieldValue");
                                txtTMN.Text = common.myDec(TMNValues).ToString("F0");
                            }


                            //PVFS

                            if (common.myStr(hdnFieldType.Value) == "D" && common.myStr(hdnFieldCode.Value) == "PVFS")
                            {

                                if (common.myStr(hdnIsMandatory.Value) == "True" && common.myStr(ddl.SelectedValue) == "")
                                {
                                    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                                    return;
                                }
                                dv = new DataView(ds.Tables[4]);
                                dv.RowFilter = "ValueId=" + common.myStr(ddl.SelectedValue) + " AND FieldCode ='PVFS'";

                                if (dv.ToTable().Rows.Count > 0)
                                {
                                    PVFSValues = common.myDbl(dv.ToTable().Rows[0]["ScoreValue"]);
                                }
                                else
                                {
                                    PVFSValues = 0;
                                }
                            }
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "PVFS")
                            {
                                TextBox txtPVFS = (TextBox)row.FindControl("txtFieldValue");
                                txtPVFS.Text = common.myDec(PVFSValues).ToString("F0");
                            }

                            ////Total
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TOT")
                            {
                                ViewState["Calculate"] = true;
                                TextBox txtTOT = (TextBox)row.FindControl("txtFieldValue");
                                txtTOT.Text = common.myInt(BHRValues + CVRValues + RPYalues + TMNValues + PVFSValues).ToString("F0");
                                if (txtTOT.Text == "")
                                {
                                    ViewState["Calculate"] = false;
                                }
                            }

                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            ds.Dispose();
            emr = null;
            dsScoreValue.Dispose();
            sb = null;
            col = null;
            dv.Dispose();
        }
    }
    protected void gvScoreCard_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnFieldType = (HiddenField)e.Row.FindControl("hdnFieldType");
                HiddenField hdnFieldId = (HiddenField)e.Row.FindControl("hdnFieldId");
                Label lblFieldName = (Label)e.Row.FindControl("lblFieldName");
                TextBox txtFieldValue = (TextBox)e.Row.FindControl("txtFieldValue");
                TextBox txtM = (TextBox)e.Row.FindControl("txtM");
                Button btnScoreCalculate = (Button)e.Row.FindControl("btnScoreCalculate");
                ImageButton ibtnEWSGraph = (ImageButton)e.Row.FindControl("ibtnEWSGraph");

                DropDownList ddlDropDown = (DropDownList)e.Row.FindControl("ddlDropDown");
                Label lblLastValue = (Label)e.Row.FindControl("lblLastValue");
                HiddenField hdnIsMandatory = (HiddenField)e.Row.FindControl("hdnIsMandatory");
                var spnId = e.Row.FindControl("spnId");
                spnId.Visible = false;
                if (common.myBool(hdnIsMandatory.Value))
                {
                    spnId.Visible = true;
                }

                txtFieldValue.Visible = true;
                btnScoreCalculate.Visible = false;
                ddlDropDown.Visible = false;
                if (common.myStr(hdnFieldType.Value) == "H")
                {
                    txtFieldValue.Visible = false;
                    btnScoreCalculate.Visible = true;
                    lblFieldName.Font.Bold = true;
                }
                if (common.myStr(hdnFieldType.Value) == "D")
                {
                    txtFieldValue.Visible = false;
                    btnScoreCalculate.Visible = false;
                    ddlDropDown.Visible = true;
                    if (ViewState["TemplateData"] != null)
                    {
                        DataSet ds = (DataSet)ViewState["TemplateData"];

                        DataView dv = new DataView(ds.Tables[1]);
                        dv.RowFilter = "FieldId=" + hdnFieldId.Value + "";
                        ddlDropDown.DataSource = dv.ToTable();
                        ddlDropDown.DataTextField = "ValueName";
                        ddlDropDown.DataValueField = "ValueId";
                        ddlDropDown.DataBind();

                        ds.Dispose();
                        dv.Dispose();
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "T")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        txtM.Visible = false;
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                lblLastValue.Text = common.myStr(dv.ToTable().Rows[0]["TextValue"]);
                                if (common.myStr(dv.ToTable().Rows[0]["FieldCode"]).Equals("TOT"))
                                    ibtnEWSGraph.Visible = true;

                            }
                            dv.Dispose();
                            dt.Dispose();
                        }
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "M")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        txtM.Visible = true;
                        txtFieldValue.Visible = false;
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            if (dv.ToTable().Rows.Count > 0)
                            {
                                lblLastValue.Text = common.myStr(dv.ToTable().Rows[0]["TextValue"]);
                            }
                            dv.Dispose();
                            dt.Dispose();
                        }
                    }
                }
                if (common.myStr(hdnFieldType.Value) == "D")
                {
                    if (ViewState["TemplateData"] != null)
                    {
                        DataTable dt = ((DataSet)ViewState["TemplateData"]).Tables[2];
                        DataTable dt1 = ((DataSet)ViewState["TemplateData"]).Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dt);
                            dv.RowFilter = "FieldId=" + common.myInt(hdnFieldId.Value);
                            DataView dv1 = new DataView(dt1);
                            dv1.RowFilter = "ValueId=" + common.myStr(dv.ToTable().Rows[0]["FieldValue"]);
                            if (dv1.ToTable().Rows.Count > 0)
                            {
                                lblLastValue.Text = common.myStr(dv1.ToTable().Rows[0]["ValueName"]);


                            }
                            dv.Dispose();
                            dt.Dispose();
                            dt1.Dispose();
                            dv1.Dispose();
                        }
                    }
                }
                //if (common.myStr(Request.QueryString["TemplateCode"]) != "EWS")
                //{
                //    btnScoreCalculate.Visible = false;
                //}
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        saveData();
    }
    private void saveData()
    {
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        StringBuilder strNonTabular = new StringBuilder();
        ArrayList coll = new ArrayList();
        int RowCaptionId = 0;
        int SectionId = 0;
        int FieldId;
        string FieldType;
        try
        {
            if (!common.myBool(Session["isEMRSuperUser"]))
            {
                if (common.myBool(ViewState["IsEncounterClose"]) || !common.myBool(btnsave.Visible))
                {
                    return;
                }
            }

            if (common.myBool(ViewState["Calculate"]) == false)
            {
                Alert.ShowAjaxMsg("Please calculate score", Page);
                return;
            }
            if (ViewState["ScoreValue"] != null)
            {
                DataTable dt = (DataTable)ViewState["ScoreValue"];
                DataView dv = new DataView(dt);
                dv.RowFilter = "IsRemarksMandatory=1";
                if (dv.ToTable().Rows.Count > 0)
                {
                    string sRemarks = "";
                    decimal totalValue = 0;
                    string Fieldname = "";
                    foreach (GridViewRow item2 in gvScoreCard.Rows)
                    {
                        TextBox txtM = (TextBox)item2.FindControl("txtM");
                        TextBox txtFieldValue = (TextBox)item2.FindControl("txtFieldValue");
                        HiddenField hdnFieldCode = (HiddenField)item2.FindControl("hdnFieldCode");
                        HiddenField hdnFieldType = (HiddenField)item2.FindControl("hdnFieldType");

                        Label lblFieldName = (Label)item2.FindControl("lblFieldName");

                        Fieldname = lblFieldName.Text;

                        if (common.myStr(hdnFieldCode.Value) == "RMKS" && common.myStr(hdnFieldType.Value) == "M")
                        {
                            sRemarks = common.myStr(txtM.Text);
                        }
                        if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TOT")
                        {
                            totalValue = common.myDec(txtFieldValue.Text);
                        }
                        dv.RowFilter = "";
                        dv.RowFilter = "IsRemarksMandatory=1 AND FieldCode<>'TOT'";
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            if (common.myStr(hdnFieldCode.Value) == "RMKS" && sRemarks == "" && common.myStr(hdnFieldType.Value) == "M")
                            {
                                txtM.Focus();
                                Alert.ShowAjaxMsg("Reason: Field Name " + common.myStr(dv.ToTable().Rows[0]["FieldName"]) + " score " + common.myStr(dv.ToTable().Rows[0]["ScoreValue"]), Page);
                                return;
                            }
                        }
                        dv.RowFilter = "";
                        dv.RowFilter = "IsRemarksMandatory=1 AND FieldCode='TOT'";
                        if (dv.ToTable().Rows.Count > 0)
                        {
                            if (common.myStr(hdnFieldType.Value) == "T" && common.myStr(hdnFieldCode.Value) == "TOT"
                                && totalValue > common.myDec(dv.ToTable().Rows[0]["MinRange"]) && sRemarks == "")
                            {
                                txtM.Focus();
                                lblMessage.Text = "Reason: Field Name" + common.myStr(lblFieldName.Text) + " score " + totalValue;
                                return;
                            }
                        }
                    }
                }
                dt.Dispose();
                dv.Dispose();
            }
            #region Non Tabular
            RowCaptionId = 1;
            coll = new ArrayList();
            foreach (GridViewRow item2 in gvScoreCard.Rows)
            {
                SectionId = common.myInt(((HiddenField)item2.FindControl("hdnSectionId")).Value);
                FieldId = common.myInt(((HiddenField)item2.FindControl("hdnFieldId")).Value);
                FieldType = common.myStr(((HiddenField)item2.FindControl("hdnFieldType")).Value);
                HiddenField hdnIsMandatory = (HiddenField)item2.FindControl("hdnIsMandatory");
                Label lblFieldName = (Label)item2.FindControl("lblFieldName");

                if (FieldType == "T")
                {
                    TextBox txtT = (TextBox)item2.FindControl("txtFieldValue");
                    if (common.myStr(hdnIsMandatory.Value) == "True" && FieldType == "T" && common.myStr(txtT.Text) == "")
                    {
                        lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                        return;
                    }

                    coll.Add(FieldId);
                    coll.Add("T");
                    coll.Add(txtT.Text);
                    coll.Add("0");
                    coll.Add(RowCaptionId);
                    strNonTabular.Append(common.setXmlTable(ref coll));
                }
                if (FieldType == "M")
                {
                    TextBox txtM = (TextBox)item2.FindControl("txtM");
                    //if (common.myStr(hdnIsMandatory.Value) == "True" && FieldType == "M" && common.myStr(txtM.Text) == "")
                    //{
                    //    lblMessage.Text = "Please Enter " + lblFieldName.Text + " Field Value";
                    //    return;
                    //}

                    coll.Add(FieldId);
                    coll.Add("M");
                    coll.Add(txtM.Text);
                    coll.Add("0");
                    coll.Add(RowCaptionId);
                    strNonTabular.Append(common.setXmlTable(ref coll));
                }
                else if (FieldType == "D")
                {
                    DropDownList ddl = (DropDownList)item2.FindControl("ddlDropDown");
                    if (common.myStr(hdnIsMandatory.Value) == "True" && FieldType == "T" && common.myStr(ddl.SelectedValue) == "")
                    {
                        lblMessage.Text = "Please select " + lblFieldName.Text + " Field Value";
                        return;
                    }
                    coll.Add(FieldId);
                    coll.Add("D");
                    coll.Add(ddl.SelectedValue);
                    coll.Add("0");
                    coll.Add(RowCaptionId);
                    strNonTabular.Append(common.setXmlTable(ref coll));
                }
                else if (FieldType == "H")
                {
                    coll.Add(FieldId);
                    coll.Add("H");
                    coll.Add("-");
                    coll.Add("0");
                    coll.Add(RowCaptionId);
                    strNonTabular.Append(common.setXmlTable(ref coll));
                }
            }

            #endregion
            //int RecordId = bindVisitRecord(false);
            if (!strNonTabular.ToString().Equals(string.Empty))
            {
                string s = objEMR.SavePatientNotesData(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityID"]), 1,
                                            0, common.myInt(ddlTemplateMain.SelectedValue), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), 0,
                                            0, strNonTabular.ToString(), "", 1, SectionId, common.myInt(ddlRecord.SelectedValue), 0,
                                            common.myInt(Session["UserID"]), 0, 0, 0, common.myInt(Session["DoctorId"]), null, false, 0);
                if (s.ToUpper().Contains("SAVED"))
                {
                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cSucceedColor);
                    lblMessage.Text = s;
                    BindScoreCardTemplate();
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
            objEMR = null;
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
            DataSet ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(Session["EncounterId"]), common.myInt(ddlTemplateMain.SelectedValue), common.myInt(Session["FacilityId"]));
            //     DataSet ds = objivf.getEMRTemplateVisitRecoreds(common.myInt(ViewState["EncounterId"]), common.myInt(ViewState["PageId"]), common.myInt(Session["FacilityId"]));
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
                    }
                    else if (common.myInt(Request.QueryString["RecordId"]) > 0)
                    {
                        ddlRecord.SelectedIndex = ddlRecord.Items.IndexOf(ddlRecord.Items.FindItemByValue(common.myInt(Request.QueryString["RecordId"]).ToString()));
                    }

                    ViewState["IsDataSaved"] = false;
                    SetPermission();
                    lblLastEnteredBy.Text = string.Empty;
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
    protected void btnNew_OnClick(Object sender, EventArgs e)
    {
        Response.Redirect(common.myStr(Request.Url.AbsoluteUri), false);
    }
    protected void btnNewRecord_OnClick(object sender, EventArgs e)
    {
        bindVisitRecord(true);
    }
    protected void ddlRecord_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindScoreCardTemplate();
    }


    protected void ibtnEWSGraph_Click(object sender, ImageClickEventArgs e)
    {
        BaseC.Patient patient = new BaseC.Patient(sConString);
        DataSet ds = new DataSet();
        try
        {

            RadWindow1.NavigateUrl = "/EMR/Templates/EWSGraph.aspx?HospitalLocationId="
                                        + common.myInt(Session["HospitalLocationID"])
                                        + "&FacilityId=" + common.myInt(Session["FacilityID"])
                                        + "&EncounterId=" + common.myInt(Session["EncounterId"])
                                        + "&RegistrationId=" + common.myInt(Session["RegistrationId"])
                                        + "&TemplateId=" + ddlTemplateMain.SelectedValue
                                        + "&From=POPUP&DisplayMenu=1";
            RadWindow1.Height = 400;
            RadWindow1.Width = 500;
            RadWindow1.Top = 10;
            RadWindow1.Left = 10;
            RadWindow1.OnClientClose = string.Empty;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;
            RadWindow1.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
            RadWindow1.Title = "Early Warnning Score";
            RadWindow1.VisibleStatusbar = false;
            RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            ds.Dispose();
            patient = null;
        }
    }


}
