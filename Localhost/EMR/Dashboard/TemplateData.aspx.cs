using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Drawing;
using BaseC;


public partial class EMR_Dashboard_PatientParts_TemplateData : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private Hashtable hstInput;
    string Saved_RTF_Content;
    StringBuilder sb = new StringBuilder();
    string Fonts = "";
    static string gBegin = "<u>";
    static string gEnd = "</u>";
    StringBuilder objStrTmp = new StringBuilder();
    private int iPrevId = 0;
    string sFontSize = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setTemplateData();
        }
    }

    private void setTemplateData()
    {
        BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);
        DataSet ds = new DataSet();

        try
        {
            tdTemplate.InnerHtml = BindEditor(true);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;
            objException.HandleException(Ex);
        }
        finally
        {
            objbc = null;
            ds.Dispose();
        }
    }

    private string BindEditor(Boolean sign)
    {
        if (Request.QueryString["ifId"] != "")
        {
            string sEREncounterId = common.myStr(Session["EncounterId"]);

            StringBuilder sbTemplateStyle = new StringBuilder();
            DataSet ds = new DataSet();
            DataSet dsTemplate = new DataSet();
            DataSet dsTemplateStyle = new DataSet();
            DataRow drTemplateStyle = null;
            DataTable dtTemplate = new DataTable();
            Hashtable hst = new Hashtable();
            string Templinespace = "";
            BaseC.DiagnosisDA fun;

            int RegId = common.myInt(Session["RegistrationID"]);
            int HospitalId = common.myInt(Session["HospitalLocationID"]);
            int EncounterId = common.myInt(Session["EncounterId"]);
            int UserId = common.myInt(Session["UserID"]);

            BindNotes bnotes = new BindNotes(sConString);
            fun = new BaseC.DiagnosisDA(sConString);

            string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
            
            //ds= bnotes.GetPatientEMRData(common.myInt(Session["encounterid"]));
            
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            clsIVF objivf = new clsIVF(sConString);

            dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
            DataView dvTemplate = dsTemplate.Tables[0].DefaultView;

            if (common.myLen(Request.QueryString["TemplateId"]) > 0)
            {
                dvTemplate.RowFilter = "TemplateId=" + common.myInt(Request.QueryString["TemplateId"]);
            }
            else
            {
                dvTemplate.RowFilter = "Sequence=1 OR (TemplateId>0 AND ShowInOrderPage=1)";
            }

            dtTemplate = dvTemplate.ToTable();

            sb.Append("<span style='" + Fonts + "'>");

            string strTemplatePatient = "0";

            for (int i = 0; i < dtTemplate.Rows.Count; i++)
            {
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Complaints"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10003))
                    {
                        bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                       "", "","");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>" + "<br/>");
                    }

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

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
                    && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
                {
                    string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
                    strTemplateType = strTemplateType.Substring(0, 1);
                    sbTemplateStyle = new StringBuilder();
                    DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                    dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
                    drTemplateStyle = null;// = dv[0].Row;
                    if (dv.Count > 0)
                    {
                        drTemplateStyle = dv[0].Row;
                        string sBegin = "", sEnd = "";
                        Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10002))
                    {
                        //bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                        //            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
                        //            "",
                        //            "",0);
                        bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
                                   common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),"","", 0,"");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10001))
                    {
                        bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
                                            Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                            "",
                                            "", 0, sEREncounterId,"");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>" + "<br/>");
                    }

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

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
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

                    // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
                    sb.Append(sbTemp);
                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10004))
                    {
                        bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "", 0, sEREncounterId,"");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10004))
                    {
                        bnotes.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
                                    DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "", "", 0, sEREncounterId,"");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10005))
                    {
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
                                       Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                       "",
                                       "", Session["OPIP"].ToString(),""
                                       );

                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
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
                        Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
                        MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                    }
                    StringBuilder sbTemp = new StringBuilder();

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10005))
                    {
                        bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
                                        Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                        "",
                                        "", Session["OPIP"].ToString(),"");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Order"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10006))
                    {
                        bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
                                Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                "",
                                "", sEREncounterId,"");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    drTemplateStyle = null;
                    Templinespace = "";
                }
                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10007))
                    {
                        bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "","");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10007))
                    {
                        bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "",
                                    "","");
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

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

                    if ((common.myInt(strTemplatePatient) == 0)
                        || (common.myInt(strTemplatePatient) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                    {
                        bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
                        if (sbTemp.ToString() != "")
                            sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }

                if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
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

                    if ((common.myInt(strTemplatePatient) == 0)
                         || (common.myInt(strTemplatePatient) == 10008))
                    {
                        bnotes.BindInjection(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
                                    common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
                                    common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
                                    "","","");

                        sb.Append(sbTemp + "<br/>");
                    }

                    Templinespace = "";
                }
                sb.Append("</span>");
            }
            drTemplateStyle = dsTemplateStyle.Tables[0].Rows[0];

            //StringBuilder temp = new StringBuilder();
            //bnotes.GetEncounterFollowUpAppointment(Session["HospitalLocationId"].ToString(), common.myInt(Session["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
            //sb.Append(temp);





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
        if (common.myInt(Session["Gender"]) == 1)
            hstInput.Add("chrGenderType", "F");
        else if (common.myInt(Session["Gender"]) == 2)
            hstInput.Add("chrGenderType", "M");
        hstInput.Add("@intFormId", common.myStr(1));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        //string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", Session["RegistrationID"]);
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
                strGender = "He";
            else
                strGender = "She";
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
            string strDisplayUserName = "select DisplayUserName from EMRTemplate where ID=@intTemplateId and HospitalLocationID=@inyHospitalLocationID";
            DataSet dsDisplayName = DlObj.FillDataSet(CommandType.Text, strDisplayUserName, hshtable);
            if (dsDisplayName.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(dsDisplayName.Tables[0].Rows[0]["DisplayUserName"]).ToUpper() == "TRUE")
                {
                    Hashtable hshUser = new Hashtable();
                    hshUser.Add("@UserID", common.myInt(ds.Tables[0].Rows[0]["EncodedBy"]));
                    hshUser.Add("@inyHospitalLocationID", common.myStr(Session["HospitalLocationID"]));
                    string strUser = "Select ISNULL(FirstName,'') + '' + ISNULL(MiddleName,'') + '' + ISNULL(LastName,'') AS EmployeeName  FROM Employee em INNER JOIN Users us ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";

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


    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myStr(Session["HospitalLocationId"]));
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
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myStr(Session["HospitalLocationId"]));
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
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
                // string str = t.Substring(RosSt, (RosEnd + 6) - RosSt);
                string str = t.Substring(RosSt, (RosEnd) - RosSt);
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

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType, string sectionId, string EntryType, int RecordId)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

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
                    dvValues.RowFilter = "FieldId='" + common.myStr(dr["FieldId"]) + "'";
                    MaxLength = dvValues.ToTable().Rows.Count;
                    DataTable dtFilter = dvValues.ToTable();

                    DataView dvFilter = new DataView(dtFilter);
                    dvFilter.RowFilter = "RowCaption='0'";
                    DataTable dtNewTable = dvFilter.ToTable();

                    if (dtNewTable.Rows.Count > 0)
                    {
                        if (MaxLength != 0)
                        {
                            if (EntryType != "M")
                            {
                                //objStr.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                objStr.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");


                                FieldsLength = objDs.Tables[0].Rows.Count;


                                if (common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != ""
                                    && common.myStr(objDs.Tables[0].Rows[0]["TRows"]).Trim() != "0")
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >&nbsp;</th>");

                                }
                                for (int i = 0; i < FieldsLength; i++)   // it makes table
                                {
                                    objStr.Append("<th align='center' style='border-bottom-color:Black; border-bottom-style:solid; border-bottom-width:1px; " + sFontSize + " ' >" + common.myStr(objDs.Tables[0].Rows[i]["FieldName"]) + "</th>");

                                    dr = objDs.Tables[0].Rows[i];
                                    dvValues = new DataView(objDs.Tables[1]);
                                    dvValues.RowFilter = "FieldId='" + dr["FieldId"].ToString() + "'";
                                    dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                                    if (dvValues.ToTable().Rows.Count > MaxLength)
                                    {
                                        MaxLength = dvValues.ToTable().Rows.Count;
                                    }
                                }
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
                                if (EntryType != "M")
                                {
                                    if (dsMain.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < MaxLength; i++)
                                        {
                                            objStr.Append("<tr>");
                                            if (common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != ""
                                                && common.myStr(dsMain.Tables[0].Rows[i]["RowCaption"]) != "0")
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
                                        }
                                    }
                                }
                                else
                                {
                                    Hashtable hstInput = new Hashtable();
                                    hstInput.Add("@intTemplateId", iRootId);

                                    if (common.myInt(Session["Gender"]) == 1)
                                    {
                                        hstInput.Add("chrGenderType", "F");
                                    }
                                    else if (common.myInt(Session["Gender"]) == 2)
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }
                                    else
                                    {
                                        hstInput.Add("chrGenderType", "M");
                                    }

                                    hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                                    hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
                                    hstInput.Add("@intRecordId", RecordId);
                                    DataSet dsTabulerTemplate = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", hstInput);

                                    DataView dvRowCaption = new DataView(dsTabulerTemplate.Tables[0]);
                                    StringBuilder sbCation = new StringBuilder();

                                    if (dvRowCaption.ToTable().Rows.Count > 0)
                                    {
                                        dvRowCaption.RowFilter = "RowNum>0";
                                        DataTable dt = dvRowCaption.ToTable();
                                        if (dt.Rows.Count > 0)
                                        {
                                            sbCation.Append("<br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellpadding='2' cellspacing='3' ><tr align='center'>");
                                            int column = dt.Columns.Count;
                                            int ColumnCount = 0;
                                            int count = 1;

                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            for (int k = 1; k < (column - 5); k++)
                                            //Added by rakesh because caption tabular template showing last column missiong start
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                                ColumnCount++;
                                            }
                                            sbCation.Append("</tr>");

                                            DataView dvRow = new DataView(dt);
                                            DataTable dtRow = dvRow.ToTable();
                                            for (int l = 1; l <= dtRow.Rows.Count - 3; l++)
                                            {
                                                sbCation.Append("<tr>");
                                                for (int i = 1; i < ColumnCount + 1; i++)
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "D")
                                                    {
                                                        DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                            if (dvDrop.ToTable().Rows.Count > 0)
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                            }
                                                            else
                                                            {
                                                                sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                                sbCation.Append("</tr>");
                                            }
                                        }
                                        sbCation.Append("</table>");
                                    }
                                    objStr.Append(sbCation);
                                }

                            }
                        }
                    }
                    else
                    {
                        Hashtable hstInput = new Hashtable();
                        hstInput.Add("@intTemplateId", iRootId);

                        if (common.myInt(Session["Gender"]) == 1)
                        {
                            hstInput.Add("chrGenderType", "F");
                        }
                        else if (common.myInt(Session["Gender"]) == 2)
                        {
                            hstInput.Add("chrGenderType", "M");
                        }
                        else
                        {
                            hstInput.Add("chrGenderType", "M");
                        }

                        hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                        hstInput.Add("@intSecId", Convert.ToInt32(sectionId));
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
                                //Commented by rakesh because caption tabular template showing last column missiong start
                                //for (int k = 1; k < (column - 5); k++)
                                //Commented by rakesh because caption tabular template showing last column missiong start

                                //Added by rakesh because caption tabular template showing last column missiong start
                                for (int k = 1; k < (column - 4); k++)
                                //Added by rakesh because caption tabular template showing last column missiong start
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
                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dt.Rows[l + 1]["RowCaptionName"]) + "</td>");
                                        }
                                        else
                                        {
                                            if (dt.Rows[1]["Col" + i].ToString() == "D")
                                            {
                                                DataView dvDrop = new DataView(dsTabulerTemplate.Tables[1]);
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    dvDrop.RowFilter = "ValueId=" + dt.Rows[l + 1]["Col" + i].ToString();
                                                    if (dvDrop.ToTable().Rows.Count > 0)
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvDrop.ToTable().Rows[0]["ValueName"].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                }
                                            }
                                            else
                                            {
                                                if (dt.Rows[l + 1]["Col" + i].ToString() != "")
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dt.Rows[l + 1]["Col" + i].ToString() + "</td>");
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
                string FieldId = "";
                string sStaticTemplate = "";
                string sEnterBy = "";
                string sVisitDate = "";
                foreach (DataRow item in objDs.Tables[0].Rows)
                {
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
                        //rafat1
                        if (objDt.Rows.Count > 0)
                        {
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);
                            if (Convert.ToBoolean(item["DisplayTitle"]))
                            {
                                // if (EntryType != "M")
                                // {
                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                //}
                                //else
                                //{
                                //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                //}
                                // 28/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                if (objStr.ToString() != "")
                                {
                                    objStr.Append(sEnd + "</li>");
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
                            if (sStaticTemplate != "<br/><br/>")
                            {
                                objStr.Append(common.myStr(item["FieldName"]));
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
                        for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                        {
                            //if (EntryType == "M")
                            //{
                            //    objStr.Append("<br/>" + BeginList + sBegin + common.myStr(item["FieldName"]));
                            //}
                            if (objDt.Rows.Count > 0)
                            {
                                //objStr.Append(": " + objDt.Rows[0]["PropertyValue"].ToString());
                                string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                if (FType == "C")
                                {
                                    FType = "C";
                                }
                                if (FType == "C" || FType == "D" || FType == "B")
                                {
                                    if (FType == "B")
                                    {
                                        if (common.myStr(objDt.Rows[i]["FieldValue"]) == "1"
                                            || common.myStr(objDt.Rows[i]["FieldValue"]) == "0")
                                        {

                                            DataView dv1 = new DataView(objDs.Tables[1]);
                                            if (common.myStr(objDs.Tables[1].Rows[i]["FieldValue"]) == "1")
                                            {
                                                dv1.RowFilter = "TextValue='Yes'";
                                            }
                                            else
                                            {
                                                dv1.RowFilter = "TextValue='No'";
                                            }

                                            DataTable dt1 = dv1.ToTable();
                                            if (dt1.Rows.Count > 0)
                                            {
                                                if (common.myStr(dt1.Rows[i]["MainText"]).Trim() != "")
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                    else
                                                    {
                                                        //if (EntryType == "M")
                                                        //{
                                                        //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        //}
                                                        //else
                                                        objStr.Append(", " + common.myStr(dt1.Rows[i]["MainText"]));
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == 0)
                                                    {
                                                        objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                    else
                                                    {
                                                        //if (EntryType == "M")
                                                        //{
                                                        // objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                        //}
                                                        //else
                                                        objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    //if (EntryType == "M")
                                                    //{
                                                    objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                    //}
                                                    //else
                                                    // objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
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
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            //if (EntryType == "M")
                                            //{
                                            //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            //}
                                            //else
                                            //{
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            // }
                                        }
                                    }
                                    else
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                        else
                                        {
                                            //if (EntryType == "M")
                                            //{
                                            //objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            //}
                                            //else

                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                else if (FType == "L")
                                {
                                    objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (ViewState["iTemplateId"].ToString() != "163")
                                    {
                                        if (FType != "C")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                    else
                                    {
                                        if (FType != "C" && FType != "T")
                                        {
                                            objStr.Append("<br />");
                                        }
                                    }
                                }
                            }
                            sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                            sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                            //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                            //{
                            //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                            //}
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
                {
                    objStr.Append(EndList);
                }
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
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
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

    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null && iFormId != "")
        {
            if (Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"] == null)
            {
                hstInput = new Hashtable();
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", 1);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
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

        int RegId = common.myInt(Session["RegistrationID"]);
        int HospitalId = common.myInt(Session["HospitalLocationID"]);
        int EncounterId = common.myInt(Session["encounterid"]);
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
                            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]),"","", TemplateFieldId,"");

                // sb.Append(sbTemp + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";
            }
            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Vitals"
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

                //sb.Append(sbTemp + "<br/>" + "<br/>");


                drTemplateStyle = null;
                Templinespace = "";

            }

            else if (common.myStr(dtTemplate.Rows[0]["TemplateName"]).Trim() == "Diagnosis"
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
                            "",
                            "", TemplateFieldId, "0","");

                //sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            //sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        hstInput = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }

        hstInput.Add("@intFormId", "1");
        hstInput.Add("@bitDischargeSummary", 0);
        DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEntryType = common.myStr(ds.Tables[0].Rows[0]["EntryType"]);
        }
        hstInput = new Hashtable();
        hstInput.Add("@intTemplateId", TemplateId);

        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }
        else
        {
            hstInput.Add("chrGenderType", "M");
        }
        if (Request.QueryString["ER"] != null && Request.QueryString["ER"].ToString() == "ER")
        {
            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
        }
        else
        {
            if (sEntryType == "S")
            {
                hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
            }
            else
            {
                hstInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            }
        }

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

        //hstInput.Add("@intEREncounterId", Request.QueryString["EREncounterId"] == null ? Session["EREncounterId"].ToString() : Request.QueryString["EREncounterId"].ToString());

        DataSet dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

        string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
        int t = 0, t2 = 0, t3 = 0;
        DataView dv = new DataView(dsAllSectionDetails.Tables[2]);

        //1234
        //if (txtFromDate.SelectedDate.Value != null && txtToDate.SelectedDate.Value != null)
        //{
        //    dv.RowFilter = "EntryDate>='" + Convert.ToDateTime(common.myDate(txtFromDate.SelectedDate.Value)).ToString("yyyy/MM/dd 00:00") +
        //        "' AND EntryDate<='" + Convert.ToDateTime(common.myDate(txtToDate.SelectedDate.Value)).ToString("yyyy/MM/dd 23:59")+"'";
        //}
        dv.Sort = "RecordId DESC";
        DataTable dtEntry = dv.ToTable(true, "RecordId");
        int iRecordId = 0;
        //dtEntry = dv.ToTable(true, "EntryDate");

        //string sEntryDate = "";

        for (int it = 0; it < dtEntry.Rows.Count; it++)
        {
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                DataTable dtFieldValue = new DataTable();
                DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                DataTable dtFieldName = dv1.ToTable();

                if (dsAllSectionDetails.Tables.Count > 2)
                {
                    DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);

                    // dv2.RowFilter = "EntryDate='" + dtEntry.Rows[it]["EntryDate"].ToString() + "'";


                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]);
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
                            str = CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]),
                                item["SectionId"].ToString(), common.myStr(item["EntryType"]), common.myInt(dtEntry.Rows[it]["RecordId"]));
                            str += " ";
                            if (sEntryType == "M" && str.Trim() != "")
                            {
                                str += "<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-size:10px'>(Entered By: " + common.myStr(dtFieldValue.Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + ")</span>";
                            }
                            //}
                            if (iPrevId == common.myInt(item["TemplateId"]))
                            {
                                if (iRecordId != Convert.ToInt16(dtEntry.Rows[it]["RecordId"]))
                                {
                                    if (sEntryType == "M")
                                    {
                                        //objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                        objStrTmp.Append("<br/><br/><b>" + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (t2 == 0)
                                {
                                    if (t3 == 0)//Template
                                    {
                                        t3 = 1;
                                        if (common.myStr(item["SectionsListStyle"]) == "1")
                                        {
                                            BeginList3 = "<ul>"; EndList3 = "</ul>";
                                        }
                                        else if (common.myStr(item["SectionsListStyle"]) == "2")
                                        {
                                            BeginList3 = "<ol>"; EndList3 = "</ol>";
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                        }
                                    }
                                    BeginList3 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                    {
                                        if (str.Trim() != "")
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                        }
                                    }
                                }

                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                else
                                {
                                    if (str.Trim() != "")
                                    {
                                        objStrTmp.Append(str);
                                    }
                                }
                            }
                            else
                            {

                                if (t == 0)
                                {
                                    t = 1;
                                    if (common.myStr(item["TemplateListStyle"]) == "1")
                                    {
                                        BeginList = "<ul>"; EndList = "</ul>";
                                    }
                                    else if (common.myStr(item["TemplateListStyle"]) == "2")
                                    {
                                        BeginList = "<ol>"; EndList = "</ol>";
                                    }
                                }
                                if (common.myStr(item["TemplateBold"]) != ""
                                    || common.myStr(item["TemplateItalic"]) != ""
                                    || common.myStr(item["TemplateUnderline"]) != ""
                                    || common.myStr(item["TemplateFontSize"]) != ""
                                    || common.myStr(item["TemplateForecolor"]) != ""
                                    || common.myStr(item["TemplateListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        if (sBegin.Contains("<br/>") == true)
                                        {
                                            sBegin = sBegin.Remove(0, 5);
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                        else
                                        {
                                            objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                        }
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                    BeginList = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                    {
                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                    }
                                    if (sEntryType == "M" && str.Trim() != "")
                                    {
                                        // objStrTmp.Append("<br/><b> Session " + (it + 1) + ": " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b><br/>");
                                        objStrTmp.Append("<br/><br/><b> " + common.myStr(dtFieldValue.Rows[0]["VisitDateTime"]) + "</b>");
                                    }
                                }
                                if (common.myStr(item["TemplateListStyle"]) == "3"
                                    || common.myStr(item["TemplateListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }
                                objStrTmp.Append(EndList);
                                if (t2 == 0)
                                {
                                    t2 = 1;
                                    if (common.myStr(item["SectionsListStyle"]) == "1")
                                    {
                                        BeginList2 = "<ul>"; EndList3 = "</ul>";
                                    }
                                    else if (common.myStr(item["SectionsListStyle"]) == "2")
                                    {
                                        BeginList2 = "<ol>"; EndList3 = "</ol>";
                                    }
                                }
                                if (common.myStr(item["SectionsBold"]) != ""
                                    || common.myStr(item["SectionsItalic"]) != ""
                                    || common.myStr(item["SectionsUnderline"]) != ""
                                    || common.myStr(item["SectionsFontSize"]) != ""
                                    || common.myStr(item["SectionsForecolor"]) != ""
                                    || common.myStr(item["SectionsListStyle"]) != "")
                                {
                                    sBegin = ""; sEnd = "";
                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                        }
                                    }
                                    BeginList2 = "";
                                }
                                else
                                {
                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                    {
                                        if (str.Trim() != "") //add 19June2010
                                        {
                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                        }
                                    }
                                }
                                if (common.myStr(item["SectionsListStyle"]) == "3"
                                    || common.myStr(item["SectionsListStyle"]) == "0")
                                {
                                    objStrTmp.Append("<br />");
                                }

                                objStrTmp.Append(str);
                            }
                            iRecordId = Convert.ToInt16(dtEntry.Rows[it]["RecordId"]);
                            iPrevId = common.myInt(item["TemplateId"]);
                        }
                    }
                }
            }
        }

        if (t2 == 1 && t3 == 1)
        {
            objStrTmp.Append(EndList3);
        }
        else
        {
            objStrTmp.Append(EndList);
        }

        if (GetPageProperty(iFormId) != null)
        {
            objStrSettings.Append(objStrTmp.ToString());
            sb.Append(objStrSettings.ToString());
        }
        else
        {
            sb.Append(objStrTmp.ToString());
        }
    }


}
