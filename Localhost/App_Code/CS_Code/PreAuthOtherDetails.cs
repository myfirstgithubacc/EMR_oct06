using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PreAuthOtherDetails
/// </summary>
//public class PreAuthOtherDetails
//{
//    public PreAuthOtherDetails()
//    {
//        //
//        // TODO: Add constructor logic here
//        //
//    }
//    public class PreAuthJson
//    {
//        public string userId { get; set; }
//        public string tokenId { get; set; }
//        public JSONDoc JSONDoc { get; set; }

//    }


//    public class JSONDoc
//    {
//        public string ReferenceNo { get; set; }
//        public List<INSURANCE_COMPANY> INSURANCE_COMPANY { get; set; }
//        public List<PATIENT_INFORMATION> PATIENT_INFORMATION { get; set; }
//        public List<TREATMENT_INFORMATION> TREATMENT_INFORMATION { get; set; }
//        public List<INVESTIGATION> INVESTIGATION { get; set; }
//        public List<Medical_Management> Medical_Management { get; set; }
//        public List<Route_of_Drug_Admin> Route_of_Drug_Admin { get; set; }
//        public List<SURGERY> SURGERY { get; set; }
//        public List<Anaesthesia> Anaesthesia { get; set; }
//        public List<STENT> STENT { get; set; }
//        public List<Proposed_line_of_treatment> Proposed_line_of_treatment { get; set; }
//        public List<MATERNITY> MATERNITY { get; set; }

//        public List<Past_history_of_any_chronic_illness> Past_history_of_any_chronic_illness { get; set; }
//        public List<PAYMENT_INFORMATION> PAYMENT_INFORMATION { get; set; }
//        public List<HOSPITAL_DETAIL> HOSPITAL_DETAIL { get; set; }
//        public List<ATTACHMENT_DETAILS> ATTACHMENT_DETAILS { get; set; }



//    }

//    public class INSURANCE_COMPANY
//    {

//        public string Insurance_Company { get; set; }
//        public string TPA_Name { get; set; }
//        public string Policy { get; set; }
//        public string TPA_Card { get; set; }
//        public string Valid_From { get; set; }
//        public string Valid_Upto { get; set; }

//    }
//    public class PATIENT_INFORMATION
//    {
//        public string First_Name { get; set; }
//        public string Last_Name { get; set; }
//        public string Gender { get; set; }
//        public string Date_of_Birth { get; set; }
//        public string Cell_Phone { get; set; }
//        public string Employee_ID { get; set; }
//        public string Martial_Status { get; set; }
//        public string Nationality { get; set; }
//        public string Relationship { get; set; }
//        public string Mediclaim_If_any { get; set; }
//        public string Family_Physician_Any { get; set; }
//        public string Company_Name { get; set; }
//        public string Give_Details { get; set; }
//        public string Physician_Name { get; set; }
//        public string Physician_Contact_No { get; set; }

//    }
//    public class TREATMENT_INFORMATION
//    {
//        public string Physician_Name { get; set; }
//        public string Physician_Contact { get; set; }
//        public string Physician_Qualification { get; set; }
//        public string Registration_with_State_Code { get; set; }
//        public string Nature_Of_Illness { get; set; }
//        public string Duration_present_ailment { get; set; }
//        public string Date_of_first_consultation { get; set; }
//        public string Clinical_Findings { get; set; }
//        public string Past_history_ailment { get; set; }
//        public string Provisional_diagnosis { get; set; }

//    }
//    public class INVESTIGATION
//    {
//        public string INVESTIGATION_NAME { get; set; }
//        public string Code { get; set; }
//    }
//    public class Medical_Management
//    {
//        public string Medication { get; set; }
//        public string Dosage { get; set; }
//    }
//    public class Route_of_Drug_Admin
//    {
//        public string Route_Of_Drug { get; set; }
//        public string Code { get; set; }
//    }
//    public class SURGERY
//    {
//        public string Name { get; set; }
//        public string Code { get; set; }
//    }
//    public class Anaesthesia
//    {
//        public string Anaesthesia_Type { get; set; }
//        public string Code { get; set; }
//    }
//    public class STENT
//    {
//        public string NAME { get; set; }
//        public string Code { get; set; }
//    }

