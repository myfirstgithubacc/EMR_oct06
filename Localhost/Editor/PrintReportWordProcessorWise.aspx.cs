using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Editor_WordProcessorWisePrintReport : System.Web.UI.Page
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
            if (Convert.ToString(Session["PrintReportWordProcessorWiseData"]) != "")
            {
                string strRtfWord = Convert.ToString(Session["PrintReportWordProcessorWiseData"]);
                BindPdf(strRtfWord);
            }
            else
            {
                Response.Write("Sorry ! Data not available.");
                return;
            }
        }
    }

    private void BindPdf(string strRtfWord)
    {
        StringBuilder objStr = new StringBuilder();
        clsIVF objIVF = new clsIVF(sConString);
        StringBuilder sbHeader = new StringBuilder();
        clsParsePDF cPrint = new clsParsePDF();
        try
        {
            if (Session["PrintReportWordProcessorWiseData"] != null)
            {
                //string strPatientHeader = objIVF.getCustomizedPatientReportHeader(common.myInt(Request.QueryString["HeaderId"]));
                //if (common.myLen(strPatientHeader).Equals(0))
                //{
                //    sbHeader.Append(getIVFPatient().ToString());
                //}
                //else
                //{
                //    sbHeader.Append(strPatientHeader);
                //}

                int MarginTop = 0;
                int MarginBottom = 0;
                int MarginLeft = 0;
                int MarginRight = 0;
                string PageSize = "A4";

                objIVF.getReportSetupMargin(common.myInt(Request.QueryString["ReportId"]), out MarginTop, out MarginBottom, out MarginLeft, out MarginRight, out PageSize);

                if (MarginTop.Equals(0))
                {
                    MarginTop = 75;
                }
                if (MarginBottom.Equals(0))
                {
                    MarginBottom = 30;
                }
                if (MarginLeft.Equals(0))
                {
                    MarginLeft = 20;
                }
                if (MarginRight.Equals(0))
                {
                    MarginRight = 20;
                }
                if (common.myLen(PageSize).Equals(0))
                {
                    PageSize = "A4";
                }

                objStr.Append(strRtfWord);

                cPrint.Size = iTextSharp.text.PageSize.GetRectangle(PageSize);

                cPrint.MarginTop = MarginTop;
                cPrint.MarginBottom = MarginBottom;
                cPrint.MarginLeft = MarginLeft;
                cPrint.MarginRight = MarginRight;

                ////remove signature image and sing note
                // string WordProcessorWiseData = strRtfWord.ToString();
                cPrint.GetFontName();
                string strHTML = objStr.ToString();

                strHTML = common.removeUnusedHTML(strHTML);
                cPrint.Html = strHTML;

                // WordProcessorWiseData.Content;

                //cPrint.FirstPageHeaderHtml = sbHeader.ToString(); // objStr.ToString();
                //cPrint.HeaderHtml = sbHeader.ToString();


                //if (common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                //                                     common.myInt(Session["FacilityId"]), "ShowHeaderOnAllPagesOfOPSummary", sConString)).Equals("Y"))
                //{
                //    cPrint.HeaderHtml = sbHeader.ToString();
                //    cPrint.FirstPageHeaderHtml = string.Empty;
                //}
                //else
                //{
                //    cPrint.HeaderHtml = string.Empty;
                //    cPrint.FirstPageHeaderHtml = sbHeader.ToString();
                //}

                if (common.myStr(Session["strPatientHeader"]) != null && !common.myStr(Session["strPatientHeader"]).Equals(string.Empty))
                {
                    sbHeader = sbHeader.Append(common.myStr(Session["strPatientHeader"]));
                }

                if (common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                     common.myInt(Session["FacilityId"]), "ShowHeaderOnAllPagesOfOPSummary", sConString)).Equals("Y"))
                {
                    cPrint.HeaderHtml = sbHeader.ToString();
                    cPrint.FirstPageHeaderHtml = sbHeader.ToString();
                }
                else
                {
                    cPrint.HeaderHtml = string.Empty;
                    cPrint.FirstPageHeaderHtml = sbHeader.ToString();
                }

                //cPrint.HeaderHtml = string.Empty;
                //cPrint.FirstPageHeaderHtml = sbHeader.ToString();

                #region Header Part
                //cPrint.FooterLeft = "1";
                //cPrint.FirstPageFooterLeft = "1";
                //cPrint.FooterMiddle = "1";
                //cPrint.FirstPageFooterMiddle = "1";
                //cPrint.FooterRight = "1";
                //cPrint.FirstPageFooterRight = "1";

                objStr = new StringBuilder();
                #endregion

                cPrint.FirstPageFooterRight = "PageNofN";
                cPrint.FooterRight = "PageNofN";
                
                //cPrint.HeaderHtml = string.Empty;// objStr.ToString();
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
                //    Response.End();
                m.Dispose();
            }
        }
        catch (Exception Ex)
        {

            //lblmassage.ForeColor = System.Drawing.Color.FromName(commonLabelSetting.cErrorColor);
            //lblmassage.Text = "Error: " + Ex.Message;
            Alert.ShowAjaxMsg("Error: " + Ex.Message, Page);
            objException.HandleException(Ex);
        }
        finally
        {
            objStr = null;
            objIVF = null;
            sbHeader = null;
            cPrint = null;
        }
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
                int RegistrationId = Convert.ToInt32(Request.QueryString["RegistrationId"]);
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
                        sb.Append("<hr width=\"100%\" />");
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
                                                sb.Append("<td valign='top'  " + Ft + "'><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></td>");
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
                                            if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                            {
                                                sb.Append("<td valign='top'  " + Ft + "'><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></td>");
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

                        sb.Append("</table>");
                        sb.Append("<hr width=\"100%\" /><br />");
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

    private StringBuilder getIVFPatient()
    {
        StringBuilder sb = new StringBuilder();
        DataSet ds = new DataSet();

        clsIVF objivf = new clsIVF(sConString);

        ds = objivf.getIVFPatient(common.myInt(Request.QueryString["RegistrationId"]), 0);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataView DV = ds.Tables[0].Copy().DefaultView;
            DV.RowFilter = "RegistrationId=" + common.myInt(Request.QueryString["RegistrationId"]);

            DataTable tbl = DV.ToTable();

            if (tbl.Rows.Count > 0)
            {
                DataRow DR = tbl.Rows[0];

                DataView DVSpouse = ds.Tables[0].Copy().DefaultView;
                DVSpouse.RowFilter = "RegistrationId<>" + common.myInt(Request.QueryString["RegistrationId"]);
                DataTable tblSpouse = DVSpouse.ToTable();

                sb.Append("<div><table border='0' width='100%' style='font-size:smaller; border-collapse:collapse;' cellpadding='2' cellspacing='3' ><tr valign='top'>");
                //sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "ivfno")) + "</td><td>: " + common.myStr(Session["IVFNo"]) + "</td>");
                sb.Append("<td style='width: 72px;'>" + common.myStr(GetGlobalResourceObject("PRegistration", "regno")) + "</td><td>: " + common.myStr(DR["RegistrationNo"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "PatientName")) + "</td><td>: " + common.myStr(DR["PatientName"]) + "</td>");
                sb.Append("<td style='width: 109px;'>Age/Gender</td><td>: " + common.myStr(DR["Age/Gender"]) + "</td>");
                sb.Append("</tr>");

                if (tblSpouse.Rows.Count > 0)
                {
                    sb.Append("<tr valign='top'>");
                    sb.Append("<td>Spouse</td><td>: " + common.myStr(tblSpouse.Rows[0]["PatientName"]) + "</td>");
                    sb.Append("<td>Spouse Age/Gender</td><td>: " + common.myStr(tblSpouse.Rows[0]["Age/Gender"]) + "</td>");
                    sb.Append("</tr>");
                }

                sb.Append("<tr valign='top'>");
                sb.Append("<td>Reg. Date</td><td>: " + common.myStr(DR["RegistrationDate"]) + "</td>");
                sb.Append("<td>Occupation</td><td>: " + common.myStr(DR["Occupation"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "email")) + "</td><td>: " + common.myStr(DR["Email"]) + "</td>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "phone")) + "</td><td>: " + common.myStr(DR["PhoneHome"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("<tr valign='top'>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "Address")) + "</td><td>: " + common.myStr(DR["PatientAddress"]) + "</td>");
                sb.Append("<td>" + common.myStr(GetGlobalResourceObject("PRegistration", "mobile")) + "</td><td>: " + common.myStr(DR["MobileNo"]) + "</td>");
                sb.Append("</tr>");

                sb.Append("</table></div>");
            }

            sb.Append("<hr />");

        }
        return sb;
    }
}