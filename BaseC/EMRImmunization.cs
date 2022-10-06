using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class EMRImmunization
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable HshOut;
        DAL.DAL objDl;


        public EMRImmunization(string Constring)
        {
            sConString = Constring;
        }

        //public SqlDataReader populateCTPCodeDropDown()
        //{
        //    Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    SqlDataReader dr = (SqlDataReader)Dl.ExecuteReader(CommandType.Text, "select CPTID,CPTCOde from CPTCodes where active=1");
        //    return dr;
        //}

        //public String GetCPTDescription(Int32 iID)
        //{
        //    Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("@CPTID", iID);
        //   return  Convert.ToString(Dl.ExecuteScalar(CommandType.Text, "select Description from CPTCodes where CPTID=@CPTID",HshIn));

        //}

        public String SaveImmunizationMaster(Int16 iHospID, String sName, String iCPTCode, String sCVXCode, Int32 iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("chvName", sName);
            HshIn.Add("chvCPTCode", iCPTCode);
            HshIn.Add("chvCVXCode", sCVXCode);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveImmunizationMaster", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetImmunizationMaster(Int16 iHospID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetImmunisationMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Int32 DeActivateImmunizationMaster(Int32 iID, Int16 iHospID, Int32 iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", iID);
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRImmunizationMaster SET Active = '0' WHERE ImmunizationID=@ID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Int32 UpdateImmunizationMaster(Int32 iID, Int16 iHospID, String sName, String iCPTCode, String sCVXCode, Int32 iUserID, Byte bActive)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@ID", iID);
                //HshIn.Add("@HospID", iHospID);
                HshIn.Add("@Name", sName);
                HshIn.Add("@CPTCode", iCPTCode);
                HshIn.Add("@CVXCode", sCVXCode);
                HshIn.Add("@Active", bActive);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRImmunizationMaster SET ImmunizationName=@Name,CPTCode=@CPTCode,CVXCode=@CVXCode,Active=@Active WHERE ImmunizationID=@ID ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public String SaveImmunizationScheduleBaby(Int16 iHospID, Int32 iImmuID, Int16 iAgeFrom1, Int16 iAgeFrom2, String sAgeFromType1, String sAgeFromType2, Int16 iAgeTo1, Int16 iAgeTo2, String sAgeToType1, String sAgeToType2, Int32 iEncodedBy, int iDoctorId)  // Added iDoctorId by Akshay Tirathram 21-Sep-2022
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("intImmunizationID", iImmuID);
            HshIn.Add("inyAgeFrom1", iAgeFrom1);
            HshIn.Add("inyAgeFrom2", iAgeFrom2);
            HshIn.Add("@chrAgeFromType1", sAgeFromType1);
            HshIn.Add("@chrAgeFromType2", sAgeFromType2);
            HshIn.Add("inyAgeTo1", iAgeTo1);
            HshIn.Add("inyAgeTo2", iAgeTo2);
            HshIn.Add("chrAgeToType1", sAgeToType1);
            HshIn.Add("chrAgeToType2", sAgeToType2);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("intDoctorId", iDoctorId); // Added by Akshay Tirathram 21-Sep-2022
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveImmunizationBaby", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader getImmunizationName(Int16 iHospID)
        {
            //HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //HshIn.Add("@HospitalLocationID", iHospID);

            try
            {


                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select immunizationid, immunizationname from emrimmunizationmaster WITH (NOLOCK) where Active=1 order by immunizationname");
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetImmunisationScheduleBaby(Int16 iHospID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationId", iHospID);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetImmunisationScheduleBaby", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEMRImmuBrand(int ImmunizationId, int HospitalId, int EncodedBy)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@ImmunizationId", ImmunizationId);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetImmuBrand", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Int32 DeActivateImmunizationBaby(Int32 iID, Int16 iHospID, Int32 iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", iID);
                HshIn.Add("@HospID", iHospID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRImmunizationScheduleBaby SET Active = '0' WHERE ID=@ID and HospitalLocationID = @HospID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Int32 UpdateImmunizationScheduleBaby(Int32 iImmuID, Int16 iAgeFrom1, Int16 iAgeFrom2, String sAgeFromType1, String sAgeFromType2, Int16 iAgeTo1, Int16 iAgeTo2, String sAgeToType1, String sAgeToType2, Int32 iID, Int32 iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("intImmunizationID", iImmuID);
            HshIn.Add("inyAgeFrom1", iAgeFrom1);
            HshIn.Add("inyAgeFrom2", iAgeFrom2);
            HshIn.Add("chrAgeFromType1", sAgeFromType1);
            HshIn.Add("chrAgeFromType2", sAgeFromType2);
            HshIn.Add("inyAgeTo1", iAgeTo1);
            HshIn.Add("inyAgeTo2", iAgeTo2);
            HshIn.Add("chrAgeToType1", sAgeToType1);
            HshIn.Add("chrAgeToType2", sAgeToType2);
            HshIn.Add("intID", iID);
            HshIn.Add("bitActive", "1");
            HshIn.Add("intEncodedBy", iEncodedBy);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRUpdateImmunizationBaby", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveEMRImmunizationBrands(int ImmunizationId, string XMLBrandId, int EncodedBy, int EMRImmunizationBrandsId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intImmunizationID", ImmunizationId);
                HshIn.Add("@xmlBrandDetails", XMLBrandId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intEMRImmunizationBrandsId", EMRImmunizationBrandsId);
                HshOut.Add("@chvErrorOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRImmunizationBrands", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
            }
            return HshOut;
        }
        //public DataSet GetImmunisationBabyDueDate(Int16 iHospID,Int32 iRegNo,Int32 iEncID, String sDOB,Int32 iUserID)
        //{
        //    HshIn = new Hashtable();
        //    Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn.Add("inyHospitalLocationID", iHospID);
        //    HshIn.Add("intRegNo", iRegNo);
        //    HshIn.Add("intEncID", iEncID);
        //    HshIn.Add("chvDOB", sDOB);
        //    HshIn.Add("intEncodedBy", iUserID);
        //    return  Dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetImmunizationDueDates", HshIn);
        //    
        //}

        //public String SaveImmunizationBabyDueDate(Int16 iHospID, Int32 iRegNo, Int32 iEncID, Int32 iUserID,String sXML)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    HshIn.Add("inyHospitalLocationID", iHospID);
        //    HshIn.Add("intRegNo", iRegNo);
        //    HshIn.Add("intEncID", iEncID);
        //    HshIn.Add("intEncodedBy", iUserID);
        //    HshIn.Add("XMLData", sXML);
        //    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveImmunizationDueDates", HshIn, HshOut);
        //    return HshOut["chvErrorStatus"].ToString();
        //}

        //public String SaveImmunization(Int16 iHospID, Int32 iRegNo,Int32 iEncID,string GivenDate, Int32 iUserID, Int32 FacilityId, Int32 PageId, String sXML)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    HshIn.Add("inyHospitalLocationID", iHospID);
        //    HshIn.Add("intRegistrationId", iRegNo);
        //    HshIn.Add("intEncounterID", iEncID);
        //    HshIn.Add("chrGivenDate", GivenDate);
        //    HshIn.Add("XMLData", sXML);
        //    HshIn.Add("intEncodedBy", iUserID);
        //    HshIn.Add("@intLoginFacilityId", FacilityId);
        //    HshIn.Add("@intPageId",PageId);
        //    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveImmunization", HshIn, HshOut);
        //    return HshOut["chvErrorStatus"].ToString();
        //}




        //public String UpdateImmunizationBabyDueDate(Int16 iHospID, String sGivenOn, Int32 sGivenBy, Int32 iChildID)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    HshIn.Add("inyHospitalLocationID", iHospID);
        //    HshIn.Add("chvGivenOn", sGivenOn);
        //    HshIn.Add("intGivenBy", sGivenBy);
        //    HshIn.Add("intChildId", iChildID);
        //    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRUpdateImmunizationDueDates", HshIn, HshOut);
        //    return HshOut["chvErrorStatus"].ToString();
        //}


        public DataSet GetPatientImmunizationDueDates(int iHospID, int iRegId, string sDOB)
        {

            Hashtable HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@intRegistrationId ", iRegId);
                HshIn.Add("@dob", sDOB);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunizationDueDates", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SavePatientImmunization(int id, int iHospitalLocationId, int iFacilityId, int iSchedulerId, int iImmunizationId,
            int iRegistrationId, int iEncounterId, string sGivenDate, int iGivenBy, string iBatchNo, bool bRejectedByPatient, bool bVaccineGivenByOutsider, string sRemarks, int iEncodedBy, int iPageId, int iBrandId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hsOutput = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@Id", id);
            HshIn.Add("@HospitalLocationId", iHospitalLocationId);
            HshIn.Add("@ScheduleId", iSchedulerId);
            HshIn.Add("@ImmunizationId", iImmunizationId);
            HshIn.Add("@iBrandId", iBrandId);
            HshIn.Add("@RegistrationId", iRegistrationId);
            HshIn.Add("@EncounterId", iEncounterId);
            HshIn.Add("@GivenDateTime", sGivenDate);// txtGivenDate.SelectedDate.Value);
            HshIn.Add("@GivenBy", iGivenBy);//Session["UserId"]);//
            HshIn.Add("@FacilityId", iFacilityId);
            HshIn.Add("@LotNo", iBatchNo);//lbllot.Text);

            HshIn.Add("@RejectedByPatient", bRejectedByPatient);
            HshIn.Add("@VaccineGivenByOutsider", bVaccineGivenByOutsider);
            HshIn.Add("@Remarks", sRemarks);
            HshIn.Add("@EncodedBy", iEncodedBy);
            HshIn.Add("@intPageId", iPageId);
            hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hsOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdatePatientImmunizationDetail", HshIn, hsOutput);
                return hsOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetImmunizationGivenBy(int iHospID)
        {

            Hashtable HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationID", iHospID);
                string query = "SELECT emp.id AS NurseId, ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'')  + ISNULL(' ' + LastName,'') AS NurseName FROM Employee emp WITH (NOLOCK) LEFT JOIN EmployeeType empt WITH (NOLOCK) on emp.EmployeeType =empt.Id " +
                       "  WHERE empt.EmployeeType in ('D','N') AND HospitalLocationId=@inyHospitalLocationID and emp.active=1 order by FirstName ";
                return objDl.FillDataSet(CommandType.Text, query, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientImmunization(int iHospID, Int32 iRegistrationId, Int32 iEncounterId, int iScheduler, int iImminizationId)
        {

            Hashtable HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intImmunizationId", iImminizationId);
                HshIn.Add("@intScheduleId", iScheduler);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientImmunization", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetEncounterFacilityForImmunization(int iHospID, Int32 iRegistrationId, Int32 iEncounterId)
        {

            Hashtable HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string sQuery = "SELECT FacilityID FROM Encounter WITH (NOLOCK) WHERE id=@intEncounterId AND RegistrationId=@intRegistrationId AND HospitalLocationId=@inyHospitalLocationID ";
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            HshIn.Add("@intEncounterId", iEncounterId);

            try
            {

                string FacilityId = objDl.ExecuteScalar(CommandType.Text, sQuery, HshIn).ToString();
                return FacilityId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int InActivePatientImmunization(int id, Int32 iEncodedBy, DateTime dtEncodedDate, string CancelRemarks, string chvType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@ID", id);
                HshIn.Add("@LastChangedBy", iEncodedBy);
                HshIn.Add("@LastChangedDate", Convert.ToDateTime(dtEncodedDate).ToString("yyyy/MM/dd"));
                HshIn.Add("@Cancel", CancelRemarks);
                HshIn.Add("@chvType", chvType);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEmrImmunizationdetailInactive", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string GetPatientDOB(int iRegistrationId, int iHospitalLocationId)
        {
            string sDOB;

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intRegistrationId", iRegistrationId);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            try
            {
                sDOB = objDl.ExecuteScalar(CommandType.Text, "select Convert(varchar,dateofbirth,103) from Registration WITH (NOLOCK) where Id=@intRegistrationId AND HospitalLocationId=@inyHospitalLocationId", HshIn).ToString();
                return sDOB;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetImmunizationStatus(int iHospitalLocationId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intHospId", iHospitalLocationId);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "SELECT StatusId,Status,StatusColor,ColorName,Code FROM DBO.GetStatus(@intHospId,'ImmunizationStatus')", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetImmunizationReminder(int iHospitalLocationId, int iFilterOption, string sFromDate, string sToDate, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intFilterOption", iFilterOption);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetImmunizationReminder", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveUpdatePatientDailyInjections(int id, int iHospitalLocationId, int iImmunizationId, int iRegistrationId, int iEncounterId,
            string sGivenDate, int iBrandId, string iLotno, int iGivenBy, int iFacilityId, string QtyGiven, string sRemarks, string cRemarks, int iEncodedBy, string ExpiryDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hsOutput = new Hashtable();
            HshIn = new Hashtable();

            HshIn.Add("@Id", id);
            HshIn.Add("@HospitalLocationId", iHospitalLocationId);
            HshIn.Add("@ImmunizationId", iImmunizationId);
            HshIn.Add("@RegistrationId", iRegistrationId);
            HshIn.Add("@EncounterId", iEncounterId);
            HshIn.Add("@GivenDateTime", sGivenDate);
            if (iBrandId != 0)
            {
                HshIn.Add("@iBrandId", iBrandId);
            }
            HshIn.Add("@LotNo", iLotno);
            HshIn.Add("@GivenBy", iGivenBy);
            HshIn.Add("@FacilityId", iFacilityId);
            HshIn.Add("@QtyGiven", QtyGiven);
            HshIn.Add("@Remarks", sRemarks);
            HshIn.Add("@CancellationRemarks", cRemarks);
            HshIn.Add("@EncodedBy", iEncodedBy);
            HshIn.Add("@ExpiryDate", ExpiryDate);
            hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hsOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdatePatientDailyInjections", HshIn, hsOutput);
                return hsOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet EMRGetPatientDailyInjections(int iHospID, int iRegistrationId)
        {

            Hashtable HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDailyInjections", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetImmunisation(int iHospID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospID);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "select ImmunizationId,ImmunizationName  from EMRImmunizationMaster WITH (NOLOCK) where Active=1 AND HospitalLocationId =@inyHospitalLocationId order by ImmunizationName", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int DeteDailyImmunisation(int id, string cRemarks)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@Id", id);
            HshIn.Add("@Remarks", cRemarks);
            try
            {
                return objDl.ExecuteNonQuery(CommandType.Text, "update EMRPatientDailyInjections set CancellationRemarks =@Remarks, Active=0 where Id =@Id", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getInjectionWithStock(int HospId, int FacilityId, int StoreId, int UserId, string ItemName, string ShortName)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("chvItemSubCategoryShortName", ShortName);
            HshIn.Add("intEncodedBy", UserId);
            HshIn.Add("chrItemSearchFor", string.Empty);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SavePatientImmunization(int id, int iHospitalLocationId, int iFacilityId, int iSchedulerId, int iImmunizationId,
              int iRegistrationId, int iEncounterId, string sGivenDate, int iGivenBy, string iBatchNo, bool bRejectedByPatient, bool bVaccineGivenByOutsider, string sRemarks, int iEncodedBy, int iPageId, int iBrandId, string ExpiryDate)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hsOutput = new Hashtable();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@Id", id);
            HshIn.Add("@HospitalLocationId", iHospitalLocationId);
            HshIn.Add("@ScheduleId", iSchedulerId);
            HshIn.Add("@ImmunizationId", iImmunizationId);
            HshIn.Add("@iBrandId", iBrandId);
            HshIn.Add("@RegistrationId", iRegistrationId);
            HshIn.Add("@EncounterId", iEncounterId);
            HshIn.Add("@GivenDateTime", sGivenDate);// txtGivenDate.SelectedDate.Value);
            HshIn.Add("@GivenBy", iGivenBy);//Session["UserId"]);//
            HshIn.Add("@FacilityId", iFacilityId);
            HshIn.Add("@LotNo", iBatchNo);//lbllot.Text);

            HshIn.Add("@RejectedByPatient", bRejectedByPatient);
            HshIn.Add("@VaccineGivenByOutsider", bVaccineGivenByOutsider);
            HshIn.Add("@Remarks", sRemarks);
            HshIn.Add("@EncodedBy", iEncodedBy);
            HshIn.Add("@intPageId", iPageId);
            HshIn.Add("@ExpiryDate", ExpiryDate);
            hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hsOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdatePatientImmunizationDetail", HshIn, hsOutput);
                return hsOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEMRPatientDueImmunizationDetail(int HospitalLocationId, int RegistrationId, string xmlImmunisationDetails, int EncodedBy)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hsOutput = new Hashtable();
            Hashtable HshIn = new Hashtable();

            //            @intHospitalLocationId int,
            //@RegistrationId int ,
            //@xmlImmunisationDetails XML,
            //@intEncodedBy int ,
            //@chvErrorStatus varchar(200) output

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@RegistrationId", RegistrationId);
            HshIn.Add("@xmlImmunisationDetails", xmlImmunisationDetails);

            HshIn.Add("@intEncodedBy", EncodedBy);

            hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {


                hsOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientDueImmunization", HshIn, hsOutput);
                return hsOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetImmuRegistrationId(int iHospID, int FacilityId, Int64 RegistrationNo)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intHospitalLocationId", iHospID);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            try
            {


                return objDl.FillDataSet(CommandType.Text, "SELECT id as RegistrationId FROM Registration WHERE RegistrationNo=@intRegistrationNo and FacilityID=@intFacilityId and HospitalLocationId=@intHospitalLocationId", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetImmunPatientDashBoardDetails(int HospitalLocationId, int FacilityId, string RegistrationNo, string Name, string MobileNo, string MotherName,
            string FromDate, string ToDate, string Status, string DateRange, string VisitType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@chvName", Name);
            HshIn.Add("@chvDateRange", DateRange);
            HshIn.Add("@chvMobileNo", MobileNo);
            HshIn.Add("@chvMotherName", MotherName);
            HshIn.Add("@chrFromDate", FromDate);
            HshIn.Add("@chrToDate", ToDate);
            HshIn.Add("@chrStatus", Status);
            if (VisitType != "")
            {
                HshIn.Add("@ChrVisitType", VisitType);
            }

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetImmunPatientDashBoardDetails", HshIn, HshOut).Tables[0];

        }

    }
}
