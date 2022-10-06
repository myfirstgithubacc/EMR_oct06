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
    public class clsDepartmentConsumption
    {
        private string sConString = "";
        public clsDepartmentConsumption(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int HospitalLocationId = 0;
        public int LoginFacilityId = 0;
        public int UserId = 0;
        public int ConsumptionId = 0;
        public int RegistrationId = 0;
        public int EncounterId = 0;
        public string ConsumptionNo = "";
        public int StoreId = 0;
        public string ConsumptionDate = "";
        public string Remarks = "";
        public string ProcessStatus = "O";
        public StringBuilder XMLItemDetails;
        public string sProc = "";
        public int ConsumptionDetailId = 0;
        public int InvoiceId = 0;//my14112016
        public string PostingDate = "";//RG 22062017
        public string uniqueNo = "";
        public DataSet GetDepartmentConsumptionDetails(int iHosID, int iFacilityId, int iStoreId, string strDocNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHosID);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intStoreId", iStoreId);
            HshIn.Add("chvDocumentNo", strDocNo);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrDepartmentConsumptionDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetDepartmentConsumptionDetails(int iHosID, int iFacilityId, int iStoreId, string strDocNo, string ProcessStatus)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", iHosID);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intStoreId", iStoreId);
                HshIn.Add("chvDocumentNo", strDocNo);
                HshIn.Add("chrProcessStatus", ProcessStatus);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrDepartmentConsumptionDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveDepartmentConsumption(clsDepartmentConsumption objDeptCon)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", objDeptCon.HospitalLocationId);
                HshIn.Add("intFacilityId", objDeptCon.LoginFacilityId);
                HshIn.Add("intStoreId", objDeptCon.StoreId);
                HshIn.Add("intConsumptionId", objDeptCon.ConsumptionId);
                HshIn.Add("dtConsumptionDate", Convert.ToDateTime(objDeptCon.ConsumptionDate));
                HshIn.Add("chrProcessStatus", objDeptCon.ProcessStatus);
                HshIn.Add("chvRemarks", objDeptCon.Remarks);
                HshIn.Add("intEncodedBy", objDeptCon.UserId);
                HshIn.Add("xmlItemDetails", objDeptCon.XMLItemDetails.ToString());
                HshOut.Add("chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("intDocId", SqlDbType.Int);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshIn.Add("intRegistrationId", objDeptCon.RegistrationId);
                HshIn.Add("intEncounterId", objDeptCon.EncounterId);
                HshIn.Add("@saveUniqueId", objDeptCon.uniqueNo);
                HshIn.Add("dtPostingDate", Convert.ToDateTime(objDeptCon.PostingDate));//RG 22062017

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objDeptCon.sProc, HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }



        public DataSet GetDepartmentConsumptionList(int iHosID, int iFacilityId, int iStoreId, string Status, string sFromDate, string sToDate, string strDocNo, int iCancelled)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", iHosID);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intStoreId", iStoreId);
                if (Status != "")
                    HshIn.Add("chrStatus", Status);
                if (sFromDate != "" && sToDate != "")
                {
                    HshIn.Add("dtFromDate", sFromDate);
                    HshIn.Add("dtToDate", sToDate);
                }
                HshIn.Add("chvDocumentNo", strDocNo);
                HshIn.Add("bitCancelled", iCancelled);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrDepartmentConsumptionList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Hashtable CancelDepartmentConsumption(clsDepartmentConsumption objDeptCon)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intConsumptionId", objDeptCon.ConsumptionId);
                HshIn.Add("intConsumptionDetailId", objDeptCon.ConsumptionDetailId);
                HshIn.Add("inyHospitalLocationId", objDeptCon.HospitalLocationId);
                HshIn.Add("intEncodedBy", objDeptCon.UserId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                return HshOut = (Hashtable)objDl.getOutputParametersValues(CommandType.StoredProcedure, objDeptCon.sProc, HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public Hashtable PostDepartmentConsumption(clsDepartmentConsumption objDeptCon)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objDeptCon.HospitalLocationId);
            HshIn.Add("intStoreId", objDeptCon.StoreId);
            HshIn.Add("intConsumptionId", objDeptCon.ConsumptionId);
            HshIn.Add("xmlItemDetails", objDeptCon.XMLItemDetails.ToString());
            HshIn.Add("intEncodedBy", objDeptCon.UserId);
            HshIn.Add("@saveUniqueId", objDeptCon.uniqueNo);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objDeptCon.sProc, HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


    }
}
