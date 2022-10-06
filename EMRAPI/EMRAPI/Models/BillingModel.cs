using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class GetPackageServiceLimit
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int PackageId { get; set; }
        public int CompanyId { get; set; }
    }
    public class SearchPatient
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public string PatienType { get; set; }
        public string RegistrationNo { get; set; }
        public string EncounterNo { get; set; }
    }
    public class GetDepartmentByType
    {
        public string DepartmentType { get; set; }
    }
    public class GetPatientRegistrationNo
    {
        public int RegistrationId { get; set; }
    }
    public class GetPatientDocuments
    {
        public int ImageCategoryId { get; set; }
        public int RegistrationId { get; set; }
        public string FolderName { get; set; }
    }
    public class RetrievePatientDocuments
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int ImageId { get; set; }
    }
    public class GetPatientRegistrationId
    {
        public int RegistrationNo { get; set; }
    }
    public class DeletePatientDocuments
    {
        public int ImageCategoryId { get; set; }
    }
    public class SavePatientDocuments
    {
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public string ThumbnailName { get; set; }
        public string xmlDocumentDetails { get; set; }
        public string ExtentionType { get; set; }
        public string DocumentType { get; set; }
        public int DocumentTypeId { get; set; }
        public int UserId { get; set; }
    }
    public class GetOrderDetailsfromEncounterID
    {
        public int EncounterId { get; set; }
        public int FacilityId { get; set; }
    }
    public class getServiceOrderDeptTypeWise
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int EncounterId { get; set; }
        public string PageType { get; set; }
        public string ServiceType { get; set; }
        public int InvoiceId { get; set; }
        public Int16 isDetails { get; set; }
        public string InvoiceNo { get; set; }
    }
    public class GetPatientDetailsIP
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int RegistrationId { get; set; }
        public int RegistrationNo { get; set; }
        public int UserId { get; set; }
        public int EncounterId { get; set; }
        public string EncounterNo { get; set; }
    }
    public class DeActivateOrderServices
    {
        public int ServiceOrderDetailAmountId { get; set; }
        public int ServiceId { get; set; }
        public int OrderId { get; set; }
        public int ServiceOrderDetailId { get; set; }
        public string Remarks { get; set; }
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int UserId { get; set; }
    }
    public class InsertIPServiceDiscount
    {
        public string ServiceType { get; set; }
        public int EncounterId { get; set; }
        public int UserId { get; set; }
        public string xmlService { get; set; }
    }
    public class getOPIPRegEncDetails
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public string OPIP { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int RegEnc { get; set; }
        public string RegistrationNo { get; set; }
        public string EncounterNo { get; set; }
        public string PatientName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string RecordButton { get; set; }
        public int RowNo { get; set; }
        public int PageSize { get; set; }
        public int UserId { get; set; }
        public int PageNo { get; set; }
        public string BedNo { get; set; }
        public int AdmissionDischargedStausId { get; set; }
        public string Company { get; set; }
        public string Nationality { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Identityno { get; set; }
        public string PassportNo { get; set; }
        public string DateOfBirth { get; set; }
        public string EmailAddress { get; set; }
        public string OldRegistrationNo { get; set; }
        public int EntrySiteId { get; set; }
    }
}