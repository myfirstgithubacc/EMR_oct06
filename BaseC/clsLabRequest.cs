using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace BaseC
{
    public class clsLabRequest
    {
        Hashtable HshIn;
        Hashtable HshOut;
        private string sConString = "";
        public clsLabRequest(string conString)
        {
            sConString = conString;
        }
        public DataSet GetInsuranceMaster()
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string strSQL = "SELECT InsuranceId, Name from Insurance WITH (NOLOCK) WHERE Active=1 ORDER BY Name";
                return objDl.FillDataSet(CommandType.Text, strSQL);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCompanyMaster(int iHospitalLocationId, string sSearchText)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
                string strSQL = "SELECT CompanyId, Name FROM Company WITH (NOLOCK) WHERE Active=1 ";
                if (sSearchText != "")
                {
                    strSQL = strSQL + " and Name LIKE '%" + sSearchText + "%'";
                }
                strSQL = strSQL + " AND HospitalLocationId = @intHospitalLocationId ORDER BY Name";

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDepartmentSubMaster(int iHospitalLocationId, int iDepartmentId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intDepartmentId", iDepartmentId);
                string strSQL = "SELECT SubDeptId, SubName  FROM DepartmentSub WITH (NOLOCK) WHERE DepartmentId = @intDepartmentId AND Active=1 AND HospitalLocationId = @intHospitalLocationId ORDER BY SubName";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLabRequestOP(int iLabNo, int intLoginFacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabRequestOP", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLabRequestIP(int iLabNo, int intLoginFacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabRequestIP", HshIn);
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
        public string CancelLabRequestService(int iDiagSampleId, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@intEncodedBy", userId);
            int Ret = 0;

            string strSQL = string.Empty;
            strSQL = "UPDATE DiagSampleIPLabMain SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE()  WHERE DiagSampleId = @intDiagSampleId";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                Ret = objDl.ExecuteNonQuery(CommandType.Text, strSQL, HshIn);
                return Ret.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string CancelLabRequestService(int iLabNo, int iServiceId, int userId, string Source)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intLabNo", iLabNo);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intEncodedBy", userId);
            int Ret = 0;

            string strSQL = string.Empty;
            if (Source == "OPD")
                strSQL = "UPDATE DiagSampleOPLabMain SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE()  WHERE LabNo = @intLabNo AND ServiceId = @intServiceId AND  StatusId = (SELECT StatusId FROM StatusMaster WITH (NOLOCK) WHERE StatusType = 'LAB' AND Code ='SNC' )";
            else
                strSQL = "UPDATE DiagSampleIPLabMain SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE()  WHERE LabNo = @intLabNo AND ServiceId = @intServiceId AND  StatusId = (SELECT StatusId FROM StatusMaster WITH (NOLOCK) WHERE StatusType = 'LAB' AND Code ='SNC' )";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                Ret = objDl.ExecuteNonQuery(CommandType.Text, strSQL, HshIn);
                return Ret.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetServicesAgainstRequest(int intRequestId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intRequestId", intRequestId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetServicesAgainstRequest", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet DiagOPPPackageDetais(int iFacilityId, int intRegistrationId, int intEncounterId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intEncounterId", intEncounterId);
            HshIn.Add("@intRegistrationId", intRegistrationId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagOPPackageDetais", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
