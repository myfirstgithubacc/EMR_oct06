using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace BaseC
{
    public partial class clsEquipment
    {
        public string SaveEMEquipmentAMCDetails(int EquipmentAMCId, int HospId, int FacilityId, string TagID, int SupplierID, string Address, string TelephoneNo, string Contact_Person,
        string Designation, string Phone, string Extension, string Email, string PurchaseNo, DateTime PurchaseDate, DateTime AMCBeginDate,
        DateTime AMCEndingDate, double AMCAmount, string Currency, string AMCType, string PaymentTerm, double FirstPayment, int NoOfPM, DateTime FirstPMDate,
        int NoOfEquipment, string Remarks, string TermsAndConditions, int Status, int UserId, string LocalServiceEngineer, string xmlEquipmentIds, string xmlPMSchedule, string xmlPaymentDetails, string TableNameToSave)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intEMEquipmentAMCID", EquipmentAMCId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvTagID", TagID);
                HshIn.Add("@intSupplierID", SupplierID);

                HshIn.Add("@chvAddress", Address);
                HshIn.Add("@chvTelephoneNo", TelephoneNo);

                HshIn.Add("@chvContact_Person", Contact_Person);
                HshIn.Add("@chvDesignation", Designation);
                HshIn.Add("@chvPhone", Phone);
                HshIn.Add("@chvExtension", Extension);
                HshIn.Add("@chvEmail", Email);
                HshIn.Add("@chvPurchaseNo", PurchaseNo);
                HshIn.Add("@dtPurchaseDate", PurchaseDate);
                HshIn.Add("@dtAMCBeginDate", AMCBeginDate);
                HshIn.Add("@dtAMCEndingDate", AMCEndingDate);
                HshIn.Add("@mnyAMCAmount", AMCAmount);

                HshIn.Add("@intCurrecny", Currency);
                HshIn.Add("@intAMCType", AMCType);

                HshIn.Add("@chvPaymentTerm", PaymentTerm);
                HshIn.Add("@mnyFirstPayment", FirstPayment);
                HshIn.Add("@intNoOfPM", NoOfPM);
                HshIn.Add("@dtFirstPMDate", FirstPMDate);
                HshIn.Add("@intNoOfEquipment", NoOfEquipment);
                HshIn.Add("@txtRemarks", Remarks);
                HshIn.Add("@txtTermsAndConditions", TermsAndConditions);
                HshIn.Add("@xmlEquipmentIds", xmlEquipmentIds);

                HshIn.Add("@xmlPMSchedule", xmlPMSchedule);
                HshIn.Add("@xmlPaymentDetails", xmlPaymentDetails);

                HshIn.Add("@chvTableNameToSave", TableNameToSave);



                HshIn.Add("@bitActive", Status);
                /*
                if (Status == 1)
                {
                    HshIn.Add("@bitActive", true);
                }
                else
                {
                    HshIn.Add("@bitActive", false);
                }
                 */
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chvLocalServiceEngineer", LocalServiceEngineer);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveEMEquipmentAMCDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getDetailsUsingTagNo(string EMEquipmentID, int FacilityId, int HospId)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intEMEquipmentID", EMEquipmentID);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@HospId", HospId);
                string qry = @"select ItemName, convert(varchar,InstallationDate,103) as InstallationDate, DepartmentName, emlm.EMLocationDescription, emslm.EMSubLocationDescription, 
                            eqmm.EquipmentDesciption, eqmm.Modelcode ,isnull(eqmm.AMCCharge,0) AMCCharge from EMEquipmentMasterMain eqmm WITH (NOLOCK) INNER JOIN DepartmentMain dm WITH (NOLOCK) on eqmm.DepartmentID=dm.DepartmentID
                            INNER JOIN EmLocationMaster emlm WITH (NOLOCK) on emlm.EMLocationID=eqmm.EMLocationID LEFT JOIN EmSubLocationMaster emslm WITH (NOLOCK)
                            on emslm.EMSubLocationID=eqmm.EMSubLocationID INNER JOIN PhrItemMaster pim WITH (NOLOCK) on eqmm.ItemID=pim.ItemId 
                            where eqmm.EMEquipmentID=@intEMEquipmentID AND eqmm.Status=1  
                            AND eqmm.FacilityId=@FacilityId AND eqmm.HospitalLocationId=@HospId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getSupplierList(int LoginFacilityId, int Id, int HospId, int Active)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intSupplierId", Id);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPhrSupplierList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getEquipmentDetails(int FacilityId, int HospId, int Status, int DepartmentID)
        {


            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intStatus", Status);
                HshIn.Add("@FacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentID);
                HshIn.Add("@HospId", HospId);
                /*
                string qry = @"select EMEquipmentID, TagID as TagNo, 
                            EquipmentDesciption, EquipmentSerialNo, picm.ItemCategoryName as 'Major Group', picd.ItemSubCategoryName as 'Minor Group' from EMEquipmentMasterMain emm
                            INNER JOIN PhrItemMaster pim on emm.ItemID=pim.ItemId
                            INNER JOIN PhrItemCategoryDetails picd on picd.ItemSubCategoryId=pim.ItemSubCategoryId
                            INNER JOIN PhrItemCategoryMaster picm on picm.ItemCategoryId=picd.ItemCategoryId
                            where emm.Status=@intStatus and emm.HospitalLocationId=@HospId and emm.FacilityId=@FacilityId";
                */
                string qry = @"select EMEquipmentID, TagID as TagNo,EquipmentDesciption, EquipmentSerialNo, picm.ItemCategoryName as 'Major Group', picd.ItemSubCategoryName as 'Minor Group' from EMEquipmentMasterMain emm WITH (NOLOCK)
                            INNER JOIN PhrItemMaster pim WITH (NOLOCK) on emm.ItemID=pim.ItemId
                            INNER JOIN PhrItemCategoryDetails picd WITH (NOLOCK) on picd.ItemSubCategoryId=pim.ItemSubCategoryId
                            INNER JOIN PhrItemCategoryMaster  picm WITH (NOLOCK) ON picd.ItemCategoryId=picm.ItemCategoryId
                            where emm.DepartmentID=@intDepartmentId and emm.Status=@intStatus and emm.FacilityId=@FacilityId and emm.HospitalLocationId=@HospId ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getCurrenciesForEquipment()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string qry = @"Select Id, Name from CurrencyMaster WITH (NOLOCK) where active=1";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getDepartments(int FacilityId, int HospId, int Active)
        {


            HshIn = new Hashtable();
            HshIn.Add("@intStatus", Active);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@HospId", HospId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                /*
                   string qry = @" select departmentid,DepartmentName  from DepartmentMain where CONVERT(varchar,DepartmentID) in
                                   (select distinct value from HospitalSetup) and Active=1";
                   */
                string qry = @"select departmentid,DepartmentName  from DepartmentMain WITH (NOLOCK) where IsMaintenanceDepartment=1
                                and Active=@intStatus and FacilityId=@FacilityId and HospitalLocationId=@HospId ";
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getDepartments(int HospId, int GroupId, int FacilityId, int Active)
        {


            HshIn = new Hashtable();
            HshIn.Add("@HospId", HospId);
            HshIn.Add("@GroupId", GroupId);
            HshIn.Add("@FacilityId", FacilityId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string qry = @" SELECT sm.StoreId, dm.DepartmentID, dm.DepartmentName    
                             FROM PhrStoreMaster sm WITH (NOLOCK)      
                             INNER JOIN SecGroupDepartments gd WITH (NOLOCK) ON sm.StoreId = gd.DepartmentId  and gd.Active=1    
                             LEFT JOIN DepartmentMain dm WITH (NOLOCK) ON sm.StoreId = dm.DepartmentID  And dm.Active=1    
                             inner join fyyearsmaster fy WITH (NOLOCK) on fy.yearid=sm.yearid      
                             WHERE gd.GroupId = @GroupId 
                             AND CONVERT(VARCHAR,GETUTCDATE(),111) BETWEEN CONVERT(VARCHAR,fy.FromDate,111) and CONVERT(VARCHAR,fy.ToDate,111)
                             AND sm.Active = 1 AND sm.facilityid=@FacilityId AND sm.HospitalLocationId = @HospId
                             ORDER BY DepartmentName";
                //And dm.IsMaintenanceDepartment=1
                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public DataSet GetEquipmentAMCAndCMCItemDetails(int HospId, int FacilityId, string ItemName, string Supplier, string Contact, string Begin, string End, string Remarks, string RecordSelectionFlag, string useTable)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvItemName", ItemName);
                HshIn.Add("@chvSupplierName", Supplier);
                HshIn.Add("@chvMobile", Contact);
                HshIn.Add("@chvBeginDate", Begin);
                HshIn.Add("@chvEndDate", End);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@chvTableToUse", useTable);
                HshIn.Add("@chvFlag", RecordSelectionFlag);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEquipmentAMCAndCMCItemDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEquipmentAMCAndCMCCompleteDetails(int HospId, int FacilityId, int EMEquipmentAMCID, string TableToUse)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEMEquipmentAMCID", EMEquipmentAMCID);
                HshIn.Add("@chvTableToUse", TableToUse);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEquipmentAMCAndCMCCompletetDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        //Returns Due or Paid after checking payment details in AMC or CMC Tables
        public string CheckPaymentStatusForAMCandCMC(int AMCID, string TableName)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intAMCorCMCID", AMCID);
                HshIn.Add("@chvTableToUse", TableName);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetCheckPaymentStatusForAMCandCMC", HshIn, HshOut);

                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        //Returns Due or Paid after checking PM Schedule details in AMC or CMC Tables
        public DataSet CheckPMScheduleStatusForAMCandCMC(int AMCID, string TableName)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intAMCorCMCID", AMCID);
                HshIn.Add("@chvTableToUse", TableName);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetCheckPMScheduleStatusForAMCandCMC", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }




    }
}
