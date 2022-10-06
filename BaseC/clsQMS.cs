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
    public class clsQMS
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable HshOut;
        public clsQMS(string conString)
        {
            sConString = conString;
        }
        public DataSet getQMSDoctorLists(int HospId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspQMSDoctorsList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getQMSDoctorListsDetails(int HospId, int FacilityId, int DoctorId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intProviderId", DoctorId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspQMSDoctorPatientsList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdateQMSDoctorRoomNo(int HospId, int FacilityId, int DoctorId, string sRoomNo)
        {
            string sResult = "";
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intProviderId", DoctorId);
            HshIn.Add("@chvRoomNo", sRoomNo);
            string sQuery = "UPDATE Employee SET RoomNo=@chvRoomNo WHERE id=@intProviderId AND HospitalLocationId=@inyHospitalLocationId AND Active=1";
            try
            {
                objDl.ExecuteNonQuery(CommandType.Text, sQuery, HshIn);

                sResult = "Room No Update!";

                return sResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorDepartmentName(int HospitalLocationId, int DoctorId)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intProviderId", DoctorId);
                string sQuery = "SELECT ISNULL(tm.Name,'') + ISNULL(' ' + em.FirstName,'') + ISNULL(' ' +em.MiddleName,'') + ISNULL(' ' + em.LastName,'') AS DoctorName,dm.DepartmentName FROM Employee em WITH (NOLOCK) INNER JOIN DepartmentMain dm WITH (NOLOCK) ON em.DepartmentId=dm.DepartmentID LEFT JOIN TitleMaster tm WITH (NOLOCK) ON em.TitleId=tm.TitleID WHERE em.Id=@intProviderId AND em.Active=1 AND em.HospitalLocationId=@inyHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int CloseQMSCall(int TokenNo, int UserID, string ActivityDesc)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.ExecuteNonQuery(CommandType.Text, "exec UspCloseQMSCall @TokenNo='" + TokenNo + "',@UserID=" + UserID + ",@ActivityDesc=" + ActivityDesc);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
