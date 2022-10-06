using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;


namespace BaseC
{
    public class Hospital
    {
        private string sConString = string.Empty;
        private Hashtable HshIn;
        private Hashtable hshOutput;

        public Hospital(string Constring)
        {
            sConString = Constring;
        }

        public void CreateHospital()
        {
            throw new System.NotImplementedException();
        }

        public void LockHospital()
        {
            throw new System.NotImplementedException();
        }

        public void DeactivateHospital()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateHospitalDetails()
        {
            throw new System.NotImplementedException();
        }

        public void CreateHospitalBranches()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateHospitalBranchDetails()
        {
            throw new System.NotImplementedException();
        }

        public void LockHospitalBranch()
        {
            throw new System.NotImplementedException();
        }

        public void GetHospitalDetails()
        {
            throw new System.NotImplementedException();
        }

        public void GetAllPlans()
        {
            throw new System.NotImplementedException();
        }

        public void GetHospitalUsers()
        {
            throw new System.NotImplementedException();
        }

        public void GetHospitalModules()
        {
            throw new System.NotImplementedException();
        }

        public Boolean GetHospitalSetUp(string Flag, int HospitalLocationID, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationID", HospitalLocationID);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("Flag", Flag);
                string strConfig = (string)objDl.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USPHospitalSetUp", HshIn);
                if (strConfig == "Y")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet fillDoctorCombo(int HospitalId, int intSpecialisationId, int FacilityId)
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

        public DataSet GetSpecialRightsEmployeeList(int HospitalId, int intSpecialisationId, int FacilityId, string SpecialRightFlag)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("HospitalLocationId", HospitalId);
            HshIn.Add("intSpecialisationId", intSpecialisationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("chvSpecialRightFlag", SpecialRightFlag);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSpecialRightsEmployeeList", HshIn);
        }


        public DataSet GetAuthorizedPrecnt(int iProviderID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intProviderId", iProviderID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetProviderDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOTEquipmentmaster(int iHospitalLocationId, int iFacilityId, int sSearchWithService)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@sSearchWithService", sSearchWithService);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetOTEquipmentmaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOTDoctorTagDetail(int iHospitalLocationId, int iFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorOTTaggingDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorDepartment(int iHospitalLocationId, int iFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetOTDoctorDepartment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable uspSaveOTEquipmentmaster(int iHospitalLocationId, int iFacilityId, int iEquipmentId, int iServiceId, int iActive)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("iHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
                HshIn.Add("iFacilityId", Convert.ToInt32(iFacilityId));
                HshIn.Add("iEquipmentId", iEquipmentId);
                HshIn.Add("iServiceId", iServiceId);
                HshIn.Add("iActive", iActive);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveOTEquipmentmaster", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string InsertTableData(String sColNames, String sColValues, String sColTable)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                hshOutput = new Hashtable();

                HshIn.Add("sColNames", sColNames);
                HshIn.Add("sColValues", sColValues);
                HshIn.Add("sColTable", sColTable);
                hshOutput.Add("iTableId", SqlDbType.Int);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "InsertTableData", HshIn, hshOutput);

                return hshOutput["iTableId"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        /// <summary>
        /// Get resource list
        /// Created by Saten
        /// Created On 02 Mar 2013
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="bitActive"></param>
        /// <param name="iResourceId"></param>
        /// <param name="iSubDeptId"></param>
        /// <returns></returns>
        public DataSet GetResourceMaster(int iHospitalLocationId, int iFacilityId, int bitActive, int iResourceId, int iSubDeptId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@bitActive", bitActive);
            HshIn.Add("@iResourceId", iResourceId);
            HshIn.Add("@iSubDeptId", iSubDeptId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetResourceMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int ExistingResourceBreak(int FacilityId, int TheaterId, string sBreakDate, string sStartTime, string sEndTime)
        {
            int BreakId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intTheaterId", TheaterId);
                HshIn.Add("@chrBreakDate", sBreakDate);
                HshIn.Add("@chrStartTime", sStartTime);
                HshIn.Add("@chrEndTime", sEndTime);

                string sQuery = "SELECT ID as BreakId " +
                                " FROM ResourceBreakList with (nolock) " +
                                " WHERE ResourceID=@intTheaterId AND FacilityID=@intFacilityID AND Active=1 " +
                                " AND SUBSTRING(@chrStartTime,0,6) BETWEEN CONVERT(VARCHAR(5), StartTime,108) AND CONVERT(VARCHAR(5), EndTime,108) " +
                                " AND SUBSTRING(@chrEndTime,0,6) BETWEEN CONVERT(VARCHAR(5), StartTime,108) AND CONVERT(VARCHAR(5), EndTime,108) " +
                                " AND CONVERT(VARCHAR(10), BreakDate,111)=@chrBreakDate ";

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    BreakId = Convert.ToInt32(dr["BreakId"]);
                }
                dr.Close();
                return BreakId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getResourceBreakDetails(int BreakId, int TheaterId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intBreakId", BreakId);
            HshIn.Add("@intTheaterId", TheaterId);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetResourceBreakDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveResourceBreaks(int BreakId, int TheaterId, int HospId, int FacilityId, string BreakName, string BreakDate,
                                   string StartTime, string EndTime, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intBreakId", BreakId);
            HshIn.Add("@intTheaterId", TheaterId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvBreakName", BreakName);
            HshIn.Add("@chrBreakDate", BreakDate);
            if (StartTime != string.Empty)
            {
                HshIn.Add("@chrStartTime", StartTime);
            }
            if (StartTime != string.Empty)
            {
                HshIn.Add("@chrEndTime", EndTime);
            }
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveResourceBreaks", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteResourceBreaks(int breakid, int resourceid, int facilityid, int hospitallocation, int userid)
        {
            String Returnmessage;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                Hashtable HasOut = new Hashtable();
                HshIn.Add("@intBreakId", breakid);
                HshIn.Add("@intTheaterId", resourceid);
                HshIn.Add("@intFacilityId", facilityid);
                HshIn.Add("@inyHospitalLocationId", hospitallocation);
                HshIn.Add("@intEncodedBy", userid);
                HasOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HasOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteResourceBreakDetails", HshIn, HasOut);
                if (HasOut["@chvErrorStatus"].ToString().Contains("Deleted"))
                {
                    Returnmessage = "Break Removed.";
                }
                else
                {
                    Returnmessage = "Error Occured. Please Contact System Administrator.";

                }
            }
            catch (Exception Ex)
            {
                Returnmessage = Ex.ToString();
            }
            finally { HshIn = null; objDl = null; }

            return Returnmessage;
        }
        public DataSet GetHospitalHoliday(int HospitalId, string sHolidayDate)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@dtHolidayDate", sHolidayDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalHolidays", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveUpdateHospitalEmployeeLeaves(int iLeaveId, int iEmployeeId, string sFromDate, string sToDate, string sRemarks, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOutput = new Hashtable();
                HshIn.Add("@intLeaveId", iLeaveId);
                HshIn.Add("@intEmployeeId", iEmployeeId);
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrFromTo", sToDate);
                HshIn.Add("@chvRemarks", sRemarks);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                hshOutput.Add("@chvErrorOutput", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateEmployeeLeaves", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetHospitalEmployeeLeave(string xmlEmployeeId, string sDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@xmlEmployeeId", xmlEmployeeId);
                HshIn.Add("@chrDate", sDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeLeaves", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int DeleteHospitalEmployeeLeave(int iEmployeeId, int iLeaveId, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                string sQuery = "UPDATE EmployeeLeaves SET Active=0,LastChangedBy=@intEncodedBy,LastChangedDate=GETUTCDATE() WHERE EmployeeID=@intEmployeeId AND id=@intLeaveId";
                HshIn.Add("@intEmployeeId", iEmployeeId);
                HshIn.Add("@intLeaveId", iLeaveId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                return objDl.ExecuteNonQuery(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetReportHeader(string type)
        {
            return GetReportHeader(type, 0);
        }
        public DataSet GetReportHeader(string type, int ReportId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@CharType", type);
            if (ReportId > 0)
            {
                HshIn.Add("@intReportId", ReportId);
            }
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetReportHeader", HshIn);
        }

        public DataSet getEMRFormHeader(int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intHospId", HospId);
            try
            {
                string sQuery = "SELECT HeaderId, HeaderName FROM EMRFormHeader WITH (NOLOCK) WHERE HospitalLocationId = @intHospId AND Active = 1 ORDER BY HeaderName";

                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }



        //<%------------------------------------Yogesh-----------------1/04/2022----------------%>
        public DataSet getImageHW()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateImage");

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }

        }
        public DataSet getType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateImageType");
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }
        }

        public string saveHeightWidthImage(int Reportid, int height, int width, int typeId, bool isHeaderHospitalLogo, bool IsHeaderAddress, bool IsHeaderNABHLogo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@Reportid", Reportid);
                HshIn.Add("@height", height);
                HshIn.Add("@width", width);
                HshIn.Add("@typeid", typeId);

                //Akshay
                HshIn.Add("@isShowHospitalLogo", isHeaderHospitalLogo);
                HshIn.Add("@isShowHeaderAddress", IsHeaderAddress);
                HshIn.Add("@isShowNABHLogo", IsHeaderNABHLogo);

                HshOut.Add("@out", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveHeigthWidthImage", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; HshIn = null; }
            return HshOut["@out"].ToString();


        }


        //<%---------------------------------Yogesh------------------1/04/2022----------------%>


        public DataSet GetReportFooter(string chrSource, int labno, string sServiceIds, int iFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chrSource", chrSource);
                HshIn.Add("@intLabNo", labno);
                HshIn.Add("@chvServiceIds", sServiceIds);
                HshIn.Add("@intLoginFacilityId", iFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintResultSignature", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReportFooter(string chrSource, int labno)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chrSource", chrSource);
                HshIn.Add("@intLabNo", labno);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintResultSignature", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetReportList(int ReportID, int GroupID, string RType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ReportId", ReportID);
                HshIn.Add("@GroupId", GroupID);
                HshIn.Add("@Type", RType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetParameter", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// GetParameter have internal setting 
        /// </summary>
        /// <param name="Reportname"></param>
        /// <param name="RType">P</param>
        /// <returns></returns>
        public DataTable GetReportParamtersList(int ReportID, int GroupID, string RType)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ReportId", ReportID);
                HshIn.Add("@GroupId", GroupID);
                HshIn.Add("@Type", RType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetParameter", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetReportsByGroupID(int GroupID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@GroupId", GroupID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDynamicGetReportListByGroupID", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetDynamicReportsAll()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDynamicReportMaster").Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int DynamicSaveSecGroupReport(string ReportId, int GroupID, int Encodedby)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@ReportId", ReportId);
            HshIn.Add("@GroupId", GroupID);
            HshIn.Add("@EncodedBy", Encodedby);

            try
            {
                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspDynamicSaveSecGroupReport", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int DynamicSaveReportParameter(int ReportId, string ParameterName, string Parametertype, string DisplayName,
            string DefaultValue, int ControlType, string Query, bool Active, bool Visible)
        {
            int result = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ReportId", ReportId);
                HshIn.Add("@ParameterName", ParameterName);
                HshIn.Add("@ParameterType", Parametertype);
                HshIn.Add("@DisplayName", DisplayName);
                HshIn.Add("@DefaultValue", DefaultValue);
                HshIn.Add("@ControlType", ControlType);
                HshIn.Add("@Visible", Visible);
                HshIn.Add("@QueryString", Query);
                hshOutput.Add("@Status", SqlDbType.Int);
                result = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspDynamicSaveParameterDetail", HshIn, hshOutput);
                result = Convert.ToInt32(hshOutput["@Status"]);
                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataTable ReportQueryResult(string Qry)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "Qry").Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        //added by niraj Start 01 Dec 2015
        public DataSet GetEMRTreatmentPlanTemplatesmaster(int iFacilityId, int DepartmentId, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intDepartmentId", DepartmentId);
            HshIn.Add("@DoctorId", DoctorId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentTemplates", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetEMRDrugSetmaster(int HospitalLocationId, int SetId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@iHospitalLocationId", HospitalLocationId);
            HshIn.Add("@iSetId", SetId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDrugSet", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        //public DataSet GetEMROrderSetmaster(int HospitalLocationId, int SetId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("@iHospitalLocationId", HospitalLocationId);
        //    HshIn.Add("@iSetId", SetId);

        //    return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetOrderSet", HshIn);
        //}


        public DataSet GetEMRTreatmentPlanSpecialities(int iTempLateId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intTemplateId", iTempLateId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentTemplateDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRFavourateDetails(int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intDoctorId", DoctorId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFavourateDrugDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet GetEMRDrugSetdetails(int HospitalLocationId, int SetId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@iHospitalLocationId", HospitalLocationId);
            HshIn.Add("@iSetId", SetId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDrugSetDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRServiceSetdetails(int HospitalLocationId, int SetId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            //HshIn.Add("@iHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intSetId", SetId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetOrderSetDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }





        public DataSet GetEMRFavourateServiceOrdersDetails(int DoctorId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMROrderSetmaster(int HospitalLocationId, int SetId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@iSetId", SetId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetOrderSet", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet GetEMRTreatmentPlanName()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            //HshIn.Add("@iSetId", SetId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTreatmentPlanTemplates");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        public string SaveEMRTreatmentPlanName(int FacilityId, String TemplateName, int SpecilityId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            try
            {
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@TemplateName", TemplateName);
                HshIn.Add("@SpecilityId", SpecilityId);
                HshIn.Add("@Encodedby", EncodedBy);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTreatmentPlanTemplates", HshIn, HshOut);
                return HshOut["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string UpdateEMRTreatmentPlanName(int TemplateId, String TemplateName, int EncodedBy)

        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@TemplateId", TemplateId);
            HshIn.Add("@TemplateName", TemplateName);
            HshIn.Add("@Encodedby", EncodedBy);
            HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEMRTreatmentPlanTemplates", HshIn, HshOut);
                return HshOut["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string DeletEMRTreatmentPlanName(int TemplateId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@TemplateId", TemplateId);
            HshIn.Add("@Encodedby", EncodedBy);
            HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeletEMRTreatmentPlanTemplates", HshIn, HshOut);
                return HshOut["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEMRTreatmentPlanMaster(int TemplateId, int HospitalLocationId, int FacilityId, int DepartmentId, string TemplateName, int Encodedby, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intTemplateId", TemplateId);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@intDepartmentId", DepartmentId);
            HshIn.Add("@TemplateName", TemplateName);
            HshIn.Add("@Encodedby", Encodedby);
            HshIn.Add("@DoctorId", DoctorId);
            try
            {

                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTreatmentPlanTemplates", HshIn, HshOut);
                return HshOut["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }




        //added by niraj end


    }
}