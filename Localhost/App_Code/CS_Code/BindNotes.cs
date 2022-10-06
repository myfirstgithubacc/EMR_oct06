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

/// <summary>
/// Summary description for BindNotes
/// </summary>
public class BindNotes
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

    public BindNotes(string Constring)
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
        string strUser = "Select ISNULL(tm.Name,'') + ISNULL(' ' + LastName,'') + ISNULL(' ' + FirstName,'') + ISNULL(' ' + MiddleName,'') AS EmployeeName FROM Employee em WITH (NOLOCK) INNER JOIN Users us WITH (NOLOCK) ON em.ID=us.EmpID LEFT JOIN TitleMaster tm WITH (NOLOCK) on em.TitleId=tm.TitleID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";
        DataSet dsUser = objtlc.GetDataSet(CommandType.Text, strUser, hshUser);
        if (dsUser.Tables[0].Rows.Count > 0)
        {
            sb.Append("<br/>");
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
            sbDisplayName.Append(sBeginFont + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered by: " + common.myStr(dsUser.Tables[0].Rows[0]["EmployeeName"]) + " on " + enterDate + "</span>" + sEndFont);
        }
        sb.Append(sbDisplayName + "<br/><br/>");
        dsUser.Dispose();
        hshUser = null;
        sbDisplayName = null;
        objtlc = null;
        return sb;
    }
    public DataSet GetTemplateStyle(int iHospitalLocationId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        Hashtable hst = new Hashtable();
        DataSet ds = new DataSet();
        hst.Add("@inyHospitalLocationID", iHospitalLocationId);
        string sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
        + " TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
        + " SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
        + " FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber,StaticTemplateName "
        + " FROM EMRTemplateStatic WITH (NOLOCK) where (HospitalLocationId = @inyHospitalLocationID or HospitalLocationId is null)";
        ds = objtlc.GetDataSet(CommandType.Text, sql, hst);
        hst = null;
        objtlc = null;
        return ds;
    }

    public DataSet GetPatientEMRData(int iEncounterId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        Hashtable hst = new Hashtable();
        DataSet ds = new DataSet();
        hst.Add("@intEncounterId", iEncounterId);
        string sql = "SELECT PatientSummary, StatusId FROM EMRPatientForms WITH (NOLOCK) WHERE EncounterId = @intEncounterId AND ACTIVE=1";
        ds = objtlc.GetDataSet(CommandType.Text, sql, hst);
        hst = null;
        objtlc = null;
        return ds;
    }

    public DataSet GetEMRTemplates(int iEncounterId, int iRegistrationId, string sEREncounterId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        DataSet ds = new DataSet();
        Hashtable hst = new Hashtable();
        hst.Add("@FormId", "1");
        hst.Add("@intEncounterId", iEncounterId);
        hst.Add("@intRegistrationId", iRegistrationId);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", hst);
        
        hst = null;
        objtlc = null;
        return ds;
    }
    public DataSet GetEMRTemplates(int iEncounterId, int iRegistrationId, string sEREncounterId, int TemplateId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        DataSet ds = new DataSet();
        Hashtable hst = new Hashtable();
        hst.Add("@FormId", "1");
        hst.Add("@intEncounterId", iEncounterId);

        hst.Add("@intRegistrationId", iRegistrationId);
        hst.Add("@intTemplateId", TemplateId);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", hst);
        
        hst = null;
        objtlc = null;
        return ds;
    }
    public StringBuilder GetEncounterFollowUpAppointment(string HospitalId, int EncounterId, StringBuilder sb,
        StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string fromDate, string toDate, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string date = string.Empty;
        int i = 0;
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationId", HospitalId);
        hsTb.Add("@intEncounterId", EncounterId);
        hsTb.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd"));
            hsTb.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }
        if (GroupingDate != "")
            hsTb.Add("@chrGroupingDate", GroupingDate);


        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetEncounterFollowUpAppointment", hsTb);

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myInt(HospitalId).ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle + "<u><b>" + "Follow-up Appointment:" + "</b></u>" + BeginList);
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append("<br/>");
            sb.Append(BeginList);
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sb.Append(sBegin + "Appointment Date : " + common.myStr(ds.Tables[0].Rows[i]["AppointmentDate"]) + " " + common.myStr(ds.Tables[0].Rows[i]["FromTime"]));
                sb.Append(", Visit Type : " + common.myStr(ds.Tables[0].Rows[i]["VisitType"]));
                sb.Append(", Doctor Name : " + common.myStr(ds.Tables[0].Rows[i]["DoctorName"]));
                sb.Append(common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() != string.Empty ? ", Remarks : " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]) + ". " : ".");
                sb.Append(sEnd + "<br/>");
            }
        }
        sb.Append(EndList);
        hsTb = null;
        ds.Dispose();
        objtlc = null;

        return sb;
    }

    public string getDefaultFontName(Page pg, string HospitalLocationId)
    {
        string sFontName = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalLocationId);
        if (FieldValue != "")
        {
            sFontName = fonts.GetFont("Name", FieldValue);
            if (sFontName != "")
                sFontName = " font-family: " + sFontName + ";";
        }
        return sFontName;
    }

    public StringBuilder DisplayUserName(string userID, string hospitalID, string enterDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        StringBuilder sb = new StringBuilder();
        sb.Append("<br/>");
        StringBuilder sbDisplayName = new StringBuilder();
        Hashtable hshUser = new Hashtable();
        hshUser.Add("@UserID", userID);
        hshUser.Add("@inyHospitalLocationID", hospitalID);
        string strUser = "Select ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS EmployeeName  FROM Employee em WITH (NOLOCK) INNER JOIN Users us WITH (NOLOCK) ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";
        DataSet dsUser = objtlc.GetDataSet(CommandType.Text, strUser, hshUser);
        if (dsUser.Tables[0].Rows.Count > 0)
        {
            string strDate = enterDate;
            sbDisplayName.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered by: " + common.myStr(dsUser.Tables[0].Rows[0]["EmployeeName"]) + " on " + strDate + "</span>");
        }

        dsUser.Dispose();
        hshUser = null;
        sb.Append(sbDisplayName);
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
        string strDisplayUserName = "select DisplayUserName from EMRTemplateStatic WITH (NOLOCK) where pageid=@intTemplateId and (HospitalLocationId = @inyHospitalLocationID or HospitalLocationId is null)";
        DataSet dsDisplayName = objtlc.GetDataSet(CommandType.Text, strDisplayUserName, hshtable);
        if (dsDisplayName.Tables[0].Rows.Count > 0)
        {
            if (common.myBool(dsDisplayName.Tables[0].Rows[0]["DisplayUserName"]))
            {
                Hashtable hshUser = new Hashtable();
                hshUser.Add("@UserID", userID);
                hshUser.Add("@inyHospitalLocationID", hospitalID);
                string strUser = "Select ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS EmployeeName FROM Employee em WITH (NOLOCK) INNER JOIN Users us WITH (NOLOCK) ON em.ID=us.EmpID WHERE us.ID=@UserID and em.HospitalLocationId=@inyHospitalLocationID";
                DataSet dsUser = objtlc.GetDataSet(CommandType.Text, strUser, hshUser);
                if (dsUser.Tables[0].Rows.Count > 0)
                {
                    sb.Append("<br/>");
                    sbDisplayName.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered by: " + common.myStr(dsUser.Tables[0].Rows[0]["EmployeeName"]) + " on " + common.myStr(Convert.ToDateTime(enterDate).Date.ToString("MMMM dd yyyy")) + "</span>");
                }
                hshUser = null;
                dsUser.Dispose();
            }
        }
        sb.Append(sbDisplayName);
        dsDisplayName.Dispose();
        hshtable = null;
        objtlc = null;
        return sb;
    }

    public StringBuilder BindVitals(string HospitalId, int EncounterId,
                                        StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                        Page pg, string pageID, string userID, int StaticTemplateId, string sEREncounterId)
    {
        return BindVitals(HospitalId, EncounterId, sb, sbTemplateStyle, drTemplateListStyle, pg, pageID, userID, "", "", StaticTemplateId, sEREncounterId, "");
    }

    public StringBuilder BindVitals(string HospitalId, int EncounterId,
                                    StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                    Page pg, string pageID, string userID, string fromDate, string toDate, int TemplateFieldId, string sEREncounterId, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        DataSet dsVitalData = new DataSet();
        hsTb.Add("@inyHospitalLocationId", HospitalId);
        hsTb.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        hsTb.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));
        hsTb.Add("@intEncounterId", EncounterId);

        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsTb.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }
        if (GroupingDate != "")
            hsTb.Add("@chrGroupingDate", GroupingDate);

        dsVitalData = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", hsTb);
        objtlc = null;

        DataView dv = new DataView(dsVitalData.Tables[0]);
        dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId;
        ds.Tables.Add(dv.ToTable());
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        dv.Dispose();
        sBeginSection = sBegin;
        sEndSection = sEnd;
        int t = 0;

        StringBuilder vital = new StringBuilder();

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (t == 0)
                {
                    if (common.myStr(sb).Contains("Vitals") == false)
                    {
                        //////  sb.Append("<br /><u><b>" + sb + "Vitals:  " + "</b></u><br /><br />" + sEndSection + BeginList);

                        if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                        {
                            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                            string BeginTemplateStyle = sBeginFont;
                            string EndTemplateStyle = sEndFont;
                            if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                            {
                                sb.Append(BeginTemplateStyle + "Vitals" + EndTemplateStyle);
                                sb.Append("<br/>");


                            }

                            else
                            {
                                sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                                sb.Append("<br/>");


                            }
                            sBeginFont = "";
                            sEndFont = "";
                        }



                    }
                    t = 1;
                }

                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append(BeginList);


            //"Vital Date","T","R","P","HT","WT","BPS","BPD","HC","MAC","BMI","BSA","Pain","T_ABNORMAL_VALUE"

            vital.Append("<table border='1' style='font-size:12px;' cellspacing='4px' >");
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

        ds.Dispose();
        hsTb = null;
        dsVitalData.Dispose();
        vital = null;
        return sb;
    }

    public StringBuilder BindDoctorProgressNote(string HospitalId, int RegistrationId, int DoctorId,
                                    StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                    Page pg, string pageID, string userID, string fromDate, string toDate, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        DataTable dtEnterByItem = new DataTable();
        DataView dvFilter = new DataView();
        DataView dvDoctorProgress = new DataView();
        try
        {
            hsTb.Add("@inyHospitalLocationId", HospitalId);
            hsTb.Add("@intLoginFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
            hsTb.Add("@intRegistrationId", common.myInt(RegistrationId));
            hsTb.Add("@intDoctorId", DoctorId);

            if (fromDate != "" && toDate != "")
            {
                hsTb.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd"));
                hsTb.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
            }
            if (GroupingDate != "")
                hsTb.Add("@chrGroupingDate", GroupingDate);
            ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetDoctorProgressNote", hsTb);
            objtlc = null;

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
                    if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                    {
                        BeginList = "<ul>"; EndList = "</ul>";
                    }
                    else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                    {
                        BeginList = "<ol>"; EndList = "</ol>";
                    }
                }
                sb.Append(BeginList);
                dvFilter = new DataView(ds.Tables[0]);
                dtEnterByItem = dvFilter.ToTable(true, "EncodedBy", "EnteredDate");
                foreach (DataRow row in dtEnterByItem.Rows)
                {
                    dvDoctorProgress = new DataView(dvFilter.ToTable());
                    dvDoctorProgress.RowFilter = "EncodedBy=" + row["EncodedBy"].ToString() + " AND EnteredDate='" + common.myStr(row["EnteredDate"]) + "'";
                    for (int i = 0; i < dvDoctorProgress.ToTable().Rows.Count; i++)
                    {
                        if (!common.myStr(ds.Tables[0].Rows[i]["ProgressNote"]).Trim().Equals(string.Empty))
                        {
                            sb.Append(i == 0 ? sBegin + common.myStr(dvDoctorProgress.ToTable().Rows[i]["ProgressNote"]).Trim() + sEnd : "<br/><br/>" + sBegin + common.myStr(dvDoctorProgress.ToTable().Rows[i]["ProgressNote"]).Trim() + sEnd);
                        }
                    }
                    sb.Append("<br/>" + DisplayUserNameInNote(common.myStr(dvDoctorProgress.ToTable().Rows[0]["EncodedBy"]), common.myInt(HospitalId), common.myStr(dvDoctorProgress.ToTable().Rows[0]["EnteredDate"]), drTemplateListStyle, pg));
                }

            }
            sb.Append(EndList);
        }
        catch (Exception ex)
        {
        }
        finally
        {
            //DlObj = null;
            ds.Dispose();
            dvFilter.Dispose();
            dtEnterByItem.Dispose();
            dvDoctorProgress.Dispose();
            hsTb = null;
        }

        return sb;
    }
    public StringBuilder BindImmunization(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string userID)
    {
        return BindImmunization(HospitalId, RegNo, EncounterID, sb, sbTemplateStyle, drTemplateListStyle, pg,
            pageID, userID, "", "", "");
    }

    public StringBuilder BindImmunization(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                string pageID, string userID, string fromDate, string toDate, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        string date = string.Empty;
        int i = 0;
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationID", HospitalId);
        hsTb.Add("@intRegistrationId ", RegNo);
        hsTb.Add("@intEncounterId", EncounterID);
        hsTb.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));

        if (fromDate != "" && toDate != "")
        {
            hsTb.Add("@chvFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsTb.Add("@chvToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }
        if (GroupingDate != "")
            hsTb.Add("@chrGroupingDate", GroupingDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunization", hsTb);
        objtlc = null;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        //MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        //MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {



            if (common.myStr(sbTemplateStyle).Contains("Immunization") == false)
            {
                //////sb.Append(sbTemplateStyle + "<u><b>" + "Immunization:" + "</b></u>" + BeginList);

                sb.Append(sbTemplateStyle + "<b>" + "Immunization " + "</b>" + BeginList);


            }


            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));


            if (common.myLen(ds.Tables[0].Rows[0]["GivenDate"]) > 0)
            {
                sb.Append(sbTemplateStyle);
                if (drTemplateListStyle != null)
                {
                    if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                    {
                        BeginList = "<ul>"; EndList = "</ul>";
                    }
                    else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                    {
                        BeginList = "<ol>"; EndList = "</ol>";
                    }
                }
                sb.Append("<br/>");
                sb.Append(BeginList);
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Equals(string.Empty))
                    {
                        sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                       + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"]) + ". " + sEnd + "<br/>");
                    }
                    else
                    {
                        sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                       + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"])
                        + ", Remarks: " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() + ". " + sEnd + "<br/>");
                    }
                }
            }
        }
        sb.Append(EndList);
        hsTb = null;
        ds.Dispose();
        return sb;
    }

    public StringBuilder BindInjection(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string userID)
    {
        return BindInjection(HospitalId, RegNo, EncounterID, sb, sbTemplateStyle, drTemplateListStyle, pg,
                     pageID, userID, "", "", "");
    }

    public StringBuilder BindInjection(string HospitalId, Int64 RegNo, int EncounterID, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string userID, string fromDate, string toDate, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
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
        if (GroupingDate != "")
            hsTb.Add("@chrGroupingDate", GroupingDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDailyInjections", hsTb);
        objtlc = null;

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        //////    MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ////// sb.Append(sbTemplateStyle + "<u><b>" + "Injection:" + "</b></u>" + BeginList);

            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;
            if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
            {
                sb.Append(BeginTemplateStyle + "Injection" + EndTemplateStyle);
                sb.Append("<br/>");

            }

            else
            {
                sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                sb.Append("<br/>");



            }
            sBeginFont = "";
            sEndFont = "";




            if (common.myLen(ds.Tables[0].Rows[0]["GivenDateTime"]) > 0)
            {
                sb.Append(sbTemplateStyle);
                if (drTemplateListStyle != null)
                {
                    if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                    {
                        BeginList = "<ul>"; EndList = "</ul>";
                    }
                    else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                    {
                        BeginList = "<ol>"; EndList = "</ol>";
                    }
                }
                sb.Append("<br/>");
                sb.Append(BeginList);
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Equals(string.Empty))
                    {
                        sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                       + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDateTime"]) + ". " + sEnd + "<br/>");
                    }
                    else
                    {
                        sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                       + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDateTime"])
                        + ", Remarks: " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() + ". " + sEnd + "<br/>");
                    }
                }
            }
        }
        sb.Append(EndList);
        hsTb = null;
        ds.Dispose();
        return sb;
    }

    public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, StringBuilder sb,
                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        return BindProblemsHPI(RegId, HospitalId, EncounterId, sb, sbTemplateStyle,
            drTemplateListStyle, pg, pageID, userID, "", "", "");
    }

    public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, StringBuilder sb,
                        StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                        string pageID, string userID, string fromDate, string toDate, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont =

