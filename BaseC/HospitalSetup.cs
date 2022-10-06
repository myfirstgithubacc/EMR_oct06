using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;

namespace BaseC
{
    public class HospitalSetup
    {
        //Declaring String Variables
        Hashtable HshIn;
        string sConString = string.Empty;
        private string sAccountName = string.Empty, sNPI = string.Empty, sEIN = string.Empty, iTimeZone = string.Empty;
        private string sMainNo = string.Empty, sAddress = string.Empty, hAddress = string.Empty, sFaxNo = string.Empty, sZip = string.Empty;
        string sEmpNo = string.Empty, sFirstName = string.Empty, sUsername = string.Empty, sPassword = string.Empty, sMail = string.Empty, sMobile = string.Empty, sFacility = string.Empty, sWork = string.Empty, sMiddleName = string.Empty, sLastName = string.Empty;
        string sBillingaddress = string.Empty, sBillingaddress2 = string.Empty, sBillingphone = string.Empty, sBillingzip = string.Empty;

        DateTime sEndofyear;

        // Declaring Integer & Boolean 
        private Int32 iCountry = 0, iState = 0, iCity = 0, iEncodedBy = 0;
        private Int32 sBillingcountry = 0, sBillingstate = 0, sBillingcity = 0;
        private Int16 iHospitalLocationID = 0, FacilityId = 0, iTrialVersion = 0, iTrialDays = 0;
        Int16 iTitleId = 0, sEmpType = 0;
        Int16 iSaveAsFacility = 0;
        Int32 iEmploymentstatus, iPOS;
        int intOffSetMinutes;

        private String _XMLHospitalFlags;
        public String XMLHospitalFlags
        {
            get { return _XMLHospitalFlags; }
            set { _XMLHospitalFlags = value; }
        }

        public HospitalSetup(string conString)
        {
            sConString = conString;
        }
        public Int32 POS
        {
            get { return iPOS; }
            set { iPOS = value; }
        }

        public String TimeZone
        {
            get { return iTimeZone; }
            set { iTimeZone = value; }
        }

        public int OffSetMinutes
        {
            get { return intOffSetMinutes; }
            set { intOffSetMinutes = value; }
        }

        public Int32 EmploymentStatus
        {
            get { return iEmploymentstatus; }
            set { iEmploymentstatus = value; }
        }
        public Int32 BillingCountry
        {
            get { return sBillingcountry; }
            set { sBillingcountry = value; }
        }
        public Int32 BillingState
        {
            get { return sBillingstate; }
            set { sBillingstate = value; }
        }
        public Int32 Billingcity
        {
            get { return sBillingcity; }
            set { sBillingcity = value; }
        }
        public String Billingzip
        {
            get { return sBillingzip; }
            set { sBillingzip = value; }
        }
        public String AccountName
        {
            get { return sAccountName; }
            set { sAccountName = value; }
        }

        public Int16 SaveAsFacility
        {
            get { return iSaveAsFacility; }
            set { iSaveAsFacility = value; }
        }

        public String NPI
        {
            get { return sNPI; }
            set { sNPI = value; }
        }

        public String EIN
        {
            get { return sEIN; }
            set { sEIN = value; }
        }

        public String MainNo
        {
            get { return sMainNo; }
            set { sMainNo = value; }
        }

        public String FaxNo
        {
            get { return sFaxNo; }
            set { sFaxNo = value; }
        }

        public String Address
        {
            get { return sAddress; }
            set { sAddress = value; }
        }

        public String Address2
        {
            get { return hAddress; }
            set { hAddress = value; }
        }
        public String BillingAddress
        {
            get { return sBillingaddress; }
            set { sBillingaddress = value; }

        }
        public String BillingAddress2
        {
            get { return sBillingaddress2; }
            set { sBillingaddress2 = value; }

        }
        public String BillingPhone
        {
            get { return sBillingphone; }
            set { sBillingphone = value; }

        }
        public DateTime Endofyear
        {
            get { return sEndofyear; }
            set { sEndofyear = value; }
        }
        public String Zip
        {
            get { return sZip; }
            set { sZip = value; }
        }

        public Int32 Country
        {
            get { return iCountry; }
            set { iCountry = value; }
        }

