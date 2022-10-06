using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.IO;

/// <summary>
/// Summary description for HCXPreApproval
/// </summary>
public class HCXPreApproval
{

    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    Hashtable HshIn;
    Hashtable HshOut;
    DataSet ds;
    DataSet ds1;
    DataSet ds2;
    SqlDataReader dr;
    DAL.DAL DlObj;
    DL_Funs ff = new DL_Funs();
    BaseC.Patient bc;
    public HCXPreApproval()
    {
        
    }
    public Hashtable SaveEnhancement(ApprovalEnhancement objenh)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@intId", objenh.EnhancementId);
        HshIn.Add("@PreAuthId", objenh.PreAuthID);
        HshIn.Add("@ClaimNo", objenh.ClaimNo);
        HshIn.Add("@DifferentialDiagnosis", objenh.DifferentialDiagnosis);
        HshIn.Add("@SurgeryName", objenh.SurgeryName);
        HshIn.Add("@AdmissionDate", objenh.AdmissionDate);
        HshIn.Add("@DischargeDate", objenh.DischargeDate);
        HshIn.Add("@RoomRent", objenh.RoomRent);
        HshIn.Add("@BedNumber", objenh.BedNumber);
        HshIn.Add("@ICUCharges", objenh.ICUCharges);
        HshIn.Add("@OTCharges", objenh.OTCharges);
        HshIn.Add("@NursingCharges", objenh.NursingCharges);
        HshIn.Add("@PatientDiet", objenh.PatientDiet);
        HshIn.Add("@ExpectedCostInvestigation", objenh.ExpectedCostInvestigation);
        HshIn.Add("@ProfessionalfeesSurgeon", objenh.ProfessionalfeesSurgeon);
        HshIn.Add("@AnaesthetistFees", objenh.AnaesthetistFees);
        HshIn.Add("@ConsultationCharges", objenh.ConsultationCharges);
        HshIn.Add("@Medicines", objenh.Medicines);
        HshIn.Add("@Consumable", objenh.Consumable);
        HshIn.Add("@CostOfImplants", objenh.CostOfImplants);
        HshIn.Add("@OtherExpensAny", objenh.OtherExpensAny);

