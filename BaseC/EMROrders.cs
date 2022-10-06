using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class EMROrders
    {
        private string sConString = "";
        Hashtable HshIn;
        public EMROrders(string Constring)
        {
            sConString = Constring;
        }

        public DataSet GetDepartments()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select DepartmentID, DepartmentName FROM departmentmain WITH (NOLOCK) WHERE Active = 1 order by DepartmentName");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader GetSubDepartments(Int16 DepartmentID)
        {
            HshIn = new Hashtable();
            string sQ = "";
            HshIn.Add("DepartmentID", DepartmentID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                if (DepartmentID == 0)
                    sQ = "SELECT SubDeptId, SubName FROM departmentsub WITH (NOLOCK) WHERE Active = 1 ORDER BY subName ";
                else
                    sQ = "SELECT SubDeptId, SubName FROM departmentsub WITH (NOLOCK) WHERE DepartmentID = @DepartmentID AND Active = 1 ORDER BY subName ";

                SqlDataReader objDs = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQ, HshIn);
                return objDs;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Not in Use
        //public SqlDataReader GetSubDepartments(Int16 DepartmentID, string dsType)
        //{
        //    HshIn = new Hashtable();
        //    string sQ = "";
        //    HshIn.Add("DepartmentID", DepartmentID);
        //    HshIn.Add("Type", dsType);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    if (DepartmentID == 0)
        //        sQ = "SELECT SubDeptId, SubName FROM departmentsub WHERE Active = 1 AND Type=@Type ORDER BY subName ";
        //    else
        //        sQ = "SELECT SubDeptId, SubName FROM departmentsub WHERE Active = 1 AND Type=@Type AND DepartmentID = @DepartmentID ORDER BY subName ";

        //    SqlDataReader objDs = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQ, HshIn);
        //    return objDs;
        //}

        public Int16 UpdateCPTCodeSercice(int ServiceID, int CPTID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("CPTID", CPTID);
                HshIn.Add("serviceID", ServiceID);
                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE ItemOfService SET CPTID = @CPTID WHERE serviceID = @ServiceID", HshIn);
                return 0;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetHospitalServices(int iHospitalLocationId, int iDepartmentId, int iSubDepartmentId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            if (iSubDepartmentId != 0)
            {
                HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            }
            HshIn.Add("chrType", "");
            HshIn.Add("intFacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetHospitalServices(Int16 iHospitalLocationId, string xServiceID)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("ServiceIdxml", xServiceID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEMRServices", HshIn);

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
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveOPOrder(int iRegistrationId, int iOPEnounterID, int iDoctorId, Int16 iHospitalLocationId, string sOrderDate, string sServices, int iEncodedBy, bool iAllergyReviewed, string FacilityId, string pageId, bool iPullForwardEMCodes, out string sOrderID, int iTemplateID)
        {
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            HshIn.Add("intRegistrationId", iRegistrationId);
            //HshIn.Add("intRegistrationNo", iRegistrationNo);
            HshIn.Add("intOPEnounterID", iOPEnounterID);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intHospitalLocationId", iHospitalLocationId);
            if (iTemplateID != 0)
            {
                HshIn.Add("intTemplateID", iTemplateID);
            }
            HshIn.Add("intLoginFacilityId", FacilityId);
            HshIn.Add("intPageId", pageId);
            HshIn.Add("dtsOrderDate", sOrderDate);
            HshIn.Add("xmlServices", sServices);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("bitAllergyReviewed", iAllergyReviewed);
            hstOutput.Add("intOrderNo", SqlDbType.Int);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            // HshIn.Add("bitPullForwardOrder", false);
            HshIn.Add("bitPullForwardEMCodes", iPullForwardEMCodes);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEMROPOrder", HshIn, hstOutput);
                int i = 0;
                if (hstOutput["intOrderNo"].ToString() != "")
                    i = (int)hstOutput["intOrderNo"];
                string strMsg = hstOutput["chvErrorStatus"].ToString();
                sOrderID = i.ToString();
                return strMsg;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public String SaveOPOrder(Int64 iRegistrationNo, int iOPEnounterID, int iDoctorId, Int16 iHospitalLocationId, string sOrderDate, string sServices, int iEncodedBy, Int16 iAllergyReviewed, out string sOrderID, int iTemplateID)
        {
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            //HshIn.Add("intRegistrationNo", iRegistrationNo);
            HshIn.Add("intOPEnounterID", iOPEnounterID);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intHospitalLocationId", iHospitalLocationId);
            if (iTemplateID != 0)
            {
                HshIn.Add("intTemplateID", iTemplateID);
            }
            HshIn.Add("dtsOrderDate", sOrderDate);
            HshIn.Add("xmlServices", sServices);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("bitAllergyReviewed", iAllergyReviewed);
            hstOutput.Add("intOrderNo", SqlDbType.Int);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            //HshIn.Add("bitPullForwardOrder", false);
            // HshIn.Add("bitPullForwardEMCodes", false);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEMROPOrder", HshIn, hstOutput);

                string i = "";
                if (hstOutput["intOrderNo"].ToString() != "")
                    i = hstOutput["intOrderNo"].ToString();
                string strMsg = hstOutput["chvErrorStatus"].ToString();
                sOrderID = i.ToString();
                return strMsg;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public String SaveOPOrder(int iRegistrationId, int iOPEnounterID, int iDoctorId, Int16 iHospitalLocationId, string sOrderDate, string sServices,
            int iEncodedBy, bool iAllergyReviewed, bool iIsPreg, bool iIsBreastFeed, int CompanyId, int iFacilityId, Int32 iPageId, bool iPullOrder,
            out string sOrderID, int iTemplateID, string sIsShowNote)
        {
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            HshIn.Add("intHospitalLocationId", iHospitalLocationId);
            HshIn.Add("dtsOrderDate", sOrderDate);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intCompanyID", CompanyId);
            if (iTemplateID != 0)
            {
                HshIn.Add("intTemplateID", iTemplateID);
            }
            HshIn.Add("xmlServices", sServices);
            HshIn.Add("intEncodedBy", iEncodedBy);
            hstOutput.Add("intOrderNo", SqlDbType.VarChar);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            hstOutput.Add("intNEncounterID", SqlDbType.Int);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, hstOutput);

                string i = "";
                if (hstOutput["intOrderNo"].ToString() != "")
                    i = hstOutput["intOrderNo"].ToString();
                string strMsg = hstOutput["chvErrorStatus"].ToString();
                sOrderID = i.ToString();
                return strMsg;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sXMLPatientAlert, string sRemark,
            Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
            int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, bool bAllergyReviewed,
            int IsERorEMRServices, int RequestId, string xmlTemplateDetails, int EntrySite)
        {
            return saveOrders(iHospitalLocationId, iFacilityId, iRegistrationId, iEncounterId, sStrXML, sXMLPatientAlert, sRemark,
               iEncodedBy, iDoctorId, iCompanyId, sOrderType, cPayerType, sPatientOPIP,
               iInsuranceId, iCardId, dtOrderDate, sChargeCalculationRequired, bAllergyReviewed,
               IsERorEMRServices, RequestId, xmlTemplateDetails, EntrySite, false, false, "");
        }
        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sXMLPatientAlert, string sRemark,
            Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
            int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, bool bAllergyReviewed,
            int IsERorEMRServices, int RequestId, string xmlTemplateDetails, int EntrySite, bool IsApprovalRequired, bool IsReadBack, string ReadBackNote)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("dtsOrderDate", dtOrderDate);
            HshIn.Add("inyOrderType", sOrderType);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEnounterId", iEncounterId);
            HshIn.Add("xmlServices", sStrXML);
            HshIn.Add("xmlPatientAlerts", sXMLPatientAlert);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("chvRemarks", sRemark);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intCompanyId", iCompanyId);
            HshIn.Add("cPayerType", cPayerType);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", sPatientOPIP);
            HshIn.Add("chrChargeCalculationRequired", sChargeCalculationRequired);
            HshIn.Add("bitAllergyReviewed", bAllergyReviewed);
            HshIn.Add("iIsERorEMRServices", IsERorEMRServices);
            HshIn.Add("intRequestId", RequestId);
            HshIn.Add("xmlTemplateDetails", xmlTemplateDetails);
            HshIn.Add("intEntrySite", EntrySite);


            HshIn.Add("@bitIsApprovalRequired", IsApprovalRequired);

            HshIn.Add("@bitIsReadBack", IsReadBack);
            HshIn.Add("@chvReadBackNote", ReadBackNote);


            HshOut.Add("intOrderNo", SqlDbType.VarChar);
            HshOut.Add("intOrderId", SqlDbType.Int);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("intNEncounterID", SqlDbType.Int);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        //Not in Use
        //public DataSet GetICDCategory()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDr = objDl.FillDataSet(CommandType.Text, "select CategoryId, CategoryDescription from MRDICDCategory where Active = 1");
        //    return objDr;
        //}
        //Not in Use
        //public DataSet GetICDCodes(Int16 iCategoryId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("CategoryId", iCategoryId);
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, "select ICDID, ICDCode, Description from MRDICDDisease where Active = 1 and CategoryId = @CategoryId", HshIn);
        //    return objDs;
        //}
        //Not in Use
        //public DataSet GetICDCodes(string sICDCode)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("ICDCode", sICDCode);
        //    string sKeyword = "EXEC ('select ICDID, ICDCode, Description from EMRICD10 where Active = 1 and ICDCode LIKE ''%'+@ICDCode+'%''')";
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, sKeyword, HshIn);
        //    return objDs;
        //}
        public SqlDataReader DefaultTableValues(Int16 iHospitalLocationID, Int32 iDoctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intDoctorID", iDoctorID);

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspEMRGetColorLegend", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader DefaultTableValues(Int16 iHospitalLocationID, Int32 iDoctorID, String sFieldID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intDoctorID", iDoctorID);
                HshIn.Add("@chvFieldID", sFieldID);

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspEMRGetColorLegend", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Not in Use
        //public SqlDataReader populateLegend()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "Select FieldID,FieldDescription from EMRDoctorDefaultOptions");
        //    return dr;
        //}
        //Not in Use
        //public SqlDataReader SaveColorLegendConfirmation(Int16 iHospitalLocationId, Int32 iDoctorId, Int16 iFieldID)
        //{
        //    HshIn = new Hashtable();
        //    Hashtable hstOutput = new Hashtable();
        //    HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("intDoctorId", iDoctorId);
        //    HshIn.Add("inyFieldID", iFieldID);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select FieldID from EMRDoctorDefaultValue where HospitalLocationID=@inyHospitalLocationID and DoctorID=@intDoctorId and FieldID=@inyFieldID", HshIn, hstOutput);
        //    return dr;
        //}
        //Not in Use
        //public String SaveColorLegend(Int16 iHospitalLocationId, Int32 iDoctorId, Int32 iUserID, Int16 iFieldID, string sFieldValue)
        //{
        //    HshIn = new Hashtable();
        //    Hashtable hstOutput = new Hashtable();
        //    HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("intDoctorId", iDoctorId);
        //    HshIn.Add("inyFieldID", iFieldID);
        //    HshIn.Add("chvFieldValue", sFieldValue);
        //    HshIn.Add("intEncodedBy", iUserID);
        //    hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveUpdateColorLegends", HshIn, hstOutput);
        //    return hstOutput["chvErrorStatus"].ToString();
        //}

        public SqlDataReader populateInvestigationSetMain(Int16 iHospitalLocationId, Int32 iDoctorID)
        {
            HshIn = new Hashtable();
            HshIn.Add("@HospitalLocationID", iHospitalLocationId);
            HshIn.Add("@DoctorId", iDoctorID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select SetID, SetName from EMRInvestigationSetMain WITH (NOLOCK) where Active=1 and HospitalLocationID=@HospitalLocationID and isnull(IsWardOrderSet,0)=0", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public SqlDataReader populateInvestigationSetMain(Int16 iHospitalLocationId, Int32 iDoctorID, int DepartmentId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@DoctorId", iDoctorID);
            HshIn.Add("@intDepartmentId", DepartmentId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT SetID,SetName FROM EMRInvestigationSetMain WITH (NOLOCK) WHERE Active=1 AND (DepartmentId IS NULL OR DepartmentId=@intDepartmentId) AND HospitalLocationID=@inyHospitalLocationId and isnull(IsWardOrderSet,0)=0", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //change palendra EMR Orderset Filter
        public SqlDataReader EMRpopulateInvestigationSetMain(Int16 iHospitalLocationId, Int32 iDoctorID, string chvServiceName)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intDoctorId", iDoctorID);
            HshIn.Add("@chvServiceName", chvServiceName);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "uspEMRGetWardOrderSet", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //change palendra EMR Orderset Filter
        //Not in Use
        //public DataSet populateInvestigationSetGrid(Int16 iHospitalLocationId, Int32 iSetID)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("@intSetId", iSetID);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetInvestigationSetDetails", HshIn);
        //    
        //}
        //Not in Use
        //public DataSet populateInvestigationSetGrid(Int16 iHospitalLocationId, Int32 iSetID, Int32 iSubDeptID)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("@intSetId", iSetID);
        //    HshIn.Add("@intSubDeptID", iSubDeptID);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetInvestigationSetDetails", HshIn);
        //    
        //}
        //Not in Use
        //public DataSet populatePreviousOrderGrid(Int16 iHospitalLocationId, Int32 iRegNo, Int32 iDoctorID, Int16 iVisitNo, String FDate, String TDate)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("@intRegistrationNo", iRegNo);
        //    HshIn.Add("@intDoctorId", iDoctorID);
        //    HshIn.Add("@inyLastVisits", iVisitNo);
        //    HshIn.Add("@chrDateFrom", FDate);
        //    HshIn.Add("@chrDateTo", TDate);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousInvestigations", HshIn);
        //    
        //}
        //Not in Use
        //public DataSet populatePreviousOrderGrid(Int16 iHospitalLocationId, Int32 iRegNo, Int32 iDoctorID, Int16 iVisitNo, String sFDate, String sTDate, Int32 iEncNo, string DepartMentId)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("@intRegistrationNo", iRegNo);
        //    HshIn.Add("@intDoctorId", iDoctorID);
        //    HshIn.Add("@inyLastVisits", iVisitNo);
        //    HshIn.Add("@chrDateFrom", sFDate);
        //    HshIn.Add("@chrDateTo", sTDate);
        //    HshIn.Add("@intEncounterNo", iEncNo);
        //    HshIn.Add("@intSubDeptID", DepartMentId);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousInvestigations", HshIn);
        //    
        //}
        //Not in Use
        public DataSet populatePreviousOrderGrid(Int16 iHospitalLocationId, Int64 iRegNo, Int32 iDoctorID, Int16 iVisitNo, String sFDate, String sTDate, Int32 iEncNo, Int32 iSubDeptID, String ServiceName)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intRegistrationNo", iRegNo);
            HshIn.Add("@intDoctorId", iDoctorID);
            HshIn.Add("@ServiceName", ServiceName);
            HshIn.Add("@inyLastVisits", iVisitNo);
            HshIn.Add("@chrDateFrom", sFDate);
            HshIn.Add("@chrDateTo", sTDate);
            HshIn.Add("@intEncounterNo", iEncNo);
            HshIn.Add("@intSubDeptID", iSubDeptID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousInvestigations", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet populateFutureOrderGrid(Int16 iHospitalLocationId, Int32 iDoctorID, String sFDate, String sTDate, Int64 iRegNo, Int32 iEncNo, string DepartMentId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intDoctorId", iDoctorID);
            HshIn.Add("@chrDateFrom", sFDate);
            HshIn.Add("@chrDateTo", sTDate);
            HshIn.Add("@intRegistrationNo", iRegNo);
            HshIn.Add("@intEncounterId", iEncNo);
            HshIn.Add("@intSubDeptID", DepartMentId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFutureInvestigations", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet populateFutureOrderGrid(Int16 iHospitalLocationId, String sFDate, String sTDate, Int16 iSearchType, String sSearchValue)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@chrDateFrom", sFDate);
            HshIn.Add("@chrDateTo", sTDate);
            HshIn.Add("@inySearchType", iSearchType);
            HshIn.Add("@chvSearchValue", sSearchValue);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFutureInvestigationsReminderMain", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet populatePreviousOrderGrid(Int32 EncId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@EncID", EncId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousInvestigationsIP", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string DeActivateOrderServices(int SodaId, int ServiceId, int OrderId, int ServiceOrderId,
            string Remarks, int HospId, int FacilityId, int EncodedBy)
        {
            return DeActivateOrderServices(SodaId, ServiceId, OrderId, ServiceOrderId,
              Remarks, HospId, FacilityId, EncodedBy, "");
        }
        public string DeActivateOrderServices(int SodaId, int ServiceId, int OrderId, int ServiceOrderId,
            string Remarks, int HospId, int FacilityId, int EncodedBy, string ServiceCancelReason)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intServiceOrderDtlAmtId", SodaId);
            HshIn.Add("@intServiceId", ServiceId);
            HshIn.Add("@intOrderId", OrderId);
            HshIn.Add("@intServiceOrderId", ServiceOrderId);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@ServiceCancelReason", ServiceCancelReason);
            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelOrderServices", HshIn, hstOutput);

                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Not in Use
        //public DataSet GetFavourites(Int16 iHospitalLocationId, Int32 iDoctorID)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("@intDoctorId", iDoctorID);
        //    StringBuilder strSQL = new StringBuilder();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteInvestigations", HshIn);
        //    return objDs;
        //}
        //Not in Use
        //public DataSet GetFavouritesWithSubDept(Int16 iHospitalLocationId, Int32 iDoctorID, Int32 iSubDeptID)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
        //    HshIn.Add("@intDoctorId", iDoctorID);
        //    HshIn.Add("@intSubDeptID", iSubDeptID);
        //    StringBuilder strSQL = new StringBuilder();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteInvestigations", HshIn);
        //    return objDs;
        //}
        //Not in Use
        //public DataSet GetFavourites(String strName, Int16 iHospitalLocationId, Int32 iDoctorID)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@HospitalLocationId", iHospitalLocationId);
        //    HshIn.Add("@DoctorID", iDoctorID);
        //    HshIn.Add("@Name", strName + "%");
        //    StringBuilder strSQL = new StringBuilder();
        //    strSQL.Append(" Select efi.serviceID,efi.DoctorID,ios.ServiceName,");
        //    strSQL.Append(" row_number() over(order by ios.servicename)as SerialNo,");
        //    strSQL.Append(" ISNULL(inf.SpecialPrecaution,'') as SpecialPrecaution ");
        //    strSQL.Append(" from EMRFavouriteInvestigations efi");
        //    strSQL.Append(" Inner Join ItemOfService ios On efi.serviceid = ios.ServiceId And ios.Active = 1");
        //    strSQL.Append(" Left Join InvestigationFlags inf On efi.serviceid = inf.serviceid");
        //    strSQL.Append(" where IOS.HospitalLocationId=@HospitalLocationId And efi.Active = 1 and efi.DoctorID=@DoctorID and IOS.ServiceName like @Name order by ios.servicename");
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = dl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
        //    return objDs;
        //}

        public DataSet populateOrderDetails(Int32 iORderID)
        {
            HshIn = new Hashtable();
            HshIn.Add("intOrderId", iORderID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetOrderDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getCurrentICDCodes(Int32 iRegNo, Int32 iDocID, Int16 iHospID, Int32 iEncID)
        {
            HshIn = new Hashtable();
            HshIn.Add("intRegistrationId", iRegNo);
            HshIn.Add("intDoctorId", iDocID);
            HshIn.Add("inyHospitalLocationID", iHospID);
            //HshIn.Add("intEncounterId", iEncID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetAllPrevAssessment", HshIn);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetUnResolvedAssessment", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //Not in Use
        //public SqlDataReader getAllergyReviewed(Int32 iRegNo, Int16 iHospID, Int32 iEncID)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@RegNo", iRegNo);
        //    HshIn.Add("@HospID", iHospID);
        //    HshIn.Add("@EncID", iEncID);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select convert(tinyint,AllergyReviewed) from encounter where RegistrationNo=@RegNo and HospitalLocationId=@HospID and Id=@EncID", HshIn);
        //    return dr;
        //}


        public DataSet GetPendingOrderForBill(int iRegID, int iHospitalID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            StringBuilder StrQuery = new StringBuilder();

            HshIn.Add("@RegId", iRegID);
            HshIn.Add("@HospitalLocationId", iHospitalID);

            StrQuery.Append("SELECT DISTINCT ENC.encounterNo AS EncounterNo, ENC.Id AS EncounterID, OSD.orderId, ");
            StrQuery.Append(" OSD.orderId AS Orderno, OSD.OrderDate, OSD.ServiceId, IOS.ServiceName, dbo.GetDoctorName(OSD.DoctorId) AS DoctorName, ");
            StrQuery.Append(" CASE WHEN OSD.BillID > 0 THEN 1 ELSE 0 END AS ISBillable, OSD.Active ");
            StrQuery.Append(" FROM encounter ENC WITH (NOLOCK) ");
            StrQuery.Append(" INNER JOIN OPServiceOrderDetail OSD WITH (NOLOCK) ON OSD.EncounterId = ENC.Id ");
            StrQuery.Append(" INNER JOIN ItemOfService IOS WITH (NOLOCK) ON IOS.ServiceId=OSD.ServiceId ");
            StrQuery.Append(" INNER JOIN Employee EMP WITH (NOLOCK) ON EMP.ID=OSD.DoctorId ");
            StrQuery.Append(" WHERE ENC.RegistrationId = @RegId AND ENC.active = 1 and OSD.Active=1 AND ENC.hospitallocationid = @HospitalLocationId ");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, StrQuery.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string[] BindCharges(int iRegID, int iHospitalID, int iServiceId, int iCompanyId, int iFacilityId, string sModifier)
        {
            string[] charge = new string[5];

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            HshIn.Add("@intRegistrationID", iRegID);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@inyHospitalLocationID", iHospitalID);
            HshIn.Add("@intCompanyid", iCompanyId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intSpecialisationId", Convert.ToInt32("0"));
            HshIn.Add("@intDoctorId", Convert.ToInt32("0"));
            HshIn.Add("@chvModifierCode", sModifier);
            hshOut.Add("@NChr", SqlDbType.Money);
            hshOut.Add("@DNchr", SqlDbType.Money);
            hshOut.Add("@DiscountNAmt", SqlDbType.Money);
            hshOut.Add("@DiscountDNAmt", SqlDbType.Money);
            hshOut.Add("@PatientNPayable", SqlDbType.Money);
            hshOut.Add("@PayorNPayable", SqlDbType.Money);
            hshOut.Add("@DiscountPerc", SqlDbType.Money);
            hshOut.Add("@IsApproval", SqlDbType.Bit);
            hshOut.Add("@IsExcluded", SqlDbType.Bit);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, hshOut);
                charge[0] = hshOut["@NChr"].ToString(); // Service Amt
                charge[1] = hshOut["@DNchr"].ToString(); // Doctor Amt
                charge[2] = hshOut["@DiscountNAmt"].ToString(); // Service Discount Amt
                charge[3] = hshOut["@DiscountDNAmt"].ToString(); // Doctor Discont Amt
                charge[4] = hshOut["@DiscountPerc"].ToString();

                return charge;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientDiagnosis(int iRegID, int iHospitalID, int iEncounterId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalID);
            HshIn.Add("@intRegistrationId", iRegID);//
            HshIn.Add("@intEncounterId", iEncounterId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetICDCode(string sIcdCode)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string sql = "SELECT ICDID, ICDCode, Description FROM ICD9SubDisease WITH (NOLOCK) Where ICDCode = @Diagnosis and Active = 1 ";
                HshIn = new Hashtable();
                HshIn.Add("@Diagnosis", sIcdCode);
                return objDl.FillDataSet(CommandType.Text, sql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public SqlDataReader GetSelectedService(int iHospitalID, int iServiceId, int iDepartmentId, int FacilityId)
        {
            SqlDataReader sqlRead;
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@chvSearchCriteria", "%%");
            HshIn.Add("@intId", iServiceId);
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intDepartmentId", iDepartmentId);
            HshIn.Add("@intExternalCenterId", null);
            HshIn.Add("intFacilityId", FacilityId);
            try
            {
                return sqlRead = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetModifier()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string sQUery = "SELECT ModifierCode,ModifierCode + '(' + Description + ')' AS Description FROM ModifierList WITH (NOLOCK) ";
                return objDl.FillDataSet(CommandType.Text, sQUery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public Hashtable GetPatientCompanyId(int iHospitalID, int iEncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            try
            {
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@inyHospitalLocationId", iHospitalID);
                hshOutput.Add("@intCompanyId", SqlDbType.Int);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "GetPatientCompanyCode", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveFavorite(int iDoctorId, int iServiceId, int iEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            string sResut = "";
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            string sqlstr = "SELECT ServiceID FROM EMRFavouriteInvestigations WITH (NOLOCK) WHERE DoctorID=@intDoctorId AND ServiceID=@intServiceId AND Active=1 ";
            ds = objDl.FillDataSet(CommandType.Text, sqlstr, HshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sResut = "Service Already Exist in Favorites";
            }
            else
            {
                if (objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveFavService", HshIn) == 0)
                {
                    sResut = "Service Saved to Favorites";
                }
            }
            return sResut;
        }

        public string SaveFavorite(int iDoctorId, int iServiceId, int iEncodedBy, int iServiceDurationId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            string sResut = "";
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@intServiceDurationId", iServiceDurationId);

            string sqlstr = "SELECT ServiceID FROM EMRFavouriteInvestigations WITH (NOLOCK) WHERE DoctorID=@intDoctorId AND ServiceID=@intServiceId AND Active=1 ";

            try
            {


                ds = objDl.FillDataSet(CommandType.Text, sqlstr, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sResut = "Service Already Exist in Favorites";
                }
                else
                {
                    if (objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRSaveFavService", HshIn) == 0)
                    {
                        sResut = "Service Saved to Favorites";
                    }
                }
                return sResut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
        public string DeleteFavorite(int iDoctorId, int iServiceId, int iEncodedBy)
        {
            string sResult = "";
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            if (objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeleteFavService", HshIn) == 0)
            {
                sResult = "Service Deleted from Favourtes successfull";
            }
            return sResult;
        }
        //public DataTable GetFavorites(int iDoctorId, int iDepartmentId, int FacilityId)
        //{
        //    
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable HshIn = new Hashtable();
        //    HshIn.Add("@intDoctorId", iDoctorId);
        //    HshIn.Add("@intDepartmentId", iDepartmentId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@chvSearchCriteria", "%%");

        //    HshIn.Add("@intExternalCenterId", null);

        //    return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteServices", HshIn);
        //    
        //}


        public DataSet GetFavorites(int iDoctorId, int iDepartmentId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intDepartmentId", iDepartmentId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvSearchCriteria", "%%");

            HshIn.Add("@intExternalCenterId", null);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetSearchServices(int iHospitalID, int iDepartmentId, string sSubDeptId, string sSearchText, int FacilityId, int SecGroupId, int EmployeeId)
        {
            return GetSearchServices(iHospitalID, iDepartmentId, sSubDeptId, sSearchText, FacilityId, SecGroupId, EmployeeId, 0);
        }
        public DataTable GetSearchServices(int iHospitalID, int iDepartmentId, string sSubDeptId, string sSearchText, int FacilityId, int SecGroupId, int EmployeeId, int ServicesForWard)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@chvSearchCriteria", sSearchText == "" ? "%%" : sSearchText);
            string storeProcedurnme = string.Empty;
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalID);
                HshIn.Add("@intDepartmentId", iDepartmentId);
                HshIn.Add("@chvSubDepartmentIds", sSubDeptId);

                HshIn.Add("@intExternalCenterId", null);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intServicesForWard", ServicesForWard);
                if (SecGroupId > 0)
                {
                    HshIn.Add("@intSecGroupId", SecGroupId);
                }
                if (EmployeeId > 0)
                {
                    HshIn.Add("@intEmployeeId", EmployeeId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //yogesh  28/09/2022
        public DataTable GetSearchServicesByEntry(int iHospitalID, int iDepartmentId, string sSubDeptId, string sSearchText, int FacilityId, int SecGroupId, int EmployeeId, int? EntrySite)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@chvSearchCriteria", sSearchText == "" ? "%%" : sSearchText);
            string storeProcedurnme = string.Empty;
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalID);
                HshIn.Add("@intDepartmentId", iDepartmentId);
                HshIn.Add("@chvSubDepartmentIds", sSubDeptId);

                HshIn.Add("@intExternalCenterId", null);
                HshIn.Add("@intFacilityId", FacilityId);
               
                if (SecGroupId > 0)
                {
                    HshIn.Add("@intSecGroupId", SecGroupId);
                }
                if (EmployeeId > 0)
                {
                    HshIn.Add("@intEmployeeId", EmployeeId);
                    if (EntrySite > 0)
                        HshIn.Add("@iEntrySiteId", EntrySite);
                    else
                        HshIn.Add("@iEntrySiteId", 0);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataTable GetEMRSearchServices(int HospitalLocationId, int EncodedBy)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetHospitalServices", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetOrderSets(int iSetId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intSetId", iSetId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetOrderSetDetails", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientServices(int iHospitalID, int iRegistrationId, string sEncounterId, int iServiceId, int iOrderId, bool bHistory)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intEncounterId", sEncounterId);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@intOrderId", iOrderId);
            HshIn.Add("@bitHistory", bHistory);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string DeletePatientServices(int iServiceId, int iHospitalID, int iRegistrationId, int iEncounterId,
            int iFacilityId, bool bCancelService, int iPageId, int iEncodedby, int iServiceOrderDtlId, bool bRequestToDepartment)
        {
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            HshIn.Add("@intServiceId", iServiceId);
            HshIn.Add("@iServiceOrderDtlId", iServiceOrderDtlId);
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intLoginFacilityId", iFacilityId);
            HshIn.Add("@intPageId", iPageId);
            HshIn.Add("@intRegistrationId", iRegistrationId);//
            HshIn.Add("@intEncounterId", iEncounterId);
            HshIn.Add("@bitCancelService", bCancelService);
            HshIn.Add("@intEncodedBy", iEncodedby);
            HshIn.Add("@bitRequestToDepartment", bRequestToDepartment);
            hstOutput.Add("@chvErrorOutput", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRDeletePatientOrders", HshIn, hstOutput);
                return hstOutput["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //public string DeletePatientServices(int iServiceId, int iHospitalID, int iRegistrationId, int iEncounterId,
        //    int iFacilityId, bool bCancelService, int iPageId, int iEncodedby, int iServiceOrderDtlId)
        //{
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable hstOutput = new Hashtable();
        //    HshIn.Add("@intServiceId", iServiceId);
        //    HshIn.Add("@iServiceOrderDtlId", iServiceOrderDtlId);
        //    HshIn.Add("@inyHospitalLocationId", iHospitalID);
        //    HshIn.Add("@intLoginFacilityId", iFacilityId);
        //    HshIn.Add("@intPageId", iPageId);
        //    HshIn.Add("@intRegistrationId", iRegistrationId);//
        //    HshIn.Add("@intEncounterId", iEncounterId);
        //    HshIn.Add("@bitCancelService", bCancelService);
        //    HshIn.Add("@intEncodedBy", iEncodedby);
        //   // HshIn.Add("@bitRequestToDepartment", bRequestToDepartment);
        //    hstOutput.Add("@chvErrorOutput", SqlDbType.VarChar);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hstOutput = dal.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRDeletePatientOrders", HshIn, hstOutput);
        //    return hstOutput["@chvErrorOutput"].ToString();
        //}
        public bool GetPatientExistServices(int iHospitalID, int iEncounterId, int iServiceId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intEncounterId", iEncounterId);
            HshIn.Add("@intServiceId", iServiceId);
            try
            {

                string sQuery = "SELECT sm.Id,sd.ServiceId  FROM  ServiceOrderMain sm WITH (NOLOCK) INNER JOIN  ServiceOrderDetail sd WITH (NOLOCK) ON sm.Id=sd.OrderId " +
                                " WHERE sm.EncounterId=@intEncounterId AND sd.ServiceId=@intServiceId AND sd.Active=1 AND sm.Active=1 AND sm.HospitalLocationId=@inyHospitalLocationId ";
                ds = objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public DataSet GetEncounterCompany(int iEncounterId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intEncounterId", iEncounterId);
            // ashma 
            string sQuery = "SELECT DoctorId, CompanyCode,InsuranceCode,CardId ,opip FROM Encounter WITH (NOLOCK) WHERE Id=@intEncounterId";

            try
            {
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetEncounterDoctors(int iHospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);

            try
            {

                string sQuery = " SELECT DISTINCT EM.ID AS DoctorId, EM.FirstName + ISNULL(' ' + EM.MiddleName,'') + ISNULL(' ' + EM.LastName,'') AS DoctorName " +
                          " FROM ServiceOrderDetail sod WITH (NOLOCK) INNER JOIN Employee EM WITH (NOLOCK) ON sod.DoctorId=EM.ID " +
                          " WHERE EM.HospitalLocationId=@inyHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public Hashtable AddServiceToday(int iHospitalLocationId, int iRegistrationId, int iDoctorId, int iEncounterId,
            int iEncodedBy, string sIds)
        {
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEncounterID", iEncounterId);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("xmlServiceIds", sIds);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("intHospitalLocationId", iHospitalLocationId);

            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEMROPOrderHistory", HshIn, hstOutput);
                return hstOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientOrderMainHistory(int iHospitalID, int iRegistrationId, int iEncounterId, int iDoctorId, int iFacilityId,
            string sFilter, string sFromDate, string sToDate, string sVisitType, int iLoginFacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intRegistrationId ", iRegistrationId);
            HshIn.Add("@intEncounterId ", iEncounterId);
            HshIn.Add("@intDoctorID", iDoctorId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvVisitType", sVisitType);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@chvDateRange", sFilter);

            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientOrderMainHistory", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientPreAuthorizeOrder(int iHospitalID, string sRegistrationNo, string sEncounterNo, string sOrderNo, string sPatientName, int iDoctorId, int iFacilityId,
            string sFilter, string sFromDate, string sToDate, string sPreAuth)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@chvRegistrationNo ", sRegistrationNo);
            HshIn.Add("@chvEncounterNo ", sEncounterNo);
            HshIn.Add("@chvOrderNo", sOrderNo);
            HshIn.Add("@intDoctorID", iDoctorId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvPreAuthorized", sPreAuth);
            HshIn.Add("@chvPatientName", sPatientName);

            if (sFilter == "TD")
            {
                HshIn.Add("@chrFromDate", DateTime.Today.ToString("yyyy/MM/dd"));
                HshIn.Add("@chrToDate", DateTime.Today.ToString("yyyy/MM/dd"));
            }
            else if (sFilter == "DR")
            {
                HshIn.Add("@chrFromDate", sFromDate);
                HshIn.Add("@chrToDate", sToDate);
            }
            else if (sFilter == "LY")
            {
                DateTime fromdate = DateTime.Today.AddYears(-1);
                HshIn.Add("@chrFromDate", fromdate.ToString("yyyy/MM/dd") + " 00:00");
                HshIn.Add("@chrToDate", DateTime.Today.ToString("yyyy/MM/dd") + " 23:59");
            }
            else if (sFilter == "LSM")
            {
                DateTime fromdate = DateTime.Today.AddMonths(-6);
                HshIn.Add("@chrFromDate", fromdate.ToString("yyyy/MM/dd") + " 00:00");
                HshIn.Add("@chrToDate", DateTime.Today.ToString("yyyy/MM/dd") + " 23:59");
            }
            else if (sFilter == "LM")
            {
                DateTime fromdate = DateTime.Today.AddMonths(-1);
                HshIn.Add("@chrFromDate", fromdate.ToString("yyyy/MM/dd") + " 00:00");
                HshIn.Add("@chrToDate", DateTime.Today.ToString("yyyy/MM/dd") + " 23:59");
            }
            else if (sFilter == "LW")
            {
                DateTime fromdate = DateTime.Today.AddDays(-7);
                HshIn.Add("@chrFromDate", fromdate.ToString("yyyy/MM/dd") + " 00:00");
                HshIn.Add("@chrToDate", DateTime.Today.ToString("yyyy/MM/dd") + " 23:59");
            }

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientPreAuthorizeOrder", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientGender(int iHospitalID, int iRegistrationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intRegistrationId", iRegistrationId);
            string sQuery = "SELECT  case Gender when 1 then 'F' when 2 then 'M' else 'O' end as Gender FROM Registration WITH (NOLOCK) WHERE ID=@intRegistrationId AND HospitalLocationId=@inyHospitalLocationId ";

            try
            {

                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveServicePreauthorizedNo(Int32 iHospitalLocationId, Int32 iRegistrationId, Int32 iEncounterId, int iOrderId, string sStrXML, Int32 iEncodedBy, Int32 iDoctorId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEnounterId", iEncounterId);
            HshIn.Add("xmlServices", sStrXML);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intOrderId", iOrderId);
            HshOut.Add("chvOrderNo", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateServicePreauthorized", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientPreAuthorizeOrderDetails(int iHospitalLocationId, int iRegistrationId, int iEncounterId, int iOrderId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intRegistrationId ", iRegistrationId);
            HshIn.Add("@intEncounterId ", iEncounterId);
            HshIn.Add("@intOrderId", iOrderId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientPreAuthorizedServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetServiceCategories(int iHospitalLocationId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetServiceCategories");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetInvestigationSetDetail(int iSetID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("SetId", iSetID);

            try
            {


                return objDl.FillDataSet(System.Data.CommandType.Text, "select isd.SetId as Id,isd.ServiceID, ios.ServiceName FROM EMRinvestigationSetDetails isd WITH (NOLOCK) INNER JOIN itemofservice ios WITH (NOLOCK) ON isd.ServiceID = ios.ServiceID WHERE isd.SetID = @SetId and isd.Active=1 ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveOrderSetName(int SetId, int iHospitalLocationId, string sOrderSetName, string DepartmentId, int iEncodedBy, int DoctorId, out int SavedSetId, int IsWardOrderSet)
        {
            HshIn = new Hashtable();
            Hashtable output = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intSetId", SetId);
            HshIn.Add("@chvSetName", sOrderSetName);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            if (DepartmentId != "")
            {
                HshIn.Add("@intDepartmentId", DepartmentId);
            }
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@DoctorId", DoctorId);
            HshIn.Add("@IsWardOrderSet", IsWardOrderSet);


            output.Add("@intSavedSetId", SqlDbType.Int);
            output.Add("@chvStatus", SqlDbType.VarChar);
            try
            {


                output = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveUpdateOrderSet", HshIn, output);

                SavedSetId = (int)output["@intSavedSetId"];

                return output["@chvStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveMedicationSetName(int SetId, int iHospitalLocationId, string sOrderSetName, string DepartmentId, int iEncodedBy,
                                            int DoctorId, out int SavedSetId)
        {
            HshIn = new Hashtable();
            Hashtable output = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intSetId", SetId);
            HshIn.Add("@chvSetName", sOrderSetName);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            if (DepartmentId != "")
            {
                HshIn.Add("@intDepartmentId", DepartmentId);
            }
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@DoctorId", DoctorId);

            output.Add("@intSavedSetId", SqlDbType.Int);
            output.Add("@chvStatus", SqlDbType.VarChar);
            try
            {
                output = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveUpdateMedicationSet", HshIn, output);

                SavedSetId = (int)output["@intSavedSetId"];
                return output["@chvStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteOrderSetService(int iId)
        {
            string sOutPut = "";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", iId);
                string strDelQuery = " Update EMRInvestigationSetDetails Set Active=0 where ServiceID=@ID";
                int i = objDl.ExecuteNonQuery(CommandType.Text, strDelQuery, HshIn);
                if (i == 0)
                {
                    sOutPut = "Record(s) Has Been Saved...";
                }
                return sOutPut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveOrderSetDetails(int iOrderSetId, string xmlOrderSetDetials)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOutPut = new Hashtable();
            hshOutPut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@intSetID", iOrderSetId);
            HshIn.Add("@XMLSetDetails", xmlOrderSetDetials);

            try
            {

                hshOutPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveOrderSet", HshIn, hshOutPut);
                return hshOutPut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetExcludedServices(int iHospitalLocationId, int RegistrationId, int EncounterId, int iServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intServiceId", iServiceId);

                string sQuery = " DECLARE @intPlanTypeId INT,@bitExcludedService BIT SELECT @intPlanTypeId=PlanTypeId " +
                                " FROM Encounter WITH (NOLOCK) " +
                                " WHERE Id=@intEncounterId  AND RegistrationId=@intRegistrationId AND HospitalLocationId=@inyHospitalLocationId " +
                                " IF EXISTS(SELECT ID FROM AccountPlanDetails WITH (NOLOCK) WHERE PlanId= @intPlanTypeId AND ServiceId =@intServiceId AND Active=1) " +
                                " BEGIN SET @bitExcludedService=0 END ELSE " +
                                " BEGIN SET @bitExcludedService=1 END " +
                                " SELECT @bitExcludedService AS ExcludedService ";

                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public bool CheckExcludedServices(int iHospitalLocationId, int RegistrationId, int EncounterId)
        {
            bool IsExcluded = false;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                string sQuery = " SELECT CASE WHEN Flag='FFS' OR FLAG='PRV' THEN 0 ELSE 1 END AS ExcludedService FROM Encounter en WITH (NOLOCK) INNER JOIN  AccountTypes at WITH (NOLOCK) ON EN.AccountTypeId=AT.TypeId  INNER JOIN CompanyType CT WITH (NOLOCK) ON AT.AccountCategoryID=CT.Id " +
                                " WHERE en.ID=@intEncounterId  AND RegistrationId=@intRegistrationId AND HospitalLocationId=@inyHospitalLocationId";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    IsExcluded = Convert.ToBoolean(dr["ExcludedService"]);
                }
                dr.Close();
                return IsExcluded;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CheckRequestToDepartment(int ServiceId)
        {
            string SRequested = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intServiceId", ServiceId);
                string sQuery = " SELECT est.SubDeptID FROM ItemOfService ios WITH (NOLOCK) INNER JOIN (SELECT DISTINCT SubDeptId FROM EMRTemplateServiceTagging WITH (NOLOCK) WHERE SendRequestToDepartment = 1 AND Active = 1) est ON est.SubDeptId = ios.SubDeptId " +
                                " where ios.ServiceId =@intServiceId and ios.Active=1";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQuery, HshIn);
                if (dr.HasRows)
                {
                    dr.Read();
                    SRequested = Convert.ToString(dr["SubDeptID"]);
                }
                dr.Close();
                return SRequested;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetRequestService(int iHospitalLocationId, int iFacilityId, string EncounterNo, int iDepartmentId,
            int iSubDeptId, string cFromDate, string cToDate, string Source, string ProcedureType, string PatientName, string RegistrationNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@iDepartmentId", iDepartmentId);
                HshIn.Add("@iSubDeptId", iSubDeptId);
                if (cFromDate != "")
                    HshIn.Add("@dtFromDate", cFromDate);

                if (cToDate != "")
                    HshIn.Add("@dtToDate", cToDate);

                HshIn.Add("@chvSource", Source);
                HshIn.Add("@chvAckStatus", ProcedureType);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@PatientName", PatientName);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetRequestFormDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Commented by rakesh start
        //public DataSet GetServiceOrderFurtherAck(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId, int iDepartmentId, int iSubDeptId, string cFromDate, string cToDate)
        //Commented by rakesh end
        //Added by rakesh start
        public DataSet GetServiceOrderFurtherAck(int iHospitalLocationId, int iFacilityId, int iRegistrationId
            , int iEncounterId, int iDepartmentId, int iSubDeptId, string cFromDate, string cToDate, int intServiceId
            , string Source, string ProcedureType, string OrderID, string PatientName, Int64 RegistrationNo, int SecGroupId)
        //Added by rakesh start
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iRegistrationId", iRegistrationId);
                HshIn.Add("@iEncounterId", iEncounterId);
                HshIn.Add("@iDepartmentId", iDepartmentId);
                HshIn.Add("@iSubDeptId", iSubDeptId);
                HshIn.Add("@cFromDate", cFromDate);
                HshIn.Add("@cToDate", cToDate);
                HshIn.Add("@intSecGroupId", SecGroupId);
                //Added by rakesh start
                HshIn.Add("@intServiceId", intServiceId);
                HshIn.Add("@chvSource", Source);
                HshIn.Add("@chvAckStatus", ProcedureType);

                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@OrderID", OrderID);
                HshIn.Add("@PatientName", PatientName);


                //Added by rakesh end
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetServiceOrderFurtherAck", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by rakesh start


        //Added on 19-04-2014 Start Naushad Ali
        public Hashtable UpdateProcedureAckOPIP(string Source, int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId
            , int OrderID, int OrderDetailID, int intServiceId, string Remarks, int Encodedby, int Status, int ProAclID, int DoctorId)
        {
            //uspUpdateProcedureAckOPIP


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOutPut = new Hashtable();
            HshIn.Add("@chvSource", Source);
            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@iRegistrationId", iRegistrationId);
            HshIn.Add("@iEncounterId", iEncounterId);
            HshIn.Add("@intOrderID", OrderID);
            HshIn.Add("@intOrderDetailID", OrderDetailID);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@Remarks", Remarks);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@AckStatus", Status);
            HshIn.Add("@intProAckID", ProAclID);
            HshIn.Add("@intDoctorId", DoctorId);
            hshOutPut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {

                hshOutPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateProcedureAckOPIP", HshIn, hshOutPut);
                return hshOutPut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet uspGetProcedureAckOPIP(
            string Source, int iHospitalLocationId, int iFacilityId, int iRegistrationId
            , int OrderID, int OrderDetailID, int intServiceId,
            int Status
            )
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chvSource", Source);
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iRegistrationId", iRegistrationId);
                HshIn.Add("@OrderID", OrderID);
                HshIn.Add("@intOrderDetailID", OrderDetailID);
                HshIn.Add("@intServiceId", intServiceId);

                HshIn.Add("@intStatus", Status);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetProcedureAckOPIP", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataTable GetAllSubDepartment()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string query = "SELECT DISTINCT dm.DepartmentId,DepartmentName,ds.SubName,ds.SubDeptId ,ds.Type ,dss.LabType, CASE WHEN est.SubDeptId IS NULL THEN 0 ELSE 1 END AS SendRequestToDepartment" +
                         " FROM DiagSampleReceivingStationDetails dsrs WITH (NOLOCK) " +
                         " Inner JOIN DiagSampleReceivingStation dss WITH (NOLOCK) ON dss.StationId=dsrs.StationId AND dss.Active = 1 " +
                         " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ds.subdeptId=dsrs.SubDeptId and ds.Active=1 AND TYPE NOT IN('R','VF','CL','RF','O')" +
                         " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON dm.DepartmentID=ds.DepartmentID " +
                         " LEFT JOIN (SELECT DISTINCT SubDeptId FROM EMRTemplateServiceTagging WITH (NOLOCK) WHERE SubDeptId IS NOT NULL AND SendRequestToDepartment = 1 AND Active = 1) est ON est.SubDeptId = ds.SubDeptId " +
                         " WHERE dsrs.Active=1 ";

                return objDl.FillDataSet(CommandType.Text, query).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable getDepartment_byGroupId(int groupID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@groupID", groupID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDepartmentbyGroupID", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Added on 19-04-2014 End

        public int IsTemplateRequiredForService(int Option, int ServiceID, int TemplateId)
        {
            int result = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@Option", Option);
                HshIn.Add("@ServiceID", ServiceID);
                HshIn.Add("@TemplateId", TemplateId);
                //SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "uspIsTemplateRequiredForService", HshIn); //cmd.ExecuteReader();

                result = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.StoredProcedure, "uspIsTemplateRequiredForService", HshIn));

                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int EMRCheckOrderTemplateDetails(int orderDetailId, int checkStatus)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hstOutput = new Hashtable();

                HshIn.Add("@intOrderDetailId", orderDetailId);

                hstOutput.Add("@inyCheckStatus", checkStatus);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCheckOrderTemplateDetails", HshIn, hstOutput);

                return Convert.ToInt32(hstOutput["@inyCheckStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by rakesh end

        public DataSet getItemOfService(string ServiceIds, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospId", HospId);

                string query = "SELECT ServiceId, ServiceName FROM ItemOfService WITH (NOLOCK) WHERE ServiceId IN (" + ServiceIds + ") AND Active = 1 AND HospitalLocationId = @intHospId";

                return objDl.FillDataSet(CommandType.Text, query, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getTemplateFields(int HospId, int SectionId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intCategoryID", SectionId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateFieldNames", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEMRTemplateFieldEmpTypesTagging(int FieldId, string xmlFieldEmpTypesIds, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@xmlFieldEmpTypesIds", xmlFieldEmpTypesIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateFieldEmpTypesTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEMRTemplateFieldEmpTypesTagging(int FieldId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intFieldId", FieldId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateFieldEmpTypesTagging", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEMRTemplateFieldsFormula(int SectionId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intSectionId", SectionId);

                string query = "SELECT FieldId, FieldName, ReferenceName, FormulaDefinition, " +
                                " (CASE WHEN ISNULL(TotalCalc, 0) = 0 THEN 0 ELSE 1 END) AS TotalCalc " +
                                " FROM EMRTemplateFields WITH (NOLOCK) " +
                                " WHERE SectionId = @intSectionId AND FieldType = 'T' AND Active = 1 AND ISNULL(ReferenceName,'') <> '' " +
                                " ORDER BY ReferenceName, FieldName";

                return objDl.FillDataSet(CommandType.Text, query, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveEMRTemplateFormulaFields(int SectionId, string xmlFormulaFields, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intSectionId", SectionId);
            HshIn.Add("@xmlFormulaFields", xmlFormulaFields);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateFormulaFields", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEMRTemplateFieldsScoreFormula(int SectionId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intSectionId", SectionId);

            string query = "SELECT FieldId, FieldName, ScoreReferenceName AS ReferenceName, ScoreFormulaDefinition AS FormulaDefinition " +
                            " FROM EMRTemplateFields WITH (NOLOCK) " +
                            " WHERE SectionId = @intSectionId " +
                            " AND FieldType IN ('R','T') AND Active = 1 AND ISNULL(ScoreReferenceName,'') <> '' " +
                            " ORDER BY ParentId, Hierarchy, SequenceNo";
            try
            {
                return objDl.FillDataSet(CommandType.Text, query, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveEMRTemplateScoreFormulaFields(int SectionId, string xmlFormulaFields, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intSectionId", SectionId);
            HshIn.Add("@xmlFormulaFields", xmlFormulaFields);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTemplateScoreFormulaFields", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetServiceDescriptionForOrderpage(int HospId, int intFacilityId, int intRegistrationId, int intEncounterId, int intServiceId,
         int intorderSetId, int intCompanyid, int intSponsorId, int intCardId, int Option, int intTemplateId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationID", HospId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intRegistrationId", intRegistrationId);
            HshIn.Add("@intEncounterId", intEncounterId);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@intorderSetId", intorderSetId);
            HshIn.Add("@intCompanyid", intCompanyid);
            HshIn.Add("@intSponsorId", intSponsorId);
            HshIn.Add("@intCardId", intCardId);
            HshIn.Add("@Option", Option);
            HshIn.Add("@intTemplateId", intTemplateId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceDescriptionForOrderpage", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetServiceDescriptionForOrderpage(int HospId, int intFacilityId, int intRegistrationId, int intEncounterId, int intServiceId,
         int intorderSetId, int intCompanyid, int intSponsorId, int intCardId, int Option, int intTemplateId, string XmlOrderSetId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationID", HospId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intRegistrationId", intRegistrationId);
            HshIn.Add("@intEncounterId", intEncounterId);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@intorderSetId", intorderSetId);
            HshIn.Add("@intCompanyid", intCompanyid);
            HshIn.Add("@intSponsorId", intSponsorId);
            HshIn.Add("@intCardId", intCardId);
            HshIn.Add("@Option", Option);
            HshIn.Add("@intTemplateId", intTemplateId);
            HshIn.Add("@intorderSetIdXML", XmlOrderSetId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceDescriptionForOrderpage", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet SetUpdate_ServiceOrderDetail(int id, int OrderId, int ServiceId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();

            hsIn.Add("@id", id);
            hsIn.Add("@OrderId", OrderId);
            hsIn.Add("@ServiceId", ServiceId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USP_Update_ServiceOrderDetail", hsIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet fillDoctorCombo(int HospitalId, int DepartmentId, int FacilityId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshInput = new Hashtable();
            HshInput.Add("HospitalLocationId", HospitalId);
            HshInput.Add("intDepartmentId", DepartmentId);
            HshInput.Add("intFacilityId", FacilityId);
            HshInput.Add("intEncodedBy", EncodedBy);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", HshInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataTable getLinkServiceSetupDetails(int FacilityId, int MainServiceId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intMainServiceId", MainServiceId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetLinkServiceSetupDetails", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet populateInvestigationSetUserWise(int iHospitalLocationId, int iDoctorID)
        {

            HshIn = new Hashtable();
            HshIn.Add("@HospitalLocationID", iHospitalLocationId);
            HshIn.Add("@DoctorId", iDoctorID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetInvestigationSet", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}