        public Int32 State
        {
            get { return iState; }
            set { iState = value; }
        }

        public Int32 City
        {
            get { return iCity; }
            set { iCity = value; }
        }

        public Int16 TrialVersion
        {
            get { return iTrialVersion; }
            set { iTrialVersion = value; }
        }

        public Int16 TrialDays
        {
            get { return iTrialDays; }
            set { iTrialDays = value; }
        }
        public Int32 EncodedBy
        {
            get { return iEncodedBy; }
            set { iEncodedBy = value; }
        }
        public Int16 HospitalLocationID
        {
            get { return iHospitalLocationID; }
            set { iHospitalLocationID = value; }
        }
        public Int16 idFacility
        {
            get { return FacilityId; }
            set { FacilityId = value; }
        }
        public String EmpNo
        {
            get { return sEmpNo; }
            set { sEmpNo = value; }
        }

        public Int16 TitleId
        {
            get { return iTitleId; }
            set { iTitleId = value; }
        }

        public Int16 EmpType
        {
            get { return sEmpType; }
            set { sEmpType = value; }
        }

        public String FirstName
        {
            get { return sFirstName; }
            set { sFirstName = value; }
        }

        public String Username
        {
            get { return sUsername; }
            set { sUsername = value; }
        }

        public String Password
        {
            get { return sPassword; }
            set { sPassword = value; }
        }

        public String Mail
        {
            get { return sMail; }
            set { sMail = value; }
        }

        public String Work
        {
            get { return sWork; }
            set { sWork = value; }
        }

        public String Mobile
        {
            get { return sMobile; }
            set { sMobile = value; }
        }

        public String Facility
        {
            get { return sFacility; }
            set { sFacility = value; }
        }

        public String LastName
        {
            get { return sLastName; }
            set { sLastName = value; }
        }

        public String MiddleName
        {
            get { return sMiddleName; }
            set { sMiddleName = value; }
        }

        string sPrivateKey = string.Empty;

