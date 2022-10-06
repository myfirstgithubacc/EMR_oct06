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
    public partial class clsPharmacy
    {
        public DataSet getIPPatientRequest(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                            int RegistrationNo, string EncounterNo, string BedNo, string PatientName,
                            DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy, int OrderTypeId, string OrderStatusType,
                            int StoreId, string EncounterStatus)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                if (RegistrationNo > 0)
                {
                    HshIn.Add("@chvRegistrationNo", RegistrationNo);
                }
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvBedNo", BedNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvFromDate", fDate);
                HshIn.Add("@chvToDate", tDate);
                HshIn.Add("@inyOrderTypeId", OrderTypeId);
                HshIn.Add("@chrPendingStatus", OrderStatusType);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStoreId", StoreId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshIn.Add("@chvEncounterStatus", EncounterStatus);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientRequest", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getIPPatientRequestInv(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                            int RegistrationNo, string EncounterNo, string BedNo, string PatientName,
                            DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy, int OrderTypeId, string OrderStatusType,
                            int StoreId, string EncounterStatus, string PatientType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd");

            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            if (RegistrationNo > 0)
            {
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
            }
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@chvFromDate", fDate);
            HshIn.Add("@chvToDate", tDate);
            HshIn.Add("@inyOrderTypeId", OrderTypeId);
            HshIn.Add("@chrPendingStatus", OrderStatusType);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@chvPatientType", PatientType);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@chvEncounterStatus", EncounterStatus);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientRequestInv", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getIPPatientItemDetails(int IndentId, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
           

            HshIn.Add("@intIndentID", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientItemDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getIPPatientItemDetails(int IndentId, int HospId, int EncodedBy, string ApprovedStatus, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
           

            HshIn.Add("@intIndentID", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@ApprovedStatus", ApprovedStatus);
            HshIn.Add("@intFacilityId", FacilityId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientItemDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getIPPatientItemDetailsAuto(int IndentId, int HospId, int EncodedBy, int StoreId, string ApprovedStatus, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("@intIndentID", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@ApprovedStatus", ApprovedStatus);
            HshIn.Add("@intFacilityId", FacilityId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientItemDetailsAuto", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string PostIPWardReturn(int IssueId, int LoginStoreId, int FacilityId, int HospId, int EncodedBy, string xmlItemIssueSale, string OPIP)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intIssueId", IssueId);
            HshIn.Add("@intLoginStoreId", LoginStoreId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlItemIssueSale", xmlItemIssueSale);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                if (OPIP == "I")
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrPostIPWardReturn", HshIn, HshOut);
                }
                else
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrPostOrderReturn", HshIn, HshOut);
                }

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetCIMSCategory(int Active)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strquery = "SELECT CIMSCategoryId, CIMSCategoryName, ShortName, Active " +
                                  " FROM PhrCIMSCategory WITH (NOLOCK) WHERE 2 = 2 ";
                if (Active == 1)
                {
                    strquery += " AND Active = 1 ";
                }
                strquery += " ORDER BY CIMSCategoryName ";

                return objDl.FillDataSet(CommandType.Text, strquery);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveCimsCategory(int Cimscategoryid, string Cimsname, string Cimsshortname, int Status, int Encodedby)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intCimsCategoryId", Cimscategoryid);
                HshIn.Add("@chvCimscategoryname", Cimsname);
                HshIn.Add("@chvShortname", Cimsshortname);
                HshIn.Add("@btStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveCIMSCategory", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetCIMSSubCategory(int CimsCategoryId, int Active)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intCimsCategoryId", CimsCategoryId);

                string strquery = "SELECT CIMSSubCategoryId, CIMSSubCategoryName, ShortName, CIMSCategoryId, Active " +
                                  " FROM PhrCIMSSubCategory WITH (NOLOCK) WHERE CIMSCategoryId = @intCimsCategoryId ";
                if (Active == 1)
                {
                    strquery += " AND Active = 1 ";
                }
                strquery += " ORDER BY CIMSSubCategoryName ";

                return objDl.FillDataSet(CommandType.Text, strquery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCimsSubCategory(int CimsSubcategoryid, string Cimsname, string Cimsshortname, int CImsCatId, int Status, int Encodedby)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intCimsSubId", CimsSubcategoryid);
                HshIn.Add("@chvCimssubname", Cimsname);
                HshIn.Add("@chvShortname", Cimsshortname);
                HshIn.Add("@intCimsCategoryId", CImsCatId);
                HshIn.Add("@btStatus", Status);
                HshIn.Add("@intEncodedBy", Encodedby);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveCIMSSubCategory", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorFrequencyFavorite(int iDoctorId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intDoctorId", iDoctorId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFavoriteFrequency", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveUpdateDoctorFrequencyFavorite(int iId, int iDoctorId, int iFrequencyId, int iEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intId", iId);
                HshIn.Add("@intDoctorId", iDoctorId);
                HshIn.Add("@intFrequencyid", iFrequencyId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                Hashtable hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateFrequencyFavorite", HshIn, HshOut);
                return hshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string getOPIPEncounter(int HospId, int FacilityId, int RegistrationId, int EncounterId)
        {
            string OPIP = string.Empty;

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                string strquery = "SELECT OPIP FROM Encounter WITH (NOLOCK) WHERE ID=@intEncounterId AND RegistrationId =@intRegistrationId AND FacilityID=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ";

                OPIP = objDl.ExecuteScalar(CommandType.Text, strquery, HshIn).ToString();

                return OPIP;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getFrequencyMaster()
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string strquery = "SELECT Id, Description + CASE WHEN ISNULL(Abbreviation,'')='' THEN '' ELSE + ' - ' + Abbreviation END AS Description, Frequency " +
                                " FROM EMRMedicationFrequencyMaster WITH (NOLOCK) " +
                                " WHERE Active = 1 AND CommonFrequency=0 " +
                                " ORDER BY Sequence";

                return objDl.FillDataSet(CommandType.Text, strquery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //public DataSet getPreviousMedicines(int HospId, int FacilityId, int EncounterId)
        //{
        //    return getPreviousMedicines(HospId, FacilityId, EncounterId, 0,0,"");
        //}

        //public DataSet getPreviousMedicines(int HospId, int FacilityId, int EncounterId, int IndentId,int ItemId, string sPreviousMedication)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@inyHospitalLocationId", HospId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@intEncounterId", EncounterId);
        //        HshIn.Add("@intIndentId", IndentId);
        //        HshIn.Add("@intItemId", ItemId);
        //        HshIn.Add("@chvPreviousMedication", sPreviousMedication);
        //        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //    return ds;
        //}




        public DataSet getPreviousMedicines(int HospId, int FacilityId, int EncounterId)
        {
            return getPreviousMedicines(HospId, FacilityId, EncounterId, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }
        public DataSet getPreviousMedicines(int HospId, int FacilityId, int EncounterId, int IndentId, int ItemId,
                                string sPreviousMedication, string ItemName, string FromDate, string ToDate)
        {
            return getPreviousMedicines(HospId, FacilityId, EncounterId, IndentId, ItemId, sPreviousMedication, ItemName, FromDate, ToDate, string.Empty);
        }

        public DataSet getPreviousMedicines(int HospId, int FacilityId, int EncounterId, int IndentId, int ItemId,
                                string sPreviousMedication, string ItemName, string FromDate, string ToDate, string sConsumable)
        {
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvPreviousMedication", sPreviousMedication);

                if (FromDate.Trim() != string.Empty && ToDate.Trim() != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }

                if (ItemName.Trim() != string.Empty)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }
                if (!sConsumable.Equals(string.Empty))
                {
                    HshIn.Add("@chvConsumable", sConsumable);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIP", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public DataSet getPreviousMedicinesNew(int HospId, int FacilityId, int EncounterId, int IndentId, int ItemId,
                               string sPreviousMedication, string ItemName, string FromDate, string ToDate)
        {
            return getPreviousMedicinesNew(HospId, FacilityId, EncounterId, IndentId, ItemId,
                                 sPreviousMedication, ItemName, FromDate, ToDate, string.Empty);
        }



        public DataSet getPreviousMedicinesNew(int HospId, int FacilityId, int EncounterId, int IndentId, int ItemId,
                              string sPreviousMedication, string ItemName, string FromDate, string ToDate, string sConsumable)
        {
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvPreviousMedication", sPreviousMedication);

                if (FromDate.Trim() != string.Empty && ToDate.Trim() != string.Empty)
                {
                    HshIn.Add("@chrFromDate", FromDate);
                    HshIn.Add("@chrToDate", ToDate);
                }

                if (ItemName.Trim() != string.Empty)
                {
                    HshIn.Add("@chvItemName", ItemName);
                }
                if (!sConsumable.Equals(string.Empty))
                {
                    HshIn.Add("@chvConsumable", sConsumable);
                }

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPreviousMedicinesIPNew", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public string stopPrescription(string DetailsIds, int CancelReasonId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intCancelReasonId", CancelReasonId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                string strqry = "UPDATE ICMIndentDetails SET Active = 0, CancelReasonId = @intCancelReasonId," +
                                " LastchangeBy = @intEncodedBy, LastChangedate = GETUTCDATE() " +
                                " WHERE Id in (" + DetailsIds + ")";

                objDl.ExecuteNonQuery(CommandType.Text, strqry, HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveItemReturnDump(string SaleSetupCode, int HospId, int FacilityId, DateTime IssueDate,
                              int StoreId, int RegistrationId, int EncounterId,
                              int IssueEmpId, string PatientName, string Age, string AgeType, string Gender,
                              int CompanyId, int RefById, string RefByName, string ProcessStatus,
                              int InvoiceId, int BillingCategoryId, int PostedById,
                              DateTime PostedDate, string Remarks, string xmlItemIssueSale,
                              string xmlPaymentMode, double InvoiceAm, int Active, int EncodedBy,
                              int IssueId, out string docN, string RegistrationNo, int TransactionTypeId, string EmployeeNo,
                              out Int32 SavedIssueId, double mRoundOffPatient, double mRoundOffPayer, string Source)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtIssueDate", IssueDate);
                HshIn.Add("@intLoginStoreId", StoreId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intConcessionEmpId", string.Empty);
                HshIn.Add("@chrConcessionRemarks", string.Empty);
                HshIn.Add("@intRequestId", 0);
                HshIn.Add("@intInvoiceId", InvoiceId);
                HshIn.Add("@chrRemarks", Remarks);
                HshIn.Add("@xmlItemIssueSale", xmlItemIssueSale);
                HshIn.Add("@xmlPaymentMode", xmlPaymentMode);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshIn.Add("@chrIssueReturn", "R");
                HshIn.Add("@chrPatientName", PatientName);
                HshIn.Add("@chrAge", Age);
                HshIn.Add("@chrAgeType", AgeType);
                HshIn.Add("@chrGender", Gender);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@intRefById", RefById);
                HshIn.Add("@chrRefByName", RefByName);

                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEmployeeNo", EmployeeNo);
                HshIn.Add("@mRoundOffPatient", mRoundOffPatient);
                HshIn.Add("@mRoundOffPayer", mRoundOffPayer);
                HshIn.Add("@intTransactionStatusId", TransactionTypeId);

                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intIssueId", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshIn.Add("@intRefIssueId", IssueId);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrIssueSaleDump", HshIn, HshOut);

                docN = Convert.ToString(HshOut["@chvDocumentNo"]);
                SavedIssueId = Convert.ToInt32(HshOut["@intIssueId"]);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveReturnApprovalAck(int DumpRefundId, int HospId, int FacilityId, int RegistrationId, int EncodedBy,
                                         out string RefundNo, out string ApprovedRefundId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            RefundNo = string.Empty;
            ApprovedRefundId = string.Empty;

            HshIn.Add("@intDumpReturnId", DumpRefundId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvRefundId", SqlDbType.VarChar);
            HshOut.Add("@chvRefundNo", SqlDbType.VarChar);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrReturnApprovalAck", HshIn, HshOut);

                RefundNo = HshOut["@chvRefundNo"].ToString();
                ApprovedRefundId = HshOut["@chvRefundId"].ToString();

                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getReturnDump(int HospId, int FacilityId, int RegistrationNo, string DateFrom, string DateTo,
                                        string PatientName, int ApprovalStatusId)
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
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrReturnDump", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveReturnApprovalDump(int HospId, int FacilityId, string xmlDumpRefunds, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlDumpRefunds", xmlDumpRefunds);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrReturnApprovalDump", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getOPReturnDetailsDump(int DocId, int StoreId, int FacilityId, int HospId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intDocId", DocId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@inyHospitalLocationID", HospId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrOPReturnDetailsDump", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getPhrGenericMasterList(string chvItemSearch)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chvItemSearch", chvItemSearch);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrGenericMasterList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string saveMasterCIMSTagging(int iHospId, int iItemId, string cUseFor, int iEncodedby, string chvCIMSItemId,
                                            string chvCIMSTYPE, string chvItemDesc, string chvCIMSItemDesc)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intItemId", iItemId);
                HshIn.Add("@chrUseFor", cUseFor);
                HshIn.Add("@intEncodedBy", iEncodedby);
                HshIn.Add("@chvCIMSItemId", chvCIMSItemId);
                HshIn.Add("@chvCIMSTYPE", chvCIMSTYPE);
                HshIn.Add("@chvItemDesc", chvItemDesc);
                HshIn.Add("@chvCIMSItemDesc", chvCIMSItemDesc);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemMasterCIMSTagging", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getCIMSTaggingDetail(int intItemId, string chrUseFor)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intItemId", intItemId);
            HshIn.Add("@chrUseFor", chrUseFor);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrCIMSTaggingDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }




        public DataSet getIPPatientRequestInv(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                            int RegistrationNo, string EncounterNo, string BedNo, string PatientName,
                            DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy, int OrderTypeId, string OrderStatusType,
                            int StoreId, string EncounterStatus, string PatientType, String IsApproved)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd");

            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            if (RegistrationNo > 0)
            {
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
            }
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@chvFromDate", fDate);
            HshIn.Add("@chvToDate", tDate);
            HshIn.Add("@inyOrderTypeId", OrderTypeId);
            HshIn.Add("@chrPendingStatus", OrderStatusType);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@chvPatientType", PatientType);
            HshIn.Add("@IsApproved", IsApproved);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@chvEncounterStatus", EncounterStatus);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientRequestInv", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getIPPatientRequestInv(int IndentId, int HospId, int FacilityId, int RegistrationId, int EncounterId,
                          int RegistrationNo, string EncounterNo, string BedNo, string PatientName,
                          DateTime dtsDateFrom, DateTime dtsDateTo, int EncodedBy, int OrderTypeId, string OrderStatusType,
                          int StoreId, string EncounterStatus, string PatientType, String IsApproved, string ShowOnlyDrugOrderForApproval)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
            string tDate = dtsDateTo.ToString("yyyy-MM-dd");

            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intEncounterId", EncounterId);
            if (RegistrationNo > 0)
            {
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
            }
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvBedNo", BedNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@chvFromDate", fDate);
            HshIn.Add("@chvToDate", tDate);
            HshIn.Add("@inyOrderTypeId", OrderTypeId);
            HshIn.Add("@chrPendingStatus", OrderStatusType);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@chvPatientType", PatientType);
            HshIn.Add("@IsApproved", IsApproved);
            HshIn.Add("@ShowOnlyDrugOrderForApproval", ShowOnlyDrugOrderForApproval);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshIn.Add("@chvEncounterStatus", EncounterStatus);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPPatientRequestInv", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int UpdatePickListPrintStatus(int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intIndentId", IndentId);
            try
            {
                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpdatePickListPrintStatus", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public bool PickListprintStatus(int IndentId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {


                bool val = false;

                HshIn.Add("@intIndentId", IndentId);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckPickListPrintStatus", HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = Convert.ToBoolean(ds.Tables[0].Rows[0]["Value"]);
                }

                return val;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }
        }


        public DataSet getAllStore(int HospId, int FacilityId, int @intEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@intEncodedBy", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetAllStore", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


    }
}
