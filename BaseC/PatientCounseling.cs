using System;
using System.Data;
using System.Collections;
using System.Configuration;

namespace BaseC
{
    public class PatientCounseling
    {
        DAL.DAL objDl;
        private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
        Hashtable HshIn;
        public PatientCounseling()
        {

        }
        //EstimationLOS ExpAlert WardDetailPage
        public DataSet GetCounselingDetails(int HospitalLocationId, int FacilityId, int CounselingId, int Active, int RegId, string WardLOSData)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@RegId", RegId);
                HshIn.Add("@WardLOSData", WardLOSData);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCounselingDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //EstimationLOS ExpAlert WardDetailPage

        public DataSet GetCounselingDetails(int HospitalLocationId, int FacilityId, int CounselingId, int Active, int RegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@RegId", RegId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCounselingDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetcounsellingDetailsHead(int HospitalLocationId, int FacilityId, int CounselingId, string typeID)
        {
            //UspgetestimationDetails

            var HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@type", typeID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspgetestimationDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public String SaveCounselingMain(int CounselingId
            , int HospitalLocationId, int FacilityId, string BillCategory, int RegistrationId, int UnregisteredPatientId,
           DateTime CounselingDate, string AboutIllness, int IsAdmissionSuggested, double CostOfTreatment,
            DateTime NextExpectedVisit, int BedCategoryId, double DrugCharges, double ConsumableCharges, double MiscCharges, string Remarks,
            int LOS, int UserId, string TreatmentServicesDetails, string BedCategoryWiseServicesDetails, int PatientBookingId, string DoctorName, string ManagementRemarks)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBillCategory", BillCategory);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intPatientUnregisteredId", UnregisteredPatientId);
            HshIn.Add("@dtCounselingDate", CounselingDate);
            HshIn.Add("@chvAboutIllness", AboutIllness);
            HshIn.Add("@bitIsAdmissionSuggested", IsAdmissionSuggested);
            HshIn.Add("@monCostOfTreatment", CostOfTreatment);
            HshIn.Add("@dtNextExpectedVisit", NextExpectedVisit);
            HshIn.Add("@intBedCategoryId", BedCategoryId);
            HshIn.Add("@monDrugCharges", DrugCharges);
            HshIn.Add("@monConsumableCharges", ConsumableCharges);
            HshIn.Add("@monMiscCharges", MiscCharges);
            HshIn.Add("@chvRemarks", Remarks);
            // HshIn.Add("@bitActive", Active);
            HshIn.Add("@LOS", LOS);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@CounselingId", SqlDbType.VarChar);
            HshOut.Add("@chvCounselingNo", SqlDbType.VarChar);
            HshIn.Add("@xmlTreatmentServicesDetails", TreatmentServicesDetails);
            HshIn.Add("@xmlBedCategoryWiseServiceDetails", BedCategoryWiseServicesDetails);
            HshIn.Add("@intPatientBookingId", PatientBookingId);
            HshIn.Add("@DoctorName", DoctorName);
            HshIn.Add("@ManagementRemarks", ManagementRemarks);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCounselingMain", HshIn, HshOut);
                return HshOut["@CounselingId"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // Paras changes By Changes
        public Hashtable SaveCounselingMain(int CounselingId
                    , int HospitalLocationId, int FacilityId, string BillCategory, int RegistrationId, int UnregisteredPatientId,
                   DateTime CounselingDate, string AboutIllness, int IsAdmissionSuggested, double CostOfTreatment,
                    DateTime NextExpectedVisit, int BedCategoryId, double DrugCharges, double ConsumableCharges, double MiscCharges, string Remarks,
                    int LOS, int UserId, string TreatmentServicesDetails, string BedCategoryWiseServicesDetails, int PatientBookingId,
                    string DoctorName, string ManagementRemarks, int PayerId, int SponserId, string UniqueValue, char Bookingtype
                    , int WardLOS, int ICULOS, DateTime EDOAdmission, int SourceId)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBillCategory", BillCategory);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intPatientUnregisteredId", UnregisteredPatientId);
            HshIn.Add("@dtCounselingDate", CounselingDate);
            HshIn.Add("@chvAboutIllness", AboutIllness);
            HshIn.Add("@bitIsAdmissionSuggested", IsAdmissionSuggested);
            HshIn.Add("@monCostOfTreatment", CostOfTreatment);
            HshIn.Add("@dtNextExpectedVisit", NextExpectedVisit);
            HshIn.Add("@intBedCategoryId", BedCategoryId);
            HshIn.Add("@monDrugCharges", DrugCharges);
            HshIn.Add("@monConsumableCharges", ConsumableCharges);
            HshIn.Add("@monMiscCharges", MiscCharges);
            HshIn.Add("@chvRemarks", Remarks);
            // HshIn.Add("@bitActive", Active);
            HshIn.Add("@LOS", LOS);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@CounselingId", SqlDbType.VarChar);
            HshOut.Add("@chvCounselingNo", SqlDbType.VarChar);
            HshIn.Add("@xmlTreatmentServicesDetails", TreatmentServicesDetails);
            HshIn.Add("@xmlBedCategoryWiseServiceDetails", BedCategoryWiseServicesDetails);
            HshIn.Add("@intPatientBookingId", PatientBookingId);
            HshIn.Add("@DoctorName", DoctorName);
            HshIn.Add("@ManagementRemarks", ManagementRemarks);
            HshIn.Add("@PayerId", PayerId);
            HshIn.Add("@SponserId", SponserId);
            HshIn.Add("@Bookingtype", Bookingtype);
            HshIn.Add("@WardLOS", WardLOS);
            HshIn.Add("@ICULOS", ICULOS);
            HshIn.Add("@EDOAdmission", EDOAdmission);
            HshIn.Add("@SourceId", SourceId);
            HshIn.Add("@UniqueValue", UniqueValue);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCounselingMain", HshIn, HshOut);
            // return HshOut["@CounselingId"].ToString();
            return HshOut;
        }
        // Paras changes By Changes
        public String SaveCounselingMain(int CounselingId
           , int HospitalLocationId, int FacilityId, string BillCategory, int RegistrationId, int UnregisteredPatientId,
          DateTime CounselingDate, string AboutIllness, int IsAdmissionSuggested, double CostOfTreatment,
           DateTime NextExpectedVisit, int BedCategoryId, double DrugCharges, double ConsumableCharges, double MiscCharges, string Remarks,
           int LOS, int UserId, string TreatmentServicesDetails, string BedCategoryWiseServicesDetails, int PatientBookingId, string DoctorName, string ManagementRemarks, string UniqueValue, int iDoctorId)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBillCategory", BillCategory);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intPatientUnregisteredId", UnregisteredPatientId);
            HshIn.Add("@dtCounselingDate", CounselingDate);
            HshIn.Add("@chvAboutIllness", AboutIllness);
            HshIn.Add("@bitIsAdmissionSuggested", IsAdmissionSuggested);
            HshIn.Add("@monCostOfTreatment", CostOfTreatment);
            HshIn.Add("@dtNextExpectedVisit", NextExpectedVisit);
            HshIn.Add("@intBedCategoryId", BedCategoryId);
            HshIn.Add("@monDrugCharges", DrugCharges);
            HshIn.Add("@monConsumableCharges", ConsumableCharges);
            HshIn.Add("@monMiscCharges", MiscCharges);
            HshIn.Add("@chvRemarks", Remarks);
            // HshIn.Add("@bitActive", Active);
            HshIn.Add("@LOS", LOS);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@CounselingId", SqlDbType.VarChar);
            HshOut.Add("@chvCounselingNo", SqlDbType.VarChar);
            HshIn.Add("@xmlTreatmentServicesDetails", TreatmentServicesDetails);
            HshIn.Add("@xmlBedCategoryWiseServiceDetails", BedCategoryWiseServicesDetails);
            HshIn.Add("@intPatientBookingId", PatientBookingId);
            HshIn.Add("@DoctorName", DoctorName);
            HshIn.Add("@ManagementRemarks", ManagementRemarks);
            HshIn.Add("@UniqueValue", UniqueValue);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCounselingMain", HshIn, HshOut);
                return HshOut["@CounselingId"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // Paras changes By Changes
        public String SaveCounselingMain(int CounselingId
                  , int HospitalLocationId, int FacilityId, string CounselingNo, int RegistrationId, int UnregisteredPatientId,
                 DateTime CounselingDate, string AboutIllness, int IsAdmissionSuggested, double CostOfTreatment,
                  DateTime NextExpectedVisit, int BedCategoryId, double DrugCharges, double ConsumableCharges, double MiscCharges, string Remarks,
                  int LOS, int UserId, string TreatmentServicesDetails, int PatientBookingId, string UniqueValue)
        {
            return SaveCounselingMain(CounselingId, HospitalLocationId, FacilityId, CounselingNo, RegistrationId, UnregisteredPatientId, CounselingDate, AboutIllness, IsAdmissionSuggested, CostOfTreatment, NextExpectedVisit, BedCategoryId, DrugCharges, ConsumableCharges, MiscCharges, Remarks, LOS, UserId, TreatmentServicesDetails, PatientBookingId, UniqueValue, "");
        }
        public String SaveCounselingMain(int CounselingId
                  , int HospitalLocationId, int FacilityId, string CounselingNo, int RegistrationId, int UnregisteredPatientId,
                 DateTime CounselingDate, string AboutIllness, int IsAdmissionSuggested, double CostOfTreatment,
                  DateTime NextExpectedVisit, int BedCategoryId, double DrugCharges, double ConsumableCharges, double MiscCharges, string Remarks,
                  int LOS, int UserId, string TreatmentServicesDetails, int PatientBookingId, string UniqueValue, string DoctorName)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intPatientUnregisteredId", UnregisteredPatientId);
            HshIn.Add("@dtCounselingDate", CounselingDate);
            HshIn.Add("@chvAboutIllness", AboutIllness);

            HshIn.Add("@bitIsAdmissionSuggested", IsAdmissionSuggested);
            HshIn.Add("@monCostOfTreatment", CostOfTreatment);

            HshIn.Add("@dtNextExpectedVisit", NextExpectedVisit);

            HshIn.Add("@intBedCategoryId", BedCategoryId);
            HshIn.Add("@monDrugCharges", DrugCharges);
            HshIn.Add("@monConsumableCharges", ConsumableCharges);
            HshIn.Add("@monMiscCharges", MiscCharges);

            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@LOS", LOS);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlTreatmentServicesDetails", TreatmentServicesDetails);
            HshIn.Add("@intPatientBookingId", PatientBookingId);
            HshIn.Add("@UniqueValue", UniqueValue);
            HshIn.Add("@DoctorName", DoctorName);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@CounselingId", SqlDbType.VarChar);
            HshOut.Add("@chvCounselingNo", SqlDbType.VarChar);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCounselingMain", HshIn, HshOut);

                return HshOut["@CounselingId"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public String ChangeCounselingStatus(int CounselingId, int Active)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@bitActive", Active);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBChangeCounselingStatus", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetRoomcharge(int companyid, int facilityid, int bedcategoryID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.ExecuteScalar(CommandType.Text, "exec UspGetRoomchargePatEst @companyid=" + companyid.ToString() + ",@bedcategoryID=" + bedcategoryID.ToString() + ",@facilityID=" + facilityid.ToString()).ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientCounselingHeads(int HospitalLocationId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientCounselingHeads", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet uspGetCounselingDetails(int intCounselingId, int bitActive)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("intCounselingId", intCounselingId);
            HshIn.Add("bitActive", bitActive);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public String SaveCounselingServiceDetails(int HospitalLocationId, int FacilityId, int RegistrationNo, int EnoucnterId, String ServiceType, int UserId, string TreatmentServicesDetails, string UniqueValue)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            HshIn.Add("@intEncounterId", EnoucnterId);
            HshIn.Add("@ServiceType", ServiceType);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@xmlTreatmentServicesDetails", TreatmentServicesDetails);
            HshIn.Add("@UniqueValue", UniqueValue);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCounselingServiceDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCounselingServiceDetails(int HospitalLocationId, int FacilityId, int RegistrationNo, int EncounterId, String ServiceType, string UniqueValue)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            HshIn.Add("intEncounterId", EncounterId);
            HshIn.Add("ServiceType", ServiceType);
            HshIn.Add("UniqueValue", UniqueValue);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingServiceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCounselingServiceDetails(int HospitalLocationId, int FacilityId, int RegistrationNo, int EncounterId, string ServiceType, int CounSelingId, string BedCategory, string UniqueValue)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            HshIn.Add("intEncounterId", EncounterId);
            HshIn.Add("ServiceType", ServiceType);
            HshIn.Add("CounSelingId", CounSelingId);
            HshIn.Add("BedCategory", BedCategory); ;
            HshIn.Add("UniqueValue", UniqueValue);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingServiceDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCounselingServiceDetails(int HospitalLocationId, int FacilityId, int RegistrationNo, string BedCategory)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            HshIn.Add("BedCategory", BedCategory);
            //HshIn.Add("intEncounterId", EncounterId);
            //HshIn.Add("ServiceType", ServiceType);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingAllServiceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //Khan
        public DataSet GetCounselingServiceDetails(int HospitalLocationId, int FacilityId, int RegistrationNo, string BedCategory, string UniqueValue)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            HshIn.Add("BedCategory", BedCategory);
            HshIn.Add("UniqueValue", UniqueValue);
            //HshIn.Add("ServiceType", ServiceType);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingAllServiceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public String RemoveCounselingServiceDetails(int HospitalLocationId, int FacilityId, int RegistrationNo, string ServiceId)
        {
            var HshIn = new Hashtable();
            var HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@ServiceId", ServiceId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspRemoveCounselingServiceDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetRoomchargeForCounseling(int companyid, int BedCategory, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("facilityID", FacilityId);
            HshIn.Add("companyid", companyid);
            HshIn.Add("bedcategoryID", BedCategory);
            //HshIn.Add("intEncounterId", EncounterId);
            //HshIn.Add("ServiceType", ServiceType);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRoomchargeForCounseling", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        // Paras changes by Chandan
        public DataSet GetCounselingDetailsWithRegId(int HospitalLocationId, int FacilityId, int CounselingId, int RegId)
        {
            DataSet ds = new DataSet();
            try
            {
                var HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@RegId", RegId);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingDetailsWithRegId", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }
        public DataSet GetBillEstimateDoctorList(Int16 HospitalId, Int16 intSpecialisationId, Int16 FacilityId, int Active)
        {
            var hshIn = new Hashtable();
            hshIn.Add("HospitalLocationId", HospitalId);
            hshIn.Add("intSpecialisationId", intSpecialisationId);
            hshIn.Add("intFacilityId", FacilityId);
            hshIn.Add("Active", Active);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBillEstimateDoctorList", hshIn);
        }
        // Paras changes by Chandan
    }
}