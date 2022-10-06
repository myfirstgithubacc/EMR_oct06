using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class EMRAllergy
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable HshOut;
        DAL.DAL objDl;


        public EMRAllergy(string Constring)
        {
            sConString = Constring;
        }

        public SqlDataReader populateAllergyType(int iHospitalLocation)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospitalLocationId", iHospitalLocation);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select TypeID,TypeName from AllergyType WITH (NOLOCK) where (HospitalLocationId=@HospitalLocationId OR HospitalLocationId IS NULL)AND Active=1", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public SqlDataReader populateFoodAllergyDropDown(int iHospitalLocationID, int iTypeID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("HospID", iHospitalLocationID);
            HshIn.Add("TypeID", iTypeID);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select AllergyID,Description from AllergyMaster WITH (NOLOCK) where TypeID=@TypeID and Active=1 and HospitalLocationID=@HospID ", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public String SaveUpdateAllergy(int iHospID, Int64 iRegNo, int iEncID, int iAllergyID, String sReaction, Boolean bitIntTolerance, String sAllergyDate, int iEncodedBy, Boolean bitNKDA, String sNewAllergyName, int TypeID, String sRemarks, int iID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("intRegistrationNo", iRegNo);
            HshIn.Add("intEncounterId", iEncID);
            HshIn.Add("intAllergyID", iAllergyID);
            HshIn.Add("chvReaction", sReaction);
            HshIn.Add("bitIntolerance", bitIntTolerance);
            HshIn.Add("chvAllergyDate", sAllergyDate);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("bitNKDA", bitNKDA);
            HshIn.Add("chvNewAllergyName", sNewAllergyName);
            HshIn.Add("inyTypeID", TypeID);
            HshIn.Add("chvRemarks", sRemarks);
            HshIn.Add("intID", iID);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveAllergy", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAllergy(int iHospID, Int64 iRegNo, int iEncID, int TypeID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("intRegistrationNo", iRegNo);
            HshIn.Add("intEncounterId", iEncID);
            HshIn.Add("inyTypeID", TypeID);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetAllergy", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int DeActivateAllergy(int iID, int iHospID, String SRemarks, int iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", iID);
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@CancellationRemarks", SRemarks);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRAllergy SET Active = '0',CancellationRemarks=@CancellationRemarks,LastChangedBy=@EncodedBy,LastChangedDate=getDate() WHERE ID=@ID and HospitalLocationID = @HospID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int UpdateAllergy(int iID, int iHospID, String sReaction, Boolean bitIntolerance, int iUserID, String sAllergyDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", iID);
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@Reaction", sReaction);
                HshIn.Add("@Intol", bitIntolerance);
                HshIn.Add("@AllergyDate", sAllergyDate);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE EMRAllergy SET Reaction=@Reaction,Intolerance=@Intol,AllergyDate=@AllergyDate,LastChangedBy=@EncodedBy,LastChangedDate=getDate() WHERE ID=@ID and HospitalLocationID = @HospID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetCancelDetail(Int16 iHospID, Int32 iRegNo, Int32 iEncID, Int32 iTableID)
        //{
        //    HshIn = new Hashtable();
        //    Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn.Add("inyHospitalLocationID", iHospID);
        //    HshIn.Add("intRegistrationNo", iRegNo);
        //    HshIn.Add("intEncounterId", iEncID);
        //    HshIn.Add("intTableID", iTableID);
        //    DataSet ds = Dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetAllergyCancelDetails", HshIn);
        //    return ds;
        //}

        public DataSet GetAllergyMaster(int iHospID, int iTypeID)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("inyTypeID", iTypeID);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetAllergyMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveAllergyMaster(int iHospID, int iTypeID, String sDescription, int iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("intTypeID", iTypeID);
            HshIn.Add("chvDescription", sDescription);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveAllergyMaster", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public String updateshownote(string shownotevalue, int pageid, int encntrid, int regstrtnid)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("chvshownotevalue", shownotevalue);
            HshIn.Add("intPageId", pageid);
            HshIn.Add("intencouterid", encntrid);
            HshIn.Add("intregid", regstrtnid);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEmrAllergyUpdateShowNote", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public String SaveAllergyTypeMaster(int iHospID, String sDescription, int iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("chvDescription", sDescription);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveAllergyType", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public int UpdateAllergyMaster(int iHospID, int iAllergyID, int iTypeID, String sDesc, int iUserID, Byte bActive)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@AllergyID", iAllergyID);
                HshIn.Add("@Desc", sDesc);
                HshIn.Add("@TypeID", iTypeID);
                HshIn.Add("@Active", bActive);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE AllergyMaster SET Description=@Desc,Active=@Active,LastChangedBy=@EncodedBy,LastChangedDate=getDate() WHERE AllergyID=@AllergyID and HospitalLocationID = @HospID and TypeID=@TypeID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int UpdateAllergyTypeMaster(int iHospID, int iTypeID, String sDesc, int iUserID, Byte bActive)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@Desc", sDesc);
                HshIn.Add("@TypeID", iTypeID);
                HshIn.Add("@Active", bActive);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE AllergyType SET TypeName=@Desc,Active=@Active WHERE TypeID=@TypeID and HospitalLocationID = @HospID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int DeActivateAllergyMaster(int iHospID, Int32 iAllergyID, Int32 iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@AllergyID", iAllergyID);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE AllergyMaster SET Active = '0',LastChangedBy=@EncodedBy,LastChangedDate=getDate() WHERE AllergyID=@AllergyID and HospitalLocationID = @HospID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int DeActivateAllergyTypeMaster(int iHospID, int iTypeID, int iUserID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@TypeID", iTypeID);
                HshIn.Add("@EncodedBy", iUserID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE AllergyType SET Active = '0' WHERE TypeID=@TypeID and HospitalLocationID = @HospID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAllergyItemList(int HospId, int FacilityId, string ItemName, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvItemName", ItemName);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAllergyItemList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string updateNoAllergy(int IsNoAllergy, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@bitNKDA", IsNoAllergy);
                HshIn.Add("@intRegistrationId", RegistrationId);

                int i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE Registration SET NoAllergies = @bitNKDA WHERE ID = @intRegistrationId ", HshIn);

                return "Update no allergies succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
