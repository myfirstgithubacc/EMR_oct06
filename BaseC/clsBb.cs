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
    public partial class clsBb
    {
        private string sConString = "";

        public clsBb(string conString)
        {
            sConString = conString;
        }

        Hashtable HshIn;
        Hashtable HshOut;


        public string SaveBloodGroupMaster(int BloodGroupId, int HospitalLocationId, int FacilityId, string BloodGroup,
                                string BloodGroupDescription, string RhType, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intBloodGroupId", BloodGroupId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBloodGroup", BloodGroup.Trim());
            HshIn.Add("@chvBloodGroupDescription", BloodGroupDescription.Trim());
            HshIn.Add("@chrRhType", RhType);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBloodGroupMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBloodGroupMaster(int BloodGroupId, int HospitalLocationId, int FacilityId, int Active)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intBloodGroupId", BloodGroupId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodGroupMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveDonationDonorTypeMaster(int DonationDonorTypeId, int HospitalLocationId, int FacilityId, int DonorTypeId,
                                    int DonationTypeId, string DonorTypeDesc, int DonationRepeatDays, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDonationDonorTypeId", DonationDonorTypeId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intDonorTypeId", DonorTypeId);
            HshIn.Add("@intDonationTypeId", DonationTypeId);
            HshIn.Add("@chvDonorTypeDesc", DonorTypeDesc);
            HshIn.Add("@intDonationRepeatDays", DonationRepeatDays);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveDonationDonorTypeMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDonationDonorTypeMaster(int DonationDonorTypeId, int DonationTypeId, int DonorTypeId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {


                HshIn.Add("@intDonationDonorTypeId", DonationDonorTypeId);
                HshIn.Add("@intDonationTypeId", DonationTypeId);
                HshIn.Add("@intDonorTypeId", DonorTypeId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonationDonorTypeMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }





        public string SaveDonationTypeMaster(int DonationTypeId, int HospitalLocationId, int FacilityId, string Prefix, string DonationType,
                         int MinAgeLimit, int MaxAgeLimit, decimal MinWeightLimit, decimal MaxWeightLimit, string UsedFor, int Issuereq, int IsProcedureRequired, int Active, int UserId, int DonorSeriesTypeID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDonationTypeId", DonationTypeId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvPrefix", Prefix.Trim());
            HshIn.Add("@chvDonationType", DonationType.Trim());
            HshIn.Add("@intMinAgeLimit", MinAgeLimit);
            HshIn.Add("@intMaxAgeLimit", MaxAgeLimit);
            HshIn.Add("@monMinWeight", MinWeightLimit);
            HshIn.Add("@monMaxWeight", MaxWeightLimit);
            HshIn.Add("@chvUsedFor", UsedFor);
            HshIn.Add("@bitIsScreenigRequired", Issuereq);
            HshIn.Add("@bitIsProcedureRequired", IsProcedureRequired);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intDonorSeriesTypeID", DonorSeriesTypeID);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveDonationTypeMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDonationTypeMaster(int HospitalLocationId, int FacilityId, int DonationTypeId, int IsProcedureRequired, int Active)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonationTypeId", DonationTypeId);
                HshIn.Add("@bitIsProcedureRequired", IsProcedureRequired);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonationTypeMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDonationTypeMaster(int HospitalLocationId, int FacilityId, int DonationTypeId, int IsProcedureRequired, int Active, string BBDonorSeriesID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {


                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonationTypeId", DonationTypeId);
                HshIn.Add("@bitIsProcedureRequired", IsProcedureRequired);
                HshIn.Add("@intBBDonorSeriesID", BBDonorSeriesID);
                HshIn.Add("@bitActive", Active);



                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonationTypeMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }
        public string SaveBloodComponentMaster(int ComponentId, int HospitalLocationId, int FacilityId,
                                string ComponentShortName, string ComponentName, int ExpiryDays, double DefaultML,
                                int IsProcedure, int IsCrossMatchRequired, int CrossMatchServiceId, int IsBillingRequired, int ComponentIssueServiceid,
                                int Active, int UserId, string ParentType, int ParentId, int NearExpiryDays
            , string LabelHeader, string LabelSubHeader, string LabelHeaderInstructions, string LabelFooter)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intComponentId", ComponentId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvComponentShortName", ComponentShortName);
            HshIn.Add("@chvComponentName", ComponentName);
            HshIn.Add("@intExpiryDays", ExpiryDays);
            HshIn.Add("@monDefaultML", DefaultML);
            HshIn.Add("@bitIsProcedure", IsProcedure);
            HshIn.Add("@bitIsCrossMatchRequired", IsCrossMatchRequired);
            HshIn.Add("@intCrossMatchServiceId", CrossMatchServiceId);
            HshIn.Add("@bitIsBillingRequired", IsBillingRequired);
            HshIn.Add("@intComponentIssueServiceid", ComponentIssueServiceid);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshIn.Add("@chrComponentType", ParentType);
            HshIn.Add("@intParentId", ParentId);
            HshIn.Add("@NearExpiryDays", NearExpiryDays);


            HshIn.Add("@LabelHeader", LabelHeader);
            HshIn.Add("@LabelSubHeader", LabelSubHeader);
            HshIn.Add("@LabelHeaderInstructions", LabelHeaderInstructions);
            HshIn.Add("@LabelFooter", LabelFooter);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBloodComponentMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveBloodStock(int Stock_id, int HospitalLocationId, int FacilityId, string GroupCode, string CollectionDate, string ParentBagId, string XmlChildBagDetails, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intStockId", Stock_id);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intGroupCode", GroupCode);
            HshIn.Add("@dtCollectionDate", CollectionDate);
            HshIn.Add("@intParentBagId", ParentBagId);
            HshIn.Add("@xmlChildBagDetails", XmlChildBagDetails);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBloodStock", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetBloodComponentMaster(int ComponentId, int HospitalLocationId, int FacilityId, int Active)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {


                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);



                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodComponentMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBloodComponentMaster(int ComponentId, int HospitalLocationId, int FacilityId, int Active, string ComponentType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrComponentType", ComponentType);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodComponentMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetChildBloodComponentForBloodStock(int ComponentId, int HospitalLocationId, int FacilityId, int Active)
        {
            return GetChildBloodComponentForBloodStock(ComponentId, HospitalLocationId, FacilityId, Active, false);
        }

        public DataSet GetChildBloodComponentForBloodStock(int ComponentId, int HospitalLocationId, int FacilityId, int Active, bool IsComponentToComponentConversion)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@bitIsComponentToComponentConversion", IsComponentToComponentConversion);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetChildBloodComponentForBloodStock", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet getBagFromBloodStock(int HospitalLocationId, int FacilityId, int Active, int StockId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStockId", StockId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagFromBloodStock", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveCrossMatchMethodMaster(int MethodId, int HospitalLocationId, int FacilityId,
                                string MethodName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intMethodId", MethodId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvMethodName", MethodName);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveCrossMatchMethodMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCrossMatchMethodMaster(int ComponentId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intMethodId", ComponentId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchMethodMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCampMaster(int CampId, int HospitalLocationId, int FacilityId,
                                string CampName, DateTime CampDate, int CountryId, int StateId, int CityId, int AreaId, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intCampId", CampId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvCampName", CampName);

            HshIn.Add("@dtCampDate", CampDate);
            HshIn.Add("@intCountryId", CountryId);
            HshIn.Add("@intStateId", StateId);
            HshIn.Add("@intCityId", CityId);
            HshIn.Add("@intAreaId", AreaId);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveCampMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCampMaster(int CampId, int HospitalLocationId, int FacilityId, int Active)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@intCampId", CampId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCampMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveBagMaster(int BagId, int HospitalLocationId, int FacilityId,
                                string BagName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intBagId", BagId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBagName", BagName);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBagMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBagMaster(int ComponentId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intBagId", ComponentId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetReferredCounselingDetails(int CounselingCenterId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intReferredCounselingCenterId", CounselingCenterId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetReferredCounselingDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveBagPremixedTypeMaster(int PremixedTypeId, int HospitalLocationId, int FacilityId,
                               string PremixedTypeName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intPremixedTypeId", PremixedTypeId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvPremixedTypeName", PremixedTypeName);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBagPremixedTypeMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBagPremixedTypeMaster(int PremixedTypeId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intPremixedTypeId", PremixedTypeId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagPremixedTypeMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveBagPremixedMaster(int PremixedId, int HospitalLocationId, int FacilityId, int PremixedTypeId, string PremixedName,
           decimal DefaultQty, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intPremixedId", PremixedId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intPremixedTypeId", PremixedTypeId);
            HshIn.Add("@chvPremixedName", PremixedName);
            HshIn.Add("@decDefaultQty", DefaultQty);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBagPremixedMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBagPremixedMaster(int PremixedId, int PremixedTypeId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intPremixedId", PremixedId);
                HshIn.Add("@intPremixedTypeId", PremixedTypeId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagPremixedMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveIndicationMaster(int IndicationId, int HospitalLocationId, int FacilityId,
                              string IndicationDescription, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intIndicationId", IndicationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvIndicationDescription", IndicationDescription);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveIndicationMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetIndicationMaster(int IndicationId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();


            try
            {

                HshIn.Add("@intIndicationId", IndicationId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetIndicationMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveDonationProcedureTypeMaster(int DonationProcedureTypeId, int HospitalLocationId, int FacilityId, string ProcedureTypeName,
                                   int ProcedureTypeId, int DonationTypeId, int DonorTypeId, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDonationProcedureTypeId", DonationProcedureTypeId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvProcedureTypeName", ProcedureTypeName);
            HshIn.Add("@intProcedureTypeId", ProcedureTypeId);
            HshIn.Add("@intDonationTypeId", DonationTypeId);
            HshIn.Add("@intDonorTypeId", DonorTypeId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveDonationProcedureTypeMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDonationProcedureTypeMaster(int DonationProcedureTypeId, int DonationTypeId, int DonorTypeId, int ProcedureTypeId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intDonationProcedureTypeId", DonationProcedureTypeId);
                HshIn.Add("@intDonationTypeId", DonationTypeId);
                HshIn.Add("@intDonorTypeId", DonorTypeId);
                HshIn.Add("@intProcedureTypeId", ProcedureTypeId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonationProcedureTypeMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveDocumentNo(int HospitalLocationId, int FacilityId, int DonationTypeId,
                    int IsYearPrefixRequired, int IsMonthPrefixRequired, string PrefixSeparator,
                    DateTime FromDate, DateTime ToDate, string xmlDocumentNo, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@inyDonationTypeId", DonationTypeId);
            HshIn.Add("@bitIsYearPrefixRequired", IsYearPrefixRequired);
            HshIn.Add("@bitIsMonthPrefixRequired", IsMonthPrefixRequired);
            HshIn.Add("@chrPrefixSeparator", PrefixSeparator);
            HshIn.Add("@dtFromDate", FromDate);
            HshIn.Add("@dtToDate", ToDate);
            HshIn.Add("@xmlDocumentNo", xmlDocumentNo);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveDocumentNo", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDocumentSetup(int DonationTypeId, DateTime fromDate, DateTime toDate, int FacilityId, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {


                HshIn.Add("@intDonationTypeId", DonationTypeId);
                HshIn.Add("@fromDate", fromDate.ToString("yyyy-MM-dd"));
                HshIn.Add("@toDate", toDate.ToString("yyyy-MM-dd"));
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@HospId", HospId);

                string qry = "SELECT dn.DocId, dn.DocumentTypeId, sm.[Status] AS DocumentType, dn.Prefix, dn.DocumentNo, dn.Active " +
                    " FROM BBDocumentNo dn WITH (NOLOCK) " +
                    " INNER JOIN StatusMaster sm WITH (NOLOCK) ON dn.DocumentTypeId = sm.StatusId AND sm.StatusType = 'BBDocumentType' " +
                    " WHERE dn.DonationTypeId = @intDonationTypeId " +
                    " AND CONVERT(DATE, GETDATE()) BETWEEN @fromDate AND @toDate " +
                    " AND dn.FacilityId = @FacilityId " +
                    " AND dn.HospitalLocationID = @HospId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetBloodBankServices(string DSType, int Active, int HospitalLocationId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {

                HshIn.Add("@DSType", DSType);
                HshIn.Add("@Active", Active);
                HshIn.Add("@HospitalLocationId", HospitalLocationId);

                string qry = "SELECT ios.ServiceId, ios.ServiceName" +
                        " FROM ItemOfService ios WITH (NOLOCK) " +
                        " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ios.SubDeptId  = ds.SubDeptId" +
                        " INNER JOIN DepartmentMain dm WITH (NOLOCK) on ds.DepartmentID=dm.DepartmentID" +
                        " WHERE dm.ShortName = @DSType AND ios.Active = @Active AND ios.HospitalLocationId = @HospitalLocationId" +
                        " ORDER BY ios.ServiceName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetStatusMaster(string StatusType)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@StatusType", StatusType);

                StringBuilder qry = new StringBuilder();

                qry.Append("SELECT StatusId, Status, StatusColor, Code FROM StatusMaster WITH (NOLOCK) ");
                qry.Append(" WHERE StatusType = @StatusType AND Active = 1 ");
                qry.Append(" ORDER BY SequenceNo ");

                return objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDonorList(int DonationTypeId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@DonationTypeId", DonationTypeId);

                StringBuilder qry = new StringBuilder();

                qry.Append("SELECT DonorTypeId,DonorTypeDesc FROM BBDonationDonorTypeMaster WITH (NOLOCK) ");
                qry.Append(" WHERE DonationTypeId = @DonationTypeId AND Active = 1 ");
                qry.Append(" ORDER BY Sequence ");

                return objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetOccupationMaster(int Active)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@Active", Active);
                string qry = "select id,name from OccupationMaster WITH (NOLOCK) where Active=@Active";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetKinRelationMaster(int Active)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@Active", Active);
                string qry = "select kinId, KinName from KinRelation WITH (NOLOCK) where Active=@Active";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        public string UpdateReleaseAcknowledge(int ReleaseID, int ReleaseAcknowledged, int ReleaseAcknowledgedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@intReleaseID", ReleaseID);
                HshIn.Add("@intReleaseAcknowledged", ReleaseAcknowledged);
                HshIn.Add("@intReleaseAcknowledgedBy", ReleaseAcknowledgedBy);
                string qry = "update BBRequestReleaseMain set ReleaseAcknowledged=@intReleaseAcknowledged, ReleaseAcknowledgedBy=@intReleaseAcknowledgedBy where ReleaseID=@intReleaseID";
                objDl.ExecuteNonQuery(CommandType.Text, qry, HshIn);
                return "Record Updated Successfully ";

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        //new procedure for test
        public string UpdateReleaseAcknowledge(int ReleaseID, int ReleaseAcknowledged, int ReleaseAcknowledgedBy, string strXML)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {

                HshIn.Add("@intReleaseID", ReleaseID);
                HshIn.Add("@intReleaseAcknowledged", ReleaseAcknowledged);
                HshIn.Add("@intReleaseAcknowledgedBy", ReleaseAcknowledgedBy);
                HshIn.Add("@strXML", strXML);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBUpdateReleaseAcknowledge", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        public Hashtable SaveDonorRegistrationMaster(int DonorRegistrationId, int HospitalLocationId, int FacilityId, string ManualDonorRegistrationNo, DateTime DonorRegistrationDate
                    , int DonationTypeId, int DonationDonorTypeId, int ProcedureTypeId, string OPIPExternal, int WhomDonatedEncounterId
                    , string WhomDonatedEncounterNo, int WhomDonatedRegistrationId, string WhomDonatedRegistrationNo
                    , int WhomDonatedRelationshipId, int TitleId, string FirstName, string MiddleName, string LastName
                    , int RelationshipTypeStatusId, string GuardianName, string DOBorAge, DateTime DOB, int AgeYear, int AgeMonth
                    , int AgeDay, int Gender, int MaritalStatus, string EducationDetails, decimal Height, decimal Weight, int BloodGroupId
                    , int OccupationId, int ReligionId, int NationalityId, string Address1, string Address2, string Address3
                    , int CountryId, int StateId, int CityId, int AreaId, string PinCode, string EmergencyPhoneNo
                    , string ResidencePhoneNo, string OfficePhoneNo, string MobileNo, string Fax, string Email, int IsDonateAgain
                    , int IsToBeVoluantryDonor, int IsExistingDonor, int LastDonorRegistrationId, string LastDonorRegistrationNo
                    , DateTime LastDonorRegistrationDate, string Remarks, string Diagnosis, int Active, int UserId, string OldRegNo
                    , int RepeatDonor, int DonationQty, string Academics, int ReferFrom, string DonationPrePost, string UHID, string ForWhomeRegNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);

            HshIn.Add("@chvManualDonorRegistrationNo", ManualDonorRegistrationNo);
            HshIn.Add("@dtDonorRegistrationDate", DonorRegistrationDate);

            HshIn.Add("@inyDonationTypeId", DonationTypeId);
            HshIn.Add("@inyDonationDonorTypeId", DonationDonorTypeId);
            HshIn.Add("@inyProcedureTypeId", ProcedureTypeId);

            HshIn.Add("@chrOPIPExternal", OPIPExternal);

            HshIn.Add("@intWhomDonatedEncounterId", WhomDonatedEncounterId);
            HshIn.Add("@chvWhomDonatedEncounterNo", WhomDonatedEncounterNo);
            HshIn.Add("@intWhomDonatedRegistrationId", WhomDonatedRegistrationId);
            HshIn.Add("@chvWhomDonatedRegistrationNo", WhomDonatedRegistrationNo);
            HshIn.Add("@intWhomDonatedRelationshipId", WhomDonatedRelationshipId);


            HshIn.Add("@intTitleId", TitleId);
            HshIn.Add("@chvFirstName", FirstName);
            HshIn.Add("@chvMiddleName", MiddleName);
            HshIn.Add("@chvLastName", LastName);
            HshIn.Add("@intRelationshipTypeStatusId", RelationshipTypeStatusId);
            HshIn.Add("@chvGuardianName", GuardianName);

            HshIn.Add("@chrDOBorAge", DOBorAge);
            HshIn.Add("@dtDOB", DOB);
            HshIn.Add("@inyAgeYear", AgeYear);
            HshIn.Add("@inyAgeMonth", AgeMonth);
            HshIn.Add("@inyAgeDay", AgeDay);

            HshIn.Add("@inyGender", Gender);
            HshIn.Add("@inyMaritalStatus", MaritalStatus);
            HshIn.Add("@chvEducationDetails", EducationDetails);

            HshIn.Add("@monHeight", Height);
            HshIn.Add("@monWeight", Weight);

            HshIn.Add("@intBloodGroupId", BloodGroupId);
            HshIn.Add("@intOccupationId", OccupationId);
            HshIn.Add("@inyReligionId", ReligionId);
            HshIn.Add("@intNationalityId", NationalityId);

            HshIn.Add("@chvAddress1", Address1);
            HshIn.Add("@chvAddress2", Address2);
            HshIn.Add("@chvAddress3", Address3);
            HshIn.Add("@intCountryId", CountryId);
            HshIn.Add("@intStateId", StateId);
            HshIn.Add("@intCityId", CityId);
            HshIn.Add("@intAreaId", AreaId);
            HshIn.Add("@chvPinCode", PinCode);

            HshIn.Add("@chvEmergencyPhoneNo", EmergencyPhoneNo);
            HshIn.Add("@chvResidencePhoneNo", ResidencePhoneNo);
            HshIn.Add("@chvOfficePhoneNo", OfficePhoneNo);
            HshIn.Add("@chvMobileNo", MobileNo);
            HshIn.Add("@chvFax", Fax);
            HshIn.Add("@chvEmail", Email);

            HshIn.Add("@bitIsDonateAgain", IsDonateAgain);
            HshIn.Add("@bitIsToBeVoluantryDonor", IsToBeVoluantryDonor);
            HshIn.Add("@bitIsExistingDonor", IsExistingDonor);
            HshIn.Add("@intLastDonorRegistrationId", LastDonorRegistrationId);
            HshIn.Add("@chvLastDonorRegistrationNo", LastDonorRegistrationNo);
            HshIn.Add("@dtLastDonorRegistrationDate", LastDonorRegistrationDate);

            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@chvDiagnosis", Diagnosis);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvDonorRegNo", SqlDbType.VarChar);
            HshIn.Add("@chvOldDonorRegNo", OldRegNo);
            HshOut.Add("@intDonorRegisId", SqlDbType.VarChar);
            HshIn.Add("@RepeatDonor", RepeatDonor);
            HshIn.Add("@DonationQty", DonationQty);
            HshIn.Add("@Academics", Academics);
            HshIn.Add("@ReferFrom", ReferFrom);

            HshIn.Add("@DonationPrePost", DonationPrePost);
            HshIn.Add("@txtUHID", UHID);
            HshIn.Add("@ForWhomeRegNo", ForWhomeRegNo);



            //DonorRegId = HshOut["@intDonorRegId"].ToString();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveDonorRegistration", HshIn, HshOut);

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDonorRegistrationList(int HospitalLocationId, int FacilityId, int DonorRegistrationId, int Active, string StartDate, string EndDate, string ForWhomeRegNo, int DonationType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@ForWhomeRegNo", ForWhomeRegNo);
                HshIn.Add("@intDonationType", DonationType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonorRegistrationList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDonorRegistrationList(int HospitalLocationId, int FacilityId, int DonorRegistrationId, int Active, string StartDate, string EndDate, string ForWhomeRegNo, int DonationType,
            int DonorType, int BloodGroup, int Gender, int Age1, int Age2, string MobileNo, string Name)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@ForWhomeRegNo", ForWhomeRegNo);
                HshIn.Add("@intDonationType", DonationType);
                HshIn.Add("@intDonorType", DonorType);
                HshIn.Add("@intBloodGroup", BloodGroup);
                HshIn.Add("@intGender", Gender);
                HshIn.Add("@intAge1", Age1);
                HshIn.Add("@intAge2", Age2);
                HshIn.Add("@MobileNo", MobileNo);
                HshIn.Add("@Name", Name);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonorRegistrationList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDonorExaminationList(int HospitalLocationId, int FacilityId, int DonorRegistrationId, int Active, string StartDate, string EndDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "BBGetExaminationList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDonorRegistration(int HospitalLocationId, int FacilityId, int DonorRegistrationId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonorRegistration", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        //public string SaveComponentBloodGroupIssueValidation(int BagId, int HospitalLocationId, int FacilityId,
        //                        string BagName, int Active, int UserId)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    HshIn.Add("@intBagId", BagId);
        //    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@chvBagName", BagName);

        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@intEncodedBy", UserId);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBagMaster", HshIn, HshOut);

        //    return HshOut["@chvErrorStatus"].ToString();
        //}

        //public DataSet GetComponentBloodGroupIssueValidation(int HospitalLocationId, int FacilityId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();

        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentBloodGroupIssueValidation", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        public DataSet GetBloodGroupList(int OrderedBloodGroupId, int OrderedComponentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@intOrderedBloodGroupId", OrderedBloodGroupId);
                HshIn.Add("@intOrderedComponentId", OrderedComponentId);


                //string qry = "SELECT bgm.BloodGroupId, bgm.BloodGroupDescription,(CASE WHEN ISNULL(iv.IssueBloodGroupId, 0) = 0 THEN 0 ELSE 1 END) IsChk " +
                //"FROM BBBloodGroupMaster bgm LEFT OUTER JOIN BBComponentBloodGroupIssueValidation iv on bgm.BloodGroupId = iv.IssueBloodGroupId AND "
                //+ "iv.ValidationId = 1 AND iv.Active =1 WHERE bgm.Active = 1";

                string qry = "SELECT bgm.BloodGroupId, bgm.BloodGroupDescription,(CASE WHEN ISNULL(iv.IssueBloodGroupId, 0) = 0 THEN 0 ELSE 1 END) IsChk"
                    + " FROM BBBloodGroupMaster bgm WITH (NOLOCK) LEFT OUTER JOIN BBComponentBloodGroupIssueValidation iv WITH (NOLOCK) on bgm.BloodGroupId = iv.IssueBloodGroupId AND iv.Active =1"
                    + " and iv.OrderedBloodGroupId=@intOrderedBloodGroupId and iv.OrderedComponentId=@intOrderedComponentId WHERE bgm.Active = 1";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }


        public string SaveComponentBloodGroupIssueValidation(int HospitalLocationId, int FacilityId, int OrderedComponentId,
                                    int OrderedBloodGroupId, string IssueBloodGroupIds, string CrossMatchMessage
                                    , int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intOrderedComponentId", OrderedComponentId);
            HshIn.Add("@intOrderedBloodGroupId", OrderedBloodGroupId);
            HshIn.Add("@xmlIssueBloodGroupIds", IssueBloodGroupIds);
            HshIn.Add("@chvCrossMatchMessage", CrossMatchMessage);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentBloodGroupIssueValidation", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentBloodGroupIssueValidation(int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentBloodGroupIssueValidation", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }



        public string SaveComponentCombinationMaster(int ComponentCombinationId, int HospitalLocationId, int FacilityId, string ComponentCombinationDescription, string ComponentIds,
                                 int IsDescriptionAutoGenerate, int Active, int UserId, int Sequence)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvComponentCombinationDescription", ComponentCombinationDescription);
            HshIn.Add("@xmlComponentIds", ComponentIds);
            HshIn.Add("@bitIsDescriptionAutoGenerate", IsDescriptionAutoGenerate);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intSequenceNo", Sequence);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentCombinationMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveComponentRequestReleaseMain(int ReleaseID, int HospitalLocationId, int FacilityId, int RequestNo, int UserId, string ReleaseDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intReleaseID", ReleaseID);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestNo", RequestNo);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlChildReleaseDetails", ReleaseDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@intSavedReleaseID", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveRequestReleaseMain", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveComponentRequestReleaseMain(int ReleaseID, int HospitalLocationId, int FacilityId, int RequestNo, int UserId, string ReleaseDetails, ref int Releaseid)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intReleaseID", ReleaseID);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestNo", RequestNo);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlChildReleaseDetails", ReleaseDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@intSavedReleaseID", SqlDbType.VarChar);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveRequestReleaseMain", HshIn, HshOut);
                Releaseid = Convert.ToInt32(HshOut["@intSavedReleaseID"].ToString());

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetComponentCombinationMaster(int HospitalLocationId, int FacilityId, int ComponentCombinationId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCombinationMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetReturnRequisitionDetailsForWard(int HospitalLocationId, int FacilityId, int RequestNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequestNo", RequestNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetReturnRequisitionDetailsForWard", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComponentCombinationDescription()
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCombinationDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveComponentCombinationIndicationTagging(int ComponentCombinationId, string IndicationIds,
                                int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
            HshIn.Add("@xmlIndicationIds", IndicationIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentCombinationIndicationTagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentCombinationIndicationTagging(int ComponentCombinationId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCombinationIndicationTagging", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveComponentRequestionQuestionMaster(int QuestionId, int HospitalLocationId, int FacilityId, string QuestionDescription, string Gender, int IsDefault,
                                int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intQuestionId", QuestionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvQuestionDescription", QuestionDescription);
            HshIn.Add("@chvGender", Gender);
            HshIn.Add("@bitIsDefault", IsDefault);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentRequestionQuestionMaster", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetComponentRequestionQuestionMaster(int HospitalLocationId, int FacilityId, int QuestionId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intQuestionId", QuestionId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequestionQuestionMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveOpeningStickPosting(int HospitalLocationId, int FacilityId, string xmlStockId, int EncodedBy)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@xmlStockId", xmlStockId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOpeningStockPosting", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return HshOut;
        }

        public DataSet GetBagMaster()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                string qry = "SELECT BagId, BagName from BBBagMaster WITH (NOLOCK) where active=1 order by sequence";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetParameterAndValuesMaster(int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                string qry = "select Parameter_Code, Parameter_Name from bbparametermaster WITH (NOLOCK) where Parameter_Type='W' and status='A' and facilityid=@intFacilityId and hospitallocationid=@inyHospitalLocationId ";
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "ParameterName";
                qry = @"select parameter_values_id, Parameter_Code,Parameter_Values from bbparametervalues WITH (NOLOCK) WHERE Parameter_Code IN
                                (select Parameter_Code from bbparametermaster WITH (NOLOCK) where Parameter_Type='W')
                                and status='A'";
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                ds.Tables.Add(dt);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return ds;
        }


        public DataSet GetBloodCenterWorkUp()
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                string qry = " SELECT COLUMN_NAME, ORDINAL_POSITION FROM INFORMATION_SCHEMA.COLUMNS WITH (NOLOCK) WHERE table_name = 'BBAdverseTransfusionWorkoutBloodBank' and ORDINAL_POSITION>=3 and ORDINAL_POSITION<=18";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }




        public String SaveBagDetails(int DetailsId, int HospitalLocationId, int FacilityId, int BagId, string BatchNo, int BagTypeId,
           DateTime ManufactureDate, DateTime ExpiryDate, int Qty, int BagCapacity, int Active, int UserId, int Anticoagulant)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDetailsId", DetailsId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intBagId", BagId);
            HshIn.Add("@chvBatchNo", BatchNo);
            HshIn.Add("@intBagTypeId", BagTypeId);
            HshIn.Add("@dtManufactureDate", ManufactureDate);
            HshIn.Add("@dtExpiryDate", ExpiryDate);
            HshIn.Add("@intQty", Qty);
            HshIn.Add("@intBagCapacity", BagCapacity);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intAnticoagulant", Anticoagulant);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBagDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBagDetails(int DetailsId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intDetailsId", DetailsId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBagDetailsSearch(int HospitalLocationId, int FacilityId, string BatchNo,
            int BagId, int BagTypeId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvBatchNo", BatchNo);
                HshIn.Add("@intBagId", BagId);
                HshIn.Add("@intBagTypeId", BagTypeId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagDetailsSearch", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetKitQualityDetailsMain(int HospitalLocationId, int FacilityId, int KitQualityMainId, int Active, string ObtainedFrom, string Specificity, string Avidity, string KitId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intKitQualityMainId", KitQualityMainId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chvObtained", ObtainedFrom);
                HshIn.Add("@chvSpecificity", Specificity);
                HshIn.Add("@chvAvidity", Avidity);
                HshIn.Add("@intKitId", KitId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitQualityDetailsMain", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetKitQualityTitreDetails(int KitQualityMainId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intKitQualityMainId", KitQualityMainId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitQualityTitreDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComponentRequisitionPatientDetails(long RegistrationId, string Ptype, int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@PType", Ptype);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionPatientDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEmployeeReturnBy(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {


                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        #region "Comment Code"
        //public String SaveComponentRequisition(int RequisitionId, string RequisitionNo, int HospitalLocationId, int FacilityId, string RequestType, int IsOuterRequest,
        // int RegistrationId, string RegistrationNo, int EncounterId, string EncounterNo, DateTime RequestDate, string PatientName, string GuardianName, int Sex, DateTime DateofBirth,
        // int AgeYear, int AgeMonth, int AgeDays, double Weight, int IsPrediatric, string ChargeSlipNo, int PatientBloodGroupId, int MotherBloodGroupId, int ReasonId,
        //int IsExchangeTransfusion, double HaematocritRequired, int IndicationId, DateTime ConsentDate, int ConsentTakenBy, string Remarks, int Active, int UserId, string ComponentRequisitionDetailsXml,
        //string ComponentRequisitionQuestionDetailsXml, float HB, float Platlet, float PT, float APTTT, float FIBRINOGEN, int ConsultantID, string Diagnosis, string Ward, string BedNo, bool Pregnancy, bool Miscarriage, int orderId,int emergencyType, int HospitalID,string DoctorName,int SampleSend)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    HshIn.Add("@intRequisitionId", RequisitionId);
        //    HshIn.Add("@chvRequisitionNo", RequisitionNo);
        //    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@chvRequestType", RequestType);
        //    HshIn.Add("@bitIsOuterRequest", IsOuterRequest);
        //    HshIn.Add("@intRegistrationId", RegistrationId);
        //    HshIn.Add("@chvRegistrationNo", RegistrationNo);
        //    HshIn.Add("@intEncounterId", EncounterId);
        //    HshIn.Add("@chvEncounterNo", EncounterNo);
        //    HshIn.Add("@dtRequestDate", RequestDate);
        //    HshIn.Add("@chvPatientName", PatientName);
        //    HshIn.Add("@chvGuardianName", GuardianName);
        //    HshIn.Add("@inySex", Sex);
        //    HshIn.Add("@dtDateofBirth", DateofBirth);
        //    HshIn.Add("@inyAgeYear", AgeYear);
        //    HshIn.Add("@inyAgeMonth", AgeMonth);
        //    HshIn.Add("@inyAgeDays", AgeDays);
        //    HshIn.Add("@monWeight", Weight);
        //    HshIn.Add("@bitIsPrediatric", IsPrediatric);
        //    HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
        //    HshIn.Add("@intPatientBloodGroupId", PatientBloodGroupId);
        //    HshIn.Add("@intMotherBloodGroupId", MotherBloodGroupId);
        //    HshIn.Add("@intReasonId", ReasonId);
        //    HshIn.Add("@bitIsExchangeTransfusion", IsExchangeTransfusion);
        //    HshIn.Add("@decHaematocritRequired", HaematocritRequired);
        //    HshIn.Add("@intIndicationId", IndicationId);
        //    HshIn.Add("@dtConsentDate", ConsentDate);
        //    HshIn.Add("@intConsentTakenBy", ConsentTakenBy);
        //    HshIn.Add("@chvRemarks", Remarks);
        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@intEncodedBy", UserId);
        //    HshIn.Add("@xmlComponentRequisitionDetails", ComponentRequisitionDetailsXml);
        //    HshIn.Add("@xmlRequisitionQuestionDetails", ComponentRequisitionQuestionDetailsXml);

        //    HshIn.Add("@flHB", HB);
        //    HshIn.Add("@flPlatlets", Platlet);
        //    HshIn.Add("@flPT", PT);
        //    HshIn.Add("@flAPTTT", APTTT);
        //    HshIn.Add("@flFIBRINOGEN", FIBRINOGEN);
        //    HshIn.Add("@intConsultantID", ConsultantID);
        //    HshIn.Add("@chvDiagnosis", Diagnosis);
        //    HshIn.Add("@chvWard", Ward);
        //    HshIn.Add("@intBedNo", BedNo);
        //    HshIn.Add("@pregnancy", Pregnancy);
        //    HshIn.Add("@miscarriage", Miscarriage);
        //    HshIn.Add("@orderId", orderId);
        //    HshIn.Add("@emergencyType", emergencyType);

        //    HshIn.Add("@intHospitalID", HospitalID);
        //    HshIn.Add("@chrDoctorName", DoctorName);
        //    HshIn.Add("@intSampleSend", SampleSend);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentRequisition", HshIn, HshOut);
        //    return HshOut["@chvErrorStatus"].ToString();

        //}

        #endregion

        #region"SAVECOMPONENT REQUISITION FOR PLATECOUNT DATATYPE TO BE STRING FOR BLK"
        public String SaveComponentRequisition(int RequisitionId, string RequisitionNo, int HospitalLocationId, int FacilityId, string RequestType, int IsOuterRequest,
 int RegistrationId, string RegistrationNo, int EncounterId, string EncounterNo, DateTime RequestDate, string PatientName, string GuardianName, int Sex, DateTime DateofBirth,
 int AgeYear, int AgeMonth, int AgeDays, double Weight, int IsPrediatric, string ChargeSlipNo, int PatientBloodGroupId, int MotherBloodGroupId, int ReasonId,
int IsExchangeTransfusion, double HaematocritRequired, int IndicationId, DateTime ConsentDate, int ConsentTakenBy, string Remarks, int Active, int UserId, string ComponentRequisitionDetailsXml,
string ComponentRequisitionQuestionDetailsXml, float HB, string Platlet, float PT, float APTTT, float FIBRINOGEN, int ConsultantID, string Diagnosis, string Ward, string BedNo, bool Pregnancy, bool Miscarriage, int orderId, int emergencyType, int HospitalID, string DoctorName, int SampleSend
   , int anyTransfusion, int anyTransfusionReaction, int irradiatedComponent,
int OtherHospitalID, string OtherMRDNO, string OtherWARDNAME, string OtherBEDNO, string OtherDOctorName)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@chvRequisitionNo", RequisitionNo);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvRequestType", RequestType);
            HshIn.Add("@bitIsOuterRequest", IsOuterRequest);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@dtRequestDate", RequestDate);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@chvGuardianName", GuardianName);
            HshIn.Add("@inySex", Sex);
            HshIn.Add("@dtDateofBirth", DateofBirth);
            HshIn.Add("@inyAgeYear", AgeYear);
            HshIn.Add("@inyAgeMonth", AgeMonth);
            HshIn.Add("@inyAgeDays", AgeDays);
            HshIn.Add("@monWeight", Weight);
            HshIn.Add("@bitIsPrediatric", IsPrediatric);
            HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
            HshIn.Add("@intPatientBloodGroupId", PatientBloodGroupId);
            HshIn.Add("@intMotherBloodGroupId", MotherBloodGroupId);
            HshIn.Add("@intReasonId", ReasonId);
            HshIn.Add("@bitIsExchangeTransfusion", IsExchangeTransfusion);
            HshIn.Add("@decHaematocritRequired", HaematocritRequired);
            HshIn.Add("@intIndicationId", IndicationId);
            HshIn.Add("@dtConsentDate", ConsentDate);
            HshIn.Add("@intConsentTakenBy", ConsentTakenBy);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlComponentRequisitionDetails", ComponentRequisitionDetailsXml);
            HshIn.Add("@xmlRequisitionQuestionDetails", ComponentRequisitionQuestionDetailsXml);

            HshIn.Add("@flHB", HB);
            HshIn.Add("@flPlatlets", Platlet);
            HshIn.Add("@flPT", PT);
            HshIn.Add("@flAPTTT", APTTT);
            HshIn.Add("@flFIBRINOGEN", FIBRINOGEN);
            HshIn.Add("@intConsultantID", ConsultantID);
            HshIn.Add("@chvDiagnosis", Diagnosis);
            HshIn.Add("@chvWard", Ward);
            HshIn.Add("@intBedNo", BedNo);
            HshIn.Add("@pregnancy", Pregnancy);
            HshIn.Add("@miscarriage", Miscarriage);
            HshIn.Add("@orderId", orderId);
            HshIn.Add("@emergencyType", emergencyType);

            HshIn.Add("@intHospitalID", HospitalID);
            HshIn.Add("@chrDoctorName", DoctorName);
            HshIn.Add("@intSampleSend", SampleSend);
            HshIn.Add("@bitanyTransfusion", anyTransfusion);
            HshIn.Add("@bitanyTransfusionReaction", anyTransfusionReaction);
            HshIn.Add("@bitirradiatedComponent", irradiatedComponent);

            HshIn.Add("@OtherHospitalID", OtherHospitalID);
            HshIn.Add("@OtherMRDNO", OtherMRDNO);
            HshIn.Add("@OtherWARDNAME", OtherWARDNAME);
            HshIn.Add("@OtherBEDNO", OtherBEDNO);
            HshIn.Add("@OtherDOctorName", OtherDOctorName);



            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentRequisition", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        #endregion

        #region"PLATECOUNT DATATYPE FLOAT"
        public String SaveComponentRequisition(int RequisitionId, string RequisitionNo, int HospitalLocationId, int FacilityId, string RequestType, int IsOuterRequest,
         int RegistrationId, string RegistrationNo, int EncounterId, string EncounterNo, DateTime RequestDate, string PatientName, string GuardianName, int Sex, DateTime DateofBirth,
         int AgeYear, int AgeMonth, int AgeDays, double Weight, int IsPrediatric, string ChargeSlipNo, int PatientBloodGroupId, int MotherBloodGroupId, int ReasonId,
        int IsExchangeTransfusion, double HaematocritRequired, int IndicationId, DateTime ConsentDate, int ConsentTakenBy, string Remarks, int Active, int UserId, string ComponentRequisitionDetailsXml,
        string ComponentRequisitionQuestionDetailsXml, float HB, float Platlet, float PT, float APTTT, float FIBRINOGEN, int ConsultantID, string Diagnosis, string Ward, string BedNo, bool Pregnancy, bool Miscarriage, int orderId, int emergencyType, int HospitalID, string DoctorName, int SampleSend
           , int anyTransfusion, int anyTransfusionReaction, int irradiatedComponent,
int OtherHospitalID, string OtherMRDNO, string OtherWARDNAME, string OtherBEDNO, string OtherDOctorName)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@chvRequisitionNo", RequisitionNo);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvRequestType", RequestType);
            HshIn.Add("@bitIsOuterRequest", IsOuterRequest);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@dtRequestDate", RequestDate);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@chvGuardianName", GuardianName);
            HshIn.Add("@inySex", Sex);
            HshIn.Add("@dtDateofBirth", DateofBirth);
            HshIn.Add("@inyAgeYear", AgeYear);
            HshIn.Add("@inyAgeMonth", AgeMonth);
            HshIn.Add("@inyAgeDays", AgeDays);
            HshIn.Add("@monWeight", Weight);
            HshIn.Add("@bitIsPrediatric", IsPrediatric);
            HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
            HshIn.Add("@intPatientBloodGroupId", PatientBloodGroupId);
            HshIn.Add("@intMotherBloodGroupId", MotherBloodGroupId);
            HshIn.Add("@intReasonId", ReasonId);
            HshIn.Add("@bitIsExchangeTransfusion", IsExchangeTransfusion);
            HshIn.Add("@decHaematocritRequired", HaematocritRequired);
            HshIn.Add("@intIndicationId", IndicationId);
            HshIn.Add("@dtConsentDate", ConsentDate);
            HshIn.Add("@intConsentTakenBy", ConsentTakenBy);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlComponentRequisitionDetails", ComponentRequisitionDetailsXml);
            HshIn.Add("@xmlRequisitionQuestionDetails", ComponentRequisitionQuestionDetailsXml);

            HshIn.Add("@flHB", HB);
            HshIn.Add("@flPlatlets", Platlet);
            HshIn.Add("@flPT", PT);
            HshIn.Add("@flAPTTT", APTTT);
            HshIn.Add("@flFIBRINOGEN", FIBRINOGEN);
            HshIn.Add("@intConsultantID", ConsultantID);
            HshIn.Add("@chvDiagnosis", Diagnosis);
            HshIn.Add("@chvWard", Ward);
            HshIn.Add("@intBedNo", BedNo);
            HshIn.Add("@pregnancy", Pregnancy);
            HshIn.Add("@miscarriage", Miscarriage);
            HshIn.Add("@orderId", orderId);
            HshIn.Add("@emergencyType", emergencyType);

            HshIn.Add("@intHospitalID", HospitalID);
            HshIn.Add("@chrDoctorName", DoctorName);
            HshIn.Add("@intSampleSend", SampleSend);
            HshIn.Add("@bitanyTransfusion", anyTransfusion);
            HshIn.Add("@bitanyTransfusionReaction", anyTransfusionReaction);
            HshIn.Add("@bitirradiatedComponent", irradiatedComponent);

            HshIn.Add("@OtherHospitalID", OtherHospitalID);
            HshIn.Add("@OtherMRDNO", OtherMRDNO);
            HshIn.Add("@OtherWARDNAME", OtherWARDNAME);
            HshIn.Add("@OtherBEDNO", OtherBEDNO);
            HshIn.Add("@OtherDOctorName", OtherDOctorName);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentRequisition", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion
        public DataSet GetComponentRequisition(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisition", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetComponentRequisition(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int ReleaseID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intReleaseID", ReleaseID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisition", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComponentRequisitionForTTIAck(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int TTIAcknowledge)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@intTTIAcknowledge", TTIAcknowledge);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionForTTIAck", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetBloodIssueDetails(int HospitalLocationId, int FacilityId, int Active, int RegistrationId, string RegistrationNo, int EncounterId, int Issue)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitIssue", Issue);

                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodIssuedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBloodIssueDetails(int HospitalLocationId, int FacilityId, int Active, int RegistrationId, string RegistrationNo, int EncounterId, int Issue, string EncounterNo, string PatientName, string RequestType, string BagNo, string StartDate, string EndDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitIssue", Issue);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@chvBagNo", BagNo);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodIssuedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetComponentIssueAcknowledgeDetails(int HospitalLocationId, int FacilityId, string CrossIssueNo, int CrossMatchId, int bloodAcknowledged, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@CrossIssueNo", CrossIssueNo);
                HshIn.Add("@intCrossMatchId", CrossMatchId);
                HshIn.Add("@bitbloodAcknowledged", bloodAcknowledged);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueAcknowledgeDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentReturnDetail(int HospitalLocationId, int FacilityId, string ComponentIssueNo, int Active, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int Acknowledged, int ReturnId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterID", EncounterID);
                HshIn.Add("@intAcknowledged", Acknowledged);
                HshIn.Add("@intReturnId", ReturnId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentReturnDetail", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveComponetReturnAcknowledge(int ReturnId, string AcknowledgeDetail)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intReturnId", ReturnId);
            HshIn.Add("@xmlComponentReturnAcknowledge", AcknowledgeDetail);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentReturnAcknowledge", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentReturnAcknowledgeDetail(int HospitalLocationId, int FacilityId, int Acknowledged, int ReturnId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intAcknowledged", Acknowledged);
                HshIn.Add("@intReturnId", ReturnId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentReturnAcknowledge", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentReturnAcknowledge(int HospitalLocationId, int FacilityId, int Active, int Acknowledged)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intAcknowledged", Acknowledged);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentReturnAcknowledgeList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCrossMatchReturnAcknowledgeDetail(int HospitalLocationId, int FacilityId, int Acknowledged, int ReturnId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intAcknowledged", Acknowledged);
                HshIn.Add("@intReturnId", ReturnId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchReturnAcknowledgeDetail", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentCrossMatchReturnDetail(int HospitalLocationId, int FacilityId, int Active, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int ReturnId, int CrossMatchId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                //HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterID", EncounterID);
                HshIn.Add("@intCrossMatchId", CrossMatchId);
                HshIn.Add("@intReturnId", ReturnId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCrossMatchReturnDetail", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentCrossMatchReturnDetail(int HospitalLocationId, int FacilityId, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int CrossMatchId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                //HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
                //HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterID", EncounterID);
                HshIn.Add("@intCrossMatchId", CrossMatchId);
                //HshIn.Add("@intReturnId", ReturnId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCrossMatchReturnDetail", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCrossMatchDetailsForCancel(int HospitalLocationId, int FacilityId, int Active, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, string PatientName, string RequestType, string UnitNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                //HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterID", EncounterID);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@UnitNo", UnitNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchReturnList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetBloodAcknowledgeDetails(int HospitalLocationId, int FacilityId, int Active, int RegistrationId, string RegistrationNo, int EncounterId, int BloodAcknowledge, string EncounterNo, string PatientName, string RequestType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitBloodAcknowledge", BloodAcknowledge);

                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvRequestType", RequestType);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodAcknowledgeDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }



        public DataSet GetUnitNoWithPatient(string RegistrationNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBUnitwithPatient", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetUnitNoWithPatient(string RegistrationNo, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);

                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBUnitwithPatient", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        //add by prashant 04/05/2014
        //public DataSet GetComponentRequisition(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int EncounterId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@intRequisitionId", RequisitionId);
        //        HshIn.Add("@chvRequestType", RequestType);
        //        HshIn.Add("@bitActive", Active);
        //        HshIn.Add("@intEncounterId", EncounterId);
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionList", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        public DataSet GetComponentRequisition(int HospitalLocationId, int FacilityId, int RequisitionId, long RegistrationId, string RequestType, int Active, int EncounterId, string EncounterNo, string PatientName, string AckStatus, string Ptype, string status, string isActive)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatienName", PatientName);
                HshIn.Add("@chrRequestAcknowledged", AckStatus);
                HshIn.Add("@chrPType", Ptype);
                HshIn.Add("@chrStatus", status);
                HshIn.Add("@chrActive", isActive);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        //prashant Bamania 04/05/2014
        public DataSet GetComponentRequisitionAcknowledge(int HospitalLocationId, int FacilityId, int EncounterId, int Active, int RegistrationId, int ReleaseAcknowledgeId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intReleaseAcknowledge", ReleaseAcknowledgeId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionAcknowledge", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }

        //prashant Bamania 04/05/2014
        public DataSet GetReleaseAcknowledge(int HospitalLocationId, int FacilityId, int Active, int ReleaseId, int ReleaseAcknowledgeId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intReleaseId", ReleaseId);
                HshIn.Add("@intReleaseAcknowledge", ReleaseAcknowledgeId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetReleaseAcknowledge", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCrossMatchedDetails(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int DetailId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intDetail", DetailId);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCrossMatchedDetails(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int DetailId, int iSCrossMatch, string RegistrationNo, string EncounterNo, string PatientName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intDetail", DetailId);
                HshIn.Add("@intCrossMatched", iSCrossMatch);

                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        //Get search cross matched data list
        public DataSet GetCrossMatchedSearchDetails(int HospitalLocationId, int FacilityId, int Active, int iSCrossMatch, string RegistrationNo, string EncounterNo, string PatientName, string UnitNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intCrossMatched", iSCrossMatch);

                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvUnitNo", UnitNo);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossMatchedSearchDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRequisitionAcknowledge(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RequistionAcknowledge)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intReuestAcknowledge", RequistionAcknowledge);

                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetRequisitionAcknowledge", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetRequisitionAcknowledgeListFilter(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RequistionAcknowledge, string RegistrationNo, string EncounterNo, string PatientName, string RequisitionNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intReuestAcknowledge", RequistionAcknowledge);

                HshIn.Add("@intRegistrationId", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvRequisitionNo", RequisitionNo);


                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetRequisitionAcknowledgeListFilter", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetRequisitionAcknowledge(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RequistionAcknowledge, string RegistrationNo, string EncounterNo, string PatientName
            //, string RequisitionNo
            )
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intReuestAcknowledge", RequistionAcknowledge);

                HshIn.Add("@intRegistrationId", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                //HshIn.Add("@chvRequisitionNo", PatientName);


                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetRequisitionAcknowledge", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetRequisitionAcknowledge(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RequistionAcknowledge, string RegistrationNo, string EncounterNo, string PatientName, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intReuestAcknowledge", RequistionAcknowledge);

                HshIn.Add("@intRegistrationId", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);

                HshIn.Add("@OPIP", OPIP);

                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetRequisitionAcknowledge", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetBloodStockComponentBagNo(int HospitalLocationId, int FacilityId, int ComponentId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodStockComponentBagNo", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetBloodStockComponentBagNo(int HospitalLocationId, int FacilityId, int ComponentId, int BloodGroupId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@intBloodGroupId", BloodGroupId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodStockComponentBagNo", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBloodBankEmployeeList(int HospitalLocationId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@cDepartmentIdendification", "BB");
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetComponentRequisitionForRelease(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisition", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }




        //public DataSet GetPatientTransfusionList(int HospitalLocationId, int FacilityId, int RegistrationNo)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@intRegistrationNo", RegistrationNo);
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "GetPatientTransfusionList", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        public DataSet GetPatientTransfusionList(int HospitalLocationId, int FacilityId, int Active, int RegistrationId, string RegistrationNo, int EncounterId, int BloodAcknowledge, int Status)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitBloodAcknowledge", BloodAcknowledge);
                HshIn.Add("@bitStatus", Status);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPatientTransfusionList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComponentRequisitionForRelease(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RegistrationId, int ReleaseAcknowledgeFlag)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intReleaseAcknowledge", ReleaseAcknowledgeFlag);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisition", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetComponentRequisitionForReleaseAknowledgeList(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RegistrationId, int ReleaseAcknowledgeList)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intReleaseAcknowledge", ReleaseAcknowledgeList);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisition", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetReleaseAcknowledgeList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getReleaseRequisitionList(int HospId, int FacilityId, string RequisitionNo, string PatientName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRequisitionNo", RequisitionNo);
                HshIn.Add("@chvPatientName", PatientName);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetReleaseRequisitionList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string releaseRequisitionCancel(int ReleaseId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@intReleaseId", ReleaseId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBReleaseRequisitionCancel", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //public DataSet GetBloodReleaseRequestion(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RegistrationId, int EncounterId)
        public DataSet GetBloodReleaseRequestion(int HospitalLocationId, int FacilityId, int RequisitionId, string RequestType, int Active, int RegistrationId, int EncounterId, string RegistrationNo, string EncounterNo, string PatientName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetRequisitionReleaseList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetComponentDetailsUsingRequestNo(int HospitalLocationId, int FacilityId, int RequisitionId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentDetailsUsingRequestNo", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        // add by prashant 04/05/2014
        public DataSet GetRequisitionAcknowledgedComponent(int HospitalLocationId, int FacilityId, int RequisitionId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetRequisitionAcknowledgedComponent", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetComponentBagNo(int HospitalLocationId, int FacilityId, int ComponentId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentBagNo", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCounselingDetails(int HospitalLocationId, int FacilityId, int CounselingId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCounselingDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentRequisitionBloodGroup(int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionBloodGroup", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentRequisitionDetails(int HospitalLocationId, int FacilityId, int RequisitionId, int DetailsId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@intDetailsId", DetailsId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentRequisitionBloodComponentMaster(int HospitalLocationId, int FacilityId, int IsProcedure, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitIsProcedure", IsProcedure);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionBloodComponentMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentRequestionPreviousHistoryQuestionMaster(int HospitalLocationId, int FacilityId, string Gender, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvGender", Gender);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequestionPreviousHistoryQuestionMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetComponentRequisitionIndicationTransfusion(string ComponentIds)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@xmlComponentIds", ComponentIds);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionIndicationTransfusion", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet GetComponentRequisitionQuestionDetails(int HospitalLocationId, int FacilityId, int RequisitionId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentRequisitionQuestionDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentForOpeningStock(int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@Active", Active);
                string qry = "select ComponentId, ComponentName from BBBloodComponentMaster WITH (NOLOCK) where Active=@Active";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet GetParentComponent(int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@Active", Active);
                string qry = "select ComponentId, ComponentName from BBBloodComponentMaster WITH (NOLOCK) where ComponentType='P' AND Active=@Active ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }


        public String SaveOpeningStock(int StockId, int HospitalLocationId, int FacilityId, string ManualNo,
            DateTime OpeningDate, int BloodGroupId, int ComponentId, int ApprovedBy, string Remarks, int Active, int UserId, string UnitInformation)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intStockId", StockId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvManualNo", ManualNo);
            HshIn.Add("@dtOpeningDate", OpeningDate);
            HshIn.Add("@intBloodGroupId", BloodGroupId);
            HshIn.Add("@intComponentId", ComponentId);
            HshIn.Add("@intApprovedBy", ApprovedBy);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@xmlOpeningStockDetails", UnitInformation);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveOpeningStockMain", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }



        public String SaveOrUpdateRequisitionAcknowledge(int RequisitionId, int HospitalLocationId, int FacilityId,
            int RequestAcknowledged, int SampleAcknowledged, int SampleAcknowledgedBy, string SampleReceivedDatetime,
            string SampleAcknowledgedTime, string ManualRequestNo, int UserId, string ReqAckNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestAcknowledged", RequestAcknowledged);
            HshIn.Add("@intSampleAcknowledged", SampleAcknowledged);
            HshIn.Add("@intSampleAcknowledgedBy", SampleAcknowledgedBy);
            HshIn.Add("@dtSampleReceivedDatetime", SampleReceivedDatetime);
            HshIn.Add("@dtSampleAcknowledgedTime", SampleAcknowledgedTime);
            HshIn.Add("@chvManualRequestNo", ManualRequestNo);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@chrReqAckNo", ReqAckNo);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveOrUpdateRequisitionAcknowledge", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public String SaveOrUpdateRequisitionAcknowledge(int RequisitionId, int HospitalLocationId, int FacilityId,
            int RequestAcknowledged, int SampleAcknowledged, int SampleAcknowledgedBy, string SampleReceivedDatetime,
            string SampleAcknowledgedTime, string ManualRequestNo, int UserId, string ReqAckNo, string BillNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestAcknowledged", RequestAcknowledged);
            HshIn.Add("@intSampleAcknowledged", SampleAcknowledged);
            HshIn.Add("@intSampleAcknowledgedBy", SampleAcknowledgedBy);
            HshIn.Add("@dtSampleReceivedDatetime", SampleReceivedDatetime);
            HshIn.Add("@dtSampleAcknowledgedTime", SampleAcknowledgedTime);
            HshIn.Add("@chvManualRequestNo", ManualRequestNo);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@chrReqAckNo", ReqAckNo);
            HshIn.Add("@chrBillNo", BillNo);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveOrUpdateRequisitionAcknowledge", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        /****SPS HOSPITAL****/
        public String SaveOrUpdateRequisitionAcknowledge(int RequisitionId, int HospitalLocationId, int FacilityId,
           int RequestAcknowledged, int SampleAcknowledged, int SampleAcknowledgedBy, string SampleReceivedDatetime,
           string SampleAcknowledgedTime, string ManualRequestNo, int UserId, string ReqAckNo, string BillNo, string DoctorId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestAcknowledged", RequestAcknowledged);
            HshIn.Add("@intSampleAcknowledged", SampleAcknowledged);
            HshIn.Add("@intSampleAcknowledgedBy", SampleAcknowledgedBy);
            HshIn.Add("@dtSampleReceivedDatetime", SampleReceivedDatetime);
            HshIn.Add("@dtSampleAcknowledgedTime", SampleAcknowledgedTime);
            HshIn.Add("@chvManualRequestNo", ManualRequestNo);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@chrReqAckNo", ReqAckNo);
            HshIn.Add("@chrBillNo", BillNo);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@DoctorId", DoctorId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveOrUpdateRequisitionAcknowledge", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }




        public String SaveOrUpdateRequisitionAcknowledgeForTTI(int RequisitionId, int HospitalLocationId, int FacilityId,
            int SampleAcknowledged, int SampleAcknowledgedBy, string ManualNumber, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intSampleAcknowledgedForTTI", SampleAcknowledged);
            HshIn.Add("@chrManualNumber", ManualNumber);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveOrUpdateRequisitionAcknowledgeForTTI", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveKitQualityDetails(int KitQualityId, int HospitalLocationId, int FacilityId, int KitNo, string StorageTemperature, string SampleObtainedFrom, string ReasonforChecking,
            string Appearance, string Volume, string Avidity, string Specificity, string Remarks, string ApprovedOrNot, string xmlKitQualityDetails, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intKitQualityId", KitQualityId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intKitNo", KitNo);
            HshIn.Add("@chvStorageTemperature", StorageTemperature);
            HshIn.Add("@chvSampleObtainedFrom", SampleObtainedFrom);
            HshIn.Add("@chvReasonforChecking", ReasonforChecking);
            HshIn.Add("@chvAppearance", Appearance);
            HshIn.Add("@chvVolume", Volume);
            HshIn.Add("@chvAvidity", Avidity);
            HshIn.Add("@chvSpecificity", Specificity);
            HshIn.Add("@xmlKitQualityDetails", xmlKitQualityDetails);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@chrApprovedOrNot", ApprovedOrNot);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveKitQualityDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveCrossMatchedMasterDetails(string CrossMatchId, int BookingNo, int HospitalLocationId, int FacilityId, string BagNo, string TestCompatible, string TestDone, string CrossMatchComponent, string ActualCrossMatchComponent
           , int CrossMatchQty, int CrossMatchedUnits, int QtyIssued, int IssueBookingNo, string ActualCrossMatchComponent_Group, string Remarks, int CrossMatchedBy, string CrossMatchDate, string ExchangeTransfusion, int Active
           , int EncodedBy, string EncodedDate, string SampleReceivedDatetime, int SampleAcknowledged, string HB, string platelet, string PTT, string PT, string ATT, string FIBRINOGEN, string Reason, int ReleaseID, string CancellationRemark, string ChargeSlipNo, decimal BillAmt, string ComConversion, string xmlDetails
           , int RegistrationId, int EncounterId, string strXML, string Instruction, int Provider, int CompanyId, string OrderType, string PayerType, string PatientOPIP
           , int InsuranceId, int CardId, DateTime OrderDate
           , string ChargeCalculationRequired, bool AllergyReview, int isErrorEMRService, int AntibodyScreening, int IsDAT, int irradiatedComponent, int NATResult, int PostTransplant, int Forward, string Reverse, int Final, ref string crossmatch, int BloodGroupEnteredInCrossmatch, string CrossmatchRemarks)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEnounterId", EncounterId);
            HshIn.Add("@xmlServices", strXML);
            //HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intDoctorId", Provider);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@inyOrderType", OrderType);
            HshIn.Add("@cPayerType", PayerType);
            HshIn.Add("@cPatientOPIP", PatientOPIP);
            HshIn.Add("@iInsuranceId", InsuranceId);
            HshIn.Add("@iCardId", CardId);
            HshIn.Add("@dtsOrderDate", OrderDate);
            HshIn.Add("@chrChargeCalculationRequired", ChargeCalculationRequired);
            HshIn.Add("@bitAllergyReviewed", AllergyReview);
            HshIn.Add("@iIsERorEMRServices", isErrorEMRService);



            HshIn.Add("@intCrossMatchNo", CrossMatchId);
            HshIn.Add("@intBookingNo", BookingNo);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBagNo", BagNo);
            HshIn.Add("@chvTestCompatible", TestCompatible);
            HshIn.Add("@chvTestDone", TestDone);
            HshIn.Add("@chrCrossMatchComponent", CrossMatchComponent);
            HshIn.Add("@chraCTUALCrossMatchComponent", ActualCrossMatchComponent);
            HshIn.Add("@intCrossMatchQty", CrossMatchQty);
            HshIn.Add("@intCrossMatchedUnits", CrossMatchedUnits);
            HshIn.Add("@intQtyIssued", QtyIssued);
            HshIn.Add("@intIssueBookingNo", IssueBookingNo);
            HshIn.Add("@chvActualCrossMatchComponent_Group", ActualCrossMatchComponent_Group);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intCrossMatchedBy", CrossMatchedBy);
            HshIn.Add("@dtCrossMatchDate", CrossMatchDate);
            HshIn.Add("@chrExchangeTransfusion", ExchangeTransfusion);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@dtEncodedDate", EncodedDate);
            HshIn.Add("@dtSampleReceivedDatetime", SampleReceivedDatetime);
            HshIn.Add("@btSampleAcknowledged", SampleAcknowledged);
            HshIn.Add("@chvHB", HB);
            HshIn.Add("@chvPlatelet", platelet);
            HshIn.Add("@chvPTT", PTT);
            HshIn.Add("@chvPT", PT);
            HshIn.Add("@chvATT", ATT);
            HshIn.Add("@chvFIBRINOGEN", FIBRINOGEN);
            HshIn.Add("@chvReason", Reason);
            HshIn.Add("@intReleaseID", ReleaseID);
            HshIn.Add("@chvCancellationRemark", CancellationRemark);
            HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
            HshIn.Add("@monBillAmt", BillAmt);
            HshIn.Add("@chrConversion", ComConversion);
            HshIn.Add("@xmlCrossMatchGridDetails", xmlDetails);

            HshIn.Add("@AntibodyScreening", AntibodyScreening);
            HshIn.Add("@IsDAT", IsDAT);
            HshIn.Add("@irradiatedComponent", irradiatedComponent);
            HshIn.Add("@NATResult", NATResult);
            HshIn.Add("@intPostTransplant", PostTransplant);
            HshIn.Add("@intForward", Forward);
            HshIn.Add("@chvReverse", Reverse);
            HshIn.Add("@intFinal", Final);
            HshIn.Add("@intBloodGroupEnteredInCrossmatch", BloodGroupEnteredInCrossmatch);
            HshIn.Add("@chvCrossmatchRemarks", CrossmatchRemarks);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvcrossMatchno1", SqlDbType.VarChar);
            //HshOut["@chvcrossMatchno1"].ToString();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveCrossMatchedMasterDetails", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        #region "KIRAN HOSPITAL CROSS MATCH NO ex IP COMPONENT REQUESTION NO + No ex KBH/01/01"
        public Hashtable SaveCrossMatchedMasterDetails(string CrossMatchId, int BookingNo, int HospitalLocationId, int FacilityId, string BagNo, string TestCompatible, string TestDone, string CrossMatchComponent, string ActualCrossMatchComponent
         , int CrossMatchQty, int CrossMatchedUnits, int QtyIssued, int IssueBookingNo, string ActualCrossMatchComponent_Group, string Remarks, int CrossMatchedBy, string CrossMatchDate, string ExchangeTransfusion, int Active
         , int EncodedBy, string EncodedDate, string SampleReceivedDatetime, int SampleAcknowledged, string HB, string platelet, string PTT, string PT, string ATT, string FIBRINOGEN, string Reason, int ReleaseID, string CancellationRemark, string ChargeSlipNo, decimal BillAmt, string ComConversion, string xmlDetails
         , int RegistrationId, int EncounterId, string strXML, string Instruction, int Provider, int CompanyId, string OrderType, string PayerType, string PatientOPIP
         , int InsuranceId, int CardId, DateTime OrderDate
         , string ChargeCalculationRequired, bool AllergyReview, int isErrorEMRService, int AntibodyScreening, int IsDAT, int irradiatedComponent, int NATResult, int PostTransplant, int Forward, string Reverse, int Final, ref string crossmatch, int BloodGroupEnteredInCrossmatch, string CrossmatchRemarks, string IPRequestNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEnounterId", EncounterId);
            HshIn.Add("@xmlServices", strXML);
            //HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intDoctorId", Provider);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@inyOrderType", OrderType);
            HshIn.Add("@cPayerType", PayerType);
            HshIn.Add("@cPatientOPIP", PatientOPIP);
            HshIn.Add("@iInsuranceId", InsuranceId);
            HshIn.Add("@iCardId", CardId);
            HshIn.Add("@dtsOrderDate", OrderDate);
            HshIn.Add("@chrChargeCalculationRequired", ChargeCalculationRequired);
            HshIn.Add("@bitAllergyReviewed", AllergyReview);
            HshIn.Add("@iIsERorEMRServices", isErrorEMRService);



            HshIn.Add("@intCrossMatchNo", CrossMatchId);
            HshIn.Add("@intBookingNo", BookingNo);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBagNo", BagNo);
            HshIn.Add("@chvTestCompatible", TestCompatible);
            HshIn.Add("@chvTestDone", TestDone);
            HshIn.Add("@chrCrossMatchComponent", CrossMatchComponent);
            HshIn.Add("@chraCTUALCrossMatchComponent", ActualCrossMatchComponent);
            HshIn.Add("@intCrossMatchQty", CrossMatchQty);
            HshIn.Add("@intCrossMatchedUnits", CrossMatchedUnits);
            HshIn.Add("@intQtyIssued", QtyIssued);
            HshIn.Add("@intIssueBookingNo", IssueBookingNo);
            HshIn.Add("@chvActualCrossMatchComponent_Group", ActualCrossMatchComponent_Group);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intCrossMatchedBy", CrossMatchedBy);
            HshIn.Add("@dtCrossMatchDate", CrossMatchDate);
            HshIn.Add("@chrExchangeTransfusion", ExchangeTransfusion);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@dtEncodedDate", EncodedDate);
            HshIn.Add("@dtSampleReceivedDatetime", SampleReceivedDatetime);
            HshIn.Add("@btSampleAcknowledged", SampleAcknowledged);
            HshIn.Add("@chvHB", HB);
            HshIn.Add("@chvPlatelet", platelet);
            HshIn.Add("@chvPTT", PTT);
            HshIn.Add("@chvPT", PT);
            HshIn.Add("@chvATT", ATT);
            HshIn.Add("@chvFIBRINOGEN", FIBRINOGEN);
            HshIn.Add("@chvReason", Reason);
            HshIn.Add("@intReleaseID", ReleaseID);
            HshIn.Add("@chvCancellationRemark", CancellationRemark);
            HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
            HshIn.Add("@monBillAmt", BillAmt);
            HshIn.Add("@chrConversion", ComConversion);
            HshIn.Add("@xmlCrossMatchGridDetails", xmlDetails);

            HshIn.Add("@AntibodyScreening", AntibodyScreening);
            HshIn.Add("@IsDAT", IsDAT);
            HshIn.Add("@irradiatedComponent", irradiatedComponent);
            HshIn.Add("@NATResult", NATResult);
            HshIn.Add("@intPostTransplant", PostTransplant);
            HshIn.Add("@intForward", Forward);
            HshIn.Add("@chvReverse", Reverse);
            HshIn.Add("@intFinal", Final);
            HshIn.Add("@intBloodGroupEnteredInCrossmatch", BloodGroupEnteredInCrossmatch);
            HshIn.Add("@chvCrossmatchRemarks", CrossmatchRemarks);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvcrossMatchno1", SqlDbType.VarChar);
            //HshOut["@chvcrossMatchno1"].ToString();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveCrossMatchedMasterDetails", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        #endregion



        public String SaveCrossMatchedMasterDetails(string CrossMatchId, int BookingNo, int HospitalLocationId, int FacilityId, string BagNo, string TestCompatible, string TestDone, string CrossMatchComponent, string ActualCrossMatchComponent
           , int CrossMatchQty, int CrossMatchedUnits, int QtyIssued, int IssueBookingNo, string ActualCrossMatchComponent_Group, string Remarks, int CrossMatchedBy, string CrossMatchDate, string ExchangeTransfusion, int Active
           , int EncodedBy, string EncodedDate, string SampleReceivedDatetime, int SampleAcknowledged, string HB, string platelet, string PTT, string PT, string ATT, string FIBRINOGEN, string Reason, int ReleaseID, string CancellationRemark, string ChargeSlipNo, decimal BillAmt, string ComConversion, string xmlDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intCrossMatchNo", CrossMatchId);
            HshIn.Add("@intBookingNo", BookingNo);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBagNo", BagNo);
            HshIn.Add("@chvTestCompatible", TestCompatible);
            HshIn.Add("@chvTestDone", TestDone);
            HshIn.Add("@chrCrossMatchComponent", CrossMatchComponent);
            HshIn.Add("@chraCTUALCrossMatchComponent", ActualCrossMatchComponent);
            HshIn.Add("@intCrossMatchQty", CrossMatchQty);
            HshIn.Add("@intCrossMatchedUnits", CrossMatchedUnits);
            HshIn.Add("@intQtyIssued", QtyIssued);
            HshIn.Add("@intIssueBookingNo", IssueBookingNo);
            HshIn.Add("@chvActualCrossMatchComponent_Group", ActualCrossMatchComponent_Group);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intCrossMatchedBy", CrossMatchedBy);
            HshIn.Add("@dtCrossMatchDate", CrossMatchDate);
            HshIn.Add("@chrExchangeTransfusion", ExchangeTransfusion);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@dtEncodedDate", EncodedDate);
            HshIn.Add("@dtSampleReceivedDatetime", SampleReceivedDatetime);
            HshIn.Add("@btSampleAcknowledged", SampleAcknowledged);
            HshIn.Add("@chvHB", HB);
            HshIn.Add("@chvPlatelet", platelet);
            HshIn.Add("@chvPTT", PTT);
            HshIn.Add("@chvPT", PT);
            HshIn.Add("@chvATT", ATT);
            HshIn.Add("@chvFIBRINOGEN", FIBRINOGEN);
            HshIn.Add("@chvReason", Reason);
            HshIn.Add("@intReleaseID", ReleaseID);
            HshIn.Add("@chvCancellationRemark", CancellationRemark);
            HshIn.Add("@chvChargeSlipNo", ChargeSlipNo);
            HshIn.Add("@monBillAmt", BillAmt);
            HshIn.Add("@chrConversion", ComConversion);
            HshIn.Add("@xmlCrossMatchGridDetails", xmlDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveCrossMatchedMasterDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveComponentIssueDetails(string ComponentIssueNo, string RequestCrossMatchNo, string BloodCenterReceivingNo, int HospitalLocationId, int FacilityId,
                DateTime IssueDate, int AttendentNameId, string Type, string Remarks, int CrossMatchedBy, int Active,
            int EncodedBy, DateTime EncodedDate, string ComponentIssueDetails, int ReleaseID, int DetailsID, string AttendentName, int CrossMatchId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
            HshIn.Add("@chvRequestCrossMatchNo", RequestCrossMatchNo);
            HshIn.Add("@chvBloodCenterReceivingNo", BloodCenterReceivingNo);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@dtIssueDate", IssueDate);
            HshIn.Add("@intAttendentNameId", AttendentNameId);
            HshIn.Add("@chrType", Type);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intCrossMatchedBy", CrossMatchedBy);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@dtEncodedDate", EncodedDate);
            HshIn.Add("@xmlComponentIssueDetails", ComponentIssueDetails);

            HshIn.Add("@intReleaseID", ReleaseID);
            HshIn.Add("@intDetailsID", DetailsID);
            HshIn.Add("@chvAttendentName", AttendentName);
            HshIn.Add("@intCrossMatchId", CrossMatchId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentIssueDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveComponetReturnDetails(int ReturnId, int HospitalLocationId, int FacilityId, string ReturnDate, string IssueNo,
                                                int AcceptedBy, int ReturnedBy, string Reason, string Remarks, int EncodedBy,
                                                string ComponentReturnDetails, int CrossMatchId, string BloodDiscartQuantity)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intReturnId", ReturnId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@dtReturnDate", ReturnDate);
            HshIn.Add("@chvIssueNo", IssueNo);
            HshIn.Add("@intAcceptedBy", AcceptedBy);
            HshIn.Add("@intReturnedBy", ReturnedBy);
            HshIn.Add("@chvReason", Reason);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@chvBloodDiscartQuantity", BloodDiscartQuantity);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlComponentReturnDetals", ComponentReturnDetails);
            HshIn.Add("@intCrossMatchId", CrossMatchId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponetReturnDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String UpdateComponentIssueDetails(string IssueNo, int HospitalLocationId, int FacilityId, string AcknowledgedDate, int AcknowledgedBy, string ComponentIssueDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chvIssueNo", IssueNo);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@dtAcknowledgedDate", AcknowledgedDate);
            HshIn.Add("@intAcknowledgedBy", AcknowledgedBy);
            HshIn.Add("@xmlComponentIssueDetails", ComponentIssueDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBUpdateComponentIssueDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveCounsellingCenterDetails(int ReferredCounselingCenterID, int HospitalLocationId, string ReferralTypeID, string CounsellingCenterName, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intReferredCounselingCenter", ReferredCounselingCenterID);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@chvReferralTypeID", ReferralTypeID);
            HshIn.Add("@chvCounsellingCenterName", CounsellingCenterName);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveReferredCounselingCenter", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public String SaveBloodTransferMain(int TransferMainID, int HospitalLocationId, int FacilityId, int CompanyFrom, int CompanyTo,
           string TransferredDate, int TransferredBy, int ComponentID, int Active, int EncodedBy, string TransferChildDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intTransferMainID", TransferMainID);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intCompanyFrom", CompanyFrom);
            HshIn.Add("@intCompanyTo", CompanyTo);
            HshIn.Add("@dtTransferredDate", TransferredDate);
            HshIn.Add("@intTransferredBy", TransferredBy);
            HshIn.Add("@intComponentId", ComponentID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlTransferChildDetails", TransferChildDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBloodTransferMain", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveSubComponentDetails(int SubComponentId, int HospitalLocationId, int FacilityId, int ComponentCombinationID, string ComponentCombinationName, int BagTypeID, string TempFirst,
            string TimeFirst, string RPMFirst, string TempSecond, string TimeSecond, string RPMSecond, string TempThird, string TimeThird, string RPMThird, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intSubComponentId", SubComponentId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intComponentCombinationID", ComponentCombinationID);
            HshIn.Add("@chvCombName", ComponentCombinationName);
            HshIn.Add("@intBagTypeID", BagTypeID);
            HshIn.Add("@chvTempFirst", TempFirst);
            HshIn.Add("@chvTimeFirst", TimeFirst);
            HshIn.Add("@chvRPMFirst", RPMFirst);
            HshIn.Add("@chvTempSecond", TempSecond);
            HshIn.Add("@chvTimeSecond", TimeSecond);
            HshIn.Add("@chvRPMSecond", RPMSecond);
            HshIn.Add("@chvTempThird", TempThird);
            HshIn.Add("@chvTimeThird", TimeThird);
            HshIn.Add("@chvRPMThird", RPMThird);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveSubComponentMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetSubComponentDetails(int HospitalLocationId, int FacilityId, int SubComponentID, int BagID, int ComponentCombinationId, string SearchText, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intSubComponentID", SubComponentID);
                HshIn.Add("@intBagID", BagID);
                HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
                HshIn.Add("@chvSearchText", SearchText);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetSubComponentDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComponentIssueDetails(int HospitalLocationId, int FacilityId, string CrossMatchNo, int Active, string CompleteRecordsOrPartial)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvCrossMatchNo", CrossMatchNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        //add new base c method to get list of employee
        public DataSet GetComponentIssueDetails(int HospitalLocationId, int FacilityId, string CrossMatchNo, int CrossMatchId, int Active, string CompleteRecordsOrPartial)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvCrossMatchNo", CrossMatchNo);
                HshIn.Add("@intCrossMatchId", CrossMatchId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }



        public DataSet GetBagNoFromStockForBulkIssue(int HospitalLocationId, int FacilityId, int ComponentId, string ComponentName, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@chvComponentName", ComponentName);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBagNoFromStockForBulkIssue", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetHospitalNamesForBulkIssue(int HospitalLocationId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetHospitalNamesForBulkIssue", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetCrossMatchedDetails(int HospitalLocationId, int FacilityId, string CrossMatchNo, int Active, string CompleteRecordsOrPartial)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvCrossMatchNo", CrossMatchNo);
                /*
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@intComponentId",ComponentId);
                */
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCrossMatchedCount(int HospitalLocationId, int FacilityId, int RequisitionId, int ComponentId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);

                /*
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@intComponentId",ComponentId);
                */
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedCount", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCrossMatchedDetails(int HospitalLocationId, int FacilityId, string CrossMatchNo, int Active, int CrossMatchId, string CompleteRecordsOrPartial)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvCrossMatchNo", CrossMatchNo);
                HshIn.Add("@intCrossMatchId", CrossMatchId);
                /*
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@intComponentId",ComponentId);
                */
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentIssueDetailsForReturning(int HospitalLocationId, int FacilityId, string ComponentIssueNo, int Active, string CompleteRecordsOrPartial)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueDetailsForReturning", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetComponentIssueDetailsForReturning(int HospitalLocationId, int FacilityId, string ComponentIssueNo, int Active, string CompleteRecordsOrPartial, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterID", EncounterID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueDetailsForReturning", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetComponentIssueDetailsForReturning(int HospitalLocationId, int FacilityId, string ComponentIssueNo, int Active, string CompleteRecordsOrPartial, string RegistrationNo, string RegistrationID, string EncounterNo, string EncounterID, int Acknowledged)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvComponentIssueNo", ComponentIssueNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intRegistrationID", RegistrationID);
                HshIn.Add("@intEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterID", EncounterID);
                HshIn.Add("@intAcknowledged", Acknowledged);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIssueDetailsForReturning", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveCrossMatchCancelDetails(string BagNo, int HospitalLocationId, int FacilityId, int CancelBy, int CrossMatchId, int EncodedBy, int ComponentID, int BloodGroupCode, int StockID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@BagNo", BagNo);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            //HshIn.Add("@dtReturnDate", CancelDate);
            HshIn.Add("@intCancelBy", CancelBy);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intCrossMatchId", CrossMatchId);
            HshIn.Add("@intComponentID", ComponentID);
            HshIn.Add("@intBloodGroupCode", BloodGroupCode);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intStockID", StockID);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveCrosMatchCancelDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetCrossMatchedDetails(int HospitalLocationId, int FacilityId, int RequisitionId, int ComponentId, int Active, string CompleteRecordsOrPartial)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequisitionId", RequisitionId);
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrCompleteRecordsOrPartial", CompleteRecordsOrPartial);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetMatchedDetailsWithRequestAndComponent", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public int CheckManulNoExistsOrNot(string ManualNo)
        {
            int check = 0;

            HshIn = new Hashtable();
            HshIn.Add("@chvManualNo", ManualNo);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                //Commented by rakesh start
                //string qry = "select ISNULL(count(ManualNo),0) from BBOpeningStockMain where ManualNo=" + ManualNo;
                //Commented by rakesh end
                //Added by rakesh start
                string qry = "select ISNULL(count(ManualNo),0) from BBOpeningStockMain WITH (NOLOCK) where ManualNo=@chvManualNo ";
                //Added by rakesh end
                object hold = objDl.ExecuteScalar(CommandType.Text, qry, HshIn);
                check = Int32.Parse(hold.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


            return check;
        }


        public DataSet GetOpeningStockDetails(int StockId, int Active)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intStockId", StockId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetOpeningStockDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetOpeningStockMain(int StockId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intStockId", StockId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetOpeningStockMain", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        //Add overload with 3 parameter
        public DataSet GetOpeningStockMain(int StockId, int Active, int Status)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intStockId", StockId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intStatus", Status);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetOpeningStockMain", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }
        //Add overload with 3 parameter
        public DataSet GetOpeningStockMain(int StockId, int Active, int Status, int ComponentId, int BloodGroupId, string ManualNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intStockId", StockId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intStatus", Status);
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@intBloodGroupId", BloodGroupId);
                HshIn.Add("@ManualNo", ManualNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetOpeningStockMain", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCrossMatchDetailValue()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                //                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //                string qry = @"select parameter_values_id,parameter_values from BBPhysicalParameterValues WITH (NOLOCK) where Status = 'A' and 
                //                                  Parameter_Code = (select Parameter_Code from BBPhysicalParameterMaster WITH (NOLOCK) where 
                //                                  Parameter_Type = 'S' and Parameter_Second_Type = 'C' and Parameter_Name = 'Anti-A')";
                //                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCrossMatchDetailValue", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public int CheckBagNoExistsOrNot(string BagNo, int ComponentId)
        {
            int check = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvBagNo", BagNo);
                HshIn.Add("@intComponentId", ComponentId);


                string qry = "select ISNULL(count(BagNo),0) from BBBagMaster WITH (NOLOCK) where BagNo like @chvBagNo ";
                string qry2 = "select ISNULL(count(BagNo),0) from BBOpeningStockDetails WITH (NOLOCK) where BagNo like @chvBagNo ";
                qry2 = "select ISNULL(count(BagNo),0) from BBOpeningStockDetails osd WITH (NOLOCK) inner join BBOpeningStockMain osm WITH (NOLOCK) on osm.StockId = osd.StockId where BagNo =@chvBagNo and ComponentId=@intComponentId ";

                object hold = objDl.ExecuteScalar(CommandType.Text, qry, HshIn);
                int tempCheck = Int32.Parse(hold.ToString());
                if (tempCheck == 0)
                {
                    hold = objDl.ExecuteScalar(CommandType.Text, qry2, HshIn);
                }

                check = Int32.Parse(hold.ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return check;
        }

        public DataSet GetDiscardedBagDetails(int BagDetailsId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intBagDetailsId", BagDetailsId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDiscardedBagDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDiscardedBagDetails(int BagDetailsId, int Active, string frmDate, string ToDate, string bagNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intBagDetailsId", BagDetailsId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@frmDate", frmDate);
                HshIn.Add("@toDate", ToDate);
                HshIn.Add("@bagno", bagNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDiscardedBagDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveBagDiscardDetails(int DetailsId, int BagDetailsId, int DiscardedQty,
            string Reason, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intDetailsId", DetailsId);
            HshIn.Add("@intBagDetailsId", BagDetailsId);
            HshIn.Add("@intDiscardedQty", DiscardedQty);
            HshIn.Add("@chvReason", Reason);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveBagDiscardDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetTableSchema(string TableName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvTableName", TableName);


                string qry = @"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS  WITH (NOLOCK) WHERE table_name = @chvTableName ORDER BY ORDINAL_POSITION ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetMasterFormCreator(string ModuleName, string TableName, int ColumnNamesOrFullTable)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvModuleName", ModuleName);
                HshIn.Add("@chvTableName", TableName);
                HshIn.Add("@intColsOrFullTable", ColumnNamesOrFullTable);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetMasterFormCreator", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveStaticGroupMaster(int GroupId, int HospitalLocationId, int FacilityId,
                              string GroupName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvGroupName", GroupName);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveStaticGroupsMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveStaticFieldsMaster(int FieldId, int HospitalLocationId, int FacilityId, int GroupId, string FieldName, string FieldValue,
           int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@chvFieldName", FieldName);
            HshIn.Add("@chvFieldValue", FieldValue);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveStaticFieldsMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetStaticFieldsMaster(int FieldId, int GroupId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intFieldId", FieldId);
                HshIn.Add("@intGroupId", FieldId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetStaticFieldsMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetStaticGroupsMaster(int GroupId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetStaticGroupsMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetPatientTransfusionDetails(int HospitalLocationId, int FacilityId, string EncounterNo, Int64 RegistrationNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intRegistrationNo", RegistrationNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetTransfusionDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientTransfusionDetail(int HospitalLocationId, int FacilityId, string CrossMatchNo, int CrossMatchId, int Active, int ComponentIssueId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvCrossMatchNo", CrossMatchNo);
                HshIn.Add("@intCrossMatchId", CrossMatchId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intComponentIssueId", ComponentIssueId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPatientTransfusionDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveMasterFormCreator(int MasterCreatorId, string ModuleName, string TableName, string ColumnName, string DisplayName, string DataType, string FieldType, int MaxLength,
                      int Rows, int Columns, string ListType, string StaticValues, string TableNameForDynamic, string DisplayNameForDynamic, string ValueNameForDynamic, int IsRequired, string RequiredMsg)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intCreatorID", MasterCreatorId);
            HshIn.Add("@chvModuleName", ModuleName);
            HshIn.Add("@chvTableName", TableName);
            HshIn.Add("@chvColumnName", ColumnName);
            HshIn.Add("@chvDisplayName", DisplayName);
            HshIn.Add("@chvDataType", DataType);
            HshIn.Add("@chvFieldType", FieldType);
            HshIn.Add("@intMaxLength", MaxLength);
            HshIn.Add("@intRows", Rows);
            HshIn.Add("@intColumns", Columns);
            HshIn.Add("@chvListType", ListType);
            HshIn.Add("@chvStaticValues", StaticValues);
            HshIn.Add("@chvTableNameForDynamic", TableNameForDynamic);
            HshIn.Add("@chvDisplayNameForDynamic", DisplayNameForDynamic);
            HshIn.Add("@chvValueNameForDynamic", ValueNameForDynamic);
            HshIn.Add("@bitIsRequired", IsRequired);
            HshIn.Add("@chvRequiredMsg", RequiredMsg);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveMasterFormCreator", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }




        public String SaveCounselingMain(int CounselingId, int HospitalLocationId, int FacilityId, string CounselingNo, int RegistrationId, int UnregisteredPatientId,
           DateTime CounselingDate, string AboutIllness, int IsAdmissionSuggested, double CostOfTreatment,
            DateTime NextExpectedVisit, int BedCategoryId, double DrugCharges, double ConsumableCharges, double MiscCharges, string Remarks, int Active, int UserId, string TreatmentServicesDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvCounselingNo", CounselingNo);
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
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            // HshIn.Add("@xmlRequestedBedCategoryDetails", RequestBedCategoryDetails);
            //  HshIn.Add("@xmlSuggestedTreatmentDetails", SuggestedTreatmentDetails);
            HshIn.Add("@xmlTreatmentServicesDetails", TreatmentServicesDetails);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCounselingMain", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public String SavePatientTransfusionDetails(int TransfusionID, int HospitalLocationId, int FacilityId,
string EncounterNo, long RegistrationNo, string IssueNo, string IssueDate, int BloodGroupId,
string Remarks, int EncodedBy, string TransfusionDetails)
        {
            return SavePatientTransfusionDetails(TransfusionID, HospitalLocationId, FacilityId,
             EncounterNo, RegistrationNo, IssueNo, IssueDate, BloodGroupId,
             Remarks, EncodedBy, TransfusionDetails, "");
        }

        public String SavePatientTransfusionDetails(int TransfusionID, int HospitalLocationId, int FacilityId,
            string EncounterNo, long RegistrationNo, string IssueNo, string IssueDate, int BloodGroupId,
            string Remarks, int EncodedBy, string TransfusionDetails, string IsTransfusion)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intTransfusionID", TransfusionID);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@chvIssueNo", IssueNo);
            HshIn.Add("@dtIssueDate", IssueDate);
            HshIn.Add("@intBloodGroupId", BloodGroupId);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@xmlTransfusionDetails", TransfusionDetails);
            HshIn.Add("@IsTransfusion", IsTransfusion);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSavePatientTransfusionDetails", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();

        }


        public DataSet GetCounselingRequestedBedCategoryDetails(int CounselingId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingRequestedBedCategoryDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetCounselingSuggestedTreatmentDetails(int CounselingId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingSuggestedTreatmentDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCounselingTreatmentServicesDetails(int CounselingId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intCounselingId", CounselingId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCounselingTreatmentServicesDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetSpecialization()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                string strsql = "SELECT ID, Name from SpecialisationMaster WITH (NOLOCK) where active=1";
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetDepartmentTypes(int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDepartmentTypes", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }




        public DataSet GetServicesBasedOnDepartmentTypes(int HospitalLocationId, int FacilityId, string DepartmentTypeIds, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDepartmentTypeId", DepartmentTypeIds);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetServiesBasedOnDepartmentTypes", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetDoctorNames(int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            StringBuilder qry = new StringBuilder();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);

                qry.Append("SELECT e.ID, ISNULL(tm.Name + ' ','') + ISNULL(FirstName,'') + ISNULL(' ' + middlename,'') + ISNULL(' ' + LastName,'') as FullName, e.Mobile ");
                qry.Append(" FROM employee e WITH (NOLOCK) ");
                qry.Append(" INNER JOIN users u WITH (NOLOCK) ON u.EmpID = e.ID ");
                qry.Append(" INNER JOIN SecUserFacility sc WITH (NOLOCK) ON sc.UserId=u.ID AND sc.FacilityId = @intFacilityId ");
                qry.Append(" LEFT OUTER JOIN TitleMaster tm WITH (NOLOCK) ON e.TitleId = tm.TitleID ");
                qry.Append(" WHERE e.EmployeeType = 1 AND e.active = 1 ORDER BY FullName ");

                return objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
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

        public DataSet GetBedDetails(int HospitalLocationId, int FacilityId, int Active)
        {
            return GetBedDetails(HospitalLocationId, FacilityId, Active, false);
        }

        public DataSet GetBedDetails(int HospitalLocationId, int FacilityId, int Active, bool bIsBillingCategory)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@bitIsBillingCategory", bIsBillingCategory);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBedDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetBindSurgeryOrderDetails(int OrderId, int HospId, int FacilityId, int SurgeryType)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intHospitalId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intOrderId", OrderId);
                HshIn.Add("@intSurgeryType", SurgeryType);
                return objDl.FillDataSet(CommandType.Text, @"select soda.ServiceId,  case when @intSurgeryType=1 then 'Surgery' else 'Package' end as 'SurgeryType', ios.ServiceName 'SurgeryName',  emp.FirstName as 'SurgeryProviderName',soda.AmountPayable as 'PayableAmount' from ServiceOrderDetailAmount soda WITH (NOLOCK)
                inner join ServiceOrderDetail sod WITH (NOLOCK) on soda.ServiceOrderDetailId=sod.Id
                inner join ItemOfService ios WITH (NOLOCK) on soda.ServiceId=ios.ServiceId
                left join employee emp WITH (NOLOCK) on emp.ID = sod.DoctorId
                where soda.OrderID=@intOrderId
                order by soda.Id", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetTreatmentServiceDetails(int CounselingID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@CounselingID", CounselingID);
                HshIn.Add("@Active", 1);

                string qry = @"select SuggestedServiceId, ResourceType, ServiceCharges, ServiceType
                                from CounselingTreatmentServices WITH (NOLOCK) where CounselingId=@CounselingID
                                and active=@Active";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetDiscardedByEmployeeList(int HospitalLocationID, int FacilityID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@HospitalLocationID", HospitalLocationID);
                HshIn.Add("@FacilityID", FacilityID);

                string qry = @"select ID,ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') as 'EmployeeName' FROM employee WITH (NOLOCK) where DepartmentId=1254 and EmployeeType=2 and Active=1 and FacilityID=@FacilityID and HospitalLocationId=@HospitalLocationID ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetBagDetailsForComponentDiscard(int HospitalLocationID, int FacilityID, int ComponentID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@HospitalLocationID", HospitalLocationID);
                HshIn.Add("@FacilityID", FacilityID);
                HshIn.Add("@ComponentID", ComponentID);

                string qry = @"SELECT ActualBagno, CONVERT(varchar,Stock_Id) + '-' + CONVERT(varchar,RTRIM(GroupCode)) + '-' + CONVERT(varchar,QtyReceived) as 'StockGroupIDQty' FROM BBBloodStock WITH (NOLOCK)
                            WHERE QtyIssued=0 and QtyDiscard=0 and ComponentCode=@ComponentID and FacilityId=@FacilityID and HospitalLocationId=@HospitalLocationID ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }



        public String ChangeCounselingStatus(int CounselingId, int Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intCounselingId", CounselingId);
            HshIn.Add("@bitActive", Active);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBChangeCounselingStatus", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetUnregisteredPatientDetails(int UnregisteredPatientID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();


                HshIn.Add("@intUnregisteredID", UnregisteredPatientID);
                string qry = @"SELECT Id,HospitalLocationId,FacilityID,TitleId,FirstName,MiddleName,LastName,
                            DateofBirth,AgeYear,AgeMonth,AgeDays,Gender,MaritalStatus,LocalAddress,LocalAddress2,
                            Localpin,LocalCity,LocalState,LocalCountry,PhoneHome,MobileNo,Email,ReligionID,NationalityID,
                            LanguageId,ResidentType,EncodedBy,EncodedDate,LastChangedBy,LastChangedDate,CityAreaID
                            FROM dbo.TempCounselingUnregisteredPatientDetails WITH (NOLOCK) where Id=@intUnregisteredID";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetComponentList(int ComponentCombinationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
                string qry = "SELECT bcm.ComponentId, bcm.ComponentName,(CASE WHEN ISNULL(ccd.ComponentId, 0) = 0 THEN 0 ELSE 1 END) IsChk"
                   + " FROM BBBloodComponentMaster bcm WITH (NOLOCK) LEFT OUTER JOIN BBComponentCombinationDetails ccd WITH (NOLOCK) on bcm.ComponentId = ccd.ComponentId AND ccd.Active =1 AND ccd.ComponentCombinationId=@intComponentCombinationId";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public String SavePhysicalExaminationPatientDetails(int PhysicalExaminationId, int HospitalLocationId, int FacilityId, string DonorRegistrationNo, string AcceptedOrRejected,
           int DoneBy, string Remarks, int Active, int UserId, string ParameterDetails, int BloodGroupId, string deferredType, int DeferReason)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();


            HshIn.Add("@intPhysicalExaminationId", PhysicalExaminationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationNo);
            HshIn.Add("@chvAcceptedOrRejected", AcceptedOrRejected);
            HshIn.Add("@intDoneBy", DoneBy);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlParameterDetails", ParameterDetails);
            HshIn.Add("@BloodGroupId", BloodGroupId);
            HshIn.Add("@deferredType", deferredType);
            HshIn.Add("@intDeferReason", DeferReason);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSavePaitentPhysicalExaminationDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public String SaveComponentDiscardDetails(int DiscardID, int HospitalLocationId, int FacilityId, int DiscardNo, DateTime DiscardDate,
            int DiscardReasonID, string Remarks, int EmpCode, int AuthorisedByID, string AutoclavingDone, DateTime AutoClavingDate, int Active, int UserId, string DiscardChildDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();


            HshIn.Add("@intDiscardID", DiscardID);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intDiscardNo", DiscardNo);
            HshIn.Add("@dtDiscardDate", DiscardDate);
            HshIn.Add("@intDiscardReasonID", DiscardReasonID);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intEmpCode", EmpCode);
            HshIn.Add("@intAuthorisedByID", AuthorisedByID);
            HshIn.Add("@chvAutoclavingDone", AutoclavingDone);
            HshIn.Add("@dtAutoClavingDate", AutoClavingDate);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlComponentDiscardDetails", DiscardChildDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentDiscardDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPhysicalExaminationPatientDetails(int HospitalLocationId, int FacilityId, int Active, int ExaminationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intExaminationID", ExaminationID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPaitentPhysicalExaminationDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPhysicalExaminationPatientDetails(int HospitalLocationId, int FacilityId, int Active, int ExaminationID, int Status)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intExaminationID", ExaminationID);
                HshIn.Add("@intStatus", Status);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPaitentPhysicalExaminationDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetPatientExaminationPatientParameterDetails(int PhysicalExaminationID, string Screen_Type, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intPhysicalExaminationID", PhysicalExaminationID);
                HshIn.Add("@chrScreen_Type", Screen_Type);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBPatientExaminationParameterDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet GetWhomToDonationInformation(int HospitalLocationId, int FacilityId, int EncounterID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@EncounterID", EncounterID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetWhomToDonationInformation", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


            //             
            //            try
            //            {
            //                HshIn = new Hashtable();
            //                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //                HshIn.Add("@RegNo", RegistrationNO);
            //                string qry = @"select FirstName+' '+MiddleName+' '+LastName as 'Name',DateofBirth,BloodGroupId,ReferredByID, admission.WardID CurrentWardId, 
            //                                WardMaster.WardNo CurrentWardNo,
            //                                CurrentBedNo from registration 
            //                                left join Encounter on registration.Id=Encounter.RegistrationId
            //                                left join admission on admission.RegistrationId = registration.Id
            //                                left join WardMaster on WardMaster.WardId = admission.WardID
            //                                where registration.RegistrationNo=@RegNo";
            //                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            //            }
            //            catch (Exception Ex)
            //            {
            //                throw Ex;
            //            }
            //            return ds;
        }
        public DataSet GetBloodStock(int HospitalLocationId, int FacilityId, int ComponentID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@ComponentID", ComponentID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodStock", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet GetBloodStock()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        //                string qry = @"SELECT Stock_Id, ActualBagno, BagNumber, bgm.BloodGroupDescription, bcm.ComponentName, QtyReceived, CollectionDate, ExpiryDate FROM BBBloodStock bs
        //        //                                INNER JOIN BBBloodComponentMaster bcm ON bs.ComponentCode=bcm.ComponentId
        //        //                                INNER JOIN BBBloodGroupMaster bgm ON bs.GroupCode=bgm.BloodGroupId 
        //        //                                LEFT join BBComponentIssueDetails BBCID on BBCID.UnitNo = bs.BagNumber
        //        //                                WHERE ISNULL(bs.QtyIssued,0)=0 AND ISNULL(QtyDiscard,0)=0 AND bs.ACTIVE=1
        //        //                                And bs.BagNumber not in (select UnitNo from BBComponentIssueDetails)";

        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodStock", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        public DataSet GetPatientDetailsUsingRegistrationId(long RegNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationNo", RegNo);
                string str = @"select bm.BedNo,CurrentBedId, wm.WardName, CurrentWardID, Diagnosis, ConsultingDoctorId from Registration pr WITH (NOLOCK) INNER JOIN Admission ad WITH (NOLOCK) ON pr.Id=ad.RegistrationId 
                           INNER JOIN BedMaster bm WITH (NOLOCK) on CurrentBedId=bm.Id
                           INNER JOIN WardMaster wm WITH (NOLOCK) on CurrentWardID=wm.WardId
                           where pr.RegistrationNo=@intRegistrationNo and ad.PatadType not in('D','C') and ad.Active=1 ";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }




        public string SaveKitTypeMaster(int KitTypeId, int HospitalLocationId, int FacilityId,
                               string KitTypeName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intKitTypeId", KitTypeId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvKitName", KitTypeName);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveKitTypeMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getKitTypeMaster(int KitTypeId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intKitTypeId", KitTypeId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitTypeMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveKitReceivingEntry(int KitId, int HospitalLocationId, int FacilityId, int KitTypeId, string KitName, string LotNo,
            DateTime ManufactureDate, DateTime ExpiryDate, int NoOfKit, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intKitId", KitId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intKitTypeId", KitTypeId);
            HshIn.Add("@chvKitName", KitName);
            HshIn.Add("@chvLotNo", LotNo);
            HshIn.Add("@dtManufactureDate", ManufactureDate);
            HshIn.Add("@dtExpiryDate", ExpiryDate);
            HshIn.Add("@intNoOfKits", NoOfKit);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveKitReceivingEntry", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetKitReceivingEntry(int KitId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intKitId", KitId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitReceivingEntry", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetKitReceivingEntry(int KitId, int HospitalLocationId, int FacilityId, int Active, string KitName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intKitId", KitId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@varchKitName", KitName);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitReceivingEntry", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetKitReceivingEntryByKitTypeId(int KitTypeId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intKitTypeId", KitTypeId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetKitReceivingEntryByKitTypeId", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getBloodBankTransactionType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();


                string qry = " SELECT StatusId, [Status], StatusType as Type, Code FROM StatusMaster WITH (NOLOCK) " +
                             " WHERE StatusType IN ('BBDocumentType') " +
                             " AND Active = 1 ORDER BY Status, SequenceNo ";

                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        //        public DataSet getPhysicalExaminationParameterDetails()
        //        {
        //            DataSet ds = new DataSet();
        //            try
        //            {
        //                HshIn = new Hashtable();
        //                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //                //commented by rakesh start
        //                //string qry = @"select Parameter_Code,Parameter_Name,Parameter_Head_Name, RenderControl from bbphysicalparametermaster
        //                //where Parameter_Type='p' and status='A' ORDER BY Print_Sequence ";
        //                //commented by rakesh start
        //                //Added by rakesh start
        //                string qry = @"select Parameter_Code,Parameter_Name,Parameter_Head_Name, RenderControl,Identification from bbphysicalparametermaster
        //                            where Parameter_Type='p' and status='A' ORDER BY Print_Sequence ";
        //                //Added by rakesh end
        //                ds = objDl.FillDataSet(CommandType.Text, qry);
        //            }
        //            catch (Exception Ex)
        //            {
        //                throw Ex;
        //            }
        //            return ds;
        //        }

        public DataSet getPhysicalExaminationParameterDetails()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                //HshIn.Add("@intFacilityId", FacilityId);
                //HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationNo);
                //HshIn.Add("@dtStartDate", StartDate);
                //HshIn.Add("@dtEndDate", EndDate);
                //HshIn.Add("@bitActive", Active);


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhysicalExaminationParameterDetails", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDonorPhlebotomyDetails(int HospitalLocationId, int FacilityId, string DonorRegistrationNo, int Active, string StartDate, string EndDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationNo);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDonorPhlebotomyDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBagDetails(int Active, int HospitalLacationID, int FacilityId, int BagTypeId, int BagCapacity, int BagId, string BagBatchNo, int CallingAction)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intBagTypeId", BagTypeId);
                HshIn.Add("@intBagCapacity", BagCapacity);
                HshIn.Add("@intBagId", BagId);
                HshIn.Add("@chvBagBatchNo", BagBatchNo);
                HshIn.Add("@intAction", CallingAction);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBBBagDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetAnticoagulantMaster(int Active, int HospitalLacationID, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intHospitalLocationId", HospitalLacationID);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetAnticoagulantMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDeferReasonMaster(int Active, int HospitalLacationID, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intHospitalLocationId", HospitalLacationID);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBBDeferReasonMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SavePhlebotomy(string DonorRegistrationno, int HospitalLocationId, int FacilityId,
       string BagNumber, int BagUsedId, int BagQtyId, int BagMakeId, string BagBatchNo, int AnticoagulantID
       , string SegmentNo, int bitStatus, int userId
       , DateTime DonationDate, DateTime BagExpDate, int ReceivedQty, string Remarks, int RejectionReasonID
       , out string docNo, out int oRequestingID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            oRequestingID = 0;
            docNo = "";

            HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationno);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityID", FacilityId);
            HshIn.Add("@chvBagNo", BagNumber);
            HshIn.Add("@BagUsedId", BagUsedId);
            HshIn.Add("@dtDonationDate", DonationDate);
            HshIn.Add("@BagQtyId", BagQtyId);
            HshIn.Add("@BagMakeId", BagMakeId);
            HshIn.Add("@BagBatchNo", BagBatchNo);
            HshIn.Add("@AnticoagulantID", AnticoagulantID);
            HshIn.Add("@SegmentNo", SegmentNo);
            HshIn.Add("@dtBagExpDate", BagExpDate);
            HshIn.Add("@intReceivedQty", ReceivedQty);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intRejectionReasonID", RejectionReasonID);
            HshIn.Add("@bitStatus", bitStatus);
            HshIn.Add("@intEncodedBy", userId);
            HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
            HshOut.Add("@intRequestingCode", SqlDbType.Int);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhlebotomy", HshIn, HshOut);

                string msg = Convert.ToString(HshOut["@intRequestingCode"]);
                if (msg != "")
                {
                    oRequestingID = Convert.ToInt32(HshOut["@intRequestingCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }
                docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SavePhlebotomy(string DonorRegistrationno, int HospitalLocationId, int FacilityId,
         string BagNumber, int BagUsedId, int BagQtyId, int BagMakeId, string BagBatchNo, int AnticoagulantID
         , string SegmentNo, int bitStatus, int userId
         , DateTime DonationDate, DateTime BagExpDate, int ReceivedQty, string Remarks, int RejectionReasonID
         , out string docNo, out int oRequestingID,
             //Added by rakesh start
             int AcceptedYesNo)
        //Added by rakesh end
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            oRequestingID = 0;
            docNo = "";

            HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationno);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityID", FacilityId);
            HshIn.Add("@chvBagNo", BagNumber);
            HshIn.Add("@BagUsedId", BagUsedId);
            HshIn.Add("@dtDonationDate", DonationDate);
            HshIn.Add("@BagQtyId", BagQtyId);
            HshIn.Add("@BagMakeId", BagMakeId);
            HshIn.Add("@BagBatchNo", BagBatchNo);
            HshIn.Add("@AnticoagulantID", AnticoagulantID);
            HshIn.Add("@SegmentNo", SegmentNo);
            HshIn.Add("@dtBagExpDate", BagExpDate);
            HshIn.Add("@intReceivedQty", ReceivedQty);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intRejectionReasonID", RejectionReasonID);
            HshIn.Add("@bitStatus", bitStatus);
            HshIn.Add("@intEncodedBy", userId);
            HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
            HshOut.Add("@intRequestingCode", SqlDbType.Int);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            //Added by rakesh start
            HshIn.Add("@AcceptedYesNo", AcceptedYesNo);
            //Added by rakesh end


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhlebotomy", HshIn, HshOut);

                string msg = Convert.ToString(HshOut["@intRequestingCode"]);
                if (msg != "")
                {
                    oRequestingID = Convert.ToInt32(HshOut["@intRequestingCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }
                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetPhlebotomyDoneList(int HospitalLocationId, int FacilityId, int DonorRegistrationId, int Active, string StartDate, string EndDate, int BloodGroupID, string RegNo, string BagNo)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        Hashtable HshIn = new Hashtable();
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
        //        HshIn.Add("@dtStartDate", StartDate);
        //        HshIn.Add("@dtEndDate", EndDate);
        //        HshIn.Add("@intBloodGroupID", BloodGroupID);
        //        HshIn.Add("@chvRegNo", RegNo);
        //        HshIn.Add("@chvBagNo", BagNo);
        //        HshIn.Add("@bitActive", Active);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPhlebotomyDoneList", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        public DataSet GetPhlebotomyDoneList(int HospitalLocationId, int FacilityId, int DonorRegistrationId, int Active, string StartDate, string EndDate, int BloodGroupID, string RegNo, string BagNo, int isPhlebotomy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@intBloodGroupID", BloodGroupID);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvBagNo", BagNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@isPhlebotomy", isPhlebotomy);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetPhlebotomyDoneList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetComponentDivisionRecord(int Id, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intID", Id);
                HshIn.Add("@intHospitalLacationID", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetComponentDivisionRecord", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public DataSet GetDetail(int DonorNo, int Active, DateTime FromDate, DateTime ToDate, int FacilityId, int HospitalLacationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn = new Hashtable();
                HshIn.Add("@DonorNo", DonorNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetComponentDivision", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDetailOfComp(int DonorId, int Active, DateTime FromDate, DateTime ToDate, int FacilityId, int HospitalLacationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn = new Hashtable();
                HshIn.Add("@DonorId", DonorId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetComponentDivisionOfComp", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPhlebotomyDetails(int DonorNo, int Active, DateTime FromDate, DateTime ToDate, int FacilityId, int HospitalLacationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@DonorNo", DonorNo);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhlebotomyDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveComponentDivision(int ComponentDivisionID, int HospitalLocationId, int FacilityId,
            string Bag_Number, int BloodGroupID, DateTime CollectionDate, int ComponentCombinationID, int MixtureID
            , string xmlReceivingItems, int bitStatus, int userId, out string docNo, out int oRequestingID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            oRequestingID = 0;
            docNo = "";

            HshIn.Add("@intComponentDivisionID", ComponentDivisionID);

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityID", FacilityId);
            HshIn.Add("@chvBag_Number", Bag_Number);
            HshIn.Add("@intBloodGroupID", BloodGroupID);
            HshIn.Add("@Collection_Date", CollectionDate);
            HshIn.Add("@intComponentCombinationID", ComponentCombinationID);
            HshIn.Add("@intMixtureID", MixtureID);
            HshIn.Add("@xmlReceivingItems", xmlReceivingItems);
            HshIn.Add("@bitStatus", bitStatus);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
            HshOut.Add("@intCompDivCode", SqlDbType.Int);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveComponentDivision", HshIn, HshOut);

                string msg = Convert.ToString(HshOut["@intCompDivCode"]);
                if (msg != "")
                {
                    oRequestingID = Convert.ToInt32(HshOut["@intCompDivCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentCombName(int ID, int Active, int FacilityId, int HospitalLacationID, int BagUsedId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@Id", ID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                HshIn.Add("@BagUsedId", BagUsedId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBB_ComponentCombName", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentMixture(int ID, int Active, int FacilityId, int HospitalLacationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@Id", ID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBBComponentMixture", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBBComponentCombDetails(int ID, int Active, int FacilityId, int HospitalLacationID, int BagQty, int CompDivID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn = new Hashtable();
                HshIn.Add("@Id", ID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLacationID", HospitalLacationID);
                HshIn.Add("@BagQty", BagQty);
                HshIn.Add("@CompDivID", CompDivID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBB_ComponentCombDetails", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBBComponentDivisionMasterList(int Id,
                                            DateTime FromDate, DateTime ToDate, int Active
                                            , int HospId, int FacilityId, int EncodedBy)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("@intId", Id);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@bitStatus", Active);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBBComponentDivisionMasterList", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetBBComponentDivisionMasterList(int Id,
                                            DateTime FromDate, DateTime ToDate, int Active
                                            , int HospId, int FacilityId, int EncodedBy, string BagNumber, int BloodGroup)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("@intId", Id);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@bitStatus", Active);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@chvBagNumber", BagNumber);
            HshIn.Add("@intBloodGroup", BloodGroup);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBBComponentDivisionMasterList", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        //for screening

        //public DataSet Screening(string screeningType, string action, int donorNo, int DonorTypeId)
        public DataSet Screening(string screeningType, string action, int donorNo)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Parameter_Second_Type", screeningType);
                HshIn.Add("@chvOption", action);
                HshIn.Add("@intDonorNo", donorNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBInfectiousScreeningCompDiv", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet ParameterCodeValue(string option, int Active, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@Parameter_Code", option);
                return objDl.FillDataSet(CommandType.StoredProcedure, "BBInfectiousScreeningParameterCode", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveInfectiousScreening(int DonorNo, int HospitalLocationId, int FacilityId,
            string objXML, string ScreeningType, int userId, out int oRequestingID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            oRequestingID = 0;

            HshIn.Add("@intDonorNo", DonorNo);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityID", FacilityId);
            HshIn.Add("@objXML", objXML);
            HshIn.Add("@ScreeningType", ScreeningType);
            HshIn.Add("@intEncodedBy", userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveInfectiousScreening", HshIn, HshOut);

                string msg = Convert.ToString(HshOut["@intCompDivCode"]);
                if (msg != "")
                {
                    oRequestingID = Convert.ToInt32(HshOut["@intCompDivCode"]);

                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSelectParameterCodeAndParameterValue(int Id, string ScreenType, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@PhysicalExaminationID", Id);
                HshIn.Add("@ScreenType", ScreenType);
                HshIn.Add("@intHospitalLacationID", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetSelectParameterCodeAndParameterValue", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetInfectiousScreeningDoneList(int HospitalLocationId, int FacilityId, string ScreenType, int Active, string StartDate, string EndDate)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ScreenType", ScreenType);
            HshIn.Add("@dtStartDate", StartDate);
            HshIn.Add("@dtEndDate", EndDate);
            HshIn.Add("@bitActive", Active);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBInfectiousScreeningDoneList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetInfectiousScreeningDoneList(int HospitalLocationId, int FacilityId, string ScreenType, int Active, string StartDate, string EndDate, string RegNo, string BagNo)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ScreenType", ScreenType);
            HshIn.Add("@dtStartDate", StartDate);
            HshIn.Add("@dtEndDate", EndDate);
            HshIn.Add("@dtRegNo", RegNo);
            HshIn.Add("@dtBagNo", BagNo);
            HshIn.Add("@bitActive", Active);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBInfectiousScreeningDoneList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //add by Balkishan start

        public string SaveInstructionMaster(int InstructionId, int HospitalLocationId, int FacilityId, string Instruction, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intInstructionId", InstructionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvInstruction", Instruction);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveInstructionMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getInstructionMaster(int InstructionId, int HospitalLocationId, int FacilityId, int Active)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intInstructionId", InstructionId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetInstructionMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        public string SaveComponentCombinationInstructionTagging(int ComponentCombinationId, string InstructionIds,
                                int Active, int UserId, int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intComponentCombinationId", ComponentCombinationId);
            HshIn.Add("@xmlInstructionIds", InstructionIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentCombinationInstructionTagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentCombinationInstructionTagging(int ComponentId, int Active, int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentCombinationInstructionTagging", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetComponentChargedServiceTagging(int ComponentId, int Active, int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentChargedServiceTagging", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetComponentChargedServiceForBloodIssue(int ComponentId, int Active, int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentChargedServiceForBloodIssue", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        //add by Balkishan end

        //Added by rakesh for checking invoice generating for the order or not on 6/10/2014 start
        public int IsInvoiceGeneratedForOrder(int OrderId, int HospitalLocationId, int FacilityId)
        {
            int result = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@OrderId", OrderId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                result = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspIsInvoiceGeneratedForOrder", HshIn));
                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int IsOrderCancelledOfRequisitionID(int OrderId)
        {
            int result = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@OrderId", OrderId);
                result = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspCancelOrderOfRequisitionID", HshIn));
                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        //Added by rakesh for checking invoice generating for the order or not on 6/10/2014 end

        public int IsPatientAdmitted(int RegID, int HospitalLocationId, int FacilityId)
        {
            int result = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                result = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspBBIsPatientAdmitted", HshIn));
                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable CreateEncounterIFNotExistsForBloodBank(Int32 iHospitalLocationId, Int32 iFacilityId, Int64 iRegistrationId, Int32 iRegistrationNo, Int32 iEncodedBy, Int32 iDoctorId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("intRegistrationNo", iRegistrationId);
            HshIn.Add("intRegistrationId", iRegistrationNo);
            HshIn.Add("intEncounterDoctorId", iDoctorId);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("intEncounterID", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBCreateEncounterIFNotExistsForBloodBank", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelComponentRequisition(int RequisitionId, int HospitalLocationId, int FacilityId, int UserId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            //result = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspBBCancelComponentRequisition", hstInput)); //
            //return HshOut["@chvErrorStatus"].ToString();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBCancelComponentRequisition", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetRegistrationId(Int64 RegNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationNo", RegNo);
                string str = " SELECT pr.Id,EN.Id AS EncounterId,EN.EncounterNo,ad.AdmissionDate,EN.EREncounterId,ad.AdmittingDoctorId AS DoctorId FROM Registration pr WITH (NOLOCK) INNER JOIN Admission ad WITH (NOLOCK) ON PR.Id=ad.RegistrationId " +
                              " INNER JOIN Encounter EN WITH (NOLOCK) ON AD.EncounterId=EN.Id WHERE pr.RegistrationNo=@intRegistrationNo AND ad.Active=1 ";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public string BBSelectDonorTypeID(int HospitalLocationId, int FacilityId, int SelectedValue)
        {
            string result = string.Empty;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@SelectedValue", SelectedValue);
                result = (string)objDl.ExecuteScalar(CommandType.StoredProcedure, "USPBBSelectDonorTypeID", HshIn);
                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet BindExaminationParameterDetailsFromDonor(int HospitalLocationId, int FacilityId, int DonorRegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDonorRegistrationId", DonorRegistrationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPBindExaminationParameterDetailsFromDonor", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string IsDonorExists(int HospitalLocationId, int FacilityId, string DonorRegistrationNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDonorRegistrationNo", DonorRegistrationNo);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBIsDonorExists", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetBloodComponentMaster()
        {
            string query = "Select ComponentId,ComponentName from BBBloodComponentMaster with (nolock) where parentId = 0";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return (objDl.FillDataSet(CommandType.Text, query)).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentCoversionValidation()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetComponentCoversionValidation");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveComponentCoversionValidation(int iHospitalLocationId, int iFacilityId, int iComponentId, int iConversionComponentId, int iEncodedBy, int iId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intComponentId", iComponentId);
                HshIn.Add("@intConversionComponentId", iConversionComponentId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@intId", iId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveComponentCoversionValidation", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string DeleteComponentCoversionValidation(int iHospitalLocationId, int iFacilityId, int iId, int iDeletedby)
        {
            return SaveComponentCoversionValidation(iHospitalLocationId, iFacilityId, 0, 0, iDeletedby, iId);
        }

        public DataSet GetEmergencyBloodRequisitionType(int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetEmergencyBloodRequisitionType", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReferFromMaster(int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();


                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetReferFromMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveHospitalMaster(int BagId, int HospitalLocationId, int FacilityId,
                               string BagName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intHospitalId", BagId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvHospitalName", BagName);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveHospitalMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetHospitalMaster(int HospitalId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intHospitalId", HospitalId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetHospitalMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet GetHospitalMaster()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        string qry = "SELECT HospitalId, HospitalName from BBHospitalMaster WITH (NOLOCK) where active=1 order by sequence";
        //        ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}
        public DataSet GetHospitalList(int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetHospitalMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveLocationMaster(int BagId, int HospitalLocationId, int FacilityId,
                               string BagName, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intLocationId", BagId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvLocationName", BagName);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveLocationMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLocationMaster(int HospitalId, int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intLocationId", HospitalId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetLocationMaster", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetComponentLists(int HospitalLocationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveComponentAndChargedServiceForTTITagging(int ComponentId, string InstructionIds,
                                int Active, int UserId, int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intComponentId", ComponentId);
            HshIn.Add("@xmlInstructionIds", InstructionIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentAndChargedServiceForTTITagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComponentIrradiatedChargedServiceTagging(int ComponentId, int Active, int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intComponentId", ComponentId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetComponentIrradiatedChargedServiceTagging", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveComponentAndChargedServiceForIrradiatedTagging(int ComponentId, string InstructionIds,
                                int Active, int UserId, int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intComponentId", ComponentId);
            HshIn.Add("@xmlInstructionIds", InstructionIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentAndChargedServiceForIrradiatedTagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveComponentAndChargedServiceForBloodIssueTagging(int ComponentId, string InstructionIds,
                                int Active, int UserId, int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intComponentId", ComponentId);
            HshIn.Add("@xmlInstructionIds", InstructionIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveComponentAndChargedServiceForBloodIssueTagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public String SaveOrUpdateRequisitionCancel(int RequisitionId, int HospitalLocationId, int FacilityId,
            int SampleAcknowledged, int SampleAcknowledgedBy, int UserId, string CancelRemarks)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intRequisitionId", RequisitionId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intSampleAcknowledgedForTTI", SampleAcknowledged);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@chrCancelRemarks", CancelRemarks);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveOrUpdateRequisitionCancel", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetSegmentNoFromBagNumber(string BagNo, int HospitalLocationId, int FacilityId)
        {
            string result = string.Empty;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@BagNo", BagNo);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                result = Convert.ToString((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspBBGetSegmentNoFromBagNumber", HshIn));
                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetReasontype(string Reason)
        {

            StringBuilder strSQL = new StringBuilder();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            strSQL.Append("select Reason, Id from ReasonMaster Where ReasonType = '" + Reason + "' and Active=1");
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; strSQL = null; }


        }
        public DataSet GetAdverseTransfusionParameterDetails()
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetAdverseTransfusionParameterDetails", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public String SaveAdverseTransfusionReactionWorkupReport(int PhysicalExaminationId, int HospitalLocationId, int FacilityId, string DonorRegistrationNo, string AcceptedOrRejected,
           int DoneBy, string Remarks, int Active, int UserId, string ParameterDetails, int BloodGroupId, string deferredType, int DeferReason)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intIssueId", PhysicalExaminationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvPrestTranBloodGroup", DonorRegistrationNo);
            HshIn.Add("@chvPostTranBloodGroup", AcceptedOrRejected);
            HshIn.Add("@chvCompatibilitymonor", DoneBy);
            HshIn.Add("@chvConclusion", Remarks);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@xmlParameterDetails", ParameterDetails);





            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveAdverseTransfusionReactionWorkupReport", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet InfectiousUnScreening(string screeningType, string action, int donorNo)
        public DataSet InfectiousUnScreening()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@option", "");
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBunscreening", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet InfectiousUnScreening(string option, string Bagno, string Donorregno, string name)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@option", option);
                HshIn.Add("@BagNo", Bagno);
                HshIn.Add("@DonorRegistrationNo", Donorregno);
                HshIn.Add("@Donorname", name);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBunscreening", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet InfectiousUnScreeningCheckboxOrderBind()
        {



            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBInfectiousUnScreeningCheckboxOrderBind", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public String SaveScreeningOrder(string inyHospitalLocationId, string xmlDonorRegistrationIds, string xmlScreeningParameters, string intEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@xmlDonorRegistrationIds", xmlDonorRegistrationIds);
            HshIn.Add("@xmlScreeningParameters", xmlScreeningParameters);
            HshIn.Add("@intEncodedBy", intEncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBSaveScreeningOrder", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet InfectiousUnScreeningCheckboxEnableDisable(string strDonorRegistrationId, string strBagNo)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {
                HshIn.Add("@DonorRegistrationId", strDonorRegistrationId);
                HshIn.Add("@BagNo", strBagNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBCheckDonorScreeningOrder", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet ScreenOrderDoneList(string strDonorRegistrationid, string strBagNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();


            try
            {
                HshIn.Add("@intDonorRegistrationId", strDonorRegistrationid);
                HshIn.Add("@chvBagNo", strBagNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetScreeningOrderDetail", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public String UpdateScreeningOrder(string strinyHospitalLocationId, string strintEncodedBy, string strID, string strDonorRegistrationId, string strBagno, string strcancelreason)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", strinyHospitalLocationId);
            HshIn.Add("@intEncodedBy", strintEncodedBy);
            HshIn.Add("@ID", strID);
            HshIn.Add("@intDonorRegistrationId", strDonorRegistrationId);
            HshIn.Add("@Bagno", strBagno);
            HshIn.Add("@chvCancelReason", strcancelreason);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBUpdateScreeningOrder", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetValidateBloodGroupCellScreenDropdown(string BloodGrp)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@BloodGroup", BloodGrp);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetValidBloodGroup", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetPhysicalCellScreenInterpretationGroupDropdown(string code, string BloodGrp)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Parameter_Code", code);
                HshIn.Add("@BloodGroup", BloodGrp);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDefaultValueofBBPhysicalParameterValuesCell", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public string Update_Blood_Group(int DonorNo, int HospitalLocationId, int FacilityId, int BloodGroup)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDonorNo", DonorNo);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@BloodGroupID", BloodGroup);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "BBuspupdatebloodgroup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string Validate_Donor_Registration(string Fname, string Mname, string Lname)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@FirstName", Fname);
            HshIn.Add("@MiddleName", Mname);
            HshIn.Add("@LastName", Lname);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspBBValidateDonor", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        #region "SPS"
        public DataSet GetDoctorList(int HospitalId, int intSpecialisationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("HospitalLocationId", HospitalId);
            hshInput.Add("intSpecialisationId", intSpecialisationId);
            hshInput.Add("intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", hshInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDoctorIDAdmissionTime(int HospitalId, Int64 intRegistrationNo, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@HospitalLocationId", HospitalId);
            hshInput.Add("@RegistrationNo", intRegistrationNo);
            hshInput.Add("@FacilityId", FacilityId);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetDoctorID", hshInput);
        }
        public DataSet GetAutoGenerateAckNumber(int HospitalId, int FacilityId, string DocumentType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", HospitalId);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@DocumentType", DocumentType);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetAckAutoGenerateNumber", hshInput);
        }

        public DataSet GetBloodIssueDetailsWithServiceID(int HospitalLocationId, int FacilityId, int Active, int RegistrationId, string RegistrationNo, int EncounterId, int Issue, string EncounterNo, string PatientName, string RequestType, string BagNo, string StartDate, string EndDate)
        {

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@bitIssue", Issue);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvRequestType", RequestType);
                HshIn.Add("@chvBagNo", BagNo);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetCrossedMatchedDetails", HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBGetBloodIssuedDetailsWithServiceOrderID", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        #endregion
        public DataSet Screening(string screeningType, string action, int donorNo, int DonationTypeId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@Parameter_Second_Type", screeningType);
            HshIn.Add("@chvOption", action);
            HshIn.Add("@intDonorNo", donorNo);
            HshIn.Add("@intDonationTypeId", DonationTypeId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspBBInfectiousScreeningCompDiv", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }


}
