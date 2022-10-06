using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace BaseC
{
    public class Patient
    {
        string sConString = string.Empty;// "server=a11;database=NH+;uid=sa;pwd=;MultipleActiveResultSets=True;";
        private string sFirstName = string.Empty, sMiddleName = string.Empty, sLastName = string.Empty, sGuardianName = string.Empty, sAddress = string.Empty, sAddressLine2 = string.Empty, sContactPerson = string.Empty, sContactPersonMobile = string.Empty, sIdentityTypeNumber = string.Empty, sPreviousName = string.Empty, sSponsorId = string.Empty;
        private string sPin = string.Empty, sPhone = string.Empty, sMobile = string.Empty, sEmail = string.Empty, sDisplayName = string.Empty, sWorkAddress = string.Empty, sWorkAddress2 = string.Empty, sWorkPin = string.Empty, sWorkEmail = string.Empty, sEducationStatus = string.Empty;
        private string sContactPersonPhone = string.Empty, sPolicyNo = string.Empty, sStatus = string.Empty, sWorkNumber = string.Empty, sWorkExtn = string.Empty;
        private string sVIPNarration = string.Empty, sSpecialNote = string.Empty, sEmployeeNo = string.Empty, sXMLAllergy = string.Empty, sDOB = string.Empty, sYear = string.Empty, sMonth = string.Empty, sDay = string.Empty;
        private string sRegistrationValidTill, sPolicyExpiryDate, sLockRecord = string.Empty, sAppointmentNotification = string.Empty;
        private Int16 iGuardianRelation = 0;
        private string iDefImageCenterID = string.Empty, iDefLabID = string.Empty, iDefPharmacy = string.Empty, iPayerId = string.Empty, iSponsorId = string.Empty, iRaceID = string.Empty, iPCP = string.Empty, iRenderingProvider = string.Empty, iIdentityTypeID = string.Empty, iNationality = string.Empty, iMaritalStatus = string.Empty, iReligionId = string.Empty, iTitle = string.Empty, iCountry = string.Empty, iState = string.Empty, iCity = string.Empty, iFacility = string.Empty, iWorkCountry = string.Empty, iWorkState = string.Empty, iWorkCity = string.Empty, sTaggedEmpNo = string.Empty, sTaggedEmpName = string.Empty, sEnrolleeNumber = string.Empty;
        private Int16 iHospitalLocationId = 0, iOccupationId = 0, iDoctorId = 0, iResposibleSelf = 0;
        private int iRegistrationId = 0, iRegistrationNo = 0, iAlternateRegistrationNo = 0, iEnableAllert = 0, iAccountCategory = 0, iAccountType = 0, iPlanType = 0;
        private Int16 iContactPersonRelaton = 0;
        private Int16 iEntryLocationID = 0, iEncodedBy = 0, iLastChangedBy = 0, iBirthOrder = 0;
        private Int32 iLanguageID = 0, iReferralByID = 0, iReferralTypeID = 0;
        private Byte bRegistryExclude = 0, bVIP = 0, bMLC = 0, bNewBorn = 0, bExternalPatient = 0;
        private Int32 iVFCProviderID = 0, iEthnicityID = 0, iEmploymentStatusID = 0, iStudentStatusID = 0, iConfidentialComm = 0;
        private Int64 iAppointmentId = 0;
        private String sEmployerName = string.Empty, sSchoolCollege = string.Empty, sResidentType = string.Empty, sSSN = string.Empty, sPrivateKey = string.Empty;
        private string dtDeathdeth = string.Empty, chvCuaseofdeth = string.Empty, sNotificationphone = string.Empty, sNotificationType = string.Empty;
        private Int32 intfacilityId = 0, intPageID = 0, sRemovePassword = 0, iLeadSourceID = 0, iShowNoteInAppointment = 0;
        private Int32 iIndustryId = 0;
        private bool bAgeIdentification = false;
        Hashtable HshIn;

        //public enum MaritalStatusOptions
        //{
        //    //------- declare a ENUM which defines the possible Marital Status -----
        //    Separated,
        //    Divorced,
        //    Married,
        //    Single,
        //    Widowed
        //}

        //public enum GenderOptions
        //{
        //    //------- declare a ENUM which defines the Gender -----
        //    Female,
        //    Male,
        //    Other,
        //    Unknown
        //}

        //public enum VIPOptions
        //{
        //   //----- declare a ENUM which defines the VIP -----
        //   Y,
        //   N
        //}

        //public enum MLCOptions
        //{
        //    //----- declare a ENUM which defines the MLC -----
        //    Y,
        //    N
        //}

        //public enum NewBornOptions
        //{
        //    //----- declare a ENUM which defines the New Born -----
        //    Y,
        //    N
        //}



        private char sVIP;

        private string iGender;

        private char sMLC;

        private char sNewBorn;

        public Patient(string Constring)
        {
            sConString = Constring;
        }
        public Int32 ShowNoteInAppointment
        {
            get { return iShowNoteInAppointment; }
            set { iShowNoteInAppointment = value; }
        }

        public Int32 FacilityId
        {
            get { return intfacilityId; }
            set { intfacilityId = value; }
        }
        public Int32 RemovePassword
        {
            get
            {
                return sRemovePassword;
            }
            set
            {
                sRemovePassword = value;
            }
        }
        public Int32 PageId
        {
            get { return intPageID; }
            set { intPageID = value; }
        }
        public string DeathDate
        {
            get
            {

                return dtDeathdeth;
            }
            set
            {
                dtDeathdeth = value;
            }
        }
        public int ConfidentialComm
        {
            get
            {

                return iConfidentialComm;
            }
            set
            {
                iConfidentialComm = value;
            }
        }
        public string CauseOfDeath
        {
            get
            {
                return chvCuaseofdeth;
            }
            set
            {
                chvCuaseofdeth = value;
            }

        }
        public string Noticationphoneno
        {
            get { return sNotificationphone; }
            set { sNotificationphone = value; }
        }
        public string NotificationNo
        {
            get
            {
                return sNotificationType;
            }
            set
            {
                sNotificationType = value;
            }
        }
        public string SocialSecurityNumber
        {
            get
            {
                return sSSN;
            }
            set
            {
                sSSN = value;
            }
        }
        public int RegistrationId
        {
            get
            {
                return iRegistrationId;
            }
            set
            {
                iRegistrationId = value;
            }
        }
        public int RegistrationNo
        {
            get
            {
                return iRegistrationNo;
            }
            set
            {
                iRegistrationNo = value;
            }
        }

        public int AlternateRegistrationNo
        {
            get
            {
                return iAlternateRegistrationNo;
            }
            set
            {
                iAlternateRegistrationNo = value;
            }
        }

        public Int16 HospitalLocationId
        {
            get
            {
                return iHospitalLocationId;
            }
            set
            {
                iHospitalLocationId = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return sDisplayName;
            }
            set
            {
                sDisplayName = value;
            }
        }

        public string Title
        {
            get
            {
                return iTitle;
            }
            set
            {
                iTitle = value;
            }
        }

        public string FirstName
        {
            get
            {
                return sFirstName;
            }
            set
            {
                sFirstName = value;
            }
        }

        public string MiddleName
        {
            get
            {
                return sMiddleName;
            }
            set
            {
                sMiddleName = value;
            }
        }

        public string LastName
        {
            get
            {
                return sLastName;
            }
            set
            {
                sLastName = value;
            }
        }

        public string DOB
        {
            get
            {
                return sDOB;
            }
            set
            {
                sDOB = value;
            }
        }

        public string Year
        {
            get
            {
                return sYear;
            }
            set
            {
                sYear = value;
            }
        }

        public string Month
        {
            get
            {
                return sMonth;
            }
            set
            {
                sMonth = value;
            }
        }

        public string Day
        {
            get
            {
                return sDay;
            }
            set
            {
                sDay = value;
            }
        }

        public string Gender
        {
            get
            {
                return iGender;
            }
            set
            {
                iGender = value;
            }
        }

        public Int16 GuardianRelation
        {
            get
            {
                return iGuardianRelation;
            }
            set
            {
                iGuardianRelation = value;
            }
        }

        public string GuardianName
        {
            get
            {
                return sGuardianName;
            }
            set
            {
                sGuardianName = value;
            }
        }

        public string MaritalStatus
        {
            get
            {
                return iMaritalStatus;
            }
            set
            {
                iMaritalStatus = value;
            }
        }

        public string Address
        {
            get
            {
                return sAddress;
            }
            set
            {
                sAddress = value;
            }
        }
        public string AddressLine2
        {
            get
            {
                return sAddressLine2;
            }
            set
            {
                sAddressLine2 = value;
            }
        }

        public string Country
        {
            get
            {
                return iCountry;
            }
            set
            {
                iCountry = value;
            }
        }

        public string State
        {
            get
            {
                return iState;
            }
            set
            {
                iState = value;
            }
        }

        public string City
        {
            get
            {
                return iCity;
            }
            set
            {
                iCity = value;
            }
        }

        public string Pin
        {
            get
            {
                return sPin;
            }
            set
            {
                sPin = value;
            }
        }

        public string Phone
        {
            get
            {
                return sPhone;
            }
            set
            {
                sPhone = value;
            }
        }

        public string Mobile
        {
            get
            {
                return sMobile;
            }
            set
            {
                sMobile = value;
            }
        }

        public string Email
        {
            get
            {
                return sEmail;
            }
            set
            {
                sEmail = value;
            }
        }

        public int ReferralTypeID
        {
            get
            {
                return iReferralTypeID;
            }
            set
            {
                iReferralTypeID = value;
            }
        }

        public int ReferredByID
        {
            get
            {
                return iReferralByID;
            }
            set
            {
                iReferralByID = value;
            }
        }

        public Int16 DoctorId
        {
            get
            {
                return iDoctorId;
            }
            set
            {
                iDoctorId = value;
            }
        }
        public string Facility
        {
            get
            {
                return iFacility;
            }
            set
            {
                iFacility = value;
            }
        }

        public string RegistrationValidTill
        {
            get
            {
                return sRegistrationValidTill;
            }
            set
            {
                sRegistrationValidTill = value;
            }
        }

        public string ReligionId
        {
            get
            {
                return iReligionId;
            }
            set
            {
                iReligionId = value;
            }
        }

        public Int16 OccupationId
        {
            get
            {
                return iOccupationId;
            }
            set
            {
                iOccupationId = value;
            }
        }

        public string Nationality
        {
            get
            {
                return iNationality;
            }
            set
            {
                iNationality = value;
            }
        }

        public string ContactPerson
        {
            get
            {
                return sContactPerson;
            }
            set
            {
                sContactPerson = value;
            }
        }

        public string ContactPersonPhone
        {
            get
            {
                return sContactPersonPhone;
            }
            set
            {
                sContactPersonPhone = value;
            }
        }

        public string ContactPersonMobile
        {
            get
            {
                return sContactPersonMobile;
            }
            set
            {
                sContactPersonMobile = value;
            }
        }

        public Int16 ContactPersonRelaton
        {
            get
            {
                return iContactPersonRelaton;
            }
            set
            {
                iContactPersonRelaton = value;
            }
        }

        public string IdentityTypeID
        {
            get
            {
                return iIdentityTypeID;
            }
            set
            {
                iIdentityTypeID = value;
            }
        }

        public string IdentityTypeNumber
        {
            get
            {
                return sIdentityTypeNumber;
            }
            set
            {
                sIdentityTypeNumber = value;
            }
        }

        public string SponsorId
        {
            get
            {
                return iSponsorId;
            }
            set
            {
                iSponsorId = value;
            }
        }

        public string PayerId
        {
            get
            {
                return iPayerId;
            }
            set
            {
                iPayerId = value;
            }
        }

        public string PolicyNo
        {
            get
            {
                return sPolicyNo;
            }
            set
            {
                sPolicyNo = value;
            }
        }

        public string PolicyExpiryDate
        {
            get
            {
                return sPolicyExpiryDate;
            }
            set
            {
                sPolicyExpiryDate = value;
            }
        }

        public char VIP
        {
            get
            {
                return sVIP;
            }
            set
            {
                sVIP = value;
            }
        }

        public string VIPNarration
        {
            get
            {
                return sVIPNarration;
            }
            set
            {
                sVIPNarration = value;
            }
        }

        public char MLC
        {
            get
            {
                return sMLC;
            }
            set
            {
                sMLC = value;
            }
        }

        public char NewBorn
        {
            get
            {
                return sNewBorn;
            }
            set
            {
                sNewBorn = value;
            }
        }

        public Int16 EntryLocationID
        {
            get
            {
                return iEntryLocationID;
            }
            set
            {
                iEntryLocationID = value;
            }
        }

        public string SpecialNote
        {
            get
            {
                return sSpecialNote;
            }
            set
            {
                sSpecialNote = value;
            }
        }

        public string EmploymentNo
        {
            get
            {
                return sEmployeeNo;
            }
            set
            {
                sEmployeeNo = value;
            }
        }

        public string xmlAllergy
        {
            get
            {
                return sXMLAllergy;
            }
            set
            {
                sXMLAllergy = value;
            }
        }

        public Int16 EncodedBy
        {
            get
            {
                return iEncodedBy;
            }
            set
            {
                iEncodedBy = value;
            }
        }

        public Int64 AppointmentId
        {
            get
            {
                return iAppointmentId;
            }
            set
            {
                iAppointmentId = value;
            }
        }

        public string Status
        {
            get
            {
                return sStatus;
            }
            set
            {
                sStatus = value;
            }
        }

        public Int16 LastChangedBy
        {
            get
            {
                return iLastChangedBy;
            }
            set
            {
                iLastChangedBy = value;
            }
        }


        public string WorkCountry
        {
            get
            {
                return iWorkCountry;
            }
            set
            {
                iWorkCountry = value;
            }
        }

        public string WorkState
        {
            get
            {
                return iWorkState;
            }
            set
            {
                iWorkState = value;
            }
        }

        public string WorkCity
        {
            get
            {
                return iWorkCity;
            }
            set
            {
                iWorkCity = value;
            }
        }
        public string TaggedEmpName
        {
            get
            {
                return sTaggedEmpName;
            }
            set
            {
                sTaggedEmpName = value;
            }
        }

        public string EnrolleeNumber
        {
            get
            {
                return sEnrolleeNumber;
            }
            set
            {
                sEnrolleeNumber = value;
            }
        }

        public string TaggedEmpNo
        {
            get
            {
                return sTaggedEmpNo;
            }
            set
            {
                sTaggedEmpNo = value;
            }
        }
        public String PreviousName
        {
            get
            {
                return sPreviousName;
            }
            set
            {
                sPreviousName = value;
            }
        }
        public string PayerType
        {
            get
            {
                return sSponsorId;
            }
            set
            {
                sSponsorId = value;
            }
        }
        public String WorkAddress
        {
            get
            {
                return sWorkAddress;
            }
            set
            {
                sWorkAddress = value;
            }
        }
        public String WorkAddress2
        {
            get
            {
                return sWorkAddress2;
            }
            set
            {
                sWorkAddress2 = value;
            }
        }

        public String WorkPin
        {
            get
            {
                return sWorkPin;
            }
            set
            {
                sWorkPin = value;
            }
        }

        public String WorkEmail
        {
            get
            {
                return sWorkEmail;
            }
            set
            {
                sWorkEmail = value;
            }
        }

        public String WorkPhoneNumber
        {
            get
            {
                return sWorkNumber;
            }
            set
            {
                sWorkNumber = value;
            }
        }
        public String WorkPhoneExtn
        {
            get
            {
                return sWorkExtn;
            }
            set
            {
                sWorkExtn = value;
            }
        }

        public String EducationStatus
        {
            get
            {
                return sEducationStatus;
            }
            set
            {
                sEducationStatus = value;
            }
        }
        public int IndustryId
        {
            get
            {
                return iIndustryId;
            }
            set
            {
                iIndustryId = value;
            }
        }
        public string RaceID
        {
            get
            {
                return iRaceID;
            }
            set
            {
                iRaceID = value;
            }
        }

        public Int32 LanguageID
        {
            get
            {
                return iLanguageID;
            }
            set
            {
                iLanguageID = value;
            }
        }

        public Int16 BirthOrder
        {
            get
            {
                return iBirthOrder;
            }
            set
            {
                iBirthOrder = value;
            }
        }

        public String LockRecord
        {
            get
            {
                return sLockRecord;
            }
            set
            {
                sLockRecord = value;
            }
        }

        public string AppointmentNotification
        {
            get
            {
                return sAppointmentNotification;
            }
            set
            {
                sAppointmentNotification = value;
            }
        }

        public Int16 ResponsibleSelf
        {
            get
            {
                return iResposibleSelf;
            }
            set
            {
                iResposibleSelf = value;
            }
        }

        public string RenderingProvider
        {
            get
            {
                return iRenderingProvider;
            }
            set
            {
                iRenderingProvider = value;
            }
        }

        public string PCP
        {
            get
            {
                return iPCP;
            }
            set
            {
                iPCP = value;
            }
        }

        public string DefaultPharmacy
        {
            get
            {
                return iDefPharmacy;
            }
            set
            {
                iDefPharmacy = value;
            }
        }

        public string DefaultImageCenter
        {
            get
            {
                return iDefImageCenterID;
            }
            set
            {
                iDefImageCenterID = value;
            }
        }

        public string DefaultLab
        {
            get
            {
                return iDefLabID;
            }
            set
            {
                iDefLabID = value;
            }
        }

        public Int32 EthnicityID
        {
            get
            {
                return iEthnicityID;
            }
            set
            {
                iEthnicityID = value;
            }
        }

        public Int32 VFCProviderID
        {
            get
            {
                return iVFCProviderID;
            }
            set
            {
                iVFCProviderID = value;
            }
        }

        public Byte VIPByte
        {
            get
            {
                return bVIP;
            }
            set
            {
                bVIP = value;
            }
        }
        public Byte ExternalPatient
        {
            get
            {
                return bExternalPatient;
            }
            set
            {
                bExternalPatient = value;
            }
        }

        public Byte NewBornByte
        {
            get
            {
                return bNewBorn;
            }
            set
            {
                bNewBorn = value;
            }
        }

        public Byte RegistryExclude
        {
            get
            {
                return bRegistryExclude;
            }
            set
            {
                bRegistryExclude = value;
            }
        }

        public int EnableAllert
        {
            get
            {
                return iEnableAllert;
            }
            set
            {
                iEnableAllert = value;
            }
        }

        public int AccountCategory
        {
            get
            {
                return iAccountCategory;
            }
            set
            {
                iAccountCategory = value;
            }
        }

        public int AccountType
        {
            get
            {
                return iAccountType;
            }
            set
            {
                iAccountType = value;
            }
        }

        public int PlanType
        {
            get
            {
                return iPlanType;
            }
            set
            {
                iPlanType = value;
            }
        }

        public Int32 EmploymentStatusID
        {
            get
            {
                return iEmploymentStatusID;
            }
            set
            {
                iEmploymentStatusID = value;
            }
        }

        public String EmployerName
        {
            get
            {
                return sEmployerName;
            }
            set
            {
                sEmployerName = value;
            }
        }
        public Int32 StudentStatusID
        {
            get
            {
                return iStudentStatusID;
            }
            set
            {
                iStudentStatusID = value;
            }
        }
        public String SchoolCollege
        {
            get
            {
                return sSchoolCollege;
            }
            set
            {
                sSchoolCollege = value;
            }
        }
        public String PrivateKey
        {
            get { return sPrivateKey; }
            set { sPrivateKey = value; }
        }
        public String ResidentType
        {
            get { return sResidentType; }
            set { sResidentType = value; }
        }
        public Int32 LeadSourceId
        {
            get { return iLeadSourceID; }
            set { iLeadSourceID = value; }
        }

        public bool AgeIdentification
        {
            get { return bAgeIdentification; }
            set { bAgeIdentification = value; }
        }



        //----------------------------------------------------------------------------
        //   Purpose           :To Calculate age on DOB
        //   Input parameters  :string
        //   Output Parameters :string()
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public string[] CalculateAge(string strDateOfBirth)
        {
            string strAge = null;
            string[] strResult = new string[6];
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                strAge = (String)objDl.ExecuteScalar(CommandType.Text, "select dbo.AgeInYrsMonthDay('" + strDateOfBirth + "' ,convert(varchar,getdate(),111))");
                char[] sep = { ' ' };
                string[] strDays = strAge.Split(sep);

                //if (strDays.Length > 1)
                //{
                //    if (!strDays[1].Trim().StartsWith("0"))
                //    {
                //        strResult[1] = strDays[1].ToString();
                //        strResult[1] = Regex.Replace(strResult[1].ToString(), "[a-zA-Z]", "");
                //        //strResult[2] = strDays[2].ToString();
                //        //strResult[2] = Regex.Replace(strResult[2].ToString(), "[a-zA-Z]", "");
                //    }
                //    if (!strDays[2].Trim().StartsWith("0"))
                //    {
                //        strResult[2] = strDays[2].ToString();
                //        strResult[2] = Regex.Replace(strResult[2].ToString(), "[a-zA-Z]", "");
                //    }
                //}
                //else if (strDays.Length == 1)
                //{
                //    strResult[0] = strDays[0].ToString().Replace(".", "");
                //    strResult[0] = Regex.Replace(strResult[0].ToString(), "[a-zA-Z]", "");
                //}
                //else
                //{
                //    throw new Exception("Date of birth is not in correct format");
                //}

                return strDays;
            }
            catch (Exception ex)
            {
                throw new Exception("There is some problem in conversion process - " + ex.Message);
            }
            finally
            {
                objDl = null;
            }

        }


        //----------------------------------------------------------------------------
        //   Purpose           :To Convert Date into any format. Format is in [DMY] , [MDY], [YMD] All in caps.
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Chandan Kumar 
        //  --------------------------------------------------------------------------




        public string FormatDate(string strDate, string strinputformat, string stroutputformat)
        {
            char[] chSperator = { '/' };
            string[] strResult = strDate.Split(chSperator);
            String str = string.Empty;

            if (strResult.Length < 3)
            {
                return strDate;
            }

            if (strinputformat == "dd/MM/yyyy")
            {
                if (stroutputformat == "dd/MM/yyyy")
                {
                    str = strResult[0] + "/" + strResult[1] + "/" + strResult[2];
                }
                else if (stroutputformat == "MM/dd/yyyy")
                {
                    str = strResult[1] + "/" + strResult[0] + "/" + strResult[2];
                }

                else if (stroutputformat == "yyyy/MM/dd")
                {
                    str = strResult[2] + "/" + strResult[1] + "/" + strResult[0];
                }

            }


            else if (strinputformat == "MM/dd/yyyy")
            {
                if (stroutputformat == "dd/MM/yyyy")
                {
                    str = strResult[1] + "/" + strResult[0] + "/" + strResult[2];
                }
                else if (stroutputformat == "MM/dd/yyyy")
                {
                    str = strResult[0] + "/" + strResult[1] + "/" + strResult[2];
                }

                else if (stroutputformat == "yyyy/MM/dd")
                {
                    str = strResult[2] + "/" + strResult[0] + "/" + strResult[1];
                }

            }


            else if (strinputformat == "yyyy/MM/dd")
            {
                if (stroutputformat == "dd/MM/yyyy")
                {
                    str = strResult[2] + "/" + strResult[1] + "/" + strResult[0];
                }
                else if (stroutputformat == "MM/dd/yyyy")
                {
                    str = strResult[1] + "/" + strResult[2] + "/" + strResult[0];
                }

                else if (stroutputformat == "yyyy/MM/dd")
                {
                    str = strResult[0] + "/" + strResult[1] + "/" + strResult[2];
                }

            }

            return str;

        }



        //----------------------------------------------------------------------------
        //   Purpose           :To Convert Date in to yyyy/MM/dd format
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public string FormatDateYearMonthDate(string strDateOfBirth)
        {
            if (strDateOfBirth != string.Empty && strDateOfBirth != "__/__/____" && strDateOfBirth != null)
            {
                //input string should be in dd/MM/YYYY
                char[] chSperator = new char[1];
                if (strDateOfBirth.Contains("/"))
                    chSperator[0] = '/';
                else
                    chSperator[0] = '-';
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[2].IndexOf(' ') != -1)
                    str.Append(strResult[2].Substring(0, strResult[2].IndexOf(' ')));
                else
                    str.Append(strResult[2]);
                str.Append("/");
                str.Append(strResult[1]);
                str.Append("/");
                str.Append(strResult[0]);
                //output string is in yyyy/MM/dd format
                return str.ToString();
            }
            return null;
        }



        public string FormatDateMDY(string strDateOfBirth)
        {
            if (strDateOfBirth != string.Empty && strDateOfBirth != "__/__/____" && strDateOfBirth != null)
            {
                //input string should be in MM/dd/YYYY
                char[] chSperator = new char[1];
                if (strDateOfBirth.Contains("/"))
                    chSperator[0] = '/';
                else
                    chSperator[0] = '-';
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[2].IndexOf(' ') != -1)
                    str.Append(strResult[2].Substring(0, strResult[2].IndexOf(' ')));
                else
                    str.Append(strResult[2]);
                str.Append("/");
                str.Append(strResult[0]);
                str.Append("/");
                str.Append(strResult[1]);
                //output string is in yyyy/MM/dd format
                return str.ToString();
            }
            return null;
        }

        public string FormatDMY(string strDateOfBirth)
        {
            if (strDateOfBirth != string.Empty && strDateOfBirth != "__/__/____")
            {
                //input string should be in dd/MM/yyyy
                char[] chSperator = { '/' };
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[1].Length == 1)
                    str.Append("0" + strResult[1]);
                else
                    str.Append(strResult[1]);
                str.Append("/");
                if (strResult[0].Length == 1)
                    str.Append("0" + strResult[0]);
                else
                    str.Append(strResult[0]);
                str.Append("/");
                str.Append(strResult[2]);
                //output string is in MM/dd/yyyy format
                return str.ToString();
            }
            else
                return null;
        }




        //----------------------------------------------------------------------------
        //   Purpose           :To Convert Date into dd/MM/yyyy format
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public string FormatDateDateMonthYear(string strDateOfBirth)
        {
            if (strDateOfBirth != string.Empty && strDateOfBirth != "__/__/____")
            {
                //input string should be in MM/dd/yyyy
                char[] chSperator = { '/' };
                string[] strResult = strDateOfBirth.Split(chSperator);
                StringBuilder str = new StringBuilder();
                if (strResult[1].Length == 1)
                    str.Append("0" + strResult[1]);
                else
                    str.Append(strResult[1]);
                str.Append("/");
                if (strResult[0].Length == 1)
                    str.Append("0" + strResult[0]);
                else
                    str.Append(strResult[0]);
                str.Append("/");
                str.Append(strResult[2]);
                //output string is in dd/MM/yyyy format
                return str.ToString();
            }
            else
                return null;
        }

        public DataSet GetSpecialization(int HospitalId, int UserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable HshIn = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalId);
            HshIn.Add("@intUserId", UserId);


            try
            {
                string strsql = "SELECT dd.DoctorId,dd.SpecialisationId,sm.Name AS SpecializationName FROM  DoctorDetails dd WITH (NOLOCK) " +
                        " INNER JOIN UserS us WITH (NOLOCK) ON dd.DoctorId=us.EmpID " +
                        " INNER JOIN SpecialisationMaster sm WITH (NOLOCK) ON dd.SpecialisationId=sm.ID " +
                        " WHERE us.ID = @intUserId AND us.HospitalLocationID =@inyHospitalLocationId ";
                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }


        public SqlDataReader getPatientDetails(Int64 regNo, int mode, int HospitalLocationId)
        {
            // mode=1: basic 
            // mode=2: Advanced
            Hashtable hsInput = new Hashtable();
            Hashtable hsOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsInput.Add("RegistrationId", regNo);
            hsInput.Add("Mode", mode);
            try
            {

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "uspGetPatientRecord", hsInput);

                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader getPatientDetails(int iRegistrationId, int iHospitalLocationId)
        {
            Hashtable hsInput = new Hashtable();

            StringBuilder objStr = new StringBuilder();
            objStr.Append("select rg.registrationno, dbo.GetPatientName(0,FirstName,MiddleName,LastName)+ ', ' +  dbo.UdfCurrentAgeGender(rg.Gender, rg.DateOfBirth, GETDATE()) as Name, rg.Gender, dbo.GetPatientName(0,FirstName,MiddleName,LastName) as PatientName, rg.Id ");
            objStr.Append(" from registration rg WITH (NOLOCK) where rg.ID = @RegistrationID AND rg.HospitalLocationId = @HospitalLocationId ");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsInput.Add("RegistrationID", iRegistrationId);
            hsInput.Add("HospitalLocationId", iHospitalLocationId);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, objStr.ToString(), hsInput);

                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getPatientOldNoDetails(int regNo, int mode, string RegNoOld)
        {
            // mode=1: basic 
            // mode=2: Advanced

            Hashtable HshIn = new Hashtable();
            Hashtable hsOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("RegistrationId", regNo);
            HshIn.Add("Mode", mode);
            HshIn.Add("intOldRegistrationId", RegNoOld);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientRecord", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet getPatientDetails(int iHospID, int iFacilityId, int iRegistrationId, int RegistrationNo, int iEncounterId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationId", iHospID);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("chvRegistrationNo", RegistrationNo);
            HshIn.Add("intEncounterId", iEncounterId);
            HshIn.Add("intEncodedBy", EncodedBy);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPatientOldNoDetails(Int64 regNo, int mode, string RegNoOld)
        {
            // mode=1: basic 
            // mode=2: Advanced

            Hashtable hsInput = new Hashtable();
            Hashtable hsOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hsInput.Add("RegistrationId", regNo);
            hsInput.Add("Mode", mode);
            hsInput.Add("intOldRegistrationId", RegNoOld);


            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientRecord", hsInput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getEMRPatientDetails(Int64 iHospID, int iFacilityId, string sRegistrationNo, string sEncounterNo, int EncodedBy)
        {
            return getEMRPatientDetails(iHospID, iFacilityId, sRegistrationNo, sEncounterNo, EncodedBy, 0);
        }


        public DataSet getEMRPatientDetails(Int64 iHospID, int iFacilityId, string sRegistrationNo, string sEncounterNo, int EncodedBy, int iRegId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intRegId", iRegId);
            HshIn.Add("@chvRegistrationNo", sRegistrationNo);
            HshIn.Add("@chvEncounterNo", sEncounterNo);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDetails", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet getEMRPatientDetailsImmunizationPrint(int iHospID, int iFacilityId, string sRegistrationNo, string sEncounterNo, int EncodedBy, int iRegId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@intRegId", iRegId);
            HshIn.Add("@chvRegistrationNo", sRegistrationNo);
            HshIn.Add("@chvEncounterNo", sEncounterNo);
            HshIn.Add("@intEncodedBy", EncodedBy);

            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDetailsImmunizationPrint", HshIn, HshOut);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Patient Details
        //   Input parameters  :registration no, HospitalLocationId
        //   Output Parameters :sqldatareader
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------


        //----------------------------------------------------------------------------
        //   Purpose           :To Get Patient Short Detail
        //   Input parameters  :registrationId, FormId and EncounterId
        //   Output Parameters :sqldatareader
        //   Author            :Rafat Anwer
        //  --------------------------------------------------------------------------
        public SqlDataReader getPatientShortDetail(int iRegistrationId, int iFormId, int iEncounterId)
        {
            Hashtable HshIn = new Hashtable();

            StringBuilder objStr = new StringBuilder();
            objStr.Append("select rg.Id, rg.registrationno, dbo.GetPatientName(0,FirstName,MiddleName,LastName)+ ', ' +  dbo.UdfCurrentAgeGender(rg.Gender, rg.DateOfBirth, GETDATE()) as Name, rg.Gender ");
            objStr.Append(" from registration rg WITH (NOLOCK) where rg.Id = @RegistrationId ");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("RegistrationId", iRegistrationId);
            if (iFormId != 0 && iEncounterId != 0)
            {
                objStr = new StringBuilder(string.Empty);
                HshIn.Add("FormId", iFormId);
                HshIn.Add("EncounterId", iEncounterId);
                objStr.Append(" select rg.Id, rg.registrationno, dbo.GetPatientName(0,FirstName,MiddleName,LastName)+ ', ' + dbo.UdfCurrentAgeGender(rg.Gender, rg.DateOfBirth, GETDATE()) +', '+ frm.FormName as Name, rg.Gender, epf.FormId ");
                objStr.Append(" from registration rg WITH (NOLOCK) Inner Join Encounter en WITH (NOLOCK) On en.registrationid = rg.Id");
                objStr.Append(" Inner Join EMRPatientForms epf WITH (NOLOCK) On en.Id = epf.EncounterId ");
                objStr.Append(" Inner Join EMRForms frm WITH (NOLOCK) On frm.Id = epf.FormId ");
                objStr.Append(" where epf.EncounterId=@EncounterId AND rg.Id = @RegistrationId and epf.FormId = @FormId and epf.Active=1 ");
            }
            try
            {

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, objStr.ToString(), HshIn);

                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public SqlDataReader getPatientShortDetail(int iRegistrationId, int iEncounterId)
        {
            Hashtable hsInput = new Hashtable();

            hsInput.Add("RegistrationId", iRegistrationId);

            StringBuilder objStr = new StringBuilder();
            objStr.Append("select rg.Id, rg.registrationno, dbo.GetPatientName(0,FirstName,MiddleName,LastName)+ ', ' +  dbo.UdfCurrentAgeGender(rg.Gender, rg.DateOfBirth, GETDATE()) as Name, rg.Gender ");
            objStr.Append(" from registration rg WITH (NOLOCK) ");
            objStr.Append(" where rg.Id = @RegistrationId ");

            if (iEncounterId > 0)
            {
                objStr = new StringBuilder();

                hsInput.Add("EncounterId", iEncounterId);

                objStr.Append(" select rg.Id, rg.registrationno, dbo.GetPatientName(0,FirstName,MiddleName,LastName)+ ', ' +  dbo.UdfCurrentAgeGender(rg.Gender, rg.DateOfBirth, GETDATE()) as Name, rg.Gender ");
                objStr.Append(" from registration rg WITH (NOLOCK) ");
                objStr.Append(" Inner Join Encounter en WITH (NOLOCK) On en.registrationid = rg.Id ");
                objStr.Append(" where en.Id=@EncounterId and rg.Id = @RegistrationId and en.active = 1 ");
            }

            //if (iEncounterId != 0)
            //{
            //    objStr = new StringBuilder("");
            //    hsInput.Add("EncounterId", iEncounterId);
            //    objStr.Append(" select rg.Id, rg.registrationno,  dbo.GetPatientName(0,FirstName,MiddleName,LastName)+ ', ' + dbo.UdfCurrentAgeGender(rg.Gender, rg.DateOfBirth, GETDATE()) + ', ' + ISNULL(ct.Name,'') as Name, rg.Gender from registration rg with (nolock) Inner Join Encounter en  with (nolock) On en.registrationid = rg.Id inner join AccountTypes at with (nolock) on en.AccountTypeId = at.TypeId inner join CompanyType ct  with (nolock) on at.AccountCategoryID = ct.Id where rg.Id = @RegistrationId and en.Id=@EncounterId and en.active = 1");
            //}

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, objStr.ToString(), hsInput);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getPatientShortDetailDS(int iRegistrationId, int iEncounterId)
        {
            Hashtable hsInput = new Hashtable();
            hsInput.Add("RegistrationId", iRegistrationId);
            hsInput.Add("EncounterId", iEncounterId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetPatientShortDetail", hsInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        //----------------------------------------------------------------------------
        //   Purpose           :To split first name into middle and lastname
        //   Input parameters  :string
        //   Output Parameters :string()
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //Not in Use
        //public string[] split_patient_name(string firstName, string middleName, string lastName)
        //{
        //    string[] string_split = null;
        //    short i = 0;
        //    string name = "";
        //    bool cont_space = false;
        //    string[] strResult = new string[3];

        //    firstName = firstName.Trim() + ' ' + middleName.Trim() + ' ' + lastName.Trim();
        //    for (i = 0; i <= firstName.Length - 1; i++)
        //    {
        //        if (i > 0)
        //        {
        //            if (firstName.Substring(i, 1) != " ")
        //            {
        //                name = name + firstName.Substring(i, 1);
        //                cont_space = false;
        //            }
        //            else if (cont_space == false & firstName.Substring(i, 1) == " ")
        //            {
        //                name = name + " ";
        //                cont_space = true;
        //            }
        //        }
        //        else
        //        {
        //            name = name + firstName.Substring(i, 1);
        //        }
        //    }
        //    firstName = name;
        //    char[] chArr = { ' ' };
        //    string_split = firstName.Trim().Split(chArr, 3);

        //    if (string_split.GetLength(0) > 0) strResult[0] = string_split[0].Trim();
        //    if (string_split.GetLength(0) > 1 & string_split.GetLength(0) <= 2)
        //    {
        //        if (string_split.GetLength(0) > 1) strResult[2] = string_split[1].Trim();
        //    }
        //    else
        //    {
        //        if (string_split.GetLength(0) > 1) strResult[1] = string_split[1].Trim();
        //    }
        //    if (string_split.GetLength(0) > 2) strResult[2] = string_split[2].Trim();
        //    return strResult;
        //}

        //----------------------------------------------------------------------------
        //   Purpose           :To parse money data type
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //Not in Use
        //public string FormatMoney(string Money)
        //{
        //    Money = Money.Trim();
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    int DecimalPoints = Convert.ToInt32((string)objDl.ExecuteScalar(CommandType.Text, "select value from HospitalSetup where Flag = 'DecimalPlaces'"));
        //    int DecimalIndex = Money.IndexOf(".");
        //    string sResult = "";
        //    if (DecimalIndex > 0)
        //    {
        //        if ((Money.Length - DecimalIndex) > DecimalPoints + 1)
        //        {
        //            sResult = Money.Substring(0, Money.Length - (Money.Length - DecimalIndex) + DecimalPoints + 1);
        //        }
        //        else if ((Money.Length - DecimalIndex) < DecimalPoints + 1 && (Money.Length - DecimalIndex) > 0)
        //        {
        //            sResult = Money;
        //            for (int i = 0; i < DecimalPoints - (Money.Length - DecimalIndex) + 1; i++)
        //            {
        //                sResult = sResult + "0";
        //            }
        //        }
        //        else if ((Money.Length - DecimalIndex) == DecimalPoints + 1)
        //        {
        //            sResult = Money;
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < DecimalPoints; i++)
        //        {
        //            sResult = sResult + "0";
        //        }
        //        sResult = Money + "." + sResult;
        //    }
        //    return sResult;
        //}

        //----------------------------------------------------------------------------
        //   Purpose           :To Save Patient Registration Information
        //   Author            :Rafat
        //   Date              :28 May 2010
        //  --------------------------------------------------------------------------
        public Hashtable SaveUpdateRegistration()
        {
            StringBuilder sXml = new StringBuilder();
            BaseC.ParseData bC = new BaseC.ParseData();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshTableOut = new Hashtable();
            Hashtable hshTable = new Hashtable();



            try
            {


                hshTable.Add("@inyHospitalLocationId", iHospitalLocationId);
                hshTable.Add("@intEncodedBy", iEncodedBy); // mandatory
                hshTable.Add("@intLoginFacilityId", FacilityId);
                hshTable.Add("@intPageId", PageId);
                if (sPreviousName != string.Empty) { hshTable.Add("chvPreviousName", sPreviousName); }
                if (iTitle != string.Empty && iTitle != "0") { hshTable.Add("@intTitle", iTitle); }

                hshTable.Add("@chvFirstName", bC.ParseQ(sFirstName)); // mandatory
                if (sMiddleName != string.Empty) { hshTable.Add("@chvMiddleName", sMiddleName); }

                if (sLastName != string.Empty && sLastName != "0")
                {
                    hshTable.Add("@chvLastName", sLastName);
                }

                if (sDisplayName != string.Empty && sDisplayName != "0") { hshTable.Add("@chvDisplayName", sDisplayName); }

                if (FormatDateYearMonthDate(sDOB) != string.Empty && FormatDateYearMonthDate(sDOB) != "0") { hshTable.Add("@chrDateofBirth", FormatDateYearMonthDate(sDOB)); }
                //hshTable.Add("@chrDateofBirth", FormatDateYearMonthDate(sDOB));
                if (sDOB != "__/__/____")
                {
                    imgCalYear_Click();
                }
                else
                {
                    if (sYear == string.Empty)
                        sYear = "0";
                    if (sMonth == string.Empty)
                        sMonth = "0";
                    if (sDay == string.Empty)
                        sDay = "0";
                }

                if (iEthnicityID != 0)
                {
                    hshTable.Add("@intEthnicityID", iEthnicityID);
                }
                if (bC.ParseQ(sYear) != string.Empty) { hshTable.Add("@inyAgeYear", bC.ParseQ(sYear)); }
                if (bC.ParseQ(sMonth) != string.Empty) { hshTable.Add("@inyAgeMonth", bC.ParseQ(sMonth)); }
                if (bC.ParseQ(sDay) != string.Empty) { hshTable.Add("@inyAgeDays", bC.ParseQ(sDay)); }
                if (iGender != string.Empty && iGender != "0") { hshTable.Add("@inySex", iGender); }
                if (iMaritalStatus != string.Empty && iMaritalStatus != "0") { hshTable.Add("@inyMaritalStatus", iMaritalStatus); }
                if (iReligionId != string.Empty && iReligionId != "0") { hshTable.Add("@inyReligionId", iReligionId); }
                if (sAddress != string.Empty && sAddress != "0") { hshTable.Add("@chvLocalAddress", sAddress); }

                if (sAddressLine2 != string.Empty && sAddressLine2 != "0") { hshTable.Add("@chvLocalAddressLine2", sAddressLine2); }


                if (sPin != string.Empty && sPin != "0") { hshTable.Add("@chvLocalPin", sPin); }
                if (iCity != string.Empty && iCity != "0") { hshTable.Add("@intLocalCity", iCity); }
                if (iState != string.Empty && iState != "0") { hshTable.Add("@intLocalState", iState); }
                if (iCountry != string.Empty && iCountry != "0") { hshTable.Add("@intLocalCountry", iCountry); }
                if (sEmail != string.Empty && sEmail != "0") { hshTable.Add("@chvEmail", sEmail); }
                if (sPhone != string.Empty && sPhone != "0") { hshTable.Add("@chvPhoneHome", sPhone); }
                if (sMobile != string.Empty && sMobile != "0") { hshTable.Add("@chvMobileNo", sMobile); }
                if (iNationality != string.Empty && iNationality != "0") { hshTable.Add("@intNationality", iNationality); }
                if (iReferralTypeID != 0) { hshTable.Add("@intReferralId", iReferralTypeID); }
                if (iIdentityTypeID != string.Empty && iIdentityTypeID != "0") { hshTable.Add("@intIdentityTypeID", iIdentityTypeID); }
                if (sIdentityTypeNumber != string.Empty && sIdentityTypeNumber != "0") { hshTable.Add("@chvIdentityNumber", sIdentityTypeNumber); }
                if (iSponsorId != string.Empty && iSponsorId != "0") { hshTable.Add("@intSponsorId", iSponsorId); }
                if (iPayerId != string.Empty && iPayerId != "0") { hshTable.Add("@intPayorId", iPayerId); }
                if (iFacility != string.Empty && iFacility != "0") { hshTable.Add("@intFacility", iFacility); }

                if (sTaggedEmpNo != string.Empty) { hshTable.Add("@sTaggedEmpNo", sTaggedEmpNo); }
                if (sTaggedEmpName != string.Empty) { hshTable.Add("@sTaggedEmpName", sTaggedEmpName); }


                hshTable.Add("@bitResponsibleSelf", iResposibleSelf); // 0 mandatory 
                if (iRenderingProvider != string.Empty && iRenderingProvider != "0") { hshTable.Add("@intRenderingProvider", iRenderingProvider); }
                if (iPCP != string.Empty && iPCP != "0") { hshTable.Add("@intPCP", iPCP); }
                if (iDefPharmacy != string.Empty && iDefPharmacy != "0") { hshTable.Add("@intDefaultPharmacyID", iDefPharmacy); }

                if (iRaceID != string.Empty && iRaceID != "0") { hshTable.Add("@intRaceID", iRaceID); }
                if (iReferralByID != 0) { hshTable.Add("@intReferredByID", iReferralByID); }
                if (sStatus != string.Empty && sStatus != "0") { hshTable.Add("@chrStatus", sStatus); }
                if (iDefLabID != string.Empty && iDefLabID != "0") { hshTable.Add("@intDefLabID", iDefLabID); }
                if (iDefImageCenterID != string.Empty && iDefImageCenterID != "0") { hshTable.Add("@intDefImageCenterID", iDefImageCenterID); }
                if (sResidentType != string.Empty && sResidentType != "0") { hshTable.Add("@chrResidentType", sResidentType); }
                if (sSSN != string.Empty && sSSN != "0") { hshTable.Add("@chvSocialSecurityNo", sSSN); }
                if (iLanguageID != 0) { hshTable.Add("@intLanguageId", iLanguageID); }
                if (iLeadSourceID != 0) { hshTable.Add("@intLeadSourceId", iLeadSourceID); }
                if (bAgeIdentification != false) { hshTable.Add("@bitAgeIdentification", bAgeIdentification); }

                if (iRegistrationId == 0)
                {
                    hshTableOut.Add("@intRegistrationNo", SqlDbType.Int);
                    hshTableOut.Add("@intRegistrationId", SqlDbType.Int);
                    hshTableOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveRegistration", hshTable, hshTableOut);
                    return hshTableOut;
                }
                else
                {
                    hshTable.Add("@intRegistrationNo", iRegistrationNo);
                    hshTable.Add("@intRegistrationId", iRegistrationId);
                    int iCount = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPUpdateRegistration", hshTable);
                    return hshTableOut;
                }

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Calculate Age from Date Of Birth
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public void imgCalYear_Click()
        {
            if (sDOB != "__/__/____")
            {
                sDOB = FormatDateDateMonthYear(sDOB);
                sDOB = FormatDateMDY(sDOB);
                sYear = string.Empty;
                sMonth = string.Empty;
                sDay = string.Empty;
                string[] result = CalculateAge(sDOB);

                //sYear = result[0];
                //sMonth = result[1];
                //sDay = result[2];

                if (result.Length == 2)
                {
                    if (result[1].ToUpper() == "YEAR")
                    {
                        sYear = result[0];
                    }
                    else
                    {
                        sDay = result[0];
                    }
                }

                if (result.Length == 4)
                {
                    sMonth = result[0];
                    sDay = result[2];
                }
                if (result.Length == 6)
                {
                    sYear = result[0];
                    sMonth = result[2];
                    sDay = result[4];
                }

                //txtYear.Text = result[0];
                //txtMonth.Text = result[1];
                //txtDays.Text = result[2];
                //if (txtYear.Text == "")
                //{
                //    txtYear.Text = "0";
                //}
                //if (txtMonth.Text == "")
                //{
                //    txtMonth.Text = "0";
                //}
                //if (txtDays.Text == "")
                //{
                //    txtDays.Text = "0";
                //}

                if (sYear == string.Empty || sYear == null)
                {
                    sYear = "0";
                }
                if (sMonth == string.Empty || sMonth == null)
                {
                    sMonth = "0";
                }
                if (sDay == string.Empty || sDay == null)
                {
                    sDay = "0";
                }
            }
            //}
        }

        public DataSet GetPatientRecord(int intRegId, int FacilityId, int intHospiLocId)
        {
            Hashtable hshTable = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder strSQ = new StringBuilder();

            try
            {


                hshTable.Add("@intRegistrationId", intRegId);
                hshTable.Add("@intFacilityId", FacilityId);
                //BaseC.Patient objBc = new BaseC.Patient(sConString);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationDetail", hshTable);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose      : To Get Patient Forms
        //   Author       : Rafat Anwer
        //   Date         : 15 June 2010
        //   Used in Page : Find Patient
        //  --------------------------------------------------------------------------
        public Int16 GetPatientDefaultFormId(int intEncounterId, int intHospiLocId)
        {
            Hashtable hshTable = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            StringBuilder strSQ = new StringBuilder();
            DataSet objDs = null;
            DataTable objDt = null;

            try
            {


                Int16 intFormId = 0;
                if (intEncounterId != 0 && intHospiLocId != 0)
                {

                    hshTable.Add("@inyHospitalLocationId", intHospiLocId);
                    hshTable.Add("@intEncounterId", intEncounterId);
                    objDs = objDl.FillDataSet(CommandType.StoredProcedure, "USPEMRGetPatientForms", hshTable);

                    if (objDs.Tables[0].Rows.Count == 0)
                    {
                        intFormId = 0;
                    }
                    else if (objDs.Tables[0].Rows.Count == 1)
                    {
                        intFormId = Convert.ToInt16(objDs.Tables[0].Rows[0]["FormId"]);
                    }
                    else
                    {

                        DataView objDv = new DataView(objDs.Tables[0]);
                        objDv.RowFilter = "DefaultForVisit = 1";
                        objDt = objDv.ToTable();
                        if (objDt.Rows.Count > 0)
                        {
                            intFormId = Convert.ToInt16(objDt.Rows[0]["FormId"]);
                        }
                        else
                        {
                            intFormId = Convert.ToInt16(objDs.Tables[0].Rows[0]["FormId"]);
                        }
                    }
                }
                return intFormId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; objDs.Dispose(); }
        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get Patient Title
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientTitle()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet objDs = objDl.FillDataSet(CommandType.Text, "select '' as id,'[Select]' TextField,'' as IsNewBornTitle, '' AS Gender UNION ALL select TitleId as id, Name as TextField,IsNewBornTitle,Gender from TitleMaster WITH (NOLOCK)  Where Active=1 order By TextField");
            return objDs;
        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get City List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientCity()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select CityMaster.cityID, CityMaster.cityname,StateID from CityMaster WITH (NOLOCK) where active=1 ORDER BY CityMaster.cityname");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get State List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientState()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select StateMaster.StateID, StateMaster.StateName, StateMaster.CountryId from StateMaster WITH (NOLOCK) where active=1 ORDER BY StateName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get Country List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientCountry()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select 0 CountryId, '[ Select ]' CountryName, 30 MobileLength UNION ALL SELECT distinct cm.CountryId, cm.CountryName, cm.MobileLength FROM CountryMaster cm WITH (NOLOCK) where cm.Active =1 ORDER BY CountryName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get Nationality List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientNationality()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,nationality_name as Name from nationalitymaster WITH (NOLOCK) where active=1 order by nationality_name ");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get Religion List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientReligion()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Name from religionmaster WITH (NOLOCK) where active=1 ");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get Occupation List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientOccupation()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "SELECT Id, Name FROM OccupationMaster WITH (NOLOCK) WHERE (Active = '1') ORDER BY Name");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //----------------------------------------------------------------------------
        //   Purpose           :To Get Referred Type sponsor
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        public DataSet GetPatientReferredType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "SELECT ID, ReferralTypeName,ReferralType FROM ReferTypeMaster WITH (NOLOCK) ORDER BY ReferralTypeName");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        //----------------------------------------------------------------------------
        //   Purpose           :To Get Field name to hide 
        //   Author            :Rajeev kumar
        //  --------------------------------------------------------------------------
        public DataSet GetHideFieldType(int intModuleId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@iModuleId", intModuleId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEmrGetFieldDisplayPermission", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public SqlDataReader getPatientalert(int iRegistrationno, int iHospitalLocationId)
        {
            Hashtable HshIn = new Hashtable();

            StringBuilder objStr = new StringBuilder();
            objStr.Append("select top 1 'Alert-' +  ISNULL(' ' + Message,'') as Message from EMRPatientAlerts WITH (NOLOCK) where Registrationno = @RegistrationNo order by ID desc ");
            //objStr.Append(" from Patient_registration rg where rg.Hosp_Location_Id = @HospitalLocationId AND rg.Registration_no = @RegistrationNo");
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("RegistrationNo", iRegistrationno);
            // HshIn.Add("HospitalLocationId", iHospitalLocationId);
            try
            {

                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, objStr.ToString(), HshIn);

                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Get Sponsor List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //public DataSet GetPatientSponsor()
        //{
        //    if (iHospitalLocationId != 0)
        //    {
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        DataSet objDs = objDl.FillDataSet(CommandType.Text, "select c.name,c.companyID from company C " + " inner join companystatus cs on c.companyID=cs.CompanyId " + " where cs.Active='1' and c.Paymenttype='C' and HospitalLocationId=" + iHospitalLocationId + " and id=(select max(id) from companystatus cs1 where cs1.companyID=cs.CompanyId)" + " order by c.name");
        //        return objDs;
        //    }
        //    return null;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Get Payer List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //public DataSet GetPatientPayer()
        //{
        //    if (iHospitalLocationId != 0)
        //    {
        //        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //        DataSet objDs = objDl.FillDataSet(CommandType.Text, "select c.name,c.companyID from company C " + " inner join companystatus cs on c.companyID=cs.CompanyId " + " where cs.Active='1' and c.Paymenttype='C' and HospitalLocationId=" + iHospitalLocationId + " and id=(select max(id) from companystatus cs1 where cs1.companyID=cs.CompanyId)" + " order by c.name");
        //        return objDs;
        //    }
        //    return null;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Get Other Allergies List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //public DataSet GetPatientOtherAllergies()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, "SELECT ID, name FROM ALLERGYMASTER WHERE (AllergyType = 'O') order by name");
        //    return objDs;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Get Food Allergies List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //public DataSet GetPatientFoodAllergies()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, "SELECT ID, name FROM ALLERGYMASTER WHERE (AllergyType = 'F') order by name");
        //    return objDs;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Get Drug Allergies List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //public DataSet GetPatientDrugAllergies()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, "SELECT ID, name FROM ALLERGYMASTER WHERE (AllergyType = 'D') order by name");
        //    return objDs;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Get KinRelation List
        //   Author            :Madhukar Jha
        //  --------------------------------------------------------------------------
        //public DataSet GetPatientKinRelation()
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    DataSet objDs = objDl.FillDataSet(CommandType.Text, "select 0 kinID,'[Select]' KinName union all select kinID, KinName from kinrelation order by KinName");
        //    return objDs;
        //}
        //Not in Use
        //----------------------------------------------------------------------------
        //   Purpose           :To Convert Date into MM/dd/yyyy format
        //   Input parameters  :string
        //   Output Parameters :string
        //   Author            :Robin Simon
        //  --------------------------------------------------------------------------
        //public string FormatDateMonthDateYear(string strDateOfBirth)
        //{
        //    if (strDateOfBirth != "" && strDateOfBirth != "__/__/____")
        //    {
        //        //input string should be in dd/MM/yyyy
        //        char[] chSperator = { '/' };
        //        string[] strResult = strDateOfBirth.Split(chSperator);
        //        StringBuilder str = new StringBuilder();

        //        if (strResult[1].Length == 1)
        //            str.Append("0" + strResult[1]);
        //        else
        //            str.Append(strResult[1]);
        //        str.Append("/");
        //        if (strResult[0].Length == 1)
        //            str.Append("0" + strResult[0]);
        //        else
        //            str.Append(strResult[0]);
        //        str.Append("/");
        //        str.Append(strResult[2]);
        //        //output string is in MM/dd/yyyy format
        //        return str.ToString();
        //    }
        //    else
        //        return null;
        //}

        //----------------------------------------------------------------------------
        //   Purpose           :To save multiple patient Relation List
        //   Author            :Robin Simon
        //  --------------------------------------------------------------------------
        public String SaveRegistrationContact(Int32 iRegId, Int32 iUHID, Int16 iHospID, Int16 iContactGroupID, String sName, String sAddress, String sAddressLine2, String sPin, String sPhone, String sMob, String sEmail, Int32 iUser, Byte bEmerContact, Byte bResponsiblePerson, Byte bStatus, Int32 iIdenTypeID, String sIdenNumber, Int32 iUpdateID, String sDOB, Int32 FacilityId, Int32 PageId)
        {
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();

            HshIn.Add("@intRegistrationID", iRegId);
            HshIn.Add("@intRegistrationNo", iUHID);
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("@inyContactGroupID", iContactGroupID);
            HshIn.Add("@chvName", sName);
            HshIn.Add("@chvAddress", sAddress);
            HshIn.Add("@chvLocalAddressLine2", sAddressLine2);
            HshIn.Add("@chvPin", sPin);
            HshIn.Add("@chvPhone", sPhone);
            HshIn.Add("@chvMobile", sMob);
            HshIn.Add("@chvEmail", sEmail);
            HshIn.Add("@bitEmergencyContact", bEmerContact);
            HshIn.Add("@bitResposiblePerson", bResponsiblePerson);
            if (iIdenTypeID != 0) { HshIn.Add("@intIdentityTypeID", iIdenTypeID); }
            HshIn.Add("@chvIdentityNumber", sIdenNumber);
            HshIn.Add("@chvDOB", sDOB);
            HshIn.Add("@bitActive", bStatus);
            HshIn.Add("@intEncodedBy", iUser);
            HshIn.Add("@intUpdateID", iUpdateID);
            HshIn.Add("@intLoginFacilityId", FacilityId);
            HshIn.Add("@intPageId", PageId);
            hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateRegistrationContacts", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        //Not in Use
        //public DataSet GetRelation(Int32 iUHID)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable hshIn = new Hashtable();
        //    hshIn.Add("intUHID", iUHID);
        //    DataSet objDs = objDl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetGuardianRelation", hshIn);
        //    return objDs;
        //}
        //Not in use
        //public String UpdateRelation(Int32 iID, Int32 iUHID, Int16 iRelationID, String sName, String sAddress, Int32 iCtry, Int32 iState, Int32 iCity, String sPin, String sContact, String sMob, String sEmail, Byte bActive)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    Hashtable hshIn = new Hashtable();
        //    Hashtable hshOut = new Hashtable();
        //    hshIn.Add("intID", iID);
        //    hshIn.Add("intUHID", iUHID);
        //    hshIn.Add("inyRelationID", iRelationID);
        //    hshIn.Add("chvName", sName);
        //    hshIn.Add("chvAddress", sAddress);
        //    hshIn.Add("intCountry", iCtry);
        //    hshIn.Add("intState", iState);
        //    hshIn.Add("intCity", iCity);
        //    hshIn.Add("chvPin", sPin);
        //    hshIn.Add("chvContact", sContact);
        //    hshIn.Add("chvMobile", sMob);
        //    hshIn.Add("chvEmail", sEmail);
        //    hshIn.Add("bitActive", bActive);
        //    hshOut.Add("chvErrorStatus", SqlDbType.VarChar);
        //    hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspEMRUpdateGuardianRelation", hshIn, hshOut);
        //    return hshOut["chvErrorStatus"].ToString();
        //}

        public DataSet GetLanguage()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {

                return objDl.FillDataSet(CommandType.Text, "select LanguageID,Language from languagemaster WITH (NOLOCK) where Active=1 order by Language");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetIndustry()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Name from IndustryMaster WITH (NOLOCK) where Active=1 order by Name");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRace()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select RaceID,Race from racemaster WITH (NOLOCK) where Active=1 order by Race");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAppointmentNotification()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Method from appointmentnotification WITH (NOLOCK) where Active=1 order by Method");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetEmploymentStatus()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "Select StatusId, Status From GetStatus(1,'Employment')");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetStudentStatus()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                return objDl.FillDataSet(CommandType.Text, "Select StatusId, Status From GetStatus(1,'Student')");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAccountCategory()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select Id, Name,isnull(Flag,'') Flag from companytype WITH (NOLOCK) order by Name");

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAccountPlans()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select PlanId, PlanName from AccountPlans WITH (NOLOCK) order by PlanName");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAccountType(int intAccountCategoryId)
        {
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@iAccountCategoryId", intAccountCategoryId);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {

                return objDl.FillDataSet(CommandType.Text, "select TypeId, TypeName,DefaultCompanyId,DefaultPlanTypeId from AccountTypes WITH (NOLOCK) where AccountCategoryId = @iAccountCategoryId order by TypeName", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        public DataSet GetEthnicity()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Ethnicity from ethnicitymaster WITH (NOLOCK) where Active=1 order by Ethnicity");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public SqlDataReader GetRegistrationOtherDetails(Int32 iRegId, Int16 iHospID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", iRegId);
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.StoredProcedure, "UspGetRegistrationOtherDetails", HshIn);
                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetRegistrationExternalPatient(Int32 iRegId, Int16 iHospID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", iRegId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationOtherDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetHealthCareMaster(int HospitalLocation, int FacilityID, int active, int id)
        {
            DataTable dt = new DataTable();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@HospitalLocation", HospitalLocation);
                HshIn.Add("@active", active);
                HshIn.Add("@id", id);
                HshIn.Add("@facility", FacilityID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "GetHealthCareFacilitatorList", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveRegistrationOtherDetails()
        {
            BaseC.ParseData bC = new BaseC.ParseData();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshOut = new Hashtable();

            try
            {


                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intRegistrationNo", iRegistrationNo);
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationId);
                HshIn.Add("@chvWorkAddress1", sWorkAddress);
                HshIn.Add("@chvWorkAddress2", sWorkAddress2);
                if (iWorkCountry != string.Empty && iWorkCountry != "0") { HshIn.Add("@intCountryID", iWorkCountry); }
                if (iWorkState != string.Empty && iWorkState != "0") { HshIn.Add("@intStateID", iWorkState); }
                if (iWorkCity != string.Empty && iWorkCity != "0") { HshIn.Add("@intCityID", iWorkCity); }
                if (sWorkPin != string.Empty && sWorkPin != "0") { HshIn.Add("@chvWorkPin", sWorkPin); }
                //  HshIn.Add("@chvWorkPin", sWorkPin);
                if (sWorkNumber != string.Empty && sWorkNumber != "0")
                {
                    HshIn.Add("@chvWorkNumber", sWorkNumber);
                }
                HshIn.Add("@chvWorkExtn", sWorkExtn);
                HshIn.Add("@chvWorkEmail", sWorkEmail);
                if (iConfidentialComm != 0)
                {
                    HshIn.Add("@intConfidentialComm", iConfidentialComm);
                }
                HshIn.Add("@intVFCProviderID", iVFCProviderID);
                HshIn.Add("@chvLockRecord", sLockRecord);
                HshIn.Add("@bitVIP", VIPByte);
                HshIn.Add("@chvVIPNarration", sVIPNarration);
                HshIn.Add("@bitNewBorn", bNewBorn);
                HshIn.Add("@inyBirthOrder", iBirthOrder);
                HshIn.Add("@bitRegistryExclude", bRegistryExclude);
                HshIn.Add("@intEnableAllert", iEnableAllert);
                HshIn.Add("@chvNote", sSpecialNote);
                HshIn.Add("@chvAppointmentNotification", sAppointmentNotification);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshIn.Add("@bitRemovePassword", sRemovePassword);
                if (iEmploymentStatusID != 0 && iEmploymentStatusID != null)
                {
                    HshIn.Add("@intEmploymentStatusID", iEmploymentStatusID);
                }
                HshIn.Add("@chvEmployerName", sEmployerName);
                HshIn.Add("@chvEmployeeNumber", sEmployeeNo);
                if (iStudentStatusID != 0 && iStudentStatusID != null)
                {
                    HshIn.Add("@intStudentStatusID", iStudentStatusID);
                }
                HshIn.Add("@chvSchoolCollege", sSchoolCollege);
                HshIn.Add("@chrDateOfDeath", dtDeathdeth);
                HshIn.Add("@chvCauseOfDeath", chvCuaseofdeth);
                HshIn.Add("@chrNotificationPhoneNo", sNotificationphone);
                HshIn.Add("@chrNotificationPhoneType", sNotificationType);
                if (iOccupationId != 0 && iOccupationId != null)
                {
                    HshIn.Add("@intOccupationId", iOccupationId);
                }
                if (iShowNoteInAppointment != 0 && iShowNoteInAppointment != null)
                {
                    HshIn.Add("@bitShowNoteInAppointment", iShowNoteInAppointment);
                }
                HshIn.Add("@ExternalPatient", ExternalPatient);

                HshIn.Add("@intLoginFacilityId", FacilityId);
                HshIn.Add("@intPageId", PageId);
                HshIn.Add("@chvPrivateKey", sPrivateKey);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpdateRegistrationOtherDetails", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientIdentityType()
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.FillDataSet(CommandType.Text, "select ID,Description from PatientIdentityType WITH (NOLOCK) where Active=1 order by Description");
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetContactGroup(Int32 iRegistrationId, Int16 iHospitalLocationID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@inyHospitalLocationID", iHospitalLocationID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetContactGroup", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetRegistrationContacts(Int16 iHospID, Int16 iGroupID, Int32 iRegID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {

                HshIn.Add("inyHospitalLocationID", iHospID);
                HshIn.Add("inyGroupID", iGroupID);
                HshIn.Add("intRegistrationID", iRegID);
                //HshIn.Add("intRegID", iRegID);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationContacts", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet GetRegistrationContacts(Int16 iHospID, Int16 iGroupID, Int32 iRegID, Int16 iID)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationID", iHospID);
            HshIn.Add("intRegistrationID", iRegID);
            HshIn.Add("inyGroupID", iGroupID);
            HshIn.Add("intID", iID);
            try
            {


                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationContacts", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public String SaveContactGroup(String SGroupName, Int32 iEncodedBy, Int32 iHospId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            Hashtable hshOut = new Hashtable();
            HshIn.Add("chvGroupName", SGroupName);
            HshIn.Add("intEncodedBy", iEncodedBy);
            HshIn.Add("@inyHospitalLocationId", iHospId);
            hshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            try
            {

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveContactGroup", HshIn, hshOut);
                return hshOut["chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public String GetPatientRegistrationNo(int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            DataSet ds = new DataSet();
            HshIn.Add("@intRegistrationId", RegistrationId);

            try
            {

                ds = objDl.FillDataSet(CommandType.Text, "Select RegistrationNo from registration WITH (NOLOCK) where Id = @intRegistrationId", HshIn);

                string strRegNo = string.Empty;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strRegNo = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                }
                return strRegNo;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }


        }

        public string GetValidPatientEmployeeTagging(string sEmpNo)
        {
            string sEmpName = string.Empty;
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@chvEmpNo", sEmpNo);
                HshIn.Add("@intFacilityId", FacilityId);
                string sQuery = "SELECT EMP.EmployeeNo, E.F_Name + ISNULL(' ' +E.M_Name,'') + ISNULL(' ' +E.L_Name,'') EmpName FROM Pr_Emp_Personal E WITH (NOLOCK) INNER JOIN Pr_Emp_Professional P WITH (NOLOCK) ON E.Emp_No = P.Emp_no INNER JOIN Employee EMP WITH (NOLOCK) ON E.Emp_No=EMP.ID AND P.Dos IS NULL AND P.Status = 'A' AND EMP.Active = 1  WHERE EMP.EmployeeNo =@chvEmpNo";
                ds = objDl.FillDataSet(CommandType.Text, sQuery, HshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sEmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                return sEmpName;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }
        }
        public string GetValidPatientEmployeeTaggingNew(string sEmpNo, int FacilityId)
        {
            string sEmpName = string.Empty;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            DataSet ds = new DataSet();
            try
            {
                hshIn.Add("@chvEmpNo", sEmpNo);
                hshIn.Add("@intFacilityId", FacilityId);
                // string sQuery = "SELECT EMP.EmployeeNo, E.F_Name + ISNULL(' ' +E.M_Name,'') + ISNULL(' ' +E.L_Name,'') EmpName FROM Pr_Emp_Personal E WITH (NOLOCK) INNER JOIN Pr_Emp_Professional P WITH (NOLOCK) ON E.Emp_No = P.Emp_no INNER JOIN Employee EMP WITH (NOLOCK) ON E.Emp_No=EMP.ID AND P.Dos IS NULL AND P.Status = 'A' AND EMP.Active = 1  WHERE EMP.EmployeeNo =@chvEmpNo";
                // string sQuery = "SELECT EMP.EmployeeNo, E.F_Name + ISNULL(' ' +E.M_Name,'') + ISNULL(' ' +E.L_Name,'') EmpName FROM Pr_Emp_Personal E WITH (NOLOCK) INNER JOIN Pr_Emp_Professional P WITH (NOLOCK) ON E.Emp_No = P.Emp_no INNER JOIN Employee EMP WITH (NOLOCK) ON E.Emp_No=EMP.ID AND P.Dos IS NULL AND P.Status = 'A'  AND EMP.Active = 1 INNER JOIN Users ur  WITH (NOLOCK) ON EMP.ID=ur.EmpID INNER JOIN SecUserFacility sf  WITH (NOLOCK) ON ur.ID=sf.UserId  And sf.Active=1    WHERE EMP.EmployeeNo =@chvEmpNo    And sf.FacilityId = @intFacilityId";
                string sQuery = "SELECT E.EmployeeNo, E.FirstName + ISNULL(' ' +E.MiddleName,'') + ISNULL(' ' +E.LastName,'') EmpName FROM  Employee E INNER JOIN Users ur  WITH (NOLOCK) ON E.ID=ur.EmpID INNER JOIN SecUserFacility sf  WITH (NOLOCK) ON ur.ID=sf.UserId  WHERE E.EmployeeNo =@chvEmpNo    And sf.FacilityId = @intFacilityId";
                ds = objDl.FillDataSet(CommandType.Text, sQuery, hshIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    sEmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                }
                return sEmpName;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; ds.Dispose(); }

        }
        public DataSet GetEmployeeTagging(string sEmpNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@EmpNo", sEmpNo);

            try
            {
                return objDl.FillDataSet(CommandType.Text, "select E.Emp_No, E.F_Name + ISNULL(' ' +E.M_Name,'') + ISNULL(' ' +E.L_Name,'') EmpName, case E.Sex  when 'F' then 'Female' when 'M' then 'Male'	else 'Unknown' end + '/' + dbo.AgeInYrsMonthDay(CONVERT(VARCHAR,E.Dob,111),CONVERT(VARCHAR,GETDATE(),111)) AS GenderAge,case E.Sex  when 'F' then 1  when 'M' then 2 else 4 end as Gender FROM Pr_Emp_Personal E WITH (NOLOCK) INNER JOIN Pr_Emp_Professional P WITH (NOLOCK) ON E.Emp_No = P.Emp_no AND P.Dos IS NULL AND P.Status = 'A' WHERE E.Emp_No = @EmpNo", hshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }


        /*--------------------------------------------------------------------------
   Purpose           :To Get Time Zone 
   Author            :Manmohan
  --------------------------------------------------------------------------*/
        public struct MyStruct
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public double TimeZoneMinutes { get; set; }
        }

        /*--------------------------------------------------------------------------
           Purpose           :To Get Time Zone 
           Author            :Manmohan
          --------------------------------------------------------------------------*/

        public Collection<MyStruct> GetTimeZones(string timeZoneID)
        {
            var myClass = new Collection<MyStruct>();

            if (timeZoneID == string.Empty && timeZoneID != "0")
            {
                myClass.Add(new MyStruct { ID = "0", Name = "[Select]", TimeZoneMinutes = 0 });
            }
            foreach (var timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {
                if (timeZoneID != string.Empty && timeZoneID != "0")
                {
                    if (timeZoneID == timeZoneInfo.Id.ToString())
                    {
                        myClass.Add(new MyStruct { ID = timeZoneInfo.Id.ToString(), Name = timeZoneInfo.DisplayName, TimeZoneMinutes = timeZoneInfo.BaseUtcOffset.TotalMinutes });
                    }
                    continue;
                }

                myClass.Add(new MyStruct { ID = timeZoneInfo.Id.ToString(), Name = timeZoneInfo.DisplayName, TimeZoneMinutes = timeZoneInfo.BaseUtcOffset.TotalMinutes });
            }
            return myClass;
        }
        //public string getRegistrationIDFromRegistrationNo(string RegistrationNo)
        //{
        //    Hashtable hshInput;
        //    int intRegistrationId = 0;
        //    try
        //    {               
        //        if (RegistrationNo != "")
        //        {
        //            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //            hshInput = new Hashtable();
        //            hshInput.Add("@RegistrationNo", RegistrationNo);
        //            intRegistrationId = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "select Id from Registration where RegistrationNo = @RegistrationNo ", hshInput));
        //            return intRegistrationId.ToString();
        //        }
        //        else
        //            return intRegistrationId.ToString();
        //    }
        //    catch (Exception ex)
        //    {                
        //        return intRegistrationId.ToString();
        //    }
        //}

        // Add 05112015

        public DataSet getPatientDetailsByMobileNo(string MobileNo, int HospitalLocation)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();

            hshInput.Add("MobileNo", MobileNo);
            hshInput.Add("HospitalLocationid", HospitalLocation);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPatientRecordByMobileNo", hshInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public string getRegistrationIDFromRegistrationNo(string RegistrationNo, int HospitalLocId)
        {
            Hashtable HshIn;
            int intRegistrationId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                if (RegistrationNo != string.Empty)
                {
                    HshIn = new Hashtable();
                    HshIn.Add("@RegistrationNo", RegistrationNo);
                    HshIn.Add("@HospitalLocationID", HospitalLocId);

                    intRegistrationId = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "select Id from Registration WITH (NOLOCK) where RegistrationNo = @RegistrationNo AND HospitalLocationId= @HospitalLocationID ", HshIn));
                    return intRegistrationId.ToString();
                }
                else
                    return intRegistrationId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public string getRegistrationIDFromRegistrationNo(int RegistrationNo, int HospitalLocId, int FacilityId)
        {
            Hashtable HshIn;
            int intRegistrationId = 0;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                if (RegistrationNo > 0)
                {

                    HshIn = new Hashtable();
                    HshIn.Add("@RegistrationNo", RegistrationNo);
                    HshIn.Add("@HospitalLocationID", HospitalLocId);
                    HshIn.Add("@FacilityId", FacilityId);

                    intRegistrationId = Convert.ToInt32(objDl.ExecuteScalar(CommandType.Text, "select Id from Registration WITH (NOLOCK) where RegistrationNo = @RegistrationNo  AND FacilityId = @FacilityId AND HospitalLocationId= @HospitalLocationID", HshIn));
                    return intRegistrationId.ToString();
                }
                else
                    return intRegistrationId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally { HshIn = null; objDl = null; }

        }


        public Hashtable SaveUpdateRegistration(int iHospitalLocationId, int FacilityId, int PageId, string sPreviousName, int iTitle, string sFirstName, string sMiddleName, string sLastName,
          string sDisplayName, int iEthnicityID, string sYear, string sMonth, string sDay, int iGender, int iMaritalStatus, int iReligionId, string sAddress, string sAddressLine2, string sPin,
           int iCity, int iState, int iCountry, string sEmail, string sPhone, string sMobile, int iNationality, string iReferralTypeID, int iIdentityTypeID, string sIdentityTypeNumber,
           int iSponsorId, int iPayerId, int iFacility, int iResposibleSelf, int iRenderingProvider, int iPCP, int iDefPharmacy, int iRaceID, int iReferralByID, string sStatus,
           int iDefLabID, int iDefImageCenterID, string sResidentType, string sSSN, bool bExternalPatient, int iLeadSourceID, int iEncodedBy, int iRegistrationId, int iRegistrationNo,
           string DOB, string sAppointmentID, int iCityAreaId, string sTaggedEmpNo, string sTaggedEmpName, int MotherRegistrationNo,
           string chvVIPNarration, string emergencyContactName, bool bitVIP, string emergencyContactNo,
           int isERRegistration, string Checklist, string EmiratesID, bool bAgeIdentification, int EntrySite, int OccupationId, int HealthcareID, string MotherName, string FatherName, decimal FamilyMonthlyIncome)
        {
            return SaveUpdateRegistration(iHospitalLocationId, FacilityId, PageId, sPreviousName, iTitle, sFirstName, sMiddleName, sLastName,
          sDisplayName, iEthnicityID, sYear, sMonth, sDay, iGender, iMaritalStatus, iReligionId, sAddress, sAddressLine2, sPin,
           iCity, iState, iCountry, sEmail, sPhone, sMobile, iNationality, iReferralTypeID, iIdentityTypeID, sIdentityTypeNumber,
           iSponsorId, iPayerId, iFacility, iResposibleSelf, iRenderingProvider, iPCP, iDefPharmacy, iRaceID, iReferralByID, sStatus,
           iDefLabID, iDefImageCenterID, sResidentType, sSSN, bExternalPatient, iLeadSourceID, iEncodedBy, iRegistrationId, iRegistrationNo,
           DOB, sAppointmentID, iCityAreaId, sTaggedEmpNo, sTaggedEmpName, MotherRegistrationNo,
           chvVIPNarration, emergencyContactName, bitVIP, emergencyContactNo,
           isERRegistration, Checklist, EmiratesID, bAgeIdentification, EntrySite, OccupationId, HealthcareID, MotherName, 0, FatherName, FamilyMonthlyIncome);
        }
        public Hashtable SaveUpdateRegistration(int iHospitalLocationId, int FacilityId, int PageId, string sPreviousName, int iTitle, string sFirstName, string sMiddleName, string sLastName,
          string sDisplayName, int iEthnicityID, string sYear, string sMonth, string sDay, int iGender, int iMaritalStatus, int iReligionId, string sAddress, string sAddressLine2, string sPin,
           int iCity, int iState, int iCountry, string sEmail, string sPhone, string sMobile, int iNationality, string iReferralTypeID, int iIdentityTypeID, string sIdentityTypeNumber,
           int iSponsorId, int iPayerId, int iFacility, int iResposibleSelf, int iRenderingProvider, int iPCP, int iDefPharmacy, int iRaceID, int iReferralByID, string sStatus,
           int iDefLabID, int iDefImageCenterID, string sResidentType, string sSSN, bool bExternalPatient, int iLeadSourceID, int iEncodedBy, int iRegistrationId, int iRegistrationNo,
           string DOB, string sAppointmentID, int iCityAreaId, string sTaggedEmpNo, string sTaggedEmpName, int MotherRegistrationNo,
           string chvVIPNarration, string emergencyContactName, bool bitVIP, string emergencyContactNo,
           int isERRegistration, string Checklist, string EmiratesID, bool bAgeIdentification, int EntrySite, int OccupationId, int HealthcareID, string MotherName, int ResourceAppointmentId, string FatherName, decimal FamilyMonthlyIncome)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable houtPara = new Hashtable();
            Hashtable hstInput = new Hashtable();

            hstInput.Add("@inyHospitalLocationId", iHospitalLocationId);
            hstInput.Add("@intEncodedBy", iEncodedBy); // mandatory
            hstInput.Add("@intLoginFacilityId", FacilityId);
            hstInput.Add("@intPageId", PageId);
            if (sPreviousName != string.Empty) { hstInput.Add("chvPreviousName", sPreviousName); }
            if (Convert.ToString(iTitle) != string.Empty && Convert.ToString(iTitle) != "0") { hstInput.Add("@intTitle", iTitle); }

            hstInput.Add("@chvFirstName", sFirstName); // mandatory
            if (sMiddleName != string.Empty) { hstInput.Add("@chvMiddleName", sMiddleName); }

            if (sLastName != string.Empty && sLastName != "0")
            {
                hstInput.Add("@chvLastName", sLastName);
            }

            if (sDisplayName != string.Empty && sDisplayName != "0") { hstInput.Add("@chvDisplayName", sDisplayName); }

            //if (FormatDateYearMonthDate(sDOB) != "" && FormatDateYearMonthDate(sDOB) != "0") { hstInput.Add("@chrDateofBirth", FormatDateYearMonthDate(sDOB)); }
            hstInput.Add("@chrDateofBirth", DOB);


            if (iEthnicityID != 0)
            {
                hstInput.Add("@intEthnicityID", iEthnicityID);
            }
            if (Convert.ToString(sYear) != string.Empty) { hstInput.Add("@inyAgeYear", sYear); }
            if (Convert.ToString(sMonth) != string.Empty) { hstInput.Add("@inyAgeMonth", sMonth); }
            if (Convert.ToString(sDay) != string.Empty) { hstInput.Add("@inyAgeDays", sDay); }
            if (Convert.ToString(iGender) != string.Empty && Convert.ToString(iGender) != "0") { hstInput.Add("@inySex", iGender); }
            if (Convert.ToString(iMaritalStatus) != string.Empty && Convert.ToString(iMaritalStatus) != "0") { hstInput.Add("@inyMaritalStatus", iMaritalStatus); }
            if (Convert.ToString(iReligionId) != string.Empty && Convert.ToString(iReligionId) != "0") { hstInput.Add("@inyReligionId", iReligionId); }
            if (sAddress != string.Empty && sAddress != "0") { hstInput.Add("@chvLocalAddress", sAddress); }

            if (sAddressLine2 != string.Empty && sAddressLine2 != "0") { hstInput.Add("@chvLocalAddressLine2", sAddressLine2); }


            if (sPin != string.Empty && sPin != "0") { hstInput.Add("@chvLocalPin", sPin); }
            if (Convert.ToString(iCity) != string.Empty && Convert.ToString(iCity) != "0") { hstInput.Add("@intLocalCity", iCity); }
            if (Convert.ToString(iState) != string.Empty && Convert.ToString(iState) != "0") { hstInput.Add("@intLocalState", iState); }
            if (Convert.ToString(iCountry) != string.Empty && Convert.ToString(iCountry) != "0") { hstInput.Add("@intLocalCountry", iCountry); }
            if (sEmail != string.Empty && sEmail != "0") { hstInput.Add("@chvEmail", sEmail); }
            if (sPhone != string.Empty && sPhone != "0") { hstInput.Add("@chvPhoneHome", sPhone); }
            if (sMobile != string.Empty && sMobile != "0") { hstInput.Add("@chvMobileNo", sMobile); }
            if (Convert.ToString(iNationality) != string.Empty && Convert.ToString(iNationality) != "0") { hstInput.Add("@intNationality", iNationality); }
            if (iReferralTypeID != string.Empty) { hstInput.Add("@intReferralId", iReferralTypeID); }
            if (Convert.ToString(iIdentityTypeID) != string.Empty && Convert.ToString(iIdentityTypeID) != "0") { hstInput.Add("@intIdentityTypeID", iIdentityTypeID); }
            if (sIdentityTypeNumber != string.Empty && sIdentityTypeNumber != "0") { hstInput.Add("@chvIdentityNumber", sIdentityTypeNumber); }
            if (Convert.ToString(iSponsorId) != string.Empty && Convert.ToString(iSponsorId) != "0") { hstInput.Add("@intSponsorId", iSponsorId); }
            if (Convert.ToString(iPayerId) != string.Empty && Convert.ToString(iPayerId) != "0") { hstInput.Add("@intPayorId", iPayerId); }
            if (Convert.ToString(iFacility) != string.Empty && Convert.ToString(iFacility) != "0") { hstInput.Add("@intFacility", iFacility); }

            hstInput.Add("@bitResponsibleSelf", iResposibleSelf); // 0 mandatory 
            if (Convert.ToString(iRenderingProvider) != string.Empty && Convert.ToString(iRenderingProvider) != "0") { hstInput.Add("@intRenderingProvider", iRenderingProvider); }
            if (Convert.ToString(iPCP) != string.Empty && Convert.ToString(iPCP) != "0") { hstInput.Add("@intPCP", iPCP); }
            if (Convert.ToString(iDefPharmacy) != string.Empty && Convert.ToString(iDefPharmacy) != "0") { hstInput.Add("@intDefaultPharmacyID", iDefPharmacy); }

            if (Convert.ToString(iRaceID) != string.Empty && Convert.ToString(iRaceID) != "0") { hstInput.Add("@intRaceID", iRaceID); }
            if (iReferralByID != 0) { hstInput.Add("@intReferredByID", iReferralByID); }
            if (sStatus != string.Empty && sStatus != "0") { hstInput.Add("@chrStatus", sStatus); }
            if (Convert.ToString(iDefLabID) != string.Empty && Convert.ToString(iDefLabID) != "0") { hstInput.Add("@intDefLabID", iDefLabID); }
            if (Convert.ToString(iDefImageCenterID) != string.Empty && Convert.ToString(iDefImageCenterID) != "0") { hstInput.Add("@intDefImageCenterID", iDefImageCenterID); }
            if (sResidentType != string.Empty && sResidentType != "0") { hstInput.Add("@chrResidentType", sResidentType); }
            if (sSSN != string.Empty && sSSN != "0") { hstInput.Add("@chvSocialSecurityNo", sSSN); }
            hstInput.Add("@bitExternalPatient", bExternalPatient);
            if (iLeadSourceID != 0) { hstInput.Add("@intLeadSourceId", iLeadSourceID); }
            hstInput.Add("@intCityAreaId", iCityAreaId);

            if (sTaggedEmpNo != string.Empty) { hstInput.Add("@sTaggedEmpNo", sTaggedEmpNo); }
            if (sTaggedEmpName != string.Empty) { hstInput.Add("@sTaggedEmpName", sTaggedEmpName); }
            hstInput.Add("@motherRegistrationNo", MotherRegistrationNo);

            hstInput.Add("@emergencyContactName", emergencyContactName);
            hstInput.Add("@emergencyContactNo", emergencyContactNo);
            hstInput.Add("@bitVIP", bitVIP);
            hstInput.Add("@chvVIPNarration", chvVIPNarration);
            hstInput.Add("@isERRegistration", isERRegistration);
            hstInput.Add("@XMLChecklist", Checklist);
            hstInput.Add("@chvEmiratesID", EmiratesID);
            hstInput.Add("@bitAgeIdentification", bAgeIdentification);
            hstInput.Add("@intEntrySite", EntrySite);
            hstInput.Add("@intOccupationId ", OccupationId);
            hstInput.Add("@intHealthcareID ", HealthcareID);
            hstInput.Add("@MotherName", MotherName);
            hstInput.Add("@FatherName", FatherName);
            hstInput.Add("@FamilyMonthlyIncome", FamilyMonthlyIncome);

            if (iRegistrationId == 0)
            {

                hstInput.Add("@intAppointmentId", sAppointmentID);
                hstInput.Add("@ResourceAppointmentId", ResourceAppointmentId);
                houtPara.Add("@intRegistrationNo", SqlDbType.BigInt);
                houtPara.Add("@intRegistrationId", SqlDbType.BigInt);

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveRegistration", hstInput, houtPara);
                return houtPara;

            }
            else
            {
                hstInput.Add("@intRegistrationNo", iRegistrationNo);
                hstInput.Add("@intRegistrationId", iRegistrationId);
                int iCount = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPUpdateRegistration", hstInput);
                return houtPara;
            }

        }

        public Hashtable SaveUpdateRegistration(int iHospitalLocationId, int FacilityId, int PageId, string sPreviousName, int iTitle, string sFirstName, string sMiddleName, string sLastName,
        string sDisplayName, int iEthnicityID, string sYear, string sMonth, string sDay, int iGender, int iMaritalStatus, int iReligionId, string sAddress, string sAddressLine2, string sPin,
         int iCity, int iState, int iCountry, string sEmail, string sPhone, string sMobile, int iNationality, string iReferralTypeID, int iIdentityTypeID, string sIdentityTypeNumber,
         int iSponsorId, int iPayerId, int iFacility, int iResposibleSelf, int iRenderingProvider, int iPCP, int iDefPharmacy, int iRaceID, int iReferralByID, string sStatus,
         int iDefLabID, int iDefImageCenterID, string sResidentType, string sSSN, bool bExternalPatient, int iLeadSourceID, int iEncodedBy, int iRegistrationId, int iRegistrationNo,
         string DOB, string sAppointmentID, int iCityAreaId, string sTaggedEmpNo, string sTaggedEmpName, int MotherRegistrationNo,
         string chvVIPNarration, string emergencyContactName, bool bitVIP, string emergencyContactNo,
         int isERRegistration, string Checklist, string EmiratesID, bool bAgeIdentification, int EntrySite, int OccupationId, int HealthcareID, string MotherName, int ResourceAppointmentId, string FatherName, decimal FamilyMonthlyIncome,int OTBookingId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable houtPara = new Hashtable();
            Hashtable hstInput = new Hashtable();

            hstInput.Add("@inyHospitalLocationId", iHospitalLocationId);
            hstInput.Add("@intEncodedBy", iEncodedBy); // mandatory
            hstInput.Add("@intLoginFacilityId", FacilityId);
            hstInput.Add("@intPageId", PageId);
            if (sPreviousName != string.Empty) { hstInput.Add("chvPreviousName", sPreviousName); }
            if (Convert.ToString(iTitle) != string.Empty && Convert.ToString(iTitle) != "0") { hstInput.Add("@intTitle", iTitle); }

            hstInput.Add("@chvFirstName", sFirstName); // mandatory
            if (sMiddleName != string.Empty) { hstInput.Add("@chvMiddleName", sMiddleName); }

            if (sLastName != string.Empty && sLastName != "0")
            {
                hstInput.Add("@chvLastName", sLastName);
            }

            if (sDisplayName != string.Empty && sDisplayName != "0") { hstInput.Add("@chvDisplayName", sDisplayName); }

            //if (FormatDateYearMonthDate(sDOB) != "" && FormatDateYearMonthDate(sDOB) != "0") { hstInput.Add("@chrDateofBirth", FormatDateYearMonthDate(sDOB)); }
            hstInput.Add("@chrDateofBirth", DOB);


            if (iEthnicityID != 0)
            {
                hstInput.Add("@intEthnicityID", iEthnicityID);
            }
            if (Convert.ToString(sYear) != string.Empty) { hstInput.Add("@inyAgeYear", sYear); }
            if (Convert.ToString(sMonth) != string.Empty) { hstInput.Add("@inyAgeMonth", sMonth); }
            if (Convert.ToString(sDay) != string.Empty) { hstInput.Add("@inyAgeDays", sDay); }
            if (Convert.ToString(iGender) != string.Empty && Convert.ToString(iGender) != "0") { hstInput.Add("@inySex", iGender); }
            if (Convert.ToString(iMaritalStatus) != string.Empty && Convert.ToString(iMaritalStatus) != "0") { hstInput.Add("@inyMaritalStatus", iMaritalStatus); }
            if (Convert.ToString(iReligionId) != string.Empty && Convert.ToString(iReligionId) != "0") { hstInput.Add("@inyReligionId", iReligionId); }
            if (sAddress != string.Empty && sAddress != "0") { hstInput.Add("@chvLocalAddress", sAddress); }

            if (sAddressLine2 != string.Empty && sAddressLine2 != "0") { hstInput.Add("@chvLocalAddressLine2", sAddressLine2); }


            if (sPin != string.Empty && sPin != "0") { hstInput.Add("@chvLocalPin", sPin); }
            if (Convert.ToString(iCity) != string.Empty && Convert.ToString(iCity) != "0") { hstInput.Add("@intLocalCity", iCity); }
            if (Convert.ToString(iState) != string.Empty && Convert.ToString(iState) != "0") { hstInput.Add("@intLocalState", iState); }
            if (Convert.ToString(iCountry) != string.Empty && Convert.ToString(iCountry) != "0") { hstInput.Add("@intLocalCountry", iCountry); }
            if (sEmail != string.Empty && sEmail != "0") { hstInput.Add("@chvEmail", sEmail); }
            if (sPhone != string.Empty && sPhone != "0") { hstInput.Add("@chvPhoneHome", sPhone); }
            if (sMobile != string.Empty && sMobile != "0") { hstInput.Add("@chvMobileNo", sMobile); }
            if (Convert.ToString(iNationality) != string.Empty && Convert.ToString(iNationality) != "0") { hstInput.Add("@intNationality", iNationality); }
            if (iReferralTypeID != string.Empty) { hstInput.Add("@intReferralId", iReferralTypeID); }
            if (Convert.ToString(iIdentityTypeID) != string.Empty && Convert.ToString(iIdentityTypeID) != "0") { hstInput.Add("@intIdentityTypeID", iIdentityTypeID); }
            if (sIdentityTypeNumber != string.Empty && sIdentityTypeNumber != "0") { hstInput.Add("@chvIdentityNumber", sIdentityTypeNumber); }
            if (Convert.ToString(iSponsorId) != string.Empty && Convert.ToString(iSponsorId) != "0") { hstInput.Add("@intSponsorId", iSponsorId); }
            if (Convert.ToString(iPayerId) != string.Empty && Convert.ToString(iPayerId) != "0") { hstInput.Add("@intPayorId", iPayerId); }
            if (Convert.ToString(iFacility) != string.Empty && Convert.ToString(iFacility) != "0") { hstInput.Add("@intFacility", iFacility); }

            hstInput.Add("@bitResponsibleSelf", iResposibleSelf); // 0 mandatory 
            if (Convert.ToString(iRenderingProvider) != string.Empty && Convert.ToString(iRenderingProvider) != "0") { hstInput.Add("@intRenderingProvider", iRenderingProvider); }
            if (Convert.ToString(iPCP) != string.Empty && Convert.ToString(iPCP) != "0") { hstInput.Add("@intPCP", iPCP); }
            if (Convert.ToString(iDefPharmacy) != string.Empty && Convert.ToString(iDefPharmacy) != "0") { hstInput.Add("@intDefaultPharmacyID", iDefPharmacy); }

            if (Convert.ToString(iRaceID) != string.Empty && Convert.ToString(iRaceID) != "0") { hstInput.Add("@intRaceID", iRaceID); }
            if (iReferralByID != 0) { hstInput.Add("@intReferredByID", iReferralByID); }
            if (sStatus != string.Empty && sStatus != "0") { hstInput.Add("@chrStatus", sStatus); }
            if (Convert.ToString(iDefLabID) != string.Empty && Convert.ToString(iDefLabID) != "0") { hstInput.Add("@intDefLabID", iDefLabID); }
            if (Convert.ToString(iDefImageCenterID) != string.Empty && Convert.ToString(iDefImageCenterID) != "0") { hstInput.Add("@intDefImageCenterID", iDefImageCenterID); }
            if (sResidentType != string.Empty && sResidentType != "0") { hstInput.Add("@chrResidentType", sResidentType); }
            if (sSSN != string.Empty && sSSN != "0") { hstInput.Add("@chvSocialSecurityNo", sSSN); }
            hstInput.Add("@bitExternalPatient", bExternalPatient);
            if (iLeadSourceID != 0) { hstInput.Add("@intLeadSourceId", iLeadSourceID); }
            hstInput.Add("@intCityAreaId", iCityAreaId);

            if (sTaggedEmpNo != string.Empty) { hstInput.Add("@sTaggedEmpNo", sTaggedEmpNo); }
            if (sTaggedEmpName != string.Empty) { hstInput.Add("@sTaggedEmpName", sTaggedEmpName); }
            hstInput.Add("@motherRegistrationNo", MotherRegistrationNo);

            hstInput.Add("@emergencyContactName", emergencyContactName);
            hstInput.Add("@emergencyContactNo", emergencyContactNo);
            hstInput.Add("@bitVIP", bitVIP);
            hstInput.Add("@chvVIPNarration", chvVIPNarration);
            hstInput.Add("@isERRegistration", isERRegistration);
            hstInput.Add("@XMLChecklist", Checklist);
            hstInput.Add("@chvEmiratesID", EmiratesID);
            hstInput.Add("@bitAgeIdentification", bAgeIdentification);
            hstInput.Add("@intEntrySite", EntrySite);
            hstInput.Add("@intOccupationId ", OccupationId);
            hstInput.Add("@intHealthcareID ", HealthcareID);
            hstInput.Add("@MotherName", MotherName);
            hstInput.Add("@FatherName", FatherName);
            hstInput.Add("@FamilyMonthlyIncome", FamilyMonthlyIncome);

            if (iRegistrationId == 0)
            {

                hstInput.Add("@intAppointmentId", sAppointmentID);
                hstInput.Add("@ResourceAppointmentId", ResourceAppointmentId);
                hstInput.Add("@intOTBookingId", OTBookingId);
                houtPara.Add("@intRegistrationNo", SqlDbType.BigInt);
                houtPara.Add("@intRegistrationId", SqlDbType.BigInt);

                houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveRegistration", hstInput, houtPara);
                return houtPara;

            }
            else
            {
                hstInput.Add("@intRegistrationNo", iRegistrationNo);
                hstInput.Add("@intRegistrationId", iRegistrationId);
                int iCount = objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPUpdateRegistration", hstInput);
                return houtPara;
            }

        }




        public string ValidateRegistrationNO(int RegistrationId, int HospitalLocationID, int FacilityID, int TitleId, string FirstName, string MiddleName,
            string LastName, string MobileNo, string Email, string DateofBirth, int Gender, string Guardian)
        {
            Hashtable hshInput;
            Hashtable hshOutput;
            string sRegistrationNo = string.Empty;
            DataSet ds = new DataSet();
            try
            {
                if (FirstName != string.Empty)
                {
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    hshInput = new Hashtable();
                    hshOutput = new Hashtable();

                    hshInput.Add("@RegistrationId", RegistrationId);
                    hshInput.Add("@HospitalLocationID", HospitalLocationID);
                    hshInput.Add("@FacilityID", FacilityID);
                    hshInput.Add("@TitleId", TitleId);
                    hshInput.Add("@FirstName", FirstName);
                    hshInput.Add("@MiddleName", MiddleName);
                    hshInput.Add("@LastName", LastName);
                    hshInput.Add("@MobileNo", MobileNo);
                    hshInput.Add("@Email", Email);
                    hshInput.Add("@DateofBirth", DateofBirth);
                    hshInput.Add("@Gender", Gender);
                    hshInput.Add("@Guardian", Guardian);

                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspValidateRegistration", hshInput, hshOutput);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return sRegistrationNo = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        }
                        else
                            return sRegistrationNo;
                    }
                    else
                        return sRegistrationNo;

                }
                else
                    return sRegistrationNo;
            }
            catch (Exception ex)
            {
                return sRegistrationNo;
            }
            finally
            {
                ds.Dispose();
            }
        }

        public string ValidateEnrolleNO(int HospitalLocId, int FacilityId, string EnrolleNo)
        {
            Hashtable hshInput;
            Hashtable hshOutput;
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string sRegistrationNo = string.Empty;
            try
            {
                if (EnrolleNo != string.Empty)
                {
                    hshInput = new Hashtable();
                    hshOutput = new Hashtable();

                    hshInput.Add("HospitalLocationID", HospitalLocId);
                    hshInput.Add("FacilityID", FacilityId);
                    hshInput.Add("@chkValue", EnrolleNo);
                    hshInput.Add("chkType", 'E');

                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspValidateRegistration", hshInput, hshOutput);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return sRegistrationNo = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        }
                        else
                            return sRegistrationNo;
                    }
                    else
                        return sRegistrationNo;

                }
                else
                    return sRegistrationNo;
            }
            catch (Exception ex)
            {
                return sRegistrationNo;
            }
            finally
            {
                ds.Dispose();
                objDl = null;
            }
        }

        public string ValidateMobileNO(int HospitalLocId, int FacilityId, string mobileNo)
        {
            Hashtable hshInput;
            Hashtable hshOutput;
            string sRegistrationNo = string.Empty;
            DataSet ds = new DataSet();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshOutput = new Hashtable();
            try
            {
                if (mobileNo != string.Empty)
                {


                    hshInput.Add("HospitalLocationID", HospitalLocId);
                    hshInput.Add("FacilityID", FacilityId);
                    hshInput.Add("PName", string.Empty);
                    hshInput.Add("DOB", string.Empty);
                    hshInput.Add("Mobile", mobileNo);
                    hshInput.Add("chkType", 'M');

                    ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspValidateRegistration", hshInput, hshOutput);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            return sRegistrationNo = ds.Tables[0].Rows[0]["RegistrationNo"].ToString();
                        }
                        else
                            return sRegistrationNo;
                    }
                    else
                        return sRegistrationNo;

                }
                else
                    return sRegistrationNo;
            }
            catch (Exception ex)
            {
                return sRegistrationNo;
            }
            finally
            {
                objDl = null;
                ds.Dispose();
            }
        }

        public string SaveUpdateLeadSource(int Hospid, int FacilityId, int Id, string Name, int Status, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshInput = new Hashtable();
            Hashtable hshOutput = new Hashtable();

            hshInput.Add("@inyHospitalLocationId", Hospid);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@intId", Id);
            hshInput.Add("@chvName", Name);
            hshInput.Add("@bitActive", Status);
            hshInput.Add("@intEncodedBy", EncodedBy);

            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            try
            {
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveUpdateLeadSource", hshInput, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet GetLeadSource(int HospId, int FacilityId, int Id)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            Hashtable hshInput = new Hashtable();


            hshInput.Add("@inyHospitalLocationId", HospId);
            hshInput.Add("@intFacilityId", FacilityId);
            hshInput.Add("@intId", Id);
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetLeadSource", hshInput);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPatientRecord(int intRegId, string sOldRegId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                HshIn.Add("@intRegistrationId", intRegId);
                HshIn.Add("@intOldRegistrationId", sOldRegId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationDetail", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPreRegistrationDetail(int intRegId, int intHospiLocId)
        {
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                StringBuilder strSQ = new StringBuilder();
                HshIn.Add("@intRegistrationId", intRegId);
                //BaseC.Patient objBc = new BaseC.Patient(sConString);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetPreRegistrationDetail", HshIn);

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }
        public SqlDataReader ShowImages(string sRegNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@chvRegistrationNo", sRegNo);
                string squery = "select PatientImage,ImageType from RegistrationImage WITH (NOLOCK) where RegistrationNo=@chvRegistrationNo and Active=1";
                SqlDataReader dr = (SqlDataReader)objDl.ExecuteReader(CommandType.Text, squery, HshIn);

                return dr;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public Hashtable SaveRegistrationInsuranceDetails(int DefaultCase, int Signaturefile, int AssignBinifits, int NoInsurrance, int CaseId, int HospitalId, int RegId,
                int InsuranceCmpId, int Insurancetypecode, string InsuranceOrder, string Insuranceplanname, int GuranterId, string InsuranceId, string Policyno, string Fromdate,
                string ToDate, string GroupNo, string Groupname, string PayClaimOfficeNo, int patientrelease, int AssignmentBenefits, int ClaimFilingIndicatorCode, int ppohmoindicator,
                int KenPac, string PPOIdentification, string authorization, string CopayAmount, int EncodedBy, string NoOfEncounter, int FacilityID, int PageId, string Status, int Id)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable houtPara = new Hashtable();


                HshIn.Add("@bitDefaultCase", DefaultCase);
                HshIn.Add("@bitDefaultSignatureOnFile", Signaturefile);
                HshIn.Add("@bitDefaultAssignmentBenefit", AssignBinifits);
                HshIn.Add("@bitNoInsurance", NoInsurrance);
                HshIn.Add("@intCaseId ", CaseId);
                HshIn.Add("@inyHospitalLocationId", HospitalId);
                HshIn.Add("@intRegistrationNo", RegId);
                HshIn.Add("@intInsuranceId", InsuranceCmpId);
                HshIn.Add("@intInsuranceTypeCode", Insurancetypecode);
                HshIn.Add("@chrInsuranceOrder", InsuranceOrder);
                HshIn.Add("@chvInsurancePlanName", Insuranceplanname);
                HshIn.Add("@intGuarantorId", GuranterId);
                HshIn.Add("@chvInsuredId", InsuranceId);
                HshIn.Add("@chvPolicyNo", Policyno);
                if (Convert.ToString(Fromdate) != string.Empty)
                {
                    HshIn.Add("@chvStartDate", Fromdate);
                }
                if (Convert.ToString(ToDate) != string.Empty)
                {
                    HshIn.Add("@chvEndDate", ToDate);
                }
                HshIn.Add("@chvGroupNo", GroupNo);
                HshIn.Add("@chvGroupName", Groupname);
                HshIn.Add("@chvPayClaimOfficeNo", PayClaimOfficeNo);
                HshIn.Add("@bitPatientReleaseOnFile ", patientrelease);
                HshIn.Add("@bitPatientAssignedBenefits", AssignmentBenefits);
                HshIn.Add("@chvClaimFilingIndicatorCode", ClaimFilingIndicatorCode);

                HshIn.Add("@bitPPO_HMO_Indicator", ppohmoindicator);
                HshIn.Add("@bitKenPac", KenPac);
                HshIn.Add("@chvPPO_Identification", PPOIdentification);
                HshIn.Add("@chvAuthorizationNo", authorization);
                if (CopayAmount != string.Empty)
                {
                    HshIn.Add("@CoPayAmt", Convert.ToDouble(CopayAmount));
                }
                HshIn.Add("@intEncodedBy", EncodedBy);
                if (NoOfEncounter != string.Empty)
                {
                    HshIn.Add("@NoOfEncounter", Convert.ToInt32(NoOfEncounter));
                }
                HshIn.Add("@intLoginFacilityId", FacilityID);
                HshIn.Add("@intPageId", PageId);

                houtPara.Add("@chvErrorStatus", SqlDbType.VarChar);
                if (Status == "New")
                {
                    HshIn.Add("@Status", "New");
                    houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpateRegistrationInsuranceDetails", HshIn, houtPara);

                }
                else
                {
                    HshIn.Add("@Status", "Edit");
                    HshIn.Add("@Id", Id);
                    houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveUpateRegistrationInsuranceDetails", HshIn, houtPara);

                }
                return houtPara;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataSet GetRegistrationpaymentdetails(int CaseId, int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@intCaseId", CaseId);
                HshIn.Add("@intRegistrationId", RegistrationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationPaymentDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        public DataSet CheckRegistrationExists(string RegNo, int HospId, int FaclityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FaclityId);
                HshIn.Add("@chvRegistrationNo", RegNo);

                string strsql = " SELECT Id FROM Encounter WITH (NOLOCK) " +
                    " WHERE convert(varchar,RegistrationNo) = @chvRegistrationNo  AND FacilityID = @intFacilityId AND HospitalLocationId =@inyHospitalLocationId ";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveRegistrationCheckList(int Hospid, int FacilityId, int RegId, string Checklist, int EncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable hshOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", Hospid);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);
                HshIn.Add("@XMLChecklist", Checklist);
                HshIn.Add("@intEncodedBy", EncodedBy);

                hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPSaveRegistrationCheckList", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationCheckList(int RegId, int HospId, int FaclityId)
        {

            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FaclityId);
                HshIn.Add("@chvRegistrationId", RegId);

                string strsql = " SELECT ChecklistId AS ID,ICLM.Description FROM RegistrationChecklist rcl WITH (NOLOCK) " +
                                " INNER JOIN InvoiceCheckListMaster ICLM WITH (NOLOCK) ON iclm.id=rcl.ChecklistId " +
                                " WHERE convert(varchar,RegistrationId) = @chvRegistrationId AND Active=1 AND FacilityID = @intFacilityId  AND HospitalLocationId =@inyHospitalLocationId";

                return objDl.FillDataSet(CommandType.Text, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public Hashtable MergePatientDetails(int iHospitalLocationId, int iOldRegistrationId, int iNewRegistrationId, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                Hashtable HshInOutput = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intOldRegistrationId", iOldRegistrationId);
                HshIn.Add("@intNewRegistrationId", iNewRegistrationId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshInOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                HshInOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspMergePatientDetails", HshIn, HshInOutput);
                return HshInOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetPatientRecordDetails(int iHospitalLocationId, int iRegistrationId, int iEncodedBy)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable HshInOutput = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intRegistrationId", iRegistrationId);
                HshIn.Add("@intEncodedBy", iEncodedBy);
                HshInOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMergePatientDetails", HshIn, HshInOutput).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public bool IsPatientAlertFound(int iRegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@iRegistrationId", iRegistrationId);
                bool iResult = false;
                iResult = (bool)objDl.ExecuteScalar(CommandType.Text, "select dbo.udfGetIsPatientAlert(@iRegistrationId)", HshIn);
                return iResult;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetAlertBlockMessage(int HospId, int FacilityId, int RegId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                string strsql = string.Empty;

                HshIn = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intRegistrationId", RegId);

                strsql = "uspGetPatientBlockAlerts";
                return objDl.FillDataSet(CommandType.StoredProcedure, strsql, HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getRegistrationDeletedRecords(int HospId, int FacilityId, int EncodedBy)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable HshInOutput = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshInOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetRegistrationDeletedRecords", HshIn, HshInOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public string SaveEmployeeSequence(int iHospitalLocationId, int intEncodedBy, string strEmployeeId, string DepartmentCaseType, int CaseId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();
                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@xmlEmployeeIds", strEmployeeId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@chrDepartmentCaseType", DepartmentCaseType);
                HshIn.Add("@intCaseId", CaseId);

                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEmployeeSequence", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }



        public Hashtable SaveUpdateUnregisteredPatientDetails(int iHospitalLocationId, int FacilityId, int iTitle, string sFirstName, string sMiddleName, string sLastName,
        string sYear, string sMonth, string sDay, int iGender, int iMaritalStatus, int iReligionId, string sAddress, string sPin, int iCity, int iState, int iCountry, string sEmail, string sPhone, string sMobile, int iNationality,
            int iEncodedBy, int iRegistrationId, int iRegistrationNo, DateTime DOB, int iCityAreaId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable houtPara = new Hashtable();
            HshIn = new Hashtable();
            try
            {


                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intEncodedBy", iEncodedBy); // mandatory
                HshIn.Add("@intLoginFacilityId", FacilityId);
                if (Convert.ToString(iTitle) != string.Empty && Convert.ToString(iTitle) != "0") { HshIn.Add("@intTitle", iTitle); }

                HshIn.Add("@chvFirstName", sFirstName); // mandatory
                if (sMiddleName != string.Empty) { HshIn.Add("@chvMiddleName", sMiddleName); }

                if (sLastName != string.Empty && sLastName != "0")
                {
                    HshIn.Add("@chvLastName", sLastName);
                }


                HshIn.Add("@chrDateofBirth", DOB);


                if (Convert.ToString(sYear) != string.Empty) { HshIn.Add("@inyAgeYear", sYear); }
                if (Convert.ToString(sMonth) != string.Empty) { HshIn.Add("@inyAgeMonth", sMonth); }
                if (Convert.ToString(sDay) != string.Empty) { HshIn.Add("@inyAgeDays", sDay); }
                if (Convert.ToString(iGender) != string.Empty && Convert.ToString(iGender) != "0") { HshIn.Add("@inySex", iGender); }
                if (Convert.ToString(iMaritalStatus) != string.Empty && Convert.ToString(iMaritalStatus) != "0") { HshIn.Add("@inyMaritalStatus", iMaritalStatus); }
                if (Convert.ToString(iReligionId) != string.Empty && Convert.ToString(iReligionId) != "0") { HshIn.Add("@inyReligionId", iReligionId); }
                if (sAddress != string.Empty && sAddress != "0") { HshIn.Add("@chvLocalAddress", sAddress); }


                if (sPin != string.Empty && sPin != "0") { HshIn.Add("@chvLocalPin", sPin); }
                if (Convert.ToString(iCity) != string.Empty && Convert.ToString(iCity) != "0") { HshIn.Add("@intLocalCity", iCity); }
                if (Convert.ToString(iState) != string.Empty && Convert.ToString(iState) != "0") { HshIn.Add("@intLocalState", iState); }
                if (Convert.ToString(iCountry) != string.Empty && Convert.ToString(iCountry) != "0") { HshIn.Add("@intLocalCountry", iCountry); }
                if (sEmail != string.Empty && sEmail != "0") { HshIn.Add("@chvEmail", sEmail); }
                if (sPhone != string.Empty && sPhone != "0") { HshIn.Add("@chvPhoneHome", sPhone); }
                if (sMobile != string.Empty && sMobile != "0") { HshIn.Add("@chvMobileNo", sMobile); }
                if (Convert.ToString(iNationality) != string.Empty && Convert.ToString(iNationality) != "0") { HshIn.Add("@intNationality", iNationality); }

                HshIn.Add("@intCityAreaId", iCityAreaId);

                if (iRegistrationId == 0)
                {
                    houtPara.Add("@intRegistrationNo", SqlDbType.Int);
                    houtPara.Add("@intRegistrationId", SqlDbType.Int);
                    houtPara.Add("@intGeneratedId", SqlDbType.Int);
                    houtPara = objDl.getOutputParametersValues(CommandType.StoredProcedure, "UspSaveCounselingUnregisteredPatientDetails", HshIn, houtPara);

                }
                return houtPara;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }



        public DataSet GetEmployeeSequenceNo(int HospId, int FacilityId, int DeprtmentId, int EmolType, string DepartmentCaseType, int CaseId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intDeortmentId", DeprtmentId);
                HshIn.Add("@intEmployeeType", EmolType);
                HshIn.Add("@chrDepartmentCaseType", DepartmentCaseType);
                HshIn.Add("@intCaseId", CaseId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPGetEmployeeSequenceNo", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getEMRTemplateEntryTracker(int HospId, int FacilityId, int UserId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacilityId", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetEMRTemplateEntryTracker", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetRegistrationUniqueKeyCombination(int HospId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacility", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "UspGetRegistrationUniqueKeyCombination", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveRegistrationUniqueKeyCombination(int HospId, int FacilityId, int UserId, string UniqueKeyCombination)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                Hashtable hshOut = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intFacility", FacilityId);
                HshIn.Add("@intEncodedBy", UserId);
                HshIn.Add("@XMLUniqueKeyCombination", UniqueKeyCombination);
                hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveRegistrationUniqueKeyCombination", HshIn, hshOut);
                return hshOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet getHospitalLocationNames()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            string strsql = "SELECT Name FROM HospitalLocation WITH(NOLOCK) WHERE Active = 1";
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.Text, strsql);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet GetPatientDashBordSection(int iHospitalLocationId, int IFacilityId)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@intHospitalLocationId", iHospitalLocationId);
            hshInput.Add("@intFacilityId", IFacilityId);

            //return  objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchPatientHistry", hshInput);  PPSpVisitHistory
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDashBordSection", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetPrescribedMedicineHistory(int HospitalLocationId, int FacilityId, string UHID)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@intHospitalLocationId", HospitalLocationId);
            hshInput.Add("@intFacility", FacilityId);
            //hshInput.Add("@EncounterNo", EncounterNo);
            hshInput.Add("@UHID", UHID);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetLastPrescribeMedicineInfo", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


            //return  objDl.FillDataSet(CommandType.StoredProcedure, "PPLabResult", hshInput);

        }
        public DataSet GetLabVisitHistory(int HospitalLocationId, int FacilityId, string UHID)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@intHospitalLocationId", HospitalLocationId);
            hshInput.Add("@intFacility", FacilityId);
            //hshInput.Add("@EncounterNo", EncounterNo);
            hshInput.Add("@UHID", UHID);
            // Change by kuldeep kumar 22/01/2021 
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetLabInfo", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


            //return  objDl.FillDataSet(CommandType.StoredProcedure, "PPLabResult", hshInput);

        }
        public DataSet GetPatientHistoryDetails(int iHospitalLocationId, int IFacilityId, int iRegId, string Daterange, string iFromdate, string iTodate, string Source)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@inyHospitalLocationId", iHospitalLocationId);
            hshInput.Add("@intFacilityId", IFacilityId);
            //hshInput.Add("@intProviderId", iDoctorId);
            hshInput.Add("@intRegistrationId", iRegId);
            hshInput.Add("@chvDateRange", Daterange);
            hshInput.Add("@chrFromDate", iFromdate);
            hshInput.Add("@chrToDate", iTodate);
            hshInput.Add("@Source", Source);
            //return  objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchPatientHistry", hshInput);  PPSpVisitHistory
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetVisitHistory", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetDocumentList(int HospitalLocationId, int FacilityId, Int64 UHID)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@UHID", UHID);
            hshInput.Add("@inyHospitalLocationId", HospitalLocationId);
            hshInput.Add("@intFacilityId", FacilityId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "PPUSPDocList", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


            //@inyHospitalLocationId    @intFacilityId
            //return  objDl.FillDataSet(CommandType.StoredProcedure, "PPFAQData");


        }
        public DataSet GetPatientDashboard(int RegistrationId)
        {

            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            hshInput = new Hashtable();
            hshInput.Add("@intRegistrationId", RegistrationId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDashboard", hshInput);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }
        public int getMainFacilityId(int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshInIn = new Hashtable();
            int intMainFacilityId = 0;
            DataSet ds = new DataSet();
            try
            {
                HshInIn.Add("@intFacilityId", FacilityId);
                //string strsql = "SELECT MainFacility FROM FacilityMaster WITH(NOLOCK) WHERE FacilityID=@intFacilityId AND Active=1";
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetMainFacility", HshInIn);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    intMainFacilityId = Convert.ToInt32(ds.Tables[0].Rows[0]["MainFacility"]);
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
                HshInIn = null;
            }
            return intMainFacilityId;
        }

        public DataTable checkPendingBookingForAdmission(int RegID, int HospId, int FacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();

                HshIn.Add("@HospitalLocationId", HospId);
                HshIn.Add("@registrationid", RegID);
                HshIn.Add("@FacilityId", FacilityId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "checkforExitBooking", HshIn).Tables[0];
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public string getPatientDetailsFromRegistrationNo(string RegistrationNo, int FacilityId)
        {
            Hashtable hshInput;
            string sPatientName = string.Empty;
            try
            {
                if (RegistrationNo != string.Empty)
                {
                    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                    hshInput = new Hashtable();
                    hshInput.Add("@RegistrationNo", RegistrationNo);
                    hshInput.Add("@FacilityId", FacilityId);

                    sPatientName = Convert.ToString(objDl.ExecuteScalar(CommandType.Text, "SELECT ISNULL(FirstName,'') + ISNULL(' ' + MiddleName,'') + ISNULL(' ' + LastName,'') AS PatientName from Registration WITH (NOLOCK) WHERE RegistrationNo = @RegistrationNo AND FacilityID = @FacilityId ", hshInput));
                    return sPatientName;
                }
                else
                {
                    return sPatientName;
                }
            }
            catch (Exception ex)
            {
                return sPatientName;
            }
        }

        public DataSet getPatientDetailsEnc(int iHospID, int iFacilityId, int iRegistrationId, string IPNo, int iEncounterId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationId", iHospID);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("chvEncounterNo", IPNo);
            HshIn.Add("intEncounterId", iEncounterId);
            HshIn.Add("intEncodedBy", EncodedBy);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetailsIP", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getPatientDetailsMRDNo(int iHospID, int iFacilityId, int iRegistrationId, string MRDNo, int iEncounterId, int EncodedBy)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn.Add("inyHospitalLocationId", iHospID);
            HshIn.Add("intFacilityId", iFacilityId);
            HshIn.Add("intRegistrationId", iRegistrationId);
            HshIn.Add("chvMRDFileNo", MRDNo);
            HshIn.Add("intEncounterId", iEncounterId);
            HshIn.Add("intEncodedBy", EncodedBy);

            HshOut.Add("chvErrorStatus", SqlDbType.VarChar);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientDetailsMRDNo", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetEmployeeTagging(int HospitalLocationId, string sEmpNo)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hshIn = new Hashtable();
            hshIn.Add("@EmpNo", sEmpNo);
            hshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select E.EmployeeNo, tm.Name + ' ' + E.FirstName + ISNULL(' ' +E.MiddleName,'') + ISNULL(' ' +E.LastName,'') EmpName, case E.Gender  when 1 then 'Female' when 2 then 'Male' else 'Unknown' end + '/' + dbo.AgeInYrsMonthDay(CONVERT(VARCHAR, E.DateofBirth, 111), CONVERT(VARCHAR, GETDATE(), 111)) AS GenderAge,E.Gender FROM Employee E WITH(NOLOCK) left JOIN TitleMaster tm WITH(NOLOCK) ON E.TitleId = tm.TitleID WHERE E.Id = @EmpNo AND E.HospitalLocationId = @inyHospitalLocationId AND E.Active = 1", hshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }



        }

        public DataSet DiagLabGraphForDashboard(int iHospId, int iFacilityId, int iRegId)
        {


            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshInput = new Hashtable();
                hshInput.Add("@inyHospitalLocationId", iHospId);
                hshInput.Add("@intFacilityId", iFacilityId);
                hshInput.Add("@intRegistrationId", iRegId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabGraphForDashboard", hshInput);

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

        public DataSet GetEMRTemplateCalculatedGraph(int RegistrationId, int EncounterId, int FieldId, DateTime FromDate, DateTime ToDate)
        {


            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshInput = new Hashtable();
                hshInput.Add("@RegistrationId", RegistrationId);
                hshInput.Add("@EncounterId", EncounterId);
                hshInput.Add("@FieldId", FieldId);
                hshInput.Add("@FromDate", FromDate);
                hshInput.Add("@ToDate", ToDate);

                return objDl.FillDataSet(CommandType.StoredProcedure, "GetEMRTemplateCalcGraph", hshInput);

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
        public DataSet GetEMREWSGraphData(int RegistrationId, int EncounterId, int TemplateId)
        {


            Hashtable hshInput;
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                hshInput = new Hashtable();
                hshInput.Add("@RegistrationId", RegistrationId);
                hshInput.Add("@EncounterId", EncounterId);
                hshInput.Add("@TemplateId", TemplateId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "getEMREWSGraphData", hshInput);

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
        public DataSet GetEWSMasterData()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.Text, "select ScoreValue,MandatoryAction from EMREWSSetup");
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet getEMRPatientDetailsFilter(int iHospID, int iFacilityId, string sRegistrationNo, int iRegId, string sEncounterNo, int iEncId)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            HshIn.Add("@inyHospitalLocationId", iHospID);
            HshIn.Add("@intFacilityId", iFacilityId);
            HshIn.Add("@chvRegistrationNo", sRegistrationNo);
            HshIn.Add("@intRegId", iRegId);
            HshIn.Add("@chvEncounterNo", sEncounterNo);
            HshIn.Add("@intEncId", iEncId);
            // Change by kuldeep kumar 22/01/2021 
            try
            {
                return objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientDetailsFilter", HshIn, HshOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }

    }
}
