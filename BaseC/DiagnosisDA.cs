using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class DiagnosisDA
    {
        string sConString = "";
        Hashtable HshIn;
        Hashtable hstOutput;
        DAL.DAL DlObj;

        public DiagnosisDA(string Constring)
        {
            sConString = Constring;
            DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }

        public string GetDoctorId(int HospitalId, int UserId)
        {
            BaseC.EMRMasters.EMRColorLegends EMRColorLegends = new BaseC.EMRMasters.EMRColorLegends(sConString);
            SqlDataReader drr = EMRColorLegends.getEmployeeId(HospitalId, UserId);
            drr.Read();
            string[] DoctorId = drr[0].ToString().Split(new char[] { ',' });
            drr.Close();
            return DoctorId[0].ToString();
        }
        public string getNameFromDiagnosisID(string ICDID)
        {
            string strDesc = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvICDCode", ICDID);
                string strSQL = "Select Description	From ICD9SubDisease WITH (NOLOCK) Where ICDCode=@chvICDCode AND Active = 1 ";

                ds = objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strDesc = Convert.ToString(ds.Tables[0].Rows[0]["Description"]);
                }
                return strDesc;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public StringBuilder BindSubjective(int RegNo, StringBuilder sb)
        //{
        //   
        //    Hashtable hsSub = new Hashtable();
        //    hsSub = new Hashtable();
        //    hsSub.Add("@intRegistrationNo", RegNo);
        //    StringBuilder sbSql = new StringBuilder();
        //    sbSql.Append("select dbo.GetPatientName(0,FirstName,MiddleName,LastName) PatientName, dbo.UdfCurrentAgeGender(Id, getdate()) Age,");
        //    sbSql.Append(" DateOfBirth DOB,dbo.GetMaritalStatus(MaritalStatus) MaritalStatus");
        //    //sbSql.Append(" when 2 then 'Divorced' when 3 then 'Married' when 4 then 'Single' when 5 then 'Widowed' else '' end");
        //    sbSql.Append(" FROM Registration WHERE (RegistrationNo = @intRegistrationNo)");
        //    ds = DlObj.FillDataSet(CommandType.Text, sbSql.ToString(), hsSub);
        //    //Header ////
        //    sb.Append("<table style=font-family:Arial width=748 border=0 cellpadding=0 cellspacing=0>");//<tr><td width=100% align=right><font size=5>" + ds.Tables[0].Rows[0]["PatientName"].ToString() + "&nbsp;&nbsp;</font></td></tr>");
        //    ///Subjective Patient's Information///
        //    //sb.Append("<tr><td style=height:30  colspan=2><font size=5>Subjective</font></td></tr>");
        //    //sb.Append("<tr><td colspan=2><font size=2>Patient is " + ds.Tables[0].Rows[0]["Age"].ToString() + ", " + ds.Tables[0].Rows[0]["MaritalStatus"].ToString() + ", born on " + Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"]).ToString("dd/MM/yyyy") + ".</font></td></tr>");
        //    return sb;
        //}
        // public StringBuilder BindProblems(int RegId, int HospitalId, int EncounterId, StringBuilder sb)
        // {
        //     Hashtable hsProblems = new Hashtable();
        //     hsProblems.Add("@inyHospitalLocationID", HospitalId);
        //     hsProblems.Add("@intRegistrationId", RegId);
        //     hsProblems.Add("@intEncounterId", EncounterId);
        //     ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", hsProblems);
        //     if (ds.Tables[0].Rows.Count > 0)
        //         sb.Append("<tr><td style=height:30  colspan=2><font size=4><u>Problems</u></font></td></tr>");
        //     foreach (DataRow dr in ds.Tables[0].Rows)
        //     {
        //         sb.Append("<tr><td colspan=2><font size=2><li>" + dr["Problem"].ToString() + "</li></font></td></tr>");
        //     }
        //     return sb;
        // }
        // public StringBuilder BindVitals(int EncounterId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle)
        // {
        //     Hashtable hsTb = new Hashtable();
        //     //hsTb.Add("@inyHospitalLocationId", HospitalId);
        //     //hsTb.Add("@intRegistrationNo", RegNo);
        //     hsTb.Add("@intEncounterId", EncounterId);
        //     ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientEncounterVitals", hsTb);
        //     //if (ds.Tables[0].Rows.Count > 0)
        //     //sb.Append("<tr><td colspan=2 style=height:40><font size=5>Objective</font></td></tr>");
        //     string BeginList = "", EndList = "", sBegin = "", sEnd = "";
        //     if (ds.Tables[0].Rows.Count > 0)
        //     {
        //         sb.Append(sbTemplateStyle);
        //         if (drTemplateListStyle != null)
        //         {
        //             if (drTemplateListStyle["TemplateListStyle"].ToString() == "1")
        //             { BeginList = "<ul>"; EndList = "</ul>"; sBegin = "<li>"; sEnd = "</li>"; }
        //             else if (drTemplateListStyle["TemplateListStyle"].ToString() == "2")
        //             { BeginList = "<ol>"; EndList = "</ol>"; sBegin = "<li>"; sEnd = "</li>"; }
        //         }
        //         sb.Append("<br />");
        //         sb.Append(BeginList);
        //     }
        //     String vitals = "";
        //     string date = string.Empty;
        //     for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //     {
        //         if (i + 1 != ds.Tables[0].Rows.Count)
        //         {
        //             date = ds.Tables[0].Rows[i + 1]["VitalDate"].ToString();
        //         }
        //         else
        //             date = "";
        //         if (ds.Tables[0].Rows[i]["VitalDate"].ToString() == date)
        //         {
        //             vitals += ds.Tables[0].Rows[i]["VitalSignName"].ToString() + ": " + ds.Tables[0].Rows[i]["VitalValue"].ToString() + ", ";
        //         }
        //         else
        //         {
        //             vitals += ds.Tables[0].Rows[i]["VitalSignName"].ToString() + ": " + ds.Tables[0].Rows[i]["VitalValue"].ToString();

        //             sb.Append(sBegin + ds.Tables[0].Rows[i]["VitalDate"].ToString() + ": " + vitals + sEnd );
        //             vitals = "";
        //         }
        //     }
        //     sb.Append(EndList);
        //     return sb;
        // }
        // public StringBuilder BindAssessments(int RegId, int HospitalId, int EncounterId, Int16 UserId, string DocId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle)
        // {
        //     StringBuilder sbChronic = new StringBuilder();
        //     StringBuilder sbPrimary = new StringBuilder();
        //     StringBuilder sbSeconday = new StringBuilder();           
        //     UInt16 pF = 0;
        //     UInt16 sF = 0;
        //     UInt16 cF = 0;
        //     Hashtable hsAss = new Hashtable();
        //     hsAss.Add("@intRegistrationId", RegId);
        //     //hsAss.Add("@intDoctorId", DocId);
        //     hsAss.Add("@inyHospitalLocationID", HospitalId);
        //     hsAss.Add("@intEncounterId", EncounterId);
        //     ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", hsAss);

        //     string BeginList = "", EndList = "", sBegin = "", sEnd = "";
        //     if (ds.Tables[0].Rows.Count > 0)
        //     {
        //         sb.Append(sbTemplateStyle);
        //         if (drTemplateListStyle != null)
        //         {
        //             if (drTemplateListStyle["TemplateListStyle"].ToString() == "1")
        //             { BeginList = "<ul>"; EndList = "</ul>"; sBegin = "<li>"; sEnd = "</li>"; }
        //             else if (drTemplateListStyle["TemplateListStyle"].ToString() == "2")
        //             { BeginList = "<ol>"; EndList = "</ol>"; sBegin = "<li>"; sEnd = "</li>"; }
        //         }
        //         //sb.Append("<br />");
        //         //sb.Append(BeginList);
        //         sbChronic.Append(BeginList);
        //         sbPrimary.Append(BeginList);
        //         sbSeconday.Append(BeginList);
        //     }


        //     foreach (DataRow dr in ds.Tables[0].Rows)
        //     {
        //         if (dr["IsChronic"].ToString() == "True")
        //         {
        //             cF = 1;
        //             if (dr["ICDCode"].ToString() != "")
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
        //                 else
        //                     sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
        //             }
        //             else
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
        //                 else
        //                     sbChronic.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["DiagnosisType"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbChronic.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
        //                 else
        //                     sbChronic.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["OnsetDate"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbChronic.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
        //                 else
        //                     sbChronic.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
        //             }
        //             if (Convert.ToString(dr["Location"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbChronic.Append(" Location : " + dr["Location"].ToString() + ", ");
        //                 else
        //                     sbChronic.Append(" Location : " + dr["Location"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //                 sbChronic.Append(" Condition : ");
        //             if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
        //             {
        //                 if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
        //                     sbChronic.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
        //                 else
        //                     sbChronic.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
        //             }
        //             if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
        //             {
        //                 if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //                     sbChronic.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
        //                 else
        //                     sbChronic.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
        //             }
        //             if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //             {
        //                 sbChronic.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
        //             }

        //             if (Convert.ToString(dr["Remarks"]) != "")
        //                 sbChronic.Append("<br /> Remarks : " + dr["Remarks"].ToString());
        //             sbChronic.Append(sEnd);
        //         }
        //         else if (dr["PrimaryDiagnosis"].ToString() == "True" && dr["IsChronic"].ToString() == "False")
        //         {
        //             pF = 1;
        //             if (dr["ICDCode"].ToString() != "")
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
        //                 else
        //                     sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
        //             }
        //             else
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
        //                 else
        //                     sbPrimary.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["DiagnosisType"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbPrimary.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
        //                 else
        //                     sbPrimary.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["OnsetDate"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbPrimary.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
        //                 else
        //                     sbPrimary.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
        //             }
        //             if (Convert.ToString(dr["Location"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbPrimary.Append(" Location : " + dr["Location"].ToString() + ", ");
        //                 else
        //                     sbPrimary.Append(" Location : " + dr["Location"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //                 sbPrimary.Append(" Condition : ");
        //             if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
        //             {
        //                 if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
        //                     sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
        //                 else
        //                     sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
        //             }
        //             if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
        //             {
        //                 if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //                     sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
        //                 else
        //                     sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
        //             }
        //             if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //             {
        //                 sbPrimary.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
        //             }

        //             if (Convert.ToString(dr["Remarks"]) != "")
        //                 sbPrimary.Append("<br /> Remarks : " + dr["Remarks"].ToString());
        //             sbPrimary.Append(sEnd);
        //         }
        //         else if (dr["PrimaryDiagnosis"].ToString() == "False" && dr["IsChronic"].ToString() == "False")
        //         {
        //             sF = 1;
        //             if (dr["ICDCode"].ToString() != "")
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "), ");
        //                 else
        //                     sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + " (ICD Code:" + dr["ICDCode"].ToString() + "). ");
        //             }
        //             else
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisType"]) != "") || (Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + ", ");
        //                 else
        //                     sbSeconday.Append(sBegin + dr["ICDDescription"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["DiagnosisType"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["OnsetDate"]) != "") || (Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbSeconday.Append(" Type : " + dr["DiagnosisType"].ToString() + ", ");
        //                 else
        //                     sbSeconday.Append(" Type : " + dr["DiagnosisType"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["OnsetDate"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["Location"]) != "") || (Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbSeconday.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ", ");
        //                 else
        //                     sbSeconday.Append(" Onset Date : " + dr["OnsetDate"].ToString() + ". ");
        //             }
        //             if (Convert.ToString(dr["Location"]) != "")
        //             {
        //                 if ((Convert.ToString(dr["DiagnosisCondition1"]) != "") || (Convert.ToString(dr["DiagnosisCondition2"]) != "") || (Convert.ToString(dr["DiagnosisCondition3"]) != ""))
        //                     sbSeconday.Append(" Location : " + dr["Location"].ToString() + ", ");
        //                 else
        //                     sbSeconday.Append(" Location : " + dr["Location"].ToString() + ". ");
        //             }

        //             if (Convert.ToString(dr["DiagnosisCondition1"]) != "" || Convert.ToString(dr["DiagnosisCondition2"]) != "" || Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //                 sbSeconday.Append(" Condition : ");
        //             if (Convert.ToString(dr["DiagnosisCondition1"]) != "")
        //             {
        //                 if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
        //                     sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ", ");
        //                 else
        //                     sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition1"]) + ". ");
        //             }
        //             if (Convert.ToString(dr["DiagnosisCondition2"]) != "")
        //             {
        //                 if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //                     sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ", ");
        //                 else
        //                     sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition2"]) + ". ");
        //             }
        //             if (Convert.ToString(dr["DiagnosisCondition3"]) != "")
        //             {
        //                 sbSeconday.Append(Convert.ToString(dr["DiagnosisCondition3"]) + ". ");
        //             }

        //             if (Convert.ToString(dr["Remarks"]) != "")
        //                 sbSeconday.Append("<br /> Remarks : " + dr["Remarks"].ToString());
        //             sbSeconday.Append(sEnd);
        //         }
        //     }
        //     if (cF == 1)
        //     {
        //         sb.Append("<br /><b>Chronic Diagnosis </b>");
        //         sbChronic.Append(EndList);
        //         sb.Append(sbChronic.ToString());
        //     }
        //     if (pF == 1)
        //     {
        //         sb.Append("<br /><b>Primary Diagnosis </b>");
        //         sbPrimary.Append(EndList);
        //         sb.Append(sbPrimary.ToString());
        //     }
        //     if (sF == 1)
        //     {
        //         sb.Append("<br /><b>Secondary Diagnosis </b>");
        //         sbSeconday.Append(EndList);
        //         sb.Append(sbSeconday.ToString());
        //     }
        //     return sb;
        // }
        //public StringBuilder BindOrders(int RegNo, int HospitalId, int EncounterId, Int16 UserId, string DocId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle)
        // {
        //     string BeginList = "", EndList = "", sBegin = "", sEnd = "";
        //     Hashtable hsOrd = new Hashtable();
        //     hsOrd.Add("@intRegistrationId", RegNo);           
        //     hsOrd.Add("@inyHospitalLocationID", HospitalId);
        //     hsOrd.Add("@intEncounterId", EncounterId);
        //     StringBuilder sbTemp = new StringBuilder();//UspEMRGetPreviousInvestigations
        //     ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", hsOrd);
        //     DataView dvmain = new DataView(ds.Tables[0]);
        //     dvmain.RowFilter = "ServiceType Not in ( 'CL','VS','VF','R','RT' )";
        //     dvmain.Sort = "DepartmentId,Id";
        //     DataTable dtmain = new DataTable();
        //     dtmain = dvmain.ToTable();

        //     if (dtmain.Rows.Count > 0)
        //     {
        //         sb.Append(sbTemplateStyle);
        //         if (drTemplateListStyle != null)
        //         {
        //             if (drTemplateListStyle["TemplateListStyle"].ToString() == "1")
        //             { BeginList = "<ul>"; EndList = "</ul>"; sBegin = "<li>"; sEnd = "</li>"; }
        //             else if (drTemplateListStyle["TemplateListStyle"].ToString() == "2")
        //             { BeginList = "<ol>"; EndList = "</ol>"; sBegin = "<li>"; sEnd = "</li>"; }
        //         }
        //         //sb.Append("<u>Orders And Procedures</u>");               
        //     }
        //    int DepartmentId = 0;
        //    DataTable dt = new DataTable();
        //    for (int i = 0; i < dtmain.Rows.Count; i++)
        //     {
        //         DataRow dr = dtmain.Rows[i] as DataRow;
        //         if (Convert.ToInt32(dr["DepartmentId"]) != DepartmentId)
        //         {
        //             sbTemp.Append("<br /><b>" + dr["DepartmentName"].ToString() + "</b> ");
        //             sbTemp.Append(BeginList);
        //             DepartmentId = Convert.ToInt32(dr["DepartmentId"]);
        //             DataView dv = new DataView(dtmain);
        //             dv.RowFilter = "DepartmentId =" + Convert.ToInt32( dr["DepartmentId"]);
        //             dt = dv.ToTable();
        //             for (int j = 0; j < dt.Rows.Count; j++)
        //             {
        //                 DataRow dr1 = dt.Rows[j] as DataRow;
        //                 if (dr["CPTCode"].ToString() != "")
        //                     sbTemp.Append(sBegin + dr1["ServiceName"].ToString() + " (CPT Code:" + dr1["CPTCode"].ToString() + ")" + sEnd);
        //                 else
        //                     sbTemp.Append(sBegin + dr1["ServiceName"].ToString() + sEnd);

        //                 //if (dr["stat"].ToString() != "")
        //                 //    sbTemp.Append(" " + dr1["stat"].ToString() + "</li>");
        //                 //else
        //                 //    sbTemp.Append(" </li>");
        //             }
        //             sbTemp.Append(EndList);
        //         }                
        //     }
        //     //foreach (DataRow dr in ds.Tables[0].Rows)
        //     //{
        //     //    //if (dr["Status"].ToString() == "Active")
        //     //    //{
        //     //        if (dr["CPTCode"].ToString() != "")
        //     //            sbTemp.Append("<li>" + dr["ServiceName"].ToString() + " (CPT Code:" + dr["CPTCode"].ToString() + ")");
        //     //        else
        //     //            sbTemp.Append("<li>" + dr["ServiceName"].ToString());

        //     //        if (dr["stat"].ToString() != "")
        //     //            sbTemp.Append(" " + dr["stat"].ToString() + "</li>");
        //     //        else
        //     //            sbTemp.Append(" </li>");
        //     //    //}
        //     //}
        //     //if (sbTemp.ToString() != "")
        //     //    if (ds.Tables[0].Rows.Count > 0)
        //             sb.Append(sbTemp.ToString());
        //     return sb;
        // }
        // public StringBuilder BindMedication(int RegNo, int HospitalId, int EncounterId, Int16 UserId, string DocId, StringBuilder sb)
        // {
        //     Hashtable hsMed = new Hashtable();
        //     hsMed = new Hashtable();
        //     hsMed.Add("@inyHospitalLocationID", HospitalId);
        //     hsMed.Add("@intRegistrationNo", RegNo);
        //     hsMed.Add("@intEncounterNo", EncounterId);
        //     hsMed.Add("@intDoctorId", DocId);
        //     ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousMedicines", hsMed);
        //     if (ds.Tables[0].Rows.Count > 0)
        //         sb.Append("<tr><td style=height:30  colspan=2><font size=4><u>Prescription</u></font></td></tr>");
        //     foreach (DataRow dr in ds.Tables[0].Rows)
        //     {
        //         sb.Append("<tr><td colspan=2><font size=2><b><li>" + dr["Item Name"].ToString() + "</li></b></font></td></tr>");
        //         sb.Append("<tr><td colspan=2 ><font size=2>&nbsp;&nbsp;Take " + dr["Qty/Dose"].ToString() + " " + dr["Frequency"].ToString() + " for " + dr["Duration"].ToString() + ".</font></td></tr>");
        //     }
        //     return sb;
        // }
        // public StringBuilder BindAllergies(int RegId, StringBuilder sb, StringBuilder sbTemplateStyle, DataRow drTemplateListStyle)
        // {
        //     Hashtable hsAllergy = new Hashtable();
        //     StringBuilder sbDrugAllergy = new StringBuilder();
        //     StringBuilder sbOtherAllergy = new StringBuilder();
        //     string BeginList = "", EndList = "", sBegin = "", sEnd = "";
        //     int t = 0;
        //     hsAllergy.Add("@intRegistrationId", RegId);
        //     ds = DlObj.FillDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", hsAllergy);

        //     DataView dvDrug = new DataView(ds.Tables[0]);
        //     dvDrug.RowFilter = "AllergyType = 'Drug'";

        //     DataView dvOther = new DataView(ds.Tables[0]);
        //     dvOther.RowFilter = "AllergyType = 'Food Allergy'";

        //     if (ds.Tables[0].Rows.Count > 0)
        //     {
        //         sb.Append(sbTemplateStyle);//sb.Append("<u>Allergies</u>");
        //         if (drTemplateListStyle != null)
        //         {
        //             if (drTemplateListStyle["TemplateListStyle"].ToString() == "1")
        //             { BeginList = "<ul>"; EndList = "</ul>"; sBegin = "<li>"; sEnd = "</li>"; }
        //             else if (drTemplateListStyle["TemplateListStyle"].ToString() == "2")
        //             { BeginList = "<ol>"; EndList = "</ol>"; sBegin = "<li>"; sEnd = "</li>"; }
        //         }
        //     }
        //     if (ds.Tables[1].Rows.Count > 0)
        //     {
        //         if (Convert.ToString(ds.Tables[1].Rows[0]["NoAllergies"]) != "True")
        //         {
        //             foreach (DataRowView dr in dvDrug)
        //             {
        //                 if (t == 0)
        //                 {
        //                     sbDrugAllergy.Append("<br /><br /><b>Drug Allergies:</b> " + BeginList);
        //                     t = 1;
        //                 }
        //                 sbDrugAllergy.Append(" " + sBegin + dr["AllergyName"].ToString() + " (Generic: " + dr["Generic_Name"].ToString() + ")");
        //                 if (Convert.ToString(dr["AllergyDate"]) != "")
        //                     sbDrugAllergy.Append(", Onset Date: " + dr["AllergyDate"].ToString());
        //                 if (Convert.ToString(dr["Reaction"]) != "")
        //                 {
        //                     if (Convert.ToString(dr["Intolerance"]) == "False" && (Convert.ToString(dr["Remarks"]) == ""))
        //                         sbDrugAllergy.Append(", Reaction: " + dr["Reaction"].ToString() + ".");
        //                     else
        //                         sbDrugAllergy.Append(", Reaction: " + dr["Reaction"].ToString() + "");
        //                 }
        //                 if (Convert.ToString(dr["Intolerance"]) == "True")
        //                 {
        //                     if (Convert.ToString(dr["Remarks"]) == "")
        //                         sbDrugAllergy.Append(", Intolerable.");
        //                     else
        //                         sbDrugAllergy.Append(", Intolerable");
        //                 }
        //                 if (Convert.ToString(dr["Remarks"]) != "")
        //                     sbDrugAllergy.Append(", Remarks: " + Convert.ToString(dr["Remarks"]) + ".");
        //                 sbDrugAllergy.Append(sEnd);
        //             }
        //             sbDrugAllergy.Append(EndList);
        //         }
        //         else
        //         {
        //             sbDrugAllergy.Append("<br /><b>Drug Allergies:</b> ");
        //             sbDrugAllergy.Append(BeginList + sBegin + " No allergies." + sEnd + EndList);
        //         }
        //     }
        //     t = 0;
        //     foreach (DataRowView dr in dvOther)
        //     {
        //         if (t == 0)
        //         {
        //             sbOtherAllergy.Append("<br /><br /><b>Other Allergies:</b> " + BeginList);
        //             t = 1;
        //         }
        //         sbOtherAllergy.Append(" " + sBegin + dr["AllergyName"].ToString());
        //         if (Convert.ToString(dr["AllergyDate"]) != "")
        //             sbOtherAllergy.Append(", Onset Date: " + dr["AllergyDate"].ToString());
        //         if (Convert.ToString(dr["Reaction"]) != "")
        //         {
        //             if (Convert.ToString(dr["Intolerance"]) == "False" && (Convert.ToString(dr["Remarks"]) == ""))
        //                 sbOtherAllergy.Append(", Reaction: " + dr["Reaction"].ToString() + ".");
        //             else
        //                 sbOtherAllergy.Append(", Reaction: " + dr["Reaction"].ToString() + "");
        //         }
        //         if (Convert.ToString(dr["Intolerance"]) == "True")
        //         {
        //             if (Convert.ToString(dr["Remarks"]) == "")
        //                 sbOtherAllergy.Append(", Intolerable.");
        //             else
        //                 sbOtherAllergy.Append(", Intolerable");
        //         }
        //         if (Convert.ToString(dr["Remarks"]) != "")
        //             sbOtherAllergy.Append(", Remarks: " + Convert.ToString(dr["Remarks"]) + ".");
        //         sbOtherAllergy.Append(sEnd);
        //     }
        //     sbOtherAllergy.Append(EndList);
        //     sb.Append(sbDrugAllergy);
        //     sb.Append(sbOtherAllergy);
        //     return sb;
        // }

        // public int InsertAssessments(BaseC.DiagnosisBL bl)
        // {
        //     DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //     HshIn = new Hashtable();
        //     houtPara = new Hashtable();
        //     HshIn.Add("@inyHospitalLocationId", bl.HospitalLocationId);
        //     HshIn.Add("@intRegistrationNo", bl.RegistrationNo);
        //     HshIn.Add("@intDoctorId", bl.DoctorId);
        //     HshIn.Add("@intEncounterId", bl.EncounterId);
        //     HshIn.Add("@xmlDiagnosisDetails", bl.XmlData);
        //     HshIn.Add("@intEncodedBy", bl.EncodedBy);
        //     houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
        //     Int16 ret = (Int16)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSavePatientDiagnosis", HshIn, houtPara);
        //     return ret;
        // }
        // public int UpdateAssessments(int id, string Description, bool prmy, int stusId, string rmks, string resolvedDate, BaseC.DiagnosisBL bl)
        // {
        //     DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //     HshIn = new Hashtable();
        //     houtPara = new Hashtable();
        //     HshIn.Add("@Id", id);
        //     HshIn.Add("@Description", Description);
        //     HshIn.Add("@Primary", prmy);
        //     HshIn.Add("@DiaStatus", stusId);
        //     HshIn.Add("@Remarks", rmks);
        //     HshIn.Add("@ResolvedDate", resolvedDate);
        //     HshIn.Add("@inyHospitalLocationId", bl.HospitalLocationId);
        //     HshIn.Add("@intRegistrationNo", bl.RegistrationNo);
        //     HshIn.Add("@intDoctorId", bl.DoctorId);
        //     HshIn.Add("@EncounterId", bl.EncounterId);
        //     HshIn.Add("@intEncodedBy", bl.EncodedBy);
        //     HshIn.Add("@xmlDiagnosisDetails", bl.XmlData);
        //     houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
        //     Int16 ret = (Int16)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateDiagnosisDetail", HshIn, houtPara);
        //     return ret;
        // }
        // public SqlDataReader DefautlLegendsValues(Int16 iHospitalLocationID, Int32 iDoctorID, String sFieldID)
        // {
        //     DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //     HshIn = new Hashtable();
        //     HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
        //     HshIn.Add("@intDoctorID", iDoctorID);
        //     HshIn.Add("@chvFieldID", sFieldID);

        //     SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspEMRGetColorLegend", HshIn);
        //     return dr;
        // }

        //Added on 17th March 2010. \\\\\Mahbub
        public string SaveFavouriteDiagnosis(int iDoctorID, int iICDID, int iUserid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@intDoctorId", iDoctorID);
                HshIn.Add("@intICDId", iICDID);
                HshIn.Add("@intEncodedBy", iUserid);

                hstOutput.Add("@chvOutputMessage", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "UspEMRSaveFavDiagnosis", HshIn, hstOutput);
                return hstOutput["@chvOutputMessage"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //if (i == 1)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public bool DeleteFavouriteDiagnosis(int iDoctorID, int iICDID, int iUserid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                int i = 0;
                HshIn = new Hashtable();

                HshIn.Add("@intDoctorId", iDoctorID);
                HshIn.Add("@intICDId", iICDID);
                HshIn.Add("@intEncodedBy", iUserid);

                i = objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "UspEMRDeleteFavDiagnosis", HshIn);
                if (i == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool DeleteFavouriteProvDiagnosis(int iDoctorID, int ID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                int i = 0;
                HshIn = new Hashtable();

                HshIn.Add("@intDoctorId", iDoctorID);
                HshIn.Add("@intProvDiagID", ID);

                i = objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "UspEMRDeleteFavProvDiagnosis", HshIn);
                if (i == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet BindCategory()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "SELECT GroupId, GroupName, GroupType FROM ICD9Group WITH (NOLOCK) where active=1 ORDER BY GroupName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindSubCategory(string GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intGroupId", GroupId);

                string sqlstr = "SELECT SubGroupId, SubGroupName FROM ICD9SubGroup WITH (NOLOCK) Where GroupId =@intGroupId and active=1 ORDER BY SubGroupName";
                return objDl.FillDataSet(CommandType.Text, sqlstr, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet BindDisease(string Code)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chvDiseaseCode", Code);

                return objDl.FillDataSet(CommandType.Text, "select DiseaseId ,DiseaseName from ICD9Disease WITH (NOLOCK) where DiseaseCode=@chvDiseaseCode and active=1 ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet BindHospitalDisease(string DiseaseCode)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@chvDiseaseCode", DiseaseCode);

                return objDl.FillDataSet(CommandType.Text, "select ICDId,GroupId,SubGroupId,DiseaseId,ICDCode,Description,Active,Case When Active=1 Then 'Active' else 'In-Active' End Status from ICD9SubDisease WITH (NOLOCK) where DiseaseCode=@chvDiseaseCode", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveUpdateDiagnosismaster(int ICDId, int GroupId, int SubGroupId, int DiseaseId, string ICDCode, string Description, int Status)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@intICDId", ICDId);
                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@inSubGroupId", SubGroupId);
                HshIn.Add("@intDiseaseId", DiseaseId);
                HshIn.Add("@chvICDCode", ICDCode);
                HshIn.Add("@chvDescription", Description);
                HshIn.Add("@intStatus", Status);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "USPEMRSaveUpdateDiagnosismaster", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet BindDiagnosis(int CatId, int SubCatId, string etext)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intGroupId", CatId);
                HshIn.Add("@intSubGroupId", SubCatId);
                if (etext.ToString().Trim() != "")
                {
                    HshIn.Add("@chvSearchCriteria", "%" + etext + "%");
                }
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet BindFavoriteDiagnosis(int DoctorId, int CatId, int SubCatId, string etext)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intGroupId", CatId);
                HshIn.Add("@intSubGroupId", SubCatId);
                if (etext.ToString().Trim() != "")
                {
                    HshIn.Add("@chvSearchCriteria", "%" + etext + "%");
                }
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavoriteDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet BindFavouriteProvDiagnosis(string txt, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                string strSearchCriteria = string.Empty;
                strSearchCriteria = "%" + txt + "%";

                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvSearchCriteria", strSearchCriteria.ToString());
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavoriteProvDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetDoctorName(int HospId, int FacilityId, int RegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                string strsql = "";
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);


                strsql = "SELECT DISTINCT epd.DoctorId,dbo.GetDoctorName(epd.DoctorId) AS DoctorName " +
                " FROM Registration pr WITH (NOLOCK) " +
                " INNER JOIN Encounter enc WITH (NOLOCK) ON pr.Id=enc.RegistrationId   " +
                " INNER JOIN EMRPatientDiagnosisDetails epd WITH (NOLOCK) ON enc.RegistrationId=epd.RegistrationId and enc.DoctorId=epd.DoctorId " +
                " INNER JOIN Employee emp WITH (NOLOCK) ON epd.DoctorId=emp.ID " +
                " WHERE epd.RegistrationId = @intRegistrationId and emp.Active=1 and enc.FacilityId =@intFacilityId and enc.HospitalLocationId=@inyHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet selectDiscriptionandICDID(string EncIcdCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvICDCode", EncIcdCode);

                string strSQL = "SELECT ICDID, Description FROM ICD9SubDisease WITH (NOLOCK) WHERE ICDCode=@chvICDCode";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet selectDiscription(string EncIcdCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvICDCode", EncIcdCode);

                string cmdstr = "SELECT [Description] FROM ICD9SubDisease WITH (NOLOCK) WHERE [ICDCode]=@chvICDCode ";
                return objDl.FillDataSet(CommandType.Text, cmdstr, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet selectICDSubDiseas(string EncIcdCode)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@icdcode", EncIcdCode);
                string cmdstr = "SELECT [Description] FROM ICD9SubDisease WITH (NOLOCK) WHERE [ICDCode]=@icdcode ";
                return objDl.FillDataSet(CommandType.Text, cmdstr, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet SelectDiagnosispatientdtl(int icdid, int RegId, int EncId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inticdid", icdid);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncId);
            string strSQL = "select icdid from EMRPatientDiagnosisDetails WITH (NOLOCK) where EncounterId=@intEncounterId and RegistrationId=@intRegistrationId and PrimaryDiagnosis=1 and Active=1 ";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet ISDiagnosesExits(int HospId, int RegId, int EncId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientDiagnosis(int HospId, int FacilityId, int RegId, int EncId, int DoctorId, int GroupId, int SubGroupid,
            string Daterang, string Fdate, string Tdate, string SearchCriteriya, bool Distinct, int StatusId, string Visittype, bool Chronic, int DiagId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDoctorID", DoctorId);
                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@intSubGroupId", SubGroupid);
                HshIn.Add("@chvDateRange", Daterang);
                HshIn.Add("@chrFromDate", Fdate);
                HshIn.Add("@chrToDate", Tdate);
                HshIn.Add("@chvSearchCriteria", SearchCriteriya);
                HshIn.Add("@bitDistinctRecords", Distinct);
                HshIn.Add("@intStatusId", StatusId);
                HshIn.Add("@chvVisitTypeId", Visittype);
                HshIn.Add("@bitChronic", Chronic);
                HshIn.Add("@intDiagId", DiagId);


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindDiagnosistype()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {

                string strsql = "Select TypeId, Description From EMRDiagnosisTypeMaster WITH (NOLOCK) Where Active=1";
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientDiagnosis_ByIndentId(int HospId, int FacilityId, int RegId, int EncId, int DoctorId, int GroupId, int SubGroupid,
            string Daterang, string Fdate, string Tdate, string SearchCriteriya, bool Distinct, int StatusId, string Visittype, bool Chronic, int DiagId, int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@intDoctorID", DoctorId);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@intSubGroupId", SubGroupid);
            HshIn.Add("@chvDateRange", Daterang);
            HshIn.Add("@chrFromDate", Fdate);
            HshIn.Add("@chrToDate", Tdate);
            HshIn.Add("@chvSearchCriteria", SearchCriteriya);
            HshIn.Add("@bitDistinctRecords", Distinct);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@chvVisitTypeId", Visittype);
            HshIn.Add("@bitChronic", Chronic);
            HshIn.Add("@intDiagId", DiagId);
            HshIn.Add("@IndentId", IndentId);

            // Change by kuldeep kumar 22/01/2021 
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis_ByIndent", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindDiagnosisCondition()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string strSql = "Select StatusId, Description From EMRDiagnosisStatusMaster WITH (NOLOCK) Where Active=1";
                return objDl.FillDataSet(CommandType.Text, strSql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet CheckDuplicateProblem(int HospitalId, int RegId, int EncId, int DiagnosisId, bool Chronic)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            string strSQL = "";
            HshIn.Add("@inyHospitalLocationID", HospitalId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@inticdid", DiagnosisId);
            try
            {

                if (Chronic == false)
                {
                    strSQL = "select icdid from EMRPatientDiagnosisDetails WITH (NOLOCK) where EncounterId=@intEncounterId and RegistrationId=@intRegistrationId and icdid = @inticdid and ischronic=0 and active=1 and HospitalLocationId=@inyHospitalLocationID ";
                }
                else
                {
                    strSQL = "select icdid from EMRPatientDiagnosisDetails WITH (NOLOCK) where RegistrationId=@intRegistrationId and icdid =@inticdid and ischronic=1 and active=1";
                }
                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet CheckDuplicateDiagnosis(int HospitalId, int RegId, int EncId, int DiagnosisId, bool Chronic)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            string strSQL = "";
            HshIn.Add("@inyHospitalLocationID", HospitalId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@inticdid", DiagnosisId);

            try
            {

                if (Chronic == false)
                {
                    strSQL = "select icdid from EMRPatientDiagnosisDetails where EncounterId=@intEncounterId and RegistrationId=@intRegistrationId and icdid =@inticdid and HospitalLocationId=@inyHospitalLocationID and ischronic=0 and active=1";
                }
                else
                {
                    strSQL = "select icdid from EMRPatientDiagnosisDetails where  RegistrationId=@intRegistrationId and icdid =@inticdid and ischronic=1 and active=1";
                }
                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string EMRSavePatientDiagnosis(int HospId, int LoginFacilityId, int RegId, int EncId, string DoctorId, int PageId, string objXML, string objXMLPatientAlert, int UserId,
          bool PullDiagnosis, bool ShowNote, int MRDCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@xmlDiagnosisDetails", objXML.ToString());
                HshIn.Add("@xmlPatientAlerts", objXMLPatientAlert);
                HshIn.Add("@intEncodedBy", UserId);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshIn.Add("@bitIsPullForward", PullDiagnosis);
                HshIn.Add("@IsShowNote", ShowNote);
                HshIn.Add("@bitMRDCode", MRDCode);

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientDiagnosis", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void EMRMUDLogSaveDiagnosis(int HospId, int RegId, int EncId, int DoctorId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intEncodedBy", UserId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogSaveDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetICD9SubDisease(string IcdCode)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn = new Hashtable();
                HshIn.Add("@chvICDCode", IcdCode);

                string strSql = "SELECT ICDID, Description FROM ICD9SubDisease WITH (NOLOCK) WHERE ICDCode=@chvICDCode ";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetICD10SubGroup(string GroupId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intGroupId", GroupId);

                string strSql = "SELECT SubGroupId,GroupId,SubGroupName FROM ICD9SubGroup WITH (NOLOCK) WHERE GroupId=@intGroupId AND ACTIVE=1 order by SubGroupName";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetICD10Disease(string GroupId, string SubGroupId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@chvGroupId", GroupId);
                HshIn.Add("@chvSubGroupId", SubGroupId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetICD10Diseases", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetICD10SubDiseases(string DiseaseId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn = new Hashtable();
                HshIn.Add("@intDiseaseId", DiseaseId);

                string strSql = "SELECT ICDID, GroupId,SubGroupId,DiseaseId,DiseaseCode,ICDCode,Description, CASE ValidForClinicalUse WHEN 0 THEN 'N' ELSE '' END AS ValidForClinicalUse, CASE ValidForPrimaryDiagnosis WHEN 0 THEN 'N' ELSE '' END AS ValidForPrimaryDiagnosis, ISNULL(AgeRange,'') AS AgeRange, CASE Gender WHEN 'M' THEN 'Male' WHEN 'F' THEN 'Female' ELSE '' END AS Gender FROM ICD9SubDisease WITH (NOLOCK) WHERE DiseaseId=@intDiseaseId AND Active=1 order by Description";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public bool CheckValidForPrimaryDiagnosis(int iICDId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intICDId", iICDId);
                string strSql = "SELECT ISNULL(ValidForPrimaryDiagnosis,0) AS ValidForPrimaryDiagnosis FROM  ICD9SubDisease WITH (NOLOCK) WHERE ICDId=@intICDId AND Active=1";
                bool bResult = Convert.ToBoolean(objDl.ExecuteScalar(CommandType.Text, strSql, HshIn));
                return bResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string EMRSaveProvisionalDiagnosisSearchCodes(int HospitalLocationId, int DiagnosisSearchId, string DiagnosisSearchCode,
                                                    int Active, int UserId, string KeywordType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intDiagnosisSearchId", DiagnosisSearchId);
                HshIn.Add("@chvDiagnosisSearchCode", DiagnosisSearchCode);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvKeywordType", KeywordType);

                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspEMRSaveDiagnosisSearchKeys", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetProvisionalDiagnosisSearchCodes(int HospitalLocationId, int UserId, string KeywordType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvKeywordType", KeywordType);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDiagnosisSearchKeys", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string EMRSavePatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, string ProvisionalDiagnosis,
            int DiagnosisSearchId, int UserId, int ProvisionalDiagnosisId)
        {
            return EMRSavePatientProvisionalDiagnosis(HospitalLocationId, RegistrationId, EncounterId, ProvisionalDiagnosis,
            DiagnosisSearchId, UserId, ProvisionalDiagnosisId, 0, null, string.Empty, false, false, false, false, false);
        }

        public string EMRSavePatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, string ProvisionalDiagnosis,
            int DiagnosisSearchId, int UserId, int ProvisionalDiagnosisId, int ProviderId, DateTime? ChangeDate, string Remarks, bool bAdmitting, bool bProvisional, bool bFinal, bool bChronic, bool bDischarge)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvProvisionalDiagnosis", ProvisionalDiagnosis);
                HshIn.Add("@intDiagnosisSearchId", DiagnosisSearchId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intProvisionalDiagnosisId", ProvisionalDiagnosisId);
                if (ProviderId > 0)
                {
                    HshIn.Add("@intProviderId", ProviderId);
                }
                if (ChangeDate != null)
                {
                    HshIn.Add("@chvChangeDate", ChangeDate);
                }
                if (!Convert.ToString(Remarks).Equals(string.Empty))
                {
                    HshIn.Add("@chvRemarks", Remarks);
                }
                HshIn.Add("@bitAdmitting", bAdmitting);
                HshIn.Add("@bitProvisional ", bProvisional);
                HshIn.Add("@bitFinal ", bFinal);
                HshIn.Add("@bitChronic", bChronic);
                HshIn.Add("@bitDischarge", bDischarge);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspEMRSavePatientProvisionalDiagnosis", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn = new Hashtable();

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", UserId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientProvisionalDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, int UserId, int ProvisionalDiagnosisId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intProvisionalDiagnosisId", ProvisionalDiagnosisId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientProvisionalDiagnosis", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, int UserId,
                                int ProvisionalDiagnosisId, string sFromDate, string sToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@intProvisionalDiagnosisId", ProvisionalDiagnosisId);
            HshIn.Add("@chrGroupingDate", GroupingDate);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientProvisionalDiagnosis", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveFavouriteProvDiagnosis(int iDoctorID, string DiagnosisDescription, int iUserid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@intDoctorId", iDoctorID);
                HshIn.Add("@strProDiagnosis", DiagnosisDescription);
                HshIn.Add("@intEncodedBy", iUserid);

                hstOutput.Add("@chvOutputMessage", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "UspEMRSaveFavProvDiagnosis", HshIn, hstOutput);
                return hstOutput["@chvOutputMessage"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //if (i == 1)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }

        public int DeletePatientProvisionalDiagnosis(int ProvisionalDiagnosisId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intProvisionalDiagnosisId", ProvisionalDiagnosisId);
                int str = objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "UspDeletePatientProvisionalDiagnosis", HshIn);
                return str;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, int UserId, string sFromDate, string sToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn = new Hashtable();

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientProvisionalDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveMRDEncounterDeficiency(int EncounterId, string xmlMRDEncounterDeficiency, int HospId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@xmlMRDEncounterDeficiency", xmlMRDEncounterDeficiency);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspSaveMRDEncounterDeficiency", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string UpdateMRDEncounterCptCode(int detailId, int MrdCptId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@detailId", detailId);
            HshIn.Add("@MrdCptId", MrdCptId);

            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspUpdateMRDEncounterCptCode", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getMRDEncounterDeficiency(int EncounterId, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@OPIP", OPIP);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDEncounterDeficiency", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet CheckDiagnosisExcluded(int inyHospitalLocationId, int intFacilityId, int intregistrationId, string DiseaseCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intregistrationId", intregistrationId);

            HshIn.Add("@DiseaseCode", DiseaseCode);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckDiagnosisExcluded", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientProvisionalDiagnosisHistory(int HospitalLocationId, int RegistrationId, int FacilityId, int DoctorId, string chvVisitTypeId,
string chrFromDate, string chrToDate, string intEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@chvVisitTypeId", chvVisitTypeId);
            HshIn.Add("@chrFromDate", chrFromDate);
            HshIn.Add("@chrToDate", chrToDate);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientProvisionalDiagnosisHistory", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }




        public string EMRCopyPatientProvisionalDiagnosis(int HospitalLocationId, int RegistrationId, int EncounterId, int EncodedBy, string xmlID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlID", xmlID);

            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspEMRCopyPatientProvisionalDiagnosis", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string InsertUpdateOnlineDeathReg(int EncounterId, string OnlineDeathRegNo, string OnlineDeathRegDate, bool IsAnesthesiarelatedDeath, int HospId, int EncodedBy, bool IsExpire, bool IsNewBorn, string OnlineBirthRegNo, string OnlineBirhtRegDate, int RegistrationID, int DeliveryType, string DeliveryDatetime, int Weight, int Height, int Gender, int GestationalWeek, string OtherSpecification, bool IsDeathRelatedtoPregnancy, bool IsInfantDeath)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@OnlineDeathRegNo", OnlineDeathRegNo);
            HshIn.Add("@OnlineDeathRegDate", OnlineDeathRegDate);
            HshIn.Add("@IsAnesthesiarelatedDeath", IsAnesthesiarelatedDeath);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@OnlineBirthRegNo", OnlineBirthRegNo);
            HshIn.Add("@OnlineBirhtRegDate", OnlineBirhtRegDate);
            HshIn.Add("@IsExpire", IsExpire);
            HshIn.Add("@IsNewBorn", IsNewBorn);
            HshIn.Add("@RegistrationID", RegistrationID);
            HshIn.Add("@DeliveryType", DeliveryType);
            HshIn.Add("@DeliveryDatetime", DeliveryDatetime);
            HshIn.Add("@Weight", Weight);
            HshIn.Add("@Height", Height);
            HshIn.Add("@Gender", Gender);
            HshIn.Add("@GestationalWeek", GestationalWeek);
            HshIn.Add("@OtherSpecification", OtherSpecification);
            HshIn.Add("@bitIsDeathRelatedtoPregnancy", IsDeathRelatedtoPregnancy);
            HshIn.Add("@bitIsInfantDeath", IsInfantDeath);

            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspInsertUpdateOnlineDeathReg", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetOnlineBirthDeathRegDetail(int EncounterId, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOnlineDeathRegDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetGetMLCandLegalDetail(int EncounterId, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMLCandLegalDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDiagnosisList(int GroupId, int SubGroupId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@intSubGroupId", SubGroupId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDiagnosisList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

    }
}
