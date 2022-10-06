using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class CompanyServiceSetup
    {
        private string sConString = "";

        DAL.DAL Dl;
        public CompanyServiceSetup(string Constring)
        {
            sConString = Constring;
            Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public int HospitalLocationId = 0;
        public int UserID = 0;
        public int CompanyID = 0;
        public int DepartmentID = 0;
        public int SubDepartmentID = 0;
        public int ShowCompanyServiceSetupOnly = 0;
        public string xmlService = "";
        public string SProc = "";
        public DataSet getData(CompanyServiceSetup cssetup)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationId", cssetup.HospitalLocationId);
                HshIn.Add("intCompanyId", cssetup.CompanyID);
                HshIn.Add("intDepartmentId", cssetup.DepartmentID);
                HshIn.Add("intSubDepartmentId", cssetup.SubDepartmentID);
                HshIn.Add("bitShowCompanyServiceSetup", cssetup.ShowCompanyServiceSetupOnly);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetCompanyServiceNames", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveData(CompanyServiceSetup cssetup)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationId", cssetup.HospitalLocationId);
            HshIn.Add("intCompanyId", cssetup.CompanyID);
            HshIn.Add("intEncodedBy", cssetup.UserID);
            HshIn.Add("xmlService", cssetup.xmlService);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, cssetup.SProc, HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getGetItemofserviceChargeType(int HospitalLocationId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("IntHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetitemOfServiceChargeType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveChargeType(int IntHospitalLocationId, string Chargetype, int ServiceId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("IntHospitalLocationId", IntHospitalLocationId);
            if (Chargetype != "0")
            {
                HshIn.Add("chrChargeType", Chargetype);
            }
            HshIn.Add("intServiceId", ServiceId);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateServiceChargeType", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


    }
}
