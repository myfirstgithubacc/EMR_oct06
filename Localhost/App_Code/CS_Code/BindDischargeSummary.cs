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

/// <summary>
/// Summary description for BindNotes
/// </summary>
public class BindDischargeSummary
{
    string sConString = string.Empty;
    //string sEncodedDate;
    //Hashtable hstInput;
    //Hashtable houtPara;
    //DataSet ds;
    //DataSet ds1;
    //DataSet ds2;
    //SqlDataReader dr;
    //DAL.DAL DlObj;
    //DL_Funs ff = new DL_Funs();
    //BaseC.Patient bc;
    // BaseC.RestFulAPI objICM = new BaseC.RestFulAPI(sConString);


    public BindDischargeSummary(string Constring)
    {
        sConString = Constring;
    }

    public string getDefaultFontSize(Page pg, string HospitalLocationId)
    {
        string sFontSize = string.Empty;
        string FieldValue = string.Empty;
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", HospitalLocationId);
        if (FieldValue != string.Empty)
        {
            sFontSize = " font-size: " + FieldValue + ";";
        }
        return sFontSize;
    }

    public string getDefaultFontName(Page pg, string HospitalLocationId)
    {
        string sFontName = string.Empty;
        string FieldValue = string.Empty;
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalLocationId);
        if (FieldValue != string.Empty)
        {
            sFontName = fonts.GetFont("Name", FieldValue);
            if (sFontName != string.Empty)
                sFontName = " font-family: " + sFontName + ";";
        }
        return sFontName;
    }

    public StringBuilder BindVitals(string HospitalId, int EncounterId, string sFromDate, string sToDate, StringBuilder sb,
                                    StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string sEncodedDate = string.Empty;
        string sPreviosDate = string.Empty;
        bool bVRecords = false;
        bool bVMRecords = false;
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@intEncounterId", EncounterId);
        hsTb.Add("@chrFromDate", sFromDate);
        hsTb.Add("@chrToDate", sToDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientEncounterVitals", hsTb);

        objtlc = null;

        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;

        if (ds.Tables[0].Rows.Count > 0)
        {
            sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["Encoded Date"]);
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
                MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(HospitalId));
                if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "1")
                {
                    BeginList = "<ul>"; EndList = "</ul>";
                }
                else if (common.myStr(drTemplateListStyle["FieldsListStyle"]) == "2")
                {
                    BeginList = "<ol>"; EndList = "</ol>";
                }
            }

            sb.Append(BeginList);
        }
        String vitals = string.Empty;

        string date = string.Empty;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            if (sEncodedDate == common.myStr(ds.Tables[0].Rows[i]["Encoded Date"]))
            {
                if (!bVRecords)
                {
                    vitals += "<b>" + common.myStr(ds.Tables[0].Rows[i]["Encoded Date"]) + "</b> :<br/>";
                    bVRecords = true;
                }
                if (i + 1 != ds.Tables[0].Rows.Count)
                {
                    date = common.myStr(ds.Tables[0].Rows[i + 1]["VitalDate"]);
                }
                else
                {
                    date = string.Empty;
                }
                if (common.myStr(ds.Tables[0].Rows[i]["VitalDate"]) == date)
                {
                    vitals += common.myStr(ds.Tables[0].Rows[i]["VitalSignName"]) + ": " + common.myStr(ds.Tables[0].Rows[i]["VitalValue"]);
                    if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() != string.Empty)
                    {
                        vitals += " - " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]);
                    }
                    vitals += ", ";
                }
                else
                {
                    vitals += common.myStr(ds.Tables[0].Rows[i]["VitalSignName"]) + ": " + common.myStr(ds.Tables[0].Rows[i]["VitalValue"]); // +" - " + Convert.ToString(ds.Tables[0].Rows[i]["Remarks"]);
                    if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() != string.Empty)
                    {
                        vitals += " - " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]);
                    }
                    vitals += "<br/>";
                }
            }
            else
            {
                if (!bVMRecords)
                {
                    vitals += "<br/>";
                    vitals += "<b>" + common.myStr(ds.Tables[0].Rows[i]["Encoded Date"]) + "</b> :<br/>";
                    bVMRecords = true;
                }
                else if (sPreviosDate != common.myStr(ds.Tables[0].Rows[i]["Encoded Date"]))
                {
                    vitals += "<br/>";
                    vitals += "<b>" + common.myStr(ds.Tables[0].Rows[i]["Encoded Date"]) + "</b> :<br/>";
                }
                if (i + 1 != ds.Tables[0].Rows.Count)
                {
                    date = common.myStr(ds.Tables[0].Rows[i + 1]["VitalDate"]);
                }
                else
                    date = string.Empty;
                if (common.myStr(ds.Tables[0].Rows[i]["VitalDate"]) == date)
                {
                    vitals += common.myStr(ds.Tables[0].Rows[i]["VitalSignName"]) + ": " + common.myStr(ds.Tables[0].Rows[i]["VitalValue"]);
                    if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() != string.Empty)
                    {
                        vitals += " - " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]);
                    }
                    vitals += ", ";
                }
                else
                {
                    vitals += common.myStr(ds.Tables[0].Rows[i]["VitalSignName"]) + ": " + common.myStr(ds.Tables[0].Rows[i]["VitalValue"]);// +" - " + Convert.ToString(ds.Tables[0].Rows[i]["Remarks"]);
                    if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]).Trim() != string.Empty)
                    {
                        vitals += " - " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]);
                    }
                    vitals += string.Empty;
                }
                sPreviosDate = common.myStr(ds.Tables[0].Rows[i]["Encoded Date"]);
            }
        }
        sb.Append(vitals);
        sb.Append(EndList);
        

        return sb;
    }

    public StringBuilder BindImmunization(string HospitalId, Int64 RegNo, int EncounterID, string sFromDate, string sToDate, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string date = string.Empty;
        int i = 0;
        string sPreviosDate = string.Empty;
        bool bIRecords = false;
        bool bIMRecords = false;
        string sEncodedDate = string.Empty;
        Hashtable hsTb = new Hashtable();
        hsTb.Add("@inyHospitalLocationID", HospitalId);
        hsTb.Add("@intRegistrationId ", RegNo);
        hsTb.Add("@intEncounterId", EncounterID);
        hsTb.Add("@chvFromDate ", sFromDate);
        hsTb.Add("@chvToDate", sToDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunization", hsTb);
        objtlc = null;

        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, HospitalId);
        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(HospitalId));
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["EntryDate"]);
            if (common.myStr(ds.Tables[0].Rows[0]["GivenDate"]) != string.Empty)
            {
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
                sb.Append("<br/>");
                sb.Append(BeginList);
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (sEncodedDate == common.myStr(ds.Tables[0].Rows[i]["EntryDate"]))
                    {
                        if (!bIRecords)
                        {
                            sb.Append(common.myStr(ds.Tables[0].Rows[i]["EntryDate"]) + " : <br/>");
                            bIRecords = true;
                        }
                        if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]) == string.Empty)
                        {
                            sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                           + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"]) + ". " + sEnd);
                        }
                        else
                        {
                            sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                           + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"])
                            + ", Remarks: " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]) + ". " + sEnd);
                        }
                    }
                    else
                    {
                        if (!bIMRecords)
                        {
                            sb.Append("<br/>");
                            sb.Append(common.myStr(ds.Tables[0].Rows[i]["EntryDate"]) + " : <br/>");
                            bIMRecords = true;
                        }
                        else if (sPreviosDate != common.myStr(ds.Tables[0].Rows[i]["EntryDate"]))
                        {
                            sb.Append("<br/>");
                            sb.Append(common.myStr(ds.Tables[0].Rows[i]["EntryDate"]) + " : <br/>");
                        }
                        if (common.myStr(ds.Tables[0].Rows[i]["Remarks"]) == string.Empty)
                        {
                            sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                           + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"]) + ". " + sEnd);
                        }
                        else
                        {
                            sb.Append(sBegin + common.myStr(ds.Tables[0].Rows[i]["ImmunizationName"]) + ", "
                           + "Given Date: " + common.myStr(ds.Tables[0].Rows[i]["GivenDate"])
                            + ", Remarks: " + common.myStr(ds.Tables[0].Rows[i]["Remarks"]) + ". " + sEnd);
                        }
                    }
                }
            }
        }
        sb.Append(EndList);
        return sb;
    }

    public StringBuilder BindProblemsHPI(int RegId, int HospitalId, int EncounterId, string sFromDate, string sToDate, StringBuilder sb,
                                         StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string sEncodedDate = string.Empty;
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        StringBuilder sbTemp = new StringBuilder();
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        Hashtable hsProblems = new Hashtable();
        DataTable dtChronic = new DataTable();
        DataTable dtNonChronic = new DataTable();
        bool bPRRecords = false;
        bool bPRMRecords = false;
        string strSql = string.Empty;
        string strAge = string.Empty;
        hsProblems.Add("@inyHospitalLocationID", HospitalId);
        hsProblems.Add("@intRegistrationId", RegId);
        hsProblems.Add("@intEncounterId", EncounterId);
        hsProblems.Add("@chrFromDate", sFromDate);
        hsProblems.Add("@chrToDate", sToDate);

        strSql = "Select r.FirstName, r.LastName, dbo.AgeInYrsMonthDay (Convert(varchar(10),DateofBirth,111), Convert(Varchar(10), Getdate(),111)) as Age, dbo.GetGender(Gender) as Gender,";
        strSql = strSql + " c.Race from Registration r WITH (NOLOCK) Left Join RaceMaster c WITH (NOLOCK) On r.RaceId = c.RaceId";
        strSql = strSql + " where r.Id = @intRegistrationId ";
        strSql = strSql + " Select IsNull(IsPregnant,0) as IsPregnant, IsNull(IsBreastFeeding,0) as IsBreastFeeding from Encounter WITH (NOLOCK) where Id = @intEncounterId";

        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", hsProblems);
        ds2 = objtlc.GetDataSet(CommandType.Text, strSql, hsProblems);

        objtlc = null;

        if (ds2.Tables.Count > 0)
        {
            if (ds2.Tables[0].Rows.Count > 0)
            {
                if (common.myStr(ds2.Tables[0].Rows[0]["Age"]).Trim() != string.Empty)
                {
                    strAge = common.myStr(ds2.Tables[0].Rows[0]["Age"]).Trim();
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sEncodedDate = string.Empty;
                    sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["EntryDate"]);
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
                    MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myStr(HospitalId));
                    MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(HospitalId));

                    DataView dvChronic = ds.Tables[0].DefaultView;
                    dvChronic.RowFilter = "IsChronic='True'";
                    dtChronic = dvChronic.ToTable();
                    DataView dvNonChronic = ds.Tables[0].DefaultView;
                    dvNonChronic.RowFilter = "IsChronic='False'";
                    dtNonChronic = dvNonChronic.ToTable();
                    if (dtChronic.Rows.Count > 0)
                    {

                        sbTemp.Append(sBeginFont);
                        sbTemp.Append("<br/>");
                        sbTemp.Append("The patient, " + common.myStr(ds2.Tables[0].Rows[0]["FirstName"]).Trim() + " " + common.myStr(ds2.Tables[0].Rows[0]["LastName"]).Trim() + ", is a ");
                        sbTemp.Append(strAge + " old " + common.myStr(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + common.myStr(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " with a history of ");
                        #region // Chronic problems starts here
                        for (int i = 0; i < dtChronic.Rows.Count; i++)
                        {
                            DataRow dr1 = dtChronic.Rows[i] as DataRow;
                            if (i == 0)
                            {
                                if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtChronic.Rows.Count - 1)
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                    }
                                }
                            }
                            else
                            {
                                if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                    }
                                }
                            }

                            if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem2"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtChronic.Rows.Count - 1)
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ", ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem2"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem3"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtChronic.Rows.Count - 1)
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ", ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem3"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem4"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtChronic.Rows.Count - 1)
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ", ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem4"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem5"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtChronic.Rows.Count - 1)
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).Trim() + ", ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem5"]) != string.Empty)
                            {
                                if (i == dtChronic.Rows.Count - 1)
                                {
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                                }
                                else
                                {
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                                }
                            }
                        }

                        #endregion // Chronic problems sentence ends here

                        #region non chronic problem sentence start
                        // Non Chronic problems starts here
                        for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                        {
                            //intRowCount = Convert.ToInt16( dtNonChronic.Rows.Count - 1 );
                            DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                            if (i == 0)
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" who presents with ");
                                //sbTemp.Append(dr1["ProblemDescription"].ToString().ToLower().Trim() + ", ");
                                //
                                if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        //sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                    }
                                }
                            }
                            else
                            {

                                if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                                {
                                    sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                }
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                }
                            }

                            if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem2"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem2"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem3"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem3"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem4"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem4"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem5"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem5"]) != string.Empty)
                            {
                                if (i == dtNonChronic.Rows.Count - 1)
                                {
                                    sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                                }
                                else
                                {
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                                }
                            }
                        }
                        sbTemp.Append(sEndFont);
                        #endregion //Non Chronic problems ends here.
                    }
                    else
                    {
                        #region if only non chronic problem then sentence start here
                        sbTemp.Append(sBeginFont);
                        sbTemp.Append("<br/>");
                        sbTemp.Append("The patient, " + common.myStr(ds2.Tables[0].Rows[0]["FirstName"]).Trim() + " " + common.myStr(ds2.Tables[0].Rows[0]["LastName"]).Trim() + ", is a ");
                        sbTemp.Append(strAge + "  old " + common.myStr(ds2.Tables[0].Rows[0]["Race"]).Trim() + " " + common.myStr(ds2.Tables[0].Rows[0]["Gender"]).ToLower().Trim() + " who presents with ");
                        for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                        {
                            DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                            if (i == 0)
                            {
                                if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                }
                            }
                            else
                            {
                                if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                                {
                                    sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                }
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + ", ");
                                }
                            }

                            if (common.myStr(dr1["AssociatedProblem1"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem2"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem1"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem2"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem3"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem2"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem3"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem4"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append("and " + common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem3"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem4"]) != string.Empty)
                            {
                                if (common.myStr(dr1["AssociatedProblem5"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if (i == dtNonChronic.Rows.Count - 1)
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["AssociatedProblem4"]).ToLower().Trim() + ", ");
                                }
                            }
                            if (common.myStr(dr1["AssociatedProblem5"]) != string.Empty)
                            {
                                if (i == dtNonChronic.Rows.Count - 1)
                                {
                                    sbTemp.Append("and " + common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ". ");
                                }
                                else
                                    sbTemp.Append(common.myStr(dr1["AssociatedProblem5"]).ToLower().Trim() + ", ");
                            }
                        }
                        sbTemp.Append(sEndFont);
                    }

                    #endregion if non chronic problem sentence ends

                    if (common.myStr(ds2.Tables[1].Rows[0]["IsPregnant"]) != "False")
                        sbTemp.Append(sBeginFont + " The patient is or may be pregnant. " + sEndFont);
                    if (common.myStr(ds2.Tables[1].Rows[0]["IsBreastFeeding"]) != "False")
                        sbTemp.Append(sBeginFont + " The patient is breast feeding. " + sEndFont);

                    //sbTemp.Append(BeginList); //cut from here 
                    #region chronic problem listing
                    for (int i = 0; i < dtChronic.Rows.Count; i++)
                    {
                        //sbTemp.Append(BeginList);// pasted here 
                        //sbTemp.Append(sBegin + " The patient describes the chronic " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                        DataRow dr1 = dtChronic.Rows[i] as DataRow;
                        if ((common.myStr(dr1["Quality1"]).ToLower() != string.Empty) || (common.myStr(dr1["Quality2"]) != string.Empty) || (common.myStr(dr1["Location"]) != string.Empty) || (common.myStr(dr1["Severity"]) != string.Empty) || (common.myStr(dr1["Onset"]) != string.Empty) || (common.myStr(dr1["Context"]) != string.Empty) || (common.myStr(dr1["Duration"]) != string.Empty) || (common.myStr(dr1["NoOfOccurrence"]).Trim() != string.Empty) || (common.myStr(dr1["PriorIllnessDate"]).Trim() != string.Empty))
                        {
                            //Changed onsetDate to onset
                            //Added this line to append string after if condition,previously it is above the if condition
                            //Vineet
                            sbTemp.Append(sBegin + " The patient describes the chronic " + common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                            if (common.myStr(dr1["Quality1"]).ToLower() != string.Empty)
                            {
                                if (common.myStr(dr1["Quality2"]) != string.Empty)
                                    sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + " ");
                                }
                            }
                            if (common.myStr(dr1["Quality2"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Quality3"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["Quality2"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality2"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality2"]).ToLower().Trim() + " ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["Quality3"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Quality4"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["Quality3"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality3"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality3"]).ToLower().Trim() + " ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["Quality4"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Quality5"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["Quality4"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality4"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality4"]).ToLower().Trim() + " ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["Quality5"]) != string.Empty)
                            {
                                if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                {
                                    sbTemp.Remove(sbTemp.Length - 2, 2);
                                    sbTemp.Append(" and " + common.myStr(dr1["Quality5"]).ToLower().Trim() + ". ");
                                }
                                else
                                {
                                    sbTemp.Remove(sbTemp.Length - 2, 2);
                                    sbTemp.Append(" and " + common.myStr(dr1["Quality5"]).ToLower().Trim() + " ");
                                }
                            }

                            if (common.myStr(dr1["Location"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Severity"]) != string.Empty)
                                {
                                    if (common.myStr(dr1["SideDescription"]).Trim() != string.Empty)
                                    {
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " (" + common.myStr(dr1["SideDescription"]).ToLower().Trim() + ") ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " ");
                                }
                                else
                                {
                                    if (common.myStr(dr1["SideDescription"]).Trim() != string.Empty)
                                    {
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " (" + common.myStr(dr1["SideDescription"]).ToLower().Trim() + "). ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + ". ");
                                }
                            }
                            if (common.myStr(dr1["Severity"]) != string.Empty)
                            {
                                if (common.myStr(dr1["OnsetDate"]) != string.Empty)
                                    sbTemp.Append("with a severity of " + common.myStr(dr1["Severity"]).ToLower().Trim() + ". ");
                                else
                                {
                                    sbTemp.Append("with a severity of " + common.myStr(dr1["Severity"]).ToLower().Trim() + ". ");
                                }
                            }


                            //sbTemp.Append("that began on [OnSet Date] ");  
                            if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) != string.Empty && common.myStr(dr1["Duration"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + ", occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + ", occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) == string.Empty && common.myStr(dr1["Context"]) != string.Empty && common.myStr(dr1["Duration"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms occur " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) == string.Empty && common.myStr(dr1["Duration"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower() + " and " + common.myStr(dr1["Duration"]).ToLower() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) == string.Empty && common.myStr(dr1["Duration"]) == string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + ". ");
                            }

                            if (common.myStr(dr1["NoOfOccurrence"]).Trim() != string.Empty && common.myStr(dr1["NoOfOccurrence"]) != "0")
                            {
                                if (common.myStr(dr1["PriorIllnessDate"]).Trim() != string.Empty)
                                    sbTemp.Append(" Problem occurred " + common.myStr(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                                else
                                    sbTemp.Append(" Problem occurred " + common.myStr(dr1["NoOfOccurrence"]).ToLower().Trim() + " times. ");
                            }
                            if (common.myStr(dr1["PriorIllnessDate"]).Trim() != string.Empty)
                                sbTemp.Append(" beginning on " + common.myStr(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");

                        }

                        if (common.myStr(dr1["AggravatingFactors"]).Trim() != string.Empty)
                            sbTemp.Append(" Aggravating factors are: " + common.myStr(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
                        if (common.myStr(dr1["RelievingFactors"]).Trim() != string.Empty)
                            sbTemp.Append(" Relieving factors are:  " + common.myStr(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
                        if ((common.myStr(dr1["DeniesSymptoms1"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms2"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms3"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms4"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms5"]) != string.Empty))
                            sbTemp.Append(" Patient denies symptoms of ");
                        if (common.myStr(dr1["DeniesSymptoms1"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms2"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms2"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms3"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms3"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms4"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms4"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms5"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms5"]) != string.Empty)
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
                        }

                        if (common.myStr(dr1["Condition"]) != string.Empty)
                        {
                            sbTemp.Append("The patient's condition is " + common.myStr(dr1["Condition"]).ToLower().Trim());
                            if (common.myStr(dr1["Percentage"]) != "0" && common.myStr(dr1["Percentage"]) != string.Empty)
                            {
                                sbTemp.Append(" by " + common.myStr(dr1["Percentage"]).Trim() + "% .");
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

                    #region nonchronic problem details with listing
                    for (int i = 0; i < dtNonChronic.Rows.Count; i++)
                    {
                        //sbTemp.Append(BeginList);// pasted here   
                        DataRow dr1 = dtNonChronic.Rows[i] as DataRow;
                        //sbTemp.Append("<br /><br />" + Convert.ToString(dr1["ProblemDescription"]) + " presents with ");
                        //sbTemp.Append(sBegin + " The patient describes the " + Convert.ToString(dr1["ProblemDescription"]).ToLower().Trim() + " ");


                        if ((common.myStr(dr1["Quality1"]).ToLower() != string.Empty) || (common.myStr(dr1["Location"]) != string.Empty) || (common.myStr(dr1["Severity"]) != string.Empty) || (common.myStr(dr1["Onset"]) != string.Empty) || (common.myStr(dr1["Context"]) != string.Empty) || (common.myStr(dr1["Duration"]) != string.Empty) || (common.myStr(dr1["NoOfOccurrence"]).Trim() != string.Empty) || (common.myStr(dr1["PriorIllnessDate"]).Trim() != string.Empty))
                        {
                            //Changed onsetDate to onset
                            //Added this line to append string after if condition,previously it is above the if condition
                            //Vineet
                            sbTemp.Append(sBegin + " The patient describes the " + common.myStr(dr1["ProblemDescription"]).ToLower().Trim() + " ");
                            if (common.myStr(dr1["Quality1"]).ToLower() != string.Empty)
                            {
                                if (common.myStr(dr1["Quality2"]) != string.Empty)
                                    sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                        sbTemp.Append("as " + common.myStr(dr1["Quality1"]).ToLower().Trim() + " ");
                                }
                            }
                            if (common.myStr(dr1["Quality2"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Quality3"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["Quality2"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality2"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality2"]).ToLower().Trim() + " ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["Quality3"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Quality4"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["Quality3"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality3"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality3"]).ToLower().Trim() + " ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["Quality4"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Quality5"]) != string.Empty)
                                    sbTemp.Append(common.myStr(dr1["Quality4"]).ToLower().Trim() + ", ");
                                else
                                {
                                    if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality4"]).ToLower().Trim() + ". ");
                                    }
                                    else
                                    {
                                        sbTemp.Remove(sbTemp.Length - 2, 2);
                                        sbTemp.Append(" and " + common.myStr(dr1["Quality4"]).ToLower().Trim() + " ");
                                    }
                                }
                            }
                            if (common.myStr(dr1["Quality5"]) != string.Empty)
                            {
                                if ((common.myStr(dr1["Location"]) == string.Empty) && (common.myStr(dr1["Severity"]) == string.Empty))
                                {
                                    sbTemp.Remove(sbTemp.Length - 2, 2);
                                    sbTemp.Append(" and " + common.myStr(dr1["Quality5"]).ToLower().Trim() + ". ");
                                }
                                else
                                {
                                    sbTemp.Remove(sbTemp.Length - 2, 2);
                                    sbTemp.Append(" and " + common.myStr(dr1["Quality5"]).ToLower().Trim() + " ");
                                }
                            }

                            if (common.myStr(dr1["Location"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Severity"]) != string.Empty)
                                {
                                    if (common.myStr(dr1["SideDescription"]).Trim() != string.Empty)
                                    {
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " (" + common.myStr(dr1["SideDescription"]).ToLower().Trim() + ") ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " ");
                                }
                                else
                                {
                                    if (common.myStr(dr1["SideDescription"]).Trim() != string.Empty)
                                    {
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + " (" + common.myStr(dr1["SideDescription"]).ToLower().Trim() + "). ");
                                    }
                                    else
                                        sbTemp.Append(common.myStr(dr1["Location"]).ToLower().Trim() + ". ");
                                }
                            }
                            if (common.myStr(dr1["Severity"]) != string.Empty)
                            {
                                if (common.myStr(dr1["Onset"]) != string.Empty)
                                    sbTemp.Append("with a severity of " + common.myStr(dr1["Severity"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("with a severity of " + common.myStr(dr1["Severity"]).ToLower().Trim() + ". ");
                            }
                            //sbTemp.Append("that began on [OnSet Date] "); 



                            if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) != string.Empty && common.myStr(dr1["Duration"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + ", occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + ", occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) == string.Empty && common.myStr(dr1["Context"]) != string.Empty && common.myStr(dr1["Duration"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms occur " + common.myStr(dr1["Context"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) == string.Empty && common.myStr(dr1["Duration"]) != string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and " + common.myStr(dr1["Duration"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) != string.Empty && common.myStr(dr1["Duration"]) == string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + " and occurs " + common.myStr(dr1["Context"]).ToLower().Trim() + ". ");
                            }
                            else if (common.myStr(dr1["Onset"]) != string.Empty && common.myStr(dr1["Context"]) == string.Empty && common.myStr(dr1["Duration"]) == string.Empty)
                            {
                                if (i == 0)
                                    sbTemp.Append("Symptom is " + common.myStr(dr1["Onset"]).ToLower().Trim() + ". ");
                                else
                                    sbTemp.Append("Symptoms are " + common.myStr(dr1["Onset"]).ToLower().Trim() + ". ");
                            }

                            if (common.myStr(dr1["NoOfOccurrence"]) != string.Empty && common.myStr(dr1["NoOfOccurrence"]) != "0")
                            {
                                if (common.myStr(dr1["PriorIllnessDate"]) != string.Empty)
                                    sbTemp.Append(" Problem occurred " + common.myStr(dr1["NoOfOccurrence"]).ToLower().Trim() + " times");
                                else
                                    sbTemp.Append(" Problem occurred " + common.myStr(dr1["NoOfOccurrence"]).ToLower().Trim() + " times.");
                            }
                            if (common.myStr(dr1["PriorIllnessDate"]) != string.Empty)
                                sbTemp.Append(" beginning on " + common.myStr(dr1["PriorIllnessDate"]).ToLower().Trim() + ". ");
                        }
                        if (common.myStr(dr1["AggravatingFactors"]).Trim() != string.Empty)
                            sbTemp.Append(" Aggravating factors are: " + common.myStr(dr1["AggravatingFactors"]).ToLower().Trim() + ". ");
                        if (common.myStr(dr1["RelievingFactors"]).Trim() != string.Empty)
                            sbTemp.Append(" Relieving factors are:  " + common.myStr(dr1["RelievingFactors"]).ToLower().Trim() + ". ");
                        if ((common.myStr(dr1["DeniesSymptoms1"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms2"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms3"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms4"]) != string.Empty) || (common.myStr(dr1["DeniesSymptoms5"]) != string.Empty))
                            sbTemp.Append(" Patient denies symptoms of ");
                        if (common.myStr(dr1["DeniesSymptoms1"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms2"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms1"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                //sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms1"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms2"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms3"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms2"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms2"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms3"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms4"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms3"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms3"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms4"]) != string.Empty)
                        {
                            if (common.myStr(dr1["DeniesSymptoms5"]) != string.Empty)
                            {
                                sbTemp.Append(common.myStr(dr1["DeniesSymptoms4"]).ToLower().Trim() + ", ");
                            }
                            else
                            {
                                sbTemp.Remove(sbTemp.Length - 2, 2);
                                sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms4"]).ToLower().Trim() + ". ");
                            }
                        }

                        if (common.myStr(dr1["DeniesSymptoms5"]) != string.Empty)
                        {
                            sbTemp.Remove(sbTemp.Length - 2, 2);
                            sbTemp.Append(" and " + common.myStr(dr1["DeniesSymptoms5"]).ToLower().Trim() + ". ");
                        }
                        if (common.myStr(dr1["Condition"]) != string.Empty)
                        {
                            sbTemp.Append("The patient's condition is " + common.myStr(dr1["Condition"]).ToLower().Trim());
                            if (common.myStr(dr1["Percentage"]) != "0" && common.myStr(dr1["Percentage"]) != string.Empty)
                            {
                                sbTemp.Append(" by " + common.myStr(dr1["Percentage"]).Trim() + "% .");
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
                }
            }

        }
        if (sbTemp.ToString() != string.Empty)
        {
            sb.Append(sbTemplateStyle);
            //sbTemp.Append(EndList);
            if (common.myStr(ds.Tables[0].Rows[0]["HPIRemarks"]).Trim() != string.Empty)
                sbTemp.Append(sBeginFont + " Remarks: " + common.myStr(ds.Tables[0].Rows[0]["HPIRemarks"]) + sEndFont);

        }

        sb.Append(sbTemp);
        return sb;
    }

    public StringBuilder BindAssessments(int RegId, int HospitalId, int EncounterId, string sFromDate, string sToDate, Int16 UserId, string DocId,
                                StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;

        StringBuilder sbChronic = new StringBuilder();
        StringBuilder sbPrimary = new StringBuilder();
        StringBuilder sbSeconday = new StringBuilder();
        string sEncodedDate = string.Empty;
        DataView dvChronic = new DataView();
        DataView dvPrimary = new DataView();
        DataView dvSecondary = new DataView();

        DataTable dtChronic = new DataTable();
        DataTable dtPrimary = new DataTable();
        DataTable dtSecondary = new DataTable();
        bool bCRecords = false;
        bool bPRecords = false;
        bool bSRecords = false;

        bool bCMRecords = false;
        bool bPMRecords = false;
        bool bSMRecords = false;

        Hashtable hsAss = new Hashtable();
        hsAss.Add("@intRegistrationId", RegId);

        hsAss.Add("@inyHospitalLocationID", HospitalId);
        hsAss.Add("@intEncounterId", EncounterId);
        hsAss.Add("@chrFromDate", sFromDate);
        hsAss.Add("@chrToDate", sToDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hsAss);

        objtlc = null;

        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        string sBeginSection = string.Empty;
        string sEndSection = string.Empty;
        if (ds.Tables[0].Rows.Count > 0)
        {
            sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["EntryDate"]);
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

        sBegin = string.Empty; sEnd = string.Empty;
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

        dvChronic = new DataView(ds.Tables[0]);
        dvChronic.RowFilter = "IsChronic=True";

        dtChronic = dvChronic.ToTable();

        foreach (DataRow dr in dtChronic.Rows)
        {
            if (sEncodedDate == common.myStr(dr["EntryDate"]))
            {
                if (!bCRecords)
                {
                    sbChronic.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> :<br/>");
                    bCRecords = true;
                }

                if (common.myStr(dr["ICDCode"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "), ");
                    else
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "). ");
                }
                else
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + ", ");
                    else
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisType"]) != string.Empty)
                {
                    if ((common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ", ");
                    else
                        sbChronic.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ". ");
                }

                if (common.myStr(dr["OnsetDate"]) != string.Empty)
                {
                    if ((common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ", ");
                    else
                        sbChronic.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ". ");
                }
                if (common.myStr(dr["Location"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(" Location : " + common.myStr(dr["Location"]) + ", ");
                    else
                        sbChronic.Append(" Location : " + common.myStr(dr["Location"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty || common.myStr(dr["DiagnosisCondition2"]) != string.Empty || common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                    sbChronic.Append(" Condition : ");
                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition1"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition2"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                {
                    sbChronic.Append(common.myStr(dr["DiagnosisCondition3"]) + ". ");
                }

                if (common.myStr(dr["Remarks"]) != string.Empty)
                    sbChronic.Append("<br /> Remarks : " + common.myStr(dr["Remarks"]) + ". ");
                sbChronic.Append(sEnd);

            }
            else
            {
                if (sEncodedDate != common.myStr(dr["EntryDate"]))
                {
                    sbChronic.Append("<br/><br/>");
                    sbChronic.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> :<br/>");
                    bCMRecords = true;
                }
                if (common.myStr(dr["ICDCode"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "), ");
                    else
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "). ");
                }
                else
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + ", ");
                    else
                        sbChronic.Append(sBegin + common.myStr(dr["ICDDescription"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisType"]) != string.Empty)
                {
                    if ((common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ", ");
                    else
                        sbChronic.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ". ");
                }

                if (common.myStr(dr["OnsetDate"]) != string.Empty)
                {
                    if ((common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ", ");
                    else
                        sbChronic.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ". ");
                }
                if (common.myStr(dr["Location"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbChronic.Append(" Location : " + common.myStr(dr["Location"]) + ", ");
                    else
                        sbChronic.Append(" Location : " + common.myStr(dr["Location"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty || common.myStr(dr["DiagnosisCondition2"]) != string.Empty || common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                    sbChronic.Append(" Condition : ");
                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition1"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbChronic.Append(common.myStr(dr["DiagnosisCondition2"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                {
                    sbChronic.Append(common.myStr(dr["DiagnosisCondition3"]) + ". ");
                }

                if (common.myStr(dr["Remarks"]) != string.Empty)
                    sbChronic.Append("<br /> Remarks : " + common.myStr(dr["Remarks"]) + ". ");
                sbChronic.Append(sEnd);

                sEncodedDate = common.myStr(dr["EntryDate"]);
            }

        }

        dvPrimary = new DataView(ds.Tables[0]);
        dvPrimary.RowFilter = "IsChronic=False AND PrimaryDiagnosis=True";

        dtPrimary = dvPrimary.ToTable();

        foreach (DataRow dr in dtPrimary.Rows)
        {
            if (sEncodedDate == common.myStr(dr["EntryDate"]))
            {
                if (!bPRecords)
                {
                    sbPrimary.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> :<br/>");
                    bPRecords = true;
                }


                if (common.myStr(dr["ICDCode"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "), ");
                    else
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "). ");
                }
                else
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + ", ");
                    else
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisType"]) != string.Empty)
                {
                    if ((common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ", ");
                    else
                        sbPrimary.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ". ");
                }

                if (common.myStr(dr["OnsetDate"]) != string.Empty)
                {
                    if ((common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ", ");
                    else
                        sbPrimary.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ". ");
                }
                if (common.myStr(dr["Location"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(" Location : " + common.myStr(dr["Location"]) + ", ");
                    else
                        sbPrimary.Append(" Location : " + common.myStr(dr["Location"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty || common.myStr(dr["DiagnosisCondition2"]) != string.Empty || common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                    sbPrimary.Append(" Condition : ");
                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition1"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition2"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                {
                    sbPrimary.Append(common.myStr(dr["DiagnosisCondition3"]) + ". ");
                }

                if (common.myStr(dr["Remarks"]) != string.Empty)
                    sbPrimary.Append("<br /> Remarks : " + common.myStr(dr["Remarks"]) + ". ");
                sbPrimary.Append(sEnd);
            }
            else
            {
                if (sEncodedDate != common.myStr(dr["EntryDate"]))
                {
                    sbPrimary.Append("<br/>");
                    sbPrimary.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> :<br/>");
                    bPMRecords = true;
                }
                if (common.myStr(dr["ICDCode"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "), ");
                    else
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "). ");
                }
                else
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + ", ");
                    else
                        sbPrimary.Append(sBegin + common.myStr(dr["ICDDescription"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisType"]) != string.Empty)
                {
                    if ((common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ", ");
                    else
                        sbPrimary.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ". ");
                }

                if (common.myStr(dr["OnsetDate"]) != string.Empty)
                {
                    if ((common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ", ");
                    else
                        sbPrimary.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ". ");
                }
                if (common.myStr(dr["Location"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbPrimary.Append(" Location : " + common.myStr(dr["Location"]) + ", ");
                    else
                        sbPrimary.Append(" Location : " + common.myStr(dr["Location"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty || common.myStr(dr["DiagnosisCondition2"]) != string.Empty || common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                    sbPrimary.Append(" Condition : ");
                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition1"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbPrimary.Append(common.myStr(dr["DiagnosisCondition2"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                {
                    sbPrimary.Append(common.myStr(dr["DiagnosisCondition3"]) + ". ");
                }

                if (common.myStr(dr["Remarks"]) != string.Empty)
                    sbPrimary.Append("<br /> Remarks : " + common.myStr(dr["Remarks"]) + ". ");
                sbPrimary.Append(sEnd);
                sEncodedDate = common.myStr(dr["EntryDate"]);
            }

        }

        dvSecondary = new DataView(ds.Tables[0]);
        dvSecondary.RowFilter = "IsChronic=False AND PrimaryDiagnosis=False";

        dtSecondary = dvSecondary.ToTable();

        foreach (DataRow dr in dtSecondary.Rows)
        {
            if (sEncodedDate == common.myStr(dr["EntryDate"]))
            {
                sBegin = string.Empty;
                sEnd = string.Empty;
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                sBeginSection = sBegin;
                sEndSection = sEnd;

                if (!bSRecords)
                {
                    sbSeconday.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> :<br/>");

                    bSRecords = true;
                }

                if (common.myStr(dr["ICDCode"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "), ");
                    else
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "). ");
                }
                else
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + ", ");
                    else
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisType"]) != string.Empty)
                {
                    if ((common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ", ");
                    else
                        sbSeconday.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ". ");
                }

                if (common.myStr(dr["OnsetDate"]) != string.Empty)
                {
                    if ((common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ", ");
                    else
                        sbSeconday.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ". ");
                }
                if (common.myStr(dr["Location"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(" Location : " + common.myStr(dr["Location"]) + ", ");
                    else
                        sbSeconday.Append(" Location : " + common.myStr(dr["Location"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty || common.myStr(dr["DiagnosisCondition2"]) != string.Empty || common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                    sbSeconday.Append(" Condition : ");
                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition1"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition2"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                {
                    sbSeconday.Append(common.myStr(dr["DiagnosisCondition3"]) + ". ");
                }

                if (common.myStr(dr["Remarks"]) != string.Empty)
                    sbSeconday.Append("<br /> Remarks : " + common.myStr(dr["Remarks"]) + ". ");
                sbSeconday.Append(sEnd);
            }
            else
            {
                if (sEncodedDate != common.myStr(dr["EntryDate"]))
                {
                    sbSeconday.Append("<br/><br/>");
                    sbSeconday.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> :<br/>");
                    bSMRecords = true;
                }
                if (common.myStr(dr["ICDCode"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "), ");
                    else
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + " (ICD Code:" + common.myStr(dr["ICDCode"]) + "). ");
                }
                else
                {
                    if ((common.myStr(dr["DiagnosisType"]) != string.Empty) || (common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + ", ");
                    else
                        sbSeconday.Append(sBegin + common.myStr(dr["ICDDescription"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisType"]) != string.Empty)
                {
                    if ((common.myStr(dr["OnsetDate"]) != string.Empty) || (common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ", ");
                    else
                        sbSeconday.Append(" Type : " + common.myStr(dr["DiagnosisType"]) + ". ");
                }

                if (common.myStr(dr["OnsetDate"]) != string.Empty)
                {
                    if ((common.myStr(dr["Location"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ", ");
                    else
                        sbSeconday.Append(" Onset Date : " + common.myStr(dr["OnsetDate"]) + ". ");
                }
                if (common.myStr(dr["Location"]) != string.Empty)
                {
                    if ((common.myStr(dr["DiagnosisCondition1"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition2"]) != string.Empty) || (common.myStr(dr["DiagnosisCondition3"]) != string.Empty))
                        sbSeconday.Append(" Location : " + common.myStr(dr["Location"]) + ", ");
                    else
                        sbSeconday.Append(" Location : " + common.myStr(dr["Location"]) + ". ");
                }

                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty || common.myStr(dr["DiagnosisCondition2"]) != string.Empty || common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                    sbSeconday.Append(" Condition : ");
                if (common.myStr(dr["DiagnosisCondition1"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition1"]) + ", ");
                    else
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition1"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition2"]) != string.Empty)
                {
                    if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition2"]) + ", ");
                    else
                        sbSeconday.Append(common.myStr(dr["DiagnosisCondition2"]) + ". ");
                }
                if (common.myStr(dr["DiagnosisCondition3"]) != string.Empty)
                {
                    sbSeconday.Append(common.myStr(dr["DiagnosisCondition3"]) + ". ");
                }

                if (common.myStr(dr["Remarks"]) != string.Empty)
                    sbSeconday.Append("<br /> Remarks : " + common.myStr(dr["Remarks"]) + ". ");

                sbSeconday.Append(sEnd);
                sEncodedDate = common.myStr(dr["EntryDate"]);
            }
        }
        if (sbChronic.ToString().Length > 15)
            sb.Append(sbChronic);
        if (sbPrimary.ToString().Length > 15)
            sb.Append(sbPrimary);
        if (sbSeconday.ToString().Length > 15)
            sb.Append(sbSeconday);
        return sb;
    }

    public StringBuilder BindAllergies(int RegId, string sFromDate, string sToDate, StringBuilder sb, StringBuilder sbTemplateStyle,
                                    DataRow drTemplateListStyle, Page pg, string hospitalID, string userID, string PageID)
    {
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        Hashtable hsAllergy = new Hashtable();
        bool bARecords = false;
        bool bAMRecords = false;
        bool bORecords = false;
        bool bOMRecords = false;
        StringBuilder sbDrugAllergy = new StringBuilder();
        StringBuilder sbOtherAllergy = new StringBuilder();
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginSection = string.Empty;
        string sEndSection = string.Empty;
        int t = 0;
        hsAllergy.Add("@intRegistrationId", RegId);
        hsAllergy.Add("@chrFromDate", sFromDate);
        hsAllergy.Add("@chrToDate", sToDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", hsAllergy);

        objtlc = null;

        DataView dvDrug = new DataView(ds.Tables[0]);
        dvDrug.RowFilter = "AllergyType='Drug'";

        DataView dvOther = new DataView(ds.Tables[0]);
        dvOther.RowFilter = "AllergyType<>'Drug'";
        string sEncodedDate = string.Empty;

        if (ds.Tables[0].Rows.Count > 0)
        {
            sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["EncodedDate"]).Substring(0, 10);
            sb.Append(sbTemplateStyle);
            if (drTemplateListStyle != null)
            {
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                sBeginSection = sBegin;
                sEndSection = sEnd;
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
        if (ds.Tables[1].Rows.Count > 0)
        {
            if (common.myStr(ds.Tables[1].Rows[0]["NoAllergies"]) != "True")
            {
                foreach (DataRow ddr in ds.Tables[0].Rows)
                {
                    if (sEncodedDate == common.myStr(ddr["EntryDate"]))
                    {
                        if (!bARecords)
                        {
                            sbDrugAllergy.Append("<b>" + common.myStr(ddr["EntryDate"]) + "</b> : <br/>");

                            sbDrugAllergy.Append(sBeginSection + "Drug Allergies:" + sEndSection + BeginList);
                            bARecords = true;
                            dvDrug.RowFilter = "EntryDate='" + common.myStr(ddr["EntryDate"]) + "' AND AllergyType='Drug'";
                            DataTable dtAD = dvDrug.ToTable();
                            foreach (DataRow dr in dtAD.Rows)
                            {
                                sBegin = string.Empty; sEnd = string.Empty;
                                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                                sbDrugAllergy.Append(" " + sBegin + common.myStr(dr["AllergyName"]) + " (Generic: " + common.myStr(dr["Generic_Name"]) + ")");
                                if (common.myStr(dr["AllergyDate"]) != string.Empty)
                                    sbDrugAllergy.Append(", Onset Date: " + common.myStr(dr["AllergyDate"]));
                                if (common.myStr(dr["Reaction"]) != string.Empty)
                                {
                                    if (common.myStr(dr["Intolerance"]) == "False" && (common.myStr(dr["Remarks"]) == string.Empty))
                                        sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]));
                                    else
                                        sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]) + string.Empty);
                                }
                                if (common.myStr(dr["Intolerance"]) == "True")
                                {
                                    if (common.myStr(dr["Remarks"]) == string.Empty)
                                        sbDrugAllergy.Append(", Intolerable.");
                                    else
                                        sbDrugAllergy.Append(", Intolerable");
                                }
                                if (common.myStr(dr["Remarks"]) != string.Empty)
                                    sbDrugAllergy.Append(", Remarks: " + common.myStr(dr["Remarks"]) + ".");

                                sbDrugAllergy.Append(sEnd);
                            }
                            dvOther.RowFilter = "EntryDate='" + common.myStr(ddr["EntryDate"]) + "' AND AllergyType='Food'";
                            DataTable dtAO = dvOther.ToTable();
                            foreach (DataRow dr in dtAO.Rows)
                            {
                                if (!bORecords)
                                {
                                    //sbOtherAllergy.Append("<b>" + dr["EncodedDate"].ToString().Substring(0, 10) + "</b> :<br/>");
                                    sbOtherAllergy.Append(sBeginSection + "Food/ Other Allergies:" + sEndSection + BeginList);
                                    bORecords = true;
                                }
                                sBegin = string.Empty; sEnd = string.Empty;
                                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));

                                sbOtherAllergy.Append(" " + sBegin + "Type: " + common.myStr(dr["AllergyType"]) + ", " + common.myStr(dr["AllergyName"]));
                                if (common.myStr(dr["AllergyDate"]) != string.Empty)
                                    sbOtherAllergy.Append(", Onset Date: " + common.myStr(dr["AllergyDate"]));
                                if (common.myStr(dr["Reaction"]) != string.Empty)
                                {
                                    if (common.myStr(dr["Intolerance"]) == "False" && (common.myStr(dr["Remarks"]) == string.Empty))
                                        sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]) + ".");
                                    else
                                        sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(dr["Reaction"]));
                                }
                                if (common.myStr(dr["Intolerance"]) == "True")
                                {
                                    if (common.myStr(dr["Remarks"]) == string.Empty)
                                        sbOtherAllergy.Append(", Intolerable.");
                                    else
                                        sbOtherAllergy.Append(", Intolerable");
                                }
                                if (common.myStr(dr["Remarks"]) != string.Empty)
                                    sbOtherAllergy.Append(", Remarks: " + common.myStr(dr["Remarks"]) + ".");
                                sbOtherAllergy.Append(sEnd);
                            }
                        }
                        // sEncodedDate = ddr["EncodedDate"].ToString().Substring(0, 10);
                    }
                    else
                    {
                        if (sEncodedDate != common.myStr(ddr["EntryDate"]))
                        {
                            sbDrugAllergy.Append("<br/>");
                            sbDrugAllergy.Append("<b>" + common.myStr(ddr["EntryDate"]).Substring(0, 10) + "</b> : <br/>");
                            sbDrugAllergy.Append(sBeginSection + "Drug Allergies:" + sEndSection + BeginList);
                            bAMRecords = true;
                            dvDrug.RowFilter = "EntryDate='" + common.myStr(ddr["EntryDate"]) + "' AND AllergyType='Drug'";
                            DataTable dtAD = dvDrug.ToTable();
                            foreach (DataRow drda in dtAD.Rows)
                            {
                                sBegin = string.Empty; sEnd = string.Empty;
                                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                                sbDrugAllergy.Append(" " + sBegin + common.myStr(drda["AllergyName"]) + " (Generic: " + common.myStr(drda["Generic_Name"]) + ")");
                                if (common.myStr(drda["AllergyDate"]) != string.Empty)
                                    sbDrugAllergy.Append(", Onset Date: " + common.myStr(drda["AllergyDate"]));
                                if (common.myStr(drda["Reaction"]) != string.Empty)
                                {
                                    if (common.myStr(drda["Intolerance"]) == "False" && (common.myStr(drda["Remarks"]) == string.Empty))
                                        sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(drda["Reaction"]) + ".");
                                    else
                                        sbDrugAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(drda["Reaction"]));
                                }
                                if (common.myStr(drda["Intolerance"]) == "True")
                                {
                                    if (common.myStr(drda["Remarks"]) == string.Empty)
                                        sbDrugAllergy.Append(", Intolerable.");
                                    else
                                        sbDrugAllergy.Append(", Intolerable");
                                }
                                if (common.myStr(drda["Remarks"]) != string.Empty)
                                    sbDrugAllergy.Append(", Remarks: " + common.myStr(drda["Remarks"]) + ".");

                                sbDrugAllergy.Append(sEnd);
                                //sEncodedDate = ddr["EncodedDate"].ToString().Substring(0, 10);
                            }
                            if (!bOMRecords)
                            {
                                sbOtherAllergy.Append("<br/>");
                                sbOtherAllergy.Append(sBeginSection + "Food/ Other Allergies:" + sEndSection + BeginList);
                                bOMRecords = true;
                            }
                            dvOther.RowFilter = "EntryDate='" + common.myStr(ddr["EntryDate"]) + "' AND AllergyType='Food'";
                            DataTable dtAO = dvOther.ToTable();
                            foreach (DataRow droa in dtAO.Rows)
                            {
                                sBegin = string.Empty; sEnd = string.Empty;
                                MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
                                sbOtherAllergy.Append(" " + sBegin + "Type: " + common.myStr(droa["AllergyType"]) + ", " + common.myStr(droa["AllergyName"]));
                                if (common.myStr(droa["AllergyDate"]) != string.Empty)
                                    sbOtherAllergy.Append(", Onset Date: " + common.myStr(droa["AllergyDate"]));
                                if (common.myStr(droa["Reaction"]) != string.Empty)
                                {
                                    if (common.myStr(droa["Intolerance"]) == "False" && (common.myStr(droa["Remarks"]) == string.Empty))
                                        sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(droa["Reaction"]) + ".");
                                    else
                                        sbOtherAllergy.Append(", <b>Reaction/Any Adverse Drug Event:</b> " + common.myStr(droa["Reaction"]));
                                }
                                if (common.myStr(droa["Intolerance"]) == "True")
                                {
                                    if (common.myStr(droa["Remarks"]) == string.Empty)
                                        sbOtherAllergy.Append(", Intolerable.");
                                    else
                                        sbOtherAllergy.Append(", Intolerable");
                                }
                                if (common.myStr(droa["Remarks"]) != string.Empty)
                                    sbOtherAllergy.Append(", Remarks: " + common.myStr(droa["Remarks"]) + ".");
                                sbOtherAllergy.Append(sEnd);
                            }
                            sEncodedDate = common.myStr(ddr["EntryDate"]);
                        }

                    }
                }
            }
            else
            {
                sbDrugAllergy.Append(sbTemplateStyle);
                sbDrugAllergy.Append("<br />" + BeginList + sBegin + " No Allergies." + sEnd + EndList);
            }
        }
        sbOtherAllergy.Append(EndList);
        sb.Append(sbDrugAllergy);
        sb.Append(sbOtherAllergy);
        return sb;
    }

    public StringBuilder BindMedication(int EncounterId, int HospitalId, int FacilityId, string sFromDate, string sToDate, StringBuilder sb, StringBuilder sbTemplateStyle, string MedicationType, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        string sBeginSection = string.Empty;
        string sEndSection = string.Empty;
        Hashtable hsMed = new Hashtable();
        hsMed = new Hashtable();
        DataSet ds = new DataSet();
        DataSet ds2 = new DataSet();
        bool bPRecords = false;
        bool bPMRecords = false;
        bool bARecords = false;
        bool bAMRecords = false;
        bool bDRecords = false;
        bool bDMRecords = false;
        bool bCRecords = false;
        bool bCMRecords = false;
        DataTable dtCurrentMedication = new DataTable();
        DataTable dtPriscription = new DataTable();
        StringBuilder sbPrescribed = new StringBuilder();
        StringBuilder sbAdministered = new StringBuilder();
        StringBuilder sbDispenced = new StringBuilder();
        StringBuilder sbCurrentMedication = new StringBuilder();
        StringBuilder sbCustomPrescription = new StringBuilder();
        StringBuilder sbCustomMedication = new StringBuilder();
        hsMed.Add("@inyHospitalLocationId", HospitalId);
        hsMed.Add("@intFacilityId", FacilityId);
        hsMed.Add("@inEncId", EncounterId);
        hsMed.Add("@chrFromDate", sFromDate);
        hsMed.Add("@chrToDate", sToDate);
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspGetMedicationIssue", hsMed);

        objtlc = null;

        // ds2 = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientCurrentMedication", hsMed);
        // dtCurrentMedication = ds2.Tables[0];
        dtPriscription = ds.Tables[0];
        DataView dvPrescribed = new DataView(ds.Tables[0]);
        //dvPrescribed.RowFilter = "PrescriptionMode ='E'";
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
        sbPrescribed.Append(BeginList);
        sbAdministered.Append(BeginList);
        sbDispenced.Append(BeginList);
        sbCurrentMedication.Append(BeginList);
        sbCustomPrescription.Append(BeginList);
        sbCustomMedication.Append(BeginList);
        string sEncodedDate = string.Empty;

        if (MedicationType == "P")
        {
            sBegin = string.Empty; sEnd = string.Empty;
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
            if (dtPriscription.Rows.Count > 0)
            {
                sEncodedDate = common.myStr(dtPriscription.Rows[0]["PostedDate"]);
                sb.Append(sbTemplateStyle);
                foreach (DataRowView dr in dvPrescribed)
                {
                    if (sEncodedDate == common.myStr(dr["PostedDate"]))
                    {
                        if (!bPRecords)
                        {
                            sbPrescribed.Append("<b><u>" + "MEDICATIONS &nbsp;:&nbsp;&nbsp;" + "</u></b>" + "<br/>");
                            sbPrescribed.Append("<b>" + "Presc No.&nbsp; :&nbsp;&nbsp;" + common.myStr(dr["IssueNo"]).Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + " </b>" + "<b>" + "Date&nbsp; :&nbsp;&nbsp;" + common.myStr(dr["PostedDate"]).Substring(0, 10) + " </b>  :<br/>");
                            sbPrescribed.Append("<b>" + "Medicine Name" + "</b>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + "<b>" + "Route" + "</b>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + "<b>" + "Frequency  </b><br/><br/>");
                            bPRecords = true;
                        }
                        if (common.myStr(dr["IsPrescription"]) == string.Empty)
                        {
                            //string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                            //string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                            sbPrescribed.Append(sBegin + " " + common.myStr(dr["ItemName"]) + ":"
                                //+ " " + Convert.ToString(dr["QtyAmount"])
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(dr["RouteName"])
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + common.myStr(dr["Frequency"]) + "<br/><br/>");

                            //if (Convert.ToString(dr["Days"]) != string.Empty)
                            //{
                            //    sbPrescribed.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                            //}
                            //else
                            //{
                            //    sbPrescribed.Append(", #" + dr["DispenseAmount"].ToString().Trim());
                            //}
                            //  sbPrescribed.Append(", Refill:" + refill + ".");
                            sbPrescribed.Append(sEnd);
                        }
                        else
                        {
                            sbPrescribed.Append(sBegin + " " + common.myStr(dr["ItemName"]));
                            sbPrescribed.Append(sEnd);
                        }
                    }
                    else
                    {
                        if (sEncodedDate != common.myStr(dr["PostedDate"]))
                        {
                            sbPrescribed.Append("<br/>");
                            sbPrescribed.Append("<b>" + common.myStr(dr["PostedDate"]) + "</b> : <br/>");
                            bPMRecords = true;
                        }
                        if (common.myStr(dr["IsPrescription"]) == string.Empty)
                        {
                            //string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                            //string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                            sbPrescribed.Append(sBegin + " " + common.myStr(dr["ItemName"]) + ":"
                                //+ " " + Convert.ToString(dr["QtyAmount"])
                                + " " + common.myStr(dr["RouteName"])
                                + " " + common.myStr(dr["Frequency"]));

                            //if (Convert.ToString(dr["Days"]) != string.Empty)
                            //{
                            //    sbPrescribed.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                            //}
                            //else
                            //{
                            //    sbPrescribed.Append(", #" + dr["DispenseAmount"].ToString().Trim());
                            //}
                            //  sbPrescribed.Append(", Refill:" + refill + ".");
                            sbPrescribed.Append(sEnd);
                        }
                        else
                        {
                            sbPrescribed.Append(sBegin + " " + common.myStr(dr["ItemName"]));
                            sbPrescribed.Append(sEnd);
                        }
                        sEncodedDate = common.myStr(dr["PostedDate"]);

                    }
                }
                //DataView dvAdministered = new DataView(ds.Tables[0]);
                //dvAdministered.RowFilter = "PrescriptionMode ='A'";
                //foreach (DataRowView dr in dvAdministered)
                //{
                //    if (sEncodedDate == dr["EntryDate"].ToString())
                //    {
                //        if (bARecords == false)
                //        {
                //            sbAdministered.Append("<b>" + dr["EntryDate"].ToString() + "</b> :<br/>");
                //            bARecords = true;
                //        }
                //        string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                //        string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                //        sbAdministered.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"
                //            + " " + Convert.ToString(dr["QtyAmount"])
                //           + " " + dr["Route_Description"].ToString()
                //           + " " + dr["Frequency_Description"].ToString());
                //        if (Convert.ToString(dr["Days"]) != string.Empty)
                //            sbAdministered.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                //        else
                //            sbAdministered.Append(", #" + Convert.ToString(dr["DispenseAmount"]).Trim());

                //        sbAdministered.Append(", Refill:" + refill + ".");//+ sEnd
                //        sbAdministered.Append("<br />" + sEnd);
                //    }
                //    else
                //    {
                //        if (sEncodedDate != dr["EntryDate"].ToString())
                //        {
                //            sbAdministered.Append("<br/>");
                //            sbAdministered.Append("<b>" + dr["EntryDate"].ToString() + "</b> :<br/>");
                //            bAMRecords = true;
                //        }
                //        string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                //        string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                //        sbAdministered.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"
                //            + " " + Convert.ToString(dr["QtyAmount"])
                //           + " " + dr["Route_Description"].ToString()
                //           + " " + dr["Frequency_Description"].ToString());
                //        if (Convert.ToString(dr["Days"]) != string.Empty)
                //            sbAdministered.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                //        else
                //            sbAdministered.Append(", #" + Convert.ToString(dr["DispenseAmount"]).Trim());

                //        sbAdministered.Append(", Refill:" + refill + ".");//+ sEnd
                //        sbAdministered.Append("<br />" + sEnd);
                //    }
                //    sEncodedDate = dr["EntryDate"].ToString();
                //}
                //DataView dvDispenced = new DataView(ds.Tables[0]);
                //dvDispenced.RowFilter = "PrescriptionMode ='D'";
                //foreach (DataRowView dr in dvDispenced)
                //{
                //    if (sEncodedDate == dr["EntryDate"].ToString())
                //    {
                //        if (bDRecords == false)
                //        {
                //            sbDispenced.Append("<b>" + dr["EntryDate"].ToString() + "</b> :<br/>");
                //            bDRecords = true;
                //        }
                //        string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                //        string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                //        sbDispenced.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"
                //            + " " + Convert.ToString(dr["QtyAmount"])
                //           + " " + dr["Route_Description"].ToString()
                //           + " " + dr["Frequency_Description"].ToString());

                //        if (Convert.ToString(dr["Days"]) != string.Empty)
                //            sbDispenced.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                //        else
                //            sbDispenced.Append(", #" + Convert.ToString(dr["DispenseAmount"]));
                //        sbDispenced.Append(", Refill:" + refill + ".");//+ sEnd
                //        sbDispenced.Append("<br />" + sEnd);
                //    }
                //    else
                //    {
                //        if (sEncodedDate != dr["EntryDate"].ToString())
                //        {
                //            sbDispenced.Append("<br/>");
                //            sbDispenced.Append("<b>" + dr["EntryDate"].ToString() + "</b> :<br/>");
                //            bDMRecords = true;
                //        }
                //        string refill = dr["Refill"].ToString() == "" ? "0" : dr["Refill"].ToString();
                //        string PRN = dr["PRN"].ToString() == "True" ? "PRN" : "";
                //        sbDispenced.Append(sBegin + " " + dr["Display_Name"].ToString() + ":"
                //            + " " + Convert.ToString(dr["QtyAmount"])
                //           + " " + dr["Route_Description"].ToString()
                //           + " " + dr["Frequency_Description"].ToString());

                //        if (Convert.ToString(dr["Days"]) != string.Empty)
                //            sbDispenced.Append(" for " + Convert.ToString(dr["Days"]) + " days");
                //        else
                //            sbDispenced.Append(", #" + Convert.ToString(dr["DispenseAmount"]));
                //        sbDispenced.Append(", Refill:" + refill + ".");//+ sEnd
                //        sbDispenced.Append("<br />" + sEnd);
                //    }
                //    sEncodedDate = dr["EntryDate"].ToString();
                //}
                sBegin = string.Empty; sEnd = string.Empty;
                MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
                sBeginSection = sBegin;
                sEndSection = sEnd;
                sb.Append(sbPrescribed.ToString());
                sb.Append(sbDispenced.ToString());
                sb.Append(sbAdministered.ToString());
            }
        }
        else
        {
            // For Current medication
            sBegin = string.Empty; sEnd = string.Empty;
            MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
            if (dtCurrentMedication.Rows.Count > 0)
            {
                sb.Append(sbTemplateStyle);
                if (!sBegin.Contains("<li>"))
                {
                    sbCurrentMedication.Append(string.Empty);
                }
                sEncodedDate = string.Empty;
                sEncodedDate = common.myStr(dtCurrentMedication.Rows[0]["EntryDate"]);
                foreach (DataRow dr in dtCurrentMedication.Rows)
                {
                    if (sEncodedDate == common.myStr(dr["EntryDate"]))
                    {
                        if (!bCRecords)
                        {
                            sbCurrentMedication.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> : <br/>");
                            bCRecords = true;
                        }
                        if (common.myStr(dr["IsPrescription"]) == string.Empty)
                        {
                            string PRN = common.myStr(dr["PRN"]) == "True" ? "" : "";
                            sbCurrentMedication.Append(sBegin + " " + common.myStr(dr["Display_Name"]) + ":"
                                + " " + common.myStr(dr["QtyAmount"])
                               + " " + common.myStr(dr["Route_Description"])
                               + " " + common.myStr(dr["Frequency_Description"]));
                        }
                        else
                        {
                            sbCurrentMedication.Append(sBegin + " " + common.myStr(dr["Display_Name"]));// + ":"
                            sbCurrentMedication.Append(sEnd);
                        }

                        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
                    }
                    else
                    {
                        if (sEncodedDate != common.myStr(dr["EntryDate"]))
                        {
                            sbCurrentMedication.Append("<br/>");
                            sbCurrentMedication.Append("<b>" + common.myStr(dr["EntryDate"]) + "</b> : <br/>");
                            bCMRecords = true;
                        }
                        if (common.myStr(dr["IsPrescription"]) == string.Empty)
                        {
                            string PRN = common.myStr(dr["PRN"]) == "True" ? "" : "";
                            sbCurrentMedication.Append(sBegin + " " + common.myStr(dr["Display_Name"]) + ":"
                                + " " + common.myStr(dr["QtyAmount"])
                               + " " + common.myStr(dr["Route_Description"])
                               + " " + common.myStr(dr["Frequency_Description"]));

                        }
                        else
                        {
                            sbCurrentMedication.Append(sBegin + " " + common.myStr(dr["Display_Name"]));// + ":"
                            sbCurrentMedication.Append(sEnd);
                        }

                        MakeFontWithoutListStyle("Fields", ref sBeginFont, ref sEndFont, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
                    }
                    sEncodedDate = common.myStr(dr["EntryDate"]);
                }
                sbCurrentMedication.Append(sEnd);
                sbCurrentMedication.Append(EndList);
                sb.Append(sbCurrentMedication.ToString());
            }
            else
            {
                DataSet dsNoMedication = new DataSet();
                BaseC.RestFulAPI objICM = new BaseC.RestFulAPI(sConString);
                dsNoMedication = objICM.GetICMNoCurrentMedication(common.myInt(HospitalId), common.myInt(EncounterId));
                if (dsNoMedication.Tables[0].Rows.Count > 0)
                {
                    if (common.myBool(dsNoMedication.Tables[0].Rows[0]["NoCurrentMedication"]))
                    {
                        sb.Append(sbTemplateStyle);
                        sb.Append(BeginList + sBegin + "</br> None." + sEnd + EndList);
                    }
                }
            }
        }
        return sb;
    }

    public StringBuilder BindOrders(Int64 RegNo, int HospitalId, int EncounterId, string sFromDate, string sToDate, Int16 UserId, string DocId,
                                StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg, string pageID, string userID)
    {
        DataSet ds = new DataSet();
        BaseC.ThreeLayerComponent objtlc = BaseC.ThreeLayerComponent.MyObject;
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        string sBeginSection = string.Empty;
        string sEndSection = string.Empty;
        Hashtable hsOrd = new Hashtable();
        bool bORecords = false;
        bool bOMRecords = false;
        string sEncodedDate = string.Empty;
        string sPreviousDate = string.Empty;
        hsOrd.Add("@intRegistrationId", RegNo);
        hsOrd.Add("@inyHospitalLocationID", HospitalId);
        hsOrd.Add("@intEncounterId", EncounterId);
        hsOrd.Add("@chrFromDate", sFromDate);
        hsOrd.Add("@chrToDate", sToDate);
        StringBuilder sbTemp = new StringBuilder();//UspEMRGetPreviousInvestigations
        ds = objtlc.GetDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hsOrd);

        objtlc = null;

        DataView dvmain = new DataView(ds.Tables[0]);
        dvmain.RowFilter = "ServiceType Not in ( 'CL','VS','VF','R','RT' )";
        dvmain.Sort = "DepartmentId,Id";
        DataTable dtmain = new DataTable();
        dtmain = dvmain.ToTable();

        if (dtmain.Rows.Count > 0)
        {
            sEncodedDate = common.myStr(dtmain.Rows[0]["EntryDate"]);
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
        int DepartmentId = 0;
        DataTable dt = new DataTable();
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());
        sBeginSection = sBegin;
        sEndSection = sEnd;

        sBegin = string.Empty; sEnd = string.Empty;
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]).ToString());


        for (int i = 0; i < dtmain.Rows.Count; i++)
        {
            if (sEncodedDate == common.myStr(dtmain.Rows[i]["EntryDate"]))
            {
                if (!bORecords)
                {
                    sbTemp.Append(common.myStr(dtmain.Rows[i]["EntryDate"]) + " : <br/>");
                    bORecords = true;
                }
                DataRow dr = dtmain.Rows[i] as DataRow;
                if (common.myInt(dr["DepartmentId"]) != DepartmentId)
                {
                    sbTemp.Append("<br />" + sBeginSection + common.myStr(dr["DepartmentName"]) + ": " + sEndSection);
                    sbTemp.Append(BeginList);
                    DepartmentId = common.myInt(dr["DepartmentId"]);
                    DataView dv = new DataView(dtmain);
                    dv.RowFilter = "DepartmentId=" + common.myInt(dr["DepartmentId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string commase = string.Empty;
                        if (j != dt.Rows.Count - 1)
                        {
                            commase = ", ";
                        }

                        DataRow dr1 = dt.Rows[j] as DataRow;
                        if (common.myStr(dr["CPTCode"]) != string.Empty)
                        {
                            sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + " (CPT Code:" + common.myStr(dr1["CPTCode"]) + ")" + commase + sEnd);
                        }
                        else
                        {
                            sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);
                        }
                    }
                    sbTemp.Append(EndList);
                }
            }
            else
            {
                if (!bOMRecords)
                {
                    sbTemp.Append("<br/>");
                    sbTemp.Append(common.myStr(dtmain.Rows[i]["EntryDate"]) + " : <br/>");
                    bOMRecords = true;
                }
                else if (sPreviousDate != common.myStr(dtmain.Rows[i]["EntryDate"]))
                {
                    sbTemp.Append("<br/>");
                    sbTemp.Append(common.myStr(dtmain.Rows[i]["EntryDate"]) + " : <br/>");
                }
                DataRow dr = dtmain.Rows[i] as DataRow;
                if (common.myInt(dr["DepartmentId"]) != DepartmentId)
                {
                    sbTemp.Append("<br />" + sBeginSection + common.myStr(dr["DepartmentName"]) + ": " + sEndSection);
                    sbTemp.Append(BeginList);
                    DepartmentId = common.myInt(dr["DepartmentId"]);
                    DataView dv = new DataView(dtmain);
                    dv.RowFilter = "DepartmentId=" + common.myInt(dr["DepartmentId"]);
                    dt = dv.ToTable();
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        string commase = string.Empty;
                        if (j != dt.Rows.Count - 1)
                        {
                            commase = ", ";
                        }

                        DataRow dr1 = dt.Rows[j] as DataRow;
                        if (common.myStr(dr["CPTCode"]) != string.Empty)
                        {
                            sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + " (CPT Code:" + common.myStr(dr1["CPTCode"]) + ")" + commase + sEnd);
                        }
                        else
                        {
                            sbTemp.Append(sBegin + common.myStr(dr1["ServiceName"]) + commase + sEnd);
                        }
                    }
                    sbTemp.Append(EndList);
                }
                sPreviousDate = common.myStr(dtmain.Rows[i]["EntryDate"]);
            }
        }
        sb.Append(sbTemp.ToString());
        return sb;
    }

    public DataSet GetICMPatientInvestigationResult(int iHospitalLocationID, int iFacilityId, int iEncounterID, string sFromDate, string sToDate)
    {
        Hashtable HshIn = new Hashtable();
        DataSet ds = new DataSet();
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

    public StringBuilder BindLabResult(int EncounterId, int HospitalId, int FacilityId, string sFromDate, string sToDate, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle, Page pg)
    {
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string sBeginFont = string.Empty;
        string sEndFont = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;
        string sBeginSection = string.Empty;
        string sEndSection = string.Empty;
        bool bLRecords = false;
        bool bLMRecords = false;
        string sEncodedDate = string.Empty;
        string sServiceID = string.Empty;
        DataSet ds = GetICMPatientInvestigationResult(HospitalId, FacilityId, EncounterId, sFromDate, sToDate);

        StringBuilder sbTemp = new StringBuilder();
        if (ds.Tables[0].Rows.Count > 0)
        {
            sServiceID = common.myStr(ds.Tables[0].Rows[0]["ServiceID"]);
            sEncodedDate = common.myStr(ds.Tables[0].Rows[0]["ResultDate"]);
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

        sBegin = string.Empty; sEnd = string.Empty;
        MakeFont("Fields", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
        string sStartTable = string.Empty;
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            sbTemp.Append(BeginList);
            if (sServiceID == common.myStr(ds.Tables[0].Rows[i]["ServiceID"]) && !bLRecords)
            {
                sbTemp.Append("<b><u>" + "Investigation(s) :" + "</u></b>" + "<br/>");
                sbTemp.Append("<b>" + "Date&nbsp;:&nbsp;&nbsp; " + ds.Tables[0].Rows[i]["ResultDate"] + "</b> : <br/>");
                sbTemp.Append("<b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b> : <br/>");
                bLRecords = true;
                //Remove Table
                sStartTable = " <table border='0' cellpadding='0' cellspacing='0' style='border-style:none;'>";
                sbTemp.Append(sStartTable);
                sServiceID = common.myStr(ds.Tables[0].Rows[i]["ServiceID"]);
            }
            else if ((sEncodedDate != common.myStr(ds.Tables[0].Rows[i]["ResultDate"])) || (sServiceID != common.myStr(ds.Tables[0].Rows[i]["ServiceID"])))
            {
                if (sEncodedDate != common.myStr(ds.Tables[0].Rows[i]["ResultDate"]))
                {
                    sStartTable = string.Empty;
                    sbTemp.Append("<tr><td><b>" + ds.Tables[0].Rows[i]["ResultDate"] + "</b> : <br/><td></tr>");
                    bLMRecords = true;
                    sEncodedDate = common.myStr(ds.Tables[0].Rows[i]["ResultDate"]);
                }
                if (sServiceID != common.myStr(ds.Tables[0].Rows[i]["ServiceID"]))
                {
                    sbTemp.Append("<tr ><td style='border-style:none;width:400px;'><b>" + ds.Tables[0].Rows[i]["ServiceName"] + "</b> : <br/></td></tr>");

                    //sStartTable = " <table border='0' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;'>";
                    //sbTemp.Append(sStartTable);
                    sServiceID = common.myStr(ds.Tables[0].Rows[i]["ServiceID"]);
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

    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item, Page Pg, string HospitalId)
    {
        DL_Funs ff = new DL_Funs();
        //string sBegin = "", sEnd = string.Empty;
        ArrayList aEnd = new ArrayList();
        if (item != null)
        {
            if (common.myStr(item[typ + "ListStyle"]) == "1")
            {
                sBegin += "<li>";
                //aEnd.Add("</li>");
            }
            else if (common.myStr(item[typ + "ListStyle"]) == "2")
            {
                sBegin += "<li>";
                //aEnd.Add("</li>");
            }

            //else
            //    sBegin += "<br />";

            if (common.myStr(item[typ + "Forecolor"]) != string.Empty || common.myStr(item[typ + "FontSize"]) != string.Empty || common.myStr(item[typ + "FontStyle"]) != string.Empty || common.myStr(item[typ + "Bold"]) != string.Empty || common.myStr(item[typ + "Italic"]) != string.Empty || common.myStr(item[typ + "Underline"]) != string.Empty)
            {
                sBegin += "<span style='";
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += ff.getDefaultFontSize(Pg, HospitalId); }
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty)
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
                { sBegin += GetFontFamily(typ, item, Pg, HospitalId); }

                if (common.myBool(item[typ + "Bold"]))
                { sBegin += " font-weight: bold;"; }
                if (common.myBool(item[typ + "Italic"]))
                { sBegin += " font-style: italic;"; }
                if (common.myBool(item[typ + "Underline"]))
                { sBegin += " text-decoration: underline;"; }

                aEnd.Add("</span>");
                for (int i = aEnd.Count - 1; i >= 0; i--)
                {
                    sEnd += aEnd[i];
                }

                sBegin += " '>";
                if (common.myStr(item[typ + "ListStyle"]) == "1")
                {
                    sEnd += "</li>";
                }
                else if (common.myStr(item[typ + "ListStyle"]) == "2")
                {
                    sEnd += "</li>";
                }
            }
            else
            {
                sBegin += "<span style='";
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += ff.getDefaultFontSize(Pg, HospitalId); }

                sBegin += GetFontFamily(typ, item, Pg, HospitalId);

                aEnd.Add("</span>");
                for (int i = aEnd.Count - 1; i >= 0; i--)
                {
                    sEnd += aEnd[i];
                }
                sBegin += " '>";
                if (common.myStr(item[typ + "ListStyle"]) == "1")
                {
                    sEnd += "</li>";
                }
                else if (common.myStr(item[typ + "ListStyle"]) == "2")
                {
                    sEnd += "</li>";
                }
            }

        }

    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item, Page Pg, string HospitalId)
    {

        //string sBegin = "", sEnd = string.Empty;
        ArrayList aEnd = new ArrayList();
        if (item != null)
        {
            if (common.myStr(item[typ + "Forecolor"]) != string.Empty || common.myStr(item[typ + "FontSize"]) != string.Empty || common.myStr(item[typ + "FontStyle"]) != string.Empty || common.myStr(item[typ + "Bold"]) != string.Empty || common.myStr(item[typ + "Italic"]) != string.Empty || common.myStr(item[typ + "Underline"]) != string.Empty)
            {
                sBegin += "<span style='";
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
                { sBegin += " font-size:11pt;"; }
                else { sBegin += getDefaultFontSize(Pg, HospitalId); }
                if (common.myStr(item[typ + "Forecolor"]) != string.Empty)
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
                { sBegin += sBegin += " font-family:Candara;"; }


                if (common.myBool(item[typ + "Bold"]))
                { sBegin += " font-weight: bold;"; }
                if (common.myBool(item[typ + "Italic"]))
                { sBegin += " font-style: italic;"; }
                if (common.myBool(item[typ + "Underline"]))
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
                if (common.myStr(item[typ + "FontSize"]) != string.Empty)
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
        string FieldValue = string.Empty;
        string FontName = string.Empty;
        string sBegin = string.Empty;
        ClinicDefaults cd = new ClinicDefaults(Pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        if (common.myStr(item[typ + "FontStyle"]) != string.Empty)
        {
            FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
            if (FontName != string.Empty)
                sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalId);
            if (FieldValue != string.Empty)
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != string.Empty)
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }

    public string GetPrescriptionDetailStringNew(DataTable dt)
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
                    if (dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty)
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

                        sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                    }

                    else
                    {
                        sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + dtString.Rows[i]["DoseTypeName"].ToString().Trim() + " - " : " ");
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
                        if (dtString.Rows[i]["Duration"].ToString().Trim() != "0")
                        {
                            sb.Append(" for " + dtString.Rows[i]["DurationText"].ToString() + " ");
                        }
                        sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : " ");
                        //sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");
                        sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                        sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);

                        try
                        {
                            //  sb.Append(", Start Date : " + dtString.Rows[i]["StartDate"].ToString() + ", End Date : " + dtString.Rows[i]["EndDate"].ToString());
                        }
                        catch { }

                        if (dtString.Rows[i]["Instructions"].ToString().Trim() != string.Empty)
                        {
                            sb.Append(Environment.NewLine + "INSTRUCTIONS : " + Environment.NewLine + dtString.Rows[i]["Instructions"].ToString().Trim().ToUpper());
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
}
