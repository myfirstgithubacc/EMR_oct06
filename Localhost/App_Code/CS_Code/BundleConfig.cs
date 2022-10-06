using System.Web.Optimization;
/// <summary>
/// Summary description for BuldleConfig
/// </summary>
public class BundleConfig
{
    public BundleConfig()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static void RegisterBundle(BundleCollection bundle)
    {
        //bundle all common js files, required in every page  
        bundle.Add(new ScriptBundle("~/bundles/EMRMasterWithTopDetailsJS")
        .Include("~/Include/JS/jquery1.11.3.min.js",
        "~/Include/JS/Common1.js",
        "~/Include/JS/chosen.jquery.js",
        "~/jsHTML/bootstrap.min.js",
        //"~/Include/JS/jsapi.js",
        "~/Include/JQueryCSS/jquery-ui.min.js"));

        bundle.Add(new ScriptBundle("~/bundles/CommonJs")
         .Include("~/Include/JS/Common1.js"));

        bundle.Add(new ScriptBundle("~/bundles/ChartJsLibrary")
            .Include("~/Include/JS/jquery1.6.4.min.js",
            "~/Include/JS/jquery.mCustomScrollbar.concat.min.js",
            "~/Scripts/ChartLibrary/Chart.min.js",
            "~/Scripts/ChartLibrary/chartjs-plugin-datalabels.js",
            "~/Scripts/ChartLibrary/utils.js",
            "~/Scripts/ChartLibrary/DashboardGraph.js"));

        bundle.Add(new ScriptBundle("~/bundles/AppointmentJs")
           .Include("~/Include/JS/jquery1.11.3.js",
           "~/Include/JS/bootstrap.min.js",
           "~/Include/JS/jquery.multiple.select.js"));


        //wrapup all css in a bundle  
        bundle.Add(new StyleBundle("~/bundles/EMRMasterWithTopDetailsCss")
            .Include(/*"~/Include/css/open-sans.css",*/
            "~/Include/css/bootstrap.min.css",
            "~/include/Style/BasicElement.css",
            //"~/Include/css/font-awesome.min.css",
            "~/Include/chosen.css",
            "~/Include/css/emr.css",
            "~/Include/css/emr1.css",
            "~/Include/css/FavoriteSet.css",
            "~/Include/css/mainStyle.css",
            "~/Include/EMRStyle.css",
            "~/Include/Style.css",
            "~/Include/JQueryCSS/jquery-ui.css"));


        bundle.Add(new StyleBundle("~/bundles/EMRMasterCss")
            .Include("~/Include/css/bootstrap.min.css",
            "~/Include/css/style_patient_dt.css",
            "~/Include/css/emr.css",
            //"~/Include/css/font-awesome.css",
            //"~/Include/css/open-sans.css",
            "~/Include/EMRStyle.css",
            "~/Include/Style.css",
            "~/Include/chosen.css",
            "~/Include/css/jquery.mCustomScrollbar.css",
            "~/Include/css/mainStyle.css",            
            "~/Include/css/mainNew.css"));

        bundle.Add(new StyleBundle("~/bundles/EMRMainStyle")
                  .Include("~/Include/css/bootstrap.min.css",
                  //"~/Include/css/open-sans.css",
                  //"~/Include/css/font-awesome.min.css",
                  "~/Include/css/emr.css",
                  "~/Include/css/mainStyle.css"));

        bundle.Add(new StyleBundle("~/bundles/AppointmentStyle")
                 .Include("~/Include/css/bootstrap.min.css",
                 //"~/Include/css/open-sans.css",
                 //"~/Include/css/font-awesome.min.css",
                 "~/Include/css/Appointment.css",
                 "~/Include/css/mainStyle.css"));

        bundle.Add(new StyleBundle("~/bundles/MainNewStyleWithBootstrap")
                 .Include("~/Include/css/bootstrap.min.css",
                 "~/Include/css/mainNew.css"));

        bundle.Add(new StyleBundle("~/bundles/BlankMasterStyle")
                .Include("~/Include/css/bootstrap.min.css",
                "~/Include/EMRStyle.css",
                "~/Include/Style.css",
                "~/Include/chosen.css",
                "~/Include/css/mainNew.css",
                "~/Include/css/mainStyle.css",
                "~/Include/css/emr.css"));

        bundle.Add(new StyleBundle("~/bundles/OrderStyle")
          .Include(/*"~/Include/css/open-sans.css",*/
          "~/Include/css/bootstrap.min.css",
          "~/include/Style/BasicElement.css",
          //"~/Include/css/font-awesome.min.css",
          "~/Include/css/Administration.css",
          "~/Include/css/emr.css",
          "~/Include/css/emr1.css",
          "~/Include/css/mainStyle.css",
          "~/Include/EMRStyle.css",
          "~/Include/Style.css",
          "~/Include/css/emr_new.css"));

        bundle.Add(new StyleBundle("~/bundles/WardDetailsStyle")
         .Include(/*"~/Include/css/open-sans.css",*/
         "~/Include/css/bootstrap.min.css",
         "~/Include/css/mainNew.css"));

        BundleTable.EnableOptimizations = true;

    }
}