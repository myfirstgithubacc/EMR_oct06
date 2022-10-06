using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BaseC
{
    public class ClsExternal
    {
        private string sConString = "";
        DAL.DAL Dl;
        public ClsExternal(string Constring)
        {
            sConString = Constring;
            Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }

        public DataSet getUserGroupNotification(int input)
        {         
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string _Text = "Select * from Employee";
            DataSet ds = objDl.FillDataSet(CommandType.Text, _Text);
            return ds;
        }
        public DataSet getUserGroupNotification(int GroupId, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("GroupId", GroupId);
            HshIn.Add("FacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetGroupNotification", HshIn);
            return ds;
        }
        public DataSet GetPatientUnbilledOrders(int EncounterId, int RegistrationId, int FacilityId, int HospitalLocationId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("EncounterId", EncounterId);
            HshIn.Add("RegistrationId", RegistrationId);
            HshIn.Add("FacilityId", FacilityId);
            HshIn.Add("HospitalLocationId", HospitalLocationId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientUnbilledOrders", HshIn);
            return ds;
        }

        public DataSet CheckInvoiceCurrentStatus(string InvoiceNo, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("InvoiceNo", InvoiceNo);
            HshIn.Add("FacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckInvoiceCurrentStatus", HshIn);
            return ds;
        }
    }
}