        public String PrivateKey
        {
            get { return sPrivateKey; }
            set { sPrivateKey = value; }
        }
        public string GetFlagValueHospitalSetup(int HospitalLocation, string strFlag, int Facilityid)
        {
            string strValue = string.Empty;
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@hospid", HospitalLocation);
            HshIn.Add("@intFacilityid", Facilityid);
            HshIn.Add("@flag", strFlag);
            string strSQL = "Select Value From HospitalSetup WITH (NOLOCK) Where Flag = @flag and FacilityId = @intFacilityid and HospitalLocationId = @hospid ";

            try
            {
                ds = objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strValue = Convert.ToString(ds.Tables[0].Rows[0]["Value"]);
                }
                return strValue;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public string saveHospitalSetUp()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            HshIn.Add("chvName", sAccountName);
            HshIn.Add("chvNPI", sNPI);
            HshIn.Add("chvEIN", sEIN);
            HshIn.Add("chvPhone", sMainNo);
            HshIn.Add("chvFax", sFaxNo);
            HshIn.Add("chvAddress1", sAddress);
            HshIn.Add("chvAddress2", hAddress);
            HshIn.Add("intCountry", iCountry);
            HshIn.Add("intState", iState);
            HshIn.Add("intCity", iCity);
            HshIn.Add("chvZip", sZip);
            HshIn.Add("chvPOSCode", iPOS);
            HshIn.Add("chvTimeZoneId", iTimeZone);
            HshIn.Add("intOffSetMinutes", intOffSetMinutes);
            HshIn.Add("chvBillingAddress1", sBillingaddress);
            HshIn.Add("chvBillingAddress2", sBillingaddress2);
            HshIn.Add("intBillingCountry", sBillingcountry);
            HshIn.Add("intBillingState", sBillingstate);
            HshIn.Add("intBillingCity", sBillingcity);
            HshIn.Add("chvBillingZip", sBillingzip);
            HshIn.Add("chvBillphone", sBillingphone);
            HshIn.Add("intEmploymentStatusId", EmploymentStatus);
            HshIn.Add("YearEndDate", Convert.ToDateTime(sEndofyear));
            HshIn.Add("bitTrialVersion", iTrialVersion);
            HshIn.Add("inyTrialDays", iTrialDays);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("chvEmpNo", sEmpNo);
            if (iTitleId != 0)
            {
                HshIn.Add("intTitleId", iTitleId);
            }
            HshIn.Add("intEmpType", sEmpType);
            HshIn.Add("chvFirstName", sFirstName);
            HshIn.Add("chvMiddleName", sMiddleName);
            HshIn.Add("chvLastName", sLastName);

            HshIn.Add("chvUsername", sUsername);
            HshIn.Add("chvpassword", sPassword);

            HshIn.Add("PrivateKey", sPrivateKey);

            HshIn.Add("chvMobile", sMobile);
            HshIn.Add("chvEmail", sMail);
            HshIn.Add("chvPhoneWork", sWork);

            hshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRSaveHospitalSetUp", HshIn, hshOut);
                return hshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string UpdateHospitalSetUp()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            HshIn.Add("intHospitalLocationID", iHospitalLocationID);
            HshIn.Add("chvName", sAccountName);
            HshIn.Add("chvNPI", sNPI);
            HshIn.Add("chvEIN", sEIN);
            HshIn.Add("chvPhone", sMainNo);
            HshIn.Add("chvFax", sFaxNo);
            HshIn.Add("chvAddress1", sAddress);
            HshIn.Add("chvAddress2", hAddress);
            HshIn.Add("intCountry", iCountry);
            HshIn.Add("intState", iState);
            HshIn.Add("intCity", iCity);
            HshIn.Add("chvZip", sZip);
            HshIn.Add("chvBillingAddress1", sBillingaddress);
            HshIn.Add("chvBillingAddress2", sBillingaddress2);
            HshIn.Add("intBillingCountry", sBillingcountry);
            HshIn.Add("intBillingState", sBillingstate);
            HshIn.Add("intBillingCity", sBillingcity);
            HshIn.Add("chvBillingZip", sBillingzip);
            HshIn.Add("chvBillphone", sBillingphone);
            HshIn.Add("YearEndDate", Convert.ToDateTime(sEndofyear));
            HshIn.Add("bitTrialVersion", iTrialVersion);

            HshIn.Add("inyTrialDays", iTrialDays);
            HshIn.Add("intEncodedBy", iEncodedBy);
            hshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRUpdateHospitalSetUp", HshIn, hshOut);
                return hshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getHospitalSetUp()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("intFacilityId", FacilityId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetHospitalSetUp", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetPageMandatoryFields(int HospId, int FacilityId, string ModuleName, string PageName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@HospId", HospId);
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@ModuleName", ModuleName);
            HshIn.Add("@PageName", PageName);

            string strSQL = "SELECT FieldName, Active FROM HospitalMandatoryFields WITH (NOLOCK) WHERE PageName = @PageName AND ModuleName = @ModuleName AND Active = 1 AND FacilityID = @FacilityId AND HospitalLocationId = @HospId ";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public string GetFlagValueHospitalSetup(int HospitalLocation, string strFlag)
        {
            string strValue = string.Empty;
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@hospid", HospitalLocation);
            HshIn.Add("@flag", strFlag);
            string strSQL = "Select Value From HospitalSetup WITH (NOLOCK) Where Flag = @flag and HospitalLocationId = @hospid ";
            try
            {

                ds = objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strValue = Convert.ToString(ds.Tables[0].Rows[0]["Value"]);
                }
                return strValue;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }
        public DataSet GetHospitalSetup()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", FacilityId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMasterGetHospitalSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getHostitalSetupModules()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            string strSQL = " SELECT DISTINCT smm.ModuleId, smm.ModuleName FROM HospitalSetupMain hsm WITH (NOLOCK) INNER JOIN SecModuleMaster smm WITH (NOLOCK) ON hsm.ModuleId = smm.ModuleId";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet getHospitalSetup(int HospId, int FacilityId, string Flag, int ModuleId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationID", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvFlag", Flag);
            HshIn.Add("@inyModuleId", ModuleId);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspMasterGetHospitalSetup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public class clsHospitalSetupFlag
        {
            private string sConString = string.Empty;
            public clsHospitalSetupFlag(string conString)
            {
                sConString = conString;
            }
            Hashtable HshIn;
            Hashtable HshOut;
            public int HospitalLocationId = 0;
            public int ID = 0;
            public int UserID = 0;
            public string Flag = string.Empty;
            public string Value = string.Empty;
            public string Description = string.Empty;

            public DataSet getData()
            {
                StringBuilder sb = new StringBuilder();
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                try
                {

                    HshIn.Add("@hospId", HospitalLocationId);
                    sb.Append("SELECT ID, Flag, Value, Description FROM HospitalSetup WITH (NOLOCK) WHERE Active = 1 and HospitalLocationId = @hospId ");
                    return objDl.FillDataSet(CommandType.Text, sb.ToString(), HshIn);
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }


            }

            public string saveData(clsHospitalSetupFlag objHSFlag)
            {
                string sProc = "uspSaveUpdateHospitalSetupFlag";
                HshIn = new Hashtable();
                HshOut = new Hashtable();
                HshIn.Add("intId", objHSFlag.ID);
                HshIn.Add("inyHospitalLocationId", objHSFlag.HospitalLocationId);
                HshIn.Add("chvFlag", objHSFlag.Flag);
                HshIn.Add("chvValue", objHSFlag.Value);
                HshIn.Add("chvDescription", objHSFlag.Description);
                HshIn.Add("intEncodedBy", objHSFlag.UserID);
                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, sProc, HshIn, HshOut);
                    return HshOut["chvErrorStatus"].ToString();
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

            public int deleteData(clsHospitalSetupFlag objHSFlag)
            {
                HshIn = new Hashtable();
                StringBuilder sb = new StringBuilder();
                HshIn.Add("@intId", objHSFlag.ID);
                HshIn.Add("@intEncodedBy", objHSFlag.UserID);
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
                {

                    sb.Append("UPDATE HospitalSetup SET Active = 0, LastChangedBy = @intEncodedBy, LastChangedDate = GETUTCDATE()  WHERE ID = @intId");
                    int intExec = objDl.ExecuteNonQuery(CommandType.Text, sb.ToString(), HshIn);
                    return intExec;
                }
                catch (Exception ex) { throw ex; }
                finally { HshIn = null; objDl = null; }

            }

        }
        public string SaveHospitalSetup()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@xmlHospitalFlags", XMLHospitalFlags);
            HshIn.Add("@intEncodedBy", iEncodedBy);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspMasterSaveHospitalSetup", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetFlagValueOfHospitalSetup(int HospitalLocation, string strFlag)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@hospid", HospitalLocation);
            HshIn.Add("@flag", strFlag);
            string strSQL = "Select Flag, Value From HospitalSetup WITH (NOLOCK) Where Flag IN ('" + strFlag + "') and HospitalLocationId = 1 ";

