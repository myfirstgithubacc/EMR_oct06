using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace BaseC
{
    public class clsIVF
    {
        string sConString = "";
        DAL.DAL objDl;

        Hashtable HshIn;
        Hashtable hstOut;
        public clsIVF(string Constring)
        {
            sConString = Constring;
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        }

        public DataTable GetIVFPatientDetails(int intIVFNo, int intRegitrationNo, int intEncodedBy, int intHospitalLocationId, int intLoginFacilityId)
        {

            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFNo", intIVFNo);
                HshIn.Add("@intRegitrationNo", intRegitrationNo);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatientDetails", HshIn, hstOut)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                hstOut = null;
                objDl = null;
            }
        }

        public Hashtable SaveIVFPatientDetails(int intIVFId, int intIVFNo, int intSpouseIVFId, int intRegitrationId, int intSpouseRegitrationId, int intProviderId,
            int intEncodedBy, int intHospitalLocationId, int intLoginFacilityId, int intDonorIVFId, int intDonorRegistrationId, int intSurrogateIVFId, int intSurrogateRegistrationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFId", intIVFId);
                HshIn.Add("@intIVFNo", intIVFNo);
                HshIn.Add("@intSpouseIVFId", intSpouseIVFId);
                HshIn.Add("@intDonorIVFId", intDonorIVFId);
                HshIn.Add("@intSurrogateIVFId", intSurrogateIVFId);
                HshIn.Add("@intRegitrationId", intRegitrationId);
                HshIn.Add("@intSpouseRegitrationId", intSpouseRegitrationId);
                HshIn.Add("@intDonorRegitrationId", intDonorRegistrationId);
                HshIn.Add("@intSurrogateRegitrationId", intSurrogateRegistrationId);
                HshIn.Add("@intProviderId", intProviderId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hstOut.Add("@intIVFNoStatus", SqlDbType.Int);
                hstOut.Add("@intPartnerTagId", SqlDbType.VarChar);
                hstOut.Add("@intSpouseTagId", SqlDbType.Int);
                hstOut.Add("@intDonorTagId", SqlDbType.VarChar);
                hstOut.Add("@intSurrogateTagId", SqlDbType.VarChar);
                return objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveIVFPatientDetails", HshIn, hstOut);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                hstOut = null;
                objDl = null;
            }
        }

        public DataTable GetIVFPatientList(int intIVFNo, int intRegitrationNo, string chvEncounterNo, string chvPatientName, int intEncodedBy, int intHospitalLocationId, int intLoginFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFNo", intIVFNo);
                HshIn.Add("@intRegitrationNo", intRegitrationNo);
                HshIn.Add("@chvEncounterNo", chvEncounterNo);
                HshIn.Add("@chvPatientName", chvPatientName);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatientList", HshIn, hstOut)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                HshIn = null;
                hstOut = null;
                objDl = null;
            }
        }
        public DataTable GetIVFPackageList(int intLoginFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPackageList", HshIn)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetIVFPatientPackageList(int intIVFId, int intEncodedBy, int intHospitalLocationId, int intLoginFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFId", intIVFId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatientPackageList", HshIn, hstOut)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string SaveIVFPatientPackageList(int intIVFId, int IntPackageId, int intEncodedBy, int intHospitalLocationId, int intLoginFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFId", intIVFId);
                HshIn.Add("@IntPackageId", IntPackageId);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hstOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveIVFPatientPackageList", HshIn, hstOut);
                return hstOut["@chvErrorStatus"].ToString();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataTable GetIVFPatientPackageDetails(int intIVFNo, int intEncodedBy, int intHospitalLocationId, int intLoginFacilityId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFNo", intIVFNo);
                HshIn.Add("@intEncodedBy", intEncodedBy);
                HshIn.Add("@intHospitalLocationId", intHospitalLocationId);
                HshIn.Add("@intLoginFacilityId", intLoginFacilityId);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatientPackageDetails", HshIn)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }


        public DataTable SwitchIVFPartner(int RegistrationId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@RegistrationId", RegistrationId);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspIVFSwitchPartner", HshIn)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public string DeletePackageTagging(int HospitalLocationId, int IVFId, int PackageId, int Encodedby)
        {


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                hstOut = new Hashtable();

                HshIn.Add("@intHospitalLocationId", HospitalLocationId);
                HshIn.Add("@IVFId", IVFId);
                HshIn.Add("@PackageId", PackageId);
                HshIn.Add("@intEncodedBy", Encodedby);

                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);
                hstOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspIVFDeletePackageTagging", HshIn, hstOut);
                return hstOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataTable GetIVFPackageServiceDetails(int PackageId)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intPackageId", PackageId);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPackageServiceList", HshIn)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable GetIVFPatientPartnerDetails(int IvfNo)
        {
            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshIn = new Hashtable();
            hstOut = new Hashtable();
            try
            {
                HshIn.Add("@intIVFNo", IvfNo);
                return (objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFPatientSpouseEncounterDetails", HshIn)).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }

        }
        public DataTable getIVFCustomFieldDetails(int IvfId)
        {
            DataTable dt = new DataTable();
            try
            {
                HshIn = new Hashtable();
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

                HshIn.Add("@IvfId", IvfId);
                HshIn.Add("@MasterOption", "IVF");



                string qry = "SELECT ifm.FieldId, ifm.FieldName, ifm.FieldType, " +
                            " ifm.FieldLength, ifm.GroupId, ifm.DecimalPlaces, ifd.ValueText, ifd.ValueId, ifd.ValueWordProcessor " +
                            " FROM PhrCustomFieldsMaster ifm LEFT OUTER JOIN IVFCustomFieldDetails ifd ON ifm.FieldId = ifd.FieldId AND ifd.Active = 1 AND ifd.IVFId = @IvfId " +
                            " WHERE ifm.Active = 1 AND MasterOption=@MasterOption";


                qry += " ORDER BY ifm.SequenceNo";

                dt = objDl.FillDataSet(CommandType.Text, qry, HshIn).Tables[0];
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally { HshIn = null; objDl = null; }
            return dt;
        }

        public DataSet getGroupValueDetails(int GroupId, int Active, int HospId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@intGroupId", GroupId);
                HshIn.Add("@inyHospitalLocationId", HospId);
                HshIn.Add("@intEncodedBy", EncodedBy);

                HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetIVFGroupValueDetails", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }



        }

        public string GetPatientRegistrationNo(int RegistrationNo)
        {


            objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn = new Hashtable();
                hstOut = new Hashtable();

                HshIn.Add("@intRegistrationNo", RegistrationNo);

                hstOut.Add("@chvErrorStatus", SqlDbType.VarChar);

                hstOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspGetIVFPatientRegistrationNo", HshIn, hstOut);
                return hstOut["@chvErrorStatus"].ToString();
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public DataSet saveRenderIVFPackageInIP(int iHospID, int iFacilityId, int iRegistrationId, string RegistrationNo, int iEncounterId, int EncodedBy)
        {
            HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("inyHospitalLocationId", iHospID);
                HshIn.Add("intFacilityId", iFacilityId);
                HshIn.Add("intRegistrationId", iRegistrationId);
                HshIn.Add("chvRegistrationNo", RegistrationNo);
                HshIn.Add("intEncounterId", iEncounterId);
                HshIn.Add("intEncodedBy", EncodedBy);

                HshOut.Add("chvErrorStatus", SqlDbType.VarChar);

                return objDl.FillDataSet(CommandType.StoredProcedure, "uspgetIVFPatientDetailsInIP", HshIn, HshOut);
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }


        }

        public int getLastConsultationDoctor(int RegistrationId)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            DataSet ds = new DataSet();
            HshIn = new Hashtable();
            try
            {
                HshIn.Add("@RegistrationId", RegistrationId);

                string disString = "SELECT TOP 1 (CASE WHEN ISNULL(sod.DoctorId,0) = 0 THEN reg.RenderingProvider ELSE sod.DoctorId END) AS RenderingProvider " +
                                    " FROM Registration reg with (nolock) " +
                                    " LEFT OUTER JOIN ServiceOrderMain som with (nolock) ON reg.Id = som.RegistrationId " +
                                    " LEFT OUTER JOIN ServiceOrderDetail sod with (nolock) ON som.Id = sod.OrderId AND ISNULL(sod.DoctorId,0)<>0 " +
                                    " WHERE reg.Id = @RegistrationId " +
                                    " ORDER BY sod.Id DESC ";

                ds = objDl.FillDataSet(CommandType.Text, disString, HshIn);

                int DoctorId = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DoctorId = Convert.ToInt32(ds.Tables[0].Rows[0]["RenderingProvider"]);
                }

                return DoctorId;
            }
            catch (Exception ex) { throw ex; }
            finally { HshIn = null; objDl = null; }

        }

        public string SaveEMRSingleScreenUserTemplates(int HospId, int FacilityId, int SpecialisationId, string xmlTemplateIds, string xmlNonFreeTextTemplateIds, int EncodedBy, int DoctorId, string DPSetting)
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();

            HshIn.Add("@inyHospitalLocationId", HospId);
            HshIn.Add("@intFacilityId", FacilityId);
            HshIn.Add("@intSpecialisationId", SpecialisationId);
            HshIn.Add("@xmlTemplateIds", xmlTemplateIds);
            HshIn.Add("@xmlNonFreeTextTemplateIds", xmlNonFreeTextTemplateIds);
            HshIn.Add("@intEncodedBy", EncodedBy);
            HshIn.Add("@intDoctorId", DoctorId);
            HshIn.Add("@chvDPSetting", DPSetting);
            HshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            HshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSaveEMRSingleScreenUserTemplates", HshIn, HshOut);

            return HshOut["@chvErrorStatus"].ToString();
        }
        public DataSet getEMRTemplateTypeWiseConsent(int HospId, string TemplateIdTypeCode, string TemplateName,int EncounterId)
        {

            try
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                DataSet ds = new DataSet();
                Hashtable HshIn = new Hashtable();

                HshIn.Add("@intHospId", HospId);
                HshIn.Add("@chvTemplateIdTypeCode", TemplateIdTypeCode);
                HshIn.Add("@chvTemplateName", "%" + TemplateName + "%");
                HshIn.Add("@EncounterId", EncounterId);

                ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetEMRTemplateTypeWiseConsent", HshIn);

                return ds;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
    }
}
