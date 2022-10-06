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
using System.Security.Principal;
using System.Net;

public partial class EMR_Templates_WordProcessor : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private Hashtable hstInput;
    clsExceptionLog objException = new clsExceptionLog();
    private int iPrevId = 0;
    StringBuilder sb = new StringBuilder();
    DAL.DAL dl;
    string Fonts = "";
    int RegId;
    int HospitalId;
    int EncounterId;
    int UserId;
    bool SignStatus = false;

    string Saved_RTF_Content;
    static string gBegin = "<u>";
    static string gEnd = "</u>";

    static DataSet dsStatusId = new DataSet();
    StringBuilder objStrTmp = new StringBuilder();
    private static String FileName = "";
    private static string strimgData = "";
    //private static String SignatureImageFile = "";
    private static String Education = "";
    protected void btnCheck_Onclick(object sender, EventArgs e)
    {
        if (Session["rtfText"] != null)
        {
            String Msg = "<br/>Date & Time: " + common.myStr(System.DateTime.Now) + "<br/>";
            Msg += "Added by: " + common.myStr(Session["EmpName"]) + "<br/>";
            RTF1.Content = RTF1.Content + Msg + common.myStr(Session["rtfText"]);
        }
        // btnSave.Enabled = true;
    }
    protected void btnAddSen_Onclick(object sender, EventArgs e)
    {
        // if (Session["rtfText"] != null)
        RTF1.Content = RTF1.Content + hdSen.Value + "<br />";
        //btnSave.Enabled = true;
    }
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";

        if (common.myStr(Request.QueryString["From"]) == "POPUP")
        {
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";

        }
        else if (common.myStr(Request.QueryString["IsEMRPopUp"]) == "1")
        {
            //Page.MasterPageFile = "/Include/Master/EMRBlankMaster.master";
            this.MasterPageFile = "~/Include/Master/BlankMaster.master";
        }
        else
        {
            this.MasterPageFile = "~/Include/Master/EMRMaster.master";
        }
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Login.aspx?Logout=1", false);
        }

        lblMessage.Text = string.Empty;
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


        if (common.myInt(Request.QueryString["TemplateId"]) > 0)
        {
            btnPrintPDFReport.Visible = true;
        }

        if (!IsPostBack)
        {

            pnlPrint.Visible = false;
            pnlCaseSheet.Visible = true;
            RTF1.RealFontSizes.Clear();
            RTF1.RealFontSizes.Add("9pt");
            RTF1.RealFontSizes.Add("11pt");
            RTF1.RealFontSizes.Add("12pt");
            RTF1.RealFontSizes.Add("14pt");
            RTF1.RealFontSizes.Add("18pt");
            RTF1.RealFontSizes.Add("20pt");
            RTF1.RealFontSizes.Add("24pt");
            RTF1.RealFontSizes.Add("26pt");
            RTF1.RealFontSizes.Add("36pt");
            bindReportList();
            RTF1.EditModes = EditModes.Preview;
            int TemplateId = common.myInt(Request.QueryString["TemplateId"].ToString());
            RTF1.Content = BindEditor(false, TemplateId);


        }
    }




    #region for initial examination

    private string BindEditor(Boolean sign, int TemplateId)
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

            //if (common.myLen(Request.QueryString["TemplateId"]) > 0)
            //{
            //    dvTemplate.RowFilter = "TemplateId=" + common.myInt(Request.QueryString["TemplateId"]);
            //}
            if (common.myLen(TemplateId) > 0)
            {
                dvTemplate.RowFilter = "TemplateId=" + TemplateId;
            }
            else
            {
                dvTemplate.RowFilter = "Sequence=1 OR (TemplateId>0 AND ShowInOrderPage=1)";
            }

            dtTemplate = dvTemplate.ToTable();
            //   sb = null; 
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
                                       "", "", "");
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
                                   common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
                                   "",
                                   "", 0, "");
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
                                            "", 0, sEREncounterId, "");
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
                                    "", 0, sEREncounterId, "");
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
                                    "", "", 0, sEREncounterId, "");
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
                                       "", Session["OPIP"].ToString(), ""
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
                                        "", Session["OPIP"].ToString(), "");
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
                                "", sEREncounterId, "");
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
                                    "", "");
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
                                    "", "");
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
                                    "",
                                    "", "");

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


    //protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    //{
    //    //string sBegin = "", sEnd = "";
    //    ArrayList aEnd = new ArrayList();
    //    if (common.myStr(item[typ + "Forecolor"]) != "" || common.myStr(item[typ + "FontSize"]) != "" || common.myStr(item[typ + "FontStyle"]) != "")
    //    {
    //        sBegin += "<span style='";
    //        if (common.myStr(item[typ + "FontSize"]) != "")
    //        { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
    //        else { sBegin += getDefaultFontSize(); }
    //        if (common.myStr(item[typ + "Forecolor"]) != "")
    //        { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
    //        if (common.myStr(item[typ + "FontStyle"]) != "")
    //        { sBegin += GetFontFamily(typ, item); }
    //    }

    //    if (common.myStr(item[typ + "Bold"]).ToUpper() == "TRUE")
    //    { sBegin += " font-weight: bold;"; }
    //    if (common.myStr(item[typ + "Italic"]).ToUpper() == "TRUE")
    //    { sBegin += " font-style: italic;"; }
    //    if (common.myStr(item[typ + "Underline"]).ToUpper() == "TRUE")
    //    { sBegin += " text-decoration: underline;"; }
    //    aEnd.Add("</span>");
    //    for (int i = aEnd.Count - 1; i >= 0; i--)
    //    {
    //        sEnd += aEnd[i];
    //    }
    //    if (sBegin != "")
    //        sBegin += " '>";
    //}
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
                                    hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                                    hstInput.Add("@chvOPIP", common.myStr(Session["OPIP"]));
                                    hstInput.Add("@intLoginEmployeeId", common.myStr(Session["EmployeeId"]));
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
                        hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
                        hstInput.Add("@chvOPIP", common.myStr(Session["OPIP"]));
                        hstInput.Add("@intLoginEmployeeId", common.myStr(Session["EmployeeId"]));
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
                                    objStr.Append(sEnd + "<br/>");
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
                                if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
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
                                else if (FType == "T" || FType == "M" || FType == "S" || FType == "ST" || FType == "SB" || FType == "W")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
                                    {
                                        if (i == 0)
                                        {
                                            objStr.Append("<span>");
                                            objStr.Append(" " + common.myStr(objDt.Rows[i]["TextValue"]));
                                            objStr.Append("</span>");
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
    protected void bindData(string iFormId, string TemplateId, StringBuilder sb)
    {
        string str = "";
        string sEntryType = "V";
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();

        Hashtable hstInput = new Hashtable();
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
        hstInput.Add("@intEncodedBy", common.myInt(Session["UserId"]));
        hstInput.Add("@chvOPIP", common.myStr(Session["OPIP"]));
        hstInput.Add("@intLoginEmployeeId", common.myStr(Session["EmployeeId"]));
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



    protected DataSet GetPageProperty(string iFormId)
    {
        if (Session["HospitalLocationID"] != null && iFormId != "")
        {
            if (Cache[common.myStr(Session["HospitalLocationID"]) + "_" + iFormId + "_FormPageSettings"] == null)
            {
                Hashtable hstInput = new Hashtable();
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
        // }
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
                            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[0]["PageID"]),
                            "",
                            "", TemplateFieldId, "");

                sb.Append(sbTemp + "<br/>");


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
                                    "", TemplateFieldId, "0", "");

                sb.Append(sbTemp + "<br/>" + "<br/>");


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
                            "", TemplateFieldId, "0", "");

                sb.Append(sbTemp + "<br/>");

                drTemplateStyle = null;
                Templinespace = "";
            }
            sb.Append("</span>");
        }
        return "<br/>" + sbStatic.ToString();
    }

    #endregion











    //void BindPatientHiddenDetails()
    //{
    //    try
    //    {
    //        BaseC.Patient bC = new BaseC.Patient(sConString);
    //        BaseC.clsLISMaster objLISMaster = new BaseC.clsLISMaster(sConString);

    //        if (Session["RegistrationID"] != null)
    //        {
    //            int HospId = common.myInt(Session["HospitalLocationID"]);
    //            int FacilityId = common.myInt(Session["FacilityId"]);
    //            int EncodedBy = common.myInt(Session["UserId"]);
    //            DataSet ds = new DataSet();
    //            if (Session["OPIP"] != null && Session["OPIP"].ToString() == "I")
    //            {
    //                BaseC.RestFulAPI objIPBill = new BaseC.RestFulAPI(sConString);
    //                ds = objIPBill.GetPatientDetailsIP(HospId, FacilityId, common.myInt(Session["RegistrationID"]), "", EncodedBy, 0, "");
    //                lblAdmissionDate.Visible = true;
    //                Label6.Visible = true;
    //            }
    //            else
    //            {
    //                ds = bC.getPatientDetails(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationID"]), "", Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]), common.myInt(Session["UserId"]));
    //                lblIpno.Text = "OP No : ";
    //                lblAdmissionDate.Visible = false;
    //                Label6.Visible = false;
    //            }

    //            if (ds.Tables.Count > 0)
    //            {
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    DataRow dr = ds.Tables[0].Rows[0];
    //                    lblPatientName.Text = common.myStr(dr["PatientName"]) + ", " + common.myStr(dr["GenderAge"]);
    //                    lblDob.Text = common.myStr(dr["DOB"]);
    //                    lblMobile.Text = common.myStr(dr["MobileNo"]);
    //                    lblEncounterNo.Text = common.myStr(dr["EncounterNo"]);
    //                    lblAdmissionDate.Text = common.myStr(dr["EncounterDate"]);
    //                }
    //                else
    //                {
    //                    lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //                    lblMessage.Text = "Patient not found !";
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //                lblMessage.Text = "Patient not found !";
    //                return;
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
    //private void bindDetails()
    //{
    //    try
    //    {
    //       // bool bSign = GetStatus();
    //        if (bSign)
    //        {
    //            BaseC.EMRMasters objbc = new BaseC.EMRMasters(sConString);
    //            DataSet ds = new DataSet();
    //            ds = objbc.GetViewHistory(common.myInt(Session["RegistrationId"]), Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["patientsummary"]);
    //            }
    //        }
    //        else
    //        {
    //            RTF1.Content = BindPatientIllustrationImages();
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
        try
        {
            RadComboBoxItem item = new RadComboBoxItem();
            clsIVF objivf = new clsIVF(sConString);
            DataSet ds = objivf.getEMRTemplateReportSetup(0, 0,
                                common.myInt(Session["EmployeeId"]), "W", 1, common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["UserID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.RowFilter = "DischargeSummary = 'False'";
                foreach (DataRow dr in ((DataTable)dv.ToTable()).Rows)
                {
                    item = new RadComboBoxItem();

                    item.Text = common.myStr(dr["ReportName"]);

                    item.Value = common.myInt(dr["ReportId"]).ToString();
                    item.Attributes.Add("SignatureLabel", common.myStr(dr["SignatureLabel"]));
                    item.Attributes.Add("FieldType", "");
                    item.Attributes.Add("SectionId", "0");

                    // ddlReport.Items.Add(item);
                    item.DataBind();
                }
            }

            ds = objivf.getEMRTemplateWordFieldList(common.myInt(ViewState["PageId"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    item = new RadComboBoxItem();

                    item.Text = common.myStr(dr["SectionName"]);
                    item.Value = common.myInt(dr["FieldId"]).ToString();
                    item.Attributes.Add("SignatureLabel", "");
                    item.Attributes.Add("FieldType", "W");//word-processor
                    item.Attributes.Add("SectionId", common.myInt(dr["SectionId"]).ToString());


                    item.DataBind();
                }
            }

            //ddlReport.Items.Insert(0, new RadComboBoxItem("", "0"));
            //ddlReport.SelectedIndex = 0;

            //ddlReport_OnSelectedIndexChanged(null, null);
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }

    //private StringBuilder getIVFPatient()
    //{
    //    StringBuilder sb = new StringBuilder();
    //    DataSet ds = new DataSet();

    //    clsIVF objivf = new clsIVF(sConString);

    //    ds = objivf.getIVFPatient(common.myInt(Session["RegistrationId"]), 0);

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        DataView DV = ds.Tables[0].Copy().DefaultView;
    //        DV.RowFilter = "RegistrationId=" + common.myInt(Session["RegistrationId"]);

    //        DataTable tbl = DV.ToTable();

    //        if (tbl.Rows.Count > 0)
    //        {
    //            DataRow DR = tbl.Rows[0];

    //            DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
    //            DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(Session["RegistrationId"]);
    //            DataTable tblSpouse = DVSpouse.ToTable();

    //            sb.Append("<div><table border='0' width='100%' style='font-size:smaller; border-collapse:collapse;' cellpadding='2' cellspacing='3' ><tr valign='top'>");
    //            //sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "ivfno")) + "</td><td>: " + common.myStr(Session["IVFNo"]) + "</td>");
    //            sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td>: " + common.myStr(DR["RegistrationNo"]) + "</td>");
    //            sb.Append("</tr>");

    //            sb.Append("<tr valign='top'>");
    //            sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "PatientName")) + "</td><td>: " + common.myStr(DR["PatientName"]) + "</td>");
    //            sb.Append("<td style='width: 109px;'>Age/Gender</td><td>: " + common.myStr(DR["Age/Gender"]) + "</td>");
    //            sb.Append("</tr>");

    //            if (tblSpouse.Rows.Count > 0)
    //            {
    //                sb.Append("<tr valign='top'>");
    //                sb.Append("<td>Spouse</td><td>: " + common.myStr(tblSpouse.Rows[0]["PatientName"]) + "</td>");
    //                sb.Append("<td>Spouse Age/Gender</td><td>: " + common.myStr(tblSpouse.Rows[0]["Age/Gender"]) + "</td>");
    //                sb.Append("</tr>");
    //            }

    //            sb.Append("<tr valign='top'>");
    //            sb.Append("<td>Reg. Date</td><td>: " + common.myStr(DR["RegistrationDate"]) + "</td>");
    //            sb.Append("<td>Occupation</td><td>: " + common.myStr(DR["Occupation"]) + "</td>");
    //            sb.Append("</tr>");

    //            sb.Append("<tr valign='top'>");
    //            sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "email")) + "</td><td>: " + common.myStr(DR["Email"]) + "</td>");
    //            sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "phone")) + "</td><td>: " + common.myStr(DR["PhoneHome"]) + "</td>");
    //            sb.Append("</tr>");

    //            sb.Append("<tr valign='top'>");
    //            sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "Address")) + "</td><td>: " + common.myStr(DR["PatientAddress"]) + "</td>");
    //            sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "mobile")) + "</td><td>: " + common.myStr(DR["MobileNo"]) + "</td>");
    //            sb.Append("</tr>");

    //            sb.Append("</table></div>");
    //        }

    //        sb.Append("<hr />");

    //    }
    //    return sb;
    //}
    //private string PrintReport(bool sign)
    //{
    //    string sEREncounterId = "0";
    //    if (Request.QueryString["EREncounterId"] != null)
    //    {
    //        sEREncounterId = Request.QueryString["EREncounterId"].ToString();
    //    }
    //    StringBuilder sbTemplateStyle = new StringBuilder();
    //    DataSet ds = new DataSet();
    //    DataSet dsTemplate = new DataSet();
    //    DataTable dtTemplate = new DataTable();
    //    DataSet dsTemplateStyle = new DataSet();
    //    DataRow drTemplateStyle = null;
    //    Hashtable hst = new Hashtable();
    //    string Templinespace = "";
    //    //BaseC.DiagnosisDA fun;

    //    RegId = Request.QueryString["RegId"] != null ? common.myInt(Request.QueryString["RegId"]) : common.myInt(Session["RegistrationID"]);
    //    HospitalId = common.myInt(Session["HospitalLocationID"]);
    //    EncounterId = Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]);
    //    UserId = common.myInt(Session["UserID"]);
    //    DL_Funs ff = new DL_Funs();
    //    BindNotes bnotes = new BindNotes(sConString);

    //    //fun = new BaseC.DiagnosisDA(sConString);
    //    string DoctorId = common.myStr(ViewState["DoctorId"]);//fun.GetDoctorId(HospitalId, UserId);
    //    string FormId = "0";
    //    if (Session["formId"] != null)
    //    {
    //        FormId = common.myStr(Session["formId"]);
    //        hst.Add("@intFormId", common.myStr(1));
    //    }
    //    if (Session["HospitalLocationID"] != null)
    //    {
    //        hst.Add("@intEncounterId", EncounterId);
    //        hst.Add("@inyModuleID", 3);
    //        hst.Add("@intGroupId", common.myInt(Session["GroupId"]));
    //    }
    //    string sql = "Select PatientSummary, StatusId from EMRPatientForms where EncounterId = @intEncounterId";

    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    ds = dl.FillDataSet(CommandType.Text, sql, hst);

    //    Saved_RTF_Content = getReportHeader().ToString();
    //    Saved_RTF_Content += getIVFPatient().ToString();

    //    string fieldT = common.myStr(ddlReport.SelectedItem.Attributes["FieldType"]).Trim();

    //    if (fieldT == "W"
    //        && common.myInt(ddlReport.SelectedValue) > 0)
    //    {
    //        clsIVF objivf = new clsIVF(sConString);

    //        DataSet dsW = new DataSet();
    //        dsW = objivf.getEMRTemplateWordData(common.myInt(ddlReport.SelectedValue), Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationID"]));

    //        if (dsW.Tables[0].Rows.Count > 0)
    //        {
    //            Saved_RTF_Content += common.myStr(dsW.Tables[0].Rows[0]["ValueWordProcessor"]);
    //        }

    //        return Saved_RTF_Content;
    //    }
    //    clsIVF note = new clsIVF(sConString);
    //    dsTemplate = note.getEMRTemplateReportSequence(common.myInt(ddlReport.SelectedValue));

    //    hst = new Hashtable();

    //    hst.Add("@inyHospitalLocationID", HospitalId);
    //    sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
    //    + "TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
    //    + "SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
    //    + "FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber "
    //    + "FROM EMRTemplateStatic where HospitalLocationId = @inyHospitalLocationID";

    //    dsTemplateStyle = dl.FillDataSet(CommandType.Text, sql, hst);

    //    dtTemplate = dsTemplate.Tables[0];

    //    if (Saved_RTF_Content == "")
    //    {
    //        sb.Append("<span style='" + Fonts + "'>");
    //    }

    //    if (dtTemplate.Rows.Count > 0)
    //    {
    //        foreach (DataRow dr in dtTemplate.Rows)
    //        {
    //            string strTemplateType = common.myStr(dr["PageIdentification"]);
    //            strTemplateType = strTemplateType.Substring(0, 1);
    //            if (common.myStr(dr["PageName"]).Trim() == "Vitals")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);

    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("VTL", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                            Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                                            common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                            common.myDate(txtToDate.SelectedDate.Value).ToString(), 0, sEREncounterId);
    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("VTL", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim() == "LAB History")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("LAB", Saved_RTF_Content, sbTemp, lck.ToString());

    //                Saved_RTF_Content += "<div id=\"LAB\"></div>";
    //                bnotes.BindLabTestResultReport(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle, Page);
    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("LAB", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim() == "Chief Complaints")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("Chf", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                    Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                                    common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                    common.myDate(txtToDate.SelectedDate.Value).ToString());

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("Chf", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim() == "Diagnosis")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myInt(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("Dia", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId), DoctorId,
    //                                        sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                        common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                        common.myDate(txtToDate.SelectedDate.Value).ToString(), 0, sEREncounterId);

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("Dia", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim() == "Allergies")
    //            {
    //                int lck = 0;

    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
    //                drTemplateStyle = null;// = dv[0].Row;
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("All", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["HospitalLocationId"]).ToString(),
    //                            common.myStr(Session["UserID"]), common.myStr(dr["PageID"]),
    //                            common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                            common.myDate(txtToDate.SelectedDate.Value).ToString(), 0);

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("All", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim().Contains("Prescription"))
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("Med", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
    //                                Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString(), Request.QueryString["OPIP"] == null ? common.myStr(Session["OPIP"]) : common.myStr(Request.QueryString["OPIP"]));

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("Med", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim() == "Current Medication")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("Cur", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
    //                                    Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                                    common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                    common.myDate(txtToDate.SelectedDate.Value).ToString(), Request.QueryString["OPIP"] == null ? common.myStr(Session["OPIP"]) : common.myStr(Request.QueryString["OPIP"]));

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("Cur", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }

    //            else if (common.myStr(dr["PageName"]).Trim() == "Orders And Procedures")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("Ord", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
    //                                Convert.ToInt16(UserId), DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString(), sEREncounterId);

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("Ord", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["PageName"]).Trim() == "Immunization Chart")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]) + sEnd);
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["PageName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("Imm", Saved_RTF_Content, sbTemp, lck.ToString());
    //                bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                            common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle,
    //                            Page, common.myStr(dr["PageId"]), common.myStr(Session["UserID"]),
    //                            common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                            common.myDate(txtToDate.SelectedDate.Value).ToString());

    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("Imm", ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                dr["PageName"] = null;
    //                Templinespace = "";
    //            }

    //            else if ((common.myStr(dr["PageName"]).Trim() == "ROS"
    //                || common.myStr(dr["TemplateType"]).Trim() == "ROS"))
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                    sbTemplateStyle.Append(gBegin + common.myStr(dr["DisplayName"]).Trim() + gEnd);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("D" + common.myStr(dr["PageId"]), Saved_RTF_Content, sbTemp, lck.ToString());
    //                BindProblemsROS(HospitalId, EncounterId, sbTemp, common.myStr(dr["DisplayName"]).Trim(), common.myStr(dr["PageName"]).Trim(), common.myStr(dr["PageId"]));
    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("D" + common.myStr(dr["PageId"]), ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }

    //                Templinespace = "";
    //            }
    //            else if (common.myStr(dr["SectionName"]).Trim() != "")
    //            {
    //                int lck = 0;
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dr["TemplateId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                    if (common.myBool(drTemplateStyle["TemplateDisplayTitle"]))
    //                    {
    //                        if (dr["DisplayName"] != null)
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["DisplayName"]).Trim() + sEnd);
    //                        }
    //                        else
    //                        {
    //                            sbTemplateStyle.Append(sBegin + common.myStr(dr["PageName"]).Trim() + sEnd);
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    Templinespace = common.myStr(dr["TemplateSpaceNumber"]);
    //                }
    //                lck = common.myInt(dr["Lock"]);
    //                StringBuilder sbTemp = new StringBuilder();
    //                AddStr1("D" + common.myStr(dr["TemplateId"]), Saved_RTF_Content, sbTemp, lck.ToString());
    //                bindTemplateReportData(FormId, common.myStr(dr["TemplateId"]), common.myStr(dr["SectionId"]), sbTemp);
    //                if (common.myStr(sbTemp).Length > 46)
    //                {
    //                    AddStr2("D" + common.myStr(dr["PageId"]), ref Saved_RTF_Content, sbTemp, lck.ToString(), Templinespace, common.myStr(dr["ShowNote"]));
    //                }
    //                Templinespace = "";
    //            }

    //        }
    //        sb.Append("</span>");
    //        StringBuilder temp = new StringBuilder();
    //        bnotes.GetEncounterFollowUpAppointment(Session["HospitalLocationId"].ToString(), Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
    //        if (temp.Length > 20)
    //        {
    //            sb.Append(temp);
    //            sb.Append("</span>");
    //            Saved_RTF_Content += temp.ToString();
    //        }


    //        if (sign == true)
    //        {
    //            if (Saved_RTF_Content != null)
    //            {
    //                if (Saved_RTF_Content.Contains("dvDoctorImage") != true)
    //                {
    //                    if (Saved_RTF_Content != "")
    //                    {
    //                        Saved_RTF_Content += hdnDoctorImage.Value;
    //                    }
    //                    else
    //                    {
    //                        sb.Append(hdnDoctorImage.Value);
    //                    }
    //                }
    //            }
    //        }
    //        else if (sign == false)
    //        {
    //            if (Saved_RTF_Content != null)
    //            {
    //                if (Saved_RTF_Content.Contains("dvDoctorImage") == true)
    //                {
    //                    Saved_RTF_Content = Saved_RTF_Content.Replace('"', '$');
    //                    string st = "<div id=$dvDoctorImage$>";
    //                    int start = Saved_RTF_Content.IndexOf(@st);
    //                    //int end = Saved_RTF_Content.IndexOf("</div>", start);
    //                    //if (start != -1)
    //                    //{
    //                    //    Saved_RTF_Content = Saved_RTF_Content.Remove(start);
    //                    //    Saved_RTF_Content = Saved_RTF_Content + "</div>";
    //                    //}
    //                    if (start > 0)
    //                    {
    //                        int End = Saved_RTF_Content.IndexOf("</div>", start);
    //                        StringBuilder sbte = new StringBuilder();
    //                        sbte.Append(Saved_RTF_Content.Substring(start, (End + 6) - start));
    //                        StringBuilder ne = new StringBuilder();
    //                        ne.Append(Saved_RTF_Content.Replace(sbte.ToString(), ""));
    //                        Saved_RTF_Content = ne.Replace('$', '"').ToString();
    //                    }
    //                }

    //            }
    //        }

    //        if (Saved_RTF_Content == "" || Saved_RTF_Content == null)
    //        {
    //            return sb.ToString();
    //        }
    //        else
    //        {
    //            return Saved_RTF_Content;
    //        }
    //        //    }                

    //    }
    //    //return "";

    //    return "";
    //}
    //protected void bindTemplateReportData(string iFormId, string TemplateId, string SectionId, StringBuilder sb)
    //{
    //    string str = "";
    //    StringBuilder objStrTmp = new StringBuilder();
    //    StringBuilder objStrSettings = new StringBuilder();

    //    hstInput = new Hashtable();
    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    hstInput.Add("@inyHospitalLocationID", common.myInt(Session["HospitalLocationID"]));
    //    hstInput.Add("@intTemplateId", TemplateId);
    //    if (common.myInt(Session["Gender"]) == 1)
    //    {
    //        hstInput.Add("chrGenderType", "F");
    //    }
    //    else if (common.myInt(Session["Gender"]) == 2)
    //    {
    //        hstInput.Add("chrGenderType", "M");
    //    }

    //    hstInput.Add("@intFormId", 1);
    //    if (common.myInt(ddlReport.SelectedValue) != 0)
    //    {
    //        hstInput.Add("@intReportId", common.myInt(ddlReport.SelectedValue));
    //    }

    //    DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);

    //    DataView dvrd = ds.Tables[0].Copy().DefaultView;
    //    dvrd.RowFilter = "SectionId IN (" + SectionId + ")";

    //    dvrd.Sort = "Hierarchy,SequenceNo";

    //    ds.Tables.RemoveAt(0);
    //    ds.Tables.Add(dvrd.ToTable());


    //    DataSet dsAllSectionDetails = new DataSet();
    //    if (common.myInt(ddlReport.SelectedValue) != 0
    //        && SectionId != ""
    //        && ds.Tables[0].Rows.Count > 0)
    //    {

    //        foreach (DataRow dr in ds.Tables[0].Rows)
    //        {
    //            DataSet dsGetData = new DataSet();
    //            hstInput = new Hashtable();
    //            hstInput.Add("@intTemplateId", dr["TemplateId"]);
    //            hstInput.Add("@intSectionID", common.myInt(SectionId));
    //            hstInput.Add("@intEncounterId", Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
    //            hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
    //            if (common.myInt(Session["Gender"]) == 1)
    //            {
    //                hstInput.Add("chrGenderType", "F");
    //            }
    //            else if (common.myInt(Session["Gender"]) == 2)
    //            {
    //                hstInput.Add("chrGenderType", "M");
    //            }

    //            hstInput.Add("@intRecordId", common.myInt(ddlReport.SelectedValue));
    //            dsGetData = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);

    //            if (dsGetData.Tables.Count > 0)
    //            {
    //                if (dsGetData.Tables[0].Rows.Count > 0)
    //                {
    //                    dsAllSectionDetails.Merge(dsGetData);
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        hstInput = new Hashtable();
    //        hstInput.Add("@intTemplateId", TemplateId);
    //        hstInput.Add("@intEncounterId", Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
    //        hstInput.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
    //        if (common.myInt(Session["Gender"]) == 1)
    //        {
    //            hstInput.Add("chrGenderType", "F");
    //        }
    //        else if (common.myInt(Session["Gender"]) == 2)
    //        {
    //            hstInput.Add("chrGenderType", "M");
    //        }

    //        hstInput.Add("@intRecordId", common.myInt(ddlReport.SelectedValue));
    //        dsAllSectionDetails = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hstInput);
    //    }
    //    string BeginList = "", EndList = "", BeginList2 = "", BeginList3 = "", EndList3 = "";
    //    int t = 0, t2 = 0, t3 = 0;
    //    foreach (DataRow item in ds.Tables[0].Rows)
    //    {
    //        DataTable dtFieldValue = new DataTable();
    //        DataView dv1 = new DataView(dsAllSectionDetails.Tables[0]);
    //        //dv1.Sort = " FieldId  ";
    //        dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
    //        DataTable dtFieldName = dv1.ToTable();
    //        if (dsAllSectionDetails.Tables.Count > 2)
    //        {
    //            DataView dv2 = new DataView(dsAllSectionDetails.Tables[2]);
    //            dtFieldValue = dv2.ToTable();
    //        }

    //        DataSet dsAllFieldsDetails = new DataSet();
    //        dsAllFieldsDetails.Tables.Add(dtFieldName);
    //        dsAllFieldsDetails.Tables.Add(dtFieldValue);

    //        if (dsAllSectionDetails.Tables[0].Rows.Count > 0)
    //        {
    //            if (dsAllSectionDetails.Tables.Count > 2)
    //            {

    //                if (dsAllSectionDetails.Tables[2].Rows.Count > 0) // add for education details
    //                {
    //                    string sBegin = "", sEnd = "";

    //                    DataRow dr3;
    //                    dr3 = dsAllSectionDetails.Tables[0].Rows[0];

    //                    getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);

    //                    ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

    //                    str = CreateReportString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]), common.myStr(item["Tabular"]), common.myInt(item["NoOfBlankRows"]));
    //                    str += " ";

    //                    if (iPrevId == common.myInt(item["TemplateId"]))
    //                    {
    //                        if (t2 == 0)
    //                        {
    //                            if (t3 == 0)//Template
    //                            {
    //                                t3 = 1;
    //                                if (common.myStr(item["SectionsListStyle"]) == "1")
    //                                {
    //                                    BeginList3 = "<ul>"; EndList3 = "</ul>";
    //                                }
    //                                else if (common.myStr(item["SectionsListStyle"]) == "2")
    //                                {
    //                                    BeginList3 = "<ol>"; EndList3 = "</ol>";
    //                                }
    //                            }
    //                        }

    //                        if (common.myStr(item["SectionsBold"]) != ""
    //                            || common.myStr(item["SectionsItalic"]) != ""
    //                            || common.myStr(item["SectionsUnderline"]) != ""
    //                            || common.myStr(item["SectionsFontSize"]) != ""
    //                            || common.myStr(item["SectionsForecolor"]) != ""
    //                            || common.myStr(item["SectionsListStyle"]) != "")
    //                        {
    //                            sBegin = ""; sEnd = "";
    //                            MakeFont("Sections", ref sBegin, ref sEnd, item);
    //                            if (common.myBool(item["SectionDisplayTitle"]))   //19June2010
    //                            {
    //                                if (str.Trim() != "")
    //                                {
    //                                    objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                    //objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
    //                                }
    //                            }
    //                            BeginList3 = "";
    //                        }
    //                        else
    //                        {
    //                            if (common.myBool(item["SectionDisplayTitle"]))    //19June
    //                            {
    //                                if (str.Trim() != "")
    //                                {
    //                                    objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
    //                                }
    //                            }
    //                        }

    //                        if (common.myStr(item["SectionsListStyle"]) == "3"
    //                            || common.myStr(item["TemplateListStyle"]) == "0")
    //                        {
    //                            objStrTmp.Append("<br />");
    //                        }
    //                        else
    //                        {
    //                            if (str.Trim() != "")
    //                            {
    //                                objStrTmp.Append(str);
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (t == 0)
    //                        {
    //                            t = 1;
    //                            if (common.myStr(item["TemplateListStyle"]) == "1")
    //                            {
    //                                BeginList = "<ul>";
    //                                EndList = "</ul>";
    //                            }
    //                            else if (common.myStr(item["TemplateListStyle"]) == "2")
    //                            {
    //                                BeginList = "<ol>";
    //                                EndList = "</ol>";
    //                            }
    //                        }
    //                        if (common.myStr(item["TemplateBold"]) != ""
    //                            || common.myStr(item["TemplateItalic"]) != ""
    //                            || common.myStr(item["TemplateUnderline"]) != ""
    //                            || common.myStr(item["TemplateFontSize"]) != ""
    //                            || common.myStr(item["TemplateForecolor"]) != ""
    //                            || common.myStr(item["TemplateListStyle"]) != "")
    //                        {
    //                            sBegin = ""; sEnd = "";
    //                            MakeFont("Template", ref sBegin, ref sEnd, item);
    //                            if (common.myBool(item["TemplateDisplayTitle"]))
    //                            {
    //                                if (sBegin.Contains("<br/>") == true)
    //                                {
    //                                    sBegin = sBegin.Remove(0, 5);
    //                                    //objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
    //                                }
    //                                else
    //                                {
    //                                    //objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd);
    //                                }
    //                            }
    //                            BeginList = "";
    //                        }
    //                        else
    //                        {
    //                            if (common.myBool(item["TemplateDisplayTitle"]))
    //                            {
    //                                objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
    //                            }
    //                        }
    //                        if (common.myStr(item["TemplateListStyle"]) == "3"
    //                            || common.myStr(item["TemplateListStyle"]) == "0")
    //                        {
    //                            objStrTmp.Append("<br />");
    //                        }
    //                        objStrTmp.Append(EndList);
    //                        if (t2 == 0)
    //                        {
    //                            t2 = 1;
    //                            if (common.myStr(item["SectionsListStyle"]) == "1")
    //                            {
    //                                BeginList2 = "<ul>"; EndList3 = "</ul>";
    //                            }
    //                            else if (common.myStr(item["SectionsListStyle"]) == "2")
    //                            {
    //                                BeginList2 = "<ol>"; EndList3 = "</ol>";
    //                            }
    //                        }
    //                        if (common.myStr(item["SectionsBold"]) != ""
    //                            || common.myStr(item["SectionsItalic"]) != ""
    //                            || common.myStr(item["SectionsUnderline"]) != ""
    //                            || common.myStr(item["SectionsFontSize"]) != ""
    //                            || common.myStr(item["SectionsForecolor"]) != ""
    //                            || common.myStr(item["SectionsListStyle"]) != "")
    //                        {
    //                            sBegin = ""; sEnd = "";
    //                            MakeFont("Sections", ref sBegin, ref sEnd, item);
    //                            if (common.myBool(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
    //                            {
    //                                if (str.Trim() != "") //add 19June2010
    //                                {
    //                                    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
    //                                }
    //                            }
    //                            BeginList2 = "";
    //                        }
    //                        else
    //                        {
    //                            if (common.myBool(item["SectionDisplayTitle"]))// Comment ON 19June2010
    //                            {
    //                                objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
    //                            }
    //                        }
    //                        if (common.myStr(item["SectionsListStyle"]) == "3"
    //                            || common.myStr(item["SectionsListStyle"]) == "0")
    //                        {
    //                            objStrTmp.Append("<br />");
    //                        }

    //                        objStrTmp.Append(str);
    //                    }

    //                    iPrevId = common.myInt(item["TemplateId"]);
    //                }
    //            }
    //        }
    //    }

    //    if (t2 == 1 && t3 == 1)
    //    {
    //        objStrTmp.Append(EndList3);
    //    }
    //    else
    //    {
    //        objStrTmp.Append(EndList);
    //    }

    //    if (GetPageProperty(iFormId) != null)
    //    {
    //        objStrSettings.Append(objStrTmp.ToString());
    //        sb.Append(objStrSettings.ToString());
    //    }
    //    else
    //    {
    //        sb.Append(objStrTmp.ToString());
    //    }
    //}

    protected string CreateReportString(DataSet objDs, int iRootId, string iRootName, string TabularType, int NoOfBlankRows)
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

                            string strHeader = common.myStr(objDs.Tables[0].Rows[i]["FieldName"]);

                            objStr.Append("<th align='center' style=' " + sFontSize + " ' >" + strHeader + "</th>");
                            dr2 = objDs.Tables[0].Rows[i];

                            dvValues.RowFilter = "";
                            dvValues = new DataView(objDs.Tables[1]);
                            dvValues.RowFilter = "FieldId='" + common.myStr(dr2["FieldId"]) + "'";
                            dsMain.Tables.Add(dvValues.ToTable("table" + i.ToString()));

                            if (dvValues.ToTable().Rows.Count > MaxLength)
                            {
                                MaxLength = dvValues.ToTable().Rows.Count;
                            }
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
                        {
                            BeginList = "<ul>"; EndList = "</ul>";
                        }
                        else if (common.myStr(item["FieldsListStyle"]) == "2")
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
                            sEnd = "";
                            MakeFont("Fields", ref sBegin, ref sEnd, item);

                            if (sBegin.StartsWith("<br/>"))
                            {
                                if (sBegin.Length > 5)
                                {
                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                }
                            }

                            if (common.myBool(item["DisplayTitle"]))
                            {
                                objStr.Append("<td>");

                                objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
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

                                objStr.Append("</td>");
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
                                objStr.Append(common.myStr(item["FieldName"]));
                                objStr.Append("</td>");
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

                        if (dtFieldType.Rows.Count > 0
                            && objDt.Rows.Count == 0)
                        {
                            string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                            if (FType == "O")
                            {
                                int DataObjectId = common.myInt(dtFieldType.Rows[0]["DataObjectId"]);

                                clsIVF objivf = new clsIVF(sConString);

                                string strOutput = objivf.getDataObjectValue(DataObjectId);

                                if (common.myLen(strOutput) > 0)
                                {
                                    objStr.Append("<td>" + common.myStr(dtFieldType.Rows[0]["FieldName"]) + "</td>");
                                    objStr.Append("<td>" + strOutput + "</td>");
                                }
                            }
                        }

                        if (objDt.Rows.Count > 0)
                        {
                            objStr.Append("<td>");

                            for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
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
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
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
                                            objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                        }
                                    }
                                }
                                if (common.myStr(item["FieldsListStyle"]) == "")
                                {
                                    if (common.myStr(ViewState["iTemplateId"]) != "163")
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
                {
                    objStr.Append(EndList);
                }

                objStr.Append("</table>");
            }
        }

        return objStr.ToString();
    }


    //protected void ddlReport_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //btnPrintReport.Visible = false;
    //        hdnReportContent.Value = "";

    //        if (common.myInt(ddlReport.SelectedValue) > 0)
    //        {
    //            hdnReportContent.Value = PrintReport(true);

    //            //comment as follow-up appointment is check inside the printreport function --Saten
    //            StringBuilder sbD = new StringBuilder();
    //            sbD.Append(" <table border='0' width='100%' style='border-collapse:collapse;' cellpadding='5' cellspacing='5' >");
    //            //sbD.Append("<tr><td>Follow Up : </td></tr>");

    //            string SignatureLabel = common.myStr(ddlReport.SelectedItem.Attributes["SignatureLabel"]).Trim();

    //            if (SignatureLabel == "")
    //            {
    //                sbD.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
    //            }
    //            else
    //            {
    //                sbD.Append("<tr><td align='right'><b>" + SignatureLabel + "</b></td></tr>");
    //            }


    //            sbD.Append("<tr><td align='right'> </td></tr>");
    //            sbD.Append("</table>");

    //            hdnReportContent.Value = hdnReportContent.Value + sbD.ToString();
    //        }
    //        else
    //        {
    //            //btnPrintReport.Visible = false;
    //            return;
    //        }
    //        //btnPrintReport.Visible = true;
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}

    private void bindTemplateControl()
    {
        try
        {
            clsIVF objIVF = new clsIVF(sConString);
            DataSet ds;

            ds = objIVF.getEMRSpecialisationTemplate(common.myInt(Session["UserId"]));

            ddlSpecilityTemplate.DataSource = ds.Tables[0];
            ddlSpecilityTemplate.DataTextField = "TemplateName";
            ddlSpecilityTemplate.DataValueField = "TemplateId";
            ddlSpecilityTemplate.DataBind();

            ddlSpecilityTemplate.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlSpecilityTemplate.SelectedIndex = 0;

            ds = objIVF.getEMRGeneralTemplate();

            ddlGeneralTemplate.DataSource = ds.Tables[0];
            ddlGeneralTemplate.DataTextField = "TemplateName";
            ddlGeneralTemplate.DataValueField = "TemplateId";
            ddlGeneralTemplate.DataBind();

            ddlGeneralTemplate.Items.Insert(0, new RadComboBoxItem("", "0"));
            ddlGeneralTemplate.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Error: " + Ex.Message;

            objException.HandleException(Ex);
        }
    }
    protected void btnclose_OnClick(object sender, EventArgs e)
    {
    }
    protected void btnDictate_OnClick(object sender, EventArgs e)
    {
        RadWindow3.NavigateUrl = "speech.aspx";
        RadWindow3.Height = 600;
        RadWindow3.Width = 800;
        RadWindow3.Top = 20;
        RadWindow3.Left = 20;
        // RadWindowForNew.Title = "Time Slot";
        RadWindow3.OnClientClose = "OnClientClose";
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        RadWindow3.VisibleStatusbar = false;
        RadWindow3.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow3.InitialBehaviors = WindowBehaviors.Maximize;
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
                RTF1.RealFontSizes.Add(common.myStr(dr["FontSize"]));
            }
        }
    }
    private void getDoctorImage()
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        int intCheckImage = 0; // Check image from signed note signature
        Stream strm;
        Object img;
        DateTime SignatureDate;
        String UserName = "", ShowSignatureDate = "", UserDoctorId = "";
        String SignImage = "", SignNote = "";
        String DivStartTag = "<div id='dvDoctorImage'>";
        String SignedDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        try
        {
            if (Session["DoctorId"] != null)
            {

                ds = lis.getDoctorImageDetails(common.myInt(Session["DoctorId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                     Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0] as DataRow;
                    if (common.myStr(dr["SignatureImage"]) != "")
                    {
                        SignedDate = common.myStr(dr["SignedDate"]);
                        FileName = common.myStr(dr["SignatureImageName"]);
                        ShowSignatureDate = " on " + SignedDate;
                        Education = common.myStr(dr["SignedProviderEducation"]);
                        img = dr["SignatureImage"];
                        UserName = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);
                        Session["EmpName"] = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);

                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");
                        strm = new MemoryStream((byte[])img);
                        byte[] buffer = new byte[strm.Length];
                        int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                        FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                        fs.Write(buffer, 0, byteSeq);
                        fs.Dispose();
                        RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName.Trim() + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                        SignImage = "<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                        strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                        intCheckImage = 1;
                        strimgData = common.myStr(dr["ImageId"]);
                        SignNote = "Electronically signed by " + UserName.Trim() + " " + Education.Trim() + " " + ShowSignatureDate.Trim() + "</div>";
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (intCheckImage == 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0] as DataRow;
                        img = dr["SignatureImage"];
                        FileName = common.myStr(dr["ImageType"]);
                        UserName = common.myStr(dr["EmployeeName"]);
                        Session["EmpName"] = common.myStr(dr["EmployeeName"]);
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(dr["DoctorId"]);
                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");

                        if (common.myStr(dr["Education"]).Trim() != ""
                            && common.myStr(dr["Education"]).Trim() != "&nbsp;")
                        {
                            Education = common.myStr(dr["Education"]);
                        }
                        SignNote = "Electronically signed by " + UserName + " " + Education + " " + ShowSignatureDate + "</div>";

                        if (FileName != "")
                        {
                            RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                            strimgData = common.myStr(dr["ImageId"]);
                        }
                        else if (common.myStr(dr["SignatureImage"]) != "")
                        {
                            strm = new MemoryStream((byte[])img);
                            byte[] buffer = new byte[strm.Length];
                            int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                            FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                            fs.Write(buffer, 0, byteSeq);
                            fs.Dispose();
                            RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                            strimgData = common.myStr(dr["ImageId"]);
                        }
                    }
                }
                if (File.Exists(strSingImagePath))
                {
                    hdnDoctorImage.Value = DivStartTag + "<table  border='0' cellpadding='0' cellspacing='0' style='font-size:10pt; font-family:Tahoma;'><tbody><tr><td align='right'>" + SignImage + "</td></tr></tbody></table>" + SignNote + "<br />";
                }
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
            lis = null;
            ds.Dispose();
            strm = null;
            img = null;
            UserName = null;
            ShowSignatureDate = null;
            UserDoctorId = null;
            SignImage = null;
            SignNote = null;
            DivStartTag = null;
            SignedDate = null;
            strSQL = null;
            strSingImagePath = null;
        }
    }
    //private string BindEditor(Boolean sign)
    //{
    //    if (Request.QueryString["ifId"] != "")
    //    {
    //        string sEREncounterId = "0";
    //        if (Request.QueryString["EREncounterId"] != null)
    //        {
    //            sEREncounterId = Request.QueryString["EREncounterId"].ToString();
    //        }
    //        StringBuilder sbTemplateStyle = new StringBuilder();
    //        DataSet ds = new DataSet();
    //        DataSet dsTemplate = new DataSet();
    //        DataSet dsTemplateStyle = new DataSet();
    //        DataRow drTemplateStyle = null;
    //        DataTable dtTemplate = new DataTable();
    //        Hashtable hst = new Hashtable();
    //        string Templinespace = "";
    //        BaseC.DiagnosisDA fun;

    //        RegId = Request.QueryString["RegId"] != null ? common.myInt(Request.QueryString["RegId"]) : common.myInt(Session["RegistrationID"]);
    //        HospitalId = common.myInt(Session["HospitalLocationID"]);
    //        EncounterId = Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]);
    //        UserId = common.myInt(Session["UserID"]);

    //        BindNotes bnotes = new BindNotes(sConString);
    //        fun = new BaseC.DiagnosisDA(sConString);

    //        string DoctorId = fun.GetDoctorId(HospitalId, Convert.ToInt16(UserId));
    //        //ds = bnotes.GetPatientEMRData(common.myInt(Session["encounterid"]));
    //        dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));


    //        dsTemplate = bnotes.GetEMRTemplates(EncounterId, RegId, sEREncounterId);
    //        dtTemplate = dsTemplate.Tables[0];

    //        sb.Append("<span style='" + Fonts + "'>");

    //        for (int i = 0; i < dtTemplate.Rows.Count; i++)
    //        {
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Complaints"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }

    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10003))
    //                {
    //                    bnotes.BindProblemsHPI(RegId, HospitalId, EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                   Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                   common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                   common.myDate(txtToDate.SelectedDate.Value).ToString());
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>" + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //               && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "HIS")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }

    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                {
    //                    bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Allergies"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                drTemplateStyle = null;// = dv[0].Row;
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10002))
    //                {
    //                    //bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
    //                    //            common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
    //                    //            common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                    //            common.myDate(txtToDate.SelectedDate.Value).ToString(),0);
    //                    bnotes.BindAllergies(RegId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Session["HospitalLocationId"]),
    //                               common.myStr(Session["UserID"]), common.myStr(dtTemplate.Rows[i]["PageID"]),
    //                               common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                               common.myDate(txtToDate.SelectedDate.Value).ToString(), 0);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Vitals"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10001))
    //                {
    //                    bnotes.BindVitals(HospitalId.ToString(), EncounterId, sbTemp, sbTemplateStyle, drTemplateStyle,
    //                                        Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                        common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                        common.myDate(txtToDate.SelectedDate.Value).ToString(), 0, sEREncounterId);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>" + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";

    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
    //                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }

    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                {
    //                    bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Lab Test Result"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                // bnotes.BindLabTestResult(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId, UserId, DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myInt(Session["FacilityID"]), common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]));
    //                sb.Append(sbTemp);
    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Diagnosis"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10004))
    //                {
    //                    bnotes.BindAssessments(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
    //                        Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString(), 0, sEREncounterId);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Provisional Diagnosis"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10004))
    //                {
    //                    bnotes.BindPatientProvisionalDiagnosis(RegId, HospitalId, EncounterId, Convert.ToInt16(UserId),
    //                                Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString("yyy/MM/dd"),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString("yyy/MM/dd"), 0, sEREncounterId);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Prescription"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);

    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }

    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10005))
    //                {
    //                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "P", drTemplateStyle,
    //                                   Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                   common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                   common.myDate(txtToDate.SelectedDate.Value).ToString(), Request.QueryString["OPIP"] == null ? Session["OPIP"].ToString() : common.myStr(Request.QueryString["OPIP"]));

    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Current Medication"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(common.myInt(drTemplateStyle["TemplateSpaceNumber"]) - 1);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10005))
    //                {
    //                    bnotes.BindMedication(EncounterId, HospitalId, RegId, sbTemp, sbTemplateStyle, "C", drTemplateStyle,
    //                                    Page, common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                    common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                    common.myDate(txtToDate.SelectedDate.Value).ToString(), Request.QueryString["OPIP"] == null ? common.myStr(Session["OPIP"]) : common.myStr(Request.QueryString["OPIP"]));
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Order"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10006))
    //                {
    //                    bnotes.BindOrders(common.myInt(Session["RegistrationId"]), HospitalId, EncounterId,
    //                            Convert.ToInt16(UserId), Request.QueryString["DoctorId"] != null ? Request.QueryString["DoctorId"].ToString() : DoctorId, sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                            common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                            common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                            common.myDate(txtToDate.SelectedDate.Value).ToString(), sEREncounterId);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                drTemplateStyle = null;
    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Immunization"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10007))
    //                {
    //                    bnotes.BindImmunization(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString());
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Doctor Progress Note"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }
    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10009))
    //                {
    //                    bnotes.BindDoctorProgressNote(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                Request.QueryString["DoctorId"] != null ? common.myInt(Request.QueryString["DoctorId"]) : common.myInt(Session["DoctorId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString());
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                Templinespace = "";
    //            }
    //            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE" && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() == "POC")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }

    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                    || (common.myInt(ddlTemplatePatient.SelectedValue) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
    //                {
    //                    bindData("1", common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp);
    //                    if (sbTemp.ToString() != "")
    //                        sb.Append(sbTemp + "<br/>");
    //                }

    //                Templinespace = "";
    //            }

    //            if (common.myStr(dtTemplate.Rows[i]["TemplateName"]).Trim() == "Daily Injections"
    //                && common.myStr(dtTemplate.Rows[i]["DataStatus"]).Trim() == "AVAILABLE")
    //            {
    //                string strTemplateType = common.myStr(dtTemplate.Rows[i]["PageIdentification"]);
    //                strTemplateType = strTemplateType.Substring(0, 1);
    //                sbTemplateStyle = new StringBuilder();
    //                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
    //                dv.RowFilter = "PageId =" + common.myStr(dtTemplate.Rows[i]["PageId"]);
    //                if (dv.Count > 0)
    //                {
    //                    drTemplateStyle = dv[0].Row;
    //                    string sBegin = "", sEnd = "";
    //                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
    //                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
    //                }

    //                StringBuilder sbTemp = new StringBuilder();

    //                if ((common.myInt(ddlTemplatePatient.SelectedValue) == 0)
    //                     || (common.myInt(ddlTemplatePatient.SelectedValue) == 10008))
    //                {
    //                    bnotes.BindInjection(HospitalId.ToString(), common.myInt(Session["RegistrationId"]),
    //                                common.myInt(Session["EncounterId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page,
    //                                common.myStr(dtTemplate.Rows[i]["PageId"]), common.myStr(Session["UserID"]),
    //                                common.myDate(txtFromDate.SelectedDate.Value).ToString(),
    //                                common.myDate(txtToDate.SelectedDate.Value).ToString());

    //                    sb.Append(sbTemp + "<br/>");
    //                }

    //                Templinespace = "";
    //            }
    //            sb.Append("</span>");
    //        }
    //        drTemplateStyle = dsTemplateStyle.Tables[0].Rows[0];
    //        StringBuilder temp = new StringBuilder();
    //        bnotes.GetEncounterFollowUpAppointment(Session["HospitalLocationId"].ToString(), Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]), temp, sbTemplateStyle, drTemplateStyle, Page);
    //        sb.Append(temp);

    //        if (sign == true)
    //        {
    //            sb.Append(hdnDoctorImage.Value);
    //        }
    //        else if (sign == false)
    //        {
    //            if (RTF1.Content != null)
    //            {
    //                if (RTF1.Content.Contains("dvDoctorImage") == true)
    //                {
    //                    string signData = RTF1.Content.Replace('"', '$');
    //                    string st = "<div id=$dvDoctorImage$>";
    //                    int start = signData.IndexOf(@st);
    //                    if (start > 0)
    //                    {
    //                        int End = signData.IndexOf("</div>", start);
    //                        StringBuilder sbte = new StringBuilder();
    //                        sbte.Append(signData.Substring(start, (End + 6) - start));
    //                        StringBuilder ne = new StringBuilder();
    //                        ne.Append(signData.Replace(sbte.ToString(), ""));
    //                        sb.Append(ne.Replace('$', '"').ToString());
    //                    }
    //                }

    //            }
    //        }

    //    }

    //    return sb.ToString();
    //}


    //bool GetStatus()
    //{
    //    int StatusId = 0;
    //    hstInput = new Hashtable();



    //    hstInput.Add("@intencounterid", Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
    //    hstInput.Add("@intFormId", common.myInt(Session["formId"]));
    //    string sql = "Select StatusId, FormName from EMRPatientForms epf inner join EMRForms ef on  epf.FormId = ef.Id"
    //        + " where EncounterId = @intencounterid"
    //        + " And FormId = @intFormId AND epf.Active=1"
    //        + " Select StatusId, Status, Code From GetStatus(1,'Notes') where Code='P'"
    //        + " Select StatusId, Status, Code From GetStatus(1,'Notes') where Code='S'"
    //        + " select s.code from encounter e inner join statusmaster s on e.statusid = s.statusid where e.id = @intencounterid and e.active = 1 ";

    //    dsStatusId = dl.FillDataSet(CommandType.Text, sql, hstInput);
    //    if (dsStatusId.Tables[1].Rows.Count > 0)
    //    {
    //        hdnUnSignedId.Value = common.myStr(dsStatusId.Tables[1].Rows[0]["StatusId"]);
    //    }
    //    if (dsStatusId.Tables[2].Rows.Count > 0)
    //    {
    //        hdnSignedId.Value = common.myStr(dsStatusId.Tables[2].Rows[0]["StatusId"]);
    //    }
    //    if (dsStatusId.Tables[0].Rows.Count > 0)
    //    {
    //        //lblFormName.Text = common.myStr(dsStatusId.Tables[0].Rows[0]["FormName"]);
    //        StatusId = common.myInt(dsStatusId.Tables[0].Rows[0]["StatusId"]);
    //        ViewState["StatusID"] = StatusId;
    //    }
    //    if (dsStatusId.Tables[3].Rows.Count > 0)
    //    {
    //        if (common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "S")
    //        {
    //            btnSeenByDoctor.Text = "Patient UnSeen";
    //        }
    //        else if (common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "O"
    //            || common.myStr(dsStatusId.Tables[3].Rows[0]["Code"]).Trim() == "VT")
    //        {
    //            btnSeenByDoctor.Text = "Patient Seen";
    //        }
    //        else
    //        {
    //            btnSeenByDoctor.Visible = false;
    //        }
    //    }

    //    if (StatusId == common.myInt(hdnSignedId.Value))
    //    {
    //        btnSentenceGallery.Enabled = false;
    //        btnSave.Enabled = false;
    //        btnSigned.Text = "UnSign";
    //        btnSeenByDoctor.Enabled = false;
    //        btnAddendum.Enabled = true;
    //        //chkUpdateTempData.Enabled = false;

    //        RTF1.EditModes = Telerik.Web.UI.EditModes.Preview;

    //        return true;
    //    }
    //    else if (StatusId == common.myInt(hdnUnSignedId.Value))
    //    {
    //        btnSigned.Text = "Sign";
    //        btnSentenceGallery.Enabled = true;
    //        btnSave.Enabled = true;
    //        btnAddendum.Enabled = false;
    //        btnSeenByDoctor.Enabled = true;
    //        //chkUpdateTempData.Enabled = true;
    //        // RTF1.EditModes = EditModes.Design;
    //        return false;
    //    }
    //    else
    //    {
    //        btnSigned.Text = "Sign";
    //        btnSentenceGallery.Enabled = true;
    //        btnSave.Enabled = true;
    //        btnAddendum.Enabled = false;
    //        btnSeenByDoctor.Enabled = true;
    //        //chkUpdateTempData.Enabled = true;
    //        //RTF1.EditModes = EditModes.Design;
    //        return false;
    //    }


    //}
    protected void btnPullForward_Click(object sender, EventArgs e)
    {

    }
    protected void btnDefaultTemplate_Click(object sender, EventArgs e)
    {

    }
    protected void btnOldNotes_Click(object sender, EventArgs e)
    {
        SaveNoFinilizedNote();

        RadWindow3.NavigateUrl = "/Emr/Letters/OldForm.aspx";
        RadWindow3.Height = 600;
        RadWindow3.Width = 900;
        RadWindow3.Top = 20;
        RadWindow3.Left = 20;
        // RadWindowForNew.Title = "Time Slot";
        RadWindow3.OnClientClose = "OnClientClose";
        RadWindow3.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow3.Modal = true;
        RadWindow3.VisibleStatusbar = false;
        RadWindow3.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow3.InitialBehaviors = WindowBehaviors.Maximize;
    }

    protected void AddStr1(string type, string Saved_RTF_Content, StringBuilder sbTemp, string Lock)
    {
        sbTemp.Append("<div id='" + type + "'><span style='color: #000000;'>");
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
            {
                sb.Append(common.myStr(sbTemp));
            }
        }
        else
        {
            //change
            Saved_RTF_Content += common.myStr(sbTemp);

            if (type != "LAB")
            {
                if (ShowNote == "True"
                && (ViewState["PullForward"] == null || ViewState["DefaultTemplate"] == null))
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

    }
    protected string GetTemplateId(string TemplateName, int HospitalLocationId)
    {
        dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        string sqlQ = "Select Id from EMRTemplate where HospitalLocationId=@HospitalLocationId and templateName like @TemplateName";
        Hashtable hs = new Hashtable();
        hs.Add("@TemplateName", TemplateName);
        hs.Add("@HospitalLocationId", HospitalLocationId);
        Object templateId = dl.ExecuteScalar(CommandType.Text, sqlQ, hs);
        return templateId.ToString();
    }
    protected void Replace(string Ttype, ref string t, string strNew, string Lock)
    {
        if (t != null)
        {
            t = t.Replace('"', '$');
            string st = "<div id=$" + Ttype + "$>";
            int RosSt = t.IndexOf(st);
            if (RosSt > 0 || RosSt == 0)
            {
                int RosEnd = t.IndexOf("</div>", RosSt);
                string str = t.Substring(RosSt, (RosEnd) - RosSt);
                string ne = t.Replace(str, strNew);
                t = ne.Replace('$', '"');
            }
            else
            {
            }
        }
        t = t.Replace('$', '"');
    }
    private void SaveNoFinilizedNote()
    {
        if (ViewState["StatusID"] != null)
        {
            if (RTF1.Content != ""
                && RTF1.Content != null
                && common.myInt(ViewState["StatusID"]) == 20)
            {
                string strCheckValue = "select RegistrationId, EncounterId from EMRPatientForms where RegistrationId=" + Session["RegistrationId"] + " and EncounterId=" + Request.QueryString["EncId"] != null ? common.myStr(Request.QueryString["EncId"]) : common.myStr(Session["EncounterId"]) + " and FormId=" + Session["formId"] + "";
                DataSet ds = new DataSet();
                hstInput = new Hashtable();
                hstInput.Add("@RegistrationId", Session["RegistrationId"]);

                hstInput.Add("@PatientSummary", RTF1.Content);
                hstInput.Add("@EncounterId", Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
                hstInput.Add("@FormId", Session["formId"]);
                hstInput.Add("@EncodedBy", Session["UserID"]);

                ds = dl.FillDataSet(CommandType.Text, strCheckValue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string SqlUpdate = " Update EMRPatientForms Set PatientSummary=@PatientSummary "
                                  + " Where RegistrationId=@RegistrationId And EncounterId = @EncounterId And FormId= @FormId";
                    dl.ExecuteNonQuery(CommandType.Text, SqlUpdate, hstInput);
                }
                else
                {
                    string SqlInsert = "INSERT into EMRPatientForms(RegistrationId,EncounterId,FormId,Priority,StatusId,Active,PatientSummary,EncodedBy,EncodedDate) VALUES(@RegistrationId,@EncounterId,@FormId,0,20,'True',@PatientSummary,@EncodedBy,GETUTCDATE())";
                    dl.ExecuteNonQuery(CommandType.Text, SqlInsert, hstInput);
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
        hstInput.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
        if (common.myInt(Session["Gender"]) == 1)
        {
            hstInput.Add("chrGenderType", "F");
        }
        else if (common.myInt(Session["Gender"]) == 2)
        {
            hstInput.Add("chrGenderType", "M");
        }

        hstInput.Add("@intFormId", common.myStr(Session["formId"]));
        DataSet dsFont = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", hstInput);
        //string BeginList = "", EndList = "", BeginList2 = "", EndList2 = "", BeginList3 = "", EndList3 = "";
        DataRow drFont = dsFont.Tables[0].Rows[0] as DataRow;


        hsGender.Add("@intRegistrationId", Session["RegistrationId"]);
        string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration where Id = @intRegistrationId";
        dsGender = DlObj.FillDataSet(CommandType.Text, SqlQry, hsGender);
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(dsGender.Tables[0].Rows[0]["Gender"]) == "Male")
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
        hsProblems.Add("@intTemplateId", GetTemplateId(sTemplateName, common.myInt(Session["HospitalLocationID"])));
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

            if (common.myStr(drFont["TemplateBold"]) != ""
                || common.myStr(drFont["TemplateItalic"]) != ""
                || common.myStr(drFont["TemplateUnderline"]) != ""
                || common.myStr(drFont["TemplateFontSize"]) != ""
                || common.myStr(drFont["TemplateForecolor"]) != ""
                || common.myStr(drFont["TemplateListStyle"]) != "")
            {
                string sBegin = "", sEnd = "";
                MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drFont);
                if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(sBegin + drFont["TemplateName"].ToString() + sEnd);
                    objStrTmp.Append(sBegin + sDisplayName.ToString() + sEnd);
                }
                //objStrTmp.Append("<br /><Strong>Positive Symptoms:</Strong>");
                //objStrTmp.Append("<br />" + sBegin + "Positive Symptoms:" + sEnd);
            }
            else
            {
                if (Convert.ToBoolean(drFont["TemplateDisplayTitle"]))
                {
                    //objStrTmp.Append(drFont["TemplateName"].ToString());//Default Setting
                    objStrTmp.Append(sDisplayName.ToString());//Default Setting
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
                    if (common.myStr(drFont["SectionsBold"]) != ""
                        || common.myStr(drFont["SectionsItalic"]) != ""
                        || common.myStr(drFont["SectionsUnderline"]) != ""
                        || common.myStr(drFont["SectionsFontSize"]) != ""
                        || common.myStr(drFont["SectionsForecolor"]) != ""
                        || common.myStr(drFont["SectionsListStyle"]) != "")
                    {
                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
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

                    if (common.myStr(dr["FieldsBold"]) != ""
                        || common.myStr(dr["FieldsItalic"]) != ""
                        || common.myStr(dr["FieldsUnderline"]) != ""
                        || common.myStr(dr["FieldsFontSize"]) != ""
                        || common.myStr(dr["FieldsForecolor"]) != ""
                        || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " has ");
                    }
                    else
                    {
                        objStrTmp.Append(strGender + " has ");
                    }

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " has ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtPositiveRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
                    dt = dv.ToTable();

                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (j == (dt.Rows.Count - 1))
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
                        {
                            objStrTmp.Append(common.myStr(dt.Rows[j]["PositiveValue"]).ToLower().Trim() + ", ");
                        }
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
                    if (common.myStr(drFont["SectionsBold"]) != ""
                        || common.myStr(drFont["SectionsItalic"]) != ""
                        || common.myStr(drFont["SectionsUnderline"]) != ""
                        || common.myStr(drFont["SectionsFontSize"]) != ""
                        || common.myStr(drFont["SectionsForecolor"]) != ""
                        || common.myStr(drFont["SectionsListStyle"]) != "")
                    {

                        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drFont);
                        if (Convert.ToBoolean(drFont["SectionDisplayTitle"]))   //19June2010
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


                    if (common.myStr(dr["FieldsBold"]) != ""
                        || common.myStr(dr["FieldsItalic"]) != ""
                        || common.myStr(dr["FieldsUnderline"]) != ""
                        || common.myStr(dr["FieldsFontSize"]) != ""
                        || common.myStr(dr["FieldsForecolor"]) != ""
                        || common.myStr(dr["FieldsListStyle"]) != "")
                    {
                        sBegin = ""; sEnd = "";
                        MakeFontWithoutListStyle("Fields", ref sBegin, ref sEnd, dr);
                        objStrTmp.Append(sBegin + strGender + " does not have ");
                    }
                    else
                    {
                        objStrTmp.Append(strGender + " does not have ");
                    }

                    //sb.Append("<br />" + dr["SectionName"].ToString().ToUpper() + ": " + strGender + " does not have ");
                    strSectionId = common.myStr(dr["SectionId"]);
                    DataView dv = new DataView(dtNegativeRos);
                    dv.RowFilter = "SectionId =" + common.myStr(dr["SectionId"]);
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
                        {
                            objStrTmp.Append(common.myStr(dt.Rows[j]["NegativeValue"]).ToLower().Trim() + ", ");
                        }
                    }
                    objStrTmp.Append(sEnd);
                }
            }
        }
        sb.Append(objStrTmp);
        //sb.Append("<br/>");
        return sb;
    }







    protected void btnExporttoWord_Click(object sender, EventArgs e)
    {
        RTF1.ExportToRtf();
    }
    protected void btnImage_Click(object sender, EventArgs e)
    {
        hdnImagePath.Value = "";
        RadWindow2.NavigateUrl = "../ImageEditor/Annotator.aspx?Page=I";
        RadWindow2.Height = 600;
        RadWindow2.Width = 1200;
        RadWindow2.Top = 20;
        RadWindow2.Left = 20;
        // RadWindowForNew.Title = "Time Slot";
        RadWindow2.OnClientClose = "GetImageOnClientClose";
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code 
        RadWindow2.Modal = true;
        RadWindow2.VisibleStatusbar = false;
        RadWindow2.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        //  BindPatientIllustrationImages();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("/EMR/Templates/Notes.aspx?ifId=" + common.myStr(Request.QueryString["ifId"]), false);
    }
    //private string BindPatientIllustrationImages()
    //{

    //    hdnImagePath.Value = "";
    //    if (hdnImagePath.Value != "")
    //    {
    //        RTF1.Content = "<table><tbody><tr><td><img src='" + hdnImagePath.Value + "' width='250px' height='250px' border='0' align='middle' alt='Image' /></td></tr></tbody></table>" + RTF1.Content;
    //    }
    //    else
    //    {
    //        BaseC.EMR emr = new BaseC.EMR(sConString);
    //        DataSet ds = emr.GetPatientIllustrationImages(Convert.ToInt32(Session["RegistrationId"]), Request.QueryString["EncId"] != null ? common.myInt(Request.QueryString["EncId"]) : common.myInt(Session["EncounterId"]));
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            int EncId = common.myInt(Request.QueryString["EncId"]);
    //            if (EncId != 0)
    //            {
    //                EncId = common.myInt(Request.QueryString["EncId"]);
    //            }
    //            else
    //            {
    //                EncId = common.myInt(Session["EncounterId"]);
    //            }

    //            DirectoryInfo objDir = new DirectoryInfo(Server.MapPath("~/PatientDocuments/" + common.myInt(Session["HospitalLocationId"]) + "/" + common.myInt(Session["RegistrationId"]) + "/" + EncId + "/"));
    //            if (objDir.Exists == true)
    //            {
    //                //     string path = "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + Request.QueryString["EncId"] != null ? Request.QueryString["EncId"].ToString() : common.myStr(Session["EncounterId"]) + "/";
    //                string path = "/PatientDocuments/" + Session["HospitalLocationID"].ToString() + "/" + Convert.ToString(Session["RegistrationId"]) + "/" + EncId + "/";
    //                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                {
    //                    hdnImagePath.Value = hdnImagePath.Value + " <table><tbody><tr><td><img src='" + path + ds.Tables[0].Rows[i]["ImageName"].ToString() + "' width='650px' height='450px' border='0' align='middle' alt='Image' /></td></tr></tbody></table><br/>";
    //                }
    //                RTF1.Content = hdnImagePath.Value + BindEditor(false);
    //            }
    //        }
    //        else
    //        {
    //            RTF1.Content = BindEditor(false);
    //        }
    //    }
    //    hdnImagePath.Value = "";
    //    return RTF1.Content;
    //}
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
            {
                sFontSize = " font-size: " + sFontSize + ";";
            }
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
        {
            sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myStr(Session["HospitalLocationId"]));
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                {
                    sBegin += " font-family: " + FontName + ";";
                }
            }
        }

        return sBegin;
    }
    protected void btnSentenceGallery_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/EMR/Templates/SentanceGallery.aspx?ctrlId=" + RTF1.ClientID + "&typ=2";
        RadWindowForNew.Height = 580;
        RadWindowForNew.Width = 650;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Sentance Gallery";
        RadWindowForNew.OnClientClose = "OnCloseSentenceGalleryRadWindow";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void btnAddendum_OnClick(object sender, EventArgs e)
    {
        RadWindowForNew.NavigateUrl = "~/Addendum.aspx";
        RadWindowForNew.Height = 445;
        RadWindowForNew.Width = 650;
        RadWindowForNew.Top = 40;
        RadWindowForNew.Left = 100;
        RadWindowForNew.Title = "Word Processor - Addendum";
        RadWindowForNew.OnClientClose = "OnClientClose";
        RadWindowForNew.Modal = true;
        RadWindowForNew.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowForNew.VisibleStatusbar = false;
        RadWindowForNew.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindowForNew.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void btnBackToWordProcessor_Click(object sender, EventArgs e)
    {
        pnlPrint.Visible = false;
        pnlCaseSheet.Visible = true;
    }
    protected void btnLetter_Click(object sender, EventArgs e)
    {
        Session["RTF"] = RTF1.Content.ToString();
        RadWindow2.NavigateUrl = "/Emr/Letters/Default.aspx";
        RadWindow2.Height = 600;
        RadWindow2.Width = 900;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;

        RadWindow2.Modal = true;
        //asdf
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;
        RadWindow2.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
    }
    protected void btnPrintPDF_Click(object sender, EventArgs e)
    {
        Session["RTF"] = RTF1.Content.ToString();

        RadWindow2.NavigateUrl = "PrintPdf.aspx";
        RadWindow2.Height = 600;
        RadWindow2.Width = 1000;
        RadWindow2.Top = 40;
        RadWindow2.Left = 100;
        RadWindow2.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow2.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindow2.Modal = true;
        //asdf
        RadWindow2.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindow2.VisibleStatusbar = false;



    }
    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
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
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    string sFontSize = "";

    protected void btnDictionary_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MPages/Dictionary.aspx", false);
    }
    protected void lnkAddAllergy_OnClick(object sender, EventArgs e)
    {
        openPopup("Allergy");
    }
    protected void lnkAddDiagnosis_OnClick(object sender, EventArgs e)
    {
        openPopup("Diagnosis");
    }
    protected void lnkAddMedication_OnClick(object sender, EventArgs e)
    {
        openPopup("Medication");
    }
    protected void lnkAddVital_OnClick(object sender, EventArgs e)
    {
        openPopup("Vital");
    }
    protected void lnkAddNote_OnClick(object sender, EventArgs e)
    {
        openPopup("Note");
    }
    protected void lnkAddProblem_OnClick(object sender, EventArgs e)
    {
        openPopup("Problem");
    }
    protected void lnkAddOrder_OnClick(object sender, EventArgs e)
    {
        openPopup("Order");
    }
    protected void lnkSpecilityTemplate_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlSpecilityTemplate.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select specility template from list !";
            Alert.ShowAjaxMsg(lblMessage.Text, this);
            return;
        }
        openPopup("SpecilityTemplate");
    }
    protected void lnkGeneralTemplate_OnClick(object sender, EventArgs e)
    {
        if (common.myInt(ddlGeneralTemplate.SelectedValue) == 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            lblMessage.Text = "Please select general template from list !";
            Alert.ShowAjaxMsg(lblMessage.Text, this);
            return;
        }
        openPopup("GeneralTemplate");
    }
    public void openPopup(string strOpen)
    {
        switch (strOpen)
        {
            case "Allergy"://Add Allergy
                RadWindow1.NavigateUrl = "~/EMR/Allergy/Allergy.aspx?From=POPUP";
                break;
            case "Diagnosis"://Add Diagnosis
                RadWindow1.NavigateUrl = "~/EMR/Assessment/Diagnosis.aspx?From=POPUP";
                break;
            case "Medication": //Add Prescription
                RadWindow1.NavigateUrl = "/EMR/Medication/MedicineOrder.aspx?Regid=" + common.myInt(Session["RegistrationID"]) +
                                        "&RegNo=" + common.myLong(Session["Regno"]) +
                                        "&EncId=" + Request.QueryString["EncId"] != null ? common.myStr(Request.QueryString["EncId"]) : common.myStr(Session["EncounterId"]) +
                                        "&EncNo=" + common.myInt(Session["Encno"]);
                break;
            case "Vital"://Add Vitals
                RadWindow1.NavigateUrl = "~/EMR/Vitals/Vitals.aspx?From=POPUP";
                break;
            case "Note"://Add Notes
                RadWindow1.NavigateUrl = "~/EMR/Templates/NoteSelect.aspx?From=POPUP";
                break;
            case "Problem"://Add Problems
                RadWindow1.NavigateUrl = "~/EMR/Problems/Default.aspx?From=POPUP";
                break;
            case "Order"://Add Orders
                RadWindow1.NavigateUrl = "/EMRBILLING/Popup/AddServices.aspx?Regid=" + common.myInt(Session["RegistrationID"]) +
                    "&RegNo=" + common.myLong(Session["Regno"]) +
                    "&EncId=" + Request.QueryString["EncId"] != null ? common.myStr(Request.QueryString["EncId"]) : common.myStr(Session["EncounterId"]) +
                    "&EncNo=" + common.myInt(Session["Encno"]) +
                    "&OP_IP=I&FromWard=Y&CompanyId=0&InsuranceId=0&CardId=0&PayerType=&BType=";
                break;
            case "SpecilityTemplate"://Add SpecilityTemplate
                RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?Regno=" + common.myStr(Session["RegistrationNo"]).Trim() + "&MASTER=No&Mpg=P" + common.myStr(ddlSpecilityTemplate.SelectedValue);
                break;
            case "GeneralTemplate"://Add GeneralTemplate
                RadWindow1.NavigateUrl = "/EMR/Templates/Default.aspx?Regno=" + common.myStr(Session["RegistrationNo"]).Trim() + "&MASTER=No&Mpg=p" + common.myStr(ddlGeneralTemplate.SelectedValue);
                break;
        }

        RadWindow1.Height = 600;
        RadWindow1.Width = 1050;
        RadWindow1.Top = 10;
        RadWindow1.Left = 10;
        RadWindow1.OnClientClose = "FillOnClientClose";
        RadWindow1.VisibleOnPageLoad = true;
        RadWindow1.Modal = true;
        RadWindow1.VisibleStatusbar = false;
        RadWindow1.Behaviors = WindowBehaviors.Close | WindowBehaviors.Minimize | WindowBehaviors.Maximize | WindowBehaviors.Pin;
        RadWindow1.InitialBehaviors = WindowBehaviors.Maximize;
    }




    //protected void ddlTemplatePatient_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bindDetails();
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblMessage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
    //        lblMessage.Text = "Error: " + Ex.Message;
    //        objException.HandleException(Ex);
    //    }
    //}




    protected void btnPrintPDFReport_Click(object sender, EventArgs e)
    {
        int TemplateId = common.myInt(Request.QueryString["TemplateId"].ToString());
        Session["EMRTemplatePrintData"] = BindEditor(false, TemplateId);
        RadWindowPrint.NavigateUrl = "/EMR/Templates/PrintPdf1.aspx?";
        RadWindowPrint.Height = 598;
        RadWindowPrint.Width = 900;
        RadWindowPrint.Top = 10;
        RadWindowPrint.Left = 10;
        RadWindowPrint.Modal = true;
        RadWindowPrint.OnClientClose = string.Empty; //"OnClientClose";
        RadWindowPrint.Behaviors = WindowBehaviors.Maximize | WindowBehaviors.Minimize | WindowBehaviors.Close | WindowBehaviors.Move | WindowBehaviors.Pin;
        RadWindowPrint.InitialBehaviors = WindowBehaviors.Maximize;
        RadWindowPrint.VisibleOnPageLoad = true; // Set this property to True for showing window from code    
        RadWindowPrint.VisibleStatusbar = false;
    }
}