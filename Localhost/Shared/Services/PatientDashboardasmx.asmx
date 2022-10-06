<%@ WebService Language="C#" Class="PatientDashboardasmx" %>

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Script.Services;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PatientDashboardasmx  : System.Web.Services.WebService {

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }

    /// <summary>
    /// To Get the EMR template calculated graph
    /// </summary>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<object> GetEMRTemplateCalculatedGraph(string fieldId, string fromDate, string toDate)
    {

        DataSet ds = new DataSet();
        BaseC.Patient objPatient = new BaseC.Patient(common.getConnection());
        clsExceptionLog objException = new clsExceptionLog();
        List<object> GraphList = new List<object>();

        try
        {
            ds = objPatient.GetEMRTemplateCalculatedGraph(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]), common.myInt(fieldId), common.myDate(fromDate), common.myDate(toDate));
            if (ds.Tables.Count > 0)
            {

                List<object> iData = new List<object>();

                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in ds.Tables[0].Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                GraphList.Add(iData);


            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            objException = null;
            objPatient = null;

        }
        return GraphList;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetEWSData()
    {
        DataSet ds = new DataSet();
        BaseC.Patient objPatient = new BaseC.Patient(common.getConnection());
        clsExceptionLog objException = new clsExceptionLog();
        var data = string.Empty;
        try
        {
            ds = objPatient.GetEWSMasterData();

            if (ds.Tables.Count > 0)
            {

                data= JsonConvert.SerializeObject(ds.Tables[0]);


            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            objException = null;

        }


        return data;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<object> GetEWSGraphData(int templateId)
    {

        DataSet ds = new DataSet();
        BaseC.Patient objPatient = new BaseC.Patient(common.getConnection());
        clsExceptionLog objException = new clsExceptionLog();
        List<object> GraphList = new List<object>();

        try
        {
            ds = objPatient.GetEMREWSGraphData(common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),templateId);

            // ds = objDal.FillDataSet(CommandType.Text, "select  top 20  ValueText,convert(varchar(10), DATEADD(mi,330, EncodedDate),103)+' '+convert(varchar(5), DATEADD(mi,330, EncodedDate),108) EncodedDate,'EWS Graph' GraphTitle from EMRPatientNotesData where fieldId in(160636,160636) and valueText not in(0,1) order by RegistrationId, EncodedDate desc");
            if (ds.Tables.Count > 0)
            {

                List<object> iData = new List<object>();

                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    List<object> x = new List<object>();
                    x = (from DataRow drr in ds.Tables[0].Rows select drr[dc.ColumnName]).ToList();
                    iData.Add(x);
                }
                GraphList.Add(iData);


            }
        }
        catch (Exception ex)
        {
            objException.HandleException(ex);
        }
        finally
        {
            ds.Dispose();
            objException = null;
            objPatient = null;

        }
        return GraphList;
    }
    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetFavoriteItems(string itemName)
    {


        DataSet dsSearch = new DataSet();
        //BaseC.clsEMR objEMR = new BaseC.clsEMR(common.getConnection());
        var favoriteItems = new List<PrescriptionFavoriteItem>();

        string data = string.Empty;
        int GenericId = 0;
        int ItemId = 0;
        int DoctorId = 0;

        if (DoctorId == 0)
        {
            DoctorId = common.myInt(Session["LoginDoctorId"]);
        }
        if (DoctorId == 0)
        {
            DoctorId = common.myInt(Session["EmployeeId"]);
        }


        try
        {
            string WebAPIAddress = common.getConnectionV3();

            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            APIRootClass.getFavoriteDrugWithStock objRoot = new global::APIRootClass.getFavoriteDrugWithStock();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.UserId = common.myInt(Session["UserId"]);
            objRoot.ItemId = ItemId;
            objRoot.GenericId = GenericId;
            objRoot.DoctorId = common.myInt(DoctorId);
            objRoot.FormularyType = string.Empty;
            objRoot.ItemName = itemName.Replace("'", "''");

            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getFavoriteDrugWithStock";
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);


            //dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
            //                    common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), ItemId,
            //                    GenericId, DoctorId, string.Empty, itemName.Replace("'", "''"));
            foreach (DataRow dr in dsSearch.Tables[0].Rows)
            {
                var favorite = new PrescriptionFavoriteItem();
                favorite.FavoriteId = common.myInt(dr["FavoriteId"]);
                favorite.CIMSItemId = common.myStr(dr["CIMSItemId"]);
                favorite.ClosingBalance = common.myInt(dr["ClosingBalance"]);
                favorite.DDCCode = dr["DDCCode"].ToString();
                favorite.Dose = common.myInt(dr["Dose"]);
                favorite.Duration = common.myInt(dr["Duration"]);
                favorite.DurationType = dr["DurationType"].ToString();
                favorite.FoodRelationshipId = common.myInt(dr["FoodRelationshipId"]); ;
                favorite.FormulationId = common.myInt(dr["FormulationId"]); ;
                favorite.FrequencyId = common.myInt(dr["FrequencyId"]);
                favorite.GenericId = common.myInt(dr["GenericId"]);
                favorite.Instructions = dr["Instructions"].ToString();
                favorite.ItemId = common.myInt(dr["ItemId"]);
                favorite.ItemName = dr["ItemName"].ToString().Replace("'", "");
                favorite.ItemNo = dr["ItemNo"].ToString();
                favorite.ItemSubCategoryShortName = dr["ItemSubCategoryShortName"].ToString().Replace("'", "");
                favorite.ItemWithStock = dr["ItemWithStock"].ToString().Replace("'", "");
                favorite.RouteId = common.myInt(dr["RouteId"]);
                favorite.StrengthId = common.myInt(dr["StrengthId"]);
                favorite.StrengthValue = dr["StrengthValue"].ToString();
                favorite.UnitId = common.myInt(dr["UnitId"]);
                favorite.VIDALItemId = common.myStr(dr["VIDALItemId"]);
                favorite.GenericName = dr["GenericName"].ToString().Replace("'", "");
                favoriteItems.Add(favorite);
            }
            data = JsonConvert.SerializeObject(favoriteItems);
        }
        catch (Exception ex)
        {
            clsExceptionLog objException = new clsExceptionLog();

            objException.HandleException(ex);
            objException = null;
        }
        finally
        {
            dsSearch.Dispose();

        }
        return data;

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetOrderSet(string orderSetName)
    {


        //BaseC.EMRMasters objMst = new BaseC.EMRMasters(common.getConnection());
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        var result = string.Empty;
        try
        {

            //ds = objMst.GetEMRDrugSet(common.myInt(Session["HospitalLocationId"]), 0);
            string WebAPIAddress = common.getConnectionV3();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            string ServiceURL = WebAPIAddress.ToString() + "api/Masters/GetMedicationSetDetails";
            APIRootClass.GetMedicationSetDetails objPatientProblem = new global::APIRootClass.GetMedicationSetDetails();
            objPatientProblem.HospitalLocationId = common.myInt(Session["HospitalLocationId"]);
            objPatientProblem.MedicationSetId = 0;
            objPatientProblem.ItemId = 0;
            string inputJson = (new JavaScriptSerializer()).Serialize(objPatientProblem);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);

            ds = JsonConvert.DeserializeObject<DataSet>(sValue);


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DV = ds.Tables[0].DefaultView;
                DV.RowFilter = "DetailActive=1 AND SetName LIKE '%" + common.myStr(orderSetName) + "%'";


                result = JsonConvert.SerializeObject(DV.ToTable());
            }
            else
            {
                result = JsonConvert.SerializeObject(ds.Tables[0]);
            }
        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {
            //objMst = null;
            ds.Dispose();
            DV.Dispose();
        }
        return result;
    }

    [WebMethod(EnableSession = true)]
    public List<CurrentMedicationItem> GetCurrentMediation(string itemName)
    {

        try
        {
            //var result = Session["currentMedication"] as List<CurrentMedicationItem>;
            //if (result == null)
            //    return new List<CurrentMedicationItem>();
            //else
            //    return result;


            DataSet dsSearch = new DataSet();
            //BaseC.clsEMR objEMR = new BaseC.clsEMR(common.getConnection());
            var favoriteItems = new List<PrescriptionFavoriteItem>();

            //dsSearch = objEMR.getOPMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
            //                  common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0,  "P" ,
            //                  common.myStr(itemName).Trim(), string.Empty, string.Empty);
            string WebAPIAddress = common.getConnectionV3();
            WebClient client = new WebClient();
            client.Headers["Content-type"] = "application/json";
            client.Encoding = Encoding.UTF8;
            APIRootClass.getOPMedicinesNew objRoot = new global::APIRootClass.getOPMedicinesNew();
            objRoot.HospitalLocationId = common.myInt(Session["HospitalLocationID"]);
            objRoot.FacilityId = common.myInt(Session["FacilityId"]);
            objRoot.RegistrationId = common.myInt(Session["RegistrationId"]);
            objRoot.EncounterId = common.myInt(Session["EncounterId"]);
            objRoot.IndentId = 0;
            objRoot.ItemId = 0;
            objRoot.PreviousMedication = "p";
            objRoot.ItemName = common.myStr(itemName);
            objRoot.FromDate = string.Empty;
            objRoot.ToDate = string.Empty;
            string ServiceURL = WebAPIAddress.ToString() + "api/EMRAPI/getOPMedicinesNew";
            string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
            string sValue = client.UploadString(ServiceURL, inputJson);
            sValue = JsonConvert.DeserializeObject<string>(sValue);
            dsSearch = JsonConvert.DeserializeObject<DataSet>(sValue);


            var currentMedicationList = new List<CurrentMedicationItem>();
            foreach (DataRow dr in dsSearch.Tables[0].Rows)
            {
                var cm = new CurrentMedicationItem()
                {
                    ItemId = dr["ItemId"].ToString(),
                    ItemName = dr["ItemName"].ToString().Replace("'", ""),
                    GenericId = dr["GenericId"].ToString(),
                    GenericName = dr["GenericName"].ToString().Replace("'", ""),
                };
                currentMedicationList.Add(cm);
            }

            //if (currentMedicationList.Count > 0)
            //{
            //    Session["currentMedication"] = currentMedicationList;
            //}
            var result = currentMedicationList as List<CurrentMedicationItem>;
            if (result == null)
                return new List<CurrentMedicationItem>();
            else
                return result;


        }
        catch (Exception Ex)
        {
            clsExceptionLog objException = new clsExceptionLog();
            objException.HandleException(Ex);
            objException = null;
        }
        finally
        {

        }
        return new List<CurrentMedicationItem>(); ;
    }
}