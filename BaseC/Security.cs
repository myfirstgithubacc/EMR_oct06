using System;
using System.Web;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace BaseC
{
    public class Security
    {
        private string _ConString = string.Empty;
        Hashtable HshIn;
        Hashtable hstOutput;

        public Security(string Constring)
        {
            _ConString = Constring;
        }

        public DataSet GetGroupName(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetGroupName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetFacilityName(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(System.Data.CommandType.Text, "Select FacilityId,Name From FacilityMaster WITH (NOLOCK) Where Active=1 AND HospitalLocationId=@HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFacilityName(string SearchType, int SearchId, string ForInterface)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@SearchType", SearchType);
                HshIn.Add("@SearchId", SearchId);
                HshIn.Add("@ForInterface", ForInterface);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "GetFacilityName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEntrySite(int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(System.Data.CommandType.Text, "Select ESId,ESName From EntrySiteMaster WITH (NOLOCK) Where Active=1 and HospitalLocationId=@HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEntrySiteFacilitywise(int HospitalLocationId, int Facilityid)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                HshIn.Add("FacilityId", Facilityid);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "USPEntrySiteFacilitywise ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetGroupName(int HospitalLocationId, string GroupName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); 
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@chvGroupName", GroupName);
            return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetGroupName", HshIn);

        }
        //Not in Use
        //public DataSet GetUserName(int HospitalLocationId)
        //{
        //    DataSet objTmpDs;
        //    EncryptDecrypt en = new EncryptDecrypt();
        //    DataRow objDr;
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
        //    HshIn = new Hashtable();
        //    HshIn.Add("inyHospitalLocationId", HospitalLocationId);
        //    DataSet objDs = objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetUserNames", HshIn);
        //    objTmpDs = objDs.Clone();
        //    foreach (DataRow item in objDs.Tables[0].Rows)
        //    {
        //        objDr = objTmpDs.Tables[0].NewRow();
        //        objDr["UserName"] = en.Decrypt(item["UserName"].ToString(), en.getKey(_ConString), true);
        //        objDr["ID"] = item["ID"].ToString();
        //        objTmpDs.Tables[0].Rows.Add(objDr);
        //    }
        //    return objTmpDs;
        //}

        public DataSet GetPageNames(int ModuleId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("ModuleId", ModuleId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetModulePage", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPageRights(int GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("intGroupId", GroupId);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetPageRights", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool GetModuleRights(int GroupId, int ModuleId)
        {
            bool IsAuthorised = false;
            DataSet objDs = new DataSet();
            try
            {
                objDs = GetGroupModules(GroupId);
                DataView dv = new DataView(objDs.Tables[0]);
                dv.RowFilter = "ModuleId =" + ModuleId;
                if (dv.Count > 0)
                {
                    return IsAuthorised = true;
                }
                else
                    return IsAuthorised = false;
            }
            catch (Exception ex) { throw ex; }
            finally { objDs.Dispose(); }


        }

        /// <summary>
        /// Created by rafat anwer on 8/12/2010, purpose - to get access rights of emr.
        /// Checks if Patient details has been locked in registration other details.
        /// </summary>
        /// <param name="RegistrationId"></param>
        /// <returns> If True then locked else Unlocked</returns>
        public bool GetEMRIsLocked(int RegistrationId)
        {
            DataSet objDs = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegistrationId);
                string strSQL = "select LockRecordPwd from RegistrationOtherDetail WITH (NOLOCK) WHERE RegistrationId=@intRegistrationId";
                objDs = objDl.FillDataSet(System.Data.CommandType.Text, strSQL, HshIn);
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToString(objDs.Tables[0].Rows[0]["LockRecordPwd"]).Trim() != string.Empty)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; objDs.Dispose(); }

        }
        /// <summary>
        /// Created by rafat anwer on 8/12/2010, purpose - to match password for access rights of emr.
        /// </summary>
        /// <param name="RegistrationId"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool ValidateEMRLockRecordPassword(int RegistrationId, string Password)
        {
            string strpassword = string.Empty;

            EncryptDecrypt en = new EncryptDecrypt();
            string sPrivateKey = string.Empty;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", RegistrationId);
                SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT PrivateKey FROM RegistrationOtherDetail WITH (NOLOCK) WHERE RegistrationId = @intRegistrationId", HshIn);

                if (objDr.Read())
                {
                    sPrivateKey = Convert.ToString(objDr["PrivateKey"]).Trim();
                }
                objDr.Close();
                HshIn.Add("@chvPassword", en.Encrypt(Password, en.getKey(_ConString), true, sPrivateKey));
                SqlDataReader objDr2 = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT LockRecordPwd FROM RegistrationOtherDetail WITH (NOLOCK) WHERE RegistrationId = @intRegistrationId and LockRecordPwd = @chvPassword ", HshIn);

                if (objDr2.Read())
                {
                    if (objDr2.HasRows)
                    {
                        if (Convert.ToString(objDr2["LockRecordPwd"]) != string.Empty)
                        {
                            strpassword = Convert.ToString(objDr2["LockRecordPwd"]);
                            objDr2.Close();
                            if (en.Decrypt(strpassword, en.getKey(_ConString), true) == Password + sPrivateKey)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                {
                    objDr2.Close();
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetGroupModules(int GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intGroupId", GroupId);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetUserModule", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetModuleNames()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetModuleNames");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string CreateUserGroup(int HospitalLocationId, string GroupName, Boolean Admin, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("inyHospitalLocationID", HospitalLocationId);
                HshIn.Add("chvGroupName", GroupName);
                HshIn.Add("bitAdmin", Admin);
                HshIn.Add("intEncodedBy", EncodedBy);
                hstOutput = new Hashtable();
                hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
                int i = (int)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "UspCreateSecGroup", HshIn, hstOutput);
                return hstOutput["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public void AuditCommonAccess(int iHospID, int iFacilityID, int iRegId, int iEncountId, int iPageId, int iTemplateId, int iEncodedBy, int iEmpId, string iAuditStatus, string chvIPAddres)
        {
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@intLoginFacilityId", iFacilityID);
            HshIn.Add("@intRegistrationID", iRegId);
            HshIn.Add("@intEncounterId", iEncountId);
            HshIn.Add("@intPageId", iPageId);
            HshIn.Add("@intTemplateId", iTemplateId);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            HshIn.Add("@intEmployeeId", iEmpId);
            HshIn.Add("@chvAuditStatus", iAuditStatus);
            HshIn.Add("@chvIPAddress", chvIPAddres);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                objDl.ExecuteNonQuery(CommandType.Text, "Exec UspGenerateAudit @inyHospitalLocationID = @inyHospitalLocationId, @intLoginFacilityId =@intLoginFacilityId,@intEmployeeId=@intEmployeeId, @intUserID = @intEncodedBy, @intRegistrationID = @intRegistrationId, @intEncounterID = @intEncounterId,@intPageId = @intPageId,@intTemplateId = @intTemplateId, @chvAuditStatus = @chvAuditStatus,@chvIPAddress=@chvIPAddress", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public string SaveAccessRights(int GroupId, string AccessRights, int EncodedBy, int iHsID,int FacilityId)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@xmlAccessRights", AccessRights);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@inyHospitalLocationId", iHsID);
            HshIn.Add("@inyFacilityID", FacilityId);
            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                int i = (int)objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPSaveUserAccessRights", HshIn, hstOutput);

                return hstOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveDashboardAccessRights(int GroupId, string AccessRights, int EncodedBy, int hospitallocationId, int facilityId, string ProcedureName)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", hospitallocationId);
            HshIn.Add("@intFacilityid", facilityId);
            HshIn.Add("@intGroupId", GroupId);
            HshIn.Add("@xmlPermission", AccessRights);
            HshIn.Add("@intEncodedBy", EncodedBy);
            hstOutput.Add("@ErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, ProcedureName, HshIn, hstOutput);

                return hstOutput["@ErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }




        public int DeActivePage(int PageId, int GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                string str = "UPDATE SecGroupPages set Active = 0 WHERE GroupId = @GroupId AND PageId = @PageId";
                HshIn = new Hashtable();
                HshIn.Add("GroupId", GroupId);
                HshIn.Add("PageId", PageId);
                return objDl.ExecuteNonQuery(System.Data.CommandType.Text, str, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int updatePageRights(int Id, bool InsertData, bool EditData, bool CancelData, bool DisplayData, bool PrintData)
        {
            string sQ = " UPDATE SecGroupPages set InsertData = @InsertData, EditData = @EditData, CancelData = @CancelData, DisplayData = @DisplayData, PrintData = @PrintData WHERE Id = @Id";
            HshIn = new Hashtable();
            HshIn.Add("Id", Id);
            HshIn.Add("InsertData", InsertData);
            HshIn.Add("EditData", EditData);
            HshIn.Add("CancelData", CancelData);
            HshIn.Add("DisplayData", DisplayData);
            HshIn.Add("PrintData", PrintData);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                return objDl.ExecuteNonQuery(CommandType.Text, sQ, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Not in Use
        //public DataSet GetUserGroup(int EmpId)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("intEmpId", EmpId);

        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
        //    DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetUserGroups", HshIn);
        //    return objDs;
        //}
        //Not in Use
        //public DataSet GetUserFacility(int EmpId)
        //{
        //    HshIn = new Hashtable();
        //    HshIn.Add("@intEmpId", EmpId);
        //    string sqlsttr = "SELECT FacilityId FROM SecUserFacility suf INNER JOIN USERS u ON suf.UserId = u.Id WHERE u.EmpId = @intEmpId AND Active = 1";
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, sqlsttr, HshIn);
        //    return objDs;
        //}




        public bool IsAdmin(int GroupId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
            string str = "SELECT Admin FROM SecGroupMaster WITH (NOLOCK) WHERE GroupId = @GroupId";
            HshIn = new Hashtable();
            HshIn.Add("GroupId", GroupId);

            bool tr = (bool)objDl.ExecuteScalar(System.Data.CommandType.Text, str, HshIn);
            return tr;
        }

        public string UpdateGroupDetails(int GroupId, bool Admin, string GroupName, int HospitalLocationId, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
            HshIn = new Hashtable();
            hstOutput = new Hashtable();
            HshIn.Add("GroupId", GroupId);
            HshIn.Add("Admin", Admin);
            HshIn.Add("GroupName", GroupName);
            HshIn.Add("HospitalLocationId", HospitalLocationId);
            HshIn.Add("intEncodedBy", iEncodedBy);
            hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPUpdateUserGroup", HshIn, hstOutput);
            return hstOutput["@chvErrorStatus"].ToString();
        }

        public bool checkempID(string employeeId, int HospitalLocationId)
        {
            bool ret = false;
            DataSet objDs = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);


            try
            {


                HshIn = new Hashtable();
                HshIn.Add("@chvEmployeeNo", employeeId);
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);

                string sqlstr = "SELECT EmployeeNo FROM Employee WITH (NOLOCK) where EmployeeNo=@chvEmployeeNo and HospitalLocationId = @inyHospitalLocationId ";
                objDs = objDl.FillDataSet(CommandType.Text, sqlstr, HshIn);
                if (objDs.Tables[0].Rows.Count > 0)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; objDs.Dispose(); }
            return ret;
        }

        public bool CheckUserRights(int iHospitalId, int iEmployeeId, int iFacilityId, string cFalg)
        {
            bool IsAuthorised = false;
            DataSet objDs = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intHospitalId", iHospitalId);
                HshIn.Add("@intEmployeeId", iEmployeeId);
                HshIn.Add("@intFacilityid", iFacilityId);
                HshIn.Add("@chFalg", cFalg);
                objDs = objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "uspCheckUserRights", HshIn);
                if (objDs.Tables.Count > 0)
                {
                    if (Convert.ToBoolean(objDs.Tables[0].Rows[0]["Active"]))
                        IsAuthorised = true;
                }
                else
                {
                    IsAuthorised = false;
                }
                return IsAuthorised;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        /// <summary>
        /// Created by rafat anwer on 3/feb/2011, purpose - to delete physical files of patient attachements.
        /// </summary>
        /// <param name="DirectoryPath"></param>
        public void DeleteFiles(string DirectoryPath)
        {
            DirectoryInfo objDir = new DirectoryInfo(DirectoryPath);
            if (objDir.Exists == true)
            {
                FileInfo[] fi_array = objDir.GetFiles();
                foreach (FileInfo files in fi_array)
                {
                    if (files.Exists && files.Extension.ToLower() != ".zip")
                    {
                        //  File.Delete(DirectoryPath + "/" + files);
                    }
                }
            }
        }

        public bool IsCopyCaseSheetAuthorized(int UserId, int HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
            string str = "select ISNULL(IsCopyClinicalCasesheet,0) IsCopyClinicalCasesheet FROM Employee EMP WITH (NOLOCK) INNER JOIN Users U WITH (NOLOCK) On U.EmpID = EMP.ID WHERE U.ID = @UserId and EMP.HospitalLocationId = @HospitalLocationId";
            HshIn = new Hashtable();
            HshIn.Add("@UserId", UserId);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
            try
            {

                return (bool)objDl.ExecuteScalar(System.Data.CommandType.Text, str, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //public DataSet getSecurityUserPages()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
        //    DataSet objDs = objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "uspSecurityGetUserPages");
        //    return objDs;
        //}

        //public DataSet getSecurityUserPages(int GroupId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
        //    Hashtable hshInput = new Hashtable();

        //    hshInput.Add("@intGroupId", GroupId);

        //    DataSet objDs = objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "uspSecurityGetUserPages", hshInput);
        //    return objDs;
        //}

        public DataSet getSecurityUserPages(int intGroupId, string chvApplicationURL, int intHospitalLocationId, int intFacilityID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chvApplicationURL", chvApplicationURL);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intFacilityID", intFacilityID);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "uspSecurityGetUserPages", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetDefaultURL(int FacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intFacilityid", FacilityId);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "UspGetDefaultURL", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetReportsMasterSetup(int iHospitalLocationId, int iFacilityId, string sModuleName, string sPageName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@iFacilityId", iFacilityId);
                HshIn.Add("@sModuleName", sModuleName);
                HshIn.Add("@sPageName", sPageName);
                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "uspGetReportsMasterSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getSecurityUserPages(string chvApplicationURL, int intHospitalLocationId, int intFacilityID, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, _ConString);
            HshIn = new Hashtable();
            HshIn.Add("@chvApplicationURL", chvApplicationURL);
            HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
            HshIn.Add("@intFacilityID", intFacilityID);
            HshIn.Add("@intUserId", UserId);
            try
            {

                return objDl.FillDataSet(System.Data.CommandType.StoredProcedure, "uspSecurityGetUserPages", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

    }
}
