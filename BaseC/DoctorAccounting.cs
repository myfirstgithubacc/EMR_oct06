using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace BaseC
{
    public partial class DoctorAccounting
    {
        private string sConString = "";
        private Hashtable HshIn;
        private Hashtable hshOut;
        DAL.DAL objDl;
        public DoctorAccounting(string Constring)
        {
            sConString = Constring;
        }
        public DataSet GetDaProvisionalStatement(int iHospitalLocationID, int iFacilityID, string sFromDate, string sToDate, string sSelectDoctor)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@FromDate", sFromDate);
            HshIn.Add("@ToDate", sToDate);
            HshIn.Add("@chvSelectDoctor", sSelectDoctor);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaDoctorStatementinGrid", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDaDoctorShareSlabPercentage(int iHospitalLocationID, int iFacilityID, string sFromDate, string sToDate, int iDoctorId, int iTypeID)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@FDate", sFromDate);
            HshIn.Add("@TDate", sToDate);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@iTypeID", iTypeID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDaDoctorShareSlabPercentage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveDaDoctorShareSlabPercentage(int iHospitalLocationID, int iFacilityID, string sFromDate, string sToDate, int iDoctorId, int iTypeID, string xmlSlabPercentage, string OpIpType, int EncodedBy)
        {

            HshIn = new Hashtable();
            hshOut = new Hashtable();

            HshIn.Add("@intHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@FDate", sFromDate);
            HshIn.Add("@TDate", sToDate);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@xmlSlabPercentage", xmlSlabPercentage);
            HshIn.Add("@iTypeID", iTypeID);
            HshIn.Add("@OpIpType", OpIpType);
            HshIn.Add("@intEncodedBy", EncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDaDoctorShareSlabPercentage", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveUpdateDaDoctorWorkDays(int iHospitalLocationID, int iFacilityID, int iMonth, int iYear, string xmlDoctorWorkingDays, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvMonth", iMonth);
            HshIn.Add("@chvYear", iYear);
            HshIn.Add("@xmlDoctorWorkingDays", xmlDoctorWorkingDays);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvOutPutError", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDaDoctorWorkingDays", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorWorkingDays(int iHospitalLocationId, int iFacilityId, string sMonth, string sYear)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvMonth", sMonth);
            HshIn.Add("@chvYear", sYear);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDaDoctorWorkingDays", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DaSaveDoctorSetUp(string xDoctorSetup)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@xmlDoctorSetup", xDoctorSetup);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveDoctorSetup", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable DaSaveExcludedServiceCharge(int iHospitalLocationId, int iFacilityId, string xmlServiceId, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@xmlServiceId", xmlServiceId);
            hshOut.Add("@chvOutPutError", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDaExcludeServiceCharge", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaExcludedServiceCharge(int iHospitalLocationId, int iFacilityId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDaExcludeServiceCharge", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool CheckWorkingDaysInFinalStatement(int iMonth, int iYear)
        {
            bool bResult = false;
            string sQuery = "SELECT DISTINCT  CASE WHEN Month IS NOT NULL  AND Year IS NOT NULL THEN 1 ELSE 0 END AS IsDoctorWorkingDays FROM DaDoctorWorkDays WITH (NOLOCK) WHERE Month=@intMonth AND Year=@intYear AND Active=1";
            HshIn = new Hashtable();
            HshIn.Add("@intMonth", iMonth);
            HshIn.Add("@intYear", iYear);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                bResult = Convert.ToBoolean(objDl.ExecuteScalar(CommandType.Text, sQuery, HshIn));
                return bResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        // add by Balkishan start
        public DataSet GetReferDrAccounting(string fdate, string tdate, int intHospitalLocationid, int intFacilityId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@fdate", fdate);
            HshIn.Add("@tdate", tdate);
            HshIn.Add("@intHospitalLocationid", intHospitalLocationid);
            HshIn.Add("@intFacilityId", intFacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaUspReferDrAccounting", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveReferralPayment(DateTime FromDate, DateTime ToDate, int intHospitalLocationId, int intFacilityId, char StatementStatus, string Xmltable, int UserID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@FromDate", FromDate.ToString("yyyy/MM/dd"));
            HshIn.Add("@ToDate", ToDate.ToString("yyyy/MM/dd"));
            HshIn.Add("@intHospitalLocationid", intHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@StatementStatus", StatementStatus);
            HshIn.Add("@Xmltable", Xmltable);
            HshIn.Add("@intEncodedBy", UserID);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveReferralPayment", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelReferralPayment(int RefStatementId, int intHospitalLocationId, int intFacilityId, int UserID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@RefStatementId", RefStatementId);
            HshIn.Add("@intHospitalLocationid", intHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intEncodedBy", UserID);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDaCancelReferralPayment", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCompanyList(int inyHospitalLocationID, string chvCompanyType, int intInsuranceId)
        {

            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationID", inyHospitalLocationID);
            HshIn.Add("@chvCompanyType", chvCompanyType);
            HshIn.Add("@intInsuranceId", intInsuranceId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaUspReferDrAccounting", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        // Added class of wcf 

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

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorSharePercentage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaDoctorServiceCharges(int iHospitalLocationID, int iFacilityID, bool bDoctorRelated)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationid", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@bitDoctorRelated", bDoctorRelated);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "DaSaveDoctorServiceSetUp", HshIn);
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intType", iTypeID);
            HshIn.Add("@xmlDoctorWise", xmlDoctorWise);
            HshIn.Add("@CompanyId", CompanyId);
            HshIn.Add("@intDepartmentTypeID", DepartmentTypeID);
            HshIn.Add("@intSubDepartmentId", iSubDeptId);
            HshIn.Add("@intServiceId", sServiceId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDoctorWiseSharePercentage", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void DaSaveDoctorProvisionalStatement(int iHospitalLocationID, int iFacilityID, int iEncodedBy, string xmlDoctorServices,
            string xmlDoctorRelease, string sOPStatus, int iPreDaId, char StatementStatus)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@intHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@DoctorServices", xmlDoctorServices);
            HshIn.Add("@DoctorRelease", xmlDoctorRelease);
            HshIn.Add("@OPStatus", sOPStatus);
            HshIn.Add("@PreDaId", iPreDaId);
            HshIn.Add("@chvStatementStatus", StatementStatus);
            HshIn.Add("@EncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "DaProcessDoctorPayment", HshIn, hshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public Hashtable GetDaStatementMonthForCancel(int iHospitalLocationID, int iFacilityID, string sStatementMonth, string sStatementYear)
        {

            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvStatementYear", sStatementYear);
            HshIn.Add("@chvStatementMonth", sStatementMonth);
            hshOut.Add("@chvOutput", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaGetStatementMonthForCancel", HshIn, hshOut);
                return hshOut;
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
                              + " StatementYear, CASE StatementStatus WHEN 'O' THEN 'Open' WHEN 'C' THEN 'Closed' END AS StatementStatus FROM  DaFinalDoctorStatementMain dsm WITH (NOLOCK) "
                              + " WHERE dsm.FacilityId=@intFacilityID AND dsm.HospitalLocationId=@inyHospitalLocationID ";

            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, sStatement, HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable CancelDaFinalStatement(int iHospitalLocationID, int iFacilityID, int iFinalStatement)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@intFinalStatementId", iFinalStatement);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaCancelDoctorFinalStatement", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveDaEarningDeductionHead(int iHeadID, int iHospitalLocationID, int iFacilityID, string sHeadName,
            char cHeadType, char cBeforeAfterTDS, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@intHeadId", iHeadID);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            HshIn.Add("@intFacilityId", iFacilityID);
            HshIn.Add("@chvHeadName", sHeadName);
            HshIn.Add("@chvHeadType", cHeadType);
            HshIn.Add("@chvBeforeAfterTDS", cBeforeAfterTDS);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDaEarningDeductionHead", HshIn, hshOut);
                return hshOut;
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetHeadName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable DaSaveDoctorHeadDetails(int iHospitalLocationID, int iFacilityID, int iHeadId, String xmlDoctorHead, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intHeadId", iHeadId);
            HshIn.Add("@xmlDoctorHead", xmlDoctorHead);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaSaveDoctorHeadDetails", HshIn, hshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDaTeamDoctorLists(int iHospitalLocationID, Int32 iTeamId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@intTeamId", iTeamId);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDepartmentWiseDoctorLists", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable DaSaveTeamDoctorDetails(int iHospitalLocationID, int iFacilityID, int iTeamId, String xmlTeamDoctor, int iEncodedBy)
        {
            HshIn = new Hashtable();
            hshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@intTeamId", iTeamId);
            HshIn.Add("@xmlTeamDoctor", xmlTeamDoctor);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveTeamDoctorDetails", HshIn, hshOut);
                return hshOut;
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            hshOut = new Hashtable();
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
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveAmountAdjustment", HshIn, hshOut);
                return hshOut;
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
            hshOut = new Hashtable();
            HshIn.Add("@intID", Id);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intFacilityID", iFacilityId);

            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@bitDefaultRange", bDefaultRange);
            HshIn.Add("@bitActive", bActive);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDaSaveDoctorDateRange", HshIn, hshOut);
                return hshOut;
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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaGetDoctorShareValue", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorList(int HospitalId, int intSpecialisationId, int FacilityId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("HospitalLocationId", HospitalId);
                HshIn.Add("intSpecialisationId", intSpecialisationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("Active", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDAMasterDoctorList(int HospitalId, int intSpecialisationId, int FacilityId, int EncodedBy, int IsMedicalProvider)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOut = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalId);
                HshIn.Add("intSpecialisationId", intSpecialisationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("intIsMedicalProvider", IsMedicalProvider);
                hshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDAMasterDoctorList", HshIn, hshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getUnSetteledBillRefferal(int HospitalId, int FacilityId, string invoicedate, string receiptdate, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityid", FacilityId);
                HshIn.Add("@vcInvoiceDate", invoicedate);
                HshIn.Add("@vcReceiptDate", receiptdate);
                HshIn.Add("@OPIP", OPIP);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USP_FillDoctorAccUnSettledInvoices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveDoctorunsettledbillTagging(int iHospId, string iXmlData, int iFaclityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@xmlDoctorunsettlebill", iXmlData);
                HshIn.Add("@intFacilityid", iFaclityId);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveReleaseUnsettledInvoice", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Omprakash Sharma
        public DataSet DoctorPaymentDetail(int HospitalId, int FacilityId, string FromDate, string ToDate, string DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityid", FacilityId);
            HshIn.Add("@FromDate", FromDate);
            HshIn.Add("@ToDate", ToDate);
            HshIn.Add("@vcdoctorId", DoctorId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspFillDoctorPayableStatementonOrderBasis", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string SaveDaProcessOrderBasedDoctorPayment(int iHospId, int iFaclityId, int EncodedBy, string iXmlData, string FromDate, string ToDate, int PreDaId)
        {
            return SaveDaProcessOrderBasedDoctorPayment(iHospId, iFaclityId, EncodedBy, iXmlData, FromDate, ToDate, PreDaId, 0);
        }
        public string SaveDaProcessOrderBasedDoctorPayment(int iHospId, int iFaclityId, int EncodedBy, string iXmlData, string FromDate, string ToDate, int PreDaId, int ModeId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOut = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFaclityId);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@DoctorServices", iXmlData);
                HshIn.Add("@intModeId", ModeId);
                HshIn.Add("@FromDate", FromDate);
                HshIn.Add("@ToDate", ToDate);
                HshIn.Add("@PreDaId", PreDaId);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "DaProcessOrderBasedDoctorPayment", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetSavedDoctorStatementSummary(int HospitalId, int FacilityId, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityid", FacilityId);
                HshIn.Add("@VCfromDate", FromDate);
                HshIn.Add("@VCToDate", ToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPFillSavedDoctorStatementSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string DeleteDaProcessOrderBasedDoctorPayment(int iHospId, int iFaclityId, int Partialid, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOut = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFaclityId);
                HshIn.Add("@Partialid", Partialid);
                HshIn.Add("@EncodedBy", EncodedBy);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDeleteDaProcessOrderBasedDoctorPayment", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDaPayoutSchemeMaster(int intHospitalLocationId, int intFacilityId, int intSchemeId, string ValidFrom, int iDepartmentID)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intSechemeId", intSchemeId);
            HshIn.Add("@ValidFrom", ValidFrom);
            HshIn.Add("@intDepartmentId", iDepartmentID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDaPayoutSchemeMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDAPayoutFinalisationServiceReferralSetup(int intHospitalLocationId, int intFacilityId, int intDoctorId, int intSubDepartmentId, string ValidFrom, string intFillType)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intDoctorId", intDoctorId);
            HshIn.Add("@SubDeptId", intSubDepartmentId);
            HshIn.Add("@ValidFrom", ValidFrom);
            HshIn.Add("@intFillType", intFillType);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPFillDAPayoutFinalisationServiceReferralSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveDaPayoutSchemeMaster(string Type, int SchemeID, string SchemeName, bool Active, int EncodedBy, int intFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOut = new Hashtable();
                HshIn.Add("@Type", Type);
                HshIn.Add("@SchemeName", SchemeName);
                HshIn.Add("@Active", Active);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@SchemeID", SchemeID);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDaPayoutSchemeMaster", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SavePayoutReportFinalisationServiceSetup(int intHospitalLocationId, int intFacilityId, int DoctorID,
             String ValidFrom, string xmlService, int EncodedBy, string intSaveType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hshOut = new Hashtable();
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@DoctorID", DoctorID);
                HshIn.Add("@ValidFrom", Convert.ToDateTime(ValidFrom));
                HshIn.Add("@XMLServiceCharge", xmlService);
                HshIn.Add("@intSaveType", intSaveType);
                HshIn.Add("@EncodedBy", EncodedBy);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSavePayoutReportFinalisationServiceSetup", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Omprakash Sharma By Sajid Sir
        public DataSet BindSlabFee()
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "DaUspBindSlabFee", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetDoctorAdditionalPhrItemCategoryDetails(int iHospitalLocationID)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationID);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorAdditionalPhrItemCategoryDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSavedDoctorWiseItemCategoryAdditionalShare(int iHospitalLocationID, int iFacilityID, int itemSubCategoryID, int CompanyTypeID, DateTime sFromDate, DateTime sToDate, string XMLServiceCharge, int EncodedBy)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityID", iFacilityID);
            HshIn.Add("@itemSubCategoryID", itemSubCategoryID);
            HshIn.Add("@CompanyTypeID", CompanyTypeID);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@XMLServiceCharge", XMLServiceCharge);
            HshIn.Add("@EncodedBy", EncodedBy);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPSAVEDaDoctorWiseItemCategoryAdditionalShare", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorAdditionalSharePharmcySubDepartmentWise(int intHospitalLocationId, int intFacilityId, int intSubDepartmentId, string intServiceId, DateTime sFromDate, DateTime sToDate)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@SubDeptId", intSubDepartmentId);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPFillDoctorAdditionalSharePharmcySubDepartmentWise", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveDoctorAdditionalSharePharmcySubDepartmentWise(int intHospitalLocationId, int intFacilityId, int SubDepartmentID, int ServiceId, String ValidFrom, string ValidTo, string xmlService, int EncodedBy, string TYPE)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                hshOut = new Hashtable();
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@SubDepartmentID", SubDepartmentID);
                HshIn.Add("@ServiceID", ServiceId);
                HshIn.Add("@ValidFrom", Convert.ToDateTime(ValidFrom));
                HshIn.Add("@ValidTo", Convert.ToDateTime(ValidTo));
                HshIn.Add("@XMLServiceCharge", xmlService);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@TYPE", TYPE);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDoctorAdditionalSharePharmcySubDepartmentWise", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDoctorDetail(int EmployeeId, int iHospitalLocationID)
        {
            Hashtable hshTable = new Hashtable();
            hshTable.Add("@EmployeeId", EmployeeId);
            hshTable.Add("@iHospitalLocationID", iHospitalLocationID);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet objDs = (DataSet)dl.FillDataSet(CommandType.Text, "select emp.Id, Emp.FirstName + ISNULL( +' ' +Emp.MiddleName,'') + ISNULL( +' ' +Emp.LastName,'') as EmployeeName ,ESI.SignatureImage from Employee emp inner join DoctorDetails dd with(nolock) on dd.DoctorId = emp.ID left join EmployeeSignatureImage ESI with(nolock) on ESI.EmployeeId = emp.ID where emp.ID = @EmployeeId and emp.HospitalLocationId = @iHospitalLocationID", hshTable);
            return objDs;
        }

    }
}
