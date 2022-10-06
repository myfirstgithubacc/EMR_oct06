using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMRAPI.Models
{


    public class VitalModel
    {
        public int HospitalId { get; set; }
        
        public int VitalId { get; set; }
        public int id { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public int iTemplateId { get; set; }

        public int Registrationid { get; set; }

        public int VitalDetailsId { get; set; }
        public int FacilityId { get; set; }

    }
    public class EmrTemplate
    {
        public int TemplateId { get; set; }
        public int SequenceNo { get; set; }
        public int VitalId { get; set; }
        public string    Vital { get; set; }
        public string DisplayName { get; set; }
        public string TemplateDescription { get; set; }
        public string MeasurementSystem { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string VitalType { get; set; }
        public int MaxLength { get; set; }
        public bool Active { get; set; }
        public int UnitId1 { get; set; }
        public string Unit1 { get; set; }
        public int UnitId2 { get; set; }
        public string Unit2 { get; set; }
        public string LastValues { get; set; }
        public string LastValue { get; set; }
        public string HeaderName { get; set; }
        public string AbNormal { get; set; }
        public string Category { get; set; }
        public int IsInvestigation { get; set; }
        public int TemplateFieldId { get; set; }
        public int TemplateData { get; set; }
        public bool IsMandatory { get; set; }
        public int ValueID { get; set; }
        public int DefaultValue { get; set; }
        public bool DisplayInGraph { get; set; }
    }

    public class PatientPreviousVitalsHistory
    {
       public int HospId { get; set; }
       public int FacId { get; set; }
       public int RegId { get; set; }
       public int EncId { get; set; }
       public int ViewType { get; set; }
       public string DRange { get; set; }
       public string FDate { get; set; }
       public string TDate { get; set; }
       public  int VitalSignId { get; set; }
       public bool Abnormal { get; set; }


    }
    public class SaveEMRMUDLogVitalValues
    {
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int EncounterId { get; set; }
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public bool VitalValues { get; set; }
    }

}