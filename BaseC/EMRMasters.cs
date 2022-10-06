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
    public class EMRMasters
    {
        private string sConString = string.Empty;
        Hashtable HshIn;
        Hashtable HshOutPut;
        DAL.DAL objDl;
        StringBuilder strSQL;
        public EMRMasters(String Constring)
        {
            sConString = Constring;
        }
        public Hashtable SaveUpdateEmployeeClassification(int HospId, int FacilityID, int EmployeeId, int ClassificationId, int EncodedBy)
        {
            return SaveUpdateEmployeeClassification(HospId, FacilityID, EmployeeId, ClassificationId, EncodedBy, 0);
        }
        public Hashtable SaveUpdateEmployeeClassification(int HospId, int FacilityID, int EmployeeId, int ClassificationId, int EncodedBy, int SpecialisationId)
        {
            HshIn = new Hashtable();
            HshOutPut = new Hashtable();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityID);
            HshIn.Add("intEmployeeId", EmployeeId);
            HshIn.Add("intClassificationId", ClassificationId);
            HshIn.Add("intEncodedBy", EncodedBy);
            HshIn.Add("intSpecialisationId", SpecialisationId);
            HshOutPut.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOutPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateEmployeeClassification", HshIn, HshOutPut);
                return HshOutPut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientRecordInfoXML(int encID, int mode, int HospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("EncounterId", encID);
            HshIn.Add("Mode", mode);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientRecordInfoXML", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetSubDeptReportTagging()
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string query = "SELECT TaggingId,TaggingName,Issubdeptgroup FROM ReportTaggingMaster WITH (NOLOCK) ORDER BY TaggingName";
                return objDl.FillDataSet(CommandType.Text, query);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //added by sikandar for code optimiz
        public DataSet GETDateRangeStaticData()
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDateRangeData");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //string query = "SELECT TaggingId,TaggingName,Issubdeptgroup FROM ReportTaggingMaster ORDER BY TaggingName";

        }
        public Hashtable SaveUpdateReportTagging(int iSubDeptId, int iTaggingId, bool bIsSubDeptId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOutPut = new Hashtable();
                HshIn.Add("@intSubDeptId", iSubDeptId);
                HshIn.Add("@intTaggingId", iTaggingId);
                HshIn.Add("@bitIsSubDeptId", bIsSubDeptId);
                //HshIn.Add("@bitActive", bActive);
                HshOutPut.Add("@chvOutPut", SqlDbType.VarChar);
                HshOutPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateReportTagging", HshIn, HshOutPut);
                return HshOutPut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetHospitalDepartment(int iHospitalLocationId, string sServiceType)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("chvDepartmentType", sServiceType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalDepartments", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        //yogesh  29/09/2022
        public DataSet GetHospitalDepartmentByEntrySite(int iHospitalLocationId, string sServiceType,int EntrySite, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("chvDepartmentType", sServiceType);
                HshIn.Add("iEntrySiteId", EntrySite);
                HshIn.Add("intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalDepartments", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetUserDepartment(int iHospitalLocationId, int UserId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intUserId", UserId);
                string strsql = "SELECT DISTINCT dm.DepartmentID, dm.DepartmentName FROM DepartmentMain dm WITH (NOLOCK)  " +
                              "INNER JOIN secgroupdepartments sgd WITH (NOLOCK) ON dm.DepartmentID=sgd.DepartmentId " +
                              "INNER JOIN SecUserGroups sug WITH (NOLOCK) ON sgd.GroupId=sug.GroupId " +
                              "WHERE sug.UserId=@intUserId AND dm.Active = 1 AND dm.HospitalLocationId = @inyHospitalLocationID ORDER BY dm.DepartmentName ";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

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

        public DataSet GetService(int iHospitalLocationId, string sSubDepID, string sType, int FacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("chvSubDepartmentIds", sSubDepID);
                HshIn.Add("chrType", sType);
                HshIn.Add("intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDefaultHospitalCompany(int iHospitalLocationId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                return objDl.FillDataSet(CommandType.Text, "select Value from HospitalSetup WITH (NOLOCK) where Flag='DefaultHospitalCompany' And HospitalLocationId =@inyHospitalLocationId", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetPatientHistoryDetails(int iHospitalLocationId, int IFacilityId, int iDoctorId, int iRegId, string Daterange, string iFromdate, string iTodate, string Source)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", IFacilityId);
                HshIn.Add("@intProviderId", iDoctorId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@chvDateRange", Daterange);
                HshIn.Add("@chrFromDate", iFromdate);
                HshIn.Add("@chrToDate", iTodate);
                HshIn.Add("@Source", Source);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchPatientHistry", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetViewHistory(int RegistraionId, int EncounterId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegistraionId);
                HshIn.Add("@intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.Text, "select patientsummary from EMRPatientForms WITH (NOLOCK) where EncounterId = (CASE @intEncounterId WHEN 0 THEN EncounterId Else @intEncounterId END) AND RegistrationId=@intRegistrationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientPrescriptionHistory(int iHospitalLocationId, int IFacilityId, int iRegId, string EncounterId, int iDoctorId, string Daterange, string iFromdate, string iTodate)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", IFacilityId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@chvEncounterId", EncounterId);
                HshIn.Add("@intProviderId", iDoctorId);
                HshIn.Add("@chvDateRange", Daterange);
                HshIn.Add("@chrFromDate", iFromdate);
                HshIn.Add("@chrToDate", iTodate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientPrescriptionHistory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientMedicationHistory(int iHospitalLocationId, int IFacilityId, int iRegId, string EncounterId, int iDoctorId, string Daterange, string iFromdate, string iTodate)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", IFacilityId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@chvEncounterId", EncounterId);
                HshIn.Add("@intProviderId", iDoctorId);
                HshIn.Add("@chvDateRange", string.Empty);
                HshIn.Add("@chrFromDate", Daterange);
                HshIn.Add("@chrToDate", iTodate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientMedicationHistory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientVitalHistory(int iHospitalLocationId, int IFacilityId, int iRegId, string EncounterId, string Daterange, string iFromdate, string iTodate)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOutPut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", IFacilityId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvDateRange", string.Empty);
                HshIn.Add("@chrFromDate", Daterange);
                HshIn.Add("@chrToDate", iTodate);


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRServiceOrderDetails(int IHospitalLocationId, int IFacilityId, int IRegId, int IEncId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", IHospitalLocationId);
                HshIn.Add("@intFacilityId", IFacilityId);
                HshIn.Add("@intRegistrationId", IRegId);
                HshIn.Add("@intEncounterId", IEncId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEMRServiceOrderDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetPlan()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string sQuery = "SELECT PlanId,PlanNo,PlanName,Active FROM [dbo].[AccountPlans] WITH (NOLOCK) Order by PlanName";
            try
            {
                return objDl.FillDataSet(CommandType.Text, sQuery).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable GetPlanDetails(int iServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intServiceId", iServiceId);

            string sQuery = " SELECT AP.PlanId,AP.PlanNo,AP.PlanName,APD.ServiceId,APD.Active FROM AccountPlanDetails APD WITH (NOLOCK) INNER JOIN AccountPlans AP WITH (NOLOCK) ON APD.PlanId=AP.PlanId  " +
                            " WHERE APD.ServiceId = @intServiceId Order by AP.PlanName ";
            try
            {
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable InsertPlan(int iPlanId, string sPlanName, bool bSave, bool bActive)
        {
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("@intPlanId", iPlanId);
            HshIn.Add("@chvPlanName", sPlanName);
            HshIn.Add("@bitActive", bActive);
            HshIn.Add("@bitSave", bSave);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateAccountPlan", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable InsertPlanDetails(string iPlanId, int iServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("@xmlItems", iPlanId);
            HshIn.Add("@intServiceId", iServiceId);
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateAccountPlanDetails", HshIn, hshOutput);

                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int UpdatePlanDetails(int iPlanId, int ServiceId, bool bActive)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intPlanId", iPlanId);
            HshIn.Add("@intServiceId", ServiceId);
            HshIn.Add("@bitActive", bActive);
            try
            {

                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE AccountPlanDetails SET Active=@bitActive WHERE PLANID=@intPlanId AND ServiceId=@intServiceId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetEMRDrugSet(int iHospitalLocationId, int iSetId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iSetId", iSetId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDrugSet", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRDrugSetDetail(int iHospitalLocationId, int iSetId, int iItemId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iSetId", iSetId);
                HshIn.Add("@iItemId", iItemId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDrugSetDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int DeleteAllPlans(int iServiceId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iServiceId", iServiceId);
                string sQuery = "UPDATE AccountPlanDetails SET Active=0 WHERE ServiceId=@iServiceId";
                return objDl.ExecuteNonQuery(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetAllTypeTemplates(int iHospitalLocationId, string sType)
        {
            return GetAllTypeTemplates(iHospitalLocationId, sType, 0);
        }

        public DataSet GetAllTypeTemplates(int iHospitalLocationId, string sType, int FacililtyId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@chvType", sType);
            if (FacililtyId > 0)
            {
                HshIn.Add("@intFacilityId", FacililtyId);
            }

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAllTypeTemplates", HshIn);
        }

        public class EMRColorLegends
        {
            private string sConString = string.Empty;
            Hashtable HshIn;
            Hashtable HshOut;
            DAL.DAL Dl;
            StringBuilder strSQL;

            public EMRColorLegends(string Constring)
            {
                sConString = Constring;
            }

            public SqlDataReader getEmployeeId(int iHospID, int iUserId)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@UserID", iUserId);
                try
                {
                    SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select dbo.GetUserEmployeeId(@HospID,@UserID)", HshIn);
                    return dr;
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }
        }

        public class EMREmployeeProfile
        {
            public EMREmployeeProfile(string Constring)
            {
                sConString = Constring;
            }

            private string sConString = string.Empty;
            Hashtable HshIn;
            Hashtable HshOut;

            public String SaveEmployeeProfile(Int32 iEmpID, String sProfile, Int32 iUserID, Int32 iHospId, Int32 iFacilityId, Int32 iPageId)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intEmployeeId", iEmpID);
                HshIn.Add("txtProfile", sProfile);
                HshIn.Add("intEncodedBy", iUserID);
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@intPageId", iPageId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveEmployeeProfile", HshIn, HshOut);
                    return HshOut["chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public SqlDataReader getDoctorProfile(Int32 iEmpID)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("@EmployeeId", iEmpID);
                    SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select Profile from employeeprofile WITH (NOLOCK) where employeeid=@EmployeeId and active=1", HshIn);
                    return dr;
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }
            //Not in Use
            //public SqlDataReader getSpecialisation(Int16 iHospID)
            //{
            //    HshIn = new Hashtable();
            //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //    HshIn.Add("@HospID", iHospID);
            //    SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "Select distinct sm.Id, sm.Name from SpecialisationMaster  sm inner join doctordetails dd on specialisationid=sm.id where sm.Active=1", HshIn);
            //    return dr;
            //}
        }

        public class EMRFacility
        {
            private string sConString = string.Empty;
            public EMRFacility(String conString)
            {
                sConString = conString;
            }

            Hashtable HshIn;
            Hashtable HshOut;


            public String SaveFacility(Int32 iHospID, String sFacilityName, Int32 iUserID)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@chvName", sFacilityName);
                HshIn.Add("@intEncodedBy", iUserID);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveFacility", HshIn, HshOut);
                    return HshOut["@chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public DataSet GetFacility(int HospLocationId)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("inyHospitalLocationID", HospLocationId);
                    // HshIn.Add("@inyUserId", UserdId);

                    return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFacilityMaster", HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public DataSet getFacility(int iHospID, int EncodedBy)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("@inyHospitalLocationID", iHospID);
                    HshIn.Add("@intEncodedBy", EncodedBy);

                    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                    return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFacilityMaster", HshIn, HshOut);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }


            public Int32 DeActivateFacilityName(int iHospID, int iFacilityID, int iUserID)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn = new Hashtable();
                    HshIn.Add("@HospID", iHospID);
                    HshIn.Add("@FacilityID", iFacilityID);
                    return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE FacilityMaster SET Active = '0' WHERE FacilityID=@FacilityID and HospitalLocationID = @HospID", HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }
            //Not in Use
            //public Int32 UpdateFacility(Int16 iHospID, Int32 iFacilityID, String sName, Int32 iUserID, Byte bActive)
            //{
            //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //    HshIn = new Hashtable();
            //    HshIn.Add("@HospID", iHospID);
            //    HshIn.Add("@FacilityID", iFacilityID);
            //    HshIn.Add("@Name", sName);
            //    HshIn.Add("@Active", bActive);
            //    HshIn.Add("@EncodedBy", iUserID);
            //    Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE FacilityMaster SET Name=@Name,Active=@Active WHERE FacilityID=@FacilityID and HospitalLocationID = @HospID", HshIn);
            //    return i;
            //}
        }

        public class EMRVisit
        {
            private string sConString = string.Empty;
            public EMRVisit(String conString)
            {
                sConString = conString;
            }

            Hashtable HshIn;
            Hashtable HshOut;


            public String SaveVisit(Int32 iHospID, Int32 EMTypeId, String sVisitName, int iIsPackageVisit, Int32 iUserID, int iNoBillForPrivatePatient, int iNoPAForInsurancePatient)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@intVisitID", 0);
                HshIn.Add("@inyEMTypeId", EMTypeId);
                HshIn.Add("@chvName", sVisitName);
                HshIn.Add("@iIsPackageVisit", iIsPackageVisit);
                HshIn.Add("@NoBillForPrivatePatient", iNoBillForPrivatePatient);
                HshIn.Add("@NoPAForInsurancePatient", iNoPAForInsurancePatient);
                HshIn.Add("@intEncodedBy", iUserID);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveVisit", HshIn, HshOut);
                    return HshOut["@chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public DataSet GetVisit(int iHospID)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("inyHospitalLocationID", iHospID);
                    return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVisit", HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }

            public Int32 DeActivateVisitName(int iHospID, int iVisitID, int iUserID)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn = new Hashtable();
                    HshIn.Add("@HospID", iHospID);
                    HshIn.Add("@VisitID", iVisitID);
                    return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRVisitType SET Active = '0' WHERE VisitTypeID=@VisitID and HospitalLocationID = @HospID", HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public String UpdateVisit(int iHospID, int iVisitID, int EMTypeId, String sName, Byte iIsPackageVisit, int iUserID, Byte bActive, int iNoBillForPrivatePatient, int iNoPAForInsurancePatient)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@intVisitID", iVisitID);
                HshIn.Add("@inyEMTypeId", EMTypeId);
                HshIn.Add("@chvName", sName);
                HshIn.Add("@iIsPackageVisit", iIsPackageVisit);
                HshIn.Add("@NoBillForPrivatePatient", iNoBillForPrivatePatient);
                HshIn.Add("@NoPAForInsurancePatient", iNoPAForInsurancePatient);
                HshIn.Add("@inyActive", bActive);
                HshIn.Add("@intEncodedBy", iUserID);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveVisit", HshIn, HshOut);
                    return HshOut["@chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

                //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //HshIn = new Hashtable();
                //HshIn.Add("@HospID", iHospID);
                //HshIn.Add("@VisitID", iVisitID);
                //HshIn.Add("@Name", sName);
                //HshIn.Add("@Active", bActive);
                //HshIn.Add("@EncodedBy", iUserID);
                //Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRVisitType SET Type=@Name,Active=@Active WHERE VisitTypeID=@VisitID and HospitalLocationID = @HospID", HshIn);
                //return i;
            }

            public DataSet GetVisitType(int iHospID)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("@inyHospitalLocationID", iHospID);
                    string strsql = "select VisitTypeID,Type AS VisitTypeName from EMRVisitType WITH (NOLOCK) WHERE Active=1 AND HospitalLocationID=@inyHospitalLocationID ";
                    return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }

            public DataSet GetVisitTypeFacilityWise(int HospId, int VisitTypeId)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("@inyHospitalLocationId", HospId);
                    HshIn.Add("@intVisitTypeId", VisitTypeId);

                    string strsql = "SELECT DISTINCT evf.Id,fm.FacilityID, fm.Name, CONVERT(int,evf.Active) AS Active, " +
                                    " CASE CONVERT(int,evf.Active) WHEN 0 THEN 'In-Active' WHEN 1 THEN 'Active' END AS Status " +
                                    " FROM EMRvisitTypeFacility evf WITH (NOLOCK) LEFT JOIN FacilityMaster fm WITH (NOLOCK) ON evf.FacilityId=fm.FacilityID  " +
                                    " WHERE evf.VisitTypeID =@intVisitTypeId AND fm.HospitalLocationID = @inyHospitalLocationId Order By Name";

                    return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public int UpdateVisitTypeFacility(int Id, int Status, int UserId, DateTime Lcdate)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("@intId", Id);
                    HshIn.Add("@intStatusId", Status);
                    HshIn.Add("@intUserId", UserId);
                    HshIn.Add("@dtLastChangeDate", Lcdate);

                    string strsql = "UPDATE EMRvisitTypeFacility SET Active=@intStatusId,LastChangedBy =@intUserId,LastChangedDate=@dtLastChangeDate WHERE Id=@intId";

                    return objDl.ExecuteNonQuery(CommandType.Text, strsql, HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }

            public String SaveVisitTypeFacility(int VisitTypeId, string FacilityId, int UserID)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intVisitTypeId", VisitTypeId);
                HshIn.Add("@xmlItems", FacilityId);
                HshIn.Add("@intEncodedBy", UserID);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveVisitTypeFacility", HshIn, HshOut);
                    return HshOut["@chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

        }

        public class EMRRoomNo
        {
            private string sConString = string.Empty;
            public EMRRoomNo(String conString)
            {
                sConString = conString;
            }

            Hashtable HshIn;
            Hashtable HshOut;

            public DataSet GetRoomNo(int iFacilityID, int iHospID)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshIn.Add("inyHospitalLocationID", iHospID);
                    HshIn.Add("intFacilityID", iFacilityID);

                    return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetRoomNo", HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }

            public String SaveRoomNo(int iHospID, int iFacilityID, String sRoomNo, string IPAddress, int iEncodedBy)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospID);
                HshIn.Add("intFacilityID", iFacilityID);
                HshIn.Add("chvRoomNo", sRoomNo);
                HshIn.Add("chvIPAddress", IPAddress);
                HshIn.Add("intEncodedBy", iEncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveRoomNo", HshIn, HshOut);
                    return HshOut["chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }
            //Not in Use
            //public Int32 DeActivateRoomNo(Int32 iRoomID)
            //{
            //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //    HshIn = new Hashtable();
            //    HshIn.Add("@RoomID", iRoomID);

            //    Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE RoomMaster SET Active = '0'WHERE RoomID=@RoomID", HshIn);
            //    return i;
            //}
            public String UpdateRoomNo(int iHospID, int iFacilityID, int iEncodedBy, int iRoomID, String sRoomNo, string IPAddress, Byte bActive)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("inyHospitalLocationID", iHospID);
                HshIn.Add("intFacilityID", iFacilityID);
                HshIn.Add("chvRoomNo", sRoomNo);
                HshIn.Add("intRoomID", iRoomID);
                HshIn.Add("chvIPAddress", IPAddress);
                HshIn.Add("inyActive", bActive);
                HshIn.Add("intEncodedBy", iEncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveRoomNo", HshIn, HshOut);
                    return HshOut["chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }
        }

        public class Fonts
        {

            public DataSet GetFontDetails(string Type)
            {
                string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


                try
                {
                    String str = string.Empty;
                    //str = "Select FontId, Font" + Type + " from Font" + Type;
                    str = " Select FontId, Font" + Type +
                    " from Font" + Type + " WITH (NOLOCK) ";

                    if (Type == "Size")
                    {
                        str += " order by FontId ";
                    }
                    else if (Type == "Name")
                    {
                        str += " order by FontName ";
                    }

                    //BaseC.Patient objBc = new BaseC.Patient(sConString);
                    return objDl.FillDataSet(CommandType.Text, str);
                }
                catch (Exception ex) { throw ex; }
                finally { objDl = null; }

            }

            public string GetFont(string Type, string FontId)
            {
                DataSet objDs = new DataSet();
                objDs = GetFontDetails(Type);

                try
                {

                    DataView objDv = objDs.Tables[0].DefaultView;
                    if (Type == "Name")
                    {
                        objDv.RowFilter = "FontId =" + FontId;
                    }
                    else if (Type == "Size")
                    {
                        objDv.RowFilter = "FontSize =" + FontId;
                    }
                    if (objDv.Count > 0)
                    {
                        DataTable objDt = objDv.ToTable();
                        return objDt.Rows[0]["Font" + Type].ToString();
                    }
                    else
                        return string.Empty;
                }
                catch (Exception ex) { throw ex; }
                finally { objDs.Dispose(); }

            }

            public string GetFontNameAndSize(int HospitalLocationId, int FormId)
            {
                string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn = new Hashtable();
                DataSet ds = new DataSet();
                string Fonts = " font-family:Times New Roman ; font-size:9pt; ";
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FormId", FormId);

                try
                {
                    StringBuilder sQ = new StringBuilder();
                    sQ.Append("SELECT PgFontType, PgFontSize FROM EMRForms WITH (NOLOCK) ");
                    sQ.Append(" WHERE 2=2 ");
                    if (FormId > 0)
                    {
                        sQ.Append(" AND Id = @FormId ");
                    }
                    else
                    {
                        sQ.Append(" AND DefaultForVisit = 1 ");
                    }
                    sQ.Append(" AND HospitalLocationId  = @HospitalLocationId ");

                    ds = objDl.FillDataSet(CommandType.Text, sQ.ToString(), HshIn);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fonts = ConcatNameSize(Convert.ToString(ds.Tables[0].Rows[0]["PgFontType"]), Convert.ToString(ds.Tables[0].Rows[0]["PgFontSize"]));
                    }
                    return Fonts;
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public string ConcatNameSize(string Ftid, string FtSize)
            {
                string sName = string.Empty;
                string sFontSize = string.Empty;
                if (Ftid == "1")
                    sName += " font-family:Arial ";
                if (Ftid == "2")
                    sName += " font-family:Courier New ";
                if (Ftid == "3")
                    sName += " font-family:Garamond ";
                if (Ftid == "4")
                    sName += " font-family:Georgia ";
                if (Ftid == "5")
                    sName += " font-family:MS Sans Serif ";
                if (Ftid == "6")
                    sName += " font-family:Segoe UI";
                if (Ftid == "7")
                    sName += " font-family:Tahoma ";
                if (Ftid == "8")
                    sName += " font-family:Times New Roman ";
                if (Ftid == "9")
                    sName += " font-family:Verdana ";


                if (FtSize == "9pt")
                    sFontSize += " ; font-size:9pt ";
                if (FtSize == "11pt")
                    sFontSize += " ; font-size:11pt ";
                if (FtSize == "12pt")
                    sFontSize += " ; font-size:12pt ";
                if (FtSize == "14pt")
                    sFontSize += " ; font-size:14pt ";
                if (FtSize == "18pt")
                    sFontSize += " ; font-size:18pt ";
                if (FtSize == "20pt")
                    sFontSize += " ; font-size:20pt ";
                if (FtSize == "24pt")
                    sFontSize += " ; font-size:24pt ";
                if (FtSize == "26pt")
                    sFontSize += " ; font-size:26pt ";
                if (FtSize == "36pt")
                    sFontSize += " ; font-size:36pt ";

                return sName + sFontSize;
            }

            public DataSet GetPageFormat(int HospitalLocationId, int FormId)
            {
                string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FormId", FormId);
                StringBuilder sQ = new StringBuilder();
                try
                {

                    sQ.Append("SELECT PgSize, PgTopMargin, PgBottomMargin, PgLeftMargin, PgRightMargin, PgNoFormat, PgNoAllignment,Pg1HeaderId, ");
                    sQ.Append(" Pg1HeaderMargin, Pg1HeaderNote, Pg2HeaderId, Pg2HeaderNote, PgFontType, PgFontSize, Pg1HeaderFontBold, ");
                    sQ.Append(" Pg1HeaderFontItalic, Pg1HeaderFontUnderline, Pg1HeaderFontColor, Pg1HeaderFontType, Pg1HeaderFontSize, ");
                    sQ.Append(" Pg2HeaderFontBold, Pg2HeaderFontItalic, Pg2HeaderFontUnderline, Pg2HeaderFontColor, Pg2HeaderFontType, ");
                    sQ.Append(" Pg2HeaderFontSize, DisplayPgNoInPage1, ISNULL(h1.ShowBorder,0) AS pg1ShowBorder, ");
                    sQ.Append(" ISNULL(h2.ShowBorder,0) AS pg2ShowBorder FROM EMRForms f WITH (NOLOCK) ");
                    sQ.Append(" INNER JOIN EMRFormHeader h1 WITH (NOLOCK) ON f.Pg1HeaderId = h1.HeaderId ");
                    sQ.Append(" INNER JOIN EMRFormHeader h2 WITH (NOLOCK) ON f.Pg2HeaderId = h2.HeaderId ");
                    sQ.Append(" WHERE 2=2 ");
                    if (FormId > 0)
                    {
                        sQ.Append(" AND f.Id = @FormId ");
                    }
                    else
                    {
                        sQ.Append(" AND f.DefaultForVisit= 1 ");
                    }
                    sQ.Append(" AND f.HospitalLocationId  = @HospitalLocationId ");

                    return objDl.FillDataSet(CommandType.Text, sQ.ToString(), HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }
            public string GetPageFormatString(int HospitalLocationId, int FormId, string pg)
            {
                DataSet ds = new DataSet();

                try
                {
                    ds = GetPageFormat(HospitalLocationId, FormId);
                    DataRow drPageHeaderFormat = ds.Tables[0].Rows[0];
                    string sFont = ConcatNameSize(Convert.ToString(drPageHeaderFormat["PgFontType"]), Convert.ToString(drPageHeaderFormat["PgFontSize"]));
                    string color = "#" + drPageHeaderFormat[pg + "HeaderFontColor"].ToString();
                    string bold = string.Empty, italic = string.Empty;
                    if (drPageHeaderFormat[pg + "HeaderFontBold"].ToString() == "True")
                        bold = "font-weight:bold;";
                    if (drPageHeaderFormat[pg + "HeaderFontItalic"].ToString() == "True")
                        italic = "font-style:italic;";
                    string HeaderFont = "style='" + sFont + ";" + bold + italic;
                    return HeaderFont;
                }
                catch (Exception ex) { throw ex; }
                finally { ds.Dispose(); }

            }
        }

        // add by Balkishan start
        public string SaveReferedbyDoctor(int HospitalLocationID, string ReferType, string name, string address, int city, int state, int country, string zip, string phonehome, string mobile, string fax, string email, string licenceno, int region, int billfee, int UserID, int FacilityId, int id, int cmd, int active)
        {

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@HospitalLocationID", HospitalLocationID);
            HshIn.Add("@ReferralTypeId", ReferType);
            HshIn.Add("@Name", name);
            HshIn.Add("@Address", address);
            HshIn.Add("@City", city);
            HshIn.Add("@State", state);
            HshIn.Add("@Country", country);
            HshIn.Add("@Pin", zip);
            HshIn.Add("@Phone", phonehome);
            HshIn.Add("@Mobile", mobile);
            HshIn.Add("@Fax", fax);
            HshIn.Add("@Email", email);
            HshIn.Add("@LicenseId", licenceno);
            HshIn.Add("@RefRegionId", region);
            HshIn.Add("@ReleaseUnsettledBillFee", billfee);
            HshIn.Add("@intEncodedBy", UserID);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@id", id);
            HshIn.Add("@cmd", cmd);
            HshIn.Add("@active", active);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveReferDoctorMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveReferServiceGroupTagging(string xmlTaggingValues)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@xmlTaggingValues", xmlTaggingValues);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveReferServiceGroupTagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public DataSet GetConsultationVisit()
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetConsultationVisit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet CurrencyDetails(int HospitalLocationId, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCurrencyDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCurrencyMaster(int iHospitalLocationId, int Id, string CurDescription, string CurCode, string EffectiveDate,
            bool Status, double ExchangeRate, int intEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intID", Id);
            HshIn.Add("@chrName", CurCode);
            HshIn.Add("@chrDescription", CurDescription);
            HshIn.Add("@dblExchangeRate", ExchangeRate);
            HshIn.Add("@chrEffectiveDate", EffectiveDate);
            HshIn.Add("@bitStatus", Status);

            HshIn.Add("@intEncodedBy", intEncodedBy);

            try
            {

                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCurrencyMaster", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBillingGroupDetail(int BillingGroupId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intBillingGroupId", BillingGroupId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBillingGroupDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDepartmentAccordingEncounter(int iHospitalLocationId, int FacilityId, int iRegId, int iEncId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@RegId", iRegId);
            HshIn.Add("@EncounterId", iEncId);
            return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDepartmentAccordingEncounter", HshIn);


        }
        public DataSet GetGroupModuleName()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                string sQuery = "SELECT DISTINCT smom.ModuleId, sm.ModuleName FROM SecModuleOptionsMaster smom WITH (NOLOCK) INNER JOIN secModuleMaster sm WITH (NOLOCK) on sm.ModuleId = smom.ModuleId AND sm.Active = 1 WHERE smom.Active = 1 Order By sm.ModuleName ";
                return objDl.FillDataSet(CommandType.Text, sQuery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetGroupPageName(int iModuleId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            StringBuilder sb = new StringBuilder();
            HshIn.Add("@intModuleId", iModuleId);
            try
            {

                string sQuery = "SELECT DISTINCT SMOM.PageCode FROM SecModuleOptionsMaster SMOM WITH (NOLOCK) WHERE SMOM.ModuleId =@intModuleId AND SMOM.Active = 1 ";
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetGroupOptionName(int iModuleId, int iGroupId, string sPageCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intModuleId", iModuleId);
            HshIn.Add("@chvPageCode", sPageCode);
            HshIn.Add("@intGroupId", iGroupId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSecGroupMenuOptions", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveSecGroupMenuTagging(int iGroupId, int iModuleId, string xmlOptionIds, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable hsout = new Hashtable();
            HshIn.Add("@intGroupId", iGroupId);
            HshIn.Add("@xmlOptionIds", xmlOptionIds);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@intModuleId", iModuleId);

            hsout.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hsout = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSecGroupMenuTagging", HshIn, hsout);
                return hsout;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string RemoveAllSecGroupMenuTagging(int iGroupId, int iModuleId)
        {
            string sResult = string.Empty;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                string sQuery = "UPDATE SecGroupOptions SET Active=0 WHERE OptionId IN (SELECT sgo.OptionId FROM SecGroupOptions sgo WITH (NOLOCK) INNER JOIN SecModuleOptionsMaster smo WITH (NOLOCK) ON  sgo.OptionId=smo.OptionId WHERE sgo.GroupId=@intGroupId AND smo.ModuleId=@intModuleId AND sgo.Active=1 AND smo.Active=1)";
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@intGroupId", iGroupId);
                HshIn.Add("@intModuleId", iModuleId);
                int i = objDl.ExecuteNonQuery(CommandType.Text, sQuery, HshIn);
                if (i == 0)
                {
                    sResult = "Record Updated...";
                }
                return sResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetTemplateGroup(int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@FacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTemplateGroup", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRTemplate()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string sQuery = "SELECT Id, TemplateName FROM EMRTemplate WITH (NOLOCK) where Active=1 ORDER BY TemplateName";

            try
            {
                return objDl.FillDataSet(CommandType.Text, sQuery);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRTemplateGroupWise(int GroupId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intGroupId", GroupId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEMRTemplateGroupWise", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEMRTemplateGroupWise(int iHospId, string iXmlData, int GroupId, bool iActive, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable(); ;

            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("xmlData", iXmlData);
            HshIn.Add("intGroupId", GroupId);
            HshIn.Add("btActive", iActive);
            HshIn.Add("intEncodedBy", iEncodedby);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateGroupWise", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveEMRFileRequest(int HospitalLocationId, int RegistrationId, int EncounterId, string Remarks, int User)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable hsout = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intEncodedBy", User);

            hsout.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hsout = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRFileRequest", HshIn, hsout);
                return hsout;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int ValidateUserForEMRFile(int HospitalLocationId, int RegistrationId, int EncounterId, int User)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable hsout = new Hashtable();

            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", User);
            hsout.Add("@intStatus", SqlDbType.Int);
            try
            {
                hsout = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspValidateUserForEMRFile", HshIn, hsout);

                return Convert.ToInt32(hsout["@intStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetConsultationType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select StatusId as ConsultationTypeID,Status as ConsultationType from StatusMaster WITH (NOLOCK) where StatusType='ConsultationType' and Active=1 ORDER BY Status ");

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }

        }

        public string SaveEMRTreatmentTemplate(int intTemplateId, int intItemId, decimal decDose, int intDoseUnitId, int intFrequencyId, int intDay, string chrDaysType, int intFoodRelationId, string chvRemarks, int intServiceId, int intEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable(); ;

            HshIn.Add("@intTemplateId", intTemplateId);
            HshIn.Add("@intItemId", intItemId);
            HshIn.Add("@decDose", decDose);
            HshIn.Add("@intDoseUnitId", intDoseUnitId);
            HshIn.Add("@intFrequencyId", intFrequencyId);
            HshIn.Add("@intDays", intDay);
            HshIn.Add("@chrDaysType", chrDaysType);
            HshIn.Add("@intFoodRelationId", intFoodRelationId);
            HshIn.Add("@chvRemarks", chvRemarks);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@intEncodedBy", intEncodedBy);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);


            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveTreatmentPlanDetails", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string DeleteEMRTreatmentTemplate(int intTemplateId, int intItemId, int intServiceId, int intEncodedBy, int DiagnosisId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable(); ;

            HshIn.Add("@intTemplateId", intTemplateId);
            HshIn.Add("@intItemId", intItemId);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@intDiagnosisId", DiagnosisId);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);


            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteTreatmentPlanDetails", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string UpdateEMRTreatmentTemplate(int intTemplateId, int intItemId, int intDoseUnitId, int intFrequencyId, int intDay, string chrDaysType, int intFoodRelationId, string chvRemarks, int intServiceId, int intEncodedBy, string planofcare, string Instructions
            , string ChiefComplaints, string History, string Examination, string Diagnosis, int ChiefComplaintsDuration, string ChiefComplaintsDurationType
            , string ICDCode, int ICDID, string ICDDescription)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable(); ;

            HshIn.Add("@intTemplateId", intTemplateId);
            HshIn.Add("@intItemId", intItemId);

            HshIn.Add("@intDoseUnitId", intDoseUnitId);
            HshIn.Add("@intFrequencyId", intFrequencyId);
            HshIn.Add("@intDays", intDay);
            HshIn.Add("@chrDaysType", chrDaysType);
            HshIn.Add("@intFoodRelationId", intFoodRelationId);
            HshIn.Add("@chvRemarks", chvRemarks);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@chvPlanofcare", planofcare);
            HshIn.Add("@chvInstructions", Instructions);

            HshIn.Add("@chvChiefComplaints", ChiefComplaints);
            HshIn.Add("@chvHistory", History);
            HshIn.Add("@chvExamination", Examination);
            HshIn.Add("@chvDiagnosis", Diagnosis);
            HshIn.Add("@ChiefComplaintsDuration", ChiefComplaintsDuration);
            HshIn.Add("@ChiefComplaintsDurationType", ChiefComplaintsDurationType);
            HshIn.Add("@ICDCode", ICDCode);
            HshIn.Add("@ICDID", ICDID);
            HshIn.Add("@ICDDescription", ICDDescription);


            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveTreatmentPlanDetails", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveTemplateGroup(int HospitalLocationId, int GroupId, int FacilityID, string chvGroupName,
                                        bool DisplayInMenuEMR, bool DisplayInMenuNB, bool Active, int EncodedBy, string chvErrorStatus,
                                        bool IsPackageTemplate, bool DisplayInWard)
        {

            //            @inyHospitalLocationId smallint,
            //@intGroupId int = 0,          
            //@inyFacilityID smallint,        
            //@chvGroupName varchar(100),   
            //@bitDisplayInMenuEMR bit = 0, 
            //@bitDisplayInMenuNB bit = 0, 
            //@bitActive bit = 1,          
            //@intEncodedBy int,          
            //@chvErrorStatus varchar(200) output   

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable(); ;

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@inyFacilityID", FacilityID);
            HshIn.Add("@chvGroupName", chvGroupName);
            HshIn.Add("@bitDisplayInMenuEMR", DisplayInMenuEMR);
            HshIn.Add("@bitDisplayInMenuNB", DisplayInMenuNB);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@IsPackageTemplate", IsPackageTemplate);
            HshIn.Add("@bitDisplayInWard", DisplayInWard);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveTemplateGroup", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetHospitalDepartmentByFacility(int iHospitalLocationId, int intFacilityId, string sServiceType, int iEntrySiteId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("intFacilityId", intFacilityId);
            HshIn.Add("chvDepartmentType", sServiceType);
            HshIn.Add("iEntrySiteId", iEntrySiteId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalDepartments", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetMHCReportFormatId()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select ReportId,ReportName from EMRTemplateReportSetup with (nolock) where ReportType='HC' and Active = 1 Order By ReportName");

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }

        }

        public DataSet EMRGetTemplateGroup(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroup", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #region PPdoctorsetup
        public Hashtable SavePPdoctorsetup(int iHospitalLocationId, int iDoctorId, int FacilityId,
         int DiscountAmount, int DiscountPercentage, int DiscountAmountOnArrival, int DiscountPercentageOnArrival, bool IsOnlinePaymentMandatory, int TeamId,
         int MorningSlots, bool MorningExtraChargeAllow, int MorningExtraChargePercent, int MorningExtraChargeAmt, int EveningSlots,
         bool EveningExtraChargeAllow, int EveningExtraChargePercent, int EveningExtraChargeAmt, string MorningSlotCutOffTime, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDiscountAmount", DiscountAmount);
                HshIn.Add("@intDiscountPercentage", DiscountPercentage);
                HshIn.Add("@intDiscountAmountOnArrival", DiscountAmountOnArrival);
                HshIn.Add("@intDiscountPercentageOnArrival", DiscountPercentageOnArrival);
                HshIn.Add("@btIsOnlinePaymentMandatory", IsOnlinePaymentMandatory);
                HshIn.Add("@intTeamId", TeamId);
                HshIn.Add("@intMorningSlots", MorningSlots);
                HshIn.Add("@btMorningExtraChargeAllow", MorningExtraChargeAllow);
                HshIn.Add("@intMorningExtraChargePercent", MorningExtraChargePercent);
                HshIn.Add("@intMorningExtraChargeAmt", MorningExtraChargeAmt);
                HshIn.Add("@intEveningSlots ", EveningSlots);
                HshIn.Add("@btEveningExtraChargeAllow ", EveningExtraChargeAllow);
                HshIn.Add("@intEveningExtraChargePercent", EveningExtraChargePercent);
                HshIn.Add("@intEveningExtraChargeAmt", EveningExtraChargeAmt);
                HshIn.Add("@tmMorningSlotCutOffTime", MorningSlotCutOffTime);
                HshIn.Add("@intEncodedBy ", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPPSavedoctorsetup", HshIn, HshOut);

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

        public DataTable GetPPdoctorsetup(int iHospitalLocationID, int iFacilityId, int DoctorId)
        {

            try
            {
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intDoctorId", DoctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPPGetdoctorsetup", HshIn).Tables[0];
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

        }

        public DataTable GetTeamByDoctors(int iHospitalLocationID, int iFacilityId, int DoctorId)
        {

            try
            {
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intDoctorId", DoctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "GetTeamByDoctors", HshIn).Tables[0];
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
        #endregion

        public string SaveAQmsAppointment(int AppointmentId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intAppointmentId", AppointmentId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveAQmsAppointment", HshIn, HshOut);
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
        public DataSet getDoctorPatientListsAttributes(int FacilityId, int EncounterId, string EncounterType, int UserId, int GroupId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrEncounterType", EncounterType);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intGroupId", GroupId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetDoctorPatientListsAttributes", HshIn);
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

    }
}
