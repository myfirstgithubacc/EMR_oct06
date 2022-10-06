using System;
using System.Data;
using System.Collections;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

/// <summary>
/// Summary description for clsReport
/// </summary>
public class clsReport
{
    //string sConString = string.Empty;
    private static string WebAPIAddress = System.Configuration.ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    DataSet ds;
    Hashtable hshIn, hshOut;
    //public clsReport(string Constring)
    //{
    //    sConString = Constring;
    //}
    public clsReport()
    {
    }
    public DataSet Get_Rpt_MRD_CountryWiseVisit(int HospitalLocationID, int FacilityId, string NationalityIds, string FromDate, string ToDate, string VisitType)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();
        //hshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@chvNationalityIds", NationalityIds);
        //hshIn.Add("@chrFromDate", FromDate);
        //hshIn.Add("@chrToDate", ToDate);
        //hshIn.Add("@VisitType", VisitType);

        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "usp_Rpt_MRD_CountryWiseVisit", hshIn);
        

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Common/Get_Rpt_MRD_CountryWiseVisit";
        APIRootClass.Get_Rpt_MRD_CountryWiseVisit objRoot = new global::APIRootClass.Get_Rpt_MRD_CountryWiseVisit();
        objRoot.HospitalLocationId = HospitalLocationID;
        objRoot.FacilityId = FacilityId;
        objRoot.NationalityIds = NationalityIds;
        objRoot.FromDate = FromDate;
        objRoot.ToDate = ToDate;
        objRoot.VisitType = VisitType;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);

        return ds;

    }

    public DataSet Get_Rpt_GetVisitSummary(int HospitalLocationID, int FacilityId, string FromDate, string ToDate, string VisitType)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();
        //hshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@chrDate", FromDate);
        //hshIn.Add("@chrToDate", ToDate);
        //hshIn.Add("@VisitType", VisitType);
        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "usp_Rpt_GetVisitSummary", hshIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/Get_Rpt_GetVisitSummary";
        APIRootClass.Get_Rpt_GetVisitSummary objRoot = new global::APIRootClass.Get_Rpt_GetVisitSummary();
        objRoot.HospitalLocationId = HospitalLocationID;
        objRoot.FacilityId = FacilityId;
        objRoot.FromDate = FromDate;
        objRoot.ToDate = ToDate;
        objRoot.VisitType = VisitType;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        return ds;
    }

    public DataSet GetDashboardRecievable(int HospitalLocationID, int FacilityId, string FromDate, string ToDate, string sCompanyIds, string ReportType, string sYearids, int InvoiceId )
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();
        //hshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@chvCompanyIds", sCompanyIds);
        //hshIn.Add("@chvFromDate", FromDate);
        //hshIn.Add("@chvToDate", ToDate);
        //hshIn.Add("@chvReportType", ReportType);
        //hshIn.Add("@chvYearId", sYearids);
        //hshIn.Add("@intInvoiceId", InvoiceId);
        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDashboardRecievable", hshIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetDashboardRecievable";
        APIRootClass.GetDashboardRecievable objRoot = new global::APIRootClass.GetDashboardRecievable();
        objRoot.HospitalLocationId = HospitalLocationID;
        objRoot.FacilityId = FacilityId;
        objRoot.FromDate = FromDate;
        objRoot.ToDate = ToDate;
        objRoot.CompanyIds = sCompanyIds;
        objRoot.ReportType = ReportType;
        objRoot.Yearids= sYearids;
        objRoot.InvoiceId = InvoiceId;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);

        return ds;
    }

    public DataSet GetYears()
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);       
        //ds = (DataSet)objDl.FillDataSet(CommandType.Text, "Select Id, Year from Years Where Active = 1");

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetYears";

        string inputJson = (new JavaScriptSerializer()).Serialize(null);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);

        return ds;
    }

    public DataSet GetDashboardRecievableDetails(int HospitalLocationID, int FacilityId, string FromDate, string ToDate, string ReportType,int CompanyId, int SubCompanyId, string YearMonth, string sCompanyIds, string syear)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();
        //hshIn.Add("@inyHospitalLocationId", HospitalLocationID);
        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@intCompanyId", CompanyId);
        //hshIn.Add("@intSubCompanyId", SubCompanyId);
        //hshIn.Add("@chvFromDate", FromDate);
        //hshIn.Add("@chvToDate", ToDate);
        //hshIn.Add("@chvReportType", ReportType);
        //hshIn.Add("@chvYearMonth", YearMonth);
        //hshIn.Add("@chvCompanyIds", sCompanyIds);
        //hshIn.Add("@chvYear", syear);

        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDashboardRecievableDetail", hshIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetDashboardRecievableDetails";
        APIRootClass.GetDashboardRecievableDetails objRoot = new global::APIRootClass.GetDashboardRecievableDetails();
        objRoot.HospitalLocationId = HospitalLocationID;
        objRoot.FacilityId = FacilityId;
        objRoot.FromDate = FromDate;
        objRoot.ToDate = ToDate;
        objRoot.ReportType = ReportType;
        objRoot.CompanyId = CompanyId;
        objRoot.SubCompanyId = SubCompanyId;
        objRoot.CompanyIds = sCompanyIds;
        objRoot.YearMonth = YearMonth;
        objRoot.Year = syear;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);

        return ds;
    }
    public DataSet GetDischargeChecklistMaster(int FacilityId,int RegistrationId,int EncounterId, int StatusId)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();

        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@intRegistrationId", RegistrationId);
        //hshIn.Add("@intEncounter", EncounterId);
        //hshIn.Add("@intStatusId", StatusId);
        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDischargeChecklistMaster", hshIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetDischargeChecklistMaster";
        APIRootClass.GetDischargeChecklistMaster objRoot = new global::APIRootClass.GetDischargeChecklistMaster();
        objRoot.FacilityId = FacilityId;
        objRoot.RegistrationId = RegistrationId;
        objRoot.EncounterId = EncounterId;
        objRoot.StatusId = StatusId;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);


        return ds;
    }
    public DataSet GetDischargeChecklistDetail(int EncounterId, int CheckListId)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();

        //hshIn.Add("@intEncounterId", EncounterId);
        //hshIn.Add("@intChecklistId", CheckListId);
        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDischargeChecklistDetail", hshIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetDischargeChecklistDetail";
        APIRootClass.GetDischargeChecklistDetail objRoot = new global::APIRootClass.GetDischargeChecklistDetail();
        objRoot.ChecklistId = CheckListId;
        objRoot.EncounterId = EncounterId;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        return ds;
    }

    public string UpdateEncouterStatus(int EncounterId, int HospId, int FacilityId, int RegistrationId, int StatusId, int UserId, DateTime? EDod, int DischargeStatus, string xmlChecklist)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();
        //hshOut = new Hashtable();

        //hshIn.Add("@intEncounterId", EncounterId);
        //hshIn.Add("@inyHospitalLocationId", HospId);
        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@intRegistrationId", RegistrationId);
        //hshIn.Add("@intStatusId", StatusId);
        //hshIn.Add("@intEncodedBy", UserId);
        //if (EDod != null)
        //{
        //    hshIn.Add("@ExpectedDateOfDischarge", EDod);
        //    hshIn.Add("@intDischarStatus", DischargeStatus);
        //}
        //if(xmlChecklist != "")
        //{
        //    hshIn.Add("@xmlCheckListFlag", xmlChecklist);
        //}
        //hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "uspUpdateEncouterStatus", hshIn, hshOut);

        //return hshOut["@chvErrorStatus"].ToString();

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/UpdateEncouterStatus";
        APIRootClass.UpdateEncouterStatus objRoot = new global::APIRootClass.UpdateEncouterStatus();
        objRoot.EncounterId = EncounterId;
        objRoot.HospitalLocationId = HospId;
        objRoot.FacilityId = FacilityId;
        objRoot.RegistrationId = RegistrationId;
        objRoot.StatusId = StatusId;
        objRoot.UserId = UserId;
        objRoot.EDod = EDod;
        objRoot.DischargeStatus = DischargeStatus;
        objRoot.xmlChecklist = xmlChecklist;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        return JsonConvert.DeserializeObject<string>(sValue);
    }


    public DataSet GetTopPrescriptionListBasedOnICDCodes(int EncounterId)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();

        //hshIn.Add("@intEncounterId", EncounterId);
        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTopPrescriptionListBasedOnICDCodes", hshIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetTopPrescriptionListBasedOnICDCodes";
        APIRootClass.GetTopPrescriptionListBasedOnICDCodes objRoot = new global::APIRootClass.GetTopPrescriptionListBasedOnICDCodes();
        objRoot.EncounterId = EncounterId;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        return ds;
    }

    public DataSet GetTopOrderListBasedOnICDCodes(int EncounterId)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();

        //hshIn.Add("@intEncounterId", EncounterId);
        //ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "uspGetTopOrderListBasedOnICDCodes", hshIn);
        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetTopOrderListBasedOnICDCodes";
        APIRootClass.GetTopOrderListBasedOnICDCodes objRoot = new global::APIRootClass.GetTopOrderListBasedOnICDCodes();
        objRoot.EncounterId = EncounterId;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);
        return ds;
    }

    public DataSet GetServiceDetail(int HospId, int intFacilityId, int intRegistrationId, int intEncounterId, int intServiceId,
       int intorderSetId, int intCompanyid, int intSponsorId, int intCardId, int Option, int intTemplateId, string xmlServiceIds)
    {

        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //Hashtable hsIn = new Hashtable();

        //hsIn.Add("@inyHospitalLocationID", HospId);
        //hsIn.Add("@intFacilityId", intFacilityId);
        //hsIn.Add("@intRegistrationId", intRegistrationId);
        //hsIn.Add("@intEncounterId", intEncounterId);
        //hsIn.Add("@intServiceId", intServiceId);
        //hsIn.Add("@intorderSetId", intorderSetId);
        //hsIn.Add("@intCompanyid", intCompanyid);
        //hsIn.Add("@intSponsorId", intSponsorId);
        //hsIn.Add("@intCardId", intCardId);
        //hsIn.Add("@Option", Option);
        //hsIn.Add("@intTemplateId", intTemplateId);
        //hsIn.Add("@xmlServiceIds", xmlServiceIds);

        //DataSet ds = (DataSet)objDl.FillDataSet(CommandType.StoredProcedure, "UspGetServiceDescriptionForOrderpage", hsIn);

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/GetServiceDetail";
        APIRootClass.GetServiceDetail objRoot = new global::APIRootClass.GetServiceDetail();
        objRoot.HospitalLocationId = HospId;
        objRoot.FacilityId = intFacilityId;
        objRoot.RegistrationId = intRegistrationId;
        objRoot.EncounterId = intEncounterId;
        objRoot.ServiceId = intServiceId;
        objRoot.orderSetId = intorderSetId;
        objRoot.CompanyId = intCompanyid;
        objRoot.SponsorId = intSponsorId;
        objRoot.CardId = intCardId;
        objRoot.Option = Option;
        objRoot.TemplateId = intTemplateId;
        objRoot.xmlServiceIds = xmlServiceIds;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        ds = JsonConvert.DeserializeObject<DataSet>(sValue);


        return ds;
    }

    public string DietUpdateOrder(int HospId, int FacilityId, int DietOrderId, string StatusCode, int UserId)
    {
        //DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        //hshIn = new Hashtable();
        //hshOut = new Hashtable();
        //hshIn.Add("@inyHospitalLocationId", HospId);
        //hshIn.Add("@intFacilityId", FacilityId);
        //hshIn.Add("@intId", DietOrderId);
        //hshIn.Add("@chrStatusCode", StatusCode);
        //hshIn.Add("@intEncodedBy", UserId);      
        //hshOut.Add("@chvErrorStatus", SqlDbType.VarChar);
        //hshOut = objDl.getOutputParametersValues(CommandType.StoredProcedure, "USPDietUpdateOrder", hshIn, hshOut);
        //return hshOut["@chvErrorStatus"].ToString();

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Reports/DietUpdateOrder";
        APIRootClass.DietUpdateOrder objRoot = new global::APIRootClass.DietUpdateOrder();
        objRoot.HospitalLocationId = HospId;
        objRoot.FacilityId = FacilityId;
        objRoot.DietOrderId = DietOrderId;
        objRoot.StatusCode = StatusCode;
        objRoot.UserId = UserId;

        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        string sValue = client.UploadString(ServiceURL, inputJson);
        sValue = JsonConvert.DeserializeObject<string>(sValue);
        return JsonConvert.DeserializeObject<string>(sValue);
    }


}