using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;

using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for Util
/// </summary>
public class Util
{
    public Util()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int MyProperty { get; set; }
    public static string GetApiUrl()
    {
        return ConfigurationManager.AppSettings["apiUrl"].ToString();
    }
    private static string WebAPIAddress = ConfigurationManager.AppSettings["WebAPIAddress"].ToString();
    //public static string GetTestApiUrl()
    //{
    //    return ConfigurationManager.AppSettings["TestapiUrl"].ToString();

    //}

    public static string GetReportPageUrl()
    {
        return ConfigurationManager.AppSettings["PatientReportUrl"].ToString();

    }
    public static string GetAppSettingKeyValue(string Key)
    {
        return ConfigurationManager.AppSettings[Key].ToString();

    }
    public static void GeneratePdfReportFromByteArray(byte[] bytes, string ReportName)
    {
        using (var memoryStream = new MemoryStream())
        {
            memoryStream.Write(bytes, 0, bytes.Length);
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "filename=" + ReportName + ".pdf");
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.BinaryWrite(memoryStream.ToArray());
            HttpContext.Current.Response.End();
        }


    }

    public static byte[] GetBytesFromUrl(string SOURCE, int LABNO, int StationId, int ServiceId,int FacilityId)
    {

        //using (var client = new HttpClient())
        //{
        //    var array = queryString.Split('|');
        //   var model = new APIRootClass.ReportInputParam()
        //    {
        //        QueryString = queryString
        //    };
        //    //var postData = client.PostAsJsonAsync(GetApiUrl() + ApiName, model).Result;
        //    // var data = postData.Content.ReadAsStringAsync().Result;
        //    //data = data.Replace("\"", "");
        //    //var bytes = Convert.FromBase64String(data);
        //    return bytes;
        //}

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Report/GetInvesticationReport";
        APIRootClass.InvesticationReport objRoot = new global::APIRootClass.InvesticationReport();
        objRoot.intLoginFacilityId = FacilityId;
        objRoot.intLabNo = LABNO;
        objRoot.RegNo = "RegNo";
        objRoot.LabNo = "LabNo";
        objRoot.intStationId = StationId;
        objRoot.chvServiceIds = ServiceId;
        objRoot.facilityName = string.Empty;
        objRoot.chrSource = SOURCE;
        objRoot.chvDiagSampleIds = 0;
        objRoot.IPNo = 0;        
        
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        var sValue = client.UploadString(ServiceURL, inputJson);
        sValue = sValue.Replace("\"", "");
        return Convert.FromBase64String(sValue);

    }

    public static byte[] GetBytesFromUrlForPriscription(string UserName, string PrescriptionId, int HospitalLocationId, int FacilityId, int EncounterId)
    {

        WebClient client = new WebClient();
        client.Headers["Content-type"] = "application/json";
        client.Encoding = Encoding.UTF8;
        string ServiceURL = WebAPIAddress.ToString() + "api/Report/GetPrescriptionReport";
        APIRootClass.GetPrescriptionReport objRoot = new global::APIRootClass.GetPrescriptionReport();
        objRoot.UserName = UserName;
        objRoot.PrescriptionId = PrescriptionId;
        objRoot.HospitalLocationId = HospitalLocationId;
        objRoot.FacilityId = FacilityId;
        objRoot.EncounterId = EncounterId;
        
        string inputJson = (new JavaScriptSerializer()).Serialize(objRoot);
        var sValue = client.UploadString(ServiceURL, inputJson);
        sValue = sValue.Replace("\"", "");
        return Convert.FromBase64String(sValue);

    }



    public static byte[] GetBytesFromAspNetPageUrl(string queryString, string PageUrl)
    {
        using (var client = new WebClient())
        {
            var bytes = client.DownloadData(PageUrl + queryString);
            return bytes;
        }
    }

    public static void GeneratePdfReportForAspNetPage(byte[] bytes, string ReportName)
    {

        HttpContext.Current.Response.AppendHeader("Content-Disposition", "filename=" + ReportName + ".pdf");
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-length", bytes.Length.ToString());
        HttpContext.Current.Response.BinaryWrite(bytes);
        //HttpContext.Current.Response.End();


    }

    //public static string Base64Decode(string Base64String)
    //{
    //    try
    //    {
    //        var result = Encoding.UTF8.GetString(Convert.FromBase64String(Base64String));
    //        return result;
    //    }
    //    catch 
    //    {

    //       return string.Empty;
    //    }


    //}
    public static string Base64Encode(string plainText)
    {
        try
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        catch
        {

            return string.Empty;
        }

    }
    public static string Base64Decode(string base64EncodedData)
    {
        try
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch
        {

            return string.Empty;
        }

    }

    public static void GetAttachedReportFromFtp(string FileName, byte[] fileBytes)
    {



        FileName = Base64Decode(FileName);
        // FileName = "231_766.pdf";
        var index = FileName.LastIndexOf('.');
        var fileExtension = FileName.Substring(index).Replace(".", "");

        switch (fileExtension)
        {
            case "pdf": HttpContext.Current.Response.ContentType = "application/pdf"; break;
            case "swf": HttpContext.Current.Response.ContentType = "application/x-shockwave-flash"; break;
            case "png": HttpContext.Current.Response.ContentType = "image/png"; break;
            case "gif": HttpContext.Current.Response.ContentType = "image/gif"; break;
            case "jpeg": HttpContext.Current.Response.ContentType = "image/jpg"; break;
            case "jpg": HttpContext.Current.Response.ContentType = "image/jpg"; break;
            case "mp4": HttpContext.Current.Response.ContentType = "video/mp4"; break;
            case "mpeg": HttpContext.Current.Response.ContentType = "video/mpeg"; break;
            case "mov": HttpContext.Current.Response.ContentType = "video/quicktime"; break;
            case "avi": HttpContext.Current.Response.ContentType = "video/x-ms-wmv"; break;
            //and so on  HttpContext.Current.         
            default: HttpContext.Current.Response.ContentType = "application/octet-stream"; break;
        }

        HttpContext.Current.Response.AddHeader("Content-Length", fileBytes.Length.ToString());
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpContext.Current.Response.BinaryWrite(fileBytes);
        HttpContext.Current.Response.End();
        
    }

    
    


    //public static string Generatehash512(string text)
    //{
    //    var message = Encoding.UTF8.GetBytes(text);
    //    var hashString = new SHA512Managed();
    //    var hashValue = hashString.ComputeHash(message);
    //    return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
    //}

    //public static string GenerateTxnId()
    //{
    //    var rnd = new Random();
    //    var strHash = Generatehash512(rnd.ToString() + DateTime.Now);
    //    var txnid1 = strHash.Substring(0, 20);
    //    return txnid1;
    //}
}