//    public class Proposed_line_of_treatment
//    {
//        public string Non_allopathic_Treatment { get; set; }
//        public string RTA { get; set; }
//        public string Accident_Type { get; set; }
//        public string How_RTA_occur { get; set; }
//        public string Date_of_Injury { get; set; }
//        public string Police_Report { get; set; }
//        public string FIR { get; set; }
//        public string Disease_by_Alcohol { get; set; }
//        public string Test_conducted { get; set; }
//        public string Grade_OF_Surgery { get; set; }
//        public string Intensive_Care { get; set; }
//    }
//    public class MATERNITY
//    {
//        public string Incase_of_Maternity { get; set; }
//        public string G { get; set; }
//        public string P { get; set; }
//        public string L { get; set; }
//        public string A { get; set; }
//        public string Date_of_Delivery { get; set; }

//    }
//    public class Past_history_of_any_chronic_illness
//    {
//        public string Diabetes { get; set; }
//        public string Diabetes_Duration { get; set; }
//        public string Diabetes_Remarks { get; set; }
//        public string Hypertension { get; set; }
//        public string Hypertension_Duration { get; set; }
//        public string Hypertension_Remarks { get; set; }
//        public string Asthma_COPD_Bronchitis { get; set; }
//        public string Duration_Asthma_COPD_Bronchitis { get; set; }
//        public string Remarks_Asthma_COPD_Bronchitis { get; set; }
//        public string Cancer { get; set; }
//        public string Duration_Cancer { get; set; }
//        public string Remarks_Cancer { get; set; }
//        public string Alcohol_or_drug_abuse { get; set; }
//        public string Duration_Alcohol_drug_abuse { get; set; }
//        public string Remarks_Alcohol_drug_abuse { get; set; }
//        public string HIV_STD_Ailment { get; set; }
//        public string Duration_HIV_STD_Ailment { get; set; }
//        public string Remark_HIV_STD_Ailment { get; set; }
//        public string Any_other_Ailment { get; set; }
//    }
//    public class PAYMENT_INFORMATION
//    {
//        public string DoA { get; set; }
//        public string DoD { get; set; }
//        public string Expected_days { get; set; }
//        public string Admission_Type { get; set; }
//        public string Claim_Category { get; set; }
//        public string Expense_Type { get; set; }
//        public string Hospitalization_Type { get; set; }
//        public string ICU_Type { get; set; }
//        public string Accommodation_Class { get; set; }
//        public string Room_Rent { get; set; }
//        public string Bed_Number { get; set; }
//        public string ICU_Charges { get; set; }
//        public string OT_Charges { get; set; }
//        public string Nursing_Charges { get; set; }
//        public string Patient_Diet { get; set; }
//        public string Professional_surgeon { get; set; }
//        public string Anaesthetist { get; set; }
//        public string Consultation { get; set; }
//        public string Medicines { get; set; }
//        public string Consumables { get; set; }
//        public string Implant_Cost { get; set; }
//        public string Other_expenses { get; set; }
//        public string Any_Applicable_Packages { get; set; }
//        public string Package_Name { get; set; }
//        public string Sum_Total_expected { get; set; }
//        public string Sum_Total_expected_InWords { get; set; }

//    }
//    public class HOSPITAL_DETAIL
//    {
//        public string Hospital_Name { get; set; }
//        public string Hospital_Address { get; set; }
//        public string Hospital_Status { get; set; }
//        public string Paneled_Hospital { get; set; }
//        public string Doctor_Registration_No { get; set; }
//        public string GIPSA_Flag { get; set; }
//        public string IRDA_hospital_code { get; set; }
//        public string ROHINI_GLN_code { get; set; }
//    }
//    public class ATTACHMENT_DETAILS
//    {
//        public string Attachment_Type { get; set; }
//        public string Attachment_Remarks { get; set; }

//    }
//}



