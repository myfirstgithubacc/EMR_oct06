using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using WebReference;


/// <summary>
/// Summary description for BindNotes
/// </summary>
public class BindCaseSheet
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
    string strShowCaseSheetInASCOrder = string.Empty;

    public BindCaseSheet(string Constring)
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

    //public StringBuilder DisplayUserNameInNote(string UserName, int hospitalID, string enterDate, DataRow drTemplateListStyle, Page pg)
    //{
    //    string sDisplayEnteredBy = common.myStr(System.Web.HttpContext.Current.Session["DisplayEnteredByInCaseSheet"]);
    //    StringBuilder sb = new StringBuilder();

    //    if (common.myLen(System.Web.HttpContext.Current.Session["ControlOnlyByDisplayEnteredByInCaseSheet"]).Equals(0))
    //    {
    //        System.Web.HttpContext.Current.Session["ControlOnlyByDisplayEnteredByInCaseSheet"] = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
    //                                                    common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "ControlOnlyByDisplayEnteredByInCaseSheet", sConString);
    //    }

    //    if (sDisplayEnteredBy.Equals("Y")
    //        || (sDisplayEnteredBy.Equals("N") && common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I") && !common.myStr(System.Web.HttpContext.Current.Session["ControlOnlyByDisplayEnteredByInCaseSheet"]).ToUpper().Equals("Y")))
    //    {
    //        string sBeginFont = "", sEndFont = "";
    //        StringBuilder sbDisplayName = new StringBuilder();

    //        sb.Append("<br/>");
    //        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
    //        if (System.Web.HttpContext.Current.Session["CurrentTemplateFontName"] != null && !common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]).Equals(string.Empty))
    //        {
    //            //sbDisplayName.Append(sBeginFont + "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered by: " + UserName + " on " + enterDate + "</span></b>" + sEndFont);
    //            sbDisplayName.Append(sBeginFont + "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered by: " + UserName + "</span></b>" + sEndFont);

    //        }
    //        else
    //        {
    //            sbDisplayName.Append(sBeginFont + "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered by: " + UserName + " on " + enterDate + "</span></b>" + sEndFont);
    //        }

    //        #region Footer Signature In Entered by
    //        System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

    //        collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]),
    //                "IsShowSignatureWithEnteredByInCasesheet", sConString);

    //        if (collHospitalSetupValues.ContainsKey("IsShowSignatureWithEnteredByInCasesheet"))
    //        {
    //            if (common.myStr(collHospitalSetupValues["IsShowSignatureWithEnteredByInCasesheet"]).Equals("Y"))
    //            {
    //                DataTable dt = new DataTable();
    //                clsIVF objivf = new clsIVF(sConString);
    //                clsExceptionLog objException = new clsExceptionLog();

    //                try
    //                {
    //                    dt = objivf.getDoctorSignatureDetails(common.myInt(System.Web.HttpContext.Current.Session["DoctorId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"])).Tables[0];
    //                    if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
    //                    {
    //                        sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</span></b>" + sEndFont);
    //                    }
    //                    if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
    //                    {
    //                        sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</span></b>" + sEndFont);
    //                    }
    //                    if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
    //                    {
    //                        sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</span></b>" + sEndFont);
    //                    }
    //                    if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
    //                    {
    //                        sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</span></b>" + sEndFont);
    //                    }
    //                }
    //                catch (Exception Ex)
    //                {
    //                    objException.HandleException(Ex);
    //                }
    //                finally
    //                {
    //                    dt.Dispose();
    //                    objivf = null;
    //                    objException = null;
    //                }
    //            }
    //        }
    //        #endregion
    //        sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + " on " + enterDate + "</span></b>" + sEndFont);
    //        sb.Append(sbDisplayName + "<br/><br/>");
    //        sbDisplayName = null;

    //    }
    //    return sb;
    //}

    public StringBuilder DisplayUserNameInNote(string UserName, int hospitalID, string enterDate, DataRow drTemplateListStyle, Page pg, int EnteredById)
    {
        string sDisplayEnteredBy = common.myStr(System.Web.HttpContext.Current.Session["DisplayEnteredByInCaseSheet"]);
        StringBuilder sb = new StringBuilder();

        if (common.myLen(System.Web.HttpContext.Current.Session["ControlOnlyByDisplayEnteredByInCaseSheet"]).Equals(0))
        {
            System.Web.HttpContext.Current.Session["ControlOnlyByDisplayEnteredByInCaseSheet"] = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                                        common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "ControlOnlyByDisplayEnteredByInCaseSheet", sConString);
        }

        if (sDisplayEnteredBy.Equals("Y")
            || (sDisplayEnteredBy.Equals("N") && common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I") && !common.myStr(System.Web.HttpContext.Current.Session["ControlOnlyByDisplayEnteredByInCaseSheet"]).ToUpper().Equals("Y")))
        {
            string sBeginFont = "", sEndFont = "";
            StringBuilder sbDisplayName = new StringBuilder();

            sb.Append("<br/>");
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
            if (System.Web.HttpContext.Current.Session["CurrentTemplateFontName"] != null && !common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]).Equals(string.Empty))
            {
                //sbDisplayName.Append(sBeginFont + "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered by: " + UserName + " on " + enterDate + "</span></b>" + sEndFont);
                sbDisplayName.Append(sBeginFont + "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered by: " + UserName + "</span></b>" + sEndFont);

            }
            else
            {
                sbDisplayName.Append(sBeginFont + "<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered by: " + UserName + " on " + enterDate + "</span></b>" + sEndFont);
            }

            #region Footer Signature In Entered by
            System.Collections.Generic.Dictionary<string, string> collHospitalSetupValues = new System.Collections.Generic.Dictionary<string, string>();

            collHospitalSetupValues = common.GetFlagValueHospitalSetupMultiple(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]),
                    "IsShowSignatureWithEnteredByInCasesheet", sConString);

            if (collHospitalSetupValues.ContainsKey("IsShowSignatureWithEnteredByInCasesheet"))
            {
                if (common.myStr(collHospitalSetupValues["IsShowSignatureWithEnteredByInCasesheet"]).Equals("Y"))
                {
                    DataTable dt = new DataTable();
                    clsIVF objivf = new clsIVF(sConString);
                    clsExceptionLog objException = new clsExceptionLog();

                    try
                    {
                        dt = objivf.getDoctorSignatureDetails(EnteredById, common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationID"])).Tables[0];
                        if (common.myStr(dt.Rows[0]["SignatureLine1"]).Trim().Length > 0)
                        {
                            sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + common.myStr(dt.Rows[0]["SignatureLine1"]).Trim() + "</span></b>" + sEndFont);
                        }
                        if (common.myStr(dt.Rows[0]["SignatureLine2"]).Trim().Length > 0)
                        {
                            sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine2"]).Trim() + "</span></b>" + sEndFont);
                        }
                        if (common.myStr(dt.Rows[0]["SignatureLine3"]).Trim().Length > 0)
                        {
                            sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine3"]).Trim() + "</span></b>" + sEndFont);
                        }
                        if (common.myStr(dt.Rows[0]["SignatureLine4"]).Trim().Length > 0)
                        {
                            sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> ," + common.myStr(dt.Rows[0]["SignatureLine4"]).Trim() + "</span></b>" + sEndFont);
                        }
                    }
                    catch (Exception Ex)
                    {
                        objException.HandleException(Ex);
                    }
                    finally
                    {
                        dt.Dispose();
                        objivf = null;
                        objException = null;
                    }
                }
            }
            #endregion
            sbDisplayName.Append(sBeginFont + " <b><span style='font-family: " + common.myStr(System.Web.HttpContext.Current.Session["CurrentTemplateFontName"]) + "; font-size:8pt;'> " + " on " + enterDate + "</span></b>" + sEndFont);
            sb.Append(sbDisplayName + "<br/><br/>");
            sbDisplayName = null;

        }
        return sb;
    }
    public DataSet GetTemplateStyle(int iHospitalLocationId)
    {
        Hashtable hst = new Hashtable();
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;

        try
        {
            hst.Add("@inyHospitalLocationID", iHospitalLocationId);
            string sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
            + " TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
            + " SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
            + " FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber,StaticTemplateName "
            + " FROM EMRTemplateStatic WITH (NOLOCK)  where (HospitalLocationId = @inyHospitalLocationID or HospitalLocationId is null)";

            ds = objtlc.GetDataSet(CommandType.Text, sql, hst);
        }
        catch (Exception)
        {
        }
        finally
        {
            hst = null;
            objtlc = null;
        }
        return ds;
    }

    public StringBuilder GetEncounterFollowUpAppointment(DataTable dtFollowAppt, StringBuilder sb,
        StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string GroupingDate, int EncounterId)
    {
        string date = string.Empty;
        int i = 0;
        ds = new DataSet();

        DataView dvFollowAppt = new DataView(dtFollowAppt);
        if (GroupingDate != "")
        {
            dvFollowAppt.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvFollowAppt.RowFilter = "EncounterId=" + EncounterId;
        }
        ds.Tables.Add(dvFollowAppt.ToTable());
        dtFollowAppt.Dispose();
        dvFollowAppt.Dispose();

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";

        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;

            if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
            {
                if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == string.Empty)
                {
                    sb.Append(BeginTemplateStyle + "Follow-up Appointment" + EndTemplateStyle);
                    //sb.Append("<br/>");
                    sb.Append("<br/>");
                }

                else
                {
                    sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                    //sb.Append("<br/>");
                    sb.Append("<br/>");

                }
            }
            sBeginFont = "";
            sEndFont = "";
            sBegin = "";
            sEnd = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
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
            //sb.Append("<br/>");
            sb.Append(BeginList);

            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                sb.Append(sBegin + "Next Visit Date : " + ds.Tables[0].Rows[i]["AppointmentDate"].ToString() + " " + ds.Tables[0].Rows[i]["FromTime"].ToString());
                //sb.Append(", Visit Type : " + ds.Tables[0].Rows[i]["VisitType"].ToString());
                //sb.Append(", Doctor Name : " + ds.Tables[0].Rows[i]["DoctorName"].ToString());
                //sb.Append(ds.Tables[0].Rows[i]["Remarks"].ToString().Trim() != "" ? ", Remarks : " + ds.Tables[0].Rows[i]["Remarks"].ToString() + ". " : ".");
                sb.Append(sEnd + "<br/>");
            }
            //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    sb.Append(sBegin + "Appointment Date : " + ds.Tables[0].Rows[i]["AppointmentDate"].ToString() + " " + ds.Tables[0].Rows[i]["FromTime"].ToString());
            //    sb.Append(", Visit Type : " + ds.Tables[0].Rows[i]["VisitType"].ToString());
            //    sb.Append(", Doctor Name : " + ds.Tables[0].Rows[i]["DoctorName"].ToString());
            //    sb.Append(ds.Tables[0].Rows[i]["Remarks"].ToString().Trim() != "" ? ", Remarks : " + ds.Tables[0].Rows[i]["Remarks"].ToString() + ". " : ".");
            //    sb.Append(sEnd + "<br/>");
            //}
        }
        sb.Append(EndList);
        ds.Dispose();
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

    //public StringBuilder BindVitals(DataTable dtVital, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
    //                                Page pg, string pageID, int TemplateFieldId, string GroupingDate, int EncounterId)
    //{
    //    ds = new DataSet();
    //    Hashtable hsTb = new Hashtable();
    //    DataSet dsVitalData = new DataSet();
    //    int totColumns1 = 0;

    //    //if (ContainColumn("GRBS", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(GRBS)", "GRBS <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("GRBS");
    //    //    }
    //    //}
    //    //if (ContainColumn("HT", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(HT)", "HT <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("HT");
    //    //    }
    //    //}
    //    //if (ContainColumn("WT", dtVital))
    //    //{

    //    //// if (dtVital.AsEnumerable().All(dr => dr.IsNull("WT")))
    //    //if (dtVital.Compute("COUNT(WT)", "WT <> ' '") == "0")
    //    //{
    //    //        dtVital.Columns.Remove("WT");
    //    //    }
    //    //}
    //    //if (ContainColumn("T", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(T)", "T <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("T");
    //    //    }
    //    //}
    //    //if (ContainColumn("R", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(R)", "R <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("R");
    //    //    }
    //    //}
    //    //if (ContainColumn("P", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(P)", "P <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("P");
    //    //    }
    //    //}
    //    //if (ContainColumn("BPS", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(BPS)", "BPS <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("BPS");
    //    //    }
    //    //}
    //    //if (ContainColumn("BPD", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(BPD)", "BPD <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("BPD");
    //    //    }
    //    //}
    //    //if (ContainColumn("MAC", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(MAC)", "MAC <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("MAC");
    //    //    }
    //    //}
    //    //if (ContainColumn("BMI", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(BMI)", "BMI <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("BMI");
    //    //    }
    //    //}
    //    //if (ContainColumn("BSA", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(BSA)", "BSA <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("BSA");
    //    //    }
    //    //}
    //    //if (ContainColumn("Pain", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(Pain)", "Pain <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("Pain");
    //    //    }
    //    //}
    //    //if (ContainColumn("SpO2", dtVital))
    //    //{
    //    //    if (common.myInt(dtVital.Compute("COUNT(SpO2)", "SpO2 <> NULL")) == 0)
    //    //    {
    //    //        dtVital.Columns.Remove("SpO2");
    //    //    }
    //    //}
    //    //if (ContainColumn("HC", dtVital))
    //    //{
    //    //    if (dtVital.Compute("COUNT(HC)", "HC <> ' '") == "0")
    //    //    {
    //    //        dtVital.Columns.Remove("HC");
    //    //    }
    //    //}

    //    //for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
    //    //{
    //    //    bool removeColumn = true;
    //    //    foreach (DataRow row in dtVital.Rows)
    //    //    {
    //    //        if (!row.IsNull(col))
    //    //        {
    //    //            removeColumn = false;
    //    //            break;
    //    //        }
    //    //    }
    //    //    if (removeColumn)
    //    //    {
    //    //        dtVital.Columns.RemoveAt(col);
    //    //    }

    //    //}

    //    string[] dtVitalRemove = new string[50];

    //    foreach (DataRow row in dtVital.Rows)
    //    {

    //        for (int col1 = dtVital.Columns.Count - 1; col1 >= 0; col1--)
    //        {
    //            dtVitalRemove[col1] = "0";
    //        }

    //    }


    //    foreach (DataRow row in dtVital.Rows)
    //    {
    //        for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
    //        {


    //            if (!row[col].Equals(string.Empty))
    //            {

    //                dtVitalRemove[col] = "1";
    //            }
    //        }
    //        // No need to continue if we removed all the columns
    //        if (dtVital.Columns.Count == 0)
    //            break;
    //    }



    //    for (int col1 = dtVital.Columns.Count - 1; col1 >= 0; col1--)
    //    {

    //        if (dtVitalRemove[col1] == "0")
    //        {
    //            dtVital.Columns.RemoveAt(col1);
    //        }
    //    }








    //    //foreach (DataRow row in dtVital.Rows)
    //    //{
    //    //    for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
    //    //    {
    //    //        // if (row.IsNull(col))
    //    //        if (DBNull.Value.Equals(row[col]) || row[col].Equals(string.Empty))
    //    //        {
    //    //            dtVital.Columns.RemoveAt(col);
    //    //        }
    //    //    }
    //    //    // No need to continue if we removed all the columns
    //    //    if (dtVital.Columns.Count == 0)
    //    //        break;
    //    //}



    //    DataView dvVital = new DataView(dtVital);
    //    if (GroupingDate != "")
    //    {
    //        dvVital.RowFilter = "GroupDate='" + common.myDate(GroupingDate).ToString("dd/MM/yyyy") + "' AND EncounterId=" + EncounterId;
    //    }
    //    else
    //    {
    //        dvVital.RowFilter = "EncounterId=" + EncounterId;
    //    }
    //    ds.Tables.Add(dvVital.ToTable());
    //    dtVital.Dispose();
    //    dvVital.Dispose();
    //    string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
    //    //  MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
    //    //MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));


    //    //sBeginSection = sBegin;
    //    //sEndSection = sEnd;
    //    int t = 0;

    //    StringBuilder vital = new StringBuilder();

    //    if (ds.Tables[0].Rows.Count > 0)
    //    {

    //        sb.Append(sbTemplateStyle);

    //        if (t == 0)
    //        {
    //            if (common.myStr(sb).Contains("Vitals") == false)
    //            {
    //                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) == true)
    //                {
    //                    MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
    //                    //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
    //                    string BeginTemplateStyle = sBeginFont;
    //                    string EndTemplateStyle = sEndFont;
    //                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
    //                    {
    //                        sb.Append(BeginTemplateStyle + "Vitals" + EndTemplateStyle);
    //                        sb.Append("<br/>");



    //                    }

    //                    else
    //                    {
    //                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
    //                        sb.Append("<br/>");


    //                    }
    //                    sBeginFont = "";
    //                    sEndFont = "";
    //                }

    //            }
    //            t = 1;
    //        }

    //        if (drTemplateListStyle != null)
    //        {

    //            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
    //            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

    //            if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(1))
    //            {
    //                BeginList = "<ul>"; EndList = "</ul>";
    //            }
    //            else if (common.myInt(drTemplateListStyle["FieldsListStyle"]).Equals(2))
    //            {
    //                BeginList = "<ol>"; EndList = "</ol>";
    //            }
    //        }
    //        sb.Append(BeginList);


    //        //"Vital Date","T","R","P","HT","WT","BPS","BPD","HC","MAC","BMI","BSA","Pain","T_ABNORMAL_VALUE"

    //       vital.Append("<table border='1'  style='border-style:solid; border-width:2px;font-size:12px;' cellspacing='4px' >");

    //        foreach (DataColumn col in ds.Tables[0].Columns)
    //        {
    //                if (!common.myStr(col.Caption).Contains("Date"))
    //                {
    //                    if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
    //                    {
    //                        break;
    //                    }
    //                    totColumns1++;
    //                }

    //        }
    //        int totalColumnsForColSpan = 0;
    //        vital.Append("<tr>");
    //        foreach (DataColumn col in ds.Tables[0].Columns)
    //        {
    //            if (col.Caption.Equals("Pain"))
    //            {
    //                totalColumnsForColSpan = totColumns1 + 1 + 3 + 1; //(Total column(1) + 5 columns for Pain field+1 vital date) 
    //                break;
    //            }
    //            else
    //            {
    //                totalColumnsForColSpan = totColumns1 + 1 + 1;

    //            }

    //        }
    //        //totalColumnsForColSpan = totColumns1 + 1+3 + 1; //(Total column(1) + 5 columns for Pain field+1 vital date) 
    //        for(int i=0;i< totalColumnsForColSpan;i++)
    //        {
    //            vital.Append("<td border='0' cellpadding='0' cellspacing='0'></td>");
    //        }
    //        vital.Append("</tr>");
    //        //vital.Append("<tr><td></td> <td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>     </tr>");

    //        vital.Append("<tr style='border-style:solid; border-width:2px;font-size:12px;'>");

    //        vital.Append("<td colspan='2' style='border-style:solid; border-width:2px;font-weight:bold;' valign='top'>" + sBegin + "Vital Date" + sEnd + "</td>");

    //        int totColumns = 0;
    //        foreach (DataColumn col in ds.Tables[0].Columns)
    //        {
    //            if (!common.myStr(col.Caption).Contains("Date"))
    //            {
    //                if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
    //                {
    //                    break;
    //                }
    //                if(col.Caption.Equals("Pain"))
    //                {
    //                    vital.Append("<td colspan='4' style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
    //                }
    //                else
    //                { 
    //                vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
    //                }
    //                totColumns++;
    //            }
    //        }


    //        vital.Append("</tr>");

    //        string vtVitalDate = string.Empty;
    //        string vtValue = string.Empty;

    //        string sFontStart = "<font color=red><b>";
    //        string sFontEnd = "</b></font>";

    //        string colName = "";
    //        DateTime vitalDate = common.myDate(ds.Tables[0].Rows[0]["Vital Date"]);
    //        bool isFirstRow = false;

    //        foreach (DataRow vDR in ds.Tables[0].Rows)
    //        {
    //            isFirstRow = false;
    //            if (vitalDate == common.myDate(vDR["Vital Date"]))
    //            {
    //                if (vitalDate.ToString("dd/MM/yyyy") == common.myDate(vDR["Vital Date"]).ToString("dd/MM/yyyy"))
    //                {
    //                    isFirstRow = true;
    //                }
    //                else
    //                {
    //                    isFirstRow = false;
    //                }
    //            }
    //            else
    //            {
    //                if (vitalDate.ToString("dd/MM/yyyy") == common.myDate(vDR["Vital Date"]).ToString("dd/MM/yyyy"))
    //                {
    //                    isFirstRow = false;
    //                }
    //                else
    //                {
    //                    isFirstRow = true;
    //                }
    //            }

    //            vitalDate = common.myDate(vDR["Vital Date"]);

    //            if (isFirstRow)
    //            {
    //                vital.Append("<tr style='border-style:solid; border-width:2px;'>");

    //                //vital.Append("<td colspan='20' style='border-style:solid; border-width:2px; valign='top' colspan='" + (totColumns + 1) + "'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");
    //                vital.Append("<td colspan='" + (totalColumnsForColSpan ) + "' style='border-style:solid; border-width:2px; valign='top'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");

    //                vital.Append("</tr>");

    //                isFirstRow = false;
    //            }

    //            vital.Append("<tr style='border-style:solid; border-width:2px;'>");
    //            vital.Append("<td colspan='2' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + (isFirstRow ? vitalDate.ToString("dd/MM/yyyy") : vitalDate.ToShortTimeString()) + sEnd + "</td>");

    //            for (int cIdx = (0 + 2); cIdx < (totColumns + 2); cIdx++)
    //            {
    //                colName = ds.Tables[0].Columns[cIdx].Caption;
    //                vtValue = (common.myStr(vDR[colName]).Trim().Length == 0) ? "&nbsp;" : common.myStr(vDR[colName]);

    //                if (common.myInt(vDR[colName]) == 1)
    //                {
    //                    vtValue = sFontStart + vtValue + sFontEnd;
    //                }
    //                if (colName.Equals("Pain"))
    //                {
    //                   // vital.Append("<td colspan='10' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
    //                    vital.Append("<td colspan='4' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");

    //                }
    //                else
    //                {
    //                    //vital.Append("<td  colspan='1' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
    //                    vital.Append("<td colspan='1'  style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");

    //                }
    //            }
    //            vital.Append("</tr>");

    //        }

    //        vital.Append("</table>");
    //    }

    //    sb.Append(vital);

    //    ds.Dispose();
    //    hsTb = null;
    //    dsVitalData.Dispose();
    //    vital = null;
    //    totColumns1 = 0;
    //    return sb;

    //}

    public StringBuilder BindVitals(DataTable dtVital, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                               Page pg, string pageID, int TemplateFieldId, string GroupingDate, int EncounterId, bool StaticTemplateDisplayTitle)
    {
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        DataSet dsVitalData = new DataSet();
        int totColumns1 = 0;

        //if (ContainColumn("GRBS", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(GRBS)", "GRBS <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("GRBS");
        //    }
        //}
        //if (ContainColumn("HT", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(HT)", "HT <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("HT");
        //    }
        //}
        //if (ContainColumn("WT", dtVital))
        //{

        //// if (dtVital.AsEnumerable().All(dr => dr.IsNull("WT")))
        //if (dtVital.Compute("COUNT(WT)", "WT <> ' '") == "0")
        //{
        //        dtVital.Columns.Remove("WT");
        //    }
        //}
        //if (ContainColumn("T", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(T)", "T <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("T");
        //    }
        //}
        //if (ContainColumn("R", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(R)", "R <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("R");
        //    }
        //}
        //if (ContainColumn("P", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(P)", "P <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("P");
        //    }
        //}
        //if (ContainColumn("BPS", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BPS)", "BPS <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BPS");
        //    }
        //}
        //if (ContainColumn("BPD", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BPD)", "BPD <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BPD");
        //    }
        //}
        //if (ContainColumn("MAC", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(MAC)", "MAC <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("MAC");
        //    }
        //}
        //if (ContainColumn("BMI", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BMI)", "BMI <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BMI");
        //    }
        //}
        //if (ContainColumn("BSA", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BSA)", "BSA <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BSA");
        //    }
        //}
        //if (ContainColumn("Pain", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(Pain)", "Pain <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("Pain");
        //    }
        //}
        //if (ContainColumn("SpO2", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(SpO2)", "SpO2 <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("SpO2");
        //    }
        //}
        //if (ContainColumn("HC", dtVital))
        //{
        //    if (dtVital.Compute("COUNT(HC)", "HC <> ' '") == "0")
        //    {
        //        dtVital.Columns.Remove("HC");
        //    }
        //}

        //for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
        //{
        //    bool removeColumn = true;
        //    foreach (DataRow row in dtVital.Rows)
        //    {
        //        if (!row.IsNull(col))
        //        {
        //            removeColumn = false;
        //            break;
        //        }
        //    }
        //    if (removeColumn)
        //    {
        //        dtVital.Columns.RemoveAt(col);
        //    }
        //}

        string[] dtVitalRemove = new string[50];

        foreach (DataRow row in dtVital.Rows)
        {
            for (int col1 = dtVital.Columns.Count - 1; col1 >= 0; col1--)
            {
                dtVitalRemove[col1] = "0";
            }
        }

        foreach (DataRow row in dtVital.Rows)
        {
            for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
            {
                if (!row[col].Equals(string.Empty))
                {
                    dtVitalRemove[col] = "1";
                }
            }
            // No need to continue if we removed all the columns
            if (dtVital.Columns.Count == 0)
                break;
        }

        for (int col1 = dtVital.Columns.Count - 1; col1 >= 0; col1--)
        {
            if (dtVitalRemove[col1] == "0")
            {
                dtVital.Columns.RemoveAt(col1);
            }
        }

        //foreach (DataRow row in dtVital.Rows)
        //{
        //    for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
        //    {
        //        // if (row.IsNull(col))
        //        if (DBNull.Value.Equals(row[col]) || row[col].Equals(string.Empty))
        //        {
        //            dtVital.Columns.RemoveAt(col);
        //        }
        //    }
        //    // No need to continue if we removed all the columns
        //    if (dtVital.Columns.Count == 0)
        //        break;
        //}

        DataView dvVital = new DataView(dtVital);
        if (GroupingDate != "")
        {
            dvVital.RowFilter = "GroupDate='" + common.myDate(GroupingDate).ToString("dd/MM/yyyy") + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvVital.RowFilter = "EncounterId=" + EncounterId;
        }
        ds.Tables.Add(dvVital.ToTable());
        dtVital.Dispose();
        dvVital.Dispose();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        //  MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        //MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

        //sBeginSection = sBegin;
        //sEndSection = sEnd;
        int t = 0;

        StringBuilder vital = new StringBuilder();

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);

            if (t == 0)
            {
                if (common.myStr(sb).Contains("Vitals") == false)
                {
                    if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                    {
                        MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                        //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                        string BeginTemplateStyle = sBeginFont;
                        string EndTemplateStyle = sEndFont;
                        if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                        {
                            sb.Append(BeginTemplateStyle + "Vitals" + EndTemplateStyle);
                            sb.Append("<br/>");
                            sb.Append("<br/>");
                        }
                        else
                        {
                            sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                            sb.Append("<br/>");
                            sb.Append("<br/>");
                        }
                        sBeginFont = "";
                        sEndFont = "";
                    }

                }
                t = 1;
            }

            if (drTemplateListStyle != null)
            {

                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
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
            sb.Append(BeginList);


            //"Vital Date","T","R","P","HT","WT","BPS","BPD","HC","MAC","BMI","BSA","Pain","T_ABNORMAL_VALUE"

            vital.Append("<table border='1' style='border-style:solid; border-width:1px;font-size:10pt;border-color:#D3D3D3;' cellpadding='0' cellspacing='0'>");

            bool IsPain = false;

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (!common.myStr(col.Caption).Contains("Date"))
                {
                    if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
                    {
                        break;
                    }

                    if (common.myStr(col.Caption).Equals("Pain"))
                    {
                        IsPain = true;
                    }

                    totColumns1++;
                }
            }

            int totalColumnsForColSpan = 0;
            vital.Append("<tr>");

            if (totColumns1 > 0)
            {
                if (IsPain)
                {
                    totalColumnsForColSpan = totColumns1 + 2 + 1; //2 for vital date and 2 for pain score
                }
                else
                {
                    totalColumnsForColSpan = totColumns1 + 2; //2 for vital date
                }
            }

            for (int i = 0; i < totalColumnsForColSpan; i++)
            {
                vital.Append("<td style='border-style:none;height:0px;'></td>");
            }
            vital.Append("</tr>");
            //vital.Append("<tr><td></td> <td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>     </tr>");

            vital.Append("<tr style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-size:10pt;'>");

            vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top'>" + sBegin + "Date Time" + sEnd + "</td>");

            int totColumns = 0;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (!common.myStr(col.Caption).Contains("Date"))
                {
                    if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
                    {
                        break;
                    }
                    if (col.Caption.Equals("Pain"))
                    {
                        vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
                    }

                    else if (col.Caption.Equals("HT"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + "Height" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("WT"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + "Weight" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("T"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + "Temp." + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("R"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + "RR" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("P"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + "Pulse" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("BPS"))
                    {
                        vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + "BP" + sEnd + "</td>");
                    }
                    else if (!col.Caption.Equals("BPS") && !col.Caption.Equals("BPD"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
                    }
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

                vitalDate = common.myDate(vDR["VitalEntryDate1"]);
                #region Hide Visit Date 
                isFirstRow = false;
                if (isFirstRow)
                {
                    vital.Append("<tr style='border-style:solid; border-width:1px;border-color:#D3D3D3;'>");
                    //vital.Append("<td colspan='20' style='border-style:solid; border-width:1px; valign='top' colspan='" + (totColumns + 1) + "'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");
                    vital.Append("<td colspan='" + (totalColumnsForColSpan) + "' style='border-style:solid; border-width:1px;border-color:#D3D3D3; valign='top'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");

                    vital.Append("</tr>");

                    isFirstRow = false;
                }
                #endregion
                vital.Append("<tr style='border-style:solid; border-width:1px;border-color:#D3D3D3;'>");
                vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;' valign='top'>" + sBegin + (vitalDate.ToString("dd/MM/yyyy") + ((totColumns1 > 6) ? "<br/>" : "&nbsp;&nbsp;") + vitalDate.ToShortTimeString()) + sEnd + "</td>");

                for (int cIdx = (0 + 2); cIdx < (totColumns + 2); cIdx++)
                {
                    colName = ds.Tables[0].Columns[cIdx].Caption;
                    vtValue = (common.myStr(vDR[colName]).Trim().Length == 0) ? "&nbsp;" : common.myStr(vDR[colName]);
                    if (common.myInt(vDR[colName]) == 1)
                    {
                        vtValue = sFontStart + vtValue + sFontEnd;
                    }
                    if (colName.Equals("Pain"))
                    {
                        // vital.Append("<td colspan='10' style='border-style:solid; border-width:1px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                        vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;' valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                    }
                    else if (colName.Equals("BPS"))
                    {
                        string strBP = vtValue;
                        cIdx++;

                        colName = ds.Tables[0].Columns[cIdx].Caption; //for BPD
                        vtValue = (common.myStr(vDR[colName]).Trim().Length == 0) ? "&nbsp;" : common.myStr(vDR[colName]);
                        strBP = strBP + "/" + vtValue;
                        //if (!common.myStr(strBP).Equals("&nbsp;/&nbsp;"))
                        //{
                        //    strBP += "&nbsp;mmHG";
                        //}

                        if (common.myLen(common.myStr(strBP).Replace("&nbsp;", string.Empty)) > 1)
                        {
                            vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;' valign='top' align='center'>" + sBegin + strBP + sEnd + "</td>");
                        }
                        else
                        {
                            vital.Append("<td colspan='2' style='border-style:solid; border-width:1px;border-color:#D3D3D3;' valign='top' align='center'>" + sBegin + "&nbsp;" + sEnd + "</td>");
                        }
                    }
                    else
                    {
                        //vital.Append("<td style='border-style:solid; border-width:1px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                        vital.Append("<td style='border-style:solid; border-width:1px;border-color:#D3D3D3;' valign='top' align='center'>" + sBegin + vtValue + sEnd + "</td>");
                    }
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
        totColumns1 = 0;
        return sb;
    }

    public StringBuilder BindVitalsVenkateshwar(DataTable dtVital, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                              Page pg, string pageID, int TemplateFieldId, string GroupingDate, int EncounterId)
    {
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        DataSet dsVitalData = new DataSet();
        int totColumns1 = 0;

        //if (ContainColumn("GRBS", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(GRBS)", "GRBS <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("GRBS");
        //    }
        //}
        //if (ContainColumn("HT", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(HT)", "HT <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("HT");
        //    }
        //}
        //if (ContainColumn("WT", dtVital))
        //{

        //// if (dtVital.AsEnumerable().All(dr => dr.IsNull("WT")))
        //if (dtVital.Compute("COUNT(WT)", "WT <> ' '") == "0")
        //{
        //        dtVital.Columns.Remove("WT");
        //    }
        //}
        //if (ContainColumn("T", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(T)", "T <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("T");
        //    }
        //}
        //if (ContainColumn("R", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(R)", "R <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("R");
        //    }
        //}
        //if (ContainColumn("P", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(P)", "P <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("P");
        //    }
        //}
        //if (ContainColumn("BPS", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BPS)", "BPS <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BPS");
        //    }
        //}
        //if (ContainColumn("BPD", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BPD)", "BPD <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BPD");
        //    }
        //}
        //if (ContainColumn("MAC", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(MAC)", "MAC <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("MAC");
        //    }
        //}
        //if (ContainColumn("BMI", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BMI)", "BMI <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BMI");
        //    }
        //}
        //if (ContainColumn("BSA", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(BSA)", "BSA <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("BSA");
        //    }
        //}
        //if (ContainColumn("Pain", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(Pain)", "Pain <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("Pain");
        //    }
        //}
        //if (ContainColumn("SpO2", dtVital))
        //{
        //    if (common.myInt(dtVital.Compute("COUNT(SpO2)", "SpO2 <> NULL")) == 0)
        //    {
        //        dtVital.Columns.Remove("SpO2");
        //    }
        //}
        //if (ContainColumn("HC", dtVital))
        //{
        //    if (dtVital.Compute("COUNT(HC)", "HC <> ' '") == "0")
        //    {
        //        dtVital.Columns.Remove("HC");
        //    }
        //}

        //for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
        //{
        //    bool removeColumn = true;
        //    foreach (DataRow row in dtVital.Rows)
        //    {
        //        if (!row.IsNull(col))
        //        {
        //            removeColumn = false;
        //            break;
        //        }
        //    }
        //    if (removeColumn)
        //    {
        //        dtVital.Columns.RemoveAt(col);
        //    }
        //}

        string[] dtVitalRemove = new string[50];

        foreach (DataRow row in dtVital.Rows)
        {
            for (int col1 = dtVital.Columns.Count - 1; col1 >= 0; col1--)
            {
                dtVitalRemove[col1] = "0";
            }
        }

        foreach (DataRow row in dtVital.Rows)
        {
            for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
            {
                if (!row[col].Equals(string.Empty))
                {
                    dtVitalRemove[col] = "1";
                }
            }
            // No need to continue if we removed all the columns
            if (dtVital.Columns.Count == 0)
                break;
        }

        for (int col1 = dtVital.Columns.Count - 1; col1 >= 0; col1--)
        {
            if (dtVitalRemove[col1] == "0")
            {
                dtVital.Columns.RemoveAt(col1);
            }
        }

        //foreach (DataRow row in dtVital.Rows)
        //{
        //    for (int col = dtVital.Columns.Count - 1; col >= 0; col--)
        //    {
        //        // if (row.IsNull(col))
        //        if (DBNull.Value.Equals(row[col]) || row[col].Equals(string.Empty))
        //        {
        //            dtVital.Columns.RemoveAt(col);
        //        }
        //    }
        //    // No need to continue if we removed all the columns
        //    if (dtVital.Columns.Count == 0)
        //        break;
        //}

        DataView dvVital = new DataView(dtVital);
        if (GroupingDate != "")
        {
            dvVital.RowFilter = "GroupDate='" + common.myDate(GroupingDate).ToString("dd/MM/yyyy") + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvVital.RowFilter = "EncounterId=" + EncounterId;
        }
        ds.Tables.Add(dvVital.ToTable());
        dtVital.Dispose();
        dvVital.Dispose();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        //  MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        //MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

        //sBeginSection = sBegin;
        //sEndSection = sEnd;
        int t = 0;

        StringBuilder vital = new StringBuilder();

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);

            if (t == 0)
            {
                if (common.myStr(sb).Contains("Vitals") == false)
                {
                    if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]))
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

            if (drTemplateListStyle != null)
            {

                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
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
            sb.Append(BeginList);


            //"Vital Date","T","R","P","HT","WT","BPS","BPD","HC","MAC","BMI","BSA","Pain","T_ABNORMAL_VALUE"

            vital.Append("<table border='1' style='border-style:solid; border-width:2px;font-size:12px;' cellpadding='1' cellspacing='0'>");

            bool IsBP = false;

            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (!common.myStr(col.Caption).Contains("Date"))
                {
                    if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
                    {
                        break;
                    }
                    totColumns1++;

                    if (common.myStr(col.Caption).Contains("BPS") || common.myStr(col.Caption).Contains("BPD"))
                    {
                        IsBP = true;
                    }
                }
            }
            if (IsBP)
            {
                totColumns1--; //BSD
            }


            int totalColumnsForColSpan = 0;
            vital.Append("<tr>");
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (col.Caption.Equals("Pain"))
                {
                    totalColumnsForColSpan = totColumns1 + 1 + 3 + 1; //(Total column(1) + 5 columns for Pain field+1 vital date) 
                    break;
                }
                else
                {
                    totalColumnsForColSpan = totColumns1 + 1 + 1;
                }
            }
            //totalColumnsForColSpan = totColumns1 + 1+3 + 1; //(Total column(1) + 5 columns for Pain field+1 vital date) 
            for (int i = 0; i < totalColumnsForColSpan; i++)
            {
                vital.Append("<td border='0' style='border-style:none;height:0px;'></td>");
            }
            vital.Append("</tr>");
            //vital.Append("<tr><td></td> <td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>     </tr>");

            vital.Append("<tr style='border-style:solid; border-width:2px;font-size:12px;'>");

            vital.Append("<td colspan='2' style='border-style:solid; border-width:2px;font-weight:bold;' valign='top'>" + sBegin + "Vital Date" + sEnd + "</td>");

            int totColumns = 0;
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                if (!common.myStr(col.Caption).Contains("Date"))
                {
                    if (common.myStr(col.Caption).Contains("_ABNORMAL_VALUE"))
                    {
                        break;
                    }
                    if (col.Caption.Equals("Pain"))
                    {
                        vital.Append("<td colspan='4' style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
                    }

                    else if (col.Caption.Equals("HT"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + "Height" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("WT"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + "Weight" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("T"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + "Temp." + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("R"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + "RR" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("P"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + "Pulse" + sEnd + "</td>");
                    }
                    else if (col.Caption.Equals("BPS"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + "BP" + sEnd + "</td>");
                    }
                    else if (!col.Caption.Equals("BPS") && !col.Caption.Equals("BPD"))
                    {
                        vital.Append("<td style='border-style:solid; border-width:2px;font-weight:bold;' valign='top' align='center'>" + sBegin + common.myStr(col.Caption) + sEnd + "</td>");
                    }
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
                    vital.Append("<tr style='border-style:solid; border-width:2px;'>");

                    //vital.Append("<td colspan='20' style='border-style:solid; border-width:2px; valign='top' colspan='" + (totColumns + 1) + "'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");
                    vital.Append("<td colspan='" + (totalColumnsForColSpan) + "' style='border-style:solid; border-width:2px; valign='top'><b>" + sBegin + vitalDate.ToString("dd/MM/yyyy") + sEnd + "</b></td>");

                    vital.Append("</tr>");

                    isFirstRow = false;
                }

                vital.Append("<tr style='border-style:solid; border-width:2px;'>");
                vital.Append("<td colspan='2' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + (isFirstRow ? vitalDate.ToString("dd/MM/yyyy") : vitalDate.ToShortTimeString()) + sEnd + "</td>");

                for (int cIdx = (0 + 2); cIdx < (totColumns + 2); cIdx++)
                {
                    colName = ds.Tables[0].Columns[cIdx].Caption;
                    vtValue = (common.myStr(vDR[colName]).Trim().Length == 0) ? "&nbsp;" : common.myStr(vDR[colName]);
                    if (common.myInt(vDR[colName]) == 1)
                    {
                        vtValue = sFontStart + vtValue + sFontEnd;
                    }
                    if (colName.Equals("Pain"))
                    {
                        // vital.Append("<td colspan='10' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                        vital.Append("<td colspan='4' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                    }
                    else if (colName.Equals("BPS"))
                    {
                        string strBP = vtValue;
                        cIdx++;

                        colName = ds.Tables[0].Columns[cIdx].Caption; //for BPD
                        vtValue = (common.myStr(vDR[colName]).Trim().Length == 0) ? "&nbsp;" : common.myStr(vDR[colName]);
                        strBP = strBP + "/" + vtValue;

                        if (common.myLen(common.myStr(strBP).Replace("&nbsp;", string.Empty)) > 1)
                        {
                            vital.Append("<td colspan='1' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + strBP + sEnd + "</td>");
                        }
                        else
                        {
                            vital.Append("<td colspan='1' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + "&nbsp;" + sEnd + "</td>");
                        }
                    }
                    else
                    {
                        //vital.Append("<td  colspan='1' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                        vital.Append("<td colspan='1' style='border-style:solid; border-width:2px; valign='top'>" + sBegin + vtValue + sEnd + "</td>");
                    }
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
        totColumns1 = 0;
        return sb;

    }
    private bool ContainColumn(string columnName, DataTable table)
    {
        DataColumnCollection columns = table.Columns;
        if (columns.Contains(columnName))
        {
            return true;
        }
        else
        {
            return false;
        }
        return false;
    }


    public StringBuilder BindDoctorProgressNote(DataTable dtDocProgressNote, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                    Page pg, string pageID, string userID, string GroupingDate, int EncounterId, bool StaticTemplateDisplayTitle)
    {
        ds = new DataSet();
        Hashtable hsTb = new Hashtable();
        DataTable dtEnterByItem = new DataTable();
        DataView dvFilter = new DataView();
        DataView dvDoctorProgress = new DataView();
        try
        {
            DataView dvDocProgressNote = new DataView(dtDocProgressNote);

            strShowCaseSheetInASCOrder = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                                    common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "ShowCaseSheetInASCOrder", sConString);

            if (GroupingDate != "")
            {
                dvDocProgressNote.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
            }
            else
            {
                dvDocProgressNote.RowFilter = "EncounterId=" + EncounterId;
            }
            if (!strShowCaseSheetInASCOrder.Equals("Y"))
            {
                dvDocProgressNote.Sort = "EncodedDate1 ASC";
            }
            else
            {
                dvDocProgressNote.Sort = "EncodedDate1 DESC";
            }


            ds.Tables.Add(dvDocProgressNote.ToTable());
            dtDocProgressNote.Dispose();
            dvDocProgressNote.Dispose();

            string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

            sBeginSection = sBegin;
            sEndSection = sEnd;
            if (ds.Tables[0].Rows.Count > 0)
            {
                MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                string BeginTemplateStyle = sBeginFont;
                string EndTemplateStyle = sEndFont;

                if (!common.myStr(sbTemplateStyle).Contains("Doctor Progress Note") && StaticTemplateDisplayTitle)
                {
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Doctor Progress Note" + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");




                    }
                }
                sBeginFont = "";
                sEndFont = "";
                sb.Append(sbTemplateStyle);
                if (drTemplateListStyle != null)
                {
                    MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
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
                sb.Append(BeginList);
                dvFilter = new DataView(ds.Tables[0]);
                dtEnterByItem = dvFilter.ToTable(true, "EncodedBy", "EncodedDate");
                foreach (DataRow row in dtEnterByItem.Rows)
                {
                    //Ritika(26-09-2022)Added Addendum Against Progress Note
                    DataTable dtfiltered = ds.Tables[0].AsEnumerable()
                    .GroupBy(r => new { ProgressNoteID = r.Field<int>("ProgressNoteID") })
                    .Select(g => g.First())
                    .CopyToDataTable();
                    dvDoctorProgress = new DataView(dtfiltered);
                    //dvDoctorProgress = new DataView(dvFilter.ToTable());
                    //------------------End
                    dvDoctorProgress.RowFilter = "EncodedBy=" + row["EncodedBy"].ToString() + " AND EncodedDate='" + row["EncodedDate"].ToString() + "'";
                    for (int i = 0; i < dvDoctorProgress.ToTable().Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["ProgressNote"].ToString() != "")
                        {
                            sb.Append(i == 0 ? sBegin + dvDoctorProgress.ToTable().Rows[i]["ProgressNote"].ToString() + sEnd : "<br/>" + sBegin + dvDoctorProgress.ToTable().Rows[i]["ProgressNote"].ToString() + sEnd);
                        }
                        sb.Append("<br/>" + DisplayUserNameInNote(dvDoctorProgress.ToTable().Rows[0]["EnteredBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), dvDoctorProgress.ToTable().Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(dvDoctorProgress.ToTable().Rows[0]["EnteredById"])));
                        //Ritika(26-09-2022)Added Addendum Against Progress Note
                        DataTable dvAddendumEncode = new DataTable();
                        DataView dvAddendumData = new DataView();
                        dvAddendumEncode = dvFilter.ToTable(true, "AddendumEncodedBy", "AddendumEncodedDate");
                        //ds.Tables[0].Select(r => r.ProgressNoteID == "+ dvDoctorProgress.ToTable().Rows[i]["ProgressNoteID"].ToString() );
                        dvAddendumData = new DataView(ds.Tables[0].Select("ProgressNoteID=" + dvDoctorProgress.ToTable().Rows[i]["ProgressNoteID"].ToString() + "").CopyToDataTable());
                        DataView dvEncodeSort = new DataView(dvAddendumEncode);
                        dvEncodeSort.Sort = "AddendumEncodedDate desc";
                        foreach (DataRow rw in dvEncodeSort.ToTable().Rows)
                        {
                            dvAddendumData.RowFilter = "AddendumEncodedBy='" + rw["AddendumEncodedBy"].ToString() + "' AND AddendumEncodedDate='" + rw["AddendumEncodedDate"].ToString() + "'";

                            for (int j = 0; j < dvAddendumData.ToTable().Rows.Count; j++)
                            {
                                if (dvAddendumData.ToTable().Rows[j]["Addendum"].ToString() != "")
                                {
                                    sb.Append(i == 0 ? sBegin + "Addendum : " + dvAddendumData.ToTable().Rows[i]["Addendum"].ToString() + sEnd : "<br/>" + sBegin + "Addendum : " + dvAddendumData.ToTable().Rows[i]["Addendum"].ToString() + sEnd);
                                }
                                sb.Append("<br/>" + DisplayUserNameInNote(dvAddendumData.ToTable().Rows[0]["AddendumEncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), dvAddendumData.ToTable().Rows[0]["AddendumEncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(dvAddendumData.ToTable().Rows[0]["AddendumEncodedById"])));
                            }
                        }
                        //----------------End
                    }
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

    public StringBuilder BindImmunization(DataTable dtImmunization, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                string pageID, string GroupingDate, int EncounterId)
    {
        ds = new DataSet();
        string date = string.Empty;
        int i = 0;
        DataView dvImmunization = new DataView(dtImmunization);
        if (GroupingDate != "")
        {
            dvImmunization.RowFilter = " GroupDate = '" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvImmunization.RowFilter = " EncounterId =" + EncounterId;
        }
        ds.Tables.Add(dvImmunization.ToTable());
        dtImmunization.Dispose();
        dvImmunization.Dispose();

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";

        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;
            if (common.myStr(sbTemplateStyle).Contains("Immunization") == false)
            {
                if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                {
                    sb.Append(BeginTemplateStyle + "Immunization" + EndTemplateStyle);
                    //sb.Append("<br/>");
                    //sb.Append(BeginTemplateStyle + "Given Immunization" + EndTemplateStyle);
                    ////sb.Append("<br/>");
                }
                else
                {
                    sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                    //sb.Append("<br/>");
                    //sb.Append(BeginTemplateStyle + "Given Immunization" + EndTemplateStyle);
                    ////sb.Append("<br/>");
                }
            }

            sBeginFont = "";
            sEndFont = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            if (ds.Tables[0].Rows[0]["GivenDate"].ToString() != null)
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
                    sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]).Trim() + (common.myLen(ds.Tables[0].Rows[i]["ItemBrandName"]) > 0 ? " (Brand: " + common.myStr(ds.Tables[0].Rows[i]["ItemBrandName"]).Trim() + ")" : string.Empty) +
                                   (common.myLen(ds.Tables[0].Rows[i]["GivenDate"]) > 0 ? ", Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"]).Trim() : string.Empty) +
                                   (common.myLen(ds.Tables[0].Rows[i]["BatchNo"]) > 0 ? ", Batch No: " + common.myStr(ds.Tables[0].Rows[i]["BatchNo"]).Trim() : string.Empty) +
                                   (common.myLen(ds.Tables[0].Rows[i]["ExpiryDate"]) > 0 ? ", Expiry Date: " + common.myStr(ds.Tables[0].Rows[i]["ExpiryDate"]).Trim() : string.Empty) +
                                   (common.myLen(ds.Tables[0].Rows[i]["Remarks"]) > 0 ? ", Remarks: " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() : string.Empty) +
                              sEnd + "<br/>");
                }
            }
        }
        sb.Append(EndList);
        ds.Dispose();
        return sb;
    }


    public StringBuilder BindImmunizationDueDate(DataTable dtImmunization, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
              string pageID, string GroupingDate, int EncounterId)
    {
        ds = new DataSet();
        string date = string.Empty;
        int i = 0;
        DataView dvImmunization = new DataView(dtImmunization);

        dvImmunization.RowFilter = " EncounterId =" + EncounterId;

        ds.Tables.Add(dvImmunization.ToTable());
        dtImmunization.Dispose();
        dvImmunization.Dispose();

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";

        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;
            //if (common.myStr(sbTemplateStyle).Contains("Immunization") == false)
            //{

            //if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
            //{
            //    sb.Append(BeginTemplateStyle + "Next Immunization Due Date" + EndTemplateStyle);
            //    //sb.Append("<br/>");

            //}
            //else
            //{
            //    sb.Append(BeginTemplateStyle + "Next Immunization Due Date" + EndTemplateStyle);
            //    // sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
            //    //sb.Append("<br/>");
            //}

            //}
            sBeginFont = "";
            sEndFont = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            if (ds.Tables[0].Rows[0]["DueDate"].ToString() != null)
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
                // sb.Append("<br/>");
                sb.Append(BeginList);
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (common.myLen(ds.Tables[0].Rows[i]["DueDate"]) > 0)
                    {
                        sb.Append(sBegin + "<b>Next Immunization</b>: " + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + "<br/>" +
                        "Next Due Date: " + ds.Tables[0].Rows[i]["DueDate"].ToString() + sEnd + "<br/>");
                    }
                }
            }
        }
        sb.Append(EndList);
        ds.Dispose();
        return sb;
    }


    public StringBuilder BindInjection(DataTable dtInjection, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg,
                    string pageID, string GroupingDate, int EncounterId)
    {
        ds = new DataSet();
        string date = string.Empty;
        int i = 0;

        DataView dvInjection = new DataView(dtInjection);
        if (GroupingDate != "")
        {
            dvInjection.RowFilter = " GroupDate = '" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvInjection.RowFilter = " EncounterId = " + EncounterId;
        }
        ds.Tables.Add(dvInjection.ToTable());
        dtInjection.Dispose();
        dvInjection.Dispose();

        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;
            if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
            {
                sb.Append(BeginTemplateStyle + "Injection" + EndTemplateStyle);
                //  sb.Append("<br/>");
            }

            else
            {
                sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                //  sb.Append("<br/>");
            }
            sBeginFont = "";
            sEndFont = "";
            if (ds.Tables[0].Rows[0]["GivenDateTime"].ToString() != null)
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
                    if (ds.Tables[0].Rows[i]["Remarks"].ToString() == "")
                    {
                        sb.Append(sBegin + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + ", "
                       + "Given Date: " + ds.Tables[0].Rows[i]["GivenDateTime"].ToString() + ", "
                      + "Given By: " + ds.Tables[0].Rows[i]["GivenByName"].ToString() + ", "
                     + "Batch No: " + ds.Tables[0].Rows[i]["LotNo"].ToString() + ", "
                     + "Quantity Given: " + ds.Tables[0].Rows[i]["QtyGiven"].ToString() + ". " + sEnd + "<br/>");

                    }
                    else
                    {
                        sb.Append(sBegin + ds.Tables[0].Rows[i]["ImmunizationName"].ToString() + ", "
                       + "Given Date: " + ds.Tables[0].Rows[i]["GivenDateTime"].ToString() + ", "
                      + "Given By: " + ds.Tables[0].Rows[i]["GivenByName"].ToString() + ", "
                     + "Batch No: " + ds.Tables[0].Rows[i]["LotNo"].ToString() + ", "
                     + "Quantity Given: " + ds.Tables[0].Rows[i]["QtyGiven"].ToString() + ", "
                      + "Remarks: " + ds.Tables[0].Rows[i]["Remarks"].ToString() + ". " + sEnd + "<br/>");
                    }
                }
            }
        }
        sb.Append(EndList);
        ds.Dispose();
        return sb;
    }

    public StringBuilder BindProblemsHPI(DataTable dtProblem, int RegId, int EncId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string GroupingDate, bool StaticTemplateDisplayTitle)
    {
        ds = new DataSet();
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "";
        StringBuilder sbTemp = new StringBuilder();
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        DataTable dtChronic = new DataTable();
        DataTable dtNonChronic = new DataTable();
        DataSet dsProblem = new DataSet();
        string strSql = "";
        string strAge = "";
        //ds2 = emr.getEMRPatientProblemAndAllergyTemplateString(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), RegId, EncId);
        if (dtProblem.Rows.Count > 0)
        {
            DataView dvProblem = new DataView(dtProblem);
            if (GroupingDate != "")
            {
                dvProblem.RowFilter = " GroupDate = '" + GroupingDate + "' AND ISNULL(HPIRemarks,'')='' AND EncounterId=" + EncId;
            }
            else
            {
                dvProblem.RowFilter = "ISNULL(HPIRemarks,'')='' AND EncounterId=" + EncId;
            }
            ds.Tables.Add(dvProblem.ToTable());
            dvProblem.Dispose();
            dtProblem.Dispose();

            strSql = String.Empty;
            //if (common.myLen(ds2.Tables[0].Rows[0]["Age"]) > 0)
            //{
            //    strAge = common.myStr(ds2.Tables[0].Rows[0]["Age"]).Trim();
            //}
            //sb.Append(sbTemplateStyle + "<u><b>" + "Chief Complaints:" + "</b></u>" + BeginList);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
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
                        //sb.Append("<br/>");
                    }
                }
                sBeginFont = "";
                sEndFont = "";


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
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

                DataView dvChronic = ds.Tables[0].DefaultView;
                dvChronic.RowFilter = "IsChronic = 'True'";
                dtChronic = dvChronic.ToTable();
                DataView dvNonChronic = ds.Tables[0].DefaultView;
                dvNonChronic.RowFilter = "IsChronic = 'False'";
                dtNonChronic = dvNonChronic.ToTable();


                dvChronic.Dispose();
                dvNonChronic.Dispose();
                if (dtChronic.Rows.Count > 0)
                {

                    sbTemp.Append(sBeginFont);
                    //  sbTemp.Append("<br/>"); yogesh
                    //sbTemp.Append("The patient, " + common.myStr(ds2.Tables[0].Rows[0]["PatientName"]).Trim() + ", is a ");
                    //sbTemp.Append(strAge + " old " + common.myStr(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + common.myStr(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " with a history of ");
                    #region // Chronic problems starts here
                    #region Comment for Chronic sentence which generate in case sheet.
                    //for (int i = 0; i < dtChronic.Rows.Count; i++)
                    //{
                    //    DataRow dr1 = dtChronic.Rows[i] as DataRow;
                    //    if (i == 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                    //        }
                    //        else
                    //        {
                    //            if (i == dtChronic.Rows.Count - 1)
                    //            {
                    //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                    //                sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                    //            }
                    //            else
                    //            {
                    //                sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                    //        }
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

                    //    if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem2"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem2"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem3"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem3"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem4"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem4"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem5"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem5"]) > 0)
                    //    {
                    //        if (i == dtChronic.Rows.Count - 1)
                    //        {
                    //            sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                    //        }
                    //        else
                    //        {
                    //            sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                    //        }
                    //    }
                    //}
                    #endregion
                    #endregion // Chronic problems sentence ends here

                    #region non chronic problem sentence start
                    // Non Chronic problems starts here
                    #region comment for Non Chronic sentence which generate in case sheet.
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
                    //        if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //        if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
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

                    //    if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem2"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                    //        }
                    //        else
                    //        {
                    //            if (i == dtNonChronic.Rows.Count - 1)
                    //            {
                    //                sbTemp.Remove(sbTemp.Length - 2, 2);
                    //                sbTemp.Append(" and " + dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ". ");
                    //            }
                    //            else
                    //            {
                    //                sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                    //            }
                    //        }
                    //    }
                    //    if (common.myLen(dr1["AssociatedProblem2"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem3"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem3"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem4"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem4"]) > 0)
                    //    {
                    //        if (common.myLen(dr1["AssociatedProblem5"]) > 0)
                    //        {
                    //            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                    //        }
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
                    //    if (common.myLen(dr1["AssociatedProblem5"]) > 0)
                    //    {
                    //        if (i == dtNonChronic.Rows.Count - 1)
                    //        {
                    //            sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                    //        }
                    //        else
                    //        {
                    //            sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                    //        }
                    //    }
                    //}
                    //sbTemp.Append(sEndFont);
                    #endregion
                    #endregion //Non Chronic problems ends here.
                }
                else
                {
                    if (dtNonChronic.Rows.Count > 0 && common.myStr(dtProblem.Rows[0]["HPIRemarks"]).Trim() == "")
                    {
                        #region if only non chronic problem then sentence start here

                        sbTemp.Append(sBeginFont);
                        if (common.myStr(sb) != "")
                        {
                            sbTemp.Append("<br/>");
                        }
                        //sbTemp.Append("The patient, " + common.myStr(ds2.Tables[0].Rows[0]["PatientName"]).Trim() + ", is a ");
                        //sbTemp.Append(strAge + "  old " + common.myStr(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + common.myStr(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " who presents with ");

                        #region comment for Non Chronic sentence which generate in case sheet.
                        //for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                        //{
                        //    DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                        //    if (i == 0)
                        //    {
                        //        if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                        //        {
                        //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");

                        //        }
                        //        else
                        //        {
                        //            if (i == dtNonChronic.Rows.Count - 1)
                        //            {
                        //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + "");
                        //                sbTemp.Append("<br/>");
                        //            }
                        //            else
                        //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + "");

                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                        //        {
                        //            sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                        //        }
                        //        else
                        //        {
                        //            if (i == dtNonChronic.Rows.Count - 1)
                        //            {
                        //                //sbTemp.Remove(sbTemp.Length - 2, 2);
                        //                sbTemp.Append("<br/>");
                        //                sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + " ");
                        //                sbTemp.Append("<br/>");

                        //                //sbTemp.Append(" and " + dr1["ProblemDescription"].ToString().ToLower().Trim() + ". ");
                        //            }
                        //            else
                        //            {
                        //                sbTemp.Append("<br/>");
                        //                sbTemp.Append("" + dr1["ProblemDescription"].ToString().ToLower().Trim() + " ");
                        //            }
                        //        }
                        //    }

                        //    if (common.myLen(dr1["AssociatedProblem1"]) > 0)
                        //    {
                        //        if (common.myLen(dr1["AssociatedProblem2"]) > 0)
                        //        {
                        //            sbTemp.Append(dr1["AssociatedProblem1"].ToString().ToLower().Trim() + ", ");
                        //        }
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
                        //    if (common.myLen(dr1["AssociatedProblem2"]) > 0)
                        //    {
                        //        if (common.myLen(dr1["AssociatedProblem3"]) > 0)
                        //        {
                        //            sbTemp.Append(dr1["AssociatedProblem2"].ToString().ToLower().Trim() + ", ");
                        //        }
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
                        //    if (common.myLen(dr1["AssociatedProblem3"]) > 0)
                        //    {
                        //        if (common.myLen(dr1["AssociatedProblem4"]) > 0)
                        //        {
                        //            sbTemp.Append(dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        //        }
                        //        else
                        //        {
                        //            if (i == dtNonChronic.Rows.Count - 1)
                        //            {
                        //                sbTemp.Remove(sbTemp.Length - 2, 2);
                        //                sbTemp.Append("and " + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ". ");
                        //            }
                        //            else
                        //                sbTemp.Append("" + dr1["AssociatedProblem3"].ToString().ToLower().Trim() + ", ");
                        //        }
                        //    }
                        //    if (common.myLen(dr1["AssociatedProblem4"]) > 0)
                        //    {
                        //        if (common.myLen(dr1["AssociatedProblem5"]) > 0)
                        //        {
                        //            sbTemp.Append(dr1["AssociatedProblem4"].ToString().ToLower().Trim() + ", ");
                        //        }
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
                        //    if (common.myLen(dr1["AssociatedProblem5"]) > 0)
                        //    {
                        //        if (i == dtNonChronic.Rows.Count - 1)
                        //        {
                        //            sbTemp.Append("and " + common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                        //        }
                        //        else
                        //        {
                        //            sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                        //        }
                        //    }
                        //}
                        //sbTemp.Append(sEndFont);
                        #endregion
                        #endregion if non chronic problem sentence ends
                    }
                }
                //if (common.myBool(ds2.Tables[1].Rows[0]["IsPregnant"]))
                //{
                //    sbTemp.Append(sBeginFont + " The patient is or may be pregnant. " + sEndFont);
                //}
                //if (common.myBool(ds2.Tables[1].Rows[0]["IsBreastFeeding"]))
                //{
                //    sbTemp.Append(sBeginFont + " The patient is breast feeding. " + sEndFont);
                //}

                //sbTemp.Append(BeginList); //cut from here 
                # region chronic problem listing
                for (int i = 0; i < dtChronic.Rows.Count; i++)
                {
                    //sbTemp.Append(BeginList);// pasted here 
                    //sbTemp.Append(sBegin + " The patient describes the chronic " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                    DataRow dr1 = dtChronic.Rows[i] as DataRow;
                    if ((common.myLen(dr1["Quality1"]) > 0) || (common.myLen(dr1["Quality2"]) > 0)
                        || (common.myLen(dr1["Location"]) > 0) || (common.myLen(dr1["Severity"]) > 0)
                        || (common.myLen(dr1["Onset"]) > 0) || (common.myLen(dr1["Context"]) > 0)
                        || (common.myLen(dr1["Duration"]) > 0) || (common.myLen(dr1["NoOfOccurrence"]) > 0)
                        || (common.myLen(dr1["PriorIllnessDate"]) > 0))
                    {
                        //Changed onsetDate to onset
                        //Added this line to append string after if condition,previously it is above the if condition
                        //Vineet
                        if (common.myLen(dr1["ProblemDescription"]) > 0)
                        {
                            //sbTemp.Append(sBegin + " The patient describes the chronic " + common.myStr(dr1["ProblemDescription"]).ToLower() + " ");

                            sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]).ToLower() + "<br/>");
                        }
                        #region Comment the sentence for chronic which generate in case sheet.
                        //if (common.myLen(dr1["Quality1"]) > 0)
                        //{
                        //    if (common.myLen(dr1["Quality2"]) > 0)
                        //    {
                        //        sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + ", ");
                        //    }
                        //    else
                        //    {
                        //        if ((common.myStr(dr1["Location"]).Equals(string.Empty)) && (common.myStr(dr1["Severity"]).Equals(string.Empty)))
                        //        {
                        //            sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + ". ");
                        //        }
                        //        else
                        //            sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + " ");
                        //    }
                        //}
                        //if (common.myLen(dr1["Quality2"]) > 0)
                        //{
                        //    if (common.myLen(dr1["Quality3"]) > 0)
                        //    {
                        //        sbTemp.Append(common.myStr(dr1["Quality2"]).ToLower().Trim() + ", ");
                        //    }
                        //    else
                        //    {
                        //        if ((common.myStr(dr1["Location"]).Equals(string.Empty)) && (common.myStr(dr1["Severity"]).Equals(string.Empty)))
                        //        {
                        //            sbTemp.Remove(sbTemp.Length - 2, 2);
                        //            sbTemp.Append(" and " + common.myStr(dr1["Quality2"]).ToLower().Trim() + ". ");
                        //        }
                        //        else
                        //        {
                        //            sbTemp.Remove(sbTemp.Length - 2, 2);
                        //            sbTemp.Append(" and " + common.myStr(dr1["Quality2"]).ToLower().Trim() + " ");
                        //        }
                        //    }
                        //}
                        //if (common.myLen(dr1["Quality3"]) > 0)
                        //{
                        //    if (common.myLen(dr1["Quality4"]) > 0)
                        //    {
                        //        sbTemp.Append(common.myStr(dr1["Quality3"]).ToLower().Trim() + ", ");
                        //    }
                        //    else
                        //    {
                        //        if (common.myLen(dr1["Location"]).Equals(0) && common.myLen(dr1["Severity"]).Equals(0))
                        //        {
                        //            sbTemp.Remove(sbTemp.Length - 2, 2);
                        //            sbTemp.Append(" and " + common.myStr(dr1["Quality3"]).ToLower().Trim() + ". ");
                        //        }
                        //        else
                        //        {
                        //            sbTemp.Remove(sbTemp.Length - 2, 2);
                        //            sbTemp.Append(" and " + common.myStr(dr1["Quality3"]).ToLower().Trim() + " ");
                        //        }
                        //    }
                        //}
                        //if (common.myLen(dr1["Quality4"]) > 0)
                        //{
                        //    if (common.myLen(dr1["Quality5"]) > 0)
                        //    {
                        //        sbTemp.Append(common.myStr(dr1["Quality4"]).ToLower().Trim() + ", ");
                        //    }
                        //    else
                        //    {
                        //        if (common.myLen(dr1["Location"]).Equals(0) && common.myLen(dr1["Severity"]).Equals(0))
                        //        {
                        //            sbTemp.Remove(sbTemp.Length - 2, 2);
                        //            sbTemp.Append(" and " + common.myStr(dr1["Quality4"]).ToLower().Trim() + ". ");
                        //        }
                        //        else
                        //        {
                        //            sbTemp.Remove(sbTemp.Length - 2, 2);
                        //            sbTemp.Append(" and " + common.myStr(dr1["Quality4"]).ToLower().Trim() + " ");
                        //        }
                        //    }
                        //}
                        //if (common.myLen(dr1["Quality5"]) > 0)
                        //{
                        //    if (common.myLen(dr1["Location"]).Equals(0) && common.myLen(dr1["Severity"]).Equals(0))
                        //    {
                        //        sbTemp.Remove(sbTemp.Length - 2, 2);
                        //        sbTemp.Append(" and " + common.myStr(dr1["Quality5"]).ToLower().Trim() + ". ");
                        //    }
                        //    else
                        //    {
                        //        sbTemp.Remove(sbTemp.Length - 2, 2);
                        //        sbTemp.Append(" and " + common.myStr(dr1["Quality5"]).ToLower().Trim() + " ");
                        //    }
                        //}

                        //if (common.myLen(dr1["Location"]) > 0)
                        //{
                        //    if (common.myLen(dr1["Severity"]) > 0)
                        //    {
                        //        if (common.myLen(dr1["SideDescription"]) > 0)
                        //        {
                        //            sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " (" + common.myStr(dr1["SideDescription"]).ToLower().Trim() + ") ");
                        //        }
                        //        else
                        //        {
                        //            sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " ");
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (common.myLen(dr1["SideDescription"]) > 0)
                        //        {
                        //            sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " (" + common.myStr(dr1["SideDescription"]).ToLower().Trim() + "). ");
                        //        }
                        //        else
                        //        {
                        //            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
                        //        }
                        //    }
                        //}
                        //if (Convert.ToString(dr1["Severity"]).Trim() != "")
                        //{
                        //    if (Convert.ToString(dr1["OnsetDate"]).Trim() != "")
                        //        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                        //    else
                        //    {
                        //        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                        //    }
                        //}


                        //sbTemp.Append("that began on [OnSet Date] ");  
                        //if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                        //{
                        //    if (i == 0)
                        //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //    else
                        //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //}
                        //else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                        //{
                        //    if (i == 0)
                        //        sbTemp.Append("symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //    else
                        //        sbTemp.Append("symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //}
                        //else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                        //{
                        //    if (i == 0)
                        //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //    else
                        //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower() + " and " + Convert.ToString(dr1["Duration"]).ToLower() + ". ");
                        //}
                        //else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                        //{
                        //    if (i == 0)
                        //        sbTemp.Append("symptoms for the  last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //    else
                        //        sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                        //}
                        //else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "")
                        //{
                        //    if (i == 0)
                        //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                        //    else
                        //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                        //}
                        //else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
                        //{
                        //    if (i == 0)
                        //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                        //    else
                        //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                        //}

                        //if (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
                        //{
                        //    if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
                        //        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                        //    else
                        //        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times. ");
                        //}
                        //if (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != "")
                        //    sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");

                    }

                    //asdfg
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
                    //    if (Convert.ToString(dr1["DeniesSymptoms3"]).Trim() != "")
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
                    sbTemp.Append(sEnd);
                    //sbTemp.Append(EndList);// pasted here

                }
                #endregion
                if (dtNonChronic.Rows.Count > 0 && common.myStr(dtProblem.Rows[0]["HPIRemarks"]).Trim() == "")
                {
                    # region nonchronic problem details with listing
                    StringBuilder sbchief = new StringBuilder();  //Ritika (20-09-2022)For comma Issue at the end
                    for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                    {
                        //sbTemp.Append(BeginList);// pasted here   
                        DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                        //sbTemp.Append("<br/><br/>" + Convert.ToString(dr1["ProblemDescription"]) + " presents with ");
                        //sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");


                        //////   if ((Convert.ToString(dr1["Quality1"]).ToLower() != "") || (Convert.ToString(dr1["Location"]) != "") || (Convert.ToString(dr1["Severity"]) != "") || (Convert.ToString(dr1["Onset"]) != "") || (Convert.ToString(dr1["Context"]) != "") || (Convert.ToString(dr1["Duration"]) != "") || (Convert.ToString(dr1["NoOfOccurrence"]).Trim() != "") || (Convert.ToString(dr1["PriorIllnessDate"]).Trim() != ""))
                        if (common.myStr(dr1["ProblemDescription"]).Trim() != "")
                        {
                            //Changed onsetDate to onset
                            //Added this line to append string after if condition,previously it is above the if condition
                            //Vineet
                            if (common.myStr(dr1["ProblemDescription"]).Trim() != "")
                            {
                                // sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]).ToLower() + "<br/>");
                                // sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"]) + "<br/>");

                                //Ritika (20-09-2022)For comma Issue at the end
                                //sbTemp.Append(sBegin + common.myStr(dr1["ProblemDescription"] + ", ")); // K.k       Added comma by Akshay 12082022 Mission
                                sbchief.Append(sBegin + (sbchief.ToString() == "" && dtNonChronic.Rows.Count > 1 ? common.myStr(dr1["ProblemDescription"]) + "," : common.myStr(dr1["ProblemDescription"])));
                                //sbTemp.Append(sBegin + " The patient describes the " + common.myStr(dr1["ProblemDescription"]).ToLower() + " ");
                            }
                            # region Comment the non Chronic sentence which generate in case sheet
                            //if (Convert.ToString(dr1["Quality1"]).ToLower() != "")
                            //{
                            //    if (Convert.ToString(dr1["Quality2"]).Trim() != "")
                            //        sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ", ");
                            //    else
                            //    {
                            //        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            //        {
                            //            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + ". ");
                            //        }
                            //        else
                            //            sbTemp.Append("as " + Convert.ToString(dr1["Quality1"]).ToLower().Trim() + " ");
                            //    }
                            //}
                            //if (Convert.ToString(dr1["Quality2"]).Trim() != "")
                            //{
                            //    if (Convert.ToString(dr1["Quality3"]).Trim() != "")
                            //        sbTemp.Append("" + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ", ");
                            //    else
                            //    {
                            //        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            //        {
                            //            sbTemp.Remove(sbTemp.Length - 2, 2);
                            //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + ". ");
                            //        }
                            //        else
                            //        {
                            //            sbTemp.Remove(sbTemp.Length - 2, 2);
                            //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality2"]).ToLower().Trim() + " ");
                            //        }
                            //    }
                            //}
                            //if (Convert.ToString(dr1["Quality3"]).Trim() != "")
                            //{
                            //    if (Convert.ToString(dr1["Quality4"]).Trim() != "")
                            //        sbTemp.Append("" + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ", ");
                            //    else
                            //    {
                            //        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            //        {
                            //            sbTemp.Remove(sbTemp.Length - 2, 2);
                            //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + ". ");
                            //        }
                            //        else
                            //        {
                            //            sbTemp.Remove(sbTemp.Length - 2, 2);
                            //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality3"]).ToLower().Trim() + " ");
                            //        }
                            //    }
                            //}
                            //if (Convert.ToString(dr1["Quality4"]).Trim() != "")
                            //{
                            //    if (Convert.ToString(dr1["Quality5"]).Trim() != "")
                            //        sbTemp.Append("" + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ", ");
                            //    else
                            //    {
                            //        if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            //        {
                            //            sbTemp.Remove(sbTemp.Length - 2, 2);
                            //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + ". ");
                            //        }
                            //        else
                            //        {
                            //            sbTemp.Remove(sbTemp.Length - 2, 2);
                            //            sbTemp.Append(" and " + Convert.ToString(dr1["Quality4"]).ToLower().Trim() + " ");
                            //        }
                            //    }
                            //}
                            //if (Convert.ToString(dr1["Quality5"]).Trim() != "")
                            //{
                            //    if ((Convert.ToString(dr1["Location"]) == "") && (Convert.ToString(dr1["Severity"]) == ""))
                            //    {
                            //        sbTemp.Remove(sbTemp.Length - 2, 2);
                            //        sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + ". ");
                            //    }
                            //    else
                            //    {
                            //        sbTemp.Remove(sbTemp.Length - 2, 2);
                            //        sbTemp.Append(" and " + Convert.ToString(dr1["Quality5"]).ToLower().Trim() + " ");
                            //    }
                            //}

                            //if (Convert.ToString(dr1["Location"]).Trim() != "")
                            //{
                            //    if (Convert.ToString(dr1["Severity"]).Trim() != "")
                            //    {
                            //        if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
                            //        {
                            //            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + ") ");
                            //        }
                            //        else
                            //            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " ");
                            //    }
                            //    else
                            //    {
                            //        if (Convert.ToString(dr1["SideDescription"]).Trim() != "")
                            //        {
                            //            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + " (" + Convert.ToString(dr1["SideDescription"]).ToLower().Trim() + "). ");
                            //        }
                            //        else
                            //            sbTemp.Append("" + Convert.ToString(dr1["Location"]).ToLower().Trim() + ". ");
                            //    }
                            //}
                            //if (Convert.ToString(dr1["Severity"]).Trim() != "")
                            //{
                            //    if (Convert.ToString(dr1["Onset"]).Trim() != "")
                            //        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("with a severity of " + Convert.ToString(dr1["Severity"]).ToLower().Trim() + ". ");
                            //}
                            //sbTemp.Append("that began on [OnSet Date] "); 



                            //if (Convert.ToString(dr1["Onset"]).Trim() != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                            //{
                            //    if (i == 0)
                            //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ", occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //}
                            //else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) != "")
                            //{
                            //    if (i == 0)
                            //        sbTemp.Append("symptom occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("symptoms occur " + Convert.ToString(dr1["Context"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //}
                            //else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                            //{
                            //    if (i == 0)
                            //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //}
                            //else if (Convert.ToString(dr1["Onset"]) == "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) != "")
                            //{
                            //    if (i == 0)
                            //        sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("symptoms for the last " + Convert.ToString(dr1["Duration"]).ToLower().Trim() + ". ");
                            //}
                            //else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) != "" && Convert.ToString(dr1["Duration"]) == "")
                            //{
                            //    if (i == 0)
                            //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + " and occurs " + Convert.ToString(dr1["Context"]).ToLower().Trim() + ". ");
                            //}
                            //else if (Convert.ToString(dr1["Onset"]) != "" && Convert.ToString(dr1["Context"]) == "" && Convert.ToString(dr1["Duration"]) == "")
                            //{
                            //    if (i == 0)
                            //        sbTemp.Append("symptom is " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                            //    else
                            //        sbTemp.Append("symptoms are " + Convert.ToString(dr1["Onset"]).ToLower().Trim() + ". ");
                            //}

                            //if (Convert.ToString(dr1["NoOfOccurrence"]) != "" && Convert.ToString(dr1["NoOfOccurrence"]) != "0")
                            //{
                            //    if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
                            //        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                            //    else
                            //        sbTemp.Append(" Problem occurred " + Convert.ToString(dr1["NoOfOccurrence"]).ToLower().Trim() + " times.");
                            //}
                            //if (Convert.ToString(dr1["PriorIllnessDate"]) != "")
                            //    sbTemp.Append(" beginning on " + Convert.ToString(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");
                        }
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
                        sbchief.Append(sEnd); //Ritika (20-09-2022)For comma Issue at the end
                        //sbTemp.Append(EndList);// pasted here      
                    }
                    sbTemp.Append(sbchief); //Ritika (20-09-2022)For comma Issue at the end
                    # endregion
                }
            }
            sb.Append(sbTemplateStyle);
            DataView dvRemarks = new DataView(dtProblem);
            if (GroupingDate != "")
            {
                dvRemarks.RowFilter = " GroupDate = '" + GroupingDate + "' AND ISNULL(HPIRemarks,'')<>''";
            }
            else
            {
                dvRemarks.RowFilter = "ISNULL(HPIRemarks,'')<>''";
            }
            if (dvRemarks.ToTable().Rows.Count > 0)
            {
                if (common.myStr(dvRemarks.ToTable().Rows[0]["HPIRemarks"]).Trim() != "")
                {
                    sbTemp.Append(sBeginFont + "<br/>" + common.myStr(dvRemarks.ToTable().Rows[0]["HPIRemarks"]).Trim() + sEndFont);
                }
            }
            sb.Append(sbTemp);
            dvRemarks.Dispose();
            ds.Dispose();
            sbTemp = null;
            fonts = null;
            dtChronic.Dispose();
            dtNonChronic.Dispose();
            dsProblem.Dispose();
        }
        //ds2.Dispose();
        return sb;
    }


    public StringBuilder BindAssessments(DataTable dtDiagnosis, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID, int TemplateFieldId, int EncounterId, string GroupingDate)
    {
        return BindAssessments(dtDiagnosis, sb, sbTemplateStyle, drTemplateListStyle,
                       pg, pageID, userID, TemplateFieldId, EncounterId, GroupingDate, "Y", false);
    }

    public StringBuilder BindAssessments(DataTable dtDiagnosis, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                       Page pg, string pageID, string userID, int TemplateFieldId, int EncounterId, string GroupingDate, string IsShowDiagnosisGroupHeading, bool StaticTemplateDisplayTitle)
    {
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

        DataView dvDiagnosis = new DataView(dtDiagnosis);
        if (GroupingDate != "")
        {
            dvDiagnosis.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvDiagnosis.RowFilter = " EncounterId=" + EncounterId;
        }
        dsDiagnosis.Tables.Add(dvDiagnosis.ToTable());
        dvDiagnosis.Dispose();
        dtDiagnosis.Dispose();

        DataView dv = new DataView(dsDiagnosis.Tables[0]);
        //dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId; 12-03-2022
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
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                {
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Diagnosis With ICD Code" + EndTemplateStyle);
                        sb.Append("<br/>");
                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");
                    }
                }
            }
            sBeginFont = "";
            sEndFont = "";
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
                    sbChronic.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                sbChronic.Append(sEnd + "<br/>");
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
                    sbPrimary.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                sbPrimary.Append(sEnd + "<br/>");
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
                    sbSeconday.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                sbSeconday.Append(sEnd + "<br/>");
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
                    sbQuery.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                sbQuery.Append(sEnd + "<br/>");
            }
            //commented by satosh first case
            else if (dr["IsChronic"].ToString() == "True" && dr["PrimaryDiagnosis"].ToString() == "True" && dr["IsQuery"].ToString() == "True")
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
                        sbChronic.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                    sbChronic.Append(sEnd + "<br/>");
                }

                if (dr["PrimaryDiagnosis"].ToString() == "True")
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
                        sbPrimary.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                    sbPrimary.Append(sEnd + "<br/>");
                }
                if (dr["IsQuery"].ToString() == "True")
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
                        sbQuery.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                    sbQuery.Append(sEnd + "<br/>");
                }
            }
            else if (dr["PrimaryDiagnosis"].ToString() == "True" && dr["IsQuery"].ToString() == "True")
            {
                if (dr["PrimaryDiagnosis"].ToString() == "True")
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
                        sbPrimary.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                    sbPrimary.Append(sEnd + "<br/>");
                }


            }

            else if (dr["IsResolved"].ToString() == "True" && dr["IsFinalDiagnosis"].ToString() == "True")
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
                    sbSeconday.Append("<br/>Remarks : " + dr["Remarks"].ToString() + ". ");
                sbSeconday.Append(sEnd + "<br/>");
            }
            //commented end
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
            //sb.Append("<br/>" + sBeginSection + "Chronic Diagnosis: " + sEndSection + "<br/>");
            if (IsShowDiagnosisGroupHeading.Equals("Y"))
            {
                sb.Append(sBeginSection + "Chronic Diagnosis: " + sEndSection + "<br/>");
            }


            sbChronic.Append(EndList);
            sb.Append(sbChronic.ToString());
        }
        if (pF == 1)
        {
            //sb.Append("<br/>" + sBeginSection + "Primary Diagnosis: " + sEndSection + "<br/>");
            if (IsShowDiagnosisGroupHeading.Equals("Y"))
            {
                sb.Append(sBeginSection + "Primary Diagnosis: " + sEndSection + "<br/>");
            }


            sbPrimary.Append(EndList);
            sb.Append(sbPrimary.ToString());
        }
        if (sF == 1)
        {
            //sb.Append("<br/>" + sBeginSection + "Secondary Diagnosis: " + sEndSection + "<br/>");

            if (IsShowDiagnosisGroupHeading.Equals("Y"))
            {
                sb.Append(sBeginSection + "Secondary Diagnosis: " + sEndSection + "<br/>");
            }

            sbSeconday.Append(EndList);
            sb.Append(sbSeconday.ToString());
        }
        if (qF == 1)
        {
            //sb.Append("<br/>" + sBeginSection + "Provisional Diagnosis: " + sEndSection + "<br/>");
            if (IsShowDiagnosisGroupHeading.Equals("Y"))
            {
                sb.Append(sBeginSection + "Provisional Diagnosis: " + sEndSection + "<br/>");
            }


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
        return sb;
    }



    public StringBuilder BindTabularAssessments(DataTable dtDiagnosis, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                       Page pg, string pageID, string userID, int TemplateFieldId, int EncounterId, string GroupingDate,
                       string IsShowDiagnosisGroupHeading, bool StaticTemplateDisplayTitle)
    {
        ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataSet dsDiagnosis = new DataSet();

        StringBuilder sbDiagnosis = new StringBuilder();


        DataView dvDiagnosis = new DataView(dtDiagnosis);
        if (GroupingDate != "")
        {
            dvDiagnosis.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvDiagnosis.RowFilter = " EncounterId=" + EncounterId;
        }
        dsDiagnosis.Tables.Add(dvDiagnosis.ToTable());
        dvDiagnosis.Dispose();
        dtDiagnosis.Dispose();

        DataView dv = new DataView(dsDiagnosis.Tables[0]);
        // dv.RowFilter = "ISNULL(TemplateFieldId,0)=" + TemplateFieldId; // 12-03-2022.
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
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                {
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Diagnosis" + EndTemplateStyle);
                        // sb.Append("<br/>");
                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        // sb.Append("<br/>");
                    }
                }
            }
            sBeginFont = "";
            sEndFont = "";
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
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: left;'><b>" + sBeginSection + "ICD Code" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='5' border='1' style='text-align: left;'><b>" + sBeginSection + "Diagnosis" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: left;'><b>" + sBeginSection + "Provisonal/Final Diagnosis" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: left;'><b>" + sBeginSection + "Primary /  Secondary" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='3' border='1' style='text-align: left;'><b>" + sBeginSection + "Chronic" + sEndSection + "</b></td>");
        sbDiagnosis.Append("<td colspan='5' border='1' style='text-align: left;'><b>" + sBeginSection + "Remarks" + sEndSection + "</b></td>");
        sbDiagnosis.Append("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {

            string strChronic = string.Empty;
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
                strChronic = "Chronic";
            }

            sbDiagnosis.Append("<tr>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1' style='text-align: left;'>" + sBeginSection + common.myStr(dr["ICDCode"]) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='5' valign='top' border='1'>" + sBeginSection + common.myStr(dr["ICDDescription"]) + sEndSection + "<span style=' font-size:7pt; color: #000000; font-family: Arial; '></span>  </td>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(dr["ProvisionalFinalDiagnosis"]) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(strPrimaryDiagnosis) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(strChronic) + sEndSection + "</td>");
            sbDiagnosis.Append("<td colspan='5' valign='top' border='1'>" + sBeginSection + common.myStr(dr["Remarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br/>") + sEndSection + "</td>");
            sbDiagnosis.Append("</tr>");
        }
        sbDiagnosis.Append("</table>");
        sBegin = "";
        sEnd = "";
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;
        sb.Append(sbDiagnosis.ToString());
        ds.Dispose();
        ds1.Dispose();
        dsDiagnosis.Dispose();

        return sb;
    }

    public StringBuilder BindPatientProvisionalDiagnosis(DataTable dtProDiagnosis, string DocId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                    Page pg, string pageID, string userID, int TemplateFieldId, int EncounterId, string GroupingDate)
    {
        return BindPatientProvisionalDiagnosis(dtProDiagnosis, DocId, sb, sbTemplateStyle, drTemplateListStyle,
                        pg, pageID, userID, TemplateFieldId, EncounterId, GroupingDate, true);
    }

    public StringBuilder BindPatientProvisionalDiagnosis(DataTable dtProDiagnosis, string DocId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageID, string userID, int TemplateFieldId, int EncounterId, string GroupingDate, bool StaticTemplateDisplayTitle)
    {
        StringBuilder sbProvisional = new StringBuilder();
        ds = new DataSet();
        //yogesh 08_09_2022
        string strIsShowProvisionalDiagnosisHeading = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                         common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "IsShowProvisionalDiagnosisHeading", sConString);

        //yogesh 08_09_2022
        string strIsShowFinalDiagnosisHeading = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                                         common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "IsShowFinalDiagnosisHeading", sConString);




        try
        {
            DataView dvProDiagnosis = new DataView(dtProDiagnosis);
            if (GroupingDate != "")
            {
                dvProDiagnosis.RowFilter = " GroupDate = '" + GroupingDate + "' AND EncounterId=" + EncounterId;
            }
            else
            {
                dvProDiagnosis.RowFilter = " EncounterId=" + EncounterId;
            }
            ds.Tables.Add(dvProDiagnosis.ToTable());
            dvProDiagnosis.Dispose();
            dtProDiagnosis.Dispose();

            string sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                {
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                    string BeginTemplateStyle = sBegin;
                    string EndTemplateStyle = sEnd;
                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Provisional Diagnosis" + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                    else
                    {
                        if (!BeginTemplateStyle.Trim().Contains("<br/>"))
                        {
                            // sb.Append(BeginTemplateStyle + "<br/>" + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                            sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);

                        }
                        else
                        {
                            sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        }
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

                    if (common.myBool(ds.Tables[0].Rows[i]["Admitting"]))
                    {
                        //Ritika(03-10-2022)Change For Comma
                        sbProvisional.Append(ds.Tables[0].Rows.Count == 1 || i+1 == ds.Tables[0].Rows.Count ? sBeginSection + "Type: Admitting" + sEndSection : sBeginSection + ", Type: Admitting" + sEndSection);
                    }
                    else if (common.myBool(ds.Tables[0].Rows[i]["Provisional"]))
                    {

                        //yogesh 08_09_2022
                        if (strIsShowProvisionalDiagnosisHeading.Equals("Y"))
                        {
                            //Ritika(03-10-2022)Change For Comma
                            sbProvisional.Append(ds.Tables[0].Rows.Count == 1 || i+1 == ds.Tables[0].Rows.Count ? sBeginSection + "Type: Provisional" + sEndSection : sBeginSection + ", Type: Provisional" + sEndSection);
                        }
                        else
                        {
                            //Ritika(03-10-2022)Change For Comma
                            sbProvisional.Append(ds.Tables[0].Rows.Count == 1 || i+1 == ds.Tables[0].Rows.Count ?  "" : sBeginSection + "," + sEndSection);
                        }


                    }
                    else if (common.myBool(ds.Tables[0].Rows[i]["Final"]))
                    {
                        //yogesh 08_09_2022
                        if (strIsShowFinalDiagnosisHeading.Equals("Y"))
                        {
                            //Ritika(03-10-2022)Change For Comma
                            sbProvisional.Append(ds.Tables[0].Rows.Count ==1 || i+1 == ds.Tables[0].Rows.Count  ? sBeginSection + "Type: Final" + sEndSection : sBeginSection + ", Type: Final" + sEndSection);
                        }
                        else
                        {
                            //Ritika(03-10-2022)Change For Comma
                            sbProvisional.Append(ds.Tables[0].Rows.Count == 1 || i+1 == ds.Tables[0].Rows.Count ? "" : sBeginSection + "," + sEndSection);
                        }
                    }

                    else if (common.myBool(ds.Tables[0].Rows[i]["Chronic"]))
                    {
                        //Ritika(03-10-2022)Change For Comma
                        sbProvisional.Append(ds.Tables[0].Rows.Count == 1 || i+1== ds.Tables[0].Rows.Count ? sBeginSection + "Type: Chronic" + sEndSection  : sBeginSection + ", Type: Chronic" + sEndSection);
                    }
                    else if (common.myBool(ds.Tables[0].Rows[i]["Discharge"]))
                    {
                        //Ritika(03-10-2022)Change For Comma
                        sbProvisional.Append(ds.Tables[0].Rows.Count == 1 || i+1 == ds.Tables[0].Rows.Count  ? sBeginSection + "Type: Discharge" + sEndSection :  sBeginSection + ", Type: Discharge" + sEndSection);
                    }

                    if (!common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Equals(string.Empty))
                    {
                        sbProvisional.Append(sBeginSection + "<br/>Remarks : " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]) + sEndSection);
                    }
                    sbProvisional.Append(DisplayUserNameInNote(ds.Tables[0].Rows[i]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[i]["EncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(ds.Tables[0].Rows[i]["EnteredById"].ToString())));
                    sb.Append(sbProvisional);
                    sb.Append("<br/>");
                }
                // sb.Append("<br/>");
            }
        }
        catch (Exception Ex)
        {
        }
        finally
        {
            ds.Dispose();
            sbProvisional = null;
        }
        return sbProvisional;
    }


    public StringBuilder BindAllergies(DataTable dtAllergy, StringBuilder sb, StringBuilder sbTemplateStyle,
       DataRow drTemplateListStyle, Page pg, string PageID, int TemplateFieldId, string GroupingDate)
    {
        return BindAllergies(dtAllergy, sb, sbTemplateStyle,
                       drTemplateListStyle, pg, PageID, TemplateFieldId, GroupingDate, true);
    }
    public StringBuilder BindAllergies(DataTable dtAllergy, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string PageID, int TemplateFieldId, string GroupingDate, bool StaticTemplateDisplayTitle)
    {
        ds = new DataSet();
        ds2 = new DataSet();
        DataSet dsAllergy = new DataSet();
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbDrugAllergy = new StringBuilder();
        StringBuilder sbOtherAllergy = new StringBuilder();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        int t = 0;

        ds2 = emr.getEMRPatientProblemAndAllergyTemplateString(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), common.myInt(System.Web.HttpContext.Current.Session["RegistrationId"]), 0);


        DataView dvAllergy = new DataView(dtAllergy);
        ds.Tables.Add(dvAllergy.ToTable());
        dvAllergy.Dispose();


        DataView dvDrug = new DataView(ds.Tables[0]);
        dvDrug.RowFilter = "AllergyType = 'Drug'";

        DataView dvOther = new DataView(ds.Tables[0]);
        dvOther.RowFilter = "AllergyType <> 'Drug'";

        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (sbTemplateStyle.ToString().Contains("Allergies") == false)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
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

            }
            sBeginFont = "";
            sEndFont = "";
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
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
        if (ds2.Tables[0].Rows.Count > 0)
        {
            //if (Convert.ToString(dsAllergy.Tables[1].Rows[0]["NoAllergies"]) == "True")
            //{
            //    sb.Append(sbTemplateStyle + "<u><b>" + "Allergies: " + "</b></u>" + BeginList);
            //}
            if (!common.myBool(ds2.Tables[0].Rows[0]["NoAllergies"]))
            {
                foreach (DataRowView dr in dvDrug)
                {
                    if (t == 0)
                    {
                        if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                        {
                            sbDrugAllergy.Append("<br/>" + sBeginSection + "Drug Allergies <br/>" + sEndSection + BeginList);
                        }
                        else
                        {
                            sbDrugAllergy.Append(sBeginSection + "Drug Allergies <br/>" + sEndSection + BeginList);
                        }
                        t = 1;
                    }
                    sBegin = ""; sEnd = "";
                    MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
                    sbDrugAllergy.Append(" " + sBegin + common.myStr(dr["AllergyName"]).Trim() + " (" + common.myStr(dr["Generic_Name"]).Trim() + ")");
                    if (common.myLen(dr["AllergyDate"]) > 0)
                    {
                        sbDrugAllergy.Append(", Onset Date: " + common.myStr(dr["AllergyDate"]).Trim());
                    }
                    if (common.myLen(dr["Reaction"]) > 0)
                    {
                        sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]).Trim());

                        if (!common.myBool(dr["Intolerance"]) && common.myLen(dr["Remarks"]).Equals(0))
                        {
                            sbDrugAllergy.Append(".");
                        }
                    }
                    if (common.myLen(dr["AllergySeverity"]) > 0)
                    {
                        sbDrugAllergy.Append(", Severity level : " + common.myStr(dr["AllergySeverity"]).Trim());
                    }
                    if (common.myBool(dr["Intolerance"]))
                    {
                        sbDrugAllergy.Append(", Intolerable");

                        if (common.myLen(dr["Remarks"]).Equals(0))
                        {
                            sbDrugAllergy.Append(".");
                        }
                    }
                    if (common.myLen(dr["Remarks"]) > 0)
                    {
                        sbDrugAllergy.Append(", Remarks: " + common.myStr(dr["Remarks"]).Trim() + ".");
                    }

                    sbDrugAllergy.Append(sEnd + "<br/>");
                }
                sbDrugAllergy.Append(EndList);
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["NoAllergyDisplay"] == null)
                {
                    sbDrugAllergy.Append(sbTemplateStyle);
                    if (StaticTemplateDisplayTitle)
                    {
                        sbDrugAllergy.Append("<br/>" + BeginList + sBegin + " No Allergies." + sEnd + EndList + "<br/>");
                    }
                    else
                    {
                        sbDrugAllergy.Append(BeginList + sBegin + "No Allergies." + sEnd + EndList);
                    }

                    System.Web.HttpContext.Current.Session["NoAllergyDisplay"] = false;
                }
            }
        }
        t = 0;
        foreach (DataRowView dr in dvOther)
        {
            if (t == 0)
            {
                //  sbOtherAllergy.Append(sBeginSection + "Food/ Other Allergies <br/>" + sEndSection + BeginList);
                if (StaticTemplateDisplayTitle)
                {
                    if (dvDrug.Count > 0)
                        sbOtherAllergy.Append(sBeginSection + "Food/ Other Allergies <br/>" + sEndSection + BeginList);
                    else
                    {
                        sbOtherAllergy.Append("<br/>" + sBeginSection + "Food/ Other Allergies <br/>" + sEndSection + BeginList); //K.D

                    }
                }
                else
                {
                    sbOtherAllergy.Append(sBeginSection + "Food/ Other Allergies <br/>" + sEndSection + BeginList);
                }

                t = 1;
            }
            sBegin = ""; sEnd = "";
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());

            sbOtherAllergy.Append(" " + sBegin + common.myStr(dr["AllergyName"]).Trim());

            if (common.myLen(dr["AllergyDate"]) > 0)
            {
                sbOtherAllergy.Append(", Onset Date: " + common.myStr(dr["AllergyDate"]));
            }
            if (common.myLen(dr["Reaction"]) > 0)
            {
                sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]).Trim());

                if (!common.myBool(dr["Intolerance"]) && common.myLen(dr["Remarks"]) > 0)
                {
                    sbOtherAllergy.Append(".");
                }
            }
            if (common.myBool(dr["Intolerance"]))
            {
                sbOtherAllergy.Append(", Intolerable");

                if (common.myLen(dr["Remarks"]).Equals(0))
                {
                    sbOtherAllergy.Append(".");
                }
            }

            if (common.myLen(dr["Remarks"]) > 0)
            {
                sbOtherAllergy.Append(", Remarks: " + common.myStr(dr["Remarks"]).Trim() + ".");
            }

            sbOtherAllergy.Append(sEnd + "<br/>");
        }
        sbOtherAllergy.Append(EndList);
        sb.Append(sbDrugAllergy);
        sb.Append(sbOtherAllergy);

        ds.Dispose();
        dsAllergy.Dispose();
        sbDrugAllergy = null;
        sbOtherAllergy = null;
        dvDrug.Dispose();
        dvOther.Dispose();
        emr = null;
        return sb;
    }


    public StringBuilder BindMedication(DataSet dsMedication, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                  Page pg, string pageID, string userID, string GroupingDate, int IndentId, string PrescriptionPrintInTabularFormat,
                  String ConvertToLanguage, bool StaticTemplateDisplayTitle)
    {
        DataView dvMed = new DataView();
        DataView dv = new DataView();
        DataTable dtEnterByItem = new DataTable();
        DataView dvEnterByItem = new DataView();
        int rowindex = 0;
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        StringBuilder sbPrescribed = new StringBuilder();
        LanguageService objLabguageService = new LanguageService();
        //Awadhesh
        string HideInstructionRemarks = common.GetFlagValueHospitalSetup(common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]),
                             common.myInt(System.Web.HttpContext.Current.Session["FacilityId"]), "HideInstructionRemarksForPrintRx", sConString);
        //
        BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
        try
        {
            dvMed = new DataView(dsMedication.Tables[0]);
            if (IndentId > 0)
            {
                dvMed.RowFilter = " IndentId = " + IndentId;
            }

            if (GroupingDate != "")
            {
                dvMed.RowFilter = " GroupDate = '" + GroupingDate + "'";
            }

            dv = new DataView(dvMed.ToTable());
            //dv.RowFilter = "ItemCategoryShortName='MED' AND DoctorCategory=1";
            dv.RowFilter = "DoctorCategory=1";

            dvMed.Dispose();
            dsMedication.Dispose();

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
                        rowindex = rowindex + 1;
                        if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                        {
                            if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                            {
                                sb.Append(BeginTemplateStyle + "Medications" + EndTemplateStyle);
                                sb.Append("<br/>" + sBegin.Replace(" font-weight: bold;", string.Empty) + rowindex + "." + sEnd);
                            }
                            else
                            {
                                sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                                if (!common.myStr(PrescriptionPrintInTabularFormat).Equals("Y"))
                                {
                                    sb.Append("<br/>" + sBegin.Replace(" font-weight: bold;", string.Empty) + rowindex + "." + sEnd);
                                }
                                //Awa
                                else
                                {

                                    // sb.Append("<br/>" + sBegin.Replace(" font-weight: bold;", string.Empty) + rowindex + "." + sEnd);
                                    sb.Append("<br/>" + sBegin.Replace(" font-weight: bold;", string.Empty) + " " + sEnd);
                                    sb.Append("<br/>");
                                }
                                //
                            }
                        }
                        else
                        {
                            sb.Append(sBegin.Replace(" font-weight: bold;", string.Empty) + rowindex + "." + sEnd);
                        }
                    }
                    sBegin = ""; ;
                    sEnd = "";
                    //sb.Append("<br/>" + sBeginSection + "<u><b>" + "Medications Prescribed:<br/>" + "</b></u>" + sEndSection);
                    //sbPrescribed.Append(EndList);
                    MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
                    sBeginSection = sBegin;
                    sEndSection = sEnd;
                    dtEnterByItem = dv.ToTable(true, "EncodedBy");
                    DataTable dtable = new DataTable();

                    if (common.myStr(PrescriptionPrintInTabularFormat).ToUpper().Equals("Y"))
                    {
                        foreach (DataRow row in dtEnterByItem.Rows)
                        {
                            dvEnterByItem = new DataView(dv.ToTable());
                            dvEnterByItem.RowFilter = "EncodedBy=" + row["EncodedBy"].ToString();
                            if (dvEnterByItem.Count > 0)
                            {
                                sbPrescribed.Append("<table border='1' cellpadding='5' cellspacing='2' style='padding:5px;' >");

                                sbPrescribed.Append("<tr>");
                                if (HideInstructionRemarks == "N" || HideInstructionRemarks == "")
                                {
                                    sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Sr No." + sEndSection + "</b></td>");
                                    sbPrescribed.Append("<td colspan='4' border='1' style='text-align: center;'><b>" + sBeginSection + "Drug Names" + sEndSection + "</b></td>");
                                    sbPrescribed.Append("<td colspan='4' border='1' style='text-align: center;'><b>" + sBeginSection + "Prescriptions  Details" + sEndSection + "</b></td>");
                                    sbPrescribed.Append("<td colspan='3' border='1' style='text-align: center;'><b>" + sBeginSection + "Instruction Remarks" + sEndSection + "</b></td>");
                                }
                                else
                                {
                                    // sbPrescribed.Append("<td colspan='4' border='1' style='text-align: center;'><b>" + sBeginSection + "Prescriptions  Details" + sEndSection + "</b></td>");
                                    //yogesh 25/05/22 accord prescription table   //yogesh 20/08/2022
                                    if (System.Web.HttpContext.Current.Session["FacilityName"].ToString().Equals("Accord Superspeciality Hospital"))
                                    {
                                        sbPrescribed.Append("<td border='1' style='text-align: center; width:10px;'><b>" + sBeginSection + "Sr No." + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='3' border='1' style='text-align: center;'><b>" + sBeginSection + "Drug Names" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Dose" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Frequency" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Route" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='2' border='1' style='text-align: center;'><b>" + sBeginSection + "Instruction" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Food Relation" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Duration" + sEndSection + "</b></td>");
                                    }
                                    //yogesh 22/08/2022
                                    else if (System.Web.HttpContext.Current.Session["FacilityName"].ToString().Equals("Alshifa Multispeciality Hospital"))
                                    {
                                        sbPrescribed.Append("<td border='1'><b>" + sBeginSection + "Sr No." + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1'><b>" + sBeginSection + "Drug Names" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='4' border='1'><b>" + sBeginSection + "Dose" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1''><b>" + sBeginSection + "Frequency" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1'><b>" + sBeginSection + "Route" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='2' border='1'><b>" + sBeginSection + "Instruction" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1'><b>" + sBeginSection + "Food Relation" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='1' border='1'><b>" + sBeginSection + "Duration" + sEndSection + "</b></td>");
                                    }
                                    else
                                    {
                                        sbPrescribed.Append("<td colspan='1' border='1' style='text-align: center;'><b>" + sBeginSection + "Sr No." + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='3' border='1' style='text-align: center;'><b>" + sBeginSection + "Drug Names" + sEndSection + "</b></td>");
                                        sbPrescribed.Append("<td colspan='4' border='1' style='text-align: center;'><b>" + sBeginSection + "Prescriptions  Details" + sEndSection + "</b></td>");

                                    }
                                }
                                sbPrescribed.Append("</tr>");

                                for (int rowIdx = 0; rowIdx < dvEnterByItem.Count; rowIdx++)
                                {
                                    string PrescriptionDetails = common.myStr(dvEnterByItem[rowIdx]["PrescriptionDetails"]);
                                    if (common.myStr(System.Web.HttpContext.Current.Session["InOPSummaryMedicationPRNConvertIfRequired"]).Equals("Y"))
                                    {
                                        if (PrescriptionDetails.StartsWith("PRN"))
                                        {
                                            PrescriptionDetails = PrescriptionDetails.Replace("PRN", "If required");
                                        }
                                    }

                                    string genericSmallFontValue = string.Empty;
                                    string generic = common.myStr(dvEnterByItem[rowIdx]["ItemName"]);
                                    string[] gen = generic.Split('(');
                                    if (gen.Count() > 1)
                                    {
                                        string[] genericValue = gen[1].Split(')');
                                        genericSmallFontValue = "(" + genericValue[0] + ")";
                                    }

                                    if (!common.myStr(ConvertToLanguage).ToUpper().Equals("EN") && common.myLen(ConvertToLanguage) > 0)
                                    {

                                        // PrescriptionDetails = LanguageTranslator.TranslateText(PrescriptionDetails, ConvertToLanguage);

                                        PrescriptionDetails = objLabguageService.Language(PrescriptionDetails, "EN", ConvertToLanguage);
                                    }
                                    sbPrescribed.Append("<tr>");
                                    if (HideInstructionRemarks == "N" || HideInstructionRemarks == "")
                                    {
                                        sbPrescribed.Append("<td colspan='1' valign='top' border='1' style='text-align: center;'>" + sBeginSection + common.myStr(rowIdx + 1) + sEndSection + "</td>");
                                        sbPrescribed.Append("<td colspan='4' valign='top' border='1'>" + sBeginSection + gen[0] + sEndSection + "<span style=' font-size:7pt; color: #000000; font-family: Arial; '>" + genericSmallFontValue + "</span>  </td>");
                                        sbPrescribed.Append("<td colspan='4' valign='top' border='1'>" + sBeginSection + PrescriptionDetails + sEndSection + "</td>");
                                        sbPrescribed.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["InstructionRemarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br/>") + sEndSection + "</td>");
                                    }
                                    else
                                    {
                                        int length = PrescriptionDetails.Length;
                                        // Added Condition by Mission_04082022_Akshay
                                        //if (PrescriptionDetails.Contains("-"))
                                        //{
                                        //    PrescriptionDetails = PrescriptionDetails.Remove(PrescriptionDetails.IndexOf('-'));
                                        //}
                                        // 25/05/22
                                        if (System.Web.HttpContext.Current.Session["FacilityName"].ToString().Equals("Accord Superspeciality Hospital"))
                                        {
                                            sbPrescribed.Append("<td valign='top' border='1' style='text-align: center; width:10px;'>" + sBeginSection + common.myStr(rowIdx + 1) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='3' valign='top' border='1'><b>" + sBeginSection + gen[0] + sEndSection + "</b><span style=' font-size:7pt; color: #000000; font-family: Arial; '>" + genericSmallFontValue + "</span>  </td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + PrescriptionDetails + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["Frequency"]) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["RouteName"]) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='2' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["InstructionRemarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br/>") + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["FoodName"]) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["Duration"]) + sEndSection + "</td>");


                                        }
                                        //yogesh 22/08/2022
                                        else if (System.Web.HttpContext.Current.Session["FacilityName"].ToString().Equals("Alshifa Multispeciality Hospital"))
                                        {
                                            sbPrescribed.Append("<td valign='top' border='1'>" + sBeginSection + common.myStr(rowIdx + 1) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'><b>" + sBeginSection + gen[0] + sEndSection + "</b><span style=' font-size:7pt; color: #000000; font-family: Arial; '>" + genericSmallFontValue + "</span>  </td>");
                                            sbPrescribed.Append("<td colspan='4' valign='top' border='1'>" + sBeginSection + PrescriptionDetails + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["Frequency"]) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["RouteName"]) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='2' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["InstructionRemarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br/>") + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["FoodName"]) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1'>" + sBeginSection + common.myStr(dvEnterByItem[rowIdx]["Duration"]) + sEndSection + "</td>");

                                        }
                                        else
                                        {
                                            sbPrescribed.Append("<td colspan='1' valign='top' border='1' style='text-align: center;'>" + sBeginSection + common.myStr(rowIdx + 1) + sEndSection + "</td>");
                                            sbPrescribed.Append("<td colspan='3' valign='top' border='1'>" + sBeginSection + gen[0] + sEndSection + "<span style=' font-size:7pt; color: #000000; font-family: Arial; '>" + genericSmallFontValue + "</span>  </td>");
                                            sbPrescribed.Append("<td colspan='4' valign='top' border='1'>" + sBeginSection + PrescriptionDetails + sEndSection + "</td>");
                                        }

                                    }
                                    sbPrescribed.Append("</tr>");
                                }

                                sbPrescribed.Append("</table>");
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow row in dtEnterByItem.Rows)
                        {
                            dvEnterByItem = new DataView(dv.ToTable());
                            dvEnterByItem.RowFilter = "EncodedBy='" + common.myStr(row["EncodedBy"]) + "' ";
                            foreach (DataRowView dr in dvEnterByItem)
                            {
                                //if(!common.myLen(dr["GenericNameFromItemMaster"]).Equals(0))
                                //{
                                //    sbPrescribed.Append(sBegin + " GenericName : " + common.myStr(dr["GenericNameFromItemMaster"]) + "<br/>");
                                //}

                                if (!common.myLen(dr["ItemName"]).Equals(0))
                                {
                                    sbPrescribed.Append(sBegin + " " + common.myStr(dr["ItemName"]) + " : ");
                                }
                                else
                                {
                                    sbPrescribed.Append(sBegin + " " + common.myStr(dr["GenericName"]) + " : ");
                                }

                                //DataView dvFilter = new DataView(dv1.ToTable());
                                //if (common.myBool(dr["CustomMedication"]) == true)
                                //{
                                //    dvFilter.RowFilter = "ISNULL(DetailsId,0)=" + dr["DetailsId"].ToString() + " AND IndentId =" + dr["IndentId"].ToString();
                                //}
                                //else
                                //{
                                //    dvFilter.RowFilter = "ISNULL(ItemId,0)=" + dr["ItemId"].ToString() + " AND IndentId =" + dr["IndentId"].ToString();
                                //}
                                //string sPrescriptionString = common.myStr(dr["PrescriptionDetails"]);
                                //string sPrescriptionString = objEMR.GetPrescriptionDetailString(dvFilter.ToTable());

                                //sbPrescribed.Append("<br/>" + common.myStr(dr["PrescriptionDetails"]));


                                string PrescriptionDetails = common.myStr(dr["PrescriptionDetails"]);
                                if (common.myStr(System.Web.HttpContext.Current.Session["InOPSummaryMedicationPRNConvertIfRequired"]).Equals("Y"))
                                {
                                    if (PrescriptionDetails.StartsWith("PRN"))
                                    {
                                        PrescriptionDetails = PrescriptionDetails.Replace("PRN", "If required");
                                    }
                                }
                                if (!common.myStr(ConvertToLanguage).ToUpper().Equals("EN") && common.myLen(ConvertToLanguage) > 0)
                                {
                                    //PrescriptionDetails = LanguageTranslator.TranslateText(PrescriptionDetails, ConvertToLanguage);
                                    PrescriptionDetails = objLabguageService.Language(PrescriptionDetails, "En", ConvertToLanguage);
                                }

                                //sbPrescribed.Append("<br/>" + ((rowindex > 9) ? "&nbsp;&nbsp;&nbsp;&nbsp;" : "&nbsp;&nbsp;&nbsp;") + PrescriptionDetails);
                                sbPrescribed.Append("&nbsp;&nbsp;" + PrescriptionDetails);

                                try
                                {
                                    if (common.myLen(dr["InstructionRemarks"]) > 0)
                                    {
                                        //sbPrescribed.Append("<br/>");
                                        //sbPrescribed.Append("&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(dr["InstructionRemarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br/> &nbsp;&nbsp;&nbsp;&nbsp;"));

                                        //sbPrescribed.Append(", Instructions: ");
                                        sbPrescribed.Append(", " + common.myStr(dr["InstructionRemarks"]).Replace("\n", "\r\n").Replace("\r\n", "<br/>"));
                                    }

                                    if (common.myBool(dr["IsInfusion"]))
                                    {
                                        if (common.myStr(dr["InfusionOrder"]).Length > 0)
                                        {

                                            sbPrescribed.Append(", " + "<br/>Infusion Order - " + common.myStr(dr["InfusionOrder"]));
                                        }
                                    }
                                }
                                catch
                                {
                                }
                                rowindex = rowindex + 1;

                                if (rowindex <= dvEnterByItem.ToTable().Rows.Count)
                                {
                                    sbPrescribed.Append(sEnd + "<br/>" + sBegin.Replace(" font-weight: bold;", string.Empty) + rowindex + "." + sEnd);
                                }
                                else
                                {
                                    sbPrescribed.Append(sEnd + "<br/>");
                                }
                                //dvFilter.Dispose();
                            }
                        }

                        sbPrescribed.Append(DisplayUserNameInNote(dvEnterByItem.ToTable().Rows[0]["IndentBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), dvEnterByItem.ToTable().Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(dvEnterByItem.ToTable().Rows[0]["EnteredByid"].ToString())));
                    }

                    sBegin = "";
                    sEnd = "";
                }
            }
            sb.Append(sbPrescribed);
        }
        catch (Exception ex)
        {
        }
        finally
        {
            dv.Dispose();
            dtEnterByItem.Dispose();
            dvEnterByItem.Dispose();
            objEMR = null;
        }
        return sb;
    }

    //bhakti
    public StringBuilder BindOTRequest(DataTable dtOTrequest, StringBuilder sb, StringBuilder sbTemplateStyle,
         DataRow drTemplateListStyle, Page pg, string PageID, int TemplateFieldId, string GroupingDate, bool StaticTemplateDisplayTitle)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        string BeginTemplate = "";
        string EndTemplate = "";
        int rowindex = 0;

        ds = new DataSet();
        DataTable dt = new DataTable();
        StringBuilder sbTemp = new StringBuilder();

        DataView dvOrder = new DataView(dtOTrequest);
        ds.Tables.Add(dvOrder.ToTable());
        dtOTrequest.Dispose();
        dvOrder.Dispose();

        HiddenField hdnEncodedBy = new HiddenField();

        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;

            if (common.myStr(sbTemplateStyle).Contains("OT Request") == false)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                {
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        //sb.Append(sbTemplateStyle + "<u><b>" + "Order And Procedures" + "<br/></b></u>" + BeginList);
                        sb.Append(BeginTemplateStyle + "OT Request" + EndTemplateStyle);
                        sb.Append("<br/>");
                    }
                    else
                    {
                        //sb.Append(BeginTemplateStyle + "OT Request" + EndTemplateStyle);
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");
                        //sb.Append(sbTemplateStyle + "<u><b>" + common.myStr(drTemplateListStyle["StaticTemplateName"]) + "<br/></b></u>" + BeginList);
                    }
                }
            }
            sBeginFont = "";
            sEndFont = "";

            //  sb.Append(sbTemplateStyle);
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
        //int DepartmentId = 0;
        hdnEncodedBy.Value = "0";
        HiddenField hdnSameEncodedBy = new HiddenField();
        HiddenField hdnSameEncodedDate = new HiddenField();
        HiddenField hdnSameEnteredByid = new HiddenField();

        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;
        StringBuilder sbSameName = new StringBuilder();
        StringBuilder sbDiffName = new StringBuilder();
        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        bool bResult = false;

        DataView dvEntryEncodedBy = new DataView(ds.Tables[0]);
        DataTable dtEntryEncodedBy = dvEntryEncodedBy.ToTable(true, "OTRequestID");

        sbTemp.Append(BeginList);
        int sno = 1;
        //sbTemp.Append("OT Request");
        //sbTemp.Append("<br/>");
        for (int i = 0; i < dtEntryEncodedBy.Rows.Count; i++)
        {

            DataView dvEncoded = new DataView(ds.Tables[0]);
            dvEncoded.RowFilter = "OTRequestID=" + common.myInt(dtEntryEncodedBy.Rows[i]["OTRequestID"]);
            DataTable dtEncoded = dvEncoded.ToTable();

            if (ds.Tables[0].Rows.Count > 0)
            {
                sbTemp.Append(sBeginSection + sno + "   " + common.myStr(ds.Tables[0].Rows[i]["ServiceName"]) + " On " + common.myStr(ds.Tables[0].Rows[i]["OTBookingDate"]) + " " + common.myStr(ds.Tables[0].Rows[i]["FromTime"]) + " By " + common.myStr(ds.Tables[0].Rows[i]["FirstName"]) + "<br/>" + sEndSection);
                sbTemp.Append(BeginList);
                sno++;
            }
            dtEncoded.Dispose();
        }

        dvEntryEncodedBy.Dispose();
        dtEntryEncodedBy.Dispose();
        DataTable dtEx = ds.Tables[0];
        sb.Append(sbTemp.ToString());
        dtEx.Dispose();
        ds.Dispose();
        dt.Dispose();
        sbTemp = null;
        return sb;
    }



    public StringBuilder BindOrders(DataTable dtOrder, string DocId,
                            StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, int EncounterId, string GroupingDate, string EMRServicePrintSeperatedWithCommas,
                            bool ShowLabDepartmentName, bool StaticTemplateDisplayTitle)
    {
        string BeginList = "", EndList = "", sBegin = "", sEnd = "", sBeginFont = "", sEndFont = "", sBeginSection = "", sEndSection = "";
        string BeginTemplate = "";
        string EndTemplate = "";
        int rowindex = 0;

        ds = new DataSet();
        DataTable dt = new DataTable();
        StringBuilder sbTemp = new StringBuilder();

        DataView dvOrder = new DataView(dtOrder);
        if (GroupingDate != "")
        {
            dvOrder.RowFilter = " GroupDate = '" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvOrder.RowFilter = " EncounterId=" + EncounterId;
        }
        ds.Tables.Add(dvOrder.ToTable());
        dtOrder.Dispose();
        dvOrder.Dispose();

        HiddenField hdnEncodedBy = new HiddenField();

        if (ds.Tables[0].Rows.Count > 0)
        {
            MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            string BeginTemplateStyle = sBeginFont;
            string EndTemplateStyle = sEndFont;

            if (common.myStr(sbTemplateStyle).Contains("Order") == false)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
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

            //  sb.Append(sbTemplateStyle);
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
        HiddenField hdnSameEnteredByid = new HiddenField();

        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;
        StringBuilder sbSameName = new StringBuilder();
        StringBuilder sbDiffName = new StringBuilder();
        sBegin = ""; sEnd = "";
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, System.Web.HttpContext.Current.Session["HospitalLocationId"].ToString());
        bool bResult = false;

        DataView dvEntryEncodedBy = new DataView(ds.Tables[0]);
        DataTable dtEntryEncodedBy = dvEntryEncodedBy.ToTable(true, "OrderId");

        /**********************************OP ******************/
        //dvFilter = new DataView(dtEncoded);
        //dvFilter.RowFilter = "DepartmentId=" + common.myInt(dtEntryDepartmentId.Rows[0]["DepartmentId"]);
        //dt = dvFilter.ToTable();

        //if (!common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I") && ShowLabDepartmentName)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        sbTemp.Append(sBeginSection + common.myStr(ds.Tables[0].Rows[0]["DepartmentName"]) + "<br/>" + sEndSection);
        //    }
        //}
        /**********************************OP ******************/

        sbTemp.Append(BeginList);


        if (System.Web.HttpContext.Current.Session["FacilityName"].ToString().Equals("Accord Superspeciality Hospital"))
        {
            //for (int i = 0; i < dtEntryEncodedBy.Rows.Count; i++)
            //{
            DataView dvEncoded = new DataView(ds.Tables[0]);
            //dvEncoded.RowFilter = "OrderId=" + common.myInt(dtEntryEncodedBy.Rows[i]["OrderId"]);
            DataTable dtEncoded = dvEncoded.ToTable();
            //if (dtEncoded.Rows.Count == ds.Tables[0].Rows.Count)
            //{
            //    bResult = true;
            //    hdnSameEncodedBy.Value = ds.Tables[0].Rows[i]["EnteredBy"].ToString();
            //    hdnSameEncodedDate.Value = ds.Tables[0].Rows[i]["EncodedDate"].ToString();
            //    hdnSameEnteredByid.Value = ds.Tables[0].Rows[i]["EnteredByid"].ToString();
            //}
            //DataView dvEntryDepartmentId = new DataView(dtEncoded);
            //DataTable dtEntryDepartmentId = dvEntryDepartmentId.ToTable(true, "DepartmentId");
            DataView dvFilter = new DataView();


            //for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
            //{
            dvFilter = new DataView(dtEncoded);
            // dvFilter.RowFilter = "DepartmentId=" + common.myInt(dtEntryDepartmentId.Rows[k]["DepartmentId"]);
            dt = dvFilter.ToTable();
            if (dt.Rows.Count > 0)
            {
                if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I") && StaticTemplateDisplayTitle)
                {
                    sbTemp.Append(sBeginSection + common.myStr(dt.Rows[0]["DepartmentName"]) + "<br/>" + sEndSection);
                    sbTemp.Append(BeginList);
                }
                if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
                {
                    DataTable uniqueServiceDuration = new DataTable();
                    DataView dvServiceDuration = new DataView();
                    uniqueServiceDuration = dt.DefaultView.ToTable(true, "ServiceDuration");

                    for (int uRowIdx = 0; uRowIdx < uniqueServiceDuration.Rows.Count; uRowIdx++)
                    {
                        string ServiceDuration = common.myStr(uniqueServiceDuration.Rows[uRowIdx]["ServiceDuration"]);

                        dvServiceDuration = dt.DefaultView;
                        dvServiceDuration.RowFilter = "ISNULL(ServiceDuration,'')='" + ServiceDuration + "'";

                        StringBuilder sbServices = new StringBuilder();
                        rowindex = 0;
                        foreach (DataRow dr1 in dvServiceDuration.ToTable().Rows)
                        {
                            string sRemarks = (common.myLen(dr1["Remarks"]) > 0) ? " Remarks: " + common.myStr(dr1["Remarks"]).Trim() : string.Empty;

                            rowindex = rowindex + 1;

                            if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
                            {
                                if (dr1["CPTCode"].ToString() != "")
                                {
                                    if (rowindex.Equals(1))
                                    {
                                        sbServices.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                    }
                                    else
                                    {
                                        sbServices.Append(sBegin + ", " + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                    }
                                }
                                else
                                {
                                    sbServices.Append(sBegin + rowindex + "." + sEnd);
                                    sbServices.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                                }
                            }
                            else
                            {
                                sbServices.Append(sBegin + rowindex + "." + sEnd);
                                sbServices.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                            }

                            if (common.myStr(dr1["InvestigationDate"]) != "")
                            {
                                sbServices.Append(sBegin + "<b>Investigation Date:</b> " + common.myDate(dr1["InvestigationDate"]).ToString("dd/MM/yyy HH:mm tt") + "<br/>" + sEnd);
                            }
                        }


                        if (sbServices.ToString() != string.Empty)
                        {
                            sbTemp.Append(sbServices.ToString());
                        }
                        if (common.myLen(ServiceDuration) > 0)
                        {
                            sbTemp.Append("-" + sBegin + common.myStr(ServiceDuration).Trim() + sEnd);
                        }
                        sbTemp.Append("<br/>");
                    }
                }
                else
                {
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        DataRow dr1 = dt.Rows[j] as DataRow;

                        string sRemarks = (common.myLen(dr1["Remarks"]) > 0) ? " Remarks: " + common.myStr(dr1["Remarks"]).Trim() : string.Empty;

                        rowindex = rowindex + 1;

                        if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
                        {
                            if (dr1["CPTCode"].ToString() != "")
                            {
                                if (rowindex.Equals(1))
                                {
                                    sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                }
                                else
                                {
                                    sbTemp.Append(sBegin + ", " + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                }
                            }
                            else
                            {
                                sbTemp.Append(sBegin + rowindex + "." + sEnd);
                                sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                            }
                        }
                        else
                        {
                            sbTemp.Append(sBegin + rowindex + "." + sEnd);
                            sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                        }

                        if (common.myStr(dr1["InvestigationDate"]) != "")
                        {
                            sbTemp.Append(sBegin + "<b>Investigation Date:</b> " + common.myDate(dr1["InvestigationDate"]).ToString("dd/MM/yyy HH:mm tt") + "<br/>" + sEnd);
                        }
                    }
                }

                //sbTemp.Append("<br/>");
                if (bResult == false)
                {
                    if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I"))
                    {
                        sbTemp.Append(EndList + "");
                        sbTemp.Append(DisplayUserNameInNote(dtEncoded.Rows[0]["EnteredBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myStr(dtEncoded.Rows[0]["EncodedDate"]), drTemplateListStyle, pg, common.myInt(dtEncoded.Rows[0]["EnteredById"])));

                    }
                }
            }
            dtEncoded.Dispose();
            //dvEntryDepartmentId.Dispose();
            //dtEntryDepartmentId.Dispose();
            dvFilter.Dispose();
            //}
            //}
        }
        else
        {
            for (int i = 0; i < dtEntryEncodedBy.Rows.Count; i++)
            {
                DataView dvEncoded = new DataView(ds.Tables[0]);
                dvEncoded.RowFilter = "OrderId=" + common.myInt(dtEntryEncodedBy.Rows[i]["OrderId"]);
                DataTable dtEncoded = dvEncoded.ToTable();
                if (dtEncoded.Rows.Count == ds.Tables[0].Rows.Count)
                {
                    bResult = true;
                    hdnSameEncodedBy.Value = ds.Tables[0].Rows[i]["EnteredBy"].ToString();
                    hdnSameEncodedDate.Value = ds.Tables[0].Rows[i]["EncodedDate"].ToString();
                    hdnSameEnteredByid.Value = ds.Tables[0].Rows[i]["EnteredByid"].ToString();
                }
                DataView dvEntryDepartmentId = new DataView(dtEncoded);
                DataTable dtEntryDepartmentId = dvEntryDepartmentId.ToTable(true, "DepartmentId");
                DataView dvFilter = new DataView();


                for (int k = 0; k < dtEntryDepartmentId.Rows.Count; k++)
                {
                    dvFilter = new DataView(dtEncoded);
                    dvFilter.RowFilter = "DepartmentId=" + common.myInt(dtEntryDepartmentId.Rows[k]["DepartmentId"]);
                    dt = dvFilter.ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I") && StaticTemplateDisplayTitle)
                        {
                            sbTemp.Append(sBeginSection + common.myStr(dt.Rows[0]["DepartmentName"]) + "<br/>" + sEndSection);
                            sbTemp.Append(BeginList);
                        }
                        if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
                        {
                            DataTable uniqueServiceDuration = new DataTable();
                            DataView dvServiceDuration = new DataView();
                            uniqueServiceDuration = dt.DefaultView.ToTable(true, "ServiceDuration");

                            for (int uRowIdx = 0; uRowIdx < uniqueServiceDuration.Rows.Count; uRowIdx++)
                            {
                                string ServiceDuration = common.myStr(uniqueServiceDuration.Rows[uRowIdx]["ServiceDuration"]);

                                dvServiceDuration = dt.DefaultView;
                                dvServiceDuration.RowFilter = "ISNULL(ServiceDuration,'')='" + ServiceDuration + "'";

                                StringBuilder sbServices = new StringBuilder();
                                rowindex = 0;
                                foreach (DataRow dr1 in dvServiceDuration.ToTable().Rows)
                                {
                                    string sRemarks = (common.myLen(dr1["Remarks"]) > 0) ? " Remarks: " + common.myStr(dr1["Remarks"]).Trim() : string.Empty;

                                    rowindex = rowindex + 1;

                                    if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
                                    {
                                        if (dr1["CPTCode"].ToString() != "")
                                        {
                                            if (rowindex.Equals(1))
                                            {
                                                sbServices.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                            }
                                            else
                                            {
                                                sbServices.Append(sBegin + ", " + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                            }
                                        }
                                        else
                                        {
                                            sbServices.Append(sBegin + rowindex + "." + sEnd);
                                            sbServices.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                                        }
                                    }
                                    else
                                    {
                                        sbServices.Append(sBegin + rowindex + "." + sEnd);
                                        sbServices.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                                    }

                                    if (common.myStr(dr1["InvestigationDate"]) != "")
                                    {
                                        sbServices.Append(sBegin + "<b>Investigation Date:</b> " + common.myDate(dr1["InvestigationDate"]).ToString("dd/MM/yyy HH:mm tt") + "<br/>" + sEnd);
                                    }
                                }


                                if (sbServices.ToString() != string.Empty)
                                {
                                    sbTemp.Append(sbServices.ToString());
                                }
                                if (common.myLen(ServiceDuration) > 0)
                                {
                                    sbTemp.Append("-" + sBegin + common.myStr(ServiceDuration).Trim() + sEnd);
                                }
                                sbTemp.Append("<br/>");
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                DataRow dr1 = dt.Rows[j] as DataRow;

                                string sRemarks = (common.myLen(dr1["Remarks"]) > 0) ? " Remarks: " + common.myStr(dr1["Remarks"]).Trim() : string.Empty;

                                rowindex = rowindex + 1;

                                if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
                                {
                                    if (dr1["CPTCode"].ToString() != "")
                                    {
                                        if (rowindex.Equals(1))
                                        {
                                            sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                        }
                                        else
                                        {
                                            sbTemp.Append(sBegin + ", " + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + sEnd);
                                        }
                                    }
                                    else
                                    {
                                        sbTemp.Append(sBegin + rowindex + "." + sEnd);
                                        sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                                    }
                                }
                                else
                                {
                                    sbTemp.Append(sBegin + rowindex + "." + sEnd);
                                    sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]).Trim() + sRemarks + "<br/>" + sEnd);
                                }

                                if (common.myStr(dr1["InvestigationDate"]) != "")
                                {
                                    sbTemp.Append(sBegin + "<b>Investigation Date:</b> " + common.myDate(dr1["InvestigationDate"]).ToString("dd/MM/yyy HH:mm tt") + "<br/>" + sEnd);
                                }
                            }
                        }

                        //sbTemp.Append("<br/>");
                        if (bResult == false)
                        {
                            if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I"))
                            {
                                sbTemp.Append(EndList + "");
                                sbTemp.Append(DisplayUserNameInNote(dtEncoded.Rows[0]["EnteredBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), common.myStr(dtEncoded.Rows[0]["EncodedDate"]), drTemplateListStyle, pg, common.myInt(dtEncoded.Rows[0]["EnteredById"])));

                            }
                        }
                    }
                    dtEncoded.Dispose();
                    dvEntryDepartmentId.Dispose();
                    dtEntryDepartmentId.Dispose();
                    dvFilter.Dispose();
                }
            }
        }



        if (EMRServicePrintSeperatedWithCommas.ToUpper().Equals("Y"))
        {
            if (sbTemp.ToString() != string.Empty && !sbTemp.ToString().EndsWith("<br/>"))
            {
                sbTemp.Append("<br/>");
            }
        }

        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                /**************IP START*****************/
                //if (bResult == false)
                //{
                //    if (common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I"))
                //    {
                //        sbTemp.Append(EndList + "");
                //        sbTemp.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EnteredBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg));

                //    }
                //}

                /**************IP END *****************/
                /**************OP START*****************/
                if (bResult == false)
                {
                    if (!common.myStr(System.Web.HttpContext.Current.Session["OPIP"]).Equals("I"))
                    {
                        sbTemp.Append(EndList + "");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sbTemp.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EnteredBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(ds.Tables[0].Rows[0]["EnteredByid"].ToString())));
                        }
                    }
                }
                /**************OP END*****************/
            }
        }
        if (bResult == true)
        {
            sbTemp.Append(EndList + "");
            sbTemp.Append(DisplayUserNameInNote(hdnSameEncodedBy.Value, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), hdnSameEncodedDate.Value, drTemplateListStyle, pg, common.myInt(hdnSameEnteredByid.Value)));
        }
        dvEntryEncodedBy.Dispose();
        dtEntryEncodedBy.Dispose();

        DataTable dtEx = ds.Tables[0];
        DataView dvExcluded = new DataView(dtEx);
        dvExcluded.RowFilter = "ExcludedServices=1";
        DataTable dtExcluded = dvExcluded.ToTable();
        if (dtExcluded.Rows.Count > 0)
        {
            sbTemp.Append("<br/>");
            sbTemp.Append("<strong>Approval required for following services :</strong><br/>");
            for (int j = 0; j < dtExcluded.Rows.Count; j++)
            {
                string commase = "";
                if (j != dtExcluded.Rows.Count - 1)
                {
                    commase = ", ";
                }

                DataRow dr1 = dtExcluded.Rows[j] as DataRow;

                rowindex = rowindex + 1;
                sb.Append("<br/>" + sBegin + rowindex + "." + sEnd);

                sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);
            }
        }
        sb.Append(sbTemp.ToString());
        dtEx.Dispose();
        dvExcluded.Dispose();
        dtExcluded.Dispose();
        ds.Dispose();
        dt.Dispose();
        sbTemp = null;
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

            //sbTemp.Append("<br/>" + sBeginSection + dr["DepartmentName"].ToString() + sEndSection);
            sbTemp.Append(BeginList);

            sbTemp.Append("<br/>" + sBegin + common.myStr(dr["FieldName"]) + sEnd);
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

    public StringBuilder BindReferalHistory(DataTable dtReferralHistory, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, int EncounterId, string GroupingDate)
    {
        ds = new DataSet();
        StringBuilder sbTemp = new StringBuilder();
        StringBuilder referal = new StringBuilder();

        DataView dvReferralHistory = new DataView(dtReferralHistory);
        if (GroupingDate != "")
        {
            dvReferralHistory.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvReferralHistory.RowFilter = "EncounterId=" + EncounterId;
        }

        ds.Tables.Add(dvReferralHistory.ToTable());
        dvReferralHistory.Dispose();
        dtReferralHistory.Dispose();

        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        sBeginSection = sBegin;
        sEndSection = sEnd;
        int t = 0;
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
            referal.Append("<span style='font-size:10pt;color:#000000;font-family:Arial;font-weight:bold;margin-bottom:15px'>REFERRAL ADVICE</span><br/>");

            int ReferralToDoctorId = 0;

            DataView dv1 = new DataView(ds.Tables[0]);
            DataTable dt = dv1.ToTable(true, "ReferralToDoctorId");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataView dv = new DataView(ds.Tables[0]);
                dv.RowFilter = "ReferralToDoctorId=" + common.myInt(dt.Rows[i]["ReferralToDoctorId"]);// +" AND ReferralFromDoctorId=" + common.myStr(dt.Rows[i]["ReferralFromDoctorId"]);
                if (common.myInt(ds.Tables[0].Rows[i]["ReferralToDoctorId"]) != ReferralToDoctorId)
                {
                    ReferralToDoctorId = common.myInt(dt.Rows[i]["ReferralToDoctorId"]);
                    //referal.Append("<br/>Referral Doctor Name : " + common.myStr(dv.ToTable().Rows[0]["FromDoctorName"]) + " Refer to Doctor Name :" + common.myStr(dv.ToTable().Rows[0]["DoctorName"]) + "");
                    referal.Append("<br/>Refer By : <b>" + common.myStr(dv.ToTable().Rows[0]["FromDoctorName"]) + "</b> Refer To : <b>" + common.myStr(dv.ToTable().Rows[0]["DoctorName"]) + "</b>");

                    referal.Append("<br/><table cellspacing='2px' border='1' width='95%' style='border-style:solid; border-width:2px;font-size:12px;' >");
                    referal.Append("<tr style='border-style:solid; border-width:2px;font-size:12px;'>");
                    referal.Append("<td style='font-weight:bold;' valign='top'>Referral Date</td>");
                    referal.Append("<td style='font-weight:bold;' valign='top'>Reason for Referral</td>");
                    referal.Append("<td style='font-weight:bold;' valign='top'>Referral Reply</td><td style='font-weight:bold;' valign='top'>Referral Replied By</td><td style='font-weight:bold;' valign='top'>Referral Reply Date</td>");
                    referal.Append("</tr>");

                    for (int j = 0; j < dv.ToTable().Rows.Count; j++)
                    {
                        referal.Append("<tr style='border-style:solid; border-width:2px;font-size:12px;'>");
                        referal.Append("<td valign='top'>" + dv.ToTable().Rows[j]["ReferralDate"].ToString() + "</td>");
                        referal.Append("<td valign='top'>" + dv.ToTable().Rows[j]["Note"].ToString() + "</td>");
                        referal.Append("<td valign='top'>" + dv.ToTable().Rows[j]["DoctorRemark"].ToString() + "</td>");
                        referal.Append("<td valign='top'>" + dv.ToTable().Rows[j]["RepliedBy"].ToString() + "</td>");
                        referal.Append("<td valign='top'>" + dv.ToTable().Rows[j]["BeforeFinalizedDate"].ToString() + "</td>");
                        referal.Append("</tr>");
                    }
                    referal.Append("</table>");
                }
                dv.Dispose();
            }
            dv1.Dispose();

            dt.Dispose();
            referal.Append(EndList);
        }
        sb.Append(referal);
        ds.Dispose();
        sbTemp = null;
        referal = null;
        return sb;
    }

    public StringBuilder BindNormalOPReferalHistory(DataTable dtReferralHistory, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, int EncounterId, string GroupingDate)
    {
        ds = new DataSet();
        StringBuilder sbTemp = new StringBuilder();
        StringBuilder referal = new StringBuilder();

        DataView dvReferralHistory = new DataView(dtReferralHistory);
        if (GroupingDate != "")
        {
            dvReferralHistory.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvReferralHistory.RowFilter = "EncounterId=" + EncounterId;
        }

        ds.Tables.Add(dvReferralHistory.ToTable());
        dvReferralHistory.Dispose();
        dtReferralHistory.Dispose();

        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

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
            referal.Append("<span style='font-size:10pt;color:#000000;font-family:Arial;font-weight:bold;margin-bottom:15px'>REFERRAL ADVICE</span><br/>");

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string remarks = common.myStr(ds.Tables[0].Rows[i]["DoctorRemark"]) == "" ? common.myStr(ds.Tables[0].Rows[i]["Note"]) : common.myStr(ds.Tables[0].Rows[i]["DoctorRemark"]);

                referal.Append("Reffer To : " + common.myStr(ds.Tables[0].Rows[i]["DoctorName"]) + " (" + common.myStr(ds.Tables[0].Rows[i]["SpecialisationName"]) + ")<br/>");
                referal.Append("Reason : " + remarks + "<br/>");
            }
            referal.Append(EndList);
        }
        sb.Append(referal);
        ds.Dispose();
        sbTemp = null;
        referal = null;
        return sb;
    }

    public StringBuilder BindNonDrugOrder(DataTable dtNonDrugOrder, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                           Page pg, string pageID, int EncounterId, string GroupingDate, bool StaticTemplateDisplayTitle)
    {
        ds = new DataSet();
        StringBuilder sbTemp = new StringBuilder();
        try
        {
            DataView dvNonDrugOrder = new DataView(dtNonDrugOrder);
            if (GroupingDate != "")
            {
                dvNonDrugOrder.RowFilter = "GroupDate='" + GroupingDate + "' AND EncounterId=" + EncounterId;
            }
            else
            {
                dvNonDrugOrder.RowFilter = "EncounterId=" + EncounterId;
            }
            ds.Tables.Add(dvNonDrugOrder.ToTable());
            dtNonDrugOrder.Dispose();
            dvNonDrugOrder.Dispose();

            string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

            sBeginSection = sBegin;
            sEndSection = sEnd;
            if (ds.Tables[0].Rows.Count > 0)
            {
                MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                //  MakeFont("Template", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                string BeginTemplateStyle = sBeginFont;
                string EndTemplateStyle = sEndFont;

                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]) && StaticTemplateDisplayTitle)
                {
                    if (common.myStr(drTemplateListStyle["StaticTemplateName"]) == null || common.myStr(drTemplateListStyle["StaticTemplateName"]) == " ")
                    {
                        sb.Append(BeginTemplateStyle + "Non Drug Order" + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                    else
                    {
                        sb.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        sb.Append("<br/>");

                    }
                }
                sBeginFont = "";
                sEndFont = "";
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
                sb.Append(BeginList);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sbTemp.Append(ds.Tables[0].Rows[i]["Prescription"].ToString());
                    sbTemp.Append(DisplayUserNameInNote(ds.Tables[0].Rows[i]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[i]["EncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(ds.Tables[0].Rows[i]["EnteredByid"].ToString())));
                }
            }
            sb.Append(sbTemp);
        }
        catch (Exception)
        {
        }
        finally
        {
            ds.Dispose();
            sbTemp = null;
        }

        return sb;
    }

    public StringBuilder BindPatientBooking(DataTable dtPatientBooking, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageId, string GroupingDate)
    {
        StringBuilder referal = new StringBuilder();
        ds = new DataSet();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        DataView dvPatientBooking = new DataView(dtPatientBooking);
        string BeginTemplateStyle = string.Empty;
        string EndTemplateStyle = string.Empty;
        try
        {
            if (GroupingDate != "")
            {
                dvPatientBooking.RowFilter = "GroupDate='" + GroupingDate + "'";
            }
            ds.Tables.Add(dvPatientBooking.ToTable());
            sBeginSection = sBegin;
            sEndSection = sEnd;
            if (ds.Tables[0].Rows.Count > 0)
            {
                sb.Append(sbTemplateStyle);
                if (drTemplateListStyle != null)
                {
                    MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

                    BeginTemplateStyle = sBeginFont;
                    EndTemplateStyle = sEndFont;

                    sBeginFont = string.Empty;
                    sEndFont = string.Empty;

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

                if (drTemplateListStyle != null)
                {
                    if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]))
                    {
                        if (common.myLen(drTemplateListStyle["StaticTemplateName"]) > 0)
                        {
                            referal.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                        }
                        else
                        {
                            referal.Append(BeginTemplateStyle + "ADMISSION ADVICE" + EndTemplateStyle);
                        }
                    }
                }
                else
                {
                    referal.Append(BeginTemplateStyle + "ADMISSION ADVICE" + EndTemplateStyle);
                }

                if (common.myStr(ds.Tables[0].Rows[0]["VisitType"]).Equals("I"))
                {
                    referal.Append("<br/><br/><table cellpadding='4' cellspacing='0' border='1' width='55%'>");
                    referal.Append("<tr>");
                    referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Date of Request" + sEndFont + "</td>");
                    referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["BookingDate"]) + sEndFont + "</td>");
                    referal.Append("</tr>");

                    referal.Append("<tr>");
                    referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Expected Date of Admission" + sEndFont + "</td>");
                    referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["ExpectedAdmissionDate"]) + sEndFont + "</td>");
                    referal.Append("</tr>");

                    referal.Append("<tr>");
                    referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Consultant Name" + sEndFont + "</td>");
                    referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["DoctorName"]) + sEndFont + "</td>");
                    referal.Append("</tr>");

                    referal.Append("<tr>");
                    referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Department Name" + sEndFont + "</td>");
                    referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["DEPARTMENTNAME"]) + sEndFont + "</td>");
                    referal.Append("</tr>");

                    if (common.myInt(ds.Tables[0].Rows[0]["ProbableStayInDays"]) > 0)
                    {
                        referal.Append("<tr>");
                        referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Probable LOS (Days)" + sEndFont + "</td>");
                        referal.Append("<td valign='top'>" + sBeginFont + common.myInt(ds.Tables[0].Rows[0]["ProbableStayInDays"]).ToString() + sEndFont + "</td>");
                        referal.Append("</tr>");
                    }

                    referal.Append("<tr>");
                    referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Booking Type" + sEndFont + "</td>");
                    referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["BookingTypeName"]) + sEndFont + "</td>");
                    referal.Append("</tr>");

                    referal.Append("<tr>");
                    referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Reason for Admission" + sEndFont + "</td>");
                    referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["ReasonForAdmission"]) + sEndFont + "</td>");
                    referal.Append("</tr>");

                    if (common.myLen(dtPatientBooking.Rows[0]["BookingRemarks"]) > 0)
                    {
                        referal.Append("<tr>");
                        referal.Append("<td style='font-weight:bold;' valign='top' width='23%'>" + sBeginFont + "Remarks" + sEndFont + "</td>");
                        referal.Append("<td valign='top'>" + sBeginFont + common.myStr(ds.Tables[0].Rows[0]["BookingRemarks"]) + sEndFont + "</td>");
                        referal.Append("</tr>");
                    }

                    referal.Append("</table>");
                }
                else
                {
                    referal.Append("<br/>" + sBeginFont + "Adviced admission on " + common.myStr(ds.Tables[0].Rows[0]["ExpectedAdmissionDate"]) + " for " + common.myStr(ds.Tables[0].Rows[0]["BookingTypeName"]).ToLower() + "." + sEndFont);
                    referal.Append("<br/>" + sBeginFont + "Reason for Admission - " + common.myStr(ds.Tables[0].Rows[0]["ReasonForAdmission"]) + sEndFont);

                }

                referal.Append("<br/>");

                referal.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg, common.myInt(ds.Tables[0].Rows[0]["EnteredById"].ToString())));
                referal.Append(EndList);
            }
            sb.Append(referal);
            referal = null;
            dtPatientBooking.Dispose();
            ds.Dispose();
        }
        catch (Exception Ex)
        {
        }
        finally
        {
        }

        return sb;
    }


    public StringBuilder BindPatientVisitRequiredDetails(DataTable dtPatientBooking, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                        Page pg, string pageId, string GroupingDate, string IsSurgeryRequired, string IsAdmissionRequired)
    {
        StringBuilder referal = new StringBuilder();
        ds = new DataSet();
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";

        DataView dvPatientBooking = new DataView(dtPatientBooking);
        string BeginTemplateStyle = string.Empty;
        string EndTemplateStyle = string.Empty;
        try
        {
            //if (GroupingDate != "")
            //{
            //    dvPatientBooking.RowFilter = "GroupDate='" + GroupingDate + "'";
            //}
            //if (dvPatientBooking.ToTable() != null)
            //{
            //    ds.Tables.Add(dvPatientBooking.ToTable());
            //}
            sBeginSection = sBegin;
            sEndSection = sEnd;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Template", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

                BeginTemplateStyle = sBeginFont;
                EndTemplateStyle = sEndFont;

                sBeginFont = string.Empty;
                sEndFont = string.Empty;

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

            if (drTemplateListStyle != null)
            {
                if (common.myBool(drTemplateListStyle["TemplateDisplayTitle"]))
                {
                    if (common.myLen(drTemplateListStyle["StaticTemplateName"]) > 0)
                    {
                        //referal.Append(BeginTemplateStyle + common.myStr(drTemplateListStyle["StaticTemplateName"]) + EndTemplateStyle);
                    }
                    else
                    {
                        //referal.Append(BeginTemplateStyle + "Visit Required Details" + EndTemplateStyle);
                    }
                }
            }
            else
            {
                //referal.Append(BeginTemplateStyle + "Visit Required Details" + EndTemplateStyle);
            }

            if (common.myLen(IsSurgeryRequired) > 0)
            {
                referal.Append("<br/>" + sBeginFont + "Surgery Required: " + IsSurgeryRequired + sEndFont);
            }
            if (common.myLen(IsAdmissionRequired) > 0)
            {
                referal.Append("<br/>" + sBeginFont + "Admission Required: " + IsAdmissionRequired + sEndFont);
            }

            referal.Append("<br/>");

            //referal.Append(DisplayUserNameInNote(ds.Tables[0].Rows[0]["EncodedBy"].ToString(), common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]), ds.Tables[0].Rows[0]["EncodedDate"].ToString(), drTemplateListStyle, pg));
            referal.Append(EndList);
            //}
            sb.Append(referal);
            referal = null;
            //dtPatientBooking.Dispose();
            //ds.Dispose();
        }
        catch (Exception Ex)
        {
        }
        finally
        {
        }

        return sb;
    }

    public StringBuilder BindDietOrderInNote(DataTable dtDietOrder, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                            Page pg, string pageID, string GroupingDate, int EncounterId)
    {
        ds = new DataSet();
        StringBuilder DietOrder = new StringBuilder();

        DataView dvDietOrder = new DataView(dtDietOrder);
        if (GroupingDate != "")
        {
            dvDietOrder.RowFilter = " GroupDate = '" + GroupingDate + "' AND EncounterId=" + EncounterId;
        }
        else
        {
            dvDietOrder.RowFilter = "EncounterId=" + EncounterId;
        }
        ds.Tables.Add(dvDietOrder.ToTable());
        dtDietOrder.Dispose();
        dvDietOrder.Dispose();

        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        sBeginSection = sBegin;
        sEndSection = sEnd;
        if (ds.Tables[0].Rows.Count > 0)
        {
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationid"]));

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

            //DietOrder.Append("<U><B>Diet Order</B></U><br/><br/>");//Ritika(14-09-2022)
            //DietOrder.Append("<B>Diet Order</B><br/><br/>");
            int iCountColumn = 0;
            DietOrder.Append("<table border='1' style='font-size:12px;' cellspacing='2px' width='100%' >");
            DietOrder.Append("<tr>");
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                if (ds.Tables[0].Columns[i].ColumnName.ToString() != "Remarks" && ds.Tables[0].Columns[i].ColumnName.ToString() != "EncounterId"
                    && ds.Tables[0].Columns[i].ColumnName.ToString() != "GroupDate")
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
                    if (ds.Tables[0].Columns[j].ColumnName.ToString() != "Remarks" && ds.Tables[0].Columns[j].ColumnName.ToString() != "EncounterId"
                        && ds.Tables[0].Columns[j].ColumnName.ToString() != "GroupDate")
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
        DietOrder = null;
        return sb;
    }

    public StringBuilder bindDrugAdministration(DataTable dtData, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle,
                                       Page pg, string pageID, int TemplateFieldId, string GroupingDate, int EncounterId)
    {
        DataSet ds = new DataSet();
        StringBuilder sbData = new StringBuilder();
        try
        {
            DataView DV = new DataView(dtData);
            if (GroupingDate != string.Empty)
            {
                DV.RowFilter = "GroupDate='" + common.myDate(GroupingDate).ToString("yyyy/MM/dd") + "' AND EncounterId=" + EncounterId;
            }
            else
            {
                DV.RowFilter = "EncounterId=" + EncounterId;
            }
            ds.Tables.Add(DV.ToTable());
            string sBegin = string.Empty;
            string sEnd = string.Empty;
            string sBeginSection = string.Empty;
            string sEndSection = string.Empty;
            sBeginSection = sBegin;
            sEndSection = sEnd;
            sbData = new StringBuilder();
            if (ds.Tables[0].Rows.Count > 0)
            {
                sbData.Append("<br/><u><b>" + sb + "Drug Administration:  " + "</b></u><br/><br/>");
                sbData.Append("<table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; font-size:11pt; color: #000000; font-family: Tahoma;' cellspacing='3' cellpadding='2'>");
                sbData.Append("<tr>");
                //sbData.Append("<td style='font-weight:bold;' valign='top' align='center'>" + sBegin + "Date" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center'  style='border: solid 1px black;'>" + sBegin + "Order Date" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center'  style='border: solid 1px black;'>" + sBegin + "Drug Date" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center'  style='border: solid 1px black;'>" + sBegin + "Drug Name" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Dose" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Route" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Frequency" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Scheduled Time" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Given Time" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Delay Reason" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Remarks" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Administered By" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Witness Name" + sEnd + "</td>");
                sbData.Append("<td valign='top' align='center' style='border: solid 1px black;'>" + sBegin + "Reason for Not administered" + sEnd + " </td>");
                sbData.Append("</tr>");
                foreach (DataRow DR in ds.Tables[0].Rows)
                {
                    sbData.Append("<tr>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["IndentDate"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["DrugDate"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["ItemName"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["Dose"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["Route"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["Frequency"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["ScheduledTime"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["GivenTime"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["DelayReason"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["Remarks"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["AdministerBy"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["WitnessName"]) + sEnd + "</td>");
                    sbData.Append("<td valign='top' style='border: solid 1px black;'>" + sBegin + common.myStr(DR["NotAdministeredReason"]) + sEnd + "</td>");
                    sbData.Append("</tr>");
                }
                sbData.Append("</table>");
            }
            sb.Append(sbData);
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
        finally
        {
            sbData = null;
        }
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
            //    sBegin += "<br/>";

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
            System.Web.HttpContext.Current.Session["CurrentTemplateFontName"] = string.Empty;
            System.Web.HttpContext.Current.Session["CurrentTemplateFontName"] = FontName;
            //ViewState["CurrentTemplateFontName"] = string.Empty;
            //ViewState["CurrentTemplateFontName"] = FontName;
            if (FontName != "")
                sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalId);
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                System.Web.HttpContext.Current.Session["CurrentTemplateFontName"] = string.Empty;
                System.Web.HttpContext.Current.Session["CurrentTemplateFontName"] = FontName;
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
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
