using EMRAPI.Models;
using System;
using System.Collections;
using System.Data;
using System.Web.Http;
using Newtonsoft.Json;
using BaseC;
namespace EMRAPI.Controllers
{
    public class BillingController : ApiController
    {
        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
       
        [HttpPost]
        [Route("api/BillingAPI/GetPackageServiceLimit")]
        public IHttpActionResult GetPackageServiceLimit([FromBody] GetPackageServiceLimit Model)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            BaseC.Package objcls = new BaseC.Package(sConString);
            try
            {
                ds = objcls.GetPackageServiceLimit(Model.HospitalLocationId, Model.PackageId, Model.CompanyId, Model.FacilityId);
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
        [Route("api/BillingAPI/SearchPatient")]
        public IHttpActionResult SearchPatient([FromBody] SearchPatient Model)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            try
            {
                ds = objcls.GetPatientDetails(Model.HospitalLocationId, Model.RegistrationId, Model.PatienType, Model.RegistrationNo, Model.EncounterNo);
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
        [Route("api/BillingAPI/GetDepartmentByType")]
        public IHttpActionResult GetDepartmentByType([FromBody] GetDepartmentByType Model)
        {
            Hashtable HshIn = new Hashtable();
            DataSet ds = new DataSet();
            BaseC.clsEMRBilling objcls = new BaseC.clsEMRBilling(sConString);
            try
            {
                ds = objcls.uspGetDepartmentByType(Model.DepartmentType);
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
        [Route("api/BillingAPI/GetAllSubDepartment")]
        public IHttpActionResult GetAllSubDepartment()
        {
            Hashtable HshIn = new Hashtable();
            DataTable ds = new DataTable();
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            try
            {
                ds = objcls.GetAllSubDepartment();
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
        [Route("api/BillingAPI/GetPatientRegistrationNo")]
        public IHttpActionResult GetPatientRegistrationNo([FromBody] GetPatientRegistrationNo Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.GetPatientRegistrationNo(Model.RegistrationId);
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
        [Route("api/BillingAPI/GetPatientDocuments")]
        public IHttpActionResult GetEMRPatientDocuments([FromBody] GetPatientDocuments Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDocuments(Model.ImageCategoryId, Model.RegistrationId, Model.FolderName);
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
        [Route("api/BillingAPI/RetrievePatientDocuments")]
        public IHttpActionResult RetrievePatientDocuments([FromBody] RetrievePatientDocuments Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.RetrievePatientDocuments(Model.RegistrationId, Model.EncounterId, Model.ImageId);
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
        [Route("api/BillingAPI/GetPatientRegistrationId")]
        public IHttpActionResult GetPatientRegistrationId([FromBody] GetPatientRegistrationId Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientRegistrationId(Model.RegistrationNo);
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
        [Route("api/BillingAPI/DeletePatientDocuments")]
        public IHttpActionResult DeletePatientDocuments([FromBody] DeletePatientDocuments Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.DeletePatientDocuments(Model.ImageCategoryId);
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
        [Route("api/BillingAPI/SavePatientDocuments")]
        public IHttpActionResult SavePatientDocuments([FromBody] SavePatientDocuments Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = string.Empty;
            try
            {
                outputresult = objcls.SavePatientDocuments(Model.RegistrationId, Model.EncounterId, Model.ThumbnailName, Model.xmlDocumentDetails, Model.ExtentionType, Model.UserId, Model.DocumentTypeId, Model.DocumentType);
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
        [Route("api/BillingAPI/GetOrderDetailsfromEncounterID")]
        public IHttpActionResult GetOrderDetailsfromEncounterID([FromBody] GetOrderDetailsfromEncounterID Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetOrderDetailsfromEncounterID(Model.EncounterId, Model.FacilityId);
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
        [Route("api/BillingAPI/getServiceOrderDeptTypeWise")]
        public IHttpActionResult getServiceOrderDeptTypeWise([FromBody] getServiceOrderDeptTypeWise Model)
        {
            Hashtable HshIn = new Hashtable();
            EMRBilling.clsOrderNBill objcls = new EMRBilling.clsOrderNBill(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getServiceOrderDeptTypeWise(Model.HospitalLocationId, Model.FacilityId, Model.EncounterId, Model.PageType, Model.ServiceType, Model.InvoiceId, Model.isDetails, Model.InvoiceNo);
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
        [Route("api/BillingAPI/GetPatientDetailsIP")]
        public IHttpActionResult GetPatientDetailsIP([FromBody] GetPatientDetailsIP Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPatientDetailsIP(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.RegistrationNo, Model.UserId, Model.EncounterId, Model.EncounterNo);
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
        [Route("api/BillingAPI/DeActivateOrderServices")]
        public IHttpActionResult DeActivateOrderServices([FromBody] DeActivateOrderServices Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.EMROrders objcls = new BaseC.EMROrders(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.DeActivateOrderServices(Model.ServiceOrderDetailAmountId, Model.ServiceId, Model.OrderId, Model.ServiceOrderDetailId, Model.Remarks, Model.HospitalLocationId, Model.FacilityId, Model.UserId);
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
        [Route("api/BillingAPI/InsertIPServiceDiscount")]
        public IHttpActionResult InsertIPServiceDiscount([FromBody] InsertIPServiceDiscount Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.EMRBilling objcls = new BaseC.EMRBilling(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.InsertIPServiceDiscount(Model.EncounterId, Model.ServiceType, Model.UserId, Model.xmlService);
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
        [Route("api/BillingAPI/getOPIPRegEncDetails")]
        public IHttpActionResult getOPIPRegEncDetails([FromBody] getOPIPRegEncDetails Model)
        {
            Hashtable HshIn = new Hashtable();
            BaseC.clsEMRBilling objcls = new BaseC.clsEMRBilling(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getOPIPRegEncDetails(Model.HospitalLocationId, Model.FacilityId, Model.OPIP, Model.RegistrationId, Model.EncounterId, Model.RegEnc,
                Model.RegistrationNo, Model.EncounterNo, Model.PatientName, Model.FromDate, Model.ToDate, Model.RecordButton, Model.RowNo, Model.PageSize, Model.UserId,
                Model.PageNo, Model.BedNo, Model.AdmissionDischargedStausId, Model.Company, Model.Nationality, Model.PhoneNo, Model.MobileNo, Model.Identityno, Model.PassportNo,
                Model.DateOfBirth, Model.EmailAddress, Model.OldRegistrationNo, Model.EntrySiteId);
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
    }
}
