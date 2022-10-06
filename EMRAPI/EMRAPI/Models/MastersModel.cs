using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{
    public class Masters
    {
    }
    public class GetFormulationMaster
    {
        public int FormulationId { get; set; }
        public int HospitalLocationId { get; set; }
        public int Active { get; set; }
        public int UserId { get; set; }
    }
    public class GetFoodMaster
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int Active { get; set; }
    }
    public class GetIndentType
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetUnitMaster
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetVolumeUnitMaster
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int IsVolume { get; set; }
    }
    public class GetMedicationSet
    {
        public int HospitalLocationId { get; set; }
        public int MedicationSetId { get; set; }
    }
    public class GetItemConversionFactor
    {
        public int ItemId { get; set; }
    }
    public class GetGenericDetails
    {
        public int GenericId { get; set; }
        public string GenericName { get; set; }
        public int Active { get; set; }
        public int HospitalLocationId { get; set; }
        public int UserId { get; set; }
        public int CIMSCategoryId { get; set; }
        public int CIMSSubCategoryId { get; set; }
    }
    public class GetNetworkWiseItemsWithStock
    {
        public int HospitalLocationId { get; set; }
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public int GenericId { get; set; }
        public int UserId { get; set; }
        public int FacilityId { get; set; }
        public int SupplierId { get; set; }
        public string ItemName { get; set; }
        public int WithStockOnly { get; set; }
        public string ItemNo { get; set; }
        public int ItemSubCategoryId { get; set; }
        public string Usedtype { get; set; }
        public int AllBrand { get; set; }
        public int RegistratioNo { get; set; }
    }
    public class ISCalculationRequired
    {
        public int ItemId { get; set; }
    }
    public class GetMedicationSetDetails
    {
        public int HospitalLocationId { get; set; }
        public int MedicationSetId { get; set; }
        public int ItemId { get; set; }
    }
    public class getItemAttributes
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
        public int ItemId { get; set; }
        public int StoreId { get; set; }
        public int USerId { get; set; }
    }
    public class GetEMRDocumentCategory
    {
        public int HospitalLocationId { get; set; }
    }
    public class GetFileServersetup
    {
        public int FacilityId { get; set; }
    }
    public class getDepartmentMain
    {
        public int HospitalLocationId { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetServiceName
    {
        public int ServiceId { get; set; }
    }
    public class GetServiceTagingWithRange
    {
        public int ServiceId { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AgeType { get; set; }
        public int FacilityId { get; set; }
    }
    public class GetDocumentType
    {
        public int HospitalLocationId { get; set; }
        public string DocumentTypeName { get; set; }
    }
    public class SaveDocumentType
    {
        public int HospitalLocationId { get; set; }
        public string DocumentTypeName { get; set; }
        public int UserId { get; set; }
    }
    public class GetDocumentCategory
    {
        public int HospitalLocationId { get; set; }
        public string DocumentCategoryName { get; set; }
    }
    public class SaveDocumentCategory
    {
        public int HospitalLocationId { get; set; }
        public string DocumentCategoryName { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public int DocumentCategoryId { get; set; }
    }
    public class GetAttachmentCategory
    {
        public int HospitalLocationId { get; set; }
    }

    public class getFrequencyMaster
    {
        public string LanguageCode { get; set; }
    }
}