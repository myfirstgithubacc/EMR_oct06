using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace BaseC
{
    public class clsIndent
    {
        private string sConString = "";
        public clsIndent(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        StringBuilder sbSQL;
        public int HospitalLocationId = 0;
        public int LoginFacilityId = 0;
        public int StoreId = 0;
        public int UserId = 0;
        public int POIndentId = 0;
        public string POIndentDate = "";
        public string Remarks = "";
        public string IndentStatus = "O";
        public int Active = 0;
        public string Source = "";
        public StringBuilder XMLItem;
        public int IndentFromROL = 0;
        public string IndentRemark = "";


        public DataSet getStockReOrderQuantity(int StoreId, int FacilityId, int ConsumptionDays, int MinDaysStock, int MaxDaysStock, string strItemSubCategory)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intConsumptionDays", ConsumptionDays);
                HshIn.Add("intMinDaysStock", MinDaysStock);
                HshIn.Add("intMaxDaysStock", MaxDaysStock);
                HshIn.Add("chvItemSubCategory", strItemSubCategory);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetStockReOrderQty", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Hashtable SavePOIndent(clsIndent objIndent)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("intIndentId", objIndent.POIndentId);
            HshIn.Add("inyHospitalLocationId", objIndent.HospitalLocationId);
            HshIn.Add("intFacilityId", objIndent.LoginFacilityId);
            HshIn.Add("intStoreId", objIndent.StoreId);
            HshIn.Add("dtPOIndentDate", Convert.ToDateTime(objIndent.POIndentDate));
            HshIn.Add("intEncodedBy", objIndent.UserId);
            HshIn.Add("xmlItemDetails", objIndent.XMLItem.ToString());
            HshIn.Add("chrSource", objIndent.Source);
            HshIn.Add("chrProcessStatus", objIndent.IndentStatus);
            HshIn.Add("bitIndentFromROL", IndentFromROL);
            HshIn.Add("strRemark", IndentRemark);

            HshOut.Add("chvPOIndentNo", SqlDbType.VarChar);
            HshOut.Add("intId", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSavePOIndent", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable ApprovePOIndent(clsIndent objIndent)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", objIndent.HospitalLocationId);
            HshIn.Add("intPOIndentId", objIndent.POIndentId);
            HshIn.Add("intEncodedBy", objIndent.UserId);
            HshIn.Add("xmlItemDetails", objIndent.XMLItem.ToString());
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrApprovePOIndent", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPOIndentList(int iHospId, int iFacilityId, int iStoreId, string sPOIndentNo, string sFromDate, string sToDate, string ProcessStatus, string IndentType, String usefor, string cisApproved)
        {

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intStoreId", iStoreId);
            HshIn.Add("chvPOIndentNo", sPOIndentNo);
            HshIn.Add("chrFromDate", sFromDate);
            HshIn.Add("chrToDate", sToDate);
            HshIn.Add("chrProcessStatus", ProcessStatus);
            HshIn.Add("chrIndentType", IndentType);
            HshIn.Add("chrUseFor", usefor);
            HshIn.Add("isApproved", cisApproved);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrSearchPOIndent", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPOIndentDetails(int iHospId, int iFacilityId, int iStoreId, string sPOIndentNo, int POIndentId, string Status)
        {

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intStoreId", iStoreId);
            HshIn.Add("intPOIndentId", POIndentId);
            HshIn.Add("chvPOIndentNo", sPOIndentNo);
            HshIn.Add("chrStatus", Status);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetPOIndentDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPOIndentDetails(int iHospId, int iFacilityId, int iStoreId, string sPOIndentNo, int POIndentId, string Status, int Urgent, string SortBy, string ItemName, int SupplierId)
        {

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intStoreId", iStoreId);
            HshIn.Add("intPOIndentId", POIndentId);
            HshIn.Add("chvPOIndentNo", sPOIndentNo);
            HshIn.Add("chrStatus", Status);
            HshIn.Add("bitUrgent", Urgent);
            HshIn.Add("chvSortBy", SortBy);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("intSupplierId", SupplierId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetPOIndentDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPOIndentDetailsAll(int iHospId, int iFacilityId, int iStoreId, string sPOIndentNo, string POIndentId, string Status, int Urgent, string SortBy, string ItemName, int SupplierId)
        {

            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intStoreId", iStoreId);
            HshIn.Add("intPOIndentId", POIndentId);
            HshIn.Add("chvPOIndentNo", sPOIndentNo);
            HshIn.Add("chrStatus", Status);
            HshIn.Add("bitUrgent", Urgent);
            HshIn.Add("chvSortBy", SortBy);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("intSupplierId", SupplierId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetPOIndentDetailsAll", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Author : Rafat Date : 13 July 2012        
        /// Used To get Indent Item PO Details after generating PO from Indent.        
        /// </summary>
        /// <param name="iPOIndentId"></param>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public DataSet getIndentItemPODetails(int iPOIndentId, int ItemId)
        {

            HshIn = new Hashtable();
            HshIn.Add("intPOIndentId", iPOIndentId);
            HshIn.Add("intItemId", ItemId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIndentItemPODetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getStockReOrderQuantity(int StoreId, int FacilityId, int ConsumptionDays, int MinDaysStock, int MaxDaysStock, string strItemSubCategory, string SortBy, string FractionAllow)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intConsumptionDays", ConsumptionDays);
                HshIn.Add("intMinDaysStock", MinDaysStock);
                HshIn.Add("intMaxDaysStock", MaxDaysStock);
                HshIn.Add("chvItemSubCategory", strItemSubCategory);
                HshIn.Add("@chvSortBy", SortBy);
                HshIn.Add("@FractionAllow", FractionAllow);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetStockReOrderQty", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getDeptStockReOrderQuantity(int StoreId, int FacilityId, int ConsumptionDays, int MinDaysStock, int MaxDaysStock, string strItemSubCategory, string SortBy, string FractionAllow)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn = new Hashtable();
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intConsumptionDays", ConsumptionDays);
                HshIn.Add("intMinDaysStock", MinDaysStock);
                HshIn.Add("intMaxDaysStock", MaxDaysStock);
                HshIn.Add("chvItemSubCategory", strItemSubCategory);
                HshIn.Add("@chvSortBy", SortBy);
                HshIn.Add("@FractionAllow", FractionAllow);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDepartmentROL", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getFSNDetails(int StorId, int FacilityId, string ReportType, int Fast, int Slow, int NonMoving, int Dormant, string procName)
        {

            HshIn = new Hashtable();
            HshIn.Add("intStoreId", StorId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("chrReportType", ReportType);
            HshIn.Add("intFast", Fast);
            HshIn.Add("intSlow", Slow);
            HshIn.Add("intNonMoving", NonMoving);
            HshIn.Add("intDormant", Dormant);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, procName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string DEPIndentClose(string IndentIds, int IndentClosedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@IndentClosedBy", IndentClosedBy);

                string qry = "SELECT IndentId FROM PhrDepIndentMain WITH (NOLOCK) WHERE Active = 1 AND IndentId IN (" + IndentIds + ") AND ProcessStatus = 'O'";


                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Close operation only allowed in Posted Indnet !";
                }

                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrDepIndentMain SET IndentClose = 1, IndentCloseDate = GETUTCDATE(), IndentClosedBy = @IndentClosedBy WHERE IndentId IN (" + IndentIds + ")", HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet getDepIndentItemPendingQty(int StoreId, int FacilityId, int ItemId, int LoginFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intDepartmentId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrDepIndentItemPendingQty", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool IsAllergyWithPrescribingMedicine(int EncounterId, int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intItemId", ItemId);

            string qry = "SELECT Id FROM PatientDrugAllergies WITH (NOLOCK) WHERE EncounterId = @intEncounterId AND ItemId = @intItemId AND Active = 1";

            try
            {

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

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

    }
}
