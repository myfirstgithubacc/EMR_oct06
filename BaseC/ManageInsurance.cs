using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;

namespace BaseC
{

    public class ManageInsurance
    {

        DAL.DAL objDl;
        private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;//connectionstring
        Hashtable HshIn;
        public ManageInsurance()
        {

        }

        /// <summary>
        /// Method to get Company list 
        /// CompanyType - C Return All Companies
        /// CompanyType - I Return All Insurance
        /// CompanyType - IC Return All Insurance Companies
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="strCompanyType"></param>
        /// <param name="InsuranceId"></param>
        /// <returns>Dataset</returns>
        public DataSet GetCompanyList(int HospitalLocationId, string strCompanyType, int InsuranceId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospitalLocationId);
            HshIn.Add("chvCompanyType", strCompanyType);
            HshIn.Add("intInsuranceId", InsuranceId);
            HshIn.Add("intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCompanyType()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID, Name from CompanyType WITH (NOLOCK) WHERE Active = 1");
            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }


        }

        /// <summary>
        /// To Fill Card Type
        /// </summary>
        /// <param name="InsuranceCompanyId"></param>
        /// <returns>List of Card Type</returns>
        public DataSet fillCardtype(string InsuranceCompanyId, int FacilityId)
        {
            //intializing the Connection
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Adding parameters
            HshIn = new Hashtable();
            HshIn.Add("@insuranceComapnyId", InsuranceCompanyId);
            HshIn.Add("@intFacilityId", FacilityId);
            //getting values

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspSelectInsuranceCard", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        /// <summary>
        /// To Fill Invoice Batch No
        /// </summary>
        /// <param name="InsuranceCompanyId"></param>
        /// <returns>List of Card Type</returns>
        public DataSet fillInsuranceBatchNo(int iInsuranceCompanyId, int iSponsor, int iCardId)
        {
            //intializing the Connection
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Adding parameters
            HshIn = new Hashtable();
            HshIn.Add("@insuranceComapnyId", iInsuranceCompanyId);
            HshIn.Add("@iSponsor", iSponsor);
            HshIn.Add("@iCardId", iCardId);
            //getting values

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInsurancePendingBatchNo", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// To Fill Invoice by Batch No
        /// </summary>
        /// <param name="InsuranceCompanyId"></param>
        /// <returns>List of Card Type</returns>
        public DataSet GetUnsettledInvoiceDetails(int iHospitalLocation, int iLoginfacilityID, int iInvoiceID)
        {
            //intializing the Connection
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //Adding parameters
            HshIn = new Hashtable();
            HshIn.Add("@iHospitalLocationId", iHospitalLocation);
            HshIn.Add("@iLoginFacilityId", iLoginfacilityID);
            //HshIn.Add("@insuranceComapnyId", iInsuranceCompanyId);
            //HshIn.Add("@iSponsor", iSponsor);
            //HshIn.Add("@iCardId", iCardId);
            //HshIn.Add("@sBatchNo", sBatchNo);
            HshIn.Add("@invoiceid", iInvoiceID);

            //getting values

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "[uspGetServiceDetailsInvoiceWinseForRemittance]", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// For Insurance
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iLoginFacilityId"></param>
        /// <param name="iInsuranceCompanyId"></param>
        /// <param name="iSponsor"></param>
        /// <param name="iCardId"></param>
        /// <param name="sBatchNo"></param>
        /// <param name="sXML"></param>
        /// <returns></returns>

        public string updateGetElciamDenail(int HospitalLocationId, int FacilityId, int KeyId, string denialCode)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@remittanceID", KeyId.ToString()); //1
                HshIn.Add("@denialCode", denialCode.ToString());//2
                HshOut.Add("@denialString", SqlDbType.VarChar);//3
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDenialCode", HshIn, HshOut);
                return HshOut["@denialString"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getBillDispatch(int HospitalLocationId, int FacilityId, string OPIP, string DateSearchon, string Fromdate, string ToDate,
          int Sponsor, int Payer, int Cardid, string Searchon, string Searchvalue, int EncodedBy, string statusID, int IsPharmacyExclude)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId); //1
            HshIn.Add("@intFacilityId", FacilityId);//2
            HshIn.Add("@chrOPIP", OPIP);//3
            HshIn.Add("@intBilltypeSearchon", DateSearchon);//4
            HshIn.Add("@chrFromDate", Fromdate);//5
            HshIn.Add("@chrToDate", ToDate);//6
            HshIn.Add("@intPayorID", Sponsor);//7
            HshIn.Add("@intSponsorID", Payer);//8
            HshIn.Add("@intCardID", Cardid);//9
            HshIn.Add("@chrSearchon", Searchon);//10
            HshIn.Add("@chvSearchvalue", Searchvalue);//11
            HshIn.Add("@statusID", statusID);//12
            HshIn.Add("@intEncodedby", EncodedBy);//13
            HshIn.Add("@IsexPharmacy", IsPharmacyExclude);//14
            HshOut.Add("@chvErrorMessage", SqlDbType.VarChar);//15
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBillDispatch", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveBillDispatchPost(int iHospitalLocationId, int iFacilityId, string xmlData, int intEncodedBy, string Flag)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@xmlBillDispatch", xmlData);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@flag", Flag);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@DispatchNo", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBillDispatchPost", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveBillDispatch(int iHospitalLocationId, int iFacilityId, string xmlData, int intEncodedBy, string Flag, int dispatchadressid)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlBillDispatch", xmlData);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@flag", Flag);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@DispatchNo", SqlDbType.VarChar);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBillDispatch", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string getCompanyFromShortName(int hospitalLocationID, string CompanyCode)
        {
            HshIn = new Hashtable();

            HshIn.Add("@inyHospId", hospitalLocationID);
            HshIn.Add("@chvShortName", CompanyCode);

            DAL.DAL objobjDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                object obj = objDl.ExecuteScalar(CommandType.Text, "SELECT CompanyID FROM Company WITH (NOLOCK) Where ShortName=@chvShortName AND HospitalLocationID=@inyHospId", HshIn);
                return obj.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelBillDispatch(int iHospitalLocationId, string xmlData, int intEncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@xmlBillDispatch", xmlData);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPCancelBillDispatch", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        ///
        ///adfa//

        public DataSet GetUnSettledInvoiceByBatchNo(int iHospitalLocationId, int iLoginFacilityId, int iInsuranceCompanyId, int iSponsor, int iCardId, string sBatchNo, string sXML, string FileType)
        {
            //intializing the Connection
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {


                //Adding parameters
                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iLoginFacilityId", iLoginFacilityId);
                //HshIn.Add("@insuranceComapnyId", iInsuranceCompanyId);
                //HshIn.Add("@iSponsor", iSponsor);
                //HshIn.Add("@iCardId", iCardId);
                //HshIn.Add("@sBatchNo", sBatchNo);
                HshIn.Add("@xmlInvoiceId", sXML);
                string proc = "uspGetUnSettledInvoiceXML";
                if (FileType == "E")
                {
                    proc = "uspGetUnSettledInvoiceXLs";
                }
                else if (FileType == "X")
                {
                    proc = "uspGetUnSettledInvoiceXML";
                }
                //getting values
                return objDl.FillDataSet(CommandType.StoredProcedure, proc, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetInsuranceSetupList(int iComanyID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@companyId", iComanyID);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspInsuranceSetupList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetuspGetSpecialityVisitList(int iHospitalLocationId, int iFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("iFacilityId", iFacilityId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSpecialityVisitList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable uspSaveSpecialityVisitCycle(int iHospitalLocationId, int iFacilityId, int iSpecilityId, int iFreeFollowUpVisits, int iFreeFollowUpDaysLimit, int iPaidFollowUpVisits, int iPaidFollowUpDaysLimit, int iActive, int iEncodedBy, int iSpecialiltyVisitId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@iSpecilityId", iSpecilityId);
            HshIn.Add("@iFreeFollowUpVisits", iFreeFollowUpVisits);
            HshIn.Add("@iFreeFollowUpDaysLimit", iFreeFollowUpDaysLimit);
            HshIn.Add("@iPaidFollowUpVisits", iPaidFollowUpVisits);
            HshIn.Add("@iPaidFollowUpDaysLimit", iPaidFollowUpDaysLimit);
            HshIn.Add("@iActive", iActive);
            HshIn.Add("@iEncodedBy", iEncodedBy);
            HshIn.Add("@iSpecialiltyVisitId", iSpecialiltyVisitId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSpecialityVisitCycle", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int CancelSpecialityVisitCycle(int iHospitalLocationId, int iFacilityId, int iSpecialiltyVisitId, int iEncodedBy)
        {
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@iEncodedBy", iEncodedBy);
            HshIn.Add("@iSpecialiltyVisitId", iSpecialiltyVisitId);
            try
            {

                return objDl.ExecuteNonQuery(CommandType.Text, "UPDATE SpecialityVisitValiditySetup SET Active= 0, LastChangedBy = @iEncodedBy , LastChangedDate = GETUTCDATE() WHERE ID = @iSpecialiltyVisitId  AND HospitalLocationId = @iHospitalLocationId AND FacilityId = @iFacilityId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetInsuranceCategory(int iHospitalLocationId, int iFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("intFacilityId", iFacilityId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInsuranceCategoryDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveInsuranceDetail(int HospitalLocationId, int FacilityID, int RegistrationId
            , int PayerId, int SponsorId, int CardId, string PolicyNo, DateTime dtValidFrom, DateTime dtValidUpto
            , string SOAPNo, string Remarks, double CreditLimit, string CoverageOPIPBOTH,
            decimal OPPharmacyCoPayPercent, decimal IPPharmacyCoPayPercent, string xmlRegistrationOtherDetails, int EncodedBy,
            int CoPayMaxLimit, int AdmissionApprovalRequired, int intSubCompanyId, string MemberId, string OPIP,
            string CardNo, double PharmacyCopayMaxLimit, double opCreditLimit, double pharmacyCreditLimit, int TopupCard)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityID", FacilityID);
            HshIn.Add("intRegistrationNo", RegistrationId);
            HshIn.Add("intSubCompanyId", intSubCompanyId);
            HshIn.Add("intPayerId", PayerId);
            HshIn.Add("intSponsorId", SponsorId);
            HshIn.Add("intCardId", CardId);
            HshIn.Add("chrPolicyNo", PolicyNo);
            HshIn.Add("dtValidFrom", dtValidFrom);
            HshIn.Add("dtValidUpto", dtValidUpto);
            HshIn.Add("chrSOAPNo", SOAPNo);
            HshIn.Add("chrRemarks", Remarks);
            HshIn.Add("CreditLimit", CreditLimit);
            HshIn.Add("CoPayMaxLimit", CoPayMaxLimit);
            HshIn.Add("AdmissionApprovalRequired", AdmissionApprovalRequired);
            HshIn.Add("chrCoverageOPIPBOTH", CoverageOPIPBOTH);
            HshIn.Add("OPPharmacyCoPayPercent", OPPharmacyCoPayPercent);
            HshIn.Add("IPPharmacyCoPayPercent", IPPharmacyCoPayPercent);
            HshIn.Add("xmlRegistrationOtherDetails", xmlRegistrationOtherDetails);
            HshIn.Add("intEncodedBy", EncodedBy);
            HshIn.Add("chrMemberId", MemberId);
            HshIn.Add("OPIP", OPIP);
            HshIn.Add("CardNo", CardNo);
            HshIn.Add("PharmacyCopayMaxLimit", PharmacyCopayMaxLimit);
            HshIn.Add("opCreditLimit", opCreditLimit);
            HshIn.Add("PharmacyCreditLimit", pharmacyCreditLimit);
            HshIn.Add("TopupCard", TopupCard);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientInsuranceDetails", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet FindPatientusingMobileNo(int HospitalLocationId, int FacilityId, string mobileno, int RegistrationNo)
        {

            HshIn = new Hashtable();
            HshIn.Add("intHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityID", FacilityId);
            HshIn.Add("bIntmobileno", mobileno);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientFromMobileNo", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet FillPreviousDetails(int HospitalLocationId, int FacilityId, int RegistrationNo)
        {
            HshIn = new Hashtable();
            HshIn.Add("intHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityID", FacilityId);
            HshIn.Add("intRegistrationNo", RegistrationNo);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousInsuranceDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet FillPreviousPayerDetails(int HospitalLocationId, int FacilityId, int intRegistrationInsuranceId)
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityID", FacilityId);
            HshIn.Add("intRegistrationInsuranceId", intRegistrationInsuranceId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousPayerCoInsurance", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getInsuranceDetails(int HospitalLocationID, int intComapnyId, int intInsuranceCardId, int intInsuranceId)
        {
            //Adding parameters
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospitalLocationID);
            HshIn.Add("@intComapnyId", intComapnyId);
            HshIn.Add("@intInsuranceCardId", intInsuranceCardId);
            HshIn.Add("@intInsuranceId", intInsuranceId);
            //getting values
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyCoInsurance", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet FillResubmissionType(int HospitalLocationID, int FacilityID)
        {
            HshIn = new Hashtable();
            HshIn.Add("@HospitalLocationID", HospitalLocationID);
            HshIn.Add("@FacilityID", FacilityID);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEclaimResubmissionType", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveAttachment(int HospitalLocationID, int FacilityID, int InvoiceID, string filename, byte[] imagedata)
        {
            //Adding parameters
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@HospitalLocationID", HospitalLocationID);
            HshIn.Add("@FacilityID", FacilityID);
            HshIn.Add("@InvoiceID", InvoiceID);
            HshIn.Add("@filename", filename);
            HshIn.Add("@imagedata", SqlDbType.Image);
            HshIn.Add("@imagedata", imagedata);
            HshOut.Add("@Error", SqlDbType.VarChar);

            //getting values
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, " ", HshIn, HshOut);
                return HshOut["Error"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
