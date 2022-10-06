using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
namespace BaseC
{
    public class Surgery
    {
        string sConString = "";
        Hashtable HshIn;
        Hashtable houtPara;
        StringBuilder sb;
        DAL.DAL objDl;

        public Surgery(string Constring)
        {
            sConString = Constring;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }

        public DataSet GetDoctorClassification(int iHospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);

            sb = new StringBuilder();
            sb.Append("SELECT ID, Name, Type, SequenceNo, Percentage, MaxLimit, DoctorRequired, Name Name1 FROM SurgeryDoctorClassification WITH(NOLOCK) WHERE Type NOT IN('SR', 'AN') AND Active = 1 AND HospitalLocationID = @inyHospitalLocationId ORDER BY SequenceNo");
            try
            {

                return objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorName(int iEmpID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intEmployeeId", iEmpID);
                sb = new StringBuilder();
                sb.Append("select  dbo.GetDoctorName(@intEmployeeId) as DoctorName");
                return objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetBillType()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                sb = new StringBuilder();
                sb.Append("SELECT ID,Description FROM BillType WITH (NOLOCK) Where Active = 1");
                return objDl.FillDataSet(CommandType.Text, sb.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetOTRoom(int iHospitalLocationId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);

                sb = new StringBuilder();

                sb.Append("SELECT * FROM dbo.OTTheatreMaster WITH (NOLOCK) WHERE FacilityId=@intFacilityId AND HospitalLocationID = @inyHospitalLocationId and Active=1");
                return objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetSavedSurgeryService(int iHospitalLocationId, int iFacilityID, int iRegID, int iEnCounterID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegID", iRegID);
                HshIn.Add("@intEnounterId", iEnCounterID);
                HshIn.Add("@intFacilityId", iFacilityID);
                sb = new StringBuilder();
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetSurgeryOrderDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getSurgeryComponent(int HospitalLocationId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspsurgerycomponent", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetResourcecharges(int iHospitalLocationId, int Facilityint, int CompanyId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityint", Facilityint);
                HshIn.Add("@intCompanyId", CompanyId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetResourcecharges", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

    }
}
