using EMRAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace EMRAPI.Controllers
{
    public class CommonController : ApiController
    {

        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;

        [HttpPost]
        [Route("api/Common/getHospitalSetupValueMultiple")]
        public IHttpActionResult getHospitalSetupValueMultiple([FromBody] getHospitalSetupValueMultiple Model)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            try
            {


                ds = objcls.getHospitalSetupValueMultiple(Model.HospitalLocationId, Model.FacilityId, Model.Flag);

                if (ds != null)
                {
                    return Ok(JsonConvert.SerializeObject(ds, Formatting.Indented));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }


        [HttpPost]
        [Route("api/Common/getHospitalSetupValueAjax")]
        public IHttpActionResult getHospitalSetupValueAjax([FromBody] getHospitalSetupValueMultiple Model)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            try
            {
                 ds = objcls.getHospitalSetupValueSingle(Model.HospitalLocationId, Model.FacilityId,common.myStr(Model.Flag));

                if (ds != null)
                {
                    return Ok(ds.Tables[0].Rows[0]["value"]);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception Ex)
                {
                throw Ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }



        [HttpPost]
        [Route("api/Common/GfsQueryCountManagement")]
        public IHttpActionResult GfsQueryCountManagement([FromBody] GfsCountManagement Model)
        {

            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GfsQueryCountManagement(Model.Employeeid);
                return Ok(JsonConvert.SerializeObject(ds));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                objcls = null;
            }

        }

        [HttpPost]
        [Route("api/Common/GfsQuery")]
        public IHttpActionResult GfsQuery([FromBody] GfsCountManagement Model)
        {

            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GfsQuery(Model.Qid);
                return Ok(JsonConvert.SerializeObject(ds));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

                objcls = null;
            }

        }

        [HttpPost]
        [Route("api/Common/GetERtoken")]
        public IHttpActionResult GetERtoken([FromBody] ERtoken Model)
        {

            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string id = string.Empty;
            try
            {
                id = objcls.GetERtoken(Model.ErEncounterID);

                return Ok(JsonConvert.SerializeObject(id));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                id = string.Empty;
                objcls = null;
            }

        }

        [HttpPost]
        [Route("api/Common/CheckPatientProblem")]
        public int CheckPatientProblem([FromBody] PatientProblem Model)
        {

            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            int PRoblem;
            try
            {
                PRoblem = objcls.CheckPatientProblem(Model.EncounterID);

                return PRoblem;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                PRoblem = 0;
                objcls = null;
            }

        }
        [HttpPost]
        [Route("api/Common/getAllergyItemList")]
        public IHttpActionResult getAllergyItemList([FromBody] AllergyItemList Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getAllergyItemList(Model.HospitalLocationId, Model.FacilityId,
                                   Model.ItemName, Model.EncodedBy);

                return Ok(JsonConvert.SerializeObject(ds));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/CalculateVitalsValue")]
        public IHttpActionResult CalculateVitalsValue([FromBody] VitalsValue model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.CalculateVitalsValue(model.HospitalLocationID, model.xmlstr);

                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                    return Ok(json);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/Common/GetICMSignDoctors")]
        public IHttpActionResult GetICMSignDoctors([FromBody] GetICMSignDoctors Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetICMSignDoctors(Model.HospitalLocationID, Model.FacilityId);


                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/Common/ChangeEncounterDoctor")]
        public IHttpActionResult ChangeEncounterDoctor([FromBody] ChangeEncounterDoctor Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            objcls.ChangeEncounterDoctor(Model.EncounterId, Model.DoctorId, Model.UserId);
            string json = JsonConvert.SerializeObject("Saved", Formatting.Indented);
            return Ok(json);
        }

        [HttpPost]
        [Route("api/Common/GetEMRUserSetup")]
        public IHttpActionResult GetEMRUserSetup([FromBody] GetEMRUserSetup Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetEMRUserSetup(Model.HospitalLocationID, Model.EmployeeId, Model.EncodedBy);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/Common/getDoctorList")]
        public IHttpActionResult getDoctorList([FromBody] getDoctorList Model)
        {
            BaseC.clsLISMaster objcls = new BaseC.clsLISMaster(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getDoctorList(Model.DoctorId, Model.DoctorName, Model.HospitalLocationId, Model.SpecialisationId, Model.FacilityId, Model.EncodedBy, Model.IsMedicalProvider);

                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/getEmployeeId")]
        public IHttpActionResult getEmployeeId([FromBody] getEmployeeId Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            int outputresult = objcls.getEmployeeId(Model.UserId);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Common/GetStatusMaster")]
        public IHttpActionResult GetStatusMaster([FromBody] GetStatusMaster Model)
        {
            BaseC.clsBb objcls = new BaseC.clsBb(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetStatusMaster(Model.StatusType);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/getFacilityList")]
        public IHttpActionResult getFacilityList([FromBody] getFacilityList Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getFacilityList(Model.HospitalLocationId, Model.UserId, Model.GroupID, Model.EncodedBy, Model.FacilityType);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/DeleteAppointment")]
        public IHttpActionResult DeleteAppointment([FromBody] DeleteAppointment Model)
        {
            BaseC.Appointment objcls = new BaseC.Appointment(sConString);
            string outputresult = objcls.DeleteAppointment(Model.CancelRemarks, Model.AppointmentId, Model.UserId);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Common/GetGfsQueryDoc")]
        public IHttpActionResult GetGfsQueryDoc([FromBody] GfsCountManagement Model)
        {

            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetGfsQueryDoc(Model.Employeeid);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }


        [HttpPost]
        [Route("api/Common/GetGfsJQueryDoc")]
        public IHttpActionResult GetGfsJQueryDoc([FromBody] GfsCountManagement Model)
        {

            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetGfsQueryDoc(Model.Employeeid);
                List<GfsCountManagementOut> output = new List<GfsCountManagementOut>();

                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GfsCountManagementOut model = new GfsCountManagementOut();
                        model.Id = common.myInt( ds.Tables[0].Rows[i]["Id"].ToString());
                        model.UHID = common.myInt(ds.Tables[0].Rows[i]["UHID"].ToString());
                        model.Query = ds.Tables[0].Rows[i]["Query"].ToString();
                       
                        output.Add(model);
                    }
                }
                return Ok(output);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //ds.Dispose();
                objcls = null;
            }
        }



        [HttpPost]
        [Route("api/Common/GetDocumentList")]
        public IHttpActionResult GetDocumentList([FromBody] GetDocumentList Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDocumentList(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationNo);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetDoctorDetails")]
        public IHttpActionResult GetDoctorDetails([FromBody] GetDoctorDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDoctorDetails(Model.DoctorId, Model.FacilityId, Model.HospitalLocationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/AuditCommonAccess")]
        public IHttpActionResult AuditCommonAccess([FromBody] AuditCommonAccess Model)
        {
            BaseC.Security objcls = new BaseC.Security(sConString);
            try
            {
                objcls.AuditCommonAccess(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.PageId, Model.TemplateId,
                    Model.EncodedBy, Model.EmployeeId, Model.AuditStatus, Model.IPAddress);
                string json = JsonConvert.SerializeObject("Sucess", Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetPatientDetails")]
        public IHttpActionResult GetPatientDetails([FromBody] GetPatientDetails Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientDetails(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.RegistrationNo, Model.EncounterId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               // outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/Common/GetEmrPatientDetail")]
        public IHttpActionResult GetEmrPatientDetail([FromBody] GetPatientDetails Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEmrPatientDetail(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.RegistrationNo, Model.EncounterId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // outputresult.Dispose();
                objcls = null;
            }
        }


        [HttpPost]
        [Route("api/Common/SearchPatientByName")]
        public IHttpActionResult SearchPatientByName([FromBody] SearchPatientByName Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SearchPatientByName(Model.HospitalLocationId, Model.PatientName, Model.Status);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/Common/getFontAttribute")]
        public IHttpActionResult getFontAttribute([FromBody] getFontAttribute Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.GetFont(Model.AttributeType, Model.AttributeValue);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/HandleException")]
        public IHttpActionResult HandleException([FromBody] HandleException Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.HandleException(Model.HospitalLocationId, Model.Source, Model.ErrorMessage, Model.QueryString, Model.TargetSite,
                    Model.StackTrace, Model.ServerName, Model.RequestURL, Model.UserAgent, Model.UserIP, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetInsuranceQuery")]
        public IHttpActionResult GetInsuranceQuery([FromBody] GetInsuranceQuery Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetInsuranceQuery(Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetHospitalSubDepartment")]
        public IHttpActionResult GetHospitalSubDepartment([FromBody] GetHospitalSubDepartment Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetHospitalSubDepartment(Model.HospitalLocationId, Model.DepartmentId, Model.DepartmentType, Model.SubDepartmentId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetPatientEncounterDetails")]
        public IHttpActionResult getPatientEncounterDetails([FromBody] GetPatientEncounterDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientEncounterDetails(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        // UspSaveGfsQuery
        [HttpPost]
        [Route("api/Common/SaveGfsQuery")]
        public IHttpActionResult SaveGfsQuery([FromBody] GfsQueryInputs Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string json = objcls.SaveGfsQuery(Model.Qid, Model.formID, Model.Query, Model.QueryBy, Model.QueryTo);
            return Ok(json);
        }


        [HttpPost]
        [Route("api/Common/GetGender")]
        public IHttpActionResult GetGender([FromBody] GetGender Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetGender(Model.RegistrationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetFontDetails")]
        public IHttpActionResult GetFontDetails([FromBody] GetFontDetails Model)
        {
            BaseC.EMRMasters.Fonts objcls = new BaseC.EMRMasters.Fonts();
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFontDetails(Model.MasterType);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/getPatientEncounterId")]
        public IHttpActionResult getPatientEncounterId([FromBody] getPatientEncounterId Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientEncounterId(Model.EncounterNo, Model.FacilityId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/Common/GetCancelRemarks")]
        public IHttpActionResult GetCancelRemarks([FromBody] EMRModel Model)
        {
            DAL.DAL objcls = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.FillDataSet(CommandType.Text, Model.Query);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetRegistrationDetails")]
        public IHttpActionResult GetRegistrationDetails([FromBody] GetRegistrationDetails Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientDetails(Model.RegistrationId, Model.FacilityId, Model.PatientName, Model.PageNo, Model.PageNo);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/getRegistrationIDFromRegistrationNo")]
        public IHttpActionResult getRegistrationIDFromRegistrationNo([FromBody] getRegistrationIDFromRegistrationNo Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.getRegistrationIDFromRegistrationNo(Model.RegistrationNo, Model.HospitalLocationId, Model.FacilityId);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetPatientHistoryDetails")]
        public IHttpActionResult GetPatientHistoryDetails([FromBody] GetPatientHistoryDetails Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientHistoryDetails(Model.HospitalLocationId, Model.FacilityId, Model.DoctorId, Model.RegistrationId, Model.DateRange, Model.FromDate,
                Model.ToDate, Model.Source);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/Common/GetEncounterDoctor")]
        public IHttpActionResult GetEncounterDoctor([FromBody] GetEncounterDoctor Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEncounterDoctor(Model.EncounterId);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                outputresult.Dispose();
                objcls = null;
            }
        }
    }
}
