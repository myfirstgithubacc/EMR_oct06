using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class clChecklist
    {
        private string sConString = string.Empty;
        public clChecklist(string conString)
        {
            sConString = conString;

        }
        Hashtable HshIn;
        Hashtable HshOut;


        public DataSet getCheckList(int id, string ModuleType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("id", id);
                HshIn.Add("ModuleType", ModuleType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspInvoiceCheckListMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveCheckList(int id, string Description, string ModuleType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("id", id);
            HshIn.Add("Description", Description);
            HshIn.Add("ModuleType", ModuleType);
            HshOut.Add("@Result", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveInvoiceCheckListMaster", HshIn, HshOut);
                return HshOut["@Result"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string SaveChecklistCompanywise(int CompanyId, string CheckListId, int UserID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@xmlItems", CheckListId);
            HshIn.Add("@intEncodedBy", UserID);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveChecklistCompanywise", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetChecklistCompanywise(int HospId, int CompanyId, string ModuleType)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@chvModuleType", ModuleType);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetChecklistCompanywise", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int UpdateChecklistType(int Id, int Status, int UserId, DateTime Lcdate)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intId", Id);
                HshIn.Add("@intStatusId", Status);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@dtLastChangeDate", Lcdate);

                string strsql = "UPDATE CompanyInvoiceCheckListMaster SET Active=@intStatusId,LastChangedBy =@intUserId,LastChangedDate=@dtLastChangeDate WHERE Id=@intId";

                return objDl.ExecuteNonQuery(CommandType.Text, strsql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }

}