        HshIn.Add("@AllInclusivePackagesAny", objenh.AllInclusivePackagesAny);
        HshIn.Add("@PackageName", objenh.PackageName);
        HshIn.Add("@SumTotalexpectedcostofhospitalization", objenh.SumTotalexpectedcostofhospitalization);
        HshIn.Add("@SumTotalexpectedcostofhospitalizationInWords", objenh.SumTotalexpectedcostofhospitalizationInWords);
        HshIn.Add("@EncodedBy", objenh.EncodedBy);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveHCXEnhancement", HshIn, HshOut);
        return HshOut;
    }
    public Hashtable SaveHCXQueryResponseData(string Query)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@preauthquery", Query);
        HshOut.Add("@Error", SqlDbType.VarChar);

        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePreAuthQueryResponse", HshIn, HshOut);

        return HshOut;
    }

    public Hashtable SaveInvoiceDocument(string Query, string PreAuth, int inyHospitalLocationId, int intEncodedBy)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@preauthquery", Query);
        HshIn.Add("@PreAuth", PreAuth);
        HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
        HshIn.Add("@intEncodedBy", intEncodedBy);
        HshOut.Add("@Error", SqlDbType.VarChar);

        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveInvoiceInsurance", HshIn, HshOut);

        return HshOut;
    }

    public DataSet fillchecklistbox()
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        return objDl.FillDataSet(CommandType.StoredProcedure, "FillPreAuthCheckList");
    }
    public DataSet GetEnhancementdetail(int PreAuthId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet dsge = new DataSet();
        HshIn = new Hashtable();
        HshIn.Add("@PreAuthId", PreAuthId);
        dsge = objDl.FillDataSet(CommandType.StoredProcedure, "UspFillEnhancement", HshIn);
        return dsge;
    }
    public Hashtable SaveHCXPreApprovalData(HCXPreApprovalModel objHCX)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@intId", objHCX.HCXID);
        HshIn.Add("@inyHospitalLocationId",objHCX.HospitalLocationId);
        HshIn.Add("@intFacilityId", objHCX.facilityId);
        HshIn.Add("@intRegistrationId", objHCX.RegistrationId);
        HshIn.Add("@intSponsorId", objHCX.SponsorId);
        HshIn.Add("@intPayerId", objHCX.PayerId);
        HshIn.Add("@chvPolicyNo", objHCX.PolicyNo);
        HshIn.Add("@chvTPACard", objHCX.TPACard);
        HshIn.Add("@dtValidFrom", objHCX.ValidFrom);
        HshIn.Add("@dtValidUpto", objHCX.ValidUpto);
        HshIn.Add("@chvFirstName", objHCX.FirstName);
        HshIn.Add("@chvLastName", objHCX.LastName);
        HshIn.Add("@chvGender", objHCX.Gender);
        HshIn.Add("@dtDateOfBirth", objHCX.DateOfBirth);
        HshIn.Add("@intAge", objHCX.Age);
        HshIn.Add("@chvEmpId", objHCX.EmpId);
        HshIn.Add("@chvMartialStatus", objHCX.MartialStatus);
        HshIn.Add("@chvNationality", objHCX.Nationality);
        HshIn.Add("@chvRelationship", objHCX.Relationship);
        HshIn.Add("@chvIsMediclaim", objHCX.IsMediclaim);
        HshIn.Add("@chvIsFamilyPhysician", objHCX.IsFamilyPhysician);
        HshIn.Add("@chvCompanyName", objHCX.CompanyName);
        HshIn.Add("@chvGiveDetails", objHCX.GiveDetails);
        HshIn.Add("@chvFamilyPhysicianName", objHCX.FamilyPhysicianName);
        HshIn.Add("@chvFamilyContactNumber", objHCX.FamilyContactNumber);
        HshIn.Add("@chvPhysicianName", objHCX.PhysicianName);
        HshIn.Add("@intTreatingDoctorId", objHCX.TreatingDoctorId);
        HshIn.Add("@chvPhysicianContact", objHCX.PhysicianContact);
        HshIn.Add("@chvPhysicianEducation", objHCX.PhysicianEducation);
        HshIn.Add("@chvPhysicianRegistration", objHCX.Registration);
        HshIn.Add("@chvDurationPresentAilment", objHCX.DurationPresentAilment);
        HshIn.Add("@dtFirstConsultationDate", objHCX.FirstConsultationDate.HasValue ? (object)objHCX.FirstConsultationDate : DBNull.Value);
        HshIn.Add("@chvRelevantClinicalFindings", objHCX.RelevantClinicalFindings);
        HshIn.Add("@chvIsPastHistoryPresentAilment", objHCX.IsPastHistoryPresentAilment);
        HshIn.Add("@chvProvisionalDiagnosis", objHCX.ProvisionalDiagnosis);
        HshIn.Add("@chvProvDiagCode", objHCX.Code);
        HshIn.Add("@chvMedicalManagement", objHCX.MedicalManagement);
        HshIn.Add("@chvMedicationAdministered", objHCX.MedicationAdministered);
        HshIn.Add("@chvDosage", objHCX.Dosage);
        HshIn.Add("@chvDrugAdministrationRoute", objHCX.DrugAdministrationRoute);
        HshIn.Add("@chvRDACode", objHCX.RDACode);
        HshIn.Add("@chvSurgeryName", objHCX.SurgeryName);
        HshIn.Add("@SgNCode", objHCX.SgNCode);
        HshIn.Add("@chvAnaesthesiaType", objHCX.AnaesthesiaType);
        HshIn.Add("@chvAnTCode", objHCX.AnTCode);
        HshIn.Add("@chvStentName", objHCX.StentName);
        HshIn.Add("@chvStNCode", objHCX.StNCode);
        HshIn.Add("@chvSurgeryLevel", objHCX.SurgeryLevel);
        HshIn.Add("@chvIntensiveCare", objHCX.IntensiveCare);
        HshIn.Add("@chvInCCode", objHCX.InCCode);
        HshIn.Add("@chvNonAllopathicTreatment", objHCX.NonAllopathicTreatment);
        HshIn.Add("@chvIsRTA", objHCX.IsRTA);
        HshIn.Add("@chvHowDidOccur", objHCX.HowDidOccur);
        HshIn.Add("@dtInjuryDate", objHCX.InjuryDate.HasValue ? (object)objHCX.InjuryDate : DBNull.Value);
        HshIn.Add("@chvIsReportedPolice", objHCX.IsReportedPolice);
        HshIn.Add("@chvFIRNo", objHCX.FIR);
        HshIn.Add("@chvIsInjueryDiseaseAlcoholConsumption", objHCX.IsInjueryDiseaseAlcoholConsumption);
        HshIn.Add("@chvIsTestConducted", objHCX.IsTestConducted);
        HshIn.Add("@chvIsMaternity", objHCX.IsMaternity);
        HshIn.Add("@chvG", objHCX.G);
        HshIn.Add("@chvL", objHCX.L);
        HshIn.Add("@chvP", objHCX.P);
        HshIn.Add("@chvA", objHCX.A);
        HshIn.Add("@dtDeliveryDate",objHCX.DeliveryDate.HasValue ? (object)objHCX.DeliveryDate : DBNull.Value );
        HshIn.Add("@chvIsDiabetes", objHCX.IsDiabetes);

        HshIn.Add("@chvDiabDuration", objHCX.DiabDuration);
        HshIn.Add("@chvDiaRemarks", objHCX.DiabRemarks);
        HshIn.Add("@chvIsHypertension", objHCX.IsHypertension);

        HshIn.Add("@chvHypDuration", objHCX.HypDuration);
        HshIn.Add("@chvHypRemarks", objHCX.HypRemarks);
        HshIn.Add("@chvIsAsthmaCopdBronchitis", objHCX.IsAsthmaCopdBronchitis);
        HshIn.Add("@chvAstDuration", objHCX.AstDuration);
        HshIn.Add("@chvAstRemarks", objHCX.AstRemarks);
        HshIn.Add("@chvCancer", objHCX.Cancer);
        HshIn.Add("@chvCanDuration", objHCX.CanDuration);
        HshIn.Add("@chvCanRemarks", objHCX.CanRemarks);
        HshIn.Add("@chvAlcoholDrugAbuse", objHCX.AlcoholDrugAbuse);
        HshIn.Add("@chvAlcDuration", objHCX.AlcDuration);
        HshIn.Add("@chvAlcRemarks", objHCX.AlcRemarks);
        HshIn.Add("@chvIsHIVSTDAilments", objHCX.IsHIVSTDAilments);
        HshIn.Add("@chvHIVDuration", objHCX.HIVDuration);
        HshIn.Add("@chvHIVRemarks", objHCX.HIVRemarks);
        HshIn.Add("@chvOtherAilment", objHCX.OtherAilment);
        HshIn.Add("@dtAdmissionDate", objHCX.AdmissionDate);
        HshIn.Add("@dtDischargeDate", objHCX.DischargeDate);
        HshIn.Add("@intExpectedStayDays", objHCX.ExpectedStayDays);
        HshIn.Add("@intAdmissionType", objHCX.AdmissionType);
        HshIn.Add("@intAccommodationClass", objHCX.AccommodationClass);
        HshIn.Add("@intRoomRent", objHCX.RoomRent);
        HshIn.Add("@intBedNumber", objHCX.BedNumber);
        HshIn.Add("@doubleICUCharges", objHCX.ICUCharges);
        HshIn.Add("@doubleOTCharges", objHCX.OTCharges);
        HshIn.Add("@doubleNursingCharges", objHCX.NursingCharges);
        HshIn.Add("@doublePatientDiet", objHCX.PatientDiet);
        HshIn.Add("@doubleExpCostInvDiag", objHCX.ExpCostInvDiag);
        HshIn.Add("@doubleSurgeonProfessionalFees", objHCX.SurgeonProfessionalFees);
        HshIn.Add("@doubleAnaesthetistFees", objHCX.AnaesthetistFees);
        HshIn.Add("@doubleConsultationCharges", objHCX.ConsultationCharges);
        HshIn.Add("@doubleMedicines", objHCX.Medicines);
        HshIn.Add("@doubleConsumables", objHCX.Consumables);
        HshIn.Add("@doublemplantCost", objHCX.ImplantCost);
        HshIn.Add("@doubleOtherHospitalExpenses", objHCX.OtherHospitalExpenses);
        HshIn.Add("@doubleInclusivePackages", objHCX.InclusivePackages);
        HshIn.Add("@chvPackageName", objHCX.PackageName);
        HshIn.Add("@intClaimCategory", objHCX.ClaimCategory);
        HshIn.Add("@intExpenseType", objHCX.ExpenseType);
        HshIn.Add("@intICUType", objHCX.ICUType);
        HshIn.Add("@intHospitalizationType", objHCX.HospitalizationType);
        HshIn.Add("@chvToalExpCostHospitalizationD", objHCX.ToalExpCostHospitalizationD);
        HshIn.Add("@chvToalExpCostHospitalizationW", objHCX.ToalExpCostHospitalizationW);
        HshIn.Add("@chvAttachmentType", objHCX.AttachmentType);
        HshIn.Add("@chvAttRemarks", objHCX.AttRemarks);
        HshIn.Add("@intEncodedBy", objHCX.EncodedBy);
        HshIn.Add("@chvMobileNo", objHCX.MobileNo);
        HshIn.Add("@chvNatureIllness", objHCX.NatureIllness);
        HshIn.Add("@xmlSeviceDetails", objHCX.xmlSeviceDetails);
        HshIn.Add("@xmlDifferentialSeviceDetails", objHCX.xmldiffDiagSeviceDetails);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        HshOut.Add("@intHCXAuthID", SqlDbType.Int);
        HshIn.Add("@intTPACode", objHCX.TPACode);
        HshIn.Add("@intHospitalCode", objHCX.HospitalCode);
        HshIn.Add("@IsEnhancement", objHCX.IsEnhancement);
        HshIn.Add("@intEnhancementRefranceId", objHCX.EnhancementRefranceId);
        HshIn.Add("@strXMLAttachement", objHCX.strXMLAttachement);
        HshIn.Add("@EnhancementRemark", objHCX.EnhancementRemark);
        HshIn.Add("@strXMLPreAuthCheckList", objHCX.xmlChecklist);
        HshIn.Add("@DetailsFromHIS", objHCX.DetailsFromHIS);
        HshIn.Add("@GroupRetail", objHCX.GroupRetail);
        HshIn.Add("@CorporateName", objHCX.CorporateName);
        HshIn.Add("@doubleSumOfInsuredAmount", objHCX.SumOfInsuredAmount);
        HshIn.Add("@intEncounterID", objHCX.intEncounterID.HasValue ? (object)objHCX.intEncounterID : DBNull.Value );
        HshIn.Add("@StrXMLMedicalManagement", objHCX.XMLMedicalManagement);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveHCXPreApprovalNew", HshIn, HshOut);

        return HshOut;
    }

    public Hashtable SaveInsuranceLogError(string charCode, string ErrorMessage, string charSuccess, int intPreAuthId, string charPageName)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@charCode", charCode);
        HshIn.Add("@ErrorMessage", ErrorMessage);
        HshIn.Add("@charSuccess", charSuccess);
        HshIn.Add("@intPreAuthId", intPreAuthId);
        HshIn.Add("@charPageName", charPageName);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveInsuranceLogError", HshIn, HshOut);
        return HshOut;

    }
    public class ApprovalEnhancement
    {
        public int EnhancementId { get; set; }
        public int PreAuthID { get; set; }
        public string ClaimNo { get; set; }
        public string DifferentialDiagnosis { get; set; }
        public string SurgeryName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime DischargeDate { get; set; }
        public double RoomRent { get; set; }
        public string BedNumber { get; set; }
        public double ICUCharges { get; set; }
        public double OTCharges { get; set; }
        public double NursingCharges { get; set; }
        public double PatientDiet { get; set; }
        public double ExpectedCostInvestigation { get; set; }
        public double ProfessionalfeesSurgeon { get; set; }
        public double AnaesthetistFees { get; set; }

        public double ConsultationCharges { get; set; }
        public double Medicines { get; set; }
        public double Consumable { get; set; }
        public double CostOfImplants { get; set; }
        public double OtherExpensAny { get; set; }
        public string AllInclusivePackagesAny { get; set; }
        public string PackageName { get; set; }
        public double SumTotalexpectedcostofhospitalization { get; set; }
        public string SumTotalexpectedcostofhospitalizationInWords { get; set; }
        public int EncodedBy { get; set; }
        
        public int LastChangedBy { get; set; }
       
        public Boolean Active { get; set; }
        public Boolean Status { get; set; }
    }
    public class HCXPreApprovalModel
    {

       
        public int HCXID { get; set; }
        public int HospitalLocationId { get; set; }
        public int RegistrationId { get; set; }
        public int SponsorId { get; set; }
        public int PayerId { get; set; }
        public string PolicyNo { get; set; }
        public string TPACard { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUpto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public string EmpId { get; set; }
        public string MartialStatus { get; set; }

        public string Nationality { get; set; }
        public string Relationship { get; set; }

        public string IsMediclaim { get; set; }
        public string IsFamilyPhysician { get; set; }
        public string CompanyName { get; set; }
        public string GiveDetails { get; set; }
        public string FamilyPhysicianName { get; set; }
        public string FamilyContactNumber { get; set; }

        public string PhysicianName { get; set; }
        public int TreatingDoctorId { get; set; }

        public string PhysicianContact { get; set; }
        public string PhysicianEducation { get; set; }

        public string Registration { get; set; }
        public string DurationPresentAilment { get; set; }
        public  DateTime? FirstConsultationDate { get; set; }

        public string RelevantClinicalFindings { get; set; }
        public string IsPastHistoryPresentAilment { get; set; }
        public string ProvisionalDiagnosis { get; set; }
        public string Code { get; set; }
        public string MedicalManagement { get; set; }
        public string MedicationAdministered { get; set; }
        public string Dosage { get; set; }
        public string DrugAdministrationRoute { get; set; }
        public string RDACode { get; set; }
        public string SurgeryName { get; set; }

        public string SgNCode { get; set; }
        public string AnaesthesiaType { get; set; }
        public string AnTCode { get; set; }
        public string StentName { get; set; }
        public string StNCode { get; set; }
        public string SurgeryLevel { get; set; }
        public string IntensiveCare { get; set; }
        public string InCCode { get; set; }
        public string NonAllopathicTreatment { get; set; }
        public string IsRTA { get; set; }
        public string HowDidOccur { get; set; }
        public DateTime? InjuryDate { get; set; }
        public string IsReportedPolice { get; set; }
        public string FIR { get; set; }
        public string IsInjueryDiseaseAlcoholConsumption { get; set; }
        public string IsTestConducted { get; set; }
        public string IsMaternity { get; set; }
        public string G { get; set; }
        public string P { get; set; }
        public string L { get; set; }
        public string A { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string IsDiabetes { get; set; }
        public string DiabDuration { get; set; }
        public string DiabRemarks { get; set; }
        public string IsHypertension { get; set; }
        public string HypDuration { get; set; }
        public string HypRemarks { get; set; }
        public string IsAsthmaCopdBronchitis { get; set; }
        public string AstDuration { get; set; }

        public string AstRemarks { get; set; }
        public string Cancer { get; set; }
        public string CanDuration { get; set; }
        public string CanRemarks { get; set; }
        public string AlcoholDrugAbuse { get; set; }
        public string AlcDuration { get; set; }
        public string AlcRemarks { get; set; }
        public string IsHIVSTDAilments { get; set; }
        public string HIVDuration { get; set; }
        public string HIVRemarks { get; set; }

        public string OtherAilment { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime DischargeDate { get; set; }
        public int ExpectedStayDays { get; set; }
        public int AdmissionType { get; set; }
        public int AccommodationClass { get; set; }
        public int RoomRent { get; set; }
        public int BedNumber { get; set; }
        public double ICUCharges { get; set; }
        public double OTCharges { get; set; }
        public double NursingCharges { get; set; }
        public double PatientDiet { get; set; }
        public double ExpCostInvDiag { get; set; }
        public double SurgeonProfessionalFees { get; set; }
        public double AnaesthetistFees { get; set; }
        public double ConsultationCharges { get; set; }
        public double Medicines { get; set; }
        public double Consumables { get; set; }
        public double ImplantCost { get; set; }
        public double OtherHospitalExpenses { get; set; }
        public double InclusivePackages { get; set; }
        public string PackageName { get; set; }
        public string ClaimCategory { get; set; }
        public string ExpenseType { get; set; }
        public string ICUType { get; set; }
        public string HospitalizationType { get; set; }
        public string ToalExpCostHospitalizationD { get; set; }
        public string ToalExpCostHospitalizationW { get; set; }
        public string AttachmentType { get; set; }
        public string AttRemarks { get; set; }
        public int EncodedBy { get; set; }
        public int facilityId { get; set; }

        public string xmlSeviceDetails { get; set; }
        public string xmldiffDiagSeviceDetails { get; set; }
        public string NatureIllness { get; set; }
        public string IsEnhancement { get; set; }
        public string EnhancementRefranceId { get; set; }
        public int TPACode { get; set; }
        public int HospitalCode { get; set; }

        public string strXMLAttachement { get; set; }

        public string EnhancementRemark { get; set; }

        public string xmlChecklist { get; set; }

        public bool DetailsFromHIS { get; set; }
        public bool GroupRetail { get; set; }

        public string CorporateName { get; set; }

        public double SumOfInsuredAmount { get; set; }

        public int? intEncounterID { get; set; }

        public string XMLMedicalManagement { get; set; }



    }
    public DataSet getPreAuthDatafortagging(int CreditId, int HospitalLocationID, int iRegistrationId,
                            int iLoginFacilityId, bool isDateFilter, DateTime dtDateFrom, DateTime dtDateTo)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@intId", CreditId);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        HshIn.Add("@intRegistrationId", iRegistrationId);
        HshIn.Add("@intLoginFacilityId", iLoginFacilityId);

        if (isDateFilter)
        {
            HshIn.Add("@dtFromDate", dtDateFrom);
            HshIn.Add("@dtToDate", dtDateTo);
        }

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPreAuthforTagging", HshIn, HshOut);

        return ds;
    }

    public DataSet bindHCXPatientList(int ID, int HospitalLocationID, int iRegistrationId,
                            int iLoginFacilityId, bool isDateFilter, DateTime dtDateFrom, DateTime dtDateTo)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@intId", ID);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        HshIn.Add("@intRegistrationId", iRegistrationId);
        HshIn.Add("@intLoginFacilityId", iLoginFacilityId);

        if (isDateFilter)
        {
            HshIn.Add("@dtFromDate", dtDateFrom);
            HshIn.Add("@dtToDate", dtDateTo);
        }

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHCXPatient", HshIn, HshOut);

        return ds;
    }
    public DataSet bindEnhancementList(int ID, int HospitalLocationID, int iRegistrationId,
                            int iLoginFacilityId, bool isDateFilter, DateTime dtDateFrom, DateTime dtDateTo)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        HshIn.Add("@intId", ID);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        HshIn.Add("@intRegistrationId", iRegistrationId);
        HshIn.Add("@intLoginFacilityId", iLoginFacilityId);

        if (isDateFilter)
        {
            HshIn.Add("@dtFromDate", dtDateFrom);
            HshIn.Add("@dtToDate", dtDateTo);
        }

        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEnhancementlist", HshIn, HshOut);

        return ds;
    }
    public DataSet bindPreAuthQueryList(int iLoginFacilityId, string dtDateFrom, string dtDateTo,int PreAuthId ,bool Respond)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();

        
        HshIn.Add("@facilityid", iLoginFacilityId);       
        HshIn.Add("@fdate", dtDateFrom);
        HshIn.Add("@tdate", dtDateTo);
        HshIn.Add("@PreAuthId", PreAuthId);
        HshIn.Add("@response", Respond);

        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "FillInsurancePreApprovalStatus", HshIn, HshOut);

        return ds;
    }

    public DataSet bindInvoiceList(int iLoginFacilityId, string dtDateFrom, string dtDateTo,int PreAuthId)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();


        HshIn.Add("@facilityid", iLoginFacilityId);
        HshIn.Add("@fdate", dtDateFrom);
        HshIn.Add("@tdate", dtDateTo);
        HshIn.Add("@PreAuthId", PreAuthId);




        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "FillFinalInvoices", HshIn, HshOut);

        return ds;
    }
    public DataSet bindDiscrepancyList()
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();   




        ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "FillDiscrepancyMaster", HshIn, HshOut);

        return ds;
    }
    public DataSet bindselectdiscrepancylist(int insurancestatusid) {
        HshIn = new Hashtable();
        DAL.DAL objdl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        HshIn.Add("@insurancestatusid", insurancestatusid);
        ds = (DataSet)objdl.FillDataSet(CommandType.StoredProcedure, "UspInsuranceQuerydescription", HshIn);
        return ds;


    }
    public Hashtable savejson(int id, string PreAuthUrl,string inputJson,string Remark)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        HshIn.Add("@PreAuthId", id);
        HshIn.Add("@JsonData", inputJson);
        HshIn.Add("@PreAuthUrl", PreAuthUrl);
        HshIn.Add("@Remark", Remark);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveInsuranceJson", HshIn,HshOut);

        return HshOut;
    }
    public Hashtable UpdateClaimNo(int id,string claimno)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        HshIn.Add("@id", id);
        HshIn.Add("@ClaimNo", claimno);
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "InsuranceUpdateClaimNo", HshIn, HshOut);

        return HshOut;
    }
    public string SaveCreditLimitData(int PreAuthId, int HospitalLocationID, int RegistrationId, int EncounterId,
                  int EncodedBy )
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();

      
        HshIn.Add("@intPreAuthId", PreAuthId);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        HshIn.Add("@intRegistrationId", RegistrationId);
        if (EncounterId > 0)
        {
            HshIn.Add("@intEncounterId", EncounterId);
        }
        else
        {
            HshIn.Add("@intEncounterId", DBNull.Value);
        }
       
        HshIn.Add("@intEncodedBy", EncodedBy);
       
        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEncounterInPreAuth", HshIn, HshOut);

        return HshOut["@chvErrorStatus"].ToString();
    }
    public Hashtable UpdateEnhancementStatus(int id,int userid)
    {
        HshIn = new Hashtable();
        HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        HshIn.Add("@id", id);
        HshIn.Add("@user", userid);

        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "PostEnhancementDetail", HshIn, HshOut);
        return HshOut;
    }

    public DataSet getPatientDetails(int iHospID, int iFacilityId, int iRegistrationId, int RegistrationNo, int EncodedBy)
    {

        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("inyHospitalLocationId", iHospID);
        HshIn.Add("intFacilityId", iFacilityId);
        HshIn.Add("intRegistrationId", iRegistrationId);       
        HshIn.Add("chvRegistrationNo", RegistrationNo);

        HshIn.Add("intEncodedBy", EncodedBy);
        HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetHCXPatientData", HshIn, HshOut);
        return ds;
    }
    public DataSet getPatientRegistrationNo(string MemberId)
    {

        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@MemberId", MemberId);               
        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "FillRegistrationAgainstMemberID", HshIn, HshOut);
        return ds;
    }

    public DataSet getinvoicedocumnet(int invoiceid)
    {

        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@Invoiceid", invoiceid);
        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "FillInvoiceDocumnet", HshIn, HshOut);
        return ds;
    }
    public DataSet getPreauthqueryResponse(int QueryId)
    {

        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@QueryId", QueryId);
        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPreAuthQueryResponse", HshIn, HshOut);
        return ds;
    }
    public DataSet GetPatientClaimNo(int encouterid)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@encounterid", encouterid);
        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "GetClaimNo", HshIn, HshOut);
        return ds;
    }
    public DataSet GetPatientClaimNoAgainstId(int id)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@refid", id);
        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "GetClaimNoAgainstRefId", HshIn, HshOut);
        return ds;
    }
    public DataSet GetEnhancementDetail(int id)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@id", id);
        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "GetEnhancementDetail", HshIn, HshOut);
        return ds;
    }
    public DataSet GetPatientInvoiceDetail(int encouterid,string strFromDate,string strToDate,int Invoiceid,int FilterByDate,int userid)
    {
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        HshIn.Add("@IPNo", encouterid);
        HshIn.Add("@strFromDate", strFromDate);
        HshIn.Add("@strToDate", strToDate);
        HshIn.Add("@Invoiceid", Invoiceid);
        HshIn.Add("@FilterByDate", FilterByDate);
       // HshIn.Add("@userid", userid);

        DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "spBillDetail", HshIn, HshOut);
        return ds;
    }

}