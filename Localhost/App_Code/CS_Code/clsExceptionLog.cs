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

/// <summary>
/// Summary description for clsExceptionLog
/// </summary>
public class clsExceptionLog
{
    string sConString;
    Hashtable HshIn, HshOut;
    DAL.DAL Dl;
    string strQueryString = "";
    public string QueryString
    {
        set { strQueryString = value; }
        get { return strQueryString; }
    }
    public clsExceptionLog()
    {
        sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    }

    public void HandleException(Exception ex)
    {
        try
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", (System.Web.HttpContext.Current.Session == null) ? 1 : common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            HshIn.Add("chvSource", "APPLICATION");
            HshIn.Add("chvMessage", common.myStr(ex.Message));
            HshIn.Add("chvQueryString", common.myStr(strQueryString));
            HshIn.Add("chvTargetSite", common.myStr(ex.TargetSite));
            HshIn.Add("chvStackTrace", common.myStr(ex.StackTrace));
            HshIn.Add("chvServerName", common.myStr(System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"]));
            HshIn.Add("chvRequestURL", common.myStr(System.Web.HttpContext.Current.Request.Url));
            HshIn.Add("chvUserAgent", common.myStr(System.Web.HttpContext.Current.Request.UserAgent));
            HshIn.Add("chvUserIP", common.myStr(System.Web.HttpContext.Current.Request.UserHostAddress));
            HshIn.Add("intUserId", (System.Web.HttpContext.Current.Session == null) ? 1 : common.myInt(System.Web.HttpContext.Current.Session["UserId"]));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSaveExceptionLog_HIS", HshIn);
        }
        catch
        {
        }
    }
    public void HandleExceptionWithMethod(Exception ex, String MethodName)
    {
        try
        {
            HshIn = new Hashtable();
            HshIn.Add("inyHospitalLocationId", common.myInt(System.Web.HttpContext.Current.Session["HospitalLocationId"]));
            HshIn.Add("chvSource", "APPLICATION");
            HshIn.Add("chvMessage", "(" + MethodName + ")" + common.myStr(ex.Message));
            HshIn.Add("chvQueryString", common.myStr(strQueryString));
            HshIn.Add("chvTargetSite", common.myStr(ex.TargetSite));
            HshIn.Add("chvStackTrace", common.myStr(ex.StackTrace));
            HshIn.Add("chvServerName", common.myStr(System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"]));
            HshIn.Add("chvRequestURL", common.myStr(System.Web.HttpContext.Current.Request.Url));
            HshIn.Add("chvUserAgent", common.myStr(System.Web.HttpContext.Current.Request.UserAgent));
            HshIn.Add("chvUserIP", common.myStr(System.Web.HttpContext.Current.Request.UserHostAddress));
            HshIn.Add("intUserId", common.myInt(System.Web.HttpContext.Current.Session["UserId"]));
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            objDl.ExecuteNonQuery(CommandType.StoredProcedure, "uspSaveExceptionLog_HIS", HshIn);
        }
        catch
        {
        }
    }

}
