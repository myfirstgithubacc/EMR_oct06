using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace BaseC
{
    public class clsConsignment
    {
        private string sConString = "";

        public clsConsignment(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;

        public int HospitalLocationId = 0;
        public int LoginFacilityId = 0;
        public int ConsignmentId = 0;
        public int StoreId = 0;
        public int SupplierId = 0;
        public string dtConsignmentDate = "";
        public string chvGateEntryNo = "";
        public int ReceivedById = 0;
        public double NetAmount = 0;
        public string Remarks = "";
        public string ProcessStatus = "O";
        public string ChallanNo = "";
        public string ChallanDate = "";
        public int UserId = 0;
        public string sProc = "";
        public int intConsignmentId = 0;
        public StringBuilder xmlItemDetails;
        public StringBuilder XMLChargeDetails;
        public string chvConsignmentNo = "";

        public Hashtable SaveConsignment(clsConsignment objGrn)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
            HshIn.Add("intLoginFacilityId", objGrn.LoginFacilityId);
            HshIn.Add("intConsignmentId", objGrn.ConsignmentId);
            HshIn.Add("intStoreId", objGrn.StoreId);
            HshIn.Add("dtConsignmentDate", Convert.ToDateTime(objGrn.dtConsignmentDate));
            HshIn.Add("intSupplierId", objGrn.SupplierId);
            HshIn.Add("chvGateEntryNo", objGrn.chvGateEntryNo);
            HshIn.Add("intReceivedById", objGrn.ReceivedById);

            HshIn.Add("mnyNetAmount", objGrn.NetAmount);
            HshIn.Add("chvNarration", objGrn.Remarks);
            HshIn.Add("chrProcessStatus", objGrn.ProcessStatus);
            HshIn.Add("ChallanNo", objGrn.ChallanNo);
            HshIn.Add("ChallanDate", objGrn.ChallanDate);
            HshIn.Add("intEncodedBy", objGrn.UserId);
            HshIn.Add("xmlItemDetails", objGrn.xmlItemDetails.ToString());
            HshIn.Add("xmlChargeDetails", objGrn.XMLChargeDetails.ToString());
            HshIn.Add("chvConsignmentNo", objGrn.chvConsignmentNo);
            HshOut.Add("intId", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objGrn.sProc, HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getConsignmentDetailsDetails(clsConsignment objGrn)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
                HshIn.Add("intFacilityId", objGrn.LoginFacilityId);
                HshIn.Add("intStoreId", objGrn.StoreId);
                HshIn.Add("intConsignmentId", objGrn.intConsignmentId);
                HshIn.Add("chvConsignmentNo", objGrn.chvConsignmentNo);  //S Proc = uspGetPhrGRN
                return objDl.FillDataSet(CommandType.StoredProcedure, objGrn.sProc, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetConsignment(int intConsignmentId, string sSearchBy, string sSearchKeyword, int iStoreId, int iSupplierId, string Status, string sFromDate, string sToDate, int iHosID, int iFacilityId, int Cancelled)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intStoreId", iStoreId);
            HshIn.Add("@intConsignmentId", intConsignmentId);
            HshIn.Add("@chvSearchBy", sSearchBy);
            HshIn.Add("@chvSearchKeyword", sSearchKeyword);
            HshIn.Add("@intSupplierId", iSupplierId);
            HshIn.Add("@chrStatus", Status);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@bitCancelled", Cancelled);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrSearchConsignment", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetConsignmentReturn(string Procedure, string sSearchBy, string sSearchKeyword, int iStoreId, int iSupplierId, string Status, string sFromDate, string sToDate, int iHosID, int iFacilityId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intStoreId", iStoreId);
            HshIn.Add("@chvSearchBy", sSearchBy);
            HshIn.Add("@chvSearchKeyword", sSearchKeyword);
            HshIn.Add("@intSupplierId", iSupplierId);
            HshIn.Add("@chrStatus", Status);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, Procedure, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string PostConsignment(int HospitalLocationId, int FacilityId, string ProcessStatus, int ConsignmentId, int UserId)
        {

            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ProcessStatus", ProcessStatus);
            HshIn.Add("@ConsignmentId", ConsignmentId);
            HshIn.Add("@userId", UserId);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                int i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrConsignmentMain Set ProcessStatus=@ProcessStatus,LastChangedBy=@userId,LastChangedDate=GETDATE(),PostedById=@userId,PostedDate=GETDATE()  where HospitalLocationId=@inyHospitalLocationId and FacilityId=@intFacilityId and ConsignmentId=@ConsignmentId AND ACtive =1", HshIn);
                return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelConsignment(int HospitalLocationId, int FacilityId, string ProcessStatus, int ConsignmentId, int UserId)
        {

            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ProcessStatus", ProcessStatus);
            HshIn.Add("@ConsignmentId", ConsignmentId);
            HshIn.Add("@userId", UserId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                int i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrConsignmentMain Set ProcessStatus=@ProcessStatus,active=0,LastChangedBy=@userId,LastChangedDate=GETDATE(),CancelDate=GETUTCDATE() where HospitalLocationId=@inyHospitalLocationId and FacilityId=@intFacilityId and ConsignmentId=@ConsignmentId ", HshIn);


                return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetConsignmentSuppierwise(int SupplierId, int FacilityId, int HospitalLocationId, int storeId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intsupplierId", SupplierId);
            HshIn.Add("@intStoreId", storeId);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRgetConsignmentSupplierwise", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetConsignmentReturnDetails(string ProcedureName, int SupplierId, int FacilityId, int HospitalLocationId, int storeId, int ReturnId, String ReturnNo)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intsupplierId", SupplierId);
            HshIn.Add("@intStoreId", storeId);
            HshIn.Add("@intReturnId", ReturnId);
            HshIn.Add("@chrReturnNo", ReturnNo);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, ProcedureName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public Hashtable SavePOGRN(int HospitalLocationId, int LoginFacilityId, int StoreId,
            int SupplierId, int userId, string xmlItemDetails, int RegId, int EncId, string EncNo, int AdvisingDoctorId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intLoginFacilityId", LoginFacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("intEncodedBy", userId);
            HshIn.Add("xmlItemDetails", xmlItemDetails.ToString());
            HshIn.Add("intId", chvConsignmentNo);

            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@chvEncounterNo", EncNo);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            HshOut.Add("intPO", SqlDbType.VarChar);
            HshOut.Add("intGRN", SqlDbType.VarChar);
            HshOut.Add("intIPIssueId", SqlDbType.VarChar);

            HshIn.Add("AdvisingDoctorId", AdvisingDoctorId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrConsignmentConsumption", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveConsignmentReturn(string Procedure, int ConsignmentReturnId, int ConsignmentReturnNo, string GatePassNo, string ProcessStatus, string Remarks, int HospitalLocationId, int LoginFacilityId, int StoreId,
           int SupplierId, int userId, string xmlItemDetails, int Active)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intConsignmentReturnId", ConsignmentReturnId);
            HshIn.Add("chvConsignmentReturnNo", ConsignmentReturnNo);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("chvGatePassNo ", GatePassNo);
            HshIn.Add("chrProcessStatus ", ProcessStatus);
            HshIn.Add("intFacilityId", LoginFacilityId);
            HshIn.Add("xmlConsignmentReturnDetails", xmlItemDetails.ToString());
            HshIn.Add("chvRemarks", Remarks);
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("bitActive", Active);
            HshIn.Add("intEncodedBy", userId);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("chvDocumentNo", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, Procedure, HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable CancelConsignmentReturn(string Procedure, int ConsignmentReturnId, int HospitalLocationId, int userId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intConsignmentReturnId", ConsignmentReturnId);
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intEncodedBy", userId);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, Procedure, HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
