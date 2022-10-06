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
        public DataSet FillComplaintStatus(string FDate, string TDate, int LocationId, int FacilityId, string JobType)
        //public DataSet FillComplaintStatus(int LocationId, int FacilityId, string JobType)       
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@FDate", FDate);
                HshIn.Add("@TDate", TDate);
                HshIn.Add("@intLocationId", LocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@JStatus", JobType);
                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetComplaintStatus", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetComplaintCategoryDetails(int FacilityId, int HospitalLocationId, int DepartmentId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentID", DepartmentId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspComplaintCategoryDetail", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveComplaintCategory(int ComplaintCategoryID, int FacilityId, int DepartmentId, string ComplaintCategoryName, int IsGeneral, int Status, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intComplaintCategoryId", ComplaintCategoryID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@chvComplaintCategory", ComplaintCategoryName);
                //HshIn.Add(" @btIsGeneral", IsGeneral);
                if (IsGeneral == 1)
                {
                    HshIn.Add("@btIsGeneral", true);
                }
                else
                {
                    HshIn.Add("@btIsGeneral", false);
                }

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

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMSaveUpdateComplaintCategory", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComplaintTypeDetails(int HopsitalLocationId, int FacilityID, int DepartmentID, int ComplaintCategoryId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HopsitalLocationId);
                HshIn.Add("@intFacilityId", FacilityID);
                HshIn.Add("@intDepartmentID", DepartmentID);
                HshIn.Add("@intComplaintCategoryId", ComplaintCategoryId);
                string strsql = "uspComplaintTypeDetail";
                return objDl.FillDataSet(CommandType.StoredProcedure, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public string SaveComplaintType(int ComplaintTypeID, int FacilityId, int HospitalLocationId, int DepartmentId, int ComplaintCategoryID, string ComplaintTypeName, int Status, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();

                HshIn.Add("@intComplaintTypeId", ComplaintTypeID);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@intComplaintCategoryId", ComplaintCategoryID);
                HshIn.Add("@chvComplaintType", ComplaintTypeName);
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

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMSaveUpdateComplaintType", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComplaintCategories(int HospId, int FacilityId, int DepartmentId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentID", DepartmentId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetComplaintCategories", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetComplaintTypes(int HospId, int FacilityId, int DepartmentId, int ComplaintCategoryId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentID", DepartmentId);
                HshIn.Add("@intComplaintCategoryID", ComplaintCategoryId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetComplaintTypes", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet getEmployeeListForComplaint(int HospId, int FacilityId, int DepartmentID)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeListForComplaint", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEmployeeListForComplaintJobOrder(int HospId, int FacilityId, int DepartmentId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeListForComplaintJobOrder", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string getUserDepartmentID(int UserId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@userID", UserId);
                string qry = "select em.DepartmentId  from Users us WITH (NOLOCK) inner join employee em WITH (NOLOCK) on em.ID=us.EmpID where us.ID=@userID";
                return objDl.ExecuteScalar(CommandType.Text, qry, HshIn).ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public string SaveComplaintDetails(int ComplaintID, int HospitalLocationId, int FacilityId, string Complaint,
            DateTime ComplaintDate, int DepartmentFromID, int DepartmentToID, int ComplaintCategoryID, int ComplaintTypeID,
            int TagID, string Remarks, int ComplaintByID, string Mobile, int Active, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intComplaintID", ComplaintID);
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chrComplaint", Complaint);
                HshIn.Add("@dtComplaintDate", ComplaintDate);
                HshIn.Add("@intDepartmentFromID", DepartmentFromID);
                HshIn.Add("@intDepartmentToID", DepartmentToID);
                HshIn.Add("@intComplaintCategoryID", ComplaintCategoryID);
                HshIn.Add("@intComplaintTypeID", ComplaintTypeID);
                HshIn.Add("@intTagID", TagID);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@intComplaintByID", ComplaintByID);
                HshIn.Add("@chvMobile", Mobile);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chrErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveComplaintDetails", HshIn, HshOut);
                return HshOut["@chrErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetComplainDetails(int HospId, int FacilityId, string ComplaintDate, int ComplaintCategoryId, int ComplaintTypeId, int TagId, string Remarks, string RecordSelectionFlag, int ComplaintID)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtComplaintDate", ComplaintDate);
                HshIn.Add("@intComplaintCategoryID", ComplaintCategoryId);
                HshIn.Add("@intComplaintTypeID", ComplaintTypeId);
                HshIn.Add("@intTagID", TagId);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@chvFlag", RecordSelectionFlag);
                HshIn.Add("@intComplainedId", ComplaintID);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetComplaintDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetComplainDetails(int HospId, int FacilityId, string ComplaintFromDate, string ComplaintToDate, string ComplaintType, string SelectionFlag, int ToDepartmentID, int FromDepartmentID)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtComplaintFromDate", ComplaintFromDate);
                HshIn.Add("@dtComplaintToDate", ComplaintToDate);
                HshIn.Add("@chvComplaintType", ComplaintType);
                HshIn.Add("@chvFlag", SelectionFlag);
                HshIn.Add("@intToDepartmentID", ToDepartmentID);
                HshIn.Add("@intFromDepartmentID", FromDepartmentID);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetComplaintDetailsForJobOrder", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetAMCOrCMCScheduleDetails(int AMCorCMCNo, int SequenceNo, string TableSelectionFlag)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intAMCorCMCNo", AMCorCMCNo);
                HshIn.Add("@intSequenceNo", SequenceNo);
                HshIn.Add("@chrTableFlagToUse", TableSelectionFlag);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMGetAMCOrCMCScheduleDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEMScheduleDetailsForUpdate(int EMScheduleNo, int SequenceNo)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intEMScheduleNo", EMScheduleNo);
                HshIn.Add("@intSequenceNo", SequenceNo);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMScheduleDetailsForUpdate", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetAMCorCMCPaymentSchedule(int HospId, int FacilityId, string PaymentFromDate, string PaymentToDate, string PaymentType, string TableSelectionFlag)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtPaymentFromDate", PaymentFromDate);
                HshIn.Add("@dtPaymentToDate", PaymentToDate);
                HshIn.Add("@chrPaymentType", PaymentType);
                HshIn.Add("@chrTableFlag", TableSelectionFlag);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetAMCorCMCPaymentSchedule", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }




        //public DataSet GetEmPMscheduleMain(int HospId, int FacilityId, int ScheduleId, int Active)
        //{
        //    try
        //    {
        //        objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //       
        //        HshIn = new Hashtable();
        //        HshOut = new Hashtable();
        //        HshIn.Add("@intHospitalLocationId", HospId);
        //        HshIn.Add("@intFacilityId", FacilityId);
        //        HshIn.Add("@intScheduleId", ScheduleId);
        //        HshIn.Add("@bitActive", Active);                
        //        HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //        return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEmPMscheduleMain", HshIn, HshOut);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    
        //}


        public DataSet GetEMScheduleDetails(int HospId, int FacilityId, string PaymentFromDate, string PaymentToDate, string PaymentType)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtPaymentFromDate", PaymentFromDate);
                HshIn.Add("@dtPaymentToDate", PaymentToDate);
                HshIn.Add("@chrPaymentType", PaymentType);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEMPaymentScheduleDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }



        public string SaveJobOrderMain(int JobNo, string JobOrderNo, int HospitalLocationId, int FacilityId, DateTime JobDate, string JobOrderType,
            string TagNo, string Equipment, string ModelNo, string EquipmentPart, int ComplaintCategory, int ComplaintType,
            int ComplaintNo, DateTime ComplaintDate, int RequestBy, string ComplaintDescription, string Remarks, int AllotedTo, int UserId, int Active)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intJobNo", JobNo);
                HshIn.Add("@chvJobOrderNo", JobOrderNo);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtJobDate", JobDate);
                HshIn.Add("@chrJobOrderType", JobOrderType);
                HshIn.Add("@chvTagNo", TagNo);
                HshIn.Add("@chvEquipment", Equipment);
                HshIn.Add("@chvModelNo", ModelNo);
                HshIn.Add("@chvEquipmentPart", EquipmentPart);
                HshIn.Add("@intComplaintCategory", ComplaintCategory);
                HshIn.Add("@intComplaintType", ComplaintType);
                HshIn.Add("@intComplaintNo", ComplaintNo);
                HshIn.Add("@dtComplaintDate", ComplaintDate);
                HshIn.Add("@intRequestBy", RequestBy);
                HshIn.Add("@chvComplaintDescription", ComplaintDescription);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@intAllotedTo", AllotedTo);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@bitActive", Active);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMSaveJobOrderMain", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveJobOrderMainWithStatus(int JobNo, string JobOrderNo, int HospitalLocationId, int FacilityId, DateTime JobDate, string JobOrderType,
          string TagNo, string Equipment, string ModelNo, string EquipmentPart, int ComplaintCategory, int ComplaintType,
          int ComplaintNo, DateTime ComplaintDate, int RequestBy, string ComplaintDescription, string Remarks, int AllotedTo, int UserId, int Active, string JobStatus, string ClosingDate, double Amountcharged, string JobOrderStatusDescription)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intJobNo", JobNo);
                HshIn.Add("@chvJobOrderNo", JobOrderNo);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtJobDate", JobDate);
                HshIn.Add("@chrJobOrderType", JobOrderType);
                HshIn.Add("@chvTagNo", TagNo);
                HshIn.Add("@chvEquipment", Equipment);
                HshIn.Add("@chvModelNo", ModelNo);
                HshIn.Add("@chvEquipmentPart", EquipmentPart);
                HshIn.Add("@intComplaintCategory", ComplaintCategory);
                HshIn.Add("@intComplaintType", ComplaintType);
                HshIn.Add("@intComplaintNo", ComplaintNo);
                HshIn.Add("@dtComplaintDate", ComplaintDate);
                HshIn.Add("@intRequestBy", RequestBy);
                HshIn.Add("@chvComplaintDescription", ComplaintDescription);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@intAllotedTo", AllotedTo);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@chrJobOrderStatus", JobStatus);
                HshIn.Add("@dtClosingDate", ClosingDate);
                HshIn.Add("@Amountcharged", Amountcharged);
                HshIn.Add("@chvJobStatusDescription", JobOrderStatusDescription);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMSaveJobOrderMainWithStatus", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SaveAMCOrCMCScheduleDetails(int AMCorCMCNo, int SequenceNo, string PerformedDate,
            string EngineerName, string WorkDetail, string ScheduleStatus, int CheckedBy, int VerifiedBy, string Remarks, int UserId, string TableFlagToUse)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intAMCorCMCNo", AMCorCMCNo);
                HshIn.Add("@intSequenceNo", SequenceNo);
                HshIn.Add("@dtPerformedDate", PerformedDate);
                HshIn.Add("@chvEngineerName", EngineerName);
                HshIn.Add("@chvWorkDetails", WorkDetail);
                HshIn.Add("@chrScheduleStatus", ScheduleStatus);
                HshIn.Add("@intCheckedBy", CheckedBy);
                HshIn.Add("@intVerifiedBy", VerifiedBy);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@chrTableFlagToUse", TableFlagToUse);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMSaveAMCOrCMCScheduleDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEMScheduleDetails(int EMScheduleNo, int SequenceNo, string PerformedDate,
           string EngineerName, string WorkDetail, string ScheduleStatus, int CheckedBy, int VerifiedBy, string Remarks, int UserId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@intEMScheduleNo", EMScheduleNo);
                HshIn.Add("@intSequenceNo", SequenceNo);
                HshIn.Add("@dtPerformedDate", PerformedDate);
                HshIn.Add("@chvEngineerName", EngineerName);
                HshIn.Add("@chvWorkDetails", WorkDetail);
                HshIn.Add("@chrScheduleStatus", ScheduleStatus);
                HshIn.Add("@intCheckedBy", CheckedBy);
                HshIn.Add("@intVerifiedBy", VerifiedBy);
                HshIn.Add("@chvRemarks", Remarks);
                HshIn.Add("@intEncodedBy", UserId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMSaveEMScheduleStatusDetails", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetJobOrderDetails(int HospId, int FacilityId, string JobOrderFromDate, string JobOrderToDate, int JobOrderNo, string SelectionFlag, int DepartmentId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtJobOrderFromDate", JobOrderFromDate);
                HshIn.Add("@dtJobOrderToDate", JobOrderToDate);
                HshIn.Add("@intJobOrderNo", JobOrderNo);
                HshIn.Add("@chvFlag", SelectionFlag);
                HshIn.Add("@DepartmentId", DepartmentId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetJobOrderDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetJobOrderDetailsWithoutComplaint(int HospId, int FacilityId, string JobOrderFromDate, string JobOrderToDate, int JobOrderNo, string SelectionFlag)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@dtJobOrderFromDate", JobOrderFromDate);
                HshIn.Add("@dtJobOrderToDate", JobOrderToDate);
                HshIn.Add("@intJobOrderNo", JobOrderNo);
                HshIn.Add("@chvFlag", SelectionFlag);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetJobOrderDetailsWithoutComplaint", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }




    }
}
