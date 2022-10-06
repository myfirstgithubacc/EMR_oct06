using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace BaseC
{
    public class Letters
    {
        private string sConString = "";
        Hashtable HshIn;
        Hashtable hstOutput;
        public Letters(string Constring)
        {
            sConString = Constring;
        }

        public DataSet Addressing(Int16 HospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.Text, "SELECT SalutationId, Message FROM LetterSalutation WITH (NOLOCK) WHERE Active = 1 AND Type = 'A' AND HospitalLocationId = @HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet Addressing(string HospitalLocationId, string AddressSalutationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                HshIn.Add("AddressSalutationId", AddressSalutationId);
                return objDl.FillDataSet(CommandType.Text, "SELECT SalutationId, Message FROM LetterSalutation WITH (NOLOCK) WHERE Active = 1 AND Type = 'A' AND SalutationId = @AddressSalutationId AND HospitalLocationId = @HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet Salutations(Int16 HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {
                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.Text, "SELECT SalutationId, Message FROM LetterSalutation WITH (NOLOCK) WHERE Active = 1 AND Type = 'S' AND HospitalLocationId = @HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet Salutations(string HospitalLocationId, string AddressSalutationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                HshIn.Add("AddressSalutationId", AddressSalutationId);
                return objDl.FillDataSet(CommandType.Text, "SELECT SalutationId, Message FROM LetterSalutation WITH (NOLOCK) WHERE Active = 1 AND Type = 'S' AND SalutationId = @AddressSalutationId AND HospitalLocationId = @HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public DataSet Types(Int16 HospitalLocationId)
        {

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("HospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.Text, "SELECT SalutationId, Message FROM LetterSalutation WITH (NOLOCK) WHERE Active = 1 AND Type = 'T' AND HospitalLocationId = @HospitalLocationId ", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        public DataSet GetLettersTemplate(string TypeId, string HospitalLocationId, string RegistrationId, string EncounterId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("@intTypeID", TypeId);
                HshIn.Add("intRegistrationId", RegistrationId);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchLetterTemplates", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int SaveLetters(string RegistrationId, string EncounterId, string FacilityId, string LetterTypeId, string PatientId, string ProviderId, string AddressingId, string SenderId, string RecipientDisplayName, string Greetings, string Body, string Closing, string SalutationId, string EncodedBy, string LetterId, string LetterTo, bool DisplayInLetter, string RegardingPatient, string cc1, string cc2, string cc3, string address, string Signature, int letterTotype, bool bitDisplay)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intRegistrationId", RegistrationId);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("insFacilityId", FacilityId);
                HshIn.Add("inyLetterTypeId", LetterTypeId);
                HshIn.Add("intPatientId", PatientId);
                HshIn.Add("intProviderId", ProviderId);
                HshIn.Add("inyAddressingId", AddressingId);
                HshIn.Add("inySenderId", SenderId);
                HshIn.Add("chvRecipientDisplayName", RecipientDisplayName);
                HshIn.Add("chvGreetings", Greetings);
                HshIn.Add("chvBody", Body);
                HshIn.Add("chvClosing", Closing);
                HshIn.Add("inySalutationId", SalutationId);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("chrLetterTo", LetterTo);
                HshIn.Add("DisplayInLetter", DisplayInLetter);
                HshIn.Add("RegardingPatient", RegardingPatient);
                if (LetterId == "0")
                    HshIn.Add("chrInsertUpdate", 'I');
                else
                    HshIn.Add("chrInsertUpdate", 'U');
                HshIn.Add("intLetterId", LetterId);
                HshIn.Add("CC1", cc1);
                HshIn.Add("CC2", cc2);
                HshIn.Add("CC3", cc3);
                HshIn.Add("Address", address);
                HshIn.Add("Signature", Signature);
                HshIn.Add("intLetterToType", letterTotype);
                HshIn.Add("bitDisplayPatientName", bitDisplay);


                hstOutput = new Hashtable();
                hstOutput.Add("@intLtterId", SqlDbType.Int);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRSaveUpdateLetter", HshIn, hstOutput);

                return Convert.ToInt32(hstOutput["@intLtterId"]);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }
        // update by Rajeev
        //public int SaveLetters(string RegistrationId, string EncounterId, string FacilityId, string LetterTypeId, string PatientId, string ProviderId, string AddressingId, string SenderId, string RecipientDisplayName, string Greetings, string Body, string Closing, string SalutationId, string EncodedBy, string LetterId, string LetterTo, bool DisplayInLetter, string RegardingPatient, string cc1, string cc2, string cc3, string address, string Signature)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hstInput = new Hashtable();
        //    hstInput.Add("intRegistrationId", RegistrationId);
        //    hstInput.Add("intEncounterId", EncounterId);
        //    hstInput.Add("insFacilityId", FacilityId);
        //    hstInput.Add("inyLetterTypeId", LetterTypeId);
        //    hstInput.Add("intPatientId", PatientId);
        //    hstInput.Add("intProviderId", ProviderId);
        //    hstInput.Add("inyAddressingId", AddressingId);
        //    hstInput.Add("inySenderId", SenderId);
        //    hstInput.Add("chvRecipientDisplayName", RecipientDisplayName);
        //    hstInput.Add("chvGreetings", Greetings);
        //    hstInput.Add("chvBody", Body);
        //    hstInput.Add("chvClosing", Closing);
        //    hstInput.Add("inySalutationId", SalutationId);
        //    hstInput.Add("intEncodedBy", EncodedBy);
        //    hstInput.Add("chrLetterTo", LetterTo);
        //    hstInput.Add("DisplayInLetter", DisplayInLetter);
        //    hstInput.Add("RegardingPatient", RegardingPatient);
        //    if (LetterId == "0")
        //        hstInput.Add("chrInsertUpdate", 'I');
        //    else
        //        hstInput.Add("chrInsertUpdate", 'U');
        //    hstInput.Add("intLetterId", LetterId);
        //    hstInput.Add("CC1", cc1);
        //    hstInput.Add("CC2", cc2);
        //    hstInput.Add("CC3", cc3);
        //    hstInput.Add("Address", address);
        //    hstInput.Add("Signature", Signature);

        //    hstOutput = new Hashtable();
        //    hstOutput.Add("@intLtterId", SqlDbType.Int);
        //    hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);

        //    hstOutput = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRSaveUpdateLetter", hstInput, hstOutput);

        //    return Convert.ToInt32(hstOutput["@intLtterId"]);

        //}

        public int SaveLettersTemplate(string RegistrationId, string EncounterId, string FacilityId, string LetterTypeId, string PatientId, string ProviderId, string AddressingId, string SenderId, string RecipientDisplayName, string Greetings, string Body, string Closing, string SalutationId, string EncodedBy, string LetterId, string LetterTo, bool DisplayInLetter, string RegardingPatient, string cc1, string cc2, string cc3, string address, string Signature)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("intRegistrationId", RegistrationId);
                HshIn.Add("intEncounterId", EncounterId);
                HshIn.Add("insFacilityId", FacilityId);
                HshIn.Add("inyLetterTypeId", LetterTypeId);
                HshIn.Add("intPatientId", PatientId);
                HshIn.Add("intProviderId", ProviderId);
                HshIn.Add("inyAddressingId", AddressingId);
                HshIn.Add("inySenderId", SenderId);
                HshIn.Add("chvRecipientDisplayName", RecipientDisplayName);
                HshIn.Add("chvGreetings", Greetings);
                HshIn.Add("chvBody", Body);
                HshIn.Add("chvClosing", Closing);
                HshIn.Add("inySalutationId", SalutationId);
                HshIn.Add("intEncodedBy", EncodedBy);
                HshIn.Add("chrLetterTo", LetterTo);
                HshIn.Add("DisplayInLetter", DisplayInLetter);
                HshIn.Add("RegardingPatient", RegardingPatient);
                if (LetterId == "0")
                    HshIn.Add("chrInsertUpdate", 'I');
                else
                    HshIn.Add("chrInsertUpdate", 'U');
                HshIn.Add("intLetterId", LetterId);
                HshIn.Add("CC1", cc1);
                HshIn.Add("CC2", cc2);
                HshIn.Add("CC3", cc3);
                HshIn.Add("Address", address);
                HshIn.Add("Signature", Signature);

                hstOutput = new Hashtable();
                hstOutput.Add("@intLtterId", SqlDbType.Int);
                hstOutput.Add("@chvErrorStatus", SqlDbType.VarChar);


                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPEMRSaveUpdateLetterTemplate", HshIn, hstOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        public int SaveAddressingSalutation(string HospitalLocationId, string Message, string TypeId, string InsertUpdate, string AddressSalutationId, int Encodedby)
        {
            HshIn = new Hashtable();
            hstOutput = new Hashtable();
            HshIn.Add("inyHospitalLocationId", HospitalLocationId);
            HshIn.Add("chvMessage", Message);
            HshIn.Add("chrTypeId", TypeId);
            HshIn.Add("chrInsertUpdate", InsertUpdate);
            HshIn.Add("intAddressSalutationId", AddressSalutationId);
            HshIn.Add("@intEncodedBy", Encodedby);
            hstOutput.Add("chvErrorStatus", SqlDbType.VarChar);
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                return objDl.ExecuteNonQuery(CommandType.StoredProcedure, "USPEMRSaveAddressingSalutation", HshIn, hstOutput);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }
        //Not in Use
        //public string getNotesSummary(string RegistrationId, string FormId, string EncounterId)
        //{
        //    hstInput = new Hashtable();
        //    hstInput.Add("RegistrationId", RegistrationId);
        //    hstInput.Add("FormId", FormId);          
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    string sSummary = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT IsNull(PatientSummary, '') PatientSummary FROM EMRPatientForms WHERE RegistrationId = @RegistrationId AND FormId = @FormId  and EncounterId="+EncounterId +" AND Active = 1", hstInput);
        //    //string sSummary = (string)objDl.ExecuteScalar(CommandType.Text, "SELECT IsNull(PatientSummary, '') PatientSummary FROM EMRPatientForms WHERE RegistrationId = @RegistrationId AND FormId = @FormId  AND Active = 1", hstInput);
        //    return sSummary;
        //}

        public DataSet GetLetters(string LetterId, string HospitalLocationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString); try
            {

                HshIn = new Hashtable();
                HshIn.Add("LetterId", LetterId);
                HshIn.Add("inyHospitalLocationId", HospitalLocationId);
                return objDl.FillDataSet(CommandType.StoredProcedure, "UspSearchLetters", HshIn);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }
        //Not in Use
        //public bool SignInLetter(string LetterId)
        //{
        //    DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //    hstInput = new Hashtable();
        //    hstInput.Add("LetterId", LetterId);
        //    string sQ = "UPDATE Letters SET StatusId = 20 WHERE LetterId = @LetterId"; 
        //    hstInput = new Hashtable();
        //    hstInput.Add("LetterId", LetterId);
        //    int i = objDl.ExecuteNonQuery(CommandType.Text, sQ, hstInput);
        //    if (i == 0)
        //        return true;
        //    else
        //        return false;

        //}
    }
}