"";
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
        if (GroupingDate != "")
            hsProblems.Add("@chrGroupingDate", GroupingDate);

        strSql = "Select ISNULL(r.FirstName,'') + ISNULL(' ' + r.MiddleName,'') + ISNULL(' ' + r.LastName,'') AS PatientName, dbo.AgeInYrsMonthDay (Convert(varchar(10),DateofBirth,111), Convert(Varchar(10), Getdate(),111)) as Age, dbo.GetGender(Gender) as Gender,";
        strSql = strSql + " c.Race from Registration r WITH (NOLOCK) Left Join RaceMaster c WITH (NOLOCK) On r.RaceId = c.RaceId";
        strSql = strSql + " where r.Id = @intRegistrationId ";
        strSql = strSql + " Select IsNull(IsPregnant,0) as IsPregnant, IsNull(IsBreastFeeding,0) as IsBreastFeeding from Encounter WITH (NOLOCK) where Id = @intEncounterId";

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", hsProblems);
        ds2 = objtlc.GetDataSet(CommandType.Text, strSql, hsProblems);
        objtlc = null;

        if (common.myLen(ds2.Tables[0].Rows[0]["Age"]) > 0)
        {
            //  strAge = common.myStr(ds2.Tables[0].Rows[0]["Age"]).Trim();
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (common.myStr(sbTemplateStyle).Contains("Chief Complaints") == false)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                {
                    MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    string BeginTemplateStyle = sBeginFont;
                    string EndTemplateStyle = sEndFont;

                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {

                        sb.Append(BeginTemplateStyle + "Chief Complaints" + EndTemplateStyle);



                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                    }
                    //sb.Append("<br/>");
                }
                sBeginFont = "";
                sEndFont = "";
                ////// sb.Append(sbTemplateStyle + "<u><b>" + "Chief Complaints:" + "</b></u>" + BeginList);
            }
            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId.ToString());

            DataView dvChronic = ds.Tables[0].DefaultView;
            dvChronic.RowFilter = "IsChronic='True'";
            dtChronic = dvChronic.ToTable();
            DataView dvNonChronic = ds.Tables[0].DefaultView;
            dvNonChronic.RowFilter = "IsChronic='False'";
            dtNonChronic = dvNonChronic.ToTable();


            dvChronic.Dispose();
            dvNonChronic.Dispose();
            if (dtChronic.Rows.Count > 0)
            {

                sbTemp.Append(sBeginFont);
                sbTemp.Append("<br/>");

            }
            else
            {
                sbTemp.Append(sBeginFont);
                sbTemp.Append("<br/>");

            }

        }





        //sbTemp.Append(BeginList); //cut from here 

        for (int i = 0; i < dtChronic.Rows.Count; i++)
        {

            DataRow dr1 = dtChronic.Rows[i] as DataRow;
            if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Quality2"]) != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
            {
                //Changed onsetDate to onset
                //Added this line to append string after if condition,previously it is above the if condition
                //Vineet
                if (common.myLen(dr1["ProblemDescription"]) > 0)
                {
                    //sbTemp.Append(sBegin + " The patient describes the chronic " + common.myStr(dr1["ProblemDescription"]).ToLower() + " ");

                    sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]) + "<br/>");
                    // sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]).ToLower() + "<br/>");
                }



            }



            sbTemp.Append(sEnd);
            //sbTemp.Append(EndList);// pasted here

        }



        for (int i = 0; i < dtNonChronic.Rows.Count; i++)
        {
            //sbTemp.Append(BeginList);// pasted here   
            DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
            //sbTemp.Append("<br /><br />" + Convert.ToString(dr1["ProblemDescription"]) + " presents with ");
            //sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");

            if (common.myStr(dr1["ProblemDescription"]).Trim() != "")

            //////   if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
            {
                //Changed onsetDate to onset
                //Added this line to append string after if condition,previously it is above the if condition
                //Vineet
                if (common.myStr(dr1["ProblemDescription"]).Trim() != "")
                {
                    sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]) + "<br/>");
                    //sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]).ToLower() + "<br/>");
                    //sbTemp.Append(sBegin + " The patient describes the " + common.myStr(dr1["ProblemDescription"]).ToLower() + " ");
                }


            }

            sbTemp.Append(sEnd);
            //sbTemp.Append(EndList);// pasted here      
        }


        //if (sbTemp.ToString() != "")
        //{
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows.Count == 0)
            {
                if (common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() != "")
                {
                    sb.Append(sbTemplateStyle + "<u><b>" + "Chief Complaints:" + "</b></u><br/>" + BeginList);
                }
            }
            sb.Append(sbTemplateStyle);

            if (common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() != "")
            {
                sbTemp.Append(sBeginFont + common.myStr(ds.Tables[1].Rows[0]["HPIRemarks"]).Trim() + sEndFont);
            }
        }
        sb.Append(sbTemp);
        ds.Dispose();

        sbTemp = null;
        fonts = null;
        hsProblems = null;
        dtChronic.Dispose();
        dtNonChronic.Dispose();
        dsProblem.Dispose();
        return sb;
    }

    public StringBuilder BindAssessments(int RegId, int HospitalId, int EncounterId, int UserId, string DocId,
                        StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID, int TemplateFieldId, string sEREncounterId)
    {
        return BindAssessments(RegId, HospitalId, EncounterId, UserId, DocId, sb, sbTemplateStyle,
                        drTemplateListStyle, pg, pageID, userID, "", "", TemplateFieldId, sEREncounterId, "");
    }

    public StringBuilder BindAssessments(int RegId, int HospitalId, int EncounterId, int UserId, string DocId,
                        StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID, string fromDate, string toDate, int TemplateFieldId,
        string sEREncounterId, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        DataSet ds1 = new DataSet();
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

        if (fromDate != "" && toDate != "")
        {
            hsAss.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsAss.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }
        if (GroupingDate != "")
            hsAss.Add("@chrGroupingDate", GroupingDate);
        dsDiagnosis = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hsAss);
        objtlc = null;
        DataView dv = new DataView(dsDiagnosis.Tables[0]);
        dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId;
        ds1.Tables.Add(dv.ToTable());


        DataView dv1 = new DataView(ds1.Tables[0]);
        dv1.RowFilter = "ISNULL(MRDCode,0)=0";
        ds.Tables.Add(dv1.ToTable());


        dv.Dispose();
        dv1.Dispose();
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;

            sb.Append(sbTemplateStyle);

            if (common.myStr(sbTemplateStyle).Contains("Diagnosis") == false)
            {
                ////// sb.Append(sbTemplateStyle + "<u><b>" + "Diagnosis:" + "</b></u>" + BeginList);

                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                {

                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Diagnosis" + EndTemplateStyle);
                        sb.Append("<br/>");
                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                }
            }
            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
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
        //if (cF == 1 || pF == 1 || sF == 1 || qF == 1)
        //{
        //    
        //}

        if (cF == 1)
        {
            sb.Append("<br />" + sBeginSection + "Chronic Diagnosis: " + sEndSection + "<br />");
            sbChronic.Append(EndList);
            sb.Append(sbChronic.ToString());
        }
        if (pF == 1)
        {
            sb.Append("<br />" + sBeginSection + "Primary Diagnosis: " + sEndSection + "<br />");
            sbPrimary.Append(EndList);
            sb.Append(sbPrimary.ToString());
        }
        if (sF == 1)
        {
            sb.Append("<br />" + sBeginSection + "Secondary Diagnosis: " + sEndSection + "<br />");
            sbSeconday.Append(EndList);
            sb.Append(sbSeconday.ToString());
        }
        if (qF == 1)
        {
            sb.Append("<br />" + sBeginSection + "Provisional Diagnosis: " + sEndSection + "<br />");
            sbQuery.Append(EndList);
            sb.Append(sbQuery.ToString());
        }


        ds.Dispose();
        ds1.Dispose();
        dsDiagnosis.Dispose();
        sbChronic = null;
        sbPrimary = null;
        sbSeconday = null;
        sbQuery = null;
        pF = 0;
        sF = 0;
        cF = 0;
        qF = 0;
        hsAss = null;

        return sb;
    }


    public StringBuilder BindTabularAssessments(int RegId, int HospitalId, int EncounterId, Int16 UserId, string DocId,
                       StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                       Page pg, string pageID, string userID, string fromDate, string toDate, int TemplateFieldId,
       string sEREncounterId, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet dsDiagnosis = new DataSet();

        StringBuilder sbDiagnosis = new StringBuilder();

        Hashtable hsAss = new Hashtable();
        hsAss.Add("@intRegistrationId", RegId);
        hsAss.Add("@inyHospitalLocationID", HospitalId);
        hsAss.Add("@intEncounterId", EncounterId);

        if (fromDate != "" && toDate != "")
        {
            hsAss.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd 00:00")); //yyyy-MM-dd 00:00
            hsAss.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy-MM-dd 23:59"));
        }
        if (GroupingDate != "")
            hsAss.Add("@chrGroupingDate", GroupingDate);
        dsDiagnosis = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hsAss);
        objtlc = null;

        DataView dv = new DataView(dsDiagnosis.Tables[0]);
        dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId;
        ds1.Tables.Add(dv.ToTable());


        DataView dv1 = new DataView(ds1.Tables[0]);
        dv1.RowFilter = "ISNULL(MRDCode,0)=0";
        ds.Tables.Add(dv1.ToTable());


        dv.Dispose();
        dv1.Dispose();
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;

            sb.Append(sbTemplateStyle);

            if (common.myStr(sbTemplateStyle).Contains("Diagnosis") == false)
            {
                ////// sb.Append(sbTemplateStyle + "<u><b>" + "Diagnosis:" + "</b></u>" + BeginList);

                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                {

                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Diagnosis" + EndTemplateStyle);
                        sb.Append("<br/>");
                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                }
            }
            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }

        }

        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());

        sbDiagnosis.Append("<table border='1' cellpadding='3' cellspacing='2'>");

        sbDiagnosis.Append("<tr>");
        sbDiagnosis.Append("<td colspan='2' border='1' style='text-align: center;'><b>" + sBeginSection + "ICDCODE" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='4' border='1' style='text-align: center;'><b>" + sBeginSection + "DIAGNOSIS" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: center;'><b>" + sBeginSection + "Provisonal/Final Diagnosis" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: center;'><b>" + sBeginSection + "PRIMARY/SECONDARY" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: center;'><b>" + sBeginSection + "CHRONIC" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='4' border='1' style='text-align: center;'><b>" + sBeginSection + "REMARKS" + sEndSection + "</b></td>");
        sbDiagnosis.Append("</tr>");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {

            string strCHRONIC = string.Empty;
            string strPrimaryDiagnosis = string.Empty;

            if (common.myBool(dr["PrimaryDiagnosis"]))
            {
                strPrimaryDiagnosis = "Primary";
            }
            else
            {
                strPrimaryDiagnosis = "Secondary";
            }

            if (common.myBool(dr["IsChronic"]))
            {
                strCHRONIC = "Chronic";
            }

            sbDiagnosis.Append("<tr>");
            sbDiagnosis.Append("<td colspan='2' valign='top' border='1' style='text-align: center;'>" + sBeginSection + common.myStr(dr["ICDCode"]) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='4' valign='top' border='1'>" + sBeginSection + common.myStr(dr["ICDDescription"]) + sEndSection + "<span style=' font-size:7pt; color: #000000; font-family: Arial; '></span>  </td>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(dr["ProvisionalFinalDiagnosis"]) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(strPrimaryDiagnosis) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(strCHRONIC) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='4' valign='top' border='1'>" + sBeginSection + common.myStr(dr["Remarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br />") + sEndSection + "</td>");
            sbDiagnosis.Append("</tr>");
        }

        sBegin = "";
        sEnd = "";
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;

        ds.Dispose();
        ds1.Dispose();
        dsDiagnosis.Dispose();

        hsAss = null;

        return sb;
    }
    public string BindVisitNotesDiagnosis(int iHospitalLocationId, int iFacilityId, int iRegistrationId, string sFromDate, string sToDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        DataSet dsObj = new DataSet();
        DataView dvObj = new DataView();
        hstInput = new Hashtable();
        DataTable dtEncounterId = new DataTable();
        DataTable dtCodingType = new DataTable();
        try
        {
            hstInput.Add("@inyHospitalLocationId", iHospitalLocationId);
            hstInput.Add("@intFacilityId", iFacilityId);
            hstInput.Add("@intRegistrationId", iRegistrationId);
            if (sFromDate != "")
                hstInput.Add("@dtFromDate", sFromDate);
            if (sToDate != "")
                hstInput.Add("@dtToDate", sToDate);

            dsObj = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosisOPIP", hstInput);
            dvObj = new DataView(dsObj.Tables[0]);
            dtEncounterId = dvObj.ToTable(true, "EncounterId");
            dtCodingType = dvObj.ToTable(true, "EncounterId", "CodingType");
            string strObj = string.Empty;
            for (int it = 0; it < dtEncounterId.Rows.Count; it++)
            {
                objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'><strong>Visit Date:</strong> " + dsObj.Tables[0].Rows[it]["VisitDate"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Doctor Name:</strong> " + dsObj.Tables[0].Rows[it]["DoctorName"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Visit Type:</strong> " + dsObj.Tables[0].Rows[it]["VisitType"].ToString() + "</span>");
                objStrTmp.Append("<br/>");
                for (int itt = 0; itt < dtCodingType.Rows.Count; itt++)
                {
                    objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'><strong>" + dtCodingType.Rows[itt]["CodingType"].ToString() + "</strong></span>");
                    objStrTmp.Append("<br/>");
                    dvObj.RowFilter = "EncounterId = '" + dtEncounterId.Rows[it]["EncounterId"].ToString() + "'and CodingType='" + dtCodingType.Rows[itt]["CodingType"].ToString() + "'";
                    break; // ONLY ONE CodingType ('ICD Codes') EXIST IN DATABASE
                }
                foreach (DataRow dr in ((DataTable)dvObj.ToTable()).Rows)
                {
                    objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'>");
                    objStrTmp.Append(common.myStr(dr["ICDCode"]) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(dr["Description"]));
                    objStrTmp.Append("</span>");
                    objStrTmp.Append("<br/>");
                }
                objStrTmp.Append("<br/>");
                objStrTmp.Append("<br/><br/>");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objtlc = null;
            objStrSettings = null;
            dsObj.Dispose();
            dvObj.Dispose();
            hstInput = null;
            dtEncounterId.Dispose();
            dtCodingType.Dispose();
        }
        return objStrTmp.ToString();
    }
    //public string bindVisitNotesPharmacy(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId, string sFromDate, string sToDate)
    //{
    //    BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
    //    StringBuilder objStrTmp = new StringBuilder();
    //    StringBuilder objStrSettings = new StringBuilder();
    //    DataSet dsObj = new DataSet();
    //    DataView dvObj = new DataView();
    //    hstInput = new Hashtable();
    //    DataTable dtPharmacy = new DataTable();
    //    try
    //    {
    //        hstInput.Add("@IntHospitalLocId", iHospitalLocationId);
    //        hstInput.Add("@IntRegistrationid", iRegistrationId);
    //        hstInput.Add("@IntFacilityId", iFacilityId);
    //        hstInput.Add("@intEncounterId", iEncounterId);
    //        if (sFromDate != "")
    //            hstInput.Add("@Fdate", sFromDate);
    //        if (sToDate != "")
    //            hstInput.Add("@Tdate", sToDate);


    //        dsObj = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEmrGetCombinedPrescription", hstInput);
    //        dvObj = new DataView(dsObj.Tables[0]);
    //        dtPharmacy = dvObj.ToTable(true, "UniqueIndentNo");
    //        string strObj = string.Empty;
    //        for (int it = 0; it < dtPharmacy.Rows.Count; it++)
    //        {
    //            objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'><strong>Visit Date:</strong> " + dsObj.Tables[0].Rows[it]["VisitDate"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Doctor Name:</strong> " + dsObj.Tables[0].Rows[it]["DoctorName"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Visit Type:</strong> " + dsObj.Tables[0].Rows[it]["VisitType"].ToString() + "</span>");
    //            objStrTmp.Append("<br/>");
    //            dvObj.RowFilter = "UniqueIndentNo = '" + dtPharmacy.Rows[it]["UniqueIndentNo"].ToString() + "'";
    //            foreach (DataRow dr in dvObj.ToTable().Rows)
    //            {
    //                objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'>");
    //                objStrTmp.Append(common.myStr(dr["Medicine"]));
    //                objStrTmp.Append("</span>");
    //                objStrTmp.Append("<br/>");
    //            }
    //            objStrTmp.Append("<br/>");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    finally
    //    {
    //        objtlc = null;
    //        objStrSettings = null;
    //        dsObj.Dispose();
    //        dvObj.Dispose();
    //        hstInput = null;
    //    }
    //    return objStrTmp.ToString();
    //}
    public string bindVisitNotesPharmacy(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId, string sFromDate, string sToDate)
    {
        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        DataSet dsObj = new DataSet();
        DataView dvObj = new DataView();
        hstInput = new Hashtable();
        DataTable dtPharmacy = new DataTable();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        try
        {
            hstInput.Add("@IntHospitalLocId", iHospitalLocationId);
            hstInput.Add("@IntRegistrationid", iRegistrationId);
            hstInput.Add("@IntFacilityId", iFacilityId);
            hstInput.Add("@intEncounterId", iEncounterId);
            if (sFromDate != "")
                hstInput.Add("@Fdate", sFromDate);
            if (sToDate != "")
                hstInput.Add("@Tdate", sToDate);


            dsObj = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEmrGetCombinedPrescription", hstInput);
            dvObj = new DataView(dsObj.Tables[0]);
            dtPharmacy = dvObj.ToTable(true, "UniqueIndentNo");

            string strObj = string.Empty;
            for (int it = 0; it < dtPharmacy.Rows.Count; it++)
            {
                dvObj.RowFilter = "UniqueIndentNo='" + common.myStr(dtPharmacy.Rows[it]["UniqueIndentNo"]) + "'";

                if (dvObj.ToTable().Rows.Count > 0)
                {
                    objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'><strong>Visit Date:</strong> " +
                                        common.myStr(dvObj.ToTable().Rows[0]["VisitDate"]) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Doctor Name:</strong> " +
                                        common.myStr(dvObj.ToTable().Rows[0]["DoctorName"]) + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Visit Type:</strong> " +
                                        common.myStr(dvObj.ToTable().Rows[0]["VisitType"]) + "</span>");

                    objStrTmp.Append("<br/>");

                    foreach (DataRow dr in dvObj.ToTable().Rows)
                    {
                        objStrTmp.Append("<span style='font-size:12;font-family:Courier New;'>");
                        objStrTmp.Append(common.myStr(dr["Medicine"]) + common.myStr(dr["PrescriptionDetail"]));

                        if (common.myLen(dr["InstructionRemarks"]) > 0)
                        {
                            objStrTmp.Append(", Instructions: ");
                            objStrTmp.Append(common.myStr(dr["InstructionRemarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br />"));
                        }

                        objStrTmp.Append("</span>");
                        objStrTmp.Append("<br/>");
                    }
                    objStrTmp.Append("<br/>");
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            objtlc = null;
            objStrSettings = null;
            dsObj.Dispose();
            dvObj.Dispose();
            hstInput = null;
        }
        return objStrTmp.ToString();
    }
    //public StringBuilder BindPatientProvisionalDiagnosis(int RegId, int HospitalId, int EncounterId, int UserId, string DocId,
    //                    StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
    //                    Page pg, string pageID, string userID, string fromDate, string toDate, int
    //    TemplateFieldId, string sEREncounterId, string GroupingDate)
    //{
    //    StringBuilder sbProvisional = new StringBuilder();
    //    BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
    //    ds = new DataSet();
    //    try
    //    {
    //        string sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
    //        ds = objDiag.GetPatientProvisionalDiagnosis(HospitalId, RegId, EncounterId, UserId, fromDate, toDate, GroupingDate);
    //        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
    //        sBeginSection = sBegin;
    //        sEndSection = sEnd;

    //        ////if (ds.Tables[0].Rows.Count > 0)
    //        ////{
    //        ////    sb.Append("<br />" + sBeginSection + "Provisional Diagnosis: " + sEndSection + "<br />");

    //        ////    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //        ////    {
    //        ////        sbProvisional = new StringBuilder();
    //        ////        //sbProvisional.Append(ds.Tables[0].Rows[i]["ProvisionalDiagnosis"].ToString());
    //        ////        sbProvisional.Append(sBeginSection + ds.Tables[0].Rows[i]["ProvisionalDiagnosis"].ToString() + sEndSection);
    //        ////     //////   sbProvisional.Append(DisplayUserNameInNote(ds.Tables[0].Rows[i]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[i]["EncodedDate"].ToString(), drTemplateListStyle, pg));
    //        ////        sb.Append(sbProvisional);
    //        ////    }
    //        ////}
    //        ////sb.Append("<br/>");



    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            sb.Append("<br />" + sBeginSection + "Provisional Diagnosis " + sEndSection + "<br />");
    //            sbProvisional.Append(sBeginSection + common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]) + sEndSection);
    //            sb.Append(sbProvisional);
    //            sb.Append("<br/>");
    //            //////    sbProvisional.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EnteredBy"].ToString(), HospitalId, ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg));


    //        }
    //        sBeginSection = "";
    //        sEndSection = "";
    //    }
    //    catch (Exception Ex)
    //    {
    //    }
    //    finally
    //    {
    //        ds.Dispose();
    //        objDiag = null;
    //        sbProvisional = null;
    //    }
    //    return sbProvisional;
    //}


    public StringBuilder BindPatientProvisionalDiagnosis(int RegId, int HospitalId, int EncounterId, int UserId, string DocId,
                    StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                    Page pg, string pageID, string userID, string fromDate, string toDate, int
                    TemplateFieldId, string sEREncounterId, string GroupingDate)
    {
        StringBuilder sbProvisional = new StringBuilder();
        BaseC.DiagnosisDA objDiag = new BaseC.DiagnosisDA(sConString);
        ds = new DataSet();
        try
        {
            string sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
            ds = objDiag.GetPatientProvisionalDiagnosis(HospitalId, RegId, EncounterId, UserId, fromDate, toDate, GroupingDate);
            MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
            sBeginSection = sBegin;
            sEndSection = sEnd;

            ////if (ds.Tables[0].Rows.Count > 0)
            ////{
            ////    sb.Append("<br />" + sBeginSection + "Provisional Diagnosis: " + sEndSection + "<br />");

            ////    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            ////    {
            ////        sbProvisional = new StringBuilder();
            ////        //sbProvisional.Append(ds.Tables[0].Rows[i]["ProvisionalDiagnosis"].ToString());
            ////        sbProvisional.Append(sBeginSection + ds.Tables[0].Rows[i]["ProvisionalDiagnosis"].ToString() + sEndSection);
            ////     //////   sbProvisional.Append(DisplayUserNameInNote(ds.Tables[0].Rows[i]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[i]["EncodedDate"].ToString(), drTemplateListStyle, pg));
            ////        sb.Append(sbProvisional);
            ////    }
            ////}
            ////sb.Append("<br/>");



            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    sb.Append("<br />" + sBeginSection + "Provisional Diagnosis " + sEndSection + "<br />");
            //    sbProvisional.Append(sBeginSection + common.clearHTMLTags(ds.Tables[0].Rows[0]["ProvisionalDiagnosis"]) + sEndSection);
            //    sb.Append(sbProvisional);
            //    sb.Append("<br/>");
            //    //////    sbProvisional.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EnteredBy"].ToString(), HospitalId, ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg));


            //}

            if (ds.Tables[0].Rows.Count > 0)
            {
                sBegin = "";
                sEnd = "";
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                {
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    string BeginTemplateStyle = sBegin;
                    string EndTemplateStyle = sEnd;
                    //MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Provisional Diagnosis" + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");


                    }
                    sBegin = "";
                    sEnd = "";
                }



                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                sBeginSection = sBegin;
                sEndSection = sEnd;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sbProvisional = new StringBuilder();
                    //sbProvisional.Append(ds.Tables[0].Rows[i]["ProvisionalDiagnosis"].ToString());
                    sbProvisional.Append(sBeginSection + ds.Tables[0].Rows[i]["ProvisionalDiagnosis"].ToString() + sEndSection);
                    //   sbProvisional.Append(DisplayUserNameInNote(ds.Tables[0].Rows[i]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[i]["EncodedDate"].ToString(), drTemplateListStyle, pg));
                    sb.Append(sbProvisional);
                    sb.Append("<br/>");
                }
                // sb.Append("<br/>");
            }
            sBeginSection = "";
            sEndSection = "";
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            ds.Dispose();
            objDiag = null;
            sbProvisional = null;
        }
        return sbProvisional;
    }


    public StringBuilder BindAllergies(int RegId, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string hospitalID, string userID, string PageID, int TemplateFieldId)
    {
        return BindAllergies(RegId, sb, sbTemplateStyle, drTemplateListStyle, pg, hospitalID, userID, PageID, "", "", TemplateFieldId, "");
    }

    public StringBuilder BindAllergies(int RegId, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string hospitalID, string userID, string PageID,
        string fromDate, string toDate, int TemplateFieldId, string GroupingDate)
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
        hsAllergy.Add("@chrGroupingDate", GroupingDate);

        dsAllergy = objtlc.GetDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", hsAllergy);
        objtlc = null;

        DataView dv = new DataView(dsAllergy.Tables[0]);
        //dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId;
        ds.Tables.Add(dv.ToTable());

        dv.Dispose();

        DataView dvDrug = new DataView(ds.Tables[0]);
        dvDrug.RowFilter = "AllergyType IN ('Drug','CIMS','VIDAL')";

        DataView dvOther = new DataView(ds.Tables[0]);
        dvOther.RowFilter = "AllergyType NOT IN ('Drug','CIMS','VIDAL')";

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (sbTemplateStyle.ToString().Contains("Allergies") == false)
            {
                //sb.Append(sbTemplateStyle + "<u><b>" + "Allergies:" + "</b></u>" + BeginList);


                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                {
                    MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    string BeginTemplateStyle = sBeginFont;
                    string EndTemplateStyle = sEndFont;

                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Allergies" + EndTemplateStyle);
                        //sb.Append("<br/>");


                    }

                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        //sb.Append("<br/>");

                    }
                }
                sBeginFont = "";
                sEndFont = "";




            }
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                sBeginSection = sBegin;
                sEndSection = sEnd;
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
        }
        if (dsAllergy.Tables[1].Rows.Count > 0)
        {
            //if (Convert.ToString(dsAllergy.Tables[1].Rows[0]["NoAllergies"]) == "True")
            //{
            //    sb.Append(sbTemplateStyle + "<u><b>" + "Allergies: " + "</b></u>" + BeginList);
            //}
            if (Convert.ToString(dsAllergy.Tables[1].Rows[0]["NoAllergies"]) != "True")
            {
                foreach (DataRowView dr in dvDrug)
                {
                    if (t == 0)
                    {

                        if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                        {
                            sbDrugAllergy.Append("<br />" + sBeginSection + "Drug Allergies <br />" + sEndSection + BeginList);
                        }
                        else
                        {

                            sbDrugAllergy.Append(sBeginSection + "Drug Allergies <br />" + sEndSection + BeginList);
                        }
                        t = 1;
                        //////sbDrugAllergy.Append("<br />" + sBeginSection + "Drug Allergies <br />" + sEndSection + BeginList);
                        //////t = 1;
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

                    sbDrugAllergy.Append(sEnd + "<br />");
                }
                sbDrugAllergy.Append(EndList);
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["NoAllergyDisplay"] == null)
                {
                    sbDrugAllergy.Append(sbTemplateStyle);
                    sbDrugAllergy.Append("<br />" + BeginList + sBegin + " No Allergies." + sEnd + EndList + "<br />");
                    System.Web.HttpContext.Current.Session["NoAllergyDisplay"] = false;
                }
            }
        }
        t = 0;
        foreach (DataRowView dr in dvOther)
        {
            if (t == 0)
            {
                sbOtherAllergy.Append("<br />" + sBeginSection + "Food/ Other Allergies <br />" + sEndSection + BeginList);
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
            sbOtherAllergy.Append(sEnd + "<br />");
        }
        sbOtherAllergy.Append(EndList);
        sb.Append(sbDrugAllergy);
        sb.Append(sbOtherAllergy);

        ds.Dispose();
        dsAllergy.Dispose();
        hsAllergy = null;
        sbDrugAllergy = null;
        sbOtherAllergy = null;
        dvDrug.Dispose();
        dvOther.Dispose();
        return sb;
    }
    public StringBuilder BindMedication(int EncounterId, int HospitalId, int RegId, StringBuilder sb,
                        StringBuilder sbTemplateStyle, string MedicationType, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID)
    {
        return BindMedication(EncounterId, HospitalId, RegId, sb, sbTemplateStyle, MedicationType,
            drTemplateListStyle, pg, pageID, userID, "", "", "", "");
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

        dtCurrentMedication = ds2.Tables[0];
        dtPriscription = ds.Tables[0];
        if (drTemplateListStyle != null)
        {
            if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
            {
                BeginList = "<ul>"; EndList = "</ul>";
            }
            else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
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
                Hashtable hsIn = new Hashtable();
                hsIn.Add("@intEncounterId", common.myInt(EncounterId));

                string Squery = "SELECT ISNULL(NoCurrentMedication,0) AS NoCurrentMedication FROM Encounter WITH (NOLOCK) WHERE Id=@intEncounterId ";
                DataSet dsNoMed = objtlc.GetDataSet(CommandType.Text, Squery, hsIn);

                if (dsNoMed.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(dsNoMed.Tables[0].Rows[0]["NoCurrentMedication"]) == "True")
                    {
                        sb.Append(sbTemplateStyle);
                        sb.Append(BeginList + sBegin + "</br> None." + sEnd + EndList);
                    }
                }

            }
        }
        //sb.Append("</ul>");
        objtlc = null;

        return sb;
    }

    public StringBuilder BindMedication(int EncounterId, int HospitalId, int RegId, StringBuilder sb,
                   StringBuilder sbTemplateStyle, string MedicationType, DataRow drTemplateListStyle,
                   Page pg, string pageID, string userID, string fromDate, string toDate, string OPIP, string GroupingDate)
    {
        DataSet ds = new DataSet();
        DataView dv = new DataView();
        DataView dv1 = new DataView();

        DataTable dtEnterByItem = new DataTable();
        DataView dvEnterByItem = new DataView();
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        StringBuilder sbPrescribed = new StringBuilder();
        string sFromDate = "";
        string sToDate = "";
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        BaseC.clsPharmacy objPharmacy = new BaseC.clsPharmacy(sConString);
        Hashtable HshIn = new Hashtable();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        try
        {
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chvPreviousMedication", "A");
            if (fromDate != "" && toDate != "")
            {
                HshIn.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd"));
                HshIn.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
            }
            if (GroupingDate != "")
                HshIn.Add("@chrGroupingDate", GroupingDate);
            if (OPIP == "I")
            {
                ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", HshIn);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetOPMedicines", HshIn); // PROC. CALL FOR OP CASE
                }
            }
            else
            {
                ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetOPMedicines", HshIn);
            }

            dv = new DataView(ds.Tables[0]);
            dv1 = new DataView(ds.Tables[1]);


            dv.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ItemCategoryShortName IN ('MED')";
            // dv1.RowFilter = "ISNULL(DoctorCategory,0)=1  AND ItemCategoryShortName IN ('MED')";

            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }

            if (dv.ToTable() != null)
            {
                if (dv.ToTable().Rows.Count > 0)
                {
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    string BeginTemplateStyle = sBegin;
                    string EndTemplateStyle = sEnd;
                    if (common.myStr(sbTemplateStyle).Contains("Prescription") == false)
                    {

                        //////if (common.myInt(drTemplateListStyle["TemplateUnderline"]).Equals(1))
                        //////{

                        //////    sb.Append(sbTemplateStyle + "<u><b>" + "Prescription" + "</b></u>" + BeginList);
                        //////}
                        //////else
                        //////{
                        //////    sb.Append(sbTemplateStyle + "<b>" + "Prescription" + "</b>" + BeginList);

                        //////}

                        if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                        {

                            if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                            {

                                sb.Append(BeginTemplateStyle + "Medications" + EndTemplateStyle);
                                sb.Append("<br/>");

                            }
                            else
                            {
                                sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                                sb.Append("<br/>");



                            }
                        }

                    }
                    sBegin = ""; ;
                    sEnd = "";

                    //////sb.Append("<br />" + sBeginSection + "<u><b>" + "Medications Prescribed:<br/>" + "</b></u>" + sEndSection);
                    //////sbPrescribed.Append(EndList);

                    dtEnterByItem = dv.ToTable(true, "EncodedBy");
                    foreach (DataRow row in dtEnterByItem.Rows)
                    {
                        dvEnterByItem = new DataView(dv.ToTable());
                        dvEnterByItem.RowFilter = "EncodedBy=" + row["EncodedBy"].ToString();

                        foreach (DataRowView dr in dvEnterByItem)
                        {
                            sbPrescribed.Append(sBegin + " " + dr["ItemName"].ToString() + " : ");
                            DataView dvFilter = new DataView(dv1.ToTable());
                            if (common.myBool(dr["CustomMedication"]) == true)
                            {
                                dvFilter.RowFilter = "ISNULL(DetailsId,0)=" + dr["DetailsId"].ToString() + " AND IndentId =" + dr["IndentId"].ToString();
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(ItemId,0)=" + dr["ItemId"].ToString() + " AND IndentId =" + dr["IndentId"].ToString();
                            }
                            string sPrescriptionString = objEMR.GetPrescriptionDetailString(dvFilter.ToTable());
                            sbPrescribed.Append(sPrescriptionString);
                            sbPrescribed.Append(sEnd + "<br />");
                            dvFilter.Dispose();
                        }
                        sbPrescribed.Append(DisplayUserNameInNote(dvEnterByItem.ToTable().Rows[0]["EncodedBy"].ToString(), HospitalId, dvEnterByItem.ToTable().Rows[0]["EntryDate"].ToString(), drTemplateListStyle, pg));
                    }
                }
            }
            sb.Append(sbPrescribed);
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dtEnterByItem.Dispose();
            dvEnterByItem.Dispose();
            objEMR = null;
            objPharmacy = null;
            HshIn = null;
            objtlc = null;
        }
        return sb;
    }

    public StringBuilder BindOrders(Int64 RegNo, int HospitalId, int EncounterId, int UserId, string DocId,
                            StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string userID, string sEREncounterId)
    {
        return BindOrders(RegNo, HospitalId, EncounterId, UserId, DocId, sb, sbTemplateStyle, drTemplateListStyle,
                            pg, pageID, userID, "", "", sEREncounterId, "");
    }

    public StringBuilder BindOrders(Int64 RegNo, int HospitalId, int EncounterId, int UserId, string DocId,
                            StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string userID, string fromDate, string toDate, string sEREncounterId, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        DataTable dtmain = new DataTable();
        ds = new DataSet();
        DataTable dt = new DataTable();
        StringBuilder sbTemp = new StringBuilder();
        Hashtable hsOrd = new Hashtable();
        hsOrd.Add("@intRegistrationId", RegNo);
        hsOrd.Add("@inyHospitalLocationID", HospitalId);
        hsOrd.Add("@intEncounterId", EncounterId);
        hsOrd.Add("@intFacilityId", System.Web.HttpContext.Current.Session["FacilityId"]);

        if (fromDate != "" && toDate != "")
        {
            hsOrd.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd")); //yyyy-MM-dd 00:00
            hsOrd.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }
        if (GroupingDate != "")
            hsOrd.Add("@chrGroupingDate", GroupingDate);

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hsOrd);
        objtlc = null;

        DataView dvmain = new DataView(ds.Tables[0]);
        dvmain.RowFilter = "ServiceType Not in ( 'CL','VS','VF','R','RT','RF','O')";
        dvmain.Sort = "DepartmentId,Id";

        dtmain = dvmain.ToTable();
        HiddenField hdnEncodedBy = new HiddenField();

        if (dtmain.Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;

            if (common.myStr(sbTemplateStyle).Contains("Order") == false)
            {
                //////  sb.Append(sbTemplateStyle + "<u><b>" + "Order And Procedures:" + "<br/></b></u>" + BeginList);



                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
                {
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {

                        //sb.Append(sbTemplateStyle + "<u><b>" + "Order And Procedures" + "<br/></b></u>" + BeginList);
                        sb.Append(BeginTemplateStyle + "Orders" + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                    else
                    {
                        //sb.Append("<br/>");
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");
                        //sb.Append(sbTemplateStyle + "<u><b>" + common.myStr(drTemplateListStyle["StaticTemplateName"]) + "<br/></b></u>" + BeginList);

                    }
                }


            }
            sBeginFont = "";
            sEndFont = "";
            //////  sb.Append(sbTemplateStyle);

            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
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
            dvEncoded.RowFilter = "EncodedBy=" + common.myInt(dtmain.Rows[i]["EncodedBy"]);
            DataTable dtEncoded = dvEncoded.ToTable();
            if (dtEncoded.Rows.Count == dtmain.Rows.Count)
            {
                bResult = true;
                hdnSameEncodedBy.Value = dtmain.Rows[i]["EncodedBy"].ToString();
                hdnSameEncodedDate.Value = dtmain.Rows[i]["EntryDate"].ToString();
            }
            dvEncoded.Dispose();
            dtEncoded.Dispose();
            DataRow dr = dtmain.Rows[i] as DataRow;
            if (common.myInt(dr["DepartmentId"]) != DepartmentId)
            {
                //////sbTemp.Append(sBeginSection + common.myStr(dr["DepartmentName"]) + ": " + sEndSection);

                sbTemp.Append(sBeginSection + common.myStr(dr["DepartmentName"]) + "<br/> " + sEndSection);
                sbTemp.Append(BeginList);
                DepartmentId = common.myInt(dr["DepartmentId"]);
                DataView dv = new DataView(dtmain);
                dv.RowFilter = "DepartmentId=" + common.myInt(dr["DepartmentId"]);
                dt = dv.ToTable();
                dv.Dispose();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    string commase = "";
                    if (j != dt.Rows.Count - 1)
                    {
                        commase = ", ";
                    }

                    DataRow dr1 = dt.Rows[j] as DataRow;
                    //////if (dr["CPTCode"].ToString() != "")
                    //////{
                    //////    //sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + " (CPT Code:" + common.myStr(dr1["CPTCode"]) + ")" + commase + sEnd);

                    ////// sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);

                    //////}
                    //////else
                    //////{
                    //////  sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);


                    //////}

                    string sRemarks = common.myStr(dr1["Remarks"]) != "" ? " Remarks: " + common.myStr(dr1["Remarks"]) : "";

                    if (dr1["CPTCode"].ToString() != "")
                    {
                        sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + sRemarks + "<br/>" + sEnd);
                    }
                    else
                    {
                        sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + sRemarks + "<br/>" + sEnd);
                    }


                }

                sbTemp.Append(EndList + "<br/>");
                if (bResult == false)
                {
                    //////sbTemp.Append(EndList + "<br/>");
                    //////sbTemp.Append(EndList + "");
                    //////sbTemp.Append(DisplayUserNameInNote(dr["EncodedBy"].ToString(), HospitalId, dr["EntryDate"].ToString(), drTemplateListStyle, pg));
                }
            }
        }
        if (bResult == true)
        {
            //////sbTemp.Append(EndList + "<br/>");
            //////sbTemp.Append(DisplayUserNameInNote(hdnSameEncodedBy.Value, HospitalId, hdnSameEncodedDate.Value, drTemplateListStyle, pg));
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
        dtEx.Dispose();
        dvExcluded.Dispose();
        dtExcluded.Dispose();
        dvmain.Dispose();
        dtmain.Dispose();
        ds.Dispose();
        dt.Dispose();
        sbTemp = null;
        hsOrd = null;
        return sb;
    }
    public StringBuilder BindLabTestResultReport(int RegistrationId, int HospitalId, int EncounterId,
        StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, bool bShowAllParameters)
    {
        ds = new DataSet();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "";
        BaseC.clsLISLabOther lis = new BaseC.clsLISLabOther(sConString);
        StringBuilder sbTemp = new StringBuilder();

        ds = lis.GetLabTestResultForNote(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                        common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), 0, EncounterId, RegistrationId,
                                        common.myInt(System.Web.HttpContext.Current.Session["UserId"]), bShowAllParameters);

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
        }
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        sBegin = ""; sEnd = "";
        if (ds.Tables.Count > 0)
        {
            int subDeptId = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                sbTemp.Append("<br/><br/>");
                sbTemp.Append("<table border='0' width='100%' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;'>");
                sbTemp.Append("<tr>");
                sbTemp.Append("<td><b>" + sBegin + "Test" + sEnd + "</b></td>");
                sbTemp.Append("<td><b>" + sBegin + "Value" + sEnd + "</b></td>");
                sbTemp.Append("<td><b>" + sBegin + "Unit" + sEnd + "</b></td>");
                sbTemp.Append("<td><b>" + sBegin + "Ideal Range" + sEnd + "</b></td>");
                sbTemp.Append("</tr>");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (common.myInt(dr["SubDeptId"]) != subDeptId)
                    {
                        DataView dv = new DataView(ds.Tables[0]);
                        dv.RowFilter = "SubDeptId=" + common.myInt(dr["SubDeptId"]);
                        DataTable dt = dv.ToTable();

                        dv.Dispose();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string sBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "<b>" : "";
                            string eBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "</b>" : "";

                            string sDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "<b>" : "";
                            string eDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "</b>" : "";

                            sbTemp.Append(BeginList);
                            sbTemp.Append("<tr>");

                            if (dt.Rows[i]["FieldType"].ToString().Trim() == "SN")
                            {
                                if (dt.Rows[i]["FieldType"].ToString().Trim() == "N" || dt.Rows[i]["FieldType"].ToString().Trim() == "F" || dt.Rows[i]["FieldType"].ToString().Trim() == "TM")
                                {
                                    sbTemp.Append("<td style='border-style:none;width:400px;'>" + sDEPBold + dt.Rows[i]["FieldName"].ToString() + eDEPBold + "</td><td style='border-style: none;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + sEnd + eBold + "</td><td style='border-style: none;'> " + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + "</td><td style='border-style: none;'>   (" + common.myStr(dt.Rows[i]["MinValue"]) + " - " + common.myStr(dt.Rows[i]["MaxValue"]) + sEnd + ") </td>");
                                }
                                else
                                {
                                    sbTemp.Append("<td style='border-style:none;' colspan='4'><b>" + dt.Rows[i]["FieldName"].ToString() + "</b></td>");
                                }
                                sbTemp.Append("</tr>");

                                sbTemp.Append("<tr>");
                                DataSet dsSen = lis.GetDiagLabSensitivityResultForNote(common.myInt(dt.Rows[i]["DiagSampleId"]), common.myInt(dt.Rows[i]["ResultId"]));
                                if (dsSen.Tables[0].Rows.Count > 0)
                                {
                                    sbTemp.Append("<td colspan='4'>");
                                    sbTemp.Append("<table border='0' width='100%' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;'>");
                                    sbTemp.Append("<tr>");
                                    int count = 0;
                                    for (int l = 0; l < dsSen.Tables[0].Columns.Count; l++)
                                    {
                                        sbTemp.Append("<td width='20%' ><b>" + dsSen.Tables[0].Columns[l].ColumnName + "</b></td>");
                                        count++;
                                    }
                                    sbTemp.Append("</tr>");

                                    for (int k = 0; k < dsSen.Tables[0].Rows.Count; k++)
                                    {
                                        sbTemp.Append("<tr>");
                                        for (int j = 0; j < count; j++)
                                        {
                                            if (dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName].ToString().Contains("<B>") == false && dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName].ToString() != null)
                                            {
                                                sbTemp.Append("<td  width='20%'>" + dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName].ToString() + "</td>");
                                            }
                                            else
                                            {
                                                sbTemp.Append("<td width='20%' colspan='" + count + "' >" + dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[0].ColumnName].ToString() + "</td>");
                                                break;
                                            }
                                        }

                                        sbTemp.Append("</tr>");
                                    }
                                    sbTemp.Append("</table>");
                                    sbTemp.Append("</td>");
                                }
                                dsSen.Dispose();
                                sbTemp.Append("</tr>");
                            }
                            else
                            {
                                if (dt.Rows[i]["FieldType"].ToString().Trim() == "N" || dt.Rows[i]["FieldType"].ToString().Trim() == "F" || dt.Rows[i]["FieldType"].ToString().Trim() == "TM")
                                {
                                    sbTemp.Append("<td style='border-style:none;width:400px;'>" + dt.Rows[i]["FieldName"].ToString() + "</td><td style='border-style: none;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + sEnd + eBold + "</td><td style='border-style: none;'> " + common.myStr(dt.Rows[i]["UnitName"]) + sEnd + "</td><td style='border-style: none;'>   (" + common.myStr(dt.Rows[i]["MinValue"]) + " - " + common.myStr(dt.Rows[i]["MaxValue"]) + sEnd + ") </td>");
                                }
                                else
                                {
                                    sbTemp.Append("<td style='border-style:none;width:400px;'>" + sDEPBold + dt.Rows[i]["FieldName"].ToString() + eDEPBold + "</td><td style='border-style: none;' colspan='2' align='left'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + sEnd + eBold + "</td>");
                                }
                            }
                            sbTemp.Append("</tr>");
                            sbTemp.Append(EndList);
                        }
                        dt.Dispose();
                        subDeptId = common.myInt(dr["SubDeptId"]);
                    }
                }
            }
        }
        sbTemp.Append("</table>");
        sb.Append(sbTemp.ToString());

        ds.Dispose();
        lis = null;
        sbTemp = null;

        return sb;
    }
    public StringBuilder BindLabTestResult(int RegNo, int HospitalId, int EncounterId, int UserId, string DocId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                               int loginFacilityID, string pageID, string userID, bool bShowAllParameters)
    {
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        ds = new DataSet();
        BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConString);
        StringBuilder sbTemp = new StringBuilder();
        DataTable dtmain = new DataTable();

        ds = objval.GetLabTestResultForNote(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                            common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), 0, EncounterId, RegNo,
                                            common.myInt(System.Web.HttpContext.Current.Session["UserId"]), bShowAllParameters);
        if (dtmain.Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
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

        for (int i = 0; i < dtmain.Rows.Count; i++)
        {
            // test name Received date: result unit (Normal Range: from - to) 
            DataRow dr = dtmain.Rows[i] as DataRow;

            //sbTemp.Append("<br />" + sBeginSection + dr["DepartmentName"].ToString() + sEndSection);
            sbTemp.Append(BeginList);

            sbTemp.Append("<br />" + sBegin + common.myStr(dr["FieldName"]) + sEnd);
            sbTemp.Append(sBegin + " Received " + common.myStr(dr["ResultDate"]) + ": " + sEnd);
            sbTemp.Append(sBegin + " " + common.myStr(dr["Result"]) + sEnd);
            sbTemp.Append(sBegin + " " + common.myStr(dr["UnitName"]) + sEnd);

            if (common.myStr(dr["FieldType"]) == "N")
            {
                string MinValue = common.myStr(dr["MinValue"]);
                string Symbol = common.myStr(dr["Symbol"]);
                string MaxValue = common.myStr(dr["MaxValue"]);

                bool AbnormalValue = common.myBool(dr["AbnormalValue"]);

                string ReferenceRange = "";
                if (common.myDbl(MinValue) == 0
                    && common.myDbl(MaxValue) == 0)
                {
                    ReferenceRange = "(Normal Range: Undefined)";
                }

                if (common.myBool(AbnormalValue) != true)
                {
                    if (common.myDbl(MinValue) != 0
                        && common.myDbl(MaxValue) != 0)
                    {
                        ReferenceRange += "(Normal Range: " + common.myDbl(MinValue) + " " + common.myStr(Symbol) + " " + common.myDbl(MaxValue) + ")";
                    }
                }

                sbTemp.Append(sBegin + " " + ReferenceRange + sEnd);
            }

            sbTemp.Append(EndList);
        }
        sb.Append(sbTemp.ToString());

        ds.Dispose();
        objval = null;
        sbTemp = null;
        dtmain.Dispose();

        return sb;
    }

    public StringBuilder BindReferalHistory(string RegNo, int HospitalId, int EncounterId, int UserId,
                            StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string fromDate, string toDate, string sEREncounterId, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;

        ds = new DataSet();
        DataView dv = new DataView();
        Hashtable hsOrd = new Hashtable();
        StringBuilder sbTemp = new StringBuilder();
        StringBuilder referal = new StringBuilder();


        hsOrd.Add("@chvRegistrationNo", RegNo);
        hsOrd.Add("@intEncounterId", EncounterId);
        hsOrd.Add("@intFacilityId", System.Web.HttpContext.Current.Session["FacilityId"]);
        hsOrd.Add("@intUserId", UserId);
        hsOrd.Add("@chvEncounterType", "B");
        if (fromDate != "" && toDate != "")
        {
            hsOrd.Add("@dtFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd")); //yyyy-MM-dd 00:00
            hsOrd.Add("@dtToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }
        if (GroupingDate != "")
            hsOrd.Add("@chrGroupingDate", GroupingDate);

        hsOrd.Add("@chvPageType", "CN");

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "USPGetDoctorReferral", hsOrd);
        objtlc = null;

        dv = new DataView(ds.Tables[0]);
        dv.RowFilter = "EncounterId=" + EncounterId;

        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        sBeginSection = sBegin;
        sEndSection = sEnd;
        if (dv.ToTable().Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());

                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append(BeginList);

            referal.Append("<U><B>Referral History</B></U><br/><br/>");

            referal.Append("<table cellspacing='2px' border='1' width='95%' style='font-size: 12px;' >");
            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top'>Referral Date</td><td style='font-weight:bold;' valign='top'>Referral Doctor Name</td>");
            referal.Append("<td style='font-weight:bold;' valign='top'>Reason for Referral</td><td style='font-weight:bold;' valign='top'>Refer to Doctor Name</td>");
            referal.Append("<td style='font-weight:bold;' valign='top'>Referral Reply</td><td style='font-weight:bold;' valign='top'>Referral Reply Date</td>");
            referal.Append("</tr>");

            for (int i = 0; i < dv.ToTable().Rows.Count; i++)
            {
                referal.Append("<tr>");
                referal.Append("<td valign='top'>" + dv.ToTable().Rows[i]["ReferralDate"].ToString() + "</td>");
                referal.Append("<td valign='top'>" + dv.ToTable().Rows[i]["FromDoctorName"].ToString() + "</td>");
                referal.Append("<td valign='top'>" + dv.ToTable().Rows[i]["Note"].ToString() + "</td>");
                referal.Append("<td valign='top'>" + dv.ToTable().Rows[i]["DoctorName"].ToString() + "</td>");
                referal.Append("<td valign='top'>" + dv.ToTable().Rows[i]["DoctorRemark"].ToString() + "</td>");
                referal.Append("<td valign='top'>" + dv.ToTable().Rows[i]["BeforeFinalizedDate"].ToString() + "</td>");
                referal.Append("</tr>");
            }
            referal.Append("</table>");
        }
        sb.Append(referal);

        ds.Dispose();
        dv.Dispose();
        hsOrd = null;
        sbTemp = null;
        referal = null;

        return sb;
    }

    public StringBuilder BindNonDrugOrder(int RegId, int HospitalId, int EncounterId, int UserId,
                           StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                           Page pg, string pageID, string fromDate, string toDate, string sEREncounterId, string GroupingDate)
    {
        ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        StringBuilder sbTemp = new StringBuilder();

        HshIn.Add("@inyHospitalLocationId", HospitalId);
        HshIn.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        HshIn.Add("@intRegistrationId", RegId);
        HshIn.Add("@intEncounterId", EncounterId);

        if (fromDate != "" && toDate != "")
        {
            HshIn.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd")); //yyyy-MM-dd 00:00
            HshIn.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }
        if (GroupingDate != "")
            HshIn.Add("@chrGroupingDate", GroupingDate);

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspICMNONDrugOrder", HshIn);

        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        sBeginSection = sBegin;
        sEndSection = sEnd;
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle + "<u><b>" + "Non Drug Order : " + "</b></u><br/>" + BeginList);
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());

                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append(BeginList);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sbTemp.Append(ds.Tables[0].Rows[i]["Prescription"].ToString());
                sbTemp.Append("<br/>&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + ds.Tables[0].Rows[i]["DoctorName"].ToString() + " on " + ds.Tables[0].Rows[i]["EncodedDate"].ToString() + "</span><br/>");
                //sOrderDate = ds.Tables[0].Rows[i]["OrderDate"].ToString();
            }
        }
        sb.Append(sbTemp);

        ds.Dispose();
        HshIn = null;
        objtlc = null;
        sbTemp = null;

        return sb;
    }

    public StringBuilder BindDietOrderInNote(int RegId, int HospitalId, int EncounterId,
                            StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string fromDate, string toDate, string GroupingDate)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        ds = new DataSet();
        Hashtable hsOrd = new Hashtable();
        StringBuilder sbTemp = new StringBuilder();
        StringBuilder DietOrder = new StringBuilder();


        hsOrd.Add("@inyHospitalLocationId", HospitalId);
        hsOrd.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
        hsOrd.Add("@intRegistrationId", RegId);
        hsOrd.Add("@intEncounterId", EncounterId);
        if (fromDate != "" && toDate != "")
        {
            hsOrd.Add("@chrFromDate", Convert.ToDateTime(fromDate).ToString("yyyy/MM/dd")); //yyyy-MM-dd 00:00
            hsOrd.Add("@chrToDate", Convert.ToDateTime(toDate).ToString("yyyy/MM/dd"));
        }
        if (GroupingDate != "")
            hsOrd.Add("@chrGroupingDate", GroupingDate);


        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspGetDietOrderDetailInNote", hsOrd);

        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        sBeginSection = sBegin;
        sEndSection = sEnd;
        int t = 0;

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, HospitalId.ToString());

                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            sb.Append(BeginList);

            DietOrder.Append("<U><B>Diet Order</B></U><br/><br/>");
            int iCountColumn = 0;
            DietOrder.Append("<table border='1' style='font-size:12px;' cellspacing='2px' width='85%' >");
            DietOrder.Append("<tr>");
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ColumnName.ToString() != "Remarks")
                {
                    DietOrder.Append("<td style='font-weight:bold;' valign='top'>" + ds.Tables[0].Columns[i].ColumnName.ToString() + "</td>");
                }
                iCountColumn++;
            }
            DietOrder.Append("<td style='font-weight:bold;' valign='top'>" + ds.Tables[0].Columns[1].ColumnName.ToString() + "</td>");
            DietOrder.Append("</tr>");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DietOrder.Append("<tr>");
                for (int j = 0; j < iCountColumn; j++)
                {
                    if (ds.Tables[0].Columns[j].ColumnName.ToString() != "Remarks")
                    {
                        DietOrder.Append("<td valign='top'>" + ds.Tables[0].Rows[i][ds.Tables[0].Columns[j].ColumnName.ToString()].ToString() + "</td>");
                    }
                }
                DietOrder.Append("<td valign='top'>" + ds.Tables[0].Rows[i][ds.Tables[0].Columns[1].ColumnName.ToString()].ToString() + "</td>");
                DietOrder.Append("</tr>");
            }
            DietOrder.Append("</table>");
        }
        sb.Append(DietOrder);

        ds.Dispose();
        hsOrd = null;
        sbTemp = null;
        DietOrder = null;
        objtlc = null;

        return sb;
    }


    public StringBuilder BindPatientBooking(int HospitalId, int FacilityId, int RegId, int EncounterId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                           Page pg, string pageId)
    {
        StringBuilder referal = new StringBuilder();
        ds = new DataSet();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        BaseC.EMR emr = new BaseC.EMR(sConString);
        ds = emr.GetEMRAdmissionRequest(HospitalId, FacilityId, EncounterId, RegId);
        sBeginSection = sBegin;
        sEndSection = sEnd;
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

                if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }
            referal.Append(BeginList);

            referal.Append("<U><B>Admission Request</B></U><br/><br/>");
            referal.Append("<table cellspacing='2px' border='1' width='55%' style='font-size: 12px;' >");
            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Date of Request</td>");
            referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["BookingDate"]) + "</td>");
            referal.Append("</tr>");

            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Expected Date of Admission</td>");
            referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["ExpectedAdmissionDate"]) + "</td>");
            referal.Append("</tr>");

            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Consultant Name</td>");
            referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["DoctorName"]) + "</td>");
            referal.Append("</tr>");

            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Depart. Name</td>");
            referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["DEPARTMENTNAME"]) + "</td>");
            referal.Append("</tr>");


            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Probable LOS (Days)</td>");
            referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["ProbableStayInDays"]) + "</td>");
            referal.Append("</tr>");


            referal.Append("<tr>");
            referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Reason for Admission</td>");
            referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["ReasonForAdmission"]) + "</td>");
            referal.Append("</tr>");

            if (common.myStr(ds.Tables[0].Rows[0]["BookingRemarks"]).Trim() != "")
            {
                referal.Append("<tr>");
                referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>Remarks</td>");
                referal.Append("<td valign='top'>" + common.myStr(ds.Tables[0].Rows[0]["BookingRemarks"]) + "</td>");
                referal.Append("</tr>");
            }

            referal.Append("</table><br/>");

            referal.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg));
            referal.Append(EndList);
        }
        sb.Append(referal);
        referal = null;
        ds.Dispose();
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
                { sBegin += GetFontFamily(typ, item, Pg, HospitalId); }


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
    public DataSet GetSubDepartment(int DepartmentId, int HospitalLocationId)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        DataSet ds = new DataSet();
        string queryString = string.Empty;
        try
        {
            Hashtable hshUser = new Hashtable();
            hshUser.Add("@intDepartmentId", DepartmentId);
            hshUser.Add("@intHospitalLocationId", HospitalLocationId);
            string strUser = "SELECT SubDeptId, SubName  FROM DepartmentSub WITH (NOLOCK) WHERE DepartmentId = @intDepartmentId AND Active=1 AND HospitalLocationId = @intHospitalLocationId ORDER BY SubName";
            ds = objtlc.GetDataSet(CommandType.Text, strUser, hshUser);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objtlc = null;
        }

        return ds;

    }


    public string SaveEMRServiceOrderExceptions(int FacilityId, int SecGroupId, int EmployeeId, int SubDepartmentId, int EncodedBy, int type)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        Hashtable hstInput = new Hashtable();
        Hashtable hstOutput = new Hashtable();
        hstInput.Add("@intFacilityId", FacilityId);
        hstInput.Add("@intSecGroupId", SecGroupId);
        hstInput.Add("@intEmployeeId", EmployeeId);
        hstInput.Add("@intSubDepartmentId", SubDepartmentId);
        hstInput.Add("@intEncodedBy", EncodedBy);
        hstInput.Add("@inttype", type);

        hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
        hstOutput = (Hashtable)objtlc.GetOutputParametersValues("uspEMRSaveOrderExceptions", hstInput, hstOutput);

        objtlc = null;

        return hstOutput["@chvErrorStatus"].ToString();
    }
    public string DeleteEMRServiceOrderExceptions(int id, int EncodedBy, int type)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        Hashtable hstInput = new Hashtable();
        Hashtable hstOutput = new Hashtable();
        hstInput.Add("@intId", id);
        hstInput.Add("@intEncodedBy", EncodedBy);
        hstInput.Add("@inttype", type);
        hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
        hstOutput = (Hashtable)objtlc.GetOutputParametersValues("uspEMRSaveOrderExceptions", hstInput, hstOutput);
        objtlc = null;
        return hstOutput["@chvErrorStatus"].ToString();
    }

    public DataSet GetEMRServiceOrderExceptions(int FacilityId, int type)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        DataSet ds = new DataSet();
        string queryString = string.Empty;
        try
        {
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@inttype", type);
            ds = objtlc.GetDataSet(CommandType.StoredProcedure, "uspEMRGetServiceOrderExceptions", hstInput);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objtlc = null;
        }

        return ds;

    }
	
	public string getFastTrack5Output(string CIMSXML)
    {
        string outputValues = string.Empty;
        try
        {
            string queryString = CIMSXML;
            string CIMSDatabasePath = common.myStr(HttpContext.Current.Session["CIMSDatabasePath"]);
            string CIMSDatabasePassword = common.myStr(HttpContext.Current.Session["CIMSDatabasePassword"]);
            string CIMSDatabaseName = common.myStr(HttpContext.Current.Session["CIMSDatabaseName"]);

            if (common.myLen(CIMSDatabaseName).Equals(0))
            {
                CIMSDatabaseName = "FastTrackData.mrc";
            }

            //Monograph
            //string queryString = "<Request><Content><Product reference=\"{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}\" /></Content></Request>";
            //Interaction
            //string queryString = "<Request><Interaction><Prescribing><Product reference="{DAABD513-72A6-41F2-ABEC-DDAEABBC18D9}" /></Prescribing><Prescribed><Product reference="{FC27925F-AEA2-48C4-8DBC-088E56C0D0B3}" /><Product reference="{DF1478CC-9840-408E-9BF3-AE95CD4036B4}" /></Prescribed><Allergies /><References/></Interaction></Request>";
            string retultInfo = string.Empty;
            string guid = string.Empty;
            //string dataPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) +
            //                    "CIMSDatabase\\FastTrackData.mrc";
            //string CIMSDatabasePassword = "GDDBDEMO";
            string initString = string.Empty;
            if (File.Exists(CIMSDatabasePath + CIMSDatabaseName))
            {
                initString = "<Initialize><DataFile password='" + CIMSDatabasePassword + "' path='" + CIMSDatabasePath + CIMSDatabaseName + "' /></Initialize>";
            }
            FastTrack5.FastTrack_Creator ftCreator = new FastTrack5.FastTrack_Creator();
            FastTrack5.IFastTrack_Server ftServer;
            ftServer = ftCreator.CreateServer(initString, out retultInfo, out guid);
            outputValues = ftServer.RequestXML(queryString, out retultInfo);
        }
        catch
        {
        }
        return outputValues;
    }

}
