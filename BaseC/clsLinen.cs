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
    public partial class clsLinen
    {
        private string sConString = "";
        public clsLinen(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;

        public DataSet getItemListForDepLinen(int ItemId, string ItemSearch, int HospId, int EncodedBy, int Active, int ToFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvItemSearch", ItemSearch);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", ToFacilityId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetLinenItemMasterList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        ////////////// Receipt ///////////////////////

        public DataSet getLinenItem(int Receipt_Id, string Receipt_No, int HospitalLocationId, int FacilityId, int FacilityToId, int EncodedBy, string Receipt_Type)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intReceipt_Id", Receipt_Id);
                HshIn.Add("@chvReceipt_No", Receipt_No);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FacilityToId", FacilityToId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Receipt_Type", Receipt_Type);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspLinenItem", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public int getLinenid(string Receipt_No, string Receipt_Type)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Receipt_No", Receipt_No);
                HshIn.Add("@Receipt_Type", Receipt_Type);

                string strquery = "SELECT count(Receipt_Id) FROM LL_Receipt_Main WITH (NOLOCK) WHERE Receipt_No=@Receipt_No and Receipt_Type=@Receipt_Type";
                int coutnRec = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, strquery, HshIn));
                int Linenid = 0;
                if (coutnRec > 0)
                {
                    string strq = "SELECT Receipt_Id FROM LL_Receipt_Main WITH (NOLOCK) WHERE  Receipt_No=@Receipt_No and Receipt_Type=@Receipt_Type";
                    Linenid = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, strq, HshIn));
                }
                return Linenid;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable SaveLinenReceipt(int Receipt_Id, int LinenBy, int FacilityId, int FacilityToId, int FromDepartment, int ToDepartment, int HospitalLocationId, int EncodedBy, string Status, string XMLItem, string LRemarks, string Receipt_Type, int WasherManID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intReceipt_Id", Receipt_Id);
            HshIn.Add("@LinenBy", LinenBy);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@FacilityToId", FacilityToId);
            HshIn.Add("@FromDepartment", FromDepartment);
            HshIn.Add("@ToDepartment", ToDepartment);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@Status", Status);
            HshIn.Add("@xmlItemDetails", XMLItem);
            HshIn.Add("@LRemarks", LRemarks);
            HshIn.Add("@Receipt_Type", Receipt_Type);
            HshIn.Add("@WasherManID", WasherManID);

            HshOut.Add("@chvLinenReceiptNo", SqlDbType.VarChar);
            HshOut.Add("@intId", SqlDbType.VarChar);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspLinenSaveItemReceipt", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetStoreToChange(int HospId, int FacilityId, string ShortName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@ShortName", ShortName);

                string strquery = "select DepartmentID,DepartmentName from DepartmentMain WITH (NOLOCK) where ShortName=@ShortName and FacilityId=@FacilityId and HospitalLocationId=@HospId order by DepartmentID";

                return objDl.FillDataSet(CommandType.Text, strquery, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet SearchLinen(int Receipt_Id, string Receipt_No, int HospitalLocationId, int FacilityId, int FacilityToId, int EncodedBy, string Receipt_Type, DateTime FromDate, DateTime ToDate, string Status)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intReceipt_Id", Receipt_Id);
                HshIn.Add("@chvReceipt_No", Receipt_No);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FacilityToId", FacilityToId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Receipt_Type", Receipt_Type);
                HshIn.Add("@FromDate", fDate);
                HshIn.Add("@ToDate", tDate);
                HshIn.Add("@Status", Status);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspSearchLinen", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public Hashtable LinenReceiptPost(int ReceiptId, int EncodedBy, string Receipt_Type, int HospitalLocationId, string Trans_Type)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intReceiptId", ReceiptId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Receipt_Type", Receipt_Type);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chrTansType", Trans_Type);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspLinenPostedItemReceipt", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        ////////////// Issue ///////////////////////

        public DataSet getLinenItemIssue(int Issue_Id, string Issue_No, int HospitalLocationId, int FacilityId, int FacilityToId, int EncodedBy, string Issue_Type)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intIssue_Id", Issue_Id);
                HshIn.Add("@chvIssue_No", Issue_No);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FacilityToId", FacilityToId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Issue_Type", Issue_Type);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspLinenItemissue", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public int getLinenidIssue(string Issue_No, string Issue_Type)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Issue_No", Issue_No);
                HshIn.Add("@Issue_Type", Issue_Type);

                string strquery = "SELECT count(Issue_Id) FROM LL_Issue_Main WITH (NOLOCK) WHERE Issue_No=@Issue_No and Issue_Type=@Issue_Type";
                int coutnRec = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, strquery, HshIn));
                int Linenid = 0;
                if (coutnRec > 0)
                {
                    string strq = "SELECT Issue_Id FROM LL_Issue_Main WITH (NOLOCK) WHERE  Issue_No=@Issue_No and Issue_Type=@Issue_Type";
                    Linenid = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, strq, HshIn));
                }
                return Linenid;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable SaveLinenIssue(int Issue_Id, int LinenBy, int FacilityId, int FacilityToId, int FromDepartment, int ToDepartment, int HospitalLocationId, int EncodedBy, string Status, string XMLItem, string LRemarks, string Issue_Type, int WasherManID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intIssue_Id", Issue_Id);
            HshIn.Add("@LinenBy", LinenBy);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@FacilityToId", FacilityToId);
            HshIn.Add("@FromDepartment", FromDepartment);
            HshIn.Add("@ToDepartment", ToDepartment);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@Status", Status);
            HshIn.Add("@xmlItemDetails", XMLItem);
            HshIn.Add("@LRemarks", LRemarks);
            HshIn.Add("@Issue_Type", Issue_Type);
            HshIn.Add("@WasherManID", WasherManID);

            HshOut.Add("@chvLinenIssueNo", SqlDbType.VarChar);
            HshOut.Add("@intId", SqlDbType.VarChar);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspLinenSaveItemIssue", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet SearchLinenIssue(int Issue_Id, string Issue_No, int HospitalLocationId, int FacilityId, int FacilityToId, int EncodedBy, string Issue_Type, DateTime FromDate, DateTime ToDate, string Status)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intIssue_Id", Issue_Id);
                HshIn.Add("@chvIssue_No", Issue_No);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FacilityToId", FacilityToId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Issue_Type", Issue_Type);
                HshIn.Add("@FromDate", fDate);
                HshIn.Add("@ToDate", tDate);
                HshIn.Add("@Status", Status);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspSearchLinenIssue", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Hashtable LinenIssuePost(int IssueId, int EncodedBy, string Issue_Type, int HospitalLocationId, string TransType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@Issue_Type", Issue_Type);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chrTransType", TransType);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspLinenPostedItemIssue", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        // Get Item Batch 

        public DataSet GetItemBatch(int HospId, int ItemId, int StoreId, int EncodedBy, int FacilityId, int ItemWithStock, int CompanyId, int TransactionId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intItemId", ItemId);
                HshIn.Add("intStoreCode", StoreId);
                HshIn.Add("bitItemsWithStock", ItemWithStock);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("intCompanyId", CompanyId);
                HshIn.Add("intTransactionId", TransactionId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetLinenBatchDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetDepartment(int HospId, int FacilityId)
        {


            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDepartmentforLinen", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }





        }

        public string SaveWasherManMaster(int WasherManId, int HospitalLocationId, int FacilityId,
                              string WasherManName, string Address, int Active, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@WasherManId", WasherManId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@WasherManName", WasherManName);
            HshIn.Add("@Address", Address);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspLinenSaveWasherManMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getWasherManMaster(int WasherManId, int HospitalLocationId, int FacilityId, int Active)
        {
            HshIn = new Hashtable();

            HshIn.Add("@WasherManId", WasherManId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@bitActive", Active);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspLinenGetWasherManMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

    }



}
