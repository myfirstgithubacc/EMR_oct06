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
        public DataSet getChargeSetup(int ChargeId, string ChargeType, int HospId, int Active, int EncodedBy, string GSTApplicableFor)
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
                HshIn.Add("@chrGSTApplicableFor", GSTApplicableFor);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrChargeSetup", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getItemChargeDetailsGST(int HospId, int ItemId, string GSTApplicablefor)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chrGSTApplicablefor", GSTApplicablefor);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrItemChargeDetails", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getChargeMaster(int ChargeId)
        {

            HshIn = new Hashtable();

            HshIn.Add("@chargeId", ChargeId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrChargeMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public string SaveGSTChargeExclusion(int GRNTypeId, string XMLChargeDetails, int EncodedBy)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            HshIn.Add("@GrnTypeId", GRNTypeId);
            HshIn.Add("@XMLCharges", XMLChargeDetails);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSavephrGstChargeExclusion", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getGSTChargeExclusion(int GSTTypeID)
        {

            HshIn = new Hashtable();

            HshIn.Add("@GSTTypeId", GSTTypeID);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetGSTChargeExclusion", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public DataSet getItemCharges(int ItemId)
        {

            HshIn = new Hashtable();

            HshIn.Add("@intItemId", ItemId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetItemCharges", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public DataSet GetHSNGroup()
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspgetPhrHSNGroupMaster", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //string qry = "Select  Id,HSNGroup from PhrHSNGroupMaster";




        }
        public DataSet getHsnCodeMaster(int HSNGroup)
        {

            HshIn = new Hashtable();

            HshIn.Add("@HSNGroupId", HSNGroup);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspgetPhrHSNCodeMaster", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public string SaveSupplierMaster(string saveFor, int Id, int HospitalLocationId, string Name,
                            string ShortName, int SupplierTypeId, int SupplierCategoryTypeId,
                            int ChallanAccept, string Add1, string Add2, string Add3, string EMail,
                            int CountryId, int StateId, int CityId, int ZipId, string Mobile, string Phone, string Fax,
                            string ContactPerson, string Designation, string ContactPersonMobile,
                            string xmlTaxDetails, int Active, int EncodedBy, string xmlFacility, int ValidityPeriod,
                            string PaymentMode, int CreditMonths, int CreditDays, int VendorPostingGroupId, string MedicalReprsentativeName,
                            string AreaSalesManager, string ZonalSalesManage, string RegionalSalesManager, string DocNo, bool bIsCSSDManufacture, int SupplierOutbound, string GSTIN, int VendorType, int isGSTIN)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
                    HshIn.Add("@bitOutbound", SupplierOutbound);
                    HshIn.Add("@intEncodedBy", EncodedBy);
                    HshIn.Add("@intValidityPeriodInDays", ValidityPeriod);
                    HshIn.Add("@intVendorPostingGroupId", VendorPostingGroupId);
                    HshIn.Add("@ChvGSTIN", GSTIN);
                    HshIn.Add("@BitIsGSTIN", isGSTIN);
                    HshIn.Add("@intVendortype", VendorType);
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

        public string SaveSupplierMaster(string saveFor, int Id, int HospitalLocationId, string Name,
                            string ShortName, int SupplierTypeId, int SupplierCategoryTypeId,
                            int ChallanAccept, string Add1, string Add2, string Add3, string EMail,
                            int CountryId, int StateId, int CityId, int ZipId, string Mobile, string Phone, string Fax,
                            string ContactPerson, string Designation, string ContactPersonMobile,
                            string xmlTaxDetails, int Active, int EncodedBy, string xmlFacility, int ValidityPeriod,
                            string PaymentMode, int CreditMonths, int CreditDays, int VendorPostingGroupId, string MedicalReprsentativeName,
                            string AreaSalesManager, string ZonalSalesManage, string RegionalSalesManager, string DocNo, bool bIsCSSDManufacture,
                            int SupplierOutbound, string GSTIN, int VendorType, int isGSTIN, int IsGSTonFreeItems)
        {
            return SaveSupplierMaster(saveFor, Id, HospitalLocationId, Name,
                             ShortName, SupplierTypeId, SupplierCategoryTypeId,
                             ChallanAccept, Add1, Add2, Add3, EMail,
                             CountryId, StateId, CityId, ZipId, Mobile, Phone, Fax,
                             ContactPerson, Designation, ContactPersonMobile,
                             xmlTaxDetails, Active, EncodedBy, xmlFacility, ValidityPeriod,
                             PaymentMode, CreditMonths, CreditDays, VendorPostingGroupId, MedicalReprsentativeName,
                             AreaSalesManager, ZonalSalesManage, RegionalSalesManager, DocNo, bIsCSSDManufacture,
                             SupplierOutbound, GSTIN, VendorType, isGSTIN, IsGSTonFreeItems, false);
        }
        public string SaveSupplierMaster(string saveFor, int Id, int HospitalLocationId, string Name,
                            string ShortName, int SupplierTypeId, int SupplierCategoryTypeId,
                            int ChallanAccept, string Add1, string Add2, string Add3, string EMail,
                            int CountryId, int StateId, int CityId, int ZipId, string Mobile, string Phone, string Fax,
                            string ContactPerson, string Designation, string ContactPersonMobile,
                            string xmlTaxDetails, int Active, int EncodedBy, string xmlFacility, int ValidityPeriod,
                            string PaymentMode, int CreditMonths, int CreditDays, int VendorPostingGroupId, string MedicalReprsentativeName,
                            string AreaSalesManager, string ZonalSalesManage, string RegionalSalesManager, string DocNo, bool bIsCSSDManufacture,
                            int SupplierOutbound, string GSTIN, int VendorType, int isGSTIN, int IsGSTonFreeItems, bool isConsignmentSupplier)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
                    HshIn.Add("@bitOutbound", SupplierOutbound);
                    HshIn.Add("@intEncodedBy", EncodedBy);
                    HshIn.Add("@intValidityPeriodInDays", ValidityPeriod);
                    HshIn.Add("@intVendorPostingGroupId", VendorPostingGroupId);
                    HshIn.Add("@ChvGSTIN", GSTIN);
                    HshIn.Add("@BitIsGSTIN", isGSTIN);
                    HshIn.Add("@intVendortype", VendorType);

                    HshIn.Add("@bitIsGSTonFreeItems", IsGSTonFreeItems);
                    HshIn.Add("@isConsignmentSupplier", isConsignmentSupplier);
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

        public string SaveItemMasterDetails(int ItemId, int HospId, int FacilityId,
                                        int RequestedFacilityId, int ShelfLifeYears, int ShelfLifeMonths, int ShelfLifeDays, int RecommendedBy,
                                        int IsFractionalIssue, int IsProfile, int IsVatInclude, string Specification, byte[] ItemImage, string ItemImageName,
                                        string xmlItemWithItemUnitIds, string xmlItemChargeDetails,
                                        string xmlItemFieldDetails, int Active, int EncodedBy, bool PanelpriceRequired, bool Reusable, double VatOnSale,
                                        string Rack, bool Consumable, bool Useforbolpatient, bool IsSubstituteNotAllowed, int DepreciationDays, int DepreciationPercentage,
                                        bool PackageRestricted, bool IsAutoIndentOnAdmission
                                       , double DrugDose, int UnitId, int FormulationId, int RouteId, int HSNCode)
        {
            return SaveItemMasterDetails(ItemId, HospId, FacilityId,
                                        RequestedFacilityId, ShelfLifeYears, ShelfLifeMonths, ShelfLifeDays, RecommendedBy,
                                        IsFractionalIssue, IsProfile, IsVatInclude, Specification, ItemImage, ItemImageName,
                                        xmlItemWithItemUnitIds, xmlItemChargeDetails,
                                        xmlItemFieldDetails, Active, EncodedBy, PanelpriceRequired, Reusable, VatOnSale,
                                        Rack, Consumable, Useforbolpatient, IsSubstituteNotAllowed, DepreciationDays, DepreciationPercentage,
                                        PackageRestricted, IsAutoIndentOnAdmission
                                      , DrugDose, UnitId, FormulationId, RouteId, HSNCode, 0, false);
        }
        public string SaveItemMasterDetails(int ItemId, int HospId, int FacilityId,
                                        int RequestedFacilityId, int ShelfLifeYears, int ShelfLifeMonths, int ShelfLifeDays, int RecommendedBy,
                                        int IsFractionalIssue, int IsProfile, int IsVatInclude, string Specification, byte[] ItemImage, string ItemImageName,
                                        string xmlItemWithItemUnitIds, string xmlItemChargeDetails,
                                        string xmlItemFieldDetails, int Active, int EncodedBy, bool PanelpriceRequired, bool Reusable, double VatOnSale,
                                        string Rack, bool Consumable, bool Useforbolpatient, bool IsSubstituteNotAllowed, int DepreciationDays, int DepreciationPercentage,
                                        bool PackageRestricted, bool IsAutoIndentOnAdmission
                                       , double DrugDose, int UnitId, int FormulationId, int RouteId, int HSNCode, double MinimumQty, bool IsCapital)
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
            HshIn.Add("@bitPackageRestricted", PackageRestricted);
            HshIn.Add("@IsAutoIndentOnAdmission", IsAutoIndentOnAdmission);

            HshIn.Add("@decDrugDose", DrugDose);
            HshIn.Add("@intUnitId", UnitId);
            HshIn.Add("@intFormulationId", FormulationId);
            HshIn.Add("@intRouteId", RouteId);
            HshIn.Add("@IntHSNId", HSNCode);

            HshIn.Add("@MinimumQty", MinimumQty);
            HshIn.Add("@IsCapital", IsCapital);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
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
        public int getFacilityStateId(int HospId, int FacilityId)
        {
            int val = 0;
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT isnull(StateId,0) as Value FROM FacilityMaster WITH (NOLOCK) " +
                            " WHERE FacilityId= @intFacilityId AND HospitalLocationId = @intHospId AND Active = 1 ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = Convert.ToInt32(ds.Tables[0].Rows[0]["Value"]);
                }
                return val;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            finally { HshIn = null; objDl = null; }

        }

        public string getItemHsnCode(int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intItemId", ItemId);


                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspGetItemHSnCode", HshIn);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return Convert.ToString(ds.Tables[0].Rows[0]["HSNCode"]);
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
            finally
            {
                objDl = null;
                ds.Dispose();
            }
        }

    }

}
