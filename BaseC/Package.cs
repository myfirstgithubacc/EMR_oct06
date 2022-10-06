using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class Package
    {
        string sConString = "";
        Hashtable HshIn;
        Hashtable houtPara;


        DAL.DAL objDl;

        public Package(string Constring)
        {
            sConString = Constring;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }

        public DataSet GetCompanyMaster(int iHospitalLocationId, string sSearchText)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);

                string strSQL = "SELECT CompanyId, UPPER(Name) AS Name FROM Company WITH (NOLOCK) WHERE Active=1 ";
                if (sSearchText != "")
                {
                    strSQL = strSQL + " and Name LIKE '%" + sSearchText + "%'";
                }
                strSQL = strSQL + " AND HospitalLocationId = @intHospitalLocationId ORDER BY Name";

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetCompanyMasterOfPackage(int iHospitalLocationId, string sSearchText, int PackageId, int FacilityId)
        {
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intPackageID", PackageId);
                HshIn.Add("@insFacilityId", FacilityId);

                string strSQL = "SELECT CompanyId, Name FROM ( SELECT DISTINCT c.CompanyId, UPPER(c.Name) AS Name FROM ServiceChargePackage scp WITH (NOLOCK) ";
                strSQL = strSQL + " INNER JOIN Company c WITH (NOLOCK) ON scp.CompanyId = c.CompanyId ";
                strSQL = strSQL + " INNER JOIN BedCategoryMaster BCM WITH (NOLOCK) ON BCM.BedCategoryId = scp.BedCategoryId AND BCM.Bedcategorytype = 'Y' ";
                strSQL = strSQL + " WHERE ISNULL(scp.ServiceId,0) = @intPackageID AND scp.Active = 1 ";

                if (!sSearchText.Trim().Equals(string.Empty))
                {
                    strSQL = strSQL + " AND c.Name LIKE '%" + sSearchText + "%' ";
                }

                strSQL = strSQL + " AND scp.FacilityId = @insFacilityId AND scp.hospitallocationid = @intHospitalLocationId )xx ORDER BY xx.Name";

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// To Get Complete Department Master
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <returns></returns>
        public DataSet GetDepartmentMainMaster(int iHospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
            string strSQL = "SELECT DISTINCT dm.DepartmentID, UPPER( dm.DepartmentName) AS DepartmentName FROM DepartmentMain dm WITH (NOLOCK) INNER JOIN DepartmentSub ds WITH (NOLOCK) ON dm.DepartmentID=ds.DepartmentID  WHERE ds.Type IN ('OPP','IPP') AND dm.Active=1 AND dm.HospitalLocationId = @intHospitalLocationId ORDER BY DepartmentName";

            try
            {
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// To Get Complete list of Sub Department with or without Department
        /// </summary>
        /// <param name="iHospitalLocationId"></param>
        /// <param name="iDepartmentId"></param>
        /// <returns></returns>
        public DataSet GetDepartmentSubMaster(int iHospitalLocationId, int iDepartmentId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                string strSQL = null;
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);

                if (iDepartmentId > 0)
                {
                    HshIn.Add("@intDepartmentId", iDepartmentId);
                    strSQL = "SELECT SubDeptId, UPPER(SubName) AS SubName FROM DepartmentSub WITH (NOLOCK) WHERE DepartmentId = @intDepartmentId AND Type IN('I','OPP','IPP') AND Active=1 AND HospitalLocationId = @intHospitalLocationId ORDER BY SubName";
                }
                else
                {
                    strSQL = "SELECT SubDeptId,  UPPER(SubName) AS SubName, DepartmentID  FROM DepartmentSub WITH (NOLOCK) WHERE Type IN ('I','OPP','IPP') AND Active=1 AND HospitalLocationId = @intHospitalLocationId ORDER BY SubName";
                }
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// To Get Package Masters
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="SubDepartmentId"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public DataSet GetPackage(int HospitalLocationId, int FacilityId, int SubDepartmentId, string Search)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intHospitalLocationId", HospitalLocationId);
                HshIn.Add("insFacilityId", HospitalLocationId);
                HshIn.Add("intSubDeptId", SubDepartmentId);
                HshIn.Add("chrSearchText", Search);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPackage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        /// <summary>
        /// Get Package Details as their days, limit, department limit etc..
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="PackageId"></param>
        /// <returns></returns>
        public DataSet GetPackageDetails(int HospitalLocationId, int PackageId, int iCompanyId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intHospitalLocationId", HospitalLocationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("intPackageId", PackageId);
                HshIn.Add("@intCompanyId", iCompanyId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPackageDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// To Get Package Departmentwise Limit
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="PackageId"></param>
        /// <returns></returns>
        public DataSet GetPackageDepartmentLimit(int HospitalLocationId, int PackageId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intHospitalLocationId", HospitalLocationId);
                HshIn.Add("intFacilityid", FacilityId);
                HshIn.Add("intPackageId", PackageId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageDepartmentLimit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// To Get Package wise Services Details
        /// </summary>
        /// <param name="HospitalLocationId"></param>
        /// <param name="PackageId"></param>
        /// <returns></returns>
        public DataSet GetPackageServiceLimit(int HospitalLocationId, int PackageId, int CompanyId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intHospitalLocationId", HospitalLocationId);
                HshIn.Add("intFacilityid", FacilityId);
                HshIn.Add("intPackageId", PackageId);
                HshIn.Add("intCompanyId", CompanyId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageServiceLimit", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        /// <summary>
        /// To Get Medicine Limit against Packages
        /// </summary>
        /// <param name="iLimitId"></param>
        /// <param name="iStoreId"></param>
        /// <param name="iPackageId"></param>
        /// <param name="iActive"></param>
        /// <returns></returns>
        public DataSet getPackageMedicineLimit(int HospitalLocationId, int FacilityId, int iLimitId, int iStoreId, int iPackageId, int iActive)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("insFacilityid", FacilityId);
                HshIn.Add("@intLimitId", iLimitId);
                HshIn.Add("@intStoreId", iStoreId);
                HshIn.Add("@intPackageId", iPackageId);
                HshIn.Add("@bitActive", iActive);

                houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageMedicineLimit", HshIn, houtPara);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public string SavePackageServiceDetails(int HospitalLocationId, int FacilityId, int PackageId, int CompanyId, string Packagetype,
                                                bool Multvisitallow, int valduration, string valdurationtype,
                                                int DaysLimit, int ICUdayLimit, bool StoreWiseLimit,
            decimal MedicineLimit, decimal MaterialLimit, bool SurgicalPackage, int TotalVisitsIncluded,
            int Encodedby)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("insFacilityid", FacilityId);
            HshIn.Add("intPackageID", PackageId);
            HshIn.Add("intCompanyId", CompanyId);
            HshIn.Add("chrPackageType", Packagetype);
            HshIn.Add("bitMultipleVisitsAllowed", Multvisitallow);
            HshIn.Add("inyValidityDuration", valduration);
            HshIn.Add("chrValidityDurationType", valdurationtype);
            HshIn.Add("inyTotalDaysLimit", DaysLimit);
            HshIn.Add("inyTotalICUDaysLimit", ICUdayLimit);
            HshIn.Add("bitStoreWiseLimit", StoreWiseLimit);
            HshIn.Add("decMedicineLimit", MedicineLimit);
            HshIn.Add("decMaterialLimit", MaterialLimit);
            HshIn.Add("bitSurgicalPackage", SurgicalPackage);
            HshIn.Add("inyTotalVisitsIncluded", TotalVisitsIncluded);
            HshIn.Add("bitActive", 1);
            HshIn.Add("intEncodedBy", Encodedby);

            houtPara.Add("chvErrorStatus", SqlDbType.VarChar);

            try
            {
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePackageServiceDetails", HshIn, houtPara);
                return houtPara["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SavePackageDepartmentLimit(int HospitalLocationId, int FacilityId, string TranType, int PackageId, int CompanyId, int DepartmentId,
                                        decimal PercentageLimit, decimal AmountLimit, bool status,
                                        int Encodedby, int iHosID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                HshIn = new Hashtable();
                houtPara = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("insFacilityid", FacilityId);
                HshIn.Add("chrTranType", TranType);
                HshIn.Add("intPackageID", PackageId);
                HshIn.Add("intCompanyId", CompanyId);
                HshIn.Add("intDepartmentId", DepartmentId);
                HshIn.Add("decPercentageLimit", PercentageLimit);
                HshIn.Add("decAmountLimit", AmountLimit);
                HshIn.Add("bitActive", status);
                HshIn.Add("intEncodedBy", Encodedby);

                houtPara.Add("chvErrorStatus", SqlDbType.VarChar);

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePackageDepartmentLimit", HshIn, houtPara);
                return houtPara["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPackageSpecialisationwise(int HospitalLocationId, int PackageId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hsOutput = new Hashtable();
                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intFacilityID", FacilityId);
                HshIn.Add("@intPackageId", PackageId);
                hsOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageVisitsLimit", HshIn, hsOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SavePackageServiceLimit(int ID, int HospitalLocationId, int FacilityId, string TranType, int PackageId, int CompanyId, int ServiceId,
                                           int UnitLimit, decimal AmountLimit, bool status, int Encodedby, int iHosID,
              int AutoInsertWithPackage, int PercentofPackageAmt

              )
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                Hashtable HshIn = new Hashtable();
                Hashtable houtPara1 = new Hashtable();
                HshIn.Add("@intId", ID);
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("insFacilityid", FacilityId);
                HshIn.Add("chrTranType", TranType);
                HshIn.Add("intPackageID", PackageId);
                HshIn.Add("intCompanyId", CompanyId);
                HshIn.Add("intServiceId", ServiceId);
                HshIn.Add("intUnitLimit", UnitLimit);
                HshIn.Add("decAmountLimit", AmountLimit);
                HshIn.Add("bitActive", status);
                HshIn.Add("intEncodedBy", Encodedby);
                houtPara1.Add("chvErrorStatus", SqlDbType.VarChar);
                HshIn.Add("AutoInsertWithPackage", AutoInsertWithPackage);
                HshIn.Add("PercentofPackageAmt", PercentofPackageAmt);
                houtPara1 = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePackageServiceLimit", HshIn, houtPara1);
                return Convert.ToString(houtPara1["chvErrorStatus"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SavePackageMedicineLimit(int HospitalLocationId, int FacilityId, int iLimitId, int iPackageId, int iStoreId,
                                       double dblMedicineLimit, int userId, int iActive, int iHosID)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("insFacilityid", FacilityId);
            HshIn.Add("@intLimitId", iLimitId);
            HshIn.Add("@intPackageId", iPackageId);
            HshIn.Add("@intStoreId", iStoreId);
            HshIn.Add("@monMedicineLimit", dblMedicineLimit);
            HshIn.Add("@bitActive", iActive);
            HshIn.Add("@intEncodedBy", userId);
            houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePackageMedicineLimit", HshIn, houtPara);

                return houtPara["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }


        public DataSet GetSurgeryList(int iHospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@inyHospId", iHospitalLocationId);
                string strSQL = "SELECT DISTINCT ios.ServiceId,ios.ServiceName FROM ItemOfService ios WITH (NOLOCK) INNER JOIN DepartmentSub ds WITH (NOLOCK) " +
                                " ON ios.SubDeptId = ds.SubDeptId WHERE ds.Type = 'S' AND ds.Active = 1 AND ds.HospitalLocationId = @inyHospId ORDER BY ios.ServiceName ";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPackagesurgerCharges(int PackageId, int CompanyId, int SurgeryId, int HospitalLocationId, int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@intPackageId", PackageId);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@intSurgeryId", SurgeryId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetPackageSurgeryCharge", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SavePackageSurgerycharge(int PackageId, int CompanyId, int SurgeryId, int DoctorFeeByPercentage, string strxml,
                                          bool status, int Encodedby, int iHosID, int FacilityId)
        {

            HshIn = new Hashtable();
            houtPara = new Hashtable();

            HshIn.Add("inyFacilityid", FacilityId);
            HshIn.Add("intPackageId", PackageId);
            HshIn.Add("intCompanyId", CompanyId);
            HshIn.Add("intSurgeryId", SurgeryId);
            HshIn.Add("intDoctorFeeByPercentage", DoctorFeeByPercentage);
            HshIn.Add("XMLBedcategory", strxml);
            HshIn.Add("bitActive", status);
            HshIn.Add("intEncodedBy", Encodedby);
            HshIn.Add("inyHospitalLocationId", iHosID);

            houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSavePackageSurgerycharge", HshIn, houtPara);

                return houtPara["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetPackageSurgeryChargeServiceList(int PackageId, int CompanyId, int HospitalLocationId, int facilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn = new Hashtable();
            HshIn.Add("@inyFacilityId", facilityId);
            HshIn.Add("@intPackageId", PackageId);
            HshIn.Add("@intCompanyId", CompanyId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPackageSurgeryChargeServiceList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getSpecialisation()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "SELECT DISTINCT sm.Id, sm.Name from SpecialisationMaster sm WITH (NOLOCK) inner join doctordetails dd WITH (NOLOCK) on specialisationid=sm.id where sm.Active=1");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SavePackageSpecialisation(int HospitalLocationId, int FacilityId, int iId, int NoOfLimit, int iPackageId,
                                    int SpecialisationId, int userId, int iActive)
        {
            HshIn = new Hashtable();
            houtPara = new Hashtable();
            HshIn.Add("intId", iId);
            HshIn.Add("intHospitalLocationId", HospitalLocationId);
            HshIn.Add("intFacilityID", FacilityId);
            HshIn.Add("intPackageId", iPackageId);
            HshIn.Add("intSpecialisationId", SpecialisationId);
            HshIn.Add("intNoOfVisits", NoOfLimit);
            HshIn.Add("bitActive", iActive);
            HshIn.Add("intEncodedBy", userId);
            houtPara.Add("chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePackageVisitsLimit", HshIn, houtPara);

                return houtPara["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPackageRegDetails(int HospId, int LoginFacilityId, int FacilityId, string RegNo,
                            DateTime? dtsDateFrom, DateTime? dtsDateTo, string PackageStatus,
                            string PatientName, string mobileNo, string PhoneNo)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                string fDate = "";
                string tDate = "";
                if (dtsDateFrom.HasValue)
                {
                    fDate = dtsDateFrom.Value.ToString("yyyy-MM-dd");
                    tDate = dtsDateTo.Value.ToString("yyyy-MM-dd");
                }
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@chvRegistrationNo", RegNo);
                HshIn.Add("@dtFromDate", fDate);
                HshIn.Add("@dtToDate", tDate);
                HshIn.Add("@chrPackageStatus", PackageStatus);
                HshIn.Add("@chvPatientName", PatientName);
                HshIn.Add("@mobileNo", mobileNo);
                HshIn.Add("@PhoneNo", PhoneNo);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientProlongPackageList", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet BindCopyBreakupcompany(int iHospitalLocationId, int PackageId, int CompanyId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intPackageId", PackageId);
                HshIn.Add("@intCompanyId", CompanyId);
                HshIn.Add("@intFacilityId", FacilityId);
                string strSQL = " SELECT DISTINCT psd.CompanyId, c.name FROM Company c WITH (NOLOCK) inner join Packageservicedetails psd WITH (NOLOCK) on c.companyid = psd.companyid where psd.packageid = @intPackageId and psd.Companyid <> @intCompanyId and psd.facilityid = @intFacilityId and psd.hospitallocationid = @intHospitalLocationId order by c.Name  ";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveCopyBreakupcompany(int iHospitalLocationId, int PackageId, int CopyFromCompanyId, int CopytoCompanyId, int FacilityId, int UserId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                houtPara = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intPackageID", PackageId);
                HshIn.Add("@intCopyFromCompanyId", CopyFromCompanyId);
                HshIn.Add("@intCopytoCompanyId", CopytoCompanyId);
                HshIn.Add("@insFacilityId", FacilityId);
                HshIn.Add("@intUserId", UserId);
                houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPCopyPackageBreakupfromOtherCompany", HshIn, houtPara);

                return houtPara["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
