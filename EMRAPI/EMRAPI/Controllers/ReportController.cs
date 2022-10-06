using EMRAPI.Models;
using System;
using System.Data;
using System.Web.Http;
using Newtonsoft.Json;
using Microsoft.Reporting.WebForms;
using System.Web.Hosting;
using System.IO;
using System.Collections.Generic;

namespace EMRAPI.Controllers
{
    public class ReportController : ApiController
    {
        private string sConString = System.Configuration.ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
        [HttpPost]
        [Route("api/Reports/Get_Rpt_MRD_CountryWiseVisit")]
        public IHttpActionResult Get_Rpt_MRD_CountryWiseVisit([FromBody] Get_Rpt_MRD_CountryWiseVisit Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.Get_Rpt_MRD_CountryWiseVisit(Model.HospitalLocationId, Model.FacilityId, Model.NationalityIds, Model.FromDate, Model.ToDate, Model.VisitType);
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
        [Route("api/Reports/Get_Rpt_GetVisitSummary")]
        public IHttpActionResult Get_Rpt_GetVisitSummary([FromBody] Get_Rpt_GetVisitSummary Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.Get_Rpt_GetVisitSummary(Model.HospitalLocationId, Model.FacilityId, Model.FromDate, Model.ToDate, Model.VisitType);
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
        [Route("api/Reports/GetDashboardRecievable")]
        public IHttpActionResult GetDashboardRecievable([FromBody] GetDashboardRecievable Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDashboardRecievable(Model.HospitalLocationId, Model.FacilityId, Model.FromDate, Model.ToDate, Model.CompanyIds, Model.ReportType, Model.Yearids, Model.InvoiceId);
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
        [Route("api/Reports/GetYears")]
        public IHttpActionResult GetYears()
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetYears();
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
        [Route("api/Reports/GetDashboardRecievableDetails")]
        public IHttpActionResult GetDashboardRecievableDetails([FromBody] GetDashboardRecievableDetails Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDashboardRecievableDetails(Model.HospitalLocationId, Model.FacilityId, Model.FromDate, Model.ToDate, Model.ReportType, 
                    Model.CompanyId, Model.SubCompanyId, Model.YearMonth, Model.CompanyIds, Model.Year);
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
        [Route("api/Reports/GetDischargeChecklistMaster")]
        public IHttpActionResult GetDischargeChecklistMaster([FromBody] GetDischargeChecklistMaster Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDischargeChecklistMaster(Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.StatusId);
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
        [Route("api/Reports/GetDischargeChecklistDetail")]
        public IHttpActionResult GetDischargeChecklistDetail([FromBody] GetDischargeChecklistDetail Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetDischargeChecklistDetail(Model.EncounterId, Model.ChecklistId);
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
        [Route("api/Reports/UpdateEncouterStatus")]
        public IHttpActionResult UpdateEncouterStatus([FromBody] UpdateEncouterStatus Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.UpdateEncouterStatus(Model.EncounterId, Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.StatusId,
                    Model.UserId, Model.EDod, Model.DischargeStatus, Model.xmlChecklist);
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
        [Route("api/Reports/GetTopPrescriptionListBasedOnICDCodes")]
        public IHttpActionResult GetTopPrescriptionListBasedOnICDCodes([FromBody] GetTopPrescriptionListBasedOnICDCodes Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTopPrescriptionListBasedOnICDCodes(Model.EncounterId);
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
        [Route("api/Reports/GetTopOrderListBasedOnICDCodes")]
        public IHttpActionResult GetTopOrderListBasedOnICDCodes([FromBody] GetTopOrderListBasedOnICDCodes Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetTopOrderListBasedOnICDCodes(Model.EncounterId);
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
        [Route("api/Reports/GetServiceDetail")]
        public IHttpActionResult GetServiceDetail([FromBody] GetServiceDetail Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            DataSet outputresult = new DataSet();
            try
            {
                outputresult = objcls.GetServiceDetail(Model.HospitalLocationId, Model.FacilityId, Model.RegistrationId, Model.EncounterId, Model.ServiceId, 
                    Model.orderSetId, Model.CompanyId, Model.SponsorId, Model.CardId, Model.Option, Model.TemplateId, Model.xmlServiceIds);
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
        [Route("api/Reports/DietUpdateOrder")]
        public IHttpActionResult DietUpdateOrder([FromBody] DietUpdateOrder Model)
        {
            BaseC.clsExternalClass objcls = new BaseC.clsExternalClass(sConString);
            string outputresult = "";
            try
            {
                outputresult = objcls.DietUpdateOrder(Model.HospitalLocationId, Model.FacilityId, Model.DietOrderId, Model.StatusCode, Model.UserId);
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
        public string HospitalLocationId { get; set; }
        public string FacilityId { get; set; }
        public string InvoiceId { get; set; }
        public string RegistrationNo { get; set; }
        public string EncounterId { get; set; }
        public string IndentId { get; set; }

        [HttpGet]
        [Route("api/Report/ReportTest")]

        public IHttpActionResult ReportTest()
        {
            var rootPath = HostingEnvironment.MapPath("~/image/reportTest.pdf");

            var fileData = File.ReadAllBytes(rootPath);

            return Ok(fileData);
        }
        [Route("api/Report/GetOpdReceiptPdf")]
        [HttpPost]
        public IHttpActionResult GetOpdReceiptPdf([FromBody] CommonReportInputParams input)
        {
            ////HospitalLocationId/FacilityId/InvoiceId/RegistrationNo
            ////   var queryString = "1|2|4|900081846";
            //// var queryString = Request.QueryString.Get("queryString");
            ////Response.Write(queryString); 
            var data = input.QueryString.Split('|');
            HospitalLocationId = data[0];
            FacilityId = data[1];
            InvoiceId = data[2];
            //RegistrationNo = data[3];

            //HospitalLocationId = common.myStr(input.HospitalLocationId);
            //FacilityId = common.myStr(input.FacilityId); ;
            //InvoiceId = common.myStr(input.InvoiceId);
            //RegistrationNo = common.myStr(input.reg);

            //return Ok(GetOpdReceiptByteArray());


            // input.ReportName = "PPOPDReceipt";
            var parameter = new List<ReportParameter>();
            parameter.Add(new ReportParameter("Loc_id", HospitalLocationId));
            parameter.Add(new ReportParameter("intFacilityId", FacilityId));
            parameter.Add(new ReportParameter("Invoiceid", InvoiceId));
            parameter.Add(new ReportParameter("year_id", "1"));
            parameter.Add(new ReportParameter("user_id", "1"));

            return Ok(Util.GetSSRSReport(parameter.ToArray(), Util.GetReportByName("OpdReceipt")));
        }



        [Route("api/Report/GetInvesticationReport")]
        [HttpPost]
        public IHttpActionResult GetInvesticationReport([FromBody] InvesticationReport input)
        {
            var parameter = new List<ReportParameter>();
            parameter.Add(new ReportParameter("intLoginFacilityId", input.intLoginFacilityId));
            parameter.Add(new ReportParameter("chrSource", input.chrSource));
            parameter.Add(new ReportParameter("intLabNo", input.intLabNo));
            parameter.Add(new ReportParameter("intStationId", input.intStationId));
            parameter.Add(new ReportParameter("chvServiceIds", input.chvServiceIds));
            parameter.Add(new ReportParameter("RegNo", input.RegNo));
            parameter.Add(new ReportParameter("IPNo", input.IPNo));
            parameter.Add(new ReportParameter("LabNo", input.LabNo));
            parameter.Add(new ReportParameter("intDiagSampleId", input.chvDiagSampleIds));
            parameter.Add(new ReportParameter("facilityName", "Yashoda Super Speciality Hospital."));
            //parameter.Add(new ReportParameter("bitShowAllParameters", "0"));
            //parameter.Add(new ReportParameter("intDiagSampleId", DiagSampleId));
            //parameter.Add(new ReportParameter("username", " "));

            return Ok(Util.GetSSRSReport(parameter.ToArray(), Util.GetReportByName("InvestigationReport")));
        }


        [Route("api/Report/GetPrescriptionReport")]
        [HttpPost]
        public IHttpActionResult GetPrescriptionReport([FromBody] PrescriptionInputReport input)
        {

            //var data = input.QueryString.Split('|');
            //HospitalLocationId = data[0];
            //FacilityId = data[1];
            //EncounterId = data[2];
            //IndentId = data[3];

            var parameter = new List<ReportParameter>();

            //parameter.Add(new ReportParameter("UserName", "OnLine Print"));
            //parameter.Add(new ReportParameter("inyHospitalLocationId", HospitalLocationId));
            //parameter.Add(new ReportParameter("intFacilityId", FacilityId));
            //parameter.Add(new ReportParameter("intEncounterId", EncounterId));
            //parameter.Add(new ReportParameter("intIndentId", IndentId));
            parameter.Add(new ReportParameter("UserName", "OnLine Print"));
            parameter.Add(new ReportParameter("inyHospitalLocationId", common.myStr(input.HospitalLocationId)));
            parameter.Add(new ReportParameter("intFacilityId", common.myStr(input.FacilityId)));
            parameter.Add(new ReportParameter("intEncounterId", common.myStr(input.EncounterId)));
            parameter.Add(new ReportParameter("intIndentId", common.myStr(input.IndentId)));

            return Ok(Util.GetSSRSReport(parameter.ToArray(), Util.GetReportByName("OPIndent")));
           
        }



        [Route("api/Report/GetFtpReport/{ReportName}")]
        [HttpGet]
        public IHttpActionResult GetFtpReport([FromUri] string ReportName)
        {
            return Ok(Util.GetFtpReport(ReportName));
        }
    }
}
