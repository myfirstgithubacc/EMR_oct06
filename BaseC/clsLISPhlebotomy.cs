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
    public class clsLISPhlebotomy
    {
        Hashtable HshIn;
        Hashtable HshOut;
        private string sConString = "";
        DAL.DAL objDl = null;

        public clsLISPhlebotomy(string conString)
        {
            sConString = conString;
        }
        public DataSet getInvSampleMainData(string Source, int iHospID, int iFacilityId, int iLoginFacilityId,
                       int iStationId, int iStatusId, int iLabNo, string sTestPriority,
                       DateTime dtsDateFrom, DateTime dtsDateTo, int iPageSize, int iPageNo,
            string sRegistrationNo, string sEncounterNo, string sPatientName, string bedNo, string WardNo,
            int Mannualrequest, int inyEntrySiteId, int intServiceId, int intSubDeptId, int bitERRequest,
            string ManualLabNo, int ReportTypeId, string PrintStatus, string ReviewStatus, int intEntrySiteActual, bool intOutsourceInv)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intStatusId", iStatusId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@dtsDateFrom", fDate);
                HshIn.Add("@dtsDateTo", tDate);
                HshIn.Add("@inyPageSize", iPageSize);
                HshIn.Add("@intPageNo", iPageNo);
                HshIn.Add("@bitManualRequest", Mannualrequest);
                HshIn.Add("@intServiceId", intServiceId);
                HshIn.Add("@intSubDeptId", intSubDeptId);
                HshIn.Add("@intReportTypeId", ReportTypeId);
                HshIn.Add("@intEntrySite", intEntrySiteActual);


                if (sTestPriority != "")
                {
                    HshIn.Add("@chrTestPriority", sTestPriority);
                }
                if (ManualLabNo != "")
                {
                    HshIn.Add("@chvManualLabNo", ManualLabNo);
                }

                if (sEncounterNo != "")
                {
                    HshIn.Add("@chvEncounterNo", sEncounterNo);
                }
                if (sRegistrationNo != "")
                {
                    HshIn.Add("@chvRegistrationNo", sRegistrationNo);
                }
                if (sPatientName != "")
                {
                    HshIn.Add("@chvPatientName", "%" + sPatientName + "%");
                }
                if (PrintStatus != "")
                {
                    HshIn.Add("@PrintStatus", PrintStatus);
                }
                if (ReviewStatus != "")
                {
                    HshIn.Add("@ReviewStatus", ReviewStatus);
                }
                HshIn.Add("@bitOutsourceInv", intOutsourceInv);
                //@chvManualLabNo

                if ((Source == "OPD") || (Source == "ER"))
                {
                    HshIn.Add("@bitERRequest", bitERRequest);
                    HshIn.Add("@inyEntrySiteId", inyEntrySiteId);

                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainOP", HshIn);
                }
                else if (Source == "IPD")
                {
                    if (bedNo != "")
                    {
                        HshIn.Add("@chvbedNo", bedNo);
                    }
                    if (WardNo != "")
                    {
                        HshIn.Add("@chvWardNo", WardNo);
                    }
                    HshIn.Add("@inyEntrySiteId", inyEntrySiteId);

                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainIP", HshIn);
                }
                else if (Source == "PACKAGE")
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainPackage", HshIn);
                }
                else //All
                {
                    if (bedNo != "")
                    {
                        HshIn.Add("@chvbedNo", bedNo);
                    }
                    if (WardNo != "")
                    {
                        HshIn.Add("@chvWardNo", WardNo);
                    }
                    HshIn.Add("@bitERRequest", bitERRequest);
                    HshIn.Add("@inyEntrySiteId", inyEntrySiteId);
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainAll", HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getInvSamplesDetailData(int iDiagSampleId, int iHospID, string Source, int iLabNo, int iStatusId, string sTestPriority, int iExternalCenterOnly, int iStationId, int iLoginFacilityId, string ManualLabNo, string PrintStatus, int iEntrySite, int intSubDeptId, bool bitOutsourceInv)
        {
            DataView dv = new DataView();
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            DataSet dsFinal = new DataSet();

            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intStatusId", iStatusId);
                HshIn.Add("@intExternalCenterId", iExternalCenterOnly);
                HshIn.Add("@chvManualLabNo", ManualLabNo);
                HshIn.Add("@intSubDeptId", intSubDeptId);
                HshIn.Add("@bitOutsourceInv", bitOutsourceInv);
                if (PrintStatus != "")
                {
                    HshIn.Add("@PrintStatus", PrintStatus);
                }
                if (sTestPriority != "")
                {
                    HshIn.Add("@chrTestPriority", sTestPriority);
                }

                if (Source == "IPD")
                {
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesDetailIP", HshIn);
                }
                else
                {
                    HshIn.Add("@inyEntrySiteId", iEntrySite);
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesDetailOP", HshIn);
                    dv = ds.Tables[0].Copy().DefaultView;
                    if (Source == "Package")
                    {
                        dv.RowFilter = "ISNULL(PackageId,0)<>0";

                        dsFinal.Tables.Add(dv.ToTable());

                    }
                    else
                    {
                        dv.RowFilter = "ISNULL(PackageId,0)=0";

                        dsFinal.Tables.Add(dv.ToTable());
                    }
                }

                if (iDiagSampleId > 0)
                {
                    dv = ds.Tables[0].Copy().DefaultView;
                    dv.RowFilter = "DiagSampleId=" + iDiagSampleId;

                    dsFinal.Tables.Add(dv.ToTable());
                }

                if (dsFinal.Tables.Count == 0)
                {
                    dsFinal = ds.Copy();
                }

                return dsFinal;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getInvSampleMainExternalData(string Source, int iHospID, int iFacilityId, int iLoginFacilityId,
                        int iStationId, int iStatusId, DateTime dtsDateFrom, DateTime dtsDateTo,
            int iExternalCenterId, int Labno, string MRNo, int PageSize, int PageIndex, int ManualRequest, string ManualLabNo, int EntrySiteActual)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {


                string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intStatusId", iStatusId);
                HshIn.Add("@intExternalCenterId", iExternalCenterId);
                HshIn.Add("@dtsDateFrom", fDate);
                HshIn.Add("@dtsDateTo", tDate);
                HshIn.Add("@intLabNo", Labno);
                HshIn.Add("@chvRegistrationNo", MRNo);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageIndex);
                HshIn.Add("@bitManualRequest", ManualRequest);
                HshIn.Add("@chvManualLabNo", ManualLabNo);
                HshIn.Add("@intEntrySite", EntrySiteActual);




                if (Source == "OPD")
                {
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainOPExternal", HshIn);
                }
                else if (Source == "IPD")
                {
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainIPExternal", HshIn);
                }
                else
                {
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesMainAllExternal", HshIn);
                }

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return ds;
        }
        public DataTable GetVacutainerDetails(string xmlService)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@xmlServices", xmlService);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetVacutainerDetails", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getStatus(int iHospID, string statusType, string Code)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospId", iHospID);
                HshIn.Add("@chvStatusType", statusType);

                string strqry = "SELECT Status, StatusColor, StatusId, Code FROM GetStatus(@intHospId, @chvStatusType) ";
                if (Code != string.Empty)
                {
                    HshIn.Add("@chvCode", Code);
                    strqry += " WHERE Code=@chvCode ";
                }
                strqry += " ORDER BY SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string saveSampleCollectionData(string Source, string xmlSampleIds, int iLoginFacilityId, DateTime currentDate,
                                      int iHospId, int iSampleCollectedBy, int userId, out int DailySerialNo, string xmlVacutainer, string IsPregnant)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {

                HshIn.Add("@xmlDiagSampleIds", xmlSampleIds);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@dtsCurrentDate", currentDate);
                HshIn.Add("@intSampleCollectedBy", iSampleCollectedBy);
                HshIn.Add("@intEncodedBy", userId);
                HshIn.Add("@xmlVacutainerIds", xmlVacutainer);
                HshIn.Add("@IsPregnant", IsPregnant);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@intDailySerialNo", SqlDbType.Int);

                if (Source == "IPD")
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleCollectionIP", HshIn, HshOut);
                }
                else
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleCollectionOP", HshIn, HshOut);
                }
                DailySerialNo = Convert.ToInt32(HshOut["@intDailySerialNo"]);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                HshOut = null;
            }
        }

        //public string saveSampleCollectionData(string Source, string xmlSampleIds, int iLoginFacilityId, DateTime currentDate,
        //                                int iHospId, int iSampleCollectedBy, int userId, bool FromWard, out int DailySerialNo)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    HshIn.Add("@xmlDiagSampleIds", xmlSampleIds);
        //    HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
        //    HshIn.Add("@dtsCurrentDate", currentDate);
        //    HshIn.Add("@intSampleCollectedBy", iSampleCollectedBy);
        //    HshIn.Add("@intEncodedBy", userId);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //    HshOut.Add("@intDailySerialNo", SqlDbType.Int);

        //    objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
        //    {


        //        if (Source == "IPD")
        //        {
        //            HshIn.Add("@bitFromWard", FromWard);
        //            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleCollectionIP", HshIn, HshOut);
        //        }
        //        else
        //        {
        //            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleCollectionOP", HshIn, HshOut);
        //        }
        //        DailySerialNo = Convert.ToInt32(HshOut["@intDailySerialNo"]);
        //        return HshOut["@chvErrorStatus"].ToString();
        //    }
        //    catch (Exception ex) { throw ex; }
        //    finally { HshIn = null; objDl = null; }

        //}
        public DataSet getInvSamplesDispatchData(int iHospID, int iFacilityId, int iStationId, int LabNo, string ManualLabNo, string sRegistrationNo,
                                string sEncounterNo, string sPatientName, string bedNo, string WardNo, int SubDeptId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intLabNo", LabNo);
                HshIn.Add("@chvManualLabNo", ManualLabNo);
                HshIn.Add("@sRegistrationNo", sRegistrationNo);
                HshIn.Add("@sEncounterNo", sEncounterNo);
                HshIn.Add("@sPatientName", sPatientName);
                HshIn.Add("@bedNo", bedNo);
                HshIn.Add("@WardNo", WardNo);
                HshIn.Add("@intSubDeptId", SubDeptId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesForDispatch", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string saveSampleDispatchData(int iHospID, int iStationId, int iEntrySiteId,
            string xmlDiagSampleIds, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@inyEntrySiteId", iEntrySiteId);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleDispatch", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getInvSamplesDispatchedDetail(int iHospitalId, int iStationId, int iEntrySiteId,
             int iLoginFacilityId, DateTime dtsDateFrom, DateTime dtsDateTo, int LabNo,
             string ManualLabNo, string chvEncounterNo, string chvRegistrationNo, int SubDeptId, int EntrySiteActual, string TestPriority)
        {
            string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospitalId);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@inyEntrySiteId", iEntrySiteId);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@dtsFromDate", fDate);
            HshIn.Add("@dtsToDate", tDate);
            HshIn.Add("@intLabNo", LabNo);
            HshIn.Add("@chvRegistrationNo", chvRegistrationNo);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@chvEncounterNo", chvEncounterNo);
            HshIn.Add("@intSubDeptId", SubDeptId);
            HshIn.Add("@intEntrySite", EntrySiteActual);
            HshIn.Add("@chrTestPriority", TestPriority);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesDispatchedDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getInvSamplesAfterAckDetail(int iHospitalId, int iStationId, int iEntrySiteId,
             int iLoginFacilityId, DateTime dtsDateFrom, DateTime dtsDateTo, int LabNo,
             string ManualLabNo, string chvEncounterNo, string chvRegistrationNo, int SubDeptId, int EntrySiteActual, string TestPriority)
        {
            string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospitalId);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@inyEntrySiteId", iEntrySiteId);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@dtsFromDate", fDate);
            HshIn.Add("@dtsToDate", tDate);
            HshIn.Add("@intLabNo", LabNo);
            HshIn.Add("@chvRegistrationNo", chvRegistrationNo);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@chvEncounterNo", chvEncounterNo);
            HshIn.Add("@intSubDeptId", SubDeptId);
            HshIn.Add("@intEntrySite", EntrySiteActual);
            HshIn.Add("@chrTestPriority", TestPriority);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesAck", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getInvSamplesDispatchedDetail(int iHospitalId, int iStationId, int iEntrySiteId,
            int iLoginFacilityId, DateTime dtsDateFrom, DateTime dtsDateTo, int LabNo,
            string ManualLabNo, string chvEncounterNo, string chvRegistrationNo, int SubDeptId, int EntrySiteActual)
        {
            string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospitalId);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@inyEntrySiteId", iEntrySiteId);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@dtsFromDate", fDate);
            HshIn.Add("@dtsToDate", tDate);
            HshIn.Add("@intLabNo", LabNo);
            HshIn.Add("@chvRegistrationNo", chvRegistrationNo);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@chvEncounterNo", chvEncounterNo);
            HshIn.Add("@intSubDeptId", SubDeptId);
            HshIn.Add("@intEntrySite", EntrySiteActual);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvSamplesDispatchedDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getProcedureAcknowledgement(int StationId, int HospId, int LoginFacilityId,
                                string dtsDateFrom, string dtsDateTo, string RegistrationNo,
                                string ServiceName, string PatientName, int StatusId)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@dtsFromDate", dtsDateFrom);
                HshIn.Add("@dtsToDate", dtsDateTo);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvServiceName", ServiceName);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@intStatusId", StatusId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetProcedureAcknowledgement", HshIn);

                DataView DV = ds.Tables[0].Copy().DefaultView;
                DV.Sort = "Source, OrderDate DESC, PatientName";


                ds.Tables.Add(DV.ToTable());
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return ds;
        }

        public string updateProcedureAcknowledgement(int ProcedureAcknowledged, int ProcedureAcknowledgedBy,
                                                 string xmlProcedureIds, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@bitProcedureAcknowledged", ProcedureAcknowledged);
            HshIn.Add("@intProcedureAcknowledgedBy", ProcedureAcknowledgedBy);
            HshIn.Add("@xmlProcedureIds", xmlProcedureIds);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateProcedureAcknowledgement", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string updateProviderServiceOrderDetail(int ProviderId, string xmlServiceIds, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intProviderId", ProviderId);
            HshIn.Add("@xmlServiceIds", xmlServiceIds);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateProviderServiceOrderDetail", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
          ;
        }

        public DataSet getLabDashboardDetails(string sSource, int iFacilityId, int iLoginFacilityId, DateTime dtsDateFrom, DateTime dtsDateTo, int iStationId, string sStatusCode, int iStat, int iLabNo, int EntrySiteId, int bitERRequest, int intSubDeptId, string chvDateCriteria, int RegistrationID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", sSource);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@dtFromDate", dtsDateFrom);
                HshIn.Add("@dtToDate", dtsDateTo);
                HshIn.Add("@chvStatusCode", sStatusCode);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@inyEntrySiteId", EntrySiteId);
                HshIn.Add("@bitStat", iStat);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@bitERRequest", bitERRequest);
                HshIn.Add("@intSubDeptId", intSubDeptId);
                HshIn.Add("@chvDateCriteria", chvDateCriteria);
                HshIn.Add("@intRegistrationNo", RegistrationID);



                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabDashboardDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // Added by niraj start 24 Nov 
        public DataSet getLabDashboardDetails(string sSource, int iFacilityId, int iLoginFacilityId, DateTime dtsDateFrom, DateTime dtsDateTo, int iStationId, string sStatusCode, int iStat, int iLabNo, int EntrySiteId, int bitERRequest, int intSubDeptId, string chvDateCriteria, int RegistrationID, string strFacility, string strStationId, int iServiceId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", sSource);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@dtFromDate", dtsDateFrom);
                HshIn.Add("@dtToDate", dtsDateTo);
                HshIn.Add("@chvStatusCode", sStatusCode);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@inyEntrySiteId", EntrySiteId);
                HshIn.Add("@bitStat", iStat);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@bitERRequest", bitERRequest);
                HshIn.Add("@intSubDeptId", intSubDeptId);
                HshIn.Add("@chvDateCriteria", chvDateCriteria);
                HshIn.Add("@intRegistrationNo", RegistrationID);
                HshIn.Add("@xmlStationId", strStationId);
                HshIn.Add("@xmlFacilityId", strFacility);
                HshIn.Add("@intServiceId", iServiceId);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabDashboardDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string cancelInvSamplesDispatchData(int iHospID, string xmlString)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@xmlDiagSampleIds", xmlString);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagCancelInvSamplesDispatch", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEntrySitesFromPhlebotomy(int iHospID, int iFacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@iFacility", iFacilityId);
            string strqry = "SELECT DISTINCT des.EntrySiteId, des.EntrySiteName, des.FacilityId, f.Name FacilityName, CASE WHEN desw.WardId IS NULL THEN '' ELSE 'Y' END AS 'IPEntrySite', des.IsMainSampleReceivingLocation, des.IsResultEntryLocation, dsrs.LabType " +
            "FROM DiagEntrySites des WITH(NOLOCK)" +
            "INNER JOIN DiagStationEntrySites dses WITH(NOLOCK) ON des.EntrySiteId = dses.EntrySiteId AND dses.Active = 1" +
            "INNER JOIN DiagSampleReceivingStation dsrs WITH(NOLOCK)ON dsrs.StationId = dses.StationId AND dsrs.Active = 1" +
            "LEFT JOIN DiagEntrySiteWards desw WITH(NOLOCK) ON desw.EntrySiteId = dses.EntrySiteId AND desw.Active = 1" +
            "LEFT JOIN FacilityMaster f WITH(NOLOCK) on des.FacilityId = f.FacilityID where des.Active = 1 and des.HospitalLocationId = @inyHospitalLocationID";

            try
            {
                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEntrySites(int iHospID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospID);
            //string strqry = "SELECT DISTINCT des.EntrySiteId, des.EntrySiteName, des.FacilityId, f.Name FacilityName ,CASE WHEN desw.WardId IS NULL THEN '' ELSE 'Y' END AS 'IPEntrySite', des.StationId FROM DiagEntrySites des WITH (NOLOCK) LEFT JOIN DiagEntrySiteWards desw WITH (NOLOCK) ON desw.EntrySiteId = des.EntrySiteId AND desw.Active = 1 LEFT JOIN FacilityMaster f WITH (NOLOCK) on des.FacilityId = f.FacilityID where des.Active = 1 and des.HospitalLocationId = 1 ";
            string strqry = "SELECT DISTINCT des.EntrySiteId, des.EntrySiteName, des.FacilityId, f.Name FacilityName, CASE WHEN desw.WardId IS NULL THEN '' ELSE 'Y' END AS 'IPEntrySite', dses.StationId, des.IsMainSampleReceivingLocation, des.IsResultEntryLocation " +
            "FROM DiagEntrySites des WITH(NOLOCK)" +
            "INNER JOIN DiagStationEntrySites dses WITH(NOLOCK) ON des.EntrySiteId = dses.EntrySiteId AND dses.Active = 1" +
            "LEFT JOIN DiagEntrySiteWards desw WITH(NOLOCK) ON desw.EntrySiteId = dses.EntrySiteId AND desw.Active = 1" +
            "LEFT JOIN FacilityMaster f WITH(NOLOCK) on des.FacilityId = f.FacilityID where des.Active = 1 and des.HospitalLocationId = @inyHospitalLocationId";

            try
            {
                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEntrySites(int iHospID, int iFacilityID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intfacid", iFacilityID);
            string strqry = "UspGetEnterySite";
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, strqry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveSampleAcknowledge(int iHospID, string xmlDiagSampleIds, int userId, int iEmpId, int iServicePerforme, int iFacilityId,
                                            string ManualLabNo, string PreviousLabResultSource, int PreviousLabResultDiagSampleId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@intSampleAckById", iEmpId);
            HshIn.Add("@intServicePerformed", iServicePerforme);
            HshIn.Add("@intEncodedBy", userId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@chvPreviousLabResultSource", PreviousLabResultSource);
            HshIn.Add("@intPreviousLabResultDiagSampleId", PreviousLabResultDiagSampleId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvOutManualLabNo", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleAcknowledge", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable saveSampleAcknowledge(int iHospID, string xmlDiagSampleIds, int userId, int iEmpId, int iServicePerforme, int iFacilityId,
                                            string ManualLabNo, string PreviousLabResultSource, int PreviousLabResultDiagSampleId, int NewReportTypeID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@intSampleAckById", iEmpId);
            HshIn.Add("@intServicePerformed", iServicePerforme);
            HshIn.Add("@intEncodedBy", userId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@chvPreviousLabResultSource", PreviousLabResultSource);
            HshIn.Add("@intPreviousLabResultDiagSampleId", PreviousLabResultDiagSampleId);
            HshIn.Add("@intNewReportTypeId", NewReportTypeID);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvOutManualLabNo", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleAcknowledge", HshIn, HshOut);

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string saveInvResult(string Source, int iLabNo, int iResultId, int iStationId, int iDiagSampleId,
       string xmlTemplateDetails, int iResultFromMachine, int iMachineId, string sReleaseStage,
       int iResultAssignedTo, int iRemarksId, int iUserId, string lblMachineResultDateTime, string ReviewRemark, bool iMultiStageResultFinalized)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intLabNo", iLabNo);
            HshIn.Add("@inyResultId", iResultId);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
            HshIn.Add("@bitResultFromMachine", iResultFromMachine);
            HshIn.Add("@intMachineId", iMachineId);
            HshIn.Add("@chrReleaseStage", sReleaseStage);
            HshIn.Add("@intResultAssignedTo", iResultAssignedTo);
            HshIn.Add("@intResultRemarksId", iRemarksId);
            HshIn.Add("@intEncodedBy", iUserId);
            HshIn.Add("@dtsMachineResultDateTime", lblMachineResultDateTime);
            HshIn.Add("@chvReviewRemark", ReviewRemark);
            HshIn.Add("@MultiStageResultFinalized", iMultiStageResultFinalized);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strSProc = "";
                if (Source == "OPD")
                {
                    strSProc = "UspDiagSaveInvResultOP";
                }
                else if (Source == "IPD")
                {
                    strSProc = "UspDiagSaveInvResultIP";
                }
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, strSProc, HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string calcFormula(int iFormulaFieldId, string xmlFields)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DataSet ds = new DataSet();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intFormulaFieldId", iFormulaFieldId);
                HshIn.Add("@xmlFields", xmlFields);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagCalculateFormula", HshIn, HshOut);
                string result = "";
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        ///result = Decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0][0]), 2).ToString("#.##");

                        result = Convert.ToString(ds.Tables[0].Rows[0][0]);


                    }

                }

                return result;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
        public DataSet GetLabNoInvestigations(string Source, int iStationId, int iLabNo, int iHospID, string xmlServices, int iFacilityId, int iIsCallFromLab, int intLoginEmployeeId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@inyHospitalLocationID", iHospID);
                HshIn.Add("@xmlServices", xmlServices);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iIsCallFromLab", iIsCallFromLab);
                HshIn.Add("@intLoginEmployeeId", intLoginEmployeeId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetLabNoInvestigations", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCalculateResultAlert(string xmlServicesDesc, string xmlServices)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@xmlFields", xmlServicesDesc);
                HshIn.Add("@xmlServiceId", xmlServices);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagCalculateResultAlert", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getLabNoInvFormats(string Source, int iLoginFacilityId, int iDiagSampleId, int iServiceID, string Gender, int AgeInDays, string StatusCode, int iHospID, int iIsCallFromLab)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@intServiceID", iServiceID);
            HshIn.Add("@chrGender", Gender);
            HshIn.Add("@intAgeInDays", AgeInDays);
            HshIn.Add("@chvCode", StatusCode);
            HshIn.Add("@iIsCallFromLab", iIsCallFromLab);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetLabNoInvFormats", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public String Deleteimageresult(int sampleID, int fieldID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@sampleID", sampleID);
            HshIn.Add("@fieldID", fieldID);
            //DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagdeleteimageResult", HshIn);
            try
            {
                int i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspDiagdeleteimageResult", HshIn);
                if (fieldID > 0 && i == 0)
                {
                    i = 1;
                    return "(" + i.ToString() + " )Image Deleted ";
                }
                else
                {
                    i = 0;
                    return "(" + i.ToString() + " )Image Deleted ";
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet Getimageresult(int sampleID, int fieldID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@sampleID", sampleID);
            HshIn.Add("@fieldID", fieldID);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiaggetimageResult", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getLabNoInvFormats(string Source, int iLoginFacilityId, int iDiagSampleId, int iServiceID, string Gender, int AgeInDays, string StatusCode, int iHospID, int iIsCallFromLab, int SampleID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@intServiceID", iServiceID);
            HshIn.Add("@chrGender", Gender);
            HshIn.Add("@intAgeInDays", AgeInDays);
            HshIn.Add("@chvCode", StatusCode);
            HshIn.Add("@iIsCallFromLab", iIsCallFromLab);
            HshIn.Add("@SampleID", SampleID);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetLabNoInvFormats", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getLabNoInvFormatsHistory(string Source, int iLoginFacilityId,
          int iDiagSampleId, int iServiceID, string StatusCode, int iHospID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intDiagSampleId", iDiagSampleId);
                HshIn.Add("@intServiceID", iServiceID);
                HshIn.Add("@chvCode", StatusCode);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiagGetLabNoInvFormats", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveCancelSampleCollection(string cSource, int iHospID, string xmlDiagSampleIds, string chrRemarks, int iLoginFacilityId, int EncodedBy)
        {
            return saveCancelSampleCollection(cSource, iHospID, xmlDiagSampleIds, chrRemarks, iLoginFacilityId, EncodedBy, string.Empty, string.Empty, null);
        }

        public string saveCancelSampleCollection(string cSource, int iHospID, string xmlDiagSampleIds, string chrRemarks, int iLoginFacilityId,
            int EncodedBy, string ToWhomInformed, string DeColectReasonText, int? DeColectReasonId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", cSource);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@chvRemarks", chrRemarks);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            if (!string.IsNullOrEmpty(ToWhomInformed))
                HshIn.Add("@chvToWhomInformed", ToWhomInformed);
            if (!string.IsNullOrEmpty(DeColectReasonText))
                HshIn.Add("@DeColectReasonText", DeColectReasonText);
            if (DeColectReasonId != null)
                HshIn.Add("@DeColectReasonId", DeColectReasonId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagCancelInvSampleCollection", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string saveCancelSampleAck(string cSource, int iHospID, string furtherAck,
                                    int iEntrySiteId, string xmlDiagSampleIds, int userId, string CancelRemarks)
        {
            return saveCancelSampleAck(cSource, iHospID, furtherAck, iEntrySiteId, xmlDiagSampleIds, userId, CancelRemarks, string.Empty, string.Empty, null, 0);
        }
        public string saveCancelSampleAck(string cSource, int iHospID, string furtherAck, int iEntrySiteId, string xmlDiagSampleIds, int userId,
                                          string CancelRemarks, string ToWhomInformed, string DeColectReasonText, int? DeColectReasonId, int ReportingFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", cSource);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@chrFurtherAcknowledgement", furtherAck);
            HshIn.Add("@inyEntrySiteId", iEntrySiteId);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@chvCancelRemarks", CancelRemarks);
            HshIn.Add("@intEncodedBy", userId);

            if (!string.IsNullOrEmpty(ToWhomInformed))
                HshIn.Add("@chvToWhomInformed", ToWhomInformed);

            if (!string.IsNullOrEmpty(DeColectReasonText))
                HshIn.Add("@DeColectReasonText", DeColectReasonText);

            if (DeColectReasonId != null)
                HshIn.Add("@DeColectReasonId", DeColectReasonId);



            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@intReportingFacilityId", ReportingFacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagCancelInvSampleAcknowledgement", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveResultFinalization(string Source, string chrDaigSampleID, int iEncodedBy, string sReleaseStage, int iEmployeeId, int iStationId,
            int centerDoctorId, int rightDoctorId, int LeftDoctor, int TatDelayReason = 0, int TatValidateFlag = 0, int LoginFacilityId = 0)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@xmlDiagSampleId", chrDaigSampleID);
            HshIn.Add("@chrReleaseStage", sReleaseStage);
            HshIn.Add("@intResultAssignedTo", iEmployeeId);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@intRightDoctor", rightDoctorId);
            HshIn.Add("@intCenterDoctor", centerDoctorId);
            HshIn.Add("@intLeftDoctor", LeftDoctor);
            HshIn.Add("@TatDelayReason", TatDelayReason);
            HshIn.Add("@TatValidateFlag", TatValidateFlag);
            HshIn.Add("@intLoginFacilityId", LoginFacilityId);
            HshIn.Add("@iLoginFacilityId", LoginFacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                if (Source == "IPD")
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDiagSaveResultFinalizationIP", HshIn, HshOut);
                }
                else
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveResultFinalization", HshIn, HshOut);
                }
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public string UpdateResultAddendum(string Source, string chrDaigSampleID, int centerAdDoctorId, int rightAdDoctorId, int LeftAdDoctor, int StationId, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@xmlDiagSampleId", chrDaigSampleID);
            HshIn.Add("@strSource", Source);
            HshIn.Add("@intRightDoctor", rightAdDoctorId);
            HshIn.Add("@intCenterDoctor", centerAdDoctorId);
            HshIn.Add("@intLeftDoctor", LeftAdDoctor);
            HshIn.Add("@intEmployeeId", UserId);
            HshIn.Add("@intStationId", StationId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateResultAddendum", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string GetAddendum(string Source, int chrDaigSampleID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intDiagSampleId", chrDaigSampleID);
            HshIn.Add("@strSource", Source);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetResultAddendum", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetResultFinalization(string Source, int iHospID, int iFacilityId, int iLoginFacilityId,
                int iStationId, int iStatusId, DateTime dtsDateFrom, DateTime dtsDateTo, string sRegistrationNo, int iRegistrationId,
                int iProviderId, string sPatientName, int iShowAssignedToMe, int iNotFinalized, int iEmployeeId,
                int iServiceId, string sResultType, int iPageSize, int iPageNo, int iLabNo, string xmlSubDeptId,
                 int ERRequest, int inyEntrySiteId, string ManualLabNo, int ReportTypeId, string sEncounterNo,
                 bool iIsCallFromLab, int EntrySiteActual, int iWardId = 0, int ShowProfileDetailsInFinalization = 1, bool isOutSourceTest = false, string sServiceName = "", int iSubDeptId = 0)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intStatusId", iStatusId);
                HshIn.Add("@chvDateFrom", fDate);
                HshIn.Add("@chvDateTo", tDate);
                HshIn.Add("@bitShowAssignedToMe", iShowAssignedToMe);
                HshIn.Add("@bitShowNotFanalized", iNotFinalized);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intLoginEmployeeId", iEmployeeId);
                HshIn.Add("@xmlSubDeptId", xmlSubDeptId);
                HshIn.Add("@intServiceId", iServiceId);
                HshIn.Add("@chrResultType", sResultType);
                HshIn.Add("@inyPageSize", iPageSize);
                HshIn.Add("@intPageNo", iPageNo);
                HshIn.Add("@intReportTypeId", ReportTypeId);
                HshIn.Add("@intEntrySiteAct", EntrySiteActual);
                HshIn.Add("@inyShowProfileDetailsInFinalization", ShowProfileDetailsInFinalization);

                if (ManualLabNo != "")
                {
                    HshIn.Add("@chvManualLabNo", ManualLabNo);
                }
                if (iLabNo > 0)
                {
                    HshIn.Add("@intLabNo", iLabNo);
                }
                if (sRegistrationNo != "")
                {
                    HshIn.Add("@chvRegistrationNo", sRegistrationNo);
                }
                if (sEncounterNo != "")
                {
                    HshIn.Add("@chvEncounterNo", sEncounterNo);
                }
                if (sPatientName != "")
                {
                    HshIn.Add("@chvPatientName", "%" + sPatientName + "%");
                }

                HshIn.Add("@bitOutsourceInv", isOutSourceTest);
                HshIn.Add("@chvServiceName", sServiceName);
                //HshIn.Add("@chvSubDeptName", sServiceName);
                //HshIn.Add("@chvSubDeptName", sSubDeptName);
                HshIn.Add("@intSubDeptId", iSubDeptId);


                if (Source == "A")
                {
                    HshIn.Add("@inyEntrySiteId", inyEntrySiteId);
                    HshIn.Add("@WardId", iWardId);
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultFinalization_OPIP", HshIn);
                }
                if (Source == "OPD")
                {
                    HshIn.Add("@inyEntrySiteId", inyEntrySiteId);
                    HshIn.Add("@bitERRequest", ERRequest);
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultFinalizationOP", HshIn);
                }
                else if (Source == "IPD")
                {
                    HshIn.Add("@inyEntrySiteId", inyEntrySiteId);
                    HshIn.Add("@iIsCallFromLab", iIsCallFromLab);
                    HshIn.Add("@WardId", iWardId);
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultFinalizationIP", HshIn);
                }
                else
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultFinalizationPackage", HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string CancelResultFinalization(string Source, int iStationId, int iHospID, string chrDaigSampleID,
            string chrCancellationRemarks, int iEncodedBy, bool bCancelResultEntry, bool bitCancelResultFinalization, bool boolCancelResultFinalization = false)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@xmlDiagSampleId", chrDaigSampleID);
            HshIn.Add("@chvCancellationRemarks", chrCancellationRemarks);
            HshIn.Add("@bitCancelResultEntry", bCancelResultEntry);
            HshIn.Add("@bitCancelResultFinalization", bitCancelResultFinalization);
            HshIn.Add("@bitCancelProvisionalRelease", boolCancelResultFinalization);
            HshIn.Add("@intEncodedBy", iEncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDiagCancelResultFinalization", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getLabDevicesMachineLabNo(int iLoginFacilityId, int iMachineId, DateTime dFromDate, DateTime dToDate, string LabNo, int pocMachine)
        {
            string fDate = dFromDate.ToString("yyyy-MM-dd");
            string tDate = dToDate.ToString("yyyy-MM-dd");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intMachineId", iMachineId);
            HshIn.Add("@dtResultFromDate", fDate);
            HshIn.Add("@dtResultToDate", tDate);
            HshIn.Add("@chvLabNo", LabNo);
            HshIn.Add("@bitPOCResults", pocMachine);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetMachineLabNo", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getLabDevicesMachineLabTest(string sSource, int iMachineId, int iLabNo, int iHostId, int StationId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@chrSource", sSource);
                HshIn.Add("@intMachineId", iMachineId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@intStationId", StationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetMachineLabTest", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getLabDevicesMachineLabResult(string sSource, int iMachineId, int iLabNo, int iRegId,
                                                    int iLoginFacilityId, int StationId, int HospId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@intMachineId", iMachineId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@inyHospitalLocationId", HospId);


                if (sSource == "OPD")
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetMachineLabResultOP", HshIn);
                }
                else
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetMachineLabResultIP", HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public string updateMachineLabResult(int Id, string result, string status)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@Id", Id);
                HshIn.Add("@Result", result);
                HshIn.Add("@status", status);
                string strqry = " UPDATE DiagInterfaceMachineResult SET Result = @Result,Active=@status WHERE Id = @Id ";

                int execute = objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);
                string retstr = "";
                if (execute == 0)
                {
                    retstr = "Record Updated...";
                }

                return retstr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getTurnAroundTimeData(int iLoginFacilityId, int iEntrySiteId, int iHostId, string Source, string ResultType, DateTime fromDate,
            DateTime toDate, int iStationId, int iServiceId, int iPageSize, int iPageNo, int iLabNo, string sRegistrationNo, string sPatientName,
            int iSubDeptId, string chvIPNo, int TATHours, bool bAbnormalResult, int iFieldId, string sResultSearchType, string dResultValue)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intEntrySiteId", iEntrySiteId);
                HshIn.Add("@intServiceId", iServiceId);
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@chrResultType", ResultType);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@inyPageSize", iPageSize);
                HshIn.Add("@intPageNo", iPageNo);
                HshIn.Add("@intSubDeptId", iSubDeptId);
                HshIn.Add("@chvEncounterNo", chvIPNo);


                HshIn.Add("@bitAbnormalResult", bAbnormalResult);
                HshIn.Add("@intFieldId", iFieldId);
                HshIn.Add("@chrResultSearchType", sResultSearchType);

                if (!(dResultValue.Equals(string.Empty)))
                    HshIn.Add("@decResultValue", dResultValue);

                //int iLabNo,string sRegistrationNo,string sPatientName)
                if (TATHours != 0)
                    HshIn.Add("@intTATHours", TATHours);
                if (iLabNo != 0)
                    HshIn.Add("@intLabNo", iLabNo);

                if (sRegistrationNo != "")
                    HshIn.Add("@chvRegistrationNo", sRegistrationNo);

                if (sPatientName != "")
                    HshIn.Add("@chvPatientName", sPatientName);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTurnAroundTime", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getTurnAroundTimeDataEC(int iLoginFacilityId, int iExternalSiteId, int iHostId, string Source,
                          DateTime fromDate, DateTime toDate, int iStationId, int iServiceId,
            int iPageSize, int iPageNo, int iLabNo, string sRegistrationNo, string sPatientName, int iSubDeptId, string chvIPNo)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intExternalCenterId", iExternalSiteId);
                HshIn.Add("@intServiceId", iServiceId);
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@inyPageSize", iPageSize);
                HshIn.Add("@intPageNo", iPageNo);
                HshIn.Add("@intSubDeptId", iSubDeptId);
                HshIn.Add("@chvEncounterNo", chvIPNo);
                if (iLabNo != 0)
                    HshIn.Add("@intLabNo", iLabNo);

                if (sRegistrationNo != "")
                    HshIn.Add("@chvRegistrationNo", sRegistrationNo);

                if (sPatientName != "")
                    HshIn.Add("@chvPatientName", sPatientName);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetTurnAroundTimeEC", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public string SaveRelayData(string Source, string xmlDiagSampleIds, int iRelayedTo, int iRelayedBy,
                                    string Remarks, int iEncodedBy, bool chkReadBack, string Active, string CancellationRemarks)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@intRelayedTo", iRelayedTo);
            HshIn.Add("@intRelayedBy", iRelayedBy);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@bitReadBack", chkReadBack);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@chvCancellationRemarks", CancellationRemarks);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDiagSaveRelayDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveLabOtherDetails(int intLoginFacilityId, string Source, string xmlDiagSampleIds, int iStationId, string ManualLabNo,
                                  string ReportTypeId, int iEncodedBy, DateTime? OralStartedTime, DateTime? ScanInTime, DateTime? ScanOutTime, int Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@intReportTypeId", ReportTypeId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@OralStartedTime", OralStartedTime);
            HshIn.Add("@ScanInTime", ScanInTime);
            HshIn.Add("@ScanOutTime", ScanOutTime);
            HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveLabOtherDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public string SaveCancerScreeningDetails(string Source, string xmlDiagSampleIds, int OriginId,
                                  bool IsCancer, int ICDId, int iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@OrganId", OriginId);
            HshIn.Add("@IsCancer", IsCancer);
            HshIn.Add("@ICDId", ICDId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveCancerScreeningDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveAddendumResult(string Source, int StationId, int DiagSampleId, string TextReport, int EncodedBy, bool ResultFinalize = false, int AddendumId = 0, int FinalizedById = 0, int FacilityId = 0)

        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", Source);
            HshIn.Add("@intStationId", StationId);
            HshIn.Add("@intDiagSampleId", DiagSampleId);
            HshIn.Add("@chvTextReport", TextReport);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@bitResultFinalize", ResultFinalize);
            HshIn.Add("@intAddendumId", AddendumId);
            HshIn.Add("@intFinalizedById", FinalizedById);
            HshIn.Add("@intFacilityId", FacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveAddendumResult", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteAddendumResults(string Source, int AddendumId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", Source);
            HshIn.Add("@intAddendumId", AddendumId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagCancelAddendumResult", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAddendumResults(string Source, int DiagSampleId, int AddendumId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@intDiagSampleId", DiagSampleId);
                HshIn.Add("@intAddendumId", AddendumId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetAddendumResults", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string DeleteCancerScreeningDetails(string Source, int Id)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", Source);
            HshIn.Add("@Id", Id);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagDeleteCancerScreeningDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string UpdateManualLabNo(int intLoginFacilityId, string Source, int DiagSampleIds, int iStationId, string ManualLabNo, int iEncodedBy, string ReportTypeId, DateTime? OralStartedTime, DateTime? ScanInTime, DateTime? ScanOutTime)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrSource", Source);
            HshIn.Add("@intDiagSampleIds", DiagSampleIds);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@chvManualLabNo", ManualLabNo);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@intReportTypeId", ReportTypeId);
            HshIn.Add("@OralStartedTime", OralStartedTime);
            HshIn.Add("@ScanInTime", ScanInTime);
            HshIn.Add("@ScanOutTime", ScanOutTime);
            HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagUpdateManualLabNo", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getRelayData(string Source, int iHostId, int iLabNo, int iStationId, int loginfacilityId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intLoginFacilityId", loginfacilityId);


                if (Source == "OPD")
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvRelayDetailsOP", HshIn);
                }
                else
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvRelayDetailsIP", HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public DataSet getLabOtherDetails(string Source, int iHostId, Int64 iLabNo, int iStationId, int loginfacilityId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@chrSource", Source);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabOtherDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet CancerScreeningDetails(string Source, int iHostId, Int64 iLabNo, int iStationId, int loginfacilityId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@chrSource", Source);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetCancerScreeningDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }




        public DataSet getLabDashboardData(DateTime fromDate, DateTime toDate, int iStationId, int iLoginFacilityId, int iLabno, string sFacilityId, int intSubDeptId, string chvDateCriteria, int RegistrationNo)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intLabno", iLabno);
                HshIn.Add("@sFacilityId", sFacilityId);
                HshIn.Add("@intSubDeptId", intSubDeptId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@chvDateCriteria", chvDateCriteria);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabDashboard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //added by njiraj 24 nov

        public DataSet getLabDashboardData(DateTime fromDate, DateTime toDate, int iStationId, int iLoginFacilityId, int iLabno, string sFacilityId, int intSubDeptId, string chvDateCriteria, int RegistrationNo, int EntrySiteId, string StationId)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intLabno", iLabno);
                HshIn.Add("@sFacilityId", sFacilityId);
                HshIn.Add("@intSubDeptId", intSubDeptId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@chvDateCriteria", chvDateCriteria);
                HshIn.Add("@EntrySiteid", EntrySiteId);
                HshIn.Add("@xmlStationId", StationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabDashboard", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPatientInvestigations(DateTime fromDate, DateTime toDate,
            int HospitalLocationId, int LoginFacilityId, int RegistrationId, bool AbnormalResult,
            bool CriticalResult)
        {

            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@chvDateFrom", fDate);
                HshIn.Add("@chvDateTo", tDate);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@bitAbnormalResult", AbnormalResult);
                HshIn.Add("@bitCriticalResult", CriticalResult);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResultNumericFields", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveUpdateDiagSampleDocument(int iDiagSampleId, string sDocumentName, string sDescription,
             int iEncodedBy, int DocumentId, string source)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@chvDocumentName", sDocumentName);
            HshIn.Add("@chvDescription", sDescription);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@intDocumentId", DocumentId);
            string ProcedureName = "";
            if (source == "OPD")
                ProcedureName = "uspDiagSaveUpdateAttachmentOP";
            else
                ProcedureName = "uspDiagSaveUpdateAttachmentIP";

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, ProcedureName, HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveSampleECDetails(string chrSource, int inyHospitalLocationId,
            string xmlDiagSampleIds, int intSampleSentById, DateTime dtsSampleSentDate, int intExternalCenterId,
            int intEncodedBy, int Status)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", chrSource);
            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@xmlDiagSampleIds", xmlDiagSampleIds);
            HshIn.Add("@intSampleSentById", intSampleSentById);
            HshIn.Add("@dtsSampleSentDate", dtsSampleSentDate);
            HshIn.Add("@intExternalCenterId", intExternalCenterId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@bitActive", Status);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveSampleECDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSampleECDetails(int HospitalLocationId, int iLoginFacilityId, int Labno, int StationId, string Source)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLabNo", Labno);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                if (Source == "OPD")
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSampleECDetailsOP", HshIn);
                else
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSampleECDetailsIP", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSamplePackageDetails(int HospitalLocationId, int Labno, int LoginFacilityId, int stationId
            , int PackageId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLabNo", Labno);
                HshIn.Add("@intStationId", stationId);
                HshIn.Add("@intPackageId", PackageId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPackageResultDetailsOP", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveSamplePackageDetails(string chrTranType,
            string xmlResultDetails, int UserId, string ServiceResult, string Remarks,
            int Active, int FinalizedBy, int StationId, int LabNo, string PackageRemarks,
            int LabInchargeId, int LabDoctorId, int PackageId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@chrTranType", chrTranType);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@intLabNo", LabNo);
                HshIn.Add("@intPackageId", PackageId);
                HshIn.Add("@chvPackageRemarks", PackageRemarks);
                if (LabInchargeId != 0)
                {
                    HshIn.Add("@intLabInchargeId", LabInchargeId);
                }
                if (LabDoctorId != 0)
                {
                    HshIn.Add("@intLabDoctorId", LabDoctorId);
                }
                if (ServiceResult != "")
                {
                    HshIn.Add("@chvServiceResult", ServiceResult);
                }
                HshIn.Add("@chvServiceRemarks", Remarks);
                if (FinalizedBy != 0)
                {
                    HshIn.Add("@intServiceFinalizedBy", FinalizedBy);
                }
                HshIn.Add("@bitServiceActive", Active);
                HshIn.Add("@xmlDiagSampleIds", xmlResultDetails);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSavePackageResultDetailsOP", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CancelPackageDetails(string PackageId, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intPackageMainId", PackageId);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagCancelPackageResultDetailsOP", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteSampleDocument(int iDiagSampleId, string sDescription, int iEncodedBy,
            int iHospitalLocationId, string source, int AttachmenId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@chvDescription", sDescription);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@AttachmenId", AttachmenId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            string ProcedureName = "";
            if (source == "OPD")
                ProcedureName = "uspDiagDeleteAttachmentOP";
            else
                ProcedureName = "uspDiagDeleteAttachmentIP";


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, ProcedureName, HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getSampleAcknowledgedByList(string sSource, int iLoginFacilityId, int iDiagSampleId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@chrSource", sSource);
                HshIn.Add("@intDiagSampleId", iDiagSampleId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetSampleAcknowledgedByList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getFinalRemarks(string sSource, int iDiagSampleId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", sSource);
                HshIn.Add("@intDiagSampleId", iDiagSampleId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetFinalRemarks", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveFinalRemarks(string sSource, int iDiagSampleId, string remarks, int userId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = new Hashtable();
                HshIn.Add("@chrSource", sSource);
                HshIn.Add("@intDiagSampleId", iDiagSampleId);
                HshIn.Add("@chvRemarks", remarks);
                HshIn.Add("@intEncodedBy", userId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveFinalRemarks", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getDigInvResultRemarks(string Source, int iLabNo, int iStationId, int iSubDeptId, int iFacilityId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intStationId", iStationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                string strTableName = "";
                if (Source == "OPD")
                {
                    strTableName = "DiagInvResultOPRemarks";
                }
                else
                {
                    strTableName = "DiagInvResultIPRemarks";
                }
                string strSQL = "SELECT LabNo, Remarks FROM " + strTableName + " dr WITH (NOLOCK) ";

                if (iSubDeptId > 0)
                {
                    strSQL = strSQL + " INNER JOIN DiagSampleReceivingStationDetails dsrs WITH (NOLOCK) ON dr.SubDeptId = dsrs.SubDeptId " +
                                    " AND dsrs.StationId = @intStationId ";
                }
                strSQL = strSQL + " WHERE dr.LabNo = @intLabNo And dr.FacilityId = @iFacilityId ";

                if (iSubDeptId > 0)
                {
                    HshIn.Add("@intSubDepartmentId", iSubDeptId);
                    strSQL = strSQL + " AND dr.SubDeptId = @intSubDepartmentId";
                }
                strSQL = strSQL + " AND dr.Active=1 ";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveUpdateDiagInvResultRemarks(string Source, int iLabNo, int iSubDepartmentId, string sRemarks, int iEncodedBy, int iFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", Source);
            HshIn.Add("@intLabNo", iLabNo);
            HshIn.Add("@intSubDeptId", iSubDepartmentId);
            HshIn.Add("@chvRemarks", sRemarks);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveUpdateInvResultRemarks", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string cancelDigInvResultRemarks(string Source, int iLabNo, int iSubDepartmentId, int iEncodedBy, int iFacilityId)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intSubDeptId", iSubDepartmentId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@iFacilityId", iFacilityId);
                string strTableName = "";
                if (Source == "OPD")
                {
                    strTableName = "DiagInvResultOPRemarks";
                }
                else
                {
                    strTableName = "DiagInvResultIPRemarks";
                }
                string strSQL = "";
                if (iSubDepartmentId > 0)
                    strSQL = "UPDATE " + strTableName + " SET Active=0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE LabNo = @intLabNo And FacilityId =  @iFacilityId AND SubDeptId = @intSubDeptId ";
                else
                    strSQL = "UPDATE " + strTableName + " SET Active=0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() WHERE LabNo = @intLabNo And FacilityId =  @iFacilityId AND SubDeptId IS NULL ";

                int iOut = objDl.ExecuteNonQuery(CommandType.Text, strSQL, HshIn);
                return iOut.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getLabResultData(int iLoginFacilityId, int iHostId, string Source,
                            DateTime fromDate, DateTime toDate, int iRegistrationId, int iProviderId)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@chvDateFrom", fDate);
                HshIn.Add("@chvDateTo", tDate);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intProviderId", iProviderId);
                if (Source == "OPD")
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResultsOP", HshIn);
                }
                else
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResultsIP", HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iLoginFacilityId"></param>
        /// <param name="iHostId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="iRegNo"></param>
        /// <param name="iProviderId"></param>
        /// <param name="iPageSize"></param>
        /// <param name="iPageNo"></param>
        /// <param name="AbnormalResult"></param>
        /// <param name="CriticalResult"></param>
        /// <param name="iStatusId"></param>
        /// <param name="FacilityId"></param>
        /// <returns></returns>


        public DataSet getPatientLabResultHistory(int iLoginFacilityId, int iHostId,
                                                DateTime fromDate, DateTime toDate, string iRegNo, int iProviderId, int iPageSize,
                                                int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId, int FacilityId,
                                                string chvEncounterNo, string Source, int SubDeptId, int ServiceId, int StationId, string ResultStatus,
                                                string DischargeSummary)
        {
            return getPatientLabResultHistory(iLoginFacilityId, iHostId, fromDate, toDate, iRegNo, iProviderId, iPageSize,
                                            iPageNo, AbnormalResult, CriticalResult, iStatusId, FacilityId,
                                            chvEncounterNo, Source, SubDeptId, ServiceId, StationId, ResultStatus, DischargeSummary, 0, 0, false);
        }
        public DataSet getPatientLabResultHistory(int iLoginFacilityId, int iHostId,
                                              DateTime fromDate, DateTime toDate, string iRegNo, int iProviderId, int iPageSize,
                                              int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId, int FacilityId,
                                              string chvEncounterNo, string Source, int SubDeptId, int ServiceId, int StationId, string ResultStatus,
                                              string DischargeSummary, int intLoginEmployeeId, int SearchFlag)
        {
            return getPatientLabResultHistory(iLoginFacilityId, iHostId, fromDate, toDate, iRegNo, iProviderId, iPageSize,
                                            iPageNo, AbnormalResult, CriticalResult, iStatusId, FacilityId,
                                            chvEncounterNo, Source, SubDeptId, ServiceId, StationId, ResultStatus, DischargeSummary, intLoginEmployeeId, SearchFlag, false);
        }

        public DataSet getPatientLabResultHistory(int iLoginFacilityId, int iHostId,
                                                DateTime fromDate, DateTime toDate, string iRegNo, int iProviderId, int iPageSize,
                                                int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId, int FacilityId,
                                                string chvEncounterNo, string Source, int SubDeptId, int ServiceId, int StationId, string ResultStatus,
                                                string DischargeSummary, int intLoginEmployeeId, int SearchFlag, bool Favourite)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@chvRegistrationNo", iRegNo);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateFrom", fDate);
                HshIn.Add("@chvDateTo", tDate);
                HshIn.Add("@intProviderId", iProviderId);

                HshIn.Add("@bitAbnormalResult", AbnormalResult);
                HshIn.Add("@bitCriticalResult", CriticalResult);
                HshIn.Add("@chvEncounterNo", chvEncounterNo);

                HshIn.Add("@intSubDeptId", SubDeptId);
                HshIn.Add("@intServiceId", ServiceId);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@chvResultStatus", ResultStatus);
                if (iStatusId != 0)
                {
                    HshIn.Add("@intStatusId", iStatusId);
                }
                if (intLoginEmployeeId > 0)
                {
                    HshIn.Add("@intLoginEmployeeId", intLoginEmployeeId);
                }
                if (DischargeSummary == "Summary")
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultsForIPSummary", HshIn);
                }
                else
                {
                    HshIn.Add("@inyPageSize", iPageSize);
                    HshIn.Add("@intPageNo", iPageNo);
                    HshIn.Add("@chvSource", Source);
                    HshIn.Add("@inySearchFlag", SearchFlag);

                    if (Favourite)
                    {
                        HshIn.Add("@bitFavourite", Favourite);
                    }

                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResults", HshIn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }

        }

        public DataSet getPatientLabResultHistory(DateTime fromDate, DateTime toDate, int iRegId, int iServiceId, int iRecordCount, String sSource, int iDiagSampleId)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("dtFromDate", fDate);
            HshIn.Add("dtToDate", tDate);
            HshIn.Add("intRegistrationId", iRegId);
            HshIn.Add("intServiceId", iServiceId);
            HshIn.Add("inyRecordCount", iRecordCount);
            HshIn.Add("@chvSource", sSource);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPatientResultHistory", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDiagPatientLabServices(int iHospitalLocationId, int iFacilityId, DateTime dfromDate, DateTime dtoDate, string cRegistrationNo, string cEncounterId)
        {
            string fDate = dfromDate.ToString("yyyy/MM/dd");
            string tDate = dtoDate.ToString("yyyy/MM/dd");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@cRegistrationNo", cRegistrationNo);
                HshIn.Add("@cEncounterId", cEncounterId);
                HshIn.Add("@cDateFrom", fDate);
                HshIn.Add("@cDateTo", tDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDiagPatientLabServices", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetDiagLabResultDynamicGrid(int iLoginFacilityId, int iHostId, DateTime fromDate, DateTime toDate, string iRegNo, int iProviderId, int iPageSize, int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId, int FacilityId, string chvEncounterNo, string xmlService, string xmlFieldId, string chvEncounterId, string cStationId, int iLoginId)
        {
            return GetDiagLabResultDynamicGrid(iLoginFacilityId, iHostId, fromDate, toDate, iRegNo, iProviderId, iPageSize, iPageNo, AbnormalResult, CriticalResult, iStatusId, FacilityId, chvEncounterNo, xmlService, xmlFieldId, chvEncounterId, cStationId, iLoginId, false);
        }

        public DataSet GetDiagLabResultDynamicGrid(int iLoginFacilityId, int iHostId, DateTime fromDate, DateTime toDate, string iRegNo, int iProviderId, int iPageSize, int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId, int FacilityId, string chvEncounterNo, string xmlService, string xmlFieldId, string chvEncounterId, string cStationId, int iLoginId, bool Favourite)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@chvRegistrationNo", iRegNo);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateFrom", fDate);
                HshIn.Add("@chvDateTo", tDate);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@inyPageSize", iPageSize);
                HshIn.Add("@intPageNo", iPageNo);
                HshIn.Add("@bitAbnormalResult", AbnormalResult);
                HshIn.Add("@bitCriticalResult", CriticalResult);
                HshIn.Add("@chvEncounterNo", chvEncounterNo);
                HshIn.Add("@xmlServiceId", xmlService);
                HshIn.Add("@xmlFieldId", xmlFieldId);
                HshIn.Add("@chvEncounterId", chvEncounterId);
                HshIn.Add("@cStationId", cStationId);
                HshIn.Add("@intLoginEmployeeId", iLoginId);

                if (iStatusId != 0)
                {
                    HshIn.Add("@intStatusId", iStatusId);
                }
                if (Favourite)
                {
                    HshIn.Add("@bitFavourite", Favourite);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDiagLabResultDynamicGrid", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iHospId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="iRegId"></param>
        /// <param name="iServiceId"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>


        public DataSet getPatientLabResultHistoryDash(int iLoginFacilityId, int iHostId, DateTime fromDate, DateTime toDate, string iRegNo,
                                 int iProviderId, int iPageSize, int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId,
                                 int FacilityId, string chvEncounterNo, int ReviewedStatus, string PatientName, int iUserId,
                                 bool IsMinLabel, bool IsMaxLabel)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", iHostId);
            HshIn.Add("@chvRegistrationNo", iRegNo);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvDateFrom", fDate);
            HshIn.Add("@chvDateTo", tDate);
            HshIn.Add("@intProviderId", iProviderId);
            HshIn.Add("@inyPageSize", iPageSize);
            HshIn.Add("@intPageNo", iPageNo);
            HshIn.Add("@bitAbnormalResult", AbnormalResult);
            HshIn.Add("@bitCriticalResult", CriticalResult);
            HshIn.Add("@chvEncounterNo", chvEncounterNo);
            HshIn.Add("@ReviewedStatus", ReviewedStatus);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@intUserId", iUserId);
            HshIn.Add("@bitMinLabel", IsMinLabel);
            HshIn.Add("@bitMaxLabel", IsMaxLabel);

            if (iStatusId != 0)
            {
                HshIn.Add("@intStatusId", iStatusId);
            }
            //ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResultsForDashboard", HshIn);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetLabResultsForDashboard", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public DataSet getPatientLabResultHistoryDash(int iLoginFacilityId, int iHostId, DateTime fromDate, DateTime toDate, string iRegNo, int iProviderId, int iPageSize, int iPageNo, bool AbnormalResult, bool CriticalResult, int iStatusId, int FacilityId, string chvEncounterNo, int ReviewedStatus, string PatientName, int iUserId)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@chvRegistrationNo", iRegNo);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvDateFrom", fDate);
                HshIn.Add("@chvDateTo", tDate);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@inyPageSize", iPageSize);
                HshIn.Add("@intPageNo", iPageNo);
                HshIn.Add("@bitAbnormalResult", AbnormalResult);
                HshIn.Add("@bitCriticalResult", CriticalResult);
                HshIn.Add("@chvEncounterNo", chvEncounterNo);
                HshIn.Add("@ReviewedStatus", ReviewedStatus);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@intUserId", iUserId);
                if (iStatusId != 0)
                {
                    HshIn.Add("@intStatusId", iStatusId);
                }
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResultsForDashboard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iHospId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="iRegId"></param>
        /// <param name="iServiceId"></param>
        /// <param name="iRecordCount"></param>
        /// <returns></returns>
        //public DataSet getPatientLabResultHistory(DateTime fromDate, DateTime toDate, int iRegId, int iServiceId, int iRecordCount)
        //{
        //    string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
        //    string tDate = toDate.ToString("yyyy-MM-dd 23:59");
        //    DataSet ds = new DataSet();
        //    HshIn = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    HshIn.Add("dtFromDate", fDate);
        //    HshIn.Add("dtToDate", tDate);
        //    HshIn.Add("intRegistrationId", iRegId);
        //    HshIn.Add("intServiceId", iServiceId);
        //    HshIn.Add("inyRecordCount", iRecordCount);

        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPatientResultHistory", HshIn);

        //    return ds;
        //}

        public DataSet getAttachmentDownload(string strSampleId, string filename)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strqry = "";
                if (filename == "")
                    strqry = "select Id, Description,DocumentName,DiagSampleId from DiagInvResultOPAttachment WITH (NOLOCK) where DiagSampleId in (" + strSampleId + ") ";
                else
                    strqry = "select Id, Description,DocumentName,DiagSampleId from DiagInvResultOPAttachment WITH (NOLOCK) where DiagSampleId in (" + strSampleId + ") and Description='" + filename + "' ";

                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet FillAttachmentDownloadDropdownOP(string strSampleId, string filename)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strqry = "";
                if (filename == "")
                    strqry = "select Id, Description,DocumentName,DiagSampleId from  DiagInvResultOPAttachment WITH (NOLOCK) where active=1 and DiagSampleId in (" + strSampleId + ")  ";
                else
                    strqry = "select Id,  Description,DocumentName,DiagSampleId from  DiagInvResultOPAttachment WITH (NOLOCK) where active=1 and  DiagSampleId in (" + strSampleId + ") and Description='" + filename + "' ";

                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet FillAttachmentDownloadDropdownIP(string strSampleId, string filename)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strqry = "";
                if (filename == "")
                    strqry = "select Id, Description,DocumentName,DiagSampleId from  DiagInvResultIPAttachment WITH (NOLOCK) where active=1 and DiagSampleId in (" + strSampleId + ")  ";
                else
                    strqry = "select Id,  Description,DocumentName,DiagSampleId from  DiagInvResultIPAttachment WITH (NOLOCK) where active=1 and  DiagSampleId in (" + strSampleId + ") and Description='" + filename + "' ";

                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getPatientDetails(Int64 registrationId, int facilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFacilityId", facilityId);
                HshIn.Add("@intRegistrationId", registrationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetRegistrationDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPatientDetails(Int64 registrationId, int facilityId, string PatientName, int PageSize, int PageNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFacilityId", facilityId);
                HshIn.Add("@intRegistrationId", registrationId);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                if (PatientName != "")
                {
                    HshIn.Add("@chvPatientName", "%" + PatientName + "%");
                }
                else
                {
                    HshIn.Add("@chvPatientName", "");
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetRegistrationDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetheaderResultEntry(string source, int hospitalLocationId, int loginFacilityId, int labNo, int iStationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chrSource", source);
                HshIn.Add("@inyHospitalLocationId", hospitalLocationId);
                HshIn.Add("@intLoginFacilityId", loginFacilityId);
                HshIn.Add("@intLabNo", labNo);
                HshIn.Add("@StationId", iStationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetResultHeader", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSampleCollectedStatus(int LoginFacilityId, DateTime ToDate, DateTime FromDate, int CallFrom = 0)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);
                HshIn.Add("@CallFrom", CallFrom);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetCountSNC", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetWorksheetData(string chrSource, int intStationId, string dtWorkSheetFromDate, string dtWorkSheetToDate
          , int intLoginFacilityId, string SubDeptIds, int intStartSerialNo, int intEndSerialNo, int intServiceId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chrSource", chrSource);
                HshIn.Add("@intStationId", intStationId);
                HshIn.Add("@dtWorkSheetFromDate", dtWorkSheetFromDate);
                HshIn.Add("@dtWorkSheetToDate", dtWorkSheetToDate);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                HshIn.Add("@xmlSubDeptId", SubDeptIds);
                HshIn.Add("@intStartSerialNo", intStartSerialNo);
                HshIn.Add("@intEndSerialNo", intEndSerialNo);
                HshIn.Add("@intServiceId", intServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetWorkSheet", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void UpdateprintStatus(string Source, int LabNo, string ServiceId, int intFacilityId, int EncodedBy)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvSource", Source);
                HshIn.Add("@intLabNo", LabNo);
                HshIn.Add("@xmlServiceId", ServiceId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                //string strqry = "UPDATE DiagSampleOPLabMain  SET PrintStatus =1 WHERE LabNo = @intLabNo AND Convert(VARCHAR,ServiceId) IN (@ServiceId)";
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspDiagUpdatePrintStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetAuditResultData(int HospitalLocationId, int intFacilityId, DateTime chvDateFrom
            , DateTime chvDateTo, string chvRegistrationNo, Int64 LabNo, string chvPatientName, string Source, int iSubDeptId, int iStatusId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@chvDateTo", chvDateTo);
                HshIn.Add("@chvDateFrom", chvDateFrom);
                HshIn.Add("@chvRegistrationNo", chvRegistrationNo);
                HshIn.Add("@LabNo", LabNo);
                HshIn.Add("@chvPatientName", chvPatientName);
                HshIn.Add("@chvSource", Source);
                HshIn.Add("@intSubDeptId", iSubDeptId);
                HshIn.Add("@intStatusId", iStatusId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetAuditResult", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveLISNotes(string procedureName, string source, int LabNo, string Notes, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", source);
            HshIn.Add("@intLabNo", LabNo);
            HshIn.Add("@chvNotes", Notes);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, procedureName, HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetLISNotes(string procedureName, string source, int LabNo, string Notes, int EncodedBy, int FacilityId, int OrderId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@chrSource", source);
            HshIn.Add("@intLabNo", LabNo);
            HshIn.Add("@chvNotes", Notes);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intOrderId", OrderId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, procedureName, HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getFormatFields(int FieldId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intFieldId", FieldId);

                string strqry = "SELECT t2.FormatId AS ValueId, t2.FormatManualCode AS ValueName, t2.Format AS Remarks, 'F' as ValueType, t2.SequenceNo " +
                        " FROM DiagFieldsText t2 WITH (NOLOCK) WHERE t2.FieldId = @intFieldId AND t2.Active = 1 ORDER BY t2.SequenceNo ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //It will be come in BaseC-clsLISPhlebotomy.cs
        //Added by rakesh to Get Patient Has Critical Parameter exists
        public string GetPatientHasCriticalParameter(string EncounterNo, int HospitalLocationID, int FacilityId, int intEncodedBy, int labNo, int serviceId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@EncounterNo", EncounterNo);
                HshIn.Add("@intHospitalLocationid", HospitalLocationID);
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@LabNo", labNo);
                HshIn.Add("@ServiceId", serviceId);
                string strResult = (string)objDl.ExecuteScalar(System.Data.CommandType.StoredProcedure, "uspDiagGetPatientHasCriticalParameter", HshIn);
                return strResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by rakesh to Get Patient Has Critical Parameter exists

        //It will be come in BaseC-clsLISPhlebotomy.cs
        //Added by rakesh to Get Patient Has Critical Parameter exists
        public string GetPatientHasCriticalParameterForOP(string EncounterNo, int HospitalLocationID, int FacilityId, int intEncodedBy, int labNo, int serviceId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@EncounterNo", EncounterNo);
                HshIn.Add("@intHospitalLocationid", HospitalLocationID);
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@LabNo", labNo);
                HshIn.Add("@ServiceId", serviceId);
                string strResult = (string)objDl.ExecuteScalar(System.Data.CommandType.StoredProcedure, "uspDiagGetPatientHasCriticalParameterForOP", HshIn);
                return strResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by rakesh to Get Patient Has Critical Parameter exists


        public int GetPatientAgeInDaysForRange(string OPIP, string DiagSampleId, string ServiceId)
        {
            string sQuery = "";
            int iResult = 0;
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            HshIn.Add("@intDiagSampleId", DiagSampleId);
            HshIn.Add("@intServiceId", ServiceId);
            if (OPIP == "OPD")
            {
                sQuery = "SELECT CASE WHEN dsom.RegistrationId IS NOT NULL THEN CASE WHEN reg.DateofBirth IS NULL OR reg.DateofBirth = '' THEN 0 ELSE DATEDIFF(D, reg.DateofBirth, DATEADD(mi, 330, dsom.EncodedDate)) END	ELSE ISNULL(dsom.Age,0) * CASE dsom.AGETYPE WHEN 'Y' THEN 365 WHEN 'M' THEN 30 WHEN 'D' THEN 1 ELSE 0 END END AS AgeInDays " +
                           " FROM DiagSampleOPLabMain dsom WITH (NOLOCK) INNER JOIN ItemOfService ios WITH (NOLOCK) ON ios.ServiceId = dsom.ServiceId " +
                           " INNER JOIN dbo.GetStatus(1, 'LAB') sm ON sm.StatusId = dsom.StatusId LEFT JOIN Employee emp WITH (NOLOCK) ON dsom.ReferredBy = emp.Id " +
                           " LEFT JOIN Registration reg WITH (NOLOCK) ON dsom.RegistrationId = reg.Id " +
                           " WHERE DSOM.DiagSampleId=@intDiagSampleId AND dsom.ServiceId=@intServiceId ";
            }
            else
            {
                sQuery = " SELECT CASE WHEN reg.DateofBirth IS NULL OR reg.DateofBirth = '' THEN 0 ELSE DATEDIFF(D, reg.DateofBirth, DATEADD(mi, 330, dsom.EncodedDate)) END AS AgeInDays " +
                                " FROM DiagSampleIPLabMain dsom WITH (NOLOCK) INNER JOIN ItemOfService ios WITH (NOLOCK) ON ios.ServiceId = dsom.ServiceId " +
                                " INNER JOIN dbo.GetStatus(1, 'LAB') sm ON sm.StatusId = dsom.StatusId LEFT JOIN Employee emp WITH (NOLOCK) ON dsom.ReferredBy = emp.Id " +
                                " LEFT JOIN Registration reg WITH (NOLOCK) ON dsom.RegistrationId = reg.Id WHERE DSOM.DiagSampleId=@intDiagSampleId AND dsom.ServiceId=@intServiceId ";
            }
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                // iResult = (int)objDl.ExecuteScalar(CommandType.Text, sQuery, HshIn);
                ds = objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    iResult = Convert.ToInt32(ds.Tables[0].Rows[0]["AgeInDays"]);
                }
                return iResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                ds.Dispose();
            }

        }
        public DataSet uspDiagPrintLabResult(int iHospId, int iLoginFacilityId, int iLabNo, int intStationId,
            string chvServiceIds, string source)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@chrSource", source);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intStationId", intStationId);
                HshIn.Add("@chvServiceIds", chvServiceIds);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintLabResult", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet DiagPrintCheckHTMLReport(int ServiceId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intServiceId", ServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintCheckHTMLReport", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool checkIsExistsTextFormat(string Source, int LabNo, int StationId, int FacilityId, string ServiceIds)
        {
            bool isExists = false;
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intLabNo", LabNo);
                HshIn.Add("@intStationId", StationId);
                HshIn.Add("@intFacilityId", FacilityId);

                string strqry = "";

                if (Source == "IPD")
                {
                    strqry = " SELECT dsim.DiagSampleId " +
                            " FROM DiagSampleIPLabMain dsim WITH (NOLOCK) " +
                            " INNER JOIN ItemOfService ios WITH (NOLOCK) ON ios.ServiceId = dsim.ServiceId " +
                            " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ds.SubDeptId = ios.SubDeptId " +
                            " INNER JOIN DiagSampleReceivingStationDetails dsrsd WITH (NOLOCK) ON dsrsd.SubDeptId = ds.SubDeptId AND dsrsd.Active = 1 " +
                            " INNER JOIN DiagInvResultIP dir WITH (NOLOCK) ON dir.DiagSampleId = dsim.DiagSampleId AND dir.Active = 1 " +
                            " INNER JOIN DiagFields df WITH (NOLOCK) ON df.FieldId = dir.FieldId " +
                            " WHERE dsim.LabNo = @intLabNo " +
                            " AND df.FieldType = 'W' " +
                            " AND dsrsd.StationId = @intStationId " +
                            " AND dsim.Active = 1 " +
                            " AND dsim.FacilityId = @intFacilityId ";
                }
                else
                {
                    strqry = " SELECT dsim.DiagSampleId " +
                            " FROM DiagSampleOPLabMain dsim WITH (NOLOCK) " +
                            " INNER JOIN ItemOfService ios WITH (NOLOCK) ON ios.ServiceId = dsim.ServiceId " +
                            " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ds.SubDeptId = ios.SubDeptId " +
                            " INNER JOIN DiagSampleReceivingStationDetails dsrsd WITH (NOLOCK) ON dsrsd.SubDeptId = ds.SubDeptId AND dsrsd.Active = 1 " +
                            " INNER JOIN DiagInvResultOP dir WITH (NOLOCK) ON dir.DiagSampleId = dsim.DiagSampleId AND dir.Active = 1 " +
                            " INNER JOIN DiagFields df WITH (NOLOCK) ON df.FieldId = dir.FieldId " +
                            " WHERE dsim.LabNo = @intLabNo " +
                            " AND df.FieldType = 'W' " +
                            " AND dsrsd.StationId = @intStationId " +
                            " AND dsim.Active = 1 " +
                            " AND dsim.FacilityId = @intFacilityId ";
                }

                if (ServiceIds.Trim() != "")
                {
                    strqry += " AND dsim.ServiceId IN (" + ServiceIds + ")";
                }

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

        public DataSet getDoctorImageDetails(int UserId, int HospitalLocationId, int LoginFacilityId, int EncounterId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", UserId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorSignDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPatientManualLabNos(int RegistrationId, int ServiceId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intServiceId", ServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetPatientManualLabNos", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string IsDeptRadiologyAndPacs(int iHospID, int iLoginFacilityId, int iServiceID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intServiceID", iServiceID);
                string strResult = (string)objDl.ExecuteScalar(System.Data.CommandType.StoredProcedure, "uspIsDeptRadiologyAndPacs", HshIn);
                return strResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet IsDeptRadiologyAndPacs(int iHospID, int iLoginFacilityId, int iServiceID)
        //{
        //    HshIn = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    HshIn.Add("@inyHospitalLocationId", iHospID);
        //    HshIn.Add("@intLoginFacilityId", iLoginFacilityId);            
        //    HshIn.Add("@intServiceID", iServiceID);            
        //    DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspIsDeptRadiologyAndPacs", HshIn);
        //    return ds;
        //}
        public DataSet GetLabNoInvFormatsForXR(string Source, int iHospID, int iLoginFacilityId, int iDiagSampleId, int iServiceID, string StatusCode)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intDiagSampleId", iDiagSampleId);
                HshIn.Add("@intServiceID", iServiceID);
                HshIn.Add("@chvCode", StatusCode);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabNoInvFormatsForXR", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable GetLabNoByManualLabNo(int iHospId, int iFacilityId, string sManualLabNo, string sSource)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@chvManualLabNo", sManualLabNo);
            HshIn.Add("@inyHospitalLocationId", iHospId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvSource", sSource);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabNoByManualLabNo", HshIn).Tables[0];


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetDiagServiceDetails(int ServiceId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    string strqry = "Select ResultHTML,PrintReferenceRangeInHTML from DiagServiceDetails WITH (NOLOCK) where serviceid =" + ServiceId;
        //    return objDl.FillDataSet(CommandType.Text, strqry);
        //}

        public DataSet GetDiagServiceDetails(int ServiceId, int FacilityId)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intServiceId", ServiceId);
            HshIn.Add("@intFacilityId", FacilityId);

            string strqry = "SELECT DISTINCT ResultHTML, PrintReferenceRangeInHTML FROM DiagServiceDetailsFacility WITH (NOLOCK) WHERE ServiceId = @intServiceId AND FacilityId = @intFacilityId";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getAllLabServices(int FacilityId, int EncounterId, string RegistrationNo, string sLabType = "")
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvLabType", sLabType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDiagAllLabServices", HshIn);
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

        public DataSet GetDiagPatientLabField(int iHospitalLocationId, int iFacilityId, DateTime dfromDate, DateTime dtoDate, string cRegistrationNo, string cEncounterId)
        {
            string fDate = dfromDate.ToString("yyyy/MM/dd");
            string tDate = dtoDate.ToString("yyyy/MM/dd");
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@cRegistrationNo", cRegistrationNo);
            HshIn.Add("@cEncounterId", cEncounterId);
            HshIn.Add("@cDateFrom", fDate);
            HshIn.Add("@cDateTo", tDate);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDiagPatientLabField", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAgeRange()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strqry = "Select id,AgeFrom,AgeTo,Active,case when Active = 1 then 'Active' else 'In-Active' end as ActiveStatus from MRDReportAgeRange with (nolock) order by id desc";
                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateMRDReportAgeRange(int iId, int iAgeFrom, int iAgeTo, bool bActive)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@iAgeFrom", iAgeFrom);
                HshIn.Add("@iAgeTo", iAgeTo);
                HshIn.Add("@bActive", bActive);
                HshIn.Add("@iId", iId);
                string qry = "update MRDReportAgeRange set AgeFrom=@iAgeFrom, AgeTo=@iAgeTo,Active=@bActive where id=@iId";
                objDl.ExecuteNonQuery(CommandType.Text, qry, HshIn);
                return "Record Updated Successfully ";
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null; HshIn = null;
            }
        }

        public DataSet getLISFormat(int HospitalLocationId, string Gender, int AgeInDays, int LoginFacilityId, int ServiceID, int DoctorId)
        {
            return getLISFormat(HospitalLocationId, Gender, AgeInDays, LoginFacilityId, ServiceID, DoctorId, false);
        }

        public DataSet getLISFormat(int HospitalLocationId, string Gender, int AgeInDays, int LoginFacilityId, int ServiceID, int DoctorId, bool IsShowAllServiceFields)
        {

            HshIn = new Hashtable();
            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chrGender", Gender);
                HshIn.Add("@intAgeInDays", AgeInDays);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intServiceID", ServiceID);

                if (DoctorId > 0)
                {
                    HshIn.Add("@intDoctorId", DoctorId);
                }
                if (IsShowAllServiceFields)
                {
                    HshIn.Add("@bitIsShowAllServiceFields", IsShowAllServiceFields);
                }

                return obj.FillDataSet(CommandType.StoredProcedure, "uspEMRGetLabInvFormats", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                obj = null;
            }

        }


        public string SaveOutsourceDt(int inyHospitalLocationId, int intFacilityId, string chvRegistrationNo, string chvEncounterNo, string xmlTemplateDetails, string xmlServiceIdManualRequest, int intLoginFacilityId, int intEncodedBy, string testdate)
        {

            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@chvRegistrationNo", chvRegistrationNo);
            HshIn.Add("@chvEncounterNo", chvEncounterNo);
            HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
            HshIn.Add("@xmlServiceIdManualRequest", xmlServiceIdManualRequest);
            HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@chvtestdate", testdate);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                HshOut = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveSample", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string Deleteimageresult(int sampleID, int fieldID, int intEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@sampleID", sampleID);
            HshIn.Add("@fieldID", fieldID);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagdeleteimageResult", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDeficiencyCategory()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strqry = "SELECT Id, Name FROM MRDDeficiencyCategory WITH (NOLOCK) WHERE Active = 1 ORDER BY Name";
            try
            {
                return objDl.FillDataSet(CommandType.Text, strqry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getInvRejectedSamples(int HospId, int FacilityId, string Source, DateTime dtsDateFrom, DateTime dtsDateTo, int CallFrom)
        {
            return getInvRejectedSamples(HospId, FacilityId, Source, dtsDateFrom, dtsDateTo, CallFrom, 0, "", "");
        }
        public DataSet getInvRejectedSamples(int HospId, int FacilityId, string Source, DateTime dtsDateFrom, DateTime dtsDateTo, int CallFrom,
            int RegistrationNo, string EncounterNo, string PatientName)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvSource", Source);
            HshIn.Add("@dtsDateFrom", fDate);
            HshIn.Add("@dtsDateTo", tDate);
            HshIn.Add("@CallFrom", CallFrom);
            if (RegistrationNo > 0)
            {
                HshIn.Add("@intRegistrationNo", RegistrationNo);
            }
            if (EncounterNo != string.Empty)
            {
                HshIn.Add("@chvEncounterNo", EncounterNo);
            }
            if (PatientName != string.Empty)
            {
                HshIn.Add("@chvPatientName", PatientName);
            }

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetInvRejectedSamples", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string ACKRejectedSample(int intHospitalLocationId, int intFacilityId, string xmlACKDiagSampleId, int intEncodedBy)
        {

            DAL.DAL obj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@xmlACKDiagSampleId", xmlACKDiagSampleId);
            HshIn.Add("@intEncodedBy", intEncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {
                HshOut = obj.getOutputParametersValues(CommandType.StoredProcedure, "uspWardACKRejectedSample", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEmailLogLabResults(int FacilityID, int HospitalLocationId, string EmailType, string EMailTo, string PatientName, int RegistrationId, string RegistrationNo,
                   int EncounterId, string EncounterNo, string LabNo, int SendTODoctorID, string SendToDoctorName, int AttendingDOctorID, string AttendingDOctorName,
                   int Active, int EncodedBy, string EncodedDate)
        {
            string sProcName = "";
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@FacilityID", FacilityID);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
            HshIn.Add("@EmailType", EmailType);
            HshIn.Add("@EMailTo", EMailTo);
            HshIn.Add("@PatientName", PatientName);
            HshIn.Add("@RegistrationId", RegistrationId);
            HshIn.Add("@RegistrationNo", RegistrationNo);
            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@EncounterNo", EncounterNo);
            HshIn.Add("@LabNo", LabNo);
            HshIn.Add("@SendTODoctorID", SendTODoctorID);
            HshIn.Add("@SendToDoctorName", SendToDoctorName);
            HshIn.Add("@AttendingDOctorID ", AttendingDOctorID);
            HshIn.Add("@AttendingDOctorName", AttendingDOctorName);
            HshIn.Add("@Active", Active);
            HshIn.Add("@EncodedBy", EncodedBy);
            HshIn.Add("@EncodedDate", EncodedDate);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            sProcName = "UspSaveLabEmailLog";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, sProcName, HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string saveInvResult(string Source, int iLabNo, int iResultId, int iStationId, int iDiagSampleId,
        string xmlTemplateDetails, int iResultFromMachine, int iMachineId, string sReleaseStage,
        int iResultAssignedTo, int iRemarksId, int iUserId, string lblMachineResultDateTime, string ReviewRemark, bool iMultiStageResultFinalized, int iLoginFacilityID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intLabNo", iLabNo);
            HshIn.Add("@inyResultId", iResultId);
            HshIn.Add("@intStationId", iStationId);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@xmlTemplateDetails", xmlTemplateDetails);
            HshIn.Add("@bitResultFromMachine", iResultFromMachine);
            HshIn.Add("@intMachineId", iMachineId);
            HshIn.Add("@chrReleaseStage", sReleaseStage);
            HshIn.Add("@intResultAssignedTo", iResultAssignedTo);
            HshIn.Add("@intResultRemarksId", iRemarksId);
            HshIn.Add("@intEncodedBy", iUserId);
            HshIn.Add("@dtsMachineResultDateTime", lblMachineResultDateTime);
            HshIn.Add("@chvReviewRemark", ReviewRemark);
            HshIn.Add("@MultiStageResultFinalized", iMultiStageResultFinalized);
            HshIn.Add("@intLoginFacilityID", iLoginFacilityID);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strSProc = "";
            if (Source == "OPD")
            {
                strSProc = "UspDiagSaveInvResultOP";
            }
            else if (Source == "IPD")
            {
                strSProc = "UspDiagSaveInvResultIP";
            }

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, strSProc, HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


    }

    public class clsMicrobiology
    {
        Hashtable HshIn;
        Hashtable HshOut;

        private string sConString = "";
        public clsMicrobiology(string conString)
        {
            sConString = conString;
        }
        public string source;
        public int iLabNo;
        public int iDiagSampleId;
        public int iResultId;
        public string xmlSensitivityFormat;
        public int iUserId;
        public int iServiceId;
        public int iFieldId;

        public bool CheckMicrobiologyResult(string Source, int iDiagSampleId, string sFieldType, string sOrganismIds, int iCount, int iResultId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            string strTableName = "";
            if (Source == "OPD")
            {
                strTableName = "DiagInvResultOP";
            }
            else
            {
                strTableName = "DiagInvResultIP";
            }
            HshIn.Add("@intResultId", iResultId);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@chrFieldType", sFieldType);
            string strSQL = "SELECT SUM(IDCount) AS MyCount FROM " +
                "( " +
                    " SELECT COUNT(dir.FieldId) AS IDCount FROM " + strTableName + " dir WITH (NOLOCK) " +
                    " INNER JOIN DiagFields df WITH (NOLOCK) ON df.FieldId = dir.FieldId AND df.FieldType = @chrFieldType " +
                    " WHERE dir.DiagSampleId = @intDiagSampleId AND dir.ResultId = @intResultId AND dir.Active = 1 " +
                    " UNION ALL " +
                    " SELECT COUNT(dir.FieldId) AS IDCount FROM " + strTableName + " dir WITH (NOLOCK) " +
                    " WHERE dir.DiagSampleId = @intDiagSampleId AND OrganismId IN (" + sOrganismIds + ") AND dir.ResultId = @intResultId AND dir.Active = 1 " +
                ") t ";
            try
            {

                ds = objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                if (ds.Tables != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //if (Convert.ToInt32(ds.Tables[0].Rows[0]["MyCount"]) == iCount)
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["MyCount"]) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }

        public DataSet GetLabSensitivityFormat(string sSource, int iDiagSampleId, string xmlOrganismIds, int iResultId, bool IsRelease)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@chrSource", sSource);
            HshIn.Add("@intDiagSampleId", iDiagSampleId);
            HshIn.Add("@inyResultId", iResultId);
            HshIn.Add("@xmlOrganismIds", xmlOrganismIds);
            HshIn.Add("@bitIsRelease", IsRelease);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabSensitivityFormat", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveData(clsMicrobiology objMicrobiology)
        {
            string sProcName = "";
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intLabNo", objMicrobiology.iLabNo);
            HshIn.Add("@intDiagSampleId", objMicrobiology.iDiagSampleId);
            HshIn.Add("@inyResultId", objMicrobiology.iResultId);
            HshIn.Add("@xmlSensitivityFormat", objMicrobiology.xmlSensitivityFormat);
            HshIn.Add("@intEncodedBy", objMicrobiology.iUserId);
            HshIn.Add("@intServiceId", objMicrobiology.iServiceId);
            HshIn.Add("@intFieldId", objMicrobiology.iFieldId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            if (source == "OPD")
            {
                sProcName = "uspDiagSaveSensitivityResultOP";
            }
            else
            {
                sProcName = "uspDiagSaveSensitivityResultIP";
            }
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, sProcName, HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDiagPatientLabField(int iHospitalLocationId, int iFacilityId, DateTime dfromDate, DateTime dtoDate, string cRegistrationNo, string cEncounterId)
        {
            string fDate = dfromDate.ToString("yyyy/MM/dd");
            string tDate = dtoDate.ToString("yyyy/MM/dd");

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@cRegistrationNo", cRegistrationNo);
            HshIn.Add("@cEncounterId", cEncounterId);
            HshIn.Add("@cDateFrom", fDate);
            HshIn.Add("@cDateTo", tDate);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDiagPatientLabField", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
