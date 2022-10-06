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
using BaseC;

public partial class EMRReports_PrintPdfForHTML : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            StringBuilder sbTemplateStyle = new StringBuilder();
            StringBuilder sbTemp = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            DataRow drTemplateStyle = null;
            DataSet ds = new DataSet();
            BindSummary summary = new BindSummary(sConString);
            BaseC.clsLISPhlebotomy ObjIcm = new BaseC.clsLISPhlebotomy(sConString);

            bool bShowAllParameters = common.myStr(Session["ModuleName"]) == "EMR" ? false : true;
            //try
            //{
            if (Request.QueryString["DiagSampleId"] != null && Request.QueryString["EncId"] != null)
            {
                string SOURCE = common.myStr(Request.QueryString["SOURCE"]);
                string LABNO = common.myStr(Request.QueryString["LABNO"]);
                string ServiceIds = common.myStr(Request.QueryString["ServiceIds"]);
                string StationId = common.myStr(Request.QueryString["StationId"]);
                Session["DateDisplay"] = false;
                bool PrintReferenceRangeInHTML = true;
                if (!common.myStr(Request.QueryString["ShowReference"]).Equals(""))
                {
                    PrintReferenceRangeInHTML = common.myBool(Request.QueryString["ShowReference"]);
                }
                if (ViewState["IsDisclaimerRequired"] == null)
                {
                    ViewState["IsDisclaimerRequired"] = common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), "IsDisclaimerRequired", sConString);
                }
                GetHTMLMargin();
                sb = summary.BindLabTestResult(common.myInt(Session["HospitalLocationId"]), common.myInt(Request.QueryString["EncId"]),
                                    common.myInt(Session["FacilityId"]), common.myInt(Session["FacilityId"]), sbTemp, sbTemplateStyle,
                                    drTemplateStyle, Page, common.myStr(Request.QueryString["DiagSampleId"]), "", bShowAllParameters,
                                    PrintReferenceRangeInHTML, string.Empty);

                ds = ObjIcm.uspDiagPrintLabResult(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]), common.myInt(LABNO), common.myInt(StationId), ServiceIds, SOURCE);
                if (sb.ToString() != "")
                {
                    StringBuilder sbReportFooterNote = new StringBuilder();
                    StringBuilder sbEndOfReport = new StringBuilder();
                    sbEndOfReport.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:small;'><tr><td align='center'>** End Of Report **</td></tr></table>");

                    string sbHeaderHtml = getResultDetails(ds).ToString();
                    sbHeaderHtml = sbHeaderHtml.Replace(".", "&#46;");
                    clsParsePDFDischarge cPrint = new clsParsePDFDischarge();
                    //  cPrint.Html = sbPatientSummary.ToString() + GetSignatureDetails() + sbFooterLabel.ToString();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string dtrdoctorsin = string.Empty;
                        if (common.myStr(ds.Tables[0].Rows[0]["RightDoctorName"]).Length > 0)
                        {
                            dtrdoctorsin = common.myStr(getDoctorsDetails());
                        }
                        else
                        {
                            dtrdoctorsin = common.myStr(getDoctorImage(common.myInt(ds.Tables[0].Rows[0]["FinalizedById"])));
                        }
                        if (common.myStr(sb.ToString()) != "</table>")
                        {
                            cPrint.Html = sb.ToString() + "" + sbEndOfReport.ToString() + "" + dtrdoctorsin;

                            sbReportFooterNote.Append(System.Web.HttpContext.Current.Server.HtmlDecode(common.myStr(ds.Tables[0].Rows[0]["ReportFooterNote"])).Replace("<br />", "\r").Replace("<BR />", "\r"));
                        }
                        else
                        {
                            cPrint.Html = "No Data";
                        }
                    }
                    else
                    {

                        if (common.myStr(sb.ToString()) != "</table>")
                        {
                            cPrint.Html = sb.ToString() + "<BR />" + sbEndOfReport.ToString();
                        }
                        else
                        {
                            cPrint.Html = "No Data";
                        }
                    }

                    if (common.myStr(ViewState["IsDisclaimerRequired"]).Equals("Y"))
                    {
                        if (sbReportFooterNote.ToString() != string.Empty)
                        {
                            cPrint.LowestFooter = sbReportFooterNote.ToString() + "\r" + "* This is a computer generated document. No signature required.";
                        }
                        else
                        {
                            //cPrint.LowestFooter = "* This is a computer generated document. No signature required.";
                            cPrint.LowestFooter = "*This is a computer generated report no signatured required.                        Printed at :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\r*Content of this report is only an opinion not the diagnosis.\r*Report shall not be reproduced except in full without written approval of the laboratory";
                        }
                    }
                    else
                    {
                        cPrint.LowestFooter = sbReportFooterNote.ToString();
                    }

                    string strHTML = cPrint.Html;

                    strHTML = common.removeUnusedHTML(strHTML);
                    cPrint.Html = strHTML;

                    cPrint.FirstPageHeaderHtml = sbHeaderHtml;
                    cPrint.HeaderHtml = sbHeaderHtml;
                    if (common.myInt(ViewState["MarginLeft"]) > 0) { cPrint.MarginLeft = common.myInt(ViewState["MarginLeft"]); } else { cPrint.MarginLeft = 20; }
                    if (common.myInt(ViewState["MarginRight"]) > 0) { cPrint.MarginRight = common.myInt(ViewState["MarginRight"]); } else { cPrint.MarginRight = 20; }
                    if (common.myInt(ViewState["MarginTop"]) > 0) { cPrint.MarginTop = common.myInt(ViewState["MarginTop"]); } else { cPrint.MarginTop = 75; }
                    if (common.myInt(ViewState["MarginBottom"]) > 0) { cPrint.MarginBottom = common.myInt(ViewState["MarginBottom"]); } else { cPrint.MarginBottom = 30; }
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
            }
            //}
            //catch (Exception ex)
            //{
            //}
            //finally
            //{
            //    ds.Dispose();
            //    sbTemplateStyle = null;
            //    sbTemp = null;
            //    sb = null;
            //    summary = null;
            //    ObjIcm = null;
            //}
        }
    }

    private StringBuilder getDoctorsDetails()
    {
        BaseC.Hospital objHospital = new BaseC.Hospital(sConString);

        StringBuilder sb = new StringBuilder();
        string sLeftEducation = string.Empty, sRightEducation = string.Empty, sCenterEducation = string.Empty, sRightDoctor = string.Empty, sCenterDoctor = string.Empty, strSignImagePath = string.Empty, sLeftDesignation = string.Empty, sRightDesignation = string.Empty, sCenterDesignation = string.Empty, Leftdoctorname = "", LeftImageType = string.Empty, RightImageType = string.Empty, CenterImageType = string.Empty, LeftImage = string.Empty, RightImage = string.Empty, CenterImage = string.Empty;
        string SOURCE = common.myStr(Request.QueryString["SOURCE"]), sLeftImagePath = string.Empty, sRightImagePath = string.Empty, sCenterImagePath = string.Empty;
        string LABNO = common.myStr(Request.QueryString["LABNO"]);
        DataSet dsFooter = objHospital.GetReportFooter(SOURCE, common.myInt(LABNO), common.myStr(Request.QueryString["ServiceIds"]), common.myInt(Session["FacilityId"]));

        if (dsFooter.Tables.Count > 0)
        {
            if (dsFooter.Tables[0].Rows.Count > 0)
            {
                LeftImageType = common.myStr(dsFooter.Tables[0].Columns.Contains("LeftImageType") ? dsFooter.Tables[0].Rows[0]["LeftImageType"] : "");
                RightImageType = common.myStr(dsFooter.Tables[0].Columns.Contains("RightImageType") ? dsFooter.Tables[0].Rows[0]["RightImageType"] : "");
                CenterImageType = common.myStr(dsFooter.Tables[0].Columns.Contains("CenterImageType") ? dsFooter.Tables[0].Rows[0]["CenterImageType"] : "");
                Leftdoctorname = common.myStr(dsFooter.Tables[0].Rows[0]["LeftDoctorName"]);
                sRightDoctor = common.myStr(dsFooter.Tables[0].Rows[0]["RightDoctorName"]);
                sCenterDoctor = common.myStr(dsFooter.Tables[0].Rows[0]["CenterDoctorName"]);
                sLeftDesignation = common.myStr(dsFooter.Tables[0].Rows[0]["leftDesignation"]);
                sRightDesignation = common.myStr(dsFooter.Tables[0].Rows[0]["RightDesignation"]);
                sCenterDesignation = common.myStr(dsFooter.Tables[0].Rows[0]["CenterDsignation"]);
                sLeftEducation = common.myStr(dsFooter.Tables[0].Rows[0]["LeftDoctorEducation"]);
                sRightEducation = common.myStr(dsFooter.Tables[0].Rows[0]["RightDoctorEducation"]);
                sCenterEducation = common.myStr(dsFooter.Tables[0].Rows[0]["CenterDoctorEducation"]);

                if (LeftImageType != "")
                {
                    sLeftImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + LeftImageType;
                    if (File.Exists(sLeftImagePath))
                    {
                        LeftImage = "<img width='150px' height='50px' src='.../PatientDocuments/DoctorImages/" + LeftImageType + "' />";

                    }
                }
                if (RightImageType != "")
                {
                    sRightImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + RightImageType;
                    if (File.Exists(sRightImagePath))
                    {
                        RightImage = "<img width='150px' height='50px' src='.../PatientDocuments/DoctorImages/" + RightImageType + "' />";

                    }
                }
                if (CenterImageType != "")
                {
                    sCenterImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + CenterImageType;
                    if (File.Exists(sCenterImagePath))
                    {
                        CenterImage = "<img width='150px' height='50px' src='.../PatientDocuments/DoctorImages/" + CenterImageType + "' />";
                    }
                }

                sb.Append("<table border='0' valign='bottom' style='width:100%; font-size:10pt; bottom:0;' cellpadding='0' cellspacing='0'>");
                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                if (common.myInt(LeftImage.Length) > 0 || common.myInt(CenterImage.Length) > 0 || common.myInt(RightImage.Length) > 0)
                {
                    sb.Append("<tr><td colspan='2'>" + LeftImage + "</td><td colspan='2'>" + CenterImage + "</td><td colspan='2'>" + RightImage + "</td></tr>");
                }
                sb.Append("<tr><td colspan='2'>" + Leftdoctorname + "</td><td colspan='2'>" + sCenterDoctor + "</td><td colspan='2'>" + sRightDoctor + "</td></tr>");
                sb.Append("<tr><td colspan='2'>" + sLeftDesignation + "</td><td colspan='2'>" + sCenterDesignation + "</td><td colspan='2'>" + sRightDesignation + "</td></tr></table>");
            }
        }
        return sb;

    }

    private StringBuilder getResultDetails(DataSet ds)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable tbl = ds.Tables[0].Copy();

                if (tbl.Rows.Count > 0)
                {
                    DataView dv = new DataView(tbl);
                    dv.RowFilter = "ShowInReportHeader='True'";
                    DataTable tb1l = dv.ToTable();
                    //if(tbl.Rows.Count>0)
                    //{
                    //    ViewState["ShowInReportHeader"] = tb1l.Rows[0]["Result"].ToString();
                    //}

                    //sb.Append(facilityImage().ToString());

                    DataRow DR = tbl.Rows[0];
                    if (common.myStr(DR["LabType"]) != "X")
                    {
                        string sSampleDate = common.myStr(DR["LabType"]) == "G" ? "Sample Date" : "Order Date";
                        sb.Append("<table border='0' width='100%' style='font-size:large; border-collapse:collapse;' cellpadding='2' cellspacing='1' >");
                        sb.Append("<tr valign='top' align='center'>");
                        //  sb.Append("<td><b>DEPARTMENT OF LABORATORY MEDICINE</b></td>");
                        sb.Append("<td><b>" + common.myStr(DR["ReportHeaderText1"]) + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table border='0' style='width:100%; font-size:9pt;' cellpadding='1' cellspacing='2'>");
                        sb.Append("<tr valign='top'>");
                        sb.Append("<td align='right'>" + common.myStr(DR["ReportFormatNumber"]) + "</td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table border='1' style='width:100%;' cellpadding='2' cellspacing='1'><tr><td>");
                        sb.Append("<table border='0' style='width:100%; font-size:10pt;' cellpadding='1' cellspacing='2' >");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Patient Name</b></td><td colspan='3'>" + common.myStr(DR["PatientName"]).Trim() + "</td>");
                        sb.Append("<td><b>Lab No</b></td><td colspan='2'>" + common.myStr(DR["LabNo"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</b></td><td colspan='3'>" + common.myStr(DR["RegistrationNo"]) + "</td>");
                        sb.Append("<td><b>" + sSampleDate + "</b></td><td colspan='2'>" + common.myStr(DR["SampleCollectedDate"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Age/Gender</b></td><td colspan='3'>" + common.myStr(DR["Age"]) + "</td>");
                        sb.Append("<td><b>Receiving&nbsp;Date</b></td><td colspan='2'>" + common.myStr(DR["SampleAcknowledgedDate"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Bed No / Ward</b></td><td colspan='3'>" + (common.myStr(DR["Ward"]).Trim() == "" ? "OPD" : common.myStr(DR["Ward"]).Trim()) + "</td>");
                        sb.Append("<td><b>Report Date</b></td><td colspan='2'>" + common.myStr(DR["ResultDate"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Referred By</b></td><td colspan='3'>" + common.myStr(DR["ReferredBy"]) + "</td>");
                        sb.Append("<td><b>Report Status</b></td><td colspan='2'>" + common.myStr(DR["DisplayReportStatus"]) + "</td>");
                        sb.Append("</tr>");

                        //sb.Append("<tr valign='top'>");
                        //sb.Append("<td colspan='2'><b>Report Stage</b></td><td colspan='3'>" + common.myStr(DR["DisplayReportStatus"]) + "</td>");
                        //sb.Append("<td><b>Test Priority</b></td><td colspan='2'>" + (common.myBool(DR["Stat"]) ? "Stat" : "Routine") + "</td>");
                        //sb.Append("</tr>");

                        foreach (DataRow dr1 in tb1l.Rows)
                        {
                            if (common.myBool(dr1["ShowInReportHeader"]))
                            {
                                sb.Append("<tr valign='top'>");
                                sb.Append("<td><b>" + common.myStr(dr1["FieldName"]) + "</b></td><td>" + common.myStr(dr1["Result"]) + "</td>");
                                sb.Append("</tr>");
                            }
                        }

                        sb.Append("</table>");
                        sb.Append("</td></tr></table>");

                        sb.Append("<table border='0'  style='width:100%; font-size:10pt;' cellpadding='0' cellspacing='0'>");
                        if (!string.IsNullOrEmpty(common.myStr(DR["SubName"])))
                        {
                            sb.Append("<tr><td align='center'><b>" + common.myStr(DR["SubName"]) + "</b></td></tr><tr><td></td></tr>");
                        }
                        //else if (!string.IsNullOrEmpty(common.myStr(DR["ReportHeaderText2"])))
                        //{
                        //    sb.Append("<td colspan='2' align='center'><b>" + common.myStr(DR["ReportHeaderText2"]) + "</b></td></tr></table>");
                        //}
                        //else
                        //{
                        //    sb.Append("<td align='center'><b>" + common.myStr(DR["ServiceName"]) + "</b></td></tr></table>");
                        //}
                        if (common.myStr(DR["PrintServiceNameInReport"]) == "True")
                        {
                            sb.Append("<tr><td align='left'><b>" + common.myStr(DR["ServiceName"]) + "</b></td></tr>");
                        }
                        sb.Append("<tr><td align='center'></td></tr></table>");
                    }
                    else
                    {
                        sb.Append("<table border='0' width='100%' style='font-size:large; border-collapse:collapse;' cellpadding='2' cellspacing='0' >");
                        sb.Append("<tr valign='top' align='center'>");
                        sb.Append("<td><b>" + common.myStr(DR["ReportHeaderText1"]) + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        //sb.Append("<table border='0' style='width:100%; font-size:9pt;' cellpadding='1' cellspacing='2'>");
                        //sb.Append("<tr valign='top'>");
                        //sb.Append("<td align='right'>" + common.myStr(DR["ReportFormatNumber"]) + "</td>");
                        //sb.Append("</tr>");
                        //sb.Append("</table>");

                        sb.Append("<table border='1' style='width:100%;' cellpadding='2' cellspacing='1'><tr><td>");

                        sb.Append("<table border='0' style='width:100%; font-size:9pt;' cellpadding='1' cellspacing='2' >");

                        //sb.Append("<tr valign='top'>");
                        //sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</b></td><td colspan='3'>" + common.myStr(DR["RegistrationNo"]) + "</td>");
                        sb.Append("<td><b>RIS No</b></td><td colspan='2'>" + common.myStr(DR["LabNo"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Patient Name</b></td><td colspan='3'>" + common.myStr(DR["PatientName"]).Trim() + "</td>");
                        sb.Append("<td><b>Age/Gender</b></td><td colspan='2'>" + common.myStr(DR["Age"]) + "</td>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Referred By</b></td><td colspan='3'>" + common.myStr(DR["ReferredBy"]) + "</td>");
                        sb.Append("<td><b>Bed No / Ward</b></td><td colspan='2'>" + (common.myStr(DR["Ward"]).Trim() == "" ? "OPD" : common.myStr(DR["Ward"]).Trim()) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Bill Date</b></td><td colspan='3'>" + common.myStr(DR["OrderDate"]) + "</td>");
                        sb.Append("<td><b>Scan Date</b></td><td colspan='2'>" + common.myStr(DR["ScanOutTime"]) + "</td>");
                        sb.Append("</tr>");

                        sb.Append("</tr>");
                        sb.Append("<tr valign='top'>");
                        sb.Append("<td><b>Report Date</b></td><td colspan='3'>" + common.myStr(DR["ResultDate"]) + "</td>");
                        sb.Append("<td><b>Report Status</b></td><td colspan='2'>" + common.myStr(DR["DisplayReportStatus"]) + "</td>");
                        sb.Append("</tr>");

                        foreach (DataRow dr1 in tb1l.Rows)
                        {
                            if (common.myStr(dr1["ShowInReportHeader"]).ToUpper() == "TRUE")
                            {
                                sb.Append("<tr valign='top'>");

                                sb.Append("<td><b>" + common.myStr(dr1["FieldName"]) + "</b></td><td colspan='3'>" + common.myStr(dr1["Result"]) + "</td>");

                                //if (i == 1)
                                //{
                                //    i++;
                                //    sb.Append("<td>Referred By</td><td colspan='2'><b>" + common.myStr(DR["ReferredBy"]) + "</b></td>");
                                //}
                                //else
                                //{
                                //    sb.Append("<td>&nbsp;</td><td colspan='2'>&nbsp;</td>");
                                //}
                                sb.Append("<td>&nbsp;</td><td colspan='2'>&nbsp;</td>");
                                sb.Append("</tr>");
                            }
                        }

                        sb.Append("</table>");

                        sb.Append("</td></tr></table>");

                        sb.Append("<table border='0'  style='width:100%; font-size:10pt;' cellpadding='0' cellspacing='0'>");
                        if (common.myStr(DR["PrintServiceNameInReport"]) == "True")
                        {
                            sb.Append("<tr><td align='center'><b>" + common.myStr(DR["ServiceName"]) + "</b></td></tr>");
                        }
                        sb.Append("</table>");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //lblMessage.Text = ex.ToString();
            //lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        return sb;
    }
    public string Addvalue(string Caption, DataSet ds)
    {
        string value = "";
        if (common.myStr(Caption) == "PN")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["PatientName"]);
        }
        if (common.myStr(Caption) == "RN")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"]);
        }
        if (common.myStr(Caption) == "SD")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["SampleCollectedDate"]);
        }
        if (common.myStr(Caption) == "PGA")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["Age"]);
        }
        if (common.myStr(Caption) == "EN")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]);
        }
        if (common.myStr(Caption) == "RD")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["ResultDate"]);
        }
        if (common.myStr(Caption) == "BNW")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["Ward"]);
        }
        if (common.myStr(Caption) == "LN")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["LabNo"]);
        }
        if (common.myStr(Caption) == "RS")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["ReportStatus"]);
        }
        if (common.myStr(Caption) == "RB")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["Referred By"]);
        }
        if (common.myStr(Caption) == "PM")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["Nationality_Name"]);
        }
        if (common.myStr(Caption) == "CN")
        {
            value = common.myStr(ds.Tables[0].Rows[0]["CompanyName"]);

        }
        return value;
    }

    private StringBuilder getDoctorImage(int FinalizedBy)
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.Hospital objHospital = new BaseC.Hospital(sConString);

        BaseC.User user = new BaseC.User(sConString);
        StringBuilder sb = new StringBuilder();
        String SignImage = "", strSignImagePath = "", FileName = "";
        string Designation = "", FinalizedByName = "";

        string Leftdoctordesignation = "", Leftdoctorname = "";

        string SOURCE = common.myStr(Request.QueryString["SOURCE"]);
        string LABNO = common.myStr(Request.QueryString["LABNO"]);
        DataSet ds = lis.getDoctorImageDetails(common.myInt(FinalizedBy), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), 0);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                FileName = common.myStr(ds.Tables[0].Rows[0]["ImageType"]);
                if (FileName != "")
                {
                    SignImage = "<img width='150px' height='50px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                    strSignImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                }
                Designation = common.myStr(ds.Tables[0].Rows[0]["Designation"]).Trim();
                FinalizedByName = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]).Trim();

            }
        }


        DataSet dsFooter = objHospital.GetReportFooter(SOURCE, common.myInt(LABNO), common.myStr(Request.QueryString["ServiceIds"]), common.myInt(Session["FacilityId"]));
        StringBuilder sb1 = new StringBuilder();

        if (dsFooter.Tables.Count > 0)
        {
            if (dsFooter.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(common.myStr(dsFooter.Tables[0].Rows[0]["RightDoctorName"])))
                {
                    Leftdoctorname = common.myStr(dsFooter.Tables[0].Rows[0]["RightDoctorName"]) + " " + "(" + common.myStr(dsFooter.Tables[0].Rows[0]["RightDoctorEducation"]) + ")";
                }
                else
                {
                    Leftdoctorname = common.myStr(dsFooter.Tables[0].Rows[0]["LeftDoctorName"]) + " " + common.myStr(dsFooter.Tables[0].Rows[0]["LeftDoctorEducation"]);
                }
                Leftdoctordesignation = common.myStr(dsFooter.Tables[0].Rows[0]["leftDesignation"]);

                sb.Append("<table border='0' valign='bottom' style='width:100%; font-size:10pt; bottom:0;' cellpadding='0' cellspacing='0'>");
                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>");
                if (File.Exists(strSignImagePath))
                {
                    sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan='2'>" + SignImage + "</td></tr>");
                }
                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan='2'>" + ((common.myLen(FinalizedByName) > 0) ? FinalizedByName : Leftdoctorname) + "</td></tr>");
                sb.Append("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td colspan='2'>" + ((common.myLen(FinalizedByName) > 0) ? Designation : Leftdoctordesignation) + "</td></tr></table>");

            }
        }
        return sb;
    }

    private StringBuilder facilityImage()
    {
        StringBuilder sbLogo = new StringBuilder();

        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        Hashtable hstInput = new Hashtable();
        string SignImage = "";
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);

        hstInput.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
        hstInput.Add("@intHospId", common.myInt(Session["HospitalLocationId"]));

        strSQL.Append("select LogoImagePath from FacilityMaster WITH(NOLOCK) where FacilityID=@intFacilityId and HospitalLocationID=@intHospId and Active=1");

        DataSet ds = dl.FillDataSet(CommandType.Text, strSQL.ToString(), hstInput);
        if (ds.Tables[0].Rows.Count > 0)
        {
            string FileName = common.myStr(ds.Tables[0].Rows[0]["LogoImagePath"]);
            if (FileName != "")
            {
                SignImage = "<img width='146px' height='48px' src='" + Server.MapPath("~") + FileName + "' />";
                strSingImagePath = Server.MapPath("~") + FileName;
            }
        }
        if (File.Exists(strSingImagePath))
        {
            sbLogo.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0'>");
            sbLogo.Append("<tr><td align='right'>" + SignImage + "</td></tr>");
            sbLogo.Append("</table>");
            sbLogo.Append("<br /><br />");
        }

        return sbLogo;
    }
    private void GetHTMLMargin()
    {
        clsLISSampleReceivingStation oclsLISSampleReceivingStation = new clsLISSampleReceivingStation(sConString);
        DataSet dsGetHTMLMargin = new DataSet();
        dsGetHTMLMargin = oclsLISSampleReceivingStation.GetStation(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["StationId"]));
        if (dsGetHTMLMargin.Tables[0].Rows.Count > 0)
        {

            if (dsGetHTMLMargin.Tables[0].Columns.Contains("MarginTop"))
            {
                ViewState["MarginTop"] = common.myStr(dsGetHTMLMargin.Tables[0].Rows[0]["MarginTop"]);
            }
            if (dsGetHTMLMargin.Tables[0].Columns.Contains("MarginLeft"))
            {
                ViewState["MarginLeft"] = common.myStr(dsGetHTMLMargin.Tables[0].Rows[0]["MarginLeft"]);
            }
            if (dsGetHTMLMargin.Tables[0].Columns.Contains("MarginRight"))
            {
                ViewState["MarginRight"] = common.myStr(dsGetHTMLMargin.Tables[0].Rows[0]["MarginRight"]);
            }
            if (dsGetHTMLMargin.Tables[0].Columns.Contains("MarginBottom"))
            {
                ViewState["MarginBottom"] = common.myStr(dsGetHTMLMargin.Tables[0].Rows[0]["MarginBottom"]);
            }
        }
        dsGetHTMLMargin = null;
    }

}
