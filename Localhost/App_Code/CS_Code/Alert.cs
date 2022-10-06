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
using System.Text;

/// <summary>
/// A JavaScript alert
/// </summary>
public static class Alert
{
    /// <summary>
    /// A JavaScript Show
    /// </summary>
  public static void Show(string message)
    {
        string cleanMessage = message.Replace("'", "\\'");
        string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";
        Page page = System.Web.HttpContext.Current.CurrentHandler as Page;
        if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
         {
             page.ClientScript.RegisterClientScriptBlock(typeof(Alert), "alert", script);
         }
    }

  public static void ShowAjaxMsg(string message, Page page)
  {
      ScriptManager.RegisterStartupScript(page, page.GetType(), "alertmsg", "alert('" + message + "');", true);
  }

}

