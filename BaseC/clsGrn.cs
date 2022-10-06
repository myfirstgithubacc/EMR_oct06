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
    public class clsGrn
    {
        private string sConString = "";
        public clsGrn(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        StringBuilder sbSQL;
        public int HospitalLocationId = 0;
        public int LoginFacilityId = 0;
        public int UserId = 0;
        public int GRNId = 0;
        public string GRNNo = "";
        public int StoreId = 0;
        public string GRNDate = "";
        public int SupplierId = 0;
        public int PurchaseOrderId = 0;
        public string PONo = "";
        public string BillNo = "";
        public string BillDate = "";
        public double BillAmount = 0;
        public string GatePassNo = "";
        public int ReceivedById = 0;
        public Boolean BillProcessedForPayment = false;
        public double NetAmount = 0;
        public double TotalOtherChargeAmount = 0;
        public double TotalAmount = 0;
        public decimal RoundOffAmount = 0;
        public string Remarks = "";
        public string ProcessStatus = "O";
        public string ChallanNo = "";
        public string ChallanDate = "";
        public StringBuilder XMLItemBatch;
        public StringBuilder XMLChargeDetails;
        public StringBuilder XMLOtherCharges;
        public StringBuilder XMLInspectedBy;
        public string sProc = "";
        public int YearId = 0;
        public int GRNTypeId = 0;
        public string IPAddress = "";
        public string ReferenceNo = "";

        public DataSet GetGRN(int iGRNId, string sSearchBy, string sSearchKeyword, int iStoreId, int iSupplierId, string Status, string sFromDate, string sToDate, int iHosID, int iFacilityId, int BillProcessPending, int WitoutPO, int GRNTypeId)
        {

            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intStoreId", iStoreId);
            HshIn.Add("@intSupplierId", iSupplierId);
            HshIn.Add("@chrStatus", Status);
            HshIn.Add("@intGRNId", iGRNId);
            HshIn.Add("@chvSearchBy", sSearchBy);
            HshIn.Add("@chvSearchKeyword", sSearchKeyword);
            HshIn.Add("@chrFromDate", sFromDate);
            HshIn.Add("@chrToDate", sToDate);
            HshIn.Add("@bitBillProcessPending", BillProcessPending);
            HshIn.Add("@bitWithouPO", WitoutPO);
            HshIn.Add("@intGRNTypeId", GRNTypeId);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrSearchGRN", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public Hashtable SaveGRN(clsGrn objGrn)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
            HshIn.Add("intLoginFacilityId", objGrn.LoginFacilityId);
            HshIn.Add("intGRNId", objGrn.GRNId);
            HshIn.Add("intStoreId", objGrn.StoreId);
            HshIn.Add("dtGRNDate", Convert.ToDateTime(objGrn.GRNDate));
            HshIn.Add("intSupplierId", objGrn.SupplierId);
            if (PurchaseOrderId > 0)
                HshIn.Add("intPurchaseOrderId", objGrn.PurchaseOrderId);
            if (BillNo != "")
            {
                HshIn.Add("chvBillNo", objGrn.BillNo);
                HshIn.Add("dtBillDate", Convert.ToDateTime(objGrn.BillDate));
            }
            if (BillAmount > 0)
                HshIn.Add("mnyBillAmount", objGrn.BillAmount);
            if (GatePassNo != "")
                HshIn.Add("chvGateEntryNo", objGrn.GatePassNo);
            HshIn.Add("intReceivedById", objGrn.ReceivedById);
            HshIn.Add("mnyNetAmount", objGrn.NetAmount);
            HshIn.Add("mnyTotalOtherChargeAmount", objGrn.TotalOtherChargeAmount);
            HshIn.Add("mnyTotalAmount", objGrn.TotalAmount);
            HshIn.Add("RoundOffAmount", objGrn.RoundOffAmount);
            if (Remarks != "")
                HshIn.Add("chvNarration", objGrn.Remarks);
            HshIn.Add("chrProcessStatus", objGrn.ProcessStatus);
            HshIn.Add("bitBillProcessedForPayment", Convert.ToInt16(objGrn.BillProcessedForPayment));
            if (ChallanNo != "")
            {
                HshIn.Add("ChallanNo", objGrn.ChallanNo);
                HshIn.Add("ChallanDate", Convert.ToDateTime(objGrn.ChallanDate));
            }
            HshIn.Add("intEncodedBy", objGrn.UserId);
            HshIn.Add("xmlItemDetails", objGrn.XMLItemBatch.ToString());
            HshIn.Add("xmlChargeDetails", objGrn.XMLChargeDetails.ToString());
            if (objGrn.XMLOtherCharges != null)
                HshIn.Add("xmlOtherChargeDetails", objGrn.XMLOtherCharges.ToString());
            if (objGrn.XMLInspectedBy != null)
                HshIn.Add("xmlInspectedBy", objGrn.XMLInspectedBy.ToString());
            HshIn.Add("chrReferenceNo", objGrn.ReferenceNo);
            //@chrReferenceNo
            HshOut.Add("intId", SqlDbType.VarChar);
            HshOut.Add("chvGRNNo", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objGrn.sProc, HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemPurchaseAndIssueUnit(int ItemId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intItemId", ItemId);
                sbSQL = new StringBuilder();
                sbSQL.Append("SELECT mt.ItemUnitId, mt.ItemUnitName, mt.IssueUnitId,um1.UnitName AS IssueUnit, ");
                sbSQL.Append(" mt.PurchaseUnitId1, um2.UnitName AS PurchaseUnit1,  mt.PurchaseUnitId2, um3.UnitName AS PurchaseUnit2, ");
                sbSQL.Append(" mt.ConversionFactor1, mt.ConversionFactor2, ISNULL(tr.IsDefault,0) AS IsDefault ");
                sbSQL.Append(" FROM PhrItemUnitMaster mt WITH (NOLOCK) ");
                sbSQL.Append(" INNER JOIN PhrItemWithItemUnitTagging tr WITH (NOLOCK) ON mt.ItemUnitId = tr.ItemUnitId AND tr.Active = 1 AND tr.ItemId = @intItemId ");
                sbSQL.Append(" INNER JOIN PhrUnitMaster um1 WITH (NOLOCK) ON mt.IssueUnitId = um1.UnitId ");
                sbSQL.Append(" INNER JOIN PhrUnitMaster um2 WITH (NOLOCK) ON mt.PurchaseUnitId1 = um2.UnitId ");
                sbSQL.Append(" INNER JOIN PhrUnitMaster um3 WITH (NOLOCK) ON mt.PurchaseUnitId2 = um3.UnitId ");
                sbSQL.Append(" WHERE mt.Active = 1 ORDER BY mt.ItemUnitName ");
                return objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getGRNDetails(clsGrn objGrn)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
                HshIn.Add("intFacilityId", objGrn.LoginFacilityId);
                HshIn.Add("intStoreId", objGrn.StoreId);
                HshIn.Add("intGRNId", objGrn.GRNId);
                HshIn.Add("chvGRNNo", objGrn.GRNNo);
                HshIn.Add("@intGRNTypeId", GRNTypeId);//S Proc = uspGetPhrGRN
                return objDl.FillDataSet(CommandType.StoredProcedure, objGrn.sProc, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getGRNPODetails(clsGrn objGrn)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
                HshIn.Add("intFacilityId", objGrn.LoginFacilityId);
                HshIn.Add("intStoreId", objGrn.StoreId);
                HshIn.Add("intPurchaseOrderId", objGrn.PurchaseOrderId);
                HshIn.Add("chvPONo", objGrn.PONo);  //S Proc = uspGetPhrPODetailsGRN
                return objDl.FillDataSet(CommandType.StoredProcedure, objGrn.sProc, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Hashtable GRNCancel(clsGrn objGrn)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intGRNId", objGrn.GRNId);
                HshIn.Add("intEncodedBy", objGrn.UserId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                return HshOut = (Hashtable)objDl.getOutputParametersValues(CommandType.StoredProcedure, objGrn.sProc, HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable PostGRN(clsGrn objGrn)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
            HshIn.Add("intStoreId", objGrn.StoreId);
            HshIn.Add("intGRNId", objGrn.GRNId);
            HshIn.Add("xmlItemDetails", objGrn.XMLItemBatch.ToString());
            HshIn.Add("intEncodedBy", objGrn.UserId);
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

        public Hashtable UpdateBillDetails(clsGrn objGrn)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intGRNId", objGrn.GRNId);
            HshIn.Add("chvBillNo", objGrn.BillNo);
            HshIn.Add("dtBillDate", Convert.ToDateTime(objGrn.BillDate));
            HshIn.Add("mnyBillAmount", objGrn.BillAmount);
            HshIn.Add("intEncodedBy", objGrn.UserId);
            HshIn.Add("ChallanNo", objGrn.ChallanNo);
            HshIn.Add("ChallanDate", Convert.ToDateTime(objGrn.ChallanDate));
            HshIn.Add("chvGateEntryNo", objGrn.GatePassNo);

            HshOut.Add("intId", SqlDbType.VarChar);
            HshOut.Add("chvGRNNo", SqlDbType.VarChar);
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

        public Hashtable UpdateItemBatchDetails(clsGrn objGrn)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objGrn.HospitalLocationId);
            HshIn.Add("intFacilityId", objGrn.LoginFacilityId);
            HshIn.Add("intStoreId", objGrn.StoreId);
            HshIn.Add("inyYearId", objGrn.YearId);
            HshIn.Add("chvRemarks", objGrn.Remarks);
            HshIn.Add("chvEncodedIPAddress", objGrn.IPAddress);
            HshIn.Add("intEncodedBy", objGrn.UserId);
            HshIn.Add("xmlbatchDetails", objGrn.XMLItemBatch.ToString());
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrUpdateBatchDetails", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemBatchAudit(int hospId, int loginfacilityId, int facilityId, int storeId, int itemId, DateTime fromdate, DateTime todate)
        {

            HshIn = new Hashtable();
            string fDate = fromdate.ToString("yyyy-MM-dd 00:00");
            string tDate = todate.ToString("yyyy-MM-dd 23:59");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", hospId);
                HshIn.Add("intLoginFacilityId", loginfacilityId);
                HshIn.Add("intFacilityId", facilityId);
                HshIn.Add("intStoreId", storeId);
                HshIn.Add("intItemId", itemId);
                HshIn.Add("dtFromdate", fDate);
                HshIn.Add("dtTodate", tDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemBatchAudit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getItemDetails(int HospitalId, int FacilityId, int ItemId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@HospitalId", HospitalId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@ItemId", ItemId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspItemDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
    }
}
