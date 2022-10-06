using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Text;

namespace BaseC
{
    public class User
    {
        private string sConString = string.Empty;
        private Hashtable HshIn;

        public User(string Constring)
        {
            sConString = Constring;
        }
        //Not in Use
        //public int MaxInvalidPasswordAttempts
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}


        //Not in Use
        //public bool RequiresUniqueEmail
        //{
        //    get
        //    {
        //        return true;
        //    }

        //}
        //Not in Use
        //public int MinRequiredNonAlphanumericCharacters
        //{
        //    get
        //    {
        //        return 0;
        //    }
        //    set
        //    {
        //    }
        //}
        //Not in Use
        //public int MinRequiredPasswordLength
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}
        //Not in Use
        //public int PasswordStrengthRegularExpression
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}
        //Not in Use
        //public int EnablePasswordRetrieval
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}
        //Not in Use
        //public int ModuleName
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //    }
        //}
        //Not in Use
        //public void CreateUser(string UserName, string Password, string HintQuestion, string HintAnswer)
        //{

        //}
        //Not in Use
        //public bool DeleteUser(string UserID)
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public bool ChangePassword(Int32 UserID, string OldPassword, string NewPassword)
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public string GetPassword(string UserID)
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public void LockUser()
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public void UnlockUser()
        //{
        //    throw new System.NotImplementedException();
        //}

        public string GetUserID(string userName)
        {
            string i = string.Empty;
            HshIn = new Hashtable();
            EncryptDecrypt eN = new EncryptDecrypt();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@userName", eN.Encrypt(userName, eN.getKey(sConString), true, string.Empty));
            try
            {

                i = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT isnull(Convert(varchar(10),ID),'0') as ID From Users WITH (NOLOCK) where userName = @userName", HshIn);
                return i;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string checkUserAvailabilityOnEdit(string userName, int EmpId)
        {
            string i = string.Empty;
            HshIn = new Hashtable();
            EncryptDecrypt eN = new EncryptDecrypt();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@userName", eN.Encrypt(userName, eN.getKey(sConString), true, string.Empty));
            HshIn.Add("@EmpId", EmpId);
            try
            {

                i = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT isnull(Convert(varchar(10),u.ID),'0') as ID From Users u WITH (NOLOCK) inner join employee e WITH (NOLOCK) on u.empid = e.id where userName=@userName and u.EmpId <> @EmpId and e.active = 1 ", HshIn);
                return i;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string GetEmpID(string EmpId, string HospitalLocationID)
        {
            string i = string.Empty;
            HshIn = new Hashtable();
            EncryptDecrypt eN = new EncryptDecrypt();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@EmpId", EmpId);// eN.Encrypt(userName, eN.getKey(sConString), true));
            HshIn.Add("@HospitalLocationId", HospitalLocationID);
            try
            {

                i = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT isnull(Convert(varchar(10),ID),'0') as ID From Users WITH (NOLOCK) where EmpId=@EmpId AND HospitalLocationId=@HospitalLocationId", HshIn);
                if (i == null)
                {
                    return i = "0";
                }
                else
                {
                    return i;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        //Not in Use
        //public SqlDataReader GetUserDetails(Int16 EmpId, Int16 HospitalLocationID)
        //{
        //    HshIn = new Hashtable();
        //    EncryptDecrypt eN = new EncryptDecrypt();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    HshIn.Add("@EmpId", EmpId);
        //    HshIn.Add("@HospitalLocationId", HospitalLocationID);

        //    SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT us.ID, us.UserName, isnull(us.HintQuestion,'') as HintQuestion, isnull(us.HintAnswer,'') as HintAnswer From Users us where us.empid=@EmpId AND HospitalLocationId=@HospitalLocationId", HshIn);
        //    return objDr;
        //}

        public DataSet ValidateUserName(string userName, string pwd)
        {
            DataSet ds = new DataSet();
            EncryptDecrypt en = new EncryptDecrypt();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("UserName", en.Encrypt(userName, en.getKey(sConString), true, string.Empty));
            SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u WITH (NOLOCK) INNER JOIN UserPrivateKey up WITH (NOLOCK) ON u.ID = up.UserId WHERE UserName = @UserName", HshIn);
            string sUserName = string.Empty, sPrivateKey = string.Empty;
            if (objDr.Read())
            {
                sUserName = objDr["UserName"].ToString();
                sPrivateKey = objDr["PrivateKey"].ToString().Trim();
            }
            objDr.Close();
            if (string.IsNullOrEmpty(sPrivateKey))
            {
                return null;
            }
            byte[] bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);

            HshIn = new Hashtable();

            HshIn.Add("@chvUserName", en.Encrypt(userName, en.getKey(sConString), true, string.Empty));
            HshIn.Add("@chvPassword", en.Encrypt(pwd, en.getKey(sConString), true, sPrivateKey));
            //string Query = "SELECT  Ur.UserName, Hs.Name AS HospitalName, Hs.Id AS HospitalLocationId,Ur.Id UserId"
            //    + " FROM  Users AS Ur INNER JOIN"
            //    + " HospitalLocation AS Hs ON Ur.HospitalLocationID = Hs.Id"
            //    + " WHERE     (Ur.UserName = @UserName)"
            //+ " SELECT GrM.GroupId, GrM.GroupName"
            //+ " FROM SecUserGroups AS UsG INNER JOIN Users AS Us ON UsG.UserId = Us.ID INNER JOIN SecGroupMaster AS GrM ON UsG.GroupId = GrM.GroupId"
            //+ " WHERE (Us.UserName = @UserName)";

            try
            {

                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspValidateUser", HshIn);
                return ds;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool ValidateUser(string UserName, string Password)
        {
            return ValidateUser(UserName, Password, "1");
        }

        public bool ValidateUser(string UserName, string Password, string HospitalLocationID)
        {

            EncryptDecrypt en = new EncryptDecrypt();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("UserName", en.Encrypt(UserName, en.getKey(sConString), true, string.Empty));
            SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u WITH (NOLOCK) INNER JOIN UserPrivateKey up WITH (NOLOCK) ON u.ID = up.UserId WHERE UserName = @UserName", HshIn);
            string sUserName = string.Empty, sPrivateKey = string.Empty;
            if (objDr.Read())
            {
                sUserName = objDr["UserName"].ToString();
                sPrivateKey = objDr["PrivateKey"].ToString();
            }
            byte[] bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);
            objDr.Close();

            HshIn = new Hashtable();
            HshIn.Add("@chvUserName", en.Encrypt(UserName, en.getKey(sConString), true, string.Empty));
            HshIn.Add("@chvPassword", en.Encrypt(Password, en.getKey(sConString), true, sPrivateKey));

            try
            {
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "UspValidateUser", HshIn);
                string userName = ds.Tables[0].Rows[0]["UserName"].ToString();
                if (!string.IsNullOrEmpty(userName))
                {
                    if (en.Decrypt(userName, en.getKey(sConString), true) == UserName)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public string GetUserName(int iUserID)
        {
            string sUName = string.Empty;
            BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@intUID", iUserID);

                string qry = " SELECT UserName, PrivateKey FROM Users u WITH (NOLOCK) INNER JOIN UserPrivateKey up WITH (NOLOCK) ON u.ID = up.UserId  ";
                qry += " WHERE u.ID=@intUID ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sUName = en.Decrypt(Convert.ToString(ds.Tables[0].Rows[0]["UserName"]), en.getKey(sConString), true);
                }

                return sUName;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        //Not in Use
        //public void UpdateFailureCount()
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public bool ChangePasswordQuestionAndAnswer(int UserID, string nQuestion, string nAnswer)
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public void PasswordCriteria()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void LogOffUser()
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public void WriteToEventLog()
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void SetUserRights()
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public void GetUserRights()
        //{
        //    throw new System.NotImplementedException();
        //}
        //Not in Use
        //public void UpdateUserRights()
        //{
        //    throw new System.NotImplementedException();
        //}
        public int getEmployeeId(int UserId)
        {
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@UserId", UserId);

                string qry = "SELECT US.Id AS UserId, US.EmpID " +
                            " FROM Users US WITH (NOLOCK) INNER JOIN Employee EMP WITH (NOLOCK) ON EMP.ID = US.EmpID " +
                            " WHERE US.ID = @UserId ";

                int empId = 0;
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    empId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpID"]);
                }

                return empId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public string getEmployeeType(int iEmployeeId)
        {
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intEmployeeId", iEmployeeId);
                string qry = "SELECT et.EmployeeType FROM Employee em WITH (NOLOCK) INNER JOIN  EmployeeType et WITH (NOLOCK) ON em.EmployeeType = et.Id WHERE em.ID = @intEmployeeId ";

                string strEmployeeType = string.Empty;
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    strEmployeeType = Convert.ToString(ds.Tables[0].Rows[0]["EmployeeType"]);
                }
                return strEmployeeType;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveFacilityWiseLogo(int HospitalLocationId, int FacilityId, string LogoPath, string LogoImage, string LoginPath, string Loginimage)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("intFacilityId", FacilityId);
                HshIn.Add("@chvLogoPath", LogoPath);
                HshIn.Add("@imgLogo", LogoImage);
                HshIn.Add("@chvLoginImagePath", LoginPath);
                HshIn.Add("@imgLoginImage", LogoImage);
                int i = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPSaveFacilityWiseLogo", HshIn);
                return i.ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable getEmployeeType()
        {
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string qry = "SELECT ID, Description, EmployeeType FROM EmployeeType WITH (NOLOCK) where Active= 1 ORDER BY Description";
                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
                DataTable objEmployeetype = new DataTable();
                objEmployeetype = ds.Tables[0];
                return objEmployeetype;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
        public DataTable getUser_ALLDetails(int Empid)
        {
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@intEmployeeId", Empid);
                string qry = "Select hl.Id As HospitalLocationId, hl.Name AS HospitalName, IsNull(PasswordExpiryPeriod,0) as PasswordExpiryPeriod,  Convert(varchar, NextExpiryDate, 111) as NextExpiryDate, Convert(varchar, GETDATE(), 111) as today, PasswordExpiryNotification, DATEDIFF(DAY, getdate(), NextExpiryDate) as dateDifference,NeverExpirePwd,ISNULL(emp.firstname,'')+ISNULL(' '+emp.middlename,'')+ISNULL(' '+emp.lastname,'') as 'EmployeeName' , emp.DepartmentId, dm.DepartmentIdendification,isnull(emp.isresource,0) isresource ,emp.EmployeeType " +
                            " From Users ur WITH (NOLOCK) Inner Join HospitalLocation hl WITH (NOLOCK) On ur.HospitalLocationID = hl.Id " +
                            " INNER JOIN Employee emp WITH (NOLOCK) ON emp.ID=ur.EmpID " +
                            " inner join DepartmentMain dm WITH (NOLOCK) on dm.DepartmentID = emp.DepartmentId " +
                            " left JOIN EmployeeType et WITH (NOLOCK) on emp.EmployeeType = et.Id " +
                            " Where ur.Id = @intEmployeeId and emp.active = 1 ";

                ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);

                DataTable objEmployeetype = new DataTable();
                objEmployeetype = ds.Tables[0];
                return objEmployeetype;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }

        public DataSet getEmployeeTypePermission(int iEmployeeId, int iHospitalLocationId, int iFacilityId, int iGroupId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intEmployeeId", iEmployeeId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intGroupId", iGroupId);
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);

            string qry = " SELECT DISTINCT ISNULL(SGM.Admin,0) AS Admin, case when isnull(et.EmployeeType,'E') = 'D' then 'D' when isnull(et.EmployeeType,'E') = 'LD' then 'D' " +
                        " when isnull(et.EmployeeType, 'E') = 'LDIR' then 'D' when isnull(et.EmployeeType, 'E') = 'DT' then 'D' " +
                        " when isnull(et.EmployeeType, 'E') = 'PT' then 'D' when isnull(et.EmployeeType, 'E') = 'N' then 'N' " +
                        " else 'E'  end EmployeeType, Isnull(convert(integer, emp.IsUserPostEmail), 0) IsUserPostEmail,(isnull(emp.isEMRSuperUser, 0)) as isEMRSuperUser, " +
                        " ISNULL(emp.UnablePrintCaseSheet, 0) UnablePrintCaseSheet, CONVERT(VARCHAR(20), ISNULL(emp.FirstName, '')) AS EmployeeFirstName, emp.ProvidingService, " +
                        " Case When ISNULL(emp.CanDownloadPatientDocument, 0) = 1 then '1' else '0' End CanDownloadPatientDocument, emp.IsDoctor " +
                        " , isnull(dd.SpecialisationId, 0) as SpecialisationId , isnull(dd.DoctorId, 0) as DoctorId,et.Id AS EmployeeTypeId, emp.DepartmentId,ISNULL(emp.UnablePrintCaseSheet, 0) UnablePrintCaseSheet " +
                        " , Case When ISNULL(emp.CanDownloadPatientDocument, 0) = 1 then '1' else '0' End CanDownloadPatientDocument, ISNULL(fm.name,'') AS FacilityName, emp.AllowEditDoctorProgressNote,ISNULL(dd.iDefaultDoctorTemplate,0) iDefaultDoctorTemplate" +
                        " FROM Employee emp WITH (NOLOCK)" +
                        " LEFT JOIN EmployeeType ET WITH(NOLOCK) ON emp.EmployeeType = ET.Id AND ET.Active = 1 " +
                        " LEFT JOIN Users US WITH(NOLOCK) ON emp.ID = US.EmpID " +
                        " LEFT JOIN SecUserGroups SUG WITH(NOLOCK) ON US.ID = SUG.UserId AND SUG.Active = 1 " +
                        " INNER JOIN SecGroupMaster SGM WITH(NOLOCK) ON SUG.GroupId = SGM.GroupId AND SGM.Active = 1 " + ((iGroupId > 0) ? " AND SUG.GroupId = @intGroupId " : "") +
                        " INNER JOIN SecUserFacility suf WITH(NOLOCK) ON US.ID = suf.UserId AND suf.Active=1 " +
                        " INNER JOIN FacilityMaster fm WITH(NOLOCK) ON suf.FacilityId = fm.FacilityID AND fm.Active = 1 AND fm.FacilityID = @intFacilityId " +
                        " LEFT OUTER JOIN DoctorDetails dd ON emp.ID = dd.DoctorId" +
                        " WHERE emp.ID = @intEmployeeId AND emp.HospitalLocationId = @inyHospitalLocationId ";
            try
            {

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEmployeeData(int iHospID, int iEmpTypeId, string xmlEmployeeType,
                               string EmpName, int iMobileNo, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@HospitalLocationId", iHospID);
                HshIn.Add("@intEmployeeTypeId", iEmpTypeId);
                HshIn.Add("@xmlEmployeeType", xmlEmployeeType);
                HshIn.Add("@chrEmployeeName", EmpName);
                HshIn.Add("@intMobileNo", iMobileNo);
                HshIn.Add("@bitStatus", 1);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEmployeeList", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet ValidateUserNamePassword(string userName, string pwd, string sPrivateKey)
        {

            EncryptDecrypt en = new EncryptDecrypt();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@chvUserName", en.Encrypt(userName, en.getKey(sConString), true, string.Empty));
            HshIn.Add("@chvPassword", en.Encrypt(pwd, en.getKey(sConString), true, sPrivateKey));
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSecurityValidateUser", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetGroupWiseMenuTagging(int iHospId, int iFacilityId, int iGroupId,
                               int iModuleId, string sPageCode)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", iHospId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@intGroupId", iGroupId);
                HshIn.Add("@intModuleId", iModuleId);
                HshIn.Add("@chvPageCode", sPageCode);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetGroupWiseMenuTagging", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet ValidateUserName(string userName, string pwd, int userid)
        {

            EncryptDecrypt en = new EncryptDecrypt();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            if (userid == 0)
            {
                HshIn.Add("UserName", en.Encrypt(userName, en.getKey(sConString), true, string.Empty));
                SqlDataReader objDr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, "SELECT UserName, PrivateKey FROM Users u WITH (NOLOCK) INNER JOIN UserPrivateKey up WITH (NOLOCK) ON u.ID = up.UserId WHERE UserName = @UserName", HshIn);
                string sUserName = string.Empty, sPrivateKey = string.Empty;
                if (objDr.Read())
                {
                    sUserName = objDr["UserName"].ToString();
                    sPrivateKey = objDr["PrivateKey"].ToString().Trim();
                }
                objDr.Close();
                if (string.IsNullOrEmpty(sPrivateKey))
                {
                    return null;
                }
                byte[] bPrivateKey = UTF8Encoding.UTF8.GetBytes(sPrivateKey);

                HshIn = new Hashtable();

                HshIn.Add("@chvUserName", en.Encrypt(userName, en.getKey(sConString), true, string.Empty));
                HshIn.Add("@chvPassword", en.Encrypt(pwd, en.getKey(sConString), true, sPrivateKey));
                HshIn.Add("@UID", userid);
            }
            else
            {
                HshIn.Add("@chvUserName", "");
                HshIn.Add("@chvPassword", "");
                HshIn.Add("@UID", userid);
            }
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspValidateUser", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet RedirectionHandler(int userid, string IPaddress, string Redirection, string RedirectionCode, out string outresult)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@UserID", userid);
            HshIn.Add("@IPaddress", IPaddress);
            HshIn.Add("@Redirection", Redirection);
            HshIn.Add("@RedirectionCode", RedirectionCode);
            try
            {
                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "userRedirectionHandler", HshIn);

                string returns = ds.Tables[0].Rows[0]["Result"].ToString();
                outresult = returns;

                ds = ValidateUserName("", "", userid);

                DataTable dt = new DataTable();
                dt.Columns.Add("outresult");
                DataRow dr = dt.NewRow();
                dr["outresult"] = outresult;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                ds.Tables.Add(dt);
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet RedirectionHandler(int userid, string IPaddress, string Redirection, string RedirectionCode, out string outresult, int SessionTimeOut = 0)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@UserID", userid);
            HshIn.Add("@IPaddress", IPaddress);
            HshIn.Add("@Redirection", Redirection);
            HshIn.Add("@RedirectionCode", RedirectionCode);
            HshIn.Add("@SessionTimeOut", SessionTimeOut);
            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "userRedirectionHandler", HshIn);
            string returns = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                returns = Convert.ToString(ds.Tables[0].Rows[0]["Result"]);
                if (!Redirection.Equals("Logout"))
                {
                    ds = ValidateUserName("", "", userid);
                    DataTable dt = new DataTable();
                    dt.Columns.Add("outresult");
                    DataRow dr = dt.NewRow();
                    dr["outresult"] = returns;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    ds.Tables.Add(dt);
                    ds.AcceptChanges();
                }
            }
            outresult = returns;
            return ds;
        }

        public DataSet RedirectionHandler(int userid, string IPaddress, string Redirection, string RedirectionCode)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@UserID", userid);
            HshIn.Add("@IPaddress", IPaddress);
            HshIn.Add("@Redirection", Redirection);
            HshIn.Add("@RedirectionCode", RedirectionCode);

            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "userRedirectionHandler", HshIn);

            string returns = ds.Tables[0].Rows[0]["Result"].ToString();

            ds = ValidateUserName("", "", userid);

            DataTable dt = new DataTable();
            dt.Columns.Add("outresult");
            DataRow dr = dt.NewRow();
            dr["outresult"] = returns;
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            return ds;
        }

        public string GetEmpName(int iUserID)
        {
            string sUName = string.Empty;
            BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@intUID", iUserID);

            string qry = " SELECT UserName, PrivateKey,isnull(emp.Firstname,'')+isnull(emp.MiddleName,'')+isnull(emp.LastName,'') as EmpName FROM Users u WITH (NOLOCK) INNER JOIN UserPrivateKey up WITH (NOLOCK) ON u.ID = up.UserId Inner JOIN Employee emp on emp.ID=u.Empid ";
            qry += " WHERE u.ID=@intUID ";

            DataSet ds = objDl.FillDataSet(CommandType.Text, qry, HshIn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                sUName = Convert.ToString(ds.Tables[0].Rows[0]["EmpName"]);

                //sUName = en.Decrypt(Convert.ToString(ds.Tables[0].Rows[0]["UserName"]), en.getKey(sConString), true);
            }

            return sUName;
        }
        public DataSet getLoginExpiryMessage(int Facitiyid, int DayDifferent, string ShowExpWarning)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("@FacilityId", Facitiyid);
            HshIn.Add("@Datediff", DayDifferent);
            HshIn.Add("@ShowExpWarning", ShowExpWarning);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetLoginExpiryMessage", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool LockUser(string UserId)
        {
            BaseC.EncryptDecrypt en = new BaseC.EncryptDecrypt();
            //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hsInput = new Hashtable();
            hsInput.Add("UserName", en.Encrypt(UserId, en.getKey(sConString), true, ""));

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            string sQ = "update Users set IsLocked = 1 where username = @UserName";

            int i = objDl.ExecuteNonQuery(CommandType.Text, sQ, hsInput);
            if (i == 0)
                return true;
            else
                return false;
        }

        public void UserLoginAudit(int UserID, string UserHostAddress, string sStatus)
        {
            BaseC.User valUser = new BaseC.User(sConString);
            DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            //clsIVF objivf = new clsIVF(sConString);
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@UID", UserID);
            hshIn.Add("@LoginStatus", sStatus);
            hshIn.Add("@IP", UserHostAddress);

            dl.FillDataSet(CommandType.StoredProcedure, "UspUserLoginLog", hshIn);

            hshIn = null;
        }

    }
}