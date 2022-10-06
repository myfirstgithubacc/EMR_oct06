using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;
using System.IO;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using System.Net;

public partial class Editor_PrintPdf : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("/Login.aspx?Logout=1", false);
        //Session["LtrID"]

        if (!IsPostBack)
        {
            //if (Convert.ToString(Session["RTF"]) != "")
            //{
            //    string strRtfWord = Convert.ToString(Session["RTF"]);
            //    if (common.myStr(Request.QueryString["page"]) == "Ward")
            //    {
            //        BindPdfWard(strRtfWord);
            //    }
            //    else
            //    {
            //        BindPdf(strRtfWord);
            //    }
            //}
            if (Request.QueryString["CaseSheetPrintReportHeader"] != null)
            {
                if (common.myStr(Request.QueryString["CaseSheetPrintReportHeader"]).Equals("True"))
                {
                    if (Session["RTFCaseSheetPrintReportHeader"] != null && common.myStr(Session["RTFCaseSheetPrintReportHeader"]) != "")
                    {
                        string strRtfWord = common.myStr(Session["RTFCaseSheetPrintReportHeader"]);
                        if (common.myStr(Request.QueryString["page"]) == "Ward")
                        {
                            BindPdfWard(strRtfWord);
                        }
                        else
                        {
                            BindPdf(strRtfWord);
                        }
                    }
                }
            }

            else if (Session["RTF"] != null && common.myStr(Session["RTF"]) != "")
            {
                string strRtfWord = Convert.ToString(Session["RTF"]);
                if (common.myStr(Request.QueryString["page"]) == "Ward")
                {
                    BindPdfWard(strRtfWord);
                }
                else
                {
                    BindPdf(strRtfWord);
                }
            }


            else
            {
                Response.Write("Sorry ! Data not available.");
                return;
            }
        }
    }

    private void BindPdfWard(string strRtfWord)
    {
        try
        {
            if (Session["RegistrationId"] != null && Session["HospitalLocationID"] != null)
            {
                DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                Hashtable hstInput = new Hashtable();
                double conversionUnit = 10;
                hstInput = new Hashtable();
                // hstInput.Add("FormId", Session["FormId"]);
                hstInput.Add("@inyHospitalLocationID", Session["HospitalLocationID"]);
                hstInput.Add("@intFacilityId", Session["FacilityId"]);
                hstInput.Add("@intEncounterId", Session["EncounterId"]);
                StringBuilder sQ = new StringBuilder();


                DataSet ds = objDl.FillDataSet(CommandType.StoredProcedure, "USPGetDischargePatientHeader", hstInput);
                clsParsePDF cPrint = new clsParsePDF();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ////    BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
                    ////    string DoctorId = fun.GetDoctorId(Convert.ToInt32(Session["HospitalLocationID"]), Convert.ToInt16(Session["UserId"]));
                    StringBuilder objStr = new StringBuilder();
                    string sStartTable = "";

                    sStartTable = " <table border='0' cellpadding='0' cellspacing='0' align='left' style='border-style:none;'>";
                    objStr.Append(sStartTable);

                    //objStr.Append("<tr align='left'><td colspan='6'><hr/><br/> </td></tr>");

                    objStr.Append("<tr align='left'><td style='border-style:none;width:50px;'>" + "Reg. No" + "</td><td align='left' style='border-style:none;'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["RegistrationNo"].ToString()) + "</td><td style='border-style:none;width:50px;'>" + "IP. No" + "</td><td align='left' style='border-style: none;'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["EncounterNo"]) + "</td></tr>");
                    objStr.Append("<tr align='left' ><td style='border-style:none;width:50px;'>" + "Patient" + "</td><td align='left' style='border-style:none;'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["PatientName"].ToString()) + "</td><td style='border-style:none;width:50px;'>" + "Age / Sex" + "</td><td align='left' style='border-style: none;'> " + "  :  " + common.myStr(ds.Tables[0].Rows[0]["AgeGender"]) + "</td></tr>");
                    objStr.Append("<tr align='left' ><td style='border-style:none;width:50px;'>" + "Address" + "</td><td align='left' colspan='3' style='border-style:none;'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["PAddress1"].ToString()) + "</td></tr>");
                    objStr.Append("<tr align='left'><td style='border-style:none;width:50px;'>" + "Bed Category" + "</td><td align='left' style='border-style:none;'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["BedCategoryName"].ToString()) + "</td><td style='border-style:none;width:50px;'>" + "Bed No." + "</td><td align='left' style='border-style: none;'> " + "  :  " + common.myStr(ds.Tables[0].Rows[0]["BedNo"]) + "</td></tr>");
                    objStr.Append("<tr align='left' ><td style='border-style:none;width:50px;'>" + "Consultant" + "</td><td align='left' colspan='3' style='border-style:none;'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["Doctorname"].ToString()) + "</td></tr>");
                    objStr.Append("<tr align='left'><td style='border-style:none;width:50px;'>" + "DOA" + "</td><td align='left' colspan='4'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["AdmissionDate"].ToString()) + "</td></tr>");
                    objStr.Append("<tr align='left'><td style='border-style:none;width:50px;'>" + "DOD" + "</td><td align='left' colspan='4'>" + "  :  " + common.myStr(ds.Tables[0].Rows[0]["DischargeDate"].ToString()) + "</td></tr>");

                    //objStr.Append("<tr ><td colspan='6'><hr/><br/> </td></tr>");


                    objStr.Append("</table>");


                    ////if (objDs.Tables[0].Rows[0]["Pg1HeaderNote"].ToString() != "")
                    ////    if (objDs.Tables[1].Rows.Count > 0)
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                    ////    }
                    ////    else
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), null, null, objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                    ////    }
                    ////if (objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString() != "")
                    ////{
                    ////    if (objDs.Tables[1].Rows.Count > 0)
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                    ////    }
                    ////    else
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), null, null, objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                    ////    }
                    ////}

                    ////    if (Convert.ToString(objDs.Tables[0].Rows[0]["PgSize"]) == "0" || Convert.ToString(objDs.Tables[0].Rows[0]["PgSize"]) == "")
                    cPrint.Size = iTextSharp.text.PageSize.GetRectangle("A4");
                    ////    else
                    ////        cPrint.Size = iTextSharp.text.PageSize.GetRectangle(objDs.Tables[0].Rows[0]["PgSize"].ToString());
                    ////    if (Convert.ToString(objDs.Tables[0].Rows[0]["PgTopMargin"]) != "")
                    cPrint.MarginTop = Convert.ToInt32(Convert.ToDouble(1 * conversionUnit));
                    ////    if (Convert.ToString(objDs.Tables[0].Rows[0]["PgRightMargin"]) != "")
                    cPrint.MarginRight = Convert.ToInt32(Convert.ToDouble(1 * conversionUnit));
                    ////    if (Convert.ToString(objDs.Tables[0].Rows[0]["PgLeftMargin"]) != "")
                    cPrint.MarginLeft = Convert.ToInt32(Convert.ToDouble(1 * conversionUnit));
                    ////    if (Convert.ToString(objDs.Tables[0].Rows[0]["PgBottomMargin"]) != "")
                    cPrint.MarginBottom = Convert.ToInt32(Convert.ToDouble(1 * conversionUnit));

                    //remove signature image and sing note
                    //string RtfContent = strRtfWord.ToString();
                    //if (RtfContent != null)
                    //{
                    //    if (RtfContent.Contains("dvDoctorImage") == true)
                    //    {
                    //        RtfContent = RtfContent.Replace('"', '$');
                    //        string st = "<div id=$dvDoctorImage$>";
                    //        int start = RtfContent.IndexOf(@st);
                    //        if (start > 0)
                    //        {
                    //            int End = RtfContent.IndexOf("</div>", start);
                    //            StringBuilder sbte = new StringBuilder();
                    //            sbte.Append(RtfContent.Substring(start, (End + 6) - start));
                    //            StringBuilder ne = new StringBuilder();
                    //            ne.Append(RtfContent.Replace(sbte.ToString(), ""));
                    //            RtfContent = ne.Replace('$', '"').ToString();
                    //        }
                    //    }
                    //}
                    //


                    cPrint.GetFontName();
                    string strHTML = strRtfWord.ToString().Replace("..", "&#46;&#46;");

                    strHTML = common.removeUnusedHTML(strHTML);

                    cPrint.Html = strHTML; // RTF1.Content;


                    cPrint.FirstPageHeaderHtml = objStr.ToString();

                    ////if (objDs.Tables[0].Rows[0]["DisplayPgNoInPage1"].ToString() != "1") // For Page 1 Page no Display
                    ////{

                    ////    if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "L")
                    ////    {
                    ////        cPrint.FooterLeft = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////        cPrint.FirstPageFooterLeft = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////    }
                    ////    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "C")
                    ////    {
                    ////        cPrint.FooterMiddle = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////        cPrint.FirstPageFooterMiddle = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////    }
                    ////    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "R")
                    ////    {
                    ////        cPrint.FooterRight = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////        cPrint.FirstPageFooterRight = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////    }
                    ////}
                    ////else
                    ////{
                    ////    if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "L")
                    ////    {
                    ////        cPrint.FooterLeft = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////    }
                    ////    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "C")
                    ////    {
                    ////        cPrint.FooterMiddle = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////    }
                    ////    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "R")
                    ////    {
                    ////        cPrint.FooterRight = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    ////    }
                    ////}

                    ////objStr = new StringBuilder();
                    ////if (objDs.Tables[0].Rows[0]["Pg2HeaderNote"].ToString() != "")
                    ////{
                    ////    if (objDs.Tables[1].Rows.Count > 0)
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    ////    }
                    ////    else
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), null, null, objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    ////    }
                    ////}

                    ////if (objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString() != "")
                    ////{
                    ////    if (objDs.Tables[1].Rows.Count > 0)
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    ////    }
                    ////    else
                    ////    {
                    ////        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), null, null, objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    ////    }
                    ////}
                    cPrint.HeaderHtml = objStr.ToString();

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
        }
        catch (Exception Ex)
        {

            //lblmassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblmassage.Text = "Error: " + Ex.Message;
            Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
            objException.HandleException(Ex);
        }
    }

    //

    private void BindPdf(string strRtfWord)
    {
        //try
        //{

        if (Session["RegistrationId"] != null && Session["HospitalLocationID"] != null)
        {
            DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
            Hashtable hstInput = new Hashtable();
            double conversionUnit = 72;
            hstInput = new Hashtable();
            //hstInput.Add("FormId", "4");
            hstInput.Add("HospitalLocationId", Session["HospitalLocationID"]);
            hstInput.Add("intRegistrationId", Session["RegistrationId"]);
            StringBuilder sQ = new StringBuilder();

            sQ.Append("SELECT PgSize, PgTopMargin, PgBottomMargin, PgLeftMargin, PgRightMargin, PgNoFormat, PgNoAllignment,Pg1HeaderId, Pg1HeaderMargin, Pg1HeaderNote, Pg2HeaderId, Pg2HeaderNote, PgFontType, PgFontSize, Pg1HeaderFontBold, Pg1HeaderFontItalic, Pg1HeaderFontUnderline, Pg1HeaderFontColor, Pg1HeaderFontType, Pg1HeaderFontSize, ");
            sQ.Append(" Pg2HeaderFontBold, Pg2HeaderFontItalic, Pg2HeaderFontUnderline, Pg2HeaderFontColor, Pg2HeaderFontType, Pg2HeaderFontSize, DisplayPgNoInPage1 FROM EMRForms WITH(NOLOCK) ");
            sQ.Append(" WHERE HospitalLocationId  = @HospitalLocationId ");
            sQ.Append(" Select Convert(varchar(10),da.AppointmentDate,103) AppointmentDate,e.FacilityId from Encounter e WITH(NOLOCK) ");
            sQ.Append(" Inner Join doctorAppointment da WITH(NOLOCK) On da.AppointmentId=e.AppointmentId ");
            sQ.Append(" Where e.RegistrationId = @intRegistrationId And e.HospitalLocationId = @HospitalLocationId ");

            DataSet objDs = objDl.FillDataSet(CommandType.Text, sQ.ToString(), hstInput);
            clsParsePDF cPrint = new clsParsePDF();
            if (objDs.Tables[0].Rows.Count > 0)
            {
                BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
                string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserId"])));
                StringBuilder objStr = new StringBuilder();
                if (Session["HeaderLogo"] != null)
                    objStr = objStr.Append(Session["HeaderLogo"].ToString());
                if (common.myStr(Session["FacilityName"]).ToUpper().Contains("ENDOWORLD"))
                    objStr = objStr.Replace("Prescription", "Case Sheet");
                    if (objDs.Tables[0].Rows[0]["Pg1HeaderNote"].ToString() != "")
                        if (objDs.Tables[1].Rows.Count > 0)
                        {
                            HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                        }
                        else
                        {
                            HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), null, common.myInt(Session["FacilityId"]).ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                        }
                if (objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString() != "")
                {
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                    }
                    else
                    {
                        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg1HeaderId"].ToString(), null, common.myInt(Session["FacilityId"]).ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg1");
                    }
                }

                if (Convert.ToString(objDs.Tables[0].Rows[0]["PgSize"]) == "0" || Convert.ToString(objDs.Tables[0].Rows[0]["PgSize"]) == "")
                    cPrint.Size = iTextSharp.text.PageSize.GetRectangle("A4");
                else
                    cPrint.Size = iTextSharp.text.PageSize.GetRectangle(objDs.Tables[0].Rows[0]["PgSize"].ToString());
                if (Convert.ToString(objDs.Tables[0].Rows[0]["PgTopMargin"]) != "")
                    cPrint.MarginTop = Convert.ToInt32(Convert.ToDouble(objDs.Tables[0].Rows[0]["PgTopMargin"]) * conversionUnit);
                if (Convert.ToString(objDs.Tables[0].Rows[0]["PgRightMargin"]) != "")
                    cPrint.MarginRight = Convert.ToInt32(Convert.ToDouble(objDs.Tables[0].Rows[0]["PgRightMargin"]) * conversionUnit);
                if (Convert.ToString(objDs.Tables[0].Rows[0]["PgLeftMargin"]) != "")
                    cPrint.MarginLeft = Convert.ToInt32(Convert.ToDouble(objDs.Tables[0].Rows[0]["PgLeftMargin"]) * conversionUnit);
                if (Convert.ToString(objDs.Tables[0].Rows[0]["PgBottomMargin"]) != "")
                    cPrint.MarginBottom = Convert.ToInt32(Convert.ToDouble(objDs.Tables[0].Rows[0]["PgBottomMargin"]) * conversionUnit);

                ////remove signature image and sing note
                string RtfContent = strRtfWord.ToString();
                RtfContent = RtfContent.Replace("/PatientDocuments", Server.MapPath("/PatientDocuments"));
                if (RtfContent != null)
                {
                    if (RtfContent.Contains("dvDoctorImage"))
                    {
                        // RtfContent = RtfContent.Replace("/PatientDocuments", ".../PatientDocuments");
                        RtfContent = RtfContent.Replace("/PatientDocuments", Server.MapPath("/PatientDocuments"));
                    }
                    else if (RtfContent.Contains("dvImageType"))
                    {
                        //RtfContent = RtfContent.Replace("/Images/TemplateImage/", ".../Images/TemplateImage/");
                        RtfContent = RtfContent.Replace("/Images/TemplateImage/", Server.MapPath("/Images/TemplateImage/"));
                    }
                }


                cPrint.GetFontName();

                string strHTML = RtfContent.Replace("..", "&#46;&#46;");

                strHTML = common.removeUnusedHTML(strHTML);

                cPrint.Html = strHTML;// RTF1.Content;

                cPrint.FirstPageHeaderHtml = objStr.ToString();
                #region Header Part
                if (objDs.Tables[0].Rows[0]["DisplayPgNoInPage1"].ToString() != "1") // For Page 1 Page no Display
                {

                    if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "L")
                    {
                        cPrint.FooterLeft = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                        cPrint.FirstPageFooterLeft = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    }
                    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "C")
                    {
                        cPrint.FooterMiddle = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                        cPrint.FirstPageFooterMiddle = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    }
                    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "R")
                    {
                        cPrint.FooterRight = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                        cPrint.FirstPageFooterRight = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    }
                }
                else
                {
                    if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "L")
                    {
                        cPrint.FooterLeft = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    }
                    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "C")
                    {
                        cPrint.FooterMiddle = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    }
                    else if (objDs.Tables[0].Rows[0]["PgNoAllignment"].ToString() == "R")
                    {
                        cPrint.FooterRight = objDs.Tables[0].Rows[0]["PgNoFormat"].ToString();
                    }
                }

                objStr = new StringBuilder();
                if (objDs.Tables[0].Rows[0]["Pg2HeaderNote"].ToString() != "")
                {
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    }
                    else
                    {
                        HeaderAndFooter(objDs.Tables[0], "HeaderNote", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), null, common.myInt(Session["FacilityId"]).ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    }
                }

                if (objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString() != "")
                {
                    if (objDs.Tables[1].Rows.Count > 0)
                    {
                        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), objDs.Tables[1].Rows[0]["AppointmentDate"].ToString(), objDs.Tables[1].Rows[0]["FacilityId"].ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    }
                    else
                    {
                        HeaderAndFooter(objDs.Tables[0], "Header", objDs.Tables[0].Rows[0]["Pg2HeaderId"].ToString(), null, common.myInt(Session["FacilityId"]).ToString(), objStr, DoctorId, Session["HospitalLocationId"].ToString(), Convert.ToInt32(Session["EncounterId"]), "pg2");
                    }
                }
                #endregion
                cPrint.HeaderHtml = objStr.ToString();

                MemoryStream m = cPrint.ParsePDF();


                #region Send EMAIL
                if (common.myStr(Request.QueryString["SendEmail"]).Equals("1"))
                {
                    BaseC.EMRMasters.EMRFacility objf = new BaseC.EMRMasters.EMRFacility(sConString);
                    BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);
                    DataSet ds1 = new DataSet();
                    try
                    {
                        m.Flush();
                        byte[] bytes = m.ToArray();

                        MemoryStream ms = new MemoryStream(bytes); ;
                        ds1 = objf.GetFacility(Convert.ToInt16(Session["HospitalLocationId"]));
                        if (ds1.Tables.Count > 0)
                        {
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                DataView dv = new DataView();
                                dv = ds1.Tables[0].DefaultView;
                                dv.RowFilter = "FacilityId = " + common.myStr(Session["FacilityID"]);
                                if (dv.Count > 0)
                                {
                                    DataTable dt1 = new DataTable();
                                    dt1 = dv.ToTable();

                                    string SMTPMailServer = common.myStr(dt1.Rows[0]["SMTPMailServer"]);
                                    string SMTPMailServerPort = common.myStr(dt1.Rows[0]["SMTPMailServerPort"]);
                                    string DefaultFromMailId = common.myStr(dt1.Rows[0]["PoliceIntimationDefaultFromMailId"]);
                                    string DefaultFromMailPws = common.myStr(dt1.Rows[0]["PoliceIntimationDefaultFromMailPws"]);

                                    string FacilityName = common.myStr(dt1.Rows[0]["Name"]);

                                    //string SMTPMailServer = "smtp.zoho.com";
                                    //string SMTPMailServerPort = "587";
                                    //string DefaultFromMailId = "noreply@chaitanyahospital.org";
                                    //string DefaultFromMailPws = "Neonate@123";



                                    MailMessage mail = new MailMessage();
                                    //Subject: Aster MIMS Calicut - MLC - < Patient Name >
                                    string MailSubject = FacilityName + " - MLC - " + common.myStr(Session["PaitentName"]);
                                    string MailBody = "Respected Sir,"
                                        + "</br></br></br>"
                                        + "Please find attached file related to " + common.myStr(Session["PaitentName"]) + " Aster ID " + common.myStr(Session["RegistrationNo"])
                                        + "</br></br></br>"
                                        + "Regards,"
                                        + "</br></br></br>"
                                        + "MLC Officer,"
                                        + "</br>"
                                        + "Emergency Department,"
                                        + "</br>"
                                        + FacilityName
                                        + "</br></br></br>";

                                    //string filePath = Server.MapPath("/PatientDocuments/DoctorImages/anil2.jpg");

                                    //System.IO.FileInfo file = new System.IO.FileInfo(filePath);
                                    //if (file.Exists)
                                    //{
                                    //    mail.Attachments.Add(new System.Net.Mail.Attachment(filePath));
                                    //}

                                    mail.Attachments.Add(new System.Net.Mail.Attachment(ms, "PoliceIntimation_" + common.myStr(DateTime.Now) + ".pdf"));

                                    mail.From = new MailAddress(DefaultFromMailId, DefaultFromMailId);
                                    // mail.To.Add("kabir.sidana@akhilsystems.com");
                                    mail.To.Add(common.myStr(dt1.Rows[0]["PoliceIntimationEmailId"]));

                                    mail.Subject = MailSubject;
                                    //mail.Body = "Dear " + common.myStr(Request.QueryString["PName"]).Trim() + "," + System.Environment.NewLine + System.Environment.NewLine + "Please! Find your attached Lab Report.";
                                    mail.Body = MailBody;
                                    mail.IsBodyHtml = true;
                                    mail.BodyEncoding = System.Text.Encoding.UTF8;



                                    SmtpClient client = new SmtpClient(SMTPMailServer, common.myInt(SMTPMailServerPort));

                                    // client.UseDefaultCredentials = false;
                                    // client.UseDefaultCredentials = true;
                                    client.EnableSsl = true;

                                    client.Credentials = new NetworkCredential(DefaultFromMailId, DefaultFromMailPws);
                                    client.Host = SMTPMailServer;
                                    client.Port = common.myInt(SMTPMailServerPort);


                                    try
                                    {

                                        client.Send(mail);
                                        Response.Write("Email Sent Successfully");

                                        ///   objEMR.UpdatePoliceIntimationSent(common.myInt(Session["EncounterId"]));

                                        return;


                                    }
                                    catch (SmtpFailedRecipientException ex)
                                    {
                                        Alert.ShowAjaxMsg("Error: " + ex.Message, Page);
                                        objException.HandleException(ex);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        Alert.ShowAjaxMsg("Error: " + ex.Message, Page);
                        objException.HandleException(ex);
                    }
                    finally
                    {
                        objf = null;
                        objEMR = null;
                        ds1.Dispose();
                    }

                }

                #endregion

                else
                {

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
        }
        // }
        //catch (Exception Ex)
        //{

        //    //lblmassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
        //    //lblmassage.Text = "Error: " + Ex.Message;
        //    Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
        //    objException.HandleException(Ex);
        //}
    }
    //
    protected void HeaderAndFooter(DataTable dt, string Type, string HeaderId, string AppointmentDate, string FacilityId, StringBuilder sb, string DoctorId, string HospitalId, int EncounterId, string pg)
    {

        DataRow dr = dt.Rows[0];
        string Fonts = getFontName(dr[pg + "HeaderFontType"].ToString(), dr[pg + "HeaderFontSize"].ToString());
        string color = "#" + dr[pg + "HeaderFontColor"].ToString();
        string bold = "", italic = "";
    

        if (dr[pg + "HeaderFontBold"].ToString() == "True")
            bold = "font-weight:bold;";
        if (dr[pg + "HeaderFontItalic"].ToString() == "True")
            italic = "font-style:italic;";
        string Ft = "style='" + Fonts + ";" + bold + italic;

        if (Type == "HeaderNote")
        {

            if (dt.Rows[0][pg + "HeaderNote"].ToString() != "")
            {
                sb.Append(dt.Rows[0][pg + "HeaderNote"].ToString());
            }
        }
        else
        {
            if (dt.Rows[0][pg + "HeaderId"].ToString() != "")
            {
                DataSet ds = new DataSet();
                DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                int RegistrationId = Convert.ToInt32(Session["RegistrationId"]);
                ds = new DataSet();
                Hashtable hsSub = new Hashtable();
                hsSub = new Hashtable();
                hsSub.Add("@intHeaderId", HeaderId);
                hsSub.Add("@chrAppointmentDate", AppointmentDate);
                hsSub.Add("@intEmployeeId", DoctorId);
                hsSub.Add("@intRegistrationId", RegistrationId);
                hsSub.Add("@intFacilityId", FacilityId);
                hsSub.Add("@intEncounterId", EncounterId);
                ds = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hsSub);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ShowBorder"].ToString().Trim() == "True")
                    {
                        // sb.Append("<hr width=\"100%\" />");
                        sb.Append("<table  border='1' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>");
                        sb.Append("<table style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");//</font></td></tr>");

                        int cc = 0;
                        for (int r = 1; r <= 8; r++)
                        {
                            DataView dvR = new DataView(ds.Tables[0]);

                            dvR.RowFilter = "RowNo=" + r.ToString();
                            DataTable dtR = dvR.ToTable();

                            if (dtR.Rows.Count != 0)
                            {
                                sb.Append("<tr align='left'>");

                                for (int c = 1; c <= dtR.Rows.Count; c++)
                                {

                                    if (dr[pg + "HeaderFontUnderline"].ToString() == "True")
                                    {
                                        if (!Convert.ToBoolean(ds.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                            {
                                                sb.Append("<td valign='top' " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></td>");
                                            }
                                            else
                                            {
                                                sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></td>");
                                            }
                                        }
                                        else
                                        {
                                            if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                            {
                                                BindImageWithControl(ds.Tables[0].Rows[cc]["ImageData"]);
                                                sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</font></u></td>");
                                            }
                                            else
                                            {
                                                BindImageWithControl(ds.Tables[0].Rows[cc]["ImageData"]);
                                                sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</font></u></td>");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!Convert.ToBoolean(ds.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            if (common.myStr(Session["FacilityName"]).ToUpper().Contains("ENDOWORLD"))
                                            {
                                                Ft = "style='font-family:Calibri;border-style:none;font-size:10pt;";

                                                
                                                if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                                {
                                                    if (c % 2 == 0)
                                                    {
                                                        sb.Append("<td valign='top' " + Ft + "text-align:center;" + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : </font></td>");
                                                        sb.Append("<td valign='top' " + Ft + "font-weight:bold; text-align:left;" + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></td>");


                                                    }
                                                    else
                                                    {
                                                        sb.Append("<td valign='top' " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : </font></td>");
                                                        sb.Append("<td valign='top' colspan='2' " + Ft + "font-weight:bold; " + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></td>");

                                                    }

                                                }
                                                else
                                                {
                                                    sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></td>");
                                                }
                                            }
                                            else
                                            {

                                                if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                                {
                                                    sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></td>");
                                                }
                                                else
                                                {
                                                    sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></td>");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                            {
                                                BindImageWithControl(ds.Tables[0].Rows[cc]["ImageData"]);
                                                sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</font></u></td>");
                                            }
                                            else
                                            {
                                                BindImageWithControl(ds.Tables[0].Rows[cc]["ImageData"]);
                                                sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</font></u></td>");
                                            }
                                        }
                                    }

                                    cc++;
                                }
                                sb.Append("</tr>");
                                sb.Append("</table>");

                                sb.Append("<table>");//</font></td></tr>");

                            }
                        }
                        if (ds.Tables[0].Rows[0]["ShowRefPhysician"].ToString().Trim() == "True")
                        {
                            string strShowRefPhysician = "Select  cont.ContactID,cont.CompanyName  from OrgContacts cont inner join OrgContactCategories cat on cont.CategoryID=cat.CategoryID inner join Encounter  en on cont.ContactID=en.ReferringPhysicianId  where en.ReferringPhysicianId=4 and en.Id=" + Session["EncounterID"] + " order by CompanyName ";
                            DataSet dsRef = new DataSet();
                            dsRef = DlObj.FillDataSet(CommandType.Text, strShowRefPhysician);
                            if (dsRef.Tables[0].Rows.Count > 0)
                            {
                                sb.Append("<tr align='left'>");
                                sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + "Referring Physician " + " : " + "<style='" + Fonts + "'>" + dsRef.Tables[0].Rows[0]["CompanyName"].ToString() + " </font></td>");
                                sb.Append("</tr>");
                            }
                        }

                        sb.Append("</table>");
                        // sb.Append("<hr width=\"100%\" /><br />");
                        sb.Append("<table  border='1' cellpadding='0' cellspacing='0'><tr><td></td></tr></table>");

                    }
                    else
                    {
                        sb.Append("<table style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");//</font></td></tr>");

                        int cc = 0;
                        for (int r = 1; r <= 8; r++)
                        {
                            DataView dvR = new DataView(ds.Tables[0]);

                            dvR.RowFilter = "RowNo=" + r.ToString();
                            DataTable dtR = dvR.ToTable();

                            if (dtR.Rows.Count != 0)
                            {
                                sb.Append("<tr align='left'>");

                                for (int c = 1; c <= dtR.Rows.Count; c++)
                                {
                                    if (dr[pg + "HeaderFontUnderline"].ToString() == "True")
                                    {
                                        if (!Convert.ToBoolean(ds.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></td>");
                                        }
                                        else
                                        {
                                            BindImageWithControl(ds.Tables[0].Rows[cc]["ImageData"]);
                                            sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</font></u></td>");

                                        }


                                    }
                                    else
                                    {
                                        if (!Convert.ToBoolean(ds.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></td>");
                                        }
                                        else
                                        {
                                            BindImageWithControl(ds.Tables[0].Rows[cc]["ImageData"]);
                                            sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + "<td valign='top' ><img src='/PatientDocuments/PatientImages/BioChem.bmp' border='0'></img></td>" + "</font></u></td>");

                                        }



                                    }



                                    cc++;
                                }
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                sb.Append("<table>");
                            }
                        }
                        if (ds.Tables[0].Rows[0]["ShowRefPhysician"].ToString().Trim() == "True")
                        {
                            string strShowRefPhysician = "Select  cont.ContactID,cont.CompanyName  from OrgContacts cont inner join OrgContactCategories cat on cont.CategoryID=cat.CategoryID inner join Encounter  en on cont.ContactID=en.ReferringPhysicianId  where en.ReferringPhysicianId=4 and en.Id=" + Session["EncounterID"] + " order by CompanyName ";
                            DataSet dsRef = new DataSet();
                            dsRef = DlObj.FillDataSet(CommandType.Text, strShowRefPhysician);
                            if (dsRef.Tables[0].Rows.Count > 0)
                            {
                                sb.Append("<tr align='left'>");
                                sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + "Referring Physician " + " : " + "<style='" + Fonts + "'>" + dsRef.Tables[0].Rows[0]["CompanyName"].ToString() + " </font></td>");
                                sb.Append("</tr>");
                            }
                        }
                        sb.Append("</table><br/><br/>");

                    }
                }
            }

        }
    }

    protected string getFontName(string id, string FtSize)
    {
        string sBegin = "";
        string sFontSize = "";
        if (id == "1")
            sBegin += " font-family:Arial ";
        if (id == "2")
            sBegin += " font-family:Courier New ";
        if (id == "3")
            sBegin += " font-family:Garamond ";
        if (id == "4")
            sBegin += " font-family:Georgia ";
        if (id == "5")
            sBegin += " font-family:MS Sans Serif ";
        if (id == "6")
            sBegin += " font-family:Segoe UI";
        if (id == "7")
            sBegin += " font-family:Tahoma ";
        if (id == "8")
            sBegin += " font-family:Times New Roman ";
        if (id == "9")
            sBegin += " font-family:Verdana ";
        if (id == "10")
            sBegin += " font-family:Candara  ";
        if (id == "11")
            sBegin += " font-family:Trebuchet MS  ";



        if (FtSize == "6")
            sFontSize += " ; font-size:6pt ";
        if (FtSize == "7")
            sFontSize += " ; font-size:7pt ";
        if (FtSize == "8")
            sFontSize += " ; font-size:8pt ";

        if (FtSize == "9")
            sFontSize += " ; font-size:9pt ";
        if (FtSize == "10")
            sFontSize += " ; font-size:10pt ";
        if (FtSize == "11")
            sFontSize += " ; font-size:11pt ";
        if (FtSize == "12")
            sFontSize += " ; font-size:12pt ";
        if (FtSize == "14")
            sFontSize += " ; font-size:14pt ";
        if (FtSize == "18")
            sFontSize += " ; font-size:18pt ";
        if (FtSize == "20")
            sFontSize += " ; font-size:20pt ";
        if (FtSize == "24")
            sFontSize += " ; font-size:24pt ";
        if (FtSize == "26")
            sFontSize += " ; font-size:26pt ";
        if (FtSize == "36")
            sFontSize += " ; font-size:36pt ";

        return sBegin + sFontSize;
    }

    protected void BindImageWithControl(object ImageData)
    {
        try
        {
            Stream strm;
            Object img = ImageData;
            String FileName = "BioChem.bmp";
            strm = new MemoryStream((byte[])img);
            byte[] buffer = new byte[strm.Length];
            int byteSeq = strm.Read(buffer, 0, System.Convert.ToInt32(strm.Length));
            FileStream fs = new FileStream(Server.MapPath("/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);
            fs.Write(buffer, 0, byteSeq);
            fs.Dispose();
        }
        catch (Exception Ex)
        {

            //lblmassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblmassage.Text = "Error: " + Ex.Message;
            Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
            objException.HandleException(Ex);
        }
    }

}
