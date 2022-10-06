using System;
using System.Data;
using System.Collections;

namespace BaseC
{
    public partial class csCSSD
    {
        private string sConString = "";
        public csCSSD(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        // For Machine Type Master
        public DataSet GetMachineTypeMaster(int MachineTypeId, int Active, int FacilityId, int HospitalLacationID)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@MachineTypeId", MachineTypeId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSMachineTypeMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string saveMachineTypeMaster(int MachineTypeId, int FacilityId, string MachineTypeName, int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intMachineTypeId", MachineTypeId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvMachineTypeName", MachineTypeName.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMachineTypeMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetMachineMaster(int MachineId, int Active, int FacilityId, int HospitalLacationID)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@MachineId", MachineId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSMachineMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string saveMachineMaster(int MachineId, int FacilityId, int MachineTypeId, DateTime Preventive, DateTime Reminder, string MachineName, string TemperatureUnit, string ChamberPressureUnit, string JacketPressureUnit, string MachineShortName, int SterilizationItemExpiryPeriod, int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intMachineId", MachineId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvMachineName", MachineName.Trim());
            HshIn.Add("@intMachineTypeID", MachineTypeId);
            HshIn.Add("@dtPreventivedate", Preventive);
            HshIn.Add("@dtReminderDate", Reminder);
            HshIn.Add("@chvTemperatureUnit", TemperatureUnit);
            HshIn.Add("@chvChamberPressureUnit", ChamberPressureUnit);
            HshIn.Add("@chvJacketPressureUnit", JacketPressureUnit);
            HshIn.Add("@chvMachineShortName", MachineShortName);
            HshIn.Add("@intSterilizationItemExpiryPeriod", SterilizationItemExpiryPeriod);
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMachineMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // Item Type Master

        public DataSet GetItemTypeMaster(int ItemTypeId, int Active, int FacilityId, int HospitalLacationID)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@ItemTypeId", ItemTypeId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSItemTypeMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string saveItemTypeMaster(int ItemTypeId, int FacilityId, string ItemTypeName, int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intItemTypeId", ItemTypeId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvItemTypeName", ItemTypeName.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveItemTypeMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // CSItem Master

        public DataSet GetCSItemMaster(int CSItemId, int Active, int FacilityId, int HospitalLacationID)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@ItemId", CSItemId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSItemMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveCSItemMaster(int CSItemId, int FacilityId, string ManualCode, string CSItemName, int GroupType, int ItemTypeID, string IsStockEffective, int Manufacturer, int CSLocationID, string ItemSize, int Quantity, string Remarks, int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intCSItemId", CSItemId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvManualCode", ManualCode.Trim());
            HshIn.Add("@chvCSItemName", CSItemName.Trim());
            HshIn.Add("@intGroupType", GroupType);
            //HshIn.Add("@btIsSet", IsSet);
            HshIn.Add("@intItemTypeID", ItemTypeID);
            HshIn.Add("@chvIsStockEffective", IsStockEffective);
            HshIn.Add("@intManufacturer", Manufacturer);
            HshIn.Add("@chvCSLocationID", CSLocationID);
            //HshIn.Add("@chvsplArea", splArea);
            HshIn.Add("@chvItemSize", ItemSize);
            HshIn.Add("@intQuantity", Quantity);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSItemMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetManufacturer()
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, "select manufacture_code,manufacture_name Manufacturer from ManufacturerMaster WITH (NOLOCK) order by manufacture_name");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //commented by rakesh start
            //return objDl.FillDataSet(CommandType.Text, "select distinct(Manufacturer) Manufacturer from CsItemMaster order by Manufacturer");
            //commemted by rakesh end


        }