public class Rootobject
{
    public Preauth Preauth { get; set; }
}
public class Preauth
{
    public Preauth_Entities Preauth_Entities { get; set; }
}

//public class Preauth
//{
//    public Preauth_Entities Preauth_Entities { get; set; }
//}
public class DiscrepancyRoot
{
    // public DiscrepancyReplyInformation DiscrepancyReplyInformation { get; set; }
    public string PreauthNo { get; set; }
    public string ClaimNo { get; set; }
    public string HospitalId { get; set; }
    public string PolicyNo { get; set; }
    public List<DiscrepancyInformation> DISCREPANCY_DETAILS { get; set; }
    public List<ATTACHMENT_DETAILS> ATTACHMENT_DETAILS { get; set; }
}
public class DiscrepancyReplyInformation
{
    public string ReferenceNo { get; set; }
    public string TPACardNo { get; set; }
    public string ClaimNumber { get; set; }
    public string DiscrepancyId { get; set; }

    public string DiscrepancyRemarks { get; set; }
   

}
public class DiscrepancyInformation
{  
    public string Discrepancy_Id { get; set; }
    public string Discrepancy_Remark { get; set; }


}
public class Preauth_Entities
{
    public string Policy_Type {get;set;}
    public string Registration_No { get; set; }
    public string Preauth_No { get; set;}
    public string ClaimNumber { get; set; }

  //  public string ReferenceNo { get; set; }
    public INSURANCE_COMPANY INSURANCE_COMPANY { get; set; }
    public PATIENT_INFORMATION PATIENT_INFORMATION { get; set; }
    public TREATMENT_INFORMATION TREATMENT_INFORMATION { get; set; }
    public List<DIAGNOSIS> DIAGNOSIS { get; set; }
    public List<INVESTIGATION> INVESTIGATION { get; set; }
    public List<Medical_Management> Medical_Management { get; set; }
    public List<Route_Of_Drug_Admin> Route_of_Drug_Admin { get; set; }
    public List<SURGERY> SURGERY { get; set; }
    public List<Anaesthesia> Anaesthesia { get; set; }
    public List<STENT> STENT { get; set; }
    public Proposed_Line_Of_Treatment Proposed_line_of_treatment { get; set; }
    public MATERNITY MATERNITY { get; set; }
    public Past_History_Of_Any_Chronic_Illness Past_history_of_any_chronic_illness { get; set; }
    public PAYMENT_INFORMATION PAYMENT_INFORMATION { get; set; }
    public HOSPITAL_DETAIL HOSPITAL_DETAIL { get; set; }
    public List<ATTACHMENT_DETAILS> ATTACHMENT_DETAILS { get; set; }
}

public class INSURANCE_COMPANY
{
    public string Insurance_Company { get; set; }
    public string Insurance_Company_Code { get; set; }
    public string TPA_Name { get; set; }
    public string TPA_Code { get; set; }
    public string Policy { get; set; }
    public string TPA_Card { get; set; }
    public string Valid_From { get; set; }
    public string Valid_Upto { get; set; }
    public string Payer_Type { get; set; }
    public string Payer_Name { get; set; }
    

}

public class PATIENT_INFORMATION
{
    public string First_Name { get; set; }
    public string Last_Name { get; set; }
    public string Gender { get; set; }
    public string Date_of_Birth { get; set; }
    public string Age { get; set; }
    public string Cell_Phone { get; set; }
    public string Employee_ID { get; set; }
    public string Martial_Status { get; set; }
    public string Nationality { get; set; }
    public string Relationship { get; set; }
    public string Mediclaim_If_any { get; set; }
    public string Family_Physician_Any { get; set; }
    public string Company_Name { get; set; }
    public string Give_Details { get; set; }
    public string Physician_Name { get; set; }
    public string Physician_Contact_No { get; set; }
    public string PAN_No { get; set; }
    public string Corporate_Name { get; set; }
    public string Proposer_Name { get; set; }
    public string Address_1 { get; set; }
    public string Address_2 { get; set; }
    public string  City { get; set; }
    public string Pincode { get; set; }
    public string State { get; set; }
    public string MRN_No { get; set; }

}

