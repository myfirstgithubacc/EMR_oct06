using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public partial class clsEquipment
    {
        private string sConString = "";
        DAL.DAL objDl;
        // DataSet ds;
        Hashtable HshIn;
        Hashtable HshOut;

        public clsEquipment(string ConString)
        {
            sConString = ConString;
        }

        public string SaveLacation(int LocationId, int FacilityId, int HospitalLocationId, int DepartmentId, string Description, int Status, int UserId)
        {
            HshIn = new Hashtable();
            HshOut = new Hashtable();

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn.Add("@intLocationId", LocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@chvLocationDescription", Description);
                if (Status == 1)
                {
                    HshIn.Add("@btStatus", true);
                }
                else
                {
                    HshIn.Add("@btStatus", false);
                }
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMSaveUpdateLocationMaster", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLocationDetails(int DepartmentID, int FacilityID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@DepartmentID", DepartmentID);
                HshIn.Add("@FacilityId", FacilityID);

                string strsql = "SELECT EMLocationID,elm.DepartmentID,dm.DepartmentName ,elm.FacilityId,fm.Name AS FacilityName,EMLocationDescription,Status,CASE WHEN Status=1 THEN 'Active' ELSE 'In-Active' END Active  " +
                              " FROM EmLocationMaster elm WITH (NOLOCK) INNER JOIN FacilityMaster fm WITH (NOLOCK) ON elm.FacilityId=fm.FacilityID " +
                              " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON elm.DepartmentID=dm.DepartmentID where elm.DepartmentID= case when @DepartmentID=0 then elm.DepartmentID else @DepartmentID end and elm.FacilityId= case when @FacilityId=0 then elm.FacilityId else @FacilityId end";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetLocationMaster(int HospitalLocationID, int facilityID, int DepartmentID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospId", HospitalLocationID);
                HshIn.Add("@intFacilityId", facilityID);
                HshIn.Add("@DepartmentID", DepartmentID);
                return objDl.FillDataSet(CommandType.Text, "select EMLocationID,EMLocationDescription from EmLocationMaster WITH (NOLOCK) WHERE Status=1 AND FacilityId=@intFacilityId AND HospitalLocationID=@inyHospId AND DepartmentID=@DepartmentID ORDER BY EMLocationDescription", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetLocationMaster(string DepartmentIds)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "select EMLocationID,EMLocationDescription from EmLocationMaster WITH (NOLOCK) where Status=1 and DepartmentID in(" + DepartmentIds + ")");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

            //HshIn = new Hashtable();
            //HshIn.Add("@chvDepartmentIds", DepartmentIds);

        }

        public string SaveSubLacation(int SubLocationId, int FacilityId, int LocationId, string Description, int Status, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {
                HshIn.Add("@intSubLocationId", SubLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intLocationId", LocationId);
                HshIn.Add("@chvLocationDescription", Description);
                if (Status == 1)
                {
                    HshIn.Add("@btStatus", true);
                }
                else
                {
                    HshIn.Add("@btStatus", false);
                }
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMSaveUpdateSubLocationMaster", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSubLocationDetails(int HospitalLocationId, int FacilityID, int LocationID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intFacility", FacilityID);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLocationId", LocationID);
                string strsql = "SELECT Esl.EMSubLocationID,Esl.EMLocationID,eml.EMLocationDescription,Esl.FacilityId,fm.Name as FacilityName,Esl.EMSubLocationDescription,Esl.Status,  " +
                              " CASE WHEN Esl.Status=1 THEN 'Active' ELSE 'In-Active' END Active, dm.DepartmentName FROM EmSubLocationMaster Esl WITH (NOLOCK) " +
                              " INNER JOIN FacilityMaster fm WITH (NOLOCK) on Esl.FacilityId=fm.FacilityID INNER JOIN EmLocationMaster eml WITH (NOLOCK) on Esl.EMLocationID=eml.EMLocationID INNER JOIN DepartmentMain dm WITH (NOLOCK) ON eml.DepartmentID=dm.DepartmentID WHERE eml.EMLocationID= case when @intLocationId=0 then eml.EMLocationID else @intLocationId end and eml.FacilityId=@intFacility and eml.HospitalLocationID=@intHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetSubLocationMaster(int LocationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intEMLocationID", LocationId);

                return objDl.FillDataSet(CommandType.Text, "select EMSubLocationID,EMSubLocationDescription from EmSubLocationMaster WITH (NOLOCK) where EMLocationID=@intEMLocationID and Status=1 ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetItemList(int HospId, int SubCategory, string Itemname, string ItemNo, int ItemId, int frmGrn)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intItemSubCategoryId", SubCategory);
                HshIn.Add("@chvItemSearch", Itemname);
                HshIn.Add("@chvintemNo", ItemNo);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intFromGrn", frmGrn);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEquipmentItemList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPrimarRespobsibility(int HospId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                return objDl.FillDataSet(CommandType.Text, "select ID, ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS Name from Employee WITH (NOLOCK) WHERE DepartmentId is not null and Active=1 and Employee.Facilityid=@intFacilityId and Employee.HospitalLocationId=@inyHospitalLocationId ORDER BY Name", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet getMaintainedUnder()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "SELECT StatusId,Status,Code,StatusColor,ColorName FROM StatusMaster WITH (NOLOCK) WHERE StatusType='EM' AND Active=1 Order By SequenceNo");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getItemCategory()
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string strsql = "SELECT icm.ItemCategoryId ,icm.ItemCategoryName  from PhrItemCategoryMaster icm WITH (NOLOCK) " +
                               " INNER JOIN StatusMaster sm WITH (NOLOCK) ON icm.CategoryTypeId=sm.StatusId where sm.Status='Assests' and sm.Code='A' and icm.Active=1 ";
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SaveEquipmentMaintMaster(int EquipmentId, int HospId, int FacilityId, int DeptId, int ItemId, string ModelCode,
            int ManufactureId, int ServiceCompany, string SlNo, string EmDescription, int EMLocationId, int SubLocationId,
            string AssestNo, string AcceptDate, string WarantyExpiry, double Laborcost, string Image, int Status,
            int Runningcost, double PerUsercost, int Noofuserperday, string Remarks, int POId, string POrNo, string PODate,
            double POCost, int Warranty, string Warrantyymd, string Lastprevdate, int GRNId, string GRNNo, int SupplierId,
            string AquisitionDate, double AquisitionCost, double NetItemValue, double MonthalyDesription, double Monthalydescriptiontax,
            double CurrentDescription, double AccumulatedDepreciation, double AccumulatedDepreciationtax, string Billno,
            int Classificationcode, int AssetDepAcknowledgedStatusid, bool IsDiscarded, string FAAcknowledgedDate,
            int FAAcknowledgedby, int Encodedby, int CalibrationPeriod, string CalibrationPeriodYearMonthDay, string FAVoucherno,
            string FAVoucherdate, int PreventiveMaintenancePeriod, string PreventiveMaintenancePeriodYearMonthDay,
            double Othercost, int FromGrnPhysicalStatusID, int MaintainedUnder, string MaintainFromDate, string MaintainToDate, int Noofprcedureperday, double AMCcharge, double AMCPercentage, double Procedurecharge, string Discardeddate, string discardresion,
            string BillDate, DateTime InstallationDate, int PrimaryResponsibility, string DiscontinueDate, int BatchId, int OAid, string TagNo,
            string DocName, string FileName, string Configuration)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {
                HshIn.Add("@intEMEquipmentID", EquipmentId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentID", DeptId);
                HshIn.Add("@intItemID", ItemId);
                HshIn.Add("@chvModelcode", ModelCode);
                HshIn.Add("@intManufactureCID", ManufactureId);
                HshIn.Add("@intServicingcompany", ServiceCompany);
                HshIn.Add("@chvEquipmentSerialNo", SlNo);
                HshIn.Add("@chvEquipmentDesciption", EmDescription);
                HshIn.Add("@intEMLocationID", EMLocationId);
                HshIn.Add("@intEMSubLocationID", SubLocationId);
                HshIn.Add("@chvAssetNo", AssestNo);
                HshIn.Add("@dtAcceptanceDate", AcceptDate);
                HshIn.Add("@dtWarrantyExpiry", WarantyExpiry);
                HshIn.Add("@mnyAccLaborCost", Laborcost);
                HshIn.Add("@imgEMEquipmentImage", Image);
                if (Status == 1)
                {
                    HshIn.Add("@btStatus", true);
                }
                else
                {
                    HshIn.Add("@btStatus", false);
                }
                HshIn.Add("@intRunningCost", Runningcost);
                HshIn.Add("@mnyPerUserCost", PerUsercost);
                HshIn.Add("@intNoofUserPerDay", Noofuserperday);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@intPurchaseOrderID", POId);
                HshIn.Add("@chvPurchaseOrderNO", POrNo);
                HshIn.Add("@dtPurchaseDate", PODate);
                HshIn.Add("@mnyPurchaseCost", POCost);
                HshIn.Add("@intWarrenty", Warranty);
                HshIn.Add("@chrWarrentyYearMonthDay", Warrantyymd);
                HshIn.Add("@dtLastPreventiveMaintenaceDate", Lastprevdate);
                HshIn.Add("@intGRNID", GRNId);
                HshIn.Add("@chvGRNNO", GRNNo);
                HshIn.Add("@intSupplierID", SupplierId);
                HshIn.Add("@dtAquisitionDate", AquisitionDate);
                HshIn.Add("@mnyAquisitionCost", AquisitionCost);
                HshIn.Add("@mnyNetItemValue", NetItemValue);
                HshIn.Add("@mnyMonthlyDepreciation", MonthalyDesription);
                HshIn.Add("@mnyMonthlyDepreciationTax", Monthalydescriptiontax);
                HshIn.Add("@mnyCurrentDepreciation", CurrentDescription);
                HshIn.Add("@mnyAccumulatedDepreciation", AccumulatedDepreciation);
                HshIn.Add("@mnyAccumulatedDepreciationTax", AccumulatedDepreciationtax);
                HshIn.Add("@chvBillNo", Billno);
                HshIn.Add("@intClassificationCode", Classificationcode);
                HshIn.Add("@intAssetDepAcknowledgedStatusID", AssetDepAcknowledgedStatusid);
                HshIn.Add("@dtFAAcknowledgedDate", FAAcknowledgedDate);
                HshIn.Add("@intFAAcknowledgedBy", FAAcknowledgedby);
                HshIn.Add("@intEncodedBy", Encodedby);
                HshIn.Add("@inyCalibrationPeriod", CalibrationPeriod);
                HshIn.Add("@chrCalibrationPeriodYearMonthDay", CalibrationPeriodYearMonthDay);
                HshIn.Add("@chvFAvoucherno", FAVoucherno);
                HshIn.Add("@dtFAvoucherdate", FAVoucherdate);
                HshIn.Add("@inyPreventiveMaintenancePeriod", PreventiveMaintenancePeriod);
                HshIn.Add("@chrPreventiveMaintenancePeriodYearMonthDay", PreventiveMaintenancePeriodYearMonthDay);
                HshIn.Add("@mnyOthercost", Othercost);
                HshIn.Add("@intFromGrnPhysicalStatusID", FromGrnPhysicalStatusID);

                HshIn.Add("@intMaintainedUnder", MaintainedUnder);
                HshIn.Add("@intMaintainedUnderFrom", MaintainFromDate);
                HshIn.Add("@intMaintainedUnderTo", MaintainToDate);

                HshIn.Add("@intNoofProcedurerPerDay", Noofprcedureperday);
                HshIn.Add("@mnyProcedureCharges", Procedurecharge);
                HshIn.Add("@mnyAMCCharges", AMCcharge);
                HshIn.Add("@mnyAMCPercentage", AMCPercentage);
                HshIn.Add("@btIsDiscarded", IsDiscarded);
                HshIn.Add("@dtDiscardedDate", Discardeddate);
                HshIn.Add("@chvDiscardedResion", discardresion);
                HshIn.Add("@dtBillDate", BillDate);
                HshIn.Add("@dtInstndate", InstallationDate);
                HshIn.Add("@intPrimarResponsibility", PrimaryResponsibility);
                HshIn.Add("@dtDiscontinueDate", DiscontinueDate);
                HshIn.Add("@intBatchId", BatchId);
                HshIn.Add("@intOAid", OAid);
                HshIn.Add("@TagNo", TagNo);
                HshIn.Add("@DocName", DocName);
                HshIn.Add("@FileName", FileName);
                HshIn.Add("@Configuration", Configuration);
                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveEMEquipmentmastermain", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getTagNo(int HospId, int FacilityId)
        {
            return getTagNo(HospId, FacilityId, 0);
        }

        public DataSet getTagNo(int HospId, int FacilityId, int DepartmentID)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentID);
                string strsql = "SELECT EMEquipmentID,TagID FROM EMEquipmentMasterMain WITH (NOLOCK) where DepartmentID = (CASE WHEN @intDepartmentId = 0 THEN DepartmentID ELSE @intDepartmentId END) and FacilityId=@intFacilityId and HospitalLocationId=@inyHospitalLocationId ";
                //string strsql = "select em.EMEquipmentID,em.TagID  from EMEquipmentMasterMain em inner join EmComplaintMain cm on em.EMEquipmentID = cm.TagID where em.HospitalLocationId=@inyHospitalLocationId and em.FacilityId=@intFacilityId and cm.DepartmentTo = (CASE WHEN @intDepartmentId = 0 THEN DepartmentID ELSE @intDepartmentId END) ";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet getTagNo(int HospId, int FacilityId, int DepartmentId, string StatusTableToUse)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@chvStatusTableToUse", StatusTableToUse);

                string strsql = @"select distinct emm.EMEquipmentID, emm.TagID from DepartmentMain dm WITH (NOLOCK)
                            inner join EMEquipmentMasterMain emm WITH (NOLOCK) on emm.DepartmentID=dm.DepartmentID
                            where emm.DepartmentId=@intDepartmentId
                            and emm.Status=1 and emm.FacilityId=@intFacilityId and emm.HospitalLocationId=@inyHospitalLocationId ";

                //            string strsql = @"select distinct emm.EMEquipmentID, emm.TagID from DepartmentMain dm 
                //   inner join PhrItemCategoryMaster picm on picm.MaintenanceDepartmentID=dm.DepartmentID
                //   inner join EMEquipmentMasterMain emm on emm.DepartmentID=dm.DepartmentID where dm.DepartmentID=@intDepartmentId
                //   and emm.Status=1 and emm.HospitalLocationId=@inyHospitalLocationId and emm.FacilityId=@intFacilityId ";
                if (StatusTableToUse.Equals("ALL"))
                {
                    strsql += " AND emm.MaintainedUnder in(select distinct StatusId from StatusMaster WITH (NOLOCK) where StatusType='EM')";
                }
                else
                {
                    strsql += " AND emm.MaintainedUnder in(select distinct StatusId from StatusMaster WITH (NOLOCK) where StatusType='EM' and Status=@chvStatusTableToUse )";
                }

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getTagNo(int HospId, int FacilityId, int DepartmentId, string StatusTableToUse, string FromForm)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@chvStatusTableToUse", StatusTableToUse);

                string strsql = @"select distinct emm.EMEquipmentID, emm.TagID from DepartmentMain dm  WITH (NOLOCK)
                            inner join EMEquipmentMasterMain emm WITH (NOLOCK) on emm.DepartmentID=dm.DepartmentID ";
                if (FromForm.Equals("C"))
                {
                    strsql += " where emm.DepartmentId=@intDepartmentId ";
                }
                if (FromForm.Equals("P"))
                {
                    strsql += " inner join EmComplaintMain cm WITH (NOLOCK) on emm.EMEquipmentID = cm.TagID ";
                    strsql += " where cm.Departmentto=@intDepartmentId ";
                }
                strsql += " and emm.Status=1 and emm.FacilityId=@intFacilityId and emm.HospitalLocationId=@inyHospitalLocationId ";

                //            string strsql = @"select distinct emm.EMEquipmentID, emm.TagID from DepartmentMain dm 
                //   inner join PhrItemCategoryMaster picm on picm.MaintenanceDepartmentID=dm.DepartmentID
                //   inner join EMEquipmentMasterMain emm on emm.DepartmentID=dm.DepartmentID where dm.DepartmentID=@intDepartmentId
                //   and emm.Status=1 and emm.HospitalLocationId=@inyHospitalLocationId and emm.FacilityId=@intFacilityId ";
                if (StatusTableToUse.Equals("ALL"))
                {
                    strsql += " AND emm.MaintainedUnder in(select distinct StatusId from StatusMaster WITH (NOLOCK) where StatusType='EM')";
                }
                else
                {
                    strsql += " AND emm.MaintainedUnder in(select distinct StatusId from StatusMaster WITH (NOLOCK) where StatusType='EM' and Status=@chvStatusTableToUse)";
                }

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet UspGetTagNoByDept(int HospId, int FacilityId, int intDepartmentId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", intDepartmentId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetTagNoByDept", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEMEquipmentMasterMain(int HospId, int FacilityId, int EqipmentId, string TagNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEMEquipmentID", EqipmentId);
                HshIn.Add("@chvTagNo", TagNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEMEquipmentMasterMain", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEquipmentItemDetails(int HospId, int FacilityId, string TagNo, string ItemNo, string ItemName, string GroupName)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvTagNo", TagNo);
                HshIn.Add("@chvItemNo", ItemNo);
                HshIn.Add("@chvItemName", ItemName);
                HshIn.Add("@chvGroupName", GroupName);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UPSGetEquipmentItemDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEquipmentItemDetails(int HospId, int FacilityId, string TagNo, string ItemNo, string ItemName, string GroupName, int DepartmentID, string StatusTableToUse)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvTagNo", TagNo);
                HshIn.Add("@chvItemNo", ItemNo);
                HshIn.Add("@chvItemName", ItemName);
                HshIn.Add("@chvGroupName", GroupName);
                HshIn.Add("@intDepartmentId", DepartmentID);
                HshIn.Add("@StatusTableToUse", StatusTableToUse);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UPSGetEquipmentItemDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetEquipmentItemDetails(int HospId, int FacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCurrencyExchange", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet PrintEquipmentDeatils(int HospId, int FacilityId, string DepartmentIds, string SubCategoryIds, string SubLocationIds)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@DepartmentIds", DepartmentIds);
                HshIn.Add("@SubCategoryIds", SubCategoryIds);
                HshIn.Add("@SubLocationIds", SubLocationIds);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPPrintEquipmentDeatils", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetSubLocationMasterForEquipmentReport(string LocationIds)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select EMSubLocationID,EMSubLocationDescription from EmSubLocationMaster WITH (NOLOCK) where EMLocationID in(" + LocationIds + ") and  Status=1 ");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SaveEP(int HospitalLocationId, int FacilityId, string TagNo, string EveryMonth, string SpecifyMonth, int EncodedBy, string xmlPMDates
        , int TagID, string Category, string SubCategory, string Equipmentmodel, int RequestedBy, int Location, int SubLocation, int DayOfMonth, DateTime StartDate, DateTime EndDate, string strMonths
        )
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvTagNo", TagNo);
                HshIn.Add("@chvEveryMonth", EveryMonth);
                HshIn.Add("@chvSpecifyMonth", SpecifyMonth);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@xmlPMDates", xmlPMDates);
                HshIn.Add("@Tag_ID", TagID);
                HshIn.Add("@Category", Category);
                HshIn.Add("@SubCategory", SubCategory);
                HshIn.Add("@Equipmentmodel", Equipmentmodel);
                HshIn.Add("@RequestedBy", RequestedBy);
                HshIn.Add("@PhyLocation", Location);
                HshIn.Add("@PhySubLocation", SubLocation);
                HshIn.Add("@DayOfM", DayOfMonth);
                HshIn.Add("@StartDate", StartDate);
                HshIn.Add("@EndDate", EndDate);
                HshIn.Add("@strMonths", strMonths);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEP", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);
                //.ToString();} catch (Exception ex) { throw ex; } finally { HshIn = null; objDl = null; }

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataSet GetEmPMscheduleMain(int HospId, int FacilityId, int ScheduleId, int Active)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intScheduleId", ScheduleId);
                HshIn.Add("@bitActive", Active);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEmPMscheduleMain", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveCondemnationofAsset(int intHospitalLocationId, int ItemId, string Remarks, int Active, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            try
            {

                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@ItemId", ItemId);
                HshIn.Add("@Remarks", Remarks);
                HshIn.Add("@Active", Active);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveCondemnationofAsset", HshIn, HshOut);

                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetCondemnationofAsset(int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@ItemId", ItemId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetCondemnationofAsset", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDocumentDownload(int ItemId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@ItemId", ItemId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDocumentName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string InactiveDoc(int Id, int LastChangedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@Id", Id);
                HshIn.Add("@LastChangedBy", LastChangedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspInactiveDoc", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);//.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDepartmentFromEqip()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDepartmentFromEqip");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetListofItem(int HospitalLocationId, int ItemId, int ItemSubCategoryId, int ItemCategoryId,
            int DepartmentId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@intItemSubCategoryId", ItemSubCategoryId);
                HshIn.Add("@intItemCategoryId", ItemCategoryId);
                HshIn.Add("@DepartmentId", DepartmentId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetListofItem", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetEquipmentItem(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEquipmentItem", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveComplaintRegister(int Id, int intHospitalLocationId, string ComplaintNo, int DepartmentId, int EquipmentId,
            int SupplierId, string NatureOfComplaint, string ModelNo, string ActionTaken, string SpareUsed, string Remarks, string Status,
            int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            try
            {

                HshIn.Add("@Id", Id);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@ComplaintNo", ComplaintNo);
                HshIn.Add("@DepartmentId", DepartmentId);
                HshIn.Add("@EquipmentId", EquipmentId);
                HshIn.Add("@SupplierId", SupplierId);
                HshIn.Add("@NatureOfComplaint", NatureOfComplaint);
                HshIn.Add("@ModelNo", ModelNo);
                HshIn.Add("@ActionTaken", ActionTaken);
                HshIn.Add("@SpareUsed", SpareUsed);
                HshIn.Add("@Remarks", Remarks);
                HshIn.Add("@Status", Status);
                HshIn.Add("@EncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveComplaintRegister", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //.ToString();
        }
        public DataSet GetComplaintDetail(int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@EncodedBy", EncodedBy);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetComplaintDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetComplaintDetailByDept(int DepartmentID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@DepartmentID", DepartmentID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetComplaintDetailByDept", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string UpdateAttendedDetail(int Attendedby, int Id, string AttendedRemarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@Id", Id);
                HshIn.Add("@Attendedby", Attendedby);
                HshIn.Add("@AttendedRemarks", AttendedRemarks);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspUpdateAttendedDetail", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //.ToString();
        }
        public string UpdateCloseDetail(int Id, int CompletedBy, string CompletedRemarks)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            try
            {

                HshIn.Add("@Id", Id);
                HshIn.Add("@CompletedBy", CompletedBy);
                HshIn.Add("@CompletedRemarks", CompletedRemarks);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspUpdateCloseDetail", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //.ToString();
        }
        public string SaveEscalationTime(int intHospitalLocationId, int FirstLevelTime, int SecondLevelTime, int ThirdLevelTime,
            bool EscalationBySMS, bool EscalationByEmail, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@FirstLevelTime", FirstLevelTime);
                HshIn.Add("@SecondLevelTime", SecondLevelTime);
                HshIn.Add("@ThirdLevelTime", ThirdLevelTime);
                HshIn.Add("@EscalationBySMS", EscalationBySMS);
                HshIn.Add("@EscalationByEmail", EscalationByEmail);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEscalationTime", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //.ToString();
        }
        public DataSet GetEscalationTime()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEscalationTime", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveEmployeeIdForExcalation(int intHospitalLocationId, string EmployeeID, int EncodedBy,
            string Level)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@EmployeeID", EmployeeID);
                HshIn.Add("@EncodedBy", EncodedBy);
                HshIn.Add("@Level", Level);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveEmployeeIdForExcalation", HshIn, HshOut);
                return Convert.ToString(HshOut["@chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
            //.ToString();
        }
        public DataSet BindEmpForEscalation(string Level)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@Level", Level);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspBindEmpForEscalation", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetEquipmentHistory(int EquipmentId, DateTime dtsDateFrom, DateTime dtsDateTo, string ModelNo)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                string fDate = dtsDateFrom.ToString("yyyy/MM/dd");
                string tDate = dtsDateTo.ToString("yyyy/MM/dd");

                HshIn.Add("@EquipmentId", EquipmentId);
                HshIn.Add("@dtsDateFrom", fDate);
                HshIn.Add("@dtsDateTo", tDate);
                HshIn.Add("@ModelNo", ModelNo);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEquipmentHistory", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
    }
}
