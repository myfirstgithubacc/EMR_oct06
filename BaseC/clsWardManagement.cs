using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BaseC
{
    public class clsWardManagement
    {
        string sConString = "";
        public clsWardManagement(string Constring)
        {
            sConString = Constring;
        }
        public DataSet GetReferralDetailCount(int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intUserId", UserId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferralCount", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
