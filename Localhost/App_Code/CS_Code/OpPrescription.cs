using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Summary description for OpPrescription
/// </summary>
public class OpPrescription
{
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    String strConString;

    Int64 RegistrationNo;
    int HospitalLocationId;
    Int64 EncounterId;
    int DoctorId;
    String PrescriptionDate;
    String Remarks;
    Boolean UnderMedication;
    int EncodedBy;
    String xmlItems, strOPIP, strStat;


    public Int64 ProRegistrationNo
    {
        get
        {
            return RegistrationNo;
        }
        set
        {
            RegistrationNo = value;
        }
    }



    public Int32 ProHospitalLocationId
    {
        get
        {
            return HospitalLocationId;
        }
        set
        {
            HospitalLocationId = value;
        }
    }

    public Int64 ProEncounterId
    {
        get
        {
            return EncounterId;
        }
        set
        {
            EncounterId = value;
        }
    }

    public Int32 ProDoctorId
    {
        get
        {
            return DoctorId;
        }
        set
        {
            DoctorId = value;
        }
    }

    public String ProPrescriptionDate
    {
        get
        {
            return PrescriptionDate;
        }
        set
        {
            PrescriptionDate = value;
        }
    }

    public String ProRemarks
    {
        get
        {
            return Remarks;
        }
        set
        {
            Remarks = value;
        }
    }

    public Boolean ProUnderMedication
    {
        get
        {
            return UnderMedication;
        }
        set
        {
            UnderMedication = value;
        }
    }

    public Int32 ProEncodedBy
    {
        get
        {
            return EncodedBy;
        }
        set
        {
            EncodedBy = value;
        }
    }

    public String ProxmlItems
    {
        get
        {
            return xmlItems;
        }
        set
        {
            xmlItems = value;
        }
    }
    public string OPIP
    {
        get
        {
            return strOPIP;
        }
        set
        {
            strOPIP = value;
        }
    }
    public string IsStat
    {
        get
        {
            return strStat;
        }
        set
        {
            strStat = value;
        }
    }

    public OpPrescription(String constring)
    {
        strConString = constring;


    }

    public SqlDataReader GetRouteDetails()
    {
        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        //BaseC.ParseData bc = new BaseC.ParseData();
        //hshIn.Add("@intBillNo", bc.ParseQ(BillNo.ToString()));
        //hshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        //hshIn.Add("@inyYearId", bc.ParseQ(YearID.ToString()));
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ID ,RouteName from EMRMedicationRouteMaster Where Active=1 order By RouteName");

        return dr;
    }

