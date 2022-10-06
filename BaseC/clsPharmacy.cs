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
        private string sConString = "";
        public clsPharmacy(string conString)
        {
            sConString = conString;
        }
        Hashtable HshIn;
        Hashtable HshOut;
        public DataSet GetPhrItemMasterMisc(int Id, string UseFor, int Active, int intFacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn = new Hashtable();
                HshIn.Add("@intId", Id);
                HshIn.Add("@Code", UseFor);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", intFacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterMisc", HshIn);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;

            }
        }
        public DataSet getStoreDepartment()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                string qry = "SELECT DISTINCT dm.DepartmentID AS StoreId, dm.DepartmentName AS StoreName " +
                            " FROM  DepartmentSub ds WITH (NOLOCK) INNER JOIN DepartmentType dt WITH (NOLOCK) ON dt.ID = ds.DepartmentTypeId AND dt.DepartmentType = 'ST' " +
                            " INNER JOIN DepartmentMain dm ON ds.DepartmentID = dm.DepartmentID ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public bool CheckDiagnosisPrimaryForPatient(int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intRegistrationId", RegistrationId);
            try
            {
                string strcount = objDl.ExecuteScalar(CommandType.Text, "select count(Id) from EMRPatientDiagnosisDetails WITH (NOLOCK) where RegistrationId=@intRegistrationId and PrimaryDiagnosis=1 and Active=1 ", HshIn).ToString();
                if (Convert.ToInt16(strcount) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getDepartmentType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                string qry = "SELECT DISTINCT Name as DepartmentType, ID as DepartmentId FROM DepartmentType WITH (NOLOCK) where DoctorRelated=1";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }


        }
        public DataSet getStoreDepartment(int HospId, int YearId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@YearId", YearId);

                string qry = "SELECT DISTINCT dm.DepartmentID AS StoreId, dm.DepartmentName AS StoreName " +
                            " FROM DepartmentMain dm WITH (NOLOCK) INNER JOIN PhrStoreMaster ps WITH (NOLOCK) on dm.DepartmentID=ps.StoreId " +
                            " WHERE ps.YearId = @YearId AND ps.Active = 1 AND ps.FacilityId = @FacilityId AND dm.HospitalLocationId = @HospId ORDER BY dm.DepartmentName  ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
        }
        public string GenerateUniqueNo()
        {

            string Uniqueid = string.Empty;

            Random Rand = new Random();

            Uniqueid = Convert.ToString(Rand.Next(100000, 999999));

            Uniqueid = Uniqueid + Convert.ToString(DateTime.Now.Year) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Day) +

                            Convert.ToString(DateTime.Now.Hour) + Convert.ToString(DateTime.Now.Minute) + Convert.ToString(DateTime.Now.Second);

            return Uniqueid;

        }
        public DataSet getStoreDepartmentAll(int HospId, int YearId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@YearId", YearId);

                string qry = "SELECT DISTINCT dm.DepartmentID AS StoreId, dm.DepartmentName AS StoreName " +
                            " FROM DepartmentMain dm WITH (NOLOCK) inner join PhrStoreMaster ps WITH (NOLOCK) on dm.DepartmentID=ps.StoreId " +
                            " WHERE ps.YearId = @YearId AND ps.Active = 1 ";

                if (FacilityId > 0)
                {
                    qry += " AND ps.FacilityId = @FacilityId ";
                }
                qry += " AND dm.HospitalLocationId = @HospId ORDER BY dm.DepartmentName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDepartmentMain(int HospId)
        {

            try
            {
                return getDepartmentMain(HospId, 0);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public DataSet getDepartmentMain(int HospId, int facilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityId", facilityId);

                string qry = "SELECT DepartmentID, DepartmentName,DepartmentIdendification FROM DepartmentMain WITH (NOLOCK) " +
                            " WHERE Active = 1 ";

                if (facilityId > 0)
                {
                    qry += " AND FacilityId = @FacilityId ";
                }
                qry += " AND HospitalLocationId = @HospId ORDER BY DepartmentName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDepartmentMain(int HospId, int facilityId, int OrderEntryStore)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();


                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityId", facilityId);
                HshIn.Add("@OrderEntryStore", OrderEntryStore);

                string qry = "SELECT DepartmentID, DepartmentName FROM  DepartmentMain WITH (NOLOCK) " +
                            " WHERE Active = 1 ";

                if (facilityId > 0)
                {
                    qry += " AND FacilityId = @FacilityId ";
                }
                if (OrderEntryStore > 0)
                {
                    qry += " AND OrderEntryStore = @OrderEntryStore ";
                }
                qry += " AND HospitalLocationId = @HospId ORDER BY DepartmentName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        //public string SaveSupplierMaster(string saveFor, int Id, int HospitalLocationId, string Name,
        //                    string ShortName, int SupplierTypeId, int SupplierCategoryTypeId,
        //                    int ChallanAccept, string Add1, string Add2, string Add3, string EMail,
        //                    int CountryId, int StateId, int CityId, int ZipId, string Mobile, string Phone, string Fax,
        //                    string ContactPerson, string Designation, string ContactPersonMobile,
        //                    string xmlTaxDetails, int Active, int EncodedBy, string xmlFacility, int ValidityPeriod,
        //                    string PaymentMode, int CreditMonths, int CreditDays, int VendorPostingGroupId)
        //{
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable HshOut = new Hashtable();

        //    if (saveFor == "SM")
        //    {
        //        HshIn.Add("@intSupplierId", Id);
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@chvSupplierName", Name.Trim());
        //        HshIn.Add("@chvSupplierShortName", ShortName.Trim());
        //        HshIn.Add("@intSupplierTypeId", SupplierTypeId);
        //        HshIn.Add("@intSupplierCategoryTypeId", SupplierCategoryTypeId);
        //        HshIn.Add("@bitChallanAccept", ChallanAccept);
        //        HshIn.Add("@chvAdd1", Add1.Trim());
        //        HshIn.Add("@chvAdd2", Add2.Trim());
        //        HshIn.Add("@chvAdd3", Add3.Trim());
        //        HshIn.Add("@chvEMail", EMail.Trim());
        //        HshIn.Add("@intCountryId", CountryId);
        //        HshIn.Add("@intStateId", StateId);
        //        HshIn.Add("@intCityId", CityId);
        //        HshIn.Add("@intZipId", ZipId);
        //        HshIn.Add("@chvMobile", Mobile.Trim());
        //        HshIn.Add("@chvPhone", Phone.Trim());
        //        HshIn.Add("@chvFax", Fax.Trim());
        //        HshIn.Add("@chvContactPerson", ContactPerson.Trim());
        //        HshIn.Add("@chvDesignation", Designation.Trim());
        //        HshIn.Add("@chvContactPersonMobile", ContactPersonMobile.Trim());
        //        HshIn.Add("@xmlTaxDetails", xmlTaxDetails);
        //        HshIn.Add("@xmlFacilityIds", xmlFacility);
        //        HshIn.Add("@chvPaymentMode", PaymentMode);
        //        HshIn.Add("@intCreditMonths", CreditMonths);
        //        HshIn.Add("@intCreditDays", CreditDays);
        //        HshIn.Add("@bitActive", Active);
        //        HshIn.Add("@intEncodedBy", EncodedBy);
        //        HshIn.Add("@intValidityPeriodInDays", ValidityPeriod);
        //        HshIn.Add("@intVendorPostingGroupId", VendorPostingGroupId);



        //        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveSupplierMaster", HshIn, HshOut);
        //    }
        //    else if (saveFor == "MM")
        //    {
        //        HshIn.Add("@intManufactureId", Id);
        //        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //        HshIn.Add("@chvManufactureName", Name.Trim());
        //        HshIn.Add("@chvManufactureShortName", ShortName.Trim());
        //        HshIn.Add("@chvAdd1", Add1.Trim());
        //        HshIn.Add("@chvAdd2", Add2.Trim());
        //        HshIn.Add("@chvAdd3", Add3.Trim());
        //        HshIn.Add("@chvEMail", EMail.Trim());
        //        HshIn.Add("@intCountryId", CountryId);
        //        HshIn.Add("@intStateId", StateId);
        //        HshIn.Add("@intCityId", CityId);
        //        HshIn.Add("@intZipId", ZipId);
        //        HshIn.Add("@chvMobile", Mobile.Trim());
        //        HshIn.Add("@chvPhone", Phone.Trim());
        //        HshIn.Add("@chvFax", Fax.Trim());
        //        HshIn.Add("@chvContactPerson", ContactPerson.Trim());
        //        HshIn.Add("@chvDesignation", Designation.Trim());
        //        HshIn.Add("@chvContactPersonMobile", ContactPersonMobile.Trim());
        //        HshIn.Add("@bitActive", Active);
        //        HshIn.Add("@intEncodedBy", EncodedBy);

        //        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrManufactureMaster", HshIn, HshOut);
        //    }

        //    return HshOut["@chvErrorStatus"].ToString();
        //}

        public string SaveSupplierMaster(string saveFor, int Id, int HospitalLocationId, string Name,
                            string ShortName, int SupplierTypeId, int SupplierCategoryTypeId,
                            int ChallanAccept, string Add1, string Add2, string Add3, string EMail,
                            int CountryId, int StateId, int CityId, int ZipId, string Mobile, string Phone, string Fax,
                            string ContactPerson, string Designation, string ContactPersonMobile,
                            string xmlTaxDetails, int Active, int EncodedBy, string xmlFacility, int ValidityPeriod,
                            string PaymentMode, int CreditMonths, int CreditDays, int VendorPostingGroupId, string MedicalReprsentativeName,
                            string AreaSalesManager, string ZonalSalesManage, string RegionalSalesManager, string DocNo)
        {
            return SaveSupplierMaster(saveFor, Id, HospitalLocationId, Name,
                             ShortName, SupplierTypeId, SupplierCategoryTypeId,
                             ChallanAccept, Add1, Add2, Add3, EMail,
                             CountryId, StateId, CityId, ZipId, Mobile, Phone, Fax,
                             ContactPerson, Designation, ContactPersonMobile,
                             xmlTaxDetails, Active, EncodedBy, xmlFacility, ValidityPeriod,
                             PaymentMode, CreditMonths, CreditDays, VendorPostingGroupId, MedicalReprsentativeName,
                             AreaSalesManager, ZonalSalesManage, RegionalSalesManager, DocNo, false);
        }

        public string SaveSupplierMaster(string saveFor, int Id, int HospitalLocationId, string Name,
                            string ShortName, int SupplierTypeId, int SupplierCategoryTypeId,
                            int ChallanAccept, string Add1, string Add2, string Add3, string EMail,
                            int CountryId, int StateId, int CityId, int ZipId, string Mobile, string Phone, string Fax,
                            string ContactPerson, string Designation, string ContactPersonMobile,
                            string xmlTaxDetails, int Active, int EncodedBy, string xmlFacility, int ValidityPeriod,
                            string PaymentMode, int CreditMonths, int CreditDays, int VendorPostingGroupId, string MedicalReprsentativeName,
                            string AreaSalesManager, string ZonalSalesManage, string RegionalSalesManager, string DocNo, bool bIsCSSDManufacture)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                if (saveFor == "SM")
                {
                    HshIn.Add("@intSupplierId", Id);
                    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                    HshIn.Add("@chvSupplierName", Name.Trim());
                    HshIn.Add("@chvSupplierShortName", ShortName.Trim());
                    HshIn.Add("@intSupplierTypeId", SupplierTypeId);
                    HshIn.Add("@intSupplierCategoryTypeId", SupplierCategoryTypeId);
                    HshIn.Add("@bitChallanAccept", ChallanAccept);
                    HshIn.Add("@chvAdd1", Add1.Trim());
                    HshIn.Add("@chvAdd2", Add2.Trim());
                    HshIn.Add("@chvAdd3", Add3.Trim());
                    HshIn.Add("@chvEMail", EMail.Trim());
                    HshIn.Add("@intCountryId", CountryId);
                    HshIn.Add("@intStateId", StateId);
                    HshIn.Add("@intCityId", CityId);
                    HshIn.Add("@intZipId", ZipId);
                    HshIn.Add("@chvMobile", Mobile.Trim());
                    HshIn.Add("@chvPhone", Phone.Trim());
                    HshIn.Add("@chvFax", Fax.Trim());
                    HshIn.Add("@chvContactPerson", ContactPerson.Trim());
                    HshIn.Add("@chvDesignation", Designation.Trim());
                    HshIn.Add("@chvContactPersonMobile", ContactPersonMobile.Trim());
                    HshIn.Add("@xmlTaxDetails", xmlTaxDetails);
                    HshIn.Add("@xmlFacilityIds", xmlFacility);
                    HshIn.Add("@vPreDocNo", DocNo);
                    HshIn.Add("@chvPaymentMode", PaymentMode);
                    HshIn.Add("@intCreditMonths", CreditMonths);
                    HshIn.Add("@intCreditDays", CreditDays);
                    HshIn.Add("@bitActive", Active);
                    HshIn.Add("@intEncodedBy", EncodedBy);
                    HshIn.Add("@intValidityPeriodInDays", ValidityPeriod);
                    HshIn.Add("@intVendorPostingGroupId", VendorPostingGroupId);

                    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveSupplierMaster", HshIn, HshOut);
                }
                else if (saveFor == "MM")
                {
                    HshIn.Add("@intManufactureId", Id);
                    HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                    HshIn.Add("@chvManufactureName", Name.Trim());
                    HshIn.Add("@chvManufactureShortName", ShortName.Trim());
                    HshIn.Add("@chvAdd1", Add1.Trim());
                    HshIn.Add("@chvAdd2", Add2.Trim());
                    HshIn.Add("@chvAdd3", Add3.Trim());
                    HshIn.Add("@chvEMail", EMail.Trim());
                    HshIn.Add("@intCountryId", CountryId);
                    HshIn.Add("@intStateId", StateId);
                    HshIn.Add("@intCityId", CityId);
                    HshIn.Add("@intZipId", ZipId);
                    HshIn.Add("@chvMobile", Mobile.Trim());
                    HshIn.Add("@chvPhone", Phone.Trim());
                    HshIn.Add("@chvFax", Fax.Trim());

                    HshIn.Add("@chvContactPerson", ContactPerson.Trim());
                    HshIn.Add("@chvDesignation", Designation.Trim());
                    HshIn.Add("@chvContactPersonMobile", ContactPersonMobile.Trim());
                    HshIn.Add("@vPreDocNo", DocNo);
                    HshIn.Add("@bitActive", Active);
                    HshIn.Add("@intEncodedBy", EncodedBy);

                    HshIn.Add("@MedicalRepresentativesName", MedicalReprsentativeName);
                    HshIn.Add("@AreaSalesManager ", AreaSalesManager);
                    HshIn.Add("@ZonalSalesManager", ZonalSalesManage);
                    HshIn.Add("@RegionalSalesManager", RegionalSalesManager);

                    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                    HshIn.Add("@bitIsCSSDManufacture", bIsCSSDManufacture);
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrManufactureMaster", HshIn, HshOut);
                }

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }



        public DataSet uspPhrGetItemBrandMaster(int HospId, int BrandId, int Active, string Name, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intItemBrandId", BrandId);
                HshIn.Add("@chvItemBrandName", Name);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemBrandMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string uspPhrSaveItemBrandMaster(int HospId, int BrandId, string Name, int GenericId, int ManufactureId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intItemBrandId", BrandId);
                HshIn.Add("@chvItemBrandName", Name);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@intManufactureId", ManufactureId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveItemBrandMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getSupplierMaster(int LoginFacilityId, int Id, int HospId, int Active, string docNo,
                                            string Name, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intSupplierId", Id);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chvSupplierNo", docNo);
                HshIn.Add("@chvSupplierName", Name);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetSupplierMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getSupplierMasterList(int LoginFacilityId, int Id, int HospId, int Active, string docNo,
                                            string Name, int EncodedBy, string SupplierType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intSupplierId", Id);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chvSupplierNo", docNo);
                HshIn.Add("@chvSupplierName", Name);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@chrSupplierType", SupplierType);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrSupplierMasterList", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getManufactureMaster(int Id, int HospId, int Active, string docNo,
                                            string Name, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intManufactureId", Id);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chvManufactureNo", docNo);
                HshIn.Add("@chvManufactureName", Name);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrManufactureMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getManufactureMasterList(int Id, int HospId, int Active, string docNo, string Name, int EncodedBy)
        {
            return getManufactureMasterList(Id, HospId, Active, docNo, Name, EncodedBy, false);
        }

        public DataSet getManufactureMasterList(int Id, int HospId, int Active, string docNo, string Name, int EncodedBy, bool bitIsCSSDManufacture)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intManufactureId", Id);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chvManufactureNo", docNo);
                HshIn.Add("@chvManufactureName", Name);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitIsCSSDManufacture", bitIsCSSDManufacture);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrManufactureMasterList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getSupplierTax(int SupplierId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@chvTaxType", "TaxType");
                HshIn.Add("@intSupplierId", SupplierId);

                string qry = "SELECT s.StatusId, s.[Status], std.TaxValue " +
                            " FROM GetStatus(1, @chvTaxType) s " +
                            " LEFT OUTER JOIN PhrSupplierTaxDetails std WITH (NOLOCK) ON s.StatusId = std.TaxId AND std.Active = 1 AND std.SupplierId = @intSupplierId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getSupplierManufactureTagging(int SupplierId, bool OnlyTagged, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@inyHospId", HospId);

                string qry = "SELECT (CASE WHEN ISNULL(smt.ManufactureId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
                            " sm.ManufactureId, sm.ManufactureNo, sm.ManufactureName " +
                            " FROM PhrManufactureMaster sm WITH (NOLOCK) " +
                            " LEFT OUTER JOIN PhrSupplierManufTagging smt WITH (NOLOCK) ON sm.ManufactureId = smt.ManufactureId AND smt.Active = 1 AND smt.SupplierId = @intSupplierId " +
                            " WHERE sm.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(smt.ManufactureId, 0) <> 0 ";
                }

                qry += " AND sm.HospitalLocationId = @inyHospId ORDER BY sm.ManufactureName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveSupplierManufTagging(int SupplierId, string ManufactureIds, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intSupplierId", SupplierId);
            HshIn.Add("@xmlManufactureIds", ManufactureIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrSupplierManufTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveItemCategoryMaster(int ItemCategoryId, string ItemCategoryName, string ShortName, int CategoryTypeId,
                                            int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemCategoryId", ItemCategoryId);
            HshIn.Add("@chvItemCategoryName", ItemCategoryName.Trim());
            HshIn.Add("@chvItemCategoryShortName", ShortName);
            HshIn.Add("@intCategoryTypeId", CategoryTypeId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemCategoryMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemCategoryMaster(int ItemCategoryId, int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intItemCategoryId", ItemCategoryId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemCategoryMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //public DataSet getItemCategoryMaster(int ItemCategoryId, int HospId, int Active, int EncodedBy, string StoreId)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet ds = new DataSet();

        //    HshIn.Add("@intItemCategoryId", ItemCategoryId);
        //    HshIn.Add("@inyHospitalLocationId", HospId);
        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@intEncodedBy", EncodedBy);
        //    HshIn.Add("@strStoreId", StoreId);            
        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemCategoryMaster", HshIn, HshOut);

        //   
        //}

        public string SaveItemCategoryDetails(int ItemSubCategoryId, string ItemSubCategoryName, string ItemSubCategoryShortName,
                                        int ItemCategoryId, int IsUnderSubCategory, int MainParentId, int ParentId, int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
            HshIn.Add("@chvItemSubCategoryName", ItemSubCategoryName.Trim());
            HshIn.Add("@chvItemSubCategoryShortName", ItemSubCategoryShortName.Trim());
            HshIn.Add("@intItemCategoryId", ItemCategoryId);
            HshIn.Add("@bitIsUnderSubCategory", IsUnderSubCategory);
            HshIn.Add("@intMainParentId", MainParentId);
            HshIn.Add("@intParentId", ParentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemCategoryDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemCategoryDetails(int ItemSubCategoryId, string ItemSubCategoryName, int ItemCategoryId, int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
                HshIn.Add("@chvItemSubCategoryName", ItemSubCategoryName.Trim());
                HshIn.Add("@intItemCategoryId", ItemCategoryId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemCategoryDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        //public DataSet getItemCategoryDetailsLeafCat(int ItemCategoryId, int HospId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@intItemCategoryId", ItemCategoryId);
        //        HshIn.Add("@inyHospId", HospId);

        //        string qry = "SELECT sm.ItemSubCategoryId, sm.ItemSubCategoryName, " +
        //                " sm.ItemCategoryId, cm.ItemCategoryName, sm.ParentId " +
        //                " FROM PhrItemCategoryDetails sm " +
        //                " INNER JOIN PhrItemCategoryMaster cm ON sm.ItemCategoryId = cm.ItemCategoryId " +
        //                " WHERE sm.ItemSubCategoryId NOT IN (SELECT ISNULL(ParentId, 0) FROM PhrItemCategoryDetails) " +
        //                " AND sm.Active = 1 AND cm.HospitalLocationId = @inyHospId " +
        //                " AND ISNULL(sm.ItemCategoryId,0) = (CASE WHEN ISNULL(@intItemCategoryId, 0) = 0 THEN ISNULL(sm.ItemCategoryId,0) ELSE ISNULL(@intItemCategoryId, 0) END) " +
        //                " ORDER BY sm.ItemSubCategoryName ";

        //        return objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}

        public string SaveValuesMaster(int ValueId, string ValueName, int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intValueId", ValueId);
            HshIn.Add("@chvValueName", ValueName.Trim());
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrValuesMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getValuesMaster(int ValueId, int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intValueId", ValueId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrValuesMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveGroupValue(int GroupId, string GroupName, int HospId, string xmlGroupValues, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@chvGroupName", GroupName.Trim());
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@xmlGroupValues", xmlGroupValues);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrGroupValue", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getGroupValueDetails(int GroupId, int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrGroupValueDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getGroupValueMain(int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrGroupValueMain", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public int DeActivateValueGroupDetails(int GroupId, int ValueId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@ValueId", ValueId);

                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE PhrGroupValueDetails SET Active = '0' WHERE GroupId = @GroupId AND ValueId = @ValueId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public int ActivateValueDetails(int GroupId, int ValueId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@ValueId", ValueId);

                return objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE PhrGroupValueDetails SET Active = '1' WHERE GroupId=@GroupId AND ValueId = @ValueId", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveItemFieldMaster(int FieldId, string FieldName, string FieldCode, string FieldType,
                                 int FieldLength, int GroupId, int DecimalPlaces, int HospId, int Active, int EncodedBy, string MasterOption)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intFieldId", FieldId);
            HshIn.Add("@chvFieldName", FieldName.Trim());
            HshIn.Add("@chvFieldCode", FieldCode.Trim());
            HshIn.Add("@chvFieldType", FieldType.Trim());
            HshIn.Add("@intFieldLength", FieldLength);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@intDecimalPlaces", DecimalPlaces);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@MasterOption", MasterOption);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemFieldMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemFieldMaster(int FieldId, int Active, int HospId, int EncodedBy, string MasterOption)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intFieldId", FieldId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@MasterOption", MasterOption);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemFieldMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        //public string SaveItemMaster(int ItemBrandId, int HospId,
        //                    int FormulationId, string xmlStrengthIds, int EncodedBy)
        //{
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable HshOut = new Hashtable();

        //    HshIn.Add("@intItemBrandId", ItemBrandId);
        //    HshIn.Add("@inyHospitalLocationId", HospId);
        //    HshIn.Add("@intFormulationId", FormulationId);
        //    HshIn.Add("@xmlStrengthIds", xmlStrengthIds);
        //    HshIn.Add("@intEncodedBy", EncodedBy);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemMaster", HshIn, HshOut);

        //    return HshOut["@chvErrorStatus"].ToString();
        //}

        //public string SaveItemMasterDetails(int ItemId, int HospId, int FacilityId,
        //                            int RequestedFacilityId, int ShelfLifeYears, int ShelfLifeMonths, int ShelfLifeDays, int RecommendedBy,
        //                            int IsFractionalIssue, int IsProfile, int IsVatInclude, string Specification, byte[] ItemImage, string ItemImageName,
        //                            string xmlItemWithItemUnitIds, string xmlItemChargeDetails,
        //    string xmlItemFieldDetails, int Active, int EncodedBy, bool PanelpriceRequired, bool Reusable,double VatOnSale)
        //{
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable HshOut = new Hashtable();

        //    HshIn.Add("@intItemId", ItemId);
        //    HshIn.Add("@inyHospitalLocationId", HospId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@intRequestedFacilityId", RequestedFacilityId);
        //    HshIn.Add("@intShelfLifeYears", ShelfLifeYears);
        //    HshIn.Add("@intShelfLifeMonths", ShelfLifeMonths);
        //    HshIn.Add("@intShelfLifeDays", ShelfLifeDays);
        //    HshIn.Add("@intRecommendedBy", RecommendedBy);
        //    HshIn.Add("@bitIsFractionalIssue", IsFractionalIssue);
        //    HshIn.Add("@bitIsProfile", IsProfile);
        //    HshIn.Add("@bitIsVatInclude", IsVatInclude);
        //    HshIn.Add("@chvSpecification", Specification.Trim());

        //    HshIn.Add("@xmlItemWithItemUnitIds", xmlItemWithItemUnitIds);
        //    HshIn.Add("@xmlItemChargeDetails", xmlItemChargeDetails);
        //    HshIn.Add("@xmlItemFieldDetails", xmlItemFieldDetails);

        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@intEncodedBy", EncodedBy);
        //    HshIn.Add("@bitPanelpriceRequired", PanelpriceRequired);
        //    HshIn.Add("@bitReusable", Reusable);
        //    HshIn.Add("@VatOnSale", VatOnSale);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemMasterDetails", HshIn, HshOut);

        //    #region Update Image
        //    if (ItemImageName.Trim() != "")
        //    {
        //        SqlConnection con = new SqlConnection(sConString);
        //        SqlCommand cmdTemp;
        //        SqlParameter prm1, prm2, prm3, prm4, prm5;

        //        string qry = "";

        //        qry = "IF EXISTS(SELECT Id FROM PhrItemMasterImage WHERE ItemId = @intItemId) " +
        //                    " BEGIN " +
        //                    " UPDATE PhrItemMasterImage SET ItemImage = @imgItemImage, ItemImageName = @chvItemImageName, " +
        //                    " Active = @bitActive, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() " +
        //                    " WHERE ItemId = @intItemId " +
        //                    " END " +
        //                    " ELSE " +
        //                    " BEGIN " +
        //                    " INSERT INTO PhrItemMasterImage(ItemId, ItemImage, ItemImageName, Active, EncodedBy, EncodedDate)     " +
        //                    " SELECT @intItemId, @imgItemImage, @chvItemImageName, 1, @intEncodedBy, GETUTCDATE() " +
        //                    " END";

        //        cmdTemp = new SqlCommand(qry, con);
        //        cmdTemp.CommandType = CommandType.Text;

        //        prm1 = new SqlParameter();
        //        prm1.ParameterName = "@intItemId";
        //        prm1.Value = Convert.ToInt32(ItemId);
        //        prm1.SqlDbType = SqlDbType.Int;
        //        prm1.Direction = ParameterDirection.Input;
        //        cmdTemp.Parameters.Add(prm1);

        //        prm2 = new SqlParameter();
        //        prm2.ParameterName = "@imgItemImage";
        //        prm2.Value = ItemImage;
        //        prm2.SqlDbType = SqlDbType.Image;
        //        prm2.Direction = ParameterDirection.Input;
        //        cmdTemp.Parameters.Add(prm2);

        //        prm3 = new SqlParameter();
        //        prm3.ParameterName = "@chvItemImageName";
        //        prm3.Value = ItemImageName;
        //        prm3.SqlDbType = SqlDbType.VarChar;
        //        prm3.Direction = ParameterDirection.Input;
        //        cmdTemp.Parameters.Add(prm3);

        //        prm4 = new SqlParameter();
        //        prm4.ParameterName = "@intEncodedBy";
        //        prm4.Value = EncodedBy;
        //        prm4.SqlDbType = SqlDbType.Int;
        //        prm4.Direction = ParameterDirection.Input;
        //        cmdTemp.Parameters.Add(prm4);

        //        prm5 = new SqlParameter();
        //        prm5.ParameterName = "@bitActive";
        //        prm5.Value = Active;
        //        prm5.SqlDbType = SqlDbType.Bit;
        //        prm5.Direction = ParameterDirection.Input;
        //        cmdTemp.Parameters.Add(prm5);

        //        con.Open();
        //        cmdTemp.ExecuteNonQuery();
        //        con.Close();
        //    }
        //    #endregion

        //    return HshOut["@chvErrorStatus"].ToString();
        //}


        public string SaveItemMasterDetails(int ItemId, int HospId, int FacilityId,
                                    int RequestedFacilityId, int ShelfLifeYears, int ShelfLifeMonths, int ShelfLifeDays, int RecommendedBy,
                                    int IsFractionalIssue, int IsProfile, int IsVatInclude, string Specification, byte[] ItemImage, string ItemImageName,
                                    string xmlItemWithItemUnitIds, string xmlItemChargeDetails,
                                    string xmlItemFieldDetails, int Active, int EncodedBy, bool PanelpriceRequired, bool Reusable, double VatOnSale,
                                    string Rack, bool Consumable, bool Useforbolpatient, bool IsSubstituteNotAllowed, int DepreciationDays, int DepreciationPercentage,
                                    double DrugDose, int UnitId, int FormulationId, int RouteId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestedFacilityId", RequestedFacilityId);
            HshIn.Add("@intShelfLifeYears", ShelfLifeYears);
            HshIn.Add("@intShelfLifeMonths", ShelfLifeMonths);
            HshIn.Add("@intShelfLifeDays", ShelfLifeDays);
            HshIn.Add("@intRecommendedBy", RecommendedBy);
            HshIn.Add("@bitIsFractionalIssue", IsFractionalIssue);
            HshIn.Add("@bitIsProfile", IsProfile);
            HshIn.Add("@bitIsVatInclude", IsVatInclude);
            HshIn.Add("@chvSpecification", Specification.Trim());

            HshIn.Add("@xmlItemWithItemUnitIds", xmlItemWithItemUnitIds);
            HshIn.Add("@xmlItemChargeDetails", xmlItemChargeDetails);
            HshIn.Add("@xmlItemFieldDetails", xmlItemFieldDetails);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@bitPanelpriceRequired", PanelpriceRequired);
            HshIn.Add("@bitReusable", Reusable);
            HshIn.Add("@VatOnSale", VatOnSale);
            HshIn.Add("@bitConsumable", Consumable);
            HshIn.Add("@bitUseforbplpatient", Useforbolpatient);
            HshIn.Add("@Rack", Rack);
            HshIn.Add("@bitIsSubstituteNotAllowed", IsSubstituteNotAllowed);
            HshIn.Add("@intDepreciationDays", DepreciationDays);
            HshIn.Add("@intDepreciationPercentage", DepreciationPercentage);
            HshIn.Add("@decDrugDose", DrugDose);
            HshIn.Add("@intUnitId", UnitId);
            HshIn.Add("@intFormulationId", FormulationId);
            HshIn.Add("@intRouteId", RouteId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemMasterDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


            #region Update Image
            if (ItemImageName.Trim() != "")
            {
                SqlConnection con = new SqlConnection(sConString);
                SqlCommand cmdTemp;
                SqlParameter prm1, prm2, prm3, prm4, prm5;

                string qry = "";

                qry = "IF EXISTS(SELECT Id FROM PhrItemMasterImage WITH (NOLOCK) WHERE ItemId = @intItemId) " +
                            " BEGIN " +
                            " UPDATE PhrItemMasterImage SET ItemImage = @imgItemImage, ItemImageName = @chvItemImageName, " +
                            " Active = @bitActive, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() " +
                            " WHERE ItemId = @intItemId " +
                            " END " +
                            " ELSE " +
                            " BEGIN " +
                            " INSERT INTO PhrItemMasterImage(ItemId, ItemImage, ItemImageName, Active, EncodedBy, EncodedDate) " +
                            " SELECT @intItemId, @imgItemImage, @chvItemImageName, 1, @intEncodedBy, GETUTCDATE() " +
                            " END";

                cmdTemp = new SqlCommand(qry, con);
                cmdTemp.CommandType = CommandType.Text;

                prm1 = new SqlParameter();
                prm1.ParameterName = "@intItemId";
                prm1.Value = Convert.ToInt32(ItemId);
                prm1.SqlDbType = SqlDbType.Int;
                prm1.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm1);

                prm2 = new SqlParameter();
                prm2.ParameterName = "@imgItemImage";
                prm2.Value = ItemImage;
                prm2.SqlDbType = SqlDbType.Image;
                prm2.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm2);

                prm3 = new SqlParameter();
                prm3.ParameterName = "@chvItemImageName";
                prm3.Value = ItemImageName;
                prm3.SqlDbType = SqlDbType.VarChar;
                prm3.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm3);

                prm4 = new SqlParameter();
                prm4.ParameterName = "@intEncodedBy";
                prm4.Value = EncodedBy;
                prm4.SqlDbType = SqlDbType.Int;
                prm4.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm4);

                prm5 = new SqlParameter();
                prm5.ParameterName = "@bitActive";
                prm5.Value = Active;
                prm5.SqlDbType = SqlDbType.Bit;
                prm5.Direction = ParameterDirection.Input;
                cmdTemp.Parameters.Add(prm5);

                con.Open();
                cmdTemp.ExecuteNonQuery();
            }
            #endregion

            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getItemMaster(int ItemId, string ItemNo, string ItemSearchName, int Active, int HospId, int EncodedBy)
        {
            return getItemMaster(ItemId, ItemNo, ItemSearchName, Active, HospId, EncodedBy, 3);
        }

        public DataSet getItemMaster(int ItemId, string ItemNo, string ItemSearchName, int Active, int HospId, int EncodedBy, int iOT)
        {
            return getItemMaster(ItemId, ItemNo, ItemSearchName, Active, HospId, EncodedBy, iOT, 0, 0);
        }

        public DataSet getItemMaster(int ItemId, string ItemNo, string ItemSearchName, int Active, int HospId, int EncodedBy,
                                    int iOT, int StoreId, int FacilityId)
        {
            return getItemMaster(ItemId, ItemNo, ItemSearchName, Active, HospId, EncodedBy,
                                 iOT, StoreId, FacilityId, string.Empty);
        }

        public DataSet getItemMaster(int ItemId, string ItemNo, string ItemSearchName, int Active, int HospId, int EncodedBy,
                                    int iOT, int StoreId, int FacilityId, string UseFor)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvItemNo", ItemNo);
                HshIn.Add("@chvItemSearch", ItemSearchName);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intOT", iOT);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (!UseFor.Equals(string.Empty))
                {
                    HshIn.Add("@chrSource", UseFor);
                }

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getItemList(int ItemId, int SupplierId, string ItemSearch,
                                int HospId, int EncodedBy)
        {
            return getItemList(ItemId, SupplierId, ItemSearch, 0, HospId, EncodedBy, 1);
        }

        public DataSet getItemList(int ItemId, int SupplierId, string ItemSearch,
                                    int StoreId, int HospId, int EncodedBy, int Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@chvItemSearch", ItemSearch);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intSupplierId", SupplierId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@bitActive", Active);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterList", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet ISCalculationRequired(int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string strsql = "";

                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);

                strsql = " SELECT im.ItemId,ISNULL(pfm.IsCalculated,0) AS CalculationRequired FROM PhrItemMaster im WITH (NOLOCK) " +
                        " LEFT JOIN PhrFormulationMaster pfm WITH (NOLOCK) ON im.FormulationId=pfm.FormulationId  " +
                        " WHERE im.ItemId=@intItemId ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getItemListForDepIndent(int ItemId, int SupplierId, string ItemSearch,
                                    int StoreId, int HospId, int EncodedBy, int Active, int ToFacilityId, int bitWithStockOnly, int LoginStorId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvItemSearch", ItemSearch);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", ToFacilityId);
                HshIn.Add("@bitWithStockOnly", bitWithStockOnly);
                HshIn.Add("@intLoginStoreId", LoginStorId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getItemChargeDetails(int ItemId, bool OnlyTagged)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);

                string qry = "SELECT chs.ChargeId, chs.ChargeName, icd.ChargeValue " +
                            " FROM PhrChargeSetup chs WITH (NOLOCK) " +
                            " LEFT OUTER JOIN PhrItemChargeDetails icd WITH (NOLOCK) ON chs.ChargeId = icd.ChargeId AND icd.Active = 1 AND icd.ItemId = @intItemId " +
                            " WHERE chs.ChargeType ='M' AND chs.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(icd.ChargeId, 0) <> 0 ";
                }

                qry += " ORDER BY chs.SequenceNo";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getItemFraction(string ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                //string qry = "select ItemId ,IsFractionalIssue  from PhrItemMaster where ItemId in ("+ItemId+")"
                // + " and IsFractionalIssue = 0 and Active=1 ";   
                //return objDl.FillDataSet(CommandType.Text, qry);
                HshIn.Add("@strItemId", ItemId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDecimalItem", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getItemFieldDetails(int ItemId, bool OnlyTagged, string MasterOption)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@MasterOption", MasterOption);



                string qry = "SELECT ifm.FieldId, ifm.FieldName, ifm.FieldType, " +
                            " ifm.FieldLength, ifm.GroupId, ifm.DecimalPlaces, ifd.ValueText, ifd.ValueId, ifd.ValueWordProcessor " +
                            " FROM PhrCustomFieldsMaster ifm WITH (NOLOCK) LEFT OUTER JOIN PhrItemFieldDetails ifd WITH (NOLOCK) ON ifm.FieldId = ifd.FieldId AND ifd.Active = 1 AND ifd.ItemId = @intItemId " +
                            " WHERE ifm.Active = 1 AND MasterOption=@MasterOption ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(ifd.FieldId, 0) <> 0 ";
                }

                qry += " ORDER BY ifm.SequenceNo";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getSupCustomFieldDetails(int SupplierId, bool OnlyTagged, string MasterOption)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@SupplierId", SupplierId);
                HshIn.Add("@MasterOption", MasterOption);

                string qry = "SELECT ifm.FieldId, ifm.FieldName, ifm.FieldType, " +
                            " ifm.FieldLength, ifm.GroupId, ifm.DecimalPlaces, ifd.ValueText, ifd.ValueId, ifd.ValueWordProcessor " +
                            " FROM PhrCustomFieldsMaster ifm WITH (NOLOCK) LEFT OUTER JOIN PhrSupCustomFieldDetails ifd WITH (NOLOCK) ON ifm.FieldId = ifd.FieldId AND ifd.Active = 1 AND ifd.SupplierId = @SupplierId " +
                            " WHERE ifm.Active = 1 AND MasterOption=@MasterOption";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(ifd.SupplierId, 0) <> 0 ";
                }

                qry += " ORDER BY ifm.SequenceNo";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveStoreItemSubGroupTagging(int StoreId, string xmlItemSubCategoryIds, int IsForDelete, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@xmlItemSubCategoryIds", xmlItemSubCategoryIds);
            HshIn.Add("@bitIsForDelete", IsForDelete);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrStoreItemSubGroupTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getStoreItemSubGroupTagging(int StoreId, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@inyHospId", HospId);

                string qry = "SELECT sgt.ItemSubCategoryId, icd.ItemSubCategoryName, icd2.ItemSubCategoryName AS MainSubGroup, cm.ItemCategoryName " +
                            " FROM PhrStoreItemSubGroupTagging sgt WITH (NOLOCK) " +
                            " INNER JOIN PhrItemCategoryDetails icd WITH (NOLOCK) ON sgt.ItemSubCategoryId = icd.ItemSubCategoryId " +
                            " INNER JOIN PhrItemCategoryMaster cm WITH (NOLOCK) ON icd.ItemCategoryId = cm.ItemCategoryId " +
                            " LEFT OUTER JOIN PhrItemCategoryDetails icd2 WITH (NOLOCK) ON icd.MainParentId = icd2.ItemSubCategoryId " +
                            " WHERE sgt.StoreId = @intStoreId AND sgt.Active = 1 AND sgt.HospitalLocationId = @inyHospId " +
                            " ORDER BY icd2.ItemSubCategoryName, icd.ItemSubCategoryName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveStoreItemTagging(int StoreId, string xmlItemIds, int IsForDelete, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@xmlItemIds", xmlItemIds);
            HshIn.Add("@bitIsForDelete", IsForDelete);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrStoreItemTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getStoreItemTagging(int StoreId, bool OnlyTagged, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@inyHospId", HospId);

                string qry = "SELECT (CASE WHEN ISNULL(sit.ItemId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
                            " im.ItemId, im.ItemName " +
                            " FROM PhrItemMaster im WITH (NOLOCK) " +
                            " LEFT OUTER JOIN PhrStoreItemTagging sit WITH (NOLOCK) ON im.ItemId = sit.ItemId AND sit.Active = 1 AND sit.StoreId = @intStoreId " +
                            " WHERE im.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(sit.ItemId, 0) <> 0 ";
                }

                qry += " AND im.HospitalLocationId = @inyHospId  ORDER BY im.ItemName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveItemSupplierTagging(int ItemId, string xmlSupplierIds, int HospId, int FacilityId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@xmlSupplierIds", xmlSupplierIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemSupplierTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemSupplierTagging(int ItemId, bool OnlyTagged, int HospId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@inyHospId", HospId);
                HshIn.Add("@inyFacilityId", FacilityId);

                string qry = "SELECT (CASE WHEN ISNULL(ist.ItemId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
                            " sm.SupplierId, sm.SupplierName, ist.Priority  " +
                            " FROM PhrSupplierMaster sm WITH (NOLOCK) " +
                            " LEFT OUTER JOIN PhrItemSupplierTagging ist WITH (NOLOCK) ON sm.SupplierId = ist.SupplierId AND ist.Active = 1 AND ist.ItemId = @intItemId " +
                            " WHERE sm.Active = 1 AND ist.FacilityId = @inyFacilityId ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(ist.ItemId, 0) <> 0 ";
                }

                qry += " AND sm.HospitalLocationId = @inyHospId ORDER BY ist.Priority";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }


        }

        public string SaveItemFlagTagging(int ItemId, string xmlItemFlagIds, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@xmlItemFlagIds", xmlItemFlagIds);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemFlagTagging", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemWithItemUnitTagging(int ItemId, bool OnlyTagged)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);

                string qry = "SELECT (CASE WHEN ISNULL(tr.ItemId, 0) = 0 THEN 0 ELSE 1 END) AS IsChk, " +
                            " (CASE WHEN ISNULL(tr.IsDefault, 0) = 0 THEN 0 ELSE 1 END) AS IsDefault, " +
                            " mt.ItemUnitId, mt.ItemUnitName, mt.PurchaseUnitId1, um1.UnitName AS PurchaseUnit1, " +
                            " mt.PurchaseUnitId2, um2.UnitName AS PurchaseUnit2, mt.IssueUnitId, um3.UnitName AS IssueUnit, " +
                            " mt.PackingId, pm.PackingName " +
                            " FROM PhrItemUnitMaster mt WITH (NOLOCK) " +
                            " LEFT JOIN PhrUnitMaster um1 WITH (NOLOCK) ON mt.PurchaseUnitId1 = um1.UnitId " +
                            " LEFT JOIN PhrUnitMaster um2 WITH (NOLOCK) ON mt.PurchaseUnitId2 = um2.UnitId " +
                            " LEFT JOIN PhrUnitMaster um3 WITH (NOLOCK) ON mt.IssueUnitId = um3.UnitId " +
                            " LEFT OUTER JOIN PhrItemWithItemUnitTagging tr WITH (NOLOCK) ON mt.ItemUnitId = tr.ItemUnitId AND tr.Active = 1 AND tr.ItemId = @intItemId " +
                            " LEFT OUTER JOIN PhrPackingMaster pm WITH (NOLOCK) ON mt.PackingId = pm.PackingId " +
                            " WHERE mt.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(tr.ItemId, 0) <> 0 ";
                }

                qry += " ORDER BY IsChk DESC, mt.ItemUnitName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getItemUnit(int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);

                string qry = "SELECT um.UnitId, um.UnitName FROM PhrItemWithItemUnitTagging iut WITH (NOLOCK) " +
                            " INNER JOIN PhrItemUnitMaster ium WITH (NOLOCK) ON ium.ItemUnitId = iut.ItemUnitId " +
                            " INNER JOIN PhrUnitMaster um WITH (NOLOCK) ON um.UnitId = ium.IssueUnitId WHERE iut.ItemId = @intItemId AND iut.IsDefault = 1 AND iut.Active = 1";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet getItemFlagTagging(int ItemId, bool OnlyTagged, int HospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@inyHospId", HospId);

                string qry = "SELECT (CASE WHEN ISNULL(ift.ItemId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
                            " ifm.ItemFlagId, ifm.ItemFlagName " +
                            " FROM PhrItemFlagMaster ifm WITH (NOLOCK) " +
                            " LEFT OUTER JOIN PhrItemFlagTagging ift WITH (NOLOCK) ON ifm.ItemFlagId = ift.ItemFlagId AND ift.Active = 1 AND ift.ItemId = @intItemId " +
                            " WHERE ifm.Active = 1 ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(ift.ItemId, 0) <> 0 ";
                }

                qry += " AND ifm.HospitalLocationId = @inyHospId ORDER BY ifm.ItemFlagName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet getTaggedItemStrength(int ItemBrandId, int FormulationId, bool OnlyTagged, int HospId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@intItemBrandId", ItemBrandId);
        //        HshIn.Add("@intFormulationId", FormulationId);
        //        HshIn.Add("@inyHospId", HospId);

        //        string qry = "SELECT (CASE WHEN ISNULL(X1.StrengthId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
        //                    " ism.StrengthId,  ISNULL(ism.StrengthValue, '') + ISNULL(' ' + ium.UnitName, '') AS Strength " +
        //                    " FROM PhrItemStrengthMaster ism " +
        //                    " INNER JOIN PhrUnitMaster ium ON ium.UnitId = ism.StrengthUnitId " +
        //                    " LEFT OUTER JOIN (SELECT StrengthId FROM PhrItemMaster WHERE Active = 1 AND ItemBrandId = @intItemBrandId AND FormulationId = @intFormulationId)X1 ON ism.StrengthId = X1.StrengthId " +
        //                    " WHERE ism.Active = 1 ";

        //        if (OnlyTagged)
        //        {
        //            qry += " AND ISNULL(X1.StrengthId, 0) <> 0 ";
        //        }

        //        qry += " ORDER BY ium.UnitName, ism.StrengthValue";

        //        return objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}

        //public string SavePOMain(int PurchaseOrderId, int HospId, int StoreId, int FacilityId, DateTime PurchaseOrderDate,
        //                    int SupplierId, double NetAmt, int PaymentMonths, int PaymentDays, string Remarks, string FileNo,
        //                    string OtherDeliveryInstruction, string OtherPaymentTerm,
        //                    string xmlPODetails, string xmlPOChargeItems, string xmlPOOtherCharge,
        //                    string xmlPOTermCondition, int EncodedBy, out int POId)
        //{
        //    POId = 0;
        //    Hashtable HshIn = new Hashtable();
        //    Hashtable HshOut = new Hashtable();
        //    HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
        //    HshIn.Add("@inyHospitalLocationId", HospId);
        //    HshIn.Add("@intStoreId", StoreId);
        //    HshIn.Add("@intFacilityId", FacilityId);
        //    HshIn.Add("@dtPurchaseOrderDate", PurchaseOrderDate);
        //    HshIn.Add("@intSupplierId", SupplierId);
        //    HshIn.Add("@monNetAmt", NetAmt);
        //    HshIn.Add("@intPaymentMonths", PaymentMonths);
        //    HshIn.Add("@intPaymentDays", PaymentDays);
        //    HshIn.Add("@chvRemarks", Remarks.Trim());
        //    HshIn.Add("@chvFileNo", FileNo.Trim());
        //    HshIn.Add("@chvOtherDeliveryInstruction", OtherDeliveryInstruction.Trim());
        //    HshIn.Add("@chvOtherPaymentTerm", OtherPaymentTerm.Trim());
        //    HshIn.Add("@xmlPODetails", xmlPODetails);
        //    HshIn.Add("@xmlPOChargeItems", xmlPOChargeItems);
        //    HshIn.Add("@xmlPOOtherCharge", xmlPOOtherCharge);
        //    HshIn.Add("@xmlPOTermCondition", xmlPOTermCondition);
        //    HshIn.Add("@intEncodedBy", EncodedBy);

        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //    HshOut.Add("@intPOId", SqlDbType.Int);
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrPOMain", HshIn, HshOut);

        //    POId = Convert.ToInt32(HshOut["@intPOId"]);

        //    return HshOut["@chvErrorStatus"].ToString();
        //}

        public string SavePOMain(int PurchaseOrderId, int HospId, int StoreId, int FacilityId, DateTime PurchaseOrderDate,
                                int SupplierId, double NetAmt, int PaymentMonths, int PaymentDays, string Remarks, string FileNo,
                                string OtherDeliveryInstruction, string OtherPaymentTerm,
                                string xmlPODetails, string xmlPOChargeItems, string xmlPOOtherCharge,
                                string xmlPOTermCondition, int EncodedBy, string ManualIndentNo, int DepartmentId,
                                string InvNo, string challanNo, DateTime InvDate, DateTime ChallanDate, string warranty, out int POId)
        {
            POId = 0;
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@dtPurchaseOrderDate", PurchaseOrderDate);
            HshIn.Add("@intSupplierId", SupplierId);
            HshIn.Add("@monNetAmt", NetAmt);
            HshIn.Add("@intPaymentMonths", PaymentMonths);
            HshIn.Add("@intPaymentDays", PaymentDays);
            HshIn.Add("@chvRemarks", Remarks.Trim());
            HshIn.Add("@chvFileNo", FileNo.Trim());
            HshIn.Add("@chvOtherDeliveryInstruction", OtherDeliveryInstruction.Trim());
            HshIn.Add("@chvOtherPaymentTerm", OtherPaymentTerm.Trim());
            HshIn.Add("@xmlPODetails", xmlPODetails);
            HshIn.Add("@xmlPOChargeItems", xmlPOChargeItems);
            HshIn.Add("@xmlPOOtherCharge", xmlPOOtherCharge);
            HshIn.Add("@xmlPOTermCondition", xmlPOTermCondition);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshIn.Add("@chvManualIndentNo", ManualIndentNo);
            HshIn.Add("@intDepartmentId", DepartmentId);

            HshIn.Add("@billNo", InvNo);
            HshIn.Add("@challanNo", challanNo);
            HshIn.Add("@billDate", InvDate);
            HshIn.Add("@challanDate", ChallanDate);
            HshIn.Add("@chrWarranty", warranty);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@intPOId", SqlDbType.Int);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrPOMain", HshIn, HshOut);

            POId = Convert.ToInt32(HshOut["@intPOId"]);

            return HshOut["@chvErrorStatus"].ToString();
        }

        public string SavePOAnnexures(int PurchaseOrderId, string Reference1, string Reference2, string Annexure, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
            HshIn.Add("@chvReference1", Reference1.Trim());
            HshIn.Add("@chvReference2", Reference2.Trim());
            HshIn.Add("@txtAnnexure", Annexure.Trim());
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrPOAnnexures", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPOAnnexureData(int PurchaseOrderId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);

                string qry = "SELECT Reference1, Reference2, Annexure FROM PhrPOAnnexureDetails WITH (NOLOCK) WHERE PurchaseOrderId = @intPurchaseOrderId";


                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getPOMain(int PurchaseOrderId, string PurchaseOrderNo, int StoreId, int FacilityId,
                                    int Active, int HospId, int EncodedBy, int POTypeId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
                HshIn.Add("@chvPurchaseOrderNo", PurchaseOrderNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intPOTypeId", POTypeId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrPOMain", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getPOMainList(int PurchaseOrderId, string PurchaseOrderNo, int StoreId,
                                    int SupplierId, DateTime FromDate, DateTime ToDate,
                                    double NetAmt, int Authorized, int Active,
                                    bool IsPendingPO, int HospId, int FacilityId, int EncodedBy,
            int AuthorizedBy, int POTypeId)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {



                HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
                HshIn.Add("@chvPurchaseOrderNo", PurchaseOrderNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@monNetAmt", NetAmt);
                HshIn.Add("@intIsAuthorized", Authorized);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intAuthorizedBy", AuthorizedBy);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intPOTypeId", POTypeId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                if (IsPendingPO)
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrPOMainListPending", HshIn, HshOut);
                }
                else
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrPOMainList", HshIn, HshOut);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getPODetails(int PurchaseOrderId, int StoreId, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrPODetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveWorkOrder(int WorkOrderId, int HospId, int StoreId, int FacilityId, DateTime WorkOrderDate,
                           int SupplierId, int DepartmentId, double NetAmt, string xmlWorkOrderDetails, string xmlWorkOrderRemarks
                         , int EncodedBy, out int intWorkId, int IsClosed)
        {
            intWorkId = 0;
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intWorkOrderId", WorkOrderId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@dtWorkOrderDate", WorkOrderDate);
            HshIn.Add("@intSupplierId", SupplierId);
            HshIn.Add("@intDepartmentId", DepartmentId);
            HshIn.Add("@NetAmt", NetAmt);
            HshIn.Add("@xmlWorkOrderDetails", xmlWorkOrderDetails);
            HshIn.Add("@xmlWorkOrderRemarks", xmlWorkOrderRemarks);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@isClosed", IsClosed);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@intWorkId", SqlDbType.Int);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrWorkOrderMain", HshIn, HshOut);

                intWorkId = Convert.ToInt32(HshOut["@intWorkId"]);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getWorkOrderMain(int WorkOrderId, string WorkOrderNo, int StoreId, int FacilityId,
                                    int Active, int HospId, int EncodedBy)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intWorkOrderId", WorkOrderId);
                HshIn.Add("@chvWorkOrderNo", WorkOrderNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrWorkOrderMain", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getWorkOrderMainList(int WorkOrderId, string WorkOrderNo, int StoreId,
                                   int SupplierId, int DepartmentId, DateTime FromDate, DateTime ToDate,
                                   double NetAmt, int IsClosed, int Active,
                                    int HospId, int FacilityId, int IsClosedBy, int EncodedBy)
        {

            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intWorkOrderId", WorkOrderId);
                HshIn.Add("@chvWorkOrderNo", WorkOrderNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@intDepartmentid", DepartmentId);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@NetAmt", NetAmt);
                HshIn.Add("@intIsClosed", IsClosed);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intIsClosedBy", IsClosedBy);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrWorkOrderMainList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet generatePOFromIndent(int POIndentId, string strItemIds)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intPOIndentId", POIndentId);
                HshIn.Add("@chvItemIds", strItemIds);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGeneratePOFromIndent", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet generatePOFromIndent(string POIndentId, string strItemIds)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intPOIndentId", POIndentId);
                HshIn.Add("@chvItemIds", strItemIds);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGeneratePOFromIndent", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string POAuthorization(string useFor, int PurchaseOrderId, int FacilityId, int HospId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                if (useFor == "Authorized")
                {
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrUpdatePOApproval", HshIn, HshOut);
                    return HshOut["@chvErrorStatus"].ToString();
                }
                else
                {
                    string qry = "SELECT GRNId FROM PhrGRNMain WITH (NOLOCK) WHERE ISNULL(PurchaseOrderId, 0) = @intPurchaseOrderId AND Active = 1";


                    ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return "GRN already made for this PO. Not allowed to unauthorize this PO !";
                    }

                    objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrPOMain SET IsAuthorized = (CASE WHEN IsAuthorized = 1 THEN 0 ELSE 1 END), AuthorizedBy = @intEncodedBy, AuthorizedDate = GETUTCDATE() WHERE PurchaseOrderId = @intPurchaseOrderId", HshIn);

                    return "Succeeded !";
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public string POCancel(int PurchaseOrderId, string CancelRemarks, int EncodedBy, int HospId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
            HshIn.Add("@chvCancelRemarks", CancelRemarks.Trim());
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@inyHospitalLocationId", HospId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrPOMainCancel", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }
        public string WorkOrderCancel(int WorkOrderId, string CancelRemarks, int EncodedBy, int HospId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intWorkOrderId", WorkOrderId);
            HshIn.Add("@chvCancelRemarks", CancelRemarks.Trim());
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@inyHospitalLocationId", HospId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrWorkOrderMainCancel", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPOTermCondition(int PurchaseOrderId, string Flag, bool OnlyTagged)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();


                HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
                HshIn.Add("@chvFlag", Flag);

                string qry = "";

                if (PurchaseOrderId > 0)
                {
                    qry = "SELECT (CASE WHEN ISNULL(tt.PurchaseOrderId, 0) = 0 THEN 0 ELSE 1 END) AS IsChk, " +
                        " mt.NotesId, mt.Notes, mt.Flag " +
                        " FROM PhrPONotes mt WITH (NOLOCK) " +
                        " INNER  JOIN PhrPOTermCondition tt WITH (NOLOCK) ON mt.NotesId = tt.NotesId AND tt.Active = 1 AND tt.PurchaseOrderId = @intPurchaseOrderId " +
                        " WHERE mt.Active = 1 ";
                }
                else
                {
                    qry = "SELECT (CASE WHEN ISNULL(mt.DefaultApplicable, 0) = 0 THEN 0 ELSE 1 END) AS IsChk, " +
                           " mt.NotesId, mt.Notes, mt.Flag " +
                           " FROM PhrPONotes mt WITH (NOLOCK) " +
                           " WHERE mt.Active = 1 ";
                }

                if (Flag != "")
                {
                    qry += " AND mt.Flag = @chvFlag ";
                }
                if (OnlyTagged)
                {
                    qry += " AND ISNULL(tt.NotesId, 0) <> 0 ";
                }

                qry += " ORDER BY IsChk DESC, Notes ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public double getLastPOItemRate(int ItemId, int SupplierId)
        {
            double Rate = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);

                string qry = "SELECT TOP 1 pod.Rate FROM PhrPODetails pod WITH (NOLOCK) LEFT OUTER JOIN PhrPOMain pom WITH (NOLOCK) ON pod.PurchaseOrderId = pom.PurchaseOrderId " +
                             " WHERE pod.ItemId = @intItemId AND pod.Active = 1 ";

                if (SupplierId > 0)
                {
                    qry += " AND pom.SupplierId = " + SupplierId;
                }

                qry += " ORDER BY pod.EncodedDate DESC ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Rate"]);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

            return Rate;
        }

        public DataSet getLastPOItemRate_PO(int ItemId, int StoreId, int HospId, int FacilityId, int SupplierId)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@intSupplierId", SupplierId);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrLastPurchaseRatesForSameSupplier", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public bool isDrugsGroup(int ItemCategoryId, int HospId)
        {
            bool isDrug = false;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemCategoryId", ItemCategoryId);
                HshIn.Add("@inyHospId", HospId);

                string qry = "SELECT icm.ItemCategoryName FROM PhrItemCategoryMaster icm WITH (NOLOCK) " +
                            " INNER JOIN StatusMaster sm WITH (NOLOCK) ON icm.CategoryTypeId = sm.StatusId AND StatusType='ItemNature' " +
                            " WHERE sm.Code='D' AND icm.ItemCategoryId = @intItemCategoryId AND icm.HospitalLocationId = @inyHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    isDrug = true;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; ds.Dispose(); }

            return isDrug;
        }

        public string SaveGroupDepartments(int GroupId, string xmlGroupDeptIds, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@xmlGroupDeptIds", xmlGroupDeptIds);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSecGroupDepartments", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getGroupDepartmentTagged(int GroupId, bool OnlyTagged)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intGroupId", GroupId);

                string qry = "SELECT chs.DepartmentID, chs.DepartmentName " +
                            " FROM DepartmentMain chs WITH (NOLOCK) " +
                            " LEFT OUTER JOIN SecGroupDepartments icd WITH (NOLOCK) ON chs.DepartmentID = icd.DepartmentId AND icd.Active = 1 AND icd.GroupId = @intGroupId " +
                            " WHERE chs.Active = 1";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(icd.DepartmentId, 0) <> 0 ";
                }

                qry += " ORDER BY chs.DepartmentName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getGroupDepartmentTagged(int GroupId, int FacilityId, bool OnlyTagged)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT chs.DepartmentID, chs.DepartmentName " +
                            " FROM DepartmentMain chs WITH (NOLOCK) " +
                            " LEFT OUTER JOIN SecGroupDepartments icd WITH (NOLOCK) ON chs.DepartmentID = icd.DepartmentId AND icd.Active = 1 AND icd.GroupId = @intGroupId " +
                            " WHERE chs.Active = 1 AND chs.FacilityId=@intFacilityId ";

                if (OnlyTagged)
                {
                    qry += " AND ISNULL(icd.DepartmentId, 0) <> 0 ";
                }

                qry += " ORDER BY chs.DepartmentName";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getLastPurchaseRates(int StoreId, int FacilityId, int ItemId, int HospId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intItemId", ItemId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrLastPurchaseRates", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveItemReorderLevel(string xmlReorderLevel, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@xmlReorderLevel", xmlReorderLevel);
            HshIn.Add("@inyHospitalLocationId", HospId);

            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemReorderLevel", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveFacilityReorderSetup(int FacilityId, string xmlReorderLevel, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlReorderLevel", xmlReorderLevel);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveFacilityItemReorderSetup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemReorderLevel(int ItemId, int StoreId, int FacilityId, string UseFor,
                                            int ItemCategoryId, int ItemSubCategoryId, string ItemName)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrUseFor", UseFor);
                HshIn.Add("@intItemCategoryId", ItemCategoryId);
                HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
                HshIn.Add("@chvItemName", ItemName);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemReorderLevel", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getEmployeeList(int HospId, int FacilityId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getIPIssueItemList(int EncounterId, string EncounterNo, int StoreId, int FacilityId, string ItemSearch)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvItemSearch", "%" + ItemSearch + "%");

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPIssueItemList", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getOpIssueItemList(int RegistratonId, string RegistrationNo, int StoreId, int FacilityId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intRegistratonId", RegistratonId);
                HshIn.Add("@chrRegistratonNo", RegistrationNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetOPIssueItemList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getIPIssueItemDetails(int EncounterId, string EncounterNo, int StoreId, int FacilityId,
                                        string xmlItemId, int LoginFacilityId, int FromWard)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@xmlItemId", xmlItemId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@inyFromWard", FromWard);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPIssueItemDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getOpenTransactions(string strTransactionType, int LoginFacilityId, int FacilityId, int StoreId, string strStoreId, int Encounterid)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@chvTransactionType", strTransactionType);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@chrStoreId", strStoreId);// use to get expiry item
                HshIn.Add("@intEncounterId", Encounterid);//Encounter id comming from Intimation
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetOpenTransactions", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getpendingrequest(int HospitalLocationId, int FacilityId, int Encounterid, DateTime FDate, DateTime TDate)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncounterId", Encounterid);
                HshIn.Add("@FromDate", FDate);
                HshIn.Add("@ToDate", TDate);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRPrintpendingrequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveDEPIndent(int IndentId, DateTime IndentDate, int HospitalLocationId, int FacilityFromId,
                    int DepartmentFromId, int FacilityToId, int DepartmentToId, int IndentBy, string IndentRemarks,
                    string ProcessStatus, int EncodedBy, string xmlItemDetails, bool IndentFromROL, out int SavedId)
        {
            SavedId = 0;

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@dtIndentDate", IndentDate);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityFromId", FacilityFromId);
            HshIn.Add("@intDepartmentFromId", DepartmentFromId);
            HshIn.Add("@intFacilityToId", FacilityToId);
            HshIn.Add("@intDepartmentToId", DepartmentToId);
            HshIn.Add("@intIndentBy", IndentBy);
            HshIn.Add("@chvIndentRemarks", IndentRemarks);
            HshIn.Add("@chrProcessStatus", ProcessStatus);
            HshIn.Add("@bitIndentFromROL", IndentFromROL);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlItemDetails", xmlItemDetails);

            HshOut.Add("@intSavedId", SqlDbType.Int);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveDEPIndent", HshIn, HshOut);

                SavedId = Convert.ToInt32(HshOut["@intSavedId"]);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveDEPIndentApproval(int IndentId, int HospitalLocationId, int ApprovalStatusId, int ApprovedBy, string ApprovalRemarks,
                                    string xmlItemDetails, string PreDocNo)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intIndentId", IndentId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intApprovalStatusId", ApprovalStatusId);
            HshIn.Add("@intApprovedBy", ApprovedBy);
            HshIn.Add("@chvApprovalRemarks", ApprovalRemarks);
            HshIn.Add("@xmlItemDetails", xmlItemDetails);
            HshIn.Add("@vPreDocNo", PreDocNo);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveDEPIndentApproval", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDEPIndent(int IndentId, string IndentNo, int HospitalLocationId, int FacilityId,
                               int StoreId, int EncodedBy, int ClosingStockRequired)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@bitClosingStockRequired", ClosingStockRequired);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDEPIndent", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public string SavePOIndentApproval(int ApprovalId, int StoreId, int HospId, int FacilityId, double MinAmount, double MaxAmount,
                                        int EmployeeId, int Active, int UserId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intId", ApprovalId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@mnMinAmount", MinAmount);
            HshIn.Add("@mnMaxAmount", MaxAmount);
            HshIn.Add("@intEmployeeId", EmployeeId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSavePOApprovalSetup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPOIndentApproval(int ApprovalId, int HospId, int LoginFacilityId, int FacilityId, int StoreId, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intApprovalId", ApprovalId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intEncodedBy", UserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetPOApprovalSetup", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getFacilityStoreEmployee(int FacilityId, int StoreId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);

                string qry = " SELECT DISTINCT poas.EmployeeId, emp.FirstName + ISNULL(' ' + emp.MiddleName,'') + ISNULL(' ' + emp.LastName,'') + ' (' + emp.EmployeeNo + ')' AS EmployeeNameWithNo " +
                            " FROM PhrPOApprovalSetup poas WITH (NOLOCK) " +
                            " INNER JOIN Employee emp WITH (NOLOCK) ON poas.EmployeeId = emp.ID " +
                            " WHERE poas.StoreId = @intStoreId AND poas.Active = 1 AND poas.FacilityId = @intFacilityId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string SavePOApprovalRouting(int RoutingId, int HospId, int FacilityId, int StoreId, DateTime FromDate, DateTime ToDate,
                                        int OriginalEmpId, int CurrentEmpId, int Active, int UserId)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intRoutingId", RoutingId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@dtFromDate", fDate);
            HshIn.Add("@dtToDate", tDate);
            HshIn.Add("@intOriginalEmpId", OriginalEmpId);
            HshIn.Add("@intCurrentEmpId", CurrentEmpId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSavePOApprovalRouting", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPOApprovalRouting(int RoutingId, int HospId, int LoginFacilityId, int FacilityId, int StoreId, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intRoutingId", RoutingId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intEncodedBy", UserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetPOApprovalRouting", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDEPIndentList(int HospitalLocationId, string IndentNo, int FacilityId, int StoreId,
                            int FacilityToId, int DepartmentToId, int IndentBy, string ProcessStatus,
                            int ApprovalStatus, DateTime FromDate, DateTime ToDate, int Active, int EncodedBy, string IndentType)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd");
            string tDate = ToDate.ToString("yyyy-MM-dd");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityToId", FacilityToId);
                HshIn.Add("@intDepartmentToId", DepartmentToId);
                HshIn.Add("@intIndentBy", IndentBy);
                HshIn.Add("@chrProcessStatus", ProcessStatus);
                HshIn.Add("@intApprovalStatus", ApprovalStatus);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@bitActive", Active);

                HshIn.Add("@chrIndentType", IndentType);

                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDEPIndentList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getStoreBasedOnFacility(int FacilityId, int YearId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@YearId", YearId);

                string qry = " SELECT DISTINCT sm.StoreId AS DepartmentId, dm.DepartmentName " +
                            " FROM PhrStoreMaster sm WITH (NOLOCK) " +
                            " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON sm.StoreId = dm.DepartmentID AND sm.Active = 1 AND dm.Active = 1 " +
                            " WHERE sm.FacilityId = @FacilityId AND sm.YearId = @YearId " +
                            " ORDER BY dm.DepartmentName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string POIndentPost(int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intIndentId", IndentId);

                string qry = "SELECT POIndentId FROM PhrPOIndentMain WITH (NOLOCK) WHERE Active = 1 AND POIndentId = @intIndentId AND ProcessStatus <> 'O'";


                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Post operation not allowed in Posted or Cancelled Indnet !";
                }

                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrPOIndentMain SET ProcessStatus = 'P' WHERE POIndentId = @intIndentId", HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); ds.Dispose(); }

        }

        public string POIndentCancel(int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intIndentId", IndentId);

                string qry = "SELECT POIndentId FROM PhrPOIndentMain WITH (NOLOCK) WHERE Active = 1 AND POIndentId = @intIndentId AND ProcessStatus <> 'O'";


                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Cancel operation not allowed in Posted or Cancelled Indnet !";
                }

                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrPOIndentMain SET ProcessStatus = 'C', Active = 0 WHERE POIndentId = @intIndentId", HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public string DEPIndentPost(int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {


                HshIn.Add("@intIndentId", IndentId);

                string qry = "SELECT IndentId FROM PhrDepIndentMain WITH (NOLOCK) WHERE Active = 1 AND IndentId = @intIndentId AND ProcessStatus <> 'O'";


                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Post operation not allowed in Posted or Cancelled Indnet !";
                }

                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrDepIndentMain SET ProcessStatus = 'P' WHERE IndentId = @intIndentId", HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public string DEPIndentCancel(int IndentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intIndentId", IndentId);
                string qry = "SELECT IndentId FROM PhrDepIndentMain WITH (NOLOCK) WHERE Active = 1 AND IndentId = @intIndentId AND ProcessStatus <> 'O'";
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Cancel operation not allowed in Posted or Cancelled Indnet !";
                }
                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrDepIndentMain SET ProcessStatus = 'C', Active = 0 WHERE IndentId = @intIndentId", HshIn);
                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet getDEPIndentApproval(int HospitalLocationId, string IndentNo, int FacilityId, int StoreId,
                            int FacilityToId, int DepartmentToId, DateTime FromDate, DateTime ToDate, int EncodedBy, int LoginFacilityId)
        {
            string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
            string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityToId", FacilityToId);
                HshIn.Add("@intDepartmentToId", DepartmentToId);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDEPIndentApproval", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDEPIndentIssue(int IndentId, string IndentNo, int HospitalLocationId, int FacilityFromId,
                        int DepartmentFromId, int FacilityToId, int DepartmentToId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {



                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@chvIndentNo", IndentNo);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityFromId", FacilityFromId);
                HshIn.Add("@intDepartmentFromId", DepartmentFromId);
                HshIn.Add("@intFacilityToId", FacilityToId);
                HshIn.Add("@intDepartmentToId", DepartmentToId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDEPIndentIssue", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string DepartmentIssueReturnPost(int IssueId, int userId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@intUserId", userId);

                string qry = "SELECT IssueId FROM PhrDepartmentIssueMain WITH (NOLOCK) WHERE Active = 1 AND IssueId = @intIssueId AND ProcessStatus <> 'O'";


                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Post operation not allowed in Posted or Cancelled Document !";
                }

                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrDepartmentIssueMain SET ProcessStatus = 'P', PostedById = @intUserId, PostedDate = GETUTCDATE() WHERE IssueId = @intIssueId", HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }
        }

        public string DepartmentIssueReturnCancel(int IssueId, int userId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@intUserId", userId);

                string qry = "SELECT IssueId FROM PhrDepartmentIssueMain WITH (NOLOCK) WHERE Active = 1 AND IssueId = @intIssueId AND ProcessStatus <> 'O'";


                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return "Cancel operation not allowed in Posted or Cancelled Document !";
                }

                objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrDepartmentIssueMain SET ProcessStatus = 'C', CanceledById = @intUserId, CanceledDate = GETUTCDATE() WHERE IssueId = @intIssueId", HshIn);

                return "Succeeded !";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemIssueSaleReturnDetails(int iIssueId, int HospId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();


                HshIn.Add("iHospitalLocationId", HospId);
                HshIn.Add("iItemIssueId", iIssueId);
                HshIn.Add("iEncodedBy", EncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetItemIssueDetails", HshIn, HshOut);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        #region ram class
        public string SaveItemFlagMaster(int iItemFlagId, string sItemFlagName, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intItemFlagId", iItemFlagId);
            HshIn.Add("@chvItemFlagName", sItemFlagName.Trim());
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePhrItemFlagMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetItemFlagMaster(int iItemFlagId, int iHosID, int Active, int iuserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intItemFlagId", iItemFlagId);
                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@bitActive", Active);

                HshIn.Add("@intEncodedBy", iuserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemFlagMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveChargeSetup(int ChargeId, string ChargeName, string ChargeCode, string ChargeType, int IsBase, string ChargeBehaviour,
                                           int HospId, int Active, string xmlSeq, string xmlChargeCalculationOn, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intChargeId", ChargeId);
            HshIn.Add("@chvChargeName", ChargeName.Trim());
            HshIn.Add("@chvChargeCode", ChargeCode.Trim());
            HshIn.Add("@chrChargeType", ChargeType);
            HshIn.Add("@intIsBase", IsBase);
            HshIn.Add("@chrChargeBehaviour", ChargeBehaviour);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            if (xmlSeq != "")
            {
                HshIn.Add("@xmlSeq", xmlSeq);
            }
            HshIn.Add("@xmlChargeCalculationOn", xmlChargeCalculationOn);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrChargeSetup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getChargeSetup(int ChargeId, string ChargeType, int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intChargeId", ChargeId);
                HshIn.Add("@chrChargeType", ChargeType.Trim());
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrChargeSetup", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getChargeCalculationOn(int ChargeId, bool OnlyTagged)
        {

            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {
                    HshIn.Add("@intChargeId", ChargeId);

                    string qry = "SELECT (CASE WHEN ISNULL(tr.ChargeId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
                                " mt.ChargeId, REPLACE(mt.ChargeName, 'Discount', 'Discounted') AS ChargeName, tr.CalculationBase " +
                                " FROM PhrChargeSetup mt WITH (NOLOCK) " +
                                " LEFT OUTER JOIN PhrChargeCalculationOn tr WITH (NOLOCK) ON mt.ChargeId = tr.BaseChargeId AND tr.Active = 1 AND tr.ChargeId = @intChargeId " +
                                " WHERE mt.ChargeType = 'M' AND mt.Active = 1 ";

                    if (ChargeId > 0)
                    {
                        qry += " AND mt.ChargeId <> @intChargeId ";
                    }

                    if (OnlyTagged)
                    {
                        qry += " AND ISNULL(tr.ChargeId, 0) <> 0 ";
                    }

                    return objDl.FillDataSet(CommandType.Text, qry, HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public DataSet getChargeCalculationOnBase(int ChargeId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intChargeId", ChargeId);

                string qry = "SELECT ChargeId, BaseChargeId, CalculationBase " +
                        " FROM PhrChargeCalculationOn WITH (NOLOCK) " +
                        " WHERE Active = 1 " +
                        " AND ChargeId =(CASE WHEN @intChargeId > 0 THEN @intChargeId ELSE ChargeId END) ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveOtherChargeSetup(int ChargeId, string ChargeBehaviour,
                                            int HospId, int Active, string xmlSeq, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intChargeId", ChargeId);
            HshIn.Add("@chrChargeBehaviour", ChargeBehaviour);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            if (xmlSeq != "")
            {
                HshIn.Add("@xmlSeq", xmlSeq);
            }
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrOtherChargeSetup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveStatusMaster(int iStatusId, string sStatusType, string sStatus, string sCode, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intStatusId", iStatusId);
            HshIn.Add("@chvStatusType", sStatusType.Trim());
            HshIn.Add("@chvStatus", sStatus.Trim());
            HshIn.Add("@chvCode", sCode.Trim());
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveStatusMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getStatus(int iHospID, string statusType, string Code, int StatusId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvStatusType", statusType);

                string strqry = "SELECT StatusId, StatusType, Status, Code, Active, " +
                    " (Case Active When 1 Then 'Active' ELSE 'In-Active' end) as sStatus, SequenceNo " +
                    " from StatusMaster WITH (NOLOCK) " +
                    " where StatusType = @chvStatusType and Active = 1 ";

                if (StatusId > 0)
                {
                    HshIn.Add("@intStatusId", StatusId);
                    strqry += " AND StatusId = @intStatusId ";
                }
                if (Code != "")
                {
                    HshIn.Add("@chvCode", Code);
                    strqry += " AND Code = @chvCode ";
                }

                strqry += " ORDER BY SequenceNo ";

                return objDl.FillDataSet(CommandType.Text, strqry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveFormulationMaster(int iFormulationId, string sFormulationName, int IsCalculated, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intFormulationId", iFormulationId);
            HshIn.Add("@chvFormulationName", sFormulationName.Trim());
            HshIn.Add("@bitIsCalculated", IsCalculated);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrFormulationMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetFormulationMaster(int iFormulationId, int iHosID, int Active, int iuserId)
        {


            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFormulationId", iFormulationId);
                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@bitActive", Active);

                HshIn.Add("@intEncodedBy", iuserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrFormulationMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveSaleSetupMaster(int iSaleSetupId, string sSaleName, int iSaleSetupTypeId,
                                    string sPriceType, double dDiscountPerc, double dServiceTaxPerc,
                                    int ReceiptAllow, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intSaleSetupId", iSaleSetupId);
            HshIn.Add("@chvSaleSetupName", sSaleName.Trim());
            HshIn.Add("@intSaleSetupTypeId", iSaleSetupTypeId);
            HshIn.Add("@chvPriceType", sPriceType.Trim());
            HshIn.Add("@monDiscountPerc", dDiscountPerc);
            HshIn.Add("@monServiceTaxPerc", dServiceTaxPerc);
            HshIn.Add("@bitIsReceiptAllow", ReceiptAllow);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrSaleSetupMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSaleSetupMaster(int iSaleSetupId, int iHosID, int Active, int iuserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intSaleSetupId", iSaleSetupId);
                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", iuserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrSaleSetupMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveSaleDiscountPeriod(int iDiscPeriod, int iSaleSetupId, DateTime dtFrom, DateTime dtTo,
                                            double dDiscountPerc, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            string fDate = dtFrom.ToString("yyyy-MM-dd");
            string tDate = dtTo.ToString("yyyy-MM-dd");

            HshIn.Add("@inyDiscountPeriodId", iDiscPeriod);
            HshIn.Add("@intSaleSetupId", iSaleSetupId);
            HshIn.Add("@dtFrom", fDate);
            HshIn.Add("@dtTo", tDate);
            HshIn.Add("@monDiscountPerc", dDiscountPerc);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrSaleSetupDiscountPeriod", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSaleDiscountPeriod(int iDiscPeriod, int iSaleSetupId, int iHosID, int Active, int iuserId)
        {


            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyDiscountPeriodId", iDiscPeriod);
                HshIn.Add("@intSaleSetupId", iSaleSetupId);
                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", iuserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrSaleSetupDiscountPeriod", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveItemStrength(int iStrengthId, string sStrengthValue, int iStrengthUnitId, int iHosID, int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intStrengthId", iStrengthId);
            HshIn.Add("@intStrengthUnitId", iStrengthUnitId);
            HshIn.Add("@chvStrengthValue", sStrengthValue.Trim());
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveItemStrengthMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetItemStrength(int iStrengthId, int iStrengthUnitId, int iHosID, int Active, int iuserId)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intStrengthId", iStrengthId);
                HshIn.Add("@intStrengthUnitId", iStrengthUnitId);
                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", iuserId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemStrengthMaster", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// //Modify by santosh
        /// </summary>
        /// <param name="iHosID"></param>
        /// <param name="iLoginFacilityId"></param>
        /// <param name="iIsReceived"></param>
        /// <param name="iStoreId"></param>
        /// <param name="ifrmDate"></param>
        /// <param name="itoDate"></param>
        /// <param name="iuserId"></param>
        /// <param name="iIssueId"></param>
        /// <returns></returns>
        public DataSet GetIssueReceipt(int iHosID, int iLoginFacilityId, int iIsReceived, int iFromStoreId, int iIssueFromFacilityId, int IssueFacility, int iToStoreId, DateTime ifrmDate, DateTime itoDate, int iuserId, string iIssueno)
        {


            string fDate = ifrmDate.ToString("yyyy-MM-dd");
            string tDate = itoDate.ToString("yyyy-MM-dd");

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
                HshIn.Add("@intFromStoreId", iFromStoreId);
                HshIn.Add("@intFromFacilityId", iIssueFromFacilityId);
                HshIn.Add("@intToStoreId", iToStoreId);
                HshIn.Add("@intToFacilityId", IssueFacility);
                HshIn.Add("@bitIsReceived", iIsReceived);
                HshIn.Add("@dtsDateFrom", fDate);
                HshIn.Add("@dtsDateTo", tDate);
                HshIn.Add("@chvIssueNo", iIssueno);
                HshIn.Add("@intEncodedBy", iuserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetReceiptFromStore", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReceiptFromDetails(int iHosID, int iIssueId, int iuserId, int iLoginFacilityId)
        {


            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", iHosID);
                HshIn.Add("@intIssueId", iIssueId);
                HshIn.Add("@intEncodedBy", iuserId);
                HshIn.Add("@intLoginFacilityId", iLoginFacilityId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetReceiptFromStoreDetail", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string SaveSaleReceiptFromStore(int iIssueId, string IssueNo, int iHosID, string strXMLItemDetails, int iUserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intIssueId", iIssueId);
            HshIn.Add("@chvIssueNo", IssueNo);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            //HshIn.Add("@intFacilityId", iFacilityId);
            //HshIn.Add("@intFromStoreId", iFromStoreId);
            //HshIn.Add("@intToFacilityId", iToFacilityId);
            //HshIn.Add("@intToStoreId", iToStoreId);
            HshIn.Add("@XMLItemDetails", strXMLItemDetails);
            HshIn.Add("@intEncodedBy", iUserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrReceiptFromStore", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveItemMaster(int ItemId, string ItemName, int HospId,
                            int ItemSubCategoryId, int GenericId, int CIMSCategoryId,
                            int CIMSSubCategoryId, int ManufactureId, string Specification,
                            int FormulationId, string xmlStrengthIds, int Active, int EncodedBy, string XMLPackingId,
                            string xmlItemWithItemUnitIds, bool ItemCloseForPurchase, bool ItemCloseForSale, int iBrandId, int StrengthId, int PackingId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@chvItemName", ItemName.Trim());
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
            //HshIn.Add("@bitIsItem", IsItem);
            HshIn.Add("@intGenericId", GenericId);
            HshIn.Add("@intCIMSCategoryId", CIMSCategoryId);
            HshIn.Add("@intCIMSSubCategoryId", CIMSSubCategoryId);
            HshIn.Add("@intManufactureId", ManufactureId);
            HshIn.Add("@chvSpecification", Specification.Trim());
            HshIn.Add("@intFormulationId", FormulationId);
            HshIn.Add("@xmlStrengthIds", xmlStrengthIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlPackingIds", XMLPackingId);
            HshIn.Add("@xmlItemWithItemUnitIds", xmlItemWithItemUnitIds);
            HshIn.Add("@bitItemCloseForPurchase", ItemCloseForPurchase);
            HshIn.Add("@bitItemCloseForSale", ItemCloseForSale);
            HshIn.Add("@intBrandId", iBrandId);
            HshIn.Add("@intStrengthId", StrengthId);
            HshIn.Add("@iPackingId", PackingId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveItemMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //public DataSet getItemBrandMaster(int ItemId, string ItemNo, string ItemName, int Active, int HospId, int EncodedBy)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet ds = new DataSet();
        //    HshIn.Add("@intItemId", ItemId);
        //    HshIn.Add("@chvItemNo", ItemNo);
        //    HshIn.Add("@chvItemSearch", ItemName);
        //    HshIn.Add("@bitActive", Active);
        //    HshIn.Add("@inyHospitalLocationId", HospId);
        //    HshIn.Add("@intEncodedBy", EncodedBy);
        //    HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);           
        //    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMaster", HshIn, HshOut);
        //   
        //}
        //public DataSet getItemBrandStrength(int ItemBrandId, int HospId, bool OnlyTagged)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@intItemBrandId", ItemBrandId);
        //        HshIn.Add("@intHospId", HospId);

        //        string qry = "SELECT (CASE WHEN ISNULL(tr.StrengthId, 0) = 0 THEN 0 ELSE 1 END) IsChk, " +
        //                " mt.StrengthId, IsNull(mt.StrengthValue, '') + IsNull(' ' + um.UnitName, '') AS Strength " +
        //                " FROM PhrItemStrengthMaster mt " +
        //                " LEFT OUTER JOIN PhrUnitMaster um ON mt.StrengthUnitId = um.UnitId " +
        //                " LEFT OUTER JOIN PhrItemMaster tr ON mt.StrengthId = tr.StrengthId AND tr.Active = 1 AND tr.ItemBrandId = @intItemBrandId " +
        //                " WHERE mt.Active = 1 AND mt.HospitalLocationId = @intHospId ";

        //        if (OnlyTagged)
        //        {
        //            qry += " AND ISNULL(tr.StrengthId, 0) <> 0 ";
        //        }

        //        qry += " ORDER BY Strength ";

        //        return objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}

        public string SaveGRN(int iGRNId, int iStoreId, string sGRNNo, DateTime dtGRNDate,
                              int iSupplierId, int iPurchaseOrderId, string sPurchaseMode, string sBillNo,
                              DateTime dtBillDate, double dblBillAmount, string sGatePassNo, int iReceivedById,
                              double dblNetAmount, double dblTotalOtherChargeAmount, double dblTotalAmount, int iPostedById, DateTime dtPostedDate, string sNarration, string sProcessStatus,
                              string sChallanNo, DateTime dtChallanDate,
                              string xmlItemDetails, string xmlChargeDetails, string @xmlOtherChargeDetails, string xmlInspected, int iHosID,
                              int Active, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intGRNId", iGRNId);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@intStoreId", iStoreId);
            HshIn.Add("@chvGRNNo", sGRNNo);
            HshIn.Add("@dtGRNDate", Convert.ToDateTime(dtGRNDate));
            HshIn.Add("@intSupplierId", iSupplierId);
            if (iPurchaseOrderId > 0)
            {
                HshIn.Add("@intPurchaseOrderId", iPurchaseOrderId);
                HshIn.Add("@chrPurchaseMode", sPurchaseMode);
            }
            if (sBillNo != "")
            {
                HshIn.Add("@chvBillNo", sBillNo);
                HshIn.Add("@dtBillDate", Convert.ToDateTime(dtBillDate));
            }
            if (dblBillAmount > 0)
                HshIn.Add("@mnyBillAmount", dblBillAmount);

            if (sGatePassNo != "")
                HshIn.Add("@chvGatePassNo", sGatePassNo);

            HshIn.Add("@intReceivedById", iReceivedById);
            HshIn.Add("@mnyNetAmount", dblNetAmount);

            HshIn.Add("@mnyTotalOtherChargeAmount", dblTotalOtherChargeAmount);
            HshIn.Add("@mnyTotalAmount", dblTotalAmount);

            if (iPostedById > 0)
            {
                HshIn.Add("@intPostedById", iPostedById);
                HshIn.Add("@dtPostedDate", Convert.ToDateTime(dtPostedDate));
            }
            if (sNarration != "")
                HshIn.Add("@chvNarration", sNarration);

            HshIn.Add("@chrProcessStatus", sProcessStatus);

            if (sChallanNo != "")
            {
                HshIn.Add("@ChallanNo", sChallanNo);
                HshIn.Add("@ChallanDate", Convert.ToDateTime(dtChallanDate));
            }

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", iuserId);
            HshIn.Add("@xmlItemDetails", xmlItemDetails);
            HshIn.Add("@xmlChargeDetails", xmlChargeDetails);
            HshIn.Add("@xmlOtherChargeDetails", @xmlOtherChargeDetails);
            if (xmlInspected != "")
                HshIn.Add("@xmlInspected", xmlInspected);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrGRN", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public DataSet getGRNPO(int iStoreId, int PurchaseOrderId, string sPONo, int iItemID, int SupplierID, int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStoreId", iStoreId);
                HshIn.Add("@intPurchaseOrderId", PurchaseOrderId);
                HshIn.Add("@chvPurchaseOrderNo", sPONo);
                HshIn.Add("@intItemId", iItemID);
                HshIn.Add("@intSupplierID", SupplierID);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrGRNPO", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }



        public string GRNPost(int iGRNId, int iStoreId, string xmlItemDetails, int iHosID, int iuserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intGRNId", iGRNId);
            HshIn.Add("@inyHospitalLocationId", iHosID);
            HshIn.Add("@intStoreId", iStoreId);
            HshIn.Add("@XMLItemDetails", xmlItemDetails);
            HshIn.Add("@intEncodedBy", iuserId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrPostGRN", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPOList(int iStoreID, int iHospId, string dtFromDate, string dtToDate, string sOrderID, string RrportFor, string FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStoreId", iStoreID);
                HshIn.Add("@intHospId", iHospId);
                HshIn.Add("@dtFrom", Convert.ToDateTime(dtFromDate).ToString("yyyy/MM/dd"));
                HshIn.Add("@dtTo", Convert.ToDateTime(dtToDate).ToString("yyyy/MM/dd"));
                HshIn.Add("@FacilityId", FacilityId);
                string qry = "";
                if (RrportFor != "GRN")
                {
                    qry = " SELECT pom.PurchaseOrderId, pom.PurchaseOrderNo, pom.SupplierId " +
                            " FROM PhrPOMain pom WITH (NOLOCK) " +
                            //" INNER JOIN PhrSupplierMaster sup WITH (NOLOCK) ON pom.SupplierId = sup.SupplierId  " +
                            "  WHERE pom.StoreId = @intStoreId " +
                            "  AND pom.PurchaseOrderDate between @dtFrom AND @dtTo " +
                            "  AND ISNULL(pom.Active, 0) = 1  " +
                            "  AND pom.FacilityId = @FacilityId " +
                            "  AND pom.HospitalLocationId = @intHospId ";

                    if (sOrderID != "")
                    {
                        qry += " AND pom.PurchaseOrderId IN(" + sOrderID + ")  ";
                    }

                }
                else
                {
                    qry = " SELECT GRN.GRNId as PurchaseOrderId, GRN.GRNNo as PurchaseOrderNo, GRN.SupplierId " +
                            " FROM PhrGRNMain GRN WITH (NOLOCK) " +
                            //"INNER JOIN PhrSupplierMaster sup WITH (NOLOCK) ON GRN.SupplierId = sup.SupplierId  " +
                            " WHERE GRN.StoreId = @intStoreId " +
                            " AND GRN.GRNDate between @dtFrom AND @dtTo " +
                            " AND ISNULL(GRN.Active, 0) = 1  " +
                            " AND GRN.FacilityId = @FacilityId " +
                            " AND GRN.HospitalLocationId = @intHospId ";

                    if (sOrderID != "")
                    {
                        qry += " AND GRN.GRNId IN(" + sOrderID + ")  ";
                    }
                }
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        // Quotation Approve 
        public string ApproveQuotation(int HospId, int facilityId, int SupId, string XmlDetails, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", facilityId);
            HshIn.Add("@intSupplierId", SupId);
            HshIn.Add("@xmlQuotDetails", XmlDetails);
            HshIn.Add("@intEncodedBy", UserId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPphrApproveQuotation", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion

        #region Santosh class

        public DataSet GetPatientDetails(int iHospId, int iRegId, string PatienType, string RegistrationNo, string EncounterNo)
        {
            HshIn = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("intRegistrationId", iRegId);
                HshIn.Add("chrPatienType", PatienType);
                HshIn.Add("chrRegistrationNo", RegistrationNo);
                HshIn.Add("chrEncounterNo", EncounterNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchPatient", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public DataSet getEncBasedOnRegNo(int HospId, int FacilityID, string RegistrationNo)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityID", FacilityID);
                HshIn.Add("@RegistrationNo", RegistrationNo);

                string qry = "SELECT TOP 1 en.EncounterId, en.EncounterNo " +
                            " FROM Admission en WITH (NOLOCK) " +
                            " WHERE RegistrationNo = @RegistrationNo AND en.Active = 1 " +
                            " AND en.PatadType <> 'D' AND en.DischargeDate IS NULL AND FacilityID = @FacilityID AND HospitalLocationId = @HospId " +
                            " ORDER BY ID DESC";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getEncounterStatus(int HospId, int FacilityID, string EncounterNo)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@FacilityID", FacilityID);
                HshIn.Add("@EncounterNo", EncounterNo);
                HshIn.Add("@StatusType", "Encounter");

                string qry = "SELECT en.Id AS EncounterId, en.EncounterNo, sm.[Code] AS PatientStatus " +
                        " FROM Encounter en WITH (NOLOCK) " +
                        " INNER JOIN StatusMaster sm WITH (NOLOCK) on en.StatusId = sm.StatusId AND sm.StatusType = @StatusType AND sm.Active = 1 " +
                        " WHERE en.Active = 1 AND en.EncounterNo = @EncounterNo AND en.FacilityID = @FacilityID AND en.HospitalLocationId = @HospId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetEmployee(int iHospId)
        {

            HshIn = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", iHospId);
                return objDl.FillDataSet(CommandType.Text, "SELECT Id,EmployeeNo, isnull(FirstName,'') + isnull(' ' + MiddleName,'') + isnull(' ' + Lastname,'') as Name FROM employee WITH (NOLOCK) WHERE Active=1 AND HospitalLocationId = @inyHospitalLocationId ORDER BY FirstName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //public DataSet GetSaleChargeMaster(int iChargeId, int iChargeTypeId, int iHosID, int Active, int iuserId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        HshOut = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        HshIn = new Hashtable();
        //        HshIn.Add("@intChargeID", iChargeId);
        //        HshIn.Add("@intChargeTypeId", iChargeTypeId);
        //        HshIn.Add("@inyHospitalLocationId", iHosID);
        //        HshIn.Add("@bitActive", Active);
        //        HshIn.Add("@intEncodedBy", iuserId);
        //        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //        return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhrSaleChargeMaster", HshIn, HshOut);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}

        public DataSet GetStaffpatient(int HospitalId, string strname, string strstatus)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospitalId);
                HshIn.Add("strName", strname);
                HshIn.Add("strStatus", strstatus);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPSearchStaffPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetAdmittedpatient(int HospitalId, string strname, string strstatus)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospitalId);
                HshIn.Add("strName", strname);
                HshIn.Add("strStatus", strstatus);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPSearchIPPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetRegistrationpatient(int HospitalId, string strname, string strstatus)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospitalId);
                HshIn.Add("strName", strname);
                HshIn.Add("strStatus", strstatus);
                return objDl.FillDataSet(CommandType.StoredProcedure, "SearchPatientByName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //public DataSet GetItemCommon(int ItemId, int ItemBrandId, int FormulationId, int StrengthId, int GenericId, int HospId, int EncodedBy)
        //{
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet ds = new DataSet();

        //    HshIn.Add("intItemId", ItemId);
        //    HshIn.Add("intItemBrandId", ItemBrandId);
        //    HshIn.Add("intFormulationId", FormulationId);
        //    HshIn.Add("intStrengthId", StrengthId);
        //    HshIn.Add("intGenericId", GenericId);
        //    //  HshIn.Add("intItemNameOrientation", ItemNameOrientation);
        //    HshIn.Add("inyHospitalLocationId", HospId);
        //    HshIn.Add("intEncodedBy", EncodedBy);
        //    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

        //    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterCommon", HshIn, HshOut);

        //   
        //}
        /// <summary>
        /// Author: Rafat
        /// To Show Batch No Item wise.
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="ItemId"></param>
        /// <param name="StoreId"></param>
        /// <param name="EncodedBy"></param>
        /// <param name="FacilityId"></param>
        /// <param name="ItemWithStock"></param>
        /// <returns></returns>
        public DataSet GetItemBatch(int HospId, int ItemId, int StoreId, int EncodedBy, int FacilityId, int ItemWithStock, int CompanyId, int TransactionId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intItemId", ItemId);
                HshIn.Add("intStoreCode", StoreId);
                HshIn.Add("bitItemsWithStock", ItemWithStock);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("intCompanyId", CompanyId);
                HshIn.Add("intTransactionId", TransactionId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhrBatchDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetItemBatch(int HospId, int ItemId, int StoreId, int EncodedBy, int FacilityId, int ItemWithStock, int CompanyId, int TransactionId, int CallingPart = 0)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intItemId", ItemId);
                HshIn.Add("intStoreCode", StoreId);
                HshIn.Add("bitItemsWithStock", ItemWithStock);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("intCompanyId", CompanyId);
                HshIn.Add("intTransactionId", TransactionId);
                HshIn.Add("intCallingPart", CallingPart);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhrBatchDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        /// <summary>
        /// For Save Item Issue Sale and Return
        /// Santosh
        /// </summary>
        /// <param name="iHospId"></param>
        /// <param name="iIssuedate"></param>
        /// <param name="iSaleSetupId"></param>
        /// <param name="iLoginStoreId"></param>
        /// <param name="iIssueStoreId"></param>
        /// <param name="iRegId"></param>
        /// <param name="iEncId"></param>
        /// <param name="iIssueEmpId"></param>
        /// <param name="iPatientname"></param>
        /// <param name="iAge"></param>
        /// <param name="iGender"></param>
        /// <param name="iCompanyId"></param>
        /// <param name="iRefByid"></param>
        /// <param name="iRefbyname"></param>
        /// <param name="iIssueReturn"></param>
        /// <param name="iProcessStatus"></param>
        /// <param name="iRequstId"></param>
        /// <param name="iInvoiceId"></param>
        /// <param name="iConcessionEmpId"></param>
        /// <param name="iConcessionRemarks"></param>
        /// <param name="iBillCatId"></param>
        /// <param name="iIsReceved"></param>
        /// <param name="iRecievDate"></param>
        /// <param name="iRecievById"></param>
        /// <param name="iPostedById"></param>
        /// <param name="iPosteddate"></param>
        /// <param name="iRemarks"></param>
        /// <param name="ixmlItemIssueSale"></param>
        /// <param name="iActive"></param>
        /// <param name="iEncodedby"></param>
        /// <returns></returns>
        public string SaveSaleIssue(int iHospId, int iFacilityId, int DocumentTypeId, DateTime iIssuedate, int iLoginStoreId, int iRegId,
                                      int iEncId, string iPatientname, string iAge, string iAgeType, string iGender,
                                      int iCompanyId, int iRefByid, string iDoctorName, string iIssueReturn, string iProcessStatus,
                                      int iRequstId, int iConcessionEmpId, string iConcessionRemarks, int iBillCatId, int iPostedById,
                                      DateTime iPosteddate, string iRemarks, string ixmlItemIssueSale, string ixmlPaymode, double iInvoiceAm,
                                      bool iActive, int iEncodedby, string SaleSetupCode, out string docNo, int iIssueId, out int oissueid,
                                      string RegistrationNo, string EmployeeNo, double CoPayAmount, double CoPayPercentage, string Source,
                                      double RoundOffAmount, string CheckCopayment, string ApprovalCode, double dRoundOffPatient,
                                      double dRoundOffPayer, string @chvUSERIP, out bool btReturnError, int ReasonId, bool bitValidateDepositExhaust = true)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                oissueid = 0;
                docNo = "";
                btReturnError = false;
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intDocTypeId", DocumentTypeId);
                HshIn.Add("@intLoginStoreId", iLoginStoreId);
                HshIn.Add("@intRegistrationId", iRegId);
                HshIn.Add("@intEncounterId", iEncId);
                HshIn.Add("@intRequestId", iRequstId);
                HshIn.Add("@chrRemarks", iRemarks);
                HshIn.Add("@xmlItemIssueSale", ixmlItemIssueSale);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intEncodedBy", iEncodedby);
                HshIn.Add("@chvUSERIP", @chvUSERIP);
                HshIn.Add("@chrSource", Source);

                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intIssueId", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                // HshIn.Add("@intRefById", iRefByid);

                string strSProc = "uspSavePhrIssueSale";

                if (SaleSetupCode == "IP-ISS" || SaleSetupCode == "IP-RET")
                {
                    if (SaleSetupCode == "IP-RET")
                    {
                        HshIn.Add("@intRefById", null);
                    }
                    else
                    {
                        HshIn.Add("@intRefById", iRefByid);
                    }
                    HshIn.Add("@chrIssueReturn", iIssueReturn);
                    HshIn.Add("@chrProcessStatus", iProcessStatus);
                    HshIn.Add("@intBillingCategoryId", iBillCatId);
                    HshIn.Add("@intPostedById", iPostedById);
                    HshOut.Add("@btReturnError", SqlDbType.Bit);
                    HshIn.Add("@bitValidateDepositExhaust", bitValidateDepositExhaust);
                    HshIn.Add("@ReasonId", ReasonId);
                    //HshIn.Add("@chrSource", Source);
                    strSProc = "uspPhrSaveIPIssue";
                }
                else
                {
                    HshIn.Add("@intRefById", iRefByid);
                    HshIn.Add("@intConcessionEmpId", iConcessionEmpId);//
                    HshIn.Add("@chrConcessionRemarks", iConcessionRemarks);
                    HshIn.Add("@dtIssueDate", iIssuedate);///
                    HshIn.Add("@xmlPaymentMode", ixmlPaymode);////
                    HshIn.Add("@chrPatientName", iPatientname);
                    HshIn.Add("@chrAge", iAge);
                    HshIn.Add("@chrAgeType", iAgeType);
                    HshIn.Add("@chrGender", iGender);
                    HshIn.Add("@intCompanyId", iCompanyId);
                    HshIn.Add("@chrRefByName", iDoctorName);
                    HshIn.Add("@chvRegistrationNo", RegistrationNo);
                    HshIn.Add("@chvEmployeeNo", EmployeeNo);
                    HshIn.Add("@mnCoPayAmount", CoPayAmount);
                    HshIn.Add("@mnCoPayPercentage", CoPayPercentage);
                    HshIn.Add("@roundOffAmount", RoundOffAmount);
                    HshIn.Add("@chrCoPayment", CheckCopayment);
                    HshIn.Add("@ApprovalCode", ApprovalCode);
                    HshIn.Add("@mRoundOffPatient", dRoundOffPatient);
                    HshIn.Add("@mRoundOffPayer", dRoundOffPayer);

                    strSProc = "uspSavePhrIssueSale";
                }
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, strSProc, HshIn, HshOut);
                string msg = Convert.ToString(HshOut["@intIssueId"]);
                if (msg != "")
                {
                    oissueid = Convert.ToInt32(HshOut["@intIssueId"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }
                if (System.DBNull.Value != HshOut["@btReturnError"])
                {
                    btReturnError = Convert.ToBoolean(HshOut["@btReturnError"]);
                }
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveItemReturn(string SaleSetupCode, int HospId, int FacilityId, DateTime IssueDate,
                              int SaleSetupId, int StoreId, int RegistrationId, int EncounterId,
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
                HshIn.Add("@intConcessionEmpId", "");//
                HshIn.Add("@chrConcessionRemarks", "");
                HshIn.Add("@intRequestId", 0);// 
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
                HshIn.Add("@intDocTypeId", SaleSetupId);
                HshIn.Add("@chvEmployeeNo", EmployeeNo);
                HshIn.Add("@mRoundOffPatient", mRoundOffPatient);
                HshIn.Add("@mRoundOffPayer", mRoundOffPayer);


                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intIssueId", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                if (SaleSetupCode == "EDR")
                {
                    HshIn.Add("@chrSource", Source);
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveOrderReturn", HshIn, HshOut);
                }
                else
                {
                    HshIn.Add("@intRefIssueId", IssueId);
                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrIssueSale", HshIn, HshOut);
                }

                docN = Convert.ToString(HshOut["@chvDocumentNo"]);
                SavedIssueId = Convert.ToInt32(HshOut["@intIssueId"]);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string SaveItemReturnWithoutIssue(int HospId, int FacilityId, DateTime IssueDate,
                                   int SaleSetupId, int StoreId, int RegistrationId, string RegistrationNo,
                                   string PatientName, string Age, string AgeType, string Gender,
                                   int CompanyId, string Remarks, string xmlItemIssueSale,
                                   string xmlPaymentMode, int Active, int EncodedBy,
                                    out string docN, out int IssueId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDocTypeId", SaleSetupId);
                HshIn.Add("@dtIssueDate", IssueDate);
                HshIn.Add("@intLoginStoreId", StoreId);
                if (RegistrationId != 0)
                {
                    HshIn.Add("@intRegistrationId", RegistrationId);
                }
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chrPatientName", PatientName);
                HshIn.Add("@chrAge", Age);
                HshIn.Add("@chrAgeType", AgeType);
                HshIn.Add("@chrGender", Gender);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@chrRemarks", Remarks);
                HshIn.Add("@xmlItemIssueSale", xmlItemIssueSale);
                HshIn.Add("@xmlPaymentMode", xmlPaymentMode);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intIssueId", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveReturnWithoutIssue", HshIn, HshOut);
                docN = Convert.ToString(HshOut["@chvDocumentNo"]);
                IssueId = Convert.ToInt32(HshOut["@intIssueId"]);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        /// <summary>
        /// Department Issue Save
        /// </summary>
        /// <param name="iHospId"></param>
        /// <param name="iFacilityId"></param>
        /// <param name="iSaleSetupId"></param>
        /// <param name="iIssuedate"></param>
        /// <param name="iLoginStoreId"></param>
        /// <param name="iIssueStoreId"></param>
        /// <param name="iIssueFacilityId"></param>
        /// <param name="iIssueEmpId"></param>
        /// <param name="iIssueReturn"></param>
        /// <param name="iProcessStatus"></param>
        /// <param name="iRequstId"></param>
        /// <param name="iIsReceved"></param>
        /// <param name="iRecievDate"></param>
        /// <param name="iRecievById"></param>
        /// <param name="iPostedById"></param>
        /// <param name="iPosteddate"></param>
        /// <param name="iRemarks"></param>
        /// <param name="ixmlItemIssueSale"></param>
        /// <param name="iActive"></param>
        /// <param name="iEncodedby"></param>
        /// <param name="docNo"></param>
        /// <param name="oissueid"></param>
        /// <returns></returns>
        public string SaveDepartmentIssue(int IssueId, int iHospId, int iFacilityId, DateTime iIssuedate, int iLoginStoreId, int iIssueStoreId, int iIssueFacilityId,
                                      int iIssueEmpId, string iIssueReturn, string iProcessStatus, int iRequstId, string iRemarks,
                                      string ixmlItemIssueSale, bool iActive, int iEncodedby, out string docNo, out int oissueid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                oissueid = 0;
                docNo = "";

                HshIn.Add("@intIssueId", IssueId);
                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@dtIssueDate", iIssuedate);
                HshIn.Add("@intLoginStoreId", iLoginStoreId);
                HshIn.Add("@intIssueStoreId", iIssueStoreId);
                HshIn.Add("@intIssueFacilityId", iIssueFacilityId);
                HshIn.Add("@intIssueEmpId", iIssueEmpId);
                HshIn.Add("@chrIssueReturn", iIssueReturn);
                HshIn.Add("@chrProcessStatus", iProcessStatus);
                HshIn.Add("@intRequestId", iRequstId);
                HshIn.Add("@chrRemarks", iRemarks);
                HshIn.Add("@xmlItemIssueSale", ixmlItemIssueSale);
                HshIn.Add("@bitActive", iActive);
                HshIn.Add("@intEncodedBy", iEncodedby);
                HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
                HshOut.Add("@intIssueCode", SqlDbType.Int);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveDepartmentIssue", HshIn, HshOut);
                oissueid = Convert.ToInt32(HshOut["intIssueCode"]);
                docNo = Convert.ToString(HshOut["chvDocumentNo"]);

                string msg = Convert.ToString(HshOut["@intIssueCode"]);
                if (msg != "")
                {
                    oissueid = Convert.ToInt32(HshOut["@intIssueCode"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        /// <summary>
        /// For Save Store Data
        /// Santosh Kumar
        /// </summary>
        /// <param name="iHospId"></param>
        /// <param name="iDeptId"></param>
        /// <param name="iFromdate"></param>
        /// <param name="iTodate"></param>
        /// <param name="iActive"></param>
        /// <param name="iXmlData"></param>
        /// <param name="iEncodedby"></param>
        /// <returns></returns>
        public string SaveStoreMaster(int iHospId, int iStoreId, int iYearid, bool iActive, string XmlData, int iEncodedby, int copyPrev, int EmployeeId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("xmlFacility", XmlData);
                HshIn.Add("intYearId", iYearid);
                HshIn.Add("intStoreId", iStoreId);
                HshIn.Add("btActive", iActive);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshIn.Add("bitCopyPreviousTransactions", copyPrev);
                HshIn.Add("intEmployeeId", EmployeeId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrStoreMaster", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string UpdateEmployeeForDepartment(int iEncodedby, string xmlStoreMasterId, int EmployeeId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@xmlStoreMasterId", xmlStoreMasterId);
                HshIn.Add("intEncodedBy", iEncodedby);
                HshIn.Add("intEmployeeId", EmployeeId);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrUpdateEmployeeDepartment", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// Get Financial Year
        /// </summary>
        /// <returns></returns>
        public DataSet GetFinancialyear()
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrFinancialYear");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            return ds;
        }
        public DataSet GetStore(int YearId)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intYearId", YearId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrStoreFYWise", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GETAllStore(int FacilityId, int HospitalLocation)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocation);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRgetAllStore", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }


        /// <summary>
        /// Author : Rafat on 6 Jul 2012
        /// To Get Store Facility Wise and Permission of Document type, like GRN, PO etc...
        /// </summary>
        /// <param name="YearId"></param>
        /// <param name="FacilityId"></param>
        /// <param name="StoreType"></param>
        /// <returns></returns>
        public DataSet GetStore(int YearId, int FacilityId, string StoreType)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intYearId", YearId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvStoreType", StoreType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrStoreFYWise", HshIn);
                // 
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        //public DataSet GetStoreToChange(int HospId, int GroupId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@HospId", HospId);
        //        HshIn.Add("@GroupId", GroupId);

        //        string qry = " SELECT sm.StoreId, dm.DepartmentName FROM PhrStoreMaster  sm " +
        //                    " INNER JOIN SecGroupDepartments gd ON sm.StoreId = gd.DepartmentId " +
        //                    " LEFT JOIN DepartmentMain dm ON sm.StoreId = dm.DepartmentID " +
        //                    " WHERE sm.Active = 1 AND sm.HospitalLocationId = @HospId " +
        //                    " AND gd.GroupId = @GroupId " +
        //                    " AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,FromDate,111) and CONVERT(VARCHAR,ToDate,111) " +
        //                    " ORDER BY DepartmentName ";

        //        return objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}

        public DataSet GetStoreToChangefromWard(int HospId, int GroupId, int FacilityId, string strDocumentType)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@DocumentType", strDocumentType);

                string qry = " SELECT DISTINCT sm.StoreId, dm.DepartmentName FROM PhrStoreMaster sm WITH (NOLOCK) " +
                            " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON sm.StoreId = dm.DepartmentID  " +
                            " INNER JOIN FyDocumentNo fy WITH (NOLOCK) ON fy.yearid=sm.yearid AND sm.StoreId = fy.StoreId   " +
                            " WHERE FY.DocumentType = @DocumentType AND FY.Active = 1 AND sm.Active = 1 " +
                            " AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) AND CONVERT(VARCHAR,fy.ToDate,111) " +
                            " AND sm.facilityid = @FacilityId and sm.HospitalLocationId = @HospId " +
                            " ORDER BY dm.DepartmentName ";

                //string qry = " SELECT sm.StoreId, dm.DepartmentName  FROM PhrStoreMaster sm " +
                //             " inner join DepartmentMain dm ON sm.StoreId = dm.DepartmentID  " +
                //             " inner join FyDocumentNo fy on fy.yearid=sm.yearid and sm.StoreId = fy.StoreId   " +
                //             " WHERE sm.HospitalLocationId = @HospId and sm.facilityid=@FacilityId " +
                //             "  and fy.HospitalLocationId = @HospId and fy.facilityid=@FacilityId " +
                //             "	AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111)  " +
                //             "	AND FY.DocumentType ='IPD Issue' AND FY.Active = 1 and sm.Active = 1  " +
                //             " order by dm.DepartmentName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }


        //public DataSet GetStoreToChangefromWard(int HospId, int GroupId, int FacilityId, string strDocumentType)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@HospId", HospId);
        //        HshIn.Add("@GroupId", GroupId);
        //        HshIn.Add("@FacilityId", FacilityId);
        //        HshIn.Add("@DocumentType", strDocumentType);

        //        string qry = " SELECT DISTINCT sm.StoreId, dm.DepartmentName  FROM PhrStoreMaster sm " +
        //                    " inner join DepartmentMain dm ON sm.StoreId = dm.DepartmentID  " +
        //                    " inner join FyDocumentNo fy on fy.yearid=sm.yearid and sm.StoreId = fy.StoreId   " +
        //                    " WHERE sm.HospitalLocationId = @HospId and sm.facilityid=@FacilityId " +
        //                    "  and fy.HospitalLocationId = @HospId and fy.facilityid=@FacilityId " +
        //                    "	AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111)  " +
        //                    "	AND FY.DocumentType =@DocumentType AND FY.Active = 1 and sm.Active = 1  " +
        //                    " order by dm.DepartmentName ";



        //        //string qry = " SELECT sm.StoreId, dm.DepartmentName  FROM PhrStoreMaster sm " +
        //        //             " inner join DepartmentMain dm ON sm.StoreId = dm.DepartmentID  " +
        //        //             " inner join FyDocumentNo fy on fy.yearid=sm.yearid and sm.StoreId = fy.StoreId   " +
        //        //             " WHERE sm.HospitalLocationId = @HospId and sm.facilityid=@FacilityId " +
        //        //             "  and fy.HospitalLocationId = @HospId and fy.facilityid=@FacilityId " +
        //        //             "	AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111)  " +
        //        //             "	AND FY.DocumentType ='IPD Issue' AND FY.Active = 1 and sm.Active = 1  " +
        //        //             " order by dm.DepartmentName ";



        //        return objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}

        public DataSet GetStoreToChangefromWard(int HospId, int GroupId, int FacilityId, string strDocumentType, int DepartmentID)
        {
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@HospId", HospId);
                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@DocumentType", strDocumentType);
                HshIn.Add("@DepartmentID", DepartmentID);

                string qry = " SELECT DISTINCT sm.StoreId, dm.DepartmentName,ISNULL(dm.OrderEntryStore,0) as OrderEntryStore FROM PhrStoreMaster sm WITH (NOLOCK) " +
                            " inner join DepartmentMain dm WITH (NOLOCK) ON sm.StoreId = dm.DepartmentID  " +
                            " inner join FyDocumentNo fy WITH (NOLOCK) on fy.yearid=sm.yearid and sm.StoreId = fy.StoreId   " +
                            " WHERE FY.DocumentType =@DocumentType AND FY.Active = 1 and sm.Active = 1 " +
                            " AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111) " +
                            " AND sm.facilityid=@FacilityId AND fy.facilityid=@FacilityId and sm.HospitalLocationId = @HospId and fy.HospitalLocationId = @HospId ";

                if (DepartmentID > 0)
                {
                    qry = qry + " AND Dm.DepartmentID=@DepartmentID ";
                }

                qry = qry + " order by dm.DepartmentName ";


                //string qry = " SELECT sm.StoreId, dm.DepartmentName  FROM PhrStoreMaster sm " +
                //             " inner join DepartmentMain dm ON sm.StoreId = dm.DepartmentID  " +
                //             " inner join FyDocumentNo fy on fy.yearid=sm.yearid and sm.StoreId = fy.StoreId   " +
                //             " WHERE sm.HospitalLocationId = @HospId and sm.facilityid=@FacilityId " +
                //             "  and fy.HospitalLocationId = @HospId and fy.facilityid=@FacilityId " +
                //             "	AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111)  " +
                //             "	AND FY.DocumentType ='IPD Issue' AND FY.Active = 1 and sm.Active = 1  " +
                //             " order by dm.DepartmentName ";



                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }

        public DataSet GetStoreToChange(int HospId, int GroupId, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospId", HospId);
                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@FacilityId", FacilityId);

                //string qry = " SELECT sm.StoreId, dm.DepartmentName FROM PhrStoreMaster  sm " +
                //    " INNER JOIN SecGroupDepartments gd ON sm.StoreId = gd.DepartmentId " +
                //    " LEFT JOIN DepartmentMain dm ON sm.StoreId = dm.DepartmentID " +
                //    " inner join fyyearsmaster fy on fy.yearid=sm.yearid " +
                //    " WHERE sm.Active = 1 AND sm.HospitalLocationId = @HospId  and sm.facilityid=@FacilityId" +
                //    " AND gd.GroupId = @GroupId " +
                //    " AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111) " +
                //    " ORDER BY DepartmentName ";

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrChangeStore", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getUniqueStore(int HospId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospId", HospId);
                string qry = " SELECT DISTINCT sm.StoreId, dm.DepartmentName FROM PhrStoreMaster sm WITH (NOLOCK) " +
                            " LEFT JOIN DepartmentMain dm WITH (NOLOCK) ON sm.StoreId = dm.DepartmentID " +
                            " WHERE sm.Active = 1 AND sm.HospitalLocationId = @HospId " +
                            " ORDER BY DepartmentName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getInventoryTransactionType()
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string qry = " SELECT StatusId, [Status],StatusType as Type, Code FROM StatusMaster WITH (NOLOCK) " +
                             " WHERE StatusType IN ('SaleSetupType','ReturnSetupType','InventoryTrans','LinenTrans') " +
                             " AND Active = 1 ORDER BY Status, SequenceNo ";

                //string qry = " SELECT StatusId, Status, 'InventoryTrans' AS Type,Code FROM StatusMaster WHERE StatusType = 'InventoryTrans' AND Active = 1 " +
                //             " UNION ALL " +
                //             " SELECT SaleSetupId, SaleSetupName, 'SaleSetupType' AS Type ,'' as Code FROM PhrSaleSetupMaster WHERE Active = 1";
                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getReturnToSupplierTransactionType()
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string qry = " SELECT StatusId, [Status],StatusType as Type, Code FROM StatusMaster WITH (NOLOCK) " +
                             " WHERE Code IN ('SUP-I','SUP-C') " +
                             " AND Active = 1 ORDER BY Status, SequenceNo ";

                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getDepartment(int HospId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospId", HospId);
                //string qry = "SELECT DepartmentID, DepartmentName FROM  DepartmentMain " +
                //            " WHERE DepartmentID NOT IN(SELECT StoreId  FROM PhrStoreMaster WHERE Active = 1 AND HospitalLocationId = @HospId " +
                //            " AND (GETUTCDATE() BETWEEN FromDate AND ToDate)) ORDER BY DepartmentName ";

                string qry = "SELECT DepartmentID, DepartmentName FROM DepartmentMain WITH (NOLOCK) " +
                             " where Active=1 and HospitalLocationId=@HospId ORDER BY DepartmentName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //public string SaveSeriesSetup(int iHospId, string iXmlData, int iStoreId, DateTime FromDate, DateTime ToDate, int iEncodedBy, int YearId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn = new Hashtable();
        //    HshOut = new Hashtable();

        //    string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
        //    string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

        //    HshIn.Add("inyHospitalLocationId", iHospId);
        //    HshIn.Add("xmlDtore", iXmlData);
        //    HshIn.Add("intStoretId", iStoreId);
        //    HshIn.Add("dtFromDate", fDate);
        //    HshIn.Add("dtToDate", tDate);
        //    HshIn.Add("YearId", YearId);
        //    HshIn.Add("intEncodedBy", iEncodedBy);
        //    HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

        //    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrSeriesSetup", HshIn, HshOut);
        //    return HshOut["chvErrorStatus"].ToString();
        //}

        public string SaveSeriesSetup(int iHospId, string iXmlData, int iStoreId, DateTime FromDate, DateTime ToDate, int iEncodedBy, int YearId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                string fDate = FromDate.ToString("yyyy-MM-dd 00:00");
                string tDate = ToDate.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("inyHospitalLocationId", iHospId);
                HshIn.Add("xmlDtore", iXmlData);
                HshIn.Add("intStoretId", iStoreId);
                HshIn.Add("dtFromDate", fDate);
                HshIn.Add("dtToDate", tDate);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("YearId", YearId);
                HshIn.Add("intEncodedBy", iEncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrSeriesSetup", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getStoreWiseSeriesSetup(int iHospId, int iStoreId, int iYearId, int iFacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospId", iHospId);
                HshIn.Add("@StoreId", iStoreId);
                HshIn.Add("@YearId", iYearId);
                HshIn.Add("@FacilityId", iFacilityId);

                string qry = "SELECT DISTINCT dn.StoreId,dm.DepartmentName as Storename,dn.DocumentType,dn.id,dn.Prefix as Code,dn.DocumentNo,dn.Active,dn.DocumentTypeId FROM FyDocumentNo dn WITH (NOLOCK) " +
                          " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON dn.StoreId=dm.DepartmentID " +
                          " WHERE dn.StoreId=@StoreId and YearId = @YearId and dn.FacilityId = @FacilityId and dn.HospitalLocationID = @HospId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        /// <summary>
        /// Get Item Sale Details
        /// </summary>
        /// <param name="HospId"></param>
        /// <param name="StorId"></param>
        /// <param name="SaleSetupId"></param>
        /// <param name="DocNo"></param>
        /// <param name="PatientName"></param>
        /// <param name="dtsDateFrom"></param>
        /// <param name="dtsDateTo"></param>
        /// <param name="EncodedBy"></param>
        /// <param name="Facilityid"></param>
        /// <returns></returns>
        public DataSet GetDocumentDetails(int HospId, int StorId, int SaleSetupId, string DocNo, string ReturnNo, string RegNo, string PatientName, string EncounterNo, string BedNo, DateTime dtsDateFrom, DateTime dtsDateTo,
                                              string IssueReturn, int EncodedBy, int Facilityid, string sOPIP, string status) // string RecordButton, int RowNo, int PageSize,
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", Facilityid);
                HshIn.Add("@intLoginStoreId", StorId);
                HshIn.Add("@intSaleSetupId", SaleSetupId);
                HshIn.Add("@chvDocNo", DocNo);
                HshIn.Add("@chvReturnNo", ReturnNo);
                HshIn.Add("@chvRegNo", RegNo);
                HshIn.Add("@chvPatientName", "%" + PatientName.Trim() + "%");
                HshIn.Add("@dtsDocFromDate", fDate);
                HshIn.Add("@dtsDocToDate", tDate);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvCurrentBedNo", BedNo);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshIn.Add("@chvOPIP", sOPIP);
                HshIn.Add("@chrIssueReturn", IssueReturn);
                HshIn.Add("@chrIssueStatus", status);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                if (IssueReturn == "R" && sOPIP == "O")
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrReturnDocuments", HshIn, HshOut);
                }
                else
                {
                    return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhrDocumentDetails", HshIn, HshOut);
                }

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDepartmentIssueDetails(int HospId, int StorId, int IssueStoreId, int IssueFacilityId, string DocNo, DateTime dtsDateFrom, DateTime dtsDateTo,
                                          string Depttype, int EncodedBy, int Facilityid, string IssueType, string ProcessStatus)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string fDate = dtsDateFrom.ToString("yyyy-MM-dd");
                string tDate = dtsDateTo.ToString("yyyy-MM-dd");

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", Facilityid);
                HshIn.Add("intLoginStoreId", StorId);
                HshIn.Add("chrIssueType", IssueType);
                HshIn.Add("intIssueFacilityId", IssueFacilityId);
                HshIn.Add("intIssueStoreId", IssueStoreId);
                HshIn.Add("chvDocumentNo", DocNo);
                HshIn.Add("dtsDocFromDate", fDate);
                HshIn.Add("dtsDocToDate", tDate);
                HshIn.Add("chrIssueReturn", Depttype);
                HshIn.Add("chrProcessStatus", ProcessStatus);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDepartmentIssue", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetphrSaleIssueItem(string IssueReturn, int HospId, int StoreId, int FacilityId,
                                        int SaleSetupId, int DocId, string DocNo, string sOPIP)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDocId", DocId);
                HshIn.Add("@chvDocNo", DocNo);

                if (sOPIP == "O" && IssueReturn == "R")
                {

                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrReturnItemDetails", HshIn);
                }
                else
                {
                    HshIn.Add("@intSaleSetupType", SaleSetupId);
                    HshIn.Add("@chrOPIP", sOPIP);
                    HshIn.Add("@chrIssueReturn", IssueReturn);
                    return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrSaleIssueItem", HshIn);
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetphrSaleItem(string IssueReturn, int HospId, int StoreId, int FacilityId,
                                       int SaleSetupId, int DocId, string DocNo, string sOPIP, string RegistrationNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDocId", DocId);
                HshIn.Add("@chvDocNo", DocNo);
                HshIn.Add("@intSaleSetupType", SaleSetupId);
                HshIn.Add("@chrRegistrationNo", RegistrationNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrReturnItemDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDepartmentIssueDetail(int HospId, int FacilityId, int StoreId, int IssueId, string IssueNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intIssueId", IssueId);
                HshIn.Add("@chvIssueNo", IssueNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDepartmentIssueDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetphrSaleIssuePaymentmode(int HospId, int FacilityId, int StoreId, int IssueId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                //HshIn.Add("inyHospitalLocationId", HospId);
                //HshIn.Add("intFacilityId", FacilityId);
                //HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intIssueId", IssueId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrSaleIssuePayment", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetphrDepartmentIssueRequest(int HospId, int FacilityId, int SaleSetupId, int IssueId, int LStoreId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intSaleSetupType", SaleSetupId);
                HshIn.Add("intIssueId", IssueId);
                HshIn.Add("intIssueStoreId", LStoreId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetphrSaleIssuePayment", HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetphrDepartmentIndentRequest(int HospId, int FacilityId, int DeptId, int IssueId, int IndentId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intDepartmentId", DeptId);
                HshIn.Add("intSaleSetupId", IssueId);
                HshIn.Add("intIndentId", IndentId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "GetphrIndentItemAgainstDepartment", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getItemMasterBarCode(int HospId, int FacilityId, int StoreId, string BarCodeValue, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@chvBarCodeValue", BarCodeValue);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterBarCode", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId,
                 int GenericId, int UserId, int FacilityId, int SupplierId,
                 string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId)
        {
            return getItemsWithStock(HospId, StoreId, ItemId,
                        GenericId, UserId, FacilityId, SupplierId,
                        ItemName, WithStockOnly, ItemNo, ItemSubCategoryId, "");
        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId,
                        int GenericId, int UserId, int FacilityId, int SupplierId,
                        string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId, string NHISDrugsList)
        {
            return getItemsWithStock(HospId, StoreId, ItemId,
                        GenericId, UserId, FacilityId, SupplierId,
                        ItemName, WithStockOnly, ItemNo, ItemSubCategoryId, "", "");
        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId,
                        int GenericId, int UserId, int FacilityId, int SupplierId,
                        string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId, string NHISDrugsList, string Usedtype)
        {
            return getItemsWithStock(HospId, StoreId, ItemId,
                        GenericId, UserId, FacilityId, SupplierId,
                        ItemName, WithStockOnly, ItemNo, ItemSubCategoryId, "", "", false);
        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId,
                    int GenericId, int UserId, int FacilityId, int SupplierId,
                    string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId, string NHISDrugsList, string Usedtype, bool IsDepartmentIssue)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intItemId", ItemId);
            // HshIn.Add("intItemBrandId", ItemBrandId);
            HshIn.Add("intGenericId", GenericId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("chvItemNo", ItemNo);
            HshIn.Add("intEncodedBy", UserId);
            HshIn.Add("intItemSubCategoryId", ItemSubCategoryId);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("bitWithStockOnly", WithStockOnly);
            HshIn.Add("chrItemSearchFor", Usedtype);
            HshIn.Add("@chrNHISDrugsList", NHISDrugsList);

            if (IsDepartmentIssue)
            {
                HshIn.Add("BitIsDepartmentIssueConsumption", 1);
            }
            else
            {
                HshIn.Add("BitIsDepartmentIssueConsumption", 0);
            }


            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock", HshIn, HshOut);


        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId, int ItemBrandId,
                        int GenericId, int UserId, int FacilityId, int SupplierId,
                        string ItemName, int WithStockOnly)
        {
            return getItemsWithStock(HospId, StoreId, ItemId, ItemBrandId,
                          GenericId, UserId, FacilityId, SupplierId,
                          ItemName, WithStockOnly, "", 1);
        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId, int ItemBrandId,
                        int GenericId, int UserId, int FacilityId, int SupplierId,
                        string ItemName, int WithStockOnly, string NHISDrugsList)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intItemId", ItemId);
            HshIn.Add("bitRandomSearch", 1);
            HshIn.Add("intGenericId", GenericId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("intEncodedBy", UserId);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("bitWithStockOnly", WithStockOnly);

            HshIn.Add("@chrNHISDrugsList", NHISDrugsList);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock", HshIn, HshOut);


        }


        public DataSet getEMRItemsWithStock(int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intEncodedBy", EncodedBy);


            return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPharmacyItems", HshIn);


        }



        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId, int ItemBrandId,
                  int GenericId, int UserId, int FacilityId, int SupplierId,
                  string ItemName, int WithStockOnly, string NHISDrugsList, int iOT)
        {

            return getItemsWithStock(HospId, StoreId, ItemId, ItemBrandId,
                                    GenericId, UserId, FacilityId, SupplierId,
                                    ItemName, WithStockOnly, NHISDrugsList, iOT, string.Empty);
        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId, int ItemBrandId,
                       int GenericId, int UserId, int FacilityId, int SupplierId,
                       string ItemName, int WithStockOnly, string NHISDrugsList, int iOT, string UseFor)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intItemId", ItemId);
                HshIn.Add("bitRandomSearch", 1);
                HshIn.Add("intGenericId", GenericId);
                HshIn.Add("intSupplierId", SupplierId);
                HshIn.Add("intEncodedBy", UserId);
                HshIn.Add("chvItemName", ItemName);
                HshIn.Add("bitWithStockOnly", WithStockOnly);

                HshIn.Add("@chrNHISDrugsList", NHISDrugsList);
                HshIn.Add("@intOT", iOT);

                if (!UseFor.Equals(string.Empty))
                {
                    HshIn.Add("@chrSource", UseFor);
                }

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetGenericDetails(int GenericId, string GenericName, int Active, int HospId, int EncodedBy)
        {
            return GetGenericDetails(GenericId, GenericName, Active, HospId, EncodedBy, 0, 0);
        }

        public DataSet GetYear(string qry)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetItemType(int HospId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                string qry = " SELECT ItemFlagId,ItemFlagName FROM PhrItemFlagMaster WITH (NOLOCK) WHERE Active=1 AND HospitalLocationId=@inyHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetItemDetails(int HospId, string StoreID, string ItemSubCategoryId, int Reusable)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@anyHospitalLocationId", HospId);
                HshIn.Add("@intStoreId", StoreID);
                HshIn.Add("@ItemSubCategoryId", ItemSubCategoryId);
                HshIn.Add("@Reusable", Reusable);
                string qry = " SELECT distinct im.ItemId, im.ItemName FROM PhrItemMaster im WITH (NOLOCK) " +
                            " INNER JOIN PhrStoreItemSubGroupTagging isg WITH (NOLOCK) ON im.ItemSubCategoryId = isg.ItemSubCategoryId AND isg.Active = 1 ";

                if (StoreID != "")
                {
                    qry += " WHERE isg.StoreId IN(" + StoreID + ")  ";
                    if (ItemSubCategoryId != "")
                    {
                        qry += " and im.ItemSubCategoryId IN(" + ItemSubCategoryId + ") ";
                    }
                    if (Reusable == 1)
                    {
                        qry += " and im.Reusable =" + Reusable;
                    }

                    qry += " and im.Active = 1  and im.HospitalLocationId =@anyHospitalLocationId ";
                }
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetItemDetailsWithoutStore(int HospId, string ItemSubCategoryId, int Reusable)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@anyHospitalLocationId", HospId);
                HshIn.Add("@ItemSubCategoryId", ItemSubCategoryId);
                HshIn.Add("@Reusable", Reusable);
                string qry = " SELECT distinct ItemId, ItemName FROM PhrItemMaster WITH (NOLOCK) WHERE 2=2 ";

                if (ItemSubCategoryId != "")
                {
                    qry += " AND ItemSubCategoryId IN(" + ItemSubCategoryId + ") ";
                    if (Reusable == 1)
                    {
                        qry += " AND Reusable =" + Reusable;
                    }
                }

                qry += " AND Active = 1 and HospitalLocationId =@anyHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetFaciliTo(int HospId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intHospId", HospId);

                string strquery = " select FacilityID,Name from FacilityMaster WITH (NOLOCK) Where Active=1 AND HospitalLocationID=@intHospId ";
                return objDl.FillDataSet(CommandType.Text, strquery, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getQuotationDetails(int QuotationId, int StoreId, int SupplierId, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intQuotationId", QuotationId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrQuotationDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveQuotationList(int QuotationId, int HospId, int FacilityId, string xmlQuotDetails, string xmlQuotChargeItems,
                                        int EncodedBy, out int QuotId, string xmlFacility, string xmlSupplier, int StoreId)
        {
            QuotId = 0;

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intQuotationId", QuotationId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlQuotDetails", xmlQuotDetails);
            HshIn.Add("@xmlQuotChargeItems", xmlQuotChargeItems);
            HshIn.Add("@xmlFacility", xmlFacility);
            HshIn.Add("@xmlSupplier", xmlSupplier);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@intQuotId", SqlDbType.Int);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrQuotation", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }

        public DataSet getQuotation(int SupplierId, int StoreId, int HospId, int FacilityId, int Encodedby, DateTime FDate, DateTime TDate, string Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("@intSupplierId", SupplierId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intEncodedBy", Encodedby);
            HshIn.Add("@FDate", FDate);
            HshIn.Add("@TDate", TDate);
            HshIn.Add("@bitActive", Active);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhrQuotation", HshIn, HshOut);


        }
        public DataSet getQuotationSupplier(int QuotationId, int FacilityId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@QuotationId", QuotationId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspPhrQuotationSupplier", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string DeleteQuotation(int QuotationId, int HospId, int FacilityId, int SupplierId, int Encodedby)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intQuotationId", QuotationId);
                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDeletePhrQuotation", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteQuotationFacility(int QuotationId, int HospId, int FacilityId, int Encodedby)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intQuotationId", QuotationId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPPhrDeleteQuotationFacility", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string DeleteQuotationSupplier(int QuotationId, int HospId, int SupplierId, int Encodedby)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intQuotationId", QuotationId);
                HshIn.Add("@intSupplierId", SupplierId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", Encodedby);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPPhrDeleteQuotationSupplier", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string QuotationTagged(int QuotationId, string xmlFacility, string xmlSupplier, int HospId, int Encodedby)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intQuotationId", QuotationId);
                HshIn.Add("@xmlFacility", xmlFacility);
                HshIn.Add("@xmlSupplier", xmlSupplier);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPPhrQuotationTagged", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion

        #region rajeev class


        public DataSet GetQuotationChargeAndRate(int HospId, int StoreId, int SuppId, int EncodedBy, int FacilityId, int ItemId, int QuotationId)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intQuotationId", QuotationId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intSupplierId", SuppId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intItemId", ItemId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetQuotationChargeAndRate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //--------Generic Master--------

        public string saveGenericMaster(int iGenericId, int iHospID, string GenericName, string DosageDetails, string Indications,
            string ContraIndication, string SpecialPrecaution, string SideEffect, string DrugInteractions, int userId, int IsActive, int CIMSCategoryId, int CIMSSubCategoryId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intGenericId", iGenericId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@chvGenericName", GenericName.Trim());
            HshIn.Add("@intCIMSCategoryId", CIMSCategoryId);
            HshIn.Add("@intCIMSSubCategoryId", CIMSSubCategoryId);
            HshIn.Add("@chvDosageDetails", DosageDetails.Trim());
            HshIn.Add("@chvIndications", Indications.Trim());
            HshIn.Add("@chvContraIndication", ContraIndication.Trim());
            HshIn.Add("@chvSpecialPrecaution", SpecialPrecaution.Trim());
            HshIn.Add("@chvSideEffect", SideEffect.Trim());
            HshIn.Add("@chvDrugInteractions", DrugInteractions.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrGenericMaster", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetGenericOfItem(int ItemId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string qry = "";
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);

                qry = " select GenericId,ItemSubCategoryId,isnull(Monopoly,0) as Monopoly  from PhrItemMaster WITH (NOLOCK) where ItemId=@intItemId and Active = 1 ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetGenericMaster()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string qry = " select GenericName,GenericId from PhrGenericMaster WITH (NOLOCK) where Active = 1 order by GenericName";
                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetGenericDetails(int GenericId, string GenericName, int Active, int HospId, int EncodedBy, int CIMSCategoryId, int CIMSSubCategoryId)
        {


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@chvGenericName", GenericName);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intCIMSCategoryId", CIMSCategoryId);
                HshIn.Add("@intCIMSSubCategoryId", CIMSSubCategoryId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrGenericMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPhrGenericMasterStoreWise(int StoreId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intStoreId", StoreId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrGenericMasterStoreWise", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //--------------ItemUnitMaster------

        public DataSet GetItemUnitMaster(int ItemUnitId, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intItemUnitId", ItemUnitId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemUnitMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string saveItemUnitMaster(int ItemUnitId, string ItemUnitName, int PurchaseUnitId1, int ConversionFactor1,
                                        int PurchaseUnitId2, int ConversionFactor2, int IssueUnitId, int PackingId,
                                        int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intItemUnitId", ItemUnitId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@chvItemUnitName", ItemUnitName.Trim());
            HshIn.Add("@intPurchaseUnitId1", PurchaseUnitId1);
            HshIn.Add("@intConversionFactor1", ConversionFactor1);
            HshIn.Add("@intPurchaseUnitId2", PurchaseUnitId2);
            HshIn.Add("@intConversionFactor2", ConversionFactor2);
            HshIn.Add("@intIssueUnitId", IssueUnitId);
            HshIn.Add("@intPackingId", PackingId);
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemUnitMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        //--------------PackingMaster------

        public DataSet GetPakingMaster(int iPakingId, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@iPackingId", iPakingId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrPackingMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetItemPaking(int iHospID, int ItemId, int userId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@inyHospitalLocationId", iHospID);
                HshIn.Add("@intEncodedBy", userId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemPacking", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string savePackingMaster(int iPackingId, string PackingName, int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intPackingId", iPackingId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@chvPackingName", PackingName.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrPackingMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //--------------Unit Master------

        public DataSet GetUnitMaster(int UnitId, int Active, int FacilityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@UnitId", UnitId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrUnitMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string saveUnitMaster(int UnitId, string UnitName, int iHospID, int IsActive, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intUnitId", UnitId);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@chvUnitName", UnitName.Trim());
            HshIn.Add("@bitActive", IsActive);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrUnitMaster", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        //-----------PurchaseOrderNotes-------

        public string savePurchaseOrderNotes(int iNotesId, int iHospID, string sNotes,
                                            string sFlag, int iDefault, int iStoreType, int Active, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intNotesId", iNotesId);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@chvNotes", sNotes.Trim());
            HshIn.Add("@chvFlag", sFlag.Trim());
            HshIn.Add("@bitDefault", iDefault);
            // HshIn.Add("@intStoreTypeId", iStoreType);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavePhrPONotes", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPurchaseOrderNotes(int iNotesID, string flag, int Active)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intNotesID", iNotesID);
                HshIn.Add("@chrFlag", flag);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrPONotes", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //--------------Report------

        public DataSet GetItemCategoryName(string StoreID)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStoreId", StoreID);
                string qry = "";
                qry = "  select distinct t1.ItemCategoryId,t1.ItemCategoryName from PhrItemCategoryMaster t1 WITH (NOLOCK) ";
                //" from PhrItemCategoryMaster t1  inner join PhrItemCategoryDetails t2 on t1.ItemCategoryId=t2.ItemCategoryId " +
                //" inner join PhrStoreItemSubGroupTagging t3 on t2.ItemSubCategoryId=t3.ItemSubCategoryId ";
                ////if (StoreID != "")
                ////{
                //qry += " where t3.StoreId IN(" + StoreID + ")  ";
                ////}

                //qry += " AND t1.Active=1 order by t1.ItemCategoryName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetItemSubCategoryName(string StoreID, string ItemCategoryId)
        {
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@intStoreId", StoreID);
                HshIn.Add("@ItemCategoryId", ItemCategoryId);
                string qry = "";
                //qry = "  select distinct t2.ItemSubCategoryName ,t2.ItemSubCategoryId " +
                //" from PhrItemCategoryMaster t1 WITH (NOLOCK) inner join PhrItemCategoryDetails t2 WITH (NOLOCK) on t1.ItemCategoryId=t2.ItemCategoryId " +
                //" inner join PhrStoreItemSubGroupTagging t3 WITH (NOLOCK) on t2.ItemSubCategoryId=t3.ItemSubCategoryId where t2.active=1 and t1.active=1 and t3.active=1 ";


                qry = " SELECT DISTINCT t2.ItemSubCategoryName, t2.ItemSubCategoryId " +
                    " FROM PhrItemCategoryMaster t1 WITH (NOLOCK) " +
                    " INNER JOIN PhrItemCategoryDetails t2 WITH (NOLOCK) ON t1.ItemCategoryId=t2.ItemCategoryId AND t1.Active = 1 AND t2.Active = 1 " +
                    " INNER JOIN PhrStoreItemSubGroupTagging t3 WITH (NOLOCK) ON t2.ItemSubCategoryId=t3.ItemSubCategoryId AND t3.Active = 1 " +
                    " WHERE 2=2 ";

                if (StoreID != "")
                {
                    qry += " and t3.StoreId IN(" + StoreID + ")  ";
                    if (ItemCategoryId != "")
                    {
                        qry += " and t1.ItemCategoryId IN(" + ItemCategoryId + ") ";
                    }
                }
                qry += " order by t2.ItemSubCategoryName";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        public DataSet GetItemSubCategoryName(string ItemCategoryId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@ItemCategoryId", ItemCategoryId);
                //string qry = " select distinct t2.ItemSubCategoryName ,t2.ItemSubCategoryId " +
                //" from PhrItemCategoryMaster t1 WITH (NOLOCK) inner join PhrItemCategoryDetails t2 WITH (NOLOCK) on t1.ItemCategoryId=t2.ItemCategoryId " +
                //" inner join PhrStoreItemSubGroupTagging t3 WITH (NOLOCK) on t2.ItemSubCategoryId=t3.ItemSubCategoryId ";
                string qry = " select distinct t2.ItemSubCategoryName ,t2.ItemSubCategoryId " +
               " from PhrItemCategoryMaster t1 WITH (NOLOCK) inner join PhrItemCategoryDetails t2 WITH (NOLOCK) on t1.ItemCategoryId=t2.ItemCategoryId and t1.Active =1 and t2.Active =1 " +
               " inner join PhrStoreItemSubGroupTagging t3 WITH (NOLOCK) on t2.ItemSubCategoryId=t3.ItemSubCategoryId and t3.Active =1 and t2.Active =1";


                if (ItemCategoryId != "")
                {
                    qry += " where t1.ItemCategoryId IN(" + ItemCategoryId + ") ";
                }

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetFacility(string YearId, string StoreID)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intStoreId", StoreID);
                HshIn.Add("@YearId", YearId);
                string qry = "";
                //qry = " SELECT T2.FacilityID, T2.Name AS FacilityName " +
                //      " FROM PhrStoreMaster T1 INNER JOIN FacilityMaster T2 ON T2.FacilityID = T1.FacilityId " +
                //      " WHERE T1.YearId =@YearId  AND T1.StoreId =@intStoreId AND T1.Active = 1 ";
                qry = " SELECT T2.FacilityID, T2.Name AS FacilityName, ISNULL(e.FirstName,'') + ISNULL(' ' + e.MiddleName,'') + ISNULL(' ' + e.LastName,'') AS EmployeeName, T1.Id " +
                      " FROM PhrStoreMaster T1 WITH (NOLOCK) INNER JOIN FacilityMaster T2 WITH (NOLOCK) ON T2.FacilityID = T1.FacilityId  " +
                      " Left join Employee e WITH (NOLOCK) on e.ID = T1.EmpId and e.Active=1 " +
                      " WHERE T1.StoreId =@intStoreId AND T1.YearId =@YearId AND T1.Active = 1  ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }


        public DataSet GetUnTaggedFacility(string YearId, string StoreID)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@StoreId", StoreID);
                HshIn.Add("@YearId", YearId);
                string qry = " SELECT fm.FacilityID, Name FROM FacilityMaster fm WITH (NOLOCK) INNER JOIN PhrStoreMaster sm WITH (NOLOCK) ON sm.FacilityId = fm.FacilityId AND StoreId = @StoreId AND YearId = @YearId ORDER BY fm.Name ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveFinancialYear(int iYearId, int iHospID, DateTime FDate,
                                            DateTime TDate, int Active, int userId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intYearId", iYearId);
            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@dtFromDate", FDate);
            HshIn.Add("@dtToDate", TDate);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", userId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveFinancialYear", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetActiveAllFinancialyear()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string qry = " SELECT YearId, DBO.GetDateFormat(FromDate,'D') + ' - ' + DBO.GetDateFormat(ToDate,'D') As Date, Active,case when Active=1 then 'Active' else 'In-Active' end as Status FROM FyYearsMaster WITH (NOLOCK) WHERE Active = 1 ORDER BY YearId DESC";
            return objDl.FillDataSet(CommandType.Text, qry);

        }
        public DataSet GetAllFinancialyear()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                string qry = " SELECT  YearId, DBO.GetDateFormat(FromDate,'D') + ' - ' + DBO.GetDateFormat(ToDate,'D') As Date, Active,case when Active=1 then 'Active' else 'In-Active' end as Status FROM FyYearsMaster WITH (NOLOCK) ORDER BY YearId DESC";
                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetStockLedgerReportExcel(string ProcName, int iFacilityId, int iLoginFacilityId, int iHospId,
                       string storeId, DateTime FDate, DateTime TDate, string ReportType, string FilterString, int summary)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@inyHospitalLocationId", iHospId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvStoreId", storeId);
            HshIn.Add("@FromDate", FDate);
            HshIn.Add("@ToDate", TDate);
            HshIn.Add("@chrReportType", ReportType);
            HshIn.Add("@chvFilterString", FilterString);
            HshIn.Add("@bitSummary", summary);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, ProcName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetBinCardReportExcel(string ProcName, int iFacilityId, int iLoginFacilityId, int iHospId,
                     string storeId, DateTime FDate, DateTime TDate, string ReportType, string FilterString)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intLoginFacilityId", iLoginFacilityId);
            HshIn.Add("@inyHospitalLocationId", iHospId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvStoreId", storeId);
            HshIn.Add("@dtFromDate", FDate);
            HshIn.Add("@dtToDate", TDate);
            HshIn.Add("@chrReportType", ReportType);
            HshIn.Add("@chvFilterString", FilterString);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, ProcName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetStockAsOnReportExcel(string ProcName, int iFacilityId, int YearId,
                     string storeId, DateTime StockDate, string ReportType, string FilterString)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intYearId", YearId);
            HshIn.Add("@chvFacilityId", iFacilityId);
            HshIn.Add("@chvStoreId", storeId);
            HshIn.Add("@dtsStockDate", StockDate);
            HshIn.Add("@chrReportType", ReportType);
            HshIn.Add("@chvFilterCriteria", FilterString);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, ProcName, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //public DataSet GetTaggedFacility(int HospId, int YearId, int StoreId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        HshIn = new Hashtable();
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        //        HshIn.Add("@HospId", HospId);
        //        HshIn.Add("@YearId", YearId);
        //        HshIn.Add("@StoreId", StoreId);              
        //        string qry = "SELECT distinct DepartmentID, DepartmentName FROM  DepartmentMain dm "+
        //                     " inner join PhrStoreMaster sm on dm.DepartmentID=sm.StoreId and StoreId=@StoreId  and YearId=@YearId and sm.Active=1 " +
        //                     " WHERE  dm.Active = 1 AND dm.HospitalLocationId = @HospId "   ;

        //        return objDl.FillDataSet(CommandType.Text, qry, HshIn);
        //    }
        //    catch (Exception Ex)
        //    {
        //        throw Ex;
        //    }
        //   
        //}


        #endregion

        #region vineet class

        public Hashtable SaveManualIndent(int DepartmentFrom, int HospId, int DepartmentTo, int IndentBy,
           string DepartmentRemarks, int EncodedBy, string XMlString)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@intDepartmentFrom", DepartmentFrom);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intDepartmentTo", DepartmentTo);
            HshIn.Add("@intIndentBy", IndentBy);
            HshIn.Add("@DepartmentRemarks", DepartmentRemarks.Trim());
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlManualIndent", XMlString);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@chvIndentNo", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveManualIndent", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet SearchManualIndent(int flag, int DepartmentFrom, int DepartmentTo, string IndentNo,
            int IndentBy, string Status, bool addDateAsParameter, DateTime FromDate, DateTime Todate)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            if (DepartmentFrom != 0)
            {
                HshIn.Add("@intDepartmentFrom", DepartmentFrom);
            }
            if (DepartmentTo != 0)
            {
                HshIn.Add("@intDepartmentTo", DepartmentTo);
            }
            if (IndentNo != "")
            {
                HshIn.Add("@chrIndentNo", IndentNo);
            }
            if (IndentBy != 0)
            {
                HshIn.Add("@intEncodedBy", IndentBy);
            }
            if (Status != "")
            {
                HshIn.Add("@chrStatus", Status);
            }
            if (addDateAsParameter == true)
            {
                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", Todate);
            }

            HshIn.Add("@flag", flag);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchManualIndent", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetUnitIssueUnit(int ItemId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@ItemId", ItemId);
            string query = "SELECT DISTINCT T2.IssueUnitId, ISNULL(T3.UnitName,'') + ISNULL(' (' + T2.ItemUnitName + ')','') AS IssueUnitName ";
            query += " FROM PhrItemWithItemUnitTagging T1 WITH (NOLOCK) ";
            query += " INNER JOIN PhrItemUnitMaster T2 WITH (NOLOCK) ON T1.ItemUnitId = T2.ItemUnitId";
            query += " INNER JOIN PhrUnitMaster T3 WITH (NOLOCK) ON T2.IssueUnitId = T3.UnitId";
            query += " WHERE T1.ItemId = @ItemId AND T1.Active = 1";

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, query, HshIn);
                // return Convert.ToString(ds.Tables[0].Rows[0]["IssueUnitName"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string UpdateManualIndent(int EncodedBy, string XMlString, string IndentNo, int HospId)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@chrIndentNo", IndentNo);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlManualIndent", XMlString);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspUpdateManualIndent", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void updateStatus(string IndentNo)
        {
            HshIn = new Hashtable();
            HshIn.Add("@IndentNo", IndentNo);
            string Query = "UPDATE PhrIndentMain SET IndentStatus='P' WHERE IndentNo=@IndentNo";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                objDl.ExecuteNonQuery(CommandType.Text, Query, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet FillStore()
        {

            string Query = "select Status,Code from StatusMaster WITH (NOLOCK) where StatusType='Indent' ";
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, Query);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string UpdatePhrIndentApprovedQty(int EncodedBy, string XMlString, string sRemarks, int HospId, int intIndentId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlManualIndent", XMlString);
            HshIn.Add("@chvRemarks", sRemarks.Trim());
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intIndentId", intIndentId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspUpdateIndentApprovedQty", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        #endregion


        public int getAdvisingDoctorId(int IndentId, int EncounterId, int RegistrationId, int FacilityId, int HospId)
        {
            int AdvisingDoctorId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();


                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospId", HospId);

                string qry = " SELECT ConsultingDoctorId AS AdvisingDoctorId FROM Admission WITH (NOLOCK) " +
                            " WHERE EncounterId = @intEncounterId AND RegistrationId = @intRegistrationId " +
                            " AND Active = 1 AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospId ";

                if (IndentId > 0)
                {
                    qry = " SELECT AdvisingDoctorId FROM ICMIndentMain WITH (NOLOCK) " +
                            " WHERE IndentId = @intIndentId AND EncounterId = @intEncounterId AND RegistrationId = @intRegistrationId " +
                            " AND Active = 1 AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospId ";
                }
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    AdvisingDoctorId = Convert.ToInt32(ds.Tables[0].Rows[0]["AdvisingDoctorId"]);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

            return AdvisingDoctorId;
        }

        public string saveBounceItem(int ItemId, string TransactionType, int UserId, int storeId, string NewItemName, string Remarks)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@ItemId", ItemId);
            HshIn.Add("@TransactionType", TransactionType);
            HshIn.Add("@intEncodedBy", UserId);
            HshIn.Add("@intStoreId", storeId);
            HshIn.Add("@chvItemName", NewItemName);
            HshIn.Add("@chvRemarks", Remarks);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveBounce", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getBounceItem(int facilityId, string TransactionType, DateTime fromDate, DateTime toDate, string storeId)
        {
            HshIn = new Hashtable();


            HshIn.Add("@intLoginFacilityId", facilityId);
            HshIn.Add("@fromDate", fromDate);
            HshIn.Add("@ToDate", toDate);
            HshIn.Add("@chvStoreId", storeId);
            HshIn.Add("@transactionType", TransactionType);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrPrintBounceItems", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string DeleteBounceItem(int bounceId, int Userid, int ItemId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@bounceId", bounceId);
                HshIn.Add("@Userid", Userid);
                string query = "";
                if (ItemId == 0)
                {
                    query = "update PhrBounceItemsNew SET ACTIVE =0,LastChangedBy=@Userid,LastChangedDate=GETUTCDATE() where Id=@bounceId";
                }
                else
                {
                    query = "update PhrBounceItemsRegular SET ACTIVE =0,LastChangedBy=@Userid,LastChangedDate=GETUTCDATE() where Id=@bounceId";
                }
                HshOut = objDl.getOutputParametersValues(CommandType.Text, query, HshIn, HshOut);

                return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet PhrGetCompanyItemDiscount(int inyHospitalLocationId, int facilityId,
            int intTransactionId, int intEncodedBy, int CompanyId, int storeId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();



            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", facilityId);
            HshIn.Add("@intStoreId", storeId);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@intTransactionId", intTransactionId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetCompanyItemDiscount", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet PhrGetmarginitem(int inyHospitalLocationId, int facilityId,
          int Subcategoryid, int intEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();



            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", facilityId);
            HshIn.Add("@subCategoryId", Subcategoryid);
            HshIn.Add("@intEncodedBy", intEncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemMargin", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string PhrSaveCompanyItemDiscount(int inyHospitalLocationId, int facilityId,
            int intTransactionId, int intEncodedBy, int CompanyId, int storeId,
            string xmlItemGroupDiscount, string xmlItemSubGroupDiscount, string xmlItemDiscount)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();



            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", facilityId);
            HshIn.Add("@intStoreId", storeId);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@intTransactionId", intTransactionId);
            HshIn.Add("@xmlItemGroupDiscount", xmlItemGroupDiscount);
            HshIn.Add("@xmlItemSubGroupDiscount", xmlItemSubGroupDiscount);
            HshIn.Add("@xmlItemDiscount", xmlItemDiscount);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveCompanyItemDiscount", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string PhrSaveItemmargin(int inyHospitalLocationId, int facilityId,
           int intEncodedBy, string xmlItemMargin)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();



            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intFacilityId", facilityId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshIn.Add("@xmlItemmargin", xmlItemMargin);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveItemMargin", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveGRNCreditNote(string CreditNoteNo, int intCreditNoteId,
          string CreditDate, string CreditAmount, string DiffAmt, string Narration,
     int GRNid, int inyHospitalLocationId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intCreditNoteId", intCreditNoteId);
            HshIn.Add("@chvCreditNoteNo", CreditNoteNo);
            HshIn.Add("@dtCreditNoteDate", CreditDate);
            HshIn.Add("@mnCreditNoteAmount", CreditAmount);
            HshIn.Add("@intGRNId", GRNid);
            HshIn.Add("@DiffAMT", DiffAmt);
            HshIn.Add("@Narration", Narration);

            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveGRNCreditNote", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetGRNCreditNote(int inyHospitalLocationId, int facilityId,
            int intGRNId, int intEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();



            HshIn.Add("@inyHospitalLocationId", inyHospitalLocationId);
            HshIn.Add("@intLoginFacilityId", facilityId);
            HshIn.Add("@intGRNId", intGRNId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetGRNCreditNote", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string DeleteCrediotNote(int CreditNoteId, int Userid)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@CreditNoteId", CreditNoteId);
                HshIn.Add("@Userid", Userid);

                string query = "update PhrGRNCreditNote SET ACTIVE =0,LastChangedBy=@Userid,LastChangedDate=GETUTCDATE() where CreditNoteId=@CreditNoteId";
                HshOut = objDl.getOutputParametersValues(CommandType.Text, query, HshIn, HshOut);

                return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetAllCompany(int HospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@HospitalLocationId", HospitalId);
                return objDl.FillDataSet(CommandType.Text, "SELECT CompanyId,name + isnull(+'('+ShortName+')','') as Name,PaymentType FROM company WITH (NOLOCK) where Active=1 and HospitalLocationId=@HospitalLocationId order by name", hashtable);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDoctor(int HospitalId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                Hashtable HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                StringBuilder sbSQL = new StringBuilder();
                sbSQL.Append(" SELECT Id, isnull(FirstName,'') + isnull(' ' + MiddleName,'') + isnull(' ' + Lastname,'') as Name, ISNULL(Mobile,'') AS MobileNo");
                sbSQL.Append(" FROM employee WITH (NOLOCK) WHERE Employeetype = 4 And HospitalLocationId = @inyHospitalLocationId  ORDER BY Name ");

                return objDl.FillDataSet(CommandType.Text, sbSQL.ToString(), HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string getRegistrationIDFromRegistrationNo(string RegistrationNo, int HospitalLocId, int FacilityId)
        {
            Hashtable HshIn;
            int intRegistrationId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                if (RegistrationNo != "")
                {
                    HshIn = new Hashtable();
                    HshIn.Add("@RegistrationNo", RegistrationNo);
                    HshIn.Add("@HospitalLocationID", HospitalLocId);
                    HshIn.Add("@FacilityId", FacilityId);

                    intRegistrationId = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "select Id from Registration WITH (NOLOCK) where RegistrationNo = @RegistrationNo AND FacilityId = @FacilityId AND HospitalLocationId= @HospitalLocationID", HshIn));
                    return intRegistrationId.ToString();
                }
                else
                    return intRegistrationId.ToString();
            }
            catch (Exception ex)
            {
                return intRegistrationId.ToString();
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getUnSetteledInvoiceRecipts(int HospId, int RegId, int LoginFacilityId,
                                        DateTime sFromDate, DateTime sToDate, string srange, int iYearID,
                                        int iCompanyID, int iReceiptID, int iInvoiceID, string cPatientTypeOPIP, int PayerID
           , string DocNo, int RegistrationNo, string PatientName, string InvoiceType)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@iHospitalLocationId", HospId);
                HshIn.Add("@iLoginFacilityId", LoginFacilityId);
                HshIn.Add("@iRegID", RegId);
                HshIn.Add("@iCompanyID", iCompanyID);
                //HshIn.Add("@iReceiptId", iReceiptID);
                HshIn.Add("@iPayerID", PayerID);
                HshIn.Add("@iInvoiceId", iInvoiceID);
                HshIn.Add("@dtFromDate", sFromDate.ToString("yyyy-MM-dd"));
                HshIn.Add("@dtToDate", sToDate.ToString("yyyy-MM-dd"));
                HshIn.Add("@chvDocNo", DocNo);
                HshIn.Add("@intRegistrationNo", RegistrationNo);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chrInvoiceType", InvoiceType);
                //if (cPatientTypeOPIP != "")
                //    HshIn.Add("cPatientType", cPatientTypeOPIP);

                //if (srange == "4")
                //{
                //    HshIn.Add("dtFromDate", Convert.ToDateTime(sFromDate).ToString("dd/MM/yyyy"));
                //    HshIn.Add("dtToDate", Convert.ToDateTime(sToDate).ToString("dd/MM/yyyy"));
                //    //hsInput.Add("chvDateRange", 0);
                //}
                //else
                //{
                //    HshIn.Add("chvDateRange", srange);
                //}


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPPharmacyOutstanding", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getServiceOrderForCreditNote(int HospId, int RegistrationId, int SponsorId,
                                  int PayerId, int InvoiceId, int IsForSearch, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intSponsorId", SponsorId);
                HshIn.Add("@intPayerId", PayerId);
                HshIn.Add("@intInvoiceId", InvoiceId);
                HshIn.Add("@bitIsForSearch", IsForSearch);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetServiceOrderForCreditNote", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        //public DataSet GetOPTransactionType(string StatusCode )
        //{
        //    DAL.DAL objSave = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable hashtable = new Hashtable();
        //    hashtable.Add("@StatusCode", StatusCode);
        //    DataSet dataset;
        //    dataset = objSave.FillDataSet(CommandType.Text, " select StatusId,Status from StatusMaster where Code =@StatusCode and Active =1", hashtable);
        //    return dataset;
        //}

        public DataSet getItemCurrentStock(int ItemId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemCurrentStock", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getProfileItems(int HospId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospId", HospId);

                //string qry = "SELECT ItemId, ItemNo, ItemName  FROM PhrItemMaster " +
                //            " WHERE IsProfile = 1 AND Active = 1 AND HospitalLocationId = @HospId";

                //string qry = "SELECT I.ItemId, I.ItemNo, I.ItemName  FROM PhrItemMaster I " +
                //         " INNER JOIN PhrItemProfileDetails IP ON I.ItemId = IP.ProfileItemId AND IP.Active = 1 " +
                //         " WHERE I.IsProfile = 1 AND I.Active = 1 AND I.HospitalLocationId = @HospId";


                string qry = "SELECT I.ItemId, I.ItemNo, I.ItemName  FROM PhrItemMaster I WITH (NOLOCK) " +
                         " WHERE I.IsProfile = 1 AND I.Active = 1 AND I.HospitalLocationId = @HospId";


                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getVendorPostingGroup()
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select id,Description from VendorPostingGroup WITH (NOLOCK) order by Description ");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveItemProfileDetails(int HospId, int ProfileItemId, string xmlItems, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intProfileItemId", ProfileItemId);
            HshIn.Add("@xmlItems", xmlItems);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveItemProfileDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getItemProfileDetails(int ProfileItemId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intProfileItemId", ProfileItemId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetItemProfileDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getPatientProfileItemDetails(int ProfileItemId, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intProfileItemId", ProfileItemId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetPatientProfileItemDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int updateDocumentPrintStatus(int DocumentId, string DocumentType)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                Hashtable HshIn;
                HshIn = new Hashtable();

                if (DocumentType == "IPISSUE")
                {
                    HshIn.Add("@intIssueId", DocumentId);
                    return objDl.ExecuteNonQuery(CommandType.Text, "update phripissuemain set documentprinted = 1 where issueid = @intIssueId", HshIn);
                }
                return 1;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getDocumentPrintStatus(int DocumentId, string DocumentType)
        {
            Hashtable HshIn;
            HshIn = new Hashtable();
            DataSet ds = new DataSet();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                if (DocumentType == "IPISSUE")
                {
                    HshIn.Add("@intIssueId", DocumentId);
                    return objDl.FillDataSet(CommandType.Text, "select documentprinted from phripissuemain WITH (NOLOCK) where issueid = @intIssueId", HshIn);
                }
                else
                {

                    return ds = null;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public string SaveIndentApproval(int HospId, int facilityId, string xmlItems,
            int StoreId, int EncodedBy, string IndentType, int AllUser)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@facilityId", facilityId);
            HshIn.Add("@xmlEmployeeId", xmlItems);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@chrIndentType", IndentType);
            HshIn.Add("@bitAllUser", AllUser);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveIndentApproval", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEmployeeListIndentApproval(int HospId, int FacilityId, int intStoreId, string IndentType)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", intStoreId);
                HshIn.Add("@chrIndentType", IndentType);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRGetIndentApproval", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Int32 DeActivateEmployeeApproved(int iID, int iHospID, int iFacilityid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ID", iID);
                HshIn.Add("@HospID", iHospID);
                HshIn.Add("@FacilityID", iFacilityid);
                Int32 i = objDl.ExecuteNonQuery(CommandType.Text, "set Dateformat dmy UPDATE phrIndentApproval SET Active = '0' WHERE ID=@ID and HospitalLocationID = @HospID and FacilityId=FacilityID", HshIn);
                return i;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetphrReturnItemFroCancellation(int HospId, int StoreId, int FacilityId,
                                     int RegistrationId, string OPIP)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                if (OPIP != "")
                    HshIn.Add("@chvOPIP", OPIP);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPHRReturnCancellation", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public string CancelItemReturn(int HospitalLocationId, int FacilityId, int IssueId, int EncodedBy, string OPIP)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@IssueId", IssueId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@chvOPIP", OPIP);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPHRCancelItemReturn", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string CancelReturn(int HospitalLocationId, int FacilityId, string ProcessStatus, int ReturnId, int UserId)
        {

            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@ProcessStatus", ProcessStatus);
            HshIn.Add("@IssueId", ReturnId);
            HshIn.Add("@userId", UserId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = objDl.ExecuteNonQuery(CommandType.Text, "UPDATE PhrIPIssueMain Set ProcessStatus=@ProcessStatus,active=0,LastChangedBy=@userId,LastChangedDate=GETUTCDATE(),CancelDate=GETUTCDATE() where HospitalLocationId=@inyHospitalLocationId and FacilityId=@intFacilityId and IssueId=@IssueId and ProcessStatus='O'", HshIn);
                return "";
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPanelPriceitem(int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRgetitemPricepanel", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCompanyTypeWiseItemMrp(int HospitalLocationId, int FacilityId, int intStoreId
            , int intTransactionId, int intItemId, int intEncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intStoreId", intStoreId);
            HshIn.Add("@intTransactionId", intTransactionId);
            HshIn.Add("@intItemId", intItemId);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspPhrGetCompanyTypeWiseItemMrp", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveCompanyTypeWiseItemMrp(int HospitalLocationId, int FacilityId, int intStoreId
            , int intTransactionId, int intItemId, int intEncodedBy, string xmlItemMrp)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intStoreId", intStoreId);
            HshIn.Add("@intItemId", intItemId);
            HshIn.Add("@intTransactionId", intTransactionId);

            HshIn.Add("@xmlItemMrp", xmlItemMrp);
            HshIn.Add("@intEncodedBy", intEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspPhrSaveCompanyWiseItemMRP", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }






        }
        public DataSet GetReusablePriceitem(int HospitalLocationId, int FacilityId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRgetitemReusable", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetReusablePriceitemDetails(int itemId, int FacilityId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@intItemId", itemId);
            HshIn.Add("@intFacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRGetItemReusableDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetStoteForWard(int Hospitallocationid, int FacilityId, string EncounterNo, int RegistrationId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitallocationId", Hospitallocationid);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@intRegistrationId", RegistrationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPHRgetStoreWard", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveReusablePriceitemDetails(int HospitalLocationId, int itemId, int FacilityId, int TotalUnitReuse, int intUserId, string xmlIServiceDetails)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("@intItemId", itemId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intTotalUnitReuse", TotalUnitReuse);
            HshIn.Add("@intUserId", intUserId);
            HshIn.Add("@xmlIServiceDetails", xmlIServiceDetails);
            HshIn.Add("@intHospitalLocationId", HospitalLocationId);


            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPHRSaveReusableitems", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet PhrPendingReceiving(int storeId, int FacilityId)
        {
            HshIn = new Hashtable();

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intStoreId", storeId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspPhrPendingReceiving", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getItemCategoryDetailsForEquipmentReport(string ItemCategoryId, int HospId, int Active, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@chvItemCategoryId", ItemCategoryId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemCategoryDetailsForEquipmentReport", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by rakesh start
        public DataSet GetFood(int HospitalLocationId, int FacilityId, int Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFoods", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetUnit(int HospitalLocationId, int FacilityId, int Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUnits", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by rakesh end

        public DataSet ItemFlagName(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                string str = "";
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                str = "select ItemFlagId,ItemFlagName  from PhrItemFlagMaster WITH (NOLOCK) Where Active=1 And HospitalLocationId =@inyHospitalLocationId";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetCompanyTypeWiseItemSale(int HospitalLocationId, int FacilityId, int CompanyTypeId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intCompanyTypeId", CompanyTypeId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyTypeWiseItemSaleSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public bool getItemIsSubstituteNotAllowed(int EncounterId, int IndentId, int ItemId)
        {
            bool isNotAllow = false;
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intIndentId", IndentId);
                HshIn.Add("@intItemId", ItemId);

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrSubstituteNotAllowed", HshIn);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsSubstituteNotAllowed"]))
                        {
                            isNotAllow = true;
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return isNotAllow;
        }


        public DataSet getEmployeeWithResource(int HospId, int EncodedBy, int SpecializationId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();

            HshIn.Add("@iHospitalLocationId", HospId);
            HshIn.Add("@iUserId", EncodedBy);
            if (SpecializationId > 0)
            {
                HshIn.Add("@intSpecializationId", SpecializationId);
            }
            HshIn.Add("@intFacilityId", FacilityId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeWithResource", HshIn);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDrugInteraction(int HospId, int FacilityId, string IndentNo, int RegistrationNo, string EncounterNo, string PatientName,
                                        int WardId, int ProviderId, string InteractionRemarksStatus, string VisitType,
                                        int PageSize, int PageNo, string FromDate, string ToDate, int EncodedBy)
        {
            return getDrugInteraction(HospId, FacilityId, IndentNo, RegistrationNo, EncounterNo, PatientName,
                                        WardId, ProviderId, InteractionRemarksStatus, VisitType,
                                        PageSize, PageNo, FromDate, ToDate, EncodedBy, 0);

        }

        public DataSet getDrugInteraction(int HospId, int FacilityId, string IndentNo, int RegistrationNo, string EncounterNo, string PatientName,
                                        int WardId, int ProviderId, string InteractionRemarksStatus, string VisitType,
                                        int PageSize, int PageNo, string FromDate, string ToDate, int EncodedBy, int ItemFlagId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvIndentNo", IndentNo);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvPatientName", PatientName);
            HshIn.Add("@intWardId", WardId);
            HshIn.Add("@intProviderId", ProviderId);
            HshIn.Add("@chrInteractionRemarksStatus", InteractionRemarksStatus);
            HshIn.Add("@chrVisitType", VisitType);
            HshIn.Add("@inyPageSize", PageSize);
            HshIn.Add("@intPageNo", PageNo);

            if (FromDate != string.Empty && ToDate != string.Empty)
            {
                HshIn.Add("@chvFromDate", FromDate);
                HshIn.Add("@chvToDate", ToDate);
            }

            HshIn.Add("@intEncodedBy", EncodedBy);
            if (ItemFlagId > 0)
            {
                HshIn.Add("@intItemFlagId", ItemFlagId);
            }

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDrugInteraction", HshIn, HshOut);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public string saveDrugInteraction(int HospId, int FacilityId, string xmlInteractionDetails, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlInteractionDetails", xmlInteractionDetails);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);


            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveDrugInteraction", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetOPPharmacyOutstandingAck(int HospId, int RegId, int LoginFacilityId,
                                                DateTime sFromDate, DateTime sToDate, string srange, int iYearID,
                                                int iReceiptID, int iInvoiceID, string cPatientTypeOPIP, int PayerID
                   , string DocNo, int RegistrationNo, string PatientName)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("@iHospitalLocationId", HospId);
            HshIn.Add("@iLoginFacilityId", LoginFacilityId);
            HshIn.Add("@iRegID", RegId);
            HshIn.Add("@iPayerID", PayerID);
            HshIn.Add("@iInvoiceId", iInvoiceID);
            HshIn.Add("@dtFromDate", sFromDate.ToString("yyyy-MM-dd"));
            HshIn.Add("@dtToDate", sToDate.ToString("yyyy-MM-dd"));
            HshIn.Add("@chvDocNo", DocNo);
            HshIn.Add("@intRegistrationNo", RegistrationNo);
            HshIn.Add("@chvPatientName", PatientName);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetOPPharmacyOutstandingForAck", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getDrugAttributesMasterList(int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetDrugAttributesMasterList", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getItemAttributes(int HospId, int ItemId, int StoreId, int FacilityId, int EncodedBy)
        {
            return getItemAttributes(HospId, ItemId, StoreId, FacilityId, EncodedBy, 0, 0, 0);
        }

        public DataSet getItemAttributes(int HospId, int ItemId, int StoreId, int FacilityId, int EncodedBy, int DoctorId, int CompanyId, int GenericId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Change by kuldeep kumar 22/01/2021 
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                if (DoctorId > 0)
                {
                    HshIn.Add("@intDoctorId", DoctorId);
                }
                if (CompanyId > 0)
                {
                    HshIn.Add("@intCompanyId", CompanyId);
                }
                if (GenericId > 0)
                {
                    HshIn.Add("@intGenericId", GenericId);
                }
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetItemAttributes", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getGenericAttributes(int HospId, int FacilityId, int StoreId, int GenericId, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Change by kuldeep kumar 22/01/2021 
            try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intGenericId", GenericId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetGenericAttributes", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId, int ItemBrandId,
                                       int GenericId, int UserId, int FacilityId, int SupplierId,
                                       string ItemName, int WithStockOnly, string NHISDrugsList, int iOT, string UseFor,
                                       bool IsPackagePatient, bool IsPanelPatient)
        {
            return getItemsWithStock(HospId, StoreId, ItemId, ItemBrandId,
                                          GenericId, UserId, FacilityId, SupplierId,
                                          ItemName, WithStockOnly, NHISDrugsList, iOT, UseFor,
                                          IsPackagePatient, IsPanelPatient, false, 0, 0, "P", 0, 0);
        }

        public DataSet getItemsWithStock(int HospId, int StoreId, int ItemId, int ItemBrandId,
                                        int GenericId, int UserId, int FacilityId, int SupplierId,
                                        string ItemName, int WithStockOnly, string NHISDrugsList, int iOT, string UseFor,
                                        bool IsPackagePatient, bool IsPanelPatient, bool AllowPanelExcludedItems,
                                        int SecGroupId, int EmployeeId, string ItemSearchFor, int CompanyId, int UniqueCategoryId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intItemId", ItemId);
            HshIn.Add("bitRandomSearch", 1);
            HshIn.Add("intGenericId", GenericId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("intEncodedBy", UserId);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("bitWithStockOnly", WithStockOnly);

            HshIn.Add("@chrNHISDrugsList", NHISDrugsList);
            HshIn.Add("@intOT", iOT);

            if (!UseFor.Equals(string.Empty))
            {
                HshIn.Add("@chrSource", UseFor);
            }
            if (IsPackagePatient)
            {
                HshIn.Add("@bitIsPackagePatient", IsPackagePatient);
            }
            if (IsPanelPatient)
            {
                HshIn.Add("@bitIsPanelPatient", IsPanelPatient);
            }
            if (AllowPanelExcludedItems)
            {
                HshIn.Add("@bitAllowPanelExcludedItems", AllowPanelExcludedItems);
            }
            if (SecGroupId > 0)
            {
                HshIn.Add("@intSecGroupId", SecGroupId);
            }
            if (EmployeeId > 0)
            {
                HshIn.Add("@intEmployeeId", EmployeeId);
            }
            if (!ItemSearchFor.Equals(string.Empty))
            {
                HshIn.Add("@chrItemSearchFor", ItemSearchFor);
            }
            if (CompanyId > 0)
            {
                HshIn.Add("@intCompanyId", CompanyId);
            }
            if (UniqueCategoryId > 0)
            {
                HshIn.Add("@intUniqueCategoryId", UniqueCategoryId);
            }

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock", HshIn, HshOut);


        }

        public DataSet getItemsWithStock_Network(int HospId, int StoreId, int ItemId,
                int GenericId, int UserId, int FacilityId, int SupplierId,
                string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId, string Usedtype, int AllBrand, int RegistratioNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();

            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intItemId", ItemId);
            // HshIn.Add("intItemBrandId", ItemBrandId);
            HshIn.Add("intGenericId", GenericId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("chvItemNo", ItemNo);
            HshIn.Add("intEncodedBy", UserId);
            HshIn.Add("intItemSubCategoryId", ItemSubCategoryId);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("bitWithStockOnly", WithStockOnly);
            HshIn.Add("chrItemSearchFor", Usedtype);

            HshIn.Add("BitAll", AllBrand);
            HshIn.Add("RegistrationNo", RegistratioNo);



            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock", HshIn, HshOut);


        }

        public DataSet getItemsWithStock_NetworkV3(int HospId, int StoreId, int ItemId,
                int GenericId, int UserId, int FacilityId, int SupplierId,
                string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId, string Usedtype, int AllBrand, int RegistratioNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("inyHospitalLocationId", HospId);
            HshIn.Add("intFacilityId", FacilityId);
            HshIn.Add("intStoreId", StoreId);
            HshIn.Add("intItemId", ItemId);
            // HshIn.Add("intItemBrandId", ItemBrandId);
            HshIn.Add("intGenericId", GenericId);
            HshIn.Add("intSupplierId", SupplierId);
            HshIn.Add("chvItemNo", ItemNo);
            HshIn.Add("intEncodedBy", UserId);
            HshIn.Add("intItemSubCategoryId", ItemSubCategoryId);
            HshIn.Add("chvItemName", ItemName);
            HshIn.Add("bitWithStockOnly", WithStockOnly);
            HshIn.Add("chrItemSearchFor", Usedtype);

            HshIn.Add("BitAll", AllBrand);
            HshIn.Add("RegistrationNo", RegistratioNo);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStockV3", HshIn, HshOut);


        }

        public string SaveItemMaster(int ItemId, string ItemName, int HospId,
                    int ItemSubCategoryId, int GenericId, int CIMSCategoryId,
                    int CIMSSubCategoryId, int ManufactureId, string Specification,
                    int FormulationId, string xmlStrengthIds, int Active, int EncodedBy, string XMLPackingId,
                    string xmlItemWithItemUnitIds, bool ItemCloseForPurchase, bool ItemCloseForSale, int iBrandId, int StrengthId, int PackingId, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@chvItemName", ItemName.Trim());
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
            //HshIn.Add("@bitIsItem", IsItem);
            HshIn.Add("@intGenericId", GenericId);
            HshIn.Add("@intCIMSCategoryId", CIMSCategoryId);
            HshIn.Add("@intCIMSSubCategoryId", CIMSSubCategoryId);
            HshIn.Add("@intManufactureId", ManufactureId);
            HshIn.Add("@chvSpecification", Specification.Trim());
            HshIn.Add("@intFormulationId", FormulationId);
            HshIn.Add("@xmlStrengthIds", xmlStrengthIds);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@xmlPackingIds", XMLPackingId);
            HshIn.Add("@xmlItemWithItemUnitIds", xmlItemWithItemUnitIds);
            HshIn.Add("@bitItemCloseForPurchase", ItemCloseForPurchase);
            HshIn.Add("@bitItemCloseForSale", ItemCloseForSale);
            HshIn.Add("@intBrandId", iBrandId);
            HshIn.Add("@intStrengthId", StrengthId);
            HshIn.Add("@iPackingId", PackingId);
            HshIn.Add("@intFacilityId", FacilityId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspPhrSaveItemMaster", HshIn, HshOut);
            return HshOut["@chvErrorStatus"].ToString();
        }
        public string SaveItemCategoryDetails(int ItemSubCategoryId, string ItemSubCategoryName, string ItemSubCategoryShortName,
                                     int ItemCategoryId, int IsUnderSubCategory, int MainParentId, int ParentId, int HospId, int Active, int EncodedBy, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
            HshIn.Add("@chvItemSubCategoryName", ItemSubCategoryName.Trim());
            HshIn.Add("@chvItemSubCategoryShortName", ItemSubCategoryShortName.Trim());
            HshIn.Add("@intItemCategoryId", ItemCategoryId);
            HshIn.Add("@bitIsUnderSubCategory", IsUnderSubCategory);
            HshIn.Add("@intMainParentId", MainParentId);
            HshIn.Add("@intParentId", ParentId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intFacilityId", FacilityId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemCategoryDetails", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveItemMasterDetails(int ItemId, int HospId, int FacilityId,
                                int RequestedFacilityId, int ShelfLifeYears, int ShelfLifeMonths, int ShelfLifeDays, int RecommendedBy,
                                int IsFractionalIssue, int IsProfile, int IsVatInclude, string Specification, byte[] ItemImage, string ItemImageName,
                                string xmlItemWithItemUnitIds, string xmlItemChargeDetails,
                                string xmlItemFieldDetails, int Active, int EncodedBy, bool PanelpriceRequired, bool Reusable, double VatOnSale,
                                string Rack, bool Consumable, bool Useforbolpatient, bool IsSubstituteNotAllowed, int DepreciationDays, int DepreciationPercentage)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRequestedFacilityId", RequestedFacilityId);
            HshIn.Add("@intShelfLifeYears", ShelfLifeYears);
            HshIn.Add("@intShelfLifeMonths", ShelfLifeMonths);
            HshIn.Add("@intShelfLifeDays", ShelfLifeDays);
            HshIn.Add("@intRecommendedBy", RecommendedBy);
            HshIn.Add("@bitIsFractionalIssue", IsFractionalIssue);
            HshIn.Add("@bitIsProfile", IsProfile);
            HshIn.Add("@bitIsVatInclude", IsVatInclude);
            HshIn.Add("@chvSpecification", Specification.Trim());

            HshIn.Add("@xmlItemWithItemUnitIds", xmlItemWithItemUnitIds);
            HshIn.Add("@xmlItemChargeDetails", xmlItemChargeDetails);
            HshIn.Add("@xmlItemFieldDetails", xmlItemFieldDetails);

            HshIn.Add("@bitActive", Active);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@bitPanelpriceRequired", PanelpriceRequired);
            HshIn.Add("@bitReusable", Reusable);
            HshIn.Add("@VatOnSale", VatOnSale);
            HshIn.Add("@bitConsumable", Consumable);
            HshIn.Add("@bitUseforbplpatient", Useforbolpatient);
            HshIn.Add("@Rack", Rack);
            HshIn.Add("@bitIsSubstituteNotAllowed", IsSubstituteNotAllowed);
            HshIn.Add("@intDepreciationDays", DepreciationDays);
            HshIn.Add("@intDepreciationPercentage", DepreciationPercentage);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePhrItemMasterDetails", HshIn, HshOut);

                #region Update Image
                if (ItemImageName.Trim() != "")
                {
                    SqlConnection con = new SqlConnection(sConString);
                    SqlCommand cmdTemp;
                    SqlParameter prm1, prm2, prm3, prm4, prm5;

                    string qry = "";

                    qry = "IF EXISTS(SELECT Id FROM PhrItemMasterImage WITH (NOLOCK) WHERE ItemId = @intItemId) " +
                                " BEGIN " +
                                " UPDATE PhrItemMasterImage SET ItemImage = @imgItemImage, ItemImageName = @chvItemImageName, " +
                                " Active = @bitActive, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE() " +
                                " WHERE ItemId = @intItemId " +
                                " END " +
                                " ELSE " +
                                " BEGIN " +
                                " INSERT INTO PhrItemMasterImage(ItemId, ItemImage, ItemImageName, Active, EncodedBy, EncodedDate) " +
                                " SELECT @intItemId, @imgItemImage, @chvItemImageName, 1, @intEncodedBy, GETUTCDATE() " +
                                " END";

                    cmdTemp = new SqlCommand(qry, con);
                    cmdTemp.CommandType = CommandType.Text;

                    prm1 = new SqlParameter();
                    prm1.ParameterName = "@intItemId";
                    prm1.Value = Convert.ToInt32(ItemId);
                    prm1.SqlDbType = SqlDbType.Int;
                    prm1.Direction = ParameterDirection.Input;
                    cmdTemp.Parameters.Add(prm1);

                    prm2 = new SqlParameter();
                    prm2.ParameterName = "@imgItemImage";
                    prm2.Value = ItemImage;
                    prm2.SqlDbType = SqlDbType.Image;
                    prm2.Direction = ParameterDirection.Input;
                    cmdTemp.Parameters.Add(prm2);

                    prm3 = new SqlParameter();
                    prm3.ParameterName = "@chvItemImageName";
                    prm3.Value = ItemImageName;
                    prm3.SqlDbType = SqlDbType.VarChar;
                    prm3.Direction = ParameterDirection.Input;
                    cmdTemp.Parameters.Add(prm3);

                    prm4 = new SqlParameter();
                    prm4.ParameterName = "@intEncodedBy";
                    prm4.Value = EncodedBy;
                    prm4.SqlDbType = SqlDbType.Int;
                    prm4.Direction = ParameterDirection.Input;
                    cmdTemp.Parameters.Add(prm4);

                    prm5 = new SqlParameter();
                    prm5.ParameterName = "@bitActive";
                    prm5.Value = Active;
                    prm5.SqlDbType = SqlDbType.Bit;
                    prm5.Direction = ParameterDirection.Input;
                    cmdTemp.Parameters.Add(prm5);

                    con.Open();
                    cmdTemp.ExecuteNonQuery();
                }
                #endregion

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string ApprovedWardIndentRequest(int HospitalLocationId, int FacilityId, int IndentId, int ApprovedBy,
            string ApprovedStatus, String Remark, String XmlIndentDetails, int ToStoreId, String xmlReviewParameters)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@indentId", IndentId);
            HshIn.Add("@ApproveStatus", ApprovedStatus);
            HshIn.Add("@ApprovedBy", ApprovedBy);
            HshIn.Add("@Remark", Remark);
            HshIn.Add("@XmlIndentDetails", XmlIndentDetails);
            HshIn.Add("@XmlReviewParameters", xmlReviewParameters);
            HshIn.Add("@intToStoreId", ToStoreId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspPhrApprovedWardRequest", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        #region Sushil

        public DataSet getIPDItemBarCode(int HospId, int FacilityId, int StoreId, string EncounterNo, string BarCodeValue, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@chvEncounterNo", EncounterNo);
            HshIn.Add("@chvBarCodeValue", BarCodeValue);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrIPDItemBarCode", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getIPIssueItemDetailsBarcode(int EncounterId, string EncounterNo, int StoreId, int FacilityId,
                                       string barCode, int LoginFacilityId, int FromWard)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Change by kuldeep kumar 22/01/2021 
            try
            {

                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@barCodeValue", barCode);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@inyFromWard", FromWard);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPIssueItemDetailsBarcode", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getIPIssueItemDetailsBatch(int EncounterId, string EncounterNo, int StoreId, int FacilityId,
                                       string xmlItemId, string xmlBatchId, int LoginFacilityId, int FromWard)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Change by kuldeep kumar 22/01/2021 
            try
            {


                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@xmlItemId", xmlItemId);
                HshIn.Add("@xmlBatchId", xmlBatchId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@inyFromWard", FromWard);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspPhrGetIPIssueItemDetailsWithBatch", HshIn, HshOut);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getIPIssueItemDetailsWithAutoReturnQty(int EncounterId, string EncounterNo, int StoreId, int FacilityId,
                               string xmlItemId, string xmlBatchId, int LoginFacilityId, int FromWard, string xmlBatchReturn)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Change by kuldeep kumar 22/01/2021 
            try
            {


                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@xmlItemId", xmlItemId);
                HshIn.Add("@xmlBatchId", xmlBatchId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@inyFromWard", FromWard);
                HshIn.Add("@xmlBatchReturn", xmlBatchReturn);

                return objDl.FillDataSet(CommandType.StoredProcedure, "PhrDistributeReturnQtyBatchWise", HshIn, HshOut);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        #endregion
        public DataSet GetDiscountType()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPharmacyDiscountType");

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }

        }
        public string SaveSaleIssue(int iHospId, int iFacilityId, int DocumentTypeId, DateTime iIssuedate, int iLoginStoreId, int iRegId,
      int iEncId, string iPatientname, string iAge, string iAgeType, string iGender,
      int iCompanyId, int iRefByid, string iDoctorName, string iIssueReturn, string iProcessStatus,
      int iRequstId, int iConcessionEmpId, string iConcessionRemarks, int iBillCatId, int iPostedById,
      DateTime iPosteddate, string iRemarks, string ixmlItemIssueSale, string ixmlPaymode, double iInvoiceAm,
      bool iActive, int iEncodedby, string SaleSetupCode, out string docNo, int iIssueId, out int oissueid,
      string RegistrationNo, string EmployeeNo, double CoPayAmount, double CoPayPercentage, string Source,
      double RoundOffAmount, string CheckCopayment, string ApprovalCode, double dRoundOffPatient,
      double dRoundOffPayer, string @chvUSERIP, out bool btReturnError, string iMobileNo, string iaddress, DateTime iFollowupdate,
      string xmlDESP, bool isAllowCashSaleOutstanding, string EmpDiscounttype, string DiscountEmpNo, bool bitValidateDepositExhaust = true)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            oissueid = 0;
            docNo = "";
            btReturnError = false;
            HshIn.Add("@inyHospitalLocationId", iHospId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intDocTypeId", DocumentTypeId);
            HshIn.Add("@intLoginStoreId", iLoginStoreId);
            HshIn.Add("@intRegistrationId", iRegId);
            HshIn.Add("@intEncounterId", iEncId);
            HshIn.Add("@intRequestId", iRequstId);
            HshIn.Add("@chrRemarks", iRemarks);
            HshIn.Add("@xmlItemIssueSale", ixmlItemIssueSale);
            HshIn.Add("@bitActive", iActive);
            HshIn.Add("@intEncodedBy", iEncodedby);
            HshIn.Add("@chvUSERIP", @chvUSERIP);
            HshIn.Add("@chrSource", Source);

            HshOut.Add("@chvDocumentNo", SqlDbType.VarChar);
            HshOut.Add("@intIssueId", SqlDbType.Int);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            // HshIn.Add("@intRefById", iRefByid);

            string strSProc = "uspSavePhrIssueSale";

            if (SaleSetupCode == "IP-ISS" || SaleSetupCode == "IP-RET")
            {
                if (SaleSetupCode == "IP-RET")
                {
                    HshIn.Add("@intRefById", null);
                }
                else
                {
                    HshIn.Add("@intRefById", iRefByid);
                }
                HshIn.Add("@chrIssueReturn", iIssueReturn);
                HshIn.Add("@chrProcessStatus", iProcessStatus);
                HshIn.Add("@intBillingCategoryId", iBillCatId);
                HshIn.Add("@intPostedById", iPostedById);
                HshOut.Add("@btReturnError", SqlDbType.Bit);
                HshIn.Add("@bitValidateDepositExhaust", bitValidateDepositExhaust);
                //HshIn.Add("@chrSource", Source);
                strSProc = "uspPhrSaveIPIssue";
            }
            else
            {
                HshIn.Add("@intRefById", iRefByid);
                HshIn.Add("@intConcessionEmpId", iConcessionEmpId);//
                HshIn.Add("@chrConcessionRemarks", iConcessionRemarks);
                HshIn.Add("@dtIssueDate", iIssuedate);///
                HshIn.Add("@xmlPaymentMode", ixmlPaymode);////
                HshIn.Add("@chrPatientName", iPatientname);

                HshIn.Add("@chrPatientMobile", iMobileNo); // Add mobile Number and address field
                HshIn.Add("@chrPatientaddress", iaddress);
                HshIn.Add("@dtFollowupDate", iFollowupdate);

                HshIn.Add("@chrAge", iAge);
                HshIn.Add("@chrAgeType", iAgeType);
                HshIn.Add("@chrGender", iGender);
                HshIn.Add("@intCompanyId", iCompanyId);
                HshIn.Add("@chrRefByName", iDoctorName);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);
                HshIn.Add("@chvEmployeeNo", EmployeeNo);
                HshIn.Add("@mnCoPayAmount", CoPayAmount);
                HshIn.Add("@mnCoPayPercentage", CoPayPercentage);
                HshIn.Add("@roundOffAmount", RoundOffAmount);
                HshIn.Add("@chrCoPayment", CheckCopayment);
                HshIn.Add("@ApprovalCode", ApprovalCode);
                HshIn.Add("@mRoundOffPatient", dRoundOffPatient);
                HshIn.Add("@mRoundOffPayer", dRoundOffPayer);
                HshIn.Add("@xmlDespenseDetails", xmlDESP);
                HshIn.Add("@IsAllowCashSaleOutstanding", isAllowCashSaleOutstanding);
                HshIn.Add("@EmpDiscountType", EmpDiscounttype);
                HshIn.Add("@DiscountEmpNo", DiscountEmpNo);
                strSProc = "uspSavePhrIssueSale";
            }
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, strSProc, HshIn, HshOut);
                string msg = Convert.ToString(HshOut["@intIssueId"]);
                if (msg != "")
                {
                    oissueid = Convert.ToInt32(HshOut["@intIssueId"]);
                    docNo = Convert.ToString(HshOut["@chvDocumentNo"]);
                }
                btReturnError = Convert.ToBoolean(HshOut["@btReturnError"]);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public int IsPackagePatient(int EncounterId, string EncounterNo)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            int val = 0;

            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@chrEncounterNo", EncounterNo);

            try
            {
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckPackagePatientIP", HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = Convert.ToInt32(ds.Tables[0].Rows[0]["Value"]);
                }

                return val;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }
        }
        public string uspGetEmergencyPatientStatus(int EncounterId, string RegistrationNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@chrRegNo", RegistrationNo);

            HshOut.Add("@PatientStatus", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetEmergencyPatientStatus", HshIn, HshOut);

                return HshOut["@PatientStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string uspGetEmergencyDoctor(int EncounterId, string RegistrationNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@EncounterId", EncounterId);
            HshIn.Add("@chrRegNo", RegistrationNo);

            HshOut.Add("@DoctorId", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetEmergencyDoctor", HshIn, HshOut);

                return HshOut["@DoctorId"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPhrCreditLimit(int HospId, int FacilityId, int RegId, int PayerId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intRegId", RegId);
            HshIn.Add("@intPayerid", PayerId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPhrQuotation", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetLastPrescribedDocter(int intRegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@intRegId", intRegId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetLastPrescribedDoctor", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetItemRequiredAlertColor(int ItemId, string Flag)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chrFlag", Flag);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetItemRequiredAlert", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public string getHospitalSetupValue(string Flag, int HospId, int FacilityId)
        {
            string val = "";
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

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
            finally
            {
                ds.Dispose();
                objDl = null;
                HshIn = null;
            }
            return val;
        }

        public DataSet GetItemCategoryDetails(int ItemId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intItemId", ItemId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetItemCategoryDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetphrCheckBatchNoNearByExpiry(int HospId, int FacilityId, int StoreId, int ItemId)

        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("@inyHospitalLocationID", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intStoreId", StoreId);
            HshIn.Add("@intItemId", ItemId);

            HshOut.Add("chvMsg", SqlDbType.VarChar);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrCheckBatchNoNearByExpiry", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getItemConversionFactor(int ItemId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            HshIn.Add("@intItemId", ItemId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetUnitConversionFactors", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPrescriptionOPRequestItemDetails(int IndentId, int HospId, int EncodedBy, int StoreId, int Facilityid)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@intIndentID", IndentId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intStoreId", StoreId);
                HshIn.Add("@intFacilityId", Facilityid);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPrescriptionItemDetailsAuto", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                HshOut = null;
                objDl = null;
            }

        }
        public DataSet GetPendingPharmacyClearanceCount(int HospId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPendingPharmacyClearance", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string GetValidateEmployeeNo(string EmployeeNo)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            string val = "";

            HshIn.Add("@chrEmployeeNo", EmployeeNo);

            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspCheckEmployeeExist", HshIn);
            try
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = Convert.ToString(ds.Tables[0].Rows[0]["Value"]);
                }

                return val;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
        public DataSet GetStoreToChangeUserWise(int HospId, int UserId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@HospId", HospId);
                HshIn.Add("@UserId", UserId);
                HshIn.Add("@FacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrChangeStoreUserWise", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetWardStoreTag(int HospitalLocationId, int FacilityId, int MapID, int WardId, int Key)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intMapID", MapID);
                HshIn.Add("@intWardId", WardId);
                HshIn.Add("@intKey", Key);

                return objDl.FillDataSet(CommandType.StoredProcedure, "upcGetWardStoreTag", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string GetSupplierItemDetails(int ItemId, int iStoreId, int iHosID, int FacilityId, int BatchId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@intItemid", ItemId);
            HshIn.Add("@invHospitallocationId", iHosID);
            HshIn.Add("@intStoreid", iStoreId);
            HshIn.Add("@intFacilityid", FacilityId);
            HshIn.Add("@intBatchId", BatchId);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetSupplierItemVatDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetItemReusableValue(int ItemId) //my
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("intItemId", ItemId);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetItemReusableValue", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetSoundLookAlike(int HospId, int ItemId, int StatusId, string StatusCode, int GroupId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@ItemId", ItemId);
            HshIn.Add("@StatusId", StatusId);
            HshIn.Add("@StatusCode", StatusCode);
            HshIn.Add("@GroupId", GroupId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetItemSoundLookAlike", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getAllFormulationUnits()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string qry = "SELECT DISTINCT um.ID AS UnitId, um.Description AS UnitName, fu.FormId as FormulationId " +
                            " FROM EMRMedicationUnitMaster um WITH (NOLOCK) " +
                            " INNER JOIN EMRFormulationUnits fu WITH (NOLOCK) ON um.ID=fu.UnitId " +
                            " WHERE um.Active=1 " +
                            " ORDER BY fu.FormId, um.Description";


                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }

        }
        public DataSet getAllFormulationRoutes()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string qry = "SELECT DISTINCT mrm.ID AS RouteId, mrm.RouteName, mrm.IsDefault, fr.FormId AS FormulationId " +
                            " FROM EMRMedicationRouteMaster mrm WITH (NOLOCK) " +
                            " INNER JOIN EMRFormulationRoutes fr WITH (NOLOCK) ON mrm.ID=fr.RouteId " +
                            " WHERE mrm.Active=1 " +
                            " ORDER BY fr.FormId, mrm.RouteName";


                return objDl.FillDataSet(CommandType.Text, qry);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
            }
        }


        public DataSet GetPeriodTypeMaster(string LanguageCode)
        {
            DataSet ds = new DataSet();
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string strquery = "SELECT Id, TypeName, value" +
                                " FROM periodTypeMaster WITH(NOLOCK) " +
                                "WHERE LanguageCode='" + LanguageCode + "'" +
                                " ORDER BY Id";

                ds = (DataSet)objDl.FillDataSet(CommandType.Text, strquery, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }
            return ds;
        }

        public DataSet getItemsWithStock_Network_ddc(int HospId, int StoreId, int ItemId,
        int GenericId, int UserId, int FacilityId, int SupplierId,
        string ItemName, int WithStockOnly, string ItemNo, int ItemSubCategoryId, string Usedtype, int AllBrand, int RegistratioNo)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            // Change by kuldeep kumar 22/01/2021 
            try
            {

                HshIn.Add("inyHospitalLocationId", HospId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intStoreId", StoreId);
                HshIn.Add("intItemId", ItemId);
                // HshIn.Add("intItemBrandId", ItemBrandId);
                HshIn.Add("intGenericId", GenericId);
                HshIn.Add("intSupplierId", SupplierId);
                HshIn.Add("chvItemNo", ItemNo);
                HshIn.Add("intEncodedBy", UserId);
                HshIn.Add("intItemSubCategoryId", ItemSubCategoryId);
                HshIn.Add("chvItemName", ItemName);
                HshIn.Add("bitWithStockOnly", WithStockOnly);
                HshIn.Add("chrItemSearchFor", Usedtype);

                HshIn.Add("BitAll", AllBrand);
                HshIn.Add("RegistrationNo", RegistratioNo);



                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemMasterWithStock_ddc", HshIn, HshOut);


            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetLowCostCompanyTagging(int HospitalId, int CompanyId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hashtable = new Hashtable();
            DataSet dataset = new DataSet();
            try
            {
                hashtable.Add("@inyHospitalLoactionId", HospitalId);
                hashtable.Add("@Companyid", CompanyId);
                hashtable.Add("@FacilityId", FacilityId);

                dataset = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrCompanyCostTagging", hashtable);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }

            return dataset;
        }

        //Akshay
        public DataSet GetPhrItemMaster(int ItemId, string ItemName, int StoreId, int HospitalId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hashtable = new Hashtable();
            DataSet dataset = new DataSet();
            try
            {
                hashtable.Add("@intItemId", ItemId);
                hashtable.Add("@chvItemName", ItemName);
                hashtable.Add("@intStoreId", StoreId);
                hashtable.Add("@inyHospitalLocationId", HospitalId);

                dataset = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemDetails", hashtable);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                objDl = null;
            }

            return dataset;
        }

        //Akshay
        public DataSet GetNarcoticDrugsDetails(int HospId, int FacilityId, int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds;
            ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intItemId", ItemId);

            ds = objDl.FillDataSet(CommandType.Text, "select im.ItemId, im.ItemName, pimd.MaxDrugDose from phrItemMaster im inner join PhrItemMasterDetails pimd on im.ItemId = pimd.ItemId where im.ItemId = @intItemId and im.HospitalLocationId = @intHospitalLocationId and im.facilityId = @intFacilityId and pimd.Active=1", HshIn);
            return ds;
        }


    }
}
