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
using System.Threading;
using System.Globalization;


/**
     <summary>
         The NDocSample encapsulates some very rich functionality.
     </summary>
 */ 
public abstract class BasePage : System.Web.UI.Page
{

    protected override void InitializeCulture()
    {
        

        HttpCookie cultureCookie = Request.Cookies["Culture"];

        string cultureCode = null;

        if ((cultureCookie == null))
        {

            cultureCode = null;

        }

        else
        {

            cultureCode = cultureCookie.Value;

        }

        if (!string.IsNullOrEmpty(cultureCode))
        {

            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureCode);

            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;

        }
        base.InitializeCulture();

    }

}
