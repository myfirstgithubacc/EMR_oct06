using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Text.RegularExpressions;

/// <summary>
/// Summary description for clsIVF
/// </summary>
public class clsIVF
{
    private string sConString = "";

    DAL.DAL objDl;
    DataSet ds;
    Hashtable hstInput;
    Hashtable hstOut;
    public clsIVF(string conString)
    {
        sConString = conString;
    }

    public DataSet getSpouseRegistrationId(int RegistrationId, int FacilityId, int HospId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        HshIn.Add("@intRegistrationId", RegistrationId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intHospId", HospId);

        string disString = "SELECT TOP 1 reg.Id AS RegistrationId, reg.RegistrationNo, enc.Id AS EncounterId, enc.EncounterNo " +
                        " FROM Registration reg  WITH (NOLOCK) " +
                        " INNER JOIN RegistrationGuarantorDetail rg WITH (NOLOCK) ON reg.Id = rg.GuarantorRegistrationId " +
                        " INNER JOIN Encounter enc WITH (NOLOCK) ON rg.GuarantorRegistrationId = enc.RegistrationId " +
                        " INNER JOIN KinRelation kr WITH (NOLOCK) ON rg.RelationshipId = kr.kinId AND kr.Code='01' " +
                        " WHERE rg.RegistrationId = @intRegistrationId AND reg.FacilityID = @intFacilityId AND reg.HospitalLocationId = @intHospId " +
                        " ORDER BY enc.Id DESC";

        ds = objDl.FillDataSet(CommandType.Text, disString, HshIn);

        return ds;

    }

    public string ParseQ(string str)
    {

        if (str == null) return null;
        if (str.Length > 0)
        {
            str = str.Replace("&nbsp;", "");
            str = str.Replace("&apos;", "'");
            str = str.Replace("&#39;", "'");
            //str = str.Replace("'", "''");
            str = str.Replace("\r\n", " ");
            str = str.Replace(";", "");

            str = str.Trim();
        }
        return str;
    }
    public string ParseSearchData(string str, int Criteria)
    {
        //0= Search all of the words
        //1= Search any of the words
        //2= Exact Phrase
        //3= Boolean Search

        str = ParseQ(str);

        bool sFlag = false;
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }
        str = str.Trim();
        while ((Regex.IsMatch(str, "\\s{2}")))
        {
            str = str.Replace("  ", " ");
        }

        if (str.IndexOf(" OR ", 0) > 1)
        {
            str = str.Replace(" OR ", " | ");
            sFlag = true;
        }
        if (str.IndexOf(" AND ", 0) > 1)
        {
            str = str.Replace(" AND ", " & ");
            sFlag = true;
        }
        if (str.IndexOf(" NOT ", 0) > 1)
        {
            str = str.Replace(" NOT ", " &! ");
            sFlag = true;
        }
        if (str.IndexOf(" AND NOT ", 0) > 1)
        {
            str = str.Replace(" AND NOT ", " &! ");
            sFlag = true;
        }
        if (str.IndexOf(" NEAR ", 0) > 1)
        {
            str = str.Replace(" NEAR ", " ~ ");
            sFlag = true;
        }

        if (str.Length < 1)
        {
            return str;
        }

        if (Criteria == 0 & !sFlag)
        {
            str = str.Replace(" ", "\" ~ \"");
        }
        else if (Criteria == 0 & Regex.IsMatch(str, "\"\\s*\""))
        {
            str = Regex.Replace(str, "\"\\s*\"", "\" ~ \"");
        }
        if (Criteria == 1 & !sFlag)
        {
            str = str.Replace(" ", "\" | \"");
        }
        if (!str.StartsWith("\"")) str = "\"" + str;
        if (!str.EndsWith("\"")) str += "\"";
        str = str.Replace("\"\"", "\"");


        //Exact Phrase
        if (Criteria == 2)
        {
            str = " " + str + " ";

            return str;

        }
        return str;

    }

    public string SetUpDateEMRToTranslateLanguage(int RegistrationId, String EMRToTranslateLanguage)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@RegistrationId", RegistrationId);
            HshIn.Add("@EMRToTranslateLanguage", EMRToTranslateLanguage);

