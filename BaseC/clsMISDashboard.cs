using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class clsMISDashboard
    {
        string sConString = "";
        DAL.DAL objDl;
        Hashtable HshIn;
        Hashtable HshOut;

        public clsMISDashboard(string Constring)
        {
            sConString = Constring;
        }
        public DataSet getMISRevenueNTarget(int iHospitalLocationId, int iFacilityId, int iYear, int iMonth)
        {
            HshIn = new Hashtable();
            string sQuery = "";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                sQuery = " SELECT t.ID RowNumber, s.ID, s.Name AS Specialisation,S.ID AS SpecializationId,t.OPTargetNos,t.IPTargetNos,t.OPTargetValue,t.IPTargetValue" +
                       " FROM SpecialisationMaster s WITH (NOLOCK) LEFT JOIN MISPerformanceTarget t WITH (NOLOCK) ON t.SpecialityId=s.ID" +
                       " AND t.FYears=@intYear AND t.CMonths=@intMonth AND t.Active=1" +
                       " AND t.FacilityId=@intFacilityId AND t.HospitalLocationId=@inyHospitalLocationId ORDER BY s.Name ASC";

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intYear", iYear);
                HshIn.Add("@intMonth", iMonth);

                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getMISFinancialYear(int iYear, int iMonth)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intCurrentYear", iYear);
                HshIn.Add("@intCurrentMonth", iMonth);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFinancialTime", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public Hashtable saveMISRevenueNTarget(int iHospitalLocationId, int iFacilityId, string iCurrentYear,
            Int32 iCurrentMonth, Int32 iFinancialMonth, string iFinancialYear, string xmlRevenueTarget, int iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intCurrentMonth", iCurrentMonth);
                HshIn.Add("@intCurrentYear", iCurrentYear);
                HshIn.Add("@intFinancialMonth", iFinancialMonth);
                HshIn.Add("@intFinancialYear", iFinancialYear);
                HshIn.Add("@xmlRevenueTarget", xmlRevenueTarget);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("@chvOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetSaveRevenueNTarget", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getMISDashBoardData(int iHospitalLocationId, int iFacilityId, string DashBoardType, int Groupid)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Locid", iHospitalLocationId);
                HshIn.Add("@FacilityId", iFacilityId);
                HshIn.Add("@DashBoardType", DashBoardType);
                HshIn.Add("@Groupid", Groupid);


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspMisDashBoardForPage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getPharmacyDashBoardData(int iHospitalLocationId, int iFacilityId, string Deptid, int Groupid)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Locid", iHospitalLocationId);
                HshIn.Add("@FacilityId", iFacilityId);
                HshIn.Add("@Deptid", Deptid);
                HshIn.Add("@Groupid", Groupid);


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspPhrDashBoard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getMISDashBoardPermission(int iHospitalLocationId, int iFacilityId, int Groupid, string ProcedureName)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFacilityid", iHospitalLocationId);
                HshIn.Add("@intHospitalLocationid", iFacilityId);
                HshIn.Add("@intGroupid", Groupid);


                return objDl.FillDataSet(CommandType.StoredProcedure, ProcedureName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getMISDashBoardMasterPages(int iHospitalLocationId, int iFacilityId, int iMasterPageId, int iId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intMasterPageId", iMasterPageId);
                HshIn.Add("@intId", iId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMISDashboardMasterPages", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getMISDashBoardScoreCardSummary(int iHospitalLocationId, int iFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@LocId", iHospitalLocationId);
                HshIn.Add("@FacilityId", iFacilityId);
                HshIn.Add("@currentDate", System.DateTime.Now.ToString("yyyy/MM/dd"));

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspMisDashBoardScoreCardSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getMISDashBoardScoreCardSummary(int iHospitalLocationId, int iFacilityId, string currentDate)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@LocId", iHospitalLocationId);
                HshIn.Add("@FacilityId", iFacilityId);
                HshIn.Add("@currentDate", currentDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspMisDashBoardScoreCardSummary", HshIn);
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

                return objDl.FillDataSet(CommandType.StoredProcedure, "DOC_PERFORMANCE_DETAIL_VIEW", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBillRegister(DateTime FromDate, DateTime ToDate, int HospId, int FacilityId,
           string PatientType, string CompanyCode, string OPIP)
        {
            Hashtable HshIn = new Hashtable();


            string fDate = FromDate.ToString("yyyy/MM/dd");
            string tDate = ToDate.ToString("yyyy/MM/dd");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@FromDate", fDate);
            HshIn.Add("@ToDate", tDate);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@PatientType", PatientType);
            HshIn.Add("@CompanyCode", CompanyCode);
            HshIn.Add("@OPIP", OPIP);
            HshIn.Add("@OutType", " ");
            HshIn.Add("@ForExport", "Y");
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspOutBillRegister", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //p[0] = new ReportParameter("inyHospitalLocationId", common.myStr(Session["HospitalLocationId"]));
        // p[1] = new ReportParameter("intFacilityId", common.myStr(Session["FacilityId"]));
        // p[2] = new ReportParameter("PatientType", common.myStr(Request.QueryString["PaymentType"]));
        // p[3] = new ReportParameter("User_id", strUserName);
        // p[4] = new ReportParameter("CompanyCode", strIdList);
        // p[5] = new ReportParameter("OPIP", common.myStr(Request.QueryString["SourceType"]));
        // p[6] = new ReportParameter("UHID", (string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));
        // p[7] = new ReportParameter("FromDate", common.myStr(frmdate.ToString("yyyy/MM/dd")));
        // p[8] = new ReportParameter("ToDate", common.myStr(todate.ToString("yyyy/MM/dd")));
        // p[9] = new ReportParameter("ReportHeader", "OutStanding Details");
        // method overload
        public DataSet getMISDashBoardData(int iHospitalLocationId, int iFacilityId, string DashBoardType, int Groupid, int EntrySiteId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Locid", iHospitalLocationId);
                HshIn.Add("@FacilityId", iFacilityId);
                HshIn.Add("@DashBoardType", DashBoardType);
                HshIn.Add("@Groupid", Groupid);
                HshIn.Add("@EntrySite", EntrySiteId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspMisDashBoardForPage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getHospitalPerformance(DateTime FromDate, DateTime ToDate, int UserId, int HospId, int FacilityId, int EntrySite)
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
                HshIn.Add("@EntrySite", EntrySite);

                return objDl.FillDataSet(CommandType.StoredProcedure, "DOC_PERFORMANCE", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


    }
}