    public DataSet dsGetRouteDetails()
    {
        DataSet ds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        try
        {
            //ds = dl.FillDataSet(CommandType.Text, "SELECT ID, RouteName, IsDefault FROM EMRMedicationRouteMaster WITH (NOLOCK) WHERE Active=1 ORDER BY SequenceNo");
            ds = dl.FillDataSet(CommandType.Text, "SELECT ID, RouteName, IsDefault FROM EMRMedicationRouteMaster WITH (NOLOCK) WHERE Active=1 ORDER BY RouteName");
        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
        return ds;
    }

    public DataSet dsGetRouteDetailsV3()
    {
        DataSet ds = new DataSet();
        //DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        try
        {
            //ds = (DataSet)dl.FillDataSet(CommandType.Text, "SELECT ID, RouteName, IsDefault FROM EMRMedicationRouteMaster WITH (NOLOCK) WHERE Active=1 ORDER BY SequenceNo");
            //ds = (DataSet)dl.FillDataSet(CommandType.Text, "SELECT ID, RouteName, IsDefault FROM EMRMedicationRouteMaster WITH (NOLOCK) WHERE Active=1 ORDER BY RouteName");

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/Masters/GetMedicationRouteMaster";

            string inputJson = (new JavaScriptSerializer()).Serialize(null);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            DataSet objDs = JsonConvert.DeserializeObject<DataSet>(sValue);

            if (sValue != "")
                ds = JsonConvert.DeserializeObject<DataSet>(sValue);


        }
        catch (Exception ex)
        {
        }
        finally
        {
        }
        return ds;
    }
    public DataSet dsGetFoodRelationShip(int FacilityId,int HospitalLocationId)
    {
        DataSet ds = new DataSet();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        Hashtable HshIn = new Hashtable();
        try
        {
            HshIn.Add("@FacilityId", FacilityId);
            HshIn.Add("@HospitalLocationId", HospitalLocationId);
	    
            ds = dl.FillDataSet(CommandType.Text, "SELECT DISTINCT ID, FoodName FROM FoodMaster WITH(NOLOCK) WHERE ISNULL(FacilityId,0) IN(@FacilityId,0) AND ISNULL(HospitalLocationId,0) IN (@HospitalLocationId,0) AND Active=1 ORDER BY FoodName", HshIn);
        }
        catch (Exception ex)
        {
        }
        finally
        {
	      dl=null;
	      HshIn=null;
        }
        return ds;
    }

    public SqlDataReader GetDrugUnitDetails()
    {

        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ID ,Description from EMRMedicationUnitMaster WITH (NOLOCK) Where Active=1");

        return dr;
    }

    public SqlDataReader GetFrequencyDetails()
    {

        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select Frequency ,Description from EMRMedicationFrequencyMaster WITH (NOLOCK) Where Active=1");

        return dr;
    }

    public SqlDataReader GetSelectedDrugDetails(string SearchStr)
    {

        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ItemId,ItemName from ItemOfInventory IOI WITH (NOLOCK) inner join Subgroupmaster SGM WITH (NOLOCK) on SGM.SubGroupId=IOI.SubGroupId inner join Groupmaster GM WITH (NOLOCK) on GM.GroupId=IOI.GroupId and GM.Type='P' Where IOI.ItemName Like'" + SearchStr + "%'");

        return dr;
    }

    public SqlDataReader GetDrugDetails(string DrugId)
    {

        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select ItemId,ItemName from ItemOfInventory IOI WITH (NOLOCK) inner join Subgroupmaster SGM WITH (NOLOCK) on SGM.SubGroupId=IOI.SubGroupId and IOI.ItemId in(" + DrugId + ") inner join Groupmaster GM WITH (NOLOCK) on GM.GroupId=IOI.GroupId and GM.Type='P' Order By ItemID");

        return dr;
    }

    public SqlDataReader GetUnit()
    {

        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select MedicineUnitID, MedicineUnitName from EMRMedicineUnit WITH (NOLOCK) where Active=1");

        return dr;
    }


    public void SetCancelRemarks(string str, int LastchangedById, string ReasonId, string OPIP)
    {
        //@chvCancelId varchar(50),
        //@inyLastChagedBy tinyint

        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);

        hshIn.Add("@chvCancelId", str);
        hshIn.Add("@inyLastChagedBy", LastchangedById);
        hshIn.Add("@ReasonId", ReasonId);
        hshIn.Add("@OPIP", OPIP);
        hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPEMRCancelPrescription", hshIn, hshOut);
    }


    //Not in Use
    //public DataSet GetFavouriteDrugDetails(int DoctorId, string SearchStr)
    //{

    //    Hashtable hshIn = new Hashtable();

    //    DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);

    //    hshIn.Add("@intDoctorId", DoctorId);
    //    hshIn.Add("@SearchStr", SearchStr);

    //    DataSet ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetFavouriteMedicines",hshIn);

    //    return ds;
    //}

    //Not in Use

    public DataSet GetPrevOrderDrugDetails(int HospitalLocationID, Int64 RegistrationNo, int DoctorId, string LastVisit, string datefrom, string dateto, Int64 EncounterNo, String OPIP)
    {
        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        BaseC.ParseData bc = new BaseC.ParseData();
        DataSet ds;
        if (OPIP == "O")
        {
            hshIn.Add("@inyHospitalLocationID", HospitalLocationID);
            hshIn.Add("@intRegistrationNo", RegistrationNo);
            hshIn.Add("@intDoctorId", DoctorId);

            hshIn.Add("@inyLastVisits", LastVisit);
            hshIn.Add("@chrDateFrom", datefrom);
            hshIn.Add("@chrDateTo", dateto);
            hshIn.Add("@intEncounterNo", EncounterNo);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousMedicines", hshIn);
        }
        else
        {
            hshIn.Add("@intEncounterNo", EncounterNo);
            ds = dl.FillDataSet(CommandType.StoredProcedure, "UspEMRGetPreviousMedicinesIP", hshIn);
        }
        return ds;
    }


