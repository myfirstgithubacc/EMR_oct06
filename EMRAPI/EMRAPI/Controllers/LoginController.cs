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

namespace EMRAPI.Controllers
{
    public class LoginController : ApiController
    {
        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;


        [HttpPost]
        [Route("api/Login/CheckQueries")]
        public IHttpActionResult CheckQueries(CheckQueries Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            int outputvalue = objcls.CheckQueries(Model.EmployeeId);
            string json = JsonConvert.SerializeObject(outputvalue, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Login/getEmployeeTypePermission")]
        public IHttpActionResult getEmployeeTypePermission([FromBody] getEmployeeTypePermission Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEmployeeTypePermission(Model.EmployeeId, Model.HospitalLocationId, Model.FacilityId, Model.GroupId);
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
        [Route("api/Login/RedirectionHandler")]
        public IHttpActionResult RedirectionHandler([FromBody] RedirectionHandler Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                string strout = "";
                outputresult = objcls.RedirectionHandler(Model.UserId, Model.IPaddress, Model.Redirection, Model.RedirectionCode, out strout, Model.Timeout);
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
        [Route("api/Login/UpdateQMSDoctorLogin")]
        public IHttpActionResult UpdateQMSDoctorLogin([FromBody] UpdateQMSDoctorLogin Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            objcls.UpdateQMSDoctorLogin(Model.IPaddress);
            string json = JsonConvert.SerializeObject("Success", Formatting.Indented);
            return Ok(json);
        }
        //[HttpPost]
        //[Route("api/Login/DeleteFiles")]
        //public IHttpActionResult DeleteFiles([FromBody] DeleteFiles Model)
        //{
        //    BaseC.Security objSecurity = new BaseC.Security(sConString);
        //    objSecurity.DeleteFiles(Model.DirectoryPath);
        //    string json = JsonConvert.SerializeObject("Success", Formatting.Indented);
        //    return Ok(json);
        //}
        [HttpPost]
        [Route("api/Login/getHospitalLocationNames")]
        public IHttpActionResult getHospitalLocationNames()
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getHospitalLocationNames();
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
        [Route("api/Login/ValidateUserName")]
        public IHttpActionResult ValidateUserName([FromBody] ValidateUserName Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.ValidateUserName(Model.UserName, Model.Password, Model.UserId);
                string json = JsonConvert.SerializeObject(outputresult, Formatting.Indented);
                if (outputresult == null)
                    outputresult = new DataSet();
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
        [Route("api/Login/ValidateUser")]
        public IHttpActionResult ValidateUser([FromBody] ValidateUser Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            bool outresult = objcls.ValidateUser(Model.UserName, Model.Password);
            string json = JsonConvert.SerializeObject(outresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Login/getMainFacilityId")]
        public IHttpActionResult getMainFacilityId([FromBody] getMainFacilityId Model)
        {
            BaseC.Patient objcls = new BaseC.Patient(sConString);
            int outresult = objcls.getMainFacilityId(Model.FacilityId);
            string json = JsonConvert.SerializeObject(outresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Login/LockUser")]
        public IHttpActionResult LockUser([FromBody] LockUser Model)
        {
            BaseC.User objcls = new BaseC.User(sConString);
            bool outresult = objcls.LockUser(Model.UserId);
            string json = JsonConvert.SerializeObject(outresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Login/getFacilityInterfaceDetails")]
        public IHttpActionResult getFacilityInterfaceDetails([FromBody] getFacilityInterfaceDetails Model)
        {
            BaseC.clsEMR objcls = new BaseC.clsEMR(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getFacilityInterfaceDetails(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/Login/getEntrySites")]
        public IHttpActionResult getEntrySites([FromBody] getFacilityInterfaceDetails Model)
        {
            BaseC.clsLISPhlebotomy objcls = new BaseC.clsLISPhlebotomy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getEntrySites(Model.HospitalLocationId, Model.FacilityId);
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
        [Route("api/Login/getHospitalLogo")]
        public IHttpActionResult getHospitalLogo([FromBody] getHospitalLogo Model)
        {
            BaseC.RestFulAPI objcls = new BaseC.RestFulAPI(sConString);
            string outresult = objcls.getHospitalLogo(Model.HospitalLocationId);
            string json = JsonConvert.SerializeObject(outresult, Formatting.Indented);
            return Ok(json);
        }
        [HttpPost]
        [Route("api/Login/GetFinancialyear")]
        public IHttpActionResult GetFinancialyear()
        {
            BaseC.clsPharmacy objcls = new BaseC.clsPharmacy(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetFinancialyear();
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
        [Route("api/Login/getNoTokenIPaddress")]
        public IHttpActionResult getNoTokenIPaddress([FromBody] UpdateQMSDoctorLogin Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getNoTokenIPaddress(Model.IPaddress);
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
        [Route("api/Login/saveQMSlogin")]
        public IHttpActionResult saveQMSlogin([FromBody] saveQMSlogin Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outresult = objcls.saveQMSlogin(Model.UserID, Model.DoctorId, Model.IPadderss, Model.FacilityId);
            string json = JsonConvert.SerializeObject(outresult, Formatting.Indented);
            return Ok(json);
        }

        [HttpPost]
        [Route("api/Login/getSecurityUserPages")]
        public IHttpActionResult getSecurityUserPages([FromBody] SecurityUserPages Model)
        {
            BaseC.Security objcls = new BaseC.Security(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.getSecurityUserPages(Model.Url, Model.HospitalLocationID, Model.FacilityId, Model.UserId);
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
