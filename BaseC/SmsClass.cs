using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
// Created by Rajeev , 11/04/2014 for sms related information
namespace BaseC
{
    public class SmsClass
    {
        private string sConString = "";
        public SmsClass(string Constring)
        {
            sConString = Constring;
        }
        private Hashtable HshIn;
        Hashtable hshOutput;

        public DataSet GetSmsDetails(int HospId, int ReferToDoctorId, int EncounterId)
        {
            HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {


                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intReferToDoctorId", ReferToDoctorId);
                HshIn.Add("@intEncounterId", EncounterId);

                return objDl.FillDataSet(CommandType.StoredProcedure, "USPSMSDetails", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetSMSEvent(int iHospitalLocationId, int iFacilityId, string sEventName, int iUserId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            try
            {
                HshIn = new Hashtable();

                HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
                HshIn.Add("@intFacilityId", iFacilityId);
                HshIn.Add("@chvEventName", sEventName);
                HshIn.Add("@intUserId", iUserId);

                string qry = " SELECT ES.EMPCODE AS EMPId, DBO.GetDoctorName(ES.EMPCODE) AS DoctorName, DescriptionOfUse,EevntName AS EventName,ISNULL(EM.Mobile,'') MobileNo " +
                             " FROM [dbo].[EventSMS] ES WITH (NOLOCK) INNER JOIN Employee EM WITH (NOLOCK) ON ES.EMPCODE=EM.ID WHERE EevntName=@chvEventName " +
                             " AND ISNULL(EM.Mobile,'')<>'' AND ES.FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId " +

                             " SELECT SV.Doctor_code AS EMPId, DBO.GetDoctorName(SV.Doctor_code) AS DoctorName,EventName " +
                             " FROM SMSserver SV WITH (NOLOCK) INNER JOIN Employee EM WITH (NOLOCK) ON SV.Doctor_code=EM.ID INNER JOIN USERS US WITH (NOLOCK) ON EM.ID=US.EmpID " +
                             " WHERE EventName=@chvEventName AND US.ID=@intUserId AND Status=1 AND CONVERT(VARCHAR(10), Send_Datetime,111)=CONVERT(VARCHAR(10),GETUTCDATE(),111)";

                return objDl.FillDataSet(CommandType.Text, qry, HshIn);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }


        }
        public Hashtable SaveMidNightSendSMS(string xmlSMSData)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn.Add("@xmlSendData", xmlSMSData);
                hshOutput.Add("@chvOutPut", SqlDbType.VarChar);
                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPInsertMidNightSendSMS", HshIn, hshOutput);
                return hshOutput;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetSMSSetup(int iHospitalLocationId, int iFacilityId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable HshIn = new Hashtable();
            HshIn.Add("@inyHospitalLocationId", iHospitalLocationId);
            HshIn.Add("@intFacilityId", iFacilityId);
            string sQuery = "SELECT PushURL,PullURL,UID,PWD,SID FROM SMSSetUp WITH (NOLOCK) WHERE Flag=1 AND FacilityId=@intFacilityId AND HospitalLocationId=@inyHospitalLocationId ";

            try
            {
                return objDl.FillDataSet(CommandType.Text, sQuery, HshIn).Tables[0];

            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }

        public string DeleteSMSSetup(int EventId, int HospitalLocationId, int FacilityId, string EventName, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();
            HshIn.Add("@intEventId", EventId);
            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEventName", EventName);
            HshIn.Add("@intEncodedBy", EncodedBy);

            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);


            try
            {

                hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteSMSEventDetails", HshIn, hshOutput);
                return hshOutput["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }
        }



        public String SaveSMSSetup(string EventName, int HospitalLocationId, int FacilityId, string EventText, int EncodedBy)
        {
            HshIn = new Hashtable();
            hshOutput = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@chvEventName", EventName);
            HshIn.Add("@chvEventText", EventText);
            HshIn.Add("@intEncodedBy", EncodedBy);

            hshOutput.Add("@chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

            hshOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveSMSEventDetails", HshIn, hshOutput);
            return hshOutput["@chvErrorStatus"].ToString();
        }


        public DataSet GetDescriptionOfUse()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Hashtable HshIn = new Hashtable();
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSMSEvents");

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }

        }
        public DataSet GetSMSKeywords()
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            // Hashtable HshIn = new Hashtable();
            try
            {

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetSMSKeywords");

            }
            catch (Exception ex) { throw ex; }
            finally { objDl = null; }

        }
    }
}
