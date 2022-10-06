using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
namespace BaseC
{
    public class ICM
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable HshOut;
        DAL.DAL objDl;

        StringBuilder strSQL;

        public ICM(string Constring)
        {
            sConString = Constring;
        }

        public DataSet GetICMTemplateName(int iEncounterID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intEncounterID", iEncounterID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetICMTemplateNames", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveICMSummaryFormat(int iID, int iHospitalLocationID, string sSummaryName, string sSummaryNote, int iEncodedBy, string sEncodedDate)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intID", iID);
                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@chvSummaryName", sSummaryName);
                HshIn.Add("@chvSummaryNote", sSummaryNote);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@chrEncodedDate", sEncodedDate);
                HshIn.Add("@intLastUpdatedBy", iEncodedBy);
                HshIn.Add("@chrLastUpdatedDate", sEncodedDate);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMSummaryFormat", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveUpdateICMPatientSummary(int iID, int iHospitalLocationID, string sRegistrationID, string sEncounterID, int iFormatID,
                                                    string sPatietSummary, int iSignDoctorID, int iEncodedBy, string sEncodedDate, String DeathDate,
                                                    String DischargeDate, int FacilityId, string synopsis, string Addendum,
                                                    bool IsMultiDepartmentCase, int CaseId, string xmlDepartmentIds, int SignJuniorDoctorID,
                                                    int IsReviewRequired, string ReviewRemarks, int SignThirdDoctorId, int SignFourthDoctorId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intID", iID);
            HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intRegistrationID", sRegistrationID);
            HshIn.Add("@intEncounterID", sEncounterID);
            HshIn.Add("@intFormatID", iFormatID);
            HshIn.Add("@chvPatientSummary", sPatietSummary);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            //HshIn.Add("@chrEncodedDate", sEncodedDate);
            HshIn.Add("@intLastUpdatedBy", iEncodedBy);
            //HshIn.Add("@chrLastUpdatedDate", sEncodedDate);
            HshIn.Add("@intSignDoctorID", iSignDoctorID);
            HshIn.Add("@chrDischargeDate", DischargeDate);
            HshIn.Add("@chrDeathDate", DeathDate);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chrsynopsis", synopsis);
            HshIn.Add("@chvAddendum", Addendum);

            if (IsMultiDepartmentCase)
            {
                HshIn.Add("@bitIsMultiDepartmentCase", IsMultiDepartmentCase);
                HshIn.Add("@intCaseId", CaseId);
                HshIn.Add("@xmlDepartmentIds", xmlDepartmentIds);
            }
            HshIn.Add("@intSignJuniorDoctorID", SignJuniorDoctorID);

            if (IsReviewRequired > -1)
            {
                HshIn.Add("@bitIsReviewRequired", IsReviewRequired);

                HshIn.Add("@chvReviewRemarks", ReviewRemarks);
            }

            HshIn.Add("@intSignThirdDoctorId", SignThirdDoctorId);
            HshIn.Add("@intSignFourthDoctorId", SignFourthDoctorId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMPatientSummary", HshIn, HshOut);
            return HshOut;
        }

        public string SaveUpdateICMPatientSummaryNew(int iID, int iHospitalLocationID, string sRegistrationID, string sEncounterID, int iFormatID,
                                                    string sPatietSummary, int iSignDoctorID, int iEncodedBy, string sEncodedDate, String DeathDate,
                                                    String DischargeDate, int FacilityId, string synopsis, string Addendum,
                                                    bool IsMultiDepartmentCase, int CaseId, string xmlDepartmentIds, int SignJuniorDoctorID,
                                                    int IsReviewRequired, string ReviewRemarks, int SignThirdDoctorId, int SignFourthDoctorId, string Type)
        {
            SqlConnection con = new SqlConnection(sConString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "UspSaveUpdateICMPatientSummary";
            cmd.Parameters.Add("@intID", SqlDbType.Int).Value = iID;
            cmd.Parameters.Add("@intHospitalLocationID", SqlDbType.Int).Value = iHospitalLocationID;
            cmd.Parameters.Add("@intRegistrationID", SqlDbType.Int).Value = sRegistrationID;
            cmd.Parameters.Add("@intEncounterID", SqlDbType.Int).Value = sEncounterID;
            cmd.Parameters.Add("@intFormatID", SqlDbType.Int).Value = iFormatID;
            cmd.Parameters.Add("@chvPatientSummary", SqlDbType.NVarChar).Value = sPatietSummary;
            cmd.Parameters.Add("@intEncodedBy", SqlDbType.Int).Value = iEncodedBy;
            //cmd.Parameters.Add("@chrEncodedDate", SqlDbType.VarChar).Value = sEncodedDate;
            cmd.Parameters.Add("@intLastUpdatedBy", SqlDbType.Int).Value = iEncodedBy;
            //cmd.Parameters.Add("@chrLastUpdatedDate", SqlDbType.VarChar).Value = sEncodedDate;
            cmd.Parameters.Add("@intSignDoctorID", SqlDbType.Int).Value = iSignDoctorID;
            cmd.Parameters.Add("@chrDischargeDate", SqlDbType.VarChar).Value = DischargeDate;
            cmd.Parameters.Add("@chrDeathDate", SqlDbType.VarChar).Value = DeathDate;
            cmd.Parameters.Add("@intFacilityId", SqlDbType.Int).Value = FacilityId;
            cmd.Parameters.Add("@chrsynopsis", SqlDbType.VarChar).Value = synopsis;
            cmd.Parameters.Add("@chvAddendum", SqlDbType.VarChar).Value = Addendum;
            cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = Type;
            if (IsMultiDepartmentCase)
            {
                cmd.Parameters.Add("@bitIsMultiDepartmentCase", SqlDbType.Bit).Value = IsMultiDepartmentCase;
                cmd.Parameters.Add("@intCaseId", SqlDbType.Int).Value = CaseId;
                cmd.Parameters.Add("@xmlDepartmentIds", SqlDbType.Xml).Value = xmlDepartmentIds;
            }
            cmd.Parameters.Add("@intSignJuniorDoctorID", SqlDbType.Int).Value = SignJuniorDoctorID;
            if (IsReviewRequired > -1)
            {
                cmd.Parameters.Add("@bitIsReviewRequired", SqlDbType.Int).Value = IsReviewRequired;
                cmd.Parameters.Add("@chvReviewRemarks", SqlDbType.VarChar).Value = ReviewRemarks;
            }
            cmd.Parameters.Add("@intSignThirdDoctorId", SqlDbType.Int).Value = SignThirdDoctorId;
            cmd.Parameters.Add("@intSignFourthDoctorId", SqlDbType.Int).Value = SignFourthDoctorId;
            cmd.Parameters.Add("@chvErrorStatus", SqlDbType.VarChar, 500);
            cmd.Parameters["@chvErrorStatus"].Direction = ParameterDirection.Output;
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally

            {
                con.Close();
                con.Dispose();

            }
            string message = string.Empty;
            if (Convert.ToString(cmd.Parameters["@chvErrorStatus"].Value).Equals(""))
            {
                message = "";
            }
            else
            {
                message = cmd.Parameters["@chvErrorStatus"].Value as string;
            }
            return message;
        }

        public DataSet GetICMSummaryFormatName(int iHospitalLocationID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
            string sFormatName = "SELECT ID,FormatName FROM SummaryFormat WITH (NOLOCK) WHERE HospitalLocationID=@intHospitalLocationID";

            try
            {

                return objDl.FillDataSet(CommandType.Text, sFormatName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveUpdateICMPatientSummaryMS(int iID, int iHospitalLocationID, string sRegistrationID, string sEncounterID, int iFormatID,
                                                string sPatietSummary, int iSignDoctorID, int iEncodedBy, string sEncodedDate, String DeathDate,
                                                String DischargeDate, int FacilityId, string synopsis, string Addendum,
                                                bool IsMultiDepartmentCase, int CaseId, string xmlDepartmentIds)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intID", iID);
            HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intRegistrationID", sRegistrationID);
            HshIn.Add("@intEncounterID", sEncounterID);
            HshIn.Add("@intFormatID", iFormatID);
            HshIn.Add("@chvPatientSummary", sPatietSummary);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            //HshIn.Add("@chrEncodedDate", sEncodedDate);
            HshIn.Add("@intLastUpdatedBy", iEncodedBy);
            //HshIn.Add("@chrLastUpdatedDate", sEncodedDate);
            HshIn.Add("@intSignDoctorID", iSignDoctorID);
            HshIn.Add("@chrDischargeDate", DischargeDate);
            HshIn.Add("@chrDeathDate", DeathDate);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chrsynopsis", synopsis);
            HshIn.Add("@chvAddendum", Addendum);

            if (IsMultiDepartmentCase)
            {
                HshIn.Add("@bitIsMultiDepartmentCase", IsMultiDepartmentCase);
                HshIn.Add("@intCaseId", CaseId);
                HshIn.Add("@xmlDepartmentIds", xmlDepartmentIds);
            }

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMPatientSummaryMS", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSummaryFormatDetails(int iHospitalLocationID, int iID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intID", iID);
                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                string sFormatDetails = "SELECT ID,SummaryNote FROM SummaryFormat WITH (NOLOCK) WHERE ID=@intID AND HospitalLocationID=@intHospitalLocationID ";
                return objDl.FillDataSet(CommandType.Text, sFormatDetails, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientSummaryDetails(int iHospitalLocationID, string sRegistrationID, string sEncounterID, int ReportId, int iFacilityId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intRegistrationID", sRegistrationID);
                HshIn.Add("@intEncounterID", sEncounterID);
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intFacilityId", iFacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientDischargeSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetICMTemplateStyle(int iHospitalLocationID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                string sqlTemplateStyle = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
                + "TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
                + "SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
                + "FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber "
                + "FROM EMRTemplateStatic WITH (NOLOCK) where HospitalLocationId = @inyHospitalLocationID";
                return objDl.FillDataSet(CommandType.Text, sqlTemplateStyle, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSignDoctors(int iHospitalLocationID, int FacilityId)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intFacilityID", FacilityId);
                //string sqlSignDoctor = "SELECT e.ID, ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') +ISNULL(LastName,'')  AS DoctorName FROM  Employee e inner join EmployeeType t on e.EmployeeType = t.Id WHERE HospitalLocationId=@inyHospitalLocationID AND e.FacilityId=@intFacilityID  and t.EmployeeType='D' AND Designation IS NOT NULL  AND e.Active=1  order by  DoctorName";
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetICMSignDoctors", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSignDoctorName(int iHospitalLocationID, int iSignDoctorID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intSignDoctorID", iSignDoctorID);
                string sqlDoctorName = "SELECT ID, ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS SignDoctorName, Designation,Education,DD.UPIN,DM.DepartmentName FROM  Employee EMP WITH (NOLOCK) INNER JOIN DoctorDetails DD WITH (NOLOCK) ON EMP.ID=DD.DoctorId INNER JOIN DepartmentMain DM WITH (NOLOCK) ON EMP.DepartmentId=DM.DepartmentID where EMP.ID=@intSignDoctorID AND EMP.HospitalLocationId=@inyHospitalLocationID ";
                return objDl.FillDataSet(CommandType.Text, sqlDoctorName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateICMFinalize(int iHospitalLocationID, int iSummaryID, bool bFinalize, int iDoctorSignID, int lastChangeBy)
        {
            return UpdateICMFinalize(iHospitalLocationID, iSummaryID, bFinalize, iDoctorSignID, lastChangeBy, string.Empty);
        }
        public string UpdateICMFinalize(int iHospitalLocationID, int iSummaryID, bool bFinalize, int iDoctorSignID, int lastChangeBy, string Type)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intSummaryID", iSummaryID);
            HshIn.Add("@bitFinalize", bFinalize);
            HshIn.Add("@intDoctorSignID", iDoctorSignID);
            HshIn.Add("@intlastChangeBy", lastChangeBy);
            HshIn.Add("@Type", Type);
            HshIn.Add("@dtlastChangeDate", DateTime.UtcNow);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRFinalizePatientSummary", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


            //string sqlSummaryFinalize = "UPDATE EMRPatientSummaryDetails SET FINALIZE=@bitFinalize,SignDoctorID=@intDoctorSignID,LastChangedBy=@lastChangeBy,lastChangedDate=@lastChangeDate, FinalizeBy = @lastChangeBy, FinalizeDateTime = @lastChangeDate WHERE HospitalLocationID=@inyHospitalLocationID AND SummaryID=@intSummaryID";
            //objDl.ExecuteNonQuery(CommandType.Text, sqlSummaryFinalize, HshIn);

        }
        public DataSet GetICMNoCurrentMedication(int iHospitalLocationID, int iMedicationID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intMedicationID", iMedicationID);
            string sqlDoctorName = " SELECT ISNULL(NoCurrentMedication,0) AS NoCurrentMedication FROM  Encounter WITH (NOLOCK) WHERE ID=@intMedicationID AND HospitalLocationId=@inyHospitalLocationID";

            try
            {

                return objDl.FillDataSet(CommandType.Text, sqlDoctorName, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDischargeSummaryAuditTrail(int FacilityId, string EncounterNo, Int64 RegistrationNo, string PatientName, int DeFinalizeRecommendBy,
                                                   DateTime FromDate, DateTime ToDate)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@intDeFinalizeRecommendBy", DeFinalizeRecommendBy);
            HshIn.Add("@chvFromDate", fDate);
            HshIn.Add("@chvToDate", tDate);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDischargeSummaryAuditTrail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientLabResult(int iHospitalLocationID, int iRegistrationID, string sFromDate, string sToDate)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intRegistrationID", iRegistrationID);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetInvestigationsResult", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientInvestigationResult(int iHospitalLocationID, int iFacilityId, int iEncounterID, string sFromDate, string sToDate)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@inEncId", iEncounterID);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetInvestigationsResult", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getHospitalPerformance(DateTime FromDate, DateTime ToDate, int UserId, int HospId, int FacilityId)
        {
            HshIn = new Hashtable();


            string fDate = FromDate.ToString("yyyy/MM/dd");
            string tDate = ToDate.ToString("yyyy/MM/dd");

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@FDate", fDate);
                HshIn.Add("@TDate", tDate);
                HshIn.Add("@loginur", UserId);
                HshIn.Add("@Locid", HospId);
                HshIn.Add("@FacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "DOC_PERFORMANCE", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable FindICMAdminUser(int UserId, int HospId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intUserID", UserId);
            HshIn.Add("@inyHospitalLocationID", HospId);
            HshOut.Add("@bitIsAdmin", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "FindICMAdminUser", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveDischargeSummaryAuditTrail(int SummaryId, int EncodedBy, int DeFinalizeRecommendBy, string DeFinalizeReason)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intSummaryId", SummaryId);
            HshIn.Add("@intDeFinalizeBy", EncodedBy);
            HshIn.Add("@intDeFinalizeRecommendBy", DeFinalizeRecommendBy);
            HshIn.Add("@chvDeFinalizeReason", DeFinalizeReason);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDischargeSummaryAuditTrail", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getMISDepartment(int HospId, string Type)
        {
            HshIn = new Hashtable();

            string strsql = "";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@Locid", HospId);
                if (Type == "Sur")
                {
                    strsql = "select distinct ds.SubDeptId ,SubName from itemofservice ios WITH (NOLOCK) " +
                         " inner join departmentsub ds WITH (NOLOCK) on ios.SubDeptId  = ds.SubDeptId " +
                         " where ds.type = 'S' and ios.Active = 1 and ios.HospitalLocationId=@Locid  " +
                         " order by ds.SubName ";
                }
                if (Type == "Pro")
                {
                    strsql = "select distinct ds.SubDeptId ,SubName from itemofservice ios WITH (NOLOCK) " +
                         " inner join departmentsub ds WITH (NOLOCK) on ios.SubDeptId  = ds.SubDeptId  " +
                         " where ds.type = 'P' and ios.Active = 1 and ios.HospitalLocationId=@Locid  " +
                         "  order by ds.SubName ";
                }

                if (Type == "Doc")
                {
                    strsql = "select distinct spm.Code AS SubDeptId,spm.Name AS SubName from Employee emp WITH (NOLOCK)  " +
                         " INNER JOIN DoctorDetails dd WITH (NOLOCK) on emp.ID =dd.DoctorId  INNER JOIN SpecialisationMaster spm WITH (NOLOCK) on dd.SpecialisationId =spm.ID  " +
                         " where emp.Active =1 and emp.HospitalLocationId =@Locid order by spm.Name ";
                }

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SavePerformanceTarget(int HospId, int FaclityId, int CMonth, int CYear, int FMonth, int FYear, int SpecialityId, decimal OPTargetNos, decimal OPTargetValue,
            decimal IPTargetNos, decimal IPTargetValue, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FaclityId);
                HshIn.Add("@intCMonth", CMonth);
                HshIn.Add("@intCYear", CYear);
                HshIn.Add("@intFMonth", FMonth);
                HshIn.Add("@intFYear", FYear);
                HshIn.Add("@intSpecialityId", SpecialityId);
                HshIn.Add("@decOPTargetNos", OPTargetNos);
                HshIn.Add("@decOPTargetValue", OPTargetValue);
                HshIn.Add("@decIPTargetNos", IPTargetNos);
                HshIn.Add("@decIPTargetValue", IPTargetValue);
                HshIn.Add("@intEncodedBy", EncodedBy);


                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveMISPerformanceTarget", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getPerformanceTarget(int HospId, int FacilityId, int Fyear, int FMonth, int CYear, int CMonth)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intFMonth", FMonth);
                HshIn.Add("@intFYear", Fyear);
                HshIn.Add("@intCMonth", CYear);
                HshIn.Add("@intCYear", CMonth);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetMISPerformanceTarget", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDischargeReportName()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ReportId,ReportName from EMRTemplateReportSetup WITH (NOLOCK) Where DischargeSummary=1 AND Active=1 ");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public DataSet GetDischargeSummaryTemplates(int iHospitalLocationId, int iReportId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intReportId", iReportId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDisChargeSummaryTemplate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet NonDrugOrder(int HospitalLocationId, int FacilityId, int LoginId, int RegistrationId, int EncounterId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intLoginId", LoginId);
            HshIn.Add("intRegistrationId", RegistrationId);
            HshIn.Add("intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspICMNONDrugOrder", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet NonDrugOrderForWard(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intRegistrationId", RegistrationId);
            HshIn.Add("intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetICMNONDrugOrderForWard", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet NonDrugOrder(int HospitalLocationId, int FacilityId, int LoginId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intLoginId", LoginId);
            HshIn.Add("intRegistrationId", RegistrationId);
            if (EncounterId > 0)
                HshIn.Add("intEncounterId", EncounterId);
            if (FromDate != "" && ToDate != "")
            {
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
            }
            if (GroupingDate != "")
                HshIn.Add("@chrGroupingDate", GroupingDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspICMNONDrugOrder", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveNonDrugOrder(int NonDrugId, int iHospitalLocationID, string sRegistrationID, string sEncounterID, String OrderDate,
           string chvPrescription, string chrOrderType, int intDoctorId, int intAckBy, string chrAckDate, string chrAckRemark,
           int iEncodedBy, int FacilityId, bool status, int SaveFor, bool isApprobalReqd)
        {
            return SaveNonDrugOrder(NonDrugId, iHospitalLocationID, sRegistrationID, sEncounterID, OrderDate,
            chvPrescription, chrOrderType, intDoctorId, intAckBy, chrAckDate, chrAckRemark,
            iEncodedBy, FacilityId, status, SaveFor, isApprobalReqd, false, "");
        }

        public string SaveNonDrugOrder(int NonDrugId, int iHospitalLocationID, string sRegistrationID, string sEncounterID, String OrderDate,
            string chvPrescription, string chrOrderType, int intDoctorId, int intAckBy, string chrAckDate, string chrAckRemark,
            int iEncodedBy, int FacilityId, bool status, int SaveFor, bool isApprobalReqd, bool bitIsReadBack, string sReadBackNote)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intNonDrugOrderId", NonDrugId);
            HshIn.Add("@intHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intRegistrationId", sRegistrationID);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", sEncounterID);
            HshIn.Add("@chrOrderDate", OrderDate);
            HshIn.Add("@chvPrescription", chvPrescription);
            HshIn.Add("@chrOrderType", chrOrderType);
            HshIn.Add("@intDoctorId", intDoctorId);
            HshIn.Add("@intAckBy", intAckBy);
            HshIn.Add("@chrAckDate", chrAckDate);
            HshIn.Add("@chrAckRemark", chrAckRemark);
            HshIn.Add("@bitStatus", status);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@intLastUpdatedBy", iEncodedBy);
            HshIn.Add("@intSaveFor", SaveFor);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@bitisApprobalReqd", isApprobalReqd);
            HshIn.Add("@bitIsReadBack", bitIsReadBack);
            HshIn.Add("@chvReadBackNote", sReadBackNote);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMNonDrugOrder", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMNurse(int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intFacilityId", FacilityId);
            string sqlSignDoctor = "SELECT DISTINCT e.ID, ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS NurseName FROM Employee e WITH(NOLOCK) INNER JOIN EmployeeType t WITH(NOLOCK) ON e.EmployeeType = t.Id AND e.Active = 1 AND t.EmployeeType='N' INNER JOIN Users u WITH(NOLOCK) ON e.Id = u.EmpId INNER JOIN SecUserFacility uf WITH(NOLOCK) ON u.Id = uf.UserId WHERE e.Designation IS NOT NULL AND uf.FacilityId = @intFacilityId ORDER BY NurseName";
            try
            {
                return objDl.FillDataSet(CommandType.Text, sqlSignDoctor, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDrugAdministrationTimings(int HospitalLocationId, int FacilityId, int EncounterId, int RegistrationId,
                                            string Time, string rangeFrom, string rangeTo, int LoginFacilityId, string PatientType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intLoginFacilityId", LoginFacilityId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@chrTime", Time);
            HshIn.Add("@chrRangeFrom", rangeFrom);
            HshIn.Add("@chrRangeTo", rangeTo);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDrugListWithTiming", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataTable GetPatientDrugMaxDurationForEncounter(int HospitalLocationId, int FacilityId,
            int RegistrationId, int EncounterId, int LoginFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDrugMaxDuration", HshIn).Tables[0];
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

        public string GetDrugAdministerStatus(int RequestID, int ItemId, string DrugAdministerDate, string DrugAdministerTime, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intRequestID", RequestID);
            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@chrDrugAdministerDate", DrugAdministerDate);
            HshIn.Add("@chrDrugAdministerTime", DrugAdministerTime);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            try
            {

                string Result = objDl.ExecuteScalar(CommandType.StoredProcedure, "uspICMSelectDrugStatus", HshIn).ToString();
                return Result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveDoseDetail(int id, int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, string DrugSchedulerDate,
            string DrugSchedulerTime, string DrugAdministerDate, string DrugAdministerTime, int ItemId, int RequestId,
            string NotAdministeredReason, string DelayReason, int EncodedBy,
            bool IsInfusion, string MedicationHangedDate, string MedicationHangedTime, string VolumnHanged, string MedCompletedDiscontinuedDate,
            string MedCompletedDiscontinuedTime, string VolumnInfusion, string HR, string RR, string SBP, string DBP, bool IsMedicationAdminister,
            int FrequencyDetailId, string AdministrationRemarks, string VolumeDiscard, string volumeUnit, string volInfusionUnit, string volDiscardUnit
            , string WitnessUserId, string Remarks)
        {
            Hashtable HashIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HashIn.Add("@intId", id);
            HashIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HashIn.Add("@intFacilityId", FacilityId);
            HashIn.Add("@intRegistrationId", RegistrationId);
            HashIn.Add("@intEncounterid", EncounterId);
            HashIn.Add("@chrDrugScheduleDate", DrugSchedulerDate);
            HashIn.Add("@chrDrugScheduleTime", DrugSchedulerTime);
            HashIn.Add("@chrDrugAdministerDate", DrugAdministerDate);
            HashIn.Add("@chrDrugAdministerTime", DrugAdministerTime);
            HashIn.Add("@intItemid", ItemId);
            HashIn.Add("@intRequestid", RequestId);
            HashIn.Add("@intEncodedby", EncodedBy);
            HashIn.Add("@chvNotAdministeredReason", NotAdministeredReason);
            HashIn.Add("@chvDelayreason", DelayReason);
            HashIn.Add("@intFrequencyDetailId", FrequencyDetailId);
            HashIn.Add("@chvVolumeDiscard", VolumeDiscard);

            HashIn.Add("@intVolumeUnit", volumeUnit);
            HashIn.Add("@intVolumeInfusionUnit", volInfusionUnit);
            HashIn.Add("@intVolumeDiscardUnit", volDiscardUnit);

            HashIn.Add("@bitIsInfusion", IsInfusion);
            if (MedicationHangedDate != null && MedicationHangedDate != "")
            {
                HashIn.Add("@chrMedicationHangedDate", MedicationHangedDate);
            }
            if (MedicationHangedTime != null && MedicationHangedTime != "")
            {
                HashIn.Add("@chrMedicationHangedTime", MedicationHangedTime);
            }
            HashIn.Add("@chvVolumnHanged", VolumnHanged);
            if (MedCompletedDiscontinuedDate != null && MedCompletedDiscontinuedDate != "")
            {
                HashIn.Add("@chrMedCompletedDiscontinuedDate", MedCompletedDiscontinuedDate);
            }
            if (MedCompletedDiscontinuedTime != null && MedCompletedDiscontinuedTime != "")
            {
                HashIn.Add("@chrMedCompletedDiscontinuedTime", MedCompletedDiscontinuedTime);
            }
            HashIn.Add("@chvVolumnInfusion", VolumnInfusion);
            HashIn.Add("@chvAdministrationRemarks", AdministrationRemarks);

            HashIn.Add("@chvHR", HR);
            HashIn.Add("@chvRR", RR);
            HashIn.Add("@chvSBP", SBP);
            HashIn.Add("@chvDBP", DBP);
            HashIn.Add("@bitIsMedicationAdminister", IsMedicationAdminister);

            HashIn.Add("@intAdminId", string.Empty);
            HashIn.Add("@intWitnessUserId", WitnessUserId);
            HashIn.Add("@chvRemarks", Remarks);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "SaveDrugAdministration", HashIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetDrugAdministerDetails(int RequestID, int ItemId, string dt, string LoginFacilityId, int FrequencyDetailId, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@RequestID", RequestID);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@drugdate", dt);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFrequencyDetailId", FrequencyDetailId);
                //HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterid", EncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspICMSelectDrugDetails", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                HshIn = null;
            }

        }
        public string CancelEMRDischargeOrDeathSummary(int HospId, int SummaryId, int RegistrationId, int EncounterId, int EncodedBy)
        {
            return CancelEMRDischargeOrDeathSummary(HospId, SummaryId, RegistrationId, EncounterId, EncodedBy, string.Empty);
        }
        public string CancelEMRDischargeOrDeathSummary(int HospId, int SummaryId, int RegistrationId, int EncounterId, int EncodedBy, string CancelRemarks)
        {
            return CancelEMRDischargeOrDeathSummary(HospId, SummaryId, RegistrationId, EncounterId, EncodedBy, CancelRemarks, string.Empty);
        }
        public string CancelEMRDischargeOrDeathSummary(int HospId, int SummaryId, int RegistrationId, int EncounterId, int EncodedBy, string CancelRemarks, string Type)
        {

            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intSummaryId", SummaryId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                //added by 24 june 2015 ujjwal to capture cancel remarks for discharge and death summary start
                if (!string.IsNullOrEmpty(CancelRemarks))
                {
                    HshIn.Add("@intCancelRemarks", CancelRemarks);
                }
                else
                {
                    HshIn.Add("@intCancelRemarks", DBNull.Value);
                }
                HshIn.Add("@Type", Type);

                //added by 24 june 2015 ujjwal to capture cancel remarks for discharge and death summary end
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCancelDischargeSummary", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getICMFavouriteNonDrugOrder(string txt, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvSearchCriteria", "%" + txt + "%");

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetICMFavouriteNonDrugOrder", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveICMFavouriteNonDrugOrder(int DoctorId, string NonDrugOrders, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hstOutput = new Hashtable();

                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvNonDrugOrders", NonDrugOrders);
                HshIn.Add("@intEncodedBy", UserId);

                hstOutput.Add("@chvOutputMessage", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspSaveICMFavouriteNonDrugOrder", HshIn, hstOutput);
                return hstOutput["@chvOutputMessage"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool DeleteICMFavouriteNonDrugOrder(int DoctorId, int FavouriteId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();

                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intFavouriteId", FavouriteId);

                i = objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "uspDeleteICMFavouriteNonDrugOrder", HshIn);
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

        public string SaveDosageFromWard(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, int ItemId, int EncodedBy, int IndentId,
                  String xmlFrequencyTime, int FrequencyId, int ItemDetailId, bool IsDoseExtend)
        {
            Hashtable HashIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HashIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HashIn.Add("@intFacilityId", FacilityId);
            HashIn.Add("@intRegistrationId", RegistrationId);
            HashIn.Add("@intEncounterId", EncounterId);
            HashIn.Add("@intItemId", ItemId);
            HashIn.Add("@intIndentId", IndentId);
            HashIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            HashIn.Add("@intItemDetailId", ItemDetailId);
            HashIn.Add("@intFrequencyId", FrequencyId);
            HashIn.Add("@intEncodedby", EncodedBy);
            HashIn.Add("@bitIsDoseExtend", IsDoseExtend);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDosageTime", HashIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet getDrugAdministeredOrNotWithExclude(int HospId, int FacilityId, int RegistrationId, int EncounterId, int ItemId, int FrequencyId, int IndentId)
        {
            HshIn = new Hashtable();


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intHospId", HospId);
            HshIn.Add("@intFrequencyId", FrequencyId);
            HshIn.Add("@intIndentId", IndentId);

            string strsql = "SELECT DISTINCT CONVERT(VARCHAR(10), DrugScheduleDate,103) AS DrugScheduleDate, FrequencyDetailId, (CASE WHEN IsMedicationAdminister=1 THEN 1 ELSE 0 END) AS IsMedicationAdminister " +
                            " FROM DrugAdministration WITH (NOLOCK) " +
                            " WHERE Encounterid=@intEncounterId " +
                            " AND RegistrationId=@intRegistrationId " +
                            " AND ItemId=@intItemId " +
                            " AND Active=1 " +
                            " AND FacilityId=@intFacilityId " +
                            " AND HospitalLocationId=@intHospId " +

                            " SELECT DISTINCT CONVERT(VARCHAR(10), FrequencyTime,103) AS DrugScheduleDate, CONVERT(VARCHAR, FrequencyTime,108) as DoseTime, " +
                            " FrequencyDetailId, (CASE WHEN DoseEnable=1 THEN 1 ELSE 0 END) AS DoseEnable, FrequencyTime " +
                            " FROM EmrFrequencyPatientDetail WITH (NOLOCK) " +
                            " WHERE EncounterId=@intEncounterId " +
                            " AND RegistrationId=@intRegistrationId " +
                            " AND ItemId=@intItemId " +
                            " AND Active=1 " +
                            " AND FrequencyId=@intFrequencyId " +
                            " AND IndentId=@intIndentId " +
                            " AND FacilityId=@intFacilityId " +
                            " ORDER BY FrequencyTime ";
            try
            {
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getExcludedDoseDetails(int FacilityId, int RegistrationId, int EncounterId, int ItemId, int FrequencyId, int IndentId)
        {
            HshIn = new Hashtable();


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@intFrequencyId", FrequencyId);
            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@intFacilityId", FacilityId);

            string strsql = "SELECT DISTINCT CONVERT(VARCHAR(10), FrequencyTime,103) AS Date, CONVERT(VARCHAR, FrequencyTime,108) as DoseTime, FrequencyDetailId, FrequencyTime " +
                            " FROM EmrFrequencyPatientDetail WITH (NOLOCK) " +
                            " WHERE EncounterId=@intEncounterId " +
                            " AND RegistrationId=@intRegistrationId " +
                            " AND ItemId=@intItemId " +
                            " AND IsDoseExtend=1 " +
                            " AND Active=1 " +
                            " AND FrequencyId=@intFrequencyId " +
                            " AND IndentId=@intIndentId " +
                            " AND FacilityId=@intFacilityId " +
                            " ORDER BY FrequencyTime ";

            try
            {
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet GetDoseOfSlotWise(int FacilityId, int EncounterId, int ItemId, int IndentId,
            int FrequencyId, int ItemDetailId, int FrequencyDetailId, String DoseDate)
        {

            Hashtable HashIn = new Hashtable();


            HashIn.Add("@intFacilityId", FacilityId);
            HashIn.Add("@intEncounterId", EncounterId);
            HashIn.Add("@intItemId", ItemId);
            HashIn.Add("@intIndentId", IndentId);
            HashIn.Add("@intItemDetailId", ItemDetailId);
            HashIn.Add("@intFrequencyId", FrequencyId);
            HashIn.Add("@intFrequencyDetailId", FrequencyDetailId);
            HashIn.Add("@chrDoseDate", DoseDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSlotWiseDose", HashIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDepartmentListOfDoctor(int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();


            HashIn.Add("@intHospitalLocationID", HospId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDepartmentListOfDoctor", HashIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEMRDSMultiDepartmentCase()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HashIn = new Hashtable();

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDSMultiDepartmentCase", HashIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientSummaryDetails(int iHospitalLocationID, string sRegistrationID,
            string sEncounterID, int ReportId, int iFacilityId, string chrSource)
        {
            return GetICMPatientSummaryDetails(iHospitalLocationID, sRegistrationID,
              sEncounterID, ReportId, iFacilityId, chrSource, string.Empty, string.Empty);
        }
        public DataSet GetICMPatientSummaryDetails(int iHospitalLocationID, string sRegistrationID,
            string sEncounterID, int ReportId, int iFacilityId, string chrSource, string Type)
        {
            return GetICMPatientSummaryDetails(iHospitalLocationID, sRegistrationID,
              sEncounterID, ReportId, iFacilityId, chrSource, Type, string.Empty);
        }
        public DataSet GetICMPatientSummaryDetails(int iHospitalLocationID, string sRegistrationID,
            string sEncounterID, int ReportId, int iFacilityId, string chrSource, string Type, string SummaryId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intRegistrationID", sRegistrationID);
            HshIn.Add("@intEncounterID", sEncounterID);
            HshIn.Add("@intReportId", ReportId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chrSource", chrSource);
            HshIn.Add("@Type", Type);
            HshIn.Add("@intSummaryId", SummaryId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientDischargeSummary", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDischargePatientSummaryData(int SummaryId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intSummaryId", SummaryId);
            string sFormatDetails = "SELECT ISNULL(PatientSummary,'') AS PatientSummary FROM EMRPatientSummaryDetailsAuditTrail WITH (NOLOCK) WHERE Id = @intSummaryId AND Active = 1";
            try
            {
                return objDl.FillDataSet(CommandType.Text, sFormatDetails, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateStatusReviewed(int Encounterid, string FinalizeDefinalize)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {

                if (FinalizeDefinalize.Equals("F"))
                {
                    HshIn.Add("@intEncounterid", Encounterid);
                    string sFormatDetails = "update encounter set EMRStatusId = (select StatusId from GetStatus(1,'EMR') where code='R') where id = @intEncounterid";
                    objDl.FillDataSet(CommandType.Text, sFormatDetails, HshIn);
                }
                else if (FinalizeDefinalize.Equals("D"))
                {
                    HshIn.Add("@intEncounterid", Encounterid);
                    string sFormatDetails = "update encounter set EMRStatusId = (select StatusId from GetStatus(1,'EMR') where code='O') where id = @intEncounterid";
                    objDl.FillDataSet(CommandType.Text, sFormatDetails, HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return "Record Update successfully";
        }

        public DataSet getDrugAdministratorHistory(int HospId, int FacilityId, int EncounterId, int RegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hstInput = new Hashtable();

            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@intEncounterId", EncounterId);
            hstInput.Add("@intRegistrationId", RegId);


            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDrugAdmistratorHistory", hstInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public int LockEmrDischargeSummary(int EncounterId, int UserId, bool IsLock)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@UserId", UserId);
                HshIn.Add("@EncounterId", EncounterId);
                HshIn.Add("@IsLock", IsLock);

                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspLockEmrDischargeSummary", HshIn);
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
        public DataTable IsLockEmrDischargeSummary(int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@EncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspIsLockEmrDischargeSummary", HshIn).Tables[0];
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



    }
}