        public DataSet GetSpecialLocation()
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select distinct(splArea) splArea from CsItemMaster WITH (NOLOCK) order by splArea");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCSSetItemDetail(int SetId, int CSItemId, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@SetId", SetId);
                HshIn.Add("@CSItemId", CSItemId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSetItemDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCSSetItems(int Active, int GroupTypeId, int FacilityId, int HospitalLacationID)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intGroupTypeId", GroupTypeId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSetItems", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveItemSetDetail(int SetId, int CSItemId, int CSItemIdSet, int iHospID, int IsActive, int userId)
        {
            return saveItemSetDetail(SetId, CSItemId, CSItemIdSet, iHospID, IsActive, userId, 0);
        }

        public string saveItemSetDetail(int SetId, int CSItemId, int CSItemIdSet, int iHospID, int IsActive, int userId, int iQty)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intSetId", SetId);
            HshIn.Add("@intCSItemId", CSItemId);
            HshIn.Add("@CSItemIDSet", CSItemIdSet);
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);
            HshIn.Add("@inyHospitalLocationId", iHospID);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@intQty", iQty);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveItemSetDetail", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        // CSLocation Location Master
        public string saveLocationTypeMaster(int CSLocationTypeId, string LocationTypeName, int LocationTypeId, int iHospID, int FacilityId, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intCSLocationTypeId", CSLocationTypeId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvTypeDescription", LocationTypeName.Trim());
            HshIn.Add("@intLocationTypeId", LocationTypeId);
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSLocationTypeMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetLocationTypeMaster(int intCSLocationTypeId, int intHospitalLacationID, int intFacilityID, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intCSLocationTypeId", intCSLocationTypeId);
                HshIn.Add("@intFacilityID", intFacilityID);
                HshIn.Add("@intHospitalLocationID", intHospitalLacationID);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSLocationTypeMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // CSLocation Master

        public DataSet GetCSLocationMaster(int CSLocationId, int intHospitalLacationID, int intFacilityID, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@CSLocationId", CSLocationId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", intFacilityID);
                HshIn.Add("@intHospitalLocationID", intHospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSLocationMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string saveCSLocationMaster(int CSLocationId, string LocationName, string LocationShortName, int CSLocationTypeId, int iHospID, int FacilityId, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intCSLocationId", CSLocationId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvLocationName", LocationName.Trim());
            HshIn.Add("@chvLocationShortName", LocationShortName.Trim());
            HshIn.Add("@intCSLocationTypeId", CSLocationTypeId);
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSLocationMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // CSProgram Master
        public string SaveCSProgramMaster(int CSProgramID, string ProgramName, int iHospID, int FacilityId, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intProgramID", CSProgramID);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyFacilityID", FacilityId);
            HshIn.Add("@chvProgramName", ProgramName.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSProgramMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetCSProgramMaster(int intCSProgramID, int intHospitalLacationID, int intFacilityID, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intProgramID", intCSProgramID);
                HshIn.Add("@intFacilityID", intFacilityID);
                HshIn.Add("@intHospitalLocationID", intHospitalLacationID);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSProgramMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        // CSMachineProgramDetails Master
        public string SaveCSMachineProgramDetails(int ID, int ProgramID, int MachineID, string Temprature, string ChamberPressure, string JacketPressure, string HoldTime, int IsActive, int userId, int intHospitalLacationID, int intFacilityID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intId", ID);
            HshIn.Add("@intProgramID", ProgramID);
            HshIn.Add("@intMachineID", MachineID);
            HshIn.Add("@chvTemprature", Temprature.Trim());
            HshIn.Add("@chvChamberPressure", ChamberPressure);
            HshIn.Add("@chvJacketPressure", JacketPressure.Trim());
            HshIn.Add("@chvHoldTime", HoldTime.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);
            HshIn.Add("@intFacilityID", intFacilityID);
            HshIn.Add("@intHospitalLocationID", intHospitalLacationID);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSMachineProgramDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetCSMachineProgramDetails(int ID, int intHospitalLacationID, int intFacilityID, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intId", ID);
                HshIn.Add("@intFacilityID", intFacilityID);
                HshIn.Add("@intHospitalLocationID", intHospitalLacationID);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSMachineProgramDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string uspSaveOTStandardList(string Stndrd, int Status, int Uid, int HospId, string NewMod, int StnId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@Stndrd", Stndrd);
                HshIn.Add("@Status", Status);
                HshIn.Add("@Uid", Uid);
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@NewMod", NewMod);
                HshIn.Add("@StnId", StnId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); 
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTStandardList", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetStandardListMaster()
        {



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select CheckListCode,Description,ValueType,Active,case Active when 1 then 'Active' Else 'In-Active' End as ActiveStatus from CS_Standard_CheckList WITH (NOLOCK)");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet DataStandardSelItem(int StndrdId, int Status)
        {

            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@Lstcode", StndrdId);
                HshIn.Add("@Status", Status);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSelStandardDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetEmployeeListOfCSSD(int HospId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeListOfCSSD", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetAllEmployeeList(int HospId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAllEmployeeList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetCSItemOfSelectedItemType(int ItemTypeID, string GroupType, int HospId, int FacilityId, int Status)
        {
            return GetCSItemOfSelectedItemType(ItemTypeID, GroupType, HospId, FacilityId, Status, "");
        }

        public DataSet GetCSItemOfSelectedItemType(int ItemTypeID, string GroupType, int HospId, int FacilityId, int Status, string chvCSItemName)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intItemTypeID", ItemTypeID);
                HshIn.Add("@chvGroupType", GroupType);
                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@chvCSItemName", chvCSItemName);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSItemOfSelectedItemType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCSItemMasterForCSSDReceiving(int CSItemId, int Active, int FacilityId, int HospitalLacationID, int Encodedby)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = new Hashtable();
                HshIn.Add("@ItemId", CSItemId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSItemMasterForCSSDReceiving", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveCSSDReceiving(int ReceivingId, int HospitalLocationId, int FacilityId, int ItemTypeID, DateTime ReceivingDate,
                                      int ReceivingLocationID, int ReceivingDepartment, int ReceivingFromEmployeeID, string Remarks, bool Status, int Encodedby,
                                      int SendFromEmployeeID, string xmlReceivingItems, out string docNo, out int oreceivingid, int IsPost, int RequestingID, int IsReSterilization, string xmlCheckList)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                oreceivingid = 0;
                docNo = "";

                HshIn.Add("@intReceivingId", ReceivingId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intItemTypeID", ItemTypeID);
                HshIn.Add("@dtReceivingDate", ReceivingDate);
                HshIn.Add("@intReceivingLocationID", ReceivingLocationID);
                HshIn.Add("@intReceivingDepartment", ReceivingDepartment);
                HshIn.Add("@intReceivingFromEmployeeID", ReceivingFromEmployeeID);
                HshIn.Add("@chrRemarks", Remarks);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@intSendFromEmployeeID", SendFromEmployeeID);
                HshIn.Add("@xmlReceivingItems", xmlReceivingItems);
                HshIn.Add("@intIsPost", IsPost);
                HshIn.Add("@intRequestingID", RequestingID);
                HshIn.Add("@intIsReSterilization", IsReSterilization);

                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intReceivingCode", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshIn.Add("@xmlCheckList", xmlCheckList);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDReceiving", HshIn, HshOut);
                oreceivingid = Convert.ToInt32(HshOut["intReceivingCode"]);
                docNo = Convert.ToString(HshOut["chvDocumentNo"]);

                string msg = Convert.ToString(HshOut["@intReceivingCode"]);
                if (msg != "")
                {
                    oreceivingid = Convert.ToInt32(HshOut["@intReceivingCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getReceivingMainList(int ReceivingId, string ReceivingNo, int ItemTypeID,
                                    DateTime FromDate, DateTime ToDate, int Active
                                    , int HospId, int FacilityId, int EncodedBy)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intReceivingId", ReceivingId);
                HshIn.Add("@chvReceivingNo", ReceivingNo);
                HshIn.Add("@intItemTypeID", ItemTypeID);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@bitStatus", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDReceiving", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCSSDReceivingMain(int ReceivingId, string ReceivingNo, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intReceivingId", ReceivingId);
                HshIn.Add("@chvReceivingNo", ReceivingNo);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDReceivingMain", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public DataSet GetCSSDReceivingDetails(int ReceivingId, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intReceivingId", ReceivingId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDReceivingDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string CancelCSSDReceiving(int ReceivingId, int HospitalLocationId, int FacilityId, bool Status, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intReceivingId", ReceivingId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelCSSDReceiving", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCSSDLocationType()
        {



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, "select StatusId as LocationTypeID,Status as LocationTypeName from StatusMaster WITH (NOLOCK) where StatusType='CSLocationType' and Active=1 ORDER BY Status ");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCSSDMachineType()
        {



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select StatusId as MachineTypeID,Status as MachineTypeName from StatusMaster WITH (NOLOCK) where StatusType='CSSDMachineType' and Active=1 ORDER BY Status ");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCSMachineProgramValues(int MachineID, int ProgramID, int Active)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intMachineId", MachineID);
                HshIn.Add("@intProgramId", ProgramID);
                HshIn.Add("@bitStatus", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetMachineProgramValues", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public string SaveCSSDBowieDickTest(int BowieDickTestID, int HospitalLocationId, int FacilityId, int MachineID, int ProgramID, DateTime TestDate,
        string StartTime, string FinishTime, string HoldingTime, string JacketPressure, string ChamberPressure,
        string Temperature, string ColorChange, string DryNess,
        string TestResult, int EmployeeID, string Problem, string ActionTaken, bool Status, int Encodedby,
        out string docNo, out int BowieDickTestCode)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                BowieDickTestCode = 0;
                docNo = "";

                HshIn.Add("@intBowieDickTestID", BowieDickTestID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intMachineID", MachineID);
                HshIn.Add("@intProgramID", ProgramID);
                HshIn.Add("@dtTestDate", TestDate);
                HshIn.Add("@chvStartTime", StartTime);
                HshIn.Add("@chvFinishTime", FinishTime);
                HshIn.Add("@chvHoldingTime", HoldingTime);
                HshIn.Add("@chvJacketPressure", JacketPressure);
                HshIn.Add("@chvChamberPressure", ChamberPressure);
                HshIn.Add("@chvTemperature", Temperature);
                HshIn.Add("@chvColorChange", ColorChange);
                HshIn.Add("@chvDryNess", DryNess);
                HshIn.Add("@chvTestResult", TestResult);
                HshIn.Add("@intEmployeeID", EmployeeID);
                HshIn.Add("@chvProblem", Problem);
                HshIn.Add("@chvActionTaken", ActionTaken);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);

                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intBowieDickCode", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDBowieDickTest", HshIn, HshOut);
                BowieDickTestCode = Convert.ToInt32(HshOut["@intBowieDickCode"]);
                docNo = Convert.ToString(HshOut["chvDocumentNo"]);

                string msg = Convert.ToString(HshOut["@intReceivingCode"]);
                if (msg != "")
                {
                    BowieDickTestCode = Convert.ToInt32(HshOut["@intBowieDickCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCSSDBowiedickTestID(string BowieDickTestNo, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@chvBowieDickTestNo", BowieDickTestNo);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDBowiedickTestID", HshIn, HshOut);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getBowiedickTestMainList(int BowieDickTestID, string BowieDickTestNo, int MachineID, int ProgramID
                                    , string TestResult, int Operator,
                                    DateTime FromDate, DateTime ToDate, int Active
                                    , int HospId, int FacilityId, int EncodedBy)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intBowieDickTestID", BowieDickTestID);
                HshIn.Add("@chvBowieDickTestNo", BowieDickTestNo);
                HshIn.Add("@intMachineID", MachineID);
                HshIn.Add("@intProgramID", ProgramID);
                HshIn.Add("@chvTestResult", TestResult);
                HshIn.Add("@intOperator", Operator);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@bitStatus", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDBowiedickTest", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getBowiedickTestDetail(int BowieDickTestID, string BowieDickTestNo, int Active
                                    , int HospId, int FacilityId, int EncodedBy)
        {
            //string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            //string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intBowieDickTestID", BowieDickTestID);
                HshIn.Add("@chvBowieDickTestNo", BowieDickTestNo);
                HshIn.Add("@bitStatus", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDBowiedickTestDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public DataSet GetCSSDItemGroupType(int hospitalLocationID, int facilityId)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select StatusId as ItemGroupTypeID,Status as ItemGroupTypeName,Code from StatusMaster WITH (NOLOCK) where StatusType='CSSDItemGroup' and Active=1 ORDER BY Status ");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string CancelCSSDBowiedickTest(int BowieDickTestId, int HospitalLocationId, int FacilityId, bool Status, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intBowieDickTestId", BowieDickTestId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelCSSDBowiedickTest", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet CheckItemExistsInReceiving(int CSItemId, int ReceivingID, int Active, int FacilityId, int HospitalLacationID, int Encodedby)
        {


            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@ItemId", CSItemId);
                HshIn.Add("@ReceivingID", ReceivingID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckItemExistsInReceiving", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCSSDReceivedItems(int hospitalLocationID, int facilityId, string ItemType)
        {

            HshIn = new Hashtable();
            HshIn.Add("@chvItemStatus", ItemType);
            HshIn.Add("@inyHospId", hospitalLocationID);
            HshIn.Add("@intFacilityID", facilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select RD.ID,RD.ReceivingID,RD.CSItemID,Loc.LocationName,IM.CSItemName,RD.Qty,(CASE WHEN ISNULL(RD.ID, 0) = 0 THEN 0 ELSE 1 END) IsChk FROM CSReceivingDetail RD WITH (NOLOCK) INNER JOIN CsItemMaster IM WITH (NOLOCK) ON IM.CSItemID=RD.CSItemID INNER JOIN CSReceivingMain RM WITH (NOLOCK) ON RM.ReceivingID=RD.ReceivingID INNER JOIN CsLocation Loc WITH (NOLOCK) on Loc.CSLocationID=RM.ReceivingLocationID WHERE RD.ItemStatus=@chvItemStatus AND RD.Active=1 AND IM.FacilityID=@intFacilityID AND IM.HospitalLocationID=@inyHospId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetBowdickTestDoneForMachine(int MachineID, int Active, DateTime TestDate)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intMachineId", MachineID);
                HshIn.Add("@bitStatus", Active);
                HshIn.Add("@dtTestDate", TestDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetBowdickTestDoneForMachine", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SaveCSSDSterilization(int TRNID, int MachineID, int HospitalLocationId, int FacilityId
            , DateTime SterileDate, int PackedBy, int CheckedBy, int OperatorLoad, int OperatorUnload
            , string Temperature, string TimeIn, string TimeOut, string TotalTime, int Program, string HoldTime
            , string ChamberPressure, string JacketPressure, string CleanedBy, string DisinfectedBy
            , string DryingBy, string AerateDryTime, bool Status, int Encodedby, string xmlReceivingItems, out string docNo, out int SterilizationCode, int ReceivedBy, string Batchno
            , string CSSDCode, int NoOfTimesToBeUsed, int NoOfTimesUsed, string xmlCheckList)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                SterilizationCode = 0;
                docNo = "";

                HshIn.Add("@intTRNID", TRNID);
                HshIn.Add("@intMachineID", MachineID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtSterileDate", SterileDate);
                HshIn.Add("@intPackedBy", PackedBy);
                HshIn.Add("@intCheckedBy", CheckedBy);
                HshIn.Add("@intOperatorLoad", OperatorLoad);
                HshIn.Add("@intOperatorUnload", OperatorUnload);
                HshIn.Add("@chvTemperature", Temperature);
                HshIn.Add("@chvTimeIn", TimeIn);
                HshIn.Add("@chvTimeOut", TimeOut);
                HshIn.Add("@chvTotalTime", TotalTime);
                HshIn.Add("@intProgram", Program);
                HshIn.Add("@chvHoldTime", HoldTime);
                HshIn.Add("@chvChamberPressure", ChamberPressure);
                HshIn.Add("@chvJacketPressure", JacketPressure);
                HshIn.Add("@chvCleanedBy", CleanedBy);
                HshIn.Add("@chvDisinfectedBy", DisinfectedBy);
                HshIn.Add("@chvDryingBy", DryingBy);
                HshIn.Add("@chvAerateDryTime", AerateDryTime);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@xmlReceivingItems", xmlReceivingItems);
                HshIn.Add("@ReceivedBy", ReceivedBy);
                HshIn.Add("@Batchno", Batchno);
                HshIn.Add("@CSSDCode", CSSDCode);
                HshIn.Add("@NoOfTimesToBeUsed", NoOfTimesToBeUsed);
                HshIn.Add("@NoOfTimesUsed", NoOfTimesUsed);
                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intSterilizeCode", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshIn.Add("@xmlCheckList", xmlCheckList);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDSterilization", HshIn, HshOut);
                SterilizationCode = Convert.ToInt32(HshOut["@intSterilizeCode"]);
                docNo = Convert.ToString(HshOut["chvDocumentNo"]);

                string msg = Convert.ToString(HshOut["@intSterilizeCode"]);
                if (msg != "")
                {
                    SterilizationCode = Convert.ToInt32(HshOut["@intSterilizeCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetStrelizationList(DateTime FDate, DateTime TDate, int HospId, int FacId, int UserId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        Hashtable HshIn = new Hashtable();
        //        Hashtable HshOut = new Hashtable();

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDStrelized", HshIn, HshOut);

        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}

        public DataSet FillItemsList(int HospId, int FacilityId, int ItemTypeId, string ItemStatus)
        {


            HshIn = new Hashtable();
            HshIn.Add("@HospLocId", HospId);
            HshIn.Add("@FacId", FacilityId);
            HshIn.Add("@TypeId", ItemTypeId);
            HshIn.Add("@chvItemStatus", ItemStatus);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select RD.ID,RD.ReceivingID,RD.CSItemID,Loc.LocationName,IM.CSItemName,RD.Qty,(CASE WHEN ISNULL(RD.ID, 0) = 0 THEN 0 ELSE 1 END) IsChk FROM CSReceivingDetail RD WITH (NOLOCK) INNER JOIN CsItemMaster IM WITH (NOLOCK) ON IM.CSItemID=RD.CSItemID INNER JOIN CSReceivingMain RM WITH (NOLOCK) ON RM.ReceivingID=RD.ReceivingID INNER JOIN CsLocation Loc WITH (NOLOCK) on Loc.CSLocationID=RM.ReceivingLocationID WHERE RD.ItemStatus=@chvItemStatus AND RD.Active=1 AND IM.ItemTypeID=@TypeId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet FillItemType(int HospId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospLocId", HospId);
                HshIn.Add("@FacId", FacilityId);
                return objDl.FillDataSet(CommandType.Text, "select ItemTypeID,ItemTypeDescription from CsItemType WITH (NOLOCK) where Status =1 and FacilityID = @FacId and HospitalLocationID=@HospLocId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFillCSSDLocation(int HospId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospLocId", HospId);
                HshIn.Add("@FacId", FacilityId);
                return objDl.FillDataSet(CommandType.Text, "select CSLocationID,LocationName from CsLocation WITH (NOLOCK) where Status=1 and FacilityID =@FacId and HospitalLocationID =@HospLocId ", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetFillCsEmpList(int HospId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospLocId", HospId);
                HshIn.Add("@FacId", FacilityId);
                return objDl.FillDataSet(CommandType.Text, "select e.ID,  ISNULL(e.FirstName,'') + ISNULL(' ' + e.MiddleName,'') + ISNULL(' ' + e.LastName,'') EmpName FROM employee e WITH (NOLOCK) inner join EmployeeType et WITH (NOLOCK) on e.EmployeeType =et.Id where e.Active=1 and e.HospitalLocationId=@HospLocId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CsSaveIssueItems(int LocId, int FacId, DateTime IssDate, int IssLoc, int IssDept, string EmpId, string Remark, string userid, DateTime SerDate, string xmlIssueItems, string itemType, out string docNo, out int oreceivingid, string xmlCheckList)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", LocId);
                HshIn.Add("@intFacilityId", FacId);
                HshIn.Add("@dtIssDate", IssDate);
                HshIn.Add("@intIssuingLocationID", IssLoc);
                HshIn.Add("@intIssuingDepartmentID", IssLoc);
                HshIn.Add("@intReceivingFromEmployeeID", EmpId);
                HshIn.Add("@chrRemarks", Remark);
                HshIn.Add("@intEncodedBy", userid);
                HshIn.Add("@chvIsSterilizedORNon", itemType);
                HshIn.Add("@xmlIssueItems", xmlIssueItems);
                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intIssueCode", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshIn.Add("@xmlCheckList", xmlCheckList);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDIssue", HshIn, HshOut);
                oreceivingid = Convert.ToInt32(HshOut["intIssueCode"]);
                docNo = Convert.ToString(HshOut["chvDocumentNo"]);
                string msg = Convert.ToString(HshOut["@intIssueCode"]);
                if (msg != "")
                {
                    oreceivingid = Convert.ToInt32(HshOut["@intIssueCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSelectIssueMainDetails(string IssueNo, int IssueID, int HospId, int FacilityId, int UserId)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@issueNo", IssueNo);
                HshIn.Add("@IssueID", IssueID);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspCssdIssueSelectIssueID", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public string CsSaveIssueItems(int IssType, int LocId, int FacId, DateTime IssDate, int IssLoc, string EmpId, string Remark, string userid, DateTime SerDate, string xmlIssueItems, out string docNo, out int oreceivingid)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn.Add("@intItemTypeID", IssType);
        //    HshIn.Add("@inyHospitalLocationId", LocId);
        //    HshIn.Add("@intFacilityId", FacId);
        //    HshIn.Add("@dtIssDate", IssDate);
        //    HshIn.Add("@intIssuingLocationID", IssLoc);
        //    HshIn.Add("@intReceivingFromEmployeeID", EmpId);
        //    HshIn.Add("@chrRemarks", Remark);
        //    HshIn.Add("@intEncodedBy", userid);
        //    HshIn.Add("@xmlIssueItems", xmlIssueItems);
        //    HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
        //    HshOut.Add("@intIssueCode", SqlDbType.Int);
        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDIssue", HshIn, HshOut);
        //    oreceivingid = Convert.ToInt32(HshOut["intIssueCode"]);
        //    docNo = Convert.ToString(HshOut["chvDocumentNo"]);
        //    string msg = Convert.ToString(HshOut["@intIssueCode"]);
        //    if (msg != "")
        //    {
        //        oreceivingid = Convert.ToInt32(HshOut["@intIssueCode"]);
        //        docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
        //    }

        //    return Convert.ToString(HshOut["@chvErrorStatus"]);

        //}
        public DataSet FillCSSDIssuingDetail(int IssueID, int HospId, int FacilityId, int UserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@IssueID", IssueID);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDIssuingDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet GetCSSDIssuingMain(string FDate, string TDate, int HospId, int FacilityId)
        //{
        //    DataSet ds = new DataSet();
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn.Add("@FromDate", FDate);
        //    HshIn.Add("@ToDate", TDate);
        //    HshIn.Add("@inyHospitalLocationId", HospId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspCssdIssueMain", HshIn, HshOut);
        //    return ds;
        //}
        public DataSet GetCSSDIssuingMain(int IssueID, string IssueNo, int HospId, int FacilityId, string FDate, string TDate, string IsSterilizedOrNon)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@IssueID", IssueID);
                HshIn.Add("@issueNo", IssueNo);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FromDate", FDate);
                HshIn.Add("@ToDate", TDate);
                HshIn.Add("@IsSterilizedOrNon", IsSterilizedOrNon);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspCssdIssueMain", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCSSDIssuingMain(int IssueID, string IssueNo, int HospId, int FacilityId, string FDate, string TDate, string IsSterilizedOrNon, int IsPost)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@IssueID", IssueID);
                HshIn.Add("@issueNo", IssueNo);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FromDate", FDate);
                HshIn.Add("@ToDate", TDate);
                HshIn.Add("@IsSterilizedOrNon", IsSterilizedOrNon);
                HshIn.Add("@IsPost", IsPost);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspCssdIssueMain", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCSSDSterilizationMain(int TRNID, string LoadNo, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intTRNID", TRNID);
                HshIn.Add("@chvLoadNo", LoadNo);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDSterilizationMain", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetCSSDSterilizationDetails(int TRNID, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@intTRNID", TRNID);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDSterilizationDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string PostCSSDSterilization(int TRNID, int HospitalLocationId, int FacilityId, int Encodedby)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intTRNID", TRNID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPostCSSDSterilization", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetStrelizationList(string LoadNo, DateTime FDate, DateTime TDate, int HospId, int FacId, int UserId)
        {


            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@LoadNo", LoadNo);
            HshIn.Add("@dtFromDate", FDate);
            HshIn.Add("@dtToDate", TDate);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacId);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDStrelized", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelCSSDSterilization(int TRNID, int HospitalLocationId, int FacilityId, bool Status, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intTRNID", TRNID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelCSSDSterilization", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCSSDItemsForSterilizationAndIssue(int intItemTypes, int intHospitalLacationID, int intFacilityID, int Encodedby, string GroupIdsStr, string TypeIdsStr, int intReceivingLocationID)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intItemTypes", intItemTypes);
                HshIn.Add("@intFacilityId", intFacilityID);
                HshIn.Add("@inyHospitalLocationId", intHospitalLacationID);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@chvGroupIdsStr", GroupIdsStr);
                HshIn.Add("@chvTypeIdsStr", TypeIdsStr);
                HshIn.Add("@intReceivingLocationID", intReceivingLocationID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetItems", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string PostCSSDIssue(int IssueId, int HospitalLocationId, int FacilityId, int Encodedby)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPostCSSDIssue", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string AcknowledgeCSSDIssue(int IssueId, int HospitalLocationId, int FacilityId, int Encodedby)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAcknowledgeCSSDIssue", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string IssueToPatient(int IssueId, int HospitalLocationId, int FacilityId, int Encodedby, int EncounterId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@intEncounterId", EncounterId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspIssueToPatient", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelCSSDIssue(int IssueID, int HospitalLocationId, int FacilityId, bool Status, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intIssueID", IssueID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelCSSDIssue", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCSItem(int HospId, int FacilityId, int Encodedby)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSItem", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getRequestingMainList(int RequestingId, string RequestingNo, int ItemTypeID,
                                    DateTime FromDate, DateTime ToDate, int Active
                                    , int HospId, int FacilityId, int EncodedBy, int isFanalized, int isReceiving)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intRequestingId", RequestingId);
                HshIn.Add("@chvRequestingNo", RequestingNo);
                HshIn.Add("@intItemTypeID", ItemTypeID);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@bitStatus", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intisFanalized", isFanalized);
                HshIn.Add("@intisReceiving", isReceiving);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDRequesting", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCSSDRequestingMain(int RequestingId, string RequestingNo, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intRequestingId", RequestingId);
                HshIn.Add("@chvRequestingNo", RequestingNo);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDRequestingMain", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetCSSDRequestingDetails(int RequestingId, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intRequestingId", RequestingId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDRequestingDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveCSSDSterilizationRequest(int RequestingID, int HospitalLocationId, int FacilityId, int ItemTypeID, DateTime RequestingDate,
                                      int RequestingLocationID, int RequestingFromEmployeeID, string Remarks, bool Status, int Encodedby,
                                      string xmlReceivingItems, out string docNo, out int oRequestingID, int IsFanalize, int IsReSterilization)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                oRequestingID = 0;
                docNo = "";

                HshIn.Add("@intRequestingID", RequestingID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intItemTypeID", ItemTypeID);
                HshIn.Add("@dtRequestingDate", RequestingDate);
                HshIn.Add("@intRequestingLocationID", RequestingLocationID);
                HshIn.Add("@intRequestingFromEmployeeID", RequestingFromEmployeeID);
                HshIn.Add("@chrRemarks", Remarks);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@xmlReceivingItems", xmlReceivingItems);
                HshIn.Add("@intIsFanalize", IsFanalize);
                HshIn.Add("@intIsReSterilization", IsReSterilization);

                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intRequestingCode", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDSterilizationRequest", HshIn, HshOut);
                oRequestingID = Convert.ToInt32(HshOut["intRequestingCode"]);
                docNo = Convert.ToString(HshOut["chvDocumentNo"]);

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
        public string CancelCSSDSterilizationRequest(int RequestingID, int HospitalLocationId, int FacilityId, bool Status, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intRequestingID", RequestingID);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelCSSDSterilizationRequest", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCSManufacturerMaster(int ManufactureCode, int Active, int FacilityId, int HospitalLacationID)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ManufacturerCode", ManufactureCode);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationID", HospitalLacationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSManufacturerMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCSStandaredCheckList(int Active)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetCSSDStandaredCheckList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveDecontaminationPacking(int ReceivingId, int HospitalLacationID, int FacilityId, int Stage, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intReceivingId", ReceivingId);
                HshIn.Add("@inyHospitalLocationId", HospitalLacationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@Stage", Stage);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDecontaminationPacking", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCSSDStatus(int intItemTypes, int intHospitalLacationID, int intFacilityID, int Encodedby, string GroupIdsStr, string TypeIdsStr, int intReceivingLocationID, DateTime FromDate, DateTime ToDate, string CSSDStatus)
        {


            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intItemTypes", intItemTypes);
                HshIn.Add("@intFacilityId", intFacilityID);
                HshIn.Add("@inyHospitalLocationId", intHospitalLacationID);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@chvGroupIdsStr", GroupIdsStr);
                HshIn.Add("@chvTypeIdsStr", TypeIdsStr);
                HshIn.Add("@intReceivingLocationID", intReceivingLocationID);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@CSSDStatus", CSSDStatus);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getCSSDStatus(int iHospID, string statusType, string Code)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospId", iHospID);
                HshIn.Add("@chvStatusType", statusType);
                HshIn.Add("@chvCode", Code);

                string strqry = "SELECT Status, StatusColor, StatusId, Code FROM GetStatus(@inyHospId, @chvStatusType) ";
                if (Code != "")
                {
                    strqry += " WHERE Code=@chvCode ";
                }
                strqry += " ORDER BY SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }















        public string saveCSSDDocument(int HospitalLocationId, int FacilityId, int TrnId, string imageName, string imagePath, string imageSize, string thumbnail, string dtype, string description, int EncodedBy)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intTrnId", TrnId);
            HshIn.Add("@strImgName", imageName);
            HshIn.Add("@strImgPath", imagePath);
            HshIn.Add("@strImgSize", imageSize);
            HshIn.Add("@strThumbnail", thumbnail);
            HshIn.Add("@strDocumentType", dtype);
            HshIn.Add("@strDescription", description);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCSSDDocument", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string updateCSSDDocument(int HospitalLocationId, int FacilityId, int DocumentId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@id", DocumentId);
            HshIn.Add("@intEncodedBy", EncodedBy);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateCSSDDocument", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCSSDDocument(int HospitalLocationId, int FacilityId, int TrnId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intTrnId", TrnId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCSSDDocument", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSterilizedLocation()
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetSterilizedLocation");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string ValidateRegistrationNoCSV(int iFacilityId, string CSVUHID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chvCSVUHID", CSVUHID);
            HshIn.Add("@intFacilityID", iFacilityId);
            HshOut.Add("@NotExistingUHID", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCSValidateRegistrationNo", HshIn, HshOut);
                return HshOut["@NotExistingUHID"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }






    }

}
