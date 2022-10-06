using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BaseC
{
    public class clsExternalClass
    {
        private string sConString = "";
        Hashtable HshIn;
        public clsExternalClass(string Constring)
        {
            sConString = Constring;
        }


        public DataSet GetEMRUserSetup(int HospitalLocationID, int EmployeeId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intEmployeeId", EmployeeId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRUserSetup", HshIn, HshOut);
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
        }

        public int CheckQueries(int EmployeeId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            
            try
            {
                int i = 0;
                HshIn.Add("@EmployeeId", EmployeeId);
                i = Convert.ToInt32(objDl.ExecuteScalar(CommandType.StoredProcedure, "uspCheckQueries", HshIn));
                return i;
            }
            catch
            {
                throw new Exception();
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
        }

        public DataSet GetGfsQueryDoc(int EmployeeId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);      

            try
            {
                HshIn.Add("@empID", EmployeeId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetGfsQueryDoc", HshIn);

            }
            catch(Exception ex)
            {
                throw new Exception();
            }
            finally
            {
                HshIn = null;

                objDl = null;
            }
        }
        public void UpdateQMSDoctorLogin(string IPaddress)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@IPaddress", IPaddress);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpdateQMSDoctorLogin", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getHospitalSetupValueMultiple(int HospitalLocationId, int FacilityId, string Flag)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@Flag", Flag);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetHospitalSetupValueMultiple", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getHospitalSetupValueSingle(int HospitalLocationId, int FacilityId, string Flag)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@Flag", Flag);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetHospitalSetupValueSingle", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getNoTokenIPaddress(string IPAddress)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@IPAddress", IPAddress);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetNoTokenIPaddress");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string saveQMSlogin(int UserID, int DoctorId, string IPadderss, int FacilityId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@UserID", UserID);
            HshIn.Add("@doctorID", DoctorId);
            HshIn.Add("@IPadderss", IPadderss);
            HshIn.Add("@FacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.ExecuteScalar(CommandType.StoredProcedure, "QMSlogin", HshIn).ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEmployeeWithResource(int HospitalLocationId, int UserId, int SpecilizationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iUserId", UserId);
                HshIn.Add("@intSpecializationId", SpecilizationId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDoctorTimeSpecialisation(int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorTimeSpecialisation", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientAccessRights(int HospitalLocationId, int UserId, int SpecilizationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iUserId", UserId);
                HshIn.Add("@intSpecializationId", SpecilizationId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientAccessRights", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTokentocallDoc(string IPaddress, string Tokenno)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@IPaddress", IPaddress);
                HshIn.Add("@Tokenno", Tokenno);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTokentocallDoc", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public string SaveDisplayCurrentPatient(string IPaddress, string sFlag, string sDoctorName, string sDeptName, string sRegistrationNo,
            int iHospitalLocationId, int iFacilityId, int intEncounterId, int intAppointmentResourceId, int Employeeid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@intAppointmentResourceId", intAppointmentResourceId);
                HshIn.Add("@chvIPAddress", IPaddress);
                HshIn.Add("@chvQMSFlag", sFlag);
                HshIn.Add("@chvDoctorName", sDoctorName);
                HshIn.Add("@chvDeptName", sDeptName);
                HshIn.Add("@chvRegistrationNo", sRegistrationNo);
                HshIn.Add("@intSource", 1);
                HshIn.Add("@Employeeid", Employeeid);

                i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveDisplayCurrentPatient", HshIn);

                return "Sucess";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public String EMRCloseQMS(int DoctorId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@DoctorId", DoctorId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCloseQMS", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string CallPatient(string TokenNo, int UserId, int ServiceID, string CounterID, int ZoneID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshIn.Add("@TokenNo", TokenNo);
                HshIn.Add("@CalledBy", UserId);
                HshIn.Add("@ServiceID", ServiceID);
                HshIn.Add("@counterID", CounterID);
                HshIn.Add("@ZoneID", ZoneID);

                objDl.FillDataSet(CommandType.StoredProcedure, "UspCallPatient", HshIn);
                return "Sucess";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet EMRGetReportName(int ReportId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@ReportId", ReportId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetReportName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet EMRGetFacilityMaster(int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFacilityMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getCustomizedPatientReportHeader(int HeaderId, string PatientType, int EncounterId, int DoctorId, int RegistrationId, int FacilityId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intHeaderId", HeaderId);
                HshIn.Add("@chrAppointmentDate", Convert.ToDateTime(DateTime.Now.Date).ToString("yyyy/MM/dd"));
                HshIn.Add("@intEmployeeId", DoctorId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrPatientType", PatientType);


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getIVFPatient(int RegistrationId, int IVFId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intIVFId", IVFId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatient", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetTemplateStyle(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();

                HshIn.Add("@HospitalLocationId", HospitalLocationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTemplateStyle", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEMRTemplates(int FormId, int EncounterId, int RegistrationId, string EREncounterId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@FormId", FormId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEREnounterId", EREncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetEMRTemplates(int EncounterId, int RegistrationId, string EREncounterId, int TemplateId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@FormId", "1");
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEREnounterId", EREncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intTemplateId", TemplateId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDoctorDetails(int DoctorId, int FacilityId, int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@DoctorId", DoctorId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorDetails", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string UpDateEMRToTranslateLanguage(int RegistrationId, string EMRToTranslateLanguage)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@EMRToTranslateLanguage", EMRToTranslateLanguage);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                //objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpDateEMRToTranslateLanguage", HshIn, HshOut);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpDateEMRToTranslateLanguage", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }
        }
        public DataSet GetProblemsList(string SearchText, int HospitalLocationId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@chvSearchCriteria", SearchText);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemsList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getIVFRegistrationId(int IVFId, int EncounterId, int HospitalLocationId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@intIVFId", IVFId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFRegistrationId", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetIVFPatient(int RegistrationId, int intIVFId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intIVFId", intIVFId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatient", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet SearchPatientByName(int HospitalLocationId, string PatientName, string Status)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@strName", PatientName);
                HshIn.Add("@strStatus", Status);

                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetFontDetails(string Type)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                String str = string.Empty;
                //str = "Select FontId, Font" + Type + " from Font" + Type;
                str = " Select FontId, Font" + Type +
                " from Font" + Type + " WITH (NOLOCK) ";

                if (Type == "Size")
                {
                    str += " order by FontId ";
                }
                else if (Type == "Name")
                {
                    str += " order by FontName ";
                }

                //BaseC.Patient objBc = new BaseC.Patient(sConString);
                return objDl.FillDataSet(CommandType.Text, str);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string GetFont(string Type, string FontId)
        {
            DataSet objDs = null;
            objDs = GetFontDetails(Type);
            DataView objDv = objDs.Tables[0].DefaultView;

            try
            {

                if (Type == "Name")
                {
                    objDv.RowFilter = "FontId =" + FontId;
                }
                else if (Type == "Size")
                {
                    objDv.RowFilter = "FontSize =" + FontId;
                }
                if (objDv.Count > 0)
                {
                    DataTable objDt = objDv.ToTable();
                    return objDt.Rows[0]["Font" + Type].ToString();
                }
                else
                    return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDs.Dispose(); }


        }

        public string GetFontNameAndSize(int HospitalLocationId, int FormId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                Hashtable HshInInput = new Hashtable();
                string Fonts = " font-family:Times New Roman ; font-size:9pt; ";
                HshInInput.Add("@HospitalLocationId", HospitalLocationId);
                HshInInput.Add("@FormId", FormId);
                StringBuilder sQ = new StringBuilder();
                sQ.Append("SELECT PgFontType, PgFontSize FROM EMRForms WITH (NOLOCK) ");
                sQ.Append(" WHERE 2=2 ");
                if (FormId > 0)
                {
                    sQ.Append(" AND Id = @FormId ");
                }
                else
                {
                    sQ.Append(" AND DefaultForVisit = 1 ");
                }
                sQ.Append(" AND HospitalLocationId  = @HospitalLocationId ");

                ds = objDl.FillDataSet(CommandType.Text, sQ.ToString(), HshInInput);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fonts = ConcatNameSize(Convert.ToString(ds.Tables[0].Rows[0]["PgFontType"]), Convert.ToString(ds.Tables[0].Rows[0]["PgFontSize"]));
                }
                return Fonts;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public string ConcatNameSize(string Ftid, string FtSize)
        {
            string sName = string.Empty;
            string sFontSize = string.Empty;
            if (Ftid == "1")
                sName += " font-family:Arial ";
            if (Ftid == "2")
                sName += " font-family:Courier New ";
            if (Ftid == "3")
                sName += " font-family:Garamond ";
            if (Ftid == "4")
                sName += " font-family:Georgia ";
            if (Ftid == "5")
                sName += " font-family:MS Sans Serif ";
            if (Ftid == "6")
                sName += " font-family:Segoe UI";
            if (Ftid == "7")
                sName += " font-family:Tahoma ";
            if (Ftid == "8")
                sName += " font-family:Times New Roman ";
            if (Ftid == "9")
                sName += " font-family:Verdana ";


            if (FtSize == "9pt")
                sFontSize += " ; font-size:9pt ";
            if (FtSize == "11pt")
                sFontSize += " ; font-size:11pt ";
            if (FtSize == "12pt")
                sFontSize += " ; font-size:12pt ";
            if (FtSize == "14pt")
                sFontSize += " ; font-size:14pt ";
            if (FtSize == "18pt")
                sFontSize += " ; font-size:18pt ";
            if (FtSize == "20pt")
                sFontSize += " ; font-size:20pt ";
            if (FtSize == "24pt")
                sFontSize += " ; font-size:24pt ";
            if (FtSize == "26pt")
                sFontSize += " ; font-size:26pt ";
            if (FtSize == "36pt")
                sFontSize += " ; font-size:36pt ";

            return sName + sFontSize;
        }

        public DataSet GetPageFormat(int HospitalLocationId, int FormId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HshInInput = new Hashtable();
                HshInInput.Add("@HospitalLocationId", HospitalLocationId);
                HshInInput.Add("@FormId", FormId);
                StringBuilder sQ = new StringBuilder();

                sQ.Append("SELECT PgSize, PgTopMargin, PgBottomMargin, PgLeftMargin, PgRightMargin, PgNoFormat, PgNoAllignment,Pg1HeaderId, ");
                sQ.Append(" Pg1HeaderMargin, Pg1HeaderNote, Pg2HeaderId, Pg2HeaderNote, PgFontType, PgFontSize, Pg1HeaderFontBold, ");
                sQ.Append(" Pg1HeaderFontItalic, Pg1HeaderFontUnderline, Pg1HeaderFontColor, Pg1HeaderFontType, Pg1HeaderFontSize, ");
                sQ.Append(" Pg2HeaderFontBold, Pg2HeaderFontItalic, Pg2HeaderFontUnderline, Pg2HeaderFontColor, Pg2HeaderFontType, ");
                sQ.Append(" Pg2HeaderFontSize, DisplayPgNoInPage1, ISNULL(h1.ShowBorder,0) AS pg1ShowBorder, ");
                sQ.Append(" ISNULL(h2.ShowBorder,0) AS pg2ShowBorder FROM EMRForms f WITH (NOLOCK) ");
                sQ.Append(" INNER JOIN EMRFormHeader h1 WITH (NOLOCK) ON f.Pg1HeaderId = h1.HeaderId ");
                sQ.Append(" INNER JOIN EMRFormHeader h2 WITH (NOLOCK) ON f.Pg2HeaderId = h2.HeaderId ");
                sQ.Append(" WHERE 2=2 ");
                if (FormId > 0)
                {
                    sQ.Append(" AND f.Id = @FormId ");
                }
                else
                {
                    sQ.Append(" AND f.DefaultForVisit= 1 ");
                }
                sQ.Append(" AND f.HospitalLocationId  = @HospitalLocationId ");

                return objDl.FillDataSet(CommandType.Text, sQ.ToString(), HshInInput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetPageFormatString(int HospitalLocationId, int FormId, string pg)
        {
            DataSet dsPageFormat = GetPageFormat(HospitalLocationId, FormId);
            try
            {
                DataRow drPageHeaderFormat = dsPageFormat.Tables[0].Rows[0];
                string sFont = ConcatNameSize(Convert.ToString(drPageHeaderFormat["PgFontType"]), Convert.ToString(drPageHeaderFormat["PgFontSize"]));
                string color = "#" + drPageHeaderFormat[pg + "HeaderFontColor"].ToString();
                string bold = string.Empty, italic = string.Empty;
                if (drPageHeaderFormat[pg + "HeaderFontBold"].ToString() == "True")
                    bold = "font-weight:bold;";
                if (drPageHeaderFormat[pg + "HeaderFontItalic"].ToString() == "True")
                    italic = "font-style:italic;";
                string HeaderFont = "style='" + sFont + ";" + bold + italic;
                return HeaderFont;
            }
            catch (Exception ex) { throw ex; }
            finally { dsPageFormat.Dispose(); }

        }
        public DataSet getFacilityList(int HospId, int UserId, int GroupId, int EncodedBy, string FacilityType)
        {

            Hashtable HshInInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshInInput.Add("@inyHospitalLocationId", HospId);
                HshInInput.Add("@intUserId", UserId);
                HshInInput.Add("@intGroupId", GroupId);
                HshInInput.Add("@chvFacilityType", FacilityType);
                HshInInput.Add("@intEncodedBy", EncodedBy);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilityList", HshInInput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet DeletePatientDiagnosis(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, int UserId, int DiagnosisId, int PageId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intDiagnosisId", DiagnosisId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRDeletePatientDiagnosis", HshIn);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public DataSet LogEducationAndMonograph(int HospitalLocationId, int RegistrationId, int EncounterId, int DoctorId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intEncodedBy", UserId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspEMRMUDLogEducationAndMonograph", HshIn);
                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public DataSet HandleException(int HospitalLocationId, string chvSource, string chvMessage, string chvQueryString, string chvTargetSite,
            string chvStackTrace, string chvServerName, string chvRequestURL, string chvUserAgent, string chvUserIP, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("chvSource", chvSource);
                HshIn.Add("chvMessage", chvMessage);
                HshIn.Add("chvQueryString", chvQueryString);
                HshIn.Add("chvTargetSite", chvTargetSite);
                HshIn.Add("chvStackTrace", chvStackTrace);
                HshIn.Add("chvServerName", chvServerName);
                HshIn.Add("chvRequestURL", chvRequestURL);
                HshIn.Add("chvUserAgent", chvUserAgent);
                HshIn.Add("chvUserIP", chvUserIP);
                HshIn.Add("intUserId", UserId);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSaveExceptionLog", HshIn);

                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public DataSet GetInsuranceQuery(int UserId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "exec uspGetInsuranceQuery " + UserId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet CheckDuplicateServiceOrder(int HospitalLocationId, int FacilityId, int EncounterId, int RegistrationId, int ServiceId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                HshIn.Add("FacilityId", FacilityId);
                HshIn.Add("EncounterId", EncounterId);
                HshIn.Add("RegistrationId", RegistrationId);
                HshIn.Add("ServiceId", ServiceId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckDuplicateServiceOrder", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetServiceDescription(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, int ServiceId,
       int OrderSetId, int CompanyId, int SponsorId, int CardId, int Option, int TemplateId, string xmlServiceIds)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intServiceId", ServiceId);
                HshIn.Add("@intorderSetId", OrderSetId);
                HshIn.Add("@intCompanyid", CompanyId);
                HshIn.Add("@intSponsorId", SponsorId);
                HshIn.Add("@intCardId", CardId);
                HshIn.Add("@Option", Option);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@xmlServiceIds", xmlServiceIds);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceDescriptionForOrderpageV3", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet ValidateErxPatientXML(int RegistrationId, int DoctorId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@registrationid", RegistrationId);
                HshIn.Add("@doctorID", DoctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspValidateErxPatientXML", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable SaveServiceOrderEMR(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId, string sStrXML, string sXMLPatientAlert, string sRemark,
          int iEncodedBy, int iDoctorId, int iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
          int iInsuranceId, int iCardId, string dtOrderDate, string sChargeCalculationRequired, bool bAllergyReviewed,
          int IsERorEMRServices, int RequestId, string xmlTemplateDetails)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("dtsOrderDate", dtOrderDate);
            HshIn.Add("inyOrderType", sOrderType);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEnounterId", iEncounterId);
            HshIn.Add("xmlServices", sStrXML);
            HshIn.Add("xmlPatientAlerts", sXMLPatientAlert);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("chvRemarks", sRemark);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intCompanyId", iCompanyId);
            HshIn.Add("cPayerType", cPayerType);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", sPatientOPIP);
            HshIn.Add("chrChargeCalculationRequired", sChargeCalculationRequired);
            HshIn.Add("bitAllergyReviewed", bAllergyReviewed);
            HshIn.Add("iIsERorEMRServices", IsERorEMRServices);
            HshIn.Add("intRequestId", RequestId);
            HshIn.Add("xmlTemplateDetails", xmlTemplateDetails);

            HshOut.Add("intOrderNo", SqlDbType.VarChar);
            HshOut.Add("intOrderId", SqlDbType.Int);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("intNEncounterID", SqlDbType.Int);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);

                DataTable dt = new DataTable();

                DataColumn dC;
                dC = new DataColumn("intOrderNo", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("intOrderId", typeof(int));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("chvErrorStatus", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("intNEncounterID", typeof(int));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);

                DataRow dr = dt.NewRow();
                if (Convert.ToString(HshOut["intOrderNo"]).Equals(""))
                {
                    dr["intOrderNo"] = "";
                }
                else
                {
                    dr["intOrderNo"] = HshOut["intOrderNo"].ToString();
                }
                if (Convert.ToString(HshOut["intOrderId"]).Equals(""))
                {
                    dr["intOrderId"] = 0;
                }
                else
                {
                    dr["intOrderId"] = Convert.ToInt32(HshOut["intOrderId"]);
                }
                if (Convert.ToString(HshOut["chvErrorStatus"]).Equals(""))
                {
                    dr["chvErrorStatus"] = "";
                }
                else
                {
                    dr["chvErrorStatus"] = HshOut["chvErrorStatus"].ToString();
                }
                if (Convert.ToString(HshOut["intNEncounterID"]).Equals(""))
                {
                    dr["intNEncounterID"] = 0;
                }
                else
                {
                    dr["intNEncounterID"] = Convert.ToInt32(HshOut["intNEncounterID"]);
                }
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSectionTreeForPatientsForms(int HospitalLocationId, int TemplateId, string GenderType, int FormId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@chrGenderType", GenderType);
                HshIn.Add("@intFormId", FormId);
                HshIn.Add("@intTemplateId", TemplateId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetTemplateDetailsTabular(int TemplateId, int EncounterId, string GenderType, int SectionId, int RecordId, int UserId, int OrderId, int RegistrationId,
            int ResultSetId, int EpisodeId, int FacilityId, int EmployeeId, string PatientOPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@chrGenderType", GenderType);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intSecId", SectionId);
                HshIn.Add("@intRecordId", RecordId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intOrderId", OrderId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intResultSetId", ResultSetId);
                HshIn.Add("@intEpisodeId", EpisodeId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intLoginEmployeeId", EmployeeId);
                HshIn.Add("@chvOPIP", PatientOPIP);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetailsTabular", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetTemplateDetails(int TemplateId, int EncounterId, string GenderType, int RegistrationId, int FacilityId, int UserId, bool ShowPreviousData, int OrderId, int RecordId, int ResultSetId,
            int EpisodeId, int SectionId, string TemplateDetails, int ServiceId, int RequestId, int EmployeeId, string PatientOPIP, int OrderDetailId, string FromDate, string ToDate, string GroupingDate, int EREncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@chrGenderType", GenderType);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@bitShowPreviousData", ShowPreviousData);
                HshIn.Add("@intOrderId", OrderId);
                HshIn.Add("@intRecordId", RecordId);
                HshIn.Add("@intResultSetId", ResultSetId);
                HshIn.Add("@intEpisodeId", EpisodeId);
                HshIn.Add("@intSectionID", SectionId);
                HshIn.Add("@xmlTemplateDetails", TemplateDetails);
                HshIn.Add("@intServiceId", ServiceId);
                HshIn.Add("@intRequestId", RequestId);
                HshIn.Add("@intLoginEmployeeId", EmployeeId);
                HshIn.Add("@chvOPIP", PatientOPIP);
                HshIn.Add("@OrderDetailId", OrderDetailId);
                if (FromDate != "")
                    HshIn.Add("@chrFromDate", FromDate);
                if (ToDate != "")
                    HshIn.Add("@chrToDate", ToDate);
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);
                if (EREncounterId > 0)
                    HshIn.Add("@intEREncounterId", EREncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetTemplateDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEncounterEMRStatus(int EncounterId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@EncounterId", EncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEncounterEMRStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientEncounterDetails(int HospitalLocationId, int FacilityId, int RegistrationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@RegistrationId", RegistrationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientEncounterDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEMRTemplateReportSetup(int ReportId, int TemplateId, int DoctorId, string Type, int Active, int HospitalLocationId, int EncodedBy)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable hsOut = new Hashtable();

                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvFlag", Type);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                hsOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateReportSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int DoLockUnLock(string AbsolutePagePath, string Type, string EncounterId, string HospitalLocationId, string FormId, string TemplateId, string Lock)
        {

            bool isDynamic = false;
            string TemplateType = TemplateId.Substring(0, 1);
            if (TemplateType == "T")
            {
                TemplateId = TemplateId.Substring(1);
                isDynamic = true;
            }
            else if (AbsolutePagePath.ToLower() == "/emr/templates/default.aspx")
            {
                isDynamic = true;
            }
            else
                isDynamic = false;

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                int ret = 0;
                string sqlQ = "";
                Hashtable HshInInput = new Hashtable();
                //HshInInput.Add("@intRegistrationNo", RegNo);
                HshInInput.Add("@intEncounterId", EncounterId);
                HshInInput.Add("@inyHospitalLocationId", HospitalLocationId);
                HshInInput.Add("@intformId", FormId);
                HshInInput.Add("@intTemplateId", TemplateId);
                if (Type == "Upd")
                {
                    if (Lock == "Lock")
                        HshInInput.Add("@Lock", 1);
                    else
                        HshInInput.Add("@Lock", 0);


                    sqlQ = "Update EMRPatientFormDetails Set Lock = @Lock From EMRPatientFormDetails fd inner Join EMRPatientForms f"
                       + " On fd.PatientFormId = f.PatientFormId where f.EncounterId = @intEncounterId";
                    if (isDynamic)
                        sqlQ += " And f.FormId = @intformId And fd.TemplateId = @intTemplateId";
                    else
                        sqlQ += " And f.FormId = @intformId And fd.PageId = @intTemplateId";
                    ret = objDl.ExecuteNonQuery(CommandType.Text, sqlQ, HshInInput);
                }
                else
                {
                    sqlQ = "Select fd.Lock From EMRPatientFormDetails fd WITH (NOLOCK) inner Join EMRPatientForms f WITH (NOLOCK) "
                        + " On fd.PatientFormId = f.PatientFormId where f.EncounterId = @intEncounterId";
                    if (isDynamic)
                        sqlQ += " And f.FormId = @intformId And fd.TemplateId = @intTemplateId";
                    else
                        sqlQ += " And f.FormId = @intformId And fd.PageId = @intTemplateId";

                    DataSet dsLocked = new DataSet();
                    dsLocked = objDl.FillDataSet(CommandType.Text, sqlQ, HshInInput);
                    if (dsLocked.Tables[0].Rows.Count > 0)
                        if (dsLocked.Tables[0].Rows[0]["Lock"].ToString() != "")
                            if (((bool)dsLocked.Tables[0].Rows[0]["Lock"]) == true)
                                ret = 1;

                }
                return ret;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSectionTree(int HospitalLocationId, int TemplateId, string GenderType)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable hsOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@chrGenderType", GenderType);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetSectionTree", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFormatText(int FormatId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable hsOut = new Hashtable();

                HshIn.Add("@FormatId", FormatId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetFormatText", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetTemplateSectionRow(int SelectedNode)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable hsOut = new Hashtable();

                HshIn.Add("@intSectionId", SelectedNode);

                return objDl.FillDataSet(CommandType.Text, "SELECT ValueId, ValueName FROM EMRTemplateRows WITH(NOLOCK) WHERE SectionId = @intSectionId AND Active = 1 ORDER BY SequenceNo", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDataObjectExecute(string Query, Hashtable Parameter)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn = Parameter;

                return objDl.FillDataSet(CommandType.Text, Query, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string getTemplateDataObjectQuery(int DataObjectId)
        {
            string Query = "";
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@intDataObjectId", DataObjectId);

                string qry = "SELECT Query " +
                        " FROM EMRTemplateDataObjects WITH (NOLOCK) " +
                        " WHERE Id = @intDataObjectId " +
                        " AND Active = 1 ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    Query = Convert.ToString(ds.Tables[0].Rows[0]["Query"]);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

            return Query;
        }
        public DataSet GetTemplateFieldFormats(int FieldId, int SpecialisationId)
        {
            string Query = "";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                Hashtable hs = new Hashtable();
                hs.Add("@intFieldId", FieldId);
                hs.Add("@SpezID", SpecialisationId);
                return objDl.FillDataSet(CommandType.Text, "Select FormatId, FormatName from EMRTemplateFieldFormats WITH(NOLOCK) WHERE FieldId=@intFieldId And Active=1 and isnull(SpecializationID,@SpezID)=@SpezID Order by FormatName", hs);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getEMRTemplateWordData(int FieldId, int EncounterId, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intFieldId", FieldId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);

                string qry = "SELECT ValueWordProcessor " +
                            " FROM EMRPatientNotesData WITH (NOLOCK) " +
                            " WHERE FieldId = @intFieldId AND EncounterId = @intEncounterId AND RegistrationId = @intRegistrationId AND Active = 1";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEMRTemplateReportSequence(int ReportId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intReportId", ReportId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetTemplateReportSequence", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEMRPatientForms(int FormId, int EncounterId, int ModuleId, int GroupId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@intFormId", FormId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@inyModuleID", ModuleId);
                HshIn.Add("@intGroupId", GroupId);

                string sql = "Select PatientSummary, StatusId from EMRPatientForms WITH(NOLOCK) where EncounterId = @intEncounterId";

                return objDl.FillDataSet(CommandType.Text, sql, HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }

        }
        public DataSet GetEMRTemplateStatic(int HospitalLocationId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);

                string sql = "SELECT PageId,TemplateDisplayTitle,TemplateBold,TemplateItalic,TemplateUnderline,TemplateForecolor,"
            + "TemplateListStyle,TemplateFontStyle,TemplateFontSize, SectionsBold, SectionsItalic, SectionsUnderline,"
            + "SectionsForecolor, SectionsListStyle, SectionsFontStyle, SectionsFontSize, FieldsBold, FieldsItalic,"
            + "FieldsUnderline, FieldsForecolor, FieldsListStyle, FieldsFontStyle, FieldsFontSize ,isnull(TemplateSpaceNumber,1) as TemplateSpaceNumber "
            + "FROM EMRTemplateStatic WITH(NOLOCK) where HospitalLocationId = @inyHospitalLocationID";

                return objDl.FillDataSet(CommandType.Text, sql, HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }

        }
        public DataSet GetSectionTreeForPatientsForms(int HospitalLocationId, int TemplateId, string GenderType, int FormId, int ReportId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("chrGenderType", GenderType);
                HshIn.Add("@intFormId", FormId);
                if (ReportId != 0)
                {
                    HshIn.Add("@intReportId", ReportId);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetSectionTreeForPatientsForms", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }

        }
        public DataSet GetGender(int RegistrationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                string SqlQry = " Select dbo.GetGender(GENDER) as 'Gender' from registration WITH(NOLOCK) where Id = @intRegistrationId";
                return objDl.FillDataSet(CommandType.Text, SqlQry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientROS(int EncounterId, int TemplateId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intTemplateId", TemplateId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientROS", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetTemplateDisplayUserName(int HospitalLocationID, int TemplateId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
                HshIn.Add("@intTemplateId", TemplateId);
                string strDisplayUserName = "select DisplayUserName from EMRTemplate WITH(NOLOCK) where ID=@intTemplateId and HospitalLocationID=@inyHospitalLocationID";
                return objDl.FillDataSet(CommandType.Text, strDisplayUserName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getEMRTemplateVisitRecoreds(int EncounterId, int TemplateId, int FacilityId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateVisitRecoreds", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getEMRTemplate(int HospId, string EmployeeType, int SpecialisationId,
                            int iServiceId, string ApplicableFor, int iTemplateId, string sTemplateIdType, int iFacilityId, bool IsAddendum)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@chvApplicableFor", ApplicableFor);
                HshIn.Add("@intTemplateId", iTemplateId);
                HshIn.Add("@intServiceId", iServiceId);
                HshIn.Add("@intSpecialisationId", SpecialisationId);
                HshIn.Add("@chvEmployeeType", EmployeeType);
                HshIn.Add("@intFacilityId", iFacilityId);

                if (IsAddendum)
                {
                    HshIn.Add("@chvTemplateType", "AD");
                }
                else
                {
                    HshIn.Add("@chvTemplateType", sTemplateIdType);
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUserWiseTemplate", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string getPatientPullforwardTemplate(int TemplateId, int EncounterId, int RegistrationId)
        {
            string pullvalue = string.Empty;
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                pullvalue = Convert.ToString(objDl.ExecuteScalar(CommandType.Text, "select PullForward from EMRPatientForms epf WITH(NOLOCK) INNER JOIN EMRPatientFormDetails epfd WITH(NOLOCK) ON epf.PatientFormId = epfd.PatientFormId AND epfd.TemplateId = " + TemplateId + " WHERE epf.EncounterId = " + EncounterId + " AND epf.RegistrationId =  " + RegistrationId + " AND epf.Active = 1 "));



            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return pullvalue;
        }
        public DataSet GetTemplateRequiredServices(string ServiceOrderDetailIds, string TagType, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@xmlServiceOrderDetailIds", ServiceOrderDetailIds);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chkTagType", TagType);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspTemplateRequiredServices", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet GetServiceTemplates(int TemplateId, int SubDepartmentId, string xmlServiceIds, string TagType, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intServiceId", 0);
                HshIn.Add("@intSubDeptId", SubDepartmentId);
                HshIn.Add("@xmlServiceIds", xmlServiceIds);
                HshIn.Add("@intTemplateId", TemplateId);

                //if (TemplateId > 0)
                //{
                //    HshIn.Add("@intServiceId", 0);
                //    HshIn.Add("@intSubDeptId", 0);
                //    HshIn.Add("@xmlServiceIds", xmlServiceIds);
                //    HshIn.Add("@intTemplateId", TemplateId);
                //}
                //else
                //{
                //    if (TagType.Equals("D"))// Sub Department
                //    {
                //        HshIn.Add("@intServiceId", 0);
                //        HshIn.Add("@intSubDeptId", SubDepartmentId);
                //    }
                //    else // Service
                //    {
                //        HshIn.Add("@intServiceId", 0);
                //        HshIn.Add("@intSubDeptId", 0);
                //        HshIn.Add("@xmlServiceIds", xmlServiceIds);
                //    }
                //}

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetServiceTemplates", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientAllergies(int RegistrationId, string FromDate, string ToDate, string GroupingDate, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chrGroupingDate", GroupingDate);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientAllergies(int RegistrationId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegistrationId);
                //HshIn.Add("@chrFromDate", FromDate);
                //HshIn.Add("@chrToDate", ToDate);
                //HshIn.Add("@chrGroupingDate", GroupingDate);
                HshIn.Add("@bitActive", Active);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientAllergies", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetPatientPreviousVitals(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientPreviousVitals", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientProblemsHPI(int HospitalLocationId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientProblems", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientBasicDetails(int RegistrationId, int EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                string strSql = "";
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                strSql = "Select ISNULL(r.FirstName,'') + ISNULL(' ' + r.MiddleName,'') + ISNULL(' ' + r.LastName,'') AS PatientName, dbo.AgeInYrsMonthDay (Convert(varchar(10),DateofBirth,111), Convert(Varchar(10), Getdate(),111)) as Age, dbo.GetGender(Gender) as Gender,";
                strSql = strSql + " c.Race from Registration r WITH (NOLOCK) Left Join RaceMaster c WITH (NOLOCK) On r.RaceId = c.RaceId";
                strSql = strSql + " where r.Id = @intRegistrationId ";
                strSql = strSql + " Select IsNull(IsPregnant,0) as IsPregnant, IsNull(IsBreastFeeding,0) as IsBreastFeeding from Encounter WITH (NOLOCK) where Id = @intEncounterId";

                return objDl.FillDataSet(CommandType.Text, strSql, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientDrugHistory(int HospitalLocationId, int RegistrationId, int EncounterId, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDrugHistory", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientCurrentMedication(int HospitalLocationId, int RegistrationId, int EncounterId, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientCurrentMedication", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet GetPreviousMedicinesIP(int HospitalLocationId, int FacilityId, int EncounterId, string FromDate, string ToDate, string GroupingDate, string PreviousMedication, int IndentId, int ItemId, string ItemName)
        //{
        //    
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL  objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try { } catch (Exception ex) { throw ex; } finally { HshIn = null; objDl = null; }
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@intEncounterId", EncounterId);


        //        if (FromDate != "" && ToDate != "")
        //        {
        //            HshIn.Add("@chrFromDate", FromDate);
        //            HshIn.Add("@chrToDate", ToDate);
        //        }
        //        if (GroupingDate != "")
        //            HshIn.Add("@chrGroupingDate", GroupingDate);
        //        if (PreviousMedication != "")
        //            HshIn.Add("@chvPreviousMedication", PreviousMedication);
        //        HshIn.Add("@intIndentId", IndentId);
        //        HshIn.Add("@intItemId", ItemId);
        //        if (ItemName.Trim() != string.Empty)
        //        {
        //            HshIn.Add("@chvItemName", ItemName);
        //        }

        //        return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    
        //}
        public DataSet GetPreviousMedicinesIP(int HospitalLocationId, int FacilityId, int EncounterId, string FromDate, string ToDate, string GroupingDate, string PreviousMedication,
        int IndentId, int ItemId, string ItemName, string IndentCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);
                if (PreviousMedication != "")
                    HshIn.Add("@chvPreviousMedication", PreviousMedication);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                //if (ItemName.ToString() != string.Empty)
                //{
                HshIn.Add("@chvItemName", ItemName);
                //}
                HshIn.Add("@chvIndentCode", IndentCode);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetOPMedicines(int HospitalLocationId, int FacilityId, int EncounterId, string FromDate, string ToDate, string GroupingDate, string PreviousMedication)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);
                if (PreviousMedication != "")
                    HshIn.Add("@chvPreviousMedication", PreviousMedication);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPMedicines", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientServices(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientServices", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDietOrderDetailInNote(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDietOrderDetailInNote", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorReferral(int UserId, int FacilityId, int RegistrationNo, int EncounterId, string EncounterType, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterType", EncounterType);

                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@dtFromDate", FromDate);
                    HshIn.Add("@dtToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferral", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; }

        }
        public DataSet GetPatientImmunization(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate, int ScheduleId, int ImminizationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);
                if (ImminizationId != 0)
                    HshIn.Add("@intImmunizationId", ImminizationId);
                if (ScheduleId != 0)
                    HshIn.Add("@intScheduleId", ScheduleId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunization", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEncounterFollowUpAppointment(int HospitalLocationId, int FacilityId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEncounterFollowUpAppointment", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorProgressNote(int HospitalLocationId, int FacilityId, int RegistrationId, int DoctorId, string FromDate, string ToDate, string GroupingDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intDoctorId", DoctorId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorProgressNote", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetDoctorProgressNote(int HospitalLocationId, int FacilityId, int RegistrationId, int DoctorId, string FromDate, string ToDate, string GroupingDate, int iProgressNoteId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn.Add("@intProgressNoteId", iProgressNoteId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intDoctorId", DoctorId);
                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorProgressNote", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null; HshIn = null;
            }
        }
        public DataSet GetPatientDailyInjections(int HospitalLocationId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }
                if (GroupingDate != "")
                    HshIn.Add("@chrGroupingDate", GroupingDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDailyInjections", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null; objDl = null;
            }
        }

        public DataSet GetInvestigationsResult(int HospitalLocationID, int FacilityId, int EncounterID, string FromDate, string ToDate)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@inEncId", EncounterID);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetInvestigationsResult", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetVisitNotesPharmacy(int HospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@IntHospitalLocId", HospitalLocationId);
                HshIn.Add("@IntFacilityId", FacilityId);
                HshIn.Add("@IntRegistrationid", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);


                if (FromDate != "" && ToDate != "")
                {
                    HshIn.Add("@Fdate", FromDate);
                    HshIn.Add("@Tdate", ToDate);
                }


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEmrGetCombinedPrescription", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDoctorSpecialisation(int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", DoctorId);
                string qry = "SELECT DISTINCT dd.SpecialisationId, sm.Name as SpecialisationName, sm.Code as SpecialisationCode " +
                    " FROM Employee emp WITH (NOLOCK) " +
                    " INNER JOIN DoctorDetails dd WITH (NOLOCK) ON emp.ID = dd.DoctorId " +
                    " INNER JOIN SpecialisationMaster sm WITH (NOLOCK) ON dd.SpecialisationId = sm.ID " +
                    " WHERE emp.ID = @intDoctorId " +
                    " AND emp.Active = 1";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getPatientEncounterId(string EncounterNo, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intFacilityId", FacilityId);
                string qry = "SELECT Id AS EncounterId FROM Encounter WITH (NOLOCK) " +
                        " WHERE EncounterNo = @chvEncounterNo AND FacilityId = @intFacilityId AND Active = 1 ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientDiagnosisOPIP(int HospitalLocationId, int FacilityId, int RegistrationId, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosisOPIP", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet CheckGenerateeOPRxXml(int IndentId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                return objDl.FillDataSet(CommandType.Text, "Exec UspCheckGenerateeOPRxXml " + FacilityId + "," + IndentId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetMedicationRouteMaster()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                return objDl.FillDataSet(CommandType.Text, "SELECT ID, RouteName, IsDefault FROM EMRMedicationRouteMaster WITH (NOLOCK) WHERE Active=1 ORDER BY RouteName");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet SaveMedicineOrderIP(int HospId, int FacilityId, int RegistrationId, int EncounterId, int IndentType, int iIndentStoreId, int AdvisingDoctorId,
            string xmlItems, string xmlItemDetail, string Remarks, int DrugOrderType, int EncodedBy, bool IsConsumable, string xmlFrequencyTime, string xmlUnApprovedPrescriptionIds)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            DataSet ds = new DataSet();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intIndentType", IndentType);
            HshIn.Add("@intStoreId", iIndentStoreId);
            HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            HshIn.Add("@xmlItems", xmlItems);
            HshIn.Add("@xmlItemDetail", xmlItemDetail);
            HshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            HshIn.Add("@chvRemarks", Remarks);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@bitIsConsumable", IsConsumable);
            HshIn.Add("@intDrugOrderType", DrugOrderType);

            if (xmlUnApprovedPrescriptionIds != string.Empty)
            {
                HshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
            }

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut.Add("@intPrescriptionId", SqlDbType.VarChar);
            try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicine", HshIn, hshOut);


                DataTable dt = new DataTable();

                DataColumn dC;
                dC = new DataColumn("chvErrorStatus", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("intPrescriptionId", typeof(int));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);


                DataRow dr = dt.NewRow();
                if (Convert.ToString(hshOut["@chvErrorStatus"]).Equals(""))
                {
                    dr["chvErrorStatus"] = "";
                }
                else
                {
                    dr["chvErrorStatus"] = hshOut["@chvErrorStatus"].ToString();
                }
                if (Convert.ToString(hshOut["@intPrescriptionId"]).Equals(""))
                {
                    dr["intPrescriptionId"] = 0;
                }
                else
                {
                    dr["intPrescriptionId"] = Convert.ToInt32(hshOut["@intPrescriptionId"]);
                }

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                ds.Tables.Add(dt);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet SaveMedicineOrderOP(int HospId, int FacilityId, int RegistrationId,
                                int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                                int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy,
                                string xmlFrequencyTime, bool IsConsumable, string xmlUnApprovedPrescriptionIds, int DrugAdminIn)
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
                HshIn.Add("@intDrugAdminIn", DrugAdminIn);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
                HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOP", HshIn, HshOut);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataColumn dC;
                dC = new DataColumn("chvErrorStatus", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("intPrescriptionId", typeof(int));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("chvPrescriptionNo", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);


                DataRow dr = dt.NewRow();
                if (Convert.ToString(HshOut["@chvErrorStatus"]).Equals(""))
                {
                    dr["chvErrorStatus"] = "";
                }
                else
                {
                    dr["chvErrorStatus"] = HshOut["@chvErrorStatus"].ToString();
                }
                if (Convert.ToString(HshOut["@intPrescriptionId"]).Equals(""))
                {
                    dr["intPrescriptionId"] = 0;
                }
                else
                {
                    dr["intPrescriptionId"] = Convert.ToInt32(HshOut["@intPrescriptionId"]);
                }
                if (Convert.ToString(HshOut["@chvPrescriptionNo"]).Equals(""))
                {
                    dr["chvPrescriptionNo"] = 0;
                }
                else
                {
                    dr["chvPrescriptionNo"] = HshOut["@chvPrescriptionNo"].ToString();
                }

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                ds.Tables.Add(dt);
                ds.AcceptChanges();
                return ds;
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

        }

        public DataSet SaveMedicineOrderOP(int HospId, int FacilityId, int RegistrationId,
                              int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                              int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy,
                              string xmlFrequencyTime, bool IsConsumable, string xmlUnApprovedPrescriptionIds, int DrugAdminIn, string strPrescriptionDetail)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            SqlConnection con = new SqlConnection(sConString);

            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "uspSaveEMRMedicineOP";

            cmd.Parameters.Add("@intEncodedBy", SqlDbType.Int).Value = EncodedBy;

            cmd.Parameters.Add("@inyHospitalLocationId", SqlDbType.Int).Value = HospId;
            cmd.Parameters.Add("@intFacilityId", SqlDbType.Int).Value = FacilityId;
            cmd.Parameters.Add("@intRegistrationId", SqlDbType.Int).Value = RegistrationId;
            cmd.Parameters.Add("@intEncounterId", SqlDbType.Int).Value = EncounterId;
            cmd.Parameters.Add("@intIndentType", SqlDbType.Int).Value = IndentType;
            cmd.Parameters.Add("@intAdvisingDoctorId", SqlDbType.Int).Value = AdvisingDoctorId;
            cmd.Parameters.Add("@bitIsPregnant", SqlDbType.Bit).Value = IsPregnant;
            cmd.Parameters.Add("@bitIsBreastFeeding", SqlDbType.Bit).Value = IsBreastFeeding;
            cmd.Parameters.Add("@xmlItems", SqlDbType.Xml).Value = xmlItems;
            //cmd.Parameters.Add("@test", 'N' + strPrescriptionDetail);
            cmd.Parameters.Add("@xmlItemDetail", SqlDbType.Xml).Value = xmlItemDetail;
            cmd.Parameters.Add("@xmlPatientAlerts", SqlDbType.Xml).Value = xmlPatientAlerts;
            cmd.Parameters.Add("@xmlFrequencyTime", SqlDbType.Xml).Value = xmlFrequencyTime;
            //  cmd.Parameters.Add("@intEncodedBy", EncodedBy);
            cmd.Parameters.Add("@intStoreId", SqlDbType.Int).Value = StoreId;
            cmd.Parameters.Add("@bitIsConsumable", SqlDbType.Bit).Value = IsConsumable;

            if (xmlUnApprovedPrescriptionIds != string.Empty)
            {
                cmd.Parameters.Add("@xmlUnApprovedPrescriptionIds", SqlDbType.Xml).Value = xmlUnApprovedPrescriptionIds;
            }

            //HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            //HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
            //HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);
            cmd.Parameters.Add("@chvPrescriptionNo", SqlDbType.VarChar, 50);
            cmd.Parameters["@chvPrescriptionNo"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@chvErrorStatus", SqlDbType.VarChar, 500);
            cmd.Parameters["@chvErrorStatus"].Direction = ParameterDirection.Output;
            cmd.Parameters.Add("@intPrescriptionId", SqlDbType.Int);
            cmd.Parameters["@intPrescriptionId"].Direction = ParameterDirection.Output;


            //SqlParameter outputPara = new SqlParameter();
            //outputPara.ParameterName = "@chvPrescriptionNo";
            //outputPara.Direction = System.Data.ParameterDirection.Output;
            //outputPara.SqlDbType = System.Data.SqlDbType.VarChar;

            //SqlParameter outputPara1 = new SqlParameter();
            //outputPara1.ParameterName = "@chvErrorStatus";
            //outputPara1.Direction = System.Data.ParameterDirection.Output;
            //outputPara1.SqlDbType = System.Data.SqlDbType.VarChar;
            //SqlParameter outputPara2 = new SqlParameter();
            //outputPara2.ParameterName = "@intPrescriptionId";
            //outputPara2.Direction = System.Data.ParameterDirection.Output;
            //outputPara2.SqlDbType = System.Data.SqlDbType.Int;          


            //cmd.Parameters.Add(outputPara);
            //outputPara1.Size = 200;
            //cmd.Parameters.Add(outputPara1);
            //outputPara2.Size = 100;
            //cmd.Parameters.Add(outputPara2);
            cmd.Connection = con;

            try

            {

                con.Open();

                cmd.ExecuteNonQuery();

                // lblMessage.Text = "Record inserted successfully";

            }

            catch (Exception ex)

            {

                throw ex;

            }

            finally

            {

                con.Close();

                con.Dispose();

            }
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //BaseC.ParseData bc = new BaseC.ParseData();
            //try
            //{
            //    HshIn.Add("@inyHospitalLocationId", HospId);
            //    HshIn.Add("@intFacilityId", FacilityId);
            //    HshIn.Add("@intRegistrationId", RegistrationId);
            //    HshIn.Add("@intEncounterId", EncounterId);
            //    HshIn.Add("@intIndentType", IndentType);
            //    HshIn.Add("@intAdvisingDoctorId", AdvisingDoctorId);
            //    HshIn.Add("@bitIsPregnant", IsPregnant);
            //    HshIn.Add("@bitIsBreastFeeding", IsBreastFeeding);
            //    HshIn.Add("@xmlItems", xmlItems);
            //    HshIn.Add("@test", 'N'+ strPrescriptionDetail);
            //    HshIn.Add("@xmlItemDetail", xmlItemDetail);
            //    HshIn.Add("@xmlPatientAlerts", xmlPatientAlerts);
            //    HshIn.Add("@xmlFrequencyTime", xmlFrequencyTime);
            //    HshIn.Add("@intEncodedBy", EncodedBy);
            //    HshIn.Add("@intStoreId", StoreId);
            //    HshIn.Add("@bitIsConsumable", IsConsumable);

            //    if (xmlUnApprovedPrescriptionIds != string.Empty)
            //    {
            //        HshIn.Add("@xmlUnApprovedPrescriptionIds", xmlUnApprovedPrescriptionIds);
            //    }

            //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            //    HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
            //    HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);

            //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOP", HshIn, HshOut);



            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataColumn dC;
            dC = new DataColumn("chvErrorStatus", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);
            dC = new DataColumn("intPrescriptionId", typeof(int));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);
            dC = new DataColumn("chvPrescriptionNo", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);


            DataRow dr = dt.NewRow();
            if (Convert.ToString(cmd.Parameters["@chvErrorStatus"].Value).Equals(""))
            {
                dr["chvErrorStatus"] = "";
            }
            else
            {
                dr["chvErrorStatus"] = cmd.Parameters["@chvErrorStatus"].Value;
            }
            if (Convert.ToString(cmd.Parameters["@intPrescriptionId"].Value).Equals(""))
            {
                dr["intPrescriptionId"] = 0;
            }
            else
            {
                dr["intPrescriptionId"] = Convert.ToInt32(cmd.Parameters["@intPrescriptionId"].Value);
            }
            if (Convert.ToString(cmd.Parameters["@chvPrescriptionNo"].Value).Equals(""))
            {
                dr["chvPrescriptionNo"] = 0;
            }
            else
            {
                dr["chvPrescriptionNo"] = cmd.Parameters["@chvPrescriptionNo"].Value;
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            return ds;
            //}
            //catch (Exception Ex)
            //{
            //    throw Ex;
            //}
            //finally
            //{
            //    HshIn = null;
            //    objDl = null;
            //    bc = null;
            //}

        }

        public DataSet SaveMedicineOrderOPV3(int HospId, int FacilityId, int RegistrationId,
                               int EncounterId, int IndentType, int StoreId, int AdvisingDoctorId, int IsPregnant,
                               int IsBreastFeeding, string xmlItems, string xmlItemDetail, string xmlPatientAlerts, int EncodedBy,
                               string xmlFrequencyTime, bool IsConsumable, string xmlUnApprovedPrescriptionIds, int DrugAdminIn)
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
                HshIn.Add("@intDrugAdminIn", DrugAdminIn);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvPrescriptionNo", SqlDbType.VarChar);
                HshOut.Add("@intPrescriptionId", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRMedicineOPV3", HshIn, HshOut);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                DataColumn dC;
                dC = new DataColumn("chvErrorStatus", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("intPrescriptionId", typeof(int));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);
                dC = new DataColumn("chvPrescriptionNo", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);


                DataRow dr = dt.NewRow();
                if (Convert.ToString(HshOut["@chvErrorStatus"]).Equals(""))
                {
                    dr["chvErrorStatus"] = "";
                }
                else
                {
                    dr["chvErrorStatus"] = HshOut["@chvErrorStatus"].ToString();
                }
                if (Convert.ToString(HshOut["@intPrescriptionId"]).Equals(""))
                {
                    dr["intPrescriptionId"] = 0;
                }
                else
                {
                    dr["intPrescriptionId"] = Convert.ToInt32(HshOut["@intPrescriptionId"]);
                }
                if (Convert.ToString(HshOut["@chvPrescriptionNo"]).Equals(""))
                {
                    dr["chvPrescriptionNo"] = 0;
                }
                else
                {
                    dr["chvPrescriptionNo"] = HshOut["@chvPrescriptionNo"].ToString();
                }

                dt.Rows.Add(dr);
                dt.AcceptChanges();
                ds.Tables.Add(dt);
                ds.AcceptChanges();
                return ds;
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

        }
        public int IsDOctorEprescriptionEnabled(int DoctorId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "selecT convert(int,isnull(EprescriptionEnabled,0))  From doctorDetails with(nolock) where DoctorID=" + DoctorId));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null; HshIn = null;
            }
        }
        public void UpdateDHARefNo(string DHARefNo, int IndentId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                objDl.ExecuteScalar(CommandType.Text, "Exec uspUpdateDHARefNo " + DHARefNo + ", " + IndentId.ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null; objDl = null;
            }
        }
        public DataSet GetClinicianLoginforErx(int DoctorId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "Exec uspGetClinicianLoginforErx " + DoctorId);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null; objDl = null;

            }
        }
        public DataSet GetItemConversionFactor(int ItemId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string strupdate = "";
            Hashtable hstInput = new Hashtable();
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                hstInput.Add("@ItemId", ItemId);

                strupdate = " SELECT t1.ItemId, t3.ConversionFactor1, t3.ConversionFactor2 " +
                            " FROM PhrItemMaster t1 WITH(NOLOCK) " +
                            " INNER JOIN PhrItemWithItemUnitTagging t2 WITH(NOLOCK) ON t1.ItemId = t2.ItemId AND t2.Active = 1 AND t1.Active = 1 " +
                            " INNER JOIN PhrItemUnitMaster t3 WITH(NOLOCK) ON t2.ItemUnitId = t3.ItemUnitId AND t3.Active = 1 " +
                            " WHERE t1.ItemId = @ItemId";

                return objDl.FillDataSet(CommandType.Text, strupdate, hstInput);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet SaveUnApprovedPrescriptions(int UnAppPrescriptionId, int HospId, int FacilityId, int EncounterId, int IndentId, string IndentNo,
                        string IndentDate, int IndentTypeId, string IndentType, int GenericId, string GenericName, int ItemId, string ItemName,
                        bool CustomMedication, int FrequencyId, string FrequencyName, double Frequency, double Dose, string Duration,
                        string DurationText, string Instructions, int UnitId, string UnitName, string cType, string StartDate, string EndDate,
                        string CIMSItemId, string CIMSType, int VIDALItemId, string XMLData, string PrescriptionDetail, int FormulationId,
                        string FormulationName, int RouteId, string RouteName, int StrengthId, string StrengthValue, double Qty, string FoodRelationship,
                        int FoodRelationshipID, int ReferanceItemId, string ReferanceItemName, int DoseTypeId, string DoseTypeName, bool NotToPharmacy,
                        bool IsInfusion, bool IsInjection, bool IsStop, bool IsCancel, string Volume, int VolumeUnitId, string TotalVolume,
                        string InfusionTime, string TimeUnit, int FlowRate, int FlowRateUnit, string VolumeUnit, string XmlVariableDose,
                        bool IsOverride, string OverrideComments, string OverrideCommentsDrugToDrug, string OverrideCommentsDrugHealth,
                        string XMLFrequencyTime, bool IsSubstituteNotAllowed, string ICDCode, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intUnAppPrescriptionId", UnAppPrescriptionId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@dtIndentDate", IndentDate);
                HshIn.Add("@inyIndentTypeId", IndentTypeId);
                HshIn.Add("@chrIndentType", IndentType);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@chvGenericName", GenericName);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvItemName", ItemName);
                HshIn.Add("@bitCustomMedication", CustomMedication);
                HshIn.Add("@inyFrequencyId", FrequencyId);
                HshIn.Add("@chvFrequencyName", FrequencyName);
                HshIn.Add("@decFrequency", Frequency);
                HshIn.Add("@decDose", Dose);
                HshIn.Add("@chvDuration", Duration);
                HshIn.Add("@chvDurationText", DurationText);
                HshIn.Add("@chvInstructions", Instructions);
                HshIn.Add("@inyUnitId", UnitId);
                HshIn.Add("@chvUnitName", UnitName);
                HshIn.Add("@chrType", cType);
                HshIn.Add("@dtStartDate", StartDate);
                HshIn.Add("@dtEndDate", EndDate);
                HshIn.Add("@chvCIMSItemId", CIMSItemId);
                HshIn.Add("@chvCIMSType", CIMSType);
                HshIn.Add("@intVIDALItemId", VIDALItemId);
                HshIn.Add("@XMLData", XMLData);
                HshIn.Add("@chvPrescriptionDetail", PrescriptionDetail);
                HshIn.Add("@intFormulationId", FormulationId);
                HshIn.Add("@chvFormulationName", FormulationName);
                HshIn.Add("@intRouteId", RouteId);
                HshIn.Add("@chvRouteName", RouteName);
                HshIn.Add("@intStrengthId", StrengthId);
                HshIn.Add("@chvStrengthValue", StrengthValue);
                HshIn.Add("@decQty", Qty);
                HshIn.Add("@chvFoodRelationship", FoodRelationship);
                HshIn.Add("@intFoodRelationshipID", FoodRelationshipID);
                HshIn.Add("@intReferanceItemId", ReferanceItemId);
                HshIn.Add("@chvReferanceItemName", ReferanceItemName);
                HshIn.Add("@intDoseTypeId", DoseTypeId);
                HshIn.Add("@chvDoseTypeName", DoseTypeName);
                HshIn.Add("@bitNotToPharmacy", NotToPharmacy);
                HshIn.Add("@bitIsInfusion", IsInfusion);
                HshIn.Add("@bitIsInjection", IsInjection);
                HshIn.Add("@bitIsStop", IsStop);
                HshIn.Add("@bitIsCancel", IsCancel);
                HshIn.Add("@chvVolume", Volume);
                HshIn.Add("@inyVolumeUnitId", VolumeUnitId);
                HshIn.Add("@chvTotalVolume", TotalVolume);
                HshIn.Add("@chvInfusionTime", InfusionTime);
                HshIn.Add("@chrTimeUnit", TimeUnit);
                HshIn.Add("@intFlowRate", FlowRate);
                HshIn.Add("@intFlowRateUnit", FlowRateUnit);
                HshIn.Add("@chvVolumeUnit", VolumeUnit);
                HshIn.Add("@XmlVariableDose", XmlVariableDose);
                HshIn.Add("@bitIsOverride", IsOverride);
                HshIn.Add("@chvOverrideComments", OverrideComments);
                HshIn.Add("@chvOverrideCommentsDrugToDrug", OverrideCommentsDrugToDrug);
                HshIn.Add("@chvOverrideCommentsDrugHealth", OverrideCommentsDrugHealth);
                HshIn.Add("@XMLFrequencyTime", XMLFrequencyTime);
                HshIn.Add("@bitIsSubstituteNotAllowed", IsSubstituteNotAllowed);
                HshIn.Add("@chvICDCode", ICDCode);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@intReturnUnAppPrescriptionId", SqlDbType.VarChar);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRUnApprovedPrescriptions", HshIn, HshOut);


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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataColumn dC;
            dC = new DataColumn("chvErrorStatus", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);
            dC = new DataColumn("intReturnUnAppPrescriptionId", typeof(int));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);


            DataRow dr = dt.NewRow();
            if (Convert.ToString(HshOut["@chvErrorStatus"]).Equals(""))
            {
                dr["chvErrorStatus"] = "";
            }
            else
            {
                dr["chvErrorStatus"] = HshOut["@chvErrorStatus"].ToString();
            }
            if (Convert.ToString(HshOut["@intReturnUnAppPrescriptionId"]).Equals(""))
            {
                dr["intReturnUnAppPrescriptionId"] = 0;
            }
            else
            {
                dr["intReturnUnAppPrescriptionId"] = Convert.ToInt32(HshOut["@intReturnUnAppPrescriptionId"]);
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            return ds;
        }
        public DataSet StopCancelPreviousMedication(int iHospitalLocationId, int iFacilityId, int iIndentId, int iItemId, int UserId,
                                                      int iRegistrationId, int iEncounterId, int bCancelStop, string sStopRemarks,
                                                      string OPIP, int IndentDetailsId, int GenericId, int StopBy)
        {

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intIndentId", iIndentId);
                HshIn.Add("@intItemId", iItemId);
                HshIn.Add("@bitCancelStop", bCancelStop);
                HshIn.Add("@chvStopRemarks", sStopRemarks);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intIndentDetailsId", IndentDetailsId);
                HshIn.Add("@StopBy", StopBy);
                HshIn.Add("@intGenericId", GenericId);
                HshOut.Add("@chvOutPut", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspStopCancelPatientMedication", HshIn, HshOut);
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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataColumn dC;
            dC = new DataColumn("chvOutPut", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);



            DataRow dr = dt.NewRow();
            if (Convert.ToString(HshOut["@chvOutPut"]).Equals(""))
            {
                dr["chvOutPut"] = "";
            }
            else
            {
                dr["chvOutPut"] = HshOut["@chvOutPut"].ToString();
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            return ds;
        }
        public DataSet GenerateeOPRxXml(int HospitalLocationId, int FacilityId, int IndentId, string DispositionFlag)
        {

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@iHospitalLocationId", HospitalLocationId);
                HshIn.Add("@iFacilityId", FacilityId);
                HshIn.Add("@PrescriptionID", IndentId);
                HshIn.Add("@DispositionFlag", DispositionFlag);
                HshOut.Add("@returnXML", SqlDbType.Xml);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGenerateeOPRxXml", HshIn, HshOut);
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
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            DataColumn dC;
            dC = new DataColumn("returnXML", typeof(string));
            dC.AllowDBNull = true;
            dt.Columns.Add(dC);



            DataRow dr = dt.NewRow();
            if (Convert.ToString(HshOut["@returnXML"]).Equals(""))
            {
                dr["returnXML"] = "";
            }
            else
            {
                dr["returnXML"] = HshOut["@returnXML"].ToString();
            }

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            return ds;
        }
        public DataSet GetPatientDocuments(int ImageCategoryId, int RegistrationId, string FolderName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string strSQL = "";

            strSQL = "select pd.Id, case Type when '.doc' then '/Images/WordThumbnail.png' when '.docx' then '/Images/WordThumbnail.png'";
            strSQL += " when '.xls' then '/Images/Excel.png' when '.xlsx' then '/Images/XLSX.png' when '.pdf' then '/Images/pdf.jpg' when '.zip' then 'winzip-icon.jpg'";
            strSQL += " else ISNULL(NULLIF(replace(ImagePath,'E:\\AlrafaWeb\\Project\\PatientDocuments\','" + FolderName + "'),''),ImagePath), end as ImagePath, ";
            strSQL += " ImageName,CompressedFileName, isnull(Description, '') as Description, Isnull(Remarks, '') as Remarks , ";
            strSQL += " isnull(pd.Thumbnail, pd.ImagePath) as Data, ROW_NUMBER() over( order by pd.id ) as slno, Type from EMRPatientDocuments pd WITH (NOLOCK) ";
            strSQL += " where 1 = 1 ";
            if (ImageCategoryId > 0)
                strSQL += " AND pd.Id = " + ImageCategoryId;
            if (RegistrationId > 0)
                strSQL += " AND RegistrationID = " + RegistrationId;
            strSQL += " AND Active=1 ";

            // Change by kuldeep kumar 22/01/2021 
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally {objDl = null; }


        }
        public DataSet GetEMRDocumentCategory(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                string strSQL = "";
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                strSQL = "SELECT id, Description FROM EMRDocumentCategory WITH (NOLOCK) where Active = 1 and (HospitalLocationId=@HospitalLocationId or HospitalLocationId is null) order by Description ";

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFileServersetup(int FacilityID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                string strSQL = "";
                HshIn.Add("@FacilityID", FacilityID);
                strSQL = "  ";

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientRegistrationId(int RegistrationNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@RegistrationNo", RegistrationNo);

                string strsql = "select Id from Registration WITH (NOLOCK) Where RegistrationNo=@RegistrationNo";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string DeletePatientDocuments(int ImageCategoryId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@ImageCategoryId", ImageCategoryId);

                string strsql = "update EMRPatientDocuments set Active=0 where Id=@ImageCategoryId";
                objDl.FillDataSet(CommandType.Text, strsql, HshIn);
                return "Document deleted successfull";
            }
            catch (Exception Ex)
            {
                throw Ex;
            }


            finally { HshIn = null; objDl = null; }

        }
        public string SavePatientDocuments(int RegistrationId, int EncounterId, string ThumbnailName, string xmlDocumentDetails, string ExtentionType, int UserId, int DocumentTypeId, string DocumentType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegistrationId);
                if (EncounterId > 0)
                    HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvThumbnail", ThumbnailName);
                HshIn.Add("@xmlDocumentDetails", xmlDocumentDetails);
                HshIn.Add("@chvDocumentType", ExtentionType);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intDocumentTypeId", DocumentTypeId);
                HshIn.Add("@chvDocType", DocumentType);

                string strsql = "update EMRPatientDocuments set Active=0 where Id=@ImageCategoryId";
                objDl.FillDataSet(CommandType.Text, strsql, HshIn);
                return "Document Saved successfull";

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReferralDetailForSerach(int UserId, int EncId, String EncounterType, int FacilityId, string RegistrationNo,
            int ConcludeReferral, int ReferralId, string PatientName, int ReferralType, string FromDate, string ToDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HshInInput = new Hashtable();

                HshInInput.Add("@intUserId", UserId);
                HshInInput.Add("@intEncounterId", EncId);
                HshInInput.Add("@chvEncounterType", EncounterType);
                HshInInput.Add("@intFacilityId", FacilityId);
                HshInInput.Add("@chvRegistrationNo", RegistrationNo);
                HshInInput.Add("@inyConcludeReferral", ConcludeReferral);
                HshInInput.Add("@intReferralId", ReferralId);
                HshInInput.Add("@dtFromDate", FromDate);
                HshInInput.Add("@dtToDate", ToDate);
                HshInInput.Add("@inyReferralType", ReferralType);
                HshInInput.Add("@chvPatientName", PatientName);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferral", HshInInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet FillDatasetQuery(string strsql)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }
        public string SaveDiagnosisStaus(int StatusId, int HospitalLocationId, string StatusName, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intStatusId", StatusId);
                HshIn.Add("@intHospitallocationId", HospitalLocationId);
                HshIn.Add("@chvStatusName", StatusName);
                HshIn.Add("@intEncodedBy", UserId);
                objDl.FillDataSet(CommandType.StoredProcedure, "UspSaveDiagnosisStaus", HshIn);
                return "Document Saved successfull";
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetFavoriteSentences(int DoctorId, string SearchCriteria)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvSearchCriteria", SearchCriteria);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavoriteSentences", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }


        }
        public string DeleteFavSentence(int DoctorId, int SentenceId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intSentenceId", SentenceId);
                HshIn.Add("@intEncodedBy", UserId);
                objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRDeleteFavSentence", HshIn);
                return "Document Saved successfull";
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }
        public string SaveFavSentence(int DoctorId, string SentenceIds, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@xmlIds", SentenceIds);
                HshIn.Add("@intEncodedBy", UserId);
                objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRSaveFavSentence", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return "Document Saved successfull";
        }
        public string stopPrescription(string DetailsIds, int CancelReasonId, int EncodedBy, int EncounterId, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@intCancelReasonId", CancelReasonId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvDetailId", DetailsIds);
                HshIn.Add("@OPIP", OPIP);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                //string strqry = "UPDATE ICMIndentDetails SET Active = 0, CancelReasonId = @intCancelReasonId,"+
                // " LastchangeBy = @intEncodedBy, LastChangedate = GETUTCDATE() " +
                // " WHERE Id in (" + DetailsIds + ")";

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspStopMedicationOPIP", HshIn, HshOut);

                return HshOut["@chvErrorOutput"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTemplateGroup(int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFavouriteTemplates(int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", DoctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteTemplates", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetTemplateGroupDetails(int GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intGroupId", GroupId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTemplateGroupDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int SaveFavouriteTemplates(int DoctorId, int TemplateId, int EncodedBy, string TranType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intTemplateID", TemplateId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chrTranType", TranType);

                int i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveFavouriteTemplates", HshIn);
                return i;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveReferralSlip(int ReferralReplyId, int ReferralId, int HospitalId, int EncounterId, DateTime Referraldate, int ReferDoctorId, string Doctornote,
    int Urgent, bool status, int Encodedby, string SaveReferralSlip, int Active, string DoctorReferral, int SpecializationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
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
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReferralDetail(int UserId, int EncId, String EncounterType, int FacilityId, string RegistrationNo, int ConcludeReferral, int ReferralId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {               

                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intEncounterId", EncId);
                HshIn.Add("@chvEncounterType", EncounterType);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@inyConcludeReferral", ConcludeReferral);
                HshIn.Add("@intReferralId", ReferralId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDoctorReferral", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveReferralSlipAutoSave(int HospitalId, int FacilityId, int RegistrationId, int EncounterId, int SpecializationId, int ReferDoctorId, int Urgent, string Doctornote, int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {            


                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                if (SpecializationId != 0)
                {
                    HshIn.Add("@intSpecializationId", SpecializationId);
                }
                else
                {
                    HshIn.Add("@intSpecializationId", DBNull.Value);
                }

                if (ReferDoctorId != 0)
                {
                    HshIn.Add("@intReferToDoctorId", ReferDoctorId);
                }
                else
                {
                    HshIn.Add("@intReferToDoctorId", DBNull.Value);
                }


                HshIn.Add("@inyUrgent", Urgent);
                HshIn.Add("@chvNote", Doctornote);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveReferralInTransit", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetReferralDetailAutoSave(int RegistrationId, int FacilityId, int EncounterId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {


               
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetReferralInTransit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        // Start
        public DataSet Get_Rpt_MRD_CountryWiseVisit(int HospitalLocationID, int FacilityId, string NationalityIds, string FromDate, string ToDate, string VisitType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                Hashtable HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvNationalityIds", NationalityIds);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@VisitType", VisitType);

                return objDl.FillDataSet(CommandType.StoredProcedure, "usp_Rpt_MRD_CountryWiseVisit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet Get_Rpt_GetVisitSummary(int HospitalLocationID, int FacilityId, string FromDate, string ToDate, string VisitType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                Hashtable HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@VisitType", VisitType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "usp_Rpt_GetVisitSummary", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDashboardRecievable(int HospitalLocationID, int FacilityId, string FromDate, string ToDate, string sCompanyIds, string ReportType, string sYearids, int InvoiceId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                Hashtable HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvCompanyIds", sCompanyIds);
                HshIn.Add("@chvFromDate", FromDate);
                HshIn.Add("@chvToDate", ToDate);
                HshIn.Add("@chvReportType", ReportType);
                HshIn.Add("@chvYearId", sYearids);
                HshIn.Add("@intInvoiceId", InvoiceId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDashboardRecievable", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetYears()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "Select Id, Year from Years Where Active = 1");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDashboardRecievableDetails(int HospitalLocationID, int FacilityId, string FromDate, string ToDate, string ReportType, int CompanyId, int SubCompanyId, string YearMonth, string sCompanyIds, string syear)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            try
            {



              
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@intSubCompanyId", SubCompanyId);
                HshIn.Add("@chvFromDate", FromDate);
                HshIn.Add("@chvToDate", ToDate);
                HshIn.Add("@chvReportType", ReportType);
                HshIn.Add("@chvYearMonth", YearMonth);
                HshIn.Add("@chvCompanyIds", sCompanyIds);
                HshIn.Add("@chvYear", syear);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDashboardRecievableDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDischargeChecklistMaster(int FacilityId, int RegistrationId, int EncounterId, int StatusId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

               

                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounter", EncounterId);
                HshIn.Add("@intStatusId", StatusId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDischargeChecklistMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDischargeChecklistDetail(int EncounterId, int CheckListId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


              

                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intChecklistId", CheckListId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDischargeChecklistDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string UpdateEncouterStatus(int EncounterId, int HospId, int FacilityId, int RegistrationId, int StatusId, int UserId, DateTime? EDod, int DischargeStatus, string xmlChecklist)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


               

                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intStatusId", StatusId);
                HshIn.Add("@intEncodedBy", UserId);
                if (EDod != null)
                {
                    HshIn.Add("@ExpectedDateOfDischarge", EDod);
                    HshIn.Add("@intDischarStatus", DischargeStatus);
                }
                if (xmlChecklist != "")
                {
                    HshIn.Add("@xmlCheckListFlag", xmlChecklist);
                }
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEncouterStatus", HshIn, hshOut);

                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetTopPrescriptionListBasedOnICDCodes(int EncounterId)
        {
            Hashtable HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTopPrescriptionListBasedOnICDCodes", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetTopOrderListBasedOnICDCodes(int EncounterId)
        {
            Hashtable HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTopOrderListBasedOnICDCodes", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetServiceDetail(int HospId, int intFacilityId, int intRegistrationId, int intEncounterId, int intServiceId,
           int intorderSetId, int intCompanyid, int intSponsorId, int intCardId, int Option, int intTemplateId, string xmlServiceIds)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intFacilityId", intFacilityId);
                HshIn.Add("@intRegistrationId", intRegistrationId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@intServiceId", intServiceId);
                HshIn.Add("@intorderSetId", intorderSetId);
                HshIn.Add("@intCompanyid", intCompanyid);
                HshIn.Add("@intSponsorId", intSponsorId);
                HshIn.Add("@intCardId", intCardId);
                HshIn.Add("@Option", Option);
                HshIn.Add("@intTemplateId", intTemplateId);
                HshIn.Add("@xmlServiceIds", xmlServiceIds);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceDescriptionForOrderpage", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string DietUpdateOrder(int HospId, int FacilityId, int DietOrderId, string StatusCode, int UserId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intId", DietOrderId);
                HshIn.Add("@chrStatusCode", StatusCode);
                HshIn.Add("@intEncodedBy", UserId);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDietUpdateOrder", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetUserModule(int UserId, int GroupId)
        {
            Hashtable HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@intUserId", UserId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetUserModule", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetModuleUserPages(int ModuleId, int EncounterId, int GroupId, int FormId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
               
                HshIn.Add("@inyModuleID", ModuleId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intGroupId", GroupId);
                if (FormId > 0)
                    HshIn.Add("@intFormId", FormId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModuleUserPages", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet ShowPageCheck(int RegistrationId, int EncounterId, int GroupId, int FormId, string EREncounterId, int TemplateId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@FormId", FormId);
                HshIn.Add("@intEREnounterId", EREncounterId);
                HshIn.Add("@intTemplateId", TemplateId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRShowPageCheck", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetROS(int HospitalLocationID, int RegistrationNo, int EncounterId)
        {


            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", HospitalLocationID);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetROS", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet populateAllergyType(int iHospitalLocation)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospitalLocationId", iHospitalLocation);
                return objDl.FillDataSet(CommandType.Text, "select TypeID,TypeName from AllergyType where Active=1 and HospitalLocationId=@HospitalLocationId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetProblemHPIDetails(int ProblemId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intProblemId", ProblemId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetProblemHPIDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveHPIProblems(int HospitalLocationId, int ProblemId, string OnsetDate, string NoOfOccurrence, string PriorIllnessDate, string RelievingFactors, string AggravatingFactors,
        string DeniesSymptomsText1, string DeniesSymptomsText2, string DeniesSymptomsText3, string DeniesSymptomsText4, string DeniesSymptomsText5,
        int UserId, int RegistrationId, int EncounterId, int FacilityId, int PageId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intProblemID", ProblemId);
                if (OnsetDate != "")
                    HshIn.Add("@chvOnsetDate", OnsetDate);
                HshIn.Add("@inyNoOfOccurrence", NoOfOccurrence);
                if (PriorIllnessDate != "")
                    HshIn.Add("@chvPriorIllnessDate", PriorIllnessDate);
                HshIn.Add("@chvRelievingFactors", RelievingFactors);
                HshIn.Add("@chvAggravatingFactors", AggravatingFactors);
                if (DeniesSymptomsText1 != "")
                    HshIn.Add("@chvDeniesSymptomsText1", DeniesSymptomsText1);
                if (DeniesSymptomsText2 != "")
                    HshIn.Add("@chvDeniesSymptomsText2", DeniesSymptomsText2);
                if (DeniesSymptomsText3 != "")
                    HshIn.Add("@chvDeniesSymptomsText3", DeniesSymptomsText3);
                if (DeniesSymptomsText4 != "")
                    HshIn.Add("@chvDeniesSymptomsText4", DeniesSymptomsText4);
                if (DeniesSymptomsText5 != "")
                    HshIn.Add("@chvDeniesSymptomsText5", DeniesSymptomsText5);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveHPIProblems", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDocumentType(int HospitalLocationId, string DocumentTypeName)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@DocumentTypeName", DocumentTypeName);
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDocumentType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDocumentCategory(int HospitalLocationId, string DocumentCategoryName)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chvCasename", DocumentCategoryName);
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.Text, "select Description from EMRDocumentCategory where Description=@chvCasename and ( HospitalLocationId=@HospitalLocationId or HospitalLocationId is null) ", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveDocumentType(int HospitalLocationId, string DocumentTypeName, int EncodedBy)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chvDocumentType", DocumentTypeName);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDocumentsType", HshIn);
                return "Document Type Saved Successfully...";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveDocumentCategory(int HospitalLocationId, string DocumentCategoryName, int EncodedBy, bool Active, int DocumentCategoryId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@chvDescription", DocumentCategoryName);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitActive", Active);
                if (DocumentCategoryId > 0)
                {
                    HshIn.Add("@intDocumentId", DocumentCategoryId);
                }
                objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspSaveDocumentsCategory", HshIn);
                return "Document Category Saved Successfully...";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAttachmentCategory(int HospitalLocationId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetAttachmentCategory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetOrderDetailsfromEncounterID(int EncounterId, int FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@encounterID", EncounterId);
                HshIn.Add("@facilityID", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetOrderDetailsfromEncounterID", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetLabDetailsEmail(string strNo)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@chvLabNo", strNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetOrderDetailsfromEncounterID", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientAssessments(int HospitalLocationId, int RegistrationId, int EncounterId, string FromDate, string ToDate, string GroupingDate)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chrFromDate", FromDate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@chrGroupingDate", GroupingDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientDiagnosis", HshIn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
            }


        }
        public DataTable SaveProblemMasters(int HospitallocationId, string Description, int EncodedBy, string TableName, int Id)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitallocationId", HospitallocationId);
            HshIn.Add("@chvDescription", Description);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@chvTableName", TableName);
            HshIn.Add("@intId", Id);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMasters", HshIn, HshOut);

                DataTable dt = new DataTable();

                DataColumn dC;
                dC = new DataColumn("chvErrorStatus", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);

                DataRow dr = dt.NewRow();
                if (Convert.ToString(HshOut["@chvErrorStatus"]).Equals(""))
                {
                    dr["chvErrorStatus"] = "";
                }
                else
                {
                    dr["chvErrorStatus"] = HshOut["@chvErrorStatus"].ToString();
                }
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable SaveProblemMastersSeq(string xmlDetails, int EncodedBy, string TableName)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@xmlDetails", xmlDetails);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@chvTableName", TableName);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveProblemMastersSeq", HshIn, HshOut);

                DataTable dt = new DataTable();

                DataColumn dC;
                dC = new DataColumn("chvErrorStatus", typeof(string));
                dC.AllowDBNull = true;
                dt.Columns.Add(dC);

                DataRow dr = dt.NewRow();
                if (Convert.ToString(HshOut["@chvErrorStatus"]).Equals(""))
                {
                    dr["chvErrorStatus"] = "";
                }
                else
                {
                    dr["chvErrorStatus"] = HshOut["@chvErrorStatus"].ToString();
                }
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                return dt;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEMRTemplateReportSetup1(int ReportId, int TemplateId, int DoctorId, string chvFlag, bool bitActive, int HospitalLocationId, string ReportType, int UserID)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intReportId", ReportId);
                HshIn.Add("@intTemplateId", TemplateId);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@chvFlag", chvFlag);
                HshIn.Add("@bitActive", bitActive);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chvReportType", ReportType);// For Death summary
                HshIn.Add("@intEncodedBy", UserID);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateReportSetup1", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveUpdateICMPatientSummary(int iID, int iHospitalLocationID, int sRegistrationID, int sEncounterID, int iFormatID,
        string sPatietSummary, int iSignDoctorID, int iEncodedBy, string sEncodedDate, String DeathDate,
        String DischargeDate, int FacilityId, string synopsis, string Addendum,
        bool IsMultiDepartmentCase, int CaseId, string xmlDepartmentIds)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intID", iID);
                HshIn.Add("@intHospitalLocationID", iHospitalLocationID);
                HshIn.Add("@intRegistrationID", sRegistrationID);
                HshIn.Add("@intEncounterID", sEncounterID);
                HshIn.Add("@intFormatID", iFormatID);
                HshIn.Add("@chvPatientSummary", sPatietSummary);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@chrEncodedDate", sEncodedDate);
                HshIn.Add("@intLastUpdatedBy", iEncodedBy);
                HshIn.Add("@chrLastUpdatedDate", sEncodedDate);
                HshIn.Add("@intSignDoctorID", iSignDoctorID);
                HshIn.Add("@chrDischargeDate", DischargeDate);
                HshIn.Add("@chrDeathDate", DeathDate);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrsynopsis", synopsis);
                HshIn.Add("@chvAddendum", Addendum);

                if (IsMultiDepartmentCase)
                {
                    HshIn.Add("@bitIsMultiDepartmentCase", IsMultiDepartmentCase);
                    HshIn.Add("@intCaseId", CaseId);
                    HshIn.Add("@xmlDepartmentIds", xmlDepartmentIds);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateICMPatientSummary", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int CancelReferral(int FacilityId, int UserId, string sRemarks, int ReferralId, string Source)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intUserId", UserId);
            HshIn.Add("@chvReason", sRemarks);
            HshIn.Add("@intReferralId", ReferralId);
            HshIn.Add("@chvSource", Source);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspCancelReferral", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SetPatinetCheckIn(int HospitalLocationId, int FacilityId, int EncounterID, int EncodedBy, int flag)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@EncounterId", EncounterID);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@Flag", flag);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePatientCheckIn", HshIn, HshOut);
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
            return HshOut;
        }


        public DataSet getPatientJLabResultHistoryDash(int iHostId, int iLoginFacilityId, int FacilityId, int iProviderId)
        {

            //string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            //string tDate = toDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHostId);
                HshIn.Add("@intProviderId", iProviderId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                //HshIn.Add("@chvDateFrom", fDate);
                //HshIn.Add("@chvDateTo", tDate);
                //HshIn.Add("@intProviderId", iProviderId);
                //HshIn.Add("@inyPageSize", iPageSize);
                //HshIn.Add("@intPageNo", iPageNo);
                //HshIn.Add("@bitAbnormalResult", AbnormalResult);
                //HshIn.Add("@bitCriticalResult", CriticalResult);
                //HshIn.Add("@chvEncounterNo", chvEncounterNo);
                //HshIn.Add("@ReviewedStatus", ReviewedStatus);
                //HshIn.Add("@chvPatientName", PatientName);
                //HshIn.Add("@intUserId", iUserId);
                //HshIn.Add("@bitMinLabel", IsMinLabel);
                //HshIn.Add("@bitMaxLabel", IsMaxLabel);
                //HshIn.Add("@IsER", IsER);
                //if (iStatusId != 0)
                //{
                //    HshIn.Add("@intStatusId", iStatusId);
                //}
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetLabResultsForDashboard", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetSearchServices(int iHospitalID, int iDepartmentId, string sSubDeptId, string sSearchText, int FacilityId, int SecGroupId, int EmployeeId)
        {
            return GetSearchServices(iHospitalID, iDepartmentId, sSubDeptId, sSearchText, FacilityId, SecGroupId, EmployeeId, 0);
        }
        public DataTable GetSearchServices(int iHospitalID, int iDepartmentId, string sSubDeptId, string sSearchText, int FacilityId, int SecGroupId, int EmployeeId, int ServicesForWard)
        {

            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@chvSearchCriteria", sSearchText == "" ? "%%" : sSearchText);
            string storeProcedurnme = string.Empty;

            HshIn.Add("@inyHospitalLocationId", iHospitalID);
            HshIn.Add("@intDepartmentId", iDepartmentId);
            HshIn.Add("@chvSubDepartmentIds", sSubDeptId);

            HshIn.Add("@intExternalCenterId", null);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intServicesForWard", ServicesForWard);
            if (SecGroupId > 0)
            {
                HshIn.Add("@intSecGroupId", SecGroupId);
            }
            if (EmployeeId > 0)
            {
                HshIn.Add("@intEmployeeId", EmployeeId);
            }
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHospitalServicesforCareTemplate", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }

}
