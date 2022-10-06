using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;


/// <summary>
/// Summary description for EMRBilling
/// </summary>
namespace BaseC
{
    public class EMRBilling
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable HshOut;
        DAL.DAL Dl;
        StringBuilder StrQuery;


        public EMRBilling(string Constring)
        {
            sConString = Constring;
            Dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }
        public DataSet getcompanyChecklist(int companyId, string ModuleType)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("companyId", companyId);
            HshIn.Add("ModuleType", ModuleType);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGETChecklistCompany", HshIn);
            return ds;
        }
        public string FormatNumber(int HospitalLocationId, object myValue, int FacilityId)
        {
            BaseC.HospitalSetup objHospital = new HospitalSetup(sConString);
            Int32 decimalplaces = Convert.ToInt32(objHospital.GetFlagValueHospitalSetup(HospitalLocationId, "DecimalPlaces", FacilityId));
            NumberFormatInfo myNumberFormat = new CultureInfo("").NumberFormat;
            myNumberFormat.NumberDecimalDigits = decimalplaces;
            myNumberFormat.NumberDecimalSeparator = ".";
            return (Convert.ToDecimal(myValue)).ToString("N", myNumberFormat);
        }
        public DataSet FillStatusMaster()
        {
            HshIn = new Hashtable();
            HshIn.Add("@Encounter", "Encounter");
            HshIn.Add("@active", "1");

            DataSet ds = Dl.FillDataSet(CommandType.StoredProcedure, "FillstatusMaster", HshIn);
            return ds;
        }
        public DataSet GetInvoice(int HospitalId, int FacilityId, int EncounterId)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounter", EncounterId);

            ds = Dl.FillDataSet(CommandType.Text, "Select invoiceid FROM Invoice WITH (NOLOCK) where EncounterId=@intEncounter and Active=1 and FacilityId=@intFacilityId and HospitalLocationId =@inyHospitalLocationId ", HshIn);

            return ds;
        }

        public DataSet GetPatientLastVisit(int HospitalId, int FacilityId, int RegistrationId)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();

            HshIn.Add("@iHospitalLocationId", HospitalId);
            HshIn.Add("@iFacilityId", FacilityId);
            HshIn.Add("@iRegistrationId", RegistrationId);

            ds = Dl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientLastVisit", HshIn);

            return ds;
        }
        public DataSet GetIntimationStatus(int HospitallocationID, int FacilityID, int Squno)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@Facilityid", FacilityID);
            HshIn.Add("@HospLocID", HospitallocationID);
            HshIn.Add("@statusId", Squno);

            DataSet DsIntimation = Dl.FillDataSet(CommandType.StoredProcedure, "USPPatientIntimationStatus", HshIn);
            return DsIntimation;
        }
        public DataSet GetIntimationStatusChanging(int seq, string module)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@Squ", seq);
            HshIn.Add("@module", module);
            DataSet dsDset = Dl.FillDataSet(CommandType.StoredProcedure, "USPGetIntimationOperation", HshIn);
            return dsDset;
        }

        public Hashtable ChangeSaveIntimationStatus(string Activity, int hosplocid, int Faclid, int EncounterID, int statusID, string remarks, int active, int userid, int id)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@chrActivity", Activity);
            HshIn.Add("@HospLocID", hosplocid);
            HshIn.Add("@FacilityID", Faclid);
            HshIn.Add("@EncounterID", EncounterID);
            HshIn.Add("@StatusID", statusID);
            HshIn.Add("@Remarks", remarks);
            HshIn.Add("@Active", active);
            HshIn.Add("@USerCode", userid);
            HshIn.Add("@ID", id);

            HshOut.Add("@Error", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveUpdatePatientStatusActivity", HshIn, HshOut);
            return HshOut;//HshOut["chvErrorStatus"].ToString();
        }
        public DataSet GetPendingOrderForBill(int iRegID, int iHospitalID)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            StrQuery = new StringBuilder();

            HshIn.Add("@RegId", iRegID);
            HshIn.Add("@HospitalLocationId", iHospitalID);

            StrQuery.Append("SELECT DISTINCT ENC.encounterNo AS EncounterNo, ENC.Id AS EncounterID, OSD.ID AS orderId, ");
            StrQuery.Append(" OSD.ID AS Orderno, OSD.OrderDate, OSD.ServiceId, IOS.ServiceName, dbo.GetDoctorName(OSD.DoctorId) AS DoctorName, ");
            StrQuery.Append(" CASE WHEN OSD.BillID > 0 THEN 1 ELSE 0 END AS ISBillable ");
            StrQuery.Append(" FROM Encounter ENC WITH (NOLOCK) ");
            StrQuery.Append(" INNER JOIN OPServiceOrderDetail OSD WITH (NOLOCK) ON OSD.EncounterId = ENC.Id ");
            StrQuery.Append(" INNER JOIN ItemOfService IOS WITH (NOLOCK) ON IOS.ServiceId=OSD.ServiceId ");
            StrQuery.Append(" INNER JOIN Employee EMP WITH (NOLOCK) ON EMP.ID=OSD.DoctorId ");
            StrQuery.Append(" WHERE ENC.RegistrationId = @RegId AND ENC.hospitallocationid = @HospitalLocationId ");
            StrQuery.Append(" AND ENC.active = 1 ");

            DataSet DsPendingOrder = Dl.FillDataSet(CommandType.Text, StrQuery.ToString(), HshIn);
            return DsPendingOrder;
        }

        public DataSet getInvUnacknowledgeService(int iHospID, int iLoginFacilityId, int EncounterId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@EncounterId", EncounterId);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPUnperformedServices", HshIn);


            return ds;
        }
        /// <summary>
        /// Get Patient Cancelled Service
        /// </summary>
        /// <param name="iHospID"></param>
        /// <param name="iLoginFacilityId"></param>
        /// <param name="EncounterId"></param>
        /// <returns></returns>
        public DataSet GetIPPatientCancelledServices(int iHospID, int iLoginFacilityId, int EncounterId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@iEncounterId", EncounterId);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIPPatientCancelledServices", HshIn);
            return ds;
        }

        public DataSet GetUSPEntitlementCategoryInvoiceDetail(int Encounterid, int HospLocationId,
            int FacilityId, int ToCategory, DateTime FromDate, DateTime ToDate, int ChangeType)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospLocationId", HospLocationId);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@ToCategory", ToCategory);
            HshIn.Add("@IntEncounterId", Encounterid);
            HshIn.Add("@FromDate", FromDate.ToString("yyyy-MM-dd hh:mm"));
            HshIn.Add("@ToDate", ToDate.ToString("yyyy-MM-dd hh:mm"));
            HshIn.Add("@ChangeType", ChangeType);
            HshOut.Add("@ErrorMessage", SqlDbType.VarChar);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPEntitlementCategoryInvoiceDetail", HshIn, HshOut);
            return ds;

        }

        public DataSet GetBedCategory(int FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@iFacilityId", FacilityId);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspSelectBedCategory", HshIn);
            return ds;
        }
        public DataSet GetWardBedCategoryWiseBedNo(int HospLocationId, int FacilityId, int iWardId, int iBedCategoryId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intWardId", iWardId);
            HshIn.Add("@intBedCategoryId", iBedCategoryId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetWardBedCategoryWiseBedNo", HshIn);
            return ds;
        }
        public DataSet GetPatientBedTransfer(int FacilityId, int RegistrationId, int EncounterID)
        {
            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientTransfer", HshIn);
            return ds;
        }

        public string SaveCompanyTariff(int HospId, int CompanyId, int FacilityId, string TaggedPriceValidId, int EncodedBy, int CardId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@inyHospitallocationId", HospId);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvTaggedPriceValidId", TaggedPriceValidId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intCardId", CardId);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCompanyTariff", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getEffectiveTariff(int FacilityId, int HospitalLocationId)
        {
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@HospitallocationId", HospitalLocationId);

                string qry = " SELECT cped.Id as ValidId, C.Name, dbo.GetDateFormat(cped.EffectiveDate, 'D') AS ValidFrom ,ic.CardType,ic.Cardid,c.companyId " +
                            " FROM Company c WITH (NOLOCK) " +
                            " INNER JOIN CompanyPriceEffectiveDate cped WITH (NOLOCK) ON c.CompanyId = cped.CompanyID " +
                            " LEFT JOIN InsuranceCards ic WITH (NOLOCK) ON ic.InsuranceId = c.CompanyID AND ic.CardId=cped.CardId" +
                            " WHERE cped.FacilityId = @FacilityId " +
                            " AND cped.HospitalLocationId = @HospitallocationId " +
                            " ORDER BY c.Name, cped.EffectiveDate ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public DataSet getEffectiveTariffTagged(int CompanyId, int FacilityId, int HospId, int CompanyCardId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@inyHospitalLocation", HospId);
            HshIn.Add("@inyFaclilityId", FacilityId);
            HshIn.Add("@intCompanyCardId", CompanyCardId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetFillTagTariffList", HshIn);

            return ds;
        }
        public DataSet getChangedServiceRates(string XMLstring, Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, Int32 iRegistrationId, Int32 iEncounterId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("XMLstring", Convert.ToString(XMLstring));
            HshIn.Add("inyHospitalLocationID", Convert.ToInt32(iHospitalLocationId));
            HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
            HshIn.Add("intCompanyid", iCompanyId);
            HshIn.Add("iInsuranceId", iInsuranceId);
            HshIn.Add("iCardId", iCardId);
            HshIn.Add("cPatientOPIP", cPatientOPIP);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("iEncounterId", iEncounterId);



            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOpChangedServiceRate", HshIn);

            return ds;
        }
        public string CloseEncounter(int EncounterId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intEncounterId", EncounterId);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCloseEncounter", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();

        }

        public DataSet getPatientTransefers(int hospitalLocationId, int facilityId, int EncounterID, string procedure, int type)
        {
            Hashtable hsIn = new Hashtable();

            hsIn.Add("@inyHospitalLocationId", hospitalLocationId);
            hsIn.Add("@intLoginFacilityId", facilityId);
            hsIn.Add("@EncounterId", EncounterID);
            hsIn.Add("@type", type);

            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, procedure, hsIn);
            return ds;
        }
        public DataSet getPreAutCreditlimit(int HospId, int facilityId, int RegistrationId, string dtFromDate, string dtToDate
          , int StatusId, int doctorId, string PatientName, string Source)
        {

            Hashtable HshIn = new Hashtable();
            HshIn.Add("intHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", facilityId);
            HshIn.Add("intRegistrationNo", RegistrationId);
            HshIn.Add("dtFromDate", dtFromDate);
            HshIn.Add("dtToDate", dtToDate);
            HshIn.Add("intStatusId", StatusId);
            HshIn.Add("intdoctorId", doctorId);
            HshIn.Add("chvPatientName", PatientName);
            HshIn.Add("Source", Source);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetPreAutCreditlimit", HshIn);
            return ds;
        }
        public DataSet ShowOPUnbilledServiceOrders(int HospId, int facilityId, string dtFromDate, string dtToDate)
        {

            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospId);
            HshIn.Add("intFacilityId", facilityId);
            HshIn.Add("chvFromDate", dtFromDate);
            HshIn.Add("chvToDate", dtToDate);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspShowOPUnbilledServiceOrders", HshIn);
            return ds;
        }
        public DataSet GetUnVerifiedInvoice(int HospId, int facilityId, string dtFromDate, string dtToDate, int bitVerified)
        {

            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospId);
            HshIn.Add("intFacilityId", facilityId);
            HshIn.Add("chvFromDate", dtFromDate);
            HshIn.Add("chvToDate", dtToDate);
            HshIn.Add("bitVerified", bitVerified);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUnVerifiedInvoice", HshIn);
            return ds;
        }
        public string tagPreAuthPatient(int PreAuthId, int TagRegistrationId, int TagEncounterId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("intPreAuthId", PreAuthId);
            HshIn.Add("intTagRegistrationId", TagRegistrationId);
            HshIn.Add("intTagEncounterId", TagEncounterId);
            HshIn.Add("intEncodedBy", EncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspTagPreAuthPatient", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();
        }
        public DataSet GetAuditInvoice(int HospId, int facilityId, string dtFromDate, string dtToDate, string type)
        {

            Hashtable HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", HospId);
            HshIn.Add("intFacilityId", facilityId);
            HshIn.Add("chvFromDate", dtFromDate);
            HshIn.Add("chvToDate", dtToDate);
            HshIn.Add("type", type);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetInvoiceForAudit", HshIn);
            return ds;
        }
        public class clsExclusion
        {
            private string sConString = "";
            public clsExclusion(string conString)
            {
                sConString = conString;
            }
            Hashtable HshIn;
            Hashtable HshOut;
            public int UserID = 0;
            public int facilityId = 0;
            public int CompanyID = 0;
            public int InsuranceID = 0;
            public int CardID = 0;
            public int hospitallocationid;
            public string xmlExcludedDetails = "";
            public string SProc = "";

            /// <summary>
            /// Used To Get Exclusions of Hospital
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iCompanyId"></param>
            /// <param name="iDepartmentId"></param>
            /// <param name="iSubDepartmentId"></param>
            /// <param name="iShowExcluded"></param>
            /// <param name="Search"></param>
            /// <param name="SProc"></param>
            /// <returns>Dataset</returns>
            public DataSet getData(int iHospitalLocationId, string sExclusionType, int iCompanyId, int iInsuranceId, int iCardId, int iDepartmentId,
                int iSubDepartmentId, int iShowExcluded, int iInsuranceCategoryId, string Search, string SProc, int FacilityId)
            {
                HshIn = new Hashtable();

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intFacilityid", FacilityId);
                HshIn.Add("chrExclusionType", sExclusionType);
                HshIn.Add("intCompanyId", iCompanyId);
                if (iInsuranceCategoryId > 0)
                    HshIn.Add("@InsuranceCategoryId", iInsuranceCategoryId);
                if (iInsuranceId > 0)
                    HshIn.Add("intInsuranceId", iInsuranceId);
                if (iCardId > 0)
                    HshIn.Add("intCardId", iCardId);
                if (iDepartmentId > 0)
                    HshIn.Add("intDepartmentId", iDepartmentId);
                if (iSubDepartmentId > 0)
                    HshIn.Add("intSubDepartmentId", iSubDepartmentId);
                HshIn.Add("bitShowExcluded", iShowExcluded);
                if (Search != "")
                {
                    HshIn.Add("chvSearch", Search);
                }
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }
            /// <summary>
            /// Used To Save And Update Exclusions
            /// </summary>
            /// <param name="objServiceExclusion"></param>
            /// <returns>Error/Success Message</returns>
            public string SaveData(clsExclusion objExclusion)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", objExclusion.hospitallocationid);
                HshIn.Add("@intFacilityId", objExclusion.facilityId);
                if (objExclusion.CompanyID != 0)
                {
                    HshIn.Add("intCompanyId", objExclusion.CompanyID);
                }
                if (objExclusion.InsuranceID != 0)
                {
                    HshIn.Add("intInsuranceId", objExclusion.InsuranceID);
                }
                if (objExclusion.CardID != 0)
                {
                    HshIn.Add("intCardId", objExclusion.CardID);
                }

                HshIn.Add("xmlExcludedDetails", objExclusion.xmlExcludedDetails);
                HshIn.Add("intEncodedBy", objExclusion.UserID);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objExclusion.SProc, HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
        }
        public class ClsCopayment
        {
            private string sConString = "";
            public ClsCopayment(string conString)
            {
                sConString = conString;
            }
            public String SaveCopayment(Int32 HospLocid, Int32 FacilID, Int32 EncounterID, Int32 RegID, Double DblCopay, Int32 intUcode)
            {
                Hashtable HshIn;
                Hashtable HshOut;
                string MessageString = "";
                try
                {
                    HshIn = new Hashtable();
                    HshOut = new Hashtable();

                    HshIn.Add("inyHospitalLocationId", HospLocid);

                    HshIn.Add("intEncid", EncounterID);

                    HshIn.Add("inyFacid", FacilID);
                    HshIn.Add("intRegid", RegID);

                    HshIn.Add("ucode", intUcode);
                    HshIn.Add("decCopay", DblCopay);
                    HshOut.Add("varRetMessage", SqlDbType.VarChar);
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveCopayment", HshIn, HshOut);
                    return HshOut["varRetMessage"].ToString();
                }
                catch (Exception ex)
                {
                    MessageString = "E-" + ex.Message.ToString();
                }
                return MessageString;

            }
        }
        public class ClsSupplementary
        {
            private string sConString = "";
            public ClsSupplementary(string conString)
            {
                sConString = conString;
            }
            public string SaveasSupplementary(string Xml, string UpdType, int EncodedBy)
            {
                Hashtable HshIn;
                Hashtable HshOut;
                string MessageString = "";
                try
                {
                    HshIn = new Hashtable();
                    HshOut = new Hashtable();
                    HshIn.Add("intEncodedBy", EncodedBy);
                    HshIn.Add("xmlServices", Xml);
                    HshIn.Add("chrUpdateType", UpdType);
                    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspMarkedUnmarkedSupplementary", HshIn, HshOut);
                    return HshOut["chvErrorStatus"].ToString();
                }
                catch (Exception ex)
                {
                    MessageString = "ERROR -" + ex.Message.ToString();
                }
                return MessageString;
            }
        }
        public class clsRefund
        {
            private string sConString = "";
            public clsRefund(string conString)
            {
                sConString = conString;
            }
            Hashtable HshIn;
            Hashtable HshOut;
            public int UserID = 0;
            public int PageID = 0;
            public int HospitalLocationId = 0;
            public int FacilityId = 0;
            public int RegistrationId = 0;
            public int RefundType = 0;
            public int InvoiceId = 0;
            public int ReceiptId = 0;
            public int ComapnyId = 0;
            public int RefundNo = 0;
            public int YearId = 0;
            public string Remarks = "";
            public int ReRequested = 0;
            public double Amount;
            public int DocId = 0;
            public string TranType = "";
            public int StatusId = 0;
            public int Refunded = 0;
            public string xmlPaymentDetail = "";
            public string xmlService = "";
            public string SProc = "";
            public int EntrySite = 0;
            public string IPAddress = "";

            public DataSet getPatientInBilling(int iHospitalLocationId, int iFacilityId, string strKeyWord)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", iFacilityId);
                HshIn.Add("@chvKeyword", strKeyWord);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspSearchBillingPatient", HshIn);
                return ds;
            }

            /// <summary>
            /// Used To get Refund Details (Excluding Cancelled Refund)
            /// </summary>
            /// <param name="iYearID"></param>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iRefundNo"></param>
            /// <param name="SProc"></param>
            /// <returns></returns>
            public DataSet getRefundDetail(int iYearID, int iHospitalLocationId, int iFacilityId, string sRefundNo, string SProc)
            {
                HshIn = new Hashtable();

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                HshIn.Add("chvRefundNo", sRefundNo);
                HshIn.Add("intYearID", iYearID);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }

            /// <summary>
            /// Used To Get Refund Details Against Advance
            /// </summary>
            /// <param name="iHospitalLocationId"> Hospital Location ID</param>
            /// <param name="iReceiptNo">Reciept Number</param>           
            /// <param name="SProc"> Procedure Name</param>
            /// <returns>Dataset</returns>
            public DataSet getAgvanceDetail(int iType, int iHospitalLocationId, int iFacilityId, string sReceiptNo,
                string sFromDate, string sToDate, int iYearID, string SProc)
            {
                HshIn = new Hashtable();

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                if (sReceiptNo != "")
                    HshIn.Add("chvReceiptNo", sReceiptNo);
                HshIn.Add("bitType", iType);
                HshIn.Add("intYearID", iYearID);
                if ((sFromDate != "") && (sToDate != ""))
                {
                    HshIn.Add("chrFromDate", sFromDate);
                    HshIn.Add("chrToDate", sToDate);
                }
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }
            /// <summary>
            /// Used to get Payment Mode Details with amount
            /// </summary>
            /// /// <param name="iHospitalLocationId"></param>
            /// <param name="iType"></param>
            /// <param name="iNo"></param>
            /// <param name="SProc"></param>
            /// <returns></returns>
            public DataSet getPaymentModeDetail(int iHospitalLocationId, int iFacilityId, int iYearId, int iType, string sReceiptIds, string sRefundNo, string SProc)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                HshIn.Add("intYearId", iYearId);
                HshIn.Add("bitType", iType);
                HshIn.Add("chvReceiptIds", sReceiptIds);
                HshIn.Add("chvRefundNo", sRefundNo);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }
            /// <summary>
            /// Used To get Patient all receipt details
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iYearId"></param>
            /// <param name="iRegistrationId"></param>
            /// <param name="SProc"></param>
            /// <returns></returns>
            public DataSet getPatientReceipt(int iHospitalLocationId, int iFacilityId, int iYearId, int iRegistrationId, string SProc, string strDocId, string strFdate, string strTdate, string sStrDocNo)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                HshIn.Add("intYearId", iYearId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("dtFromDate", strFdate);
                HshIn.Add("dtToDate", strTdate);
                HshIn.Add("iDocumentId", strDocId);
                HshIn.Add("sDocumentNo", sStrDocNo);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }

            /// <summary>
            /// Used to get Previous Refund Detail
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iType"></param>
            /// <param name="iNo"></param>
            /// <param name="SProc"></param>
            /// <returns></returns>
            public DataSet getPreviousRefundDetail(int iHospitalLocationId, int iFacilityId, int iYearId, int iType, string sReceiptIds, string sRefundNo, int iInvoiceId, string SProc)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                HshIn.Add("intYearId", iYearId);
                HshIn.Add("intType", iType);
                HshIn.Add("chvReceiptIds", sReceiptIds);
                HshIn.Add("chvRefundNo", sRefundNo);
                HshIn.Add("intInvoiceId", iInvoiceId);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }
            /// <summary>
            /// Used To get All Invoice Setteled against given Receipt Ids
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iYearId"></param>
            /// <param name="iType"></param>
            /// <param name="sReceiptIds"></param>
            /// <param name="iRefundNo"></param>
            /// <param name="SProc"></param>
            /// <returns></returns>
            public DataSet getInvoiceSettlementDetail(int iHospitalLocationId, int iFacilityId, int iYearId, int iType, string sReceiptIds, string sRefundNo, int iInvoiceId, string SProc)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                HshIn.Add("intYearId", iYearId);
                HshIn.Add("intType", iType);
                HshIn.Add("chvReceiptIds", sReceiptIds);
                HshIn.Add("chvRefundNo", sRefundNo);
                HshIn.Add("intInvoiceId", iInvoiceId);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }
            /// <summary>
            /// Used To Get Refund Mode Details
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iYearId"></param>
            /// <param name="iRefundNo"></param>
            /// <param name="SProc"></param>
            /// <returns>Dataset</returns>
            public DataSet getRefundModeDetail(int iHospitalLocationId, int iFacilityId, int iYearId, string sRefundNo, string SProc)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intLoginFacilityId", iFacilityId);
                HshIn.Add("intYearId", iYearId);
                HshIn.Add("chvRefundNo", sRefundNo);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }
            /// <summary>
            /// Used To get Invoice Service Details
            /// </summary>
            /// <param name="iInvoiceId"></param>
            /// <param name="SProc"></param>
            /// <returns></returns>
            public DataSet getInvoiceServiceDetails(int iHospitalLocationId, int iInvoiceId, string SProc, int FacilityId)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intInvoiceId", iInvoiceId);
                HshIn.Add("intFacilityId", FacilityId);
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }

            public Hashtable SaveData(clsRefund objRefund)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("inyHospitalLocationId", objRefund.HospitalLocationId);
                HshIn.Add("intLoginFacilityId", objRefund.FacilityId);
                HshIn.Add("intRegistrationId", objRefund.RegistrationId);
                HshIn.Add("intRefundType", objRefund.RefundType);
                if (objRefund.InvoiceId != 0)
                {
                    HshIn.Add("intInvoiceId", objRefund.InvoiceId);
                }
                if (objRefund.ReceiptId != 0)
                {
                    HshIn.Add("intReceiptId", objRefund.ReceiptId);
                }
                HshIn.Add("chvRemarks", objRefund.Remarks);
                HshIn.Add("xmlPaymentDetail", objRefund.xmlPaymentDetail);
                HshIn.Add("xmlServiecDetail", objRefund.xmlService);
                HshIn.Add("intEncodedBy", objRefund.UserID);
                HshIn.Add("intPageId", objRefund.PageID);
                HshIn.Add("intSponsorId", objRefund.ComapnyId);
                HshIn.Add("intEntrySite", objRefund.EntrySite);
                HshOut.Add("chvRefundNo", SqlDbType.VarChar);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshIn.Add("ip", objRefund.IPAddress);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objRefund.SProc, HshIn, HshOut);
                return HshOut;
            }
            public string CancelRefund(clsRefund objRefund)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("inyHospitalLocationId", objRefund.HospitalLocationId);
                HshIn.Add("intLoginFacilityId", objRefund.FacilityId);
                HshIn.Add("intYearId", objRefund.YearId);
                HshIn.Add("intRefundType", objRefund.RefundType);
                HshIn.Add("chvRefundNo", objRefund.RefundNo);
                HshIn.Add("chvRemarks", objRefund.Remarks);
                HshIn.Add("xmlPaymentDetail", objRefund.xmlPaymentDetail);
                HshIn.Add("xmlServiecDetail", objRefund.xmlService);
                HshIn.Add("intEncodedBy", objRefund.UserID);
                HshIn.Add("intPageId", objRefund.PageID);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objRefund.SProc, HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }

            public string SaveRefundApproval(clsRefund objRefund)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("inyHospitalLocationId", objRefund.HospitalLocationId);
                if (objRefund.DocId != 0)
                {
                    HshIn.Add("intDocId", objRefund.DocId);
                }
                HshIn.Add("intReceiptId", objRefund.ReceiptId);
                HshIn.Add("mnyAmount", objRefund.Amount);
                HshIn.Add("chrTranType", objRefund.TranType);
                HshIn.Add("chvRefundType", objRefund.RefundType);
                HshIn.Add("intStatusId", objRefund.StatusId);
                HshIn.Add("intReRequest", objRefund.ReRequested);
                HshIn.Add("intRefunded", objRefund.Refunded);
                HshIn.Add("chvRemarks", objRefund.Remarks);
                HshIn.Add("intEncodedBy", objRefund.UserID);
                HshIn.Add("intPageId", objRefund.PageID);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, objRefund.SProc, HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            public DataSet getRefundDump(int HospId, int FacilityId, Int64 RegistrationNo, string DateFrom, string DateTo,
                                        string PatientName, int ApprovalStatusId, string SProc)
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@chvDateFrom", DateFrom);
                HshIn.Add("@chvDateTo", DateTo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@intApprovalStatusId", ApprovalStatusId);

                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, SProc, HshIn);
                return ds;
            }

            public string SaveRefundApprovalDump(int HospId, int FacilityId, string xmlDumpRefunds, int EncodedBy, string spName)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@xmlDumpRefunds", xmlDumpRefunds);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, spName, HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }

            public Hashtable SaveRefundApprovalAck(int DumpRefundId, int HospId, int FacilityId, int RegistrationId, int EncodedBy,
                                         string spName, out string RefundNo)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                RefundNo = "";

                HshIn.Add("@intDumpRefundId", DumpRefundId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvRefundNo", SqlDbType.VarChar);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("@chvRefundId", SqlDbType.VarChar);


                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, spName, HshIn, HshOut);

                RefundNo = HshOut["@chvRefundNo"].ToString();

                return HshOut;
                //return HshOut["@chvErrorStatus"].ToString();
            }

        }

        public class clsDebitCreditNote
        {
            private string sConString = "";
            public clsDebitCreditNote(string conString)
            {
                sConString = conString;
            }
            Hashtable HshIn;
            Hashtable HshOut;
            public int UserID = 0;
            public int HospitalLocationId = 0;
            public int RegistrationId = 0;
            public int InvoiceId = 0;
            public string Remarks = "";
            public string xmlDebitCreditNoteDetail = "";
            public string SProc = "";
        }

        /// <summary>
        /// For form Order & Bill and their Service 
        /// </summary>
        public class clsOrderNBill
        {
            private string sConString = "";
            public clsOrderNBill(string conString)
            {
                sConString = conString;
            }
            Hashtable HshIn;
            Hashtable HshOut;

            public Hashtable GetPatientEncounterStatus(Int32 ihospLID, Int32 iEncounterID, Int32 ifaciltiyid, string sStatusCode)
            {
                Hashtable HshIn;
                Hashtable HshOut;
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@iHospitalLId", ihospLID);
                HshIn.Add("@iEncid", iEncounterID);
                HshIn.Add("@ifacId", ifaciltiyid);
                HshIn.Add("@sStatus", sStatusCode);

                HshOut.Add("@vpStatus", SqlDbType.VarChar);
                HshOut.Add("@vpSCode", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetPatientEncounterStatus", HshIn, HshOut);

                return HshOut;
            }
            public Hashtable saveIPPackage(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, Int32 ipackageCode, Int32 iEncodedBy, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, decimal dServiceAmount)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("HospLocationID", Convert.ToInt32(iHospitalLocationId));
                HshIn.Add("FacilityCode", Convert.ToInt32(iFacilityId));
                HshIn.Add("OrderType", sOrderType);
                HshIn.Add("registrationID", iRegistrationId);
                HshIn.Add("IntEncno", iEncounterId);
                HshIn.Add("intPackageCode", ipackageCode);
                HshIn.Add("UserCode", iEncodedBy);
                HshIn.Add("intCompanyId", iCompanyId);
                HshIn.Add("cPayerType", cPayerType);
                HshIn.Add("ServiceAmount", dServiceAmount);
                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("intNEncounterID", SqlDbType.Int);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveIPDPackage", HshIn, HshOut);
                //USPSaveIPDPackage
                //return HshOut["chvErrorStatus"].ToString();
                return HshOut;
            }
            public Hashtable ChangeCategory(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iEncounterID, Int32 iEncodedBy, Int32 iFromCategory, Int32 iTocategory, Int32 iTransfeType, Int32 iTransferID, string sErrorMessage, DateTime dFromDate, DateTime dTodate)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("IntEncounterId", iEncounterID);
                HshIn.Add("HospLocationId", iHospitalLocationId);
                HshIn.Add("FacilityId", iFacilityId);
                HshIn.Add("FromCategory", iFromCategory);
                HshIn.Add("ToCategory", iTocategory);
                HshIn.Add("ChangeType", iTransfeType);
                HshIn.Add("FromDate", dFromDate);
                HshIn.Add("ToDate", dTodate);
                HshIn.Add("intUSerCode", iEncodedBy);


                HshOut.Add("TransferID", SqlDbType.Int);
                HshOut.Add("ErrorMessage", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPChangedCategory", HshIn, HshOut);

                CalculateRoomRent(iEncounterID, iHospitalLocationId, iFacilityId, iEncodedBy);
                return HshOut;
            }
            public string CalculateRoomRent(Int32 iEncounterId, Int32 iHospitalLocationId, Int32 iFacilityID, Int32 iUserCode)
            {
                HshIn = new Hashtable();
                try
                {
                    HshIn.Add("@EncId", iEncounterId);
                    HshIn.Add("@HospLocation", iHospitalLocationId);
                    HshIn.Add("@FacilityID", iFacilityID);
                    HshIn.Add("@UserID", iUserCode);
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPInvRoomRentBedCat", HshIn);
                    return "Success";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            public Hashtable PatientMinMAxCslipDate(Int32 iHospitalLocationid, Int32 iFacilityID, Int32 iEncounterID)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationid));
                HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityID));
                HshIn.Add("IntEncounterId", iEncounterID);

                HshOut.Add("Mindate", SqlDbType.VarChar);
                HshOut.Add("Maxdate", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetMinMaxChargeslipDate", HshIn, HshOut);
                return HshOut;
            }


            public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sRemark,
                            Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
                            int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, int EntrySite, int ModuleId)
            {
                return saveOrders(iHospitalLocationId, iFacilityId, iRegistrationId, iEncounterId, sStrXML, sRemark,
                                                  iEncodedBy, iDoctorId, iCompanyId, sOrderType, cPayerType, sPatientOPIP,
                                                  iInsuranceId, iCardId, dtOrderDate, sChargeCalculationRequired, EntrySite, 0, false,false, ModuleId);
            }

            public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sRemark,
                                    Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
                                    int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, int EntrySite, int IsEmergency, bool isGenerateAdvance, bool IsApprovalRequired, int ModuleId)
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
                HshIn.Add("@bitIsApprovalRequired", IsApprovalRequired);
                HshIn.Add("ModuleID", ModuleId);

                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("intOrderId", SqlDbType.Int);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("intNEncounterID", SqlDbType.Int);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);
                return HshOut;
            }

            public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sRemark,
                                    Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP,
                                    int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, int EntrySite, int IsEmergency, 
                                    bool isGenerateAdvance, bool IsApprovalRequired, bool IsReadBack,string ReadBackNote)
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
                HshIn.Add("@bitIsApprovalRequired", IsApprovalRequired);

                HshIn.Add("@bitIsReadBack", IsReadBack);
                HshIn.Add("@chvReadBackNote", ReadBackNote);

                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("intOrderId", SqlDbType.Int);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("intNEncounterID", SqlDbType.Int);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);
                return HshOut;
            }
            public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML,
                                    string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType,
                                    string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, string sXMLSurgeryDetail,
                                    DateTime dtOrderDate, string sChargeCalculationRequired, bool IsMultiIncision, int EntrySite,int ModuleId)
            {
                return saveOrders(iHospitalLocationId, iFacilityId, iRegistrationId, iEncounterId, sStrXML,
                                    sRemark, iEncodedBy, iDoctorId, iCompanyId, sOrderType,
                                    cPayerType, sPatientOPIP, iInsuranceId, iCardId, sXMLSurgeryDetail,
                                    dtOrderDate, sChargeCalculationRequired, IsMultiIncision, EntrySite, 0, false, ModuleId);
            }

            public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML,
                                    string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType,
                                    string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, string sXMLSurgeryDetail,
                                    DateTime dtOrderDate, string sChargeCalculationRequired, bool IsMultiIncision, int EntrySite, int IsEmergency, bool isGenerateAdvance, int ModuleId)
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
                HshIn.Add("ModuleID", ModuleId);

                HshOut.Add("intOrderId", SqlDbType.Int);
                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("intNEncounterID", SqlDbType.Int);


                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);

                //return HshOut["chvErrorStatus"].ToString();
                return HshOut;
            }


            public Hashtable saveOrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML,
                                 string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType,
                                 string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, string sXMLSurgeryDetail,
                                 DateTime dtOrderDate, string sChargeCalculationRequired, bool IsMultiIncision, string sXml, int ModuleId)
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
                HshIn.Add("ModuleID", ModuleId);

                HshOut.Add("intOrderId", SqlDbType.Int);
                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("intNEncounterID", SqlDbType.Int);


                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);

                //return HshOut["chvErrorStatus"].ToString();
                return HshOut;
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


                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);

                //return HshOut["chvErrorStatus"].ToString();
                return HshOut;
            }

            /// <summary>
            /// To Save ER EMR order All Kinds of Order Details in Order Tables ServiceOrderMain & ServiceOrderDetail
            /// </summary>
            /// <param name="iHospitalLocationId"></param> 
            /// <param name="iFacilityId"></param>
            /// <param name="iRegistrationId"></param>
            /// <param name="iEncounterId"></param>
            /// <param name="sStrXML"></param>
            /// <param name="sRemark"></param>
            /// <param name="iEncodedBy"></param>
            /// <param name="iDoctorId"></param>
            /// <param name="iCompanyId"></param>
            /// <param name="sOrderType"></param>
            /// <returns></returns>
            public Hashtable saveEROrders(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, string sStrXML, string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired, int iIsERorEMRServices, string IIpaddress)
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
                HshIn.Add("iIsERorEMRServices", iIsERorEMRServices);
                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("intOrderId", SqlDbType.Int);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut.Add("intNEncounterID", SqlDbType.Int);
                HshIn.Add("IP", IIpaddress);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderWard", HshIn, HshOut);
                return HshOut;
            }


            public Hashtable saveOrdersForEquipment(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId, string sStrXML, string sRemark, int iEncodedBy, int iDoctorId, int iCompanyId, string sOrderType, string cPayerType, string sPatientOPIP, int iInsuranceId, int iCardId, DateTime dtOrderDate, string sChargeCalculationRequired)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
                HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
                HshIn.Add("dtsOrderDate", dtOrderDate);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("xmlServices", sStrXML);
                HshIn.Add("intEncodedBy", iEncodedBy);
                HshIn.Add("chvRemarks", sRemark);
                HshIn.Add("intDoctorId", iDoctorId);
                HshIn.Add("intCompanyId", iCompanyId);
                HshIn.Add("cPatientOPIP", sPatientOPIP);
                HshOut.Add("intOrderNo", SqlDbType.VarChar);
                HshOut.Add("intOrderId", SqlDbType.Int);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOrderForEquipment", HshIn, HshOut);
                return HshOut;
            }


            public DataSet GetDeptlist(Int32 hosplocId, Int32 FacID, string Stype, Int32 Encounterid)
            {
                HshIn = new Hashtable();
                string Query = "";

                Query = "UspGetDepartmentsIPbillingEncwise";
                HshIn.Add("@Locid", hosplocId);
                HshIn.Add("@FacID", FacID);
                HshIn.Add("@encounterid", Encounterid);
                HshIn.Add("@type", Stype);

                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                return dl.FillDataSet(CommandType.StoredProcedure, Query, HshIn);

            }

            public DataSet getServiceAmount(int HospitalLocationId, int intRegistrationId,
                                            int EncounterId, int DoctorId, int FacilityId, int PayerId, int intCompanyId)
            {
                DataSet ds = new DataSet();
                try
                {
                    HshIn = new Hashtable();
                    HshOut = new Hashtable();
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    HshIn = new Hashtable();
                    HshIn.Add("@inyHospitalLocationID", HospitalLocationId);
                    HshIn.Add("@intRegistrationId", intRegistrationId);
                    HshIn.Add("@intDoctorId", DoctorId);
                    HshIn.Add("@intPayerId", PayerId);
                    HshIn.Add("@intCompanyId", intCompanyId);
                    HshIn.Add("@intLoginFacilityId", FacilityId);
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientOPVisitCharges", HshIn);
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
                return ds;



            }

            /// <summary>
            /// To Get Single Service Amount
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iFacilityId"></param>
            /// <param name="iCompanyId"></param>
            /// <param name="iServiceId"></param>
            /// <param name="iRegistrationId"></param>
            /// <param name="iEncounterId"></param>
            /// <param name="iDoctorId"></param>
            /// <returns></returns>
            /// 

            public Hashtable getSingleServiceAmountTimeBase(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
         Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge)
            {
                return getSingleServiceAmountTimeBase(iHospitalLocationId, iFacilityId, iCompanyId, iInsuranceId, iCardId, cPatientOPIP, iServiceId,
                        iRegistrationId, iEncounterId, iDoctorId, intSpecialisationId, iEMergencyCharge, DateTime.Now);
            }

            public Hashtable getSingleServiceAmountTimeBase(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
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
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, HshOut);

                    return HshOut;
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }


            public Hashtable getSingleServiceAmount(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
               Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge, string OrderDate)
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

                if (OrderDate != string.Empty)
                {
                    HshIn.Add("@dtsOrderDate", OrderDate);
                }

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

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, HshOut);

                return HshOut;
            }

            public Hashtable getSingleServiceAmount(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
              Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge, string OrderDate ,int Isbedsidecharges)
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

                if (OrderDate != string.Empty)
                {
                    HshIn.Add("@dtsOrderDate", OrderDate);
                }
                HshIn.Add("Isbedsidecharges", Isbedsidecharges);
                

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
                

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, HshOut);

                return HshOut;
            }

            public Hashtable getSingleServiceAmount_WithDateTime(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
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
                HshIn.Add("dtsOrderDate", Billingdate);

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
                HshOut.Add("chvServiceRemarks", SqlDbType.VarChar);
                HshOut.Add("chvCompanyServiceCode", SqlDbType.VarChar);
                HshOut.Add("TAT", SqlDbType.Int);
                HshOut.Add("ServicelimitAmount", SqlDbType.Money);
                HshOut.Add("IsServiceApprovalRequired", SqlDbType.Bit);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneService", HshIn, HshOut);

                return HshOut;
            }
            public Hashtable getSingleServiceAmountForCounSeling(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iInsuranceId, Int32 iCardId, string cPatientOPIP, int iServiceId,
                Int32 iRegistrationId, Int32 iEncounterId, Int32 iDoctorId, int intSpecialisationId, int iEMergencyCharge
                , int Bedcatid)
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
                HshIn.Add("intBedCategoryId", Bedcatid);
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
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetServiceChargeOPOneServiceForCounseling", HshIn, HshOut);

                return HshOut;
            }
            /// <summary>
            /// Used to Get Visit Charges (single)
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iFacilityId"></param>
            /// <param name="iCompanyId"></param>
            /// <param name="iBedCategoryId"></param>
            /// <param name="iSpecialisationId"></param>
            /// <param name="iDoctorId"></param>
            /// <param name="iServiceId"></param>
            /// <param name="sModifierCode"></param>
            /// <returns></returns>
            /// 
            public Hashtable GetSingleServiceChargeVisit(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iCompanyId, Int32 iBedCategoryId, Int32 iSpecialisationId, Int32 iDoctorId, Int32 iServiceId, String sModifierCode)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("inyHospitalLocationID", Convert.ToInt32(iHospitalLocationId));
                HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
                HshIn.Add("intCompanyid", iCompanyId);
                if (iBedCategoryId != 0)
                    HshIn.Add("intBedCategoryId", iBedCategoryId);
                if (iSpecialisationId != 0)
                    HshIn.Add("intSpecialisationId", iSpecialisationId);
                if (iDoctorId != 0)
                    HshIn.Add("intDoctorId", iDoctorId);
                if (sModifierCode != "")
                    HshIn.Add("chvModifierCode", sModifierCode);
                HshIn.Add("intServiceId", iServiceId);
                HshOut.Add("NChr", SqlDbType.Money);
                HshOut.Add("DiscountNAmt", SqlDbType.Money);
                HshOut.Add("PatientNPayable", SqlDbType.Money);
                HshOut.Add("PayorNPayable", SqlDbType.Money);
                HshOut.Add("DiscountPerc", SqlDbType.Money);
                HshOut.Add("DepartmentId", SqlDbType.VarChar);
                HshOut.Add("ServiceType", SqlDbType.VarChar);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetServiceChargeVisit", HshIn, HshOut);

                return HshOut;
            }

            /// <summary>
            /// To Get saved Order Package Service Id of an Encounter
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iEncounterId"></param>
            /// <returns></returns>
            public DataSet getOrderServiceIdByEncounter(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iEncounterId, String sPageType, String sServType, Int32 iBillId, Int16 isDetails, String sBillNo)
            {
                DataSet ds = new DataSet();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                if (sPageType == "P")
                {
                    HshIn.Add("@intHospitalId", iHospitalLocationId);
                    HshIn.Add("@intEncounterId", iEncounterId);
                    ds = objDl.FillDataSet(CommandType.Text, " select od.ServiceId,ios.ServiceName, od.id as ID from ServiceOrderMain Om WITH (NOLOCK) inner join ServiceOrderDetail od WITH (NOLOCK) on Om.Id = od.OrderId  inner join ItemOfService ios WITH (NOLOCK) on od.ServiceId = ios.ServiceId where Om.EncounterId = @intEncounterId AND od.PackageId = od.ServiceId and Om.HospitalLocationId = @intHospitalId order by ServiceName", HshIn);
                }

                else if (sPageType != "")
                {
                    HshIn.Add("@intHospitalId", iHospitalLocationId);
                    HshIn.Add("@intEncounterId", iEncounterId);

                    if (iBillId > 0)
                    {
                        HshIn.Add("BillId", iBillId);
                    }
                    ds = objDl.FillDataSet(CommandType.Text, " select od.ServiceId,ios.ServiceName,od.id as ID from ServiceOrderMain Om WITH (NOLOCK) inner join ServiceOrderDetail od WITH (NOLOCK) on Om.Id = od.OrderId  inner join ItemOfService ios WITH (NOLOCK) on od.ServiceId = ios.ServiceId where Om.EncounterId = @intEncounterId and od.ServiceType IN (" + sServType + ") AND Om.HospitalLocationId = @intHospitalId order by ServiceName", HshIn);
                }
                else if (sPageType == "")
                {
                    HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                    HshIn.Add("intFacilityId", iFacilityId);
                    HshIn.Add("EncounterId", iEncounterId);
                    HshIn.Add("sServType", sServType);
                    if (iBillId > 0)
                    {
                        HshIn.Add("BillId", iBillId);
                        HshIn.Add("sumOrDtl", isDetails);
                    }
                    else if (sBillNo != "")
                    {
                        HshIn.Add("chvBillNo", sBillNo);
                        HshIn.Add("sumOrDtl", isDetails);
                    }
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceOrder", HshIn);
                }



                return ds;
            }
            public DataSet getOrderServiceIdByEncounterIP(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iEncounterId, String sPageType, String sServType, Int32 iBillId, Int16 isDetails, String sBillNo)
            {
                DataSet ds = new DataSet();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("EncounterId", iEncounterId);
                HshIn.Add("sServType", sServType);
                HshIn.Add("@chrFromPage", sPageType);

                if (iBillId > 0)
                {
                    HshIn.Add("BillId", iBillId);
                    HshIn.Add("sumOrDtl", isDetails);
                }
                else if (sBillNo != "")
                {
                    HshIn.Add("chvBillNo", sBillNo);
                    HshIn.Add("sumOrDtl", isDetails);
                }
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceOrder", HshIn);

                return ds;
            }
            /// <summary>
            /// view Service order
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iFacilityId"></param>
            /// <param name="iEncNo"></param>
            /// <param name="iRegNo"></param>
            /// <param name="iOrderno"></param>
            /// <param name="iFromeDate"></param>
            /// <param name="iToDate"></param>
            /// <returns></returns>
            public DataSet GetViewServiceOrder(int iHospitalLocationId, int iFacilityId, string iEncNo, string iRegNo, string iPName, string iOrderno, string iFromeDate, string iToDate, int iDeptId)
            {
                DataSet ds = new DataSet();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncounterId", iEncNo);
                HshIn.Add("@intRegistrationId", iRegNo);
                HshIn.Add("@chvPatientName", iPName);
                HshIn.Add("@chvOrderNo", iOrderno);
                HshIn.Add("@dtFromDate", iFromeDate);
                HshIn.Add("@dtToDate", iToDate);
                HshIn.Add("@intDepartment", iDeptId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGtViewServiceOrder", HshIn);

                return ds;

            }
            /// <summary>
            /// Viw service lise agains order
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iFacilityId"></param>
            /// <param name="iOrderId"></param>
            /// <returns></returns>
            public DataSet GetServiceOrderList(int iHospitalLocationId, int iFacilityId, int iOrderId)
            {
                DataSet ds = new DataSet();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intOrderId", iOrderId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetServiceOrderList", HshIn);

                return ds;


            }
            public void DeleteServiceOrder(int iHospitalLocationId, int iFacilityId, int DetailId, int SodaId, string Cancelremarks)
            {
                //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                //objDl.ExecuteNonQuery(CommandType.Text, "UPDATE ServiceOrdermain SET Active = 0, CancelRemarks = '" + Cancelremarks + "'  WHERE Id = " + iOrderId + " AND HospitalLocationId=" + iHospitalLocationId + " AND FacilityId=" + iFacilityId + "");

                //objDl.ExecuteNonQuery(CommandType.Text, "UPDATE ServiceOrderDetail SET Active = 0, CancelRemarks = '" + Cancelremarks + "' WHERE OrderId = " + iOrderId + "");
            }

            public void DeleteServicelist(int iOrderId, string Cancelremarks)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE ServiceOrderDetail SET Active = 0, CancelRemarks = '" + Cancelremarks + "' WHERE Id = " + iOrderId + "");
            }

            /// <summary>
            ///  To Get Department Type Wise Service 
            /// </summary>
            /// <param name="iHospitalLocationId"></param>
            /// <param name="iFacilityId"></param>
            /// <param name="iEncounterId"></param>
            /// <param name="sPageType"></param>
            /// <param name="sServType"></param>
            /// <param name="iBillId"></param>
            /// <param name="isDetails"></param>
            /// <param name="sBillNo"></param>
            /// <returns></returns>
            public DataSet getServiceOrderDeptTypeWise(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iEncounterId, String sPageType, String sServType, Int32 iBillId, Int16 isDetails, String sBillNo)
            {
                DataSet ds = new DataSet();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                if (sPageType == "P")
                {
                    HshIn.Add("@intHospitalId", iHospitalLocationId);
                    HshIn.Add("@intEncounterId", iEncounterId);
                    ds = objDl.FillDataSet(CommandType.Text, "select od.ServiceId,ios.ServiceName,od.id as ID from ServiceOrderMain Om WITH (NOLOCK) inner join ServiceOrderDetail od WITH (NOLOCK) on Om.Id = od.OrderId  inner join ItemOfService ios WITH (NOLOCK) on od.ServiceId = ios.ServiceId where Om.EncounterId = @intEncounterId AND od.PackageId = od.ServiceId and Om.HospitalLocationId = @intHospitalId  order by ServiceName", HshIn);
                }
                else if (sPageType != "")
                {
                    HshIn.Add("@intHospitalId", iHospitalLocationId);
                    HshIn.Add("@intEncounterId", iEncounterId);

                    if (iBillId > 0)
                    {
                        HshIn.Add("BillId", iBillId);
                    }
                    ds = objDl.FillDataSet(CommandType.Text, "select od.ServiceId,ios.ServiceName,od.id as ID from ServiceOrderMain Om WITH (NOLOCK) inner join ServiceOrderDetail od WITH (NOLOCK) on Om.Id = od.OrderId  inner join ItemOfService ios WITH (NOLOCK) on od.ServiceId = ios.ServiceId where Om.EncounterId = @intEncounterId and od.ServiceType IN (" + sServType + ") AND Om.HospitalLocationId = @intHospitalId order by ServiceName", HshIn);
                }
                else if (sPageType == "")
                {
                    HshIn.Add("inyHospitalLocationID", iHospitalLocationId);
                    HshIn.Add("intFacilityId", iFacilityId);
                    HshIn.Add("EncounterId", iEncounterId);
                    HshIn.Add("sServType", sServType);
                    if (iBillId > 0)
                    {
                        HshIn.Add("BillId", iBillId);
                        HshIn.Add("sumOrDtl", isDetails);
                    }
                    else if (sBillNo != "")
                    {
                        HshIn.Add("chvBillNo", sBillNo);
                        HshIn.Add("sumOrDtl", isDetails);
                    }
                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceOrderDeptTypeWise", HshIn);
                }
                return ds;
            }



            public DataSet getOutstanding(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iYearId, Int32 iRegistrationId, Int32 iCompanyId, Int32 iReceiptId, Int32 iInvoiceId, String sDateRange, String sFromDate, String sToDate)
            {
                DataSet ds = new DataSet();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn = new Hashtable();
                HshIn.Add("iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("iLoginFacilityId", iFacilityId);
                HshIn.Add("iRegID", iRegistrationId);
                HshIn.Add("iCompanyID", iCompanyId);
                HshIn.Add("iReceiptId", iReceiptId);
                HshIn.Add("iInvoiceId", iInvoiceId);
                HshIn.Add("chvDateRange", sDateRange);
                HshIn.Add("dtFromDate", sFromDate);
                HshIn.Add("dtToDate", sToDate);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOutStanding", HshIn);
                return ds;
            }

            public string UpdateServiceDiscountIP(string xmlService, string issuetype)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@xmlServices", xmlService);
                HshIn.Add("@issuetype", issuetype);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "usp_Update_ServiceDiscountIP", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();


            }
            public string DeactivateOrderService(int SodaId, string ServiceDetailId, int UserId)
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn = new Hashtable();
                HshIn.Add("ServiceDetailId", ServiceDetailId);
                HshIn.Add("ServiceOrderDtlAmtID", SodaId);
                HshIn.Add("UserId", UserId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelOrderService", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();

            }
            public string DeactivateOrderServiceOP(int HospitalLocationId, int FacilityId, int ServiceId, int OrderId,
              int ServiceOrderId, int ServiceOrderDtlAmtId, string chvRemarks, int intEncodedBy)
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn = new Hashtable();

                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intServiceId", ServiceId);
                HshIn.Add("intOrderId", OrderId);
                HshIn.Add("intServiceOrderId", ServiceOrderId);
                HshIn.Add("intServiceOrderDtlAmtId", ServiceOrderDtlAmtId);
                HshIn.Add("chvRemarks", chvRemarks);
                HshIn.Add("intEncodedBy", intEncodedBy);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelOrderServicesOP", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();

            }
           // public string TimeBasedServiceSaveData(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId,
           //Int32 iEncounterId, string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iServiceId
           //, int Units, string sPatientOPIP,
           // DateTime dtOrderDate, DateTime dtFromDate, DateTime dtToDate,
           //double UnitServiceAmt, double UnitDrAmt, double UnitDiscPerc)
           // {
           //     HshIn = new Hashtable();
           //     HshOut = new Hashtable();
           //     HshIn.Add("intHospitalLocationId", iHospitalLocationId);
           //     HshIn.Add("intFacilityId", iFacilityId);
           //     HshIn.Add("cPatientOPIP", sPatientOPIP);
           //     HshIn.Add("dtsOrderDate", dtOrderDate);
           //     HshIn.Add("intRegistrationId", iRegistrationId);
           //     // HshIn.Add("intBedCatID", BedCatID);
           //     //HshIn.Add("intBillCatID", BillCatID);
           //     HshIn.Add("intEncodedBy", iEncodedBy);
           //     //HshIn.Add("intAdvDoctorId", AdvDoctorId);
           //     HshIn.Add("intServiceId", iServiceId);
           //     HshIn.Add("dtFromDate", dtFromDate);
           //     HshIn.Add("dtToDate", dtToDate);
           //     HshIn.Add("units", Units);
           //     HshIn.Add("mnUnitServiceAmt", UnitServiceAmt);
           //     HshIn.Add("mnUnitDrAmt", UnitDrAmt);
           //     HshIn.Add("mnUnitDiscPerc", UnitDiscPerc);
           //     HshIn.Add("intEncounterID", iEncounterId);
           //     HshIn.Add("intDoctorId", iDoctorId);
           //     HshIn.Add("chvServiceRemark", sRemark);
           //     HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
           //     DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
           //     HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveTimeBasedService", HshIn, HshOut);
           //     return HshOut["chvErrorStatus"].ToString();
           // }


            public Hashtable TimeBasedServiceSaveData(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId,
          Int32 iEncounterId, string sRemark, Int32 iEncodedBy, Int32 iDoctorId, Int32 iServiceId
          , int Units, string sPatientOPIP,
           DateTime dtOrderDate, DateTime dtFromDate, DateTime dtToDate,
          double UnitServiceAmt, double UnitDrAmt, double UnitDiscPerc)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("cPatientOPIP", sPatientOPIP);
                HshIn.Add("dtsOrderDate", dtOrderDate);
                HshIn.Add("intRegistrationId", iRegistrationId);
                // HshIn.Add("intBedCatID", BedCatID);
                //HshIn.Add("intBillCatID", BillCatID);
                HshIn.Add("intEncodedBy", iEncodedBy);
                //HshIn.Add("intAdvDoctorId", AdvDoctorId);
                HshIn.Add("intServiceId", iServiceId);
                HshIn.Add("dtFromDate", dtFromDate);
                HshIn.Add("dtToDate", dtToDate);
                HshIn.Add("units", Units);
                HshIn.Add("mnUnitServiceAmt", UnitServiceAmt);
                HshIn.Add("mnUnitDrAmt", UnitDrAmt);
                HshIn.Add("mnUnitDiscPerc", UnitDiscPerc);
                HshIn.Add("intEncounterID", iEncounterId);
                HshIn.Add("intDoctorId", iDoctorId);
                HshIn.Add("chvServiceRemark", sRemark);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    return objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveTimeBasedService", HshIn, HshOut);

                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }
            public string WardValidateDoctorVisits(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId, string sStrXML,
                     int iDoctorId, DateTime dtOrderDate, int iEncodedBy)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("intHospitalLocationId", Convert.ToInt32(iHospitalLocationId));
                HshIn.Add("intFacilityId", Convert.ToInt32(iFacilityId));
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("intEnounterId", iEncounterId);
                HshIn.Add("xmlServices", sStrXML);
                HshIn.Add("intDoctorId", iDoctorId);
                HshIn.Add("dtsOrderDate", dtOrderDate.ToString("yyyy/MM/dd"));
                HshIn.Add("intEncodedBy", iEncodedBy);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);


                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspWardValidateDoctorVisits", HshIn, HshOut);

                return HshOut["chvErrorStatus"].ToString();

            }

            public string SavePatientTimeBasedService(int Id, int EncounterId, int ServiceId,
        int DoctorId, DateTime StartDateTime, DateTime? EndDateTime, bool IsStop, int OrderId
        , int ServiceOrderDetailId, string OrderPeriod,
         int UnitsCalculated, string Remarks, int EncodedBy)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intId", Id);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("intServiceId", ServiceId);
                HshIn.Add("intDoctorId", DoctorId);
                HshIn.Add("dtStartDateTime", StartDateTime);
                HshIn.Add("dtEndDateTime", EndDateTime);
                HshIn.Add("bitIsStop", IsStop);
                HshIn.Add("intOrderId", OrderId);
                HshIn.Add("intServiceOrderDetailId", ServiceOrderDetailId);
                HshIn.Add("intOrderPeriod", OrderPeriod);
                HshIn.Add("intUnitsCalculated", UnitsCalculated);
                HshIn.Add("chvRemarks", Remarks);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientTimeBasedService", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            public DataSet GetPatientTimeBasedService(int EncounterId)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intEncounterId", EncounterId);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientTimeBasedService", HshIn, HshOut);
            }
            public string CancelPatientTimeBasedService(int Id, int EncodedBy)
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intId", Id);
                HshIn.Add("EncodedBy", EncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCancelPatientTimeBasedService", HshIn, HshOut);

                return HshOut["chvErrorStatus"].ToString();
            }
        }

        public class clsDocSetup
        {
            private string sConString = "";
            public clsDocSetup(string conString)
            {
                sConString = conString;
            }

            public DataSet getDocumentList(int iHospitalLocationId, int intEncodedBy)
            {
                Hashtable HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDocumentList", HshIn, HshOut);
                return ds;
            }

            public string SaveDocumentSetup(int iHospitalLocationId, int iID, int iYearID, string sPrefixType,
                                            string sPrefix, DateTime dtFromDate, DateTime dtToDate,
                                            int iDocNo, string sDType, int intEncodedBy)
            {
                Hashtable HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                if (iID > 0)
                    HshIn.Add("@intid", iID);

                HshIn.Add("@intYearId", iYearID);
                HshIn.Add("@chPrefixType", sPrefixType);
                HshIn.Add("@vPrefix", sPrefix);
                HshIn.Add("@FromDate", dtFromDate);
                HshIn.Add("@ToDate", dtToDate);
                HshIn.Add("@intDocumentNo", iDocNo);
                HshIn.Add("@vDocumentType", sDType);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDocumentSetup", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }

        }

        public class clsBillDispatch
        {
            private string sConString = "";
            public clsBillDispatch(string conString)
            {
                sConString = conString;
            }

            public DataSet getBillDispatch(int HospitalLocationId, int FacilityId, string OPIP, string DateSearchon, string Fromdate, string ToDate,
                int Company, string Searchon, string Searchvalue, int EncodedBy)
            {
                Hashtable HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrOPIP", OPIP);
                HshIn.Add("@chrDateSearchon", DateSearchon);
                HshIn.Add("@chrFromDate", Fromdate);
                HshIn.Add("@chrToDate", ToDate);
                HshIn.Add("@intCompany", Company);
                HshIn.Add("@chrSearchon", Searchon);
                HshIn.Add("@chvSearchvalue", Searchvalue);

                HshIn.Add("@intEncodedby", EncodedBy);
                HshOut.Add("@chvErrorMessage", SqlDbType.VarChar);

                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetBillDispatch", HshIn, HshOut);
                return ds;
            }
        }

        public string SaveBillDispatch(int iHospitalLocationId, string xmlData, int intEncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@xmlBillDispatch", xmlData);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveBillDispatch", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataSet getAcountReport(int iHospitalLocationId, int iFacilityId, int iDepartmentId, int iEmpId, DateTime Fromdate, DateTime ToDate)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            if (iDepartmentId != 0)
            {
                HshIn.Add("@fdate", Fromdate.ToString("dd/MM/yyyy 00:00"));
                HshIn.Add("@tdate", ToDate.ToString("dd/MM/yyyy 23:59"));
                HshIn.Add("@usercode", iEmpId);
                HshIn.Add("@subdeptcode", "0");
                HshIn.Add("@deptcode", iDepartmentId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "FaAccountingReports", HshIn);
            }
            else
            {
                HshIn.Add("@fdate", Fromdate.ToString("yyyy/MM/dd"));
                HshIn.Add("@tdate", ToDate.ToString("yyyy/MM/dd"));
                HshIn.Add("@HospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityid", iFacilityId);
                HshIn.Add("@deptcode", iDepartmentId);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "FaOutstandingReport", HshIn);
            }
            return ds;
        }

        public string UpdatePatientPackageOrder(Int32 iHospitalLocationId, Int32 iFacilityId, Int32 iRegistrationId, Int32 iEncounterId, Int32 iPackageId, String xmlService, int iIsExcluded)
        {
            Hashtable hstInput = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("inyHospitalLocationID", iHospitalLocationId);
            hstInput.Add("intFacilityId", iFacilityId);
            hstInput.Add("intRegistrationId", iRegistrationId);
            hstInput.Add("intEncounterId", iEncounterId);
            hstInput.Add("intPackageId", iPackageId);
            hstInput.Add("xmlServiceList", xmlService);
            hstInput.Add("intExclude", iIsExcluded);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdatePatientPackageOrder", hstInput, hstOutput);
            return hstOutput["chvErrorStatus"].ToString();
        }

        public string InsertIPServiceDiscount(Int32 iEncounterId, string sIssueType, Int32 iUserId, String xmlService)
        {
            Hashtable hstInput = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("intEncounterId", iEncounterId);
            hstInput.Add("issuetype", sIssueType);
            hstInput.Add("intUserId", iUserId);
            hstInput.Add("xmlServices", xmlService);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspInsertIPServiceDiscount", hstInput, hstOutput);
            return hstOutput["chvErrorStatus"].ToString();
        }

        public DataSet GetPackageSurgeryToFillOrderGrid(Int16 HospId, Int32 FacilityId, Int32 iEncounterId, Int32 iPackageId, Int32 iCompanyId,
                                            Int32 IsMainPackage, DateTime dtOrderDate, Int32 iPackageCount, int isMultiIncision)
        {
            Hashtable hstInput = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hstInput.Add("inyHospitalLocationId", HospId);
            hstInput.Add("intFacilityId", FacilityId);
            hstInput.Add("intEncounterId", iEncounterId);
            hstInput.Add("intPackageId", iPackageId);
            hstInput.Add("intCompanyId", iCompanyId);
            hstInput.Add("intIsMainPackage", IsMainPackage);
            hstInput.Add("dtsOrderDate", dtOrderDate);
            hstInput.Add("iPackageCount", iPackageCount);
            hstInput.Add("isMultiIncision", isMultiIncision);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageSurgeryToFillOrderGrid", hstInput);
            return ds;
        }

        //public DataSet GetFillPatientVisitHistory(int HospId, int FacilityId, string RegistrationNo)
        //{
        //    Hashtable hstInput = new Hashtable();
        //    DataSet ds = new DataSet();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //    hstInput.Add("inyHospitalLocationid", HospId);
        //    hstInput.Add("insFacilityId", FacilityId);
        //    hstInput.Add("intRegistrationNo", RegistrationNo);

        //    ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspFillPatientVisitHistory", hstInput);
        //    return ds;
        //}

        public DataSet GetFillPatientVisitHistory(int HospId, int FacilityId, string RegistrationNo, DateTime fromDate, DateTime toDate)
        {
            string fDate = fromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = toDate.ToString("yyyy-MM-dd 23:59");
            Hashtable hstInput = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("inyHospitalLocationid", HospId);
            hstInput.Add("insFacilityId", FacilityId);
            hstInput.Add("intRegistrationNo", RegistrationNo);
            hstInput.Add("chvDateFrom", fDate);
            hstInput.Add("chvDateTo", tDate);
            return objDl.FillDataSet(CommandType.StoredProcedure, "UspFillPatientVisitHistory", hstInput);
        }
        public string UpdateServiceDetailAmtCoPay(string xmlCoPayDetail, string DepartmentStoreType)
        {
            Hashtable hstInput = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("xmlCoPayDetail", xmlCoPayDetail);
            hstInput.Add("DepartmentStoreType", DepartmentStoreType);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateServiceDetailAmtCoPay", hstInput, hstOutput);
            return hstOutput["chvErrorStatus"].ToString();
        }
        public string UpdateApprovalCode(string xmlApprovalCode, int InvoiceId)
        {
            Hashtable hstInput = new Hashtable();
            Hashtable hstOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("xmlapprovalDetails", xmlApprovalCode);
            hstInput.Add("intinvoiceId", InvoiceId);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspsaveinvoiceApprovalCode", hstInput, hstOutput);
            return hstOutput["chvErrorStatus"].ToString();
        }
        public string InsuranceDetailCompany(int RegistrationId, int payerid, int sponsorid, int cardid)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intRegitrationId", RegistrationId);
            HshIn.Add("intPayerId", payerid);
            HshIn.Add("intSponsorId", sponsorid);
            HshIn.Add("intCardId", cardid);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspCheckInsuranceDetailCompany", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();

        }
        public string UpdateMemberIdEmirateid(int RegistrationId, int RegistartionInsuranceId, string chrMemberId, string chrEmiratesId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intRegistrationId", RegistrationId);
            HshIn.Add("intRegistrationInsuranceId", RegistartionInsuranceId);
            HshIn.Add("chrMemberId", chrMemberId);
            HshIn.Add("chrEmiratesId", chrEmiratesId);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateMemberIdEmirateid", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();

        }
        public DataSet GetTopupCompanyPatientWise(int RegistrationId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intRegistrationId", RegistrationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTopupCompanyPatientWise", HshIn);
            return ds;

        }

        public string VerifyCreditInvoice(int intinvoiceId, int UserId, int intstatus)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intinvoiceId", intinvoiceId);
            HshIn.Add("intuserId", UserId);
            HshIn.Add("intstatus", intstatus);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspVerifyCreditInvoice", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();

        }
        public string ApplyCreditLimitInIPBill(int intEncounterId, int intRegistrationId, int inyHospLocationId, int insFacilityId, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intEncounterId", intEncounterId);
            HshIn.Add("intRegistrationId", intRegistrationId);
            HshIn.Add("inyHospLocationId", inyHospLocationId);
            HshIn.Add("insFacilityId", insFacilityId);
            HshIn.Add("intUserId", UserId);

            HshOut.Add("ErrorMessage", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPApplyCreditLimitInIPBill", HshIn, HshOut);
            return HshOut["ErrorMessage"].ToString();
        }

        public DataSet getRegistrationChecklist(int intRegistrationId, string ForPage)
        {
            HshIn = new Hashtable();
            HshIn.Add("intRegistrationId", intRegistrationId);
            HshIn.Add("ForPage", ForPage);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspgetRegistrationChecklist", HshIn);
            return ds;
        }
        // wcf methods  


        public DataSet uspGetIPServiceOrderSummary(Int32 iHospitalLocationId, Int32 iFacilityId,
        Int32 iRegistrationId, Int32 iEncounterId, String sPageType, String sServType, Int32 iBillId, string Procedure)
        {
            DataSet ds = new DataSet();
            Hashtable hstInput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hstInput.Add("inyHospitalLocationID", iHospitalLocationId);
            hstInput.Add("intFacilityId", iFacilityId);
            hstInput.Add("RegId", iRegistrationId);
            hstInput.Add("EncounterId", iEncounterId);
            hstInput.Add("sServType", sServType);
            hstInput.Add("chrFromPage", sPageType);

            if (iBillId > 0)
            {
                hstInput.Add("BillId", iBillId);

            }

            ds = objDl.FillDataSet(CommandType.StoredProcedure, Procedure, hstInput);

            return ds;
        }

        public int DeletePatientNotes(int HospId, int FacilityId, int Id, int UserId)
        {

            Hashtable hstInput = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hstInput.Add("@inyHospitalLocationId", HospId);
            hstInput.Add("@intFacilityId", FacilityId);
            hstInput.Add("@intId", Id);
            hstInput.Add("@intLadtchangeBy", UserId);

            string strdel = "Update PatientNotes Set Active=0,LastChangedBy=@intLadtchangeBy,LastChangedDate=GETUTCDATE() where Id =@intId and HospitalLocationId=@inyHospitalLocationId and FacilityId=@intFacilityId";
            int i = objDl.ExecuteNonQuery(CommandType.Text, strdel, hstInput);
            return i;
        }

        public string AuditInvoice(int intinvoiceId, int UserId, string AuditType)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("intinvoiceId", intinvoiceId);
            HshIn.Add("intuserId", UserId);
            HshIn.Add("chrAuditType", AuditType);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveAuditInvoice", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();

        }

        public DataSet getServiceDetailsER(int iHospitalLocationId, int FacilityId, int RegistrationId, int EncounterId, int InvoiceID)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intInvoiceID", InvoiceID);

            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "USpGetOPERMedicineDetails", HshIn);
            return ds;
        }
        public string SaveOPDiscount(int HospId, int FacilityId, int EncId, int RegId, int AuthorisedBy, int CompnayId, string XmlService, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncounterId", EncId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intAuthorisedBy", AuthorisedBy);
            HshIn.Add("@intCompanyId", CompnayId);
            HshIn.Add("@xmlService", XmlService);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvOutputMessge", SqlDbType.VarChar);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveOPDiscount", HshIn, HshOut);

            return HshOut["@chvOutputMessge"].ToString();
        }

        public string GetPayParty(int Payer)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            HshIn.Add("@Payer", Payer);
            string PayParty = "0";
            ds = Dl.FillDataSet(CommandType.Text, "select Companyid,isnull(IsInsuranceCompany,0) as IsInsuranceCompany,PaymentType   from Company where CompanyId=@Payer", HshIn);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["IsInsuranceCompany"]) > 0 && Convert.ToString(ds.Tables[0].Rows[0]["PaymentType"]) == "B")
                {
                    PayParty = "2";
                }

                if (Convert.ToInt32(ds.Tables[0].Rows[0]["IsInsuranceCompany"]) == 0 && Convert.ToString(ds.Tables[0].Rows[0]["PaymentType"]) == "B")
                {
                    PayParty = "1";

                }

                if (Convert.ToString(ds.Tables[0].Rows[0]["PaymentType"]) == "C")
                {
                    PayParty = "0";

                }
            }

            return PayParty;
        }
        public DataSet GetReasontype(string Reason)
        {
            DataSet ds = new DataSet();
            StringBuilder strSQL = new StringBuilder();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@chvReasonType", Reason);

            strSQL.Append("SELECT DISTINCT Reason, Id FROM ReasonMaster WITH(NOLOCK) WHERE ReasonType = @chvReasonType AND Active=1");
            ds = objDl.FillDataSet(CommandType.Text, strSQL.ToString(), HshIn);

            return ds;
        }
        public DataSet Getdetectabledetails(int invoiceid)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();

            HshIn.Add("@intinvoiceid", invoiceid);

            ds = Dl.FillDataSet(CommandType.Text, "Select * FROM InvoiceDetectable WITH (NOLOCK) where invoiceID=@intinvoiceid ", HshIn);

            return ds;
        }
        public string updateDetactable(int invoiceid, string reason, int active, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intinvoiceID", invoiceid);
            HshIn.Add("@Chrreasion", reason);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intactive", active);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspupdateDetectable", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();


        }
		
		public string SaveSpecialRightMaster(int ID, string Flag, int HospitalLocationId, string Description, bool Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@ID", ID);
            HshIn.Add("@Flag", Flag);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
            HshIn.Add("@Description", Description);
            HshIn.Add("@Active", Active);
            HshIn.Add("@EncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveSpecialRightMaster", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataTable getSpecialRightMaster(int HospitalLocationId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);

            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetSpecialRightMaster", HshIn);
            return ds.Tables[0];
        }

		
		public DataTable getEmployeeList(int iHospitalLocationId, int iFacilityId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn);
            return ds.Tables[0];
        }
        public DataSet getEmployeeWiseSpecialRightTagging(int HospitalLocationId, int facilityId, int employeeId, 
            string ShowEmpTaggedData, string searchFlagLeft, string searchFlagRight)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
            HshIn.Add("@iFacilityId", facilityId);

            HshIn.Add("@sShowEmpTaggedData", ShowEmpTaggedData);
            HshIn.Add("@iemployeeId", employeeId);
            HshIn.Add("@chSearchFlagLeft", searchFlagLeft);
            HshIn.Add("@chSearchFlagRight", searchFlagRight);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetSpecialRightMaster", HshIn);
            return ds;
        }
        public string UpdateEmployeeWiseSpecialRightTagging(int ID, int HospitalLocationId, int FacilityId, int SpecialRightId, int EmployeeId, bool Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@ID", ID);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@SpecialRightId", SpecialRightId);
            HshIn.Add("@EmployeeId", EmployeeId);
            HshIn.Add("@Active", Active);
            HshIn.Add("@EncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspUpdateEmployeeWiseSpecialRightTagging", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }
        public string SaveEmployeeWiseSpecialRightTagging(int iHospId, string iXmlData, int iEmployeeId, int iFaclityId, bool iActive, int iEncodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("intHospitalLocationId", iHospId);
            HshIn.Add("xmlEmployeeSpecRightTag", iXmlData);
            HshIn.Add("intFacilityId", iFaclityId);
            HshIn.Add("btActive", iActive);
            HshIn.Add("intEncodedBy", iEncodedby);
            HshIn.Add("intEmployeeId", iEmployeeId);
            //HshIn.Add("intSpecialRightId", SpecialRightId);
            // HshIn.Add("flag", sFlag);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEmployeeWiseSpecialRightTagging", HshIn, HshOut);
            return HshOut["chvErrorStatus"].ToString();
        }
        public DataSet getserviceactivity(int hospitalLocationId, int facilityId, int RegId, int EncounterID, int BillId, int isPackageUnder, string procedure)
        {
            Hashtable hsIn = new Hashtable();

            hsIn.Add("@inyHospitalLocationId", hospitalLocationId);
            hsIn.Add("@intFacilityId", facilityId);
            hsIn.Add("@RegId", RegId);
            hsIn.Add("@EncounterId", EncounterID);
            hsIn.Add("@BillId", BillId);
            hsIn.Add("@isPackageUnder", isPackageUnder);


            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, procedure, hsIn);
            return ds;
        }


        public string SaveFileCloseMRD (int hospitalLocationId, int facilityId, int RegId,
            int EncounterID, int EncodedBy,string RackNumber, bool StatusBit)
        {
           
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intHospitalLocationId", hospitalLocationId);
            HshIn.Add("@intFacilityId", facilityId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intEncounterId", EncounterID);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@chrRackNumber", RackNumber);
            HshIn.Add("@isStatusBit", StatusBit);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveFileCloseMRD", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataTable GetEquipmentDetails(int HospitalId, int FacilityId, int ServiceId)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intServiceId", ServiceId);

            ds = Dl.FillDataSet(CommandType.Text, "select IsEquipmentType, ChargesPeriod, GracePeriod from otequipmentmaster WITH (NOLOCK) where ServiceId = @intServiceId  and  Active = 1 and FacilityId = @intFacilityId and HospitalLocationId = @inyHospitalLocationId ", HshIn);
             
            return ds.Tables[0];
        }

    }

}
