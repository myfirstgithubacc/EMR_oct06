using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class EMRModel
    {
        public int HospitalLocationId { get; set; }

        public int iFacilityId { get; set; }
        public DateTime dfromDate { get; set; }
        public DateTime dtoDate { get; set; }
        public string cRegistrationNo { get; set; }
        public string cEncounterId { get; set; }
        public int doctorid { get; set; }
        public string Query { get; set; }
        public int UserId { get; set; }
        public int FormId { get; set; }
        public int RegistrationId { get; set; }


    }
    public class SingleScreenDashboard
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string xmlProblemDetails { get; set; }
        public string sProvisionalDiagnosis { get; set; }
        public string xmlVitalString { get; set; }
        public string strXMLDrug { get; set; }
        public string strXMLOther { get; set; }
        public int ProvisionalDiagnosisId { get; set; }
        public int DiagnosisSearchId { get; set; }
        public int bitNKDA { get; set; }
        public string xmlTemplateDetails { get; set; }
        public int iSign { get; set; }
        public string xmlNonDrugOrder { get; set; }

        // added by kk
        public string sTemplateType { get; set; }
        public int iTemplateId { get; set; }
        public string sTemplateName { get; set; }
        public string EncounterDate { get; set; }
        public string ToDate { get; set; }
        public int pageSize { get; set; }
        public int PageNo { get; set; }
        public bool IsProvisionalDignosis { get; set; }
        public bool IsFinalDignosis { get; set; }

    }
    public class SingleScreenCarePlanDashboard
    {
        public string PlanName { get; set; }
     

        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public string xmlProblemDetails { get; set; }
        public string strXMLDrug { get; set; }
        public string xmlTemplateDetails { get; set; }
        public string xmlOrderProcedure { get; set; }
        public string xmlOrderProcedure1 { get; set; }
        public string xmlDiagnosis { get; set; }

    }
    public class OutSaveSingleScreenDashboard
    {
        public string chvErrorStatus { get; set; }
    }

    public class GetProvisionalDiagnosisSearchCodes
    {
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
        public string KeywordType { get; set; }
    }

    public class DeletePatientProvisionalDiagnosis
    {
        public int ProvisionalDiagnosisId { get; set; }
        public int UserId { get; set; }

        public int idoctorId { get; set; }

        public int id { get; set; }
    }

    public class GetEvitalresult
    {
        public int EncounterID { get; set; }
    }
    public class Canceltodayproblem
    {
        public int ProblemId { get; set; }
        public int RegistrationID { get; set; }
        public int Encounterid { get; set; }
        public int HospitalLocationID { get; set; }
        public int FacilityId { get; set; }
        public int Pageid { get; set; }
        public int UserId { get; set; }
        public int Shownote { get; set; }
    }

    public class getEMRSingleScreenDataInTransit
    {
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int EncodedBy { get; set; }
    }

    public class SaveSingleScreenDataInTransit
    {
        public int HospId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public string Complaints { get; set; }
        public bool IsNoAllergies { get; set; }
        public int AllergyId { get; set; }
        public string AllergyName { get; set; }
        public string AllergyType { get; set; }
        public int ServerityId { get; set; }
        public string VitalHT { get; set; }
        public string VitalWT { get; set; }
        public string VitalHC { get; set; }
        public string VitalT { get; set; }
        public string VitalR { get; set; }
        public string VitalP { get; set; }
        public string VitalBPS { get; set; }
        public string VitalBPD { get; set; }
        public string VitalMAC { get; set; }
        public string VitalSPO2 { get; set; }
        public string VitalBMI { get; set; }
        public string VitalBSA { get; set; }
        public string History { get; set; }
        public string PastHistory { get; set; }
        public string PreviousTreatment { get; set; }
        public string Examination { get; set; }
        public string NutritionalStatus { get; set; }
        public string CostAnalysis { get; set; }
        public string PlanOfCare { get; set; }
        public int DiagnosisSearchKeyId { get; set; }
        public string ProvisionalDiagnosis { get; set; }
        public string OrderType { get; set; }
        public int NonDrugDoctorId { get; set; }
        public string NonDrugOrder { get; set; }
        public int EncodedBy { get; set; }
    }

    public class getPAstHistory
    {
        public int regID { get; set; }
        public int encounterID { get; set; }
        public int doctorID { get; set; }
    }

    public class getPAstCheifComplaints
    {
        public int regID { get; set; }
        public int encounterID { get; set; }
        public int doctorID { get; set; }
    }
    public class getSingleScreenUserTemplates
    {
        public int SpecialisationId { get; set; }
        public int FacilityId { get; set; }
        public int DoctorId { get; set; }
    }
    public class GetEmployeeWithResource
    {
        public int SpecialisationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public int HospitalLocationId { get; set; }
    }
    public class SearchAppointmentList
    {
        public int HospitalLocationId { get; set; }
        public string DoctorId { get; set; }
        public int FacilityId { get; set; }
        public string DateRange { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int RegistrationNo { get; set; }
        public string PatientName { get; set; }
        public string OldRegistrationNo { get; set; }
        public string EncounterNo { get; set; }
        public string MobileNo { get; set; }
        public string EnrolleNo { get; set; }
        public int LoginFacilityId { get; set; }
        public int StatusId { get; set; }
        public int intemrStatusId { get; set; }
        public string chrEncounterType { get; set; }

        public string intSpecialisationId { get; set; }
    }
    public class GetDoctorPatientLists
    {
        public int HospitalLocationId { get; set; }
        public int DoctorId { get; set; }
        public int FacilityId { get; set; }
        public string DateRange { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int RegistrationNo { get; set; }
        public string PatientName { get; set; }
        public string OldRegistrationNo { get; set; }
        public string EncounterNo { get; set; }
        public string MobileNo { get; set; }
        public string EnrolleNo { get; set; }
        public int LoginFacilityId { get; set; }
        public int StatusId { get; set; }
        public int intemrStatusId { get; set; }
        public string chrEncounterType { get; set; }
        public int intSpecialisationId { get; set; }



    }

    public class GetDoctorTimeSpecialisation
    {
        public int FacilityId { get; set; }
        public int HospitalLocationId { get; set; }
    }
    public class ValidateUserForEMRFile
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationID { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
    }
    public class getEMRPatientDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string RegistrationNo { get; set; }
        public string EncounterNo { get; set; }
        public int UserId { get; set; }
        public int RegistrationId { get; set; }
    }
    public class EMRAcknowledgeNextPatient
    {
        public int FacilityId { get; set; }
        public int DoctorId { get; set; }
    }
    public class GetDefaultTemplate
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
    }
    public class GetDefaultPageByUser
    {
        public int GroupId { get; set; }
        public string URLAddress { get; set; }
    }
    public class GetTokentocallDoc
    {
        public string IPAddress { get; set; }
        public string Tokenno { get; set; }
    }

    public class QMSClose
    {
        public int DoctorId { get; set; }
        public int facilityid { get; set; }
    }
    public class GetDisplayCurrentPatient
    {
        public string IPAddress { get; set; }
        public string Tokenno { get; set; }
        public int iHospitalLocationId { get; set; }
        public int FacilityId { get; set; }

        public int EncounterId { get; set; }

        public int AppointmentResourceId { get; set; }

        public string sFlag { get; set; }

        public string sDoctorName { get; set; }

        public string sDeptName { get; set; }

        public string sRegistrationNo { get; set; }
        public int Employeeid { get; set; }


    }
    public class CallPatient
    {
        public int UserId { get; set; }
        public string Tokenno { get; set; }
        public int ServiceID { get; set; }
        public string CounterID { get; set; }
        public int ZoneID { get; set; }
    }
    public class GetReferralDetailCount
    {
        public int UserId { get; set; }
    }
    public class getPatientShortDetail
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetPatientDashBordSection
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetPatientDashboard
    {
        public int RegistrationId { get; set; }
    }
    public class getDoctorImageDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int DoctorID { get; set; }
    }
    public class EMRGetReportName
    {
        public int ReportId { get; set; }
    }
    public class getCustomizedPatientReportHeader
    {
        public int HeaderId { get; set; }
        public string PatientType { get; set; }
        public int EncounterId { get; set; }
        public int DoctorId { get; set; }
        public int RegistrationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class getEMRPrintCaseSheet
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string TemplateName { get; set; }
        public int TemplateId { get; set; }
        public string TemplateType { get; set; }
        public bool ChronologicalOrder { get; set; }

        public int ReportId { get; set; }

        
    }
    public class GetTemplateStyle
    {
        public int HospitalLocationId { get; set; }
    }
    //public class GetEMRTemplates
    //{
    //    public int EncounterId { get; set; }
    //    public int RegistrationId { get; set; }
    //    public string EREncounterId { get; set; }

    //    public int TemplateId { get; set; }
    //}
    public class UpDateEMRToTranslateLanguage
    {
        public int RegistrationId { get; set; }
        public string EMRToTranslateLanguage { get; set; }
    }
    public class getTaggedReportFormatDetails
    {
        public int DoctorId { get; set; }
    }
    public class GetICMPatientSummaryDetails
    {
        public int HospitalLocationID { get; set; }
        public string RegistrationId { get; set; }
        public string EncounterId { get; set; }
        public int ReportId { get; set; }
        public int FacilityId { get; set; }
    }
    public class FillAttachmentDownloadDropdownIP
    {
        public string DiagSampleId { get; set; }
        public string FileName { get; set; }
    }
    public class FillAttachmentDownloadDropdownOP
    {
        public string DiagSampleId { get; set; }
        public string FileName { get; set; }
    }
    public class BindPullFarwordAndRemarks
    {
        public int EncounterId { get; set; }
    }
    public class GetChiefProblem
    {
        public int HospitalLocationID { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int DoctorId { get; set; }
        public string Daterange { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchCriteriya { get; set; }
        public bool IsDistinct { get; set; }
        public bool IsChronic { get; set; }
        public int ProblemId { get; set; }
        public string VisitType { get; set; }
    }
    public class SavePatientProblems
    {
        public int HospitalLocationID { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int PageId { get; set; }
        public string xmlProblemDetails { get; set; }
        public int UserId { get; set; }
        public string Remarks { get; set; }
        public bool IsPregment { get; set; }
        public bool IsBreastFeed { get; set; }
        public bool IsShowNote { get; set; }
        public int DoctorId { get; set; }
    }

    public class BindFavouriteProblemsOutput
    {
        public int ProblemId { get; set; }
        public string ProblemDescription { get; set; }
        public string SNOMEDCode { get; set; }
    }

    public class BindFavouriteProblems
    {
        public string strSearchCriteria { get; set; }
        public int DoctorID { get; set; }
        public string Type { get; set; }
    }
    public class PopulateAllProblem
    {
        public string strSearchCriteria { get; set; }
        public int HospitalLocationId { get; set; }
        public int DoctorID { get; set; }
    }
    public class SaveEMRFavProblem
    {
        public int ProblemId { get; set; }
        public int HospitalLocationId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string ProblemDescription { get; set; }
    }
    public class BindODLQCS
    {
        public int HospitalLocationId { get; set; }
    }
    public class CheckDuplicateProblem
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int ProblemId { get; set; }
        public bool IsChronic { get; set; }
    }
    public class DeleteEMRFavProblem
    {
        public int DoctorId { get; set; }
        public int ProblemId { get; set; }
        public int UserId { get; set; }
    }
    public class SaveHPIRemarks
    {
        public int EncounterId { get; set; }
        public string Remarks { get; set; }
        public bool IsPullForwardComplaint { get; set; }
    }
    public class GetProblemsList
    {
        public string SearchText { get; set; }
        public int HospitalLocationId { get; set; }
    }
    public class getIVFRegistrationId
    {
        public int HospitalLocationId { get; set; }
        public int IVFId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
    }
    public class getIVFPatient
    {
        public int RegistrationId { get; set; }
        public int IVFId { get; set; }
    }
    public class ISDiagnosesExits
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetPatientDiagnosis
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }

        public int DoctorId { get; set; }
        public int DiagnosisGroupId { get; set; }
        public int DiagnosisSubGroupId { get; set; }
        public string DateRange { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string SearchKeyword { get; set; }
        public bool IsDistinct { get; set; }
        public int StatusId { get; set; }
        public string VisitType { get; set; }
        public bool IsChronic { get; set; }
        public int DiagnosisId { get; set; }
    }
    public class GetDiscriptionandICDID
    {
        public string IcdCode { get; set; }
    }
    public class GetICDSubGroup
    {
        public string DiagnosisGroupId { get; set; }
    }
    public class GetDiagnosisList
    {
        public int DiagnosisGroupId { get; set; }
        public int DiagnosisSubGroupId { get; set; }
    }
    public class BindDiagnosis
    {
        public int DiagnosisGroupId { get; set; }
        public int DiagnosisSubGroupId { get; set; }
        public string DiagnosisCode { get; set; }
    }
    public class BindFavouriteDiagnosis
    {
        public int DoctorId { get; set; }
        public int DiagnosisGroupId { get; set; }
        public int DiagnosisSubGroupId { get; set; }
        public string DiagnosisCode { get; set; }
    }
    public class SaveFavouriteDiagnosis
    {
        public int DoctorId { get; set; }
        public int DiagnosisId { get; set; }
        public int UserId { get; set; }
    }
    public class GetDiagnosisDescription
    {
        public string IcdCode { get; set; }
    }
    public class CheckDiagnosisExcluded
    {
        public int HospitalLocationId { get; set; }
        public int Facilityid { get; set; }
        public int RegistrationId { get; set; }
        public string IcdCode { get; set; }
    }
    public class CheckPatientPrimaryDiagnosis
    {
        public int DiagnosisId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class CheckValidForPrimaryDiagnosis
    {
        public int DiagnosisId { get; set; }
    }
    public class CheckDuplicateDiagnosis
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int DiagnosisId { get; set; }
        public bool IsChronic { get; set; }
    }
    public class SavePatientDiagnosis
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string DoctorId { get; set; }
        public int PageId { get; set; }
        public string DiagnosisXML { get; set; }
        public string PatientAlertXML { get; set; }
        public int UserId { get; set; }
        public bool IsPullDiagnosis { get; set; }
        public bool IsShowNote { get; set; }
        public int MRDCode { get; set; }
    }
    public class DeleteFavouriteDiagnosis
    {
        public int DoctorId { get; set; }
        public int DiagnosisId { get; set; }
        public int UserId { get; set; }
    }
    public class TaggingStaticTemplateWithTemplateField
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int SectionId { get; set; }
        public int FieldId { get; set; }
        public int TemplateId { get; set; }
        public int UserId { get; set; }
    }
    public class DeletePatientDiagnosis
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int DiagnosisId { get; set; }
        public int UserId { get; set; }
        public int PageId { get; set; }
    }
    public class LogEducationAndMonograph
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int HospitalLocationId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
    }
    public class CheckDuplicateServiceOrder
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int ServiceId { get; set; }
    }
    public class GetServiceDescriptionForOrderpage
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int ServiceId { get; set; }
        public int OrderSetId { get; set; }
        public int CompanyId { get; set; }
        public int SponsorId { get; set; }
        public int CardId { get; set; }
        public int Option { get; set; }
        public int TemplateId { get; set; }
        public string xmlServiceIds { get; set; }
    }
    public class DeleteFavouriteOrder
    {
        public int DoctorId { get; set; }
        public int FavoriteId { get; set; }
        public int UserId { get; set; }


    }
    public class GetFavouriteOrder
    {
        public int DoctorId { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
    }
    public class GetSearchServices
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int DepartmentId { get; set; }
        public string SubDepartmentId { get; set; }
        public string ServiceName { get; set; }

        public int SecGroupId { get; set; }

        public int EmployeeId { get; set; }
        

    }
    public class PopulateInvestigationSetUserWise
    {
        public int HospitalLocationId { get; set; }
        public int Employeeid { get; set; }
    }
    public class GetPatientConsultingDoctor
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetPatientProvisionalDiagnosis
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
        public int ProvisionalDiagnosisId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class SaveFavouriteOrder
    {
        public int DoctorId { get; set; }
        public int ServiceId { get; set; }
        public int UserId { get; set; }
    }
    public class ValidateErxPatientXML
    {
        public int RegistrationId { get; set; }
        public int DoctorId { get; set; }
    }
    public class GetEncounterCompany
    {
        public int EncounterId { get; set; }
    }
    public class SaveServiceOrderEMR
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string xmlServiceList { get; set; }
        public string XMLPatientAlert { get; set; }
        public string Remark { get; set; }
        public int UserId { get; set; }
        public int DoctorId { get; set; }
        public int CompanyId { get; set; }
        public string OrderType { get; set; }
        public string PayerType { get; set; }
        public string PatientOPIP { get; set; }
        public int InsuranceId { get; set; }
        public int CardId { get; set; }
        public string OrderDate { get; set; }
        public string ChargeCalculationRequired { get; set; }
        public bool IsAllergyReviewed { get; set; }
        public int IsERorEMRServices { get; set; }
        public int RequestId { get; set; }
        public string xmlTemplateDetails { get; set; }
    }
    public class GetSectionTreeForPatientsForms
    {
        public int HospitalLocationId { get; set; }
        public int TemplateId { get; set; }
        public string GenderType { get; set; }
        public int FormId { get; set; }
        public int ReportId { get; set; }
    }
    public class GetTemplateDetailsTabular
    {
        public int TemplateId { get; set; }
        public string GenderType { get; set; }
        public int EncounterId { get; set; }
        public int SectionId { get; set; }
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int RegistrationId { get; set; }
        public int ResultSetId { get; set; }
        public int EpisodeId { get; set; }
        public int FacilityId { get; set; }
        public int EmployeeId { get; set; }
        public string PatientOPIP { get; set; }
    }
    public class GetTemplateDetails
    {
        public int TemplateId { get; set; }
        public string GenderType { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public bool ShowPreviousData { get; set; }
        public int OrderId { get; set; }
        public int RecordId { get; set; }
        public int ResultSetId { get; set; }
        public int EpisodeId { get; set; }
        public int SectionId { get; set; }
        public string TemplateDetails { get; set; }
        public int ServiceId { get; set; }
        public int RequestId { get; set; }
        public int EmployeeId { get; set; }
        public string PatientOPIP { get; set; }
        public int OrderDetailId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
        public int EREncounterId { get; set; }
    }
    public class getEncounterEMRStatus
    {
        public int EncounterId { get; set; }
    }
    public class getPermissionConfidentialUsers
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int TemplateId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetGroupWiseMenuTagging
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int GroupId { get; set; }
        public int ModuleId { get; set; }
        public string PageCode { get; set; }
    }
    public class GetNewBornBabyMotherDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class getEMRTemplateReportSetup
    {
        public int ReportId { get; set; }
        public int TemplateId { get; set; }
        public int DoctorId { get; set; }
        public int Active { get; set; }
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
        public string Flag { get; set; }
    }
    public class DoLockUnLock
    {
        public string AbsolutePagePath { get; set; }
        public string Type { get; set; }
        public string EncounterId { get; set; }
        public string HospitalLocationId { get; set; }
        public string FormId { get; set; }
        public string TemplateId { get; set; }
        public string Lock { get; set; }
    }
    public class IsSectionAddendum
    {
        public int SectionId { get; set; }
    }
    public class GetSectionTree
    {
        public int HospitalLocationId { get; set; }
        public int TemplateId { get; set; }
        public string GenderType { get; set; }
    }
    public class GetFormatText
    {
        public int FormatId { get; set; }
    }

    public class PatientDiagnosisInputParam
    {
        public int Registraionid { get; set; }
        public int HospitalLocationid { get; set; }
        public int Encounterid { get; set; }
        public string dob { get; set; }
    }

    //yogesh

    public class PatientDiagnosisOutParam
    {
        public string Description { get; set; }
        public string VisitType { get; set; }
        public string Date { get; set; }

        public bool IsChronic { get; set; }
        public  bool IsPrimaryDiagnosis { get; set; }
        public bool IsFinalDiagnosis { get; set; }
    }

    public class PatientDrugOrderOutParam
    {
        public string Medicine { get; set; }
        public string Detail { get; set; }

        public string Date { get; set; }
    }

    public class PatientImmunizationOutParam
    {
        public int ImmunizationId { get; set; }
        public string ImmunizationName { get; set; }

        public string Status { get; set; }

        public string Duedate { get; set; }

        public string GivenDate { get; set; }
    }

    public class PatientOrderProcedureOutParam
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ServiceName { get; set; }
        public string Date { get; set; }
        public string SOURCE { get; set; }
        public int LABNO { get; set; }
        public int StationId { get; set; }
        public int ServiceIds { get; set; }
        public string StatusCode { get; set; }
        public string FlagName { get; set; }
        public bool OutsourceInvestigation { get; set; }

    }

    public class PatientOrderChifComplaintsOutParam
    {
        public int Id { get; set; }
        public string ProblemDescription { get; set; }
        public string EntryDate { get; set; }
    }

    public class PatinetPastHistory
    {
        public string ValueWordProcessor { get; set; }
        public string EncodedDate { get; set; }
    }

    public class SingleScreenPatientDetailInParam
    {
        public int RegistrationNo { get; set; }

        public int RegistrationId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }

    public class SingleScreenPatientDetailOutParam
    {
        
        public int RegistrationId { get; set; }
        public int RegistrationNo { get; set; }
        public string PatientName { get; set; }
        public string DOB { get; set; }
        public string GenderAge { get; set; }
        public string EncounterDate { get; set; }
        public string EncounterNo { get; set; }
        public string DoctorName { get; set; }

        public int EncounterId { get; set; }
        public string EncDate { get; set; }
        public string EncMonth { get; set; }
        public string EncDay { get; set; }
        public string OPIP { get; set; }
        public string Diagnosis { get; set; }


    }

    public class PatientAllergiesOutParam
    {
        public string Allergies { get; set; }
    }

    public class EmployeeData
    {
        public int iHospID { get; set; }
        public int iStationId { get; set; }
        public int iEmpTypeId { get; set; }
        public string xmlEmployeeType { get; set; }
        public string EmpName { get; set; }
        public int iMobileNo { get; set; }
        public int EncodedBy { get; set; }
        public string cDepartmentIdendification { get; set; }
        public int intFacilityId { get; set; }
    }
    public class ResultReviewedData
    {
        public int DiagSampleId { get; set; }
        public string Source { get; set; }

        public string xmlDiagSampleId { get; set; }
        public int reviewedStatus { get; set; }

        public DateTime reviewedDate { get; set; }
        public string reviewedComments { get; set; }

        public int reviewedBy { get; set; }
        public string LabFlagValue { get; set; }
        public string TestResultStatus { get; set; }
        public int iNoSMS { get; set; }


    }
    //Ujjwal need to create document
    public class GetTemplateSectionRow
    {
        public int SelectedNode { get; set; }
    }
    public class GetAllTypeTemplates
    {
        public int HospitalLocationId { get; set; }
        public string sType { get; set; }
    }

    public class getTemplateDataObjectQuery
    {
        public int DataObjectId { get; set; }
    }
    public class getDataObjectExecute
    {
        public string Query { get; set; }
        public int DataObjectId { get; set; }
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
        public int IVFId { get; set; }
    }
    public class GetTemplateFieldFormats
    {
        public int SpecialisationId { get; set; }
        public int FieldId { get; set; }
    }
    public class getEMRTemplateWordData
    {
        public int FieldId { get; set; }
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
    }
    public class getEMRTemplateReportSequence
    {
        public int ReportId { get; set; }
    }
    public class SavePatientNotesData
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int FormId { get; set; }
        public int IsPullForward { get; set; }
        public int PageId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int OrderId { get; set; }
        public int TemplateTypeId { get; set; }
        public string xmlTemplateDetails { get; set; }
        public string xmlTabularTemplateDetails { get; set; }
        public int IsShowNote { get; set; }
        public int SectionId { get; set; }
        public int RecordId { get; set; }
        public int EpisodeId { get; set; }
        public int EncodedBy { get; set; }
        public int OrderDetailId { get; set; }
        public int OrderRequestId { get; set; }
        public int SubDeptId { get; set; }
        public int ProviderId { get; set; }
        public DateTime? ChangeDate { get; set; }

        public bool IsApproved { get; set; }
        public int AdvisingDoctorId { get; set; }
        
    }
    public class updateFurtherAckServiceOrderDetail
    {
        public int ServiceOrderDetailsId { get; set; }
        public int ServiceId { get; set; }
    }
    public class GetEMRPatientForms
    {
        public int FormId { get; set; }
        public int EncounterId { get; set; }
        public int ModuleId { get; set; }
        public int GroupId { get; set; }
    }
    public class GetEMRTemplateStatic
    {
        public int HospitalLocationId { get; set; }
    }
    public class GetPatientROS
    {
        public int EncounterId { get; set; }
        public int TemplateId { get; set; }
    }
    public class GetTemplateDisplayUserName
    {
        public int HospitalLocationId { get; set; }
        public int TemplateId { get; set; }
    }
    public class getEMRTemplateVisitRecoreds
    {
        public int EncounterId { get; set; }
        public int TemplateId { get; set; }
        public int FacilityId { get; set; }
    }
    public class getEMRTemplate
    {
        public int HospitalLocationId { get; set; }
        public string EmployeeType { get; set; }
        public int SpecialisationId { get; set; }
        public int ServiceId { get; set; }
        public string ApplicableFor { get; set; }
        public int TemplateId { get; set; }
        public string TemplateType { get; set; }
        public int FacilityId { get; set; }
        public bool IsAddendum { get; set; }
    }
    public class getTemplateEnteredList
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class getPatientClosedEpisode
    {
        public int TemplateId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class CreateTemplateEpisode
    {
        public int EpisodeId { get; set; }
        public int TemplateId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string SaveMode { get; set; }
        public int EpisodeClose { get; set; }
        public int Active { get; set; }
        public int UserId { get; set; }
    }
    public class getPatientPullforwardTemplate
    {
        public int TemplateId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class getTemplateEpisode
    {
        public int TemplateId { get; set; }
        public int RegistrationId { get; set; }
    }
    public class GetTemplateRequiredServices
    {
        public string ServiceOrderDetailIds { get; set; }
        public string TagType { get; set; }
        public int FacilityId { get; set; }
    }
    public class IsTemplateRequiredForService
    {
        public int Option { get; set; }
        public int ServiceId { get; set; }
        public int TemplateId { get; set; }
    }
    public class GetServiceTemplates
    {
        public int TemplateId { get; set; }
        public int FacilityId { get; set; }
        public int ServiceId { get; set; }
        public int SubDepartmentId { get; set; }
        public string xmlServiceIds { get; set; }
        public string TagType { get; set; }
    }
    public class SaveEMRTemplateResultSet
    {
        public int ResultSetId { get; set; }
        public string ResultSetName { get; set; }
        public int TemplateId { get; set; }
        public string xmlTemplateData { get; set; }
        public int DoctorId { get; set; }
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
    }
    public class getTemplateResultSet
    {
        public int TemplateId { get; set; }
        public int DoctorId { get; set; }
    }
    public class GetICMNurse
    {
        public int HospitalLocationId { get; set; }
    }
    public class NonDrugOrder
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class SaveNonDrugOrder
    {
        public int NonDrugId { get; set; }
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string OrderDate { get; set; }
        public string PrescriptionText { get; set; }
        public string OrderType { get; set; }
        public int DoctorId { get; set; }
        public int AcknoledgeedBy { get; set; }
        public string AcnoledgekDate { get; set; }
        public string AcknoledgeRemark { get; set; }
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public bool Status { get; set; }
        public int SaveFor { get; set; }

        public bool isApprobalReqd { get; set; }
        
    }
    public class DeleteFavouriteNonDrugOrder
    {
        public int DoctorId { get; set; }
        public int FavouriteId { get; set; }
    }
    public class GetavouriteNonDrugOrder
    {
        public string SearchCriteria { get; set; }
        public int DoctorId { get; set; }
    }
    public class SaveFavouriteNonDrugOrder
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string PrescriptionText { get; set; }
    }
    public class GetViewHistory
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetDateWiseGroupingTemplate
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class GetPatientAllergies
    {
        //  public int ReportId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
        //  public int HeaderId { get; set; }
        //  public DateTime AppointmentDate { get; set; }
        //  public int DoctorId { get; set; }
        public int RegistrationId { get; set; }
        // public int FacilityId { get; set; }
        // public int EncounterId { get; set; }
        public int Active { get; set; }
    }
    public class getPatientNoteStatus
    {
        public int EncounterId { get; set; }
        public int TemplateId { get; set; }
    }
    public class SavePatientSignData
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public bool IsSigned { get; set; }
        public int DoctorId { get; set; }
        public string PatientSummary { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
    }
    public class GetEMRTemplateId
    {
        public int HospitalLocationId { get; set; }
        public string TemplateName { get; set; }
    }
    public class updateEMRPatientForms
    {
        public string PatientSummary { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetPatientIllustrationImages
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class SaveSeenByDoctor
    {
        public int EncounterId { get; set; }
        public int DoctorId { get; set; }
        public int SeenStatus { get; set; }
        public int UserId { get; set; }
    }
    public class IsCopyCaseSheetAuthorized
    {
        public int UserId { get; set; }
        public int HospitalLocationId { get; set; }
    }
    public class GetPatientPreviousVitals
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetPatientProblemsHPI
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetPatientBasicDetails
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetPatientDrugHistory
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class GetPatientCurrentMedication
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class GetPreviousMedicinesIP
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
        public string PreviousMedication { get; set; }
        public int IndentId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string IndentCode { get; set; }
    }
    public class GetOPMedicines
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
        public string PreviousMedication { get; set; }
    }
    public class GetPatientServices
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetDietOrderDetailInNote
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetDoctorReferral
    {
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationNo { get; set; }
        public int EncounterId { get; set; }
        public string EncounterType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetPatientImmunization
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
        public int ScheduleId { get; set; }
        public int ImminizationId { get; set; }
    }
    public class GetEncounterFollowUpAppointment
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetDoctorProgressNote
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int DoctorId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }

        public int ProgressNoteId { get; set; }
    }

    public class GetDoctorNote
    {
        public int ProgressNoteId { get; set; }
        public string DoctorName { get; set; }
        public DateTime EnteredDate { get; set; }
    }
        public class GetPatientDailyInjections
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetLabTestResultForNote
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int LabNo { get; set; }
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
        public int UserId { get; set; }
        public bool IsShowAllParameters { get; set; }
    }
    public class GetDiagLabSensitivityResultForNote
    {
        public int DiagSampleId { get; set; }
        public int ResultId { get; set; }
    }
    public class vital
    {
        public int Hospitalid { get; set; }
        public int UserId { get; set; }
        public string date { get; set; }
        public int Encounterid { get; set; }
    }

    public class CloseQMSCall
    {
        public int TokenNo { get; set; }
        public int UserID { get; set; }
        public string ActivityDesc { get; set; }
    }
    public class CancelVital
    {
        public int CancelremarkId { get; set; }
        public int RegId { get; set; }
        public int EncId { get; set; }
        public int HospId { get; set; }
        public int FacilityId { get; set; }
        public int PageId { get; set; }
        public int Encodedby { get; set; }
        public string xmlstring { get; set; }
    }

    public class chart
    {

        public int vitalId { get; set; }
        public string Category { get; set; }

    }
    public class VitalGraph
    {
        public int HospId { get; set; }
        public int FacId { get; set; }
        public int RegId { get; set; }
        public string DiasplayName { get; set; }
        public string DateRange { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
    }
    public class SaveVitals
    {
        public string EntryDate { get; set; }
        public int RegId { get; set; }
        public int EncId { get; set; }
        public int HospId { get; set; }
        public int FacilityId { get; set; }
        public int PageId { get; set; }
        public int Encodedby { get; set; }
        public string xmlstring { get; set; }
        public int TemplateFieldId { get; set; }
        public int VitalDetailsId { get; set; }
        public int TriageTypeId { get; set; }
         

    }
    public class dischargeReport
    {
        public int ReportId { get; set; }
        public int HeaderId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DoctorId { get; set; }
        public int RegistrationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetVisitNotesPharmacy
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class GetDoctorSpecialisation
    {
        public int DoctorId { get; set; }
    }

    public class getEMRDoctorWisePatientNote
    {
        public int RegistrationId { get; set; }
        public int DoctorId { get; set; }
    }
    public class GetPatientVisits
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public string ViewType { get; set; }
        public int DoctorId { get; set; }
        public int TemplateId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class SaveEMRDoctorWisePatientNote
    {
        public int RegistrationId { get; set; }
        public int DoctorId { get; set; }
        public string PatientNotes { get; set; }
        public int UserId { get; set; }
    }
    public class GetPatientDiagnosisOPIP
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int FacilityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    // Pre auth
    public class PreAuth
    {
        public int RegistrationId { get; set; }
        public int LoginSpecialisationID { get; set; }
        public int EncounterId { get; set; }
        public string EmirateID { get; set; }
        public string Query { get; set; }
    }
    public class PreAuthInputs
    {
        public string rdlOPIP { get; set; }
        public bool chkctg { get; set; }
        public string txtCTGremark { get; set; }
        public bool chkpregenent { get; set; }
        public DateTime rdLMP { get; set; }
        public string ddlx { get; set; }
        public bool chkwrokrelated { get; set; }
        public bool chksportrelated { get; set; }
        public bool chkprofessionalSport { get; set; }
        public bool chkRTArelated { get; set; }
        public string txtAlcohalintake { get; set; }
        public string txtreatmentdetail { get; set; }
        public string txtClinicalFindings { get; set; }
        public string txtTreatmentPlan { get; set; }
        public string txtremark { get; set; }
        public int RegistrationID { get; set; }
        public int EncounterId { get; set; }
        public int UserID { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetIPServiceOrderDetails
    {

        public int iHospitalLocationId { get; set; }
        public int iFacilityId { get; set; }
        public int iRegistrationId { get; set; }
        public int iEncounterId { get; set; }
        public int iBillId { get; set; }
        public int iDepartmentId { get; set; }
        public string chrDepartmentType { get; set; }
    }
    public class PatientLabResultHistory
    {
        public int iLoginFacilityId { get; set; }
        public int iHostId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string iRegNo { get; set; }
        public int iProviderId { get; set; }
        public int iPageSize { get; set; }
        public int iPageNo { get; set; }
        public bool AbnormalResult { get; set; }
        public bool CriticalResult { get; set; }
        public int iStatusId { get; set; }
        public int FacilityId { get; set; }
        public string chvEncounterNo { get; set; }
        public string Source { get; set; }
        public int SubDeptId { get; set; }
        public int ServiceId { get; set; }
        public int StationId { get; set; }
        public string ResultStatus { get; set; }
        public string DischargeSummary { get; set; }
    }
    public class GetDiagLabResultDynamicGrid
    {
        public int iLoginFacilityId { get; set; }
        public int iHostId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public string iRegNo { get; set; }
        public int iProviderId { get; set; }
        public int iPageSize { get; set; }
        public int iPageNo { get; set; }
        public bool AbnormalResult { get; set; }
        public bool CriticalResult { get; set; }
        public int iStatusId { get; set; }
        public int FacilityId { get; set; }
        public string chvEncounterNo { get; set; }
        public string xmlService { get; set; }
        public string xmlFieldId { get; set; }
        public string chvEncounterId { get; set; }
        public string cStationId { get; set; }

        public int iLoginId { get; set; }
    }
    public class getLabNoInvFormatsHistory
    {
        public string Source { get; set; }
        public int iLoginFacilityId { get; set; }
        public int iDiagSampleId { get; set; }
        public int iServiceID { get; set; }
        public string StatusCode { get; set; }
        public int iHospID { get; set; }
    }
    public class GrowthChart
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public string DisplayName { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public string DayType { get; set; }
        public int GrowthChartId { get; set; }

        public string Measurmentsystem { get; set; }
    }
    public class Allergy
    {
        public int RegistrationId { get; set; }
        public int HospId { get; set; }
        public int FacilityId { get; set; }
        public int interfaceFor { get; set; }
        public int Id { get; set; }
        public string CancelRemarks { get; set; }
        public int PageId { get; set; }
        public int Flag { get; set; }
        public int EncounterId { get; set; }
        public int EncodedBy { get; set; }
        public string showNotesVal { get; set; }
        public string ItemName { get; set; }
        public int AllergyTypeId { get; set; }
    }
    public class AllergySaveInputs
    {
        public int HospId { get; set; }
        public int FacilityId { get; set; }
        public int PageId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string xmlDrugAllergyDetails { get; set; }
        public string xmlOtherAllergyDetails { get; set; }
        public int EncodedBy { get; set; }
        public int IsNKDA { get; set; }
        public int IsShowNote { get; set; }
    }
    public class GenericDetail
    {
        public int GenericId { get; set; }
        public string GenericName { get; set; }
        public int Active { get; set; }
        public int HospId { get; set; }
        public int EncodedBy { get; set; }
    }
    public class Pharmacy
    {
        public int HospId { get; set; }
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public string ItemNo { get; set; }

        public int active { get; set; }
        public string ItemsearchName { get; set; }
        public int ItemBrandId { get; set; }
        public int GenericId { get; set; }
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public int SupplierId { get; set; }
        public string ItemName { get; set; }
        public int WithStockOnly { get; set; }
        public string NHISDrugsList { get; set; }
    }
    public class Security
    {
        public Int16 iHospID { get; set; }
        public Int32 iFacilityID { get; set; }
        public Int32 iRegId { get; set; }
        public Int32 iEncountId { get; set; }
        public Int32 iPageId { get; set; }
        public Int32 iTemplateId { get; set; }
        public Int32 iEncodedBy { get; set; }
        public Int32 iEmpId { get; set; }
        public string iAuditStatus { get; set; }
        public string chvIPAddres { get; set; }

    }
    public class ProvisionalDiagnosis
    {
        public int HospitalId { get; set; }
        public int intSpecialisationId { get; set; }
        public int FacilityId { get; set; }

        public string strSearchCriteria { get; set; }
        public int DoctorID { get; set; }

        public string DiagnosisDescription { get; set; }
        public int userid { get; set; }
    }
    public class saveDiagnosisInputs
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string ProvisionalDiagnosis { get; set; }
        public int DiagnosisSearchId { get; set; }
        public int UserId { get; set; }
        public int ProvisionalDiagnosisId { get; set; }
        public int ProviderId { get; set; }
        public DateTime? ChangeDate { get; set; }

        public string Remarks { get; set; }

        public bool bAdmitting { get; set; }

        public bool bProvisional { get; set; }

        public bool bFinal { get; set; }

        public bool bChronic { get; set; }

        public bool bDischarge { get; set; }

    }

    public class Referrals
    {
        public int HospitalId { get; set; }
        public int intSpecialisationId { get; set; }
        public int FacilityId { get; set; }

    }
    public class CheckGenerateeOPRxXml
    {
        public int IndentId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetStoreToChangefromWard
    {
        public int HospitalLocationId { get; set; }
        public int GroupId { get; set; }
        public int FacilityId { get; set; }
        public int DepartmentId { get; set; }
        public int StoreId { get; set; }
        public string DocumentType { get; set; }
    }
    public class GetDoctorPrescriptionInstructions
    {
        public int DoctorId { get; set; }
    }
    public class GetPrescriptionRemarks
    {
        public int RemarkId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int Active { get; set; }
        public int DoctorId { get; set; }
    }
    public class CheckPrimaryDiagnosisForEncounter
    {
        public int EncounterId { get; set; }
        public string RegistraionId { get; set; }
    }
    public class SaveMedicineOrderOP
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int IndentType { get; set; }
        public int StoreId { get; set; }
        public int DoctorId { get; set; }
        public int IsPregnant { get; set; }
        public int IsBreastFeeding { get; set; }
        public string xmlItems { get; set; }
        public string xmlItemDetail { get; set; }
        public string xmlPatientAlerts { get; set; }
        public int UserId { get; set; }
        public string xmlFrequencyTime { get; set; }
        public bool IsConsumable { get; set; }
        public string xmlUnApprovedPrescriptionIds { get; set; }
        public int intDrugAdminIn { get; set; }
    }
    public class SaveMedicineOrderIP
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int IndentType { get; set; }
        public int StoreId { get; set; }
        public int DoctorId { get; set; }
        public string xmlItems { get; set; }
        public string xmlItemDetail { get; set; }
        public string Remarks { get; set; }
        public int DrugOrderType { get; set; }
        public int UserId { get; set; }
        public string xmlFrequencyTime { get; set; }
        public bool IsConsumable { get; set; }
        public string xmlUnApprovedPrescriptionIds { get; set; }
    }
    public class IsDOctorEprescriptionEnabled
    {
        public int DoctorId { get; set; }
    }
    public class UpdateDHARefNo
    {
        public string DHARefNo { get; set; }
        public int IndentId { get; set; }
    }
    public class GetClinicianLoginforErx
    {
        public int DoctorId { get; set; }
    }
    public class deleteUnApprovedPrescriptions
    {
        public int UnAppPrescriptionId { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
        public int FacilityId { get; set; }
    }
    public class getScreeningParameters
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class getFavoriteDrugWithStock
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int GenericId { get; set; }
        public int DoctorId { get; set; }
        public string FormularyType { get; set; }
        public string ItemName { get; set; }
    }
    public class getPreviousMedicinesNew
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }

        public int EncounterId { get; set; }
        public int IndentId { get; set; }
        public int ItemId { get; set; }
        public string PreviousMedication { get; set; }
        public string ItemName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class getPreviousMedicationOP
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int IndentId { get; set; }
        public int ItemId { get; set; }
        public string PreviousMedication { get; set; }
        public string ItemName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int UserId { get; set; }
    }
    public class getOPMedicinesNew
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int IndentId { get; set; }
        public int ItemId { get; set; }
        public string PreviousMedication { get; set; }
        public string ItemName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class DeleteFavoriteMedicine
    {
        public int DoctorId { get; set; }
        public int ItemId { get; set; }
        public string FormularyType { get; set; }
        public int UserId { get; set; }
        public int GenericId { get; set; }

        public int FavoriteId { get; set; }
    }
    public class SaveFavoriteMedicine
    {
        public int DoctorId { get; set; }
        public int ItemId { get; set; }
        public string FormularyType { get; set; }
        public int UserId { get; set; }
        public int DoseId { get; set; }
        public int UnitId { get; set; }
        public int StrengthId { get; set; }
        public int FormulationId { get; set; }
        public int RouteId { get; set; }
        public int FrequencyId { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        public int FoodRelationshipId { get; set; }
        public string StrengthValue { get; set; }
        public string Instructions { get; set; }
        public int GenericId { get; set; }
    }
    public class getMedicinesOPList
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int IndentId { get; set; }
        public int RegistrationId { get; set; }
        public string IndentNo { get; set; }
    }
    public class SaveDhaError
    {
        public int IndentId { get; set; }
        public string Error { get; set; }
        public string FormName { get; set; }
        public string MethodName { get; set; }
        public string DHAMethodName { get; set; }
        public int Facilityid { get; set; }
        public int ModuleId { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class SaveDoctorPrescriptionInstructions
    {
        public int DoctorId { get; set; }
        public string InstructionText { get; set; }
        public int UserId { get; set; }
    }
    public class getDiagnosis
    {
        public int HospitalLocationId { get; set; }
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
    }
    public class GetEMRExistingMedicationOrder
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
        public int ItemId { get; set; }
        public string OPIP { get; set; }
    }
    public class getGenericRouteUnitOfSelectedDrug
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int ItemId { get; set; }
    }
    public class getDrugAllergiesInterfaceCode
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
    }
    public class getUnApprovedPrescriptions
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
    }
    public class getCopyPreviousMedicinesOP
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class SaveUnApprovedPrescriptions
    {
        public int UnAppPrescriptionId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int IndentId { get; set; }
        public string IndentNo { get; set; }
        public string IndentDate { get; set; }
        public int IndentTypeId { get; set; }
        public string IndentType { get; set; }
        public int GenericId { get; set; }
        public string GenericName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public bool CustomMedication { get; set; }
        public int FrequencyId { get; set; }
        public string FrequencyName { get; set; }
        public double Frequency { get; set; }
        public double Dose { get; set; }
        public string Duration { get; set; }
        public string DurationText { get; set; }
        public string Instructions { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string cType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string CIMSItemId { get; set; }
        public string CIMSType { get; set; }
        public int VIDALItemId { get; set; }
        public string XMLData { get; set; }
        public string PrescriptionDetail { get; set; }
        public int FormulationId { get; set; }
        public string FormulationName { get; set; }
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public int StrengthId { get; set; }
        public string StrengthValue { get; set; }
        public double Qty { get; set; }
        public string FoodRelationship { get; set; }
        public int FoodRelationshipID { get; set; }
        public int ReferanceItemId { get; set; }
        public string ReferanceItemName { get; set; }
        public int DoseTypeId { get; set; }
        public string DoseTypeName { get; set; }
        public bool NotToPharmacy { get; set; }
        public bool IsInfusion { get; set; }
        public bool IsInjection { get; set; }
        public bool IsStop { get; set; }
        public bool IsCancel { get; set; }
        public string Volume { get; set; }
        public int VolumeUnitId { get; set; }
        public string TotalVolume { get; set; }
        public string InfusionTime { get; set; }
        public string TimeUnit { get; set; }
        public int FlowRate { get; set; }
        public int FlowRateUnit { get; set; }
        public string VolumeUnit { get; set; }
        public string XmlVariableDose { get; set; }
        public bool IsOverride { get; set; }
        public string OverrideComments { get; set; }
        public string OverrideCommentsDrugToDrug { get; set; }
        public string OverrideCommentsDrugHealth { get; set; }
        public string XMLFrequencyTime { get; set; }
        public bool IsSubstituteNotAllowed { get; set; }
        public string ICDCode { get; set; }
        public int UserId { get; set; }
    }
    public class StopCancelPreviousMedication
    {
        public int iHospitalLocationId { get; set; }
        public int iFacilityId { get; set; }
        public int iIndentId { get; set; }
        public int iItemId { get; set; }
        public int UserId { get; set; }
        public int iRegistrationId { get; set; }
        public int iEncounterId { get; set; }
        public int bCancelStop { get; set; }
        public string sStopRemarks { get; set; }
        public string OPIP { get; set; }
        public int IndentDetailsId { get; set; }
        public int GenericId { get; set; }
        public int StopBy { get; set; }
    }
    public class GenerateeOPRxXml
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int IndentId { get; set; }
        public string DispositionFlag { get; set; }
    }


    public class InActiveSingleScreenDataInTransit
    {
        public int TransitId { get; set; }
        public int Userid { get; set; }
    }
    public class GetEMRFileRequest
    {
        public int UserId { get; set; }
        public int RegistrationId { get; set; }
        public int iLoginFacilityId { get; set; }
        public int EncounterId { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public int RegistrationNo { get; set; }
        public string EncounterNo { get; set; }
        public string PatientName { get; set; }
        public string RequestedBy { get; set; }
        public char RequestStatus { get; set; }
    }

    public class GetReferralDetailForSerach
    {
        public int UserId { get; set; }
        public int EncId { get; set; }
        public String EncounterType { get; set; }
        public int FacilityId { get; set; }
        public string RegistrationNo { get; set; }
        public int ConcludeReferral { get; set; }
        public int ReferralId { get; set; }
        public string PatientName { get; set; }
        public int ReferralType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class SaveProvisionalDiagnosisSearchCodes
    {
        public int HospitalLocationId { get; set; }
        public int DiagnosisSearchId { get; set; }
        public string DiagnosisSearchCode { get; set; }
        public int Active { get; set; }
        public int UserId { get; set; }
        public string KeywordType { get; set; }
    }
    public class SaveDiagnosisStaus
    {
        public int HospitalLocationId { get; set; }
        public int StatusId { get; set; }
        public int UserId { get; set; }
        public string StatusName { get; set; }
    }
    public class GetFavoriteSentences
    {
        public int DoctorId { get; set; }
        public string SearchCriteria { get; set; }
    }
    public class DeleteFavSentence
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public int SentenceId { get; set; }
    }
    public class SaveFavSentence
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string SentenceIds { get; set; }
    }

    public class GetPatientDOB
    {
        public int RegistrationId { get; set; }
        public int HospitalLocationId { get; set; }
    }
    public class GetImmunizationStatus
    {
        public int HospitalLocationId { get; set; }
    }
    public class GetPatientImmunizationDueDates
    {
        public int RegistrationId { get; set; }
        public int HospitalLocationId { get; set; }
        public string DateOfBirth { get; set; }
    }
    public class InActivePatientImmunization
    {
        public int ImmunizationId { get; set; }
        public int UserId { get; set; }
        public DateTime EncodedDate { get; set; }
        public string CancelRemarks { get; set; }

        public string Type { get; set; }
    }
    public class GetEncounterFacilityForImmunization
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
    }
    public class GetImmunizationGivenBy
    {
        public int HospitalLocationId { get; set; }
    }
    public class SavePatientImmunization
    {
        public int Id { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int SchedulerId { get; set; }
        public int ImmunizationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string GivenDate { get; set; }
        public int GivenBy { get; set; }
        public string BatchNo { get; set; }
        public bool IsRejectedByPatient { get; set; }
        public bool IsVaccineGivenByOutsider { get; set; }
        public string Remarks { get; set; }
        public int UserId { get; set; }
        public int PageId { get; set; }
        public int BrandId { get; set; }
    }
    public class DeleteEMRTemplateResultSet
    {
        public int ResultSetId { get; set; }
        public int UserId { get; set; }
    }
    public class stopPrescription
    {
        public string DetailsIds { get; set; }
        public int CancelReasonId { get; set; }
        public int UserId { get; set; }
        public int EncounterId { get; set; }
        public string OPIP { get; set; }
    }
    public class GetTemplateGroup
    {
        public int FacilityId { get; set; }
        public int HospitalLocationId { get; set; }
    }
    public class GetFavouriteTemplates
    {
        public int DoctorId { get; set; }
    }
    public class GetTemplateGroupDetails
    {
        public int GroupId { get; set; }
    }
    public class SaveFavouriteTemplates
    {
        public int DoctorId { get; set; }
        public int TemplateId { get; set; }
        public int UserId { get; set; }
        public string TranType { get; set; }
    }
    public class BindGrowthChartFor2Yto20YHTWT
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
    }
    public class SaveReferralSlip
    {
        public int ReferralReplyId { get; set; }
        public int ReferralId { get; set; }
        public int HospitalLocationId { get; set; }
        public int EncounterId { get; set; }
        public DateTime Referraldate { get; set; }
        public int ReferDoctorId { get; set; }
        public string Doctornote { get; set; }
        public int Urgent { get; set; }
        public bool status { get; set; }
        public int UserId { get; set; }
        public string ReferralSlip { get; set; }
        public int Active { get; set; }
        public string DoctorReferral { get; set; }
        public int SpecializationId { get; set; }
    }
    public class GetReferralDetail
    {
        public int UserId { get; set; }
        public int EncounterId { get; set; }
        public string EncounterType { get; set; }
        public int FacilityId { get; set; }
        public string RegistrationNo { get; set; }
        public int ConcludeReferral { get; set; }
        public int ReferralId { get; set; }
    }
    public class SaveReferralSlipAutoSave
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int SpecializationId { get; set; }
        public int ReferDoctorId { get; set; }
        public int Urgent { get; set; }
        public string Doctornote { get; set; }
        public int UserId { get; set; }
    }
    public class GetReferralDetailAutoSave
    {
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
    }

    public class ShowPageCheck
    {
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
        public string EREncounterId { get; set; }
        public int FormId { get; set; }
        public int TemplateId { get; set; }
    }
    public class GetROS
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationNo { get; set; }
        public int EncounterId { get; set; }

    }
    public class populateAllergyType
    {
        public int HospitalLocationId { get; set; }

    }
    public class SaveAllergyMaster
    {
        public int HospitalLocationId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
    public class DeActivateAllergyMaster
    {
        public int HospitalLocationId { get; set; }
        public int AllergyId { get; set; }
        public int UserId { get; set; }
    }
    public class UpdateAllergyMaster
    {
        public int HospitalLocationId { get; set; }
        public int TypeId { get; set; }
        public int AllergyId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
    public class DeActivateAllergyTypeMaster
    {
        public int HospitalLocationId { get; set; }
        public int TypeId { get; set; }
        public int UserId { get; set; }
    }
    public class UpdateAllergyTypeMaster
    {
        public int HospitalLocationId { get; set; }
        public int TypeId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
    public class SaveAllergyTypeMaster
    {
        public int HospitalLocationId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
    public class GetICDCode
    {
        public string ICDCode { get; set; }
    }
    public class GetEMRDoctorPatientwise
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
    }

    public class GetEMRDataForSingleScreen
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public string sTemplateType { get; set; }
        public int iTemplateId { get; set; }
        public string sTemplateName { get; set; }
        public string EncounterDate { get; set; }
        public string ToDate { get; set; }
        public int pageSize { get; set; }
        public int PageNo { get; set; }
        public string IsCopyLastOPDSummary { get; set; }

        public int DoctorId { get; set; }
    }
    public class SavePatientLastEncounterData
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string TemplateIdDetails { get; set; }
        public string PrescriptionDetails { get; set; }
    }
    public class GetProblemHPIDetails
    {
        public int ProblemId { get; set; }
    }
    public class SaveHPIProblems
    {
        public int HospitalLocationId { get; set; }
        public int ProblemId { get; set; }
        public string OnsetDate { get; set; }
        public string NoOfOccurrence { get; set; }
        public string PriorIllnessDate { get; set; }
        public string RelievingFactors { get; set; }
        public string AggravatingFactors { get; set; }
        public string DeniesSymptomsText1 { get; set; }
        public string DeniesSymptomsText2 { get; set; }
        public string DeniesSymptomsText3 { get; set; }
        public string DeniesSymptomsText4 { get; set; }
        public string DeniesSymptomsText5 { get; set; }
        public int UserId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
        public int PageId { get; set; }
    }
    public class GetEMRPatientAlerts
    {
        public int HospitalLocationId { get; set; }
        public string AlertType { get; set; }
        public int RegistrationId { get; set; }

    }
    public class getEncounterBasedOnPrescription
    {
        public int PrescriptionId { get; set; }
        public string OPIP { get; set; }
    }
    public class CheckUserDoctorOrNot
    {
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
    }

    public class SaveEmailLog
    {
        public int FacilityID { get; set; }
        public int HospitalLocationId { get; set; }
        public string EmailType { get; set; }
        public string EMailTo { get; set; }
        public string PatientName { get; set; }
        public int RegistrationId { get; set; }
        public string RegistrationNo { get; set; }
        public int EncounterId { get; set; }
        public string EncounterNo { get; set; }
        public string LabNo { get; set; }
        public int SendTODoctorID { get; set; }
        public string SendToDoctorName { get; set; }
        public int AttendingDOctorID { get; set; }
        public string AttendingDOctorName { get; set; }
        public int Active { get; set; }
        public int EncodedBy { get; set; }
        public string EncodedDate { get; set; }
    }
    public class GetLabDetailsEmail
    {
        public string chvLabNo { get; set; }
    }
    public class GetPatientAssessments
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string GroupingDate { get; set; }
    }
    public class GetRequestService
    {
        public int iHospitalLocationId { get; set; }
        public int iFacilityId { get; set; }
        public string EncounterNo { get; set; }
        public int iDepartmentId { get; set; }
        public int iSubDeptId { get; set; }
        public string cFromDate { get; set; }
        public string cToDate { get; set; }
        public string Source { get; set; }
        public string ProcedureType { get; set; }
        public string PatientName { get; set; }
        public string RegistrationNo { get; set; }
    }


    public class DeletePatientServices
    {
        public int iServiceId { get; set; }
        public int iHospitalID { get; set; }
        public int iRegistrationId { get; set; }
        public int iEncounterId { get; set; }
        public int iFacilityId { get; set; }
        public bool bCancelService { get; set; }
        public int iPageId { get; set; }
        public int iEncodedby { get; set; }
        public int iServiceOrderDtlId { get; set; }

        public bool bRequestToDepartment { get; set; }

    }
    public class SaveProblemMasters
    {
        public int HospitallocationId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public int Id { get; set; }
    }
    public class SaveProblemMastersSeq
    {
        public string xmlDetails { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
    }

    public class getSingleScreenUserTemplatesIconOut
    {
        public string TemplateCode { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsCollapse { get; set; }

    }

    public class UpdateICMFinalize
    {
        public int HospitalLocationId { get; set; }
        public int SummaryID { get; set; }
        public bool IsFinalize { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string ChangeDate { get; set; }
    }
    public class GetICMTemplateStyle
    {
        public int HospitalLocationId { get; set; }
    }
    public class GetICMTemplateName
    {
        public int EncounterId { get; set; }
    }
    public class GetEMRTemplateReportSetup1
    {
        public int ReportId { get; set; }
        public int TemplateId { get; set; }
        public int DoctorId { get; set; }
        public string chvFlag { get; set; }
        public bool bitActive { get; set; }
        public int HospitalLocationId { get; set; }
        public string ReportType { get; set; }
        public int UserId { get; set; }
    }
    public class CancelEMRDischargeOrDeathSummary
    {
        public int HospitalLocationId { get; set; }
        public int SummaryId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
        public string CancelRemarks { get; set; }
    }
    public class SaveUpdateICMPatientSummary
    {
        public int SummaryId { get; set; }
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int FormatId { get; set; }
        public string PatientSummary { get; set; }
        public int SignDoctorId { get; set; }
        public int UserId { get; set; }
        public string sEncodedDate { get; set; }
        public string DeathDate { get; set; }
        public string DischargeDate { get; set; }
        public int FacilityId { get; set; }
        public string synopsis { get; set; }
        public string Addendum { get; set; }
        public bool IsMultiDepartmentCase { get; set; }
        public int CaseId { get; set; }
        public string xmlDepartmentIds { get; set; }

    }
    public class CancelReferral
    {
        public int FacilityId { get; set; }
        public int UserId { get; set; }
        public string sRemarks { get; set; }
        public int ReferralId { get; set; }
        public string Source { get; set; }


    }

    public class DiagPrintLabResult
    {
        public int iHospId { get; set; }
        public int iLoginFacilityId { get; set; }
        public int iLabNo { get; set; }
        public int intStationId { get; set; }
        public string chvServiceIds { get; set; }
        public string source { get; set; }
    }
    public class GetReportHeader
    {
        public string Type { get; set; }
    }

    public class GetAdmissionDetails
    {
        public int HospitalLocationId { get; set; }
        public int userid { get; set; }
    }

    public class UserLoginAudit

    {
        public int UserID { get; set; }
        public string UserHostAddress { get; set; }
        public string sStatus { get; set; }


    }
    public class LoginExpiryMessage
    {
        public int Facitiyid { get; set; }
        public int DayDifferent { get; set; }
        public string ShowExpWarning { get; set; }
    }

    public class DeleteFavoriteDrugsWithFavoriteId
    {

        public int FavoriteId { get; set; }
        public int EncodedBy { get; set; }
    }

    public class SetPatinetCheckInPut
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int EncodedBy { get; set; }
        public int flag { get; set; }


    }

    public class GetDataObjectDetails
    {
        public string HeaderId { get; set; }
        public string AppointmentDate { get; set; }
        public string DoctorId { get; set; }
        public int RegistrationId { get; set; }
        public string FacilityId { get; set; }
        public int EncounterId { get; set; }
    }


    public class ClinicalPath
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string TemplateId { get; set; }
        public string DrugName { get; set; }
        public int PlanId { get; set; }

        public int DayId { get; set; }
        public int DayDetailId { get; set; }
        public int SelectedDayId { get; set; }
        public int SelectedDayDetailId { get; set; }
        public int EncodedBy { get; set; }
        public int EncounterId { get; set; }
        public int RegistrationId { get; set; }
        public string xmlSpecialtyDetails { get; set; }
        public string xmlServiceDetails { get; set; }
        public string xmlDrugClassDetails { get; set; }
        public string xmlPrescriptionDetails { get; set; }
        public string xmlTemplateDetails { get; set; }
        public string xmlTemplateFieldDetails { get; set; }
        public string TemplateType { get; set; }
        public bool IsCopy { get; set; }
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string PlanName { get; set; }
        public int DoctorId { get; set; }
        public string PlanType { get; set; }
        public string DocumentName { get; set; }
        public string xmlDays { get; set; }
        public string xmlDiagnosis { get; set; }
        public bool IsSurgical { get; set; }
        public int SpecialityId { get; set; }
        public int ServiceId { get; set; }
        public int DrugClassId { get; set; }
        public int GenericId { get; set; }
        public int ItemId { get; set; }
        public int FrequencyId { get; set; }
        public int Days { get; set; }
        public string DaysType { get; set; }
        public decimal Dose { get; set; }
        public int DoseUnitId { get; set; }
        public int FoodRelationId { get; set; }
        public string Instruction { get; set; }
        public int TemplateTaggingId { get; set; }
        public int RouteId { get; set; }
        public double totalQty { get; set; }

        public string ChiefComplaints { get; set; }

        public string History { get; set; }

        public string Examination { get; set; }

        public string PlanOfCare { get; set; }
        public string xmlItems { get; set; }

        public string xmlItemDetail { get; set; }

        public string xmlProblems { get; set; }


    }

    public class SaveProblemMaster
    {
        public string Description { get; set; }
        public int DoctorId { get; set; }
        public bool IsFavourite { get; set; }
        public int HospitalLocationID { get; set; }
        public int EncodedBy { get; set; }
        public int specialisationid { get; set; }
    }

    public class DoctoPanelSettingParam
    {
        public int DoctorId { get; set; }


    }
    public class DoctorPanelSettingOutParam
    {
        public int DoctorId { get; set; }
        public string DoctorPanelSetting { get; set; }


    }

    public class DoctorProblem
    {
        public int DoctorId { get; set; }
        public int ProblemId { get; set; }

    }
    public class ProgressNoteFormat
    {
        public int FormatId { get; set; }
        public int FacilityId { get; set; }
        public int Active { get; set; }
        public string FormatName { get; set; }
        public string FormatText { get; set; }
        public int EncodedBy { get; set; }
        public string chvErrorStatus { get; set; }

    }
}



