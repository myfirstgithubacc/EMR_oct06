using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class CommonModel
    {
    }
    public class GfsCountManagement
    {
        public int Employeeid { get; set; }
        public int Qid { get; set; }
    }

    public class GfsCountManagementOut
    {
        public int Id { get; set; }
        public int UHID { get; set; }
        public string Query { get; set; }

    }

    public class ERtoken
    {
        public string ErEncounterID { get; set; }
    }
    public class PatientProblem
    {
        public int EncounterID { get; set; }
    }

    public class VitalsValue
    {
        public int HospitalLocationID { get; set; }
        public string xmlstr { get; set; }
    }
    public class AllergyItemList
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string ItemName { get; set; }
        public int EncodedBy { get; set; }
    }
    public class GetICMSignDoctors
    {
        public int HospitalLocationID { get; set; }
        public int FacilityId { get; set; }
    }
    public class ChangeEncounterDoctor
    {
        public int EncounterId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
    }
    public class GetEMRUserSetup
    {
        public int HospitalLocationID { get; set; }
        public int EmployeeId { get; set; }
        public int EncodedBy { get; set; }
    }
    public class getDoctorList
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int HospitalLocationId { get; set; }
        public int SpecialisationId { get; set; }
        public int FacilityId { get; set; }
        public int EncodedBy { get; set; }
        public int IsMedicalProvider { get; set; }
    }
    public class getEmployeeId
    {
        public int UserId { get; set; }
    }
    public class HospitalsetupFlag
    {
        public string Value { get; set; }
        public string Flag { get; set; }
    }
    public class getHospitalSetupValueMultiple
    {
        public string Flag { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetStatusMaster
    {
        public string StatusType { get; set; }
    }
    public class getFacilityList
    {
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
        public int GroupID { get; set; }
        public int EncodedBy { get; set; }
        public string FacilityType { get; set; }
    }
    public class DeleteAppointment
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string CancelRemarks { get; set; }
    }
    public class GetDocumentList
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationNo { get; set; }
    }
    public class GetDoctorDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int DoctorId { get; set; }
    }
    public class AuditCommonAccess
    {
        public Int16 HospitalLocationId { get; set; }
        public Int32 FacilityId { get; set; }
        public Int32 RegistrationId { get; set; }
        public Int32 EncounterId { get; set; }
        public Int32 PageId { get; set; }
        public Int32 TemplateId { get; set; }
        public Int32 EncodedBy { get; set; }
        public Int32 EmployeeId { get; set; }
        public string AuditStatus { get; set; }
        public string IPAddress { get; set; }
    }
    public class GetPatientDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int RegistrationNo { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
    }
    public class SearchPatientByName
    {
        public int HospitalLocationId { get; set; }
        public string PatientName { get; set; }
        public string Status { get; set; }
    }

    public class getFontAttribute
    {
        public string AttributeType { get; set; }
        public string AttributeValue { get; set; }
    }
    public class HandleException
    {
        public int HospitalLocationId { get; set; }
        public string Source { get; set; }
        public string ErrorMessage { get; set; }
        public string QueryString { get; set; }
        public string TargetSite { get; set; }
        public string StackTrace { get; set; }
        public string ServerName { get; set; }
        public string RequestURL { get; set; }
        public string UserAgent { get; set; }
        public string UserIP { get; set; }
        public int UserId { get; set; }
    }
    public class GetInsuranceQuery
    {
        public int UserId { get; set; }
    }
    public class GetHospitalSubDepartment
    {
        public int HospitalLocationId { get; set; }
        public int DepartmentId { get; set; }
        public int SubDepartmentId { get; set; }
        public string DepartmentType { get; set; }
    }
    public class GetPatientEncounterDetails
    {
        public int RegistrationId { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GfsQueryInputs
    {
        public int Qid { get; set; }
        public string formID { get; set; }
        public string Query { get; set; }
        public int QueryBy { get; set; }
        public int QueryTo { get; set; }
    }


    public class LabResultHistory
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
        public int ReviewedStatus { get; set; }
        public string PatientName { get; set; }
        public int iUserId { get; set; }
        public bool IsMinLabel { get; set; }
        public bool IsMaxLabel { get; set; }
        public bool IsER { get; set; }

    }

    public class JLabResultHistoryOut
    {
        public string Source { get; set; }
        public string ServiceName { get; set; }
        public string RegistrationNo { get; set; }
        public string PatientName { get; set; }

        public string OrderDate { get; set; }

    }
    public class GetGender
    {
        public int RegistrationId { get; set; }
    }
    public class GetFontDetails
    {
        public string MasterType { get; set; }
    }

    public class getPatientEncounterId
    {
        public string EncounterNo { get; set; }
        public int FacilityId { get; set; }
    }
    
    public class GetUserModule
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
    public class GetModuleUserPages
    {
        public int ModuleId { get; set; }
        public int EncounterId { get; set; }
        public int GroupId { get; set; }
        public int FormId { get; set; }
    }
    public class GetRegistrationDetails
    {
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public string PatientName { get; set; }
        public int PageSize { get; set; }
        public int PageNo { get; set; }
    }
    public class getRegistrationIDFromRegistrationNo
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationNo { get; set; }
    }
    public class GetPatientHistoryDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int DoctorId { get; set; }
        public string DateRange { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }
    public class GetEncounterDoctor
    {
        public int EncounterId { get; set; }
    }

    public class CommonBaseClass
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
    public class PrescriptionInputReport
    {
        public string UserName { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public int IndentId { get; set; }
        public string QueryString { get; set; }




    }
}