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
    public class clsLISMaster
    {
        Hashtable HshIn;
        Hashtable HshOut;

        private string sConString = "";
        public clsLISMaster(string conString)
        {
            sConString = conString;
        }
        public DataSet GetHospitalServices(Int16 iHospitalLocationId, Int16 iDepartmentId, Int16 iSubDepartmentId, int FacilityId)
        {
            return GetHospitalServices(iHospitalLocationId, iDepartmentId, iSubDepartmentId, FacilityId, string.Empty, string.Empty);
        }
        public DataSet GetHospitalServices(Int16 iHospitalLocationId, Int16 iDepartmentId, Int16 iSubDepartmentId, int FacilityId, string ServiceName, string CPTCode)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intDepartmentId", iDepartmentId);
            HshIn.Add("chvSubDepartmentIds", iSubDepartmentId);
            HshIn.Add("chrType", "");
            HshIn.Add("intFacilityId", FacilityId);
            if (!string.IsNullOrEmpty(ServiceName))
                HshIn.Add("chvServiceName", ServiceName);
            if (!string.IsNullOrEmpty(CPTCode))
                HshIn.Add(" chvCPTCode", CPTCode);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetHospitalServices", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetServicesDetails(string xServiceID)
        {
            HshIn = new Hashtable();
            HshIn.Add("@XMLServiceId", xServiceID);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetServiceDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getFacility(int iHospID, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationID", iHospID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFacilityMaster", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getFacilityList(int iHospID, int iUserId, int iGroupID)
        {
            return getFacilityList(iHospID, iUserId, iGroupID, 0);
        }

        public DataSet getFacilityList(int iHospID, int iUserId, int iGroupID, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intUserId", iUserId);
                HshIn.Add("@intGroupId", iGroupID);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getSubDepartmentData(int iHospID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@iHospID", iHospID);

                string qry = " SELECT SubDeptId, Departmentname, SubName FROM DepartmentSub ds WITH (NOLOCK) " +
                              " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON dm.DepartmentID = ds.DepartmentID AND dm.Active = 1 " +
                              " WHERE [Type] IN ('I', 'IS', 'P') AND ds.Active = 1 AND ds.HospitalLocationId = @iHospID " +
                              " ORDER BY Departmentname, SubName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getSubDepartmentByStationAndLabNo(string Source, int iStationId, int iLabNo)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intLabNo", iLabNo);

                string strTableName = "";
                if (Source == "OPD")
                {
                    strTableName = "DiagSampleOPLabMain";
                }
                else if (Source == "IPD")
                {
                    strTableName = "DiagSampleIPLabMain";
                }

                string qry = " SELECT DISTINCT ds.SubDeptId, ds.SubName  FROM " + strTableName + " dsom WITH (NOLOCK) " +
                    " INNER JOIN DepartmentSuB ds WITH (NOLOCK) ON dsom.SubDeptId = ds.SubDeptId " +
                    " INNER JOIN DiagSampleReceivingStationDetails dsrs WITH (NOLOCK) ON ds.SubDeptId = dsrs.SubDeptId " +
                    " AND dsrs.StationId = @intStationId AND dsrs.Active=1 " +
                    " INNER JOIN StatusMaster sm WITH (NOLOCK) ON sm.StatusId = dsom.StatusId AND sm.Code NOT IN ('RF')" +
                    " WHERE dsom.LabNo = @intLabNo  AND dsom.Active=1 ORDER BY SubName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEmployeeData(int iHospID, int iEmpTypeId, string xmlEmployeeType,
                                string EmpName, int iMobileNo, int EncodedBy)
        {
            return getEmployeeData(iHospID, 0, iEmpTypeId, xmlEmployeeType, EmpName, iMobileNo, EncodedBy, "", 0);
        }

        public DataSet getEmployeeData(int iHospID, int iStationId, int iEmpTypeId, string xmlEmployeeType,
                               string EmpName, int iMobileNo, int EncodedBy, string cDepartmentIdendification, int intFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@HospitalLocationId", iHospID);
                HshIn.Add("@intEmployeeTypeId", iEmpTypeId);
                if (xmlEmployeeType != "")
                {
                    HshIn.Add("@xmlEmployeeType", xmlEmployeeType);
                }
                HshIn.Add("@chrEmployeeName", EmpName);
                HshIn.Add("@intLabStationId", iStationId);
                HshIn.Add("@intMobileNo", iMobileNo);
                HshIn.Add("@bitStatus", 1);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@cDepartmentIdendification", cDepartmentIdendification);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable getDoctorList(int iDoctorId, string DoctorName, int iHospID, int iSpecialisationId, int iFacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@HospitalLocationId", iHospID);
                HshIn.Add("@intSpecialisationId", iSpecialisationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);


                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", HshIn, HshOut);

                DataTable TBL = ds.Tables[0].Copy();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (iDoctorId > 0 || DoctorName != "")
                    {
                        DataView DV = ds.Tables[0].Copy().DefaultView;
                        string strFilter = "";
                        if (iDoctorId > 0)
                        {
                            strFilter += "DoctorId = " + iDoctorId;
                        }

                        if (DoctorName != "")
                        {
                            if (strFilter != "")
                            {
                                strFilter += " AND ";
                            }

                            strFilter += " DoctorName LIKE '%" + DoctorName + "%'";
                        }

                        DV.RowFilter = strFilter;

                        TBL = DV.ToTable();
                    }
                }

                return TBL;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDoctorList(int iDoctorId, string DoctorName, int iHospID, int iSpecialisationId, int iFacilityId, int EncodedBy, int IsMedicalProvider)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@HospitalLocationId", iHospID);
                HshIn.Add("@intSpecialisationId", iSpecialisationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                if (IsMedicalProvider == 0)
                {
                    HshIn.Add("@intIsMedicalProvider", IsMedicalProvider);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataTable GetEmployeeType()
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string Query = "SELECT Id,Description,EmployeeType FROM EmployeeType WITH (NOLOCK) WHERE EmployeeType in('LIC','LS','LD','LT','NU','LDIR') ORDER BY Description";
            try
            {
                return objDl.FillDataSet(CommandType.Text, Query).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string ChangeSpanTagToFontTag(string SpanTagSting)
        {
            SpanTagSting = SpanTagSting.Replace("<span style=\"color:", "<font color=");
            SpanTagSting = SpanTagSting.Replace("color:", "color=");
            SpanTagSting = SpanTagSting.Replace(";\">", ">");
            SpanTagSting = SpanTagSting.Replace("</span>", "</Font>");
            SpanTagSting = SpanTagSting.Replace("<em>", "<i>");
            SpanTagSting = SpanTagSting.Replace("</em>", "</i>");
            SpanTagSting = SpanTagSting.Replace("<strong>", "<b>");
            SpanTagSting = SpanTagSting.Replace("</strong>", "</b>");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 10px", "font size=1");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 13px", "font size=2");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 16px", "font size=3");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 18px", "font size=4");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 24px", "font size=5");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 32px", "font size=6");
            SpanTagSting = SpanTagSting.Replace("span style=\"font-size: 48px", "font size=7");
            SpanTagSting = SpanTagSting.Replace("font-size: 10px", "font size=1");
            SpanTagSting = SpanTagSting.Replace("font-size: 13px", "font size=2");
            SpanTagSting = SpanTagSting.Replace("font-size: 16px", "font size=3");
            SpanTagSting = SpanTagSting.Replace("font-size: 18px", "font size=4");
            SpanTagSting = SpanTagSting.Replace("font-size: 24px", "font size=5");
            SpanTagSting = SpanTagSting.Replace("font-size: 32px", "font size=6");
            SpanTagSting = SpanTagSting.Replace("font-size: 48px", "font size=7");
            SpanTagSting = SpanTagSting.Replace("span style=\">", "");
            return SpanTagSting;
        }
        public string ChangeFontToSpanTag(string FontTagString)
        {
            FontTagString = FontTagString.Replace("<font color=", "<span style=\"color:");
            FontTagString = FontTagString.Replace("color=", "color:");
            FontTagString = FontTagString.Replace(">", ";\">");
            FontTagString = FontTagString.Replace("Font size=1", "span style=\"font-size: 10px");
            FontTagString = FontTagString.Replace("Font size=2", "span style=\"font-size: 13px");
            FontTagString = FontTagString.Replace("Font size=3", "span style=\"font-size: 16px");
            FontTagString = FontTagString.Replace("Font size=4", "span style=\"font-size: 18px");
            FontTagString = FontTagString.Replace("Font size=5", "span style=\"font-size: 24px");
            FontTagString = FontTagString.Replace("Font size=6", "span style=\"font-size: 32px");
            FontTagString = FontTagString.Replace("Font size=7", "span style=\"font-size: 48px");
            FontTagString = FontTagString.Replace("font size=1", "span style=\"font-size: 10px");
            FontTagString = FontTagString.Replace("font size=2", "span style=\"font-size: 13px");
            FontTagString = FontTagString.Replace("font size=3", "span style=\"font-size: 16px");
            FontTagString = FontTagString.Replace("font size=4", "span style=\"font-size: 18px");
            FontTagString = FontTagString.Replace("font size=5", "span style=\"font-size: 24px");
            FontTagString = FontTagString.Replace("font size=6", "span style=\"font-size: 32px");
            FontTagString = FontTagString.Replace("font size=7", "span style=\"font-size: 48px");
            FontTagString = FontTagString.Replace("</font>", "</span>");
            FontTagString = FontTagString.Replace("</Font>", "</span>");
            FontTagString = FontTagString.Replace(";\";", ";");

            return FontTagString;
        }
        public SqlDataReader GetEmployeeId(int iUserId, Int16 iHospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intUserId", iUserId);
                SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT EmpId, dbo.GetDoctorName(empid) as DoctorName From Users us WITH (NOLOCK) Inner Join Employee emp WITH (NOLOCK) On us.EmpId = emp.Id	Left Join EmployeeType empt WITH (NOLOCK) On empt.Id = emp.EmployeeType Where us.Id = @intUserId And us.HospitalLocationID = @inyHospitalLocationId ", HshIn);
                return objDr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDepartment(int StationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                Hashtable HshIn;
                HshIn = new Hashtable();
                HshIn.Add("@intStationId", StationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetDepartment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPackage(int StationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HshIn;
                HshIn = new Hashtable();
                HshIn.Add("@intStationId", StationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPackageList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPackageLab(int intLabno, int stationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intLabNo", intLabno);
                HshIn.Add("@intstationId", stationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPackageListOPLabNo", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPackageDetails(int PackageId, int stationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intPackageId", PackageId);
                HshIn.Add("@intstationId", stationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPackageDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSubDepartment(int StationId, int DepartmentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn;
            HshIn = new Hashtable();
            HshIn.Add("@intStationId", StationId);
            HshIn.Add("@intDepartmentId", DepartmentId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSubDepartment", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getRISSubDepartment(int FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                StringBuilder qry = new StringBuilder();

                HshIn.Add("@intFacilityId", FacilityId);

                qry.Append(" SELECT DISTINCT ds.SubDeptId, ds.SubName ");
                qry.Append(" FROM DepartmentSub ds WITH(NOLOCK) ");
                qry.Append(" INNER JOIN ResourceMaster rm WITH(NOLOCK) ON rm.SubDeptId = ds.SubDeptId AND rm.FacilityId = @intFacilityId AND rm.Active = 1 ");
                qry.Append(" INNER JOIN DiagSampleReceivingStationDetails dsrsD WITH(NOLOCK) ON dsrsD.SubDeptId = ds.SubDeptId AND ds.Active = 1 AND dsrsD.Active = 1 ");
                qry.Append(" WHERE dsrsD.StationId IN ");
                qry.Append(" ( ");
                qry.Append(" SELECT t1.StationId  FROM DiagSampleReceivingStation t1 WITH(NOLOCK) ");
                qry.Append(" INNER JOIN FlagsMaster t2 WITH(NOLOCK) ON t1.FlagId = t2.Id ");
                qry.Append(" WHERE t1.Active = 1 AND t2.FlagName = 'RIS' ");
                qry.Append(" ) ");
                qry.Append(" ORDER BY ds.SubName ");

                return objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getWard(int HospitalId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospitalId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMasterGetWards", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getMethodName(int HospitalId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationID", HospitalId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetMethodName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveUpdateMethodName(int MethodId, string MethodName, int statusId, int HospitalLocationId, int UserId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@MethodId", MethodId);
                HshIn.Add("@MethodName", MethodName);
                HshIn.Add("@Status", statusId);
                HshIn.Add("@UserId", UserId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagSaveMethodName", HshIn);
                return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet GetEntrySite(int HospitalLocationId, int iFacility)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacility", iFacility);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetEntrySites", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEntrySiteWards(int EntrySiteID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyEntrySiteID", EntrySiteID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetEntrySiteWards", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEntrySite(int EntrySiteId, string EntrySiteName, int HospitalLocationid, int bitActive, string xmlWard, int EncodedBy, int iFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intEntrySiteId", EntrySiteId);
            HshIn.Add("@chvEntrySiteName", EntrySiteName);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationid);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@bitActive", bitActive);
            HshIn.Add("@xmlWards", xmlWard);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveEntrySites", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetClinicalDetails(string Source, string EncounterNo, int labNo, int intEncounterId, int MRDCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HshIn;
                HshIn = new Hashtable();
                HshIn.Add("@chvSource", Source);
                HshIn.Add("@intLabNo", labNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@bitMRDCode", MRDCode);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveReportType(int HosptId, int ReportTypeId, string ReportTypeName, int Active, int UserId, int StationId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HosptId);
            HshIn.Add("@intStationId", StationId);
            HshIn.Add("@intReportTypeID", ReportTypeId);
            HshIn.Add("@chvReportType", ReportTypeName);
            HshIn.Add("@inyActive", Active);
            HshIn.Add("@intEncodedBy", UserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveReportType", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getReportType(int iHospID, int iActive, int StationId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intStationId", StationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetReportTypes", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int deActivateReportType(int iHospID, int iValueID, int iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@HospitalLocationID", iHospID);
            HshIn.Add("@ValueId", iValueID);

            string strUpdateUser = "";
            if (iUserID > 0)
            {
                strUpdateUser = ", EncodedBy = " + iUserID;
            }

            try
            {
                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE DiagValues SET Active = 0, " + strUpdateUser + " WHERE ValueId=@ValueId AND HospitalLocationID = @HospitalLocationID", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet getHospitalSetUp(int iHospitalLocationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospitalLocationID);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspMasterGetHospitalSetUp", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetSurgeryandProcedure(int intEncounterId, string strseacrh)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HshIn;
                HshIn = new Hashtable();
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@ChrSearch", strseacrh);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspSurgeryandProcedure", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getTextFields(int HospId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                StringBuilder qry = new StringBuilder();

                HshIn.Add("@intHospId", HospId);

                qry.Append("SELECT DISTINCT df.FieldId, df.FieldName ");
                qry.Append(" FROM DiagFields df WITH (NOLOCK) ");
                qry.Append(" INNER JOIN DiagFieldsText dft WITH (NOLOCK) ON df.FieldId = dft.FieldId AND df.Active = 1 AND dft.Active = 1 ");
                qry.Append(" WHERE df.HospitalLocationId = @intHospId ");
                qry.Append(" ORDER BY df.FieldName ");

                return objDl.FillDataSet(CommandType.Text, qry.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
    public class clsLISValue
    {
        private string sConString = "";
        public clsLISValue(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int ValueId = 0;
        public int StationId = 0;
        public long HospitalLocationID = 0;
        public string ValueName = "";
        public int Active = 1;
        public int userId = 0;
        public string SaveData(clsLISValue objLISValue)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", objLISValue.HospitalLocationID);
            HshIn.Add("@intValueID", objLISValue.ValueId);
            HshIn.Add("@chvValueName", objLISValue.ValueName);
            HshIn.Add("@inyActive", objLISValue.Active);
            HshIn.Add("@intEncodedBy", objLISValue.userId);
            HshIn.Add("@intStationId", objLISValue.StationId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateValue", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iHospID, int iActive, int StationId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intStationId", StationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateValues", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int deActivateData(int iHospID, int iValueID, int iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationID", iHospID);
                HshIn.Add("@ValueId", iValueID);

                string strUpdateUser = "";
                if (iUserID > 0)
                {
                    strUpdateUser = ", EncodedBy = " + iUserID;
                }

                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE DiagValues SET Active = 0, " + strUpdateUser + " WHERE ValueId=@ValueId AND HospitalLocationID = @HospitalLocationID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
    }

    public class clsLISSignatureSetup
    {
        private string sConString = "";
        public clsLISSignatureSetup(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int HospitalLocationId = 0;
        public int StationId = 0;
        public int OldEmployeeId = 0;
        public int EmployeeId = 0;
        public string ReportingStage = "";
        public int CancelFinalizedResult = 0;
        public int EditMachineResult = 0;
        public int Active = 1;
        public int userId = 0;
        public string sPermissionType = "";
        public bool editRelayDetails;
        public bool editManualRequest;
        public bool editPackageDetails;
        public int intEmployeeTypeId = 0;
        public string SaveData(clsLISSignatureSetup objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intStationId", objVal.StationId);
            HshIn.Add("@intEmployeeId", objVal.EmployeeId);
            HshIn.Add("@bitEditManualRequest", objVal.editManualRequest);
            HshIn.Add("@intEmployeeTypeId", objVal.intEmployeeTypeId);
            HshIn.Add("@chrReportingStage", objVal.ReportingStage);
            HshIn.Add("@bitCancelFinalizedResult", objVal.CancelFinalizedResult);
            HshIn.Add("@bitEditMachineResult", objVal.EditMachineResult);
            HshIn.Add("@bitEditRelayDetails", objVal.editRelayDetails);
            HshIn.Add("@bitEditPackageDetails", objVal.editPackageDetails);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@intEncodedBy", objVal.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSignatureSetup", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iStationId, int iEmployeeId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intEmployeeId", iEmployeeId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSignatureSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getSignatureDatawithFacility(int iStationId, int iEmployeeId, int @iFacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intEmployeeId", iEmployeeId);
                HshIn.Add("@iFacilityId", iFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSignatureSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }





        public DataSet getPermission(clsLISSignatureSetup objVal)
        {
            HshIn = new Hashtable();
            string strSQL = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStationId", objVal.StationId);
                HshIn.Add("@intEncodedBy", objVal.userId);
                HshIn.Add("@hospitalLocationId", objVal.HospitalLocationId);
                if (objVal.sPermissionType == "C")
                {
                    strSQL = "SELECT EmployeeId FROM DiagSignatureSetup WITH (NOLOCK) " +
                        " WHERE EmployeeId = (SELECT EmpID FROM Users WITH (NOLOCK) WHERE ID = @intEncodedBy) " +
                        " AND StationId = @intStationId AND CancelFinalizedResult = 1";
                }
                if (objVal.sPermissionType == "E")
                {
                    strSQL = "SELECT EmployeeId FROM DiagSignatureSetup WITH (NOLOCK) " +
                        " WHERE EmployeeId = (SELECT EmpID FROM Users WITH (NOLOCK) WHERE ID = @intEncodedBy) " +
                        " AND StationId = @intStationId AND EditMachineResult = 1";
                }
                if (objVal.sPermissionType == "F")
                {

                    strSQL = "SELECT EmployeeId FROM DiagSignatureSetup WITH (NOLOCK) " +
                        " WHERE EmployeeId = (SELECT EmpID FROM Users WITH (NOLOCK) WHERE ID = @intEncodedBy) " +
                        " AND StationId = @intStationId AND ReportingStage = 'F'";
                }
                if (objVal.sPermissionType == "P")
                {
                    strSQL = "SELECT EmployeeId FROM DiagSignatureSetup WITH (NOLOCK) " +
                        " WHERE EmployeeId = (SELECT EmpID FROM Users WITH (NOLOCK) WHERE ID = @intEncodedBy) AND " +
                        " StationId = @intStationId AND ReportingStage IN ('P','F')";
                }
                if (objVal.sPermissionType == "H")
                {
                    strSQL = "SELECT Value FROM HospitalSetup WITH (NOLOCK) WHERE Flag='EditMachineResultLevel' AND HospitalLocationId = @hospitalLocationId";
                }
                if (objVal.sPermissionType == "R")
                {
                    strSQL = "SELECT EmployeeId FROM DiagSignatureSetup WITH (NOLOCK) " +
                        " WHERE EmployeeId = (SELECT EmpID FROM Users WITH (NOLOCK) WHERE ID = @intEncodedBy) " +
                        " AND StationId = @intStationId AND EditRelayDetails = 1";
                }

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPermission(string MachineNo)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@MachineId", MachineNo);
                string strSQL = "";
                strSQL = "SELECT EditMachineResult FROM DiagInterfaceMachines WITH (NOLOCK) WHERE MachineID=@MachineId";

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getPermissions(clsLISSignatureSetup objVal)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStationId", objVal.StationId);
                HshIn.Add("@intEmployeeId", objVal.userId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetUserPermission", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
    }
    public class clsLISSampleReceivingStation
    {
        private string sConString = "";
        public clsLISSampleReceivingStation(string conString)
        {
            sConString = conString;
        }

        Hashtable HshIn;
        Hashtable HshOut;
        public int StationId = 0;
        public string StationName = "";
        public int HospitalLocationId = 0;
        public string xmlSubDepts = "";
        public int Active = 1;
        public int userId = 0;
        public string SaveData(clsLISSampleReceivingStation objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intStationId", objVal.StationId);
            HshIn.Add("@chvStationName", objVal.StationName);
            HshIn.Add("@inyHospitalLocationId", objVal.HospitalLocationId);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@xmlSubDepts", objVal.xmlSubDepts);

            HshIn.Add("@intEncodedBy", objVal.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleStation", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getMainData(int iStationId, int iHospID, int iActive)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@iStationId", iStationId);
                HshIn.Add("@iHospID", iHospID);
                HshIn.Add("@iActive", iActive);

                string strqry = " SELECT StationId, StationName, Active, " +
                                " (CASE WHEN Active=1 THEN 'Active' ELSE 'In-Active' END) AS [Status] " +
                                " FROM DiagSampleReceivingStation WITH (NOLOCK) " +
                                " WHERE StationId=(CASE WHEN @iStationId > 0 THEN @iStationId ELSE StationId END) " +
                                " AND Active=(CASE WHEN @iActive > 0 THEN @iActive ELSE Active END) " +
                                " AND HospitalLocationId = @iHospID " +
                                " ORDER BY StationName ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getStation(int employeeId, int GroupId)
        {
            return getStation(employeeId, GroupId, "");
        }

        public DataSet getStation(int employeeId, int GroupId, string flag)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intEmployeeID", employeeId);
            HshIn.Add("@intGroupId", GroupId);

            if (flag != "")
            {
                HshIn.Add("@sflx", flag);
            }

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetUserStations", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetStation(int HospitalLocationID, int StationId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
            HshIn.Add("@intStationId", StationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetStations", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetStation(int HospitalLocationID)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetStations", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDetailsData(int iStationId, int iHospID, int iActive)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@iStationId", iStationId);
                HshIn.Add("@iHospID", iHospID);
                HshIn.Add("@iActive", iActive);

                string strqry = " SELECT dsrd.StationId, dsrd.SubDeptId, dsr.StationName, dm.Departmentname, ds.SubName, " +
                                " dsr.Active,'' LeftDoctor, '' RightDoctor, '' CenterDoctor  " +
                                " FROM DiagSampleReceivingStationDetails dsrd WITH (NOLOCK) " +
                                " INNER JOIN DiagSampleReceivingStation dsr WITH (NOLOCK) ON dsrd.StationId = dsr.StationId " +
                                " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON dsrd.SubDeptId = ds.SubDeptId AND ds.Active = 1 " +
                                " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON dm.DepartmentID = ds.DepartmentID AND dm.Active = 1 " +
                                " WHERE dsrd.StationId = (CASE WHEN @iStationId > 0 THEN @iStationId ELSE dsrd.StationId END) " +
                                " AND dsr.Active = (CASE WHEN @iActive > 0 THEN @iActive ELSE dsr.Active END) " +
                                " AND dsrd.Active = 1 " +
                                " AND dsr.HospitalLocationId = @iHospID " +
                                " ORDER BY dsr.StationName, dm.Departmentname, ds.SubName ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool checkIsExistsDepartment(int iSubDeptId)
        {
            bool isExists = false;
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intSubDeptId", iSubDeptId);

                string strqry = " SELECT StationId FROM DiagSampleReceivingStationDetails WITH (NOLOCK) " +
                                " WHERE SubDeptId = @intSubDeptId AND Active = 1 ";

                ds = objDl.FillDataSet(CommandType.Text, strqry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    isExists = true;
                }

                return isExists;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

    }
    public class clsLISSampleMaster
    {
        private string sConString = "";
        public clsLISSampleMaster(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int SampleID = 0;
        public string Name = "";
        public int HospitalLocationId = 0;
        public int Active = 1;
        public int userId = 0;
        public string SaveData(clsLISSampleMaster objVal, string masterType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", objVal.HospitalLocationId);
                HshIn.Add("@bitActive", objVal.Active);
                HshIn.Add("@intEncodedBy", objVal.userId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                if (object.Equals(masterType, "SAMPLE"))
                {
                    HshIn.Add("@intSampleID", objVal.SampleID);
                    HshIn.Add("@chvName", objVal.Name);

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleMaster", HshIn, HshOut);
                }
                else if (object.Equals(masterType, "SAMPLE_UNIT"))
                {
                    HshIn.Add("@intUnitId", objVal.SampleID);
                    HshIn.Add("@chvUnitName", objVal.Name);

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveUnitMaster", HshIn, HshOut);
                }

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iSampleID, int iHospID, int iActive, string masterType)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@bitActive", iActive);
                if (object.Equals(masterType, "SAMPLE"))
                {
                    HshIn.Add("@intSampleID", iSampleID);
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSampleMaster", HshIn);

                    DataColumn col = new DataColumn("UnitId", typeof(System.Int32));
                    ds.Tables[0].Columns.Add(col);

                    col = new DataColumn("UnitName", typeof(System.String));
                    ds.Tables[0].Columns.Add(col);
                }
                else if (object.Equals(masterType, "SAMPLE_UNIT"))
                {
                    HshIn.Add("@intUnitId", iSampleID);
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetUnitMaster", HshIn);

                    DataColumn col = new DataColumn("SampleID", typeof(System.Int32));
                    ds.Tables[0].Columns.Add(col);

                    col = new DataColumn("Name", typeof(System.String));
                    ds.Tables[0].Columns.Add(col);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return ds;
        }
        public DataSet GetSampleTypeTagWithService(int iHospID, int ServiceId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@ServiceId", ServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSampleTypeTagWithService", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
    }
    public class clsLISExternalCenter
    {
        private string sConString = "";
        public clsLISExternalCenter(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int ExternalCenterId = 0;
        public string CenterName = "";
        public string Address1 = "";
        public string Address2 = "";
        public string Phone = "";
        public string Email = "";
        public string Fax = "";
        public int HospitalLocationId = 0;
        public int Active = 1;
        public int userId = 0;
        public string SaveData(clsLISExternalCenter objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intExternalCenterId", objVal.ExternalCenterId);
            HshIn.Add("@chvCenterName", objVal.CenterName);
            HshIn.Add("@chvAddress1", objVal.Address1);
            HshIn.Add("@chvAddress2", objVal.Address2);
            HshIn.Add("@chvPhone", objVal.Phone);
            HshIn.Add("@chvEmail", objVal.Email);
            HshIn.Add("@chvFax", objVal.Fax);
            HshIn.Add("@inyHospitalLocationId", objVal.HospitalLocationId);

            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@intEncodedBy", objVal.userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveExternalCenter", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iExternalCenterId, int iHospID, int iActive)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intExternalCenterId", iExternalCenterId);
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@bitActive", iActive);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetExternalCenter", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable getDoctorList(int iDoctorId, string DoctorName, int iHospID, int iSpecialisationId, int iFacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospitalLocationId", iHospID);
                HshIn.Add("@intSpecialisationId", iSpecialisationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorList", HshIn, HshOut);

                DataTable TBL = ds.Tables[0].Copy();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (iDoctorId > 0 || DoctorName != "")
                    {
                        DataView DV = ds.Tables[0].Copy().DefaultView;
                        string strFilter = "";
                        if (iDoctorId > 0)
                        {
                            strFilter += "DoctorId = " + iDoctorId;
                        }

                        if (DoctorName != "")
                        {
                            if (strFilter != "")
                            {
                                strFilter += " AND ";
                            }

                            strFilter += " DoctorName LIKE '%" + DoctorName + "%'";
                        }

                        DV.RowFilter = strFilter;

                        TBL = DV.ToTable();
                    }
                }

                return TBL;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
    }
    public class clsLISInterfaceMachines
    {
        private string sConString = "";
        public clsLISInterfaceMachines(string conString)
        {
            sConString = conString;
        }

        Hashtable HshIn;
        Hashtable HshOut;
        public int MachineID = 0;
        public int HospitalLocationId = 0;
        public string MachineName = "";
        public int BaudRate = 0;
        public string Parity = "";
        public int DataBits = 0;
        public int StopBits = 0;
        public int SubDeptId = 0;
        public int Port = 0;
        public int Active = 1;
        public int userId = 0;
        public int PocMachine = 0;
        public bool bitEditMachineResult;
        public int FacilityId = 0;

        public string SaveData(clsLISInterfaceMachines objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intMachineID", objVal.MachineID);
            HshIn.Add("@inyHospitalLocationId", objVal.HospitalLocationId);
            HshIn.Add("@chvMachineName", objVal.MachineName);
            HshIn.Add("@intBaudRate", objVal.BaudRate);
            HshIn.Add("@chrParity", objVal.Parity);
            HshIn.Add("@inyDataBits", objVal.DataBits);
            HshIn.Add("@inyStopBits", objVal.StopBits);
            HshIn.Add("@intSubDeptId", objVal.SubDeptId);
            HshIn.Add("@inyHostComputerCOMPort", objVal.Port);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@bitPocMachine", objVal.PocMachine);
            HshIn.Add("@intEncodedBy", objVal.userId);
            HshIn.Add("@bitEditMachineResult", objVal.bitEditMachineResult);
            HshIn.Add("@intFacilityId", objVal.FacilityId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveInterfaceMachines", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iMachineID, int iHospID, int iSubDeptId, int iActive, int StationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intMachineID", iMachineID);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInterfaceMachines", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public string SaveHostComputerData(int Id, int MachineID, int PortNo, string ComputerName, int Active, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intId", Id);
            HshIn.Add("@intMachineID", MachineID);
            HshIn.Add("@inyPortNo", PortNo);
            HshIn.Add("@chvComputerName", ComputerName);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveHostComputer", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getHostComputerData(int id, int iMachineID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string qry = " SELECT Id, MachineID, HostComputerName, HostComputerCOMPort, Active, " +
                             " (CASE WHEN Active = 1 THEN 'Active' ELSE 'In-Active' END) AS [Status] " +
                             " FROM DiagInterfaceMachineHostComp WITH (NOLOCK) " +
                             " WHERE 2 = 2 ";

                if (id > 0)
                {
                    HshIn.Add("@id", id);

                    qry += " AND Id = @id";
                }
                if (iMachineID > 0)
                {
                    HshIn.Add("@MachineID", iMachineID);

                    qry += " AND MachineID = @MachineID";
                }
                qry += " ORDER BY MachineID ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsLISInterfaceMachinesTest
    {
        private string sConString = "";
        public clsLISInterfaceMachinesTest(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public string strUseFor = "A"; //A-Antibiotics, O-Organism 
        public int Id = 0;
        public int MachineId = 0;
        public string MachineTestCode = "";
        public int ServiceId = 0;
        public int FieldId = 0;
        public int RoundOff = 0;
        public double MultiplyBy = 0;
        public double DivideBy = 0;
        public int Active = 0;
        public int userId = 0;
        public int IsCalculated = 0;

        public string SaveData(clsLISInterfaceMachinesTest objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intId", objVal.Id);
            HshIn.Add("@intMachineId", objVal.MachineId);
            HshIn.Add("@chvMachineTestCode", objVal.MachineTestCode);
            HshIn.Add("@intServiceId", objVal.ServiceId);
            HshIn.Add("@intFieldId", objVal.FieldId);
            HshIn.Add("@bitCalculatedField", objVal.IsCalculated);
            HshIn.Add("@inyRoundOff", objVal.RoundOff);
            HshIn.Add("@dblMultiplyBy", objVal.MultiplyBy);
            HshIn.Add("@dblDivideBy", objVal.DivideBy);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@intEncodedBy", objVal.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveInterfaceMachinesTest", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet getData(int iID, int iHospID, int StationId)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet ds = new DataSet();

        //    HshIn.Add("@intID", iID);
        //    HshIn.Add("@inyHospitalLocationId", iHospID);
        //    HshIn.Add("@intStationId", StationId);


        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInterfaceMachinesTest", HshIn, HshOut);

        //    return ds;
        //}

        public DataSet getData(int iID, int iHospID, int StationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intID", iID);
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@intFacilityId", FacilityId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInterfaceMachinesTest", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveInterfaceMachinesMicrobiology(clsLISInterfaceMachinesTest objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrUseFor", objVal.strUseFor);
            HshIn.Add("@intId", objVal.Id);
            HshIn.Add("@intMachineId", objVal.MachineId);
            HshIn.Add("@chvMachineTestCode", objVal.MachineTestCode);
            HshIn.Add("@intServiceId", objVal.ServiceId);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@intEncodedBy", objVal.userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveInterfaceMachinesMicrobiology", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getInterfaceMachinesMicrobiology(string useFor, int iID, int StationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@chrUseFor", useFor);
                HshIn.Add("@intID", iID);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@intFacilityId", FacilityId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInterfaceMachinesMicrobiology", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


    }
    public class clsTemplateFieldNumeric
    {
        private string sConString = "";
        public clsTemplateFieldNumeric(string conString)
        {
            sConString = conString;
        }

        Hashtable HshIn;
        Hashtable HshOut;
        public int FormatId = 0;
        public int FieldId = 0;
        public string LimitType = "";
        public int UnitId = 0;
        public char Sex = ' ';
        public int AgeFrom = 0;
        public char AgeFromType = ' ';
        public int AgeTo = 0;
        public char AgeToType = ' ';
        public double? MinValue = null;
        public double? MaxValue = null;
        public double? MinPanicValue = null;
        public double? MaxPanicValue = null;
        public char Symbol = ' ';
        public int Active = 0;
        public int userId = 0;
        public int MachineId = 0;
        public int MinHours = 0;
        public int MinMinutes = 0;
        public int MinSeconds = 0;
        public int MaxHours = 0;
        public int MaxMinutes = 0;
        public int MaxSeconds = 0;
        public int MinPanicHours = 0;
        public int MinPanicMinutes = 0;
        public int MinPanicSeconds = 0;
        public int MaxPanicHours = 0;
        public int MaxPanicMinutes = 0;
        public int MaxPanicSeconds = 0;
        public int FacilityId = 0;
        public string SaveData(clsTemplateFieldNumeric objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intFacilityId", objVal.FacilityId);
            HshIn.Add("@intFormatID", objVal.FormatId);
            HshIn.Add("@intFieldId", objVal.FieldId);
            HshIn.Add("@chvLimitType", objVal.LimitType);
            HshIn.Add("@intUnitId", objVal.UnitId);
            HshIn.Add("@chrSex", objVal.Sex);
            HshIn.Add("@inyAgeFrom", objVal.AgeFrom);
            HshIn.Add("@inyAgeTo", objVal.AgeTo);
            HshIn.Add("@chrAgeFromType", objVal.AgeFromType);
            HshIn.Add("@chrAgeToType", objVal.AgeToType);
            HshIn.Add("@decMinValue", objVal.MinValue);
            HshIn.Add("@decMaxValue", objVal.MaxValue);
            HshIn.Add("@chrSymbol", objVal.Symbol);
            HshIn.Add("@decMinPanicValue", objVal.MinPanicValue);
            HshIn.Add("@decMaxPanicValue", objVal.MaxPanicValue);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@intEncodedBy", objVal.userId);
            if (objVal.MachineId > 0)
            {
                HshIn.Add("@intMachineId", objVal.MachineId);
            }
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDiagSaveTemplateFieldNumeric", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveDataTime(clsTemplateFieldNumeric objVal)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intFormatID", objVal.FormatId);
            HshIn.Add("@intFieldId", objVal.FieldId);
            HshIn.Add("@chrSex", objVal.Sex);
            HshIn.Add("@inyAgeFrom", objVal.AgeFrom);
            HshIn.Add("@inyAgeTo", objVal.AgeTo);
            HshIn.Add("@chrAgeFromType", objVal.AgeFromType);
            HshIn.Add("@chrAgeToType", objVal.AgeToType);

            HshIn.Add("@inyMinHours", objVal.MinHours);
            HshIn.Add("@inyMinMinutes", objVal.MinMinutes);
            HshIn.Add("@inyMinSeconds", objVal.MinSeconds);
            HshIn.Add("@inyMaxHours", objVal.MaxHours);
            HshIn.Add("@inyMaxMinutes", objVal.MaxMinutes);
            HshIn.Add("@inyMaxSeconds", objVal.MaxSeconds);
            HshIn.Add("@chrSymbol", objVal.Symbol);
            HshIn.Add("@inyMinPanicHours", objVal.MinPanicHours);
            HshIn.Add("@inyMinPanicMinutes", objVal.MinPanicMinutes);
            HshIn.Add("@inyMinPanicSeconds", objVal.MinPanicSeconds);
            HshIn.Add("@inyMaxPanicHours", objVal.MaxPanicHours);
            HshIn.Add("@inyMaxPanicMinutes", objVal.MaxPanicMinutes);
            HshIn.Add("@inyMaxPanicSeconds", objVal.MaxPanicSeconds);
            HshIn.Add("@chvLimitType", objVal.LimitType);
            HshIn.Add("@bitActive", objVal.Active);
            HshIn.Add("@intEncodedBy", objVal.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateFieldTime", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string getFieldNameById(int ifieldID)
        {
            HshIn = new Hashtable();
            string strFieldName = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intFieldID", ifieldID);

                string strqry = "SELECT FieldName FROM DiagFields WITH (NOLOCK) WHERE FieldId = @intFieldID";

                ds = objDl.FillDataSet(CommandType.Text, strqry, HshIn);
                return strFieldName = Convert.ToString(ds.Tables[0].Rows[0]["FieldName"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
        public DataSet getUnit(int ifieldID, int LoginFacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intFieldID", ifieldID);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);

                string strqry = "SELECT dfmu.UnitId, dum.UnitName FROM DiagFields df WITH (NOLOCK) INNER JOIN DiagFacilityMethodUnit dfmu WITH (NOLOCK) ON dfmu.FieldId = df.FieldId AND dfmu.FacilityId = @intLoginFacilityId AND dfmu.Active = 1 INNER JOIN DiagUnitMaster dum WITH (NOLOCK) ON dum.UnitId = dfmu.UnitId WHERE df.FieldId = @intFieldID";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getData(int ifieldID, int iFormatId, int FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("intFieldID", ifieldID);
                HshIn.Add("intFacilityId", FacilityId);

                if (iFormatId > 0)
                {
                    HshIn.Add("intFormatID", iFormatId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateFieldNumeric", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getDataTimeField(int ifieldID, int iFormatId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("intFieldID", ifieldID);

                if (iFormatId > 0)
                {
                    HshIn.Add("intFormatID", iFormatId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateFieldTime", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int deActivateData(int fieldID, int iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@FieldId", fieldID);

                string strUpdateUser = "";
                if (iUserID > 0)
                {
                    strUpdateUser = ", EncodedBy = " + iUserID;
                }

                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE DiagFieldsNumeric SET Active = 0, " + strUpdateUser + " WHERE FieldId=@FieldId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string saveTemplateFormatSequence(string strObj, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@xmlFormatIds", strObj);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateFormatSequence", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsTemplateFieldGroup
    {
        private string sConString = "";
        public clsTemplateFieldGroup(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public string SaveData(int FieldId, string GroupFields, string TableRows, string FormulaName, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@xmlGroupFields", GroupFields);
            if (TableRows != "")
            {
                HshIn.Add("@xmlTableRows", TableRows);
            }
            HshIn.Add("@chvFormulaName", FormulaName);
            HshIn.Add("@bitActive", 1);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateFieldsGroup", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetResultAlert(int ServiceId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intServiceId", ServiceId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultAlert", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetResultAlertDescription(int FormulaId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intResultAlertId", FormulaId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultAlertDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveresultAlert(int ResultAlertId, int ServiceId, string xmlGroupFields, string chvDescription,
            int decValue, int userId, string FormulaDefinition, string active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intResultAlertId", ResultAlertId);
            HshIn.Add("@intServiceId", ServiceId);
            HshIn.Add("@xmlGroupFields", xmlGroupFields);
            HshIn.Add("@chvDescription", chvDescription);
            HshIn.Add("@chvFormulaDefinition", FormulaDefinition);
            HshIn.Add("@decValue", decValue);
            HshIn.Add("@intEncodedBy", userId);
            HshIn.Add("@bitActive", active);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveResultAlert", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getFieldsGroup(int ifieldID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("FieldId", ifieldID);

                string strqry = "SELECT SequenceNo, ReferenceName, AssociatedFieldId FROM DiagFieldsGroup WITH (NOLOCK) " +
                                " WHERE FieldId=@FieldId AND Active=1 ORDER BY SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


    }
    public class InvestigationFormat
    {
        string _sConString = "";
        Hashtable HshIn;
        public InvestigationFormat(string ConString)
        {
            _sConString = ConString;
        }
        public DataSet GetInvestigationSubDepartments()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                string strqry = "SELECT SubDeptId, ds.SubName FROM DepartmentSub ds WITH (NOLOCK) " +
                         " WHERE ds.Type IN ('I','IS') AND ds.Active = 1 AND ds.HospitalLocationID = 1 " +
                         " ORDER BY ds.subname";

                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetInvestigationServices(string subDepartmentId, string type, string HospitalLocationId, int iStationId, int FacilityId)
        {
            Hashtable HshInput = new Hashtable();

            HshInput.Add("@inyHospitalLocationId", HospitalLocationId);
            HshInput.Add("@intLabStationId", iStationId);
            HshInput.Add("@chvSubDepartmentIds", subDepartmentId);
            HshInput.Add("@chrType", type);
            HshInput.Add("intFacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", HshInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetTextTemplateField(string FieldID, int iFormatID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intFieldID", FieldID);

            if (iFormatID > 0)
            {
                HshIn.Add("@intFormatID", iFormatID);
            }
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateFieldText", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveTextTemplate(string FormatID, string FieldId, string Sex, string FormatManualCode, string Format, string EncodedBy, string Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);

            HshIn = new Hashtable();
            HshIn.Add("@intFormatID", FormatID);
            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@chrSex", Sex);
            HshIn.Add("@chvFormatManualCode", FormatManualCode);
            HshIn.Add("@txtFormat", Format);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            Hashtable hshOutput = new Hashtable();
            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateFieldText", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet getAssociatedFields(string fieldId)
        {
            StringBuilder objStr = new StringBuilder();
            HshIn = new Hashtable();
            HshIn.Add("FieldId", fieldId);

            objStr.Append(" SELECT dfg.AssociatedFieldId AS FieldId, df.FieldName, dfg.ReferenceName AS Reference FROM DiagFieldsGroup dfg WITH (NOLOCK) ");
            objStr.Append(" INNER JOIN DiagFields df WITH (NOLOCK) ON dfg.AssociatedFieldId = df.FieldId ");
            objStr.Append(" WHERE dfg.FieldId = @FieldId AND dfg.Active = 1 ");
            objStr.Append(" ORDER BY dfg.ReferenceName, df.FieldName ");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, objStr.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveTaggedRemarksService(string intRemarksId, string ServiceId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intRemarksId", intRemarksId);
                HshIn.Add("@xmlServiceIds", ServiceId);
                HshIn.Add("@intEncodedBy", UserId);

                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveServiceRemarks", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string RemoveAllTaggedRemarksService(int intRemarksId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRemarksId", intRemarksId);
                HshIn.Add("@intEncodedBy", UserId);

                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspRemoveAllTaggedRemarksService", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet getFieldNameAndFormula(string FieldId)
        {
            StringBuilder objStr = new StringBuilder();
            HshIn = new Hashtable();
            HshIn.Add("FieldId", FieldId);

            string strqry = "SELECT FieldName, FormulaDefinition FROM DiagFields WITH (NOLOCK) WHERE FieldId = @FieldId";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveUpdateTemplateServiceRemarks(int iRemarksID, int iHospId, string sRemarksName,
            string sRemarksFormat, int iActive, int iEncodedBy, int iStationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRemarksId", iRemarksID);
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@chvRemarksName", sRemarksName);
                HshIn.Add("@txtRemarksFormat", sRemarksFormat);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                Hashtable HshOut = new Hashtable();
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveServiceRemarksTemplate", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getServiceRemarksTemplate(int iHospId, int iRemarksId, int iStationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                StringBuilder objStr = new StringBuilder();
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intRemarksId", iRemarksId);
                HshIn.Add("@intStationId", iStationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetServiceRemarksTemplate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveTextTemplateMultiLine(string FormatID, string FieldId, string Sex, string FormatManualCode, string Format, string EncodedBy, string Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFormatID", FormatID);
                HshIn.Add("@intFieldId", FieldId);
                HshIn.Add("@chrSex", Sex);
                HshIn.Add("@chvFormatManualCode", FormatManualCode);
                HshIn.Add("@txtFormat", Format);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                Hashtable hshOutput = new Hashtable();
                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateFieldText", HshIn, hshOutput);

                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getServiceTaggedRemarks(string RemarksId)
        {
            StringBuilder objStr = new StringBuilder();
            HshIn = new Hashtable();
            HshIn.Add("@RemarksId", RemarksId);

            string strqry = "select DSR.ServiceId,ios.ServiceName,RemarksId from DiagServiceRemarks DSR WITH (NOLOCK) INNER JOIN ItemOfService ios WITH (NOLOCK) ON ios.ServiceId=DSR.ServiceId where RemarksId = @RemarksId AND DSR.Active=1";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
    }
    public class clsGroupValue
    {
        private string sConString = "";
        public clsGroupValue(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public string SaveValueGroupDetails(int iGroupID, int iEncodedBy, string XMLDetails,
            int iHospitalLocationId, string sGroupName, int sActive, int iStationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intGroupID", iGroupID);

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@chvGroupName", sGroupName);
                HshIn.Add("@XMLGroupValues", XMLDetails);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@bitActive", sActive);
                HshIn.Add("@intStationId", iStationId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateGroups", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTemplateGroupDetails(int @intGroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intGroupId", @intGroupId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateGroupDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int DeActivateValueGroupDetails(int iGroupID, int iValueID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@GroupID", iGroupID);
                HshIn.Add("@ValueID", iValueID);

                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE DiagGroupValueDetails SET Active = '0' WHERE GroupID=@GroupID AND ValueID = @ValueID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public int ActivateValueDetails(int iGroupID, int iValueID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@GroupID", iGroupID);
                HshIn.Add("@ValueID", iValueID);

                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE DiagGroupValueDetails SET Active = '1' WHERE GroupID=@GroupID AND ValueID = @ValueID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetTemplateGroup(int @inyHospitalLocationId, int iStationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", @inyHospitalLocationId);
                HshIn.Add("@intStationId", iStationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateGroups", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsServiceTaging
    {
        private string sConString = "";
        public clsServiceTaging(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public string SaveServiceTaging(int intServiceId, string XMLDetails, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intServiceId", intServiceId);
                HshIn.Add("@xmlFields", XMLDetails);
                HshIn.Add("@intEncodedBy", iEncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDiagSaveServiceFields", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTemplateFieldInvestigations(int intFieldId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intFieldId", intFieldId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateFieldInvestigations", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveTemplateFieldInvestigations(int intRecordId, int intFieldId, int intServiceId,
                                           int intLabFieldId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intRecordId", intRecordId);
            HshIn.Add("@intFieldId", intFieldId);
            HshIn.Add("@intServiceId", intServiceId);
            HshIn.Add("@intLabFieldId", intLabFieldId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveTemplateFieldInvestigations", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetServiceTaging(int intServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intServiceId", intServiceId);

                string strqry = " SELECT ROW_NUMBER() OVER(ORDER BY DS.SequenceNo) as ID , DS.FieldId, DF.FieldName, DS.SequenceNo, ISNULL(DF.FieldType, '') AS FieldType, DFT.Name AS ControlTypeName,DS.HeaderEndSNo,DS.HeaderStartSNo,ISNULL(DS.MandatoryForFinalRelease,0) AS MandatoryForFinalRelease  " +
                    " FROM DiagServiceFields DS WITH (NOLOCK) " +
                    " INNER JOIN DiagFields DF WITH (NOLOCK) ON DF.FieldId = DS.FieldId " +
                    " INNER JOIN DiagFieldTypes DFT WITH (NOLOCK) ON DF.FieldType = DFT.Code " +
                    " WHERE ds.ServiceId = @intServiceId AND DS.Active = 1 " +
                    " ORDER BY DS.SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
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
        public DataSet GetServiceName(int intServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intServiceId", intServiceId);

            string strqry = "SELECT ServiceId, ServiceName FROM ItemOfService " +
                            " WHERE ServiceId = @intServiceId AND Active=1";

            DataSet objds = objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            return objds;
        }
        public DataSet GetServiceTagingWithRange(int intServiceId, int iAge, string sGender, string sAgeType, int iFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intServiceId", intServiceId);
                HshIn.Add("@intAge", iAge);
                HshIn.Add("@chvGender", sGender);
                HshIn.Add("@chvAgeType", sAgeType);
                HshIn.Add("@intFacilityId", iFacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetServiceTagingWithRange", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetServiceDetail(int intServiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intServiceId", intServiceId);

                string strqry = "SELECT ServiceId, ServiceName FROM ItemOfService WITH (NOLOCK) WHERE ServiceId = @intServiceId AND Active=1";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetFieldGroupsID(int intFieldId, string ToCheckFieldId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFieldId", intFieldId);
                HshIn.Add("@xmlFields", ToCheckFieldId);

                ////string strqry = "SELECT DISTINCT FieldId FROM DiagFieldsGroup " +
                ////            " WHERE FieldId =@intFieldId  AND Active = 1 ";

                //string strqry = "SELECT  FieldId FROM DiagFieldsGroup WHERE AssociatedFieldId =@intFieldId AND FieldId  in (@toCheckFieldId) AND Active = 1";

                //return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAssociatedGroup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
    public class clsFieldType
    {
        private string sConString = "";
        public clsFieldType(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public string SaveFieldType(int intParentId, int intSubField, int intFieldId, int intParentValue,
                         int iHospitalLocationId, string chvCode, string chvFieldName, string chvDisplayName, string chrFieldType,
                         string chrGender, int intUnitId, int intUnitIdSI, string ConversionFormula,
                         string chvLimitType, int intGroupId, int inyMaxLength, int intEncodedBy, int bitActive,
                         int inyTableColumns, int intTableRows, bool chkCheckMachine, bool bitDiaplyInReport,
                         bool bitEditFormulaField, int iStationId, int intInvestigationMethodId,
                         int DigitsAfterDecimal, int SampleId, string SpecialReferenceRange, bool bitShowAfterResultFinalization, int Facilityid)
        {
            return SaveFieldType(intParentId, intSubField, intFieldId, intParentValue,
                         iHospitalLocationId, chvCode, chvFieldName, chvDisplayName, chrFieldType,
                         chrGender, intUnitId, intUnitIdSI, ConversionFormula,
                         chvLimitType, intGroupId, inyMaxLength, intEncodedBy, bitActive,
                         inyTableColumns, intTableRows, chkCheckMachine, bitDiaplyInReport,
                         bitEditFormulaField, iStationId, intInvestigationMethodId,
                         DigitsAfterDecimal, SampleId, SpecialReferenceRange, bitShowAfterResultFinalization, Facilityid, false);

        }
        public string SaveFieldType(int intParentId, int intSubField, int intFieldId, int intParentValue,
                          int iHospitalLocationId, string chvCode, string chvFieldName, string chvDisplayName, string chrFieldType,
                          string chrGender, int intUnitId, int intUnitIdSI, string ConversionFormula,
                          string chvLimitType, int intGroupId, int inyMaxLength, int intEncodedBy, int bitActive,
                          int inyTableColumns, int intTableRows, bool chkCheckMachine, bool bitDiaplyInReport,
                          bool bitEditFormulaField, int iStationId, int intInvestigationMethodId,
                          int DigitsAfterDecimal, int SampleId, string SpecialReferenceRange, bool bitShowAfterResultFinalization, int Facilityid, bool bitIsAddendum)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intFacilityId", Facilityid);
                if (intParentId != 0)
                {
                    HshIn.Add("@intParentId", intParentId);
                }

                HshIn.Add("@bitSubField", intSubField);  // 0-> Main,  New
                if (intFieldId != 0)
                {
                    HshIn.Add("@intFieldId", intFieldId);
                }

                if (intParentValue != 0)
                {
                    HshIn.Add("@intParentValue", intParentValue);
                }

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);

                HshIn.Add("@chvCode", chvCode);
                HshIn.Add("@chvFieldName", chvFieldName);
                HshIn.Add("@chvDisplayName", chvDisplayName);
                HshIn.Add("@chrFieldType", chrFieldType);
                HshIn.Add("@intStationId", iStationId);
                if (intUnitId > 0)
                {
                    HshIn.Add("@intUnitId", intUnitId);
                }
                if (intUnitIdSI > 0)
                {
                    HshIn.Add("@intUnitIdSI", intUnitIdSI);
                }
                if (ConversionFormula != "")
                {
                    HshIn.Add("@chvConversionFormula", ConversionFormula);
                }

                HshIn.Add("@chvLimitType", chvLimitType);
                if (intGroupId != 0)
                {
                    HshIn.Add("@intGroupId", intGroupId);
                }
                if (intInvestigationMethodId != 0)
                {
                    HshIn.Add("@intInvestigationMethodId", intInvestigationMethodId);
                }

                HshIn.Add("@inyMaxLength", inyMaxLength);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@bitActive", bitActive);
                if (inyTableColumns != 0)
                {
                    HshIn.Add("@inyTableColumns", inyTableColumns);
                }
                if (intTableRows != 0)
                {
                    HshIn.Add("@intTableRows", intTableRows);
                }
                HshIn.Add("@bitLimitMachineWise", chkCheckMachine);
                HshIn.Add("@bitNotDisplayFNInReport", bitDiaplyInReport);
                HshIn.Add("@bitEditFormulaField", bitEditFormulaField);

                HshIn.Add("@inyDigitsAfterDecimal", DigitsAfterDecimal);
                HshIn.Add("@intSampleId", SampleId);
                HshIn.Add("@chvSpecialReferenceRange", SpecialReferenceRange);
                HshIn.Add("@bitShowAfterResultFinalization", bitShowAfterResultFinalization);
                HshIn.Add("@bitIsAddendum", bitIsAddendum);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveTemplateField", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetFieldType()
        {
            string strqry = "SELECT RTRIM(Name) AS Name, Code FROM DiagFieldTypes WITH (NOLOCK) ORDER BY SequenceNo";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, strqry);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetGroupValue(int intHospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);

                string strqry = "SELECT GroupName, GroupID FROM DiagGroupValueMain WITH (NOLOCK) " +
                            " WHERE HospitalLocationId=@intHospitalLocationId AND Active=1 ORDER BY GroupName ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetSubField(int intHospitalLocationId, int intFieldID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intFieldID", intFieldID);

                string strqry = "SELECT FieldID, Fieldname FROM DiagFields WITH (NOLOCK) " +
                        " WHERE ParentID=@intFieldID AND Active=1 AND HospitalLocationId=@intHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetFieldValue(int intHospitalLocationId, int intFieldID, string chvSearchString,
            int isActive, int iStationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", intHospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                if (intFieldID > 0)
                {
                    HshIn.Add("@intFieldID", intFieldID);
                }
                if (chvSearchString != "")
                {
                    HshIn.Add("@chvSearchString", "%" + chvSearchString + "%");
                }

                HshIn.Add("@bitActive", isActive);
                HshIn.Add("@intStationId", iStationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTemplateFields", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFieldService(int iFieldID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@FieldID", iFieldID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetFieldService", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetGroupService(int groupID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intGroupId", groupID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetGroupFields", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetValueTaggedGroup(int ValueID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intValueId", ValueID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetValueGroups", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsOtherServiceDetails
    {
        private string sConString = "";
        public clsOtherServiceDetails(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public DataSet GetVacutainer(int iVacutainerId, int iHospitalLocationId, string strSelectedServices)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            if (iVacutainerId != 0)
            {
                HshIn.Add("@intVacutainerId", iVacutainerId);
            }
            if (strSelectedServices != "")
            {
                HshIn.Add("@chvServiceIds", strSelectedServices);
            }
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);

            DataSet objds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetVacutainerMaster", HshIn);
            return objds;
        }
        public string SaveVacutainer(int iVacutainerId, int iHospitalLocationId, string sVacutainerName, int iVialColorId, int sActive, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                if (iVacutainerId != 0)
                {
                    HshIn.Add("@intVacutainerId", iVacutainerId);
                }
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@chvVacutainerName", sVacutainerName);
                HshIn.Add("@intVialColorId", iVialColorId);
                HshIn.Add("@bitActive", sActive);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDiagSaveVacutainerMaster", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetVacutainer(int iVacutainerId, int iHospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                if (iVacutainerId != 0)
                {
                    HshIn.Add("@intVacutainerId", iVacutainerId);
                }
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetVacutainerMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetColor()
        {
            string strqry = "SELECT ColorId, ColorName FROM ColorMaster WITH (NOLOCK) ORDER BY ColorName ";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, strqry);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetOtherServiceDetails(string XMLServiceId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@XMLServiceId", XMLServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetServiceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveOtherDetails(int intServiceId, int intSampleId, decimal decSampleQuantity, int intSampleUnitId,
                                    bool bitPrintServiceNameInReport, bool bitMultipleResultEntry, bool bitMultipleLabDeviceResult,
                                    int intVacutainerId, string chvInstructionForPhlebotomy, string chvInstructionForFrontDesk,
                                    string chvTestDays, int iEncodedBy, string XMLExternalCenter, string statTo, string stattotype,
                                    string statfrom, string statfromtype, string criticalto, string criticaltotype, string criticalFrom,
                                    string criticalfromtype, string routineto, string routinetotype, string routinefrom,
                                    string routinefromtype, bool ExcludeWeeklyOffOPRoutineTAT, bool ExcludeWeeklyOffIPRoutineTAT,
                                    bool bitHorizontalView, bool bitResultAlert, bool isResultHTML, bool ShowTextFormatInPopupPage,
                                    int ProvisionalReportCommentId, int FinalReportCommentId, int ServiceReportHeaderId,
                                    int ReportTypeId, int DefaultRemarksId)
        {
            return SaveOtherDetails(intServiceId, intSampleId, decSampleQuantity, intSampleUnitId,
                                     bitPrintServiceNameInReport, bitMultipleResultEntry, bitMultipleLabDeviceResult,
                                     intVacutainerId, chvInstructionForPhlebotomy, chvInstructionForFrontDesk,
                                     chvTestDays, iEncodedBy, XMLExternalCenter, statTo, stattotype,
                                     statfrom, statfromtype, criticalto, criticaltotype, criticalFrom,
                                     criticalfromtype, routineto, routinetotype, routinefrom,
                                     routinefromtype, ExcludeWeeklyOffOPRoutineTAT, ExcludeWeeklyOffIPRoutineTAT,
                                     bitHorizontalView, bitResultAlert, isResultHTML, ShowTextFormatInPopupPage,
                                     ProvisionalReportCommentId, FinalReportCommentId, ServiceReportHeaderId, ReportTypeId, DefaultRemarksId, false);
        }

        public string SaveOtherDetails(int intServiceId, int intSampleId, decimal decSampleQuantity, int intSampleUnitId,
                                    bool bitPrintServiceNameInReport, bool bitMultipleResultEntry, bool bitMultipleLabDeviceResult,
                                    int intVacutainerId, string chvInstructionForPhlebotomy, string chvInstructionForFrontDesk,
                                    string chvTestDays, int iEncodedBy, string XMLExternalCenter, string statTo, string stattotype,
                                    string statfrom, string statfromtype, string criticalto, string criticaltotype, string criticalFrom,
                                    string criticalfromtype, string routineto, string routinetotype, string routinefrom,
                                    string routinefromtype, bool ExcludeWeeklyOffOPRoutineTAT, bool ExcludeWeeklyOffIPRoutineTAT,
                                    bool bitHorizontalView, bool bitResultAlert, bool isResultHTML, bool ShowTextFormatInPopupPage,
                                    int ProvisionalReportCommentId, int FinalReportCommentId, int ServiceReportHeaderId, int ReportTypeId,
                                    int DefaultRemarksId, bool PrintReferenceRangeInHTML)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intServiceId", intServiceId);
                //if (intSampleId != 0)
                //{
                //    HshIn.Add("@intSampleId", intSampleId);
                //}
                //else
                //{
                //    HshIn.Add("@intSampleId", DBNull.Value);
                //}
                if (decSampleQuantity != 0)
                {
                    HshIn.Add("@decSampleQuantity", decSampleQuantity);
                }
                else
                {
                    HshIn.Add("@decSampleQuantity", DBNull.Value);
                }
                //if (intSampleUnitId != 0)
                //{
                //    HshIn.Add("@intSampleUnitId", intSampleUnitId);
                //}
                //else
                //{
                //    HshIn.Add("@intSampleUnitId", DBNull.Value);
                //}
                HshIn.Add("@bitPrintServiceNameInReport", bitPrintServiceNameInReport);
                HshIn.Add("@bitMultipleResultEntry", bitMultipleResultEntry);
                HshIn.Add("@bitMultipleLabDeviceResult", bitMultipleLabDeviceResult);
                if (intVacutainerId != 0)
                {
                    HshIn.Add("@intVacutainerId", intVacutainerId);
                }
                else
                {
                    HshIn.Add("@intVacutainerId", DBNull.Value);
                }
                if (chvInstructionForPhlebotomy != "")
                {
                    HshIn.Add("@chvInstructionForPhlebotomy", chvInstructionForPhlebotomy);
                }
                else
                {
                    HshIn.Add("@chvInstructionForPhlebotomy", DBNull.Value);
                }
                if (chvInstructionForFrontDesk != "")
                {
                    HshIn.Add("@chvInstructionForFrontDesk", chvInstructionForFrontDesk);
                }
                else
                {
                    HshIn.Add("@chvInstructionForFrontDesk", DBNull.Value);
                }

                if (chvTestDays != "")
                {
                    HshIn.Add("@chvTestDays", chvTestDays);
                }
                else
                {
                    HshIn.Add("@chvTestDays", DBNull.Value);
                }
                HshIn.Add("@intEncodedBy", iEncodedBy);

                if (XMLExternalCenter != "")
                {
                    HshIn.Add("@XMLExternalCenter", XMLExternalCenter);
                }
                else
                {
                    HshIn.Add("@XMLExternalCenter", DBNull.Value);
                }


                if ((statfrom != "") && (statfrom != "0"))
                {
                    HshIn.Add("@inyStatDurationFrom", statfrom);
                    HshIn.Add("@chrStatDurationFromType", statfromtype);
                }
                else
                {
                    HshIn.Add("@inyStatDurationFrom", 0);
                    HshIn.Add("@chrStatDurationFromType", DBNull.Value);
                }

                if ((statfrom != "") && (statfrom != "0"))
                {
                    HshIn.Add("@inyStatDurationTo", statTo);
                    HshIn.Add("@chrStatDurationToType", stattotype);
                }
                else
                {
                    HshIn.Add("@inyStatDurationTo", 0);
                    HshIn.Add("@chrStatDurationToType", DBNull.Value);
                }

                if ((routinefrom != "") && (routinefrom != "0"))
                {
                    HshIn.Add("@inyRoutineDurationFrom", routinefrom);
                    HshIn.Add("@chrRoutineDurationFromType", routinefromtype);
                }
                else
                {
                    HshIn.Add("@inyRoutineDurationFrom", 0);
                    HshIn.Add("@chrRoutineDurationFromType", DBNull.Value);
                }

                if ((routineto != "") && (routineto != "0"))
                {
                    HshIn.Add("@inyRoutineDurationTo", routineto);
                    HshIn.Add("@chrRoutineDurationToType", routinetotype);
                }
                else
                {
                    HshIn.Add("@inyRoutineDurationTo", 0);
                    HshIn.Add("@chrRoutineDurationToType", DBNull.Value);
                }


                if ((criticalFrom != "") && (criticalFrom != "0"))
                {
                    HshIn.Add("@inyCriticalDurationFrom", criticalFrom);
                    HshIn.Add("@chrCriticalDurationFromType", criticalfromtype);
                }
                else
                {
                    HshIn.Add("@inyCriticalDurationFrom", 0);
                    HshIn.Add("@chrCriticalDurationFromType", DBNull.Value);
                }

                if ((criticalto != "") && (criticalto != "0"))
                {
                    HshIn.Add("@inyCriticalDurationTo", criticalto);
                    HshIn.Add("@chrCriticalDurationToType", criticaltotype);
                }
                else
                {
                    HshIn.Add("@inyCriticalDurationTo", 0);
                    HshIn.Add("@chrCriticalDurationToType", DBNull.Value);
                }

                HshIn.Add("@bitExcludeWeeklyOffOPRoutineTAT", ExcludeWeeklyOffOPRoutineTAT);
                HshIn.Add("@bitExcludeWeeklyOffIPRoutineTAT", ExcludeWeeklyOffIPRoutineTAT);
                HshIn.Add("@bitHorizontalView", bitHorizontalView);
                HshIn.Add("@bitResultAlert", bitResultAlert);
                HshIn.Add("@bitResultHTML", isResultHTML);
                HshIn.Add("@bitchkShowTextFormatInPopupPage", ShowTextFormatInPopupPage);


                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshIn.Add("@intProvisionalReportCommentId", ProvisionalReportCommentId);
                HshIn.Add("@intFinalReportCommentId", FinalReportCommentId);
                HshIn.Add("@intServiceReportHeaderId", ServiceReportHeaderId);
                HshIn.Add("@intReportTypeId", ReportTypeId);
                HshIn.Add("@PrintReferenceRangeInHTML", PrintReferenceRangeInHTML);

                if (DefaultRemarksId > 0)
                {
                    HshIn.Add("@intDefaultRemarksId", DefaultRemarksId);
                }

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveServiceDetails", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
    }
    public class clsEnzyme
    {
        private string sConString = "";
        public clsEnzyme(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int EnzymeId = 0;
        public long HospitalLocationID = 0;
        public string EnzymeName = "";
        public int Active = 1;
        public int userId = 0;
        public string SaveData(clsEnzyme objEnzyme)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", objEnzyme.HospitalLocationID);
            HshIn.Add("@intEnzymeId", objEnzyme.EnzymeId);
            HshIn.Add("@chvEnzymeName", objEnzyme.EnzymeName);
            HshIn.Add("@bitActive", objEnzyme.Active);
            HshIn.Add("@intEncodedBy", objEnzyme.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveEnzyme", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iHospID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationID", iHospID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetEnzymes", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsOrganism
    {
        private string sConString = "";
        public clsOrganism(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int OrganismId = 0;
        public long HospitalLocationID = 0;
        public string OrganismName = "";
        public int Active = 1;
        public int userId = 0;
        public string xmlOrganismIds = "";
        public int ServiceId = 0;
        public string SaveData(clsOrganism objOrganism)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", objOrganism.HospitalLocationID);
            HshIn.Add("@intOrganismId", objOrganism.OrganismId);
            HshIn.Add("@chvOrganismName", objOrganism.OrganismName);
            HshIn.Add("@bitActive", objOrganism.Active);
            HshIn.Add("@intEncodedBy", objOrganism.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveOrganism", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iHospID, int iOrganismId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@intOrganismId", iOrganismId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetOrganism", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable getDataOrganismService(string iServiceId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intServiceId", iServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetOrganismService", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveDataOrganismService(clsOrganism objOrganism)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intServiceId", objOrganism.ServiceId);
            HshIn.Add("@xmlOrganismIds", objOrganism.xmlOrganismIds);
            HshIn.Add("@intEncodedBy", objOrganism.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveOrganismService", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsAntibiotic
    {
        private string sConString = "";
        public clsAntibiotic(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;

        public int AntibioticId = 0;
        public long HospitalLocationID = 0;
        public string AntibioticName = "";
        public string xmlAntibioticIds = "";
        public int Active = 1;
        public int userId = 0;

        public string SaveData(clsAntibiotic objAntibiotic)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", objAntibiotic.HospitalLocationID);
            HshIn.Add("@intAntibioticId", objAntibiotic.AntibioticId);
            HshIn.Add("@chvAntibioticName", objAntibiotic.AntibioticName);
            HshIn.Add("@bitActive", objAntibiotic.Active);
            HshIn.Add("@intEncodedBy", objAntibiotic.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveAntibiotic", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpadeteAntibioticSequence(clsAntibiotic objAntibiotic)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@xmlAntibioticIds", objAntibiotic.xmlAntibioticIds);
            HshIn.Add("@intEncodedBy", objAntibiotic.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagUpdateAntibioticSequence", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getData(int iHospID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationID", iHospID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetAntibiotics", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
    public class clsOrganismAntibiotic
    {
        private string sConString = "";
        public clsOrganismAntibiotic(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int OrganismId = 0;
        public string xmlAntibioticIds = "";
        public int userId = 0;
        public string SaveData(clsOrganismAntibiotic objOrganismAntibiotic)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intOrganismId", objOrganismAntibiotic.OrganismId);
            HshIn.Add("@xmlAntibioticIds", objOrganismAntibiotic.xmlAntibioticIds);
            HshIn.Add("@intEncodedBy", objOrganismAntibiotic.userId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveOrganismAntibiotics", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getData(int iOrganismId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intOrganismId", iOrganismId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetOrganismAntibiotics", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
    }
    public class clsEntrySite
    {
        private string sConString = "";
        public clsEntrySite(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;

    }



}
