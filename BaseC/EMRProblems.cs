using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class EMRProblems
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable hstOutput;


        public EMRProblems(string Constring)
        {
            sConString = Constring;
        }

        public bool SaveFavouriteProblems(int iDoctorID, int iProblemID, int iUserid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();

                HshIn.Add("@DoctorID", iDoctorID);
                HshIn.Add("@ProblemID", iProblemID);
                HshIn.Add("@Userid", iUserid);
                i = objDl.ExecuteNonQuery(System.Data.CommandType.Text, "IF NOT exists(select DoctorID FROM emrfavouriteproblems WITH (NOLOCK) WHERE doctorID = @DoctorID AND ProblemID = @ProblemID) insert into emrfavouriteproblems(DoctorID, ProblemID,Active,EncodedBy,EncodedDate) Values(@DoctorID, @ProblemID,1,@Userid,getdate())", HshIn);

                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool DeleteFavouriteProblem(int iProblemID, int iDoctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("@ProblemID", iProblemID);
                HshIn.Add("@doctorid", iDoctorID);
                i = objDl.ExecuteNonQuery(CommandType.Text, "delete from emrfavouriteproblems where ProblemID = @ProblemID and Doctorid=@doctorid", HshIn);
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetChiefProblem(int HospId, int FacilityId, int RegId, int DoctorId, string Daterang, string Fdate, string Tdate, string SearchCriteriya,
            bool Distinct, bool IsChronic, int ProblemId, string Visittype)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intDoctorID", DoctorId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateRange", Daterang);
                HshIn.Add("@chrFromDate", Fdate);
                HshIn.Add("@chrToDate", Tdate);
                HshIn.Add("@chvSearchCriteria", SearchCriteriya);
                HshIn.Add("@bitDistinctRecords", Distinct);
                HshIn.Add("@bitChronic", IsChronic);
                HshIn.Add("@intProblemId", ProblemId);
                HshIn.Add("@chvVisitType", Visittype);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string EMRSavePatientProblems(int HospId, int FacilityId, int RegId, int EncId, int PageId, string strProb, int UserId, string Remarks,
                                bool Pregment, bool BreastFeed, bool ShowNote, int DoctorId)
        {
            return EMRSavePatientProblems(HospId, FacilityId, RegId, EncId, PageId, strProb, UserId, Remarks,
                                Pregment, BreastFeed, ShowNote, DoctorId, 0, null);
        }

        public string EMRSavePatientProblems(int HospId, int FacilityId, int RegId, int EncId, int PageId, string strProb, int UserId, string Remarks,
                                bool Pregment, bool BreastFeed, bool ShowNote, int DoctorId, int ProviderId, DateTime? ChangeDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@intPageId", PageId);
            HshIn.Add("@xmlProblemDetails", strProb);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@IsPregnant", Pregment);
            HshIn.Add("@IsBreastFeeding", BreastFeed);
            HshIn.Add("@IsShowNote", ShowNote);
            HshIn.Add("@intDoctorId", DoctorId);

            if (ProviderId > 0)
            {
                HshIn.Add("@intProviderId", ProviderId);
            }
            if (ChangeDate != null)
            {
                HshIn.Add("@chvChangeDate", ChangeDate);
            }

            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientProblems", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string EMRUpdatePatientProblems(int HospId, int FacilityId, int RegId, int EncId, int PageId, string strProb, int UserId, string Remarks,
          bool Pregment, bool BreastFeed, bool ShowNote, int DoctorId, int ProblemId, bool Chronic)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@xmlProblemDetails", strProb);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@IsPregnant", Pregment);
                HshIn.Add("@IsBreastFeeding", BreastFeed);
                HshIn.Add("@IsShowNote", ShowNote);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intProblemId", ProblemId);
                HshIn.Add("@bitChronic", Chronic);


                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRUpdatePatientProblems", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int SaveHPIRemarks(int EncId, string Remarks, bool PullForwardComplain)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@chvHPIRemarks", Remarks);
            HshIn.Add("@bitPullForwardComplain", PullForwardComplain);
            try
            {

                return objDl.ExecuteNonQuery(CommandType.Text, "Update Encounter Set HPIRemarks =@chvHPIRemarks,PullForwardComplain=@bitPullForwardComplain Where ID =@intEncounterId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetFacility(int HospId, int UserId, int GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@chvFacilityType", "O");
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet CheckDuplicateProblem(int HospitalId, int RegId, int EncId, int ProblemId, bool Chronic)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                string strSQL = "";
                HshIn.Add("@inyHospitalLocationID", HospitalId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intProblemId", ProblemId);
                HshIn.Add("@bitischronic", Chronic);

                /*
                if (Chronic == false)
                {
                    strSQL = "select ProblemId from EMRPatientProblemDetails WITH (NOLOCK) where ProblemId=@intProblemId and EncounterId=@intEncounterId and RegistrationId=@intRegistrationId and ischronic=0 and active=1 and HospitalLocationId=@inyHospitalLocationID ";
                }
                else
                {
                    strSQL = "select ProblemId from EMRPatientProblemDetails WITH (NOLOCK) where ProblemId=@intProblemId and EncounterId=@intEncounterId and RegistrationId=@intRegistrationId and ischronic=1 and active=1 and HospitalLocationId=@inyHospitalLocationID ";
                }
                ds = objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
                */

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckDuplicateProblem", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindPullFarwordAndRemarks(int EncId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                string strSQL = "";

                HshIn.Add("@intEncounterId", EncId);

                strSQL = "Select PullForwardComplain,HPIRemarks from Encounter WITH (NOLOCK) where ID=@intEncounterId";

                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet BindCondition(int HospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                string strSQL = "";

                HshIn.Add("@HospID", HospitalId);

                strSQL = "Select ID, Description from EMRProblemsCondition WITH (NOLOCK) where active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";

                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet BindFavouriteProblems(string txt, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            string strSearchCriteria = string.Empty;
            strSearchCriteria = "%" + txt + "%";
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@chvSearchCriteria", strSearchCriteria.ToString());
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavoriteProblems", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet BindFavouriteProblems(string txt, int DoctorId, string Type)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            string strSearchCriteria = string.Empty;
            strSearchCriteria = "%" + txt + "%";

            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@Type", Type);
            HshIn.Add("@chvSearchCriteria", strSearchCriteria.ToString());
            // Change by kuldeep kumar 22/01/2021 
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavoriteProblems", HshIn);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPeriodTypeMaster(string LanguageCode)
        {
            
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string strquery = "SELECT Id, TypeName, value" +
                                " FROM periodTypeMaster WITH(NOLOCK) " +
                                "WHERE LanguageCode='" + LanguageCode + "'" +
                                " ORDER BY Id";

                return objDl.FillDataSet(CommandType.Text, strquery, HshIn);
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

        public DataSet PopulateAllProblem(string txt, int HospitalId, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                string strSearchCriteria = string.Empty;
                strSearchCriteria = "%" + txt + "%";

                HshIn.Add("@chvSearchCriteria", strSearchCriteria.ToString());
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intDoctorId", DoctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemsList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int SaveEMRFavProblem(int HospitalLocationId, int Doctorid, int ProblemId, int UserId, string sDescription)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intDoctorID", Doctorid);
                HshIn.Add("@intProblemId", ProblemId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvDescription", sDescription);
                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveFavProblem", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindODLQCS(int HospitalId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                //string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                //strSql = "SELECT id, Description FROM ProblemOnset where HospitalLocationId is  null or HospitalLocationId=@HospID and Active=1 order by SequenceNo asc";

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEmrGetProblemsData", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindOnset(int HospitalId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                strSql = "SELECT id, Description FROM ProblemOnset WITH (NOLOCK) where Active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";

                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindDuration(int HospitalId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                strSql = "SELECT id, Description FROM EMRProblemsDuration WITH (NOLOCK) where Active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindLocation(int HospitalId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                strSql = "SELECT id, Description FROM EMRProblemsLocation WITH (NOLOCK) where Active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindQuality(int HospitalId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                strSql = "SELECT id, Description FROM EMRProblemsQuality WITH (NOLOCK) where Active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet BindContext(int HospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                strSql = "SELECT id, Description FROM EMRProblemsContext WITH (NOLOCK) where Active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindSeverity(int HospitalId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@HospID", HospitalId);
                strSql = "SELECT id, Description FROM EMRProblemsSeverity WITH (NOLOCK) where Active=1 and HospitalLocationId is  null or HospitalLocationId=@HospID order by SequenceNo asc";
                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet ISProblemExits(int Hospitalid, int RegId, int EncId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();

                string sSQL;
                HshIn.Add("@inyHospitalLocationID", Hospitalid);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);

                sSQL = "select ProblemId from EMRPatientProblemDetails WITH (NOLOCK) where EncounterId=@intEncounterId and RegistrationId=@intRegistrationId and active=1 and HospitalLocationId=@inyHospitalLocationID ";

                return objDl.FillDataSet(CommandType.Text, sSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public int DeleteEMRFavProblem(int Doctorid, int problemId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intDoctorID", Doctorid);
                HshIn.Add("@intProblemId", problemId); //Convert.ToInt32(gvProblems.SelectedRow.Cells[0].Text));
                HshIn.Add("@intEncodedBy", UserId);

                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeleteFavProblem", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void Canceltodayproblem(int Strid, int RegId, int Encid, int HospId, int FacilityId, int Pageid, int UserId, int Shownote)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intId", Strid);
                HshIn.Add("@intRegistrationId", RegId);//@intRegistrationId
                HshIn.Add("@intEncounterId", Encid);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", Pageid);
                HshIn.Add("@intEncodedBy", UserId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientProblem", HshIn);


                HshIn = new Hashtable();

                HshIn.Add("@ShowNote", Shownote);
                HshIn.Add("@intPageId", Pageid);
                HshIn.Add("@intEncounterId", Encid);
                HshIn.Add("@intRegistrationId", RegId);

                string sQuery = " UPDATE EMRPatientFormDetails SET ShowNote = @ShowNote " +
                            " FROM EMRPatientForms epf WITH (NOLOCK) " +
                            " INNER JOIN EMRPatientFormDetails epfd WITH (NOLOCK) ON epf.PatientFormId = epfd.PatientFormId AND epfd.PageId = @intPageId " +
                            " WHERE epf.EncounterId = @intEncounterId " +
                            " AND epf.RegistrationId = @intRegistrationId " +
                            " AND epf.Active = 1 ";

                objDl.ExecuteNonQuery(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void Cancelchronicproblem(int Strid, int RegId, int Encid, int HospId, int FacilityId, int Pageid, int UserId, int Shownote)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intId", Strid);
                HshIn.Add("@intRegistrationId", RegId);//@intRegistrationId
                HshIn.Add("@intEncounterId", Encid);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", Pageid);

                HshIn.Add("@intEncodedBy", UserId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientProblem", HshIn);

                string sQuery = "";
                sQuery += " UPDATE EMRPatientFormDetails SET ShowNote = " + Shownote + "  ";
                sQuery += " FROM EMRPatientForms epf WITH (NOLOCK) ";
                sQuery += " INNER JOIN EMRPatientFormDetails epfd WITH (NOLOCK) ON epf.PatientFormId = epfd.PatientFormId ";
                sQuery += " AND epfd.PageId = " + Pageid + "  WHERE epf.EncounterId = " + Encid + " ";
                sQuery += " AND epf.RegistrationId = " + RegId + "    ";
                sQuery += " AND epf.Active = 1 ";

                objDl.ExecuteNonQuery(CommandType.Text, sQuery);

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
                " INNER JOIN EMRPatientProblemDetails epd WITH (NOLOCK) ON enc.RegistrationId=epd.RegistrationId and enc.DoctorId=epd.DoctorId " +
                " INNER JOIN Employee emp WITH (NOLOCK) ON epd.DoctorId=emp.ID " +
                " WHERE epd.RegistrationId = @intRegistrationId and emp.Active=1 and enc.FacilityId =@intFacilityId and enc.HospitalLocationId=@inyHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }
        }


        public DataSet GetEMRCheifComplainHistory(int RegistrationId)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intRegistrationId", RegistrationId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetComplaintsHistory", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



    }
}
