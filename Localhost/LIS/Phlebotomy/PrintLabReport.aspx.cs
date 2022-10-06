using System;
using System.IO;
using System.Data;
using BaseC;
using System.Configuration;
using System.Collections;
using Microsoft.Reporting.WebForms;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;

[Serializable]
public partial class LIS_PrintLabReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private string reportServer = ConfigurationManager.ConnectionStrings["rsvr"].ConnectionString;
    private string reportFolder = ConfigurationManager.AppSettings["SSRSFolder"];
    private string SysUser = ConfigurationManager.AppSettings["SysUser"];
    private string SysPassword = ConfigurationManager.AppSettings["SysPassword"];
    private string SysDomain = ConfigurationManager.AppSettings["SysDomain"];
    Hashtable hstInput;
    Hashtable houtPara;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowReport();
        }
    }
    #region update print times
    private void PrintStatusUpdate()
    {
        clsLISPhlebotomy objclsLISPhlebotomy;
        objclsLISPhlebotomy = new clsLISPhlebotomy(sConString);
        //objclsLISPhlebotomy.UpdateprintStatus(common.myStr(ViewState["Source"]), common.myInt(Request.QueryString["LABNO"])), common.myStr(strXml.ToString()), common.myInt(Session["UserID"]), common.myInt(Session["FacilityId"]));
        objclsLISPhlebotomy = null;
    }
    #endregion
    protected void Page_Init(object sender, EventArgs e)
    {

    }

    protected void ShowReport()
    {
        BaseC.User valUser = new BaseC.User(sConString);
        string sUserName = valUser.GetUserName(common.myInt(Session["UserID"]));
        int iDiagSampleId = common.myInt(Request.QueryString["DiagSampleId"]);
        if (Cache["HospitalSetup"] == null)
        {
            clsLISMaster eHospitalSetup = new clsLISMaster(sConString);
            // eHospitalSetup.HospitalLocationID = Convert.ToInt16(Session["HospitalLocationID"]);
            DataSet ds = eHospitalSetup.getHospitalSetUp(common.myInt(Session["HospitalLocationId"]));
            Cache["HospitalSetup"] = ds.Tables[0];
        }
        DataTable dsHospitalSetup = (DataTable)Cache["HospitalSetup"];
        if (dsHospitalSetup.Rows.Count == 0)
        {
            clsLISMaster eHospitalSetup = new clsLISMaster(sConString);
            // eHospitalSetup.HospitalLocationID = Convert.ToInt16(Session["HospitalLocationID"]);
            DataSet ds = eHospitalSetup.getHospitalSetUp(common.myInt(Session["HospitalLocationId"]));
            Cache["HospitalSetup"] = ds.Tables[0];
        }
        dsHospitalSetup.DefaultView.RowFilter = "";
        dsHospitalSetup.DefaultView.RowFilter = "Flag='DefaultFacility'";
        int FacilityId = common.myInt(Session["FacilityId"]);

        if (Cache["FACILITY"] == null)
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            Cache["FACILITY"] = ds;
        }
        DataSet dsFacility = (DataSet)Cache["FACILITY"];
        if (dsFacility.Tables[0].Rows.Count == 0)
        {
            BaseC.clsLISMaster objMaster = new BaseC.clsLISMaster(sConString);
            DataSet ds = objMaster.getFacilityList(common.myInt(Session["HospitalLocationID"]), 0, 0);
            Cache["FACILITY"] = ds;
        }

        dsFacility.Tables[0].DefaultView.RowFilter = "";
        dsFacility.Tables[0].DefaultView.RowFilter = "FacilityId=" + common.myStr(Session["FacilityId"]);
        string DefaultFacilityName = string.Empty;
        if (common.myInt(dsFacility.Tables[0].Rows.Count) > 0)
        {
            DefaultFacilityName = common.myStr(dsFacility.Tables[0].DefaultView[0]["FacilityName"]);
        }

        string sShowAllParameters = common.myStr(Session["ModuleName"]) == "EMR" ? "0" : "1";
        string sSampleStatusId = common.myInt(Request.QueryString["intSampleStatusId"]).ToString();

        ReportParameter[] p;
        if (common.myStr(Session["FacilityName"]).ToUpper().Contains("KIRAN"))
        {
            p = new ReportParameter[14];
        }
        else
        {
            //p = new ReportParameter[13];
            p = new ReportParameter[12];
        }

        if (iDiagSampleId > 0)
        {
            string sRptName = "InvestigationResult";

            p[0] = new ReportParameter("intLoginFacilityId", common.myStr(Session["FacilityID"]));
            p[1] = new ReportParameter("chrSource", common.myStr(Request.QueryString["SOURCE"]));
            p[2] = new ReportParameter("intLabNo", common.myStr(Request.QueryString["LABNO"]));
            p[3] = new ReportParameter("intStationId", common.myStr(Request.QueryString["StationId"]));
            if (common.myStr(Request.QueryString["ServiceIds"]) != "")
            {
                p[4] = new ReportParameter("chvServiceIds", common.myStr(Request.QueryString["ServiceIds"]));
            }
            else
            {
                p[4] = new ReportParameter("chvServiceIds", " ");
            }
            p[5] = new ReportParameter("RegNo", common.myStr(GetGlobalResourceObject("PRegistration", "regno")));
            p[6] = new ReportParameter("IPNo", common.myStr(GetGlobalResourceObject("PRegistration", "EncounterNo")).Replace("&nbsp;", " "));
            if (common.myStr(Session["ModuleName"]) == "RIS")
            {
                p[7] = new ReportParameter("LabNo", common.myStr(GetGlobalResourceObject("PRegistration", "RadiologyNo")));
            }
            else
            {
                p[7] = new ReportParameter("LabNo", common.myStr(GetGlobalResourceObject("PRegistration", "LABNO")));
            }
            p[8] = new ReportParameter("facilityName", DefaultFacilityName);
            p[9] = new ReportParameter("bitShowAllParameters", sShowAllParameters);
            p[10] = new ReportParameter("intDiagSampleId", common.myInt(Request.QueryString["DiagSampleId"]).ToString());
            p[11] = new ReportParameter("username", common.myStr(sUserName));
            //p[12] = new ReportParameter("chvDiagSampleIds", common.myInt(Request.QueryString["DiagSampleId"]).ToString());
            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("KIRAN"))
            {
                p[12] = new ReportParameter("pagecode", common.myStr(Request.QueryString["pagecode"]));
                p[13] = new ReportParameter("intSampleStatusId", common.myStr(sSampleStatusId));
            }
           // else
           // {
             //   if (common.myStr(Request.QueryString["pagecode"]) != null)
             //   {
                //    p[12] = new ReportParameter("pagecode", common.myStr(Request.QueryString["pagecode"]));
               // }
            //    else
             //   {
              //      p[12] = new ReportParameter("pagecode", "");
              //  }

             //   if (common.myStr(sSampleStatusId) != null)
            //    {
              //      p[13] = new ReportParameter("intSampleStatusId", common.myStr(sSampleStatusId));
              //  }
              //  else
              //  {
             //       p[13] = new ReportParameter("intSampleStatusId", "");
             //   }
           // }

            PrintReport(sRptName, p);
        }
        else if (iDiagSampleId == 0 && common.myStr(Request.QueryString["IsArchive"]).Trim() == "0" && (common.myStr(Request.QueryString["ReportType"]).Trim() == "N" || common.myStr(Request.QueryString["ReportType"]).Trim() == "G"))
        {
            ReportParameter[] pn = new ReportParameter[9];

            string sRptName = "NormalReport";
            string StrOpdIpd = "";

            if (common.myStr(Request.QueryString["SOURCE"]) == "IPD")
            {
                StrOpdIpd = "I";
            }
            else if (common.myStr(Request.QueryString["SOURCE"]) == "OPD")
            {
                StrOpdIpd = "O";
            }

            pn[0] = new ReportParameter("intDept_Code", common.myStr(Request.QueryString["DepartmentCode"]));
            pn[1] = new ReportParameter("strLab_RegNo", common.myStr(Request.QueryString["LABNO"]));
            pn[2] = new ReportParameter("StrOpdIpd", StrOpdIpd);
            pn[3] = new ReportParameter("intSeries_No", "1");
            pn[4] = new ReportParameter("intSubDept_Code", common.myStr(Request.QueryString["SubDeptCode"]));
            pn[5] = new ReportParameter("LocationId", common.myStr(Session["HospitalLocationID"]));
            pn[6] = new ReportParameter("UHID", "UHID");
            pn[7] = new ReportParameter("IPNO", "IP. No.");

            if (common.myStr(Request.QueryString["ServiceIds"]) != "")
            {
                pn[8] = new ReportParameter("StrFormula", common.myStr(Request.QueryString["ServiceIds"]));
            }
            else
            {
                pn[8] = new ReportParameter("StrFormula", " ");
            }
            PrintReport(sRptName, pn);
        }

        else if (iDiagSampleId == 0 && common.myStr(Request.QueryString["IsArchive"]).Trim() == "0" && common.myStr(Request.QueryString["ReportType"]).Trim() == "T")
        {
            ReportParameter[] pn = new ReportParameter[9];
            string sRptName = "TypicalReport";
            string StrOpdIpd = "";

            if (common.myStr(Request.QueryString["SOURCE"]) == "IPD")
            {
                StrOpdIpd = "I";
            }
            else if (common.myStr(Request.QueryString["SOURCE"]) == "OPD")
            {
                StrOpdIpd = "O";
            }

            pn[0] = new ReportParameter("intDept_Code", common.myStr(Request.QueryString["DepartmentCode"]));
            pn[1] = new ReportParameter("strLab_RegNo", common.myStr(Request.QueryString["LABNO"]));
            pn[2] = new ReportParameter("OPD_IPD", StrOpdIpd);
            pn[3] = new ReportParameter("intSeries_No", "1");
            pn[4] = new ReportParameter("intSubDept_Code", common.myStr(Request.QueryString["SubDeptCode"]));
            pn[5] = new ReportParameter("UHID", "UHID");
            pn[6] = new ReportParameter("IPNO", "IP. No.");

            if (common.myStr(Request.QueryString["ServiceIds"]) != "")
            {
                pn[7] = new ReportParameter("StrFormula", common.myStr(Request.QueryString["ServiceIds"]));
            }
            else
            {
                pn[7] = new ReportParameter("StrFormula", " ");
            }
            pn[8] = new ReportParameter("StrOpdIpd", StrOpdIpd);

            PrintReport(sRptName, pn);
        }
        else if (iDiagSampleId == 0 && common.myStr(Request.QueryString["IsArchive"]).Trim() == "0" && common.myStr(Request.QueryString["ReportType"]).Trim() == "P")
        {
            ReportParameter[] pn = new ReportParameter[10];
            string sRptName = "SpecialReport_" + common.myStr(Request.QueryString["FormatID"]);
            string StrOpdIpd = "";

            if (common.myStr(Request.QueryString["SOURCE"]) == "IPD")
            {
                StrOpdIpd = "I";
            }
            else if (common.myStr(Request.QueryString["SOURCE"]) == "OPD")
            {
                StrOpdIpd = "O";
            }
            pn[0] = new ReportParameter("LabReg_No", common.myStr(Request.QueryString["LABNO"]));
            pn[1] = new ReportParameter("OPD_IPD", StrOpdIpd);
            pn[2] = new ReportParameter("Result_ID", "1");
            pn[3] = new ReportParameter("format_code", common.myStr(Request.QueryString["FormatID"]));
            pn[4] = new ReportParameter("intDept_Code", common.myStr(Request.QueryString["DepartmentCode"]));
            pn[5] = new ReportParameter("SubDeptCode", common.myStr(Request.QueryString["SubDeptCode"]));
            pn[6] = new ReportParameter("LocationId", common.myStr(Session["HospitalLocationID"]));
            pn[7] = new ReportParameter("UHID", "UHID");
            pn[8] = new ReportParameter("IPNO", "IP. No.");

            if (common.myStr(Request.QueryString["ServiceIds"]) != "")
            {
                pn[9] = new ReportParameter("ServiceCode", common.myStr(Request.QueryString["ServiceIds"]));
            }
            else
            {
                pn[9] = new ReportParameter("ServiceCode", " ");
            }
            PrintReport(sRptName, pn);
        }
        else if (iDiagSampleId == 0 && common.myStr(Request.QueryString["IsArchive"]).Trim() == "1" && (common.myStr(Request.QueryString["ReportType"]).Trim() == "N" || common.myStr(Request.QueryString["ReportType"]).Trim() == "G"))
        {
            ReportParameter[] pn = new ReportParameter[9];

            string sRptName = "NormalReport_archieve";
            string StrOpdIpd = "";

            if (common.myStr(Request.QueryString["SOURCE"]) == "IPD")
            {
                StrOpdIpd = "I";
            }
            else if (common.myStr(Request.QueryString["SOURCE"]) == "OPD")
            {
                StrOpdIpd = "O";
            }

            pn[0] = new ReportParameter("intDept_Code", common.myStr(Request.QueryString["DepartmentCode"]));
            pn[1] = new ReportParameter("strLab_RegNo", common.myStr(Request.QueryString["LABNO"]));
            pn[2] = new ReportParameter("StrOpdIpd", StrOpdIpd);
            pn[3] = new ReportParameter("intSeries_No", "1");
            pn[4] = new ReportParameter("intSubDept_Code", common.myStr(Request.QueryString["SubDeptCode"]));
            pn[5] = new ReportParameter("LocationId", common.myStr(Session["HospitalLocationID"]));
            pn[6] = new ReportParameter("UHID", "UHID");
            pn[7] = new ReportParameter("IPNO", "IP. No.");

            if (common.myStr(Request.QueryString["ServiceIds"]) != "")
            {
                pn[8] = new ReportParameter("StrFormula", common.myStr(Request.QueryString["ServiceIds"]));
            }
            else
            {
                pn[8] = new ReportParameter("StrFormula", " ");
            }
            PrintReport(sRptName, pn);
        }
        else if (iDiagSampleId == 0 && common.myStr(Request.QueryString["IsArchive"]).Trim() == "1" && common.myStr(Request.QueryString["ReportType"]).Trim() == "T")
        {
            ReportParameter[] pn = new ReportParameter[9];
            string sRptName = "TypicalReport_archieve";
            string StrOpdIpd = "";

            if (common.myStr(Request.QueryString["SOURCE"]) == "IPD")
            {
                StrOpdIpd = "I";
            }
            else if (common.myStr(Request.QueryString["SOURCE"]) == "OPD")
            {
                StrOpdIpd = "O";
            }

            pn[0] = new ReportParameter("intDept_Code", common.myStr(Request.QueryString["DepartmentCode"]));
            pn[1] = new ReportParameter("strLab_RegNo", common.myStr(Request.QueryString["LABNO"]));
            pn[2] = new ReportParameter("OPD_IPD", StrOpdIpd);
            pn[3] = new ReportParameter("intSeries_No", "1");
            pn[4] = new ReportParameter("intSubDept_Code", common.myStr(Request.QueryString["SubDeptCode"]));
            pn[5] = new ReportParameter("UHID", "UHID");
            pn[6] = new ReportParameter("IPNO", "IP. No.");

            if (common.myStr(Request.QueryString["ServiceIds"]) != "")
            {
                pn[7] = new ReportParameter("StrFormula", common.myStr(Request.QueryString["ServiceIds"]));
            }
            else
            {
                pn[7] = new ReportParameter("StrFormula", " ");
            }
            pn[8] = new ReportParameter("StrOpdIpd", StrOpdIpd);
            PrintReport(sRptName, pn);
        }
    }
    private void PrintReport(string sReportName, ReportParameter[] para)
    {
        BaseC.Security objSecurity = new Security(sConString);
        bool bPrintReportRight = false;
        string flgLabPrintReportRestriction = string.Empty, sPreview = string.Empty;
        try
        {
            sPreview = common.myStr(Request.QueryString["preview"]);
            bPrintReportRight = objSecurity.CheckUserRights(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["EmployeeId"]), common.myInt(Session["FacilityId"]), "PrintLabReportsUserWise");
            flgLabPrintReportRestriction = common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), "PrintLabReportRestriction", sConString));

            if (common.myStr(flgLabPrintReportRestriction).Equals("Y"))
            {
                if (!common.myBool(bPrintReportRight)) { sPreview = "PP"; }
            }

            string ReportServerPath = "http://" + reportServer + "/ReportServer";
            //string ReportServerPath = reportServer;
            ReportViewer1.ServerReport.ReportServerUrl = new Uri(ReportServerPath);
            IReportServerCredentials irsc = new CustomReportCredentials(SysUser, SysPassword, SysDomain);
            ReportViewer1.ServerReport.ReportServerCredentials = irsc;
            ReportViewer1.ProcessingMode = ProcessingMode.Remote;
            ReportViewer1.ShowCredentialPrompts = false;
            ReportViewer1.ShowFindControls = false;
            ReportViewer1.ShowParameterPrompts = false;

            ReportViewer1.ServerReport.ReportPath = "/" + reportFolder + "/" + sReportName;
            ReportViewer1.ServerReport.SetParameters(para);
            ReportViewer1.ServerReport.Refresh();


            if (common.myStr(sPreview).Equals("PP"))
            {
                ReportViewer1.ShowPrintButton = false;
                ReportViewer1.ShowExportControls = false;
                return;
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

            if (common.myStr(Request.QueryString["EMail"]).Trim().Length > 3)
            {
                EMRMasters.EMRFacility objf = new EMRMasters.EMRFacility(sConString);
                DataSet ds = objf.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataView dv = ds.Tables[0].DefaultView;
                        dv.RowFilter = "FacilityId = " + common.myStr(Session["FacilityID"]);
                        if (dv.Count > 0)
                        {
                            DataTable dt = dv.ToTable();
                            string SMTPMailServer = common.myStr(dt.Rows[0]["SMTPMailServer"]);
                            string SMTPMailServerPort = common.myStr(dt.Rows[0]["SMTPMailServerPort"]);
                            string DefaultFromMailId = common.myStr(dt.Rows[0]["DefaultFromMailId"]);
                            string DefaultFromMailPws = common.myStr(dt.Rows[0]["DefaultFromMailPws"]);

                            MemoryStream ms = new MemoryStream(bytes); ;
                            MailMessage mail = new MailMessage();
                            mail.Attachments.Add(new Attachment(ms, common.myStr(Request.QueryString["LABNO"]).Trim() + ".pdf"));

                            mail.From = new MailAddress(DefaultFromMailId, DefaultFromMailId);
                            mail.To.Add(common.myStr(Request.QueryString["EMail"]).Trim());
                            mail.Subject = "Lab Result For Lab No. " + common.myStr(Request.QueryString["LABNO"]).Trim();
                            mail.Body = "Dear " + common.myStr(Request.QueryString["PName"]).Trim() + "," + System.Environment.NewLine + System.Environment.NewLine + "Please! Find your attached Lab Report.";
                            mail.IsBodyHtml = false;
                            mail.BodyEncoding = System.Text.Encoding.UTF8;

                            SmtpClient client = new SmtpClient(SMTPMailServer, common.myInt(SMTPMailServerPort));
                            client.Credentials = new NetworkCredential(DefaultFromMailId, DefaultFromMailPws);
                            client.Host = SMTPMailServer;
                            client.Port = common.myInt(SMTPMailServerPort);
                            client.Send(mail);
                            ScriptManager.RegisterStartupScript(Page, GetType(), "scrip1", " var oWindow = null; if (window.radWindow) oWindow = window.radWindow; else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow; oWindow.close(); ", true);
                        }
                    }
                }
            }
            else
            {
                MemoryStream oStream = new MemoryStream();
                oStream.Write(bytes, 0, bytes.Length);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.BinaryWrite(oStream.ToArray());
                //Response.End();
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception ex)
        {
            //msg.Text = "1 " + ex.Message;
        }
        finally { objSecurity = null; }
    }

}
