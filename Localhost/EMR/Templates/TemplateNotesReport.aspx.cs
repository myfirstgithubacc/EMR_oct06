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

public partial class EMR_Templates_TemplateNotesReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();
    string sFontSize = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("/Login.aspx?Logout=1", false);

        if (!IsPostBack)
        {
            try
            {
                BindPdf();
            }
            catch
            {
            }
        }
    }

    private void BindPdf()
    {
        StringBuilder objStr = new StringBuilder();
        clsIVF objIVF = new clsIVF(sConString);
        StringBuilder sbHeader = new StringBuilder();
        clsParsePDF cPrint = new clsParsePDF();
        try
        {
            int MarginTop = 0;
            int MarginBottom = 0;
            int MarginLeft = 0;
            int MarginRight = 0;
            string PageSize = "A4";

            ViewState["ReportId"] = "5";
            ViewState["HeaderId"] = "25";

            DataSet ds = new DataSet();
            BaseC.clsEMR objEMR = new BaseC.clsEMR(sConString);

            ds = objEMR.getReportFormatDetails(common.myInt(Session["DoctorId"]));
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ViewState["ReportId"] = common.myStr(ds.Tables[0].Rows[0]["ReportId"]);
                        ViewState["ReportName"] = common.myStr(ds.Tables[0].Rows[0]["ReportName"]);
                        ViewState["HeaderId"] = common.myStr(ds.Tables[0].Rows[0]["HeaderId"]);
                    }
                }
            }

            getDoctorImage();

            objIVF.getReportSetupMargin(common.myInt(ViewState["ReportId"]), out MarginTop, out MarginBottom, out MarginLeft, out MarginRight, out PageSize);

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

            objStr.Append(PrintReport(false, common.myInt(Request.QueryString["TemplateId"]), common.myInt(ViewState["ReportId"])));

            cPrint.Size = iTextSharp.text.PageSize.GetRectangle(PageSize);

            cPrint.MarginTop = MarginTop;
            cPrint.MarginBottom = MarginBottom;
            cPrint.MarginLeft = MarginLeft;
            cPrint.MarginRight = MarginRight;

            cPrint.GetFontName();
            string strHTML = objStr.ToString();

            strHTML = common.removeUnusedHTML(strHTML);
            cPrint.Html = strHTML;

            if (common.myLen(Session["strPatientHeader"]) > 0)
            {
                sbHeader = sbHeader.Append(common.myStr(Session["strPatientHeader"]));
            }

            string strReportHeader = getReportHeader(common.myInt(ViewState["ReportId"])).ToString();

            if (common.myStr(common.GetFlagValueHospitalSetup(common.myInt(Session["HospitalLocationId"]),
                                                 common.myInt(Session["FacilityId"]), "ShowHeaderOnAllPagesOfOPSummary", sConString)).Equals("Y"))
            {
                cPrint.HeaderHtml = strReportHeader + sbHeader.ToString();
                cPrint.FirstPageHeaderHtml = strReportHeader + sbHeader.ToString();
            }
            else
            {
                cPrint.HeaderHtml = string.Empty;
                cPrint.FirstPageHeaderHtml = strReportHeader + sbHeader.ToString();
            }

            objStr = new StringBuilder();

            cPrint.FirstPageFooterRight = "PageNofN";
            cPrint.FooterRight = "PageNofN";

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
            m.Dispose();
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

    protected string GetFontFamily(string typ, DataRow item)
    {
        string FieldValue = "";
        string FontName = "";
        string sBegin = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FontName = fonts.GetFont("Name", common.myStr(item[typ + "FontStyle"]));
        ViewState["CurrentTemplateFontName"] = string.Empty;
        ViewState["CurrentTemplateFontName"] = FontName;
        if (FontName != "")
        {
            sBegin += " font-family: " + FontName + ";";

            //sBegin += " font-family: " + FontName + ", sans-serif;";
        }
        else
        {
            FieldValue = cd.GetHospitalDefaults("DefaultFontType", common.myInt(Session["HospitalLocationId"]).ToString());
            if (FieldValue != "")
            {
                FontName = fonts.GetFont("Name", FieldValue);
                if (FontName != "")
                {
                    sBegin += " font-family: " + FontName + ";";
                }
            }
        }

        return sBegin;
    }

    public string getDefaultFontSize()
    {
        string sFontSize = "";
        string FieldValue = "";
        ClinicDefaults cd = new ClinicDefaults(Page);
        BaseC.EMRMasters.Fonts fonts = new BaseC.EMRMasters.Fonts();
        FieldValue = cd.GetHospitalDefaults("DefaultFontSize", common.myInt(Session["HospitalLocationId"]).ToString());
        if (FieldValue != "")
        {
            sFontSize = fonts.GetFont("Size", FieldValue);
            if (sFontSize != "")
            {
                sFontSize = " font-size: " + sFontSize + ";";
            }
        }
        return sFontSize;
    }

    protected void MakeFontWithoutListStyle(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }

        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }
        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    private StringBuilder getReportHeader(int ReportId)
    {
        StringBuilder sb = new StringBuilder();
        string FileName = string.Empty;
        string SignImage = string.Empty;
        string strSingImagePath = string.Empty;

        string NABHFileName = string.Empty;
        string NABHSignImage = string.Empty;
        string NABHstrSingImagePath = string.Empty;
        try
        {
            DataSet ds = new DataSet();

            bool IsPrintHospitalHeader = false;
            clsIVF objivf = new clsIVF(sConString);
            ds = objivf.EditReportName(ReportId);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    IsPrintHospitalHeader = common.myBool(ds.Tables[0].Rows[0]["IsPrintHospitalHeader"]);
                }
            }

            ds = new DataSet();
            ds = objivf.getFacility(common.myInt(Session["FacilityId"]), common.myInt(Session["HospitalLocationID"]));

            sb.Append("<div>");

            if (IsPrintHospitalHeader)
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
                        //sb.Append("<td valign='top'>" + (File.Exists(NABHstrSingImagePath) ? NABHSignImage : string.Empty) + "</td>");
                        sb.Append("<td valign='top'>" + string.Empty + "</td>");
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

            sb.Append("<table border='0' width='100%' style='text-align:center;'  cellpadding='2' cellspacing='3' ><tr>");
            sb.Append("<td align=center><U>" + common.myStr(Request.QueryString["TemplateName"]) + "</U></td>");
            sb.Append("</tr></table></div>");

            return sb;
        }

        catch (Exception Ex)
        {
            objException.HandleException(Ex);
            sb = new StringBuilder();
            return sb;
        }
    }

    private void getDoctorImage()
    {
        BaseC.clsLISPhlebotomy lis = new BaseC.clsLISPhlebotomy(sConString);
        BaseC.User user = new BaseC.User(sConString);
        DataSet ds = new DataSet();
        int intCheckImage = 0; // Check image from signed note signature
        Stream strm;
        Object img;
        DateTime SignatureDate;
        String UserName = "", ShowSignatureDate = "", UserDoctorId = "";
        String SignImage = "", SignNote = "";
        String DivStartTag = "<div id='dvDoctorImage'>";
        String SignedDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
        StringBuilder strSQL = new StringBuilder();
        String strSingImagePath = "";
        String Education = string.Empty;
        String FileName = string.Empty;
        string strimgData = string.Empty;
        try
        {
            if (common.myInt(Session["DoctorID"]) > 0)
            {
                //ds = lis.getDoctorImageDetails(common.myInt(ViewState["DoctorId"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                //                                common.myInt(ViewState["EncounterId"]));
                ds = lis.getDoctorImageDetails(common.myInt(Session["DoctorID"]), common.myInt(Session["HospitalLocationId"]), common.myInt(Session["FacilityId"]),
                                              common.myInt(Session["EncounterId"]));
                if (ds.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[1].Rows[0] as DataRow;
                    if (common.myStr(dr["SignatureImage"]) != "")
                    {
                        SignedDate = common.myStr(dr["SignedDate"]);
                        FileName = common.myStr(dr["SignatureImageName"]);
                        ShowSignatureDate = " on " + SignedDate;
                        Education = common.myStr(dr["SignedProviderEducation"]);
                        img = dr["SignatureImage"];
                        UserName = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]);
                        Session["EmpName"] = common.myStr(ds.Tables[0].Rows[0]["EmployeeName"]).Trim();
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(ds.Tables[0].Rows[0]["DoctorId"]);

                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");
                        strm = new MemoryStream((byte[])img);
                        byte[] buffer = new byte[strm.Length];
                        int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                        FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                        fs.Write(buffer, 0, byteSeq);
                        fs.Dispose();
                        //    RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName.Trim() + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                        SignImage = "<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />";
                        strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                        intCheckImage = 1;
                        strimgData = common.myStr(dr["ImageId"]);
                        SignNote = "Electronically signed by " + UserName.Trim() + " " + Education.Trim() + " " + ShowSignatureDate.Trim() + "</div>";
                    }
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (intCheckImage == 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0] as DataRow;
                        img = dr["SignatureImage"];
                        FileName = common.myStr(dr["ImageType"]);
                        UserName = common.myStr(dr["EmployeeName"]);
                        Session["EmpName"] = common.myStr(dr["EmployeeName"]).Trim();
                        SignatureDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SignatureWithDateTime"].ToString());
                        UserDoctorId = common.myStr(dr["DoctorId"]);
                        ShowSignatureDate = " on " + SignatureDate.ToString("dd/MM/yyyy hh:mm tt");

                        if (common.myStr(dr["Education"]).Trim() != ""
                            && common.myStr(dr["Education"]).Trim() != "&nbsp;")
                        {
                            Education = common.myStr(dr["Education"]);
                        }
                        SignNote = "Electronically signed by " + UserName + " " + Education + " " + ShowSignatureDate + "</div>";

                        if (FileName != "")
                        {
                            //RTF1.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;
                            strimgData = common.myStr(dr["ImageId"]);
                        }
                        else if (common.myStr(dr["SignatureImage"]) != "")
                        {
                            strm = new MemoryStream((byte[])img);
                            byte[] buffer = new byte[strm.Length];
                            int byteSeq = strm.Read(buffer, 0, common.myInt(strm.Length));
                            FileStream fs = new FileStream(Server.MapPath("~/PatientDocuments/DoctorImages/" + FileName), FileMode.Create, FileAccess.Write);

                            fs.Write(buffer, 0, byteSeq);
                            fs.Dispose();
                            //  RTF.Snippets.Add("<img width='100px' height='80px' src='/PatientDocuments/DoctorImages/" + FileName + "' />", "<img src='/PatientDocuments/DoctorImages/" + FileName + "' />");
                            SignImage = "<img width='100px' height='80px' src='../PatientDocuments/DoctorImages/" + FileName + "' />";
                            strSingImagePath = Server.MapPath("~") + @"\PatientDocuments\DoctorImages\" + FileName;

                            strimgData = common.myStr(dr["ImageId"]);
                        }
                    }
                }
                if (File.Exists(strSingImagePath))
                {
                    hdnDoctorImage.Value = DivStartTag + "<table  border='0' cellpadding='0' cellspacing='0' style='font-size:10pt; font-family:Tahoma;'><tbody><tr><td align='right'>" + SignImage + "</td></tr></tbody></table><br />";
                }
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            lis = null;
            ds.Dispose();
            strm = null;
            img = null;
            UserName = null;
            ShowSignatureDate = null;
            UserDoctorId = null;
            SignImage = null;
            SignNote = null;
            DivStartTag = null;
            SignedDate = null;
            strSQL = null;
            strSingImagePath = null;
        }
    }

    protected string CreateString(DataSet objDs, int iRootId, string iRootName, string TabularType,
     string sectionId, string EntryType, int RecordId, string GroupingDate, bool IsConfidential)
    {
        DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        StringBuilder objStr = new StringBuilder();
        DataView objDv = new DataView();
        DataTable objDt = new DataTable();
        DataSet dsMain = new DataSet();
        StringBuilder objStrTmp = new StringBuilder();
        DataSet dsTabulerTemplate = new DataSet();
        try
        {
            if (objDs != null)
            {
                if (IsConfidential == false)
                {
                    #region Tabular
                    if (bool.Parse(TabularType) == true)
                    {
                        DataView dvFilter = new DataView(objDs.Tables[0]);
                        if (objDs.Tables[0].Rows.Count > 0)
                        {
                            string sBegin = string.Empty;
                            string sEnd = string.Empty;
                            dvFilter.Sort = "RowNum ASC";
                            if (GroupingDate != "")
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                            }
                            else
                            {
                                dvFilter.RowFilter = "ISNULL(RowCaptionName,'')='' AND RowNum > 2 AND RecordId<>0 AND RecordId= " + RecordId;
                            }
                            DataTable dtNewTable = dvFilter.ToTable();
                            if (dtNewTable.Rows.Count > 0)
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                StringBuilder sbCation = new StringBuilder();
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'  cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();
                                        for (int k = 0; k < column; k++)
                                        {
                                            sbCation.Append("<td>");
                                            sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                            sbCation.Append("</td>");
                                            count++;
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId;
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 1; i < ColumnCount + 1; i++)
                                            {
                                                if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td align='center' ><img  id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                                else
                                                {
                                                    if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                    }
                                                    else
                                                    {
                                                        sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        dt.Dispose();
                                        dvData.Dispose();
                                    }
                                    sbCation.Append("</table>");
                                }
                                objStr.Append(sbCation);
                                dsTabulerTemplate.Dispose();
                                sbCation = null;

                            }
                            else
                            {
                                DataView dvRowCaption = new DataView(objDs.Tables[0]);
                                if (GroupingDate != "")
                                {
                                    dvRowCaption.RowFilter = "GroupDate='" + GroupingDate + "' AND RecordId= " + RecordId;
                                }
                                else
                                {
                                    dvRowCaption.RowFilter = "RecordId= " + RecordId;
                                }
                                if (dvRowCaption.ToTable().Rows.Count > 0)
                                {
                                    StringBuilder sbCation = new StringBuilder();
                                    dvRowCaption.RowFilter = "RowNum>0";
                                    DataTable dt = dvRowCaption.ToTable();
                                    // dvRowCaption.Dispose();
                                    if (dt.Rows.Count > 0)
                                    {
                                        sbCation.Append("<br /><br /><table border='1' style='border-color:#000000; border:solid;  border-collapse:collapse; " + sFontSize + "'   cellspacing='3' ><tr align='center'>");
                                        DataView dvColumnCount = new DataView(objDs.Tables[1]);
                                        dvColumnCount.RowFilter = "SectionId=" + sectionId;

                                        int column = common.myInt(dvColumnCount.ToTable().Rows[0]["ColumnCount"]);
                                        int ColumnCount = 0;
                                        int count = 1;
                                        dvColumnCount.Dispose();

                                        for (int k = 0; k < column + 1; k++)
                                        {
                                            if (common.myStr(dt.Rows[0]["RowCaptionName"]) == ""
                                                && ColumnCount == 0)
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(" + ");
                                                sbCation.Append("</td>");
                                            }
                                            else
                                            {
                                                sbCation.Append("<td>");
                                                sbCation.Append(common.myStr(dt.Rows[0]["Col" + count]));
                                                sbCation.Append("</td>");
                                                count++;
                                            }
                                            ColumnCount++;
                                        }
                                        sbCation.Append("</tr>");

                                        DataView dvData = new DataView(dt);
                                        if (GroupingDate != "")
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0 AND GroupDate='" + GroupingDate + "'";
                                        }
                                        else
                                        {
                                            dvData.RowFilter = "RecordId=" + RecordId + " AND RowCaptionId>0";
                                        }

                                        for (int l = 1; l <= dvData.ToTable().Rows.Count; l++)
                                        {
                                            sbCation.Append("<tr>");
                                            for (int i = 0; i < ColumnCount; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + common.myStr(dvData.ToTable().Rows[l - 1]["RowCaptionName"]) + "</td>");
                                                }
                                                else
                                                {
                                                    if (dt.Rows[1]["Col" + i].ToString() == "IM")
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td align='center' ><img id='dvImageType' runat='server'  alt='Image' width='30px' heigth='30px' src='" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "' /></td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dvData.ToTable().Rows[l - 1]["Col" + i].ToString() != "")
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>" + dvData.ToTable().Rows[l - 1]["Col" + i].ToString() + "</td>");
                                                        }
                                                        else
                                                        {
                                                            sbCation.Append("<td style=' " + sFontSize + "' align='center'>&nbsp;</td>");
                                                        }
                                                    }
                                                }
                                            }
                                            sbCation.Append("</tr>");
                                        }
                                        sbCation.Append("</table>");
                                        dvData.Dispose();
                                    }
                                    objStr.Append(sbCation);
                                    dt.Dispose();
                                    sbCation = null;
                                }
                            }
                        }
                    }
                    #endregion
                    #region Non Tabular
                    else // For Non Tabular Templates
                    {
                        string BeginList = "", EndList = "";
                        string sBegin = "", sEnd = "";
                        int t = 0;
                        string FieldId = "";
                        string sStaticTemplate = "";
                        string sEnterBy = "";
                        string sVisitDate = "";
                        foreach (DataRow item in objDs.Tables[0].Rows)
                        {
                            objDv = new DataView(objDs.Tables[1]);
                            objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                            objDt = objDv.ToTable();
                            if (t == 0)
                            {
                                t = 1;
                                if (common.myStr(item["FieldsListStyle"]) == "1")
                                {
                                    BeginList = "<ul>"; EndList = "</ul>";
                                }
                                else if (item["FieldsListStyle"].ToString() == "2")
                                {
                                    BeginList = "<ol>"; EndList = "</ol>";
                                }
                            }
                            if (common.myStr(item["FieldsBold"]) != ""
                                || common.myStr(item["FieldsItalic"]) != ""
                                || common.myStr(item["FieldsUnderline"]) != ""
                                || common.myStr(item["FieldsFontSize"]) != ""
                                || common.myStr(item["FieldsForecolor"]) != ""
                                || common.myStr(item["FieldsListStyle"]) != "")
                            {
                                //rafat1
                                if (objDt.Rows.Count > 0)
                                {
                                    sBegin = "";
                                    sEnd = "";

                                    MakeFont("Fields", ref sBegin, ref sEnd, item);
                                    if (common.myBool(item["DisplayTitle"]))
                                    {
                                        // if (EntryType != "M")
                                        // {


                                        ////if (sBegin.StartsWith("<br/>"))
                                        ////{
                                        ////    if (sBegin.Length > 5)
                                        ////    {
                                        ////        sBegin = sBegin.Substring(5, sBegin.Length - 5);

                                        ////    }
                                        ////}

                                        objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        //}
                                        //else
                                        //{
                                        //objStr.Append(BeginList + sBegin + common.myStr(item["FieldName"]));
                                        //}
                                        // 28/08/2011
                                        //if (objDt.Rows.Count > 0)
                                        //{
                                        if (objStr.ToString() != "")
                                        {
                                            //  objStr.Append(sEnd + "</li>");
                                        }
                                        ViewState["sBegin"] = sBegin;
                                    }

                                    BeginList = "";
                                    sBegin = "";
                                    sEnd = "";

                                }

                            }
                            else
                            {
                                if (objDt.Rows.Count > 0)
                                {
                                    if (sStaticTemplate != "<br/><br/>")
                                    {
                                        objStr.Append(common.myStr(item["FieldName"]));
                                    }
                                }
                            }
                            if (objDs.Tables.Count > 1)
                            {

                                objDv = new DataView(objDs.Tables[1]);
                                objDv.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                objDt = objDv.ToTable();
                                DataView dvFieldType = new DataView(objDs.Tables[0]);
                                dvFieldType.RowFilter = "FieldId='" + common.myStr(item["FieldId"]) + "'";
                                DataTable dtFieldType = dvFieldType.ToTable("FieldType");
                                sBegin = "";
                                sEnd = "";

                                string sbeginTemp = string.Empty;
                                MakeFontWithoutBR("Fields", ref sBegin, ref sEnd, item);
                                // MakeFont("Fields", ref sBegin, ref sEnd, item);
                                for (int i = 0; i < objDv.ToTable().Rows.Count; i++)
                                {
                                    if (objDt.Rows.Count > 0)
                                    {

                                        sbeginTemp = common.myStr(ViewState["sBegin"]);
                                        if (sbeginTemp.StartsWith("<br/>"))
                                        {
                                            if (sbeginTemp.Length > 5)
                                            {
                                                sbeginTemp = sbeginTemp.Substring(0, 5);

                                                //objStrTmp.Append(sBegin + common.myStr(item["SectionName"]) + sEnd);
                                            }
                                        }



                                        string FType = common.myStr(dtFieldType.Rows[0]["FieldType"]);
                                        if (FType == "C")
                                        {
                                            FType = "C";
                                        }
                                        if (FType == "C" || FType == "D" || FType == "B" || FType == "R")
                                        {
                                            if (FType == "B")
                                            {

                                                objStr.Append(" : " + objDt.Rows[i]["TextValue"]);
                                                //objStr.Append("  " + objDt.Rows[i]["TextValue"]);
                                            }
                                            else
                                            {
                                                //////BindDataValue(objDs, objDt, objStr, i, FType) //comeented by niraj , create and added below overloading methd
                                                BindDataValue(objDs, objDt, objStr, i, FType, sBegin, sEnd);
                                            }
                                        }
                                        else if (FType == "T" || FType == "M" || FType == "S" || FType == "W")
                                        {
                                            if (common.myStr(ViewState["iTemplateId"]) != "163")
                                            {
                                                if (i == 0)
                                                {
                                                    if (FType == "W")
                                                    {
                                                        objStr.Append(sBegin + " <br /> " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    }
                                                    else if (FType == "M")
                                                    {
                                                        objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    }
                                                    else
                                                    {
                                                        objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    }

                                                }
                                                else
                                                {
                                                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //if (FType == "M" || FType == "W")
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                                                    //}
                                                    //else
                                                    //{
                                                    //    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);

                                                    //}

                                                }
                                            }
                                            else
                                            {
                                                if (i == 0)
                                                {
                                                    objStr.Append(": " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                                else
                                                {
                                                    objStr.Append(", " + common.myStr(objDt.Rows[i]["TextValue"]));
                                                }
                                            }
                                        }
                                        else if (FType == "L")
                                        {
                                            //objStr.Append(BindStaticTemplates(common.myInt(objDt.Rows[0]["StaticTemplateId"]), common.myInt(objDt.Rows[0]["FieldId"])));
                                        }
                                        else if (FType == "IM")
                                        {
                                            objStr.Append(BindNonTabularImageTypeFieldValueTemplates(objDt));
                                        }
                                        if (common.myStr(item["FieldsListStyle"]) == "")
                                        {
                                            if (ViewState["iTemplateId"].ToString() != "163")
                                            {
                                                if (FType != "C")
                                                {

                                                    if (common.myStr(objDt.Rows[i]["StaticTemplateId"]) == null || common.myStr(objDt.Rows[i]["StaticTemplateId"]) == string.Empty || common.myInt(objDt.Rows[i]["StaticTemplateId"]) == 0)
                                                    {

                                                    }
                                                    else
                                                    {
                                                        objStr.Append("<br />");

                                                    }

                                                }

                                            }
                                            else
                                            {
                                                if (FType != "C" && FType != "T")
                                                {
                                                    objStr.Append("<br />");
                                                }
                                            }
                                        }





                                    }
                                    sEnterBy = objDt.Rows[i]["EnterBy"].ToString();
                                    sVisitDate = objDt.Rows[i]["VisitDateTime"].ToString();
                                    //if (EntryType == "M" && sEnterBy != "" && sVisitDate != "")
                                    //{
                                    //    objStr.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style=' font-size:8pt;'>(Entered By: " + sEnterBy + " Date/Time: " + sVisitDate + ")</span>");
                                    //}
                                }
                                sBegin = "";
                                sEnd = "";
                                dvFieldType.Dispose();
                                dtFieldType.Dispose();

                                // Cmt 25/08/2011
                                //if (objDt.Rows.Count > 0)
                                //{
                                //    if (objStr.ToString() != "")
                                //        objStr.Append(sEnd + "</li>");
                                //}
                            }

                            //objStr.Append(" <span style=\" font-weight:bold; font-size:medium; color:Gray\">|</span> ");
                        }

                        if (objStr.ToString() != "")
                        {
                            objStr.Append(EndList);
                        }
                    }
                    #endregion
                }
                string sDisplayEnteredBy = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
                if ((sDisplayEnteredBy == "Y") || (sDisplayEnteredBy == "N" && common.myStr(HttpContext.Current.Session["OPIP"]) == "I"))
                {
                    if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == false)
                    {
                        DataView dvValues = new DataView(objDs.Tables[1]);
                        dvValues.RowFilter = "SectionId=" + common.myStr(sectionId);
                        if (dvValues.ToTable().Rows.Count > 0)
                        {
                            //objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");

                            if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                            {
                                // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; '>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " Date/Time: " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span><br/>");
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");

                            }
                            else
                            {
                                objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["VisitDateTime"]) + "</span></b>");
                            }
                        }
                        dvValues.Dispose();
                    }
                    else
                    {
                        if ((objStr.ToString() != "" || IsConfidential == true) && bool.Parse(TabularType) == true)
                        {
                            DataView dvValues = new DataView(objDs.Tables[0]);
                            dvValues.RowFilter = "SectionId=" + common.myStr(sectionId) + " AND RecordId=" + RecordId + " AND IsData='D'";
                            if (dvValues.ToTable().Rows.Count > 0)
                            {
                                // objStr.Append("<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span><br/>");
                                if (ViewState["CurrentTemplateFontName"] != null && !common.myStr(ViewState["CurrentTemplateFontName"]).Equals(string.Empty))
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: " + common.myStr(ViewState["CurrentTemplateFontName"]) + "; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span></b>");
                                }
                                else
                                {
                                    objStr.Append("<b><br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='font-family: Tahoma; font-size:8pt;'>Entered By: " + common.myStr(dvValues.ToTable().Rows[0]["EnterBy"]) + " on " + common.myStr(dvValues.ToTable().Rows[0]["EntryDate"]) + "</span></b>");
                                }
                            }
                            dvValues.Dispose();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

        }
        finally
        {
            objDv.Dispose();
            objDt.Dispose();
            dsMain.Dispose();
            objDs.Dispose();
            dsTabulerTemplate.Dispose();
        }
        return objStr.ToString();
    }

    protected void BindDataValue(DataSet objDs, DataTable objDt, StringBuilder objStr, int i, string FType, string sBegin, string sEnd)
    {
        if (i == 0)
        {
            objStr.Append(sBegin + " : " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
        }
        else
        {
            if (FType != "C")
            {
                objStr.Append(sBegin + ", " + sBegin + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
            }
            else
            {
                if (i == 0)
                {
                    objStr.Append(sBegin + " " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
                else if (i + 1 == objDs.Tables[1].Rows.Count)
                {
                    objStr.Append(sBegin + " and " + common.myStr(objDt.Rows[i]["TextValue"]) + "." + sEnd);
                }
                else
                {
                    objStr.Append(sBegin + ", " + common.myStr(objDt.Rows[i]["TextValue"]) + sEnd);
                }
            }
        }
        //}
    }

    private string BindNonTabularImageTypeFieldValueTemplates(DataTable dtIMTypeTemplate)
    {
        StringBuilder sb = new StringBuilder();
        if (dtIMTypeTemplate.Rows.Count > 0)
        {
            if (common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) != "")
            {
                sb.Append("<table id='dvImageType' runat='server'><tr><td>" + common.myStr(dtIMTypeTemplate.Rows[0]["TextValue"]) + "</td></tr><tr align='left'><td align='center'><img src='" + common.myStr(dtIMTypeTemplate.Rows[0]["ImagePath"]) + "' width='80px' height='80px' border='0' align='left' alt='Image' /></td></tr></table>");
            }
        }
        return sb.ToString();
    }

    protected void MakeFontWithoutBR(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            //if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            //{
            //    sBegin += "<br/>";
            //}
            //else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            //{
            //    sBegin += "; ";
            //}
            //else
            //{
            //    sBegin += "<br/>";
            //}
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }

    protected void bindData(DataSet dsDynamicTemplateData, string TemplateId, StringBuilder sb, string GroupingDate)
    {
        DataSet ds = new DataSet();
        DataSet dsAllNonTabularSectionDetails = new DataSet();
        DataSet dsAllTabularSectionDetails = new DataSet();
        DataSet dsAllFieldsDetails = new DataSet();

        DataTable dtFieldValue = new DataTable();
        DataTable dtEntry = new DataTable();
        DataTable dtFieldName = new DataTable();

        DataView dv = new DataView();
        DataView dv1 = new DataView();
        DataView dv2 = new DataView();

        DataRow dr3;

        StringBuilder objStrTmp = new StringBuilder();
        StringBuilder objStrSettings = new StringBuilder();
        StringBuilder str = new StringBuilder();
        string sEntryType = "V";
        string BeginList = string.Empty;
        string EndList = string.Empty;
        string BeginList2 = string.Empty;
        string BeginList3 = string.Empty;
        string EndList3 = string.Empty;
        string sBegin = string.Empty;
        string sEnd = string.Empty;

        int t = 0;
        int t2 = 0;
        int t3 = 0;
        int iRecordId = 0;
        DataView dvDyTable1 = new DataView();
        try
        {
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;

            t = 0;
            t2 = 0;
            t3 = 0;

            dvDyTable1 = new DataView(dsDynamicTemplateData.Tables[0]);
            DataView dvDyTable2 = new DataView(dsDynamicTemplateData.Tables[1]);
            DataView dvDyTable3 = new DataView(dsDynamicTemplateData.Tables[2]);

            dvDyTable1.ToTable().TableName = "TemplateSectionName";
            dvDyTable2.ToTable().TableName = "FieldName";
            dvDyTable3.ToTable().TableName = "PatientValue";
            dsAllNonTabularSectionDetails = new DataSet();
            if (dvDyTable3.ToTable().Rows.Count > 0)
            {
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable2.ToTable());
                dsAllNonTabularSectionDetails.Tables.Add(dvDyTable3.ToTable());
            }
            dvDyTable2.Dispose();
            dvDyTable3.Dispose();

            dsDynamicTemplateData.Dispose();

            #region Non Tabular
            if (dsAllNonTabularSectionDetails.Tables.Count > 0 && dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvNonTabular = new DataView(dvDyTable1.ToTable());
                dvNonTabular.RowFilter = "Tabular=0";
                if (dvNonTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvNonTabular.ToTable());//Section Name Table

                    dv = new DataView(dsAllNonTabularSectionDetails.Tables[1]);

                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvNonTabular.Dispose();

                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllNonTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllNonTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = "RecordId=" + common.myStr(dtEntry.Rows[it]["RecordId"]) + " AND SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllNonTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllNonTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllNonTabularSectionDetails.Tables[1].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dsAllNonTabularSectionDetails.Tables[0].Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();

                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                            str.Append("<br/> ");


                                            dr3 = null;
                                            dsAllNonTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();
                                            string sBreak = common.myBool(item["IsConfidential"]) == true ? "<br/>" : "";
                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        ////// objStrTmp.Append("<br />"); //code commented  for Examination (SectonName and fieldname getting extra space)
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (sBegin.Contains("<br/>"))
                                                        {
                                                            sBegin = sBegin.Remove(0, 5);
                                                            objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                        }
                                                        else
                                                        {
                                                            objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(sBreak + common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {

                                                            if (sBegin.StartsWith("<br/>"))
                                                            {
                                                                if (sBegin.Length > 5)
                                                                {

                                                                    //sBegin = sBegin.Remove(0, 5);
                                                                    //objStrTmp.Append(BeginList + sBegin + sBreak + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                                    sBegin = sBegin.Substring(5, sBegin.Length - 5);
                                                                    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);

                                                            }

                                                            //if (sBegin.Contains("<br/>"))
                                                            //{
                                                            //    sBegin = sBegin.Remove(0, 5);
                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}
                                                            //else
                                                            //{

                                                            //    objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                            //}

                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            //if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            //{
                                            iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                            ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            // }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Tabular
            DataView dvDyTable4 = new DataView(dsDynamicTemplateData.Tables[3]);
            DataView dvDyTable5 = new DataView(dsDynamicTemplateData.Tables[4]);
            DataView dvDyTable6 = new DataView(dsDynamicTemplateData.Tables[5]);

            dvDyTable4.ToTable().TableName = "TabularData";
            dvDyTable5.ToTable().TableName = "TabularColumnCount";
            dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";

            dsAllTabularSectionDetails = new DataSet();
            if (dvDyTable4.ToTable().Rows.Count > 0)
            {
                dsAllTabularSectionDetails.Tables.Add(dvDyTable4.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable5.ToTable());
                dsAllTabularSectionDetails.Tables.Add(dvDyTable6.ToTable());
            }

            dvDyTable4.Dispose();
            dvDyTable5.Dispose();



            if (dsAllTabularSectionDetails.Tables.Count > 0 && dsAllTabularSectionDetails.Tables[1].Rows.Count > 0)
            {
                DataView dvTabular = new DataView(dvDyTable1.ToTable());
                dvTabular.RowFilter = "Tabular=1";
                if (dvTabular.ToTable().Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(dvTabular.ToTable());//Section Name Table
                    dv = new DataView(dsAllTabularSectionDetails.Tables[0]);
                    dv.Sort = "RecordId DESC";
                    dtEntry = dv.ToTable(true, "RecordId");
                    iRecordId = 0;
                    dv.Dispose();
                    dvTabular.Dispose();
                    for (int it = 0; it < dtEntry.Rows.Count; it++)
                    {
                        if (common.myInt(dtEntry.Rows[it]["RecordId"]) != 0)
                        {
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                dv1 = new DataView(dsAllTabularSectionDetails.Tables[0]);
                                dv1.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                DataView dvFieldStyle = new DataView(dsAllTabularSectionDetails.Tables[2]);
                                dvFieldStyle.RowFilter = "SectionId=" + common.myStr(item["SectionId"]);
                                dtFieldName = dv1.ToTable();

                                if (dsAllTabularSectionDetails.Tables.Count > 1)
                                {
                                    dv2 = new DataView(dsAllTabularSectionDetails.Tables[1]);
                                    dv2.RowFilter = " SectionId=" + common.myStr(item["SectionId"]);
                                    dtFieldValue = dv2.ToTable();
                                    dv2.Dispose();
                                }

                                dsAllFieldsDetails = new DataSet();
                                dsAllFieldsDetails.Tables.Add(dtFieldName);
                                dsAllFieldsDetails.Tables.Add(dtFieldValue);

                                dsAllFieldsDetails.Tables.Add(dvDyTable6.ToTable());
                                dvDyTable6.Dispose();
                                dtFieldName.Dispose();
                                dtFieldValue.Dispose();
                                dv1.Dispose();

                                if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                {
                                    if (dsAllTabularSectionDetails.Tables.Count > 1)
                                    {
                                        if (dsAllTabularSectionDetails.Tables[0].Rows.Count > 0)
                                        {
                                            sBegin = string.Empty;
                                            sEnd = string.Empty;
                                            dr3 = dvFieldStyle.ToTable().Rows[0];
                                            getabulerFontSize("Fields", ref sBegin, ref sEnd, dr3);
                                            ViewState["iTemplateId"] = common.myInt(item["TemplateId"]);

                                            str = new StringBuilder();
                                            str.Append(CreateString(dsAllFieldsDetails, common.myInt(item["TemplateId"]), common.myStr(item["TemplateName"]),
                                                        common.myStr(item["Tabular"]), item["SectionId"].ToString(), common.myStr(item["EntryType"]),
                                                        common.myInt(dtEntry.Rows[it]["RecordId"]), GroupingDate, common.myBool(item["IsConfidential"])));

                                            str.Append("<br/> ");

                                            dr3 = null;
                                            dsAllTabularSectionDetails.Dispose();
                                            dsAllFieldsDetails.Dispose();

                                            if (common.myInt(ViewState["iPrevId"]).Equals(common.myInt(item["TemplateId"])))
                                            {
                                                if (iRecordId != common.myInt(dtEntry.Rows[it]["RecordId"]))
                                                {
                                                    if (sEntryType.Equals("M"))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (t2.Equals(0))
                                                {
                                                    if (t3.Equals(0))//Template
                                                    {
                                                        t3 = 1;
                                                        if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                        {
                                                            BeginList3 = "<ul>";
                                                            EndList3 = "</ul>";
                                                        }
                                                        else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                        {
                                                            BeginList3 = "<ol>";
                                                            EndList3 = "</ol>";
                                                        }
                                                    }
                                                }

                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))   //19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(BeginList3 + sBegin + common.myStr(item["SectionName"]) + sEnd); //    objStrTmp.Append("<br />" + BeginList3 + sBegin + item["SectionName"].ToString() + sEnd);
                                                        }
                                                    }
                                                    BeginList3 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))    //19June
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //objStrTmp.Append("<br />" + item["SectionName"].ToString());
                                                        }
                                                    }
                                                }

                                                if (!str.ToString().Trim().Equals(string.Empty))
                                                {
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                        || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                    {
                                                        objStrTmp.Append("<br />");
                                                    }
                                                    objStrTmp.Append(str.ToString());
                                                }
                                            }
                                            else
                                            {
                                                if (t.Equals(0))
                                                {
                                                    t = 1;
                                                    if (common.myInt(item["TemplateListStyle"]).Equals(1))
                                                    {
                                                        BeginList = "<ul>"; EndList = "</ul>";
                                                    }
                                                    else if (common.myInt(item["TemplateListStyle"]).Equals(2))
                                                    {
                                                        BeginList = "<ol>"; EndList = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["TemplateBold"]) != string.Empty
                                                    || common.myStr(item["TemplateItalic"]) != string.Empty
                                                    || common.myStr(item["TemplateUnderline"]) != string.Empty
                                                    || common.myStr(item["TemplateFontSize"]) != string.Empty
                                                    || common.myStr(item["TemplateForecolor"]) != string.Empty
                                                    || common.myStr(item["TemplateListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Template", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["TemplateDisplayTitle"]))
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty))
                                                        {
                                                            if (sBegin.Contains("<br/>"))
                                                            {
                                                                sBegin = sBegin.Remove(0, 5);
                                                                objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                            }
                                                            else
                                                            {
                                                                objStrTmp.Append(BeginList + sBegin + common.myStr(item["TemplateName"]) + sEnd + "<br/>");
                                                            }
                                                        }
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                    BeginList = string.Empty;
                                                }
                                                else
                                                {
                                                    if (common.myBool(item["TemplateDisplayTitle"]))
                                                    {
                                                        objStrTmp.Append(common.myStr(item["TemplateName"]));//Default Setting
                                                    }
                                                    if (sEntryType.Equals("M") && !str.ToString().Trim().Equals(string.Empty))
                                                    {
                                                        objStrTmp.Append("<br/>");
                                                    }
                                                }
                                                if (common.myInt(item["TemplateListStyle"]).Equals(3)
                                                    || common.myInt(item["TemplateListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(EndList);
                                                if (t2.Equals(0))
                                                {
                                                    t2 = 1;
                                                    if (common.myInt(item["SectionsListStyle"]).Equals(1))
                                                    {
                                                        BeginList2 = "<ul>";
                                                        EndList3 = "</ul>";
                                                    }
                                                    else if (common.myInt(item["SectionsListStyle"]).Equals(2))
                                                    {
                                                        BeginList2 = "<ol>";
                                                        EndList3 = "</ol>";
                                                    }
                                                }
                                                if (common.myStr(item["SectionsBold"]) != string.Empty
                                                    || common.myStr(item["SectionsItalic"]) != string.Empty
                                                    || common.myStr(item["SectionsUnderline"]) != string.Empty
                                                    || common.myStr(item["SectionsFontSize"]) != string.Empty
                                                    || common.myStr(item["SectionsForecolor"]) != string.Empty
                                                    || common.myStr(item["SectionsListStyle"]) != string.Empty)
                                                {
                                                    sBegin = string.Empty;
                                                    sEnd = string.Empty;
                                                    MakeFont("Sections", ref sBegin, ref sEnd, item);
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"])) // Comment On 19June2010 hit1
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(BeginList2 + sBegin + common.myStr(item["SectionName"]) + sEnd);
                                                        }
                                                    }
                                                    BeginList2 = string.Empty;
                                                }
                                                else
                                                {
                                                    if (Convert.ToBoolean(item["SectionDisplayTitle"]))// Comment ON 19June2010
                                                    {
                                                        if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                                        {
                                                            objStrTmp.Append(common.myStr(item["SectionName"])); //Comment On 19June2010
                                                        }
                                                    }
                                                }
                                                if (common.myInt(item["SectionsListStyle"]).Equals(3)
                                                    || common.myInt(item["SectionsListStyle"]).Equals(0))
                                                {
                                                    //objStrTmp.Append("<br />");
                                                }

                                                objStrTmp.Append(str.ToString());
                                            }
                                            if (!str.ToString().Trim().Equals(string.Empty)) //add 19June2010
                                            {
                                                iRecordId = common.myInt(dtEntry.Rows[it]["RecordId"]);
                                                ViewState["iPrevId"] = common.myInt(item["TemplateId"]);
                                            }
                                        }
                                        str = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            if (t2.Equals(1) && t3.Equals(1))
            {
                objStrTmp.Append(EndList3);
            }
            else
            {
                objStrTmp.Append(EndList);
            }
            if (GetPageProperty("1") != null)
            {
                objStrSettings.Append(objStrTmp.ToString());
                sb.Append(objStrSettings.ToString());
            }
            else
            {
                sb.Append(objStrTmp.ToString());
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            ds.Dispose();
            dsAllNonTabularSectionDetails.Dispose();
            dsAllTabularSectionDetails.Dispose();
            dsAllFieldsDetails.Dispose();

            dtFieldValue.Dispose();
            dtEntry.Dispose();
            dtFieldName.Dispose();
            dvDyTable1.Dispose();
            dv.Dispose();
            dv1.Dispose();
            dv2.Dispose();

            dr3 = null;

            objStrTmp = null;
            objStrSettings = null;

            sEntryType = string.Empty;
            BeginList = string.Empty;
            EndList = string.Empty;
            BeginList2 = string.Empty;
            BeginList3 = string.Empty;
            EndList3 = string.Empty;
            sBegin = string.Empty;
            sEnd = string.Empty;
        }
    }
    protected DataSet GetPageProperty(string iFormId)
    {
        Hashtable hstInput = new Hashtable();
        if (common.myInt(Session["HospitalLocationID"]) > 0 && iFormId != "")
        {
            if (Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"] == null)
            {
                DAL.DAL dl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
                hstInput.Add("@inyHospitalLocationId", common.myInt(Session["HospitalLocationID"]));
                hstInput.Add("@intFormId", iFormId);
                DataSet ds = null;//dl.FillDataSet(CommandType.StoredProcedure, "EMRGetFormPageSettingDetails", hstInput);
                //Cache.Insert(Session["HospitalLocationID"].ToString() + "_" + iFormId + "_FormPageSettings", ds, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                return ds;
            }
            else
            {
                DataSet objDs = (DataSet)Cache[common.myInt(Session["HospitalLocationID"]).ToString() + "_" + iFormId + "_FormPageSettings"];
                return objDs;
            }
        }
        return null;
    }

    protected string getabulerFontSize(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        sFontSize = string.Empty;

        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sFontSize += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sFontSize += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sFontSize += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sFontSize += GetFontFamily(typ, item);
            };

            if (common.myStr(item[typ + "Bold"]) == "True")
            {
                sFontSize += " font-weight: bold;";
            }
            if (common.myStr(item[typ + "Italic"]) == "True")
            {
                sFontSize += " font-style: italic;";
            }
            if (common.myStr(item[typ + "Underline"]) == "True")
            {
                sFontSize += " text-decoration: underline;";
            }
        }

        return sFontSize;
    }


    protected void MakeFont(string typ, ref string sBegin, ref string sEnd, DataRow item)
    {
        //string sBegin = "", sEnd = "";
        ArrayList aEnd = new ArrayList();
        if (common.myStr(item[typ + "ListStyle"]) == "1")
        {
            sBegin += "<li>";
            //aEnd.Add("</li>");
        }
        else if (common.myStr(item[typ + "ListStyle"]) == "2")
        {
            sBegin += "<li>";
            // aEnd.Add("</li>");
        }
        else
        {
            if (common.myStr(ViewState["iTemplateId"]) != "163" && typ != "Fields")
            {
                sBegin += "<br/>";
            }
            else if (common.myStr(ViewState["iTemplateId"]) == "163" && typ == "Fields")
            {
                sBegin += "; ";
            }
            else
            {
                sBegin += "<br/>";
                //// //sBegin += "<br/>";
            }
        }

        if (common.myStr(item[typ + "Forecolor"]) != ""
            || common.myStr(item[typ + "FontSize"]) != ""
            || common.myStr(item[typ + "FontStyle"]) != "")
        {
            sBegin += "<span style='";
            if (common.myStr(item[typ + "FontSize"]) != "")
            {
                sBegin += " font-size:" + item[typ + "FontSize"] + ";";
            }
            else
            {
                sBegin += getDefaultFontSize();
            }
            if (common.myStr(item[typ + "Forecolor"]) != "")
            {
                sBegin += " color: #" + item[typ + "Forecolor"] + ";";
            }
            if (common.myStr(item[typ + "FontStyle"]) != "")
            {
                sBegin += GetFontFamily(typ, item);
            }
        }
        if (common.myStr(item[typ + "Bold"]) == "True")
        {
            sBegin += " font-weight: bold;";
        }
        if (common.myStr(item[typ + "Italic"]) == "True")
        {
            sBegin += " font-style: italic;";
        }
        if (common.myStr(item[typ + "Underline"]) == "True")
        {
            sBegin += " text-decoration: underline;";
        }

        aEnd.Add("</span>");
        for (int i = aEnd.Count - 1; i >= 0; i--)
        {
            sEnd += aEnd[i];
        }
        //sEnd += "<br/>";
        if (sBegin != "")
        {
            sBegin += " '>";
        }
    }
    private string PrintReport(bool sign, int TemplateId, int ReportId)
    {
        string strDisplayEnteredByInCaseSheet = common.myStr(Session["DisplayEnteredByInCaseSheet"]);
        Session["DisplayEnteredByInCaseSheet"] = string.Empty;

        StringBuilder sb = new StringBuilder();
        StringBuilder sbTemplateStyle = new StringBuilder();
        StringBuilder TemplateString;
        DataSet ds = new DataSet();
        DataSet dsTemplateStyle = new DataSet();
        DataRow drTemplateStyle = null;
        DataTable dtTemplate = new DataTable();
        DataView dvDataFilter = new DataView();
        DataTable dtEncounter = new DataTable();

        string Templinespace = "";
        BindNotes bnotes = new BindNotes(sConString);
        BaseC.DiagnosisDA fun = new BaseC.DiagnosisDA(sConString);
        BaseC.clsEMR emr = new BaseC.clsEMR(sConString);
        StringBuilder sbTemp = new StringBuilder();
        bool bAllergyDisplay = false;
        bool bPatientBookingDisplay = false;

        clsIVF objIVF = new clsIVF(sConString);

        string strPatientHeader = objIVF.getCustomizedPatientReportHeader(common.myInt(ViewState["HeaderId"]));
        if (common.myLen(strPatientHeader).Equals(0))
        {
            Session["strPatientHeader"] = getIVFPatient().ToString();
        }
        else
        {
            Session["strPatientHeader"] = strPatientHeader;
        }

        string sTemplateName = common.myStr("ALL") == "ALL" ? "" : common.myStr("ALL");

        DataSet dsTemplateData = new DataSet();
        BindCaseSheet BindCaseSheet = new BindCaseSheet(sConString);

        try
        {
            string DoctorId = fun.GetDoctorId(common.myInt(Session["HospitalLocationID"]), Convert.ToInt16(common.myInt(Session["UserID"])));
            dsTemplateStyle = bnotes.GetTemplateStyle(common.myInt(Session["HospitalLocationId"]));

            dsTemplateData = emr.getEMRPrintTemplateMasterData(common.myInt(Session["HospitalLocationId"]),
                                    common.myInt(Session["FacilityId"]), common.myInt(Session["RegistrationId"]), common.myInt(Session["EncounterId"]),
                                    common.myDate(DateTime.Now.AddYears(-1)).ToString("yyyy/MM/dd"),
                                    common.myDate(DateTime.Now).ToString("yyyy/MM/dd"),
                                    string.Empty, TemplateId, "D", false, 0);


            dvDataFilter = new DataView(dsTemplateData.Tables[21]);
            dtEncounter = dsTemplateData.Tables[22];
            for (int iEn = 0; iEn < dtEncounter.Rows.Count; iEn++)
            {
                if (dvDataFilter.ToTable().Rows.Count > 0)
                {
                    #region Template Wise
                    {
                        dtTemplate = dvDataFilter.ToTable();
                        TemplateString = new StringBuilder();
                        for (int i = 0; i < dtTemplate.Rows.Count; i++)
                        {
                            if (common.myStr(dtTemplate.Rows[i]["TemplateId"]).Trim() != ""
                                && common.myStr(dtTemplate.Rows[i]["TemplateType"]).Trim() == "D"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "POC"
                                && common.myStr(dtTemplate.Rows[i]["TemplateCode"]).Trim() != "HIS")
                            {
                                DataView dv = new DataView(dsTemplateStyle.Tables[0]);
                                dv.RowFilter = "PageId=" + common.myInt(dtTemplate.Rows[i]["TemplateId"]).ToString();
                                if (dv.Count > 0)
                                {
                                    drTemplateStyle = dv[0].Row;
                                    string sBegin = "", sEnd = "";
                                    Templinespace = common.myStr(drTemplateStyle["TemplateSpaceNumber"]);
                                    MakeFontWithoutListStyle("Template", ref sBegin, ref sEnd, drTemplateStyle);
                                }
                                dv.Dispose();
                                sbTemp = new StringBuilder();
                                //if ((common.myInt(0) == 0)
                                //    || (common.myInt(0) == common.myInt(dtTemplate.Rows[i]["TemplateId"])))
                                //{
                                #region Assign Data and call all Dynamic Template Except Hostory and Plan of Care template

                                DataSet dsDymanicTemplateData = new DataSet();

                                DataView dvDyTable1 = new DataView(dsTemplateData.Tables[14]);
                                DataView dvDyTable2 = new DataView(dsTemplateData.Tables[15]);
                                DataView dvDyTable3 = new DataView(dsTemplateData.Tables[16]);
                                DataView dvDyTable4 = new DataView(dsTemplateData.Tables[17]);
                                DataView dvDyTable5 = new DataView(dsTemplateData.Tables[18]);
                                DataView dvDyTable6 = new DataView(dsTemplateData.Tables[19]);
                                DataTable dtDyTempTable = new DataTable();

                                dvDyTable1.ToTable().TableName = "TemplateSectionName";
                                dvDyTable6.ToTable().TableName = "TabularTemplateFieldStyle";
                                if (common.myInt(0) == 0)
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(dtTemplate.Rows[i]["TemplateId"]);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                else
                                {
                                    dvDyTable1.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable4.RowFilter = "TemplateId=" + common.myStr(0);
                                    dvDyTable6.RowFilter = "TemplateId=" + common.myStr(0);
                                    dtDyTempTable = dvDyTable4.ToTable();
                                    dvDyTable4.Sort = "RecordId ASC";
                                }
                                string sSectionId = "0";
                                for (int iS = 0; iS < dvDyTable1.ToTable().Rows.Count; iS++)
                                {
                                    sSectionId = iS == 0 ? "'" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'"
                                        : sSectionId + ", '" + dvDyTable1.ToTable().Rows[iS]["SectionId"].ToString() + "'";
                                }
                                dvDyTable2.ToTable().TableName = "FieldName";
                                dvDyTable2.RowFilter = "SectionId IN (" + sSectionId + ")";
                                dvDyTable6.RowFilter = "SectionId IN (" + sSectionId + ")";

                                dvDyTable3.ToTable().TableName = "PatientValue";
                                if (dvDyTable3.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable3.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }
                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dvDyTable4.RowFilter = "SectionId IN (" + sSectionId + ") AND EncounterId=" + common.myInt(dtEncounter.Rows[iEn]["EncounterId"]);
                                }

                                dsDymanicTemplateData.Tables.Add(dvDyTable1.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable2.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable3.ToTable());


                                if (dvDyTable4.ToTable().Rows.Count > 0)
                                {
                                    dsDymanicTemplateData.Tables.Add(dtDyTempTable);
                                }
                                else
                                {
                                    dsDymanicTemplateData.Tables.Add(dvDyTable4.ToTable());
                                }
                                dsDymanicTemplateData.Tables.Add(dvDyTable5.ToTable());
                                dsDymanicTemplateData.Tables.Add(dvDyTable6.ToTable());
                                if (dsDymanicTemplateData.Tables[2].Rows.Count > 0 || dsDymanicTemplateData.Tables[3].Rows.Count > 0)
                                {
                                    bindData(dsDymanicTemplateData, common.myStr(dtTemplate.Rows[i]["TemplateId"]), sbTemp, "");
                                    if (sbTemp.Length > 20)
                                    {
                                        TemplateString.Append(sbTemp + "<br/>");
                                    }
                                }
                                sbTemp = null;
                                dvDyTable1.Dispose();
                                dvDyTable2.Dispose();
                                dvDyTable3.Dispose();
                                dvDyTable4.Dispose();
                                dvDyTable5.Dispose();
                                dvDyTable6.Dispose();
                                dtDyTempTable.Dispose();
                                dsDymanicTemplateData.Dispose();
                                sSectionId = "";
                                #endregion
                                //}

                                Templinespace = "";
                            }
                        }
                        if (TemplateString.Length > 30)
                        {
                            //if (iEn == 0)
                            //{
                            //sb.Append("<span style='font-size:20px; font-family:Tohama;'>");
                            //sb.Append("<b><u>Initial Assessment</u></b><br/><br/>");
                            //sb.Append("</span>");
                            //}
                            sb.Append("<span style='" + String.Empty + "'>");
                            sb.Append(TemplateString);
                            //sb.Append("</span><br/>");
                            sb.Append("</span>");
                            TemplateString = null;
                        }
                    }
                    #endregion
                }
            }
            Session["NoAllergyDisplay"] = null;
            if (sign == true)
            {
                //sb.Append("</span>");
                sb.Append(hdnDoctorImage.Value);
            }
        }
        catch
        {
        }
        finally
        {
            Session["DisplayEnteredByInCaseSheet"] = strDisplayEnteredByInCaseSheet;

            sbTemplateStyle = null;
            TemplateString = null;
            ds.Dispose();
            dsTemplateStyle.Dispose();
            dvDataFilter.Dispose();
            drTemplateStyle = null;
            dtTemplate.Dispose();
            Templinespace = "";
            bnotes = null;
            fun = null;
            emr = null;
            sbTemp = null;
            BindCaseSheet = null;
            dsTemplateData.Dispose();
        }
        return sb.ToString();
    }

}