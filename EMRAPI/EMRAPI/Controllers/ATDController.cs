using EMRAPI.Models;
using System;
using System.Data;
using System.Web.Http;
using Newtonsoft.Json;

namespace EMRAPI.Controllers
{
    public class ATDController : ApiController
    {
        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;


        [HttpPost]
        [Route("api/ATD/getAdmissionDetailsFromRegistrationNo")]
        public IHttpActionResult getAdmissionDetailsFromRegistrationNo([FromBody] getAdmissionDetailsFromRegistrationNo Model)
        {
            BaseC.ATD objcls = new BaseC.ATD(sConString);
            DataSet OutputResult = new DataSet();
            try
            {
                OutputResult = objcls.GetRegistrationId(Model.RegistrationNo);
                string json = JsonConvert.SerializeObject(OutputResult, Formatting.Indented);
                return Ok(json);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                OutputResult.Dispose();
                objcls = null;
            }
        }
    }
}
