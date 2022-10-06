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

public partial class LIS_Phlebotomy_PrintConsolidateLabReport : System.Web.UI.Page
{
    private string sConString = ConfigurationManager.ConnectionStrings["akl"].ConnectionString;
    clsExceptionLog objException = new clsExceptionLog();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
            Response.Redirect("/Login.aspx?Logout=1", false);
        if (!IsPostBack)
        {

            BindPdf(common.myStr(Session["RTF"]));
        }
    }
    private void BindPdf(string strRtfWord)
    {
        DAL.DAL objDl = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        Hashtable hstInput = new Hashtable();
        StringBuilder sQ = new StringBuilder();
        StringBuilder objStr = new StringBuilder();
        clsParsePDF cPrint = new clsParsePDF();
        double conversionUnit = 72;

        string ReportType = common.myStr(Request.QueryString["RT"]) == "CLR" ? "CLR" : "";

        DataSet objDs = new DataSet();
        try
        {
            hstInput.Add("HospitalLocationId", Session["HospitalLocationID"]);
            sQ.Append("SELECT PgSize, PgTopMargin, PgBottomMargin, PgLeftMargin, PgRightMargin, PgNoFormat, PgNoAllignment,Pg1HeaderId, Pg1HeaderMargin, Pg1HeaderNote, Pg2HeaderId, Pg2HeaderNote, PgFontType, PgFontSize, Pg1HeaderFontBold, Pg1HeaderFontItalic, Pg1HeaderFontUnderline, Pg1HeaderFontColor, Pg1HeaderFontType, Pg1HeaderFontSize, ");
            sQ.Append(" Pg2HeaderFontBold, Pg2HeaderFontItalic, Pg2HeaderFontUnderline, Pg2HeaderFontColor, Pg2HeaderFontType, Pg2HeaderFontSize, DisplayPgNoInPage1  ");
            sQ.Append(" FROM EMRForms WHERE Id = 1 AND HospitalLocationId  = @HospitalLocationId ");

            objDs = objDl.FillDataSet(CommandType.Text, sQ.ToString(), hstInput);
            if (objDs.Tables[0].Rows.Count > 0)
            {
                HeaderAndFooter(objDs.Tables[0], "Header", ReportType, objStr, "pg1");

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

                cPrint.GetFontName();

                string strHTML = strRtfWord.ToString();

                strHTML = common.removeUnusedHTML(strHTML);
                cPrint.Html = strHTML;

                cPrint.FirstPageHeaderHtml = objStr.ToString();

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
        catch (Exception Ex)
        {
            objException.HandleException(Ex);
        }
        finally
        {
            objDl = null;
            hstInput = null;
            sQ = null;
            objStr = null;
            cPrint = null;
            conversionUnit = 0;
            objDs.Dispose();
        }
    }
    protected void HeaderAndFooter(DataTable dt, string Type, string HeaderType, StringBuilder sb, string pg)
    {
        DAL.DAL DlObj = new DAL.DAL(DAL.DAL.DBType.SqlServer, sConString);
        DataSet ds = new DataSet();
        Hashtable hsSub = new Hashtable();
        DataView dvR = new DataView();
        DataTable dtR = new DataTable();
        DataTable dtFilter = new DataTable();

        try
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
                hsSub.Add("@chvReportType", HeaderType);
                hsSub.Add("@intEmployeeId", common.myInt(Session["DoctorId"]));
                hsSub.Add("@intRegistrationId", common.myInt(Session["RegistrationId"]));
                hsSub.Add("@intFacilityId", common.myInt(Session["FacilityId"]));
                hsSub.Add("@intEncounterId", common.myInt(Session["EncounterId"]));
                ds = DlObj.FillDataSet(CommandType.StoredProcedure, "uspGetDataObjectDetails", hsSub);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ShowBorder"].ToString().Trim() == "True")
                    {
                        sb.Append("<hr width=\"100%\" />");
                        sb.Append("<table style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");//</font></td></tr>");
                        int cc = 0;
                        dvR = new DataView(ds.Tables[0]);
                        dtFilter = dvR.ToTable(true, "RowNo");
                        for (int r = 1; r <= dtFilter.Rows.Count; r++)
                        {
                            dvR.RowFilter = "RowNo=" + r.ToString();
                            dtR = dvR.ToTable();
                            if (dtR.Rows.Count != 0)
                            {
                                int countRow = 0;
                                sb.Append("<tr >");
                                for (int c = 1; c <= dtR.Rows.Count; c++)
                                {
                                    if (dr[pg + "HeaderFontUnderline"].ToString() == "True")
                                    {
                                        if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                        {
                                            sb.Append("<td ' align='left'  " + Ft + "'><b><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></b></td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td  align='left'  " + Ft + "'><b><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></b></td>");
                                        }
                                    }
                                    else
                                    {
                                        countRow++;
                                        if (ds.Tables[0].Rows[cc]["FieldCaption"].ToString() != "")
                                        {
                                            sb.Append("<td  align='left' " + Ft + "' ><b><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " : " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></b></td>");
                                        }
                                        else
                                        {
                                            sb.Append("<td  align='left'  " + Ft + "' ><b><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font>/<b></td>");
                                        }
                                    }
                                    cc++;
                                }
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                sb.Append("<table>");
                            }
                        }
                        sb.Append("</table>");
                        sb.Append("<hr width=\"100%\" />");
                    }
                    else
                    {
                        sb.Append("<table style='" + Fonts + "; border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'>");//</font></td></tr>");
                        int cc = 0;
                        for (int r = 1; r <= 8; r++)
                        {
                            dvR = new DataView(ds.Tables[0]);
                            dvR.RowFilter = "RowNo=" + r.ToString();
                            dtR = dvR.ToTable();
                            if (dtR.Rows.Count != 0)
                            {
                                sb.Append("<tr align='left'>");
                                for (int c = 1; c <= dtR.Rows.Count; c++)
                                {
                                    if (dr[pg + "HeaderFontUnderline"].ToString() == "True")
                                    {
                                        if (!Convert.ToBoolean(ds.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            sb.Append("<td align='left'  " + Ft + "'><b><u><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></b></td>");
                                        }
                                    }
                                    else
                                    {
                                        if (!Convert.ToBoolean(ds.Tables[0].Rows[cc]["IsBinary"]))
                                        {
                                            sb.Append("<td align='left'  " + Ft + "'><b><font color='" + color + "'>" + ds.Tables[0].Rows[cc]["FieldCaption"].ToString() + " - " + ds.Tables[0].Rows[cc]["Value"].ToString() + "</font></u></b></td>");
                                        }
                                    }
                                    cc++;
                                }
                                sb.Append("</tr>");
                                // sb.Append("</table>");
                                // sb.Append("<table>");
                            }
                        }
                        sb.Append("</table>");



                    }

                    //sb.Append("<hr width=\"100%\" /><table width='100%' style='border-collapse:collapse;' border='0' cellpadding='0' cellspacing='0'><tr>");
                    //sb.Append("<td width='60%' align='left'  " + Ft + "'><b><font color='" + color + "'>Service Name</font></b></td>");
                    //sb.Append("<td  align='left'  " + Ft + "'><b><font color='" + color + "'>Result</font></b></td>");
                    //sb.Append("<td align='left'  " + Ft + "'><b><font color='" + color + "'>Unit</font></b></td>");
                    //sb.Append("<td align='left'  " + Ft + "'><b><font color='" + color + "'>Reference Range</font></b></td>");
                    //sb.Append("</tr></table><hr width=\"100%\"/>");
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            DlObj = null;
            ds.Dispose();
            hsSub = null;
            dvR.Dispose();
            dtR.Dispose();
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


        if (FtSize == "9")
            sFontSize += " ; font-size:9pt ";
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
}
