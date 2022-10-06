
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PdfReport : System.Web.UI.Page
{
    string _SOURC; int _LABNO; int _StationId; int _ServiceId;int _LogingFacilityId;
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (common.myStr(Request.QueryString["ReportName"]) == "OPIndent")
        {
            BindReportForPriscription();
        }
        else if(common.myStr(Request.QueryString["ReportName"]) == "OPLabReport")
        {
            _SOURC = Request.QueryString["SOURCE"].ToString();
            _LABNO = common.myInt(Request.QueryString["LABNO"]);
            _StationId = common.myInt(Request.QueryString["StationId"]);
            _ServiceId = common.myInt(Request.QueryString["ServiceId"]);
            _LogingFacilityId = common.myInt(Session["FacilityId"]);
             BindReport(_SOURC, _LABNO, _StationId, _ServiceId, _LogingFacilityId);
        }
        
    }

    private void BindReport(string SOURCE, int LABNO, int StationId, int ServiceId,int FacilityId)
    {
        var bytes = Util.GetBytesFromUrl( SOURCE, LABNO, StationId, ServiceId, FacilityId);
        Util.GeneratePdfReportForAspNetPage(bytes, "InvestigationReport");
    }

    private void BindReportForPriscription()
    {
        string PrescriptionId = common.myInt(Request.QueryString["PId"]).ToString();
        string sUserName = common.myStr(Session["EmployeeName"]);
        int encounterId = 0;
        encounterId = common.myInt(Request.QueryString["EncId"]);
        if (encounterId.Equals(0))
        {
            encounterId = common.myInt(Session["EncounterId"]);
        }
        var bytes = Util.GetBytesFromUrlForPriscription(sUserName, PrescriptionId, common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityID"]), common.myInt(encounterId));
        Util.GeneratePdfReportForAspNetPage(bytes, "OPIndent");
    }


}