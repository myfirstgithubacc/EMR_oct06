using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class EMR
    {
        string sConString = "";
        bool disposed = false;
        Hashtable HshIn;
        Hashtable HshOut;
        public EMR(string Constring)
        {
            sConString = Constring;
        }
        public DataSet Getentrysite(int iFacilityID, int iHospID)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("inyHospitalLocationID", iHospID);
                HshIn.Add("intFacilityID", iFacilityID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMREntrysite", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public String Updateentrysite(int iHospID, int iFacilityID, int iEncodedBy, int iESID, String sentrysite, Byte bActive)
        {

            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("intFacilityID", iFacilityID);
            HshIn.Add("chventrysite", sentrysite);
            HshIn.Add("intentryID", iESID);
            HshIn.Add("inyActive", bActive);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveentrysite", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public String Saveentrysite(int iHospID, int iFacilityID, String sentrysite, int iEncodedBy)
        {



            HshIn = new Hashtable();
            HshOut = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospID);
            HshIn.Add("intFacilityID", iFacilityID);
            HshIn.Add("chventrysite", sentrysite);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveentrysite", HshIn, HshOut);
                return HshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool SaveDaignosisFavourite(int iDoctorID, int Icdid, int intUserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();

                HshIn.Add("DoctorID", iDoctorID);
                HshIn.Add("Icdid", Icdid);
                HshIn.Add("intUserId", intUserId);
                i = objDl.ExecuteNonQuery(System.Data.CommandType.Text, "IF NOT exists(select DoctorID FROM EMRFavouriteDiagnosis WITH (NOLOCK) WHERE doctorID = @DoctorID AND Icdid = @Icdid) insert into EMRFavouriteDiagnosis(DoctorID, Icdid,Active,EncodedBy,EncodedDate) Values(@DoctorID, @Icdid,1,@intUserId,getdate())", HshIn);

                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool SaveFavouriteMedicine(int iDoctorID, String itemid, int iUserid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();

                HshIn.Add("DoctorID", iDoctorID);
                HshIn.Add("itemid", itemid);
                HshIn.Add("iUserid", iUserid);
                i = objDl.ExecuteNonQuery(System.Data.CommandType.Text, "IF NOT exists(select DoctorID FROM EMRFavouriteMedicines WITH (NOLOCK) WHERE doctorID = @DoctorID AND Itemid = @itemid) insert into EMRFavouriteMedicines(DoctorID, itemid,Active,EncodedBy,EncodedDate) Values(@DoctorID, @itemid,1,@iUserid,getdate())", HshIn);

                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorsCommonService(int iDoctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("DoctorID", iDoctorID);
                return objDl.FillDataSet(System.Data.CommandType.Text, "select odc.ServiceID, Ios.ServiceName FROM EMRFavouriteInvestigations odc WITH (NOLOCK) INNER JOIN itemofservice ios WITH (NOLOCK) ON odc.ServiceID = ios.ServiceID WHERE doctorID = @DoctorID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorsDiagnosis(int iDoctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("DoctorID", iDoctorID);
                return objDl.FillDataSet(System.Data.CommandType.Text, "select odc.Icdid, Ios.Description FROM EMRFavouriteDiagnosis odc WITH (NOLOCK) INNER JOIN MRDICDDISEASE ios WITH (NOLOCK) ON odc.icdid = ios.icdid WHERE doctorID = @DoctorID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetDoctorsMedicines(int iDoctorID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("DoctorID", iDoctorID);
                return objDl.FillDataSet(System.Data.CommandType.Text, "select odc.Itemid,GD.GENERIC_NAME Itemname FROM EMRFavouriteMedicines odc WITH (NOLOCK) inner join CORE_GENDRUG GD WITH (NOLOCK) on odc.itemid=GD.DRUG_ID WHERE odc.doctorID =@DoctorID", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetInvestigationSetDetail(int iSetID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("SetId", iSetID);
                return objDl.FillDataSet(System.Data.CommandType.Text, "select isd.ServiceID, ios.ServiceName FROM EMRinvestigationSetDetails isd WITH (NOLOCK) INNER JOIN itemofservice ios WITH (NOLOCK) ON isd.ServiceID = ios.ServiceID WHERE SetID = @SetId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool SaveFavourite(int iDoctorID, int iServiceID, string iSetID, UInt16 Type, int iUserid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            int i = 0;
            HshIn = new Hashtable();
            try
            {
                if (Type.ToString() == "1")
                {
                    HshIn.Add("DoctorID", iDoctorID);
                    HshIn.Add("ServiceID", iServiceID);
                    HshIn.Add("iUserid", iUserid);
                    i = objDl.ExecuteNonQuery(System.Data.CommandType.Text, "IF NOT exists(select DoctorID FROM EMRFavouriteInvestigations WITH (NOLOCK) WHERE doctorID = @DoctorID AND ServiceID = @ServiceID) insert into EMRFavouriteInvestigations(DoctorID, ServiceID,Active,EncodedBy,EncodedDate) Values(@DoctorID, @ServiceID,1,@iUserid,getdate())", HshIn);
                }
                else
                {
                    HshIn.Add("SetId", iSetID);
                    HshIn.Add("ServiceID", iServiceID);
                    i = objDl.ExecuteNonQuery(System.Data.CommandType.Text, "IF NOT exists(select SetID FROM EMRInvestigationSetDetails WITH (NOLOCK) WHERE SetID = @SetId AND ServiceID = @ServiceID) insert into EMRInvestigationSetDetails(SetId, ServiceID) values(@SetId, @ServiceID)", HshIn);

                }
                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetFavouriteInvestigationSet(int iHospitalLocationId, int DoctorId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            if (DoctorId > 0)
            {
                HshIn.Add("@intDoctorId", DoctorId);
            }

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetOrderSet", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public DataTable GetFavouriteMedicationSet(int iHospitalLocationId, int DoctorId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            if (DoctorId > 0)
            {
                HshIn.Add("@intDoctorId", DoctorId);
            }

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetMedicationSet", HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public bool DeleteInvestigationSet(int iSetID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            int i = 0;
            HshIn.Add("SetID", iSetID);
            i = objDl.ExecuteNonQuery(CommandType.Text, "update EMRInvestigationSetDetails Set Active =0 where SetId = @SetID;update EMRInvestigationSetMain set Active =0 where SetId = @SetID", HshIn);
            return true;
        }
        public bool DeleteMedicationSet(int iSetID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("SetID", iSetID);
            try
            {
                objDl.ExecuteNonQuery(CommandType.Text, "update EMRDrugSetDetails Set Active =0 where SetId = @SetID;update EMRDrugSetMain set Active =0 where SetId = @SetID", HshIn);
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool CreateInvestigationSet(int EncodedBy, string sSetName, string EmployeeId, string HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                int i = 0;
                HshIn = new Hashtable();
                HshIn.Add("SetName", sSetName);
                HshIn.Add("EncodedBy", EncodedBy);
                HshIn.Add("DoctorID", EmployeeId);
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                i = objDl.ExecuteNonQuery(System.Data.CommandType.Text, "IF NOT exists(select DoctorId FROM EMRInvestigationSetMain WITH (NOLOCK) WHERE SetName = @SetName AND DoctorID = @DoctorID) insert into EMRInvestigationSetMain(SetName, DoctorId,EncodedBy,HospitalLocationId,Active) VALUES (@SetName,@DoctorID,@EncodedBy, @HospitalLocationId,1)", HshIn);
                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool DeleteServices(int iServiceID, UInt16 Type)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("ServiceID", iServiceID);
                if (Type.ToString() == "1")
                {
                    i = objDl.ExecuteNonQuery(CommandType.Text, "delete from EMRFavouriteInvestigations where ServiceID = @ServiceID", HshIn);
                }
                else
                {
                    i = objDl.ExecuteNonQuery(CommandType.Text, "delete from EMRInvestigationSetDetails where ServiceID = @ServiceID", HshIn);
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool DeleteDiagnosis(int IcdId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("Icdid", IcdId);
                i = objDl.ExecuteNonQuery(CommandType.Text, "delete from EMRFavouriteDiagnosis where Icdid = @IcdId", HshIn);
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool DeleteMedicine(String Itemid, int @doctorid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("Itemid", Itemid);
                HshIn.Add("doctorid", @doctorid);
                i = objDl.ExecuteNonQuery(CommandType.Text, "delete from EMRFavouriteMedicines where Itemid = @Itemid and Doctorid=@doctorid", HshIn);
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader GetEmployeeId(int iUserId, int iHospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("intUserId", iUserId);
                SqlDataReader objDlr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT EmpId, dbo.GetDoctorName(empid) as DoctorName From Users us WITH (NOLOCK) Inner Join Employee emp WITH (NOLOCK) On us.EmpId = emp.Id	Left Join EmployeeType empt WITH (NOLOCK) On empt.Id = emp.EmployeeType Where emp.id <> 1 And us.Id = @intUserId AND us.HospitalLocationID = @inyHospitalLocationId ", HshIn);
                return objDlr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader CheckUserDoctorOrNot(int iHospID, int iUserId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospID", iHospID);
            HshIn.Add("@UserID", iUserId);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "select dbo.CheckUserDoctorOrNot(@HospID,@UserID)", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet CheckUserDoctorOrNotDS(int iHospID, int iUserId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@HospID", iHospID);
            HshIn.Add("@UserID", iUserId);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "select dbo.CheckUserDoctorOrNot(@HospID,@UserID)", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCompanySpecialTariff(int CompanyId, int HospId, int FacilityId, int ValidId,
                                       string XMLServiceCharge, int EncodedBy, int CardId, int Doctorid, int Specialisation)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@intCompanyID", CompanyId);
            HshIn.Add("@intHospitalLocationID", HospId);
            HshIn.Add("@intCardId", CardId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intValidId", ValidId);
            HshIn.Add("@XMLServiceCharge", XMLServiceCharge);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intDoctorid", Doctorid);
            HshIn.Add("@intSpecialisation", Specialisation);

            try
            {
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateCompanySpecialTariff", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCompanyBedMatrixTariff(int CompanyId, int HospId, int FacilityId, int ValidId,
                                        int ICUCategoryId, string XMLServiceCharge, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                HshIn.Add("@intCompanyID", CompanyId);
                HshIn.Add("@intHospitalLocationID", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intValidId", ValidId);
                HshIn.Add("@intICUCategoryID", ICUCategoryId);
                HshIn.Add("@XMLServiceCharge", XMLServiceCharge);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateCompanyBedMatrixTariff", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getCompanyBedMatrixTariff(int CompanyId, int FacilityId, int HospId, int ICUCategoryId,
                                            string CPTCode, string ServiceName, string JCode, int ValidId)
        {


            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@intCompanyID", CompanyId);
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intICUCategoryID", ICUCategoryId);
                HshIn.Add("@chvCPTCode", CPTCode);
                HshIn.Add("@chvServiceName", ServiceName);
                HshIn.Add("@chvJCode", JCode);
                HshIn.Add("@intValidId", ValidId);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetCompanyBedMatrixTariff", HshIn, HshOut);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public bool IsDifferByBedCategory(int CompanyId, int DepartmentTypeId, int HospId)
        {
            bool isDiff = false;

            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@intDepartmentTypeId", DepartmentTypeId);
                HshIn.Add("@intHospId", HospId);

                string qry = " SELECT ISNULL(DifferByBedCategory, 0) AS DifferByBedCategory, ISNULL(DifferbyPercentage, 0) AS DifferbyPercentage " +
                    " FROM CompanyBedCategoryDifference WITH (NOLOCK) " +
                    " WHERE CompanyId = @intCompanyId AND DepartmentTypeId = @intDepartmentTypeId " +
                    " AND ISNULL(DifferByBedCategory, 0) = 1 AND ISNULL(DifferbyPercentage, 0) = 0 AND Active=1 AND HospitalLocationId = @intHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    isDiff = true;
                }


                return isDiff;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet getServicesDifferByBedCategory(int CompanyId, int SubDeptId, int HospId)
        {


            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intSubDeptId", SubDeptId);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@intHospId", HospId);

                string qry = "SELECT ios.ServiceId, ios.ServiceName " +
                            " FROM ItemOfService ios WITH (NOLOCK) " +
                            " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ios.SubDeptId = ds.SubDeptId " +
                            " INNER JOIN CompanyBedCategoryDifference cbcd WITH (NOLOCK) ON ds.DepartmentTypeId = cbcd.DepartmentTypeId AND ISNULL(cbcd.DifferByBedCategory, 0) = 1 AND ISNULL(cbcd.DifferbyPercentage, 0) = 0 AND cbcd.Active = 1 " +
                            " WHERE ios.SubDeptId = @intSubDeptId AND cbcd.CompanyId = @intCompanyId " +
                            " AND ISNULL(ios.DifferByBedCategory, 0) = 1 AND ISNULL(ios.DifferbyPercentage, 0) = 0 " +
                            " AND ios.Active = 1 AND ios.HospitalLocationId = @intHospId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);


            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public int getDefaultSponsorId(int RegistrationId, int HospId, int FacilityId)
        {
            int SponsorId = 0;

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {


                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT SponsorId FROM Admission WITH (NOLOCK) " +
                            " WHERE RegistrationId = @intRegistrationId " +
                            " AND DischargeDate IS NULL AND PatadType <> 'D' AND Active = 1 " +
                            " AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospId ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    qry = "SELECT SponsorId FROM Registration WITH (NOLOCK) " +
                            " WHERE Id = @intRegistrationId AND [Status] = 'A' AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospId ";

                    ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    SponsorId = Convert.ToInt32(ds.Tables[0].Rows[0]["SponsorId"]);
                }

                return SponsorId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }


        public DataSet getCompanySpecialTariffBYBedCategory(int CompanyId, int FacilityId, int HospId, int ServiceId
            , int TariffId, int NetworkId, int DoctorId, int specializationId)
        {


            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intSpecCompanyID", CompanyId);
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@inyHospitalLocationID", HospId);
                HshIn.Add("@intServiceId", ServiceId);
                HshIn.Add("@intTariffId", TariffId);
                HshIn.Add("@Doctorid", DoctorId);
                HshIn.Add("@Specialisation", specializationId);
                HshIn.Add("@intCardId", NetworkId);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetCompanySpecialTariffBYBedCategory", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getCreditApprovalDetail(int HospId, int FacilityId, int ApprovalStatus,
                            string PatientName, string EncounterNo, string RegistrationNo)
        {


            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intApprovalStatus", ApprovalStatus);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@chvEncounterNo", EncounterNo);
                HshIn.Add("@chvRegistrationNo", RegistrationNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspFillCreditPatientApprovalDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveCreditApproval(int HospId, int FacilityId, string xmlCreditApproval, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@xmlCreditApproval", xmlCreditApproval);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCreditApproval", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getSupplementaryBillServices(int HospId, int FacilityId, int DepartmentId,
                                                int EncounterId, int RegistrationId, int SupplementaryStatus)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDepartmentId", DepartmentId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intSupplementaryStatus", SupplementaryStatus);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSupplementaryBillServices", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveSupplementaryBillServices(int HospId, string xmlServiceOrderDetail, int EncodedBy, int Facilityid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", Facilityid);
                HshIn.Add("@xmlServiceOrderDetail", xmlServiceOrderDetail);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSupplementaryBillServices", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getServiceOrderDepartment(int EncounterId, int FacilityId, int HospId)
        {

            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intHospId", HospId);

                string qry = "SELECT DISTINCT dm.DepartmentID, dm.DepartmentName " +
                        " FROM ServiceOrderDetail sod WITH (NOLOCK) " +
                        " INNER JOIN ServiceOrderMain som WITH (NOLOCK) ON sod.OrderId = som.Id AND sod.Active = 1 AND som.Active = 1 " +
                        " INNER JOIN ItemOfService ios WITH (NOLOCK) ON sod.ServiceId = ios.ServiceId " +
                        " INNER JOIN DepartmentSub ds WITH (NOLOCK) ON ios.SubDeptId = ds.SubDeptId " +
                        " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON ds.DepartmentID = dm.DepartmentID " +
                        " WHERE som.EncounterId = @intEncounterId AND som.FacilityId = @intFacilityId AND som.HospitalLocationId = @intHospId " +

                        " UNION " +

                        " SELECT DISTINCT dm.DepartmentID, dm.DepartmentName " +
                        " FROM PhrIPIssueMain som WITH (NOLOCK) " +
                        " INNER JOIN DepartmentMain dm WITH (NOLOCK) ON som.LoginStoreId = dm.DepartmentID AND som.Active = 1 " +
                        " WHERE som.EncounterId = @intEncounterId AND som.FacilityId = @intFacilityId AND som.HospitalLocationId = @intHospId " +
                        " ORDER BY dm.DepartmentName ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }

        public string SaveCompanyMarkup(int Id, int CompanyId, int HospitalLocationId, int FacilityId,
                   decimal MarkupPercentOP, decimal MarkdownPercentOP, decimal MarkupPercentIP, decimal MarkdownPercentIP,
                   DateTime ValidFrom, DateTime ValidTo, int Active, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                string fDate = ValidFrom.ToString("yyyy-MM-dd 00:00");
                string tDate = ValidTo.ToString("yyyy-MM-dd 23:59");

                HshIn.Add("@intId", Id);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@decMarkupPercentOP", MarkupPercentOP);
                HshIn.Add("@decMarkdownPercentOP", MarkdownPercentOP);
                HshIn.Add("@decMarkupPercentIP", MarkupPercentIP);
                HshIn.Add("@decMarkdownPercentIP", MarkdownPercentIP);
                HshIn.Add("@dtValidFrom", fDate);
                HshIn.Add("@dtValidTo", tDate);
                HshIn.Add("@bitActive", Active);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveCompanyMarkup", HshIn, HshOut);

                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getCompanyMarkup(int Id, int CompanyId, int HospitalLocationId,
                                                int FacilityId, int Active)
        {
            DataSet ds = new DataSet();
            try
            {
                Hashtable HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();

                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@intId", Id);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@bitActive", Active);

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCompanyMarkup", HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }


        public DataSet getENMCheckLists(int HospId, int iFacilityId, string sType, int iEncounterId, int iRegistrtionId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncounterId", iEncounterId);
                HshIn.Add("@intRegistrationId", iRegistrtionId);
                HshIn.Add("@chvType", sType);
                HshIn.Add("@bitBilling", Convert.ToBoolean(0));
                HshOut.Add("@intServiceId", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMInteractiveCalculator", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getENMVisitType(int HospId, int iFacilityId, string sRegistrtionNo, int iEncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@chvRegistrationNo", sRegistrtionNo);
                HshIn.Add("@intEncounterId", iEncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetENMVisitType", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SavePatientSignData(int HospId, int FacilityId, int RegId, bool bSign,
                                        int DoctorId, string PatientSummary, int EncounterId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intFacilityID", FacilityId);
            HshIn.Add("@intDoctorID", DoctorId);
            HshIn.Add("@bitSign", bSign);
            HshIn.Add("@chvPatientSummary", PatientSummary);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@bitSignedNote", SqlDbType.Bit);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientForms", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet SavePatientSignDataDS(int HospId, int FacilityId, int RegId, bool bSign,
                                int DoctorId, string PatientSummary, int EncounterId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intRegistrationId", RegId);
            HshIn.Add("@intFacilityID", FacilityId);
            HshIn.Add("@intDoctorID", DoctorId);
            HshIn.Add("@bitSign", bSign);
            HshIn.Add("@chvPatientSummary", PatientSummary);
            HshIn.Add("@intEncounterId", EncounterId);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            HshOut.Add("@bitSignedNote", SqlDbType.Bit);
            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSavePatientForms", HshIn, HshOut);

                DataTable dt = new DataTable();
                dt.Columns.Add("chvErrorStatus");
                dt.Columns.Add("bitSignedNote");
                DataRow dr = dt.NewRow();
                dr["chvErrorStatus"] = HshOut["@chvErrorStatus"];
                dr["bitSignedNote"] = HshOut["@bitSignedNote"];
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetTemplateFieldValue(int HospId, int iSectionId, int iFieldId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intSectionId", iSectionId);
                HshIn.Add("@intFieldId", iFieldId);
                string sQuery = " SELECT TV.FieldId,TV.ValueId,TV.ValueName,TM.Id AS TemplateId,TM.TemplateName,TS.SectionId,TS.SectionName,TF.FieldName,TF.FieldType,TV.Active " +
                                " FROM EMRTemplate TM WITH (NOLOCK) INNER JOIN EMRTemplateSections TS WITH (NOLOCK) ON TM.Id=TS.TemplateID " +
                                " INNER JOIN EMRTemplateFields TF WITH (NOLOCK) ON TF.SectionId=TS.SectionId INNER JOIN EMRTemplateValues TV WITH (NOLOCK) ON TF.FieldId =TV.FieldId " +
                                " WHERE TV.FieldId=@intFieldId AND TF.SectionId=@intSectionId AND TM.HospitalLocationID=@inyHospitalLocationId ORDER BY TV.ValueName";

                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveTemplateValues(int iValueId, int iFieldId, string sValueName, bool bActive, bool bUpdate, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable HshOut = new Hashtable();
                HshIn.Add("@intValueId", iValueId);
                HshIn.Add("@intFieldId", iFieldId);
                HshIn.Add("@chvValueName", sValueName);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@bitActive", bActive);
                HshIn.Add("@bitUpdate", bUpdate);
                HshOut.Add("@chvErrorOutput", SqlDbType.VarChar);

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveFieldValues", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetEMRDoctorPatientwise(int iHospitalLocationId, int iFacilityId, int iRegistrationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationid", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iRegistrationId", iRegistrationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRDoctorPatientwise", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRPatientAlerts(int iHospitalLocationId, int iRegistrationId, string sAlertType)
        {
            HshIn = new Hashtable();
            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iRegistrationId", iRegistrationId);
            HshIn.Add("@chrAlertType", sAlertType);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRPatientAlerts", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetDoctorNextPatient(int iHospitalLocationId, int iFacilityId, int iDoctorId, int RegistrationId)
        {
            HshIn = new Hashtable();
            HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@iFacilityId", iFacilityId);
            HshIn.Add("@iDoctorId", iDoctorId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorNextPatient", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public bool EMRSaveDisplayCurrentPatient(int iHospitalLocationId, int iFacilityId, int intEncounterId,
            int intAppointmentResourceId, string chvIPAddress, string sFlag, string sDoctorName, string sDeptName, string sRegistrationNo, string TokenNo, string StatusCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intEncounterId", intEncounterId);
                HshIn.Add("@intAppointmentResourceId", intAppointmentResourceId);
                HshIn.Add("@chvIPAddress", chvIPAddress);
                HshIn.Add("@chvQMSFlag", sFlag);
                HshIn.Add("@chvDoctorName", sDoctorName);
                HshIn.Add("@chvDeptName", sDeptName);
                HshIn.Add("@chvRegistrationNo", sRegistrationNo);
                HshIn.Add("@Tokenno", TokenNo);
                HshIn.Add("@TokenStatus", StatusCode);

                i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRSaveDisplayCurrentPatient", HshIn);
                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public bool EMRAcknowledgeNextPatient(int iFacilityId, int iDoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                int i = 0;
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@iDoctorId", iDoctorId);
                i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspEMRAcknowledgeNextPatient", HshIn);
                if (i.ToString() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public String EMRSaveSeenByDoctor(int iEncounterId, int iDoctorId, int iCurrentStatus, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@iEncouterId", iEncounterId);
            HshIn.Add("@iDoctorId", iDoctorId);
            HshIn.Add("@iCurrentStatus", iCurrentStatus);
            HshIn.Add("@iEncodedBy", iEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveSeenByDoctor", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPatientIllustrationImages(int iHospitalLocationId, int iFacilityId, int iRegistrationId, int iEncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitallocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncounterId", iEncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMedicationIllustrationImage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable SaveDoctorImageTagging(bool bNewUplaod, int iHospitalLocationId, int iFacilityId, string xmlDocumentDetail, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@bitNewUpload", bNewUplaod);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlDocumentDetails", xmlDocumentDetail);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            try
            {

                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveDoctorImageTagging", HshIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDoctorImageTagging(int iHospitalLocationId, int iFacilityId, int iDoctorId, bool bNewUpload, int DepartmentId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@bitNewUpload", bNewUpload);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorWiseImageTagging", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable CreateEmergencyEncounter(int iHospitalLocationId, int iFacilityId, int iDoctorId, int iBedNo, int RegistrationId,
            int iEncodedBy, string sRemarks, DateTime EncounterDate)
        {
            HshIn = new Hashtable();
            Hashtable hsOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intRegistrationId", RegistrationId);
            HshIn.Add("@intBedId", iBedNo);
            HshIn.Add("@intDoctorId", iDoctorId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@dEncounterDate", EncounterDate);
            HshIn.Add("@cRemarks", sRemarks);
            hsOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            hsOut.Add("@intOutEncounterId", SqlDbType.Int);
            hsOut.Add("@intOutRegistrationId", SqlDbType.Int);
            hsOut.Add("@chvOutRegistrationNo", SqlDbType.VarChar);
            hsOut.Add("@chvOutEncounterNo", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                hsOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveAccidentInfo", HshIn, hsOut);
                return hsOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet BindSentenceGalleryGridView(int iTemplateFieldId, int iSectionId, int iHospitalLocationId, int iFacilityId, string Type, int LabFieldId)
        {
            HshIn = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intFieldId ", iTemplateFieldId);
                HshIn.Add("@intSectionId", iSectionId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intLabFieldId", LabFieldId);
                HshIn.Add("@chrType", Type);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGeTaggingSentences", HshIn);


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

        }
        public Hashtable SaveDeleteSentenceGallery(string sMode, int iSentenceId, string sSentence, int iSectionId, int iFieldId, int iHospitalLocationId, int iFacilityId, int iEncodedBy, int LabId)
        {
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intSentenceId ", iSentenceId);
                HshIn.Add("@intFieldId ", iFieldId);
                HshIn.Add("@intSectionId", iSectionId);
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@chvSentence", sSentence);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@chvMode", sMode);
                HshIn.Add("@intLabFieldId", LabId);
                hshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveDeleteSentenceTaggingField", HshIn, hshOut);

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
            return hshOut;
        }
        public DataSet GetDefaultTemplate(int iHospitalLocationId, int IFacilityId, int UserId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable HshOutPut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", IFacilityId);
                HshIn.Add("@intUserId", UserId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetDefaultTemplate", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationId(Int64 RegNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationNo", RegNo);
                string str = "SELECT Id FROM Registration WITH (NOLOCK) WHERE RegistrationNo=@intRegistrationNo  AND Status='A' ";
                return objDl.FillDataSet(CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable GetSpeciliazationMaster(int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorTimeSpecialisation", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataTable GetDoctorSpecialisationTagging(int HospitalLocationId, int FacilityId, int DoctorId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intDoctorId", DoctorId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDoctorSpecialisationTagging", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveUpdateDoctorSpecialisationTagging(int HospitalLocationId, int FacilityId, int DoctorId, string SpecialisationId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@xmlSpecialisationId", SpecialisationId);
            HshIn.Add("@intEncodedBy", EncodedBy);
            hshOutput.Add("@chvOutPut", SqlDbType.VarChar);
            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveUpdateDoctorSpecialisationTagging", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by Ujjwal 14 May 2015 to get all file requests from EMR to CEO Start
        public DataSet GetEMRFileRequest(int UserId, int RegistrationId, int iLoginFacilityId, int EncounterId, string fromdate, string todate)
        {
            return GetEMRFileRequest(UserId, RegistrationId, iLoginFacilityId, EncounterId, fromdate, todate, 0, "", "", "", 'A');
        }
        public DataSet GetEMRFileRequest(int UserId, int RegistrationId, int iLoginFacilityId, int EncounterId, string fromdate, string todate, Int64 RegistrationNo, string EncounterNo, string PatientName, string RequestedBy, char RequestStatus)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intFacilityId", iLoginFacilityId);
                HshIn.Add("@intUserId", UserId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
                {
                    HshIn.Add("@chvFromDate", fromdate);
                    HshIn.Add("@chvToDate", todate);
                }

                HshIn.Add("@intRegistrationNo", RegistrationNo);

                if (!string.IsNullOrEmpty(EncounterNo))
                {
                    HshIn.Add("@chvEncounterNo", EncounterNo);
                }
                if (!string.IsNullOrEmpty(PatientName))
                {
                    HshIn.Add("@chvPatientName", PatientName);
                }
                if (!string.IsNullOrEmpty(RequestedBy))
                {
                    HshIn.Add("@chvRequestedBy", RequestedBy);
                }
                HshIn.Add("@chrRequestStatus", RequestStatus);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRFileRequest", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveEMRFileRequest(int HospitalLocationId, int RequestId, int EncodedBy, int PermittedDuration, string PermittedDurationType, bool PermissionGranted)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intRequestId", RequestId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intPermittedDuration", PermittedDuration);
                HshIn.Add("@chrPermittedDurationType", PermittedDurationType);
                HshIn.Add("@PermissionGranted", PermissionGranted);
                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRFileRequestPermission", HshIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Added by Ujjwal 14 May 2015 to get all file requests from EMR to CEO End

        public string GetDefaultPageByUser(int GroupId, string checkPageUrl)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshOutPut = new Hashtable();

                HshIn.Add("@GroupId", GroupId);
                HshIn.Add("@checkPageUrl", checkPageUrl);
                HshOutPut.Add("@getPageUrl", SqlDbType.VarChar);
                HshOutPut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspGetDefaultPageByUser", HshIn, HshOutPut);
                return HshOutPut["@getPageUrl"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEMRAdmissionRequest(int HospitalLocationId, int FacilityId, int EncounterId, int RegistrationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRAdmissionRequest", HshIn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
                HshIn = null;
            }

        }
        public DataTable GetEMRExistingMedicationOrder(int HospitalLocationId, int FacilityId, int EncounterId, int RegistrationId, int ItemId,
            string OPIP)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOutput = new Hashtable();
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                HshIn.Add("@intEncounterId", EncounterId);
                HshIn.Add("@intItemId", ItemId);
                HshIn.Add("@chvOPIP", OPIP);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRExistingMedicationOrder", HshIn).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                objDl = null;
                HshIn = null;
                hshOutput = null;
            }

        }

        public Hashtable DeleteDoctorImageTagging(int iHospitalLocationId, int iFacilityId, string xmlDoctorIds, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            hsIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            hsIn.Add("@intFacilityId", iFacilityId);
            hsIn.Add("@xmlDoctorIds", xmlDoctorIds);
            hsIn.Add("@intEncodedBy", iEncodedBy);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteDoctorImageTagging", hsIn, HshOut);
                return HshOut;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetEMRTreatmentPlanTemplates(int FacilityId, int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();

            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetTreatmentPlanList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool DeleteTreatmentPlan(int iSetID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("SetID", iSetID);
            try
            {

                objDl.ExecuteNonQuery(CommandType.Text, "update EMRTreatmentPlanTemplates Set Active =0 where TemplateId = @SetID", HshIn);
                return true;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string GetUpToDateURL(string SearchText)
        {
            string URL = string.Empty;
            if (!SearchText.Equals(string.Empty))
            {
                URL = "http://www.uptodate.com/contents/search?sp=0&source=USER_PREF&search=" + SearchText + "&searchType=PLAIN_TEXT";
            }
            return URL;
        }

        public String EMRCloseQMS(int DoctorId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            hsIn.Add("@DoctorId", DoctorId);
            hsIn.Add("@intFacilityId", FacilityId);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRCloseQMS", hsIn, HshOut);
                return HshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }

}
