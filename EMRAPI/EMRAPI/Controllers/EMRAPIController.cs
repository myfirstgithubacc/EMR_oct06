using EMRAPI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;
using System.Data.SqlClient;

namespace EMRAPI.Controllers
{
    public class EMRAPIController : ApiController
    {
        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        Hashtable hshInput;
        DataSet ds;

        [HttpPost]
        [System.Web.Http.Route("api/EMRAPI/SaveSingleScreenDashboard")]
        public IHttpActionResult SaveSingleScreenDashboard([FromBody] SingleScreenDashboard Model)
        {

            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            Hashtable HshOut = new Hashtable();
            try
            {
                HshOut = objcls.SaveSingleScreenDashboard(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId,
                        Model.FacilityId, Model.DoctorId, Model.UserId, Model.xmlProblemDetails, Model.sProvisionalDiagnosis,
                        Model.xmlVitalString, Model.strXMLDrug, Model.strXMLOther, Model.ProvisionalDiagnosisId, Model.DiagnosisSearchId,
                        Model.bitNKDA, Model.xmlTemplateDetails, Model.iSign, Model.xmlNonDrugOrder,Model.IsProvisionalDignosis,Model.IsFinalDignosis);

                //return HshOut["@chvErrorStatus"].ToString();
                string json = JsonConvert.SerializeObject(HshOut["@chvErrorStatus"].ToString(), Formatting.Indented);
                return Ok(json);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshOut = null;
                objcls = null;
            }

        }

        //care plan save master
        [HttpPost]
        [System.Web.Http.Route("api/EMRAPI/SaveSingleScreenCarePlanMaster")]
        public IHttpActionResult SaveSingleScreenCarePlanMaster([FromBody] SingleScreenCarePlanDashboard Model)
        {

            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            Hashtable HshOut = new Hashtable();
            try
            {
                HshOut = objcls.SaveSingleScreenCareMaster(Model.PlanName, Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId,
                        Model.FacilityId,  Model.UserId, Model.xmlProblemDetails,  Model.strXMLDrug, 
                         Model.xmlTemplateDetails, Model.xmlOrderProcedure, Model.xmlOrderProcedure1,Model.xmlDiagnosis);

                //return HshOut["@chvErrorStatus"].ToString();
                string json = JsonConvert.SerializeObject(HshOut["@chvErrorStatus"].ToString(), Formatting.Indented);
                return Ok(json);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshOut = null;
                objcls = null;
            }

        }