    public String SaveOPPrescription()
    {

        //@registrationNo int,
        //@HospitalLocationId tinyint,
        //@EncounterId Int,
        //@DoctorId Int,
        //@PrescriptionDate SmallDateTime,
        //@Remarks Varchar(100),
        //@UnderMedication Bit,
        //@EncodedBy Int,

        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();
        BaseC.ParseData bc = new BaseC.ParseData();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        hshIn.Add("@registrationNo", RegistrationNo);
        hshIn.Add("@HospitalLocationId", HospitalLocationId);
        hshIn.Add("@EncounterId", EncounterId);
        hshIn.Add("@DoctorId", DoctorId);
        hshIn.Add("@PrescriptionDate", PrescriptionDate);
        hshIn.Add("@Remarks", Remarks);
        hshIn.Add("@UnderMedication", UnderMedication);
        hshIn.Add("@EncodedBy", EncodedBy);
        hshIn.Add("@xmlItems", xmlItems);

        hshOut.Add("@ErrorMsg", SqlDbType.VarChar);
        hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "USPSavePrescription", hshIn, hshOut);
        return hshOut["@ErrorMsg"].ToString();
    }
    public SqlDataReader GetCancelRemarks()
    {

        Hashtable hshIn = new Hashtable();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        SqlDataReader dr = (SqlDataReader)dl.ExecuteReader(CommandType.Text, "select Id,Description from  EMRPrescriptionCancelReasonMaster WITH (NOLOCK) Where Active=1");

        return dr;
    }

    public string SaveEMROTRequest(int OTRequestID, bool IsEmergency, int EncounterId, int RegistrationId, string FirstName, string MiddleName,
                            string LastName, DateTime DateOfBirth, int AgeYears, int AgeMonths, int AgeDays, int Gender, string MobileNo, int OTDuration,
                            String OTDurationType, DateTime OTBookingDate, DateTime FromTime, string Remarks, bool Active, int EncodedBy,
                            DateTime EncodedDate, int LastChangedBy, DateTime LastChangedDate, String xmlServiceDetails, string xmlResourceDetails, 
                            string Diagnosis, int Anaesthesia,bool IsInfectiousCase, string InfectiousCaseRemarks, int OTTheaterId, bool PACRequired)
    {
        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();
        BaseC.ParseData bc = new BaseC.ParseData();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        hshIn.Add("@intOTRequestID", OTRequestID);
        hshIn.Add("@bitIsEmergency", IsEmergency);
        hshIn.Add("@intEncounterId", EncounterId);
        hshIn.Add("@intRegistrationId", RegistrationId);
        hshIn.Add("@chvFirstName", FirstName);
        hshIn.Add("@chvMiddleName", MiddleName);
        hshIn.Add("@chvLastName", LastName);
        hshIn.Add("@dtDateOfBirth", DateOfBirth);
        hshIn.Add("@intAgeYears", AgeYears);
        hshIn.Add("@intAgeMonths", AgeMonths);
        hshIn.Add("@intAgeDays", AgeDays);
        hshIn.Add("@intGender", Gender);
        hshIn.Add("@chvMobileNo", MobileNo);
        hshIn.Add("@intOTDuration", OTDuration);
        hshIn.Add("@intOTDurationType", OTDurationType);
        hshIn.Add("@dtOTBookingDate", OTBookingDate);
        hshIn.Add("@dtFromTime", FromTime);
        hshIn.Add("@chvRemarks", Remarks);
        hshIn.Add("@bitActive", Active);
        hshIn.Add("@intEncodedBy", EncodedBy);
        hshIn.Add("@dtEncodedDate", EncodedDate);
        hshIn.Add("@intLastChangedBy", LastChangedBy);
        hshIn.Add("@dtLastChangedDate", LastChangedDate);
        hshIn.Add("@xmlServiceDetails", xmlServiceDetails);
        hshIn.Add("@xmlResourceDetails", xmlResourceDetails);
        hshIn.Add("@chvDiagnosis", Diagnosis);
        hshIn.Add("@intAnesthesiaId", Anaesthesia);
        hshIn.Add("@bitIsInfectiousCase", IsInfectiousCase);
        hshIn.Add("@chvInfectiousCaseRemarks", InfectiousCaseRemarks);
        hshIn.Add("@intOTTheaterId", OTTheaterId);
        hshIn.Add("@IsPACRequired", PACRequired);
        hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        hshOut.Add("@intId", SqlDbType.Int);

        hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRSaveOTRequest", hshIn, hshOut);
        return hshOut["@chvErrorStatus"].ToString();

    }


    public DataSet getOTBookingDetails(int OTRequestID)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        DataSet ds = new DataSet();
        Hashtable hshIn = new Hashtable();
        hshIn.Add("@OTRequestID", OTRequestID);
        ds = dl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetOTRequest", hshIn);
        return ds;
    }


    public DataSet GetEMROTRequest(Int64 RegistrationNo)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        DataSet ds = new DataSet();
        Hashtable hshIn = new Hashtable();
        hshIn.Add("@inRegistrationNo", RegistrationNo);
        ds = dl.FillDataSet(CommandType.StoredProcedure, "uspEMRGetPatientOTRequests", hshIn);
        return ds;
    }

    public string DeleteEMROTRequest(int OTRequestID, int EncodedBy)
    {
        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();
        BaseC.ParseData bc = new BaseC.ParseData();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        hshIn.Add("@intOTRequestID", OTRequestID);
        hshIn.Add("@intEncodedby", EncodedBy);
        hshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
        hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspEMRDeleteOTRequest", hshIn, hshOut);
        return hshOut["@chvErrorOutput"].ToString();

    }

    public string DeleteEMROTRequestServices(int id, int EncodedBy)
    {
        Hashtable hshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();
        BaseC.ParseData bc = new BaseC.ParseData();
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        hshIn.Add("@intId", id);
        hshIn.Add("@intEncodedby", EncodedBy);
        hshOut.Add("@chvErrorOutput", SqlDbType.VarChar);
        hshOut = dl.getOutputParametersValues(CommandType.StoredProcedure, "uspDeleteEMROTRequestServices", hshIn, hshOut);
        return hshOut["@chvErrorOutput"].ToString();

    }

    public DataSet GetOTScheduler(int HospId, int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();

        HshIn.Add("@inyHospitalLocationId", HospId);
        if (FacilityId == 0)
        {
            HshIn.Add("@intFacilityId", "0");
        }
        else
        {
            HshIn.Add("@intFacilityId", FacilityId);
        }
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspOTDisplayTV", HshIn);

        return ds;
    }

    public DataSet GetAdmissionCurrentLocation(Int64 RegistrationNo,string EncounterNo, int HospitalLocationId, int FacilityId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        HshIn.Add("@intRegistrationNo", RegistrationNo);
        HshIn.Add("@chvEncounterNo", EncounterNo);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        HshIn.Add("@intFacilityId", FacilityId);
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetCurrentLocation", HshIn);

        return ds;
    }

    public String UpdateAdmissionPatientLocation(Int64 RegistrationNo, string EncounterNo, int HospitalLocationId, int FacilityId,int PatientLocationId)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, strConString);
   
        Hashtable HshIn = new Hashtable();
        Hashtable hshOut = new Hashtable();
        HshIn.Add("@intRegistrationNo", RegistrationNo);
        HshIn.Add("@chvEncounterNo", EncounterNo);
        HshIn.Add("@inyHospitalLocationId", HospitalLocationId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@intPatientLocationId", PatientLocationId);
        hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateAdmissionPatientLocation", HshIn, hshOut);
        return hshOut["@chvErrorStatus"].ToString();

    }
}
