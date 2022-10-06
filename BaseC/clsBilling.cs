using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for clsBilling
/// </summary>
public class clsBilling
{
    string sConString = string.Empty;
    DataSet ds;
    Hashtable HshIn, hshOut;

    public clsBilling(string Constring)
    {
        sConString = Constring;
        //
        // TODO: Add constructor logic here
        //
    }

    public DataSet GetInvoiceCreditNoteDump(int HospitalLocationID, int FacilityId, string RegNo, string FromDate, string ToDate, string PatientName, int ApprovalStatusId, int Encodedby)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn = new Hashtable();
        HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intRegistrationNo", RegNo);
        HshIn.Add("@chvDateFrom", FromDate);
        HshIn.Add("@chvDateTo", ToDate);
        HshIn.Add("@chvPatientName", PatientName);
        HshIn.Add("@intApprovalStatusId", ApprovalStatusId);
        HshIn.Add("@intencodedby", Encodedby);
        try
        {

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetInvoiceCreditNoteDump", HshIn);

        }
        catch (Exception ex) { throw ex; }
        finally { HshIn = null; objDl = null; }

    }

    public string SaveCreditNoteApprovalDump(int HospId, int FacilityId, string xmlDumpRefunds, int EncodedBy, string spName)
    {
        HshIn = new Hashtable();
        hshOut = new Hashtable();

        HshIn.Add("@inyHospitalLocationId", HospId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@xmlDumpRefunds", xmlDumpRefunds);
        HshIn.Add("@intEncodedBy", EncodedBy);
        hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, spName, HshIn, hshOut);

            return hshOut["@chvErrorStatus"].ToString();
        }
        catch (Exception ex) { throw ex; }
        finally { HshIn = null; objDl = null; }


    }

    public string SavePatientOutstandingApproval(int HospId, int FacilityId, int RegId, int EncId, int IsApproved, string Remarks, int EncodedBy)
    {
        HshIn = new Hashtable();
        hshOut = new Hashtable();
        HshIn.Add("@inyHospitalLocationId", HospId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intRegistrationId", RegId);
        HshIn.Add("@intEncounterId", EncId);
        HshIn.Add("@intIsApproved", IsApproved);
        HshIn.Add("@chvRemarks", Remarks);
        HshIn.Add("@intEncodedBy", EncodedBy);
        hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {


            hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspSavePatientOutstandingApproval", HshIn, hshOut);
            return hshOut["@chvErrorStatus"].ToString();
        }
        catch (Exception ex) { throw ex; }
        finally { HshIn = null; objDl = null; }

    }

    public DataSet GetPatientOutstandingApproval(int HospitalLocationID, int FacilityId, int RegId, int EncId, int Encodedby)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn = new Hashtable();
        HshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intRegistrationId", RegId);
        HshIn.Add("@intEncounterId", EncId);
        HshIn.Add("@intencodedby", Encodedby);
        try
        {

            return objDl.FillDataSet(CommandType.StoredProcedure, "uspGetPatientOutstandingApproval", HshIn);
        }
        catch (Exception ex) { throw ex; }
        finally { HshIn = null; objDl = null; }


    }


}