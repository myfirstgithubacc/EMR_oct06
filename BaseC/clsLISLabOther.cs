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
    public class clsLISLabOther
    {
        Hashtable HshIn;
        Hashtable HshOut;

        private string sConString = "";
        public clsLISLabOther(string conString)
        {
            sConString = conString;
        }

        public string SaveServiceLink(int intId, int iHospitalLocationId, int iServiceID, string sCPTCode,
                        int iExternalCenterId, string sCustomName, string sCustomCode, string sDescription,
                        int iAOE, int iRequisitionForm, int iSpecimenLabel, int iManiFestForm, string sComments,
                        int sActive, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intID", intId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intServiceId", iServiceID);
                HshIn.Add("@chvCPTCode", sCPTCode);
                HshIn.Add("@intExternalCenterId", iExternalCenterId);
                HshIn.Add("@chvCustomName", sCustomName);
                HshIn.Add("@chvCustomCode", sCustomCode);
                HshIn.Add("@chvDescription", sDescription);
                HshIn.Add("@intAOE", iAOE);
                HshIn.Add("@intRequisitionForm", iRequisitionForm);
                HshIn.Add("@intSpecimenLabel", iSpecimenLabel);
                HshIn.Add("@intManiFestForm", iManiFestForm);
                HshIn.Add("@chvComments", sComments);
                HshIn.Add("@bitActive", sActive);
                HshIn.Add("@intEncodedBy", iEncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveServiceLinkCode", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetServiceLink(int iServiceId, int iExternalCenterId, int iAOE, int iRequisitionForm,
                                        int iSpecimenLabel, int iManiFestForm, int iHospitalLocationId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intServiceID", iServiceId);
                HshIn.Add("@intExternalCenterId", iExternalCenterId);
                HshIn.Add("@intAOE", iAOE);
                HshIn.Add("@intRequisitionForm", iRequisitionForm);
                HshIn.Add("@intSpecimenLabel", iSpecimenLabel);
                HshIn.Add("@intManiFestForm", iManiFestForm);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetServiceLinkCode", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAOEList()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                string query = "";

                HshIn = new Hashtable();

                HshOut = new Hashtable();

                query += " SELECT ET.TemplateName, ET.Id, ET.Code ";
                query += " FROM EMRTemplate ET WITH (NOLOCK) ";
                query += " INNER JOIN EMRTemplateTypes ETT WITH (NOLOCK) ON ET.TemplateTypeID = ETT.ID  ";
                query += " WHERE ETT.TypeName = 'AOE (Ask at Order Exam)' ";

                return objDl.FillDataSet(CommandType.Text, query, HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetRequisitionForm()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshOut = new Hashtable();

                return objDl.FillDataSet(CommandType.Text, "Select ID, RequisitionName FROM EMRRequisitionForm WITH (NOLOCK) ORDER BY RequisitionName", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSpecimenLabel()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshOut = new Hashtable();

                return objDl.FillDataSet(CommandType.Text, "Select ID, SpecimenName FROM EMRSpecimenLabel WITH (NOLOCK) ORDER BY SpecimenName", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetManifestForm()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshOut = new Hashtable();

                return objDl.FillDataSet(CommandType.Text, "Select ID, ManifestName FROM EMRManifestForm WITH (NOLOCK) ORDER BY ManifestName", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveLabFormLink(int intId, int iHospitalLocationId, int iExternalCenterId, int iRequisitionForm, int iSpecimenLabel,
                                        int iManiFestForm, int iActive, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intID", intId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intExternalCenterId", iExternalCenterId);
                HshIn.Add("@intRequisitionForm", iRequisitionForm);
                HshIn.Add("@intSpecimenLabel", iSpecimenLabel);
                HshIn.Add("@intManiFestForm", iManiFestForm);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intEncodedBy", iEncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveLabFormLink", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetLabFormLink(int iExternalCenterId, int iRequisitionForm, int iSpecimenLabel,
                                      int iManiFestForm, int iHospitalLocationId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intExternalCenterId", iExternalCenterId);
                HshIn.Add("@intRequisitionForm", iRequisitionForm);
                HshIn.Add("@intSpecimenLabel", iSpecimenLabel);
                HshIn.Add("@intManiFestForm", iManiFestForm);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabFormLink", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable getLabCPTCode(int iHospitalLocationId, int EncodedBy, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationId", iHospitalLocationId);

                //return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetServiceCategories", HshIn);
                //DataView DV = ds.Tables[0].DefaultView;
                //DV.RowFilter = "Type IN ('I')  ";
                //DV.RowFilter = "DepartmentName = 'Pathology and Laboratory Procedures' ";

                string qry = "SELECT DISTINCT dm.DepartmentID, dm.DepartmentName " +
                            " FROM  DepartmentMain dm WITH (NOLOCK) INNER JOIN DepartmentSub ds WITH (NOLOCK) ON dm.DepartmentID = ds.DepartmentID AND ds.Active = 1 " +
                            " WHERE ds.Type = 'I' AND dm.Active = 1 AND (dm.HospitalLocationId = @HospitalLocationId OR dm.HospitalLocationId IS NULL) ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                int DepartmentId = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DepartmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["DepartmentID"]);
                }

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intDepartmentId", DepartmentId); //"1011136");
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("intFacilityId", FacilityId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServices", HshIn, HshOut);

                DataView DV = ds.Tables[0].DefaultView;
                DV.RowFilter = "CPTCode <> '' AND CPTCode <> 0";
                DV.Sort = "CPTCode";

                return DV.ToTable();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet GetLabTestResult(int iHospId, int iLoginFacilityId, int iLabNo, int iOrderId, int iRegistrationId, int iProviderId,
                                int iEncounterId, int iReviewedStatus, DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intOrderId", iOrderId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intReviewedStatus", iReviewedStatus);
                HshIn.Add("@dtsResultFromDate", fDate);
                HshIn.Add("@dtsResultToDate", tDate);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultOP", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDiagPrintLabResultService(string Source, int LoginFacilityId, int FacilityId, DateTime dtsDateFrom, DateTime dtsDateTo,
            int RefBy, int RegistrationId, int ServiceId, int StatusId, bool AbnormalResult, bool CriticalResult, int EntrySiteId, bool Stat, string OPDSourceType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@chrSource", Source);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtDateFrom", fDate);
                HshIn.Add("@dtDateTo", tDate);
                HshIn.Add("@intReferredBy", RefBy);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intServiceId", ServiceId);
                HshIn.Add("@intStatusId", StatusId);
                HshIn.Add("@bitAbnormalResult", AbnormalResult);
                HshIn.Add("@bitCriticalResult", CriticalResult);
                HshIn.Add("@inyEntrySiteId", EntrySiteId);
                HshIn.Add("@bitStat", Stat);
                HshIn.Add("@chrOPDSourceType", OPDSourceType);




                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagPrintLabResultService", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetLabTestResultForOrder(int iHospId, int iLoginFacilityId, int iLabNo, int iOrderId, int iRegistrationId, int iProviderId,
                                int iEncounterId, int iReviewedStatus, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intOrderId", iOrderId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intReviewedStatus", iReviewedStatus);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultForOrderOP", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //public DataSet GetLabTestResultForNote(int iHospId, int iLoginFacilityId, int iLabNo, int iEncounterId, int EncodedBy)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    HshIn.Add("@inyHospitalLocationId", iHospId);
        //    HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
        //    HshIn.Add("@intLabNo", iLabNo);
        //    HshIn.Add("@intEncounterId", iEncounterId);
        //    HshIn.Add("@intEncodedBy", EncodedBy);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    DataSet ds = new DataSet();
        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultForNoteOP", HshIn, HshOut);

        //    return ds;
        //}

        public DataSet GetLabTestResultForNote(int iEncounterId, int HospId, int FacilityId, string OPIP, bool bShowAllParameters)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", iEncounterId);
                //HshIn.Add("@bitShowAllParameters", bShowAllParameters);

                string sp = "uspDiagLabTestResultForNoteOP";
                if (OPIP == "I")
                {
                    sp = "uspDiagLabTestResultForNoteIP";
                }


                return objDl.FillDataSet(CommandType.StoredProcedure, sp, HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetLabTestResultForNote(int iHospId, int iLoginFacilityId, int iLabNo, int iEncounterId, int RegistrationId, int EncodedBy,
            bool bShowAllParameters)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                //HshIn.Add("@bitShowAllParameters", bShowAllParameters);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultForNoteOP", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetDiagLabSensitivityResultForNote(int iDiagSampleId, int iResultId)
        {
            return GetDiagLabSensitivityResultForNote(iDiagSampleId, iResultId, string.Empty);
        }
        public DataSet GetDiagLabSensitivityResultForNote(int iDiagSampleId, int iResultId, string Source)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            // Change by kuldeep kumar 22/01/2021 
            try
            {

                HshIn.Add("@intDiagSampleId", iDiagSampleId);
                HshIn.Add("@intResultId", iResultId);

                if (!Source.Equals(string.Empty))
                {
                    HshIn.Add("@chrSource", Source);
                }

                
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabSensitivityResultForNote", HshIn);

                
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetLabTestResultIP(int iHospId, int iLoginFacilityId, int iLabNo, int iOrderId,
      int iRegistrationId, int iProviderId,
                             int iEncounterId, int iReviewedStatus, DateTime dtsDateFrom,
      DateTime dtsDateTo, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd 00:00");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intLabNo", iLabNo);
                HshIn.Add("@intOrderId", iOrderId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intReviewedStatus", iReviewedStatus);
                HshIn.Add("@dtsResultFromDate", fDate);
                HshIn.Add("@dtsResultToDate", tDate);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultIP", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string updateReviewedResult(int resultId, int reviewedStatus, DateTime reviewedDate, string reviewedComments,
                                    int reviewedBy, string LabFlagValue, string TestResultStatus)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string retstr = string.Empty;
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@ResultId", resultId);
                HshIn.Add("@ReviewedStatus", reviewedStatus);
                HshIn.Add("@ReviewedDate", reviewedDate);
                HshIn.Add("@ReviewedComments", reviewedComments);
                HshIn.Add("@ReviewedBy", reviewedBy);
                HshIn.Add("@LabFlagValue", LabFlagValue);
                HshIn.Add("@TestResultStatus", TestResultStatus);

                // string strqry = " UPDATE DiagInvResultIP SET ReviewedStatus = @ReviewedStatus, ReviewedDate=@ReviewedDate, ReviewedComments = @ReviewedComments, ReviewedBy = @ReviewedBy, LabFlagValue=@LabFlagValue, TestResultStatus=@TestResultStatus WHERE Id = @ResultId ";
                string strqry = " UPDATE DiagInvResultOP SET ReviewedStatus = @ReviewedStatus, ReviewedDate=@ReviewedDate, ReviewedComments = @ReviewedComments, ReviewedBy = @ReviewedBy, LabFlagValue=@LabFlagValue, TestResultStatus=@TestResultStatus WHERE Id = @ResultId ";

                int execute = objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

                return retstr = "Record Updated...";

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }



        public string updateReviewedResultByDiagSampleId(string xmlDiagSampleId, int reviewedStatus, DateTime reviewedDate, string reviewedComments,
                           int reviewedBy, string LabFlagValue, string TestResultStatus, int iNoSMS)
        {
            string retstr = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@xmlDiagSampleId ", xmlDiagSampleId);
                HshIn.Add("@sReviewedStatus", reviewedStatus);
                HshIn.Add("@sReviewedDate", reviewedDate);
                HshIn.Add("@sReviewedComments", reviewedComments.Split('$')[0]);
                HshIn.Add("@iReviewedBy", reviewedBy);
                HshIn.Add("@cLabFlagValue", LabFlagValue);
                HshIn.Add("@cTestResultStatus", TestResultStatus);
                HshIn.Add("@iActive", 1);
                HshIn.Add("@opip", reviewedComments.Split('$')[1]);
                HshIn.Add("@iNoSMS", iNoSMS);

                int execute = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspDiagInsertReviewedStatus", HshIn);

                if (execute == 0)
                {
                    retstr = "Record Updated...";
                }
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
            return retstr;
        }


        public string updateReviewedResultByDiagSampleId(int DiagSampleId, int reviewedStatus, DateTime reviewedDate, string reviewedComments,
                                   int reviewedBy, string LabFlagValue, string TestResultStatus, string opip)
        {
            string retstr = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@iDiagSampleId", DiagSampleId);
                HshIn.Add("@sReviewedStatus", reviewedStatus);
                HshIn.Add("@sReviewedDate", reviewedDate);
                HshIn.Add("@sReviewedComments", reviewedComments);
                HshIn.Add("@iReviewedBy", reviewedBy);
                HshIn.Add("@cLabFlagValue", LabFlagValue);
                HshIn.Add("@cTestResultStatus", TestResultStatus);
                HshIn.Add("@iActive", 1);
                HshIn.Add("@opip", opip);


                int execute = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspDiagInsertReviewedStatus", HshIn);

                if (execute == 0)
                {
                    retstr = "Record Updated...";
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return retstr;
        }

        public string updateNotesResult(string resultIds, int isNote, int IsShowNote, int iPageId,
                                        int iEncounterId, int iRegistrationId)
        {
            string retstr = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                string strqry = " UPDATE DiagInvResultOP SET IsNote = " + isNote + " WHERE Id IN(" + resultIds + ") ";

                int execute = objDl.ExecuteNonQuery(CommandType.Text, strqry);

                if (execute == 0)
                {
                    retstr = "Record Updated...";
                }

                if (isNote == 1)
                {
                    HshIn = new Hashtable();
                    HshIn.Add("@IsShowNote", IsShowNote);
                    HshIn.Add("@iPageId", iPageId);
                    HshIn.Add("@iEncounterId", iEncounterId);
                    HshIn.Add("@iRegistrationId", iRegistrationId);

                    strqry = "UPDATE EMRPatientFormDetails SET ShowNote = @IsShowNote " +
                            " FROM EMRPatientForms epf WITH (NOLOCK) " +
                            " INNER JOIN EMRPatientFormDetails epfd WITH (NOLOCK) ON epf.PatientFormId = epfd.PatientFormId AND epfd.PageId = @iPageId " +
                            " WHERE epf.EncounterId = @iEncounterId AND epf.RegistrationId = @iRegistrationId " +
                            " AND epf.Active = 1";

                    execute = objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return retstr;
        }

        public DataSet getResultData(int resultId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@ResultId", resultId);

                string qry = "SELECT ReviewedStatus, ReviewedDate, ReviewedComments, ReviewedBy, LabFlagValue, TestResultStatus FROM DiagInvResultOP WITH (NOLOCK) WHERE ID = @ResultId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getResultReviewedData(int DiagSampleId, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                string qry = "";
                HshIn.Add("@DiagSampleId", DiagSampleId);
                if (OPIP == "IPD")
                {
                    qry = "SELECT ReviewedStatus, ReviewedDate, ReviewedComments, ReviewedBy, LabFlagValue, TestResultStatus, Active FROM DiagResultReviewStatusIP WITH (NOLOCK)  WHERE DiagSampleId = @DiagSampleId and Active = 1 ";
                }
                else
                {
                    qry = "SELECT ReviewedStatus, ReviewedDate, ReviewedComments, ReviewedBy, LabFlagValue, TestResultStatus, Active FROM DiagResultReviewStatusOP WITH (NOLOCK)  WHERE DiagSampleId = @DiagSampleId and Active = 1 ";
                }
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getLabRegistration(int iHospId, int iProviderId, int iEncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospitalLocationId", iHospId);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@intEncounterId", iEncounterId);

                string qry = "SELECT DISTINCT dsom.RegistrationId, reg.RegistrationNo " +
                            " FROM DiagSampleOPLabMain dsom WITH (NOLOCK) " +
                            " LEFT JOIN Registration reg WITH (NOLOCK) ON dsom.RegistrationId = reg.Id " +
                            " LEFT JOIN OPServiceOrderDetail ODR WITH (NOLOCK) ON dsom.OrderId = ODR.OrderId " +
                            " WHERE ODR.EncounterId = CASE WHEN @intEncounterId = 0 THEN ODR.EncounterId ELSE @intEncounterId END " +
                            " AND ODR.DoctorId = CASE WHEN @intProviderId = 0 THEN ODR.DoctorId ELSE @intProviderId END " +
                            " AND dsom.HospitalLocationId = @HospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveDiagReviewed(int iHospitalLocationId, string xmlResult, bool reviewedStatus, int reviewedBy, string LabFlagValue,
                                        string TestResultStatus)
        {
            string chvErrorStatus = string.Empty;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {
                HshIn.Add("@xmlResult", xmlResult);
                HshIn.Add("@sReviewedStatus", reviewedStatus);
                HshIn.Add("@iReviewedBy", reviewedBy);
                HshIn.Add("@cLabFlagValue", LabFlagValue);
                HshIn.Add("@cTestResultStatus", TestResultStatus);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDiagSaveReviewed", HshIn, HshOut);
                chvErrorStatus = HshOut["@chvErrorStatus"].ToString();
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
            return chvErrorStatus;
        }
    }
}
