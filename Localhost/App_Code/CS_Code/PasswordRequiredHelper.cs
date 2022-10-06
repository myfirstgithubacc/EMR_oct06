using System.Collections;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for PasswordRequiredHelper
/// </summary>
public class PasswordRequiredHelper
{
    public PasswordRequiredHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Common password required methods
    public static bool CheckIsPasswordRequiredForSecModuleOptionPages(int ModuleId, string OptionCode, string sConString)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        bool result;
        try
        {
            hshIn.Add("@ModuleId", ModuleId);
            hshIn.Add("@OptionCode", OptionCode);
            result = (bool)dl.ExecuteScalar(CommandType.Text, @"select IsPasswordRequired from SecModuleOptionsMaster where moduleid=@ModuleId and OptionCode=@OptionCode", hshIn);
            return result;
        }
        catch { }
        finally
        {
            dl = null;
        }
        return false;
    }
    public static string CheckPasswordScreenType(int ModuleId, string OptionCode, string sConString)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshIn = new Hashtable();
        string result;
        try
        {
            hshIn.Add("@ModuleId", ModuleId);
            hshIn.Add("@OptionCode", OptionCode);
            result = (string)dl.ExecuteScalar(CommandType.Text, @"select PwdScreenType from SecModuleOptionsMaster where moduleid=@ModuleId and OptionCode=@OptionCode", hshIn);
            return result;
        }
        catch { }
        finally
        {
            dl = null;
        }
        return "P";
    }
    public static void SetIsPasswordRequired(bool value)
    {
        try
        {
            System.Web.HttpContext.Current.Session["IsPasswordRequired"] = value;
        }
        catch { }

    }
    public static bool GetIsPasswordRequired()
    {
        try
        {
            return common.myBool(System.Web.HttpContext.Current.Session["IsPasswordRequired"]);
        }
        catch { }
        return false;

    }
    public static void RemoveIsPasswordRequiredSession()
    {
        try
        {
            System.Web.HttpContext.Current.Session["IsPasswordRequired"] = null;
        }
        catch { }

    }


    #endregion

    #region Transection user methods
    public static void SetTransactionUserId(string userId)
    {
        try
        {
            System.Web.HttpContext.Current.Session["TxnUserId"] = userId;
        }
        catch { }


    }
    public static string GetTransactionUserId()
    {
        try
        {
            return common.myStr(System.Web.HttpContext.Current.Session["TxnUserId"]);
        }
        catch { }

        return string.Empty;
    }
    public static void RemoveTransactionUserIdSession()
    {
        try
        {
            System.Web.HttpContext.Current.Session["TxnUserId"] = null;
        }
        catch { }

    }
    #endregion


}