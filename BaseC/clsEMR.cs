using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace BaseC
{
    public class clsEMR
    {
        private string sConString = string.Empty;
        public clsEMR(string conString)
        {
            sConString = conString;
        }



        public DataSet getSpecialisationMaster(int SpecialisationId, int HospId, int FacilityId, int Active)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intSpecialisationId", SpecialisationId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSpecialisationMaster", HshIn);
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


        public DataSet getBankMaster(int BankId, int HospId, int FacilityId, int Active)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intBankId", BankId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBankMaster", HshIn);
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
        public DataSet getDiagDoctorOutsideLab(int FacilityId, int DoctorID)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intDoctorID", DoctorID);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDiagDoctorOutsideLab", HshIn);
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
        public string SaveBankMaster(int BankId, int HospId, int FacilityId, string Name, string Address, string Phone,
                                        string Fax, int Active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intBankId", BankId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvName", Name);
                HshIn.Add("@chvAddress", Address);
                HshIn.Add("@chvPhone", Phone);
                HshIn.Add("@chvFax", Fax);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveBankMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public Hashtable SaveEMROutsideLabResultSetup(int FacilityID, string XMLFieldID, int DoctorID, int ServiceID, int EncodedBy, int LabResultSetupId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inFacilityID", FacilityID);
                HshIn.Add("@xmlFieldDetails", XMLFieldID);
                HshIn.Add("@intDoctorID", DoctorID);
                HshIn.Add("@intServiceID", ServiceID);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intLabResultSetupId", LabResultSetupId);
                HshOut.Add("@chvErrorOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMROutsideLabResultSetup", HshIn, HshOut);
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
            return HshOut;
        }
        public string SaveSpecialisationMaster(int SpecialisationId, int HospId, int FacilityId, string SpecialisationName,
                                                int Active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intSpecialisationId", SpecialisationId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvName", SpecialisationName);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSpecialisationMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public DataSet getScreeningParameters(int EncounterId, int RegistrationId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "DRCScreeningParameters", HshIn);
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

        public DataSet getEncounterList(int EncounterId, int RegistrationId, int FacilityId, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder qry = new StringBuilder();
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospId", HospId);

                qry.Append("SELECT Id AS EncounterId, EncounterNo, OPIP, RegistrationId, RegistrationNo, DoctorId ");
                qry.Append(" FROM Encounter WITH (NOLOCK) ");
                qry.Append(" WHERE Id = (CASE WHEN @intEncounterId=0 THEN Id ELSE @intEncounterId END) ");
                qry.Append(" AND RegistrationId = (CASE WHEN @intRegistrationId=0 THEN RegistrationId ELSE @intRegistrationId END) ");
                qry.Append(" AND Active = 1");
                qry.Append(" AND FacilityID = @intFacilityId ");
                qry.Append(" AND HospitalLocationId = @intHospId ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                qry = null;
                HshIn = null;
            }
            return ds;
        }

        public string SaveFavouriteDrugs(int DoctorId, int ItemId, string FormularyType, int EncodedBy,
                                        double Dose, int UnitId, int StrengthId, int FormulationId, int RouteId,
                                        int FrequencyId, int Duration, string DurationType, int FoodRelationshipId,
                                        string StrengthValue, string Instructions, int GenericId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@Drug_Syn_Id", ItemId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chrFormularyType", FormularyType);
                HshIn.Add("@intStrengthId", StrengthId);
                HshIn.Add("@chvStrengthValue", StrengthValue);

                if (Dose > 0)
                {
                    HshIn.Add("@decDose", Dose);
                }
                if (UnitId > 0)
                {
                    HshIn.Add("@intUnitId", UnitId);
                }
                if (FormulationId > 0)
                {
                    HshIn.Add("@intFormulationId", FormulationId);
                }
                if (RouteId > 0)
                {
                    HshIn.Add("@intRouteId", RouteId);
                }
                if (FrequencyId > 0)
                {
                    HshIn.Add("@intFrequencyId", FrequencyId);
                }
                if (Duration > 0)
                {
                    HshIn.Add("@intDuration", Duration);
                }
                if (DurationType.Trim().Length > 0)
                {
                    HshIn.Add("@chrDurationType", DurationType);
                }
                if (FoodRelationshipId > 0)
                {
                    HshIn.Add("@intFoodRelationshipId", FoodRelationshipId);
                }
                if (!string.IsNullOrEmpty(Instructions))
                {
                    HshIn.Add("@Instructions", Instructions);
                }
                if (GenericId > 0)
                {
                    HshIn.Add("@intGenericId", GenericId);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveFavDrugs", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }


        public string SaveFavouriteDrugs(int DoctorId, int ItemId, string FormularyType, int EncodedBy,
                                        double Dose, int UnitId, int StrengthId, int FormulationId, int RouteId,
                                        int FrequencyId, int Duration, string DurationType, int FoodRelationshipId,
                                        string StrengthValue, string Instructions, int GenericId,
                                        string VariableDoseString)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@Drug_Syn_Id", ItemId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chrFormularyType", FormularyType);

                HshIn.Add("@decDose", Dose);
                HshIn.Add("@intUnitId", UnitId);
                HshIn.Add("@intStrengthId", StrengthId);
                HshIn.Add("@intFormulationId", FormulationId);
                HshIn.Add("@intRouteId", RouteId);
                HshIn.Add("@intFrequencyId", FrequencyId);
                HshIn.Add("@intDuration", Duration);
                HshIn.Add("@chrDurationType", DurationType);
                HshIn.Add("@intFoodRelationshipId", FoodRelationshipId);
                HshIn.Add("@chvStrengthValue", StrengthValue);
                HshIn.Add("@chvInstructions", Instructions);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@XmlVariableDose", VariableDoseString);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveFavDrugs", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getFavoriteDrugWithStock(int HospId, int FacilityId, int EncodedBy, int ItemId,
                                        int GenericId, int DoctorId, string FormularyType, string ItemName)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chrFormularyType", FormularyType);
                HshIn.Add("@chvItemName", ItemName);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFavoriteDrugWithStock", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
            }
            return ds;

        }

        public DataSet getFavoriteDrugWithStockV3(int HospId, int FacilityId, int EncodedBy, int ItemId,
                                       int GenericId, int DoctorId, string FormularyType, string ItemName)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chrFormularyType", FormularyType);
                HshIn.Add("@chvItemName", ItemName);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFavoriteDrugWithStockV3", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
            }
            return ds;

        }
        public string DeleteFavoriteDrugs(int DoctorId, int ItemId, string FormularyType, int EncodedBy)
        {
            return DeleteFavoriteDrugs(DoctorId, ItemId, FormularyType, EncodedBy, 0);
        }

        public string DeleteFavoriteDrugs(int DoctorId, int ItemId, string FormularyType, int EncodedBy, int GenericId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@Drug_Syn_Id", ItemId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chrFormularyType", FormularyType);
                HshIn.Add("@intGenericId", GenericId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteFavDrugs", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public string SaveMonographMUD(int HospId, int EncounterId, int DoctorId, int RegistrationId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intEncounterID", EncounterId);
                HshIn.Add("@intDoctorID", DoctorId);
                HshIn.Add("@intRegistrationID", RegistrationId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRMUDLogEducationAndMonograph", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public string EMRSaveUpdateChemoTherapySchedule(int inyHospitalLocationId, int RegistrationId, int EncounterId, int DoctorCode, int Cycal, int Weight, int Height, string BSA, string ChemioTherayProtocol, string Diagnosis, string Status, int EncodedBy, string XMLTherapyDetails)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@EncounterId", EncounterId);
                HshIn.Add("@Cycal", Cycal);
                HshIn.Add("@Weight", Weight);
                HshIn.Add("@Height", Height);
                HshIn.Add("@BSA", BSA);
                HshIn.Add("@ChemoTherayProtocol", ChemioTherayProtocol);
                HshIn.Add("@Diagnosis", Diagnosis);
                HshIn.Add("@Status", Status);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@XMLTherapyDetails", XMLTherapyDetails);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveUpdateChemoTherapyShedule", HshIn, HshOut);

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
            return HshOut["@chvErrorStatus"].ToString();
        }


        public DataSet GetEMRChemoTherapyCycleDetails(DateTime FromDate, DateTime ToDate, int Cycle, int RegistrationId, int EncounterId, string chvName, string MobileNo, Int64 chvRegistrationNo)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();

            string fDate = FromDate.ToString("yyyy/MM/dd");
            string tDate = ToDate.ToString("yyyy/MM/dd");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@fdate", fDate);
                HshIn.Add("@tdate", tDate);
                HshIn.Add("@Cycle", Cycle);
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@EncounterId", EncounterId);
                HshIn.Add("@chvName", chvName);
                HshIn.Add("@MobileNo", MobileNo);
                HshIn.Add("@chvRegistrationNo", chvRegistrationNo);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetChemoTherapyCycleDetails", HshIn);
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


        public DataSet getDiagnosis(int HospId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
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
        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType,
          int StoreId, int AdvisingDoctorId, string xmlItemDetail, int EncodedBy, string xmlPlaneofCare
          , string sStrXML, string sXMLPatientAlert, string sRemark,
          int iEncodedBy, int iDoctorId, int iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
          int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, bool bAllergyReviewed,
          int IsERorEMRServices, int RequestId, string xmlTemplateDetails, int EntrySite)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@intEncodedBy", EncodedBy);
            hshIn.Add("@intStoreId", StoreId);
            hshIn.Add("@xmlTemplateDetails", xmlPlaneofCare);

            //below service parameter



            hshIn.Add("dtsOrderDate", dtOrderDate);
            hshIn.Add("inyOrderType", sOrderType);

            hshIn.Add("xmlServices", sStrXML);
            hshIn.Add("xmlPatientAlerts", sXMLPatientAlert);

            hshIn.Add("chvRemarks", sRemark);
            hshIn.Add("intDoctorId", iDoctorId);
            hshIn.Add("intCompanyId", iCompanyId);
            hshIn.Add("cPayerType", cPayerType);
            hshIn.Add("iInsuranceId", iInsuranceId);
            hshIn.Add("iCardId", iCardId);
            hshIn.Add("cPatientOPIP", sPatientOPIP);
            hshIn.Add("chrChargeCalculationRequired", sChargeCalculationRequired);
            hshIn.Add("bitAllergyReviewed", bAllergyReviewed);
            hshIn.Add("iIsERorEMRServices", IsERorEMRServices);
            hshIn.Add("intRequestId", RequestId);
            hshIn.Add("xmlTemplateDetails1", xmlTemplateDetails);
            hshIn.Add("intEntrySite", EntrySite);
            //    hshIn.Add("intOrderId", SqlDbType.Int);
            hshOut.Add("intOrderNo", SqlDbType.VarChar);
            hshOut.Add("@intOrderId", SqlDbType.Int);
            hshOut.Add("@intNEncounterID", SqlDbType.Int);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOut.Add("@chvErrorStatus1", SqlDbType.VarChar);



            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientTreatmentPlan_New", hshIn, hshOut);
            // return hshOut["@chvErrorStatus"].ToString();
            return hshOut;
        }


        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId,
                               int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                               int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy, string xmlFrequencyTime, bool IsConsumable)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@bitIsPregnant", IsPregnant);
            hshIn.Add("@bitIsBreastFeeding", IsBreastFeeding);
            hshIn.Add("@xmlItems", xmlItems);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@xmlPatientAlerts", xmlPatientAlerts);
            hshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            hshIn.Add("@intEncodedBy", EncodedBy);
            hshIn.Add("@intStoreId", StoreId);
            hshIn.Add("@bitIsConsumable", IsConsumable);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineIP", hshIn, hshOut);
            // return hshOut["@chvErrorStatus"].ToString();
            return hshOut;
        }

        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId,
                              int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId,
                               string xmlItemDetail, int EncodedBy, string PrescriptionDetailText)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@intEncodedBy", EncodedBy);
            hshIn.Add("@intStoreId", StoreId);
            hshIn.Add("@PrescriptionDetailText", PrescriptionDetailText);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOP", hshIn, hshOut);
            // return hshOut["@chvErrorStatus"].ToString();
            return hshOut;
        }

        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, string xmlItemDetail, int EncodedBy, string xmlPlaneofCareInstructions, string PatientType, string xmlProblemDetails, int DoctorId, string strXMLDiagnosisDetails)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@intStoreId", StoreId);
            hshIn.Add("@intEncodedBy", EncodedBy);

            hshIn.Add("@xmlTemplateDetails", xmlPlaneofCareInstructions);//Plan of care and instructions
            hshIn.Add("@xmlProblemDetails", xmlProblemDetails);
            hshIn.Add("@intDoctorId", DoctorId);

            if (strXMLDiagnosisDetails != string.Empty)
            {
                hshIn.Add("@XMLDiagnosisDetails", strXMLDiagnosisDetails);
            }
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            if (Convert.ToString(PatientType) == "I")
            {
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientTreatmentPlan", hshIn, hshOut);
            }
            else
            {
                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientTreatmentPlan", hshIn, hshOut);
            }

            // return hshOut["@chvErrorStatus"].ToString();
            return hshOut;
        }

        public Hashtable SaveEMRMedicineOP(int IndentId, int HospId, int FacilityId, int RegistrationId,
                              int EncounterId, int IndentType, int AdvisingDoctorId, int IsPregnant,
                              int IsBreastFeeding, int IsNoCurrentMedications, string Remarks,
                              string xmlItems, string xmlPatientAlerts, int EncodedBy)
        {
            return SaveEMRMedicineOP(HospId, FacilityId, RegistrationId,
                                EncounterId, IndentType, 0, AdvisingDoctorId, IsPregnant,
                                IsBreastFeeding, xmlItems, string.Empty, xmlPatientAlerts, EncodedBy, string.Empty, false, string.Empty);
        }

        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId,
                                int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                                int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy,
                                string xmlFrequencyTime, bool IsConsumable, string xmlUnApprovedPrescriptionIds)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.ParseData bc = new BaseC.ParseData();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentType", IndentType);
                HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
                HshIn.Add("@bitIsPregnant", IsPregnant);
                HshIn.Add("@bitIsBreastFeeding", IsBreastFeeding);
                HshIn.Add("@xmlItems", xmlItems);
                HshIn.Add("@xmlItemDetail", xmlItemDetail);
                HshIn.Add("@xmlPatientAlerts", xmlPatientAlerts);
                HshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@bitIsConsumable", IsConsumable);
                if (xmlUnApprovedPrescriptionIds != string.Empty)
                {
                    HshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
                }
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
                HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOP", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
                bc = null;
            }
            // return HshOut["@chvErrorStatus"].ToString();
            return HshOut;
        }

        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId,
                                 int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                                 int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy,
                                 string xmlFrequencyTime, bool IsConsumable, string xmlUnApprovedPrescriptionIds, int DrugAdminIn)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.ParseData bc = new BaseC.ParseData();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentType", IndentType);
                HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
                HshIn.Add("@bitIsPregnant", IsPregnant);
                HshIn.Add("@bitIsBreastFeeding", IsBreastFeeding);
                HshIn.Add("@xmlItems", xmlItems);
                HshIn.Add("@xmlItemDetail", xmlItemDetail);
                HshIn.Add("@xmlPatientAlerts", xmlPatientAlerts);
                HshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@bitIsConsumable", IsConsumable);
                HshIn.Add("@intDrugAdminIn", DrugAdminIn);

                if (xmlUnApprovedPrescriptionIds != string.Empty)
                {
                    HshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
                HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOP", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
                bc = null;
            }
            // return HshOut["@chvErrorStatus"].ToString();
            return HshOut;
        }


        public DataSet getPreviousMedicinesOP(int HospId, int FacilityId, int EncounterId, int IndentId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesOP", HshIn);
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
        public DataSet getOPMedicines(int HospId, int FacilityId, int EncounterId, int RegistrationId, int IndentId, int ItemId, string sPreviousMedication)
        {
            return getOPMedicines(HospId, FacilityId, EncounterId, RegistrationId, IndentId, ItemId, sPreviousMedication, string.Empty, string.Empty, string.Empty);

        }

        public DataSet getOPMedicines(int HospId, int FacilityId, int EncounterId, int RegistrationId, int IndentId, int ItemId, string sPreviousMedication,
                                       string ItemName, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvPreviousMedication", sPreviousMedication);
                if (FromDate.Trim() != string.Empty && ToDate.Trim() != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (ItemName.Trim() != string.Empty)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPMedicines", HshIn);
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

        public DataSet getOPMedicinesNew(int HospId, int FacilityId, int EncounterId, int RegistrationId, int IndentId, int ItemId, string sPreviousMedication,
                                      string ItemName, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvPreviousMedication", sPreviousMedication);
                if (FromDate.Trim() != string.Empty && ToDate.Trim() != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (ItemName.Trim() != string.Empty)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPMedicinesNew", HshIn);
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

        public string GetPrescriptionDetailString(DataTable dt)
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
                        if (dtString.Rows[i]["ReferanceItemName"].ToString().Trim() == string.Empty)
                        {
                            sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? dtString.Rows[i]["DoseTypeName"].ToString() + " - " : string.Empty);
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]) + " " : "");
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
                                    sb.Append(numberToSplit.ToString("0.##") + " ");
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : ""); 
                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : string.Empty);
                            if (Convert.ToDouble(dtString.Rows[i]["Duration"]) != 0.0)
                            {
                                sb.Append(" for " + dtString.Rows[i]["DurationText"].ToString() + " ");
                            }
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : string.Empty);
                            sb.Append(dtString.Rows[i]["Instructions"].ToString() != string.Empty ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : string.Empty);
                            sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
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
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]) + " " : "");
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
                                    sb.Append(numberToSplit.ToString("0.##") + " ");
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : ""); 

                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : string.Empty);
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : string.Empty);
                            sb.Append(dtString.Rows[i]["Instructions"].ToString() != string.Empty ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : string.Empty);
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                        }
                    }
                    else
                    {
                        if (dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty)
                        {
                            sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? dtString.Rows[i]["DoseTypeName"].ToString() + " - " : string.Empty);
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]) + " " : "");
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
                                    sb.Append(numberToSplit.ToString("0.##") + " ");
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : ""); 
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                        }
                        else
                        {
                            sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? dtString.Rows[i]["DoseTypeName"].ToString() + " - " : string.Empty);
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]) + " " : "");
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
                                    sb.Append(numberToSplit.ToString("0.##") + " ");
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : ""); 
                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : string.Empty);

                            if (dtString.Rows[i]["Duration"].ToString().Trim() != "" && dtString.Rows[i]["Duration"].ToString().Trim() != "0")
                            {
                                sb.Append(" for " + dtString.Rows[i]["DurationText"].ToString() + " ");
                            }
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : string.Empty);
                            sb.Append(dtString.Rows[i]["Instructions"].ToString() != string.Empty ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : string.Empty);
                            sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
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
            return sb.ToString().Trim();
        }
        public string GetPrescriptionDetailStringNew(DataTable dt, string EMRPrescriptionDoseShowInFractionalValue)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dtString = new DataTable();
            dtString = dt;
            double numberToSplit = 0;
            double decimalresult = 0;
            string DoseInFraction = string.Empty;
            try
            {
                for (int i = 0; i < dtString.Rows.Count; i++)
                {
                    try
                    {
                        DoseInFraction = string.Empty;

                        if (EMRPrescriptionDoseShowInFractionalValue == "Y")
                        {
                            if (dtString.Columns.Contains("DoseInFraction"))
                            {
                                DoseInFraction = Convert.ToString(dtString.Rows[i]["DoseInFraction"]).Trim();
                            }
                            else
                            {
                                if (dtString.Rows[i]["GenericId"] != null && dtString.Rows[i]["Dose"] != null)
                                {
                                    if (Convert.ToInt32(dtString.Rows[i]["ItemId"]) > 0)
                                    {
                                        DoseInFraction = convertNumberToFraction(Convert.ToString(dtString.Rows[i]["Dose"]));
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                    numberToSplit = Convert.ToDouble(dtString.Rows[i]["Dose"]);
                    decimalresult = (int)numberToSplit - numberToSplit;

                    if (Convert.ToBoolean(dtString.Rows[i]["IsInfusion"]))
                    {
                        if (dtString.Rows[i]["ReferanceItemName"].ToString().Trim().Equals(string.Empty))
                        {
                            sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + (dtString.Rows[i]["DoseTypeName"].ToString().Trim().Equals("PRN") ? "PRN(If Required)" : dtString.Rows[i]["DoseTypeName"].ToString().Trim()) + " - " : " ");

                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : " ");

                            if (Convert.ToString(DoseInFraction).Length > 1)
                            {
                                sb.Append(DoseInFraction + " ");
                            }
                            else
                            {
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
                                        sb.Append(numberToSplit.ToString("0.##") + " ");
                                    }
                                }
                            }

                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : "");

                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : " ");


                            if (Convert.ToDouble(dtString.Rows[i]["Duration"]) != 0.0)
                            {
                                sb.Append((dtString.Rows[i]["Type"].ToString() == "C" ? " " : " for ") + dtString.Rows[i]["DurationText"].ToString() + " ");
                            }


                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : string.Empty);
                            //  sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");
                            // sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != string.Empty ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : string.Empty);
                            //  sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                        }
                        else
                        {
                            sb.Append(dtString.Rows[i]["ItemName"].ToString());
                            // sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != "" ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : "");
                            sb.Append(" to go over ");
                            if (Convert.ToDouble(dtString.Rows[i]["Duration"]) != 0.0)
                            {
                                sb.Append(dtString.Rows[i]["DurationText"].ToString() + ", ");
                            }
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : " ");

                            if (Convert.ToString(DoseInFraction).Length > 1)
                            {
                                sb.Append(DoseInFraction + " ");
                            }
                            else
                            {
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
                                        sb.Append(numberToSplit.ToString("0.##") + " ");
                                    }
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : "");

                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : " ");
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : " ");
                            //sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");
                            //sb.Append(dtString.Rows[i]["RouteName"].ToString() != "" ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : "");
                        }
                    }
                    else
                    {
                        if (dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty)
                        {
                            sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + (dtString.Rows[i]["DoseTypeName"].ToString().Trim().Equals("PRN") ? "PRN(If Required)" : dtString.Rows[i]["DoseTypeName"].ToString().Trim()) + " - " : " ");
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : " ");
                            if (Convert.ToString(DoseInFraction).Length > 1)
                            {
                                sb.Append(DoseInFraction + " ");
                            }
                            else
                            {
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
                                        sb.Append(numberToSplit.ToString("0.##") + " ");
                                    }
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != string.Empty ? dtString.Rows[i]["FormulationName"].ToString() + " " : "");
                            if (dtString.Rows[i]["DoseTypeName"].ToString().ToUpper().Contains("URGENT"))
                            {
                                sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : string.Empty);
                            }
                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                        }
                        else
                        {
                            sb.Append(dtString.Rows[i]["DoseTypeName"].ToString() != string.Empty ? " " + (dtString.Rows[i]["DoseTypeName"].ToString().Trim().Equals("PRN") ? "PRN(If Required)" : dtString.Rows[i]["DoseTypeName"].ToString().Trim()) + " - " : " ");
                            //sb.Append(Convert.ToDouble(dtString.Rows[i]["Dose"]) != 0.0 ? Convert.ToDecimal(dtString.Rows[i]["Dose"]).ToString() + " " : " ");
                            if (Convert.ToString(DoseInFraction).Length > 1)
                            {
                                sb.Append(DoseInFraction + " ");
                            }
                            else
                            {
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
                                        sb.Append(numberToSplit.ToString("0.##") + " ");
                                    }
                                }
                            }
                            sb.Append(dtString.Rows[i]["UnitName"].ToString() != string.Empty ? dtString.Rows[i]["UnitName"].ToString() + " " : string.Empty);
                            //sb.Append(dtString.Rows[i]["FormulationName"].ToString() != "" ? dtString.Rows[i]["FormulationName"].ToString() + " " : "");

                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " -  " + dtString.Rows[i]["FrequencyName"].ToString() + " - " : string.Empty);

                            if (dtString.Rows[i]["Duration"].ToString().Trim() != "" && dtString.Rows[i]["Duration"].ToString().Trim() != "0")
                            {
                                sb.Append((dtString.Rows[i]["Type"].ToString() == "C" ? " " : " for ") + dtString.Rows[i]["DurationText"].ToString() + " ");
                            }

                            sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? ", " + dtString.Rows[i]["FoodRelationship"].ToString() : " ");
                            //  sb.Append(dtString.Rows[i]["Instructions"].ToString() != "" ? " (" + dtString.Rows[i]["Instructions"].ToString() + ") " : "");
                            //  sb.Append(dtString.Rows[i]["ReferanceItemName"].ToString() != "" ? " add to " + dtString.Rows[i]["ReferanceItemName"].ToString() : "");
                            //    sb.Append(dtString.Rows[i]["RouteName"].ToString() != "" ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : "");
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
            return sb.ToString().Trim();
        }

        public string convertNumberToFraction(string NumValue)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            string strFractionValue = string.Empty;
            try
            {
                if (NumValue != string.Empty)
                {
                    if (Convert.ToDouble(NumValue) > 0)
                    {
                        HshIn.Add("@dblNumValue", NumValue);

                        qry.Append("SELECT dbo.udfConvertNumberToFraction(ISNULL(@dblNumValue,0)) as FractionValue ");

                        ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            strFractionValue = Convert.ToString(ds.Tables[0].Rows[0]["FractionValue"]);
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
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return strFractionValue;
        }

        public int DeletePrescription(int Id)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            try
            {
                HshIn.Add("@intId", Id);
                i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE OPPrescriptionDetail set Active=0 Where Id=@intId", HshIn);
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
            return i;

        }
        public DataSet GetUnitMaster(int HospId, int FacilityId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                qry.Append(" SELECT DISTINCT um.ID, um.Description AS UnitName FROM EMRMedicationUnitMaster um WITH (NOLOCK) ");
                qry.Append(" LEFT JOIN EMRMedicationPhrItemCategoryDetails_EMRMedicationUnitMaster mt WITH (NOLOCK) ON um.ID=mt.EMRMedicationUnitMasterID ");
                qry.Append(" LEFT JOIN PhrItemMaster im WITH (NOLOCK) ON im.ItemSubCategoryId=mt.ItemSubCategoryId AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ");
                qry.Append(" WHERE um.Active=1 ORDER BY um.Description ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }
        public DataSet GetUnitMaster(int HospId, int FacilityId, int IsValume)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intIsValume", IsValume);

                qry.Append(" SELECT DISTINCT um.ID, um.Description AS UnitName FROM EMRMedicationUnitMaster um WITH (NOLOCK) ");
                qry.Append(" LEFT JOIN EMRMedicationPhrItemCategoryDetails_EMRMedicationUnitMaster mt WITH (NOLOCK) ON um.ID=mt.EMRMedicationUnitMasterID ");
                qry.Append(" LEFT JOIN PhrItemMaster im WITH (NOLOCK) ON im.ItemSubCategoryId=mt.ItemSubCategoryId AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ");
                qry.Append(" WHERE um.Active=1 And um.IsVolumeUnit = @intIsValume ORDER BY um.ID ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }


        public DataSet getCopyPreviousMedicinesOP(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCopyPreviousMedicinesOP", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }


        public DataSet getCopyPreviousMedicinesOP(int HospId, int FacilityId, int RegistrationId, int EncounterId, int UserId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", UserId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCopyPreviousMedicinesOP", HshIn);
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

        public DataSet getCopyPreviousMedicinesIP(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCopyPreviousMedicinesIP", HshIn);
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


        public DataSet getMedicinesOPList(int HospId, int FacilityId, int EncounterId, int IndentId)
        {
            return getMedicinesOPList(HospId, FacilityId, EncounterId, IndentId, 0, string.Empty);
        }

        public DataSet getMedicinesOPList(int HospId, int FacilityId, int EncounterId, int IndentId,
                                    int RegistrationId, string IndentNo)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@chvIndentNo", IndentNo);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMedicinesOPList", HshIn);
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

        public DataSet getMedicationDispenseOPList(int HospId, int FacilityId, int DoctorId,
                                    string FromDate, string ToDate, string IndentNo, string RegistrationNo,
                                    string EncounterNo, string PatientName, int DispenseStatus, int iUserId, int StoreId, bool bER)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@intDispenseStatus", DispenseStatus);
                HshIn.Add("@iUserId", iUserId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@bitER", bER);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMedicationDispenseOPList", HshIn);
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

        public DataSet getMedicationPreAuthorizationOPList(int HospId, int FacilityId, int DoctorId,
                                    string FromDate, string ToDate, string IndentNo, string RegistrationNo,
                                    string EncounterNo, string PatientName, int AuthorizationStatus)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@intAuthorizationStatus", AuthorizationStatus);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMedicationPreAuthorizationOPList", HshIn);
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

        public DataSet getPreviousMedicationsOPList(int HospId, int FacilityId, int EncounterId, int DoctorId,
                                                    string FromDate, string ToDate, string IndentNo)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chvIndentNo", IndentNo);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicationsOPList", HshIn);
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

        public void cancelPrescription(int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intIndentId", IndentId);

                qry.Append("UPDATE OPPrescriptionMain SET Active = 0 WHERE IndentId = @intIndentId");

                objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
        }

        public string SaveEMRPrescriptionRemarks(int RemarkId, int HospId, int FacilityId, string Remarks,
                                    int Active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRemarkId", RemarkId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRPrescriptionRemarks", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getEMRPrescriptionRemarks(int RemarkId, int HospitalLocationId, int FacilityId, int Active, int DoctorID)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRemarkId", RemarkId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intDoctorID", DoctorID);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRPrescriptionRemarks", HshIn);
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


        public string SaveEMRPatientAlertMaster(int AlertId, int HospId, string AlertDescription,
                                            int Active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAlertId", AlertId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@chvAlertDescription", AlertDescription);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRPatientAlertMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getEMRPatientAlertMaster(int AlertId, int HospitalLocationId, int Active)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAlertId", AlertId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@bitActive", Active);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRPatientAlertMaster", HshIn);
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

        public DataSet getEMRPatientAlertDetails(int RegistrationId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRegistrationId", RegistrationId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRPatientAlertDetails", HshIn);
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

        public string stopPrescription(string DetailsIds, int CancelReasonId, int EncodedBy, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intCancelReasonId", CancelReasonId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                if (OPIP == "I")
                {
                    qry.Append("UPDATE ICMIndentDetails SET Active = 0, CancelReasonId = @intCancelReasonId,");
                    qry.Append(" LastchangeBy = @intEncodedBy, LastChangedate = GETUTCDATE() ");
                    qry.Append(" WHERE Id in (" + DetailsIds + ")");
                }
                else
                {
                    qry.Append("UPDATE OPPrescriptionDetail SET Active = 0, CancelReasonId = @intCancelReasonId,");
                    qry.Append(" LastchangeBy = @intEncodedBy, LastChangedate = GETUTCDATE() ");
                    qry.Append(" WHERE Id in (" + DetailsIds + ")");
                }
                objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }

            return "Succeeded !";
        }

        public string setMedicationDispense(string DetailsIds, int DispenseBy, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intDispenceBy", DispenseBy);

                qry.Append("UPDATE OPPrescriptionDetail SET MedicationDispense = 1, DispenseBy=@intDispenceBy,");
                qry.Append(" LastchangeBy = @intEncodedBy, LastChangedate = GETUTCDATE() ");
                qry.Append(" WHERE Id in (" + DetailsIds + ")");

                objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return "Succeeded !";
        }

        public string MedicationDispense(int HospId, int IndentId, int DispenseBy, string xmlDetails, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intDispenceBy", DispenseBy);
                HshIn.Add("@xmlDetails", xmlDetails);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRMedicationDispense", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }


        public string setMedicationPreAuthorization(int HospId, int IndentId, string xmlDetails, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@xmlDetails", xmlDetails);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRMedicationPreAuthorization", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getAllergyTypeMaster(int HospId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intHospId", HospId);
                qry.Append("SELECT TypeID AS GenericId, TypeName AS GenericName FROM AllergyType WITH (NOLOCK) WHERE HospitalLocationId = @intHospId");
                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;

        }

        public DataSet getAllergyMaster(int HospId, string ItemName, int AllergyTypeId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                if (ItemName.Trim().Length > 0)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }
                HshIn.Add("@intAllergyTypeId", AllergyTypeId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetAllergyMaster", HshIn);
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

        public string EMRMUDLogMedicationAllergy(int HospId, int RegistrationId, int EncounterId, int DoctorId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRMUDLogMedicationAllergy", HshIn, HshOut);
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
            return string.Empty;

        }

        public string SavePatientAllergy(int HospId, int FacilityId, int PageId, int RegistrationId,
                                    int EncounterId, string xmlDrugAllergyDetails, string xmlOtherAllergyDetails,
                                    int EncodedBy, int IsNKDA, int IsShowNote)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@xmlDrugAllergyDetails", xmlDrugAllergyDetails);
                HshIn.Add("@xmlOtherAllergyDetails", xmlOtherAllergyDetails);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitNKDA", IsNKDA);
                HshIn.Add("@IsShowNote", IsShowNote);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePatientAllergy", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public string deActivatePatientAllergy(int HospId, int FacilityId, int PageId, int Flag,
                            int RegistrationId, int EncounterId, int Id, string CancelRemarks, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@inyFlag", Flag);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intId", Id);
                HshIn.Add("@chrCancelRemarks", CancelRemarks);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@ERROR", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDeactivatePatientAllergy", HshIn, HshOut);
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
            return HshOut["@ERROR"].ToString();

        }

        public DataSet getUserFacility(int UserId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intUserId", UserId);

                qry.Append("SELECT suf.FacilityId, fm.Name AS FacilityName ");
                qry.Append(" FROM SecUserFacility suf WITH (NOLOCK) ");
                qry.Append(" INNER JOIN FacilityMaster fm WITH (NOLOCK) ON suf.FacilityId = fm.FacilityId AND fm.Active = 1 ");
                qry.Append(" WHERE suf.UserId = @intUserId AND suf.Active = 1");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public DataSet getTemplateEnteredList(int HospId, int RegistrationId, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateEnteredList", HshIn);
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
        public DataSet getTemplateEnteredList(int HospId, int FacilityId, int RegistrationId, int EncounterId, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@dtFromDate", sFromDate);
                HshIn.Add("@dtToDate", sToDate);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientTemplatesList", HshIn);
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
        public DataSet getEMRPrintCaseSheetDate(int HospId, int FacilityId, int RegistrationId, int EncounterId,
                                                string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                                string sTemplateType, bool bChronologicalOrder, int ReportId)
        {
            return getEMRPrintCaseSheetDate(HospId, FacilityId, RegistrationId, EncounterId,
                                                sFromDate, sToDate, sTemplateName, TemplateId,
                                                sTemplateType, bChronologicalOrder, ReportId, 0, false);

        }

        public DataSet getEMRPrintCaseSheetDate(int HospId, int FacilityId, int RegistrationId, int EncounterId,
                                              string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                              string sTemplateType, bool bChronologicalOrder, int ReportId, int SectionId, bool IncludePastHistory)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@chvTemplateName", sTemplateName);
                HshIn.Add("@chvTemplateType", sTemplateType);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@bitChronologicalOrder", bChronologicalOrder);
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intSectionidFilter", SectionId);


                if (IncludePastHistory)
                {
                    HshIn.Add("@bitIncludePastHistory", IncludePastHistory);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRPrintCaseSheet", HshIn);
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


        public DataSet getEMRPrintCaseSheetDate(int HospId, int FacilityId, int RegistrationId, int EncounterId,
                                             string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                             string sTemplateType, bool bChronologicalOrder, int ReportId, int SectionId, bool IncludePastHistory, int intEpisodeId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@chvTemplateName", sTemplateName);
                HshIn.Add("@chvTemplateType", sTemplateType);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@bitChronologicalOrder", bChronologicalOrder);
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intSectionidFilter", SectionId);
                HshIn.Add("@intEpisodeId", intEpisodeId);

                if (IncludePastHistory)
                {
                    HshIn.Add("@bitIncludePastHistory", IncludePastHistory);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRPrintCaseSheet", HshIn);
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

        public DataSet getEMRPrintTemplateMasterData(int HospId, int FacilityId, int RegistrationId, int EncounterId,
                                        string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                        string sTemplateType, bool bChronologicalOrder, int ReportId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@chvTemplateName", sTemplateName);
                HshIn.Add("@chvTemplateType", sTemplateType);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@bitChronologicalOrder", bChronologicalOrder);
                HshIn.Add("@intReportId", ReportId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRPrintTemplateMasterData", HshIn);
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

        public DataSet getEMRPatientProblemAndAllergyTemplateString(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);

                qry.Append(" SELECT ISNULL(r.FirstName,'') +  ISNULL(' ' + r.MiddleName,'') +  ISNULL(' ' +r.LastName,'') AS PatientName,dbo.AgeInYrsMonthDay (CONVERT(VARCHAR(10),DateofBirth,111), CONVERT(VARCHAR(10), Getdate(),111)) AS Age, dbo.GetGender(Gender) AS Gender,c.Race,NoAllergies ");
                qry.Append(" FROM Registration r WITH (NOLOCK) LEFT JOIN RaceMaster c WITH (NOLOCK) ON r.RaceId = c.RaceId WHERE r.Id = @intRegistrationId AND R.FacilityID=@intFacilityId AND R.HospitalLocationId=@inyHospitalLocationId ");

                qry.Append(" SELECT ISNULL(IsPregnant,0) AS IsPregnant, ISNULL(IsBreastFeeding,0) AS IsBreastFeeding FROM Encounter WITH (NOLOCK) ");
                qry.Append(" WHERE Id = @intEncounterId AND RegistrationId=@intRegistrationId AND FacilityID=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public string SaveEMRTemplateResultSet(int ResultSetId, string ResultSetName, int TemplateId, string xmlTemplateData, int DoctorId,
                                    int HospId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intResultSetId", ResultSetId);
                HshIn.Add("@chvResultSetName", ResultSetName);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@xmlTemplateDetails", xmlTemplateData);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateResultSet", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }


        public int DeleteEMRTemplateResultSet(int ResultSetID, int userId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            int i = 0;

            try
            {
                HshIn.Add("@ResultSetID", ResultSetID);
                HshIn.Add("@intEncodedBy", userId);

                qry.Append("UPDATE EMRTemplateResultSetMain SET Active=0,LastChangedBy=@intEncodedBy,LastChangedDate=GETUTCDATE() WHERE ResultSetId = @ResultSetID AND Active = 1 ");
                i = objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return i;
        }

        public DataSet getTemplateResultSet(int TemplateId, int DoctorId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intDoctorId", DoctorId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateResultSet", HshIn);
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

        public DataSet getTemplateDetails(int TemplateId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@intTemplateId", TemplateId);

                qry.Append("SELECT t1.Id PageId, TemplateName AS PageName, ");
                qry.Append(" 0 ParentId, 0 Hierarchy, 0 Sequence, ");
                qry.Append(" '/EMR/Templates/Default.aspx' PageUrl, 0 StaticPage, 3 AS ModuleId, 'T' +  Convert(varchar(10), t1.Id) as PageIdentification, ");
                qry.Append(" ISNULL(t1.TemplateName,'') AS DisplayName, 1 AS DisplayInNotes, TemplateSpaceNumber , ETT.TypeName as TemplateType, ");
                qry.Append(" 1 as ShowNote, 0 Lock, 'T' AS PageType ");
                qry.Append(" FROM EMRTemplate t1 WITH (NOLOCK) ");
                qry.Append(" LEFT OUTER JOIN EMRTemplateTypes ETT WITH (NOLOCK) ON t1.TemplateTypeID = ETT.ID ");
                qry.Append(" WHERE t1.Id = @intTemplateId AND t1.Active = 1");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public string CreateTemplateEpisode(int EpisodeId, int RegistrationId, int EncounterId, int TemplateId,
                                            string SaveMode, int EpisodeClose, int Active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intEpisodeId", EpisodeId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@chrSaveMode", SaveMode);
                HshIn.Add("@bitEpisodeClose", EpisodeClose);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRPatientTemplateCreateEpisode", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet UspEMRGetTemplateDetailsisApprovel(int RegistrationId, int intEncounterId, int intTemplateID)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@intTemplateID", intTemplateID);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsisApprovel", HshIn);

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
        public DataSet getTemplateEpisode(int RegistrationId, int TemplateId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intTemplateId", TemplateId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientTemplateEpisode", HshIn);
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

        public DataSet getPatientClosedEpisode(int iTemplateId, int iRegistrtionId, int iEncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intRegistrationId", iRegistrtionId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intTemplateId", iTemplateId);

                qry.Append(" SELECT EpisodeClosed FROM [dbo].[EMRPatientTemplateEpisodes] WITH (NOLOCK) WHERE RegistrationId=@intRegistrationId AND TemplateId=@intTemplateId AND EpisodeClosed=0 AND Active=1 ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public DataSet GetAccounttypeId(int iRegistrtionId, int iEncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intRegistrationId", iRegistrtionId);
                HshIn.Add("@intEncounterId", iEncounterId);

                qry.Append("select AccountTypeId FROM Encounter WITH (NOLOCK) WHERE Id=@intEncounterId AND RegistrationId=@intRegistrationId AND Active=1");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public DataSet GetMedicationSet()
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder qry = new StringBuilder();

            try
            {
                qry.Append("SELECT SetId, SetName FROM EMRDrugSetMain WITH (NOLOCK) WHERE Active=1");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                qry = null;
            }
            return ds;
        }

        public Hashtable SaveMedicationItemSet(string xmlMedicationSet, int iSetId, int iEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@xmlDrugDetails", xmlMedicationSet);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@intSetId", iSetId);
                HshOut.Add("@chvErrorOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMedicationItemSet", HshIn, HshOut);
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
            return HshOut;

        }

        public Hashtable SaveMedicationSet(int iSetId, string sSetName, int iHospitalLocationId, int iEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intSetId", iSetId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@chvSetName", sSetName);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("@chvErrorOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMedicationSet", HshIn, HshOut);
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
            return HshOut;

        }

        public int DeleteMedicationItem(int iSetId, int iGenericId, int iItemId, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            int i = 0;
            try
            {
                if (iSetId == 0 || (iGenericId == 0 && iItemId == 0))
                {
                    return 0;
                }

                HshIn.Add("@intSetId", iSetId);
                HshIn.Add("@intGenericId", iGenericId);
                HshIn.Add("@intItemId", iItemId);
                HshIn.Add("@intEncodedBy", iEncodedBy);

                qry.Append("UPDATE EMRDrugSetDetails SET Active=0,LastChangedBy=@intEncodedBy,LastChangedDate=GETUTCDATE()");
                qry.Append(" WHERE SetId=@intSetId ");

                if (iItemId > 0)
                {
                    qry.Append(" AND ItemId=@intItemId");
                }
                else if (iGenericId > 0)
                {
                    qry.Append(" AND GenericId=@intGenericId");
                }

                i = objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return i;
        }

        public int DeleteMedicationSet(int iSetId, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            int i = 0;
            try
            {
                HshIn.Add("@intSetId", iSetId);
                HshIn.Add("@intEncodedBy", iEncodedBy);

                qry.Append("UPDATE EMRDrugSetMain SET Active=0,LastChangedBy=@intEncodedBy,LastChangedDate=GETUTCDATE() WHERE SetID=@intSetId");
                i = objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return i;
        }
        public Hashtable SaveUpdatePatientAlertMaster(int iAlertId, int iHospitalLocationId, string sAlertName,
            string sGender, int iSequenceNo, bool bActive, int iEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAlertId", iAlertId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@chvAlertName", sAlertName);
                HshIn.Add("@chvGender", sGender);
                HshIn.Add("@intSequenceNo", iSequenceNo);
                HshIn.Add("@bitActive", bActive);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdatePatientAlertMaster", HshIn, HshOut);
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
            return HshOut;

        }

        public DataSet getPrescriptionOPRequest(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                            string RegistrationNo, string EncounterNo, string BedNo, string PatientName,
                            DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy, int OrderTypeId, string OrderStatusType)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvFromDate", fDate);
                HshIn.Add("@chvToDate", tDate);
                HshIn.Add("@inyOrderTypeId", OrderTypeId);
                HshIn.Add("@chrPendingStatus", OrderStatusType);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPrescriptionOPRequest", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;

        }

        public DataSet getPrescriptionOPRequestItemDetails(int IndentId, int HospId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intIndentID", IndentId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPrescriptionOPRequestItemDetails", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;

        }

        public DataSet getCIMSItemId(string ItemIds)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder qry = new StringBuilder();
            try
            {
                qry.Append("SELECT ItemId, CIMSItemId FROM PhrItemMasterCIMSTagging WITH (NOLOCK) WHERE ItemId IN(" + ItemIds + ")");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                qry = null;
            }
            return ds;
        }

        public DataSet GetPatientAlert(int iAlertId, int iHospitalLocationId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAlertId", iAlertId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientAlertMaster", HshIn);
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

        public DataSet getDrugAllergiesInterfaceCode(int HospId, int RegistrationId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);

                qry.Append("SELECT da.AllergyId, am.[Description] as AllergyName, da.Reaction, da.Remarks, ");
                qry.Append(" at.TypeName as AllergyType, am.InterfaceCode, am.CIMSTYPE ");
                qry.Append(" FROM PatientOtherAllergies da WITH (NOLOCK) ");
                qry.Append(" INNER JOIN AllergyMaster am WITH (NOLOCK) ON da.AllergyId = am.AllergyId ");
                qry.Append(" INNER JOIN AllergyType at WITH (NOLOCK) ON am.TypeID = at.TypeID and at.TypeName in ('CIMS','VIDAL') ");
                qry.Append(" WHERE da.RegistrationId = @intRegistrationId ");
                qry.Append(" AND da.Active = 1 ");
                qry.Append(" AND am.HospitalLocationId = @intHospId");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public enum enumCIMSorVIDALInterfaceFor
        {
            None = 0,
            InterfaceForEMRDrugOrder = 1,
            InterfaceForWordDrugRequisition = 2,
            InterfaceForOPSale = 3,
            InterfaceForIPIssue = 4,
            InterfaceForDrugAdministered = 5,
            InterfaceForMedicationDispense = 6
        }

        public DataSet getFacilityInterfaceDetails(int HospId, int FacilityId, enumCIMSorVIDALInterfaceFor interfaceFor)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                qry.Append(" SELECT IsCIMSInterfaceActive, CIMSDatabasePath, CIMSDatabasePassword,CIMSDatabaseName, IsVIDALInterfaceActive ");
                qry.Append(" FROM FacilityMaster WITH (NOLOCK) ");
                qry.Append(" WHERE Active = 1 ");
                switch (interfaceFor)
                {
                    case enumCIMSorVIDALInterfaceFor.None:
                        break;
                    case enumCIMSorVIDALInterfaceFor.InterfaceForEMRDrugOrder:
                        qry.Append(" AND InterfaceForEMRDrugOrder = 1 ");
                        break;
                    case enumCIMSorVIDALInterfaceFor.InterfaceForWordDrugRequisition:
                        qry.Append(" AND InterfaceForWordDrugRequisition = 1 ");
                        break;
                    case enumCIMSorVIDALInterfaceFor.InterfaceForOPSale:
                        qry.Append(" AND InterfaceForOPSale = 1 ");
                        break;
                    case enumCIMSorVIDALInterfaceFor.InterfaceForIPIssue:
                        qry.Append(" AND InterfaceForIPIssue = 1 ");
                        break;
                    case enumCIMSorVIDALInterfaceFor.InterfaceForDrugAdministered:
                        qry.Append(" AND InterfaceForDrugAdministered = 1 ");
                        break;
                    case enumCIMSorVIDALInterfaceFor.InterfaceForMedicationDispense:
                        qry.Append(" AND InterfaceForMedicationDispense = 1 ");
                        break;
                }
                qry.Append(" AND FacilityID = @intFacilityId ");
                qry.Append(" AND HospitalLocationId = @intHospId ");
                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;

        }
        public DataSet getFacilityInterfaceDetails(int HospId, int FacilityId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                string qry = "SELECT IsCIMSInterfaceActive, CIMSDatabasePath, CIMSDatabasePassword, CIMSDatabaseName, IsVIDALInterfaceActive," +
                            " InterfaceForEMRDrugOrder,InterfaceForWordDrugRequisition,InterfaceForOPSale,InterfaceForIPIssue FROM FacilityMaster fm WITH (NOLOCK) " +
                            " WHERE fm.FacilityID = @intFacilityId " +
                            " AND fm.Active = 1" +
                            " AND fm.HospitalLocationId = @intHospId ";
                //switch (interfaceFor)
                //{
                //    case enumCIMSorVIDALInterfaceFor.None:
                //        break;
                //    case enumCIMSorVIDALInterfaceFor.InterfaceForEMRDrugOrder:
                //        qry += " AND InterfaceForEMRDrugOrder = 1 ";
                //        break;
                //    case enumCIMSorVIDALInterfaceFor.InterfaceForWordDrugRequisition:
                //        qry += " AND InterfaceForWordDrugRequisition = 1 ";
                //        break;
                //    case enumCIMSorVIDALInterfaceFor.InterfaceForOPSale:
                //        qry += " AND InterfaceForOPSale = 1 ";
                //        break;
                //    case enumCIMSorVIDALInterfaceFor.InterfaceForIPIssue:
                //        qry += " AND InterfaceForIPIssue = 1 ";
                //        break;
                //}
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
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


        public DataSet getInterfaceItemDetails(int ItemId, int HospId, int FacilityId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospId", HospId);

                qry.Append("SELECT im.ItemId, cims.CIMSItemId, cims.CIMSTYPE, ISNULL(vidal.VIDALItemId,0) AS VIDALItemId ");
                qry.Append(" FROM PhrItemMaster im WITH (NOLOCK) ");
                qry.Append(" LEFT OUTER JOIN PhrItemMasterCIMSTagging cims WITH (NOLOCK) on im.ItemId = cims.ItemId ");
                qry.Append(" LEFT OUTER JOIN PhrItemMasterVIDALTagging vidal WITH (NOLOCK) on im.ItemId = vidal.ItemId ");
                qry.Append(" WHERE im.ItemId = @intItemId ");
                qry.Append(" AND im.Active = 1 ");
                qry.Append(" AND im.FacilityId = @intFacilityId ");
                qry.Append(" AND im.HospitalLocationId = @intHospId");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }


        public Hashtable TaggingStaticTemplateWithTemplateField(int iRegistrationId, int iEncounterId, int iSectionId,
            int iFieldId, int iStaticTemplate, int iEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intSectionId", iSectionId);
                HshIn.Add("@intFieldId", iFieldId);
                HshIn.Add("@intStaticTemplateId", iStaticTemplate);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspTaggingStaticTemplateWithTemplateField", HshIn, HshOut);
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
            return HshOut;

        }
        public Hashtable SaveDoctorProgressNote(int iProgressNoteId, int iHospitalLocationId, int iFacilityId, int iRegistrationId,
            int iEncounterId, int iDoctorId, string sProgressNote, int iEncodedBy, char progressNoteFor)
        {
            return SaveDoctorProgressNote(iProgressNoteId, iHospitalLocationId, iFacilityId, iRegistrationId,
            iEncounterId, iDoctorId, sProgressNote, iEncodedBy, 0, null, progressNoteFor);
        }
        public Hashtable SaveDoctorProgressNote(int iProgressNoteId, int iHospitalLocationId, int iFacilityId, int iRegistrationId,
            int iEncounterId, int iDoctorId, string sProgressNote, int iEncodedBy, int ProviderId, DateTime? ChangeDate, char progressNoteFor)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intProgressNoteId", iProgressNoteId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@chvProgressNote", sProgressNote);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@progressNoteFor", progressNoteFor);
                if (ProviderId > 0)
                {
                    HshIn.Add("@intProviderId", ProviderId);
                }
                if (ChangeDate != null)
                {
                    HshIn.Add("@chvChangeDate", ChangeDate);
                }
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDoctorProgressNote", HshIn, HshOut);
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
            return HshOut;

        }
        public DataSet GetDoctorProgressNote(int iProgressNoteId, int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iDoctorId, string sDateRange,
            string sFromDate, string sToDate, int EncounterId, string progressNoteFor)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intProgressNoteId", iProgressNoteId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@chvDateRange", sDateRange);
                HshIn.Add("@progressNoteFor", progressNoteFor);
                if (!sFromDate.Equals(string.Empty) && !sToDate.Equals(string.Empty))
                {
                    HshIn.Add("@chrFromDate", sFromDate);
                    HshIn.Add("@chrToDate", sToDate);
                }
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorProgressNote", HshIn);
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
        //Ritika(21-09-2022)Doctor Progress Note Adendum
        public DataSet GetDoctorProgressNoteAdendum(int ProgressNoteID)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intProgressNoteID", ProgressNoteID);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorProgressNoteAddendum", HshIn);
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
        //Ritika(21-09-2022)Doctor Progress Note Adendum
        public DataSet GetDoctorProgressNoteMergedAdendum(int ProgressNoteID)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intProgressNoteID", ProgressNoteID);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetProgressNoteMergedAddendum", HshIn);
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
        //Ritika(21-09-2022)Doctor Progress Note Adendum
        public Hashtable DeleteProgressNoteAddendum(int AddendumID, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAddendumID", AddendumID);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDeleteProgressNoteAddendum", HshIn);
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
            return HshOut;

        }
        //Ritika(21-09-2022)Doctor Progress Note Addendum
        public Hashtable SaveDoctorProgressNoteAddendum(int AddendumID, int iProgressNoteId, string Addendum, int iEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAddendumID", AddendumID);
                HshIn.Add("@intProgressNoteId", iProgressNoteId);
                HshIn.Add("@chvAddendum", Addendum);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDoctorProgressNoteAddendum", HshIn, HshOut);
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
            return HshOut;

        }
        public int inActiveDoctorProgressNote(int ProgressNoteId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            int i = 0;
            try
            {
                HshIn.Add("@intProgressNoteId", ProgressNoteId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                qry.Append("UPDATE DoctorProgressNote SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE Id = @intProgressNoteId");

                i = objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }

            return i;
        }

        public DataSet GetIndentType(int iHospitalLocationId, int iFacilityId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            //StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);

                //qry.Append("SELECT Id, IndentType, IndentCode,BeforeIndentDischarge,AfterIndentDischarge FROM IndentTypeMaster WITH (NOLOCK) WHERE Active=1 AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ");
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIndentType", HshIn);
                //ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
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

        //public DataSet GetPatientDischargeStatus(int iHospitalLocationId, int RegistrationId,string EncounterNo)
        //{
        //    DataSet ds = new DataSet();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable HshIn = new Hashtable();
        //    //StringBuilder qry = new StringBuilder();
        //    try
        //    {
        //        HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
        //        HshIn.Add("@intRegistrationId", RegistrationId);
        //        HshIn.Add("@chvEncounterNo", EncounterNo);

        //        //qry.Append("SELECT Id, IndentType, IndentCode,BeforeIndentDischarge,AfterIndentDischarge FROM IndentTypeMaster WITH (NOLOCK) WHERE Active=1 AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ");
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDischargePatient", HshIn);
        //        //ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    finally
        //    {
        //        objDl = null;
        //        HshIn = null;

        //    }
        //    return ds;
        //}


        public Hashtable StopCancelPreviousMedication(int iHospitalLocationId, int iFacilityId, int iIndentId, int iItemId, int UserId,
                                              int iRegistrationId, int iEncounterId, int bCancelStop, string sStopRemarks,
                                              string OPIP, int IndentDetailsId)
        {
            return StopCancelPreviousMedication(iHospitalLocationId, iFacilityId, iIndentId, iItemId, UserId,
                                                       iRegistrationId, iEncounterId, bCancelStop, sStopRemarks,
                                                       OPIP, IndentDetailsId, 0, 0);
        }

        public Hashtable StopCancelPreviousMedication(int iHospitalLocationId, int iFacilityId, int iIndentId, int iItemId, int UserId,
                                                      int iRegistrationId, int iEncounterId, int bCancelStop, string sStopRemarks,
                                                      string OPIP, int IndentDetailsId, int GenericId, int StopBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intIndentId", iIndentId);
                HshIn.Add("@intItemId", iItemId);
                HshIn.Add("@bitCancelStop", bCancelStop);
                HshIn.Add("@chvStopRemarks", sStopRemarks);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intIndentDetailsId", IndentDetailsId);
                HshIn.Add("@StopBy", StopBy);
                HshIn.Add("@intGenericId", GenericId);
                HshOut.Add("@chvOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspStopCancelPatientMedication", HshIn, HshOut);
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
            return HshOut;
        }


        public Hashtable CancelMultiplePrescriptions(int HospId, int FacilityId, int RegistrationId, string Remarks,
                                              string OPIP, string XMLData, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@XMLData", XMLData);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCancelMultiplePrescriptions", HshIn, HshOut);
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
            return HshOut;
        }

        public DataTable getEMRDietMaster(int HospitalLocationId, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDietMaster", HshIn);
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
            return ds.Tables[0];
        }


        public DataTable getEMRDietDetail(int HospitalLocationId, int FacilityId, int RegistrationID, int EncounterId)
        {
            DataTable dtEmrDietMaster = new DataTable();
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterID", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDietMain", HshIn);
                dtEmrDietMaster = ds.Tables[0];
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
            return dtEmrDietMaster;

        }



        public DataTable getEMRDietDetail(int HospitalLocationId, int FacilityId, int RegistrationID, int EncounterId, int RequestedId)
        {
            DataTable dtEmrDietMaster = new DataTable();
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterID", EncounterId);
                HshIn.Add("@intDietRequestID", RequestedId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDietDetail", HshIn);
                dtEmrDietMaster = ds.Tables[0];
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
            return dtEmrDietMaster;

        }


        public Hashtable UpdateEMRDiet(int HospitalLocationId, int FacilityId, int RegistrationId, int EncouterId,
                                    int DietRequestID, string xmlEMRDietDetails, int EncodedBy, string Remarks,
                                    string Diagnosis, string DietRequested, string DietrequestFoodhabit, int DietTypeCategoryId, int NPO,
                                    string xmlDiet, string xmlDietList, int ModeofFeeding, int International, bool IsUrgent)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationID", RegistrationId);
                HshIn.Add("@intEncounterID", EncouterId);
                HshIn.Add("@intDietRequestID", DietRequestID);
                HshIn.Add("@intDietTypeCategoryId", DietTypeCategoryId);
                HshIn.Add("@xmlDietDetail", xmlEMRDietDetails);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Remarks", Remarks);

                HshIn.Add("@Diagnosis", Diagnosis);
                HshIn.Add("@DietRequested", DietRequested);
                HshIn.Add("@DietrequestFoodhabit", DietrequestFoodhabit);
                HshIn.Add("@NPO", NPO);

                if (xmlDiet != string.Empty)
                    HshIn.Add("@xmlDiet", xmlDiet);
                if (xmlDietList != string.Empty)
                    HshIn.Add("@xmlDietList", xmlDietList);
                if (ModeofFeeding > 0)
                    HshIn.Add("@ModeofFeeding", ModeofFeeding);
                if (International > 0)
                    HshIn.Add("@International", International);

                if (IsUrgent)
                {
                    HshIn.Add("@isUrgent", IsUrgent);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvEMRDIETID", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRupdateDiet", HshIn, HshOut);
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
            return HshOut;
        }

        public Hashtable UpdateEMRDiet(int HospitalLocationId, int FacilityId, int RegistrationId, int EncouterId,
          int DietRequestID, string xmlEMRDietDetails, int EncodedBy, string Remarks, string Diagnosis, string DietRequested, string DietrequestFoodhabit, int DietTypeCategoryId, int NPO)
        {
            Hashtable hshInput = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@intHospitalLocationId", HospitalLocationId);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@intRegistrationID", RegistrationId);
            hshInput.Add("@intEncounterID", EncouterId);
            hshInput.Add("@intDietRequestID", DietRequestID);
            hshInput.Add("@intDietTypeCategoryId", DietTypeCategoryId);
            hshInput.Add("@xmlDietDetail", xmlEMRDietDetails);
            hshInput.Add("@intEncodedBy", EncodedBy);
            hshInput.Add("@Remarks", Remarks);
            hshInput.Add("@Diagnosis", Diagnosis);
            hshInput.Add("@DietRequested", DietRequested);
            hshInput.Add("@DietrequestFoodhabit", DietrequestFoodhabit);
            hshInput.Add("@NPO", NPO);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOutput.Add("@chvEMRDIETID", SqlDbType.VarChar);
            hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRupdateDiet", hshInput, hshOutput);
            return hshOutput;

        }


        public Hashtable UpdateEMRDiet(int HospitalLocationId, int FacilityId, int RegistrationId, int EncouterId,
           int DietRequestID, string xmlEMRDietDetails, int EncodedBy, string Remarks, string Diagnosis, string DietRequested, string DietrequestFoodhabit, int DietTypeCategoryId, int NPO
           , string xmlDiet, string xmlDietList, int ModeofFeeding, int International
           )
        {
            Hashtable hshInput = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@intHospitalLocationId", HospitalLocationId);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@intRegistrationID", RegistrationId);
            hshInput.Add("@intEncounterID", EncouterId);
            hshInput.Add("@intDietRequestID", DietRequestID);
            hshInput.Add("@intDietTypeCategoryId", DietTypeCategoryId);
            hshInput.Add("@xmlDietDetail", xmlEMRDietDetails);
            hshInput.Add("@intEncodedBy", EncodedBy);
            hshInput.Add("@Remarks", Remarks);
            hshInput.Add("@Diagnosis", Diagnosis);
            hshInput.Add("@DietRequested", DietRequested);
            hshInput.Add("@DietrequestFoodhabit", DietrequestFoodhabit);
            hshInput.Add("@NPO", NPO);
            hshInput.Add("@xmlDiet", xmlDiet);
            hshInput.Add("@xmlDietList", xmlDietList);
            hshInput.Add("@ModeofFeeding", ModeofFeeding);
            hshInput.Add("@International", International);


            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOutput.Add("@chvEMRDIETID", SqlDbType.VarChar);
            hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRupdateDiet", hshInput, hshOutput);
            return hshOutput;

        }


        public int DeleteEMRDiet(int HospitalLocationId, int FacilityId, int RegistrationId, int EncouterId,
        int DietRequestID, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationID", RegistrationId);
                HshIn.Add("@intEncounterID", EncouterId);
                HshIn.Add("@intDietRequestID", DietRequestID);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                i = objDl.ExecuteNonQuery(CommandType.Text, "update EMRDietRequestMain set Active=0,LastChangedDate=GETUTCDATE(),LastChangedBy=" + EncodedBy + " where  ID=" + DietRequestID + " and RegistrationID=" + RegistrationId + "  and EncounterID=" + EncouterId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return i;

        }

        public DataSet getEMRPatientVisits(int HospId, int FacilityId, int RegistrationId, string ViewType, int DoctorId, int TemplateId, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@chrViewType", ViewType);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intTemplateId", TemplateId);
                if (sFromDate != string.Empty)
                    HshIn.Add("@dtFromDate", sFromDate);
                if (sToDate != string.Empty)
                    HshIn.Add("@dtToDate", sToDate);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientVisits", HshIn);
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

        public DataSet getEMRPatientVisits(int HospId, int RegistrationId, string ViewType, int DoctorId, int TemplateId, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@chrViewType", ViewType);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intTemplateId", TemplateId);
                if (sFromDate != "")
                    HshIn.Add("@dtFromDate", sFromDate);
                if (sToDate != "")
                    HshIn.Add("@dtToDate", sToDate);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientVisits", HshIn);
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

        public DataTable GetDateWiseGroupingTemplate(int HospId, int FacilityId, int RegistrationId, int EncounterId, string sFromDate, string sToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRDateWiseGroupingTemplate", HshIn);
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
            return ds.Tables[0];

        }
        public DataTable GetMotherNewBornBabyRelation(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetMotherNewBornBabyRelation", HshIn);
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
            return ds.Tables[0];

        }

        public DataSet getEncounterDoctor(int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                /*
                qry.Append("SELECT emp.ID AS DoctorId, ISNULL(emp.FirstName, '') + ISNULL(' ' + emp.MiddleName, '') + ISNULL(' ' + emp.LastName, '')  AS DoctorName");
                qry.Append(" FROM Encounter enc WITH (NOLOCK) ");
                qry.Append(" INNER JOIN Employee emp WITH (NOLOCK) ON enc.DoctorId = emp.ID");
                qry.Append(" WHERE enc.Id = @intEncounterId");
                */
                qry.Append("SELECT emp.ID AS DoctorId,ISNULL(A.ConsultingDoctorId,enc.DoctorId) ConsultingDoctorId,ISNULL(emp.FirstName, '') + ISNULL(' ' + emp.MiddleName, '') + ISNULL(' ' + emp.LastName, '')  AS DoctorName");
                qry.Append(" FROM Encounter enc WITH (NOLOCK) ");
                qry.Append(" INNER JOIN Employee emp WITH (NOLOCK) ON enc.DoctorId = emp.ID");
                qry.Append(" LEFT JOIN Admission A WITH (NOLOCK) ON enc.Id = A.EncounterId");
                qry.Append(" WHERE enc.Id = @intEncounterId AND enc.Active = 1 ");
                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public DataSet getGenericRouteUnitOfSelectedDrug(int ItemId, int HospId, int FacilityId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@ItemID", ItemId);
                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrGenericRouteUnitOfSelectedDrug", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;

        }







        public string SavePatientNotesData(int HospId, int FacilityId, int FormId, int IsPullForward, int PageId, int RegistrationId, int EncounterId,
                                        int OrderId, int TemplateTypeId, string xmlTemplateDetails, string xmlTabularTemplateDetails, int IsShowNote,
                                        int SectionId, int RecordId, int EpisodeId, int EncodedBy, int OrderDetailId, int OrderRequestId, int SubDeptId)
        {
            return SavePatientNotesData(HospId, FacilityId, FormId, IsPullForward, PageId, RegistrationId, EncounterId,
                                        OrderId, TemplateTypeId, xmlTemplateDetails, xmlTabularTemplateDetails, IsShowNote,
                                        SectionId, RecordId, EpisodeId, EncodedBy, OrderDetailId, OrderRequestId, SubDeptId, 0, null, false, 0);
        }
        public string SavePatientNotesData(int HospId, int FacilityId, int FormId, int IsPullForward, int PageId, int RegistrationId, int EncounterId,
                                        int OrderId, int TemplateTypeId, string xmlTemplateDetails, string xmlTabularTemplateDetails, int IsShowNote,
                                        int SectionId, int RecordId, int EpisodeId, int EncodedBy, int OrderDetailId, int OrderRequestId, int SubDeptId, int ProviderId, DateTime? ChangeDate, bool IsApproved, int AdvisingDoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intFormId", FormId);
                HshIn.Add("@bitPullForward", IsPullForward);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                if (OrderId > 0)
                {
                    HshIn.Add("@intOrderId", OrderId);
                }
                HshIn.Add("@inyTemplateType", TemplateTypeId);
                HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
                HshIn.Add("@xmlTabularTemplateDetails", xmlTabularTemplateDetails);
                HshIn.Add("@IsShowNote", IsShowNote);
                HshIn.Add("@intSectionId", SectionId);
                HshIn.Add("@intRecordId", RecordId);
                HshIn.Add("@intEpisodeId", EpisodeId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                if (OrderDetailId > 0)
                {
                    HshIn.Add("@intOrderDetailId", OrderDetailId);
                }
                HshIn.Add("@intOrderRequestId", OrderRequestId);
                HshIn.Add("@intSubDeptId", SubDeptId);
                if (ProviderId > 0)
                {
                    HshIn.Add("@intProviderId", ProviderId);
                }
                if (ChangeDate != null)
                {
                    HshIn.Add("@chvChangeDate", ChangeDate);
                }
                HshIn.Add("@bitIsApprovalRequired", IsApproved);
                HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientTemplates", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public void updateFurtherAckServiceOrderDetail(int ServiceOrderDetailsId, int ServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@ServiceOrderDtlID", ServiceOrderDetailsId);
                HshIn.Add("@iServiceId", ServiceId);

                qry.Append("UPDATE ServiceOrderDetail SET FurtherAck = 1 WHERE id = @ServiceOrderDtlID AND ServiceId = @iServiceId AND Active = 1");

                objDl.ExecuteNonQuery(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
        }

        public bool IsSectionAddendum(int SectionId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            bool IsAddendum = false;
            try
            {
                HshIn.Add("@intSectionId", SectionId);

                qry.Append("SELECT SectionId FROM EMRTemplateSections WITH (NOLOCK) WHERE SectionId = @intSectionId AND IsAddendum = 1 AND Active = 1");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsAddendum = true;
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
                qry = null;
                ds.Dispose();
            }
            return IsAddendum;
        }

        public DataSet GetEMRDataForSingleScreen(int HospId, int RegistrationId, int FacilityId, int EncounterId,
                                               string sTemplateType, int TemplateId, string sTemplateName, string sFromDate, string sToDate,
                                               int PageSize, int PageNo, string CopyLastPrescritpion, int DoctorId)
        {
            DataSet ds = new DataSet();
            Hashtable hshInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hshInput.Add("@inyHospitalLocationId", HospId);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@intRegistrationId", RegistrationId);
            hshInput.Add("@intEncounterId", EncounterId);

            hshInput.Add("@chvTemplateType", sTemplateType);
            hshInput.Add("@intTemplateId", TemplateId);
            hshInput.Add("@chvTemplateName", sTemplateName);

            hshInput.Add("@chrFromDate", sFromDate);
            hshInput.Add("@chrToDate", sToDate);
            hshInput.Add("@inyPageSize", PageSize);
            hshInput.Add("@intPageNo", PageNo);
            hshInput.Add("@chvCopyLastPrescritpion", CopyLastPrescritpion);
            hshInput.Add("@intDoctorId", DoctorId);


            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetSingleScreenData", hshInput);
            return ds;
        }

        public DataSet GetEMREncounterStatus(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                string sQuery = " SELECT s.code FROM Encounter e WITH (NOLOCK) INNER JOIN StatusMaster s WITH (NOLOCK) ON e.EMRStatusId = s.statusid where e.id = @intEncounterId AND e.RegistrationId = @intRegistrationId AND e.active = 1 AND e.FacilityId = @intFacilityId AND e.HospitalLocationId = @inyHospitalLocationId ";
                ds = objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
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

        public DataSet GetFrequencyDoseTimeDetails(int HospitalLocationId, int FacilityId, int FrequencyId, int Days)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intFrequencyId", FrequencyId);
                HshIn.Add("@intDays", Days);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFrequencyDoseTime", HshIn);
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

        public DataSet GetFrequencyDoseTimeDetails(int HospitalLocationId, int FacilityId, int FrequencyId, int Days, int Encounterid, int Registrationid, int IndentId, int ItemId, string RangeTo, string RangeFrom, string PatientType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intFrequencyId", FrequencyId);
                HshIn.Add("@intDays", Days);

                HshIn.Add("@intEncounterid", Encounterid);
                HshIn.Add("@intRegistrationid", Registrationid);
                HshIn.Add("@IndentId", IndentId);
                HshIn.Add("@ItemId", ItemId);

                HshIn.Add("@chrRangeTo", RangeTo);
                HshIn.Add("@chrRangeFrom", RangeFrom);
                HshIn.Add("@chrPatientType", PatientType);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFrequencyDoseTime", HshIn);
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

        public Hashtable SaveSingleScreenDashboard(int iHospitalLocationId, int iRegistrationId, int iEncounterId,
             int iLoginFacilityId, int iDoctorId, int iUserId, string xmlProblemDetails, string sProvisionalDiagnosis,
             string xmlVitalString, string strXMLDrug, string strXMLOther, int iProvisionalDiagnosisId, int iDiagnosisSearchId,
             int bitNKDA, string xmlTemplateDetails, int iSign, string xmlNonDrugOrder, string InstructionRemarks, int ConsultationVisitTypeId,
             bool bAdmitting, bool bProvisional, bool bFinal, bool bChronic, bool bDischarge)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intEncodedBy", iUserId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@xmlProblemDetails", xmlProblemDetails);
                HshIn.Add("@intProvisionalDiagnosisId", iProvisionalDiagnosisId);
                HshIn.Add("@chvProvisionalDiagnosis", sProvisionalDiagnosis);
                HshIn.Add("@intDiagnosisSearchId", iDiagnosisSearchId);
                HshIn.Add("@xmlVitalDetails", xmlVitalString);
                HshIn.Add("@xmlDrugAllergyDetails", strXMLDrug);
                HshIn.Add("@xmlOtherAllergyDetails", strXMLOther);
                HshIn.Add("@bitNKDA ", bitNKDA);
                HshIn.Add("@intSign ", iSign);
                HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
                HshIn.Add("@xmlNonDrugOrder", xmlNonDrugOrder);

                HshIn.Add("@bitAdmitting", bAdmitting);
                HshIn.Add("@bitProvisional ", bProvisional);
                HshIn.Add("@bitFinal ", bFinal);
                HshIn.Add("@bitChronic", bChronic);
                HshIn.Add("@bitDischarge", bDischarge);

                if (ConsultationVisitTypeId > 0)
                {
                    HshIn.Add("@intConsultationVisitTypeId", ConsultationVisitTypeId);
                }
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@intEMRTemplateDataId", SqlDbType.Int);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientDashBoard", HshIn, HshOut);

                if (!Convert.ToString(InstructionRemarks).Equals(string.Empty) && Convert.ToInt32(HshOut["@intEMRTemplateDataId"]) > 0)
                {
                    string sql = "update EMRPatientNotesData set ValueWordProcessor=N'" + InstructionRemarks + "' where Id=" + (HshOut["@intEMRTemplateDataId"]) + " and EncounterId=" + iEncounterId + " and RegistrationId=" + iRegistrationId + "";
                    int i = objDl.ExecuteNonQuery(CommandType.Text, sql);
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
            return HshOut;

        }

        public Hashtable SaveSingleScreenDashboard(int iHospitalLocationId, int iRegistrationId, int iEncounterId,
             int iLoginFacilityId, int iDoctorId, int iUserId, int intIndentType, string xmlItemDetail, int intStoreId)
        {




            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            HshIn.Add("@intEncounterId", iEncounterId);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intEncodedBy", iUserId);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intIndentType", intIndentType);
            HshIn.Add("@xmlItemDetail", xmlItemDetail);
            HshIn.Add("@intStoreId", intStoreId);



            //            @intIndentType TINYINT = 1, 
            //@xmlItemDetail XML,  
            //@intStoreId INT = NULL


            //HshIn.Add("@xmlProblemDetails", xmlProblemDetails);
            //HshIn.Add("@intProvisionalDiagnosisId", iProvisionalDiagnosisId);
            //HshIn.Add("@chvProvisionalDiagnosis", sProvisionalDiagnosis);
            //HshIn.Add("@intDiagnosisSearchId", iDiagnosisSearchId);
            //HshIn.Add("@xmlVitalDetails", xmlVitalString);
            //HshIn.Add("@xmlDrugAllergyDetails", strXMLDrug);
            //HshIn.Add("@xmlOtherAllergyDetails", strXMLOther);

            //HshIn.Add("@bitNKDA ", bitNKDA);
            //HshIn.Add("@intSign ", iSign);
            //HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
            //HshIn.Add("@xmlNonDrugOrder", xmlNonDrugOrder);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientDashBoard", HshIn, HshOut);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveSingleScreenData", HshIn, HshOut);
            return HshOut;
        }


        public Hashtable SaveSingleScreenDashboard(int iHospitalLocationId, int iRegistrationId, int iEncounterId,
                            int iLoginFacilityId, int iDoctorId, int iUserId, string xmlProblemDetails, string sProvisionalDiagnosis,
                            string xmlVitalString, string strXMLDrug, string strXMLOther, int iProvisionalDiagnosisId, int iDiagnosisSearchId,
                            int bitNKDA, string xmlTemplateDetails, int iSign, string xmlNonDrugOrder, bool IsProvisionalDignosis, bool IsFinalDignosis)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intEncodedBy", iUserId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@xmlProblemDetails", xmlProblemDetails);
                HshIn.Add("@intProvisionalDiagnosisId", iProvisionalDiagnosisId);
                HshIn.Add("@chvProvisionalDiagnosis", sProvisionalDiagnosis);
                HshIn.Add("@intDiagnosisSearchId", iDiagnosisSearchId);
                HshIn.Add("@xmlVitalDetails", xmlVitalString);
                HshIn.Add("@xmlDrugAllergyDetails", strXMLDrug);
                HshIn.Add("@xmlOtherAllergyDetails", strXMLOther);
                HshIn.Add("@bitNKDA ", bitNKDA);
                HshIn.Add("@intSign ", iSign);
                HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
                HshIn.Add("@xmlNonDrugOrder", xmlNonDrugOrder);
                HshIn.Add("@bitProvisional", IsProvisionalDignosis);
                HshIn.Add("@bitFinal", IsFinalDignosis);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientDashBoard", HshIn, HshOut);
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
            return HshOut;
        }

        public Hashtable SaveSingleScreenCareMaster(string PlanName, int iHospitalLocationId, int iRegistrationId, int iEncounterId,
                         int iLoginFacilityId, int iUserId, string xmlProblemDetails,
                         string strXMLDrug, string xmlTemplateDetails, string xmlOrderProcedure, string xmlOrderProcedure1, string xmlDiagnosis)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@PlanName", PlanName);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intEncodedBy", iUserId);
                HshIn.Add("@xmlProblemDetails", xmlProblemDetails);
                HshIn.Add("@xmlDrugDetails", strXMLDrug);
                HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
                HshIn.Add("@xmlOrderProcedure", xmlOrderProcedure);
                HshIn.Add("@xmlOrderProcedure1", xmlOrderProcedure1);
                HshIn.Add("@xmlDiagnosis", xmlDiagnosis);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveDoctorCarePlan", HshIn, HshOut);
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
            return HshOut;
        }

        public DataSet getPermissionConfidentialUsers(int HospitalLocationId, int FacilityId, int TemplateId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intEncounteId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRPermissionConfidentialUsers", HshIn);
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
        public Hashtable SavePermissionConfidentialUsers(int HospitalLocationId, int FacilityId, int TemplateId, int EncounterId, int EncodedBy, string xmlDoctorId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intEncounteId", EncounterId);
                HshIn.Add("@xmlDoctorId", xmlDoctorId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRPermissionConfidentialUsers", HshIn, HshOut);
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
            return HshOut;

        }
        public DataSet getPreviousMedicationOP(int HospId, int FacilityId, int EncounterId, int RegistrationId, int IndentId, int ItemId,
                                                 string sPreviousMedication, string ItemName, string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvPreviousMedication", sPreviousMedication);
                if (FromDate.Trim() != string.Empty && ToDate.Trim() != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (ItemName.Trim() != string.Empty)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPreviousMedicationOP", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet getPreviousMedicationOP(int HospId, int FacilityId, int EncounterId, int RegistrationId, int IndentId, int ItemId,
                                        string sPreviousMedication, string ItemName, string FromDate, string ToDate, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chvPreviousMedication", sPreviousMedication);

                if (FromDate.Trim() != string.Empty && ToDate.Trim() != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }

                if (ItemName.Trim() != string.Empty)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPreviousMedicationOP", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet getCompanyType(int Id)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetCompanyType", HshIn);
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

        public string SaveCompanyType(int id, string Name, string Flag, int OutStandingLimitInMonth, int OverDueLimitInDays, int AgeingInterval, int Active, int userId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@chrFlag", Flag);
                HshIn.Add("@intOutStandingLimitInMonth", OutStandingLimitInMonth);
                HshIn.Add("@intOverDueLimitInDays", OverDueLimitInDays);
                HshIn.Add("@intAgeingInterval", AgeingInterval);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", userId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCompanyType", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public int getEncounterBasedOnPrescription(int IndentId, string OPIP)
        {
            DataSet ds = new DataSet();
            int encounterId = 0;
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intIndentId", IndentId);

                string strquery = string.Empty;

                if (OPIP.Equals("I"))
                {
                    strquery = "SELECT EncounterId FROM ICMIndentMain WITH (NOLOCK) WHERE IndentId = @intIndentId AND Active = 1";
                }
                else
                {
                    strquery = "SELECT EncounterId FROM OPPrescriptionMain WITH (NOLOCK) WHERE IndentId = @intIndentId AND Active = 1";
                }

                ds = objDl.FillDataSet(CommandType.Text, strquery, HshIn);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        encounterId = Convert.ToInt32(ds.Tables[0].Rows[0]["EncounterId"]);
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
            return encounterId;
        }


        public string SaveUnApprovedPrescriptions(int UnAppPrescriptionId, int HospId, int FacilityId, int EncounterId, int IndentId, string IndentNo,
            string IndentDate, int IndentTypeId, string IndentType, int GenericId, string GenericName, int ItemId, string ItemName,
            bool CustomMedication, int FrequencyId, string FrequencyName, double Frequency, double Dose, string Duration,
            string DurationText, string Instructions, int UnitId, string UnitName, string cType, string StartDate, string EndDate,
            string CIMSItemId, string CIMSType, int VIDALItemId, string XMLData, string PrescriptionDetail, int FormulationId,
            string FormulationName, int RouteId, string RouteName, int StrengthId, string StrengthValue, double Qty, string FoodRelationship,
            int FoodRelationshipID, int ReferanceItemId, string ReferanceItemName, int DoseTypeId, string DoseTypeName, bool NotToPharmacy,
            bool IsInfusion, bool IsInjection, bool IsStop, bool IsCancel, string Volume, int VolumeUnitId, string TotalVolume,
            string InfusionTime, string TimeUnit, int FlowRate, int FlowRateUnit, string VolumeUnit, string XmlVariableDose,
            bool IsOverride, string OverrideComments, string OverrideCommentsDrugToDrug, string OverrideCommentsDrugHealth,
            string XMLFrequencyTime, bool IsSubstituteNotAllowed, string ICDCode, int EncodedBy, ref string ReturnUnAppPrescriptionId, int ClosingBalanceQty = 0)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                ReturnUnAppPrescriptionId = "0";
                HshIn.Add("@intUnAppPrescriptionId", UnAppPrescriptionId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@dtIndentDate", IndentDate);
                HshIn.Add("@inyIndentTypeId", IndentTypeId);
                HshIn.Add("@chrIndentType", IndentType);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@chvGenericName", GenericName);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvItemName", ItemName);
                HshIn.Add("@bitCustomMedication", CustomMedication);
                HshIn.Add("@inyFrequencyId", FrequencyId);
                HshIn.Add("@chvFrequencyName", FrequencyName);
                HshIn.Add("@decFrequency", Frequency);
                HshIn.Add("@decDose", Dose);
                HshIn.Add("@chvDuration", Duration);
                HshIn.Add("@chvDurationText", DurationText);
                HshIn.Add("@chvInstructions", Instructions);
                HshIn.Add("@inyUnitId", UnitId);
                HshIn.Add("@chvUnitName", UnitName);
                HshIn.Add("@chrType", cType);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@chvCIMSItemId", CIMSItemId);
                HshIn.Add("@chvCIMSType", CIMSType);
                HshIn.Add("@intVIDALItemId", VIDALItemId);
                HshIn.Add("@XMLData", XMLData);
                HshIn.Add("@chvPrescriptionDetail", PrescriptionDetail);
                HshIn.Add("@intFormulationId", FormulationId);
                HshIn.Add("@chvFormulationName", FormulationName);
                HshIn.Add("@intRouteId", RouteId);
                HshIn.Add("@chvRouteName", RouteName);
                HshIn.Add("@intStrengthId", StrengthId);
                HshIn.Add("@chvStrengthValue", StrengthValue);
                HshIn.Add("@decQty", Qty);
                HshIn.Add("@chvFoodRelationship", FoodRelationship);
                HshIn.Add("@intFoodRelationshipID", FoodRelationshipID);
                HshIn.Add("@intReferanceItemId", ReferanceItemId);
                HshIn.Add("@chvReferanceItemName", ReferanceItemName);
                HshIn.Add("@intDoseTypeId", DoseTypeId);
                HshIn.Add("@chvDoseTypeName", DoseTypeName);
                HshIn.Add("@bitNotToPharmacy", NotToPharmacy);
                HshIn.Add("@bitIsInfusion", IsInfusion);
                HshIn.Add("@bitIsInjection", IsInjection);
                HshIn.Add("@bitIsStop", IsStop);
                HshIn.Add("@bitIsCancel", IsCancel);
                HshIn.Add("@chvVolume", Volume);
                HshIn.Add("@inyVolumeUnitId", VolumeUnitId);
                HshIn.Add("@chvTotalVolume", TotalVolume);
                HshIn.Add("@chvInfusionTime", InfusionTime);
                HshIn.Add("@chrTimeUnit", TimeUnit);
                HshIn.Add("@intFlowRate", FlowRate);
                HshIn.Add("@intFlowRateUnit", FlowRateUnit);
                HshIn.Add("@chvVolumeUnit", VolumeUnit);
                HshIn.Add("@XmlVariableDose", XmlVariableDose);
                HshIn.Add("@bitIsOverride", IsOverride);
                HshIn.Add("@chvOverrideComments", OverrideComments);
                HshIn.Add("@chvOverrideCommentsDrugToDrug", OverrideCommentsDrugToDrug);
                HshIn.Add("@chvOverrideCommentsDrugHealth", OverrideCommentsDrugHealth);
                HshIn.Add("@XMLFrequencyTime", XMLFrequencyTime);
                HshIn.Add("@bitIsSubstituteNotAllowed", IsSubstituteNotAllowed);
                HshIn.Add("@chvICDCode", ICDCode);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intClosingBalanceQty", ClosingBalanceQty);
                HshOut.Add("@intReturnUnAppPrescriptionId", SqlDbType.VarChar);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRUnApprovedPrescriptions", HshIn, HshOut);
                try
                {
                    ReturnUnAppPrescriptionId = HshOut["@intReturnUnAppPrescriptionId"].ToString();
                }
                catch
                {
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
            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataSet getUnApprovedPrescriptions(int HospId, int FacilityId, int EncounterId, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRUnApprovedPrescriptions", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }

        public string deleteUnApprovedPrescriptions(int UnAppPrescriptionId, int EncounterId, int EncodedBy, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intUnAppPrescriptionId", UnAppPrescriptionId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intFacilityId", FacilityId);
                string qry = "UPDATE EMRUnApprovedPrescriptions SET Active=0, LastChangedBy=@intEncodedBy, LastChangedDate=GETUTCDATE() " +
                             " WHERE UnAppPrescriptionId=@intUnAppPrescriptionId AND EncounterId=@intEncounterId AND FacilityId=@intFacilityId";
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
            return "Delete from un-approved prescriptions succeeded !";
        }

        public string SaveSingleScreenDataInTransit(int HospId, int RegistrationId, int EncounterId, int FacilityId, string Complaints, bool IsNoAllergies,
                                            int AllergyId, string AllergyName, string AllergyType, int ServerityId,
                                            string VitalHT, string VitalWT, string VitalHC, string VitalT, string VitalR, string VitalP, string VitalBPS, string VitalBPD, string VitalMAC, string VitalSPO2, string VitalBMI, string VitalBSA,
                                            string History, string PastHistory, string PreviousTreatment, string Examination, string NutritionalStatus,
                                            string CostAnalysis, string PlanOfCare, int DiagnosisSearchKeyId, string ProvisionalDiagnosis, string OrderType,
                                            int NonDrugDoctorId, string NonDrugOrder, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvComplaints", Complaints);
                HshIn.Add("@bitNoAllergies", IsNoAllergies);
                HshIn.Add("@intAllergyId", AllergyId);
                HshIn.Add("@chvAllergyName", AllergyName);
                HshIn.Add("@chvAllergyType", AllergyType);
                HshIn.Add("@intServerityId", ServerityId);
                HshIn.Add("@chvHT", VitalHT);
                HshIn.Add("@chvWT", VitalWT);
                HshIn.Add("@chvHC", VitalHC);
                HshIn.Add("@chvT", VitalT);
                HshIn.Add("@chvR", VitalR);
                HshIn.Add("@chvP", VitalP);
                HshIn.Add("@chvBPS", VitalBPS);
                HshIn.Add("@chvBPD", VitalBPD);
                HshIn.Add("@chvMAC", VitalMAC);
                HshIn.Add("@chvSPO2", VitalSPO2);
                HshIn.Add("@chvBMI", VitalBMI);
                HshIn.Add("@chvBSA", VitalBSA);
                HshIn.Add("@chvHistory", History);
                HshIn.Add("@chvPastHistory", PastHistory);
                HshIn.Add("@chvPreviousTreatment", PreviousTreatment);
                HshIn.Add("@chvExamination", Examination);
                HshIn.Add("@chvNutritionalStatus", NutritionalStatus);
                HshIn.Add("@chvCostAnalysis", CostAnalysis);
                HshIn.Add("@chvPlanOfCare", PlanOfCare);
                HshIn.Add("@intDiagnosisSearchKeyId", DiagnosisSearchKeyId);
                HshIn.Add("@chvProvisionalDiagnosis", ProvisionalDiagnosis);
                HshIn.Add("@chvOrderType", OrderType);
                HshIn.Add("@intNonDrugDoctorId", NonDrugDoctorId);
                HshIn.Add("@chvNonDrugOrder", NonDrugOrder);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveDataInTransit", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataSet getEMRSingleScreenDataInTransit(int FacilityId, int EncounterId, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetSingleScreenDataInTransit", HshIn);
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
        public DataSet getEMRSingleScreenDataInTransitList(int FacilityId, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetSingleScreenDataInTransitList", HshIn);
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
        public int InActiveSingleScreenDataInTransit(int TransitId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            try
            {
                HshIn.Add("@intTransitId", TransitId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                string strQry = "UPDATE EMRSingleScreenDataInTransit SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE Id = @intTransitId";
                i = objDl.ExecuteNonQuery(CommandType.Text, strQry, HshIn);
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
            return i;
        }
        public bool IsEPrescriptionEnabled(int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            bool IsEprescriptionEnabled = false;
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);
                string strquery = "SELECT CONVERT(TINYINT,ISNULL(EprescriptionEnabled,0)) AS EprescriptionEnabled FROM doctorDetails WITH(NOLOCK) WHERE DoctorID=@intDoctorId";
                ds = objDl.FillDataSet(CommandType.Text, strquery, HshIn);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        IsEprescriptionEnabled = (Convert.ToInt32(ds.Tables[0].Rows[0]["EprescriptionEnabled"]) == 1) ? true : false;
                    }
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
            return IsEprescriptionEnabled;
        }
        public void UpdateDHARefNo(string RegNo, int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@refno", RegNo);
                HshIn.Add("@identid", IndentId);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateDHARefNo", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                HshOut = null;
            }
        }
        public DataSet ValidateErxPatientXML(int RegistrationId, int DoctorId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@registrationid", RegistrationId);
                HshIn.Add("@doctorID", DoctorId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspValidateErxPatientXML", HshIn);
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

        public DataSet getClinicianLoginforErx(int UserId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@userid", UserId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetClinicianLoginforErx", HshIn);
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

        public string GenerateeOPRxXml(int HospId, int FacilityId, int IndentId, string DispositionFlag)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@iHospitalLocationId", HospId);
                HshIn.Add("@iFacilityId", FacilityId);
                HshIn.Add("@PrescriptionID", IndentId);
                if (!DispositionFlag.Equals(string.Empty))
                {
                    HshIn.Add("@DispositionFlag", DispositionFlag);
                }
                HshOut.Add("@returnXML", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGenerateeOPRxXml", HshIn, HshOut);
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
            return HshOut["@returnXML"].ToString();
        }

        public DataSet GetPrescriptionMasterList(int HospId, int FacilityId, int RegistrationId, int EncounterId, int UserId,
                                        int DepartmentId, int DoctorId, int WardId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", UserId);

                if (DepartmentId > 0)
                {
                    HshIn.Add("@intDepartmentId", DepartmentId);
                }
                if (DoctorId > 0)
                {
                    HshIn.Add("@DoctorId", DoctorId);
                }
                if (WardId > 0)
                {
                    HshIn.Add("@intWardId", WardId);
                }


                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPrescriptionMasterList", HshIn, HshOut);
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

        public DataSet GetPrescriptionMasterList(int HospId, int FacilityId, int RegistrationId, int EncounterId, int UserId,
                                        int DepartmentId, int DoctorId, int WardId, int FormId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", UserId);

                if (DepartmentId > 0)
                {
                    HshIn.Add("@intDepartmentId", DepartmentId);
                }
                if (DoctorId > 0)
                {
                    HshIn.Add("@DoctorId", DoctorId);
                }
                if (WardId > 0)
                {
                    HshIn.Add("@intWardId", WardId);
                }
                if (FormId > 0)
                {
                    HshIn.Add("@intFormId", FormId);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPrescriptionMasterList", HshIn, HshOut);
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


        public DataSet getStoreMasterList(int HospId, int FacilityId, int UserId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intEncounterId", EncounterId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetStoreMasterList", HshIn, HshOut);
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



        public string EMRSaveProgressNoteInTransit(int inyHospitalLocationId, int intLoginFacilityId, int intRegistrationId, int intEncounterId,
                                            int intDoctorId, string chvProgressNote, int intEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                HshIn.Add("@intRegistrationId", intRegistrationId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@intDoctorId", intDoctorId);
                HshIn.Add("@chvProgressNote", chvProgressNote);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProgressNoteInTransit", HshIn, HshOut);
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
            return HshOut["@chvErrorOutput"].ToString();
        }
        public DataSet GetProgressNoteInTransit(int intFacilityId, int intEncounterId, int intEncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetProgressNoteInTransit", HshIn);
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

        public DataSet getPatientNoteStatus(int EncounterId, int FormId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder sb = new StringBuilder();
            try
            {
                HshIn.Add("@intencounterid", EncounterId);
                HshIn.Add("@intFormId", FormId);
                sb.Append("SELECT StatusId, FormName FROM EMRPatientForms epf WITH(NOLOCK) INNER JOIN EMRForms ef WITH(NOLOCK) ON epf.FormId = ef.Id");
                sb.Append(" WHERE EncounterId = @intencounterid");
                sb.Append(" AND FormId = @intFormId AND epf.Active=1");
                sb.Append(" SELECT StatusId, Status, Code FROM GetStatus(1,'Notes') WHERE Code='P'");
                sb.Append(" SELECT StatusId, Status, Code FROM GetStatus(1,'Notes') WHERE Code='S'");
                ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
                sb = null;
            }
            return ds;
        }

        public int GetEMRTemplateId(int HospId, string TemplateName)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder sb = new StringBuilder();
            int TemplateId = 0;
            try
            {
                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@TemplateName", TemplateName);
                sb.Append("SELECT Id FROM EMRTemplate WITH(NOLOCK) WHERE templateName = @TemplateName AND HospitalLocationId=@HospitalLocationId");
                ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    TemplateId = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
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
                sb = null;
                ds.Dispose();
            }
            return TemplateId;
        }

        public int updateEMRPatientForms(int RegistrationId, int EncounterId, string PatientSummary)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            StringBuilder sb = new StringBuilder();
            try
            {
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@EncounterId", EncounterId);
                HshIn.Add("@PatientSummary", PatientSummary);
                sb.Append("UPDATE EMRPatientForms SET PatientSummary=@PatientSummary, PdfNote=CONVERT(VARBINARY(MAX),@PatientSummary)");
                sb.Append(" WHERE RegistrationId=@RegistrationId AND EncounterId = @EncounterId");
                i = objDl.ExecuteNonQuery(CommandType.Text, sb.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
                sb = null;
            }
            return i;
        }

        public DataSet getBillingActivitySheet(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBillingActivitySheet", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }
        public string SaveBillingActivitySheet(int intHospId, int intFacilityId, int intRegistrationId, int intEncounterId,
                                    DateTime dtVisitDate, bool boolFirstVisit, bool boolFreeVisit, bool boolRepeatVisit, bool boolFollowUpVisit,
                                    bool boolInsuranceCovered, string strRemarks, bool boolActive, int intEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", intHospId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@intRegistrationId", intRegistrationId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@dtVisitDate", dtVisitDate);
                HshIn.Add("@bitFirstVisit", boolFirstVisit);
                HshIn.Add("@bitFreeVisit", boolFreeVisit);
                HshIn.Add("@bitRepeatVisit", boolRepeatVisit);
                HshIn.Add("@bitFollowUpVisit", boolFollowUpVisit);
                HshIn.Add("@bitInsuranceCovered", boolInsuranceCovered);
                HshIn.Add("@chvRemarks", strRemarks);
                HshIn.Add("@bitActive", boolActive);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveBillingActivitySheet", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetHospitalSetup(int intHospitalLocationId, int intFacilityId, int Id)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@iHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@iFacilityId", intFacilityId);
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalSetup", HshIn);
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

        public string uspSaveHospitalSetup(int id, int intHospitalLocationId, int intFacilityId, string Flag, string Value, int Active, int intEncodedby)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@chrFlag", Flag);
                HshIn.Add("@chrvalue", Value);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", intEncodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveHospitalSetup", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getExternalCentre(int Id)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetExternalCentre", HshIn);
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

        public string SaveExternalCentre(int id, int HospitalLocationId, string CenterName, string AccountNo, string Address1, string Address2, string Phone, string Email, string Fax, int Active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chrName", CenterName);
                HshIn.Add("@chrAccountNo", AccountNo);
                HshIn.Add("@chrAddress1", Address1);
                HshIn.Add("@chrAddress2", Address2);
                HshIn.Add("@chrPhone", Phone);
                HshIn.Add("@chrEmail", Email);
                HshIn.Add("@chrFax", Fax);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveExternalCentre", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getLeadSource(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetLeadSourceMaster", HshIn, HshOut);
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


        public string SaveLeadSource(int id, int HospitalLocationId, int FacilityId, string Name, int Active, int Encodedby)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveLeadSourceMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getFacility()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objDl.FillDataSet(CommandType.Text, "select facilityid, name from facilitymaster WITH (NOLOCK)");
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

        public DataSet getStation()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objDl.FillDataSet(CommandType.Text, "select stationid,stationname from DiagSampleReceivingStation WITH (NOLOCK) where active=1");
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

        public DataSet getDiagEntrySite(int Id)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDiagEntrySite", HshIn);
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

        public string SaveDiagEntrySite(int id, int HospitalLocationId, int FacilityId, string Name, int StationId, int Active, int Encodedby)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDiagEntrySite", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }


        public DataSet getReligion(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetReligionMaster", HshIn, HshOut);
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


        public string SaveReligion(int id, string Name, int Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveReligionMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public DataSet getOccupation(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetOccupationMaster", HshIn, HshOut);
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


        public string SaveOccupation(int id, string Name, int Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveoccupationmaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getSubDepartment()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objDl.FillDataSet(CommandType.Text, "select SubDeptId,SubName from departmentsub ds WITH (NOLOCK) inner join departmentmain dm WITH (NOLOCK) on ds.DepartmentID = dm.DepartmentID where dm.DepartmentID =1");
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
        public DataSet getPaymentMode(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPaymentMode", HshIn, HshOut);
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
        public string SavePaymentMode(int id, string Name, int SubDeptId, string InterfaceMappingPrefix, string InterfaceMappingMode, string AdjustmentSeqNo, int Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@intSubDeptId", SubDeptId);
                HshIn.Add("@chrInterfaceMappingPrefix", InterfaceMappingPrefix);
                HshIn.Add("@chrInterfaceMappingMode", InterfaceMappingMode);
                HshIn.Add("@intAdjustmentSeqNo", AdjustmentSeqNo);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePaymentMode", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getSource(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetSource", HshIn, HshOut);
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


        public string SaveSource(int id, string Name, string Type, int Active, int userId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@chrType", Type);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", userId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSource", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public DataSet getRelationship(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRelation", HshIn, HshOut);
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


        public string SaveRelationship(int id, string Code, string Name, int Gender, int Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrcode", Code);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@intGender", Gender);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveRelationship", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public DataSet getPatientStatus(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientStatus", HshIn, HshOut);
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

        public string SavePatientStatus(int id, string Name, string Type, string Status, int userId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrName", Name);
                HshIn.Add("@chrType", Type);
                HshIn.Add("@bitActive", Status);
                HshIn.Add("@intEncodedBy", userId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientStatus", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet getStatus(int Id, string StatusType)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                HshIn.Add("@StatusType", StatusType);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetStatus", HshIn, HshOut);
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
        public string SaveStatus(int Statusid, string StatusType, string Status, string StatusColor, string ColorName, string StatusCode, int Sequenceno, int Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@Statusid", Statusid);
                HshIn.Add("@StatusType", StatusType);
                HshIn.Add("@Status", Status);
                HshIn.Add("@StatusColor", StatusColor);
                HshIn.Add("@ColorName", ColorName);
                HshIn.Add("@Code", StatusCode);
                HshIn.Add("@SequenceNo", Sequenceno);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveStatusmasters", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }


        public DataSet getEmployeeType(int Id)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@id", Id);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEpmloyeeType", HshIn, HshOut);
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
        public string SaveEmployeeType(int id, string Name, string EmployeeType, int Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@id", id);
                HshIn.Add("@chrDescription", Name);
                HshIn.Add("@chrEmployeeType", EmployeeType);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEmployeeType", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public DataSet getTitleMaster(int TitleId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intTitleId", TitleId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTitleMaster", HshIn, HshOut);
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
        public string SaveTitleMaster(int TitleId, string Name, string Gender, int Active, int IsNewBornTitle)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@TitleId", TitleId);
                HshIn.Add("@ChrName", Name);
                HshIn.Add("@chrGender", Gender);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@bitIsNewBornTitle", IsNewBornTitle);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveTitleMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public DataSet getStatusType()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objDl.FillDataSet(CommandType.Text, "select distinct StatusType from StatusMaster WITH (NOLOCK) where active=1");
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
        public string SaveFacilitatorMaster(int ID, string Name, int Active, int EncodedBy, int hospitallocation, int facility)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@Id", ID);
                HshIn.Add("@ChrName", Name);
                HshIn.Add("@hospitalLocationID", hospitallocation);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@FacilityID", facility);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveFacilitatorMaster", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }
        public string BindApplicationName(int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            string sApplicationName = string.Empty;
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                string sQuery = "select Name from FacilityMaster WITH (NOLOCK) where Active = 1 and FacilityId=@intFacilityId AND HospitalLocationId = @inyHospitalLocationId ";
                ds = objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sApplicationName = ds.Tables[0].Rows[0]["Name"].ToString();
                }
                else
                {
                    sApplicationName = "EMR";
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
            return sApplicationName;

        }


        public bool IsPatientExpire(string chvRegistrationNo)
        {
            bool IsExpire = false;
            DataSet ds1 = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@registrationNo", chvRegistrationNo);
                ds1 = objDl.FillDataSet(CommandType.StoredProcedure, "uspPatientDeathRecord", HshIn);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    IsExpire = true;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                ds1.Dispose();
                HshIn = null;
                objDl = null;
            }
            return IsExpire;

        }

        public DataSet GetConsultationVisit()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetConsultationVisit", HshIn);
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

        public DataSet getEMRCommonServicesForTemplate(string xmlServices, int TemplateId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@xmlServices", xmlServices);
                HshIn.Add("@intTemplateId", TemplateId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetCommonServicesForTemplate", HshIn);
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

        public DataSet GetMLCDetails(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetMLCDetails", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;

        }

        public string SaveMLCDetails(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId,
            string MLCReferBy, string MLCReferNo, int MLCType, bool MLC)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@MLCReferBy", MLCReferBy);
                HshIn.Add("@MLCReferNo", MLCReferNo);
                HshIn.Add("@MLCType", MLCType);
                HshIn.Add("@MLC", MLC);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveMLCDetails", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

















        public DataSet getLastEncounterDetails(int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@EncounterId", EncounterId);

                qry.Append("SELECT TOP 1 Id AS EncounterId, EncounterNo, RegistrationId, RegistrationNo, OPIP, DoctorId ");
                qry.Append(" FROM Encounter WITH (NOLOCK) ");
                qry.Append(" WHERE RegistrationId = @RegistrationId AND Active = 1 ");
                qry.Append(" AND Id < @EncounterId AND FacilityID = @FacilityId ");
                qry.Append(" ORDER BY Id DESC");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }






        public DataSet GetEMRServiceOrderDetails(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId,
                                                int iBillId, int iDepartmentId, string chrDepartmentType)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("RegId", iRegistrationId);
                HshIn.Add("EncounterId", iEncounterId);
                HshIn.Add("intDepartmentId", iDepartmentId);
                HshIn.Add("chrDepartmentType", chrDepartmentType);
                if (iBillId > 0)
                {
                    HshIn.Add("BillId", iBillId);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetServiceOrderDetails", HshIn);
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

        public DataSet GetEMRServiceOrderDetails(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId,
                                              int iBillId, int iDepartmentId, string chrDepartmentType, DateTime? FromDate, DateTime? ToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("RegId", iRegistrationId);
                HshIn.Add("EncounterId", iEncounterId);
                HshIn.Add("intDepartmentId", iDepartmentId);
                HshIn.Add("chrDepartmentType", chrDepartmentType);
                if (iBillId > 0)
                {
                    HshIn.Add("BillId", iBillId);
                }
                HshIn.Add("Fdate", FromDate);
                HshIn.Add("Tdate", ToDate);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetServiceOrderDetails", HshIn);
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



        public DataSet Get_EMRProblems(string Typename, int SpecializationId)
        {
            DataSet ds = new DataSet();
            Hashtable hstInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hstInput.Add("@Typename", Typename);
            hstInput.Add("@intSpecializationId", SpecializationId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "usp_Get_EMRProblems", hstInput);
            return ds;
        }


        public string SaveEMRMedicineAddFav(int DoctorId, int BrandId, int EncodedBy, string FormularyType, double Dose, int UnitId,
                            int StrengthId, int FormulationId, int RouteId, int FrequencyId, int Duration,
                            string DurationType, int FoodRelationshipId, string StrengthValue, string Instruction)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@intDoctorId", DoctorId);
            hshIn.Add("@Drug_Syn_Id", BrandId);
            hshIn.Add("@intEncodedBy", EncodedBy);
            hshIn.Add("@chrFormularyType", FormularyType);
            hshIn.Add("@decDose", Dose);
            hshIn.Add("@intUnitId", UnitId);


            hshIn.Add("@intStrengthId", StrengthId);
            hshIn.Add("@intFormulationId", FormulationId);
            hshIn.Add("@intRouteId", RouteId);
            hshIn.Add("@intFrequencyId", FrequencyId);
            hshIn.Add("@intDuration", Duration);
            hshIn.Add("@chrDurationType", DurationType);
            hshIn.Add("@intFoodRelationshipId", FoodRelationshipId);
            hshIn.Add("@chvStrengthValue", StrengthValue);
            hshIn.Add("@chvInstruction", Instruction);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveFavDrugs", hshIn, hshOut);
            return hshOut["@chvErrorStatus"].ToString();

        }

        public string DeleteFavoriteOrder(int DoctorId, int ItemId, int EncodedBy)
        {

            string sResult = "";
            Hashtable hstInput = new Hashtable();
            hstInput.Add("@intDoctorId", DoctorId);
            hstInput.Add("@intServiceId", ItemId);
            hstInput.Add("@intEncodedBy", EncodedBy);
            DAL.DAL dal = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (dal.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeleteFavService", hstInput) == 0)
            {
                sResult = "Service Deleted from Favourtes successfull";
            }
            return sResult;

        }

        public Hashtable DeleteExamination(int intHospitalLocationId, int intRegistrationId, int intEncounterId,
                    int intRecordId, int intTemplateId, int intEncodedBy)
        {

            //            @intHospitalLocationId INT,
            //@intRegistrationId INT,
            //@intEncounterId INT,
            //@intRecordId INT,
            //@intTemplateId INT,
            //@intEncodedBy INT,
            //@chvErrorStatus VARCHAR(MAX)='' OUTPUT



            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            hshIn.Add("@intRegistrationId", intRegistrationId);
            hshIn.Add("@intEncounterId", intEncounterId);
            hshIn.Add("@intRecordId", intRecordId);
            hshIn.Add("@intTemplateId", intTemplateId);
            hshIn.Add("@intEncodedBy", intEncodedBy);

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeletePatientTemplateData", hshIn, hshOut);
            // return hshOut["@chvErrorStatus"].ToString();
            return hshOut;
        }

        public Hashtable DeleteDiagnosisDetails(int intHospitalLocationId, int intEncounterId, int intEncodedBy,
                int UniqueId)
        {


            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            hshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            hshIn.Add("@intEncounterId", intEncounterId);
            hshIn.Add("@intEncodedBy", intEncodedBy);
            hshIn.Add("@UniqueId", UniqueId);

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteDiagnosisDetails", hshIn, hshOut);
            // return hshOut["@chvErrorStatus"].ToString();
            return hshOut;
        }

        public Hashtable SavePatientLastEncounterData(int iHospitalLocationId, int iRegistrationId, int iEncounterId,
     int iLoginFacilityId, int iDoctorId, int iUserId, string sTemplateId, string PrescriptionDetails)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            HshIn.Add("@intEncounterId", iEncounterId);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intEncodedBy", iUserId);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@xmlTemplateId", sTemplateId);
            HshIn.Add("@xmlPrescriptionDetails", PrescriptionDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientLastEncounterData", HshIn, HshOut);
            return HshOut;
        }

        public DataSet SavePatientLastEncounterDataDS(int iHospitalLocationId, int iRegistrationId, int iEncounterId,
                        int iLoginFacilityId, int iDoctorId, int iUserId, string sTemplateId, string PrescriptionDetails)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            HshIn.Add("@intEncounterId", iEncounterId);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intEncodedBy", iUserId);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@xmlTemplateId", sTemplateId);
            HshIn.Add("@xmlPrescriptionDetails", PrescriptionDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvPrecriptionNo", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientLastEncounterData", HshIn, HshOut);
            //return HshOut;
            HshOut["@chvErrorStatus"].ToString();

            DataTable dt = new DataTable();

            DataColumn dC;
            dC = new DataColumn("chvErrorStatus", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);
            dC = new DataColumn("chvPrescriptionNo", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);


            DataRow dr = dt.NewRow();
            if (Convert.ToString(HshOut["@chvErrorStatus"]).Equals(""))
            {
                dr["chvErrorStatus"] = "";
            }
            else
            {
                dr["chvErrorStatus"] = HshOut["@chvErrorStatus"].ToString();
            }
            if (Convert.ToString(HshOut["@chvPrecriptionNo"]).Equals(""))
            {
                dr["chvPrescriptionNo"] = "0$0";
            }
            else
            {
                dr["chvPrescriptionNo"] = HshOut["@chvPrecriptionNo"].ToString();
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            return ds;


        }

        /* added by kabir on 20-11-2015 */

        public DataSet SetDeletegvAddList(int ItemId, int DetailsId, int IndentId, string OPIP)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@ItemId", ItemId);
                HshIn.Add("@DetailsId", DetailsId);
                HshIn.Add("@IndentId", IndentId);
                HshIn.Add("@OPIP", OPIP);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDeletegvAddList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public DataSet GetPrescriptionDetails()
        {
            Hashtable HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();


            ds = objDl.FillDataSet(CommandType.StoredProcedure, "usp_GetPrescriptionDetails");

            return ds;
        }

        public DataSet getHealthCheckUpCheckLIsts(int EncounterId, int RegistrationId, int ReportId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intReportId", ReportId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRHealthCheckUpCheckLists", HshIn);
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



        public DataSet getHealthCheckUpCheckLIsts(int EncounterId, int RegistrationId, int ReportId, int DoctorId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intDoctorId", DoctorId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRHealthCheckUpCheckLists", HshIn);
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
        public DataSet IsShowSectionAndTemplate(int ReportId, int SectionId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder qry = new StringBuilder();
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intSectionId", SectionId);

                qry.Append(" SELECT DISTINCT ISNULL(ShowTemplateNameInNote,0) as ShowTemplateNameInNote, ISNULL(ShowSectionNameInNote,0) as ShowSectionNameInNote ");
                qry.Append(" FROM EMRTemplateReportSetupDetails WITH(NOLOCK) ");
                qry.Append(" WHERE ReportId=@intReportId ");
                qry.Append(" AND SectionId=@intSectionId ");
                qry.Append(" AND Active=1 ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                qry = null;
                HshIn = null;
            }
            return ds;
        }
        public DataSet GetWindowPaitentInvestigationResult(Int64 RegistrationNo, string fromDate, string toDate, string strServiceCodes, string strResultView, string FlagDepatment)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@RegNo", RegistrationNo);
                HshIn.Add("@fromDate", fromDate);
                HshIn.Add("@toDate", toDate);
                HshIn.Add("@chvServiceCodes", strServiceCodes);
                HshIn.Add("@chrResultView", strResultView);
                HshIn.Add("@FlagDepatment", FlagDepatment);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "Get_Patient_Investigation_Result_ForWeb", HshIn);
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

        public DataSet GetOPIP_Investigation_Report_Status(Int64 RegistrationNo, string fromDate, string toDate, string FlagDepatment)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@RegNo", RegistrationNo);
                HshIn.Add("@fromDate", fromDate);
                HshIn.Add("@toDate", toDate);
                HshIn.Add("@FlagDepatment", FlagDepatment);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "OPIP_Investigation_Report_Status_ForWeb", HshIn);
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
        public DataSet getDischargeSummaryPrescription(int HospId, int FacilityId, int EncounterId, bool IsLastIndent, bool IsStopMedicine)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                if (IsLastIndent)
                {
                    HshIn.Add("@chrLastIndent", "Y");
                }
                if (IsStopMedicine)
                {
                    HshIn.Add("@chrStopMedicine", "Y");
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDischargeSummaryPrescription", HshIn);
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
        public DataSet getExternalCenterLabAttachments(int FacilityId, int RegistrationId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetExternalCenterLabAttachments", HshIn);
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

        public DataSet getEMRFavouriteDrugsAttributes(int DoctorId, int ItemId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intItemId", ItemId);

                qry.Append("SELECT FrequencyId,Duration,DurationType,Dose,UnitId, FormulationId, RouteId, FoodRelationshipId, Instructions FROM EMRFavouriteDrugs WITH(NOLOCK) WHERE DoctorID=@intDoctorId AND Drug_Syn_Id=@intItemId AND Active=1");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public DataSet getReportFormatDetails(int DoctorId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetReportFormatDetails", HshIn);
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

        public DataSet GetPatientPreviousWards(int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetPatientPreviousWards", HshIn, HshOut);
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

        public DataSet getFrequencyMaster(int FrequencyId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFrequencyId", FrequencyId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFrequencyMaster", HshIn);
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


        public DataSet getFrequencyDetails(int HospitalLocationID, int FacilityId, int FrequencyId, bool Active)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intFrequencyId", FrequencyId);
                HshIn.Add("@bitActive", Active);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFrequencyDetails", HshIn);
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

        public string SaveFrequencyDetails(int HospitalLocationId, int FacilityId, int FrequencyDetailId, int FrequencyId, DateTime FrequencyTime, bool Active)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intFrequencyDetailId", FrequencyDetailId);
                HshIn.Add("@intFrequencyId", FrequencyId);
                HshIn.Add("@dtFrequencyTime", FrequencyTime);
                HshIn.Add("@bitActive", Active);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveFrequencyDetails", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataTable GetCommonServices(int FacilityId, string chvServiceType, string chvServiceName, string chrOPIP)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvServiceType", chvServiceType);
                HshIn.Add("@chvServiceName", chvServiceName);
                HshIn.Add("@chrOPIP", chrOPIP);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetCommonServices", HshIn);

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
            return ds.Tables[0];
        }


        // Cancel EMR Diet
        public string CancelEMRDiet(int HospitalLocationId, int FacilityId, int RegistrationId, int EncouterId, int DietRequestID, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationID", RegistrationId);
                HshIn.Add("@intEncounterID", EncouterId);
                HshIn.Add("@intDietRequestID", DietRequestID);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCancelDietMain", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }


        public DataSet getEmployeePermission(int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder qry = new StringBuilder();
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@intUserId", UserId);

                qry.Append(" SELECT DISTINCT ISNULL(emp.IsAccessAllEncounter,0) AS IsAccessAllEncounter, ISNULL(emp.IsAccessSpecialisationResource,0) AS IsAccessSpecialisationResource ");
                qry.Append(" FROM Users usr WITH(NOLOCK) ");
                qry.Append(" INNER JOIN Employee emp WITH(NOLOCK) ON usr.EmpID = emp.ID AND emp.Active = 1 ");
                qry.Append(" WHERE usr.ID = @intUserId ");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                qry = null;
                HshIn = null;
            }
            return ds;
        }

        public string SavePatientCaseSheetData(int FacilityId, int EncounterId, int RegistrationId, StringBuilder CaseSheetSummary, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@CaseSheetSummary", CaseSheetSummary.ToString());
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientCaseSheetData", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getAntibiloticTaggedWithOTPatient(int otbookingId, int ServiceId)
        {
            Hashtable hshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hshIn.Add("@intOtbookingId", otbookingId);
            hshIn.Add("@intserviceId", ServiceId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspOTGetAntibioticDetails", hshIn);
            return ds;


        }

        public DataSet GetDiagAntibiotics(int HospitalLocationId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetAntibiotics", HshIn);
            return ds;

        }

        public DataSet DoctorPreviousNotes(int ID)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intProviderId", ID);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspDoctorPreviousNotes", HshIn);
            return ds;
        }

        public DataTable GetFreeTextTemplateSetup(int HospId, int FacilityId, int SpecialisationId, int DoctorId, int TemplateId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intSpecialisationId", SpecialisationId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intTemplateId", TemplateId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFreeTextTemplateSetup", HshIn);

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
            return ds.Tables[0];
        }

        public DataSet GetEMRGetTemplateDetails(int iFacilityId, int EncounterId, int RegistrationId, int TemplateId, string Gender, int RecordId, int EnocdedBy)
        {
            Hashtable hshInput = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@intFacilityId", iFacilityId);
            hshInput.Add("@intRegistrationId", RegistrationId);
            hshInput.Add("@intEncounterId", EncounterId);
            hshInput.Add("@intTemplateId", TemplateId);
            hshInput.Add("@chrGenderType", Gender);
            hshInput.Add("@intRecordId", RecordId);
            hshInput.Add("@intEncodedBy", EnocdedBy);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", hshInput);
            return ds;
        }
        public DataSet GetEWSScoreValues(int iHospitalId, int iFacilityId, string xmlScoreValue)
        {
            Hashtable hshInput = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@inyHospitalLocationId", iHospitalId);
            hshInput.Add("@intFacilityId", iFacilityId);
            hshInput.Add("@xmlScoreValue", xmlScoreValue);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEWSScoreValues", hshInput);
            return ds;
        }
        public DataTable GetEMRTemplateDataObjects()
        {
            string Query = "SELECT Id, ObjectName FROM EMRTemplateDataObjects WITH(NOLOCK) WHERE Active=1 ORDER BY ObjectName";
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.Text, Query);
            return ds.Tables[0];
        }

        public DataTable GetFindPatientVisitType(int iHospitalId)
        {
            Hashtable hshInt = new Hashtable();
            DataSet ds = new DataSet();
            hshInt.Add("@inyHospitalLocationId", iHospitalId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFindPatientVisitType", hshInt);
            return ds.Tables[0];
        }


        public DataTable GetEncounterFormatFinalizeStatus(int iEncounterId, int RegistrationID)
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@iRegistrationID", RegistrationID);
            hshInput.Add("@iEncounterId", iEncounterId);

            string strqry = "select FormatID,(case when Finalize = 1 then 'F' ELSE 'DF' END) AS FinalizeStatus from EMRPatientSummaryDetails where RegistrationID = @iRegistrationID and EncounterID = @iEncounterId";

            DataSet ds = objDl.FillDataSet(CommandType.Text, strqry, hshInput);
            return ds.Tables[0];
        }

        public DataTable GetConfidentialTemplate()
        {
            Hashtable hshInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            //hshInput.Add("@iRegistrationID", RegistrationID);
            //hshInput.Add("@iEncounterId", iEncounterId);

            string strqry = "SELECT ID , TemplateName FROM EMRTemplate WHERE IsConfidential=1 AND Active=1";

            DataSet ds = objDl.FillDataSet(CommandType.Text, strqry, hshInput);
            return ds.Tables[0];
        }

        public DataSet GetRequestedConfidentialTemplateDetails(int HospId, int FacilityId, int RequestedTemplateId, int RequestedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@HospitalLocationID", HospId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@RequestedTemplateId", RequestedTemplateId);
                HshIn.Add("@RequestedBy", RequestedBy);


                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetRequestedConfidentialTemplateDetails", HshIn);

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

        public string EMRConfidentialTemplateTagging(int HospitalLocationID, int FacilityId, int RequestedTemplateId, int RequestedBy,
            bool Approved, int ApprovedBy, string FlagRequestApprove)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@HospitalLocationID", HospitalLocationID);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@RequestedTemplateId", RequestedTemplateId);
                HshIn.Add("@RequestedBy", RequestedBy);
                HshIn.Add("@Approved", Approved);
                HshIn.Add("@ApprovedBy", ApprovedBy);
                HshIn.Add("@FlagRequestApprove", FlagRequestApprove);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRConfidentialTemplateTagging", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getVaccinationStockDetails(int FacilityId, string OPIP, int ItemBrandId, int ItemId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                if (ItemBrandId > 0)
                {
                    HshIn.Add("@intItemBrandId", ItemBrandId);
                }
                if (ItemId > 0)
                {
                    HshIn.Add("@intItemId", ItemId);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetVaccinationStockDetails", HshIn);
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

        public DataSet GetReferFromEmployeeList(int iEncounterId, int RegistrationID, int iFacilityId, int iHospitalLocationId, int RequestId)
        {
            Hashtable hshInput = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@inyHospitalLocationId", iHospitalLocationId);
            hshInput.Add("@intFacilityId", iFacilityId);
            hshInput.Add("@intRegistrationId", RegistrationID);
            hshInput.Add("@intEncounterId", iEncounterId);
            hshInput.Add("@intRequestId", RequestId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetReferFromEmployeeList", hshInput);
            return ds;
        }

        public DataTable GetLabResultTabularFormat(int iHospitalId, int iFacilityId, int RegistrationId, int DoctorId)
        {
            Hashtable hshInput = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput.Add("@inyHospitalLocationId", iHospitalId);
            hshInput.Add("@intFacilityId", iFacilityId);
            hshInput.Add("@intRegistrationId", RegistrationId);
            hshInput.Add("@intDoctorId", DoctorId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRLabResultTabularFormat", hshInput);
            return ds.Tables[0];
        }

        public string SetEMRCloseEncounter(int HospitalLocationId, int EncounterId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCloseEncounter", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getProvisionalAndFinalDiagnosis(int FacilityId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetProvisionalAndFinalDiagnosis", HshIn);
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

        public string SaveEncounterDetail(int HospitalLocationId, int EncounterId, bool IsSurgeryRequired, bool IsAdmissionRequired, bool IsRevisit, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitIsSurgeryRequired", IsSurgeryRequired);
                HshIn.Add("@bitIsAdmissionRequired", IsAdmissionRequired);
                HshIn.Add("@bitIsRevisit", IsRevisit);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveEncounterDetail", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getEncounterDetail(int FacilityId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetEncounterDetail", HshIn);
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

        public DataSet getPatientVisitRequiredReport(int FacilityId, string FromDate, string ToDate, int SurgeryRequired, int AdmissionRequired,
                                                    string EncounterNo, string RegistrationNo)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                if (FromDate != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                }
                if (ToDate != string.Empty)
                {
                    HshIn.Add("@chrToDate", ToDate);
                }

                HshIn.Add("@intSurgeryRequired", SurgeryRequired);
                HshIn.Add("@intAdmissionRequired", AdmissionRequired);
                if (EncounterNo != string.Empty)
                {
                    HshIn.Add("@chvEncounterNo", EncounterNo);
                }
                if (RegistrationNo != string.Empty)
                {
                    HshIn.Add("@chvRegistrationNo", RegistrationNo);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientVisitRequiredReport", HshIn);
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

        public DataSet GetDiagPrintTemplate(int HospId, int FacilityId, int RegistrationId, int EncounterId,
                                               string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                               string sTemplateType, bool bChronologicalOrder, int ReportId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@chvTemplateName", sTemplateName);
                HshIn.Add("@chvTemplateType", sTemplateType);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@bitChronologicalOrder", bChronologicalOrder);
                HshIn.Add("@intReportId", ReportId);

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintTemplate", HshIn);
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

        public string saveFavouriteDoctorProgressNote(int doctorId, string ProgressNote, int userId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable Hshout = new Hashtable();
            try
            {
                HshIn.Add("@intDoctorId", doctorId);
                HshIn.Add("@chvProgressNote", ProgressNote);
                HshIn.Add("@intEncodedBy", userId);

                Hshout.Add("@chvOutputMessage", SqlDbType.VarChar);

                Hshout = objDl.getOutputParametersValues(System.Data.CommandType.StoredProcedure, "uspEMRSaveFavouriteDoctorProgressNote", HshIn, Hshout);
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

            return Hshout["@chvOutputMessage"].ToString();
        }

        public DataTable getFavouriteDoctorProgressNote(int doctorId, string txt)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();

            try
            {
                string strSearchCriteria = "%" + txt.Trim() + "%";

                HshIn.Add("@intDoctorId", doctorId);
                HshIn.Add("@chvSearchCriteria", strSearchCriteria.ToString());

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFavouriteDoctorProgressNote", HshIn);
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

            return ds.Tables[0];
        }


        public bool deleteFavouriteDoctorProgressNote(int doctorId, int favouriteDPNId, int userId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intDoctorId", doctorId);
                HshIn.Add("@intFavouriteDPNId", favouriteDPNId);
                HshIn.Add("@intEncodedBy", userId);

                i = objDl.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "UspEMRDeleteFavouriteDoctorProgressNote", HshIn);
                if (i == 1)
                {
                    return true;
                }
                else
                {
                    return false;
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

        public DataTable getMedicationSetMasterList(int HospId, int FacilityId, int DepartmentId, int DoctorId, string OPIP)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@DoctorId", DoctorId);
                HshIn.Add("@chvOPIP", OPIP);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetMedicationSetMasterList", HshIn);
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
            return ds.Tables[0];
        }

        public DataSet getCompoundedDrugDetails(int FacilityId, int EncounterId, int IndentId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetCompoundedDrugDetails", HshIn);
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

        public DataSet getPendingConsumableIndent(int FacilityId, int IndentId, int EncounterId, int RegistrationId, int WardId, int ClosedStatus,
                                                string FromDate, string ToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intWardId", WardId);
                HshIn.Add("@intClosedStatus", ClosedStatus);

                if (FromDate != string.Empty && ToDate != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetPendingConsumableIndent", HshIn);
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
        public DataSet GetEMRPrintCaseSheetMRdReports(int HospId, int FacilityId, string RegistrationId, string EncounterId,
                                                     string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                                     string sTemplateType, int ReportId, string xmlColumnName)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@chvTemplateName", sTemplateName);
                HshIn.Add("@chvTemplateType", sTemplateType);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@xmlColumnName", xmlColumnName);

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRPrintCaseSheetMRdReports", HshIn);
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


        public DataSet GetEMRPrintCaseSheetMRDFildName(int HospId, int FacilityId, string RegistrationId, string EncounterId,
                                             string sFromDate, string sToDate, string sTemplateName, int TemplateId,
                                             string sTemplateType, int ReportId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                HshIn.Add("@chvTemplateName", sTemplateName);
                HshIn.Add("@chvTemplateType", sTemplateType);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intReportId", ReportId);


                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRPrintCaseSheetMRDFildName", HshIn);
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

        public DataSet GfsQueryCountManagement(int empid)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            DataSet ds = new DataSet();
            try
            {

                hstInput.Add("@empID", empid);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetCountGfsQueryDoc", hstInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
            return ds;
        }

        public DataSet GfsQuery(int QID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            DataSet ds = new DataSet();
            try
            {

                hstInput.Add("@QID", QID);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetGfsQuery", hstInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
            return ds;

        }

        public string GetERtoken(string encounterid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string Id = string.Empty;
            try
            {

                string _text = "select ID from ERtoken with(nolock) where ErEncounterID= '" + encounterid + "'";
                Id = (string)objDl.ExecuteScalar(CommandType.Text, _text);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
            return Id;
        }

        public int CheckPatientProblem(int encounterid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            int PRoblem;
            try
            {
                hstInput.Add("@encounterID", encounterid);
                PRoblem = (int)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspCheckPatientProblem", hstInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
            return PRoblem;
        }

        public void ChangeEncounterDoctor(int encounterID, int doctorID, int userID)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            try
            {
                hstInput.Add("@encounterID", encounterID);
                hstInput.Add("@doctorID", doctorID);
                hstInput.Add("@userID", userID);
                dl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeEncounterDoctor", hstInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                dl = null;
            }
        }

        public string SaveGfsQuery(int Qid, string formID, string Query, int QueryBy, int QueryTo)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@QID", Qid);
                HshIn.Add("@formID", formID);
                HshIn.Add("@Query", Query);
                HshIn.Add("@QueryBy", QueryBy);
                HshIn.Add("@QueryTo", QueryTo);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveGfsQuery", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet SearchAppointmentList(int HospitalLocationId, string DoctorId, int FacilityId, string DateRange, string FromDate, string ToDate, int RegistrationNo, string PatientName,
            string OldRegistrationNo, string EnrolleNo, string MobileNo, string EncounterNo, int LoginFacilityId, int StatusId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chvProviderId", DoctorId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateRange", DateRange);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvName", PatientName);
                HshIn.Add("@chvOldRegistrationNo", OldRegistrationNo);
                HshIn.Add("@cEnrolleNo", EnrolleNo);
                HshIn.Add("@MobileNo", MobileNo);
                // HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intStatusId", StatusId);

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchAppointmentList", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet GetDoctorPatientLists(int HospitalLocationId, int DoctorId, int FacilityId, string DateRange, string FromDate, string ToDate, int RegistrationNo, string PatientName,
            string OldRegistrationNo, string EnrolleNo, string MobileNo, string EncounterNo, int LoginFacilityId, int StatusId, int EMRStatusId, string EncounterType, int SpecialisationId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intProviderId", DoctorId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateRange", DateRange);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvName", PatientName);
                HshIn.Add("@chvOldRegistrationNo", OldRegistrationNo);
                HshIn.Add("@cEnrolleNo", EnrolleNo);
                HshIn.Add("@MobileNo", MobileNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intStatusId", StatusId);
                HshIn.Add("@intemrStatusId", EMRStatusId);
                HshIn.Add("@chrEncounterType", EncounterType);
                HshIn.Add("@intSpecialisationId", SpecialisationId);

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorPatientLists", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet GetGlobalToothNo()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetGlobalToothNo");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
            return ds;
        }

        public DataSet getPAstHistory(int regID, int encounterID, int doctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hstInput = new Hashtable();
            try
            {
                hstInput.Add("@regID", regID);
                hstInput.Add("@encounterID", encounterID);
                hstInput.Add("@doctorID", doctorID);
                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspgetPAstHistory", hstInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
            return ds;
        }

        public DataSet getPAstCheifComplaints(int regID, int encounterID, int doctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable hstInput = new Hashtable();
            try
            {
                hstInput.Add("@regID", regID);
                hstInput.Add("@encounterID", encounterID);
                hstInput.Add("@doctorID", doctorID);
                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspgetPAstCheifComplaints", hstInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
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

        public DataTable getEMRDoctorWisePatientNote(int RegistrationId, int DoctorId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            StringBuilder sb = new StringBuilder();
            try
            {

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intDoctorId", DoctorId);

                sb.Append("select PatientNotes,DoctorId from EMRDoctorWisePatientNote ");
                sb.Append(" WHERE RegistrationId = @intRegistrationId ");
                sb.Append(" AND DoctorId = @intDoctorId ");


                ds = (DataSet)objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
                sb = null;
            }
            return ds.Tables[0];
        }

        public string SaveEMRDoctorWisePatientNote(int RegistrationId, int DoctorId, string PatientNotes, int EncodedBy)
        {

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvPatientNotes", PatientNotes);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveEMRDoctorWisePatientNote", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetEMRDoctorPrescriptionInstructions(int DoctorId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);

                qry.Append("SELECT enc.Id, Instructions");
                qry.Append(" FROM EMRDoctorPrescriptionInstructions enc WITH (NOLOCK) ");
                qry.Append(" INNER JOIN Employee emp WITH (NOLOCK) ON enc.DoctorId = emp.ID");
                qry.Append(" WHERE enc.DoctorId = @intDoctorId AND enc.Active = 1 ");

                ds = (DataSet)objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

        public string SaveEMRDoctorPrescriptionInstructions(int DoctorId, string Instructions, int EncodedBy)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            string Retqry = "";
            try
            {
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvInstructions", Instructions);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRDoctorPrescriptionInstructions", HshIn, HshOut);
                Retqry = HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception Ex)
            {
                Retqry = Ex.Message;
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                HshOut = null;
                Retqry = null;
            }
            return Retqry;
        }

        public DataSet getFavoriteDrugWithStockWithFavoriteId(int FavoriteId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@FavoriteId", FavoriteId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetFavoriteDrugWithStockWithFavoriteId", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
            }
            return ds;
        }

        public string DeleteFavoriteDrugsWithFavoriteId(int FavoriteId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@FavoriteId", FavoriteId);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteFavDrugsWithFavoriteId", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetPACDashboard(int HospitalLocationId, int FacilityId, int DoctorId, int EncodedBy,
                        int RegistrationNo, string MobileNo, string PatientName, int PACStatus, string FromDate, string ToDate)
        {
            Hashtable hstInput = new Hashtable();
            Hashtable hstOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("intHospitalLocationId", HospitalLocationId);
            hstInput.Add("intFacilityId", FacilityId);
            hstInput.Add("intDoctorId", DoctorId);
            hstInput.Add("intEncodedBy", EncodedBy);
            hstInput.Add("chvRegistrationNo", RegistrationNo);
            hstInput.Add("chvMobileNo", MobileNo);
            hstInput.Add("chvPatientName", PatientName);
            hstInput.Add("PACStatus", PACStatus);
            hstInput.Add("FromDate", FromDate);
            hstInput.Add("ToDate", ToDate);
            hstOut.Add("chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPACDashboard", hstInput, hstOut);

        }

        public DataSet getFindPatientMasterList(int HospId, int FacilityId, int UserId)
        {
            DataSet objDs = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intUserId", UserId);

                objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFindPatientMasterList", HshIn);
            }
            catch (Exception)
            {
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }

            return objDs;
        }

        public DataSet GetEMRGetPatientAccessRights(int HospId, int FacilityId, int SpecializationId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {
                hsIn.Add("@inyHospitalLocationId", HospId);
                hsIn.Add("@iUserId", UserId);
                hsIn.Add("@intSpecializationId", SpecializationId);
                hsIn.Add("@intFacilityId", FacilityId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientAccessRights", hsIn);
            }
            catch
            {
            }
            finally
            {
                objDl = null;
                hsIn = null;
            }

            return ds;
        }

        public bool EMRSaveDisplayCurrentPatient(int iHospitalLocationId, int iFacilityId, int intEncounterId,
                       int intAppointmentResourceId, string chvIPAddress, string sFlag, string sDoctorName, string sDeptName, string sRegistrationNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            int i = 0;
            hsIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            hsIn.Add("@intFacilityId", iFacilityId);
            hsIn.Add("@intEncounterId", intEncounterId);
            hsIn.Add("@intAppointmentResourceId", intAppointmentResourceId);
            hsIn.Add("@chvIPAddress", chvIPAddress);
            hsIn.Add("@chvQMSFlag", sFlag);
            hsIn.Add("@chvDoctorName", sDoctorName);
            hsIn.Add("@chvDeptName", sDeptName);
            hsIn.Add("@chvRegistrationNo", sRegistrationNo);
            hsIn.Add("@intSource", 1);

            i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveDisplayCurrentPatient", hsIn);
            if (i.ToString() == "1")
                return true;
            else
                return false;
        }

        public DataSet getEmployeeWithResource(int FacilityId, int SpecializationId, int EncodedBy)
        {
            DataSet objDs = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intSpecializationId", SpecializationId);
                HshIn.Add("@iUserId", EncodedBy);

                objDs = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetEmployeeWithResource", HshIn);
            }
            catch (Exception)
            {
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }

            return objDs;
        }

        public DataSet GetDoctorPatientListsV3(int HospitalLocationId, int DoctorId, int FacilityId, string DateRange, string FromDate, string ToDate, int RegistrationNo, string PatientName,
           string OldRegistrationNo, string EnrolleNo, string MobileNo, string EncounterNo, int LoginFacilityId, int StatusId, int EMRStatusId, string EncounterType, int SpecialisationId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intProviderId", DoctorId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateRange", DateRange);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvName", PatientName);
                HshIn.Add("@chvOldRegistrationNo", OldRegistrationNo);
                HshIn.Add("@cEnrolleNo", EnrolleNo);
                HshIn.Add("@MobileNo", MobileNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intStatusId", StatusId);
                HshIn.Add("@intemrStatusId", EMRStatusId);
                HshIn.Add("@chrEncounterType", EncounterType);
                HshIn.Add("@intSpecialisationId", SpecialisationId);

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorPatientListsV3", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
            return ds;
        }
        public DataSet GetPOCServiceOrderDetails(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId,
         Int32 iBillId, Int32 iDepartmentId, string chrDepartmentType)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("RegId", iRegistrationId);
                HshIn.Add("EncounterId", iEncounterId);
                HshIn.Add("intDepartmentId", iDepartmentId);
                HshIn.Add("chrDepartmentType", chrDepartmentType);
                if (iBillId > 0)
                {
                    HshIn.Add("BillId", iBillId);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPOCServiceOrderDetails", HshIn);
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
                            sb.Append(dtString.Rows[i]["FrequencyName"].ToString() != string.Empty ? " " + dtString.Rows[i]["FrequencyName"].ToString() + " , " : " ");
                            sb.Append(dtString.Rows[i]["FoodRelationship"].ToString() != string.Empty ? " " + dtString.Rows[i]["FoodRelationship"].ToString() : " ");
                            if (dtString.Rows[i]["Duration"].ToString().Trim() != "0")
                            {
                                sb.Append(" for " + dtString.Rows[i]["DurationText"].ToString() + " ");
                            }


                            if (dtString.Rows[i]["RouteName"].ToString().Trim() != string.Empty)
                            {
                                sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                            }


                            if (dtString.Rows[i]["Instructions"].ToString().Trim() != string.Empty)
                            {
                                sb.Append(Environment.NewLine + "INSTRUCTIONS : " + Environment.NewLine + dtString.Rows[i]["Instructions"].ToString().Trim().ToUpper());
                            }
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


                            if (dtString.Rows[i]["RouteName"].ToString().Trim() != string.Empty)
                            {
                                sb.Append(dtString.Rows[i]["RouteName"].ToString() != string.Empty ? " (" + dtString.Rows[i]["RouteName"].ToString() + ")" : string.Empty);
                            }


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

        public DataSet GetEmrPatientDetail(int iHospID, int iFacilityId, int iRegistrationId, int RegistrationNo, int iEncounterId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationId", iHospID);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("chvRegistrationNo", RegistrationNo);
            HshIn.Add("intEncounterId", iEncounterId);
            HshIn.Add("intEncodedBy", EncodedBy);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEmrPatientDetail", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelIndentFromWardPreviousMedicine(int IndentId, string Remarks, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@IndentId", IndentId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chvRemarks", Remarks);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelIndentFromWardPreviousMedicine", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public string SaveFormat(int FormatId, int FacilityId,
                                  string FormatName, string FormatText, int Active,
                                   int EncodedBy, string chvErrorStatus)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFormatId", FormatId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvFormatName", FormatName);
                HshIn.Add("@chvFormatText", FormatText);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitActive", Active);
                //HshIn.Add("@intEncodedDate", EncodedDate);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveProgressNoteFormat", HshIn, HshOut);
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
            return HshOut["@chvErrorStatus"].ToString();

        }

        public DataSet GetProgressNoteFormat(int iFacilityId, int FormatId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@intFormatId", FormatId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetProgressNoteFormat", HshIn);
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

        public DataSet IsUniqueProgressNoteFormat(int iFacilityId, int FormatId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@intFormatId", FormatId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetProgressNoteFormat", HshIn);
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
        public DataSet inActiveEMRProgressNoteFormat(int FormatId, int EncodedBy)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFormatId", FormatId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRUpdateProgressNoteFormat", HshIn);
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

        public DataSet getFormatList(int iFacilityId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", iFacilityId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRBindProgressNoteFormat", HshIn);
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

        public string checkDrugType(int ItemId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string ItemType = "";
            try
            {
                HshIn.Add("@ItemId", ItemId);

                ItemType = (string)objDl.ExecuteScalar(CommandType.StoredProcedure, "UspEMRCheckItemType", HshIn);


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
            return ItemType;
        }

        public DataSet getDischargeSummary(int EnconterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();

            try
            {
                HshIn.Add("@EnconterId", EnconterId);

                qry.Append(" select distinct psd.IsNewDischargeSummary, psd.encounterid from EMRPatientSummaryDetails psd with (nolock)");
                qry.Append(" inner join EMRPatientNotesData pnd WITH (NOLOCK) ON psd.encounterId = pnd.encounterid");
                qry.Append(" Where psd.encounterid = @EnconterId");

                ds = objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                qry = null;
            }
            return ds;
        }

    }



}

