<%@ Application Language="C#" %>
<%@ Import Namespace= "System.Web.Optimization" %> 
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {

        Application["HospitalName"] = "";
        Application["IPIncluded"] = "";
        Application["LISIncluded"] = "";

        Application["InputDateFormat"] = "dd/MM/yyyy";
        Application["OutputDateFormat"] = "dd/MM/yyyy";

        BundleConfig.RegisterBundle(BundleTable.Bundles);
        BundleTable.EnableOptimizations = true;
    }

    void Application_End(object sender, EventArgs e)
    {
        //  Code that runs on application shutdown
        Application["HospitalName"] = "";
        Application["IPIncluded"] = "";
        Application["LISIncluded"] = "";
    }

    void Application_Error(object sender, EventArgs e)
    {
        Exception ex = System.Web.HttpContext.Current.Server.GetLastError();
        clsExceptionLog excp = new clsExceptionLog();
        excp.HandleException(ex);

        if (ex.InnerException != null)
        {
           Response.Redirect("Login.aspx?Error" + common.myStr(ex.InnerException));
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        //Session["Theme"] = "Gray";
        //Session["HospitalLocationID"] = 1;
        //Session["UserID"] = 1;
    }

    void Session_End(object sender, EventArgs e)
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

    protected void Application_BeginRequest(Object sender, EventArgs e)

    {
        //// Force SSL if configured to do so.
        //if (!Request.IsSecureConnection)
        //{
        //   string path = string.Format("https{0}", Request.Url.AbsoluteUri.Substring(4));

        //            Response.Redirect(path);
        //      }
    }
</script>