public class TREATMENT_INFORMATION
{
    public string Physician_Name { get; set; }
    public string Physician_Code { get; set; }
    public string Physician_Contact { get; set; }
    public string Physician_Qualification { get; set; }
    public string Registration_with_State_Code { get; set; }
    public string Nature_Of_Illness { get; set; }
    public string Duration_present_ailment { get; set; }
    public string Date_of_first_consultation { get; set; }
    public string Clinical_Findings { get; set; }
    public string Past_history_ailment { get; set; }
    //public string Provisional_diagnosis { get; set; }
    //public string Provisional_Diagnosis_ICD_10 { get; set; }

}

public class Proposed_Line_Of_Treatment
{
    public string Non_allopathic_Treatment { get; set; }
    public string RTA { get; set; }
    public string Accident_Type { get; set; }
    public string How_RTA_occur { get; set; }
    public string Date_of_Injury { get; set; }
    public string Police_Report { get; set; }
    public string FIR { get; set; }
    public string Disease_by_Alcohol { get; set; }
    public string Test_conducted { get; set; }
    public string Grade_OF_Surgery { get; set; }
    public string Intensive_Care { get; set; }
    public string How_Did_Injury_Occur { get; set; }

}

public class MATERNITY
{
    public string Incase_of_Maternity { get; set; }
    public string G { get; set; }
    public string P { get; set; }
    public string L { get; set; }
    public string A { get; set; }
    public string Date_of_Delivery { get; set; }
}

public class Past_History_Of_Any_Chronic_Illness
{
    public string Diabetes { get; set; }
    public string Diabetes_Duration { get; set; }
    public string Diabetes_Remarks { get; set; }
    public string Hypertension { get; set; }
    public string Hypertension_Duration { get; set; }
    public string Hypertension_Remarks { get; set; }
    public string Asthma_COPD_Bronchitis { get; set; }
    public string Duration_Asthma_COPD_Bronchitis { get; set; }
    public string Remarks_Asthma_COPD_Bronchitis { get; set; }
    public string Cancer { get; set; }
    public string Duration_Cancer { get; set; }
    public string Remarks_Cancer { get; set; }
    public string Alcohol_or_drug_abuse { get; set; }
    public string Duration_Alcohol_drug_abuse { get; set; }
    public string Remarks_Alcohol_drug_abuse { get; set; }
    public string HIV_STD_Ailment { get; set; }
    public string Duration_HIV_STD_Ailment { get; set; }
    public string Remark_HIV_STD_Ailment { get; set; }
    public string Any_other_Ailment { get; set; }
}
public class Enhancement
{
    
    public Enhancement_Entities Enhancement_Entities { get; set; }
    public List<ATTACHMENT_DETAILS> ATTACHMENT_DETAILS { get; set; }
}
public class RootEnhancement
{
    public Enhancement Enhancement { get; set; }
}

public class Enhancement_Entities
{
    public int ReferenceNo { get; set; }
    public string ClaimNumber { get; set; }
    public string Differential_Diagnosis { get; set; }
    public string Surgery_name_If { get; set; }
    public string DoA { get; set; }
    public string DoD { get; set; }
    public Double Room_Rent { get; set; }
    public string Bed_Number { get; set; }
    public Double ICU_Charges { get; set; }
    public Double OT_Charges { get; set; }
    public Double Nursing_Charges { get; set; }
    public Double Patient_Diet { get; set; }
    public Double Expected_Cost_Investigation_diagnostic { get; set; }
    public Double Professional_fees_Surgeon { get; set; }
    public Double Anaesthetist_fees { get; set; }
    public Double Consultation_charges { get; set; }
    public Double Medicines { get; set; }
    public Double Consumable { get; set; }
    public Double Cost_of_implants { get; set; }
    public Double Other_expens_any { get; set; }
    public string All_Inclusive_packages_any { get; set; }
    public string Package_Name { get; set; }

