using System;
using System.Text;
using System.Data;
using System.Collections;

namespace BaseC
{

    public class Diet
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable HshOut;
        DAL.DAL objDl;
        public Diet(string Constring)
        {
            sConString = Constring;
        }

        public DataSet GetAdmissionList(int HospitalId, int FacilityId, string Type, string fromDate, string todate, string FilterATD,
            string RegNo, string EncNo, string PName, string BedNo, string wardName)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@Type", Type);
                HshIn.Add("@DtFrom", fromDate);
                HshIn.Add("@DtTo", todate);
                HshIn.Add("@FilterATD", FilterATD);

                if (EncNo != "")
                {
                    HshIn.Add("@chvEncounterNo", EncNo);
                }
                if (RegNo != "")
                {
                    HshIn.Add("@chvRegistrationNo", RegNo);
                }
                if (BedNo != "")
                {
                    HshIn.Add("@chvBedNo", BedNo);
                }

                if (PName != "")
                {
                    HshIn.Add("@chvPatientName", "%" + PName + "%");
                }

                if (wardName != "")
                {
                    HshIn.Add("@chvWardName", "%" + wardName + "%");
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPDt_AdmissionList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveDietMaster(int DietId, int DietSlotId, string DietName, int DietTypeCategoryId,
            int IsDefaultMeal, int Status, int EncodedBy, int calories, int protien)
        {
            HshIn = new Hashtable();
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

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietMaster", HshIn, HshOut);

                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string DietcancelOrder(int HospitalId, int FacilityId, int Id, int UserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@inFacilityId", FacilityId);
                HshIn.Add("@intId", Id);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDietUpdateOrder", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public string DietAcknowledgment(int HospitalId, int FacilityId, int Id, string Type, int Acknowby)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intId", Id);
                HshIn.Add("@chrType", Type);
                HshIn.Add("@DietAckBy", Acknowby);

                HshOut.Add("@chvErrormsg", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspDietAcknowledgment", HshIn, HshOut);
                return HshOut["@chvErrormsg"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


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
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                HshIn.Add("intHospitalLocationId", HospId);
                HshIn.Add("intDietId", DietId);

                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietCategoryType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        /// <summary>
        /// Get Diet Type
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="DietId"></param>
        /// <returns></returns>
        public DataSet GetDietType(int HospId, int DietId, int DietCatId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                HshIn.Add("intHospitalLocationId", HospId);
                HshIn.Add("intDietDetailId", DietId);
                HshIn.Add("intDietCategoryId", DietCatId);

                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietTypeDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public DataSet GetInternationalMeal()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select id,MealName from DietInternationalMealType WITH (NOLOCK) where Active=1");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetPatientHeightWeight(int HospId, int FacilityId, int EncounterId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPDietGetPatientHeightWeight", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public Hashtable UpdateDietAcknowledgement(int ID, string xtype, int StatusID, int EncounterID, int userid)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intId", ID);
            HshIn.Add("@chvtype", xtype);
            HshIn.Add("@StatusID", StatusID);
            HshIn.Add("@intEncounterID", EncounterID);
            HshIn.Add("@UserID", userid);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpateStatus", HshIn, HshOut);
                //  return HshOut["@chvErrorStatus"].ToString();

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable DietRequsition(int HospitalId, int FacilityId, int RegId, int EncId, int DietCatId, bool DietCahne, string Diagnosis,
                   string bee, string Remarks, int internationalmeal, int UserId, string StingXml,
                   bool Npo, string FoodPrecation, string FoodHabit, int Volume, int NoofAttendent, bool MotherDiet)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilitryId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDietCategoryId", DietCatId);
                HshIn.Add("@btDietChange", DietCahne);
                HshIn.Add("@chvDiagnosis", Diagnosis);
                HshIn.Add("@chvBEE", bee);
                HshIn.Add("@chvRemarks", Remarks);
                if (internationalmeal != 0)
                    HshIn.Add("@intInternationalMeal", internationalmeal);
                else
                    HshIn.Add("@intInternationalMeal", DBNull.Value);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@xmlItemDetails", StingXml);
                HshIn.Add("@btDietNPO", Npo);
                HshIn.Add("@chvFoodPrecaution", FoodPrecation);
                HshIn.Add("@chvFoodHabit", FoodHabit);
                HshIn.Add("@Volume", Volume);
                HshIn.Add("@intNoofAttendent", NoofAttendent);
                HshIn.Add("@bitMotherDiet", MotherDiet);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvOrderNo", SqlDbType.VarChar);
                HshOut.Add("@chrintDietMainID", SqlDbType.VarChar);


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietRequest", HshIn, HshOut);
                //  return HshOut["@chvErrorStatus"].ToString();

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        //Added on 27-08-2014  Start  Naushad
        public int UpdateDietRequisitionMain(int DietRequestID, int EmrDietMainID)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.ExecuteNonQuery(CommandType.Text, "update DietRequestMain set EmrDietMainID=" + EmrDietMainID + " where ID=" + DietRequestID);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added on 27-08-2014 End Naushad


        public DataSet GetPatientsWardWiseForPrintDietMealCard(int HospitalId, int FacilityId, string fromDate, string WardId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@FrmDate", fromDate);
                HshIn.Add("@WardId", WardId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPatientsForPrintDietMealCard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDietOrderForm(int HospitalId, int FacilityId, string fromDate, string todate, string EncNo, string RegNo, string BedNo, string PName, string OrderNo, string @cDietType, bool randomChecked)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@FrmDate", fromDate);
                HshIn.Add("@ToDate", todate);
                HshIn.Add("@cDietType", cDietType);


                if (EncNo != "")
                {
                    HshIn.Add("@chvEncounterNo", EncNo);
                }
                if (RegNo != "")
                {
                    HshIn.Add("@chvRegistrationNo", RegNo);
                }
                if (BedNo != "")
                {
                    HshIn.Add("@chvBedNo", BedNo);
                }

                if (PName != "")
                {
                    //HshIn.Add("@chvPatientName", "%" + PName + "%");
                    if (randomChecked)
                    {
                        HshIn.Add("@chvPatientName", "%" + PName + "%");
                    }
                    else
                    {
                        HshIn.Add("@chvPatientName", "" + PName + "%");
                    }
                }


                if (OrderNo != "")
                {
                    HshIn.Add("@chvOrderNo", OrderNo);
                }


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPDietOrderFromWard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string DietModifyOrderFrom(int HospitalId, int FacilityId, int Id, string Type, string Dietryremarks, int UserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@inFacilityId", FacilityId);
                HshIn.Add("@intId", Id);
                HshIn.Add("@chrType", Type);
                HshIn.Add("@chvDieticianRemarks", Dietryremarks);
                HshIn.Add("@intEncodedBy", UserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDietUpdateOrderStatus", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetKitchenOrder(int HospitalId, int FacilityId, string fromDate, string todate, string EncNo, string RegNo, string BedNo, string PName, string OrderNo, string OPIP)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FrmDate", fromDate);
                HshIn.Add("@ToDate", todate);



                if (EncNo != "")
                {
                    HshIn.Add("@chvEncounterNo", EncNo);
                }
                if (RegNo != "")
                {
                    HshIn.Add("@chvRegistrationNo", RegNo);
                }
                if (BedNo != "")
                {
                    HshIn.Add("@chvBedNo", BedNo);
                }

                if (PName != "")
                {
                    //HshIn.Add("@chvPatientName", "%" + PName + "%");
                    HshIn.Add("@chvPatientName", "" + PName + "%");

                }


                if (OrderNo != "")
                {
                    HshIn.Add("@chvOrderNo", OrderNo);
                }
                HshIn.Add("@OPIP", OPIP);


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetKitchenOrderDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string KOTModifyOrder(int HospitalId, int FacilityId, int Id, string Type, int UserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@inFacilityId", FacilityId);
                HshIn.Add("@intId", Id);
                HshIn.Add("@chrType", Type);
                HshIn.Add("@intEncodedBy", UserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPUpdateKitchenOrderStatus", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDietPatientHistory(int HospitalId, int FacilityId, int EncounterId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPDietHistoryofPatient", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDietiryRemarks(int HospitalId, int FacilityId, int Id)
        {
            string strsql = "";

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@intId", Id);

                strsql = "select DieticianRemarks from DietRequestMain WITH (NOLOCK) where  Id=@intId and FacilityId=@insFacilityId and HospitalLocationId=@inyHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetWard(int HospitalId)
        {
            string strsql = "";

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);

                strsql = "select WardId,WardName from WardMaster WITH (NOLOCK) where Active=1 and HospitalLocationId=@inyHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientDietList(int HospitalId, int FacilityId, string fromDate, int WardId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@FrmDate", fromDate);
                HshIn.Add("@WardId", WardId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPatientDietList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientDietListWardStationWise(int HospitalId, int FacilityId, string fromDate, int WardStationId)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@FrmDate", fromDate);
                HshIn.Add("@WardId", 0);
                HshIn.Add("@WardStationId", WardStationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPatientDietList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CalculateBMIBEE(decimal Height, decimal Weightg, int Age, int Gender, string Type)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHeight", Height);
                HshIn.Add("@intWeight", Weightg);
                HshIn.Add("@intAge", Age);
                HshIn.Add("@intGender", Gender);
                HshIn.Add("@chrType", Type);

                HshOut.Add("@chvResult", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPCalculateBMIandBEE", HshIn, HshOut);
                return HshOut["@chvResult"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //CalculateBMIandBEE
        }

        public DataSet GetDietSlots()
        {
            string strsql = "";

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                strsql = "select DietSlotsId,DietSlotName ,GroupType,SequenceNo from DietSlots WITH (NOLOCK)  where Status=1 order by SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strsql);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetPatientDietMaster(int SlotId, int DietTypeCategoryId)
        public DataSet GetPatientDietMaster(int SlotId, int DietTypeCategoryId)
        {

            string strsql = "";

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intId", SlotId);
                HshIn.Add("@intDietTypeCategoryId", DietTypeCategoryId);
                strsql = "select DietID,Name, ISNULL(Calories,0) as 'calories', ISNULL(Protien,0) as 'protien' from DietMaster WITH (NOLOCK) where DietSlot =@intId and DietTypeCategoryId=@intDietTypeCategoryId and Status=1";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetAlertRecords()
        {


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDietAlert");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetKitchenEntryMaster()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USpDietGetKitchenEntries");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPrecaution()
        {
            string strsql = "";

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            strsql = "select Id,Name from PrecautionMaster WITH (NOLOCK) where Active=1";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strsql);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetFoodHabit()
        {
            string strsql = "";

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                strsql = "select Id,Name from FoodHabit WITH (NOLOCK) where Active=1";

                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SavePatietDietCard(int HospId, int FacilityId, int RegId, int EncId, string XmlDiet, int BedId, string Precation, string FoodHabit, string Remarks, DateTime DietDate, int EncodedDay, int DietCategoryId, int DietRequestID, string SpecialRecommendation, int MealTypeID)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationid", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@xmlDiet", XmlDiet);
                HshIn.Add("@intBedId", BedId);
                HshIn.Add("@chvPrecationDiet", Precation);
                HshIn.Add("@chvFoodHabitDiet", FoodHabit);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@btStatus", 1);
                HshIn.Add("@dtDietDate", DietDate);
                HshIn.Add("@intDietCategoryId", DietCategoryId);
                HshIn.Add("@intEncodedby", EncodedDay);
                HshIn.Add("@intDietRequestID", DietRequestID);
                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
                HshIn.Add("@chvSpecialRecommendation", SpecialRecommendation);
                HshIn.Add("@intMealTypeID", MealTypeID);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSavePatientDietCard", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatietPreviousDietList(int HospitalId, int FacilityId, int RegId, int EncId, string EncNo, DateTime fromDate)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationid", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@chvEncounterNo", EncNo);
                HshIn.Add("@chvDietDate", fromDate.ToString("yyyy/MM/dd"));

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPatientDietCard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string UpdatePatietDietCard(int HospId, int FacilityId, int RegId, int EncId, int DietId, string XmlDiet, string Precation, string FoodHabit, string Remarks, DateTime DietDate, int EncodedDay)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationid", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@xmlDiet", XmlDiet);
                HshIn.Add("@intDietId", DietId);
                HshIn.Add("@chvPrecationDiet", Precation);
                HshIn.Add("@chvFoodHabitDiet", FoodHabit);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@btStatus", 1);
                HshIn.Add("@dtDietDate", DietDate.ToString("yyyy/MM/dd"));
                HshIn.Add("@intEncodedby", EncodedDay);


                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPUpdatePatientDietCard", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetLiquidService(int HospitalId, int FacilityId, string sDate)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrDate", sDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetLiquidService", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDietPatientDetails(int HospitalId, int FacilityId, string IPNo, string BedNo)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                if (IPNo != "")
                    HshIn.Add("@chvEncounterNo", IPNo);
                if (BedNo != "")
                    HshIn.Add("@chvBedNo", BedNo);


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietPatientDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLiquideDietSlotDetails(int FacilityId, int PeriodId)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intPeriodId", PeriodId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetLiquideDietSlotDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetLiquidCode()
        {
            string strsql = "";

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            strsql = "SELECT ID,DietName FROM LiquidDietMaster WITH (NOLOCK) WHERE Active=1 order by DietName";
            try
            {


                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /*
        public string SaveDietTRFeed(int RequestId, string Volume, string First, string Second, string Third, string Fourth, string fifth, string six, string seven, string Eight,
            string Nine, string Ten, string Eleven, string Twelve, string Thirteen, string Fourteen, string Fiveteen, string Sixteen,
            string Seventeen, string Eighteen, string Nineteen, string calorie, string protien, string Remarks, int Period, int EncodedBy, int iRTDetailId, string Precation, string FoodHabit)
        {
           
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intRequestId", RequestId);
            HshIn.Add("@chvVolume", Volume);
            HshIn.Add("@chvFirstSlot", First);
            HshIn.Add("@chvSecondSlot", Second);
            HshIn.Add("@chvThirdSlot", Third);
            HshIn.Add("@chvFourthSlot", Fourth);
            HshIn.Add("@chvFifthSlot", fifth);
            HshIn.Add("@chvSixslot", six);
            HshIn.Add("@chvSevenSlot", seven);
            HshIn.Add("@chvEightSlot", Eight);
            HshIn.Add("@chvNineSlot", Nine);
            HshIn.Add("@chvTenSlot", Ten);
            HshIn.Add("@chvElevenSlot", Eleven);

            HshIn.Add("@chvTwelveSlot", Twelve);
            HshIn.Add("@chvThirteenSlot", Thirteen);
            HshIn.Add("@chvFourteenSlot", Fourteen);
            HshIn.Add("@chvFiveteenSlot", Fiveteen);
            HshIn.Add("@chvSixteenSlot", Sixteen);
            HshIn.Add("@chvSeventeenSlot", Seventeen);
            HshIn.Add("@chvEighteenSlot", Eighteen);
            HshIn.Add("@chvNineteenSlot", Nineteen);

            HshIn.Add("@chvCalorie", calorie);
            HshIn.Add("@chvProtein", protien);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intPeriodId", Period);
            HshIn.Add("@iRTDetailId", iRTDetailId);
            HshIn.Add("@intEncodedby", EncodedBy);
            HshIn.Add("@chvPrecationDiet", Precation);
            HshIn.Add("@chvFoodHabitDiet", FoodHabit);

            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietTRFeed", HshIn, HshOut);
            return HshOut["@chrErrorStatus"].ToString();
        }
        */

        public string SaveLequideMaster(string iFacilityID, string AM6, string AM7, string AM8, string AM9, string AM10, string AM11, string PM12, string PM1, string PM2, string PM3, string PM4, string PM5, string PM6, string PM7, string PM8, string PM9, string PM10, string PM11, string AM12, string AM1, string AM2, string AM3, string AM4, string AM5,
       int Period, int EncodedBy)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@FacilityID", iFacilityID);
            HshIn.Add("@AM6", AM6);
            HshIn.Add("@AM7", AM7);
            HshIn.Add("@AM8", AM8);
            HshIn.Add("@AM9", AM9);
            HshIn.Add("@AM10", AM10);
            HshIn.Add("@AM11", AM11);
            HshIn.Add("@PM12", PM12);
            HshIn.Add("@PM1", PM1);
            HshIn.Add("@PM2", PM2);
            HshIn.Add("@PM3", PM3);
            HshIn.Add("@PM4", PM4);
            HshIn.Add("@PM5", PM5);
            HshIn.Add("@PM6", PM6);
            HshIn.Add("@PM7", PM7);
            HshIn.Add("@PM8", PM8);
            HshIn.Add("@PM9", PM9);
            HshIn.Add("@PM10", PM10);
            HshIn.Add("@PM11", PM11);
            HshIn.Add("@AM12", AM12);
            HshIn.Add("@AM1", AM1);
            HshIn.Add("@AM2", AM2);
            HshIn.Add("@AM3", AM3);
            HshIn.Add("@AM4", AM4);
            HshIn.Add("@AM5", AM5);
            HshIn.Add("@intEncodedby", EncodedBy);
            HshIn.Add("@intPeriodId", Period);
            try
            {

                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveLiquideDietSlotDetails", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveDietTRFeed(int RequestId, string Volume
            , string AM6, string AM7, string AM8, string AM9, string AM10, string AM11, string PM12, string PM1, string PM2, string PM3, string PM4, string PM5, string PM6, string PM7, string PM8, string PM9, string PM10, string PM11, string AM12, string AM1, string AM2, string AM3, string AM4, string AM5,
        string calorie, string protien, string Remarks, int Period, int EncodedBy, int iRTDetailId, string Precation, string FoodHabit, DateTime DietDate)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@intRequestId", RequestId);
            HshIn.Add("@chvVolume", Volume);

            HshIn.Add("@AM6", AM6);
            HshIn.Add("@AM7", AM7);
            HshIn.Add("@AM8", AM8);
            HshIn.Add("@AM9", AM9);
            HshIn.Add("@AM10", AM10);
            HshIn.Add("@AM11", AM11);
            HshIn.Add("@PM12", PM12);
            HshIn.Add("@PM1", PM1);
            HshIn.Add("@PM2", PM2);
            HshIn.Add("@PM3", PM3);
            HshIn.Add("@PM4", PM4);
            HshIn.Add("@PM5", PM5);
            HshIn.Add("@PM6", PM6);
            HshIn.Add("@PM7", PM7);
            HshIn.Add("@PM8", PM8);
            HshIn.Add("@PM9", PM9);
            HshIn.Add("@PM10", PM10);
            HshIn.Add("@PM11", PM11);
            HshIn.Add("@AM12", AM12);
            HshIn.Add("@AM1", AM1);
            HshIn.Add("@AM2", AM2);
            HshIn.Add("@AM3", AM3);
            HshIn.Add("@AM4", AM4);
            HshIn.Add("@AM5", AM5);

            HshIn.Add("@chvCalorie", calorie);
            HshIn.Add("@chvProtein", protien);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intPeriodId", Period);
            HshIn.Add("@iRTDetailId", iRTDetailId);
            HshIn.Add("@intEncodedby", EncodedBy);
            HshIn.Add("@chvPrecationDiet", Precation);
            HshIn.Add("@chvFoodHabitDiet", FoodHabit);
            HshIn.Add("@dtDietDate", DietDate);
            HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
            try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveDietTRFeed", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetRTFeedLiquid(int HospitalId, int FacilityId, int RequestId, int SlotHour)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRequestId", RequestId);
                HshIn.Add("@intSlotHour", SlotHour);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetRTFeedLiquid", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void DeleteRTFeedLiquid(int RequestId)
        {

            HshIn = new Hashtable();
            string sQuery = "UPDATE DietRTFeedDetail SET Active=0 WHERE RequestID=@intRequestId";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intRequestId", RequestId);
                objDl.ExecuteNonQuery(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLiquidSlot()
        {

            string sQuery = "SELECT  ID, Description FROM DietLiquidSlotMaster WITH (NOLOCK) WHERE Active=1 order by PrintSeq  ";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, sQuery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //public DataSet GetLiquidSlot()
        //{
        //    DataSet
        //    string sQuery = "SELECT [SlotId ] as ID,[SlotDesc ] as Description FROM DietLiquidSlotMaster WHERE Active=1 order by PrintSeq  ";
        //    objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //   return objDl.FillDataSet(CommandType.Text, sQuery);
        //   

        //}

        public DataSet GetDefaultMeal(int DietTypeCategoryId, int MealetypeID, string Weekday, DateTime DateMonth, int intFacilityId, int intHospitalLocationID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("intDietTypeCategoryId", DietTypeCategoryId);
            HshIn.Add("intMealetypeID", MealetypeID);
            HshIn.Add("Weekdayname", Weekday);
            HshIn.Add("DateMonth", DateMonth);
            HshIn.Add("intFacilityId", intFacilityId);
            HshIn.Add("intHospitalLocationId", intHospitalLocationID);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDietGetDefaultMeal", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet GetDefaultMealNew(int DietTypeCategoryId, int MealetypeID,int DietSlotID, string Weekday, DateTime DateMonth, int intFacilityId, int intHospitalLocationID)
        //{
        //    objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //   
        //    Hashtable HshIn = new Hashtable();
        //    HshIn.Add("intDietTypeCategoryId", DietTypeCategoryId);
        //    HshIn.Add("intMealetypeID", MealetypeID);
        //    HshIn.Add("SlotId", DietSlotID);
        //    HshIn.Add("Weekdayname", Weekday);
        //    HshIn.Add("DateMonth", DateMonth);
        //    HshIn.Add("intFacilityId", intFacilityId);
        //    HshIn.Add("intHospitalLocationId", intHospitalLocationID);

        //   return objDl.FillDataSet(CommandType.StoredProcedure, "uspDietGetDefaultMealwthMealtype", HshIn);
        //   
        //}
        public DataSet GetDefaultMealNew(int DietTypeCategoryId, int MealetypeID, int DietSlotID, string Weekday, DateTime DateMonth, int intFacilityId, int intHospitalLocationID, int GroupID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("intDietTypeCategoryId", DietTypeCategoryId);
            HshIn.Add("intMealetypeID", MealetypeID);
            HshIn.Add("SlotId", DietSlotID);
            HshIn.Add("Weekdayname", Weekday);
            HshIn.Add("DateMonth", DateMonth);
            HshIn.Add("intFacilityId", intFacilityId);
            HshIn.Add("intHospitalLocationId", intHospitalLocationID);
            HshIn.Add("intGroupID", GroupID);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDietGetDefaultMealwthMealtype", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        //Added on 14-08-2014 Start Naushad Ali
        public string SaveDietPlannerMaster(int HospitalLocationId, int FacilityId,
            int DayofWeek, DateTime DietDate, DateTime MonthYear, int Status, string WeekdayName
            , string xmlDietPlannerDetail, int DietTypeCategoryId, int MealtypeID, int intEncodedBy
            )
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intDayofWeek", DayofWeek);
            HshIn.Add("DietDate", DietDate);
            HshIn.Add("MonthYear", MonthYear);
            HshIn.Add("Status", Status);
            HshIn.Add("WeekdayName", WeekdayName);
            HshIn.Add("xmlDietPlannerDetail", xmlDietPlannerDetail);
            HshIn.Add("DietTypeCategoryId", DietTypeCategoryId);
            HshIn.Add("MealtypeID", MealtypeID);
            HshIn.Add("intEncodedBy", DietTypeCategoryId);



            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspupdateDietPlanner", HshIn, HshOut);


                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDietPlannerMealType(int DietId, int SlotId,
          int iMealtypeID, int intDietPlannerID, DateTime DietDateMonth, int intFacilityId, int intHospitalLocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intDietId", DietId);
            HshIn.Add("intSlotId", SlotId);
            HshIn.Add("iMealtypeId", iMealtypeID);
            HshIn.Add("intDietPlannerID", intDietPlannerID);
            HshIn.Add("DietDateMonth", DietDateMonth);
            HshIn.Add("intFacilityId", intFacilityId);
            HshIn.Add("intHospitalLocationId", intHospitalLocationId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietMasterForPlannerMealType", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDietPlannerMaster(int DietId, int SlotId,
          int iDietTypeCategoryId, int iMealtypeID, int intDietPlannerID, DateTime DietDateMonth, int intFacilityId, int intHospitalLocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("intDietId", DietId);
                HshIn.Add("intSlotId", SlotId);
                HshIn.Add("iMealtypeId", iMealtypeID);
                HshIn.Add("iDietTypeCategoryId", iDietTypeCategoryId);
                HshIn.Add("intDietPlannerID", intDietPlannerID);
                HshIn.Add("DietDateMonth", DietDateMonth);
                HshIn.Add("intFacilityId", intFacilityId);
                HshIn.Add("intHospitalLocationId", intHospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDietMasterForPlanner", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //Added on 14-08-2014 End Naushad Ali

        public DataSet getDietOrderFromEMR(int HospitalId, int FacilityId, string fromDate, string todate, string EncNo, string RegNo,
                                        string BedNo, string PName, string OrderNo, string @cDietType, bool randomChecked)
        {

            Hashtable HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@FrmDate", fromDate);
                HshIn.Add("@ToDate", todate);
                HshIn.Add("@cDietType", cDietType);

                if (EncNo != "")
                {
                    HshIn.Add("@chvEncounterNo", EncNo);
                }
                if (RegNo != "")
                {
                    HshIn.Add("@chvRegistrationNo", RegNo);
                }
                if (BedNo != "")
                {
                    HshIn.Add("@chvBedNo", BedNo);
                }

                if (PName != "")
                {
                    if (randomChecked)
                    {
                        HshIn.Add("@chvPatientName", "%" + PName + "%");
                    }
                    else
                    {
                        HshIn.Add("@chvPatientName", "" + PName + "%");
                    }
                }

                if (OrderNo != "")
                {

                    HshIn.Add("@chvOrderNo", OrderNo);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPDietOrderFromEMR", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetKitchenOrderByNo(int HospitalId, int FacilityId, string KOTNo)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@KOTNo", KOTNo);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetKitchenOrderbyId", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetKitchenOrderSummry(int HospitalId, int FacilityId, string fromDate, string todate)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@FrmDate", fromDate);
            HshIn.Add("@ToDate", todate);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetKitchenOrderSummry", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDietSlot()
        {

            string sQuery = "SELECT DietSlotsId,DietSlotName FROM DietSlots WITH (NOLOCK) WHERE Status=1 order by DietSlotName  ";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                return objDl.FillDataSet(CommandType.Text, sQuery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetSession()
        {

            string sQuery = "SELECT DietSlotsId,DietSlotName FROM DietSlots WITH (NOLOCK) WHERE Status=1 order by DietSlotName  ";
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, sQuery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDietCategoryType()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                string sQuery = "SELECT DietId,DietName FROM DietTypeCategory WITH (NOLOCK) WHERE DietStatus=1 order by DietName  ";
                return objDl.FillDataSet(CommandType.Text, sQuery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetStoredFoodHabitsPrecaution(int HospitalId, int FacilityId, int RequestId, int RequestType)
        {

            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestId", RequestId);
            HshIn.Add("@intType", RequestType);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDietGetStoredFoodHabitsPrecaution", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetPatientPreviousDietDetails(int HospitalId, int FacilityId, int RegId, int EncId, int DietVisitType)
        {


            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationid", RegId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@intDietVisitType", DietVisitType);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPatientPreviousDietDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetKotMaster(int Id)
        {
            Hashtable HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@Id", Id);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetKOTMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
    }
}