        [HttpPost]
        [Route("api/EMRAPI/GetProvisionalDiagnosisSearchCodes")]
        public IHttpActionResult GetProvisionalDiagnosisSearchCodes([FromBody] GetProvisionalDiagnosisSearchCodes Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetProvisionalDiagnosisSearchCodes(Model.HospitalLocationId, Model.UserId, Model.KeywordType);

                //string JSONString = string.Empty;
                //JSONString =JsonConvert.SerializeObject(ds);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
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
        [System.Web.Http.Route("api/EMRAPI/DeletePatientProvisionalDiagnosis")]
        public IHttpActionResult DeletePatientProvisionalDiagnosis([FromBody] DeletePatientProvisionalDiagnosis Model)
        {

            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            Hashtable HshOut = new Hashtable();
            try
            {
                int outresult = objcls.DeletePatientProvisionalDiagnosis(Model.ProvisionalDiagnosisId, Model.UserId);
                return Ok(JsonConvert.SerializeObject(outresult));

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshOut = null;
                objcls = null;
            }

        }
        [HttpPost]
        [Route("api/EMRAPI/GetEMRDataForSingleScreen")]
        public IHttpActionResult GetEMRDataForSingleScreen([FromBody] GetEMRDataForSingleScreen Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetEMRDataForSingleScreen(Model.HospitalLocationId, Model.RegistrationId, Model.FacilityId, Model.EncounterId, Model.sTemplateType,
                    Model.iTemplateId, Model.sTemplateName,
                    Model.EncounterDate, Model.ToDate, Model.pageSize, Model.PageNo, Model.IsCopyLastOPDSummary, Model.DoctorId);

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
        [Route("api/EMRAPI/GetEvitalresult")]
        public IHttpActionResult GetEvitalresult([FromBody] GetEvitalresult Model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetEvitalresult(Model.EncounterID);
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
        [Route("api/EMRAPI/Canceltodayproblem")]
        public IHttpActionResult Canceltodayproblem([FromBody] Canceltodayproblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            objcls.Canceltodayproblem(Model.ProblemId, Model.RegistrationID, Model.Encounterid,
                 Model.HospitalLocationID, Model.FacilityId, Model.Pageid,
                 Model.UserId, Model.Shownote);

            return Ok(JsonConvert.SerializeObject("Saved"));
        }

        [HttpPost]
        [Route("api/EMRAPI/CancelChronicProblem")]
        public IHttpActionResult CancelChronicProblem([FromBody] Canceltodayproblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            objcls.Cancelchronicproblem(Model.ProblemId, Model.RegistrationID, Model.Encounterid,
                 Model.HospitalLocationID, Model.FacilityId, Model.Pageid,
                 Model.UserId, Model.Shownote);

            return Ok(JsonConvert.SerializeObject("Saved"));
        }

        [HttpPost]
        [Route("api/EMRAPI/getEMRSingleScreenDataInTransit")]
        public IHttpActionResult getEMRSingleScreenDataInTransit([FromBody] getEMRSingleScreenDataInTransit Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getEMRSingleScreenDataInTransit(Model.FacilityId, Model.EncounterId, Model.EncodedBy);
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
        [Route("api/EMRAPI/SaveSingleScreenDataInTransit")]
        public IHttpActionResult SaveSingleScreenDataInTransit([FromBody] SaveSingleScreenDataInTransit Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);


            string OutSb = objcls.SaveSingleScreenDataInTransit(Model.HospId, Model.RegistrationId, Model.EncounterId, Model.FacilityId, Model.Complaints,
                Model.IsNoAllergies, Model.AllergyId, Model.AllergyName, Model.AllergyType, Model.ServerityId, Model.VitalHT, Model.VitalWT, Model.VitalHC,
                Model.VitalT, Model.VitalR, Model.VitalP, Model.VitalBPS, Model.VitalBPD, Model.VitalMAC, Model.VitalBMI, Model.VitalSPO2,
                Model.VitalBSA, Model.History, Model.PastHistory, Model.PreviousTreatment, Model.Examination, Model.NutritionalStatus, Model.CostAnalysis, Model.PlanOfCare,
                Model.DiagnosisSearchKeyId, Model.ProvisionalDiagnosis, Model.OrderType, Model.NonDrugDoctorId, Model.NonDrugOrder, Model.EncodedBy);
            return Ok(JsonConvert.SerializeObject(OutSb));
        }

        [HttpPost]
        [Route("api/EMRAPI/getPAstHistory")]
        public IHttpActionResult getPAstHistory([FromBody] getPAstHistory Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getPAstHistory(Model.regID, Model.encounterID, Model.doctorID);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/getPAstCheifComplaints")]
        public IHttpActionResult getPAstCheifComplaints([FromBody] getPAstCheifComplaints Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getPAstCheifComplaints(Model.regID, Model.encounterID, Model.doctorID);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);

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
        [Route("api/EMRAPI/getSingleScreenUserTemplates")]
        public IHttpActionResult getSingleScreenUserTemplates([FromBody] getSingleScreenUserTemplates Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getSingleScreenUserTemplates(Model.SpecialisationId, Model.FacilityId, Model.DoctorId);
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
        [Route("api/EMRAPI/getSingleScreenUserTemplatesIcon")]
        public IHttpActionResult getSingleScreenUserTemplatesIcon([FromBody] getSingleScreenUserTemplates Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getSingleScreenUserTemplates(Model.SpecialisationId, Model.FacilityId, Model.DoctorId);
                List<getSingleScreenUserTemplatesIconOut> OutParam = new List<getSingleScreenUserTemplatesIconOut>();

                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getSingleScreenUserTemplatesIconOut model = new getSingleScreenUserTemplatesIconOut();
                        model.TemplateCode = ds.Tables[0].Rows[i]["TemplateCode"].ToString();
                        model.IsCollapse = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsCollapse"]);
                        model.IsMandatory = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsMandatory"]);
                        OutParam.Add(model);
                    }
                }
                else
                {
                    OutParam.Add(null);
                }

                return Ok(OutParam);
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
        [Route("api/EMRAPI/SearchAppointmentList")]
        public IHttpActionResult SearchAppointmentList([FromBody] SearchAppointmentList Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.SearchAppointmentList(Model.HospitalLocationId, Model.DoctorId, Model.FacilityId, Model.DateRange, Model.FromDate, Model.ToDate, Model.RegistrationNo,
                    Model.PatientName, Model.OldRegistrationNo, Model.EnrolleNo, Model.MobileNo, Model.EncounterNo, Model.LoginFacilityId, Model.StatusId);
                return Ok(JsonConvert.SerializeObject(ds));
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
        [Route("api/EMRAPI/GetDoctorPatientLists")]
        public IHttpActionResult GetDoctorPatientLists([FromBody] GetDoctorPatientLists Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetDoctorPatientLists(Model.HospitalLocationId, Model.DoctorId, Model.FacilityId, Model.DateRange, Model.FromDate, Model.ToDate, Model.RegistrationNo,
                    Model.PatientName, Model.OldRegistrationNo, Model.EnrolleNo, Model.MobileNo, Model.EncounterNo, Model.LoginFacilityId, Model.StatusId, Model.intemrStatusId,
                    Model.chrEncounterType, Model.intSpecialisationId);
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
        [Route("api/EMRAPI/GetEmployeeWithResource")]
        public IHttpActionResult GetEmployeeWithResource([FromBody] GetEmployeeWithResource Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetEmployeeWithResource(Model.HospitalLocationId, Model.UserId, Model.SpecialisationId, Model.FacilityId);
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
        [Route("api/EMRAPI/GetDoctorTimeSpecialisation")]
        public IHttpActionResult GetDoctorTimeSpecialisation([FromBody] GetDoctorTimeSpecialisation Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetDoctorTimeSpecialisation(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/EMRAPI/GetPatientAccessRights")]
        public IHttpActionResult GetPatientAccessRights([FromBody] GetEmployeeWithResource Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetPatientAccessRights(Model.HospitalLocationId, Model.UserId, Model.SpecialisationId, Model.FacilityId);
                return Ok(JsonConvert.SerializeObject(ds, Formatting.Indented));
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
        [Route("api/EMRAPI/ValidateUserForEMRFile")]
        public IHttpActionResult ValidateUserForEMRFile([FromBody] ValidateUserForEMRFile Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);

            int outputresult = objcls.ValidateUserForEMRFile(Model.HospitalLocationId, Model.RegistrationID, Model.EncounterId, Model.UserId);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRAPI/getEMRPatientDetails")]
        public IHttpActionResult getEMRPatientDetails([FromBody] getEMRPatientDetails Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);

            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getEMRPatientDetails(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationNo, Model.EncounterNo, Model.UserId, Model.RegistrationId);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/EMRAcknowledgeNextPatient")]
        public IHttpActionResult EMRAcknowledgeNextPatient([FromBody] EMRAcknowledgeNextPatient Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            bool outputresult = objcls.EMRAcknowledgeNextPatient(Model.FacilityId, Model.DoctorId);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRAPI/GetDefaultTemplate")]
        public IHttpActionResult GetDefaultTemplate([FromBody] GetDefaultTemplate Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDefaultTemplate(Model.HospitalLocationId, Model.FacilityId, Model.UserId);
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
        [Route("api/EMRAPI/GetDefaultPageByUser")]
        public IHttpActionResult GetDefaultPageByUser([FromBody] GetDefaultPageByUser Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            string outputresult = objcls.GetDefaultPageByUser(Model.GroupId, Model.URLAddress);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRAPI/GetTokentocallDoc")]
        public IHttpActionResult GetTokentocallDoc([FromBody] GetTokentocallDoc Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTokentocallDoc(Model.IPAddress, Model.Tokenno);
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
        [Route("api/EMRAPI/GetDisplayCurrentPatient")]
        public IHttpActionResult GetDisplayCurrentPatient([FromBody] GetDisplayCurrentPatient Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);

            try
            {
                string outputresult = objcls.SaveDisplayCurrentPatient(Model.IPAddress, Model.sFlag, Model.sDoctorName, Model.sDeptName, Model.sRegistrationNo, Model.iHospitalLocationId, Model.FacilityId, Model.EncounterId, Model.AppointmentResourceId, Model.Employeeid);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/QMSClose")]
        public IHttpActionResult QMSClose([FromBody] QMSClose Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);

            try
            {
                string outputresult = objcls.EMRCloseQMS(Model.DoctorId, Model.facilityid);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/CallPatient")]
        public IHttpActionResult CallPatient([FromBody] CallPatient Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = objcls.CallPatient(Model.Tokenno, Model.UserId, Model.ServiceID, Model.CounterID, Model.ZoneID);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRAPI/GetReferralDetailCount")]
        public IHttpActionResult GetReferralDetailCount([FromBody] GetReferralDetailCount Model)
        {
            BaseC.clsWardManagement objcls = new BaseC.clsWardManagement(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetReferralDetailCount(Model.UserId);
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
        [Route("api/EMRAPI/getPatientShortDetail")]
        public IHttpActionResult getPatientShortDetail([FromBody] getPatientShortDetail Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientShortDetailDS(Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetPatientDashBordSection")]
        public IHttpActionResult GetPatientDashBordSection([FromBody] GetPatientDashBordSection Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDashBordSection(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/EMRAPI/GetPatientDashboard")]
        public IHttpActionResult GetPatientDashboard([FromBody] GetPatientDashboard Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDashboard(Model.RegistrationId);
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
        [Route("api/EMRAPI/getDoctorImageDetails")]
        public IHttpActionResult getDoctorImageDetails([FromBody] getDoctorImageDetails Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getDoctorImageDetails(Model.DoctorID, Model.HospitalLocationId, Model.FacilityId, Model.EncounterId);
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
        [Route("api/EMRAPI/EMRGetReportName")]
        public IHttpActionResult EMRGetReportName([FromBody] EMRGetReportName Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.EMRGetReportName(Model.ReportId);
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
        [Route("api/EMRAPI/EMRGetFacilityMaster")]
        public IHttpActionResult EMRGetFacilityMaster([FromBody] GetPatientDashBordSection Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.EMRGetFacilityMaster(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/EMRAPI/getCustomizedPatientReportHeader")]
        public IHttpActionResult getCustomizedPatientReportHeader([FromBody] getCustomizedPatientReportHeader Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getCustomizedPatientReportHeader(Model.HeaderId, Model.PatientType, Model.EncounterId, Model.DoctorId, Model.RegistrationId, Model.FacilityId);
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
        [Route("api/EMRAPI/getEMRPrintCaseSheet")]
        public IHttpActionResult getEMRPrintCaseSheet([FromBody] getEMRPrintCaseSheet Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRPrintCaseSheetDate(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId,
                                Model.EncounterId, Model.FromDate, Model.ToDate, Model.TemplateName, Model.TemplateId, Model.TemplateType,
                                Model.ChronologicalOrder, Model.ReportId);
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
        [Route("api/EMRAPI/GetTemplateStyle")]
        public IHttpActionResult GetTemplateStyle([FromBody] GetTemplateStyle Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateStyle(Model.HospitalLocationId);
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
        //[HttpPost]
        //[Route("api/EMRAPI/GetEMRTemplates")]
        //public IHttpActionResult GetEMRTemplates([FromBody] GetEMRTemplates Model)
        //{
        //    BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
        //    DataSet outputresult = new DataSet();
        //    try
        //    {
        //        if (Model.TemplateId > 0)
        //        {
        //            outputresult = objcls.GetEMRTemplates(Model.EncounterId, Model.RegistrationId, Model.EREncounterId,Model.TemplateId);
        //        }
        //        else
        //        {
        //            outputresult = objcls.GetEMRTemplates(Model.EncounterId, Model.RegistrationId, Model.EREncounterId);
        //        }

        //        string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
        //        return Ok(json);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    finally
        //    {
        //        outputresult.Dispose();
        //        objcls = null;
        //    }
        //}
        [HttpPost]
        [Route("api/EMRAPI/UpDateEMRToTranslateLanguage")]
        public IHttpActionResult UpDateEMRToTranslateLanguage([FromBody] UpDateEMRToTranslateLanguage Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = objcls.UpDateEMRToTranslateLanguage(Model.RegistrationId, Model.EMRToTranslateLanguage);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRAPI/getTaggedReportFormatDetails")]
        public IHttpActionResult getTaggedReportFormatDetails([FromBody] getTaggedReportFormatDetails Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getReportFormatDetails(Model.DoctorId);
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
        [Route("api/EMRAPI/GetICMPatientSummaryDetails")]
        public IHttpActionResult GetICMPatientSummaryDetails([FromBody] GetICMPatientSummaryDetails Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetICMPatientSummaryDetails(Model.HospitalLocationID, Model.RegistrationId, Model.EncounterId, Model.ReportId, Model.FacilityId);
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
        [Route("api/EMRAPI/FillAttachmentDownloadDropdownIP")]
        public IHttpActionResult FillAttachmentDownloadDropdownIP([FromBody] FillAttachmentDownloadDropdownIP Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.FillAttachmentDownloadDropdownIP(Model.DiagSampleId, Model.FileName);
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
        [Route("api/EMRAPI/FillAttachmentDownloadDropdownOP")]
        public IHttpActionResult FillAttachmentDownloadDropdownOP([FromBody] FillAttachmentDownloadDropdownOP Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.FillAttachmentDownloadDropdownOP(Model.DiagSampleId, Model.FileName);
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
        [Route("api/EMRAPI/BindPullFarwordAndRemarks")]
        public IHttpActionResult BindPullFarwordAndRemarks([FromBody] BindPullFarwordAndRemarks Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindPullFarwordAndRemarks(Model.EncounterId);
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
        [Route("api/EMRAPI/GetChiefProblem")]
        public IHttpActionResult GetChiefProblem([FromBody] GetChiefProblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetChiefProblem(Model.HospitalLocationID, Model.FacilityId, Model.RegistrationId, Model.DoctorId, Model.Daterange,
                    Model.FromDate, Model.ToDate, Model.SearchCriteriya, Model.IsDistinct, Model.IsChronic, Model.ProblemId, Model.VisitType);
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
        [Route("api/EMRAPI/SavePatientProblems")]
        public IHttpActionResult SavePatientProblems([FromBody] SavePatientProblems Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.EMRSavePatientProblems(Model.HospitalLocationID, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.PageId,
                    Model.xmlProblemDetails, Model.UserId, Model.Remarks, Model.IsPregment, Model.IsBreastFeed, Model.IsShowNote, Model.DoctorId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                outputresult = string.Empty;
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/BindFavouriteProblems")]
        public IHttpActionResult BindFavouriteProblems([FromBody] BindFavouriteProblems Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindFavouriteProblems(Model.strSearchCriteria, Model.DoctorID);
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
        [Route("api/EMRAPI/BindDoctorFavouriteProblems")]
        public IHttpActionResult BindDoctorFavouriteProblems([FromBody] BindFavouriteProblems Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet ds = new DataSet();
            ds = objcls.BindFavouriteProblems(Model.strSearchCriteria, Model.DoctorID, Model.Type);
            List<BindFavouriteProblemsOutput> OutParam = new List<BindFavouriteProblemsOutput>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        BindFavouriteProblemsOutput model = new BindFavouriteProblemsOutput();
                        model.ProblemId = common.myInt(ds.Tables[0].Rows[i]["ProblemId"]);
                        model.ProblemDescription = ds.Tables[0].Rows[i]["ProblemDescription"].ToString();
                        model.SNOMEDCode = ds.Tables[0].Rows[i]["SNOMEDCode"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Ok(OutParam);
        }

        [HttpPost]
        [Route("api/EMRAPI/PopulateAllProblem")]
        public IHttpActionResult PopulateAllProblem([FromBody] PopulateAllProblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.PopulateAllProblem(Model.strSearchCriteria, Model.HospitalLocationId, Model.DoctorID);
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
        [Route("api/EMRAPI/SaveEMRFavProblem")]
        public IHttpActionResult SaveEMRFavProblem([FromBody] SaveEMRFavProblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.SaveEMRFavProblem(Model.HospitalLocationId, Model.DoctorId, Model.ProblemId, Model.UserId, Model.ProblemDescription);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/BindODLQCS")]
        public IHttpActionResult BindODLQCS([FromBody] BindODLQCS Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindODLQCS(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/CheckDuplicateProblem")]
        public IHttpActionResult CheckDuplicateProblem([FromBody] CheckDuplicateProblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.CheckDuplicateProblem(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.ProblemId, Model.IsChronic);
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
        [Route("api/EMRAPI/DeleteEMRFavProblem")]
        public IHttpActionResult DeleteEMRFavProblem([FromBody] DeleteEMRFavProblem Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.DeleteEMRFavProblem(Model.DoctorId, Model.ProblemId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/SaveHPIRemarks")]
        public IHttpActionResult SaveHPIRemarks([FromBody] SaveHPIRemarks Model)
        {
            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.SaveHPIRemarks(Model.EncounterId, Model.Remarks, Model.IsPullForwardComplaint);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetProblemsList")]
        public IHttpActionResult GetProblemsList([FromBody] GetProblemsList Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetProblemsList(Model.SearchText, Model.HospitalLocationId);
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
        [Route("api/EMRAPI/getIVFRegistrationId")]
        public IHttpActionResult getIVFRegistrationId([FromBody] getIVFRegistrationId Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getIVFRegistrationId(Model.IVFId, Model.EncounterId, Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/EMRAPI/getIVFPatient")]
        public IHttpActionResult getIVFPatient([FromBody] getIVFPatient Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getIVFPatient(Model.RegistrationId, Model.IVFId);
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
        [Route("api/EMRAPI/ISDiagnosesExits")]
        public IHttpActionResult ISDiagnosesExits([FromBody] ISDiagnosesExits Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.ISDiagnosesExits(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetPatientDiagnosis")]
        public IHttpActionResult GetPatientDiagnosis([FromBody] GetPatientDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDiagnosis(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId,
                    Model.DoctorId, Model.DiagnosisGroupId, Model.DiagnosisSubGroupId, Model.DateRange, Model.FromDate, Model.ToDate,
                    Model.SearchKeyword, Model.IsDistinct, Model.StatusId, Model.VisitType, Model.IsChronic, Model.DiagnosisId);
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
        [Route("api/EMRAPI/GetDiscriptionandICDID")]
        public IHttpActionResult GetDiscriptionandICDID([FromBody] GetDiscriptionandICDID Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.selectDiscriptionandICDID(Model.IcdCode);
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
        [Route("api/EMRAPI/DeleteFavouriteDiagnosis")]
        public IHttpActionResult DeleteFavouriteDiagnosis([FromBody] DeleteFavouriteDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            bool outputresult = false;
            try
            {
                outputresult = objcls.DeleteFavouriteDiagnosis(Model.DoctorId, Model.DiagnosisId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetICDGroup")]
        public IHttpActionResult GetICDGroup()
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindCategory();
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
        [Route("api/EMRAPI/GetICDSubGroup")]
        public IHttpActionResult GetICDSubGroup([FromBody] GetICDSubGroup Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindSubCategory(Model.DiagnosisGroupId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/GetDiagnosisList")]
        public IHttpActionResult GetDiagnosisList([FromBody] GetDiagnosisList Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string json = string.Empty;
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDiagnosisList(Model.DiagnosisGroupId, Model.DiagnosisSubGroupId);
                json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
            return Ok(json);
        }

        [HttpPost]
        [Route("api/EMRAPI/BindDiagnosis")]
        public IHttpActionResult BindDiagnosis([FromBody] BindDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindDiagnosis(Model.DiagnosisGroupId, Model.DiagnosisSubGroupId, Model.DiagnosisCode);
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
        [Route("api/EMRAPI/BindFavouriteDiagnosis")]
        public IHttpActionResult BindFavouriteDiagnosis([FromBody] BindFavouriteDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindFavoriteDiagnosis(Model.DoctorId, Model.DiagnosisGroupId, Model.DiagnosisSubGroupId, Model.DiagnosisCode);
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
        [Route("api/EMRAPI/SaveFavouriteDiagnosis")]
        public IHttpActionResult SaveFavouriteDiagnosis([FromBody] SaveFavouriteDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveFavouriteDiagnosis(Model.DoctorId, Model.DiagnosisId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetDiagnosisDescription")]
        public IHttpActionResult GetDiagnosisDescription([FromBody] GetDiagnosisDescription Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.selectDiscription(Model.IcdCode);
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
        [Route("api/EMRAPI/BindDiagnosistype")]
        public IHttpActionResult BindDiagnosistype()
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindDiagnosistype();
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
        [Route("api/EMRAPI/BindDiagnosisCondition")]
        public IHttpActionResult BindDiagnosisCondition()
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindDiagnosisCondition();
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
        [Route("api/EMRAPI/CheckDiagnosisExcluded")]
        public IHttpActionResult CheckDiagnosisExcluded([FromBody] CheckDiagnosisExcluded Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.CheckDiagnosisExcluded(Model.HospitalLocationId, Model.Facilityid, Model.RegistrationId, Model.IcdCode);
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
        [Route("api/EMRAPI/CheckPatientPrimaryDiagnosis")]
        public IHttpActionResult CheckPatientPrimaryDiagnosis([FromBody] CheckPatientPrimaryDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SelectDiagnosispatientdtl(Model.DiagnosisId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/CheckValidForPrimaryDiagnosis")]
        public IHttpActionResult CheckValidForPrimaryDiagnosis([FromBody] CheckValidForPrimaryDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            bool outputresult = false;
            try
            {
                outputresult = objcls.CheckValidForPrimaryDiagnosis(Model.DiagnosisId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/CheckDuplicateDiagnosis")]
        public IHttpActionResult CheckDuplicateDiagnosis([FromBody] CheckDuplicateDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.CheckDuplicateDiagnosis(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.DiagnosisId, Model.IsChronic);
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
        [Route("api/EMRAPI/SavePatientDiagnosis")]
        public IHttpActionResult SavePatientDiagnosis([FromBody] SavePatientDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.EMRSavePatientDiagnosis(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.DoctorId,
                    Model.PageId, Model.DiagnosisXML, Model.PatientAlertXML, Model.UserId, Model.IsPullDiagnosis, Model.IsShowNote, Model.MRDCode);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/TaggingStaticTemplateWithTemplateField")]
        public IHttpActionResult TaggingStaticTemplateWithTemplateField([FromBody] TaggingStaticTemplateWithTemplateField Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);

            try
            {
                objcls.TaggingStaticTemplateWithTemplateField(Model.RegistrationId, Model.EncounterId, Model.SectionId, Model.FieldId, Model.TemplateId, Model.UserId);
                string json = JsonConvert.SerializeObject("Saved", Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/DeletePatientDiagnosis")]
        public IHttpActionResult DeletePatientDiagnosis([FromBody] DeletePatientDiagnosis Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.DeletePatientDiagnosis(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.UserId, Model.DiagnosisId, Model.PageId);
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
        [Route("api/EMRAPI/LogEducationAndMonograph")]
        public IHttpActionResult LogEducationAndMonograph([FromBody] LogEducationAndMonograph Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.LogEducationAndMonograph(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.DoctorId, Model.UserId);
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
        [Route("api/EMRAPI/CheckDuplicateServiceOrder")]
        public IHttpActionResult CheckDuplicateServiceOrder([FromBody] CheckDuplicateServiceOrder Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.CheckDuplicateServiceOrder(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.RegistrationId, Model.ServiceId);
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
        [Route("api/EMRAPI/GetServiceDescriptionForOrderpage")]
        public IHttpActionResult GetServiceDescriptionForOrderpage([FromBody] GetServiceDescriptionForOrderpage Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetServiceDescription(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.ServiceId,
                                                Model.OrderSetId, Model.CompanyId, Model.SponsorId, Model.CardId, Model.Option, Model.TemplateId, Model.xmlServiceIds);
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
        [Route("api/EMRAPI/DeleteFavouriteOrder")]
        public IHttpActionResult DeleteFavouriteOrder([FromBody] DeleteFavouriteOrder Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.DeleteFavorite(Model.DoctorId, Model.FavoriteId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetFavouriteOrder")]
        public IHttpActionResult GetFavouriteOrder([FromBody] GetFavouriteOrder Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFavorites(Model.DoctorId, Model.DepartmentId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult.Tables[0], Formatting.Indented);
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
        [Route("api/EMRAPI/GetSearchServices")]
        public IHttpActionResult GetSearchServices([FromBody] GetSearchServices Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.GetSearchServices(Model.HospitalLocationId, Model.DepartmentId, Model.SubDepartmentId, Model.ServiceName, Model.FacilityId, Model.SecGroupId, Model.EmployeeId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/PopulateInvestigationSetUserWise")]
        public IHttpActionResult PopulateInvestigationSetUserWise([FromBody] PopulateInvestigationSetUserWise Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.populateInvestigationSetUserWise(Model.HospitalLocationId, Model.Employeeid);
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
        [Route("api/EMRAPI/GetPatientConsultingDoctor")]
        public IHttpActionResult GetPatientConsultingDoctor([FromBody] GetPatientConsultingDoctor Model)
        {
            BaseC.clsEMRBilling objcls = new BaseC.clsEMRBilling(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.GetPatientConsultingDoctor(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetGlobalToothNo")]
        public IHttpActionResult GetGlobalToothNo()
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetGlobalToothNo();
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
        [Route("api/EMRAPI/GetPatientProvisionalDiagnosis")]
        public IHttpActionResult GetPatientProvisionalDiagnosis([FromBody] GetPatientProvisionalDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientProvisionalDiagnosis(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.UserId, Model.ProvisionalDiagnosisId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/SaveFavouriteOrder")]
        public IHttpActionResult SaveFavouriteOrder([FromBody] SaveFavouriteOrder Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveFavorite(Model.DoctorId, Model.ServiceId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/ValidateErxPatientXML")]
        public IHttpActionResult ValidateErxPatientXML([FromBody] ValidateErxPatientXML Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.ValidateErxPatientXML(Model.RegistrationId, Model.DoctorId);
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
        [Route("api/EMRAPI/GetEncounterCompany")]
        public IHttpActionResult GetEncounterCompany([FromBody] GetEncounterCompany Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEncounterCompany(Model.EncounterId);
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
        [Route("api/EMRAPI/SaveServiceOrderEMR")]
        public IHttpActionResult SaveServiceOrderEMR([FromBody] SaveServiceOrderEMR Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.SaveServiceOrderEMR(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.xmlServiceList,
                    Model.XMLPatientAlert, Model.Remark, Model.UserId, Model.DoctorId, Model.CompanyId, Model.OrderType, Model.PayerType, Model.PatientOPIP, Model.InsuranceId,
                    Model.CardId, Model.OrderDate, Model.ChargeCalculationRequired, Model.IsAllergyReviewed, Model.IsERorEMRServices, Model.RequestId, Model.xmlTemplateDetails);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                // outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetSectionTreeForPatientsForms")]
        public IHttpActionResult GetSectionTreeForPatientsForms([FromBody] GetSectionTreeForPatientsForms Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetSectionTreeForPatientsForms(Model.HospitalLocationId, Model.TemplateId, Model.GenderType, Model.FormId, Model.ReportId);
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
        [Route("api/EMRAPI/GetTemplateDetailsTabular")]
        public IHttpActionResult GetTemplateDetailsTabular([FromBody] GetTemplateDetailsTabular Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateDetailsTabular(Model.TemplateId, Model.EncounterId, Model.GenderType, Model.SectionId, Model.RecordId, Model.UserId,
                    Model.OrderId, Model.RegistrationId, Model.ResultSetId, Model.EpisodeId, Model.FacilityId, Model.EmployeeId, Model.PatientOPIP);
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
        [Route("api/EMRAPI/GetTemplateDetails")]
        public IHttpActionResult GetTemplateDetails([FromBody] GetTemplateDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateDetails(Model.TemplateId, Model.EncounterId, Model.GenderType, Model.RegistrationId, Model.FacilityId, Model.UserId,
                        Model.ShowPreviousData, Model.OrderId, Model.RecordId, Model.ResultSetId, Model.EpisodeId, Model.SectionId, Model.TemplateDetails, Model.ServiceId,
                        Model.RequestId, Model.EmployeeId, Model.PatientOPIP, Model.OrderDetailId, Model.FromDate, Model.ToDate, Model.GroupingDate, Model.EREncounterId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                // outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getEncounterEMRStatus")]
        public IHttpActionResult getEncounterEMRStatus([FromBody] getEncounterEMRStatus Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEncounterEMRStatus(Model.EncounterId);
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
        [Route("api/EMRAPI/getPermissionConfidentialUsers")]
        public IHttpActionResult getPermissionConfidentialUsers([FromBody] getPermissionConfidentialUsers Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPermissionConfidentialUsers(Model.HospitalLocationId, Model.FacilityId, Model.TemplateId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetGroupWiseMenuTagging")]
        public IHttpActionResult GetGroupWiseMenuTagging([FromBody] GetGroupWiseMenuTagging Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetGroupWiseMenuTagging(Model.HospitalLocationId, Model.FacilityId, Model.GroupId, Model.ModuleId, Model.PageCode);
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
        [Route("api/EMRAPI/GetNewBornBabyMotherDetails")]
        public IHttpActionResult GetNewBornBabyMotherDetails([FromBody] GetNewBornBabyMotherDetails Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.GetMotherNewBornBabyRelation(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/getEMRTemplateReportSetup")]
        public IHttpActionResult getEMRTemplateReportSetup([FromBody] getEMRTemplateReportSetup Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRTemplateReportSetup(Model.ReportId, Model.TemplateId, Model.DoctorId, Model.Flag, Model.Active, Model.HospitalLocationId, Model.UserId);
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
        [Route("api/EMRAPI/DoLockUnLock")]
        public IHttpActionResult DoLockUnLock([FromBody] DoLockUnLock Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.DoLockUnLock(Model.AbsolutePagePath, Model.Type, Model.EncounterId, Model.HospitalLocationId, Model.FormId, Model.TemplateId, Model.Lock);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/IsSectionAddendum")]
        public IHttpActionResult IsSectionAddendum([FromBody] IsSectionAddendum Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            bool outputresult = false;
            try
            {
                outputresult = objcls.IsSectionAddendum(Model.SectionId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetSectionTree")]
        public IHttpActionResult GetSectionTree([FromBody] GetSectionTree Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetSectionTree(Model.HospitalLocationId, Model.TemplateId, Model.GenderType);
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
        [Route("api/EMRAPI/GetFormatText")]
        public IHttpActionResult GetFormatText([FromBody] GetFormatText Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFormatText(Model.FormatId);
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
        [Route("api/EMRAPI/GetPatientDiagnosisPreHistory")]
        public IHttpActionResult GetPatientDiagnosisPreHistory([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", param.Registraionid);
            hshInput.Add("@inyHospitalLocationId", param.HospitalLocationid);
            hshInput.Add("@intEncounterId", param.Encounterid);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "GetPatientDiagnosis", hshInput);
            List<PatientDiagnosisOutParam> OutParam = new List<PatientDiagnosisOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientDiagnosisOutParam model = new PatientDiagnosisOutParam();
                        model.Description = ds.Tables[0].Rows[i]["ICDDescription"].ToString();
                        model.VisitType = ds.Tables[0].Rows[i]["VisitType"].ToString();
                        model.Date = ds.Tables[0].Rows[i]["EncodedDate"].ToString();
                        model.IsChronic = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsChronic"]);
                        model.IsPrimaryDiagnosis = Convert.ToBoolean(ds.Tables[0].Rows[i]["PrimaryDiagnosis"]);
                        model.IsFinalDiagnosis = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsFinalDiagnosis"]);
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);
        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientDrugOrder")]
        public IHttpActionResult GetPatientDrugOrder([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@registrationid", param.Registraionid);
            hshInput.Add("@HospitalLocationId", param.HospitalLocationid);
            hshInput.Add("@intEncounterId", param.Encounterid);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "GetPatientDrugOrder", hshInput);
            List<PatientDrugOrderOutParam> OutParam = new List<PatientDrugOrderOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientDrugOrderOutParam model = new PatientDrugOrderOutParam();
                        model.Medicine = ds.Tables[0].Rows[i]["ItemName"].ToString();
                        model.Detail = ds.Tables[0].Rows[i]["PrescriptionDetail"].ToString();
                        model.Date = ds.Tables[0].Rows[i]["medStartDate"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);
        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientImmunizationDueDates")]
        public IHttpActionResult GetPatientImmunizationDueDates([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", param.Registraionid);
            hshInput.Add("@inyHospitalLocationID", param.HospitalLocationid);
            hshInput.Add("@dob", param.dob);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunizationDueDates", hshInput);
            List<PatientImmunizationOutParam> OutParam = new List<PatientImmunizationOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientImmunizationOutParam model = new PatientImmunizationOutParam();
                        model.ImmunizationId = Convert.ToInt16(ds.Tables[0].Rows[i]["ImmunizationID"]);
                        model.ImmunizationName = ds.Tables[0].Rows[i]["ImmunizationName"].ToString();
                        model.Status = ds.Tables[0].Rows[i]["GivenStatus"].ToString();
                        model.Duedate = ds.Tables[0].Rows[i]["ImmunizationDueDate"].ToString();
                        model.GivenDate = ds.Tables[0].Rows[i]["GivenDate"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);

        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientOrderProcedure")]
        public IHttpActionResult GetPatientOrderProcedure([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@registrationid", param.Registraionid);
            hshInput.Add("@HospitalLocationId", param.HospitalLocationid);
            hshInput.Add("@intEncounterId", param.Encounterid);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "GetPatientOrderProcedure", hshInput);
            List<PatientOrderProcedureOutParam> OutParam = new List<PatientOrderProcedureOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientOrderProcedureOutParam model = new PatientOrderProcedureOutParam();
                    
                        model.ServiceName = ds.Tables[0].Rows[i]["ServiceName"].ToString();
                        model.Date = ds.Tables[0].Rows[i]["ToDate"].ToString();
                        model.SOURCE = ds.Tables[0].Rows[i]["SOURCE"].ToString();
                        model.LABNO = common.myInt(ds.Tables[0].Rows[i]["LABNO"]);
                        model.ServiceIds = common.myInt(ds.Tables[0].Rows[i]["ServiceId"]);
                        model.StationId = common.myInt(ds.Tables[0].Rows[i]["StationId"]);
                        model.StatusCode = common.myStr(ds.Tables[0].Rows[i]["StatusCode"]);
                        model.FlagName = common.myStr(ds.Tables[0].Rows[i]["FlagName"]);
                        model.OutsourceInvestigation = common.myBool(ds.Tables[0].Rows[i]["OutsourceInvestigation"]);
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);

        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientChifComplaints")]
        public IHttpActionResult GetPatientChifComplaints([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@registrationid", param.Registraionid);
            hshInput.Add("@HospitalLocationId", param.HospitalLocationid);
            hshInput.Add("@intEncounterId", param.Encounterid);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientChifComplaints", hshInput);
            List<PatientOrderChifComplaintsOutParam> OutParam = new List<PatientOrderChifComplaintsOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientOrderChifComplaintsOutParam model = new PatientOrderChifComplaintsOutParam();

                        model.Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);
                        model.ProblemDescription = ds.Tables[0].Rows[i]["ProblemDescription"].ToString();
                        model.EntryDate = ds.Tables[0].Rows[i]["EntryDate"].ToString();


                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);

        }
        [HttpPost]
        [Route("api/EMRAPI/RemoveDoctorFavProblems")]
        public IHttpActionResult RemoveDoctorFavProblems([FromBody] DoctorProblem param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string message = "";
            hshInput = new Hashtable();
            hshInput.Add("@DoctorId", param.DoctorId);
            hshInput.Add("@ProblemId", param.ProblemId);
            try
            {
                message  = (string)objDl.ExecuteScalar(CommandType.StoredProcedure, "UspRemveDoctorFavProblem", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(message);

        }


        // RemoveDoctorFavProblems

        [HttpPost]
        [Route("api/EMRAPI/GetPatientPastHistory")]
        public IHttpActionResult GetPatientPastHistory([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@registrationid", param.Registraionid);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientPastHistory", hshInput);
            List<PatinetPastHistory> OutParam = new List<PatinetPastHistory>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatinetPastHistory model = new PatinetPastHistory();
                        model.ValueWordProcessor = ds.Tables[0].Rows[i]["ValueWordProcessor"].ToString();
                        model.EncodedDate = ds.Tables[0].Rows[i]["EncodedDate"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);

        }

        [HttpPost]
        [Route("api/EMRAPI/GetSingleScreenPatientDetails")]
        public IHttpActionResult GetSingleScreenPatientDetails([FromBody] SingleScreenPatientDetailInParam param)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@chvRegistrationNo", param.RegistrationNo);
            hshInput.Add("@inyHospitalLocationId", param.HospitalLocationId);
            hshInput.Add("@intFacilityId", param.FacilityId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSingleScreenPatientDetails", hshInput);
            List<SingleScreenPatientDetailOutParam> OutParam = new List<SingleScreenPatientDetailOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SingleScreenPatientDetailOutParam model = new SingleScreenPatientDetailOutParam();

                        model.RegistrationNo = Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationNo"]);
                        model.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        model.GenderAge = ds.Tables[0].Rows[i]["GenderAge"].ToString();
                        model.DoctorName = ds.Tables[0].Rows[i]["DoctorName"].ToString();
                        model.EncounterDate = ds.Tables[0].Rows[i]["EncounterDate"].ToString();
                        model.EncounterNo = ds.Tables[0].Rows[i]["EncounterNo"].ToString();
                        model.DOB = ds.Tables[0].Rows[i]["DOB"].ToString();
                        model.EncDate = ds.Tables[0].Rows[i]["EncDate"].ToString();
                        model.EncMonth = ds.Tables[0].Rows[i]["EncMonth"].ToString();
                        model.EncDay = ds.Tables[0].Rows[i]["EncDay"].ToString();
                        model.OPIP = ds.Tables[0].Rows[i]["OPIP"].ToString();
                        model.Diagnosis = ds.Tables[0].Rows[i]["ICDDescription"].ToString();


                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);
        }

        [HttpPost]
        [Route("api/EMRAPI/GetSingleScreenPatientPastHistory")]
        public IHttpActionResult GetSingleScreenPatientPastHistory([FromBody] SingleScreenPatientDetailInParam param)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", param.RegistrationId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDashboard", hshInput);
            List<SingleScreenPatientDetailOutParam> OutParam = new List<SingleScreenPatientDetailOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SingleScreenPatientDetailOutParam model = new SingleScreenPatientDetailOutParam();

                        //model.RegistrationNo = Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationNo"]);
                        //model.PatientName = ds.Tables[0].Rows[i]["PatientName"].ToString();
                        //model.GenderAge = ds.Tables[0].Rows[i]["GenderAge"].ToString();
                        model.RegistrationId = common.myInt(ds.Tables[0].Rows[i]["RegistrationId"].ToString());
                        model.DoctorName = ds.Tables[0].Rows[i]["DoctorName"].ToString();
                        // model.EncounterDate = ds.Tables[0].Rows[i]["EncounterDate"].ToString();
                        model.EncounterNo = ds.Tables[0].Rows[i]["EncounterNo"].ToString();
                        model.EncounterId = common.myInt(ds.Tables[0].Rows[i]["EncounterId"].ToString());
                        // model.DOB = ds.Tables[0].Rows[i]["DOB"].ToString();
                        model.EncDate = ds.Tables[0].Rows[i]["VisitDate"].ToString();
                        model.EncMonth = ds.Tables[0].Rows[i]["EncMonth"].ToString();
                        model.EncDay = ds.Tables[0].Rows[i]["EncDay"].ToString();
                        model.OPIP = ds.Tables[0].Rows[i]["VisitType"].ToString();
                        // model.Diagnosis = ds.Tables[0].Rows[i]["Diagnosis"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);
        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientAllergies")]
        public IHttpActionResult GetPatientAllergies([FromBody] GetPatientAllergies Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientAllergies(Model.RegistrationId, Model.Active);
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
        [Route("api/EMRAPI/GetPatientPatientAllergies")]
        public IHttpActionResult GetPatientPatientAllergies([FromBody] SingleScreenPatientDetailInParam param)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", param.RegistrationId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientAllergies", hshInput);
            List<PatientAllergiesOutParam> OutParam = new List<PatientAllergiesOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientAllergiesOutParam model = new PatientAllergiesOutParam();


                        model.Allergies = ds.Tables[0].Rows[i]["AllergyName"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);
        }
        // Lab Results
        // Start 

        [HttpPost]
        [Route("api/EMRAPI/getPatientLabResultHistoryDash")]
        public IHttpActionResult getPatientLabResultHistoryDash([FromBody] LabResultHistory Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientLabResultHistoryDash(Model.iLoginFacilityId, Model.iHostId, Model.fromDate, Model.toDate, Model.iRegNo, Model.iProviderId, Model.iPageSize, Model.iPageNo, Model.AbnormalResult, Model.CriticalResult, Model.iStatusId, Model.FacilityId, Model.chvEncounterNo, Model.ReviewedStatus, Model.PatientName, Model.iUserId);
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




        //[HttpPost]
        //[Route("api/EMRAPI/FillAttachmentDownloadDropdownOP")]
        //public IHttpActionResult FillAttachmentDownloadDropdownOP([FromBody] DownloadResults Model)
        //{
        //    BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
        //    DataSet outputresult = new DataSet();
        //    try
        //    {
        //        outputresult = objcls.FillAttachmentDownloadDropdownOP(Model.sampleid,Model.fileName);
        //        string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
        //        return Ok(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        outputresult.Dispose();
        //        objcls = null;
        //    }
        //}

        //[HttpPost]
        //[Route("api/EMRAPI/FillAttachmentDownloadDropdownIP")]
        //public IHttpActionResult FillAttachmentDownloadDropdownIP([FromBody] DownloadResults Model)
        //{
        //    BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
        //    DataSet outputresult = new DataSet();
        //    try
        //    {
        //        outputresult = objcls.FillAttachmentDownloadDropdownIP(Model.sampleid, Model.fileName);
        //        string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
        //        return Ok(json);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        outputresult.Dispose();
        //        objcls = null;
        //    }
        //}

        [HttpPost]
        [Route("api/EMRAPI/getEmployeeData")]
        public IHttpActionResult getEmployeeData([FromBody] EmployeeData Model)
        {
            BaseC.clsLISMaster objcls = new BaseC.clsLISMaster(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEmployeeData(Model.iHospID, Model.iEmpTypeId, Model.xmlEmployeeType, Model.EmpName, Model.iMobileNo, Model.EncodedBy);
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
        [Route("api/EMRAPI/getResultReviewedData")]
        public IHttpActionResult getResultReviewedData([FromBody] ResultReviewedData Model)
        {
            BaseC.clsLISLabOther objcls = new BaseC.clsLISLabOther(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getResultReviewedData(Model.DiagSampleId, Model.Source);
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
        [Route("api/EMRAPI/updateReviewedResultByDiagSampleId")]
        public IHttpActionResult updateReviewedResultByDiagSampleId([FromBody] ResultReviewedData Model)
        {
            BaseC.clsLISLabOther objcls = new BaseC.clsLISLabOther(sConString);
            string outputresult;
            try
            {
                outputresult = objcls.updateReviewedResultByDiagSampleId(Model.xmlDiagSampleId, Model.reviewedStatus, Model.reviewedDate, Model.reviewedComments, Model.reviewedBy, Model.LabFlagValue, Model.TestResultStatus, Model.iNoSMS);
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
        [Route("api/EMRAPI/GetTemplateSectionRow")]
        public IHttpActionResult GetTemplateSectionRow([FromBody] GetTemplateSectionRow Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateSectionRow(Model.SelectedNode);
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
        [Route("api/EMRAPI/GetAllTypeTemplates")]
        public IHttpActionResult GetAllTypeTemplates([FromBody] GetAllTypeTemplates Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetAllTypeTemplates(Model.HospitalLocationId, Model.sType);
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
        [Route("api/EMRAPI/getTemplateDataObjectQuery")]
        public IHttpActionResult getDataObjectValue([FromBody] getTemplateDataObjectQuery Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.getTemplateDataObjectQuery(Model.DataObjectId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/getDataObjectExecute")]
        public IHttpActionResult getDataObjectExecute([FromBody] getDataObjectExecute Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                Hashtable coll = new Hashtable();
                switch (Model.DataObjectId)
                {
                    case 26:
                        coll.Add("@intEncounterId", Model.EncounterId);
                        coll.Add("@intRegistrationId", Model.RegistrationId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);
                        break;

                    case 27: //Patient Vitals Last Height
                        coll.Add("@intEncounterId", Model.EncounterId);
                        coll.Add("@intRegistrationId", Model.RegistrationId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;

                    case 28: //Patient Vitals Last Weight
                        coll.Add("@intEncounterId", Model.EncounterId);
                        coll.Add("@intRegistrationId", Model.RegistrationId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;

                    case 29: //Template Female Diagnosis
                        coll.Add("@intEncounterId", Model.EncounterId);
                        coll.Add("@intRegistrationId", Model.RegistrationId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;

                    case 30: //Template Male Diagnosis
                        coll.Add("@intEncounterId", Model.EncounterId);
                        coll.Add("@intRegistrationId", Model.RegistrationId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;

                    case 31: //Template Female Spouse Diagnosis

                        coll.Add("@intIVFId", Model.IVFId);
                        coll.Add("@intEncounterId", Model.EncounterId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;

                    case 32: //Template Prescaption
                        coll.Add("@intEncounterId", Model.EncounterId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;

                    case 33: //Template Male Spouse Diagnosis
                        coll.Add("@intIVFId", Model.IVFId);
                        coll.Add("@intEncounterId", Model.EncounterId);
                        outputresult = objcls.getDataObjectExecute(Model.Query, coll);

                        break;
                }

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
        [Route("api/EMRAPI/GetTemplateFieldFormats")]
        public IHttpActionResult GetTemplateFieldFormats([FromBody] GetTemplateFieldFormats Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateFieldFormats(Model.FieldId, Model.SpecialisationId);
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
        [Route("api/EMRAPI/getEMRTemplateWordData")]
        public IHttpActionResult getEMRTemplateWordData([FromBody] getEMRTemplateWordData Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRTemplateWordData(Model.FieldId, Model.EncounterId, Model.EncounterId);
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
        [Route("api/EMRAPI/getEMRTemplateReportSequence")]
        public IHttpActionResult getEMRTemplateReportSequence([FromBody] getEMRTemplateReportSequence Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRTemplateReportSequence(Model.ReportId);
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
        [Route("api/EMRAPI/SavePatientNotesData")]
        public IHttpActionResult SavePatientNotesData([FromBody] SavePatientNotesData Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SavePatientNotesData(Model.HospitalLocationId, Model.FacilityId, Model.FormId, Model.IsPullForward, Model.PageId, Model.RegistrationId, Model.EncounterId,
                                        Model.OrderId, Model.TemplateTypeId, Model.xmlTemplateDetails, Model.xmlTabularTemplateDetails, Model.IsShowNote,
                                        Model.SectionId, Model.RecordId, Model.EpisodeId, Model.EncodedBy, Model.OrderDetailId, Model.OrderRequestId, Model.SubDeptId, Model.ProviderId,
                                        Model.ChangeDate, Model.IsApproved, Model.AdvisingDoctorId);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/updateFurtherAckServiceOrderDetail")]
        public IHttpActionResult updateFurtherAckServiceOrderDetail([FromBody] updateFurtherAckServiceOrderDetail Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = "Success";
            try
            {
                objcls.updateFurtherAckServiceOrderDetail(Model.ServiceOrderDetailsId, Model.ServiceId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetEMRPatientForms")]
        public IHttpActionResult GetEMRPatientForms([FromBody] GetEMRPatientForms Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRPatientForms(Model.FormId, Model.EncounterId, Model.ModuleId, Model.GroupId);
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
        [Route("api/EMRAPI/GetEMRTemplateStatic")]
        public IHttpActionResult GetEMRTemplateStatic([FromBody] GetEMRTemplateStatic Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRTemplateStatic(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/GetPatientROS")]
        public IHttpActionResult GetPatientROS([FromBody] GetPatientROS Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientROS(Model.EncounterId, Model.TemplateId);
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
        [Route("api/EMRAPI/GetTemplateDisplayUserName")]
        public IHttpActionResult GetTemplateDisplayUserName([FromBody] GetTemplateDisplayUserName Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateDisplayUserName(Model.HospitalLocationId, Model.TemplateId);
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
        [Route("api/EMRAPI/getEMRTemplateVisitRecoreds")]
        public IHttpActionResult getEMRTemplateVisitRecoreds([FromBody] getEMRTemplateVisitRecoreds Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRTemplateVisitRecoreds(Model.EncounterId, Model.TemplateId, Model.FacilityId);
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
        [Route("api/EMRAPI/getEMRTemplate")]
        public IHttpActionResult getEMRTemplate([FromBody] getEMRTemplate Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRTemplate(Model.HospitalLocationId, Model.EmployeeType, Model.SpecialisationId,
                            Model.ServiceId, Model.ApplicableFor, Model.TemplateId, Model.TemplateType, Model.FacilityId, Model.IsAddendum);
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
        [Route("api/EMRAPI/getTemplateEnteredList")]
        public IHttpActionResult getTemplateEnteredList([FromBody] getTemplateEnteredList Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getTemplateEnteredList(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/getPatientClosedEpisode")]
        public IHttpActionResult getPatientClosedEpisode([FromBody] getPatientClosedEpisode Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientClosedEpisode(Model.TemplateId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/CreateTemplateEpisode")]
        public IHttpActionResult CreateTemplateEpisode([FromBody] CreateTemplateEpisode Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.CreateTemplateEpisode(Model.EpisodeId, Model.RegistrationId, Model.EncounterId, Model.TemplateId,
                                            Model.SaveMode, Model.EpisodeClose, Model.Active, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getPatientPullforwardTemplate")]
        public IHttpActionResult getPatientPullforwardTemplate([FromBody] getPatientPullforwardTemplate Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.getPatientPullforwardTemplate(Model.TemplateId, Model.RegistrationId, Model.EncounterId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getTemplateEpisode")]
        public IHttpActionResult getTemplateEpisode([FromBody] getTemplateEpisode Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getTemplateEpisode(Model.RegistrationId, Model.TemplateId);
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
        [Route("api/EMRAPI/GetTemplateRequiredServices")]
        public IHttpActionResult GetTemplateRequiredServices([FromBody] GetTemplateRequiredServices Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateRequiredServices(Model.ServiceOrderDetailIds, Model.TagType, Model.FacilityId);
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
        [Route("api/EMRAPI/IsTemplateRequiredForService")]
        public IHttpActionResult IsTemplateRequiredForService([FromBody] IsTemplateRequiredForService Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.IsTemplateRequiredForService(Model.Option, Model.ServiceId, Model.TemplateId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetServiceTemplates")]
        public IHttpActionResult GetServiceTemplates([FromBody] GetServiceTemplates Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetServiceTemplates(Model.TemplateId, Model.SubDepartmentId, Model.xmlServiceIds, Model.TagType, Model.FacilityId);
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
        [Route("api/EMRAPI/SaveEMRTemplateResultSet")]
        public IHttpActionResult SaveEMRTemplateResultSet([FromBody] SaveEMRTemplateResultSet Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveEMRTemplateResultSet(Model.ResultSetId, Model.ResultSetName, Model.TemplateId, Model.xmlTemplateData, Model.DoctorId, Model.HospitalLocationId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getTemplateResultSet")]
        public IHttpActionResult getTemplateResultSet([FromBody] getTemplateResultSet Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getTemplateResultSet(Model.TemplateId, Model.DoctorId);
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
        [Route("api/EMRAPI/GetICMNurse")]
        public IHttpActionResult GetICMNurse([FromBody] GetICMNurse Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetICMNurse(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/NonDrugOrder")]
        public IHttpActionResult NonDrugOrder([FromBody] NonDrugOrder Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.NonDrugOrder(Model.HospitalLocationId, Model.FacilityId, Model.UserId, Model.RegistrationId,
                                    Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);

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
        [Route("api/EMRAPI/SaveNonDrugOrder")]
        public IHttpActionResult SaveNonDrugOrder([FromBody] SaveNonDrugOrder Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveNonDrugOrder(Model.NonDrugId, Model.HospitalLocationId, Model.RegistrationId.ToString(),
                                Model.EncounterId.ToString(), Model.OrderDate, Model.PrescriptionText, Model.OrderType,
                                Model.DoctorId, Model.AcknoledgeedBy, Model.AcnoledgekDate, Model.AcknoledgeRemark, Model.UserId,
                                Model.FacilityId, Model.Status, Model.SaveFor, Model.isApprobalReqd);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/DeleteFavouriteNonDrugOrder")]
        public IHttpActionResult DeleteFavouriteNonDrugOrder([FromBody] DeleteFavouriteNonDrugOrder Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            string outputresult = "Success";
            try
            {
                objcls.DeleteICMFavouriteNonDrugOrder(Model.DoctorId, Model.FavouriteId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetavouriteNonDrugOrder")]
        public IHttpActionResult GetavouriteNonDrugOrder([FromBody] GetavouriteNonDrugOrder Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getICMFavouriteNonDrugOrder(Model.SearchCriteria, Model.DoctorId);
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
        [Route("api/EMRAPI/SaveFavouriteNonDrugOrder")]
        public IHttpActionResult SaveFavouriteNonDrugOrder([FromBody] SaveFavouriteNonDrugOrder Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveICMFavouriteNonDrugOrder(Model.DoctorId, Model.PrescriptionText, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }


        // Discharge summry

        [HttpPost]
        [Route("api/EMRAPI/getReportSetupMargin")]
        public IHttpActionResult getReportSetupMargin([FromBody] dischargeReport Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intReportId", Model.ReportId);
                sb.Append("SELECT ISNULL(MarginTop,0) AS MarginTop, ISNULL(MarginBottom,0) AS MarginBottom, ISNULL(MarginLeft,0) AS MarginLeft, ISNULL(MarginRight,0) AS MarginRight, 'A4' AS PageSize FROM EMRTemplateReportSetup WITH (NOLOCK) WHERE ReportId = @intReportId AND Active = 1");
                ds = objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                ds = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/HeaderAndFooter")]
        public IHttpActionResult HeaderAndFooter([FromBody] dischargeReport Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable hsSub = new Hashtable();
            try
            {
                hsSub = new Hashtable();
                hsSub.Add("@intHeaderId", Model.HeaderId);
                hsSub.Add("@chrAppointmentDate", Model.AppointmentDate);
                hsSub.Add("@intEmployeeId", Model.DoctorId);
                hsSub.Add("@intRegistrationId", Model.RegistrationId);
                hsSub.Add("@intFacilityId", Model.FacilityId);
                hsSub.Add("@intEncounterId", Model.EncounterId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hsSub);

                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);

                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                ds = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/OrgContacts")]
        public IHttpActionResult OrgContacts([FromBody] dischargeReport Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            Hashtable hsSub = new Hashtable();
            try
            {
                string strShowRefPhysician = "Select  cont.ContactID,cont.CompanyName  from OrgContacts cont inner join OrgContactCategories cat on cont.CategoryID=cat.CategoryID inner join Encounter  en on cont.ContactID=en.ReferringPhysicianId  where en.ReferringPhysicianId=4 and en.Id=" + Model.EncounterId + " order by CompanyName ";
                ds = objDl.FillDataSet(CommandType.Text, strShowRefPhysician);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                ds = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/GetViewHistory")]
        public IHttpActionResult GetViewHistory([FromBody] GetViewHistory Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);

            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetViewHistory(Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetDateWiseGroupingTemplate")]
        public IHttpActionResult GetDateWiseGroupingTemplate([FromBody] GetDateWiseGroupingTemplate Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.GetDateWiseGroupingTemplate(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/getPatientNoteStatus")]
        public IHttpActionResult getPatientNoteStatus([FromBody] getPatientNoteStatus Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientNoteStatus(Model.EncounterId, Model.TemplateId);
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
        [Route("api/EMRAPI/SavePatientSignData")]
        public IHttpActionResult SavePatientSignData([FromBody] SavePatientSignData Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SavePatientSignDataDS(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId,
                                Model.IsSigned, Model.DoctorId, Model.PatientSummary, Model.EncounterId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetEMRTemplateId")]
        public IHttpActionResult GetEMRTemplateId([FromBody] GetEMRTemplateId Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.GetEMRTemplateId(Model.HospitalLocationId, Model.TemplateName);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/updateEMRPatientForms")]
        public IHttpActionResult updateEMRPatientForms([FromBody] updateEMRPatientForms Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.updateEMRPatientForms(Model.RegistrationId, Model.EncounterId, Model.PatientSummary);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetPatientIllustrationImages")]
        public IHttpActionResult GetPatientIllustrationImages([FromBody] GetPatientIllustrationImages Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientIllustrationImages(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/SaveSeenByDoctor")]
        public IHttpActionResult SaveSeenByDoctor([FromBody] SaveSeenByDoctor Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.EMRSaveSeenByDoctor(Model.EncounterId, Model.DoctorId, Model.SeenStatus, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/IsCopyCaseSheetAuthorized")]
        public IHttpActionResult IsCopyCaseSheetAuthorized([FromBody] IsCopyCaseSheetAuthorized Model)
        {
            BaseC.Security objcls = new BaseC.Security(sConString);
            bool outputresult = false;
            try
            {
                outputresult = objcls.IsCopyCaseSheetAuthorized(Model.UserId, Model.HospitalLocationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetPatientPreviousVitals")]
        public IHttpActionResult GetPatientPreviousVitals([FromBody] GetPatientPreviousVitals Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientPreviousVitals(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetPatientProblemsHPI")]
        public IHttpActionResult GetPatientProblemsHPI([FromBody] GetPatientProblemsHPI Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientProblemsHPI(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetPatientBasicDetails")]
        public IHttpActionResult GetPatientBasicDetails([FromBody] GetPatientBasicDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientBasicDetails(Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetPatientDrugHistory")]
        public IHttpActionResult GetPatientDrugHistory([FromBody] GetPatientDrugHistory Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDrugHistory(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/GetPatientCurrentMedication")]
        public IHttpActionResult GetPatientCurrentMedication([FromBody] GetPatientCurrentMedication Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientCurrentMedication(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/GetPreviousMedicinesIP")]
        public IHttpActionResult GetPreviousMedicinesIP([FromBody] GetPreviousMedicinesIP Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPreviousMedicinesIP(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate, Model.PreviousMedication, Model.IndentId, Model.ItemId, Model.ItemName, Model.IndentCode);
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
        [Route("api/EMRAPI/GetOPMedicines")]
        public IHttpActionResult GetOPMedicines([FromBody] GetOPMedicines Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetOPMedicines(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate, Model.PreviousMedication);
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
        [Route("api/EMRAPI/GetPatientServices")]
        public IHttpActionResult GetPatientServices([FromBody] GetPatientServices Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientServices(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetDietOrderDetailInNote")]
        public IHttpActionResult GetDietOrderDetailInNote([FromBody] GetDietOrderDetailInNote Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDietOrderDetailInNote(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetDoctorReferral")]
        public IHttpActionResult GetDoctorReferral([FromBody] GetDoctorReferral Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDoctorReferral(Model.UserId, Model.FacilityId, Model.RegistrationNo, Model.EncounterId, Model.EncounterType, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetPatientImmunization")]
        public IHttpActionResult GetPatientImmunization([FromBody] GetPatientImmunization Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                //outputresult = objcls.GetPatientImmunization(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate, Model.ScheduleId, Model.ImminizationId);
                outputresult = objcls.GetPatientImmunization(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.ScheduleId, Model.ImminizationId);
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
        [Route("api/EMRAPI/GetEncounterFollowUpAppointment")]
        public IHttpActionResult GetEncounterFollowUpAppointment([FromBody] GetEncounterFollowUpAppointment Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEncounterFollowUpAppointment(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetDoctorProgressNote")]
        public IHttpActionResult GetDoctorProgressNote([FromBody] GetDoctorProgressNote Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDoctorProgressNote(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.DoctorId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetPatientDailyInjections")]
        public IHttpActionResult GetPatientDailyInjections([FromBody] GetPatientDailyInjections Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDailyInjections(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
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
        [Route("api/EMRAPI/GetLabTestResultForNote")]
        public IHttpActionResult GetLabTestResultForNote([FromBody] GetLabTestResultForNote Model)
        {
            BaseC.clsLISLabOther objcls = new BaseC.clsLISLabOther(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetLabTestResultForNote(Model.HospitalLocationId, Model.FacilityId, Model.LabNo, Model.EncounterId, Model.RegistrationId, Model.UserId, Model.IsShowAllParameters);
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
        [Route("api/EMRAPI/GetDiagLabSensitivityResultForNote")]
        public IHttpActionResult GetDiagLabSensitivityResultForNote([FromBody] GetDiagLabSensitivityResultForNote Model)
        {
            BaseC.clsLISLabOther objcls = new BaseC.clsLISLabOther(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDiagLabSensitivityResultForNote(Model.DiagSampleId, Model.ResultId);
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
        [Route("api/EMRAPI/GetVisitNotesPharmacy")]
        public IHttpActionResult GetVisitNotesPharmacy([FromBody] GetVisitNotesPharmacy Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetVisitNotesPharmacy(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/GetDoctorSpecialisation")]
        public IHttpActionResult GetDoctorSpecialisation([FromBody] GetDoctorSpecialisation Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDoctorSpecialisation(Model.DoctorId);
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
        [Route("api/EMRAPI/getEMRDoctorWisePatientNote")]
        public IHttpActionResult getEMRDoctorWisePatientNote([FromBody] getEMRDoctorWisePatientNote Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.getEMRDoctorWisePatientNote(Model.RegistrationId, Model.DoctorId);
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
        [Route("api/EMRAPI/GetPatientVisits")]
        public IHttpActionResult GetPatientVisits([FromBody] GetPatientVisits Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRPatientVisits(Model.HospitalLocationId, Model.RegistrationId, Model.ViewType, Model.DoctorId,
                                    Model.TemplateId, Model.FromDate, Model.ToDate);

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
        [Route("api/EMRAPI/SaveEMRDoctorWisePatientNote")]
        public IHttpActionResult SaveEMRDoctorWisePatientNote([FromBody] SaveEMRDoctorWisePatientNote Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveEMRDoctorWisePatientNote(Model.RegistrationId, Model.DoctorId, Model.PatientNotes, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetPatientDiagnosisOPIP")]
        public IHttpActionResult GetPatientDiagnosisOPIP([FromBody] GetPatientDiagnosisOPIP Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDiagnosisOPIP(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/GetIPServiceOrderDetails")]
        public IHttpActionResult GetIPServiceOrderDetails([FromBody] GetIPServiceOrderDetails Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetIPServiceOrderDetails(Model.iHospitalLocationId, Model.iFacilityId, Model.iRegistrationId, Model.iEncounterId, Model.iBillId, Model.iDepartmentId, Model.chrDepartmentType);
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
        [Route("api/EMRAPI/GetServiceDetailsForPreAuth")]
        public IHttpActionResult GetServiceDetailsForPreAuth([FromBody] GetIPServiceOrderDetails Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetServiceDetailsForPreAuth(Model.iHospitalLocationId, Model.iFacilityId, Model.iRegistrationId, Model.iEncounterId);

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
        [Route("api/EMRAPI/GetEMROrderPatientDiagnosis")]
        public IHttpActionResult GetEMROrderPatientDiagnosis([FromBody] GetPatientDiagnosis Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDiagnosis(Model.RegistrationId, Model.HospitalLocationId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetsetPAtDAta")]
        public IHttpActionResult GetsetPAtDAta([FromBody] PreAuth Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetsetPAtDAta(Model.RegistrationId, Model.LoginSpecialisationID);
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
        [Route("api/EMRAPI/dataPayer")]
        public IHttpActionResult dataPayer([FromBody] PreAuth Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDataPayer(Model.EncounterId);
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
        [Route("api/EMRAPI/PreAuthPatientDtl")]
        public IHttpActionResult PreAuthPatientDtl([FromBody] PreAuth Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.PreAuthPatientDtl(Model.Query);
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
        [Route("api/EMRAPI/SavePreAuth")]
        public IHttpActionResult SavePreAuth([FromBody] PreAuthInputs Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                string json = objcls.SavePreAuth(common.myInt(Model.rdlOPIP), Model.chkctg, Model.txtCTGremark, Model.chkpregenent, Model.rdLMP, Model.ddlx, Model.chkwrokrelated, Model.chksportrelated, Model.chkprofessionalSport, Model.chkRTArelated, Model.txtAlcohalintake, Model.txtreatmentdetail, Model.txtClinicalFindings, Model.txtTreatmentPlan, Model.txtremark, Model.RegistrationID, Model.EncounterId, Model.UserID, Model.FacilityId);
                json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
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



        // LabResult & Radiology Results

        [HttpPost]
        [Route("api/EMRAPI/GetStation")]
        public IHttpActionResult GetStation([FromBody] EMRModel Model)
        {
            BaseC.clsLISSampleReceivingStation objcls = new BaseC.clsLISSampleReceivingStation(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetStation(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/getDiagPatientLabServices")]
        public IHttpActionResult getDiagPatientLabServices([FromBody] EMRModel Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getDiagPatientLabServices(Model.HospitalLocationId, Model.iFacilityId, Model.dfromDate, Model.dtoDate, Model.cRegistrationNo, Model.cEncounterId);
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
        [Route("api/EMRAPI/DiagHISPACSIntegrationSetup")]
        public IHttpActionResult DiagHISPACSIntegrationSetup([FromBody] EMRModel Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string result;
            try
            {
                result = (string)objDl.ExecuteScalar(CommandType.Text, Model.Query);
                return Ok(result);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("api/EMRAPI/getPatientLabResultHistory")]
        public IHttpActionResult getPatientLabResultHistory([FromBody] PatientLabResultHistory Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientLabResultHistory(Model.iLoginFacilityId, Model.iHostId, Model.fromDate, Model.toDate, Model.iRegNo, Model.iProviderId, Model.iPageSize, Model.iPageNo, Model.AbnormalResult, Model.CriticalResult, Model.iStatusId, Model.FacilityId, Model.chvEncounterNo, Model.Source, Model.SubDeptId, Model.ServiceId, Model.StationId, Model.ResultStatus, Model.DischargeSummary);
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
        [Route("api/EMRAPI/GetDiagLabResultDynamicGrid")]
        public IHttpActionResult GetDiagLabResultDynamicGrid([FromBody] GetDiagLabResultDynamicGrid Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDiagLabResultDynamicGrid(Model.iLoginFacilityId, Model.iHostId, Model.fromDate, Model.toDate,
                    Model.iRegNo, Model.iProviderId, Model.iPageSize, Model.iPageNo, Model.AbnormalResult,
                    Model.CriticalResult, Model.iStatusId, Model.FacilityId, Model.chvEncounterNo, Model.xmlService,
                    Model.xmlFieldId, Model.chvEncounterId, Model.cStationId, Model.iLoginId);

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
        [Route("api/EMRAPI/getLabNoInvFormatsHistory")]
        public IHttpActionResult getLabNoInvFormatsHistory([FromBody] getLabNoInvFormatsHistory Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getLabNoInvFormatsHistory(Model.Source, Model.iLoginFacilityId, Model.iDiagSampleId, Model.iServiceID, Model.StatusCode, Model.iHospID);
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

        // Groth Charts

        [HttpPost]
        [Route("api/EMRAPI/GetAgeinDays")]
        public IHttpActionResult GetAgeinDays([FromBody] GrowthChart Model)
        {
            BaseC.GrowthChart objcls = new BaseC.GrowthChart(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetAgeinDays(Model.HospitalLocationId, Model.RegistrationId);
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
        [Route("api/EMRAPI/BindGrowthChart2Yto20Y")]
        public IHttpActionResult BindGrowthChart2Yto20Y([FromBody] GrowthChart Model)
        {
            BaseC.GrowthChart objcls = new BaseC.GrowthChart(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindGrowthChart2Yto20Y(Model.HospitalLocationId, Model.RegistrationId, Model.DisplayName, Model.FromAge, Model.ToAge, Model.DayType, Model.GrowthChartId);
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
        [Route("api/EMRAPI/BindGrowthChart")]
        public IHttpActionResult BindGrowthChart([FromBody] GrowthChart Model)
        {
            BaseC.GrowthChart objcls = new BaseC.GrowthChart(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindGrowthChart(Model.FacilityId);
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


        // Allergy popup

        [HttpPost]
        [Route("api/EMRAPI/GetPatientRegistrationNo")]
        public IHttpActionResult GetPatientRegistrationNo([FromBody] Allergy Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.GetPatientRegistrationNo(Model.RegistrationId);
                return Ok(outputresult);
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
        [Route("api/EMRAPI/getFacilityInterfaceDetails")]
        public IHttpActionResult getFacilityInterfaceDetails([FromBody] Allergy Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            BaseC.clsEMR.enumCIMSorVIDALInterfaceFor varEnum = (BaseC.clsEMR.enumCIMSorVIDALInterfaceFor)Enum.ToObject(typeof(BaseC.clsEMR.enumCIMSorVIDALInterfaceFor), Model.interfaceFor);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getFacilityInterfaceDetails(Model.HospId, Model.FacilityId, varEnum);
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


        // 
        [HttpPost]
        [Route("api/EMRAPI/getAllergies")]
        public IHttpActionResult getAllergies([FromBody] Allergy Model)
        {
            BaseC.Dashboard objcls = new BaseC.Dashboard();
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getAllergies(Model.RegistrationId, 1);
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
        [Route("api/EMRAPI/SavePatientAllergy")]
        public IHttpActionResult SavePatientAllergy([FromBody] AllergySaveInputs Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SavePatientAllergy(Model.HospId, Model.FacilityId, Model.PageId, Model.RegistrationId, Model.EncounterId, Model.xmlDrugAllergyDetails, Model.xmlOtherAllergyDetails, Model.EncodedBy, Model.IsNKDA, Model.IsShowNote);
                return Ok(outputresult);
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

        //deActivatePatientAllergy

        [HttpPost]
        [Route("api/EMRAPI/deActivatePatientAllergy")]
        public IHttpActionResult deActivatePatientAllergy([FromBody] Allergy Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.deActivatePatientAllergy(Model.HospId, Model.FacilityId, Model.PageId, Model.Flag, Model.RegistrationId, Model.EncounterId, Model.Id, Model.CancelRemarks, Model.EncodedBy);
                return Ok(outputresult);
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

        // updateshownote

        [HttpPost]
        [Route("api/EMRAPI/updateshownote")]
        public IHttpActionResult updateshownote([FromBody] Allergy Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.updateshownote(Model.showNotesVal, Model.PageId, Model.EncounterId, Model.RegistrationId);
                return Ok(outputresult);
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
        // getAllergyMaster
        [HttpPost]
        [Route("api/EMRAPI/getAllergyMaster")]
        public IHttpActionResult getAllergyMaster([FromBody] Allergy Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getAllergyMaster(Model.HospId, Model.ItemName, Model.AllergyTypeId);
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
        //getAllergyTypeMaster

        [HttpPost]
        [Route("api/EMRAPI/getAllergyTypeMaster")]
        public IHttpActionResult getAllergyTypeMaster([FromBody] Allergy Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getAllergyTypeMaster(Model.HospId);
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

        // GetGenericDetails
        [HttpPost]
        [Route("api/EMRAPI/GetGenericDetails")]
        public IHttpActionResult GetGenericDetails([FromBody] GenericDetail Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetGenericDetails(Model.GenericId, Model.GenericName, Model.Active, Model.HospId, Model.EncodedBy);
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

        // getItemsWithStock

        [HttpPost]
        [Route("api/EMRAPI/getItemsWithStock")]
        public IHttpActionResult getItemsWithStock([FromBody] Pharmacy Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getItemsWithStock(Model.HospId, Model.StoreId, Model.ItemId, Model.ItemBrandId, Model.GenericId, Model.UserId, Model.FacilityId, Model.SupplierId, Model.ItemName, Model.WithStockOnly, Model.NHISDrugsList);
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
        [Route("api/EMRAPI/Inline")]
        public IHttpActionResult Inline([FromBody] EMRModel Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string result;
            try
            {
                result = (string)objDl.ExecuteScalar(CommandType.Text, Model.Query);
                return Ok(result);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

            }
        }

        [HttpPost]
        [Route("api/EMRAPI/AllergyPatientDtl")]
        public IHttpActionResult AllergyPatientDtl([FromBody] PreAuth Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.PreAuthPatientDtl(Model.Query);
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
        // getItemMaster

        [HttpPost]
        [Route("api/EMRAPI/getItemMaster")]
        public IHttpActionResult getItemMaster([FromBody] Pharmacy Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getItemMaster(Model.ItemId, Model.ItemNo, Model.ItemsearchName, Model.active, Model.HospId, Model.UserId);
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
        [Route("api/EMRAPI/AuditCommonAccess")]
        public IHttpActionResult AuditCommonAccess([FromBody] Security Model)
        {
            BaseC.Security objcls = new BaseC.Security(sConString);

            try
            {
                objcls.AuditCommonAccess(Model.iHospID, Model.iFacilityID, Model.iRegId, Model.iEncountId, Model.iPageId, Model.iTemplateId, Model.iEncodedBy, Model.iEmpId, Model.iAuditStatus, Model.chvIPAddres);
                return Ok("Saved");
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


        // Provisional Diagnosis popup


        [HttpPost]
        [Route("api/EMRAPI/GetDoctorList")]
        public IHttpActionResult GetDoctorList([FromBody] ProvisionalDiagnosis Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDoctorList(Model.HospitalId, Model.intSpecialisationId, Model.FacilityId);
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
        [Route("api/EMRAPI/EMRSavePatientProvisionalDiagnosis")]
        public IHttpActionResult EMRSavePatientProvisionalDiagnosis([FromBody] saveDiagnosisInputs Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.EMRSavePatientProvisionalDiagnosis(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId,
                                        Model.ProvisionalDiagnosis, Model.DiagnosisSearchId, Model.UserId, Model.ProvisionalDiagnosisId,
                                        Model.ProviderId, Model.ChangeDate, Model.Remarks, Model.bAdmitting, Model.bProvisional,
                                        Model.bFinal, Model.bChronic, Model.bDischarge);

                return Ok(outputresult);
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
        [System.Web.Http.Route("api/EMRAPI/DeleteFavouriteProvDiagnosis")]
        public IHttpActionResult DeleteFavouriteProvDiagnosis([FromBody] DeletePatientProvisionalDiagnosis Model)
        {

            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            Hashtable HshOut = new Hashtable();
            try
            {
                bool outresult = objcls.DeleteFavouriteProvDiagnosis(Model.idoctorId, Model.id);
                return Ok(JsonConvert.SerializeObject(outresult));

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshOut = null;
                objcls = null;
            }

        }
        //BindFavouriteProvDiagnosis

        [HttpPost]
        [Route("api/EMRAPI/BindFavouriteProvDiagnosis")]
        public IHttpActionResult BindFavouriteProvDiagnosis([FromBody] ProvisionalDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindFavouriteProvDiagnosis(Model.strSearchCriteria, Model.DoctorID);
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
        [Route("api/EMRAPI/SaveFavouriteProvDiagnosis")]
        public IHttpActionResult SaveFavouriteProvDiagnosis([FromBody] ProvisionalDiagnosis Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveFavouriteProvDiagnosis(Model.DoctorID, Model.DiagnosisDescription, Model.userid);
                return Ok(outputresult);
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

        // Slip
        [HttpPost]
        [Route("api/EMRAPI/Specialization")]
        public IHttpActionResult Specialization([FromBody] EMRModel Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorSpecialisation");
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {

            }
        }

        //fillDoctorCombo

        [HttpPost]
        [Route("api/EMRAPI/fillDoctorCombo")]
        public IHttpActionResult fillDoctorCombo([FromBody] Referrals Model)
        {
            BaseC.Hospital objcls = new BaseC.Hospital(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.fillDoctorCombo(Model.HospitalId, Model.intSpecialisationId, Model.FacilityId);
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
        [Route("api/EMRAPI/CheckGenerateeOPRxXml")]
        public IHttpActionResult CheckGenerateeOPRxXml([FromBody] CheckGenerateeOPRxXml Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.CheckGenerateeOPRxXml(Model.IndentId, Model.FacilityId);
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
        [Route("api/EMRAPI/GetStoreToChangefromWard")]
        public IHttpActionResult GetStoreToChangefromWard([FromBody] GetStoreToChangefromWard Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetStoreToChangefromWard(Model.HospitalLocationId, Model.GroupId, Model.FacilityId, Model.DocumentType, Model.DepartmentId);
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
        [Route("api/EMRAPI/GetDoctorPrescriptionInstructions")]
        public IHttpActionResult GetDoctorPrescriptionInstructions([FromBody] GetDoctorPrescriptionInstructions Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRDoctorPrescriptionInstructions(Model.DoctorId);
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
        [Route("api/EMRAPI/GetPrescriptionRemarks")]
        public IHttpActionResult GetPrescriptionRemarks([FromBody] GetPrescriptionRemarks Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRPrescriptionRemarks(Model.RemarkId, Model.HospitalLocationId, Model.FacilityId, Model.Active, Model.DoctorId);
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
        [Route("api/EMRAPI/CheckPrimaryDiagnosisForEncounter")]
        public IHttpActionResult CheckPrimaryDiagnosisForEncounter([FromBody] CheckPrimaryDiagnosisForEncounter Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            bool outputresult = false;
            try
            {
                outputresult = objcls.CheckDiagnosisPrimaryForPatient(common.myInt(Model.RegistraionId));
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveMedicineOrderOP")]
        public IHttpActionResult SaveMedicineOrderOP([FromBody] SaveMedicineOrderOP Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SaveMedicineOrderOP(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.IndentType,
                    Model.StoreId, Model.DoctorId, Model.IsPregnant, Model.IsBreastFeeding, Model.xmlItems, Model.xmlItemDetail, Model.xmlPatientAlerts, Model.UserId,
                    Model.xmlFrequencyTime, Model.IsConsumable, Model.xmlUnApprovedPrescriptionIds, Model.intDrugAdminIn, "");
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                // outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveMedicineOrderIP")]
        public IHttpActionResult SaveMedicineOrderIP([FromBody] SaveMedicineOrderIP Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SaveMedicineOrderIP(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.IndentType,
                    Model.StoreId, Model.DoctorId, Model.xmlItems, Model.xmlItemDetail, Model.Remarks, Model.DrugOrderType, Model.UserId,
                    Model.IsConsumable, Model.xmlFrequencyTime, Model.xmlUnApprovedPrescriptionIds);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/IsDOctorEprescriptionEnabled")]
        public IHttpActionResult IsDOctorEprescriptionEnabled([FromBody] IsDOctorEprescriptionEnabled Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.IsDOctorEprescriptionEnabled(Model.DoctorId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/UpdateDHARefNo")]
        public IHttpActionResult UpdateDHARefNo([FromBody] UpdateDHARefNo Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            //int outputresult = 0;
            try
            {
                objcls.UpdateDHARefNo(Model.DHARefNo, Model.IndentId);
                string json = JsonConvert.SerializeObject("sucess", Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetClinicianLoginforErx")]
        public IHttpActionResult GetClinicianLoginforErx([FromBody] GetClinicianLoginforErx Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetClinicianLoginforErx(Model.DoctorId);
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
        [Route("api/EMRAPI/deleteUnApprovedPrescriptions")]
        public IHttpActionResult deleteUnApprovedPrescriptions([FromBody] deleteUnApprovedPrescriptions Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.deleteUnApprovedPrescriptions(Model.UnAppPrescriptionId, Model.EncounterId, Model.UserId, Model.FacilityId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getScreeningParameters")]
        public IHttpActionResult getScreeningParameters([FromBody] getScreeningParameters Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getScreeningParameters(Model.EncounterId, Model.RegistrationId);
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
        [Route("api/EMRAPI/getFavoriteDrugWithStock")]
        public IHttpActionResult getFavoriteDrugWithStock([FromBody] getFavoriteDrugWithStock Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getFavoriteDrugWithStockV3(Model.HospitalLocationId, Model.FacilityId, Model.UserId, Model.ItemId,
                                                        Model.GenericId, Model.DoctorId, Model.FormularyType, Model.ItemName);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getPreviousMedicinesNew")]
        public IHttpActionResult getPreviousMedicinesNew([FromBody] getPreviousMedicinesNew Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPreviousMedicinesNew(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.IndentId,
                                                        Model.ItemId, Model.PreviousMedication, Model.ItemName, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/getPreviousMedicationOP")]
        public IHttpActionResult getPreviousMedicationOP([FromBody] getPreviousMedicationOP Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPreviousMedicationOP(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId,
                                    Model.RegistrationId, Model.IndentId, Model.ItemId, Model.PreviousMedication, Model.ItemName,
                                    Model.FromDate, Model.ToDate, Model.UserId);
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
        [Route("api/EMRAPI/getOPMedicinesNew")]
        public IHttpActionResult getOPMedicinesNew([FromBody] getOPMedicinesNew Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getOPMedicinesNew(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.RegistrationId, Model.IndentId,
                                                        Model.ItemId, Model.PreviousMedication, Model.ItemName, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/DeleteFavoriteMedicine")]
        public IHttpActionResult DeleteFavoriteMedicine([FromBody] DeleteFavoriteMedicine Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.DeleteFavoriteDrugs(Model.DoctorId, Model.ItemId, Model.FormularyType, Model.UserId, Model.GenericId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveFavoriteMedicine")]
        public IHttpActionResult SaveFavoriteMedicine([FromBody] SaveFavoriteMedicine Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveFavouriteDrugs(Model.DoctorId, Model.ItemId, Model.FormularyType, Model.UserId,
                                        Model.DoseId, Model.UnitId, Model.StrengthId, Model.FormulationId, Model.RouteId, Model.FrequencyId, Model.Duration,
                                        Model.DurationType, Model.FoodRelationshipId, Model.StrengthValue, Model.Instructions, Model.GenericId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getMedicinesOPList")]
        public IHttpActionResult getMedicinesOPList([FromBody] getMedicinesOPList Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getMedicinesOPList(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.IndentId, Model.RegistrationId, Model.IndentNo);
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
        [Route("api/EMRAPI/SaveDhaError")]
        public IHttpActionResult SaveDhaError([FromBody] SaveDhaError Model)
        {
            BaseC.clsErx objcls = new BaseC.clsErx(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                objcls.SaveDhaError(Model.IndentId, Model.Error, Model.FormName, Model.MethodName, Model.DHAMethodName, Model.Facilityid, Model.ModuleId, Model.ErrorMessage);
                string json = JsonConvert.SerializeObject("SUCCESS", Formatting.Indented);
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
        [Route("api/EMRAPI/SaveDoctorPrescriptionInstructions")]
        public IHttpActionResult SaveDoctorPrescriptionInstructions([FromBody] SaveDoctorPrescriptionInstructions Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveEMRDoctorPrescriptionInstructions(Model.DoctorId, Model.InstructionText, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getDiagnosis")]
        public IHttpActionResult getDiagnosis([FromBody] getDiagnosis Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getDiagnosis(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/GetEMRExistingMedicationOrder")]
        public IHttpActionResult GetEMRExistingMedicationOrder([FromBody] GetEMRExistingMedicationOrder Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.GetEMRExistingMedicationOrder(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.RegistrationId, Model.ItemId, Model.OPIP);
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
        [Route("api/EMRAPI/getGenericRouteUnitOfSelectedDrug")]
        public IHttpActionResult getGenericRouteUnitOfSelectedDrug([FromBody] getGenericRouteUnitOfSelectedDrug Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getGenericRouteUnitOfSelectedDrug(Model.ItemId, Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/EMRAPI/getDrugAllergiesInterfaceCode")]
        public IHttpActionResult getDrugAllergiesInterfaceCode([FromBody] getDrugAllergiesInterfaceCode Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getDrugAllergiesInterfaceCode(Model.HospitalLocationId, Model.RegistrationId);
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
        [Route("api/EMRAPI/getUnApprovedPrescriptions")]
        public IHttpActionResult getUnApprovedPrescriptions([FromBody] getUnApprovedPrescriptions Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getUnApprovedPrescriptions(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.UserId);
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
        [Route("api/EMRAPI/getCopyPreviousMedicinesOP")]
        public IHttpActionResult getCopyPreviousMedicinesOP([FromBody] getCopyPreviousMedicinesOP Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getCopyPreviousMedicinesOP(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId);
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
        [Route("api/EMRAPI/SaveUnApprovedPrescriptions")]
        public IHttpActionResult SaveUnApprovedPrescriptions([FromBody] SaveUnApprovedPrescriptions Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SaveUnApprovedPrescriptions(Model.UnAppPrescriptionId, Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.IndentId, Model.IndentNo,
                        Model.IndentDate, Model.IndentTypeId, Model.IndentType, Model.GenericId, Model.GenericName, Model.ItemId, Model.ItemName,
                        Model.CustomMedication, Model.FrequencyId, Model.FrequencyName, Model.Frequency, Model.Dose, Model.Duration,
                        Model.DurationText, Model.Instructions, Model.UnitId, Model.UnitName, Model.cType, Model.StartDate, Model.EndDate,
                        Model.CIMSItemId, Model.CIMSType, Model.VIDALItemId, Model.XMLData, Model.PrescriptionDetail, Model.FormulationId,
                        Model.FormulationName, Model.RouteId, Model.RouteName, Model.StrengthId, Model.StrengthValue, Model.Qty, Model.FoodRelationship,
                        Model.FoodRelationshipID, Model.ReferanceItemId, Model.ReferanceItemName, Model.DoseTypeId, Model.DoseTypeName, Model.NotToPharmacy,
                        Model.IsInfusion, Model.IsInjection, Model.IsStop, Model.IsCancel, Model.Volume, Model.VolumeUnitId, Model.TotalVolume,
                        Model.InfusionTime, Model.TimeUnit, Model.FlowRate, Model.FlowRateUnit, Model.VolumeUnit, Model.XmlVariableDose,
                        Model.IsOverride, Model.OverrideComments, Model.OverrideCommentsDrugToDrug, Model.OverrideCommentsDrugHealth,
                        Model.XMLFrequencyTime, Model.IsSubstituteNotAllowed, Model.ICDCode, Model.UserId);
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
        [Route("api/EMRAPI/StopCancelPreviousMedication")]
        public IHttpActionResult StopCancelPreviousMedication([FromBody] StopCancelPreviousMedication Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.StopCancelPreviousMedication(Model.iHospitalLocationId, Model.iFacilityId, Model.iIndentId, Model.iItemId, Model.UserId,
                                                      Model.iRegistrationId, Model.iEncounterId, Model.bCancelStop, Model.sStopRemarks,
                                                      Model.OPIP, Model.IndentDetailsId, Model.GenericId, Model.StopBy);
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
        [Route("api/EMRAPI/GenerateeOPRxXml")]
        public IHttpActionResult GenerateeOPRxXml([FromBody] GenerateeOPRxXml Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GenerateeOPRxXml(Model.HospitalLocationId, Model.FacilityId, Model.IndentId, Model.DispositionFlag);
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
        [Route("api/EMRAPI/smsSetup")]
        public IHttpActionResult smsSetup(EMRModel Model)
        {
            BaseC.SmsClass smsSetup = new BaseC.SmsClass(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = smsSetup.GetSMSSetup(Model.HospitalLocationId, Model.iFacilityId);
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
                //objcls = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/GetSmsDetails")]
        public IHttpActionResult GetSmsDetails(EMRModel Model)
        {
            BaseC.SmsClass smsSetup = new BaseC.SmsClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = smsSetup.GetSmsDetails(Model.HospitalLocationId, Model.doctorid, Convert.ToInt16(Model.cEncounterId));
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
                //objcls = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/GetEMRData")]  // Inline Query
        public IHttpActionResult GetEMRData(EMRModel Model)
        {
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = dl.FillDataSet(CommandType.Text, Model.Query);

                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //ds.Dispose();
                dl = null;
            }
        }



        [HttpPost]
        [Route("api/EMRAPI/ExecuteQuery")]  // Inline ExecuteNonQuery
        public IHttpActionResult ExecuteQuery(EMRModel Model)
        {
            DAL.DAL objcls = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int outputresult;
            try
            {
                outputresult = objcls.ExecuteNonQuery(CommandType.Text, Model.Query);

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

        // Emr Single Screen UnSaved Data 

        [HttpPost]
        [Route("api/EMRAPI/getEMRSingleScreenDataInTransitList")]
        public IHttpActionResult getEMRSingleScreenDataInTransitList([FromBody] EMRModel Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRSingleScreenDataInTransitList(Model.iFacilityId, Model.UserId);
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
        [Route("api/EMRAPI/InActiveSingleScreenDataInTransit")]
        public IHttpActionResult InActiveSingleScreenDataInTransit([FromBody] InActiveSingleScreenDataInTransit Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            int outputresult;
            try
            {
                outputresult = objcls.InActiveSingleScreenDataInTransit(Model.TransitId, Model.Userid);
                return Ok(outputresult);
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
        // GetEMRFileRequest
        [HttpPost]
        [Route("api/EMRAPI/GetEMRFileRequest")]
        public IHttpActionResult GetEMRFileRequest([FromBody] GetEMRFileRequest Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {

                if (Model.RegistrationNo == 0)
                {
                    outputresult = objcls.GetEMRFileRequest(Model.UserId, Model.RegistrationId, Model.iLoginFacilityId, Model.EncounterId, Model.fromdate, Model.todate);

                }
                else
                {
                    outputresult = objcls.GetEMRFileRequest(Model.UserId, Model.RegistrationId, Model.iLoginFacilityId, Model.EncounterId, Model.fromdate, Model.todate, Model.RegistrationNo, Model.EncounterNo, Model.PatientName, Model.RequestedBy, Model.RequestStatus);

                }

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
        [Route("api/EMRAPI/GetReferralDetailForSerach")]
        public IHttpActionResult GetReferralDetailForSerach([FromBody] GetReferralDetailForSerach Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetReferralDetailForSerach(Model.UserId, Model.EncId, Model.EncounterType, Model.FacilityId, Model.RegistrationNo, Model.ConcludeReferral, Model.ReferralId, Model.PatientName, Model.ReferralType, Model.FromDate, Model.ToDate);
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
        [Route("api/EMRAPI/SaveProvisionalDiagnosisSearchCodes")]
        public IHttpActionResult SaveProvisionalDiagnosisSearchCodes([FromBody] SaveProvisionalDiagnosisSearchCodes Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.EMRSaveProvisionalDiagnosisSearchCodes(Model.HospitalLocationId, Model.DiagnosisSearchId, Model.DiagnosisSearchCode, Model.Active, Model.UserId, Model.KeywordType);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveDiagnosisStaus")]
        public IHttpActionResult SaveDiagnosisStaus([FromBody] SaveDiagnosisStaus Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveDiagnosisStaus(Model.StatusId, Model.HospitalLocationId, Model.StatusName, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetFavoriteSentences")]
        public IHttpActionResult GetFavoriteSentences([FromBody] GetFavoriteSentences Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFavoriteSentences(Model.DoctorId, Model.SearchCriteria);
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
        [Route("api/EMRAPI/DeleteFavSentence")]
        public IHttpActionResult DeleteFavSentence([FromBody] DeleteFavSentence Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.DeleteFavSentence(Model.DoctorId, Model.SentenceId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveFavSentence")]
        public IHttpActionResult SaveFavSentence([FromBody] SaveFavSentence Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveFavSentence(Model.DoctorId, Model.SentenceIds, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientDOB")]
        public IHttpActionResult GetPatientDOB([FromBody] GetPatientDOB Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.GetPatientDOB(Model.RegistrationId, Model.HospitalLocationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetImmunizationStatus")]
        public IHttpActionResult GetImmunizationStatus([FromBody] GetImmunizationStatus Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetImmunizationStatus(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/GetPatientImmunizationDueDatesforBaby")]
        public IHttpActionResult GetPatientImmunizationDueDatesforBaby([FromBody] GetPatientImmunizationDueDates Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientImmunizationDueDates(Model.HospitalLocationId, Model.RegistrationId, Model.DateOfBirth);
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

            return Ok();
        }

        [HttpPost]
        [Route("api/EMRAPI/GetPatientImmunizationDueDatesDashbord")]
        public IHttpActionResult GetPatientImmunizationDueDatesDashbord([FromBody] PatientDiagnosisInputParam param)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", param.Registraionid);
            hshInput.Add("@inyHospitalLocationID", param.HospitalLocationid);
            hshInput.Add("@dob", param.dob);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPatientImmunizationDueDates", hshInput);
            List<PatientImmunizationOutParam> OutParam = new List<PatientImmunizationOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PatientImmunizationOutParam model = new PatientImmunizationOutParam();
                        model.ImmunizationId = Convert.ToInt16(ds.Tables[0].Rows[i]["ImmunizationID"]);
                        model.ImmunizationName = ds.Tables[0].Rows[i]["ImmunizationName"].ToString();
                        model.Status = ds.Tables[0].Rows[i]["GivenStatus"].ToString();
                        model.Duedate = ds.Tables[0].Rows[i]["ImmunizationDueDate"].ToString();
                        model.GivenDate = ds.Tables[0].Rows[i]["GivenDate"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);

        }
        [HttpPost]
        [Route("api/EMRAPI/InActivePatientImmunization")]
        public IHttpActionResult InActivePatientImmunization([FromBody] InActivePatientImmunization Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.InActivePatientImmunization(Model.ImmunizationId, Model.UserId, Model.EncodedDate, Model.CancelRemarks, Model.Type);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetEncounterFacilityForImmunization")]
        public IHttpActionResult GetEncounterFacilityForImmunization([FromBody] GetEncounterFacilityForImmunization Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.GetEncounterFacilityForImmunization(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetImmunizationGivenBy")]
        public IHttpActionResult GetImmunizationGivenBy([FromBody] GetImmunizationGivenBy Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetImmunizationGivenBy(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/SavePatientImmunization")]
        public IHttpActionResult SavePatientImmunization([FromBody] SavePatientImmunization Model)
        {
            BaseC.EMRImmunization objcls = new BaseC.EMRImmunization(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SavePatientImmunization(Model.Id, Model.HospitalLocationId, Model.FacilityId, Model.SchedulerId, Model.ImmunizationId, Model.RegistrationId,
                Model.EncounterId, Model.GivenDate, Model.GivenBy, Model.BatchNo, Model.IsRejectedByPatient, Model.IsVaccineGivenByOutsider, Model.Remarks,
                Model.UserId, Model.PageId, Model.BrandId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/stopPrescription")]
        public IHttpActionResult stopPrescription([FromBody] stopPrescription Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.stopPrescription(Model.DetailsIds, Model.CancelReasonId, Model.UserId, Model.EncounterId, Model.OPIP);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetTemplateGroup")]
        public IHttpActionResult GetTemplateGroup([FromBody] GetTemplateGroup Model)
        {
            //BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);

            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.EMRGetTemplateGroup(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/GetFavouriteTemplates")]
        public IHttpActionResult GetFavouriteTemplates([FromBody] GetFavouriteTemplates Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFavouriteTemplates(Model.DoctorId);
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
        [Route("api/EMRAPI/GetTemplateGroupDetails")]
        public IHttpActionResult GetTemplateGroupDetails([FromBody] GetTemplateGroupDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTemplateGroupDetails(Model.GroupId);
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
        [Route("api/EMRAPI/SaveFavouriteTemplates")]
        public IHttpActionResult SaveFavouriteTemplates([FromBody] SaveFavouriteTemplates Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            int outputresult = 1;
            try
            {
                outputresult = objcls.SaveFavouriteTemplates(Model.DoctorId, Model.TemplateId, Model.UserId, Model.TranType);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/BindGrowthChartFor2Yto20YHTWT")]
        public IHttpActionResult BindGrowthChartFor2Yto20YHTWT([FromBody] BindGrowthChartFor2Yto20YHTWT Model)
        {
            BaseC.GrowthChart objcls = new BaseC.GrowthChart(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.BindGrowthChartFor2Yto20YHTWT(Model.HospitalLocationId, Model.RegistrationId);
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
        [Route("api/EMRAPI/SaveReferralSlip")]
        public IHttpActionResult SaveReferralSlip([FromBody] SaveReferralSlip Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveReferralSlip(Model.ReferralReplyId, Model.ReferralId, Model.HospitalLocationId,
                Model.EncounterId, Model.Referraldate, Model.ReferDoctorId, Model.Doctornote, Model.Urgent, Model.status,
                Model.UserId, Model.ReferralSlip, Model.Active, Model.DoctorReferral, Model.SpecializationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetReferralDetail")]
        public IHttpActionResult GetReferralDetail([FromBody] GetReferralDetail Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetReferralDetail(Model.UserId, Model.EncounterId, Model.EncounterType,
                Model.FacilityId, Model.RegistrationNo, Model.ConcludeReferral, Model.ReferralId);
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
        [Route("api/EMRAPI/SaveReferralSlipAutoSave")]
        public IHttpActionResult SaveReferralSlipAutoSave([FromBody] SaveReferralSlipAutoSave Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveReferralSlipAutoSave(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId,
                Model.EncounterId, Model.SpecializationId, Model.ReferDoctorId, Model.Urgent, Model.Doctornote, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetReferralDetailAutoSave")]
        public IHttpActionResult GetReferralDetailAutoSave([FromBody] GetReferralDetailAutoSave Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetReferralDetailAutoSave(Model.RegistrationId, Model.FacilityId, Model.EncounterId,
                Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/DeleteEMRTemplateResultSet")]
        public IHttpActionResult DeleteEMRTemplateResultSet([FromBody] DeleteEMRTemplateResultSet Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            int outputresult;
            try
            {
                outputresult = objcls.DeleteEMRTemplateResultSet(Model.ResultSetId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetROS")]
        public IHttpActionResult GetROS([FromBody] GetROS Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetROS(Model.HospitalLocationId, Model.RegistrationNo, Model.EncounterId);
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
        [Route("api/EMRAPI/populateAllergyType")]
        public IHttpActionResult populateAllergyType([FromBody] populateAllergyType Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.populateAllergyType(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/SaveAllergyMaster")]
        public IHttpActionResult SaveAllergyMaster([FromBody] SaveAllergyMaster Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveAllergyMaster(Model.HospitalLocationId, Model.TypeId, Model.Description, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/DeActivateAllergyMaster")]
        public IHttpActionResult DeActivateAllergyMaster([FromBody] DeActivateAllergyMaster Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            int outputresult = 2;
            try
            {
                outputresult = objcls.DeActivateAllergyMaster(Model.HospitalLocationId, Model.AllergyId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/UpdateAllergyMaster")]
        public IHttpActionResult UpdateAllergyMaster([FromBody] UpdateAllergyMaster Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            int outputresult = 2;
            byte active = 1;
            try
            {
                if (Model.IsActive)
                    active = 1;
                else
                    active = 0;

                outputresult = objcls.UpdateAllergyMaster(Model.HospitalLocationId, Model.AllergyId, Model.TypeId, Model.Description, Model.UserId, active);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/DeActivateAllergyTypeMaster")]
        public IHttpActionResult DeActivateAllergyTypeMaster([FromBody] DeActivateAllergyTypeMaster Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            int outputresult = 2;
            try
            {
                outputresult = objcls.DeActivateAllergyTypeMaster(Model.HospitalLocationId, Model.TypeId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/UpdateAllergyTypeMaster")]
        public IHttpActionResult UpdateAllergyTypeMaster([FromBody] UpdateAllergyTypeMaster Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            int outputresult = 2;
            byte active = 1;
            try
            {
                if (Model.IsActive)
                    active = 1;
                else
                    active = 0;

                outputresult = objcls.UpdateAllergyTypeMaster(Model.HospitalLocationId, Model.TypeId, Model.Description, Model.UserId, active);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/SaveAllergyTypeMaster")]
        public IHttpActionResult SaveAllergyTypeMaster([FromBody] SaveAllergyTypeMaster Model)
        {
            BaseC.EMRAllergy objcls = new BaseC.EMRAllergy(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveAllergyTypeMaster(Model.HospitalLocationId, Model.Description, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetICDCode")]
        public IHttpActionResult GetICDCode([FromBody] GetICDCode Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetICDCode(Model.ICDCode);
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
        [Route("api/EMRAPI/GetEMRDoctorPatientwise")]
        public IHttpActionResult GetEMRDoctorPatientwise([FromBody] GetEMRDoctorPatientwise Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRDoctorPatientwise(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId);
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
        [Route("api/EMRAPI/SavePatientLastEncounterData")]
        public IHttpActionResult SavePatientLastEncounterData([FromBody] SavePatientLastEncounterData Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.SavePatientLastEncounterDataDS(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FacilityId, Model.DoctorId,
                    Model.UserId, Model.TemplateIdDetails, Model.PrescriptionDetails);
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
        [Route("api/EMRAPI/GetProblemHPIDetails")]
        public IHttpActionResult GetProblemHPIDetails([FromBody] GetProblemHPIDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetProblemHPIDetails(Model.ProblemId);
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
        [Route("api/EMRAPI/SaveHPIProblems")]
        public IHttpActionResult SaveHPIProblems([FromBody] SaveHPIProblems Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveHPIProblems(Model.HospitalLocationId, Model.ProblemId, Model.OnsetDate, Model.NoOfOccurrence, Model.PriorIllnessDate, Model.RelievingFactors, Model.AggravatingFactors, Model.DeniesSymptomsText1, Model.DeniesSymptomsText2, Model.DeniesSymptomsText3, Model.DeniesSymptomsText4, Model.DeniesSymptomsText5, Model.UserId, Model.RegistrationId, Model.EncounterId, Model.FacilityId, Model.PageId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetEMRPatientAlerts")]
        public IHttpActionResult GetEMRPatientAlerts([FromBody]GetEMRPatientAlerts Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRPatientAlerts(Model.HospitalLocationId, Model.RegistrationId, Model.AlertType);
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
        [Route("api/EMRAPI/ShowPageCheck")]
        public IHttpActionResult ShowPageCheck([FromBody]ShowPageCheck Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.ShowPageCheck(Model.RegistrationId, Model.EncounterId, 0, Model.FormId, Model.EREncounterId, Model.TemplateId);
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
        [Route("api/EMRAPI/getEncounterBasedOnPrescription")]
        public IHttpActionResult getEncounterBasedOnPrescription([FromBody]getEncounterBasedOnPrescription Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.getEncounterBasedOnPrescription(Model.PrescriptionId, Model.OPIP);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/CheckUserDoctorOrNot")]
        public IHttpActionResult CheckUserDoctorOrNot([FromBody] CheckUserDoctorOrNot Model)
        {
            BaseC.EMR objEmr = new BaseC.EMR(sConString);
            try
            {
                var objDr = objEmr.CheckUserDoctorOrNot(Model.HospitalLocationId, Model.UserId);
                string json = JsonConvert.SerializeObject(objDr, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        [Route("api/EMRAPI/SaveEmailLog")]
        public IHttpActionResult SaveEmailLog([FromBody] SaveEmailLog Model)
        {

            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            /// Hashtable HshOut = new Hashtable();
            try
            {
                string json = objcls.SaveEmailLogLabResults(Model.FacilityID, Model.HospitalLocationId, Model.EmailType, Model.EMailTo, Model.PatientName, Model.RegistrationId, Model.RegistrationNo,
                                   Model.EncounterId, Model.EncounterNo, Model.LabNo, Model.SendTODoctorID, Model.SendToDoctorName, Model.AttendingDOctorID, Model.AttendingDOctorName,
                                   Model.Active, Model.EncodedBy, Model.EncodedDate);
                return Ok(json);

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
        [Route("api/EMRAPI/getHospitalSetUp")]
        public IHttpActionResult getHospitalSetUp([FromBody]getFacilityList Model)
        {
            BaseC.clsLISMaster objcls = new BaseC.clsLISMaster(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getHospitalSetUp(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/GetLabDetailsEmail")]
        public IHttpActionResult GetLabDetailsEmail([FromBody]GetLabDetailsEmail Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetLabDetailsEmail(Model.chvLabNo);
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
        [Route("api/EMRAPI/GetDoctorId")]
        public IHttpActionResult GetDoctorId([FromBody] EMRModel Model)
        {
            BaseC.DiagnosisDA objcls = new BaseC.DiagnosisDA(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.GetDoctorId(Model.HospitalLocationId, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/GetPatientAssessments")]
        public IHttpActionResult GetPatientAssessments([FromBody] GetPatientAssessments Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientAssessments(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FromDate, Model.ToDate, Model.GroupingDate);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/GetRequestService")]
        public IHttpActionResult GetRequestService([FromBody] GetRequestService Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetRequestService(Model.iHospitalLocationId, Model.iFacilityId, Model.EncounterNo, Model.iDepartmentId, Model.iSubDeptId, Model.cFromDate, Model.cToDate, Model.Source, Model.ProcedureType, Model.PatientName, Model.RegistrationNo);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getPatientDetails")]
        public IHttpActionResult getPatientDetails([FromBody] EMRModel Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getPatientDetails(Convert.ToInt16(Model.cRegistrationNo), 1, Model.HospitalLocationId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/DeletePatientServices")]
        public IHttpActionResult GetDoctorId([FromBody] DeletePatientServices Model)
        {
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.DeletePatientServices(Model.iServiceId, Model.iHospitalID, Model.iRegistrationId, Model.iEncounterId,
                                Model.iFacilityId, Model.bCancelService, Model.iPageId, Model.iEncodedby, Model.iServiceOrderDtlId,
                                Model.bRequestToDepartment);

                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
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
        [Route("api/EMRAPI/SaveProblemMasters")]
        public IHttpActionResult SaveProblemMasters([FromBody] SaveProblemMasters Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.SaveProblemMasters(Model.HospitallocationId, Model.Description, Model.UserId, Model.TableName, Model.Id);
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
        [Route("api/EMRAPI/SaveProblemMastersSeq")]
        public IHttpActionResult SaveProblemMastersSeq([FromBody] SaveProblemMastersSeq Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataTable outputresult = new DataTable();
            try
            {
                outputresult = objcls.SaveProblemMastersSeq(Model.xmlDetails, Model.UserId, Model.TableName);
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
        [Route("api/EMRAPI/UpdateICMFinalize")]
        public IHttpActionResult UpdateICMFinalize([FromBody] UpdateICMFinalize Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            //DataTable outputresult = new DataTable();
            try
            {
                //outputresult = 
                objcls.UpdateICMFinalize(Model.HospitalLocationId, Model.SummaryID, Model.IsFinalize, Model.DoctorId, Model.UserId, Model.ChangeDate);
                string json = JsonConvert.SerializeObject("Record Updated", Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetICMTemplateStyle")]
        public IHttpActionResult GetICMTemplateStyle([FromBody] GetICMTemplateStyle Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetICMTemplateStyle(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/GetICMTemplateName")]
        public IHttpActionResult GetICMTemplateName([FromBody] GetICMTemplateName Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetICMTemplateName(Model.EncounterId);
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
        [Route("api/EMRAPI/GetEMRTemplateReportSetup1")]
        public IHttpActionResult GetEMRTemplateReportSetup1([FromBody] GetEMRTemplateReportSetup1 Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRTemplateReportSetup1(Model.ReportId, Model.TemplateId, Model.DoctorId, Model.chvFlag, Model.bitActive, Model.HospitalLocationId, Model.ReportType, Model.UserId);
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
        [Route("api/EMRAPI/CancelEMRDischargeOrDeathSummary")]
        public IHttpActionResult CancelEMRDischargeOrDeathSummary([FromBody] CancelEMRDischargeOrDeathSummary Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.CancelEMRDischargeOrDeathSummary(Model.HospitalLocationId, Model.SummaryId, Model.RegistrationId, Model.EncounterId, Model.UserId, Model.CancelRemarks);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveUpdateICMPatientSummary")]
        public IHttpActionResult SaveUpdateICMPatientSummary([FromBody] SaveUpdateICMPatientSummary Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveUpdateICMPatientSummary(Model.SummaryId, Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.FormatId, Model.PatientSummary, Model.SignDoctorId,
                Model.UserId, Model.sEncodedDate, Model.DeathDate, Model.DischargeDate, Model.FacilityId, Model.synopsis, Model.Addendum, Model.IsMultiDepartmentCase, Model.CaseId, Model.xmlDepartmentIds);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/getDepartmentListOfDoctor")]
        public IHttpActionResult getDepartmentListOfDoctor([FromBody] EMRModel Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getDepartmentListOfDoctor(Model.HospitalLocationId);
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
        [Route("api/EMRAPI/getEMRDSMultiDepartmentCase")]
        public IHttpActionResult getEMRDSMultiDepartmentCase([FromBody] EMRModel Model)
        {
            BaseC.ICM objcls = new BaseC.ICM(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEMRDSMultiDepartmentCase();
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
        [Route("api/EMRAPI/CancelReferral")]
        public IHttpActionResult CancelReferral([FromBody] CancelReferral Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            int outputresult = 0;
            try
            {
                outputresult = objcls.CancelReferral(Model.FacilityId, Model.UserId, Model.sRemarks, Model.ReferralId, Model.Source);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                //outputresult.Dispose();
                objcls = null;
            }
        }
        //
        [HttpPost]
        [Route("api/EMRAPI/DiagPrintLabResult")]
        public IHttpActionResult DiagPrintLabResult([FromBody] DiagPrintLabResult Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.uspDiagPrintLabResult(Model.iHospId, Model.iLoginFacilityId, Model.iLabNo, Model.intStationId, Model.chvServiceIds, Model.source);
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
        [Route("api/EMRAPI/GetReportHeader")]
        public IHttpActionResult GetReportHeader([FromBody] GetReportHeader Model)
        {
            BaseC.Hospital objcls = new BaseC.Hospital(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetReportHeader(Model.Type);
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
        [Route("api/EMRAPI/GetAdmissionDetails")]
        public IHttpActionResult GetAdmissionDetails([FromBody] GetAdmissionDetails Model)
        {
            BaseC.WardManagement Objstatus = new BaseC.WardManagement();
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = Objstatus.GetAdmissionDetails(Model.HospitalLocationId, Model.userid);
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
                Objstatus = null;
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/UserLoginAudit")]
        public IHttpActionResult UserLoginAudit([FromBody] UserLoginAudit Model)
        {
            BaseC.User objUser = new BaseC.User(sConString);
            try
            {
                objUser.UserLoginAudit(Model.UserID, Model.UserHostAddress, Model.sStatus);
                return Ok("OK");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        [HttpPost]
        [Route("api/EMRAPI/GetLoginExpiryMessage")]
        public IHttpActionResult GetLoginExpiryMessage([FromBody] LoginExpiryMessage Model)
        {
            BaseC.User Objstatus = new BaseC.User(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = Objstatus.getLoginExpiryMessage(Model.Facitiyid, Model.DayDifferent, Model.ShowExpWarning);
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
                Objstatus = null;
            }
        }


        //  getFavoriteDrugWithStockWithFavoriteId
        [HttpPost]
        [Route("api/EMRAPI/getFavoriteDrugWithStockWithFavoriteId")]
        public IHttpActionResult getFavoriteDrugWithStockWithFavoriteId([FromBody] DeleteFavoriteMedicine Model)
        {
            BaseC.clsEMR Objstatus = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = Objstatus.getFavoriteDrugWithStockWithFavoriteId(Model.FavoriteId);
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
                Objstatus = null;
            }
        }


        //DeleteFavoriteDrugsWithFavoriteId

        [HttpPost]
        [Route("api/EMRAPI/DeleteFavoriteDrugsWithFavoriteId")]
        public IHttpActionResult DeleteFavoriteDrugsWithFavoriteId([FromBody] DeleteFavoriteDrugsWithFavoriteId Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.DeleteFavoriteDrugsWithFavoriteId(Model.FavoriteId, Model.EncodedBy);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                outputresult = string.Empty;
                objcls = null;
            }
        }


        //uspMasterGetUserFacilityList

        [HttpPost]
        [Route("api/EMRAPI/MasterGetUserFacilityList")]
        public IHttpActionResult MasterGetUserFacilityList([FromBody] EMRModel Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", Model.HospitalLocationId);
            hshInput.Add("@intUserId", Model.UserId);
            try
            {
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspMasterGetUserFacilityList", hshInput);
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

            }
        }

        [HttpPost]
        [Route("api/EMRAPI/GetFlagValueHospitalSetup")]
        public IHttpActionResult GetFlagValueHospitalSetup([FromBody] getHospitalSetupValueMultiple Model)
        {
            string sValue = string.Empty;

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@iHospitalLocationId", common.myInt(Model.HospitalLocationId));
            hshIn.Add("@iFacilityId", common.myInt(Model.FacilityId));
            DataSet dsHs = new DataSet();
            dsHs = dl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalSetup", hshIn);
            System.Web.HttpContext.Current.Cache.Insert("HospitalSetup", dsHs.Tables[0], null, DateTime.Now.AddYears(10), System.Web.Caching.Cache.NoSlidingExpiration);
            DataView dv = dsHs.Tables[0].DefaultView;
            dv.RowFilter = "Flag = '" + Model.Flag + "' AND facilityId = " + Model.FacilityId;
            if (dv.ToTable().Rows.Count > 0)
                sValue = common.myStr(dv.ToTable().Rows[0]["value"]);
            dv.RowFilter = string.Empty;
            //  }
            return Ok(sValue);
        }


        [HttpPost]
        [System.Web.Http.Route("api/EMRAPI/SetPatinetCheckIn")]
        public IHttpActionResult SetPatinetCheckIn([FromBody] SetPatinetCheckInPut Model)
        {

            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            Hashtable HshOut = new Hashtable();
            try
            {
                HshOut = objcls.SetPatinetCheckIn(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.EncodedBy, Model.flag);
                string json = JsonConvert.SerializeObject(HshOut["@chvErrorStatus"].ToString(), Formatting.Indented);
                return Ok(json.ToString());

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshOut = null;
                objcls = null;
            }

        }


        [HttpPost]
        [Route("api/EMRAPI/getPatientJLabResultHistoryDash")]
        public IHttpActionResult getPatientJLabResultHistoryDash([FromBody] LabResultHistory Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getPatientLabResultHistoryDash(Model.iLoginFacilityId, Model.iHostId, Model.fromDate, Model.toDate, Model.iRegNo, Model.iProviderId, Model.iPageSize, Model.iPageNo, Model.AbnormalResult, Model.CriticalResult, Model.iStatusId, Model.FacilityId, Model.chvEncounterNo, Model.ReviewedStatus, Model.PatientName, Model.iUserId);
                //   string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                List<JLabResultHistoryOut> output = new List<JLabResultHistoryOut>();
                if (ds != null)
                {

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        JLabResultHistoryOut model = new JLabResultHistoryOut();
                        model.PatientName = common.myStr(ds.Tables[0].Rows[i]["PatientName"]);
                        model.RegistrationNo = common.myStr(ds.Tables[0].Rows[i]["RegistrationNo"]);
                        model.Source = common.myStr(ds.Tables[0].Rows[i]["Source"]);
                        model.ServiceName = common.myStr(ds.Tables[0].Rows[i]["ServiceName"]);
                        model.OrderDate = common.myStr(ds.Tables[0].Rows[i]["OrderDate"]);
                        output.Add(model);
                    }
                    return Ok(output);

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
        [Route("api/EMRAPI/GetAppointmentDate")]
        public IHttpActionResult GetAppointmentDate([FromBody] EMRModel Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@intHospitalLocationId", Model.HospitalLocationId);
            hshInput.Add("@intRegistrationId", Model.cRegistrationNo);
            hshInput.Add("@FormId", Model.FormId);
            //   hstInput.Add("", Session["RegistrationId"]);
            try
            {
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAppointmentDate", hshInput);
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

            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetDataObjectDetails")]
        public IHttpActionResult GetDataObjectDetails([FromBody] GetDataObjectDetails Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();
            hshInput.Add("@intHeaderId", Model.HeaderId);
            hshInput.Add("@chrAppointmentDate", Model.AppointmentDate);
            hshInput.Add("@intEmployeeId", Model.DoctorId);
            hshInput.Add("@intRegistrationId", Model.RegistrationId);
            hshInput.Add("@intFacilityId", Model.FacilityId);
            hshInput.Add("@intEncounterId", Model.EncounterId);
            try
            {
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hshInput);
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

            }
        }


        /// <summary>
        /// Clinical Pathway API
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/EMRAPI/EMRGetClinicalPathTemplateDetails")]
        public IHttpActionResult EMRGetClinicalPathTemplateDetails([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();

            try
            {
                hshInput.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                hshInput.Add("@intFacilityId", Model.FacilityId);
                hshInput.Add("@intTemplateId", Model.TemplateId);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetClinicalPathTemplateDetails", hshInput);
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
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetEMRDrugClassMaster")]
        public IHttpActionResult GetEMRDrugClassMaster([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();

            try
            {
                hshInput.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                hshInput.Add("@intFacilityId", Model.FacilityId);
                hshInput.Add("@chvDrugName", Model.DrugName);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDrugClassMaster", hshInput);
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
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetPlanTypeDurationMaster")]
        public IHttpActionResult EMRGetPlanTypeDurationMaster([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();

            try
            {
                hshInput.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                hshInput.Add("@intFacilityId", Model.FacilityId);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPlanTypeDurationMaster", hshInput);
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
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetPlanTypeDurationDetails")]
        public IHttpActionResult EMRGetPlanTypeDurationDetails([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet outputresult = new DataSet();
            Hashtable hshInput = new Hashtable();

            try
            {
                hshInput.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                hshInput.Add("@intFacilityId", Model.FacilityId);
                hshInput.Add("@intPlanId", Model.PlanId);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPlanTypeDurationDetails", hshInput);
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
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanList")]
        public IHttpActionResult EMRGetTreatmentPlanList([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncodedby", Model.EncodedBy);

                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanList", HshIn);
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
                objDl = null;
                HshIn = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanDiagnosis")]
        public IHttpActionResult EMRGetTreatmentPlanDiagnosis([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intPlanId", Model.PlanId);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanDiagnosis", HshIn);
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
                objDl = null;
                HshIn = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetPlanWiseDayEnteredData")]
        public IHttpActionResult GetPlanWiseDayEnteredData([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPlanWiseDayEnteredData", HshIn);
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
                objDl = null;
                HshIn = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/GetPatientPlanWiseDayEnteredData")]
        public IHttpActionResult GetPatientPlanWiseDayEnteredData([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncounterId", Model.EncounterId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);

                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientPlanWiseDayEnteredData", HshIn);
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
                objDl = null;
                HshIn = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRCopyTreatmentPlanDetails")]
        public IHttpActionResult EMRCopyTreatmentPlanDetails([FromBody] ClinicalPath Model)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@intCopyDayId", Model.DayId);
                HshIn.Add("@intCopyDayDetailId", Model.DayDetailId);
                HshIn.Add("@intSelectedDayId", Model.SelectedDayId);
                HshIn.Add("@intSelectedDayDetailId", Model.SelectedDayDetailId);


                HshIn.Add("@xmlSpecialtyDetails", Model.xmlSpecialtyDetails);
                HshIn.Add("@xmlServiceDetails", Model.xmlServiceDetails);
                HshIn.Add("@xmlDrugClassDetails", Model.xmlDrugClassDetails);
                HshIn.Add("@xmlPrescriptionDetails", Model.xmlPrescriptionDetails);
                HshIn.Add("@xmlTemplateDetails", Model.xmlTemplateDetails);
                HshIn.Add("@xmlTemplateFieldDetails", Model.xmlTemplateFieldDetails);



                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCopyTreatmentPlanDetails", HshIn, HshOut);
                return Ok(HshOut["@chvErrorStatus"].ToString());
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
        [HttpPost]
        [Route("api/EMRAPI/EMRPatientTreatmentPlanNameLists")]
        public IHttpActionResult EMRPatientTreatmentPlanNameLists([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncounterId", Model.EncounterId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);

                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRPatientTreatmentPlanNameLists", HshIn);
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
                objDl = null;
                HshIn = null;

            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRSavePatientTreatmentPlanTemplates")]
        public IHttpActionResult EMRSavePatientTreatmentPlanTemplates([FromBody] ClinicalPath Model)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);
                HshIn.Add("@intEncounterId", Model.EncounterId);

                HshIn.Add("@xmlOrderServices", Model.xmlServiceDetails);
                HshIn.Add("@xmlProblemDetails", Model.xmlProblems);
                HshIn.Add("@xmlTemplateDetails", Model.xmlTemplateDetails);
                HshIn.Add("@xmlItems", Model.xmlItems);
                HshIn.Add("@xmlItemDetail", Model.xmlItemDetail);

                HshIn.Add("@intPlanId", Model.PlanId);

                HshIn.Add("@DayId", Model.DayId);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);

                HshIn.Add("@intDoctorId", Model.DoctorId);



                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientTreatmentPlanTemplates", HshIn, HshOut);
                return Ok(HshOut["@chvErrorStatus"].ToString());
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
        [HttpPost]
        [Route("api/EMRAPI/EMRSavePatientTreatmentPlanDetails")]
        public IHttpActionResult EMRSavePatientTreatmentPlanDetails([FromBody] ClinicalPath Model)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);
                HshIn.Add("@intEncounterId", Model.EncounterId);

                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@intDayId", Model.DayId);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);

                HshIn.Add("@xmlSpecialtyDetails", Model.xmlSpecialtyDetails);
                HshIn.Add("@xmlServiceDetails", Model.xmlServiceDetails);
                HshIn.Add("@xmlDrugClassDetails", Model.xmlDrugClassDetails);
                HshIn.Add("@xmlPrescriptionDetails", Model.xmlPrescriptionDetails);
                HshIn.Add("@xmlTemplateDetails", Model.xmlTemplateDetails);
                HshIn.Add("@xmlTemplateFieldDetails", Model.xmlTemplateFieldDetails);


                HshIn.Add("@chvChiefComplaints", Model.ChiefComplaints);
                HshIn.Add("@chvHistory", Model.History);
                HshIn.Add("@chvExamination", Model.Examination);
                HshIn.Add("@chvPlanOfCare", Model.PlanOfCare);
                HshIn.Add("@chvTextInstructions", Model.Instruction);

                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientTreatmentPlanDetails", HshIn, HshOut);
                return Ok(HshOut["@chvErrorStatus"].ToString());
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
        [HttpPost]
        [Route("api/EMRAPI/EMRSavePatientTemplateWiseTreatmentPlanDetails")]
        public IHttpActionResult EMRSavePatientTemplateWiseTreatmentPlanDetails([FromBody] ClinicalPath Model)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);
                HshIn.Add("@intEncounterId", Model.EncounterId);

                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@intDayId", Model.DayId);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);

                HshIn.Add("@xmlSpecialtyDetails", Model.xmlSpecialtyDetails);
                HshIn.Add("@xmlServiceDetails", Model.xmlServiceDetails);
                HshIn.Add("@xmlDrugClassDetails", Model.xmlDrugClassDetails);
                HshIn.Add("@xmlPrescriptionDetails", Model.xmlPrescriptionDetails);
                HshIn.Add("@xmlTemplateDetails", Model.xmlTemplateDetails);
                HshIn.Add("@xmlTemplateFieldDetails", Model.xmlTemplateFieldDetails);




                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSavePatientTemplateWiseTreatmentPlanDetails", HshIn, HshOut);
                return Ok(HshOut["@chvErrorStatus"].ToString());
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

        [HttpPost]
        [Route("api/EMRAPI/EMRGetPatientTreatmentPlanDetails")]
        public IHttpActionResult EMRGetPatientTreatmentPlanDetails([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncounterId", Model.EncounterId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);

                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@DayId", Model.DayId);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);
                HshIn.Add("@chvTemplateType", Model.TemplateType);

                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientTreatmentPlanDetails", HshIn);
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
                objDl = null;
                HshIn = null;

            }
        }

        [HttpPost]
        [Route("api/EMRAPI/EMRGetPatientTreatmentPlanTaggedDetails")]
        public IHttpActionResult EMRGetPatientTreatmentPlanTaggedDetails([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            try
            {

                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncounterId", Model.EncounterId);
                HshIn.Add("@intRegistrationId", Model.RegistrationId);

                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@DayId", Model.DayId);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);
                HshIn.Add("@chvTemplateType", Model.TemplateType);

                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientTreatmentPlanTaggedDetails", HshIn);
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
                objDl = null;
                HshIn = null;

            }
        }

        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanDetails")]
        public IHttpActionResult EMRGetTreatmentPlanDetails([FromBody] ClinicalPath Model)
        {
            DataSet outputresult = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            try
            {
                hshInput.Add("@intPlanId", Model.TemplateId);
                hshInput.Add("@DayId", Model.DayId);
                hshInput.Add("@intDayDetailId", Model.DayDetailId);
                hshInput.Add("@chvTemplateType", Model.TemplateType);
                hshInput.Add("@bitCopy", Model.IsCopy);
                outputresult = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanDetails", hshInput);
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
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveEMRTreatmentPlanName")]
        public IHttpActionResult SaveEMRTreatmentPlanName([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@intId", Model.Id);
                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intDepartmentId", Model.DepartmentId);
                HshIn.Add("@chvPlanName", Model.PlanName);
                HshIn.Add("@intEncodedby", Model.EncodedBy);
                HshIn.Add("@intDoctorId", Model.DoctorId);
                HshIn.Add("@chvPlanType", Model.PlanType);
                HshIn.Add("@chvDocumentName", Model.DocumentName);
                HshIn.Add("@xmlDays", Model.xmlDays);
                HshIn.Add("@xmlDiagnosis", Model.xmlDiagnosis);
                HshIn.Add("@bitIsSurgical", Model.IsSurgical);
                HshIn.Add("@intTemplateId", Model.TemplateId);

                HshOut.Add("@intPlanId", SqlDbType.Int);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRTreatmentPlanName", HshIn, HshOut);
                return Ok(HshOut["@chvErrorOutput"].ToString() + "_" + HshOut["@intPlanId"].ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshOut = null;
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRDeleteTreatmentPlanDetail")]
        public IHttpActionResult EMRDeleteTreatmentPlanDetail([FromBody] ClinicalPath Model)//string TemplateType, int id, int PlanId, int DayId, int DayDetailId, int EncodedBy)
        {
            Hashtable hstInput = new Hashtable();
            Hashtable hstOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hstInput.Add("@chvTemplateType", Model.TemplateType);
                hstInput.Add("@Id", Model.Id);
                hstInput.Add("@intPlanId", Model.PlanId);
                hstInput.Add("@DayId", Model.DayId);
                hstInput.Add("@intDayDetailId", Model.DayDetailId);
                hstInput.Add("@intEncodedBy", Model.EncodedBy);

                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hstOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteTreatmentPlanDetail", hstInput, hstOut);
                return Ok(hstOut["@chvErrorStatus"].ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                hstOut = null;
                objDl = null;
                hshInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/DeleteTreatmentPlan")]
        public IHttpActionResult DeleteTreatmentPlan([FromBody] ClinicalPath Model)//int PlanId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            try
            {
                int i = 0;
                hstInput.Add("@PlanId", Model.PlanId);
                hstInput.Add("@intEncodedBy", Model.EncodedBy);
                i = objDl.ExecuteNonQuery(CommandType.Text, "update EMRTreatmentPlanName Set Active = 0,LastChangedBy = @intEncodedBy,LastChangedDate = GETUTCDATE() where PlanId = @PlanId", hstInput);
                return Ok(i.ToString());
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                hstInput = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/SaveEMRTreatmentTemplate")]
        public IHttpActionResult SaveEMRTreatmentTemplate([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {

                HshIn.Add("@chvTemplateType", Model.TemplateType);
                HshIn.Add("@intPlanId", Model.PlanId);
                HshIn.Add("@intDayId", Model.DayId);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);
                HshIn.Add("@intSpecialityId", Model.SpecialityId);
                HshIn.Add("@intServiceId", Model.ServiceId);
                HshIn.Add("@intDrugClassId", Model.DrugClassId);
                HshIn.Add("@GenericId", Model.GenericId);
                HshIn.Add("@intItemId", Model.ItemId);
                HshIn.Add("@intFrequencyId", Model.FrequencyId);
                HshIn.Add("@intDays", Model.Days);
                HshIn.Add("@chvDaysType", Model.DaysType);
                HshIn.Add("@decDose", Model.Dose);
                HshIn.Add("@intDoseUnitId", Model.DoseUnitId);
                HshIn.Add("@intFoodRelationId", Model.FoodRelationId);
                HshIn.Add("@chvInstruction", Model.Instruction);
                HshIn.Add("@intTemplateTaggingId", Model.TemplateTaggingId);
                HshIn.Add("@xmlTemplateFieldDetails", Model.xmlTemplateFieldDetails);
                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshIn.Add("@intRouteId", Model.RouteId);
                HshIn.Add("@decQty", Model.totalQty);

                HshIn.Add("@chvChiefComplaints", Model.ChiefComplaints);
                HshIn.Add("@chvHistory", Model.History);
                HshIn.Add("@chvExamination", Model.Examination);
                HshIn.Add("@chvPlanOfCare", Model.PlanOfCare);
                HshIn.Add("@chvTextInstructions", Model.Instruction);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveTreatmentPlanDetails", HshIn, HshOut);
                return Ok(HshOut["@chvErrorStatus"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                HshOut = null;
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanPatientLists")]
        public IHttpActionResult EMRGetTreatmentPlanPatientLists([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {
                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanPatientLists", HshIn);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
                ds.Dispose();
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanItemMasterList")]
        public IHttpActionResult EMRGetTreatmentPlanItemMasterList([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanItemMasterList", HshIn, HshOut);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                ds.Dispose();
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanItemAttributes")]
        public IHttpActionResult EMRGetTreatmentPlanItemAttributes([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intItemId", Model.ItemId);
                HshIn.Add("@intEncodedBy", Model.EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanItemAttributes", HshIn, HshOut);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                ds.Dispose();
            }
        }
        [HttpPost]
        [Route("api/EMRAPI/EMRGetTreatmentPlanValidationForDataEntry")]
        public IHttpActionResult EMRGetTreatmentPlanValidationForDataEntry([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);
                HshIn.Add("@intEncounterId", Model.EncounterId);
                HshIn.Add("@chvDayType", Model.DaysType);
                HshIn.Add("@intDayDetailId", Model.DayDetailId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanValidationForDataEntry", HshIn, HshOut);
                return Ok(HshOut["@chvErrorStatus"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;

                HshIn = null;
                HshOut = null;
            }
        }


        [HttpPost]
        [Route("api/EMRAPI/SavePatientChiefComplaint")]
        public IHttpActionResult SavePatientChiefComplaint(PateintProblemModel model)
        {
            var data = SavePatinetProblem(model);
            return Ok(data);
        }

        public string SavePatinetProblem(PateintProblemModel Model)
        {
            //StringBuilder xmlFeedback = new StringBuilder();
            //ArrayList feedbackDetailsList = new ArrayList();
            StringBuilder xmlPatientProblem = new StringBuilder();
            ArrayList ProblemtList = new ArrayList();
            foreach (var data in Model.PatientProblem)
            {
                //ProblemtList.Add(data.problemId);
                //ProblemtList.Add(data.problemName);
                //ProblemtList.Add(data.intduration);
                //ProblemtList.Add(data.duration);
                //ProblemtList.Add(data.remarks);

                ProblemtList.Add(0);//Id
                ProblemtList.Add(data.problemId);//ProblemId
                ProblemtList.Add(data.problemName);//Problem
                ProblemtList.Add(data.intduration);//DurationID
                if (common.myStr(data.duration) == "O")
                    ProblemtList.Add(common.ParseString(""));//Duration            
                else
                    ProblemtList.Add(string.Empty); //Duration

                ProblemtList.Add(0);//ContextID
                ProblemtList.Add(common.ParseString(""));//Context
                ProblemtList.Add(0);//SeverityId
                ProblemtList.Add("");//Severity
                ProblemtList.Add(false);//IsPrimary
                ProblemtList.Add(false);//IsChronic
                ProblemtList.Add(common.myInt(Model.DoctorId));//DoctorId
                ProblemtList.Add(common.myInt(Model.FacilityId));//FacilityId
                ProblemtList.Add(common.ParseString(""));//SCTId
                ProblemtList.Add(common.ParseString(""));//QualityIDs
                ProblemtList.Add(0);//LocationID
                ProblemtList.Add(common.ParseString(""));//Location
                ProblemtList.Add(0);//OnsetID
                ProblemtList.Add("");//AssociatedProblemId1
                ProblemtList.Add("");//AssociatedProblem1
                ProblemtList.Add("");//AssociatedProblemId2
                ProblemtList.Add("");//AssociatedProblem2
                ProblemtList.Add("");//AssociatedProblemId3
                ProblemtList.Add("");//AssociatedProblem3
                ProblemtList.Add("");//AssociatedProblemId4
                ProblemtList.Add("");//AssociatedProblem4
                ProblemtList.Add("");//AssociatedProblemId5
                ProblemtList.Add("");//AssociatedProblem5
                ProblemtList.Add(0);//Side
                ProblemtList.Add(0);//ConditionId
                ProblemtList.Add("");//Percentage
                ProblemtList.Add(data.intduration);//Durations
                ProblemtList.Add(data.duration);//DurationType
                ProblemtList.Add(0);//TemplateFieldId
                ProblemtList.Add(0);//ComplaintSearchId
                ProblemtList.Add(data.remarks);//ProblemDetails

                xmlPatientProblem.Append(common.setXmlTable(ref ProblemtList));

            }


            ///var result = SavePatientProblem(xmlPatientProblem.ToString());
            // return result;

            BaseC.EMRProblems objcls = new BaseC.EMRProblems(sConString);
            string outputresult = "";
            string json = string.Empty;
            try
            {
                outputresult = objcls.EMRSavePatientProblems(Model.HospitalLocationID, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.PageId,
                xmlPatientProblem.ToString(), Model.UserId, Model.Remarks, Model.IsPregment, Model.IsBreastFeed, Model.IsShowNote, Model.DoctorId);
                json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                outputresult = string.Empty;
                objcls = null;
            }

            return json;
        }
        /// <summary>
        /// Save Master
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/EMRAPI/SaveMasterComplaint")]
        public IHttpActionResult SaveMasterComplaint([FromBody] SaveProblemMaster model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string message = string.Empty;
            hshInput = new Hashtable();
            Hashtable hshOut = new Hashtable();
            hshInput.Add("@HospitalLocationID", model.HospitalLocationID);
            hshInput.Add("@DoctorId", model.DoctorId);
            hshInput.Add("@specialisationid", model.specialisationid);
            hshInput.Add("@strDescription", common.myStr(model.Description));
            hshInput.Add("@IsFavourite", model.IsFavourite);
            hshInput.Add("@EncodedBy", model.EncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveMasterChiefComplaintdata", hshInput, hshOut);

            return Ok(hshOut["@chvErrorStatus"].ToString());

        }


        [HttpPost]
        [Route("api/EMRAPI/SaveDoctorFavChiefComplaint")]
        public IHttpActionResult SaveDoctorFavChiefComplaint(DoctorProblemModel model)
        {
            var data = SaveDoctorFavProblem(model);
            return Ok(data);
        }

        public string SaveDoctorFavProblem(DoctorProblemModel Model)
        {
            //StringBuilder xmlFeedback = new StringBuilder();
            //ArrayList feedbackDetailsList = new ArrayList();
            StringBuilder xmlPatientProblem = new StringBuilder();
            ArrayList ProblemtList = new ArrayList();
            foreach (var data in Model.DoctorProblem)
            {
                ProblemtList.Add(data.problemId);//ProblemId
                xmlPatientProblem.Append(common.setXmlTable(ref ProblemtList));
            }

            string outputresult = "";
            string json = string.Empty;
            try
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = new DataSet();
                Hashtable hsInput = new Hashtable();
                Hashtable hstOutput = new Hashtable();
                hsInput.Add("@xmlProblemDetails", xmlPatientProblem.ToString());
                hsInput.Add("@intDoctorId", Model.DoctorId);
                hsInput.Add("@intUserId", Model.UserId);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveDoctorFavProblems", hsInput, hstOutput);
                return hstOutput["@chvErrorStatus"].ToString();
                //outputresult = objcls.EMRSavePatientProblems(Model.HospitalLocationID, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.PageId,
                //xmlPatientProblem.ToString(), Model.UserId, Model.Remarks, Model.IsPregment, Model.IsBreastFeed, Model.IsShowNote, Model.DoctorId);
                //json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                outputresult = string.Empty;

            }

        }


        [HttpPost]
        [Route("api/EMRAPI/GetDoctorProgressNoteForSingleScreen")]
        public IHttpActionResult GetDoctorProgressNoteForSingleScreen([FromBody] GetDoctorProgressNote Model)
        {
            string json = "";
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            List<GetDoctorNote> OutParam = new List<GetDoctorNote>();
            try
            {
                ds = objcls.GetDoctorProgressNote(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.DoctorId, Model.FromDate, Model.ToDate, Model.GroupingDate, Model.ProgressNoteId);
                // string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                if (Model.ProgressNoteId > 0)
                {
                    json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                    return Ok(json);
                }
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        GetDoctorNote model = new GetDoctorNote();
                        model.DoctorName = ds.Tables[0].Rows[i]["DoctorName"].ToString();
                        model.EnteredDate = common.myDate(ds.Tables[0].Rows[i]["EnteredDate"].ToString());
                        model.ProgressNoteId = common.myInt(ds.Tables[0].Rows[i]["ProgressNoteId"]);
                        OutParam.Add(model);
                    }
                }
                else
                {
                    OutParam.Add(null);
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
            if (Model.ProgressNoteId == 0)
            {
                return Ok(OutParam);
            }
            else
            {
                return Ok(json);
            }

        }


        [HttpPost]
        [Route("api/EMRAPI/EMRGetDefaultTemplateForCarePlan")]
        public IHttpActionResult EMRGetDefaultTemplateForCarePlan([FromBody] ClinicalPath Model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", Model.HospitalLocationId);
                HshIn.Add("@intFacilityId", Model.FacilityId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDefaultTemplateForCarePlan", HshIn);
                string json = JsonConvert.SerializeObject(ds, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDl = null;
                ds.Dispose();
            }
        }

        [HttpPost]
        [Route("api/EMRAPI/GetDoctorPanelSetting")]
        public IHttpActionResult GetDoctorPanelSetting([FromBody] DoctoPanelSettingParam param)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            ds = new DataSet();
            hshInput = new Hashtable();
            hshInput.Add("@intDoctorId", param.DoctorId);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetDoctorPanelSetting", hshInput);
            List<DoctorPanelSettingOutParam> OutParam = new List<DoctorPanelSettingOutParam>();
            try
            {
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        DoctorPanelSettingOutParam model = new DoctorPanelSettingOutParam();


                        model.DoctorId = common.myInt(ds.Tables[0].Rows[i]["DoctorId"]);
                        model.DoctorPanelSetting = ds.Tables[0].Rows[i]["DoctorPanelSetting"].ToString();
                        OutParam.Add(model);
                    }

                }
                else
                {
                    OutParam.Add(null);
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return Ok(OutParam);
        }

        [HttpPost]
        [Route("api/EMRAPI/SaveFormat")]
        public IHttpActionResult SaveFormat([FromBody] ProgressNoteFormat Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SaveFormat(Model.FormatId, Model.FacilityId, Model.FormatName, Model.FormatText, Model.Active, Model.EncodedBy, Model.chvErrorStatus);
                return Ok(outputresult);
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
    }


}









