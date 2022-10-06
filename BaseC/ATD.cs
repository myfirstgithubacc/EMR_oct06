using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class ATD
    {
        string sConString = "";//
        Hashtable HshIn;
        Hashtable houtPara;
        DataSet ds;
        DAL.DAL objDl;
        int hospiId, regno, ipno, facilityId, admitingdocId, AdmitingteamId, wardid, bedcateid, billingid, bedid, refferalid, casetype;
        int kinprevid, kinid, kinrelationid, kindob, kingender, kincountryid, kinstateid, kincityid;
        string kinfname, kinmname, kinlname, kinaddress1, kinadress2, kinzipcode, kinres, kinmob, kinemail;

        public ATD(string Constring)
        {
            sConString = Constring;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }

        public string saveAdmission(string s)
        {
            return s;
        }

        public DataSet GetRegistrationId(Int64 RegNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intRegistrationNo", RegNo);
                string str = " SELECT pr.Id,EN.Id AS EncounterId,EN.EncounterNo,ad.AdmissionDate,EN.EREncounterId,ad.ConsultingDoctorId AS DoctorId,pr.RegistrationNo FROM Registration pr WITH (NOLOCK) INNER JOIN Admission ad WITH (NOLOCK) ON PR.Id=ad.RegistrationId " +
                              " INNER JOIN Encounter EN WITH (NOLOCK) ON AD.EncounterId=EN.Id WHERE pr.RegistrationNo=@intRegistrationNo AND ad.PatadType NOT IN('D','C') AND ad.Active=1 ";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetRegistrationDS(Int64 RegNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intRegistrationNo", RegNo);
                string str = " SELECT pr.Id,EN.Id AS EncounterId,EN.EncounterNo,ad.AdmissionDate,EN.EREncounterId,ad.ConsultingDoctorId AS DoctorId,pr.RegistrationNo FROM Registration pr WITH (NOLOCK) INNER JOIN Admission ad WITH (NOLOCK) ON PR.Id=ad.RegistrationId " +
                              " INNER JOIN Encounter EN ON AD.EncounterId=EN.Id WHERE pr.RegistrationNo=@intRegistrationNo AND ad.PatadType NOT IN('C') AND ad.Active=1 ";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetPatientPayerdeatils(int iHospitalId, int iFacilityId, int iRegno)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intHospitalLocationId", iHospitalId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationNo", iRegno);
                string str = "select id,SponsorId,PayorId,ReferredTypeID,ReferredByID from Registration WITH (NOLOCK) " +
                    " where RegistrationNo=@intRegistrationNo and FacilityID=@intFacilityId and HospitalLocationId=@intHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveWard(int HospitalLocationId, int WardId, string ShortName, string Wardname, string Incharge, string BlockName,
                               string FloorName, int Encodedby, int FacilityId)
        {


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                HshIn.Add("intHospitalLocationId", HospitalLocationId);
                HshIn.Add("intWardId", WardId);
                HshIn.Add("chrShortName", ShortName);
                HshIn.Add("chrWardName", Wardname);
                HshIn.Add("chrWardIncharge", Incharge);
                HshIn.Add("chrBlockname", BlockName);
                HshIn.Add("chrFloorNo", FloorName);
                HshIn.Add("intEncodedBy", Encodedby);
                HshIn.Add("intFacilityId", FacilityId);
                houtPara.Add("chvErrorStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPWardSaveUpdate", HshIn, houtPara);
                return houtPara["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getWardMaster(int HospitalLocationId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityId);

                string str = "SELECT WardId, WardName, BlockName, WardshortName, WardIncharge, FloorNo, FacilityId FROM WardMaster WITH (NOLOCK) WHERE FacilityId=@iFacilityId AND Active=1 AND HospitalLocationId=@inyHospitalLocationId ORDER BY WardName ";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getWardStationMaster(int FacilityId, int HospitalLocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);

                string str = "SELECT id AS WardStationId, StationName FROM WardStationMaster WITH (NOLOCK) " +
                    " WHERE Active = 1 AND FacilityId = @intFacilityId AND HospitalLocationId = @inyHospitalLocationId ORDER BY StationName";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveWardStationMaster(int WardStationId, int HospitalLocationId, int FacilityId,
                                    string StationName, int EncodedBy)
        {


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                HshIn.Add("@intWardStationId", WardStationId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvStationName", StationName);
                HshIn.Add("@intEncodedBy", EncodedBy);

                houtPara.Add("chvErrorStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveWardStationMaster", HshIn, houtPara);

                return houtPara["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBedDetails(int HospitalLocationId, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBedDeatils", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDischargeOutlierRemarks(int HospitalLocationId, string ReasonType)
        {
            DataSet ds = new DataSet();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@ReasonType", ReasonType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetReasonMasterbyReasonType", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetWard(int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.Text, "select distinct WardId,WardName,0 as FavouriteWard From WardMaster WITH (NOLOCK) Where Active=1 And FacilityId=@intFacilityId Order By 2", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetWardStation(int FacilityId,int EmployeeId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEmployeeId", EmployeeId);
            StringBuilder st = new StringBuilder();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                st.Append(" Select ws.ID,ws.StationName from WardTagging wt with (nolock)");
                st.Append(" Inner Join WardStationMaster ws with(nolock) on wt.WardStationId = ws.Id");
                st.Append(" Where wt.EmployeeId = @intEmployeeId and wt.facilityId = @intFacilityId and wt.active = 1");
                return objDl.FillDataSet(CommandType.Text, st.ToString(), HshIn);
                //return objDl.FillDataSet(CommandType.Text, "select ID,StationName from WardStationMaster WITH (NOLOCK) where FacilityId =@intFacilityId order by StationName", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBedCategory(int FacilityId)
        {
            return GetBedCategory("", FacilityId);
        }

        public DataSet GetBedCategory(string BedCategoryType, int FacilityId)
        {
            //ds = new DataSet();
            //objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //HshIn = new Hashtable();

            //HshIn.Add("@chvBedCategoryType", BedCategoryType);
            //HshIn.Add("@intFacilityId", FacilityId);

            //string qry = "SELECT b.BedCategoryId, i.ServiceName AS BedCategoryName, b.BedCategoryType,  b.IsICU, b.BedCategoryIdentification, b.IsBillingCategory " +
            //        " FROM ItemOfService i WITH (NOLOCK) " +
            //        " INNER JOIN FacilityWiseItemOfService fi WITH(NOLOCK) on fi.serviceId = i.ServiceId " +
            //        " INNER JOIN BedCategoryMaster b WITH (NOLOCK) ON b.BedCategoryId = i.ServiceId AND i.Active = 1" +
            //        " WHERE fi.FacilityId = Case When @intFacilityId = 0 then fi.FacilityId else @intFacilityId end ";

            //if (BedCategoryType.Trim() != "")
            //{
            //    qry += " AND b.BedCategoryType = @chvBedCategoryType ";
            //}

            //ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            //return ds;

            DataSet objDs = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();

            HshIn.Add("@chvBedCategoryType", BedCategoryType.Trim());
            HshIn.Add("@intFacilityId", FacilityId);

            objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetBedCategory", HshIn);
            return objDs;
        }
        public DataSet GetBillingCategory(string BedCategoryType, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@chvBedCategoryType", BedCategoryType);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT b.BedCategoryId, i.ServiceName AS BedCategoryName, b.BedCategoryType " +
                        " FROM ItemOfService i WITH (NOLOCK) " +
                        " Inner join FacilityWiseItemOfService fi WITH(NOLOCK) on fi.serviceId = i.ServiceId " +
                        " INNER JOIN BedCategoryMaster b WITH (NOLOCK) ON b.BedCategoryId = i.ServiceId AND i.Active = 1" +
                        " WHERE isnull(b.isBillingCategory,0)=1 and fi.FacilityId = Case When @intFacilityId = 0 then fi.FacilityId else @intFacilityId end " +
                        " and b.BedCategoryId not in (select value from HospitalSetup WITH (NOLOCK) where flag in ('DefaultOPDCategoryService','DefaultIPDCategoryService'))";

                if (BedCategoryType.Trim() != "")
                {
                    qry += " AND b.BedCategoryType = @chvBedCategoryType ";
                }
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetBedNo(int wadrid, int BedCategory)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intBedCategoryId", BedCategory);
            HshIn.Add("@intWardId", wadrid);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "SELECT Id, bedno FROM BedMaster WITH (NOLOCK) where BedCategoryId=@intBedCategoryId and WardId=@intWardId and  BedStatus='V' And Active=1", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// Save  Bed Deatils
        /// Santosh Kumar
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="WardId"></param>
        /// <param name="BedCategoryId"></param>
        /// <param name="BedId"></param>
        /// <param name="BedNo"></param>
        /// <param name="Bsestatus"></param>
        /// <param name="Reservationfor"></param>
        /// <param name="Temprorybed"></param>
        /// <param name="ExcludeOccupancy"></param>
        /// <param name="phoneext"></param>
        /// <param name="Encodedby"></param>
        /// <returns></returns>
        public string SaveBedDetails(int HospitalLocationId, int WardId, int WardStationId, int BedCategoryId, int BedId, string BedNo,
                string Bsestatus, string Reservationfor, bool Temprorybed,
            bool ExcludeOccupancy, string phoneext, DateTime OpeningDate, DateTime ClosingDate, int Encodedby, int facilityId)
        {
            return SaveBedDetails(HospitalLocationId, WardId, WardStationId, BedCategoryId, BedId, BedNo,
                Bsestatus, Reservationfor, Temprorybed,
            ExcludeOccupancy, phoneext, OpeningDate, ClosingDate, Encodedby, facilityId, "");
        }
        public string SaveBedDetails(int HospitalLocationId, int WardId, int WardStationId, int BedCategoryId, int BedId, string BedNo,
                string Bsestatus, string Reservationfor, bool Temprorybed,
            bool ExcludeOccupancy, string phoneext, DateTime OpeningDate, DateTime ClosingDate, int Encodedby, int facilityId, string Remarks)
        {
            DataSet ds = new DataSet();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            HshIn.Add("intHospitalLocationId", HospitalLocationId);
            HshIn.Add("intWardId", WardId);
            HshIn.Add("intWardStationId", WardStationId);
            HshIn.Add("intBedCategoryId", BedCategoryId);
            HshIn.Add("intBedId", BedId);
            HshIn.Add("chrBedNo", BedNo);
            HshIn.Add("chrBedstatus", Bsestatus);
            HshIn.Add("chrBedReservationFor", Reservationfor);
            HshIn.Add("bitlTemporaryBed", Temprorybed);
            HshIn.Add("bitExcludeOccupancy", ExcludeOccupancy);
            HshIn.Add("chrPhoneExtNo", phoneext);
            HshIn.Add("dtOpeningDate", OpeningDate);
            HshIn.Add("dtClosingDate", ClosingDate);
            HshIn.Add("intEncodedBy", Encodedby);
            HshIn.Add("intFacilityId", facilityId);
            houtPara.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPBedSaveUpdate", HshIn, houtPara);
                return houtPara["chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        /// <summary>
        /// Get Bed Status
        /// Santosh kumar
        /// Funtion Name - FillBedStatus
        /// </summary>
        /// <param name="bedcategory"></param>
        /// <param name="bedstatus"></param>
        /// <param name="wardfloor"></param>
        /// <returns></returns>
        public DataSet FillBedStatus(int BedCategoryId, string BedStatus, int WardId, string BedNo, int RegistrationId,
            int facilityId, int hospitallocationid)
        {
            DataSet ds = new DataSet();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intBedCategoryId", BedCategoryId);
            HshIn.Add("@chvBedStatus", BedStatus);
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@inyHospitalLocationId", hospitallocationid);
            HshIn.Add("@intFacilityId", facilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "IpAtdGetBedStatusBetCategorywise", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        /// <summary>
        /// Get Ip Patient Detail
        /// Santosh Kumar
        /// </summary>
        /// <param name="regno"></param>
        /// <param name="encno"></param>
        /// <returns></returns>
        public DataSet GetIPPatientDetails(int regno, string encno)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@intRegistrationNo", regno);
            HshIn.Add("@chvEncounterNo", encno);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetIPPatientDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int cancelBedRetain(int iBedId, int iRegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@intBedId", iBedId);
            HshIn.Add("@intRegId", iRegId);
            try
            {

                string strqry = " UPDATE BedMaster SET BedStatus='V' WHERE Id = @intBedId ";

                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

                strqry = " UPDATE BedRetain SET Status = 'RL' WHERE BedId = @intBedId AND RegistrationId = @intRegId ";

                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int cancelBedRelease(int iBedId, int iRegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@intBedId", iBedId);
            HshIn.Add("@intRegId", iRegId);
            try
            {

                string strqry = " UPDATE BedMaster SET BedStatus='R' WHERE Id = @intBedId ";

                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

                strqry = " UPDATE BedRetain SET Status = 'RT' AND ReleaseDate = NULL WHERE BedId = @intBedId AND RegistrationId = @intRegId ";

                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveBedRelease(int HospiId, int RegId, string Encno, int BedId, int BillingcatId, DateTime ReleaseDT, bool Isrelease, int CurrentBedId, int UserId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {

                HshIn.Add("intHospitalLocationId", HospiId);
                HshIn.Add("intRegistrationId", RegId);
                HshIn.Add("intEncounterno", Encno);
                HshIn.Add("intbedId", BedId);
                HshIn.Add("intBillingCategory", BillingcatId);
                HshIn.Add("dateReleaseDate", ReleaseDT);
                if (Isrelease == true)
                {
                    HshIn.Add("@intCurrentbedId", CurrentBedId);
                }
                HshIn.Add("charStatus", "RL");

                HshIn.Add("intEncodedBy", UserId);
                houtPara.Add("chvErrorStatus", SqlDbType.VarChar);

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBedRetainRelease", HshIn, houtPara);
                return houtPara["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDischargeStatus()
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "SELECT Id,Name FROM PatientStatusMaster WITH (NOLOCK) WHERE (Status = 'A') ORDER BY Name");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        /// <summary>
        /// Get Dotor Transfer.
        /// Created By - Santosh Chaursia
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <param name="RegId"></param>
        /// <param name="EncNo"></param>
        /// <returns></returns>
        public DataSet GetDoctorTransfer(int HospitalId, int RegId, string EncNo, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitallocationId", HospitalId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@chrEncounterNo", EncNo);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorTransferDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetToHospital(int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@facilityid", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetTransferredFacility", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetBedTransfer(int HospitalId, int RegId, string EncNo, string Fromdate, string Todate, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitallocationId", hospiId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterNo", EncNo);
            HshIn.Add("@chrFromDate", Fromdate);
            HshIn.Add("@chrToDate", Todate);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBedTransferDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetBedTransfer(int HospitalId, int RegId, string EncNo, string Fromdate, string Todate, int FacilityId,string BedReq)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitallocationId", hospiId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterNo", EncNo);
            HshIn.Add("@chrFromDate", Fromdate);
            HshIn.Add("@chrToDate", Todate);
            HshIn.Add("@chrBedReq", BedReq);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBedTransferDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetAdmissionDetails(int HospitalId, int RegNo, string DateRange, string Fromdate, string Todate)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitallocationId", HospitalId);
            HshIn.Add("@intRegistrationNo", RegNo);
            HshIn.Add("@chvDateRange", DateRange);
            HshIn.Add("@chrFromDate", Fromdate);
            HshIn.Add("@chrToDate", Todate);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetAdmittedPatientEMR", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Boolean isBedRetain(string sRegNo, string sEncNo)
        {
            Boolean isBedRetain = false;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            HshIn.Add("@RegistrationNo", sRegNo);
            HshIn.Add("@EncounterNo", sEncNo);
            try
            {
                ds = objDl.FillDataSet(CommandType.Text, "SELECT BR.id FROM BedRetain BR WITH (NOLOCK) INNER JOIN Admission A WITH (NOLOCK) ON BR.RegistrationId = A.RegistrationId AND A.EncounterId = BR.EncounterId WHERE A.EncounterNo = @EncounterNo AND A.RegistrationNo = @RegistrationNo AND Status = 'RT' And ReleaseDate IS NULL ", HshIn);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        isBedRetain = true;
                }
                else
                {
                    isBedRetain = true;
                }
                return isBedRetain;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }

        public DataSet GetBedretainDetails(int HospitalId, int regId, string DateRange, string Fromdate, string Todate)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitallocationId", HospitalId);
            HshIn.Add("@intRegistrationId", regId);
            HshIn.Add("@chvDateRange", DateRange);
            HshIn.Add("@chrFromDate", Fromdate);
            HshIn.Add("@chrToDate", Todate);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPatientBedRetain", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveUpdateVIPPatient(int HospitalId, int RegId, int Regno, bool Vip, string VIPNarration, int Encodedby)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalId);
            HshIn.Add("intRegistrationId", RegId);
            HshIn.Add("intRegistrationNo", Regno);
            HshIn.Add("bitVIP", Vip);
            HshIn.Add("chvVipNarration", VIPNarration);
            HshIn.Add("intEncodedby", Encodedby);
            houtPara.Add("chvErrorStatus", SqlDbType.VarChar);

            try
            {
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPVIPPatientSaveUpdate", HshIn, houtPara);
                return Convert.ToString(houtPara["chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveUpdateATDPackage(int HospitalId, int RegId, int EncNo, int PackageId, DateTime PackageDate, int Encodedby)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalId);
            HshIn.Add("intRegistrationId", RegId);
            HshIn.Add("intEncounterNo", EncNo);
            HshIn.Add("intPackageId", PackageId);
            HshIn.Add("dtPackgaeDate", PackageDate);
            HshIn.Add("intEncodedBy", Encodedby);
            houtPara.Add("chvErrormasseg", SqlDbType.VarChar);
            try
            {
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPATDPackageSaveUpdate", HshIn, houtPara);
                return Convert.ToString(houtPara["chvErrormasseg"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetIPPackage()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "SELECT pd.PackageID,ios.ServiceName AS PackageName FROM PackageServiceDetails pd WITH (NOLOCK) " +
                                                 " INNER JOIN ItemOfService ios WITH (NOLOCK) ON pd.PackageID =ios.ServiceId WHERE PackageType IN('I','B') ");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetPatientpackage(int HospitalId, int RegId, int EncNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@chvEncounterNo", EncNo);

                return objDl.FillDataSet(CommandType.Text, "Select apd.Id,pr.RegistrationNo,EncounterNo,apd.RegistrationId,apd.PackageId,ios.ServiceName As PackageName ,Convert(varchar,apd.PackageDate,103) PackageDate  from AdmissionPackageDetails apd WITH (NOLOCK) " +
                                                      " INNER JOIN Registration pr WITH (NOLOCK) ON apd.RegistrationId=pr.Id " +
                                                      " INNER JOIN PackageServiceDetails psd WITH (NOLOCK) ON apd.PackageId=psd.PackageID " +
                                                      " INNER JOIN ItemOfService ios WITH (NOLOCK) ON psd.PackageID=ios.ServiceId" +
                                                      " WHERE apd.EncounterNo=@chvEncounterNo And apd.RegistrationId=@intRegistrationId And apd.Active=1", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string DeletePatientPackage(int Id)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {

                houtPara.Add("chvErrorStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.Text, "UPDATE AdmissionPackageDetails SET Active=0 WHERE Id=" + Id + "", HshIn, houtPara);
                return Convert.ToString(houtPara["chvErrormasseg"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetNonAdmitedpatient(int HospitalId, int FacilityId, string strname, string strstatus)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("inyHospitalLocationId", HospitalId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("strName", strname);
                HshIn.Add("strStatus", strstatus);
                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchNonAdmitedPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPAdress(int HospitalId, int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intHospId", HospitalId);
                HshIn.Add("@intRegistrationId", RegId);

                return objDl.FillDataSet(CommandType.Text, "select LocalCountry,LocalState,LocalCity,Localpin FROM Registration WITH (NOLOCK) where id=@intRegistrationId and HospitalLocationId=@intHospId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetReportTypes(int iHospitalId, string iReprtType, int Facitlityid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("inyHospitalLocationId", iHospitalId);
                HshIn.Add("chrReprtType", iReprtType);
                HshIn.Add("intFacilityId", Facitlityid);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetReportType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReportTypes(int iHospitalId, string iReprtType, int Facitlityid, string chrFromDate, string chrToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospitalId);
                HshIn.Add("chrReprtType", iReprtType);
                HshIn.Add("intFacilityId", Facitlityid);
                HshIn.Add("chrFromDate", chrFromDate);
                HshIn.Add("chrToDate", chrToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetReportType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetBedStatus()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string str = "select Code,Status from StatusMaster WITH (NOLOCK) where StatusType='Bedstatus' and Active=1";
                return objDl.FillDataSet(CommandType.Text, str);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetHospitalBedStatus(int iHospitalId, string Bedstatus, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospitalId);
                HshIn.Add("chrBedStatus", Bedstatus);
                HshIn.Add("intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBedStilOccupied", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveUpdateBedstatus(int HospitalId, int RegId, int EncId, int BedId, string status, int Encodedby, int FacilityId, string Type)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                HshIn.Add("inyHospitalLocationId", HospitalId);
                HshIn.Add("intRegistrationId", RegId);
                HshIn.Add("intEncounterId", EncId);
                HshIn.Add("intBedId", BedId);
                HshIn.Add("chrBedStatus", status);
                HshIn.Add("intEncodedBy", Encodedby);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("vcType", Type);
                houtPara.Add("chvErrormasseg", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPChangeBedstatus", HshIn, houtPara);
                return Convert.ToString(houtPara["chvErrormasseg"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        /// <summary>
        /// Save Admission
        /// Function name -SaveAdmission
        /// Create By Santosh Chaurasia
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <param name="Regid"></param>
        /// <param name="Regno"></param>
        /// <param name="Wardid"></param>
        /// <param name="Bedcategory"></param>
        /// <param name="BillingCategory"></param>
        /// <param name="Bedid"></param>
        /// <param name="admitingdocId"></param>
        /// <param name="Admitingteam"></param>
        /// <param name="ConsultingDocId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="cwardid"></param>
        /// <param name="cBedcategory"></param>
        /// <param name="cBillingcategry"></param>
        /// <param name="cBedid"></param>
        /// <param name="sourceId"></param>
        /// <param name="PayerType"></param>
        /// <param name="Guarantorid"></param>
        /// <param name="KinId"></param>
        /// <param name="Fname"></param>
        /// <param name="Mname"></param>
        /// <param name="Lname"></param>
        /// <param name="Kindob"></param>
        /// <param name="KinGender"></param>
        /// <param name="KinAddress1"></param>
        /// <param name="KinAddress2"></param>
        /// <param name="KinCountery"></param>
        /// <param name="KinState"></param>
        /// <param name="KinCity"></param>
        /// <param name="kinArea"></param>
        /// <param name="Kinzip"></param>
        /// <param name="kinhome"></param>
        /// <param name="kinMob"></param>
        /// <param name="kinEmail"></param>
        /// <param name="PatientDaignosis"></param>
        /// <param name="RefBy"></param>
        /// <param name="Refid"></param>
        /// <param name="problestayduration"></param>
        /// <param name="BookingId"></param>
        /// <param name="Advicestatus"></param>
        /// <param name="Pendingadmssionadvice"></param>
        /// <param name="Admissiondate"></param>
        /// <param name="EncodedBy"></param>
        /// <param name="Encodeddate"></param>
        /// <param name="CreditId"></param>
        /// <param name="Credittype"></param>
        /// <param name="Departmentname"></param>
        /// <param name="status"></param>
        /// <param name="encounterno"></param>
        /// <returns></returns>
        public string SaveAdmission(int HospitalId, int Regid, int Regno, int Wardid, int Bedcategory, int BillingCategory, int Bedid, int admitingdocId, int Admitingteam,
            int ConsultingDocId, int FacilityId, int cwardid, int cBedcategory, int cBillingcategry, int cBedid, int sourceId, int PayerType, int Guarantorid, int KinId, string Fname,
            string Mname, string Lname, DateTime Kindob, string KinGender, string KinAddress1, string KinAddress2, int KinCountery, int KinState, int KinCity, int kinArea, string Kinzip,
            string kinhome, string kinMob, string kinEmail, string PatientDaignosis, string RefBy, int Refid, int problestayduration, int BookingId, int Advicestatus, string Pendingadmssionadvice,
            DateTime Admissiondate, int EncodedBy, DateTime Encodeddate, string CreditId, int Credittype, string Departmentname, string status, string encounterno)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveAdmission", HshIn, houtPara);

                string str = "";
                return str;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        /// <summary>
        /// UpDate Admission
        /// Function Name UpdateAdmission
        /// Create By Santosh Chaurasia
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <param name="Regid"></param>
        /// <param name="Regno"></param>
        /// <param name="Wardid"></param>
        /// <param name="Bedcategory"></param>
        /// <param name="BillingCategory"></param>
        /// <param name="Bedid"></param>
        /// <param name="admitingdocId"></param>
        /// <param name="Admitingteam"></param>
        /// <param name="ConsultingDocId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="cwardid"></param>
        /// <param name="cBedcategory"></param>
        /// <param name="cBillingcategry"></param>
        /// <param name="cBedid"></param>
        /// <param name="sourceId"></param>
        /// <param name="PayerType"></param>
        /// <param name="Guarantorid"></param>
        /// <param name="KinId"></param>
        /// <param name="Fname"></param>
        /// <param name="Mname"></param>
        /// <param name="Lname"></param>
        /// <param name="Kindob"></param>
        /// <param name="KinGender"></param>
        /// <param name="KinAddress1"></param>
        /// <param name="KinAddress2"></param>
        /// <param name="KinCountery"></param>
        /// <param name="KinState"></param>
        /// <param name="KinCity"></param>
        /// <param name="kinArea"></param>
        /// <param name="Kinzip"></param>
        /// <param name="kinhome"></param>
        /// <param name="kinMob"></param>
        /// <param name="kinEmail"></param>
        /// <param name="PatientDaignosis"></param>
        /// <param name="RefBy"></param>
        /// <param name="Refid"></param>
        /// <param name="problestayduration"></param>
        /// <param name="BookingId"></param>
        /// <param name="Advicestatus"></param>
        /// <param name="Pendingadmssionadvice"></param>
        /// <param name="Admissiondate"></param>
        /// <param name="EncodedBy"></param>
        /// <param name="Encodeddate"></param>
        /// <param name="CreditId"></param>
        /// <param name="Credittype"></param>
        /// <param name="Departmentname"></param>
        /// <param name="status"></param>
        /// <param name="encounterno"></param>
        /// <returns></returns>
        public string UpdateAdmission(int HospitalId, int Regid, int Regno, int Wardid, int Bedcategory, int BillingCategory, int Bedid, int admitingdocId, int Admitingteam,
           int ConsultingDocId, int FacilityId, int cwardid, int cBedcategory, int cBillingcategry, int cBedid, int sourceId, int PayerType, int Guarantorid, int KinId, string Fname,
           string Mname, string Lname, DateTime Kindob, string KinGender, string KinAddress1, string KinAddress2, int KinCountery, int KinState, int KinCity, int kinArea, string Kinzip,
           string kinhome, string kinMob, string kinEmail, string PatientDaignosis, string RefBy, int Refid, int problestayduration, int BookingId, int Advicestatus, string Pendingadmssionadvice,
           DateTime Admissiondate, int EncodedBy, DateTime Encodeddate, string CreditId, int Credittype, string Departmentname, string status, string encounterno)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPUpdateAdmission", HshIn, houtPara);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            string str = "";
            return str;
        }
        /// <summary>
        /// Retrive Admission against Such patient
        /// Functionname - GetAdmission
        /// Create By Santosh Chaurasia
        /// </summary>
        /// <param name="iHospitalId"></param>
        /// <param name="RegistrationId"></param>
        /// <returns></returns>

        public DataSet GetAdmission(int iHospitalId, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospitalId);
                HshIn.Add("intRegistrationId", RegistrationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPRetrieveAdmission", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// Cancel Admission
        /// FunctionName - CancelAdmission
        /// Create By Santosh Chaurasia
        /// </summary>
        /// <param name="HospitalId"></param>
        /// <param name="RegId"></param>
        /// <param name="BedId"></param>
        /// <param name="EncounterNo"></param>
        /// <param name="GuarantorId"></param>
        /// <param name="status"></param>
        /// <param name="Encodedby"></param>
        /// <returns></returns>
        public string CancelAdmission(int HospitalId, int RegId, int BedId, int EncounterNo, int GuarantorId, string status, int Encodedby)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {

                HshIn.Add("inyHospitalLocationId", HospitalId);
                HshIn.Add("intRegistrationId", RegId);
                HshIn.Add("intEncounterno", EncounterNo);
                HshIn.Add("intBedId", BedId);
                HshIn.Add("chrBedStatus", status);
                HshIn.Add("intEncodedBy", Encodedby);

                houtPara.Add("chvErrormasseg", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPChangeBedstatus", HshIn, houtPara);
                return Convert.ToString(houtPara["chvErrormasseg"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        /// <summary>
        /// Get Vacant Bed
        /// Function Name - GetVacantBed
        /// Created By - Santosh Chaurasia.
        /// </summary>
        /// <param name="IBedCat"></param>
        /// <param name="IWardno"></param>
        /// <returns></returns>
        public DataSet GetVacantBed(int IBedCat, int IWardno)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@IntBedCat", IBedCat);
                HshIn.Add("@IntWardno", IWardno);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPRetrieveAdmission", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveBedTansferRequest(int HospitalLocationID, int EncounterId, int RegistrationId
            , int FromBedCategoryId, int FromBillCategoryId, int FromBedId
            , int FromWardId, int ToBedCategoryId, int ToBillCategoryId, int ToBedId,
            int ToWardId, int UserId, DateTime TransferDate, string RequestRemarks)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                HshIn.Add("HospitalLocationId", HospitalLocationID);
                HshIn.Add("EncounterId", Convert.ToString(EncounterId));
                HshIn.Add("RegistrationId", RegistrationId);

                HshIn.Add("FromBedCategoryId", FromBedCategoryId);
                HshIn.Add("FromBillCategoryId", FromBillCategoryId);
                HshIn.Add("FromBedId", FromBedId);
                HshIn.Add("FromWardId", FromWardId);

                HshIn.Add("ToBedCategoryId", ToBedCategoryId);
                HshIn.Add("ToBillCategoryId", ToBillCategoryId);
                HshIn.Add("ToWardId", ToWardId);
                HshIn.Add("ToBedId", ToBedId);
                HshIn.Add("UserId", UserId);
                HshIn.Add("TransferDate", TransferDate);
                HshIn.Add("RequestRemarks", RequestRemarks);
                houtPara.Add("StrStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveBedTransferRequest", HshIn, houtPara);
                return Convert.ToString(houtPara["StrStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetBedTransferRequest(int hospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("HospitallocationId", hospitalLocationId);
                HshIn.Add("intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBedTransferRequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getVacantBed(int bedCategoryId, int wardId, int FacilityId)
        {

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@IntBedCat", bedCategoryId);
                HshIn.Add("@IntWardno", wardId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetVacantbedStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveBedTansfer(int HospitalLocationID, int RegistrationId, int FromBedCategoryId, int FromBillCategoryId, int FromBedId
            , int FromWardId, int ToBedCategoryId, int ToBillCategoryId, int ToBedId, int ToWardId,
            int UserId, int TransferRequistionId, DateTime TransferDate, int FacilitityId)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            try
            {
                HshIn.Add("@TransferRequistionId", TransferRequistionId);
                HshIn.Add("@HospitalLocationId", HospitalLocationID);
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@FromBedCategoryId", FromBedCategoryId);
                HshIn.Add("@FromBillCategoryId", FromBillCategoryId);
                HshIn.Add("@FromBedId", FromBedId);
                HshIn.Add("@FromWardId", FromWardId);
                HshIn.Add("@ToBedCategoryId", ToBedCategoryId);
                HshIn.Add("@ToBillCategoryId", ToBillCategoryId);
                HshIn.Add("@ToBedId", ToBedId);
                HshIn.Add("@ToWardId", ToWardId);
                HshIn.Add("@UserId", UserId);
                HshIn.Add("@TransferDate", TransferDate);
                HshIn.Add("@intFacilityId", FacilitityId);
                houtPara.Add("@StrStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveBedTransfer", HshIn, houtPara);
                return Convert.ToString(houtPara["@StrStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string SaveBedTansferClearance(int HospitalLocationID, int FacilitityId, int TransferRequistionId, int RegistrationId, int iEncounterId, int UserId)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilitityId);
                HshIn.Add("@intRequestId", TransferRequistionId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intEncodedBy", UserId);
                houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateICUClearance", HshIn, houtPara);
                return Convert.ToString(houtPara["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int CancelBedTransferRequesrt(int TransferRequestId, string strRemarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@TransferRequestId", TransferRequestId);
                HshIn.Add("@chvRemarks", strRemarks);
                string strqry = "UPDATE BedTransferRequest SET Active = 0, Remarks = @chvRemarks where Id = @TransferRequestId AND AcknowledgeBy IS NULL";
                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Edit by Rakesh this function //Add new GroupingId field on 28/08/2013 start
        public DataSet GetAdmissionReportData(int HospitalLocationID, int UserId,
                        int iFacilityId, string sFromDate, string sToDate,
                        string sFilterCateria, string sGroupHierarchy, int groupingId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@UserId", UserId);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@FilterCriteria", sFilterCateria);
            HshIn.Add("@GroupHierarchy", sGroupHierarchy);
            HshIn.Add("@GroupingId", groupingId);
            return objDl.FillDataSet(CommandType.StoredProcedure, "USPAdmissionData", HshIn);

        }
        //Edit by Rakesh this function //Add new GroupingId field on 28/08/2013 end
        public void SaveAdmissionCustomColumn(int iHospitalLocationId, int iUserId, int iFacilityId,
                                            string sCustomColumnXml, int GroupingId)
        {

            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intUserId", iUserId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlColumnData", sCustomColumnXml);
            HshIn.Add("@GroupingId", GroupingId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try { objDl.FillDataSet(CommandType.StoredProcedure, "uspSaveAdmissionCustomColumn", HshIn); } catch (Exception ex) { throw ex; } finally { HshIn = null; objDl = null; }

        }
        public void SaveDischargeCustomColumn(int iHospitalLocationId, int iUserId, int iFacilityId,
                                            string sCustomColumnXml, int GroupingId)
        {

            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intUserId", iUserId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlColumnData", sCustomColumnXml);
            HshIn.Add("@GroupingId", GroupingId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try { objDl.FillDataSet(CommandType.StoredProcedure, "uspSaveDischargeCustomColumn", HshIn); } catch (Exception ex) { throw ex; } finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDischargeReportData(int HospitalLocationID, int UserId, int iFacilityId, string sFromDate, string sToDate, string sFilterCateria, string sGroupHierarchy)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@UserId", UserId);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@FilterCriteria", sFilterCateria);
            HshIn.Add("@GroupHierarchy", sGroupHierarchy);
            try { return objDl.FillDataSet(CommandType.StoredProcedure, "USPDischargeData", HshIn); } catch (Exception ex) { throw ex; } finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDischargeDataForCancellation(DateTime dtFromDate, DateTime dttoDate, int intFacilityId, int iEncounterId, string sCType)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@FromDate", dtFromDate);
                HshIn.Add("@ToDate", dttoDate);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@iEncounterId", iEncounterId);
                HshIn.Add("@sCType", sCType);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDischargePatientList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string CancelDischarge(int HospitalLocationID, int intFacilityId, int EncounterId, int RegistrationId, int intWardId,
            int intBedcategory, int intBillingCategory, int intBedId, string bedChanged, int intUserId, string Remarks, string sCType)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("intHospitalLocationId", HospitalLocationID);
                HshIn.Add("intFacilityId", Convert.ToString(intFacilityId));
                HshIn.Add("intRegistrationId", RegistrationId);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("intWardId", intWardId);
                HshIn.Add("intBedcategory", intBedcategory);
                HshIn.Add("intBillingCategory", intBillingCategory);
                HshIn.Add("intBedId", intBedId);
                HshIn.Add("bedChanged", bedChanged);
                HshIn.Add("intUserId", intUserId);
                HshIn.Add("Remarks", Remarks);
                HshIn.Add("sCType", sCType);
                houtPara.Add("charStrStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDischargePatienCancel", HshIn, houtPara);
                return Convert.ToString(houtPara["charStrStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelBedRetain(int bedRetainId)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("intBedretainId", bedRetainId);
                houtPara.Add("StrStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelBedRetain", HshIn, houtPara);
                return Convert.ToString(houtPara["StrStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAdmissionBookingType(int BookingTypeId, string BookingTypeName, string BookingTypeValue)
        {
            DataSet objds = new DataSet();
            Hashtable hsinput = new Hashtable();
            hsinput.Add("@BookingTypeId", BookingTypeId);
            hsinput.Add("@BookingTypeName", BookingTypeName);
            hsinput.Add("@BookingTypeValue", BookingTypeValue);

            objds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetAdmissionBookingType", hsinput);
            return objds;
        }
        public Hashtable SaveBedBooking(int HospitalLocationID, int FacilitityId, int RegistrationId, int EncounterId, int BookingId,
           string BookingDate, int SourceId, string BookingType, string BookintStatus, int BedCategoryId1, int BedCategoryId2, int BedCategoryId3,
           int DocotrId, string ExpAdDate, string Remarks, string HeadCode, int UserId, int ProbableStayInDays, string ReasonForAdmission)
        {
            return SaveBedBooking(HospitalLocationID, FacilitityId, RegistrationId, EncounterId, BookingId,
            BookingDate, SourceId, BookingType, BookintStatus, BedCategoryId1, BedCategoryId2, BedCategoryId3,
            DocotrId, ExpAdDate, Remarks, HeadCode, UserId, ProbableStayInDays, ReasonForAdmission
            , string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public Hashtable SaveBedBooking(int HospitalLocationID, int FacilitityId, int RegistrationId, int EncounterId, int BookingId,
             string BookingDate, int SourceId, string BookingType, string BookintStatus, int BedCategoryId1, int BedCategoryId2, int BedCategoryId3,
             int DocotrId, string ExpAdDate, string Remarks, string HeadCode, int UserId, int ProbableStayInDays, string ReasonForAdmission
            , string ProcedureName, string ProcedureDuration, string ImplantDetails, string AnaesthesiaType, string ExpDischDate, string BedNo)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
            HshIn.Add("@intFacilityId", FacilitityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intBookingId", BookingId);
            HshIn.Add("@dtBookingDate", BookingDate);
            HshIn.Add("@intSourceId", SourceId);
            HshIn.Add("@chrBookingType", BookingType);
            HshIn.Add("@chrBookingStatus", BookintStatus);
            HshIn.Add("@intBedCategoryId1", BedCategoryId1);
            HshIn.Add("@intBedCategoryId2", BedCategoryId2);
            HshIn.Add("@intBedCategoryId3", BedCategoryId3);
            HshIn.Add("@intDoctorId", DocotrId);
            HshIn.Add("@dtExpAdmDate", ExpAdDate);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@chvHeadCode", HeadCode);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intProbableStayInDays", ProbableStayInDays);
            HshIn.Add("@chvReasonForAdmission", ReasonForAdmission);

            if (ProcedureName != "")
            {
                HshIn.Add("@chvProcedureName", ProcedureName);
            }
            if (ProcedureDuration != "")
            {
                HshIn.Add("@chvProcedureDuration", ProcedureDuration);
            }
            if (ImplantDetails != "")
            {
                HshIn.Add("@chvImplantDetails", ImplantDetails);
            }
            if (AnaesthesiaType != "")
            {
                HshIn.Add("@chvAnaesthesiaType", AnaesthesiaType);
            }

            houtPara.Add("@intId", SqlDbType.Int);
            houtPara.Add("@chvBookingNo", SqlDbType.VarChar);
            houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
            houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientBedBooking", HshIn, houtPara);
            return houtPara;
        }
        public Hashtable SaveBedBooking(int HospitalLocationID, int FacilitityId, int RegistrationId, int EncounterId, int BookingId,
           string BookingDate, int SourceId, string BookingType, string BookintStatus, int BedCategoryId1, int BedCategoryId2, int BedCategoryId3,
           int DocotrId, string ExpAdDate, string Remarks, string HeadCode, int UserId, int ProbableStayInDays, string ReasonForAdmission, string InternalRemark, double bookingAmt, int ReceiptId, int iWardId)
        {//// changed
            HshIn = new Hashtable();
            houtPara = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
            HshIn.Add("@intFacilityId", FacilitityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intBookingId", BookingId);
            HshIn.Add("@dtBookingDate", BookingDate);
            HshIn.Add("@intSourceId", SourceId);
            HshIn.Add("@chrBookingType", BookingType);
            HshIn.Add("@chrBookingStatus", BookintStatus);
            HshIn.Add("@intBedCategoryId1", BedCategoryId1);
            HshIn.Add("@intBedCategoryId2", BedCategoryId2);
            HshIn.Add("@intBedCategoryId3", BedCategoryId3);
            HshIn.Add("@intDoctorId", DocotrId);
            HshIn.Add("@dtExpAdmDate", ExpAdDate);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@chvHeadCode", HeadCode);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intProbableStayInDays", ProbableStayInDays);
            HshIn.Add("@chvReasonForAdmission", ReasonForAdmission);

            HshIn.Add("@InternalRemarks", InternalRemark);
            HshIn.Add("@BookingAmt", bookingAmt);
            HshIn.Add("@intReceiptId", ReceiptId);
            HshIn.Add("@iWardId", iWardId);
            houtPara.Add("@intId", SqlDbType.Int);
            houtPara.Add("@chvBookingNo", SqlDbType.VarChar);
            houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
            houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientBedBooking", HshIn, houtPara);
            return houtPara;
        }

        public DataSet GetPatientBedBookingDetails(int HospitalLocationID, int iFacilityId, string sFromDate, string sToDate,
           int iBookingId, string sBookingNo, string RegNo, string DoctorName, string BookingType, string ReportType)
        {
            return GetPatientBedBookingDetails(HospitalLocationID, iFacilityId, sFromDate, sToDate,
            iBookingId, sBookingNo, RegNo, DoctorName, BookingType, ReportType, 0);
        }

        public DataSet GetPatientBedBookingDetails(int HospitalLocationID, int iFacilityId, string sFromDate, string sToDate,
            int iBookingId, string sBookingNo, string RegNo, string DoctorName, string BookingType, string ReportType, int EncounterId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationID", HospitalLocationID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@intBookingId", iBookingId);
                HshIn.Add("@chvBookingNo", sBookingNo);
                HshIn.Add("@chvRegistrationNo", RegNo);
                HshIn.Add("@chvDoctorName", DoctorName);
                HshIn.Add("@chrBookingType", BookingType);
                HshIn.Add("@reportType", ReportType);
                HshIn.Add("@EncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetAdmissionRequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //Added by Rakesh Start
        public DataSet getDynamicReportGrouping(int ReportId, int UserId, int FacilityId, int HospitalLocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);

                string str = "SELECT GroupingId, ReportGroupName FROM DynamicReportGrouping WITH (NOLOCK) WHERE ReportId = @intReportId AND UserId = @intUserId AND Active = 1 AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public int SaveReportGrouping(int ReportId, string ReportGroupName, int FacilitityId, int HospitalLocationID, int UserId, int Active, int GroupingId)
        {
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@ReportId", ReportId);
                HshIn.Add("@ReportGroupName", ReportGroupName);
                HshIn.Add("@FacilityId", FacilitityId);
                HshIn.Add("@HospitalLocationId", HospitalLocationID);
                HshIn.Add("@UserId", UserId);
                HshIn.Add("@Active", Active);
                HshIn.Add("@GroupingId", GroupingId);
                return  objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveReportGrouping", HshIn);
               
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetReportGrouping(int ReportId, int UserId, int FacilityId, int HospitalLocationId, int GroupingId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intGroupingId", GroupingId);

                string str = "SELECT ReportGroupName,Active FROM DynamicReportGrouping WITH (NOLOCK) WHERE GroupingId=@intGroupingId AND ReportId =@intReportId AND UserId = @intUserId AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDateFilterType()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string str = "SELECT ID,Description FROM DateFilterType WITH (NOLOCK) WHERE Active=1 ";
                return objDl.FillDataSet(CommandType.Text, str);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDateByFilterTypeWise(int DateFilterTypeID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@DateTypeId", DateFilterTypeID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDynamicgetfilterdate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int SaveReportGroupingFilter(int ReportId, string ReportGroupFilterName, string ReportGroupFilterString
            , int FacilitityId, int HospitalLocationID, int UserId, int Active, int GroupingId, int FilterId, int DateTypeId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@ReportId", ReportId);
                HshIn.Add("@ReportGroupFilterName", ReportGroupFilterName);
                HshIn.Add("@ReportGroupFilterString", ReportGroupFilterString);
                HshIn.Add("@FacilityId", FacilitityId);
                HshIn.Add("@HospitalLocationId", HospitalLocationID);
                HshIn.Add("@UserId", UserId);
                HshIn.Add("@Active", Active);
                HshIn.Add("@GroupingId", GroupingId);
                HshIn.Add("@FilterId", FilterId);
                HshIn.Add("@DateTypeId", DateTypeId);
                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveReportGroupingFilter", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFilterNames(int ReportId, int UserId, int FacilityId, int HospitalLocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);

                string str = "SELECT FilterId,ReportGroupFilterName,ReportGroupFilterString,Active FROM DynamicReportGroupingFilter WITH (NOLOCK) WHERE ReportId =@intReportId AND UserId = @intUserId AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetFilterNaming(int ReportId, int UserId, int FacilityId, int HospitalLocationId, int FilterId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@FilterId", FilterId);

                string str = "SELECT GroupingId,ReportGroupFilterName,ReportGroupFilterString,Active,DateTypeId FROM DynamicReportGroupingFilter WITH (NOLOCK) WHERE FilterId=@filterID AND ReportId =@intReportId AND UserId = @intUserId AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int SaveDischargeChecklist(int inyHospitalLocationId, int intFacility, int intEncodedBy, int intRegistrationId, string XMLChecklist,int intEncounterId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
                HshIn.Add("@intFacility", intFacility);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intRegistrationId", intRegistrationId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@XMLChecklist", XMLChecklist);
                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSaveDischargeChecklist", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet CheckOTChecklist(int orderid)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intBookingOrderid", orderid);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckOTChecklist", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by Rakesh End

        //Added by Balkishan Start

        public DataSet getGroupValueMain(int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAdmissionGroupValueMain", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getGroupValueDetails(int GroupId, int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAdmissionGroupValueDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveItemFieldMaster(int FieldId, string FieldName, string FieldCode, string FieldType,
                                         int FieldLength, int GroupId, int DecimalPlaces, int HospId, int Active, int EncodedBy, string MasterOption)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@chvFieldName", FieldName.Trim());
            HshIn.Add("@chvFieldCode", FieldCode.Trim());
            HshIn.Add("@chvFieldType", FieldType.Trim());
            HshIn.Add("@intFieldLength", FieldLength);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@intDecimalPlaces", DecimalPlaces);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@MasterOption", MasterOption);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveAdmissionItemFieldMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetidNo(string ipno)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intipno", ipno);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select isnull(EncounterId,0) from Admission WITH (NOLOCK) where EncounterNo=@intipno", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAdmissionFieldDetails(int AdmissionId, bool OnlyTagged, string MasterOption)
        {

            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@AdmissionId", AdmissionId);
                HshIn.Add("@MasterOption", MasterOption);



                string qry = "SELECT ifm.FieldId, ifm.FieldName, ifm.FieldType, " +
                            " ifm.FieldLength, ifm.GroupId, ifm.DecimalPlaces, ifd.ValueText, ifd.ValueId, ifd.ValueWordProcessor " +
                            " FROM PhrCustomFieldsMaster ifm WITH (NOLOCK) LEFT OUTER JOIN AdmissionCustomFieldDetails ifd WITH (NOLOCK) ON ifm.FieldId = ifd.FieldId AND ifd.Active = 1 AND ifd.EncounterId = @AdmissionId " +
                            " WHERE MasterOption=@MasterOption AND ifm.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(ifd.EncounterId, 0) <> 0 ";
                }

                qry += " ORDER BY ifm.SequenceNo";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        //updated by rakesh for  custom field on 4/7/2014 start

        public string SaveAdditionalfieldsMaster(string saveFor, int EncounterId, int HospitalLocationId, string xmlTaxDetails, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                if (saveFor == "SM")
                {
                    HshIn.Add("@EncounterId", EncounterId);
                    HshIn.Add("@xmlTaxDetails", xmlTaxDetails);
                    HshIn.Add("@intEncodedBy", EncodedBy);
                    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAdditionalfieldsSaveAdmission", HshIn, HshOut);


                }
                else if (saveFor.Equals("Registration"))
                {
                    HshIn.Add("@RegId", EncounterId);
                    HshIn.Add("@xmlTaxDetails", xmlTaxDetails);
                    HshIn.Add("@intEncodedBy", EncodedBy);
                    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAdditionalfieldsSaveRegistration", HshIn, HshOut);
                }

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //updated by rakesh for  custom field on 4/7/2014 end

        public DataSet getRegistrationFieldDetails(int AdmissionId, bool OnlyTagged, string MasterOption)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@AdmissionId", AdmissionId);
                HshIn.Add("@MasterOption", MasterOption);

                string qry = "SELECT ifm.FieldId, ifm.FieldName, ifm.FieldType, " +
                            " ifm.FieldLength, ifm.GroupId, ifm.DecimalPlaces, ifd.ValueText, ifd.ValueId, ifd.ValueWordProcessor " +
                            " FROM PhrCustomFieldsMaster ifm WITH (NOLOCK) LEFT OUTER JOIN RegistrationCustomFieldDetails ifd WITH (NOLOCK) ON ifm.FieldId = ifd.FieldId AND ifd.Active = 1 AND ifd.RegId = @AdmissionId " +
                            " WHERE MasterOption = @MasterOption AND ifm.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(ifd.RegId, 0) <> 0 ";
                }

                qry += " ORDER BY ifm.SequenceNo";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public int inActivePatientBooking(int BookingId, int EncodedBy)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intBookingId", BookingId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                string strQry = "UPDATE PatientBooking SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE Id = @intBookingId";

                return objDl.ExecuteNonQuery(CommandType.Text, strQry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int UpdateACKByWard(int TransferRequestId, string strRemarks)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@TransferRequestId", TransferRequestId);
                HshIn.Add("@chvRemarks", strRemarks);
                string strqry = "UPDATE BedTransfer SET IsACKByWard = 1, Remarks = @chvRemarks where Id = @TransferRequestId and isnull(IsACKByWard,0) = 0";
                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getRemarksBedTransferRequest(int HospitalLocationId, int FacilityId, int TransferRequestId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityId);
                HshIn.Add("@TransferRequestId", TransferRequestId);
                string str = "SELECT Remarks FROM BedTransferRequest WITH (NOLOCK) WHERE Active=1 AND HospitalLocationId=@inyHospitalLocationId AND Id=@TransferRequestId";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveBedTansfer(int HospitalLocationID, int RegistrationId, int FromBedCategoryId, int FromBillCategoryId, int FromBedId
          , int FromWardId, int ToBedCategoryId, int ToBillCategoryId, int ToBedId, int ToWardId,
          int UserId, int TransferRequistionId, DateTime TransferDate, int FacilitityId, DateTime ActualTransferDate)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();


            HshIn.Add("@TransferRequistionId", TransferRequistionId);
            HshIn.Add("@HospitalLocationId", HospitalLocationID);
            HshIn.Add("@RegistrationId", RegistrationId);
            HshIn.Add("@FromBedCategoryId", FromBedCategoryId);
            HshIn.Add("@FromBillCategoryId", FromBillCategoryId);
            HshIn.Add("@FromBedId", FromBedId);
            HshIn.Add("@FromWardId", FromWardId);
            HshIn.Add("@ToBedCategoryId", ToBedCategoryId);
            HshIn.Add("@ToBillCategoryId", ToBillCategoryId);
            HshIn.Add("@ToBedId", ToBedId);
            HshIn.Add("@ToWardId", ToWardId);
            HshIn.Add("@UserId", UserId);
            HshIn.Add("@TransferDate", TransferDate);
            HshIn.Add("@ActualTransferDate", ActualTransferDate);
            HshIn.Add("@intFacilityId", FacilitityId);
            houtPara.Add("@StrStatus", SqlDbType.VarChar);
            try
            {
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveBedTransfer", HshIn, houtPara);
                return Convert.ToString(houtPara["@StrStatus"]);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetExpiredReason()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "upsGetExpiredReason", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int UpdateBedTransferReqCheckOut(int TransferRequestId, string strRemarks, int CheckOutBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@TransferRequestId", TransferRequestId);
                HshIn.Add("@chvRemarks", strRemarks);
                HshIn.Add("@CheckOutBy", CheckOutBy);
                string strqry = "UPDATE bedtransferrequest SET CheckOut = 1,CheckOutBy=@CheckOutBy, Remarks = @chvRemarks,CheckOutDate=GETUTCDATE() where Id = @TransferRequestId and isnull(CheckOut,0) = 0";
                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int UpdateBedTransferReqCheckIn(int TransferRequestId, string strRemarks, int CheckInBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@TransferRequestId", TransferRequestId);
                HshIn.Add("@chvRemarks", strRemarks);
                HshIn.Add("@CheckInBy", CheckInBy);
                string strqry = "UPDATE bedtransferrequest SET CheckIn = 1,CheckInBy=@CheckInBy, Remarks = @chvRemarks where Id = @TransferRequestId and isnull(CheckIn,0) = 0";
                return objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }

}