using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Data;

/// <summary>
/// Developed By Rajeev
/// </summary>

namespace BaseC
{
    public class Dynamic
    {
        Hashtable HshIn;
        Hashtable HshOut;
        private string sConString = "";
        public Dynamic(string conString)
        {
            sConString = conString;
        }
        public DataSet GetTableWiseData(int iHospID, int iFacilityId, string TableName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospId", iHospID);
                HshIn.Add("@intFacilityId", iFacilityId);
                string strqry = "SELECT ICCAPatientDocumentPath FROM " + TableName + " where  FacilityID = @intFacilityId and HospitalLocationID=@intHospId and Active=1 ";
                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
