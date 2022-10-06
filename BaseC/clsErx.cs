using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BaseC
{

    public class clsErx
    {
        private string sConString = "";
        public clsErx(string conString)
        {
            sConString = conString;
        }
        public void SaveDhaError(int IndentId, string Error, string FormName, string MethodName, string DHAMethodName, int facilityid, int ModuleId, string ErrorMessage)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@IndentId", IndentId);
            HshIn.Add("@Error", Error);
            HshIn.Add("@FormName", FormName);
            HshIn.Add("@MethodName", MethodName);
            HshIn.Add("@DHAMethodName", DHAMethodName);
            HshIn.Add("@facilityid", facilityid);
            HshIn.Add("@ModuleId", ModuleId);
            HshIn.Add("@ErrorMessage", ErrorMessage);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDhaError", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet FillDHAError(string fromdate, string todate, int facilityid, int ModuleId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("fDate", fromdate);
            HshIn.Add("tDate", todate);
            HshIn.Add("facilityid", facilityid);
            HshIn.Add("ModuleId", ModuleId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspFillDHAError", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
    }
}