    public Double Sum_Total_expected_cost { get; set; }
    public string Sum_Total_expected_cost_words { get; set; }
   
}
public class PAYMENT_INFORMATION
{
    public string DoA { get; set; }
    public string DoD { get; set; }
    public string Expected_days { get; set; }
    public string Admission_Type { get; set; }
    public string Room_Type { get; set; }
    public string Accommodation_Class { get; set; }
    public string Claim_Category { get; set; }
    public string Expense_Type { get; set; }
    public string Hospitalization_Type { get; set; }
    public string ICU_Type { get; set; }   
    public string Room_Rent { get; set; }
    public string Bed_Number { get; set; }
    public string ICU_Charges { get; set; }
    public string OT_Charges { get; set; }
    public string Nursing_Charges { get; set; }
    public string Patient_Diet { get; set; }
    public string Expected_Cost_Investigation_diagnos { get; set; }
    public string Professional_surgeon { get; set; }
    public string Anaesthetist { get; set; }
    public string Consultation { get; set; }
    public string Medicines { get; set; }
    public string Consumables { get; set; }
    public string Implant_Cost { get; set; }
    public string Other_expenses { get; set; }
    public string Any_Applicable_Packages { get; set; }
    public string Package_Name { get; set; }
    public string Sum_Total_expected { get; set; }
    public string Sum_Total_expected_InWords { get; set; }

 


}

public class HOSPITAL_DETAIL
{
    public string Hospital_Name { get; set; }
    public string Hospital_Address { get; set; }
    public string Hospital_Status { get; set; }
    public string Paneled_Hospital { get; set; }
    public string Doctor_Registration_No { get; set; }
    public string GIPSA_Flag { get; set; }
    public string IRDA_hospital_code { get; set; }
    public string ROHINI_GLN_code { get; set; }
}

public class INVESTIGATION
{
    public string INVESTIGATION_NAME { get; set; }
    public string Code { get; set; }
}

public class DIAGNOSIS
{
    public string Provisional_diagnosis { get; set; }
    public string Provisional_Diagnosis_ICD_10 { get; set; }
    public string Provisional_Diagnosis_ICDCode { get; set; }

}

public class Medical_Management
{
    public string Medication { get; set; }
    public string Dosage { get; set; }
}

public class Route_Of_Drug_Admin
{
    public string Route_Of_Drug { get; set; }
    public string Code { get; set; }
}

public class SURGERY
{
    public string Name { get; set; }
    public string CODE { get; set; }
}

public class Anaesthesia
{
    public string Anaesthesia_Type { get; set; }
    public string Code { get; set; }
}

public class STENT
{
    public string NAME { get; set; }
    public string Code { get; set; }
}

public class ATTACHMENT_DETAILS
{
    public string Attachment_FileName { get; set; }
    public string Attachment_Type { get; set; }
    public string Attachment_Remarks { get; set; }
}
public class ATTACHMENT_DETAILSInvocie
{
    public string Attachment_FileName { get; set; }
    public string Attachment_Type { get; set; }
    public string Attachment_Remarks { get; set; }
}
public class RoomRentCharges
{
    public string Total_Room_Rent { get; set; }
    public string ChargesCode { get; set; }
    public List<RoomRentServices> RoomRentServices { get; set; }
}
public class RoomRentServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class ICUCharges
{
    public string Total_ICU_Charges { get; set; }
    public string ChargesCode { get; set; }
    public List<ICUServices> ICUServices { get; set; }
}
public class ICUServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class PhysicianCharges
{
    public string Total_PhysicianCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<PhysicianServices> PhysicianServices { get; set; }
}
public class PhysicianServices
{

    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class NursingCharges
{
    public string Total_NursingCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<NursingServices> NursingServices { get; set; }
}
public class NursingServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
     public string Charges { get; set; }

}
public class InvestigationCharges
{
    public string Total_InvestigationCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<InvestigationServices> InvestigationServices { get; set; }
}
public class InvestigationServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class PharmacyCharges
{
    public string Total_PharmacyCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<PharmacyServices> PharmacyServices { get; set; }
}
public class PharmacyServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class SurgeryCharges
{
    public string Total_SurgeryCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<SurgeryServices> SurgeryServices { get; set; }
}
public class SurgeryServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class AnesthesiaCharges
{
    public string Total_AnesthesiaCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<AnesthesiaServices> AnesthesiaServices { get; set; }
}
public class AnesthesiaServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class MiscellaneousCharges
{
    public string Total_MiscellaneousCharges { get; set; }
    public string ChargesCode { get; set; }
    public List<MiscellaneousServices> MiscellaneousServices { get; set; }
}
public class MiscellaneousServices
{
    public string BillLevel1 { get; set; }
    public string BillLevel2 { get; set; }
    public string BillLevel3 { get; set; }
    public string BillNo { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }
    public string Charges { get; set; }

}
public class RootFinalBill
{
    public FinalBill FinalBill { get; set; }
}
public class FinalBill
{
    public FinalBill_Entities FinalBill_Entities { get; set; }
}
public class FinalBill_Entities
{
    public string PreauthNo { get; set; }

