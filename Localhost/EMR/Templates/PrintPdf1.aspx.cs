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
using System.IO;
using System.Text;
using iTextSharp.text;

public partial class EMR_Templates_PrintPdf1 : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    private static string strimgData = "";
    private static string Education = "";
    string EMRPrintBlankConsultantSignatureLabel = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string EMRTemplatePrintSignatureOnFooter = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                         common.myInt(Session["FacilityId"]), "InEMRTemplatePrintSignatureOnFooter", sConString);
            clsIVF objIVF = new clsIVF(sConString);
            DataSet ds = new DataSet();

            int MarginTop = 0;
            int MarginBottom = 0;
            int MarginLeft = 0;
            int MarginRight = 0;

            clsParsePDFDischarge cPrint = new clsParsePDFDischarge();

            try
            {
                Session["FacilityName"] = Session["FacilityName"];

                if (common.myLen(HttpContext.Current.Session["FacilityName"]).Equals(0))
                {
                    this.Title = string.Empty;

                    return;
                }

                string strHTML = common.myStr(Session["EMRTemplatePrintData"]).Replace("..", "&#46;&#46;");

                StringBuilder sbNextHeader = new StringBuilder();
                StringBuilder sbtopheader = new StringBuilder();

                ds = objIVF.getPrintTemplateReportSetup("PT");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    MarginTop = common.myInt(ds.Tables[0].Rows[0]["MarginTop"]);
                    MarginBottom = common.myInt(ds.Tables[0].Rows[0]["MarginBottom"]);
                    MarginLeft = common.myInt(ds.Tables[0].Rows[0]["MarginLeft"]);
                    MarginRight = common.myInt(ds.Tables[0].Rows[0]["MarginRight"]);

                    if (common.myBool(ds.Tables[0].Rows[0]["ShowPrintByInPageFooter"]))
                    {
                        cPrint.FooterLeftText = "Printed By: " + common.myStr(Session["UserName"]);
                    }
                    if (common.myBool(ds.Tables[0].Rows[0]["ShowPrintDateInPageFooter"]))
                    {
                        cPrint.FooterCenterText = "Print Date & Time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    }

                    if (EMRTemplatePrintSignatureOnFooter.Equals("Y") && common.myStr(Session["OPIP"]).Equals("O"))
                    {
                        DataSet ds1 = new DataSet();

                        if (common.myInt(Session["DoctorID"]) > 0)
                        {
                            ds1 = objIVF.getDoctorSignatureDetails(common.myInt(Session["DoctorID"]));

                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                //cPrint.LowestFooter = "                                                                                                                                                                       " + common.myStr(ds1.Tables[0].Rows[0]["DoctorName"]).Trim() + "\n                                                                                                                                                                       " + common.myStr(ds1.Tables[0].Rows[0]["DoctorDesignation"]).Trim() + "\n\n\n";

                                cPrint.LowestFooter1 = "                                                                                                                                                     " + common.myStr(ds1.Tables[0].Rows[0]["DoctorName"]).Trim();
                                cPrint.LowestFooter = "                                                                                                                                                     " + common.myStr(ds1.Tables[0].Rows[0]["DoctorDesignation"]).Trim() + "\n\n\n";
                            }
                        }
                    }
                    else
                    {
                        strHTML += getSignatureDetails(common.myBool(ds.Tables[0].Rows[0]["IsPrintDoctorSignature"]));
                    }

                    if (common.myStr(HttpContext.Current.Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                    {
                        if (common.myLen(ds.Tables[0].Rows[0]["ReportFooterText"]) > 0 && common.myStr(Session["OPIP"]).ToUpper() != "I")
                        {

                            cPrint.LowestFooter += "\n" + common.myStr(ds.Tables[0].Rows[0]["ReportFooterText"]) + Environment.NewLine + " ";
                        }
                        else
                        {
                            cPrint.LowestFooter += " " + Environment.NewLine + " ";
                        }
                    }
                    else
                    {
                        if (common.myLen(ds.Tables[0].Rows[0]["ReportFooterText"]) > 0)
                        {
                            cPrint.LowestFooter = common.myStr(ds.Tables[0].Rows[0]["ReportFooterText"]) + Environment.NewLine + " ";
                        }
                        else
                        {
                            cPrint.LowestFooter = " " + Environment.NewLine + " ";
                        }
                    }
                }

                if (MarginTop.Equals(0)) { MarginTop = 60; }
                if (MarginBottom.Equals(0)) { MarginBottom = 60; }
                if (MarginLeft.Equals(0)) { MarginLeft = 22; }
                if (MarginRight.Equals(0)) { MarginRight = 13; }

                cPrint.MarginLeft = MarginLeft;
                cPrint.MarginRight = MarginRight;
                cPrint.MarginTop = MarginTop;
                cPrint.MarginBottom = MarginBottom;

                sbtopheader.Append(getReportHeader(ds).ToString());

                cPrint.GetFontName();

                cPrint.FirstPageHeaderHtml = sbtopheader.ToString();
                cPrint.HeaderHtml = sbtopheader.ToString();

                strHTML = common.removeUnusedHTML(strHTML);
                cPrint.Html = strHTML;

                cPrint.Size = iTextSharp.text.PageSize.GetRectangle("A4");
                MemoryStream m = cPrint.ParsePDF();
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "inline;filename=download_report.pdf");
                Response.AppendHeader("Content-Length", m.GetBuffer().Length.ToString());

                Response.OutputStream.Write(m.GetBuffer(), 0, m.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
                Response.End();

            }
            catch (Exception Ex)
            {
                //objException.HandleException(Ex);
            }
            finally
            {
                objIVF = null;
            }
        }
    }

    private StringBuilder getReportHeader(DataSet ds)
    {
        StringBuilder sb = new StringBuilder();

        bool IsPrintHospitalHeader = false;
        clsIVF objivf = new clsIVF(sConString);
        string FileName = string.Empty;
        string SignImage = string.Empty;
        string strSingImagePath = string.Empty;

        string NABHFileName = string.Empty;
        string NABHSignImage = string.Empty;
        string NABHstrSingImagePath = string.Empty;

        int HeaderId = 19;

        try
        {
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);

                    HeaderId = common.myInt(ds.Tables[0].Rows[0]["HeaderId"]);
                }
            }

            if (common.myBool(Request.QueryString["PrintHeader"]))
            {
                IsPrintHospitalHeader = common.myBool(Request.QueryString["PrintHeader"]);
            }

            sb.Append("<div>");

            if (IsPrintHospitalHeader)
            {
                ds = new DataSet();
                ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

                if (common.myStr(HttpContext.Current.Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataSet dsSignature = new DataSet();
                        dsSignature = objivf.getDoctorSignatureDetails(common.myInt(Session["DoctorId"]));

                        sb.Append("<table border='0' width='100%' cellpadding='1' cellspacing='1' style='font-size:9pt;'>");
                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];

                            FileName = common.myStr(DR["LogoImagePath"]);
                            if (FileName != "")
                            {
                                SignImage = "<img width='200px' height='60px' src='" + Server.MapPath("~") + FileName + "' />";
                                strSingImagePath = Server.MapPath("~") + FileName;
                            }

                            NABHFileName = common.myStr(DR["NABHLogoImagePath"]);
                            if (NABHFileName != "")
                            {
                                NABHSignImage = "<img width='40px' height='40px' src='" + Server.MapPath("~") + NABHFileName + "' />";
                                NABHstrSingImagePath = Server.MapPath("~") + NABHFileName;
                            }

                            sb.Append("<tr>");
                            sb.Append("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");
                            if (dsSignature.Tables[0].Rows.Count > 0 && idx == 0)
                            {
                                sb.Append("<td colspan='5' valign='top' align='left'>");
                                sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");

                                sb.Append("<tr>");
                                sb.Append("<td valign='top'>");
                                sb.Append("<span style='font-weight: bold;'>" + common.myStr(dsSignature.Tables[0].Rows[0]["DoctorName"]).Trim() + "</span>");
                                sb.Append("</td>");
                                sb.Append("</tr>");

                                if (common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine1"]).Trim().Length > 0)
                                {
                                    sb.Append("<tr>");
                                    sb.Append("<td valign='top'>" + common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine1"]).Trim() + "</td>");
                                    sb.Append("</tr>");
                                }
                                if (common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine2"]).Trim().Length > 0)
                                {
                                    sb.Append("<tr>");
                                    sb.Append("<td valign='top'>" + common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine2"]).Trim() + "</td>");
                                    sb.Append("</tr>");
                                }
                                if (common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine3"]).Trim().Length > 0)
                                {
                                    sb.Append("<tr>");
                                    sb.Append("<td valign='top'>" + common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine3"]).Trim() + "</td>");
                                    sb.Append("</tr>");
                                }
                                if (common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine4"]).Trim().Length > 0)
                                {
                                    sb.Append("<tr>");
                                    sb.Append("<td valign='top'>" + common.myStr(dsSignature.Tables[0].Rows[0]["SignatureLine4"]).Trim() + "</td>");
                                    sb.Append("</tr>");
                                }

                                sb.Append("</table>");
                                sb.Append("</td>");
                            }
                            else
                            {
                                sb.Append("<td colspan='5' valign='top'>");
                                sb.Append("</td>");
                            }

                            sb.Append("<td colspan='3' valign='top'>");
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                            sb.Append("<tr>");
                            sb.Append("<td valign='top'>" + (File.Exists(strSingImagePath) ? SignImage : string.Empty) + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");

                            //sb.Append("<td align='center' valign='top'>");
                            //sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                            //sb.Append("<tr>");
                            //sb.Append("<td valign='top'>" + (File.Exists(NABHstrSingImagePath) ? NABHSignImage : string.Empty) + "</td>");
                            //sb.Append("</tr>");
                            //sb.Append("</table>");
                            //sb.Append("</td>");

                            sb.Append("</tr>");

                            //sb.Append("<tr>");
                            //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
                            //sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                    }
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        sb.Append("<table border='0' width='100%' cellpadding='1' cellspacing='1' style='font-size:9pt;'>");
                        for (int idx = 0; idx < ds.Tables[0].Rows.Count; idx++)
                        {
                            DataRow DR = ds.Tables[0].Rows[idx];

                            FileName = common.myStr(DR["LogoImagePath"]);
                            if (FileName != "")
                            {
                                SignImage = "<img width='130px' height='50px' src='" + Server.MapPath("~") + FileName + "' />";
                                strSingImagePath = Server.MapPath("~") + FileName;
                            }

                            NABHFileName = common.myStr(DR["NABHLogoImagePath"]);
                            if (NABHFileName != "")
                            {
                                NABHSignImage = "<img width='40px' height='40px' src='" + Server.MapPath("~") + NABHFileName + "' />";
                                NABHstrSingImagePath = Server.MapPath("~") + NABHFileName;
                            }

                            sb.Append("<tr>");
                            sb.Append("<td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                            sb.Append("</tr>");

                            sb.Append("<tr>");

                            sb.Append("<td colspan='2' valign='top'>");
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                            sb.Append("<tr>");
                            sb.Append("<td valign='top'>" + (File.Exists(strSingImagePath) ? SignImage : string.Empty) + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");

                            sb.Append("<td colspan='5' valign='top'>");
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                            sb.Append("<tr>");
                            sb.Append("<td valign='top'><b>" + common.myStr(DR["FacilityName"]).Trim() + "</b></td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td valign='top'>" + common.myStr(DR["Address1"]).Trim() + ", " + common.myStr(DR["Address2"]).Trim() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<td valign='top'>Phone:" + common.myStr(DR["Phone"]).Trim() + " Fax:" + common.myStr(DR["Fax"]).Trim() + " Email:" + common.myStr(DR["EmailId"]).Trim() + " " + common.myStr(DR["WebSite"]).Trim() + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");

                            sb.Append("<td align='center' valign='top'>");
                            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
                            sb.Append("<tr>");
                            sb.Append("<td valign='top'>" + (File.Exists(NABHstrSingImagePath) ? NABHSignImage : string.Empty) + "</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</td>");

                            sb.Append("</tr>");

                            //sb.Append("<tr>");
                            //sb.Append("<td align ='center'>" + common.myStr(DR["CityName"]) + "(" + common.myStr(DR["PinNo"]) + "), " + common.myStr(DR["StateName"]) + "</td>");
                            //sb.Append("</tr>");
                        }
                        sb.Append("</table>");
                    }
                }
            }
            else
            {
                sb.Append("<br />");
                sb.Append("<br />");
                sb.Append("<br />");
            }

            sb.Append("</div>");


            string ShowDepartmentNameInEMRTempatePrint = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "ShowDepartmentNameInEMRTempatePrint", sConString);

            if (ShowDepartmentNameInEMRTempatePrint.ToUpper().Equals("Y"))
            {
                DataSet dsDoctorDepartmentDetails = new DataSet();

                dsDoctorDepartmentDetails = objivf.getDoctorDepartmentDetails(common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]));

                if (dsDoctorDepartmentDetails.Tables.Count > 0)
                {
                    if (dsDoctorDepartmentDetails.Tables[0].Rows.Count > 0)
                    {
                        sb.Append("<table width='100%' cellpadding='3' cellspacing='0' style='font-size:12pt;font-family:Calibri;'>");
                        sb.Append("<tr><td valign='top' align='center' style='font-size:12pt;font-family:Calibri;font-weight:bold;'>DEPARTMENT OF " + common.myStr(dsDoctorDepartmentDetails.Tables[0].Rows[0]["ConsultingDoctorDepartment"]).ToUpper() + "</td></tr>");
                        sb.Append("</table>");
                    }
                }
            }

            string strPatientHeader = objivf.getCustomizedPatientReportHeaderIncludeTemplate(HeaderId, common.myStr(Session["OPIP"]), common.myInt(Request.QueryString["TemplateId"]));
            if (common.myLen(strPatientHeader).Equals(0))
            {
                sb.Append(getIVFPatient().ToString());
            }
            else
            {
                sb.Append(strPatientHeader);
            }
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            ds.Dispose();
            objivf = null;
        }
        return sb;
    }

    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();

        clsIVF objivf = new clsIVF(sConString);

        ds = objivf.getIVFPatient(common.myInt(Session["RegistrationId"]), 0, common.myInt(Session["EncounterId"]));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "RegistrationId=" + common.myInt(Session["RegistrationId"]);

            DataTable tbl = DV.ToTable();

            if (tbl.Rows.Count > 0)
            {
                DataRow DR = tbl.Rows[0];

                DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
                DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(Session["RegistrationId"]);
                DataTable tblSpouse = DVSpouse.ToTable();

                sb.Append("<div><table border='0' width='50%' style='font-size:10pt; border-collapse:collapse;' ><tr valign='top'>");
                sb.Append("<td >" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td>: " + common.myStr(DR["RegistrationNo"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "PatientName")) + "</td><td>: " + common.myStr(DR["PatientName"]) + "</td>");
                sb.Append("<td >Age/Gender</td><td>: " + common.myStr(DR["Age/Gender"]) + "</td>");
                sb.Append("</tr>");

                if (tblSpouse.Rows.Count > 0)
                {
                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>Spouse</td><td>: " + common.myStr(tblSpouse.Rows[0]["PatientName"]) + "</td>");
                    sb.Append("<td>Spouse Age/Gender</td><td>: " + common.myStr(tblSpouse.Rows[0]["Age/Gender"]) + "</td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr valign='top'>");
                sb.Append("<td>Visit Date</td><td>: " + common.myStr(DR["RegistrationDate"]) + "</td>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "mobile")) + "</td><td>: " + common.myStr(DR["MobileNo"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "Address")) + "</td><td>: " + common.myStr(DR["PatientAddress"]) + "</td>");

                sb.Append("</tr>");

                sb.Append("</table></div>");
            }

            sb.Append("<hr />");

        }
        return sb;
    }

    private string getSignatureDetails(bool IsPrintDoctorSignature)
    {
        string sImage = "";
        StringBuilder sbSign = new StringBuilder();
        string FileName = string.Empty;

        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";

        string SignImage = "";
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        clsIVF objIVF = new clsIVF(sConString);
        try
        {
            string IsShowAllDepartmentDoctorsInEMRTemplate = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "IsShowAllDepartmentDoctorsInEMRTemplate", sConString);

            if (IsShowAllDepartmentDoctorsInEMRTemplate.ToUpper().Equals("Y") && common.myStr(Session["OPIP"]).Equals("I"))
            {
                StringBuilder sbShowAllDepartmentDoctors = new StringBuilder();

                DataSet dsDoctorDepartmentDetails = new DataSet();

                dsDoctorDepartmentDetails = objIVF.getDoctorDepartmentDetails(common.myInt(Session["EncounterId"]), common.myInt(Session["FacilityId"]));

                if (dsDoctorDepartmentDetails.Tables.Count > 0)
                {
                    if (dsDoctorDepartmentDetails.Tables[0].Rows.Count > 0)
                    {
                        sbShowAllDepartmentDoctors.Append(ShowAllDepartmentDoctors(common.myInt(dsDoctorDepartmentDetails.Tables[0].Rows[0]["ConsultingDoctorDepartmentId"]),
                                                common.myStr(dsDoctorDepartmentDetails.Tables[0].Rows[0]["ConsultingDoctorDepartment"]),
                                                common.myStr(dsDoctorDepartmentDetails.Tables[0].Rows[0]["EntrySite"]),
                                                common.myInt(dsDoctorDepartmentDetails.Tables[0].Rows[0]["SpecialisationId"])));

                        if (sbShowAllDepartmentDoctors.ToString() != string.Empty)
                        {
                            sbSign.Append(sbShowAllDepartmentDoctors.ToString());
                        }
                    }
                }
            }
            else
            {
                string IsPrintConsultantSignatureInEMRTemplate = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "InEMRTemplatePrintDoctorSignatureOnFooter", sConString);

                if (IsPrintDoctorSignature)
                {
                    int doctorId = common.myInt(Session["DoctorID"]);

                    string IsPrintLoginDoctorSignatureInEMRTemplate = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                common.myInt(Session["FacilityId"]), "IsPrintLoginDoctorSignatureInEMRTemplate", sConString);

                    if (IsPrintLoginDoctorSignatureInEMRTemplate.Equals("Y"))
                    {
                        if (common.myBool(Session["IsLoginDoctor"]))
                        {
                            doctorId = common.myInt(Session["EmployeeId"]);
                        }
                        else
                        {
                            doctorId = 0;
                        }
                    }

                    if (doctorId > 0)
                    {
                        ds = objIVF.getDoctorSignatureDetails(doctorId);

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FileName = common.myStr(ds.Tables[0].Rows[0]["ImageType"]);
                            if (FileName != "")
                            {
                                SignImage = "<img width='100px' height='80px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                                strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                            }
                        }
                        if (File.Exists(strSingImagePath))
                        {
                            hdnDoctorImage.Value = SignImage;
                        }

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Calibri;font-size:10pt;'>");
                            sbSign.Append("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");

                            if (!common.myStr(HttpContext.Current.Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                            {
                                sbSign.Append("<tr>");
                                sbSign.Append("<td colspan='5'></td>");
                                sbSign.Append("<td colspan='4' align='left'>");
                                sbSign.Append(hdnDoctorImage.Value.ToString());
                                sbSign.Append("</td>");
                                sbSign.Append("</tr>");
                            }

                            sbSign.Append("<tr>");
                            sbSign.Append("<td colspan='5'></td>");
                            sbSign.Append("<td colspan='4' valign='top' align='left'>");
                            sbSign.Append("<span style='font-weight: bold;'>" + common.myStr(ds.Tables[0].Rows[0]["DoctorName"]).Trim() + "</span>");
                            sbSign.Append("</td>");
                            sbSign.Append("</tr>");

                            //sbSign.Append("<tr>");
                            //sbSign.Append("<td colspan='5'></td>");
                            //sbSign.Append("<td colspan='4' align='left'>");
                            //sbSign.Append("Regn. No. : " + common.myStr(ViewState["UPIN"]).Trim());
                            //sbSign.Append("</td>");
                            //sbSign.Append("</tr>");

                            if (common.myStr(HttpContext.Current.Session["FacilityName"]).ToUpper().Contains("HEALTHWORLD"))
                            {
                                sbSign.Append("<tr>");
                                sbSign.Append("<td colspan='5'></td>");
                                sbSign.Append("<td colspan='4' align='left'>");
                                sbSign.Append(common.myStr(ds.Tables[0].Rows[0]["DoctorDesignation"]).Trim()); //common.myStr(ds.Tables[0].Rows[0]["DoctorSpecialization"]).Trim()
                                sbSign.Append("</td>");
                                sbSign.Append("</tr>");
                            }
                            else
                            {
                                if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]).Trim().Length > 0)
                                {
                                    sbSign.Append("<tr>");
                                    sbSign.Append("<td colspan='5'></td>");
                                    sbSign.Append("<td colspan='4' align='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine1"]).Trim() + "</td>");
                                    sbSign.Append("</tr>");
                                }
                                if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]).Trim().Length > 0)
                                {
                                    sbSign.Append("<tr>");
                                    sbSign.Append("<td colspan='5'></td>");
                                    sbSign.Append("<td colspan='4' align='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine2"]).Trim() + "</td>");
                                    sbSign.Append("</tr>");
                                }
                                if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]).Trim().Length > 0)
                                {
                                    sbSign.Append("<tr>");
                                    sbSign.Append("<td colspan='5'></td>");
                                    sbSign.Append("<td colspan='4' align='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine3"]).Trim() + "</td>");
                                    sbSign.Append("</tr>");
                                }
                                if (common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]).Trim().Length > 0)
                                {
                                    sbSign.Append("<tr>");
                                    sbSign.Append("<td colspan='5'></td>");
                                    sbSign.Append("<td colspan='4' align='left'>" + common.myStr(ds.Tables[0].Rows[0]["SignatureLine4"]).Trim() + "</td>");
                                    sbSign.Append("</tr>");
                                }
                            }

                            sbSign.Append("</table>");
                        }
                        else
                        {
                            sbSign.Append("<table cellspacing='0' cellpadding='0' border='0'>");
                            sbSign.Append("<tr><td></td></tr>");
                            sbSign.Append("<tr><td></td></tr>");
                            if (IsPrintConsultantSignatureInEMRTemplate == "Y")
                            {
                                sbSign.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                            }
                            sbSign.Append("</table>");
                        }
                    }
                }
                else
                {
                    sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Candara;font-size:10pt;'>");
                    sbSign.Append("<tr><td></td></tr>");
                    sbSign.Append("<tr><td></td></tr>");
                    if (IsPrintConsultantSignatureInEMRTemplate == "Y")
                    {
                        sbSign.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
                    }
                    sbSign.Append("</table>");
                }
            }

            //else
            //{
            //    EMRPrintBlankConsultantSignatureLabel = objIVF.getEMRPrintBlankConsultantSignatureLabel(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]));
            //    if (EMRPrintBlankConsultantSignatureLabel == "Y")
            //    {
            //        sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Candara;font-size:10pt;'>");
            //        sbSign.Append("<tr><td></td></tr>");
            //        sbSign.Append("<tr><td></td></tr>");
            //        sbSign.Append("<tr><td align='right'><b>Consultant&nbsp;Signature</b></td></tr>");
            //        sbSign.Append("</table>");
            //    }
            //    else
            //    {
            //        sbSign.Append("<table cellspacing='0' cellpadding='0' border='0' style='font-family: Candara;font-size:10pt;'>");
            //        sbSign.Append("<tr><td></td></tr>");
            //        sbSign.Append("<tr><td></td></tr>");
            //        sbSign.Append("<tr><td align='right'><b></b></td></tr>");
            //        sbSign.Append("</table>");
            //    }
            //}
        }
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            strSQL = null;
            dl = null;
            ds.Dispose();
        }

        return sbSign.ToString();
    }


    public StringBuilder ShowAllDepartmentDoctors(int DepartmentId, string ConsultingDoctorDepartment, string EntrySite, int SpecialisationId)
    {
        StringBuilder sbTable = new StringBuilder();
        StringBuilder sbRow = new StringBuilder();

        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hshInput = new Hashtable();

        try
        {
            hshInput.Add("@intDepartmentId", DepartmentId);
            hshInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));

            if (common.myInt(Session["EncounterId"]) > 0)
            {
                hshInput.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
            }

            if (SpecialisationId > 0)
            {
                hshInput.Add("@intSpecialisationId", common.myInt(SpecialisationId));
            }

            DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspGetDepartmentDoctorsList", hshInput);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataView dvCol1 = new DataView();
                DataView dvCol2 = new DataView();
                DataTable tblCol1 = new DataTable();
                DataTable tblCol2 = new DataTable();

                ArrayList colSeq = new ArrayList();

                foreach (DataRow DR in ds.Tables[0].Rows)
                {
                    if (!colSeq.Contains(common.myInt(DR["ColumnSequenceNo"])))
                    {
                        colSeq.Add(common.myInt(DR["ColumnSequenceNo"]));
                    }
                }

                if (colSeq.Count > 0)
                {
                    //DataView DV = new DataView();
                    //DataTable tbl = new DataTable();
                    //for (int idx = 0; idx < colSeq.Count; idx++)
                    //{
                    //    if (idx > 2)//print number of columns
                    //    {
                    //        break;
                    //    }

                    //    DV = ds.Tables[0].DefaultView;
                    //    DV.RowFilter = "";

                    //    DV.RowFilter = "ColumnSequenceNo=" + common.myInt(colSeq[idx]);

                    //    tbl = DV.ToTable();

                    //    if (tbl.Rows.Count > 0)
                    //    {
                    //        foreach (DataRow DR in tbl.Rows)
                    //        {
                    //            sbRow.Append("<tr>");
                    //            sbRow.Append("<td valign='top' align='left'><span style='font-family:MS Sans Serif; font-weight: bold; font-size:9pt;'>" + common.myStr(DR["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:MS Sans Serif;  font-size:8pt;'>" + common.myStr(DR["SignatureLine"]).Trim() + "</span></td>");
                    //            sbRow.Append("</tr>");
                    //        }
                    //    }
                    //}

                    int rowCount = 0;

                    dvCol1 = ds.Tables[0].DefaultView;
                    dvCol1.RowFilter = "ColumnSequenceNo=1";
                    tblCol1 = dvCol1.ToTable();

                    if (tblCol1.Rows.Count > 0)
                    {
                        rowCount = tblCol1.Rows.Count;
                    }

                    dvCol2 = ds.Tables[0].DefaultView;
                    dvCol2.RowFilter = "ColumnSequenceNo=2";
                    tblCol2 = dvCol2.ToTable();

                    if (tblCol2.Rows.Count > 0 && rowCount < tblCol2.Rows.Count)
                    {
                        rowCount = tblCol2.Rows.Count;
                    }

                    for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
                    {
                        sbRow.Append("<tr>");

                        if (tblCol1.Rows.Count > 0)
                        {
                            if (rowIdx < tblCol1.Rows.Count)
                            {
                                sbRow.Append("<td valign='top' align='left'><span style='font-family:Calibri; font-weight: bold; font-size:9pt;'>" + common.myStr(tblCol1.Rows[rowIdx]["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:Calibri; font-size:8pt;'>" + common.myStr(tblCol1.Rows[rowIdx]["SignatureLine"]).Trim() + "</span></td>");
                            }
                            else
                            {
                                sbRow.Append("<td></td>");
                            }
                        }

                        if (tblCol2.Rows.Count > 0)
                        {
                            if (rowIdx < tblCol2.Rows.Count)
                            {
                                sbRow.Append("<td valign='top' align='left'><span style='font-family:Calibri; font-weight: bold; font-size:9pt;'>" + common.myStr(tblCol2.Rows[rowIdx]["ShortDoctorName"]).Trim() + "</span>, <span style='font-family:Calibri; font-size:8pt;'>" + common.myStr(tblCol2.Rows[rowIdx]["SignatureLine"]).Trim() + "</span></td>");
                            }
                            else
                            {
                                sbRow.Append("<td></td>");
                            }
                        }

                        sbRow.Append("</tr>");
                    }

                    if (sbRow.ToString() != string.Empty)
                    {
                        sbTable.Append("<table border='0' cellpadding='0' cellspacing='0' valign='top' align='left' width='100%'>");

                        if (colSeq.Count.Equals(2)) //blank row
                        {
                            sbTable.Append("<tr><td style='color: white;'>.</td><td></td></tr>");
                            //sbTable.Append("<tr><td style='color: white;'>.</td><td></td></tr>");
                        }
                        else
                        {
                            sbTable.Append("<tr><td style='color: white;'>.</td></tr>");
                            //sbTable.Append("<tr><td style='color: white;'>.</td></tr>");
                        }

                        sbTable.Append(sbRow.ToString());

                        sbTable.Append("</table>");
                    }
                }
            }
        }
        catch
        {
        }
        finally
        {
            objDl = null;
            hshInput = null;
        }
        return sbTable;
    }

}
