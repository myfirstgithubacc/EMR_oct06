using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using System.Linq;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Principal;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Telerik.Windows.Documents.Common.FormatProviders;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx;
using Telerik.Windows.Documents.Flow.FormatProviders.Html;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf;
using Telerik.Windows.Documents.Flow.FormatProviders.Txt;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Fixed.Model;
using Telerik.Windows.Documents.Flow.Model.Editing;


public partial class EMR_Dashboard_DashboardDischargeSummary : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
   
    
    DataSet ds = new DataSet();
    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        Page.Theme = "DefaultControls";
        if (common.myStr(Request.QueryString["From"]).Equals("POPUP"))
        {
            Page.MasterPageFile = "/Include/Master/BlankMaster.master";
        }
        else
        {
            Page.MasterPageFile = "/Include/Master/EMRMasterWithTopDetails_1.master";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.MaintainScrollPositionOnPostBack = true;
     
        if (!IsPostBack)
        {
           
            Page.MaintainScrollPositionOnPostBack = true;

            BaseC.ICM ObjIcm = new BaseC.ICM(sConString);

            ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(Request.QueryString["RegistrationId"]),
                                      common.myStr(Request.QueryString["EncounterId"]), 0, common.myInt(Session["FacilityId"]), "DI");

            //ds = ObjIcm.GetICMPatientSummaryDetails(common.myInt(Session["HospitalLocationID"]), common.myStr(ViewState["RegistrationId"]),
            //                           common.myStr(Request.QueryString["EncounterId"]), ReportId, common.myInt(Session["FacilityId"]), "DI", type, SummaryId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RTF1.Content = common.myStr(ds.Tables[0].Rows[0]["PatientSummary"]);
                if (ds.Tables[0].Rows[0]["FormatId"].ToString().Contains("#"))
                {
                    ViewState["SavedReportFormat"] = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).Split('#')[1];
                }
                else
                {
                    ViewState["SavedReportFormat"] = common.myStr(ds.Tables[0].Rows[0]["FormatId"]).ToString();
                }

            }
           

        }
    }

    protected void btnPrintPdf_Click(object sender, EventArgs e)
    {
        BaseC.Security ObjSecurity = new BaseC.Security(sConString);
        try
        {

            RadWindow2.NavigateUrl = "~/EMRReports/PrintPdf1.aspx?page=Ward&EncId=" + common.myStr(Request.QueryString["EncounterId"]) + "&RegId=" + common.myStr(Request.QueryString["RegistrationId"]) + "&For=DISSUM&Finalize=" + common.myStr("") + "&ExportToWord=" + common.myBool(false) + "&ReportId=" + common.myStr(ViewState["SavedReportFormat"]) + "&Type=" + common.myStr("DI") + "&SummaryId=" + common.myStr("0");
            RadWindow2.Width = 1200;
            RadWindow2.Height = 630;
            RadWindow2.Top = 10;
            RadWindow2.Left = 10;

            RadWindow2.VisibleOnPageLoad = true; //Set this property to True for showing window from code 
            RadWindow2.Modal = true;
            RadWindow2.VisibleStatusbar = false;
            RadWindow2.InitialBehavior = Telerik.Web.UI.WindowBehaviors.Maximize;

        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            ObjSecurity = null;
        }
    }
}