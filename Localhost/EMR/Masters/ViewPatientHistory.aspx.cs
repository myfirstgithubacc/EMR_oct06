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
using System.IO;
using System.Data.SqlClient;
using Telerik.Web.UI;


public partial class EMR_Masters_ViewPatientHistory : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    BaseC.EMRMasters objbc;
    DataSet ds;
    string Fonts = "";
    private int iPrevId = 0;
    clsExceptionLog objException = new clsExceptionLog();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);

            ViewState["RegistrationId"] = common.myStr(Request.QueryString["RegId"]) != "" ? common.myStr(Request.QueryString["RegId"]) : common.myStr(Session["RegistrationId"]);
            ViewState["EncounterId"] = common.myStr(Request.QueryString["EncId"]) != "" ? common.myStr(Request.QueryString["EncId"]) : common.myStr(Session["EncounterId"]);
            ViewState["OPIP"] = common.myStr(Request.QueryString["OPIP"]) != "" ? common.myStr(Request.QueryString["OPIP"]) : common.myStr(Session["OPIP"]);
            txtFromDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);
            txtToDate.DateInput.DateFormat = common.myStr(Session["OutputDateFormat"]);

           if (common.myStr(Request.QueryString["From"]) == "POPUP")
            {
                btnClosePopup.Visible = true;
                btnClose.Visible = false;
            }
            else
            {
                btnClose.Visible = true;
                btnClosePopup.Visible = false;
            }

           
           
            if (ViewState["EncounterId"].ToString() != "" && ViewState["EncounterId"] != null)
            {
               
                bool  bSign = GetStatus();
                if (bSign == true)
                {

                    DataSet ds = new DataSet();
                    ds = objbc.GetViewHistory(common.myInt(ViewState["RegistrationId"]), common.myInt(ViewState["EncounterId"]));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        editorBody.Content = common.myStr(ds.Tables[0].Rows[0]["patientsummary"]);
                    }
                }
                else
                {
                    //RTF1.EditModes = EditModes.Design;
                    editorBody.Content = BindEditor(bSign);
                }
                // BindEditor(false);
            }
            else if (common.myStr(Request.QueryString["ViewAll"]) != "")
            {
                ds = objbc.GetViewHistory(common.myInt(ViewState["RegistrationId"]), 0);
                editorBody.Content = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int ridx = 0; ridx < ds.Tables[0].Rows.Count; ridx++)
                    {
                        editorBody.Content = editorBody.Content + "<br />" + common.myStr(ds.Tables[0].Rows[ridx]["patientsummary"]);
                    }
                }
                else
                {
                    editorBody.Content = "<span style=\"color:Red; font-size: 21px;\">Not Available</span>";
                }
            }

            if (ViewState["OPIP"] != null && common.myStr(ViewState["OPIP"]) == "I")
            {
                if (Session["AdmissionDate"] != null)
                {
                    //txtFromDate.SelectedDate = Convert.ToDateTime(Session["AdmissionDate"]);
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                }
                else
                {
                    txtFromDate.SelectedDate = DateTime.Now;
                    txtToDate.SelectedDate = DateTime.Now;
                }

            }
            else
            {
                txtFromDate.SelectedDate = DateTime.Now;
                txtToDate.SelectedDate = DateTime.Now;

            }
            bindPatientTemplateList();
        }

    }

    protected void btnClosePopup_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("/MRD/Diagnosis.aspx?From=POPUP");
    }

    bool GetStatus()
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        int StatusId = 0;
        Hashtable hstInput = new Hashtable();
        hstInput.Add("@intencounterid", common.myInt(ViewState["EncounterId"]));
        hstInput.Add("@intFormId", common.myInt("1"));
        string sql = "Select StatusId, FormName from EMRPatientForms epf inner join EMRForms ef on  epf.FormId = ef.Id"
            + " where EncounterId = @intencounterid"
            + " And FormId = @intFormId AND epf.Active=1"
            + " Select StatusId, Status, Code From GetStatus(1,'Notes') where Code='P'"
            + " Select StatusId, Status, Code From GetStatus(1,'Notes') where Code='S'"
            + " select s.code from encounter e inner join statusmaster s on e.statusid = s.statusid where e.id = @intencounterid and e.active = 1 ";

        DataSet dsStatusId = dl.FillDataSet(CommandType.Text, sql, hstInput);
        if (dsStatusId.Tables[1].Rows.Count > 0)
        {
            hdnUnSignedId.Value = common.myStr(dsStatusId.Tables[1].Rows[0]["StatusId"]);
        }
        if (dsStatusId.Tables[2].Rows.Count > 0)
        {
            hdnSignedId.Value = common.myStr(dsStatusId.Tables[2].Rows[0]["StatusId"]);
        }
        if (dsStatusId.Tables[0].Rows.Count > 0)
        {
            //lblFormName.Text = common.myStr(dsStatusId.Tables[0].Rows[0]["FormName"]);
            StatusId = common.myInt(dsStatusId.Tables[0].Rows[0]["StatusId"]);
            ViewState["StatusID"] = StatusId;
        }
        //if (dsStatusId.Tables[3].Rows.Count > 0)
        //{
        //    if (common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "S")
        //    {
        //        btnSeenByDoctor.Text = "Patient UnSeen";
        //    }
        //    else if (common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "O"
        //        || common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "VT")
        //    {
        //        btnSeenByDoctor.Text = "Patient Seen";
        //    }
        //    else
        //    {
        //        btnSeenByDoctor.Visible = false;
        //    }
        //}

        if (StatusId == common.myInt(hdnSignedId.Value))
        {
            //btnSentenceGallery.Enabled = false;
            //btnSave.Enabled = false;
            //btnSigned.Text = "UnSign";
            //btnSeenByDoctor.Enabled = false;
            //btnAddendum.Enabled = true;
            //chkUpdateTempData.Enabled = false;

            editorBody.EditModes = Telerik.Web.UI.EditModes.Preview;

            return true;
        }
        else if (StatusId == common.myInt(hdnUnSignedId.Value))
        {
            //btnSigned.Text = "Sign";
            //btnSentenceGallery.Enabled = true;
            //btnSave.Enabled = true;
            //btnAddendum.Enabled = false;
            //btnSeenByDoctor.Enabled = true;
            //chkUpdateTempData.Enabled = true;
            // RTF1.EditModes = EditModes.Design;
            return false;
        }
        else
        {
            //btnSigned.Text = "Sign";
            //btnSentenceGallery.Enabled = true;
            //btnSave.Enabled = true;
            //btnAddendum.Enabled = false;
            //btnSeenByDoctor.Enabled = true;
            //chkUpdateTempData.Enabled = true;
            //RTF1.EditModes = EditModes.Design;
            return false;
        }


    }
    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", item[typ + "FontStyle"].ToString());
        if (FontName != "")
            sBegin += " font-family: " + FontName + ";";
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", Session["HospitalLocationId"].ToString());
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }
    private void GetFonts()
    {
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        DataSet ds = fonts.GetFontDetails("Size");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                editorBody.RealFontSizes.Add(dr["FontSize"].ToString());
            }
        }
    }
    string sFontSize = "";
    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", Session["HospitalLocationId"].ToString());
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
                sFontSize = " font-size: " + sFontSize + ";";
        }
        return sFontSize;
    }
    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "")
        {
            sBegin += "<span style='";
            if (item[typ + "FontSize"].ToString() != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (item[typ + "Forecolor"].ToString() != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (item[typ + "FontStyle"].ToString() != "")
            { sBegin += GetFontFamily(typ, item); }
        }

        if (item[typ + "Bold"].ToString() == "True")
        { sBegin += " font-weight: bold;"; }
        if (item[typ + "Italic"].ToString() == "True")
        { sBegin += " font-style: italic;"; }
        if (item[typ + "Underline"].ToString() == "True")
        { sBegin += " text-decoration: underline;"; }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
            sBegin += " '>";
    }
    private string BindEditor(Boolean sign)
    {
        StringBuilder sb = new StringBuilder();
        if (Request.QueryString["ifId"] != "")
        {
           
            StringBuilder sbTemplateStyle = new StringBuilder();
            DataSet ds = new DataSet();
            DataSet dsTemplate = new DataSet();
            DataSet dsTemplateStyle = new DataSet();
            DataRow drTemplateStyle = null;
            DataTable dtTemplate = new DataTable();
            Hashtable hst = new Hashtable();
            string Templinespace = "";
            BaseC.DiagnosisDA fun;

            Int32 RegId = Convert.ToInt32(ViewState["RegistrationId"]);
            Int32 HospitalId = Convert.ToInt32(Session["HospitalLocationID"]);
            Int32 EncounterId = Convert.ToInt32(ViewState["EncounterId"]);
           Int16 UserId = Convert.ToInt16(Session["UserID"]);

            BindNotes bnotes = new BindNotes(sConString);
            fun = new BaseC.DiagnosisDA(sConString);

            string DoctorId = fun.GetDoctorId(HospitalId, UserId);
            //ds = bnotes.GetPatientEMRData(Convert.ToInt32(Session["encounterid"]));
            dsTemplateStyle = bnotes.GetTemplateStyle(Convert.ToInt16(Session["HospitalLocationId"]));



            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId,"0");

            dtTemplate = dsTemplate.Tables[0];
            sb.Append("<span style='" + Fonts + "'>");
            for (int i = 0; i < dtTemplate.Rows.Count; i++)
            {
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Complaints" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                    
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);

                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]));
                    sb.Append(sbTemp + "<br/>" + "<br/>");
                    drTemplateStyle = null;
                    Templinespace = "";

                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                   && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();

                    
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    

                    Templinespace = "";
                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Allergies" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                    
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, Session["HospitalLocationId"].ToString(), Convert.ToString(Session["UserID"]), dtTemplate.Rows[i]["PageID"].ToString(),0);
                    sb.Append(sbTemp);
                    drTemplateStyle = null;
                    Templinespace = "";

                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Vitals" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                   
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]), 0, "0");
                    sb.Append(sbTemp + "<br/>" + "<br/>");
                    drTemplateStyle = null;
                    Templinespace = "";

                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                    && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");

                    Templinespace = "";
                }
                
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Lab Test Result" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                   
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    //bnotes.BindLabTestResult(common.myInt(ViewState["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                    sb.Append(sbTemp);
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Diagnosis" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                   
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);

                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindAssessments(RegId, HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]), 0, "0");
                    sb.Append(sbTemp + "<br/>");
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
                         || (common.myInt(ddlTemplatePatient.SelectedValue) == 10004))
                    {
                        bnotes.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, UserId, DoctorId, sbTemp, 
                            sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]),"","", 0, "0","");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Prescription" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                    
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);

                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle, Page,
                        Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]), "", "",common.myStr(ViewState["OPIP"]),"");
                    sb.Append(sbTemp + "<br/>");
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Current Medication" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                   
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(Convert.ToInt32(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]));
                    sb.Append(sbTemp + "<br/>");
                    drTemplateStyle = null;
                    Templinespace = "";
                }

                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Order" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                    
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindOrders(Convert.ToInt32(ViewState["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]), "0");
                    sb.Append(sbTemp + "<br/>");
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Immunization"
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                    
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindImmunization(HospitalId.ToString(), Convert.ToInt32(ViewState["RegistrationId"]), Convert.ToInt32(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]));
                    sb.Append(sbTemp + "<br/>");
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                     bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Request.QueryString["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "","");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }

                    StringBuilder sbTemp = new StringBuilder();
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    Templinespace = "";
                }
                if (dtTemplate.Rows[i]["TemplateName"].ToString().Trim() == "Daily Injections" 
                    && dtTemplate.Rows[i]["DataStatus"].ToString().Trim() == "AVAILABLE")
                {
                    string strTemplateType = dtTemplate.Rows[i]["PageIdentification"].ToString();
                    strTemplateType = strTemplateType.Substring(0, 1);
                   
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + Convert.ToString(dtTemplate.Rows[i]["PageId"]);
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = Convert.ToString(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();
                    bnotes.BindInjection(HospitalId.ToString(), Convert.ToInt32(ViewState["RegistrationId"]), Convert.ToInt32(ViewState["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page, Convert.ToString(dtTemplate.Rows[i]["PageId"]), Convert.ToString(Session["UserID"]));
                    sb.Append(sbTemp + "<br/>");
                    Templinespace = "";
                }

                sb.Append("</span>");
            }
            //if (sign == true)
            //{
            //    sb.Append(hdnDoctorImage.Value);
            //}
            //else if (sign == false)
            //{
            //    if (RTF1.Content != null)
            //    {
            //        if (RTF1.Content.Contains("dvDoctorImage") == true)
            //        {
            //            string signData = RTF1.Content.Replace('"', '$');
            //            string st = "<div id=$dvDoctorImage$>";
            //            int start = signData.IndexOf(@st);
            //            if (start > 0)
            //            {
            //                int End = signData.IndexOf("</div>", start);
            //                StringBuilder sbte = new StringBuilder();
            //                sbte.Append(signData.Substring(start, (End + 6) - start));
            //                StringBuilder ne = new StringBuilder();
            //                ne.Append(signData.Replace(sbte.ToString(), ""));
            //                sb.Append(ne.Replace('$', '"').ToString());
            //            }
            //        }

            //    }
            //}
        }
        return sb.ToString();
    }
    private string BindStaticTemplates(int StaticTemplateId, int TemplateFieldId)
    {
        StringBuilder sbStatic = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        DataSet dsTemplate = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        Hashtable hst = new Hashtable();
        string Templinespace = "";
        BaseC.DiagnosisDA fun;

        int RegId = common.myInt(Session["RegistrationID"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(Session["encounterid"]);
        int UserId = common.myInt(Session["UserID"]);

        BindNotes bnotes = new BindNotes(sConString);
        fun = new BaseC.DiagnosisDA(sConString);

        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));

        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

        dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId,"0");
        DataView dvFilterStaticTemplate = new DataView(dsTemplate.Tables[0]);
        dvFilterStaticTemplate.RowFilter = "PageId=" + StaticTemplateId;
        dtTemplate = dvFilterStaticTemplate.ToTable();

        sb.Append("<span style='" + Fonts + "'>");

       if(dtTemplate.Rows.Count>0)
        {
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Allergies"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
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


                bnotes.BindAllergies(RegId, sbStatic, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]),"", "", TemplateFieldId,"");

                sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
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
                                    Page, common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", TemplateFieldId, "0","");

                sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
                && common.myStr(dtTemplate.Rows[0]["DataStatus"]).Trim() == "AVAILABLE")
            {
                string strTemplateType = common.myStr(dtTemplate.Rows[0]["PageIdentification"]);
                strTemplateType = strTemplateType.Substring(0, 1);
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
                            common.myStr(dtTemplate.Rows[0]["PageId"]), common.myStr(Session["UserID"]),
                            "", "", TemplateFieldId, "0","");

                sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
        }
       return "<br/>" + sbStatic.ToString();
    }
    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);
        if (Convert.ToInt16(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (Convert.ToInt16(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = ds.Tables[0].Rows[0]["EntryType"].ToString();
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);

        if (Convert.ToInt16(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (Convert.ToInt16(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        else
            hstInput.Add("chrGenderType", "M");
        if (sEntryType == "S")
        {
            hstInput.Add("@intRegistrationId", Convert.ToInt32(Session["RegistrationId"]));
        }
        else
        {
            hstInput.Add("@intEncounterId", Convert.ToInt32(ViewState["EncounterId"]));
        }
        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);


        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        foreach (DataRow item in ds.Tables[0].Rows)
        {

            DataTable dtFieldValue = new DataTable();
            DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
            dv1.RowFilter = "SectionId=" + Convert.ToString(item["SectionId"]);
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

                        ViewState["iTemplateId"] = Convert.ToInt32(item["TemplateId"]);
                        str = CreateString(dsAllFieldsDetails, Convert.ToInt32(item["TemplateId"]), item["TemplateName"].ToString(), item["Tabular"].ToString());
                        str += " ";
                        //}
                        if (iPrevId == Convert.ToInt32(item["TemplateId"]))
                        {
                            if (t2 == 0)
                                if (t3 == 0)//Template
                                {
                                    t3 = 1;
                                    if (item["SectionsListStyle"].ToString() == "1")
                                    { BeginList3 = "<ul>"; EndList3 = "</ul>"; }
                                    else if (item["SectionsListStyle"].ToString() == "2")
                                    { BeginList3 = "<ol>"; EndList3 = "</ol>"; }
                                }


                            if (item["SectionsBold"].ToString() != "" || item["SectionsItalic"].ToString() != "" || item["SectionsUnderline"].ToString() != "" || item["SectionsFontSize"].ToString() != "" || item["SectionsForecolor"].ToString() != "" || item["SectionsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                {
                                    if (str.Trim() != "")
                                        objStrTmp.Append(BeginList3 + sBegin + item["SectionName"].ToString() + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                }
                                BeginList3 = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                {
                                    if (str.Trim() != "")
                                        objStrTmp.Append(item["SectionName"].ToString()); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                }
                            }

                            if (item["SectionsListStyle"].ToString() == "3" || item["TemplateListStyle"].ToString() == "0")
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
                                if (item["TemplateListStyle"].ToString() == "1")
                                { BeginList = "<ul>"; EndList = "</ul>"; }
                                else if (item["TemplateListStyle"].ToString() == "2")
                                { BeginList = "<ol>"; EndList = "</ol>"; }
                            }
                            if (item["TemplateBold"].ToString() != "" || item["TemplateItalic"].ToString() != "" || item["TemplateUnderline"].ToString() != "" || item["TemplateFontSize"].ToString() != "" || item["TemplateForecolor"].ToString() != "" || item["TemplateListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Template", ref sBegin, ref sEnd, item);
                                if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    if (sBegin.Contains("<br/>") == true)
                                    {
                                        sBegin = sBegin.Remove(0, 5);
                                        objStrTmp.Append(BeginList + sBegin + item["TemplateName"].ToString() + sEnd);
                                    }
                                    else
                                    {
                                        objStrTmp.Append(BeginList + sBegin + item["TemplateName"].ToString() + sEnd);
                                    }
                                BeginList = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    objStrTmp.Append(item["TemplateName"].ToString());//Default Setting
                            }
                            if (item["TemplateListStyle"].ToString() == "3" || item["TemplateListStyle"].ToString() == "0")
                                objStrTmp.Append("<br />");
                            objStrTmp.Append(EndList);
                            if (t2 == 0)
                            {
                                t2 = 1;
                                if (item["SectionsListStyle"].ToString() == "1")
                                { BeginList2 = "<ul>"; EndList3 = "</ul>"; }
                                else if (item["SectionsListStyle"].ToString() == "2")
                                { BeginList2 = "<ol>"; EndList3 = "</ol>"; }
                            }
                            if (item["SectionsBold"].ToString() != "" || item["SectionsItalic"].ToString() != "" || item["SectionsUnderline"].ToString() != "" || item["SectionsFontSize"].ToString() != "" || item["SectionsForecolor"].ToString() != "" || item["SectionsListStyle"].ToString() != "")
                            {
                                sBegin = ""; sEnd = "";
                                MakeFont("Sections", ref sBegin, ref sEnd, item);
                                if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    if (str.Trim() != "") //add 19June2010
                                    {
                                        objStrTmp.Append(BeginList2 + sBegin + item["SectionName"].ToString() + sEnd);
                                    }
                                BeginList2 = "";
                            }
                            else
                            {
                                if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    objStrTmp.Append(item["SectionName"].ToString()); //Comment On 19June2010
                            }
                            if (item["SectionsListStyle"].ToString() == "3" || item["SectionsListStyle"].ToString() == "0")
                                objStrTmp.Append("<br />");

                            objStrTmp.Append(str);
                        }
                        iPrevId = Convert.ToInt32(item["TemplateId"]);
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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            objbc = new BaseC.EMRMasters(sConString);
            StringBuilder sb = new StringBuilder();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            Session["RTF"] = editorBody.Content;

            RadWindow1.NavigateUrl = "~/Editor/PrintPdf.aspx";
            RadWindow1.Height = 555;
            RadWindow1.Width = 800;
            RadWindow1.Top = 40;
            RadWindow1.Left = 100;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
            RadWindow1.Modal = true;

            RadWindow1.VisibleStatusbar = false;
            RadWindow1.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
            RadWindow1.VisibleStatusbar = false; // Set this property to True for showing window from code    

        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }

    }
    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (item[typ + "ListStyle"].ToString() == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (item[typ + "ListStyle"].ToString() == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (ViewState["iTemplateId"].ToString() != "163" && typ != "Fields")
            { sBegin += "<br/>"; }
            else if (ViewState["iTemplateId"].ToString() == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
                sBegin += "<br/>";
        }


        if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "")
        {
            sBegin += "<span style='";
            if (item[typ + "FontSize"].ToString() != "")
            { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sBegin += getDefaultFontSize(); }
            if (item[typ + "Forecolor"].ToString() != "")
            { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (item[typ + "FontStyle"].ToString() != "")
            { sBegin += GetFontFamily(typ, item); }
        }
        if (item[typ + "Bold"].ToString() == "True")
        { sBegin += " font-weight: bold;"; }
        if (item[typ + "Italic"].ToString() == "True")
        { sBegin += " font-style: italic;"; }
        if (item[typ + "Underline"].ToString() == "True")
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
    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        ArrayList aEnd = new ArrayList();
        if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "")
        {
            if (item[typ + "FontSize"].ToString() != "")
            { sFontSize += " font-size:" + item[typ + "FontSize"] + ";"; }
            else { sFontSize += getDefaultFontSize(); }
            if (item[typ + "Forecolor"].ToString() != "")
            { sFontSize += " color: #" + item[typ + "Forecolor"] + ";"; }
            if (item[typ + "FontStyle"].ToString() != "")
            { sFontSize += GetFontFamily(typ, item); };

            if (item[typ + "Bold"].ToString() == "True")
            { sFontSize += " font-weight: bold;"; }
            if (item[typ + "Italic"].ToString() == "True")
            { sFontSize += " font-style: italic;"; }
            if (item[typ + "Underline"].ToString() == "True")
            { sFontSize += " text-decoration: underline;"; }

        }
        return sFontSize;
    }
    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType)
    {
        StringBuilder objStr = new StringBuilder();
        DataView objDv = null;
        DataTable objDt = null;
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        int MaxLength = 0, FieldsLength = 0;
        if (objDs != null)
        {
            if (bool.Parse(TabularType) == true)
            {
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = objDs.Tables[0].Rows[0];
                    DataView dvValues = new DataView(objDs.Tables[1]);
                    dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();

                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();
                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {


                            //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                            objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                            FieldsLength = objDs.Tables[0].Rows.Count;


                            if (objDs.Tables[0].Rows[0]["TRows"].ToString().Trim() != "" && objDs.Tables[0].Rows[0]["TRows"].ToString().Trim() != "0")
                            {
                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");

                            }

                            for (int i = 0; i < FieldsLength; i++)   // it makes table
                            {

                                objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + objDs.Tables[0].Rows[i]["FieldName"].ToString() + "</th>");

                                dr = objDs.Tables[0].Rows[i];
                                dvValues = new DataView(objDs.Tables[1]);
                                dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                                dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));


                                if (dvValues.ToTable().Rows.Count > MaxLength)
                                    MaxLength = dvValues.ToTable().Rows.Count;
                            }



                            objStr.Append("</tr>");
                            if (MaxLength == 0)
                            {
                                //objStr.Append("<tr>");
                                //for (int i = 0; i < FieldsLength; i++)
                                //{
                                //    objStr.Append("<td style=' " + sFontSize + "'> </td>");
                                //}
                                //objStr.Append("</tr></table>");
                            }


                            else
                            {
                                if (dsMain.Tables[0].Rows.Count > 0)
                                {
                                    //for (int i = 0; i < MaxLength; i++)
                                    for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                                    {
                                        objStr.Append("<tr>");
                                        if (dsMain.Tables[0].Rows[i]["RowCaption"].ToString() != "" && dsMain.Tables[0].Rows[i]["RowCaption"].ToString() != "0")
                                        {
                                            objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) + "</td>");
                                        }
                                        //else
                                        //{
                                        //     objStr.Append("<td style=' " + sFontSize + "' align='right'>&nbsp;</td>");
                                        //}

                                        for (int j = 0; j < dsMain.Tables.Count; j++)
                                        {
                                            if (dsMain.Tables[j].Rows.Count > i
                                                && dsMain.Tables[j].Rows.Count > 0)
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dsMain.Tables[j].Rows[i]["TextValue"]) + "</td>");
                                            }
                                            else
                                            {
                                                objStr.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                            }
                                        }
                                        objStr.Append("</tr>");
                                    }
                                    objStr.Append("</table>");
                                }
                            }
                        }
                    }
                    else
                    {
                        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (Convert.ToInt16(Session["Gender"]) == 1)
                            hstInput.Add("chrGenderType", "F");
                        else if (Convert.ToInt16(Session["Gender"]) == 2)
                            hstInput.Add("chrGenderType", "M");
                        else
                            hstInput.Add("chrGenderType", "M");

                        hstInput.Add("@intEncounterId", Convert.ToInt32(ViewState["EncounterId"]));

                        DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                        DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);

                        dvRowCaption.RowFilter = "RowCaptionId>0";
                        if (dvRowCaption.ToTable().Rows.Count > 0)
                        {
                            StringBuilder sbCation = new StringBuilder();
                            dvRowCaption.RowFilter = "RowNum>0";
                            DataTable dt = dvRowCaption.ToTable();
                            if (dt.Rows.Count > 0)
                            {
                                sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                int column = dt.Columns.Count;
                                int ColumnCount = 0;
                                int count = 1;
                                for (int k = 1; k < column - 4; k++)
                                {
                                    if (dt.Rows[0]["RowCaptionName"].ToString() == "" && ColumnCount == 0)
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(" + ");
                                        sbCation.Append("</td>");
                                    }
                                    else
                                    {
                                        sbCation.Append("<td>");
                                        sbCation.Append(dt.Rows[0]["Col" + count].ToString());
                                        sbCation.Append("</td>");
                                        count++;
                                    }
                                    ColumnCount++;
                                }
                                sbCation.Append("</tr>");

                                DataView dvRow = new DataView(dt);
                                dvRow.RowFilter = "RowCaptionId>0";
                                DataTable dtRow = dvRow.ToTable();
                                for (int l = 1; l <= dtRow.Rows.Count; l++)
                                {
                                    sbCation.Append("<tr>");
                                    for (int i = 0; i < ColumnCount; i++)
                                    {
                                        if (i == 0)
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["RowCaptionName"].ToString() + "</td>");
                                        }
                                        else
                                        {
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                        }
                                    }
                                    sbCation.Append("</tr>");
                                }
                                sbCation.Append("</table>");
                            }
                            objStr.Append(sbCation);
                        }

                    }
                }
            }
            else // For Non Tabular Templates
            {
                string BeginList = "", EndList = "";
                string sBegin = "", sEnd = "";
                int t = 0;
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
                    objDv = new DataView(objDs.Tables[1]);
                    objDv.RowFilter = "FieldId='" + item["FieldId"].ToString() + "'";
                    objDt = objDv.ToTable();
                    if (t == 0)
                    {
                        t = 1;
                        if (item["FieldsListStyle"].ToString() == "1")
                        { BeginList = "<ul>"; EndList = "</ul>"; }
                        else if (item["FieldsListStyle"].ToString() == "2")
                        { BeginList = "<ol>"; EndList = "</ol>"; }
                    }
                    if (item["FieldsBold"].ToString() != "" || item["FieldsItalic"].ToString() != "" || item["FieldsUnderline"].ToString() != "" || item["FieldsFontSize"].ToString() != "" || item["FieldsForecolor"].ToString() != "" || item["FieldsListStyle"].ToString() != "")
                    {
                        //rafat1
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                            {
                                objStr.Append(BeginList + sBegin + item["FieldName"].ToString());
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                    objStr.Append(sEnd + "</li>");
                                //}
                            }
                            BeginList = "";
                            sBegin = "";
                        }

                    }
                    else
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                                objStr.Append(item["FieldName"].ToString());
                        }
                    }
                    if (objDs.Tables.Count > 1)
                    {
                        objDv = new DataView(objDs.Tables[1]);
                        objDv.RowFilter = "FieldId='" + item["FieldId"].ToString() + "'";
                        objDt = objDv.ToTable();
                        DataView dvFieldType = new DataView(objDs.Tables[0]);
                        dvFieldType.RowFilter = "FieldId='" + item["FieldId"].ToString() + "'";
                        DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            if (objDt.Rows.Count > 0)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = dtFieldType.Rows[0]["FieldType"].ToString();
                                if (FType == "C")
                                    FType = "C";
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (objDt.Rows[i]["FieldValue"].ToString() == "1" || objDt.Rows[i]["FieldValue"].ToString() == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (objDs.Tables[1].Rows[i]["FieldValue"].ToString() == "1")

                                                dv1.RowFilter = "TextValue='Yes'";
                                            else
                                                dv1.RowFilter = "TextValue='No'";
                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (dt1.Rows[i]["MainText"].ToString().Trim() != "")
                                                {
                                                    if (i == 0)
                                                        objStr.Append(" " + dt1.Rows[i]["MainText"].ToString());
                                                    else
                                                        objStr.Append(", " + dt1.Rows[i]["MainText"].ToString());
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                        objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                                    else
                                                        objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                    objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                                else
                                                    objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
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
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                {
                                    if (ViewState["iTemplateId"].ToString() != "163")
                                    {
                                        if (i == 0)
                                            objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                                        else
                                            objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                    }
                                    else
                                    {
                                        if (i == 0)
                                            objStr.Append(": " + objDt.Rows[i]["TextValue"].ToString());
                                        else
                                            objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
                                    }
                                }
                                else if (FType == "L")
                                {
                                    objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                }
                                if (item["FieldsListStyle"].ToString() == "")
                                    if (ViewState["iTemplateId"].ToString() != "163")
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
                    }
                    //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                }

                if (objStr.ToString() != "")
                    objStr.Append(EndList);
            }
        }

        return objStr.ToString();
    }
    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null && iFormId != "")
        {
            if (Cache[Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                Hashtable hstInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", Convert.ToInt16(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", iFormId);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }
    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType)
    {

        if (i == 0)
            objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
        else
        {
            if (FType != "C")
                objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
            else
            {
                if (i == 0)
                    objStr.Append(" " + objDt.Rows[i]["TextValue"].ToString());
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                    objStr.Append(" and " + objDt.Rows[i]["TextValue"].ToString() + ".");
                else
                    objStr.Append(", " + objDt.Rows[i]["TextValue"].ToString());
            }
        }
        //}
    }

    //add by Balkishan start


    private void bindPatientTemplateList()
    {
        try
        {
            DataSet ds = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
            ds = objEMR.getTemplateEnteredList(common.myInt(Session["HospitalLocationId"]),
                            common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]));

            DataRow DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Vitals";
            DR["TemplateId"] = "10001";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Allergies";
            DR["TemplateId"] = "10002";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Chief Complaints";
            DR["TemplateId"] = "10003";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Diagnosis";
            DR["TemplateId"] = "10004";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Prescription";
            DR["TemplateId"] = "10005";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Orders And Procedures";
            DR["TemplateId"] = "10006";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Immunization";
            DR["TemplateId"] = "10007";
            ds.Tables[0].Rows.Add(DR);

            DR = ds.Tables[0].NewRow();
            DR["TemplateName"] = "Daily Injections";
            DR["TemplateId"] = "10008";
            ds.Tables[0].Rows.Add(DR);

            ddlTemplatePatient.DataSource = ds.Tables[0];
            ddlTemplatePatient.DataTextField = "TemplateName";
            ddlTemplatePatient.DataValueField = "TemplateId";
            ddlTemplatePatient.DataBind();
            ddlTemplatePatient.Items.Insert(0, new RadComboBoxItem("ALL", "0"));

            ddlTemplatePatient.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    private string BindPatientIllustrationImages()
    {

        hdnImagePath.Value = "";
        if (hdnImagePath.Value != "")
        {
            editorBody.Content = "<table><tbody><tr><td><img src='" + hdnImagePath.Value + "' width='250px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table>" + editorBody.Content;
        }
        else
        {
            BaseC.EMR emr = new BaseC.EMR(sConString);
            DataSet ds = emr.GetPatientIllustrationImages(Convert.ToInt32(Session["HospitalLocationId"]), Convert.ToInt32(Session["FacilityId"]), Convert.ToInt32(Session["RegistrationId"]), Convert.ToInt32(Session["EncounterId"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("/PatientDocuments/" + Convert.ToInt32(Session["HospitalLocationId"]) + "/" + Convert.ToInt32(Session["RegistrationId"]) + "/" + Session["EncounterId"].ToString() + "/"));
                if (objDir.Exists == true)
                {
                    string path = "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Convert.ToString(Session["EncounterId"]) + "/";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        hdnImagePath.Value = hdnImagePath.Value + " <table><tbody><tr><td><img src='" + path + ds.Tables[0].Rows[i]["ImageName"].ToString() + "' width='250px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table><br/>";
                    }
                    editorBody.Content = hdnImagePath.Value + BindEditor(false);
                }
            }
            else
            {
                editorBody.Content = BindEditor(false);
            }
        }
        hdnImagePath.Value = "";
        return editorBody.Content;
    }

    private void bindDetails()
    {
        try
        {
            bool bSign = GetStatus();
            if (bSign)
            {
                BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);
                DataSet ds = new DataSet();
                ds = objbc.GetViewHistory(common.myInt(ViewState["RegistrationID"]), common.myInt(ViewState["EncounterId"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    editorBody.Content = common.myStr(ds.Tables[0].Rows[0]["patientsummary"]);
                }
            }
            else
            {
                editorBody.Content = BindPatientIllustrationImages();
            }
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }


    protected void btnRefreshData_OnClick(object sender, EventArgs e)
    {
        try
        {
            bindDetails();
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
    }

    //add by Balkishan end

}
