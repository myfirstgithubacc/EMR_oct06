using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.IO;

public class BindSummary
{
    string sConString = "";
    Hashtable hstInput;
    Hashtable houtPara;
    DataSet ds;
    DataSet ds1;
    DataSet ds2;
    SqlDataReader dr;
    DL_Funs ff = new DL_Funs();
    BaseC.Patient bc;
    public BindSummary(string Constring)
    {
        sConString = Constring;
        bc = new BaseC.Patient(sConString);
    }
    public string getDefaultFontSize(Page pg, string HospitalLocationId)
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", HospitalLocationId);
        if (FieldValue != "")
        {
            sFontSize = " font-size: " + FieldValue + ";";
        }
        return sFontSize;
    }

    public StringBuilder DisplayUserNameInNote(string userID, int hospitalID, string enterDate, DataRow drTemplateListStyle, Page pg)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        StringBuilder sb = new StringBuilder();
        string sBeginFont = "", sEndFont = "";
        StringBuilder sbDisplayName = new StringBuilder();
        Hashtable hshUser = new Hashtable();
        hshUser.Add("@UserID", userID);
        hshUser.Add("@inyHospitalLocationID", hospitalID);
        string strUser = "Select ISNULL(tm.Name + ' ','') +  ISNULL(LastName,'') +  ISNULL(' ' + FirstName,'') +  ISNULL(' ' + MiddleName,'') AS EmployeeName FROM Employee em WITH (NOLOCK) INNER JOIN Users us WITH (NOLOCK) ON em.ID=us.EmpID LEFT JOIN TitleMaster tm WITH (NOLOCK) on em.TitleId=tm.TitleID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";
        DataSet dsUser = objtlc.GetDataSet(CommandType.Text, strUser, hshUser);
        if (dsUser.Tables[0].Rows.Count > 0)
        {
            sb.Append("<br/>");
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
            sbDisplayName.Append(sBeginFont + "Entered by " + dsUser.Tables[0].Rows[0]["EmployeeName"].ToString() + " on " + enterDate + "" + sEndFont);
        }
        sb.Append(sbDisplayName + "<br/><br/>");
        objtlc = null;
        return sb;
    }
    //public DataSet GetTemplateStyle(int iHospitalLocationId)
    //{
    //    Hashtable hst = new Hashtable();
    //    DataSet ds = new DataSet();
    //    hst.Add("@inyHospitalLocationID", iHospitalLocationId);
    //    string sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
    //    + " TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
    //    + " SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
    //    + " FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber "
    //    + " FROM EMRTemplateStatic WITH (NOLOCK) where (HospitalLocationId = @inyHospitalLocationID OR HospitalLocationId IS NULL)";
    //    ds = DlObj.FillDataSet(CommandType.Text, sql, hst);
    //    return ds;
    //}

    public DataSet GetTemplateStyle(int iHospitalLocationId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        Hashtable hst = new Hashtable();
        DataSet ds = new DataSet();
        hst.Add("@inyHospitalLocationID", iHospitalLocationId);
        string sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
        + "TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
        + "SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
        + "FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber, CODE, StaticTemplateName "
        + "FROM EMRTemplateStatic WITH(NOLOCK) WHERE HospitalLocationId = @inyHospitalLocationID";
        ds = objtlc.GetDataSet(CommandType.Text, sql, hst);

        objtlc = null;
        return ds;
    }
    public StringBuilder GetEncounterFollowUpAppointment(string HospitalId, int EncounterId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string date = string.Empty;
        int i = 0;
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationId", HospitalId);
        hsTb.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        hsTb.Add("@intEncounterId", EncounterId);

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetEncounterFollowUpAppointment", hsTb);
        objtlc = null;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle + "<u><b>" + "Follow-up Appointment: " + "</b></u>" + BeginList);
            // sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append("<br/>");
            sb.Append(BeginList);
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sb.Append(sBegin + "Appointment Date : " + ds.Tables[0].Rows[i]["AppointmentDate"].ToString() + " " + ds.Tables[0].Rows[i]["FromTime"].ToString());
                sb.Append(", Visit Type : " + ds.Tables[0].Rows[i]["VisitType"].ToString());
                sb.Append(", Doctor Name : " + ds.Tables[0].Rows[i]["DoctorName"].ToString());
                sb.Append(ds.Tables[0].Rows[i]["Remarks"].ToString().Trim() != "" ? ", Remarks : " + ds.Tables[0].Rows[i]["Remarks"].ToString() + ". " : ".");
                sb.Append(sEnd + "<br/>");
            }
        }
        sb.Append(EndList);
        return sb;
    }

    public StringBuilder DisplayUserNameInNote(string userID, string hospitalID, string enterDate, string pageID)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        StringBuilder sb = new StringBuilder();

        Hashtable hshtable = new Hashtable();
        StringBuilder sbDisplayName = new StringBuilder();
        hshtable.Add("@intTemplateId", pageID);
        hshtable.Add("@inyHospitalLocationID", hospitalID);
        string strDisplayUserName = "select DisplayUserName from EMRTemplateStatic WITH (NOLOCK) where pageid=@intTemplateId and (HospitalLocationID=@inyHospitalLocationID OR HospitalLocationID IS NULL)";
        DataSet dsDisplayName = objtlc.GetDataSet(CommandType.Text, strDisplayUserName, hshtable);
        if (dsDisplayName.Tables[0].Rows.Count > 0)
        {
            if (dsDisplayName.Tables[0].Rows[0]["DisplayUserName"].ToString() == "True")
            {
                Hashtable hshUser = new Hashtable();
                hshUser.Add("@UserID", userID);
                hshUser.Add("@inyHospitalLocationID", hospitalID);
                string strUser = "Select ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS EmployeeName FROM Employee em WITH (NOLOCK) INNER JOIN Users us WITH (NOLOCK) ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";
                DataSet dsUser = objtlc.GetDataSet(CommandType.Text, strUser, hshUser);
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<br/>");
                    sbDisplayName.Append("Entered and Verified by " + dsUser.Tables[0].Rows[0]["EmployeeName"].ToString() + " " + Convert.ToString(Convert.ToDateTime(enterDate).Date.ToString("MMMM dd yyyy")) + "");
                }
            }
        }
        sb.Append(sbDisplayName);

        objtlc = null;
        return sb;
    }


    public StringBuilder BindVitals(string HospitalId, int EncounterId,
                                    StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                    Page pg, string pageID, string userID, string fromDate, string toDate, int TemplateFieldId, string sEREncounterId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        DataSet dsVitalData = new DataSet();
        hsTb.Add("@inyHospitalLocationId", HospitalId);
        hsTb.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        hsTb.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));
        hsTb.Add("@intEncounterId", EncounterId);
        hsTb.Add("@intEREncounterId", sEREncounterId);

        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsTb.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }
        dsVitalData = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", hsTb);
        objtlc = null;

        DataView dv = new DataView(dsVitalData.Tables[0]);
        dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId;
        ds.Tables.Add(dv.ToTable());
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        sBeginSection = sBegin;
        sEndSection = sEnd;
        int t = 0;

        StringBuilder vital = new StringBuilder();

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                //if (t == 0)
                //{

                //    sb.Append("<br /><u><b>" + sb  + "</b></u><br /><br />" + sEndSection + BeginList);

                //    t = 1;
                //}

                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append(BeginList);


            //"Vital Date","T","R","P","HT","WT","BPS","BPD","HC","MAC","BMI","BSA","Pain","T_ABNORMAL_VALUE"

            vital.Append("<table border='1' style='font-size:12px;' cellspacing='0' >");
            vital.Append("<tr>");

            vital.Append("<td style='font-weight:bold;' valign='top'>" + sBegin + "Vital Date" + sEnd + "</td>");

            int totColumns = 0;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (!common.myStr(col.Caption).Contains("Date"))
                {
                    if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
                    {
                        break;
                    }

                    vital.Append("<td style='font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
                    totColumns++;
                }
            }

            vital.Append("</tr>");

            string vtVitalDate = string.Empty;
            string vtValue = string.Empty;

            string sFontStart = "<font color=red><b>";
            string sFontEnd = "</b></font>";

            string colName = "";
            DateTime vitalDate = common.myDate(ds.Tables[0].Rows[0]["Vital Date"]);
            bool isFirstRow = false;

            foreach (DataRow vDR in ds.Tables[0].Rows)
            {
                isFirstRow = false;
                if (vitalDate == common.myDate(vDR["Vital Date"]))
                {
                    if (vitalDate.ToString("dd/MM/yyyy") == common.myDate(vDR["Vital Date"]).ToString("dd/MM/yyyy"))
                    {
                        isFirstRow = true;
                    }
                    else
                    {
                        isFirstRow = false;
                    }
                }
                else
                {
                    if (vitalDate.ToString("dd/MM/yyyy") == common.myDate(vDR["Vital Date"]).ToString("dd/MM/yyyy"))
                    {
                        isFirstRow = false;
                    }
                    else
                    {
                        isFirstRow = true;
                    }
                }

                vitalDate = common.myDate(vDR["Vital Date"]);

                if (isFirstRow)
                {
                    vital.Append("<tr>");

                    vital.Append("<td valign='top' colspan='" + (totColumns + 1) + "'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");

                    vital.Append("</tr>");

                    isFirstRow = false;
                }

                vital.Append("<tr>");
                vital.Append("<td valign='top'>" + sBegin + (isFirstRow ? vitalDate.ToString("dd/MM/yyyy") : vitalDate.ToShortTimeString()) + sEnd + "</td>");

                for (int cIdx = (0 + 2); cIdx < (totColumns + 2); cIdx++)
                {
                    colName = ds.Tables[0].Columns[cIdx].Caption;
                    vtValue = (common.myStr(vDR[colName]).Trim().Length == 0) ? "&nbsp;" : common.myStr(vDR[colName]);
                    if (common.myInt(vDR[colName]) == 1)
                    {
                        vtValue = sFontStart + vtValue + sFontEnd;
                    }
                    vital.Append("<td valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                }
                vital.Append("</tr>");

            }

            vital.Append("</table>");
        }

        sb.Append(vital);
        return sb;
    }

    public StringBuilder BindDoctorProgressNote(string HospitalId, int RegistrationId, int DoctorId,
                                    StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                    Page pg, string pageID, string userID, string fromDate, string toDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationId", HospitalId);
        hsTb.Add("@intLoginFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        hsTb.Add("@intRegistrationId", common.myInt(RegistrationId));
        hsTb.Add("@intDoctorId", DoctorId);

        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd"));
            hsTb.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetDoctorProgressNote", hsTb);
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        sBeginSection = sBegin;
        sEndSection = sEnd;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(sbTemplateStyle).Contains("Doctor Progress Note") == false)
            {
                sb.Append("<u><b>" + sb + "Doctor Progress Note:  " + "</b></u><br />" + sEndSection + BeginList);
            }
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append(BeginList);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["ProgressNote"].ToString() != "")
                {
                    sb.Append(i == 0 ? sBegin + ds.Tables[0].Rows[i]["ProgressNote"].ToString() + sEnd : "<br/><br/>" + sBegin + ds.Tables[0].Rows[i]["ProgressNote"].ToString() + sEnd);
                }
            }
        }
        sb.Append(EndList);
        objtlc = null;
        return sb;
    }
    public StringBuilder BindImmunization(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string userID)
    {
        return BindImmunization(HospitalId, RegNo, EncounterID, sb, sbTemplateStyle, drTemplateListStyle, pg,
            pageID, userID, "", "");
    }

    public StringBuilder BindImmunization(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                string pageID, string userID, string fromDate, string toDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string date = string.Empty;
        int i = 0;
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationID", HospitalId);
        hsTb.Add("@intRegistrationId ", RegNo);
        hsTb.Add("@intEncounterId", EncounterID);

        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chvFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsTb.Add("@chvToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunization", hsTb);
        objtlc = null;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            //if (common.myStr(sbTemplateStyle).Contains("Immunization") == false)
            //{
            //    sb.Append(sbTemplateStyle + "<u><b>" + "Immunization:" + "</b></u>" + BeginList);
            //}
            if (ds.Tables[0].Rows[0]["GivenDate"].ToString() != null)
            {
                sb.Append(sbTemplateStyle);
                if (drTemplateListStyle != null)
                {
                    if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                    {
                        BeginList = "<ul>"; EndList = "</ul>";
                    }
                    else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                    {
                        BeginList = "<ol>"; EndList = "</ol>";
                    }
                }
                sb.Append("<br/>");
                sb.Append(BeginList);
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Remarks"].ToString() == "")
                    {
                        sb.Append(sBegin + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + ", "
                       + "Given Date: " + ds.Tables[0].Rows[i]["GivenDate"].ToString() + ". " + sEnd + "<br/>");
                    }
                    else
                    {
                        sb.Append(sBegin + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + ", "
                       + "Given Date: " + ds.Tables[0].Rows[i]["GivenDate"].ToString()
                        + ", Remarks: " + ds.Tables[0].Rows[i]["Remarks"].ToString() + ". " + sEnd + "<br/>");
                    }
                }
            }
        }
        sb.Append(EndList);
        return sb;
    }

    public StringBuilder BindInjection(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string userID)
    {
        return BindInjection(HospitalId, RegNo, EncounterID, sb, sbTemplateStyle, drTemplateListStyle, pg,
                     pageID, userID, "", "");
    }

    public StringBuilder BindInjection(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string userID, string fromDate, string toDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string date = string.Empty;
        int i = 0;
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationID", HospitalId);
        hsTb.Add("@intRegistrationId ", RegNo);
        hsTb.Add("@intEncounterId", EncounterID);

        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chvFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsTb.Add("@chvToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDailyInjections", hsTb);

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle + "<u><b>" + "Injection:" + "</b></u>" + BeginList);
            if (ds.Tables[0].Rows[0]["GivenDateTime"].ToString() != null)
            {
                sb.Append(sbTemplateStyle);
                if (drTemplateListStyle != null)
                {
                    if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                    {
                        BeginList = "<ul>"; EndList = "</ul>";
                    }
                    else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                    {
                        BeginList = "<ol>"; EndList = "</ol>";
                    }
                }
                sb.Append("<br/>");
                sb.Append(BeginList);
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["Remarks"].ToString() == "")
                    {
                        sb.Append(sBegin + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + ", "
                       + "Given Date: " + ds.Tables[0].Rows[i]["GivenDateTime"].ToString() + ". " + sEnd + "<br/>");
                    }
                    else
                    {
                        sb.Append(sBegin + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + ", "
                       + "Given Date: " + ds.Tables[0].Rows[i]["GivenDateTime"].ToString()
                        + ", Remarks: " + ds.Tables[0].Rows[i]["Remarks"].ToString() + ". " + sEnd + "<br/>");
                    }
                }
            }
        }
        sb.Append(EndList);
        return sb;
    }

    public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        return BindProblemsHPI(RegId, HospitalId, EncounterId, sb, sbTemplateStyle,
            drTemplateListStyle, pg, pageID, userID, "", "");
    }



    public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, StringBuilder sb,
                      StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                      string pageID, string userID, string fromDate, string toDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        StringBuilder sbTemp = new StringBuilder();
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        Hashtable hsProblems = new Hashtable();
        DataTable dtChronic = new DataTable();
        DataTable dtNonChronic = new DataTable();
        DataSet dsProblem = new DataSet();
        string strSql = "";
        string strAge = "";
        hsProblems.Add("@inyHospitalLocationID", HospitalId);
        hsProblems.Add("@intRegistrationId", RegId);
        hsProblems.Add("@intEncounterId", EncounterId);

        if (fromDate != "" && toDate != "")
        {
            hsProblems.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsProblems.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }

        strSql = "Select ISNULL(r.FirstName,'') + ISNULL(' ' + r.MiddleName,'') + ISNULL(' ' + r.LastName,'') AS PatientName, dbo.AgeInYrsMonthDay (Convert(varchar(10),DateofBirth,111), Convert(Varchar(10), Getdate(),111)) as Age, dbo.GetGender(Gender) as Gender,";
        strSql = strSql + " c.Race from Registration r WITH (NOLOCK) Left Join RaceMaster c WITH (NOLOCK) On r.RaceId = c.RaceId";
        strSql = strSql + " where r.Id = @intRegistrationId ";
        strSql = strSql + " Select IsNull(IsPregnant,0) as IsPregnant, IsNull(IsBreastFeeding,0) as IsBreastFeeding FROM Encounter WITH (NOLOCK) where Id = @intEncounterId";

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", hsProblems);
        ds2 = objtlc.GetDataSet(CommandType.Text, strSql, hsProblems);
        objtlc = null;

        if (Convert.ToString(ds2.Tables[0].Rows[0]["Age"]).Trim() != "")
        {
            strAge = Convert.ToString(ds2.Tables[0].Rows[0]["Age"]).Trim();
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            //if (common.myStr(sbTemplateStyle).Contains("Chief Complaints") == false)
            //{
            sb.Append(sbTemplateStyle + "<u><b>" + "</b></u>" + BeginList);
            //}
            if (drTemplateListStyle != null)
            {
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId.ToString());

            DataView dvChronic = ds.Tables[0].DefaultView;
            dvChronic.RowFilter = "IsChronic = 'True'";
            dtChronic = dvChronic.ToTable();
            DataView dvNonChronic = ds.Tables[0].DefaultView;
            dvNonChronic.RowFilter = "IsChronic = 'False'";
            dtNonChronic = dvNonChronic.ToTable();
            if (dtChronic.Rows.Count > 0)
            {

                sbTemp.Append(sBeginFont);

                //sbTemp.Append("The patient, " + Convert.ToString(ds2.Tables[0].Rows[0]["PatientName"]).Trim() + ", is a ");
                //sbTemp.Append(strAge + " old " + Convert.ToString(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " with a history of ");

                #region commented // Chronic problems starts here
                //#region // Chronic problems starts here
                //for (int i = 0; i < dtChronic.Rows.Count; i++)
                //{
                //    DataRow dr1 = dtChronic.Rows[i] as DataRow;
                //    if (i == 0)
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtChronic.Rows.Count - 1)
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //    }

                //    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
                //            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtChronic.Rows.Count - 1)
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
                //            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtChronic.Rows.Count - 1)
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
                //            sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtChronic.Rows.Count - 1)
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
                //            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtChronic.Rows.Count - 1)
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().Trim() + ", ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
                //    {
                //        if (i == dtChronic.Rows.Count - 1)
                //        {
                //            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                //        }
                //        else
                //        {
                //            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                //        }
                //    }
                //}

                //#endregion // Chronic problems sentence ends here
                #endregion

                #region commented non chronic problem sentence start
                //#region non chronic problem sentence start
                //// Non Chronic problems starts here
                //for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                //{
                //    //intRowCount = Convert.ToInt16( dtNonChronic.Rows.Count - 1 );
                //    DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                //    if (i == 0)
                //    {
                //        sbTemp.Remove(sbTemp.Length - 2, 2);
                //        sbTemp.Append(" who presents with ");
                //        //sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //        //
                //        if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //    }
                //    else
                //    {

                //        if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //        {
                //            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //        }
                //        else
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                //        }
                //    }

                //    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
                //            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
                //            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
                //            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
                //            sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                //        else
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                //        }
                //    }
                //    if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
                //    {
                //        if (i == dtNonChronic.Rows.Count - 1)
                //        {
                //            sbTemp.Append(" and " + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                //        }
                //        else
                //        {
                //            sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                //        }
                //    }
                //}
                //sbTemp.Append(sEndFont);
                //#endregion //Non Chronic problems ends here.
                #endregion
            }
            else
            {

                #region if only non chronic problem then sentence start here
                sbTemp.Append(sBeginFont);

                //sbTemp.Append("The patient, " + Convert.ToString(ds2.Tables[0].Rows[0]["PatientName"]).Trim() + ", is a ");
                //sbTemp.Append(strAge + "  old " + Convert.ToString(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " who presents with ");

                #region comment for Non Chronic sentence which generate in case sheet.
                //    for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                //    {
                //        DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                //        if (i == 0)
                //        {
                //            if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //                sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //            else
                //            {
                //                if (i == dtNonChronic.Rows.Count - 1)
                //                {
                //                    sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + "<br /> ");
                //                }
                //                else
                //                    sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + "<br /> ");
                //            }
                //        }
                //        else
                //        {
                //            if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //            {
                //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //            }
                //            else
                //            {
                //                if (i == dtNonChronic.Rows.Count - 1)
                //                {
                //                    sbTemp.Remove(sbTemp.Length - 2, 2);
                //                    sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                //                }
                //                else
                //                    sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }

                //        if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
                //        {
                //            if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
                //                sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                //            else
                //            {
                //                if (i == dtNonChronic.Rows.Count - 1)
                //                {
                //                    sbTemp.Remove(sbTemp.Length - 2, 2);
                //                    sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                //                }
                //                else
                //                    sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //        if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
                //        {
                //            if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
                //                sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                //            else
                //            {
                //                if (i == dtNonChronic.Rows.Count - 1)
                //                {
                //                    sbTemp.Remove(sbTemp.Length - 2, 2);
                //                    sbTemp.Append(" and " + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
                //                }
                //                else
                //                    sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //        if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
                //        {
                //            if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
                //                sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                //            else
                //            {
                //                if (i == dtNonChronic.Rows.Count - 1)
                //                {
                //                    sbTemp.Remove(sbTemp.Length - 2, 2);
                //                    sbTemp.Append("and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                //                }
                //                else
                //                    sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //        if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
                //        {
                //            if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
                //                sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                //            else
                //            {
                //                if (i == dtNonChronic.Rows.Count - 1)
                //                {
                //                    sbTemp.Remove(sbTemp.Length - 2, 2);
                //                    sbTemp.Append(" and " + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ". ");
                //                }
                //                else
                //                    sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                //            }
                //        }
                //        if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
                //        {
                //            if (i == dtNonChronic.Rows.Count - 1)
                //            {
                //                sbTemp.Append("and " + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                //        }
                //    }
                //    sbTemp.Append(sEndFont);
                //}
            }
            #endregion
            #endregion if non chronic problem sentence ends

            //if (Convert.ToString(ds2.Tables[1].Rows[0]["IsPregnant"]) != "False")
            //    sbTemp.Append(sBeginFont + " The patient is or may be pregnant. " + sEndFont);
            //if (Convert.ToString(ds2.Tables[1].Rows[0]["IsBreastFeeding"]) != "False")
            //    sbTemp.Append(sBeginFont + " The patient is breast feeding. " + sEndFont);

            //sbTemp.Append(BeginList); //cut from here 
            #region chronic problem listing
            for (int i = 0; i < dtChronic.Rows.Count; i++)
            {
                //sbTemp.Append(BeginList);// pasted here 
                //sbTemp.Append(sBegin + " The patient describes the chronic " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                DataRow dr1 = dtChronic.Rows[i] as DataRow;
                if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Quality2"]) != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
                {
                    //Changed onsetDate to onset
                    //Added this line to append string after if condition,previously it is above the if condition
                    //Vineet
                    sbTemp.Append(sBegin + " The patient describes the chronic " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                    if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
                    {
                        if (Convert.ToString(dr1["Quality2"]).Trim() != "")
                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
                            }
                            else
                                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
                        }
                    }
                    if (Convert.ToString(dr1["Quality2"]).Trim() != "")
                    {
                        if (Convert.ToString(dr1["Quality3"]).Trim() != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality3"]).Trim() != "")
                    {
                        if (Convert.ToString(dr1["Quality4"]).Trim() != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]).Trim() == "") && (Convert.ToString(dr1["Severity"]).Trim() == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality4"]).Trim() != "")
                    {
                        if (Convert.ToString(dr1["Quality5"]).Trim() != "")
                            sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
                        else
                        {
                            if ((Convert.ToString(dr1["Location"]).Trim() == "") && (Convert.ToString(dr1["Severity"]).Trim() == ""))
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
                            }
                        }
                    }
                    if (Convert.ToString(dr1["Quality5"]).Trim() != "")
                    {
                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
                        }
                        else
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
                        }
                    }

                    if (Convert.ToString(dr1["Location"]).Trim() != "")
                    {
                        if (Convert.ToString(dr1["Severity"]).Trim() != "")
                        {
                            if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
                            {
                                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + ") ");
                            }
                            else
                                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
                        }
                        else
                        {
                            if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
                            {
                                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + "). ");
                            }
                            else
                                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
                        }
                    }
                    if (Convert.ToString(dr1["Severity"]).Trim() != "")
                    {
                        if (Convert.ToString(dr1["OnsetDate"]).Trim() != "")
                            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                        else
                        {
                            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                        }
                    }


                    //sbTemp.Append("that began on [OnSet Date] ");  
                    if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower() + " and " + Convert.ToString(dr1["Duration"]).ToLower() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("symptoms for the  last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "")
                    {
                        if (i == 0)
                            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                    }
                    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
                    {
                        if (i == 0)
                            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                        else
                            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                    }

                    if (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
                    {
                        if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
                            sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                        else
                            sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times. ");
                    }
                    if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
                        sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");

                }

                //asdfg
                if (Convert.ToString(dr1["AggravatingFactors"]).Trim() != "")
                    sbTemp.Append(" Aggravating factors are: " + Convert.ToString(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
                if (Convert.ToString(dr1["RelievingFactors"]).Trim() != "")
                    sbTemp.Append(" Relieving factors are:  " + Convert.ToString(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
                if ((Convert.ToString(dr1["DeniesSymptoms1"]) != "") || (Convert.ToString(dr1["DeniesSymptoms2"]) != "") || (Convert.ToString(dr1["DeniesSymptoms3"]) != "") || (Convert.ToString(dr1["DeniesSymptoms4"]) != "") || (Convert.ToString(dr1["DeniesSymptoms5"]) != ""))
                    sbTemp.Append(" Patient denies symptoms of ");
                if (Convert.ToString(dr1["DeniesSymptoms1"]) != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
                {
                    if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
                    {
                        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
                    }
                    else
                    {
                        sbTemp.Remove(sbTemp.Length - 2, 2);
                        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
                    }
                }

                if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
                {
                    sbTemp.Remove(sbTemp.Length - 2, 2);
                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
                }

                if (Convert.ToString(dr1["Condition"]).Trim() != "")
                {
                    sbTemp.Append("The patient's condition is " + Convert.ToString(dr1["Condition"]).ToLower().Trim());
                    if (Convert.ToString(dr1["Percentage"]) != "0" && Convert.ToString(dr1["Percentage"]) != "")
                    {
                        sbTemp.Append(" by " + Convert.ToString(dr1["Percentage"]).Trim() + "% .");
                    }
                    else
                    {
                        sbTemp.Append(". ");
                    }
                }

                sbTemp.Append(sEnd);
                //sbTemp.Append(EndList);// pasted here

            }
            #endregion

            # region nonchronic problem details with listing
            for (int i = 0; i < dtNonChronic.Rows.Count; i++)
            {
                //sbTemp.Append(BeginList);// pasted here   
                DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                //sbTemp.Append("<br /><br />" + Convert.ToString(dr1["ProblemDescription"]) + " presents with ");
                //sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");


                //if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
                //{
                //Changed onsetDate to onset
                //Added this line to append string after if condition,previously it is above the if condition
                //Vineet


                //sbTemp.Append(sBegin + " The patient describes the chronic " + common.myStr(dr1["ProblemDescription"]).ToLower() + " ");

                // sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]).ToLower() + "<br/>");
                sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]) + "<br/>");

                #region Comment the sentence for chronic which generate in case sheet.
                //    if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
                //    {
                //        if (Convert.ToString(dr1["Quality2"]).Trim() != "")
                //            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
                //        else
                //        {
                //            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                //            {
                //                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
                //            }
                //            else
                //                sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
                //        }
                //    }
                //    if (Convert.ToString(dr1["Quality2"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["Quality3"]).Trim() != "")
                //            sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
                //        else
                //        {
                //            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["Quality3"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["Quality4"]).Trim() != "")
                //            sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
                //        else
                //        {
                //            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["Quality4"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["Quality5"]).Trim() != "")
                //            sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
                //        else
                //        {
                //            if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
                //            }
                //            else
                //            {
                //                sbTemp.Remove(sbTemp.Length - 2, 2);
                //                sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
                //            }
                //        }
                //    }
                //    if (Convert.ToString(dr1["Quality5"]).Trim() != "")
                //    {
                //        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                //        {
                //            sbTemp.Remove(sbTemp.Length - 2, 2);
                //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
                //        }
                //        else
                //        {
                //            sbTemp.Remove(sbTemp.Length - 2, 2);
                //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
                //        }
                //    }

                //    if (Convert.ToString(dr1["Location"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["Severity"]).Trim() != "")
                //        {
                //            if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
                //            {
                //                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + ") ");
                //            }
                //            else
                //                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
                //        }
                //        else
                //        {
                //            if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
                //            {
                //                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + "). ");
                //            }
                //            else
                //                sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
                //        }
                //    }
                //    if (Convert.ToString(dr1["Severity"]).Trim() != "")
                //    {
                //        if (Convert.ToString(dr1["Onset"]).Trim() != "")
                //            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                //    }
                //    //sbTemp.Append("that began on [OnSet Date] "); 



                //    if (Convert.ToString(dr1["Onset"]).Trim() != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                //    {
                //        if (i == 0)
                //            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //    }
                //    else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                //    {
                //        if (i == 0)
                //            sbTemp.Append("symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //    }
                //    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                //    {
                //        if (i == 0)
                //            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //    }
                //    else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                //    {
                //        if (i == 0)
                //            sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                //    }
                //    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) == "")
                //    {
                //        if (i == 0)
                //            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                //    }
                //    else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
                //    {
                //        if (i == 0)
                //            sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                //        else
                //            sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                //    }

                //    if (Convert.ToString(dr1["NoOfOccurrence"]) != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
                //    {
                //        if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
                //            sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                //        else
                //            sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times.");
                //    }
                //    if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
                //        sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");
                //}
                //if (Convert.ToString(dr1["AggravatingFactors"]).Trim() != "")
                //    sbTemp.Append(" Aggravating factors are: " + Convert.ToString(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
                //if (Convert.ToString(dr1["RelievingFactors"]).Trim() != "")
                //    sbTemp.Append(" Relieving factors are:  " + Convert.ToString(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
                //if ((Convert.ToString(dr1["DeniesSymptoms1"]) != "") || (Convert.ToString(dr1["DeniesSymptoms2"]) != "") || (Convert.ToString(dr1["DeniesSymptoms3"]) != "") || (Convert.ToString(dr1["DeniesSymptoms4"]) != "") || (Convert.ToString(dr1["DeniesSymptoms5"]) != ""))
                //    sbTemp.Append(" Patient denies symptoms of ");
                //if (Convert.ToString(dr1["DeniesSymptoms1"]) != "")
                //{
                //    if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
                //    {
                //        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
                //    }
                //    else
                //    {
                //        //sbTemp.Remove(sbTemp.Length - 2, 2);
                //        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
                //    }
                //}

                //if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
                //{
                //    if (Convert.ToString(dr1["DeniesSymptoms3"]) != "")
                //    {
                //        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
                //    }
                //    else
                //    {
                //        sbTemp.Remove(sbTemp.Length - 2, 2);
                //        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
                //    }
                //}

                //if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
                //{
                //    if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
                //    {
                //        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
                //    }
                //    else
                //    {
                //        sbTemp.Remove(sbTemp.Length - 2, 2);
                //        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
                //    }
                //}

                //if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
                //{
                //    if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
                //    {
                //        sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
                //    }
                //    else
                //    {
                //        sbTemp.Remove(sbTemp.Length - 2, 2);
                //        sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
                //    }
                //}

                //if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
                //{
                //    sbTemp.Remove(sbTemp.Length - 2, 2);
                //    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
                //}
                //if (Convert.ToString(dr1["Condition"]).Trim() != "")
                //{
                //    sbTemp.Append("The patient's condition is " + Convert.ToString(dr1["Condition"]).ToLower().Trim());
                //    if (Convert.ToString(dr1["Percentage"]) != "0" && Convert.ToString(dr1["Percentage"]) != "")
                //    {
                //        sbTemp.Append(" by " + Convert.ToString(dr1["Percentage"]).Trim() + "% .");
                //    }
                //    else
                //    {
                //        sbTemp.Append(". ");
                //    }
                //}

                #endregion
                //}
                sbTemp.Append(sEnd);
                //sbTemp.Append(EndList);// pasted here      
            }
            # endregion
        }
        //if (sbTemp.ToString() != "")
        //{
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                if (common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() != "")
                {
                    //  sb.Append(sbTemplateStyle + "<u><b>" + "Chief Complaints:" + "</b></u><br/>" + BeginList);
                }
            }
            if (sbTemplateStyle.ToString() == "")
            {
                sb.Append(sbTemplateStyle);
            }

            if (common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() != "")
            {
                sbTemp.Append(sBeginFont + common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() + sEndFont);
            }
        }
        sb.Append(sbTemp);
        return sb;
    }

    //public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, StringBuilder sb,
    //                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
    //                    string pageID, string userID, string fromDate, string toDate)
    //{
    //    ds = new DataSet();
    //    string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
    //    StringBuilder sbTemp = new StringBuilder();
    //    BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
    //    Hashtable hsProblems = new Hashtable();
    //    DataTable dtChronic = new DataTable();
    //    DataTable dtNonChronic = new DataTable();
    //    DataSet dsProblem = new DataSet();
    //    string strSql = "";
    //    string strAge = "";
    //    hsProblems.Add("@inyHospitalLocationID", HospitalId);
    //    hsProblems.Add("@intRegistrationId", RegId);
    //    hsProblems.Add("@intEncounterId", EncounterId);

    //    if (fromDate != "" && toDate != "")
    //    {
    //        hsProblems.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
    //        hsProblems.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
    //    }

    //    strSql = "Select ISNULL(r.FirstName,'') + ISNULL(' ' + r.MiddleName,'') + ISNULL(' ' + r.LastName,'') AS PatientName, dbo.AgeInYrsMonthDay (Convert(varchar(10),DateofBirth,111), Convert(Varchar(10), Getdate(),111)) as Age, dbo.GetGender(Gender) as Gender,";
    //    strSql = strSql + " c.Race from Registration r WITH (NOLOCK) Left Join RaceMaster c WITH (NOLOCK) On r.RaceId = c.RaceId";
    //    strSql = strSql + " where r.Id = @intRegistrationId ";
    //    strSql = strSql + " Select IsNull(IsPregnant,0) as IsPregnant, IsNull(IsBreastFeeding,0) as IsBreastFeeding FROM Encounter WITH (NOLOCK) where Id = @intEncounterId";

    //    ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", hsProblems);
    //    ds2 = DlObj.FillDataSet(CommandType.Text, strSql, hsProblems);

    //    if (Convert.ToString(ds2.Tables[0].Rows[0]["Age"]).Trim() != "")
    //    {
    //        strAge = Convert.ToString(ds2.Tables[0].Rows[0]["Age"]).Trim();
    //    }
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        //if (common.myStr(sbTemplateStyle).Contains("Chief Complaints") == false)
    //        //{
    //        sb.Append(sbTemplateStyle + "<u><b>" + "</b></u>" + BeginList);
    //        //}
    //        if (drTemplateListStyle != null)
    //        {
    //            if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
    //            {
    //                BeginList = "<ul>"; EndList = "</ul>";
    //            }
    //            else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
    //            {
    //                BeginList = "<ol>"; EndList = "</ol>";
    //            }
    //        }
    //        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
    //        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId.ToString());

    //        DataView dvChronic = ds.Tables[0].DefaultView;
    //        dvChronic.RowFilter = "IsChronic = 'True'";
    //        dtChronic = dvChronic.ToTable();
    //        DataView dvNonChronic = ds.Tables[0].DefaultView;
    //        dvNonChronic.RowFilter = "IsChronic = 'False'";
    //        dtNonChronic = dvNonChronic.ToTable();
    //        if (dtChronic.Rows.Count > 0)
    //        {

    //            sbTemp.Append(sBeginFont);
    //            //sbTemp.Append("<br/>");
    //            sbTemp.Append("The patient, " + Convert.ToString(ds2.Tables[0].Rows[0]["PatientName"]).Trim() + ", is a ");
    //            sbTemp.Append(strAge + " old " + Convert.ToString(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " with a history of ");
    //            #region // Chronic problems starts here
    //            for (int i = 0; i < dtChronic.Rows.Count; i++)
    //            {
    //                DataRow dr1 = dtChronic.Rows[i] as DataRow;
    //                if (i == 0)
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                        sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtChronic.Rows.Count - 1)
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                        sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                        }
    //                    }
    //                }

    //                if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtChronic.Rows.Count - 1)
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtChronic.Rows.Count - 1)
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
    //                        sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtChronic.Rows.Count - 1)
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtChronic.Rows.Count - 1)
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().Trim() + ", ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
    //                {
    //                    if (i == dtChronic.Rows.Count - 1)
    //                    {
    //                        sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
    //                    }
    //                    else
    //                    {
    //                        sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
    //                    }
    //                }
    //            }

    //            #endregion // Chronic problems sentence ends here

    //            #region non chronic problem sentence start
    //            // Non Chronic problems starts here
    //            for (int i = 0; i < dtNonChronic.Rows.Count; i++)
    //            {
    //                //intRowCount = Convert.ToInt16( dtNonChronic.Rows.Count - 1 );
    //                DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
    //                if (i == 0)
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" who presents with ");
    //                    //sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    //
    //                    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                        sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                        }
    //                    }
    //                }
    //                else
    //                {

    //                    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                    {
    //                        sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                    }
    //                }

    //                if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
    //                        sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
    //                {
    //                    if (i == dtNonChronic.Rows.Count - 1)
    //                    {
    //                        sbTemp.Append(" and " + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
    //                    }
    //                    else
    //                    {
    //                        sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
    //                    }
    //                }
    //            }
    //            sbTemp.Append(sEndFont);
    //            #endregion //Non Chronic problems ends here.
    //        }
    //        else
    //        {
    //            #region if only non chronic problem then sentence start here
    //            sbTemp.Append(sBeginFont);
    //            // sbTemp.Append("<br/>");
    //            sbTemp.Append("The patient, " + Convert.ToString(ds2.Tables[0].Rows[0]["PatientName"]).Trim() + ", is a ");
    //            sbTemp.Append(strAge + "  old " + Convert.ToString(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + Convert.ToString(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " who presents with ");
    //            for (int i = 0; i < dtNonChronic.Rows.Count; i++)
    //            {
    //                DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
    //                if (i == 0)
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                        sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                else
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                    {
    //                        sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }

    //                if (Convert.ToString(dr1["AssociatedProblem1"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem2"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem3"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append("and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem4"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
    //                        sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if (i == dtNonChronic.Rows.Count - 1)
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["AssociatedProblem5"]).Trim() != "")
    //                {
    //                    if (i == dtNonChronic.Rows.Count - 1)
    //                    {
    //                        sbTemp.Append("and " + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
    //                    }
    //                    else
    //                        sbTemp.Append("" + Convert.ToString(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
    //                }
    //            }
    //            sbTemp.Append(sEndFont);
    //        }

    //            #endregion if non chronic problem sentence ends

    //        if (Convert.ToString(ds2.Tables[1].Rows[0]["IsPregnant"]) != "False")
    //            sbTemp.Append(sBeginFont + " The patient is or may be pregnant. " + sEndFont);
    //        if (Convert.ToString(ds2.Tables[1].Rows[0]["IsBreastFeeding"]) != "False")
    //            sbTemp.Append(sBeginFont + " The patient is breast feeding. " + sEndFont);

    //        //sbTemp.Append(BeginList); //cut from here 
    //        # region chronic problem listing
    //        for (int i = 0; i < dtChronic.Rows.Count; i++)
    //        {
    //            //sbTemp.Append(BeginList);// pasted here 
    //            //sbTemp.Append(sBegin + " The patient describes the chronic " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
    //            DataRow dr1 = dtChronic.Rows[i] as DataRow;
    //            if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Quality2"]) != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
    //            {
    //                //Changed onsetDate to onset
    //                //Added this line to append string after if condition,previously it is above the if condition
    //                //Vineet
    //                sbTemp.Append(sBegin + " The patient describes the chronic " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
    //                if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality2"]).Trim() != "")
    //                        sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                        {
    //                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality2"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality3"]).Trim() != "")
    //                        sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality3"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality4"]).Trim() != "")
    //                        sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]).Trim() == "") && (Convert.ToString(dr1["Severity"]).Trim() == ""))
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality4"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality5"]).Trim() != "")
    //                        sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]).Trim() == "") && (Convert.ToString(dr1["Severity"]).Trim() == ""))
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality5"]).Trim() != "")
    //                {
    //                    if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                    {
    //                        sbTemp.Remove(sbTemp.Length - 2, 2);
    //                        sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
    //                    }
    //                    else
    //                    {
    //                        sbTemp.Remove(sbTemp.Length - 2, 2);
    //                        sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
    //                    }
    //                }

    //                if (Convert.ToString(dr1["Location"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Severity"]).Trim() != "")
    //                    {
    //                        if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
    //                        {
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + ") ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
    //                    }
    //                    else
    //                    {
    //                        if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
    //                        {
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + "). ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Severity"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["OnsetDate"]).Trim() != "")
    //                        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
    //                    else
    //                    {
    //                        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
    //                    }
    //                }


    //                //sbTemp.Append("that began on [OnSet Date] ");  
    //                if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower() + " and " + Convert.ToString(dr1["Duration"]).ToLower() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptoms for the  last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
    //                }

    //                if (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
    //                {
    //                    if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
    //                        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
    //                    else
    //                        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times. ");
    //                }
    //                if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
    //                    sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");

    //            }

    //            //asdfg
    //            if (Convert.ToString(dr1["AggravatingFactors"]).Trim() != "")
    //                sbTemp.Append(" Aggravating factors are: " + Convert.ToString(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
    //            if (Convert.ToString(dr1["RelievingFactors"]).Trim() != "")
    //                sbTemp.Append(" Relieving factors are:  " + Convert.ToString(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
    //            if ((Convert.ToString(dr1["DeniesSymptoms1"]) != "") || (Convert.ToString(dr1["DeniesSymptoms2"]) != "") || (Convert.ToString(dr1["DeniesSymptoms3"]) != "") || (Convert.ToString(dr1["DeniesSymptoms4"]) != "") || (Convert.ToString(dr1["DeniesSymptoms5"]) != ""))
    //                sbTemp.Append(" Patient denies symptoms of ");
    //            if (Convert.ToString(dr1["DeniesSymptoms1"]) != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
    //            {
    //                sbTemp.Remove(sbTemp.Length - 2, 2);
    //                sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
    //            }

    //            if (Convert.ToString(dr1["Condition"]).Trim() != "")
    //            {
    //                sbTemp.Append("The patient's condition is " + Convert.ToString(dr1["Condition"]).ToLower().Trim());
    //                if (Convert.ToString(dr1["Percentage"]) != "0" && Convert.ToString(dr1["Percentage"]) != "")
    //                {
    //                    sbTemp.Append(" by " + Convert.ToString(dr1["Percentage"]).Trim() + "% .");
    //                }
    //                else
    //                {
    //                    sbTemp.Append(". ");
    //                }
    //            }

    //            sbTemp.Append(sEnd);
    //            //sbTemp.Append(EndList);// pasted here

    //        }
    //        #endregion

    //        # region nonchronic problem details with listing
    //        for (int i = 0; i < dtNonChronic.Rows.Count; i++)
    //        {
    //            //sbTemp.Append(BeginList);// pasted here   
    //            DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
    //            //sbTemp.Append("<br /><br />" + Convert.ToString(dr1["ProblemDescription"]) + " presents with ");
    //            //sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");


    //            if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
    //            {
    //                //Changed onsetDate to onset
    //                //Added this line to append string after if condition,previously it is above the if condition
    //                //Vineet
    //                sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
    //                if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality2"]).Trim() != "")
    //                        sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                        {
    //                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality2"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality3"]).Trim() != "")
    //                        sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality3"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality4"]).Trim() != "")
    //                        sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality4"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Quality5"]).Trim() != "")
    //                        sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
    //                    else
    //                    {
    //                        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
    //                        }
    //                        else
    //                        {
    //                            sbTemp.Remove(sbTemp.Length - 2, 2);
    //                            sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
    //                        }
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Quality5"]).Trim() != "")
    //                {
    //                    if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
    //                    {
    //                        sbTemp.Remove(sbTemp.Length - 2, 2);
    //                        sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
    //                    }
    //                    else
    //                    {
    //                        sbTemp.Remove(sbTemp.Length - 2, 2);
    //                        sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
    //                    }
    //                }

    //                if (Convert.ToString(dr1["Location"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Severity"]).Trim() != "")
    //                    {
    //                        if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
    //                        {
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + ") ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
    //                    }
    //                    else
    //                    {
    //                        if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
    //                        {
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + "). ");
    //                        }
    //                        else
    //                            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
    //                    }
    //                }
    //                if (Convert.ToString(dr1["Severity"]).Trim() != "")
    //                {
    //                    if (Convert.ToString(dr1["Onset"]).Trim() != "")
    //                        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
    //                }
    //                //sbTemp.Append("that began on [OnSet Date] "); 



    //                if (Convert.ToString(dr1["Onset"]).Trim() != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) == "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
    //                }
    //                else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
    //                {
    //                    if (i == 0)
    //                        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
    //                    else
    //                        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
    //                }

    //                if (Convert.ToString(dr1["NoOfOccurrence"]) != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
    //                {
    //                    if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
    //                        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
    //                    else
    //                        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times.");
    //                }
    //                if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
    //                    sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");
    //            }
    //            if (Convert.ToString(dr1["AggravatingFactors"]).Trim() != "")
    //                sbTemp.Append(" Aggravating factors are: " + Convert.ToString(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
    //            if (Convert.ToString(dr1["RelievingFactors"]).Trim() != "")
    //                sbTemp.Append(" Relieving factors are:  " + Convert.ToString(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
    //            if ((Convert.ToString(dr1["DeniesSymptoms1"]) != "") || (Convert.ToString(dr1["DeniesSymptoms2"]) != "") || (Convert.ToString(dr1["DeniesSymptoms3"]) != "") || (Convert.ToString(dr1["DeniesSymptoms4"]) != "") || (Convert.ToString(dr1["DeniesSymptoms5"]) != ""))
    //                sbTemp.Append(" Patient denies symptoms of ");
    //            if (Convert.ToString(dr1["DeniesSymptoms1"]) != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    //sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms2"]).Trim() != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms3"]) != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms4"]).Trim() != "")
    //            {
    //                if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
    //                {
    //                    sbTemp.Append(Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
    //                }
    //                else
    //                {
    //                    sbTemp.Remove(sbTemp.Length - 2, 2);
    //                    sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
    //                }
    //            }

    //            if (Convert.ToString(dr1["DeniesSymptoms5"]).Trim() != "")
    //            {
    //                sbTemp.Remove(sbTemp.Length - 2, 2);
    //                sbTemp.Append(" and " + Convert.ToString(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
    //            }
    //            if (Convert.ToString(dr1["Condition"]).Trim() != "")
    //            {
    //                sbTemp.Append("The patient's condition is " + Convert.ToString(dr1["Condition"]).ToLower().Trim());
    //                if (Convert.ToString(dr1["Percentage"]) != "0" && Convert.ToString(dr1["Percentage"]) != "")
    //                {
    //                    sbTemp.Append(" by " + Convert.ToString(dr1["Percentage"]).Trim() + "% .");
    //                }
    //                else
    //                {
    //                    sbTemp.Append(". ");
    //                }
    //            }
    //            sbTemp.Append(sEnd);
    //            //sbTemp.Append(EndList);// pasted here      
    //        }
    //        # endregion
    //    }
    //    //if (sbTemp.ToString() != "")
    //    //{
    //    if (ds.Tables[1].Rows.Count > 0)
    //    {
    //        if (ds.Tables[0].Rows.Count == 0)
    //        {
    //            if (common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() != "")
    //            {
    //                //  sb.Append(sbTemplateStyle + "<u><b>" + "Chief Complaints:" + "</b></u><br/>" + BeginList);
    //            }
    //        }
    //        if (sbTemplateStyle.ToString() == "")
    //        {
    //            sb.Append(sbTemplateStyle);
    //        }

    //        if (common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() != "")
    //        {
    //            sbTemp.Append(sBeginFont + common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() + sEndFont);
    //        }
    //    }
    //    sb.Append(sbTemp);
    //    return sb;
    //}


    public StringBuilder BindAssessments(int RegId, int HospitalId, int EncounterId, Int16 UserId, string DocId,
                        StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID, string fromDate, string toDate, int TemplateFieldId, string sEREncounterId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        DataSet dsDiagnosis = new DataSet();
        StringBuilder sbChronic = new StringBuilder();
        StringBuilder sbPrimary = new StringBuilder();
        StringBuilder sbSeconday = new StringBuilder();
        StringBuilder sbQuery = new StringBuilder();
        UInt16 pF = 0;
        UInt16 sF = 0;
        UInt16 cF = 0;
        UInt16 qF = 0;
        Hashtable hsAss = new Hashtable();
        hsAss.Add("@intRegistrationId", RegId);
        hsAss.Add("@inyHospitalLocationID", HospitalId);
        hsAss.Add("@intEncounterId", EncounterId);
        hsAss.Add("@intEREncounterId", sEREncounterId);
        if (fromDate != "" && toDate != "")
        {
            hsAss.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsAss.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }
        dsDiagnosis = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hsAss);
        DataView dv = new DataView(dsDiagnosis.Tables[0]);
        dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId;
        ds.Tables.Add(dv.ToTable());
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);

            if (drTemplateListStyle != null)
            {
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sbChronic.Append(BeginList);
            sbPrimary.Append(BeginList);
            sbSeconday.Append(BeginList);
            sbQuery.Append(BeginList);
        }
        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        foreach (DataRow dr in ds.Tables[0].Rows)
        {

            if (dr["IsChronic"].ToString() == "True")
            {
                cF = 1;
                if (dr["ICDCode"].ToString() != "")
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
                    else
                        sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
                }
                else
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
                    else
                        sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisType"]) != "")
                {
                    if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbChronic.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
                    else
                        sbChronic.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
                }

                if (Convert.ToString(dr["OnsetDate"]) != "")
                {
                    if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbChronic.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
                    else
                        sbChronic.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
                }
                if (Convert.ToString(dr["Location"]) != "")
                {
                    if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbChronic.Append(" Location : " + dr["Location"].ToString() + ", ");
                    else
                        sbChronic.Append(" Location : " + dr["Location"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
                    sbChronic.Append(" Condition : ");
                if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                        sbChronic.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbChronic.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                        sbChronic.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbChronic.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                {
                    sbChronic.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
                }

                if (Convert.ToString(dr["Remarks"]) != "")
                    sbChronic.Append("<br /> Remarks : " + dr["Remarks"].ToString() + ". ");
                sbChronic.Append(sEnd + "<br />");
            }
            else if (dr["PrimaryDiagnosis"].ToString() == "True" && dr["IsChronic"].ToString() == "False" && dr["IsQuery"].ToString() == "False")
            {
                pF = 1;
                if (dr["ICDCode"].ToString() != "")
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
                    else
                        sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
                }
                else
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
                    else
                        sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisType"]) != "")
                {
                    if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbPrimary.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
                    else
                        sbPrimary.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
                }

                if (Convert.ToString(dr["OnsetDate"]) != "")
                {
                    if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbPrimary.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
                    else
                        sbPrimary.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
                }
                if (Convert.ToString(dr["Location"]) != "")
                {
                    if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbPrimary.Append(" Location : " + dr["Location"].ToString() + ", ");
                    else
                        sbPrimary.Append(" Location : " + dr["Location"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
                    sbPrimary.Append(" Condition : ");
                if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                        sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                        sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                {
                    sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
                }

                if (Convert.ToString(dr["Remarks"]) != "")
                    sbPrimary.Append("<br /> Remarks : " + dr["Remarks"].ToString() + ". ");
                sbPrimary.Append(sEnd + "<br />");
            }
            else if (dr["PrimaryDiagnosis"].ToString() == "False" && dr["IsChronic"].ToString() == "False" && dr["IsQuery"].ToString() == "False")
            {
                sF = 1;
                if (dr["ICDCode"].ToString() != "")
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
                    else
                        sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
                }
                else
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
                    else
                        sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisType"]) != "")
                {
                    if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbSeconday.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
                    else
                        sbSeconday.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
                }

                if (Convert.ToString(dr["OnsetDate"]) != "")
                {
                    if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbSeconday.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
                    else
                        sbSeconday.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
                }
                if (Convert.ToString(dr["Location"]) != "")
                {
                    if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbSeconday.Append(" Location : " + dr["Location"].ToString() + ", ");
                    else
                        sbSeconday.Append(" Location : " + dr["Location"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
                    sbSeconday.Append(" Condition : ");
                if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                        sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                        sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                {
                    sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
                }

                if (Convert.ToString(dr["Remarks"]) != "")
                    sbSeconday.Append("<br /> Remarks : " + dr["Remarks"].ToString() + ". ");
                sbSeconday.Append(sEnd + "<br />");
            }
            else if (dr["PrimaryDiagnosis"].ToString() == "False" && dr["IsChronic"].ToString() == "False" && dr["IsQuery"].ToString() == "True")
            {
                qF = 1;
                if (dr["ICDCode"].ToString() != "")
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbQuery.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
                    else
                        sbQuery.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
                }
                else
                {
                    if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbQuery.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
                    else
                        sbQuery.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisType"]) != "")
                {
                    if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbQuery.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
                    else
                        sbQuery.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
                }

                if (Convert.ToString(dr["OnsetDate"]) != "")
                {
                    if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbQuery.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
                    else
                        sbQuery.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
                }
                if (Convert.ToString(dr["Location"]) != "")
                {
                    if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
                        sbQuery.Append(" Location : " + dr["Location"].ToString() + ", ");
                    else
                        sbQuery.Append(" Location : " + dr["Location"].ToString() + ". ");
                }

                if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
                    sbQuery.Append(" Condition : ");
                if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                        sbQuery.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbQuery.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
                {
                    if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                        sbQuery.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbQuery.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
                }
                if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
                {
                    sbQuery.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
                }

                if (Convert.ToString(dr["Remarks"]) != "")
                    sbQuery.Append("<br /> Remarks : " + dr["Remarks"].ToString() + ". ");
                sbQuery.Append(sEnd + "<br />");
            }
        }
        sBegin = "";
        sEnd = "";
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;
        if (cF == 1)
        {
            //sb.Append("<span>Chronic Diagnosis: </span><br />");
            sbChronic.Append(EndList);
            sb.Append(sbChronic.ToString());
        }
        if (pF == 1)
        {
            // sb.Append(" <span>Primary Diagnosis: </span><br />");
            sbPrimary.Append(EndList);
            sb.Append(sbPrimary.ToString());
        }
        if (sF == 1)
        {
            // sb.Append(" <span>Secondary Diagnosis: </span> <br />");
            sbSeconday.Append(EndList);
            sb.Append(sbSeconday.ToString());
        }
        if (qF == 1)
        {
            // sb.Append(" <span>Provisional Diagnosis: </span><br />");
            sbQuery.Append(EndList);
            sb.Append(sbQuery.ToString());
        }
        return sb;
    }



    public StringBuilder BindAllergies(int RegId, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string hospitalID, string userID, string PageID,
        string fromDate, string toDate, int TemplateFieldId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        DataSet dsAllergy = new DataSet();
        Hashtable hsAllergy = new Hashtable();
        StringBuilder sbDrugAllergy = new StringBuilder();
        StringBuilder sbOtherAllergy = new StringBuilder();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        int t = 0;
        hsAllergy.Add("@intRegistrationId", RegId);

        if (fromDate != "" && toDate != "")
        {
            hsAllergy.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsAllergy.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }

        dsAllergy = objtlc.GetDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", hsAllergy);
        objtlc = null;
        DataView dv = new DataView(dsAllergy.Tables[0]);
        ds.Tables.Add(dv.ToTable());
        DataView dvDrug = new DataView(ds.Tables[0]);
        dvDrug.RowFilter = "AllergyType = 'Drug'";

        DataView dvOther = new DataView(ds.Tables[0]);
        dvOther.RowFilter = "AllergyType <> 'Drug'";

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);

            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                sBeginSection = sBegin;
                sEndSection = sEnd;
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
        }
        if (dsAllergy.Tables[1].Rows.Count > 0)
        {
            if (Convert.ToString(dsAllergy.Tables[1].Rows[0]["NoAllergies"]) == "True")
            {
                // sb.Append(sbTemplateStyle + "<u><b>" + "Allergies: " + "</b></u>" + BeginList);
            }
            if (Convert.ToString(dsAllergy.Tables[1].Rows[0]["NoAllergies"]) != "True")
            {
                foreach (DataRowView dr in dvDrug)
                {
                    if (t == 0)
                    {
                        // sbDrugAllergy.Append("<br />" + sBeginSection + "Drug Allergies: <br />" + sEndSection + BeginList);
                        t = 1;
                    }
                    sBegin = ""; sEnd = "";
                    MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    sbDrugAllergy.Append(" " + sBegin + dr["AllergyName"].ToString() + " (Generic: " + dr["Generic_Name"].ToString() + ")");
                    if (Convert.ToString(dr["AllergyDate"]) != "")
                        sbDrugAllergy.Append(", Onset Date: " + dr["AllergyDate"].ToString());
                    if (Convert.ToString(dr["Reaction"]) != "")
                    {
                        if (Convert.ToString(dr["Intolerance"]) == "False" && (Convert.ToString(dr["Remarks"]) == ""))
                            sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + dr["Reaction"].ToString() + ".");
                        else
                            sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + dr["Reaction"].ToString());
                    }
                    if (dr["AllergySeverity"].ToString() != "")
                    {
                        sbDrugAllergy.Append(", Severity level : " + dr["AllergySeverity"].ToString());
                    }
                    if (Convert.ToString(dr["Intolerance"]) == "True")
                    {
                        if (Convert.ToString(dr["Remarks"]) == "")
                            sbDrugAllergy.Append(", Intolerable.");
                        else
                            sbDrugAllergy.Append(", Intolerable");
                    }
                    if (Convert.ToString(dr["Remarks"]) != "")
                        sbDrugAllergy.Append(", Remarks: " + Convert.ToString(dr["Remarks"]) + ".");

                    // sbDrugAllergy.Append(sEnd + "<br />");
                    sbDrugAllergy.Append(sEnd);
                }
                sbDrugAllergy.Append(EndList);
            }
            else
            {
                sbDrugAllergy.Append(sbTemplateStyle);
                //sbDrugAllergy.Append("<br />" + BeginList + sBegin + " No Allergies." + sEnd + EndList);
                sbDrugAllergy.Append(BeginList + sBegin + sEnd + EndList);
            }
        }
        t = 0;
        foreach (DataRowView dr in dvOther)
        {
            if (t == 0)
            {
                //sbOtherAllergy.Append("<br />" + sBeginSection + "Food/ Other Allergies: <br />" + sEndSection + BeginList);
                t = 1;
            }
            sBegin = ""; sEnd = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());

            sbOtherAllergy.Append(" " + sBegin + dr["AllergyName"].ToString());
            if (Convert.ToString(dr["AllergyDate"]) != "")
                sbOtherAllergy.Append(", Onset Date: " + dr["AllergyDate"].ToString());
            if (Convert.ToString(dr["Reaction"]) != "")
            {
                if (Convert.ToString(dr["Intolerance"]) == "False" && (Convert.ToString(dr["Remarks"]) == ""))
                    sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + dr["Reaction"].ToString() + ".");
                else
                    sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + dr["Reaction"].ToString());
            }
            if (Convert.ToString(dr["Intolerance"]) == "True")
            {
                if (Convert.ToString(dr["Remarks"]) == "")
                    sbOtherAllergy.Append(", Intolerable.");
                else
                    sbOtherAllergy.Append(", Intolerable");
            }
            if (Convert.ToString(dr["Remarks"]) != "")
                sbOtherAllergy.Append(", Remarks: " + Convert.ToString(dr["Remarks"]) + ".");
            // sbOtherAllergy.Append(sEnd + "<br />");
            sbOtherAllergy.Append(sEnd);
        }
        sbOtherAllergy.Append(EndList);
        sb.Append(sbDrugAllergy);
        sb.Append(sbOtherAllergy);
        return sb;
    }

    public StringBuilder BindMedication(int EncounterId, int HospitalId, int RegId, StringBuilder sb,
                    StringBuilder sbTemplateStyle, string MedicationType, DataRow drTemplateListStyle,
                    Page pg, string pageID, string userID, string fromDate, string toDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        Hashtable hsMed = new Hashtable();
        hsMed = new Hashtable();
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        DataTable dtCurrentMedication = new DataTable();
        DataTable dtPriscription = new DataTable();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sbAdministered = new StringBuilder();
        StringBuilder sbDispenced = new StringBuilder();
        StringBuilder sbCurrentMedication = new StringBuilder();
        StringBuilder sbCustomPrescription = new StringBuilder();
        StringBuilder sbCustomMedication = new StringBuilder();
        hsMed.Add("@inyHospitalLocationID", HospitalId);
        hsMed.Add("@intRegistrationId", RegId);
        hsMed.Add("@intEncounterId", EncounterId);

        if (fromDate != "" && toDate != "")
        {
            hsMed.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsMed.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrugHistory", hsMed);
        ds2 = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientCurrentMedication", hsMed);
        objtlc = null;
        dtCurrentMedication = ds2.Tables[0];
        dtPriscription = ds.Tables[0];
        if (drTemplateListStyle != null)
        {
            if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
            {
                BeginList = "<ul>"; EndList = "</ul>";
            }
            else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
            {
                BeginList = "<ol>"; EndList = "</ol>";
            }
        }
        sbPrescribed.Append(BeginList);
        sbAdministered.Append(BeginList);
        sbDispenced.Append(BeginList);
        sbCurrentMedication.Append(BeginList);
        sbCustomPrescription.Append(BeginList);
        sbCustomMedication.Append(BeginList);
        if (MedicationType == "P")
        {
            sBegin = ""; sEnd = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
            if (dtPriscription.Rows.Count > 0)
            {
                bool bSameResultP = false;
                bool bDiffResultP = false;
                bool bSameResultA = false;
                bool bDiffResultA = false;
                sb.Append(sbTemplateStyle); //sb.Append("<u>Prescription</u>");
                //sb.Append("<br />");
                HiddenField hdnEncodedBy = new HiddenField();
                DataView dv = new DataView(dtPriscription);
                dv.RowFilter = "PrescriptionMode ='P'";
                DataTable dt = dv.ToTable();
                hdnEncodedBy.Value = dt.Rows[0]["EncodedBy"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //hdnPrescriptionNo.Value = dt.Rows[i]["PrescriptionNo"].ToString();
                    DataView dvFilter = new DataView(dt);
                    dvFilter.RowFilter = "EncodedBy='" + hdnEncodedBy.Value + "'";
                    DataView dvPrescribed = dvFilter;
                    if (bSameResultP == false)
                    {
                        if (hdnEncodedBy.Value == dvPrescribed.ToTable().Rows[i]["EncodedBy"].ToString())
                        {
                            foreach (DataRowView dr in dvPrescribed)
                            {
                                string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                                string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                                sbPrescribed.Append(sBegin + " " + dr["Display_Name"].ToString() + ","
                                    //+ " " + Convert.ToInt32(dr["QtyAmount"])
                                    + " " + dr["Route_Description"].ToString()
                                    + " " + dr["Frequency_Description"].ToString());

                                if (Convert.ToString(dr["Days"]) != "")
                                {
                                    sbPrescribed.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                                }
                                if (dr["Strength"].ToString() != "" && dr["Strength"].ToString() != null)
                                {
                                    sbPrescribed.Append(", Strength: " + dr["Strength"].ToString().Trim());
                                }
                                if (dr["Dose"].ToString() != "" && dr["Dose"].ToString() != null)
                                {
                                    sbPrescribed.Append(", Dose: " + dr["Dose"].ToString());
                                }
                                if (refill != "0")
                                {
                                    sbPrescribed.Append(", Refill:" + refill);
                                }
                                if (PRN == "PRN")
                                {
                                    sbPrescribed.Append(", (<b>PRN</b>).");
                                }
                                if (dr["Remarks"].ToString().Trim() != "")
                                {
                                    sbPrescribed.Append(" Remarks: " + dr["Remarks"].ToString());
                                }
                                sbPrescribed.Append(sEnd + "<br />");
                                hdnEncodedBy.Value = dr["EncodedBy"].ToString();
                                bSameResultP = true;
                            }
                            if (bSameResultP == true)
                            {
                                sbPrescribed.Append(DisplayUserNameInNote(dvPrescribed.ToTable().Rows[0]["EncodedBy"].ToString(), HospitalId, dvPrescribed.ToTable().Rows[0]["EntryDate"].ToString(), drTemplateListStyle, pg));
                                //bResult = false;
                            }

                        }
                    }
                    else if (hdnEncodedBy.Value != dt.Rows[i]["EncodedBy"].ToString())
                    {
                        DataTable dt3 = dv.ToTable();
                        DataView dv3 = new DataView(dt3);

                        dv3.RowFilter = "EncodedBy='" + dt.Rows[i]["EncodedBy"].ToString() + "'";
                        DataView dvPrescribed1 = dv3;
                        foreach (DataRowView dr in dvPrescribed1)
                        {
                            string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                            string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                            sbPrescribed.Append(sBegin + " " + dr["Display_Name"].ToString() + ","
                                //+ " " + Convert.ToInt32(dr["QtyAmount"])
                                + " " + dr["Route_Description"].ToString()
                                + " " + dr["Frequency_Description"].ToString());

                            if (Convert.ToString(dr["Days"]) != "")
                            {
                                sbPrescribed.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                            }
                            if (dr["Strength"].ToString() != "" && dr["Strength"].ToString() != null)
                            {
                                sbPrescribed.Append(", Strength: " + dr["Strength"].ToString().Trim());
                            }
                            if (dr["Dose"].ToString() != "" && dr["Dose"].ToString() != null)
                            {
                                sbPrescribed.Append(", Dose: " + dr["Dose"].ToString());
                            }
                            if (refill != "0")
                            {
                                sbPrescribed.Append(", Refill:" + refill);
                            }
                            if (PRN == "PRN")
                            {
                                sbPrescribed.Append(", (<b>PRN</b>).");
                            }
                            if (dr["Remarks"].ToString().Trim() != "")
                            {
                                sbPrescribed.Append(" Remarks: " + dr["Remarks"].ToString());
                            }
                            sbPrescribed.Append(sEnd + "<br />");
                            hdnEncodedBy.Value = dr["EncodedBy"].ToString();
                            bDiffResultP = true;
                        }
                        if (bDiffResultP == true)
                        {
                            sbPrescribed.Append(DisplayUserNameInNote(dvPrescribed1.ToTable().Rows[0]["EncodedBy"].ToString(), HospitalId, dvPrescribed1.ToTable().Rows[0]["EntryDate"].ToString(), drTemplateListStyle, pg));
                        }

                    }
                }

                DataView dv1 = new DataView(dtPriscription);
                dv1.RowFilter = "PrescriptionMode ='A'";
                DataTable dt1 = dv1.ToTable();
                if (dt1.Rows.Count > 0)
                {
                    hdnEncodedBy.Value = dt1.Rows[0]["EncodedBy"].ToString();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        DataView dvResult = new DataView(dt1);
                        dvResult.RowFilter = "EncodedBy='" + hdnEncodedBy.Value + "'";
                        DataView dvAdministered = dvResult;
                        if (bSameResultA == false)
                        {
                            if (hdnEncodedBy.Value == dt1.Rows[i]["EncodedBy"].ToString())
                            {
                                foreach (DataRowView dr in dvAdministered)
                                {
                                    string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                                    string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                                    sbAdministered.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"

                                       + " " + dr["Route_Description"].ToString()
                                       + " " + dr["Frequency_Description"].ToString());
                                    if (Convert.ToString(dr["Days"]) != "")
                                    {
                                        sbAdministered.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                                    }
                                    if (dr["Strength"].ToString() != "" && dr["Strength"].ToString() != null)
                                    {
                                        sbAdministered.Append(", Strength: " + dr["Strength"].ToString().Trim());
                                    }
                                    if (dr["Dose"].ToString() != "" && dr["Dose"].ToString() != null)
                                    {
                                        sbAdministered.Append(", Dose: " + dr["Dose"].ToString());
                                    }
                                    if (refill != "0")
                                    {
                                        sbAdministered.Append(", Refill:" + refill);
                                    }
                                    if (PRN == "PRN")
                                    {
                                        sbAdministered.Append(", (<b>PRN</b>).");
                                    }
                                    if (dr["Remarks"].ToString().Trim() != "")
                                    {
                                        sbAdministered.Append(" Remarks: " + dr["Remarks"].ToString());
                                    }
                                    sbAdministered.Append(sEnd + "<br />");
                                    hdnEncodedBy.Value = dr["EncodedBy"].ToString();
                                    bSameResultA = true;
                                }
                                if (bSameResultA == true)
                                {
                                    sbAdministered.Append(DisplayUserNameInNote(dvAdministered.ToTable().Rows[0]["EncodedBy"].ToString(), HospitalId, dvAdministered.ToTable().Rows[0]["EntryDate"].ToString(), drTemplateListStyle, pg));
                                }
                            }
                        }
                        else if (hdnEncodedBy.Value != dt1.Rows[i]["EncodedBy"].ToString())
                        {
                            DataView dvResult1 = new DataView(dt1);
                            dvResult1.RowFilter = "EncodedBy='" + dt1.Rows[i]["EncodedBy"].ToString() + "'";
                            // dv1.RowFilter = "EncodedBy='" + dt1.Rows[i]["EncodedBy"].ToString() + "'";
                            DataView dvAdministered1 = dvResult1;
                            foreach (DataRowView dr in dvAdministered1)
                            {
                                string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                                string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                                sbAdministered.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"

                                   + " " + dr["Route_Description"].ToString()
                                   + " " + dr["Frequency_Description"].ToString());
                                if (Convert.ToString(dr["Days"]) != "")
                                {
                                    sbAdministered.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                                }
                                if (dr["Strength"].ToString() != "" && dr["Strength"].ToString() != null)
                                {
                                    sbAdministered.Append(", Strength: " + dr["Strength"].ToString().Trim());
                                }
                                if (dr["Dose"].ToString() != "" && dr["Dose"].ToString() != null)
                                {
                                    sbAdministered.Append(", Dose: " + dr["Dose"].ToString());
                                }
                                if (refill != "0")
                                {
                                    sbAdministered.Append(", Refill:" + refill);
                                }
                                if (PRN == "PRN")
                                {
                                    sbAdministered.Append(", (<b>PRN</b>).");
                                }
                                if (dr["Remarks"].ToString().Trim() != "")
                                {
                                    sbAdministered.Append(" Remarks: " + dr["Remarks"].ToString());
                                }
                                sbAdministered.Append(sEnd + "<br />");
                                hdnEncodedBy.Value = dr["EncodedBy"].ToString();
                                bDiffResultA = true;
                            }
                            if (bDiffResultA == true)
                            {
                                sbAdministered.Append(DisplayUserNameInNote(dvAdministered1.ToTable().Rows[0]["EncodedBy"].ToString(), HospitalId, dvAdministered1.ToTable().Rows[0]["EntryDate"].ToString(), drTemplateListStyle, pg));
                            }
                        }
                    }
                }
                DataView dvDispenced = new DataView(ds.Tables[0]);
                dvDispenced.RowFilter = "PrescriptionMode ='D'";
                foreach (DataRowView dr in dvDispenced)
                {
                    string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                    string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                    sbDispenced.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"
                        + " " + Convert.ToString(dr["QtyAmount"])
                       + " " + dr["Route_Description"].ToString()
                       + " " + dr["Frequency_Description"].ToString());
                    if (Convert.ToString(dr["Days"]) != "")
                        sbDispenced.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                    else
                        sbDispenced.Append(", #" + Convert.ToString(dr["DispenseAmount"]));


                    sbPrescribed.Append(", Refill:" + refill + ".");//+ sEnd

                    sbPrescribed.Append("<br />" + sEnd + "<br />");
                    //sb.Append(med + "</li>");
                }
                sBegin = ""; sEnd = "";
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                sBeginSection = sBegin;
                sEndSection = sEnd;

                if (dv.Count > 0)
                {
                    if (common.myStr(sbTemplateStyle).Contains("Prescription") == false)
                    {
                        sb.Append(sbTemplateStyle + "<u><b>" + "Prescription:" + "</b></u>" + BeginList);
                    }
                    sb.Append("<br />" + sBeginSection + "<u><b>" + "Medications Prescribed:<br/>" + "</b></u>" + sEndSection);
                    sbPrescribed.Append(EndList);
                    int count = 0;
                    foreach (DataRowView dr in dv)
                    {
                        if (dr["GeneralRemarks"].ToString().Trim() != "")
                        {
                            if (count == 0)
                            {
                                sbPrescribed.Append("Remarks: ");
                            }
                            sbPrescribed.Append(dr["GeneralRemarks"].ToString() + ".<br/>");
                            count++;
                        }
                    }
                    sb.Append(sbPrescribed.ToString());
                }
                if (dv1.Count > 0)
                {
                    if (dv.Count == 0)
                    {
                        sb.Append(sbTemplateStyle + "<u><b>" + "Prescription:" + "</b></u>" + BeginList);
                    }
                    sb.Append("<br />" + sBeginSection + "<u><b>" + "Medications Administered:<br/>" + "</b></u>" + sEndSection);
                    sbAdministered.Append(EndList);
                    int count = 0;
                    foreach (DataRowView dr in dv1)
                    {
                        if (dr["GeneralRemarks"].ToString().Trim() != "")
                        {
                            if (count == 0)
                            {
                                sbAdministered.Append("Remarks: ");
                            }
                            sbAdministered.Append(dr["GeneralRemarks"].ToString() + ".<br/>");
                            count++;
                        }
                    }
                    sb.Append(sbAdministered.ToString());
                }
                if (dvDispenced.Count > 0)
                {
                    sb.Append(sbTemplateStyle + "<u><b>" + "Prescription:" + "</b></u><br />" + BeginList);
                    //sb.Append("<br />" + sBeginSection + "Medications Dispenced:" + sEndSection);
                    sbDispenced.Append(EndList);
                    sb.Append(sbDispenced.ToString());
                }

            }
        }
        else
        {
            // For Current medication
            sBegin = ""; sEnd = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
            if (dtCurrentMedication.Rows.Count > 0)
            {
                sb.Append(sbTemplateStyle); //sb.Append("<u>Prescription</u>");
                if (sBegin.Contains("<li>") != true)
                {
                    sbCurrentMedication.Append("<br />");
                }
                foreach (DataRow dr in dtCurrentMedication.Rows)
                {
                    if (dr["IsPrescription"].ToString() == "")
                    {
                        //string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                        string PRN = dr["PRN"].ToString() == "True" ? "" : "";
                        sbCurrentMedication.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"
                            + " " + Convert.ToString(dr["QtyAmount"])
                           + " " + dr["Route_Description"].ToString()
                           + " " + dr["Frequency_Description"].ToString());
                        //if (PRN != "")
                        // sbAdministered.Append(", " + PRN);
                        // if (Convert.ToString(dr["unit_abbreviation"]) != "")
                        // sbAdministered.Append(", " + Convert.ToString(dr["unit_abbreviation"]));

                        if (Convert.ToString(dr["Days"]) != "")
                            sbCurrentMedication.Append(" for " + Convert.ToString(dr["Days"]) + " days.");
                        //else
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["DispenceAmount"]));
                        //if (PRN != "")
                        // sbAdministered.Append(", " + PRN);
                        // if (Convert.ToString(dr["unit_abbreviation"]) != "")
                        // sbAdministered.Append(", " + Convert.ToString(dr["unit_abbreviation"]));

                        //if (Convert.ToString(dr["Days"]) != "")
                        //    sbCurrentMedication.Append("for " + Convert.ToString(dr["Days"]));
                        //else
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["DispenceAmount"]));


                        //    + ", " + dr["Route_Description"].ToString() + ", " + dr["Doseform_Description"].ToString() + ", "
                        //    + dr["Frequency_Description"].ToString());
                        //if (PRN != "")
                        //    sbCurrentMedication.Append(", " + PRN);
                        //if (Convert.ToString(dr["unit_abbreviation"]) != "")
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["unit_abbreviation"]));
                        //if (Convert.ToString(dr["QtyAmount"]) != "")
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["QtyAmount"]));
                        //if (Convert.ToString(dr["Days"]) != "")
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["Days"]));
                        //if (Convert.ToString(dr["StartDate"]) != "")
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["StartDate"]));
                        //if (Convert.ToString(dr["Comments"]) != "")
                        //    sbCurrentMedication.Append(", " + Convert.ToString(dr["Comments"]));

                        sbCurrentMedication.Append(sEnd);
                    }
                    else
                    {
                        sbCurrentMedication.Append(sBegin + " " + dr["Display_Name"].ToString());// + ":"
                        sbCurrentMedication.Append(sEnd);
                    }
                }
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());

                sb.Append("<br />" + sBeginFont + "Current Medication: " + sEndFont);
                sbCurrentMedication.Append(EndList);
                sb.Append(sbCurrentMedication.ToString());
            }
            else
            {
                Hashtable htbl = new Hashtable();
                htbl.Add("@intEncounterId", common.myInt(EncounterId));

                string Squery = "SELECT ISNULL(NoCurrentMedication,0) AS NoCurrentMedication FROM Encounter WITH (NOLOCK) WHERE Id=@intEncounterId ";

                DataSet dsNoMed = objtlc.GetDataSet(CommandType.Text, Squery, htbl);

                if (Convert.ToString(dsNoMed.Tables[0].Rows[0]["NoCurrentMedication"]) == "True")
                {
                    sb.Append(sbTemplateStyle);
                    sb.Append(BeginList + sBegin + "</br> None." + sEnd + EndList);
                }
            }
        }
        //sb.Append("</ul>");
        objtlc = null;
        return sb;
    }

    public StringBuilder BindOrders(Int64 RegNo, int HospitalId, int EncounterId, Int16 UserId, string DocId,
                            StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string userID, string fromDate, string toDate, string sEREncounterId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        Hashtable hsOrd = new Hashtable();
        hsOrd.Add("@intRegistrationId", RegNo);
        hsOrd.Add("@inyHospitalLocationID", HospitalId);
        hsOrd.Add("@intEncounterId", EncounterId);
        hsOrd.Add("@intEREncounterId", sEREncounterId);

        if (fromDate != "" && toDate != "")
        {
            hsOrd.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd")); //yyyy-MM-dd 00:00
            hsOrd.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }

        StringBuilder sbTemp = new StringBuilder();//UspEMRGetPreviousInvestigations
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hsOrd);
        DataView dvmain = new DataView(ds.Tables[0]);
        dvmain.RowFilter = "ServiceType Not in ( 'CL','VS','VF','R','RT','RF' )";
        dvmain.Sort = "DepartmentId,Id";
        DataTable dtmain = new DataTable();
        dtmain = dvmain.ToTable();
        HiddenField hdnEncodedBy = new HiddenField();
        if (dtmain.Rows.Count > 0)
        {
            //if (common.myStr(sbTemplateStyle).Contains("Order") == false)
            //{
            //    sb.Append(sbTemplateStyle + "<u><b>" + "Order And Procedures:" + "<br/></b></u>" + BeginList);
            //}
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (drTemplateListStyle["FieldsListStyle"].ToString() == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (drTemplateListStyle["FieldsListStyle"].ToString() == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            //sb.Append("<u>Orders And Procedures</u>");               
        }
        int DepartmentId = 0;
        hdnEncodedBy.Value = "0";
        HiddenField hdnSameEncodedBy = new HiddenField();
        HiddenField hdnSameEncodedDate = new HiddenField();
        DataTable dt = new DataTable();
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;
        StringBuilder sbSameName = new StringBuilder();
        StringBuilder sbDiffName = new StringBuilder();
        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        bool bResult = false;
        for (int i = 0; i < dtmain.Rows.Count; i++)
        {
            DataView dvEncoded = new DataView(dtmain);
            dvEncoded.RowFilter = "EncodedBy=" + Convert.ToInt32(dtmain.Rows[i]["EncodedBy"]);
            DataTable dtEncoded = dvEncoded.ToTable();
            if (dtEncoded.Rows.Count == dtmain.Rows.Count)
            {
                bResult = true;
                hdnSameEncodedBy.Value = dtmain.Rows[i]["EncodedBy"].ToString();
                hdnSameEncodedDate.Value = dtmain.Rows[i]["EntryDate"].ToString();
            }
            DataRow dr = dtmain.Rows[i] as DataRow;
            if (common.myInt(dr["DepartmentId"]) != DepartmentId)
            {
                sbTemp.Append(sBeginSection + common.myStr(dr["DepartmentName"]) + ": " + sEndSection);
                sbTemp.Append(BeginList);
                DepartmentId = Convert.ToInt32(dr["DepartmentId"].ToString());
                DataView dv = new DataView(dtmain);
                dv.RowFilter = "DepartmentId =" + Convert.ToInt32(dr["DepartmentId"]);
                dt = dv.ToTable();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string commase = "";
                    if (j != dt.Rows.Count - 1)
                    {
                        commase = ", ";
                    }

                    DataRow dr1 = dt.Rows[j] as DataRow;
                    if (dr["CPTCode"].ToString() != "")
                    {
                        sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + " (CPT Code:" + common.myStr(dr1["CPTCode"]) + ")" + commase + sEnd);
                    }
                    else
                    {
                        sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);
                    }
                }
                //sbTemp.Append(EndList + "<br/>");
                sbTemp.Append(EndList);
                if (bResult == false)
                {
                    // sbTemp.Append(EndList + "<br/>");
                    sbTemp.Append(EndList);
                    sbTemp.Append(DisplayUserNameInNote(dr["EncodedBy"].ToString(), HospitalId, dr["EntryDate"].ToString(), drTemplateListStyle, pg));
                }
            }
        }
        if (bResult == true)
        {
            //sbTemp.Append(EndList + "<br/>");
            sbTemp.Append(EndList);
            sbTemp.Append(DisplayUserNameInNote(hdnSameEncodedBy.Value, HospitalId, hdnSameEncodedDate.Value, drTemplateListStyle, pg));
        }

        sbTemp.Append("<br/>");
        DataTable dtEx = dvmain.ToTable();
        DataView dvExcluded = new DataView(dtEx);
        dvExcluded.RowFilter = "ExcludedServices=1";
        DataTable dtExcluded = dvExcluded.ToTable();
        if (dtExcluded.Rows.Count > 0)
        {
            sbTemp.Append("<strong>Approval required for following services :</strong><br/>");
            for (int j = 0; j < dtExcluded.Rows.Count; j++)
            {
                string commase = "";
                if (j != dtExcluded.Rows.Count - 1)
                {
                    commase = ", ";
                }

                DataRow dr1 = dtExcluded.Rows[j] as DataRow;
                sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);
            }
        }
        sb.Append(sbTemp.ToString());
        objtlc = null;
        return sb;
    }
    //public DataSet GetLabTestResultForNote(int iEncounterId, int HospId, int FacilityId, string strSampleId)
    //{
    //    Hashtable HshIn = new Hashtable();
    //    Hashtable HshOut = new Hashtable();
    //BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;

    //    HshIn.Add("@inyHospitalLocationId", HospId);
    //    HshIn.Add("@intLoginFacilityId", FacilityId);
    //    HshIn.Add("@intEncounterId", iEncounterId);
    //    HshIn.Add("@xmlDiagSampleIds", strSampleId);
    //    string sp = "uspDiagLabTestResultForNoteIP";

    //    DataSet ds = new DataSet();
    //    ds = objtlc.GetDataSet(CommandType.StoredProcedure, sp, HshIn, HshOut);
    //    return ds;
    //}
    public DataSet GetPrintDischargeSummary(int iEncounterId, int HospId, int RegistrationID, int FormatId, int FacilityId)
    {
        return GetPrintDischargeSummary(iEncounterId, HospId, RegistrationID, FormatId, FacilityId, 0, string.Empty);
    }
    public DataSet GetPrintDischargeSummary(int iEncounterId, int HospId, int RegistrationID, int FormatId, int FacilityId, int SummaryId, string Type)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;

        HshIn.Add("@inyHospitalLocationID", HospId);
        HshIn.Add("@intRegistrationID", RegistrationID);
        HshIn.Add("@intEncounterID", iEncounterId);
        HshIn.Add("@intReportId", FormatId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intSummaryId", SummaryId);
        HshIn.Add("@Type", Type);

        string sp = "UspGetPatientDischargeSummary";

        DataSet ds = new DataSet();
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, sp, HshIn, HshOut);

        return ds;
    }

    public StringBuilder BindLabTestResult(int HospitalId, int EncounterId, int FacilityId, int LoginFacilityId, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string SampleId, string DischargeSummary, bool bShowAllParameters)
    {
        return BindLabTestResult(HospitalId, EncounterId, FacilityId, LoginFacilityId, sb, sbTemplateStyle,
        drTemplateListStyle, pg, SampleId, DischargeSummary, bShowAllParameters, false, string.Empty);
    }
    public StringBuilder BindLabTestResult(int HospitalId, int EncounterId, int FacilityId, int LoginFacilityId, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string SampleId, string DischargeSummary, bool bShowAllParameters, bool PrintReferenceRangeInHTML, string summaryType)
    {

        string FontName = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "DischargeSummaryFont", sConString);
        if (common.myStr(FontName).Equals(string.Empty))
        {
            FontName = "Candara";
        }
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        BaseC.clsLISLabOther lis = new BaseC.clsLISLabOther(sConString);
        BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        HshIn.Add("@inyHospitalLocationId", common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        HshIn.Add("@intLoginFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        HshIn.Add("@intEncounterId", EncounterId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@xmlDiagSampleIds", SampleId);
        HshIn.Add("@chvDischargeSummary", DischargeSummary);
        //HshIn.Add("@bitShowAllParameters", bShowAllParameters); 
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultForNoteIP", HshIn, HshOut);

        objtlc = null;

        StringBuilder sbTemp = new StringBuilder();
        sBegin = ""; sEnd = "";
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]), FontName);

        string border = summaryType.Equals("HC") ? "0" : "1";

        if (ds.Tables.Count > 0)
        {
            int subDeptId = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                sbTemp.Append("<table border='" + border + "' width='100%' cellpadding='0' cellspacing='0' style='font-family:Arial; font-size:10pt;'>");

                if (common.myStr(ds.Tables[0].Rows[0]["LabType"]).ToUpper().Trim() != "X"
                    && common.myStr(DischargeSummary).Equals("D"))
                {
                    sbTemp.Append("<tr valign ='top'>");
                    sbTemp.Append("<td style='width: 60%; border:" + border + "px solid black;'><b>" + sBegin + "Test" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 10%; border:" + border + "px solid black;'><b>" + sBegin + "Value" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 10%; border:" + border + "px solid black;'><b>" + sBegin + "Unit" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 20%; border:" + border + "px solid black;'><b>" + sBegin + "Reference Range" + sEnd + "</b></td>");
                    sbTemp.Append("</tr>");
                }
                else
                {
                    sbTemp.Append("<tr valign ='top'>");
                    sbTemp.Append("<td style='width: 60%;border:0px;'>&nbsp;</td>");
                    sbTemp.Append("<td style='width: 10%;border:0px;'>&nbsp;</td>");
                    sbTemp.Append("<td style='width: 10%;border:0px;'>&nbsp;</td>");
                    sbTemp.Append("<td style='width: 20%;border:0px;'>&nbsp;</td>");
                    sbTemp.Append("</tr>");
                }

                //if(!common.myBool(PrintReferenceRangeInHTML))
                //{
                //    sbTemp = new StringBuilder();
                //    sbTemp.Append("<table border='"+ border + "' width='100%' cellpadding='0' cellspacing='0'  style='font-size:small;'>");
                //    sbTemp.Append("<tr valign ='top'><td colspan='4'></td></tr>");
                //}
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "<b>" : "";
                    string eBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "</b>" : "";

                    string sDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "<b>" : "";
                    string eDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "</b>" : "";

                    sbTemp.Append(BeginList);
                    sbTemp.Append("<tr valign='top'>");
                    string FieldType = common.myStr(dt.Rows[i]["FieldType"]).ToUpper().Trim();
                    string Symbol = common.myStr(dt.Rows[i]["Symbol"]).ToUpper().Trim();
                    string LimitType = common.myStr(dt.Rows[i]["LimitType"]).ToUpper().Trim();

                    //sbTemp.Append("<td colspan='4'><b>" + dt.Rows[i]["FieldName"].ToString() + "</b></td>");

                    if (FieldType == "SN" || FieldType == "W" || FieldType == "H")
                    {
                        if (summaryType.Equals("HC"))
                        {
                            sbTemp.Append("<td colspan='4' style='border:" + border + "px solid black;'><span style='color: #ffffff;'>.</span></td></tr>");

                            sbTemp.Append("<tr valign='top'>");

                            if (common.myInt(dt.Rows[i]["SequenceNo"]).Equals(1)
                                && common.myLen(dt.Rows[i]["FieldName"]) > 0 && !common.myStr(dt.Rows[i]["FieldName"]).Equals("&nbsp;")
                                && common.myLen(dt.Rows[i]["ServiceName"]) > 0 && !common.myStr(dt.Rows[i]["ServiceName"]).Equals("&nbsp;"))
                            {
                                sbTemp.Append("<td colspan='4' style='border:" + border + "px solid black;'>" + sBegin + "<u><b>" + common.myStr(dt.Rows[i]["ServiceName"]) + "</b></u>:" + sEnd + "</td></tr>");

                                sbTemp.Append("<tr valign='top'>");
                            }
                        }

                        DataSet dsSen = new DataSet();
                        sbTemp.Append("<td colspan='4' style='border:" + border + "px solid black;'><b>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + "</b></td>");
                        if (FieldType == "SN")
                        {
                            dsSen = lis.GetDiagLabSensitivityResultForNote(common.myInt(dt.Rows[i]["DiagSampleId"]), common.myInt(dt.Rows[i]["ResultId"]), common.myStr(System.Web.HttpContext.Current.Session["OPIP"]));
                            if (dsSen.Tables[0].Rows.Count > 0)
                            {
                                sbTemp.Append("</tr><tr valign ='top'>");
                                sbTemp.Append("<td colspan='4' style='border:" + border + "px solid black;'>");
                                sbTemp.Append("<table border='0' width='100%' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;font-family:Arial; font-size:10pt;'>");
                                sbTemp.Append("<tr>");
                                int count = 0;
                                for (int l = 0; l < dsSen.Tables[0].Columns.Count; l++)
                                {
                                    sbTemp.Append("<td width='20%' style='border:" + border + "px solid black;'><b>" + sBegin + dsSen.Tables[0].Columns[l].ColumnName + sEnd + "</b></td>");
                                    count++;
                                }
                                sbTemp.Append("</tr>");

                                for (int k = 0; k < dsSen.Tables[0].Rows.Count; k++)
                                {
                                    sbTemp.Append("<tr valign ='top'>");
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName]).Contains("<B>") == false
                                            && common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName]) != null)
                                        {
                                            sbTemp.Append("<td width='20%' style='border:" + border + "px solid black;'>" + sBegin + common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName]) + sEnd + "</td>");
                                        }
                                        else
                                        {
                                            sbTemp.Append("<td width='20%' colspan='" + count + "' style='border:" + border + "px solid black;' >" + sBegin + common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[0].ColumnName]) + sEnd + "</td>");
                                            break;
                                        }
                                    }

                                    sbTemp.Append("</tr>");
                                }
                                sbTemp.Append("</table>");
                                sbTemp.Append("</td>");
                            }
                        }
                        else
                        {
                            if (FieldType != "H")
                            {
                                sbTemp.Append("</tr>");
                                if (FieldType == "W")
                                {
                                    sbTemp.Append("<tr><td colspan='4' style='width: 60%;'>&nbsp;</td></tr>");
                                }
                                sbTemp.Append("<tr valign ='top'>");
                                sbTemp.Append("<td colspan='4' style='width: 60%;'>" + sBegin + System.Web.HttpContext.Current.Server.HtmlDecode(common.myStr(dt.Rows[i]["Result"]).Replace("&lt;", "LLLLL").Replace("&gt;", "GGGGG")).Replace("LLLLL", "&lt;").Replace("GGGGG", "&gt;") + sEnd + "</td>");
                                //sbTemp.Append("<td colspan='4' style='width: 60%; border:"+ border + "px solid black;'>" + sBegin + System.Web.HttpContext.Current.Server.HtmlDecode(dt.Rows[i]["Result"].ToString()) + sEnd + "</td>");
                            }
                        }
                    }
                    else
                    {
                        if (summaryType.Equals("HC"))
                        {
                            if (common.myInt(dt.Rows[i]["SequenceNo"]).Equals(1)
                                && common.myLen(dt.Rows[i]["FieldName"]) > 0 && !common.myStr(dt.Rows[i]["FieldName"]).Equals("&nbsp;")
                                && common.myLen(dt.Rows[i]["ServiceName"]) > 0 && !common.myStr(dt.Rows[i]["ServiceName"]).Equals("&nbsp;"))
                            {
                                sbTemp.Append("<td colspan='4' style='border:" + border + "px solid black;'>" + sBegin + "<u><b>" + common.myStr(dt.Rows[i]["ServiceName"]) + "</b></u>:" + sEnd + "</td></tr>");

                                sbTemp.Append("<tr valign='top'>");
                            }
                        }

                        if (FieldType == "N" || FieldType == "F" || FieldType == "TM")
                        {
                            if ((Symbol == "<" || Symbol == ">") && LimitType != "SR")
                            {
                                if (Symbol == "<")
                                {
                                    Symbol = "&lt;";
                                }
                                else if (Symbol == ">")
                                {
                                    Symbol = "&gt;";
                                }
                                sbTemp.Append("<td valign='top' style='width: 60%; border:" + border + "px solid black;'>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + "</td><td valign='top' style='width: 10%; border:" + border + "px solid black;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td valign='top' style='width: 10%; border:" + border + "px solid black;'> " + sBegin + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + "</td><td valign='top' style='width: 20%; border:" + border + "px solid black;'> " + sBegin + common.myStr(Symbol) + " " + common.myStr(dt.Rows[i]["MaxValue"]) + sEnd + " </td>");
                            }
                            else if (LimitType == "SR")
                            {
                                sbTemp.Append("<td valign='top' style='width: 60%; border:" + border + "px solid black;'>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + "</td><td valign='top' style='width: 10%; border:" + border + "px solid black;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td valign='top' style='width: 10%; border:" + border + "px solid black;'> " + sBegin + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + "</td><td valign='top' style='width: 20%; border:" + border + "px solid black;'>" + sBegin + "(" + common.myStr(Symbol) + " " + common.myStr(dt.Rows[i]["SpecialReferenceRange"]) + ")" + sEnd + "</td>");
                            }
                            else
                            {
                                sbTemp.Append("<td valign='top' style='width: 60%; border:" + border + "px solid black;'>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + "</td><td valign='top' style='width: 10%; border:" + border + "px solid black;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td valign='top' style='width: 10%; border:" + border + "px solid black;'> " + sBegin + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + "</td><td valign='top' style='width: 20%; border:" + border + "px solid black;'>" + sBegin + "(" + common.myStr(dt.Rows[i]["MinValue"]) + " - " + common.myStr(dt.Rows[i]["MaxValue"]) + ")" + sEnd + "</td>");
                            }
                        }
                        else if (FieldType == "T" || FieldType == "M" || FieldType == "O" || FieldType == "E")
                        {
                            sbTemp.Append("<td align='left' valign='top' style='border:" + border + "px solid black;'>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + "</td><td colspan='3' style='border:" + border + "px solid black;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td>");
                        }
                        else if (FieldType == "D")
                        {
                            sbTemp.Append("<td  align='left' style='border:" + border + "px solid black;'>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + "</td><td colspan='3' style='border:" + border + "px solid black;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td>");
                        }

                        else
                        {
                            if (FieldType == "DT")
                            {
                                if (System.Web.HttpContext.Current.Session["DateDisplay"] == null)
                                {
                                    sbTemp.Append("<td style='font-family:Arial; font-size:10pt; border:" + border + "px solid black;' colspan='4' align='left'>" + sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td>");
                                }
                                System.Web.HttpContext.Current.Session["DateDisplay"] = null;
                            }
                            else //Heading
                            {
                                sbTemp.Append("<td colspan='4' align='left' style='border:" + border + "px solid black;'>" + sBegin + sDEPBold + common.myStr(dt.Rows[i]["FieldName"]) + eDEPBold + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td>");
                            }
                        }
                    }
                    sbTemp.Append("</tr>");
                    sbTemp.Append(EndList);
                }

            }
        }
        sbTemp.Append("</table>");
        sb.Append(sbTemp.ToString());

        sb = sb.Replace("font-size: 16px; font-family: &quot;microsoft sans serif&quot;;", "font-size: 10pt; font-family: Arial;");
        sb = sb.Replace("font-size: 16px; font-family: &quot;segoe ui&quot;;", "font-size: 10pt; font-family: Arial;");
        sb = sb.Replace("font-family: &quot;segoe ui&quot;;", " font-family: Arial;");

        return sb;
    }

    public StringBuilder BindLabTestResultNormal(int HospitalId, int EncounterId, int FacilityId, int LoginFacilityId, StringBuilder sb, StringBuilder sbTemplateStyle,
            DataRow drTemplateListStyle, Page pg, string SampleId, string DischargeSummary, bool bShowAllParameters)
    {

        string FontName = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "DischargeSummaryFont", sConString);
        if (common.myStr(FontName).Equals(string.Empty))
        {
            FontName = "Calibri";
        }
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        BaseC.clsLISLabOther lis = new BaseC.clsLISLabOther(sConString);
        BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        HshIn.Add("@inyHospitalLocationId", common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        HshIn.Add("@intLoginFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        HshIn.Add("@intEncounterId", EncounterId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@xmlDiagSampleIds", SampleId);
        HshIn.Add("@chvDischargeSummary", DischargeSummary);
        //HshIn.Add("@bitShowAllParameters", bShowAllParameters); 
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultForNoteIP", HshIn, HshOut);
        StringBuilder sbTemp = new StringBuilder();
        sBegin = ""; sEnd = "";

        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]), FontName);

        if (ds.Tables.Count > 0)
        {
            int subDeptId = 0;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "<b>" : "";
                    string eBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "</b>" : "";

                    string sDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "<b>" : "";
                    string eDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "</b>" : "";

                    string FieldType = common.myStr(dt.Rows[i]["FieldType"]).ToUpper().Trim();
                    string Symbol = common.myStr(dt.Rows[i]["Symbol"]).ToUpper().Trim();
                    string LimitType = common.myStr(dt.Rows[i]["LimitType"]).ToUpper().Trim();

                    if (FieldType.ToUpper().Equals("R") || common.myInt(dt.Rows[i]["ResultId"]) < 0)
                    {
                        continue;
                    }

                    if (common.myInt(dt.Rows[i]["SequenceNo"]).Equals(1) && FieldType != "DT" && common.myLen(dt.Rows[i]["ServiceName"]) > 0 && common.myStr(dt.Rows[i]["ServiceName"]).ToUpper() != "&NBSP;")
                    {
                        //if (common.myLen(dt.Rows[i]["SubName"]) > 0 && common.myStr(dt.Rows[i]["SubName"]).ToUpper() != "&NBSP;"
                        //&& ((subDeptId == 0 && common.myInt(dt.Rows[i]["SubDeptId"]) > 0) || (subDeptId != common.myInt(dt.Rows[i]["SubDeptId"]))))
                        //{
                        //    sbTemp.Append("<br/><b>" + sBegin + common.myStr(dt.Rows[i]["SubName"]) + sEnd + "</b>: ");
                        //    subDeptId = common.myInt(dt.Rows[i]["SubDeptId"]);
                        //}

                        if (common.myLen(dt.Rows[i]["SubName"]) > 0 && common.myStr(dt.Rows[i]["SubName"]).ToUpper() != "&NBSP;")
                        {
                            sbTemp.Append("<br/><b>" + sBegin + common.myStr(dt.Rows[i]["SubName"]) + sEnd + "</b>: ");
                            subDeptId = common.myInt(dt.Rows[i]["SubDeptId"]);
                        }

                        if (common.myBool(dt.Rows[i]["PrintServiceNameInReport"]))
                        {
                            sbTemp.Append(" <b>" + sBegin + common.myStr(dt.Rows[i]["ServiceName"]) + sEnd + "</b>: ");
                        }
                    }

                    sbTemp.Append(BeginList);
                    //sbTemp.Append("<tr valign ='top'>");

                    if (FieldType != "H")
                    {
                        if (FieldType == "SN" || FieldType == "W" || FieldType == "H")
                        {
                            DataSet dsSen = new DataSet();
                            if (common.myLen(dt.Rows[i]["FieldName"]) > 0 && common.myStr(dt.Rows[i]["FieldName"]).ToUpper() != "&NBSP;")
                            {
                                sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd);
                            }

                            if (FieldType == "SN")
                            {
                                dsSen = lis.GetDiagLabSensitivityResultForNote(common.myInt(dt.Rows[i]["DiagSampleId"]), common.myInt(dt.Rows[i]["ResultId"]), common.myStr(System.Web.HttpContext.Current.Session["OPIP"]));
                                if (dsSen.Tables[0].Rows.Count > 0)
                                {
                                    //sbTemp.Append("</tr><tr valign ='top'>");
                                    //sbTemp.Append("<td colspan='4' style='border:1px solid black;'>");
                                    //sbTemp.Append("<table border='0' width='100%' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;font-family:Arial; font-size:10pt;'>");
                                    //sbTemp.Append("<tr>");
                                    int count = 0;
                                    for (int l = 0; l < dsSen.Tables[0].Columns.Count; l++)
                                    {
                                        sbTemp.Append("<b>" + sBegin + dsSen.Tables[0].Columns[l].ColumnName + sEnd + "</b>, ");
                                        count++;
                                    }
                                    //sbTemp.Append("</tr>");
                                    sbTemp.Append(" ");

                                    for (int k = 0; k < dsSen.Tables[0].Rows.Count; k++)
                                    {
                                        //sbTemp.Append("<tr valign ='top'>");
                                        for (int j = 0; j < count; j++)
                                        {
                                            if (common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName]).Contains("<B>") == false
                                                && common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName]) != null)
                                            {
                                                sbTemp.Append(sBegin + common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName]) + sEnd + ", ");
                                            }
                                            else
                                            {
                                                sbTemp.Append(count + sBegin + common.myStr(dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[0].ColumnName]) + sEnd + ", ");
                                                break;
                                            }
                                        }
                                        //sbTemp.Append("</tr>");
                                        sbTemp.Append(" ");
                                    }
                                    //sbTemp.Append("</table>");
                                    //sbTemp.Append("</td>");
                                }
                            }
                            else
                            {
                                if (FieldType != "H")
                                {
                                    //sbTemp.Append("</tr>");
                                    //if (FieldType == "W")
                                    //{
                                    //    sbTemp.Append("<tr><td colspan='4' style='width: 60%;'>&nbsp;</td></tr>");
                                    //}
                                    //sbTemp.Append("<tr valign ='top'>");
                                    //sbTemp.Append("</tr><tr valign ='top'>");

                                    sbTemp.Append(" ");
                                    sbTemp.Append(sBegin + System.Web.HttpContext.Current.Server.HtmlDecode(common.myStr(dt.Rows[i]["Result"]).Replace("&lt;", "LLLLL").Replace("&gt;", "GGGGG")).Replace("LLLLL", "&lt;").Replace("GGGGG", "&gt;") + sEnd + ", ");
                                    //sbTemp.Append("<td colspan='4' style='width: 60%; border:1px solid black;'>" + sBegin + System.Web.HttpContext.Current.Server.HtmlDecode(dt.Rows[i]["Result"].ToString()) + sEnd + "</td>");
                                }
                            }
                        }
                        else
                        {
                            if (FieldType == "N" || FieldType == "F" || FieldType == "TM")
                            {
                                if ((Symbol == "<" || Symbol == ">") && LimitType != "SR")
                                {
                                    if (Symbol == "<")
                                    {
                                        Symbol = "&lt;";
                                    }
                                    else if (Symbol == ">")
                                    {
                                        Symbol = "&gt;";
                                    }
                                    sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + (common.myStr(dt.Rows[i]["FieldName"]).EndsWith(":") ? " " : ": ") + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + " " + sBegin + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + sBegin + common.myStr(Symbol) + ", ");
                                }
                                else if (LimitType == "SR")
                                {
                                    sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + (common.myStr(dt.Rows[i]["FieldName"]).EndsWith(":") ? " " : ": ") + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + " " + sBegin + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + " " + sBegin + common.myStr(Symbol) + ", ");
                                }
                                else
                                {
                                    sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + (common.myStr(dt.Rows[i]["FieldName"]).EndsWith(":") ? " " : ": ") + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + " " + sBegin + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + ", ");
                                }
                            }
                            else if (FieldType == "T" || FieldType == "M" || FieldType == "O" || FieldType == "E")
                            {
                                sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + (common.myStr(dt.Rows[i]["FieldName"]).EndsWith(":") ? " " : ": ") + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + ", ");
                            }
                            else if (FieldType == "D")
                            {
                                sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sEnd + (common.myStr(dt.Rows[i]["FieldName"]).EndsWith(":") ? " " : ": ") + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + ", ");
                            }
                            else
                            {
                                if (FieldType == "DT")
                                {
                                    if (System.Web.HttpContext.Current.Session["DateDisplay"] == null)
                                    {
                                        if (sbTemp.ToString() != string.Empty)
                                        {
                                            sbTemp.Append("<br/><br/>");
                                        }
                                        sbTemp.Append(sBegin + common.myStr(dt.Rows[i]["FieldName"]) + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + " " + sEnd);
                                    }
                                    System.Web.HttpContext.Current.Session["DateDisplay"] = null;
                                }
                                else if (common.myLen(dt.Rows[i]["FieldName"]) > 0)
                                {
                                    sbTemp.Append(sBegin + sDEPBold + common.myStr(dt.Rows[i]["FieldName"]) + eDEPBold + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + ", ");
                                }
                            }
                        }
                    }

                    //sbTemp.Append("</tr>");


                    sbTemp.Append(EndList);
                }

            }
        }
        //sbTemp.Append("</table>");
        sb.Append(sbTemp.ToString());
        objtlc = null;

        return sb;
    }

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item, Page Pg, string HospitalId)
    {

        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (item != null)
        {
            if (item[typ + "ListStyle"].ToString() == "1")
            {
                sBegin += "<li>";
                //aEnd.Add("</li>");
            }
            else if (item[typ + "ListStyle"].ToString() == "2")
            {
                sBegin += "<li>";
                //aEnd.Add("</li>");
            }

            //else
            //    sBegin += "<br />";

            if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "" || item[typ + "Bold"].ToString() != "" || item[typ + "Italic"].ToString() != "" || item[typ + "Underline"].ToString() != "")
            {
                sBegin += "<span style='";
                if (item[typ + "FontSize"].ToString() != "")
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += ff.getDefaultFontSize(Pg, HospitalId); }
                if (item[typ + "Forecolor"].ToString() != "")
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (item[typ + "FontStyle"].ToString() != "")
                { sBegin += GetFontFamily(typ, item, Pg, HospitalId); }
                //{ sBegin += " font-family: Candara ;"; }

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

                sBegin += " '>";
                if (item[typ + "ListStyle"].ToString() == "1")
                {
                    sEnd += "</li>";
                }
                else if (item[typ + "ListStyle"].ToString() == "2")
                {
                    sEnd += "</li>";
                }
            }
            else
            {
                sBegin += "<span style='";
                if (item[typ + "FontSize"].ToString() != "")
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += ff.getDefaultFontSize(Pg, HospitalId); }

                sBegin += GetFontFamily(typ, item, Pg, HospitalId);

                aEnd.Add("</span>");
                for (int i = aEnd.Count - 1; i >= 0; i--)
                {
                    sEnd += aEnd[i];
                }
                sBegin += " '>";
                if (item[typ + "ListStyle"].ToString() == "1")
                {
                    sEnd += "</li>";
                }
                else if (item[typ + "ListStyle"].ToString() == "2")
                {
                    sEnd += "</li>";
                }
            }

        }

    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item, Page Pg, string HospitalId)
    {
        MakeFontWithoutListStyle(typ, ref sBegin, ref sEnd, item, Pg, HospitalId, "Candara");
    }


    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item, Page Pg, string HospitalId, string FontName)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (item != null)
        {
            if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "" || item[typ + "Bold"].ToString() != "" || item[typ + "Italic"].ToString() != "" || item[typ + "Underline"].ToString() != "")
            {
                sBegin += "<span style='";
                if (item[typ + "FontSize"].ToString() != "")
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += getDefaultFontSize(Pg, HospitalId); }
                if (item[typ + "Forecolor"].ToString() != "")
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (item[typ + "FontStyle"].ToString() != "")
                //{ sBegin += " font-family: Candara ;"; }
                { sBegin += " font-family: " + common.myStr(FontName) + " ;"; }

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
                sBegin += " '>";
            }
            else
            {
                sBegin += "<span style='";
                if (item[typ + "FontSize"].ToString() != "")
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += getDefaultFontSize(Pg, HospitalId); }

                sBegin += GetFontFamily(typ, item, Pg, HospitalId);

                aEnd.Add("</span>");
                for (int i = aEnd.Count - 1; i >= 0; i--)
                {
                    sEnd += aEnd[i];
                }
                sBegin += " '>";
            }
        }

    }
    protected string GetFontFamily(string typ, DataRow item, Page Pg, string HospitalId)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        if (item[typ + "FontStyle"].ToString() != "")
        {
            FontName = fonts.GetFont("Name", item[typ + "FontStyle"].ToString());
            if (FontName != "")
                sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalId);
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }
    public StringBuilder BindLabResult(int EncounterId, int HospitalId, int FacilityId, string sFromDate, string sToDate, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string strLabNo, string strServiceId)
    {
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        bool bLRecords = false;
        bool bLMRecords = false;
        string sEncodedDate = "";
        string sServiceID = "";
        //  DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate,strLabNo,strServiceId);
        DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate, strLabNo, strServiceId);

        StringBuilder sbTemp = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            sServiceID = ds.Tables[0].Rows[0]["ServiceID"].ToString();
            sEncodedDate = ds.Tables[0].Rows[0]["ResultDate"].ToString();
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
        }

        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        sBeginSection = sBegin;
        sEndSection = sEnd;

        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        string sStartTable = "";
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sbTemp.Append(BeginList);
            if (sServiceID == ds.Tables[0].Rows[i]["ServiceID"].ToString() && bLRecords == false)
            {
                sbTemp.Append("<b><u>" + "Investigation(s) :" + "</u></b>" + "<br/>");
                sbTemp.Append("<b>" + "Date&nbsp;:&nbsp;&nbsp; " + ds.Tables[0].Rows[i]["ResultDate"] + "</b> : <br/>");
                sbTemp.Append("<b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b> : <br/>");
                bLRecords = true;
                //Remove Table
                sStartTable = " <table border='0' cellpadding='0' cellspacing='0' style='border-style:none;'>";
                sbTemp.Append(sStartTable);
                sServiceID = ds.Tables[0].Rows[i]["ServiceID"].ToString();
            }
            else if ((sEncodedDate != ds.Tables[0].Rows[i]["ResultDate"].ToString()) || (sServiceID != ds.Tables[0].Rows[i]["ServiceID"].ToString()))
            {
                if (sEncodedDate != ds.Tables[0].Rows[i]["ResultDate"].ToString())
                {
                    sStartTable = "";
                    sbTemp.Append("<tr><td><b>" + ds.Tables[0].Rows[i]["ResultDate"] + "</b> : <br/><td></tr>");
                    bLMRecords = true;
                    sEncodedDate = ds.Tables[0].Rows[i]["ResultDate"].ToString();
                }
                if (sServiceID != ds.Tables[0].Rows[i]["ServiceID"].ToString())
                {
                    sbTemp.Append("<tr ><td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b> : <br/></td></tr>");

                    //sStartTable = " <table border='0' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;'>";
                    //sbTemp.Append(sStartTable);
                    sServiceID = ds.Tables[0].Rows[i]["ServiceID"].ToString();
                }
            }

            sbTemp.Append("<tr ><td style='border-style:none;width:400px;'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["FieldName"]) + "</td><td style='border-style: none;'> " + common.myStr(ds.Tables[0].Rows[i]["ValueText"]) + sEnd + "<br/></td></tr>");
            // sbTemp.Append( "<br/>"+ sBegin +  common.myStr(ds.Tables[0].Rows[i]["FieldName"]) +  "<span style=width: 400px> </span>"+ common.myStr(ds.Tables[0].Rows[i]["ValueText"]) + sEnd + "<br/>");



        }
        sbTemp.Append("</table>");
        sStartTable = " <table border='0'  width='100%'height ='50px' cellpadding='0' cellspacing='0' style='border-style:none;'>";
        sbTemp.Append(sStartTable);
        sbTemp.Append("<tr ><td style='border-style:none;width:100%;'> </td></tr>");
        sbTemp.Append("</table>");

        sbTemp.Append(EndList);


        sb.Append(sbTemp.ToString());
        return sb;
    }
    public DataSet GetICMPatientInvestigationResult(int iHospitalLocationID, int iFacilityId, int iEncounterID, string sFromDate, string sToDate, string srtLabNo, string strServiceId)
    {
        Hashtable HshIn = new Hashtable();
        ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
        HshIn.Add("@intFacilityId", iFacilityId);
        HshIn.Add("@inEncId", iEncounterID);
        HshIn.Add("@chrFromDate", sFromDate);
        HshIn.Add("@chrToDate", sToDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspGetInvestigationsResult", HshIn);
        objtlc = null;
        return ds;
    }

    public StringBuilder BindPendingInvestigation(int HospitalId, int FacilityId, int EncounterId, StringBuilder sb,
                  StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                  string pageID)
    {
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        bool bLRecords = false;
        bool bLMRecords = false;
        string sEncodedDate = "";
        string sServiceID = "";
        //  DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate,strLabNo,strServiceId);
        DataSet ds = GetPendingInvestigations(HospitalId, FacilityId, EncounterId);

        StringBuilder sbTemp = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            //    sServiceID = ds.Tables[0].Rows[0]["ServiceID"].ToString();
            //    sEncodedDate = ds.Tables[0].Rows[0]["ResultDate"].ToString();
            sb.Append(sbTemplateStyle);
            sbTemp.Append("<table border='0' cellpadding='0' cellspacing='0' style='border-style:none;'><tr>");
            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                //Console.Write("Item: ");
                //Console.Write(column.ColumnName);
                //Console.Write(" ");
                //Console.WriteLine(row[column]);


                //  Console.Write(column.ColumnName);
                if (column.ColumnName.Equals("Sno"))
                { sbTemp.Append("<td style='border-style:none;width:30px;'><b>" + column.ColumnName + "</b>  <br/></td>"); }

                if (column.ColumnName.Equals("OrderDate"))
                { sbTemp.Append("<td style='border-style:none;width:160px;'><b>" + column.ColumnName + " </b> <br/></td>"); }
                if (column.ColumnName.Equals("LabNo"))
                { sbTemp.Append("<td style='border-style:none;width:100px;'><b>" + column.ColumnName + " </b> <br/></td>"); }
                if (column.ColumnName.Equals("ServiceName"))
                { sbTemp.Append("<td style='border-style:none;width:250px;'><b>" + column.ColumnName + " </b> <br/></td>"); }

                if (column.ColumnName.Equals("Status"))
                { sbTemp.Append("<td style='border-style:none;width:250px;'><b>" + column.ColumnName + "</b>  <br/></td>"); }
                //sbTemp.Append("<td>" + column.ColumnName + "  <br/><td>");


            }

            sbTemp.Append("</tr></table>");


            //    if (drTemplateListStyle != null)
            //    {
            //        if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "1")
            //        {
            //            BeginList = "<ul>"; EndList = "</ul>";
            //        }
            //        else if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "2")
            //        {
            //            BeginList = "<ol>"; EndList = "</ol>";
            //        }
            //    }
        }

        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        sBeginSection = sBegin;
        sEndSection = sEnd;

        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        string sStartTable = "";
        if (!common.myInt(ds.Tables[0].Rows.Count).Equals(0))
        {


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sbTemp.Append(BeginList);
                //if (sServiceID == ds.Tables[0].Rows[i]["ServiceID"].ToString() && bLRecords == false)
                //{
                //sbTemp.Append("<b><u>" + "Pending Investigation(s) :" + "</u></b>" + "<br/>");
                //sbTemp.Append("<b>" + "Sno." + ds.Tables[0].Rows[i]["Sno"] + "</b>");
                //sbTemp.Append("<b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b> : <br/>");
                // bLRecords = true;
                //Remove Table
                sStartTable = " <table border='0' cellpadding='0' cellspacing='0' style='border-style:none;'>";
                sbTemp.Append(sStartTable);
                //sServiceID = ds.Tables[0].Rows[i]["ServiceID"].ToString();
                //}
                //else if ((sEncodedDate != ds.Tables[0].Rows[i]["ResultDate"].ToString()) || (sServiceID != ds.Tables[0].Rows[i]["ServiceID"].ToString()))
                //{
                //if (sEncodedDate != ds.Tables[0].Rows[i]["ResultDate"].ToString())
                //{
                //sStartTable = "";
                //    sbTemp.Append("<tr><td>" + ds.Tables[0].Rows[i]["Sno"] + "  <br/><td>");
                //sbTemp.Append("<td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["OrderNo"] + "</b>  <br/></td>");
                //sbTemp.Append("<td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["OrderDate"] + "</b>  <br/></td>");
                //sbTemp.Append("<td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["LabNo"] + "</b>  <br/></td>");
                //sbTemp.Append("<td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b>  <br/></td>");
                //sbTemp.Append("<td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["SubDepartment"] + "</b>  <br/></td>");
                //sbTemp.Append("<td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["Status"] + "</b>  <br/></td></tr>");

                sbTemp.Append("<tr><td  style='border-style:none;width:30px;'>" + ds.Tables[0].Rows[i]["Sno"] + "  <br/></td>");

                sbTemp.Append("<td style='border-style:none;width:160px;'>" + ds.Tables[0].Rows[i]["OrderDate"] + "  <br/></td>");
                sbTemp.Append("<td style='border-style:none;width:100px;'>" + ds.Tables[0].Rows[i]["LabNo"] + "  <br/></td>");
                sbTemp.Append("<td style='border-style:none;width:250px;'>" + ds.Tables[0].Rows[i]["ServiceName"] + "  <br/></td>");

                sbTemp.Append("<td style='border-style:none;width:250px;'>" + ds.Tables[0].Rows[i]["Status"] + "  <br/></td></tr>");



                //}
                //if (sServiceID != ds.Tables[0].Rows[i]["ServiceID"].ToString())
                //{
                // sbTemp.Append("<tr ><td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b> : <br/></td></tr>");

                //sStartTable = " <table border='0' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;'>";
                //sbTemp.Append(sStartTable);

                //}
                //}

                // sbTemp.Append("<tr ><td style='border-style:none;width:400px;'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["FieldName"]) + "</td><td style='border-style: none;'> " + common.myStr(ds.Tables[0].Rows[i]["ValueText"]) + sEnd + "<br/></td></tr>");
                // sbTemp.Append( "<br/>"+ sBegin +  common.myStr(ds.Tables[0].Rows[i]["FieldName"]) +  "<span style=width: 400px> </span>"+ common.myStr(ds.Tables[0].Rows[i]["ValueText"]) + sEnd + "<br/>");



            }
            sbTemp.Append("</table>");
            //sStartTable = " <table border='0'  width='100%'height ='50px' cellpadding='0' cellspacing='0' style='border-style:none;'>";
            //sbTemp.Append(sStartTable);
            //sbTemp.Append("<tr ><td style='border-style:none;width:100%;'> </td></tr>");
            //sbTemp.Append("</table>");

            sbTemp.Append(EndList);


            sb.Append(sbTemp.ToString());
        }
        return sb;
    }

    //public StringBuilder BindMedicationTabular(int HospitalId, int FacilityId, int EncounterId, StringBuilder sb,
    //             StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
    //             string pageID)
    //{
    //    string BeginList = "", EndList = "",  sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

    //    //  DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate,strLabNo,strServiceId);
    //    DataSet ds = GetMedicationTabular(HospitalId, FacilityId, EncounterId);

    //    StringBuilder sbTemp = new StringBuilder();
    //    if (ds.Tables[0].Rows.Count > 0)
    //    {
    //        int totalColumnsForColSpan =  ds.Tables[0].Columns.Count;
    //        foreach (DataColumn columnCount in ds.Tables[0].Columns)
    //        {
    //            if (columnCount.Caption.Equals("Dose"))
    //            {
    //                totalColumnsForColSpan = totalColumnsForColSpan + 1;
    //            }
    //            if (columnCount.Caption.Equals("ItemName"))
    //            {
    //                totalColumnsForColSpan = totalColumnsForColSpan + 2;
    //            }
    //            if (columnCount.Caption.Equals("Instructions"))
    //            {
    //                totalColumnsForColSpan = totalColumnsForColSpan + 2;
    //            }
    //        }
    //            sb.Append(sbTemplateStyle);
    //        sbTemp.Append("<table width='100%' border='1' cellpadding='2' cellspacing='1' style='border-style:none;'>");
    //        sbTemp.Append("<tr>");
    //        for (int i = 0; i < totalColumnsForColSpan; i++)
    //        {
    //            sbTemp.Append("<td border='0' cellpadding='0' cellspacing='0'></td>");
    //            // sbTemp.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
    //        }
    //        sbTemp.Append("</tr>");

    //        foreach (DataColumn column in ds.Tables[0].Columns)
    //        {

    //            if (column.ColumnName.Equals("SNo"))
    //            {
    //                sbTemp.Append("<tr><th colspan='0' style='border='1';width:1%;'><b>" + column.ColumnName + "</b>  <br/></th>");
    //            }

    //         else if (column.ColumnName.Equals("ItemName"))
    //            {
    //                sbTemp.Append("<th  colspan='3' style='border='1';width:20%; '><b>" + column.ColumnName + " </b> <br/></th>");
    //            }
    //            else if (column.ColumnName.Equals("Dose"))
    //            {
    //                sbTemp.Append("<th colspan='2' style='border='1';width:5%; '><b>" + column.ColumnName + " </b> <br/></th>");
    //            }
    //            else if (column.ColumnName.Equals("Frequency"))
    //            {
    //                sbTemp.Append("<th colspan='1' style='border='1';width:10%;'><b>" + column.ColumnName + " </b> <br/></th>");
    //            }

    //            else if (column.ColumnName.Equals("Duration"))
    //            {
    //                sbTemp.Append("<th colspan='2' style='border='1';width:13%;'><b>" + column.ColumnName + "</b>  <br/></th>");
    //            }
    //            else if (column.ColumnName.Equals("Route"))
    //            {
    //                sbTemp.Append("<th colspan='1' style='border='1';width:10%; '><b>" + column.ColumnName + "</b>  <br/></th>");
    //            }
    //            else if (column.ColumnName.Equals("StartDate"))
    //            {
    //                sbTemp.Append("<th colspan='1' style='border='1';width:10%; '><b>" + column.ColumnName + "</b>  <br/></th>");
    //            }
    //            else if (column.ColumnName.Equals("Instructions"))
    //            {
    //                sbTemp.Append("<th colspan='3' style='border='1';width:10%; '><b>" + column.ColumnName + "</b>  <br/></th>");
    //            }
    //            else
    //            {
    //                sbTemp.Append("<th colspan='1' style='border='1';width:20%; '><b>" + column.ColumnName + "</b>  <br/></th>");
    //            }



    //        }

    //        //sbTemp.Append("</tr></table>");
    //        sbTemp.Append("</tr>");

    //    }

    //    MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
    //    sBeginSection = sBegin;
    //    sEndSection = sEnd;

    //    sBegin = ""; sEnd = "";
    //    MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
    //    string sStartTable = "";
    //    if (!common.myInt(ds.Tables[0].Rows.Count).Equals(0))
    //    {

    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        {
    //            sbTemp.Append(BeginList);

    //          //  sStartTable = " <table border='0' cellpadding='0' cellspacing='0' style='border-style:none;'>";

    //            sbTemp.Append(sStartTable);
    //            foreach (DataColumn column in ds.Tables[0].Columns)
    //            {

    //                if (column.ColumnName.Equals("SNo"))
    //                {
    //                    sbTemp.Append("<tr><td colspan='0'  style='border='1';width:1%;'>" + ds.Tables[0].Rows[i]["SNo"] + "  <br/></td>");

    //                }
    //              else  if (column.ColumnName.Equals("ItemName"))
    //                {
    //                    sbTemp.Append("<td  colspan='3' style='border='1';'>" + ds.Tables[0].Rows[i]["ItemName"] + "  <br/></td>");

    //                }
    //                else if (column.ColumnName.Equals("Dose"))
    //                {
    //                    sbTemp.Append("<td  colspan='2' style='border='1';'>" + ds.Tables[0].Rows[i]["Dose"] + "  <br/></td>");

    //                }
    //                else if (column.ColumnName.Equals("Frequency"))
    //                {
    //                    sbTemp.Append("<td  colspan='1' style='border='1';'>" + ds.Tables[0].Rows[i]["Frequency"] + "  <br/></td>");

    //                }
    //                else if (column.ColumnName.Equals("Duration"))
    //                {
    //                    sbTemp.Append("<td  colspan='2' style='border='1';'>" + ds.Tables[0].Rows[i]["Duration"] + "  <br/></td>");

    //                }
    //                else if (column.ColumnName.Equals("Route"))
    //                {
    //                    sbTemp.Append("<td  colspan='1' style='border='1';'>" + ds.Tables[0].Rows[i]["Route"] + "  <br/></td>");

    //                }

    //                else if (column.ColumnName.Equals("StartDate"))
    //                {
    //                    sbTemp.Append("<td  colspan='1' style='border='1';'>" + ds.Tables[0].Rows[i]["StartDate"] + "  <br/></td>");

    //                }
    //                else if (column.ColumnName.Equals("Instructions"))
    //                {
    //                    sbTemp.Append("<td  colspan='3' style='border='1';'>" + ds.Tables[0].Rows[i]["Instructions"] + "  <br/></td>");
    //                }
    //                else
    //                {
    //                    sbTemp.Append("<td  colspan='1' style='border='1';'>" + string.Empty  + "  <br/></td>");
    //                }

    //            }
    //            sbTemp.Append("</tr>");
    //            //sbTemp.Append("</table>");

    //        }
    //        sbTemp.Append("</table>");



    //        sbTemp.Append(EndList);


    //        sb.Append(sbTemp.ToString());
    //    }
    //    return sb;
    //}


    public StringBuilder BindMedicationTabular(int HospitalId, int FacilityId, int EncounterId, StringBuilder sb,
            StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
            string pageID)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        //  DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate,strLabNo,strServiceId);
        DataSet ds = GetMedicationTabular(HospitalId, FacilityId, EncounterId);

        StringBuilder sbTemp = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            int totalColumnsForColSpan = ds.Tables[0].Columns.Count;
            foreach (DataColumn columnCount in ds.Tables[0].Columns)
            {
                if (columnCount.Caption.Equals("Dose"))
                {
                    totalColumnsForColSpan += 1;
                }
                if (columnCount.Caption.Equals("ItemName"))
                {
                    totalColumnsForColSpan += 2;
                }
                if (columnCount.Caption.Equals("Instructions"))
                {
                    totalColumnsForColSpan += 2;
                }
            }
            sb.Append(sbTemplateStyle);
            sbTemp.Append("<table width='100%' cellpadding='2' cellspacing='1' style='border-style:solid; border:1px;'>");
            sbTemp.Append("<tr>");
            for (int i = 0; i < totalColumnsForColSpan; i++)
            {
                sbTemp.Append("<td border='0'></td>");
                // sbTemp.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
            }
            sbTemp.Append("</tr>");

            foreach (DataColumn column in ds.Tables[0].Columns)
            {

                if (column.ColumnName.Equals("SNo"))
                {
                    sbTemp.Append("<tr><th  style='border='1';width:1%;'><b>" + column.ColumnName + "</b>  <br/></th>");
                }

                else if (column.ColumnName.Equals("ItemName"))
                {
                    sbTemp.Append("<th  colspan='3' style='border='1';width:20%; '><b>" + column.ColumnName + " </b> <br/></th>");
                }
                else if (column.ColumnName.Equals("Dose"))
                {
                    sbTemp.Append("<th colspan='2' style='border='1';width:5%; '><b>" + column.ColumnName + " </b> <br/></th>");
                }
                else if (column.ColumnName.Equals("Frequency"))
                {
                    sbTemp.Append("<th colspan='1' style='border='1';width:10%;'><b>" + column.ColumnName + " </b> <br/></th>");
                }

                else if (column.ColumnName.Equals("Duration"))
                {
                    sbTemp.Append("<th colspan='2' style='border='1';width:13%;'><b>" + column.ColumnName + "</b>  <br/></th>");
                }
                else if (column.ColumnName.Equals("Route"))
                {
                    sbTemp.Append("<th colspan='1' style='border='1';width:10%; '><b>" + column.ColumnName + "</b>  <br/></th>");
                }
                else if (column.ColumnName.Equals("StartDate"))
                {
                    sbTemp.Append("<th colspan='1' style='border='1';width:10%; '><b>" + column.ColumnName + "</b>  <br/></th>");
                }
                else if (column.ColumnName.Equals("Instructions"))
                {
                    sbTemp.Append("<th colspan='3' style='border='1';width:10%; '><b>" + column.ColumnName + "</b>  <br/></th>");
                }
                else
                {
                    sbTemp.Append("<th colspan='1' style='border='1';width:20%; '><b>" + column.ColumnName + "</b>  <br/></th>");
                }



            }

            //sbTemp.Append("</tr></table>");
            sbTemp.Append("</tr>");

        }

        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        sBeginSection = sBegin;
        sEndSection = sEnd;

        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        string sStartTable = "";
        if (!common.myInt(ds.Tables[0].Rows.Count).Equals(0))
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sbTemp.Append(BeginList);

                //  sStartTable = " <table border='0' cellpadding='0' cellspacing='0' style='border-style:none;'>";

                sbTemp.Append(sStartTable);
                foreach (DataColumn column in ds.Tables[0].Columns)
                {

                    if (column.ColumnName.Equals("SNo"))
                    {
                        sbTemp.Append("<tr><td   style='border='1';width:1%;'>" + ds.Tables[0].Rows[i]["SNo"] + "  <br/></td>");

                    }
                    else if (column.ColumnName.Equals("ItemName"))
                    {
                        sbTemp.Append("<td  colspan='3' style='border='1';'>" + ds.Tables[0].Rows[i]["ItemName"] + "  <br/></td>");

                    }
                    else if (column.ColumnName.Equals("Dose"))
                    {
                        sbTemp.Append("<td  colspan='2' style='border='1';'>" + ds.Tables[0].Rows[i]["Dose"] + "  <br/></td>");

                    }
                    else if (column.ColumnName.Equals("Frequency"))
                    {
                        sbTemp.Append("<td   style='border='1';'>" + ds.Tables[0].Rows[i]["Frequency"] + "  <br/></td>");

                    }
                    else if (column.ColumnName.Equals("Duration"))
                    {
                        sbTemp.Append("<td  colspan='2' style='border='1';'>" + ds.Tables[0].Rows[i]["Duration"] + "  <br/></td>");

                    }
                    else if (column.ColumnName.Equals("Route"))
                    {
                        sbTemp.Append("<td   style='border='1';'>" + ds.Tables[0].Rows[i]["Route"] + "  <br/></td>");

                    }

                    else if (column.ColumnName.Equals("StartDate"))
                    {
                        sbTemp.Append("<td   style='border='1';'>" + ds.Tables[0].Rows[i]["StartDate"] + "  <br/></td>");

                    }
                    else if (column.ColumnName.Equals("Instructions"))
                    {
                        sbTemp.Append("<td  colspan='3' style='border='1';'>" + ds.Tables[0].Rows[i]["Instructions"] + "  <br/></td>");
                    }
                    else
                    {
                        sbTemp.Append("<td   style='border='1';'>" + string.Empty + "  <br/></td>");
                    }

                }
                sbTemp.Append("</tr>");
                //sbTemp.Append("</table>");

            }
            sbTemp.Append("</table>");



            sbTemp.Append(EndList);


            sb.Append(sbTemp.ToString());
        }
        return sb;
    }
    public DataSet GetPendingInvestigations(int iHospitalLocationID, int iFacilityId, int iEncounterID)
    {
        Hashtable HshIn = new Hashtable();
        ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
        HshIn.Add("@intLoginFacilityId", iFacilityId);
        HshIn.Add("@EncounterId", iEncounterID);
        //HshIn.Add("@type", sFromDate);

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspEMRGetPendingInvestigations", HshIn);
        objtlc = null;
        return ds;
    }
    public DataSet GetMedicationTabular(int iHospitalLocationID, int iFacilityId, int iEncounterID)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        Hashtable hsMed = new Hashtable();
        hsMed = new Hashtable();
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        DataTable dtPriscription = new DataTable();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sbPrescribedFinal = new StringBuilder();
        BaseC.clsEMR clsemrobj = new BaseC.clsEMR(sConString);

        hsMed.Add("@inyHospitalLocationId", iHospitalLocationID);
        hsMed.Add("@intFacilityId", iFacilityId);
        hsMed.Add("@intEncounterId", iEncounterID);
        if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("O"))
        {
            hsMed.Add("@chvIndentCode", string.Empty);
        }
        else
        {
            hsMed.Add("@chvIndentCode", "D");
        }
        hsMed.Add("@chvPreviousMedication", "T");

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", hsMed);
        objtlc = null;
        return ds;
    }

    public DataSet GetMedicationTabular(int iHospitalLocationID, int iFacilityId, int iEncounterID, string xmlIndentDetailIds)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        Hashtable hsMed = new Hashtable();
        hsMed = new Hashtable();
        DataSet ds = new DataSet();
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataTable dtPriscription = new DataTable();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sbPrescribedFinal = new StringBuilder();
        BaseC.clsEMR clsemrobj = new BaseC.clsEMR(sConString);

        hsMed.Add("@inyHospitalLocationId", iHospitalLocationID);
        hsMed.Add("@intFacilityId", iFacilityId);
        hsMed.Add("@intEncounterId", iEncounterID);
        if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("O"))
        {
            hsMed.Add("@chvIndentCode", string.Empty);
        }
        else
        {
            hsMed.Add("@chvIndentCode", "D");
        }
        hsMed.Add("@chvPreviousMedication", "T");

        if (common.myLen(xmlIndentDetailIds) > 0)
        {
            hsMed.Add("@xmlIndentDetailIds", xmlIndentDetailIds);
        }

        ds = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", hsMed);

        return ds;
    }
    public StringBuilder BindMedicationTabular(int HospitalId, int FacilityId, int EncounterId, StringBuilder sb,
               StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string xmlIndentDetailIds)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        //DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate,strLabNo,strServiceId);
        DataSet ds = GetMedicationTabular(HospitalId, FacilityId, EncounterId, xmlIndentDetailIds);

        //MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        //sBeginSection = sBegin;
        //sEndSection = sEnd;

        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

        StringBuilder sbTemp = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            int totalColumnsForColSpan = ds.Tables[0].Columns.Count;
            foreach (DataColumn columnCount in ds.Tables[0].Columns)
            {
                if (columnCount.Caption.Equals("ItemName"))
                {
                    totalColumnsForColSpan += 2;
                }
                else if (columnCount.Caption.Equals("Dose"))
                {
                    totalColumnsForColSpan += 1;
                }
                else if (columnCount.Caption.Equals("Frequency"))
                {
                    totalColumnsForColSpan += 1;
                }
                else if (columnCount.Caption.Equals("Duration"))
                {
                    totalColumnsForColSpan += 1;
                }
                else if (columnCount.Caption.Equals("FoodRelationship"))
                {
                    totalColumnsForColSpan += 1;
                }
                else if (columnCount.Caption.Equals("Instructions"))
                {
                    totalColumnsForColSpan += 2;
                }
            }
            sb.Append(sbTemplateStyle);
            sbTemp.Append("<table width='100%' cellpadding='2' cellspacing='1' style='border-style:solid; border:1px;'>");

            if (totalColumnsForColSpan > 0)
            {
                sbTemp.Append("<tr>");
                for (int i = 0; i < totalColumnsForColSpan; i++)
                {
                    sbTemp.Append("<td border='0'></td>");
                }
                sbTemp.Append("</tr>");
            }

            foreach (DataColumn column in ds.Tables[0].Columns)
            {
                if (column.ColumnName.Equals("SNo"))
                {
                    sbTemp.Append("<tr><th border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("ItemName"))
                {
                    sbTemp.Append("<th colspan='3' border='1'><b>" + sBegin + "Drug Name" + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("Dose"))
                {
                    sbTemp.Append("<th colspan='2' border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("Frequency"))
                {
                    sbTemp.Append("<th colspan='2' border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("Duration"))
                {
                    sbTemp.Append("<th colspan='2' border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("FoodRelationship"))
                {
                    sbTemp.Append("<th colspan='2' border='1'><b>" + sBegin + "Food Relationship" + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("Route"))
                {
                    sbTemp.Append("<th border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("StartDate"))
                {
                    sbTemp.Append("<th border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else if (column.ColumnName.Equals("Instructions"))
                {
                    sbTemp.Append("<th colspan='3' border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
                else
                {
                    sbTemp.Append("<th border='1'><b>" + sBegin + column.ColumnName + sEnd + "</b></th>");
                }
            }

            sbTemp.Append("</tr>");
        }


        string sStartTable = "";
        if (!common.myInt(ds.Tables[0].Rows.Count).Equals(0))
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sbTemp.Append(BeginList);

                sbTemp.Append(sStartTable);
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    if (column.ColumnName.Equals("SNo"))
                    {
                        sbTemp.Append("<tr><td border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["SNo"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("ItemName"))
                    {
                        sbTemp.Append("<td colspan='3' border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["ItemName"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("Dose"))
                    {
                        sbTemp.Append("<td colspan='2' border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["Dose"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("Frequency"))
                    {
                        sbTemp.Append("<td colspan='2' border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["Frequency"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("Duration"))
                    {
                        sbTemp.Append("<td colspan='2' border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["Duration"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("FoodRelationship"))
                    {
                        sbTemp.Append("<td colspan='2' border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["FoodRelationship"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("Route"))
                    {
                        sbTemp.Append("<td border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["Route"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("StartDate"))
                    {
                        sbTemp.Append("<td border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["StartDate"], true) + sEnd + "</td>");
                    }
                    else if (column.ColumnName.Equals("Instructions"))
                    {
                        sbTemp.Append("<td colspan='3' border='1'>" + sBegin + common.myStr(ds.Tables[0].Rows[i]["Instructions"], true) + sEnd + "</td>");
                    }
                    else
                    {
                        sbTemp.Append("<td border='1'></td>");
                    }
                }
                sbTemp.Append("</tr>");
            }
            sbTemp.Append("</table>");

            sbTemp.Append(EndList);
            sb.Append(sbTemp.ToString());
        }
        return sb;
    }

}

