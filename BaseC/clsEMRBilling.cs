using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace BaseC
{
    public class clsEMRBilling
    {
        private string sConString = "";
        public clsEMRBilling(string conString)
        {
            sConString = conString;
        }

        Hashtable HshIn;
        Hashtable HshOut;
        public DataTable GetEncounterTimeBaseServices(Hashtable HshIn)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEncounterTimeBaseServices", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet getIPEncounterList(int RegistrationId, int HospId, int FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@RegistrationId", RegistrationId);
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityId", FacilityId);

                string qry = " SELECT enc.Id AS EncounterId, enc.EncounterNo " +
                            " FROM Encounter enc WITH (NOLOCK) " +
                            " WHERE enc.RegistrationId = @RegistrationId " +
                            " AND enc.OPIP = 'I' " +
                            " AND enc.Active = 1 " +
                            " AND enc.StatusId <> (SELECT MAX(StatusId) AS StatusId FROM StatusMaster WITH (NOLOCK) WHERE StatusType = 'Encounter' AND Code = 'C') " +
                            " AND enc.FacilityID = @FacilityId " +
                            " AND enc.HospitalLocationId = @HospId " +
                            " ORDER BY enc.EncounterNo ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable getSingleServiceAmount_WithDate(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
               Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge, DateTime Billingdate)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("intCompanyid", iCompanyId);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", cPatientOPIP);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("iEncounterId", iEncounterId);
            HshIn.Add("intServiceId", iServiceId);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intBedCategoryId", 0);
            HshIn.Add("intSpecialisationId", intSpecialisationId);
            HshIn.Add("iEMergencyCharge", iEMergencyCharge);
            HshIn.Add("BillingDate", Billingdate);

            HshOut.Add("NChr", SqlDbType.Money);
            HshOut.Add("DNchr", SqlDbType.Money);
            HshOut.Add("DiscountNAmt", SqlDbType.Money);
            HshOut.Add("DiscountDNAmt", SqlDbType.Money);
            HshOut.Add("PatientNPayable", SqlDbType.Money);
            HshOut.Add("PayorNPayable", SqlDbType.Money);
            HshOut.Add("DiscountPerc", SqlDbType.Money);
            HshOut.Add("IsApproval", SqlDbType.Bit);
            HshOut.Add("IsExcluded", SqlDbType.Bit);
            HshOut.Add("DepartmentId", SqlDbType.VarChar);
            HshOut.Add("DoctorRequired", SqlDbType.Bit);
            HshOut.Add("ServiceType", SqlDbType.VarChar);
            HshOut.Add("IsPriceEditable", SqlDbType.Bit);
            HshOut.Add("ChargeType", SqlDbType.VarChar);
            HshOut.Add("insCoPayPerc", SqlDbType.Money);
            HshOut.Add("insCoPayAmt", SqlDbType.Money);
            HshOut.Add("IsCoPayOnNet", SqlDbType.SmallInt);
            HshOut.Add("mnDeductibleAmt", SqlDbType.Money);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, HshOut);

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getunAdjustedAdvance(int registrationID, int HospitalLocationID, int FacilityID, int CompanyID, string PaymentReference)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@hospitallocationid", HospitalLocationID);
                HshIn.Add("@FacilityID", FacilityID);
                HshIn.Add("@regint", registrationID);
                HshIn.Add("@companyid", CompanyID);
                HshIn.Add("@advanceType", "0");
                HshIn.Add("@PaymentReference", PaymentReference);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUnAdjstutedAdvance", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SavePreAuthData(int CreditId, int HospitalLocationID, int FacilityId, int RegistrationId,
                                    int SponsorId, int PayerId, string PolicyNo, string EmpNo,
                                    int GuarantorId, string NatureOfIllness, DateTime IllnessDate, int TreatingDoctorId,
                                    string KinFirstName, string KinMiddleName, string KinLastName, int KinRelationshipId,
                                    int KinGenderId, DateTime KinDateOfBirth, string KinAddress1, string KinAddress2,
                                    int KinCountryId, int KinStateId, int KinCityId, string KinZipCode, string KinPhone,
                                    string KinMobile, string KinEmail, int EncodedBy, string RequestFileName, string Remarks,
                                    string source, string xmlSeviceDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intId", CreditId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intSponsorId", SponsorId);
                HshIn.Add("@intPayerId", PayerId);
                HshIn.Add("@chvPolicyNo", PolicyNo);
                HshIn.Add("@chvEmpNo", EmpNo);
                HshIn.Add("@intGuarantorId", GuarantorId);
                HshIn.Add("@chvNatureOfIllness", NatureOfIllness);
                HshIn.Add("@dtIllnessDate", IllnessDate);
                HshIn.Add("@intTreatingDoctorId", TreatingDoctorId);
                HshIn.Add("@chvKinFirstName", KinFirstName);
                HshIn.Add("@chvKinMiddleName", KinMiddleName);
                HshIn.Add("@chvKinLastName", KinLastName);
                HshIn.Add("@intKinRelationshipId", KinRelationshipId);
                HshIn.Add("@intKinGenderId", KinGenderId);
                HshIn.Add("@dtKinDateOfBirth", KinDateOfBirth);
                HshIn.Add("@chvKinAddress1", KinAddress1);
                HshIn.Add("@chvKinAddress2", KinAddress2);
                HshIn.Add("@intKinCountryId", KinCountryId);
                HshIn.Add("@intKinStateId", KinStateId);
                HshIn.Add("@intKinCityId", KinCityId);
                HshIn.Add("@chvKinZipCode", KinZipCode);
                HshIn.Add("@chvKinPhone", KinPhone);
                HshIn.Add("@chvKinMobile", KinMobile);
                HshIn.Add("@chvKinEmail", KinEmail);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@RequestFileName", RequestFileName);
                HshIn.Add("@Remarks", Remarks);
                HshIn.Add("@Source", source);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshIn.Add("@xmlSeviceDetails", xmlSeviceDetails);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePreAuth", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable getPatientDiffPayer(int HospitalLocationID, int iLoginFacilityId, int iRegistrationId,
                            int iEncounterId, string OPIP)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intFacilityId", iLoginFacilityId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@chvOPIP", OPIP);
                HshOut.Add("@intDiffCompany", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetPatientCompanyInAdvanceAndIPBill", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPreAuthData(int CreditId, int HospitalLocationID, int iRegistrationId,
                            int iLoginFacilityId, bool isDateFilter, DateTime dtDateFrom, DateTime dtDateTo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
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

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPreAuth", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetPackage(Int32 iHospitalLocationId, Int32 iFacilityId,
        Int32 iRegistrationId, Int32 iEncounterId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@RegistrationId", iRegistrationId);
                HshIn.Add("@EncounterId", iEncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageName", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }



        public string SaveCreditLimitData(int CreditId, int PreAuthId, int HospitalLocationID, int RegistrationId, int EncounterId,
                            DateTime LetterDate, string LetterRefNo, double TreatmentLimitAmount,
                            DateTime TreatmentStartDate, DateTime TreatmentEndDate,
                            string ApprovedBy, int BedCategoryId, double BedCharges,
                            string Source, int StatusId, int EncodedBy, string approvalFilePath, string Remarks, string xmlServiceDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intId", CreditId);
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
            HshIn.Add("@dtLetterDate", LetterDate);
            HshIn.Add("@chvLetterRefNo", LetterRefNo);
            HshIn.Add("@monTreatmentLimitAmount", TreatmentLimitAmount);
            HshIn.Add("@dtTreatmentStartDate", TreatmentStartDate);
            HshIn.Add("@dtTreatmentEndDate", TreatmentEndDate);
            HshIn.Add("@chvApprovedBy", ApprovedBy);
            HshIn.Add("@intBedCategoryId", BedCategoryId);
            HshIn.Add("@dblBedCharges", BedCharges);
            HshIn.Add("@chrSource", Source);
            HshIn.Add("@intStatusId", StatusId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@approvalFilePath", approvalFilePath);
            HshIn.Add("@Remarks", Remarks);
            HshIn.Add("@xmlServiceDetails", xmlServiceDetails);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePatientLimit", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getCreditLimitData(int CreditId, int HospitalLocationID, int iRegistrationId, int iStatusId,
                              string Source, int iLoginFacilityId, bool isDateFilter, DateTime dtDateFrom, DateTime dtDateTo)
        {
            string fDate = dtDateFrom.ToString("yyyy-MM-dd 00:00");
            string tDate = dtDateTo.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intId", CreditId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@chrSource", Source);
                HshIn.Add("@intStatusId", iStatusId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);

                if (isDateFilter)
                {
                    HshIn.Add("@dtFromDate", fDate);
                    HshIn.Add("@dtToDate", tDate);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientLimit", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }




        public DataSet getCompanyList(int iHospId, string CompanyType, int InsuranceId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationID", iHospId);
                HshIn.Add("@chvCompanyType", CompanyType);
                HshIn.Add("@intInsuranceId", InsuranceId);
                HshIn.Add("intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyList", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public int getDefaultCompany(int iHospId, int FacilityId)
        {
            int IsDefault = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intHospId", iHospId);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT dbo.GetHospitalSetupValue(@intHospId,@intFacilityId, 'DefaultHospitalCompany') AS IsDefault";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["IsDefault"] != null)
                    {
                        IsDefault = Convert.ToInt32(ds.Tables[0].Rows[0]["IsDefault"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return IsDefault;
        }

        public DataSet getOutstandingInvoice(int HospId, int RegId, int LoginFacilityId,
                                    DateTime fromDate, DateTime toDate)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@dtFromDate", fromDate);
                HshIn.Add("@dtToDate", toDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetOutstandingInvoice", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getGetOutStanding(int HospId, int RegId, int LoginFacilityId,
                                            string sFromDate, string sToDate, int iCompanyID, int iReceiptID, int iInvoiceID,
                                           string DocNo, Int64 RegistrationNo, string PatientName)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("iHospitalLocationId", HospId);
            HshIn.Add("iLoginFacilityId", LoginFacilityId);
            HshIn.Add("iRegID", RegId);

            if ((sFromDate != "") && (sToDate != ""))
            {
                HshIn.Add("dtFromDate", sFromDate);
                HshIn.Add("dtToDate", sToDate);
            }
            HshIn.Add("iCompanyID", iCompanyID);
            HshIn.Add("iReceiptId", iReceiptID);
            HshIn.Add("iInvoiceId", iInvoiceID);
            HshIn.Add("chvDocNo", DocNo);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            HshIn.Add("chvPatientName", PatientName);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOutStanding", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getAdvanceAndOutstanding(int HospId, int RegId, int LoginFacilityId,
                                            string iEncounterId, string OPIPType, int iCompanyID)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("iHospitalLocationId", HospId);
                HshIn.Add("iFacilityId", LoginFacilityId);
                HshIn.Add("iRegID", RegId);
                HshIn.Add("iEncounterId", iEncounterId);
                HshIn.Add("OPIPType", OPIPType);
                HshIn.Add("iCompanyID", iCompanyID);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetAdvanceOutstandingForRegID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public DataSet getUnSetteledInvoiceRecipts(int HospId, int RegId, int LoginFacilityId,
                                          string sFromDate, string sToDate, string srange, int iYearID,
                                          int iCompanyID, int iReceiptID, int iInvoiceID, string cPatientTypeOPIP)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("iHospitalLocationId", HospId);
                HshIn.Add("iLoginFacilityId", LoginFacilityId);
                HshIn.Add("iRegID", RegId);
                HshIn.Add("iCompanyID", iCompanyID);
                HshIn.Add("iReceiptId", iReceiptID);
                HshIn.Add("iInvoiceId", iInvoiceID);
                if (cPatientTypeOPIP != "")
                    HshIn.Add("cPatientType", cPatientTypeOPIP);

                HshIn.Add("dtFromDate", sFromDate);
                HshIn.Add("dtToDate", sToDate);
                HshIn.Add("chvDateRange", srange);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOutStanding", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public DataSet getServiceOrderForCreditNote(int HospId, int iFacilityId, int InvoiceId, int EncodedBy, int iCreditNoteId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@iHospitalLocationId", HospId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iInvoiceId", InvoiceId);
                HshIn.Add("@iEncodedBy", EncodedBy);
                HshIn.Add("@iCreditNoteId", iCreditNoteId);
                HshOut.Add("@cErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetServiceOrderForCreditNote", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getuspGetInvoiceList(int iHospitalLocationId, int iFacilityId,
            int iInvoiceId, string sInvoiceNo, string dtDateFrom, string dtDateTo, int allInvoice, string sPatientType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iInvoiceId", iInvoiceId);
                HshIn.Add("@sInvoiceNo", sInvoiceNo);
                HshIn.Add("@sFromDate", dtDateFrom);
                HshIn.Add("@sToDate", dtDateTo);
                HshIn.Add("@allInvoice", allInvoice);
                HshIn.Add("@sPatientType", sPatientType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInvoiceList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveInvoiceCreditNote(int HospId, int FacilityId, string xmlCreditNote, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlCreditNote", xmlCreditNote);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveInvoiceCreditNote", HshIn, HshOut);

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getInvoicePatientCompany(int HospId, int RegistrationId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@inyHospitalLocationId", HospId);


                HshIn.Add("@intRegistrationId", RegistrationId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInvoicePatientCompany", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getInvoicePatientCompany(int HospId, int RegistrationId, int EncounterId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);

                if (EncounterId > 0)
                    HshIn.Add("@intEncounterId", EncounterId);
                if (FacilityId > 0)
                    HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInvoicePatientCompany", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getOPIPRegEncDetails(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                        string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                        string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                        int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                        string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno, int EntrySite)
        {
            return getOPIPRegEncDetails(HospId, FacilityId, OPIP, Regid, Encid, RegEnc,
                            RegNo, EncNo, PatientName, dtsDateFrom, dtsDateTo,
                            RecordButton, RowNo, PageSize, EncodedBy, PageNo, BedNo,
                            AdmissionDischargedStausId, Company, Nationality, PhoneNo, mobileNo,
                            Identityno, Passportno, Dob, Email, OldRegistrationno, EntrySite,
                            "", "", "",
                            0, 0, 0, false, "",0);
        }

        public DataSet getOPIPRegEncDetails(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                        string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                        string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                        int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                        string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno, int EntrySite,
                        string MotherName, string FatherName, string PreviousName,
                        int wardid, int DoctorId, int SpecialisationId, bool AllPatients, string LocalAddress, int WardStationId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = "";
                string tDate = "";
                if (dtsDateFrom.HasValue)
                {
                    fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                    tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
                }
                //string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                //string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intRegid", Regid);
                HshIn.Add("@intEncid", Encid);
                HshIn.Add("@intRegEnc", RegEnc);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvEncNo", EncNo);
                HshIn.Add("@chvPatientName", PatientName + "%");
                HshIn.Add("@dtsEncFromDate", fDate);
                HshIn.Add("@dtsEncToDate", tDate);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                HshIn.Add("@chvCompany", Company);
                HshIn.Add("@chvNationality", Nationality);
                HshIn.Add("@intAdmissionDischargedStausId", AdmissionDischargedStausId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@PhoneNo", PhoneNo);
                HshIn.Add("@mobileNo", mobileNo);
                HshIn.Add("@chvIdentityNumber", Identityno);
                HshIn.Add("@chvPassportNo", Passportno);
                if (Dob != "")
                {
                    HshIn.Add("@dtDateofBirth", Dob);
                }
                HshIn.Add("@chvEmail", Email);
                HshIn.Add("@chvRegistrationNoOld", OldRegistrationno);

                if (EntrySite > 0)
                    HshIn.Add("@intEntrySite", EntrySite);

                if (MotherName != string.Empty)
                    HshIn.Add("@MotherName", MotherName);

                if (FatherName != string.Empty)
                    HshIn.Add("@FatherName", FatherName);

                if (PreviousName != string.Empty)
                    HshIn.Add("@PreviousName", PreviousName);

                if (wardid > 0)
                    HshIn.Add("@intWardId", wardid);

                if (DoctorId > 0)
                    HshIn.Add("@intDoctorId", DoctorId);

                if (SpecialisationId > 0)
                    HshIn.Add("@intSpecialisationId", SpecialisationId);

                if (AllPatients)
                    HshIn.Add("@bitAllPatients", AllPatients);

                if (LocalAddress != string.Empty)
                    HshIn.Add("@LocalAddress", LocalAddress);
                if (WardStationId > 0)
                    HshIn.Add("@intWardStationId", WardStationId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPIPRegEncDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getMRDOPIPRegEncDetails(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                   string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                   string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                   int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                   string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno, int EntrySite,
                   string MotherName, string FatherName, string PreviousName,
                   int wardid, int DoctorId, int SpecialisationId, bool AllPatients, string LocalAddress)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = "";
                string tDate = "";
                if (dtsDateFrom.HasValue)
                {
                    fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                    tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
                }
                //string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                //string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intRegid", Regid);
                HshIn.Add("@intEncid", Encid);
                HshIn.Add("@intRegEnc", RegEnc);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvEncNo", EncNo);
                HshIn.Add("@chvPatientName", PatientName + "%");
                HshIn.Add("@dtsEncFromDate", fDate);
                HshIn.Add("@dtsEncToDate", tDate);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                HshIn.Add("@chvCompany", Company);
                HshIn.Add("@chvNationality", Nationality);
                HshIn.Add("@intAdmissionDischargedStausId", AdmissionDischargedStausId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@PhoneNo", PhoneNo);
                HshIn.Add("@mobileNo", mobileNo);
                HshIn.Add("@chvIdentityNumber", Identityno);
                HshIn.Add("@chvPassportNo", Passportno);
                if (Dob != "")
                {
                    HshIn.Add("@dtDateofBirth", Dob);
                }
                HshIn.Add("@chvEmail", Email);
                HshIn.Add("@chvRegistrationNoOld", OldRegistrationno);
                HshIn.Add("@intEntrySite", EntrySite);

                if (MotherName != string.Empty)
                    HshIn.Add("@MotherName", MotherName);
                if (FatherName != string.Empty)
                    HshIn.Add("@FatherName", FatherName);
                if (PreviousName != string.Empty)
                    HshIn.Add("@PreviousName", PreviousName);

                HshIn.Add("@intWardId", wardid);
                HshIn.Add("@intDoctorId", DoctorId);
                HshIn.Add("@intSpecialisationId", SpecialisationId);
                HshIn.Add("@bitAllPatients", AllPatients);

                if (LocalAddress != string.Empty)
                    HshIn.Add("@LocalAddress", LocalAddress);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMRDOPIPRegEncDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getOPIPRegEncDetailsDischargesForMRDFileIssue(int HospId, int FacilityId, int Regid, int Encid,
                             string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo
            , int MRDStatusId, bool MoreThenTimeLimit, string RackNo, string WardId, string CompanyId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = "";
                string tDate = "";
                if (dtsDateFrom.HasValue)
                {
                    fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                    tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
                }
                //string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                //string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                HshIn.Add("@intRegid", Regid);
                HshIn.Add("@intEncid", Encid);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvEncNo", EncNo);
                HshIn.Add("@chvRackNo", RackNo);
                HshIn.Add("@chvPatientName", PatientName + "%");
                HshIn.Add("@dtsEncFromDate", fDate);
                HshIn.Add("@dtsEncToDate", tDate);
                HshIn.Add("@intMRDStatusId", MRDStatusId);
                HshIn.Add("@bitMoreThenTimeLimit", MoreThenTimeLimit);
                HshIn.Add("@intWardId", WardId);
                HshIn.Add("@CompanyId", CompanyId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIpRegEncDetailsDischargesForMRDFileIssue", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getOPIPRegEncDetailsFemaleOnly(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                                     string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                                     string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                                     int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                                     string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno, int gender,
                                     int EntrySite, string MotherName, String FatherName, string PreviousName)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = "";
                string tDate = "";
                if (dtsDateFrom.HasValue)
                {
                    fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                    tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
                }
                //string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                //string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intRegid", Regid);
                HshIn.Add("@intEncid", Encid);
                HshIn.Add("@intRegEnc", RegEnc);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvEncNo", EncNo);
                HshIn.Add("@chvPatientName", PatientName + "%");
                HshIn.Add("@dtsEncFromDate", fDate);
                HshIn.Add("@dtsEncToDate", tDate);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                HshIn.Add("@chvCompany", Company);
                HshIn.Add("@chvNationality", Nationality);
                HshIn.Add("@intAdmissionDischargedStausId", AdmissionDischargedStausId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@PhoneNo", PhoneNo);
                HshIn.Add("@mobileNo", mobileNo);
                HshIn.Add("@chvIdentityNumber", Identityno);
                HshIn.Add("@chvPassportNo", Passportno);
                if (Dob != "")
                {
                    HshIn.Add("@dtDateofBirth", Dob);
                }
                HshIn.Add("@chvEmail", Email);
                HshIn.Add("@chvRegistrationNoOld", OldRegistrationno);
                HshIn.Add("@gender", gender);

                HshIn.Add("@intEntrySite", EntrySite);
                if (MotherName != string.Empty)
                    HshIn.Add("@MotherName", MotherName);
                if (FatherName != string.Empty)
                    HshIn.Add("@FatherName", FatherName);
                if (PreviousName != string.Empty)
                    HshIn.Add("@PreviousName", PreviousName);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPIPRegEncDetailsFemaleOnly", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEntrySite(int Id, int FaclityId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@Userid", Id);
                HshIn.Add("@FacilityId", FaclityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEntrySites", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getEntrySiteForLogin(int Id, int FaclityId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@Userid", Id);
                HshIn.Add("@FacilityId", FaclityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEntrySitesForLogin", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getPatientDashboard(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int AdmDisc,
                          string RegNo, string EncNo, string PatientName, string sDateFrom, string sDateTo,
                          string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                          int AdmissionDischargedStausId, string Company, string DoctorName, string PatientAddress, string PhoneNo, string mobileNo, string AdmissionDate, int WardStationId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intRegid", Regid);
                HshIn.Add("@intEncid", Encid);
                HshIn.Add("@intAdmDischarge", AdmDisc);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvEncNo", EncNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@dtsEncFromDate", sDateFrom);
                HshIn.Add("@dtsEncToDate", sDateTo);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@chvCompany", Company);
                HshIn.Add("@intAdmissionDischargedStausId", AdmissionDischargedStausId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@PhoneNo", PhoneNo);
                HshIn.Add("@mobileNo", mobileNo);
                if (PatientAddress != "")
                {
                    HshIn.Add("@chvPatientAddress", PatientAddress);
                }
                HshIn.Add("@chvDoctorName", DoctorName);
                HshIn.Add("@chrAdmissionDate", AdmissionDate);
                HshIn.Add("@WardStationId", WardStationId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPAdmittedDischargeDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getSearchPatientRecords(int HospId, int FacilityId, string RegNo, string PatientName, string sDateFrom, string sDateTo,
            int PageSize, int EncodedBy, int PageNo, string Company, string MobileNo, string PatientAddress, int IsER, int AdmDischarge)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                HshIn.Add("@chvCompany", Company);
                HshIn.Add("@chrFromDate", sDateFrom);
                HshIn.Add("@chrToDate", sDateTo);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@mobileNo", MobileNo);
                if (PatientAddress != "")
                {
                    HshIn.Add("@chvPatientAddress", PatientAddress);
                }
                HshIn.Add("@IsER", IsER);
                HshIn.Add("@intAdmDischarge", AdmDischarge);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSearchRegistrationRecords", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getInvoiceCreditNote(int CreditNoteId, int HospId, int FacilityId, int RegistrationId,
                                            string PatientName, string DocumentNo, string DocumentType,
                                            DateTime dtsDateFrom, DateTime dtsDateTo,
                                            string RecordButton, int RowNo, int PageSize, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intCreditNoteId", CreditNoteId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvDocumentNo", DocumentNo);
                HshIn.Add("@chrDocumentType", DocumentType);
                HshIn.Add("@dtsFromDate", fDate);
                HshIn.Add("@dtsToDate", tDate);
                HshIn.Add("@chvRecordButton", RecordButton);
                HshIn.Add("@intRowNo", RowNo);
                //HshIn.Add("@intPageSize", PageSize);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInvoiceCreditNote", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getOutStandingCompany(int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intHospId", HospId);

                string qry = "SELECT * FROM ( " +
                        " SELECT DISTINCT inv.SponsorId AS CompanyId, cmp.Name AS Company " +
                        " FROM Invoice inv WITH (NOLOCK) INNER JOIN Company cmp WITH (NOLOCK) ON inv.SponsorId = cmp.CompanyId " +
                        " WHERE inv.Active=1 AND inv.HospitalLocationId=@intHospId " +
                        " UNION  " +
                        " SELECT DISTINCT rec.CompanyId AS CompanyId, cmp.Name AS Company " +
                        " FROM ReceiptMain rec WITH (NOLOCK) INNER JOIN Company cmp WITH (NOLOCK) ON rec.CompanyId = cmp.CompanyId " +
                        " WHERE rec.Active=1 AND rec.HospitalLocationId=@intHospId " +
                        " )X ORDER BY Company ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public Boolean IsInvoiceAdjusted(int iInvoiceID, int HospId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@intInvoiceID", iInvoiceID);
                HshIn.Add("@intHospId", HospId);

                string qry = " SELECT ISNULL(SUM(AmountAdjusted),0) as AmountAdjusted FROM InvoiceSettlementDetail WITH (NOLOCK) WHERE InvoiceId =@intInvoiceID And HospitalLocationId =@intHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToDouble(ds.Tables[0].Rows[0]["AmountAdjusted"]) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }


        public String GetEncountNo(int iEncID, int HospId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@intEncounterID", iEncID);
                HshIn.Add("@intHospId", HospId);

                string qry = " select * from Encounter WITH (NOLOCK) where ID =@intEncounterID And HospitalLocationId =@intHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0]["EncounterNo"]);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

            //
        }


        public Boolean IsInvoiceCancel(int iInvoiceID, int HospId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@intInvoiceID", iInvoiceID);
                HshIn.Add("@intHospId", HospId);

                string qry = " Select InvoiceId from Invoice WITH (NOLOCK) where RefInvoiceId =@intInvoiceID And HospitalLocationId =@intHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientwisebedcategory(int iHospitalId, int iRegId, int iIPNO)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@intEncounterNo", iIPNO);
                string str = "select CurrentBedCategory from Admission WITH (NOLOCK) where EncounterNo= @intEncounterNo and RegistrationId=@intRegistrationId and  HospitalLocationId=@intHospitalLocationId";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }
        }
        public int GetPatientConsultingDoctor(int iHospitalId, int iFacilityId, int iRegId, int iEncdId)
        {
            int iAdviserDoctor = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospitalId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@intEncounterId", iEncdId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientConsultingDoctor", HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    iAdviserDoctor = Convert.ToInt32(ds.Tables[0].Rows[0]["ConsultingDoctorId"]);
                }

                return iAdviserDoctor;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDepartmentInSubDepartment(int iHospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalId);
                string str = " SELECT DISTINCT dm.DepartmentID, dm.DepartmentName FROM DepartmentMain dm WITH (NOLOCK) " +
                            " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ds.DepartmentID = dm.DepartmentID AND ds.Active = 1 " +
                            " WHERE dm.Active = 1 AND dm.HospitalLocationId = @intHospitalLocationId ORDER BY dm.DepartmentName";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet fillPreviousEncounter(int HospitalLocationId, int statusId, string RegistrationNo, int facilityid)
        {

            Hashtable hsInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hsInput.Add("inyHospitalLocationId", HospitalLocationId);
                hsInput.Add("intStatusId", statusId);
                hsInput.Add("chvRegistrationNo", RegistrationNo);
                hsInput.Add("intLoginFacilityId", facilityid);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchEncounter", hsInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public Hashtable CreateEncounter(int HospitalLocationId, int RegistrationID, string RegistrationNo, int doctorid, int EncodedBy,
                                        int facilityId, int InternalEncounter)
        {
            Hashtable hsOutput = new Hashtable();
            Hashtable hsInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                hsInput.Add("inyHospitalLocationID", HospitalLocationId);
                hsInput.Add("intRegistrationNo", RegistrationNo);
                hsInput.Add("intRegistrationID", RegistrationID);
                hsInput.Add("intDoctorID", doctorid);
                hsInput.Add("intEncodedBy", EncodedBy);
                hsInput.Add("intFacilityID", facilityId);
                hsInput.Add("isInternalEncounter", InternalEncounter);

                hsOutput.Add("intOPEncounterNo", SqlDbType.Int);
                hsOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                hsOutput.Add("intEncounterID", SqlDbType.Int);

                hsOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGenerateOPEncounterNo", hsInput, hsOutput);
                return hsOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet CurrencyExchange(int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCurrencyExchange", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetApprovalStatusServiceWise(int intPreAuthId)
        {
            HshIn = new Hashtable();
            HshIn.Add("intPreAuthId", intPreAuthId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetApprovalStatusServiceWise", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAppointmentserviceId(int HospitalLocationId, int FacilityId, int AppId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intAppointmentId", AppId);

            string strsql = "select PackageId,RegistrationId,ios.ServiceName,VisitTypeId from DoctorAppointment da WITH (NOLOCK) " +
                            " INNER JOIN ItemOfService ios WITH (NOLOCK) on da.PackageId =ios.ServiceId " +
                            " where AppointmentId =@intAppointmentId and FacilityID=@intFacilityId and da.HospitalLocationId =@inyHospitalLocationId";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPackageServiceRendering(int ProlongPackageId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshIn.Add("intProlongPackageId", ProlongPackageId);
            HshIn.Add("intLoginFacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientProlongPackageDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPackageUnitConsumed(int HospId, int ProlongPackageId, int ServiceId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intProlongPackageId", ProlongPackageId);
            HshIn.Add("intServiceId", ServiceId);
            HshIn.Add("intLoginFacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageServiceRenderDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int SavePackageServiceRendering(int HospitalLocationId, int ProlongPackageId, int PackageId, int FacilityId, string strXMLService, int RegistrationId, int EncodedBy)
        {
            // Hashtable hsOutput = new Hashtable();
            Hashtable hsInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                hsInput.Add("inyHospitalLocationId", HospitalLocationId);
                hsInput.Add("intProlongPackageId", ProlongPackageId);
                hsInput.Add("intPackageId", PackageId);
                hsInput.Add("intFacilityId", FacilityId);
                hsInput.Add("xmlServices", strXMLService);
                hsInput.Add("intRegistrationId", RegistrationId);
                hsInput.Add("intEncodedBy", EncodedBy);

                return Convert.ToInt32(objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGenerateOPEncounterNo", hsInput));

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string deletePackageRenderedService(int ServiceId, int RenderedServiceId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intServiceId", ServiceId);
            HshIn.Add("@intRenderedServiceId", RenderedServiceId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeletePackageRenderedService", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPatientApprovalCode(int registrationId, int doctorid)
        {
            HshIn = new Hashtable();
            HshIn.Add("intregistrationId", registrationId);
            HshIn.Add("intDoctorId", doctorid);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientApprovalCode", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelPreAuth(int preAuthid, string CancelReason)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@PreAuthid", preAuthid);
            HshIn.Add("@CancelReason", CancelReason);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelPreAuth", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getUnSetteledInvoiceReciptsReport(int HospId, int RegId, int LoginFacilityId,
                                          string sFromDate, string sToDate, string srange, int iYearID,
                                          int iCompanyID, int iReceiptID, int iInvoiceID, string cPatientTypeOPIP)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("iHospitalLocationId", HospId);
                HshIn.Add("iLoginFacilityId", LoginFacilityId);
                HshIn.Add("iRegID", RegId);
                HshIn.Add("iCompanyID", iCompanyID);
                HshIn.Add("iReceiptId", iReceiptID);
                HshIn.Add("iInvoiceId", iInvoiceID);
                if (cPatientTypeOPIP != "")
                    HshIn.Add("cPatientType", cPatientTypeOPIP);

                HshIn.Add("dtFromDate", sFromDate);
                HshIn.Add("dtToDate", sToDate);
                HshIn.Add("chvDateRange", srange);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOutStandingReport", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet uspgetReceiptSettlement(int HospId, int FacilityId, string ReceiptNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("hospLocationID", HospId);
                HshIn.Add("facilityID", FacilityId);
                HshIn.Add("ReceiptNo", ReceiptNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetReceiptSettlement", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet uspgetReceiptSettlementByDate(int HospId, int FacilityId, string FromDate, string ToDate, int ComapanyId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("hospLocationID", HospId);
                HshIn.Add("facilityID", FacilityId);
                HshIn.Add("FromDate", FromDate);
                HshIn.Add("ToDate", ToDate);
                HshIn.Add("ComapanyId", ComapanyId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetReceiptSettlementDateWise", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string getHospitalSetupValue(string Flag, int HospId)
        {
            string val = "";
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@chvFlag", Flag);
                HshIn.Add("@intHospId", HospId);

                string qry = "SELECT Value FROM HospitalSetup WITH (NOLOCK) WHERE Flag = @chvFlag AND Active = 1 AND HospitalLocationId = @intHospId  ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = Convert.ToString(ds.Tables[0].Rows[0]["Value"]);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

            return val;
        }

        public string getHospitalSetupValue(string Flag, int HospId, int FacilityId)
        {
            string val = "";
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn.Add("@chvFlag", Flag);
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT Value FROM HospitalSetup WITH (NOLOCK) " +
                            " WHERE Flag = @chvFlag AND Active = 1 AND FacilityId= @intFacilityId AND HospitalLocationId = @intHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = Convert.ToString(ds.Tables[0].Rows[0]["Value"]);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

            return val;
        }

        public DataSet uspGetDepartmentByType(string type)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("type", type);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDepartmentByLab", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet DoctorConsultationVisit(int HospId, int facilityid, int doctorId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityid", facilityid);
                HshIn.Add("@intDoctorId", doctorId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetDoctorConsultationVisit", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public void CalculateLuxuryTax(int EncounterId, int RegistrationId, int CompanyId, int HospitalLocationId, int FacilityId, int Userid, string OPIP, string TaxType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@insHospitalLocationId", HospitalLocationId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Userid);
                HshIn.Add("@OPIP", OPIP);
                HshIn.Add("@TaxType", TaxType);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                int Result = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspCalculateLuxuryTax", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetServiceChargeSurgery_WithoutEncounter(int iHospId, int iFacilityId, int iRegId, int iEncId, int iCompId, int iInsId,
        int iCardId, int iBedCatId, String sOPIP, int iOTServiceId, int iAnesthesiaServiceId, StringBuilder xmlSurgeryServices, DateTime dtOrderDate)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                Hashtable hstInput = new Hashtable();
                hstInput.Add("inyHospitalLocationID", iHospId);
                hstInput.Add("intFacilityId", iFacilityId);
                hstInput.Add("intRegistrationId", iRegId);
                hstInput.Add("iEncounterId", iEncId);
                hstInput.Add("intCompanyid", iCompId);
                hstInput.Add("iInsuranceId", iInsId);
                hstInput.Add("iCardId", iCardId);
                hstInput.Add("intBedCategoryId", iBedCatId);
                hstInput.Add("cPatientOPIP", sOPIP);
                hstInput.Add("intOTServiceId", iOTServiceId);
                hstInput.Add("intAnesthesiaServiceId", iAnesthesiaServiceId);
                hstInput.Add("xmlSurgeryServices", xmlSurgeryServices.ToString());
                hstInput.Add("dtsOrderDate", dtOrderDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceChargeSurgery_WithoutEncounter", hstInput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public DataSet GetUnregisteredPatientDetails(int UnregisteredPatientID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn.Add("@intUnregisteredID", UnregisteredPatientID);

                string qry = @"SELECT Id,HospitalLocationId,FacilityID,TitleId,FirstName,MiddleName,LastName,
                            DateofBirth,AgeYear,AgeMonth,AgeDays,Gender,MaritalStatus,LocalAddress,LocalAddress2,
                            Localpin,LocalCity,LocalState,LocalCountry,PhoneHome,MobileNo,Email,ReligionID,NationalityID,
                            LanguageId,ResidentType,EncodedBy,EncodedDate,LastChangedBy,LastChangedDate,CityAreaID
                            FROM dbo.TempCounselingUnregisteredPatientDetails WITH (NOLOCK) where Id=@intUnregisteredID";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getApprovalStatusDetail(int IntFacilityId, int IntHospitalLocationId, DateTime FromDate, DateTime ToDate, string ApprovalLevel)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
                string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("@IntFacilityId", IntFacilityId);
                HshIn.Add("@IntHospitalLocationId", IntHospitalLocationId);
                HshIn.Add("@FromDate", fDate);
                HshIn.Add("@ToDate", tDate);
                HshIn.Add("@ApprovalLevel", ApprovalLevel);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiscountApprovalAproveStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPendingStatusDetail(int IntFacilityId, int IntHospitalLocationId, DateTime FromDate, DateTime ToDate, string ApprovalLevel)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string fDate = FromDate.ToString("yyyy/MM/dd");
                string tDate = ToDate.ToString("yyyy/MM/dd");

                HshIn.Add("@IntFacilityId", IntFacilityId);
                HshIn.Add("@IntHospitalLocationId", IntHospitalLocationId);
                HshIn.Add("@FromDate", fDate);
                HshIn.Add("@ToDate", tDate);
                HshIn.Add("@ApprovalLevel", ApprovalLevel);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiscountApprovalPendingStatus", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPatientApprovalDetail(int ApprovalId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@ApprovalId", ApprovalId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspDiscountApprovalDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string UpdateApprovelDetail(int HospitalLocationID, int EncodedBy, string ApprovalLevel, string xmlAppStatus)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospitalLocationID);
            HshIn.Add("@EncodedBy", EncodedBy);
            HshIn.Add("@ApprovalLevel", ApprovalLevel);
            HshIn.Add("@xmlAppStatus", xmlAppStatus);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateApprovalStatus", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDiscountApprovalLevel(int IntFacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@Facilityid", IntFacilityId);
                HshIn.Add("@EmployeeId", EncodedBy);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDiscountApprovalLevel", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetApprovalMasterDetail(int HospitalLocationID, int IntFacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inthospitalLocationID", HospitalLocationID);
                HshIn.Add("@intFacilityID", IntFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetApprovalMasterDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEmployeeApprovalDetail(int IntFacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intFacilityId", IntFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEmployeeApprovalDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveEmployeeDiscountDetail(int intFacilityid, int inyHospitalLocationId, int intApprovalLevel,
            int intEmployeeId, int Active, int intEncodedBy)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intFacilityid", intFacilityid);
            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intApprovalLevel", intApprovalLevel);
            HshIn.Add("@intEmployeeId", intEmployeeId);
            HshIn.Add("@Active", Active);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEmployeeDiscountDetail", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string ValidateConsultationDoctor(int HospitalLocationId, int Facility, int registrationid, int newConsultationDoctor, string ConsultantChangeRemarks)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();


            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacility", Facility);
            HshIn.Add("@registrationid", registrationid);
            HshIn.Add("@newConsultationDoctor", newConsultationDoctor);
            HshIn.Add("@ConsultantChangeRemarks", ConsultantChangeRemarks);
            HshOut.Add("@isAllow", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspValidateConsultationDoctor", HshIn, HshOut);
                return HshOut["@isAllow"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getExternalPatientStatus(int iHospID, string statusType, string Code)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospId", iHospID);
                HshIn.Add("@chvStatusType", statusType);
                HshIn.Add("@chvCode", Code);

                string strqry = "SELECT Status, StatusColor, StatusId, Code FROM GetStatus(@inyHospId, @chvStatusType) ";
                if (Code != "")
                {
                    strqry += " WHERE Code=@chvCode ";
                }
                strqry += " ORDER BY SequenceNo";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPaymentModeAndBankMaster()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPaymentModeAndBankMaster");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetPaymentModeAndBankMaster(int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("iFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPaymentModeAndBankMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPackageServiceRenderingArvind(int ProlongPackageId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshIn.Add("intProlongPackageId", ProlongPackageId);
            HshIn.Add("intLoginFacilityId", FacilityId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientProlongPackageDetailsArvind", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getInvoiceIDAndYearIDByInvoiceNoAndFacilityID(int iFacilityID, string sInvoiceNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@sInvoiceNo", sInvoiceNo);
                HshIn.Add("@iFacilityID", iFacilityID);
                string strqry = "Select InvoiceId,YearId from Invoice where InvoiceNo = @sInvoiceNo and FacilityId = @iFacilityID";
                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getOPIPRegEncDetails(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                             string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                             string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                             int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                             string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno, int EntrySite, string MotherName, string FatherName, string PreviousName, string LocalAddress)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                string fDate = "";
                string tDate = "";
                if (dtsDateFrom.HasValue)
                {
                    fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                    tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
                }
                //string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                //string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvOPIP", OPIP);
                HshIn.Add("@intRegid", Regid);
                HshIn.Add("@intEncid", Encid);
                HshIn.Add("@intRegEnc", RegEnc);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvEncNo", EncNo);
                HshIn.Add("@chvPatientName", PatientName + "%");
                HshIn.Add("@dtsEncFromDate", fDate);
                HshIn.Add("@dtsEncToDate", tDate);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@inyPageSize", PageSize);
                HshIn.Add("@intPageNo", PageNo);
                HshIn.Add("@chvCompany", Company);
                HshIn.Add("@chvNationality", Nationality);
                HshIn.Add("@intAdmissionDischargedStausId", AdmissionDischargedStausId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@PhoneNo", PhoneNo);
                HshIn.Add("@mobileNo", mobileNo);
                HshIn.Add("@chvIdentityNumber", Identityno);
                HshIn.Add("@chvPassportNo", Passportno);
                if (Dob != "")
                {
                    HshIn.Add("@dtDateofBirth", Dob);
                }
                HshIn.Add("@chvEmail", Email);
                HshIn.Add("@chvRegistrationNoOld", OldRegistrationno);
                HshIn.Add("@intEntrySite", EntrySite);
                HshIn.Add("@MotherName", MotherName);
                HshIn.Add("@FatherName", FatherName);
                HshIn.Add("@PreviousName", PreviousName);
                HshIn.Add("@LocalAddress", LocalAddress);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPIPRegEncDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getCompanyListByCompanyType(int iHospId, string CompanyType, int iCompanyTypeId, int InsuranceId, int FacilityId) //my
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@inyHospitalLocationID", iHospId);
                HshIn.Add("@chvCompanyType", CompanyType);
                HshIn.Add("@iCompanyTypeId", iCompanyTypeId);
                HshIn.Add("@intInsuranceId", InsuranceId);
                HshIn.Add("intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyListByCompanyType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getPatientDetailsWithInvoiceNo(int iHospID, int iFacilityId, string RegistrationNo)//my14112016
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("intHospitalLocationId", iHospID);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("chvRegistrationNo", RegistrationNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetailsWithInvoiceNo", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet CheckSentForBilling(int iEncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iEncounterId", iEncounterId);

                string strqry = "select sm.Code,sm.StatusId from encounter e inner join StatusMaster sm on sm.StatusId = e.StatusId where id = @iEncounterId ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetLinkedSeviceDetails(int MainServiceId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@MainServiceId", MainServiceId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetLinkedSeviceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveLinkService(int HospitalLocationId, int FacilityId, string XMLSetDetails, int MainServiceId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@HospitalLocationId", HospitalLocationId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@XMLSetDetails", XMLSetDetails);
                HshIn.Add("@MainServiceId", MainServiceId);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveLinkService", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string DeactivateLinkServiceSetup(int id)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@Id", id);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeactivateLinkServiceSetup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetOTRequestList(int HospId, int FacilityId, int Encodedby)
        {
            return GetOTRequestList(HospId, FacilityId, Encodedby, "", "", "");

        }
        public DataSet GetOTRequestList(int HospId, int FacilityId, int Encodedby, string RegistratioNo, string EncounterNo, string PatientName)
        {
            return GetOTRequestList(HospId, FacilityId, Encodedby, RegistratioNo, EncounterNo, PatientName, "", "");

        }
        public DataSet GetOTRequestList(int HospId, int FacilityId, int Encodedby, string RegistratioNo, string EncounterNo, string PatientName, string FromDate, string ToDate)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {



                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@chvRegistrationNo", RegistratioNo);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@FromDate", FromDate);
                HshIn.Add("@ToDate", ToDate);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOTRequestList", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAnesthesiaType(int iFacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@FacilityId", iFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetAnesthesiaType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public Hashtable SaveDischargeFiletransferfromWardtoMRD(int HospId, int FacilityId, string xmlServices,
           int type, int Encodedby)

        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@xmlServices", xmlServices);
            HshIn.Add("@intType", type);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@intOrderNo", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDischargeFiletransferfromWardtoMRD", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string UpdateDischargeFileRemark(int HospId, int FacilityId, string xmlServices, int Encodedby)

        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@xmlServices", xmlServices);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPUpdateDischargeFileRemark", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationList(int intRegistrationNo, string chvPatientName, string chvmobile,
                        string LocalAddress, string chvCompany, string chvRegistrationNoOld, string DOB, string TaggedEmpNo,
                        string GuardianName)
        {
            Hashtable hsIn = new Hashtable();
            hsIn.Add("@intRegistrationNo", intRegistrationNo);
            hsIn.Add("@chvPatientName", chvPatientName);
            hsIn.Add("@chvmobile", chvmobile);
            hsIn.Add("@LocalAddress", LocalAddress);
            hsIn.Add("@chvCompany", chvCompany);
            hsIn.Add("@chvRegistrationNoOld", chvRegistrationNoOld);
            if (DOB != "")
            {
                hsIn.Add("@dtDateofBirth", DOB);
            }
            hsIn.Add("@TaggedEmpNo", TaggedEmpNo);
            hsIn.Add("@GuardianName", GuardianName);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetRegistrationList", hsIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataTable GetHCXSurgeryLists(string etext)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            // objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // DataSet ds = new DataSet();
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                if (etext.ToString().Trim() != "")
                {
                    hstInput.Add("@chvSearchCriteria", "%" + etext + "%");
                }

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHCXSurgeryLists", hstInput).Tables[0];

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }
        #region PreAuth
        public DataSet getInsuranceCompanyList(int iHospId, string CompanyType, int InsuranceId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("@inyHospitalLocationID", iHospId);
            HshIn.Add("@chvCompanyType", CompanyType);
            HshIn.Add("@intInsuranceId", InsuranceId);
            //HshIn.Add("@intFacilityId", FacilityId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyList", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPreAuthStatus(int iHospId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // DataSet ds = new DataSet();

            HshIn.Add("@inyHospitalLocationID", iHospId);
            HshIn.Add("@intFacilityId", FacilityId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
               return  objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreAuthStatus", HshIn);
              
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        #endregion

        //consent Form
        public DataSet getOPIPPendingSignature(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                    string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                    string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                    int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                    string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno)
        {
            return getOPIPPendingSignature(HospId, FacilityId, OPIP, Regid, Encid, RegEnc,
                             RegNo, EncNo, PatientName, dtsDateFrom, dtsDateTo,
                             RecordButton, RowNo, PageSize, EncodedBy, PageNo, BedNo,
                             AdmissionDischargedStausId, Company, Nationality, PhoneNo, mobileNo,
                             Identityno, Passportno, Dob, Email, OldRegistrationno, 0);
        }
        public DataSet getOPIPPendingSignature(int HospId, int FacilityId, string OPIP, int Regid, int Encid, int RegEnc,
                           string RegNo, string EncNo, string PatientName, DateTime? dtsDateFrom, DateTime? dtsDateTo,
                           string RecordButton, int RowNo, int PageSize, int EncodedBy, int PageNo, string BedNo,
                           int AdmissionDischargedStausId, string Company, string Nationality, string PhoneNo, string mobileNo,
                           string Identityno, string Passportno, string Dob, string Email, string OldRegistrationno, int EntrySite)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            string fDate = "";
            string tDate = "";
            if (dtsDateFrom.HasValue)
            {
                fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
            }
            //string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
            //string tDate = dtsDateTo.ToString("yyyy-MM-dd");

            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvOPIP", OPIP);
            HshIn.Add("@intRegid", Regid);
            HshIn.Add("@intEncid", Encid);
            HshIn.Add("@intRegEnc", RegEnc);
            HshIn.Add("@chvRegNo", RegNo);
            HshIn.Add("@chvEncNo", EncNo);
            HshIn.Add("@chvPatientName", PatientName + "%");
            HshIn.Add("@dtsEncFromDate", fDate);
            HshIn.Add("@dtsEncToDate", tDate);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@inyPageSize", PageSize);
            HshIn.Add("@intPageNo", PageNo);
            HshIn.Add("@chvCompany", Company);
            HshIn.Add("@chvNationality", Nationality);
            HshIn.Add("@intAdmissionDischargedStausId", AdmissionDischargedStausId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@PhoneNo", PhoneNo);
            HshIn.Add("@mobileNo", mobileNo);
            HshIn.Add("@chvIdentityNumber", Identityno);
            HshIn.Add("@chvPassportNo", Passportno);
            if (Dob != "")
            {
                HshIn.Add("@dtDateofBirth", Dob);
            }
            HshIn.Add("@chvEmail", Email);
            HshIn.Add("@chvRegistrationNoOld", OldRegistrationno);
            HshIn.Add("@intEntrySite", EntrySite);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPIPPendingSignature", HshIn, HshOut);
            return ds;
        }

        //Admission Form DischargeSummary
        public DataSet GetDocumentPrintingLog(string DocumentType, int RegistrationId, int EncounterId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("DocumentType", DocumentType);
                HshIn.Add("RegistrationId", RegistrationId);
                HshIn.Add("EncounterId", EncounterId);
                HshIn.Add("intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDocumentPrintingLog", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        //Admission Form Discharge Summary
        public DataSet GetRegistrationNoFromIPNo(Hashtable hshInput)//
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select Id,RegistrationId,RegistrationNo from Encounter where EncounterNo =@EncounterNo and FacilityId=@FacilityId", hshInput);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }


        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sRemark,
                                  Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
                                  int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, int EntrySite)
        {
            return saveOrders(iHospitalLocationId, iFacilityId, iRegistrationId, iEncounterId, sStrXML, sRemark,
                                              iEncodedBy, iDoctorId, iCompanyId, sOrderType, cPayerType, sPatientOPIP,
                                              iInsuranceId, iCardId, dtOrderDate, sChargeCalculationRequired, EntrySite, 0, false);
        }

        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sRemark,
                                Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
                                int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, int EntrySite, int IsEmergency, bool isGenerateAdvance)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("dtsOrderDate", dtOrderDate);
            HshIn.Add("inyOrderType", sOrderType);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEnounterId", iEncounterId);
            HshIn.Add("xmlServices", sStrXML);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("chvRemarks", sRemark);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intCompanyId", iCompanyId);
            HshIn.Add("cPayerType", cPayerType);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", sPatientOPIP);
            HshIn.Add("chrChargeCalculationRequired", sChargeCalculationRequired);
            HshIn.Add("intEntrySite", EntrySite);
            HshIn.Add("iIsERorEMRServices", IsEmergency);
            HshIn.Add("bitIsGenerateAdvance", isGenerateAdvance);


            HshOut.Add("intOrderNo", SqlDbType.VarChar);
            HshOut.Add("intOrderId", SqlDbType.Int);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("intNEncounterID", SqlDbType.Int);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveOrder", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML,
                                string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType,
                                string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, string sXMLSurgeryDetail,
                                DateTime dtOrderDate, string sChargeCalculationRequired, bool IsMultiIncision, int EntrySite)
        {
            return saveOrders(iHospitalLocationId, iFacilityId, iRegistrationId, iEncounterId, sStrXML,
                                sRemark, iEncodedBy, iDoctorId, iCompanyId, sOrderType,
                                cPayerType, sPatientOPIP, iInsuranceId, iCardId, sXMLSurgeryDetail,
                                dtOrderDate, sChargeCalculationRequired, IsMultiIncision, EntrySite, 0, false);
        }

        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML,
                                string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType,
                                string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, string sXMLSurgeryDetail,
                                DateTime dtOrderDate, string sChargeCalculationRequired, bool IsMultiIncision, int EntrySite, int IsEmergency, bool isGenerateAdvance)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("dtsOrderDate", dtOrderDate);
            HshIn.Add("inyOrderType", sOrderType);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEnounterId", iEncounterId);
            HshIn.Add("xmlServices", sStrXML);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("chvRemarks", sRemark);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intCompanyId", iCompanyId);
            HshIn.Add("cPayerType", cPayerType);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", sPatientOPIP);
            HshIn.Add("chrChargeCalculationRequired", sChargeCalculationRequired);
            HshIn.Add("xmlSurgeryOrder", sXMLSurgeryDetail);
            HshIn.Add("bitIsMultiIncision", IsMultiIncision);
            HshIn.Add("intEntrySite", EntrySite);
            HshIn.Add("iIsERorEMRServices", IsEmergency);
            HshIn.Add("bitIsGenerateAdvance", isGenerateAdvance);

            HshOut.Add("intOrderId", SqlDbType.Int);
            HshOut.Add("intOrderNo", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("intNEncounterID", SqlDbType.Int);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveOrder", HshIn, HshOut);

                //return HshOut["chvErrorStatus"].ToString();
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML,
                             string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType,
                             string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, string sXMLSurgeryDetail,
                             DateTime dtOrderDate, string sChargeCalculationRequired, bool IsMultiIncision, string sXml)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("dtsOrderDate", dtOrderDate);
            HshIn.Add("inyOrderType", sOrderType);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("intEnounterId", iEncounterId);
            HshIn.Add("xmlServices", sStrXML);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("chvRemarks", sRemark);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intCompanyId", iCompanyId);
            HshIn.Add("cPayerType", cPayerType);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", sPatientOPIP);
            HshIn.Add("chrChargeCalculationRequired", sChargeCalculationRequired);
            HshIn.Add("xmlSurgeryOrder", sXMLSurgeryDetail);
            HshIn.Add("bitIsMultiIncision", IsMultiIncision);

            HshOut.Add("intOrderId", SqlDbType.Int);
            HshOut.Add("intOrderNo", SqlDbType.VarChar);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("intNEncounterID", SqlDbType.Int);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveOrder", HshIn, HshOut);

                //return HshOut["chvErrorStatus"].ToString();
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetRegistrationNoFromEMRPatientSummaryDetails(Hashtable hshInput)//
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select Enc.Id,Enc.RegistrationId,ESD.formatid from Encounter Enc inner join EMRPatientSummaryDetails ESD on Enc.id = ESD.Encounterid  where Enc.EncounterNo =@EncounterNo and Enc.FacilityId=@FacilityId", hshInput);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }

        public Hashtable getSingleServiceAmount(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
                Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge)
        {
            return getSingleServiceAmount(iHospitalLocationId, iFacilityId, iCompanyId, iInsuranceId, iCardId, cPatientOPIP, iServiceId,
                    iRegistrationId, iEncounterId, iDoctorId, intSpecialisationId, iEMergencyCharge, DateTime.Now);
        }

        public Hashtable getSingleServiceAmount(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
            Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge, DateTime OrderDate)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("intCompanyid", iCompanyId);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", cPatientOPIP);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("iEncounterId", iEncounterId);
            HshIn.Add("intServiceId", iServiceId);
            HshIn.Add("intDoctorId", iDoctorId);
            HshIn.Add("intBedCategoryId", 0);
            HshIn.Add("intSpecialisationId", intSpecialisationId);
            HshIn.Add("iEMergencyCharge", iEMergencyCharge);
            //HshIn.Add("OrderDate", OrderDate);

            HshOut.Add("NChr", SqlDbType.Money);
            HshOut.Add("DNchr", SqlDbType.Money);
            HshOut.Add("DiscountNAmt", SqlDbType.Money);
            HshOut.Add("DiscountDNAmt", SqlDbType.Money);
            HshOut.Add("PatientNPayable", SqlDbType.Money);
            HshOut.Add("PayorNPayable", SqlDbType.Money);
            HshOut.Add("DiscountPerc", SqlDbType.Money);
            HshOut.Add("IsApproval", SqlDbType.Bit);
            HshOut.Add("IsExcluded", SqlDbType.Bit);
            HshOut.Add("IsBlocked", SqlDbType.Bit);
            HshOut.Add("DepartmentId", SqlDbType.VarChar);
            HshOut.Add("DoctorRequired", SqlDbType.Bit);
            HshOut.Add("ServiceType", SqlDbType.VarChar);
            HshOut.Add("IsPriceEditable", SqlDbType.Bit);
            HshOut.Add("ChargeType", SqlDbType.VarChar);
            HshOut.Add("insCoPayPerc", SqlDbType.Money);
            HshOut.Add("insCoPayAmt", SqlDbType.Money);
            HshOut.Add("IsCoPayOnNet", SqlDbType.SmallInt);
            HshOut.Add("mnDeductibleAmt", SqlDbType.Money);
            HshOut.Add("chvServiceInstructions", SqlDbType.VarChar);
            HshOut.Add("isNonDiscService", SqlDbType.Bit);
            HshOut.Add("OutsourceInvestigation", SqlDbType.Bit);
            HshOut.Add("ServicelimitAmount", SqlDbType.Money);
            HshOut.Add("IsServiceApprovalRequired", SqlDbType.Bit);
            HshOut.Add("DefaultTariffFound", SqlDbType.VarChar);
            HshOut.Add("DefaultTariffId", SqlDbType.Int);
            HshOut.Add("IsPanelService", SqlDbType.Bit);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, HshOut);

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetBillDetails(Hashtable hshInput)//
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetBillDetails", hshInput).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
            }
        }


    }
}