using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class EMRVitals
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable hstOutput;
        public EMRVitals(string Constring)
        {
            sConString = Constring;
        }
        //Not in Use
        //public SqlDataReader GetVitals()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    string sQ = "select VitalId, VitalSignName from EMRVitalSignTypes WHERE Active = 1";
        //    SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQ);
        //    return objDr;
        //}
        //Not in Use
        //public SqlDataReader GetVitalTemplateName(Int16 HospitalLocationID)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("HospitalLocationId", HospitalLocationID);
        //    string sQ = "SELECT ID, TemplateName FROM EMRVitalSignTemplate WHERE HospitalLocationId = @HospitalLocationId";
        //    SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, sQ, HshIn);
        //    return objDr;
        //}
        public DataSet GetVitalTemplate(Int16 HospitalLocationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationID);
                string sQ = "SELECT ID, TemplateName FROM EMRVitalSignTemplate WITH (NOLOCK) WHERE HospitalLocationId = @HospitalLocationId";
                return objDl.FillDataSet(CommandType.Text, sQ, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveVitalTemplateDetails(Int32 iTemplateID, int iEncodedBy, String XMLDetails, int iHospitalLocationId, string sTemplateName, string sDescription, char MeasurementSystem, Boolean Default)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hstOutput = new Hashtable();
                HshIn.Add("@intTemplateID", iTemplateID);

                HshIn.Add("@HospitalLocationId", iHospitalLocationId);
                HshIn.Add("@TemplateName", sTemplateName);
                HshIn.Add("@TemplateDescription", sDescription);
                HshIn.Add("@MeasurementSystem", MeasurementSystem);

                HshIn.Add("@XMLTemplateDetails", XMLDetails);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@bitDefaultTemplate", Default);


                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveVitalTemplateDetails", HshIn, hstOutput);
                return hstOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRDashboardData(string strUserId)
        {
            //Abhishek

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@UserId", strUserId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEMRDashboardData", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEMRGetVitalDetails(int RegistrationId, int EncounterId, string VitalEntryDate)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@RegistrationId", RegistrationId);
            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@VitalEntryDate", VitalEntryDate);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UpsEMRGetVitalDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public String SaveEMRVitalDetails(int RegistrationId, int EncounterId, int UserId, string VitalDatetime, string xmlVitalDetails)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            try
            {
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@EncounterId", EncounterId);
                HshIn.Add("@UserId", UserId);
                HshIn.Add("@VitalEntryDate", VitalDatetime);
                HshIn.Add("@xmlVitalDetails", xmlVitalDetails);
                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRVitalsDetails", HshIn, hstOutput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
            }
            return hstOutput["chvErrorStatus"].ToString();
        }
        public String SaveEMRDashboardData(string strVitals, string strCheifComplaints, string strAlergies, string strDiagnosis, string strPrescription, string strUserId, string strCreatedBy)
        {
            //Abhishek 
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hstOutput = new Hashtable();
                HshIn.Add("@Vitals", strVitals);
                HshIn.Add("@CheifComplaints", strCheifComplaints);
                HshIn.Add("@Alergies", strAlergies);
                HshIn.Add("@Diagnosis", strDiagnosis);
                HshIn.Add("@Prescription", strPrescription);
                HshIn.Add("@UserId", strUserId);
                HshIn.Add("@CreatedBy", strCreatedBy);
                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRDashboardSave", HshIn, hstOutput);
                return hstOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetVitalTemplateDetails(Int32 iTemplateId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@TemplateId", iTemplateId);
                //string sQ = "select td.TemplateId, td.VitalId, t.TemplateName, vt.VitalSignName FROM emrvitalsignTemplateDetails td INNER JOIN EMRVitalSignTemplate t ON td.TemplateId = t.Id INNER JOIN emrVitalSignTypes vt ON td.VitalId = vt.VitalId WHERE vt.active = 1 AND t.HospitalLocationId = @HospitalLocationId order by vt.SequenceNo";
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalTemplateDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet GetVitalTemplateDetails(Int16 iHospitalLocationId, Int32 iTemplateId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("@HospitalLocationId", iHospitalLocationId);
        //    HshIn.Add("@TemplateId", iTemplateId);
        //    //string sQ = "select td.TemplateId, td.VitalId, t.TemplateName, vt.VitalSignName FROM emrvitalsignTemplateDetails td INNER JOIN EMRVitalSignTemplate t ON td.TemplateId = t.Id INNER JOIN emrVitalSignTypes vt ON td.VitalId = vt.VitalId WHERE vt.active = 1 AND t.HospitalLocationId = @HospitalLocationId order by vt.SequenceNo";
        //    DataSet objds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalTemplateDetails", HshIn);
        //    return objds;
        //}


        //Not in use
        //public int SaveVitalTemplate(int iHospitalLocationId, string sTemplateName, string sDescription, char MeasurementSystem)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("@HospitalLocationId", iHospitalLocationId);
        //    HshIn.Add("@TemplateName", sTemplateName);
        //    HshIn.Add("@TemplateDescription", sDescription);
        //    HshIn.Add("@MeasurementSystem", MeasurementSystem);
        //    StringBuilder objStr = new StringBuilder();
        //    objStr.Append("IF NOT exists(select ID FROM EMRVitalSignTemplate WHERE TemplateName = @TemplateName)");
        //    objStr.Append("begin ");
        //    objStr.Append("Insert Into EMRVitalSignTemplate(HospitalLocationId,TemplateName,TemplateDescription,MeasurementSystem)");
        //    objStr.Append(" values(@HospitalLocationId, @TemplateName, @TemplateDescription, @MeasurementSystem)");
        //    objStr.Append(" end");
        //    int i = objDl.ExecuteNonQuery(CommandType.Text, objStr.ToString(), HshIn);
        //    return i;
        //}
        //Not in use
        //public int UpdateSequenceNo(int iTemplateId, int iVitalID, int iSequenceNo)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("@SequenceNo", iSequenceNo);
        //    HshIn.Add("@TemplateId", iTemplateId);
        //    HshIn.Add("@VitalId", iVitalID);
        //    StringBuilder objStr = new StringBuilder();
        //    objStr.Append("Update EMRVitalSignTemplateDetails set SequenceNo = @SequenceNo where TemplateId = @TemplateId and VitalId = @VitalId");
        //    int i = objDl.ExecuteNonQuery(CommandType.Text, objStr.ToString(), HshIn);
        //    return i;
        //}
        //Not in use
        //public bool DeleteTemplate(int iTemplateId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    int i = 0;
        //    HshIn.Add("TemplateId", iTemplateId);
        //    i = objDl.ExecuteNonQuery(CommandType.Text, "DELETE EMRVitalSignTemplate WHERE Id = @TemplateId ;DELETE EMRVitalSignTemplateDetails WHERE TemplateId = @TemplateId", HshIn);
        //    return true;
        //}

        public DataSet GetDoctors(int iHospitalLocationId)
        {
            //Binding Doctor  dropdownlist inside grid
            DataTable dt = new DataTable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospId", iHospitalLocationId);

                StringBuilder str = new StringBuilder();

                str.Append("select e.ID as DoctorID, dbo.GetDoctorName(e.ID) as DoctorName, vst.TemplateName, sm.Name from Employee e WITH (NOLOCK) ");
                str.Append(" LEFT JOIN EMRDoctorVitalSignTemplate dvt WITH (NOLOCK) ON e.ID = dvt.DoctorID and dvt.Active=1 ");
                str.Append(" LEFT JOIN EMRVitalSignTemplate vst WITH (NOLOCK) ON dvt.TemplateId = vst.Id ");
                str.Append(" LEFT JOIN DOCTORDETAILS dd WITH (NOLOCK) ON e.id = dd.DoctorId ");
                str.Append(" LEFT JOIN SpecialisationMaster sm WITH (NOLOCK) ON dd.SpecialisationId = sm.ID ");
                str.Append(" WHERE EmployeeType in (1,2) and e.HospitalLocationId=@inyHospId order by DoctorName ");

                return objDl.FillDataSet(CommandType.Text, str.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorsForVitalTemplates(int iHospitalLocationId, string DoctorText)
        {
            //Binding Doctor  dropdownlist inside grid
            DataTable dt = new DataTable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                StringBuilder str = new StringBuilder();
                str.Append("select e.ID as DoctorID, dbo.GetDoctorName(e.ID) as DoctorName, vst.TemplateName, sm.Name from Employee e WITH (NOLOCK) ");
                str.Append(" LEFT JOIN EMRDoctorVitalSignTemplate dvt WITH (NOLOCK) ON e.ID = dvt.DoctorID ");
                str.Append(" LEFT JOIN EMRVitalSignTemplate vst WITH (NOLOCK) ON dvt.TemplateId = vst.Id ");
                str.Append(" LEFT JOIN DOCTORDETAILS dd WITH (NOLOCK) ON e.id = dd.DoctorId ");
                str.Append(" LEFT JOIN SpecialisationMaster sm WITH (NOLOCK) ON dd.SpecialisationId = sm.ID ");
                str.Append(" where EmployeeType in (1,2) and dbo.GetDoctorName(e.ID) like '%" + DoctorText + "%'  order by DoctorName ");
                return objDl.FillDataSet(CommandType.Text, str.ToString());
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDoctors(int iHospitalLocationId, Int32 iSpecialisationID)
        {
            //Binding Doctor  dropdownlist inside grid
            DataTable dt = new DataTable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                StringBuilder str = new StringBuilder();
                HshIn = new Hashtable();
                HshIn.Add("@HospID", iHospitalLocationId);
                HshIn.Add("@SpecID", iSpecialisationID);

                str.Append(" select emp.ID as DoctorID, dbo.GetDoctorName(emp.ID) as DoctorName from Employee emp WITH (NOLOCK) ");
                str.Append(" inner join employeetype et WITH (NOLOCK) on emp.EmployeeType=et.Id ");
                str.Append(" inner join TitleMaster tm WITH (NOLOCK) on tm.Titleid=emp.TitleId   ");
                str.Append(" inner join doctordetails dd WITH (NOLOCK) on dd.doctorid=emp.id  ");
                str.Append(" inner join SpecialisationMaster sm WITH (NOLOCK) on sm.id=dd.specialisationid ");
                str.Append(" where emp.EmployeeType in (1,2) and et.EmployeeType='D' ");
                str.Append(" and DD.SpecialisationID=@SpecID and emp.hospitalLocationID=@HospID ");
                str.Append(" order by DoctorName  ");

                return objDl.FillDataSet(CommandType.Text, str.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctors(int iHospitalLocationId, string sDoctorName)
        {
            //Binding Doctor  dropdownlist inside grid
            DataTable dt = new DataTable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                StringBuilder str = new StringBuilder();
                HshIn = new Hashtable();
                HshIn.Add("DoctorName", sDoctorName);
                str.Append("EXEC ('select e.ID as DoctorID, dbo.GetDoctorName(e.ID) as DoctorName, vst.TemplateName, sm.Name from Employee e WITH (NOLOCK)  LEFT JOIN EMRDoctorVitalSignTemplate dvt WITH (NOLOCK) ON e.ID = dvt.DoctorID  LEFT JOIN EMRVitalSignTemplate vst WITH (NOLOCK) ON dvt.TemplateId = vst.Id  LEFT JOIN DOCTORDETAILS dd WITH (NOLOCK) ON e.id = dd.DoctorId  LEFT JOIN SpecialisationMaster sm WITH (NOLOCK) ON dd.SpecialisationId = sm.ID  where EmployeeType in (1,2)  AND (e.FirstName LIKE ''%'+@DoctorName+'%'' OR e.MiddleName LIKE ''%'+@DoctorName+'%'' OR e.LastName LIKE ''%'+@DoctorName+'%'') order by DoctorName')");

                return objDl.FillDataSet(CommandType.Text, str.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //Cache["Doctor"] = dr;
        }

        public DataSet VitalTemplateDoctors(Int32 iVitalTemplateID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder str = new StringBuilder();
            HshIn = new Hashtable();
            HshIn.Add("@VitalTemplateID", iVitalTemplateID);
            str.Append("Select dbo.GetDoctorName(DoctorId) as DoctorName,DoctorId,id From EMRDoctorVitalSignTemplate WITH (NOLOCK) where Templateid = @VitalTemplateID And Active = 1");
            try
            {
                return objDl.FillDataSet(CommandType.Text, str.ToString(), HshIn);

                //Cache["Doctor"] = dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet DeActivateVitalTemplateDoctors(int Id)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder str = new StringBuilder();
            HshIn = new Hashtable();
            HshIn.Add("@Id", Id);
            str.Append("update EMRDoctorVitalSignTemplate set Active=0 where id=@Id");
            try
            {
                return objDl.FillDataSet(CommandType.Text, str.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //Cache["Doctor"] = dr;
        }

        public String LinkDoctorVitalTemplate(Int32 iDoctorId, Int32 iTemplateId, Int32 iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intTemplateId", iTemplateId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                Hashtable hstOutput = new Hashtable();
                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "EMRSaveDoctorVitalSignTemplate", HshIn, hstOutput);
                return hstOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAllVitals(Int16 iHospID)
        {
            StringBuilder strSQL = new StringBuilder();
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospID);
                // strSQL.Remove(0, strSQL.Length);
                //strSQL.Append("select vitalid,vitalsignName FROM EMRVitalSignTypes Where  Active=1 and AutoCalculation=0");
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public Int32 DeActivateVitalTemplateDetails(Int32 iTempID, Int32 iVitalID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@TempID", iTempID);
                HshIn.Add("@VitalID", iVitalID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE emrvitalsignTemplateDetails SET Active = '0' WHERE TemplateID=@TempID and VitalID = @VitalID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Int32 ActivateVitalTemplateDetails(Int32 iTempID, Int32 iVitalID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@TempID", iTempID);
                HshIn.Add("@VitalID", iVitalID);
                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE emrvitalsignTemplateDetails SET Active = '1' WHERE TemplateID=@TempID and VitalID = @VitalID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientEncounterVitals(int EncId)
        {
            return GetPatientEncounterVitals(EncId, 0, "", "");
        }
        public DataSet GetPatientEncounterVitals(int EncId, int VitalSignId, string fromdate, string todate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@intVitalSignId", VitalSignId);
            HshIn.Add("@chrFromDate", fromdate);
            HshIn.Add("@chrToDate", todate);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientEncounterVitals", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetHospitalVitalTemplate(int HospId, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intDoctorId", DoctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetHospitalVitalTemplate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetVitalTemplateDetails(int iTemplateId, int FacilityId, int RegId)
        {
            return GetVitalTemplateDetails(iTemplateId, FacilityId, RegId, 0);
        }

        public DataSet GetVitalTemplateDetails(int iTemplateId, int FacilityId, int RegId, int VitalDetailsId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@TemplateId", iTemplateId);
                HshIn.Add("@intFacilitiId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);

                if (VitalDetailsId > 0)
                {
                    HshIn.Add("@intVitalDetailsId", VitalDetailsId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalTemplateDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public void SaveEMRMUDLogVitalValues(int HospId, int RegId, int EncId, int Doctorid, int Encodedby, bool VitalValues)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@intDoctorId", Doctorid);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@bitVitalValues", VitalValues);

                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogVitalValues", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelPatientVitals(int CancelremarkId, int RegId, int EncId, int HospId, int FacilityId, int PageId, int Encodedby,
            string xmlstring)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                hstOutput = new Hashtable();

                HshIn.Add("@inyCancellationRemarksId", CancelremarkId);
                HshIn.Add("@intRegistrationId", RegId); //Request.QueryString["RegNo"];
                HshIn.Add("@intEncounterId", EncId); //Request.QueryString["IpNo"];
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("xmlVitalDetails", xmlstring);

                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                //hshOutput.Add("chvDocumentNo", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRCancelPatientVitals", HshIn, hstOutput);

                return hstOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetVitalSignName(string Dt, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            string sqlstr = "";
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@dtVitalEntryDate", Dt);

            if (Dt != "")
            {
                sqlstr = "set dateformat mdy; Select Distinct evst.VitalId, evst.VitalSignName, evst.DisplayName from EMRPatientVitalSignDetails evsd WITH (NOLOCK) inner join EMRVitalSignTypes evst WITH (NOLOCK) on EVST.VitalId=EVSD.VitalId Where evsd.VitalEntryDate=@dtVitalEntryDate And evst.DisplayInGraph=1";
                ds = objDl.FillDataSet(CommandType.Text, sqlstr, HshIn);
            }
            else
            {
                sqlstr = " select Distinct evst.VitalId, evst.VitalSignName, evst.DisplayName from EMRVitalSignTypes evst WITH (NOLOCK) LEFT JOIN EMRVitalSignTemplateDetails evstd WITH (NOLOCK) on evst.VitalId=evstd.VitalId WHERE evst.DisplayInGraph=1 and evst.Active=1";

                ds = objDl.FillDataSet(CommandType.Text, sqlstr, HshIn);
            }

            return ds;
        }


        public string SavePatientVitalSign(string EntryDate, int RegId, int EncId, int HospId, int FacilityId, int PageId, int Encodedby,
            string xmlstring, int TemplateFieldId, int intVitalDetailsId, int TriageTypeId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@chrVitalEntryDate", EntryDate);
            HshIn.Add("@intRegistrationId", RegId); //Request.QueryString["RegNo"];
            HshIn.Add("@intEncounterId", EncId); //Request.QueryString["IpNo"];
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intPageId", PageId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("xmlVitalDetails", xmlstring);
            HshIn.Add("@intTemplateFieldId", TemplateFieldId);
            HshIn.Add("@intVitalDetailsId", intVitalDetailsId);
            HshIn.Add("@intTriageTypeId", TriageTypeId);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            //hstOutput.Add("chvDocumentNo", SqlDbType.VarChar);


            hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRSavePatientVitalSign", HshIn, hstOutput);

            return hstOutput["chvErrorStatus"].ToString();
        }

        public DataSet bindAutoCalculate(int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospId);
                return objDl.FillDataSet(CommandType.Text, "select Distinct EVSt.VitalId, DisplayName, '' as value, '' as Category, MaxLength, ResultType,0 UnitId from EMRVitalSignTypes EVST WITH (NOLOCK) Left join EMRVitalSignUnits EVU WITH (NOLOCK) on EVU.VitalId=EVST.VitalId Where AutoCalculation=1 and EVST.Active=1 and EVST.HospitalLocationId=@inyHospitalLocationID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet CalculateVitalsValue(int HospId, string xmlstring)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@xmlVitalDetails", xmlstring);


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRCalculateVitalsValue", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindChart(int VitalId, string Category)
        {
            StringBuilder strSQL = new StringBuilder();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intVitalId", VitalId);

                strSQL.Append(" select case Convert(varchar,Convert(int,minvalue)) when '0' then '<='+space(1)+Convert(varchar,maxvalue) else ");
                strSQL.Append(" case Convert(varchar,Convert(int,maxvalue)) when '0' then '>='+space(1)+Convert(varchar,minvalue) else Convert(varchar,minvalue)+space(1)+'-'+space(1)+  Convert(varchar,maxvalue) end end as Range,");
                strSQL.Append(" Category as [" + Category + " Category]");
                strSQL.Append(" from EMRVitalSignRange WITH (NOLOCK) where VitalId=@intVitalId ");

                return objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientPreviousVitals(int HospId, int FacId, int RegId, int EncId, int ViewType, string DRange, string FDate, string TDate,
             int VitalSignId, bool Abnormal)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@inyViewType", ViewType);
                HshIn.Add("@chvDateRange", DRange);
                HshIn.Add("@chrFromDate", FDate);
                HshIn.Add("@chrToDate", TDate);
                HshIn.Add("@intVitalSignId", VitalSignId);
                HshIn.Add("@bitAbnormalValue", Abnormal);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetVitalSignType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strsql = "select DISTINCT VitalId ,(VitalSignName +' ('+ DisplayName + ')') AS VitalSignName ,SequenceNo " +
                               " from EMRVitalSignTypes WITH (NOLOCK)  where VitalId in (select VitalId  from EMRVitalSignTemplateDetails WITH (NOLOCK) where Active=1) " +
                               " order by SequenceNo ";

                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetVitalRange(int VitalId, int AgeFrom, string AgeFromType, int AgeTo, string AgeToType)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intVitalId", VitalId);
            HshIn.Add("@intAgeFrom", AgeFrom);
            HshIn.Add("@chvAgeFromType", AgeFromType);
            HshIn.Add("@intAgeTo", AgeTo);
            HshIn.Add("@chvAgeToType", AgeToType);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strsql = "select AgeFrom,AgeFromType ,AgeTo,AgeToType ,MinValue,MaxValue,Sex,Category  from EMRVitalSignRange WITH (NOLOCK) " +
                                " where VitalId=@intVitalId and AgeFrom=@intAgeFrom and AgeFromType=@chvAgeFromType " +
                                " and AgeTo =@intAgeTo and AgeToType=@chvAgeToType and Active=1  ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetAge(int RegId, int HospId, int FacId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacId);
                HshIn.Add("@intRegistrationId", RegId);

                string strsql = "  select DATEDIFF(DAY,Dateofbirth,getdate())  As Days" +
                                " from Registration WITH (NOLOCK) where Id=@intRegistrationId and FacilityID =@intFacilityId and HospitalLocationId =@inyHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet VitalGraph(int HospId, int FacId, int RegId, string DiasplayName, string DateRange, string FDate, string TDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@chvDisplayName", DiasplayName);
                HshIn.Add("@chvDateRange", DateRange);
                HshIn.Add("@FromDate", FDate);
                HshIn.Add("@ToDate", TDate);


                return objDl.FillDataSet(CommandType.StoredProcedure, "USPEMRVitalGraph", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetVitalImage(int HospId, int FacilityId, int VitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intVitalId", VitalId);
                string strVitalValue = "SELECT id,ImageName,ImagePath FROM EMRVitalImageMaster WITH (NOLOCK) WHERE VitalID=@intVitalId AND Active=1 AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, strVitalValue, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SavePatientVitalSignFromMaster(string EntryDate, int RegId, int EncId, int HospId, int FacilityId, int PageId, int Encodedby,
          string xmlstring)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@chrVitalEntryDate", EntryDate);
            HshIn.Add("@intRegistrationId", RegId); //Request.QueryString["RegNo"];
            HshIn.Add("@intEncounterId", EncId); //Request.QueryString["IpNo"];
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intPageId", PageId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("xmlVitalDetails", xmlstring);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);

            try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientVitalSignFromMasterPage", HshIn, hstOutput);
                return hstOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet VitalGraphMultiple(int HospId, int FacId, int RegId, string DiasplayName, string DateRange, string FDate, string TDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@chvDisplayName", DiasplayName);
            HshIn.Add("@chvDateRange", DateRange);
            HshIn.Add("@FromDate", FDate);
            HshIn.Add("@ToDate", TDate);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPEMRVitalGraphMultiple", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public Hashtable EMRImportVitalsFromICCA(int FacId, string chvId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            HshIn.Add("@intFacilityId", FacId);
            HshIn.Add("@chvId", chvId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            hstOutput.Add("@chvOutPut", SqlDbType.VarChar);
            try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRImportVitalsFromICCA", HshIn, hstOutput);
                return hstOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GEtMEWsScore(int encounterID)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet dxs = dl.FillDataSet(CommandType.Text, "exec uspGEtMEWsScore @encounterID=" + encounterID);
            return dxs;

        }
        public DataSet EMRVitalSignTypes()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "select VitalSignName + ISNULL(' (' + DisplayName + ')','') AS Vital from EMRVitalSignTypes where VitalType='D'");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int ERtoken(int EncounterID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                int strtriageID = Convert.ToInt16(objDl.ExecuteScalar(CommandType.Text, "select ID from ERtoken with(nolock) where ErEncounterID=" + EncounterID));
                return strtriageID;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEvitalresult(int EncounterID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@EncounterID", EncounterID);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEvitalresult", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
