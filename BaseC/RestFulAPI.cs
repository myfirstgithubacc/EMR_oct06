
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DAL;

namespace BaseC
{
    public class RestFulAPI
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable hshOutput;

        private DAL.DAL objDl;
        /// <summary>
        /// RestAPI from WCF
        /// </summary>
        /// <param name="constring"></param>
        public RestFulAPI(string constring)
        {
            sConString = constring;
        }
        #region Common
        /// <summary>
        /// Get Complete Data of Hospital Setup Table
        /// </summary>
        /// <returns>Dataset</returns>
        public DataSet GetHospitalSetup(int iHospitalLocationID, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMasterGetHospitalSetup", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        /// <summary>
        /// Return a Flag Value From Hospital Setup table
        /// </summary>
        /// <param name="HospitalLocation"></param>
        /// <param name="strFlag"></param>
        /// <returns>String</returns>
        public string GetFlagValueHospitalSetup(int HospitalLocation, string strFlag)
        {
            string strValue = "";
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@hospid", HospitalLocation);
            HshIn.Add("@flag", strFlag);
            try
            {

                string strSQL = "Select Value From HospitalSetup Where HospitalLocationId = @hospid and Flag = @flag";
                ds = objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strValue = Convert.ToString(ds.Tables[0].Rows[0]["Value"]);
                }
                return strValue;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// Get Facility Master Data
        /// </summary>
        /// <param name="iHospID">Hospital Location Id</param>
        /// <param name="iUserId">Facility by User permission</param>
        /// <param name="iGroupID">Facility by User Group Permission</param>
        /// <param name="EncodedBy">location created by</param>
        /// <returns>Dataset</returns>
        public DataSet GetFacilityList(int iHospID, int iUserId, int iGroupID, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("inyHospitalLocationId", iHospID);
                HshIn.Add("intUserId", iUserId);
                HshIn.Add("intGroupId", iGroupID);
                HshIn.Add("intEncodedBy", EncodedBy);

                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        /// <summary>
        /// Get Doctor List with filter
        /// </summary>
        /// <param name="HospitalId"> Hospital Location Id</param>
        /// <param name="intSpecialisationId"> Specialization Id</param>
        /// <param name="FacilityId">Facility Id</param>
        /// <returns>Dataset</returns>
        public DataSet GetDoctorList(int HospitalId, int intSpecialisationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("HospitalLocationId", HospitalId);
                HshIn.Add("intSpecialisationId", intSpecialisationId);
                HshIn.Add("intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Get Hospital company list
        /// </summary>
        /// <param name="iHospId">Hospital Location</param>
        /// <param name="CompanyType">Company type as company, insurance</param>
        /// <param name="InsuranceId">Insurance Id to Select under insurance company</param>
        /// <returns>Dataset</returns>
        public DataSet GetCompanyList(int iHospId, string CompanyType, int InsuranceId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("inyHospitalLocationID", iHospId);
            HshIn.Add("chvCompanyType", CompanyType);
            HshIn.Add("intInsuranceId", InsuranceId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Get Hospital Default company id
        /// </summary>
        /// <param name="iHospId"></param>
        /// <returns></returns>
        public int GetDefaultCompany(int iHospId)
        {
            int IsDefault = 0;

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("intHospId", iHospId);

                IsDefault = Convert.ToInt32(GetFlagValueHospitalSetup(iHospId, "DefaultHospitalCompany"));


                return IsDefault;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Get Hospital's Department 
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="sServiceType">Filter for seleted service type</param>
        /// <returns>Dataset</returns>
        public DataSet GetHospitalDepartment(int iHospitalLocationId, string sServiceType)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("chvDepartmentType", sServiceType);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalDepartments", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        /// <summary>
        /// Get Hosptial's Departmentwise sub department
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iDepartmentID">Selected Department ID</param>
        /// <param name="sServiceType">Selected Service type</param>
        /// <param name="iSubDeptID">Selected sub dept id</param>
        /// <returns>Dataset</returns>
        public DataSet GetHospitalSubDepartment(int iHospitalLocationId, int iDepartmentID, string sServiceType, int iSubDeptID)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("chvDepartmentType", sServiceType);
                HshIn.Add("intDepartmentId", iDepartmentID);
                HshIn.Add("intSubDepartmentId", iSubDeptID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSubDepartments", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Get Facilitywise Department
        /// </summary>
        /// <param name="iHospId"></param>
        /// <param name="iFlag"></param>
        /// <param name="iDeptId"></param>
        /// <param name="iFaclityId"></param>
        /// <param name="iEncodedby"></param>
        /// <returns>Dataset</returns>
        public DataSet GetFacilityDepartment(int iHospId, int iFlag, int iDeptId, int iFaclityId, int iEncodedby)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("flag", iFlag);
                HshIn.Add("intDepartmentId", iDeptId);
                HshIn.Add("intFacilityId", iFaclityId);
                HshIn.Add("intEncodedBy", iEncodedby);

                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityWiseDepartment", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// Get Hospital Services
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="sSubDepID">Selected sud department</param>
        /// <param name="sType">Selected Service type</param>
        /// <returns>Dataset</returns>
        public DataSet GetService(int iHospitalLocationId, string sSubDepID, string sType, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("chvSubDepartmentIds", sSubDepID);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("chrType", sType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Get Employee Id from selected User Id
        /// </summary>
        /// <param name="iUserId"></param>
        /// <param name="iHospitalLocationId"></param>
        /// <returns>String</returns>
        public string GetEmployeeIdFromUserId(int iUserId, int iHospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                string i = "";

                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intUserId", iUserId);
                i = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT isnull(Convert(varchar(10),EmpId),'0') EmpId  From Users  Where HospitalLocationID = @inyHospitalLocationId And Id = @intUserId", HshIn);
                if (i == null)
                {
                    return i = "0";
                }
                else
                {
                    return i;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// Get User Id From Employee id
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="HospitalLocationID"></param>
        /// <returns></returns>
        public string GetUserIDFromEmployeeId(string EmpId, string HospitalLocationID)
        {
            string i = "";
            HshIn = new Hashtable();

            DAL.DAL dL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@EmpId", EmpId);
                HshIn.Add("@HospitalLocationId", HospitalLocationID);

                i = (string)dL.ExecuteScalar(CommandType.Text, "SELECT isnull(Convert(varchar(10),ID),'0') as ID From Users where EmpId=@EmpId AND HospitalLocationId=@HospitalLocationId", HshIn);
                if (i == null)
                {
                    return i = "0";
                }
                else
                {
                    return i;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        /// <summary>
        /// Search Patient complete details from reg to admission hospital wise & encounterid
        /// </summary>
        /// <param name="iHospID"></param>
        /// <param name="iRegistrationId"></param>
        /// <param name="iEncounterId"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>
        public DataSet GetPatientSummary(int iHospID, int iFacilityId, int iRegistrationId, int iEncounterId, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", iHospID);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intEncodedBy", EncodedBy);

                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetails", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Get Employee Details
        /// </summary>
        /// <param name="iHospID"></param>
        /// <param name="iStationId"></param>
        /// <param name="iEmpTypeId"></param>
        /// <param name="xmlEmployeeType"></param>
        /// <param name="EmpName"></param>
        /// <param name="iMobileNo"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>
        public DataSet GetEmployeeData(int iHospID, int FacilityId, int iStationId, int iEmpTypeId, string xmlEmployeeType, string EmpName, int iMobileNo, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("HospitalLocationId", iHospID);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intEmployeeTypeId", iEmpTypeId);
                if (xmlEmployeeType != "")
                {
                    HshIn.Add("xmlEmployeeType", xmlEmployeeType);
                }
                HshIn.Add("chrEmployeeName", EmpName);
                HshIn.Add("intLabStationId", iStationId);
                HshIn.Add("intMobileNo", iMobileNo);
                HshIn.Add("bitStatus", 1);
                HshIn.Add("intEncodedBy", EncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getPatientDetails(Int64 regNo, int mode, int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("RegistrationId", regNo);
                HshIn.Add("Mode", mode);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientRecord", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientCountry()
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select 0 CountryId, '[ Select ]' CountryName union all SELECT distinct CountryMaster.CountryId, CountryMaster.CountryName FROM CountryMaster  where Active = 1 ORDER BY CountryName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientState(int CountryId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                if (CountryId > 0)
                    return objDl.FillDataSet(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName,StateMaster.CountryID from StateMaster where StateMaster.CountryID='" + CountryId + "'  AND Active = 1 ORDER BY StateName");
                else
                    return objDl.FillDataSet(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName,StateMaster.CountryID from StateMaster where Active = 1 ORDER BY StateName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientCity(int StateId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                if (StateId > 0)
                    return objDl.FillDataSet(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname,StateId from CityMaster where StateId='" + StateId + "'  AND Active = 1 ORDER BY CityMaster.cityname");
                else
                    return objDl.FillDataSet(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname,StateId from CityMaster where Active = 1 ORDER BY CityMaster.cityname");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientCityArea(int CityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                if (CityId > 0)
                    return objDl.FillDataSet(CommandType.Text, "SELECT AreaId,AreaName,CityID,ZipCode FROM AreaMaster WHERE CityID='" + CityId + "' AND Active = 1  ORDER BY AreaName");
                else
                    return objDl.FillDataSet(CommandType.Text, "SELECT AreaId,AreaName,CityID,ZipCode FROM AreaMaster WHERE Active = 1  ORDER BY AreaName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientZipcode(int CityAreaId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "SELECT DISTINCT ZP.ZIPID, ZP.ZipCode FROM ZipMaster ZP INNER JOIN AreaMaster AM ON ZP.CityId=AM.CityId " +
                                                          " WHERE AM.AreaId='" + CityAreaId + "'  AND Active = 1 ORDER BY ZP.ZipCode");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetDoctors(int HospitalId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                StringBuilder str = new StringBuilder();
                str.Append("select e.ID as DoctorID, dbo.GetDoctorName(e.ID) as DoctorName, vst.TemplateName, sm.Name,sm.ID as SpecialisationId from Employee e ");
                str.Append(" LEFT JOIN EMRDoctorVitalSignTemplate dvt ON e.ID = dvt.DoctorID and dvt.Active=1 ");
                str.Append(" LEFT JOIN EMRVitalSignTemplate vst ON dvt.TemplateId = vst.Id ");
                str.Append(" LEFT JOIN DOCTORDETAILS dd ON e.id = dd.DoctorId ");
                str.Append(" LEFT JOIN SpecialisationMaster sm ON dd.SpecialisationId = sm.ID ");
                str.Append(" where EmployeeType in (1,2) and e.HospitalLocationId=" + HospitalId + " order by DoctorName  ");
                return objDl.FillDataSet(CommandType.Text, str.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetPatientIdentityType()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Description from PatientIdentityType where Active=1 order by Description");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public DataSet GetLanguage()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select LanguageID,Language from languagemaster where Active=1 order by Language");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientContectno(int RegId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegId);
                return objDl.FillDataSet(CommandType.Text, "select PhoneHome,MobileNo from Registration WHERE Id=@intRegistrationId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string GetPatientRegistrationNo(Int32 RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                ds = objDl.FillDataSet(CommandType.Text, "Select RegistrationNo from registration where Id =" + RegistrationId);
                string strRegNo = "";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRegNo = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                }
                return strRegNo;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }

        public DataSet SearchPatientname(int HospitalId, string Name)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@strName", Name);
                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet Getinsurancetypecode()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select * From GetBillingCodes('InsuranceTypeCodes");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetCompany(int HospitalId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@HospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            // return  objDl.FillDataSet(CommandType.Text, " select  CompanyId ,Name,PaymentType,CardHeaderText from Company where (HospitalLocationId=@HospitalLocationId or HospitalLocationId is null) and Active=1 order by Name", HshIn);
            try
            {

                return objDl.FillDataSet(CommandType.Text, " select  c.CompanyId ,c.Name,c.PaymentType,c.CardHeaderText from Company c inner join FacilityWiseCompany fwc on fwc.CompanyId = c.CompanyId where (c.HospitalLocationId=@HospitalLocationId or c.HospitalLocationId is null) and c.Active=1 and fwc.FacilityId=@intFacilityId  order by Name ", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetCompany(int HospitalId, int FacilityId, string UseFor)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@HospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@UseFor", UseFor);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return dl.FillDataSet(CommandType.StoredProcedure, "USPGetCompanyForReport", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetHospitalServices(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, string sType, string sServiceName, int FacilityId, bool IsGradBased)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chvServiceName", sServiceName);
            HshIn.Add("chrType", sType);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("IsGradBased", IsGradBased);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return dl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetHospitalServices(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, string sType, string sServiceName, int FacilityId, string iIsIncludeOrderSet, int iEntrySiteId, int SecGroupId, int EmployeeId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chvServiceName", sServiceName);
            HshIn.Add("chrType", sType);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("iIsIncludeOrderSet", iIsIncludeOrderSet);
            HshIn.Add("iEntrySiteId", iEntrySiteId);
            if (SecGroupId > 0)
            {
                HshIn.Add("@intSecGroupId", SecGroupId);
            }
            if (EmployeeId > 0)
            {
                HshIn.Add("@intEmployeeId", EmployeeId);
            }
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return dl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetHospitalServices(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, string sType, string sServiceName, int FacilityId, int SecGroupId, int EmployeeId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chvServiceName", sServiceName);
            HshIn.Add("chrType", sType);
            HshIn.Add("intFacilityId", FacilityId);
            if (SecGroupId > 0)
            {
                HshIn.Add("@intSecGroupId", SecGroupId);
            }
            if (EmployeeId > 0)
            {
                HshIn.Add("@intEmployeeId", EmployeeId); 
            }

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {

                return dl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        //yogesh 29/09/2022
        public DataSet GetHospitalServicesByEntrySite(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, string sType, string sServiceName, int FacilityId, int SecGroupId, int EmployeeId, int? EntrySite) //yogesh
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chvServiceName", sServiceName);
            HshIn.Add("chrType", sType);
            HshIn.Add("intFacilityId", FacilityId);
            if (SecGroupId > 0)
            {
                HshIn.Add("@intSecGroupId", SecGroupId);
            }
            if (EmployeeId > 0)
            {//yogesh
                HshIn.Add("@intEmployeeId", EmployeeId);
                if (EntrySite > 0)
                    HshIn.Add("@iEntrySiteId", EntrySite);
                else
                    HshIn.Add("@iEntrySiteId", 0);
            }

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {

                return dl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetHospitalServices(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, string sType, string sServiceName, int FacilityId, string iIsIncludeOrderSet, int iEntrySiteId, string bIsShowAllService)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chvServiceName", sServiceName);
            HshIn.Add("chrType", sType);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("iIsIncludeOrderSet", iIsIncludeOrderSet);
            HshIn.Add("iEntrySiteId", iEntrySiteId);
            HshIn.Add("bIsShowAllService", bIsShowAllService);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetHospitalServices(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, string sType, string sServiceName, int FacilityId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chvServiceName", sServiceName);
            HshIn.Add("chrType", sType);
            HshIn.Add("intFacilityId", FacilityId);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return dl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetYear(int HospitalId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetFinancialYear", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getStatus(int iHospID, string statusType, string Code)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strqry = "SELECT Status, StatusColor, StatusId, Code FROM GetStatus(" + iHospID + " , '" + statusType + "')";
                if (Code != "")
                {
                    strqry += " WHERE Code='" + Code + "'";
                }
                strqry += " ORDER BY SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet SearchEmployeeByName(int iHospId, String SearchText)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("strSearchText", SearchText);
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchEmployeeByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet getMonthYear(string MonthYear)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@type", MonthYear);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspFillMonthYear", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getCompanyNonDiscServices(int HospId, int CompanyId, int DepartmentId, int SubDeptId,
                                                string ServiceName, string RefServiceCode, int CardId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@intSubDeptId", SubDeptId);
                HshIn.Add("@chvServiceName", ServiceName);
                HshIn.Add("@chvRefServiceCode", RefServiceCode);
                HshIn.Add("@intCardId", CardId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyNonDiscServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCompanyNonDiscServices(int HospId, int CompanyId, string xmlServices, int EncodedBy, int CardId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@xmlServices", xmlServices);
            HshIn.Add("@intEncodedBy", EncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@intCardId", CardId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCompanyNonDiscServices", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int InActivateCompanyNonDiscServices(int NDSId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intNDSId", NDSId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE CompanyNonDiscServices SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE NDSId = @intNDSId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public int SaveHospitalLogo(int HospId, string sLogoName, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@chvLogoName", sLogoName);
                HshIn.Add("@intEncodedBy", EncodedBy);
                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE HospitalLocation SET LogoName = @chvLogoName, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE Id = @inyHospitalLocationId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string getHospitalLogo(int HospId)
        {
            String strLogo = "";
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalLocationLogo", HshIn);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strLogo = Convert.ToString(ds.Tables[0].Rows[0]["LogoName"]);
                    }
                }
                return strLogo;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet GetEmployeeClassification(int HospId, int EmployeeTypeId, int EmployeeId, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEmployeeTypeId", EmployeeTypeId);
                HshIn.Add("@intEmployeeId", EmployeeId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeClassification", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEmployeeClassification(int HospId, int EmployeeTypeId, int EmployeeId, int FacilityId, int TheatreId, int WeekDayId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEmployeeTypeId", EmployeeTypeId);
            HshIn.Add("@intEmployeeId", EmployeeId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intTheatreId", TheatreId);
            HshIn.Add("@intWeekDayId", WeekDayId);
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeClassification", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //yogesh 28/06/2022
        public DataSet GetEmployeeClassification(int HospId, int EmployeeTypeId, int EmployeeId, int FacilityId, int TheatreId, int WeekDayId, int ServiceId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEmployeeTypeId", EmployeeTypeId);
            HshIn.Add("@intEmployeeId", EmployeeId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intTheatreId", TheatreId);
            HshIn.Add("@intWeekDayId", WeekDayId);
            HshIn.Add("@ServiceId", ServiceId);
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMREmployeeClassification", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveUpdateEmployeeClassification(int HospId, int EmployeeId, int ClassificationId, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intEmployeeId", EmployeeId);
            HshIn.Add("intClassificationId", ClassificationId);
            HshIn.Add("intEncodedBy", EncodedBy);
            hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateEmployeeClassification", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientAccount(int HospId, int FacilityId, string Fdate, string Tdate)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvFdate", Fdate);
                HshIn.Add("@chvTdate", Tdate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "GetPatientAccountUser", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string GetHospitalDecimalValue(int iHospitalLocationId)
        {

            string sValue = "";
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            string sQuery = "SELECT Value FROM HospitalSetup WHERE Flag='DecimalPlaces' AND HospitalLocationId=@inyHospitalLocationId";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                sValue = objDl.ExecuteScalar(CommandType.Text, sQuery, HshIn).ToString();
                return sValue;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion
        #region Registration
        public DataSet GetPatientRecord(int intRegId, int intHospiLocId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", intRegId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetUnRegistedPatientRecord(int inyHospitalLocationID, int intAppointmentID)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", inyHospitalLocationID);
                HshIn.Add("@intAppointmentId", intAppointmentID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPEMRGetAppointmentDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public Hashtable SaveUpdateRegistration(int iHospitalLocationId, int FacilityId, int PageId, string sPreviousName, int iTitle, string sFirstName, string sMiddleName, string sLastName,
           string sDisplayName, int iEthnicityID, string sYear, string sMonth, string sDay, int iGender, int iMaritalStatus, int iReligionId, string sAddress, string sAddressLine2, string sPin,
            int iCity, int iState, int iCountry, string sEmail, string sPhone, string sMobile, int iNationality, string iReferralTypeID, int iIdentityTypeID, string sIdentityTypeNumber,
            int iSponsorId, int iPayerId, int iFacility, int iResposibleSelf, int iRenderingProvider, int iPCP, int iDefPharmacy, int iRaceID, int iReferralByID, string sStatus,
            int iDefLabID, int iDefImageCenterID, string sResidentType, string sSSN, bool bExternalPatient, int iLeadSourceID, int iEncodedBy, int iRegistrationId, Int64 iRegistrationNo, string sDOB, string sAppointmentID, int iCityAreaId, string sTaggedEmpNo, string sTaggedEmpName)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshOutput = new Hashtable();
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intEncodedBy", iEncodedBy); // mandatory
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intPageId", PageId);
            if (sPreviousName != "") { HshIn.Add("chvPreviousName", sPreviousName); }
            if (Convert.ToString(iTitle) != "" && Convert.ToString(iTitle) != "0") { HshIn.Add("@intTitle", iTitle); }

            HshIn.Add("@chvFirstName", sFirstName); // mandatory
            if (sMiddleName != "") { HshIn.Add("@chvMiddleName", sMiddleName); }

            if (sLastName != "" && sLastName != "0")
            {
                HshIn.Add("@chvLastName", sLastName);
            }

            if (sDisplayName != "" && sDisplayName != "0") { HshIn.Add("@chvDisplayName", sDisplayName); }

            if (FormatDateYearMonthDate(sDOB) != "" && FormatDateYearMonthDate(sDOB) != "0") { HshIn.Add("@chrDateofBirth", FormatDateYearMonthDate(sDOB)); }
            //HshIn.Add("@chrDateofBirth", FormatDateYearMonthDate(sDOB));
            if (sDOB != "__/__/____")
            {
                if (sDOB != "__/__/____")
                {
                    sDOB = FormatDateDateMonthYear(sDOB);
                    sDOB = FormatDateMDY(sDOB);
                    sYear = "";
                    sMonth = "";
                    sDay = "";
                    string[] result = CalculateAge(sDOB);

                    //sYear = result[0];
                    //sMonth = result[1];
                    //sDay = result[2];

                    if (result.Length == 2)
                    {
                        if (result[1].ToUpper() == "YEAR")
                        {
                            sYear = result[0];
                        }
                        else
                        {
                            sDay = result[0];
                        }
                    }

                    if (result.Length == 4)
                    {
                        sMonth = result[0];
                        sDay = result[2];
                    }
                    if (result.Length == 6)
                    {
                        sYear = result[0];
                        sMonth = result[2];
                        sDay = result[4];
                    }



                    if (sYear == "" || sYear == null)
                    {
                        sYear = "0";
                    }
                    if (sMonth == "" || sMonth == null)
                    {
                        sMonth = "0";
                    }
                    if (sDay == "" || sDay == null)
                    {
                        sDay = "0";
                    }
                }
            }
            else
            {
                if (sYear == "")
                    sYear = "0";
                if (sMonth == "")
                    sMonth = "0";
                if (sDay == "")
                    sDay = "0";
            }

            if (iEthnicityID != 0)
            {
                HshIn.Add("@intEthnicityID", iEthnicityID);
            }
            if (Convert.ToString(sYear) != "") { HshIn.Add("@inyAgeYear", sYear); }
            if (Convert.ToString(sMonth) != "") { HshIn.Add("@inyAgeMonth", sMonth); }
            if (Convert.ToString(sDay) != "") { HshIn.Add("@inyAgeDays", sDay); }
            if (Convert.ToString(iGender) != "" && Convert.ToString(iGender) != "0") { HshIn.Add("@inySex", iGender); }
            if (Convert.ToString(iMaritalStatus) != "" && Convert.ToString(iMaritalStatus) != "0") { HshIn.Add("@inyMaritalStatus", iMaritalStatus); }
            if (Convert.ToString(iReligionId) != "" && Convert.ToString(iReligionId) != "0") { HshIn.Add("@inyReligionId", iReligionId); }
            if (sAddress != "" && sAddress != "0") { HshIn.Add("@chvLocalAddress", sAddress); }

            if (sAddressLine2 != "" && sAddressLine2 != "0") { HshIn.Add("@chvLocalAddressLine2", sAddressLine2); }


            if (sPin != "" && sPin != "0") { HshIn.Add("@chvLocalPin", sPin); }
            if (Convert.ToString(iCity) != "" && Convert.ToString(iCity) != "0") { HshIn.Add("@intLocalCity", iCity); }
            if (Convert.ToString(iState) != "" && Convert.ToString(iState) != "0") { HshIn.Add("@intLocalState", iState); }
            if (Convert.ToString(iCountry) != "" && Convert.ToString(iCountry) != "0") { HshIn.Add("@intLocalCountry", iCountry); }
            if (sEmail != "" && sEmail != "0") { HshIn.Add("@chvEmail", sEmail); }
            if (sPhone != "" && sPhone != "0") { HshIn.Add("@chvPhoneHome", sPhone); }
            if (sMobile != "" && sMobile != "0") { HshIn.Add("@chvMobileNo", sMobile); }
            if (Convert.ToString(iNationality) != "" && Convert.ToString(iNationality) != "0") { HshIn.Add("@intNationality", iNationality); }
            if (iReferralTypeID != "") { HshIn.Add("@intReferralId", iReferralTypeID); }
            if (Convert.ToString(iIdentityTypeID) != "" && Convert.ToString(iIdentityTypeID) != "0") { HshIn.Add("@intIdentityTypeID", iIdentityTypeID); }
            if (sIdentityTypeNumber != "" && sIdentityTypeNumber != "0") { HshIn.Add("@chvIdentityNumber", sIdentityTypeNumber); }
            if (Convert.ToString(iSponsorId) != "" && Convert.ToString(iSponsorId) != "0") { HshIn.Add("@intSponsorId", iSponsorId); }
            if (Convert.ToString(iPayerId) != "" && Convert.ToString(iPayerId) != "0") { HshIn.Add("@intPayorId", iPayerId); }
            if (Convert.ToString(iFacility) != "" && Convert.ToString(iFacility) != "0") { HshIn.Add("@intFacility", iFacility); }

            HshIn.Add("@bitResponsibleSelf", iResposibleSelf); // 0 mandatory 
            if (Convert.ToString(iRenderingProvider) != "" && Convert.ToString(iRenderingProvider) != "0") { HshIn.Add("@intRenderingProvider", iRenderingProvider); }
            if (Convert.ToString(iPCP) != "" && Convert.ToString(iPCP) != "0") { HshIn.Add("@intPCP", iPCP); }
            if (Convert.ToString(iDefPharmacy) != "" && Convert.ToString(iDefPharmacy) != "0") { HshIn.Add("@intDefaultPharmacyID", iDefPharmacy); }

            if (Convert.ToString(iRaceID) != "" && Convert.ToString(iRaceID) != "0") { HshIn.Add("@intRaceID", iRaceID); }
            if (iReferralByID != 0) { HshIn.Add("@intReferredByID", iReferralByID); }
            if (sStatus != "" && sStatus != "0") { HshIn.Add("@chrStatus", sStatus); }
            if (Convert.ToString(iDefLabID) != "" && Convert.ToString(iDefLabID) != "0") { HshIn.Add("@intDefLabID", iDefLabID); }
            if (Convert.ToString(iDefImageCenterID) != "" && Convert.ToString(iDefImageCenterID) != "0") { HshIn.Add("@intDefImageCenterID", iDefImageCenterID); }
            if (sResidentType != "" && sResidentType != "0") { HshIn.Add("@chrResidentType", sResidentType); }
            if (sSSN != "" && sSSN != "0") { HshIn.Add("@chvSocialSecurityNo", sSSN); }
            HshIn.Add("@bitExternalPatient", bExternalPatient);
            if (iLeadSourceID != 0) { HshIn.Add("@intLeadSourceId", iLeadSourceID); }
            HshIn.Add("@intCityAreaId", iCityAreaId);

            if (sTaggedEmpNo != "") { HshIn.Add("@sTaggedEmpNo", sTaggedEmpNo); }
            if (sTaggedEmpName != "") { HshIn.Add("@sTaggedEmpName", sTaggedEmpName); }

            if (iRegistrationId == 0)
            {
                HshIn.Add("@intAppointmentId", sAppointmentID);
                hshOutput.Add("@intRegistrationNo", SqlDbType.BigInt);
                hshOutput.Add("@intRegistrationId", SqlDbType.Int);

                hshOutput = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveRegistration", HshIn, hshOutput);
                return hshOutput;

            }
            else
            {
                HshIn.Add("@intRegistrationNo", iRegistrationNo);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                int iCount = dl.ExecuteNonQuery(CommandType.StoredProcedure, "USPUpdateRegistration", HshIn);
                return hshOutput;
            }

        }


        public string FormatDateYearMonthDate(string strDateOfBirth)
        {
            if (strDateOfBirth != "" && strDateOfBirth != "__/__/____" && strDateOfBirth != null)
            {
                //input string should be in dd/MM/YYYY
                char[] chSperator = new char[1];
                if (strDateOfBirth.Contains("/"))
                    chSperator[0] = '/';
                else
                    chSperator[0] = '-';
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[2].IndexOf(' ') != -1)
                    str.Append(strResult[2].Substring(0, strResult[2].IndexOf(' ')));
                else
                    str.Append(strResult[2]);
                str.Append("/");
                str.Append(strResult[1]);
                str.Append("/");
                str.Append(strResult[0]);
                //output string is in yyyy/MM/dd format
                return str.ToString();
            }
            return null;
        }

        public void imgCalYear_Click(string sDOB, string sYear, string sMonth, string sDay)
        {


        }
        public string FormatDateDateMonthYear(string strDateOfBirth)
        {
            if (strDateOfBirth != "" && strDateOfBirth != "__/__/____")
            {
                //input string should be in MM/dd/yyyy
                char[] chSperator = { '/' };
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[1].Length == 1)
                    str.Append("0" + strResult[1]);
                else
                    str.Append(strResult[1]);
                str.Append("/");
                if (strResult[0].Length == 1)
                    str.Append("0" + strResult[0]);
                else
                    str.Append(strResult[0]);
                str.Append("/");
                str.Append(strResult[2]);
                //output string is in dd/MM/yyyy format
                return str.ToString();
            }
            else
                return null;
        }
        public string FormatDateMDY(string strDateOfBirth)
        {
            if (strDateOfBirth != "" && strDateOfBirth != "__/__/____" && strDateOfBirth != null)
            {
                //input string should be in MM/dd/YYYY
                char[] chSperator = new char[1];
                if (strDateOfBirth.Contains("/"))
                    chSperator[0] = '/';
                else
                    chSperator[0] = '-';
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[2].IndexOf(' ') != -1)
                    str.Append(strResult[2].Substring(0, strResult[2].IndexOf(' ')));
                else
                    str.Append(strResult[2]);
                str.Append("/");
                str.Append(strResult[0]);
                str.Append("/");
                str.Append(strResult[1]);
                //output string is in yyyy/MM/dd format
                return str.ToString();
            }
            return null;
        }

        public string[] CalculateAge(string strDateOfBirth)
        {
            string strAge = null;
            string[] strResult = new string[6];

            try
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                strAge = (String)objDl.ExecuteScalar(CommandType.Text, "select dbo.AgeInYrsMonthDay('" + strDateOfBirth + "' ,convert(varchar,getdate(),111))");
                char[] sep = { ' ' };
                string[] strDays = strAge.Split(sep);



                return strDays;
            }
            catch (Exception ex)
            {
                throw new Exception("There is some problem in conversion process - " + ex.Message);
            }

        }



        public DataSet GetReferType(int HospitalId, string Refertype)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospitalId", HospitalId);
                HshIn.Add("@chvRefType", Refertype);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetReferByDoctor", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetPCP(int HospitalId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select contactid,ISNULL(firstname,'')+' '+ISNULL(middlename,'')+' '+ISNULL(lastname,'') as ContactName from orgcontacts where  HospitalLocationID=" + HospitalId + " and ISNULL(FirstName,'') <> '' AND Active = 1");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetReferBy()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetReferby");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }




        public DataSet GetRace()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select RaceID,Race from racemaster where Active=1 order by Race");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet populateDetailsFromDocAppointment(int AppID, int HospitalId)
        {
            StringBuilder strSQL = new StringBuilder();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@AppID", AppID);
                HshIn.Add("@HospID", HospitalId);

                strSQL.Append(" select AppointmentId,HospitalLocationId,FacilityID,PatientName,DateofBirth,");
                strSQL.Append(" AgeYears,AgeMonths,AgeDays,Gender,ResidencePhone,Mobile,ISNULL(Email,'')Email,Remarks,");
                strSQL.Append(" ISNULL(SocialSecurityNo,'')SocialSecurityNo,OtherIdentityType,OtherIdentityNo");
                strSQL.Append(" from doctorappointment where HospitalLocationID=@HospID and Active=1 and appointmentID=@AppID");
                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindPharmacy()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select cont.ContactID,cont.CompanyName  from OrgContacts cont inner join OrgContactCategories cat on cont.CategoryID=cat.CategoryID  where cat.Name='Pharmacy'");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet BindImageCenter()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select  cont.ContactID,cont.CompanyName  from OrgContacts cont inner join OrgContactCategories cat on cont.CategoryID=cat.CategoryID  where cat.Name='Imaging Center'");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet BindLab()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select  cont.ContactID,cont.CompanyName  from OrgContacts cont inner join OrgContactCategories cat on cont.CategoryID=cat.CategoryID  where cat.Name='Lab'");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        // ********************  Patient IContacts Deatils ***********************************************



        public String SaveRegistrationContact(int iRegId, Int64 iUHID, int iHospID, int iContactGroupID,
            String sName, String sAddress, String sAddressLine2, String sPin,
            String sPhone, String sMob, String sEmail, int iUser, Byte bEmerContact,
            Byte bResponsiblePerson, Byte bStatus, int CountryId, int StateId, int CityId, int AreaId,
            int iUpdateID, String sDOB, int FacilityId, int PageId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@intRegistrationID", iRegId);
            HshIn.Add("@intRegistrationNo", iUHID);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyContactGroupID", iContactGroupID);
            HshIn.Add("@chvName", sName);
            HshIn.Add("@chvAddress", sAddress);
            HshIn.Add("@chvLocalAddressLine2", sAddressLine2);
            HshIn.Add("@chvPin", sPin);
            HshIn.Add("@chvPhone", sPhone);
            HshIn.Add("@chvMobile", sMob);
            HshIn.Add("@chvEmail", sEmail);
            HshIn.Add("@bitEmergencyContact", bEmerContact);
            HshIn.Add("@bitResposiblePerson", bResponsiblePerson);
            HshIn.Add("@CountryId", CountryId);
            HshIn.Add("@StateId", StateId);
            HshIn.Add("@CityId", CityId);
            HshIn.Add("@AreaId", AreaId);
            HshIn.Add("@bitActive", bStatus);
            HshIn.Add("@intEncodedBy", iUser);
            HshIn.Add("@intUpdateID", iUpdateID);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intPageId", PageId);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateRegistrationContacts", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetContactGroup(int iRegistrationId, int iHospitalLocationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetContactGroup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public String SaveContactGroup(String SGroupName, Int32 iEncodedBy, Int32 iHospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("chvGroupName", SGroupName);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("@inyHospitalLocationId", iHospId);
            hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveContactGroup", HshIn, hshOutput);
                return hshOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetRegistrationContacts(int iHospID, int iGroupID, int iRegID, int iID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("inyGroupID", iGroupID);
            HshIn.Add("intRegistrationID", iRegID);
            HshIn.Add("intID", iID);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationContacts", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public void SaveupdateGroup(int contactid, int HospitalId, string Groupname, int Status, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@intContactId", contactid);
            HshIn.Add("@intHospitalLlocationId", HospitalId);
            HshIn.Add("@chvGroupName", Groupname);
            HshIn.Add("@bitActive", Status);
            HshIn.Add("@intEncodedBy", EncodedBy);
            try
            {

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSavePatientContactGroups", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientContactGroups(int HospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientContactGroups", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetContactGroupMaster(string GroupName, int HospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();


            HshIn.Add("@chvName", GroupName);
            HshIn.Add("@intHospitalLocationId", HospitalId);

            string str1 = "select Name from ContactGroupMaster where Name=@chvName and HospitalLocationId=@intHospitalLocationId";
            try
            {

                return objDl.FillDataSet(CommandType.Text, str1, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        ///// ************************************* Patient Other Details ********************************************

        public DataSet GetAppointmentNotification()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Method from appointmentnotification where Active=1 order by Method");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEthnicity()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select ID,Ethnicity from ethnicitymaster where Active=1 order by Ethnicity");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientOccupation()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "SELECT Id, Name FROM OccupationMaster WHERE (Active = '1') ORDER BY Name");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEmploymentStatus()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select StatusId, Status From GetStatus(1,'Employment')");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetStudentStatus()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select StatusId, Status From GetStatus(1,'Student')");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveRegistrationOtherDetails(int iRegistrationId, Int64 iRegistrationNo, int iHospitalLocationId, string sWorkAddress, string sWorkAddress2,
            int iWorkCountry, int iWorkState, int iWorkCity, string sWorkPin, string sWorkNumber, string sWorkExtn, string sWorkEmail, int iConfidentialComm,
            int iVFCProviderID, string sLockRecord, int VIPByte, string sVIPNarration, int bNewBorn, int iBirthOrder, int bRegistryExclude, int iEnableAllert,
            string sSpecialNote, string sAppointmentNotification, int iEncodedBy, int sRemovePassword, int iEmploymentStatusID, string sEmployerName, string sEmployeeNo,
            int iStudentStatusID, string sSchoolCollege, string dtDeathdeth, string chvCuaseofdeth, string sNotificationphone, string sNotificationType, int iOccupationId,
            int iShowNoteInAppointment, int iLanguage, int FacilityId, int PageId, string sPrivateKey)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshOutput = new Hashtable();
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intRegistrationNo", iRegistrationNo);
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@chvWorkAddress1", sWorkAddress);
                HshIn.Add("@chvWorkAddress2", sWorkAddress2);
                if (Convert.ToString(iWorkCountry) != "" && Convert.ToString(iWorkCountry) != "0") { HshIn.Add("@intCountryID", iWorkCountry); }
                if (Convert.ToString(iWorkState) != "" && Convert.ToString(iWorkState) != "0") { HshIn.Add("@intStateID", iWorkState); }
                if (Convert.ToString(iWorkCity) != "" && Convert.ToString(iWorkCity) != "0") { HshIn.Add("@intCityID", iWorkCity); }
                if (sWorkPin != "" && sWorkPin != "0") { HshIn.Add("@chvWorkPin", sWorkPin); }
                //   HshIn.Add("@chvWorkPin", sWorkPin);
                if (sWorkNumber != "" && sWorkNumber != "0")
                {
                    HshIn.Add("@chvWorkNumber", sWorkNumber);
                }
                HshIn.Add("@chvWorkExtn", sWorkExtn);
                HshIn.Add("@chvWorkEmail", sWorkEmail);
                if (iConfidentialComm != 0)
                {
                    HshIn.Add("@intConfidentialComm", iConfidentialComm);
                }
                HshIn.Add("@intVFCProviderID", iVFCProviderID);
                HshIn.Add("@chvLockRecord", sLockRecord);
                HshIn.Add("@bitVIP", VIPByte);
                HshIn.Add("@chvVIPNarration", sVIPNarration);
                HshIn.Add("@bitNewBorn", bNewBorn);
                HshIn.Add("@inyBirthOrder", iBirthOrder);
                HshIn.Add("@bitRegistryExclude", bRegistryExclude);
                HshIn.Add("@intEnableAllert", iEnableAllert);
                HshIn.Add("@chvNote", sSpecialNote);
                HshIn.Add("@chvAppointmentNotification", sAppointmentNotification);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@bitRemovePassword", sRemovePassword);
                if (iEmploymentStatusID != 0 && iEmploymentStatusID != null)
                {
                    HshIn.Add("@intEmploymentStatusID", iEmploymentStatusID);
                }
                HshIn.Add("@chvEmployerName", sEmployerName);
                HshIn.Add("@chvEmployeeNumber", sEmployeeNo);
                if (iStudentStatusID != 0 && iStudentStatusID != null)
                {
                    HshIn.Add("@intStudentStatusID", iStudentStatusID);
                }
                HshIn.Add("@chvSchoolCollege", sSchoolCollege);
                HshIn.Add("@chrDateOfDeath", dtDeathdeth);
                HshIn.Add("@chvCauseOfDeath", chvCuaseofdeth);
                HshIn.Add("@chrNotificationPhoneNo", sNotificationphone);
                HshIn.Add("@chrNotificationPhoneType", sNotificationType);
                if (iOccupationId != 0 && iOccupationId != null)
                {
                    HshIn.Add("@intOccupationId", iOccupationId);
                }
                if (iShowNoteInAppointment != 0 && iShowNoteInAppointment != null)
                {
                    HshIn.Add("@bitShowNoteInAppointment", iShowNoteInAppointment);
                }

                HshIn.Add("@intLanguageID", iLanguage);

                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@chvPrivateKey", sPrivateKey);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateRegistrationOtherDetails", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationOtherDetails(int iRegId, int iHospID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", iRegId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationOtherDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        /// ******************************* Responsible Party *******************************************

        public DataSet GetRegistrationGuarantor(int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationGuarantor", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet PopulateGuarantordetails(int intGuarantorId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", intGuarantorId);

                sb.Append("Select   GuarantorId,RegistrationId, GuarantorRegistrationId, FirstName,");
                sb.Append(" MiddleName, LastName, RelationshipId,");
                sb.Append(" GenderId, dbo.GetDateFormat(DateofBirth,'D')as DOB,");
                sb.Append(" IdentityTypeId, IdentityNumber, Address, Address2, CountryId,");
                sb.Append(" StateId, CityId, ZipCode, Phone, Mobile, Email, EncodedBy, EncodedDate,");
                sb.Append(" EmployerName, EmployerAddress1, EmployerAddress2, EmployerPhone");
                sb.Append(" From RegistrationGuarantorDetail where GuarantorId =@intRegistrationId");

                return objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationDetail(int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetGurantorInfo()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, "select KinId,KinName from KinRelation order by KinName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveRegistrationGuarantor(int EncodedBy, int GuarantorId, int RegistrationId, int HospitalId, int GuarantorRegId, int GuarantorRegNo,
            string FName, string MName, string LName, int RelationshipId, int GenderId, DateTime DTO, int IdentitytypeId, string Identityno, int Country, int State,
            int City, string zipcode, string Phoneno, string Mobileno, string Email, string SSN, string Address1, string Address2, string EmpName, string EmpAddress1,
            string EmpAddress2, int EmpCountry, int EmpState, int EmpCity, string EmpZipcod, string EmpPhone, int FacilityId, int PageId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intGuarantorId", GuarantorId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intGuarantorRegistrationId", GuarantorRegId);
                HshIn.Add("@intGuarantorRegistrationNo", GuarantorRegNo);
                HshIn.Add("@chvFirstName", FName);
                HshIn.Add("@chvMiddleName", MName);
                HshIn.Add("@chvLastName", LName);
                if (Convert.ToString(RelationshipId) != "0")
                {
                    HshIn.Add("@intRelationshipId", RelationshipId);
                }
                HshIn.Add("@inyGenderId", GenderId);

                if (Convert.ToString(DTO) != "")
                {
                    HshIn.Add("@dtsDateOfBirth", DTO.ToString("dd/MM/yyyy"));
                }
                if (Convert.ToString(IdentitytypeId) != "0")
                {
                    HshIn.Add("@inyIdentityTypeId", IdentitytypeId);
                }
                HshIn.Add("@chvIdentityNumber", Identityno);

                if (Convert.ToString(Country) != "0")
                {
                    HshIn.Add("@intCountryId", Country);
                }
                if (Convert.ToString(State) != "")
                {
                    HshIn.Add("@intStateId", State);
                }
                if (Convert.ToString(City) != "")
                {
                    HshIn.Add("@intCityId", City);
                }
                if (Convert.ToString(zipcode) != "Select" && (Convert.ToString(zipcode) != ""))
                {
                    HshIn.Add("@chvZipCode", zipcode);
                }
                else
                {
                    HshIn.Add("@chvZipCode", DBNull.Value);
                }
                if (Phoneno != "___-___-____")
                    HshIn.Add("@chvPhone", Phoneno);
                if (Mobileno != "___-___-____")
                    HshIn.Add("@chvMobile", Mobileno);
                HshIn.Add("@chvEmail", Email);
                HshIn.Add("@chvSocialSecurityNo", SSN);
                HshIn.Add("@chvAddress", Address1);
                HshIn.Add("@chvAddress2", Address2);
                HshIn.Add("@chvEmployerName", EmpName);
                HshIn.Add("@chvEmployerAddress1", EmpAddress1);
                HshIn.Add("@chvEmployerAddress2", EmpAddress2);
                if (Convert.ToString(EmpCountry) != "0")
                    HshIn.Add("@intEmployerCountryId", EmpCountry);
                if (Convert.ToString(EmpState) != "")
                    HshIn.Add("@intEmployerStateId", EmpState);
                if (Convert.ToString(EmpCity) != "")
                    HshIn.Add("@intEmployerCityId", EmpCity);
                if (Convert.ToString(EmpZipcod) != "Select")
                    HshIn.Add("@chvEmployerZipCode", EmpZipcod);
                else
                    HshIn.Add("@chvEmployerZipCode", DBNull.Value);

                HshIn.Add("@chvEmployerPhone", EmpPhone);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateRegistrationGuarantor", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void DeleteRegistrationGuarantor(int GuarantorId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@GuarantorID", GuarantorId);
                string str = " Update RegistrationGuarantorDetail set Active=0 where GuarantorId = @GuarantorID";

                objDl.ExecuteNonQuery(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetGuarantorName(int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegId);
                string str = " select GuarantorId,GuarantorRegistrationId  from RegistrationGuarantorDetail  where RegistrationId=@intRegistrationId and GuarantorRegistrationId=@intRegistrationId and Active=1";

                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }


        //// ************************* Patient Payer *****************************************

        public DataSet GetKinName()
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, "Select KinId,KinName from KinRelation Order by KinName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetRegistrationpaymentdetails(int CaseId, int RegistrationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intCaseId", CaseId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationPaymentDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetCaseName(int HospitalId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intHospitalLocationId", HospitalId);
                return objDl.FillDataSet(CommandType.Text, "Select CaseId,CaseName  from RegistrationPaymentCases where (HospitalLocationId=@intHospitalLocationId or HospitalLocationId is null) and Active=1 ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet DefaultCaseId(int RegistrationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                return objDl.FillDataSet(CommandType.Text, "Select DefaultPaymentCaseId from Registration where Id=@intRegistrationId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public void DeleteInsuranceDeatils(int Id)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Id", Id);
                string str = "Update RegistrationInsuranceDetail Set Active=0 where Id=@Id";

                objDl.ExecuteNonQuery(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetInsuranceDetails(int CaseId, int RegistrationId, string InsuranceOrder)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intCaseId", CaseId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@chrInsuranceOrder", InsuranceOrder);

                return objDl.FillDataSet(CommandType.Text, "select InsuranceOrder from RegistrationInsuranceDetail where RegistrationId=@intRegistrationId and CaseId=@intCaseId and InsuranceOrder=@chrInsuranceOrder and Active=1", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveRegistrationInsuranceDetails(int DefaultCase, int Signaturefile, int AssignBinifits, int NoInsurrance, int CaseId, int HospitalId, int RegId,
            int InsuranceCmpId, int Insurancetypecode, string InsuranceOrder, string Insuranceplanname, int GuranterId, string InsuranceId, string Policyno, string Fromdate,
            string ToDate, string GroupNo, string Groupname, string PayClaimOfficeNo, int patientrelease, int AssignmentBenefits, int ClaimFilingIndicatorCode, int ppohmoindicator,
            int KenPac, string PPOIdentification, string authorization, string CopayAmount, int EncodedBy, string NoOfEncounter, int FacilityID, int PageId, string Status, int Id)
        {
            string strsave = "";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@bitDefaultCase", DefaultCase);
                HshIn.Add("@bitDefaultSignatureOnFile", Signaturefile);
                HshIn.Add("@bitDefaultAssignmentBenefit", AssignBinifits);
                HshIn.Add("@bitNoInsurance", NoInsurrance);
                HshIn.Add("@intCaseId ", CaseId);
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intInsuranceId", InsuranceCmpId);
                HshIn.Add("@intInsuranceTypeCode", Insurancetypecode);
                HshIn.Add("@chrInsuranceOrder", InsuranceOrder);
                HshIn.Add("@chvInsurancePlanName", Insuranceplanname);
                HshIn.Add("@intGuarantorId", GuranterId);
                HshIn.Add("@chvInsuredId", InsuranceId);
                HshIn.Add("@chvPolicyNo", Policyno);
                if (Convert.ToString(Fromdate) != "")
                {
                    HshIn.Add("@chvStartDate", Fromdate);
                }
                if (Convert.ToString(ToDate) != "")
                {
                    HshIn.Add("@chvEndDate", ToDate);
                }
                HshIn.Add("@chvGroupNo", GroupNo);
                HshIn.Add("@chvGroupName", Groupname);
                HshIn.Add("@chvPayClaimOfficeNo", PayClaimOfficeNo);
                HshIn.Add("@bitPatientReleaseOnFile ", patientrelease);
                HshIn.Add("@bitPatientAssignedBenefits", AssignmentBenefits);
                HshIn.Add("@chvClaimFilingIndicatorCode", ClaimFilingIndicatorCode);

                HshIn.Add("@bitPPO_HMO_Indicator", ppohmoindicator);
                HshIn.Add("@bitKenPac", KenPac);
                HshIn.Add("@chvPPO_Identification", PPOIdentification);
                HshIn.Add("@chvAuthorizationNo", authorization);
                if (CopayAmount != "")
                {
                    HshIn.Add("@CoPayAmt", Convert.ToDouble(CopayAmount));
                }
                HshIn.Add("@intEncodedBy", EncodedBy);
                if (NoOfEncounter != "")
                {
                    HshIn.Add("@NoOfEncounter", Convert.ToInt32(NoOfEncounter));
                }
                HshIn.Add("@intLoginFacilityId", FacilityID);
                HshIn.Add("@intPageId", PageId);

                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                if (Status == "New")
                {
                    HshIn.Add("@Status", "New");
                    hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpateRegistrationInsuranceDetails", HshIn, hshOutput);

                }
                else
                {
                    HshIn.Add("@Status", "Edit");
                    HshIn.Add("@Id", Id);
                    hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpateRegistrationInsuranceDetails", HshIn, hshOutput);

                }

                return strsave;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string OSaveRegistrationInsuranceDetails(int id, int CaseId, int HospitalId, int RegId, int WorkmanCompensation, int autoaccident, int state,
            int otheraccident, int FacilityID, int PageId, int billonlytopatient, string InitialTreatmentDate, string HospitalizationFromDate, string HospitalizationToDate,
            string MissedWorkFromDate, string MissedWorkToDate, string Disablefrmdt, string DisabledToDate, string Attorney, int EncodedBy)
        {
            int i;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@intId", id);
                HshIn.Add("@intCaseId", CaseId);
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@bitWorkmanCompensation", WorkmanCompensation);
                HshIn.Add("@bitAutoAccident", autoaccident);
                HshIn.Add("@intAccidentOccuredState", state);
                HshIn.Add("@bitOtherAccident", otheraccident);
                HshIn.Add("@intLoginFacilityId", FacilityID);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@bitBillOnlyToPatient", billonlytopatient);

                HshIn.Add("@chrInitialTreatmentDate", InitialTreatmentDate);

                if (Convert.ToString(HospitalizationFromDate) != "")
                {
                    HshIn.Add("@chrHospitalizationFromDate", HospitalizationFromDate);
                }

                if (Convert.ToString(HospitalizationToDate) != "")
                {
                    HshIn.Add("@chrHospitalizationToDate", HospitalizationToDate);
                }

                if (Convert.ToString(MissedWorkFromDate) != "")
                {
                    HshIn.Add("@chrMissedWorkFromDate", MissedWorkFromDate);
                }

                if (Convert.ToString(MissedWorkToDate) != "")
                {
                    HshIn.Add("@chrMissedWorkToDate", MissedWorkToDate);
                }

                if (Convert.ToString(Disablefrmdt) != "")
                {
                    HshIn.Add("@chrDisabledFromDate", Disablefrmdt);
                }

                if (Convert.ToString(DisabledToDate) != "")
                {
                    HshIn.Add("@chrDisabledToDate", DisabledToDate);
                }

                HshIn.Add("@chvAttorney", Attorney);
                HshIn.Add("@bitActive", 1);
                HshIn.Add("@intEncodedBy", EncodedBy);

                i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveRegistrationCaseDetails", HshIn);

                return i.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GurantorRelation(int GuarantorId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intGuarantorId", GuarantorId);
                return objDl.FillDataSet(CommandType.Text, "Select kr.kinId,kr.KinName,rgd.SocialSecurityNo  From RegistrationGuarantorDetail rgd  inner join KinRelation kr on rgd.RelationshipId = kr.KinId  where rgd.GuarantorId=@intGuarantorId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet IndicatorCode()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "SELECT Code,Description FROM IndicatorCode");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet ClaimFilingIndicatorCode(int CompanyId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intCompanyId", CompanyId);
                return objDl.FillDataSet(CommandType.Text, "select ic.Description,ic.Code from company cm inner join IndicatorCode ic on cm.ClaimFilingIndicatorCode=ic.Code where cm.CompanyId=@intCompanyId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetState()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, "select StateId,StateName  from StateMaster where Active=1 order by StateName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetPreviouscase(int CaseId, string Casename, int Hospitalid)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@CaseID", CaseId);
                HshIn.Add("@chvCaseName", Casename);
                HshIn.Add("@HospitalLocationId", Hospitalid);

                if (CaseId == 0)
                    return objDl.FillDataSet(CommandType.Text, "select CaseId  from RegistrationPaymentCases where  CaseName=@chvCaseName  and HospitalLocationId=@HospitalLocationId and Active=1", HshIn);
                else
                    return objDl.FillDataSet(CommandType.Text, "select CaseId  from RegistrationPaymentCases where  CaseName=@chvCaseName and CaseId!=@CaseID and HospitalLocationId=@HospitalLocationId  and Active=1", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public void SaveCasename(int Caseid, int HospitalId, string Casename, int Encodedby)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intCaseId", Caseid);
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@chvCaseName", Casename);
                HshIn.Add("@intEncodedBy", Encodedby);

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSavePaymentCase", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPaymentCase(int HospitalId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPaymentCase", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCase(int HospitalId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                string str1 = "select CaseId,CaseName  from RegistrationPaymentCases where HospitalLocationId=@inyHospitalLocationId and Active=1";
                return objDl.FillDataSet(CommandType.Text, str1, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        // ******************** Document Attachment ************************************************

        public DataSet RetrievePatientDocuments(int RegId, int Encid, int ImageId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", Encid);
                HshIn.Add("@OnlyImage", ImageId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspRetrievePatientDocuments", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetFileName(string sId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@Id", sId);
                return objDl.FillDataSet(CommandType.Text, "select ImagePath, ImageName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks from EMRPatientDocuments where id = @Id", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet BindImage(int RegId, int ImageCategoryId, string StartWidth, int CheckAll, string sFileName)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strSQL = "";
                HshIn = new Hashtable();
                if (RegId != 0)
                {
                    HshIn.Add("@ImageCategoryId", ImageCategoryId);
                    if (StartWidth == "c")
                    {
                        HshIn.Add("@RegistrationId", RegId);
                        if (CheckAll == 1)
                        {
                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd";
                            strSQL += " where RegistrationID = @RegistrationId  AND Active=1";
                        }
                        else
                        {

                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd";
                            strSQL += " where RegistrationID = @RegistrationId And ImageCategoryId = @ImageCategoryId  AND Active=1";
                        }
                    }

                    else
                    {

                        strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                        strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                        strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                        strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd";
                        strSQL += " where pd.Id = @ImageCategoryId  AND Active=1";
                    }
                    return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                }
                else
                {

                    if (CheckAll == 1)
                    {

                        HshIn.Add("RegistrationId", RegId);
                        strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                        strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                        strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                        strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd";
                        strSQL += " where RegistrationID = @RegistrationId  AND Active=1";

                    }
                    else
                    {
                        HshIn.Add("@ImageCategoryId", ImageCategoryId);
                        if (sFileName == "p")
                        {
                            HshIn.Add("RegistrationId", RegId);
                            HshIn.Add("ImageCategoryId", ImageCategoryId);
                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd";
                            strSQL += " where RegistrationID = @RegistrationId And ImageCategoryId = @ImageCategoryId  AND Active=1   order by slno";
                        }
                        else if (sFileName == "c")
                        {
                            HshIn.Add("ImageCategoryId", ImageCategoryId);
                            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
                            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
                            strSQL += " else ImagePath end as ImagePath, ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks ,";
                            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd ";
                            strSQL += " where pd.Id = @ImageCategoryId  AND Active=1 order by slno ";

                        }

                    }
                    return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }





        }
        public DataSet GetDocumentCategory(int HospitalId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strSQL = "";

                strSQL = "SELECT id, Description FROM EMRDocumentCategory where  (HospitalLocationId=" + HospitalId + " or HospitalLocationId is null) and Active = 1 order by Description";
                return objDl.FillDataSet(CommandType.Text, strSQL);
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }



        }

        public DataSet GetGroupmaster(int GroupId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strSQL = "";

                HshIn = new Hashtable();
                HshIn.Add("@intGroupId", GroupId);
                strSQL = "select GroupId from SecGroupMaster where GroupId= @intGroupId and Admin=1 and Active=1";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public void DeleteDoc(int Id)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", Id);
                string str = "update EMRPatientDocuments set Active=0 where Id = @ID";

                objDl.ExecuteNonQuery(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet DisplayDocment(int Id)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strSQL = "";
                string[] strdocType = { ".pdf", ".doc", ".xls", ".docx", ".xlsx", ".txt", ".zip" };
                HshIn = new Hashtable();

                HshIn.Add("@ImageCategoryId", Id);
                strSQL = "select pd.Id, ImagePath, ImageName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks , dd.Data, dd.Type from EMRPatientDocuments pd INNER JOIN EMRPatientDocumentData dd on pd.Id = dd.DocumentId where pd.id =@ImageCategoryId AND Type in ('" + string.Join("' ,'", strdocType) + "')";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet DisplayDoc()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strSQL = "";
                string[] strdocType = { ".pdf", ".doc", ".xls", ".docx", ".xlsx", ".txt", ".zip" };
                strSQL = "select pd.Id, ImagePath, ImageName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks , dd.Data, dd.Type from EMRPatientDocuments pd INNER JOIN EMRPatientDocumentData dd on pd.Id = dd.DocumentId where pd.id =@ImageCategoryId AND Type in ('" + string.Join("' ,'", strdocType) + "')";
                return objDl.FillDataSet(CommandType.Text, strSQL);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet DownloadImage(int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@RegistrationId", RegId);
                string disString = "select Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'" +
                             "when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'" +
                            "else ImagePath end as ImagePath, ImagePath as FilePath,Type,ImageName " +
                             "from EMRPatientDocuments " +
                             "where RegistrationID = @RegistrationId AND Active=1";

                return objDl.FillDataSet(CommandType.Text, disString, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        //************************* Health Record ******************************************88

        public DataSet GetRegistrationHealthRecords(int RegId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationHealthRecords", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public void EMRMUDLogHealthRecordRequests(int HospitalId, int RegId, int EncId, int DoctorId, int Encodedby)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospitalId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intEncodedBy", EncId);

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogHealthRecordRequests", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationHealthRecordsDetails(int Id)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intId", Id);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationHealthRecordsDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        //public Hashtable SaveRegistrationHealthRecords(int FacilityId,int PageId,int RegId,int Regno,int HospitalId,)
        //{
        //    objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    hshOutput = new Hashtable();

        //    HshIn.Add("@intLoginFacilityId", Session["FacilityID"]);
        //    HshIn.Add("@intPageId", strpage);
        //    HshIn.Add("@intRegistrationId", Convert.ToInt32(txtRegNo.Text));
        //    HshIn.Add("@intRegistrationNo", Convert.ToInt32(txtAccountNo.Text));
        //    HshIn.Add("@inyHospitalLocationID", Convert.ToInt16(Session["HospitalLocationID"]));
        //    if (dtDateRequest.DateInput.Text != "")
        //    {

        //        //daterequest = bC.FormatDate(dtDateRequest.SelectedDate.Value.ToString(), "MM/dd/yyyy", "yyyy/MM/dd");
        //        HshIn.Add("@chrRequestDate", bC.FormatDateDateMonthYear(dtDateRequest.SelectedDate.Value.ToString())); //daterequest = ;// dtDateRequest.SelectedDate.Value.ToString();
        //    }
        //    else
        //    {
        //        lblMessage.Text = "Please select the Date";
        //        return;
        //    }


        //    //hshTableIn.Add("@chrRequestDate", Convert.ToDateTime(daterequest));
        //    if (ddlEmployee.SelectedValue != "" && ddlEmployee.SelectedValue != "0")
        //    {
        //        HshIn.Add("@chvEmpProvidingRecord", ddlEmployee.SelectedValue);
        //    }
        //    else
        //    {
        //        lblMessage.Text = "Please select the employee";
        //        return;
        //    }

        //    if (dtProvidedDate.DateInput.Text != "")
        //    {
        //        //dateProvided = bC.FormatDate(dtProvidedDate.SelectedDate.Value.ToString(), "MM/dd/yyyy", "MM/dd/yyyy");
        //        HshIn.Add("@chrProvidedDate", bC.FormatDateDateMonthYear(dtProvidedDate.SelectedDate.Value.ToString())); //dateProvided = bC.FormatDateDateMonthYear(dtProvidedDate.SelectedDate.Value.ToString());
        //    }
        //    else
        //    {
        //        lblMessage.Text = "Please select the Date";
        //        return;
        //    }

        //    HshIn.Add("@chvWhoRequestedInfo", txtWhoRequested.Text);
        //    HshIn.Add("@chvDescriptionofInfo", txtDescription.Text);
        //    HshIn.Add("@intMethode", ddlMethod.SelectedValue);
        //    HshIn.Add("@intEncodedBy", Session["UserID"]);
        //    hshTableOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //    hshTableOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveRegistrationHealthRecords", HshIn, hshTableOut);

        //}

        public DataSet getRegistrationReportFilter(int HospitalLocationId, int FacilityID, string FromDate, string ToDate, int FilterType, string CTypeId, string CmpId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityID);
                HshIn.Add("@FDate", FromDate);
                HshIn.Add("@TDate", ToDate);
                HshIn.Add("@FilterType", FilterType);
                HshIn.Add("@CTypeId", CTypeId);
                HshIn.Add("@CmpId ", CmpId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetRegistrationReportFilter", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getRegistrationReportData(int HospitalLocationId, int FacilityID, string FromDate, string ToDate, string CTypeId, string CmpId, string ProviderId, string ExcelExp)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityID);
                HshIn.Add("@FDate", FromDate);
                HshIn.Add("@TDate", ToDate);
                HshIn.Add("@CTypeId", CTypeId);
                HshIn.Add("@CmpId", CmpId);
                HshIn.Add("@ProviderId", ProviderId);
                HshIn.Add("@ExcelExport", ExcelExp);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspRegistrationReportData", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getSpouseRegistrationId(int RegistrationId, int FacilityId, int HospId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospId", HospId);

                string disString = "SELECT TOP 1 reg.Id AS RegistrationId, reg.RegistrationNo, enc.Id AS EncounterId, enc.EncounterNo " +
                                " FROM Registration reg " +
                                " INNER JOIN RegistrationGuarantorDetail rg ON reg.Id = rg.GuarantorRegistrationId " +
                                " INNER JOIN Encounter enc ON rg.GuarantorRegistrationId = enc.RegistrationId " +
                                " INNER JOIN KinRelation kr ON rg.RelationshipId = kr.kinId AND kr.Code='01' " +
                                " WHERE rg.RegistrationId = @intRegistrationId AND reg.FacilityID = @intFacilityId AND reg.HospitalLocationId = @intHospId " +
                                " ORDER BY enc.Id DESC";

                return objDl.FillDataSet(CommandType.Text, disString, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }



        // --------------------------- Dynamic ---------------------------------



        public DataSet GetRegistrationDynamicFilterData(string iControlID, int iReportType, string iControlValue)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@FilterColName", iControlID);
                HshIn.Add("@ReportType", iReportType);
                HshIn.Add("@Value", iControlValue);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspFillDependedControl", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetRegistrationDynamicControlToBeFilter(string iControlID, int iReportType)
        {
            string sControlToBeFilter = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvSelectControl", iControlID);
                HshIn.Add("@intReportType", iReportType);
                sControlToBeFilter = (string)objDl.ExecuteScalar(CommandType.StoredProcedure, "UspGetControlToBeFilter", HshIn);
                return sControlToBeFilter;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        //------

        public DataSet GetRegistrationReportData(int iHospitalLocationID, int iUserID, int iFacilityID, string sFromDate, string sToDate, string sSearch, string sGroupBy)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@Locid", iHospitalLocationID);
                HshIn.Add("@UserId", iUserID);
                HshIn.Add("@FacilityId", iFacilityID);
                HshIn.Add("@Fdate", sFromDate);
                HshIn.Add("@Tdate", sToDate);
                HshIn.Add("@FilterCriteria", sSearch);
                HshIn.Add("@GroupHierarchy", sGroupBy);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspPatientRegistrationData", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetUserCustomColumn(int iHospitalLocationID, int iUserID, int iFacilityID, int iReportType)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
                HshIn.Add("@intUserId", iUserID);
                HshIn.Add("@intFacilityId", iFacilityID);
                HshIn.Add("@intReportType", iReportType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUserCustomColumn", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet ReportCustomColumn()
        {
            string sColname = "";

            sColname = "SELECT ColOrder,ColName, 0 MarkCustomColumn FROM ColOrderRegistration";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, sColname);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void SaveRegistrationCustomColumn(int iHospitalLocationId, int iUserId, int iFacilityId, string sCustomColumnXml)
        {

            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intUserId", iUserId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlColumnData", sCustomColumnXml);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                objDl.FillDataSet(CommandType.StoredProcedure, "uspSaveRegistrationCustomColumn", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDynamicFilterColumnValues(int iReportType)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@ReportType", iReportType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetFilterQuery", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDynamicFilterData(string iControlID, int iReportType, string iControlValue)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@FilterColName", iControlID);
                HshIn.Add("@ReportType", iReportType);
                HshIn.Add("@Value", iControlValue);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspFillDependedControl", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetDynamicControlToBeFilter(string iControlID, int iReportType)
        {
            string sControlToBeFilter = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chvSelectControl", iControlID);
                HshIn.Add("@intReportType", iReportType);
                sControlToBeFilter = (string)objDl.ExecuteScalar(CommandType.StoredProcedure, "UspGetControlToBeFilter", HshIn);
                return sControlToBeFilter;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDynamicBillRegister(int iHospitalLocationID, int iUserID, int iFacilityID, string sFromDate, string sToDate, string sSearch, string sGroupBy, string sOutType)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
                HshIn.Add("@UserId", iUserID);
                HshIn.Add("@intFacilityId", iFacilityID);
                HshIn.Add("@FromDate", sFromDate);
                HshIn.Add("@ToDate", sToDate);
                HshIn.Add("@FilterCriteria", sSearch);
                HshIn.Add("@GroupHierarchy", sGroupBy);
                HshIn.Add("@OutType", sGroupBy);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDynamicBillRegister", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void SaveBillRegistrationCustomColumn(int iHospitalLocationId, int iUserId, int iFacilityId, string sCustomColumnXml)
        {

            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intUserId", iUserId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlColumnData", sCustomColumnXml);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                objDl.FillDataSet(CommandType.StoredProcedure, "uspSaveBillRegistrationCustomColumn", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        #endregion
        #region OPbilling
        public DataSet GetOPServiceOrder(int iHospId, int iFacilityId, int iRegistrationId, int iEncounterId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPServiceOrder", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOPBillDetail(int iHospId, int iFacilityId, int iBillId, string sBillNo, int inyBillTypeId, int intYearId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intBillId", iBillId);
                HshIn.Add("chvBillNo", sBillNo);
                HshIn.Add("inyBillTypeId", inyBillTypeId);
                HshIn.Add("intYearId", intYearId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPBillDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public DataSet GetEmployeeDiscountAuthorised(int iEmployeeId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intEmployeeId", iEmployeeId);
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append("SELECT ISNULL(DiscountPrecentage,0) AS DiscountPrecentage FROM Employee ");
                sbSQL.Append("WHERE Id = @intEmployeeId AND Active = 1");

                return objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetServiceChargeSurgery(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
            int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, int iAnesthesiaServiceId, StringBuilder xmlSurgeryServices, DateTime dtOrderDate)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("iEncounterId", iEncId);
                HshIn.Add("intCompanyid", iCompId);
                HshIn.Add("iInsuranceId", iInsId);
                HshIn.Add("iCardId", iCardId);
                HshIn.Add("intBedCategoryId", iBedCatId);
                HshIn.Add("cPatientOPIP", sOPIP);
                HshIn.Add("intOTServiceId", iOTServiceId);
                HshIn.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
                HshIn.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
                HshIn.Add("dtsOrderDate", dtOrderDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getPackageServiceDetails(int PackageId, Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId)
        {
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("packageId", Convert.ToInt32(PackageId));
            HshIn.Add("inyHospitalLocationID", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("intCompanyid", iCompanyId);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", cPatientOPIP);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("iEncounterId", iEncounterId);
            HshIn.Add("intDoctorId", iDoctorId);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPPackageBreakup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCurrencyExchange(int HospitalLocationId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCurrencyExchange", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getChangedServiceRates(string XMLstring, Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, Int32 iRegistrationId, Int32 iEncounterId)
        {
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("XMLstring", Convert.ToString(XMLstring));
            HshIn.Add("inyHospitalLocationID", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("intCompanyid", iCompanyId);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", cPatientOPIP);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("iEncounterId", iEncounterId);



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOpChangedServiceRate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientRegistartionCharges(int HospitalLocationId, int RegistrationId, int intLoginFacilityId, int intCompanyId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intRegistrationId", RegistrationId);
            HshIn.Add("intLoginFacilityId", intLoginFacilityId);
            HshIn.Add("intCompanyId", intCompanyId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientRegistrationCharges", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion
        #region IPbilling
        public DataSet getIPServiceOrder(int HospId, int FacilityId, int RegistrationId, int EncounterId, int BillId, string BillNo)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intBillId", BillId);
                HshIn.Add("@chvBillNo", BillNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPServiceOrder", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// Get IP Summary Record against Encounter
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="iEncounterId"></param>
        /// <param name="sPageType"></param>
        /// <param name="sServType"></param>
        /// <param name="iBillId"></param>
        /// <param name="isDetails"></param>
        /// <param name="sBillNo"></param>
        /// <returns></returns>
        public DataSet uspGetIPServiceOrderSummary(Int32 iHospitalLocationId, Int32 iFacilityId,
            Int32 iRegistrationId, Int32 iEncounterId, String sPageType, String sServType, Int32 iBillId, string Procedure)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("RegId", iRegistrationId);
                HshIn.Add("EncounterId", iEncounterId);
                HshIn.Add("sServType", sServType);
                HshIn.Add("chrFromPage", sPageType);

                if (iBillId > 0)
                {
                    HshIn.Add("BillId", iBillId);

                }

                return objDl.FillDataSet(CommandType.StoredProcedure, Procedure, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// Get Patient Service order Details against a patient encounter
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="iRegistrationId">Patient registration</param>
        /// <param name="iEncounterId">Patient encounter</param>
        /// <param name="sPageType">Type of page</param>
        /// <param name="sServType">Send Multiple/Single service type for filter</param>
        /// <param name="iBillId">If search against bill</param>
        /// <param name="sDepartmentId">For getting Department wise details (Single/Multiple department)</param>
        /// <returns>Dataset</returns>
        public DataSet GetIPServiceOrderDetails(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId,
            Int32 iBillId, Int32 iDepartmentId, string chrDepartmentType)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetIPServiceOrderDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetIPPackageExclusionInclusion(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, Int32 iPackageId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intPackageId", iPackageId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPPackageExclusionInclusion", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientPackageList(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientPackageList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string UpdatePatientPackageOrder(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId,
            Int32 iPackageId, String xmlService, int iIsExcluded)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intPackageId", iPackageId);
                HshIn.Add("xmlServiceList", xmlService);
                HshIn.Add("intExclude", iIsExcluded);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdatePatientPackageOrder", HshIn, hshOutput);
                return hshOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable IncludeServiceInPackageByDateRange(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId,
            Int32 iEncounterId, Int32 iPackageId, string sFromDt, string sToDt, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intPackageId", iPackageId);
                HshIn.Add("intEncodedBy", iEncodedBy);
                HshIn.Add("vcFromDt", sFromDt);
                HshIn.Add("vctoDt", sToDt);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspIncludeServiceInPackageByDateRange", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable IncludeServiceInPackageByDeptAmount(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId,
            Int32 iEncounterId, Int32 iPackageId, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intPackageId", iPackageId);
                HshIn.Add("intEncodedBy", iEncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspIncludeServiceInPackageByDeptAmount", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPackageBreakup(Int32 iHospitalLocationId, Int32 iPackageId, Int32 iCompanyId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intPackageId", iPackageId);
                HshIn.Add("intCompanyId", iCompanyId);
                HshIn.Add("insFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageBreakup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable InsertIPServiceDiscount(Int32 iEncounterId, string sIssueType, Int32 iUserId, String xmlService)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("issuetype", sIssueType);
                HshIn.Add("intUserId", iUserId);
                HshIn.Add("xmlServices", xmlService);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspInsertIPServiceDiscount", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable InsertIPRoomRentManual(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId,
            Int32 iEncounterId, Int32 iBedCategoryId, DateTime dtFromdate, DateTime dtToDate, decimal dCharge, decimal dDiscountPercent, Int32 iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intBedCategoryId", iBedCategoryId);
                HshIn.Add("dtFromDate", dtFromdate);
                HshIn.Add("dtToDate", dtToDate);
                HshIn.Add("mnyCharge", dCharge);
                HshIn.Add("mnyDiscountPercent", dDiscountPercent);
                HshIn.Add("intEncodedBy", iEncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspInsertIPRoomRentManual", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable ChangeCategory(Int32 IntEncounterId, Int32 iHospitalLocationId,
            Int32 FacilityId,
            Int32 intToPayerId, Int32 intToSponsorId, Int32 intToCardId,
            Int32 intFromBillCategory, Int32 intToBillCategory, int vcChangeType,
            int iEncodedBy, DateTime dFromDate, DateTime dTodate)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("IntEncounterId", IntEncounterId);
            HshIn.Add("HospLocationId", iHospitalLocationId);
            HshIn.Add("FacilityId", FacilityId);
            HshIn.Add("intToPayerId", intToPayerId);
            HshIn.Add("intToSponsorId", intToSponsorId);
            HshIn.Add("intToCardId", intToCardId);
            HshIn.Add("intFromBillCategory", intFromBillCategory);
            HshIn.Add("intToBillCategory", intToBillCategory);
            HshIn.Add("vcChangeType", vcChangeType);
            HshIn.Add("FromDate", dFromDate);
            HshIn.Add("ToDate", dTodate);
            HshIn.Add("intUSerCode", iEncodedBy);
            hshOutput.Add("TransferID", SqlDbType.Int);
            hshOutput.Add("ErrorMessage", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPChangeIP_PayerSponsor_BillingCatg", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable InsertUpdateRoomRent(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterID, Int32 iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("insHospitalLocationId", iHospitalLocationId);
                HshIn.Add("insFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterID);
                HshIn.Add("intEncodedBy", iEncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USpInsertUpdateRoomRent", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int GetServiceNonDiscountableStatus(int iHospId, int iFacilityId, int iRegistrationId, int iEncounterID, int iServiceId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            Int16 iStatus = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterID);
                HshIn.Add("intServiceId", iServiceId);
                hshOutput.Add("intStatus", SqlDbType.Int);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetServiceNonDiscountableStatus", HshIn, hshOutput);
                if (hshOutput.Count > 0)
                {
                    if (hshOutput["intStatus"] != null)
                    {
                        iStatus = Convert.ToInt16(hshOutput["intStatus"]);
                    }
                }
                return iStatus;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet uspGetPackageDetails(Int32 iHospId, Int32 iSubDeptId, Int32 iPackageId, Int32 iCompanyId, Int32 FacilityId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("intHospitalLocationId", iHospId);
                HshIn.Add("intSubDeptId", iSubDeptId);
                HshIn.Add("intPackageId", iPackageId);
                HshIn.Add("intCompanyId", iCompanyId);
                HshIn.Add("intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPackageDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientDetailsIP(Int32 iHospId, Int32 iFacilityId, Int32 iRegId, int sRegNo, Int32 UserId, Int32 EncounterId, String sEncouterNo)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("chvRegistrationNo", sRegNo);
                HshIn.Add("intEncodedBy", UserId);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("chvEncounterNo", sEncouterNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetailsIP", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientAdmissionPackageDetails(Int32 iRegId, String sEncounterNo, Int32 iCompanyId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("chvEncounterNo", sEncounterNo);
                HshIn.Add("intCompanyId", iCompanyId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientAdmissionPackageDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Hashtable CancelInvoice(int iHospId, int iFacilityId, int InvoiceId, int UserId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intInvoiceId", InvoiceId);
                HshIn.Add("intEncodedBy", UserId);
                hshOutput.Add("chvCancelInvoiceNo", SqlDbType.VarChar);
                hshOutput.Add("intCancelInvoiceId", SqlDbType.Int);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspInvoiceCancel", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet SearchAdmitedPatientByName(int HospId, int FacilityId, String sKeyword)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("chvKeyword", sKeyword);
                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchAdmitedPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPatientNotes(int HospId, int FacilityId, int iRegId, int iEncounterId, int iNoteType, int EncodedBy)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("inyNoteType", iNoteType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientNotesAndPreferences", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Hashtable SaveUpdatePatientNotes(int iHospId, int iFacilityId, int iRegId, int iEncounterId, int iNoteType, String Notes, int iNoteId, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("inyNoteType", iNoteType);
                HshIn.Add("chvNotes", Notes);
                HshIn.Add("intId", iNoteId);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdatePatientNotes", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPackageSurgeryToFillOrderGrid(int HospId, int FacilityId, int iEncounterId, int iPackageId, int iCompanyId, int IsMainPackage, DateTime dtOrderDate)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intPackageId", iPackageId);
                HshIn.Add("intCompanyId", iCompanyId);
                HshIn.Add("intIsMainPackage", IsMainPackage);
                HshIn.Add("dtsOrderDate", dtOrderDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageSurgeryToFillOrderGrid", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string TagReceiptWithEncounter(int RegId, int EncounterId, int EmpId, String xmlReceiptIds)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("intTagRegistrationId", RegId);
                HshIn.Add("intTagEncounterId", EncounterId);
                HshIn.Add("intEncodedEmpId", EmpId);
                HshIn.Add("xmlReceiptIds", xmlReceiptIds);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspTagReceiptWithEncounter", HshIn, hshOutput);
                return hshOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientEncounterStatusList(int HospId, int FacilityId, int EncounterId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterStatusList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetPatientDiagnosisandDiet(int HospId, int FacilityId, int RegId, int EncounterId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intRegistrationId", RegId);
                HshIn.Add("intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDiagnosisandDiet", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        #endregion
        #region Service
        public DataSet getBillDispatch(int HospitalLocationId, int FacilityId, string OPIP, string DateSearchon, string Fromdate, string ToDate,
           int Company, string Searchon, string Searchvalue, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrOPIP", OPIP);
                HshIn.Add("@intBilltypeSearchon", DateSearchon);
                HshIn.Add("@chrFromDate", Fromdate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@intCompany", Company);
                HshIn.Add("@chrSearchon", Searchon);
                HshIn.Add("@chvSearchvalue", Searchvalue);

                HshIn.Add("@intEncodedby", EncodedBy);
                hshOutput.Add("@chvErrorMessage", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBillDispatch", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveBillDispatch(int iHospitalLocationId, int iFacilityId, string xmlData, int intEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@xmlBillDispatch", xmlData);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput.Add("@DispatchNo", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBillDispatch", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelBillDispatch(int iHospitalLocationId, string xmlData, int intEncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@xmlBillDispatch", xmlData);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPCancelBillDispatch", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        public Hashtable SaveKOTOrder(int iHospitalLocationId, int iFacilityId, int RegId, int EncounterId, int BedId, int WardId, int ReasonId, String Description, string KOTStatus, string OPIP, string HostName, string IPAddress, int Active, int AcknoweldgeBy, int intEncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intRegId", RegId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intBedId", BedId);
                HshIn.Add("@intWardId", WardId);
                HshIn.Add("@intReasonId", ReasonId);
                HshIn.Add("@Description", Description);

                HshIn.Add("@KOTStatus", KOTStatus);
                HshIn.Add("@OPIP", OPIP);
                HshIn.Add("@HostName", HostName);
                HshIn.Add("@IPAddress", IPAddress);
                HshIn.Add("@Active", Active);
                HshIn.Add("@intAcknoweldgeBy", AcknoweldgeBy);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput.Add("@KOTOrderNo", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveKOTOrder", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        #endregion
        #region OT

        public DataSet PopulateOTName(int HospitalLocationId, int LoginFacilityId)
        {
            return PopulateOTName(HospitalLocationId, LoginFacilityId, 0);
        }

        public DataSet PopulateOTName(int HospitalLocationId, int LoginFacilityId, int employeeId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intFacilityID", LoginFacilityId);

                if (employeeId > 0)
                {
                    HshIn.Add("@intemployeeId", employeeId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTs", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTimeZone(int LoginFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intFacilityID", LoginFacilityId);

                string strtimezone = "Select TimeZoneOffSetMinutes  from FacilityMaster where FacilityID=@intFacilityID";
                return objDl.FillDataSet(CommandType.Text, strtimezone, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet PopulatColorList(int HospitalLocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strsql = "select Status,StatusColor, StatusId, Code from GetStatus(" + HospitalLocationId + ",'OT') order by SequenceNo asc";
                return objDl.FillDataSet(CommandType.Text, strsql);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOTScheduler(int HospId, int FacilityId, string xmlTheaterId, DateTime Fordate, string CurrentView, int statuscolor)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                if (FacilityId == 0)
                {
                    HshIn.Add("@intFacilityId", "0");

                }
                else
                {
                    HshIn.Add("@intFacilityId", FacilityId);
                }
                HshIn.Add("@xmlOTTheaterIds", xmlTheaterId);
                HshIn.Add("@chrForDate", Fordate.ToString("yyyy/MM/dd"));
                HshIn.Add("@chrDateOption", CurrentView == null ? "D" : CurrentView);
                HshIn.Add("@intStatusId", statuscolor);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTSchedule", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet BindAppointmentContaxtmanu(int EncounterId, int RegId, string strAppId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intEncounterID", EncounterId);
                HshIn.Add("@intRegistrationID", RegId);
                HshIn.Add("@BookingID", strAppId);

                string strOTStatus = "SELECT StatusId FROM OTBooking WHERE EncounterID=@intEncounterID AND RegistrationID=@intRegistrationID AND OTBookingID=@BookingID";
                return objDl.FillDataSet(CommandType.Text, strOTStatus, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public void ChangeOTStatus(int HospId, int strAppId, string sStatusType, int FacilityId, int LastChangedBy)
        {
            ChangeOTStatus(HospId, strAppId, sStatusType, FacilityId, LastChangedBy, null, null);
        }
        public void ChangeOTStatus(int HospId, int strAppId, string sStatusType, int FacilityId, int LastChangedBy, string chvRemark, string UnplannedSurgeryRemarks)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intOTBookingID", strAppId);
                HshIn.Add("@chvStatusType", sStatusType);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intLastChangedBy", LastChangedBy);
                HshIn.Add("@chvCancelRemarks", chvRemark);
                HshIn.Add("@UnplannedSurgeryRemarks", UnplannedSurgeryRemarks);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeOTStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable GetRecurringAppointment(int AppointmentID, int HospitalId, int FacilityId, DateTime dtStart)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@intParentAppointmentID", AppointmentID);
                HshIn.Add("@inyHospitalLocationID", HospitalId);
                HshIn.Add("@intFacilityID", FacilityId);
                hshOutput.Add("@intStatusID", SqlDbType.Int);

                HshIn.Add("@chrAppointmentDate", dtStart.ToString("MM/dd/yyyy"));

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRGetRecurringAppointment", HshIn, hshOutput);

                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable ChangeRecurringAppointmentStatus(int AppointmentId, int EncodedBy, int StatusId, DateTime dtStart)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@intAppointmentId", AppointmentId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStatusId", StatusId);
                hshOutput.Add("@intRecAppointmentId", SqlDbType.Int);
                HshIn.Add("@dtAppointmentDate", dtStart.Date);


                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspChangeRecurringAppointmentStatus", HshIn, hshOutput);

                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void ChangeAppointmentStatus(int AppointmentId, int EncodedBy, int StatusId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@intAppointmentId", AppointmentId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStatusId", StatusId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeAppointmentStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void EMRMUDLogCPTInEAndMAndClinicalSummary(int HospitalId, int RegId, int EncId, int DoctorId, int EncodedBy, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospitalId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intFacilityID", FacilityId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogCPTInE&MAndClinicalSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void CancelThisApp(int AppId, string sCancelReason, int LastChangedBy, int ReasonId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intOTBookingId", AppId);
            HshIn.Add("@chvCancelRemarks", sCancelReason);
            HshIn.Add("@intLastChangedBy", LastChangedBy);
            HshIn.Add("@intReasonId", ReasonId);
            objDl.ExecuteNonQuery(CommandType.Text, "Set DateFormat ymd Update OTBooking Set CancelReason= @chvCancelRemarks, Active=0, LastChangedBy = @intLastChangedBy, LastChangedDate = GETUTCDATE(), OTCancelReasonId =@intReasonId where OTBookingId = @intOTBookingId  Update BreakList SET Active=0 WHERE OtBookingId=@intOTBookingId", HshIn);

        }

        public DataSet GetCompanyCode(int EncId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string strCompany = " select CompanyCode from Encounter where id=" + EncId;
                return objDl.FillDataSet(CommandType.Text, strCompany);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetRecurrenceRule(int AppointId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string strsql = "Select isnull(RecurrenceRule,'') as RecurrenceRule from doctorappointment where appointmentid=" + AppointId;
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetFromTime(int AppointId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string strsql = "Select isnull(RecurrenceRule,'') as RecurrenceRule from doctorappointment where appointmentid=" + AppointId;
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        //******************** OT Booking *************************************************************************


        public DataSet GetOTEquipment(int HospitalId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospitalId);
                HshIn.Add("@intFacilityID", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTEquipment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable SaveOTBooking(int HospitalLocationId, int LoginFacilityId, int OTBookingId, int RegId, string RegistrationNo, int EncounterId, string FirstName, string MiddleName,
               string LastName, string DOB, int AgeYears, int AgeMonths, int AgeDays, int Gender, int TheaterId, string OTBookingDate, string FromTime, string ToTime, string Remarks,
             int Active, int UserId, string XMLServiceDetails, string XMLResourceDetails, string XMLEquipmentDetails)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", LoginFacilityId);
            HshIn.Add("intOTBookingId", OTBookingId);
            HshIn.Add("intRegistrationId", RegId);
            HshIn.Add("chvRegistrationNo", RegistrationNo);
            HshIn.Add("intEncounterId", EncounterId);
            HshIn.Add("chvFirstName", FirstName);
            HshIn.Add("chvMiddleName", MiddleName);
            HshIn.Add("chvLastName", LastName);
            HshIn.Add("dtDOB", Convert.ToDateTime(DOB));
            HshIn.Add("inyAgeYears", AgeYears);
            HshIn.Add("inyAgeMonths", AgeMonths);
            HshIn.Add("inyAgeDays", AgeDays);
            HshIn.Add("inyGender", Gender);
            HshIn.Add("intTheaterId", TheaterId);
            HshIn.Add("dtOTBookingDate", Convert.ToDateTime(OTBookingDate));
            HshIn.Add("dtFromTime", FromTime);
            HshIn.Add("dtToTime", ToTime);
            HshIn.Add("chvRemarks", Remarks);
            HshIn.Add("bitActive", Active);
            HshIn.Add("intEncodedBy", UserId);

            HshIn.Add("xmlServiceDetails", XMLServiceDetails);
            HshIn.Add("xmlResourceDetails", XMLResourceDetails);
            HshIn.Add("xmlEquipmentDetails", XMLEquipmentDetails);

            hshOutput.Add("chvOTBookingNo", SqlDbType.VarChar);
            hshOutput.Add("intId", SqlDbType.VarChar);
            hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTBooking", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOTBookingDetails(int HospId, int FacilityId, int OTBookingId, string sOTBookingNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intOTBookingId", OTBookingId);
                HshIn.Add("chvOTBookingNo", sOTBookingNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTBookingDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable CopyOTBooking(int HospitalLocationId, int LoginFacilityId, int OTBookingId, string OTBookingDate, string FromTime, int TheaterId, int UserId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", LoginFacilityId);
            HshIn.Add("intCopyOTBookingID", OTBookingId);
            HshIn.Add("dtOTBookingDate", Convert.ToDateTime(OTBookingDate));
            HshIn.Add("chrAppFromTime", FromTime);
            HshIn.Add("intTheaterId", TheaterId);
            HshIn.Add("intEncodedBy", UserId);
            hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspCopyOTTheaterBooking", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOtDetails(int HospId, int FacilityId, int TheaterId, DateTime Date)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intTheaterId", TheaterId);
                HshIn.Add("@dtDate", Date.ToString("yyyy-MM-dd"));
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetOTDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetOTPatientDetails(int BookingId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intOtbookingId", BookingId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetOTPatientResource", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveOTChecklistMaster(int Id, string Description, string Type, int Status, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@intId", Id);
                HshIn.Add("@chvDescription", Description);
                HshIn.Add("@chvValueType", Type);
                HshIn.Add("@bitStatus", Status);
                HshIn.Add("@intEncodedBy", UserId);

                hshOutput.Add("@chvErrorMassage", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateOTCheckListMaster", HshIn, hshOutput);
                return hshOutput["@chvErrorMassage"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetOTChecklistMaster()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string strsql = "Select Id,Description,ValueType,Status AS Active, CASE WHEN Status=1 THEN 'Active' ELSE 'InActive' END AS Status from OTChecklistMaster";
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SaveOTChecklistTransaction(int HosptalId, int FacilityId, int BookingNo, int EncId, int? ServiceId, int BedId, int CheckedBy, int UserId, string XmlService)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HosptalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intBookingNo", BookingNo);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intServiceId", ServiceId);
                HshIn.Add("@intBedId", BedId);
                HshIn.Add("@intCheckedBy", CheckedBy);
                HshIn.Add("@xmlService", XmlService);
                HshIn.Add("@intEncodedBy", UserId);

                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOTChecklistTransaction", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetChecklistOTTransaction(int HospitalId, int FacilityId, int EncounterId, int BookingId, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intBookingId", BookingId);
                HshIn.Add("@intEncodedBy", UserId);

                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetOTChecklistTransaction", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveOTBookingBillClearance(int HospId, string xmlOTBooking, int EncodedBy, string ClearanceType, string Remarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@xmlOTBooking", xmlOTBooking);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chvClearanceType", ClearanceType);
                HshIn.Add("@chvRemarks", Remarks);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTBookingBillClearance", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateOTBookingWithOrderId(int HospId, int FacilityId, int OTBookingId, int OrderId, int EncodedBy,
                        string AmtChangeReason, int SurgeryChangedStatusId, string SurgeryChangedRemarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("inyFacilityId", FacilityId);
            HshIn.Add("intOTBookingId", OTBookingId);
            HshIn.Add("intOrderId", OrderId);
            HshIn.Add("intEncodedBy", EncodedBy);
            HshIn.Add("chvAmtChangeReason", AmtChangeReason);
            HshIn.Add("intSurgeryChangedStatusId", SurgeryChangedStatusId);
            HshIn.Add("chvSurgeryChangedRemarks", SurgeryChangedRemarks);

            hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

            hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateOTBooking", HshIn, hshOutput);

            return Convert.ToString(hshOutput["chvErrorStatus"]);
        }
        public string UpdateOTBookingWithOrderId(int HospId, int FacilityId, int OTBookingId, int OrderId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("inyFacilityId", FacilityId);
                HshIn.Add("intOTBookingId", OTBookingId);
                HshIn.Add("intOrderId", OrderId);
                HshIn.Add("intEncodedBy", EncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateOTBooking", HshIn, hshOutput);

                return Convert.ToString(hshOutput["chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        /// <summary>
        /// SProc - USPShowOTOrderInGrid
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="dtFromDate"></param>
        /// <param name="dtToDate"></param>
        /// <param name="FilterType"></param>
        /// <param name="OrderId"></param>
        /// <returns>DataSet</returns>
        public DataSet GetOTOrderList(int HospId, int FacilityId, string dtFromDate, string dtToDate, int FilterType, int OrderId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("insFacilityId", FacilityId);
                HshIn.Add("inyFilterType", FilterType);
                HshIn.Add("dtFromDate", dtFromDate);
                HshIn.Add("dtToDate", dtToDate);
                HshIn.Add("intOrderId", OrderId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPShowOTOrderInGrid", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string TagPatient(int OTBookingId, int TagRegId, int TagEncounterId, string TagEncounterNo, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                HshIn.Add("intOTBookingId", OTBookingId);
                HshIn.Add("intTagRegistrationId", TagRegId);
                HshIn.Add("intTagEncounterId", TagEncounterId);
                HshIn.Add("chvTagEncounterNo", TagEncounterNo);
                HshIn.Add("intEncodedBy", EncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspOTTagPatient", HshIn, hshOutput);

                return Convert.ToString(hshOutput["chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelOTOrder(int HospId, int FacilityId, int EncounterId, int OrderId, string Remarks, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("intOrderId", OrderId);
                HshIn.Add("chvRemarks", Remarks);
                HshIn.Add("intEncodedBy", EncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelOTOrder", HshIn, hshOutput);

                return Convert.ToString(hshOutput["chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOTPacServiceConfirm(int HospId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("inyFacilityId", FacilityId);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.Text, "select Value from HospitalSetup where Flag ='OtPACServiceId'", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveOTPACClearanceService(int ServId, int HospId, int FacilityId, int RegId, int OrderId, int EncId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();
                HshIn.Add("serviceId", ServId);
                HshIn.Add("HospitalLocationId", HospId);
                HshIn.Add("iFacilityId", FacilityId);
                HshIn.Add("iRegistrationId", RegId);
                HshIn.Add("iEncounterID", EncId);
                HshIn.Add("iOrderId", OrderId);
                HshIn.Add("iEncodedBy", EncodedBy);
                hshOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspOtPacClearanceServiceSave", HshIn, hshOutput);
                return Convert.ToString(hshOutput["chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion
        #region ICM
        public DataSet GetICMTemplateName(int iEncounterID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intEncounterID", iEncounterID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetICMTemplateNames", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveICMSummaryFormat(int iID, int iHospitalLocationID, string sSummaryName, string sSummaryNote, int iEncodedBy, string sEncodedDate)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intID", iID);
                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@chvSummaryName", sSummaryName);
                HshIn.Add("@chvSummaryNote", sSummaryNote);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@chrEncodedDate", sEncodedDate);
                HshIn.Add("@intLastUpdatedBy", iEncodedBy);
                HshIn.Add("@chrLastUpdatedDate", sEncodedDate);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMSummaryFormat", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveUpdateICMPatientSummary(int iID, int iHospitalLocationID, string sRegistrationID, string sEncounterID, int iFormatID, string sPatietSummary, int iSignDoctorID, int iEncodedBy, string sEncodedDate)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intID", iID);
                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intRegistrationID", sRegistrationID);
                HshIn.Add("@intEncounterID", sEncounterID);
                HshIn.Add("@intFormatID", iFormatID);
                HshIn.Add("@chvPatientSummary", sPatietSummary);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@chrEncodedDate", sEncodedDate);
                HshIn.Add("@intLastUpdatedBy", iEncodedBy);
                HshIn.Add("@chrLastUpdatedDate", sEncodedDate);
                HshIn.Add("@intSignDoctorID", iSignDoctorID);
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMPatientSummary", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSummaryFormatName(int iHospitalLocationID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                string sFormatName = "SELECT ID,FormatName FROM SummaryFormat WHERE HospitalLocationID=@intHospitalLocationID";
                return objDl.FillDataSet(CommandType.Text, sFormatName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSummaryFormatDetails(int iHospitalLocationID, int iID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intID", iID);
                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                string sFormatDetails = "SELECT ID,SummaryNote FROM SummaryFormat WHERE HospitalLocationID=@intHospitalLocationID AND ID=@intID";
                return objDl.FillDataSet(CommandType.Text, sFormatDetails, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientSummaryDetails(int iHospitalLocationID, string sRegistrationID, string sEncounterID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intRegistrationID", sRegistrationID);
                HshIn.Add("@intEncounterID", sEncounterID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientDischargeSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetICMTemplateStyle(int iHospitalLocationID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                string sqlTemplateStyle = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
                + "TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
                + "SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
                + "FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber "
                + "FROM EMRTemplateStatic where HospitalLocationId = @inyHospitalLocationID";
                return objDl.FillDataSet(CommandType.Text, sqlTemplateStyle, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSignDoctors(int iHospitalLocationID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                string sqlSignDoctor = "SELECT ID, ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') +ISNULL(LastName,'')  AS DoctorName FROM  Employee WHERE HospitalLocationId=@inyHospitalLocationID AND Designation IS NOT NULL";
                return objDl.FillDataSet(CommandType.Text, sqlSignDoctor, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMSignDoctorName(int iHospitalLocationID, int iSignDoctorID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intSignDoctorID", iSignDoctorID);
                string sqlDoctorName = "SELECT ID, ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') +ISNULL(LastName,'')  AS SignDoctorName, Designation FROM  Employee WHERE HospitalLocationId=@inyHospitalLocationID AND ID=@intSignDoctorID";
                return objDl.FillDataSet(CommandType.Text, sqlDoctorName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void UpdateICMFinalize(int iHospitalLocationID, int iSummaryID, bool bFinalize, int iDoctorSignID, int lastChangeBy, string lastChangeDate)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intSummaryID", iSummaryID);
                HshIn.Add("@bitFinalize", bFinalize);
                HshIn.Add("@lastChangeBy", lastChangeBy);
                HshIn.Add("@lastChangeDate", lastChangeDate);
                string sqlSummaryFinalize = "UPDATE EMRPatientSummaryDetails SET FINALIZE=@bitFinalize,SignDoctorID=@intDoctorSignID,LastChangeBy=@lastChangeBy,lastChangeDate=@lastChangeDate WHERE HospitalLocationID=@inyHospitalLocationID AND SummaryID=@intSummaryID";
                objDl.ExecuteNonQuery(CommandType.Text, sqlSummaryFinalize, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMNoCurrentMedication(int iHospitalLocationID, int iMedicationID)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intMedicationID", iMedicationID);
                string sqlDoctorName = " SELECT ISNULL(NoCurrentMedication,0) AS NoCurrentMedication FROM  Encounter WHERE HospitalLocationId=@inyHospitalLocationID AND ID=@intMedicationID";
                return objDl.FillDataSet(CommandType.Text, sqlDoctorName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientLabResult(int iHospitalLocationID, int iRegistrationID, string sFromDate, string sToDate)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intRegistrationID", iRegistrationID);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetInvestigationsResult", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICMPatientInvestigationResult(int iHospitalLocationID, int iFacilityId, int iEncounterID, string sFromDate, string sToDate)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@inEncId", iEncounterID);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetInvestigationsResult", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getHospitalPerformance(DateTime FromDate, DateTime ToDate, int UserId, int HospId, int FacilityId)
        {
            HshIn = new Hashtable();


            string fDate = FromDate.ToString("yyyy/MM/dd");
            string tDate = ToDate.ToString("yyyy/MM/dd");

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@FDate", fDate);
                HshIn.Add("@TDate", tDate);
                HshIn.Add("@loginur", UserId);
                HshIn.Add("@Locid", HospId);
                HshIn.Add("@FacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "DOC_PERFORMANCE", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getHospitalDoctorPerformance(DateTime FromDate, DateTime ToDate, int DeptCode, int UserId, int HospId, int FacilityId)
        {
            HshIn = new Hashtable();


            string fDate = FromDate.ToString("yyyy/MM/dd");
            string tDate = ToDate.ToString("yyyy/MM/dd");

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@FDate", fDate);
                HshIn.Add("@TDate", tDate);
                HshIn.Add("@Dept_Code", DeptCode);
                HshIn.Add("@LoginUr", UserId);
                HshIn.Add("@Locid", HospId);
                HshIn.Add("@FacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "DOC_PERFORMANCE_DETAIL", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getHospitalPerformanceHead(DateTime FromDate, DateTime ToDate, int DeptCode, int Headcode, int Doccode, int UserId, int HospId, int FacilityId)
        {
            HshIn = new Hashtable();


            string fDate = FromDate.ToString("yyyy/MM/dd");
            string tDate = ToDate.ToString("yyyy/MM/dd");

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@FDate", fDate);
                HshIn.Add("@TDate", tDate);
                HshIn.Add("@Dept_Code", DeptCode);
                HshIn.Add("@Head_Code", Headcode);
                HshIn.Add("@Doc_Code", Doccode);
                HshIn.Add("@loginur", UserId);
                HshIn.Add("@Locid", HospId);
                HshIn.Add("@FacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "DOC_PERFORMANCE_DETAIL_Heads", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public Hashtable FindICMAdminUser(int UserId, int HospId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOutput = new Hashtable();
                HshIn.Add("@intUserID", UserId);
                HshIn.Add("@inyHospitalLocationID", HospId);
                hshOutput.Add("@bitIsAdmin", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "FindICMAdminUser", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetMISCriteria(int HospId, string Type, int iDpartId)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@chvCriteria", Type);
                HshIn.Add("@intDepartmentId", iDpartId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMISCriteria", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetMISContributionServiceDetails(int iHospId, int iFacilityId, string sFromDate, string sToDate,
            int iSubDeptId, string sServiceId)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@FDate", sFromDate);
                HshIn.Add("@TDate", sToDate);
                HshIn.Add("@SubDeptId", iSubDeptId);
                HshIn.Add("@service_code", sServiceId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMISEBTReportService", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetMISContributionDepartmentDetails(int iHospId, int iFacilityId, string sFromDate, string sToDate,
            string sDepartmentId)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@FDate", sFromDate);
                HshIn.Add("@TDate", sToDate);
                HshIn.Add("@doc_department_code", sDepartmentId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMISEBTReportDepartment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetMISContributionDoctorDetails(int iHospId, int iFacilityId, string sFromDate, string sToDate,
            string sDoctorId)
        {
            HshIn = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@FDate", sFromDate);
                HshIn.Add("@TDate", sToDate);
                HshIn.Add("@admitting_doctor_code", sDoctorId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMISEBTReportDoctor", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion
        #region MRD
        public string MRDSaveRequestFile(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, int DoctorId,
                            int DepartmentId, DateTime FileRequiredDate, string Remarks, string RequestFor, int EncodedBy, string sConString, int RequestById, string DocumentPath, string DocumentName)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intDepartmentId", DepartmentId);
            HshIn.Add("@dtFileRequiredDate", FileRequiredDate);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@chvRequestFor", RequestFor);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intRequestBy", RequestById);
            HshIn.Add("@chvDocumentPath", DocumentPath);
            HshIn.Add("@chvDocumentName", DocumentName);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMRDRequestFile", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string MRDSaveFileRackNo(int HospitalLocationId, int FacilityId, int EncounterId, int RegistrationId
           , string RackNumber, string ShelfNo, string Remarks, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@chrRackNumber", RackNumber);
            HshIn.Add("@chrShelfNo", ShelfNo);
            HshIn.Add("@chrRemarks", Remarks);
            HshIn.Add("@intEncodedBy", EncodedBy);

            hshOutput.Add("@chrErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspMRDSaveFileRackNo", HshIn, hshOutput);

                return hshOutput["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetMRDRequestFile(int StatusId, int RegistrationId, int HospitalLocationId, int FacilityId,
                                       string OPIP, DateTime FromDate, DateTime ToDate, string sConString,
                                       string SearchOnForUHID, string SearchOnEnc, string SearchOnPat)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
            string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvOPIP", OPIP);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@chvSearchOnForUHID", SearchOnForUHID);
            HshIn.Add("@chvSearchOnEnc", SearchOnEnc);
            HshIn.Add("@chvSearchOnPat", SearchOnPat);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDRequestFile", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getManualMRDFileIssueRetun(string IssueReturn, int RegistrationId, int HospitalLocationId, int FacilityId,
                                        string OPIP, DateTime FromDate, DateTime ToDate, string sConString)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
            string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

            HshIn.Add("@chvIssueReturn", IssueReturn);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvOPIP", OPIP);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetManualMRDFileIssueRetun", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetMRDEmployeeDepartment(int iHospitalLocationID, Int32 iEmployeeID, int iFacilityID, string sConString)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intEmployeeID", iEmployeeID);
            HshIn.Add("@intFacilityID", iFacilityID);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "GetMRDEmployeeDepartment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetICPDisease(string sConString)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "select Id,DiseaseCode,DiseaseName,Status,Case When Status=1 Then 'Active' else 'In-Actice' End Active from MRDIcpDisease");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveMrdProcedure(int Id, string Code, string Name, bool status, string sConString)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intId", Id);
            HshIn.Add("@chvCode", Code);
            HshIn.Add("@chvName", Name);
            HshIn.Add("@btStatus", status);

            hshOutput.Add("@chvErrormassege", SqlDbType.VarChar);
            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspMRDIcpSaveUpdate", HshIn, hshOutput);
                return hshOutput["@chvErrormassege"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveICD(int Id, string Code, string Name, bool status, int GroupId, int SubGroupId, string sConString)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intId", Id);
            HshIn.Add("@chvCode", Code);
            HshIn.Add("@chvName", Name);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@intSubGroupId", SubGroupId);
            HshIn.Add("@btStatus", status);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspMRDICDSaveUpdate", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string UpdateMRDFileStatus(int RequestId, int StatusId, int HospitalLocationId, int EncodedBy, string sConString, string FileIssueorReturnRemarks)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@intRequestId", RequestId);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@FileIssueorReturnRemarks", FileIssueorReturnRemarks);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateMRDFileStatus", HshIn, hshOutput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            return hshOutput["@chvErrorStatus"].ToString();
        }

        public string UpdateManualMRDFileIssueRetun(int IssueId, int HospitalLocationId, int EncodedBy, string sConString)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@intIssueId", IssueId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateManualMRDFileIssueRetun", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            return hshOutput["@chvErrorStatus"].ToString();
        }



        #endregion
        #region DoctorAccounting
        public DataSet GetDaDoctorServiceCharges(int iHospitalLocationID, int iFacilityID, bool bDoctorRelated)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@bitDoctorRelated", bDoctorRelated);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorServiceSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void SaveDaDoctorServiceCharge(int iHospitalLocationID, int iFacilityID, int iDepartmentTypeID, bool bDoctorRelated, bool bDoctorShareDoctorWise, bool bDoctorShareServiceWise)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intDepartmentTypeID", iDepartmentTypeID);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@bitDoctorRelated", bDoctorRelated);
            HshIn.Add("@bitDoctorShareServiceWise", bDoctorShareServiceWise);
            HshIn.Add("@bitDoctorShareDoctorWise", bDoctorShareDoctorWise);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "DaSaveDoctorServiceSetUp", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetDaDoctorSharePercentage(int iHospitalLocationID, int iFacilityID, int iType, int iDepartmentTypeID, int iCompanyId, int iSubDeptId, int iServiceId, string sFromDate, string sToDate)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intType", iType);
            HshIn.Add("@intDepartmentTypeID", iDepartmentTypeID);
            HshIn.Add("@iCompanyId", iCompanyId);
            HshIn.Add("@intSubDepartmentId", iSubDeptId);
            HshIn.Add("@intServiceId", iServiceId);


            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorSharePercentage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaDoctorShareFlag(int iHospitalLocationID, int iFacilityID, int iDepartmentTypeID)
        {


            HshIn = new Hashtable();
            HshIn.Add("@intDepartmentTypeID", iDepartmentTypeID);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorShareFlag", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DaSaveDoctorWiseSharePercentage(int iHospitalLocationID, int iFacilityID, int iTypeID, int DepartmentTypeID, String xmlDoctorWise,
            int CompanyId, int iSubDeptId, int sServiceId, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intType", iTypeID);
            HshIn.Add("@xmlDoctorWise", xmlDoctorWise);
            HshIn.Add("@CompanyId", CompanyId);
            HshIn.Add("@intDepartmentTypeID", DepartmentTypeID);
            HshIn.Add("@intSubDepartmentId", iSubDeptId);
            HshIn.Add("@intServiceId", sServiceId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDoctorWiseSharePercentage", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaProvisionalStatement(int iHospitalLocationID, int iFacilityID, string sFromDate, string sToDate, string sSelectDoctor)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@FromDate", sFromDate);
            HshIn.Add("@ToDate", sToDate);
            HshIn.Add("@chvSelectDoctor", sSelectDoctor);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaDoctorStatementinGrid", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void DaSaveDoctorProvisionalStatement(int iHospitalLocationID, int iFacilityID, int iEncodedBy, string xmlDoctorServices,
            string xmlDoctorRelease, string sOPStatus, int iPreDaId, char StatementStatus)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@DoctorServices", xmlDoctorServices);
            HshIn.Add("@DoctorRelease", xmlDoctorRelease);
            HshIn.Add("@OPStatus", sOPStatus);
            HshIn.Add("@PreDaId", iPreDaId);
            HshIn.Add("@chvStatementStatus", StatementStatus);
            HshIn.Add("@EncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "DaProcessDoctorPayment", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public Hashtable GetDaStatementMonthForCancel(int iHospitalLocationID, int iFacilityID, string sStatementMonth, string sStatementYear)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvStatementYear", sStatementYear);
            HshIn.Add("@chvStatementMonth", sStatementMonth);
            hshOutput.Add("@chvOutput", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaGetStatementMonthForCancel", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetDaFinalStatements(int iHospitalLocationID, int iFacilityID)
        {
            DataTable dt = new DataTable();

            HshIn = new Hashtable();
            string sStatement = "SELECT DISTINCT dsm.DrFinalStatementId,dsm.StatementMonth AS Month,"
                              + " CASE dsm.StatementMonth WHEN 1 THEN 'January' WHEN 2 THEN 'February' WHEN 3 THEN 'March' WHEN 4 THEN 'April'  WHEN 5 THEN 'May' "
                              + " WHEN 6 THEN 'June' WHEN 7 THEN 'July' WHEN 8 THEN 'August' WHEN 9 THEN 'September' WHEN 10 THEN 'October' WHEN 11 THEN 'November' "
                              + " WHEN 12 THEN 'December' ELSE '' END AS 'StatementMonth',"
                              + " StatementYear, CASE StatementStatus WHEN 'O' THEN 'Open' WHEN 'C' THEN 'Closed' END AS StatementStatus FROM  DaFinalDoctorStatementMain dsm "
                              + " WHERE dsm.HospitalLocationId=@inyHospitalLocationID AND dsm.FacilityId=@intFacilityID ";
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, sStatement, HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable CancelDaFinalStatement(int iHospitalLocationID, int iFacilityID, int iFinalStatement)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@intFinalStatementId", iFinalStatement);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaCancelDoctorFinalStatement", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveDaEarningDeductionHead(int iHeadID, int iHospitalLocationID, int iFacilityID, string sHeadName,
            char cHeadType, char cBeforeAfterTDS, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intHeadId", iHeadID);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvHeadName", sHeadName);
            HshIn.Add("@chvHeadType", cHeadType);
            HshIn.Add("@chvBeforeAfterTDS", cBeforeAfterTDS);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDaEarningDeductionHead", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaEarningDeductionHead(int iHospitalLocationID, int iFacilityID, int iHeadID)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@intHeadId", iHeadID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetEarningDeductionHead", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaHeadDetails(int iHospitalLocationID, int iFacilityID, char cType, int iHeadId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvType", cType);
            HshIn.Add("@intHeadID", iHeadId);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorHeadDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDaHeadNames(int iHospitalLocationID, int iFacilityID, char cType)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvType", cType);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetHeadName", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable DaSaveDoctorHeadDetails(int iHospitalLocationID, int iFacilityID, int iHeadId, String xmlDoctorHead, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intHeadId", iHeadId);
            HshIn.Add("@xmlDoctorHead", xmlDoctorHead);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDoctorHeadDetails", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaTeamDoctorLists(int iHospitalLocationID, Int32 iTeamId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intTeamId", iTeamId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetTeamDoctorLists", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet DaGetDoctorTeamDetails(int iHospitalLocationID, int iFacilityId, int iTeamId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intTeamId", iTeamId);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorTeamDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet DaGetDepartmentWiseDoctorLists(int iHospitalLocationID, int iDepartmentId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intDepartmentId", iDepartmentId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDepartmentWiseDoctorLists", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable DaSaveTeamDoctorDetails(int iHospitalLocationID, int iFacilityID, int iTeamId, String xmlTeamDoctor, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intTeamId", iTeamId);
            HshIn.Add("@xmlTeamDoctor", xmlTeamDoctor);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveTeamDoctorDetails", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet DaGetInvoiceDetails(int iHospitalLocationId, int iFacilityId, string sInvoiceNo)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intFacilityID", iFacilityId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@chvInvoiceNo", sInvoiceNo);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDaGetInvoiceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DaSaveDoctorAmountAdjustment(int iAmtAdjustId, int iHospitalLocationID, int iFacilityID,
            int iInvoiceId, int iDoctorId, decimal dcAmount, string sReceiptDate, string sRemarks, int iAdjustmentMonth, int iAdjustmentYear, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intAmtAdjustId", iAmtAdjustId);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@intInvoiceId", iInvoiceId);
            HshIn.Add("@intDoctorId", iDoctorId);

            HshIn.Add("@chvAmount", dcAmount);
            HshIn.Add("@chrReceiptDate", sReceiptDate);

            HshIn.Add("@chvRemarks", sRemarks);

            HshIn.Add("@intAdjustmentMonth", iAdjustmentMonth);
            HshIn.Add("@intAdjustmentYear", iAdjustmentYear);

            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveAmountAdjustment", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet DaGetDoctorAmountAdjustment(int iHospitalLocationId, int iFacilityId, int iAmtAdjustId, int iDoctorId,
            int itAdjustmentMonth, int iAdjustmentYear)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intFacilityID", iFacilityId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intAmtAdjustId", iAmtAdjustId);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intAdjustmentMonth", itAdjustmentMonth);
            HshIn.Add("@intAdjustmentYear", iAdjustmentYear);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDaGetDoctorAmountAdjustmentDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet DaGetShareDate(int iHospitalLocationId, int iFacilityId, int iDoctorId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intFacilityID", iFacilityId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intDoctor", iDoctorId);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetShareDateRange", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DaSaveDoctorDateRange(int Id, int iHospitalLocationId, int iFacilityId, int iDoctorId, string sFromDate, string sToDate,
            bool bDefaultRange, bool bActive, int iEncodedBy)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intID", Id);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intFacilityID", iFacilityId);

            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@bitDefaultRange", bDefaultRange);
            HshIn.Add("@bitActive", bActive);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveDoctorDateRange", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDaDoctorShareValue(int iHospitalLocationID, int iFacilityID, int iType, int iDepartmentTypeID,
            int iDoctorId, int iSubDeptId, int iServiceId, string sFromDate, string sToDate)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intType", iType);
            HshIn.Add("@intDepartmentTypeID", iDepartmentTypeID);

            HshIn.Add("@intSubDepartmentId", iSubDeptId);
            HshIn.Add("@intServiceId", iServiceId);


            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@intDoctorId", iDoctorId);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorShareValue", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DaSaveDoctorSetUp(string xDoctorSetup)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@xmlDoctorSetup", xDoctorSetup);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveDoctorSetup", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        #endregion
        public Hashtable GetIsunPlannedReturnToOt(int RegId, int FacilityId, DateTime OtBookingDate)
        {
            DAL.DAL objDL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            Hashtable hsOut = new Hashtable();
            hsIn.Add("@intRegId", RegId);
            hsIn.Add("@FacilityId", FacilityId);
            hsIn.Add("@dtOTBookingDate", OtBookingDate);
            hsOut.Add("@bitIsUnPlannedReturnToOT", SqlDbType.Bit);
            hsOut.Add("@chvLastCheckoutTime", SqlDbType.VarChar);
            hsOut.Add("@intTimeDiffInMin", SqlDbType.Int);


            try
            {

                hsOut = objDL.getOutputParametersValues(CommandType.StoredProcedure, "uspOTIsUnPlannedReturn", hsIn, hsOut);

                return hsOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int OTUncheckinThisApp(int AppId, string sCancelReason, int LastChangedBy)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();

            hsIn.Add("@intOTBookingId", AppId);
            hsIn.Add("@chvCancelRemarks", sCancelReason);
            hsIn.Add("@intLastChangedBy", LastChangedBy);
            try
            {

                int intResult = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspChangeOTStatus", hsIn);
                return intResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEmployeeId(int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string strQuery = "select U.empid from Users U  INNER JOIN EmployeeWiseOTTagging ET on U.Empid = ET.EmployeeId";
            try
            {


                return objDl.FillDataSet(CommandType.Text, strQuery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //public Hashtable GetIsunPlannedReturnToOt(int RegId, int FacilityId, DateTime OtBookingDate)
        //{
        //    DAL.DAL objDL = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable hsIn = new Hashtable();
        //    Hashtable hsOut = new Hashtable();
        //    hsIn.Add("@intRegId", RegId);
        //    hsIn.Add("@FacilityId", FacilityId);
        //    hsIn.Add("@dtOTBookingDate", OtBookingDate);
        //    hsOut.Add("@bitIsUnPlannedReturnToOT", SqlDbType.Bit);
        //    hsOut.Add("@chvLastCheckoutTime", SqlDbType.VarChar);
        //    hsOut.Add("@intTimeDiffInMin", SqlDbType.Int);

        //    hsOut = objDL.getOutputParametersValues(CommandType.StoredProcedure, "uspOTIsUnPlannedReturn", hsIn, hsOut);

        //    return hsOut;
        //}

        public DataSet getOTRequestDetails(int HospId, int FacilityId, int OTRequestId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intOTRequestId", OTRequestId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTRequestDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string MRDSaveRequestFileFromWardManagement(int HospitalLocationId, int FacilityId, int RegistrationId, string EncounterNo, int DoctorId,
                            int DepartmentId, string Remarks, int EncodedBy, string sConString, int RequestById)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterNo", EncounterNo);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intDepartmentId", DepartmentId);
            //HshIn.Add("@dtFileRequiredDate", FileRequiredDate);
            HshIn.Add("@chvRemarks", Remarks);
            //HshIn.Add("@chvRequestFor", RequestFor);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intRequestBy", RequestById);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMRDRequestFileFromWardManagement", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetMRDRequestFileFromWard(int HospitalLocationId, int FacilityId,
                                      DateTime FromDate, DateTime ToDate, string sConString,
                                     string SearchOnForUHID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
            string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

            //HshIn.Add("@intStatusId", StatusId);
            //HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            //HshIn.Add("@chvOPIP", OPIP);GetMRDRequestFileFromWard
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@chvSearchOnForUHID", SearchOnForUHID);
            //HshIn.Add("@chvSearchOnEnc", SearchOnEnc);
            //HshIn.Add("@chvSearchOnPat", SearchOnPat);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDRequestFileFromWard", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetMRDRequestFileFromWardReports(int HospitalLocationId, int FacilityId,
                                             DateTime FromDate, DateTime ToDate, string sConString,
                                            string SearchOnForUHID)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
            string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@chvSearchOnForUHID", SearchOnForUHID);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDRequestFileFromWardReports", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetMRDReturnFile(int StatusId, int RegistrationId, int HospitalLocationId, int FacilityId,
                               string OPIP, DateTime FromDate, DateTime ToDate, string sConString,
                               string SearchOnForUHID, string SearchOnEnc, string SearchOnPat)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
            string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvOPIP", OPIP);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@chvSearchOnForUHID", SearchOnForUHID);
            HshIn.Add("@chvSearchOnEnc", SearchOnEnc);
            HshIn.Add("@chvSearchOnPat", SearchOnPat);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDRequestFile", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSurgeryGradeStatus(int ServiceId, int EncounterId, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("ServiceId", ServiceId);
            HshIn.Add("EncounterId", EncounterId);
            HshIn.Add("intFacilityId", FacilityId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspgetSurgeryGradeStatus", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string MRDSaveMRDNonReturnable(int RegistrationId, int EncounterNo, int FacilityId,
            string NonReturnableDetails, string Reason, bool Active, string DocumentPath, string DocumentName,
             int EncodedBy, int Employeeid, string outsidePersonName, string mobileno, string typeofdocument, DateTime RequestTime, int MRDNonReturnableid)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterNo);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvNonReturnableDetails", NonReturnableDetails);
            HshIn.Add("@chvReason", Reason);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@chvDocumentPath", DocumentPath);
            HshIn.Add("@chvDocumentName", DocumentName);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intEmployeeid", Employeeid);
            HshIn.Add("@outsidePersonName", outsidePersonName);
            HshIn.Add("@Mobiileno", mobileno);
            HshIn.Add("@typeofdocument", typeofdocument);
            HshIn.Add("@RequestTime", RequestTime);
            HshIn.Add("@MRDNonReturnableid", MRDNonReturnableid);

            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMRDNonReturnable", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetMRDNonReturnable(int RegistrationId, int EncounterId, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@insFacilityId", FacilityId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetMRDNonReturnable", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetMRDManualFileIssue(int RegistrationId, int EncounterId, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@insFacilityId", FacilityId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetMRDManualFileIssue", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string MRDSaveMLCAndLegalDetails(int RegistrationId, int EncounterNo, int FacilityId, int RecordType, int EncodedBy, DateTime ReceivedDate,
            string PINo, string MLRNo, string Remark, string CaseBetween, string NameofJudge)
        {

            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterNo);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRecordType", RecordType);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@ReceivedDate", ReceivedDate);
            HshIn.Add("@PINo", PINo);
            HshIn.Add("@MLRNo", MLRNo);
            HshIn.Add("@Remark", Remark);
            HshIn.Add("@CaseBetween", CaseBetween);
            HshIn.Add("@NameofJudge", NameofJudge);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMLCAndLegalDetails", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string MRDSaveMRDFileDetails(string OPIP, int RegistrationId, int EncounterId, int FacilityId, int HospitalLocationId, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@OPIP", OPIP);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMRDFileDetails", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            return hshOutput["@chvErrorStatus"].ToString();

        }
        public DataSet PreAuthPatientDtl(string Query)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                return dl.FillDataSet(CommandType.Text, Query);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetServiceDetailsForPreAuth(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("RegId", iRegistrationId);
            HshIn.Add("EncounterId", iEncounterId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceDetailsForPreAuth", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetsetPAtDAta(int RegistrationId, int LoginSpecialisationID)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return dl.FillDataSet(CommandType.Text, "select RegistrationNo,MobileNo, EmiratesID,FirstName+Isnull(' '+Middlename,'')+isnull(' '+LastNAme,'') as Name,PayorId from Registration with(nolock) where ID='" + RegistrationId + "';select Name From SpecialisationMaster where ID=" + LoginSpecialisationID);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetDataPayer(int EncounterId)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return dl.FillDataSet(CommandType.Text, "select Distinct CompanyID,Name from company with(nolock) where active=1 and IsInsuranceCompany=1 ORder by Name asc; exec uspGetEMRdataforPreauth @encounterID=" + EncounterId);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SavePreAuth(int rdlOPIP, bool chkctg, string txtCTGremark, bool chkpregenent, DateTime rdLMP, string ddlx, bool chkwrokrelated
   , bool chksportrelated, bool chkprofessionalSport, bool chkRTArelated, string txtAlcohalintake, string txtreatmentdetail, string txtClinicalFindings
   , string txtTreatmentPlan, string txtremark, int RegistrationID, int EncounterId, int UserID, int FacilityId)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@visitType", rdlOPIP);
            HshIn.Add("@CTG", chkctg);
            HshIn.Add("@CTGremark", txtCTGremark);
            HshIn.Add("@pregnant", chkpregenent);
            HshIn.Add("@lmp", rdLMP);
            HshIn.Add("@modeofconception", ddlx);
            HshIn.Add("@Workrelated", chkwrokrelated);
            HshIn.Add("@sportrelated", chksportrelated);
            HshIn.Add("@professionalsport", chkprofessionalSport);
            HshIn.Add("@RTArelated", chkRTArelated);
            HshIn.Add("@AlcohalintakeQty", txtAlcohalintake);
            HshIn.Add("@treatmentdetail", txtreatmentdetail);
            HshIn.Add("@ClinicalFindings", txtClinicalFindings);
            HshIn.Add("@TreatmentPlan", txtTreatmentPlan);
            HshIn.Add("@Remark", txtremark);
            HshIn.Add("@regid", RegistrationID);
            HshIn.Add("@EncounterID", EncounterId);
            HshIn.Add("@createdby", UserID);
            HshIn.Add("@facilityid", FacilityId);
            hshOutput.Add("@errormsg", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPGlobalClaimForm_NEWEMR", HshIn, hshOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return hshOutput["@errormsg"].ToString();



        }

        public DataSet GetPACClearanceDetailsForOTRequest(int EncounterId, int DOctorId)
        {

            DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intDOctorId", DOctorId);

            try
            {

                return objdl.FillDataSet(CommandType.StoredProcedure, "uspGetPACClearanceDetailsForOTRequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public void SavePACClearanceDetailsForOTRequest(int EncounterId, int EncodedBy, int FitForSurgery, string PACRemarks)
        {

            DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intFitForSurgery", FitForSurgery);
            HshIn.Add("@chvPACRemarks", PACRemarks);
            try
            {

                objdl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSavePACClearanceDetailsForOTRequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public void CancelPACClearanceDetailsForOTRequest(int EncounterId)
        {

            DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intEncounterId", EncounterId);
            try
            {

                objdl.ExecuteNonQuery(CommandType.StoredProcedure, "uspCancelPACClearanceDetailsForOTRequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string DeleteEMROTRequest(int OTRequestID, int EncodedBy)
        {
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshIn.Add("@intOTRequestID", OTRequestID);
            hshIn.Add("@intEncodedby", EncodedBy);
            hshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
            try
            {

                hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteOTRequest", hshIn, hshOut);
                return hshOut["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEMROTRequest(int RegistrationNo)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hshIn = new Hashtable();
            hshIn.Add("@inRegistrationNo", RegistrationNo);
            try
            {

                return dl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientOTRequests", hshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
