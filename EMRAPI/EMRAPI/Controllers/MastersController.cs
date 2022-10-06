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

namespace EMRAPI.Controllers
{
    public class MastersController : ApiController
    {
        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        [HttpPost]
        [Route("api/Masters/getAllFormulationUnits")]
        public IHttpActionResult getAllFormulationUnits()
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getAllFormulationUnits();
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
        [Route("api/Masters/getAllFormulationRoutes")]
        public IHttpActionResult getAllFormulationRoutes()
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getAllFormulationRoutes();
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
        [Route("api/Masters/GetFormulationMaster")]
        public IHttpActionResult GetFormulationMaster([FromBody] GetFormulationMaster Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFormulationMaster(Model.FormulationId, Model.HospitalLocationId, Model.Active, Model.UserId);
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
        [Route("api/Masters/GetMedicationRouteMaster")]
        public IHttpActionResult GetMedicationRouteMaster()
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetMedicationRouteMaster();
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
        [Route("api/Masters/GetFrequencyMaster")]
        public IHttpActionResult GetFrequencyMaster()
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getFrequencyMaster();
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
        [Route("api/Masters/GetFoodMaster")]
        public IHttpActionResult GetFoodMaster([FromBody] GetFoodMaster Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFood(Model.HospitalLocationId, Model.FacilityId, Model.Active);
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
        [Route("api/Masters/GetIndentType")]
        public IHttpActionResult GetIndentType([FromBody] GetIndentType Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetIndentType(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/Masters/GetUnitMaster")]
        public IHttpActionResult GetUnitMaster([FromBody] GetUnitMaster Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetUnitMaster(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/Masters/GetVolumeUnitMaster")]
        public IHttpActionResult GetVolumeUnit([FromBody] GetVolumeUnitMaster Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetUnitMaster(Model.HospitalLocationId, Model.FacilityId, Model.IsVolume);
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
        [Route("api/Masters/GetMedicationSet")]
        public IHttpActionResult GetMedicationSet([FromBody] GetMedicationSet Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);
            DataSet ds = new DataSet();
            try
            {
                ds = objcls.GetEMRDrugSet(Model.HospitalLocationId, Model.MedicationSetId);
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
        [Route("api/Masters/getItemAttributes")]
        public IHttpActionResult getItemAttributes([FromBody] getItemAttributes Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getItemAttributes(Model.HospitalLocationId, Model.ItemId, Model.StoreId, Model.FacilityId, Model.USerId);
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
        [Route("api/Masters/GetMedicationSetDetails")]
        public IHttpActionResult GetMedicationSetDetails([FromBody] GetMedicationSetDetails Model)
        {
            BaseC.EMRMasters objcls = new BaseC.EMRMasters(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRDrugSetDetail(Model.HospitalLocationId, Model.MedicationSetId, Model.ItemId);
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
        [Route("api/Masters/GetItemConversionFactor")]
        public IHttpActionResult GetItemConversionFactor([FromBody] GetItemConversionFactor Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetItemConversionFactor(Model.ItemId);
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
        [Route("api/Masters/GetGenericDetails")]
        public IHttpActionResult GetGenericDetails([FromBody] GetGenericDetails Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetGenericDetails(Model.GenericId, Model.GenericName, Model.Active, Model.HospitalLocationId, Model.UserId, Model.CIMSCategoryId, Model.CIMSSubCategoryId);
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
        [Route("api/Masters/GetNetworkWiseItemsWithStock")]
        public IHttpActionResult GetNetworkWiseItemsWithStock([FromBody] GetNetworkWiseItemsWithStock Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getItemsWithStock_NetworkV3(Model.HospitalLocationId, Model.StoreId, Model.ItemId, Model.GenericId, Model.UserId, Model.FacilityId, Model.SupplierId,
                                            Model.ItemName, Model.WithStockOnly, Model.ItemNo, Model.ItemSubCategoryId, Model.Usedtype, Model.AllBrand, Model.RegistratioNo);
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
        [Route("api/Masters/ISCalculationRequired")]
        public IHttpActionResult ISCalculationRequired([FromBody] ISCalculationRequired Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.ISCalculationRequired(Model.ItemId);
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
        [Route("api/Masters/GetEMRDocumentCategory")]
        public IHttpActionResult GetEMRDocumentCategory([FromBody] GetEMRDocumentCategory Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetEMRDocumentCategory(Model.HospitalLocationId);
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
        [Route("api/Masters/GetFileServersetup")]
        public IHttpActionResult GetFileServersetup([FromBody] GetFileServersetup Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFileServersetup(Model.FacilityId);
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
        [Route("api/Masters/getDepartmentMain")]
        public IHttpActionResult getDepartmentMain([FromBody] getDepartmentMain Model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getDepartmentMain(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/Masters/GetServiceName")]
        public IHttpActionResult GetServiceName([FromBody] GetServiceName Model)
        {
            BaseC.clsServiceTaging objcls = new BaseC.clsServiceTaging(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetServiceName(Model.ServiceId);
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
        [Route("api/Masters/GetServiceTagingWithRange")]
        public IHttpActionResult GetServiceTagingWithRange([FromBody] GetServiceTagingWithRange Model)
        {
            BaseC.clsServiceTaging objcls = new BaseC.clsServiceTaging(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetServiceTagingWithRange(Model.ServiceId, Model.Age, Model.Gender, Model.AgeType, Model.FacilityId);
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
        [Route("api/Masters/GetDocumentType")]
        public IHttpActionResult GetDocumentType([FromBody] GetDocumentType Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDocumentType(Model.HospitalLocationId, Model.DocumentTypeName);
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
        [Route("api/Masters/GetDocumentCategory")]
        public IHttpActionResult GetDocumentCategory([FromBody] GetDocumentCategory Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDocumentCategory(Model.HospitalLocationId, Model.DocumentCategoryName);
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
        [Route("api/Masters/SaveDocumentType")]
        public IHttpActionResult SaveDocumentType([FromBody] SaveDocumentType Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveDocumentType(Model.HospitalLocationId, Model.DocumentTypeName, Model.UserId);
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
        [Route("api/Masters/SaveDocumentCategory")]
        public IHttpActionResult SaveDocumentCategory([FromBody] SaveDocumentCategory Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.SaveDocumentCategory(Model.HospitalLocationId, Model.DocumentCategoryName, Model.UserId, Model.Active, Model.DocumentCategoryId);
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
        [Route("api/Masters/GetAttachmentCategory")]
        public IHttpActionResult GetAttachmentCategory([FromBody] GetAttachmentCategory Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetAttachmentCategory(Model.HospitalLocationId);
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
        [Route("api/Masters/GetPeriodTypeMaster")]
        public IHttpActionResult GetPeriodTypeMaster(getFrequencyMaster model)
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetPeriodTypeMaster(model.LanguageCode);
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
