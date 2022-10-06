using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class clsMRD
    {
        string sConString = "";
        Hashtable HshIn;
        Hashtable hstOutput;

        public clsMRD(string Constring)
        {
            sConString = Constring;
        }
        public DataSet GetMRDIssueFile(int StatusId, int RegistrationId, int HospitalLocationId, int FacilityId,
                                 string OPIP, string StatusType, DateTime FromDate, DateTime ToDate,
            string sPatientName, string sRegNo, string sEncNo, string sMobileNo)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string fDate = FromDate.ToString("yyyy/MM/dd 00:00");
                string tDate = ToDate.ToString("yyyy/MM/dd 23:59");

                HshIn.Add("@intStatusId", 0);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@chvStatusType", StatusType);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);

                HshIn.Add("@chvPatientName", sPatientName);
                HshIn.Add("@chvRegistrationNo", sRegNo);
                HshIn.Add("@chvEncounterNo", sEncNo);
                HshIn.Add("@chvMobileNo", sMobileNo);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDIssueFile", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable UpdateRequestReturnStatus(int RegistrationId, int EncounterId, int IssueId, bool IsWard, int ReturnBy)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intReturnedBy", ReturnBy);
                HshIn.Add("@bitIsWard", IsWard);
                hstOutput.Add("@chvOutPut", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateMRDFileRequestReturnStatus", HshIn, hstOutput);
                return hstOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveFileReturn(int RegistrationId, int EncounterId, int EncodedBy, int IssueId)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                hstOutput.Add("@chvOutPut", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveMRDFileReturnAcknowledge", HshIn, hstOutput);
                return hstOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICDCode()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetICDCode", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetICDFlag(int ICDId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@ICDId", ICDId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetICDFlag", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetICDFlagMaster()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetICDFlagMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveICDFlagMaster(int iItemFlagId, string sItemFlagName, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@intICDFlagId", iItemFlagId);
            HshIn.Add("@chvICDFlagName", sItemFlagName.Trim());
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);
            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveICDFlagMaster", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTaggedICDFlag(int ICDId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@ICDId", ICDId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTaggedICDFlag", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveICDFlagTagging(int ItemId, string xmlItemFlagIds, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intICDId", ItemId);
            HshIn.Add("@xmlItemFlagIds", xmlItemFlagIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveICDFlagTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSearchICDByName(string text)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@strSearchText", text);
                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchICDByName", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveICD9Disease(int icdId, string ICDCode, string ICDName, int intGroupid, int intSubGroupid, int Active, int UserCode)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intICDId", icdId);
            HshIn.Add("@chrICDDescription", ICDName);
            HshIn.Add("@intGroupId", intGroupid);
            HshIn.Add("@intSubGroupid", intSubGroupid);
            HshIn.Add("@chrICDCode", ICDCode);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intUserCode", UserCode);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveICD9DiseaseCode", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetIcdCodeList(int GroupId, string SubGroupId, int PageIndex, int PageSize, string Icd9Desc)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@subGroupId", SubGroupId);
                HshIn.Add("@PageIndex", PageIndex);
                HshIn.Add("@PageSize", PageSize);
                HshIn.Add("@icd9Desc", Icd9Desc);
                return objDl.FillDataSet(CommandType.StoredProcedure, "getIcd9CodeList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getIcd9SubGroup(int GroupId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@GroupId", GroupId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "getIcd9SubGroup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getIcd9Group()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "getIcd9Group", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getIcd9SubGroupDetail()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "getIcd9SubGroupDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveICD9SubGroupMaster(int intGroupId, int intSubGroupId, string chvICD9SubGroupName, string chvRangeStart,
            string chvRangeEnd, int iHosID, int bitActive, int iuserId)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@intGroupId", intGroupId);
            HshIn.Add("@intSubGroupId", intSubGroupId);
            HshIn.Add("@chvICD9SubGroupName", chvICD9SubGroupName.Trim());
            HshIn.Add("@chvRangeStart", chvRangeStart);
            HshIn.Add("@chvRangeEnd", chvRangeEnd);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", bitActive);
            HshIn.Add("@intEncodedBy", iuserId);
            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveICD9SubGroup", HshIn, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
