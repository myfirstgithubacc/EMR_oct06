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
    public class clsFacility
    {
        private string sConString = string.Empty;
        public clsFacility(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public DataSet GetFacilityWiseTaggedMasterName()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityWiseTaggedMasterName");

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetFacilityWiseCommonMaster(int iHospId, string sFlag, int iDeptId, int iFaclityId, int iEncodedby, string sSearch, int iGroupId, int iSubGroupId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("flag", sFlag);
                HshIn.Add("intDepartmentId", iDeptId);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshIn.Add("sSearch", sSearch);
                HshIn.Add("iGroupId", iGroupId);
                HshIn.Add("iSubGroupId", iSubGroupId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityWiseCommonMaster", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSpecializationWiseWiseCommonMaster(int iHospId, int iFaclityId, int iSpecializationid)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("iSpecialisationid", iSpecializationid);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSpecializationWiseTemplate", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveFacilityWiseCommonMaster(int iHospId, string iXmlData, int iFaclityId, bool iActive, int iEncodedby, string sFlag)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("xmlDepartment", iXmlData);
            HshIn.Add("intFacilityId", iFaclityId);
            HshIn.Add("btActive", iActive);
            HshIn.Add("intEncodedBy", iEncodedby);
            HshIn.Add("flag", sFlag);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveFacilityWiseCommonMaster", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getFacilityDepartment(int iHospId, int iFlag, int iDeptId, int iFaclityId, int iEncodedby)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("flag", iFlag);
                HshIn.Add("intDepartmentId", iDeptId);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intEncodedBy", iEncodedby);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityWiseDepartment", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveFacilityDepartmentTagg(int iHospId, string iXmlData, int iFaclityId, bool iActive, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("xmlDepartment", iXmlData);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("btActive", iActive);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveFacilityWiseDepartment", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateFacilityDepartmentTagg(int iHospId, string iXmlData, int iFaclityId, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("xmlDeptId", iXmlData);
            HshIn.Add("intFacilityId", iFaclityId);
            HshIn.Add("intEncodedBy", iEncodedby);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateFacilityWiseDept", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFacilityMasterName(int iHospId, int iFaclityId)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFaclityId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityMasterName", HshIn);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetEntrySiteMaster(int HospitalLocationId, int FacilityId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);

                HshIn.Add("intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEntrySiteMaster", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTaggedEntrySiteMaster(int HospitalLocationId, int FacilityId, int UserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intUserId", UserId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTaggedEntrySiteMaster", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEntrysiteDepartmentMaster(int iHospId, int iFaclityId, int iEncodedby, string sSearch)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshIn.Add("sSearch", sSearch);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEntrySiteWiseDepartment", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getTaggedEntrysiteDepartmentMaster(int iHospId, int iFaclityId, int iEncodedby, string sSearch, int EntrySiteId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshIn.Add("sSearch", sSearch);
                HshIn.Add("intEntrySiteId", EntrySiteId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEntrySiteWiseDepartment", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        public string SaveEntrySiteWiseDepartmentMaster(int iHospId, string iXmlData, int iFaclityId, int iEntrySiteId, bool iActive, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("xmlDepartment", iXmlData);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intEntrySiteId", iEntrySiteId);
                HshIn.Add("btActive", iActive);
                HshIn.Add("intEncodedBy", iEncodedby);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEntrySiteWiseDepartment", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateEntrySiteDepartmentTagged(int iHospId, int iFacilityId, int iDepartmentId, int iEntrySiteId, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("DeptId", iDepartmentId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intEntrySiteId", iEntrySiteId);
                HshIn.Add("intEncodedBy", iEncodedby);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEntrySiteWiseDept", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                HshIn = null; objDl = null;
            }
        }

        public string DeleteEntrySiteDepartmentTagged(int iHospId, int iFacilityId, int iDepartmentId, int iEntrySiteId, int iEncodedby, string griddepid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("DeptId", iDepartmentId);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intEntrySiteId", iEntrySiteId);
            HshIn.Add("intEncodedBy", iEncodedby);

            HshIn.Add("intdepid", griddepid);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteEntrySiteWiseDept", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCompanywiseCommonMaster(int iHospId, string iXmlData, int iFaclityId, int companytypeid, int companyid, bool iActive, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("xmlserviceid", iXmlData);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intcompanytypeid", companytypeid);
                HshIn.Add("intcompanyid", companyid);
                HshIn.Add("btActive", iActive);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCompanyWisePriceEditableService", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getCompanyservice(int iHospId, int iFaclityId, int companyid, int departmentid, int subdepartmentid)
        {



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocation", iHospId);
                HshIn.Add("@intFacilityId", iFaclityId);
                HshIn.Add("@companyid", companyid);
                HshIn.Add("@Departmentemnt", departmentid);
                HshIn.Add("@subDepartment", subdepartmentid);


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetCompanyWisePriceEditableService", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}