            string qry = "update Registration set EMRToTranslateLanguage=@EMRToTranslateLanguage where Id=@RegistrationId";
            objDl.ExecuteNonQuery(CommandType.Text, qry, HshIn);

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return " Record Update ";
    }

    public string SetEMRToTranslateLanguage(int RegistrationId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        DataSet ds = new DataSet();
        string EMRToTranslateLanguage = "";

        try
        {
            HshIn.Add("@RegistrationId", RegistrationId);

            string qry = "Select EMRToTranslateLanguage from Registration with(nolock) where Id=@RegistrationId";
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                EMRToTranslateLanguage = common.myStr(ds.Tables[0].Rows[0]["EMRToTranslateLanguage"]);
            }

        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return EMRToTranslateLanguage;
    }

    public DataSet getWardGetPendingTemplates(int HospId, int FacilityId, int RegistrationId, int WardId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@anyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            if (WardId > 0)
            {
                HshIn.Add("@intWardId", WardId);
            }

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetPendingTemplates", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return ds;
    }


    public DataSet getIVFRegistrationId(int IVFId, int EncounterId, int FacilityId, int HospId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        HshIn.Add("@intIVFId", IVFId);
        HshIn.Add("@intEncounterId", EncounterId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intHospId", HospId);

        //string disString = "SELECT TOP 1 reg.IVFId, reg.IVFNo, reg.Spouse, reg.Id AS RegistrationId, reg.RegistrationNo, enc.Id AS EncounterId, enc.EncounterNo " +
        //                " FROM Registration reg " +
        //                " INNER JOIN Encounter enc ON reg.Id = enc.RegistrationId " +
        //                " WHERE IVFId = (SELECT MAX(IVFId) FROM Registration WHERE Id = @intRegistrationId) " +
        //                " AND reg.Id <> @intRegistrationId AND reg.FacilityID = @intFacilityId AND reg.HospitalLocationId = @intHospId";

        string disString = " SELECT DISTINCT ivf.Id IVFId, ivf.IVFNo, ivf.Spouse, enc.RegistrationId, enc.RegistrationNo, enc.Id AS EncounterId, enc.EncounterNo,  " +
                            " reg.Gender, dbo.UdfCurrentAgeGender(reg.Gender, reg.DateOfBirth, GETDATE()) AgeGender, enc.DoctorId, esm.Status, " +
                            " enc.OPIP, dbo.GetDateFormatUTC(enc.EncodedDate, 'DT', fm.TimeZoneOffSetMinutes) AS EncounterDate " +
                            " FROM Encounter enc WITH (NOLOCK) " +
                            " INNER JOIN Registration reg WITH(NOLOCK) ON enc.RegistrationId = reg.Id AND enc.Active = 1 " +
                            " INNER JOIN GetStatus(@intHospId, 'EMR') esm ON enc.EMRStatusId = esm.StatusId " +
                            " INNER JOIN FacilityMaster fm ON enc.FacilityID = fm.FacilityID " +
                            " INNER JOIN IVFPatientDetail ivf WITH (NOLOCK) ON enc.Id = ivf.EncounterId AND ivf.Active = 1 " +
                            " WHERE ivf.Id = @intIVFId " +
                            " AND enc.Id <> @intEncounterId " +
                            " AND enc.FacilityID = @intFacilityId " +
                            " AND enc.HospitalLocationId = @intHospId ";

        ds = objDl.FillDataSet(CommandType.Text, disString, HshIn);

        return ds;

    }
    public int getEncounterId(string EncounterNo, int FacilityId)
    {
        DataSet ds = new DataSet();
        int EncounterId = 0;
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intFacilityId", FacilityId);

            string qry = "SELECT Id AS EncounterId FROM Encounter WITH (NOLOCK) " +
                        " WHERE EncounterNo = @chvEncounterNo AND FacilityId = @intFacilityId AND Active = 1 ";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                EncounterId = common.myInt(ds.Tables[0].Rows[0]["EncounterId"]);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return EncounterId;
    }
    public DataSet getIVFPatient(int RegistrationId, int IVFId)
    {
        return getIVFPatient(RegistrationId, IVFId, 0);
    }

    public DataSet getIVFPatient(int RegistrationId, int IVFId, int EncounterId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        HshIn.Add("@intRegistrationId", RegistrationId);
        HshIn.Add("@intIVFId", IVFId);
        HshIn.Add("@intEncounterId", EncounterId);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatient", HshIn);

        return ds;
    }

    public string getCustomizedPatientReportHeader(int HeaderId)
    {
        return getCustomizedPatientReportHeader(HeaderId, string.Empty);
    }
    public string getCustomizedPatientReportHeader(int HeaderId, string patientType)
    {
        return getCustomizedPatientReportHeaderIncludeTemplate(HeaderId, patientType, 0);
    }

    public string getCustomizedPatientReportHeaderIncludeTemplate(int HeaderId, string patientType, int TemplateId)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            //string Fonts = getFontName("1", "10");
            //string bold = "", italic = "";

            //string Ft = "style='" + Fonts + ";" + bold + italic;

            DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet dsHead = new DataSet();
            Hashtable hsSub = new Hashtable();
            hsSub = new Hashtable();

            hsSub.Add("@intHeaderId", HeaderId);
            hsSub.Add("@chrAppointmentDate", Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd"));
            hsSub.Add("@intEmployeeId", common.myInt(System.Web.HttpContext.Current.Session["DoctorId"]));
            hsSub.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));
            hsSub.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
            hsSub.Add("@intEncounterId", common.myStr(System.Web.HttpContext.Current.Session["EncounterId"]));
            if (TemplateId > 0)
            {
                hsSub.Add("@intTemplateId", common.myInt(TemplateId));
            }
            if (common.myLen(patientType) > 0)
            {
                hsSub.Add("@chrPatientType", patientType);
            }

            dsHead = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hsSub);
            // string Fonts = getFontName("1", "10");

            if (dsHead.Tables.Count > 0)
            {
                if (dsHead.Tables[0].Rows.Count > 0)
                {
                    // string Fonts = getFontName("1", "10");
                    string fontweightbold = string.Empty;
                    if (common.myBool(dsHead.Tables[1].Rows[0]["HeaderFontBold"]))
                    {
                        fontweightbold = "font-weight:bold;";
                    }

                    string Fonts = getFontName(common.myStr(dsHead.Tables[1].Rows[0]["HeaderFontId"]), common.myStr(dsHead.Tables[1].Rows[0]["HeaderFontSize"]));

                    System.Web.HttpContext.Current.Session["HeaderFooterFont"] = Fonts;
                    string bold = "", italic = "";

                    string Ft = "style='" + Fonts + ";" + bold + italic;

                    if (dsHead.Tables[0].Rows.Count > 0)
                    {
                        if (common.myStr(System.Web.HttpContext.Current.Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                        {
                            sb.Append("<table width='100%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='1' cellspacing='1'>");
                        }
                        else
                        {
                            sb.Append("<table width='100%' style='" + Fonts + "; border-collapse:collapse;' border='1' cellpadding='1' cellspacing='1' " + fontweightbold + ">");
                        }
                        sb.Append("<tr align='center'>");
                        sb.Append("<td align='center' valign='top' " + fontweightbold + ">");

                        DataRow dr = dsHead.Tables[0].Rows[0];
                        if (common.myBool(dsHead.Tables[0].Rows[0]["ShowBorder"]))
                        {
                            sb.Append("<table width='100%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");

                            int maxColNo = common.myInt(dsHead.Tables[0].Compute("Max(colNo)", string.Empty));
                            if (maxColNo.Equals(3))
                            {
                                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                            }
                            else
                            {
                                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                            }

                            int cc = 0;
                            for (int r = 1; r <= 8; r++)
                            {
                                DataView dvR = new DataView(dsHead.Tables[0]);
                                dvR.RowFilter = "RowNo=" + r.ToString();
                                DataTable dtR = dvR.ToTable();
                                if (dtR.Rows.Count != 0)
                                {
                                    sb.Append("<tr align='left'>");
                                    for (int c = 1; c <= dtR.Rows.Count; c++)
                                    {
                                        if (maxColNo.Equals(3))
                                        {
                                            if (common.myLen(dsHead.Tables[0].Rows[cc]["FieldCaption"]) > 0)
                                            {
                                                sb.Append("<td valign='top' style='border-style:none;font-size:10pt; " + fontweightbold + "'>" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Trim() + "</td>");

                                                if (c.Equals(1))
                                                {
                                                    if (common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Contains("Address") || common.myInt(dtR.Rows.Count).Equals(1))
                                                    {
                                                        sb.Append("<td colspan='8' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sb.Append("<td colspan='3' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                    }
                                                }
                                                else if (c.Equals(2))
                                                {
                                                    if (common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Contains("Address") || common.myInt(dtR.Rows.Count).Equals(1))
                                                    {
                                                        sb.Append("<td colspan='8' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sb.Append("<td colspan='1' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");

                                                        if (dtR.Rows.Count < maxColNo)
                                                        {
                                                            sb.Append("<td colspan='3'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Contains("Address") || common.myInt(dtR.Rows.Count).Equals(1))
                                                    {
                                                        sb.Append("<td colspan='8' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sb.Append("<td colspan='2' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                sb.Append("<td colspan='3' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (common.myLen(dsHead.Tables[0].Rows[cc]["FieldCaption"]) > 0)
                                            {
                                                sb.Append("<td valign='top' style='border-style:none;font-size:10pt;'>" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Trim() + "</td>");

                                                if (common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Contains("Address") || common.myInt(dtR.Rows.Count).Equals(1))
                                                {
                                                    sb.Append("<td colspan='5' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                }
                                                else
                                                {
                                                    sb.Append("<td colspan='2' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                                }
                                            }
                                            else
                                            {
                                                sb.Append("<td colspan='3' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                            }
                                        }
                                        cc++;
                                    }
                                    sb.Append("</tr>");

                                }
                            }
                            sb.Append("</table>");
                        }
                        else
                        {
                            sb.Append("<table width='99%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0' " + fontweightbold + ">");

                            int cc = 0;
                            for (int r = 1; r <= 8; r++)
                            {
                                DataView dvR = new DataView(dsHead.Tables[0]);

                                dvR.RowFilter = "RowNo=" + r.ToString();
                                DataTable dtR = dvR.ToTable();

                                if (dtR.Rows.Count != 0)
                                {
                                    sb.Append("<tr align='left'>");

                                    for (int c = 1; c <= dtR.Rows.Count; c++)
                                    {
                                        if (!Convert.ToBoolean(dsHead.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            sb.Append("<td valign='top' style='border-style:none;font-size:10pt; " + fontweightbold + "'>" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]) + " - " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]) + "</td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td valign='top' style='border-style:none;font-size:10pt; " + fontweightbold + "'>:" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]) + " - " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</td>");
                                        }
                                        cc++;
                                    }
                                    sb.Append("</tr>");
                                }
                            }
                            sb.Append("</table>");

                        }
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
        }
        return sb.ToString();
    }

    public string getCustomizedPatientReportHeader(int HeaderId, string patientType, int EncounterId)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            //string Fonts = getFontName("1", "10");
            //string bold = "", italic = "";

            //string Ft = "style='" + Fonts + ";" + bold + italic;

            DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet dsHead = new DataSet();
            Hashtable hsSub = new Hashtable();
            hsSub = new Hashtable();

            hsSub.Add("@intHeaderId", HeaderId);
            hsSub.Add("@chrAppointmentDate", Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd"));
            hsSub.Add("@intEmployeeId", common.myInt(System.Web.HttpContext.Current.Session["DoctorId"]));
            hsSub.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));
            hsSub.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));
            hsSub.Add("@intEncounterId", (EncounterId != 0 ? EncounterId : common.myInt(System.Web.HttpContext.Current.Session["EncounterId"])));

            if (common.myLen(patientType) > 0)
            {
                hsSub.Add("@chrPatientType", patientType);
            }

            dsHead = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hsSub);
            // string Fonts = getFontName("1", "10");
            if (dsHead.Tables.Count > 0)
            {
                if (dsHead.Tables[0].Rows.Count > 0)
                {
                    // string Fonts = getFontName("1", "10");
                    string Fonts = getFontName(common.myStr(dsHead.Tables[1].Rows[0]["HeaderFontId"]), common.myStr(dsHead.Tables[1].Rows[0]["HeaderFontSize"]));
                    System.Web.HttpContext.Current.Session["HeaderFooterFont"] = Fonts;
                    string bold = "", italic = "";

                    string Ft = "style='" + Fonts + ";" + bold + italic;
                    sb.Append("<hr>");
                    if (dsHead.Tables[0].Rows.Count > 0)
                    {
                        sb.Append("<table width='100%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='1' cellspacing='1'>");
                        sb.Append("<tr align='center'>");
                        sb.Append("<td align='center' valign='top'>");

                        DataRow dr = dsHead.Tables[0].Rows[0];
                        if (common.myBool(dsHead.Tables[0].Rows[0]["ShowBorder"]))
                        {
                            sb.Append("<table width='99%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");

                            sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");

                            int cc = 0;
                            for (int r = 1; r <= 8; r++)
                            {
                                DataView dvR = new DataView(dsHead.Tables[0]);
                                dvR.RowFilter = "RowNo=" + r.ToString();
                                DataTable dtR = dvR.ToTable();
                                if (dtR.Rows.Count != 0)
                                {
                                    sb.Append("<tr align='left'>");
                                    for (int c = 1; c <= dtR.Rows.Count; c++)
                                    {
                                        if (common.myLen(dsHead.Tables[0].Rows[cc]["FieldCaption"]) > 0)
                                        {
                                            sb.Append("<td valign='top' style='border-style:none;font-size:10pt;'>" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Trim() + "</td>");

                                            if (common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]).Contains("Address") || common.myInt(dtR.Rows.Count).Equals(1))
                                            {
                                                sb.Append("<td colspan='5' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                            }
                                            else
                                            {
                                                sb.Append("<td colspan='2' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                            }
                                        }
                                        else
                                        {
                                            sb.Append("<td colspan='3' valign='top' style='border-style:none;font-size:10pt;'>: " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]).Trim() + "</td>");
                                        }
                                        cc++;
                                    }
                                    sb.Append("</tr>");

                                }
                            }
                            sb.Append("</table>");
                        }
                        else
                        {
                            sb.Append("<table width='99%' style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");

                            int cc = 0;
                            for (int r = 1; r <= 8; r++)
                            {
                                DataView dvR = new DataView(dsHead.Tables[0]);

                                dvR.RowFilter = "RowNo=" + r.ToString();
                                DataTable dtR = dvR.ToTable();

                                if (dtR.Rows.Count != 0)
                                {
                                    sb.Append("<tr align='left'>");

                                    for (int c = 1; c <= dtR.Rows.Count; c++)
                                    {
                                        if (!Convert.ToBoolean(dsHead.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            sb.Append("<td valign='top' style='border-style:none;font-size:10pt;'>" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]) + " - " + common.myStr(dsHead.Tables[0].Rows[cc]["Value"]) + "</u></td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td valign='top' style='border-style:none;font-size:10pt;'>:" + common.myStr(dsHead.Tables[0].Rows[cc]["FieldCaption"]) + " - " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</u></td>");
                                        }
                                        cc++;
                                    }
                                    sb.Append("</tr>");
                                }
                            }
                            sb.Append("</table>");

                        }
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");
                    }
                    sb.Append("<hr>");
                }
            }
        }
        catch (Exception Ex)
        {
        }
        return sb.ToString();
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
        if (id == "11")
            sBegin += " font-family:Trebuchet MS ";

        if (sBegin == string.Empty)
        {
            sBegin += " font-family:Calibri ";
        }

        if (FtSize == "9")
            sFontSize += " ; font-size:9pt ";
        if (FtSize == "10")
            sFontSize += " ; font-size:10pt ";
        if (FtSize == "11")
            sFontSize += " ; font-size:11pt ";
        if (FtSize == "12")
            sFontSize += " ; font-size:12pt; ";
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

        if (sFontSize == string.Empty)
        {
            sFontSize += " ; font-size:10pt ";
        }

        return sBegin + sFontSize;
    }

    public string GetReportImageUrl(int id)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        return (string)objDl.ExecuteScalar(CommandType.Text, "select OtherImageTextUrl from EMRTemplateReportSetup WITH (NOLOCK) Where Active=1 and ReportId= '" + id + "'");


    }
    public DataSet GetReportName()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        ds = objDl.FillDataSet(CommandType.Text, "select ReportId,ReportName,IsCheckListRequired from EMRTemplateReportSetup WITH (NOLOCK) Where Active=1 ");

        return ds;

    }

    //yogesh 16/08/2022
    public DataSet getFacility(int FacilityId, int HospId, string FacilityName)
    {
        return getFacility(FacilityId, HospId, false, FacilityName);
    }

    public DataSet getFacility(int FacilityId, int HospId)
    {
        return getFacility(FacilityId, HospId, false);
    }
    //yogesh 16/08/2022
    public DataSet getFacility(int FacilityId, int HospId, bool IsHospitalLocationDetails, string FacilityName)
    {
        DataSet ds = new DataSet();
        string qry = string.Empty;
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intHospId", HospId);
            if (!IsHospitalLocationDetails)
            {//yogesh
                if (FacilityName == "PRACHI HOSPITAL ")
                {
                    qry = "SELECT fm.Name as FacilityName, fm.Address1, fm.Address2, cm.CityName, sm.StateName, fm.Phone, fm.Fax,fm.PinNo, fm.LogoImagePath, fm.EmailId, fm.WebSite, fm.NABHLogoImagePath, isnull(fm.Name2,'') Name2, isnull(fm.HeaderLine2,'') HeaderLine2, isnull(fm.HeaderLine3,'') HeaderLine3,isnull(fm.TollFree,'') TollFree,isnull(fm.ImmunizationHeaderMobNo,'') ImmunizationHeaderMobNo" +
                " FROM FacilityMaster fm WITH (NOLOCK) " +
                " LEFT JOIN CityMaster cm WITH (NOLOCK) ON fm.CityID = cm.CityID " +
                " LEFT JOIN StateMaster sm WITH (NOLOCK) ON fm.StateID = sm.StateID " +
                " WHERE fm.FacilityID = @intFacilityId " +
                " AND fm.Active = 1 " +
                " AND fm.HospitalLocationId = @intHospId ";
                }
                else
                {
                    qry = "SELECT fm.Name as FacilityName, fm.Address1, fm.Address2, cm.CityName, sm.StateName, fm.Phone, fm.Fax,fm.PinNo, fm.LogoImagePath, fm.EmailId, fm.WebSite, fm.NABHLogoImagePath" +
               " FROM FacilityMaster fm WITH (NOLOCK) " +
               " LEFT JOIN CityMaster cm WITH (NOLOCK) ON fm.CityID = cm.CityID " +
               " LEFT JOIN StateMaster sm WITH (NOLOCK) ON fm.StateID = sm.StateID " +
               " WHERE fm.FacilityID = @intFacilityId " +
               " AND fm.Active = 1 " +
               " AND fm.HospitalLocationId = @intHospId ";
                }



            }
            else
            {//yogesh
                if (FacilityName == "PRACHI HOSPITAL ")
                {
                    qry = "SELECT fm.Name as FacilityName, fm.Address1, fm.Address2, cm.CityName, sm.StateName, fm.Phone, fm.Fax,fm.PinNo, fm.LogoImagePath, fm.EmailId, fm.WebSite, fm.NABHLogoImagePath, isnull(fm.Name2,'') Name2, isnull(fm.HeaderLine2,'') HeaderLine2, isnull(fm.HeaderLine3,'') HeaderLine3,isnull(fm.TollFree,'') TollFree,isnull(fm.ImmunizationHeaderMobNo,'') ImmunizationHeaderMobNo" +
                " FROM FacilityMaster fm WITH (NOLOCK) " +
                " LEFT JOIN CityMaster cm WITH (NOLOCK) ON fm.CityID = cm.CityID " +
                " LEFT JOIN StateMaster sm WITH (NOLOCK) ON fm.StateID = sm.StateID " +
                " WHERE fm.FacilityID = @intFacilityId " +
                " AND fm.Active = 1 " +
                " AND fm.HospitalLocationId = @intHospId ";
                }
                else
                {
                    qry = "SELECT fm.Name as FacilityName, fm.Address1, fm.Address2, cm.CityName, sm.StateName, fm.Phone, fm.Fax,fm.PinNo, fm.LogoImagePath, fm.EmailId, fm.WebSite, fm.NABHLogoImagePath" +
               " FROM FacilityMaster fm WITH (NOLOCK) " +
               " LEFT JOIN CityMaster cm WITH (NOLOCK) ON fm.CityID = cm.CityID " +
               " LEFT JOIN StateMaster sm WITH (NOLOCK) ON fm.StateID = sm.StateID " +
               " WHERE fm.FacilityID = @intFacilityId " +
               " AND fm.Active = 1 " +
               " AND fm.HospitalLocationId = @intHospId ";
                }

            }
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }



    public DataSet getFacility(int FacilityId, int HospId, bool IsHospitalLocationDetails)
    {
        DataSet ds = new DataSet();
        string qry = string.Empty;
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intHospId", HospId);
            if (!IsHospitalLocationDetails)
            {
                qry = "SELECT fm.Name as FacilityName, fm.Address1, fm.Address2, cm.CityName, sm.StateName, fm.Phone, fm.Fax,fm.PinNo, fm.LogoImagePath, fm.EmailId, fm.WebSite, fm.NABHLogoImagePath " +
                   " FROM FacilityMaster fm WITH (NOLOCK) " +
                   " LEFT JOIN CityMaster cm WITH (NOLOCK) ON fm.CityID = cm.CityID " +
                   " LEFT JOIN StateMaster sm WITH (NOLOCK) ON fm.StateID = sm.StateID " +
                   " WHERE fm.FacilityID = @intFacilityId " +
                   " AND fm.Active = 1 " +
                   " AND fm.HospitalLocationId = @intHospId ";
            }
            else
            {
                qry = "SELECT fm.Name as FacilityName, fm.Address1, fm.Address2, cm.CityName, sm.StateName, fm.Phone, fm.Fax,fm.PinNo, fm.LogoImagePath, fm.EmailId, fm.WebSite, fm.NABHLogoImagePath" +
               " FROM FacilityMaster fm WITH (NOLOCK) " +
               " LEFT JOIN CityMaster cm WITH (NOLOCK) ON fm.CityID = cm.CityID " +
               " LEFT JOIN StateMaster sm WITH (NOLOCK) ON fm.StateID = sm.StateID " +
               " WHERE fm.FacilityID = @intFacilityId " +
               " AND fm.Active = 1 " +
               " AND fm.HospitalLocationId = @intHospId ";
            }
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getDoctorSignatureDetails(int DoctorId, int FacilityId, int HospId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@DoctorId", DoctorId);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@HospitalLocationId", HospId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorDetails", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet CheckReportName(string Name)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@chvReportName", Name);
            string qry = "";
            qry = "select ReportId from EMRTemplateReportSetup WITH (NOLOCK) where ReportName=@chvReportName";
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }
    //public DataSet EditReportName(int Id)
    //{
    //    DataSet ds = new DataSet();
    //    Hashtable HshIn = new Hashtable();
    //    try
    //    {
    //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

    //        HshIn.Add("@intReportId", Id);
    //        string qry = "";
    //        qry = "select ReportId,ReportName,TemplateId,DoctorId,SignatureLabel, ShowPageNoInPageFooter, ShowPrintByInPageFooter, ShowPrintDateInPageFooter, IsPrintHospitalHeader,IsPrintDoctorSignature,PrintHeaderImage,PrintHeaderImagePath,SignDoctorHeight,SignDoctorWidth FROM EMRTemplateReportSetup WITH (NOLOCK) where ReportId=@intReportId";

    //        ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    return ds;
    //}

    public DataSet EditReportName(int Id)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intReportId", Id);
            // string qry = "";
            // qry = "select ReportId,ReportName,TemplateId,DoctorId,SignatureLabel, ShowPageNoInPageFooter, ShowPrintByInPageFooter, ShowPrintDateInPageFooter, IsPrintHospitalHeader,IsPrintDoctorSignature,PrintHeaderImage,PrintHeaderImagePath FROM EMRTemplateReportSetup WITH (NOLOCK) where ReportId=@intReportId";

            // ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateReportSetupImage", HshIn);//Added by Shabana on 4/4/22 
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }


    public int GetDoctorPrintRxId(int EmployeeId)
    {

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@EmployeeId", EmployeeId);
            string qry = "";
            // qry = "select ISNull(DefaultPrescriptionFormat,0) as DefaultPrescriptionFormat from DoctorDetails where DoctorId=@EmployeeId";
            qry = "UspGetEMRDoctorPrintRxId";
            return (int)objDl.ExecuteScalar(CommandType.StoredProcedure, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }
    public int GetDefaultPrintRxId(string TemplateName)
    {

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@TemplateName", TemplateName);
            string qry = "";
            qry = "select ISNull(ReportId,0) as DefaultPrescriptionFormat  from EMRTemplateReportSetup where ReportName=@TemplateName";

            return (int)objDl.ExecuteScalar(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }

    public DataSet getPrintTemplateReportSetup(string ReportType)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@chvReportType", ReportType);

            string qry = "SELECT ReportId, ReportName, TemplateId, DoctorId, SignatureLabel, ShowPageNoInPageFooter, ShowPrintByInPageFooter, ShowPrintDateInPageFooter, IsPrintHospitalHeader, IsPrintDoctorSignature, MarginTop, MarginBottom, MarginLeft, MarginRight, HeaderId, ReportFooterText FROM EMRTemplateReportSetup WITH (NOLOCK) WHERE ReportType = @chvReportType AND Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public bool getPrintTpltRtDrSignatureCheck()
    {
        bool IsAllow = false;
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            string qry = "SELECT IsPrintDoctorSignature FROM EMRTemplateReportSetup WITH (NOLOCK) WHERE ReportType = 'PT' AND Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]))
                    {
                        IsAllow = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            ds.Dispose();
            objDl = null;
        }
        return IsAllow;
    }
    public bool getPrintTemplateReportHeaderCheck()
    {
        bool IsAllow = false;
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            string qry = "SELECT IsPrintHospitalHeader FROM EMRTemplateReportSetup WITH (NOLOCK) WHERE ReportType = 'PT' AND Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]))
                    {
                        IsAllow = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            ds.Dispose();
            objDl = null;
        }
        return IsAllow;
    }
    public DataSet getEMRTemplateGroup(int HospId, int iTemplateId, string sTemplateIdType)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string qry = "";
            HshIn.Add("@intHospId", HospId);
            HshIn.Add("@intTemplateId", iTemplateId);
            if (sTemplateIdType == "TG")
            {
                qry = "  SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.CODE AS TemplateTypeCode FROM SecModulePageTemplates mpt WITH (NOLOCK) INNER JOIN EMRTemplate et WITH (NOLOCK) ON ET.Id=mpt.TemplateId left join EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID " +
                              " WHERE mpt.PAGEID=@intTemplateId AND et.Active = 1 AND mpt.Active=1 AND et.HospitalLocationID = @intHospId ORDER BY et.TemplateName  ";
            }
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }


    public DataSet getEMRTemplateTypeWise(int HospId, string TemplateIdTypeCode, string TemplateName)
    {

        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intHospId", HospId);
            HshIn.Add("@chvTemplateIdTypeCode", TemplateIdTypeCode);
            HshIn.Add("@chvTemplateName", "%" + TemplateName + "%");
            HshIn.Add("@EncounterId", common.myInt(HttpContext.Current.Session["EncounterId"]).ToString());

            ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEMRTemplateTypeWise", HshIn);

            return ds;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

    }


    public DataSet getEMRTemplate(int HospId, string EmployeeType, int SpecialisationId,
                                    int iServiceId, string ApplicableFor, string sTemplateIdType)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string qry = "";
            HshIn.Add("@intHospId", HospId);
            HshIn.Add("@chrApplicableFor", ApplicableFor);

            if (iServiceId > 0)
            {
                HshIn.Add("@iServiceId", iServiceId);

                qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName,t.Code AS TemplateTypeCode,et.Code as TemplateCode, et.DocumentNo " +
                      " FROM EMRTemplate et WITH (NOLOCK) left join EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID " +
                      " INNER JOIN EMRTemplateServiceTagging es WITH (NOLOCK) on es.TemplateId = et.id " +
                      " WHERE et.Active = 1 " +
                      " AND es.Serviceid = @iServiceId " +
                      " AND (ISNULL(et.ApplicableFor, 'B') = 'B' OR ISNULL(et.ApplicableFor, 'B') = @chrApplicableFor) " +
                      " AND et.HospitalLocationID = @intHospId " +
                      " ORDER BY et.TemplateName";
            }
            else if (sTemplateIdType == "TT")
            {
                qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName ,t.Code AS TemplateTypeCode,et.Code as TemplateCode , et.DocumentNo " +
                            " FROM EMRTemplate et WITH (NOLOCK) left join EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID " +
                            " WHERE et.Active = 1 " +
                            " AND (ISNULL(et.ApplicableFor, 'B') = 'B' OR ISNULL(et.ApplicableFor, 'B') = @chrApplicableFor) " +
                            " AND et.HospitalLocationID = @intHospId " +
                            " ORDER BY et.TemplateName ";
            }
            else if (sTemplateIdType == "TG")
            {
                HshIn.Add("@intTemplateId", ApplicableFor);
                qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName ,t.Code AS TemplateTypeCode,et.Code as TemplateCode, et.DocumentNo " +
                           " FROM EMRTemplate et WITH (NOLOCK) INNER JOIN SecModulePageTemplates mpt WITH (NOLOCK) ON ET.Id=mpt.TemplateId " +
                           " LEFT JOIN EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID  " +
                           " WHERE et.Active = 1 " +
                           " AND (ISNULL(et.ApplicableFor, 'B') = 'B' OR ISNULL(et.ApplicableFor, 'B') = @chrApplicableFor) " +
                           " AND et.HospitalLocationID = @intHospId  AND mpt.PAGEID=@intTemplateId " +
                           " ORDER BY et.TemplateName ";
            }
            else
            {
                HshIn.Add("@intSpecialisationId", SpecialisationId);
                if (EmployeeType.Trim() == "N")
                {
                    qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName ,t.Code AS TemplateTypeCode,et.Code as TemplateCode , et.DocumentNo " +
                        " FROM EMRTemplate et WITH (NOLOCK)  left join EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID " +
                        " INNER JOIN SpecialisationMaster sm WITH (NOLOCK) ON et.SpecialisationId = sm.ID and sm.Code='N' " +
                        " WHERE et.Active = 1 " +
                        " AND (ISNULL(et.ApplicableFor, 'B') = 'B' OR ISNULL(et.ApplicableFor, 'B') = @chrApplicableFor) " +
                        " AND et.HospitalLocationID = @intHospId " +
                        " Union all " +
                        " SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName ,t.Code AS TemplateTypeCode ,et.Code as TemplateCode , et.DocumentNo " +
                        " FROM EMRTemplate et  WITH (NOLOCK) left join EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID  " +
                        " WHERE et.Active = 1  and ISNULL(et.PublicTemplate,0) = 1 " +
                        " AND et.HospitalLocationID = @intHospId " +
                        " ORDER BY et.TemplateName ";
                }
                else
                {
                    qry = "SELECT et.Id AS TemplateId, et.TemplateName, et.TemplateTypeID, et.EntryType, et.SpecialisationId, t.TypeName ,t.Code AS TemplateTypeCode,et.Code as TemplateCode , et.DocumentNo " +
                        " FROM EMRTemplate et WITH (NOLOCK) left join EMRTemplateTypes t WITH (NOLOCK) on t.ID = et.TemplateTypeID " +
                        " WHERE et.Active = 1 " +
                        " AND (ISNULL(et.ApplicableFor, 'B') = 'B' OR ISNULL(et.ApplicableFor, 'B') = @chrApplicableFor) " +
                        " AND (et.SpecialisationId = @intSpecialisationId OR et.SpecialisationId IS NULL) " +
                        " AND et.HospitalLocationID = @intHospId " +
                        " ORDER BY et.TemplateName ";
                }
            }
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRTemplate(int HospId, string EmployeeType, int SpecialisationId, int iServiceId, string ApplicableFor,
                               int iTemplateId, string sTemplateIdType, int iFacilityId)
    {
        return getEMRTemplate(HospId, EmployeeType, SpecialisationId, iServiceId, ApplicableFor,
                         iTemplateId, sTemplateIdType, iFacilityId, false);
    }
    public DataSet getEMRTemplate(int HospId, string EmployeeType, int SpecialisationId, int iServiceId, string ApplicableFor,
                              int iTemplateId, string sTemplateIdType, int iFacilityId, bool IsAddendum)
    {
        return getEMRTemplate(HospId, EmployeeType, SpecialisationId, iServiceId, ApplicableFor,
                         iTemplateId, sTemplateIdType, iFacilityId, IsAddendum, 0, 0, 0, false, 0, 0);
    }
    public DataSet getEMRTemplate(int HospId, string EmployeeType, int SpecialisationId, int iServiceId, string ApplicableFor,
                         int iTemplateId, string sTemplateIdType, int iFacilityId, bool IsAddendum, int InvoiceId,
                         int RegistrationId, int EncounterId, bool IsSuperUserLogin, int FreeTextTemplateId, int DoctorId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvApplicableFor", ApplicableFor);
            HshIn.Add("@chvEmployeeType", EmployeeType);
            HshIn.Add("@intTemplateId", iTemplateId);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intSpecialisationId", SpecialisationId);

            if (IsAddendum)
            {
                HshIn.Add("@chvTemplateType", "AD");
            }
            else
            {
                HshIn.Add("@chvTemplateType", sTemplateIdType);
            }


            HshIn.Add("@intInvoiceId", InvoiceId);


            HshIn.Add("@intRegistrationId", RegistrationId);


            HshIn.Add("@intEncounterId", EncounterId);
            if (IsSuperUserLogin)
            {
                HshIn.Add("@bitIsSuperUserLogin", 1);
            }
            if (FreeTextTemplateId > 0)
            {
                HshIn.Add("@FreeTextTemplateId", FreeTextTemplateId);
            }
            if (DoctorId > 0)
            {
                HshIn.Add("@intDoctorId", DoctorId);
            }

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUserWiseTemplate", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRReportSetupSection(int ReportId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intReportId", ReportId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRReportTemplates", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }


    public DataSet getEMRTemplateReportSetupSection(int iHospitalLocationId, int TemplateId, int ReportId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intTemplateId", TemplateId);
            HshIn.Add("@inyiHospitalLocationId", iHospitalLocationId);

            string qry = "SELECT  sec.SectionId, sec.SectionName, sec.ParentId " +
                 " FROM EMRTemplateSections sec WITH (NOLOCK) " +
                 " WHERE sec.TemplateID = @intTemplateId " +
                 " AND sec.Active = 1 " +
                 " ORDER BY sec.Hierarchy, sec.SequenceNo ";


            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }
    public DataSet getEMRTemplateReportSetupSection(int TemplateId, int ReportId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intTemplateId", TemplateId);
            HshIn.Add("@intReportId", ReportId);

            string qry = "SELECT  sec.SectionId, sec.SectionName, sec.ParentId, sec.Tabular " +
                    " FROM EMRTemplateSections sec WITH (NOLOCK) " +
                    " WHERE sec.TemplateID = @intTemplateId " +
                    " AND sec.Active = 1 " +
                    " ORDER BY sec.Hierarchy, sec.SequenceNo ";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRReportSetupSection(int iHospitalLocationId, int TemplateId, int ReportId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intTemplateId", TemplateId);
            HshIn.Add("@inyiHospitalLocationId", iHospitalLocationId);

            string qry = "SELECT  sec.SectionId, sec.SectionName, sec.ParentId,'D' AS Type,tem.TemplateName,'Dynamic' AS  TemplateTypeName " +
                 " FROM EMRTemplateSections sec WITH (NOLOCK) INNER JOIN EMRTemplate tem WITH (NOLOCK) ON SEC.TemplateID=tem.Id " +
                 " WHERE sec.TemplateID = @intTemplateId " +
                 " AND sec.Active = 1 " +
                 " ORDER BY sec.Hierarchy, sec.SequenceNo ";


            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }
    public string getEMRPrintBlankConsultantSignatureLabel(int hospitallocationid, int facilityid)
    {
        DataSet ds = new DataSet();
        string flag = string.Empty;
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inthospitalid", hospitallocationid);
            HshIn.Add("@intfacilityid", facilityid);


            string qry = "select value from hospitalsetup where Flag='EMRPrintBlankConsultantSignatureLabel' and HospitalLocationId=@inthospitalid and facilityId=@intfacilityid";
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                flag = common.myStr(ds.Tables[0].Rows[0]["value"]);

            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return flag;
    }

    public DataSet getEMRReportSetupTemplateFields(int SectionId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intSectionId", SectionId);

            string qry = "SELECT etf.FieldId, etf.FieldName, ets.SectionId, ets.SectionName, 'D' AS Type, 'Dynamic' AS TemplateTypeName, et.TemplateName FROM EMRTemplateFields etf WITH (NOLOCK) INNER JOIN EMRTemplateSections ets WITH (NOLOCK) ON ets.SectionId = etf.SectionId INNER JOIN EMRTemplate et WITH (NOLOCK) ON et.Id = ets.TemplateId WHERE etf.SectionID = @intSectionId AND etf.Hierarchy = 0 AND etf.Active = 1 ORDER BY etf.SequenceNo";
            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRTemplateReportSetup(int ReportId, int TemplateId, int DoctorId, string Type, int Active, int HospId, int EncodedBy)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@intReportId", ReportId);
        HshIn.Add("@intTemplateId", TemplateId);
        HshIn.Add("@intDoctorId", DoctorId);
        HshIn.Add("@chvFlag", Type);
        HshIn.Add("@bitActive", Active);
        HshIn.Add("@inyHospitalLocationId", HospId);

        HshIn.Add("@intEncodedBy", EncodedBy);

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateReportSetup", HshIn, HshOut);

        return ds;
    }
    public DataSet getEMRTemplateReportSequence(int ReportId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intReportId", ReportId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetTemplateReportSequence", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRTemplateReportSetupDetails(int ReportId, string FieldType)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intReportId", ReportId);
            string qry = "";

            if (FieldType == "T")
            {
                qry = "SELECT SectionId " +
                        " FROM EMRTemplateSections WITH (NOLOCK) " +
                        " WHERE TemplateID = @intReportId AND Active = 1" +
                        " ORDER BY SequenceNo";
            }
            else
            {
                qry = "SELECT SectionId " +
                        " FROM EMRTemplateReportSetupDetails WITH (NOLOCK) " +
                        " WHERE ReportId = @intReportId AND Active = 1" +
                        " ORDER BY SequenceNo";
            }

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public string SaveEMRTemplateReportName(int RportId, string ReportName, int Templateid, int DoctorId, string SignatureLabel,
                                            int Status, bool ShowPageNumber, bool ShowPrintBy, bool ShowPrintDate, int EncodedBy,
                                            string HeadingName, string ReportType, int HeaderId, bool IsPrintHospitalHeader,
                                            bool IsPrintDoctorSignature, bool IsShowFilledTemplates, bool IsCheckListRequired,
                                            int MarginLeft, int MarginRight, int MarginTop, int MarginBottom, string ReportFooterText,
                                            bool IsDefaultForOP, string PageSize, string OtherImageTextUrl  /*yogesh   int signHeight, int signWidth*/
        )
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        HshIn.Add("@intReportId", RportId);
        HshIn.Add("@chvReportName", ReportName);
        HshIn.Add("@intTemplateid", Templateid);
        if (DoctorId != 0)
        {
            HshIn.Add("@intDoctorId", DoctorId);
        }
        HshIn.Add("@chvSignatureLabel", SignatureLabel);
        HshIn.Add("@bitActive", Status);
        HshIn.Add("@bitShowPageNo", ShowPageNumber);
        HshIn.Add("@bitShowPrintBy", ShowPrintBy);
        HshIn.Add("@bitShowPrintDate", ShowPrintDate);
        HshIn.Add("@intEncodedBy", EncodedBy);
        HshIn.Add("@chvHeadingName", HeadingName);
        HshIn.Add("@ReportType", ReportType);
        if (HeaderId > 0)
        {
            HshIn.Add("@intHeaderId", HeaderId);
        }
        HshIn.Add("@bitIsPrintHospitalHeader", IsPrintHospitalHeader);
        HshIn.Add("@bitIsPrintDoctorSignature", IsPrintDoctorSignature);
        HshIn.Add("@bitIsShowFilledTemplates", IsShowFilledTemplates);
        HshIn.Add("@bitIsCheckListRequired", IsCheckListRequired);

        HshIn.Add("@intMarginLeft", MarginLeft);
        HshIn.Add("@intMarginRight", MarginRight);
        HshIn.Add("@intMarginTop", MarginTop);
        HshIn.Add("@intMarginBottom", MarginBottom);
        HshIn.Add("@ReportFooterText", ReportFooterText);
        HshIn.Add("@IsDefaultForOP", IsDefaultForOP);
        HshIn.Add("@chvPageSize", PageSize);
        if (OtherImageTextUrl.Length > 0)
        {
            HshIn.Add("@OtherImageTextUrl", OtherImageTextUrl);
        }
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateReportName", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }

    public string SaveEMRTemplateReportName(int RportId, string ReportName, int Templateid, int DoctorId, string SignatureLabel,
                                            int Status, bool ShowPageNumber, bool ShowPrintBy, bool ShowPrintDate, int EncodedBy,
                                            string HeadingName, string ReportType, int HeaderId, bool IsPrintHospitalHeader,
                                            bool IsPrintDoctorSignature, bool IsShowFilledTemplates, bool IsCheckListRequired,
                                            int MarginLeft, int MarginRight, int MarginTop, int MarginBottom, string ReportFooterText,
                                            bool IsDefaultForOP, string PageSize, string OtherImageTextUrl, bool IsShowHeaderImage, bool IsShowFooterImage,
                                            string PrintVersionCode, string HeaderImageUrl, string FooterImageUrl  /*yogesh    int signHeight, int signWidth*/
                                            )
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        HshIn.Add("@intReportId", RportId);
        HshIn.Add("@chvReportName", ReportName);
        HshIn.Add("@intTemplateid", Templateid);
        if (DoctorId != 0)
        {
            HshIn.Add("@intDoctorId", DoctorId);
        }
        HshIn.Add("@chvSignatureLabel", SignatureLabel);
        HshIn.Add("@bitActive", Status);
        HshIn.Add("@bitShowPageNo", ShowPageNumber);
        HshIn.Add("@bitShowPrintBy", ShowPrintBy);
        HshIn.Add("@bitShowPrintDate", ShowPrintDate);
        HshIn.Add("@intEncodedBy", EncodedBy);
        HshIn.Add("@chvHeadingName", HeadingName);
        HshIn.Add("@ReportType", ReportType);
        if (HeaderId > 0)
        {
            HshIn.Add("@intHeaderId", HeaderId);
        }
        HshIn.Add("@bitIsPrintHospitalHeader", IsPrintHospitalHeader);
        HshIn.Add("@bitIsPrintDoctorSignature", IsPrintDoctorSignature);
        HshIn.Add("@bitIsShowFilledTemplates", IsShowFilledTemplates);
        HshIn.Add("@bitIsCheckListRequired", IsCheckListRequired);

        HshIn.Add("@intMarginLeft", MarginLeft);
        HshIn.Add("@intMarginRight", MarginRight);
        HshIn.Add("@intMarginTop", MarginTop);
        HshIn.Add("@intMarginBottom", MarginBottom);
        HshIn.Add("@ReportFooterText", ReportFooterText);
        HshIn.Add("@IsDefaultForOP", IsDefaultForOP);
        HshIn.Add("@chvPageSize", PageSize);
        if (OtherImageTextUrl.Length > 0)
        {
            HshIn.Add("@OtherImageTextUrl", OtherImageTextUrl);
        }

        //change palendra
        HshIn.Add("@bitIsShowHeaderImage", IsShowHeaderImage);
        HshIn.Add("@bitIsShowFooterImage", IsShowFooterImage);
        HshIn.Add("@PrintVersionCode", PrintVersionCode);
        if (HeaderImageUrl.Length > 0)
        {
            HshIn.Add("@HeaderImageUrl", HeaderImageUrl);
        }
        if (FooterImageUrl.Length > 0)
        {
            HshIn.Add("@FooterImageUrl", FooterImageUrl);
        }

        /* Yogesh */
        //HshIn.Add("@SignDoctorHeight", signHeight);
        //HshIn.Add("@SignDoctorWidth", signWidth);


        //chnage palendra
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateReportName", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }

    public string SaveEMRTemplateReportSetup(int ReportId, int Active, string xmlSections, int HospId, int EncodedBy)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@intReportId", ReportId);
        HshIn.Add("@bitActive", Active);
        HshIn.Add("@xmlSections", xmlSections);
        HshIn.Add("@inyHospitalLocationId", HospId);

        HshIn.Add("@intEncodedBy", EncodedBy);

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateReportSetup", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }

    public DataSet getChkUserSetup(int DoctorId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intDoctorId", DoctorId);

            string qry = "SELECT (CASE WHEN ISNULL(us.SetupId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
                    " emp.Id AS EmployeeId, dbo.GetDoctorName(emp.Id) as DoctorTeamName " +
                    " FROM Employee emp WITH (NOLOCK) " +
                    " LEFT OUTER JOIN EMRUserSetup us WITH (NOLOCK) ON emp.ID = us.DoctorTeamId AND us.Active = 1 AND us.DoctorId = @intDoctorId " +
                    " WHERE emp.Active = 1 AND emp.ID <> @intDoctorId AND @intDoctorId > 0" +
                    " ORDER BY DoctorTeamName";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataTable getDischargeType(int Encounterid)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intEncounterid", Encounterid);

            string qry = "SELECT psm.name as DischargeType " +
                        "FROM EncounterStatus es WITH(NOLOCK) " +
                        "LEFT JOIN PatientStatusMaster psm WITH(NOLOCK) ON es.DischargeStatus = psm.Id AND psm.Status = 'A' " +
                        "WHERE es.encounterid = @intEncounterid AND es.Active = 1 AND es.DischargeStatus IS NOT NULL ";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds.Tables[0];
    }


    public DataTable getAllFontName()
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string qry = "select fontid,fontname from  fontname ";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds.Tables[0];
    }

    public DataSet getUserSetupDoctor(int EmployeeId, int HospId, int EncodedBy)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@intEmployeeId", EmployeeId);
        HshIn.Add("@inyHospitalLocationId", HospId);

        HshIn.Add("@intEncodedBy", EncodedBy);

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRUserSetup", HshIn, HshOut);

        return ds;
    }

    public string SaveEMRUserSetup(int DoctorId, string xmlIds, int HospId, int EncodedBy)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@intDoctorId", DoctorId);
        HshIn.Add("@xmlIds", xmlIds);
        HshIn.Add("@inyHospitalLocationId", HospId);
        HshIn.Add("@intEncodedBy", EncodedBy);

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRUserSetup", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }

    public DataSet getEMRTemplateWordFieldList(int TemplateId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intTemplateId", TemplateId);

            string qry = "SELECT tf.FieldId, tf.SectionId, ts.SectionName +', '+ tf.FieldName AS SectionName " +
                        " FROM EMRTemplateFields tf WITH (NOLOCK) " +
                        " INNER JOIN EMRTemplateSections ts WITH (NOLOCK) ON tf.SectionId =  ts.SectionId AND tf.DisplayTitle = 1 AND tf.Active = 1 " +
                        " WHERE ts.TemplateID = @intTemplateId " +
                        " AND tf.FieldType = 'W' And tf.PrintRequired = 1 ";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRTemplateWordData(int FieldId, int EncounterId, int RegistrationId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            string qry = "SELECT ValueWordProcessor " +
                        " FROM EMRPatientNotesData WITH (NOLOCK) " +
                        " WHERE FieldId = @intFieldId AND EncounterId = @intEncounterId AND RegistrationId = @intRegistrationId AND Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataTable getTemplateDataObjectQuery(int DataObjectId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intDataObjectId", DataObjectId);

            string qry = "SELECT Query, ObjectFieldType " +
                    " FROM EMRTemplateDataObjects WITH (NOLOCK) " +
                    " WHERE Id = @intDataObjectId " +
                    " AND Active = 1 ";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    Query = common.myStr(ds.Tables[0].Rows[0]["Query"]);
            //}
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds.Tables[0];
    }

    public DataSet getDataObjectExecute(string Query, Hashtable Parameter)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = Parameter;

            ds = objDl.FillDataSet(CommandType.Text, Query, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public string getDataObjectValue(int DataObjectId)
    {
        string strOutput = "";

        try
        {
            DataSet ds = new DataSet();
            Hashtable coll = new Hashtable();

            DataTable dtObject = getTemplateDataObjectQuery(DataObjectId);

            switch (DataObjectId)
            {
                case 26: //Patient Last BMI
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));
                    coll.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["BMI"]);
                    }

                    break;

                case 27: //Patient Vitals Last Height
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));
                    coll.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["Height"]);
                    }

                    break;

                case 28: //Patient Vitals Last Weight
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));
                    coll.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["Weight"]);
                    }

                    break;

                case 29: //Template Female Diagnosis
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));
                    coll.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["Diagnosis"]);
                    }

                    break;

                case 30: //Template Male Diagnosis
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));
                    coll.Add("@intRegistrationId", common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["Diagnosis"]);
                    }

                    break;

                case 31: //Template Female Spouse Diagnosis

                    coll.Add("@intIVFId", common.myInt(System.Web.HttpContext.Current.Session["IVFId"]));
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["Diagnosis"]);
                    }

                    break;

                case 32: //Template Prescaption
                    strOutput = "";

                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = "POST OCR ORDER TR. STANDARD : ";
                        strOutput += StripHTML(common.myStr(ds.Tables[0].Rows[0]["ValueWordProcessor"]));

                        if (ds.Tables[0].Rows.Count > 1)
                        {
                            strOutput += Environment.NewLine + Environment.NewLine + "POST ET ORDER TR. STANDARD : ";
                            strOutput += StripHTML(common.myStr(ds.Tables[0].Rows[1]["ValueWordProcessor"]));
                        }
                    }

                    break;

                case 33: //Template Male Spouse Diagnosis
                    coll.Add("@intIVFId", common.myInt(System.Web.HttpContext.Current.Session["IVFId"]));
                    coll.Add("@intEncounterId", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));

                    ds = getDataObjectExecute(common.myStr(dtObject.Rows[0]["Query"]), coll);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strOutput = common.myStr(ds.Tables[0].Rows[0]["Diagnosis"]);
                    }

                    break;
            }
            dtObject.Dispose();
        }
        catch
        {
        }
        return strOutput;
    }
    public DataTable getDataObjectValue(int FacilityId, int DataObjectId)
    {
        DataSet ds = new DataSet();
        try
        {
            Hashtable coll = new Hashtable();
            DataTable dt = getTemplateDataObjectQuery(DataObjectId);
            switch (DataObjectId)
            {
                case 67: // Populate Scrub Nurse List
                    coll.Add("@intfacilityid", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));

                    ds = getDataObjectExecute(common.myStr(dt.Rows[0]["Query"]), coll);

                    break;
            }
        }
        catch
        {
        }
        return ds.Tables[0];
    }
    private string StripHTML(string source)
    {
        try
        {
            string result;

            // Remove HTML Development formatting
            // Replace line breaks with space
            // because browsers inserts space
            result = source.Replace("\r", " ");
            // Replace line breaks with space
            // because browsers inserts space
            result = result.Replace("\n", " ");
            // Remove step-formatting
            result = result.Replace("\t", string.Empty);
            // Remove repeating spaces because browsers ignore them
            result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                  @"( )+", " ");

            // Remove the header (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*head([^>])*>", "<head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*head( )*>)", "</head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<head>).*(</head>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all scripts (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*script([^>])*>", "<script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*script( )*>)", "</script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            //result = System.Text.RegularExpressions.Regex.Replace(result,
            //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
            //         string.Empty,
            //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<script>).*(</script>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // remove all styles (prepare first by clearing attributes)
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*style([^>])*>", "<style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"(<( )*(/)( )*style( )*>)", "</style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(<style>).*(</style>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert tabs in spaces of <td> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*td([^>])*>", "\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line breaks in places of <BR> and <LI> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*br( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*li( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // insert line paragraphs (double line breaks) in place
            // if <P>, <DIV> and <TR> tags
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*div([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*tr([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<( )*p([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // Remove remaining tags like <a>, links, images,
            // comments etc - anything that's enclosed inside < >
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"<[^>]*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // replace special characters:
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @" ", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&bull;", " * ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lsaquo;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&rsaquo;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&trade;", "(tm)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&frasl;", "/",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&lt;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&gt;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&copy;", "(c)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&reg;", "(r)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove all others. More can be added, see
            // http://hotwired.lycos.com/webmonkey/reference/special_characters/
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     @"&(.{2,6});", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // for testing
            //System.Text.RegularExpressions.Regex.Replace(result,
            //       this.txtRegex.Text,string.Empty,
            //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            // make line breaking consistent
            result = result.Replace("\n", "\r");

            // Remove extra line breaks and tabs:
            // replace over 2 breaks with 2 and over 4 tabs with 4.
            // Prepare first to remove any whitespaces in between
            // the escaped characters and remove redundant tabs in between line breaks
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\t)", "\t\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\t)( )+(\r)", "\t\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)( )+(\t)", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove redundant tabs
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Remove multiple tabs following a line break with just one tab
            result = System.Text.RegularExpressions.Regex.Replace(result,
                     "(\r)(\t)+", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            // Initial replacement target string for line breaks
            string breaks = "\r\r\r";
            // Initial replacement target string for tabs
            string tabs = "\t\t\t\t\t";
            for (int index = 0; index < result.Length; index++)
            {
                result = result.Replace(breaks, "\r\r");
                result = result.Replace(tabs, "\t\t\t\t");
                breaks = breaks + "\r";
                tabs = tabs + "\t";
            }

            // That's it.
            return result;
        }
        catch
        {
            return source;
        }
    }

    public DataSet getEMRTemplateVisitRecoreds(int EncounterId, int TemplateId, int FacilityId)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@intEncounterId", EncounterId);
        HshIn.Add("@intTemplateId", TemplateId);
        HshIn.Add("@intFacilityId", FacilityId);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateVisitRecoreds", HshIn, HshOut);

        return ds;
    }

    public DataSet getInvestigationResult(int RegistrationId, int serviceid, DateTime fdate, DateTime tdate)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@regno", RegistrationId);
        HshIn.Add("@serviceid", serviceid);
        HshIn.Add("@fdate", fdate);
        HshIn.Add("@tdate", tdate);
        HshIn.Add("@Encounterid", common.myInt(System.Web.HttpContext.Current.Session["EncounterId"]));
        HshIn.Add("@intFacilityId", common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]));

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "Get_Patient_Investigation_Result_reg_opd", HshIn);

        return ds;
    }

    public DataSet getSpecialResult(int labregno, int serviceid)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@labregno", labregno);
        HshIn.Add("@serviceid", serviceid);

        ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetSpecialResult", HshIn);

        return ds;
    }

    public string closeEncounter(int EncounterId, int EncodedBy)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@intEncounterId", EncounterId);
        HshIn.Add("@intEncodedBy", EncodedBy);

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCloseEncounter", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }

    public bool IsEncounterClose(int EncounterId)
    {
        bool IsClose = false;

        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intEncounterId", EncounterId);

            string qry = "SELECT enc.EMRStatusId " +
                        " FROM Encounter enc WITH (NOLOCK) " +
                        " INNER JOIN StatusMaster sm WITH (NOLOCK) ON enc.EMRStatusId = sm.StatusId AND sm.StatusType = 'EMR' AND sm.Code = 'C' " +
                        " WHERE enc.Id = @intEncounterId AND enc.Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                IsClose = true;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return IsClose;
    }

    public DataSet getEMRSpecialisationTemplate(int UserId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospId", common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            HshIn.Add("@intUserId", UserId);

            string qry = "SELECT DISTINCT tm.Id AS TemplateId, tm.TemplateName " +
                    " FROM EMRTemplate tm WITH (NOLOCK) " +
                    " INNER JOIN DoctorDetails dd WITH (NOLOCK) ON tm.SpecialisationId = dd.SpecialisationId " +
                    " INNER JOIN Users us WITH (NOLOCK) ON dd.DoctorId = us.EmpID " +
                    " WHERE tm.Active = 1 " +
                    " AND us.ID = @intUserId " +
                    " AND tm.HospitalLocationID = @inyHospId " +
                    " ORDER BY tm.TemplateName";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getEMRGeneralTemplate()
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospId", common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

            string qry = "SELECT DISTINCT tm.Id AS TemplateId, tm.TemplateName " +
                        " FROM EMRTemplate tm WITH (NOLOCK) " +
                        " WHERE tm.Active = 1 " +
                        " AND tm.IsGeneralTemplate = 1 " +
                        " AND tm.HospitalLocationID = @inyHospId " +
                        " ORDER BY tm.TemplateName";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }

    public DataSet getDoctorSpecialisation(int DoctorId)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intDoctorId", DoctorId);

            string qry = "SELECT DISTINCT dd.SpecialisationId, sm.Name as SpecialisationName, sm.Code as SpecialisationCode " +
                    " FROM Employee emp WITH (NOLOCK) " +
                    " INNER JOIN DoctorDetails dd WITH (NOLOCK) ON emp.ID = dd.DoctorId " +
                    " INNER JOIN SpecialisationMaster sm WITH (NOLOCK) ON dd.SpecialisationId = sm.ID " +
                    " WHERE emp.ID = @intDoctorId " +
                    " AND emp.Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }




    public int getFirstTemplateEditBy(int EncounterId, int RecordId, int TemplateId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        int encodedBy = 0;
        try
        {
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRecordId", RecordId);
            HshIn.Add("@intTemplateId", TemplateId);

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT TOP 1 nd.EncodedBy ");
            sb.Append(" FROM EMRPatientNotesData nd WITH (NOLOCK) ");
            sb.Append(" INNER JOIN EMRTemplateFields tf WITH (NOLOCK) ON nd.FieldId = tf.FieldId ");
            sb.Append(" INNER JOIN EMRTemplateSections ts WITH (NOLOCK) ON tf.SectionId = ts.SectionId AND ts.TemplateID = @intTemplateId ");
            sb.Append(" WHERE nd.EncounterId = @intEncounterId ");
            sb.Append(" AND nd.RecordId = @intRecordId ");
            sb.Append(" AND nd.Active = 1 ");
            sb.Append(" ORDER BY nd.Id ");

            ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                encodedBy = common.myInt(ds.Tables[0].Rows[0]["EncodedBy"]);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objDl = null;
            HshIn = null;
            ds.Dispose();
        }
        return encodedBy;

    }
    public DataSet getSingleScreenTemplates(int HospId, int FacilityId, int SpecialisationId, int DoctorId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        try
        {
            HshIn.Add("@intHospId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intSpecialisationId", SpecialisationId);
            HshIn.Add("@intDoctorId", DoctorId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetEMRTemplatesSetup", HshIn);

            //StringBuilder sb = new StringBuilder();

            //sb.Append("SELECT DISTINCT sst.Id, sst.TemplateName, (CASE WHEN ssut.Id IS NULL THEN 0 ELSE 1 END) AS IsChk, ssut.IsMandatory, ssut.IsCollapse ");
            //sb.Append(" FROM EMRSingleScreenTemplates sst WITH (NOLOCK) ");
            //sb.Append(" LEFT OUTER JOIN EMRSingleScreenUserTemplates ssut WITH (NOLOCK) ON sst.Id=ssut.TemplateId AND SpecialisationId=@intSpecialisationId AND ssut.Active=1 AND ssut.FacilityId=@intFacilityId ");
            ////sb.Append(" WHERE (sst.HospitalLocationID=@intHospId or sst.HospitalLocationID is null ");
            ////sb.Append(" WHERE sst.HospitalLocationID=@intHospId ");
            //if (DoctorId > 0)
            //{
            //    sb.Append(" AND ssut.DoctorId=@intDoctorId");
            //}

            //sb.Append(" ORDER BY sst.TemplateName ");

            //ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objDl = null;
            HshIn = null;
        }
        return ds;

    }
    public DataSet getSingleScreenUserTemplates(int SpecialisationId, int FacilityId, int DoctorId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intSpecialisationId", SpecialisationId);
            HshIn.Add("@intDoctorId", DoctorId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetSingleScreenTemplates", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objDl = null;
            HshIn = null;
        }
        return ds;
    }
    public string SaveEMRSingleScreenUserTemplates(int HospId, int FacilityId, int SpecialisationId, string xmlTemplateIds, string xmlNonFreeTextTemplateIds, int EncodedBy, int DoctorId)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@inyHospitalLocationId", HospId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intSpecialisationId", SpecialisationId);
        HshIn.Add("@xmlTemplateIds", xmlTemplateIds);
        HshIn.Add("@xmlNonFreeTextTemplateIds", xmlNonFreeTextTemplateIds);
        HshIn.Add("@intEncodedBy", EncodedBy);
        HshIn.Add("@intDoctorId", DoctorId);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRSingleScreenUserTemplates", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }


    public int getEMRFormHeaderIdBasedOnHeaderType(string HeaderType)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        DataSet ds = new DataSet();
        int HeaderId = 1;
        try
        {
            hstInput.Add("@chvHeaderType", common.myStr(HeaderType));
            hstInput.Add("@inyHospId", common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"]));

            ds = objDl.FillDataSet(CommandType.Text, "SELECT HeaderId FROM EMRFormHeader WITH (NOLOCK) WHERE Active = 1 AND HeaderType = @chvHeaderType AND HospitalLocationId = @inyHospId", hstInput);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    HeaderId = common.myInt(ds.Tables[0].Rows[0]["HeaderId"]);
                }
            }
        }
        catch
        {
            objDl = null;
            hstInput = null;
            ds.Dispose();
        }
        finally
        {
        }
        return HeaderId;
    }

    public void getReportSetupMargin(int ReportId, out int MarginTop, out int MarginBottom, out int MarginLeft, out int MarginRight, out string PageSize)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        StringBuilder sb = new StringBuilder();

        Hashtable HshIn = new Hashtable();

        try
        {
            MarginTop = 0;
            MarginBottom = 0;
            MarginLeft = 0;
            MarginRight = 0;
            PageSize = "A4";

            HshIn.Add("@intReportId", ReportId);

            sb.Append("SELECT ISNULL(MarginTop,0) AS MarginTop, ISNULL(MarginBottom,0) AS MarginBottom, ISNULL(MarginLeft,0) AS MarginLeft, ISNULL(MarginRight,0) AS MarginRight, ISNULL(PageSize,'A4') AS PageSize FROM EMRTemplateReportSetup WITH (NOLOCK) WHERE ReportId = @intReportId AND Active = 1");

            ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                MarginTop = common.myInt(ds.Tables[0].Rows[0]["MarginTop"]);
                MarginBottom = common.myInt(ds.Tables[0].Rows[0]["MarginBottom"]);
                MarginLeft = common.myInt(ds.Tables[0].Rows[0]["MarginLeft"]);
                MarginRight = common.myInt(ds.Tables[0].Rows[0]["MarginRight"]);
                PageSize = common.myStr(ds.Tables[0].Rows[0]["PageSize"]);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objDl = null;
            HshIn = new Hashtable();
            sb = null;
            ds.Dispose();
        }
    }

    public DataTable getFacilityMasterAttribute(int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            hstInput.Add("@intFacilityId", common.myStr(FacilityId));

            ds = objDl.FillDataSet(CommandType.Text, "SELECT OPSummaryEmergencyContactNumber, Name AS FacilityName, Address2 AS Address FROM FacilityMaster WITH(NOLOCK) WHERE FacilityID = @intFacilityId AND Active = 1", hstInput);
        }
        catch
        {
            objDl = null;
            hstInput = null;
        }
        finally
        {
        }
        return ds.Tables[0];
    }

    public DataSet GetLanguageMaster(bool IsAllowTranslate)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@bitIsAllowTranslate", IsAllowTranslate);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetLanguageMaster", HshIn);
        }
        catch
        {
        }
        return ds;
    }
    //public string SetDefaultText(int DetailId, string DefaultText)
    //{
    //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
    //    Hashtable HshIn = new Hashtable();
    //    try
    //    {
    //        HshIn.Add("@DetailId", DetailId);
    //        HshIn.Add("@DefaultText", DefaultText);
    //        string qry = "update EMRTemplateReportSetupDetails set DefaultText=@DefaultText where DetailId=@DetailId";
    //        objDl.ExecuteNonQuery(CommandType.Text, qry, HshIn);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw Ex;
    //    }
    //    finally
    //    {
    //        HshIn = null;
    //        objDl = null;
    //    }
    //    return " Record Saved ";
    //}

    public string SetDefaultText(int DetailId, string DefaultText)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable HshIn = new Hashtable();
        try
        {
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update EMRTemplateReportSetupDetails set DefaultText=@DefaultText where DetailId=@DetailId";
            cmd.Parameters.Add("@DetailId", SqlDbType.Int).Value = DetailId;
            cmd.Parameters.Add("@DefaultText", SqlDbType.NVarChar).Value = DefaultText;
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return " Record Saved ";
    }

    public void GetDefaultText(int DetailId, out string DefaultText)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DefaultText = string.Empty;
            HshIn.Add("@DetailId", DetailId);
            sb.Append("select isnull(DefaultText,'') as DefaultText from EMRTemplateReportSetupDetails where  DetailId=@DetailId");
            ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DefaultText = common.myStr(ds.Tables[0].Rows[0]["DefaultText"]);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }

    }

    public DataSet getEMRGridColumnsVisibilitySetup(int FacilityId, string OptionName, int ModuleId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvOptionName", OptionName);
            HshIn.Add("@intModuleId", ModuleId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetGridColumnsVisibilitySetup", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objDl = null;
            HshIn = null;
        }
        return ds;

    }
    public DataSet GetStatusMaster(string StatusType)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@chrStatusType", StatusType);


            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetStatusMaster", HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            objDl = null;
            HshIn = null;
        }
        return ds;

    }
    public bool chkIsDoctorSignatureExists(string FileName)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        bool IsExists = false;

        try
        {
            HshIn.Add("@chvFileName", common.myStr(FileName).Trim());

            string qry = "SELECT ImageId FROM EmployeeSignatureImage WITH(NOLOCK) WHERE Active=1 AND LTRIM(RTRIM(ISNULL(ImageType,'')))=@chvFileName";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myInt(ds.Tables[0].Rows[0]["ImageId"]) > 0)
                    {
                        IsExists = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            ds.Dispose();
            HshIn = null;
            objDl = null;
        }

        return IsExists;
    }

    public DataSet getPrescriptionMandatoryFieldSetup(int FacilityId, int EmployeeId, string VisitType)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        try
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEmployeeId", EmployeeId);
            HshIn.Add("@chvVisitType", VisitType);

            string qry = "SELECT DISTINCT FieldCode, ISNULL(IsMandatory,0) AS IsMandatory " +
                        " FROM EMRPrescriptionMandatoryFieldSetup WITH(NOLOCK) " +
                        " WHERE DoctorId = @intEmployeeId " +
                        " AND FacilityId = @intFacilityId " +
                        " AND(ISNULL(VisitType,'B') = @chvVisitType OR ISNULL(VisitType,'B') = 'B') " +
                        " AND Active = 1";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        return ds;
    }
    public bool getIsNoFollowUpRequired(int RegistrationId, int EncounterId, int FacilityID)
    {
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        bool IsExists = false;

        try
        {
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intFacilityID", FacilityID);

            string qry = "SELECT AppointmentId FROM DoctorAppointment WITH(NOLOCK) WHERE RegistrationId=@intRegistrationId  AND ISNULL(SourceEncounterId,0) = @intEncounterId AND FacilityID=@intFacilityID AND Active=1";

            ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (common.myInt(ds.Tables[0].Rows[0]["AppointmentId"]) > 0)
                    {
                        IsExists = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            ds.Dispose();
            HshIn = null;
            objDl = null;
        }

        return IsExists;
    }
    public void SaveIsNoFollowUpRequired(int EncounterId, bool IsNoFollowUpRequired)
    {
        Hashtable HshIn = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        try
        {
            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@IsNoFollowUpRequired", IsNoFollowUpRequired);

            string qry = "Update Encounter Set IsNoFollowUpRequired=@IsNoFollowUpRequired where Id=@EncounterId";

            objDl.ExecuteNonQuery(CommandType.Text, qry, HshIn);
        }

        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
    }



    public DataSet getDoctorDepartmentDetails(int EncounterId, int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intFacilityId", FacilityId);

            string disString = string.Empty;

            if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I"))
            {
                disString = "SELECT DISTINCT dm1.DepartmentName AS ConsultingDoctorDepartment, dm1.DepartmentId AS ConsultingDoctorDepartmentId, fwd1.DepartmentSpecification AS EntrySite, dd.SpecialisationId " +
                                " FROM Admission en WITH(NOLOCK) " +
                                " LEFT JOIN Employee e2 WITH(NOLOCK) ON en.ConsultingDoctorId = e2.ID " +
                                " LEFT JOIN DepartmentMain dm1 WITH(NOLOCK) ON e2.DepartmentId = dm1.DepartmentID " +
                                " LEFT JOIN FacilityWiseDepartment fwd1 WITH(NOLOCK) ON e2.DepartmentId = fwd1.DepartmentId AND fwd1.FacilityId = @intFacilityId " +
                                " LEFT JOIN DoctorDetails dd WITH (NOLOCK) ON e2.ID = dd.DoctorId " +
                                " WHERE en.EncounterId = @intEncounterId " +
                                " AND en.Active = 1 " +
                                " AND en.FacilityId = @intFacilityId";
            }
            else
            {
                disString = "SELECT DISTINCT dm1.DepartmentName AS ConsultingDoctorDepartment, dm1.DepartmentId AS ConsultingDoctorDepartmentId, fwd1.DepartmentSpecification AS EntrySite, dd.SpecialisationId " +
                                " FROM Encounter en WITH(NOLOCK) " +
                                " LEFT JOIN Employee e2 WITH(NOLOCK) ON en.DoctorId = e2.ID " +
                                " LEFT JOIN DepartmentMain dm1 WITH(NOLOCK) ON e2.DepartmentId = dm1.DepartmentID " +
                                " LEFT JOIN FacilityWiseDepartment fwd1 WITH(NOLOCK) ON e2.DepartmentId = fwd1.DepartmentId AND fwd1.FacilityId = @intFacilityId " +
                                " LEFT JOIN DoctorDetails dd WITH (NOLOCK) ON e2.ID = dd.DoctorId " +
                                " WHERE en.Id = @intEncounterId " +
                                " AND en.Active = 1 " +
                                " AND en.FacilityId = @intFacilityId";
            }

            ds = objDl.FillDataSet(CommandType.Text, disString, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return ds;

    }

    public DataSet getDoctorSignatureDetails(int DoctorId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@intDoctorId", DoctorId);

            string stString = "SELECT TOP 1 ISNULL(tm.Name + ' ','') + ISNULL(e.FirstName,'') + ISNULL(' ' + e.MiddleName,'') + ISNULL(' ' + e.LastName,'') AS DoctorName, " +
                            " e.Education AS DoctorEducation, e.Designation AS DoctorDesignation, " +
                            " e.ID AS DoctorId, dd.UPIN, spm.Name AS DoctorSpecialization, GETUTCDATE() AS SignatureWithDateTime, " +
                            " esi.ImageId, esi.SignatureImage, esi.ImageType ,dd.SignatureLine1, dd.SignatureLine2, dd.SignatureLine3, dd.SignatureLine4 " +
                            " FROM Employee e WITH(NOLOCK) " +
                            " LEFT OUTER JOIN DoctorDetails dd WITH(NOLOCK) ON e.ID = dd.DoctorId " +
                            " LEFT OUTER JOIN SpecialisationMaster spm WITH(NOLOCK) ON dd.SpecialisationId = spm.Id AND spm.Active = 1 " +
                            " LEFT OUTER JOIN EmployeeSignatureImage esi WITH(NOLOCK) ON esi.EmployeeId = e.ID AND esi.Active = 1 " +
                            " LEFT JOIN TitleMaster tm WITH (NOLOCK) ON tm.TitleID = e.TitleId " +
                            " WHERE e.ID = @intDoctorId " +
                            " AND e.Active = 1 " +
                            " ORDER BY esi.ImageId DESC ";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return ds;

    }

    public string getStaticTemplateCode(int StaticTemplateId, int StaticFieldId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        string strCode = string.Empty;
        try
        {
            HshIn.Add("@intStaticTemplateId", StaticTemplateId);
            HshIn.Add("@intStaticFieldId", StaticFieldId);

            string stString = "SELECT DISTINCT t4.CODE AS StaticTemplateCode " +
                                " FROM EMRTemplateFields t3 WITH(NOLOCK) " +
                                " INNER JOIN EMRTemplateStatic t4 WITH(NOLOCK) ON t3.StaticTemplateId = t4.PageId AND t3.Active = 1 AND t4.Active = 1 " +
                                " WHERE t3.FieldId = @intStaticFieldId " +
                                " AND t3.FieldType = 'L' " +
                                " AND t4.PageId = @intStaticTemplateId ";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strCode = Convert.ToString(ds.Tables[0].Rows[0]["StaticTemplateCode"]);
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
            ds.Dispose();
        }
        return strCode;

    }
    public bool GetCheckgenerateInstruction(int ItemId)
    {

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        bool IsVariableInstructions = false;
        try
        {
            HshIn.Add("@ItemId", ItemId);

            string stString = "SELECT IsVariableInstructions FROM PhrItemMaster WITH(NOLOCK) WHERE ItemId = @ItemId AND Active=1";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsVariableInstructions = common.myBool(ds.Tables[0].Rows[0]["IsVariableInstructions"]);
                }
            }
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            HshIn = null;
            objDl = null;
            ds.Dispose();
        }
        return IsVariableInstructions;
    }

    public bool chkEMROutsideLabResultSetupDefine(int DoctorId, int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        bool IsExists = false;
        try
        {
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intFacilityId", FacilityId);

            string stString = "SELECT LabResultSetupId FROM EMROutsideLabResultSetup WITH(NOLOCK) WHERE DoctorId=@intDoctorId AND Active=1 AND FacilityId=@intFacilityId";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsExists = true;
                }
            }
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            HshIn = null;
            objDl = null;
            ds.Dispose();
        }
        return IsExists;
    }

    public DataSet getServiceFromOutsideLabResultSetup(int DoctorId, int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        try
        {
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intFacilityId", FacilityId);

            string stString = "SELECT DISTINCT lrs.ServiceId, ios.ServiceName FROM EMROutsideLabResultSetup lrs WITH(NOLOCK) INNER JOIN ItemOfService ios WITH(NOLOCK) ON lrs.ServiceId = ios.ServiceId AND lrs.Active = 1 AND ios.Active = 1 WHERE DoctorId=@intDoctorId AND FacilityId=@intFacilityId ORDER BY ios.ServiceName";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return ds;

    }

    public bool getIsPatientHeightWeightEntered(int EncounterId, int RegistrationId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        bool IsEntered = false;
        try
        {
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            string stString = "SELECT Id FROM EMRPatientVitalSignDetails WITH(NOLOCK) WHERE EncounterId = @intEncounterId AND RegistrationId = @intRegistrationId AND VitalId in(4,5) AND Active = 1 AND DATEDIFF(DD, VitalEntryDate, GETUTCDATE()) < 8 ";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);

            if (ds.Tables[0].Rows.Count > 1)
            {
                IsEntered = false;
            }
            else
            {
                IsEntered = true;
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return IsEntered;
    }

    public DataTable GetIVFPackageServiceDetails(int PackageId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        Hashtable hstOut = new Hashtable();
        try
        {
            hstInput.Add("@intPackageId", PackageId);
            return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPackageServiceList", hstInput)).Tables[0];
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            hstInput = null;
            hstOut = null;
        }
    }

    public string getEncounterStatusRemarks(int EncounterId, int DischargeStatusId, int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        string rmks = string.Empty;
        try
        {
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intDischargeStatusId", DischargeStatusId);
            HshIn.Add("@intFacilityId", FacilityId);

            string stString = "SELECT CommonRemarks FROM EncounterStatus WITH(NOLOCK) WHERE EncounterId = @intEncounterId AND DischargeStatus = @intDischargeStatusId AND Active = 1 AND FacilityID = @intFacilityId";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                rmks = common.myStr(ds.Tables[0].Rows[0]["CommonRemarks"]);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
            ds.Dispose();
        }
        return rmks;
    }

    public bool getIsDischargeSummaryFinalized(int EncounterId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        bool IsFinalized = false;
        try
        {
            HshIn.Add("@intEncounterId", EncounterId);

            string stString = "SELECT SummaryID FROM EMRPatientSummaryDetails WITH(NOLOCK) WHERE EncounterID=@intEncounterId AND FormatID IN (SELECT ReportId FROM EMRTemplateReportSetup WITH(NOLOCK) WHERE ReportType='DI' AND Active=1) AND Active=1 AND DischargeDate IS NOT NULL AND ISNULL(Finalize,0)=1";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myInt(ds.Tables[0].Rows[0]["SummaryID"]) > 0)
                {
                    IsFinalized = true;
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return IsFinalized;
    }

    public bool getIsChkPendingIndent(int EncounterId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        bool IsAllow = true;
        try
        {
            HshIn.Add("@intEncounterId", EncounterId);

            string stString = "SELECT DISTINCT pim.IndentId FROM ICMIndentMain pim WITH(NOLOCK) INNER JOIN ICMIndentDetails pid WITH(NOLOCK) ON pim.IndentId = pid.IndentId WHERE pim.EncounterId=@intEncounterId AND pim.Active=1 AND ISNULL(pim.IsCompoundedDrugOrder,0)<>1 AND ISNULL(pid.IsStop,0)<>1 AND ISNULL(pid.IsCancel,0)<>1 AND ISNULL(pid.isApproved,'P')<>'A'";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myInt(ds.Tables[0].Rows[0]["IndentId"]) > 0)
                {
                    IsAllow = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return IsAllow;
    }


    public string getEncounterLegacyData(int EncounterId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        string strSummaryHTML = string.Empty;

        try
        {
            HshIn.Add("@intEncounterId", EncounterId);

            string stString = "SELECT (CASE VisitType WHEN 'I' THEN DischargeSummaryHTML WHEN 'O' THEN OPSummary WHEN 'H' THEN MHCSummary ELSE OPSummary END) AS Summary FROM EncounterLegacy WITH(NOLOCK) WHERE Id=@intEncounterId";

            ds = objDl.FillDataSet(CommandType.Text, stString, HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                strSummaryHTML = common.myStr(ds.Tables[0].Rows[0]["Summary"]);
            }
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            HshIn = null;
            objDl = null;
        }
        return strSummaryHTML;
    }

    public DataTable SwitchIVFPartner(int RegistrationId)
    {
        objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        hstInput = new Hashtable();
        hstOut = new Hashtable();
        try
        {
            hstInput.Add("@RegistrationId", RegistrationId);
            return (objDl.FillDataSet(CommandType.StoredProcedure, "uspIVFSwitchPartner", hstInput)).Tables[0];
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
        }
    }


    public string FormatDateDateMonthYear(string strDateOfBirth)
    {
        if (strDateOfBirth != string.Empty && strDateOfBirth != "__/__/____")
        {
            //input string should be in MM/dd/yyyy
            char[] chSperator = { '/' };
            string[] strResult = strDateOfBirth.Split(chSperator);
            StringBuilder str = new StringBuilder();
            if (strResult[1].Length == 1)
                str.Append("0" + strResult[1]);
            else
                str.Append(strResult[1]);
            str.Append("/");
            if (strResult[0].Length == 1)
                str.Append("0" + strResult[0]);
            else
                str.Append(strResult[0]);
            str.Append("/");
            str.Append(strResult[2]);
            //output string is in dd/MM/yyyy format
            return str.ToString();
        }
        else
            return null;
    }
    public string FormatDate(string strDate, string strinputformat, string stroutputformat)
    {
        char[] chSperator = { '/' };
        string[] strResult = strDate.Split(chSperator);
        String str = string.Empty;

        if (strResult.Length < 3)
        {
            return strDate;
        }

        if (strinputformat == "dd/MM/yyyy")
        {
            if (stroutputformat == "dd/MM/yyyy")
            {
                str = strResult[0] + "/" + strResult[1] + "/" + strResult[2];
            }
            else if (stroutputformat == "MM/dd/yyyy")
            {
                str = strResult[1] + "/" + strResult[0] + "/" + strResult[2];
            }

            else if (stroutputformat == "yyyy/MM/dd")
            {
                str = strResult[2] + "/" + strResult[1] + "/" + strResult[0];
            }

        }


        else if (strinputformat == "MM/dd/yyyy")
        {
            if (stroutputformat == "dd/MM/yyyy")
            {
                str = strResult[1] + "/" + strResult[0] + "/" + strResult[2];
            }
            else if (stroutputformat == "MM/dd/yyyy")
            {
                str = strResult[0] + "/" + strResult[1] + "/" + strResult[2];
            }

            else if (stroutputformat == "yyyy/MM/dd")
            {
                str = strResult[2] + "/" + strResult[0] + "/" + strResult[1];
            }

        }


        else if (strinputformat == "yyyy/MM/dd")
        {
            if (stroutputformat == "dd/MM/yyyy")
            {
                str = strResult[2] + "/" + strResult[1] + "/" + strResult[0];
            }
            else if (stroutputformat == "MM/dd/yyyy")
            {
                str = strResult[1] + "/" + strResult[2] + "/" + strResult[0];
            }

            else if (stroutputformat == "yyyy/MM/dd")
            {
                str = strResult[0] + "/" + strResult[1] + "/" + strResult[2];
            }

        }

        return str;

    }

    public string GetPrescriptionDetailStringV2(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        DataTable dtString = new DataTable();
        dtString = dt;
        double numberToSplit = 0;
        double decimalresult = 0;

        try
        {
            for (int i = 0; i < dtString.Rows.Count; i++)
            {
                numberToSplit = Convert.ToDouble(dtString.Rows[i]["Dose"]);
                decimalresult = (int)numberToSplit - numberToSplit;

                if (Convert.ToBoolean(dtString.Rows[i]["IsInfusion"]))
                {
                    if (dtString.Rows[i]["ReferanceItemName"].ToString().Trim().Equals(string.Empty))
                    {
                        sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + dtString.Rows[i]["DoseTypeName"].ToString() + " - " : " ");
                        //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : "");

                        if (numberToSplit == 0.0)
                        {
                            sb.Append(" ");
                        }
                        else
                        {
                            if (decimalresult == 0)
                            {
                                sb.Append(((int)numberToSplit).ToString() + " ");
                            }
                            else
                            {
                                sb.Append(numberToSplit.ToString("F2") + " ");
                            }
                        }

                        sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                        sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : " ");
                        if (Convert.ToDouble(dtString.Rows[i]["Duration"]) != 0.0)
                        {
                            sb.Append(" for " + dtString.Rows[i]["DurationText"].ToString() + " ");
                        }
                        sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : string.Empty);

                        //sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");

                        sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                        sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);

                        try
                        {
                            // sb.Append(", Start Date : " + dtString.Rows[i]["StartDate"].ToString() + ", End Date : " + dtString.Rows[i]["EndDate"].ToString());
                        }
                        catch { }

                        if (dtString.Rows[i]["Instructions"].ToString().Trim() != string.Empty)
                        {
                            sb.Append(Environment.NewLine + "INSTRUCTIONS : " + Environment.NewLine + dtString.Rows[i]["Instructions"].ToString().Trim().ToUpper());
                        }
                    }
                    else
                    {
                        sb.Append(dtString.Rows[i]["ItemName"].ToString());
                        sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                        sb.Append(" to go over ");
                        if (Convert.ToDouble(dtString.Rows[i]["Duration"]) != 0.0)
                        {
                            sb.Append(dtString.Rows[i]["DurationText"].ToString() + ", ");
                        }

                        //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : "");
                        if (numberToSplit == 0.0)
                        {
                            sb.Append(" ");
                        }
                        else
                        {
                            if (decimalresult == 0)
                            {
                                sb.Append(((int)numberToSplit).ToString() + " ");
                            }
                            else
                            {
                                sb.Append(numberToSplit.ToString("F2") + " ");
                            }
                        }
                        sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                        sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : " ");
                        sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : " ");
                        //sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");
                        sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);

                        try
                        {
                            // sb.Append(", Start Date : " + dtString.Rows[i]["StartDate"].ToString() + ", End Date : " + dtString.Rows[i]["EndDate"].ToString());
                        }
                        catch { }

                        if (dtString.Rows[i]["Instructions"].ToString().Trim() != string.Empty)
                        {
                            sb.Append(Environment.NewLine + "INSTRUCTIONS : " + Environment.NewLine + dtString.Rows[i]["Instructions"].ToString().Trim().ToUpper());
                        }
                    }
                }

                else
                {
                    sb.Append("Take ");
                    //Take txtDose +ddlUnit ,ddlFrequencyId.text , ddlFoodRelation + for  txtDuration + ddlPeriodType
                    if (dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty)
                    {
                        sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + dtString.Rows[i]["DoseTypeName"].ToString() + " " : " ");
                        //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : "");

                        if (numberToSplit == 0.0)
                        {
                            sb.Append(" ");
                        }
                        else
                        {
                            if (decimalresult == 0)
                            {
                                sb.Append(((int)numberToSplit).ToString() + " ");
                            }
                            else
                            {
                                sb.Append(numberToSplit.ToString("F2") + " ");
                            }
                        }

                        sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " , " : string.Empty);

                        sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                    }

                    else
                    {
                        //Take txtDose +ddlUnit ,ddlFrequencyId.text , ddlFoodRelation + for  txtDuration + ddlPeriodType
                        sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + dtString.Rows[i]["DoseTypeName"].ToString().Trim() + " " : " ");
                        //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : "");
                        if (numberToSplit == 0.0)
                        {
                            sb.Append(" ");
                        }
                        else
                        {
                            if (decimalresult == 0)
                            {
                                sb.Append(((int)numberToSplit).ToString() + " ");
                            }
                            else
                            {
                                sb.Append(numberToSplit.ToString("F2") + " ");
                            }
                        }

                        sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " , " : string.Empty);
                        sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " " + dtString.Rows[i]["FrequencyName"].ToString() + " , " : " ");
                        sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? " " + dtString.Rows[i]["FoodRelationship"].ToString() : " ");
                        if (dtString.Rows[i]["Duration"].ToString().Trim() != "0")
                        {
                            sb.Append(" for " + dtString.Rows[i]["DurationText"].ToString() + " ");
                        }

                        //sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");
                        sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                        // sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);

                        try
                        {
                            //  sb.Append(", Start Date : " + dtString.Rows[i]["StartDate"].ToString() + ", End Date : " + dtString.Rows[i]["EndDate"].ToString());
                        }
                        catch { }

                        if (dtString.Rows[i]["Instructions"].ToString().Trim() != string.Empty)
                        {
                            sb.Append(Environment.NewLine + "INSTRUCTIONS : " + Environment.NewLine + dtString.Rows[i]["Instructions"].ToString().Trim().ToUpper());
                        }
                        if (dtString.Rows[i]["RouteName"].ToString().Trim() != string.Empty)
                        {
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                        }
                    }
                }

                if (i < dtString.Rows.Count - 1)
                {
                    sb.Append(" Then ");
                }
            }
        }
        catch
        {
        }
        finally
        {
        }
        return sb.ToString().Trim();
    }


    public DataSet getEMRDoctorDetails(int DoctorId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        DataSet ds = new DataSet();
        try
        {
            hstInput.Add("@intDoctorId", DoctorId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDoctorDetails", hstInput);
        }
        catch
        {
        }
        finally
        {
            objDl = null;
            hstInput = null;
        }
        return ds;
    }

    public string SaveEMRSingleScreenUserTemplates(int HospId, int FacilityId, int SpecialisationId, string xmlTemplateIds, string xmlNonFreeTextTemplateIds, int EncodedBy, int DoctorId, string DPSetting)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();

        HshIn.Add("@inyHospitalLocationId", HospId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intSpecialisationId", SpecialisationId);
        HshIn.Add("@xmlTemplateIds", xmlTemplateIds);
        HshIn.Add("@xmlNonFreeTextTemplateIds", xmlNonFreeTextTemplateIds);
        HshIn.Add("@intEncodedBy", EncodedBy);
        HshIn.Add("@intDoctorId", DoctorId);
        HshIn.Add("@chvDPSetting", DPSetting);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRSingleScreenUserTemplates", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }

}
