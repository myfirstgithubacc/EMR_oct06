using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace BaseC
{
    public class AuditTrail
    {
        string sConString = "";
        public AuditTrail(string Constring)
        {
            sConString = Constring;
        }
        public DataSet GetAuditTrailTemplate(int HospitalLocationId, int LoginFacilityId, int FacilityId, string RegistrationNo, string EncounterNo,
            string TemplateName, string SectionName, string FieldName, string FromDate, string ToDate, string Options)
        {
            DataSet ds = new DataSet();
            Hashtable HshIn = new Hashtable();
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            try
            {
                HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
                HshIn.Add("@intLoginFacilityId", LoginFacilityId);
                HshIn.Add("@intFacilityId", FacilityId);

                if (RegistrationNo != "")
                    HshIn.Add("@chvRegistrationNo", RegistrationNo);

                if (EncounterNo != "")
                    HshIn.Add("@chvEncounterNo", EncounterNo);

                if (TemplateName != "")
                    HshIn.Add("@chvTemplateName", TemplateName);

                if (SectionName != "")
                    HshIn.Add("@chvSectionName", SectionName);

                if (FieldName != "")
                    HshIn.Add("@chvFieldName", FieldName);

                HshIn.Add("@dtFromDate", FromDate);
                HshIn.Add("@dtToDate", ToDate);
                HshIn.Add("@chvOptions", Options);
                ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspEMRTemplateAuditTrail", HshIn);
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
            return ds;
        }
    }
}