    public string RegistrationNo { get; set; }
    public string ClaimNumber { get; set; }
    public string Claim_ID { get; set; }

    public string Approx_ClaimAmount { get; set; }
    public string Policy_No { get; set; }
    public string Date_of_Admission { get; set; }
    public string Date_of_Discharge { get; set; }
    public string Admission_Time { get; set; }
    public string Discharge_Time { get; set; }
    public string Days { get; set; }

    //public RoomRentCharges RoomRentCharges { get; set; }
   // public ICUCharges ICUCharges { get; set; }
   // public PhysicianCharges PhysicianCharges { get; set; }
   // public NursingCharges NursingCharges { get; set; }
   // public InvestigationCharges InvestigationCharges { get; set; }
   // public PharmacyCharges PharmacyCharges { get; set; }
   /// public SurgeryCharges SurgeryCharges { get; set; }
    //public AnesthesiaCharges AnesthesiaCharges { get; set; }
   // public MiscellaneousCharges MiscellaneousCharges { get; set; }
   public List<ServiceDetails> Service_Details { get; set; }
    public string Paid_By_Patient { get; set; }
    public string Paid_By_Payer { get; set; }
    public string Total_Claimed_Amount { get; set; }
    public string Total_Claimed_Amount_In_Words { get; set; }

    public List<ATTACHMENT_DETAILSInvocie> ATTACHMENT_DETAILS { get; set; }

    

}

public class ServiceDetails
{

    public string BillLevel1_Text { get; set; }
    public string BillLevel1_Code { get; set; }
    public string BillLevel2_Text { get; set; }
    public string BillLevel2_Code { get; set; }

    public string BillLevel3_Text { get; set; }
    public string BillLevel3_Code { get; set; }
    //public string Bill_Code { get; set; }
    public List<BIllDetails> BIll_Details { get; set; }

}
public class BIllDetails
{

    public string BillNo { get; set; }
    public string ServiceRemarks { get; set; }
    public string BillDate { get; set; }
    public string Units { get; set; }

    public string Rate { get; set; }
    public string Charges { get; set; }
    public string DiscAmt { get; set; }
    public string NetAmt { get; set; }

}
public class DailyBasis_Entities
{
    public string Department { get; set; }
    public string SubDepartment { get; set; }
    public string OrderDate { get; set; }
    public string ServiceName { get; set; }
    public string Unit { get; set; }
    public string Provider { get; set; }
    public string ServiceAmount { get; set; }
    public string DoctorAmount { get; set; }
    public string Discount_Percent { get; set; }
    public string DiscountAmount { get; set; }
    public string NetCharges { get; set; }
    public string PatientPayable { get; set; }
    public string PayerPayble { get; set; }
    public string NetAmount { get; set; }
        
}
public class DailyBasisBill
{
    public string ReferenceNo { get; set; }
    public string ClaimNo { get; set; }
    public List<DailyBasis_Entities> DailyBasis_Entities { get; set; }
}
public class RootobjectDailyBill
{
    public DailyBasisBill DailyBasisBill { get; set; }
}
