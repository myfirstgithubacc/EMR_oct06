using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class Get_Rpt_MRD_CountryWiseVisit
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string VisitType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string NationalityIds { get; set; }
    }
    public class Get_Rpt_GetVisitSummary
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string VisitType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class GetDashboardRecievable
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string ReportType { get; set; }
        public string Yearids { get; set; }
        public int InvoiceId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CompanyIds { get; set; }
    }
    public class GetDashboardRecievableDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportType { get; set; }
        public int CompanyId { get; set; }
        public int SubCompanyId { get; set; }
        public string YearMonth { get; set; }
        public string CompanyIds { get; set; }
        public string Year { get; set; }

    }
    public class GetDischargeChecklistMaster
    {
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int StatusId { get; set; }
    }
    public class GetDischargeChecklistDetail
    {
        public int ChecklistId { get; set; }
        public int EncounterId { get; set; }
    }
    public class UpdateEncouterStatus
    {
        public int EncounterId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public DateTime? EDod { get; set; }
        public int DischargeStatus { get; set; }
        public string xmlChecklist { get; set; }
    }
    public class GetTopPrescriptionListBasedOnICDCodes
    {
        public int EncounterId { get; set; }
    }
    public class GetTopOrderListBasedOnICDCodes
    {
        public int EncounterId { get; set; }
    }
    public class GetServiceDetail
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int ServiceId { get; set; }
        public int orderSetId { get; set; }
        public int CompanyId { get; set; }
        public int SponsorId { get; set; }
        public int CardId { get; set; }
        public int Option { get; set; }
        public int TemplateId { get; set; }
        public string xmlServiceIds { get; set; }
    }
    public class DietUpdateOrder
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int DietOrderId { get; set; }
        public string StatusCode { get; set; }
        public int UserId { get; set; }
    }

    public class InvesticationReport
    {
        public string intLoginFacilityId { get; set; }
        public string chrSource { get; set; }
        public string intLabNo { get; set; }
        public string intStationId { get; set; }
        public string chvServiceIds { get; set; }
        public string RegNo { get; set; }
        public string IPNo { get; set; }
        public string LabNo { get; set; }
        public string chvDiagSampleIds { get; set; }
        public string facilityName { get; set; }
    }
       
}