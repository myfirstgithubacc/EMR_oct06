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
using System.Diagnostics;

/// <summary>
/// Summary description for clsUseful
/// </summary>
public class clsUseful
{
    public clsUseful()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public void LogEvent(String Message, System.Diagnostics.EventLogEntryType type)
    {
        if (!EventLog.SourceExists("Billing"))
        {
            EventLog.CreateEventSource("Billing", "Application");
        }
        EventLog.WriteEntry("Billing", Message, type);

    }

}
