<%@ WebService Language="C#" Class="PrescriptionService" %>
using System;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Configuration;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class PrescriptionService : System.Web.Services.WebService
{

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetFavoriteItems(string itemName)
    {


        DataSet dsSearch = new DataSet();
        BaseC.clsEMR objEMR = new BaseC.clsEMR(common.getConnection());
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
            dsSearch = objEMR.getFavoriteDrugWithStock(common.myInt(Session["HospitalLocationID"]),
                                common.myInt(Session["FacilityId"]), common.myInt(Session["UserId"]), ItemId,
                                GenericId, DoctorId, string.Empty, itemName.Replace("'", "''"));
            foreach (DataRow dr in dsSearch.Tables[0].Rows)
            {
                var favorite = new PrescriptionFavoriteItem();
                favorite.FavoriteId = common.myInt(dr["FavoriteId"]);
                favorite.CIMSItemId = common.myStr(dr["CIMSItemId"]);
                favorite.CIMSType = common.myStr(dr["CIMSType"]);
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


        BaseC.EMRMasters objMst = new BaseC.EMRMasters(common.getConnection());
        DataSet ds = new DataSet();
        DataView DV = new DataView();
        var result = string.Empty;
        try
        {

            ds = objMst.GetEMRDrugSet(common.myInt(Session["HospitalLocationId"]), 0);

            if (ds.Tables.Count > 0)
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
            objMst = null;
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
            BaseC.clsEMR objEMR = new BaseC.clsEMR(common.getConnection());
            var favoriteItems = new List<PrescriptionFavoriteItem>();

            dsSearch = objEMR.getOPMedicinesNew(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                              common.myInt(Session["EncounterId"]), common.myInt(Session["RegistrationId"]), 0, 0, "P",
                              common.myStr(itemName).Trim(), string.Empty, string.Empty);


            var currentMedicationList = new List<CurrentMedicationItem>();
            foreach (DataRow dr in dsSearch.Tables[0].Rows)
            {
                var cm = new CurrentMedicationItem()
                {
                    ItemId = dr["ItemId"].ToString(),
                    ItemName = dr["ItemName"].ToString().Replace("'", ""),
                    GenericId = dr["GenericId"].ToString(),
                    GenericName = dr["GenericName"].ToString().Replace("'", ""),
                    CIMSItemId = dr["CIMSItemId"].ToString(),
                    CIMSType = dr["CIMSType"].ToString(),
                    VIDALItemId = dr["VIDALItemId"].ToString(),
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



    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<setNotification> setNotification(string  input)
    {
        BaseC.ClsExternal objExt = new BaseC.ClsExternal(common.getConnection());
        DataSet ds = new DataSet();
        var NotificationItem = new List<setNotification>();
        try
        {
            ds = objExt.getUserGroupNotification(common.myInt(Session["GroupId"]), common.myInt(Session["FacilityId"]));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var Notification = new setNotification();
                Notification.GroupNotification = common.myStr(dr["GroupNotification"]);
                Notification.LinkURL = common.myStr(dr["LinkURL"]);

                NotificationItem.Add(Notification);
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
            objExt = null;
            ds.Dispose();

        }
        var result = NotificationItem as List<setNotification>;
        if (result == null)
            return new List<setNotification>();
        else
            return result;
    }

}
public class setNotification
{

    public string GroupNotification { get; set; }
    public string LinkURL { get; set; }
}
