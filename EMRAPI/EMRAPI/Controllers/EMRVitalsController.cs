using EMRAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EMRAPI.Controllers
{
   
    public class EMRVitalsController : ApiController
    {
        private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        private Hashtable hstInput = new Hashtable();
        private DataSet ds = new DataSet();
       
        [HttpPost]
        [Route("api/EMRVitals/GetVitalTemplateDetails")]
        public IHttpActionResult GetVitalTemplateDetails([FromBody] VitalModel model)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput = new Hashtable();
            hstInput.Add("@TemplateId", model.iTemplateId);
            hstInput.Add("@intFacilitiId", model.FacilityId);
            hstInput.Add("@intRegistrationId", model.Registrationid);

            if (model.VitalDetailsId > 0)
            {
                hstInput.Add("@intVitalDetailsId", model.VitalDetailsId);
            }
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetVitalTemplateDetails", hstInput);
            List<EmrTemplate> obj = new List<EmrTemplate>();
            if (ds!=null)
            {
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    EmrTemplate emrtemplate = new EmrTemplate();
                //    emrtemplate.TemplateId = common.myInt(ds.Tables[0].Rows[i]["TemplateId"]);
                //    emrtemplate.SequenceNo = common.myInt(ds.Tables[0].Rows[i]["SequenceNo"]);
                //    emrtemplate.VitalId = common.myInt(ds.Tables[0].Rows[i]["VitalId"]);
                //    emrtemplate.Vital = common.myStr(ds.Tables[0].Rows[i]["Vital"]);
                //    emrtemplate.DisplayName = common.myStr(ds.Tables[0].Rows[i]["DisplayName"]);
                //    emrtemplate.TemplateDescription = common.myStr(ds.Tables[0].Rows[i]["TemplateDescription"]);
                //    emrtemplate.MeasurementSystem = common.myStr(ds.Tables[0].Rows[i]["MeasurementSystem"]);
                //    emrtemplate.MinValue = common.myInt(ds.Tables[0].Rows[i]["MinValue"]);
                //    emrtemplate.MaxValue = common.myInt(ds.Tables[0].Rows[i]["MaxValue"]);
                //    emrtemplate.VitalType = common.myStr(ds.Tables[0].Rows[i]["VitalType"]);
                //    emrtemplate.MaxLength = common.myInt(ds.Tables[0].Rows[i]["MaxLength"]);
                //    emrtemplate.Active = common.myBool(ds.Tables[0].Rows[i]["Active"]);
                //    emrtemplate.UnitId1 = common.myInt(ds.Tables[0].Rows[i]["UnitId1"]);
                //    emrtemplate.Unit1 = common.myStr(ds.Tables[0].Rows[i]["Unit1"]);
                //    emrtemplate.UnitId2 = common.myInt(ds.Tables[0].Rows[i]["UnitId2"]);
                //    emrtemplate.LastValues = common.myStr(ds.Tables[0].Rows[i]["LastValues"]);
                //    emrtemplate.LastValue = common.myStr(ds.Tables[0].Rows[i]["LastValue"]);
                //    emrtemplate.HeaderName = common.myStr(ds.Tables[0].Rows[i]["HeaderName"]);
                //    emrtemplate.AbNormal = common.myStr(ds.Tables[0].Rows[i]["AbNormal"]);
                //    emrtemplate.Category = common.myStr(ds.Tables[0].Rows[i]["Category"]);
                //    emrtemplate.IsInvestigation = common.myInt(ds.Tables[0].Rows[i]["IsInvestigation"]);
                //    emrtemplate.TemplateFieldId = common.myInt(ds.Tables[0].Rows[i]["TemplateFieldId"]);
                //    emrtemplate.TemplateData = common.myInt(ds.Tables[0].Rows[i]["TemplateData"]);
                //    emrtemplate.IsMandatory = common.myBool(ds.Tables[0].Rows[i]["IsMandatory"]);
                //    emrtemplate.ValueID = common.myInt(ds.Tables[0].Rows[i]["ValueID"]);
                //    emrtemplate.DefaultValue = common.myInt(ds.Tables[0].Rows[i]["DefaultValue"]);
                //    emrtemplate.DisplayInGraph = common.myBool(ds.Tables[0].Rows[i]["DisplayInGraph"]);                  

                //    obj.Add(emrtemplate);
                //}
                //  return Ok(obj);
                return Ok(JsonConvert.SerializeObject(ds));
               
            }
            else
            {
                return NotFound();

            }
            
        }

        /// <summary>
        ///  Find patient
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>

        [HttpPost]
        [Route("api/EMRVitals/getHospitalSetupValueMultiple")]
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
        [Route("api/EMRVitals/SearchAppointmentList")]
        public IHttpActionResult SearchAppointmentList(SearchAppointmentList Model)
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
                ds.Dispose();
                objcls = null;
            }
        }

        [HttpPost]
        [Route("api/EMRVitals/GetDoctorPatientLists")]
        public IHttpActionResult GetDoctorPatientLists(GetDoctorPatientLists Model)
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
        [Route("api/EMRVitals/GetStatusMaster")]
        public IHttpActionResult GetStatusMaster(GetStatusMaster Model)
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
        [Route("api/EMRVitals/GetEmployeeWithResource")]
        public IHttpActionResult GetEmployeeWithResource(GetEmployeeWithResource Model)
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
        [Route("api/EMRVitals/GetDoctorTimeSpecialisation")]
        public IHttpActionResult GetDoctorTimeSpecialisation(GetDoctorTimeSpecialisation Model)
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
        [Route("api/EMRVitals/GetPatientAccessRights")]
        public IHttpActionResult GetPatientAccessRights(GetEmployeeWithResource Model)
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
        [Route("api/EMRVitals/getFacilityList")]
        public IHttpActionResult getFacilityList(getFacilityList Model)
        {
            BaseC.clsLISMaster objcls = new BaseC.clsLISMaster(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getFacilityList(Model.HospitalLocationId, Model.UserId, Model.GroupID, Model.EncodedBy);
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
        [Route("api/EMRVitals/ValidateUserForEMRFile")]
        public IHttpActionResult ValidateUserForEMRFile(ValidateUserForEMRFile Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);

            int outputresult = objcls.ValidateUserForEMRFile(Model.HospitalLocationId, Model.RegistrationID, Model.EncounterId, Model.UserId);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRVitals/getEMRPatientDetails")]
        public IHttpActionResult getEMRPatientDetails(getEMRPatientDetails Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);

            DataSet ds = new DataSet();
            try
            {
                ds = objcls.getEMRPatientDetails(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationNo, Model.EncounterNo, Model.UserId);
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
        [Route("api/EMRVitals/DeleteAppointment")]
        public IHttpActionResult DeleteAppointment(DeleteAppointment Model)
        {
            Hashtable outputresult = new Hashtable();
            BaseC.Appointment objcls = new BaseC.Appointment(sConString);
            outputresult = objcls.DeleteAppointment(Model.AppointmentId, Model.UserId,Model.CancelRemarks);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }

        [HttpPost]
        [Route("api/EMRVitals/EMRAcknowledgeNextPatient")]
        public IHttpActionResult EMRAcknowledgeNextPatient(EMRAcknowledgeNextPatient Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            bool outputresult = objcls.EMRAcknowledgeNextPatient(Model.FacilityId, Model.DoctorId);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }

        [HttpPost]
        [Route("api/EMRVitals/GetDefaultTemplate")]
        public IHttpActionResult GetDefaultTemplate(GetDefaultTemplate Model)
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
        [Route("api/EMRVitals/GetDefaultPageByUser")]
        public IHttpActionResult GetDefaultPageByUser(GetDefaultPageByUser Model)
        {
            BaseC.EMR objcls = new BaseC.EMR(sConString);
            string outputresult = objcls.GetDefaultPageByUser(Model.GroupId, Model.URLAddress);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/EMRVitals/GetReferralDetailCount")]
        public IHttpActionResult GetReferralDetailCount(GetReferralDetailCount Model)
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
        [Route("api/EMRVitals/GetTokentocallDoc")]
        public IHttpActionResult GetTokentocallDoc(GetTokentocallDoc Model)
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
        [Route("api/EMRVitals/CallPatient")]
        public IHttpActionResult CallPatient(CallPatient Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = objcls.CallPatient(Model.Tokenno, Model.UserId, Model.ServiceID, Model.CounterID, Model.ZoneID);
            string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
            return Ok(json);
        }
        // End Find patient

        [HttpPost]
        [Route("api/EMRVitals/bindAutoCalculate")]
        public IHttpActionResult bindAutoCalculate(vital Model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.bindAutoCalculate(Model.Hospitalid);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }
            
            
        }
        [HttpPost]
        [Route("api/EMRVitals/GetHospitalVitalTemplate")]
        public IHttpActionResult GetHospitalVitalTemplate(vital Model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.GetHospitalVitalTemplate(Model.Hospitalid,Model.UserId);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }


        }


        [HttpPost]
        [Route("api/EMRVitals/GetVitalImage")]
        public IHttpActionResult GetVitalImage([FromBody] VitalModel model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);
            
            try
            {
                ds = objcls.GetVitalImage(model.HospitalId, model.FacilityId,model.VitalId);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }


        }



        [HttpPost]
        [Route("api/EMRVitals/GetVitalSignName")]
        public IHttpActionResult GetVitalSignName([FromBody] vital model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.GetVitalSignName(model.date, model.Hospitalid);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }


        }

        [HttpPost]
        [Route("api/EMRVitals/SavePatientVitalSign")]
        public string SavePatientVitalSign([FromBody] SaveVitals model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                string response = objcls.SavePatientVitalSign(model.EntryDate, model.RegId,model.EncId,model.FacilityId,model.PageId,model.PageId,model.Encodedby,model.xmlstring,model.TemplateFieldId, model.VitalDetailsId, model.TriageTypeId);

                return response;


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                ds.Dispose();
            }


        }

        [HttpPost]
        [Route("api/EMRVitals/GEtMEWsScore")]
        public IHttpActionResult GEtMEWsScore([FromBody] vital model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.GEtMEWsScore(model.Encounterid);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }
        }

        [HttpPost]
        [Route("api/EMRVitals/EMRVitalSignTypes")]
        public IHttpActionResult EMRVitalSignTypes([FromBody] vital model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.EMRVitalSignTypes();
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }
        }

        //[HttpPost]
        //[Route("api/EMRVitals/AuditCommonAccess")]
        //public IHttpActionResult AuditCommonAccess([FromBody] CommonAccess model)
        //{
        //    BaseC.Security objcls = new BaseC.Security(sConString);

        //    try
        //    {
        //        objcls.AuditCommonAccess(model.iHospID, model.iFacilityID, model.iRegId, model.iEncountId, model.iPageId, model.iTemplateId,model.iEmpId,model.iAuditStatus,model.chvIPAddres);

        //        if (ds != null)
        //        {
        //            string json = JsonConvert.SerializeObject(ds);
        //            return Ok(json);
        //        }

        //        else
        //        {
        //            return NotFound();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //    }
        //}


        [HttpPost]
        [Route("api/EMRVitals/CloseQMSCall")]
        public IHttpActionResult CloseQMSCall([FromBody] CloseQMSCall model)
        {
            BaseC.clsQMS objcls = new BaseC.clsQMS(sConString);

            try
            {
                int i  = objcls.CloseQMSCall(model.TokenNo,model.UserID,model.ActivityDesc);

                return Ok(i);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            
        }

        [HttpPost]
        [Route("api/EMRVitals/ERtoken")]
        public int ERtoken([FromBody] vital model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                int i = objcls.ERtoken(model.Encounterid);

                return i;
            }
            catch (Exception ex)
            {
                throw ex;

            }

        }

        // Patient Vital Details
        [HttpPost]
        [Route("api/EMRVitals/GetPatientEncounterVitals")]
        public IHttpActionResult GetPatientEncounterVitals(vital model)
        {

            BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
            DataSet objDs = new DataSet();
            objDs = (DataSet)objv.GetPatientEncounterVitals(Convert.ToInt32(model.Encounterid));
            if (objDs != null)
            {
                string json = JsonConvert.SerializeObject(objDs);

                return Ok(json);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/EMRVitals/CancelPatientVitals")]
        public IHttpActionResult CancelPatientVitals(CancelVital model)
        {
            BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
            string result = string.Empty;
            result = objv.CancelPatientVitals(model.CancelremarkId,model.RegId,model.EncId,model.HospId,model.FacilityId,model.PageId,model.Encodedby,model.xmlstring);
            return Ok(result);
            
        }

        //[HttpPost]
        //[Route("api/EMRVitals/TaggingStaticTemplateWithTemplateField")]
        //public IHttpActionResult TaggingStaticTemplateWithTemplateField(TaggingTemplate model)
        //{
        //    BaseC.clsEMR objv = new BaseC.clsEMR(sConString);
        //    Hashtable HshOut = new Hashtable();
        //    HshOut = objv.TaggingStaticTemplateWithTemplateField(model.);
        //    return Ok(result);
        //} 
               
        // BindChart

        [HttpPost]
        [Route("api/EMRVitals/BindChart")]
        public IHttpActionResult BindChart(chart model)
        {

            BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
            DataSet objDs = new DataSet();
            objDs = objv.BindChart(model.vitalId,model.Category);
            if (objDs != null)
            {
                string json = JsonConvert.SerializeObject(objDs);

                return Ok(json);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/EMRVitals/CalculateVitalsValue")]
        public IHttpActionResult CalculateVitalsValue(VitalsValue model)
        {

            BaseC.EMRVitals objv = new BaseC.EMRVitals(sConString);
            DataSet objDs = new DataSet();
            objDs = objv.CalculateVitalsValue(model.HospitalLocationID, model.xmlstr);
            if (objDs != null)
            {
                string json = JsonConvert.SerializeObject(objDs);

                return Ok(json);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("api/EMRVitals/VitalGraph")]
        public IHttpActionResult VitalGraph([FromBody] VitalGraph model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.VitalGraph(model.HospId,model.FacId,model.RegId,model.DiasplayName,model.DateRange,model.FDate,model.TDate);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }


        }
        [HttpPost]
        [Route("api/EMRVitals/GetVitalSignType")]
        public IHttpActionResult GetVitalSignType([FromBody] VitalModel Model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);

            try
            {
                ds = objcls.GetVitalSignType();
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }
        }

        [HttpPost]
        [Route("api/EMRVitals/GetPatientPreviousVitalHistory")]
        public IHttpActionResult GetPatientPreviousVitalHistory([FromBody] PatientPreviousVitalsHistory Model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetPatientPreviousVitals(Model.HospId,Model.FacId,Model.RegId,Model.EncId,Model.ViewType,Model.DRange,Model.FDate,Model.TDate,Model.VitalSignId,Model.Abnormal);
                if (ds != null)
                {
                    string json = JsonConvert.SerializeObject(ds);
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
            }
        }
        [HttpPost]
        [Route("api/EMRVitals/SaveEMRMUDLogVitalValues")]
        public IHttpActionResult SaveEMRMUDLogVitalValues([FromBody] SaveEMRMUDLogVitalValues Model)
        {
            BaseC.EMRVitals objcls = new BaseC.EMRVitals(sConString);
            DataSet ds = new DataSet();
            try
            {
                objcls.SaveEMRMUDLogVitalValues(Model.HospitalLocationId, Model.RegistrationId, Model.EncounterId, Model.DoctorId, Model.UserId, Model.VitalValues);

                string json = JsonConvert.SerializeObject("Saved");
                return Ok(json);

            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                //ds.Dispose();
            }
        }


    }

   
}
