using System;
using System.Collections;
using System.Data;
using System.Configuration;

namespace BaseC
{
    public class Dashboard
    {
        private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;


        public DataSet getDashBoardValue(Int16 HospitalLocationId, int RegistrationId, string sFromDate, string sToDate, string DateVale,
                        string EncounterId, Int32 LoginfacilityId, string ProcName)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            if (ProcName == "UspSearchForms")
            {
                HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            }
            if ((EncounterId != "0") && (EncounterId != ""))
            {
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("intRegistrationId", RegistrationId);
            }
            else
            {
                HshIn.Add("intRegistrationId", RegistrationId);
                HshIn.Add("@chvDateRange", DateVale);
                if (DateVale == "4")
                {
                    HshIn.Add("chrFromDate", sFromDate);
                    HshIn.Add("chrToDate", sToDate);
                }
            }

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, ProcName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getAllergies(int RegistrationId)
        {
            return getAllergies(RegistrationId, 1);
        }

        public DataSet getAllergies(int RegistrationId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            try
            {


                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getResourcePDAppointment(int HospId, string PatientName, string ResourceIds, int StatusId, int FacilityId,
                                    Int64 RegistrationNo, string DateRange, string FromDate, string ToDate,
                                    int LoginFacilityId, string MobileNo, string chvResource, int WardId, int isActive, string EncounterNo)
        {
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@chvName", PatientName);
            HshIn.Add("@chvResourceId", ResourceIds);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@chvDateRange", DateRange);
            HshIn.Add("@chrFromDate", FromDate);
            HshIn.Add("@chrToDate", ToDate);
            HshIn.Add("@intLoginFacilityId", LoginFacilityId);
            HshIn.Add("@chvResource", chvResource);
            HshIn.Add("@MobileNo", MobileNo);
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@IsActive", isActive);
            HshIn.Add("@EncounterNo", EncounterNo);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchResourceAppointmentList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEMRPatientTemplates(int HospitalLocationId, int FacilityId, int EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientTemplates", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable getNotesForProvider(Int16 HospitalLocationId, Int16 EmpId, Int32 LoginfacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            //string strsql = "select Statusid from GetStatus(" + HospitalLocationId + " ,'Notes') where Code='P'";
            //string StatusId = (string)objDl.ExecuteScalar(CommandType.Text, strsql);

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intProviderId", EmpId);
            //HshIn.Add("@intStatusId", StatusId);

            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchForms", HshIn);
                DataView dv = new DataView(objDs.Tables[0]);
                dv.RowFilter = "StatusId<>21";
                DataTable dt = new DataTable();
                dt = dv.ToTable();
                //dv =(DataView);
                return dt;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getNotes(Int16 HospitalLocationId, int RegistrationId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchForms", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getNotes(Int16 HospitalLocationId, int RegistrationId, string fromDate, string toDate, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@chrFromDate", fromDate);
            HshIn.Add("@chrToDate", toDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchForms", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getNotes(Int16 HospitalLocationId, string EncounterId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intEncounterId", EncounterId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchForms", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getTasks(Int16 HospitalLocationId, int RegistrationId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getTasks(Int16 HospitalLocationId, int RegistrationId, string fromDate, string toDate, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            HshIn.Add("@chrFromDate", fromDate);
            HshIn.Add("@chrToDate", toDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getTasks(Int16 HospitalLocationId, string EncounterId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intEncounterId", EncounterId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getProblems(Int16 HospitalLocationId, int RegistrationId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getProblems(Int16 HospitalLocationId, int RegistrationId, string fromDate, string toDate)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            HshIn.Add("@chvFromDate", fromDate);
            HshIn.Add("@chvToDate", toDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getProblems(Int16 HospitalLocationId, string EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intEncounterId", EncounterId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getMedication(Int16 HospitalLocationId, int RegistrationNo)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrugHistory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getMedication(Int16 HospitalLocationId, int RegistrationNo, string fromDate, string toDate)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            HshIn.Add("@chrFromDate", fromDate);
            HshIn.Add("@chrToDate", toDate);
            HshIn.Add("@chrDateType", 4);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrugHistory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getMedication(Int16 HospitalLocationId, string EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intEncounterId", EncounterId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrugHistory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAssessment(Int16 HospitalLocationId, int DoctorId, int RegistrationId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@intRegistrationId", RegistrationId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetAllPrevAssessment", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOrders(Int16 HospitalLocationId, int RegistrationNo)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            //HshIn.Add("@intEncounterId", 0);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOrders(Int16 HospitalLocationId, int RegistrationNo, string fromDate, string toDate)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            HshIn.Add("@chvFromDate", fromDate);
            HshIn.Add("@chvToDate", toDate);
            //HshIn.Add("@intEncounterId", 0);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOrders(Int16 HospitalLocationId, string EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intEncounterId", EncounterId);
            //HshIn.Add("@intEncounterId", 0);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAppointments(Int16 HospitalLocationId, int RegistrationId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intRegistrationId", RegistrationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAppointments(Int16 HospitalLocationId, int RegistrationId, string sFromDate, string sToDate)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intRegistrationId", RegistrationId);
            HshIn.Add("inyDateType", 4);
            HshIn.Add("chrFromDate", sFromDate);
            HshIn.Add("chrToDate", sToDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAppointments(Int16 HospitalLocationId, string EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDiagnosis(Int16 HospitalLocationId, int RegistrationId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intRegistrationId", RegistrationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDiagnosis(Int16 HospitalLocationId, int RegistrationId, string sFromDate, string sToDate)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intRegistrationId", RegistrationId);
            HshIn.Add("chvFromDate", sFromDate);
            HshIn.Add("chvToDate", sToDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDiagnosis(Int16 HospitalLocationId, string EncounterId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intEncounterId", EncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAllergies(Int16 HospitalLocationId, int RegistrationNo, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            //HshIn.Add("@intEncounterId", 0);
            //AddFilterCriteria(HshIn);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


            //Changed DataTable with DataSet
            //DataTable dtDrug = CreateTable();
            //DataTable dtFood = CreateTable();
            //DataTable dtOther = CreateTable();

            //Hashtable HshIn = new Hashtable();
            //HshIn.Add("@HospID", Session["HospitalLocationID"].ToString());

            //System.Text.StringBuilder strSQL = new System.Text.StringBuilder();
            //strSQL.Append("select Convert(tinyint,ISNULL(NKDA,0)) NKDA from registration where HospitalLocationID=@HospID and RegistrationNo=@RegNo and Status='A';");
            //strSQL.Append("select Convert(tinyint,ISNULL(NKFA,0)) NKFA from registration where HospitalLocationID=@HospID and RegistrationNo=@RegNo and Status='A';");
            //strSQL.Append("select Convert(tinyint,ISNULL(NKOA,0)) NKOA from registration where HospitalLocationID=@HospID and RegistrationNo=@RegNo and Status='A';");
            //DataSet ds1 = dl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
            //if (ds1.Tables[0].Rows.Count > 0)
            //{
            //    if (ds1.Tables[0].Rows[0][0].ToString() == "0")
            //    {
            //        DataView dv;
            //        dv = ds.Tables[0].DefaultView;
            //        dv.RowFilter = "TypeID = 1 ";
            //        if (dv.Count > 0)
            //        {
            //            dv.ToTable("dtDrug");
            //        }
            //    }
            //    else
            //    {
            //        DataRow drDrug = dtDrug.NewRow();
            //        drDrug["TypeID"] = "0";
            //        drDrug["TypeName"] = "No Known Drug Allergy";
            //        dtDrug.Rows.Add(drDrug);
            //    }
            //}
            //if (ds1.Tables[1].Rows.Count > 0)
            //{
            //    if (ds1.Tables[1].Rows[0][0].ToString() == "0")
            //    {
            //        DataView dv;
            //        dv = ds.Tables[0].DefaultView;
            //        dv.RowFilter = "TypeID = 2 ";
            //        if (dv.Count > 0)
            //        {
            //            dv.ToTable("dtFood");
            //        }
            //    }
            //    else
            //    {
            //        DataRow drFood = dtFood.NewRow();
            //        drFood["TypeID"] = "0";
            //        drFood["TypeName"] = "No Known Food Allergy";
            //        dtFood.Rows.Add(drFood);
            //    }

            //}
            //if (ds1.Tables[2].Rows.Count > 0)
            //{
            //    if (ds1.Tables[2].Rows[0][0].ToString() == "0")
            //    {
            //        DataView dv;
            //        dv = ds.Tables[0].DefaultView;
            //        dv.RowFilter = "TypeID = 3 ";
            //        if (dv.Count > 0)
            //        {
            //            //dv.ToTable("dtOther");
            //            dtOther = dv.Table;
            //        }
            //    }
            //    else
            //    {
            //        DataRow drOther = dtOther.NewRow();
            //        drOther["TypeID"] = "0";
            //        drOther["TypeName"] = "No Known Other Allergy";
            //        dtOther.Rows.Add(drOther);
            //    }

            //}
            //DataTable dtTemp = new DataTable();
            //dtTemp.Merge(dtDrug);
            //dtTemp.Merge(dtFood);
            //dtTemp.Merge(dtOther);
            //return dtTemp; 
        }

        public DataSet getVitals(Int16 HospitalLocationId, int RegistrationNo) //, int EncounterNo)
        {
            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            HshIn.Add("@inyViewType", 2);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getVitals(Int16 HospitalLocationId, int RegistrationNo, string sFromDate, string sToDate) //, int EncounterNo)
        {
            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", RegistrationNo);
            HshIn.Add("@inyViewType", 2);
            HshIn.Add("@chvFromDate", sFromDate);
            HshIn.Add("@chvToDate", sToDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getVitals(Int16 HospitalLocationId, string EncounterId) //, int EncounterNo)
        {
            Hashtable HshIn = new Hashtable();
            //HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intRegistrationId", 0);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@inyViewType", 2);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        private DataTable CreateTable()
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("TypeID");
            Dt.Columns.Add("TypeName");
            return Dt;
        }


        public DataSet getPDAppointment(Int16 HospitalLocationId, Int16 EmpId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospital5LocationId", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@chvProviderId", EmpId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchAppointmentList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPDAppointment(Int16 HospitalLocationId, Int32 EmpId, string daterange, string FromDate, string ToDate, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@chvProviderId", EmpId);

            HshIn.Add("@chvDateRange", daterange);
            if ((daterange == "4") || (daterange == "WW0") || (daterange == "WW+1") || (daterange == "MM0"))
            {
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
            }
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchAppointmentList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPDAppointment(int HospId, string PatientName, string ProviderIds, int StatusId, int FacilityId,
                                    Int64 RegistrationNo, string OldRegistrationNo, string DateRange, string FromDate, string ToDate,
                                    int LoginFacilityId, string EnrolleNo, string MobileNo)
        {
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@chvName", PatientName);
            HshIn.Add("@chvProviderId", ProviderIds);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvRegistrationNo", RegistrationNo);
            HshIn.Add("@chvOldRegistrationNo", OldRegistrationNo);
            HshIn.Add("@chvDateRange", DateRange);
            HshIn.Add("@chrFromDate", FromDate);
            HshIn.Add("@chrToDate", ToDate);
            HshIn.Add("@intLoginFacilityId", LoginFacilityId);
            HshIn.Add("@cEnrolleNo", EnrolleNo);
            HshIn.Add("@MobileNo", MobileNo);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchAppointmentList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPDNotes(Int16 HospitalLocationId, Int16 EmpId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);

                string strsql = "select Statusid from GetStatus(@inyHospitalLocationId,'Notes') where Code='P'";
                Int16 StatusId = (Int16)objDl.ExecuteScalar(CommandType.Text, strsql);

                HshIn.Add("@intLoginFacilityId", LoginfacilityId);
                HshIn.Add("@intProviderId", EmpId);
                HshIn.Add("@intStatusId", StatusId);

                //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); 
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchForms", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getPDTask(Int16 HospitalLocationId, Int16 EmpId, Int32 LoginfacilityId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intLoginFacilityId", LoginfacilityId);
            HshIn.Add("@intEmpAssignedTo", EmpId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTaskDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPatientEncounterDetails(int HospId, int FacilityId, int RegistrationId)
        {
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@HospitalLocationId", HospId);
            HshIn.Add("@FacilityID", FacilityId);
            HshIn.Add("@RegistrationId", RegistrationId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}