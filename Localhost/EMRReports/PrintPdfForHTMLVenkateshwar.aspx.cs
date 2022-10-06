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
                int iStationId = 0;
                Session["DateDisplay"] = false;
                bool PrintReferenceRangeInHTML = true;
                if (!common.myStr(Request.QueryString["ShowReference"]).Equals(""))
                {
                    PrintReferenceRangeInHTML = common.myBool(Request.QueryString["ShowReference"]);
                }
                sb = BindLabTestResult(common.myInt(Session["HospitalLocationId"]), common.myInt(Request.QueryString["EncId"]),
                    common.myInt(Session["FacilityId"]), common.myInt(Session["FacilityId"]), sbTemp, sbTemplateStyle, drTemplateStyle, Page, common.myStr(Request.QueryString["DiagSampleId"]), "", bShowAllParameters, PrintReferenceRangeInHTML);
               iStationId = GetStationId(common.myStr(Request.QueryString["DiagSampleId"]));
                ds = ObjIcm.uspDiagPrintLabResult(common.myInt(Session["HospitalLocationID"]), common.myInt(Session["FacilityId"]),
                                        common.myInt(LABNO), common.myInt(iStationId), ServiceIds, SOURCE);
                if (sb.ToString() != "")
                {
                    string sbLabTestResult = sb.ToString();
                    sbLabTestResult = sbLabTestResult.Replace(".", "&#46;");
                    
                    sb = new StringBuilder();
                    sb.Append(sbLabTestResult);
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
                        //if (common.myStr(ds.Tables[0].Rows[0]["RightDoctorName"]).Length > 0)
                        //{
                            dtrdoctorsin = common.myStr(getDoctorsDetails());
                        //}
                        //else
                        //{
                        //    dtrdoctorsin = common.myStr(getDoctorImage(common.myInt(ds.Tables[0].Rows[0]["FinalizedById"])));
                        //}
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

                    string sFooterNote = "• This is a computer generated report no signatured required.                        Printed at :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\r•Content of this report is only an opinion not the diagnosis.\r*Report shall not be reproduced except in full without written approval of the laboratory";
                    string sFooterAddNote = string.Empty, sAccreditationNote= string.Empty;
                    if (common.myStr(ds.Tables[0].Rows[0]["ReportStatus"]).Contains("Final")) { sFooterAddNote = "This is computer generated report no signature required."; }
                    else { sFooterAddNote = "This is only a provisional/interim report made available for the benefit of clinician for prompt treatment. Final report shall follow."; }

                    sAccreditationNote = "• Tests marked with (*) are not in the Scope of NABL Accreditation. ";
                    if (common.myStr(ds.Tables[0].Rows[0]["LabType"]) == "X")
                    {
                        
                           sFooterNote = "• Clinical correlation is essential for final diagnosis. This report is not valid for medicolegal purpose. " + sFooterAddNote;
                    }
                    else
                    {
                        sFooterNote = sAccreditationNote + "• Clinical correlation is essential for final diagnosis. If the test result are unexpected please contact the laboratory. This report is not valid for medicolegal purpose. " +"• "+ sFooterAddNote;
                    }

                    if (sbReportFooterNote.ToString() != string.Empty)
                    {
                        cPrint.LowestFooter = sbReportFooterNote.ToString();// + "\r" + "* This is a computer generated document. No signature required.";
                    }
                    else
                    {
                        cPrint.LowestFooter = sFooterNote;
                        //"*This is a computer generated report no signatured required.                        Printed at :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\r*Content of this report is only an opinion not the diagnosis.\r*Report shall not be reproduced except in full without written approval of the laboratory";
                    }
                    cPrint.FirstPageHeaderHtml = sbHeaderHtml;
                    cPrint.HeaderHtml = sbHeaderHtml;
                    cPrint.MarginLeft = 20;
                    cPrint.MarginRight = 20;
                    cPrint.MarginTop = 60;
                    cPrint.MarginBottom = 30;
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
        //    catch (Exception ex)
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

    private int GetStationId(string sDiagSampleId)
    {
        int iStationid = 0;
        DataSet ds = new DataSet();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        try
        {
            Hashtable HshIn = new Hashtable();
            Hashtable HshOut = new Hashtable();
            HshIn.Add("@xmlDiagSampleIds", sDiagSampleId);
            ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagGetStationBySampleId", HshIn, HshOut);
            if (common.myInt(ds.Tables[0].Rows.Count) > 0)
            {
                iStationid = common.myInt(ds.Tables[0].Rows[0]["StationId"]);
            }
            return iStationid;
        }
        catch (Exception ex)
        {
            return iStationid;
        }
        finally
        {
            ds = null;
            objDl = null;
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
                sb.Append("<tr><td colspan='2'>" + sLeftEducation + "</td><td colspan='2'>" + sCenterEducation + "</td><td colspan='2'>" + sRightEducation + "</td></tr>");
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
                    DataRow DR = tbl.Rows[0];
                    if (common.myStr(DR["LabType"]) != "X")
                    {
                        sb.Append("<table border='0' width='100%' style='font-size:medium; border-collapse:collapse;' cellpadding='2' cellspacing='3' >");
                        sb.Append("<tr valign='top' align='center'>");
                        //  sb.Append("<td><b>DEPARTMENT OF LABORATORY MEDICINE</b></td>");
                        sb.Append("<td><b>" + common.myStr(DR["ReportHeaderText1"]) + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table border='1' style='width:100%;' cellpadding='2' cellspacing='1'><tr><td>");

                        sb.Append("<table border='0' style='width:100%; font-size:9pt;' cellpadding='1' cellspacing='2' >");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Patient Name</td><td colspan='3'> <b>" + common.myStr(DR["PatientName"]).Trim() + "</b></td>");
                        sb.Append("<td>Order Date</td><td colspan='2'> <b>" + common.myStr(DR["SampleCollectedDate"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Age/Gender</td><td colspan='3'> <b>" + common.myStr(DR["Age"]) + "</b></td>");
                        sb.Append("<td>Ack. Date</td><td colspan='2'> <b>" + common.myStr(DR["SampleAcknowledgedDate"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td colspan='3'> <b>" + common.myStr(DR["RegistrationNo"]) + "</b></td>");
                        sb.Append("<td>Report Date</td><td colspan='2'> <b>" + common.myStr(DR["ResultDate"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Bed No / Ward</td><td colspan='3'> <b>" + (common.myStr(DR["Ward"]).Trim() == "" ? "OPD" : common.myStr(DR["Ward"]).Trim()) + "</b></td>");
                        sb.Append("<td>Lab No</td><td colspan='2'> <b>" + common.myStr(DR["LabNo"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Referred By</td><td colspan='3'> <b>" + common.myStr(DR["ReferredBy"]).Trim() + "</b></td>");
                        sb.Append("<td>Report Status</td><td colspan='2'> <b>" + common.myStr(DR["ReportStatus"]) + "</b></td>");
                        sb.Append("</tr>");

                        //santosh chaurasia -- ShowInReportHeader column if true then show column in report header.
                        int i = 1;
                        //if (common.myStr(ViewState["ShowInReportHeader"]) != "")
                        foreach (DataRow dr1 in tb1l.Rows)
                        {
                            if (common.myStr(dr1["ShowInReportHeader"]).ToUpper() == "TRUE")
                            {
                                sb.Append("<tr valign='top'>");

                                sb.Append("<td colspan='2'>" + common.myStr(dr1["FieldName"]) + "</td><td colspan='3'><b>" + common.myStr(dr1["Result"]) + "</b></td>");

                                if (i == 1)
                                {
                                    i++;
                                    sb.Append("<td>Referred By</td><td colspan='2'><b>" + common.myStr(DR["ReferredBy"]) + "</b></td>");
                                }
                                else if (i == 2)
                                {
                                    sb.Append("<td>Report Status</td><td colspan='2'> <b>" + common.myStr(DR["ReportStatus"]) + "</b></td>");
                                }
                                else
                                {
                                    sb.Append("<td>&nbsp;</td><td colspan='2'>&nbsp;</td>");
                                }

                                sb.Append("</tr>");
                            }
                            else
                            {
                                sb.Append("<tr valign='top'>");
                                sb.Append("<td colspan='2'>&nbsp;</td><td colspan='3'>&nbsp;</td>");
                                sb.Append("<td>Referred By</td><td colspan='2'><b>" + common.myStr(DR["ReferredBy"]) + "</b></td>");
                                sb.Append("</tr>");
                            }
                        }

                        //sb.Append("<tr valign='top'>");
                        //sb.Append("<td colspan='2'>&nbsp;</td><td colspan='3'>&nbsp;</td>");
                        //sb.Append("<td>Test Priority</td><td colspan='2'> <b>" + (common.myBool(DR["Stat"]) ? "Stat" : "Routine") + "</b></td>");
                        //sb.Append("</tr>");
                        sb.Append("</table></td></tr></table>");
                        sb.Append("<table border='0'  style='width:100%; font-size:9pt;' cellpadding='0' cellspacing='0'><tr>");
                        if (!string.IsNullOrEmpty(common.myStr(DR["SubName"])))
                        {
                            sb.Append("<td align='center'> <b>" + common.myStr(DR["SubName"]) + " </b></td></tr></table>");
                        }
                        else if (!string.IsNullOrEmpty(common.myStr(DR["ReportHeaderText2"])))
                        {
                            sb.Append("<td colspan='2' align='center'> <b>" + common.myStr(DR["ReportHeaderText2"]) + " </b></td></tr></table>");
                        }
                        else
                        {
                            sb.Append("<td align='center'> <b>" + common.myStr(DR["ServiceName"]) + " </b></td></tr></table>");
                        }
                    }
                    else
                    {
                        sb.Append("<table border='0' width='100%' style='font-size:large; border-collapse:collapse;' cellpadding='2' cellspacing='3' >");
                        sb.Append("<tr valign='top' align='center'>");
                        sb.Append("<td><b>" + common.myStr(DR["ReportHeaderText1"]) + "</b></td>");
                        sb.Append("</tr>");
                        sb.Append("</table>");

                        sb.Append("<table border='1' style='width:100%;' cellpadding='2' cellspacing='1'><tr><td>");

                        sb.Append("<table border='0' style='width:100%; font-size:9pt;' cellpadding='1' cellspacing='2' >");

                        //sb.Append("<tr valign='top'>");
                        //sb.Append("<td>  &nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Patient Name</td><td colspan='3'> <b>" + common.myStr(DR["PatientName"]).Trim() + "</b></td>");
                        sb.Append("<td>Request Date</td><td colspan='2'> <b>" + common.myStr(DR["OrderDate"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Age/Gender</td><td colspan='3'> <b>" + common.myStr(DR["Age"]) + "</b></td>");
                        sb.Append("<td>Ack. Date</td><td colspan='2'> <b>" + common.myStr(DR["SampleAcknowledgedDate"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td colspan='3'> <b>" + common.myStr(DR["RegistrationNo"]) + "</b></td>");
                        sb.Append("<td>Report Date</td><td colspan='2'> <b>" + common.myStr(DR["ResultDate"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Bed No / Ward</td><td colspan='3'> <b>" + (common.myStr(DR["Ward"]).Trim() == "" ? "OPD" : common.myStr(DR["Ward"]).Trim()) + "</b></td>");
                        sb.Append("<td>Lab No</td><td colspan='2'> <b>" + common.myStr(DR["LabNo"]) + "</b></td>");
                        sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>Referred By</td><td colspan='3'> <b>" + common.myStr(DR["ReferredBy"]).Trim() + "</b></td>");
                        sb.Append("<td>Report Status</td><td colspan='2'> <b>" + common.myStr(DR["ReportStatus"]) + "</b></td>");
                        sb.Append("</tr>");
                        //santosh chaurasia --  show Top in header column in header data.
                        int i = 1;
                        //if (common.myStr(ViewState["ShowInReportHeader"]) != "")
                        foreach (DataRow dr1 in tb1l.Rows)
                        {
                            if (common.myStr(dr1["ShowInReportHeader"]).ToUpper() == "TRUE")
                            {
                                sb.Append("<tr valign='top'>");

                                sb.Append("<td colspan='2'>" + common.myStr(dr1["FieldName"]) + "</td><td colspan='3'><b>" + common.myStr(dr1["Result"]) + "</b></td>");

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
                            else
                            {
                                //sb.Append("<tr valign='top'>");
                                //sb.Append("<td colspan='2'>&nbsp;</td><td colspan='3'>&nbsp;</td>");
                                //sb.Append("<td>Referred By</td><td colspan='2'><b>" + common.myStr(DR["ReferredBy"]) + "</b></td>");
                                //sb.Append("</tr>");
                            }
                        }
                        //end

                        //sb.Append("<tr valign='top'>");
                        //sb.Append("<td colspan='2'>&nbsp;</td><td colspan='3'>&nbsp;</td>");
                        //sb.Append("<td>&nbsp;</td><td colspan='2'>&nbsp;</td>");
                        //sb.Append("</tr>");

                        sb.Append("<tr valign='top'>");
                        sb.Append("<td colspan='2'>&nbsp;</td><td colspan='3'>&nbsp;</td>");
                        sb.Append("<td>&nbsp;</td><td colspan='2'>&nbsp;</td>");
                        sb.Append("</tr>");

                        sb.Append("</table>");

                        sb.Append("</td></tr></table>");

                        sb.Append("<table border='0'  style='width:100%; font-size:12pt;' cellpadding='0' cellspacing='0'><tr>");
                        if (!string.IsNullOrEmpty(common.myStr(DR["SubName"])))
                        {
                            sb.Append("<td align='center'> <b>" + common.myStr(DR["SubName"]) + " </b></td></tr></table>");
                        }
                        else if (!string.IsNullOrEmpty(common.myStr(DR["ReportHeaderText2"])))
                        {
                            sb.Append("<td colspan='2' align='center'> <b>" + common.myStr(DR["ReportHeaderText2"]) + " </b></td></tr></table>");
                        }
                        else
                        {
                            sb.Append("<td align='center'> <b>" + common.myStr(DR["ServiceName"]) + " </b></td></tr></table>");
                        }

                    }
                }
            }

            //hdnReportContent.Value = sb.ToString();
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
    //public Image byteArrayToImage(byte[] byteArrayIn)
    //{
    //    MemoryStream ms = new MemoryStream(byteArrayIn);
    //    Image returnImage = Image.FromStream(ms);
    //    return returnImage;
    //}
    private StringBuilder getDoctorImage(int FinalizedBy)
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);

        BaseC.User user = new BaseC.User(sConString);
        StringBuilder sb = new StringBuilder();
        String SignImage = "", strSignImagePath = "", FileName = "";
        string Designation = "", FinalizedByName = "", Education = string.Empty;
        DataSet ds = lis.getDoctorImageDetails(common.myInt(FinalizedBy), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]), common.myInt(Request.QueryString["EncId"]));

        ViewState["strSignImagePath"] = "";
        ViewState["Designation"] = "";
        ViewState["FinalizedByName"] = "";

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                FileName = common.myStr(ds.Tables[0].Rows[0]["ImageType"]);
                if (FileName != "")
                {

                    //SignImage = "<img width='100px' height='50px' src='.../PatientDocuments/DoctorImages/" + FileName + "' />";
                    strSignImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                    SignImage = "<img width='100px' height='80px' src='" + strSignImagePath + "'/>";
                    //strSignImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                }
                Education = common.myStr(ds.Tables[0].Rows[0]["Education"]);
                Designation = common.myStr(ds.Tables[0].Rows[0]["Designation"]);
                FinalizedByName = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);

                ViewState["strSignImagePath"] = strSignImagePath;
                ViewState["Designation"] = Designation;
                ViewState["Education"] = Education;
                ViewState["FinalizedByName"] = FinalizedByName;
            }
        }
        if (File.Exists(strSignImagePath))
        {
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            sb.Append("<tr><td align='right' valign='bottom' height='80px'></td></tr>");
            sb.Append("<tr><td align='right' valign='bottom'>" + SignImage + "</td></tr>");
            sb.Append("<tr><td align='right'  valign='bottom' style='font-weight:bold;'>" + FinalizedByName.Trim() + ", " + Education + "</td></tr>");
            sb.Append("<tr><td align='right' valign='bottom' style='font-weight:bold;'>" + Designation + "</td></tr></table>");
        }
        else
        {
            sb.Append("<table border='0' cellpadding='0' cellspacing='0'>");
            //sb.Append("<tr><td align='right' valign='bottom' style='font-weight:bold;' height='80px'>&nbsp;<br/><br/><br/></td></tr>");
            sb.Append("<tr><td align='right' valign='bottom' style='font-weight:bold;'>" + FinalizedByName.Trim() + "</td></tr>");
            sb.Append("<tr><td align='right' valign='bottom' style='font-weight:bold;'>" + Education + "</td></tr>");
            sb.Append("<tr><td align='right' valign='bottom' style='font-weight:bold;'>" + Designation + "</td></tr></table>");
        }
        return sb;
    }
    public StringBuilder BindLabTestResult(int HospitalId, int EncounterId, int FacilityId, int LoginFacilityId, StringBuilder sb, StringBuilder sbTemplateStyle,
        DataRow drTemplateListStyle, Page pg, string SampleId, string DischargeSummary, bool bShowAllParameters, bool PrintReferenceRangeInHTML)
    {
        string BeginList = "", EndList = "", sBeginFont = "", sEndFont = "", sBegin = "", sEnd = "", sBeginSection = "", sEndSection = "";
        BaseC.clsLISLabOther lis = new BaseC.clsLISLabOther(sConString);
        BaseC.clsLISLabOther objval = new BaseC.clsLISLabOther(sConString);
        DataSet ds = new DataSet();
        Hashtable HshIn = new Hashtable();
        Hashtable HshOut = new Hashtable();
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        HshIn.Add("@inyHospitalLocationId", common.myInt(HttpContext.Current.Session["HospitalLocationId"]));
        HshIn.Add("@intLoginFacilityId", common.myInt(HttpContext.Current.Session["FacilityId"]));
        HshIn.Add("@intEncounterId", EncounterId);
        HshIn.Add("@intFacilityId", FacilityId);
        HshIn.Add("@xmlDiagSampleIds", SampleId);
        HshIn.Add("@chvDischargeSummary", DischargeSummary);
        //HshIn.Add("@bitShowAllParameters", bShowAllParameters); 
        ds = objDl.FillDataSet(CommandType.StoredProcedure, "uspDiagLabTestResultForNoteIP", HshIn, HshOut);
        StringBuilder sbTemp = new StringBuilder();
        sBegin = ""; sEnd = "";
        MakeFontWithoutListStyle("Sections", ref sBegin, ref sEnd, drTemplateListStyle, pg, common.myStr(HttpContext.Current.Session["HospitalLocationId"]));

        if (ds.Tables.Count > 0)
        {
            int subDeptId = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                sbTemp.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0' style='font-size:medium;'>");

                if ((common.myStr(ds.Tables[0].Rows[0]["LabType"].ToString().Trim()) != "X") && common.myBool(PrintReferenceRangeInHTML))
                {
                    sbTemp.Append("<tr valign ='top'>");
                    sbTemp.Append("<td style='width: 60%;' colspan='2' cellpadding='3' border='1'><b>" + sBegin + "Test" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 10%;' cellpadding='3' border='1'><b>" + sBegin + "Value" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 10%;' cellpadding='3' border='1'><b>" + sBegin + "Unit" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 20%;' cellpadding='3' border='1'><b>" + sBegin + "Reference Range" + sEnd + "</b></td>");
                    sbTemp.Append("<td style='width: 20%;' cellpadding='3' border='1'><b>" + sBegin + "Method Name" + sEnd + "</b></td>");
                    sbTemp.Append("</tr>");
                }
                else
                {
                    sbTemp.Append("<tr valign ='top'");
                    sbTemp.Append("<td colspan='2' style='width: 60%;' >&nbsp;</td>");
                    sbTemp.Append("<td style='width: 10%;'>&nbsp;</td>");
                    sbTemp.Append("<td style='width: 10%;'>&nbsp;</td>");
                    sbTemp.Append("<td style='width: 20%;'>&nbsp;</td>");
                    sbTemp.Append("<td style='width: 20%;'>&nbsp;</td>");
                    sbTemp.Append("</tr>");
                }
                
                if (!common.myBool(PrintReferenceRangeInHTML))
                {
                    sbTemp = new StringBuilder();
                    sbTemp.Append("<table border='0' width='100%' cellpadding='0' cellspacing='0'  style='font-size:medium;'>");
                    sbTemp.Append("<tr valign ='top'><td colspan='6'></td></tr>");
                }

                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "<b>" : "";
                    string eBold = common.myBool(dt.Rows[i]["AbnormalValue"]) ? "</b>" : "";

                    string sDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "<b>" : "";
                    string eDEPBold = common.myStr(dt.Rows[i]["FieldType"]).Trim() == "DEP" ? "</b>" : "";

                    sbTemp.Append(BeginList);
                    sbTemp.Append("<tr valign ='top'>");
                    string FieldType = common.myStr(dt.Rows[i]["FieldType"]).ToUpper().Trim();
                    string Symbol = common.myStr(dt.Rows[i]["Symbol"]).ToUpper().Trim();
                    string LimitType = common.myStr(dt.Rows[i]["LimitType"]).ToUpper().Trim();
                    string FieldName= common.myStr(dt.Rows[i]["FieldName"]).ToUpper().Trim();
                    string Result= common.myStr(dt.Rows[i]["Result"]).ToUpper().Trim();
                    string ReplaceResult = string.Empty;
                    

                    //sbTemp.Append("<td  colspan='4'><b>" + dt.Rows[i]["FieldName"].ToString() + "</b></td>");

                    if (FieldType == "SN" || FieldType == "W" || FieldType == "H")
                    {
                        DataSet dsSen = new DataSet();
                        sbTemp.Append("<td style='font-size:medium;' colspan='6'><b>" + sBegin + dt.Rows[i]["FieldName"].ToString() + sEnd + "</b></td>");
                        if (FieldType == "SN")
                        {
                            dsSen = lis.GetDiagLabSensitivityResultForNote(common.myInt(dt.Rows[i]["DiagSampleId"]), Convert.ToInt16(dt.Rows[i]["ResultId"]));
                            if (dsSen.Tables[0].Rows.Count > 0)
                            {
                                sbTemp.Append("</tr><tr valign ='top'>");
                                sbTemp.Append("<td colspan='6'>");
                                sbTemp.Append("<table border='0' width='100%' visible='false' cellpadding='0' cellspacing='0' style='border-style:none;font-weight: 700;'>");
                                sbTemp.Append("<tr>");
                                int count = 0;
                                for (int l = 0; l < dsSen.Tables[0].Columns.Count; l++)
                                {
                                    sbTemp.Append("<td width='20%' ><b>" + sBegin + dsSen.Tables[0].Columns[l].ColumnName + sEnd + "</b></td>");
                                    count++;
                                }
                                sbTemp.Append("</tr>");

                                for (int k = 0; k < dsSen.Tables[0].Rows.Count; k++)
                                {
                                    sbTemp.Append("<tr valign ='top'>");
                                    for (int j = 0; j < count; j++)
                                    {
                                        if (dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName].ToString().Contains("<B>") == false && dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName].ToString() != null)
                                        {
                                            sbTemp.Append("<td  width='20%'>" + sBegin + dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[j].ColumnName].ToString() + sEnd + "</td>");
                                        }
                                        else
                                        {
                                            sbTemp.Append("<td width='20%' " + count + "' >" + sBegin + dsSen.Tables[0].Rows[k][dsSen.Tables[0].Columns[0].ColumnName].ToString() + sEnd + "</td>");
                                            break;
                                        }
                                    }

                                    sbTemp.Append("</tr>");
                                }
                                sbTemp.Append("</table>");
                                sbTemp.Append("</td>");
                            }
                        }
                        else
                        {
                            if (FieldType != "H")
                            {
                                sbTemp.Append("</tr>");
                                if (FieldType == "W")
                                {
                                    sbTemp.Append("<tr><td colspan='6' style='width: 60%;'>&nbsp;</td></tr>");
                                }
                                sbTemp.Append("<tr valign ='top'>");

                                //sbTemp.Append("<td colspan='5' style='width: 60%;'>" + sBegin + System.Web.HttpContext.Current.Server.HtmlDecode(dt.Rows[i]["Result"].ToString().Replace("<td>", "<td border='1'>").Replace("<td ", "<td border='1' ")) + sEnd + "</td>");
                                sbTemp.Append("<td colspan='6' style='width: 60%;'>" + sBegin + common.myStr(dt.Rows[i]["Result"]).Replace("<td>", "<td border='1'>").Replace("<td ", "<td border='1' ") + sEnd + "</td>");
                            }
                        }
                    }

                    else
                    {
                        if (FieldType == "N" || FieldType == "F" || FieldType == "TM" || FieldType == "T")
                        {
                            if ((Symbol == "<" || Symbol == ">") && LimitType != "SR")
                            {
                                if (Symbol == "<")
                                {
                                    Symbol = "&lt;";
                                }
                                else if (Symbol == ">")
                                {
                                    Symbol = "&gt;";
                                }
                                if(FieldName.Contains("LDL"))//Only for LDL
                                {
                                   if( Result.Contains(">"))
                                     {
                                        ReplaceResult = Result.Replace(">", "&gt; ");
                                     }
                                    else if(Result.Contains("<"))
                                    {
                                        
                                        ReplaceResult=Result.Replace("<", "&lt; ");

                                    }
                                    if(ReplaceResult.Length>0)
                                    {
                                        Result = ReplaceResult;
                                    }
                                }
                                sbTemp.Append("<td valign='top' colspan='2' style='width: 60%;'>" + sBegin + dt.Rows[i]["FieldName"].ToString() + sEnd + "</td><td valign='top' style='width: 10%;'> "+ sBegin + sBold +common.myStr(Result) + eBold + sEnd + "</td><td valign='top' style='width: 10%;'> " + common.myStr(dt.Rows[i]["UnitName"]) + sBegin + "</td><td valign='top' style='width: 20%;'>   " + common.myStr(Symbol) + " " + common.myStr(dt.Rows[i]["MaxValue"]) + sEnd + " </td><td valign='top' style='width: 10%;font-size:8pt;'>   " + common.myStr(dt.Rows[i]["MethodName"]) + sEnd + " </td>");
                            }
                            else if (LimitType == "SR")
                            {
                                sbTemp.Append("<td valign='top' colspan='2' style='width: 60%;'>" + sBegin + dt.Rows[i]["FieldName"].ToString() + sEnd + "</td><td valign='top' style='width: 10%;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td valign='top' style='width: 10%;'> " + common.myStr(dt.Rows[i]["UnitName"]) + sBegin + "</td><td valign='top' style='width: 20%;'>   (" + common.myStr(Symbol) + " " + common.myStr(dt.Rows[i]["SpecialReferenceRange"]) + sEnd + ")</td><td valign='top' style='width: 10%;font-size:8pt;'>   " + common.myStr(dt.Rows[i]["MethodName"]) + sEnd + " </td>");
                            }
                            else
                            {
                                string sValue = common.myStr(dt.Rows[i]["MinValue"]) == "0" && common.myStr(dt.Rows[i]["MaxValue"])=="0" ? "" : "(" + common.myStr(dt.Rows[i]["MinValue"]) + " - " + common.myStr(dt.Rows[i]["MaxValue"]) + sEnd + ")";
                                sbTemp.Append("<td valign='top' colspan='2' style='width: 60%;'>" + sBegin + dt.Rows[i]["FieldName"].ToString() + sEnd + "</td><td valign='top' style='width: 10%;'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td valign='top' style='width: 10%;'> " + common.myStr(dt.Rows[i]["UnitName"]) + sBegin + "</td><td valign='top' style='width: 20%;'>   "+sValue+ " </td><td valign='top' style='width: 10%;font-size:8pt;'>   " + common.myStr(dt.Rows[i]["MethodName"]) + sEnd + " </td>");
                            }
                        }
                        else if (FieldType == "T" || FieldType == "M" || FieldType == "O" || FieldType == "E")
                        {
                            sbTemp.Append("<td align='left' colspan='2' valign='top' >" + sBegin + dt.Rows[i]["FieldName"].ToString() + sEnd + "</td><td colspan='3'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td align='left' valign='top' style='font-size:8pt;'>   " + common.myStr(dt.Rows[i]["MethodName"]) + sEnd + " </td>");
                        }
                        else if (FieldType == "D")
                        {
                            sbTemp.Append("<td  colspan='2' align='left'>" + sBegin + dt.Rows[i]["FieldName"].ToString() + sEnd + "</td><td  colspan='2'> " + sBegin + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold + sEnd + "</td><td valign='top' style='width: 20%;'>" + common.myStr(dt.Rows[i]["FindingRemarks"]).Replace("<td>", "<td border='1'>").Replace("<td ", "<td border='1' ") + sEnd + " </td><td align='left' valign='top' style='font-size:8pt;'>   " + common.myStr(dt.Rows[i]["MethodName"]) + sEnd + " </td>");
                        }

                        else
                        {
                            if (FieldType == "DT")
                            {
                                if (HttpContext.Current.Session["DateDisplay"] == null)
                                {
                                    sbTemp.Append("<td style='font-weight: 700;' colspan='6' align='left'>" + sBegin + dt.Rows[i]["FieldName"].ToString() + sBold + common.myStr(dt.Rows[i]["Result"]) + eBold +  common.myStr(dt.Rows[i]["MethodName"]) + eBold + sEnd + "</td>");
                                }
                                HttpContext.Current.Session["DateDisplay"] = null;
                            }
                            else //Heading
                            {
                                sbTemp.Append("<td  colspan='6' align='left'>" + sBegin + sDEPBold + dt.Rows[i]["FieldName"].ToString() + eDEPBold + sBold + common.myStr(dt.Rows[i]["Result"]).Replace("<td>", "<td border='1'>").Replace("<td ", "<td border='1' ") + eBold + common.myStr(dt.Rows[i]["MethodName"]) + eBold + sEnd + "</td>");
                            }
                        }
                    }
                    sbTemp.Append("</tr>");
                    sbTemp.Append(EndList);
                }

            }
        }
        sbTemp.Append("</table>");
        sb.Append(sbTemp.ToString());
        return sb;
    }
    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item, Page Pg, string HospitalId)
    {

        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (item != null)
        {
            if (item[typ + "Forecolor"].ToString() != "" || item[typ + "FontSize"].ToString() != "" || item[typ + "FontStyle"].ToString() != "" || item[typ + "Bold"].ToString() != "" || item[typ + "Italic"].ToString() != "" || item[typ + "Underline"].ToString() != "")
            {
                sBegin += "<span style='";
                if (item[typ + "FontSize"].ToString() != "")
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += getDefaultFontSize(Pg, HospitalId); }
                if (item[typ + "Forecolor"].ToString() != "")
                { sBegin += " color: #" + item[typ + "Forecolor"] + ";"; }
                if (item[typ + "FontStyle"].ToString() != "")
                { sBegin += " font-family: Candara ;"; }


                if (item[typ + "Bold"].ToString() == "True")
                { sBegin += " font-weight: bold;"; }
                if (item[typ + "Italic"].ToString() == "True")
                { sBegin += " font-style: italic;"; }
                if (item[typ + "Underline"].ToString() == "True")
                { sBegin += " text-decoration: underline;"; }
                aEnd.Add("</span>");
                for (int i = aEnd.Count - 1; i >= 0; i--)
                {
                    sEnd += aEnd[i];
                }
                sBegin += " '>";
            }
            else
            {
                sBegin += "<span style='";
                if (item[typ + "FontSize"].ToString() != "")
                { sBegin += " font-size:" + item[typ + "FontSize"] + ";"; }
                else { sBegin += getDefaultFontSize(Pg, HospitalId); }

                sBegin += GetFontFamily(typ, item, Pg, HospitalId);

                aEnd.Add("</span>");
                for (int i = aEnd.Count - 1; i >= 0; i--)
                {
                    sEnd += aEnd[i];
                }
                sBegin += " '>";
            }
        }

    }
    public string getDefaultFontSize(Page pg, string HospitalLocationId)
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", HospitalLocationId);
        if (FieldValue != "")
        {
            sFontSize = " font-size: " + FieldValue + ";";
        }
        return sFontSize;
    }
    protected string GetFontFamily(string typ, DataRow item, Page Pg, string HospitalId)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Pg);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        if (item[typ + "FontStyle"].ToString() != "")
        {
            FontName = fonts.GetFont("Name", item[typ + "FontStyle"].ToString());
            if (FontName != "")
                sBegin += " font-family: " + FontName + ";";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", HospitalId);
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                    sBegin += " font-family: " + FontName + ";";
            }
        }

        return sBegin;
    }
}