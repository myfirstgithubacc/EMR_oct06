using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BaseC
{
    public class WardManagement
    {
        private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        DAL.DAL objDl;
        DataSet ds;
        Hashtable hstInput;
        Hashtable houtPut;



        /// <summary>
        /// Show Ward Patient wise
        /// Create By Santosh chaurasia
        /// </summary>
        /// <param name="IHospitalId"></param>
        /// <param name="RegistrationId"></param>
        /// <param name="WardId"></param>
        /// <returns></returns>
        public DataSet GetWardwisepatient(int IHospitalId, int RegistrationId, int WardId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();

            hstInput.Add("inyHospitalLocationId", IHospitalId);
            hstInput.Add("intRegistrationId", RegistrationId);
            hstInput.Add("intWardId", WardId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetWardwisePatient", hstInput);
            return ds;


        }
        /// <summary>
        /// For Get Drug List.
        /// Create By - Santosh
        /// </summary>
        /// <param name="iTypeid"></param>
        /// <param name="iDrugid"></param>
        /// <param name="iDrugname"></param>
        /// <returns></returns>
        public DataSet GetDrugList(int iTypeid, string iDrugid, string iDrugname)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("intSynonym_Type_Id", iTypeid);
            hstInput.Add("chvDrugId", iDrugid);
            hstInput.Add("chvSearchCriteria", iDrugname);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDrugList", hstInput);


            return ds;
        }
        /// <summary>
        /// Save Diet Category Type masster
        /// Create By santosh
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <param name="Dietname"></param>
        /// <param name="DietType"></param>
        /// <param name="Status"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>
        public string SaveDietCategoryType(int HospId, int DietId, string Dietname, string DietType, int Status, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intHospitalLocationId", HospId);
            HshIn.Add("intDietId", DietId);
            HshIn.Add("chrDitetName", Dietname);
            HshIn.Add("chrDietType", DietType);
            HshIn.Add("bitStatus", Status);
            HshIn.Add("intEncodeBy", EncodedBy);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietCategoryType", HshIn, HshOut);

            return HshOut["@chrErrorStatus"].ToString();
        }
        /// <summary>
        /// Get Diet Category Type Master.
        /// Create By Santosh
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetDietCategoryType(int HospId, int DietId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", HospId);
            HshIn.Add("intDietId", DietId);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietCategoryType", HshIn);


            return ds;
        }

        /// <summary>
        /// Save Diet Type
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietDetailId"></param>
        /// <param name="DietId"></param>
        /// <param name="Dietname"></param>
        /// <param name="Status"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>

        public string SaveDietType(int HospId, int DietDetailId, int DietId, string Dietname, int Status, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intHospitalLocationId", HospId);
            HshIn.Add("intDietDetailId", DietDetailId);
            HshIn.Add("intDietId", DietId);
            HshIn.Add("chrDietSubcategory", Dietname);
            HshIn.Add("bitStatus", Status);
            HshIn.Add("intEncodeBy", EncodedBy);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietTypeDetail", HshIn, HshOut);

            return HshOut["@chrErrorStatus"].ToString();
        }
        /// <summary>
        /// Get Diet Type
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetDietType(int HospId, int DietId, int DietCatId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", HospId);
            HshIn.Add("intDietDetailId", DietId);
            HshIn.Add("intDietCategoryId", DietCatId);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietTypeDetail", HshIn);


            return ds;
        }

        public DataSet GetEmployeeType()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.Text, "Select Id,Description from EmployeeType WITH (NOLOCK) where Active=1 ");
            return ds;
        }
        public DataSet GetEmployee(int HospId, int EmpTypeId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intEmployeetypeId", EmpTypeId);
            HshIn.Add("@intFacilityId", FacilityId);

            ds = objDl.FillDataSet(CommandType.Text, "Select e.ID,dbo.GetDoctorName(e.ID) EmployeeName From Employee e WITH (NOLOCK) inner join users u WITH (NOLOCK) on e.id = u.empid inner Join SecUserFacility suf WITH (NOLOCK) On suf.UserId = u.Id And suf.FacilityId =@intFacilityId And suf.Active = 1 where EmployeeType=@intEmployeetypeId and e.Active=1 and e.HospitalLocationId=@intHospitalLocationId order by EmployeeName", HshIn);
            return ds;
        }

        public string SaveWardlagTagging(string xmlEmployeeId, string xmlItemFlagIds, int HospId, int FacilityId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@xmlEmployeeId", xmlEmployeeId);
            HshIn.Add("@xmlwardFlagIds", xmlItemFlagIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chrErrormessage", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveWardTagging", HshIn, HshOut);

            return HshOut["@chrErrormessage"].ToString();
        }
        public string SaveOTTagging(string xmlEmployeeId, string xmlItemFlagIds, int FacilityId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlEmployeeId", xmlEmployeeId);
            HshIn.Add("@xmlOTFlagIds", xmlItemFlagIds);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chrErrormessage", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOTTagging", HshIn, HshOut);

            return HshOut["@chrErrormessage"].ToString();
        }
        public DataSet GetWardTagging(int iHospId, bool OnlyTagged, int iEmpId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intEmpId", iEmpId);
                HshIn.Add("@inyHospId", iHospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@OnlyTagged", OnlyTagged);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetWardTagging", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }
        }

        public DataSet GetWardComputerTagging(int iHospId, bool OnlyTagged, string ComputerName, int FacilityId)
        {
            try
            {
                ds = new DataSet();
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@chvComputerName", ComputerName);
                HshIn.Add("@inyHospId", iHospId);
                HshIn.Add("@intFacilityId", FacilityId);

                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                string qry = "select wt.ComputerId  as EmployeeId, wm.WardId, wm.WardName, CASE WHEN ISNULL(wt.ComputerId, 0) = 0 THEN 0 ELSE 1 END IsChk, wt.Id as TagId " +
                   " from WardMaster wm WITH(NOLOCK) " +
                 " LEFT OUTER JOIN WardComputerTagging wt WITH (NOLOCK)ON wm.WardId = wt.WardId And wt.FacilityId = @intFacilityId AND wt.Active = 1 " +
                 " AND wt.ComputerId = (select  top 1  id from ComputerName where ComputerName = rtrim(ltrim(@chvComputerName)) and active = 1) " +
                 " WHERE wm.FacilityId = @intFacilityId And wm.Active = 1 and wm.HospitalLocationId = @inyHospId";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(wt.ComputerId, 0) <> 0 ";
                }

                qry += " ORDER BY wm.WardName ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;


        }
        public DataSet GetOtTagging(bool OnlyTagged, int iEmpId, int FacilityId)
        {
            try
            {
                ds = new DataSet();
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@intEmpId", iEmpId);
                HshIn.Add("@intFacilityId", FacilityId);
                objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                string query = "select OT.TheatreId as 'WardId', OT.TheatreName as 'WardName', ET.Id as TagId from EmployeeWiseOTTagging ET inner join OTTheatreMaster OT  on ET.TheatreId = ot.TheatreId  Where ET.EmployeeId = @intEmpId ANd ET.Active=1";
                if (OnlyTagged)
                {
                    query += " AND ISNULL(ET.EmployeeId, 0) <> 0 ";


                }
                query += "Order By OT.TheatreName";
                ds = objDl.FillDataSet(CommandType.Text, query, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }
        public DataSet GetAdmissionDetails(int iHospId, int iEmpId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospId);
            HshIn.Add("intEmployeeId", iEmpId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetAdmissionDetails", HshIn);
            return ds;
        }
        public DataSet GetWardDoctorwise(int iHospId, int iEmpId, int iRegId, int WardId, int DoctorId, int facilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("anyHospitalLocationId", iHospId);
            HshIn.Add("intUserId", iEmpId);
            HshIn.Add("intRegistrationId", iRegId);
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intFacilityId", facilityId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetWardDoctorwise", HshIn);
            return ds;
        }
        /// <summary>
        ///  Save Liquid Diet Master 
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <param name="Dietname"></param>
        /// <param name="DietType"></param>
        /// <param name="Status"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>
        public string SaveLiquidDiet(int LiquidId, int ItemId, string ShortName, string LiqidName, int Status, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intLiquidId", LiquidId);
            HshIn.Add("intItemId", ItemId);
            HshIn.Add("chrShortName", ShortName);
            HshIn.Add("chrDitetName", LiqidName);
            HshIn.Add("bitStatus", Status);
            HshIn.Add("intEncodeBy", EncodedBy);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveLiquidDietMaster", HshIn, HshOut);

            return HshOut["@chrErrorStatus"].ToString();
        }
        /// <summary>
        /// Get Liquid Diet  Master.
        /// Create By Santosh
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetLiquidDiet(int DietId, int ItemId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intLiquidId", DietId);
            HshIn.Add("intItemId", ItemId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetLiquidDietMaster", HshIn);


            return ds;
        }
        /// <summary>
        /// Save Diet Slots
        /// </summary>
        /// <param name="DietSlotId"></param>
        /// <param name="SlotName"></param>
        /// <param name="Status"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>

        public string SaveDietSlots(int DietSlotId, string SlotName, int Status, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intDietSlotId", DietSlotId);
            HshIn.Add("chrDietSlotName", SlotName);
            HshIn.Add("bitStatus", Status);
            HshIn.Add("intEncodeBy", EncodedBy);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietSlotMaster", HshIn, HshOut);

            return HshOut["@chrErrorStatus"].ToString();
        }
        /// <summary>
        /// Get Diet Slots.
        /// Create By Santosh
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetDietSlots(int DietSlotId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intDietSlotId", DietSlotId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietSlot", HshIn);


            return ds;
        }

        /// <summary>
        /// Save Diet Master
        /// </summary>
        /// <param name="DietSlotId"></param>
        /// <param name="SlotName"></param>
        /// <param name="Status"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>




        public string SaveDietMaster(int DietId, int DietSlotId, string DietName, int DietTypeCategoryId,
                    int IsDefaultMeal, int Status, int EncodedBy, int calories, int protien)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intDietID", DietId);
            HshIn.Add("intSlotId", DietSlotId);
            HshIn.Add("chrDietName", DietName);
            HshIn.Add("intDietTypeCategoryId", DietTypeCategoryId);
            HshIn.Add("bitIsDefault", IsDefaultMeal);
            HshIn.Add("bitStatus", Status);
            HshIn.Add("intEncodeBy", EncodedBy);
            HshIn.Add("calories", calories);
            HshIn.Add("protien", protien);


            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietMaster", HshIn, HshOut);

            return HshOut["@chrErrorStatus"].ToString();
        }
        /// <summary>
        /// Get Diet Master.
        /// Create By Santosh
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetDietMaster(int DietId, int SlotId, int iDietTypeCategoryId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intDietId", DietId);
            HshIn.Add("intSlotId", SlotId);
            HshIn.Add("iDietTypeCategoryId", iDietTypeCategoryId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietMaster", HshIn);


            return ds;
        }


        /// <summary>
        /// Save Precaution
        /// </summary>
        /// <param name="DietSlotId"></param>
        /// <param name="SlotName"></param>
        /// <param name="Status"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>

        public string SavePrecation(int Id, int Type, string Name, int Status, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intId", Id);
            HshIn.Add("intType", Type);
            HshIn.Add("chrName", Name);
            HshIn.Add("bitStatus", Status);
            HshIn.Add("intEncodeBy", EncodedBy);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSavePrecautionMaster", HshIn, HshOut);

            return HshOut["@chrErrorStatus"].ToString();
        }
        /// <summary>
        /// Get Precaution details.
        /// Create By Santosh
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetPrecaution(int Type, int Id)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();

            HshIn.Add("intType", Type);
            HshIn.Add("intId", Id);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPrecaution", HshIn);
            return ds;
        }
        public DataSet GetDoctor(int RegId, int EncId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intRegId", RegId);
            HshIn.Add("@intEncId", EncId);

            ds = objDl.FillDataSet(CommandType.Text, "select AdmittingDoctorId from Admission WITH (NOLOCK) where EncounterId=@intEncId and RegistrationId=@intRegId ", HshIn);
            return ds;
        }

        //public string ChangeEncouterStatus(int HospiId, int FaciId, int EncId, int RegId, int StatusId,int userId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet ds = new DataSet();
        //    string str = "";
        //    if (StatusId == 183)
        //    {
        //        objDl.ExecuteNonQuery(CommandType.Text, "UPDATE Encounter SET StatusId=" + StatusId + " WHERE  Id =" + EncId + " AND RegistrationId=" + RegId + " AND HospitalLocationId=" + HospiId + " AND FacilityID=" + FaciId + "");
        //        objDl.ExecuteNonQuery(CommandType.Text,"Insert into EncounterStatus (HospitalLocationID,FacilityID,EncounterID,StatusID,Active,Encodedby,EncodedDate) VALUES("+HospiId+","+FaciId+","+EncId+","+StatusId+","+'1'+","+userId+",'"+DateTime.Now.Date+"')");

        //        str = "Change.";
        //    }
        //    else
        //    {
        //        ds = objDl.FillDataSet(CommandType.Text, "SELECT StatusId FROM Encounter WHERE  Id =" + EncId + " AND RegistrationId=" + RegId + " AND HospitalLocationId=" + HospiId + " AND FacilityID=" + FaciId + "");
        //        if (ds.Tables[0].Rows[0]["StatusId"].ToString().Trim() == "183")
        //        {
        //            objDl.ExecuteNonQuery(CommandType.Text, "UPDATE Encounter SET StatusId=" + StatusId + "  WHERE  Id =" + EncId + " AND RegistrationId=" + RegId + " AND HospitalLocationId=" + HospiId + " AND FacilityID=" + FaciId + "");
        //            objDl.ExecuteNonQuery(CommandType.Text, "Insert into EncounterStatus (HospitalLocationID,FacilityID,EncounterID,StatusID,Active,Encodedby,EncodedDate) VALUES(" + HospiId + "," + FaciId + "," + EncId + "," + StatusId + "," + '1' + "," + userId + ",'" + DateTime.Now.Date + "')");

        //            str = "Change.";
        //        }
        //        else
        //        {
        //            str = "Patient not Marked For Discharged";
        //        }
        //    }
        //    return str;
        //}

        public string ChangeEncouterStatus(int HospId, int FacilityId, int EncounterId, int RegistrationId, int StatusId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intHospId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intUserId", UserId);
            HshIn.Add("@dtCurrentDate", DateTime.Now.Date);

            string str = string.Empty;
            if (StatusId == 183)
            {
                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE Encounter SET StatusId=@intStatusId WHERE Id = @intEncounterId AND RegistrationId=@intRegistrationId AND FacilityID=@intFacilityId AND HospitalLocationId=@intHospId ", HshIn);
                objDl.ExecuteNonQuery(CommandType.Text, "Insert into EncounterStatus (HospitalLocationID,FacilityID,EncounterID,StatusID,Active,Encodedby,EncodedDate) VALUES(@intHospId,@intFacilityId,@intEncounterId,@intStatusId,1,@intUserId,@dtCurrentDate) ", HshIn);

                str = "Change.";
            }
            else
            {
                ds = objDl.FillDataSet(CommandType.Text, "SELECT StatusId FROM Encounter WITH (NOLOCK) WHERE Id = @intEncounterId AND RegistrationId=@intRegistrationId AND FacilityID=@intFacilityId AND HospitalLocationId=@intHospId ", HshIn);
                if (ds.Tables[0].Rows[0]["StatusId"].ToString().Trim() == "183")
                {
                    objDl.ExecuteNonQuery(CommandType.Text, "UPDATE Encounter SET StatusId=@intStatusId WHERE Id = @intEncounterId AND RegistrationId=@intRegistrationId AND FacilityID=@intFacilityId AND HospitalLocationId=@intHospId ", HshIn);
                    objDl.ExecuteNonQuery(CommandType.Text, "Insert into EncounterStatus (HospitalLocationID,FacilityID,EncounterID,StatusID,Active,Encodedby,EncodedDate) VALUES(@intHospId,@intFacilityId,@intEncounterId,@intStatusId,1,@intUserId,@dtCurrentDate) ", HshIn);

                    str = "Change.";
                }
                else
                {
                    str = "Patient not Marked For Discharged";
                }
            }
            return str;
        }

        public string UpdateEncouterStatus(int EncounterId, int HospId, int FacilityId, int RegistrationId, int StatusId, int UserId,
                                            DateTime? EDod, int DischargeStatus, string CommonRemarks, int ReasonId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            houtPut = new Hashtable();

            hstInput.Add("@intEncounterId", EncounterId);
            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@intRegistrationId", RegistrationId);
            hstInput.Add("@intStatusId", StatusId);
            hstInput.Add("@intEncodedBy", UserId);
            if (EDod != null)
            {
                hstInput.Add("@ExpectedDateOfDischarge", EDod);
                hstInput.Add("@intDischarStatus", DischargeStatus);
            }

            if (CommonRemarks != string.Empty)
            {
                hstInput.Add("@chvCommonRemarks", CommonRemarks);
            }
            if (ReasonId > 0)
            {
                hstInput.Add("@intReasonId", ReasonId);
            }


            houtPut.Add("@chvErrorStatus", SqlDbType.VarChar);

            houtPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEncouterStatus", hstInput, houtPut);

            return houtPut["@chvErrorStatus"].ToString();
        }
        public string UpdateEncouterStatusForDischargeSummaryAcknowledge(int EncounterId, int HospId, int FacilityId, int RegistrationId, int StatusId, int UserId, DateTime? EDod, int DischargeStatus)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            houtPut = new Hashtable();

            hstInput.Add("@intEncounterId", EncounterId);
            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@intRegistrationId", RegistrationId);
            hstInput.Add("@intStatusId", StatusId);
            hstInput.Add("@intEncodedBy", UserId);
            if (EDod != null)
            {
                hstInput.Add("@ExpectedDateOfDischarge", EDod);
                hstInput.Add("@intDischarStatus", DischargeStatus);
            }


            houtPut.Add("@chvErrorStatus", SqlDbType.VarChar);

            houtPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEncouterStatus_forDischargeSummary", hstInput, houtPut);

            return houtPut["@chvErrorStatus"].ToString();
        }
        public void UpdateEncouterProbableDischargeDate(int EncounterId, int FacilityId, int UserId, DateTime? EDod)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            houtPut = new Hashtable();
            DataSet ds = new DataSet();
            hstInput.Add("@intEncounterId", EncounterId);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@ExpectedDateOfDischarge", EDod);
            //houtPut.Add("@chvErrorStatus", SqlDbType.VarChar);
            objDl.FillDataSet(CommandType.StoredProcedure, "uspWardUpdateProbableDOD", hstInput);
        }
        public DataSet getBedMaster(int BedId, string BedNo, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();

            hstInput.Add("@intBedId", BedId);
            hstInput.Add("@chvBedNo", BedNo);
            hstInput.Add("@intHospId", HospId);

            string qry = "SELECT Id AS BedId, BedNo, BedStatus, BedCategoryId, WardId FROM BedMaster WITH (NOLOCK) " +
                        " WHERE Active = 1 ";

            if (BedId > 0)
            {
                qry += " AND Id = @intBedId ";
            }

            if (BedNo.Trim() != string.Empty)
            {
                qry += " AND BedNo = @chvBedNo ";
            }

            qry += " AND HospitalLocationId = @intHospId ";

            ds = objDl.FillDataSet(CommandType.Text, qry, hstInput);
            return ds;
        }

        public string updateBedStatus(int BedId, string NextBedStatus, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            houtPut = new Hashtable();

            hstInput.Add("@intBedId", BedId);
            hstInput.Add("@chvNextBedStatus", NextBedStatus);
            hstInput.Add("@intEncodedBy", EncodedBy);

            string qry = "UPDATE BedMaster SET BedStatus = @chvNextBedStatus, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() " +
                         " WHERE Id = @intBedId AND @chvNextBedStatus IN ('H', 'V')";

            objDl.ExecuteNonQuery(CommandType.Text, qry, hstInput);

            return "Bed status changed succeeded !";
        }

        public DataSet GetEncounterStatus()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.Text, "select StatusId, Status, StatusColor, Code from Statusmaster WITH (NOLOCK) where Statustype='Encounter' and Code in('MD','SB')");
            return ds;
        }

        public DataSet getNextWardEncounterStatus(int statusId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            DataSet ds = new DataSet();
            hstInput.Add("@intStatusid", statusId);
            //
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetIPPreviousNExtEncStatus", hstInput);

            return ds;
        }
        public DataSet getNextWardEncounterStatusForDischargeSummary(int statusId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            DataSet ds = new DataSet();
            hstInput.Add("@intStatusid", statusId);
            //
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetIPPreviousNextEncStatus_forDischargeAcknowledge", hstInput);

            return ds;
        }


        public DataTable getNextWardEncounterStatus(int EncounterId, int FacilityId, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            DataSet ds = new DataSet();
            string strqry = string.Empty;

            hstInput.Add("@EncounterId", EncounterId);
            hstInput.Add("@FacilityId", FacilityId);
            hstInput.Add("@HospId", HospId);

            strqry = "SELECT TOP 1 StatusID FROM EncounterStatus WITH (NOLOCK) " +
                    " WHERE EncounterID = @EncounterId AND Active = 1 AND FacilityID = @FacilityId AND HospitalLocationID = @HospId " +
                    " ORDER BY Id DESC ";

            ds = objDl.FillDataSet(CommandType.Text, strqry, hstInput);

            int currentStatusId = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                currentStatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusID"]);
            }

            strqry = " SELECT StatusId, Status, Code" +
                        " FROM StatusMaster WITH (NOLOCK) " +
                        " WHERE StatusType = 'Encounter' AND Active = 1 ";

            strqry += " ORDER BY SequenceNo ";

            ds = objDl.FillDataSet(CommandType.Text, strqry, hstInput);

            int nextStautsId = 0;
            bool isMatch = false;
            for (int rIdx = 0; rIdx < ds.Tables[0].Rows.Count; rIdx++)
            {
                DataRow DR = ds.Tables[0].Rows[rIdx];

                if (Convert.ToInt32(DR["StatusId"]) == currentStatusId)
                {
                    isMatch = true;
                    continue;
                }
                if (isMatch)
                {
                    if (Convert.ToString(DR["Code"]) == "MD"
                        || Convert.ToString(DR["Code"]) == "SB"
                        || Convert.ToString(DR["Code"]) == "DA"
                        || Convert.ToString(DR["Code"]) == "BR")
                    {
                        nextStautsId = Convert.ToInt32(DR["StatusId"]);

                        if (nextStautsId == 199 && currentStatusId != 185)//199-Discharge Approved, 185-Bill Prepared
                        {
                            nextStautsId = 0;
                        }
                        break;
                    }
                }
            }

            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "StatusId = " + nextStautsId;

            return DV.ToTable();
        }

        public String SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int AdvisingDoctorId,
                                    string Remarks, string xmlItems, int EncodedBy)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();

            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@chvRemarks", Remarks);
            hshIn.Add("@xmlItems", xmlItems);
            hshIn.Add("@intEncodedBy", EncodedBy);


            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicine", hshIn, hshOut);
            return hshOut["@chvErrorStatus"].ToString();
        }


        //palendra
        public String SaveInterHospitalTranfer(int TransferId, int FacilityId, int HospitalLocationId, int RegistrationId, int EncounterId, int FromFacilityId, int ToFacilityId,
                                   int SpecilizationId, int ProviderId, int BedCategoryId, int EncodedBy)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();

            hshIn.Add("@TransferId", TransferId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@HospitalLocationId", HospitalLocationId);
            hshIn.Add("@RegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@FromFacilityId", FromFacilityId);
            hshIn.Add("@ToFacilityId", ToFacilityId);
            hshIn.Add("@intSpecilizationId", SpecilizationId);
            hshIn.Add("@intDoctorId", ProviderId);
            hshIn.Add("@intBedCategoryId", BedCategoryId);
            hshIn.Add("@Encodedby", EncodedBy);

            hshOut.Add("@ErrorStatus", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveFacilityTransfer", hshIn, hshOut);
            return hshOut["@ErrorStatus"].ToString();
        }

        public DataSet GetFacilityTransferDetails(int FacilityId, int HospitalLocationId, int intEncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            DataSet ds = new DataSet();
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@inyHospitallocationId", HospitalLocationId);
            hstInput.Add("@intEncounterId", intEncounterId);
            //
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetFacilityTransferDetails", hstInput);

            return ds;
        }
        //palendra
        /// <summary>
        /// To save the Dischage Summary
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="EncounterID"></param>
        /// <param name="RegistrationID"></param>
        /// <param name="DischargedSummary"></param>
        /// <param name="EncodedBy"></param>
        /// <returns></returns>
        public string SaveDischargeSummary(int HospitalLocationId, int EncounterID, int RegistrationID,
            string DischargedSummary, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("HospitalLocationId", HospitalLocationId);
            HshIn.Add("EncounterID", EncounterID);
            HshIn.Add("RegistrationID", RegistrationID);
            HshIn.Add("DischargedSummary", DischargedSummary);
            HshIn.Add("intEncodeBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "usp_Save_DischargeSummary", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }

        public string SaveReferralSlip(int ReferralReplyId, int ReferralId, int HospitalId, int EncounterId, DateTime Referraldate, int ReferDoctorId, string Doctornote,
                                 int Urgent, bool status, int Encodedby, string SaveReferralSlip, int Active, string DoctorReferral, int SpecializationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intReferralId", ReferralId);
            HshIn.Add("@intReferralReplyId", ReferralReplyId);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@dtReferralDate", Referraldate);
            HshIn.Add("@intReferToDoctorId", ReferDoctorId);
            HshIn.Add("@chvNote", Doctornote);
            HshIn.Add("@chvDoctorRemarks", DoctorReferral);
            HshIn.Add("@bitUrgent", Urgent);
            HshIn.Add("@bitStatus", status);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedby", Encodedby);
            HshIn.Add("@intSpecializationId", SpecializationId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveIPReferralDetail", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataSet GetReferralDetail(int UserId, int EncId, string EncounterType, int FacilityId, string RegistrationNo,
                                        int ConcludeReferral, int ReferralId, int NurseReferralRequestId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();

            hstInput.Add("@intUserId", UserId);
            hstInput.Add("@intEncounterId", EncId);
            hstInput.Add("@chvEncounterType", EncounterType);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@chvRegistrationNo", RegistrationNo);
            hstInput.Add("@inyConcludeReferral", ConcludeReferral);
            hstInput.Add("@intReferralId", ReferralId);
            if (NurseReferralRequestId > 0)
            {
                hstInput.Add("@intNurseReferralRequestId", NurseReferralRequestId);
            }
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferral", hstInput);
            return ds;
        }
        public DataSet GetReferralDetailForSerach(int UserId, int EncId, String EncounterType, int FacilityId, string RegistrationNo,
            int ConcludeReferral, int ReferralId, string PatientName, int ReferralType, DateTime FromDate, DateTime ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();

            hstInput.Add("@intUserId", UserId);
            hstInput.Add("@intEncounterId", EncId);
            hstInput.Add("@chvEncounterType", EncounterType);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@chvRegistrationNo", RegistrationNo);
            hstInput.Add("@inyConcludeReferral", ConcludeReferral);
            hstInput.Add("@intReferralId", ReferralId);
            hstInput.Add("@dtFromDate", FromDate);
            hstInput.Add("@dtToDate", ToDate);
            hstInput.Add("@inyReferralType", ReferralType);
            hstInput.Add("@chvPatientName", PatientName);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferral", hstInput);
            return ds;
        }
        public DataSet GetReferralDetailCount(int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("@intUserId", UserId);
            //hstInput.Add("@chvEncounterType", EncounterType);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferralCount", hstInput);
            return ds;
        }
        public string SaveReferralConclusion(int Id, string Doctroremarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strupdate = string.Empty;
            hstInput = new Hashtable();

            hstInput.Add("@intId", Id);
            hstInput.Add("@chvDoctorremarks", Doctroremarks);

            strupdate = "UPDATE  IPReferralDetail SET DoctorRemark=@chvDoctorremarks , ConcludeReferral=1 WHERE Id=@intId";

            int i = objDl.ExecuteNonQuery(CommandType.Text, strupdate, hstInput);

            return Convert.ToString(i);

        }


        public DataSet GetReferralDetailOP(int UserId, int EncId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            hstInput = new Hashtable();
            // string strsql = "";
            //strsql = "select rd.Id,EncounterId,Convert(varchar,ReferralDate,113) ReferralDate,ReferToDoctorId,dbo.GetDoctorName(emp.Id) As DoctorName,Note,CASE WHEN Urgent=0 THEN 'Routine' ELSE 'Urgent' END  Urgent, " +
            //       " CASE WHEN ConcludeReferral=0 THEN 'Open' ELSE 'Close' END  ConcludeReferral,rd.DoctorRemark,dbo.GetDoctorName(emp1.ID) As EncodedBy  from IPReferralDetail rd " +
            //       " inner join Employee emp on rd.ReferToDoctorId=emp.ID  inner join Users us On rd.EncodedBy  = us.ID " +
            //       " inner join Employee emp1 on us.EmpID=emp1.ID where rd.EncounterId =" + EncId;

            hstInput.Add("@intUserId", UserId);
            hstInput.Add("@intEncounterId", EncId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferralOP", hstInput);

            return ds;

        }
        public string SaveReferralConclusionOP(int Id, string Doctroremarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strupdate = string.Empty;
            hstInput = new Hashtable();

            hstInput.Add("@intId", Id);
            hstInput.Add("@chvDoctorremarks", Doctroremarks);

            strupdate = "UPDATE  OPReferralDetail SET DoctorRemark=@chvDoctorremarks , ConcludeReferral=1 WHERE Id=@intId";

            int i = objDl.ExecuteNonQuery(CommandType.Text, strupdate, hstInput);

            return Convert.ToString(i);

        }

        public DataSet GetItemConversionFactor(int ItemId)
        {
            ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strupdate = string.Empty;

            hstInput = new Hashtable();
            hstInput.Add("@ItemId", ItemId);

            strupdate = " SELECT t1.ItemId, t3.ConversionFactor1, t3.ConversionFactor2 " +
                        " FROM PhrItemMaster t1 WITH (NOLOCK) " +
                        " INNER JOIN PhrItemWithItemUnitTagging t2 WITH (NOLOCK) ON t1.ItemId = t2.ItemId AND t2.Active = 1 AND t1.Active = 1 " +
                        " INNER JOIN PhrItemUnitMaster t3 WITH (NOLOCK) ON t2.ItemUnitId = t3.ItemUnitId AND t3.Active = 1 " +
                        " WHERE t1.ItemId = @ItemId";

            ds = objDl.FillDataSet(CommandType.Text, strupdate, hstInput);
            return ds;
        }

        //Added on 21-08-2014 Start Naushad
        public DataSet getAcknowlegeDurgMain(int HospId, int FacilityId, int RegistraionID, int EncoutnerID, int WardId,
                                            string Status, string IssueNo)
        {
            ds = new DataSet();
            try
            {
                Hashtable HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistraionid", RegistraionID);
                HshIn.Add("@intEncouterID", EncoutnerID);
                HshIn.Add("@intWardid", WardId);
                HshIn.Add("@chrAckStatus", Status);
                HshIn.Add("@chvIssueNo", IssueNo);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrMain", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }


        public DataSet getAcknowlegeDurgDetail(int RegistraionID, int EncoutnerID, int IssueID)
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@intRegistraionid", RegistraionID);
                HshIn.Add("@intEncouterID", EncoutnerID);
                HshIn.Add("@intIssueId", IssueID);



                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAckPhrDetail", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public string AcknowledgePhrissueDetail(int HospId, int FacilityID, int RegID, int EncounterID, string xmlDetails, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intHospitalLocationID", HospId);
            HshIn.Add("@intFacilityId", FacilityID);
            HshIn.Add("@intRegistraionId", RegID);
            HshIn.Add("@intEncounterId", EncounterID);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlPhrissueDetail", xmlDetails);
            HshOut.Add("@Chrstatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspAcknowledgePhrissueDetail", HshIn, HshOut);

            return HshOut["@Chrstatus"].ToString();
        }


        public DataSet GetExpectedDateOfDischargePatient(int HospId, int FacilityId, string Daterange, string Fomedate, string Todate,
                                                        string StatusCode, int WardId)
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                HshIn.Add("@chvDateRange", Daterange);
                HshIn.Add("@chrFromDate", Fomedate);
                HshIn.Add("@chrToDate", Todate);

                if (StatusCode != string.Empty)
                {
                    HshIn.Add("@chvStatusCode", StatusCode);
                }
                if (WardId > 0)
                {
                    HshIn.Add("@intWardId", WardId);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetExpectedDateOfDischargePatient", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }
        public DataSet GetDischargeStatusfromward(int HospId, int FacilityId, int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            string strsql = string.Empty;

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncounterId);

            //strsql = "SELECT DischargeStatus FROM EncounterStatus WITH (NOLOCK) WHERE Id  = (SELECT MAX(ID) FROM EncounterStatus WITH (NOLOCK) where EncounterID =@intEncounterId and StatusID=183 and FacilityID=@intFacilityId and HospitalLocationID =@inyHospitalLocationId )";

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEncounterDischargeStatus", HshIn);
            return ds;
        }

        public DataSet GetDischargeStatusfromMRD(int HospId, int FacilityId, int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            string strsql = string.Empty;

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncounterId);

            //strsql = "SELECT DischargeStatus FROM EncounterStatus WITH (NOLOCK) WHERE Id  = (SELECT MAX(ID) FROM EncounterStatus WITH (NOLOCK) where EncounterID =@intEncounterId and StatusID=183 and FacilityID=@intFacilityId and HospitalLocationID =@inyHospitalLocationId )";

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEncounterDischargeStatusMrd", HshIn);
            return ds;
        }
        public DataSet GetDischargeStatusNo(int HospId, int FacilityId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetExpectedDateOfDischargeNoOf", HshIn);
            return ds;
        }

        public DataSet getUnacknowledgeServices(int iHospID, int iLoginFacilityId, string fromdate, string todate,
                                int WardId, int RegistrationNo, string EncounterNo, string PatientName)
        {
            return getUnacknowledgeServices(iHospID, iLoginFacilityId, fromdate, todate,
                              WardId, RegistrationNo, EncounterNo, PatientName, 0, string.Empty, string.Empty);
        }

        public DataSet getUnacknowledgeServices(int iHospID, int iLoginFacilityId, string fromdate, string todate,
                              int WardId, int RegistrationNo, string EncounterNo, string PatientName, int EncounterId, string type, string StatusCode)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            if (EncounterId > 0)
            {
                HshIn.Add("@EncounterId", EncounterId);
            }
            if (type != string.Empty)
            {
                HshIn.Add("@type", type);
            }
            if (!string.IsNullOrEmpty(fromdate))
            {
                HshIn.Add("@chvToDate", todate);
                HshIn.Add("@chvFromDate", fromdate);
            }
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvPatientName", PatientName);
            if (StatusCode != string.Empty)
            {
                HshIn.Add("@chvStatusCode", StatusCode);
            }
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPUnperformedServicesForWard", HshIn);
            return ds;
        }

        public DataSet getIPPatientRequest(int HospId, int FacilityId, string EncounterNo, int StoreId, string PendingStatus, string fromdate, string todate,
                                    int WardId, int RegistrationNo, string PatientName)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@chrPendingStatus", PendingStatus);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            if (!string.IsNullOrEmpty(fromdate))
            {
                HshIn.Add("@chvToDate", todate);
                HshIn.Add("@chvFromDate", fromdate);
            }

            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@intWardId", WardId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientRequest", HshIn, HshOut);
            return ds;
        }

        public DataSet NonDrugOrderForWard(int HospitalLocationId, int FacilityId, string fromdate, string todate,
                                int WardId, int RegistrationNo, string EncounterNo, string PatientName)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intRegistrationId", 0);
            HshIn.Add("intEncounterId", 0);
            if (!string.IsNullOrEmpty(fromdate))
            {
                HshIn.Add("@chvToDate", todate);
                HshIn.Add("@chvFromDate", fromdate);
            }
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvPatientName", PatientName);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetICMNONDrugOrderForWard", HshIn);
            return ds;
        }

        public DataSet GetDrugOrderDetails(int HospitalLocationId, int FacilityId, int IndentId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspPendingDrugOrderDetails", HshIn);
            return ds;
        }
        public string IsSendToBilling(int HospId, int FacilityID, int RegID, int EncounterID, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intHospitalLocationID", HospId);
            HshIn.Add("@intFacilityId", FacilityID);
            HshIn.Add("@intRegistraionId", RegID);
            HshIn.Add("@intEncounterId", EncounterID);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@Chrstatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspIsSendToBilling", HshIn, HshOut);

            return HshOut["@Chrstatus"].ToString();
        }

        public Hashtable SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
                                       string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime,
                                       string xmlUnApprovedPrescriptionIds, int RequestFromOtherWardId, bool IsFromWard, Boolean ApprovalRequired)
        {
            return SaveEMRMedicine(HospId, FacilityId, RegistrationId, EncounterId, IndentType, iIndentStoreId, AdvisingDoctorId,
                                        xmlItems, xmlItemDetail, Remarks, DrugOrderType, EncodedBy, IsConsumable, xmlFrequencyTime,
                                        xmlUnApprovedPrescriptionIds, RequestFromOtherWardId, IsFromWard, ApprovalRequired, false,
                                        string.Empty, false, string.Empty, string.Empty, 0, string.Empty, 0, 0);
        }

        public Hashtable SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
                                       string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime,
                                       string xmlUnApprovedPrescriptionIds, int RequestFromOtherWardId, bool IsFromWard, Boolean ApprovalRequired,
                                       bool IsReadBack, string IsReadBackNote, bool IsCompoundedDrugOrder, string CompoundedDrugName,
                                       string CompoundedDose, int CompoundedUnitId, string CompoundedPrescriptionDetail, int CompoundedItemId)
        {
            return SaveEMRMedicine(HospId, FacilityId, RegistrationId, EncounterId, IndentType, iIndentStoreId, AdvisingDoctorId,
                                        xmlItems, xmlItemDetail, Remarks, DrugOrderType, EncodedBy, IsConsumable, xmlFrequencyTime,
                                        xmlUnApprovedPrescriptionIds, RequestFromOtherWardId, IsFromWard, ApprovalRequired, IsReadBack,
                                        IsReadBackNote, IsCompoundedDrugOrder, CompoundedDrugName, CompoundedDose, CompoundedUnitId, CompoundedPrescriptionDetail, CompoundedItemId, 0);
        }
        //public Hashtable SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
        //                              string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime,
        //                              string xmlUnApprovedPrescriptionIds, int RequestFromOtherWardId, bool IsFromWard, Boolean ApprovalRequired,
        //                              bool IsReadBack, string IsReadBackNote, bool IsCompoundedDrugOrder, string CompoundedDrugName,
        //                              string CompoundedDose, int CompoundedUnitId, string CompoundedPrescriptionDetail, int CompoundedItemId, int ReasonId)
        //{
        //    return SaveEMRMedicine(HospId, FacilityId, RegistrationId, EncounterId, IndentType, iIndentStoreId, AdvisingDoctorId,
        //                               xmlItems, xmlItemDetail, Remarks, DrugOrderType, EncodedBy, IsConsumable, xmlFrequencyTime,
        //                               xmlUnApprovedPrescriptionIds, RequestFromOtherWardId, IsFromWard, ApprovalRequired,
        //                               IsReadBack, IsReadBackNote, IsCompoundedDrugOrder, CompoundedDrugName,
        //                               CompoundedDose, CompoundedUnitId, CompoundedPrescriptionDetail, CompoundedItemId, ReasonId);

        //}


        public Hashtable SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
                                   string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime,
                                   string xmlUnApprovedPrescriptionIds, int RequestFromOtherWardId, bool IsFromWard, Boolean ApprovalRequired,
                                   bool IsReadBack, string IsReadBackNote, bool IsCompoundedDrugOrder, string CompoundedDrugName,
                                   string CompoundedDose, int CompoundedUnitId, string CompoundedPrescriptionDetail, int CompoundedItemId, int ReasonId)
        {
            return SaveEMRMedicine(HospId, FacilityId, RegistrationId, EncounterId, IndentType, iIndentStoreId, AdvisingDoctorId,
                                        xmlItems, xmlItemDetail, Remarks, DrugOrderType, EncodedBy, IsConsumable, xmlFrequencyTime,
                                        xmlUnApprovedPrescriptionIds, RequestFromOtherWardId, IsFromWard, ApprovalRequired,
                                        IsReadBack, IsReadBackNote, IsCompoundedDrugOrder, CompoundedDrugName,
                                        CompoundedDose, CompoundedUnitId, CompoundedPrescriptionDetail, CompoundedItemId, ReasonId, 0);
        }
        public Hashtable SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
                                       string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime,
                                       string xmlUnApprovedPrescriptionIds, int RequestFromOtherWardId, bool IsFromWard, Boolean ApprovalRequired,
                                       bool IsReadBack, string IsReadBackNote, bool IsCompoundedDrugOrder, string CompoundedDrugName,
                                       string CompoundedDose, int CompoundedUnitId, string CompoundedPrescriptionDetail, int CompoundedItemId, int ReasonId, int OTBookingId)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();

            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@intStoreId", iIndentStoreId);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@xmlItems", xmlItems);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            hshIn.Add("@chvRemarks", Remarks);
            hshIn.Add("@intEncodedBy", EncodedBy);
            hshIn.Add("@bitIsConsumable", IsConsumable);
            hshIn.Add("@intDrugOrderType", DrugOrderType);

            if (!xmlUnApprovedPrescriptionIds.Equals(string.Empty))
            {
                hshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
            }
            if (RequestFromOtherWardId > 0)
            {
                hshIn.Add("@intRequestFromOtherWardId", RequestFromOtherWardId);
            }
            if (IsFromWard)
            {
                hshIn.Add("@bitIsFromWard", IsFromWard);
            }
            hshIn.Add("@bitApprovalRequired", ApprovalRequired);
            hshIn.Add("@bitIsReadBack", IsReadBack);
            hshIn.Add("@chvIsReadBackNote", IsReadBackNote);

            if (IsCompoundedDrugOrder)
            {
                hshIn.Add("@bitIsCompoundedDrugOrder", IsCompoundedDrugOrder);
                hshIn.Add("@chvCompoundedDrugName", CompoundedDrugName);
                hshIn.Add("@chvCompoundedDose", CompoundedDose);
                hshIn.Add("@intCompoundedUnitId", CompoundedUnitId);
                hshIn.Add("@chvCompoundedPrescriptionDetail", CompoundedPrescriptionDetail);
                hshIn.Add("@intCompoundedItemId", CompoundedItemId);
            }
            if (ReasonId > 0)
            {
                hshIn.Add("@intReasonId", ReasonId);
            }
            if (OTBookingId > 0)
            {
                hshIn.Add("@intOTBookingId", OTBookingId);
            }

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut.Add("@intPrescriptionId", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicine", hshIn, hshOut);
            return hshOut;
        }

        public Hashtable SaveEMRMedicine(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
    string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime, string xmlUnApprovedPrescriptionIds)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();

            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@intIndentType", IndentType);
            hshIn.Add("@intStoreId", iIndentStoreId);
            hshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            hshIn.Add("@xmlItems", xmlItems);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            hshIn.Add("@chvRemarks", Remarks);
            hshIn.Add("@intEncodedBy", EncodedBy);
            hshIn.Add("@bitIsConsumable", IsConsumable);
            hshIn.Add("@intDrugOrderType", DrugOrderType);

            if (xmlUnApprovedPrescriptionIds != string.Empty)
            {
                hshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
            }

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut.Add("@intPrescriptionId", SqlDbType.VarChar);

            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicine", hshIn, hshOut);
            return hshOut;
        }

        public Hashtable SaveEMRMedicineAddNewDrug(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                                      string xmlItems, string xmlItemDetail, string xmlFrequencyTime, string Remarks, int userId)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            BaseC.ParseData bc = new BaseC.ParseData();

            hshIn.Add("@intIndentId", IndentId);
            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@intRegistrationId", RegistrationId);
            hshIn.Add("@intEncounterId", EncounterId);
            hshIn.Add("@xmlItems", xmlItems);
            hshIn.Add("@xmlItemDetail", xmlItemDetail);
            hshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            hshIn.Add("@chvRemarks", Remarks);
            hshIn.Add("@intEncodedBy", userId);

            hshOut.Add("@intPrescriptionId", SqlDbType.VarChar);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


            hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineIPAddNewDrug", hshIn, hshOut);
            return hshOut;
        }
        public Hashtable SaveEMRMedicineOP(int HospId, int FacilityId, int RegistrationId,
                                int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                                int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy,
                                string xmlFrequencyTime, bool IsConsumable, string xmlUnApprovedPrescriptionIds)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            BaseC.ParseData bc = new BaseC.ParseData();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentType", IndentType);
                HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
                HshIn.Add("@bitIsPregnant", IsPregnant);
                HshIn.Add("@bitIsBreastFeeding", IsBreastFeeding);
                HshIn.Add("@xmlItems", xmlItems);
                HshIn.Add("@xmlItemDetail", xmlItemDetail);
                HshIn.Add("@xmlPatientAlerts", xmlPatientAlerts);
                HshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@bitIsConsumable", IsConsumable);
                if (xmlUnApprovedPrescriptionIds != string.Empty)
                {
                    HshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
                }
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
                HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOP", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
                bc = null;
            }
            // return HshOut["@chvErrorStatus"].ToString();
            return HshOut;
        }




        public string SaveReferralSlip(int ReferralReplyId, int ReferralId, int HospitalId, int EncounterId, DateTime Referraldate, int ReferDoctorId, string Doctornote,
                             int Urgent, bool status, int Encodedby, string SaveReferralSlip, int Active, string DoctorReferral, int SpecializationId, int ReferralRequestId,
                             int intFacilityId, int ReferFromDoctorId, int NurseReferralRequestId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intReferralId", ReferralId);
            HshIn.Add("@intReferralReplyId", ReferralReplyId);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@dtReferralDate", Referraldate);
            HshIn.Add("@intReferToDoctorId", ReferDoctorId);
            HshIn.Add("@chvNote", Doctornote);
            HshIn.Add("@chvDoctorRemarks", DoctorReferral);
            HshIn.Add("@bitUrgent", Urgent);
            HshIn.Add("@bitStatus", status);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedby", Encodedby);
            HshIn.Add("@intSpecializationId", SpecializationId);
            HshIn.Add("@ReferralRequestId", ReferralRequestId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intReferFromDoctorId", ReferFromDoctorId);
            if (NurseReferralRequestId > 0)
            {
                HshIn.Add("@intNurseReferralRequestId", NurseReferralRequestId);
            }
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveIPReferralDetail", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }

        public string SendReferralAck(int ReferralId, int HospitalId, int EncounterId, int ReferDoctorId, int Encodedby, int intFacilityId, int ReferFromDoctorId, int NurseReferralRequestId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intReferralId", ReferralId);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intReferToDoctorId", ReferDoctorId);
            HshIn.Add("@intEncodedby", Encodedby);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intReferFromDoctorId", ReferFromDoctorId);
            if (NurseReferralRequestId > 0)
            {
                HshIn.Add("@intNurseReferralRequestId", NurseReferralRequestId);
            }
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDoctorReferralAck", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }




        public DataSet GetIPServiceAcitivityDetails(int inyHospitalLocationId, int intFacilityId, int RegId, int EncounterId, int serviceId,
                  int intDepartmentId, string DepartmentType, int StatusId, int Status)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@RegId", RegId);
            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@intServiceId", serviceId);
            HshIn.Add("@intDepartmentId", intDepartmentId);
            HshIn.Add("@chrDepartmentType", DepartmentType);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intActive", Status);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetIPServiceAcitivityDetails", HshIn);
            return ds;
        }

        //chnage Palendra
        public DataSet GetEmployeDetail(int inyHospitalLocationId, int intFacilityId, int UserId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", intFacilityId);
            HshIn.Add("@intUserID", UserId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeID", HshIn);
            return ds;
        }
        //chnage Palendra
        public string SaveDischargeOutlierStatus(int EncounterId, int OutlierId,
                 int Active, int Encodedby)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@OutlierId", OutlierId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardSaveDischargeOutlier", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetDischargeOutlierStatus(int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();


            HshIn.Add("@EncounterId", EncounterId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetDischargeOutlier", HshIn);
            return ds;
        }

        public string SavePatientAcknowledgement(int EncounterId, DateTime AcknowledgedDateTime, int Encodedby)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@AcknowledgedDateTime", AcknowledgedDateTime);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardSavePatientAcknowledgeTime", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetPatientAcknowledgementStatus(int EncounterId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@EncounterId", EncounterId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetPatientAcknowledgeTime", HshIn);
            return ds;
        }
        public DataSet getOralGivenDetail(int hospid, int FacilityId, int wardId)
        {
            return getOralGivenDetail(hospid, FacilityId, wardId, 0, "", "");
        }

        public DataSet getOralGivenDetail(int hospid, int FacilityId, int wardId, int RegistrationNo, string EncounterNo, string PatientName)
        {
            Hashtable hsIN = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsIN.Add("@inyHospitalLocationId", hospid);
            hsIN.Add("@intFacilityId", FacilityId);
            hsIN.Add("@intWardId", wardId);

            if (RegistrationNo > 0)
            {
                hsIN.Add("@intRegistrationNo", RegistrationNo);
            }
            if (EncounterNo != string.Empty)
            {
                hsIN.Add("@chvEncounterNo", EncounterNo);
            }
            if (PatientName != string.Empty)
            {
                hsIN.Add("@chvPatientName", PatientName);
            }
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetOralDetails", hsIN);
            return ds;


        }
        public DataSet GetRejectedDrugs(int EncounterId, int FacilityId, bool All)
        {
            Hashtable hsIN = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsIN.Add("@intEncounterId", EncounterId);
            hsIN.Add("@intFacilityId", FacilityId);
            hsIN.Add("@bitAll", All);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetPatientUnApprovedDrugs", hsIN);
            return ds;


        }
        public DataSet getAckByWarkDetails(int HospitalLocationID, int FacilityId, string EncounterNo, int WardId, int RegistrationNo, string PatientName)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("inyHospitalLocationId", HospitalLocationID);
            hstInput.Add("intFacilityId", FacilityId);
            hstInput.Add("intRegistrationId", RegistrationNo);
            hstInput.Add("intWardId", WardId);
            hstInput.Add("sSearchContent", PatientName);
            hstInput.Add("intEncounterNo", EncounterNo);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetWardAckByWardDetails", hstInput);
            return ds;
        }

        public string SaveUpdateHousekeepingRequest(int RequestId, int FacilityId, int BedId, int EncounterId, int ReasonId,
                                                    string OtherRemarks, string Operation, bool RevertBack, int EncodedBy, int OTId)// Akshay
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intRequestId", RequestId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intBedId", BedId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intOTId", OTId);// Akshay

                if (ReasonId > 0)
                {
                    HshIn.Add("@intReasonId", ReasonId);
                }

                if (OtherRemarks != string.Empty)
                {
                    HshIn.Add("@chvOtherRemarks", OtherRemarks);
                }

                HshIn.Add("@chrOperation", Operation); //G-Generate, L-Cancel, A-Acknowledge, C-Close

                if (RevertBack)
                {
                    HshIn.Add("@bitRevertBack", RevertBack);
                }

                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardSaveUpdateHousekeepingRequest", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getWardHousekeepingRequest(int FacilityId, int EncounterId, int RegistrationNo, string EncounterNo,
                                                string BedNo, string FromDate, string ToDate, string SearchFor, int OTId)
        {
            ds = new DataSet();
            try
            {
                Hashtable HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@intFacilityId", FacilityId);
                if (EncounterId > 0)
                {
                    HshIn.Add("@intEncounterId", EncounterId);
                }
                if (RegistrationNo > 0)
                {
                    HshIn.Add("@intRegistrationNo", RegistrationNo);
                }

                if (EncounterNo != string.Empty)
                {
                    HshIn.Add("@chvEncounterNo", EncounterNo);
                }
                if (BedNo != string.Empty)
                {
                    HshIn.Add("@chvBedNo", BedNo);
                }
                if (FromDate != string.Empty && ToDate != string.Empty)
                {
                    HshIn.Add("@chvFromDate", FromDate);
                    HshIn.Add("@chvToDate", ToDate);
                }

                HshIn.Add("@intOTId", OTId);

                HshIn.Add("@chrSearchFor", SearchFor);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetHousekeepingRequest", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public DataSet GetWardDashBoard(int hospitalLocationId, int FacilityId, int WardId, int WardStationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();

            hstInput.Add("inyHospitalLocationId", hospitalLocationId);
            hstInput.Add("intFacilityId", FacilityId);
            if (WardId > 0)
            {
                hstInput.Add("intWardId", WardId);
            }
            if (WardStationId > 0)
            {
                hstInput.Add("intWardStationId", WardStationId);
            }

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardNotifications", hstInput);
            return ds;
        }

        public DataSet EMRGetNotifications(int hospitalLocationId, int FacilityId, int WardId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();

            hstInput.Add("inyHospitalLocationId", hospitalLocationId);
            hstInput.Add("intFacilityId", FacilityId);
            hstInput.Add("intWardId", WardId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetNotifications", hstInput);
            return ds;
        }

        public DataSet WardNotificationDetails(int iHospitalId, int iFacilityId, int WardId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalId);
            HshIn.Add("@intFacilityId", iFacilityId);
            if (WardId > 0)
            {
                HshIn.Add("@intWardId", WardId);
            }

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardNotificationDetails", HshIn);
            return ds;

        }

        public DataSet GetEMRGetOrdersForApproval(int FacilityId, int AdvisingDoctorId, int Approved, string RegistrationNo, string FirstName, string Ward, string BedNo, string OrderName)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            HshIn.Add("@intApproved", Approved);
            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@chvFirstName", FirstName);
            HshIn.Add("@chvWard", Ward);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@chvOrderName", OrderName);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetOrdersForApproval", HshIn);
            return ds;

        }

        public String EMRUpdateOrdersApproval(int HospitalLocationId, String xmlOrders, int AdvisingDoctorId, int EncodedBy)
        {

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@xmlOrders", xmlOrders);
                HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRUpdateOrdersApproval", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetStopMedicationNotification(int HospitalLocationId, int FacilityId, string RegistratioNo, string EncounterNo,
            string PatientName, int WardId, string FromDate, string ToDate)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvRegistrationNo", RegistratioNo);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@dtFromDate", FromDate);
            HshIn.Add("@dtToDate", ToDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardStopMedicationNotification", HshIn);
            return ds;
        }

        public DataSet GetVulnerableType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.Text, "select StatusId, Status, StatusColor, Code from Statusmaster WITH (NOLOCK) where Statustype='VulnerableType' and active=1 ");
            return ds;
        }

        public string SaveVulnerablePatientDetails(int EncounterId, int HospId, int FacilityId, int RegistrationId, bool IsVulnerablePatient, int VulnerableTypeId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            houtPut = new Hashtable();

            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@intRegistrationId", RegistrationId);
            hstInput.Add("@intEncounterId", EncounterId);
            hstInput.Add("@bitIsVulnerablePatient", IsVulnerablePatient);
            hstInput.Add("@intVulnerableTypeId", VulnerableTypeId);
            hstInput.Add("@intEncodedBy", UserId);

            houtPut.Add("@chvErrorStatus", SqlDbType.VarChar);

            houtPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveVulnerablePatient", hstInput, houtPut);

            return houtPut["@chvErrorStatus"].ToString();
        }


        public DataSet GetVulnerablePatientDetails(int HospitalId, int RegId, int EncId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("@inyHospitallocationId", HospitalId);
            hstInput.Add("@intRegistrationId", RegId);
            hstInput.Add("@intEncounterId", EncId);
            hstInput.Add("@intFacilityId", FacilityId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetVulnerablePatientDetails", hstInput);
            return ds;
        }

        public DataSet GetConsumableOrderHistory(int HospitalLocationId, int FacilityId, int RegistrationId, string ItemName, string FromDate, string ToDate, string OPIP)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@chvItemName", ItemName);
            HshIn.Add("@chrFromDate", FromDate);
            HshIn.Add("@chrToDate", ToDate);
            HshIn.Add("@chvOPIP", OPIP);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspWardGetConsumableOrderHistory", HshIn);
            return ds;
        }

        public string SavePatientDischargeSummaryAcknowledgement(int EncounterId, DateTime AcknowledgedDateTime, int Encodedby, string AcknowledgeType)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@AcknowledgedDateTime", AcknowledgedDateTime);
            HshIn.Add("@AcknowledgeType", AcknowledgeType);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardSavePatientDischargeSummaryAcknowledgeTime", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetPatientDischargeSummaryAcknowledgementStatus(int EncounterId, string AcknowledgeType)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@AcknowledgeType", AcknowledgeType);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetPatientDischargeSummaryAcknowledgeTime", HshIn);
            return ds;
        }

        public string SaveUpdatePatientNurseTagging(int EncounterId, int NurseId, string Remarks, int EncodedBy)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@NurseId", NurseId);
            HshIn.Add("@Remarks", Remarks);
            HshIn.Add("@EncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateWardPatientNurseTagging", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataTable GetEMRPatientNurseTagging(int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@EncounterId", EncounterId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            ds = objDl.FillDataSet(CommandType.Text, "Select NurseId,Remarks from WardPatientNurseTagging Where EncounterId=@EncounterId and IsActiveNurse=1", HshIn);
            return ds.Tables[0];

        }

        public DataTable GetTemplateForMandatoryForMarkForDischarge(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId)
        {

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitallocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            return (objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTemplateForMandatoryForMarkForDischarge", HshIn)).Tables[0];

        }
        public DataTable GetEMRPatientsByNurseId(int NurseId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@NurseId", NurseId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetNurseTaggingedPatients", HshIn);
            return ds.Tables[0];

        }
        public string SavePatientNurseTransfer(int FromNurseId, int ToNurseId, string XMLEncounterData, string Remarks, int TransferBy, int HospitalLocationId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn = new Hashtable();
            HshIn.Add("@fromNurseId", FromNurseId);
            HshIn.Add("@toNurseId", ToNurseId);
            HshIn.Add("@xmlEnounterData", XMLEncounterData);
            HshIn.Add("@Remarks", Remarks);
            HshIn.Add("@intEncodedBy", TransferBy);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardSavePatientTransfer", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getCompoundedDrugOrder(int HospLocId, int FacilityId, int RegistrationId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospLocId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetCompoundedDrugOrder", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }
        public DataSet getWardGetIPPatientApprovalPendingLists(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                     int RegistrationNo, string EncounterNo, string BedNo, string PatientName,
                     DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy, int OrderTypeId, string OrderStatusType,
                     int StoreId, string EncounterStatus, string PatientType, String IsApproved, int WardId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd");

            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            if (RegistrationNo > 0)
            {
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
            }
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@chvFromDate", fDate);
            HshIn.Add("@chvToDate", tDate);
            HshIn.Add("@inyOrderTypeId", OrderTypeId);
            HshIn.Add("@chrPendingStatus", OrderStatusType);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@chvPatientType", PatientType);
            HshIn.Add("@IsApproved", IsApproved);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@chvEncounterStatus", EncounterStatus);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetIPPatientApprovalPendingLists", HshIn, HshOut);
        }

        public string updateIPIndent(int HospitalLocationId, int FacilityId, int EncounterId, int IndentId, string xmlItems,
                                    bool IsClosedByNurse, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@xmlItems", xmlItems);
            HshIn.Add("@bitIsClosedByNurse", IsClosedByNurse);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardUpdateIPIndent", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet GetWARDPatientIndentDetails(int IndentId, int HospId, int EncodedBy, int FacilityId)
        {
            hstInput = new Hashtable();
            houtPut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            hstInput.Add("@intIndentID", IndentId);
            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intEncodedBy", EncodedBy);

            hstInput.Add("@intFacilityId", FacilityId);

            houtPut.Add("@chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspWARDPatientIndentDetails", hstInput, houtPut);

        }

        public DataSet GetEMRPrescriptionReviewMaster(int HospId, int FacilityId)
        {
            hstInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intFacilityId", FacilityId);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRPrescriptionReviewMaster", hstInput);
        }

        public string SaveEmrSoftDietAcknowlegment(int inyHospitalLocationId, int intEncodedBy, string chvAcknowledgeDietXML, string chvAckRemarks)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@chvAcknowledgeDietXML", chvAcknowledgeDietXML);
            HshIn.Add("@chvAckRemarks", chvAckRemarks);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEmrSoftDietAcknowlegment", HshIn, HshOut);
            return HshOut["@chrErrorStatus"].ToString();
        }
        public DataSet GetPreparedMealForApproval(int HospLocId, int FacilityId, string DietType, string FNBDate, string RegistrationNo, string EncounterNo,
                                                  string PatientName, int intDietSlotsId = 0, string ForAckOrUnAck = "UA")
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intHospitalLocationId", HospLocId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvDietType", DietType);
                HshIn.Add("@intDietSlotsId", intDietSlotsId);
                HshIn.Add("@FNBDate", FNBDate);
                HshIn.Add("@ForAckOrUnAck", ForAckOrUnAck);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreparedMealForApproval", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }


        public string saveIPReferralRequest(int EncounterId, int RequestId, int RequestType, int RequestToDoctorId,
                                            string RequestRemarks, int Active, int EncodedBy, int RequestSpecialisationId, int ReferFromDoctorId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intRequestId", RequestId);
            HshIn.Add("@intRequestType", RequestType);
            HshIn.Add("@intRequestToDoctorId", RequestToDoctorId);
            HshIn.Add("@chvRequestRemarks", RequestRemarks);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intRequestSpecialisationId", RequestSpecialisationId);
            HshIn.Add("@intReferFromDoctorId", ReferFromDoctorId);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardSaveIPReferralRequest", HshIn, HshOut);
            return HshOut["@chrErrorStatus"].ToString();
        }

        public DataSet getIPReferralRequest(int FacilityId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetIPReferralRequest", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }


        public DataSet getIPReferralRequestHIS(int FacilityId, int RequestStatusId, int RequestToDoctorId, int RegistrationNo,
                                               string EncounterNo, string PatientName, string FromDate, string ToDate)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequestStatusId", RequestStatusId);
                HshIn.Add("@intRequestToDoctorId", RequestToDoctorId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspWardGetIPReferralRequestHIS", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet getDoctorBasedEmployment(int FacilityId, string EmploymentStatusCode)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvEmploymentStatusCode", EmploymentStatusCode);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDoctorBasedEmployment", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }

        public string updateReferralRequest(int RequestId, string RequestStatus, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intRequestId", RequestId);
            HshIn.Add("@chvRequestStatus", RequestStatus);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardUpdateReferralRequest", HshIn, HshOut);
            return HshOut["@chrErrorStatus"].ToString();
        }

        public DataTable GetReasonMasterList(int Active, int FacilityId, string Flag)
        {
            DataSet ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("@bitActive", Active);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@flag", Flag.Trim());
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrReasonMaster", hstInput);
            return ds.Tables[0];
        }

        public DataTable CheckNoOfIndentInOneDay(int HospId, int FacilityId, int UserId, int EncounterId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intEncounterId", EncounterId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRCheckNoOfIndentInOneDay", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds.Tables[0];
        }

        public DataSet BedTransferCheckInCheckOut(int HospitalLocationID, int FacilityId, string EncounterNo, int WardId, int RegistrationNo, string PatientName)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hstInput = new Hashtable();
            hstInput.Add("inyHospitalLocationId", HospitalLocationID);
            hstInput.Add("intFacilityId", FacilityId);
            hstInput.Add("intRegistrationId", RegistrationNo);
            hstInput.Add("intWardId", WardId);
            hstInput.Add("sSearchContent", PatientName);
            hstInput.Add("intEncounterNo", EncounterNo);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPWardGetCheckInCheckOutBedTransferReq", hstInput);
            return ds;
        }

        public DataSet GetIPTATDashBoardDetails(int HospId, int FacilityId)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UseGetIPTATDashBoard", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet GetIPTATDashBoardDetails(int HospId, int FacilityId, string WardId, int intWardStationId, string ddlfilter, int ddlDuration)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intWardId", WardId);
                if (intWardStationId > 0)
                {
                    HshIn.Add("@intWardStationId", intWardStationId);
                }
                HshIn.Add("@FilterType", ddlfilter);
                if (ddlDuration > 0)
                {
                    HshIn.Add("@FilterDuration", ddlDuration);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UseGetIPTATDashBoard", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }

        //Added By Manoj Puri
        public DataSet CheckDischargeCheckList(int RegistraionID, int EncoutnerID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intRegistraionid", RegistraionID);
                HshIn.Add("@intEncouterID", EncoutnerID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckDischargeChecklist", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }
        }
        #region omprakash sharma ( Favourite ward tagging for nurses)
        public DataSet GetFavouriteWardTagging(int iHospId, bool OnlyTagged, int iEmpId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intEmpId", iEmpId);
                HshIn.Add("@inyHospId", iHospId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetFavouriteWardTagging", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }
        }

        public string SaveFavouriteWardlagTagging(string xmlEmployeeId, string xmlItemFlagIds, int HospId, int FacilityId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@xmlEmployeeId", xmlEmployeeId);
            HshIn.Add("@xmlwardFlagIds", xmlItemFlagIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chrErrormessage", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveFavouriteWardTagging", HshIn, HshOut);

            return HshOut["@chrErrormessage"].ToString();
        }
        #endregion

        // Akshay Tirathram_13082022 
        public DataSet GetNarcoticDetailsForOneDay(int HospId, int FacilityId, int RegistrationId, int ItemId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intItemId", ItemId);

            ds = objDl.FillDataSet(CommandType.Text, "SELECT iim.registrationId, CONVERT(DATE, iim.IndentDate) AS IndentDate, iid.ItemId, iid.Qty FROM ICMIndentMain iim INNER JOIN ICMIndentDetails iid WITH(NOLOCK) ON iim.IndentId = iid.IndentId AND iim.Active=1 AND iid.Active=1 where iim.HospitalLocationId = @intHospitalLocationId AND iim.FacilityId = @intFacilityId AND iim.RegistrationId = @intRegistrationId AND iid.ItemId = @intItemId AND CONVERT(DATE, iim.IndentDate) = CONVERT(DATE, GETDATE())", HshIn);
            return ds;
        }

    }
}
