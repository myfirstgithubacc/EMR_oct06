using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class Appointment
    {
        private string sConString = "";
        public Appointment(string Constring)
        {
            sConString = Constring;
        }

        private Hashtable HshIn;
        private Int16 iHospitalLocationId = 0;
        public DataSet uspFindDoctor(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId,
            int SpecializationId, int DoctorId, string Qualification, string Languages, string sServiceOffered, int iGender, int iNationality)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("HospitalLocationId", iHospitalLocationId);
                HshIn.Add("FaciltyId", iFacilityId);
                HshIn.Add("SpecializationId", SpecializationId);
                HshIn.Add("DoctorId", DoctorId);
                HshIn.Add("Qualification", Qualification);
                HshIn.Add("Languages", Languages);
                HshIn.Add("sServiceOffered", sServiceOffered);
                HshIn.Add("iGender", iGender);
                HshIn.Add("iNationality", iNationality);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspFindDoctor", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getPatientAppointments(int HospId, int FacilityId, int RegistrationId, int AppointmentId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intAppointmentId", AppointmentId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspAppointmentSlip", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable AppointmentsCancelAndUpdate(int HospId, int FacilityId, int iDoctorId, int iMoveDoctorId, string sMoveToDate, string xmlAppointmentIds,
            string sCancelRemarks, string sUpdateCancelMoveAppt, int iEncodedby)
        {
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@intMoveDoctorId", iMoveDoctorId);
                HshIn.Add("@xmlAppointmentId", xmlAppointmentIds);
                HshIn.Add("@chvUpdateCancelMoveAppt", sUpdateCancelMoveAppt);
                HshIn.Add("@chrMoveToDate", sMoveToDate);
                HshIn.Add("@intEncodedBy", iEncodedby);
                HshIn.Add("@chvCancelRemarks", sCancelRemarks);
                hstOutput.Add("@chvErrorOutput", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAppointmentCancelMove", HshIn, hstOutput);
                return hstOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getDoctorSpecialization(int HospId, int iDoctorId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intDoctorId", iDoctorId);
                string sQuery = " SELECT SP.ID AS SpecializationId,sp.Name AS SpecializationName FROM Employee EMP WITH (NOLOCK) " +
                              " INNER JOIN DoctorDetails DD WITH (NOLOCK) ON EMP.ID=DD.DoctorId " +
                              " INNER JOIN SpecialisationMaster SP WITH (NOLOCK) ON DD.SpecialisationId=SP.ID " +
                              " WHERE EMP.ID=@intDoctorId AND EMP.HospitalLocationId=@inyHospitalLocationId";
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getAppointmentEncounter(int AppointmentId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@intAppointmentId", AppointmentId);

                string qry = "SELECT dap.RegistrationId, enc.Id AS EncounterId, enc.StatusId as EncounterStatusId " +
                            " FROM DoctorAppointment dap WITH (NOLOCK) INNER JOIN Encounter enc WITH (NOLOCK) ON dap.AppointmentId = enc.AppointmentID AND enc.Active = 1 " +
                            " WHERE dap.AppointmentId = @intAppointmentId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet isCheckAdmittedPatient(int RegistrationId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn.Add("@iRegistrationId", RegistrationId);

                string qry = "SELECT EncounterNo FROM Admission WITH(NOLOCK) WHERE RegistrationId = @iRegistrationId AND DischargeDate IS NULL AND PATADTYPE <> 'D'";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// For filling of Resource appointment in RIS
        /// Created by Saten
        /// Created On 02 Mar 2013
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="xmlResourceIds"></param>
        /// <param name="cForDate"></param>
        /// <param name="cDateOption"></param>
        /// <param name="iStatusId"></param>
        /// <returns></returns>
        public DataSet GetResourceAppointment(int iHospitalLocationId, int iFacilityId, string xmlResourceIds, string cForDate, string cDateOption, int iStatusId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@xmlResourceIds", xmlResourceIds.ToString());
                HshIn.Add("@chrForDate", cForDate);
                HshIn.Add("@chrDateOption", cDateOption);
                HshIn.Add("@intStatusId", iStatusId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetResourceSchedule", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// Save Resource Booking as per selected patient & service
        /// Created by Saten
        /// Created On 02 Mar 2013
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="iRegistrationId"></param>
        /// <param name="cRegistrationNo"></param>
        /// <param name="iResourceId"></param>
        /// <param name="iServiceId"></param>
        /// <param name="iOrderId"></param>
        /// <param name="iRefferdId"></param>
        /// <param name="iRefferdById"></param>
        /// <param name="dtBookingDate"></param>
        /// <param name="dtFromTime"></param>
        /// <param name="dtToTime"></param>
        /// <param name="cRemarks"></param>
        /// <param name="bitActive"></param>
        /// <param name="vCancelRemark"></param>
        /// <param name="iEncodedBy"></param>
        /// <param name="iAppointmentId"></param>
        /// <returns></returns>
        //public Hashtable SaveResourceBooking(int iHospitalLocationId, int iFacilityId, int iRegistrationId, string cRegistrationNo, int iResourceId, int iServiceId, int iOrderId, int iRefferdId, int iRefferdById, string dtBookingDate, string dtFromTime, string dtToTime, string cRemarks, int bitActive, string vCancelRemark, int iEncodedBy, int iAppointmentId)
        //{
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable HshOut = new Hashtable();
        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    HshIn.Add("@iAppointmentId", iAppointmentId);

        //    HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
        //    HshIn.Add("@iFacilityId", iFacilityId);
        //    HshIn.Add("@iRegistrationId", iRegistrationId);
        //    HshIn.Add("@cRegistrationNo", cRegistrationNo);
        //    HshIn.Add("@iResourceId", iResourceId);
        //    HshIn.Add("@iServiceId", iServiceId);
        //    HshIn.Add("@iOrderId", iOrderId);
        //    HshIn.Add("@iRefferdId", iRefferdId);
        //    HshIn.Add("@iRefferdById", iRefferdById);
        //    HshIn.Add("@dtBookingDate", Convert.ToDateTime(dtBookingDate));
        //    HshIn.Add("@dtFromTime", dtFromTime);
        //    HshIn.Add("@dtToTime", dtToTime);
        //    HshIn.Add("@cRemarks", cRemarks);
        //    HshIn.Add("@bitActive", bitActive);
        //    HshIn.Add("@vCancelRemark", vCancelRemark);
        //    HshIn.Add("@iEncodedBy", iEncodedBy);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveResourceBooking", HshIn, HshOut);
        //    return HshOut;
        //}
        //public Hashtable SaveResourceBooking(int iHospitalLocationId, int iFacilityId, int iRegistrationId,
        //   string cRegistrationNo, int iResourceId, int iServiceId, int iOrderId, int iRefferdId, int
        //   iRefferdById, string dtBookingDate, string dtFromTime, string dtToTime, string cRemarks,
        //   int bitActive, string vCancelRemark, int iEncodedBy, int iAppointmentId,
        //   string FirstName, string MiddleName, string LastName, string DOB, int AgeYears,
        //   int AgeMonths, int AgeDays, int Gender, string PhNo,string OPIP)
        //{
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable HshOut = new Hashtable();

        //    HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
        //    HshIn.Add("@iFacilityId", iFacilityId);
        //    HshIn.Add("@iRegistrationId", iRegistrationId);
        //    HshIn.Add("@cRegistrationNo", cRegistrationNo);
        //    HshIn.Add("@iResourceId", iResourceId);
        //    HshIn.Add("@iServiceId", iServiceId);
        //    HshIn.Add("@iOrderId", iOrderId);
        //    HshIn.Add("@iRefferdId", iRefferdId);
        //    HshIn.Add("@iRefferdById", iRefferdById);
        //    HshIn.Add("@dtBookingDate", dtBookingDate);
        //    HshIn.Add("@dtFromTime", dtFromTime);
        //    HshIn.Add("@dtToTime", dtToTime);
        //    HshIn.Add("@cRemarks", cRemarks);
        //    HshIn.Add("@bitActive", bitActive);
        //    HshIn.Add("@vCancelRemark", vCancelRemark);
        //    HshIn.Add("@iEncodedBy", iEncodedBy);

        //    HshIn.Add("@iAppointmentId", iAppointmentId);
        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    HshIn.Add("@chvFirstName", FirstName);
        //    HshIn.Add("@chvMiddleName", MiddleName);
        //    HshIn.Add("@chvLastName", LastName);
        //    HshIn.Add("@dtDOB", Convert.ToDateTime(DOB));
        //    HshIn.Add("@inyAgeYears", AgeYears);
        //    HshIn.Add("@inyAgeMonths", AgeMonths);
        //    HshIn.Add("@inyAgeDays", AgeDays);
        //    HshIn.Add("@inyGender", Gender);
        //    HshIn.Add("@chvPhNo", PhNo);
        //    HshIn.Add("@chvOPIP", OPIP);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveResourceBooking", HshIn, HshOut);
        //    return HshOut;
        //}

        public Hashtable SaveResourceBooking(int iHospitalLocationId, int iFacilityId,
            int iRegistrationId, string cRegistrationNo, int iResourceId, int iServiceId,
            int iOrderId, int iRefferdId, int iRefferdById, string dtBookingDate,
            string dtFromTime, string dtToTime, string cRemarks, int bitActive, string vCancelRemark,
            int iEncodedBy, int iAppointmentId, int iRadiologistId, int iEncounterId, string OPIP, string Precaution,
            string dtDateOfBirth, int AgeYear, int AgeMonth, int AgeDays, int Gender, string FirstName, string MiddleName, string LastName)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshIn.Add("@iAppointmentId", iAppointmentId);

                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iRegistrationId", iRegistrationId);
                HshIn.Add("@cRegistrationNo", cRegistrationNo);
                HshIn.Add("@iResourceId", iResourceId);
                HshIn.Add("@iServiceId", iServiceId);
                HshIn.Add("@iOrderId", iOrderId);
                HshIn.Add("@iRefferdId", iRefferdId);
                HshIn.Add("@iRefferdById", iRefferdById);
                HshIn.Add("@dtBookingDate", Convert.ToDateTime(dtBookingDate));
                HshIn.Add("@dtFromTime", dtFromTime);
                HshIn.Add("@dtToTime", dtToTime);
                HshIn.Add("@cRemarks", cRemarks);
                HshIn.Add("@bitActive", bitActive);
                HshIn.Add("@vCancelRemark", vCancelRemark);
                HshIn.Add("@iRadiologistId", iRadiologistId);
                HshIn.Add("@iEncodedBy", iEncodedBy);
                HshIn.Add("@iEncounterId", iEncounterId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@chvPrecaution", Precaution);


                HshIn.Add("@chvFirstName", FirstName);
                HshIn.Add("@chvMiddleName", MiddleName);
                HshIn.Add("@chvLastName", LastName);

                if (dtDateOfBirth != string.Empty)
                {
                    HshIn.Add("@chrDateOfBirth", dtDateOfBirth);
                }

                HshIn.Add("@intGender", Gender);
                HshIn.Add("@intAgeYear", AgeYear);
                HshIn.Add("@intAgeMonth", AgeMonth);
                HshIn.Add("@intAgeDays", AgeDays);



                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveResourceBooking", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void CancelThisResourceApp(int iHospitalLocationId, int iFacilityId, int AppId, string vCancelRemark, int iCancelStatusId, int LastChangedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable(); try
            {
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iAppointmentId", AppId);
                HshIn.Add("@iLastChangedBy", LastChangedBy);
                HshIn.Add("@vCancelRemark", vCancelRemark);
                HshIn.Add("@iCancelStatusId", iCancelStatusId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspCancelResourceAppointment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //public void CancelThisResourceApp(int iHospitalLocationId, int iFacilityId, int AppId, string vCancelRemark, int iCancelStatusId, int LastChangedBy)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable HshIn = new Hashtable();
        //    HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
        //    HshIn.Add("@iFacilityId", iFacilityId);
        //    HshIn.Add("@iAppointmentId", AppId);
        //    HshIn.Add("@iLastChangedBy", LastChangedBy);
        //    HshIn.Add("@cLastChangedDate", DateTime.Now);
        //    HshIn.Add("@vCancelRemark", vCancelRemark);
        //    HshIn.Add("@iCancelStatusId", iCancelStatusId);
        //    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspCancelResourceAppointment", HshIn);

        //}
        /// <summary>
        /// Used for get appointment data
        /// Created by Saten
        /// Created On 04 Mar 2013
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="iAppointmentId"></param>
        /// <returns></returns>
        public DataSet uspGetBookingDetails(int iHospitalLocationId, int iFacilityId, int iAppointmentId, int iResourceId, string cFromDate, string cToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable(); try
            {
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iAppointmentId", iAppointmentId);
                HshIn.Add("@iResourceId", iResourceId);
                HshIn.Add("@cFromDate", cFromDate);
                HshIn.Add("@cToDate", cToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBookingDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// Used for copy appointment data
        /// Created by Saten
        /// Created On 04 Mar 2013
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="LoginFacilityId"></param>
        /// <param name="AppointmentId"></param>
        /// <param name="BookingDate"></param>
        /// <param name="FromTime"></param>
        /// <param name="ResourceId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public Hashtable CopyResourceBooking(int HospitalLocationId, int FacilityId, int AppointmentId, string BookingDate, string FromTime, int ResourceId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@iHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityId);
                HshIn.Add("@iAppointmentId", AppointmentId);
                HshIn.Add("@dtBookingDate", Convert.ToDateTime(BookingDate));
                HshIn.Add("@cAppFromTime", FromTime);
                HshIn.Add("@iResourceId", ResourceId);
                HshIn.Add("@iEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspCopyResourceBooking", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable UpdateResourceBooking(int HospitalLocationId, int FacilityId, int AppointmentId, string BookingDate, string FromTime, int ResourceId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@iHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityId);
                HshIn.Add("@iAppointmentId", AppointmentId);
                HshIn.Add("@dtBookingDate", Convert.ToDateTime(BookingDate));
                HshIn.Add("@cAppFromTime", FromTime);
                HshIn.Add("@iResourceId", ResourceId);
                HshIn.Add("@iEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspUpdateResourceBooking", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Not in Use
        //    public Int16 HospitalLocationId
        //    {
        //        get
        //        {
        //            return iHospitalLocationId;
        //        }
        //        set
        //        {
        //            iHospitalLocationId = value;
        //        }
        //    }
        //Not in Use
        //    public SqlDataReader getDoctorList()
        //    {
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        StringBuilder str = new StringBuilder();
        //        HshIn = new Hashtable();
        //        str.Append("select ID as DoctorID, dbo.GetDoctorName(ID) as DoctorName from Employee where ");
        //        str.Append(" EmployeeType in (1,2) and HospitalLocationId=@HospitalLocationId order by DoctorName");
        //        HshIn.Add("HospitalLocationId", iHospitalLocationId);
        //        SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, str.ToString(), HshIn);
        //        return objDr;
        //    }
        //Not in Use
        //    public bool CheckAppointment(string sFromDate, string sToDate, string sFromTime, string sToTime, Int16 iDoctorId, Int16 iHospitalLocationId)
        //    {
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        StringBuilder str = new StringBuilder();
        //        BaseC.Patient objBc = new BaseC.Patient(sConString);
        //        HshIn = new Hashtable();
        //        if (sFromTime.Trim() != "" && sToTime.Trim() != "")
        //        {
        //            str.Append("select DoctorId from DoctorAppointment where status <> 'I' and DoctorId = @DoctorId and HospitalLocationId = @HospitalLocationId and convert(varchar(10), ");
        //            str.Append(" AppointmentDate,111) = @FromDate and (convert(varchar(5),FromTime,108) between @FromTime and @ToTime or ");
        //            str.Append(" convert(varchar(5),ToTime,108) between @FromTime and @ToTime) ");
        //            HshIn.Add("FromDate", objBc.FormatDateYearMonthDate(sFromDate));
        //            HshIn.Add("FromTime", sFromTime);
        //            HshIn.Add("ToTime", sToTime);
        //            HshIn.Add("DoctorId", iDoctorId);
        //            HshIn.Add("HospitalLocationId", iHospitalLocationId);
        //        }
        //        else
        //        {
        //            str.Append("select DoctorId from DoctorAppointment where status <> 'I' and DoctorId = @DoctorId and HospitalLocationId = @HospitalLocationId ");
        //            str.Append("  and convert(varchar(10),AppointmentDate,111) between @FromDate and @ToDate ");
        //            HshIn.Add("FromDate", objBc.FormatDateYearMonthDate(sFromDate));
        //            HshIn.Add("ToDate", objBc.FormatDateYearMonthDate(sToDate));
        //            HshIn.Add("DoctorId", iDoctorId);
        //            HshIn.Add("HospitalLocationId", iHospitalLocationId);
        //        }
        //Not in Use
        //        SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, str.ToString(), HshIn);
        //        if (objDr.Read())
        //        {
        //            objDr.Close();
        //            return false;
        //        }
        //        else
        //        {
        //            objDr.Close();
        //            return true;
        //        }
        //    }
        //Not in Use
        //    public Int16 SaveDoctorLeave(string sFromDate, string sToDate, string sFromTime, string sToTime, Int16 iDoctorId, string sRemarks, Int32 iUserId, string sTimely)
        //    {
        //        DAL.DAL objDl =new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        BaseC.Patient objParse = new BaseC.Patient(sConString);
        //        HshIn = new Hashtable();
        //        HshIn.Add("dtFromDate", objParse.FormatDateYearMonthDate(sFromDate));   
        //        HshIn.Add("dtToDate",  objParse.FormatDateYearMonthDate(sToDate)); 
        //        HshIn.Add("chvFromTime", sFromTime); 
        //        HshIn.Add("chvToTime", sToTime);   
        //        HshIn.Add("chvRemarks", sRemarks); 
        //        HshIn.Add("inyDoctorID", iDoctorId); 
        //        HshIn.Add("inyHospitalLocationID", iHospitalLocationId);   
        //        HshIn.Add("intEncodedBy", iUserId);

        //        if (sTimely == "TRUE")
        //            HshIn.Add("chrTimely", 'Y'); 
        //        else
        //            HshIn.Add("chrTimely", 'N');
        //        Int16 iCount = (Int16)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPSaveDoctorLeave", HshIn);
        //        return iCount;
        //    }
        //Not in Use
        //    public SqlDataReader GetDoctorLeaveDetails(Int16 iDoctorId, string sType)
        //    {
        //        string strSql="";
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        BaseC.Patient objParse = new BaseC.Patient(sConString);
        //        if (sType == "1")
        //        {
        //            strSql = "select Id, '' as LeaveDate, convert(varchar, FromDate,103) as FromDate, convert(varchar, ToDate,103) as ToDate, Remarks, Convert(VarChar,FromDate,108) as Fromtime, Convert(VarChar,ToDate,108) as ToTime, case Active when 1 then 'Active' when 0 then 'InActive' end Status, Active, HospitalLocationId from DoctorLeave where DoctorId = @DoctorId and  convert(varchar,ToDate,111) >= convert(varchar,GetDate(),111) and HospitalLocationId = @HospitalLocationId";
        //        }
        //        else
        //        {
        //            strSql = "select Id, convert(varchar, LeaveDate, 103) as LeaveDate,'' as ToDate, '' as fromDate, Remarks, substring(convert(varchar, FromTime,113),13,5) FromTime, substring(convert(varchar, ToTime,113),13,5) ToTime, HospitalLocationId , case Active when 1 then 'Active' when 0 then 'InActive' end Status, Active from DoctorPeriodicLeave where DoctorId= @DoctorId   and convert(varchar,LeaveDate,111) >= convert(varchar,GetDate(),111) and HospitalLocationId = @HospitalLocationId";
        //        }
        //        HshIn = new Hashtable();
        //        HshIn.Add("HospitalLocationId", iHospitalLocationId);
        //        HshIn.Add("DoctorId", iDoctorId);
        //        SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, strSql, HshIn);
        //        return objDr;
        //    }
        //Not in Use
        //    public Int16 UpdateDoctorLeave(int iId, string sFromDate, string sToDate, string sFromTime, string sToTime, Int16 iDoctorId, string sRemarks, Int32 iUserId, string sTimely, Boolean Status)
        //    {
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        BaseC.Patient objParse = new BaseC.Patient(sConString);
        //        HshIn = new Hashtable();
        //        HshIn.Add("@intId", iId);
        //        HshIn.Add("dtFromDate", objParse.FormatDateYearMonthDate(sFromDate));
        //        HshIn.Add("dtToDate", objParse.FormatDateYearMonthDate(sToDate));
        //        HshIn.Add("chvFromTime", sFromTime);
        //        HshIn.Add("chvToTime", sToTime);
        //        HshIn.Add("chvRemarks", sRemarks);
        //        HshIn.Add("inyDoctorID", iDoctorId);
        //        HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
        //        HshIn.Add("intEncodedBy", iUserId);
        //        HshIn.Add("Status", Status);
        //        if (sTimely == "TRUE")
        //            HshIn.Add("chrTimely", 'Y');
        //        else
        //            HshIn.Add("chrTimely", 'N');
        //        Int16 iCount = (Int16)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPUpdateDoctorLeave", HshIn);
        //        return iCount;
        //    }
        //}

        public string ExistBreakAndBlock(int BreakId, int FacilityId, int DoctorId, string sBreakDate, string sStartTime, string sEndTime)
        {
            string sResult = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intBreakId", BreakId);
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intDoctorID", DoctorId);
                HshIn.Add("@chrBreakDate", sBreakDate);
                HshIn.Add("@chrStartTime", sStartTime);
                HshIn.Add("@chrEndTime", sEndTime);
                string sQuery = "SELECT id as BreakId FROM BreakList WITH (NOLOCK) WHERE DoctorID=@intDoctorID AND CONVERT(VARCHAR(10), BreakDate,111)=@chrBreakDate AND Active=1 AND RecurrenceRule='' AND SUBSTRING(@chrStartTime,0,6) BETWEEN CONVERT(VARCHAR(5), StartTime,108) AND CONVERT(VARCHAR(5), EndTime,108) AND SUBSTRING(@chrEndTime,0,6) BETWEEN CONVERT(VARCHAR(5), StartTime,108) AND CONVERT(VARCHAR(5), EndTime,108) AND FacilityID=@intFacilityID ";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    sResult = dr["BreakId"].ToString();
                }
                dr.Close();
                return sResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string ExistSameDoctorAppointmentForPatient(int iHospitalLocationId, int RegistrationId, int FacilityId, int DoctorId, string sAppointmentDate, string sStartTime, string sEndTime)
        {
            string sResult = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chrAppointmentDate", sAppointmentDate);
                HshIn.Add("@chrFromTime", sStartTime);
                HshIn.Add("@chrToTime", sEndTime);
                string sQuery = " SELECT AppointmentId FROM DoctorAppointment WITH (NOLOCK) " +
                                 " WHERE CONVERT(VARCHAR(10), AppointmentDate,111)=@chrAppointmentDate AND DoctorId=@intDoctorId AND Active=1  " +
                                 " AND SUBSTRING(@chrFromTime,0,6) BETWEEN CONVERT(VARCHAR(5), FromTime,108) AND CONVERT(VARCHAR(5), ToTime,108) " +
                                 " AND SUBSTRING(@chrToTime,0,6) BETWEEN CONVERT(VARCHAR(5), FromTime,108) AND CONVERT(VARCHAR(5), ToTime,108) " +
                                 " AND FacilityID=@intFacilityId AND HospitalLocationId=@inyHospitalLocationID ";

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    sResult = dr["AppointmentId"].ToString();
                }
                dr.Close();
                return sResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string ExistDoctorAppointmentForPatient(int iHospitalLocationId, int RegistrationId, int FacilityId, string sAppointmentDate, string sStartTime, string sEndTime)
        {
            string sResult = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable(); try
            {
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrAppointmentDate", sAppointmentDate);
                HshIn.Add("@chrFromTime", sStartTime);
                HshIn.Add("@chrToTime", sEndTime);
                string sQuery = " SELECT AppointmentId FROM DoctorAppointment WITH (NOLOCK) WHERE RegistrationId=@intRegistrationId " +
                                 " AND CONVERT(VARCHAR(10), AppointmentDate,111)=@chrAppointmentDate AND Active=1 " +
                                 " AND SUBSTRING(@chrFromTime,0,6) BETWEEN CONVERT(VARCHAR(5), FromTime,108) AND CONVERT(VARCHAR(5), ToTime,108) " +
                                 " AND SUBSTRING(@chrToTime,0,6) BETWEEN CONVERT(VARCHAR(5), FromTime,108) AND CONVERT(VARCHAR(5), ToTime,108) " +
                                 " AND FacilityID=@intFacilityId AND HospitalLocationId=@inyHospitalLocationID ";

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    sResult = dr["AppointmentId"].ToString();
                }
                dr.Close();
                return sResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetPatientCompanyId(int iHospitalLocationId, int RegistrationId, int FacilityId)
        {
            string sResult = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable(); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                string sQuery = "SELECT r.SponsorId,r.PlanTypeID, r.accounttypeid,r.AccountCategoryId, isnull(c.Flag,'') Flag FROM Registration r WITH (NOLOCK)  INNER JOIN Company cm WITH (NOLOCK) ON CM.CompanyId=R.SponsorId LEFT JOIN Companytype c WITH (NOLOCK) ON r.AccountCategoryId = c.id  WHERE r.ID=@intRegistrationId AND cm.Active=1 AND r.HospitalLocationId=@inyHospitalLocationId ";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    sResult = dr["SponsorId"].ToString() + "," + dr["PlanTypeID"].ToString() + "," + dr["Flag"].ToString();
                }
                dr.Close();
                return sResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet BindBreakAndBlockDetailGrid(Int32 iDoctorId, int iBreakId, int iFacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intDoctorID", iDoctorId);
                HshIn.Add("@intID", iBreakId);
                HshIn.Add("@intFacilityID", iFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetBreakDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool GetPatientAccountType(int iHospitalLocationId, int RegistrationId, int AppointmentId)
        {
            bool IsRequired = false;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intAppointmentId", AppointmentId);
                string sQuery = " SELECT CT.ID AS AccountCategoryId, CASE WHEN (CT.ApprovalReqdToCheckIn = 1) AND isnull(da.AuthorizationNo,'') = ''  THEN 1 ELSE 0 END AS RequiredPreAuthNo FROM DoctorAppointment da WITH (NOLOCK) INNER JOIN Registration rg WITH (NOLOCK) on da.RegistrationId=rg.Id  INNER JOIN CompanyType CT WITH (NOLOCK) ON CT.Id=rg.AccountCategoryId WHERE RegistrationId=@intRegistrationId AND AppointmentId= @intAppointmentId AND da.Active=1 AND rg.Status='A' AND isnull(da.AuthorizationNo,'') = '' AND rg.HospitalLocationId=@inyHospitalLocationID ";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    IsRequired = Convert.ToBoolean(dr["RequiredPreAuthNo"]);
                }
                dr.Close();
                return IsRequired;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool GetPatientAccountTypeOnEdit(int iHospitalLocationId, int RegistrationId, int AppointmentId)
        {
            bool IsRequired = false;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intAppointmentId", AppointmentId);
                string sQuery = " SELECT CT.ID AS AccountCategoryId, CASE WHEN (CT.ApprovalReqdToCheckIn = 1) THEN 1 ELSE 0 END AS RequiredPreAuthNo FROM DoctorAppointment da WITH (NOLOCK) INNER JOIN Registration rg WITH (NOLOCK) on da.RegistrationId=rg.Id  INNER JOIN CompanyType CT WITH (NOLOCK) ON CT.Id=rg.AccountCategoryId WHERE RegistrationId=@intRegistrationId AND AppointmentId= @intAppointmentId AND da.Active=1 AND rg.Status='A' AND rg.HospitalLocationId=@inyHospitalLocationID ";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    IsRequired = Convert.ToBoolean(dr["RequiredPreAuthNo"]);
                }
                dr.Close();
                return IsRequired;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientVisitLimit(int iHospitalLocationId, int iFacilityId, string sRegistrationNo, string strPAno, string cCallType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@vPreAuthorizationNo", strPAno);
                HshIn.Add("@vRegistrationNo", sRegistrationNo);
                HshIn.Add("@cCallType", cCallType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientVisitLimit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataTable GetEncounterStatusToDelete(int EncounterId, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable(); try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@FormId", 1);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable Checkvisittype(int AppointmentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable(); try
            {
                HshIn.Add("@intAppointmentId", AppointmentId);
                //string strsql = "select DA.VisitTypeId,EVT.Type, evt.Active , f.Active from DoctorAppointment DA WITH (NOLOCK) INNER JOIN EMRVisitType EVT ON DA.VisitTypeId=EVT.VisitTypeID inner join EMRVisitTypeFacility f  WITH (NOLOCK) on evt.VisitTypeID = f.VisitTypeID WHERE AppointmentId = @intAppointmentId and evt.Active = 1 and f.Active = 1 ";
                string strsql = "select DA.VisitTypeId,EVT.Type, evt.Active , f.Active from DoctorAppointment DA WITH (NOLOCK) INNER JOIN EMRVisitType EVT ON DA.VisitTypeId=EVT.VisitTypeID Left join EMRVisitTypeFacility f  WITH (NOLOCK) on evt.VisitTypeID = f.VisitTypeID and f.Active = 1 WHERE AppointmentId = @intAppointmentId and evt.Active = 1  ";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable CheckServiceCoverForClient(int iFacilityId, int iPlanId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intPlanId", iPlanId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckServiceCoverForClient", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable CheckExistServiceForClientPatient(int iAppointmentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intAppointmentId", iAppointmentId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRCheckVisitValidity", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAppointmentDoneUser(int HospId, int FacilityId, string Fdate, string Tdate)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hshInput = new Hashtable();

            hshInput.Add("@inyHospitalLocationId", HospId);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@chvFdate", Fdate);
            hshInput.Add("@chvTdate", Tdate);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointmentDoneUser", hshInput);
            return ds;
        }

        public DataSet getAppointmentSummary(int HospId, string FacilityIds, string xmlDoctorIds, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@xmlFacilityIds", FacilityIds);
                HshIn.Add("@xmlDoctorIds", xmlDoctorIds);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointmentSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DeleteAppointment(int iAppointmentId, int iUserId, string sRemarks)
        {
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            HshIn.Add("@intAppointmentId", iAppointmentId);
            HshIn.Add("@intLastChangedBy", iUserId);
            HshIn.Add("@chvCancelRemark", sRemarks);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteAppointment(string sRemarks, int iAppointmentId, int iUserId)
        {
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            HshIn.Add("@intAppointmentId", iAppointmentId);
            HshIn.Add("@intLastChangedBy", iUserId);
            HshIn.Add("@chvCancelRemark", sRemarks);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteAppointment", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorListForAppointment(Int16 HospitalId, Int16 intSpecialisationId, Int16 FacilityId, DateTime dt)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("HospitalLocationId", HospitalId);
                HshIn.Add("intSpecialisationId", intSpecialisationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("@AppointmentDate", dt.ToString("MM/dd/yyyy"));
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorListForAppointment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorProfile(int DoctorId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("EmpId", DoctorId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "PPDoctorDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAppintmentDetails(int HospId, int FacilityId, string Fromdate, string DoctorId, string Status, string DateOption,
            string Providerid, int statusId, string RegNo, string PName, string EnrollNo, string MobNo)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("fromDate", Fromdate);
                HshIn.Add("DoctorId", DoctorId);
                HshIn.Add("chrStatusID", Status);
                HshIn.Add("chrDateOption", DateOption);
                HshIn.Add("chvProviderId", Providerid);
                HshIn.Add("intStatusId", statusId);
                HshIn.Add("chvRegistrationNo", RegNo);
                HshIn.Add("chvName", PName);
                HshIn.Add("cEnrolleNo", EnrollNo);
                HshIn.Add("MobileNo", MobNo);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointmentDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        private DataSet populatColorList(int HospId, string statustype)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                string strsql = "";
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@chvStatusType", statustype);
                strsql = "select Status,StatusColor from GetStatus(" + HospId + "," + statustype + ")";
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void DeleteAppointmentConsultation(int AppointmentId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intAppintmentId", AppointmentId);
                HshIn.Add("@intEncodedBy", UserId);

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspdeletConsultationOrdr", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

    }
}