            try
            {
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string SaveHospitalMandatoryFields(int HospId, int FacilityId, string ModuleName,
                                    string PageName, string xmlFieldNames, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityID", FacilityId);
            hshIn.Add("@chvModuleName", ModuleName);
            hshIn.Add("@chvPageName", PageName);
            hshIn.Add("@xmlFieldNames", xmlFieldNames);
            hshIn.Add("@intEncodedBy", EncodedBy);

            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveHospitalMandatoryFields", hshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getHospitalMandatoryFields(int HospId, int FacilityId, string ModuleName, string PageName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@inyHospitalLocationId", HospId);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@chvModuleName", ModuleName);
            hshIn.Add("@chvPageName", PageName);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetHospitalMandatoryFields", hshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getModuleMaster()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();

            string strSQL = "SELECT ModuleId, ModuleName FROM SecModuleMaster WITH (NOLOCK) WHERE Active = 1 order by ModuleName";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL, hshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getModulePages(int ModuleId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@intModuleId", ModuleId);

            string strSQL = "SELECT PageName FROM HospitalMandatoryPages WITH (NOLOCK) WHERE ModuleId = @intModuleId AND Active = 1 ORDER BY PageName";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL, hshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getHospitalMandatoryPages()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            string strSQL = "SELECT PageId, PageName, TableName FROM HospitalMandatoryPages WITH (NOLOCK) WHERE Active = 1";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        private string getSplitedTable(string TableNames)
        {
            string[] tblName = TableNames.Split(',');

            StringBuilder sbIN = new StringBuilder();

            for (int tIdx = 0; tIdx < tblName.Length; tIdx++)
            {
                string tName = tblName[tIdx].Trim();
                if (tName != string.Empty)
                {
                    if (sbIN.ToString() == string.Empty)
                    {
                        sbIN.Append("OBJECT_ID('" + tName + "')");
                    }
                    else
                    {
                        sbIN.Append(", OBJECT_ID('" + tName + "')");
                    }
                }
            }

            if (sbIN.ToString() == string.Empty)
            {
                sbIN.Append("OBJECT_ID('')");
            }

            return sbIN.ToString();
        }

        public DataSet getTableColumns(string TableNames)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            string strIN = getSplitedTable(TableNames);

            string strSQL = "SELECT DISTINCT name as ColumnName FROM sys.columns WITH (NOLOCK) WHERE object_id IN (" + strIN + ") ORDER BY name";
            try
            {

                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetRegistrationchargeapplicable()
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); 
            try
            {

                string strsql = "SELECT DISTINCT rgc.Id,cmp.CompanyId,cmp.Name,rgc.Regchargeapplicable  FROM Company cmp WITH (NOLOCK) " +
                              " INNER JOIN Registrationcharges rgc WITH (NOLOCK) ON cmp.CompanyId=rgc.CompanyId WHERE Regchargeapplicable=1 ORDER BY cmp.Name  ";

                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public int DeleteRegistrationchargecompany(int Id)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strupdate = " UPDATE Registrationcharges SET Regchargeapplicable=0 WHERE Id=" + Id;
                return objDl.ExecuteNonQuery(CommandType.Text, strupdate);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }




        }
        public string SaveRegistrationchargeapplicable(string XMLCompanyId, int FacilityId, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();

                HshIn.Add("@XMLCompanyId", XMLCompanyId);
                HshIn.Add("@intEncodedBy", EncodedBy);
                HshIn.Add("@intFacilityId", Facility);

                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveRegistrationcharges", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAppliationServerDetails(string IpAddress)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();

            try
            {

                HshIn.Add("@chvIpAddress", IpAddress);
                string strSQL = "SELECT FacilityID, FM.Name AS FacilityName, ID AS HospitalId,hl.Name AS HospitalLocationName FROM FacilityMaster FM WITH (NOLOCK) INNER JOIN HospitalLocation HL WITH (NOLOCK) ON FM.HospitalLocationID=HL.Id where ApplicationServerIPAddress = @chvIpAddress";
                return objDl.FillDataSet(CommandType.Text, strSQL, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet GetAppliationServerDetails(int iFacilityId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@intFacilityId", iFacilityId);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFacilitySerialNo", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public int UpdateTransactionTableInOffline(int iHospitalLocationId, int iFacilityId, string xmlSeriesNote)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@xmlSeriesNos", xmlSeriesNote);

            try
            {

                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspUpdateOfflineSite", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPageMandatoryAndEnableFields(int HospId, int FacilityId, string ModuleName, string PageName)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();

            hshIn.Add("@HospId", HospId);
            hshIn.Add("@FacilityId", FacilityId);
            hshIn.Add("@ModuleName", ModuleName);
            hshIn.Add("@PageName", PageName);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPageMandatoryAndEnableFields", hshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetFlagValueOfHospitalSetup(int HospitalLocation, int FacilityId, string strFlag)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@intHospitalLocationId", HospitalLocation);
            hshIn.Add("@intFacilityId", FacilityId);
            hshIn.Add("@strFlag", strFlag);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetFlagValueFacilityWise", hshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public DataSet getHospitalSetupValueMultiple(string Flag, int HospId, int FacilityId)
        {
            Hashtable HshIn = new Hashtable();
            //DataSet ds = new DataSet();
            //HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);

                string qry = "SELECT Flag, Value FROM HospitalSetup WITH (NOLOCK) " +
                            " WHERE Flag IN ( " + Flag + " ) AND Active = 1 AND FacilityId = @intFacilityId AND HospitalLocationId = @intHospId ";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                objDl = null;
                HshIn = null;
            }
            //return ds;
        }

    }
}
