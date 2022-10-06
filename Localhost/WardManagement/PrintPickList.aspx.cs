using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Reporting.WebForms;
using System.Data.SqlClient;
using System.IO;

public partial class EMR_Medication_PrintPickList : System.Web.UI.Page
{
    //palendra
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowReport();
        }
    }

    protected void ShowReport()
    {
        //if (common.myInt(Request.QueryString["EncId"]) > 0)
        //{
            string ReportServerPath = "http://" + reportServer + "/ReportServer";

            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;

            ReportViewer1.ShowCredentialPrompts = false;
            ReportViewer1.ShowFindControls = false;
            ReportViewer1.ShowParameterPrompts = false;

            ReportParameter[] p;

            int PrescriptionId = common.myInt(Request.QueryString["PId"]);

            BaseC.User valUser = new BaseC.User(sConString);
            //BaseC.clsPharmacy objPhr = new BaseC.clsPharmacy(sConString);
            string sUserName = common.myStr(valUser.GetUserName(common.myInt(Session["UserID"])));

            switch (common.myStr(Request.QueryString["PT"]))
            {
                //case "P":
                //    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/OPIndent";
                //    p = new ReportParameter[6];

                //    p[0] = new ReportParameter("UserName", sUserName);
                //    p[1] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
                //    p[2] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
                //    p[3] = new ReportParameter("intEncounterId", Request.QueryString["EncId"].ToString());
                //    p[4] = new ReportParameter("intIndentId", PrescriptionId.ToString());
                //    p[5] = new ReportParameter("UHID", (string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));


                //    ReportViewer1.ServerReport.SetParameters(p);
                //    ReportViewer1.ServerReport.Refresh();



                //    break;
                case "I":
                if (common.myInt(Request.QueryString["EncId"]) > 0)
                {
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/IPDPickList";
                    p = new ReportParameter[6];

                    p[0] = new ReportParameter("UserName", sUserName);
                    p[1] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
                    p[2] = new ReportParameter("intFacilityId", common.myInt(Session["FacilityID"]).ToString());
                    p[3] = new ReportParameter("intEncounterId", Request.QueryString["EncId"].ToString());
                    p[4] = new ReportParameter("intIndentId", PrescriptionId.ToString());
                    p[5] = new ReportParameter("UHID", (string)HttpContext.GetGlobalResourceObject("PRegistration", "UHID"));


                    ReportViewer1.ServerReport.SetParameters(p);
                    ReportViewer1.ServerReport.Refresh();



                    break;
                }
                else
                {
                    return;
                }
                case "DI":
                    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/DIPickList";
                    p = new ReportParameter[10];

                    p[0] = new ReportParameter("UserName", sUserName);
                    p[1] = new ReportParameter("inyHospitalLocationId", common.myInt(Session["HospitalLocationId"]).ToString());
                    p[2] = new ReportParameter("chvIndentNo", "");
                    p[3] = new ReportParameter("intIndentId", "0");
                    p[4] = new ReportParameter("intFacilityFromId", Request.QueryString["FFId"].ToString());
                    p[5] = new ReportParameter("intDepartmentFromId", Request.QueryString["FDId"].ToString());
                    p[6] = new ReportParameter("intFacilityToId", common.myInt(Session["FacilityID"]).ToString());
                    p[7] = new ReportParameter("intDepartmentToId", common.myInt(Session["StoreId"]).ToString());
                    p[8] = new ReportParameter("dtFromdate", Request.QueryString["FDate"].ToString());
                    p[9] = new ReportParameter("dtTodate", Request.QueryString["TDate"].ToString());

                    ReportViewer1.ServerReport.SetParameters(p);
                    ReportViewer1.ServerReport.Refresh();



                    break;

                    //case "M":
                    //    ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/PrintCurrentMedication";
                    //    p = new ReportParameter[3];

                    //    p[0] = new ReportParameter("inyHospitalLocationID", common.myInt(Session["HospitalLocationId"]).ToString());
                    //    p[2] = new ReportParameter("intRegistrationId", common.myInt(Session["RegistrationId"]).ToString());
                    //    p[1] = new ReportParameter("intLoginFacilityId", common.myInt(Session["FacilityID"]).ToString());

                    //    ReportViewer1.ServerReport.SetParameters(p);
                    //    ReportViewer1.ServerReport.Refresh();



                    //    break;


            }
            string[] streamids = null;
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string mimeType;
            string encoding;
            string extension;
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource(ReportServerPath));
            byte[] bytes = this.ReportViewer1.ServerReport.Render(
              "PDF", null, out mimeType, out encoding,
               out extension,
              out streamids, out warnings);

            MemoryStream oStream = new MemoryStream(); ;
            oStream.Write(bytes, 0, bytes.Length);
            Response.Clear();
            Response.Buffer = true;
            string filename = common.myStr(Request.QueryString["ReportName"]) + ".pdf";
            Response.AppendHeader("Content-Disposition", "filename=" + filename + "");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(oStream.ToArray());
            Response.End();

        //}
    }
}